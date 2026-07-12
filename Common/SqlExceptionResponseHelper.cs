using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace LitteraCore.Common
{
    public static class SqlExceptionResponseHelper
    {
        public static BadRequestObjectResult CreateBadRequest(SqlException ex)
        {
            return new BadRequestObjectResult(new
            {
                success = false,
                errorCode = "SQL_ERROR",
                message = ExtractMessage(ex)
            });
        }

        private static string ExtractMessage(SqlException ex)
        {
            if (ex?.Errors != null && ex.Errors.Count > 0)
            {
                for (int i = ex.Errors.Count - 1; i >= 0; i--)
                {
                    string message = ex.Errors[i].Message?.Trim();
                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        return message;
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(ex?.Message))
            {
                return ex.Message.Trim();
            }

            return "A database error occurred while processing the request.";
        }
    }
}
