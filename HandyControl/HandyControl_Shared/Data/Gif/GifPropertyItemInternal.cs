// Decompiled with JetBrains decompiler
// Type: HandyControl.Data.GifPropertyItemInternal
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools.Interop;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace HandyControl.Data;

[StructLayout(LayoutKind.Sequential)]
public class GifPropertyItemInternal : IDisposable
{
  public int id;
  public int len;
  public short type;
  public IntPtr value = IntPtr.Zero;

  internal GifPropertyItemInternal()
  {
  }

  public byte[] Value
  {
    get
    {
      if (this.len == 0)
        return (byte[]) null;
      byte[] destination = new byte[this.len];
      Marshal.Copy(this.value, destination, 0, this.len);
      return destination;
    }
  }

  public void Dispose() => this.Dispose(true);

  ~GifPropertyItemInternal() => this.Dispose(false);

  private void Dispose(bool disposing)
  {
    if (this.value != IntPtr.Zero)
    {
      Marshal.FreeHGlobal(this.value);
      this.value = IntPtr.Zero;
    }
    if (!disposing)
      return;
    GC.SuppressFinalize((object) this);
  }

  internal static GifPropertyItemInternal ConvertFromPropertyItem(GifPropertyItem propItem)
  {
    GifPropertyItemInternal propertyItemInternal = new GifPropertyItemInternal()
    {
      id = propItem.Id,
      len = 0,
      type = propItem.Type
    };
    byte[] source = propItem.Value;
    if (source != null)
    {
      int length = source.Length;
      propertyItemInternal.len = length;
      propertyItemInternal.value = Marshal.AllocHGlobal(length);
      Marshal.Copy(source, 0, propertyItemInternal.value, length);
    }
    return propertyItemInternal;
  }

  internal static GifPropertyItem[] ConvertFromMemory(IntPtr propdata, int count)
  {
    GifPropertyItem[] gifPropertyItemArray = new GifPropertyItem[count];
    for (int index = 0; index < count; ++index)
    {
      GifPropertyItemInternal propertyItemInternal = (GifPropertyItemInternal) null;
      try
      {
        propertyItemInternal = (GifPropertyItemInternal) InteropMethods.PtrToStructure(propdata, typeof (GifPropertyItemInternal));
        gifPropertyItemArray[index] = new GifPropertyItem()
        {
          Id = propertyItemInternal.id,
          Len = propertyItemInternal.len,
          Type = propertyItemInternal.type,
          Value = propertyItemInternal.Value
        };
        propertyItemInternal.value = IntPtr.Zero;
      }
      finally
      {
        propertyItemInternal?.Dispose();
      }
      propdata = (IntPtr) ((long) propdata + (long) Marshal.SizeOf(typeof (GifPropertyItemInternal)));
    }
    return gifPropertyItemArray;
  }
}
