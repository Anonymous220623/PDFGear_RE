// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.Compression.AcmFormatChoose
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace NAudio.Wave.Compression;

[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Auto)]
internal struct AcmFormatChoose
{
  public int structureSize;
  public AcmFormatChooseStyleFlags styleFlags;
  public IntPtr ownerWindowHandle;
  public IntPtr selectedWaveFormatPointer;
  public int selectedWaveFormatByteSize;
  [MarshalAs(UnmanagedType.LPTStr)]
  public string title;
  [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48 /*0x30*/)]
  public string formatTagDescription;
  [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128 /*0x80*/)]
  public string formatDescription;
  [MarshalAs(UnmanagedType.LPTStr)]
  public string name;
  public int nameByteSize;
  public AcmFormatEnumFlags formatEnumFlags;
  public IntPtr waveFormatEnumPointer;
  public IntPtr instanceHandle;
  [MarshalAs(UnmanagedType.LPTStr)]
  public string templateName;
  public IntPtr customData;
  public AcmInterop.AcmFormatChooseHookProc windowCallbackFunction;
}
