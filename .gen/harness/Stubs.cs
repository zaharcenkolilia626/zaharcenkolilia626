// Minimal UnityEngine surface so the REAL generator source compiles and runs headlessly.
// Only the members EmberLevelGenerator / ChamberConfig actually touch are stubbed, and
// each mirrors Unity's documented behaviour exactly.
namespace UnityEngine
{
    public static class Mathf
    {
        public static int Min(int a, int b) { return a < b ? a : b; }
        public static int Max(int a, int b) { return a > b ? a : b; }
        public static int CeilToInt(float f) { return (int)System.Math.Ceiling(f); }
        public static int RoundToInt(float f) { return (int)System.Math.Round(f, System.MidpointRounding.ToEven); }
        public static int Clamp(int v, int lo, int hi) { return v < lo ? lo : (v > hi ? hi : v); }
        public static float Clamp(float v, float lo, float hi) { return v < lo ? lo : (v > hi ? hi : v); }
        public static float Min(float a, float b) { return a < b ? a : b; }
        public static float Max(float a, float b) { return a > b ? a : b; }
    }

    public static class Debug
    {
        public static void Log(object message) { }
    }
}
