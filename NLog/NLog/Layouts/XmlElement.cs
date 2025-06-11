// Decompiled with JetBrains decompiler
// Type: NLog.Layouts.XmlElement
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using System.ComponentModel;

#nullable disable
namespace NLog.Layouts;

[NLogConfigurationItem]
public class XmlElement(string elementName, Layout elementValue) : XmlElementBase(elementName, elementValue)
{
  private const string DefaultElementName = "item";

  public XmlElement()
    : this("item", (Layout) null)
  {
  }

  [DefaultValue("item")]
  public string Name
  {
    get => this.ElementNameInternal;
    set => this.ElementNameInternal = value;
  }

  public Layout Value
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
}
