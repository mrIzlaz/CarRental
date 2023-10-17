using System.Reflection;

namespace CarRental.Common.Extensions;

public static class ReflectionExtensions
{
    public static FieldInfo[] GetVariables(this Type type) =>
        type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

    public static FieldInfo? FindCollection<T>(this FieldInfo[] fields) where T : class =>
        fields.FirstOrDefault(f => f.FieldType == typeof(List<T>) && f.IsInitOnly) ??
        throw new InvalidOperationException("Unsupported type");

    public static object? GetData(this FieldInfo? field, object container) =>
        field?.GetValue(container) ?? throw new InvalidDataException();

    public static IQueryable<T> ToQueryable<T>(this object? data) where T : class => data is List<T> list
        ? list.AsQueryable()
        : throw new InvalidDataException();

    public static List<T> Filter<T>(this IQueryable<T> collection, Func<T, bool>? expression) =>
        expression is null ? collection.ToList() : collection.AsEnumerable().Where(expression).ToList();
}