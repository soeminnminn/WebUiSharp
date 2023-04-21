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
        private readonly WebUiBrowser browser;
        private readonly WebUiRuntime runtime;
        private readonly string currentPath;
        private WebUiWindow mainWindow = null;
        private readonly List<WebUiWindow> windows;
        #endregion

        #region Constructors
        public WebUiApplication()
            : this(WebUiBrowser.any)
        { }

        public WebUiApplication(WebUiBrowser browser)
            : this(browser, WebUiRuntime.none)
        { }

        public WebUiApplication(WebUiBrowser browser, WebUiRuntime runtime)
        {
            this.browser = browser;
            this.runtime = runtime;
            this.windows = new List<WebUiWindow>();
            currentPath = NativeMethods.webui_get_current_path();
        }

        ~WebUiApplication()
        {
            Dispose();
        }
        #endregion

        #region Properties
        public string CurrentPath
        {
            get => currentPath;
        }

        public WebUiBrowser Browser
        {
            get => browser;
        }

        public WebUiRuntime Runtime
        {
            get => runtime;
        }
        #endregion

        #region Methods
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

        public bool IsAppRunning()
        {
            return NativeMethods.webui_is_app_running();
        }

        public bool IsAnyWindowRunning()
        {
            return NativeMethods.webui_is_any_window_running();
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
            return window.WindowNumber == mainWindow.WindowNumber;
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
