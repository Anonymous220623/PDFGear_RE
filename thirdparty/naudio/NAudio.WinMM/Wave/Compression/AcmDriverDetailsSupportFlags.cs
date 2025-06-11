// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.Compression.AcmDriverDetailsSupportFlags
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using System;

#nullable disable
namespace NAudio.Wave.Compression;

[Flags]
public enum AcmDriverDetailsSupportFlags
{
  Codec = 1,
  Converter = 2,
  Filter = 4,
  Hardware = 8,
  Async = 16, // 0x00000010
  Local = 1073741824, // 0x40000000
  Disabled = -2147483648, // 0x80000000
}
