// Decompiled with JetBrains decompiler
// Type: NLog.IValueFormatter
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.MessageTemplates;
using System;
using System.Text;

#nullable disable
namespace NLog;

public interface IValueFormatter
{
  bool FormatValue(
    object value,
    string format,
    CaptureType captureType,
    IFormatProvider formatProvider,
    StringBuilder builder);
}
