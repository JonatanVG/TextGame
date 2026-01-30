using System.Reflection;
using TextGame.Entities;

namespace TextGame.Utilites.Functions
{
  internal class ObjectPropertiesHandler
  {
    public static List<PropertyInfo> StatInfo()
    {
      return [.. typeof(PlayerStats)
        .GetProperties(BindingFlags.NonPublic | BindingFlags.Instance)
        .Where(x => x.Name.StartsWith("Stat"))];
    }

    public static PropertyInfo? GetObjectProperty(object obj, string propertyName)
    {
      var propertyInfo = obj.GetType().GetProperty(
        propertyName,
        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
      );
      if (propertyInfo != null)
      {
        Console.WriteLine($"Property '{propertyName}'");
        return propertyInfo!;
      }
      else
      {
        Console.WriteLine($"Property '{propertyName}' not found on object of type '{obj.GetType().Name}'.");
        return null;
      }
    }
  }
}
