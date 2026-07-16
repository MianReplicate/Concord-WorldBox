using NeoModLoader.api;

namespace Concord;

public class TestMod : BasicMod<TestMod>
{
    protected override void OnModLoad()
    {
        Patcher.Apply(typeof(TestMod).Assembly);
        LogInfo("test mod patches applied");
    }
    
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3400", Justification = "Must be a method so Concord can patch it; the test probes the patched return value.")]
    public static float Threshold() {
        return 18f;
    }
}