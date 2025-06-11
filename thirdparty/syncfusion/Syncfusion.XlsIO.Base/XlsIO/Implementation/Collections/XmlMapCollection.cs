// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.XmlMapCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Compression;
using Syncfusion.XlsIO.Implementation.XmlReaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class XmlMapCollection
{
  internal const string MapInfoElement = "MapInfo";
  internal const string SelectionNamespacesAttribute = "SelectionNamespaces";
  private IList<XmlMap> m_xmlMaps;
  private WorkbookImpl m_parent;

  public XmlMap this[int index]
  {
    get
    {
      return this.m_xmlMaps != null && index < this.m_xmlMaps.Count && index > -1 ? this.m_xmlMaps[index] : (XmlMap) null;
    }
  }

  public int Count => this.m_xmlMaps.Count;

  public XmlMapCollection(WorkbookImpl parent)
  {
    this.m_parent = parent;
    this.m_xmlMaps = (IList<XmlMap>) new List<XmlMap>();
  }

  internal bool Add(Stream stream, IWorksheet sheet, int row, int column)
  {
    XmlMap xmlMap = new XmlMap();
    if (!xmlMap.BindXml(stream, this.m_parent, sheet, row, column, this.m_xmlMaps.Count))
      return false;
    this.m_xmlMaps.Add(xmlMap);
    return true;
  }

  internal bool Contains(string rootElement)
  {
    if (this.m_xmlMaps != null)
    {
      foreach (XmlMap xmlMap in (IEnumerable<XmlMap>) this.m_xmlMaps)
      {
        if (xmlMap.RootElement == rootElement)
          return true;
      }
    }
    return false;
  }

  internal void Parse(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException("Reader");
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "Schema":
            XmlMap xmlMap1;
            XmlMap xmlMap2 = xmlMap1 = new XmlMap();
            if (reader.MoveToAttribute("ID"))
              xmlMap2.SchemaId = reader.Value;
            reader.Read();
            xmlMap2.ParseSchema(reader);
            this.m_xmlMaps.Add(xmlMap2);
            Excel2007Parser.SkipWhiteSpaces(reader);
            reader.Read();
            continue;
          case "Map":
            if (reader.MoveToAttribute("SchemaID"))
            {
              XmlMap xmlMap3 = this.GetXmlMap(reader.Value);
              if (xmlMap3 != null)
              {
                if (reader.MoveToAttribute("ID"))
                  xmlMap3.MapId = XmlConvertExtension.ToInt32(reader.Value);
                if (reader.MoveToAttribute("Name"))
                  xmlMap3.Name = reader.Value;
                if (reader.MoveToAttribute("RootElement"))
                  xmlMap3.RootElement = reader.Value;
              }
            }
            reader.Skip();
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
  }

  internal XmlMap GetXmlMap(string schemaId)
  {
    foreach (XmlMap xmlMap in (IEnumerable<XmlMap>) this.m_xmlMaps)
    {
      if (xmlMap.SchemaId == schemaId)
        return xmlMap;
    }
    return (XmlMap) null;
  }

  internal void Serialize(XmlWriter writer)
  {
    writer.WriteStartElement("MapInfo", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    writer.WriteAttributeString("SelectionNamespaces", string.Empty);
    for (int index = 0; index < this.m_xmlMaps.Count; ++index)
      this.m_xmlMaps[index].SerializeSchema(writer);
    for (int index = 0; index < this.m_xmlMaps.Count; ++index)
      this.m_xmlMaps[index].SerializeMapInformation(writer);
    writer.WriteEndElement();
  }

  internal void Dispose()
  {
    if (this.m_xmlMaps == null)
      return;
    foreach (XmlMap xmlMap in (IEnumerable<XmlMap>) this.m_xmlMaps)
      xmlMap.Dispose();
    this.m_xmlMaps = (IList<XmlMap>) null;
  }
}
