using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModel.Finance;
using Microsoft.EntityFrameworkCore;

namespace AccuApp32MVC.Helper
{
    public static class FinanceScheme
    {
        public static FINANCEContext dbContext { get; set; }
        public static async Task<List<Field>> GetFields(string entityName)
        {
            List<Field> fields = new List<Field>();
            var entityType = dbContext.Model.FindEntityType(entityName);

            // Table info s
            var tableName = entityType.GetTableName();
            var tableSchema = entityType.GetSchema();

            // Column info 
            foreach (var property in entityType.GetProperties())
            {
                var columnName = property.GetColumnName();
                var columnType = property.GetColumnType();
                fields.Add(new Field
                {
                    ColumnName = columnName,
                    ColumnType = (columnType.Contains("char") || columnType.Contains("text")) ? "text" : ((columnType.Contains("money") || columnType.Contains("decimal") || columnType.Contains("float") || columnType.Contains("int")) ? "numeric" : (columnType.Contains("date") ? "date" : "unknown")),
                    TableName = tableName
                });
            };
            return fields;
        }
    }
    public class Field
    {
        public string ColumnName { get; set; }
        public string ColumnType { get; set; }
        public string TableName { get; set; }

    }
}
