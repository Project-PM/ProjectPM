using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>() ?? go.AddComponent<T>();
        return component;
    }

    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }

    public static T ParseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    public static T ToObject<T>(this IDictionary<string, object> source)
        where T : class, new()
    {
        var someObject = new T();
        var someObjectType = someObject.GetType();

        foreach (var item in source)
        {
            someObjectType
                     .GetProperty(item.Key)
                     .SetValue(someObject, item.Value, null);
        }

        return someObject;
    }

    public static IDictionary<string, object> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
    {
        return source.GetType().GetProperties(bindingAttr).ToDictionary
        (
            propInfo => propInfo.Name,
            propInfo => propInfo.GetValue(source, null)
        );

    }
}
