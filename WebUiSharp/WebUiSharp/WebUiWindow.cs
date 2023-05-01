using System;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;

namespace WebUiSharp
{
    public class WebUiWindow : IDisposable
    {
        #region Variables
        private IntPtr handle;
        private readonly WebUiApplication application;
        #endregion

        #region Constructors
        internal WebUiWindow(WebUiApplication app)
        {
            application = app;
            handle = NativeMethods.webui_new_window();

            if (app.Runtime != WebUiRuntimes.None)
            {
                NativeMethods.webui_set_runtime(handle, (ushort)app.Runtime);
            }
        }
        #endregion

        #region Properties
        internal IntPtr Handle
        {
            get => handle;
        }

        public WebUiApplication Application
        {
            get => application;
        }

        public bool IsShown
        {
            get
            {
                if (handle == IntPtr.Zero) return false;
                return NativeMethods.webui_is_shown(handle);
            }
        }
        #endregion

        #region Methods
        public void SetIcon(string iconPath, string iconType)
        {
            using(var pathHandle = new GCString(iconPath))
            {
                using (var typeHandle = new GCString(iconType))
                {
                    NativeMethods.webui_set_icon(handle, (IntPtr)pathHandle, (IntPtr)typeHandle);
                }
            }
        }

        public void SetMultiAccess(bool status)
        {
            NativeMethods.webui_set_multi_access(handle, status);
        }

        public bool Show(string html)
        {
            using (var htmlHandle = new GCString(html, GCString.EncodingTypes.UTF8))
            {
                return NativeMethods.webui_show(handle, (IntPtr)htmlHandle);
            }
        }
        
        public bool ShowBrowser(string html, WebUiBrowsers browser)
        {
            using (var htmlHandle = new GCString(html, GCString.EncodingTypes.UTF8))
            {
                return NativeMethods.webui_show_browser(handle, (IntPtr)htmlHandle, (ushort)browser);
            }
        }

        public ushort Bind(string element, EventCallback callback)
        {
            using (var eleHandle = new GCString(element))
            {
                var cb = new cb_fn((e) => 
                {
                    var ev = new WebUiEvent(e);
                    callback.Invoke(ev);
                });

                IntPtr cbPtr = Marshal.GetFunctionPointerForDelegate(cb);
                return NativeMethods.webui_bind(handle, (IntPtr)eleHandle, cbPtr);
            }
        }

        public bool RunScript(string script)
        {
            if (string.IsNullOrEmpty(script)) return false;

            using (var scriptHandle = new GCString(script, GCString.EncodingTypes.UTF8))
            {
                return NativeMethods.webui_run(handle, (IntPtr)scriptHandle);
            }
        }

        public string RunScriptWithResult(string script, ushort timeout = 30, int bufferLength = 1024)
        {
            if (string.IsNullOrEmpty(script)) return string.Empty;

            using (var scriptHandle = new GCString(script, GCString.EncodingTypes.UTF8))
            {
                IntPtr bufferPtr = Marshal.AllocHGlobal(bufferLength);
                bool result = NativeMethods.webui_script(handle, (IntPtr)scriptHandle, timeout, ref bufferPtr, bufferLength);
                if (result)
                {
                    string res = Marshal.PtrToStringUTF8(bufferPtr, bufferLength);
                    Marshal.FreeHGlobal(bufferPtr);

                    if (string.IsNullOrEmpty(res)) return string.Empty;
                    return res.Trim();
                }
            }
            return string.Empty;
        }

        public void Close()
        {
            if (application.IsMainWindow(this))
            {
                application.CloseAllWindows(false);
            }

            application.WindowClosing(this);
            CloseInternal();
        }

        internal void CloseInternal()
        {
            NativeMethods.webui_close(handle);
        }

        public void Dispose()
        {
            handle = IntPtr.Zero;
        }
        #endregion
    }
}