// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.Compression.AcmFormatDetails
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace NAudio.Wave.Compression;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
internal struct AcmFormatDetails
{
  public int structSize;
  public int formatIndex;
  public int formatTag;
  public AcmDriverDetailsSupportFlags supportFlags;
  public IntPtr waveFormatPointer;
  public int waveFormatByteSize;
  [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128 /*0x80*/)]
  public string formatDescription;
  public const int FormatDescriptionChars = 128 /*0x80*/;
}
