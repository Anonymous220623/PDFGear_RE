// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Security.XmlEncryptionDescriptor
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Security;

[CLSCompliant(false)]
internal class XmlEncryptionDescriptor
{
  private const string XMLNameSpace = "xmlns";
  private KeyData m_keyData = new KeyData();
  private DataIntegrity m_dataIntegrity = new DataIntegrity();
  private KeyEncryptors m_keyEncryptors = new KeyEncryptors();

  internal KeyData KeyData
  {
    get => this.m_keyData;
    set => this.m_keyData = value;
  }

  internal DataIntegrity DataIntegrity
  {
    get => this.m_dataIntegrity;
    set => this.m_dataIntegrity = value;
  }

  internal KeyEncryptors KeyEncryptors
  {
    get => this.m_keyEncryptors;
    set => this.m_keyEncryptors = value;
  }

  public void Parse(Stream stream)
  {
    XmlReader reader = this.CreateReader(stream);
    reader.Read();
    while (reader.LocalName != "encryption")
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "keyData":
            this.m_keyData.Parse(reader);
            break;
          case "dataIntegrity":
            this.m_dataIntegrity.Parse(reader);
            break;
          case "keyEncryptors":
            this.m_keyEncryptors.Parse(reader);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  public void Serialize(Stream stream)
  {
    XmlWriter writer = this.CreateWriter(stream);
    writer.WriteStartElement("encryption", "http://schemas.microsoft.com/office/2006/encryption");
    writer.WriteAttributeString("xmlns", "http://schemas.microsoft.com/office/2006/encryption");
    writer.WriteAttributeString("xmlns", "p", (string) null, "http://schemas.microsoft.com/office/2006/keyEncryptor/password");
    if (this.m_keyData.KeyBits == 256 /*0x0100*/)
      writer.WriteAttributeString("xmlns", "c", (string) null, "http://schemas.microsoft.com/office/2006/keyEncryptor/certificate");
    this.m_keyData.Serialize(writer);
    this.m_dataIntegrity.Serialize(writer);
    this.m_keyEncryptors.Serialize(writer);
    writer.WriteEndElement();
    writer.Flush();
  }

  private XmlWriter CreateWriter(Stream data)
  {
    XmlWriterSettings settings = new XmlWriterSettings();
    XmlWriter writer = XmlWriter.Create(data, settings);
    writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"");
    return writer;
  }

  public XmlReader CreateReader(Stream data)
  {
    XmlReader reader = XmlReader.Create(data);
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    return reader;
  }
}
