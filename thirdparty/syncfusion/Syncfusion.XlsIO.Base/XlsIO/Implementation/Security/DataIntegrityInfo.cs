// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Security.DataIntegrityInfo
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Security;

public class DataIntegrityInfo
{
  private byte[] m_HMacKey;
  private byte[] m_HmacValue;

  internal byte[] HMacKey
  {
    get => this.m_HMacKey;
    set => this.m_HMacKey = value;
  }

  internal byte[] HMacValue
  {
    get => this.m_HmacValue;
    set => this.m_HmacValue = value;
  }

  internal void Parse(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "dataIntegrity")
      throw new XmlException();
    if (reader.MoveToAttribute("encryptedHmacKey"))
      this.m_HMacKey = Convert.FromBase64String(reader.Value);
    if (!reader.MoveToAttribute("encryptedHmacValue"))
      return;
    this.m_HmacValue = Convert.FromBase64String(reader.Value);
  }

  internal void Serialize(XmlWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartElement("dataIntegrity");
    writer.WriteAttributeString("encryptedHmacKey", Convert.ToBase64String(this.m_HMacKey));
    writer.WriteAttributeString("encryptedHmacValue", Convert.ToBase64String(this.m_HmacValue));
    writer.WriteEndElement();
  }
}
