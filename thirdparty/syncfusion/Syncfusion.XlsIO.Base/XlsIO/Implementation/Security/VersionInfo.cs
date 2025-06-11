// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Security.VersionInfo
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Security;

internal class VersionInfo
{
  private string m_strFeatureId = "Microsoft.Container.DataSpaces";
  private int m_iReaderVersion = 1;
  private int m_iUpdaterVersion = 1;
  private int m_iWriterVersion = 1;

  public string FeatureId
  {
    get => this.m_strFeatureId;
    set => this.m_strFeatureId = value;
  }

  public int ReaderVersion
  {
    get => this.m_iReaderVersion;
    set => this.m_iReaderVersion = value;
  }

  public int UpdaterVersion
  {
    get => this.m_iUpdaterVersion;
    set => this.m_iUpdaterVersion = value;
  }

  public int WriterVersion
  {
    get => this.m_iWriterVersion;
    set => this.m_iWriterVersion = value;
  }

  public void Serialize(Stream stream)
  {
    SecurityHelper.WriteUnicodeStringP4(stream, this.m_strFeatureId);
    SecurityHelper.WriteInt32(stream, this.m_iReaderVersion);
    SecurityHelper.WriteInt32(stream, this.m_iUpdaterVersion);
    SecurityHelper.WriteInt32(stream, this.m_iWriterVersion);
  }
}
