using System;
using System.Runtime.InteropServices;

namespace WebUiSharp
{
    #region -- Delegates ---------------------------
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void cb_fn(IntPtr e);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public delegate void cb_interface_fn(IntPtr window, WebUiEvents event_type, IntPtr element, IntPtr data, ref IntPtr response);
    #endregion

    #region -- Enums ---------------------------
    public enum WebUiBrowsers : ushort
    {
        AnyBrowser, // 0. Default recommended web browser
        Chrome, // 1. Google Chrome
        Firefox, // 2. Mozilla Firefox
        Edge, // 3. Microsoft Edge
        Safari, // 4. Apple Safari
        Chromium, // 5. The Chromium Project
        Opera, // 6. Opera Browser
        Brave, // 7. The Brave Browser
        Vivaldi, // 8. The Vivaldi Browser
        Epic, // 9. The Epic Browser
        Yandex, // 10. The Yandex Browser
    }

    public enum WebUiRuntimes : ushort
    {
        None, // 0. Prevent WebUI from using any runtime for .js and .ts files
        Deno, // 1. Use Deno runtime for .js and .ts files
        NodeJS, // 2. Use Nodejs runtime for .js files
    }

    public enum WebUiEvents : ushort
    {
        WEBUI_EVENT_DISCONNECTED, // 0. Window disconnection event
        WEBUI_EVENT_CONNECTED, // 1. Window connection event
        WEBUI_EVENT_MULTI_CONNECTION, // 2. New window connection event
        WEBUI_EVENT_UNWANTED_CONNECTION, // 3. New unwanted window connection event
        WEBUI_EVENT_MOUSE_CLICK, // 4. Mouse click event
        WEBUI_EVENT_NAVIGATION, // 5. Window navigation event
        WEBUI_EVENT_CALLBACK, // 6. Function call event
    }
    #endregion

    #region -- Structs -------------------------
    [StructLayout(LayoutKind.Sequential)]
    public struct webui_event_t
    {
        public IntPtr window; // Pointer to the window object
        public WebUiEvents type; // Event type
        public string element; // HTML element ID
        public IntPtr data; // JavaScript data
        public IntPtr response; // Callback response
    }
    #endregion

    internal class NativeMethods
    {
        #region Constants
        private const string DllFile = "webui-2-x64";

        public const string WEBUI_VERSION = "2.2.0";
        #endregion

        #region -- Definitions ---------------------
        // Create a new webui window object.
        [DllImport(DllFile, EntryPoint = "webui_new_window", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr webui_new_window();

        // Bind a specific html element click event with a function. Empty element means all events.
        [DllImport(DllFile, EntryPoint = "webui_bind", CallingConvention = CallingConvention.Cdecl)]
        public extern static ushort webui_bind([In, Out] IntPtr window, IntPtr element, IntPtr func);

        // Show a window using a embedded HTML, or a file. If the window is already opened then it will be refreshed.
        [DllImport(DllFile, EntryPoint = "webui_show", CallingConvention = CallingConvention.Cdecl)]
        public extern static bool webui_show([In, Out] IntPtr window, IntPtr content);

        // Same as webui_show(). But with a specific web browser.
        [DllImport(DllFile, EntryPoint = "webui_show_browser", CallingConvention = CallingConvention.Cdecl)]
        public extern static bool webui_show_browser([In, Out] IntPtr window, IntPtr content, ushort browser);

        // Wait until all opened windows get closed.
        [DllImport(DllFile, EntryPoint = "webui_wait", CallingConvention = CallingConvention.Cdecl)]
        public extern static void webui_wait();

        // Close a specific window.
        [DllImport(DllFile, EntryPoint = "webui_close", CallingConvention = CallingConvention.Cdecl)]
        public extern static void webui_close(IntPtr window);

        // Close all opened windows. webui_wait() will break.
        [DllImport(DllFile, EntryPoint = "webui_exit", CallingConvention = CallingConvention.Cdecl)]
        public extern static void webui_exit();
        #endregion

        #region -- Other ---------------------------
        // Check a specific window if it's still running
        [DllImport(DllFile, EntryPoint = "webui_is_shown", CallingConvention = CallingConvention.Cdecl)]
        public extern static bool webui_is_shown(IntPtr window);

        // Set the maximum time in seconds to wait for browser to start
        [DllImport(DllFile, EntryPoint = "webui_set_timeout", CallingConvention = CallingConvention.Cdecl)]
        public extern static void webui_set_timeout(ushort second);

        // Set the default embedded HTML favicon
        [DllImport(DllFile, EntryPoint = "webui_set_icon", CallingConvention = CallingConvention.Cdecl)]
        public extern static void webui_set_icon([In, Out] IntPtr window, IntPtr icon, IntPtr type);

        // Allow the window URL to be re-used in normal web browsers
        [DllImport(DllFile, EntryPoint = "webui_set_multi_access", CallingConvention = CallingConvention.Cdecl)]
        public extern static void webui_set_multi_access([In, Out] IntPtr window, bool status);
        #endregion

        #region -- JavaScript ----------------------
        // Run JavaScript quickly with no waiting for the response.
        [DllImport(DllFile, EntryPoint = "webui_run", CallingConvention = CallingConvention.Cdecl)]
        public extern static bool webui_run([In, Out] IntPtr window, IntPtr script);

        // Run a JavaScript, and get the response back (Make sure your local buffer can hold the response).
        [DllImport(DllFile, EntryPoint = "webui_script", CallingConvention = CallingConvention.Cdecl)]
        public extern static bool webui_script([In, Out] IntPtr window, IntPtr script, ushort timeout, ref IntPtr buffer, int buffer_length);

        // Chose between Deno and Nodejs runtime for .js and .ts files.
        [DllImport(DllFile, EntryPoint = "webui_set_runtime", CallingConvention = CallingConvention.Cdecl)]
        public extern static void webui_set_runtime([In, Out] IntPtr window, ushort runtime);

        // Parse argument as integer.
        [DllImport(DllFile, EntryPoint = "webui_get_int", CallingConvention = CallingConvention.Cdecl)]
        public extern static long webui_get_int(IntPtr e);

        // Parse argument as string.
        [DllImport(DllFile, EntryPoint = "webui_get_string", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr webui_get_string(IntPtr e);

        // Parse argument as boolean.
        [DllImport(DllFile, EntryPoint = "webui_get_bool", CallingConvention = CallingConvention.Cdecl)]
        public extern static bool webui_get_bool(IntPtr e);

        // Return the response to JavaScript as integer.
        [DllImport(DllFile, EntryPoint = "webui_return_int", CallingConvention = CallingConvention.Cdecl)]
        public extern static void webui_return_int(IntPtr e, long n);

        // Return the response to JavaScript as string.
        [DllImport(DllFile, EntryPoint = "webui_return_string", CallingConvention = CallingConvention.Cdecl)]
        public extern static void webui_return_string(IntPtr e, IntPtr s);

        // Return the response to JavaScript as boolean.
        [DllImport(DllFile, EntryPoint = "webui_return_bool", CallingConvention = CallingConvention.Cdecl)]
        public extern static void webui_return_bool(IntPtr e, bool b);
        #endregion

        #region -- Interface -----------------------
        // Bind a specific html element click event with a function. Empty element means all events. This replace webui_bind(). The func is (Window, EventType, Element, Data, Response)
        [DllImport(DllFile, EntryPoint = "webui_interface_bind", CallingConvention = CallingConvention.Cdecl)]
        public extern static ushort webui_interface_bind([In, Out] IntPtr window, IntPtr element, IntPtr func);

        // When using `webui_interface_bind()` you need this function to easily set your callback response.
        [DllImport(DllFile, EntryPoint = "webui_interface_set_response", CallingConvention = CallingConvention.Cdecl)]
        public extern static void webui_interface_set_response(IntPtr ptr, IntPtr response);

        // Check if the app still running or not. This replace webui_wait().
        [DllImport(DllFile, EntryPoint = "webui_interface_is_app_running", CallingConvention = CallingConvention.Cdecl)]
        public extern static bool webui_interface_is_app_running();

        // Get window unique ID
        [DllImport(DllFile, EntryPoint = "webui_interface_get_window_id", CallingConvention = CallingConvention.Cdecl)]
        public extern static ushort webui_interface_get_window_id(IntPtr window);
        #endregion
    }
}
