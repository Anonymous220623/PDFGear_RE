// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.Compression.AcmStreamOpenFlags
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using System;

#nullable disable
namespace NAudio.Wave.Compression;

[Flags]
internal enum AcmStreamOpenFlags
{
  Query = 1,
  Async = 2,
  NonRealTime = 4,
  CallbackTypeMask = 458752, // 0x00070000
  CallbackNull = 0,
  CallbackWindow = 65536, // 0x00010000
  CallbackTask = 131072, // 0x00020000
  CallbackFunction = CallbackTask | CallbackWindow, // 0x00030000
  CallbackThread = CallbackTask, // 0x00020000
  CallbackEvent = 327680, // 0x00050000
}
