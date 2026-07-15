using System;
using System.Collections.Generic;
using System.Linq;
using Concord.AttachedData;

namespace Concord;

public class PropertyRegistry
{
    private readonly List<PropertyEntry> entries = new();
    private readonly HashSet<string> keys = new();
    private readonly Dictionary<Type, PropertyEntry[]> byType = new();

    public bool IsEmpty => entries.Count == 0;

    public void Add(Type baseType, string key, Type valueType, Func<object, bool> validate) {
        if (IsBclType(baseType)) {
            throw new ArgumentException("Attached properties cannot target BCL types: " + baseType.FullName, nameof(baseType));
        }

        if (!IsSupportedValueType(valueType)) {
            throw new ArgumentException("Attached-property type is not supported for save/load: " + valueType.FullName, nameof(valueType));
        }

        string composite = baseType.FullName + "::" + key;
        if (!keys.Add(composite)) {
            throw new InvalidOperationException("Duplicate attached property key: " + composite);
        }

        entries.Add(new PropertyEntry(baseType, key, valueType, validate, new Slot(), "concord." + key));
        byType.Clear();
    }

    public IReadOnlyList<PropertyEntry> ForBaseType(Type type) {
        if (byType.TryGetValue(type, out PropertyEntry[] cached)) {
            return cached;
        }

        PropertyEntry[] array = entries.Where(entry => entry.BaseType.IsAssignableFrom(type)).ToArray();
        byType[type] = array;
        return array;
    }

    private static bool IsSupportedValueType(Type valueType) {
        return valueType == typeof(int) || valueType == typeof(bool) || valueType == typeof(float) || valueType == typeof(string) || valueType == typeof(long);
    }

    // Might consider adding, but it restricts modders who may benefit otherwise without needing saving
    private static bool IsBaseSystemDataType(Type baseType)
    {
        return baseType.IsAssignableFrom(typeof(BaseSystemData));
    }
    
    private static bool IsBclType(Type type) {
        string ns = type.Namespace;
        if (ns == null) {
            return false;
        }

        return ns == "System" || ns.StartsWith("System.", StringComparison.Ordinal);
    }

    private sealed class Slot : IAttachedSlot {
        private readonly AttachedField<object, object> field = new AttachedField<object, object>();

        public object Get(object target) {
            return field.Get(target);
        }

        public void Set(object target, object value) {
            field.Set(target, value);
        }
    }
}