// Decompiled with JetBrains decompiler
// Type: NLog.Internal.FileAppenders.IFileAppenderCache
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;

#nullable disable
namespace NLog.Internal.FileAppenders;

internal interface IFileAppenderCache : IDisposable
{
  ICreateFileParameters CreateFileParameters { get; }

  IFileAppenderFactory Factory { get; }

  int Size { get; }

  event EventHandler CheckCloseAppenders;

  BaseFileAppender AllocateAppender(string fileName);

  void CloseAppenders(string reason);

  void CloseAppenders(DateTime expireTime);

  void FlushAppenders();

  DateTime? GetFileCreationTimeSource(string filePath, DateTime? fallbackTimeSource = null);

  DateTime? GetFileLastWriteTimeUtc(string filePath);

  long? GetFileLength(string filePath);

  BaseFileAppender InvalidateAppender(string filePath);

  string ArchiveFilePatternToWatch { get; set; }

  void InvalidateAppendersForArchivedFiles();
}
