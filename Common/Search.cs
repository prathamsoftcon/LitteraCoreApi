using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace LitteraCore.Common
{
    public class SearchCriteria
    {
        public string? Column { get; set; }
        public string? Value { get; set; }
        public string? Condition { get; set; } // "=", "LIKE", ">", "<", etc.
        public string? NextOperator { get; set; } // "AND" or "OR"
    }
    public class SearchParam
    {
        public SearchCriteria[]? SearchCriteria { get; set; } 
    }
    public class SearchService
    {
        public List<T> FilterItems<T>(List<T> items, List<SearchCriteria> searchCriteria)
        {
            if (items == null || searchCriteria == null || !searchCriteria.Any())
            {
                return items;
            }

            var query = items.AsQueryable();
            var parameter = Expression.Parameter(typeof(T), "x");
            Expression combinedPredicate = Expression.Constant(true); // always true predicate to start with
            int index = 0;
            foreach (var criteria in searchCriteria)
            {
                var currentPredicate = CreatePredicate<T>(criteria, parameter);

                if (index == 0)
                {
                    combinedPredicate = currentPredicate;
                }
                else
                {
                    if (criteria.NextOperator.ToString().ToUpper() == "AND")
                    {
                        var nextOperator = criteria.NextOperator?.ToUpper() ?? "AND";
                        combinedPredicate = CombinePredicates(combinedPredicate, currentPredicate, nextOperator);
                    }
                    else
                    {
                        var nextOperator = criteria.NextOperator?.ToUpper() ?? "OR";
                        combinedPredicate = CombinePredicates(combinedPredicate, currentPredicate, nextOperator);
                    }
                   
                }
                index = index + 1;
            }

            var finalPredicate = Expression.Lambda<Func<T, bool>>(combinedPredicate, parameter);
            return query.Where(finalPredicate).ToList();
        }

        private Expression CreatePredicate<T>(SearchCriteria criteria, ParameterExpression parameter)
        {
            var column = criteria.Column;
            var value = criteria.Value;
            var condition = criteria.Condition.ToUpper();

            var property = Expression.Property(parameter, column);
            var propertyValue = Expression.Constant(Convert.ChangeType(value, property.Type), property.Type);

            Expression body = condition switch
            {
                "=" => Expression.Equal(property, propertyValue),
                "LIKE" => Expression.Call(property, typeof(string).GetMethod("Contains", new[] { typeof(string) }), propertyValue),
                ">" => Expression.GreaterThan(property, propertyValue),
                "<" => Expression.LessThan(property, propertyValue),
                _ => throw new NotSupportedException($"Condition '{condition}' is not supported.")
            };

            return body;
        }

        private Expression CombinePredicates(Expression left, Expression right, string nextOperator)
        {
            var operatorType = nextOperator.ToUpper();

            if (operatorType == "OR")
            {
                return Expression.OrElse(left, right);
            }
            else if (operatorType == "AND")
            {
                return Expression.AndAlso(left, right);
            }

            throw new NotSupportedException($"Operator '{nextOperator}' is not supported.");
        }
    }

    public class SearchParamModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var request = bindingContext.HttpContext.Request;

            if (request.ContentLength == 0)
            {
                bindingContext.Result = ModelBindingResult.Success(new SearchParam());
                return;
            }

            using (var reader = new StreamReader(request.Body))
            {
                var body = await reader.ReadToEndAsync();
                if (string.IsNullOrEmpty(body))
                {
                    bindingContext.Result = ModelBindingResult.Success(new SearchParam());
                }
                else
                {
                    var model = JsonSerializer.Deserialize<SearchParam>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    bindingContext.Result = ModelBindingResult.Success(model);
                }
            }
        }
    }
}
