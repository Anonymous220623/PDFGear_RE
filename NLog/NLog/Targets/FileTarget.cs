// Decompiled with JetBrains decompiler
// Type: NLog.Targets.FileTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using NLog.Internal.FileAppenders;
using NLog.Layouts;
using NLog.Targets.FileArchiveModes;
using NLog.Time;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

#nullable disable
namespace NLog.Targets;

[Target("File")]
public class FileTarget : TargetWithLayoutHeaderAndFooter, ICreateFileParameters
{
  private const int InitializedFilesCleanupPeriod = 2;
  private const int InitializedFilesCounterMax = 25;
  private const int ArchiveAboveSizeDisabled = -1;
  private readonly Dictionary<string, DateTime> _initializedFiles = new Dictionary<string, DateTime>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
  private IFileAppenderCache _fileAppenderCache;
  private IFileArchiveMode _fileArchiveHelper;
  private Timer _autoClosingTimer;
  private int _initializedFilesCounter;
  private int _maxArchiveFiles;
  private int _maxArchiveDays;
  private FilePathLayout _fullFileName;
  private FilePathLayout _fullArchiveFileName;
  private FileArchivePeriod _archiveEvery;
  private long _archiveAboveSize;
  private bool _enableArchiveFileCompression;
  private DateTime? _previousLogEventTimestamp;
  private string _previousLogFileName;
  private bool _concurrentWrites;
  private bool _keepFileOpen;
  private bool _cleanupFileName;
  private FilePathKind _fileNameKind;
  private FilePathKind _archiveFileKind;
  private bool? _archiveOldFileOnStartup;
  private string _archiveDateFormat;
  private ArchiveNumberingMode _archiveNumbering;
  private readonly ReusableStreamCreator _reusableFileWriteStream = new ReusableStreamCreator(4096 /*0x1000*/);
  private readonly ReusableStreamCreator _reusableAsyncFileWriteStream = new ReusableStreamCreator(4096 /*0x1000*/);
  private readonly ReusableBufferCreator _reusableEncodingBuffer = new ReusableBufferCreator(1024 /*0x0400*/);
  private SortHelpers.KeySelector<AsyncLogEventInfo, string> _getFullFileNameDelegate;

  private IFileArchiveMode GetFileArchiveHelper(string archiveFilePattern)
  {
    return this._fileArchiveHelper ?? (this._fileArchiveHelper = FileArchiveModeFactory.CreateArchiveStyle(archiveFilePattern, this.ArchiveNumbering, this.GetArchiveDateFormatString(this.ArchiveDateFormat), this.ArchiveFileName != null, this.MaxArchiveFiles > 0 || this.MaxArchiveDays > 0));
  }

  public FileTarget()
    : this((IFileAppenderCache) FileAppenderCache.Empty)
  {
  }

  internal FileTarget(IFileAppenderCache fileAppenderCache)
  {
    this.ArchiveNumbering = ArchiveNumberingMode.Sequence;
    this._maxArchiveFiles = 0;
    this._maxArchiveDays = 0;
    this.ConcurrentWriteAttemptDelay = 1;
    this.ArchiveEvery = FileArchivePeriod.None;
    this.ArchiveAboveSize = -1L;
    this.ArchiveOldFileOnStartupAboveSize = 0L;
    this.ConcurrentWriteAttempts = 10;
    this.ConcurrentWrites = true;
    this.Encoding = Encoding.Default;
    this.BufferSize = 32768 /*0x8000*/;
    this.AutoFlush = true;
    this.FileAttributes = Win32FileAttributes.Normal;
    this.LineEnding = LineEndingMode.Default;
    this.EnableFileDelete = true;
    this.OpenFileCacheTimeout = -1;
    this.OpenFileCacheSize = 5;
    this.CreateDirs = true;
    this.ForceManaged = false;
    this.ArchiveDateFormat = string.Empty;
    this._fileAppenderCache = fileAppenderCache;
    this.CleanupFileName = true;
    this.WriteFooterOnArchivingOnly = false;
    this.OptimizeBufferReuse = this.GetType() == typeof (FileTarget);
  }

  static FileTarget()
  {
    FileTarget.FileCompressor = (IFileCompressor) new ZipArchiveFileCompressor();
  }

  public FileTarget(string name)
    : this()
  {
    this.Name = name;
  }

  [RequiredParameter]
  public Layout FileName
  {
    get => this._fullFileName?.GetLayout();
    set
    {
      this._fullFileName = this.CreateFileNameLayout(value);
      this.ResetFileAppenders("FileName Changed");
    }
  }

  private FilePathLayout CreateFileNameLayout(Layout value)
  {
    return value == null ? (FilePathLayout) null : new FilePathLayout(value, this.CleanupFileName, this.FileNameKind);
  }

  [DefaultValue(true)]
  public bool CleanupFileName
  {
    get => this._cleanupFileName;
    set
    {
      if (this._cleanupFileName == value)
        return;
      this._cleanupFileName = value;
      this._fullFileName = this.CreateFileNameLayout(this.FileName);
      this._fullArchiveFileName = this.CreateFileNameLayout(this.ArchiveFileName);
      this.ResetFileAppenders("CleanupFileName Changed");
    }
  }

  [DefaultValue(FilePathKind.Unknown)]
  public FilePathKind FileNameKind
  {
    get => this._fileNameKind;
    set
    {
      if (this._fileNameKind == value)
        return;
      this._fileNameKind = value;
      this._fullFileName = this.CreateFileNameLayout(this.FileName);
      this.ResetFileAppenders("FileNameKind Changed");
    }
  }

  [DefaultValue(true)]
  [Advanced]
  public bool CreateDirs { get; set; }

  [DefaultValue(false)]
  public bool DeleteOldFileOnStartup { get; set; }

  [DefaultValue(false)]
  [Advanced]
  public bool ReplaceFileContentsOnEachWrite { get; set; }

  [DefaultValue(false)]
  public bool KeepFileOpen
  {
    get => this._keepFileOpen;
    set
    {
      if (this._keepFileOpen == value)
        return;
      this._keepFileOpen = value;
      this.ResetFileAppenders("KeepFileOpen Changed");
    }
  }

  [Obsolete("This option will be removed in NLog 5. Marked obsolete on NLog 4.5")]
  [DefaultValue(0)]
  public int maxLogFilenames { get; set; }

  [DefaultValue(true)]
  public bool EnableFileDelete { get; set; }

  [Advanced]
  public Win32FileAttributes FileAttributes { get; set; }

  bool ICreateFileParameters.IsArchivingEnabled => this.IsArchivingEnabled;

  [Advanced]
  public LineEndingMode LineEnding { get; set; }

  [DefaultValue(true)]
  public bool AutoFlush { get; set; }

  [DefaultValue(5)]
  [Advanced]
  public int OpenFileCacheSize { get; set; }

  [DefaultValue(-1)]
  [Advanced]
  public int OpenFileCacheTimeout { get; set; }

  public int OpenFileFlushTimeout { get; set; }

  [DefaultValue(32768 /*0x8000*/)]
  public int BufferSize { get; set; }

  public Encoding Encoding { get; set; }

  [DefaultValue(false)]
  [Advanced]
  public bool DiscardAll { get; set; }

  [DefaultValue(true)]
  public bool ConcurrentWrites
  {
    get => this._concurrentWrites;
    set
    {
      if (this._concurrentWrites == value)
        return;
      this._concurrentWrites = value;
      this.ResetFileAppenders("ConcurrentWrites Changed");
    }
  }

  [DefaultValue(false)]
  public bool NetworkWrites { get; set; }

  [DefaultValue(false)]
  public bool WriteBom { get; set; }

  [DefaultValue(10)]
  [Advanced]
  public int ConcurrentWriteAttempts { get; set; }

  [DefaultValue(1)]
  [Advanced]
  public int ConcurrentWriteAttemptDelay { get; set; }

  [DefaultValue(false)]
  public bool ArchiveOldFileOnStartup
  {
    get => this._archiveOldFileOnStartup ?? false;
    set => this._archiveOldFileOnStartup = new bool?(value);
  }

  [DefaultValue(0)]
  public long ArchiveOldFileOnStartupAboveSize { get; set; }

  [DefaultValue("")]
  public string ArchiveDateFormat
  {
    get => this._archiveDateFormat;
    set
    {
      if (!(this._archiveDateFormat != value))
        return;
      this._archiveDateFormat = value;
      this.ResetFileAppenders("ArchiveDateFormat Changed");
    }
  }

  public long ArchiveAboveSize
  {
    get => this._archiveAboveSize;
    set
    {
      if (this.ArchiveAboveSize > -1L != value > -1L)
      {
        this._archiveAboveSize = value;
        this.ResetFileAppenders("ArchiveAboveSize Changed");
      }
      else
        this._archiveAboveSize = value;
    }
  }

  public FileArchivePeriod ArchiveEvery
  {
    get => this._archiveEvery;
    set
    {
      if (this._archiveEvery == value)
        return;
      this._archiveEvery = value;
      this.ResetFileAppenders("ArchiveEvery Changed");
    }
  }

  public FilePathKind ArchiveFileKind
  {
    get => this._archiveFileKind;
    set
    {
      if (this._archiveFileKind == value)
        return;
      this._archiveFileKind = value;
      this._fullArchiveFileName = this.CreateFileNameLayout(this.ArchiveFileName);
      this.ResetFileAppenders("ArchiveFileKind Changed");
    }
  }

  public Layout ArchiveFileName
  {
    get
    {
      return this._fullArchiveFileName == null ? (Layout) null : this._fullArchiveFileName.GetLayout();
    }
    set
    {
      this._fullArchiveFileName = this.CreateFileNameLayout(value);
      this.ResetFileAppenders("ArchiveFileName Changed");
    }
  }

  [DefaultValue(0)]
  public int MaxArchiveFiles
  {
    get => this._maxArchiveFiles;
    set
    {
      if (this._maxArchiveFiles == value)
        return;
      this._maxArchiveFiles = value;
      this.ResetFileAppenders("MaxArchiveFiles Changed");
    }
  }

  [DefaultValue(0)]
  public int MaxArchiveDays
  {
    get => this._maxArchiveDays;
    set
    {
      if (this._maxArchiveDays == value)
        return;
      this._maxArchiveDays = value;
      this.ResetFileAppenders("MaxArchiveDays Changed");
    }
  }

  public ArchiveNumberingMode ArchiveNumbering
  {
    get => this._archiveNumbering;
    set
    {
      if (this._archiveNumbering == value)
        return;
      this._archiveNumbering = value;
      this.ResetFileAppenders("ArchiveNumbering Changed");
    }
  }

  public static IFileCompressor FileCompressor { get; set; }

  [DefaultValue(false)]
  public bool EnableArchiveFileCompression
  {
    get => this._enableArchiveFileCompression && FileTarget.FileCompressor != null;
    set
    {
      if (this._enableArchiveFileCompression == value)
        return;
      this._enableArchiveFileCompression = value;
      this.ResetFileAppenders("EnableArchiveFileCompression Changed");
    }
  }

  [DefaultValue(false)]
  public bool ForceManaged { get; set; }

  [DefaultValue(false)]
  public bool ForceMutexConcurrentWrites { get; set; }

  [DefaultValue(false)]
  public bool WriteFooterOnArchivingOnly { get; set; }

  protected internal string NewLineChars => this.LineEnding.NewLineCharacters;

  private void RefreshArchiveFilePatternToWatch(string fileName, LogEventInfo logEvent)
  {
    if (this._fileAppenderCache == null)
      return;
    this._fileAppenderCache.CheckCloseAppenders -= new EventHandler(this.AutoCloseAppendersAfterArchive);
    int num = !this.IsArchivingEnabled || !this.KeepFileOpen ? 0 : (this.ConcurrentWrites ? 1 : 0);
    if ((num | (!this.KeepFileOpen || !this.EnableFileDelete || this.NetworkWrites || this.ReplaceFileContentsOnEachWrite ? (false ? 1 : 0) : (!this.EnableFileDeleteSimpleMonitor ? 1 : 0))) != 0)
      this._fileAppenderCache.CheckCloseAppenders += new EventHandler(this.AutoCloseAppendersAfterArchive);
    if (num != 0)
    {
      string archiveFileNamePattern = this.GetArchiveFileNamePattern(fileName, logEvent);
      string path2 = (!string.IsNullOrEmpty(archiveFileNamePattern) ? this.GetFileArchiveHelper(archiveFileNamePattern) : (IFileArchiveMode) null) != null ? this._fileArchiveHelper.GenerateFileNameMask(archiveFileNamePattern) : string.Empty;
      this._fileAppenderCache.ArchiveFilePatternToWatch = !string.IsNullOrEmpty(path2) ? Path.Combine(Path.GetDirectoryName(archiveFileNamePattern), path2) : string.Empty;
    }
    else
      this._fileAppenderCache.ArchiveFilePatternToWatch = (string) null;
  }

  public void CleanupInitializedFiles()
  {
    try
    {
      this.CleanupInitializedFiles(TimeSource.Current.Time.AddDays(-2.0));
    }
    catch (Exception ex)
    {
      if (ex.MustBeRethrownImmediately())
        throw;
      object[] objArray = new object[1]{ (object) this };
      InternalLogger.Error(ex, "{0}: Exception in CleanupInitializedFiles", objArray);
    }
  }

  public void CleanupInitializedFiles(DateTime cleanupThreshold)
  {
    if (InternalLogger.IsTraceEnabled)
      InternalLogger.Trace<string, DateTime>("FileTarget(Name={0}): Cleanup Initialized Files with cleanupThreshold {1}", this.Name, cleanupThreshold);
    List<string> stringList = (List<string>) null;
    foreach (KeyValuePair<string, DateTime> initializedFile in this._initializedFiles)
    {
      if (initializedFile.Value < cleanupThreshold)
      {
        if (stringList == null)
          stringList = new List<string>();
        stringList.Add(initializedFile.Key);
      }
    }
    if (stringList != null)
    {
      foreach (string fileName in stringList)
        this.FinalizeFile(fileName);
    }
    InternalLogger.Trace<string>("FileTarget(Name={0}): CleanupInitializedFiles Done", this.Name);
  }

  protected override void FlushAsync(AsyncContinuation asyncContinuation)
  {
    try
    {
      InternalLogger.Trace<string>("FileTarget(Name={0}): FlushAsync", this.Name);
      this._fileAppenderCache.FlushAppenders();
      asyncContinuation((Exception) null);
      InternalLogger.Trace<string>("FileTarget(Name={0}): FlushAsync Done", this.Name);
    }
    catch (Exception ex)
    {
      InternalLogger.Warn(ex, "FileTarget(Name={0}): Exception in FlushAsync", (object) this.Name);
      if (this.ExceptionMustBeRethrown(ex))
        throw;
      asyncContinuation(ex);
    }
  }

  private IFileAppenderFactory GetFileAppenderFactory()
  {
    if (this.DiscardAll)
      return NullAppender.TheFactory;
    if (!this.KeepFileOpen || this.NetworkWrites)
      return RetryingMultiProcessFileAppender.TheFactory;
    if (this.ConcurrentWrites)
    {
      if (!this.ForceMutexConcurrentWrites && PlatformDetector.IsWin32 && !PlatformDetector.IsMono)
        return WindowsMultiProcessFileAppender.TheFactory;
      return PlatformDetector.SupportsSharableMutex ? MutexMultiProcessFileAppender.TheFactory : RetryingMultiProcessFileAppender.TheFactory;
    }
    return this.IsArchivingEnabled ? CountingSingleProcessFileAppender.TheFactory : SingleProcessFileAppender.TheFactory;
  }

  private bool IsArchivingEnabled => this.ArchiveAboveSize > -1L || this.ArchiveEvery != 0;

  private bool IsSimpleKeepFileOpen
  {
    get
    {
      return this.KeepFileOpen && !this.ConcurrentWrites && !this.NetworkWrites && !this.ReplaceFileContentsOnEachWrite;
    }
  }

  private bool EnableFileDeleteSimpleMonitor
  {
    get => this.EnableFileDelete && !PlatformDetector.IsWin32 && this.IsSimpleKeepFileOpen;
  }

  bool ICreateFileParameters.EnableFileDeleteSimpleMonitor => this.EnableFileDeleteSimpleMonitor;

  protected override void InitializeTarget()
  {
    base.InitializeTarget();
    IFileAppenderFactory fileAppenderFactory = this.GetFileAppenderFactory();
    if (InternalLogger.IsTraceEnabled)
      InternalLogger.Trace<string, Type>("FileTarget(Name={0}): Using appenderFactory: {1}", this.Name, fileAppenderFactory.GetType());
    this._fileAppenderCache = (IFileAppenderCache) new FileAppenderCache(this.OpenFileCacheSize, fileAppenderFactory, (ICreateFileParameters) this);
    if (this.OpenFileCacheSize <= 0 && !this.EnableFileDelete || this.OpenFileCacheTimeout <= 0 && this.OpenFileFlushTimeout <= 0)
      return;
    int num = this.OpenFileCacheTimeout <= 0 || this.OpenFileFlushTimeout <= 0 ? Math.Max(this.OpenFileCacheTimeout, this.OpenFileFlushTimeout) : Math.Min(this.OpenFileCacheTimeout, this.OpenFileFlushTimeout);
    InternalLogger.Trace<string>("FileTarget(Name={0}): Start autoClosingTimer", this.Name);
    this._autoClosingTimer = new Timer((TimerCallback) (state => this.AutoClosingTimerCallback((object) this, EventArgs.Empty)), (object) null, num * 1000, num * 1000);
  }

  protected override void CloseTarget()
  {
    base.CloseTarget();
    foreach (string fileName in new List<string>((IEnumerable<string>) this._initializedFiles.Keys))
      this.FinalizeFile(fileName);
    this._fileArchiveHelper = (IFileArchiveMode) null;
    Timer autoClosingTimer = this._autoClosingTimer;
    if (autoClosingTimer != null)
    {
      InternalLogger.Trace<string>("FileTarget(Name={0}): Stop autoClosingTimer", this.Name);
      this._autoClosingTimer = (Timer) null;
      autoClosingTimer.WaitForDispose(TimeSpan.Zero);
    }
    this._fileAppenderCache.CloseAppenders("Dispose");
    this._fileAppenderCache.Dispose();
  }

  private void ResetFileAppenders(string reason)
  {
    this._fileArchiveHelper = (IFileArchiveMode) null;
    if (!this.IsInitialized)
      return;
    this._fileAppenderCache.CloseAppenders(reason);
    this._initializedFiles.Clear();
  }

  protected override void Write(LogEventInfo logEvent)
  {
    string fullFileName = this.GetFullFileName(logEvent);
    if (string.IsNullOrEmpty(fullFileName))
      throw new ArgumentException("The path is not of a legal form.");
    if (this.OptimizeBufferReuse)
    {
      using (ReusableObjectCreator<MemoryStream>.LockOject lockOject1 = this._reusableFileWriteStream.Allocate())
      {
        using (ReusableObjectCreator<StringBuilder>.LockOject lockOject2 = this.ReusableLayoutBuilder.Allocate())
        {
          using (ReusableObjectCreator<char[]>.LockOject lockOject3 = this._reusableEncodingBuffer.Allocate())
            this.RenderFormattedMessageToStream(logEvent, lockOject2.Result, lockOject3.Result, lockOject1.Result);
        }
        this.ProcessLogEvent(logEvent, fullFileName, new ArraySegment<byte>(lockOject1.Result.GetBuffer(), 0, (int) lockOject1.Result.Length));
      }
    }
    else
    {
      byte[] bytesToWrite = this.GetBytesToWrite(logEvent);
      this.ProcessLogEvent(logEvent, fullFileName, new ArraySegment<byte>(bytesToWrite));
    }
  }

  internal string GetFullFileName(LogEventInfo logEvent)
  {
    if (this._fullFileName == null)
      return (string) null;
    if (!this.OptimizeBufferReuse)
      return this._fullFileName.Render(logEvent);
    using (ReusableObjectCreator<StringBuilder>.LockOject lockOject = this.ReusableLayoutBuilder.Allocate())
      return this._fullFileName.RenderWithBuilder(logEvent, lockOject.Result);
  }

  [Obsolete("Instead override Write(IList<AsyncLogEventInfo> logEvents. Marked obsolete on NLog 4.5")]
  protected override void Write(AsyncLogEventInfo[] logEvents)
  {
    this.Write((IList<AsyncLogEventInfo>) logEvents);
  }

  protected override void Write(IList<AsyncLogEventInfo> logEvents)
  {
    if (this._getFullFileNameDelegate == null)
      this._getFullFileNameDelegate = (SortHelpers.KeySelector<AsyncLogEventInfo, string>) (c => this.GetFullFileName(c.LogEvent));
    SortHelpers.ReadOnlySingleBucketDictionary<string, IList<AsyncLogEventInfo>> bucketDictionary = logEvents.BucketSort<AsyncLogEventInfo, string>(this._getFullFileNameDelegate);
    using (ReusableObjectCreator<MemoryStream>.LockOject lockOject = this.OptimizeBufferReuse ? this._reusableAsyncFileWriteStream.Allocate() : this._reusableAsyncFileWriteStream.None)
    {
      MemoryStream ms = lockOject.Result ?? new MemoryStream();
      foreach (KeyValuePair<string, IList<AsyncLogEventInfo>> keyValuePair in bucketDictionary)
      {
        int count = keyValuePair.Value.Count;
        if (count > 0)
        {
          string key = keyValuePair.Key;
          if (string.IsNullOrEmpty(key))
          {
            InternalLogger.Warn<string>("FileTarget(Name={0}): FileName Layout returned empty string. The path is not of a legal form.", this.Name);
            ArgumentException argumentException = new ArgumentException("The path is not of a legal form.");
            for (int index = 0; index < count; ++index)
              keyValuePair.Value[index].Continuation((Exception) argumentException);
          }
          else
          {
            int num = 0;
            while (num < keyValuePair.Value.Count)
            {
              ms.Position = 0L;
              ms.SetLength(0L);
              int memoryStream = this.WriteToMemoryStream(keyValuePair.Value, num, ms);
              Exception lastException;
              this.FlushCurrentFileWrites(key, keyValuePair.Value[num].LogEvent, ms, out lastException);
              for (int index = 0; index < memoryStream; ++index)
                keyValuePair.Value[num++].Continuation(lastException);
            }
          }
        }
      }
    }
  }

  private int WriteToMemoryStream(
    IList<AsyncLogEventInfo> logEvents,
    int startIndex,
    MemoryStream ms)
  {
    if (this.OptimizeBufferReuse)
    {
      int num = this.BufferSize * 100;
      using (ReusableObjectCreator<StringBuilder>.LockOject lockOject1 = this.ReusableLayoutBuilder.Allocate())
      {
        using (ReusableObjectCreator<char[]>.LockOject lockOject2 = this._reusableEncodingBuffer.Allocate())
        {
          using (ReusableObjectCreator<MemoryStream>.LockOject lockOject3 = this._reusableFileWriteStream.Allocate())
          {
            StringBuilder result1 = lockOject1.Result;
            char[] result2 = lockOject2.Result;
            MemoryStream result3 = lockOject3.Result;
            for (int index = startIndex; index < logEvents.Count; ++index)
            {
              result3.Position = 0L;
              result3.SetLength(0L);
              result1.ClearBuilder();
              this.RenderFormattedMessageToStream(logEvents[index].LogEvent, result1, result2, result3);
              ms.Write(result3.GetBuffer(), 0, (int) result3.Length);
              if (ms.Length > (long) num && !this.ReplaceFileContentsOnEachWrite)
                return index - startIndex + 1;
            }
          }
        }
      }
    }
    else
    {
      for (int index = startIndex; index < logEvents.Count; ++index)
      {
        byte[] bytesToWrite = this.GetBytesToWrite(logEvents[index].LogEvent);
        if (ms.Capacity == 0)
          ms.Capacity = this.GetMemoryStreamInitialSize(logEvents.Count, bytesToWrite.Length);
        ms.Write(bytesToWrite, 0, bytesToWrite.Length);
      }
    }
    return logEvents.Count - startIndex;
  }

  private int GetMemoryStreamInitialSize(int eventsCount, int firstEventSize)
  {
    if (eventsCount > 10)
      return (1 + eventsCount) * (firstEventSize / 1024 /*0x0400*/ + 1) * 1024 /*0x0400*/;
    return eventsCount > 1 ? (1 + eventsCount) * firstEventSize : firstEventSize;
  }

  private void ProcessLogEvent(
    LogEventInfo logEvent,
    string fileName,
    ArraySegment<byte> bytesToWrite)
  {
    DateTime previousLogEventTimestamp = this.InitializeFile(fileName, logEvent);
    bool initializedNewFile = previousLogEventTimestamp == DateTime.MinValue;
    if (initializedNewFile && fileName == this._previousLogFileName && this._previousLogEventTimestamp.HasValue)
      previousLogEventTimestamp = this._previousLogEventTimestamp.Value;
    if (this.TryArchiveFile(fileName, logEvent, bytesToWrite.Count, previousLogEventTimestamp, initializedNewFile))
      initializedNewFile = this.InitializeFile(fileName, logEvent) == DateTime.MinValue | initializedNewFile;
    if (this.ReplaceFileContentsOnEachWrite)
      this.ReplaceFileContent(fileName, bytesToWrite, true);
    else
      this.WriteToFile(fileName, bytesToWrite, initializedNewFile);
    this._previousLogFileName = fileName;
    this._previousLogEventTimestamp = new DateTime?(logEvent.TimeStamp);
  }

  protected virtual string GetFormattedMessage(LogEventInfo logEvent)
  {
    return this.Layout.Render(logEvent);
  }

  protected virtual byte[] GetBytesToWrite(LogEventInfo logEvent)
  {
    string formattedMessage = this.GetFormattedMessage(logEvent);
    int byteCount1 = this.Encoding.GetByteCount(formattedMessage);
    int byteCount2 = this.Encoding.GetByteCount(this.NewLineChars);
    byte[] bytes = new byte[byteCount1 + byteCount2];
    this.Encoding.GetBytes(formattedMessage, 0, formattedMessage.Length, bytes, 0);
    this.Encoding.GetBytes(this.NewLineChars, 0, this.NewLineChars.Length, bytes, byteCount1);
    return this.TransformBytes(bytes);
  }

  protected virtual byte[] TransformBytes(byte[] value) => value;

  protected virtual void RenderFormattedMessageToStream(
    LogEventInfo logEvent,
    StringBuilder formatBuilder,
    char[] transformBuffer,
    MemoryStream streamTarget)
  {
    this.RenderFormattedMessage(logEvent, formatBuilder);
    formatBuilder.Append(this.NewLineChars);
    this.TransformBuilderToStream(logEvent, formatBuilder, transformBuffer, streamTarget);
  }

  protected virtual void RenderFormattedMessage(LogEventInfo logEvent, StringBuilder target)
  {
    this.Layout.RenderAppendBuilder(logEvent, target);
  }

  private void TransformBuilderToStream(
    LogEventInfo logEvent,
    StringBuilder builder,
    char[] transformBuffer,
    MemoryStream workStream)
  {
    builder.CopyToStream(workStream, this.Encoding, transformBuffer);
    this.TransformStream(logEvent, workStream);
  }

  protected virtual void TransformStream(LogEventInfo logEvent, MemoryStream stream)
  {
  }

  private void FlushCurrentFileWrites(
    string currentFileName,
    LogEventInfo firstLogEvent,
    MemoryStream ms,
    out Exception lastException)
  {
    lastException = (Exception) null;
    try
    {
      if (currentFileName == null)
        return;
      ArraySegment<byte> bytesToWrite = new ArraySegment<byte>(ms.GetBuffer(), 0, (int) ms.Length);
      this.ProcessLogEvent(firstLogEvent, currentFileName, bytesToWrite);
    }
    catch (Exception ex)
    {
      if (this.ExceptionMustBeRethrown(ex))
        throw;
      lastException = ex;
    }
  }

  private void ArchiveFile(string fileName, string archiveFileName)
  {
    string directoryName = Path.GetDirectoryName(archiveFileName);
    if (directoryName != null && !Directory.Exists(directoryName))
      Directory.CreateDirectory(directoryName);
    if (string.Equals(fileName, archiveFileName, StringComparison.OrdinalIgnoreCase))
      InternalLogger.Info<string, string>("FileTarget(Name={0}): Archiving {1} skipped as ArchiveFileName equals FileName", this.Name, fileName);
    else if (this.EnableArchiveFileCompression)
    {
      InternalLogger.Info<string, string, string>("FileTarget(Name={0}): Archiving {1} to compressed {2}", this.Name, fileName, archiveFileName);
      FileTarget.FileCompressor.CompressFile(fileName, archiveFileName);
      this.DeleteAndWaitForFileDelete(fileName);
    }
    else
    {
      InternalLogger.Info<string, string, string>("FileTarget(Name={0}): Archiving {1} to {2}", this.Name, fileName, archiveFileName);
      if (File.Exists(archiveFileName))
        this.ArchiveFileAppendExisting(fileName, archiveFileName);
      else
        this.ArchiveFileMove(fileName, archiveFileName);
    }
  }

  private void ArchiveFileAppendExisting(string fileName, string archiveFileName)
  {
    InternalLogger.Info<string, string>("FileTarget(Name={0}): Already exists, append to {1}", this.Name, archiveFileName);
    FileShare share = FileShare.ReadWrite;
    if (this.EnableFileDelete)
      share |= FileShare.Delete;
    using (FileStream input = File.Open(fileName, FileMode.Open, System.IO.FileAccess.ReadWrite, share))
    {
      using (FileStream output = File.Open(archiveFileName, FileMode.Append))
      {
        input.CopyAndSkipBom((Stream) output, this.Encoding);
        input.SetLength(0L);
        if (this.EnableFileDelete && !this.DeleteOldArchiveFile(fileName))
          share &= ~FileShare.Delete;
        input.Close();
        output.Flush(true);
      }
    }
    if ((share & FileShare.Delete) != FileShare.None)
      return;
    this.DeleteOldArchiveFile(fileName);
  }

  private void ArchiveFileMove(string fileName, string archiveFileName)
  {
    try
    {
      InternalLogger.Debug<string, string, string>("FileTarget(Name={0}): Move file from '{1}' to '{2}'", this.Name, fileName, archiveFileName);
      File.Move(fileName, archiveFileName);
    }
    catch (IOException ex)
    {
      if (this.IsSimpleKeepFileOpen)
        throw;
      if (!this.EnableFileDelete && this.KeepFileOpen)
        throw;
      if (this.ConcurrentWrites && !PlatformDetector.SupportsSharableMutex)
        throw;
      object[] objArray = new object[3]
      {
        (object) this.Name,
        (object) fileName,
        (object) archiveFileName
      };
      InternalLogger.Warn((Exception) ex, "FileTarget(Name={0}): Archiving failed. Checking for retry move of {1} to {2}.", objArray);
      if (!File.Exists(fileName) || File.Exists(archiveFileName))
        throw;
      AsyncHelpers.WaitForDelay(TimeSpan.FromMilliseconds(50.0));
      if (!File.Exists(fileName) || File.Exists(archiveFileName))
        throw;
      InternalLogger.Debug<string, string, string>("FileTarget(Name={0}): Archiving retrying move of {1} to {2}.", this.Name, fileName, archiveFileName);
      File.Move(fileName, archiveFileName);
    }
  }

  private bool DeleteOldArchiveFile(string fileName)
  {
    try
    {
      InternalLogger.Info<string, string>("FileTarget(Name={0}): Deleting old archive file: '{1}'.", this.Name, fileName);
      File.Delete(fileName);
      return true;
    }
    catch (DirectoryNotFoundException ex)
    {
      object[] objArray = new object[2]
      {
        (object) this.Name,
        (object) fileName
      };
      InternalLogger.Debug((Exception) ex, "FileTarget(Name={0}): Failed to delete old log file '{1}' as directory is missing.", objArray);
      return false;
    }
    catch (Exception ex)
    {
      InternalLogger.Warn(ex, "FileTarget(Name={0}): Failed to delete old archive file: '{1}'.", (object) this.Name, (object) fileName);
      if (!this.ExceptionMustBeRethrown(ex))
        return false;
      throw;
    }
  }

  private void DeleteAndWaitForFileDelete(string fileName)
  {
    try
    {
      InternalLogger.Trace<string, string>("FileTarget(Name={0}): Waiting for file delete of '{1}' for 12 sec", this.Name, fileName);
      DateTime creationTime = new FileInfo(fileName).CreationTime;
      if (!this.DeleteOldArchiveFile(fileName) || !File.Exists(fileName))
        return;
      for (int index = 0; index < 120; ++index)
      {
        AsyncHelpers.WaitForDelay(TimeSpan.FromMilliseconds(100.0));
        FileInfo fileInfo = new FileInfo(fileName);
        if (!fileInfo.Exists || fileInfo.CreationTime != creationTime)
          return;
      }
      InternalLogger.Warn<string, string>("FileTarget(Name={0}): Timeout while deleting old archive file: '{1}'.", this.Name, fileName);
    }
    catch (Exception ex)
    {
      InternalLogger.Warn(ex, "FileTarget(Name={0}): Failed to delete old archive file: '{1}'.", (object) this.Name, (object) fileName);
      if (!this.ExceptionMustBeRethrown(ex))
        return;
      throw;
    }
  }

  private string GetArchiveDateFormatString(string defaultFormat)
  {
    if (!string.IsNullOrEmpty(defaultFormat))
      return defaultFormat;
    switch (this.ArchiveEvery)
    {
      case FileArchivePeriod.Year:
        return "yyyy";
      case FileArchivePeriod.Month:
        return "yyyyMM";
      case FileArchivePeriod.Hour:
        return "yyyyMMddHH";
      case FileArchivePeriod.Minute:
        return "yyyyMMddHHmm";
      default:
        return "yyyyMMdd";
    }
  }

  private DateTime? GetArchiveDate(
    string fileName,
    LogEventInfo logEvent,
    DateTime previousLogEventTimestamp)
  {
    DateTime? lastWriteTimeUtc = this._fileAppenderCache.GetFileLastWriteTimeUtc(fileName);
    InternalLogger.Trace<string, DateTime?, DateTime>("FileTarget(Name={0}): Calculating archive date. File-LastModifiedUtc: {1}; Previous LogEvent-TimeStamp: {2}", this.Name, lastWriteTimeUtc, previousLogEventTimestamp);
    if (!lastWriteTimeUtc.HasValue)
    {
      if (!(previousLogEventTimestamp == DateTime.MinValue))
        return new DateTime?(previousLogEventTimestamp);
      InternalLogger.Info<string, string>("FileTarget(Name={0}): Unable to acquire useful timestamp to archive file: {1}", this.Name, fileName);
      return new DateTime?();
    }
    DateTime lastFileWrite = TimeSource.Current.FromSystemTime(lastWriteTimeUtc.Value);
    if (previousLogEventTimestamp != DateTime.MinValue)
    {
      if (previousLogEventTimestamp > lastFileWrite)
      {
        InternalLogger.Trace<string, DateTime, DateTime>("FileTarget(Name={0}): Using previous LogEvent-TimeStamp {1}, because more recent than File-LastModified {2}", this.Name, previousLogEventTimestamp, lastFileWrite);
        return new DateTime?(previousLogEventTimestamp);
      }
      if (this.PreviousLogOverlappedPeriod(logEvent, previousLogEventTimestamp, lastFileWrite))
      {
        InternalLogger.Trace<string, DateTime, DateTime>("FileTarget(Name={0}): Using previous LogEvent-TimeStamp {1}, because archive period is overlapping with File-LastModified {2}", this.Name, previousLogEventTimestamp, lastFileWrite);
        return new DateTime?(previousLogEventTimestamp);
      }
      if (!this.AutoFlush && this.IsSimpleKeepFileOpen && previousLogEventTimestamp < lastFileWrite)
      {
        InternalLogger.Trace<string, DateTime, DateTime>("FileTarget(Name={0}): Using previous LogEvent-TimeStamp {1}, because AutoFlush=false affects File-LastModified {2}", this.Name, previousLogEventTimestamp, lastFileWrite);
        return new DateTime?(previousLogEventTimestamp);
      }
    }
    InternalLogger.Trace<string, DateTime>("FileTarget(Name={0}): Using last write time: {1}", this.Name, lastFileWrite);
    return new DateTime?(lastFileWrite);
  }

  private bool PreviousLogOverlappedPeriod(
    LogEventInfo logEvent,
    DateTime previousLogEventTimestamp,
    DateTime lastFileWrite)
  {
    string dateFormatString = this.GetArchiveDateFormatString(string.Empty);
    string str1 = lastFileWrite.ToString(dateFormatString, (IFormatProvider) CultureInfo.InvariantCulture);
    string str2 = logEvent.TimeStamp.ToString(dateFormatString, (IFormatProvider) CultureInfo.InvariantCulture);
    if (str1 != str2)
      return false;
    DateTime? archiveEventTime = this.CalculateNextArchiveEventTime(previousLogEventTimestamp);
    if (!archiveEventTime.HasValue)
      return false;
    string str3 = archiveEventTime.Value.ToString(dateFormatString, (IFormatProvider) CultureInfo.InvariantCulture);
    return str1 == str3;
  }

  private DateTime? CalculateNextArchiveEventTime(DateTime timestamp)
  {
    switch (this.ArchiveEvery)
    {
      case FileArchivePeriod.Year:
        return new DateTime?(timestamp.AddYears(1));
      case FileArchivePeriod.Month:
        return new DateTime?(timestamp.AddMonths(1));
      case FileArchivePeriod.Day:
        return new DateTime?(timestamp.AddDays(1.0));
      case FileArchivePeriod.Hour:
        return new DateTime?(timestamp.AddHours(1.0));
      case FileArchivePeriod.Minute:
        return new DateTime?(timestamp.AddMinutes(1.0));
      case FileArchivePeriod.Sunday:
        return new DateTime?(FileTarget.CalculateNextWeekday(timestamp, DayOfWeek.Sunday));
      case FileArchivePeriod.Monday:
        return new DateTime?(FileTarget.CalculateNextWeekday(timestamp, DayOfWeek.Monday));
      case FileArchivePeriod.Tuesday:
        return new DateTime?(FileTarget.CalculateNextWeekday(timestamp, DayOfWeek.Tuesday));
      case FileArchivePeriod.Wednesday:
        return new DateTime?(FileTarget.CalculateNextWeekday(timestamp, DayOfWeek.Wednesday));
      case FileArchivePeriod.Thursday:
        return new DateTime?(FileTarget.CalculateNextWeekday(timestamp, DayOfWeek.Thursday));
      case FileArchivePeriod.Friday:
        return new DateTime?(FileTarget.CalculateNextWeekday(timestamp, DayOfWeek.Friday));
      case FileArchivePeriod.Saturday:
        return new DateTime?(FileTarget.CalculateNextWeekday(timestamp, DayOfWeek.Saturday));
      default:
        return new DateTime?();
    }
  }

  public static DateTime CalculateNextWeekday(
    DateTime previousLogEventTimestamp,
    DayOfWeek dayOfWeek)
  {
    int dayOfWeek1 = (int) previousLogEventTimestamp.DayOfWeek;
    int num = (int) dayOfWeek;
    if (num <= dayOfWeek1)
      num += 7;
    return previousLogEventTimestamp.AddDays((double) (num - dayOfWeek1));
  }

  private void DoAutoArchive(
    string fileName,
    LogEventInfo eventInfo,
    DateTime previousLogEventTimestamp,
    bool initializedNewFile)
  {
    InternalLogger.Debug<string, string>("FileTarget(Name={0}): Do archive file: '{1}'", this.Name, fileName);
    FileInfo fileInfo = new FileInfo(fileName);
    if (!fileInfo.Exists)
    {
      this._fileAppenderCache.InvalidateAppender(fileName)?.Dispose();
    }
    else
    {
      string archiveFileNamePattern = this.GetArchiveFileNamePattern(fileName, eventInfo);
      if (string.IsNullOrEmpty(archiveFileNamePattern))
      {
        InternalLogger.Warn<string>("FileTarget(Name={0}): Skip auto archive because archiveFilePattern is NULL", this.Name);
      }
      else
      {
        DateTime? archiveDate = this.GetArchiveDate(fileName, eventInfo, previousLogEventTimestamp);
        string nameAfterCleanup = this.GenerateArchiveFileNameAfterCleanup(fileName, fileInfo, archiveFileNamePattern, archiveDate, initializedNewFile);
        if (string.IsNullOrEmpty(nameAfterCleanup))
          return;
        this.ArchiveFile(fileInfo.FullName, nameAfterCleanup);
      }
    }
  }

  private string GenerateArchiveFileNameAfterCleanup(
    string fileName,
    FileInfo fileInfo,
    string archiveFilePattern,
    DateTime? archiveDate,
    bool initializedNewFile)
  {
    InternalLogger.Trace<string, string>("FileTarget(Name={0}): Archive pattern '{1}'", this.Name, archiveFilePattern);
    IFileArchiveMode fileArchiveHelper = this.GetFileArchiveHelper(archiveFilePattern);
    List<DateAndSequenceArchive> existingArchiveFiles = fileArchiveHelper.GetExistingArchiveFiles(archiveFilePattern);
    if (this.MaxArchiveFiles == 1)
    {
      InternalLogger.Trace<string>("FileTarget(Name={0}): MaxArchiveFiles = 1", this.Name);
      for (int index = existingArchiveFiles.Count - 1; index >= 0; --index)
      {
        DateAndSequenceArchive andSequenceArchive = existingArchiveFiles[index];
        if (!string.Equals(andSequenceArchive.FileName, fileInfo.FullName, StringComparison.OrdinalIgnoreCase))
        {
          this.DeleteOldArchiveFile(andSequenceArchive.FileName);
          existingArchiveFiles.RemoveAt(index);
        }
      }
      if (initializedNewFile && string.Equals(Path.GetDirectoryName(archiveFilePattern), fileInfo.DirectoryName, StringComparison.OrdinalIgnoreCase))
      {
        this.DeleteOldArchiveFile(fileName);
        return (string) null;
      }
    }
    DateAndSequenceArchive archiveFileName = archiveDate.HasValue ? fileArchiveHelper.GenerateArchiveFileName(archiveFilePattern, archiveDate.Value, existingArchiveFiles) : (DateAndSequenceArchive) null;
    if (archiveFileName == null)
      return (string) null;
    if (!initializedNewFile)
      this.FinalizeFile(fileName, true);
    if (existingArchiveFiles.Count > 0)
      this.CleanupOldArchiveFiles(fileInfo, archiveFilePattern, existingArchiveFiles, archiveFileName);
    return archiveFileName.FileName;
  }

  private void CleanupOldArchiveFiles(
    FileInfo currentFile,
    string archiveFilePattern,
    List<DateAndSequenceArchive> existingArchiveFiles,
    DateAndSequenceArchive newArchiveFile = null)
  {
    IFileArchiveMode fileArchiveHelper = this.GetFileArchiveHelper(archiveFilePattern);
    if (!fileArchiveHelper.IsArchiveCleanupEnabled)
      return;
    if (currentFile != null)
      FileTarget.ExcludeActiveFileFromOldArchiveFiles(currentFile, existingArchiveFiles);
    if (newArchiveFile != null)
      existingArchiveFiles.Add(newArchiveFile);
    foreach (DateAndSequenceArchive andSequenceArchive in fileArchiveHelper.CheckArchiveCleanup(archiveFilePattern, existingArchiveFiles, this.MaxArchiveFiles, this.MaxArchiveDays))
      this.DeleteOldArchiveFile(andSequenceArchive.FileName);
  }

  private static void ExcludeActiveFileFromOldArchiveFiles(
    FileInfo currentFile,
    List<DateAndSequenceArchive> existingArchiveFiles)
  {
    if (existingArchiveFiles.Count <= 0 || !string.Equals(Path.GetDirectoryName(existingArchiveFiles[0].FileName), currentFile.DirectoryName, StringComparison.OrdinalIgnoreCase))
      return;
    for (int index = 0; index < existingArchiveFiles.Count; ++index)
    {
      if (string.Equals(existingArchiveFiles[index].FileName, currentFile.FullName, StringComparison.OrdinalIgnoreCase))
      {
        existingArchiveFiles.RemoveAt(index);
        break;
      }
    }
  }

  private string GetArchiveFileNamePattern(string fileName, LogEventInfo eventInfo)
  {
    if (this._fullArchiveFileName != null)
      return this._fullArchiveFileName.Render(eventInfo);
    return this.EnableArchiveFileCompression ? Path.ChangeExtension(fileName, ".zip") : fileName;
  }

  private bool TryArchiveFile(
    string fileName,
    LogEventInfo ev,
    int upcomingWriteSize,
    DateTime previousLogEventTimestamp,
    bool initializedNewFile)
  {
    if (!this.IsArchivingEnabled)
      return false;
    string archiveFile = string.Empty;
    BaseFileAppender baseFileAppender = (BaseFileAppender) null;
    try
    {
      archiveFile = this.GetArchiveFileName(fileName, ev, upcomingWriteSize, previousLogEventTimestamp, initializedNewFile);
      if (!string.IsNullOrEmpty(archiveFile))
        baseFileAppender = this.TryCloseFileAppenderBeforeArchive(fileName, archiveFile);
      this._fileAppenderCache.InvalidateAppendersForArchivedFiles();
    }
    catch (Exception ex)
    {
      InternalLogger.Warn(ex, "FileTarget(Name={0}): Failed to check archive for file '{1}'.", (object) this.Name, (object) fileName);
      if (this.ExceptionMustBeRethrown(ex))
        throw;
    }
    if (string.IsNullOrEmpty(archiveFile))
      return false;
    try
    {
      try
      {
        if (baseFileAppender is BaseMutexFileAppender mutexFileAppender && mutexFileAppender.ArchiveMutex != null)
          mutexFileAppender.ArchiveMutex.WaitOne();
        else if (!this.IsSimpleKeepFileOpen)
          InternalLogger.Debug<string, string>("FileTarget(Name={0}): Archive mutex not available for file '{1}'", this.Name, archiveFile);
      }
      catch (AbandonedMutexException ex)
      {
      }
      this.ArchiveFileAfterCloseFileAppender(archiveFile, ev, upcomingWriteSize, previousLogEventTimestamp);
      return true;
    }
    finally
    {
      if (baseFileAppender is BaseMutexFileAppender mutexFileAppender)
        mutexFileAppender.ArchiveMutex?.ReleaseMutex();
      baseFileAppender?.Dispose();
    }
  }

  private BaseFileAppender TryCloseFileAppenderBeforeArchive(string fileName, string archiveFile)
  {
    InternalLogger.Trace<string, string>("FileTarget(Name={0}): Archive attempt for file '{1}'", this.Name, archiveFile);
    BaseFileAppender baseFileAppender1 = this._fileAppenderCache.InvalidateAppender(fileName);
    if (fileName != archiveFile)
    {
      BaseFileAppender baseFileAppender2 = this._fileAppenderCache.InvalidateAppender(archiveFile);
      baseFileAppender1 = baseFileAppender1 ?? baseFileAppender2;
    }
    if (!string.IsNullOrEmpty(this._previousLogFileName) && this._previousLogFileName != archiveFile && this._previousLogFileName != fileName)
    {
      BaseFileAppender baseFileAppender3 = this._fileAppenderCache.InvalidateAppender(this._previousLogFileName);
      baseFileAppender1 = baseFileAppender1 ?? baseFileAppender3;
    }
    return baseFileAppender1;
  }

  private void ArchiveFileAfterCloseFileAppender(
    string archiveFile,
    LogEventInfo ev,
    int upcomingWriteSize,
    DateTime previousLogEventTimestamp)
  {
    try
    {
      string archiveFileName = this.GetArchiveFileName(archiveFile, ev, upcomingWriteSize, previousLogEventTimestamp, false);
      if (string.IsNullOrEmpty(archiveFileName))
      {
        InternalLogger.Debug<string, string>("FileTarget(Name={0}): Skip archiving '{1}' because no longer necessary", this.Name, archiveFile);
        this._initializedFiles.Remove(archiveFile);
      }
      else
      {
        if (archiveFile != archiveFileName)
        {
          this._initializedFiles.Remove(archiveFile);
          archiveFile = archiveFileName;
        }
        this._initializedFiles.Remove(archiveFile);
        this.DoAutoArchive(archiveFile, ev, previousLogEventTimestamp, false);
      }
      if (!(this._previousLogFileName == archiveFile))
        return;
      this._previousLogFileName = (string) null;
      this._previousLogEventTimestamp = new DateTime?();
    }
    catch (Exception ex)
    {
      InternalLogger.Warn(ex, "FileTarget(Name={0}): Failed to archive file '{1}'.", (object) this.Name, (object) archiveFile);
      if (!this.ExceptionMustBeRethrown(ex))
        return;
      throw;
    }
  }

  private string GetArchiveFileName(
    string fileName,
    LogEventInfo ev,
    int upcomingWriteSize,
    DateTime previousLogEventTimestamp,
    bool initializedNewFile)
  {
    fileName = fileName ?? this._previousLogFileName;
    return !string.IsNullOrEmpty(fileName) ? this.GetArchiveFileNameBasedOnFileSize(fileName, upcomingWriteSize, initializedNewFile) ?? this.GetArchiveFileNameBasedOnTime(fileName, ev, previousLogEventTimestamp, initializedNewFile) : (string) null;
  }

  private string GetPotentialFileForArchiving(string fileName)
  {
    return !string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(this._previousLogFileName) ? fileName : this._previousLogFileName;
  }

  private string GetArchiveFileNameBasedOnFileSize(
    string fileName,
    int upcomingWriteSize,
    bool initializedNewFile)
  {
    if (this.ArchiveAboveSize <= -1L)
      return (string) null;
    string fileForArchiving = this.GetPotentialFileForArchiving(fileName);
    if (string.IsNullOrEmpty(fileForArchiving))
      return (string) null;
    long? fileLength = this._fileAppenderCache.GetFileLength(fileForArchiving);
    if (!fileLength.HasValue)
    {
      string previousLogFileName = this.TryFallbackToPreviousLogFileName(fileForArchiving, initializedNewFile);
      if (string.IsNullOrEmpty(previousLogFileName))
        return (string) null;
      upcomingWriteSize = 0;
      return this.GetArchiveFileNameBasedOnFileSize(previousLogFileName, upcomingWriteSize, false);
    }
    if (fileForArchiving != fileName)
      upcomingWriteSize = 0;
    if (fileLength.Value + (long) upcomingWriteSize <= this.ArchiveAboveSize)
      return (string) null;
    InternalLogger.Debug("FileTarget(Name={0}): Start archiving '{1}' because FileSize={2} + {3} is larger than ArchiveAboveSize={4}", (object) this.Name, (object) fileForArchiving, (object) fileLength.Value, (object) upcomingWriteSize, (object) this.ArchiveAboveSize);
    return fileForArchiving;
  }

  private string TryFallbackToPreviousLogFileName(string archiveFileName, bool initializedNewFile)
  {
    if (!initializedNewFile && this._initializedFiles.Remove(archiveFileName))
    {
      InternalLogger.Debug<string, string>("FileTarget(Name={0}): Invalidates appender for archive file '{1}' since it no longer exists", this.Name, archiveFileName);
      this._fileAppenderCache.InvalidateAppender(archiveFileName)?.Dispose();
    }
    return !string.IsNullOrEmpty(this._previousLogFileName) && !string.Equals(archiveFileName, this._previousLogFileName, StringComparison.OrdinalIgnoreCase) ? this._previousLogFileName : string.Empty;
  }

  private string GetArchiveFileNameBasedOnTime(
    string fileName,
    LogEventInfo logEvent,
    DateTime previousLogEventTimestamp,
    bool initializedNewFile)
  {
    if (this.ArchiveEvery == FileArchivePeriod.None)
      return (string) null;
    string fileForArchiving = this.GetPotentialFileForArchiving(fileName);
    if (string.IsNullOrEmpty(fileForArchiving))
      return (string) null;
    DateTime? creationTimeSource = this.TryGetArchiveFileCreationTimeSource(fileForArchiving, previousLogEventTimestamp);
    if (!creationTimeSource.HasValue)
    {
      string previousLogFileName = this.TryFallbackToPreviousLogFileName(fileForArchiving, initializedNewFile);
      return !string.IsNullOrEmpty(previousLogFileName) ? this.GetArchiveFileNameBasedOnTime(previousLogFileName, logEvent, previousLogEventTimestamp, false) : (string) null;
    }
    if (FileTarget.TruncateArchiveTime(creationTimeSource.Value, this.ArchiveEvery) != FileTarget.TruncateArchiveTime(logEvent.TimeStamp, this.ArchiveEvery))
    {
      string dateFormatString = this.GetArchiveDateFormatString(string.Empty);
      DateTime dateTime = this.EnsureValidLogEventTimeStamp(logEvent.TimeStamp, creationTimeSource.Value);
      string str1 = creationTimeSource.Value.ToString(dateFormatString, (IFormatProvider) CultureInfo.InvariantCulture);
      string str2 = dateTime.ToString(dateFormatString, (IFormatProvider) CultureInfo.InvariantCulture);
      if (str1 != str2)
      {
        InternalLogger.Debug("FileTarget(Name={0}): Start archiving '{1}' because FileCreatedTime='{2}' is older than now '{3}' using ArchiveEvery='{4}'", (object) this.Name, (object) fileForArchiving, (object) str1, (object) str2, (object) dateFormatString);
        return fileForArchiving;
      }
    }
    return (string) null;
  }

  private DateTime? TryGetArchiveFileCreationTimeSource(
    string fileName,
    DateTime previousLogEventTimestamp)
  {
    DateTime? fallbackTimeSource = !(previousLogEventTimestamp != DateTime.MinValue) || !this.IsSimpleKeepFileOpen ? new DateTime?() : new DateTime?(previousLogEventTimestamp);
    DateTime? creationTimeSource = this._fileAppenderCache.GetFileCreationTimeSource(fileName, fallbackTimeSource);
    if (!creationTimeSource.HasValue)
      return new DateTime?();
    if (previousLogEventTimestamp != DateTime.MinValue)
    {
      DateTime dateTime = previousLogEventTimestamp;
      DateTime? nullable = creationTimeSource;
      if ((nullable.HasValue ? (dateTime < nullable.GetValueOrDefault() ? 1 : 0) : 0) != 0 && FileTarget.TruncateArchiveTime(previousLogEventTimestamp, FileArchivePeriod.Minute) < FileTarget.TruncateArchiveTime(creationTimeSource.Value, FileArchivePeriod.Minute) && PlatformDetector.IsUnix)
      {
        if (this.IsSimpleKeepFileOpen)
        {
          InternalLogger.Debug<string, DateTime?, DateTime>("FileTarget(Name={0}): Adjusted file creation time from {1} to {2}. Linux FileSystem probably don't support file birthtime.", this.Name, creationTimeSource, previousLogEventTimestamp);
          creationTimeSource = new DateTime?(previousLogEventTimestamp);
        }
        else
          InternalLogger.Debug<string, DateTime?, DateTime>("FileTarget(Name={0}): File creation time {1} newer than previous file write time {2}. Linux FileSystem probably don't support file birthtime, unless multiple applications are writing to the same file. Configure FileTarget.KeepFileOpen=true AND FileTarget.ConcurrentWrites=false, so NLog can fix this.", this.Name, creationTimeSource, previousLogEventTimestamp);
      }
    }
    return creationTimeSource;
  }

  private static DateTime TruncateArchiveTime(DateTime input, FileArchivePeriod resolution)
  {
    switch (resolution)
    {
      case FileArchivePeriod.Year:
        return new DateTime(input.Year, 1, 1, 0, 0, 0, 0, input.Kind);
      case FileArchivePeriod.Month:
        return new DateTime(input.Year, input.Month, 1, 0, 0, 0, input.Kind);
      case FileArchivePeriod.Day:
        return input.Date;
      case FileArchivePeriod.Hour:
        return input.AddTicks(-(input.Ticks % 36000000000L));
      case FileArchivePeriod.Minute:
        return input.AddTicks(-(input.Ticks % 600000000L));
      case FileArchivePeriod.Sunday:
        return FileTarget.CalculateNextWeekday(input.Date, DayOfWeek.Sunday);
      case FileArchivePeriod.Monday:
        return FileTarget.CalculateNextWeekday(input.Date, DayOfWeek.Monday);
      case FileArchivePeriod.Tuesday:
        return FileTarget.CalculateNextWeekday(input.Date, DayOfWeek.Tuesday);
      case FileArchivePeriod.Wednesday:
        return FileTarget.CalculateNextWeekday(input.Date, DayOfWeek.Wednesday);
      case FileArchivePeriod.Thursday:
        return FileTarget.CalculateNextWeekday(input.Date, DayOfWeek.Thursday);
      case FileArchivePeriod.Friday:
        return FileTarget.CalculateNextWeekday(input.Date, DayOfWeek.Friday);
      case FileArchivePeriod.Saturday:
        return FileTarget.CalculateNextWeekday(input.Date, DayOfWeek.Saturday);
      default:
        return input;
    }
  }

  private void AutoCloseAppendersAfterArchive(object sender, EventArgs state)
  {
    if (!Monitor.TryEnter(this.SyncRoot, TimeSpan.FromSeconds(2.0)))
      return;
    try
    {
      if (!this.IsInitialized)
        return;
      InternalLogger.Trace<string>("FileTarget(Name={0}): Auto Close FileAppenders after archive", this.Name);
      this._fileAppenderCache.CloseAppenders(DateTime.MinValue);
    }
    catch (Exception ex)
    {
      InternalLogger.Warn(ex, "FileTarget(Name={0}): Exception in AutoCloseAppendersAfterArchive", (object) this.Name);
      if (!ex.MustBeRethrownImmediately())
        return;
      throw;
    }
    finally
    {
      Monitor.Exit(this.SyncRoot);
    }
  }

  private void AutoClosingTimerCallback(object sender, EventArgs state)
  {
    if (!Monitor.TryEnter(this.SyncRoot, TimeSpan.FromSeconds(0.5)))
      return;
    try
    {
      if (!this.IsInitialized)
        return;
      if (this.OpenFileCacheTimeout > 0)
      {
        DateTime expireTime = DateTime.UtcNow.AddSeconds((double) -this.OpenFileCacheTimeout);
        InternalLogger.Trace<string>("FileTarget(Name={0}): Auto Close FileAppenders", this.Name);
        this._fileAppenderCache.CloseAppenders(expireTime);
      }
      if (this.OpenFileFlushTimeout <= 0 || this.AutoFlush)
        return;
      this.ConditionalFlushOpenFileAppenders();
    }
    catch (Exception ex)
    {
      InternalLogger.Warn(ex, "FileTarget(Name={0}): Exception in AutoClosingTimerCallback", (object) this.Name);
      if (!ex.MustBeRethrownImmediately())
        return;
      throw;
    }
    finally
    {
      Monitor.Exit(this.SyncRoot);
    }
  }

  private void ConditionalFlushOpenFileAppenders()
  {
    DateTime dateTime = TimeSource.Current.Time.AddSeconds((double) (-Math.Max(this.OpenFileFlushTimeout, 5) * 2));
    bool flag = false;
    foreach (KeyValuePair<string, DateTime> initializedFile in this._initializedFiles)
    {
      if (initializedFile.Value > dateTime)
      {
        flag = true;
        break;
      }
    }
    if (!flag)
      return;
    InternalLogger.Trace<string>("FileTarget(Name={0}): Auto Flush FileAppenders", this.Name);
    this._fileAppenderCache.FlushAppenders();
  }

  private void WriteToFile(string fileName, ArraySegment<byte> bytes, bool initializedNewFile)
  {
    BaseFileAppender appender = this._fileAppenderCache.AllocateAppender(fileName);
    try
    {
      if (initializedNewFile)
        this.WriteHeaderAndBom(appender);
      appender.Write(bytes.Array, bytes.Offset, bytes.Count);
      if (!this.AutoFlush)
        return;
      appender.Flush();
    }
    catch (Exception ex)
    {
      object[] objArray = new object[2]
      {
        (object) this.Name,
        (object) fileName
      };
      InternalLogger.Error(ex, "FileTarget(Name={0}): Failed write to file '{1}'.", objArray);
      this._fileAppenderCache.InvalidateAppender(fileName)?.Dispose();
      throw;
    }
  }

  private DateTime InitializeFile(string fileName, LogEventInfo logEvent)
  {
    if (this._initializedFiles.Count != 0 && this._previousLogEventTimestamp.HasValue && this._previousLogFileName == fileName && logEvent.TimeStamp == this._previousLogEventTimestamp.Value)
      return this._previousLogEventTimestamp.Value;
    DateTime timeStamp = logEvent.TimeStamp;
    DateTime previousTimeStamp;
    if (!this._initializedFiles.TryGetValue(fileName, out previousTimeStamp))
    {
      this.PrepareForNewFile(fileName, logEvent);
      ++this._initializedFilesCounter;
      if (this._initializedFilesCounter >= 25)
      {
        this._initializedFilesCounter = 0;
        this.CleanupInitializedFiles();
      }
      this._initializedFiles[fileName] = timeStamp;
      return DateTime.MinValue;
    }
    if (previousTimeStamp != timeStamp)
    {
      DateTime dateTime = this.EnsureValidLogEventTimeStamp(timeStamp, previousTimeStamp);
      this._initializedFiles[fileName] = dateTime;
    }
    return previousTimeStamp;
  }

  private DateTime EnsureValidLogEventTimeStamp(
    DateTime logEventTimeStamp,
    DateTime previousTimeStamp)
  {
    if (logEventTimeStamp < previousTimeStamp && logEventTimeStamp.Date < previousTimeStamp.Date)
    {
      DateTime time = TimeSource.Current.Time;
      if (logEventTimeStamp.Date < time.AddMinutes(-1.0).Date)
        return time.Date < previousTimeStamp.Date ? time : previousTimeStamp;
    }
    return logEventTimeStamp;
  }

  private void FinalizeFile(string fileName, bool isArchiving = false)
  {
    try
    {
      InternalLogger.Trace<string, string, bool>("FileTarget(Name={0}): FinalizeFile '{1}, isArchiving: {2}'", this.Name, fileName, isArchiving);
      if (isArchiving || !this.WriteFooterOnArchivingOnly)
        this.WriteFooter(fileName);
      this._fileAppenderCache.InvalidateAppender(fileName)?.Dispose();
    }
    finally
    {
      this._initializedFiles.Remove(fileName);
    }
  }

  private void WriteFooter(string fileName)
  {
    ArraySegment<byte> layoutBytes = this.GetLayoutBytes(this.Footer);
    if (layoutBytes.Count <= 0 || !File.Exists(fileName))
      return;
    this.WriteToFile(fileName, layoutBytes, false);
  }

  internal bool ShouldArchiveOldFileOnStartup(string fileName)
  {
    bool? oldFileOnStartup1 = this._archiveOldFileOnStartup;
    bool flag1 = false;
    if (oldFileOnStartup1.GetValueOrDefault() == flag1 & oldFileOnStartup1.HasValue)
      return false;
    if (this.ArchiveOldFileOnStartupAboveSize > 0L)
    {
      long? fileLength = this._fileAppenderCache.GetFileLength(fileName);
      return fileLength.HasValue && fileLength.Value > this.ArchiveOldFileOnStartupAboveSize;
    }
    bool? oldFileOnStartup2 = this._archiveOldFileOnStartup;
    bool flag2 = true;
    return oldFileOnStartup2.GetValueOrDefault() == flag2 & oldFileOnStartup2.HasValue;
  }

  private void PrepareForNewFile(string fileName, LogEventInfo logEvent)
  {
    InternalLogger.Debug<string, string>("FileTarget(Name={0}): Preparing for new file '{1}'", this.Name, fileName);
    this.RefreshArchiveFilePatternToWatch(fileName, logEvent);
    try
    {
      if (this.ShouldArchiveOldFileOnStartup(fileName))
        this.DoAutoArchive(fileName, logEvent, DateTime.MinValue, true);
    }
    catch (Exception ex)
    {
      InternalLogger.Warn(ex, "FileTarget(Name={0}): Unable to archive old log file '{1}'.", (object) this.Name, (object) fileName);
      if (this.ExceptionMustBeRethrown(ex))
        throw;
    }
    if (this.DeleteOldFileOnStartup)
      this.DeleteOldArchiveFile(fileName);
    try
    {
      string archiveFileNamePattern = this.GetArchiveFileNamePattern(fileName, logEvent);
      if (string.IsNullOrEmpty(archiveFileNamePattern))
        return;
      IFileArchiveMode fileArchiveHelper = this.GetFileArchiveHelper(archiveFileNamePattern);
      if (!fileArchiveHelper.AttemptCleanupOnInitializeFile(archiveFileNamePattern, this.MaxArchiveFiles, this.MaxArchiveDays))
        return;
      List<DateAndSequenceArchive> existingArchiveFiles = fileArchiveHelper.GetExistingArchiveFiles(archiveFileNamePattern);
      if (existingArchiveFiles.Count <= 0)
        return;
      this.CleanupOldArchiveFiles(new FileInfo(fileName), archiveFileNamePattern, existingArchiveFiles);
    }
    catch (Exception ex)
    {
      InternalLogger.Warn(ex, "FileTarget(Name={0}): Failed to cleanup old archive files when starting on new file: '{1}'", (object) this.Name, (object) fileName);
      if (!this.ExceptionMustBeRethrown(ex))
        return;
      throw;
    }
  }

  private void ReplaceFileContent(string fileName, ArraySegment<byte> bytes, bool firstAttempt)
  {
    try
    {
      using (FileStream fileStream = File.Create(fileName))
      {
        ArraySegment<byte> layoutBytes1 = this.GetLayoutBytes(this.Header);
        if (layoutBytes1.Count > 0)
          fileStream.Write(layoutBytes1.Array, layoutBytes1.Offset, layoutBytes1.Count);
        fileStream.Write(bytes.Array, bytes.Offset, bytes.Count);
        ArraySegment<byte> layoutBytes2 = this.GetLayoutBytes(this.Footer);
        if (layoutBytes2.Count <= 0)
          return;
        fileStream.Write(layoutBytes2.Array, layoutBytes2.Offset, layoutBytes2.Count);
      }
    }
    catch (DirectoryNotFoundException ex)
    {
      if (!this.CreateDirs || !firstAttempt)
        throw;
      Directory.CreateDirectory(Path.GetDirectoryName(fileName));
      this.ReplaceFileContent(fileName, bytes, false);
    }
  }

  private void WriteHeaderAndBom(BaseFileAppender appender)
  {
    if (this.Header == null && !this.WriteBom)
      return;
    long? fileLength = appender.GetFileLength();
    if (fileLength.HasValue)
    {
      long? nullable = fileLength;
      long num = 0;
      if (!(nullable.GetValueOrDefault() == num & nullable.HasValue))
        return;
    }
    if (this.WriteBom)
    {
      InternalLogger.Trace<string, Encoding>("FileTarget(Name={0}): Write byte order mark from encoding={1}", this.Name, this.Encoding);
      byte[] preamble = this.Encoding.GetPreamble();
      if (preamble.Length != 0)
        appender.Write(preamble, 0, preamble.Length);
    }
    if (this.Header == null)
      return;
    InternalLogger.Trace<string>("FileTarget(Name={0}): Write header", this.Name);
    ArraySegment<byte> layoutBytes = this.GetLayoutBytes(this.Header);
    if (layoutBytes.Count <= 0)
      return;
    appender.Write(layoutBytes.Array, layoutBytes.Offset, layoutBytes.Count);
  }

  private ArraySegment<byte> GetLayoutBytes(Layout layout)
  {
    if (layout == null)
      return new ArraySegment<byte>();
    if (!this.OptimizeBufferReuse)
      return new ArraySegment<byte>(this.TransformBytes(this.Encoding.GetBytes(layout.Render(LogEventInfo.CreateNullEvent()) + this.NewLineChars)));
    using (ReusableObjectCreator<StringBuilder>.LockOject lockOject1 = this.ReusableLayoutBuilder.Allocate())
    {
      using (ReusableObjectCreator<char[]>.LockOject lockOject2 = this._reusableEncodingBuffer.Allocate())
      {
        LogEventInfo nullEvent = LogEventInfo.CreateNullEvent();
        layout.RenderAppendBuilder(nullEvent, lockOject1.Result);
        lockOject1.Result.Append(this.NewLineChars);
        using (MemoryStream workStream = new MemoryStream(lockOject1.Result.Length))
        {
          this.TransformBuilderToStream(nullEvent, lockOject1.Result, lockOject2.Result, workStream);
          return new ArraySegment<byte>(workStream.ToArray());
        }
      }
    }
  }
}
