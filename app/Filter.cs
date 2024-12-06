using System.Reflection;

namespace app;

public static class Filter
{
    
    public static List<T> FilterObjectList<T>(List<T> objectsToSort, List<string> propertyNames, List<string> filterFor)
    {
        List<T> filteredList = new List<T>();
        if (objectsToSort.Count < 1 || objectsToSort[0] == null) return objectsToSort;
        List<PropertyInfo> properties = new List<PropertyInfo>();
        foreach (var propertyName in propertyNames)
        {
            properties.Add(objectsToSort[0]?.GetType().GetProperty(propertyName));
        }

        foreach (var property in properties)
        {
            foreach (var filter in filterFor)
            {
                filteredList.AddRange(objectsToSort.Where(obj => property.GetValue(obj)?.ToString() == filter));
            }
        }

        filteredList = filteredList.Distinct().ToList();
        return filteredList;
    }
}