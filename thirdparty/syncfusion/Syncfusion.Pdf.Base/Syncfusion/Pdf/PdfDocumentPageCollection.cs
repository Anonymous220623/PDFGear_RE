// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfDocumentPageCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfDocumentPageCollection : IEnumerable
{
  private PdfDocument m_document;
  private Dictionary<PdfPage, int> m_pageCollectionIndex = new Dictionary<PdfPage, int>();
  internal int count;

  public int Count => this.CountPages();

  public PdfPage this[int index] => this.GetPageByIndex(index);

  internal Dictionary<PdfPage, int> PageCollectionIndex => this.m_pageCollectionIndex;

  public event PageAddedEventHandler PageAdded;

  internal PdfDocumentPageCollection(PdfDocument document)
  {
    this.m_document = document != null ? document : throw new ArgumentNullException(nameof (document));
  }

  public PdfPage Add()
  {
    PdfPage page = new PdfPage();
    if (PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_A3B || PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_A3A || PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_A3U || PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_A2B || PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_A2A || PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_A2U)
    {
      PdfName key = new PdfName("Resources");
      page.Dictionary.Items.Add(key, (IPdfPrimitive) new PdfDictionary());
    }
    page.IsNewPage = true;
    this.Add(page);
    page.IsNewPage = false;
    return page;
  }

  internal void Add(PdfPage page)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    PdfSection pdfSection = this.GetLastSection();
    if (this.GetLastSection().PageSettings.Orientation != this.m_document.PageSettings.Orientation)
    {
      pdfSection = this.m_document.Sections.Add();
      pdfSection.PageSettings.Orientation = this.m_document.PageSettings.Orientation;
    }
    if (!this.m_pageCollectionIndex.ContainsKey(page))
      this.m_pageCollectionIndex.Add(page, this.count++);
    pdfSection.Add(page);
  }

  public void Insert(int index, PdfPage page)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    if (index < 0)
      throw new ArgumentOutOfRangeException(nameof (index));
    if (index > this.Count)
      throw new ArgumentOutOfRangeException(nameof (index), "Value can not be less 0, equal or more than number of pages in the document.");
    if (page.m_section != null)
    {
      page.Section = (PdfSection) null;
      for (int index1 = 0; index1 < this.m_document.Sections.Count; ++index1)
      {
        if (this.m_document.Sections[index1].Pages.Contains(page))
          this.m_document.Sections[index1].Remove(page);
      }
    }
    if (index == this.Count)
    {
      this.GetLastSection().Add(page);
    }
    else
    {
      int num = 0;
      int index2 = 0;
      for (int count = this.m_document.Sections.Count; index2 < count; ++index2)
      {
        PdfSection section = this.m_document.Sections[index2];
        for (int index3 = 0; index3 < section.Pages.Count; ++index3)
        {
          if (num == index)
          {
            section.Insert(index3, page);
            return;
          }
          ++num;
        }
      }
    }
  }

  public void Insert(int index, PdfPageBase loadedPage)
  {
    if (loadedPage == null)
      throw new ArgumentNullException(nameof (loadedPage));
    if (index < 0)
      throw new ArgumentOutOfRangeException(nameof (index), "Value cannot be less than zero");
    if (index > this.Count)
      throw new ArgumentOutOfRangeException(nameof (index), "Value can not be more than number of pages in the document.");
    PdfDictionary dictionary = loadedPage.Dictionary;
    if (dictionary.ContainsKey("Parent"))
    {
      if (PdfCrossTable.Dereference(dictionary["Parent"]) is PdfDictionary pdfDictionary1)
      {
        if (pdfDictionary1.ContainsKey("MediaBox") && !dictionary.ContainsKey("MediaBox"))
          dictionary.Items.Add((PdfName) "MediaBox", pdfDictionary1["MediaBox"]);
        if (pdfDictionary1.ContainsKey("Rotate") && !dictionary.ContainsKey("Rotate"))
          dictionary.Items.Add((PdfName) "Rotate", pdfDictionary1["Rotate"]);
      }
      if (!dictionary.ContainsKey("MediaBox") && PdfCrossTable.Dereference(pdfDictionary1["Parent"]) is PdfDictionary pdfDictionary2 && pdfDictionary2["MediaBox"] != null)
        dictionary.Items.Add((PdfName) "MediaBox", pdfDictionary2["MediaBox"]);
    }
    if (dictionary.ContainsKey("Contents"))
    {
      PdfArray pdfArray = loadedPage.ReInitializeContentReference();
      if (pdfArray.Elements.Count > 0)
        dictionary["Contents"] = (IPdfPrimitive) pdfArray;
    }
    if (dictionary.ContainsKey("Resources"))
    {
      PdfDictionary pdfDictionary = loadedPage.ReinitializePageResources();
      if (dictionary["Resources"] as PdfReferenceHolder != (PdfReferenceHolder) null)
        dictionary["Resources"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary);
    }
    if (dictionary.ContainsKey("Annots"))
    {
      PdfCatalog catalog1 = (loadedPage as PdfLoadedPage).Document.Catalog;
      PdfDictionary acroFormData = (PdfDictionary) null;
      if (catalog1.ContainsKey("AcroForm"))
        acroFormData = !(catalog1["AcroForm"] as PdfReferenceHolder != (PdfReferenceHolder) null) ? catalog1["AcroForm"] as PdfDictionary : PdfCrossTable.Dereference(catalog1["AcroForm"]) as PdfDictionary;
      loadedPage.ReInitializePageAnnotation(acroFormData);
      if (acroFormData != null)
      {
        PdfCatalog catalog2 = this.m_document.Catalog;
        catalog2.Items.Add((PdfName) "AcroForm", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) acroFormData));
        catalog2.Modify();
      }
    }
    if (dictionary.ContainsKey("Thumb"))
      loadedPage.ReInitializeThumbnail();
    int num = 0;
    int index1 = 0;
    for (int count = this.m_document.Sections.Count; index1 < count; ++index1)
    {
      PdfSection section = this.m_document.Sections[index1];
      for (int index2 = 0; index2 < section.Pages.Count; ++index2)
      {
        if (num == index)
        {
          section.Insert(index2, loadedPage);
          return;
        }
        ++num;
      }
    }
  }

  public int IndexOf(PdfPage page)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    int num1 = -1;
    int num2 = 0;
    int index = 0;
    for (int count = this.m_document.Sections.Count; index < count; ++index)
    {
      PdfSection section = this.m_document.Sections[index];
      num1 = section.IndexOf(page);
      if (num1 >= 0)
      {
        num1 += num2;
        break;
      }
      num2 += section.Count;
    }
    return num1;
  }

  internal PdfSection Remove(PdfPage page)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    PdfSection pdfSection = (PdfSection) null;
    int index = 0;
    for (int count = this.m_document.Sections.Count; index < count; ++index)
    {
      pdfSection = this.m_document.Sections[index];
      if (pdfSection.Pages.Contains(page))
      {
        pdfSection.Pages.Remove(page);
        break;
      }
    }
    return pdfSection;
  }

  private void RemoveAndClearAllPages()
  {
    int index1 = 0;
    for (int count = this.m_document.Sections.Count; index1 < count; ++index1)
    {
      PdfSection section = this.m_document.Sections[index1];
      for (int index2 = 0; index2 < section.Pages.Count; ++index2)
      {
        PdfPage page = section.Pages[index2];
        section.Pages.Remove(page);
        page.Clear();
      }
    }
  }

  internal void Clear()
  {
    this.RemoveAndClearAllPages();
    this.m_pageCollectionIndex.Clear();
    this.m_pageCollectionIndex = (Dictionary<PdfPage, int>) null;
    this.m_document = (PdfDocument) null;
  }

  private int CountPages()
  {
    PdfSectionCollection sections = this.m_document.Sections;
    int num = 0;
    foreach (PdfSection pdfSection in sections)
      num += pdfSection.Count;
    return num;
  }

  private PdfPage GetPageByIndex(int index)
  {
    if (index < 0 || index >= this.Count)
      throw new ArgumentOutOfRangeException(nameof (index), "Value can not be less 0, equal or more than number of pages in the document.");
    PdfPage pageByIndex = (PdfPage) null;
    int num = 0;
    int index1 = 0;
    for (int count1 = this.m_document.Sections.Count; index1 < count1; ++index1)
    {
      PdfSection section = this.m_document.Sections[index1];
      int count2 = section.Count;
      int index2 = index - num;
      if (index >= num && index2 < count2)
      {
        pageByIndex = section[index2];
        break;
      }
      num += count2;
    }
    return pageByIndex;
  }

  private void Add(PdfLoadedPage page) => throw new NotImplementedException();

  private PdfSection GetLastSection()
  {
    PdfSectionCollection sections = this.m_document.Sections;
    if (sections.Count == 0)
      sections.Add();
    return sections[sections.Count - 1];
  }

  internal void OnPageAdded(PageAddedEventArgs args)
  {
    if (this.PageAdded == null)
      return;
    this.PageAdded((object) this, args);
  }

  internal PdfPageBase Add(PdfLoadedDocument ldDoc, PdfPageBase page, List<PdfArray> destinations)
  {
    if (ldDoc == null)
      throw new ArgumentNullException(nameof (ldDoc));
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    PdfSection pdfSection;
    if (this.CanPageFitLastSection(page))
    {
      pdfSection = this.GetLastSection();
    }
    else
    {
      pdfSection = this.m_document.Sections.Add();
      PdfPageSettings pageSettings = pdfSection.PageSettings;
      pageSettings.Size = page.Size;
      pageSettings.Orientation = page.Orientation != PdfPageOrientation.Portrait || !page.Dictionary.ContainsKey("CropBox") || page.Rotation != PdfPageRotateAngle.RotateAngle90 && page.Rotation != PdfPageRotateAngle.RotateAngle270 ? page.Orientation : PdfPageOrientation.Landscape;
      pageSettings.Rotate = page.Rotation;
      pageSettings.m_isRotation = false;
      if (!ldDoc.IsExtendMargin)
        pageSettings.Margins.All = 0.0f;
      pageSettings.Origin = page.Origin;
      pdfSection.m_importedSection = true;
    }
    PdfPage pdfPage = pdfSection.Add();
    this.m_pageCollectionIndex.Add(pdfPage, this.count++);
    if (page.Dictionary["CropBox"] is PdfArray primitive1)
      pdfPage.Dictionary.SetProperty("CropBox", (IPdfPrimitive) primitive1);
    SizeF size = pdfPage.Size;
    if (page.Dictionary.ContainsKey("MediaBox") && page.Dictionary["MediaBox"] is PdfArray primitive2)
    {
      pdfPage.Dictionary.SetProperty("MediaBox", (IPdfPrimitive) primitive2);
      SizeF sizeF = new SizeF((primitive2[2] as PdfNumber).FloatValue, (primitive2[3] as PdfNumber).FloatValue);
    }
    PdfTemplate template = (PdfTemplate) null;
    if (ldDoc.IsExtendMargin)
      template = page.ContentTemplate;
    if (template != null && template.m_content != null && template.m_content.Data.Length > 0)
    {
      template.isLoadedPageTemplate = this.m_document.EnableMemoryOptimization;
      pdfPage.Graphics.DrawPdfTemplate(template, PointF.Empty, pdfPage.GetClientSize());
      if (ldDoc.IsOptimizeIdentical)
      {
        PdfResources resources = pdfPage.GetResources();
        pdfPage.DestinationDocument = ldDoc.DestinationDocument;
        pdfPage.RemoveIdenticalResources(resources, pdfPage);
        pdfPage.Dictionary["Resources"] = (IPdfPrimitive) resources;
        pdfPage.SetResources(resources);
      }
    }
    else if (page.Contents.Count > 0)
    {
      foreach (IPdfPrimitive content in page.Contents)
      {
        IPdfPrimitive element = !this.m_document.EnableMemoryOptimization ? content : content.Clone(this.m_document.CrossTable);
        pdfPage.Contents.Add(element);
      }
      PdfResources pdfResources = !this.m_document.EnableMemoryOptimization ? page.GetResources() : new PdfResources(page.GetResources().Clone(this.m_document.CrossTable) as PdfDictionary);
      if (ldDoc.IsOptimizeIdentical)
      {
        pdfPage.DestinationDocument = ldDoc.DestinationDocument;
        pdfPage.RemoveIdenticalResources(pdfResources, pdfPage);
      }
      pdfPage.Dictionary["Resources"] = (IPdfPrimitive) pdfResources;
      pdfPage.SetResources(pdfResources);
    }
    if (!this.m_document.EnableMemoryOptimization)
      pdfPage.ImportAnnotations(ldDoc, page, destinations);
    return (PdfPageBase) pdfPage;
  }

  private bool CanPageFitLastSection(PdfPageBase page) => false;

  public IEnumerator GetEnumerator()
  {
    return (IEnumerator) new PdfDocumentPageCollection.PdfPageEnumerator(this);
  }

  private struct PdfPageEnumerator : IEnumerator
  {
    private PdfDocumentPageCollection m_pageCollection;
    private int m_currentIndex;

    internal PdfPageEnumerator(PdfDocumentPageCollection pageCollection)
    {
      this.m_pageCollection = pageCollection != null ? pageCollection : throw new ArgumentNullException(nameof (pageCollection));
      this.m_currentIndex = -1;
    }

    public object Current
    {
      get
      {
        this.CheckIndex();
        return (object) this.m_pageCollection[this.m_currentIndex];
      }
    }

    public bool MoveNext()
    {
      ++this.m_currentIndex;
      return this.m_currentIndex < this.m_pageCollection.Count;
    }

    public void Reset() => this.m_currentIndex = -1;

    private void CheckIndex()
    {
      if (this.m_currentIndex < 0 || this.m_currentIndex >= this.m_pageCollection.Count)
        throw new IndexOutOfRangeException();
    }
  }
}
