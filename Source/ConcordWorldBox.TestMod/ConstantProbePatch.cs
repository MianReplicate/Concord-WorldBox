namespace Concord;

[Patch(typeof(TestMod))]
public abstract class ConstantProbePatch {
    [Inject(nameof(TestMod.Threshold), 18f, At.Constant)]
    private static float RaiseThreshold(float original) {
        return 20f;
    }
    
    [Inject(nameof(TestMod.GetRealHealth), typeof(TestMod), nameof(TestMod.DefaultHealth), At.Around)]
    private static int GetDefaultHealthInGetHealth(Operation<int> read)
    {
        return read.Invoke() + 100;
    }
}