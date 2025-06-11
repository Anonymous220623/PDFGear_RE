// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.MsoDataStore
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.CompoundFile.XlsIO;
using Syncfusion.XlsIO.Implementation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

internal class MsoDataStore
{
  private WorkbookImpl m_book;
  private IApplication m_application;
  private ICompoundStorage m_storage;
  private int m_Count;

  public MsoDataStore(ICompoundStorage compoundStorage, WorkbookImpl book)
  {
    this.m_book = book;
    this.m_storage = compoundStorage;
  }

  public void ParseMsoDataStore()
  {
    List<string> schemas = (List<string>) null;
    ICustomXmlPartCollection customXmlparts = this.m_book.CustomXmlparts;
    foreach (string storage in this.m_storage.Storages)
    {
      ICompoundStorage compoundStorage = this.m_storage.OpenStorage(storage);
      Stream propertystream = (Stream) compoundStorage.OpenStream("Properties");
      Stream stream = (Stream) compoundStorage.OpenStream("Item");
      string xmlItemProperties = this.ParseCustomXmlItemProperties(propertystream, ref schemas);
      this.ParseCustomXmlParts(stream, customXmlparts, xmlItemProperties, schemas);
    }
  }

  private void ParseCustomXmlParts(
    Stream stream,
    ICustomXmlPartCollection customXmlParts,
    string XmlId,
    List<string> schemas)
  {
    if (stream == null)
      return;
    stream.Position = 0L;
    byte[] buffer = new byte[16384 /*0x4000*/];
    using (MemoryStream memoryStream = new MemoryStream())
    {
      int count;
      while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
        memoryStream.Write(buffer, 0, count);
      ICustomXmlPart customXmlPart = customXmlParts.Add(XmlId, memoryStream.ToArray());
      if (schemas == null || schemas.Count <= 0)
        return;
      foreach (string schema in schemas)
        customXmlPart.Schemas.Add(schema);
    }
  }

  private string ParseCustomXmlItemProperties(Stream propertystream, ref List<string> schemas)
  {
    propertystream.Position = 0L;
    return this.ParseItemProperties(UtilityMethods.CreateReader(propertystream), ref schemas);
  }

  private string ParseItemProperties(XmlReader reader, ref List<string> schemas)
  {
    string itemProperties = (string) null;
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    List<string> stringList = new List<string>();
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "datastoreItem":
            if (reader.MoveToAttribute("ds:itemID"))
              itemProperties = reader.Value;
            reader.Read();
            continue;
          case "schemaRefs":
            try
            {
              this.ParseschemaReference(reader, ref schemas);
              reader.Read();
              continue;
            }
            catch (XmlException ex)
            {
              return itemProperties;
            }
          default:
            continue;
        }
      }
    }
    return itemProperties;
  }

  private void ParseschemaReference(XmlReader reader, ref List<string> schemas)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "schemaRefs")
      throw new XmlException("Wrong xml tag");
    schemas = new List<string>();
    if (reader.IsEmptyElement)
      return;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      switch (reader.LocalName)
      {
        case "schemaRef":
          this.ParseSchemaRef(reader, ref schemas);
          continue;
        case "schemaRefs":
          reader.Read();
          continue;
        default:
          reader.Skip();
          continue;
      }
    }
  }

  private void ParseSchemaRef(XmlReader reader, ref List<string> schemas)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "schemaRef")
      throw new XmlException("Wrong xml tag");
    if (reader.MoveToAttribute("ds:uri"))
    {
      string str = reader.Value;
      schemas.Add(str);
    }
    reader.Read();
  }

  internal void SerializeMetaStore()
  {
    ICustomXmlPartCollection customXmlparts = this.m_book.CustomXmlparts;
    if (customXmlparts == null || customXmlparts.Count <= 0)
      return;
    ICompoundStorage storage1 = this.m_storage.CreateStorage(nameof (MsoDataStore));
    for (int index = 0; index < customXmlparts.Count; ++index)
    {
      ICompoundStorage storage2 = storage1.CreateStorage($"cxds{index}");
      Stream stream1 = (Stream) storage2.CreateStream("Properties");
      Stream stream2 = (Stream) storage2.CreateStream("Item");
      ICustomXmlPart customXmlPart = customXmlparts[index];
      this.SerializeCustomXmlProperty(stream1, customXmlPart);
      this.SerializeCustomXmlPart(stream2, customXmlPart.Data);
    }
  }

  private void SerializeCustomXmlProperty(Stream itemsteam, ICustomXmlPart customXmlPart)
  {
    MemoryStream memoryStream = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) new StreamWriter((Stream) memoryStream));
    this.SerializeCustomXmlPartProperty(writer, customXmlPart);
    writer.Flush();
    memoryStream.Position = 0L;
    byte[] array = memoryStream.ToArray();
    itemsteam.Write(array, 0, array.Length);
  }

  public void SerializeCustomXmlPartProperty(XmlWriter writer, ICustomXmlPart customXmlPart)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    string str1 = customXmlPart != null ? customXmlPart.Id : throw new ArgumentNullException(nameof (customXmlPart));
    ICustomXmlSchemaCollection schemas = customXmlPart.Schemas;
    writer.WriteStartDocument(true);
    writer.WriteStartElement("ds", "datastoreItem", "http://schemas.openxmlformats.org/officeDocument/2006/customXml");
    writer.WriteAttributeString("ds", "itemID", (string) null, str1);
    if (schemas != null && schemas.Count > 0)
    {
      writer.WriteStartElement("ds", "schemaRefs", (string) null);
      foreach (string str2 in (IEnumerable) schemas)
      {
        writer.WriteStartElement("ds", "schemaRef", (string) null);
        writer.WriteAttributeString("ds", "uri", (string) null, str2);
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private void SerializeCustomXmlPart(Stream storage, byte[] data)
  {
    if (data == null)
      return;
    storage.Write(data, 0, data.Length);
  }
}
