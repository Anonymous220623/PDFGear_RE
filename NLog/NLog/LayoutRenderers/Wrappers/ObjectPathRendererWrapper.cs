// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.ObjectPathRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.Globalization;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers;

[LayoutRenderer("Object-Path")]
[AmbientProperty("ObjectPath")]
[ThreadSafe]
[ThreadAgnostic]
public class ObjectPathRendererWrapper : WrapperLayoutRendererBase, IRawValue
{
  private readonly ObjectPropertyHelper _objectPropertyHelper = new ObjectPropertyHelper();

  public string Path
  {
    get => this.ObjectPath;
    set => this.ObjectPath = value;
  }

  public string ObjectPath
  {
    get => this._objectPropertyHelper.ObjectPath;
    set => this._objectPropertyHelper.ObjectPath = value;
  }

  public string Format { get; set; }

  public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;

  protected override string Transform(string text) => throw new NotSupportedException();

  protected override void RenderInnerAndTransform(
    LogEventInfo logEvent,
    StringBuilder builder,
    int orgLength)
  {
    object obj;
    if (!this.TryGetRawValue(logEvent, out obj))
      return;
    IFormatProvider formatProvider = this.GetFormatProvider(logEvent, (IFormatProvider) this.Culture);
    builder.AppendFormattedValue(obj, this.Format, formatProvider);
  }

  public bool TryGetRawValue(LogEventInfo logEvent, out object value)
  {
    object rawValue;
    if (this.Inner != null && this.Inner.TryGetRawValue(logEvent, out rawValue) && this._objectPropertyHelper.TryGetObjectProperty(rawValue, out value))
      return true;
    value = (object) null;
    return false;
  }
}
