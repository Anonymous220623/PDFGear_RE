// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Security.EncryptionTransformInfo
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Security;

internal class EncryptionTransformInfo
{
  private string m_strName;
  private int m_iBlockSize = 16 /*0x10*/;
  private int m_iCipherMode;
  private int m_iReserved = 4;

  public string Name
  {
    get => this.m_strName;
    set => this.m_strName = value;
  }

  public int BlockSize
  {
    get => this.m_iBlockSize;
    set => this.m_iBlockSize = value;
  }

  public int CipherMode => this.m_iCipherMode;

  public int Reserved => this.m_iReserved;

  public EncryptionTransformInfo()
  {
  }

  public EncryptionTransformInfo(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    byte[] buffer = new byte[4];
    this.m_strName = SecurityHelper.ReadUnicodeStringP4(stream);
    this.m_iBlockSize = SecurityHelper.ReadInt32(stream, buffer);
    this.m_iCipherMode = SecurityHelper.ReadInt32(stream, buffer);
    this.m_iReserved = SecurityHelper.ReadInt32(stream, buffer);
  }

  public void Serialize(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    SecurityHelper.WriteUnicodeStringP4(stream, this.m_strName);
    SecurityHelper.WriteInt32(stream, this.m_iBlockSize);
    SecurityHelper.WriteInt32(stream, this.m_iCipherMode);
    SecurityHelper.WriteInt32(stream, this.m_iReserved);
  }
}
