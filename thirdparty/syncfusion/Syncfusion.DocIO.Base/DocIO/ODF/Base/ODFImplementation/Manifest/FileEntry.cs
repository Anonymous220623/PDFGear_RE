// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.Manifest.FileEntry
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation.Manifest;

internal class FileEntry
{
  private string m_path;
  private string m_mediaType;

  internal string Path
  {
    get => this.m_path;
    set => this.m_path = value;
  }

  internal string MediaType
  {
    get => this.m_mediaType;
    set => this.m_mediaType = value;
  }
}
