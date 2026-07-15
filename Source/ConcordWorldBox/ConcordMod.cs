using System;
using System.Linq;
using System.Reflection;
using NeoModLoader.api;

namespace Concord;
public class ConcordMod : BasicMod<ConcordMod>
{
    protected override void OnModLoad()
    {
        if (ShippedConcordCannotLoad()) {
            LogError(
                "The shipped Concord runtime depends on System.Reflection.Emit " +
                "facade assemblies that this runtime does not provide. Concord wiring is skipped; " +
                "mods depending on Concord will not be patched. A runtime-compatible Concord build " +
                "is required.");
            return;
        }

        try {
            WorldBoxAdapter.Wire();
            LogInfo("Concord runtime wired.");
        } catch (Exception e) {
            LogError("Failed to initialize Concord: " + e);
        }
    }
        
    private static bool ShippedConcordCannotLoad() {
        Assembly concord = null;
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
            if (assembly.GetName().Name == "Concord") {
                concord = assembly;
                break;
            }
        }

        if (concord == null) {
            return false;
        }

        bool referencesFacade = concord.GetReferencedAssemblies().Any(reference =>
            reference.Name == "System.Reflection.Emit.Lightweight" ||
            reference.Name == "System.Reflection.Emit.ILGeneration");

        if (!referencesFacade) {
            return false;
        }

        return Type.GetType(
            "System.Reflection.Emit.DynamicMethod, System.Reflection.Emit.Lightweight, " +
            "Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
            false) == null;
    }
}