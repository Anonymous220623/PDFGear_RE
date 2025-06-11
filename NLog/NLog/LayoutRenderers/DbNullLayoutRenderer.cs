// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.DbNullLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("db-null")]
[ThreadSafe]
[ThreadAgnostic]
public class DbNullLayoutRenderer : LayoutRenderer, IRawValue
{
  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
  }

  bool IRawValue.TryGetRawValue(LogEventInfo logEvent, out object value)
  {
    value = (object) DBNull.Value;
    return true;
  }
}
