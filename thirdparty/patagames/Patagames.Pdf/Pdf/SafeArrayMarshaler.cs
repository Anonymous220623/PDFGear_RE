// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.SafeArrayMarshaler
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

internal class SafeArrayMarshaler : ICustomMarshaler
{
  private static ICustomMarshaler marshaler = (ICustomMarshaler) new SafeArrayMarshaler();

  public static ICustomMarshaler GetInstance(string cookie) => SafeArrayMarshaler.marshaler;

  public object MarshalNativeToManaged(IntPtr pNativeData)
  {
    if (pNativeData == IntPtr.Zero)
      return (object) null;
    byte[] data;
    ((SafeArrayMarshaler.safearray) Marshal.PtrToStructure(pNativeData, typeof (SafeArrayMarshaler.safearray))).GetData(out data);
    return (object) data;
  }

  public IntPtr MarshalManagedToNative(object ManagedObj)
  {
    if (ManagedObj == null || !(ManagedObj is byte[]))
      return IntPtr.Zero;
    SafeArrayMarshaler.safearray structure = new SafeArrayMarshaler.safearray(ManagedObj as byte[]);
    IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf<SafeArrayMarshaler.safearray>(structure));
    Marshal.StructureToPtr<SafeArrayMarshaler.safearray>(structure, ptr, false);
    return ptr;
  }

  public void CleanUpManagedData(object ManagedObj)
  {
  }

  public void CleanUpNativeData(IntPtr pNativeData)
  {
    if (pNativeData == IntPtr.Zero)
      return;
    Marshal.PtrToStructure<SafeArrayMarshaler.safearray>(pNativeData).FreeData();
    Marshal.FreeHGlobal(pNativeData);
  }

  public int GetNativeDataSize() => -1;

  private struct safearraybound
  {
    public long cElements;
    public long lLbound;

    public safearraybound(byte[] data)
    {
      this.cElements = (long) data.Length;
      this.lLbound = 0L;
    }
  }

  private struct safearray
  {
    public short cDims;
    public short fFeatures;
    public short cbElements;
    public short cLocks;
    public short handle;
    public IntPtr pvData;
    public SafeArrayMarshaler.safearraybound rgsabound;

    public safearray(byte[] data)
    {
      this.cDims = (short) 1;
      this.fFeatures = (short) 2;
      this.cbElements = (short) 1;
      this.cLocks = (short) 0;
      this.handle = (short) 0;
      this.pvData = IntPtr.Zero;
      int length = data.Length;
      if (length >= 0)
      {
        this.pvData = Marshal.AllocHGlobal(length);
        Marshal.Copy(data, 0, this.pvData, length);
      }
      this.rgsabound = new SafeArrayMarshaler.safearraybound(data);
    }

    public void GetData(out byte[] data)
    {
      data = (byte[]) null;
      if (this.pvData == IntPtr.Zero)
        return;
      long cElements = this.rgsabound.cElements;
      if (cElements < 0L)
        return;
      data = new byte[cElements];
      if (cElements <= 0L)
        return;
      Marshal.Copy(this.pvData, data, 0, (int) cElements);
    }

    public void FreeData()
    {
      if (!(this.pvData != IntPtr.Zero))
        return;
      Marshal.FreeHGlobal(this.pvData);
    }
  }
}
