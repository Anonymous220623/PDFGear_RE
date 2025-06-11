// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Security.EncryptionHeader
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Security;

[CLSCompliant(false)]
internal class EncryptionHeader
{
  private int m_iFlags;
  private int m_iSizeExtra;
  private int m_iAlgorithmId;
  private int m_iAlgorithmIdHash;
  private int m_iKeySize;
  private int m_iProviderType;
  private int m_iReserved1;
  private int m_iReserved2;
  private string m_strCSPName;
  private SecurityHelper m_securityHelper = new SecurityHelper();

  internal int Flags
  {
    get => this.m_iFlags;
    set => this.m_iFlags = value;
  }

  internal int SizeExtra
  {
    get => this.m_iSizeExtra;
    set => this.m_iSizeExtra = value;
  }

  internal int AlgorithmId
  {
    get => this.m_iAlgorithmId;
    set => this.m_iAlgorithmId = value;
  }

  internal int AlgorithmIdHash
  {
    get => this.m_iAlgorithmIdHash;
    set => this.m_iAlgorithmIdHash = value;
  }

  internal int KeySize
  {
    get => this.m_iKeySize;
    set => this.m_iKeySize = value;
  }

  internal int ProviderType
  {
    get => this.m_iProviderType;
    set => this.m_iProviderType = value;
  }

  internal int Reserved1
  {
    get => this.m_iReserved1;
    set => this.m_iReserved1 = value;
  }

  internal int Reserved2
  {
    get => this.m_iReserved2;
    set => this.m_iReserved2 = value;
  }

  internal string CSPName
  {
    get => this.m_strCSPName;
    set
    {
      this.m_strCSPName = value != null && value.Length != 0 ? value : throw new ArgumentOutOfRangeException();
    }
  }

  internal EncryptionHeader()
  {
  }

  internal EncryptionHeader(Stream stream) => this.Parse(stream);

  internal void Parse(Stream stream)
  {
    byte[] buffer = new byte[4];
    long position = stream.Position;
    int num = this.m_securityHelper.ReadInt32(stream, buffer);
    this.m_iFlags = this.m_securityHelper.ReadInt32(stream, buffer);
    this.m_iSizeExtra = this.m_securityHelper.ReadInt32(stream, buffer);
    this.m_iAlgorithmId = this.m_securityHelper.ReadInt32(stream, buffer);
    this.m_iAlgorithmIdHash = this.m_securityHelper.ReadInt32(stream, buffer);
    this.m_iKeySize = this.m_securityHelper.ReadInt32(stream, buffer);
    this.m_iProviderType = this.m_securityHelper.ReadInt32(stream, buffer);
    this.m_iReserved1 = this.m_securityHelper.ReadInt32(stream, buffer);
    this.m_iReserved2 = this.m_securityHelper.ReadInt32(stream, buffer);
    this.m_strCSPName = this.m_securityHelper.ReadUnicodeStringZero(stream);
    stream.Position = position + (long) num + 4L;
  }

  internal void Serialize(Stream stream)
  {
    long position1 = stream.Position;
    stream.Position += 4L;
    this.m_securityHelper.WriteInt32(stream, this.m_iFlags);
    this.m_securityHelper.WriteInt32(stream, this.m_iSizeExtra);
    this.m_securityHelper.WriteInt32(stream, this.m_iAlgorithmId);
    this.m_securityHelper.WriteInt32(stream, this.m_iAlgorithmIdHash);
    this.m_securityHelper.WriteInt32(stream, this.m_iKeySize);
    this.m_securityHelper.WriteInt32(stream, this.m_iProviderType);
    this.m_securityHelper.WriteInt32(stream, this.m_iReserved1);
    this.m_securityHelper.WriteInt32(stream, this.m_iReserved2);
    this.m_securityHelper.WriteUnicodeStringZero(stream, this.m_strCSPName);
    long position2 = stream.Position;
    int num = (int) (position2 - position1) - 4;
    stream.Position = position1;
    this.m_securityHelper.WriteInt32(stream, num);
    stream.Position = position2;
  }
}
