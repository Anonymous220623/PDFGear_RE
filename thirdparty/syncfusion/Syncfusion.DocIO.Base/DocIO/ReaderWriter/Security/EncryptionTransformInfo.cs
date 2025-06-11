// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Security.EncryptionTransformInfo
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Security;

[CLSCompliant(false)]
internal class EncryptionTransformInfo
{
  private string m_strName;
  private int m_iBlockSize;
  private int m_iCipherMode;
  private int m_iReserved = 4;
  private SecurityHelper m_securityHelper = new SecurityHelper();

  internal string Name
  {
    get => this.m_strName;
    set => this.m_strName = value;
  }

  internal int BlockSize
  {
    get => this.m_iBlockSize;
    set => this.m_iBlockSize = value;
  }

  internal int CipherMode => this.m_iCipherMode;

  internal int Reserved => this.m_iReserved;

  internal EncryptionTransformInfo()
  {
  }

  internal EncryptionTransformInfo(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    byte[] buffer = new byte[4];
    this.m_strName = this.m_securityHelper.ReadUnicodeString(stream);
    this.m_iBlockSize = this.m_securityHelper.ReadInt32(stream, buffer);
    this.m_iCipherMode = this.m_securityHelper.ReadInt32(stream, buffer);
    this.m_iReserved = this.m_securityHelper.ReadInt32(stream, buffer);
  }

  internal void Serialize(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    this.m_securityHelper.WriteUnicodeString(stream, this.m_strName);
    this.m_securityHelper.WriteInt32(stream, this.m_iBlockSize);
    this.m_securityHelper.WriteInt32(stream, this.m_iCipherMode);
    this.m_securityHelper.WriteInt32(stream, this.m_iReserved);
  }
}
