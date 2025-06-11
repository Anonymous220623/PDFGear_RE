// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfSection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfSection : IPdfWrapper, IEnumerable
{
  private List<PdfPageBase> m_pages = new List<PdfPageBase>();
  private PdfArray m_pagesReferences;
  private PdfDictionary m_section;
  private PdfNumber m_count;
  private PdfSectionCollection m_parent;
  private PdfDictionary m_resources;
  private PdfPageSettings m_settings;
  private PdfSectionTemplate m_pageTemplate;
  private PdfPageLabel m_pageLabel;
  private bool m_isProgressOn;
  private PdfPageSettings m_initialSettings;
  private PdfPageTransition m_savedTransition;
  private bool m_isTransitionSaved;
  private PdfSectionPageCollection m_pagesCollection;
  internal PdfDocumentBase m_document;
  private bool m_isNewPageSection;
  internal bool m_importedSection;

  public PdfSectionPageCollection Pages
  {
    get
    {
      if (this.m_pagesCollection == null)
        this.m_pagesCollection = new PdfSectionPageCollection(this);
      return this.m_pagesCollection;
    }
  }

  public PdfPageLabel PageLabel
  {
    get => this.m_pageLabel;
    set
    {
      if (value != null)
        this.Parent.PageLabelsSet();
      this.m_pageLabel = value;
    }
  }

  public PdfPageSettings PageSettings
  {
    get => this.m_settings;
    set
    {
      this.m_settings = value != null ? value : throw new ArgumentNullException(nameof (PageSettings));
    }
  }

  public PdfSectionTemplate Template
  {
    get
    {
      if (this.m_pageTemplate == null)
        this.m_pageTemplate = new PdfSectionTemplate();
      return this.m_pageTemplate;
    }
    set => this.m_pageTemplate = value;
  }

  internal PdfPage this[int index]
  {
    get
    {
      return 0 <= index && this.Count > index ? this.m_pages[index] as PdfPage : throw new ArgumentOutOfRangeException(nameof (index), "The index can't be less then zero or greater then Count.");
    }
  }

  internal int Count => this.m_pagesReferences.Count;

  internal PdfSectionCollection Parent
  {
    get => this.m_parent;
    set
    {
      this.m_parent = value;
      if (value != null)
        this.m_section[nameof (Parent)] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) value);
      else
        this.m_section.Remove(nameof (Parent));
    }
  }

  internal PdfDictionary Resources
  {
    get
    {
      if (this.m_resources == null)
      {
        this.m_resources = new PdfDictionary();
        this.m_section[nameof (Resources)] = (IPdfPrimitive) this.m_resources;
      }
      return this.m_resources;
    }
  }

  internal PdfDocument Document => this.m_parent.Document;

  internal PdfDocumentBase ParentDocument => this.m_document;

  public event PageAddedEventHandler PageAdded;

  private PdfSection() => this.Initialize();

  internal PdfSection(PdfDocumentBase document, PdfPageSettings pageSettings)
    : this()
  {
    this.m_document = document;
    this.m_settings = (PdfPageSettings) pageSettings.Clone();
    this.m_initialSettings = (PdfPageSettings) this.m_settings.Clone();
  }

  internal PdfSection(PdfDocument document)
    : this((PdfDocumentBase) document, document.PageSettings)
  {
  }

  internal PdfPage Add()
  {
    PdfPage page = new PdfPage();
    this.m_isNewPageSection = true;
    this.Add(page);
    this.m_isNewPageSection = false;
    return page;
  }

  internal void Add(PdfPage page)
  {
    if (!this.m_isNewPageSection)
      this.m_isNewPageSection = page.IsNewPage;
    PdfReferenceHolder element = this.m_isNewPageSection ? new PdfReferenceHolder((IPdfWrapper) page) : this.CheckPresence(page);
    this.m_isNewPageSection = false;
    this.m_pages.Add((PdfPageBase) page);
    this.m_pagesReferences.Add((IPdfPrimitive) element);
    page.AssignSection(this);
    if (this.m_isProgressOn)
      page.SetProgress();
    else
      page.ResetProgress();
    this.PageAddedMethod(page);
  }

  internal void Insert(int index, PdfPage page)
  {
    PdfReferenceHolder element = this.CheckPresence(page);
    this.m_pages.Insert(index, (PdfPageBase) page);
    this.m_pagesReferences.Insert(index, (IPdfPrimitive) element);
    page.AssignSection(this);
    if (this.m_isProgressOn)
      page.SetProgress();
    else
      page.ResetProgress();
    this.PageAddedMethod(page);
  }

  internal void Insert(int index, PdfPageBase loadedPage)
  {
    PdfReferenceHolder element = new PdfReferenceHolder((IPdfWrapper) loadedPage);
    this.m_pages.Insert(index, loadedPage);
    if (loadedPage.Dictionary.ContainsKey("Parent"))
    {
      loadedPage.Dictionary.Items.Remove((PdfName) "Parent");
      loadedPage.Dictionary["Parent"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this);
    }
    if (this.m_pagesReferences.Contains((IPdfPrimitive) element))
      throw new ArgumentException("The page already exists in some section, it can't be contained by several sections", nameof (loadedPage));
    this.m_pagesReferences.Insert(index, (IPdfPrimitive) element);
    this.m_count.IntValue = this.Count;
  }

  internal int IndexOf(PdfPage page)
  {
    return this.m_pages.Contains((PdfPageBase) page) ? this.m_pages.IndexOf((PdfPageBase) page) : this.m_pagesReferences.IndexOf((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) page));
  }

  internal bool Contains(PdfPage page) => 0 <= this.IndexOf(page);

  internal void Remove(PdfPage page)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    this.m_pagesReferences.Remove((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) page));
    this.m_pages.Remove((PdfPageBase) page);
  }

  internal void RemoveAt(int index)
  {
    PdfPage pdfPage = this[index];
    this.m_pagesReferences.RemoveAt(index);
    this.m_pages.RemoveAt(index);
    pdfPage?.AssignSection((PdfSection) null);
  }

  internal void Clear()
  {
    for (int index = this.m_pages.Count - 1; index > -1; --index)
    {
      PdfPage page = this.m_pages[index] as PdfPage;
      this.Remove(page);
      page.Clear();
    }
    if (this.m_pages != null)
      this.m_pages.Clear();
    if (this.m_pagesReferences != null)
      this.m_pagesReferences.Clear();
    if (this.m_resources != null)
      this.m_resources.Clear();
    if (this.m_section != null)
      this.m_section.Clear();
    if (this.m_pagesCollection != null)
      this.m_pagesCollection.Clear();
    while (this.Count > 0)
      this.RemoveAt(this.Count - 1);
    this.m_pages = (List<PdfPageBase>) null;
    this.m_resources = (PdfDictionary) null;
    this.m_pagesReferences = (PdfArray) null;
    this.m_initialSettings = (PdfPageSettings) null;
    this.m_settings = (PdfPageSettings) null;
    this.m_parent = (PdfSectionCollection) null;
    this.m_document = (PdfDocumentBase) null;
    this.m_pagesCollection = (PdfSectionPageCollection) null;
    this.m_pageTemplate = (PdfSectionTemplate) null;
    this.m_section = (PdfDictionary) null;
  }

  public IEnumerator GetEnumerator() => (IEnumerator) new PdfSection.PdfPageEnumerator(this);

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_section;

  internal bool ContainsTemplates(PdfDocument document, PdfPage page, bool foreground)
  {
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    PdfPageTemplateElement[] documentTemplates1 = this.GetDocumentTemplates(document, page, true, foreground);
    PdfPageTemplateElement[] documentTemplates2 = this.GetDocumentTemplates(document, page, false, foreground);
    PdfPageTemplateElement[] sectionTemplates1 = this.GetSectionTemplates(page, true, foreground);
    PdfPageTemplateElement[] sectionTemplates2 = this.GetSectionTemplates(page, false, foreground);
    return documentTemplates1.Length > 0 || documentTemplates2.Length > 0 || sectionTemplates1.Length > 0 || sectionTemplates2.Length > 0;
  }

  internal void DrawTemplates(
    PdfPage page,
    PdfPageLayer layer,
    PdfDocument document,
    bool foreground)
  {
    if (layer == null)
      throw new ArgumentNullException(nameof (layer));
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    PdfPageTemplateElement[] documentTemplates1 = this.GetDocumentTemplates(document, page, true, foreground);
    PdfPageTemplateElement[] documentTemplates2 = this.GetDocumentTemplates(document, page, false, foreground);
    PdfPageTemplateElement[] sectionTemplates1 = this.GetSectionTemplates(page, true, foreground);
    PdfPageTemplateElement[] sectionTemplates2 = this.GetSectionTemplates(page, false, foreground);
    if (foreground)
    {
      this.DrawTemplates(layer, document, sectionTemplates1);
      this.DrawTemplates(layer, document, sectionTemplates2);
      this.DrawTemplates(layer, document, documentTemplates1);
      this.DrawTemplates(layer, document, documentTemplates2);
    }
    else
    {
      this.DrawTemplates(layer, document, documentTemplates1);
      this.DrawTemplates(layer, document, documentTemplates2);
      this.DrawTemplates(layer, document, sectionTemplates1);
      this.DrawTemplates(layer, document, sectionTemplates2);
    }
  }

  internal RectangleF GetActualBounds(PdfPage page, bool includeMargins)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    RectangleF actualBounds;
    if (this.Parent != null)
    {
      actualBounds = this.GetActualBounds(this.Parent.Document ?? throw new PdfDocumentException("The section should be added to the section collection before this operation"), page, includeMargins);
    }
    else
    {
      SizeF size = includeMargins ? this.PageSettings.GetActualSize() : this.PageSettings.Size;
      actualBounds = new RectangleF(new PointF(includeMargins ? this.PageSettings.Margins.Left : 0.0f, includeMargins ? this.PageSettings.Margins.Top : 0.0f), size);
    }
    return actualBounds;
  }

  internal RectangleF GetActualBounds(PdfDocument document, PdfPage page, bool includeMargins)
  {
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    RectangleF empty = RectangleF.Empty with
    {
      Size = includeMargins ? this.PageSettings.Size : this.PageSettings.GetActualSize()
    };
    float leftIndentWidth = this.GetLeftIndentWidth(document, page, includeMargins);
    float topIndentHeight = this.GetTopIndentHeight(document, page, includeMargins);
    float rightIndentWidth = this.GetRightIndentWidth(document, page, includeMargins);
    float bottomIndentHeight = this.GetBottomIndentHeight(document, page, includeMargins);
    empty.X += leftIndentWidth;
    empty.Y += topIndentHeight;
    empty.Width -= leftIndentWidth + rightIndentWidth;
    empty.Height -= topIndentHeight + bottomIndentHeight;
    return empty;
  }

  internal float GetLeftIndentWidth(PdfDocument document, PdfPage page, bool includeMargins)
  {
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    float num = includeMargins ? this.PageSettings.Margins.Left : 0.0f;
    float val1 = this.Template.GetLeft(page) != null ? this.Template.GetLeft(page).Width : 0.0f;
    float val2 = document.Template.GetLeft(page) != null ? document.Template.GetLeft(page).Width : 0.0f;
    return num + (this.Template.ApplyDocumentLeftTemplate ? Math.Max(val1, val2) : val1);
  }

  internal float GetTopIndentHeight(PdfDocument document, PdfPage page, bool includeMargins)
  {
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    float num = includeMargins ? this.PageSettings.Margins.Top : 0.0f;
    float val1 = this.Template.GetTop(page) != null ? this.Template.GetTop(page).Height : 0.0f;
    float val2 = document.Template.GetTop(page) != null ? document.Template.GetTop(page).Height : 0.0f;
    return num + (this.Template.ApplyDocumentTopTemplate ? Math.Max(val1, val2) : val1);
  }

  internal float GetRightIndentWidth(PdfDocument document, PdfPage page, bool includeMargins)
  {
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    float num = includeMargins ? this.PageSettings.Margins.Right : 0.0f;
    float val1 = this.Template.GetRight(page) != null ? this.Template.GetRight(page).Width : 0.0f;
    float val2 = document.Template.GetRight(page) != null ? document.Template.GetRight(page).Width : 0.0f;
    return num + (this.Template.ApplyDocumentRightTemplate ? Math.Max(val1, val2) : val1);
  }

  internal float GetBottomIndentHeight(PdfDocument document, PdfPage page, bool includeMargins)
  {
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    float num = includeMargins ? this.PageSettings.Margins.Bottom : 0.0f;
    float val1 = this.Template.GetBottom(page) != null ? this.Template.GetBottom(page).Height : 0.0f;
    float val2 = document.Template.GetBottom(page) != null ? document.Template.GetBottom(page).Height : 0.0f;
    return num + (this.Template.ApplyDocumentBottomTemplate ? Math.Max(val1, val2) : val1);
  }

  internal PointF PointToNativePdf(PdfPage page, PointF point)
  {
    RectangleF actualBounds = this.GetActualBounds(page, true);
    point.X += actualBounds.Left;
    point.Y = this.PageSettings.Height - (actualBounds.Top + point.Y);
    return point;
  }

  private void DrawTemplates(
    PdfPageLayer layer,
    PdfDocument document,
    PdfPageTemplateElement[] templates)
  {
    if (layer == null)
      throw new ArgumentNullException(nameof (layer));
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    if (templates == null || templates.Length <= 0)
      return;
    int index = 0;
    for (int length = templates.Length; index < length; ++index)
      templates[index].Draw(layer, document);
  }

  private PdfPageTemplateElement[] GetDocumentTemplates(
    PdfDocument document,
    PdfPage page,
    bool headers,
    bool foreground)
  {
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    List<PdfPageTemplateElement> pageTemplateElementList = new List<PdfPageTemplateElement>();
    if (headers)
    {
      if (this.Template.ApplyDocumentTopTemplate && document.Template.GetTop(page) != null && document.Template.GetTop(page).Foreground == foreground)
        pageTemplateElementList.Add(document.Template.GetTop(page));
      if (this.Template.ApplyDocumentBottomTemplate && document.Template.GetBottom(page) != null && document.Template.GetBottom(page).Foreground == foreground)
        pageTemplateElementList.Add(document.Template.GetBottom(page));
      if (this.Template.ApplyDocumentLeftTemplate && document.Template.GetLeft(page) != null && document.Template.GetLeft(page).Foreground == foreground)
        pageTemplateElementList.Add(document.Template.GetLeft(page));
      if (this.Template.ApplyDocumentRightTemplate && document.Template.GetRight(page) != null && document.Template.GetRight(page).Foreground == foreground)
        pageTemplateElementList.Add(document.Template.GetRight(page));
    }
    else if (this.Template.ApplyDocumentStamps)
    {
      int index = 0;
      for (int count = document.Template.Stamps.Count; index < count; ++index)
      {
        PdfPageTemplateElement stamp = document.Template.Stamps[index];
        if (stamp.Foreground == foreground)
          pageTemplateElementList.Add(stamp);
      }
    }
    return pageTemplateElementList.ToArray();
  }

  private PdfPageTemplateElement[] GetSectionTemplates(PdfPage page, bool headers, bool foreground)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    List<PdfPageTemplateElement> pageTemplateElementList = new List<PdfPageTemplateElement>();
    if (headers)
    {
      if (this.Template.GetTop(page) != null && this.Template.GetTop(page).Foreground == foreground)
        pageTemplateElementList.Add(this.Template.GetTop(page));
      if (this.Template.GetBottom(page) != null && this.Template.GetBottom(page).Foreground == foreground)
        pageTemplateElementList.Add(this.Template.GetBottom(page));
      if (this.Template.GetLeft(page) != null && this.Template.GetLeft(page).Foreground == foreground)
        pageTemplateElementList.Add(this.Template.GetLeft(page));
      if (this.Template.GetRight(page) != null && this.Template.GetRight(page).Foreground == foreground)
        pageTemplateElementList.Add(this.Template.GetRight(page));
    }
    else
    {
      int index = 0;
      for (int count = this.Template.Stamps.Count; index < count; ++index)
      {
        PdfPageTemplateElement stamp = this.Template.Stamps[index];
        if (stamp.Foreground == foreground)
          pageTemplateElementList.Add(stamp);
      }
    }
    return pageTemplateElementList.ToArray();
  }

  protected virtual void OnPageAdded(PageAddedEventArgs args)
  {
    if (this.PageAdded == null)
      return;
    this.PageAdded((object) this, args);
  }

  internal void SetProgress()
  {
    if (this.m_isProgressOn)
      return;
    foreach (PdfPageBase pdfPageBase in this)
      pdfPageBase.SetProgress();
    this.m_isProgressOn = true;
  }

  internal void ResetProgress()
  {
    if (!this.m_isProgressOn)
      return;
    foreach (PdfPageBase pdfPageBase in this)
      pdfPageBase.ResetProgress();
    this.m_isProgressOn = false;
  }

  internal void OnPageSaving(PdfPage page) => this.Parent.OnPageSaving(page);

  private PdfReferenceHolder CheckPresence(PdfPage page)
  {
    PdfReferenceHolder element = new PdfReferenceHolder((IPdfWrapper) page);
    bool flag = false;
    if (this.m_parent != null)
    {
      foreach (PdfSection pdfSection in this.Parent)
      {
        flag |= pdfSection.Contains(page);
        if (flag)
          break;
      }
    }
    else
      flag = this.m_pagesReferences.Contains((IPdfPrimitive) element);
    if (flag)
      throw new ArgumentException("The page already exists in some section, it can't be contained by several sections", nameof (page));
    return element;
  }

  private void SetPageSettings(PdfDictionary container, PdfPageSettings parentSettings)
  {
    if (container == null)
      throw new ArgumentNullException(nameof (container));
    if (parentSettings == null || this.PageSettings.Size != parentSettings.Size)
    {
      RectangleF rectangle = new RectangleF();
      rectangle = parentSettings == null || !(this.PageSettings.Size != parentSettings.Size) || !this.m_importedSection || this.PageSettings.Rotate == parentSettings.Rotate ? new RectangleF(this.PageSettings.Origin, this.PageSettings.Size) : new RectangleF(this.PageSettings.Origin, parentSettings.Size);
      container["MediaBox"] = (IPdfPrimitive) PdfArray.FromRectangle(rectangle);
    }
    if (parentSettings != null)
    {
      int rotate1 = (int) this.PageSettings.Rotate;
    }
    int num1 = 0;
    if (this.m_parent != null)
    {
      if (this.PageSettings.m_isRotation && !this.Document.PageSettings.m_isRotation)
        num1 = 90 * (int) this.PageSettings.Rotate;
      else if (!this.Document.PageSettings.m_isRotation)
      {
        int rotate2 = (int) this.PageSettings.Rotate;
        num1 = 90 * (int) this.PageSettings.Rotate;
      }
      else if (this.PageSettings.m_isRotation)
        num1 = 90 * (int) this.PageSettings.Rotate;
      else if (parentSettings != null)
        num1 = 90 * (int) parentSettings.Rotate;
    }
    else
      num1 = 90 * (int) this.PageSettings.Rotate;
    PdfNumber pdfNumber = new PdfNumber(num1);
    if (pdfNumber.IntValue != 0)
      container["Rotate"] = (IPdfPrimitive) pdfNumber;
    if (parentSettings != null && this.m_importedSection)
    {
      int rotate3 = (int) this.PageSettings.Rotate;
      if (container.ContainsKey("Kids") && PdfCrossTable.Dereference(container["Kids"]) is PdfArray pdfArray)
      {
        for (int index = 0; index < pdfArray.Count; ++index)
        {
          if (PdfCrossTable.Dereference(pdfArray[index]) is PdfDictionary pdfDictionary)
            pdfDictionary["Rotate"] = (IPdfPrimitive) pdfNumber;
        }
      }
    }
    if (parentSettings != null && this.PageSettings.Unit == parentSettings.Unit)
      return;
    float num2 = new PdfUnitConvertor().ConvertUnits(1f, this.PageSettings.Unit, PdfGraphicsUnit.Point);
    container["UserUnit"] = (IPdfPrimitive) new PdfNumber(num2);
  }

  private void Initialize()
  {
    this.m_pagesReferences = new PdfArray();
    this.m_section = new PdfDictionary();
    this.m_section.BeginSave += new SavePdfPrimitiveEventHandler(this.BeginSave);
    this.m_section.EndSave += new SavePdfPrimitiveEventHandler(this.EndSave);
    this.m_count = new PdfNumber(0);
    this.m_section["Count"] = (IPdfPrimitive) this.m_count;
    this.m_section["Type"] = (IPdfPrimitive) new PdfName("Pages");
    this.m_section["Kids"] = (IPdfPrimitive) this.m_pagesReferences;
  }

  internal PdfPageTransition GetTransitionSettings()
  {
    if (!this.m_isTransitionSaved)
    {
      if (this.m_settings.AssignTransition() == null)
      {
        if (this.Document.PageSettings.AssignTransition() != null)
          this.m_savedTransition = (PdfPageTransition) this.Document.PageSettings.Transition.Clone();
      }
      else if (this.Document.PageSettings.AssignTransition() == null)
      {
        this.m_savedTransition = (PdfPageTransition) this.m_settings.Transition.Clone();
      }
      else
      {
        this.m_savedTransition = new PdfPageTransition();
        PdfPageTransition transition1 = this.m_initialSettings.Transition;
        PdfPageTransition transition2 = this.Document.PageSettings.Transition;
        bool flag1 = (double) transition1.PageDuration == (double) this.m_settings.Transition.PageDuration;
        this.m_savedTransition.PageDuration = (double) transition2.PageDuration != (double) this.m_settings.Transition.PageDuration || flag1 ? transition1.PageDuration : transition2.PageDuration;
        bool flag2 = transition1.Dimension == this.m_settings.Transition.Dimension;
        this.m_savedTransition.Dimension = transition2.Dimension != this.m_settings.Transition.Dimension || flag2 ? transition1.Dimension : transition2.Dimension;
        bool flag3 = transition1.Direction == this.m_settings.Transition.Direction;
        this.m_savedTransition.Direction = transition2.Direction != this.m_settings.Transition.Direction || flag3 ? transition1.Direction : transition2.Direction;
        bool flag4 = transition1.Motion == this.m_settings.Transition.Motion;
        this.m_savedTransition.Motion = transition2.Motion != this.m_settings.Transition.Motion || flag4 ? transition1.Motion : transition2.Motion;
        bool flag5 = (double) transition1.Scale == (double) this.m_settings.Transition.Scale;
        this.m_savedTransition.Scale = (double) transition2.Scale != (double) this.m_settings.Transition.Scale || flag5 ? transition1.Scale : transition2.Scale;
        bool flag6 = transition1.Style == this.m_settings.Transition.Style;
        this.m_savedTransition.Style = transition2.Style != this.m_settings.Transition.Style || flag6 ? transition1.Style : transition2.Style;
        bool flag7 = (double) transition1.Duration == (double) this.m_settings.Transition.Duration;
        this.m_savedTransition.Duration = (double) transition2.Duration != (double) this.m_settings.Transition.Duration || flag7 ? transition1.Duration : transition2.Duration;
      }
    }
    return this.m_savedTransition;
  }

  internal void DropCropBox()
  {
    this.SetPageSettings(this.m_section, (PdfPageSettings) null);
    this.m_section["CropBox"] = this.m_section["MediaBox"];
  }

  private void PageAddedMethod(PdfPage page)
  {
    PageAddedEventArgs args = new PageAddedEventArgs(page);
    this.OnPageAdded(args);
    this.Parent?.Document.Pages.OnPageAdded(args);
    this.m_count.IntValue = this.Count;
  }

  private void BeginSave(object sender, SavePdfPrimitiveEventArgs e)
  {
    this.m_count.IntValue = this.Count;
    if (e.Writer.Document is PdfDocument document)
      this.SetPageSettings(this.m_section, document.PageSettings);
    else
      this.SetPageSettings(this.m_section, (PdfPageSettings) null);
  }

  private void EndSave(object sender, SavePdfPrimitiveEventArgs ars)
  {
    this.m_savedTransition = (PdfPageTransition) null;
  }

  private struct PdfPageEnumerator : IEnumerator
  {
    private PdfSection m_section;
    private int m_index;

    internal PdfPageEnumerator(PdfSection section)
    {
      this.m_section = section != null ? section : throw new ArgumentNullException(nameof (section));
      this.m_index = -1;
    }

    public object Current
    {
      get
      {
        this.CheckIndex();
        return (object) this.m_section[this.m_index];
      }
    }

    public bool MoveNext()
    {
      ++this.m_index;
      return this.m_index < this.m_section.Count;
    }

    public void Reset() => this.m_index = -1;

    private void CheckIndex()
    {
      if (this.m_index < 0 || this.m_index >= this.m_section.Count)
        throw new IndexOutOfRangeException();
    }
  }
}
