namespace Controller.Controller;

public static class RandomExtensions
{
    public static T? GetStaticProperty<T>(this Type type, string name)
        => (T?)type.GetProperty(name)?.GetValue(null);
}