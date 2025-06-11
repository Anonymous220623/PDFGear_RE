// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfDocumentLinkAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfDocumentLinkAnnotation : PdfLinkAnnotation
{
  private PdfDestination m_destination;

  public PdfDestination Destination
  {
    get => this.m_destination;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Destination));
      if (this.m_destination == value)
        return;
      this.m_destination = value;
    }
  }

  public PdfDocumentLinkAnnotation(RectangleF rectangle)
    : base(rectangle)
  {
  }

  public PdfDocumentLinkAnnotation(RectangleF rectangle, PdfDestination destination)
    : base(rectangle)
  {
    this.Destination = destination != null ? destination : throw new ArgumentNullException(nameof (destination));
  }

  protected override void Save()
  {
    base.Save();
    if (this.m_destination == null)
      return;
    this.Dictionary.SetProperty("Dest", (IPdfWrapper) this.m_destination);
  }
}
