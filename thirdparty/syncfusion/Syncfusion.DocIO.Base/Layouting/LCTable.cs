// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.LCTable
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.DLS.Rendering;
using Syncfusion.Office;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class LCTable : LayoutContext
{
  private const float DEF_MIN_WIDTH = 16f;
  private bool m_bHeaderRepeat;
  private bool isRowMoved;
  private int m_currHeaderRowIndex = -1;
  private int m_currRowIndex = -1;
  private int m_currColIndex = -1;
  private LayoutedWidget m_currRowLW;
  private LayoutedWidget m_currCellLW;
  protected bool m_bAtLastOneCellFitted;
  private SplitWidgetContainer[] m_splitedCells;
  private LayoutState m_blastRowState;
  private SplitTableWidget m_spitTableWidget;
  private WTable m_table;
  private bool m_isTableSplitted;
  private LayoutArea m_rowLayoutArea;
  private List<LayoutedWidget> m_verticallyMergeStartLW = new List<LayoutedWidget>();
  private float m_headerRowHeight;
  private float m_verticallyMergedCellFootnoteHeight;
  private RectangleF TableClientActiveArea;
  private float LayoutedHeaderRowHeight;

  private bool IsFirstItemInPage
  {
    get => ((int) this.m_bFlags & 128 /*0x80*/) >> 7 != 0;
    set
    {
      this.m_bFlags = (byte) ((int) this.m_bFlags & (int) sbyte.MaxValue | (value ? 1 : 0) << 7);
    }
  }

  public ILayoutInfo TableLayoutInfo => this.TableWidget.LayoutInfo;

  public ITableWidget TableWidget => this.m_widget as ITableWidget;

  public int CurrRowIndex => this.m_bHeaderRepeat ? this.m_currHeaderRowIndex : this.m_currRowIndex;

  internal float LeftPad
  {
    get
    {
      float leftPad = (((IWidget) this.m_table.Rows[0].Cells[0]).LayoutInfo as CellLayoutInfo).Paddings.Left + (((IWidget) this.m_table.Rows[0].Cells[0]).LayoutInfo as CellLayoutInfo).Margins.Left;
      if ((double) this.m_table.TableFormat.CellSpacing > 0.0)
        leftPad += this.m_table.TableFormat.Borders.Left.LineWidth;
      return leftPad;
    }
  }

  public LCTable(SplitTableWidget splitWidget, ILCOperator lcOperator, bool isForceFitLayout)
    : base((IWidget) splitWidget.TableWidget, lcOperator, isForceFitLayout)
  {
    this.m_bHeaderRepeat = splitWidget.TableWidget is WTable && !(splitWidget.TableWidget as WTable).IsInCell;
    this.m_currRowIndex = splitWidget.StartRowNumber - (this.m_bHeaderRepeat ? 1 : 2);
    this.m_spitTableWidget = splitWidget;
    this.m_isTableSplitted = true;
  }

  public LCTable(ITableWidget table, ILCOperator lcOperator, bool isForceFitLayout)
    : base((IWidget) table, lcOperator, isForceFitLayout)
  {
  }

  private new Entity GetBaseEntity(Entity entity)
  {
    Entity baseEntity = entity;
    while (baseEntity.Owner != null)
    {
      baseEntity = baseEntity.Owner;
      switch (baseEntity)
      {
        case WSection _:
        case HeaderFooter _:
        case WTableCell _:
        case WTextBox _:
        case Shape _:
        case GroupShape _:
          return baseEntity;
        default:
          continue;
      }
    }
    return baseEntity;
  }

  public override LayoutedWidget Layout(RectangleF rect)
  {
    this.m_table = this.m_widget as WTable;
    if ((this.m_lcOperator as Layouter).IsNeedToRelayoutTable)
      (this.m_lcOperator as Layouter).IsNeedToRelayoutTable = false;
    if (this.GetLayoutedFloatingTable())
    {
      this.m_ltState = LayoutState.Fitted;
      return this.m_ltWidget;
    }
    this.IsFirstItemInPage = this.IsForceFitLayout;
    bool isSplittedTable = (this.TableLayoutInfo as ITableLayoutInfo).IsSplittedTable;
    MarginsF marginsF = this.InitializePageMargins();
    if (this.m_table.TableFormat.WrapTextAround && !this.IsInTextBoxOrShape(this.m_table))
    {
      this.TableClientActiveArea = rect;
      Entity baseEntity = this.GetBaseEntity((Entity) this.m_table);
      float height = (this.m_lcOperator as Layouter).ClientLayoutArea.Height;
      if (baseEntity is HeaderFooter)
      {
        height = (baseEntity.Owner as WSection).PageSetup.PageSize.Height;
        rect.Height = height;
      }
      float x = rect.X;
      float y = rect.Y;
      if (this.m_table.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 && !this.m_table.IsInCell && !(this.TableLayoutInfo as ITableLayoutInfo).IsSplittedTable && this.m_table.TableFormat.WrapTextAround)
      {
        if (this.m_table.TableFormat.Positioning.VertRelationTo == VerticalRelation.Page && this.m_table.TableFormat.Positioning.VertPositionAbs == VerticalPosition.None)
          rect.Height = this.m_table.TableFormat.Positioning.VertPosition - (this.m_lcOperator as Layouter).ClientLayoutArea.Y + (this.m_lcOperator as Layouter).ClientLayoutArea.Height;
        else if (this.m_table.TableFormat.Positioning.VertRelationTo == VerticalRelation.Margin && this.m_table.TableFormat.Positioning.VertPositionAbs == VerticalPosition.None)
          rect.Height = this.m_table.TableFormat.Positioning.VertPosition + (this.m_lcOperator as Layouter).ClientLayoutArea.Height;
      }
      if (!(this.TableLayoutInfo as ITableLayoutInfo).IsSplittedTable)
      {
        if (this.m_table.IsInCell)
        {
          if ((double) rect.Height <= (double) Math.Abs(this.m_table.Rows[0].Height) && this.m_table.GetOwnerTableCell().OwnerRow.HeightType == TableRowHeightType.Exactly)
            rect.Y = y;
          else if (this.m_table.TableFormat.Positioning.VertPositionAbs == VerticalPosition.None)
            rect.Y += this.m_table.TableFormat.Positioning.VertPosition;
          if ((double) rect.Y < (double) (this.m_table.GetOwnerTableCell().m_layoutInfo as CellLayoutInfo).CellContentLayoutingBounds.Top || this.m_table.TableFormat.Positioning.VertRelationTo == VerticalRelation.Margin && this.m_table.TableFormat.Positioning.VertPositionAbs == VerticalPosition.Top)
            rect.Y = (this.m_table.GetOwnerTableCell().m_layoutInfo as CellLayoutInfo).CellContentLayoutingBounds.Top;
        }
        else if (this.m_table.TableFormat.Positioning.VertRelationTo == VerticalRelation.Paragraph)
        {
          if ((double) this.m_table.TableFormat.Positioning.VertPosition < 0.0 && (double) this.GetTableHeight() > (double) rect.Height && this.IsTableMoveToNextPage((TextBodyItem) this.m_table) && !this.IsFirstItemInPage)
            rect.Y += rect.Height;
          else if (this.m_table.TableFormat.Positioning.VertPositionAbs == VerticalPosition.None)
            rect.Y += this.m_table.TableFormat.Positioning.VertPosition - (this.m_table.TableFormat.Bidi || this.m_table.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 || (double) (this.m_lcOperator as Layouter).WrappingDifference == -3.4028234663852886E+38 || (double) (this.m_lcOperator as Layouter).WrappingDifference < 0.0 ? 0.0f : (this.m_lcOperator as Layouter).WrappingDifference);
        }
        else if (this.m_table.TableFormat.Positioning.VertPositionAbs != VerticalPosition.None)
        {
          if ((this.m_table.TableFormat.Positioning.VertPositionAbs == VerticalPosition.Top || this.m_table.TableFormat.Positioning.VertPositionAbs == VerticalPosition.Inside) && this.m_table.TableFormat.Positioning.VertRelationTo == VerticalRelation.Margin && marginsF != null)
            rect.Y = marginsF.Top + (this.m_table.Document.DOP.GutterAtTop ? marginsF.Gutter : 0.0f);
        }
        else if ((double) this.m_table.TableFormat.Positioning.VertPosition != 0.0)
          rect.Y = this.m_table.IsInCell || this.m_table.TableFormat.Positioning.VertRelationTo != VerticalRelation.Margin || marginsF == null ? this.m_table.TableFormat.Positioning.VertPosition : (float) ((double) marginsF.Top + (double) this.m_table.TableFormat.Positioning.VertPosition + (this.m_table.Document.DOP.GutterAtTop ? (double) marginsF.Gutter : 0.0));
        if ((double) rect.Y < 0.0 && !(this.m_lcOperator as Layouter).IsLayoutingHeaderFooter)
          rect.Y = y;
        if (this.m_table.TableFormat.WrapTextAround && this.m_table.Document.DOP.Dop2000.Copts.DontBreakWrappedTables && this.IsFirstItemInPage && !this.m_table.IsInCell && baseEntity is WSection)
          rect.Height = (baseEntity as WSection).PageSetup.PageSize.Height;
        else if (this.m_table.OwnerTextBody != null && !(this.m_lcOperator as Layouter).IsLayoutingHeaderFooter && this.m_table.TableFormat.WrapTextAround && this.m_table.TableFormat.Positioning.VertPositionAbs == VerticalPosition.Bottom && (this.m_table.TableFormat.Positioning.VertRelationTo == VerticalRelation.Margin || this.m_table.TableFormat.Positioning.VertRelationTo == VerticalRelation.Page) && !this.m_table.Document.DOP.Dop2000.Copts.DontBreakWrappedTables)
        {
          rect.Height = this.TableClientActiveArea.Height;
        }
        else
        {
          rect.Height += y - rect.Y;
          if (this.m_table.TableFormat.Positioning.VertPositionAbs != VerticalPosition.None && this.m_table.TableFormat.Positioning.VertRelationTo == VerticalRelation.Page && !this.m_table.IsInCell)
            rect.Height = height;
        }
      }
      bool flag = false;
      if (this.m_table.TableFormat.Positioning.HorizRelationTo == HorizontalRelation.Column && this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Left)
      {
        rect.X += this.m_table.TableFormat.Positioning.HorizPosition;
        flag = true;
      }
      if (this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Left && (!(baseEntity is WSection) || (baseEntity as WSection).Columns.Count <= 1))
      {
        if (this.m_table.IsInCell)
        {
          CellLayoutInfo layoutInfo = ((IWidget) this.m_table.GetOwnerTableCell()).LayoutInfo as CellLayoutInfo;
          if ((double) rect.Right + (double) layoutInfo.Margins.Right < (double) x + (double) this.m_table.Width + (double) this.m_table.TableFormat.Positioning.HorizPosition)
          {
            float num = rect.Right + layoutInfo.Margins.Right - this.m_table.Width;
            if ((double) x - (double) layoutInfo.Margins.Left > (double) num)
              num = x - layoutInfo.Margins.Left;
            rect.X = num;
          }
          else if (!flag)
            rect.X += this.m_table.TableFormat.Positioning.HorizPosition;
        }
        else
          rect.X = this.m_table.TableFormat.Positioning.HorizRelationTo != HorizontalRelation.Margin && this.m_table.TableFormat.Positioning.HorizRelationTo != HorizontalRelation.Column || marginsF == null ? this.m_table.TableFormat.Positioning.HorizPosition : Layouter.GetLeftMargin(this.m_table.GetOwnerSection((Entity) this.m_table) as WSection) + this.m_table.TableFormat.Positioning.HorizPosition;
      }
      if (this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Inside && this.m_table.TableFormat.Positioning.HorizRelationTo == HorizontalRelation.Page)
        rect.X = 0.0f;
    }
    if (this.m_table.OwnerTextBody.Owner is WTextBox && (this.m_table.OwnerTextBody.Owner as WTextBox).TextBoxFormat.AutoFit)
    {
      float tableHeight = this.GetTableHeight();
      if ((double) tableHeight > (double) rect.Height)
        rect.Height = tableHeight;
    }
    this.CreateTableClientArea(ref rect);
    if (this.m_table.IsInCell)
    {
      float num = (this.m_table.GetOwnerTableCell().m_layoutInfo as CellLayoutInfo).CellContentLayoutingBounds.Left - (this.m_table.GetOwnerTableCell().m_layoutInfo as CellLayoutInfo).Margins.Left;
      if ((double) rect.X < (double) num)
        rect.X = num;
      this.CreateLayoutArea(rect);
    }
    SizeF size = this.DrawingContext.MeasureString(" ", this.m_table.Rows[0].Cells[0].LastParagraph != null ? this.m_table.Rows[0].Cells[0].LastParagraph.BreakCharacterFormat.GetFontToRender(FontScriptType.English) : this.m_table.Rows[0].Cells[0].CharacterFormat.GetFontToRender(FontScriptType.English), (StringFormat) null) with
    {
      Width = this.m_table.Width
    };
    float num1 = this.m_table.Rows.Count > 0 ? this.m_table.Rows[0].Height : 0.0f;
    if ((double) num1 > (double) size.Height)
      size.Height = num1;
    if (this.m_table.TableFormat.WrapTextAround && this.m_table.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 && (this.m_table.TableFormat.Positioning.HorizRelationTo == HorizontalRelation.Margin || this.m_table.TableFormat.Positioning.HorizRelationTo == HorizontalRelation.Column || this.m_table.TableFormat.Positioning.HorizRelationTo == HorizontalRelation.Page) && (this.m_table.TableFormat.Positioning.VertRelationTo == VerticalRelation.Page || this.m_table.TableFormat.Positioning.VertRelationTo == VerticalRelation.Paragraph || this.m_table.TableFormat.Positioning.VertRelationTo == VerticalRelation.Margin) && !this.IsFirstItemInPage)
      size.Height = this.GetTableHeight();
    RectangleF tableClientArea = rect;
    this.AdjustClientAreaBasedOnTextWrap(size, ref rect);
    bool isWrappedTable = this.m_table.TableFormat.WrapTextAround && (double) rect.Y != (double) tableClientArea.Y && this.m_table.TableFormat.Positioning.VertRelationTo == VerticalRelation.Page;
    bool isFitRowByUsingVerticalDistance = false;
    this.CreateLayoutedWidget(rect.Location);
    int currRowIndex = this.m_currRowIndex;
    do
    {
      IWidget rowLayoutedWidget = this.CreateRowLayoutedWidget();
      if (rowLayoutedWidget == null || rowLayoutedWidget.LayoutInfo.IsSkip)
      {
        if (this.m_bAtLastOneCellFitted)
          this.m_ltState = LayoutState.Fitted;
        if (rowLayoutedWidget != null && this.m_table.Rows[this.CurrRowIndex].IsDeleteRevision)
        {
          if (!this.m_table.Document.RevisionOptions.ShowDeletedText)
          {
            this.m_ltState = LayoutState.Unknown;
            goto label_77;
          }
        }
        else
          break;
      }
      this.LayoutRow(rowLayoutedWidget, ref isFitRowByUsingVerticalDistance);
      if (DocumentLayouter.IsUpdatingTOC && DocumentLayouter.IsEndUpdateTOC)
        return (LayoutedWidget) null;
      if (this.IsAdjacentCellHasFootnote())
        this.FootnoteRowLayouting();
      this.CommitRow();
      if ((rowLayoutedWidget as WTableRow).IsDeleteRevision && (rowLayoutedWidget as WTableRow).Index != (this.Widget as WTable).LastRow.Index)
        this.m_ltState = LayoutState.Unknown;
      ((this.m_currRowLW.Widget as WTableRow).m_layoutInfo as RowLayoutInfo).IsFootnoteReduced = false;
      ((this.m_currRowLW.Widget as WTableRow).m_layoutInfo as RowLayoutInfo).IsFootnoteSplitted = false;
      if (isFitRowByUsingVerticalDistance && this.m_ltWidget.ChildWidgets.Count == 1)
        this.m_layoutArea = new LayoutArea(this.m_layoutArea.ClientActiveArea with
        {
          Height = 0.0f
        });
label_77:;
    }
    while (this.m_ltState == LayoutState.Unknown && !(this.m_lcOperator as Layouter).IsNeedToRelayoutTable || this.IsTableRelayout(ref tableClientArea, currRowIndex));
    this.UpdateTableLWBounds(isWrappedTable, isSplittedTable);
    return this.m_ltWidget;
  }

  private void ClearVerticalMergeStartLW()
  {
    foreach (LayoutedWidget childWidget in (List<LayoutedWidget>) this.m_currRowLW.ChildWidgets)
    {
      if (this.m_verticallyMergeStartLW == null || this.m_verticallyMergeStartLW.Count == 0)
        break;
      if (this.m_verticallyMergeStartLW.Contains(childWidget))
        this.m_verticallyMergeStartLW.Remove(childWidget);
    }
  }

  private bool IsLayoutedFloatingTableInTextBodyItems()
  {
    int count = (this.m_lcOperator as Layouter).MaintainltWidget.ChildWidgets.Count;
    while (count > 0)
    {
      LayoutedWidget childWidget1 = (this.m_lcOperator as Layouter).MaintainltWidget.ChildWidgets[--count];
      for (int index1 = childWidget1.ChildWidgets.Count - 1; index1 >= 0; --index1)
      {
        LayoutedWidget childWidget2 = childWidget1.ChildWidgets[index1];
        for (int index2 = childWidget2.ChildWidgets.Count - 1; index2 >= 0; --index2)
        {
          if (this.m_table == childWidget2.ChildWidgets[index2].Widget)
          {
            this.m_ltWidget = childWidget2.ChildWidgets[index2];
            return true;
          }
        }
      }
    }
    return false;
  }

  private bool IsTableRelayout(ref RectangleF tableClientArea, int startingRowIndex)
  {
    if (this.m_ltState == LayoutState.Fitted || !this.m_table.TableFormat.WrapTextAround || this.m_table.TableFormat.Positioning.VertRelationTo != VerticalRelation.Paragraph || !this.IsWord2013(this.m_table.Document) || !(tableClientArea != RectangleF.Empty) || (double) tableClientArea.Y >= (double) this.m_ltWidget.Bounds.Y)
      return false;
    while (this.m_ltWidget.ChildWidgets.Count > 0)
    {
      this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1].InitLayoutInfoAll();
      this.m_ltWidget.ChildWidgets.RemoveAt(this.m_ltWidget.ChildWidgets.Count - 1);
    }
    this.CreateLayoutArea(tableClientArea);
    this.CreateLayoutedWidget(tableClientArea.Location);
    this.m_currRowIndex = startingRowIndex;
    this.m_currColIndex = -1;
    tableClientArea = RectangleF.Empty;
    this.m_blastRowState = this.m_ltState = LayoutState.Unknown;
    return true;
  }

  private bool IsNeedToMoveRow(FloatingItem item)
  {
    return !item.LayoutInCell && this.m_table.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 && item.TextWrappingStyle == TextWrappingStyle.TopAndBottom;
  }

  private bool IsInTextBoxOrShape(WTable table)
  {
    switch (this.GetBaseEntity((Entity) table))
    {
      case WTextBox _:
      case Shape _:
        return true;
      default:
        return false;
    }
  }

  private bool IsLayoutedFloatingTableInWTablacell()
  {
    for (int index = (this.m_lcOperator as Layouter).MaintainltWidget.ChildWidgets.Count - 1; index >= 0; --index)
    {
      if (this.m_table == (this.m_lcOperator as Layouter).MaintainltWidget.ChildWidgets[index].Widget)
      {
        this.m_ltWidget = (this.m_lcOperator as Layouter).MaintainltWidget.ChildWidgets[index];
        return true;
      }
    }
    return false;
  }

  private bool GetLayoutedFloatingTable()
  {
    for (int index = 0; index < (this.m_lcOperator as Layouter).FloatingItems.Count; ++index)
    {
      if ((this.m_lcOperator as Layouter).FloatingItems[index].FloatingEntity == this.m_table && (this.m_lcOperator as Layouter).MaintainltWidget.ChildWidgets.Count > 0 && (this.m_table.GetOwnerSection((Entity) this.m_table) as WSection).Columns.Count <= 1)
      {
        IWidget widget = (this.m_lcOperator as Layouter).MaintainltWidget.Widget;
        if (widget is WTableCell || widget is SplitWidgetContainer && (widget as SplitWidgetContainer).RealWidgetContainer is WTableCell)
          return this.IsLayoutedFloatingTableInWTablacell();
        if (!this.m_table.IsInCell)
          return this.IsLayoutedFloatingTableInTextBodyItems();
      }
    }
    return false;
  }

  private void FootnoteRowLayouting()
  {
    float height = 0.0f;
    LayoutedWidget layoutedWidget = new LayoutedWidget(this.m_currRowLW as IWidget);
    if (((this.m_currRowLW.Widget as WTableRow).m_layoutInfo as RowLayoutInfo).IsFootnoteSplitted)
    {
      layoutedWidget.GetFootnoteHeightForTableRow(ref height, this.m_currRowLW);
      for (int index = 0; index < (this.m_lcOperator as Layouter).FootnoteSplittedWidgets.Count; ++index)
        height -= (this.m_lcOperator as Layouter).FootnoteSplittedWidgets[index].m_currentChild.LayoutInfo.Size.Height;
    }
    else
      layoutedWidget.GetFootnoteHeightForTableRow(ref height, this.m_currRowLW);
    height += this.m_verticallyMergedCellFootnoteHeight;
    this.m_currColIndex = -1;
    this.ClearVerticalMergeStartLW();
    this.m_currRowLW.ChildWidgets.Clear();
    (this.m_lcOperator as Layouter).FootnoteSplittedWidgets.Clear();
    ((this.m_currRowLW.Widget as WTableRow).m_layoutInfo as RowLayoutInfo).IsFootnoteSplitted = false;
    this.RemoveFootnoteFromLayouter();
    RectangleF clientActiveArea = this.m_layoutArea.ClientActiveArea;
    clientActiveArea.Height -= height;
    this.m_layoutArea.UpdateClientActiveArea(clientActiveArea);
    this.m_currRowLW.Bounds = new RectangleF(this.m_currRowLW.Bounds.Location, new SizeF(0.0f, 0.0f));
    ((this.m_currRowLW.Widget as WTableRow).m_layoutInfo as RowLayoutInfo).IsFootnoteReduced = true;
    this.LayoutRow((IWidget) (this.m_currRowLW.Widget as WTableRow));
    this.m_verticallyMergedCellFootnoteHeight = 0.0f;
  }

  private bool IsAdjacentCellHasFootnote()
  {
    bool flag1 = false;
    bool flag2 = false;
    for (int index1 = 0; index1 < (this.m_currRowLW.Widget as WTableRow).Cells.Count; ++index1)
    {
      if (flag1)
        flag2 = true;
      for (int index2 = 0; index2 < (this.m_currRowLW.Widget as WTableRow).Cells[index1].Items.Count; ++index2)
      {
        CellLayoutInfo layoutInfo = ((IWidget) (this.m_currRowLW.Widget as WTableRow).Cells[index1]).LayoutInfo as CellLayoutInfo;
        if (!layoutInfo.IsSkip && !layoutInfo.IsRowMergeContinue && (this.m_currRowLW.Widget as WTableRow).Cells[index1].Items[index2] is WParagraph && (this.m_currRowLW.Widget as WTableRow).Cells[index1].Items[index2] is WParagraph wparagraph)
        {
          for (int index3 = 0; index3 < wparagraph.Items.Count; ++index3)
          {
            if (wparagraph.Items[index3] is WFootnote)
            {
              if (!flag1)
                flag1 = true;
              else if (flag2)
                return true;
            }
          }
        }
      }
    }
    return false;
  }

  private void RemoveFootnoteFromLayouter()
  {
    for (int index1 = 0; index1 < (this.m_currRowLW.Widget as WTableRow).Cells.Count; ++index1)
    {
      for (int index2 = 0; index2 < (this.m_currRowLW.Widget as WTableRow).Cells[index1].Items.Count; ++index2)
      {
        if ((this.m_currRowLW.Widget as WTableRow).Cells[index1].Items[index2] is WParagraph && (this.m_currRowLW.Widget as WTableRow).Cells[index1].Items[index2] is WParagraph wparagraph)
        {
          for (int index3 = 0; index3 < wparagraph.Items.Count; ++index3)
          {
            if (wparagraph.Items[index3] is WFootnote)
            {
              for (int index4 = (this.m_lcOperator as Layouter).FootnoteWidgets.Count - 1; index4 >= 0; --index4)
              {
                LayoutedWidget footnoteWidget = (this.m_lcOperator as Layouter).FootnoteWidgets[index4];
                if (footnoteWidget.Widget is WTextBody && (footnoteWidget.Widget as WTextBody).Owner == wparagraph.Items[index3] && wparagraph.Items[index3] is WFootnote)
                {
                  (this.m_lcOperator as Layouter).FootnoteWidgets.RemoveAt(index4);
                  (wparagraph.Items[index3] as WFootnote).IsLayouted = false;
                }
              }
            }
          }
        }
      }
    }
  }

  private bool IsWrappedTable()
  {
    return this.m_table.TableFormat.WrapTextAround && this.m_table.OwnerTextBody.OwnerBase is WSection && !(this.m_lcOperator as Layouter).IsLayoutingHeaderFooter && this.m_table.TableFormat.Positioning.VertRelationTo == VerticalRelation.Paragraph && (double) this.m_table.TableFormat.Positioning.VertPosition < 0.0 && this.m_ltState == LayoutState.Splitted && (double) this.m_ltWidget.Bounds.X < (double) (this.m_table.OwnerTextBody.OwnerBase as WSection).PageSetup.PageSize.Width && this.m_table.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 && !this.IsFirstItemInPage;
  }

  private void UpdateTableLWBounds(bool isWrappedTable, bool isSplittedtable)
  {
    if (this.m_ltWidget.ChildWidgets.Count == 0)
      (this.TableLayoutInfo as Syncfusion.Layouting.TableLayoutInfo).IsSplittedTable = false;
    if (!isSplittedtable && this.m_table.TableFormat.Positioning.VertPositionAbs != VerticalPosition.None && (this.m_table.TableFormat.Positioning.VertRelationTo == VerticalRelation.Page || this.m_table.TableFormat.Positioning.VertRelationTo == VerticalRelation.Margin && this.m_table.TableFormat.Positioning.VertPositionAbs == VerticalPosition.Bottom))
      this.m_ltWidget.ShiftLocation(0.0, -(double) this.m_ltWidget.Bounds.Y, true, false);
    this.UpdateTableKeepWithNext();
    Syncfusion.Layouting.TableLayoutInfo tableLayoutInfo = this.TableLayoutInfo as Syncfusion.Layouting.TableLayoutInfo;
    if (this.m_table.TableFormat.WrapTextAround && this.m_table.OwnerTextBody.OwnerBase is WSection && !(this.m_lcOperator as Layouter).IsLayoutingHeaderFooter && (double) this.m_ltWidget.Bounds.Height > (double) this.TableClientActiveArea.Height && this.m_table.TableFormat.Positioning.VertRelationTo == VerticalRelation.Page && (this.m_table.TableFormat.Positioning.HorizRelationTo != HorizontalRelation.Page || (double) this.m_ltWidget.Bounds.X >= (double) (this.m_table.OwnerTextBody.OwnerBase as WSection).PageSetup.PageSize.Width) && (this.m_table.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 && !this.IsFirstItemInPage || this.m_table.TableFormat.Positioning.VertPositionAbs == VerticalPosition.None && (double) this.m_ltWidget.Bounds.X >= (double) this.TableClientActiveArea.X && (double) this.m_ltWidget.Bounds.Right > (double) this.TableClientActiveArea.Right))
      isWrappedTable = true;
    else if (!isWrappedTable)
      isWrappedTable = this.IsWrappedTable();
    bool flag1 = (this.m_lcOperator as Layouter).CurrentSection.Columns.Count > 1;
    if (tableLayoutInfo != null && !tableLayoutInfo.IsSplittedTable && this.m_table.TableFormat.WrapTextAround && this.m_table.TableFormat.Positioning.VertRelationTo != VerticalRelation.Paragraph)
      this.UpdateAbsoluteTablePosition();
    float y = this.m_ltWidget.Bounds.Y;
    if (this.m_table.TableFormat.WrapTextAround && ((this.m_table.Document.DOP.Dop2000.Copts.DontBreakWrappedTables || flag1) && this.m_table.TableFormat.Positioning.VertRelationTo == VerticalRelation.Paragraph || isWrappedTable))
      y -= this.m_table.TableFormat.Positioning.VertPosition;
    if (!DocumentLayouter.IsFirstLayouting)
      this.m_ltWidget.Widget.LayoutInfo.IsFirstItemInPage = false;
    float pageTopMargin = (this.m_lcOperator as Layouter).PageTopMargin;
    if (this.IsFirstItemInPage)
      this.m_ltWidget.Widget.LayoutInfo.IsFirstItemInPage = true;
    if (flag1 && Math.Round((double) y, 2) == Math.Round((double) pageTopMargin, 2))
      this.m_ltWidget.Widget.LayoutInfo.IsFirstItemInPage = true;
    WTableRow wtableRow = (WTableRow) null;
    if (this.m_ltWidget.ChildWidgets.Count > 0)
    {
      wtableRow = this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1].Widget as WTableRow;
      RowLayoutInfo layoutInfo = this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1].Widget.LayoutInfo as RowLayoutInfo;
      bool skipSplittingTable = this.IsNeedToSkipSplittingTable();
      if (this.m_table.TableFormat.WrapTextAround && ((this.m_table.Document.DOP.Dop2000.Copts.DontBreakWrappedTables || flag1) && this.m_table.TableFormat.Positioning.VertRelationTo == VerticalRelation.Paragraph || isWrappedTable) || skipSplittingTable)
      {
        if (this.m_ltWidget.Widget.LayoutInfo.IsFirstItemInPage)
        {
          Entity baseEntity = this.GetBaseEntity((Entity) this.m_table);
          float num = baseEntity is WSection ? (baseEntity as WSection).PageSetup.PageSize.Height : 0.0f;
          float bottom = this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1].Bounds.Bottom;
          float yOffset = (double) bottom > (double) num ? num - bottom : 0.0f;
          if ((double) Math.Abs(yOffset) > (double) pageTopMargin)
            yOffset = -pageTopMargin;
          this.m_ltState = LayoutState.Fitted;
          if ((double) yOffset == 0.0)
            return;
          this.m_ltWidget.ShiftLocation(0.0, (double) yOffset, this.m_ltWidget.Bounds.Width, this.m_ltWidget.Bounds.Height);
          return;
        }
        if ((wtableRow == null || wtableRow.Index == this.m_table.Rows.Count - 1) && (this.m_ltState == LayoutState.Fitted || !this.m_table.TableFormat.WrapTextAround || !isWrappedTable && !flag1) && (this.m_ltState != LayoutState.Fitted || !this.m_table.TableFormat.WrapTextAround || !isWrappedTable || (double) this.TableClientActiveArea.Height >= (double) this.m_ltWidget.Bounds.Height) && !skipSplittingTable)
          return;
        if (isWrappedTable || flag1)
        {
          for (int index = 0; index < (this.m_lcOperator as Layouter).FloatingItems.Count; ++index)
          {
            if (this.GetOwnerTable((this.m_lcOperator as Layouter).FloatingItems[index].FloatingEntity) == this.m_table)
              (this.m_lcOperator as Layouter).FloatingItems.RemoveAt(index);
          }
        }
        this.m_ltState = LayoutState.NotFitted;
        tableLayoutInfo.IsSplittedTable = false;
        return;
      }
      if (wtableRow != null && wtableRow.GetRowIndex() == this.m_table.Rows.Count - 1 && DocumentLayouter.IsFirstLayouting && !layoutInfo.IsRowSplitted)
        tableLayoutInfo.IsSplittedTable = false;
    }
    if (this.m_ltWidget.ChildWidgets.Count > 0)
      tableLayoutInfo.IsHeaderRowHeightUpdated = true;
    if ((double) this.LayoutedHeaderRowHeight > 0.0)
    {
      tableLayoutInfo.HeaderRowHeight = this.LayoutedHeaderRowHeight;
      if (wtableRow != null && this.m_ltState == LayoutState.Fitted && wtableRow.Index == this.m_table.Rows.Count - 1)
        tableLayoutInfo.IsHeaderNotRepeatForAllPages = true;
    }
    if (!this.m_table.TableFormat.WrapTextAround && this.m_table.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 && this.m_ltWidget.ChildWidgets.Count > 0 && (this.m_ltWidget.ChildWidgets[0].Widget as WTableRow).GetRowIndex() == 0 && this.m_ltWidget.ChildWidgets.Count < this.m_table.Rows.Count && this.m_table.Rows[this.m_ltWidget.ChildWidgets.Count - 1].IsHeader)
    {
      bool flag2 = true;
      for (int index = 0; index < this.m_ltWidget.ChildWidgets.Count - 1; ++index)
      {
        if (!this.m_table.Rows[index].IsHeader)
          flag2 = false;
      }
      if (flag2)
      {
        if (!this.IsFirstItemInPage)
        {
          tableLayoutInfo.IsHeaderNotRepeatForAllPages = false;
          (this.m_table.Rows[this.CurrRowIndex].m_layoutInfo as RowLayoutInfo).IsRowBreakByPageBreakBefore = false;
          (this.m_ltWidget.Widget.LayoutInfo as Syncfusion.Layouting.TableLayoutInfo).HeaderRowHeight = 0.0f;
          this.m_ltState = LayoutState.NotFitted;
          this.IsVerticalNotFitted = true;
        }
        else
          (this.m_table.Rows[this.CurrRowIndex].m_layoutInfo as RowLayoutInfo).IsRowBreakByPageBreakBefore = true;
        tableLayoutInfo.IsHeaderRowHeightUpdated = false;
      }
    }
    if (!(this.m_table.Owner is WTextBody) || (this.m_lcOperator as Layouter).IsLayoutingHeaderFooter || !this.m_table.TableFormat.WrapTextAround || this.m_table.TableFormat.Positioning.VertPositionAbs != VerticalPosition.Bottom || this.m_spitTableWidget != null || this.m_table.Document.DOP.Dop2000.Copts.DontBreakWrappedTables)
      return;
    float num1 = this.m_table.TableFormat.Positioning.VertRelationTo == VerticalRelation.Margin ? (this.m_lcOperator as Layouter).CurrentSection.PageSetup.Margins.Bottom : 0.0f;
    if (tableLayoutInfo.IsSplittedTable && (this.m_table.TableFormat.Positioning.VertRelationTo == VerticalRelation.Page || this.m_table.TableFormat.Positioning.VertRelationTo == VerticalRelation.Margin))
      this.m_ltWidget.ShiftLocation(0.0, (double) ((this.m_lcOperator as Layouter).CurrentSection.PageSetup.PageSize.Height - this.m_ltWidget.Bounds.Height - num1), false, false);
    if (this.m_table.TableFormat.Positioning.VertRelationTo != VerticalRelation.Page || this.m_table.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 || this.m_ltWidget.ChildWidgets.Count <= 0)
      return;
    if (!this.IsForceFitLayout && (double) this.m_ltWidget.Bounds.Y + (double) this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1].Bounds.Height > (double) this.TableClientActiveArea.Bottom)
      this.m_ltState = LayoutState.NotFitted;
    else
      this.RelayoutingTable();
  }

  internal Entity GetOwnerTable(Entity entity)
  {
    Entity entity1 = entity;
    if (entity1.Owner == null)
      return (Entity) null;
    Entity owner;
    for (owner = entity1.Owner; !(owner is WTable); owner = owner.Owner)
    {
      if (owner.Owner == null)
        return (Entity) null;
    }
    return owner is WTable ? (Entity) (owner as WTable) : (Entity) null;
  }

  private bool IsNeedToSkipSplittingTable()
  {
    return !this.m_table.TableFormat.WrapTextAround && !this.m_table.IsInCell && this.m_table.IsCompleteFrame && this.m_ltState != LayoutState.Fitted;
  }

  private void RelayoutingTable()
  {
    int count = 0;
    for (int index = 0; index < this.m_ltWidget.ChildWidgets.Count; ++index)
    {
      if ((double) this.m_ltWidget.ChildWidgets[index].Bounds.Bottom > (double) this.TableClientActiveArea.Bottom)
      {
        ++count;
        if (index == 0)
        {
          count = this.m_ltWidget.ChildWidgets.Count - count;
          break;
        }
      }
    }
    this.m_ltWidget.ChildWidgets.RemoveRange(this.m_ltWidget.ChildWidgets.Count - count, count);
    this.m_sptWidget = (IWidget) new SplitTableWidget(this.TableWidget, this.m_ltWidget.ChildWidgets.Count + 1);
    this.m_ltState = LayoutState.Splitted;
    (this.TableLayoutInfo as ITableLayoutInfo).IsSplittedTable = true;
  }

  private void UpdateTableKeepWithNext()
  {
    if (this.m_table.TableFormat.WrapTextAround || this.m_table.IsInCell || this.m_ltWidget.ChildWidgets.Count <= 0)
      return;
    this.m_ltWidget.Widget.LayoutInfo.IsKeepWithNext = true;
    for (int index = 0; index < this.m_ltWidget.ChildWidgets.Count; ++index)
    {
      if (!this.m_ltWidget.ChildWidgets[index].Widget.LayoutInfo.IsKeepWithNext)
      {
        this.m_ltWidget.Widget.LayoutInfo.IsKeepWithNext = false;
        break;
      }
    }
  }

  private void UpdateAbsoluteTablePosition()
  {
    float xOffset = 0.0f;
    float yOffset = 0.0f;
    MarginsF marginsF = this.InitializePageMargins();
    float num1 = 0.0f;
    if (marginsF != null)
    {
      Entity baseEntity = this.GetBaseEntity((Entity) this.m_table);
      if (baseEntity is HeaderFooter)
      {
        num1 = (baseEntity.Owner as WSection).PageSetup.PageSize.Height;
      }
      else
      {
        while (!(baseEntity is WSection))
          baseEntity = this.GetBaseEntity(baseEntity);
        float num2 = this.m_table.Document.DOP.GutterAtTop ? (baseEntity as WSection).PageSetup.Margins.Gutter : 0.0f;
        num1 = (this.m_lcOperator as Layouter).ClientLayoutArea.Height + marginsF.Top + num2 + marginsF.Bottom;
      }
    }
    if ((double) this.m_ltWidget.Bounds.Height < (double) (this.m_lcOperator as Layouter).ClientLayoutArea.Height && this.m_table.TableFormat.Positioning.VertRelationTo != VerticalRelation.Page)
      yOffset = this.m_table.TableFormat.Positioning.VertPositionAbs != VerticalPosition.Bottom || this.m_table.TableFormat.Positioning.VertRelationTo != VerticalRelation.Margin ? (this.m_table.TableFormat.Positioning.VertPositionAbs != VerticalPosition.Center ? ((this.m_lcOperator as Layouter).CurrPageIndex % 2 != 0 ? (this.m_lcOperator as Layouter).ClientLayoutArea.Height - this.m_ltWidget.Bounds.Height : (this.m_lcOperator as Layouter).ClientLayoutArea.Y - this.m_ltWidget.Bounds.Y) : ((float) ((double) (this.m_lcOperator as Layouter).ClientLayoutArea.Height / 2.0 - (double) this.m_ltWidget.Bounds.Height / 2.0) + (this.m_lcOperator as Layouter).ClientLayoutArea.Y - this.m_ltWidget.Bounds.Y) * 2f) : (this.m_lcOperator as Layouter).ClientLayoutArea.Bottom - this.m_ltWidget.Bounds.Height - this.m_ltWidget.Bounds.Y;
    else if (this.m_table.TableFormat.Positioning.VertPositionAbs == VerticalPosition.Outside && this.m_table.TableFormat.Positioning.VertRelationTo == VerticalRelation.Page)
      yOffset = (this.m_lcOperator as Layouter).CurrPageIndex % 2 != 0 ? (float) ((double) num1 - (double) this.m_ltWidget.Bounds.Height - (double) (this.m_lcOperator as Layouter).CurrentSection.PageSetup.FooterDistance / 2.0) : (this.m_lcOperator as Layouter).CurrentSection.PageSetup.HeaderDistance / 2f - this.m_ltWidget.Bounds.Y;
    else if (this.m_table.TableFormat.Positioning.VertPositionAbs == VerticalPosition.Inside && this.m_table.TableFormat.Positioning.VertRelationTo == VerticalRelation.Page)
      yOffset = (this.m_lcOperator as Layouter).CurrPageIndex % 2 == 0 ? (float) ((double) num1 - (double) this.m_ltWidget.Bounds.Height - (double) (this.m_lcOperator as Layouter).CurrentSection.PageSetup.FooterDistance / 2.0) : (this.m_lcOperator as Layouter).CurrentSection.PageSetup.HeaderDistance / 2f - this.m_ltWidget.Bounds.Y;
    else if ((double) this.m_ltWidget.Bounds.Height < (double) num1)
      yOffset = num1 - this.m_ltWidget.Bounds.Height;
    if (this.m_table.OwnerTextBody != null && this.m_table.TableFormat.Positioning.VertPositionAbs == VerticalPosition.Bottom && (this.m_table.TableFormat.Positioning.VertRelationTo == VerticalRelation.Page || this.m_table.TableFormat.Positioning.VertRelationTo == VerticalRelation.Margin))
    {
      float num3 = this.m_table.TableFormat.Positioning.VertRelationTo == VerticalRelation.Margin ? (this.m_lcOperator as Layouter).CurrentSection.PageSetup.Margins.Bottom : 0.0f;
      if ((this.m_table.Document.ActualFormatType == FormatType.Doc && this.m_table.Document.WordVersion <= (ushort) 257 || this.m_table.Document.DOP.Dop2000.Copts.DontBreakWrappedTables) && (double) this.m_ltWidget.Bounds.Height > (double) this.TableClientActiveArea.Height)
      {
        this.m_ltState = LayoutState.NotFitted;
        return;
      }
      yOffset = (this.m_lcOperator as Layouter).CurrentSection.PageSetup.PageSize.Height - this.m_ltWidget.Bounds.Height - num3;
    }
    if (this.m_table.IsInCell || (double) yOffset == 0.0)
      return;
    if (this.m_table.TableFormat.Positioning.VertPositionAbs == VerticalPosition.Bottom || this.m_table.TableFormat.Positioning.VertPositionAbs == VerticalPosition.Outside || this.m_table.TableFormat.Positioning.VertPositionAbs == VerticalPosition.Inside)
    {
      this.m_ltWidget.ShiftLocation((double) xOffset, (double) yOffset, true, false);
    }
    else
    {
      if (this.m_table.TableFormat.Positioning.VertPositionAbs != VerticalPosition.Center)
        return;
      this.m_ltWidget.ShiftLocation((double) xOffset, (double) yOffset / 2.0, true, false);
    }
  }

  private MarginsF InitializePageMargins()
  {
    MarginsF marginsF = (MarginsF) null;
    IEntity owner = (IEntity) this.m_table.Owner;
    if (owner != null)
    {
      while (owner.EntityType != EntityType.Section && owner.Owner != null)
        owner = (IEntity) owner.Owner;
      if (owner.EntityType == EntityType.Section)
      {
        IWSection wsection = (IWSection) (owner as WSection);
        if (wsection != null)
          marginsF = wsection.PageSetup.Margins;
      }
    }
    return marginsF;
  }

  private void AdjustClientAreaBasedOnTextWrap(SizeF size, ref RectangleF rect)
  {
    float firstRowWidth = this.GetFirstRowWidth();
    if ((this.m_lcOperator as Layouter).IsLayoutingHeaderFooter && this.m_table.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 || this.IsEntityOwnerIsWTextbox((Entity) this.m_table) != null)
      return;
    RectangleF clientLayoutArea = (this.m_lcOperator as Layouter).ClientLayoutArea;
    int floattingItemIndex = this.GetFloattingItemIndex(this.GetBaseEntity((Entity) this.m_table));
    FloatingItem.SortFloatingItems((this.m_lcOperator as Layouter).FloatingItems, SortPosition.Y);
    for (int index = 0; index < (this.m_lcOperator as Layouter).FloatingItems.Count; ++index)
    {
      RectangleF textWrappingBounds = (this.m_lcOperator as Layouter).FloatingItems[index].TextWrappingBounds;
      TextWrappingStyle textWrappingStyle = (this.m_lcOperator as Layouter).FloatingItems[index].TextWrappingStyle;
      bool allowOverlap = (this.m_lcOperator as Layouter).FloatingItems[index].AllowOverlap;
      if (this.IsAdjustTightAndThroughBounds(textWrappingStyle, index))
        textWrappingBounds = this.AdjustTightAndThroughBounds((this.m_lcOperator as Layouter).FloatingItems[index], rect, size.Height);
      WTextBody ownerBody = (WTextBody) null;
      if ((this.IsInSameTextBody((TextBodyItem) this.m_table, (this.m_lcOperator as Layouter).FloatingItems[index], ref ownerBody) || !this.m_table.IsInCell || !(ownerBody is WTableCell)) && (!this.IsInFrame((this.m_lcOperator as Layouter).FloatingItems[index].FloatingEntity as WParagraph) || !this.m_table.IsFrame))
      {
        textWrappingBounds = this.AdjustTextWrappingBounds((this.m_lcOperator as Layouter).FloatingItems[index], clientLayoutArea, size, floattingItemIndex, index, rect, textWrappingBounds, textWrappingStyle, allowOverlap);
        if ((double) clientLayoutArea.X <= (double) textWrappingBounds.Right + 16.0 && (double) clientLayoutArea.Right >= (double) textWrappingBounds.X - 16.0)
        {
          if (this.IsFloatingItemIntersect(floattingItemIndex, index, rect, textWrappingBounds, textWrappingStyle, allowOverlap, size))
          {
            if ((double) rect.X >= (double) textWrappingBounds.X && (double) rect.X < (double) textWrappingBounds.Right)
            {
              rect.Width -= textWrappingBounds.Right - rect.X;
              if ((double) rect.Width < 16.0 || (double) rect.Width < (double) firstRowWidth && (double) firstRowWidth > 0.0)
              {
                rect.Width = this.m_layoutArea.ClientActiveArea.Right - textWrappingBounds.Right;
                if ((double) rect.Width < 16.0 || (double) rect.Width < (double) firstRowWidth && (double) firstRowWidth > 0.0 && (double) clientLayoutArea.Right <= (double) firstRowWidth + (double) textWrappingBounds.Right)
                {
                  float num = (double) textWrappingBounds.Bottom > (double) rect.Y ? textWrappingBounds.Bottom - rect.Y : 0.0f;
                  rect.Y = textWrappingBounds.Bottom;
                  rect.Width = this.m_layoutArea.ClientArea.Width;
                  rect.Height -= num;
                  this.CreateLayoutArea(rect);
                }
                else
                {
                  rect.X = textWrappingBounds.Right;
                  if (textWrappingStyle == TextWrappingStyle.Through && (this.m_lcOperator as Layouter).FloatingItems[index].IsDoesNotDenotesRectangle)
                  {
                    textWrappingBounds = this.AdjustTightAndThroughBounds((this.m_lcOperator as Layouter).FloatingItems[index], rect, size.Height);
                    if ((double) textWrappingBounds.X != 0.0)
                      rect.Width = textWrappingBounds.X - rect.X;
                  }
                  this.CreateLayoutArea(rect);
                }
              }
              else
              {
                rect.X = textWrappingBounds.Right;
                if (textWrappingStyle == TextWrappingStyle.Through && (this.m_lcOperator as Layouter).FloatingItems[index].IsDoesNotDenotesRectangle)
                {
                  textWrappingBounds = this.AdjustTightAndThroughBounds((this.m_lcOperator as Layouter).FloatingItems[index], rect, size.Height);
                  if ((double) textWrappingBounds.X != 0.0)
                    rect.Width = textWrappingBounds.X - rect.X;
                }
                this.CreateLayoutArea(rect);
              }
            }
            else if ((double) rect.Right - (double) textWrappingBounds.Right > 0.0 && (double) rect.Right - (double) textWrappingBounds.Right < (double) rect.Width && ((double) rect.Y >= (double) textWrappingBounds.Y || (double) rect.Y + (double) size.Height >= (double) textWrappingBounds.Y))
            {
              if ((double) rect.X < (double) textWrappingBounds.X && (double) rect.Right > (double) textWrappingBounds.X)
              {
                if (this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Left)
                  rect.X += this.m_table.TableFormat.Positioning.DistanceFromLeft;
                else if (this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Right)
                  rect.X -= this.m_table.TableFormat.Positioning.DistanceFromRight;
              }
              float num = (double) textWrappingBounds.Bottom > (double) rect.Y ? textWrappingBounds.Bottom - rect.Y : 0.0f;
              rect.Y = textWrappingBounds.Bottom;
              rect.Height -= num;
              this.CreateLayoutArea(rect);
            }
            else if ((double) textWrappingBounds.X > (double) rect.X && (double) rect.Right > (double) textWrappingBounds.X)
            {
              rect.Width = textWrappingBounds.X - rect.X;
              if ((double) rect.Width < 16.0 || (double) rect.Width < (double) firstRowWidth && (double) firstRowWidth > 0.0)
              {
                rect.Width = this.m_layoutArea.ClientActiveArea.Right - textWrappingBounds.Right;
                if ((double) rect.Width < 16.0 || (double) rect.Width < (double) firstRowWidth && (double) firstRowWidth > 0.0)
                {
                  if ((double) this.m_layoutArea.ClientArea.Right < (double) (this.m_lcOperator as Layouter).ClientLayoutArea.Right && (double) textWrappingBounds.Right < (double) (this.m_lcOperator as Layouter).ClientLayoutArea.Right && (double) (this.m_lcOperator as Layouter).ClientLayoutArea.Right - (double) textWrappingBounds.Right > 16.0 && Math.Round((double) this.m_layoutArea.ClientActiveArea.Width, 2) > Math.Round((double) firstRowWidth, 2))
                  {
                    rect.Width = (this.m_lcOperator as Layouter).ClientLayoutArea.Right - textWrappingBounds.Right;
                    rect.X = textWrappingBounds.Right;
                    if (textWrappingStyle == TextWrappingStyle.Through && (this.m_lcOperator as Layouter).FloatingItems[index].IsDoesNotDenotesRectangle)
                    {
                      textWrappingBounds = this.AdjustTightAndThroughBounds((this.m_lcOperator as Layouter).FloatingItems[index], rect, size.Height);
                      if ((double) textWrappingBounds.X != 0.0)
                        rect.Width = textWrappingBounds.X - rect.X;
                    }
                  }
                  else
                  {
                    float num = (double) textWrappingBounds.Bottom > (double) rect.Y ? textWrappingBounds.Bottom - rect.Y : 0.0f;
                    rect.Y = textWrappingBounds.Bottom;
                    rect.Height -= num;
                  }
                  this.CreateLayoutArea(rect);
                }
              }
              else
                this.CreateLayoutArea(rect);
            }
            else if ((double) rect.X > (double) textWrappingBounds.X && (double) rect.X > (double) textWrappingBounds.Right)
            {
              rect.Width = this.m_layoutArea.ClientArea.Width;
              this.CreateLayoutArea(rect);
            }
            else if ((double) rect.X > (double) textWrappingBounds.X && (double) rect.X < (double) textWrappingBounds.Right)
            {
              rect.Width -= textWrappingBounds.Right - rect.X;
              rect.X = textWrappingBounds.Right;
              if (textWrappingStyle == TextWrappingStyle.Through && (this.m_lcOperator as Layouter).FloatingItems[index].IsDoesNotDenotesRectangle)
              {
                textWrappingBounds = this.AdjustTightAndThroughBounds((this.m_lcOperator as Layouter).FloatingItems[index], rect, size.Height);
                if ((double) textWrappingBounds.X != 0.0)
                  rect.Width = textWrappingBounds.X - rect.X;
              }
              this.CreateLayoutArea(rect);
            }
          }
          else if (this.IsFloatingItemIntersectForTopAndBottom(floattingItemIndex, index, rect, textWrappingBounds, textWrappingStyle, allowOverlap, size))
          {
            rect.Y = textWrappingBounds.Bottom;
            rect.Height -= textWrappingBounds.Height;
            this.CreateLayoutArea(rect);
          }
        }
      }
    }
  }

  internal RectangleF AdjustTextWrappingBounds(
    FloatingItem floatingItem,
    RectangleF clientLayoutArea,
    SizeF size,
    int wrapItemIndex,
    int i,
    RectangleF rect,
    RectangleF textWrappingBounds,
    TextWrappingStyle textWrappingStyle,
    bool allowOverlap)
  {
    float DistanceLeft = 0.0f;
    float DistanceRight = 0.0f;
    float DistanceTop = 0.0f;
    float DistanceBottom = 0.0f;
    bool flag = floatingItem.FloatingEntity.EntityType == EntityType.Table ? (floatingItem.FloatingEntity as WTable).TableFormat != null && (floatingItem.FloatingEntity as WTable).TableFormat.Positioning.AllowOverlap && this.m_table.TableFormat.Positioning.AllowOverlap : floatingItem.TextWrappingStyle != TextWrappingStyle.Inline && floatingItem.TextWrappingStyle != TextWrappingStyle.InFrontOfText && floatingItem.TextWrappingStyle != TextWrappingStyle.Behind;
    if (this.m_table.TableFormat != null && this.m_table.TableFormat.WrapTextAround && flag && !this.IsAdjustTightAndThroughBounds(textWrappingStyle, i) && (double) clientLayoutArea.X <= (double) rect.Right + 16.0 && (double) clientLayoutArea.Right >= (double) rect.X - 16.0 && (this.IsFloatingItemIntersect(wrapItemIndex, i, rect, textWrappingBounds, textWrappingStyle, allowOverlap, size) || this.IsFloatingItemIntersectForTopAndBottom(wrapItemIndex, i, rect, textWrappingBounds, textWrappingStyle, allowOverlap, size)))
    {
      switch (floatingItem.FloatingEntity.EntityType)
      {
        case EntityType.Table:
          WTable floatingEntity1 = floatingItem.FloatingEntity as WTable;
          DistanceLeft = floatingEntity1.TableFormat.Positioning.DistanceFromLeft;
          DistanceTop = floatingEntity1.TableFormat.Positioning.DistanceFromTop;
          DistanceRight = floatingEntity1.TableFormat.Positioning.DistanceFromRight;
          DistanceBottom = floatingEntity1.TableFormat.Positioning.DistanceFromBottom;
          break;
        case EntityType.Picture:
          WPicture floatingEntity2 = floatingItem.FloatingEntity as WPicture;
          DistanceLeft = floatingEntity2.DistanceFromLeft;
          DistanceTop = floatingEntity2.DistanceFromTop;
          DistanceRight = floatingEntity2.DistanceFromRight;
          DistanceBottom = floatingEntity2.DistanceFromBottom;
          break;
        case EntityType.TextBox:
          WTextBox floatingEntity3 = floatingItem.FloatingEntity as WTextBox;
          DistanceLeft = floatingEntity3.TextBoxFormat.WrapDistanceLeft;
          DistanceTop = floatingEntity3.TextBoxFormat.WrapDistanceTop;
          DistanceRight = floatingEntity3.TextBoxFormat.WrapDistanceRight;
          DistanceBottom = floatingEntity3.TextBoxFormat.WrapDistanceBottom;
          break;
        case EntityType.Chart:
        case EntityType.AutoShape:
        case EntityType.GroupShape:
          ShapeBase shapeBase = !(floatingItem.FloatingEntity is Shape) ? (!(floatingItem.FloatingEntity is GroupShape) ? (ShapeBase) (floatingItem.FloatingEntity as WChart) : (ShapeBase) (floatingItem.FloatingEntity as GroupShape)) : (ShapeBase) (floatingItem.FloatingEntity as Shape);
          DistanceLeft = shapeBase.WrapFormat.DistanceLeft;
          DistanceTop = shapeBase.WrapFormat.DistanceTop;
          DistanceRight = shapeBase.WrapFormat.DistanceRight;
          DistanceBottom = shapeBase.WrapFormat.DistanceBottom;
          break;
      }
    }
    return this.AdjustWrappingBounds(textWrappingBounds, DistanceLeft, DistanceRight, DistanceTop, DistanceBottom);
  }

  private RectangleF AdjustWrappingBounds(
    RectangleF textWrappingBounds,
    float DistanceLeft,
    float DistanceRight,
    float DistanceTop,
    float DistanceBottom)
  {
    textWrappingBounds.X += DistanceLeft;
    textWrappingBounds.Y += DistanceTop;
    textWrappingBounds.Width -= DistanceRight + DistanceLeft;
    textWrappingBounds.Height -= DistanceBottom + DistanceTop;
    return textWrappingBounds;
  }

  private bool IsFloatingItemIntersect(
    int wrapItemIndex,
    int i,
    RectangleF rect,
    RectangleF textWrappingBounds,
    TextWrappingStyle textWrappingStyle,
    bool allowOverlap,
    SizeF size)
  {
    if ((this.m_lcOperator as Layouter).FloatingItems.Count <= 0 || wrapItemIndex == (this.m_lcOperator as Layouter).FloatingItems[i].WrapCollectionIndex || (Math.Round((double) rect.Y + (double) size.Height, 2) < Math.Round((double) textWrappingBounds.Y, 2) || Math.Round((double) rect.Y, 2) >= Math.Round((double) textWrappingBounds.Bottom, 2)) && (Math.Round((double) rect.Y + (double) size.Height, 2) > Math.Round((double) textWrappingBounds.Bottom, 2) || Math.Round((double) rect.Y + (double) size.Height, 2) < Math.Round((double) textWrappingBounds.Y, 2)) || textWrappingStyle == TextWrappingStyle.Inline || textWrappingStyle == TextWrappingStyle.TopAndBottom || textWrappingStyle == TextWrappingStyle.InFrontOfText || textWrappingStyle == TextWrappingStyle.Behind)
      return false;
    return !allowOverlap || this.m_table.TableFormat == null || !this.m_table.TableFormat.WrapTextAround || !this.m_table.TableFormat.Positioning.AllowOverlap;
  }

  private bool IsFloatingItemIntersectForTopAndBottom(
    int wrapItemIndex,
    int i,
    RectangleF rect,
    RectangleF textWrappingBounds,
    TextWrappingStyle textWrappingStyle,
    bool allowOverlap,
    SizeF size)
  {
    if ((this.m_lcOperator as Layouter).FloatingItems.Count <= 0 || wrapItemIndex == (this.m_lcOperator as Layouter).FloatingItems[i].WrapCollectionIndex || ((double) rect.Y < (double) textWrappingBounds.Y || (double) rect.Y >= (double) textWrappingBounds.Bottom) && ((double) rect.Y + (double) this.GetMaxCellHeight(size.Height) < (double) textWrappingBounds.Y || (double) rect.Y + (double) size.Height >= (double) textWrappingBounds.Bottom) || textWrappingStyle != TextWrappingStyle.TopAndBottom)
      return false;
    return !allowOverlap || this.m_table.TableFormat == null || !this.m_table.TableFormat.WrapTextAround || !this.m_table.TableFormat.Positioning.AllowOverlap;
  }

  private bool IsAdjustTightAndThroughBounds(TextWrappingStyle textWrappingStyle, int i)
  {
    return (textWrappingStyle == TextWrappingStyle.Tight || textWrappingStyle == TextWrappingStyle.Through) && (this.m_lcOperator as Layouter).FloatingItems[i].IsDoesNotDenotesRectangle;
  }

  private float GetTableHeight()
  {
    float tableHeight = 0.0f;
    foreach (WTableRow row in (Syncfusion.DocIO.DLS.CollectionImpl) this.m_table.Rows)
      tableHeight += this.GetRowHeight(row);
    return tableHeight;
  }

  private bool IsTableMoveToNextPage(TextBodyItem widget)
  {
    if (widget.PreviousSibling is WTable && !(widget.PreviousSibling as WTable).TableFormat.WrapTextAround || widget.PreviousSibling is BlockContentControl && (widget.PreviousSibling as BlockContentControl).LastChildEntity is WTable && !((widget.PreviousSibling as BlockContentControl).LastChildEntity as WTable).TableFormat.WrapTextAround)
      return true;
    if (widget.PreviousSibling == null && widget.Owner.Owner is BlockContentControl)
      return this.IsTableMoveToNextPage(widget.Owner.Owner as TextBodyItem);
    return widget.PreviousSibling is WParagraph;
  }

  private float GetRowHeight(WTableRow row)
  {
    float rowHeight = row.Height;
    Spacings margins = (((IWidget) row.Cells[0]).LayoutInfo as CellLayoutInfo).Margins;
    Spacings paddings = (((IWidget) row.Cells[0]).LayoutInfo as CellLayoutInfo).Paddings;
    if (row.HeightType == TableRowHeightType.Exactly)
      rowHeight = row.Height + margins.Bottom;
    else if (row.HeightType == TableRowHeightType.AtLeast)
      rowHeight = row.Height + margins.Top + margins.Bottom + paddings.Top;
    return rowHeight;
  }

  private float GetFirstRowWidth()
  {
    float firstRowWidth = 0.0f;
    int currRowIndex = this.CurrRowIndex == -1 ? 0 : this.CurrRowIndex;
    if (this.m_table.Rows.Count > currRowIndex)
    {
      for (int colIndex = 0; colIndex < this.m_table.Rows[currRowIndex].Cells.Count; ++colIndex)
        firstRowWidth += this.GetCellWidth(currRowIndex, colIndex);
    }
    return firstRowWidth;
  }

  private float GetMaxCellHeight(float cellMinHeight)
  {
    float val1 = 0.0f;
    for (int colIndex = 0; colIndex < this.m_table.Rows[0].Cells.Count; ++colIndex)
      val1 = Math.Max(val1, this.GetCellHeight(0, colIndex, cellMinHeight));
    return val1;
  }

  private IWidget CreateRowLayoutedWidget()
  {
    if (this.CurrRowIndex + 1 >= this.TableWidget.RowsCount)
      return (IWidget) null;
    this.m_currColIndex = -1;
    this.NextRowIndex();
    while (this.CurrRowIndex < this.TableWidget.RowsCount && (this.TableWidget as WTable).IsHiddenRow(this.CurrRowIndex, this.TableWidget as WTable))
    {
      ++this.m_currRowIndex;
      if (this.CurrRowIndex == this.TableWidget.RowsCount)
      {
        --this.m_currRowIndex;
        return (IWidget) null;
      }
    }
    IWidget rowWidget = this.TableWidget.GetRowWidget(this.CurrRowIndex);
    this.m_currRowLW = new LayoutedWidget(rowWidget);
    this.m_rowLayoutArea = new LayoutArea(this.m_layoutArea.ClientActiveArea, rowWidget.LayoutInfo as ILayoutSpacingsInfo, rowWidget);
    this.m_currRowLW.Bounds = new RectangleF(this.m_rowLayoutArea.ClientActiveArea.Location, new SizeF());
    if (rowWidget.LayoutInfo.IsSkip)
      this.m_bAtLastOneCellFitted = true;
    return rowWidget;
  }

  private void InitCellSpacing(WTableRow row)
  {
    for (int index = 0; index < row.Cells.Count; ++index)
    {
      WTableCell cell = row.Cells[index];
      if (cell.m_layoutInfo != null)
        (cell.m_layoutInfo as CellLayoutInfo).InitSpacings();
    }
  }

  private void LayoutRow(IWidget rowWidget)
  {
    bool isFitRowByUsingVerticalDistance = false;
    this.LayoutRow(rowWidget, ref isFitRowByUsingVerticalDistance);
  }

  private void LayoutRow(IWidget rowWidget, ref bool isFitRowByUsingVerticalDistance)
  {
    RowLayoutInfo layoutInfo1 = rowWidget.LayoutInfo as RowLayoutInfo;
    float maxTopPading = 0.0f;
    float maxBottomPadding = 0.0f;
    float maxTopMargin = 0.0f;
    float maxBottomMargin = 0.0f;
    this.GetCellsMaxTopAndBottomPadding(rowWidget as WTableRow, out maxTopPading, out maxBottomPadding, out maxTopMargin, out maxBottomMargin);
    if ((double) (this.m_currRowLW.Widget as WTableRow).RowFormat.CellSpacing <= 0.0 && (double) this.m_table.TableFormat.CellSpacing <= 0.0)
    {
      if (!(this.m_lcOperator as Layouter).IsLayoutingHeaderRow && !layoutInfo1.IsCellPaddingUpdated)
        this.UpdateCellsMaxTopAndBottomPadding(rowWidget as WTableRow, maxTopPading, maxBottomPadding);
      this.m_rowLayoutArea = this.CreateRowLayoutArea(rowWidget as WTableRow, maxBottomPadding);
    }
    else
    {
      float bottomPad = this.UpdateCellsBottomPaddingAndMargin(rowWidget as WTableRow, maxBottomPadding, maxBottomMargin);
      this.m_rowLayoutArea = this.CreateRowLayoutArea(rowWidget as WTableRow, bottomPad);
    }
    Entity baseEntity = this.GetBaseEntity((Entity) this.m_table);
    bool flag1 = true;
    if (!(baseEntity is WTextBox) && !(baseEntity is Shape) && !this.m_table.TableFormat.WrapTextAround && !this.IsInFrame() && this.m_table.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013)
    {
      foreach (FloatingItem floatingItem in ((Layouter) this.m_lcOperator).FloatingItems)
      {
        if ((floatingItem.FloatingEntity is WTable floatingEntity && floatingEntity != this.m_table && !this.m_table.IsInCell || this.IsNeedToMoveRow(floatingItem)) && floatingItem.TextWrappingBounds.IntersectsWith(this.m_currRowLW.Bounds) && (double) (this.m_lcOperator as Layouter).ClientLayoutArea.Bottom >= (double) floatingItem.TextWrappingBounds.Y)
        {
          this.m_currRowLW.Bounds = new RectangleF(this.m_currRowLW.Bounds.X, floatingItem.TextWrappingBounds.Bottom, this.m_currRowLW.Bounds.Width, this.m_currRowLW.Bounds.Height);
          flag1 = false;
          if (this.IsNeedToMoveRow(floatingItem))
          {
            LayoutArea layoutArea = new LayoutArea(this.m_layoutArea.ClientActiveArea with
            {
              Y = floatingItem.TextWrappingBounds.Bottom
            });
            this.m_rowLayoutArea = layoutArea;
            this.m_layoutArea = layoutArea;
            if (this.m_currRowIndex != 0)
            {
              (((IWidget) (this.m_table.ChildEntities[this.m_currRowIndex - 1] as WTableRow)).LayoutInfo as RowLayoutInfo).IsRowSplittedByFloatingItem = true;
              break;
            }
            break;
          }
          break;
        }
      }
    }
    if (layoutInfo1.IsExactlyRowHeight || (rowWidget as WTableRow).HeightType == TableRowHeightType.AtLeast || layoutInfo1.IsVerticalText)
    {
      if ((double) this.m_currRowLW.Bounds.Bottom > (double) this.m_layoutArea.ClientActiveArea.Bottom && !this.IsInExactlyRow((Entity) this.m_table))
      {
        switch (baseEntity)
        {
          case WTextBox _:
          case Shape _:
          case GroupShape _:
            break;
          default:
            if (this.m_table.TableFormat.WrapTextAround || !this.IsInFrame())
            {
              bool flag2 = false;
              if (this.m_table.IsInCell && this.m_table.GetOwnerTableCell().ChildEntities[0] is WTable && this.m_currRowIndex == 0)
                flag2 = true;
              bool flag3 = false;
              if (this.m_table.TableFormat.WrapTextAround && (this.m_currRowLW.Widget as WTableRow).Index == 0 && !this.m_table.IsInCell && this.m_table.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013)
                flag3 = this.IsForceFitLayout;
              if (!flag3 && !this.IsForceFitLayout && (!flag2 || this.m_table.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013))
              {
                if (this.IsFitRowByUsingVerticalDistance())
                {
                  this.m_currRowLW.Bounds = new RectangleF(this.m_currRowLW.Bounds.Location, new SizeF(this.m_currRowLW.Bounds.Width, this.m_currRowLW.Bounds.Height - layoutInfo1.Margins.Bottom - layoutInfo1.Paddings.Bottom));
                  isFitRowByUsingVerticalDistance = true;
                  goto label_36;
                }
                this.m_ltState = LayoutState.NotFitted;
                if (!flag1)
                  return;
                this.CommitKeepWithNext();
                return;
              }
              if ((rowWidget as WTableRow).Document.Settings.CompatibilityMode != CompatibilityMode.Word2013)
              {
                this.m_currRowLW.Bounds = new RectangleF(this.m_currRowLW.Bounds.Location, new SizeF(this.m_currRowLW.Bounds.Width, this.m_layoutArea.ClientArea.Height - this.m_headerRowHeight));
                goto label_36;
              }
              if ((rowWidget as WTableRow).Document.Settings.CompatibilityMode == CompatibilityMode.Word2013)
              {
                this.m_currRowLW.Bounds = (rowWidget as WTableRow).HeightType != TableRowHeightType.Exactly ? new RectangleF(this.m_currRowLW.Bounds.Location, new SizeF(this.m_currRowLW.Bounds.Width, this.m_layoutArea.ClientArea.Height)) : new RectangleF(this.m_currRowLW.Bounds.Location, new SizeF(this.m_currRowLW.Bounds.Width, this.m_currRowLW.Bounds.Height));
                goto label_36;
              }
              goto label_36;
            }
            break;
        }
      }
      this.m_currRowLW.Bounds = new RectangleF(this.m_currRowLW.Bounds.Location, new SizeF(this.m_currRowLW.Bounds.Width, this.m_currRowLW.Bounds.Height - layoutInfo1.Margins.Bottom - layoutInfo1.Paddings.Bottom));
    }
    else if (!layoutInfo1.IsRowBreakByPageBreakBefore && this.m_ltState != LayoutState.NotFitted && !this.IsForceFitLayout && !(rowWidget as WTableRow).OwnerTable.IsInCell && this.IsRowSplitByPageBreakBefore(rowWidget as WTableRow))
    {
      this.m_ltState = LayoutState.NotFitted;
      layoutInfo1.IsRowBreakByPageBreakBefore = true;
      return;
    }
label_36:
    this.m_splitedCells = new SplitWidgetContainer[this.m_table.Rows[this.CurrRowIndex].Cells.Count];
    this.m_currColIndex = this.m_table == null || !this.m_table.TableFormat.Bidi ? -1 : this.m_table.Rows[this.CurrRowIndex].Cells.Count;
    do
    {
      LayoutContext nextCellContext = this.CreateNextCellContext();
      if (nextCellContext == null)
        break;
      CellLayoutInfo layoutInfo2 = nextCellContext.Widget.LayoutInfo as CellLayoutInfo;
      LayoutArea cellClientArea = this.GetCellClientArea(layoutInfo2, this.CurrRowIndex, this.m_currColIndex, maxTopPading, maxTopMargin);
      nextCellContext.ClientLayoutAreaRight = (float) cellClientArea.Width;
      if (!layoutInfo1.IsRowHasVerticalTextCell && layoutInfo2.IsVerticalText)
        layoutInfo1.IsRowHasVerticalTextCell = !layoutInfo1.IsVerticalText && layoutInfo2.IsVerticalText;
      this.m_currCellLW = this.LayoutCell(nextCellContext, cellClientArea.ClientArea, layoutInfo2.IsRowMergeStart || layoutInfo2.IsRowMergeContinue || !layoutInfo1.IsVerticalText && layoutInfo2.IsVerticalText);
      if (DocumentLayouter.IsEndUpdateTOC && DocumentLayouter.IsUpdatingTOC)
        break;
      if (this.m_table.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013)
        layoutInfo1.IsFootnoteSplitted = this.CheckFootnoteInRowIsSplitted(nextCellContext);
      if (isFitRowByUsingVerticalDistance && nextCellContext.State == LayoutState.Splitted)
      {
        this.m_ltState = LayoutState.NotFitted;
        if (flag1)
          this.CommitKeepWithNext();
        (this.m_lcOperator as Layouter).RemovedWidgetsHeight += this.m_layoutArea.ClientActiveArea.Height;
        break;
      }
      this.SaveChildContextState(nextCellContext);
    }
    while (this.State == LayoutState.Unknown && !(this.m_lcOperator as Layouter).IsNeedToRelayoutTable);
  }

  private bool IsInExactlyRow(Entity entity)
  {
    Entity entity1 = entity;
    while (entity1.Owner != null)
    {
      entity1 = entity1.Owner;
      if (entity1 is WTableCell && (((IWidget) (entity1 as WTableCell).OwnerRow).LayoutInfo as RowLayoutInfo).IsExactlyRowHeight)
        return true;
    }
    return false;
  }

  private bool IsFitRowByUsingVerticalDistance()
  {
    float num = (this.m_lcOperator as Layouter).ClientLayoutArea.Bottom - (this.m_currRowLW.Bounds.Y - this.m_table.TableFormat.Positioning.VertPosition);
    if (!this.m_table.TableFormat.WrapTextAround || this.IsWord2013(this.m_table.Document) || this.m_currRowIndex != 0 || this.m_table.TableFormat.Positioning.VertRelationTo != VerticalRelation.Paragraph || (double) num <= (double) this.m_currRowLW.Bounds.Height || !(this.m_table.PreviousSibling is IWidget) || (this.m_table.PreviousSibling as IWidget).LayoutInfo.IsKeepWithNext)
      return false;
    RowLayoutInfo layoutInfo = this.m_currRowLW.Widget.LayoutInfo as RowLayoutInfo;
    LayoutArea layoutArea = new LayoutArea(this.m_layoutArea.ClientActiveArea with
    {
      Height = layoutInfo == null || !layoutInfo.IsExactlyRowHeight ? num : this.m_currRowLW.Bounds.Height
    });
    this.m_rowLayoutArea = layoutArea;
    this.m_layoutArea = layoutArea;
    return true;
  }

  private bool IsRowSplitByPageBreakBefore(WTableRow tableRow)
  {
    WTableCell cell = tableRow.Cells[0];
    WParagraph wparagraph = (WParagraph) null;
    if (cell.ChildEntities[0] is WParagraph)
      wparagraph = cell.ChildEntities[0] as WParagraph;
    else if (cell.ChildEntities[0] is WTable)
      return this.IsRowSplitByPageBreakBefore((cell.ChildEntities[0] as WTable).FirstRow);
    return wparagraph != null && wparagraph.ParagraphFormat.PageBreakBefore;
  }

  private bool CheckFootnoteInRowIsSplitted(LayoutContext childContext)
  {
    bool flag = false;
    if (childContext.SplittedWidget is SplitWidgetContainer && (childContext.SplittedWidget as SplitWidgetContainer).m_currentChild is SplitWidgetContainer)
    {
      SplitWidgetContainer currentChild = (childContext.SplittedWidget as SplitWidgetContainer).m_currentChild as SplitWidgetContainer;
      WParagraph wparagraph = (WParagraph) null;
      IEntity entity = (IEntity) null;
      if (currentChild.m_currentChild is SplitStringWidget)
      {
        WTextRange realStringWidget = (currentChild.m_currentChild as SplitStringWidget).RealStringWidget as WTextRange;
        wparagraph = realStringWidget.GetOwnerParagraphValue();
        if (realStringWidget.Owner == null)
          wparagraph = realStringWidget.CharacterFormat.BaseFormat.OwnerBase as WParagraph;
        entity = (IEntity) realStringWidget;
      }
      else if (currentChild.m_currentChild is IEntity)
      {
        entity = currentChild.m_currentChild as IEntity;
        if (entity is ParagraphItem)
          wparagraph = (entity as ParagraphItem).GetOwnerParagraphValue();
        if (entity is WTextRange && entity.Owner == null)
          wparagraph = (entity as WTextRange).CharacterFormat.BaseFormat.OwnerBase as WParagraph;
      }
      int num = 0;
      if (wparagraph != null && entity != null)
        num = ((IWidgetContainer) wparagraph).WidgetInnerCollection.InnerList.IndexOf((object) entity);
      for (int index = num; index < currentChild.WidgetInnerCollection.Count; ++index)
      {
        if (currentChild.WidgetInnerCollection[index] is WFootnote)
        {
          flag = true;
          break;
        }
      }
    }
    return flag;
  }

  private LayoutArea CreateRowLayoutArea(WTableRow row, float bottomPad)
  {
    RowLayoutInfo layoutInfo = this.m_currRowLW.Widget.LayoutInfo as RowLayoutInfo;
    this.isRowMoved = !layoutInfo.IsRowSplitted;
    RectangleF clientActiveArea = this.m_layoutArea.ClientActiveArea;
    Spacings margins1 = (this.m_currRowLW.Widget.LayoutInfo as RowLayoutInfo).Margins;
    Spacings paddings1 = (this.m_currRowLW.Widget.LayoutInfo as RowLayoutInfo).Paddings;
    if (((double) row.RowFormat.BeforeWidth != 0.0 || (double) row.RowFormat.GridBeforeWidth.Width != 0.0) && (row.Document.DOP.Dop2000.Copts.AlignTablesRowByRow ? (row.OwnerTable.TableFormat.HorizontalAlignment == RowAlignment.Left ? 1 : 0) : 1) != 0)
      clientActiveArea.X += (double) row.RowFormat.BeforeWidth != 0.0 ? row.RowFormat.BeforeWidth : row.RowFormat.GridBeforeWidth.Width;
    int rowIndex = row.GetRowIndex();
    float num1 = (double) row.RowFormat.CellSpacing > 0.0 ? row.RowFormat.CellSpacing : ((double) row.OwnerTable.TableFormat.CellSpacing > 0.0 ? row.OwnerTable.TableFormat.CellSpacing : 0.0f);
    if (!this.m_table.TableFormat.Bidi && !(this.m_lcOperator as Layouter).IsLayoutingHeaderRow && row.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013)
      paddings1.Left = (double) num1 > 0.0 ? (((IWidget) this.m_table.Rows[0].Cells[0]).LayoutInfo as CellLayoutInfo).Paddings.Left / 2f : (((IWidget) this.m_table.Rows[0].Cells[0]).LayoutInfo as CellLayoutInfo).Paddings.Left;
    else if (this.m_table.TableFormat.Bidi && !(this.m_lcOperator as Layouter).IsLayoutingHeaderRow && row.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013)
      paddings1.Left = (double) num1 > 0.0 ? (((IWidget) this.m_table.Rows[0].Cells[this.m_table.Rows[0].Cells.Count - 1]).LayoutInfo as CellLayoutInfo).Paddings.Left / 2f : (((IWidget) this.m_table.Rows[0].Cells[this.m_table.Rows[0].Cells.Count - 1]).LayoutInfo as CellLayoutInfo).Paddings.Left;
    if ((double) num1 > 0.0 && !(this.m_lcOperator as Layouter).IsLayoutingHeaderRow)
    {
      if (rowIndex == 0 && rowIndex == row.OwnerTable.Rows.Count - 1)
      {
        margins1.Top = num1 * 2f;
        margins1.Bottom = num1 * 2f;
        if (row.OwnerTable.TableFormat.Borders.Top.IsBorderDefined && row.OwnerTable.TableFormat.Borders.Top.BorderType != BorderStyle.None && row.OwnerTable.TableFormat.Borders.Top.BorderType != BorderStyle.Cleared)
          paddings1.Top = row.OwnerTable.TableFormat.Borders.Top.GetLineWidthValue();
        if (row.OwnerTable.TableFormat.Borders.Bottom.IsBorderDefined && row.OwnerTable.TableFormat.Borders.Bottom.BorderType != BorderStyle.None && row.OwnerTable.TableFormat.Borders.Bottom.BorderType != BorderStyle.Cleared)
          paddings1.Bottom = row.OwnerTable.TableFormat.Borders.Bottom.GetLineWidthValue() + bottomPad;
      }
      else if (rowIndex == 0)
      {
        margins1.Top = num1 * 2f;
        if (row.OwnerTable.TableFormat.Borders.Top.IsBorderDefined && row.OwnerTable.TableFormat.Borders.Top.BorderType != BorderStyle.None && row.OwnerTable.TableFormat.Borders.Top.BorderType != BorderStyle.Cleared)
          paddings1.Top = row.OwnerTable.TableFormat.Borders.Top.GetLineWidthValue();
        margins1.Bottom = num1;
        paddings1.Bottom = bottomPad;
      }
      else if (rowIndex == row.OwnerTable.Rows.Count - 1)
      {
        margins1.Top = num1;
        margins1.Bottom = num1 * 2f;
        if (row.OwnerTable.TableFormat.Borders.Bottom.IsBorderDefined && row.OwnerTable.TableFormat.Borders.Bottom.BorderType != BorderStyle.None && row.OwnerTable.TableFormat.Borders.Bottom.BorderType != BorderStyle.Cleared)
          paddings1.Bottom = row.OwnerTable.TableFormat.Borders.Bottom.GetLineWidthValue() + bottomPad;
      }
      else
      {
        margins1.Top = num1;
        margins1.Bottom = num1;
        paddings1.Bottom = bottomPad;
      }
      if (!this.m_table.TableFormat.Bidi && row.OwnerTable.TableFormat.Borders.Right.IsBorderDefined && row.OwnerTable.TableFormat.Borders.Right.BorderType != BorderStyle.None && row.OwnerTable.TableFormat.Borders.Right.BorderType != BorderStyle.Cleared)
        paddings1.Right = row.OwnerTable.TableFormat.Borders.Right.GetLineWidthValue();
      else if (this.m_table.TableFormat.Bidi && row.OwnerTable.TableFormat.Borders.Left.IsBorderDefined && row.OwnerTable.TableFormat.Borders.Left.BorderType != BorderStyle.None && row.OwnerTable.TableFormat.Borders.Left.BorderType != BorderStyle.Cleared)
        paddings1.Right = row.OwnerTable.TableFormat.Borders.Left.GetLineWidthValue();
      margins1.Right = num1 * 2f;
    }
    else if (!(this.m_lcOperator as Layouter).IsLayoutingHeaderRow)
      paddings1.Bottom = bottomPad;
    layoutInfo.IsRowSplitted = false;
    layoutInfo.IsRowHasVerticalMergeStartCell = false;
    float height1 = clientActiveArea.Height;
    LayoutArea layoutArea = new LayoutArea();
    LayoutArea rowLayoutArea;
    if (layoutInfo.IsVerticalText)
    {
      Spacings margins2 = (((IWidget) row.Cells[0]).LayoutInfo as CellLayoutInfo).Margins;
      Spacings paddings2 = (((IWidget) row.Cells[0]).LayoutInfo as CellLayoutInfo).Paddings;
      if (layoutInfo.IsExactlyRowHeight)
      {
        float num2 = (float) layoutInfo.RowHeight + margins2.Bottom;
        rowLayoutArea = new LayoutArea(new RectangleF(clientActiveArea.X + paddings1.Left, clientActiveArea.Y + margins1.Top + paddings1.Top, clientActiveArea.Width - paddings1.Right, num2 - (margins1.Top + paddings1.Top)));
        this.m_currRowLW.Bounds = new RectangleF(rowLayoutArea.ClientArea.X, rowLayoutArea.ClientArea.Y, rowLayoutArea.ClientArea.Width, rowLayoutArea.ClientArea.Height + margins1.Bottom + paddings1.Bottom);
      }
      else if (row.HeightType == TableRowHeightType.AtLeast)
      {
        float height2 = (float) layoutInfo.RowHeight + margins2.Top + margins2.Bottom + paddings2.Top;
        rowLayoutArea = new LayoutArea(new RectangleF(clientActiveArea.X + paddings1.Left, clientActiveArea.Y + margins1.Top + paddings1.Top, clientActiveArea.Width - paddings1.Right, height2));
        this.m_currRowLW.Bounds = rowLayoutArea.ClientArea;
      }
      else
      {
        float height3 = (row.Cells[0].LastParagraph as IWidget).LayoutInfo.Size.Height + margins2.Top + margins2.Bottom + paddings2.Top + margins1.Bottom + paddings1.Bottom;
        rowLayoutArea = new LayoutArea(new RectangleF(clientActiveArea.X + paddings1.Left, clientActiveArea.Y + margins1.Top + paddings1.Top, clientActiveArea.Width - paddings1.Right, height3));
        this.m_currRowLW.Bounds = rowLayoutArea.ClientArea;
      }
    }
    else
    {
      if (layoutInfo.IsExactlyRowHeight)
      {
        float num3 = (float) layoutInfo.RowHeight + (((IWidget) row.Cells[0]).LayoutInfo as CellLayoutInfo).Margins.Bottom;
        rowLayoutArea = new LayoutArea(new RectangleF(clientActiveArea.X + paddings1.Left, clientActiveArea.Y + margins1.Top + paddings1.Top, clientActiveArea.Width - paddings1.Right, num3 - (margins1.Top + paddings1.Top)));
        this.m_currRowLW.Bounds = new RectangleF(rowLayoutArea.ClientArea.X, rowLayoutArea.ClientArea.Y, rowLayoutArea.ClientArea.Width, rowLayoutArea.ClientArea.Height + margins1.Bottom + paddings1.Bottom);
      }
      else
        rowLayoutArea = new LayoutArea(new RectangleF(clientActiveArea.X + paddings1.Left, clientActiveArea.Y + margins1.Top + paddings1.Top, clientActiveArea.Width - paddings1.Right, height1 - (margins1.Top + margins1.Bottom + paddings1.Top + paddings1.Bottom)));
      if (row.HeightType == TableRowHeightType.AtLeast)
      {
        Spacings margins3 = (((IWidget) row.Cells[0]).LayoutInfo as CellLayoutInfo).Margins;
        Spacings paddings3 = (((IWidget) row.Cells[0]).LayoutInfo as CellLayoutInfo).Paddings;
        float height4 = (float) layoutInfo.RowHeight + margins3.Top + margins3.Bottom + paddings3.Top + margins1.Bottom + paddings1.Bottom;
        this.m_currRowLW.Bounds = new RectangleF(rowLayoutArea.ClientArea.X, rowLayoutArea.ClientArea.Y, rowLayoutArea.ClientArea.Width, height4);
      }
    }
    return rowLayoutArea;
  }

  private void GetCellsMaxTopAndBottomPadding(
    WTableRow row,
    out float maxTopPading,
    out float maxBottomPadding,
    out float maxTopMargin,
    out float maxBottomMargin)
  {
    maxBottomPadding = 0.0f;
    maxTopPading = 0.0f;
    maxTopMargin = 0.0f;
    maxBottomMargin = 0.0f;
    RowLayoutInfo layoutInfo1 = ((IWidget) row).LayoutInfo as RowLayoutInfo;
    for (int index = 0; index < row.Cells.Count; ++index)
    {
      CellLayoutInfo layoutInfo2 = ((IWidget) row.Cells[index]).LayoutInfo as CellLayoutInfo;
      if (!DocumentLayouter.IsFirstLayouting)
      {
        layoutInfo2.IsRowMergeStart = false;
        layoutInfo2.IsRowMergeContinue = false;
        layoutInfo2.IsRowMergeEnd = false;
        layoutInfo2.InitMerges();
      }
      if (!(this.m_lcOperator as Layouter).IsLayoutingHeaderRow && !row.IsHeader && (this.m_table.m_layoutInfo as Syncfusion.Layouting.TableLayoutInfo).IsSplittedTable && (double) row.RowFormat.CellSpacing <= 0.0 && (double) this.m_table.TableFormat.CellSpacing <= 0.0 && this.IsForceFitLayout && this.m_ltWidget.ChildWidgets.Count >= 1 && layoutInfo2.UpdatedSplittedTopBorders == null)
      {
        layoutInfo2.UpdatedSplittedTopBorders = new Dictionary<CellLayoutInfo.CellBorder, float>();
        double topHalfWidth = (double) layoutInfo2.GetTopHalfWidth(index, row.GetRowIndex(), row.Cells[index], this.m_ltWidget.ChildWidgets.Count - 1);
      }
      if (this.m_ltWidget.ChildWidgets.Count == 0 && (double) row.RowFormat.CellSpacing <= 0.0 && (double) this.m_table.TableFormat.CellSpacing <= 0.0 && this.m_currRowIndex != 0)
      {
        WTableCell wtableCell = row.Cells[index];
        if (layoutInfo2.IsRowMergeContinue)
        {
          WTableCell verticalMergeStartCell = wtableCell.GetVerticalMergeStartCell();
          if (verticalMergeStartCell != null)
          {
            wtableCell = verticalMergeStartCell;
            layoutInfo2 = ((IWidget) verticalMergeStartCell).LayoutInfo as CellLayoutInfo;
          }
        }
        Border top = wtableCell.CellFormat.Borders.Top;
        if (!top.IsBorderDefined || top.IsBorderDefined && top.BorderType == BorderStyle.None && (double) top.LineWidth == 0.0 && top.Color.IsEmpty)
          top = wtableCell.OwnerRow.RowFormat.Borders.Top;
        if (!top.IsBorderDefined)
          top = wtableCell.OwnerRow.OwnerTable.TableFormat.Borders.Top;
        if (top.IsBorderDefined)
        {
          layoutInfo2.TopBorder = new CellLayoutInfo.CellBorder(top.BorderType, top.Color, top.GetLineWidthValue(), top.LineWidth);
          layoutInfo2.TopPadding = layoutInfo2.TopBorder.BorderType == BorderStyle.None || layoutInfo2.TopBorder.BorderType == BorderStyle.Cleared ? 0.0f : top.GetLineWidthValue();
          layoutInfo2.SkipTopBorder = false;
          if (!(wtableCell.OwnerRow.m_layoutInfo as RowLayoutInfo).IsRowSplitted && !layoutInfo2.IsRowMergeStart && (layoutInfo2.TopBorder.BorderType == BorderStyle.Cleared || layoutInfo2.TopBorder.BorderType == BorderStyle.None))
            layoutInfo2.SkipTopBorder = true;
        }
      }
      float num = this.m_currRowIndex == 0 || (double) row.RowFormat.CellSpacing > 0.0 || (double) this.m_table.TableFormat.CellSpacing > 0.0 ? layoutInfo2.TopPadding : (layoutInfo1.IsRowSplitted ? layoutInfo2.TopPadding : layoutInfo2.UpdatedTopPadding);
      if ((double) maxTopPading < (double) num)
        maxTopPading = num;
      if ((double) maxBottomPadding < (double) layoutInfo2.BottomPadding)
        maxBottomPadding = layoutInfo2.BottomPadding;
      if ((double) maxTopMargin < (double) layoutInfo2.Margins.Top)
        maxTopMargin = layoutInfo2.Margins.Top;
      if ((double) maxBottomMargin < (double) layoutInfo2.Margins.Bottom)
        maxBottomMargin = layoutInfo2.Margins.Bottom;
    }
  }

  private void UpdateCellsMaxTopAndBottomPadding(
    WTableRow row,
    float maxTopPading,
    float maxBottomPading)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    for (int index = 0; index < row.Cells.Count; ++index)
    {
      CellLayoutInfo layoutInfo = ((IWidget) row.Cells[index]).LayoutInfo as CellLayoutInfo;
      float num3 = (double) layoutInfo.Margins.Top + (double) layoutInfo.TopPadding < (double) maxTopPading ? 0.0f : layoutInfo.Margins.Top + layoutInfo.TopPadding - maxTopPading;
      if ((double) num1 < (double) num3)
        num1 = num3;
      layoutInfo.Paddings.Top = maxTopPading;
      float num4 = (double) layoutInfo.Margins.Bottom + (double) layoutInfo.BottomPadding < (double) maxBottomPading ? 0.0f : layoutInfo.Margins.Bottom + layoutInfo.BottomPadding - maxBottomPading;
      if ((double) num2 < (double) num4)
        num2 = num4;
    }
    for (int index = 0; index < row.Cells.Count; ++index)
    {
      CellLayoutInfo layoutInfo = ((IWidget) row.Cells[index]).LayoutInfo as CellLayoutInfo;
      layoutInfo.Margins.Top = num1;
      layoutInfo.Margins.Bottom = num2;
    }
    (((IWidget) row).LayoutInfo as RowLayoutInfo).IsCellPaddingUpdated = true;
  }

  private float UpdateCellsBottomPaddingAndMargin(
    WTableRow row,
    float maxBottomPading,
    float maxBottomMargin)
  {
    float num1 = 0.0f;
    for (int index = 0; index < row.Cells.Count; ++index)
    {
      CellLayoutInfo layoutInfo = ((IWidget) row.Cells[index]).LayoutInfo as CellLayoutInfo;
      float num2 = (double) layoutInfo.BottomPadding - ((double) maxBottomMargin - (double) layoutInfo.Margins.Bottom) < 0.0 ? 0.0f : layoutInfo.BottomPadding - (maxBottomMargin - layoutInfo.Margins.Bottom);
      layoutInfo.Margins.Bottom = maxBottomMargin;
      if ((double) num1 < (double) num2)
        num1 = num2;
    }
    return num1;
  }

  private void CommitRow()
  {
    if (!this.m_table.IsInCell)
      (this.m_lcOperator as Layouter).IsRowFitInSamePage = false;
    RowLayoutInfo layoutInfo1 = this.m_currRowLW.Widget.LayoutInfo as RowLayoutInfo;
    LayoutedWidget rowLW = this.m_currRowLW;
    WTableRow widget = this.m_currRowLW.Widget as WTableRow;
    if (this.m_ltState == LayoutState.NotFitted && this.m_ltWidget.ChildWidgets.Count > 0)
    {
      layoutInfo1.IsRowSplitted = false;
      LayoutedWidget childWidget = this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1];
      if (childWidget.Widget.LayoutInfo is RowLayoutInfo)
      {
        RowLayoutInfo layoutInfo2 = childWidget.Widget.LayoutInfo as RowLayoutInfo;
        if (!layoutInfo2.IsRowHasVerticalMergeEndCell || layoutInfo2.IsRowHasVerticalMergeStartCell || this.IsPreviousRowHasVerticalMergeContinueCell(childWidget.Widget as WTableRow))
          rowLW = childWidget;
      }
      if (this.m_currRowIndex - 1 >= 0 && this.IsFirstItemInPage)
      {
        if (this.IsWord2013(this.m_table.Document))
        {
          if (!layoutInfo1.IsRowBreakByPageBreakBefore && (double) (this.m_widget.LayoutInfo as Syncfusion.Layouting.TableLayoutInfo).HeaderRowHeight + (double) this.m_table.Rows[this.m_currRowIndex].Height >= (double) this.m_layoutArea.ClientArea.Height)
          {
            (this.m_widget.LayoutInfo as Syncfusion.Layouting.TableLayoutInfo).IsHeaderNotRepeatForAllPages = true;
          }
          else
          {
            (this.m_widget.LayoutInfo as Syncfusion.Layouting.TableLayoutInfo).IsHeaderNotRepeatForAllPages = false;
            layoutInfo1.IsRowBreakByPageBreakBefore = false;
          }
        }
        else if ((double) (this.m_widget.LayoutInfo as Syncfusion.Layouting.TableLayoutInfo).HeaderRowHeight + (double) this.m_table.Rows[this.m_currRowIndex].Height >= (double) this.m_layoutArea.ClientArea.Height)
          (this.m_widget.LayoutInfo as Syncfusion.Layouting.TableLayoutInfo).IsHeaderNotRepeatForAllPages = true;
      }
      this.UpdateSplittedCells();
    }
    this.UpdateVerticalMergedCell(rowLW, this.m_ltState == LayoutState.NotFitted);
    if (layoutInfo1.IsRowHasVerticalTextCell && !layoutInfo1.IsVerticalText && this.m_currRowLW.ChildWidgets.Count > 0 && this.m_ltState != LayoutState.NotFitted)
      this.UpdateVerticalTextCellLW();
    this.m_currRowLW.Bounds = new RectangleF(this.m_currRowLW.Bounds.X, this.m_currRowLW.Bounds.Y, this.m_currRowLW.Bounds.Width, this.m_currRowLW.Bounds.Height + (this.CurrRowIndex == this.m_table.Rows.Count - 1 || (double) (this.m_currRowLW.Widget as WTableRow).RowFormat.CellSpacing > 0.0 || (double) this.m_table.TableFormat.CellSpacing > 0.0 || this.m_blastRowState == LayoutState.Splitted ? (this.m_currRowLW.Widget.LayoutInfo as RowLayoutInfo).Paddings.Bottom + (this.m_currRowLW.Widget.LayoutInfo as RowLayoutInfo).Margins.Bottom : 0.0f));
    bool flag1 = true;
    if (this.m_ltState == LayoutState.Unknown && this.m_bAtLastOneCellFitted)
    {
      IWidget childParaWidget = this.GetChildParaWidget(this.m_currRowLW);
      bool flag2 = false;
      if (this.m_table.TableFormat.WrapTextAround && widget.Index == 0 && !this.m_table.IsInCell && this.IsWord2013(this.m_table.Document))
        flag2 = this.IsForceFitLayout;
      if ((childParaWidget != null && childParaWidget is WParagraph && (childParaWidget as WParagraph).ParagraphFormat.PageBreakBefore || Math.Round((double) this.m_currRowLW.Bounds.Height, 2) > (double) this.m_layoutArea.ClientActiveArea.Height && this.m_blastRowState != LayoutState.Splitted) && !this.IsForceFitLayout && !flag2 && widget.OwnerTable.ChildEntities.Count == 1 && Math.Round((double) this.m_currRowLW.Bounds.Bottom, 2) > Math.Round((double) (this.m_lcOperator as Layouter).ClientLayoutArea.Bottom, 2) && !widget.OwnerTable.IsFrame)
      {
        this.m_sptWidget = (IWidget) new SplitTableWidget(this.TableWidget, this.CurrRowIndex + 1);
        this.m_ltState = LayoutState.Splitted;
      }
      else
      {
        for (int index = 0; index < this.m_currRowLW.ChildWidgets.Count; ++index)
        {
          if (!layoutInfo1.IsExactlyRowHeight)
          {
            this.UpdateCellHeight(index);
          }
          else
          {
            LayoutedWidget childWidget = this.m_currRowLW.ChildWidgets[index];
            if (childWidget.Widget.LayoutInfo.IsVerticalText)
              this.UpdateVerticalTextCellAlignment(childWidget);
            else
              this.UpdateVerticalCellAlignment(childWidget);
            if (!childWidget.Widget.LayoutInfo.IsVerticalText)
            {
              RectangleF bounds = childWidget.ChildWidgets[0].Bounds;
              bounds.Height = childWidget.Bounds.Bottom - bounds.Top - (childWidget.Widget.LayoutInfo as CellLayoutInfo).Margins.Bottom;
              childWidget.ChildWidgets[0].Bounds = bounds;
            }
          }
        }
        if (this.m_blastRowState != LayoutState.Splitted && this.m_table.IsInCell && !this.IsForceFitLayout)
        {
          for (int index = 0; index < this.m_currRowLW.ChildWidgets.Count; ++index)
          {
            LayoutedWidget childWidget = this.m_currRowLW.ChildWidgets[index].ChildWidgets[0];
            if (childWidget.ChildWidgets.Count > 0 && childWidget.ChildWidgets[0].Widget is WTable && childWidget.ChildWidgets[0].ChildWidgets.Count > 0)
            {
              RowLayoutInfo layoutInfo3 = childWidget.ChildWidgets[0].ChildWidgets[0].Widget.LayoutInfo as RowLayoutInfo;
              if (childWidget.ChildWidgets.Count == 1 && layoutInfo3.RowHeight > this.m_rowLayoutArea.Height)
              {
                this.m_sptWidget = (IWidget) new SplitTableWidget(this.TableWidget, this.CurrRowIndex + 1);
                this.m_ltState = LayoutState.Splitted;
                return;
              }
            }
          }
        }
        bool isForceFitLayout = this.IsForceFitLayout;
        if (this.m_blastRowState == LayoutState.Splitted)
        {
          if (this.IsRowNeedToBeSplitted(isForceFitLayout))
          {
            if (!this.IsWord2013(this.m_table.Document) && (this.m_table.IsInCell ? (!this.m_table.GetOwnerTableCell().OwnerRow.RowFormat.IsBreakAcrossPages || !widget.RowFormat.IsBreakAcrossPages ? (isForceFitLayout ? 1 : 0) : 0) : 0) != 0)
            {
              for (int index = this.CurrRowIndex + 1; index < this.m_table.Rows.Count; ++index)
                ((IWidget) this.m_table.Rows[index]).LayoutInfo.IsSkip = true;
            }
            if (widget.RowFormat.IsBreakAcrossPages && !widget.IsHeader || this.IsWord2013(this.m_table.Document) || !isForceFitLayout)
            {
              this.m_sptWidget = (IWidget) new SplitTableWidget(this.TableWidget, this.CurrRowIndex + 1, this.m_splitedCells);
              this.m_ltState = LayoutState.Splitted;
            }
          }
          else
          {
            flag1 = false;
            if (this.m_ltWidget.ChildWidgets.Count > 0)
            {
              if (layoutInfo1.IsRowHasVerticalMergeStartCell && layoutInfo1.IsRowSplitted || layoutInfo1.IsRowHasVerticalMergeEndCell || layoutInfo1.IsRowSplitted && layoutInfo1.IsRowHasVerticalMergeContinueCell)
              {
                LayoutedWidget childWidget = this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1];
                if (childWidget.Widget.LayoutInfo is RowLayoutInfo)
                {
                  RowLayoutInfo layoutInfo4 = childWidget.Widget.LayoutInfo as RowLayoutInfo;
                  if (!layoutInfo4.IsRowHasVerticalMergeEndCell || layoutInfo4.IsRowHasVerticalMergeStartCell)
                    rowLW = childWidget;
                }
                this.m_splitedCells = new SplitWidgetContainer[this.m_table.Rows[this.CurrRowIndex].Cells.Count];
                for (int index = 0; index < this.m_table.Rows[this.CurrRowIndex].Cells.Count; ++index)
                  this.m_splitedCells[index] = this.m_table.Rows[this.CurrRowIndex].Cells[index].ChildEntities.Count > 0 ? new SplitWidgetContainer((IWidgetContainer) this.m_table.Rows[this.CurrRowIndex].Cells[index], this.m_table.Rows[this.CurrRowIndex].Cells[index].ChildEntities[0] as IWidget, 0) : new SplitWidgetContainer((IWidgetContainer) this.m_table.Rows[this.CurrRowIndex].Cells[index]);
                this.UpdateVerticalMergedCell(rowLW, true);
                if (this.IsNotEmptySplittedCell())
                {
                  this.m_sptWidget = (IWidget) new SplitTableWidget(this.TableWidget, this.CurrRowIndex + 1, this.m_splitedCells);
                  this.m_ltState = LayoutState.Splitted;
                }
                else
                {
                  this.m_sptWidget = (IWidget) new SplitTableWidget(this.TableWidget, this.CurrRowIndex + 1);
                  this.m_ltState = LayoutState.Splitted;
                }
              }
              else
              {
                this.m_sptWidget = (IWidget) new SplitTableWidget(this.TableWidget, this.CurrRowIndex + 1);
                this.m_ltState = LayoutState.Splitted;
              }
            }
            else
            {
              this.m_ltState = LayoutState.NotFitted;
              if (DocumentLayouter.IsUpdatingTOC && !widget.RowFormat.IsBreakAcrossPages && !this.IsWord2013(this.m_table.Document))
                (this.m_lcOperator as Layouter).IsRowFitInSamePage = true;
            }
          }
        }
        if (flag1)
        {
          this.m_ltWidget.ChildWidgets.Add(this.m_currRowLW);
          this.m_currRowLW.Owner = this.m_ltWidget;
          this.UpdateLWBounds();
          this.UpdateClientArea();
          if (!(this.m_lcOperator as Layouter).IsLayoutingHeaderRow)
            this.UpdateForceFitLayoutState((LayoutContext) this);
        }
        if (!flag1 || (this.m_lcOperator as Layouter).IsLayoutingHeaderRow || !widget.IsHeader || (this.LayoutInfo as Syncfusion.Layouting.TableLayoutInfo).IsHeaderRowHeightUpdated || widget.Index != 0 && !(widget.PreviousSibling as WTableRow).IsHeader)
          return;
        (this.LayoutInfo as Syncfusion.Layouting.TableLayoutInfo).HeaderRowHeight += this.m_currRowLW.Bounds.Height;
      }
    }
    else
    {
      if (this.m_ltWidget.ChildWidgets.Count <= 0)
        return;
      if (this.IsNotEmptySplittedCell())
      {
        this.m_sptWidget = (IWidget) new SplitTableWidget(this.TableWidget, this.CurrRowIndex + 1, this.m_splitedCells);
        this.m_ltState = LayoutState.Splitted;
      }
      else
      {
        this.m_sptWidget = (IWidget) new SplitTableWidget(this.TableWidget, this.CurrRowIndex + 1);
        this.m_ltState = LayoutState.Splitted;
      }
      (this.TableLayoutInfo as ITableLayoutInfo).IsSplittedTable = true;
    }
  }

  private bool IsNeedToLayoutHeaderRow()
  {
    if (this.m_table.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013)
    {
      WTableRow widget = this.m_currRowLW.Widget as WTableRow;
      if (widget.NextSibling is WTableRow && !(widget.NextSibling as WTableRow).IsHeader)
        return true;
    }
    return false;
  }

  private void UpdateVerticalMergedCell(LayoutedWidget rowLW, bool isNextRowNotFitted)
  {
    RowLayoutInfo layoutInfo = rowLW.Widget.LayoutInfo as RowLayoutInfo;
    bool flag1 = false;
    WTableRow widget1 = rowLW.Widget is WTableRow ? rowLW.Widget as WTableRow : (WTableRow) null;
    bool flag2 = false;
    bool flag3 = false;
    List<LayoutedWidget> layoutedWidgetList1 = new List<LayoutedWidget>();
    if (widget1 != null && widget1.IsHeader && (layoutInfo.IsRowHasVerticalMergeContinueCell || layoutInfo.IsRowHasVerticalMergeStartCell))
      flag1 = this.IsNeedToLayoutHeaderRow();
    if (layoutInfo.IsRowHasVerticalMergeStartCell && (layoutInfo.IsRowSplitted || isNextRowNotFitted || flag1) || layoutInfo.IsRowHasVerticalMergeEndCell || (layoutInfo.IsRowSplitted || isNextRowNotFitted || flag1) && layoutInfo.IsRowHasVerticalMergeContinueCell)
    {
      List<LayoutedWidget> layoutedWidgetList2 = new List<LayoutedWidget>();
      int count1 = (this.m_lcOperator as Layouter).FootnoteWidgets.Count;
      for (int index1 = 0; index1 < rowLW.ChildWidgets.Count; ++index1)
      {
        LayoutedWidget childWidget = rowLW.ChildWidgets[index1];
        LayoutedWidget mergeStartLW;
        LayoutContext layoutContext;
        LayoutedWidget cellLW;
        while (true)
        {
          mergeStartLW = (childWidget.Widget.LayoutInfo as CellLayoutInfo).IsRowMergeContinue && (childWidget.Widget.LayoutInfo as CellLayoutInfo).IsRowMergeEnd || layoutInfo.IsRowHasVerticalMergeEndCell && (childWidget.Widget.LayoutInfo as CellLayoutInfo).IsRowMergeStart && (childWidget.Widget.LayoutInfo as CellLayoutInfo).IsRowMergeEnd || (childWidget.Widget.LayoutInfo as CellLayoutInfo).IsRowMergeContinue && (layoutInfo.IsRowSplitted || isNextRowNotFitted || flag1) ? this.GetVerticalMergeStartLW(childWidget) : (!(childWidget.Widget.LayoutInfo as CellLayoutInfo).IsRowMergeStart || !layoutInfo.IsRowSplitted && !isNextRowNotFitted && !flag1 ? (LayoutedWidget) null : childWidget);
          if (mergeStartLW != null)
          {
            if ((mergeStartLW.Widget.LayoutInfo as CellLayoutInfo).IsVerticalText)
            {
              if (!layoutedWidgetList2.Contains(childWidget))
              {
                if (index1 != rowLW.ChildWidgets.Count - 1)
                  break;
              }
              else
                layoutedWidgetList2.Remove(childWidget);
            }
            float height = layoutInfo.IsExactlyRowHeight || flag1 && (rowLW.Widget as WTableRow).HeightType != TableRowHeightType.AtLeast || isNextRowNotFitted || (mergeStartLW.Widget.LayoutInfo as CellLayoutInfo).IsVerticalText || layoutInfo.IsRowSplitted && layoutInfo.IsRowHasVerticalMergeContinueCell ? rowLW.Bounds.Bottom - mergeStartLW.Bounds.Top : this.m_layoutArea.ClientActiveArea.Bottom - mergeStartLW.Bounds.Top;
            LayoutArea layoutArea = new LayoutArea(new RectangleF(mergeStartLW.Bounds.Left, mergeStartLW.Bounds.Top, mergeStartLW.Bounds.Width, height));
            layoutContext = LayoutContext.Create(mergeStartLW.Widget, this.m_lcOperator, this.IsForceFitLayout || !this.IsWord2013(widget1.Document));
            if (mergeStartLW != childWidget)
              (mergeStartLW.Widget.LayoutInfo as CellLayoutInfo).Margins.Bottom = (childWidget.Widget.LayoutInfo as CellLayoutInfo).Margins.Bottom;
            layoutContext.ClientLayoutAreaRight = (float) layoutArea.Width;
            (this.m_currRowLW.Widget.LayoutInfo as RowLayoutInfo).IsHiddenRow = false;
            cellLW = this.LayoutCell(layoutContext, layoutArea.ClientArea, false);
            cellLW.Bounds = new RectangleF(cellLW.Bounds.X, cellLW.Bounds.Y, cellLW.Bounds.Width, layoutInfo.IsExactlyRowHeight || flag1 && (rowLW.Widget as WTableRow).HeightType != TableRowHeightType.AtLeast || isNextRowNotFitted || (mergeStartLW.Widget.LayoutInfo as CellLayoutInfo).IsVerticalText || layoutInfo.IsRowSplitted && layoutInfo.IsRowHasVerticalMergeContinueCell ? layoutArea.ClientArea.Height : ((double) cellLW.Bounds.Bottom < (double) rowLW.Bounds.Bottom ? rowLW.Bounds.Bottom : cellLW.Bounds.Bottom) - layoutArea.ClientArea.Top);
            if ((double) cellLW.Bounds.Bottom >= (double) this.m_layoutArea.ClientActiveArea.Bottom && layoutInfo.IsRowHasVerticalMergeEndCell && !this.IsWord2013(widget1.Document) && !layoutInfo.IsKeepWithNext && !flag2 && !this.IsRowHasExactlyHeight((Entity) (rowLW.Widget as WTableRow)))
            {
              this.UpdateSplittedCells();
              this.m_ltState = LayoutState.NotFitted;
              layoutContext.m_ltState = LayoutState.Splitted;
              flag2 = true;
            }
            if (layoutContext.State != LayoutState.NotFitted)
            {
              if ((mergeStartLW.Widget.LayoutInfo as CellLayoutInfo).IsVerticalText)
                this.UpdateVerticalTextCellAlignment(cellLW);
              else
                this.UpdateVerticalCellAlignment(cellLW);
              if (!(mergeStartLW.Widget.LayoutInfo as CellLayoutInfo).IsVerticalText)
              {
                RectangleF bounds = cellLW.ChildWidgets[0].Bounds;
                bounds.Height = cellLW.Bounds.Bottom - bounds.Top - (mergeStartLW.Widget.LayoutInfo as CellLayoutInfo).Margins.Bottom;
                cellLW.ChildWidgets[0].Bounds = bounds;
              }
              this.m_currCellLW = cellLW;
              this.m_currCellLW.Owner = mergeStartLW.Owner;
              if (layoutInfo.IsRowHasVerticalMergeEndCell && !(mergeStartLW.Widget.LayoutInfo as CellLayoutInfo).IsVerticalText)
                this.UpdateRowLWBounds();
              this.UpdateSplittedVerticalMergeCell(layoutContext, mergeStartLW, layoutInfo, childWidget, rowLW, index1, isNextRowNotFitted);
              if ((childWidget.Widget.LayoutInfo as CellLayoutInfo).IsRowMergeStart)
              {
                if (!(mergeStartLW.Widget.LayoutInfo as CellLayoutInfo).IsVerticalText)
                  rowLW.ChildWidgets[index1] = this.m_currCellLW;
                else
                  rowLW.ChildWidgets[rowLW.ChildWidgets.IndexOf(childWidget)] = this.m_currCellLW;
                if (this.m_verticallyMergeStartLW.Contains(childWidget))
                  this.m_verticallyMergeStartLW.Remove(childWidget);
              }
              else if ((childWidget.Widget.LayoutInfo as CellLayoutInfo).IsRowMergeContinue || layoutInfo.IsRowHasVerticalMergeEndCell && (childWidget.Widget.LayoutInfo as CellLayoutInfo).IsRowMergeStart && (childWidget.Widget.LayoutInfo as CellLayoutInfo).IsRowMergeEnd)
              {
                for (int index2 = 0; index2 < mergeStartLW.Owner.ChildWidgets.Count; ++index2)
                {
                  if (mergeStartLW.Owner.ChildWidgets[index2].Widget == mergeStartLW.Widget)
                  {
                    layoutedWidgetList1.Add(this.m_currCellLW);
                    mergeStartLW.Owner.ChildWidgets[index2] = this.m_currCellLW;
                    break;
                  }
                }
                if (!layoutInfo.IsExactlyRowHeight && layoutContext.m_ltState == LayoutState.Splitted && !isNextRowNotFitted)
                {
                  this.m_blastRowState = LayoutState.Splitted;
                  layoutInfo.IsRowSplitted = true;
                  (this.TableLayoutInfo as ITableLayoutInfo).IsSplittedTable = true;
                }
                this.m_verticallyMergeStartLW.Remove(mergeStartLW);
              }
            }
            else
              goto label_16;
          }
          if (index1 == rowLW.ChildWidgets.Count - 1 && layoutedWidgetList2.Count > 0)
            childWidget = layoutedWidgetList2[0];
          else
            goto label_46;
        }
        layoutedWidgetList2.Add(childWidget);
        continue;
label_16:
        WTableCell wtableCell1 = cellLW.Widget is WTableCell ? cellLW.Widget as WTableCell : (!(cellLW.Widget is SplitWidgetContainer) || !((cellLW.Widget as SplitWidgetContainer).RealWidgetContainer is WTableCell) ? (WTableCell) null : (cellLW.Widget as SplitWidgetContainer).RealWidgetContainer as WTableCell);
        this.m_currColIndex = wtableCell1 != null ? wtableCell1.GetIndexInOwnerCollection() : -1;
        WTableCell wtableCell2 = layoutContext.Widget is WTableCell ? layoutContext.Widget as WTableCell : (!(layoutContext.Widget is SplitWidgetContainer) || !((layoutContext.Widget as SplitWidgetContainer).RealWidgetContainer is WTableCell) ? (WTableCell) null : (layoutContext.Widget as SplitWidgetContainer).RealWidgetContainer as WTableCell);
        IWidget widget2 = layoutContext.Widget is SplitTableWidget ? (IWidget) (layoutContext.Widget as SplitWidgetContainer) : (wtableCell2 != null ? (IWidget) new SplitWidgetContainer(layoutContext.Widget as IWidgetContainer, wtableCell2.ChildEntities[0] as IWidget, 0) : (IWidget) null);
        if (isNextRowNotFitted || flag3)
        {
          rowLW = this.m_currRowLW;
          layoutInfo = this.m_currRowLW.Widget.LayoutInfo as RowLayoutInfo;
          if (widget2 != null)
          {
            layoutContext.SplittedWidget = widget2;
            this.UpdateSplittedVerticalMergeCell(layoutContext, mergeStartLW, layoutInfo, childWidget, this.m_currRowLW, index1, true);
          }
        }
        else
        {
          int count2 = this.m_ltWidget.ChildWidgets.Count;
          this.MarkAsNotFitted(layoutContext);
          this.UpdateSplittedCells();
          if (widget2 != null && this.m_ltWidget.ChildWidgets.Count == count2)
          {
            layoutContext.SplittedWidget = widget2;
            this.UpdateSplittedVerticalMergeCell(layoutContext, mergeStartLW, layoutInfo, childWidget, this.m_currRowLW, index1, isNextRowNotFitted);
          }
        }
        flag3 = true;
        continue;
label_46:
        if (mergeStartLW == null && widget1.NextSibling is WTableRow && this.m_verticallyMergeStartLW.Count > 0 && this.m_verticallyMergeStartLW[0].Widget is SplitWidgetContainer)
        {
          WTableRow nextSibling = widget1.NextSibling as WTableRow;
          if (index1 < nextSibling.Cells.Count && ((IWidget) nextSibling.Cells[index1]).LayoutInfo is CellLayoutInfo && !(nextSibling.Cells[index1].m_layoutInfo as CellLayoutInfo).IsRowMergeContinue && this.m_verticallyMergeStartLW.Contains(childWidget))
            this.m_verticallyMergeStartLW.Remove(childWidget);
        }
      }
      if (layoutedWidgetList1.Count > 0)
      {
        foreach (LayoutedWidget cellLW in layoutedWidgetList1)
        {
          if ((double) cellLW.Bounds.Bottom < (double) rowLW.Bounds.Bottom)
          {
            cellLW.Bounds = new RectangleF(cellLW.Bounds.X, cellLW.Bounds.Y, cellLW.Bounds.Width, rowLW.Bounds.Bottom - cellLW.Bounds.Top);
            if (!(cellLW.Widget.LayoutInfo as CellLayoutInfo).IsVerticalText)
            {
              this.UpdateVerticalCellAlignment(cellLW);
              RectangleF bounds = cellLW.ChildWidgets[0].Bounds;
              bounds.Height = cellLW.Bounds.Bottom - bounds.Top - (cellLW.Widget.LayoutInfo as CellLayoutInfo).Margins.Bottom;
              cellLW.ChildWidgets[0].Bounds = bounds;
            }
          }
        }
      }
      if (count1 > 0 && (this.m_lcOperator as Layouter).FootnoteWidgets.Count > count1)
        this.UpdateUnorderedFootNotesBounds((this.m_lcOperator as Layouter).FootnoteWidgets.Count - count1);
    }
    else if (isNextRowNotFitted)
      this.m_splitedCells = new SplitWidgetContainer[this.m_table.Rows[this.CurrRowIndex].Cells.Count];
    if (this.m_ltState == LayoutState.NotFitted || !flag3)
      return;
    this.m_ltState = LayoutState.NotFitted;
  }

  private bool IsRowHasExactlyHeight(Entity entity)
  {
    while (!(entity is WTableRow) || (entity as WTableRow).HeightType != TableRowHeightType.Exactly)
    {
      entity = entity.Owner;
      if (entity is WSection)
        return false;
    }
    return true;
  }

  private void UpdateUnorderedFootNotesBounds(int unOrderCount)
  {
    LayoutedWidgetList footnoteWidgets = (this.m_lcOperator as Layouter).FootnoteWidgets;
    if (footnoteWidgets.Count <= 2 || footnoteWidgets.Count < unOrderCount)
      return;
    for (; unOrderCount > 0; --unOrderCount)
    {
      LayoutedWidget layoutedWidget1 = footnoteWidgets[footnoteWidgets.Count - unOrderCount];
      int index1 = -1;
      if (this.IsFootNote(layoutedWidget1.Widget))
      {
        int footNoteId = this.GetFootNoteID(layoutedWidget1.Widget);
        for (int index2 = footnoteWidgets.Count - unOrderCount - 1; index2 > 0; --index2)
        {
          LayoutedWidget layoutedWidget2 = footnoteWidgets[index2];
          if (this.IsFootNote(layoutedWidget2.Widget) && this.GetFootNoteID(layoutedWidget2.Widget) > footNoteId)
          {
            float yOffset = layoutedWidget2.Bounds.Y - layoutedWidget1.Bounds.Y;
            layoutedWidget1.ShiftLocation(0.0, (double) yOffset, true, false, true);
            layoutedWidget2.ShiftLocation(0.0, (double) Math.Abs(yOffset), true, false, true);
            index1 = index2;
          }
        }
        if (index1 != -1)
        {
          footnoteWidgets.RemoveAt(footnoteWidgets.Count - unOrderCount);
          footnoteWidgets.Insert(index1, layoutedWidget1);
        }
      }
    }
  }

  private int GetFootNoteID(IWidget widget)
  {
    return Convert.ToInt32((((widget as WTextBody).Owner as IWidget).LayoutInfo as FootnoteLayoutInfo).FootnoteID);
  }

  private bool IsFootNote(IWidget widget)
  {
    return widget is WTextBody && (widget as WTextBody).Owner is WFootnote;
  }

  private void UpdateSplittedVerticalMergeCell(
    LayoutContext lc,
    LayoutedWidget mergeStartLW,
    RowLayoutInfo rowInfo,
    LayoutedWidget childLW,
    LayoutedWidget rowLW,
    int currentIndex,
    bool isNextRowNotFitted)
  {
    if (!(mergeStartLW.Widget.LayoutInfo as CellLayoutInfo).IsVerticalText)
    {
      int index1 = currentIndex;
      if (isNextRowNotFitted)
      {
        float cellStartPosition = (mergeStartLW.Widget is SplitWidgetContainer ? (mergeStartLW.Widget as SplitWidgetContainer).RealWidgetContainer as WTableCell : mergeStartLW.Widget as WTableCell).CellStartPosition;
        float num = 0.0f;
        for (int index2 = 0; index2 < this.m_table.Rows[this.CurrRowIndex].Cells.Count; ++index2)
        {
          if (Math.Round((double) num, 2) == Math.Round((double) cellStartPosition, 2))
          {
            index1 = index2;
            break;
          }
          num += this.m_table.Rows[this.CurrRowIndex].Cells[index2].Width;
        }
      }
      if ((lc.State != LayoutState.NotFitted || !this.IsHeaderRow(rowLW.Widget as WTableRow)) && (rowInfo.IsRowSplitted || isNextRowNotFitted || !rowInfo.IsExactlyRowHeight && lc.m_ltState == LayoutState.Splitted))
        (mergeStartLW.Widget.LayoutInfo as CellLayoutInfo).IsRowMergeEnd = this.m_splitedCells[index1] != null && this.m_splitedCells[index1].RealWidgetContainer != null && (this.m_splitedCells[index1].RealWidgetContainer.LayoutInfo as CellLayoutInfo).IsRowMergeEnd;
      this.m_splitedCells[index1] = lc.SplittedWidget is SplitWidgetContainer ? lc.SplittedWidget as SplitWidgetContainer : new SplitWidgetContainer(lc.Widget as IWidgetContainer);
    }
    else
    {
      int index3 = rowLW.ChildWidgets.IndexOf(childLW);
      if (isNextRowNotFitted)
      {
        float cellStartPosition = (mergeStartLW.Widget is SplitWidgetContainer ? (mergeStartLW.Widget as SplitWidgetContainer).RealWidgetContainer as WTableCell : mergeStartLW.Widget as WTableCell).CellStartPosition;
        float num = 0.0f;
        for (int index4 = 0; index4 < this.m_table.Rows[this.CurrRowIndex].Cells.Count; ++index4)
        {
          if (Math.Round((double) num, 2) == Math.Round((double) cellStartPosition, 2))
          {
            index3 = index4;
            break;
          }
          num += this.m_table.Rows[this.CurrRowIndex].Cells[index4].Width;
        }
      }
      if (rowInfo.IsRowSplitted || isNextRowNotFitted || !rowInfo.IsExactlyRowHeight && lc.m_ltState == LayoutState.Splitted)
        (mergeStartLW.Widget.LayoutInfo as CellLayoutInfo).IsRowMergeEnd = this.m_splitedCells[index3] != null && this.m_splitedCells[index3].RealWidgetContainer != null && (this.m_splitedCells[index3].RealWidgetContainer.LayoutInfo as CellLayoutInfo).IsRowMergeEnd;
      this.m_splitedCells[index3] = lc.SplittedWidget is SplitWidgetContainer ? lc.SplittedWidget as SplitWidgetContainer : new SplitWidgetContainer(lc.Widget as IWidgetContainer);
    }
  }

  private bool IsHeaderRow(WTableRow row)
  {
    bool isHeader = row.IsHeader;
    if (isHeader && this.IsWord2013(row.Document))
    {
      while (row.PreviousSibling != null)
      {
        row = row.PreviousSibling as WTableRow;
        if (!row.IsHeader)
          return false;
      }
    }
    return isHeader;
  }

  private LayoutedWidget GetVerticalMergeStartLW(LayoutedWidget verticalMergeEndLW)
  {
    WTableCell wtableCell1 = verticalMergeEndLW.Widget is WTableCell ? verticalMergeEndLW.Widget as WTableCell : (!(verticalMergeEndLW.Widget is SplitWidgetContainer) || !((verticalMergeEndLW.Widget as SplitWidgetContainer).RealWidgetContainer is WTableCell) ? (WTableCell) null : (verticalMergeEndLW.Widget as SplitWidgetContainer).RealWidgetContainer as WTableCell);
    for (int index = 0; index < this.m_verticallyMergeStartLW.Count; ++index)
    {
      LayoutedWidget verticalMergeStartLw = this.m_verticallyMergeStartLW[index];
      WTableCell wtableCell2 = verticalMergeStartLw.Widget is WTableCell ? verticalMergeStartLw.Widget as WTableCell : (!(verticalMergeStartLw.Widget is SplitWidgetContainer) || !((verticalMergeStartLw.Widget as SplitWidgetContainer).RealWidgetContainer is WTableCell) ? (WTableCell) null : (verticalMergeStartLw.Widget as SplitWidgetContainer).RealWidgetContainer as WTableCell);
      if (Math.Round((double) verticalMergeStartLw.Bounds.Left) == Math.Round((double) verticalMergeEndLW.Bounds.Left) && wtableCell1.OwnerRow.Index >= wtableCell2.OwnerRow.Index)
        return verticalMergeStartLw;
    }
    return (LayoutedWidget) null;
  }

  private void UpdateVerticalTextCellLW()
  {
    for (int index = 0; index < this.m_currRowLW.ChildWidgets.Count; ++index)
    {
      LayoutedWidget childWidget = this.m_currRowLW.ChildWidgets[index];
      CellLayoutInfo layoutInfo = childWidget.Widget.LayoutInfo as CellLayoutInfo;
      if (layoutInfo.IsVerticalText && !layoutInfo.IsRowMergeStart && !layoutInfo.IsRowMergeContinue)
      {
        float height = this.m_currRowLW.Bounds.Bottom - childWidget.Bounds.Top;
        LayoutArea layoutArea = new LayoutArea(new RectangleF(childWidget.Bounds.Left, childWidget.Bounds.Top, childWidget.Bounds.Width, height));
        LayoutContext childContext = LayoutContext.Create(childWidget.Widget, this.m_lcOperator, this.IsForceFitLayout);
        childContext.ClientLayoutAreaRight = (float) layoutArea.Width;
        LayoutedWidget layoutedWidget = this.LayoutCell(childContext, layoutArea.ClientArea, false);
        layoutedWidget.Owner = this.m_currRowLW.ChildWidgets[index].Owner;
        this.m_currRowLW.ChildWidgets[index] = layoutedWidget;
      }
    }
  }

  private void UpdateSplittedCells()
  {
    this.m_splitedCells = new SplitWidgetContainer[this.m_table.Rows[this.CurrRowIndex].Cells.Count];
    for (int index = 0; index < this.m_table.Rows[this.CurrRowIndex].Cells.Count; ++index)
      this.m_splitedCells[index] = this.m_table.Rows[this.CurrRowIndex].Cells[index].ChildEntities.Count > 0 ? new SplitWidgetContainer((IWidgetContainer) this.m_table.Rows[this.CurrRowIndex].Cells[index], this.m_table.Rows[this.CurrRowIndex].Cells[index].ChildEntities[0] as IWidget, 0) : new SplitWidgetContainer((IWidgetContainer) this.m_table.Rows[this.CurrRowIndex].Cells[index]);
  }

  private IWidget GetChildParaWidget(LayoutedWidget layoutedWidget)
  {
    if (layoutedWidget.ChildWidgets != null && layoutedWidget.ChildWidgets.Count > 0)
    {
      if (layoutedWidget.ChildWidgets[0].Widget is WTableCell || layoutedWidget.ChildWidgets[0].Widget is WTableRow || layoutedWidget.ChildWidgets[0].Widget is WTable)
        return this.GetChildParaWidget(layoutedWidget.ChildWidgets[0]);
      if (layoutedWidget.ChildWidgets[0].Widget is WParagraph)
        return layoutedWidget.ChildWidgets[0].Widget;
    }
    return (IWidget) null;
  }

  private LayoutedWidget GetChildParagraphWidgets(LayoutedWidget layoutedWidget)
  {
    if (layoutedWidget.ChildWidgets != null && layoutedWidget.ChildWidgets.Count > 0)
    {
      if (!this.IsTableWidget(layoutedWidget.ChildWidgets[0].Widget))
        return this.GetChildParagraphWidgets(layoutedWidget.ChildWidgets[0]);
      if (layoutedWidget.ChildWidgets[0].Widget is WParagraph || layoutedWidget.ChildWidgets[0].Widget is SplitWidgetContainer && (layoutedWidget.ChildWidgets[0].Widget as SplitWidgetContainer).RealWidgetContainer is WParagraph)
        return layoutedWidget;
    }
    return (LayoutedWidget) null;
  }

  private bool IsTableWidget(IWidget widget)
  {
    return !(widget is WTableCell) && !(widget is WTableRow) && !(widget is WTable) && (!(widget is SplitWidgetContainer) || !((widget as SplitWidgetContainer).RealWidgetContainer is WTableCell)) && (!(widget is SplitWidgetContainer) || !((widget as SplitWidgetContainer).RealWidgetContainer is WTableRow)) && (!(widget is SplitWidgetContainer) || !((widget as SplitWidgetContainer).RealWidgetContainer is WTable));
  }

  private bool IsNotEmptySplittedCell()
  {
    for (int index = 0; index < this.m_splitedCells.Length; ++index)
    {
      if (this.m_splitedCells[index] != null && this.m_splitedCells[index].m_currentChild != null)
        return true;
    }
    return false;
  }

  private bool IsRowNeedToBeSplitted(bool isFirstItemInPage)
  {
    WTableRow rowWidget = this.TableWidget.GetRowWidget(this.CurrRowIndex) as WTableRow;
    if ((rowWidget.RowFormat.IsBreakAcrossPages || this.m_table.TableFormat.WrapTextAround && rowWidget.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 || isFirstItemInPage) && (!this.m_currRowLW.Widget.LayoutInfo.IsKeepWithNext || isFirstItemInPage) && (this.IsHeaderRow(rowWidget) ? (isFirstItemInPage ? 1 : 0) : 1) != 0)
    {
      if (this.IsHeaderRow(rowWidget))
        (this.TableLayoutInfo as Syncfusion.Layouting.TableLayoutInfo).IsHeaderNotRepeatForAllPages = true;
      return true;
    }
    return (this.m_currRowLW.Widget.LayoutInfo.IsKeepWithNext || this.m_currRowIndex >= 1 && this.TableWidget.GetRowWidget(this.CurrRowIndex - 1).LayoutInfo.IsKeepWithNext) && (this.CommitKeepWithNext() || this.IsFirstItemInPage && this.IsAllRowHavingKeepWithNext()) && this.m_table.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013;
  }

  private bool IsAllRowHavingKeepWithNext()
  {
    bool flag = false;
    for (int index = 0; index < this.m_ltWidget.ChildWidgets.Count; ++index)
    {
      if (!this.m_ltWidget.ChildWidgets[index].Widget.LayoutInfo.IsKeepWithNext)
        return false;
      flag = true;
    }
    return flag;
  }

  private bool IsCellhMergeTillEnd(WTableRow currRow, int maxColIndex)
  {
    if (this.m_currColIndex <= -1 || this.m_currColIndex > maxColIndex || currRow.Cells[this.m_currColIndex].CellFormat.HorizontalMerge != CellMerge.Start)
      return false;
    for (int index = this.m_currColIndex + 1; index <= maxColIndex; ++index)
    {
      if (currRow.Cells[index].CellFormat.HorizontalMerge != CellMerge.Continue)
        return false;
    }
    return true;
  }

  private double GetRightPadValue(WTableRow currRow, int maxColIndex)
  {
    return this.m_currColIndex != maxColIndex && !this.IsCellhMergeTillEnd(currRow, maxColIndex) || (double) (this.m_currRowLW.Widget as WTableRow).RowFormat.CellSpacing <= 0.0 && (double) this.m_table.TableFormat.CellSpacing <= 0.0 ? 0.0 : (double) (this.m_currRowLW.Widget.LayoutInfo as RowLayoutInfo).Margins.Right;
  }

  private void UpdateRowLWBounds()
  {
    RectangleF bounds1 = this.m_currRowLW.Bounds;
    RectangleF bounds2 = this.m_currCellLW.Bounds;
    WTableRow row = this.m_table.Rows[this.m_currRowIndex];
    double rightPadValue = this.GetRightPadValue(row, row.Cells.Count - 1);
    double num1 = Math.Max((double) bounds2.Right + rightPadValue, (double) bounds1.Right);
    double num2 = (double) Math.Max(bounds2.Bottom, bounds1.Bottom);
    SizeF size = new SizeF((float) num1 - bounds1.Left, (float) num2 - bounds1.Top);
    this.m_currRowLW.Bounds = new RectangleF(bounds1.Location, size);
  }

  private void NextRowIndex()
  {
    if (this.m_bHeaderRepeat)
    {
      WTable tableWidget = this.TableWidget as WTable;
      WTableRow row = tableWidget.Rows[0];
      if (row.IsHeader && !tableWidget.TableFormat.WrapTextAround && (this.m_currRowIndex <= 0 || !tableWidget.Rows[this.m_currRowIndex].IsHeader || !this.IsNeedToLayoutHeaderRow(tableWidget, this.m_currRowIndex)) && !(this.LayoutInfo as Syncfusion.Layouting.TableLayoutInfo).IsHeaderNotRepeatForAllPages && !this.SkipLayoutingHeaderRow())
        this.LayoutHeaderRow(row);
      else
        this.m_bHeaderRepeat = false;
    }
    else
      ++this.m_currRowIndex;
  }

  private bool IsNeedToLayoutHeaderRow(WTable table, int currRowIndex)
  {
    if (this.IsWord2013(table.Document))
    {
      for (; currRowIndex > 0; --currRowIndex)
      {
        if (!table.Rows[currRowIndex].IsHeader)
          return false;
      }
    }
    return true;
  }

  private bool SkipLayoutingHeaderRow()
  {
    float pictureHeight = 0.0f;
    if (this.IsWord2013(this.m_table.Document) && this.IsCurrentPageFirstItemPicture(ref pictureHeight) && this.TableLayoutInfo is Syncfusion.Layouting.TableLayoutInfo)
    {
      Syncfusion.Layouting.TableLayoutInfo tableLayoutInfo = this.TableLayoutInfo as Syncfusion.Layouting.TableLayoutInfo;
      if ((double) tableLayoutInfo.HeaderRowHeight + (double) pictureHeight > (double) (this.m_lcOperator as Layouter).ClientLayoutArea.Height)
      {
        this.LayoutedHeaderRowHeight = tableLayoutInfo.HeaderRowHeight;
        tableLayoutInfo.HeaderRowHeight = 0.0f;
        return true;
      }
    }
    return false;
  }

  private bool IsCurrentPageFirstItemPicture(ref float pictureHeight)
  {
    if (this.m_spitTableWidget.SplittedCells != null)
    {
      for (int index = this.m_spitTableWidget.SplittedCells.Length - 1; index >= 0; --index)
      {
        if (this.m_spitTableWidget.SplittedCells[index] != null && this.m_spitTableWidget.SplittedCells[index].Count > 0)
        {
          SplitWidgetContainer splittedCell = this.m_spitTableWidget.SplittedCells[index];
          if (splittedCell.RealWidgetContainer is WTableCell && (splittedCell.RealWidgetContainer as WTableCell).OwnerRow.HeightType == TableRowHeightType.Exactly)
            return false;
          float cellPadding = (splittedCell.LayoutInfo as CellLayoutInfo).TopPadding + (splittedCell.LayoutInfo as CellLayoutInfo).BottomPadding;
          if (splittedCell.m_currentChild is WParagraph)
          {
            WPicture pictureInParagraph = this.FindPictureInParagraph(splittedCell.m_currentChild as WParagraph);
            if (pictureInParagraph != null)
            {
              pictureHeight = pictureInParagraph.Height + cellPadding;
              return true;
            }
          }
          return this.IsSplittedParagrapghHasPicture(splittedCell, ref pictureHeight, cellPadding);
        }
      }
    }
    else if (this.m_currRowIndex >= 0 && this.m_currRowIndex < this.TableWidget.RowsCount && this.m_table.Rows[this.m_currRowIndex].HeightType != TableRowHeightType.Exactly)
      return this.IsAnyOfCellFirstItemPicture(ref pictureHeight);
    return false;
  }

  private bool IsSplittedParagrapghHasPicture(
    SplitWidgetContainer widgetContainer,
    ref float pictureHeight,
    float cellPadding)
  {
    while (widgetContainer.m_currentChild is SplitWidgetContainer)
    {
      widgetContainer = widgetContainer.m_currentChild as SplitWidgetContainer;
      if (widgetContainer == null)
        return false;
    }
    if (widgetContainer == null || !(widgetContainer.m_currentChild is WPicture) && (!(widgetContainer.m_currentChild is WOleObject) || (widgetContainer.m_currentChild as WOleObject).OlePicture == null) || !(widgetContainer.m_currentChild is Entity) || (widgetContainer.m_currentChild as Entity).IsFloatingItem(false))
      return false;
    pictureHeight = !(widgetContainer.m_currentChild is WOleObject) ? (widgetContainer.m_currentChild as WPicture).Height + cellPadding : (widgetContainer.m_currentChild as WOleObject).OlePicture.Height + cellPadding;
    return true;
  }

  private bool IsAnyOfCellFirstItemPicture(ref float pictureHeight)
  {
    List<WPicture> picturesFromTableCell = this.FindPicturesFromTableCell();
    if (picturesFromTableCell.Count == 0)
      return false;
    foreach (WPicture wpicture in picturesFromTableCell)
    {
      if ((double) pictureHeight < (double) wpicture.Height)
        pictureHeight = wpicture.Height;
    }
    return true;
  }

  private List<WPicture> FindPicturesFromTableCell()
  {
    List<WPicture> picturesFromTableCell = new List<WPicture>();
    foreach (WTableCell cell in (Syncfusion.DocIO.DLS.CollectionImpl) this.m_table.Rows[this.m_currRowIndex].Cells)
    {
      if (cell.ChildEntities.Count > 0 && cell.ChildEntities[0] is WParagraph)
      {
        WPicture pictureInParagraph = this.FindPictureInParagraph(cell.ChildEntities[0] as WParagraph);
        if (pictureInParagraph != null)
          picturesFromTableCell.Add(pictureInParagraph);
      }
    }
    return picturesFromTableCell;
  }

  private WPicture FindPictureInParagraph(WParagraph paragraph)
  {
    foreach (Entity childEntity in (Syncfusion.DocIO.DLS.CollectionImpl) paragraph.ChildEntities)
    {
      switch (childEntity)
      {
        case WPicture _ when !childEntity.IsFloatingItem(false):
          return childEntity as WPicture;
        case WOleObject _ when (childEntity as WOleObject).OlePicture != null && !childEntity.IsFloatingItem(false):
          return (childEntity as WOleObject).OlePicture;
        default:
          continue;
      }
    }
    return (WPicture) null;
  }

  private void LayoutHeaderRow(WTableRow row)
  {
    if (!row.IsHeader)
      return;
    (this.m_lcOperator as Layouter).IsLayoutingHeaderRow = true;
    bool flag = false;
    if ((double) (this.LayoutInfo as Syncfusion.Layouting.TableLayoutInfo).HeaderRowHeight <= (double) (this.m_lcOperator as Layouter).ClientLayoutArea.Height)
    {
      for (; row.IsHeader || flag; row = row.NextSibling as WTableRow)
      {
        ++this.m_currHeaderRowIndex;
        if (this.m_currRowIndex != this.m_currHeaderRowIndex)
        {
          this.UpdateHeaderRowWidget();
          this.CommitRow();
          this.m_currColIndex = -1;
          flag = ((row.m_layoutInfo as RowLayoutInfo).IsRowHasVerticalMergeStartCell || (row.m_layoutInfo as RowLayoutInfo).IsRowHasVerticalMergeContinueCell && !(row.m_layoutInfo as RowLayoutInfo).IsRowHasVerticalMergeEndCell) && row.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013;
          if ((row.NextSibling == null || !(row.NextSibling is WTableRow) || !(row.NextSibling as WTableRow).IsHeader) && !flag)
          {
            this.m_bHeaderRepeat = false;
            break;
          }
        }
        else
        {
          this.m_bHeaderRepeat = false;
          break;
        }
      }
      if (this.m_ltWidget.ChildWidgets.Count > 0)
        this.m_headerRowHeight = (float) Math.Round((double) this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1].Bounds.Bottom - (double) this.m_ltWidget.ChildWidgets[0].Bounds.Top, 2);
    }
    (this.m_lcOperator as Layouter).IsLayoutingHeaderRow = false;
  }

  private void UpdateHeaderRowWidget()
  {
    this.m_currRowLW = new LayoutedWidget(this.TableWidget.GetRowWidget(this.CurrRowIndex));
    this.m_currRowLW.Bounds = new RectangleF(this.m_layoutArea.ClientActiveArea.Location, new SizeF());
    for (int index1 = 0; index1 < (this.m_currRowLW.Widget as WTableRow).Cells.Count; ++index1)
    {
      for (int index2 = 0; index2 < (this.m_currRowLW.Widget as WTableRow).Cells[index1].Items.Count; ++index2)
      {
        if ((this.m_currRowLW.Widget as WTableRow).Cells[index1].Items[index2] is WParagraph && (this.m_currRowLW.Widget as WTableRow).Cells[index1].Items[index2] is WParagraph wparagraph)
        {
          for (int index3 = 0; index3 < wparagraph.Items.Count; ++index3)
          {
            if (wparagraph.Items[index3] is WFootnote)
              ((IWidget) (wparagraph.Items[index3] as WFootnote)).LayoutInfo.IsSkip = true;
          }
        }
      }
    }
    this.LayoutRow((IWidget) (this.m_currRowLW.Widget as WTableRow));
  }

  private bool IsInFrame()
  {
    bool flag = true;
    if (this.m_table.IsInCell)
      flag = false;
    return this.m_table.IsFrame && flag;
  }

  private void UpdateCellHeight(int column)
  {
    LayoutedWidget childWidget = this.m_currRowLW.ChildWidgets[column];
    RectangleF bounds1 = childWidget.Bounds;
    RectangleF bounds2 = this.m_currRowLW.Bounds;
    float num = this.m_currRowIndex == this.m_table.Rows.Count - 1 || (double) (this.m_currRowLW.Widget as WTableRow).RowFormat.CellSpacing > 0.0 || (double) this.m_table.TableFormat.CellSpacing > 0.0 || this.m_blastRowState == LayoutState.Splitted ? (this.m_currRowLW.Widget.LayoutInfo as RowLayoutInfo).Paddings.Bottom + (this.m_currRowLW.Widget.LayoutInfo as RowLayoutInfo).Margins.Bottom : 0.0f;
    bounds1.Height = bounds2.Bottom - bounds1.Top - num;
    childWidget.Bounds = bounds1;
    if (!childWidget.Widget.LayoutInfo.IsVerticalText)
      this.UpdateVerticalCellAlignment(childWidget);
    else
      this.UpdateVerticalTextCellAlignment(childWidget);
    if (childWidget.Widget.LayoutInfo.IsVerticalText)
      return;
    RectangleF bounds3 = childWidget.ChildWidgets[0].Bounds;
    bounds3.Height = childWidget.Bounds.Bottom - bounds3.Top - (childWidget.Widget.LayoutInfo as CellLayoutInfo).Margins.Bottom;
    childWidget.ChildWidgets[0].Bounds = bounds3;
  }

  private void UpdateVerticalCellAlignment(LayoutedWidget cellLW)
  {
    RectangleF bounds = cellLW.Bounds;
    LayoutedWidget childWidget = cellLW.ChildWidgets[0];
    float displacementValue = this.GetDisplacementValue(childWidget, bounds.Bottom, false, 0.0f);
    if ((double) displacementValue <= 0.0)
      return;
    bool flag = false;
    float bottomOfFloattingItem = this.FindMaximumBottomOfFloattingItem(childWidget);
    if ((double) displacementValue > 0.0 && (double) bottomOfFloattingItem < (double) childWidget.Bounds.Bottom && (this.m_table.Document.DOP.Dop2000.Copts.DontVertAlignCellWithSp ? (this.IsCellHavingShapes(childWidget) ? 1 : 0) : 0) == 0)
    {
      flag = true;
      childWidget.ShiftLocation(0.0, (double) displacementValue, false, true, false);
    }
    else if ((this.m_table.Document.ActualFormatType != FormatType.Doc || !this.m_table.Document.DOP.Dop2000.Copts.DontVertAlignCellWithSp) && (double) displacementValue > 0.0 && cellLW.Widget is WTableCell && (double) (cellLW.Widget as WTableCell).OwnerRow.Height > 0.0 && (double) bounds.Height > (double) childWidget.Bounds.Height && (double) bottomOfFloattingItem > (double) childWidget.Bounds.Bottom && (double) bounds.Bottom > (double) bottomOfFloattingItem)
    {
      flag = true;
      float remainingSpace = cellLW.Bounds.Bottom - bottomOfFloattingItem;
      displacementValue = this.GetDisplacementValue(childWidget, bounds.Height, true, remainingSpace);
      childWidget.ShiftLocation(0.0, (double) displacementValue, false, true, false);
    }
    if (!flag || (this.m_lcOperator as Layouter).TrackChangesMarkups.Count <= 0 || this.m_table.Document.RevisionOptions.CommentDisplayMode != CommentDisplayMode.ShowInBalloons)
      return;
    childWidget.ShiftLocationOfCommentsMarkups(0.0f, displacementValue, (this.m_lcOperator as Layouter).TrackChangesMarkups);
  }

  private void UpdateVerticalTextCellAlignment(LayoutedWidget cellLW)
  {
    LayoutedWidget childWidget = cellLW.ChildWidgets[0];
    CellLayoutInfo layoutInfo = cellLW.Widget.LayoutInfo as CellLayoutInfo;
    if (layoutInfo.VerticalAlignment == VerticalAlignment.Top || childWidget.Widget is SplitWidgetContainer)
      return;
    float width = cellLW.Bounds.Width;
    float num = (cellLW.Widget as WTableCell).Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 || layoutInfo.VerticalAlignment != VerticalAlignment.Middle ? width - (layoutInfo.Margins.Left + layoutInfo.Margins.Right + layoutInfo.Paddings.Left + layoutInfo.Paddings.Right) : width - (layoutInfo.Paddings.Left + layoutInfo.Paddings.Right);
    float yOffset = 0.0f;
    switch (layoutInfo.VerticalAlignment)
    {
      case VerticalAlignment.Middle:
        yOffset = (float) (((double) num - (double) childWidget.Bounds.Height) / 2.0);
        break;
      case VerticalAlignment.Bottom:
        yOffset = num - childWidget.Bounds.Height;
        break;
    }
    if ((double) yOffset <= 0.0)
      return;
    childWidget.ShiftLocation(0.0, (double) yOffset, false, true);
  }

  private float GetDisplacementValue(
    LayoutedWidget child,
    float cellBottom,
    bool isRemainingSpace,
    float remainingSpace)
  {
    CellLayoutInfo layoutInfo = child.Widget.LayoutInfo as CellLayoutInfo;
    if ((child.Widget.LayoutInfo.IsVerticalText || !(child.Widget is SplitWidgetContainer) ? 1 : (!this.isRowMoved ? 0 : (layoutInfo.IsRowMergeStart || layoutInfo.IsRowMergeContinue ? 0 : (!layoutInfo.IsRowMergeEnd ? 1 : 0)))) != 0)
    {
      switch ((child.Widget.LayoutInfo as CellLayoutInfo).VerticalAlignment)
      {
        case VerticalAlignment.Middle:
          return !isRemainingSpace ? (float) (((double) cellBottom - (double) (child.Widget.LayoutInfo as CellLayoutInfo).Margins.Bottom - (double) child.Bounds.Bottom) / 2.0) : remainingSpace / 2f;
        case VerticalAlignment.Bottom:
          return !isRemainingSpace ? cellBottom - (child.Widget.LayoutInfo as CellLayoutInfo).Margins.Bottom - child.Bounds.Bottom : remainingSpace;
      }
    }
    return 0.0f;
  }

  private bool IsCellHavingShapes(LayoutedWidget child)
  {
    for (int index1 = 0; index1 < child.ChildWidgets.Count; ++index1)
    {
      for (int index2 = 0; index2 < child.ChildWidgets[index1].ChildWidgets.Count; ++index2)
      {
        if (child.ChildWidgets[index1].ChildWidgets[index2].Widget is WParagraph || (child.ChildWidgets[index1].ChildWidgets[index2].Widget is SplitWidgetContainer ? ((child.ChildWidgets[index1].ChildWidgets[index2].Widget as SplitWidgetContainer).RealWidgetContainer is WParagraph ? 1 : 0) : 0) != 0)
        {
          for (int index3 = 0; index3 < child.ChildWidgets[index1].ChildWidgets[index2].ChildWidgets.Count; ++index3)
          {
            WPicture widget1 = child.ChildWidgets[index1].ChildWidgets[index2].ChildWidgets[index3].Widget as WPicture;
            Shape widget2 = child.ChildWidgets[index1].ChildWidgets[index2].ChildWidgets[index3].Widget as Shape;
            GroupShape widget3 = child.ChildWidgets[index1].ChildWidgets[index2].ChildWidgets[index3].Widget as GroupShape;
            WTextBox widget4 = child.ChildWidgets[index1].ChildWidgets[index2].ChildWidgets[index3].Widget as WTextBox;
            if (child.ChildWidgets[index1].ChildWidgets[index2].ChildWidgets[index3].Widget is ShapeObject widget5 && widget5.AllowInCell || widget2 != null || widget3 != null || widget4 != null || widget1 != null && widget1.TextWrappingStyle != TextWrappingStyle.Inline)
              return true;
            if (widget1 != null)
            {
              int textWrappingStyle = (int) widget1.TextWrappingStyle;
            }
          }
        }
      }
    }
    return false;
  }

  private float FindMaximumBottomOfFloattingItem(LayoutedWidget child)
  {
    float bottomOfFloattingItem = float.MinValue;
    WTableCell wtableCell = child.Widget is WTableCell ? child.Widget as WTableCell : (child.Widget as SplitWidgetContainer).RealWidgetContainer as WTableCell;
    int num1 = 0;
    int num2 = 0;
    if (wtableCell != null)
    {
      num2 = wtableCell.OwnerRow.GetIndexInOwnerCollection();
      num1 = wtableCell.GetIndexInOwnerCollection();
    }
    for (int index1 = 0; index1 < child.ChildWidgets.Count; ++index1)
    {
      for (int index2 = 0; index2 < child.ChildWidgets[index1].ChildWidgets.Count; ++index2)
      {
        if (child.ChildWidgets[index1].ChildWidgets[index2].Widget is WParagraph || (child.ChildWidgets[index1].ChildWidgets[index2].Widget is SplitWidgetContainer ? ((child.ChildWidgets[index1].ChildWidgets[index2].Widget as SplitWidgetContainer).RealWidgetContainer is WParagraph ? 1 : 0) : 0) != 0)
        {
          for (int index3 = 0; index3 < child.ChildWidgets[index1].ChildWidgets[index2].ChildWidgets.Count; ++index3)
          {
            WPicture widget1 = child.ChildWidgets[index1].ChildWidgets[index2].ChildWidgets[index3].Widget as WPicture;
            Shape widget2 = child.ChildWidgets[index1].ChildWidgets[index2].ChildWidgets[index3].Widget as Shape;
            WTextBox widget3 = child.ChildWidgets[index1].ChildWidgets[index2].ChildWidgets[index3].Widget as WTextBox;
            WChart widget4 = child.ChildWidgets[index1].ChildWidgets[index2].ChildWidgets[index3].Widget as WChart;
            GroupShape widget5 = child.ChildWidgets[index1].ChildWidgets[index2].ChildWidgets[index3].Widget as GroupShape;
            SizeF sizeF = this.DrawingContext.MeasureString(" ", this.m_table.Rows[num2].Cells[num1].LastParagraph != null ? this.m_table.Rows[num2].Cells[num1].LastParagraph.BreakCharacterFormat.GetFontToRender(FontScriptType.English) : this.m_table.Rows[num2].Cells[num1].CharacterFormat.GetFontToRender(FontScriptType.English), (StringFormat) null);
            RectangleF cellBounds = new RectangleF(child.Bounds.X, child.Bounds.Y, this.GetCellWidth(num2, num1), this.GetCellHeight(num2, num1, sizeF.Height));
            if ((widget1 != null && widget1.LayoutInCell && this.IsLayoutedWidgetNeedToBeShifted(widget1.TextWrappingStyle, cellBounds, child.ChildWidgets[index1].ChildWidgets[index2].ChildWidgets[index3].Bounds, widget1.DistanceFromRight, widget1.DistanceFromLeft, widget1.DistanceFromTop, widget1.DistanceFromBottom, widget1.Document.Settings.CompatibilityMode, widget1.LayoutInCell) || widget2 != null && widget2.LayoutInCell && this.IsLayoutedWidgetNeedToBeShifted(widget2.WrapFormat.TextWrappingStyle, cellBounds, child.ChildWidgets[index1].ChildWidgets[index2].ChildWidgets[index3].Bounds, widget2.WrapFormat.DistanceRight, widget2.WrapFormat.DistanceLeft, widget2.WrapFormat.DistanceTop, widget2.WrapFormat.DistanceBottom, widget2.Document.Settings.CompatibilityMode, widget2.LayoutInCell) || widget5 != null && widget5.LayoutInCell && this.IsLayoutedWidgetNeedToBeShifted(widget5.WrapFormat.TextWrappingStyle, cellBounds, child.ChildWidgets[index1].ChildWidgets[index2].ChildWidgets[index3].Bounds, widget5.WrapFormat.DistanceRight, widget5.WrapFormat.DistanceLeft, widget5.WrapFormat.DistanceTop, widget5.WrapFormat.DistanceBottom, widget5.Document.Settings.CompatibilityMode, widget5.LayoutInCell) || widget4 != null && widget4.LayoutInCell && this.IsLayoutedWidgetNeedToBeShifted(widget4.WrapFormat.TextWrappingStyle, cellBounds, child.ChildWidgets[index1].ChildWidgets[index2].ChildWidgets[index3].Bounds, widget4.WrapFormat.DistanceRight, widget4.WrapFormat.DistanceLeft, widget4.WrapFormat.DistanceTop, widget4.WrapFormat.DistanceBottom, widget4.Document.Settings.CompatibilityMode, widget4.LayoutInCell) || widget3 != null && widget3.OwnerParagraph != null && widget3.OwnerParagraph.IsInCell && this.IsLayoutedWidgetNeedToBeShifted(widget3.TextBoxFormat.TextWrappingStyle, cellBounds, child.ChildWidgets[index1].ChildWidgets[index2].ChildWidgets[index3].Bounds, widget3.TextBoxFormat.WrapDistanceRight, widget3.TextBoxFormat.WrapDistanceLeft, widget3.TextBoxFormat.WrapDistanceTop, widget3.TextBoxFormat.WrapDistanceBottom, widget3.Document.Settings.CompatibilityMode, widget3.TextBoxFormat.AllowInCell)) && (double) bottomOfFloattingItem < (double) child.ChildWidgets[index1].ChildWidgets[index2].ChildWidgets[index3].Bounds.Bottom)
              bottomOfFloattingItem = child.ChildWidgets[index1].ChildWidgets[index2].ChildWidgets[index3].Bounds.Bottom;
          }
        }
      }
    }
    return bottomOfFloattingItem;
  }

  private bool IsLayoutedWidgetNeedToBeShifted(
    TextWrappingStyle textWrappingStyle,
    RectangleF cellBounds,
    RectangleF floatingItemBounds,
    float distanceFromRight,
    float distanceFromLeft,
    float distanceFromTop,
    float distanceFromBottom,
    CompatibilityMode mode,
    bool isLayoutInCell)
  {
    floatingItemBounds = new RectangleF(floatingItemBounds.X - distanceFromLeft, floatingItemBounds.Y + distanceFromTop, floatingItemBounds.Width + (distanceFromLeft + distanceFromRight), floatingItemBounds.Height + (distanceFromTop + distanceFromBottom));
    return textWrappingStyle != TextWrappingStyle.Inline && (mode != CompatibilityMode.Word2013 && isLayoutInCell || mode == CompatibilityMode.Word2013) && floatingItemBounds.IntersectsWith(cellBounds);
  }

  private void CreateTableClientArea(ref RectangleF rect)
  {
    ITableLayoutInfo tableLayoutInfo = this.TableLayoutInfo as ITableLayoutInfo;
    Paddings cellPadding = new Paddings();
    this.CorrectTableClientArea(ref rect);
    if ((double) tableLayoutInfo.Height > 0.0 && (double) tableLayoutInfo.Height < (double) rect.Height)
      rect.Height = tableLayoutInfo.Height;
    this.CreateLayoutArea(rect, cellPadding);
  }

  private LayoutedWidget LayoutCell(LayoutContext childContext, RectangleF cellArea, bool isSkip)
  {
    CellLayoutInfo layoutInfo1 = childContext.LayoutInfo as CellLayoutInfo;
    int count = (this.m_lcOperator as Layouter).FloatingItems.Count;
    RowLayoutInfo layoutInfo2 = this.m_currRowLW.Widget.LayoutInfo as RowLayoutInfo;
    LayoutedWidget layoutedWidget1 = new LayoutedWidget(childContext.Widget, cellArea.Location);
    if (!isSkip && !layoutInfo2.IsHiddenRow)
      layoutedWidget1 = childContext.Layout(cellArea);
    if (DocumentLayouter.IsEndPage)
    {
      DocumentLayouter.IsEndPage = false;
      if (layoutedWidget1.ChildWidgets.Count == 1 && layoutedWidget1.ChildWidgets[0].TextTag == "Splitted" && !this.IsForceFitLayout && layoutedWidget1.ChildWidgets[0].ChildWidgets.Count < 4)
        this.m_ltState = LayoutState.NotFitted;
    }
    if (DocumentLayouter.IsUpdatingTOC && DocumentLayouter.IsEndUpdateTOC)
      return (LayoutedWidget) null;
    for (int index = (this.m_lcOperator as Layouter).FloatingItems.Count - 1; index >= count; --index)
    {
      TextWrappingStyle textWrappingStyle = (this.m_lcOperator as Layouter).FloatingItems[index].TextWrappingStyle;
      RectangleF textWrappingBounds = (this.m_lcOperator as Layouter).FloatingItems[index].TextWrappingBounds;
      if ((double) textWrappingBounds.Width > (double) cellArea.Width && textWrappingStyle != TextWrappingStyle.InFrontOfText && textWrappingStyle != TextWrappingStyle.Behind)
      {
        textWrappingBounds.Width = cellArea.Width;
        (this.m_lcOperator as Layouter).FloatingItems[index].TextWrappingBounds = textWrappingBounds;
      }
      if (this.CompareOwnerOfTableCell(this.m_currRowLW.Widget as WTableRow, (this.m_lcOperator as Layouter).FloatingItems[index]) && (double) textWrappingBounds.Bottom > (double) layoutedWidget1.Bounds.Bottom && (double) layoutedWidget1.Bounds.Right > (double) textWrappingBounds.X && textWrappingStyle != TextWrappingStyle.InFrontOfText && textWrappingStyle != TextWrappingStyle.Behind && (this.m_table.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 || (this.m_lcOperator as Layouter).FloatingItems[index].LayoutInCell))
      {
        float num = textWrappingBounds.Bottom - layoutedWidget1.Bounds.Bottom;
        layoutedWidget1.Bounds = new RectangleF(layoutedWidget1.Bounds.X, layoutedWidget1.Bounds.Y, layoutedWidget1.Bounds.Width, layoutedWidget1.Bounds.Height + num);
      }
    }
    layoutedWidget1.Bounds = new RectangleF(layoutInfo1.CellContentLayoutingBounds.X, layoutInfo1.CellContentLayoutingBounds.Y, layoutInfo1.CellContentLayoutingBounds.Width, layoutedWidget1.Bounds.Height);
    LayoutedWidget layoutedWidget2 = new LayoutedWidget(childContext.Widget, cellArea.Location);
    layoutedWidget2.ChildWidgets.Add(layoutedWidget1);
    layoutedWidget1.Owner = layoutedWidget2;
    float bottom = layoutInfo1.Margins.Bottom;
    if (isSkip)
    {
      layoutedWidget2.Bounds = !layoutInfo1.IsRowMergeContinue || !layoutInfo2.IsExactlyRowHeight ? new RectangleF(cellArea.X, cellArea.Y, cellArea.Width, 0.0f) : new RectangleF(cellArea.X, cellArea.Y, cellArea.Width, cellArea.Height);
      if (layoutInfo1.IsRowMergeStart || layoutInfo1.IsRowMergeContinue)
      {
        if (!layoutInfo2.IsRowHasVerticalMergeContinueCell)
          layoutInfo2.IsRowHasVerticalMergeContinueCell = layoutInfo1.IsRowMergeContinue;
        if (!layoutInfo2.IsRowHasVerticalMergeEndCell)
          layoutInfo2.IsRowHasVerticalMergeEndCell = layoutInfo1.IsRowMergeEnd;
        if (!layoutInfo2.IsRowHasVerticalMergeStartCell)
          layoutInfo2.IsRowHasVerticalMergeStartCell = layoutInfo1.IsRowMergeStart;
        if (layoutInfo1.IsRowMergeStart)
          this.m_verticallyMergeStartLW.Add(layoutedWidget2);
      }
      if (!layoutInfo2.IsExactlyRowHeight)
        this.m_splitedCells[this.m_currColIndex] = new SplitWidgetContainer(childContext.Widget as IWidgetContainer);
      if (layoutInfo1.IsRowMergeStart && layoutInfo1.IsCellHasEndNote && childContext.Widget is WTextBody)
        this.AddVerticallyMergedCellFootNote(childContext.Widget as WTextBody);
      if ((layoutInfo1.IsRowMergeStart || layoutInfo1.IsVerticalText) && layoutInfo1.IsCellHasFootNote && !layoutInfo2.IsFootnoteReduced)
      {
        LayoutContext childContext1 = LayoutContext.Create(childContext.Widget, this.m_lcOperator, this.IsForceFitLayout);
        (this.m_lcOperator as Layouter).IsLayoutingVerticalMergeStartCell = true;
        LayoutedWidget layoutedWidget3 = childContext1.Layout(cellArea);
        (this.m_lcOperator as Layouter).IsLayoutingVerticalMergeStartCell = false;
        if (DocumentLayouter.IsUpdatingTOC && DocumentLayouter.IsEndUpdateTOC)
          return (LayoutedWidget) null;
        if (this.m_table.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013)
          layoutInfo2.IsFootnoteSplitted = this.CheckFootnoteInRowIsSplitted(childContext1);
        layoutedWidget3.GetFootnoteHeight(ref this.m_verticallyMergedCellFootnoteHeight);
      }
    }
    else
      layoutedWidget2.Bounds = new RectangleF(cellArea.X, cellArea.Y, cellArea.Width, layoutInfo2.IsExactlyRowHeight || layoutInfo1.IsVerticalText ? cellArea.Height : layoutedWidget1.Bounds.Bottom - cellArea.Top + bottom);
    return layoutedWidget2;
  }

  private bool CompareOwnerOfTableCell(WTableRow CurrentRow, FloatingItem Item)
  {
    Entity baseEntity = this.GetBaseEntity(Item.FloatingEntity);
    WTableRow ownerRow = baseEntity is WTableCell ? (baseEntity as WTableCell).GetOwnerRow(baseEntity as WTableCell) : (WTableRow) null;
    return baseEntity != null && baseEntity is WTableCell && ownerRow == CurrentRow;
  }

  private void AddVerticallyMergedCellFootNote(WTextBody textBody)
  {
    for (int index1 = 0; index1 < textBody.Items.Count; ++index1)
    {
      if (textBody.Items[index1] is WParagraph)
        this.AddVerticallyMergedCellFootNote(textBody.Items[index1] as WParagraph);
      else if (textBody.Items[index1] is WTable)
      {
        WTable wtable = textBody.Items[index1] as WTable;
        for (int index2 = 0; index2 < wtable.Rows.Count; ++index2)
        {
          WTableRow row = wtable.Rows[index2];
          for (int index3 = 0; index3 < row.Cells.Count; ++index3)
            this.AddVerticallyMergedCellFootNote((WTextBody) row.Cells[index3]);
        }
      }
    }
  }

  private void AddVerticallyMergedCellFootNote(WParagraph paragraph)
  {
    for (int index = 0; index < paragraph.Items.Count; ++index)
    {
      if (paragraph.Items[index] is WFootnote && (paragraph.Items[index] as WFootnote).FootnoteType == FootnoteType.Endnote && !(this.m_lcOperator as Layouter).EndnotesInstances.Contains((Entity) paragraph.Items[index]))
        (this.m_lcOperator as Layouter).EndnotesInstances.Add((Entity) paragraph.Items[index]);
    }
  }

  private LayoutContext CreateNextCellContext()
  {
    while (this.m_table != null && this.m_table.TableFormat.Bidi && this.m_currColIndex - 1 >= 0 || !this.m_table.TableFormat.Bidi && this.m_currColIndex + 1 < this.m_table.Rows[this.CurrRowIndex].Cells.Count)
    {
      if (this.m_table.TableFormat.Bidi)
        --this.m_currColIndex;
      else
        ++this.m_currColIndex;
      IWidgetContainer widgetContainer = (IWidgetContainer) null;
      if (this.m_spitTableWidget != null && this.m_spitTableWidget.SplittedCells != null && this.CurrRowIndex == this.m_spitTableWidget.StartRowNumber - 1 && this.m_currColIndex < this.m_spitTableWidget.SplittedCells.Length)
        widgetContainer = (IWidgetContainer) this.m_spitTableWidget.SplittedCells[this.m_currColIndex] ?? (IWidgetContainer) new SplitWidgetContainer(this.TableWidget.GetCellWidget(this.CurrRowIndex, this.m_currColIndex));
      if (widgetContainer == null)
        widgetContainer = this.TableWidget.GetCellWidget(this.CurrRowIndex, this.m_currColIndex);
      if (widgetContainer == null || widgetContainer.LayoutInfo == null || !widgetContainer.LayoutInfo.IsSkip)
        return LayoutContext.Create((IWidget) widgetContainer, this.m_lcOperator, this.IsForceFitLayout);
    }
    return (LayoutContext) null;
  }

  private void SaveChildContextState(LayoutContext childContext)
  {
    switch (childContext.State)
    {
      case LayoutState.Unknown:
        this.m_currRowLW.ChildWidgets.Add(this.m_currCellLW);
        this.m_currCellLW.Owner = this.m_currRowLW;
        this.m_bAtLastOneCellFitted = true;
        break;
      case LayoutState.NotFitted:
        this.MarkAsNotFitted(childContext);
        break;
      case LayoutState.Splitted:
        this.MarkAsSplitted(childContext);
        break;
      case LayoutState.Fitted:
        this.MarkAsFitted(childContext);
        break;
      case LayoutState.Breaked:
        this.MarkAsBreaked(childContext);
        break;
    }
  }

  private void MarkAsSplitted(LayoutContext childContext)
  {
    if (this.m_ltState == LayoutState.NotFitted)
      this.CommitKeepWithNext();
    RowLayoutInfo layoutInfo = this.m_currRowLW.Widget.LayoutInfo as RowLayoutInfo;
    if (layoutInfo.IsExactlyRowHeight || childContext.LayoutInfo.IsVerticalText)
    {
      this.MarkAsFitted(childContext);
      this.m_ltState = LayoutState.Unknown;
    }
    else
    {
      layoutInfo.IsRowSplitted = true;
      this.MarkAsFitted(childContext);
      if (this.m_blastRowState != LayoutState.Unknown || (this.m_currCellLW.Widget.LayoutInfo as CellLayoutInfo).IsRowMergeStart)
        return;
      this.m_blastRowState = LayoutState.Splitted;
      (this.TableLayoutInfo as ITableLayoutInfo).IsSplittedTable = true;
    }
  }

  protected virtual void MarkAsBreaked(LayoutContext childContext)
  {
    RowLayoutInfo layoutInfo = this.m_currRowLW.Widget.LayoutInfo as RowLayoutInfo;
    if (layoutInfo.IsExactlyRowHeight)
    {
      this.m_splitedCells[this.m_currColIndex] = childContext.SplittedWidget is SplitWidgetContainer ? childContext.SplittedWidget as SplitWidgetContainer : new SplitWidgetContainer(childContext.Widget as IWidgetContainer);
      layoutInfo.IsRowSplitted = true;
    }
    this.m_currRowLW.ChildWidgets.Add(this.m_currCellLW);
    this.m_currCellLW.Owner = this.m_currRowLW;
    this.m_bAtLastOneCellFitted = true;
    this.m_blastRowState = LayoutState.Breaked;
  }

  private void MarkAsNotFitted(LayoutContext childContext)
  {
    RowLayoutInfo layoutInfo = this.m_currRowLW.Widget.LayoutInfo as RowLayoutInfo;
    if (!layoutInfo.IsExactlyRowHeight)
      this.CommitKeepWithNext();
    if (childContext.IsVerticalNotFitted)
    {
      if (layoutInfo.IsExactlyRowHeight)
        this.MarkAsFitted(childContext);
      else
        this.m_ltState = LayoutState.NotFitted;
    }
    else if (this.CurrRowIndex < this.TableWidget.RowsCount - 1)
    {
      this.MarkAsSplitted(childContext);
      this.m_ltState = LayoutState.NotFitted;
    }
    else
      this.m_ltState = LayoutState.NotFitted;
  }

  private bool CommitKeepWithNext()
  {
    bool isAllItemsInPageHavingKeepWihtNext = false;
    bool commitKeepWithNext = this.IsNeedToCommitKeepWithNext(ref isAllItemsInPageHavingKeepWihtNext);
    bool flag = !this.IsFirstItemInPage && this.IsWord2013(this.m_table.Document) && this.IsHeaderRow(this.m_table.LastRow);
    while (this.m_ltWidget.ChildWidgets.Count > 0 && !(this.m_lcOperator as Layouter).IsLayoutingHeaderFooter && (commitKeepWithNext || isAllItemsInPageHavingKeepWihtNext && this.IsWord2013(this.m_table.Document) || flag) && (this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1].Widget.LayoutInfo.IsKeepWithNext && !this.IsForceFitLayout || flag))
    {
      LayoutedWidget childWidget = this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1];
      RowLayoutInfo layoutInfo1 = childWidget.Widget.LayoutInfo as RowLayoutInfo;
      if (layoutInfo1.IsRowHasVerticalMergeStartCell && !flag)
      {
        for (int index = 0; index < childWidget.ChildWidgets.Count; ++index)
        {
          CellLayoutInfo layoutInfo2 = childWidget.ChildWidgets[index].Widget.LayoutInfo as CellLayoutInfo;
          if (layoutInfo2.IsRowMergeStart && layoutInfo2.IsRowMergeEnd)
            layoutInfo2.IsRowMergeEnd = false;
        }
      }
      layoutInfo1.IsRowHasVerticalMergeStartCell = false;
      layoutInfo1.IsRowHasVerticalMergeEndCell = false;
      layoutInfo1.IsRowHasVerticalMergeContinueCell = false;
      this.RemoveBehindWidgets(this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1]);
      this.m_ltWidget.ChildWidgets.RemoveAt(this.m_ltWidget.ChildWidgets.Count - 1);
      if (this.m_splitedCells != null && this.m_splitedCells.Length > 0)
        Array.Clear((Array) this.m_splitedCells, 0, this.m_splitedCells.Length);
      --this.m_currRowIndex;
    }
    return isAllItemsInPageHavingKeepWihtNext;
  }

  private bool IsNeedToCommitKeepWithNext(ref bool isAllItemsInPageHavingKeepWihtNext)
  {
    bool commitKeepWithNext = false;
    bool flag1 = this.IsContainsKeepLines();
    for (int index = 0; index < this.m_ltWidget.ChildWidgets.Count; ++index)
    {
      IWidget widget = this.m_ltWidget.ChildWidgets[index].Widget;
      WTableRow wtableRow = widget as WTableRow;
      if (!widget.LayoutInfo.IsKeepWithNext && (this.IsWord2013(wtableRow.Document) ? (wtableRow.IsHeader ? 0 : (this.m_isTableSplitted ? 1 : (flag1 ? 1 : 0))) : (!this.m_isTableSplitted ? 1 : (!wtableRow.IsHeader ? 1 : 0))) != 0)
      {
        commitKeepWithNext = true;
        break;
      }
    }
    if (!commitKeepWithNext && this.m_ltWidget.ChildWidgets.Count > 0 && !this.IsFirstItemInPage && this.m_ltWidget.ChildWidgets[0].Widget as WTableRow == this.m_table.Rows[0] && !this.m_table.TableFormat.WrapTextAround && !this.m_table.IsInCell)
    {
      bool flag2 = false;
      for (Entity previousSibling = this.m_table.PreviousSibling as Entity; previousSibling != null && (!(previousSibling is WTable) || !(previousSibling as WTable).TableFormat.WrapTextAround); previousSibling = previousSibling.PreviousSibling as Entity)
      {
        if (!(previousSibling as TextBodyItem).m_layoutInfo.IsKeepWithNext)
        {
          flag2 = false;
          commitKeepWithNext = true;
          break;
        }
        flag2 = true;
        if ((previousSibling as TextBodyItem).m_layoutInfo.IsFirstItemInPage)
        {
          flag2 = false;
          isAllItemsInPageHavingKeepWihtNext = true;
          break;
        }
      }
      Entity baseEntity = this.GetBaseEntity(this.Widget as Entity);
      WSection wsection = baseEntity != null ? baseEntity as WSection : (WSection) null;
      if (wsection != null && wsection.BreakCode == SectionBreakCode.NoBreak && (wsection.Index > 0 && flag2 || this.m_table.Index == 0))
        commitKeepWithNext = true;
    }
    return commitKeepWithNext;
  }

  private bool IsContainsKeepLines()
  {
    for (int index = 0; index < this.m_ltWidget.ChildWidgets.Count; ++index)
    {
      WTableRow widget = this.m_ltWidget.ChildWidgets[index].Widget as WTableRow;
      if (widget.Cells.Count > 0 && widget.Cells[0].ChildEntities != null && widget.Cells[0].ChildEntities.FirstItem is WParagraph && (widget.Cells[0].ChildEntities.FirstItem as WParagraph).ParagraphFormat.Keep)
        return true;
    }
    return false;
  }

  private bool IsPreviousRowHasVerticalMergeContinueCell(WTableRow PreviousRow)
  {
    for (int index = 0; index < PreviousRow.ChildEntities.Count; ++index)
    {
      CellLayoutInfo layoutInfo = ((WidgetBase) PreviousRow.ChildEntities.InnerList[index]).m_layoutInfo as CellLayoutInfo;
      if (layoutInfo.IsRowMergeContinue && !layoutInfo.IsRowMergeEnd)
        return true;
    }
    return false;
  }

  private void MarkAsFitted(LayoutContext childContext)
  {
    if (!(this.m_currRowLW.Widget.LayoutInfo as RowLayoutInfo).IsExactlyRowHeight)
      this.m_splitedCells[this.m_currColIndex] = childContext.SplittedWidget is SplitWidgetContainer ? childContext.SplittedWidget as SplitWidgetContainer : new SplitWidgetContainer(childContext.Widget as IWidgetContainer);
    this.m_currRowLW.ChildWidgets.Add(this.m_currCellLW);
    this.m_currCellLW.Owner = this.m_currRowLW;
    this.UpdateRowLWBounds();
    this.m_bAtLastOneCellFitted = true;
  }

  private void UpdateClientArea()
  {
    float height = 0.0f;
    if (((this.m_currRowLW.Widget as WTableRow).m_layoutInfo as RowLayoutInfo).IsFootnoteReduced)
    {
      this.m_layoutArea.CutFromTop((double) this.m_currRowLW.Bounds.Bottom, height, this.IsInsideClippableItem());
      ((this.m_currRowLW.Widget as WTableRow).m_layoutInfo as RowLayoutInfo).IsFootnoteReduced = false;
    }
    else
    {
      this.GetFootnoteHeight(ref height);
      this.m_layoutArea.CutFromTop((double) this.m_currRowLW.Bounds.Bottom, height, this.IsInsideClippableItem());
    }
  }

  private bool IsInsideClippableItem()
  {
    if (this.m_table.IsFrame)
    {
      WParagraph paragraph = this.m_table.Rows[0].Cells[0].Paragraphs[0];
      if (paragraph != null && paragraph.ParagraphFormat.IsFrame && (double) paragraph.ParagraphFormat.FrameHeight != 0.0)
        return ((int) (ushort) ((double) paragraph.ParagraphFormat.FrameHeight * 20.0) & 32768 /*0x8000*/) == 0;
    }
    Entity owner = this.m_table.OwnerTextBody != null ? this.m_table.OwnerTextBody.Owner : (Entity) null;
    switch (owner)
    {
      case WTextBox _:
        WTextBox wtextBox = owner as WTextBox;
        if (wtextBox.TextBoxFormat.AutoFit)
          return false;
        return !wtextBox.IsShape || !wtextBox.Shape.TextFrame.ShapeAutoFit;
      case Shape _:
        return !(owner as Shape).TextFrame.ShapeAutoFit;
      case ChildShape _:
        return owner is ChildShape childShape && !childShape.TextFrame.ShapeAutoFit;
      default:
        return false;
    }
  }

  private void GetFootnoteHeight(ref float height)
  {
    LayoutedWidget childWidget = this.m_ltWidget.ChildWidgets[this.m_ltWidget.ChildWidgets.Count - 1];
    for (int index1 = 0; index1 < childWidget.ChildWidgets.Count; ++index1)
    {
      LayoutedWidget paragraphWidgets = this.GetChildParagraphWidgets(childWidget.ChildWidgets[index1]);
      if (paragraphWidgets != null)
      {
        for (int index2 = 0; index2 < paragraphWidgets.ChildWidgets.Count; ++index2)
        {
          for (int index3 = 0; index3 < paragraphWidgets.ChildWidgets[index2].ChildWidgets.Count; ++index3)
          {
            if (paragraphWidgets.ChildWidgets[index2].ChildWidgets[index3].Widget is WFootnote)
              height += (paragraphWidgets.ChildWidgets[index2].ChildWidgets[index3].Widget.LayoutInfo as FootnoteLayoutInfo).FootnoteHeight;
            else if (paragraphWidgets.ChildWidgets[index2].ChildWidgets[index3].Widget is IWidgetContainer)
              paragraphWidgets.ChildWidgets[index2].ChildWidgets[index3].GetFootnoteHeight(ref height);
          }
        }
      }
    }
  }

  private float GetRowWidth(WTableRow ownerRow)
  {
    float rowWidth = 0.0f;
    for (int colIndex = 0; colIndex < ownerRow.Cells.Count; ++colIndex)
      rowWidth += this.GetCellWidth(ownerRow.Index, colIndex);
    return rowWidth;
  }

  private LayoutArea GetCellClientArea(
    CellLayoutInfo cellInfo,
    int rowIndex,
    int columnIndex,
    float maxCellsTopPadding,
    float maxCellsTopMargin)
  {
    RectangleF clientActiveArea = this.m_rowLayoutArea.ClientActiveArea;
    float num1 = 0.0f;
    float width1 = this.m_table.Width;
    float num2 = (double) this.m_table.Rows[rowIndex].RowFormat.CellSpacing > 0.0 ? this.m_table.Rows[rowIndex].RowFormat.CellSpacing : ((double) this.m_table.TableFormat.CellSpacing > 0.0 ? this.m_table.TableFormat.CellSpacing : 0.0f);
    double width2 = (double) this.GetCellWidth(rowIndex, columnIndex);
    if (this.m_currRowLW.ChildWidgets.Count > 0)
      num1 = this.m_currRowLW.ChildWidgets[this.m_currRowLW.ChildWidgets.Count - 1].Bounds.Right + num2;
    double x = (double) num1 != 0.0 ? (double) num1 : (double) clientActiveArea.X;
    double y = (double) clientActiveArea.Y;
    double height = (double) clientActiveArea.Height <= (double) this.m_layoutArea.ClientArea.Height || (this.m_currRowLW.Widget as WTableRow).Document.Settings.CompatibilityMode != CompatibilityMode.Word2010 || (this.m_currRowLW.Widget as WTableRow).HeightType != TableRowHeightType.Exactly ? (double) clientActiveArea.Height : (double) this.m_currRowLW.Bounds.Height;
    if (cellInfo.IsColumnMergeStart)
      width2 = (double) this.GetCellMergedWidth(rowIndex, columnIndex);
    if ((double) num2 > 0.0)
    {
      y += (double) maxCellsTopPadding + (double) maxCellsTopMargin - ((double) cellInfo.Margins.Top + (double) cellInfo.Paddings.Top);
      height -= (double) maxCellsTopPadding + (double) maxCellsTopMargin - ((double) cellInfo.Margins.Top + (double) cellInfo.Paddings.Top);
      if (!this.m_table.TableFormat.Bidi && columnIndex == 0 || this.m_table.TableFormat.Bidi && columnIndex == this.m_table.Rows[rowIndex].Cells.Count - 1)
      {
        x += (double) num2 * 2.0;
        width2 -= (double) num2 * 3.0;
      }
      else if (!this.m_table.TableFormat.Bidi && columnIndex == this.m_table.Rows[rowIndex].Cells.Count - 1 || this.m_table.TableFormat.Bidi && columnIndex == 0)
      {
        x += (double) num2;
        width2 -= (double) num2 * 3.0;
      }
      else
      {
        x += (double) num2;
        width2 -= (double) num2 * 2.0;
      }
    }
    if (this.m_table != null && this.m_table.TableFormat.Bidi && columnIndex == this.m_table.Rows[rowIndex].Cells.Count - 1)
    {
      float rowWidth = this.GetRowWidth(this.m_table.Rows[rowIndex]);
      float maxRowLeftIndent = this.GetMaxRowLeftIndent();
      float num3 = width1 - maxRowLeftIndent;
      if (Math.Round((double) rowWidth, 2) < Math.Round((double) num3, 2))
      {
        float num4 = num3 - rowWidth;
        x += (double) num4;
      }
    }
    return new LayoutArea(new RectangleF((float) x, (float) y, (float) width2, (float) height));
  }

  private float GetMaxRowLeftIndent()
  {
    float maxRowLeftIndent = 0.0f;
    foreach (WTableRow row in (Syncfusion.DocIO.DLS.CollectionImpl) this.m_table.Rows)
    {
      if (row.RowFormat.HorizontalAlignment == RowAlignment.Left && (double) maxRowLeftIndent < (double) Math.Abs(row.RowFormat.LeftIndent))
        maxRowLeftIndent = Math.Abs(row.RowFormat.LeftIndent);
    }
    return maxRowLeftIndent;
  }

  private float GetCellWidth(int rowIndex, int colIndex)
  {
    float cellWidth = this.m_table.Rows[rowIndex].Cells[colIndex].Width;
    if ((double) cellWidth == 0.0)
    {
      CellLayoutInfo layoutInfo = ((IWidget) this.m_table.Rows[rowIndex].Cells[colIndex]).LayoutInfo as CellLayoutInfo;
      if (layoutInfo.IsColumnMergeContinue)
      {
        WTableCell cell = this.m_table.Rows[rowIndex].Cells[colIndex];
        cellWidth = cell.GetLeftPadding() + cell.GetRightPadding();
      }
      else
        cellWidth = layoutInfo.Paddings.Left + layoutInfo.Paddings.Right + layoutInfo.Margins.Left + layoutInfo.Margins.Right;
    }
    return cellWidth;
  }

  private float GetCellHeight(int rowIndex, int colIndex, float cellMinHeight)
  {
    float num = this.m_table.Rows[rowIndex].Height;
    if ((double) num <= 0.0)
      num = cellMinHeight;
    return num + ((((IWidget) this.m_table.Rows[rowIndex].Cells[colIndex]).LayoutInfo as CellLayoutInfo).Paddings.Top + (((IWidget) this.m_table.Rows[rowIndex].Cells[colIndex]).LayoutInfo as CellLayoutInfo).Paddings.Bottom + (((IWidget) this.m_table.Rows[rowIndex].Cells[colIndex]).LayoutInfo as CellLayoutInfo).Margins.Top + (((IWidget) this.m_table.Rows[rowIndex].Cells[colIndex]).LayoutInfo as CellLayoutInfo).Margins.Bottom);
  }

  private float GetCellMergedWidth(int rowIndex, int colIndex)
  {
    float cellWidth = this.GetCellWidth(rowIndex, colIndex);
    WTableCell wtableCell = (WTableCell) null;
    int num = colIndex + 1;
    if (num < this.m_table.Rows[rowIndex].Cells.Count)
      wtableCell = this.m_table.Rows[rowIndex].Cells[num];
    for (; wtableCell != null && wtableCell.CellFormat.HorizontalMerge == CellMerge.Continue; wtableCell = num >= this.m_table.Rows[rowIndex].Cells.Count ? (WTableCell) null : this.m_table.Rows[rowIndex].Cells[num])
    {
      cellWidth += this.GetCellWidth(rowIndex, num);
      ++num;
    }
    return cellWidth;
  }

  private void UpdateLWBounds()
  {
    RectangleF bounds = this.m_ltWidget.Bounds with
    {
      Width = this.m_currRowLW.Bounds.Width + ((this.TableWidget.LayoutInfo as Syncfusion.Layouting.TableLayoutInfo).Paddings.Left + (this.TableWidget.LayoutInfo as Syncfusion.Layouting.TableLayoutInfo).Paddings.Right)
    };
    bounds.Height = this.m_currRowLW.Bounds.Bottom - bounds.Top + (this.TableWidget.LayoutInfo as Syncfusion.Layouting.TableLayoutInfo).Paddings.Bottom;
    this.m_ltWidget.Bounds = bounds;
  }

  private void CorrectTableClientArea(ref RectangleF rect)
  {
    ITableLayoutInfo tableLayoutInfo = this.TableLayoutInfo as ITableLayoutInfo;
    bool flag = false;
    float num1 = 0.0f;
    int num2 = 0;
    int index1 = 0;
    for (int length = tableLayoutInfo.IsDefaultCells.Length; index1 < length; ++index1)
    {
      if (tableLayoutInfo.IsDefaultCells[index1])
      {
        int num3 = flag ? 1 : 0;
        flag = true;
        num1 += tableLayoutInfo.CellsWidth[index1];
        ++num2;
      }
    }
    if (!flag && (double) tableLayoutInfo.Width > (double) rect.Width)
      tableLayoutInfo.Width = rect.Width - (float) tableLayoutInfo.CellSpacings - (float) tableLayoutInfo.CellPaddings;
    else if (num2 == this.m_table.Rows[this.CurrRowIndex + 1].Cells.Count)
    {
      tableLayoutInfo.Width = 0.0f;
      for (int index2 = 0; index2 < num2; ++index2)
        tableLayoutInfo.Width += tableLayoutInfo.CellsWidth[index2];
    }
    RowFormat tableFormat = this.m_table.TableFormat;
    RowAlignment horizontalAlignment = this.GetHorizontalAlignment();
    MarginsF marginsF = this.InitializePageMargins();
    Entity baseEntity = this.GetBaseEntity((Entity) this.m_table);
    if ((double) this.m_table.IndentFromLeft != -3.4028234663852886E+38 && horizontalAlignment == RowAlignment.Left)
    {
      if (this.m_table.TableFormat.WrapTextAround)
      {
        if (this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Left)
        {
          if (this.m_table.IsInCell && (double) rect.Width > (double) tableLayoutInfo.Width || this.m_table.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013)
            rect.X += this.m_table.IndentFromLeft + (((IWidget) this.m_table.Rows[0].Cells[0]).LayoutInfo as CellLayoutInfo).Paddings.Left;
          else
            rect.X += this.m_table.IndentFromLeft - this.LeftPad;
        }
        else
          rect.X += this.m_table.IndentFromLeft;
      }
      else if (!tableFormat.Bidi)
      {
        if (this.m_table.IsInCell || this.m_table.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013)
          rect.X += this.m_table.IndentFromLeft;
        else
          rect.X += this.m_table.IndentFromLeft - this.LeftPad;
      }
      else if (tableFormat.Bidi)
      {
        float rightPad = this.GetRightPad(this.m_table.Rows[0].Cells[0]);
        rect.X += rect.Width - (tableLayoutInfo.Width - rightPad);
        rect.X -= this.m_table.IndentFromLeft;
        if (this.m_table.TableFormat.WrapTextAround)
          rect.X += this.GetMinimumRightPad() - rightPad;
      }
    }
    if (!(baseEntity is WSection) || (baseEntity as WSection).Columns.Count <= 1)
    {
      if (this.m_table.TableFormat.WrapTextAround && (this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Right || this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Outside || this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Inside && (this.m_lcOperator as Layouter).CurrPageIndex % 2 == 0) && this.m_table.TableFormat.Positioning.HorizRelationTo == HorizontalRelation.Page && !this.m_table.IsInCell && marginsF != null)
      {
        WSection ownerSection = baseEntity.GetOwnerSection((Entity) this.m_table) as WSection;
        rect.X = (double) tableLayoutInfo.Width + (double) tableFormat.Paddings.Right >= (double) Layouter.GetRightMargin(ownerSection) ? (this.m_table.TableFormat.Positioning.HorizPositionAbs != HorizontalPosition.Outside || (this.m_lcOperator as Layouter).CurrPageIndex % 2 != 0 ? (this.m_lcOperator as Layouter).ClientLayoutArea.Width + Layouter.GetLeftMargin(ownerSection) + Layouter.GetRightMargin(ownerSection) - tableLayoutInfo.Width - tableFormat.Paddings.Right : 0.0f) : (this.m_lcOperator as Layouter).ClientLayoutArea.Width + Layouter.GetLeftMargin(ownerSection);
      }
      else if (this.m_table.TableFormat.WrapTextAround && this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Left && (double) this.m_table.TableFormat.Positioning.HorizPosition == 0.0 && this.m_table.TableFormat.Positioning.HorizRelationTo == HorizontalRelation.Page && !this.m_table.IsInCell && marginsF != null)
      {
        float leftMargin = Layouter.GetLeftMargin(baseEntity as WSection);
        rect.X = (double) tableLayoutInfo.Width + (double) tableFormat.Paddings.Left + (double) this.m_table.IndentFromLeft >= (double) leftMargin ? this.m_table.IndentFromLeft : leftMargin - tableLayoutInfo.Width - tableFormat.Paddings.Left - this.m_table.IndentFromLeft;
      }
      else if (horizontalAlignment == RowAlignment.Right || this.m_table.TableFormat.WrapTextAround && this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Right || this.m_table.TableFormat.WrapTextAround && this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Outside && (this.m_lcOperator as Layouter).CurrPageIndex % 2 != 0)
      {
        if (!this.m_table.TableFormat.Bidi)
        {
          float num4 = this.IsWord2013(this.m_table.Document) ? 0.0f : this.GetRightPad(this.m_table.Rows[0].Cells[this.m_table.Rows[0].Cells.Count - 1]);
          rect.X += rect.Width - (tableLayoutInfo.Width - num4);
          if (!this.IsWord2013(this.m_table.Document) && this.m_table.TableFormat.WrapTextAround)
            rect.X += this.GetMinimumRightPad() - num4;
        }
        else if (this.m_table.TableFormat.Bidi)
          rect.X = rect.X;
        if (this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Left)
          rect.X -= this.m_table.TableFormat.Positioning.HorizPosition;
      }
      else if (horizontalAlignment == RowAlignment.Center || this.m_table.TableFormat.WrapTextAround && this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Center)
      {
        rect.X += (float) (((double) rect.Width - (double) this.m_table.Width) / 2.0);
        if (this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Left)
          rect.X -= this.m_table.TableFormat.Positioning.HorizPosition;
      }
    }
    else if (baseEntity is WSection && (baseEntity as WSection).Columns.Count > 1)
    {
      if (this.m_table.TableFormat.Positioning.HorizRelationTo == HorizontalRelation.Margin)
      {
        if (this.m_table.TableFormat.WrapTextAround && (this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Right || this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Outside && (this.m_lcOperator as Layouter).CurrPageIndex % 2 == 0) && !this.m_table.IsInCell && marginsF != null)
        {
          float num5 = 0.0f;
          if (!this.IsWord2013(this.m_table.Document) && this.m_table.Rows.Count > 0 && this.m_table.Rows[0].Cells.Count > 0)
            num5 = this.GetRightPad(this.m_table.Rows[0].Cells[this.m_table.Rows[0].Cells.Count - 1]);
          rect.X = (baseEntity as WSection).PageSetup.PageSize.Width - (Layouter.GetRightMargin(baseEntity as WSection) + (tableLayoutInfo.Width - num5));
        }
        else if (this.m_table.TableFormat.WrapTextAround && (this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Left || this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Inside) && (double) this.m_table.TableFormat.Positioning.HorizPosition == 0.0 && !this.m_table.IsInCell && marginsF != null)
          rect.X = Layouter.GetLeftMargin(baseEntity as WSection) + this.m_table.IndentFromLeft - this.LeftPad;
        else if (this.m_table.TableFormat.WrapTextAround && this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Left && (double) this.m_table.TableFormat.Positioning.HorizPosition != 0.0 && this.m_table.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 && !this.m_table.IsInCell && marginsF != null)
          rect.X = Layouter.GetLeftMargin(baseEntity as WSection) + this.m_table.TableFormat.Positioning.HorizPosition;
        else if (this.m_table.TableFormat.WrapTextAround && this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Center)
          rect.X = (float) ((double) (baseEntity as WSection).PageSetup.PageSize.Width / 2.0 - (double) this.m_table.Width / 2.0);
      }
      else if (this.m_table.TableFormat.Positioning.HorizRelationTo == HorizontalRelation.Column)
      {
        if (this.m_table.TableFormat.WrapTextAround && (this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Right || this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Outside) && !(this.m_table.OwnerTextBody is WTableCell) && marginsF != null)
          rect.X += (this.m_lcOperator as Layouter).ClientLayoutArea.Width - tableLayoutInfo.Width;
        else if (this.m_table.TableFormat.WrapTextAround && (this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Left || this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Inside) && (double) this.m_table.TableFormat.Positioning.HorizPosition == 0.0 && !this.m_table.IsInCell && marginsF != null)
          rect.X -= (this.m_lcOperator as Layouter).ClientLayoutArea.Width - tableLayoutInfo.Width;
        else if (this.m_table.TableFormat.WrapTextAround && this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Center)
          rect.X += (float) (((double) (this.m_lcOperator as Layouter).ClientLayoutArea.Width - (double) tableLayoutInfo.Width) / 2.0);
      }
      else if (this.m_table.TableFormat.Positioning.HorizRelationTo == HorizontalRelation.Page)
      {
        if (this.m_table.TableFormat.WrapTextAround && (this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Right || this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Outside) && !this.m_table.IsInCell && marginsF != null)
          rect.X = (baseEntity as WSection).PageSetup.PageSize.Width - tableLayoutInfo.Width;
        else if (this.m_table.TableFormat.WrapTextAround && (this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Left || this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Inside) && (double) this.m_table.TableFormat.Positioning.HorizPosition == 0.0 && !this.m_table.IsInCell && marginsF != null)
          rect.X = this.m_table.IndentFromLeft;
        else if (this.m_table.TableFormat.WrapTextAround && this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Left && (double) this.m_table.TableFormat.Positioning.HorizPosition > 0.0 && this.m_table.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 && !this.m_table.IsInCell && marginsF != null)
          rect.X = this.m_table.TableFormat.Positioning.HorizPosition;
        else if (this.m_table.TableFormat.WrapTextAround && this.m_table.TableFormat.Positioning.HorizPositionAbs == HorizontalPosition.Center)
          rect.X = (float) ((double) (baseEntity as WSection).PageSetup.PageSize.Width / 2.0 - (double) this.m_table.Width / 2.0);
      }
    }
    if (this.m_table.IsInCell && this.m_table.GetOwnerTableCell().m_layoutInfo.IsVerticalText)
      rect.Height = rect.Width;
    rect.Width = tableLayoutInfo.Width;
    float num6 = (tableLayoutInfo.Width - num1) / (float) (tableLayoutInfo.CellsWidth.Length - num2);
    int index3 = 0;
    for (int length = tableLayoutInfo.IsDefaultCells.Length; index3 < length; ++index3)
    {
      if (!tableLayoutInfo.IsDefaultCells[index3])
        tableLayoutInfo.CellsWidth[index3] = num6;
    }
  }

  private RowAlignment GetHorizontalAlignment()
  {
    RowAlignment horizontalAlignment = this.m_table.TableFormat.HorizontalAlignment;
    if (this.m_table.Rows[0].RowFormat.PropertiesHash.ContainsKey(105))
      horizontalAlignment = this.m_table.Rows[0].RowFormat.HorizontalAlignment;
    else if (this.m_table.TableFormat.PropertiesHash.ContainsKey(105))
      horizontalAlignment = this.m_table.TableFormat.HorizontalAlignment;
    return horizontalAlignment;
  }

  private float GetMinimumRightPad()
  {
    float minimumRightPad = this.GetRightPad(this.m_table.Rows[0].Cells[this.m_table.Rows[0].Cells.Count - 1]);
    for (int index = 1; index < this.m_table.Rows.Count; ++index)
    {
      int count = this.m_table.Rows[index].Cells.Count;
      float rightPad = this.GetRightPad(this.m_table.Rows[index].Cells[count - 1]);
      if ((double) minimumRightPad > (double) rightPad)
        minimumRightPad = rightPad;
    }
    return minimumRightPad;
  }

  private float GetRightPad(WTableCell tableCell)
  {
    float rightPad = tableCell.CellFormat.Paddings.Right;
    if (tableCell.CellFormat.SamePaddingsAsTable || (double) rightPad == -0.05000000074505806)
      rightPad = !tableCell.OwnerRow.RowFormat.Paddings.HasKey(4) ? (!tableCell.OwnerRow.OwnerTable.TableFormat.Paddings.HasKey(4) ? (tableCell.Document.ActualFormatType != FormatType.Doc ? 5.4f : 0.0f) : tableCell.OwnerRow.OwnerTable.TableFormat.Paddings.Right) : tableCell.OwnerRow.RowFormat.Paddings.Right;
    return rightPad;
  }
}
