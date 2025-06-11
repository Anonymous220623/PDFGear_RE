// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.Compression.AcmDriverDetails
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace NAudio.Wave.Compression;

[StructLayout(LayoutKind.Sequential, Pack = 2)]
internal struct AcmDriverDetails
{
  public int structureSize;
  public uint fccType;
  public uint fccComp;
  public ushort manufacturerId;
  public ushort productId;
  public uint acmVersion;
  public uint driverVersion;
  public AcmDriverDetailsSupportFlags supportFlags;
  public int formatTagsCount;
  public int filterTagsCount;
  public IntPtr hicon;
  [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32 /*0x20*/)]
  public string shortName;
  [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128 /*0x80*/)]
  public string longName;
  [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80 /*0x50*/)]
  public string copyright;
  [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128 /*0x80*/)]
  public string licensing;
  [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512 /*0x0200*/)]
  public string features;
  private const int ShortNameChars = 32 /*0x20*/;
  private const int LongNameChars = 128 /*0x80*/;
  private const int CopyrightChars = 80 /*0x50*/;
  private const int LicensingChars = 128 /*0x80*/;
  private const int FeaturesChars = 512 /*0x0200*/;
}
