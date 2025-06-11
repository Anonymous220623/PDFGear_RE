// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.PerformanceCounterLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using NLog.Layouts;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("performancecounter")]
[ThreadSafe]
public class PerformanceCounterLayoutRenderer : LayoutRenderer
{
  private PerformanceCounterLayoutRenderer.PerformanceCounterCached _fixedPerformanceCounter;
  private PerformanceCounterLayoutRenderer.PerformanceCounterCached _performanceCounter;
  private SimpleLayout _instance;
  private SimpleLayout _machineName;

  [RequiredParameter]
  public string Category { get; set; }

  [RequiredParameter]
  public string Counter { get; set; }

  public string Instance
  {
    get => this._instance?.Text;
    set
    {
      this._instance = value != null ? new SimpleLayout(value) : (SimpleLayout) null;
      this.ResetPerformanceCounters();
    }
  }

  public string MachineName
  {
    get => this._machineName?.Text;
    set
    {
      this._machineName = value != null ? new SimpleLayout(value) : (SimpleLayout) null;
      this.ResetPerformanceCounters();
    }
  }

  public string Format { get; set; }

  public CultureInfo Culture { get; set; }

  protected override void InitializeLayoutRenderer()
  {
    base.InitializeLayoutRenderer();
    if (this._instance == null && string.Equals(this.Category, "Process", StringComparison.OrdinalIgnoreCase))
      this._instance = (SimpleLayout) (PerformanceCounterLayoutRenderer.GetCurrentProcessInstanceName(this.Category) ?? string.Empty);
    this.LookupPerformanceCounter(LogEventInfo.CreateNullEvent());
  }

  protected override void CloseLayoutRenderer()
  {
    base.CloseLayoutRenderer();
    this._fixedPerformanceCounter?.Close();
    this._performanceCounter?.Close();
    this.ResetPerformanceCounters();
  }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    PerformanceCounterLayoutRenderer.PerformanceCounterCached performanceCounterCached = this.LookupPerformanceCounter(logEvent);
    IFormatProvider formatProvider = this.GetFormatProvider(logEvent, (IFormatProvider) this.Culture);
    builder.Append(performanceCounterCached.GetValue().ToString(this.Format, formatProvider));
  }

  private void ResetPerformanceCounters()
  {
    this._fixedPerformanceCounter = (PerformanceCounterLayoutRenderer.PerformanceCounterCached) null;
    this._performanceCounter = (PerformanceCounterLayoutRenderer.PerformanceCounterCached) null;
  }

  private PerformanceCounterLayoutRenderer.PerformanceCounterCached LookupPerformanceCounter(
    LogEventInfo logEventInfo)
  {
    PerformanceCounterLayoutRenderer.PerformanceCounterCached performanceCounter1 = this._fixedPerformanceCounter;
    if (performanceCounter1 != null)
      return performanceCounter1;
    PerformanceCounterLayoutRenderer.PerformanceCounterCached performanceCounter2 = this._performanceCounter;
    string machineName = this._machineName?.Render(logEventInfo) ?? string.Empty;
    string instanceName = this._instance?.Render(logEventInfo) ?? string.Empty;
    if (performanceCounter2 != null && performanceCounter2.MachineName == machineName && performanceCounter2.InstanceName == instanceName)
      return performanceCounter2;
    PerformanceCounter performanceCounter3 = this.CreatePerformanceCounter(machineName, instanceName);
    PerformanceCounterLayoutRenderer.PerformanceCounterCached performanceCounterCached = new PerformanceCounterLayoutRenderer.PerformanceCounterCached(machineName, instanceName, performanceCounter3);
    if ((this._machineName?.Text == null || this._machineName.IsFixedText) && (this._instance?.Text == null || this._instance.IsFixedText))
      this._fixedPerformanceCounter = performanceCounterCached;
    else
      this._performanceCounter = performanceCounterCached;
    return performanceCounterCached;
  }

  private PerformanceCounter CreatePerformanceCounter(string machineName, string instanceName)
  {
    return !string.IsNullOrEmpty(machineName) ? new PerformanceCounter(this.Category, this.Counter, instanceName, machineName) : new PerformanceCounter(this.Category, this.Counter, instanceName, true);
  }

  private static string GetCurrentProcessInstanceName(string category)
  {
    try
    {
      using (Process currentProcess = Process.GetCurrentProcess())
      {
        int id = currentProcess.Id;
        foreach (string instanceName in new PerformanceCounterCategory(category).GetInstanceNames())
        {
          using (PerformanceCounter performanceCounter = new PerformanceCounter(category, "ID Process", instanceName, true))
          {
            if ((int) performanceCounter.RawValue == id)
              return instanceName;
          }
        }
        InternalLogger.Debug<int>("PerformanceCounter - Failed to auto detect current process instance. ProcessId={0}", id);
      }
    }
    catch (Exception ex)
    {
      if (ex.MustBeRethrown())
        throw;
      InternalLogger.Warn(ex, "PerformanceCounter - Failed to auto detect current process instance.");
    }
    return string.Empty;
  }

  private class PerformanceCounterCached
  {
    private readonly PerformanceCounter _perfCounter;
    private readonly object _lockObject = new object();
    private CounterSample _prevSample = CounterSample.Empty;
    private CounterSample _nextSample = CounterSample.Empty;

    public PerformanceCounterCached(
      string machineName,
      string instanceName,
      PerformanceCounter performanceCounter)
    {
      this.MachineName = machineName;
      this.InstanceName = instanceName;
      this._perfCounter = performanceCounter;
      double num = (double) this.GetValue();
    }

    public string MachineName { get; }

    public string InstanceName { get; }

    public float GetValue()
    {
      lock (this._lockObject)
      {
        CounterSample nextCounterSample = this._perfCounter.NextSample();
        if (nextCounterSample.SystemFrequency != 0L)
        {
          float num = (float) (nextCounterSample.TimeStamp - this._nextSample.TimeStamp) / (float) nextCounterSample.SystemFrequency;
          if ((double) num > 0.5 || (double) num < -0.5)
          {
            this._prevSample = this._nextSample;
            this._nextSample = nextCounterSample;
            if (this._prevSample.Equals(CounterSample.Empty))
              this._prevSample = nextCounterSample;
          }
        }
        else
        {
          this._prevSample = this._nextSample;
          this._nextSample = nextCounterSample;
        }
        return CounterSample.Calculate(this._prevSample, nextCounterSample);
      }
    }

    public void Close() => this._perfCounter.Close();
  }
}
