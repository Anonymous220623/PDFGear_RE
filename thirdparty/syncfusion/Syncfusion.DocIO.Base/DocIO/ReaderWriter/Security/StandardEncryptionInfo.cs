// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Security.StandardEncryptionInfo
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Security;

[CLSCompliant(false)]
internal class StandardEncryptionInfo
{
  private int m_iVersionInfo;
  private int m_iFlags;
  private EncryptionHeader m_header = new EncryptionHeader();
  private EncryptionVerifier m_verifier = new EncryptionVerifier();
  private SecurityHelper m_securityHelper = new SecurityHelper();

  internal int VersionInfo
  {
    get => this.m_iVersionInfo;
    set => this.m_iVersionInfo = value;
  }

  internal int Flags
  {
    get => this.m_iFlags;
    set => this.m_iFlags = value;
  }

  internal EncryptionHeader Header => this.m_header;

  internal EncryptionVerifier Verifier => this.m_verifier;

  internal StandardEncryptionInfo()
  {
  }

  internal StandardEncryptionInfo(Stream stream)
  {
    byte[] buffer = new byte[4];
    this.m_iVersionInfo = this.m_securityHelper.ReadInt32(stream, buffer);
    this.m_iFlags = this.m_securityHelper.ReadInt32(stream, buffer);
    this.m_header.Parse(stream);
    this.m_verifier.Parse(stream);
  }

  internal void Serialize(Stream stream)
  {
    this.m_securityHelper.WriteInt32(stream, this.m_iVersionInfo);
    this.m_securityHelper.WriteInt32(stream, this.m_iFlags);
    this.m_header.Serialize(stream);
    this.m_verifier.Serialize(stream);
  }
}
