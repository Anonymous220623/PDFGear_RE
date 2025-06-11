// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xmp.XmpArray
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xmp;

public class XmpArray : XmpCollection
{
  internal const string c_dateFormat = "yyyy-MM-dd'T'HH:mm:ss.ffzzz";
  private XmpArrayType m_arrayType;

  internal XmpArray(
    XmpMetadata xmp,
    XmlNode parent,
    string prefix,
    string localName,
    string namespaceURI,
    XmpArrayType type)
    : base(xmp, parent, prefix, localName, namespaceURI)
  {
    this.m_arrayType = type;
    this.Initialize();
  }

  public string[] Items => this.GetArrayValues();

  protected override XmpArrayType ArrayType => this.m_arrayType;

  public void Add(string value)
  {
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    XmpUtils.SetTextValue(this.CreateItem(), value);
  }

  public void Add(int value) => XmpUtils.SetIntValue(this.CreateItem(), value);

  public void Add(float value) => XmpUtils.SetRealValue(this.CreateItem(), value);

  public void Add(DateTime value) => this.Add(value.ToString("yyyy-MM-dd'T'HH:mm:ss.ffzzz"));

  public void Add(DateTime value, string format)
  {
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    this.Add(value.ToString(format));
  }

  public void Add(XmpStructure structure)
  {
    if (structure == null)
      throw new ArgumentNullException(nameof (structure));
    XmlElement parent = this.CreateItem();
    XmpUtils.SetXmlValue(parent, structure.XmlData);
    this.ChangeParent((XmlNode) parent, (XmpEntityBase) structure);
  }

  private XmlElement CreateItem()
  {
    if (this.ItemsContainer.FirstChild != null)
      this.ItemsContainer.RemoveChild(this.ItemsContainer.FirstChild);
    XmlElement element = this.Xmp.CreateElement("rdf", "li", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
    this.ItemsContainer.AppendChild((XmlNode) element);
    return element;
  }

  private string[] GetArrayValues()
  {
    string[] arrayValues = new string[1];
    if (this.XmlData.InnerXml.Contains("rdf"))
    {
      XmlNodeList arrayItems = this.GetArrayItems();
      if (arrayItems.Count == 0)
      {
        arrayValues[0] = string.Empty;
      }
      else
      {
        arrayValues = new string[arrayItems.Count];
        int i = 0;
        for (int count = arrayItems.Count; i < count; ++i)
        {
          XmlNode xmlNode = arrayItems[i];
          arrayValues[i] = xmlNode.InnerXml;
        }
      }
    }
    else
      arrayValues[0] = this.XmlData.InnerText;
    return arrayValues;
  }

  private void ChangeParent(XmlNode parent, XmpEntityBase entity)
  {
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    if (entity == null)
      throw new ArgumentNullException(nameof (entity));
    entity.SetXmlParent(parent);
  }
}
