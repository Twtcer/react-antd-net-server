using System;

namespace ReactAntdServer.Comm
{
    public static class ArrayExtension
    {
        public static void ForEach<T>(this T[] ts, Action action)
        {
            foreach (var item in ts)
            {
                action(item);
            }
        }
    }
}
