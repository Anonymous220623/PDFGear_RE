// Decompiled with JetBrains decompiler
// Type: NLog.Internal.FileAppenders.WindowsMultiProcessFileAppender
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using System;
using System.IO;
using System.Security;
using System.Security.AccessControl;
using System.Threading;

#nullable disable
namespace NLog.Internal.FileAppenders;

[SecuritySafeCritical]
internal class WindowsMultiProcessFileAppender : BaseMutexFileAppender
{
  public static readonly IFileAppenderFactory TheFactory = (IFileAppenderFactory) new WindowsMultiProcessFileAppender.Factory();
  private FileStream _fileStream;

  public WindowsMultiProcessFileAppender(string fileName, ICreateFileParameters parameters)
    : base(fileName, parameters)
  {
    try
    {
      this.CreateAppendOnlyFile(fileName);
    }
    catch
    {
      if (this._fileStream != null)
        this._fileStream.Dispose();
      this._fileStream = (FileStream) null;
      throw;
    }
  }

  private void CreateAppendOnlyFile(string fileName)
  {
    string directoryName = Path.GetDirectoryName(fileName);
    if (!Directory.Exists(directoryName))
    {
      if (!this.CreateFileParameters.CreateDirs)
        throw new DirectoryNotFoundException(directoryName);
      Directory.CreateDirectory(directoryName);
    }
    FileShare share = FileShare.ReadWrite;
    if (this.CreateFileParameters.EnableFileDelete)
      share |= FileShare.Delete;
    try
    {
      int num = File.Exists(fileName) ? 1 : 0;
      this._fileStream = new FileStream(fileName, FileMode.Append, FileSystemRights.AppendData | FileSystemRights.Synchronize, share, 1, FileOptions.None);
      long position = this._fileStream.Position;
      if (num != 0 || position > 0L)
      {
        this.CreationTimeUtc = File.GetCreationTimeUtc(this.FileName);
        if (!(this.CreationTimeUtc < DateTime.UtcNow - TimeSpan.FromSeconds(2.0)) || position != 0L)
          return;
        Thread.Sleep(50);
        this.CreationTimeUtc = File.GetCreationTimeUtc(this.FileName);
      }
      else
      {
        this.CreationTimeUtc = DateTime.UtcNow;
        File.SetCreationTimeUtc(this.FileName, this.CreationTimeUtc);
      }
    }
    catch
    {
      if (this._fileStream != null)
        this._fileStream.Dispose();
      this._fileStream = (FileStream) null;
      throw;
    }
  }

  public override void Write(byte[] bytes, int offset, int count)
  {
    if (this._fileStream == null)
      return;
    this._fileStream.Write(bytes, offset, count);
  }

  public override void Close()
  {
    if (this._fileStream == null)
      return;
    InternalLogger.Trace<string>("Closing '{0}'", this.FileName);
    try
    {
      this._fileStream?.Dispose();
    }
    catch (Exception ex)
    {
      object[] objArray = new object[1]
      {
        (object) this.FileName
      };
      InternalLogger.Warn(ex, "Failed to close file '{0}'", objArray);
      Thread.Sleep(1);
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
      return (BaseFileAppender) new WindowsMultiProcessFileAppender(fileName, parameters);
    }
  }
}
