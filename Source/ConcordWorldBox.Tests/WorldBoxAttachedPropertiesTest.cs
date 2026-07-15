using Concord;
using Xunit;

namespace ConcordWorldBox.Tests;

public sealed class RimWorldAttachedPropertyRegistryTests {
    private sealed class Target { }

    [Fact]
    public void RegisterAttachedProperty_NamespacesKeyWithModId() {
        PropertyRegistry registry = new PropertyRegistry();
        WorldBoxAttachedPropertyRegistry adapter = new WorldBoxAttachedPropertyRegistry(registry, "MyMod");

        adapter.RegisterAttachedProperty(typeof(Target), "count", typeof(int));

        System.Collections.Generic.IReadOnlyList<PropertyEntry> entries = registry.ForBaseType(typeof(Target));
        Assert.Single(entries);
        Assert.Equal("MyMod.count", entries[0].Key);
    }
}