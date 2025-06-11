// Decompiled with JetBrains decompiler
// Type: NLog.Filters.WhenRepeatedFilter
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

#nullable disable
namespace NLog.Filters;

[Filter("whenRepeated")]
public class WhenRepeatedFilter : LayoutBasedFilter
{
  private const int MaxInitialRenderBufferLength = 16384 /*0x4000*/;
  internal readonly ReusableBuilderCreator ReusableLayoutBuilder = new ReusableBuilderCreator();
  private readonly Dictionary<WhenRepeatedFilter.FilterInfoKey, WhenRepeatedFilter.FilterInfo> _repeatFilter = new Dictionary<WhenRepeatedFilter.FilterInfoKey, WhenRepeatedFilter.FilterInfo>(1000);
  private readonly Stack<KeyValuePair<WhenRepeatedFilter.FilterInfoKey, WhenRepeatedFilter.FilterInfo>> _objectPool = new Stack<KeyValuePair<WhenRepeatedFilter.FilterInfoKey, WhenRepeatedFilter.FilterInfo>>(1000);

  [DefaultValue(10)]
  public int TimeoutSeconds { get; set; }

  [DefaultValue(1000)]
  public int MaxLength { get; set; }

  [DefaultValue(false)]
  public bool IncludeFirst { get; set; }

  [DefaultValue(50000)]
  public int MaxFilterCacheSize { get; set; }

  [DefaultValue(1000)]
  public int DefaultFilterCacheSize { get; set; }

  [DefaultValue(null)]
  public string FilterCountPropertyName { get; set; }

  [DefaultValue(null)]
  public string FilterCountMessageAppendFormat { get; set; }

  [DefaultValue(true)]
  public bool OptimizeBufferReuse { get; set; }

  [DefaultValue(1000)]
  public int OptimizeBufferDefaultLength { get; set; }

  public WhenRepeatedFilter()
  {
    this.TimeoutSeconds = 10;
    this.MaxLength = 1000;
    this.DefaultFilterCacheSize = 1000;
    this.MaxFilterCacheSize = 50000;
    this.OptimizeBufferReuse = true;
    this.OptimizeBufferDefaultLength = this.MaxLength;
  }

  protected override FilterResult Check(LogEventInfo logEvent)
  {
    FilterResult filterResult = FilterResult.Neutral;
    bool flag = false;
    lock (this._repeatFilter)
    {
      using (ReusableObjectCreator<StringBuilder>.LockOject lockOject = this.OptimizeBufferReuse ? this.ReusableLayoutBuilder.Allocate() : this.ReusableLayoutBuilder.None)
      {
        if (this.OptimizeBufferReuse && lockOject.Result.Capacity != this.OptimizeBufferDefaultLength)
        {
          if (this.OptimizeBufferDefaultLength < 16384 /*0x4000*/)
          {
            this.OptimizeBufferDefaultLength = this.MaxLength;
            while (this.OptimizeBufferDefaultLength < lockOject.Result.Capacity && this.OptimizeBufferDefaultLength < 16384 /*0x4000*/)
              this.OptimizeBufferDefaultLength *= 2;
          }
          lockOject.Result.Capacity = this.OptimizeBufferDefaultLength;
        }
        WhenRepeatedFilter.FilterInfoKey key = this.RenderFilterInfoKey(logEvent, this.OptimizeBufferReuse ? lockOject.Result : (StringBuilder) null);
        WhenRepeatedFilter.FilterInfo filterInfo1;
        if (!this._repeatFilter.TryGetValue(key, out filterInfo1))
        {
          WhenRepeatedFilter.FilterInfo filterInfo2 = this.CreateFilterInfo(logEvent);
          if (this.OptimizeBufferReuse && filterInfo2.StringBuffer != null)
          {
            filterInfo2.StringBuffer.ClearBuilder();
            int num = Math.Min(lockOject.Result.Length, this.MaxLength);
            for (int index = 0; index < num; ++index)
              filterInfo2.StringBuffer.Append(lockOject.Result[index]);
          }
          filterInfo2.Refresh(logEvent.Level, logEvent.TimeStamp, 0);
          this._repeatFilter.Add(new WhenRepeatedFilter.FilterInfoKey(filterInfo2.StringBuffer, key.StringValue, new int?(key.StringHashCode)), filterInfo2);
          flag = true;
        }
        else
        {
          if (this.IncludeFirst)
            flag = filterInfo1.IsObsolete(logEvent.TimeStamp, this.TimeoutSeconds);
          filterResult = this.RefreshFilterInfo(logEvent, filterInfo1);
        }
      }
    }
    if (this.IncludeFirst & flag)
      filterResult = this.Action;
    return filterResult;
  }

  private WhenRepeatedFilter.FilterInfo CreateFilterInfo(LogEventInfo logEvent)
  {
    if (this._objectPool.Count == 0 && this._repeatFilter.Count > this.DefaultFilterCacheSize)
    {
      int val2 = this._repeatFilter.Count > this.MaxFilterCacheSize ? this.TimeoutSeconds * 2 / 3 : this.TimeoutSeconds;
      this.PruneFilterCache(logEvent, Math.Max(1, val2));
      if (this._repeatFilter.Count > this.MaxFilterCacheSize)
        this.PruneFilterCache(logEvent, Math.Max(1, this.TimeoutSeconds / 2));
    }
    WhenRepeatedFilter.FilterInfo filterInfo;
    if (this._objectPool.Count == 0)
    {
      filterInfo = new WhenRepeatedFilter.FilterInfo(this.OptimizeBufferReuse ? new StringBuilder(this.OptimizeBufferDefaultLength) : (StringBuilder) null);
    }
    else
    {
      filterInfo = this._objectPool.Pop().Value;
      if (this.OptimizeBufferReuse && filterInfo.StringBuffer != null && filterInfo.StringBuffer.Capacity != this.OptimizeBufferDefaultLength)
        filterInfo.StringBuffer.Capacity = this.OptimizeBufferDefaultLength;
    }
    return filterInfo;
  }

  private void PruneFilterCache(LogEventInfo logEvent, int aggressiveTimeoutSeconds)
  {
    foreach (KeyValuePair<WhenRepeatedFilter.FilterInfoKey, WhenRepeatedFilter.FilterInfo> keyValuePair in this._repeatFilter)
    {
      if (keyValuePair.Value.IsObsolete(logEvent.TimeStamp, aggressiveTimeoutSeconds))
        this._objectPool.Push(keyValuePair);
    }
    foreach (KeyValuePair<WhenRepeatedFilter.FilterInfoKey, WhenRepeatedFilter.FilterInfo> keyValuePair in this._objectPool)
      this._repeatFilter.Remove(keyValuePair.Key);
    if (this._repeatFilter.Count * 2 > this.DefaultFilterCacheSize && this.DefaultFilterCacheSize < this.MaxFilterCacheSize)
      this.DefaultFilterCacheSize *= 2;
    while (this._objectPool.Count != 0 && this._objectPool.Count > this.DefaultFilterCacheSize)
      this._objectPool.Pop();
  }

  private WhenRepeatedFilter.FilterInfoKey RenderFilterInfoKey(
    LogEventInfo logEvent,
    StringBuilder targetBuilder)
  {
    if (targetBuilder != null)
    {
      this.Layout.RenderAppendBuilder(logEvent, targetBuilder);
      if (targetBuilder.Length > this.MaxLength)
        targetBuilder.Length = this.MaxLength;
      return new WhenRepeatedFilter.FilterInfoKey(targetBuilder, (string) null);
    }
    string stringValue = this.Layout.Render(logEvent) ?? string.Empty;
    if (stringValue.Length > this.MaxLength)
      stringValue = stringValue.Substring(0, this.MaxLength);
    return new WhenRepeatedFilter.FilterInfoKey((StringBuilder) null, stringValue);
  }

  private FilterResult RefreshFilterInfo(
    LogEventInfo logEvent,
    WhenRepeatedFilter.FilterInfo filterInfo)
  {
    if (filterInfo.HasExpired(logEvent.TimeStamp, this.TimeoutSeconds) || logEvent.Level.Ordinal > filterInfo.LogLevel.Ordinal)
    {
      int val2 = filterInfo.FilterCount;
      if (val2 > 0 && filterInfo.IsObsolete(logEvent.TimeStamp, this.TimeoutSeconds))
        val2 = 0;
      filterInfo.Refresh(logEvent.Level, logEvent.TimeStamp, 0);
      if (val2 > 0)
      {
        if (!string.IsNullOrEmpty(this.FilterCountPropertyName))
        {
          object obj;
          if (!logEvent.Properties.TryGetValue((object) this.FilterCountPropertyName, out obj))
            logEvent.Properties[(object) this.FilterCountPropertyName] = (object) val2;
          else if (obj is int val1)
          {
            val2 = Math.Max(val1, val2);
            logEvent.Properties[(object) this.FilterCountPropertyName] = (object) val2;
          }
        }
        if (!string.IsNullOrEmpty(this.FilterCountMessageAppendFormat) && logEvent.Message != null)
          logEvent.Message += string.Format(this.FilterCountMessageAppendFormat, (object) val2.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      }
      return FilterResult.Neutral;
    }
    filterInfo.Refresh(logEvent.Level, logEvent.TimeStamp, filterInfo.FilterCount + 1);
    return this.Action;
  }

  private class FilterInfo
  {
    public FilterInfo(StringBuilder stringBuilder) => this.StringBuffer = stringBuilder;

    public void Refresh(NLog.LogLevel logLevel, DateTime logTimeStamp, int filterCount)
    {
      if (filterCount == 0)
      {
        this.LastLogTime = logTimeStamp;
        this.LogLevel = logLevel;
      }
      else if (this.LogLevel == (NLog.LogLevel) null || logLevel.Ordinal > this.LogLevel.Ordinal)
        this.LogLevel = logLevel;
      this.LastFilterTime = logTimeStamp;
      this.FilterCount = filterCount;
    }

    public bool IsObsolete(DateTime logEventTime, int timeoutSeconds)
    {
      if (this.FilterCount == 0)
        return this.HasExpired(logEventTime, timeoutSeconds);
      return (logEventTime - this.LastFilterTime).TotalSeconds > (double) timeoutSeconds && this.HasExpired(logEventTime, timeoutSeconds * 2);
    }

    public bool HasExpired(DateTime logEventTime, int timeoutSeconds)
    {
      return (logEventTime - this.LastLogTime).TotalSeconds > (double) timeoutSeconds;
    }

    public StringBuilder StringBuffer { get; private set; }

    public NLog.LogLevel LogLevel { get; private set; }

    private DateTime LastLogTime { get; set; }

    private DateTime LastFilterTime { get; set; }

    public int FilterCount { get; private set; }
  }

  private struct FilterInfoKey : IEquatable<WhenRepeatedFilter.FilterInfoKey>
  {
    private readonly StringBuilder _stringBuffer;
    public readonly string StringValue;
    public readonly int StringHashCode;

    public FilterInfoKey(StringBuilder stringBuffer, string stringValue, int? stringHashCode = null)
    {
      this._stringBuffer = stringBuffer;
      this.StringValue = stringValue;
      if (stringHashCode.HasValue)
        this.StringHashCode = stringHashCode.Value;
      else if (stringBuffer != null)
      {
        int hashCode = stringBuffer.Length.GetHashCode();
        int num = Math.Min(stringBuffer.Length, 100);
        for (int index = 0; index < num; ++index)
          hashCode ^= stringBuffer[index].GetHashCode();
        this.StringHashCode = hashCode;
      }
      else
        this.StringHashCode = StringComparer.Ordinal.GetHashCode(this.StringValue);
    }

    public override int GetHashCode() => this.StringHashCode;

    public bool Equals(WhenRepeatedFilter.FilterInfoKey other)
    {
      if (this.StringValue != null)
        return string.Equals(this.StringValue, other.StringValue, StringComparison.Ordinal);
      return this._stringBuffer != null && other._stringBuffer != null ? (this._stringBuffer.Capacity != other._stringBuffer.Capacity ? this._stringBuffer.EqualTo(other._stringBuffer) : this._stringBuffer.Equals(other._stringBuffer)) : this._stringBuffer == other._stringBuffer && (object) this.StringValue == (object) other.StringValue;
    }

    public override bool Equals(object obj)
    {
      return obj is WhenRepeatedFilter.FilterInfoKey other && this.Equals(other);
    }
  }
}
