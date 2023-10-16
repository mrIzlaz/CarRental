using System.Reflection;

namespace CarRental.Common.Extensions;

public static class ObjectExtension
{
    public static bool IsNullOrEmpty<T>(this List<T>? list)
    {
        return list == null || list.Count == 0;
    }

    public static bool IsNullOrEmpty<T>(this T? obj)
    {
        if (obj is null)
            return true;


        Type t = obj.GetType();

        if (t.IsArray)
            return false;
        if (t == typeof(string))
            return string.IsNullOrEmpty(obj as string);

        if (t == typeof(T[]))
        {
            var arr = obj as T[];
            return arr == null || arr.Length == 0;
        }

        return false;
    }

    public static bool IsNullOrEmpty<T>(this T? obj) where T : struct => !obj.HasValue;
}