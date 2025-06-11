// Decompiled with JetBrains decompiler
// Type: NLog.Internal.FileAppenders.ICreateFileParameters
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Targets;

#nullable disable
namespace NLog.Internal.FileAppenders;

internal interface ICreateFileParameters
{
  int ConcurrentWriteAttemptDelay { get; }

  int ConcurrentWriteAttempts { get; }

  bool ConcurrentWrites { get; }

  bool CreateDirs { get; }

  bool EnableFileDelete { get; }

  int BufferSize { get; }

  bool ForceManaged { get; }

  Win32FileAttributes FileAttributes { get; }

  bool IsArchivingEnabled { get; }

  bool EnableFileDeleteSimpleMonitor { get; }
}
