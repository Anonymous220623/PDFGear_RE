// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Security.EncryptionVerifier
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Security;

public class EncryptionVerifier
{
  private byte[] m_arrSalt;
  private byte[] m_arrEncryptedVerifier = new byte[16 /*0x10*/];
  private byte[] m_arrEncryptedVerifierHash;
  private int m_iVerifierHashSize;

  public byte[] Salt
  {
    get => this.m_arrSalt;
    set => this.m_arrSalt = value;
  }

  public byte[] EncryptedVerifier
  {
    get => this.m_arrEncryptedVerifier;
    set => this.m_arrEncryptedVerifier = value;
  }

  public byte[] EncryptedVerifierHash
  {
    get => this.m_arrEncryptedVerifierHash;
    set => this.m_arrEncryptedVerifierHash = value;
  }

  public int VerifierHashSize
  {
    get => this.m_iVerifierHashSize;
    set => this.m_iVerifierHashSize = value;
  }

  public EncryptionVerifier()
  {
  }

  public EncryptionVerifier(Stream stream) => this.Parse(stream);

  public void Parse(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    byte[] buffer = new byte[4];
    int count1 = SecurityHelper.ReadInt32(stream, buffer);
    this.m_arrSalt = new byte[count1];
    stream.Read(this.m_arrSalt, 0, count1);
    stream.Read(this.m_arrEncryptedVerifier, 0, this.m_arrEncryptedVerifier.Length);
    this.m_iVerifierHashSize = SecurityHelper.ReadInt32(stream, buffer);
    int count2 = (int) (stream.Length - stream.Position);
    this.m_arrEncryptedVerifierHash = new byte[count2];
    stream.Read(this.m_arrEncryptedVerifierHash, 0, count2);
  }

  public void Serialize(Stream stream)
  {
    int length1 = this.m_arrSalt.Length;
    SecurityHelper.WriteInt32(stream, length1);
    stream.Write(this.m_arrSalt, 0, length1);
    stream.Write(this.m_arrEncryptedVerifier, 0, this.m_arrEncryptedVerifier.Length);
    SecurityHelper.WriteInt32(stream, this.m_iVerifierHashSize);
    int length2 = this.m_arrEncryptedVerifierHash.Length;
    stream.Write(this.m_arrEncryptedVerifierHash, 0, length2);
  }
}
