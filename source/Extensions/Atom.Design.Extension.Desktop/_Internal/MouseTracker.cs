using Atom.Design.Extension.Desktop.Controls;
using Caliburn.Micro;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Atom.Design.Extension.Desktop
{
    internal sealed class MouseTracker : IDisposable
    {
        private const int ShowPopupDelay = 1000;
        private readonly ElementDetailsPopup _popup;
        private readonly ElementHighlighter _highlighter;
        private readonly System.Timers.Timer _updateTimer;
        private Element _currentElement;

        public MouseTracker()
        {
            _popup = new ElementDetailsPopup();
            _highlighter = new ElementHighlighter();
            _updateTimer = new System.Timers.Timer() { AutoReset = true, Interval = 300 };
            _updateTimer.Elapsed += OnUpdateTimerElapsed;
            _updateTimer.Start();
        }
        
        private bool IsTargetKeyPressed
        {
            get { return Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl); }
        }

        private bool IsVisible
        {
            get { return _popup.IsVisible; }
        }

        public event EventHandler<ElementEventArgs> InsertElement
        {
            add { _popup.InsertElement += value; }
            remove { _popup.InsertElement -= value; }
        }

        public event EventHandler<ElementEventArgs> SyncronizeElement
        {
            add { _popup.SyncronizeElement += value; }
            remove { _popup.SyncronizeElement -= value; }
        }

        private void OnUpdateTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Execute.OnUIThread(UpdateInternal);
        }

        private void UpdateInternal()
        {
            if (IsTargetKeyPressed)
            {
                if (!IsVisible)
                {
                    Thread.Sleep(ShowPopupDelay);
                    if (IsTargetKeyPressed)
                    {
                        ShowInternal();
                    }
                }
            }
            else
            {
                if (IsVisible)
                {
                    HideInternal();
                }
            }
        }

        private void ShowInternal()
        {
            POINT cursorPosition;
            if (!GetCursorPos(out cursorPosition))
            {
                return;
            }
            Point point = new Point(cursorPosition.X, cursorPosition.Y);
            _currentElement = ElementLocator.FromPoint(point);
            if (_currentElement != null)
            {
                Rect elementRectangle = _currentElement.Properties.BoundingRectangle;
                if (elementRectangle != Rect.Empty)
                {
                    _highlighter.Left = (int)elementRectangle.Left;
                    _highlighter.Top = (int)elementRectangle.Top;
                    _highlighter.Height = (int)elementRectangle.Height;
                    _highlighter.Width = (int)elementRectangle.Width;
                    _highlighter.Show();
                }
                _popup.Left = cursorPosition.X + 10;
                _popup.Top = cursorPosition.Y + 10;
                _popup.Show(_currentElement);
            }
        }

        private void HideInternal()
        {
            _currentElement = null;
            _popup.Hide();
            _highlighter.Hide();
        }
        
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        public void Dispose()
        {
            _updateTimer.Dispose();
            HideInternal();
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;
        }
    }
}
