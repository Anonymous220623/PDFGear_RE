// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.Compression.AcmFormatEnumFlags
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using System;

#nullable disable
namespace NAudio.Wave.Compression;

[Flags]
public enum AcmFormatEnumFlags
{
  None = 0,
  Convert = 1048576, // 0x00100000
  Hardware = 4194304, // 0x00400000
  Input = 8388608, // 0x00800000
  Channels = 131072, // 0x00020000
  SamplesPerSecond = 262144, // 0x00040000
  Output = 16777216, // 0x01000000
  Suggest = 2097152, // 0x00200000
  BitsPerSample = 524288, // 0x00080000
  FormatTag = 65536, // 0x00010000
}
