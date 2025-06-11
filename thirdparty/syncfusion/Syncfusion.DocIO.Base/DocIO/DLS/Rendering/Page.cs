// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Rendering.Page
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Layouting;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS.Rendering;

internal class Page
{
  private LayoutedWidgetList m_pageWidgets = new LayoutedWidgetList();
  private LayoutedWidgetList m_footnoteWidgets = new LayoutedWidgetList();
  private LayoutedWidgetList m_lineNumberWidgets = new LayoutedWidgetList();
  private LayoutedWidgetList m_endnoteWidgets = new LayoutedWidgetList();
  private List<Syncfusion.Layouting.TrackChangesMarkups> m_trackChangesMarkups = new List<Syncfusion.Layouting.TrackChangesMarkups>();
  private List<int> m_endNotesectionIndex = new List<int>();
  private List<int> m_footNotesectionIndex = new List<int>();
  private WPageSetup m_pageSetup;
  private WHeadersFooters m_headersFooters;
  private IWSection m_docSection;
  private int m_iNumber;
  private Image m_backgroundImage;
  private Color m_backgroundColor = Color.Empty;
  private List<IWField> m_cachedFields = new List<IWField>();
  private LayoutedWidgetList m_behindWidgets;
  private int m_numberOfBehindShapeWidgetsInHeader;
  private int m_numberOfBehindShapeWidgetsInFooter;

  public LayoutedWidgetList PageWidgets => this.m_pageWidgets;

  internal IWSection DocSection => this.m_docSection;

  internal Image BackgroundImage => this.m_backgroundImage;

  internal Color BackgroundColor => this.m_backgroundColor;

  internal LayoutedWidgetList FootnoteWidgets => this.m_footnoteWidgets;

  internal LayoutedWidgetList LineNumberWidgets => this.m_lineNumberWidgets;

  internal LayoutedWidgetList EndnoteWidgets => this.m_endnoteWidgets;

  internal List<Syncfusion.Layouting.TrackChangesMarkups> TrackChangesMarkups
  {
    get
    {
      if (this.m_trackChangesMarkups == null)
        this.m_trackChangesMarkups = new List<Syncfusion.Layouting.TrackChangesMarkups>();
      return this.m_trackChangesMarkups;
    }
    set => this.m_trackChangesMarkups = value;
  }

  internal LayoutedWidgetList BehindWidgets
  {
    get
    {
      if (this.m_behindWidgets == null)
        this.m_behindWidgets = new LayoutedWidgetList();
      return this.m_behindWidgets;
    }
  }

  internal List<int> EndNoteSectionIndex => this.m_endNotesectionIndex;

  internal List<int> FootNoteSectionIndex => this.m_footNotesectionIndex;

  public WPageSetup Setup => this.m_pageSetup;

  public int Number
  {
    get => this.m_iNumber;
    set => this.m_iNumber = value;
  }

  internal int NumberOfBehindWidgetsInHeader
  {
    get => this.m_numberOfBehindShapeWidgetsInHeader;
    set => this.m_numberOfBehindShapeWidgetsInHeader = value;
  }

  internal int NumberOfBehindWidgetsInFooter
  {
    get => this.m_numberOfBehindShapeWidgetsInFooter;
    set => this.m_numberOfBehindShapeWidgetsInFooter = value;
  }

  internal bool SwapMargins
  {
    get
    {
      return this.m_docSection.Document.DOP.MirrorMargins && this.m_docSection.PageSetup.Orientation == PageOrientation.Portrait && DocumentLayouter.PageNumber % 2 == 0;
    }
  }

  public Page(IWSection section, int iNumber)
  {
    this.m_docSection = section;
    this.m_pageSetup = section.PageSetup;
    this.m_headersFooters = section.HeadersFooters;
    this.m_iNumber = iNumber;
    IWordDocument document = (IWordDocument) section.Document;
    this.m_backgroundImage = document.BackgroundImage;
    this.m_backgroundColor = document.Background.Color;
  }

  public void InitLayoutInfo()
  {
    for (int index = 0; index < this.m_pageWidgets.Count; ++index)
      this.m_pageWidgets[index].InitLayoutInfoAll();
    for (int index = 0; index < this.m_footnoteWidgets.Count; ++index)
    {
      LayoutedWidget footnoteWidget = this.m_footnoteWidgets[index];
      WTextBody wtextBody = footnoteWidget.Widget is WTextBody ? footnoteWidget.Widget as WTextBody : (footnoteWidget.Widget is SplitWidgetContainer ? (footnoteWidget.Widget as SplitWidgetContainer).RealWidgetContainer as WTextBody : (WTextBody) null);
      if (wtextBody != null && wtextBody.Owner is WFootnote)
        (wtextBody.Owner as WFootnote).IsLayouted = false;
      footnoteWidget.InitLayoutInfoAll();
    }
    for (int index = 0; index < this.m_endnoteWidgets.Count; ++index)
    {
      LayoutedWidget endnoteWidget = this.m_endnoteWidgets[index];
      WTextBody wtextBody = endnoteWidget.Widget is WTextBody ? endnoteWidget.Widget as WTextBody : (endnoteWidget.Widget is SplitWidgetContainer ? (endnoteWidget.Widget as SplitWidgetContainer).RealWidgetContainer as WTextBody : (WTextBody) null);
      if (wtextBody != null && wtextBody.Owner is WFootnote)
        (wtextBody.Owner as WFootnote).IsLayouted = false;
      endnoteWidget.InitLayoutInfoAll();
    }
    if (this.m_pageWidgets != null)
    {
      this.m_pageWidgets.Clear();
      this.m_pageWidgets = (LayoutedWidgetList) null;
    }
    if (this.m_footnoteWidgets != null)
    {
      this.m_footnoteWidgets.Clear();
      this.m_footnoteWidgets = (LayoutedWidgetList) null;
    }
    if (this.m_endnoteWidgets != null)
    {
      this.m_endnoteWidgets.Clear();
      this.m_endnoteWidgets = (LayoutedWidgetList) null;
    }
    if (this.m_endNotesectionIndex != null)
    {
      this.m_endNotesectionIndex.Clear();
      this.m_endNotesectionIndex = (List<int>) null;
    }
    if (this.m_footNotesectionIndex != null)
    {
      this.m_footNotesectionIndex.Clear();
      this.m_footNotesectionIndex = (List<int>) null;
    }
    if (this.m_cachedFields != null)
    {
      this.m_cachedFields.Clear();
      this.m_cachedFields = (List<IWField>) null;
    }
    if (this.m_behindWidgets == null)
      return;
    this.m_behindWidgets.Clear();
    this.m_behindWidgets = (LayoutedWidgetList) null;
  }

  public void UpdateFieldsNumPages(int numPages)
  {
    for (int index = 0; index < this.m_cachedFields.Count; ++index)
    {
      IWField cachedField = this.m_cachedFields[index];
      if (cachedField is WField && cachedField.FieldType == FieldType.FieldNumPages)
        (cachedField as WField).FieldResult = numPages.ToString();
    }
  }

  public void AddCachedFields(IWField field) => this.m_cachedFields.Add(field);

  protected internal RectangleF GetHeaderArea()
  {
    float leftMargin = (double) this.m_pageSetup.Margins.Left != -0.05000000074505806 ? this.m_pageSetup.Margins.Left : 0.0f;
    float rightMargin = (double) this.m_pageSetup.Margins.Right != -0.05000000074505806 ? this.m_pageSetup.Margins.Right : 0.0f;
    float y = (double) this.m_pageSetup.HeaderDistance != -0.05000000074505806 ? this.m_pageSetup.HeaderDistance : 36f;
    float width = this.m_pageSetup.PageSize.Width;
    float height = this.m_pageSetup.PageSize.Height;
    if (this.SwapMargins)
      this.UpdateMirrorMargins(ref leftMargin, ref rightMargin);
    if ((double) this.m_pageSetup.Margins.Gutter > 0.0 && this.m_pageSetup.Document.DOP.GutterAtTop && this.m_docSection.PageSetup.Orientation != this.m_pageSetup.Document.Sections[0].PageSetup.Orientation || !this.m_pageSetup.Document.DOP.GutterAtTop && this.m_docSection.PageSetup.Orientation == this.m_pageSetup.Document.Sections[0].PageSetup.Orientation)
    {
      if (this.SwapMargins)
        rightMargin += this.m_pageSetup.Margins.Gutter;
      else
        leftMargin += this.m_pageSetup.Margins.Gutter;
    }
    return new RectangleF(leftMargin, y, width - (leftMargin + rightMargin), height / 2f + y);
  }

  protected internal RectangleF GetFooterArea()
  {
    float leftMargin = (double) this.m_pageSetup.Margins.Left != -0.05000000074505806 ? this.m_pageSetup.Margins.Left : 0.0f;
    float rightMargin = (double) this.m_pageSetup.Margins.Right != -0.05000000074505806 ? this.m_pageSetup.Margins.Right : 0.0f;
    float num = (double) this.m_pageSetup.FooterDistance != -0.05000000074505806 ? this.m_pageSetup.FooterDistance : 36f;
    float width = this.m_pageSetup.PageSize.Width;
    float height = this.m_pageSetup.PageSize.Height;
    if (this.SwapMargins)
      this.UpdateMirrorMargins(ref leftMargin, ref rightMargin);
    if ((double) this.m_pageSetup.Margins.Gutter > 0.0 && this.m_pageSetup.Document.DOP.GutterAtTop && this.m_docSection.PageSetup.Orientation != this.m_pageSetup.Document.Sections[0].PageSetup.Orientation || !this.m_pageSetup.Document.DOP.GutterAtTop && this.m_docSection.PageSetup.Orientation == this.m_pageSetup.Document.Sections[0].PageSetup.Orientation)
    {
      if (this.SwapMargins)
        rightMargin += this.m_pageSetup.Margins.Gutter;
      else
        leftMargin += this.m_pageSetup.Margins.Gutter;
    }
    return new RectangleF(leftMargin, height - num, width - (leftMargin + rightMargin), height / 2f);
  }

  private void UpdateGutterValue(ref float margin, Column column)
  {
    if (this.m_docSection.Columns[0] == column)
      margin += this.m_pageSetup.Margins.Gutter;
    else
      margin += this.m_pageSetup.Margins.Gutter / (float) this.m_docSection.Columns.Count;
  }

  private void UpdateMirrorMargins(ref float leftMargin, ref float rightMargin)
  {
    MarginsF margins = this.m_docSection.PageSetup.Margins;
    leftMargin = (double) margins.Right != -0.05000000074505806 ? margins.Right : 0.0f;
    rightMargin = (double) margins.Left != -0.05000000074505806 ? margins.Left : 0.0f;
  }

  protected internal RectangleF GetColumnArea(
    Column column,
    float prevWidth,
    bool isNeedtoAdjustFooter)
  {
    MarginsF margins = this.m_pageSetup.Margins;
    float width1 = this.m_pageSetup.PageSize.Width;
    float height = this.m_pageSetup.PageSize.Height;
    float val1_1 = Math.Abs((double) margins.Top != -0.05000000074505806 ? margins.Top : 0.0f);
    float num1 = (double) margins.Left != -0.05000000074505806 ? margins.Left : 0.0f;
    float num2 = (double) margins.Right != -0.05000000074505806 ? margins.Right : 0.0f;
    float val1_2 = Math.Abs((double) margins.Bottom != -0.05000000074505806 ? margins.Bottom : 0.0f);
    if (this.SwapMargins)
      this.UpdateMirrorMargins(ref num1, ref num2);
    if (this.SwapMargins)
      this.UpdateMirrorMargins(ref num1, ref num2);
    float val2_1 = this.m_pageWidgets[0].ChildWidgets.Count == 0 || (double) margins.Top < 0.0 && (double) val1_1 > 0.0 ? 0.0f : this.m_pageWidgets[0].Bounds.Height + ((double) this.m_pageSetup.HeaderDistance != -0.05000000074505806 ? this.m_pageSetup.HeaderDistance : 36f);
    float val2_2 = !isNeedtoAdjustFooter ? (this.m_pageWidgets[1].ChildWidgets.Count == 0 || (double) margins.Bottom < 0.0 && (double) val1_2 > 0.0 ? 0.0f : this.m_pageWidgets[1].Bounds.Height + ((double) this.m_pageSetup.FooterDistance != -0.05000000074505806 ? this.m_pageSetup.FooterDistance : 36f)) : (this.m_pageWidgets[1].ChildWidgets.Count == 0 || (double) margins.Bottom < 0.0 && (double) val1_2 > 0.0 ? 0.0f : this.m_pageWidgets[1].Bounds.Height);
    float width2 = this.m_docSection.Columns.Count <= 1 ? this.m_docSection.PageSetup.ClientWidth : (column == null ? width1 - (num1 + num2) : column.Width);
    if ((double) this.m_pageSetup.Margins.Gutter > 0.0 && this.m_docSection.Columns.Count > 0 && this.m_docSection.PageSetup.Orientation != this.m_pageSetup.Document.Sections[0].PageSetup.Orientation)
    {
      if (this.m_pageSetup.Document.DOP.GutterAtTop)
      {
        if (this.SwapMargins)
          this.UpdateGutterValue(ref num2, column);
        else
          this.UpdateGutterValue(ref num1, column);
        width2 -= this.m_pageSetup.Margins.Gutter / (float) this.m_docSection.Columns.Count;
      }
      else
        val1_2 += this.m_pageSetup.Margins.Gutter;
    }
    else if ((double) this.m_pageSetup.Margins.Gutter > 0.0 && this.m_pageSetup.Document.DOP.GutterAtTop)
      val1_1 += this.m_pageSetup.Margins.Gutter;
    else if ((double) this.m_pageSetup.Margins.Gutter > 0.0 && this.m_docSection.Columns.Count > 0)
    {
      if (this.SwapMargins)
        this.UpdateGutterValue(ref num2, column);
      else
        this.UpdateGutterValue(ref num1, column);
      width2 -= this.m_pageSetup.Margins.Gutter / (float) this.m_docSection.Columns.Count;
    }
    return new RectangleF(num1 + prevWidth, Math.Max(val1_1, val2_1), width2, height - (Math.Max(val1_1, val2_1) + Math.Max(val1_2, val2_2)));
  }

  protected internal RectangleF GetColumnArea(
    int columnIndex,
    ref float prevColumnsWidth,
    bool isNeedtoAdjustFooter)
  {
    Column column = this.m_docSection.Columns.Count > columnIndex ? this.m_docSection.Columns[columnIndex] : (Column) null;
    RectangleF columnArea = this.GetColumnArea(column, prevColumnsWidth, isNeedtoAdjustFooter);
    if (column != null)
      prevColumnsWidth += column.Width + column.Space;
    return columnArea;
  }

  protected internal RectangleF GetSectionArea(Column column, float prevWidth)
  {
    MarginsF margins = this.m_docSection.PageSetup.Margins;
    float width1 = this.m_docSection.PageSetup.PageSize.Width;
    float height = this.m_docSection.PageSetup.PageSize.Height;
    float y = Math.Abs((double) margins.Top != -0.05000000074505806 ? margins.Top : 0.0f);
    float leftMargin = (double) margins.Left != -0.05000000074505806 ? margins.Left : 0.0f;
    float rightMargin = (double) margins.Right != -0.05000000074505806 ? margins.Right : 0.0f;
    float num = Math.Abs((double) margins.Bottom != -0.05000000074505806 ? margins.Bottom : 0.0f);
    if (this.SwapMargins)
      this.UpdateMirrorMargins(ref leftMargin, ref rightMargin);
    if (this.SwapMargins)
      this.UpdateMirrorMargins(ref leftMargin, ref rightMargin);
    float width2 = this.m_docSection.Columns.Count <= 1 ? this.m_docSection.PageSetup.ClientWidth : (column == null ? width1 - (leftMargin + rightMargin) : column.Width);
    if ((double) this.m_pageSetup.Margins.Gutter > 0.0 && this.m_docSection.PageSetup.Orientation != this.m_pageSetup.Document.Sections[0].PageSetup.Orientation)
    {
      if (this.m_pageSetup.Document.DOP.GutterAtTop)
      {
        if (this.SwapMargins)
          rightMargin += this.m_pageSetup.Margins.Gutter;
        else
          leftMargin += this.m_pageSetup.Margins.Gutter;
        width2 -= margins.Gutter;
      }
      else
        num += margins.Gutter;
    }
    else if (this.m_docSection.Document.DOP.GutterAtTop)
    {
      y += margins.Gutter;
    }
    else
    {
      if (this.SwapMargins)
        rightMargin += this.m_pageSetup.Margins.Gutter;
      else
        leftMargin += this.m_pageSetup.Margins.Gutter;
      width2 -= margins.Gutter;
    }
    return new RectangleF(leftMargin + prevWidth, y, width2, height - (y + num));
  }

  protected internal RectangleF GetSectionArea(
    int columnIndex,
    ref float prevColumnsWidth,
    bool isNextSection,
    bool isSplittedWidget)
  {
    int num = this.m_docSection.Document.Sections.IndexOf(this.m_docSection);
    if (!isSplittedWidget)
      --num;
    if (this.m_docSection.Document.Sections.Count - 1 > num && columnIndex == 0 && !isNextSection)
      this.m_docSection = (IWSection) this.m_docSection.Document.Sections[num + 1];
    Column column = this.m_docSection.Columns.Count > columnIndex ? this.m_docSection.Columns[columnIndex] : (Column) null;
    RectangleF sectionArea = this.GetSectionArea(column, prevColumnsWidth);
    if (column != null)
      prevColumnsWidth += column.Width + column.Space;
    return sectionArea;
  }
}
