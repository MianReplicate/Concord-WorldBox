using System.Collections.Generic;
using System.Reflection;
using Concord.Emit;
using Concord.Orchestration;

namespace Concord;

public class WorldBoxPatchApplier : IPatchApplier
{
    private readonly List<IPatchHandle> handles = new();

    public void ApplyPatch(MethodBase target, Injection injection) {
        handles.Add(Patcher.PatchInjection(target, injection));
    }
}