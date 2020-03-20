using NHibernate.Cfg;

namespace TgStickers.Infrastructure.NHibernate
{
    public class PostgresNamingStrategy : INamingStrategy
    {
        public string ClassToTableName(string className)
        {
            return ToDoubleQuote(className);
        }

        public string PropertyToColumnName(string propertyName)
        {
            return ToDoubleQuote(propertyName);
        }

        public string TableName(string tableName)
        {
            return ToDoubleQuote(tableName);
        }

        public string ColumnName(string columnName)
        {
            return ToDoubleQuote(columnName);
        }

        public string PropertyToTableName(string className, string propertyName)
        {
            return ToDoubleQuote(propertyName);
        }

        public string LogicalColumnName(string columnName, string propertyName)
        {
            return string.IsNullOrWhiteSpace(columnName)
                ? ToDoubleQuote(propertyName)
                : ToDoubleQuote(columnName);
        }

        private string ToDoubleQuote(string name)
        {
            return $"\"{name.Replace("`", "")}\"";
        }
    }
}
