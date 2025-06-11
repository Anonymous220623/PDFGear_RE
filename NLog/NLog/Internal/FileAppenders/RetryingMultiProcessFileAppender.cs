// Decompiled with JetBrains decompiler
// Type: NLog.Internal.FileAppenders.RetryingMultiProcessFileAppender
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.IO;
using System.Security;

#nullable disable
namespace NLog.Internal.FileAppenders;

[SecuritySafeCritical]
internal class RetryingMultiProcessFileAppender(string fileName, ICreateFileParameters parameters) : 
  BaseMutexFileAppender(fileName, parameters)
{
  public static readonly IFileAppenderFactory TheFactory = (IFileAppenderFactory) new RetryingMultiProcessFileAppender.Factory();

  public override void Write(byte[] bytes, int offset, int count)
  {
    using (FileStream fileStream = this.CreateFileStream(false, Math.Min((count / 4096 /*0x1000*/ + 1) * 4096 /*0x1000*/, this.CreateFileParameters.BufferSize)))
      fileStream.Write(bytes, offset, count);
  }

  public override void Flush()
  {
  }

  public override void Close()
  {
  }

  public override DateTime? GetFileCreationTimeUtc()
  {
    FileInfo fileInfo = new FileInfo(this.FileName);
    return fileInfo.Exists ? new DateTime?(fileInfo.LookupValidFileCreationTimeUtc()) : new DateTime?();
  }

  public override long? GetFileLength()
  {
    FileInfo fileInfo = new FileInfo(this.FileName);
    return fileInfo.Exists ? new long?(fileInfo.Length) : new long?();
  }

  private class Factory : IFileAppenderFactory
  {
    BaseFileAppender IFileAppenderFactory.Open(string fileName, ICreateFileParameters parameters)
    {
      return (BaseFileAppender) new RetryingMultiProcessFileAppender(fileName, parameters);
    }
  }
}
