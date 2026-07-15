using Concord.Orchestration;

namespace Concord;

public class WorldBoxAdapter
{
    private static bool wired;
    private static WorldBoxPatchApplier patchApplier;

    public static void Wire() {
        if (wired) {
            return;
        }

        wired = true;
        PropertyRegistry registry = new PropertyRegistry();
        WorldBoxRuntime.Registry = registry;
        patchApplier = new WorldBoxPatchApplier();

        WorldBoxAttachedPropertyRegistry propertyRegistry = new WorldBoxAttachedPropertyRegistry(registry, "Concord.WorldBox");
        PatchDeclarationScanner.ScanAssembly(typeof(WorldBoxAdapter).Assembly, patchApplier, propertyRegistry);
    }
}