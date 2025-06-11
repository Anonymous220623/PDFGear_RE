// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.Compression.AcmFormatTagDetails
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using System.Runtime.InteropServices;

#nullable disable
namespace NAudio.Wave.Compression;

internal struct AcmFormatTagDetails
{
  public int structureSize;
  public int formatTagIndex;
  public int formatTag;
  public int formatSize;
  public AcmDriverDetailsSupportFlags supportFlags;
  public int standardFormatsCount;
  [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48 /*0x30*/)]
  public string formatDescription;
  public const int FormatTagDescriptionChars = 48 /*0x30*/;
}
