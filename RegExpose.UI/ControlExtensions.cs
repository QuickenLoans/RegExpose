using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RegExpose.UI
{
    public static class ControlExtensions
    {
        public static IDisposable LockWindowUpdate(this Control control)
        {
            LockWindowUpdate(control.Handle);
            return new Disposable();
        }

        [DllImport("user32.dll")]
        private static extern long LockWindowUpdate(IntPtr handle);

        private class Disposable : IDisposable
        {
            public void Dispose()
            {
                LockWindowUpdate(IntPtr.Zero);
            }
        }
    }
}