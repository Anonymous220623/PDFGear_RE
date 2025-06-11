// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Security.EncryptionVerifier
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Security;

[CLSCompliant(false)]
internal class EncryptionVerifier
{
  private byte[] m_arrSalt;
  private byte[] m_arrEncryptedVerifier = new byte[16 /*0x10*/];
  private byte[] m_arrEncryptedVerifierHash;
  private int m_iVerifierHashSize;
  private SecurityHelper m_securityHelper = new SecurityHelper();

  internal byte[] Salt
  {
    get => this.m_arrSalt;
    set => this.m_arrSalt = value;
  }

  internal byte[] EncryptedVerifier
  {
    get => this.m_arrEncryptedVerifier;
    set => this.m_arrEncryptedVerifier = value;
  }

  internal byte[] EncryptedVerifierHash
  {
    get => this.m_arrEncryptedVerifierHash;
    set => this.m_arrEncryptedVerifierHash = value;
  }

  internal int VerifierHashSize
  {
    get => this.m_iVerifierHashSize;
    set => this.m_iVerifierHashSize = value;
  }

  internal EncryptionVerifier()
  {
  }

  internal EncryptionVerifier(Stream stream) => this.Parse(stream);

  internal void Parse(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    byte[] buffer = new byte[4];
    int count1 = this.m_securityHelper.ReadInt32(stream, buffer);
    this.m_arrSalt = new byte[count1];
    stream.Read(this.m_arrSalt, 0, count1);
    stream.Read(this.m_arrEncryptedVerifier, 0, this.m_arrEncryptedVerifier.Length);
    this.m_iVerifierHashSize = this.m_securityHelper.ReadInt32(stream, buffer);
    int count2 = (int) (stream.Length - stream.Position);
    this.m_arrEncryptedVerifierHash = new byte[count2];
    stream.Read(this.m_arrEncryptedVerifierHash, 0, count2);
  }

  internal void Serialize(Stream stream)
  {
    int length1 = this.m_arrSalt.Length;
    this.m_securityHelper.WriteInt32(stream, length1);
    stream.Write(this.m_arrSalt, 0, length1);
    stream.Write(this.m_arrEncryptedVerifier, 0, this.m_arrEncryptedVerifier.Length);
    this.m_securityHelper.WriteInt32(stream, this.m_iVerifierHashSize);
    int length2 = this.m_arrEncryptedVerifierHash.Length;
    stream.Write(this.m_arrEncryptedVerifierHash, 0, length2);
  }
}
