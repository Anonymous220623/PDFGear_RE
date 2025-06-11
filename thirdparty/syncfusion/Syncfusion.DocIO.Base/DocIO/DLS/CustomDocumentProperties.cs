// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.CustomDocumentProperties
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using Syncfusion.DocIO.Utilities;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class CustomDocumentProperties : XDLSSerializableBase
{
  internal const string TagName = "property";
  internal const string NameAttribute = "name";
  internal const string PIDAttribute = "pid";
  internal const string FMTIDAttribute = "fmtid";
  protected Dictionary<string, DocumentProperty> m_customList;

  internal Dictionary<string, DocumentProperty> CustomHash => this.m_customList;

  public DocumentProperty this[string name]
  {
    get => this.m_customList.ContainsKey(name) ? this.m_customList[name] : (DocumentProperty) null;
  }

  public DocumentProperty this[int index]
  {
    get
    {
      int num = 0;
      foreach (string key in this.m_customList.Keys)
      {
        if (num == index)
          return this.m_customList[key];
        ++num;
      }
      return (DocumentProperty) null;
    }
  }

  public int Count => this.m_customList.Count;

  internal CustomDocumentProperties()
    : this(0)
  {
  }

  internal CustomDocumentProperties(int count)
    : base((WordDocument) null, (Entity) null)
  {
    this.m_customList = new Dictionary<string, DocumentProperty>(count);
  }

  public DocumentProperty Add(string name, object value)
  {
    DocumentProperty documentProperty = new DocumentProperty(name, value, DocumentProperty.DetectPropertyType(value));
    this.m_customList.Add(name, documentProperty);
    return documentProperty;
  }

  public void Remove(string name) => this.CustomHash.Remove(name);

  public CustomDocumentProperties Clone()
  {
    CustomDocumentProperties documentProperties = new CustomDocumentProperties(this.m_customList.Count);
    foreach (string key in this.m_customList.Keys)
    {
      DocumentProperty custom = this.m_customList[key];
      documentProperties.m_customList.Add(key, custom.Clone());
    }
    return documentProperties;
  }

  protected override void WriteXmlContent(IXDLSContentWriter writer)
  {
    base.WriteXmlContent(writer);
    IXDLSAttributeWriter xdlsAttributeWriter = writer as IXDLSAttributeWriter;
    XmlWriter innerWriter = (writer as XDLSWriter).InnerWriter;
    if (this.m_customList == null || this.m_customList.Count <= 0)
      return;
    foreach (string key in this.m_customList.Keys)
    {
      DocumentProperty custom = this.m_customList[key];
      innerWriter.WriteStartElement("property");
      innerWriter.WriteAttributeString("Name", key);
      switch (custom.PropertyType)
      {
        case Syncfusion.CompoundFile.DocIO.PropertyType.Double:
          innerWriter.WriteAttributeString("Type", "double");
          xdlsAttributeWriter.WriteValue("Value", custom.Double);
          break;
        case Syncfusion.CompoundFile.DocIO.PropertyType.Bool:
          innerWriter.WriteAttributeString("Type", "bool");
          xdlsAttributeWriter.WriteValue("Value", custom.Boolean);
          break;
        case Syncfusion.CompoundFile.DocIO.PropertyType.Int:
          innerWriter.WriteAttributeString("Type", "int");
          xdlsAttributeWriter.WriteValue("Value", custom.Integer);
          break;
        case Syncfusion.CompoundFile.DocIO.PropertyType.String:
          innerWriter.WriteAttributeString("Type", "string");
          xdlsAttributeWriter.WriteValue("Value", custom.ToString());
          break;
        case Syncfusion.CompoundFile.DocIO.PropertyType.DateTime:
          innerWriter.WriteAttributeString("Type", "DateTime");
          xdlsAttributeWriter.WriteValue("Value", custom.DateTime);
          break;
      }
      innerWriter.WriteEndElement();
    }
  }

  protected override bool ReadXmlContent(IXDLSContentReader reader)
  {
    bool flag = base.ReadXmlContent(reader);
    this.ReadProperty(reader as XDLSReader);
    return flag;
  }

  private void ReadProperty(XDLSReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (!(reader.InnerReader.LocalName == "property"))
      return;
    string str1 = reader.ReadString("Type");
    string str2 = reader.ReadString("Name");
    object obj = (object) null;
    switch (str1)
    {
      case "bool":
        obj = (object) reader.ReadBoolean("Value");
        break;
      case "string":
        obj = (object) reader.ReadString("Value");
        break;
      case "DateTime":
        obj = (object) reader.ReadDateTime("Value");
        break;
      case "int":
        obj = (object) reader.ReadInt("Value");
        break;
      case "double":
        obj = (object) reader.ReadDouble("Value");
        break;
      case "array":
        if (!reader.InnerReader.IsEmptyElement)
        {
          string localName = reader.InnerReader.LocalName;
          reader.InnerReader.ReadStartElement();
          reader.InnerReader.Read();
          if (!(localName == reader.InnerReader.LocalName) || reader.InnerReader.NodeType != XmlNodeType.EndElement)
          {
            while (reader.NodeType != XmlNodeType.Element)
              reader.InnerReader.Read();
            obj = (object) reader.ReadChildBinaryElement();
            break;
          }
          break;
        }
        break;
      case "clip":
        string data = reader.ReadString("Value");
        ClipDataWrapper clipDataWrapper = new ClipDataWrapper();
        clipDataWrapper.Read(data);
        obj = (object) clipDataWrapper;
        break;
    }
    DocumentProperty documentProperty = new DocumentProperty(str2, obj);
    this.m_customList.Add(str2, documentProperty);
  }

  internal override void Close()
  {
    base.Close();
    if (this.m_customList == null)
      return;
    foreach (DocumentProperty documentProperty in this.m_customList.Values)
      documentProperty.Close();
    this.m_customList.Clear();
    this.m_customList = (Dictionary<string, DocumentProperty>) null;
  }
}
