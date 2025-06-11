// Decompiled with JetBrains decompiler
// Type: NLog.Internal.FileAppenders.NullAppender
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Security;

#nullable disable
namespace NLog.Internal.FileAppenders;

[SecuritySafeCritical]
internal class NullAppender(string fileName, ICreateFileParameters createParameters) : 
  BaseFileAppender(fileName, createParameters)
{
  public static readonly IFileAppenderFactory TheFactory = (IFileAppenderFactory) new NullAppender.Factory();

  public override void Close()
  {
  }

  public override void Flush()
  {
  }

  public override DateTime? GetFileCreationTimeUtc() => new DateTime?(DateTime.UtcNow);

  public override long? GetFileLength() => new long?(0L);

  public override void Write(byte[] bytes, int offset, int count)
  {
  }

  private class Factory : IFileAppenderFactory
  {
    BaseFileAppender IFileAppenderFactory.Open(string fileName, ICreateFileParameters parameters)
    {
      return (BaseFileAppender) new NullAppender(fileName, parameters);
    }
  }
}
