// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Security.VersionInfo
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Security;

[CLSCompliant(false)]
internal class VersionInfo
{
  private string m_strFeatureId = "Microsoft.Container.DataSpaces";
  private int m_iReaderVersion = 1;
  private int m_iUpdaterVersion = 1;
  private int m_iWriterVersion = 1;
  private SecurityHelper m_securityHelper = new SecurityHelper();

  internal string FeatureId
  {
    get => this.m_strFeatureId;
    set => this.m_strFeatureId = value;
  }

  internal int ReaderVersion
  {
    get => this.m_iReaderVersion;
    set => this.m_iReaderVersion = value;
  }

  internal int UpdaterVersion
  {
    get => this.m_iUpdaterVersion;
    set => this.m_iUpdaterVersion = value;
  }

  internal int WriterVersion
  {
    get => this.m_iWriterVersion;
    set => this.m_iWriterVersion = value;
  }

  internal VersionInfo()
  {
  }

  internal void Serialize(Stream stream)
  {
    this.m_securityHelper.WriteUnicodeString(stream, this.m_strFeatureId);
    this.m_securityHelper.WriteInt32(stream, this.m_iReaderVersion);
    this.m_securityHelper.WriteInt32(stream, this.m_iUpdaterVersion);
    this.m_securityHelper.WriteInt32(stream, this.m_iWriterVersion);
  }
}
