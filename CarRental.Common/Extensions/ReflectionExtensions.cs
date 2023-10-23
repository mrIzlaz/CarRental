using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using CarRental.Common.Interfaces;

namespace CarRental.Common.Extensions;

public static class ReflectionExtensions
{
    public static FieldInfo[] GetVariables(this Type type) =>
        type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

    public static FieldInfo? FindCollection<T>(this FieldInfo[] fields) where T : class =>
        fields.FirstOrDefault(f => f.FieldType == typeof(List<T>) && f.IsInitOnly) ??
        throw new InvalidOperationException("Unsupported type");

    public static List<FieldInfo>? FindSearchableCollection<T>(this FieldInfo[] fields) where T : ISearchable =>
        fields.Where(f =>
                f.FieldType == typeof(List<T>) || f.FieldType.GetInterface("ISearchable") == typeof(T) ||
                f.FieldType.IsAssignableFrom(typeof(T)))
            .ToList() ??
        throw new InvalidOperationException("Unsupported type");

    public static object? GetData(this FieldInfo? field, object container) =>
        field?.GetValue(container) ?? throw new InvalidDataException();

    public static IQueryable<T> ToQueryable<T>(this object? data) where T : class => data is List<T> list
        ? list.AsQueryable()
        : throw new InvalidDataException();

    public static IEnumerable<T> ToQueryableSearchables<T>(this object? data) where T : ISearchable =>
        data is List<T> list
            ? list.AsQueryable()
            : throw new InvalidDataException();

    public static List<T> Filter<T>(this IQueryable<T> collection, Func<T, bool>? expression) =>
        expression is null ? collection.ToList() : collection.AsEnumerable().Where(expression).ToList();

    public static bool IsNotMatchingThis<T>(this T item, string prompt)
    {
        if (item is not ISearchable) return false; // Stödjer den ISearchable?

        var asdasdas = item.GetType();
        
        Type t = typeof(T);
        var test = t.GetProperties().Where(x => x.ToString().Contains(prompt));
        Type tasd = item.GetType();
        FieldInfo[] fieldinfo = tasd.GetFields(BindingFlags.DeclaredOnly);
        Console.WriteLine(tasd.ToString());

        for (int i = 0; i < fieldinfo.Length; i++)
        {
            Console.WriteLine("\nName            : {0}", fieldinfo[i].Name);
            Console.WriteLine("Declaring Type  : {0}", fieldinfo[i].DeclaringType);
            Console.WriteLine("IsPublic        : {0}", fieldinfo[i].IsPublic);
            Console.WriteLine("MemberType      : {0}", fieldinfo[i].MemberType);
            Console.WriteLine("FieldType       : {0}", fieldinfo[i].FieldType);
            Console.WriteLine("IsFamily        : {0}", fieldinfo[i].IsFamily);
        }


        foreach (var field in fieldinfo)
        {
            Console.WriteLine(field.GetValue(item).ToString());
        }

        var list = fieldinfo.FirstOrDefault(f => f.GetValue(item)!.ToString().Contains(prompt));
        return list != null;
    }
}