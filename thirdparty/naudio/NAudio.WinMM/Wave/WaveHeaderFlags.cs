// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.WaveHeaderFlags
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using System;

#nullable disable
namespace NAudio.Wave;

[Flags]
public enum WaveHeaderFlags
{
  BeginLoop = 4,
  Done = 1,
  EndLoop = 8,
  InQueue = 16, // 0x00000010
  Prepared = 2,
}
