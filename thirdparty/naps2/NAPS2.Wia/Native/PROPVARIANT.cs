// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.Native.PROPVARIANT
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using System;
using System.Runtime.InteropServices;

#nullable enable
namespace NAPS2.Wia.Native;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
internal struct PROPVARIANT
{
  [FieldOffset(0)]
  internal ushort varType;
  [FieldOffset(2)]
  internal ushort wReserved1;
  [FieldOffset(4)]
  internal ushort wReserved2;
  [FieldOffset(6)]
  internal ushort wReserved3;
  [FieldOffset(8)]
  internal byte bVal;
  [FieldOffset(8)]
  internal sbyte cVal;
  [FieldOffset(8)]
  internal ushort uiVal;
  [FieldOffset(8)]
  internal short iVal;
  [FieldOffset(8)]
  internal uint uintVal;
  [FieldOffset(8)]
  internal int intVal;
  [FieldOffset(8)]
  internal ulong ulVal;
  [FieldOffset(8)]
  internal long lVal;
  [FieldOffset(8)]
  internal float fltVal;
  [FieldOffset(8)]
  internal double dblVal;
  [FieldOffset(8)]
  internal short boolVal;
  [FieldOffset(8)]
  internal IntPtr pclsidVal;
  [FieldOffset(8)]
  internal IntPtr pszVal;
  [FieldOffset(8)]
  internal IntPtr pwszVal;
  [FieldOffset(8)]
  internal IntPtr punkVal;
  [FieldOffset(8)]
  internal PROPARRAY ca;
  [FieldOffset(8)]
  internal System.Runtime.InteropServices.ComTypes.FILETIME filetime;

  private static unsafe void CopyBytes(byte* pbTo, int cbTo, byte* pbFrom, int cbFrom)
  {
    if (cbFrom > cbTo)
      throw new InvalidOperationException();
    byte* numPtr1 = pbFrom;
    byte* numPtr2 = pbTo;
    for (int index = 0; index < cbFrom; ++index)
      numPtr2[index] = numPtr1[index];
  }

  internal void InitVector(Array array, Type type, VarEnum varEnum)
  {
    this.Init(array, type, varEnum | VarEnum.VT_VECTOR);
  }

  internal unsafe void Init(Array array, Type type, VarEnum vt)
  {
    this.varType = (ushort) vt;
    this.ca.cElems = 0U;
    this.ca.pElems = IntPtr.Zero;
    int length = array.Length;
    if (length <= 0)
      return;
    long num1 = (long) (Marshal.SizeOf(type) * length);
    IntPtr num2 = IntPtr.Zero;
    GCHandle gcHandle = new GCHandle();
    try
    {
      num2 = Marshal.AllocCoTaskMem((int) num1);
      gcHandle = GCHandle.Alloc((object) array, GCHandleType.Pinned);
      PROPVARIANT.CopyBytes((byte*) (void*) num2, (int) num1, (byte*) (void*) gcHandle.AddrOfPinnedObject(), (int) num1);
      this.ca.cElems = (uint) length;
      this.ca.pElems = num2;
      num2 = IntPtr.Zero;
    }
    finally
    {
      if (gcHandle.IsAllocated)
        gcHandle.Free();
      if (num2 != IntPtr.Zero)
        Marshal.FreeCoTaskMem(num2);
    }
  }

  internal void Init(string[] value, bool fAscii)
  {
    this.varType = fAscii ? (ushort) 30 : (ushort) 31 /*0x1F*/;
    this.varType |= (ushort) 4096 /*0x1000*/;
    this.ca.cElems = 0U;
    this.ca.pElems = IntPtr.Zero;
    int length = value.Length;
    if (length <= 0)
      return;
    IntPtr ptr = IntPtr.Zero;
    int num = sizeof (IntPtr);
    long cb = (long) (num * length);
    int index1 = 0;
    try
    {
      IntPtr zero = IntPtr.Zero;
      ptr = Marshal.AllocCoTaskMem((int) cb);
      for (index1 = 0; index1 < length; ++index1)
      {
        IntPtr val = !fAscii ? Marshal.StringToCoTaskMemUni(value[index1]) : Marshal.StringToCoTaskMemAnsi(value[index1]);
        Marshal.WriteIntPtr(ptr, index1 * num, val);
      }
      this.ca.cElems = (uint) length;
      this.ca.pElems = ptr;
      ptr = IntPtr.Zero;
    }
    finally
    {
      if (ptr != IntPtr.Zero)
      {
        for (int index2 = 0; index2 < index1; ++index2)
          Marshal.FreeCoTaskMem(Marshal.ReadIntPtr(ptr, index2 * num));
        Marshal.FreeCoTaskMem(ptr);
      }
    }
  }

  internal void Init(object value)
  {
    if (value == null)
      this.varType = (ushort) 0;
    else if (value is Array)
    {
      Type type = value.GetType();
      if (type == typeof (sbyte[]))
        this.InitVector(value as Array, typeof (sbyte), VarEnum.VT_I1);
      else if (type == typeof (byte[]))
        this.InitVector(value as Array, typeof (byte), VarEnum.VT_UI1);
      else if (value is char[])
      {
        this.varType = (ushort) 30;
        this.pszVal = Marshal.StringToCoTaskMemAnsi(new string(value as char[]));
      }
      else if (value is char[][])
      {
        char[][] chArray = value as char[][];
        string[] strArray = new string[chArray.GetLength(0)];
        for (int index = 0; index < chArray.Length; ++index)
          strArray[index] = new string(chArray[index]);
        this.Init(strArray, true);
      }
      else if (type == typeof (short[]))
        this.InitVector(value as Array, typeof (short), VarEnum.VT_I2);
      else if (type == typeof (ushort[]))
        this.InitVector(value as Array, typeof (ushort), VarEnum.VT_UI2);
      else if (type == typeof (int[]))
        this.InitVector(value as Array, typeof (int), VarEnum.VT_I4);
      else if (type == typeof (uint[]))
        this.InitVector(value as Array, typeof (uint), VarEnum.VT_UI4);
      else if (type == typeof (long[]))
        this.InitVector(value as Array, typeof (long), VarEnum.VT_I8);
      else if (type == typeof (ulong[]))
      {
        this.InitVector(value as Array, typeof (ulong), VarEnum.VT_UI8);
      }
      else
      {
        switch (value)
        {
          case float[] _:
            this.InitVector(value as Array, typeof (float), VarEnum.VT_R4);
            break;
          case double[] _:
            this.InitVector(value as Array, typeof (double), VarEnum.VT_R8);
            break;
          case Guid[] _:
            this.InitVector(value as Array, typeof (Guid), VarEnum.VT_CLSID);
            break;
          case string[] _:
            this.Init(value as string[], false);
            break;
          case bool[] _:
            bool[] flagArray = value as bool[];
            short[] numArray = new short[flagArray.Length];
            for (int index = 0; index < flagArray.Length; ++index)
              numArray[index] = flagArray[index] ? (short) -1 : (short) 0;
            this.InitVector((Array) numArray, typeof (short), VarEnum.VT_BOOL);
            break;
          default:
            throw new InvalidOperationException();
        }
      }
    }
    else
    {
      Type type = value.GetType();
      if (value is string)
      {
        this.varType = (ushort) 31 /*0x1F*/;
        this.pwszVal = Marshal.StringToCoTaskMemUni(value as string);
      }
      else if (type == typeof (sbyte))
      {
        this.varType = (ushort) 16 /*0x10*/;
        this.cVal = (sbyte) value;
      }
      else if (type == typeof (byte))
      {
        this.varType = (ushort) 17;
        this.bVal = (byte) value;
      }
      else if (type == typeof (System.Runtime.InteropServices.ComTypes.FILETIME))
      {
        this.varType = (ushort) 64 /*0x40*/;
        this.filetime = (System.Runtime.InteropServices.ComTypes.FILETIME) value;
      }
      else if (value is char)
      {
        this.varType = (ushort) 30;
        this.pszVal = Marshal.StringToCoTaskMemAnsi(new string(value as char[]));
      }
      else if (type == typeof (short))
      {
        this.varType = (ushort) 2;
        this.iVal = (short) value;
      }
      else if (type == typeof (ushort))
      {
        this.varType = (ushort) 18;
        this.uiVal = (ushort) value;
      }
      else if (type == typeof (int))
      {
        this.varType = (ushort) 3;
        this.intVal = (int) value;
      }
      else if (type == typeof (uint))
      {
        this.varType = (ushort) 19;
        this.uintVal = (uint) value;
      }
      else if (type == typeof (long))
      {
        this.varType = (ushort) 20;
        this.lVal = (long) value;
      }
      else if (type == typeof (ulong))
      {
        this.varType = (ushort) 21;
        this.ulVal = (ulong) value;
      }
      else
      {
        switch (value)
        {
          case float num1:
            this.varType = (ushort) 4;
            this.fltVal = num1;
            break;
          case double num2:
            this.varType = (ushort) 5;
            this.dblVal = num2;
            break;
          case Guid guid:
            byte[] byteArray = guid.ToByteArray();
            this.varType = (ushort) 72;
            this.pclsidVal = Marshal.AllocCoTaskMem(byteArray.Length);
            Marshal.Copy(byteArray, 0, this.pclsidVal, byteArray.Length);
            break;
          case bool flag:
            this.varType = (ushort) 11;
            this.boolVal = flag ? (short) -1 : (short) 0;
            break;
          default:
            throw new InvalidOperationException();
        }
      }
    }
  }

  internal void Clear()
  {
    VarEnum varType = (VarEnum) this.varType;
    if ((varType & VarEnum.VT_VECTOR) == VarEnum.VT_EMPTY)
    {
      switch (varType)
      {
        case VarEnum.VT_UNKNOWN:
          Marshal.Release(this.punkVal);
          goto label_13;
        case VarEnum.VT_LPSTR:
        case VarEnum.VT_LPWSTR:
        case VarEnum.VT_CLSID:
          Marshal.FreeCoTaskMem(this.pwszVal);
          goto label_13;
        case VarEnum.VT_BLOB:
          break;
        default:
          goto label_13;
      }
    }
    if (this.ca.pElems != IntPtr.Zero)
    {
      switch (varType & ~VarEnum.VT_VECTOR)
      {
        case VarEnum.VT_UNKNOWN:
          IntPtr pElems1 = this.ca.pElems;
          int num1 = sizeof (IntPtr);
          for (uint index = 0; index < this.ca.cElems; ++index)
            Marshal.Release(Marshal.ReadIntPtr(pElems1, (int) ((long) index * (long) num1)));
          break;
        case VarEnum.VT_LPSTR:
        case VarEnum.VT_LPWSTR:
          IntPtr pElems2 = this.ca.pElems;
          int num2 = sizeof (IntPtr);
          for (uint index = 0; index < this.ca.cElems; ++index)
            Marshal.FreeCoTaskMem(Marshal.ReadIntPtr(pElems2, (int) ((long) index * (long) num2)));
          break;
      }
      Marshal.FreeCoTaskMem(this.ca.pElems);
    }
label_13:;
  }

  internal object ToObject(object syncObject)
  {
    VarEnum varType = (VarEnum) this.varType;
    if ((varType & VarEnum.VT_VECTOR) != VarEnum.VT_EMPTY)
    {
      switch (varType & ~VarEnum.VT_VECTOR)
      {
        case VarEnum.VT_EMPTY:
          return (object) null;
        case VarEnum.VT_I2:
          short[] destination1 = new short[(int) this.ca.cElems];
          Marshal.Copy(this.ca.pElems, destination1, 0, (int) this.ca.cElems);
          return (object) destination1;
        case VarEnum.VT_I4:
          int[] destination2 = new int[(int) this.ca.cElems];
          Marshal.Copy(this.ca.pElems, destination2, 0, (int) this.ca.cElems);
          return (object) destination2;
        case VarEnum.VT_R4:
          float[] destination3 = new float[(int) this.ca.cElems];
          Marshal.Copy(this.ca.pElems, destination3, 0, (int) this.ca.cElems);
          return (object) destination3;
        case VarEnum.VT_R8:
          double[] destination4 = new double[(int) this.ca.cElems];
          Marshal.Copy(this.ca.pElems, destination4, 0, (int) this.ca.cElems);
          return (object) destination4;
        case VarEnum.VT_BOOL:
          bool[] flagArray = new bool[(int) this.ca.cElems];
          for (int index = 0; (long) index < (long) this.ca.cElems; ++index)
            flagArray[index] = Marshal.ReadInt16(this.ca.pElems, index * 2) != (short) 0;
          return (object) flagArray;
        case VarEnum.VT_I1:
          sbyte[] numArray1 = new sbyte[(int) this.ca.cElems];
          for (int ofs = 0; (long) ofs < (long) this.ca.cElems; ++ofs)
            numArray1[ofs] = (sbyte) Marshal.ReadByte(this.ca.pElems, ofs);
          return (object) numArray1;
        case VarEnum.VT_UI1:
          byte[] destination5 = new byte[(int) this.ca.cElems];
          Marshal.Copy(this.ca.pElems, destination5, 0, (int) this.ca.cElems);
          return (object) destination5;
        case VarEnum.VT_UI2:
          ushort[] numArray2 = new ushort[(int) this.ca.cElems];
          for (int index = 0; (long) index < (long) this.ca.cElems; ++index)
            numArray2[index] = (ushort) Marshal.ReadInt16(this.ca.pElems, index * 2);
          return (object) numArray2;
        case VarEnum.VT_UI4:
          uint[] numArray3 = new uint[(int) this.ca.cElems];
          for (int index = 0; (long) index < (long) this.ca.cElems; ++index)
            numArray3[index] = (uint) Marshal.ReadInt32(this.ca.pElems, index * 4);
          return (object) numArray3;
        case VarEnum.VT_I8:
          long[] destination6 = new long[(int) this.ca.cElems];
          Marshal.Copy(this.ca.pElems, destination6, 0, (int) this.ca.cElems);
          return (object) destination6;
        case VarEnum.VT_UI8:
          ulong[] numArray4 = new ulong[(int) this.ca.cElems];
          for (int index = 0; (long) index < (long) this.ca.cElems; ++index)
            numArray4[index] = (ulong) Marshal.ReadInt64(this.ca.pElems, index * 8);
          return (object) numArray4;
        case VarEnum.VT_LPSTR:
          string[] strArray1 = new string[(int) this.ca.cElems];
          int num1 = sizeof (IntPtr);
          for (int index = 0; (long) index < (long) this.ca.cElems; ++index)
          {
            IntPtr ptr = Marshal.ReadIntPtr(this.ca.pElems, index * num1);
            strArray1[index] = Marshal.PtrToStringAnsi(ptr);
          }
          return (object) strArray1;
        case VarEnum.VT_LPWSTR:
          string[] strArray2 = new string[(int) this.ca.cElems];
          int num2 = sizeof (IntPtr);
          for (int index = 0; (long) index < (long) this.ca.cElems; ++index)
          {
            IntPtr ptr = Marshal.ReadIntPtr(this.ca.pElems, index * num2);
            strArray2[index] = Marshal.PtrToStringUni(ptr);
          }
          return (object) strArray2;
        case VarEnum.VT_CLSID:
          Guid[] guidArray = new Guid[(int) this.ca.cElems];
          for (int index = 0; (long) index < (long) this.ca.cElems; ++index)
          {
            byte[] numArray5 = new byte[16 /*0x10*/];
            Marshal.Copy(this.ca.pElems, numArray5, index * 16 /*0x10*/, 16 /*0x10*/);
            guidArray[index] = new Guid(numArray5);
          }
          return (object) guidArray;
      }
    }
    else
    {
      switch (varType)
      {
        case VarEnum.VT_EMPTY:
          return (object) null;
        case VarEnum.VT_I2:
          return (object) this.iVal;
        case VarEnum.VT_I4:
          return (object) this.intVal;
        case VarEnum.VT_R4:
          return (object) this.fltVal;
        case VarEnum.VT_R8:
          return (object) this.dblVal;
        case VarEnum.VT_BOOL:
          return (object) (this.boolVal != (short) 0);
        case VarEnum.VT_I1:
          return (object) this.cVal;
        case VarEnum.VT_UI1:
          return (object) this.bVal;
        case VarEnum.VT_UI2:
          return (object) this.uiVal;
        case VarEnum.VT_UI4:
          return (object) this.uintVal;
        case VarEnum.VT_I8:
          return (object) this.lVal;
        case VarEnum.VT_UI8:
          return (object) this.ulVal;
        case VarEnum.VT_LPSTR:
          return (object) Marshal.PtrToStringAnsi(this.pszVal);
        case VarEnum.VT_LPWSTR:
          return (object) Marshal.PtrToStringUni(this.pwszVal);
        case VarEnum.VT_FILETIME:
          return (object) this.filetime;
        case VarEnum.VT_CLSID:
          byte[] numArray6 = new byte[16 /*0x10*/];
          Marshal.Copy(this.pclsidVal, numArray6, 0, 16 /*0x10*/);
          return (object) new Guid(numArray6);
      }
    }
    throw new NotSupportedException();
  }

  internal bool RequiresSyncObject => this.varType == (ushort) 13;
}
