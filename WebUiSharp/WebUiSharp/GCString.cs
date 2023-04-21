using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WebUiSharp
{
    public class GCString : IDisposable
    {
        #region Variables
        private readonly GCHandle handle;
        private readonly EncodingTypes encodingType;
        #endregion

        #region Constructors
        public GCString(string str)
            : this(str, EncodingTypes.Ansi)
        {   
        }

        public GCString(string str, EncodingTypes encodingType)
        {
            this.encodingType = encodingType;

            Encoding encoding;
            switch (encodingType)
            {
                case EncodingTypes.Unicode:
                    encoding = Encoding.Unicode;
                    break;
                case EncodingTypes.UTF8:
                    encoding = Encoding.UTF8;
                    break;
                default:
                    encoding = Encoding.ASCII;
                    break;
            }

            byte[] buffer = encoding.GetBytes(str ?? "");
            handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        }
        #endregion

        #region Properties
        public GCHandle Handle
        {
            get => handle;
        }
        #endregion

        #region Methods
        public IntPtr ToPointer()
        {
            if (!handle.IsAllocated) return IntPtr.Zero;
            return handle.AddrOfPinnedObject();
        }

        public override int GetHashCode()
        {
            return handle.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return handle.Equals(obj);
        }

        public override string ToString()
        {
            if (handle.IsAllocated)
            {
                IntPtr ptr = ToPointer();
                switch (encodingType)
                {
                    case EncodingTypes.Unicode:
                        return Marshal.PtrToStringUni(ptr);
                    case EncodingTypes.UTF8:
                        return Marshal.PtrToStringUTF8(ptr);
                    default:
                        return Marshal.PtrToStringAnsi(ptr);
                }
            }

            return base.ToString();
        }

        public void Dispose()
        {
            if (handle.IsAllocated)
                handle.Free();
        }

        ~GCString()
        {
            Dispose();
        }
        #endregion

        #region Operators
        public static implicit operator GCString(string value)
        {
            return new GCString(value);
        }

        public static explicit operator IntPtr(GCString value)
        {
            return value.ToPointer();
        }
        #endregion

        #region Nested Types
        public enum EncodingTypes
        {
            Ansi,
            Unicode,
            UTF8
        }
        #endregion
    }
}
