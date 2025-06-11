// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfGoToAction
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfGoToAction : PdfAction
{
  private PdfDestination m_destination;

  public PdfGoToAction(PdfDestination destination)
  {
    this.m_destination = destination != null ? destination : throw new ArgumentNullException(nameof (destination));
  }

  public PdfGoToAction(PdfPage page)
  {
    this.m_destination = page != null ? new PdfDestination((PdfPageBase) page) : throw new ArgumentNullException(nameof (page));
  }

  public PdfDestination Destination
  {
    get => this.m_destination;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Destination));
      if (value == this.m_destination)
        return;
      this.m_destination = value;
    }
  }

  protected override void Initialize()
  {
    base.Initialize();
    this.Dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
    this.Dictionary.SetProperty("S", (IPdfPrimitive) new PdfName("GoTo"));
  }

  private void Dictionary_BeginSave(object sender, SavePdfPrimitiveEventArgs ars)
  {
    this.Dictionary.SetProperty("D", (IPdfWrapper) this.m_destination);
  }
}
