namespace Concord;

[Patch]
public abstract class TestPatch : MapBox {
    private static bool logged;

    [Inject(At.Head, nameof(Update))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S2696", Justification = "This injected instance patch deliberately sets a static once-flag so the probe logs only on the first tick.")]
    public void ProbeUpdate(ControlHandle ch) {
        if (logged) {
            return;
        }

        logged = true;
        TestMod.LogInfo("probe patch fired on MapBox.Update");
        TestMod.LogInfo("constant probe: Threshold() = " + TestMod.Threshold());
    }
}