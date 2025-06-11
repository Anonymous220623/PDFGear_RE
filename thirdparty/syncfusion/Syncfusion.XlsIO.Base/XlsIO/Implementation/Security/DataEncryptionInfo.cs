// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Security.DataEncryptionInfo
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Security;

public class DataEncryptionInfo
{
  private int m_saltSize;
  private int m_blockSize;
  private int m_keyBits;
  private int m_hashSize;
  private string m_cipherAlgorithm;
  private string m_cipherChaining;
  private string m_hashAlgorithm;
  private byte[] m_saltValue;

  internal byte[] SaltValue
  {
    get => this.m_saltValue;
    set => this.m_saltValue = value;
  }

  internal DataEncryptionInfo()
  {
    this.m_saltSize = 16 /*0x10*/;
    this.m_blockSize = 16 /*0x10*/;
    this.m_keyBits = 128 /*0x80*/;
    this.m_hashSize = 20;
    this.m_cipherAlgorithm = "AES";
    this.m_cipherChaining = "ChainingModeCBC";
    this.m_hashAlgorithm = "SHA1";
  }

  internal void Parse(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "keyData")
      throw new XmlException();
    if (reader.MoveToAttribute("saltSize"))
      this.m_saltSize = Convert.ToInt32(reader.Value);
    if (reader.MoveToAttribute("blockSize"))
      this.m_blockSize = Convert.ToInt32(reader.Value);
    if (reader.MoveToAttribute("keyBits"))
      this.m_keyBits = Convert.ToInt32(reader.Value);
    if (reader.MoveToAttribute("hashSize"))
      this.m_hashSize = Convert.ToInt32(reader.Value);
    if (reader.MoveToAttribute("cipherAlgorithm"))
      this.m_cipherAlgorithm = reader.Value;
    if (reader.MoveToAttribute("cipherChaining"))
      this.m_cipherChaining = reader.Value;
    if (reader.MoveToAttribute("hashAlgorithm"))
      this.m_hashAlgorithm = reader.Value;
    if (!reader.MoveToAttribute("saltValue"))
      return;
    this.m_saltValue = Convert.FromBase64String(reader.Value);
  }

  internal void Serialize(XmlWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartElement("keyData");
    writer.WriteAttributeString("saltSize", this.m_saltSize.ToString());
    writer.WriteAttributeString("blockSize", this.m_blockSize.ToString());
    writer.WriteAttributeString("keyBits", this.m_keyBits.ToString());
    writer.WriteAttributeString("hashSize", this.m_hashSize.ToString());
    writer.WriteAttributeString("cipherAlgorithm", this.m_cipherAlgorithm.ToString());
    writer.WriteAttributeString("cipherChaining", this.m_cipherChaining.ToString());
    writer.WriteAttributeString("hashAlgorithm", this.m_hashAlgorithm.ToString());
    writer.WriteAttributeString("saltValue", Convert.ToBase64String(this.m_saltValue));
    writer.WriteEndElement();
  }
}
