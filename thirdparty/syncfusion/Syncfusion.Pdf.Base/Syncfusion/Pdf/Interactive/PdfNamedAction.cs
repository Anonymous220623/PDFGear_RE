// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfNamedAction
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfNamedAction : PdfAction
{
  private PdfActionDestination m_destination = PdfActionDestination.NextPage;

  public PdfActionDestination Destination
  {
    get => this.m_destination;
    set
    {
      if (this.m_destination == value)
        return;
      this.m_destination = value;
      this.Dictionary.SetName("N", this.m_destination.ToString());
    }
  }

  public PdfNamedAction(PdfActionDestination destination) => this.Destination = destination;

  protected override void Initialize()
  {
    base.Initialize();
    this.Dictionary.SetProperty("S", (IPdfPrimitive) new PdfName("Named"));
    this.Dictionary.SetProperty("N", (IPdfPrimitive) new PdfName(this.m_destination.ToString()));
  }
}
