// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.Manifest.Manifest
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation.Manifest;

internal class Manifest
{
  private List<FileEntry> m_files;

  internal List<FileEntry> Files
  {
    get
    {
      if (this.m_files == null)
        this.m_files = new List<FileEntry>();
      return this.m_files;
    }
    set => this.m_files = value;
  }
}
