// Decompiled with JetBrains decompiler
// Type: NLog.Internal.FileAppenders.MutexMultiProcessFileAppender
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using System;
using System.IO;
using System.Security;
using System.Threading;

#nullable disable
namespace NLog.Internal.FileAppenders;

[SecuritySafeCritical]
internal class MutexMultiProcessFileAppender : BaseMutexFileAppender
{
  public static readonly IFileAppenderFactory TheFactory = (IFileAppenderFactory) new MutexMultiProcessFileAppender.Factory();
  private FileStream _fileStream;
  private Mutex _mutex;

  public MutexMultiProcessFileAppender(string fileName, ICreateFileParameters parameters)
    : base(fileName, parameters)
  {
    try
    {
      this._mutex = this.CreateSharableMutex("FileLock");
      this._fileStream = this.CreateFileStream(true);
    }
    catch
    {
      if (this._mutex != null)
      {
        this._mutex.Close();
        this._mutex = (Mutex) null;
      }
      if (this._fileStream != null)
      {
        this._fileStream.Close();
        this._fileStream = (FileStream) null;
      }
      throw;
    }
  }

  public override void Write(byte[] bytes, int offset, int count)
  {
    if (this._mutex == null)
      return;
    if (this._fileStream == null)
      return;
    try
    {
      this._mutex.WaitOne();
    }
    catch (AbandonedMutexException ex)
    {
    }
    try
    {
      this._fileStream.Seek(0L, SeekOrigin.End);
      this._fileStream.Write(bytes, offset, count);
      this._fileStream.Flush();
    }
    finally
    {
      this._mutex.ReleaseMutex();
    }
  }

  public override void Close()
  {
    if (this._mutex == null && this._fileStream == null)
      return;
    InternalLogger.Trace<string>("Closing '{0}'", this.FileName);
    try
    {
      this._mutex?.Close();
    }
    catch (Exception ex)
    {
      object[] objArray = new object[1]
      {
        (object) this.FileName
      };
      InternalLogger.Warn(ex, "Failed to close mutex: '{0}'", objArray);
    }
    finally
    {
      this._mutex = (Mutex) null;
    }
    try
    {
      this._fileStream?.Close();
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
      this._fileStream = (FileStream) null;
    }
  }

  public override void Flush()
  {
  }

  public override DateTime? GetFileCreationTimeUtc() => new DateTime?(this.CreationTimeUtc);

  public override long? GetFileLength() => this._fileStream?.Length;

  private class Factory : IFileAppenderFactory
  {
    BaseFileAppender IFileAppenderFactory.Open(string fileName, ICreateFileParameters parameters)
    {
      return (BaseFileAppender) new MutexMultiProcessFileAppender(fileName, parameters);
    }
  }
}
