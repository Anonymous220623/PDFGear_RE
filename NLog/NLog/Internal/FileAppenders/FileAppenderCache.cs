// Decompiled with JetBrains decompiler
// Type: NLog.Internal.FileAppenders.FileAppenderCache
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Time;
using System;
using System.IO;
using System.Threading;

#nullable disable
namespace NLog.Internal.FileAppenders;

internal sealed class FileAppenderCache : IFileAppenderCache, IDisposable
{
  private readonly BaseFileAppender[] _appenders;
  private Timer _autoClosingTimer;
  private string _archiveFilePatternToWatch;
  private readonly MultiFileWatcher _externalFileArchivingWatcher = new MultiFileWatcher(NotifyFilters.FileName | NotifyFilters.DirectoryName);
  private bool _logFileWasArchived;
  public static readonly FileAppenderCache Empty = new FileAppenderCache();

  private FileAppenderCache()
    : this(0, (IFileAppenderFactory) null, (ICreateFileParameters) null)
  {
  }

  public FileAppenderCache(
    int size,
    IFileAppenderFactory appenderFactory,
    ICreateFileParameters createFileParams)
  {
    this.Size = size;
    this.Factory = appenderFactory;
    this.CreateFileParameters = createFileParams;
    this._appenders = new BaseFileAppender[this.Size];
    this._autoClosingTimer = new Timer(new TimerCallback(this.AutoClosingTimerCallback), (object) null, -1, -1);
    this._externalFileArchivingWatcher.FileChanged += new FileSystemEventHandler(this.ExternalFileArchivingWatcher_OnFileChanged);
  }

  private void ExternalFileArchivingWatcher_OnFileChanged(object sender, FileSystemEventArgs e)
  {
    if (this._logFileWasArchived || this.CheckCloseAppenders == null || this._autoClosingTimer == null)
      return;
    if (this.FileAppenderFolderChanged(e.FullPath))
    {
      if ((e.ChangeType & (WatcherChangeTypes.Deleted | WatcherChangeTypes.Renamed)) != (WatcherChangeTypes) 0)
        this._logFileWasArchived = true;
    }
    else if ((e.ChangeType & WatcherChangeTypes.Created) == WatcherChangeTypes.Created)
      this._logFileWasArchived = true;
    if (!this._logFileWasArchived || this._autoClosingTimer == null)
      return;
    this._autoClosingTimer.Change(50, -1);
  }

  private bool FileAppenderFolderChanged(string fullPath)
  {
    if (string.IsNullOrEmpty(fullPath))
      return false;
    if (string.IsNullOrEmpty(this._archiveFilePatternToWatch))
      return true;
    string directoryName1 = Path.GetDirectoryName(this._archiveFilePatternToWatch);
    if (string.IsNullOrEmpty(directoryName1))
      return true;
    string directoryName2 = Path.GetDirectoryName(fullPath);
    return !string.Equals(directoryName1, directoryName2, StringComparison.OrdinalIgnoreCase);
  }

  public string ArchiveFilePatternToWatch
  {
    get => this._archiveFilePatternToWatch;
    set
    {
      if (!(this._archiveFilePatternToWatch != value))
        return;
      if (!string.IsNullOrEmpty(this._archiveFilePatternToWatch))
      {
        string directoryName = Path.GetDirectoryName(this._archiveFilePatternToWatch);
        if (string.IsNullOrEmpty(directoryName))
          this._externalFileArchivingWatcher.StopWatching(directoryName);
      }
      this._archiveFilePatternToWatch = value;
      this._logFileWasArchived = false;
    }
  }

  public void InvalidateAppendersForArchivedFiles()
  {
    if (!this._logFileWasArchived)
      return;
    this._logFileWasArchived = false;
    InternalLogger.Trace("FileAppender: Invalidate archived files");
    this.CloseAppenders("Cleanup Archive");
  }

  private void AutoClosingTimerCallback(object state)
  {
    EventHandler checkCloseAppenders = this.CheckCloseAppenders;
    if (checkCloseAppenders == null)
      return;
    checkCloseAppenders((object) this, EventArgs.Empty);
  }

  public ICreateFileParameters CreateFileParameters { get; private set; }

  public IFileAppenderFactory Factory { get; private set; }

  public int Size { get; private set; }

  public event EventHandler CheckCloseAppenders;

  public BaseFileAppender AllocateAppender(string fileName)
  {
    BaseFileAppender baseFileAppender = (BaseFileAppender) null;
    int freeSpot = this._appenders.Length - 1;
    for (int index1 = 0; index1 < this._appenders.Length; ++index1)
    {
      if (this._appenders[index1] == null)
      {
        freeSpot = index1;
        break;
      }
      if (string.Equals(this._appenders[index1].FileName, fileName, StringComparison.OrdinalIgnoreCase))
      {
        BaseFileAppender appender = this._appenders[index1];
        if (index1 > 0)
        {
          for (int index2 = index1; index2 > 0; --index2)
            this._appenders[index2] = this._appenders[index2 - 1];
          this._appenders[0] = appender;
        }
        baseFileAppender = appender;
        break;
      }
    }
    if (baseFileAppender == null)
      baseFileAppender = this.CreateAppender(fileName, freeSpot);
    return baseFileAppender;
  }

  private BaseFileAppender CreateAppender(string fileName, int freeSpot)
  {
    BaseFileAppender appender;
    try
    {
      InternalLogger.Debug<string>("Creating file appender: {0}", fileName);
      BaseFileAppender baseFileAppender = this.Factory.Open(fileName, this.CreateFileParameters);
      if (this._appenders[freeSpot] != null)
      {
        this.CloseAppender(this._appenders[freeSpot], "Stale", false);
        this._appenders[freeSpot] = (BaseFileAppender) null;
      }
      for (int index = freeSpot; index > 0; --index)
        this._appenders[index] = this._appenders[index - 1];
      this._appenders[0] = baseFileAppender;
      appender = baseFileAppender;
      if (this.CheckCloseAppenders != null)
      {
        if (freeSpot == 0)
          this._logFileWasArchived = false;
        if (!string.IsNullOrEmpty(this._archiveFilePatternToWatch))
        {
          string directoryName = Path.GetDirectoryName(this._archiveFilePatternToWatch);
          if (!Directory.Exists(directoryName))
            Directory.CreateDirectory(directoryName);
          this._externalFileArchivingWatcher.Watch(this._archiveFilePatternToWatch);
        }
        this._externalFileArchivingWatcher.Watch(appender.FileName);
      }
    }
    catch (Exception ex)
    {
      object[] objArray = new object[1]{ (object) fileName };
      InternalLogger.Warn(ex, "Failed to create file appender: {0}", objArray);
      throw;
    }
    return appender;
  }

  public void CloseAppenders(string reason)
  {
    for (int index = 0; index < this._appenders.Length; ++index)
    {
      BaseFileAppender appender = this._appenders[index];
      if (appender == null)
        break;
      this.CloseAppender(appender, reason, true);
      this._appenders[index] = (BaseFileAppender) null;
      appender.Dispose();
    }
  }

  public void CloseAppenders(DateTime expireTime)
  {
    if (this._logFileWasArchived)
    {
      this._logFileWasArchived = false;
      this.CloseAppenders("Cleanup Timer");
    }
    else
    {
      if (!(expireTime != DateTime.MinValue))
        return;
      for (int index1 = 0; index1 < this._appenders.Length && this._appenders[index1] != null; ++index1)
      {
        if (this._appenders[index1].OpenTimeUtc < expireTime)
        {
          for (int index2 = index1; index2 < this._appenders.Length; ++index2)
          {
            BaseFileAppender appender = this._appenders[index2];
            if (appender == null)
              break;
            this.CloseAppender(appender, "Expired", index1 == 0);
            this._appenders[index2] = (BaseFileAppender) null;
            appender.Dispose();
          }
          break;
        }
      }
    }
  }

  public void FlushAppenders()
  {
    foreach (BaseFileAppender appender in this._appenders)
    {
      if (appender == null)
        break;
      try
      {
        appender.Flush();
      }
      catch (Exception ex)
      {
        object[] objArray = new object[1]
        {
          (object) appender.FileName
        };
        InternalLogger.Error(ex, "Failed to flush file '{0}'.", objArray);
        this.InvalidateAppender(appender.FileName)?.Dispose();
        throw;
      }
    }
  }

  private BaseFileAppender GetAppender(string fileName)
  {
    for (int index = 0; index < this._appenders.Length; ++index)
    {
      BaseFileAppender appender = this._appenders[index];
      if (appender != null)
      {
        if (string.Equals(appender.FileName, fileName, StringComparison.OrdinalIgnoreCase))
          return appender;
      }
      else
        break;
    }
    return (BaseFileAppender) null;
  }

  public DateTime? GetFileCreationTimeSource(string filePath, DateTime? fallbackTimeSource = null)
  {
    BaseFileAppender appender = this.GetAppender(filePath);
    DateTime? creationTimeSource = new DateTime?();
    if (appender != null)
    {
      try
      {
        creationTimeSource = appender.GetFileCreationTimeUtc();
        if (creationTimeSource.HasValue)
        {
          DateTime creationTimeUtc = appender.CreationTimeUtc;
          if (creationTimeSource.Value != creationTimeUtc)
            appender.CreationTimeUtc = creationTimeSource.Value;
          return new DateTime?(appender.CreationTimeSource);
        }
      }
      catch (Exception ex)
      {
        object[] objArray = new object[1]
        {
          (object) appender.FileName
        };
        InternalLogger.Error(ex, "Failed to get file creation time for file '{0}'.", objArray);
        this.InvalidateAppender(appender.FileName)?.Dispose();
        throw;
      }
    }
    FileInfo fileInfo = new FileInfo(filePath);
    if (!fileInfo.Exists)
      return creationTimeSource;
    creationTimeSource = new DateTime?(fileInfo.LookupValidFileCreationTimeUtc(fallbackTimeSource));
    return new DateTime?(TimeSource.Current.FromSystemTime(creationTimeSource.Value));
  }

  public DateTime? GetFileLastWriteTimeUtc(string filePath)
  {
    FileInfo fileInfo = new FileInfo(filePath);
    return fileInfo.Exists ? new DateTime?(fileInfo.GetLastWriteTimeUtc()) : new DateTime?();
  }

  public long? GetFileLength(string filePath)
  {
    BaseFileAppender appender = this.GetAppender(filePath);
    if (appender != null)
    {
      try
      {
        long? fileLength = appender.GetFileLength();
        if (fileLength.HasValue)
          return fileLength;
      }
      catch (IOException ex)
      {
        object[] objArray = new object[1]
        {
          (object) appender.FileName
        };
        InternalLogger.Error((Exception) ex, "Failed to get length for file '{0}'.", objArray);
        this.InvalidateAppender(appender.FileName)?.Dispose();
      }
      catch (Exception ex)
      {
        object[] objArray = new object[1]
        {
          (object) appender.FileName
        };
        InternalLogger.Error(ex, "Failed to get length for file '{0}'.", objArray);
        this.InvalidateAppender(appender.FileName)?.Dispose();
        throw;
      }
    }
    FileInfo fileInfo = new FileInfo(filePath);
    return fileInfo.Exists ? new long?(fileInfo.Length) : new long?();
  }

  public BaseFileAppender InvalidateAppender(string filePath)
  {
    for (int index1 = 0; index1 < this._appenders.Length; ++index1)
    {
      BaseFileAppender appender = this._appenders[index1];
      if (appender != null)
      {
        if (string.Equals(appender.FileName, filePath, StringComparison.OrdinalIgnoreCase))
        {
          for (int index2 = index1; index2 < this._appenders.Length - 1; ++index2)
            this._appenders[index2] = this._appenders[index2 + 1];
          this._appenders[this._appenders.Length - 1] = (BaseFileAppender) null;
          this.CloseAppender(appender, "Invalidate", this._appenders[0] == null);
          return appender;
        }
      }
      else
        break;
    }
    return (BaseFileAppender) null;
  }

  private void CloseAppender(BaseFileAppender appender, string reason, bool lastAppender)
  {
    InternalLogger.Debug<string, string>("FileAppender Closing {0} - {1}", reason, appender.FileName);
    if (lastAppender)
    {
      this._autoClosingTimer.Change(-1, -1);
      this._externalFileArchivingWatcher.StopWatching();
      this._logFileWasArchived = false;
    }
    else
      this._externalFileArchivingWatcher.StopWatching(appender.FileName);
    appender.Close();
  }

  public void Dispose()
  {
    this.CheckCloseAppenders = (EventHandler) null;
    this._externalFileArchivingWatcher.Dispose();
    this._logFileWasArchived = false;
    Timer autoClosingTimer = this._autoClosingTimer;
    if (autoClosingTimer == null)
      return;
    this._autoClosingTimer = (Timer) null;
    autoClosingTimer.WaitForDispose(TimeSpan.Zero);
  }
}
