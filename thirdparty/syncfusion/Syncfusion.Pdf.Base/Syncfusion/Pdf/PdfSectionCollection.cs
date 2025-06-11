// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfSectionCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfSectionCollection : IPdfWrapper, IEnumerable
{
  internal const int RotateFactor = 90;
  private PdfArray m_sectionCollection;
  private List<PdfSection> m_sections = new List<PdfSection>();
  private PdfDictionary m_pages;
  private PdfNumber m_count;
  private PdfDocument m_document;

  public PdfSection this[int index]
  {
    get
    {
      return index >= 0 && index < this.Count ? this.m_sections[index] : throw new IndexOutOfRangeException();
    }
  }

  public int Count => this.m_sections.Count;

  internal PdfDocument Document => this.m_document;

  internal PdfSectionCollection(PdfDocument document)
  {
    this.m_document = document != null ? document : throw new ArgumentNullException(nameof (document));
    this.Initialize();
  }

  public PdfSection Add()
  {
    PdfSection section = new PdfSection(this.m_document);
    this.AddSection(section);
    return section;
  }

  public int IndexOf(PdfSection section)
  {
    return this.m_sectionCollection.IndexOf((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) section));
  }

  public void Insert(int index, PdfSection section)
  {
    if (index < 0 || index >= this.Count)
      throw new IndexOutOfRangeException();
    PdfReferenceHolder element = this.CheckSection(section);
    this.m_sectionCollection.Insert(index, (IPdfPrimitive) element);
    if (this.m_sections.Contains(section))
      return;
    this.m_sections.Add(section);
  }

  public bool Contains(PdfSection section)
  {
    if (section == null)
      throw new ArgumentNullException(nameof (section));
    return this.IndexOf(section) >= 0;
  }

  public IEnumerator GetEnumerator()
  {
    return (IEnumerator) new PdfSectionCollection.PdfSectionEnumerator(this);
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_pages;

  internal void PageLabelsSet() => this.Document.PageLabelsSet();

  internal void ResetProgress()
  {
    foreach (PdfSection pdfSection in this)
      pdfSection.ResetProgress();
  }

  internal void SetProgress()
  {
    foreach (PdfSection pdfSection in this)
      pdfSection.SetProgress();
  }

  internal void OnPageSaving(PdfPage page) => this.Document.OnPageSave((PdfPageBase) page);

  private void SetPageSettings(PdfDictionary container, PdfPageSettings pageSettings)
  {
    if (container == null)
      throw new ArgumentNullException(nameof (container));
    if (pageSettings == null)
      throw new ArgumentNullException(nameof (pageSettings));
    RectangleF rectangle = new RectangleF(PointF.Empty, pageSettings.Size);
    if (PdfDocument.ConformanceLevel != PdfConformanceLevel.Pdf_X1A2001)
      container["MediaBox"] = (IPdfPrimitive) PdfArray.FromRectangle(rectangle);
    int rotate = (int) pageSettings.Rotate;
    PdfNumber pdfNumber = new PdfNumber(90 * (int) pageSettings.Rotate);
    container["Rotate"] = (IPdfPrimitive) pdfNumber;
    if (pageSettings.Unit == PdfGraphicsUnit.Point)
      return;
    float num = new PdfUnitConvertor().ConvertUnits(1f, pageSettings.Unit, PdfGraphicsUnit.Point);
    container["UserUnit"] = (IPdfPrimitive) new PdfNumber(num);
  }

  private PdfReferenceHolder CheckSection(PdfSection section)
  {
    PdfReferenceHolder element = new PdfReferenceHolder((IPdfWrapper) section);
    bool flag = this.m_sectionCollection.Contains((IPdfPrimitive) element);
    if (section.Parent != null && section.Parent != this)
      section.Parent = this;
    if (flag)
      throw new ArgumentException("The object can't be added twice to the collection.", nameof (section));
    return element;
  }

  private int CountPages()
  {
    int num = 0;
    foreach (PdfSection pdfSection in this)
      num += pdfSection.Count;
    return num;
  }

  private int Add(PdfSection section)
  {
    if (section == null)
      throw new ArgumentNullException(nameof (section));
    PdfReferenceHolder element = !this.m_document.IsMergeDocHasSections ? new PdfReferenceHolder((IPdfWrapper) section) : this.CheckSection(section);
    this.m_sections.Add(section);
    section.Parent = this;
    this.m_sectionCollection.Add((IPdfPrimitive) element);
    return this.m_sections.IndexOf(section);
  }

  private void AddSection(PdfSection section)
  {
    if (section == null)
      throw new ArgumentNullException(nameof (section));
    PdfReferenceHolder element = !this.m_document.IsMergeDocHasSections ? new PdfReferenceHolder((IPdfWrapper) section) : this.CheckSection(section);
    this.m_sections.Add(section);
    section.Parent = this;
    this.m_sectionCollection.Add((IPdfPrimitive) element);
  }

  private void Initialize()
  {
    this.m_count = new PdfNumber(0);
    this.m_sectionCollection = new PdfArray();
    this.m_pages = new PdfDictionary();
    this.m_pages.BeginSave += new SavePdfPrimitiveEventHandler(this.BeginSave);
    this.m_pages["Type"] = (IPdfPrimitive) new PdfName("Pages");
    this.m_pages["Kids"] = (IPdfPrimitive) this.m_sectionCollection;
    this.m_pages["Count"] = (IPdfPrimitive) this.m_count;
    this.m_pages["Resources"] = (IPdfPrimitive) new PdfDictionary();
    this.SetPageSettings(this.m_pages, this.m_document.PageSettings);
  }

  internal void Clear()
  {
    foreach (PdfSection pdfSection in this)
      pdfSection.Clear();
    if (this.m_pages != null)
      this.m_pages.Clear();
    if (this.m_sectionCollection != null)
      this.m_sectionCollection.Clear();
    if (this.m_sections != null)
      this.m_sections.Clear();
    this.m_pages = (PdfDictionary) null;
    this.m_sectionCollection = (PdfArray) null;
    this.m_sections = (List<PdfSection>) null;
    this.m_document = (PdfDocument) null;
  }

  private void BeginSave(object sender, SavePdfPrimitiveEventArgs e)
  {
    this.m_count.IntValue = this.CountPages();
    this.SetPageSettings(this.m_pages, this.m_document.PageSettings);
  }

  private struct PdfSectionEnumerator : IEnumerator
  {
    private PdfSectionCollection m_sectionCollection;
    private int m_currentIndex;

    internal PdfSectionEnumerator(PdfSectionCollection sectionCollection)
    {
      this.m_sectionCollection = sectionCollection != null ? sectionCollection : throw new ArgumentNullException(nameof (sectionCollection));
      this.m_currentIndex = -1;
    }

    public object Current
    {
      get
      {
        this.CheckIndex();
        return (object) this.m_sectionCollection[this.m_currentIndex];
      }
    }

    public bool MoveNext()
    {
      ++this.m_currentIndex;
      return this.m_currentIndex < this.m_sectionCollection.Count;
    }

    public void Reset() => this.m_currentIndex = -1;

    private void CheckIndex()
    {
      if (this.m_currentIndex < 0 || this.m_currentIndex >= this.m_sectionCollection.Count)
        throw new IndexOutOfRangeException();
    }
  }
}
