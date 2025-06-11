// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfDocumentActions
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfDocumentActions : IPdfWrapper
{
  private PdfDictionary m_dictionary = new PdfDictionary();
  private PdfAction m_afterOpen;
  private PdfAction m_beforeClose;
  private PdfJavaScriptAction m_beforeSave;
  private PdfJavaScriptAction m_afterSave;
  private PdfJavaScriptAction m_beforePrint;
  private PdfJavaScriptAction m_afterPrint;
  private PdfCatalog m_catalog;

  internal PdfDocumentActions(PdfCatalog catalog)
  {
    if (catalog == null)
      throw new ArgumentNullException(nameof (catalog));
    if (PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_A1B || PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_A1A)
      throw new PdfConformanceException("Usage of Javascript are not allowed by the PDF/A1-B or PDF/A1-A standard");
    this.m_catalog = catalog;
  }

  public PdfAction AfterOpen
  {
    get => (PdfAction) this.GetAfterOpenDictionary();
    set
    {
      if (value == null)
      {
        this.m_afterOpen = value;
        if (this.m_catalog.ContainsKey("OpenAction"))
          this.m_catalog.Remove("OpenAction");
        this.RemoveJavaScriptAction();
      }
      else
      {
        if (value == this.GetAfterOpenDictionary())
          return;
        this.m_afterOpen = value;
        PdfDictionary.SetProperty((PdfDictionary) this.m_catalog, "OpenAction", (IPdfWrapper) this.m_afterOpen);
      }
    }
  }

  private PdfJavaScriptAction GetAfterOpenDictionary()
  {
    PdfDictionary pdfDictionary = PdfCrossTable.Dereference(this.m_catalog["OpenAction"]) as PdfDictionary;
    string empty = string.Empty;
    if (pdfDictionary != null && pdfDictionary.ContainsKey("JS") && PdfCrossTable.Dereference(pdfDictionary["JS"]) is PdfString pdfString)
      this.m_afterOpen = (PdfAction) new PdfJavaScriptAction(pdfString.Value.ToString());
    return this.m_afterOpen as PdfJavaScriptAction;
  }

  private void RemoveJavaScriptAction()
  {
    if (!this.m_catalog.ContainsKey("Names") || !(PdfCrossTable.Dereference(this.m_catalog["Names"]) is PdfDictionary pdfDictionary))
      return;
    if (pdfDictionary.ContainsKey("JavaScript"))
    {
      pdfDictionary.Remove("JavaScript");
    }
    else
    {
      if (!pdfDictionary.ContainsKey("JS"))
        return;
      pdfDictionary.Remove("JS");
    }
  }

  public PdfJavaScriptAction BeforeClose
  {
    get => this.GetBeforeCloseDictionary();
    set
    {
      if (value == this.GetBeforeCloseDictionary())
        return;
      this.m_beforeClose = (PdfAction) value;
      if (PdfCrossTable.Dereference(this.m_catalog["AA"]) is PdfDictionary dictionary)
      {
        PdfCrossTable.Dereference(dictionary["WC"]);
        PdfDictionary.SetProperty(dictionary, "WC", (IPdfWrapper) this.m_beforeClose);
      }
      else
        this.m_dictionary.SetProperty("WC", (IPdfWrapper) this.m_beforeClose);
    }
  }

  private PdfJavaScriptAction GetBeforeCloseDictionary()
  {
    PdfDictionary pdfDictionary1 = PdfCrossTable.Dereference(this.m_catalog["AA"]) as PdfDictionary;
    string empty = string.Empty;
    if (pdfDictionary1 != null && pdfDictionary1.ContainsKey("WC") && PdfCrossTable.Dereference(pdfDictionary1["WC"]) is PdfDictionary pdfDictionary2 && pdfDictionary2.ContainsKey("Type") && pdfDictionary2.ContainsKey("JS"))
      this.m_beforeClose = (PdfAction) new PdfJavaScriptAction((PdfCrossTable.Dereference(pdfDictionary2["JS"]) as PdfString).Value.ToString());
    return this.m_beforeClose as PdfJavaScriptAction;
  }

  public PdfJavaScriptAction BeforeSave
  {
    get => this.GetBeforeSaveDictionary();
    set
    {
      if (value == this.GetBeforeSaveDictionary())
        return;
      this.m_beforeSave = value;
      if (PdfCrossTable.Dereference(this.m_catalog["AA"]) is PdfDictionary dictionary)
      {
        PdfCrossTable.Dereference(dictionary["WS"]);
        PdfDictionary.SetProperty(dictionary, "WS", (IPdfWrapper) this.m_beforeSave);
      }
      else
        this.m_dictionary.SetProperty("WS", (IPdfWrapper) this.m_beforeSave);
    }
  }

  private PdfJavaScriptAction GetBeforeSaveDictionary()
  {
    PdfDictionary pdfDictionary1 = PdfCrossTable.Dereference(this.m_catalog["AA"]) as PdfDictionary;
    string empty = string.Empty;
    if (pdfDictionary1 != null && pdfDictionary1.ContainsKey("WS") && PdfCrossTable.Dereference(pdfDictionary1["WS"]) is PdfDictionary pdfDictionary2 && pdfDictionary2.ContainsKey("Type") && pdfDictionary2.ContainsKey("JS"))
      this.m_beforeSave = new PdfJavaScriptAction((PdfCrossTable.Dereference(pdfDictionary2["JS"]) as PdfString).Value.ToString());
    return this.m_beforeSave;
  }

  public PdfJavaScriptAction AfterSave
  {
    get => this.GetAfterSaveDictionary();
    set
    {
      if (value == this.GetAfterSaveDictionary())
        return;
      this.m_afterSave = value;
      if (PdfCrossTable.Dereference(this.m_catalog["AA"]) is PdfDictionary dictionary)
      {
        PdfCrossTable.Dereference(dictionary["DS"]);
        PdfDictionary.SetProperty(dictionary, "DS", (IPdfWrapper) this.m_afterSave);
      }
      else
        this.m_dictionary.SetProperty("DS", (IPdfWrapper) this.m_afterSave);
    }
  }

  private PdfJavaScriptAction GetAfterSaveDictionary()
  {
    PdfDictionary pdfDictionary1 = PdfCrossTable.Dereference(this.m_catalog["AA"]) as PdfDictionary;
    string empty = string.Empty;
    if (pdfDictionary1 != null && pdfDictionary1.ContainsKey("DS") && PdfCrossTable.Dereference(pdfDictionary1["DS"]) is PdfDictionary pdfDictionary2 && pdfDictionary2.ContainsKey("Type") && pdfDictionary2.ContainsKey("JS"))
      this.m_afterSave = new PdfJavaScriptAction((PdfCrossTable.Dereference(pdfDictionary2["JS"]) as PdfString).Value.ToString());
    return this.m_afterSave;
  }

  public PdfJavaScriptAction BeforePrint
  {
    get => this.GetBeforePrintDictionary();
    set
    {
      if (value == this.GetBeforePrintDictionary())
        return;
      this.m_beforePrint = value;
      if (PdfCrossTable.Dereference(this.m_catalog["AA"]) is PdfDictionary dictionary)
      {
        PdfCrossTable.Dereference(dictionary["WP"]);
        PdfDictionary.SetProperty(dictionary, "WP", (IPdfWrapper) this.m_beforePrint);
      }
      else
        this.m_dictionary.SetProperty("WP", (IPdfWrapper) this.m_beforePrint);
    }
  }

  private PdfJavaScriptAction GetBeforePrintDictionary()
  {
    PdfDictionary pdfDictionary1 = PdfCrossTable.Dereference(this.m_catalog["AA"]) as PdfDictionary;
    string empty = string.Empty;
    if (pdfDictionary1 != null && pdfDictionary1.ContainsKey("WP") && PdfCrossTable.Dereference(pdfDictionary1["WP"]) is PdfDictionary pdfDictionary2 && pdfDictionary2.ContainsKey("Type") && pdfDictionary2.ContainsKey("JS"))
      this.m_beforePrint = new PdfJavaScriptAction((PdfCrossTable.Dereference(pdfDictionary2["JS"]) as PdfString).Value.ToString());
    return this.m_beforePrint;
  }

  public PdfJavaScriptAction AfterPrint
  {
    get => this.GetAfterPrintDictionary();
    set
    {
      if (value == this.GetAfterPrintDictionary())
        return;
      this.m_afterPrint = value;
      if (PdfCrossTable.Dereference(this.m_catalog["AA"]) is PdfDictionary dictionary)
      {
        PdfCrossTable.Dereference(dictionary["DP"]);
        PdfDictionary.SetProperty(dictionary, "DP", (IPdfWrapper) this.m_afterPrint);
      }
      else
        this.m_dictionary.SetProperty("DP", (IPdfWrapper) this.m_afterPrint);
    }
  }

  private PdfJavaScriptAction GetAfterPrintDictionary()
  {
    PdfDictionary pdfDictionary1 = PdfCrossTable.Dereference(this.m_catalog["AA"]) as PdfDictionary;
    string empty = string.Empty;
    if (pdfDictionary1 != null && pdfDictionary1.ContainsKey("DP") && PdfCrossTable.Dereference(pdfDictionary1["DP"]) is PdfDictionary pdfDictionary2 && pdfDictionary2.ContainsKey("Type") && pdfDictionary2.ContainsKey("JS"))
      this.m_afterPrint = new PdfJavaScriptAction((PdfCrossTable.Dereference(pdfDictionary2["JS"]) as PdfString).Value.ToString());
    return this.m_afterPrint;
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
