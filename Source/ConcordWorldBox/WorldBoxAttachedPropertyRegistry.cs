using System;
using Concord.Orchestration;

namespace Concord;

public class WorldBoxAttachedPropertyRegistry : IAttachedPropertyRegistry
{
    private readonly string modId;
    private readonly PropertyRegistry registry;

    public WorldBoxAttachedPropertyRegistry(PropertyRegistry registry, string modId) {
        this.registry = registry;
        this.modId = modId;
    }

    public void RegisterAttachedProperty(Type baseType, string name, Type valueType) {
        string key = modId + "." + name;
        registry.Add(baseType, key, valueType, null);
    }
}