using System.Linq.Expressions;
using System.Reflection;

namespace Reimbit.Infrastructure.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplySorting<T>(
        this IQueryable<T> query,
        string? sortField,
        string? sortOrder,
        string defaultField)
    {
        var propertyName = !string.IsNullOrWhiteSpace(sortField) ? sortField : defaultField;
        var isDescending = string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase);

        var propertyInfo = typeof(T).GetProperty(
            propertyName,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        if (propertyInfo == null)
        {
            propertyInfo = typeof(T).GetProperty(
                defaultField,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null) return query;
        }

        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyAccess = Expression.MakeMemberAccess(parameter, propertyInfo);
        var lambda = Expression.Lambda(propertyAccess, parameter);

        var methodName = isDescending ? nameof(Queryable.OrderByDescending) : nameof(Queryable.OrderBy);

        var resultExpression = Expression.Call(
            typeof(Queryable),
            methodName,
            new[] { typeof(T), propertyInfo.PropertyType },
            query.Expression,
            Expression.Quote(lambda)
        );

        return query.Provider.CreateQuery<T>(resultExpression);
    }
}