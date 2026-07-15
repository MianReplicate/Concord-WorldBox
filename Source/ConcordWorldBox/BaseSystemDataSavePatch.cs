using System;
using System.Collections.Generic;

namespace Concord;

[Patch]
public class BaseSystemDataSavePatch : BaseSystemData
{
    [Inject(At.Head, nameof(save))]
    public void SaveAttachedProperties(ControlHandle ch)
    {
        PropertyRegistry registry = WorldBoxRuntime.Registry;
        if (registry == null || registry.IsEmpty) {
            return;
        }

        IReadOnlyList<PropertyEntry> entries = registry.ForBaseType(GetType());
        foreach (PropertyEntry entry in entries)
        {
            SaveOne(this, entry);
        }
    }
    
    [Inject(At.Head, nameof(load))]
    public void LoadAttachedProperties(ControlHandle ch)
    {
        PropertyRegistry registry = WorldBoxRuntime.Registry;
        if (registry == null || registry.IsEmpty) {
            return;
        }

        IReadOnlyList<PropertyEntry> entries = registry.ForBaseType(GetType());
        foreach (PropertyEntry entry in entries)
        {
            LoadOne(this, entry);
        }
    }

    private static void SaveOne(BaseSystemData self, PropertyEntry entry)
    {
        try
        {
            object current = entry.Slot.Get(self);
            switch (current)
            {
                case int i:
                    self.set(entry.SaveLabel, i);
                    break;
                case float f:
                    self.set(entry.SaveLabel, f);
                    break;
                case string s:
                    self.set(entry.SaveLabel, s);
                    break;
                case bool b:
                    self.set(entry.SaveLabel, b);
                    break;
                case long l:
                    self.set(entry.SaveLabel, l);
                    break;
            }
        } catch (Exception e) {
            ConcordMod.LogError("Failed to save attached property '" + entry.Key + "' on " + self.GetType().FullName + ": " + e);
        }
    }
    
    private static void LoadOne(BaseSystemData self, PropertyEntry entry)
    {
        try
        {
            object current = null;
            if (entry.ValueType == typeof(int))
            {
                self.get(entry.SaveLabel, out int i);
                current = i;
            } else if (entry.ValueType == typeof(float))
            {
                self.get(entry.SaveLabel, out float f);
                current = f;
            } else if (entry.ValueType == typeof(string))
            {
                self.get(entry.SaveLabel, out string s);
                current = s;
            } else if (entry.ValueType == typeof(bool))
            {
                self.get(entry.SaveLabel, out bool b);
                current = b;
            } else if (entry.ValueType == typeof(long))
            {
                self.get(entry.SaveLabel, out long l);
                current = l;
            }

            entry.Slot.Set(self, current);
        } catch (Exception e) {
            ConcordMod.LogError("Failed to load attached property '" + entry.Key + "' on " + self.GetType().FullName + ": " + e);
        }
    }
}