using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WebUiSharp
{
    public class WebUiApplication : IDisposable
    {
        #region Variables
        private readonly WebUiRuntimes runtime;
        private WebUiWindow mainWindow = null;
        private readonly List<WebUiWindow> windows;
        #endregion

        #region Constructors
        public WebUiApplication()
            : this(WebUiRuntimes.None)
        { }

        public WebUiApplication(WebUiRuntimes runtime)
        {
            this.runtime = runtime;
            this.windows = new List<WebUiWindow>();
        }

        ~WebUiApplication()
        {
            Dispose();
        }
        #endregion

        #region Properties
        public WebUiRuntimes Runtime
        {
            get => runtime;
        }
        #endregion

        #region Methods
        public void SetTimeout(ushort second)
        {
            NativeMethods.webui_set_timeout(second);
        }

        public WebUiWindow NewWindow()
        {
            var window = new WebUiWindow(this);
            if (mainWindow == null)
                mainWindow = window;
            else
                windows.Add(window);
            return window;
        }

        public void Wait()
        {
            NativeMethods.webui_wait();
        }

        public void Exit()
        {
            CloseAllWindows(true);

            NativeMethods.webui_exit();
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!OperatingSystem.IsWindows())
                {
                    NativeMethods.webui_exit();
                }

                GC.SuppressFinalize(this);
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        internal bool IsMainWindow(WebUiWindow window)
        {
            if (mainWindow == null || window == null) return false;
            return window.Handle == mainWindow.Handle;
        }

        internal bool IsHasMainWindow()
        {
            if (mainWindow == null) return false;
            return mainWindow.Handle != IntPtr.Zero;
        }

        internal void WindowClosing(WebUiWindow window)
        {
            if (!IsMainWindow(window))
            {
                windows.Remove(window);
            }
        }

        internal void CloseAllWindows(bool includeMain)
        {
            foreach (var item in windows)
            {
                item.CloseInternal();
            }

            if (includeMain && mainWindow != null)
            {
                mainWindow.Close();
            }
        }
        #endregion
    }
}
