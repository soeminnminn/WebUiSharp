using System;
using System.Runtime.InteropServices;

namespace WebUiSharp
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void cb_fn(IntPtr e);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public delegate void cb_interface_fn(ushort element_id, ushort window_id, IntPtr element_name, IntPtr window, IntPtr data, ref IntPtr response);

    [StructLayout(LayoutKind.Sequential)]
    public struct webui_timer
    {
        public TimeSpan start;
        public TimeSpan now;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct webui_window_core
    {
        public ushort window_number;
        public bool server_running;
        public bool connected;
        public bool server_handled;
        public bool multi_access;
        public bool server_root;
        public ushort server_port;
        public bool is_bind_all;
        public IntPtr url;
        public IntPtr cb_all;
        public IntPtr html;
        public IntPtr html_cpy;
        public IntPtr icon;
        public IntPtr icon_type;
        public ushort CurrentBrowser;
        public IntPtr browser_path;
        public IntPtr profile_path;
        public ushort connections;
        public ushort runtime;
        public bool detect_process_close;
        public IntPtr server_thread;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct webui_window
    {
        public webui_window_core core;
        public IntPtr path;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct webui_event
    {
        public ushort window_id;
        public ushort element_id;
        public string element_name;
        public IntPtr window;
        public IntPtr data;
        public IntPtr response;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct webui_javascript_result
    {
        public bool error;
        public ushort length;
        public IntPtr data;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct webui_script
    {
        public IntPtr script;
        public ushort timeout;
        public IntPtr result;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct webui_cb
    {
        public IntPtr win;
        public IntPtr webui_internal_id;
        public IntPtr element_name;
        public IntPtr data;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct webui_cmd_async
    {
        public IntPtr win;
        public IntPtr cmd;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct webui_custom_browser
    {
        public IntPtr app;
        public IntPtr arg;
        public bool auto_link;
    }

    public enum WebUiBrowser: ushort
    {
        any = 0,
        chrome = 1,
        firefox = 2,
        edge = 3,
        safari = 4,
        chromium = 5,
        custom = 99
    }

    public enum WebUiRuntime : ushort
    {
        none = 0,
        deno = 1,
        nodejs = 2
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct webui
    {
        public ushort servers;
        public ushort connections;
        public ushort process;
        public IntPtr custom_browser;
        public bool wait_for_socket_window;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string html_elements;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
        public ushort[] used_ports;
        public ushort last_window;
        public ushort startup_timeout;
        public bool use_timeout;
        public bool timeout_extra;
        public bool exit_now;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string run_responses;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
        public bool[] run_done;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
        public bool[] run_error;
        public ushort run_last_id;
        public IntPtr mg_mgrs;
        public IntPtr mg_connections;
        public ushort browser;
        public ushort runtime;
        public bool initialized;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
        public cb_fn[] cb;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
        public cb_interface_fn[] cb_interface;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
        public cb_interface_fn[] cb_interface_all;
        public IntPtr executable_path;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
        public IntPtr[] ptr_list;
        public ushort ptr_position;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
        public ushort[] ptr_size;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct webui_script_interface
    {
        public IntPtr script;
        public ushort timeout;
        public bool error;
        public ushort length;
        public IntPtr data;
    }

    internal class NativeMethods
    {
        #region Constants
        private const string DllFile = "webui-2-x64";

        internal const string WEBUI_VERSION = "2.0.6";      // Version
        internal const ushort WEBUI_HEADER_SIGNATURE = 0xFF;  // All packets should start with this 8bit
        internal const ushort WEBUI_HEADER_JS = 0xFE;         // Javascript result in frontend
        internal const ushort WEBUI_HEADER_CLICK = 0xFD;      // Click event
        internal const ushort WEBUI_HEADER_SWITCH = 0xFC;     // Frontend refresh
        internal const ushort WEBUI_HEADER_CLOSE = 0xFB;      // Close window
        internal const ushort WEBUI_HEADER_CALL_FUNC = 0xFA;  // Call a backend function
        internal const int WEBUI_MAX_ARRAY = 1024;         // Max threads, servers, windows, pointers..
        internal const int WEBUI_MIN_PORT = 10000;         // Minimum socket port
        internal const int WEBUI_MAX_PORT = 65500;         // Should be less than 65535
        internal const int WEBUI_MAX_BUF = 1024000;        // 1024 Kb max dynamic memory allocation
        internal const string WEBUI_DEFAULT_PATH = ".";     // Default root path
        internal const int WEBUI_DEF_TIMEOUT = 8;          // Default startup timeout in seconds
        #endregion

        #region -- Definitions ---------------------
        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webui_wait();

        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webui_exit();

        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static bool webui_is_any_window_running();

        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static bool webui_is_app_running();

        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webui_set_timeout(ushort second);

        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static IntPtr webui_new_window();

        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static bool webui_show([In, Out] IntPtr win, IntPtr html, WebUiBrowser browser);

        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static bool webui_show_cpy([In, Out] IntPtr win, IntPtr html, WebUiBrowser browser);

        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static bool webui_refresh([In, Out] IntPtr win, IntPtr html);

        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static bool webui_refresh_cpy([In, Out] IntPtr win, IntPtr html);

        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webui_set_icon([In, Out] IntPtr win, IntPtr icon_s, IntPtr type_s);

        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webui_multi_access([In, Out] IntPtr win, bool status);

        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static IntPtr webui_new_server([In, Out] IntPtr win, IntPtr path);

        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webui_close(IntPtr win);

        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static bool webui_is_shown(IntPtr win);

        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webui_script([In, Out] IntPtr win, [In, Out] IntPtr script);

        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static ushort webui_bind([In, Out] IntPtr win, IntPtr element, IntPtr func);

        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webui_bind_all([In, Out] IntPtr win, IntPtr func);

        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static bool webui_open([In, Out] IntPtr win, IntPtr url, WebUiBrowser browser);

        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webui_script_cleanup([In, Out] IntPtr script);

        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webui_script_runtime([In, Out] IntPtr win, WebUiRuntime runtime);

        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static int webui_get_int(IntPtr e);

        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static IntPtr webui_get_string(IntPtr e);

        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static bool webui_get_bool(IntPtr e);

        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webui_return_int(IntPtr e, ref int n);

        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webui_return_string(IntPtr e, ref IntPtr s);

        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webui_return_bool(IntPtr e, ref bool b);
        #endregion

        #region -- Interface -----------------------
        // Used by other languages to create WebUI wrappers
        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static ushort webui_bind_interface([In, Out] IntPtr win, IntPtr element, IntPtr func);

        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webui_script_interface(IntPtr win, IntPtr script, ushort timeout, bool error, ushort length, ref IntPtr data);
        
        [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webui_script_interface_struct(IntPtr win, [In, Out] IntPtr js_int);
        #endregion

        #region -- Core -----------------------
        [DllImport(DllFile, EntryPoint = "_webui_init", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webui_init();

        [DllImport(DllFile, EntryPoint = "_webui_get_cb_index", CallingConvention = CallingConvention.Cdecl)]
        internal extern static ushort webui_get_cb_index(IntPtr webui_internal_id);

        [DllImport(DllFile, EntryPoint = "_webui_set_cb_index", CallingConvention = CallingConvention.Cdecl)]
        internal extern static ushort webui_set_cb_index(IntPtr webui_internal_id);

        [DllImport(DllFile, EntryPoint = "_webui_get_free_port", CallingConvention = CallingConvention.Cdecl)]
        internal extern static ushort webui_get_free_port();

        [DllImport(DllFile, EntryPoint = "_webui_get_new_window_number", CallingConvention = CallingConvention.Cdecl)]
        internal extern static ushort webui_get_new_window_number();

        [DllImport(DllFile, EntryPoint = "_webui_wait_for_startup", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webui_wait_for_startup();

        [DllImport(DllFile, EntryPoint = "_webui_free_port", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webui_free_port(ushort port);

        [DllImport(DllFile, EntryPoint = "_webui_set_custom_browser", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webui_set_custom_browser(IntPtr p);

        [DllImport(DllFile, EntryPoint = "_webui_get_current_path", CallingConvention = CallingConvention.Cdecl)]
        internal extern static string webui_get_current_path();

        [DllImport(DllFile, EntryPoint = "_webui_window_receive", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webui_window_receive(IntPtr win, IntPtr packet, ushort len);

        [DllImport(DllFile, EntryPoint = "_webui_window_send", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webui_window_send(IntPtr win, IntPtr packet, ushort packets_size);

        [DllImport(DllFile, EntryPoint = "_webui_window_event", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webui_window_event(IntPtr win, IntPtr element_id, IntPtr element, IntPtr data, ushort data_len);

        [DllImport(DllFile, EntryPoint = "_webui_window_get_number", CallingConvention = CallingConvention.Cdecl)]
        internal extern static ushort webui_window_get_number(IntPtr win);

        [DllImport(DllFile, EntryPoint = "_webui_window_open", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webui_window_open([In, Out] IntPtr win, IntPtr link, WebUiBrowser browser);

        [DllImport(DllFile, EntryPoint = "_webui_cmd_sync", CallingConvention = CallingConvention.Cdecl)]
        internal extern static int webui_cmd_sync(IntPtr cmd, bool show);

        [DllImport(DllFile, EntryPoint = "_webui_cmd_async", CallingConvention = CallingConvention.Cdecl)]
        internal extern static int webui_cmd_async(IntPtr cmd, bool show);

        [DllImport(DllFile, EntryPoint = "_webui_run_browser", CallingConvention = CallingConvention.Cdecl)]
        internal extern static int webui_run_browser(IntPtr win, IntPtr cmd);

        [DllImport(DllFile, EntryPoint = "_webui_clean", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webui_clean();

        [DllImport(DllFile, EntryPoint = "_webui_browser_exist", CallingConvention = CallingConvention.Cdecl)]
        internal extern static bool webui_browser_exist([In, Out] IntPtr win, WebUiBrowser browser);

        [DllImport(DllFile, EntryPoint = "_webui_browser_get_temp_path", CallingConvention = CallingConvention.Cdecl)]
        internal extern static string webui_browser_get_temp_path(WebUiBrowser browser);

        [DllImport(DllFile, EntryPoint = "_webui_folder_exist", CallingConvention = CallingConvention.Cdecl)]
        internal extern static bool webui_folder_exist(IntPtr folder);

        [DllImport(DllFile, EntryPoint = "_webui_browser_create_profile_folder", CallingConvention = CallingConvention.Cdecl)]
        internal extern static bool webui_browser_create_profile_folder([In, Out] IntPtr win, WebUiBrowser browser);

        [DllImport(DllFile, EntryPoint = "_webui_browser_start_edge", CallingConvention = CallingConvention.Cdecl)]
        internal extern static bool webui_browser_start_edge([In, Out] IntPtr win, IntPtr address);

        [DllImport(DllFile, EntryPoint = "_webui_browser_start_firefox", CallingConvention = CallingConvention.Cdecl)]
        internal extern static bool webui_browser_start_firefox([In, Out] IntPtr win, IntPtr address);

        [DllImport(DllFile, EntryPoint = "_webui_browser_start_custom", CallingConvention = CallingConvention.Cdecl)]
        internal extern static bool webui_browser_start_custom([In, Out] IntPtr win, IntPtr address);

        [DllImport(DllFile, EntryPoint = "_webui_browser_start_chrome", CallingConvention = CallingConvention.Cdecl)]
        internal extern static bool webui_browser_start_chrome([In, Out] IntPtr win, IntPtr address);

        [DllImport(DllFile, EntryPoint = "_webui_browser_start", CallingConvention = CallingConvention.Cdecl)]
        internal extern static bool webui_browser_start([In, Out] IntPtr win, IntPtr address, WebUiBrowser browser);

        [DllImport(DllFile, EntryPoint = "_webui_timer_diff", CallingConvention = CallingConvention.Cdecl)]
        internal extern static long webui_timer_diff(IntPtr start, IntPtr end);

        [DllImport(DllFile, EntryPoint = "_webui_timer_start", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webui_timer_start([In, Out] IntPtr t);

        [DllImport(DllFile, EntryPoint = "_webui_timer_is_end", CallingConvention = CallingConvention.Cdecl)]
        internal extern static bool webui_timer_is_end([In, Out] IntPtr t, ushort ms);

        [DllImport(DllFile, EntryPoint = "_webui_timer_clock_gettime", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webui_timer_clock_gettime(long spec);

        [DllImport(DllFile, EntryPoint = "_webui_set_root_folder", CallingConvention = CallingConvention.Cdecl)]
        internal extern static bool webui_set_root_folder([In, Out] IntPtr win, IntPtr path);

        [DllImport(DllFile, EntryPoint = "_webui_wait_process", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webui_wait_process([In, Out] IntPtr win, bool status);

        [DllImport(DllFile, EntryPoint = "_webui_generate_js_bridge", CallingConvention = CallingConvention.Cdecl)]
        internal extern static string webui_generate_js_bridge(IntPtr win);

        [DllImport(DllFile, EntryPoint = "_webui_print_hex", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webui_print_hex(string data, ushort len);

        [DllImport(DllFile, EntryPoint = "_webui_free_mem", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webui_free_mem(IntPtr p);

        [DllImport(DllFile, EntryPoint = "_webui_str_copy", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void webui_str_copy(IntPtr destination, IntPtr source);
        #endregion
    }
}
