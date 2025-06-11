// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Security.EncryptionHeader
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Security;

public class EncryptionHeader
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

  public int Flags
  {
    get => this.m_iFlags;
    set => this.m_iFlags = value;
  }

  public int SizeExtra
  {
    get => this.m_iSizeExtra;
    set => this.m_iSizeExtra = value;
  }

  public int AlgorithmId
  {
    get => this.m_iAlgorithmId;
    set => this.m_iAlgorithmId = value;
  }

  public int AlgorithmIdHash
  {
    get => this.m_iAlgorithmIdHash;
    set => this.m_iAlgorithmIdHash = value;
  }

  public int KeySize
  {
    get => this.m_iKeySize;
    set => this.m_iKeySize = value;
  }

  public int ProviderType
  {
    get => this.m_iProviderType;
    set => this.m_iProviderType = value;
  }

  public int Reserved1
  {
    get => this.m_iReserved1;
    set => this.m_iReserved1 = value;
  }

  public int Reserved2
  {
    get => this.m_iReserved2;
    set => this.m_iReserved2 = value;
  }

  public string CSPName
  {
    get => this.m_strCSPName;
    set
    {
      this.m_strCSPName = value != null && value.Length != 0 ? value : throw new ArgumentOutOfRangeException();
    }
  }

  public EncryptionHeader()
  {
  }

  public EncryptionHeader(Stream stream) => this.Parse(stream);

  public void Parse(Stream stream)
  {
    byte[] buffer = new byte[4];
    long position = stream.Position;
    int num = SecurityHelper.ReadInt32(stream, buffer);
    this.m_iFlags = SecurityHelper.ReadInt32(stream, buffer);
    this.m_iSizeExtra = SecurityHelper.ReadInt32(stream, buffer);
    this.m_iAlgorithmId = SecurityHelper.ReadInt32(stream, buffer);
    this.m_iAlgorithmIdHash = SecurityHelper.ReadInt32(stream, buffer);
    this.m_iKeySize = SecurityHelper.ReadInt32(stream, buffer);
    this.m_iProviderType = SecurityHelper.ReadInt32(stream, buffer);
    this.m_iReserved1 = SecurityHelper.ReadInt32(stream, buffer);
    this.m_iReserved2 = SecurityHelper.ReadInt32(stream, buffer);
    this.m_strCSPName = SecurityHelper.ReadUnicodeStringZero(stream);
    stream.Position = position + (long) num + 4L;
  }

  public void Serialize(Stream stream)
  {
    long position1 = stream.Position;
    stream.Position += 4L;
    SecurityHelper.WriteInt32(stream, this.m_iFlags);
    SecurityHelper.WriteInt32(stream, this.m_iSizeExtra);
    SecurityHelper.WriteInt32(stream, this.m_iAlgorithmId);
    SecurityHelper.WriteInt32(stream, this.m_iAlgorithmIdHash);
    SecurityHelper.WriteInt32(stream, this.m_iKeySize);
    SecurityHelper.WriteInt32(stream, this.m_iProviderType);
    SecurityHelper.WriteInt32(stream, this.m_iReserved1);
    SecurityHelper.WriteInt32(stream, this.m_iReserved2);
    SecurityHelper.WriteUnicodeStringZero(stream, this.m_strCSPName);
    long position2 = stream.Position;
    int num = (int) (position2 - position1) - 4;
    stream.Position = position1;
    SecurityHelper.WriteInt32(stream, num);
    stream.Position = position2;
  }
}
