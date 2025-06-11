// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfCatalog
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using Syncfusion.Pdf.Xmp;
using System;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf;

internal class PdfCatalog : PdfDictionary
{
  private PdfSectionCollection m_sections;
  private PdfAttachmentCollection m_attachment;
  private PdfViewerPreferences m_viewerPreferences;
  private PdfCatalogNames m_names;
  private XmpMetadata m_metadata;
  private PdfForm m_form;
  private PdfLoadedForm m_loadedForm;
  private PdfLoadedDocument m_loadedDocument;
  private PdfDictionary m_dictionary = new PdfDictionary();
  private PdfPortfolioInformation m_pdfPortfolio;
  private bool m_noNames;
  [ThreadStatic]
  internal static PdfStructTreeRoot m_structTreeRoot;

  internal PdfCatalog() => this["Type"] = (IPdfPrimitive) new PdfName("Catalog");

  internal PdfCatalog(PdfLoadedDocument document, PdfDictionary catalog)
    : base(catalog)
  {
    this.m_loadedDocument = document;
    if (PdfCrossTable.Dereference(this[nameof (Names)]) is PdfDictionary root)
      this.m_names = new PdfCatalogNames(root);
    else
      this.m_noNames = true;
    this.ReadMetadata();
    this.FreezeChanges((object) this);
  }

  public PdfViewerPreferences ViewerPreferences
  {
    get => this.m_viewerPreferences;
    set
    {
      if (this.m_viewerPreferences == value)
        return;
      this.m_viewerPreferences = value;
      if (this[nameof (ViewerPreferences)] != null && this.LoadedDocument != null)
      {
        this.m_dictionary = this[nameof (ViewerPreferences)] as PdfDictionary;
        PdfReferenceHolder primitive = this[nameof (ViewerPreferences)] as PdfReferenceHolder;
        this[nameof (ViewerPreferences)] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) value);
        if (this.m_dictionary != null)
          this.SetProperty(nameof (ViewerPreferences), (IPdfPrimitive) new PdfDictionary(this.m_dictionary));
        else
          this.SetProperty(nameof (ViewerPreferences), (IPdfPrimitive) primitive);
      }
      else
        this[nameof (ViewerPreferences)] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) value);
    }
  }

  internal PdfPortfolioInformation PdfPortfolio
  {
    get => this.m_pdfPortfolio;
    set
    {
      this.m_pdfPortfolio = value;
      this["Collection"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_pdfPortfolio);
    }
  }

  public static PdfStructTreeRoot StructTreeRoot => PdfCatalog.m_structTreeRoot;

  public PdfForm Form
  {
    get => this.m_form;
    set
    {
      if (this.m_form == value)
        return;
      this.m_form = value;
      this["AcroForm"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_form);
    }
  }

  public PdfCatalogNames Names
  {
    get
    {
      if (!this.m_noNames)
        this.CreateNamesIfNone();
      return this.m_names;
    }
  }

  internal PdfDictionary Destinations
  {
    get
    {
      PdfDictionary destinations = (PdfDictionary) null;
      if (this.ContainsKey("Dests"))
        destinations = PdfCrossTable.Dereference(this["Dests"]) as PdfDictionary;
      return destinations;
    }
  }

  internal PdfLoadedForm LoadedForm
  {
    get => this.m_loadedForm;
    set
    {
      this.m_loadedForm = value != null ? value : throw new ArgumentNullException(nameof (LoadedForm));
    }
  }

  internal PdfLoadedDocument LoadedDocument => this.m_loadedDocument;

  internal PdfSectionCollection Pages
  {
    get => this.m_sections;
    set
    {
      if (this.m_sections == value)
        return;
      this.m_sections = value;
      this[nameof (Pages)] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) value);
    }
  }

  internal PdfAttachmentCollection Attachments
  {
    get => this.m_attachment;
    set
    {
      this.m_attachment = value != null ? value : throw new ArgumentNullException("LoadedForm");
    }
  }

  internal XmpMetadata Metadata
  {
    get => this.m_metadata;
    set
    {
      if (value == null && !this.m_loadedDocument.ConformanceEnabled)
        throw new ArgumentNullException(nameof (Metadata));
      this.m_metadata = value;
    }
  }

  internal void CreateNamesIfNone()
  {
    if (this.m_names != null)
      return;
    this.m_names = new PdfCatalogNames();
    this["Names"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_names);
  }

  internal void InitializeStructTreeRoot()
  {
    if (this.ContainsKey("StructTreeRoot") || PdfCatalog.m_structTreeRoot != null)
      return;
    PdfCatalog.m_structTreeRoot = new PdfStructTreeRoot();
    this["StructTreeRoot"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) PdfCatalog.m_structTreeRoot);
  }

  private void ReadMetadata()
  {
    if (!(PdfCrossTable.Dereference(this["Metadata"]) is PdfStream pdfStream))
      return;
    bool flag = false;
    if (pdfStream.ContainsKey("Filter"))
    {
      IPdfPrimitive pdfPrimitive1 = pdfStream["Filter"];
      if ((object) (pdfPrimitive1 as PdfReferenceHolder) != null)
        pdfPrimitive1 = (pdfPrimitive1 as PdfReferenceHolder).Object;
      if (pdfPrimitive1 != null)
      {
        if ((object) (pdfPrimitive1 as PdfName) != null)
        {
          if ((pdfPrimitive1 as PdfName).Value == "FlateDecode")
            flag = true;
        }
        else if (pdfPrimitive1 is PdfArray)
        {
          foreach (IPdfPrimitive pdfPrimitive2 in pdfPrimitive1 as PdfArray)
          {
            if ((pdfPrimitive2 as PdfName).Value == "FlateDecode")
              flag = true;
          }
        }
      }
    }
    if (!pdfStream.Compress)
    {
      if (!flag)
        goto label_21;
    }
    try
    {
      pdfStream.Decompress();
    }
    catch (Exception ex)
    {
    }
label_21:
    MemoryStream inStream1 = new MemoryStream(pdfStream.Data);
    XmlDocument xmp = new XmlDocument();
    try
    {
      xmp.Load((Stream) inStream1);
    }
    catch (XmlException ex1)
    {
      pdfStream.Decompress();
      MemoryStream inStream2 = new MemoryStream(pdfStream.Data);
      try
      {
        xmp.Load((Stream) inStream2);
      }
      catch (XmlException ex2)
      {
        return;
      }
    }
    this.m_metadata = new XmpMetadata(xmp);
    this.LoadedDocument.DublinSchema = this.m_metadata.DublinCoreSchema;
    this.m_metadata.isLoadedDocument = true;
  }

  internal void ApplyPdfXConformance()
  {
    PdfDictionary element = new PdfDictionary();
    element["S"] = (IPdfPrimitive) new PdfName("GTS_PDFX");
    element["OutputConditionIdentifier"] = (IPdfPrimitive) new PdfString("CGATS TR 001");
    element["Info"] = (IPdfPrimitive) new PdfString(string.Empty);
    element["OutputCondition"] = (IPdfPrimitive) new PdfString("SWOP CGATS TR 001-1995");
    element["Type"] = (IPdfPrimitive) new PdfName("OutputIntent");
    element["RegistryName"] = (IPdfPrimitive) new PdfString("http://www.color.org");
    PdfArray pdfArray = new PdfArray();
    pdfArray.Insert(0, (IPdfPrimitive) element);
    this["OutputIntents"] = (IPdfPrimitive) pdfArray;
  }

  internal new void Clear()
  {
    if (this.m_names != null)
    {
      this.m_names.Clear();
      this.m_names = (PdfCatalogNames) null;
    }
    if (this.m_viewerPreferences != null)
    {
      this.m_viewerPreferences = (PdfViewerPreferences) null;
      this.m_viewerPreferences = (PdfViewerPreferences) null;
    }
    if (this.m_attachment != null)
    {
      this.m_attachment.Clear();
      this.m_attachment = (PdfAttachmentCollection) null;
    }
    if (PdfCatalog.m_structTreeRoot != null)
    {
      PdfCatalog.m_structTreeRoot.Clear();
      PdfCatalog.m_structTreeRoot = (PdfStructTreeRoot) null;
    }
    this.m_form = (PdfForm) null;
    this.m_loadedDocument = (PdfLoadedDocument) null;
    this.m_loadedForm = (PdfLoadedForm) null;
    this.m_metadata = (XmpMetadata) null;
    this.m_sections = (PdfSectionCollection) null;
    base.Clear();
  }
}
