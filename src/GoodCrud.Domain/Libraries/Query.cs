
using System;
using System.Linq;
using System.Linq.Expressions;

namespace GoodCrud.Domain.Libraries
{
    public class Query
    {

        public static Expression<Func<General, bool>> CreatePredicate<General>(string columnName, object searchValue)
        {
            var xType = typeof(General);
            var x = Expression.Parameter(xType, "x");
            var column = xType.GetProperties().FirstOrDefault(p => p.Name == columnName);

            var body = column == null
                ? (Expression)Expression.Constant(true)
                : Expression.Equal(
                    Expression.PropertyOrField(x, columnName),
                    Expression.Constant(searchValue));

            return Expression.Lambda<Func<General, bool>>(body, x);
        }
    }
}