// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfPageTemplateCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfPageTemplateCollection : IEnumerable, IPdfWrapper
{
  private List<PdfPageTemplate> m_pageTemplatesCollection = new List<PdfPageTemplate>();
  private PdfDictionary m_dictionary = new PdfDictionary();
  private PdfCrossTable m_crossTable = new PdfCrossTable();
  private System.Collections.Generic.Dictionary<PdfPageBase, string> m_pages = new System.Collections.Generic.Dictionary<PdfPageBase, string>();
  private System.Collections.Generic.Dictionary<PdfPageBase, string> m_templates = new System.Collections.Generic.Dictionary<PdfPageBase, string>();
  private PdfArray m_namedPages = new PdfArray();
  private PdfArray m_namedTempates = new PdfArray();

  internal PdfPageTemplateCollection() => this.Initialize();

  internal PdfPageTemplateCollection(PdfDictionary dictionary, PdfCrossTable crossTable)
  {
    this.m_dictionary = dictionary;
    if (crossTable != null)
      this.m_crossTable = crossTable;
    PdfLoadedDocument document = this.m_crossTable.Document as PdfLoadedDocument;
    if (this.Dictionary != null && this.Dictionary.ContainsKey("Pages") && PdfCrossTable.Dereference(this.Dictionary["Pages"]) is PdfDictionary pdfDictionary1 && pdfDictionary1.ContainsKey("Names") && PdfCrossTable.Dereference(pdfDictionary1["Names"]) is PdfArray pdfArray1)
    {
      this.m_namedPages = pdfArray1;
      if (document != null && this.m_namedPages.Count > 0)
        this.ParsingExistingPageTemplates(document.Pages, this.m_namedPages, true);
      this.m_namedPages.Clear();
    }
    if (this.Dictionary != null && this.Dictionary.ContainsKey("Templates") && PdfCrossTable.Dereference(this.Dictionary["Templates"]) is PdfDictionary pdfDictionary2 && pdfDictionary2.ContainsKey("Names") && PdfCrossTable.Dereference(pdfDictionary2["Names"]) is PdfArray pdfArray2)
    {
      this.m_namedTempates = pdfArray2;
      if (document != null && this.m_namedTempates.Count > 0)
        this.ParsingExistingPageTemplates(document.Pages, this.m_namedTempates, false);
      this.m_namedTempates.Clear();
    }
    this.CrossTable.Document.Catalog.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
    this.CrossTable.Document.Catalog.Modify();
  }

  internal void Initialize()
  {
    this.Dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
  }

  internal PdfDictionary Dictionary => this.m_dictionary;

  internal PdfCrossTable CrossTable => this.m_crossTable;

  public int Count => this.m_pageTemplatesCollection.Count;

  public PdfPageTemplate this[int index]
  {
    get
    {
      return index >= 0 && index <= this.Count - 1 ? this.m_pageTemplatesCollection[index] : throw new ArgumentOutOfRangeException(nameof (index), "Index is out of range.");
    }
  }

  public void Add(PdfPageTemplate pdfPageTemplate)
  {
    if (pdfPageTemplate == null)
      throw new ArgumentNullException("The PdfPageTemplate value can't be null");
    this.m_pageTemplatesCollection.Add(pdfPageTemplate);
  }

  public bool Contains(PdfPageTemplate pdfPageTemplate)
  {
    return pdfPageTemplate != null ? this.m_pageTemplatesCollection.Contains(pdfPageTemplate) : throw new ArgumentNullException("The PdfPageTemplate value can't be null");
  }

  public void RemoveAt(int index)
  {
    if (index >= this.m_pageTemplatesCollection.Count)
      throw new PdfException("The index value should not be greater than or equal to the count ");
    this.m_pageTemplatesCollection.RemoveAt(index);
  }

  public void Remove(PdfPageTemplate pdfPageTemplate)
  {
    if (!this.m_pageTemplatesCollection.Contains(pdfPageTemplate))
      return;
    this.m_pageTemplatesCollection.Remove(pdfPageTemplate);
  }

  public void Clear() => this.m_pageTemplatesCollection.Clear();

  private void Dictionary_BeginSave(object sender, SavePdfPrimitiveEventArgs ars)
  {
    foreach (PdfPageTemplate pageTemplates in this.m_pageTemplatesCollection)
    {
      if (pageTemplates.IsVisible)
      {
        this.m_pages[pageTemplates.PdfPageBase] = pageTemplates.Name;
      }
      else
      {
        this.m_templates[pageTemplates.PdfPageBase] = pageTemplates.Name;
        if (pageTemplates.PdfPageBase is PdfLoadedPage pdfPageBase2 && pdfPageBase2.Document != null)
        {
          int pageCount1 = pdfPageBase2.Document.PageCount;
        }
        else if (pageTemplates.PdfPageBase is PdfPage pdfPageBase1 && pdfPageBase1.Document != null)
        {
          int pageCount2 = pdfPageBase1.Document.PageCount;
        }
      }
    }
    if (this.m_pages.Count > 0)
    {
      foreach (KeyValuePair<PdfPageBase, string> page in this.m_pages)
      {
        this.m_namedPages.Add((IPdfPrimitive) new PdfString(page.Value));
        this.m_namedPages.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) page.Key));
      }
      this.Dictionary["Pages"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) new PdfDictionary()
      {
        ["Names"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this.m_namedPages)
      });
    }
    if (this.m_templates.Count <= 0)
      return;
    foreach (KeyValuePair<PdfPageBase, string> template in this.m_templates)
    {
      this.m_namedTempates.Add((IPdfPrimitive) new PdfString(template.Value));
      this.m_namedTempates.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) template.Key));
    }
    this.Dictionary["Templates"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) new PdfDictionary()
    {
      ["Names"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this.m_namedTempates)
    });
  }

  private void ParsingExistingPageTemplates(
    PdfLoadedPageCollection pageCollection,
    PdfArray pageTemplates,
    bool isVisible)
  {
    for (int index = 1; index <= pageTemplates.Count; index += 2)
    {
      if (PdfCrossTable.Dereference(pageTemplates[index]) is PdfDictionary dic)
      {
        PdfPageBase page = pageCollection.GetPage(dic);
        if (PdfCrossTable.Dereference(pageTemplates[index - 1]) is PdfString pdfString && page != null)
        {
          PdfPageTemplate pdfPageTemplate = new PdfPageTemplate();
          pdfPageTemplate.PdfPageBase = page;
          pdfPageTemplate.IsVisible = isVisible;
          pdfPageTemplate.Name = pdfString.Value;
          if (!this.m_pageTemplatesCollection.Contains(pdfPageTemplate))
            this.m_pageTemplatesCollection.Add(pdfPageTemplate);
        }
      }
    }
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    return (IEnumerator) this.m_pageTemplatesCollection.GetEnumerator();
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
