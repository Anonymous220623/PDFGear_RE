// Decompiled with JetBrains decompiler
// Type: NLog.ILogger
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.ComponentModel;

#nullable disable
namespace NLog;

[CLSCompliant(false)]
public interface ILogger : ILoggerBase, ISuppress
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  void Trace(object value);

  [EditorBrowsable(EditorBrowsableState.Never)]
  void Trace(IFormatProvider formatProvider, object value);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Trace(string message, object arg1, object arg2);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Trace(string message, object arg1, object arg2, object arg3);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Trace(IFormatProvider formatProvider, string message, bool argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Trace(string message, bool argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Trace(IFormatProvider formatProvider, string message, char argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Trace(string message, char argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Trace(IFormatProvider formatProvider, string message, byte argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Trace(string message, byte argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Trace(IFormatProvider formatProvider, string message, string argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Trace(string message, string argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Trace(IFormatProvider formatProvider, string message, int argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Trace(string message, int argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Trace(IFormatProvider formatProvider, string message, long argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Trace(string message, long argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Trace(IFormatProvider formatProvider, string message, float argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Trace(string message, float argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Trace(IFormatProvider formatProvider, string message, double argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Trace(string message, double argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Trace(IFormatProvider formatProvider, string message, Decimal argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Trace(string message, Decimal argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Trace(IFormatProvider formatProvider, string message, object argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Trace(string message, object argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Trace(IFormatProvider formatProvider, string message, sbyte argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Trace(string message, sbyte argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Trace(IFormatProvider formatProvider, string message, uint argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Trace(string message, uint argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Trace(IFormatProvider formatProvider, string message, ulong argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Trace(string message, ulong argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  void Debug(object value);

  [EditorBrowsable(EditorBrowsableState.Never)]
  void Debug(IFormatProvider formatProvider, object value);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Debug(string message, object arg1, object arg2);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Debug(string message, object arg1, object arg2, object arg3);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Debug(IFormatProvider formatProvider, string message, bool argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Debug(string message, bool argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Debug(IFormatProvider formatProvider, string message, char argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Debug(string message, char argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Debug(IFormatProvider formatProvider, string message, byte argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Debug(string message, byte argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Debug(IFormatProvider formatProvider, string message, string argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Debug(string message, string argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Debug(IFormatProvider formatProvider, string message, int argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Debug(string message, int argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Debug(IFormatProvider formatProvider, string message, long argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Debug(string message, long argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Debug(IFormatProvider formatProvider, string message, float argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Debug(string message, float argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Debug(IFormatProvider formatProvider, string message, double argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Debug(string message, double argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Debug(IFormatProvider formatProvider, string message, Decimal argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Debug(string message, Decimal argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Debug(IFormatProvider formatProvider, string message, object argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Debug(string message, object argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Debug(IFormatProvider formatProvider, string message, sbyte argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Debug(string message, sbyte argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Debug(IFormatProvider formatProvider, string message, uint argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Debug(string message, uint argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Debug(IFormatProvider formatProvider, string message, ulong argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Debug(string message, ulong argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  void Info(object value);

  [EditorBrowsable(EditorBrowsableState.Never)]
  void Info(IFormatProvider formatProvider, object value);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Info(string message, object arg1, object arg2);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Info(string message, object arg1, object arg2, object arg3);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Info(IFormatProvider formatProvider, string message, bool argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Info(string message, bool argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Info(IFormatProvider formatProvider, string message, char argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Info(string message, char argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Info(IFormatProvider formatProvider, string message, byte argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Info(string message, byte argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Info(IFormatProvider formatProvider, string message, string argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Info(string message, string argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Info(IFormatProvider formatProvider, string message, int argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Info(string message, int argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Info(IFormatProvider formatProvider, string message, long argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Info(string message, long argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Info(IFormatProvider formatProvider, string message, float argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Info(string message, float argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Info(IFormatProvider formatProvider, string message, double argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Info(string message, double argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Info(IFormatProvider formatProvider, string message, Decimal argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Info(string message, Decimal argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Info(IFormatProvider formatProvider, string message, object argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Info(string message, object argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Info(IFormatProvider formatProvider, string message, sbyte argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Info(string message, sbyte argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Info(IFormatProvider formatProvider, string message, uint argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Info(string message, uint argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Info(IFormatProvider formatProvider, string message, ulong argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Info(string message, ulong argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  void Warn(object value);

  [EditorBrowsable(EditorBrowsableState.Never)]
  void Warn(IFormatProvider formatProvider, object value);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Warn(string message, object arg1, object arg2);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Warn(string message, object arg1, object arg2, object arg3);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Warn(IFormatProvider formatProvider, string message, bool argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Warn(string message, bool argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Warn(IFormatProvider formatProvider, string message, char argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Warn(string message, char argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Warn(IFormatProvider formatProvider, string message, byte argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Warn(string message, byte argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Warn(IFormatProvider formatProvider, string message, string argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Warn(string message, string argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Warn(IFormatProvider formatProvider, string message, int argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Warn(string message, int argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Warn(IFormatProvider formatProvider, string message, long argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Warn(string message, long argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Warn(IFormatProvider formatProvider, string message, float argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Warn(string message, float argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Warn(IFormatProvider formatProvider, string message, double argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Warn(string message, double argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Warn(IFormatProvider formatProvider, string message, Decimal argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Warn(string message, Decimal argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Warn(IFormatProvider formatProvider, string message, object argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Warn(string message, object argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Warn(IFormatProvider formatProvider, string message, sbyte argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Warn(string message, sbyte argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Warn(IFormatProvider formatProvider, string message, uint argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Warn(string message, uint argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Warn(IFormatProvider formatProvider, string message, ulong argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Warn(string message, ulong argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  void Error(object value);

  [EditorBrowsable(EditorBrowsableState.Never)]
  void Error(IFormatProvider formatProvider, object value);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Error(string message, object arg1, object arg2);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Error(string message, object arg1, object arg2, object arg3);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Error(IFormatProvider formatProvider, string message, bool argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  void Error(string message, bool argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Error(IFormatProvider formatProvider, string message, char argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Error(string message, char argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Error(IFormatProvider formatProvider, string message, byte argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  void Error(string message, byte argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Error(IFormatProvider formatProvider, string message, string argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Error(string message, string argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Error(IFormatProvider formatProvider, string message, int argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Error(string message, int argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Error(IFormatProvider formatProvider, string message, long argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Error(string message, long argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Error(IFormatProvider formatProvider, string message, float argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  void Error(string message, float argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Error(IFormatProvider formatProvider, string message, double argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Error(string message, double argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Error(IFormatProvider formatProvider, string message, Decimal argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Error(string message, Decimal argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Error(IFormatProvider formatProvider, string message, object argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Error(string message, object argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Error(IFormatProvider formatProvider, string message, sbyte argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Error(string message, sbyte argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Error(IFormatProvider formatProvider, string message, uint argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Error(string message, uint argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Error(IFormatProvider formatProvider, string message, ulong argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Error(string message, ulong argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  void Fatal(object value);

  [EditorBrowsable(EditorBrowsableState.Never)]
  void Fatal(IFormatProvider formatProvider, object value);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Fatal(string message, object arg1, object arg2);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Fatal(string message, object arg1, object arg2, object arg3);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Fatal(IFormatProvider formatProvider, string message, bool argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Fatal(string message, bool argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Fatal(IFormatProvider formatProvider, string message, char argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Fatal(string message, char argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Fatal(IFormatProvider formatProvider, string message, byte argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Fatal(string message, byte argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Fatal(IFormatProvider formatProvider, string message, string argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Fatal(string message, string argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Fatal(IFormatProvider formatProvider, string message, int argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Fatal(string message, int argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Fatal(IFormatProvider formatProvider, string message, long argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Fatal(string message, long argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Fatal(IFormatProvider formatProvider, string message, float argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Fatal(string message, float argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Fatal(IFormatProvider formatProvider, string message, double argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Fatal(string message, double argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Fatal(IFormatProvider formatProvider, string message, Decimal argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  void Fatal(string message, Decimal argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Fatal(IFormatProvider formatProvider, string message, object argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Fatal(string message, object argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Fatal(IFormatProvider formatProvider, string message, sbyte argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Fatal(string message, sbyte argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Fatal(IFormatProvider formatProvider, string message, uint argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Fatal(string message, uint argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Fatal(IFormatProvider formatProvider, string message, ulong argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Fatal(string message, ulong argument);

  bool IsTraceEnabled { get; }

  bool IsDebugEnabled { get; }

  bool IsInfoEnabled { get; }

  bool IsWarnEnabled { get; }

  bool IsErrorEnabled { get; }

  bool IsFatalEnabled { get; }

  void Trace<T>(T value);

  void Trace<T>(IFormatProvider formatProvider, T value);

  void Trace(LogMessageGenerator messageFunc);

  [Obsolete("Use Trace(Exception exception, string message, params object[] args) method instead. Marked obsolete before v4.3.11")]
  void TraceException([Localizable(false)] string message, Exception exception);

  void Trace(Exception exception, [Localizable(false)] string message);

  [MessageTemplateFormatMethod("message")]
  void Trace(Exception exception, [Localizable(false)] string message, params object[] args);

  [MessageTemplateFormatMethod("message")]
  void Trace(
    Exception exception,
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    params object[] args);

  [MessageTemplateFormatMethod("message")]
  void Trace(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args);

  void Trace([Localizable(false)] string message);

  [MessageTemplateFormatMethod("message")]
  void Trace([Localizable(false)] string message, params object[] args);

  [Obsolete("Use Trace(Exception exception, string message, params object[] args) method instead. Marked obsolete before v4.3.11")]
  void Trace([Localizable(false)] string message, Exception exception);

  [MessageTemplateFormatMethod("message")]
  void Trace<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument);

  [MessageTemplateFormatMethod("message")]
  void Trace<TArgument>([Localizable(false)] string message, TArgument argument);

  [MessageTemplateFormatMethod("message")]
  void Trace<TArgument1, TArgument2>(
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2);

  [MessageTemplateFormatMethod("message")]
  void Trace<TArgument1, TArgument2>([Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2);

  [MessageTemplateFormatMethod("message")]
  void Trace<TArgument1, TArgument2, TArgument3>(
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3);

  [MessageTemplateFormatMethod("message")]
  void Trace<TArgument1, TArgument2, TArgument3>(
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3);

  void Debug<T>(T value);

  void Debug<T>(IFormatProvider formatProvider, T value);

  void Debug(LogMessageGenerator messageFunc);

  [Obsolete("Use Debug(Exception exception, string message, params object[] args) method instead. Marked obsolete before v4.3.11")]
  void DebugException([Localizable(false)] string message, Exception exception);

  void Debug(Exception exception, [Localizable(false)] string message);

  [MessageTemplateFormatMethod("message")]
  void Debug(Exception exception, [Localizable(false)] string message, params object[] args);

  [MessageTemplateFormatMethod("message")]
  void Debug(
    Exception exception,
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    params object[] args);

  [MessageTemplateFormatMethod("message")]
  void Debug(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args);

  void Debug([Localizable(false)] string message);

  [MessageTemplateFormatMethod("message")]
  void Debug([Localizable(false)] string message, params object[] args);

  [Obsolete("Use Debug(Exception exception, string message, params object[] args) method instead. Marked obsolete before v4.3.11")]
  void Debug([Localizable(false)] string message, Exception exception);

  [MessageTemplateFormatMethod("message")]
  void Debug<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument);

  [MessageTemplateFormatMethod("message")]
  void Debug<TArgument>([Localizable(false)] string message, TArgument argument);

  [MessageTemplateFormatMethod("message")]
  void Debug<TArgument1, TArgument2>(
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2);

  [MessageTemplateFormatMethod("message")]
  void Debug<TArgument1, TArgument2>([Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2);

  [MessageTemplateFormatMethod("message")]
  void Debug<TArgument1, TArgument2, TArgument3>(
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3);

  [MessageTemplateFormatMethod("message")]
  void Debug<TArgument1, TArgument2, TArgument3>(
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3);

  void Info<T>(T value);

  void Info<T>(IFormatProvider formatProvider, T value);

  void Info(LogMessageGenerator messageFunc);

  [Obsolete("Use Info(Exception exception, string message, params object[] args) method instead. Marked obsolete before v4.3.11")]
  void InfoException([Localizable(false)] string message, Exception exception);

  void Info(Exception exception, [Localizable(false)] string message);

  [MessageTemplateFormatMethod("message")]
  void Info(Exception exception, [Localizable(false)] string message, params object[] args);

  [MessageTemplateFormatMethod("message")]
  void Info(
    Exception exception,
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    params object[] args);

  [MessageTemplateFormatMethod("message")]
  void Info(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args);

  void Info([Localizable(false)] string message);

  [MessageTemplateFormatMethod("message")]
  void Info([Localizable(false)] string message, params object[] args);

  [Obsolete("Use Info(Exception exception, string message, params object[] args) method instead. Marked obsolete before v4.3.11")]
  void Info([Localizable(false)] string message, Exception exception);

  [MessageTemplateFormatMethod("message")]
  void Info<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument);

  [MessageTemplateFormatMethod("message")]
  void Info<TArgument>([Localizable(false)] string message, TArgument argument);

  [MessageTemplateFormatMethod("message")]
  void Info<TArgument1, TArgument2>(
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2);

  [MessageTemplateFormatMethod("message")]
  void Info<TArgument1, TArgument2>([Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2);

  [MessageTemplateFormatMethod("message")]
  void Info<TArgument1, TArgument2, TArgument3>(
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3);

  [MessageTemplateFormatMethod("message")]
  void Info<TArgument1, TArgument2, TArgument3>(
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3);

  void Warn<T>(T value);

  void Warn<T>(IFormatProvider formatProvider, T value);

  void Warn(LogMessageGenerator messageFunc);

  [Obsolete("Use Warn(Exception exception, string message, params object[] args) method instead. Marked obsolete before v4.3.11")]
  void WarnException([Localizable(false)] string message, Exception exception);

  void Warn(Exception exception, [Localizable(false)] string message);

  [MessageTemplateFormatMethod("message")]
  void Warn(Exception exception, [Localizable(false)] string message, params object[] args);

  [MessageTemplateFormatMethod("message")]
  void Warn(
    Exception exception,
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    params object[] args);

  [MessageTemplateFormatMethod("message")]
  void Warn(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args);

  void Warn([Localizable(false)] string message);

  [MessageTemplateFormatMethod("message")]
  void Warn([Localizable(false)] string message, params object[] args);

  [Obsolete("Use Warn(Exception exception, string message, params object[] args) method instead. Marked obsolete before v4.3.11")]
  void Warn([Localizable(false)] string message, Exception exception);

  [MessageTemplateFormatMethod("message")]
  void Warn<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument);

  [MessageTemplateFormatMethod("message")]
  void Warn<TArgument>([Localizable(false)] string message, TArgument argument);

  [MessageTemplateFormatMethod("message")]
  void Warn<TArgument1, TArgument2>(
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2);

  [MessageTemplateFormatMethod("message")]
  void Warn<TArgument1, TArgument2>([Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2);

  [MessageTemplateFormatMethod("message")]
  void Warn<TArgument1, TArgument2, TArgument3>(
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3);

  [MessageTemplateFormatMethod("message")]
  void Warn<TArgument1, TArgument2, TArgument3>(
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3);

  void Error<T>(T value);

  void Error<T>(IFormatProvider formatProvider, T value);

  void Error(LogMessageGenerator messageFunc);

  [Obsolete("Use Error(Exception exception, string message, params object[] args) method instead. Marked obsolete before v4.3.11")]
  void ErrorException([Localizable(false)] string message, Exception exception);

  void Error(Exception exception, [Localizable(false)] string message);

  [MessageTemplateFormatMethod("message")]
  void Error(Exception exception, [Localizable(false)] string message, params object[] args);

  [MessageTemplateFormatMethod("message")]
  void Error(
    Exception exception,
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    params object[] args);

  [MessageTemplateFormatMethod("message")]
  void Error(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args);

  void Error([Localizable(false)] string message);

  [MessageTemplateFormatMethod("message")]
  void Error([Localizable(false)] string message, params object[] args);

  [Obsolete("Use Error(Exception exception, string message, params object[] args) method instead. Marked obsolete before v4.3.11")]
  void Error([Localizable(false)] string message, Exception exception);

  [MessageTemplateFormatMethod("message")]
  void Error<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument);

  [MessageTemplateFormatMethod("message")]
  void Error<TArgument>([Localizable(false)] string message, TArgument argument);

  [MessageTemplateFormatMethod("message")]
  void Error<TArgument1, TArgument2>(
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2);

  [MessageTemplateFormatMethod("message")]
  void Error<TArgument1, TArgument2>([Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2);

  [MessageTemplateFormatMethod("message")]
  void Error<TArgument1, TArgument2, TArgument3>(
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3);

  [MessageTemplateFormatMethod("message")]
  void Error<TArgument1, TArgument2, TArgument3>(
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3);

  void Fatal<T>(T value);

  void Fatal<T>(IFormatProvider formatProvider, T value);

  void Fatal(LogMessageGenerator messageFunc);

  [Obsolete("Use Fatal(Exception exception, string message, params object[] args) method instead. Marked obsolete before v4.3.11")]
  void FatalException([Localizable(false)] string message, Exception exception);

  void Fatal(Exception exception, [Localizable(false)] string message);

  [MessageTemplateFormatMethod("message")]
  void Fatal(Exception exception, [Localizable(false)] string message, params object[] args);

  [MessageTemplateFormatMethod("message")]
  void Fatal(
    Exception exception,
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    params object[] args);

  [MessageTemplateFormatMethod("message")]
  void Fatal(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args);

  void Fatal([Localizable(false)] string message);

  [MessageTemplateFormatMethod("message")]
  void Fatal([Localizable(false)] string message, params object[] args);

  [Obsolete("Use Fatal(Exception exception, string message, params object[] args) method instead. Marked obsolete before v4.3.11")]
  void Fatal([Localizable(false)] string message, Exception exception);

  [MessageTemplateFormatMethod("message")]
  void Fatal<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument);

  [MessageTemplateFormatMethod("message")]
  void Fatal<TArgument>([Localizable(false)] string message, TArgument argument);

  [MessageTemplateFormatMethod("message")]
  void Fatal<TArgument1, TArgument2>(
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2);

  [MessageTemplateFormatMethod("message")]
  void Fatal<TArgument1, TArgument2>([Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2);

  [MessageTemplateFormatMethod("message")]
  void Fatal<TArgument1, TArgument2, TArgument3>(
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3);

  [MessageTemplateFormatMethod("message")]
  void Fatal<TArgument1, TArgument2, TArgument3>(
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3);
}
