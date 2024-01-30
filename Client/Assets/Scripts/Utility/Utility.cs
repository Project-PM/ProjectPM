using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonDataAttribute : ResourceAttribute
{
    public JsonDataAttribute(string path) : base("Datas/JsonData/" + path, ResourceType.json)
    {

    }
}

public class ResourceAttribute : Attribute
{
    public readonly string resourcePath = string.Empty;
    public readonly ResourceType resourceType = ResourceType.None;

    public ResourceAttribute(string path, ResourceType type)
    {
        this.resourcePath = "Assets/Bundle/" + path;
        this.resourceType = type;
    }
}

public static class Utility
{
    public static bool TryParse<TEnum>(string enumValue, out TEnum result) where TEnum : struct, Enum
    {
        if (Enum.TryParse(enumValue, out result))
        {
            return true;
        }

        return false;
    }

    public static IEnumerable<Type> GetSubClassTypes<T>()
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(T)))
                {
                    yield return type;
                }
            }
        }
    }

    public static string GetResourcePath<T>() where T : UnityEngine.Object
    {
        Type type = typeof(T);

        Attribute[] attributes = Attribute.GetCustomAttributes(type);

        foreach (Attribute attr in attributes)
        {
            ResourceAttribute resourceAttr = attr as ResourceAttribute;

            if (resourceAttr != null)
            {
                return resourceAttr.resourcePath;
            }
        }

        return string.Empty;
    }

    public static string GetResourcePath(Type type)
    {
        Attribute[] attributes = Attribute.GetCustomAttributes(type);

        foreach (Attribute attr in attributes)
        {
            ResourceAttribute resourceAttr = attr as ResourceAttribute;

            if (resourceAttr != null)
            {
                return resourceAttr.resourcePath;
            }
        }

        return string.Empty;
    }

    public static ResourceType GetResourceType<T>() where T : UnityEngine.Object
    {
        Type type = typeof(T);

        Attribute[] attributes = Attribute.GetCustomAttributes(type);

        foreach (Attribute attr in attributes)
        {
            ResourceAttribute resourceAttr = attr as ResourceAttribute;

            if (resourceAttr != null)
            {
                return resourceAttr.resourceType;
            }
        }

        return ResourceType.None;
    }

    public static ResourceType GetResourceType(Type type)
    {
        Attribute[] attributes = Attribute.GetCustomAttributes(type);

        foreach (Attribute attr in attributes)
        {
            ResourceAttribute resourceAttr = attr as ResourceAttribute;

            if (resourceAttr != null)
            {
                return resourceAttr.resourceType;
            }
        }

        return ResourceType.None;
    }

}