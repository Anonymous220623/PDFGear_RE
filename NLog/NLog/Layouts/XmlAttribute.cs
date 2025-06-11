// Decompiled with JetBrains decompiler
// Type: NLog.Layouts.XmlAttribute
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using NLog.LayoutRenderers.Wrappers;
using System.ComponentModel;

#nullable disable
namespace NLog.Layouts;

[NLogConfigurationItem]
public class XmlAttribute
{
  private string _name;
  internal readonly XmlEncodeLayoutRendererWrapper LayoutWrapper = new XmlEncodeLayoutRendererWrapper();

  public XmlAttribute()
    : this((string) null, (Layout) null, true)
  {
  }

  public XmlAttribute(string name, Layout layout)
    : this(name, layout, true)
  {
  }

  public XmlAttribute(string name, Layout layout, bool encode)
  {
    this.Name = name;
    this.Layout = layout;
    this.Encode = encode;
    this.IncludeEmptyValue = false;
    this.LayoutWrapper.XmlEncodeNewlines = true;
  }

  [RequiredParameter]
  public string Name
  {
    get => this._name;
    set => this._name = XmlHelper.XmlConvertToElementName(value?.Trim(), true);
  }

  [RequiredParameter]
  public Layout Layout
  {
    get => this.LayoutWrapper.Inner;
    set => this.LayoutWrapper.Inner = value;
  }

  [DefaultValue(true)]
  public bool Encode
  {
    get => this.LayoutWrapper.XmlEncode;
    set => this.LayoutWrapper.XmlEncode = value;
  }

  public bool IncludeEmptyValue { get; set; }
}
