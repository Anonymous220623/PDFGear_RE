// Decompiled with JetBrains decompiler
// Type: NLog.Internal.FileAppenders.CountingSingleProcessFileAppender
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using System;
using System.IO;
using System.Security;

#nullable disable
namespace NLog.Internal.FileAppenders;

[SecuritySafeCritical]
internal class CountingSingleProcessFileAppender : BaseFileAppender
{
  public static readonly IFileAppenderFactory TheFactory = (IFileAppenderFactory) new CountingSingleProcessFileAppender.Factory();
  private FileStream _file;
  private long _currentFileLength;
  private readonly bool _enableFileDeleteSimpleMonitor;
  private DateTime _lastSimpleMonitorCheckTimeUtc;

  public CountingSingleProcessFileAppender(string fileName, ICreateFileParameters parameters)
    : base(fileName, parameters)
  {
    FileInfo fileInfo = new FileInfo(fileName);
    this._currentFileLength = fileInfo.Exists ? fileInfo.Length : 0L;
    this._file = this.CreateFileStream(false);
    this._enableFileDeleteSimpleMonitor = parameters.EnableFileDeleteSimpleMonitor;
    this._lastSimpleMonitorCheckTimeUtc = this.OpenTimeUtc;
  }

  public override void Close()
  {
    if (this._file == null)
      return;
    InternalLogger.Trace<string>("Closing '{0}'", this.FileName);
    try
    {
      this._file.Close();
    }
    catch (Exception ex)
    {
      object[] objArray = new object[1]
      {
        (object) this.FileName
      };
      InternalLogger.Warn(ex, "Failed to close file: '{0}'", objArray);
      AsyncHelpers.WaitForDelay(TimeSpan.FromMilliseconds(1.0));
    }
    finally
    {
      this._file = (FileStream) null;
    }
  }

  public override void Flush()
  {
    if (this._file == null)
      return;
    this._file.Flush();
  }

  public override DateTime? GetFileCreationTimeUtc() => new DateTime?(this.CreationTimeUtc);

  public override long? GetFileLength() => new long?(this._currentFileLength);

  public override void Write(byte[] bytes, int offset, int count)
  {
    if (this._file == null)
      return;
    if (this._enableFileDeleteSimpleMonitor && BaseFileAppender.MonitorForEnableFileDeleteEvent(this.FileName, ref this._lastSimpleMonitorCheckTimeUtc))
    {
      this._file.Dispose();
      this._file = this.CreateFileStream(false);
      this._currentFileLength = this._file.Length;
    }
    this._currentFileLength += (long) count;
    this._file.Write(bytes, offset, count);
  }

  private class Factory : IFileAppenderFactory
  {
    BaseFileAppender IFileAppenderFactory.Open(string fileName, ICreateFileParameters parameters)
    {
      return (BaseFileAppender) new CountingSingleProcessFileAppender(fileName, parameters);
    }
  }
}
