using System;
using System.Runtime.InteropServices;

namespace WebUiSharp
{
    public delegate void EventCallback(WebUiEvent e);

    public class WebUiEvent
    {
        #region Properties
        public string Element { get; private set; }

        public WebUiEvents EventType { get; private set; }

        public string Data { get; private set; }

        public string Response { get; private set; }
        #endregion

        #region Constructors
        internal WebUiEvent(IntPtr ptr)
        {
            Data = string.Empty;
            Response = string.Empty;

            if (ptr != IntPtr.Zero)
            {
                var ev = Marshal.PtrToStructure<webui_event_t>(ptr);
                EventType = ev.type;
                Element = ev.element;

                if (ev.data != IntPtr.Zero)
                    Data = Marshal.PtrToStringUTF8(ev.data);

                if (ev.response != IntPtr.Zero)
                    Response = Marshal.PtrToStringUTF8(ev.response);
            }
        }
        #endregion
    }
}
