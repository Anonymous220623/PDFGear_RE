// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfPortfolioInformation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.Primitives;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfPortfolioInformation : IPdfWrapper
{
  private PdfCatalog m_catalog;
  private PdfDictionary m_dictionary = new PdfDictionary();
  private PdfPortfolioSchema m_Schema;
  private PdfPortfolioViewMode m_viewMode;
  private PdfAttachment m_startupDocument;

  public PdfPortfolioSchema Schema
  {
    get => this.m_Schema;
    set
    {
      this.m_Schema = value;
      this.m_dictionary.SetProperty(nameof (Schema), (IPdfWrapper) this.m_Schema);
    }
  }

  public PdfPortfolioViewMode ViewMode
  {
    get => this.m_viewMode;
    set
    {
      this.m_viewMode = value;
      if (this.m_viewMode == PdfPortfolioViewMode.Details)
        this.m_dictionary.SetProperty("View", (IPdfPrimitive) new PdfName("D"));
      else if (this.m_viewMode == PdfPortfolioViewMode.Hidden)
      {
        this.m_dictionary.SetProperty("View", (IPdfPrimitive) new PdfName("H"));
      }
      else
      {
        if (this.m_viewMode != PdfPortfolioViewMode.Tile)
          return;
        this.m_dictionary.SetProperty("View", (IPdfPrimitive) new PdfName("T"));
      }
    }
  }

  public PdfAttachment StartupDocument
  {
    get => this.m_startupDocument;
    set
    {
      this.m_startupDocument = value;
      this.m_dictionary.SetProperty("D", (IPdfPrimitive) new PdfString(this.m_startupDocument.FileName));
    }
  }

  public PdfPortfolioInformation() => this.Initialize();

  internal PdfPortfolioInformation(PdfDictionary portfolioDictionary)
  {
    if (portfolioDictionary == null)
      return;
    this.m_dictionary = portfolioDictionary;
    PdfDictionary schemaDictionary = (PdfDictionary) null;
    if (this.m_dictionary[nameof (Schema)] is PdfDictionary)
      schemaDictionary = this.m_dictionary[nameof (Schema)] as PdfDictionary;
    else if ((object) (this.m_dictionary[nameof (Schema)] as PdfReferenceHolder) != null)
      schemaDictionary = (this.m_dictionary[nameof (Schema)] as PdfReferenceHolder).Object as PdfDictionary;
    if (schemaDictionary != null)
      this.m_Schema = new PdfPortfolioSchema(schemaDictionary);
    PdfName pdfName = this.m_dictionary["View"] as PdfName;
    if (!(pdfName != (PdfName) null))
      return;
    if (pdfName.Value.Equals("D"))
      this.ViewMode = PdfPortfolioViewMode.Details;
    else if (pdfName.Value.Equals("T"))
    {
      this.ViewMode = PdfPortfolioViewMode.Tile;
    }
    else
    {
      if (!pdfName.Value.Equals("H"))
        return;
      this.ViewMode = PdfPortfolioViewMode.Hidden;
    }
  }

  private void Initialize()
  {
    this.m_dictionary.SetProperty("Type", (IPdfPrimitive) new PdfName("Collection"));
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
