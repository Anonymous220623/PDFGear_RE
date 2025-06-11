// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Security.EncryptedKey
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;
using System.Xml;

#nullable disable
namespace Syncfusion.Presentation.Security;

[CLSCompliant(false)]
internal class EncryptedKey
{
  private int m_iSpinCount;
  private int m_iSaltSize;
  private int m_iBlockSize;
  private int m_iKeyBits;
  private int m_iHashSize;
  private string m_sCipherAlgorithm;
  private string m_sCipherChaining;
  private string m_sHashAlgorithm;
  private byte[] m_arrSalt;
  private byte[] m_encryptedVerifierHashInput;
  private byte[] m_encryptedVerifierHashValue;
  private byte[] m_encryptedKeyValue;

  internal int SpinCount
  {
    get => this.m_iSpinCount;
    set => this.m_iSpinCount = value;
  }

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

  internal byte[] EncryptedVerifierHashInput
  {
    get => this.m_encryptedVerifierHashInput;
    set => this.m_encryptedVerifierHashInput = value;
  }

  internal byte[] EncryptedVerifierHashValue
  {
    get => this.m_encryptedVerifierHashValue;
    set => this.m_encryptedVerifierHashValue = value;
  }

  internal byte[] EncryptedKeyValue
  {
    get => this.m_encryptedKeyValue;
    set => this.m_encryptedKeyValue = value;
  }

  internal EncryptedKey()
  {
  }

  internal void Parse(XmlReader reader)
  {
    this.m_iSpinCount = int.Parse(reader.GetAttribute("spinCount"));
    this.m_iSaltSize = int.Parse(reader.GetAttribute("saltSize"));
    this.m_iBlockSize = int.Parse(reader.GetAttribute("blockSize"));
    this.m_iKeyBits = int.Parse(reader.GetAttribute("keyBits"));
    this.m_iHashSize = int.Parse(reader.GetAttribute("hashSize"));
    this.m_sCipherAlgorithm = reader.GetAttribute("cipherAlgorithm");
    this.m_sCipherChaining = reader.GetAttribute("cipherChaining");
    this.m_sHashAlgorithm = reader.GetAttribute("hashAlgorithm");
    this.m_arrSalt = Convert.FromBase64String(reader.GetAttribute("saltValue"));
    this.m_encryptedVerifierHashInput = Convert.FromBase64String(reader.GetAttribute("encryptedVerifierHashInput"));
    this.m_encryptedVerifierHashValue = Convert.FromBase64String(reader.GetAttribute("encryptedVerifierHashValue"));
    this.m_encryptedKeyValue = Convert.FromBase64String(reader.GetAttribute("encryptedKeyValue"));
  }

  internal void Serialize(XmlWriter writer)
  {
    writer.WriteStartElement("p", "encryptedKey", "http://schemas.microsoft.com/office/2006/keyEncryptor/password");
    writer.WriteAttributeString("spinCount", this.m_iSpinCount.ToString());
    writer.WriteAttributeString("saltSize", this.m_iSaltSize.ToString());
    writer.WriteAttributeString("blockSize", this.m_iBlockSize.ToString());
    writer.WriteAttributeString("keyBits", this.m_iKeyBits.ToString());
    writer.WriteAttributeString("hashSize", this.m_iHashSize.ToString());
    writer.WriteAttributeString("cipherAlgorithm", this.m_sCipherAlgorithm);
    writer.WriteAttributeString("cipherChaining", this.m_sCipherChaining);
    writer.WriteAttributeString("hashAlgorithm", this.m_sHashAlgorithm);
    writer.WriteAttributeString("saltValue", Convert.ToBase64String(this.m_arrSalt));
    writer.WriteAttributeString("encryptedVerifierHashInput", Convert.ToBase64String(this.m_encryptedVerifierHashInput));
    writer.WriteAttributeString("encryptedVerifierHashValue", Convert.ToBase64String(this.m_encryptedVerifierHashValue));
    writer.WriteAttributeString("encryptedKeyValue", Convert.ToBase64String(this.m_encryptedKeyValue));
    writer.WriteEndElement();
  }
}
