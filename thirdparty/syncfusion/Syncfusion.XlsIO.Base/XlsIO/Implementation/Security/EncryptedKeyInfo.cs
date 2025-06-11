// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Security.EncryptedKeyInfo
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Security;

public class EncryptedKeyInfo
{
  internal const int DefaultSpinCount = 100000;
  internal const int DefaultHashSize = 20;
  internal const string DefaultCipherAlgorithm = "AES";
  internal const string DefaultCipherChaining = "ChainingModeCBC";
  internal const string DefaultHashAlgorithm = "SHA1";
  internal const int DefaultKeyBits = 128 /*0x80*/;
  internal const string AdvancedHashAlgorithm = "SHA512";
  private int m_spintCount;
  private int m_saltSize;
  private int m_blockSize;
  private int m_keyBits;
  private int m_hasSize;
  private string m_cipherAlgorithm;
  private string m_cipherChaining;
  private string m_hashAlgorithm;
  private byte[] m_saltValue;
  private byte[] m_verifierHashInput;
  private byte[] m_verifierHashValue;
  private byte[] m_encryptedKeyValue;

  internal int HashSize
  {
    get => this.m_hasSize;
    set => this.m_hasSize = value;
  }

  internal int SpinCount
  {
    get => this.m_spintCount;
    set => this.m_spintCount = value;
  }

  internal int BlockSize
  {
    get => this.m_blockSize;
    set => this.m_blockSize = value;
  }

  internal int KeyBits
  {
    get => this.m_keyBits / 8;
    set => this.m_keyBits = value;
  }

  internal byte[] SaltValue
  {
    get => this.m_saltValue;
    set => this.m_saltValue = value;
  }

  internal byte[] VerifierHashValue
  {
    get => this.m_verifierHashValue;
    set => this.m_verifierHashValue = value;
  }

  internal byte[] VerifierHashInput
  {
    get => this.m_verifierHashInput;
    set => this.m_verifierHashInput = value;
  }

  internal byte[] KeyValue
  {
    get => this.m_encryptedKeyValue;
    set => this.m_encryptedKeyValue = value;
  }

  internal string HashAlgorithm
  {
    get => this.m_hashAlgorithm;
    set => this.m_hashAlgorithm = value;
  }

  internal EncryptedKeyInfo()
  {
    this.m_spintCount = 100000;
    this.m_saltSize = 16 /*0x10*/;
    this.m_blockSize = 16 /*0x10*/;
    this.m_hasSize = 20;
    this.m_keyBits = 128 /*0x80*/;
    this.m_cipherAlgorithm = "AES";
    this.m_cipherChaining = "ChainingModeCBC";
    this.m_hashAlgorithm = "SHA1";
  }

  internal void Parse(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "encryptedKey")
      throw new XmlException();
    if (reader.MoveToAttribute("spinCount"))
      this.m_spintCount = Convert.ToInt32(reader.Value);
    if (reader.MoveToAttribute("saltSize"))
      this.m_saltSize = Convert.ToInt32(reader.Value);
    if (reader.MoveToAttribute("blockSize"))
      this.m_blockSize = Convert.ToInt32(reader.Value);
    if (reader.MoveToAttribute("keyBits"))
      this.m_keyBits = Convert.ToInt32(reader.Value);
    if (reader.MoveToAttribute("hashSize"))
      this.m_hasSize = Convert.ToInt32(reader.Value);
    if (reader.MoveToAttribute("cipherAlgorithm"))
      this.m_cipherAlgorithm = reader.Value;
    if (reader.MoveToAttribute("cipherChaining"))
      this.m_cipherChaining = reader.Value;
    if (reader.MoveToAttribute("hashAlgorithm"))
      this.m_hashAlgorithm = reader.Value;
    if (reader.MoveToAttribute("saltValue"))
      this.m_saltValue = Convert.FromBase64String(reader.Value);
    if (reader.MoveToAttribute("encryptedVerifierHashInput"))
      this.m_verifierHashInput = Convert.FromBase64String(reader.Value);
    if (reader.MoveToAttribute("encryptedVerifierHashValue"))
      this.m_verifierHashValue = Convert.FromBase64String(reader.Value);
    if (!reader.MoveToAttribute("encryptedKeyValue"))
      return;
    this.m_encryptedKeyValue = Convert.FromBase64String(reader.Value);
  }

  internal void Serialize(XmlWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartElement("keyEncryptor");
    writer.WriteAttributeString("uri", "http://schemas.microsoft.com/office/2006/keyEncryptor/password");
    writer.WriteStartElement("p", "encryptedKey", "http://schemas.microsoft.com/office/2006/keyEncryptor/password");
    writer.WriteAttributeString("spinCount", this.m_spintCount.ToString());
    writer.WriteAttributeString("saltSize", this.m_saltSize.ToString());
    writer.WriteAttributeString("blockSize", this.m_blockSize.ToString());
    writer.WriteAttributeString("keyBits", this.m_keyBits.ToString());
    writer.WriteAttributeString("hashSize", this.m_hasSize.ToString());
    writer.WriteAttributeString("cipherAlgorithm", this.m_cipherAlgorithm.ToString());
    writer.WriteAttributeString("cipherChaining", this.m_cipherChaining.ToString());
    writer.WriteAttributeString("hashAlgorithm", this.m_hashAlgorithm.ToString());
    writer.WriteAttributeString("saltValue", Convert.ToBase64String(this.m_saltValue));
    writer.WriteAttributeString("encryptedVerifierHashInput", Convert.ToBase64String(this.m_verifierHashInput));
    writer.WriteAttributeString("encryptedVerifierHashValue", Convert.ToBase64String(this.m_verifierHashValue));
    writer.WriteAttributeString("encryptedKeyValue", Convert.ToBase64String(this.m_encryptedKeyValue));
    writer.WriteEndElement();
    writer.WriteEndElement();
  }
}
