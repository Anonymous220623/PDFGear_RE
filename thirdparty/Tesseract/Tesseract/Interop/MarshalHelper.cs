// Decompiled with JetBrains decompiler
// Type: Tesseract.Interop.MarshalHelper
// Assembly: Tesseract, Version=4.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: C5D5562D-D917-402B-968F-9F8B28C3D951
// Assembly location: D:\PDFGear\bin\Tesseract.dll

using System;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Tesseract.Interop;

internal static class MarshalHelper
{
  public static IntPtr StringToPtr(string value, Encoding encoding)
  {
    encoding.GetEncoder();
    byte[] numArray = new byte[encoding.GetByteCount(value) + 1];
    encoding.GetBytes(value, 0, value.Length, numArray, 0);
    IntPtr destination = Marshal.AllocHGlobal(new IntPtr(numArray.Length));
    Marshal.Copy(numArray, 0, destination, numArray.Length);
    return destination;
  }

  public static unsafe string PtrToString(IntPtr handle, Encoding encoding)
  {
    int length = MarshalHelper.StrLength(handle);
    return new string((sbyte*) handle.ToPointer(), 0, length, encoding);
  }

  public static unsafe int StrLength(IntPtr handle)
  {
    if (handle == IntPtr.Zero)
      return 0;
    byte* pointer = (byte*) handle.ToPointer();
    int index = 0;
    while (pointer[index] != (byte) 0)
      ++index;
    return index;
  }
}
