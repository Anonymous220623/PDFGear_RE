// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfAttachmentAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfAttachmentAnnotation : PdfFileAnnotation
{
  private PdfAttachmentIcon m_attachmentIcon;
  private PdfEmbeddedFileSpecification m_fileSpecification;

  public PdfAttachmentIcon Icon
  {
    get => this.m_attachmentIcon;
    set
    {
      this.m_attachmentIcon = value;
      this.Dictionary.SetName("Name", this.m_attachmentIcon.ToString());
    }
  }

  public override string FileName
  {
    get => this.m_fileSpecification.FileName;
    set
    {
      switch (value)
      {
        case null:
          throw new ArgumentNullException(nameof (FileName));
        case "":
          throw new ArgumentException("FileName can't be empty");
        default:
          if (!(this.m_fileSpecification.FileName != value))
            break;
          this.m_fileSpecification.FileName = value;
          break;
      }
    }
  }

  public PdfPopupAnnotationCollection ReviewHistory
  {
    get
    {
      return this.m_reviewHistory != null ? this.m_reviewHistory : (this.m_reviewHistory = new PdfPopupAnnotationCollection((PdfAnnotation) this, true));
    }
  }

  public PdfPopupAnnotationCollection Comments
  {
    get
    {
      return this.m_comments != null ? this.m_comments : (this.m_comments = new PdfPopupAnnotationCollection((PdfAnnotation) this, false));
    }
  }

  public PdfAttachmentAnnotation(RectangleF rectangle, string fileName)
    : base(rectangle)
  {
    this.m_fileSpecification = fileName != null ? new PdfEmbeddedFileSpecification(fileName) : throw new ArgumentNullException(nameof (fileName));
  }

  public PdfAttachmentAnnotation(RectangleF rectangle, string fileName, byte[] data)
    : base(rectangle)
  {
    if (fileName == null)
      throw new ArgumentNullException(nameof (fileName));
    this.m_fileSpecification = data != null ? new PdfEmbeddedFileSpecification(fileName, data) : throw new ArgumentNullException(nameof (data));
  }

  public PdfAttachmentAnnotation(RectangleF rectangle, string fileName, Stream stream)
    : base(rectangle)
  {
    if (fileName == null)
      throw new ArgumentNullException(nameof (fileName));
    this.m_fileSpecification = stream != null ? new PdfEmbeddedFileSpecification(fileName, stream) : throw new ArgumentNullException(nameof (stream));
  }

  protected override void Initialize()
  {
    base.Initialize();
    this.Dictionary.SetProperty("Subtype", (IPdfPrimitive) new PdfName("FileAttachment"));
  }

  protected override void Save()
  {
    base.Save();
    this.Dictionary.SetProperty("FS", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_fileSpecification));
  }
}
