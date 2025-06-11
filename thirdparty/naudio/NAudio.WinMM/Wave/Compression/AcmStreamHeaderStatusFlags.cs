// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.Compression.AcmStreamHeaderStatusFlags
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using System;

#nullable disable
namespace NAudio.Wave.Compression;

[Flags]
internal enum AcmStreamHeaderStatusFlags
{
  Done = 65536, // 0x00010000
  Prepared = 131072, // 0x00020000
  InQueue = 1048576, // 0x00100000
}
