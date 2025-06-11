// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfFileStructure
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfFileStructure
{
  private PdfVersion m_version;
  private PdfCrossReferenceType m_crossReferenceType;
  private PdfFileFormat m_fileformat;
  private bool m_incrementalUpdate;
  private bool m_taggedPdf;
  internal bool m_fileID;

  internal event EventHandler TaggedPdfChanged;

  public PdfFileStructure()
  {
    this.m_version = PdfVersion.Version1_5;
    this.m_crossReferenceType = PdfCrossReferenceType.CrossReferenceStream;
    this.m_fileformat = PdfFileFormat.Plain;
    this.m_incrementalUpdate = true;
    this.m_taggedPdf = false;
  }

  public PdfVersion Version
  {
    get => this.m_version;
    set
    {
      this.m_version = value;
      if (this.m_version > PdfVersion.Version1_3)
        return;
      this.m_crossReferenceType = PdfCrossReferenceType.CrossReferenceTable;
    }
  }

  public bool IncrementalUpdate
  {
    get => this.m_incrementalUpdate;
    set => this.m_incrementalUpdate = value;
  }

  public bool EnableTrailerId
  {
    get => this.m_fileID;
    set => this.m_fileID = value;
  }

  public PdfCrossReferenceType CrossReferenceType
  {
    get => this.m_crossReferenceType;
    set => this.m_crossReferenceType = value;
  }

  internal PdfFileFormat FileFormat
  {
    get => this.m_fileformat;
    set => this.m_fileformat = value;
  }

  public bool TaggedPdf
  {
    get => this.m_taggedPdf;
    internal set
    {
      if (this.m_taggedPdf != value)
        this.m_taggedPdf = value;
      this.OnTaggedPdfChanged(new EventArgs());
    }
  }

  protected void OnTaggedPdfChanged(EventArgs e)
  {
    EventHandler taggedPdfChanged = this.TaggedPdfChanged;
    if (taggedPdfChanged == null)
      return;
    taggedPdfChanged((object) this, e);
  }
}
