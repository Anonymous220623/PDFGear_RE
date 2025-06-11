// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.ProcessInfoLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("processinfo")]
[ThreadSafe]
public class ProcessInfoLayoutRenderer : LayoutRenderer
{
  private Process _process;
  private ReflectionHelpers.LateBoundMethod _lateBoundPropertyGet;

  [DefaultValue("Id")]
  [DefaultParameter]
  public ProcessInfoProperty Property { get; set; } = ProcessInfoProperty.Id;

  [DefaultValue(null)]
  public string Format { get; set; }

  protected override void InitializeLayoutRenderer()
  {
    base.InitializeLayoutRenderer();
    PropertyInfo property = typeof (Process).GetProperty(this.Property.ToString());
    this._lateBoundPropertyGet = !(property == (PropertyInfo) null) ? ReflectionHelpers.CreateLateBoundMethod(property.GetGetMethod()) : throw new ArgumentException($"Property '{this.Property}' not found in System.Diagnostics.Process");
    this._process = Process.GetCurrentProcess();
  }

  protected override void CloseLayoutRenderer()
  {
    if (this._process != null)
    {
      this._process.Close();
      this._process = (Process) null;
    }
    base.CloseLayoutRenderer();
  }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    object obj = this.GetValue();
    if (obj == null)
      return;
    IFormatProvider formatProvider = this.GetFormatProvider(logEvent);
    builder.AppendFormattedValue(obj, this.Format, formatProvider);
  }

  private object GetValue()
  {
    ReflectionHelpers.LateBoundMethod boundPropertyGet = this._lateBoundPropertyGet;
    return boundPropertyGet == null ? (object) null : boundPropertyGet((object) this._process, (object[]) null);
  }
}
