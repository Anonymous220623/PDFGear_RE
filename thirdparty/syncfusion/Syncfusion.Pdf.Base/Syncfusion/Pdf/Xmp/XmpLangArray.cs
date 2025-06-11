// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xmp.XmpLangArray
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xmp;

public class XmpLangArray : XmpCollection
{
  private const string c_langName = "x-default";
  private const string c_langAttribute = "lang";

  internal XmpLangArray(
    XmpMetadata xmp,
    XmlNode parent,
    string prefix,
    string localName,
    string namespaceURI)
    : base(xmp, parent, prefix, localName, namespaceURI)
  {
  }

  public string DefaultText
  {
    get
    {
      return this.XmlData.InnerXml.Contains("rdf") ? (this.GetItem("x-default") ?? this.CreateItem("x-default")).InnerXml : this.XmlData.InnerText;
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (DefaultText));
      XmpUtils.SetTextValue(this.GetItem("x-default") ?? this.CreateItem("x-default"), value);
    }
  }

  public string this[string lang]
  {
    get
    {
      string str = (string) null;
      XmlElement xmlElement = this.GetItem(lang);
      if (xmlElement != null)
        str = xmlElement.InnerXml;
      return str;
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      XmpUtils.SetTextValue(this.GetItem(lang) ?? this.CreateItem(lang), value);
    }
  }

  protected override XmpArrayType ArrayType => XmpArrayType.Alt;

  public void Add(string lang, string value)
  {
    if (lang == null)
      throw new ArgumentNullException(nameof (lang));
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    XmpUtils.SetTextValue(this.CreateItem(lang), value);
  }

  protected override void CreateEntity()
  {
    base.CreateEntity();
    this.CreateItem("x-default");
  }

  private XmlElement CreateItem(string lang)
  {
    if (lang == null)
      throw new ArgumentNullException(nameof (lang));
    XmlElement element = this.Xmp.CreateElement("rdf", "li", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
    this.ItemsContainer.AppendChild((XmlNode) element);
    XmlAttribute attribute = this.Xmp.CreateAttribute("xml", nameof (lang), "http://www.w3.org/XML/1998/namespace", lang);
    element.Attributes.Append(attribute);
    return element;
  }

  private XmlElement GetItem(string lang)
  {
    if (lang == null)
      throw new ArgumentNullException(nameof (lang));
    return this.ItemsContainer.SelectSingleNode($"./rdf:li[@xml:lang=\"{lang}\"]", this.Xmp.NamespaceManager) as XmlElement;
  }
}
