// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.ExceptionLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using NLog.MessageTemplates;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("exception")]
[ThreadAgnostic]
[ThreadSafe]
public class ExceptionLayoutRenderer : LayoutRenderer, IRawValue
{
  private string _format;
  private string _innerFormat = string.Empty;
  private readonly Dictionary<ExceptionRenderingFormat, Action<StringBuilder, Exception, Exception>> _renderingfunctions;
  private static readonly Dictionary<string, ExceptionRenderingFormat> _formatsMapping = new Dictionary<string, ExceptionRenderingFormat>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
  {
    {
      "MESSAGE",
      ExceptionRenderingFormat.Message
    },
    {
      "TYPE",
      ExceptionRenderingFormat.Type
    },
    {
      "SHORTTYPE",
      ExceptionRenderingFormat.ShortType
    },
    {
      "TOSTRING",
      ExceptionRenderingFormat.ToString
    },
    {
      "METHOD",
      ExceptionRenderingFormat.Method
    },
    {
      "TARGETSITE",
      ExceptionRenderingFormat.Method
    },
    {
      "SOURCE",
      ExceptionRenderingFormat.Source
    },
    {
      "STACKTRACE",
      ExceptionRenderingFormat.StackTrace
    },
    {
      "DATA",
      ExceptionRenderingFormat.Data
    },
    {
      "@",
      ExceptionRenderingFormat.Serialize
    },
    {
      "HRESULT",
      ExceptionRenderingFormat.HResult
    },
    {
      "PROPERTIES",
      ExceptionRenderingFormat.Properties
    }
  };
  private static readonly HashSet<string> ExcludeDefaultProperties = new HashSet<string>((IEnumerable<string>) new string[9]
  {
    "Type",
    "Data",
    "HelpLink",
    "HResult",
    "InnerException",
    "Message",
    "Source",
    "StackTrace",
    "TargetSite"
  }, (IEqualityComparer<string>) StringComparer.Ordinal);
  private ObjectReflectionCache _objectReflectionCache;

  private ObjectReflectionCache ObjectReflectionCache
  {
    get
    {
      return this._objectReflectionCache ?? (this._objectReflectionCache = new ObjectReflectionCache());
    }
  }

  public ExceptionLayoutRenderer()
  {
    this.Format = "message";
    this.Separator = " ";
    this.ExceptionDataSeparator = ";";
    this.InnerExceptionSeparator = EnvironmentHelper.NewLine;
    this.MaxInnerExceptionLevel = 0;
    this._renderingfunctions = new Dictionary<ExceptionRenderingFormat, Action<StringBuilder, Exception, Exception>>()
    {
      {
        ExceptionRenderingFormat.Message,
        (Action<StringBuilder, Exception, Exception>) ((sb, ex, aggex) => this.AppendMessage(sb, ex))
      },
      {
        ExceptionRenderingFormat.Type,
        (Action<StringBuilder, Exception, Exception>) ((sb, ex, aggex) => this.AppendType(sb, ex))
      },
      {
        ExceptionRenderingFormat.ShortType,
        (Action<StringBuilder, Exception, Exception>) ((sb, ex, aggex) => this.AppendShortType(sb, ex))
      },
      {
        ExceptionRenderingFormat.ToString,
        (Action<StringBuilder, Exception, Exception>) ((sb, ex, aggex) => this.AppendToString(sb, ex))
      },
      {
        ExceptionRenderingFormat.Method,
        (Action<StringBuilder, Exception, Exception>) ((sb, ex, aggex) => this.AppendMethod(sb, ex))
      },
      {
        ExceptionRenderingFormat.Source,
        (Action<StringBuilder, Exception, Exception>) ((sb, ex, aggex) => this.AppendSource(sb, ex))
      },
      {
        ExceptionRenderingFormat.StackTrace,
        (Action<StringBuilder, Exception, Exception>) ((sb, ex, aggex) => this.AppendStackTrace(sb, ex))
      },
      {
        ExceptionRenderingFormat.Data,
        (Action<StringBuilder, Exception, Exception>) ((sb, ex, aggex) => this.AppendData(sb, ex, aggex))
      },
      {
        ExceptionRenderingFormat.Serialize,
        (Action<StringBuilder, Exception, Exception>) ((sb, ex, aggex) => this.AppendSerializeObject(sb, ex))
      },
      {
        ExceptionRenderingFormat.HResult,
        (Action<StringBuilder, Exception, Exception>) ((sb, ex, aggex) => this.AppendHResult(sb, ex))
      },
      {
        ExceptionRenderingFormat.Properties,
        (Action<StringBuilder, Exception, Exception>) ((sb, ex, aggex) => this.AppendProperties(sb, ex))
      }
    };
  }

  [DefaultParameter]
  public string Format
  {
    get => this._format;
    set
    {
      this._format = value;
      this.Formats = ExceptionLayoutRenderer.CompileFormat(value, nameof (Format));
    }
  }

  public string InnerFormat
  {
    get => this._innerFormat;
    set
    {
      this._innerFormat = value;
      this.InnerFormats = ExceptionLayoutRenderer.CompileFormat(value, nameof (InnerFormat));
    }
  }

  [DefaultValue(" ")]
  public string Separator { get; set; }

  [DefaultValue(";")]
  public string ExceptionDataSeparator { get; set; }

  [DefaultValue(0)]
  public int MaxInnerExceptionLevel { get; set; }

  public string InnerExceptionSeparator { get; set; }

  [DefaultValue(false)]
  public bool BaseException { get; set; }

  [DefaultValue(true)]
  public bool FlattenException { get; set; } = true;

  public List<ExceptionRenderingFormat> Formats { get; private set; }

  public List<ExceptionRenderingFormat> InnerFormats { get; private set; }

  bool IRawValue.TryGetRawValue(LogEventInfo logEvent, out object value)
  {
    value = (object) this.GetTopException(logEvent);
    return true;
  }

  private Exception GetTopException(LogEventInfo logEvent)
  {
    if (!this.BaseException)
      return logEvent.Exception;
    return logEvent.Exception?.GetBaseException();
  }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    Exception topException = this.GetTopException(logEvent);
    if (topException == null)
      return;
    int currentLevel1 = 0;
    if (logEvent.Exception is AggregateException exception)
    {
      Exception currentException = this.FlattenException ? ExceptionLayoutRenderer.GetPrimaryException(exception) : (Exception) exception;
      this.AppendException(currentException, this.Formats, builder, (Exception) exception);
      if (currentLevel1 >= this.MaxInnerExceptionLevel)
        return;
      int currentLevel2 = this.AppendInnerExceptionTree(currentException, currentLevel1, builder);
      if (currentLevel2 >= this.MaxInnerExceptionLevel)
        return;
      ReadOnlyCollection<Exception> innerExceptions = exception.InnerExceptions;
      // ISSUE: explicit non-virtual call
      if ((innerExceptions != null ? (__nonvirtual (innerExceptions.Count) > 1 ? 1 : 0) : 0) == 0)
        return;
      this.AppendAggregateException(exception, currentLevel2, builder);
    }
    else
    {
      this.AppendException(topException, this.Formats, builder);
      if (currentLevel1 >= this.MaxInnerExceptionLevel)
        return;
      this.AppendInnerExceptionTree(topException, currentLevel1, builder);
    }
  }

  private static Exception GetPrimaryException(AggregateException aggregateException)
  {
    if (aggregateException.InnerExceptions.Count == 1)
    {
      Exception innerException = aggregateException.InnerExceptions[0];
      if (!(innerException is AggregateException))
        return innerException;
    }
    aggregateException = aggregateException.Flatten();
    return aggregateException.InnerExceptions.Count == 1 ? aggregateException.InnerExceptions[0] : (Exception) aggregateException;
  }

  private void AppendAggregateException(
    AggregateException primaryException,
    int currentLevel,
    StringBuilder builder)
  {
    AggregateException aggregateException = primaryException.Flatten();
    if (aggregateException.InnerExceptions == null)
      return;
    for (int index = 0; index < aggregateException.InnerExceptions.Count && currentLevel < this.MaxInnerExceptionLevel; ++currentLevel)
    {
      Exception innerException = aggregateException.InnerExceptions[index];
      if (innerException != primaryException.InnerException)
      {
        if (innerException == null)
        {
          InternalLogger.Debug("Skipping rendering exception as exception is null");
        }
        else
        {
          this.AppendInnerException(innerException, builder);
          ++currentLevel;
          currentLevel = this.AppendInnerExceptionTree(innerException, currentLevel, builder);
        }
      }
      ++index;
    }
  }

  private int AppendInnerExceptionTree(
    Exception currentException,
    int currentLevel,
    StringBuilder sb)
  {
    for (currentException = currentException.InnerException; currentException != null && currentLevel < this.MaxInnerExceptionLevel; currentException = currentException.InnerException)
    {
      this.AppendInnerException(currentException, sb);
      ++currentLevel;
    }
    return currentLevel;
  }

  private void AppendInnerException(Exception currentException, StringBuilder builder)
  {
    builder.Append(this.InnerExceptionSeparator);
    this.AppendException(currentException, this.InnerFormats ?? this.Formats, builder);
  }

  private void AppendException(
    Exception currentException,
    List<ExceptionRenderingFormat> renderFormats,
    StringBuilder builder,
    Exception aggregateException = null)
  {
    int length1 = builder.Length;
    foreach (ExceptionRenderingFormat renderFormat in renderFormats)
    {
      int length2 = builder.Length;
      this._renderingfunctions[renderFormat](builder, currentException, aggregateException);
      if (builder.Length != length2)
      {
        length1 = builder.Length;
        builder.Append(this.Separator);
      }
    }
    builder.Length = length1;
  }

  protected virtual void AppendMessage(StringBuilder sb, Exception ex)
  {
    try
    {
      sb.Append(ex.Message);
    }
    catch (Exception ex1)
    {
      string message = $"Exception in {typeof (ExceptionLayoutRenderer).FullName}.AppendMessage(): {ex1.GetType().FullName}.";
      sb.Append("NLog message: ");
      sb.Append(message);
      InternalLogger.Warn(ex1, message);
    }
  }

  protected virtual void AppendMethod(StringBuilder sb, Exception ex)
  {
    sb.Append(ex.TargetSite?.ToString());
  }

  protected virtual void AppendStackTrace(StringBuilder sb, Exception ex)
  {
    sb.Append(ex.StackTrace);
  }

  protected virtual void AppendToString(StringBuilder sb, Exception ex) => sb.Append(ex.ToString());

  protected virtual void AppendType(StringBuilder sb, Exception ex)
  {
    sb.Append(ex.GetType().FullName);
  }

  protected virtual void AppendShortType(StringBuilder sb, Exception ex)
  {
    sb.Append(ex.GetType().Name);
  }

  protected virtual void AppendSource(StringBuilder sb, Exception ex) => sb.Append(ex.Source);

  protected virtual void AppendHResult(StringBuilder sb, Exception ex)
  {
    if (ex.HResult == 0 || ex.HResult == 1)
      return;
    sb.AppendFormat("0x{0:X8}", (object) ex.HResult);
  }

  private void AppendData(StringBuilder builder, Exception ex, Exception aggregateException)
  {
    if (aggregateException != null)
    {
      int? count = aggregateException.Data?.Count;
      int num = 0;
      if (count.GetValueOrDefault() > num & count.HasValue && ex != aggregateException)
      {
        this.AppendData(builder, aggregateException);
        builder.Append(this.Separator);
      }
    }
    this.AppendData(builder, ex);
  }

  protected virtual void AppendData(StringBuilder sb, Exception ex)
  {
    IDictionary data = ex.Data;
    if ((data != null ? (data.Count > 0 ? 1 : 0) : 0) == 0)
      return;
    string str = string.Empty;
    foreach (object key in (IEnumerable) ex.Data.Keys)
    {
      sb.Append(str);
      sb.AppendFormat("{0}: {1}", key, ex.Data[key]);
      str = this.ExceptionDataSeparator;
    }
  }

  protected virtual void AppendSerializeObject(StringBuilder sb, Exception ex)
  {
    ConfigurationItemFactory.Default.ValueFormatter.FormatValue((object) ex, (string) null, CaptureType.Serialize, (IFormatProvider) null, sb);
  }

  protected virtual void AppendProperties(StringBuilder sb, Exception ex)
  {
    string str1 = string.Empty;
    foreach (ObjectReflectionCache.ObjectPropertyList.PropertyValue lookupObjectProperty in this.ObjectReflectionCache.LookupObjectProperties((object) ex))
    {
      if (!ExceptionLayoutRenderer.ExcludeDefaultProperties.Contains(lookupObjectProperty.Name))
      {
        string str2 = lookupObjectProperty.Value?.ToString();
        if (!string.IsNullOrEmpty(str2))
        {
          sb.Append(str1);
          sb.AppendFormat("{0}: {1}", (object) lookupObjectProperty.Name, (object) str2);
          str1 = this.ExceptionDataSeparator;
        }
      }
    }
  }

  private static List<ExceptionRenderingFormat> CompileFormat(
    string formatSpecifier,
    string propertyName)
  {
    List<ExceptionRenderingFormat> exceptionRenderingFormatList = new List<ExceptionRenderingFormat>();
    foreach (string splitAndTrimToken in formatSpecifier.SplitAndTrimTokens(','))
    {
      ExceptionRenderingFormat exceptionRenderingFormat;
      if (ExceptionLayoutRenderer._formatsMapping.TryGetValue(splitAndTrimToken, out exceptionRenderingFormat))
        exceptionRenderingFormatList.Add(exceptionRenderingFormat);
      else
        InternalLogger.Warn<string, string>("Exception-LayoutRenderer assigned unknown {0}: {1}", propertyName, splitAndTrimToken);
    }
    return exceptionRenderingFormatList;
  }
}
