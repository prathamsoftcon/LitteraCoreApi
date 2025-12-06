using Microsoft.Data.SqlClient;

namespace LitteraCore.Common
{
    public static class SqlDataReaderExtensions
    {
        public static bool HasColumn(this SqlDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        public static string GetStringSafe(this SqlDataReader reader, string columnName)
        {
            return reader.HasColumn(columnName) && reader[columnName] != DBNull.Value
                ? Convert.ToString(reader[columnName])
                : "";
        }

        //public static int? GetIntSafe(this SqlDataReader reader, string columnName)
        //{
        //    return reader.HasColumn(columnName) && reader[columnName] != DBNull.Value &&
        //           int.TryParse(reader[columnName].ToString(), out int val)
        //        ? val
        //        : (int?)null;
        //}
        public static int GetIntSafe(this SqlDataReader reader, string columnName)
        {
            if (!reader.HasColumn(columnName) || reader[columnName] == DBNull.Value)
                return 0;

            return int.TryParse(reader[columnName].ToString(), out int val)
                ? val
                : 0;
        }

        public static decimal? GetDecimalSafe(this SqlDataReader reader, string columnName)
        {
            return reader.HasColumn(columnName) && reader[columnName] != DBNull.Value &&
                   decimal.TryParse(reader[columnName].ToString(), out decimal val)
                ? val
                : (decimal?)null;
        }

        public static DateTime? GetDateSafe(this SqlDataReader reader, string columnName)
        {
            return reader.HasColumn(columnName) && reader[columnName] != DBNull.Value &&
                   DateTime.TryParse(reader[columnName].ToString(), out DateTime val)
                ? val
                : (DateTime?)null;
        }
    }
}
