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

            if (app.Runtime != WebUiRuntime.none)
            {
                NativeMethods.webui_script_runtime(handle, app.Runtime);
            }
        }
        #endregion

        #region Properties
        internal IntPtr Handle
        {
            get => handle;
        }

        public uint WindowNumber
        {
            get
            {
                if (handle == IntPtr.Zero) return 0;
                return NativeMethods.webui_window_get_number(handle);
            }
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
            NativeMethods.webui_multi_access(handle, status);
        }

        public bool Open(string url)
        {
            using (var urlHandle = new GCString(url))
            {
                return NativeMethods.webui_open(handle, (IntPtr)urlHandle, application.Browser);
            }
        }

        public bool Show(string html)
        {
            using (var htmlHandle = new GCString(html, GCString.EncodingTypes.UTF8))
            {
                return NativeMethods.webui_show(handle, (IntPtr)htmlHandle, application.Browser);
            }
        }
        
        public bool Refresh(string html)
        {
            using (var htmlHandle = new GCString(html, GCString.EncodingTypes.UTF8))
            {
                return NativeMethods.webui_refresh(handle, (IntPtr)htmlHandle);
            }
        }

        public string NewServer(string rootPath)
        {
            using (var pathHandle = new GCString(rootPath))
            {
                var urlPtr = NativeMethods.webui_new_server(handle, (IntPtr)pathHandle);
                if (urlPtr != IntPtr.Zero)
                {
                    var ansiStr = Marshal.PtrToStringAnsi(urlPtr);
                    return ansiStr;
                }
            }

            return string.Empty;
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

        public void BindAll(EventCallback callback)
        {
            var cb = new cb_fn((e) =>
            {
                var ev = new WebUiEvent(e);
                callback.Invoke(ev);
            });

            IntPtr cbPtr = Marshal.GetFunctionPointerForDelegate(cb);
            NativeMethods.webui_bind_all(handle, cbPtr);
        }

        public JavaScriptResult RunJavaScript(string script, ushort timeOut = 30)
        {
            if (string.IsNullOrEmpty(script)) return JavaScriptResult.Empty;

            using (var scriptHandle = new GCString(script, GCString.EncodingTypes.UTF8))
            {
                var scriptInterface = new webui_script()
                {
                    script = (IntPtr)scriptHandle,
                    timeout = timeOut,
                    result = IntPtr.Zero
                };

                IntPtr interfacePtr = Marshal.AllocHGlobal(Marshal.SizeOf(scriptInterface));
                try
                {   
                    Marshal.StructureToPtr(scriptInterface, interfacePtr, true);
                    NativeMethods.webui_script_interface_struct(handle, interfacePtr);

                    return new JavaScriptResult(scriptInterface.result);
                }
                catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
                finally
                {
                    Marshal.FreeHGlobal(interfacePtr);
                }
            }

            return JavaScriptResult.Empty;
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
            
        }
        #endregion
    }
}