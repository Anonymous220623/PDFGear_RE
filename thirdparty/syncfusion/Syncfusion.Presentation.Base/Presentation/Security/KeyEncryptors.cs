// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Security.KeyEncryptors
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;
using System.Xml;

#nullable disable
namespace Syncfusion.Presentation.Security;

[CLSCompliant(false)]
internal class KeyEncryptors
{
  private EncryptedKey m_encryptedKey = new EncryptedKey();

  internal EncryptedKey EncryptedKey => this.m_encryptedKey;

  internal KeyEncryptors()
  {
  }

  internal void Parse(XmlReader reader)
  {
    reader.Read();
    while (reader.LocalName != "keyEncryptors")
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "encryptedKey":
            this.m_encryptedKey.Parse(reader);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  internal void Serialize(XmlWriter writer)
  {
    writer.WriteStartElement("keyEncryptors");
    writer.WriteStartElement("keyEncryptor");
    writer.WriteAttributeString("uri", "http://schemas.microsoft.com/office/2006/keyEncryptor/password");
    this.m_encryptedKey.Serialize(writer);
    writer.WriteEndElement();
    writer.WriteEndElement();
  }
}
