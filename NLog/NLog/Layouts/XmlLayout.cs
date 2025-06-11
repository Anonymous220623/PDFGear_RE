// Decompiled with JetBrains decompiler
// Type: NLog.Layouts.XmlLayout
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System.ComponentModel;

#nullable disable
namespace NLog.Layouts;

[Layout("XmlLayout")]
[NLog.Config.ThreadAgnostic]
[NLog.Config.ThreadSafe]
public class XmlLayout(string elementName, Layout elementValue) : XmlElementBase(elementName, elementValue)
{
  private const string DefaultRootElementName = "logevent";

  public XmlLayout()
    : this("logevent", (Layout) null)
  {
  }

  [DefaultValue("logevent")]
  public string ElementName
  {
    get => this.ElementNameInternal;
    set => this.ElementNameInternal = value;
  }

  public Layout ElementValue
  {
    get => this.LayoutWrapper.Inner;
    set => this.LayoutWrapper.Inner = value;
  }

  [DefaultValue(true)]
  public bool ElementEncode
  {
    get => this.LayoutWrapper.XmlEncode;
    set => this.LayoutWrapper.XmlEncode = value;
  }
}
