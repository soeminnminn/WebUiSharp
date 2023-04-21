using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WebUiSharp
{
    internal class WebUiWinHandle
    {
        #region Variables
        private readonly IntPtr handle;
        #endregion

        #region Constructors
        internal WebUiWinHandle(IntPtr handle)
        {
            this.handle = handle;
        }
        #endregion

        #region Properties
        public IntPtr Handle
        {
            get => handle;
        }

        public bool IsEmpty
        {
            get => handle == IntPtr.Zero;
        }

        public ushort WindowNumber { get; private set; }
        public bool ServerRunning { get; private set; }
        public bool Connected { get; private set; }
        public bool ServerHandled { get; private set; }
        public bool MultiAccess { get; private set; }
        public bool ServerRoot { get; private set; }
        public ushort ServerPort { get; private set; }
        public bool IsBindAll { get; private set; }
        public string Url { get; private set; }
        public IntPtr CbAll { get; private set; }
        public string Html { get; private set; }
        public string HtmlCpy { get; private set; }
        public string Icon { get; private set; }
        public string IconType { get; private set; }
        public ushort CurrentBrowser { get; private set; }
        public string BrowserPath { get; private set; }
        public string ProfilePath { get; private set; }
        public ushort Connections { get; private set; }
        public ushort Runtime { get; private set; }
        public bool DetectProcessClose { get; private set; }
        public IntPtr ServerThread { get; private set; }
        public string Path { get; private set; }
        #endregion

        #region Methods
        internal WebUiWinHandle ReadFromMemory()
        {
            var win = Marshal.PtrToStructure<webui_window>(handle);
            
            WindowNumber = win.core.window_number;
            ServerRunning = win.core.server_running;
            Connected = win.core.connected;
            ServerHandled = win.core.server_handled;
            MultiAccess = win.core.multi_access;
            ServerRoot = win.core.server_root;
            // TODO:
            ServerPort = win.core.server_port;
            IsBindAll = win.core.is_bind_all;
            Url = win.core.url == IntPtr.Zero ? string.Empty : Marshal.PtrToStringAnsi(win.core.url);
            CbAll = win.core.cb_all;
            Html = win.core.html == IntPtr.Zero ? string.Empty : Marshal.PtrToStringUTF8(win.core.html);
            HtmlCpy = win.core.html_cpy == IntPtr.Zero ? string.Empty : Marshal.PtrToStringUTF8(win.core.html_cpy);
            Icon = win.core.icon == IntPtr.Zero ? string.Empty : Marshal.PtrToStringAnsi(win.core.icon);
            IconType = win.core.icon_type == IntPtr.Zero ? string.Empty : Marshal.PtrToStringAnsi(win.core.icon_type);
            // TODO:
            CurrentBrowser = win.core.CurrentBrowser;
            BrowserPath = win.core.browser_path == IntPtr.Zero ? string.Empty : Marshal.PtrToStringAnsi(win.core.browser_path);
            ProfilePath = win.core.profile_path == IntPtr.Zero ? string.Empty : Marshal.PtrToStringAnsi(win.core.profile_path);
            Connections = win.core.connections;
            Runtime = win.core.runtime;
            DetectProcessClose = win.core.detect_process_close;
            ServerThread = win.core.server_thread;
            Path = win.path == IntPtr.Zero ? string.Empty : Marshal.PtrToStringAnsi(win.path);

            return this;
        }
        #endregion

        #region Operators
        public static explicit operator IntPtr(WebUiWinHandle value)
        {
            return value.Handle;
        }
        #endregion
    }
}
