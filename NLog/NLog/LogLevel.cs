// Decompiled with JetBrains decompiler
// Type: NLog.LogLevel
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace NLog;

[TypeConverter(typeof (LogLevelTypeConverter))]
public sealed class LogLevel : IComparable, IEquatable<LogLevel>, IConvertible
{
  public static readonly LogLevel Trace = new LogLevel(nameof (Trace), 0);
  public static readonly LogLevel Debug = new LogLevel(nameof (Debug), 1);
  public static readonly LogLevel Info = new LogLevel(nameof (Info), 2);
  public static readonly LogLevel Warn = new LogLevel(nameof (Warn), 3);
  public static readonly LogLevel Error = new LogLevel(nameof (Error), 4);
  public static readonly LogLevel Fatal = new LogLevel(nameof (Fatal), 5);
  public static readonly LogLevel Off = new LogLevel(nameof (Off), 6);
  private static readonly IList<LogLevel> allLevels = (IList<LogLevel>) new List<LogLevel>()
  {
    LogLevel.Trace,
    LogLevel.Debug,
    LogLevel.Info,
    LogLevel.Warn,
    LogLevel.Error,
    LogLevel.Fatal,
    LogLevel.Off
  }.AsReadOnly();
  private static readonly IList<LogLevel> allLoggingLevels = (IList<LogLevel>) new List<LogLevel>()
  {
    LogLevel.Trace,
    LogLevel.Debug,
    LogLevel.Info,
    LogLevel.Warn,
    LogLevel.Error,
    LogLevel.Fatal
  }.AsReadOnly();
  private readonly int _ordinal;
  private readonly string _name;

  public static IEnumerable<LogLevel> AllLevels => (IEnumerable<LogLevel>) LogLevel.allLevels;

  public static IEnumerable<LogLevel> AllLoggingLevels
  {
    get => (IEnumerable<LogLevel>) LogLevel.allLoggingLevels;
  }

  private LogLevel(string name, int ordinal)
  {
    this._name = name;
    this._ordinal = ordinal;
  }

  public string Name => this._name;

  internal static LogLevel MaxLevel => LogLevel.Fatal;

  internal static LogLevel MinLevel => LogLevel.Trace;

  public int Ordinal => this._ordinal;

  public static bool operator ==(LogLevel level1, LogLevel level2)
  {
    if ((object) level1 == null)
      return (object) level2 == null;
    return (object) level2 != null && level1.Ordinal == level2.Ordinal;
  }

  public static bool operator !=(LogLevel level1, LogLevel level2)
  {
    if ((object) level1 == null)
      return level2 != null;
    return (object) level2 == null || level1.Ordinal != level2.Ordinal;
  }

  public static bool operator >(LogLevel level1, LogLevel level2)
  {
    if (level1 == (LogLevel) null)
      throw new ArgumentNullException(nameof (level1));
    if (level2 == (LogLevel) null)
      throw new ArgumentNullException(nameof (level2));
    return level1.Ordinal > level2.Ordinal;
  }

  public static bool operator >=(LogLevel level1, LogLevel level2)
  {
    if (level1 == (LogLevel) null)
      throw new ArgumentNullException(nameof (level1));
    if (level2 == (LogLevel) null)
      throw new ArgumentNullException(nameof (level2));
    return level1.Ordinal >= level2.Ordinal;
  }

  public static bool operator <(LogLevel level1, LogLevel level2)
  {
    if (level1 == (LogLevel) null)
      throw new ArgumentNullException(nameof (level1));
    if (level2 == (LogLevel) null)
      throw new ArgumentNullException(nameof (level2));
    return level1.Ordinal < level2.Ordinal;
  }

  public static bool operator <=(LogLevel level1, LogLevel level2)
  {
    if (level1 == (LogLevel) null)
      throw new ArgumentNullException(nameof (level1));
    if (level2 == (LogLevel) null)
      throw new ArgumentNullException(nameof (level2));
    return level1.Ordinal <= level2.Ordinal;
  }

  public static LogLevel FromOrdinal(int ordinal)
  {
    switch (ordinal)
    {
      case 0:
        return LogLevel.Trace;
      case 1:
        return LogLevel.Debug;
      case 2:
        return LogLevel.Info;
      case 3:
        return LogLevel.Warn;
      case 4:
        return LogLevel.Error;
      case 5:
        return LogLevel.Fatal;
      case 6:
        return LogLevel.Off;
      default:
        throw new ArgumentException("Invalid ordinal.");
    }
  }

  public static LogLevel FromString(string levelName)
  {
    if (levelName == null)
      throw new ArgumentNullException(nameof (levelName));
    if (levelName.Equals("Trace", StringComparison.OrdinalIgnoreCase))
      return LogLevel.Trace;
    if (levelName.Equals("Debug", StringComparison.OrdinalIgnoreCase))
      return LogLevel.Debug;
    if (levelName.Equals("Info", StringComparison.OrdinalIgnoreCase))
      return LogLevel.Info;
    if (levelName.Equals("Warn", StringComparison.OrdinalIgnoreCase))
      return LogLevel.Warn;
    if (levelName.Equals("Error", StringComparison.OrdinalIgnoreCase))
      return LogLevel.Error;
    if (levelName.Equals("Fatal", StringComparison.OrdinalIgnoreCase))
      return LogLevel.Fatal;
    if (levelName.Equals("Off", StringComparison.OrdinalIgnoreCase) || levelName.Equals("None", StringComparison.OrdinalIgnoreCase))
      return LogLevel.Off;
    if (levelName.Equals("Information", StringComparison.OrdinalIgnoreCase))
      return LogLevel.Info;
    if (levelName.Equals("Warning", StringComparison.OrdinalIgnoreCase))
      return LogLevel.Warn;
    throw new ArgumentException("Unknown log level: " + levelName);
  }

  public override string ToString() => this.Name;

  public override int GetHashCode() => this.Ordinal;

  public override bool Equals(object obj)
  {
    LogLevel logLevel = obj as LogLevel;
    return (object) logLevel != null && this.Ordinal == logLevel.Ordinal;
  }

  public bool Equals(LogLevel other) => other != (LogLevel) null && this.Ordinal == other.Ordinal;

  public int CompareTo(object obj)
  {
    if (obj == null)
      throw new ArgumentNullException(nameof (obj));
    return this.Ordinal - ((LogLevel) obj).Ordinal;
  }

  TypeCode IConvertible.GetTypeCode() => TypeCode.Object;

  byte IConvertible.ToByte(IFormatProvider provider) => Convert.ToByte(this._ordinal);

  bool IConvertible.ToBoolean(IFormatProvider provider) => throw new InvalidCastException();

  char IConvertible.ToChar(IFormatProvider provider) => Convert.ToChar(this._ordinal);

  DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new InvalidCastException();

  Decimal IConvertible.ToDecimal(IFormatProvider provider) => Convert.ToDecimal(this._ordinal);

  double IConvertible.ToDouble(IFormatProvider provider) => (double) this._ordinal;

  short IConvertible.ToInt16(IFormatProvider provider) => Convert.ToInt16(this._ordinal);

  int IConvertible.ToInt32(IFormatProvider provider) => Convert.ToInt32(this._ordinal);

  long IConvertible.ToInt64(IFormatProvider provider) => Convert.ToInt64(this._ordinal);

  sbyte IConvertible.ToSByte(IFormatProvider provider) => Convert.ToSByte(this._ordinal);

  float IConvertible.ToSingle(IFormatProvider provider) => Convert.ToSingle(this._ordinal);

  string IConvertible.ToString(IFormatProvider provider) => this._name;

  object IConvertible.ToType(Type conversionType, IFormatProvider provider)
  {
    return conversionType == typeof (string) ? (object) this.Name : Convert.ChangeType((object) this._ordinal, conversionType, provider);
  }

  ushort IConvertible.ToUInt16(IFormatProvider provider) => Convert.ToUInt16(this._ordinal);

  uint IConvertible.ToUInt32(IFormatProvider provider) => Convert.ToUInt32(this._ordinal);

  ulong IConvertible.ToUInt64(IFormatProvider provider) => Convert.ToUInt64(this._ordinal);
}
