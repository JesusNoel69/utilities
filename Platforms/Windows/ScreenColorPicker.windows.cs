using utilities.Interfaces;
using System.Runtime.InteropServices;
using System.Diagnostics;
namespace utilities.Platforms.Windows
{
    public sealed class ScreenColorPicker : IScreenColorPicker
    {
        public bool IsSupported => true;
        private IntPtr _hookId = IntPtr.Zero;
        private LowLevelMouseProc? _proc;
        public event Action? OnGlobalClick;

        public void StartGlobalClickListener()
        {
            _proc = HookCallback;
            _hookId = SetHook(_proc);
        }
        public void StopGlobalClickListener()
        {
            if (_hookId != IntPtr.Zero)
            {
                UnhookWindowsHookEx(_hookId);
                _hookId = IntPtr.Zero;
            }
        }
        private IntPtr SetHook(LowLevelMouseProc proc)
        {
            using var curProcess = Process.GetCurrentProcess();
            using var curModule = curProcess.MainModule!;
            return SetWindowsHookEx(WH_MOUSE_LL, proc,
                GetModuleHandle(curModule.ModuleName), 0);
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            const int WM_LBUTTONDOWN = 0x0201;

            if (nCode >= 0 && wParam == (IntPtr)WM_LBUTTONDOWN)
            {
                OnGlobalClick?.Invoke();
            }

            return CallNextHookEx(_hookId, nCode, wParam, lParam);
        }
        public Color GetColorUnderCursor()
        {
            // Cursor position
            if (!GetCursorPos(out POINT p))
                return Colors.Transparent;
            // DC of desktop
            IntPtr hdc = GetDC(IntPtr.Zero);
            if (hdc == IntPtr.Zero)
                return Colors.Transparent;
            try
            {
                // COLORREF (0x00bbggrr)
                uint colorRef = GetPixel(hdc, p.X, p.Y);

                byte r = (byte)(colorRef & 0x000000FF);
                byte g = (byte)((colorRef & 0x0000FF00) >> 8);
                byte b = (byte)((colorRef & 0x00FF0000) >> 16);
                return Color.FromRgb(r, g, b);
            }
            finally
            {
                ReleaseDC(IntPtr.Zero, hdc);
            }
        }
        // Win32
        private const int WH_MOUSE_LL = 14;
        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk,
            int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll")]
        private static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);
    }
}