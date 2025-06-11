// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.IO.ArchiveInformation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;

#nullable disable
namespace Syncfusion.Pdf.IO;

internal class ArchiveInformation
{
  private long m_archiveNumber;
  private long m_index;
  private PdfStream m_archive;
  private GetArchive m_getArchive;

  public PdfStream Archive
  {
    get
    {
      if (this.m_archive == null)
        this.m_archive = this.m_getArchive(this.m_archiveNumber);
      return this.m_archive;
    }
  }

  public long Index => this.m_index;

  internal long ArchiveNumber => this.m_archiveNumber;

  public ArchiveInformation(long arcNum, long index, GetArchive getArchive)
  {
    this.m_archiveNumber = arcNum;
    this.m_index = index;
    this.m_getArchive = getArchive;
  }
}
