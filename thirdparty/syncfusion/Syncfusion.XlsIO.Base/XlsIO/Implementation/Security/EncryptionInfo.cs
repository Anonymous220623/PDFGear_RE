// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Security.EncryptionInfo
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Security;

public class EncryptionInfo
{
  private int m_iVersionInfo;
  private int m_iFlags;
  private EncryptionHeader m_header = new EncryptionHeader();
  private EncryptionVerifier m_verifier = new EncryptionVerifier();
  private EncryptedKeyInfo m_keyInfo;
  private DataIntegrityInfo m_dataIntegrity;
  private DataEncryptionInfo m_dataEncryption;

  internal EncryptedKeyInfo KeyInfo
  {
    get => this.m_keyInfo;
    set => this.m_keyInfo = value;
  }

  internal DataIntegrityInfo DataIntegrity
  {
    get => this.m_dataIntegrity;
    set => this.m_dataIntegrity = value;
  }

  internal DataEncryptionInfo DataEncryption
  {
    get => this.m_dataEncryption;
    set => this.m_dataEncryption = value;
  }

  public int VersionInfo
  {
    get => this.m_iVersionInfo;
    set => this.m_iVersionInfo = value;
  }

  public int Flags
  {
    get => this.m_iFlags;
    set => this.m_iFlags = value;
  }

  public EncryptionHeader Header => this.m_header;

  public EncryptionVerifier Verifier => this.m_verifier;

  public EncryptionInfo()
  {
    this.m_keyInfo = new EncryptedKeyInfo();
    this.m_dataIntegrity = new DataIntegrityInfo();
    this.m_dataEncryption = new DataEncryptionInfo();
  }

  public EncryptionInfo(Stream stream)
  {
    byte[] buffer = new byte[4];
    this.m_iVersionInfo = SecurityHelper.ReadInt32(stream, buffer);
    this.m_iFlags = SecurityHelper.ReadInt32(stream, buffer);
    if (this.m_iVersionInfo == 262148 /*0x040004*/)
    {
      XmlReader fromStreamPosition = UtilityMethods.CreateReaderFromStreamPosition(stream);
      fromStreamPosition.Read();
      this.m_dataEncryption = new DataEncryptionInfo();
      this.m_dataEncryption.Parse(fromStreamPosition);
      fromStreamPosition.Read();
      this.m_dataIntegrity = new DataIntegrityInfo();
      this.m_dataIntegrity.Parse(fromStreamPosition);
      fromStreamPosition.Read();
      fromStreamPosition.Read();
      fromStreamPosition.Read();
      this.m_keyInfo = new EncryptedKeyInfo();
      this.m_keyInfo.Parse(fromStreamPosition);
    }
    else
    {
      this.m_header.Parse(stream);
      this.m_verifier.Parse(stream);
    }
  }

  public void Serialize(Stream stream)
  {
    SecurityHelper.WriteInt32(stream, this.m_iVersionInfo);
    SecurityHelper.WriteInt32(stream, this.m_iFlags);
    if (this.m_iVersionInfo == 262148 /*0x040004*/)
    {
      MemoryStream data = new MemoryStream();
      XmlWriter writer = UtilityMethods.CreateWriter((Stream) data, Encoding.UTF8);
      writer.WriteStartDocument();
      writer.WriteStartElement("encryption", "http://schemas.microsoft.com/office/2006/encryption");
      writer.WriteAttributeString("xmlns", "p", (string) null, "http://schemas.microsoft.com/office/2006/keyEncryptor/password");
      this.m_dataEncryption.Serialize(writer);
      if (this.m_dataIntegrity != null)
        this.m_dataIntegrity.Serialize(writer);
      writer.WriteStartElement("keyEncryptors");
      this.m_keyInfo.Serialize(writer);
      writer.WriteEndElement();
      writer.WriteEndDocument();
      writer.Flush();
      data.Position = 0L;
      byte[] buffer = new byte[data.Length];
      data.Read(buffer, 0, buffer.Length);
      data.Close();
      stream.Write(buffer, 0, buffer.Length);
    }
    else
    {
      this.m_header.Serialize(stream);
      this.m_verifier.Serialize(stream);
    }
  }
}
