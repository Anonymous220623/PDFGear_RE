// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Security.KeyData
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Xml;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Security;

[CLSCompliant(false)]
internal class KeyData
{
  private int m_iSaltSize;
  private int m_iBlockSize;
  private int m_iKeyBits;
  private int m_iHashSize;
  private string m_sCipherAlgorithm;
  private string m_sCipherChaining;
  private string m_sHashAlgorithm;
  private byte[] m_arrSalt;

  internal int SaltSize
  {
    get => this.m_iSaltSize;
    set => this.m_iSaltSize = value;
  }

  internal int BlockSize
  {
    get => this.m_iBlockSize;
    set => this.m_iBlockSize = value;
  }

  internal int KeyBits
  {
    get => this.m_iKeyBits;
    set => this.m_iKeyBits = value;
  }

  internal int HashSize
  {
    get => this.m_iHashSize;
    set => this.m_iHashSize = value;
  }

  internal string CipherAlgorithm
  {
    get => this.m_sCipherAlgorithm;
    set => this.m_sCipherAlgorithm = value;
  }

  internal string CipherChaining
  {
    get => this.m_sCipherChaining;
    set => this.m_sCipherChaining = value;
  }

  internal string HashAlgorithm
  {
    get => this.m_sHashAlgorithm;
    set => this.m_sHashAlgorithm = value;
  }

  internal byte[] Salt
  {
    get => this.m_arrSalt;
    set => this.m_arrSalt = value;
  }

  internal KeyData()
  {
  }

  internal void Parse(XmlReader reader)
  {
    this.m_iSaltSize = int.Parse(reader.GetAttribute("saltSize"));
    this.m_iBlockSize = int.Parse(reader.GetAttribute("blockSize"));
    this.m_iKeyBits = int.Parse(reader.GetAttribute("keyBits"));
    this.m_iHashSize = int.Parse(reader.GetAttribute("hashSize"));
    this.m_sCipherAlgorithm = reader.GetAttribute("cipherAlgorithm");
    this.m_sCipherChaining = reader.GetAttribute("cipherChaining");
    this.m_sHashAlgorithm = reader.GetAttribute("hashAlgorithm");
    this.m_arrSalt = Convert.FromBase64String(reader.GetAttribute("saltValue"));
  }

  internal void Serialize(XmlWriter writer)
  {
    writer.WriteStartElement("keyData");
    writer.WriteAttributeString("saltSize", this.m_iSaltSize.ToString());
    writer.WriteAttributeString("blockSize", this.m_iBlockSize.ToString());
    writer.WriteAttributeString("keyBits", this.m_iKeyBits.ToString());
    writer.WriteAttributeString("hashSize", this.m_iHashSize.ToString());
    writer.WriteAttributeString("cipherAlgorithm", this.m_sCipherAlgorithm);
    writer.WriteAttributeString("cipherChaining", this.m_sCipherChaining);
    writer.WriteAttributeString("hashAlgorithm", this.m_sHashAlgorithm);
    writer.WriteAttributeString("saltValue", Convert.ToBase64String(this.m_arrSalt));
    writer.WriteEndElement();
  }
}
