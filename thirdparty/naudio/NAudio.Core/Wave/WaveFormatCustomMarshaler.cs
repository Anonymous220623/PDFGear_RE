// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.WaveFormatCustomMarshaler
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace NAudio.Wave;

public sealed class WaveFormatCustomMarshaler : ICustomMarshaler
{
  private static WaveFormatCustomMarshaler marshaler;

  public static ICustomMarshaler GetInstance(string cookie)
  {
    if (WaveFormatCustomMarshaler.marshaler == null)
      WaveFormatCustomMarshaler.marshaler = new WaveFormatCustomMarshaler();
    return (ICustomMarshaler) WaveFormatCustomMarshaler.marshaler;
  }

  public void CleanUpManagedData(object ManagedObj)
  {
  }

  public void CleanUpNativeData(IntPtr pNativeData) => Marshal.FreeHGlobal(pNativeData);

  public int GetNativeDataSize() => throw new NotImplementedException();

  public IntPtr MarshalManagedToNative(object ManagedObj)
  {
    return WaveFormat.MarshalToPtr((WaveFormat) ManagedObj);
  }

  public object MarshalNativeToManaged(IntPtr pNativeData)
  {
    return (object) WaveFormat.MarshalFromPtr(pNativeData);
  }
}
