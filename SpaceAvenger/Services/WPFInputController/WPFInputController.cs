using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using WPFGameEngine.WPF.GE.Component.Controllers;

namespace SpaceAvenger.Services.WPFInputControllers
{
    public class WPFInputController : ControllerComponent
    {
        #region Fields
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_MOUSEMOVE = 0x0200;
        private Window m_window;
        private HwndSource m_hwndSource;
        private Dictionary<Key, bool> m_activeKeys;
        private Dictionary<MouseButton, bool> m_MouseButtons;
        #endregion

        #region Ctor
        public WPFInputController(Window window) : base()
        {
            m_window = window ?? throw new ArgumentNullException(nameof(window));
            IntPtr hwnd = new WindowInteropHelper(window).Handle;
            m_hwndSource = HwndSource.FromHwnd(hwnd);

            m_MouseButtons = new Dictionary<MouseButton, bool>();
            m_MouseButtons.Add(MouseButton.Left, false);

            m_activeKeys = new Dictionary<Key, bool>();
            m_activeKeys.Add(Key.A, false);
            m_activeKeys.Add(Key.W, false);
            m_activeKeys.Add(Key.S, false);
            m_activeKeys.Add(Key.D, false);
            m_activeKeys.Add(Key.E, false);
            m_activeKeys.Add(Key.R, false);

            if (m_hwndSource != null)
            {
                m_hwndSource.AddHook(WndProc);
            }
        }
        #endregion

        #region Methods
        
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_KEYDOWN:
                case WM_KEYUP:
                    ProcessKeyboard(wParam, msg == WM_KEYDOWN, ref handled);
                    break;

                case WM_LBUTTONDOWN:
                case WM_LBUTTONUP:
                    ProcessMouseClick(lParam, msg == WM_LBUTTONDOWN, MouseButton.Left, ref handled);
                    break;

                case WM_MOUSEMOVE:
                    ProcessMouseMove(lParam, ref handled);
                    break;
            }
            return IntPtr.Zero;
        }

        private System.Windows.Point ExtractMousePosition(IntPtr lParam)
        {
            int x = lParam.ToInt32() & 0xFFFF;
            int y = (lParam.ToInt32() >> 16) & 0xFFFF;

            System.Windows.Point positionInPixels = new System.Windows.Point(x, y);
            PresentationSource source = PresentationSource.FromVisual(m_window);

            if (source == null || source.CompositionTarget == null)
            {
                return positionInPixels;
            }

            Matrix transform = source.CompositionTarget.TransformFromDevice;

            System.Windows.Point positionInDIPs = transform.Transform(positionInPixels);

            return positionInDIPs;
        }

        private void ProcessKeyboard(IntPtr wParam, bool isDown, ref bool handled)
        {
            int virtualKeyCode = wParam.ToInt32();
            Key wpfKey = KeyInterop.KeyFromVirtualKey(virtualKeyCode);

            if (wpfKey != Key.None)
            {
                if (isDown)
                {
                    m_activeKeys[wpfKey] = true;
                }
                else
                {
                    m_activeKeys[wpfKey] = false;
                }
            }
        }

        private void ProcessMouseClick(IntPtr lParam, bool isDown, MouseButton button, ref bool handled)
        {
            if (isDown)
            {
                m_MouseButtons[button] = true;
            }
            else
            {
                m_MouseButtons[button] = false;
            }
        }

        private void ProcessMouseMove(IntPtr lParam, ref bool handled)
        {
            System.Windows.Point p = ExtractMousePosition(lParam);

            MousePosition = new System.Numerics.Vector2((float)p.X, (float)p.Y);
        }

        public override void Dispose()
        {
            m_hwndSource.RemoveHook(WndProc);
            m_hwndSource.Dispose();
        }

        public override bool IsKeyDown(Key key)
        {
            return m_activeKeys.ContainsKey(key) && m_activeKeys[key];
        }

        public override bool IsMouseButtonDown(MouseButton mouseButton)
        {
            return m_MouseButtons.ContainsKey(mouseButton) && m_MouseButtons[mouseButton];
        }

        public override object Clone()
        {
            return new WPFInputController(m_window);
        }
        #endregion
    }
}
