using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace LitteraCore.Common
{
    public class PagedResult<T>
    {
        public IEnumerable<T>? Items { get; set; }
        public int TotalRecords { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public List<T> pagedata { get; private set; }
      
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        public static PagedList<T> ToPagedList(List<T> source, int pageNumber, int pageSize)
        {
            if (pageSize == 0)
            {
                pageSize = source.Count();
            }
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }



     
    }

    public class Paging
    {
        public static PagedList<T> GetPagedList<T>(PaginationParam param, List<T> items)
        {
            if (param != null)
            {
                if (param.PageSize > 0)
                {
                    return PagedList<T>.ToPagedList(items.ToList(),
                        param.PageNumber,
                        param.PageSize);
                }
                else
                {
                    return PagedList<T>.ToPagedList(items.ToList(),
                        1,
                        items.Count());
                }
            }
            else
            {
                return PagedList<T>.ToPagedList(items.ToList(),
                    1,
                    items.Count());
            }
        }

        public static PagedResult<T> GetPagedData<T>(PaginationParam param, List<T> items)
        {
            var pagedList = Paging.GetPagedList(param, items);
            if (pagedList.Count() > 0)
            {
                var metadata = new
                {
                    pagedList.TotalCount,
                    pagedList.PageSize,
                    pagedList.CurrentPage,
                    pagedList.TotalPages,
                    pagedList.HasNext,
                    pagedList.HasPrevious,
                };

                return new PagedResult<T>
                {
                    Items = pagedList,
                    TotalRecords = pagedList.TotalCount,
                    PageSize = pagedList.PageSize,
                    TotalPages = (int)Math.Ceiling((double)pagedList.TotalCount / pagedList.PageSize),
                    CurrentPage = pagedList.CurrentPage
                };

                // return Ok(pagedList);
            }
            else
            {
                return new PagedResult<T>
                {
                    Items = pagedList,
                    TotalRecords = pagedList.TotalCount,
                    PageSize = pagedList.PageSize,
                    TotalPages = (int)Math.Ceiling((double)pagedList.TotalCount / pagedList.PageSize),
                    CurrentPage = pagedList.CurrentPage
                };
            }
        }
    }
   
}
