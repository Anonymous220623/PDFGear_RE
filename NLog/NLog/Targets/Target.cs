// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Target
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using NLog.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

#nullable disable
namespace NLog.Targets;

[NLogConfigurationItem]
public abstract class Target : ISupportsInitialize, IInternalLoggerContext, IDisposable
{
  private List<Layout> _allLayouts;
  private bool _allLayoutsAreThreadAgnostic;
  private bool _allLayoutsAreThreadSafe;
  private bool _oneLayoutIsMutableUnsafe;
  private bool _scannedForLayouts;
  private Exception _initializeException;
  private volatile bool _isInitialized;
  internal readonly ReusableBuilderCreator ReusableLayoutBuilder = new ReusableBuilderCreator();
  private StringBuilderPool _precalculateStringBuilderPool;

  internal StackTraceUsage StackTraceUsage { get; private set; }

  public string Name { get; set; }

  public bool OptimizeBufferReuse { get; set; }

  protected object SyncRoot { get; } = new object();

  protected LoggingConfiguration LoggingConfiguration { get; private set; }

  LogFactory IInternalLoggerContext.LogFactory => this.LoggingConfiguration?.LogFactory;

  protected bool IsInitialized
  {
    get
    {
      if (this._isInitialized)
        return true;
      lock (this.SyncRoot)
        return this._isInitialized;
    }
  }

  void ISupportsInitialize.Initialize(LoggingConfiguration configuration)
  {
    lock (this.SyncRoot)
    {
      int num = this._isInitialized ? 1 : 0;
      this.Initialize(configuration);
      if (num == 0 || configuration == null)
        return;
      this.FindAllLayouts();
    }
  }

  void ISupportsInitialize.Close() => this.Close();

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  public void Flush(AsyncContinuation asyncContinuation)
  {
    asyncContinuation = asyncContinuation != null ? AsyncHelpers.PreventMultipleCalls(asyncContinuation) : throw new ArgumentNullException(nameof (asyncContinuation));
    lock (this.SyncRoot)
    {
      if (!this.IsInitialized)
      {
        asyncContinuation((Exception) null);
      }
      else
      {
        try
        {
          this.FlushAsync(asyncContinuation);
        }
        catch (Exception ex)
        {
          if (this.ExceptionMustBeRethrown(ex))
            throw;
          asyncContinuation(ex);
        }
      }
    }
  }

  public void PrecalculateVolatileLayouts(LogEventInfo logEvent)
  {
    if (this._allLayoutsAreThreadAgnostic && (!this._oneLayoutIsMutableUnsafe || logEvent.IsLogEventMutableSafe()))
      return;
    if (this.OptimizeBufferReuse && this._allLayoutsAreThreadSafe)
      this.PrecalculateVolatileLayoutsConcurrent(logEvent);
    else
      this.PrecalculateVolatileLayoutsWithLock(logEvent);
  }

  private void PrecalculateVolatileLayoutsConcurrent(LogEventInfo logEvent)
  {
    if (!this.IsInitialized || this._allLayouts == null)
      return;
    if (this._precalculateStringBuilderPool == null)
      Interlocked.CompareExchange<StringBuilderPool>(ref this._precalculateStringBuilderPool, new StringBuilderPool(Environment.ProcessorCount * 2), (StringBuilderPool) null);
    using (StringBuilderPool.ItemHolder itemHolder = this._precalculateStringBuilderPool.Acquire())
    {
      foreach (Layout allLayout in this._allLayouts)
      {
        itemHolder.Item.ClearBuilder();
        LogEventInfo logEvent1 = logEvent;
        StringBuilder target = itemHolder.Item;
        allLayout.PrecalculateBuilder(logEvent1, target);
      }
    }
  }

  private void PrecalculateVolatileLayoutsWithLock(LogEventInfo logEvent)
  {
    lock (this.SyncRoot)
    {
      if (!this._isInitialized || this._allLayouts == null)
        return;
      if (this.OptimizeBufferReuse)
      {
        using (ReusableObjectCreator<StringBuilder>.LockOject lockOject = this.ReusableLayoutBuilder.Allocate())
        {
          foreach (Layout allLayout in this._allLayouts)
          {
            lockOject.Result.ClearBuilder();
            LogEventInfo logEvent1 = logEvent;
            StringBuilder result = lockOject.Result;
            allLayout.PrecalculateBuilder(logEvent1, result);
          }
        }
      }
      else
      {
        foreach (Layout allLayout in this._allLayouts)
          allLayout.Precalculate(logEvent);
      }
    }
  }

  public override string ToString()
  {
    TargetAttribute customAttribute = this.GetType().GetCustomAttribute<TargetAttribute>();
    return customAttribute != null ? $"{customAttribute.Name} Target[{this.Name ?? "(unnamed)"}]" : this.GetType().Name;
  }

  public void WriteAsyncLogEvent(AsyncLogEventInfo logEvent)
  {
    if (!this.IsInitialized)
    {
      lock (this.SyncRoot)
        logEvent.Continuation((Exception) null);
    }
    else if (this._initializeException != null)
    {
      lock (this.SyncRoot)
        logEvent.Continuation(this.CreateInitException());
    }
    else
    {
      AsyncContinuation asyncContinuation = AsyncHelpers.PreventMultipleCalls(logEvent.Continuation);
      AsyncLogEventInfo logEvent1 = logEvent.LogEvent.WithContinuation(asyncContinuation);
      try
      {
        this.WriteAsyncThreadSafe(logEvent1);
      }
      catch (Exception ex)
      {
        if (this.ExceptionMustBeRethrown(ex))
          throw;
        logEvent1.Continuation(ex);
      }
    }
  }

  public void WriteAsyncLogEvents(params AsyncLogEventInfo[] logEvents)
  {
    if (logEvents == null || logEvents.Length == 0)
      return;
    this.WriteAsyncLogEvents((IList<AsyncLogEventInfo>) logEvents);
  }

  public void WriteAsyncLogEvents(IList<AsyncLogEventInfo> logEvents)
  {
    if (logEvents == null || logEvents.Count == 0)
      return;
    if (!this.IsInitialized)
    {
      lock (this.SyncRoot)
      {
        for (int index = 0; index < logEvents.Count; ++index)
          logEvents[index].Continuation((Exception) null);
      }
    }
    else if (this._initializeException != null)
    {
      lock (this.SyncRoot)
      {
        for (int index = 0; index < logEvents.Count; ++index)
          logEvents[index].Continuation(this.CreateInitException());
      }
    }
    else
    {
      IList<AsyncLogEventInfo> logEvents1;
      if (this.OptimizeBufferReuse)
      {
        for (int index = 0; index < logEvents.Count; ++index)
          logEvents[index] = logEvents[index].LogEvent.WithContinuation(AsyncHelpers.PreventMultipleCalls(logEvents[index].Continuation));
        logEvents1 = logEvents;
      }
      else
      {
        AsyncLogEventInfo[] asyncLogEventInfoArray = new AsyncLogEventInfo[logEvents.Count];
        for (int index = 0; index < logEvents.Count; ++index)
        {
          AsyncLogEventInfo logEvent = logEvents[index];
          asyncLogEventInfoArray[index] = logEvent.LogEvent.WithContinuation(AsyncHelpers.PreventMultipleCalls(logEvent.Continuation));
        }
        logEvents1 = (IList<AsyncLogEventInfo>) asyncLogEventInfoArray;
      }
      try
      {
        this.WriteAsyncThreadSafe(logEvents1);
      }
      catch (Exception ex)
      {
        if (this.ExceptionMustBeRethrown(ex))
          throw;
        for (int index = 0; index < logEvents1.Count; ++index)
          logEvents1[index].Continuation(ex);
      }
    }
  }

  internal void Initialize(LoggingConfiguration configuration)
  {
    lock (this.SyncRoot)
    {
      this.LoggingConfiguration = configuration;
      if (this.IsInitialized)
        return;
      try
      {
        PropertyHelper.CheckRequiredParameters((object) this);
        this.InitializeTarget();
        this._initializeException = (Exception) null;
        if (this._scannedForLayouts)
          return;
        InternalLogger.Debug<Target>("{0}: InitializeTarget is done but not scanned For Layouts", this);
        this.FindAllLayouts();
      }
      catch (Exception ex)
      {
        InternalLogger.Error(ex, "{0}: Error initializing target", (object) this);
        this._initializeException = ex;
        if (!this.ExceptionMustBeRethrown(ex))
          return;
        throw;
      }
      finally
      {
        this._isInitialized = true;
      }
    }
  }

  internal void Close()
  {
    lock (this.SyncRoot)
    {
      this.LoggingConfiguration = (LoggingConfiguration) null;
      if (!this.IsInitialized)
        return;
      this._isInitialized = false;
      try
      {
        if (this._initializeException != null)
          return;
        InternalLogger.Debug<Target>("Closing target '{0}'.", this);
        this.CloseTarget();
        InternalLogger.Debug<Target>("Closed target '{0}'.", this);
      }
      catch (Exception ex)
      {
        InternalLogger.Error(ex, "{0}: Error closing target", (object) this);
        if (!this.ExceptionMustBeRethrown(ex))
          return;
        throw;
      }
    }
  }

  protected virtual void Dispose(bool disposing)
  {
    if (!disposing || !this._isInitialized)
      return;
    this._isInitialized = false;
    if (this._initializeException != null)
      return;
    this.CloseTarget();
  }

  protected virtual void InitializeTarget() => this.FindAllLayouts();

  private void FindAllLayouts()
  {
    this._allLayouts = ObjectGraphScanner.FindReachableObjects<Layout>(false, (object) this);
    InternalLogger.Trace<Target, int>("{0} has {1} layouts", this, this._allLayouts.Count);
    this._allLayoutsAreThreadAgnostic = this._allLayouts.All<Layout>((Func<Layout, bool>) (layout => layout.ThreadAgnostic));
    this._oneLayoutIsMutableUnsafe = this._allLayoutsAreThreadAgnostic && this._allLayouts.Any<Layout>((Func<Layout, bool>) (layout => layout.MutableUnsafe));
    if (!this._allLayoutsAreThreadAgnostic || this._oneLayoutIsMutableUnsafe)
      this._allLayoutsAreThreadSafe = this._allLayouts.All<Layout>((Func<Layout, bool>) (layout => layout.ThreadSafe));
    this.StackTraceUsage = this._allLayouts.DefaultIfEmpty<Layout>().Max<Layout, StackTraceUsage>((Func<Layout, StackTraceUsage>) (layout => layout == null ? StackTraceUsage.None : layout.StackTraceUsage));
    if (this is IUsesStackTrace usesStackTrace && usesStackTrace.StackTraceUsage > this.StackTraceUsage)
      this.StackTraceUsage = usesStackTrace.StackTraceUsage;
    this._scannedForLayouts = true;
  }

  protected virtual void CloseTarget()
  {
  }

  protected virtual void FlushAsync(AsyncContinuation asyncContinuation)
  {
    asyncContinuation((Exception) null);
  }

  protected virtual void Write(LogEventInfo logEvent)
  {
  }

  protected virtual void Write(AsyncLogEventInfo logEvent)
  {
    try
    {
      this.Write(logEvent.LogEvent);
      logEvent.Continuation((Exception) null);
    }
    catch (Exception ex)
    {
      if (this.ExceptionMustBeRethrown(ex))
        throw;
      logEvent.Continuation(ex);
    }
  }

  protected virtual void WriteAsyncThreadSafe(AsyncLogEventInfo logEvent)
  {
    lock (this.SyncRoot)
    {
      if (!this.IsInitialized)
        logEvent.Continuation((Exception) null);
      else
        this.Write(logEvent);
    }
  }

  [Obsolete("Instead override Write(IList<AsyncLogEventInfo> logEvents. Marked obsolete on NLog 4.5")]
  protected virtual void Write(AsyncLogEventInfo[] logEvents)
  {
    this.Write((IList<AsyncLogEventInfo>) logEvents);
  }

  protected virtual void Write(IList<AsyncLogEventInfo> logEvents)
  {
    for (int index = 0; index < logEvents.Count; ++index)
      this.Write(logEvents[index]);
  }

  [Obsolete("Instead override WriteAsyncThreadSafe(IList<AsyncLogEventInfo> logEvents. Marked obsolete on NLog 4.5")]
  protected virtual void WriteAsyncThreadSafe(AsyncLogEventInfo[] logEvents)
  {
    this.WriteAsyncThreadSafe((IList<AsyncLogEventInfo>) logEvents);
  }

  protected virtual void WriteAsyncThreadSafe(IList<AsyncLogEventInfo> logEvents)
  {
    lock (this.SyncRoot)
    {
      if (!this.IsInitialized)
      {
        for (int index = 0; index < logEvents.Count; ++index)
          logEvents[index].Continuation((Exception) null);
      }
      else if (!this.OptimizeBufferReuse && logEvents is AsyncLogEventInfo[] logEvents1)
        this.Write(logEvents1);
      else
        this.Write(logEvents);
    }
  }

  private Exception CreateInitException()
  {
    return (Exception) new NLogRuntimeException($"Target {this} failed to initialize.", this._initializeException);
  }

  [Obsolete("Logger.Trace(logEvent) now automatically captures the logEvent Properties. Marked obsolete on NLog 4.6")]
  protected void MergeEventProperties(LogEventInfo logEvent)
  {
    if (logEvent.Parameters == null || logEvent.Parameters.Length == 0)
      return;
    for (int index = 0; index < logEvent.Parameters.Length; ++index)
    {
      if (logEvent.Parameters[index] is LogEventInfo parameter && parameter.HasProperties)
      {
        foreach (object key in (IEnumerable<object>) parameter.Properties.Keys)
          logEvent.Properties.Add(key, parameter.Properties[key]);
        parameter.Properties.Clear();
      }
    }
  }

  protected string RenderLogEvent(Layout layout, LogEventInfo logEvent)
  {
    if (layout == null || logEvent == null)
      return (string) null;
    if (!this.OptimizeBufferReuse)
      return layout.Render(logEvent);
    if (layout is SimpleLayout simpleLayout && simpleLayout.IsFixedText)
      return simpleLayout.Render(logEvent);
    string str;
    if (Target.TryGetCachedValue(layout, logEvent, out str))
      return str;
    if (simpleLayout != null && simpleLayout.IsSimpleStringText)
      return simpleLayout.Render(logEvent);
    using (ReusableObjectCreator<StringBuilder>.LockOject lockOject = this.ReusableLayoutBuilder.Allocate())
      return layout.RenderAllocateBuilder(logEvent, lockOject.Result);
  }

  private static bool TryGetCachedValue(Layout layout, LogEventInfo logEvent, out string value)
  {
    object obj;
    if ((!layout.ThreadAgnostic || layout.MutableUnsafe) && logEvent.TryGetCachedLayoutValue(layout, out obj))
    {
      value = obj?.ToString() ?? string.Empty;
      return true;
    }
    value = (string) null;
    return false;
  }

  public static void Register<T>(string name) where T : Target
  {
    Type targetType = typeof (T);
    Target.Register(name, targetType);
  }

  public static void Register(string name, Type targetType)
  {
    ConfigurationItemFactory.Default.Targets.RegisterDefinition(name, targetType);
  }

  internal bool ExceptionMustBeRethrown(Exception exception)
  {
    return exception.MustBeRethrown((IInternalLoggerContext) this);
  }
}
