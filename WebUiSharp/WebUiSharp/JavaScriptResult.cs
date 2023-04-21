using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace WebUiSharp
{
    public class JavaScriptResult
    {
        #region Constructors
        internal JavaScriptResult(IntPtr handle)
        {
            if (handle != IntPtr.Zero)
            {
                var result = Marshal.PtrToStructure<webui_javascript_result>(handle);
                Error = result.error;
                Length = result.length;

                if (result.data != IntPtr.Zero)
                    Data = Marshal.PtrToStringUTF8(result.data);
                else
                    Data = string.Empty;
            }
            else
            {
                Error = true;
                Length = 0;
                Data = string.Empty;
            }
        }
        #endregion

        #region Properties
        public bool Error { get; private set; }

        public uint Length { get; private set; }

        public string Data { get; private set; }
        #endregion

        #region Methods
        internal static JavaScriptResult Empty
        {
            get => new JavaScriptResult(IntPtr.Zero);
        }
        #endregion
    }
}
