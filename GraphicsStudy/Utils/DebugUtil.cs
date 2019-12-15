using System.Diagnostics;
using SDebug = System.Diagnostics.Debug;

namespace GraphicsStudy
{
    public static class DebugUtil
    {
        [Conditional("_DEBUG")]
        public static void WriteLine(object value)
        {
            SDebug.WriteLine(value);
        }

        [Conditional("_DEBUG")]
        public static void WriteLine(string message)
        {
            SDebug.WriteLine(message);
        }

        [Conditional("_DEBUG")]
        public static void WriteLine(string format, params object[] args)
        {
            SDebug.WriteLine(format, args);
        }
    }
}
