// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xmp.XmpCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xmp;

public abstract class XmpCollection : XmpType
{
  protected const string c_itemName = "li";

  internal XmpCollection(
    XmpMetadata xmp,
    XmlNode parent,
    string prefix,
    string localName,
    string namespaceURI)
    : base(xmp, parent, prefix, localName, namespaceURI)
  {
  }

  public int Count => this.GetItemsCount();

  protected abstract XmpArrayType ArrayType { get; }

  protected XmlElement ItemsContainer
  {
    get
    {
      return (this.XmlData.SelectSingleNode("./rdf:" + this.GetArrayName(), this.Xmp.NamespaceManager) ?? throw new ArgumentNullException("node")) as XmlElement;
    }
  }

  protected override void CreateEntity()
  {
    if (this.ArrayType == XmpArrayType.Unknown)
      return;
    base.CreateEntity();
    this.XmlData.AppendChild((XmlNode) this.Xmp.CreateElement("rdf", this.GetArrayName(), "http://www.w3.org/1999/02/22-rdf-syntax-ns#"));
  }

  protected XmlNodeList GetArrayItems()
  {
    return this.ItemsContainer.SelectNodes("./rdf:li", this.Xmp.NamespaceManager);
  }

  private string GetArrayName()
  {
    string arrayName = XmpArrayType.Bag.ToString();
    if (this.XmlData.InnerXml.Contains("rdf:Seq"))
      arrayName = XmpArrayType.Seq.ToString();
    else if (this.XmlData.InnerXml.Contains("rdf:Alt"))
      arrayName = XmpArrayType.Alt.ToString();
    return arrayName;
  }

  private int GetItemsCount() => this.GetArrayItems().Count;
}
