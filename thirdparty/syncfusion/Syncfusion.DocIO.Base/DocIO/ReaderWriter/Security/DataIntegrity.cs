// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Security.DataIntegrity
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Xml;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Security;

[CLSCompliant(false)]
internal class DataIntegrity
{
  private byte[] m_encryptedHmacKey;
  private byte[] m_encryptedHmacValue;

  internal byte[] EncryptedHmacKey
  {
    get => this.m_encryptedHmacKey;
    set => this.m_encryptedHmacKey = value;
  }

  internal byte[] EncryptedHmacValue
  {
    get => this.m_encryptedHmacValue;
    set => this.m_encryptedHmacValue = value;
  }

  internal DataIntegrity()
  {
  }

  internal void Parse(XmlReader reader)
  {
    this.m_encryptedHmacKey = Convert.FromBase64String(reader.GetAttribute("encryptedHmacKey"));
    this.m_encryptedHmacValue = Convert.FromBase64String(reader.GetAttribute("encryptedHmacValue"));
  }

  internal void Serialize(XmlWriter writer)
  {
    writer.WriteStartElement("dataIntegrity");
    writer.WriteAttributeString("encryptedHmacKey", Convert.ToBase64String(this.m_encryptedHmacKey));
    writer.WriteAttributeString("encryptedHmacValue", Convert.ToBase64String(this.m_encryptedHmacValue));
    writer.WriteEndElement();
  }
}
