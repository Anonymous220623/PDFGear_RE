// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.CellLayoutInfo
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class CellLayoutInfo : LayoutInfo, ILayoutSpacingsInfo
{
  private WTableCell m_cell;
  private VerticalAlignment m_verticalAlignment;
  private byte m_bFlags;
  private Spacings m_paddings;
  private Spacings m_margins;
  private RectangleF m_cellContentLayoutingBounds;
  private byte m_bFlags1;
  private float m_topPadding;
  private float m_updatedTopPadding;
  private float m_bottomPadding;
  private Dictionary<CellLayoutInfo.CellBorder, float> m_updatedTopBorders;
  private Dictionary<CellLayoutInfo.CellBorder, float> m_updatedSplittedTopBorders;
  private CellLayoutInfo.CellBorder m_topBorder;
  private CellLayoutInfo.CellBorder m_bottomBorder;
  private CellLayoutInfo.CellBorder m_leftBorder;
  private CellLayoutInfo.CellBorder m_rightBorder;
  private CellLayoutInfo.CellBorder m_prevCellTopBorder;
  private CellLayoutInfo.CellBorder m_nextCellTopBorder;
  private CellLayoutInfo.CellBorder m_prevCellBottomBorder;
  private CellLayoutInfo.CellBorder m_nextCellBottomBorder;

  internal bool IsColumnMergeStart
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal bool IsColumnMergeContinue
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  internal bool IsRowMergeStart
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  internal bool IsRowMergeContinue
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 247 | (value ? 1 : 0) << 3);
  }

  internal bool IsRowMergeEnd
  {
    get => ((int) this.m_bFlags & 16 /*0x10*/) >> 4 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 239 | (value ? 1 : 0) << 4);
  }

  internal VerticalAlignment VerticalAlignment
  {
    get => this.m_verticalAlignment;
    set => this.m_verticalAlignment = value;
  }

  internal RectangleF CellContentLayoutingBounds
  {
    get => this.m_cellContentLayoutingBounds;
    set => this.m_cellContentLayoutingBounds = value;
  }

  internal bool SkipTopBorder
  {
    get => ((int) this.m_bFlags1 & 1) != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 65534 | (value ? 1 : 0));
  }

  internal bool SkipBottomBorder
  {
    get => ((int) this.m_bFlags1 & 2) >> 1 != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 65533 | (value ? 1 : 0) << 1);
  }

  internal bool SkipLeftBorder
  {
    get => ((int) this.m_bFlags1 & 4) >> 2 != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 65531 | (value ? 1 : 0) << 2);
  }

  internal bool SkipRightBorder
  {
    get => ((int) this.m_bFlags1 & 8) >> 3 != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 65527 | (value ? 1 : 0) << 3);
  }

  internal float TopPadding
  {
    get => this.m_topPadding;
    set => this.m_topPadding = value;
  }

  internal float UpdatedTopPadding
  {
    get => this.m_updatedTopPadding;
    set => this.m_updatedTopPadding = value;
  }

  internal float BottomPadding
  {
    get => this.m_bottomPadding;
    set => this.m_bottomPadding = value;
  }

  internal CellLayoutInfo.CellBorder TopBorder
  {
    get => this.m_topBorder;
    set => this.m_topBorder = value;
  }

  internal CellLayoutInfo.CellBorder BottomBorder
  {
    get => this.m_bottomBorder;
    set => this.m_bottomBorder = value;
  }

  internal CellLayoutInfo.CellBorder LeftBorder
  {
    get => this.m_leftBorder;
    set => this.m_leftBorder = value;
  }

  internal CellLayoutInfo.CellBorder RightBorder
  {
    get => this.m_rightBorder;
    set => this.m_rightBorder = value;
  }

  internal Dictionary<CellLayoutInfo.CellBorder, float> UpdatedTopBorders
  {
    get
    {
      if (this.m_updatedTopBorders == null)
        this.m_updatedTopBorders = new Dictionary<CellLayoutInfo.CellBorder, float>();
      return this.m_updatedTopBorders;
    }
  }

  internal Dictionary<CellLayoutInfo.CellBorder, float> UpdatedSplittedTopBorders
  {
    get => this.m_updatedSplittedTopBorders;
    set => this.m_updatedSplittedTopBorders = value;
  }

  internal CellLayoutInfo.CellBorder PrevCellTopBorder => this.m_prevCellTopBorder;

  internal CellLayoutInfo.CellBorder NextCellTopBorder
  {
    get => this.m_nextCellTopBorder;
    set => this.m_nextCellTopBorder = value;
  }

  internal CellLayoutInfo.CellBorder PrevCellBottomBorder
  {
    get => this.m_prevCellBottomBorder;
    set => this.m_prevCellBottomBorder = value;
  }

  internal CellLayoutInfo.CellBorder NextCellBottomBorder
  {
    get => this.m_nextCellBottomBorder;
    set => this.m_nextCellBottomBorder = value;
  }

  internal bool IsCellHasEndNote
  {
    get => ((int) this.m_bFlags & 32 /*0x20*/) >> 5 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 223 | (value ? 1 : 0) << 5);
  }

  internal bool IsCellHasFootNote
  {
    get => ((int) this.m_bFlags & 64 /*0x40*/) >> 6 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 191 | (value ? 1 : 0) << 6);
  }

  public Spacings Paddings
  {
    get
    {
      if (this.m_paddings == null)
        this.m_paddings = new Spacings();
      return this.m_paddings;
    }
  }

  public Spacings Margins
  {
    get
    {
      if (this.m_margins == null)
        this.m_margins = new Spacings();
      return this.m_margins;
    }
  }

  public CellLayoutInfo(WTableCell cell)
    : base(ChildrenLayoutDirection.Vertical)
  {
    this.m_cell = cell;
    this.InitMerges();
    this.InitSpacings();
    CellFormat cellFormat = this.m_cell.CellFormat;
    if (cellFormat.TextDirection == TextDirection.VerticalBottomToTop || cellFormat.TextDirection == TextDirection.VerticalTopToBottom)
      this.IsVerticalText = true;
    this.VerticalAlignment = !cellFormat.HasKey(2) ? (!(this.m_cell.OwnerRow.OwnerTable.GetStyle() is WTableStyle style) || !style.CellProperties.HasKey(2) || style.CellProperties.VerticalAlignment == VerticalAlignment.Top ? cellFormat.VerticalAlignment : style.CellProperties.VerticalAlignment) : cellFormat.VerticalAlignment;
    this.TextWrap = cellFormat.TextWrap;
  }

  internal void InitMerges()
  {
    CellFormat cellFormat = this.m_cell.CellFormat;
    int num1 = this.m_cell.OwnerRow.Cells.IndexOf(this.m_cell);
    this.IsColumnMergeStart = cellFormat.HorizontalMerge == CellMerge.Start && num1 < this.m_cell.OwnerRow.Cells.Count - 1 && this.m_cell.OwnerRow.Cells[num1 + 1].CellFormat.HorizontalMerge == CellMerge.Continue;
    if (cellFormat.HorizontalMerge == CellMerge.Continue && num1 > 0 && ((((IWidget) this.m_cell.OwnerRow.Cells[num1 - 1]).LayoutInfo as CellLayoutInfo).IsColumnMergeStart || (((IWidget) this.m_cell.OwnerRow.Cells[num1 - 1]).LayoutInfo as CellLayoutInfo).IsColumnMergeContinue))
      this.IsColumnMergeContinue = true;
    int rowIndex = this.m_cell.OwnerRow.GetRowIndex();
    int num2 = this.m_cell.OwnerRow.OwnerTable.Rows.Count - 1;
    float cellStartPosition = this.m_cell.CellStartPosition;
    float cellWidth = this.m_cell.GetCellWidth();
    if (rowIndex < num2 && cellFormat.VerticalMerge == CellMerge.Start)
      this.IsRowMergeStart = this.IsCellVerticallyMerged(cellStartPosition, cellWidth, rowIndex + 1, false, true);
    if (rowIndex > 0 && cellFormat.VerticalMerge == CellMerge.Continue && cellFormat.VerticalMerge != CellMerge.Start)
      this.IsRowMergeContinue = this.IsCellVerticallyMerged(cellStartPosition, cellWidth, rowIndex - 1, false, false);
    if (!this.IsRowMergeContinue && !this.IsRowMergeStart && rowIndex < num2 && cellFormat.VerticalMerge == CellMerge.Continue)
      this.IsRowMergeStart = this.IsCellVerticallyMerged(cellStartPosition, cellWidth, rowIndex + 1, false, true);
    if (!this.IsRowMergeContinue)
      return;
    if (rowIndex < num2)
      this.IsRowMergeEnd = !this.IsCellVerticallyMerged(cellStartPosition, cellWidth, rowIndex + 1, true, false);
    else
      this.IsRowMergeEnd = true;
  }

  private bool IsCellVerticallyMerged(
    float cellStartPos,
    float cellWidth,
    int adjRowIndex,
    bool checkMergeEnd,
    bool checkMergeStart)
  {
    float num = 0.0f;
    bool flag = false;
    for (int index = 0; index < this.m_cell.OwnerRow.OwnerTable.Rows[adjRowIndex].Cells.Count; ++index)
    {
      if (Math.Round((double) cellStartPos, 2) == Math.Round((double) num, 2))
      {
        WTableCell cell = this.m_cell.OwnerRow.OwnerTable.Rows[adjRowIndex].Cells[index];
        if (checkMergeEnd)
        {
          if (cell.CellFormat.VerticalMerge == CellMerge.Start || cell.CellFormat.VerticalMerge == CellMerge.None)
          {
            flag = false;
            break;
          }
          float cellWidth1 = cell.GetCellWidth();
          flag = Math.Round((double) cellWidth, 2) == Math.Round((double) cellWidth1, 2);
          break;
        }
        if (checkMergeStart && cell.CellFormat.VerticalMerge == CellMerge.Continue)
        {
          float cellWidth2 = cell.GetCellWidth();
          flag = Math.Round((double) cellWidth, 2) == Math.Round((double) cellWidth2, 2);
          break;
        }
        if (!checkMergeStart && (cell.CellFormat.VerticalMerge == CellMerge.Start || cell.CellFormat.VerticalMerge == CellMerge.Continue))
        {
          float cellWidth3 = cell.GetCellWidth();
          flag = Math.Round((double) cellWidth, 2) == Math.Round((double) cellWidth3, 2);
          break;
        }
      }
      else if ((double) cellStartPos < (double) num)
      {
        flag = false;
        break;
      }
      num += this.m_cell.OwnerRow.OwnerTable.Rows[adjRowIndex].Cells[index].Width;
    }
    return flag;
  }

  internal void InitSpacings()
  {
    if (this.IsColumnMergeContinue)
    {
      this.SkipBottomBorder = true;
      this.SkipLeftBorder = true;
      this.SkipRightBorder = true;
      this.SkipTopBorder = true;
    }
    else
    {
      float num1 = this.m_cell.CellFormat.Paddings.Left;
      float num2 = this.m_cell.CellFormat.Paddings.Right;
      float num3 = this.m_cell.CellFormat.Paddings.Top;
      float num4 = this.m_cell.CellFormat.Paddings.Bottom;
      CellFormat cellFormatFromStyle = this.m_cell.OwnerRow.OwnerTable.GetCellFormatFromStyle(this.m_cell.OwnerRow.Index, this.m_cell.Index);
      bool flag = true;
      if (cellFormatFromStyle != null && (cellFormatFromStyle.Paddings.HasKey(1) || cellFormatFromStyle.Paddings.HasKey(4) || cellFormatFromStyle.Paddings.HasKey(2) || cellFormatFromStyle.Paddings.HasKey(3)))
        flag = false;
      if (this.m_cell.CellFormat.SamePaddingsAsTable && flag)
      {
        WTable ownerTable = this.m_cell.OwnerRow.OwnerTable;
        WTableStyle wtableStyle = (WTableStyle) null;
        if (ownerTable.StyleName != null && ownerTable.StyleName != string.Empty && ownerTable.Document.StyleNameIds.ContainsValue(ownerTable.StyleName))
          wtableStyle = ownerTable.Document.Styles.FindByName(ownerTable.StyleName) as WTableStyle;
        num1 = !this.m_cell.OwnerRow.RowFormat.Paddings.HasKey(1) ? (!ownerTable.TableFormat.Paddings.HasKey(1) ? (wtableStyle == null || !this.IsTableHavePadding(wtableStyle.TableProperties.Paddings) ? (this.m_cell.Document.ActualFormatType != FormatType.Doc ? 5.4f : 0.0f) : wtableStyle.TableProperties.Paddings.Left) : ownerTable.TableFormat.Paddings.Left) : this.m_cell.OwnerRow.RowFormat.Paddings.Left;
        num2 = !this.m_cell.OwnerRow.RowFormat.Paddings.HasKey(4) ? (!ownerTable.TableFormat.Paddings.HasKey(4) ? (this.m_cell.Document.ActualFormatType != FormatType.Doc ? (wtableStyle == null || !wtableStyle.TableProperties.Paddings.HasKey(4) ? 5.4f : wtableStyle.TableProperties.Paddings.Right) : 0.0f) : ownerTable.TableFormat.Paddings.Right) : this.m_cell.OwnerRow.RowFormat.Paddings.Right;
        num3 = !this.m_cell.OwnerRow.RowFormat.Paddings.HasKey(2) ? (!ownerTable.TableFormat.Paddings.HasKey(2) ? (wtableStyle == null || !wtableStyle.TableProperties.Paddings.HasKey(2) ? 0.0f : wtableStyle.TableProperties.Paddings.Top) : ownerTable.TableFormat.Paddings.Top) : this.m_cell.OwnerRow.RowFormat.Paddings.Top;
        num4 = !this.m_cell.OwnerRow.RowFormat.Paddings.HasKey(3) ? (!ownerTable.TableFormat.Paddings.HasKey(3) ? (wtableStyle == null || !wtableStyle.TableProperties.Paddings.HasKey(3) ? 0.0f : wtableStyle.TableProperties.Paddings.Bottom) : ownerTable.TableFormat.Paddings.Bottom) : this.m_cell.OwnerRow.RowFormat.Paddings.Bottom;
      }
      else if (cellFormatFromStyle == null)
      {
        num1 = this.m_cell.GetLeftPadding();
        num2 = this.m_cell.GetRightPadding();
        num3 = this.m_cell.GetTopPadding();
        num4 = this.m_cell.GetBottomPadding();
      }
      else
      {
        if (!cellFormatFromStyle.Paddings.HasKey(1) || (double) num1 == 0.0 || (double) num1 == -0.05000000074505806)
          num1 = this.m_cell.GetLeftPadding();
        if (!cellFormatFromStyle.Paddings.HasKey(4) || (double) num2 == 0.0 || (double) num2 == -0.05000000074505806)
          num2 = this.m_cell.GetRightPadding();
        if (!cellFormatFromStyle.Paddings.HasKey(2) || (double) num3 == 0.0 || (double) num3 == -0.05000000074505806)
          num3 = this.m_cell.GetTopPadding();
        if (!cellFormatFromStyle.Paddings.HasKey(3) || (double) num4 == 0.0 || (double) num3 == -0.05000000074505806)
          num4 = this.m_cell.GetBottomPadding();
      }
      int cellIndex = this.m_cell.GetCellIndex();
      int rowIndex = this.m_cell.OwnerRow.GetRowIndex();
      int cellLast = this.m_cell.OwnerRow.Cells.Count - 1;
      int rowLast = this.m_cell.OwnerRow.OwnerTable.Rows.Count - 1;
      double topHalfWidth = (double) this.GetTopHalfWidth(cellIndex, rowIndex, this.m_cell, rowIndex - 1);
      this.Paddings.Left = this.GetLeftHalfWidth(cellIndex);
      this.Paddings.Right = this.GetRightHalfWidth(cellIndex, cellLast);
      this.GetBottomHalfWidth(cellIndex, cellLast, rowIndex, rowLast);
      if ((double) this.m_cell.OwnerRow.RowFormat.CellSpacing > 0.0 || (double) this.m_cell.OwnerRow.OwnerTable.TableFormat.CellSpacing > 0.0)
      {
        this.Paddings.Top = this.TopPadding;
        this.Margins.Left = num1;
        this.Margins.Right = num2;
      }
      else
      {
        this.Margins.Left = (double) num1 != 0.0 ? ((double) num1 - (double) this.Paddings.Left < 0.0 ? 0.0f : num1 - this.Paddings.Left) : 0.0f;
        this.Margins.Right = (double) num2 != 0.0 ? ((double) num2 - ((double) this.Paddings.Right + ((double) this.Paddings.Left - (double) num1 > 0.0 ? (double) this.Paddings.Left - (double) num1 : 0.0)) > 0.0 ? num2 - (this.Paddings.Right + ((double) this.Paddings.Left - (double) num1 > 0.0 ? this.Paddings.Left - num1 : 0.0f)) : 0.0f) : 0.0f;
      }
      this.Margins.Top = num3;
      this.Margins.Bottom = num4;
    }
  }

  private bool IsTableHavePadding(Syncfusion.DocIO.DLS.Paddings padding)
  {
    if (padding.HasKey(1))
      return true;
    while (padding.BaseFormat is Syncfusion.DocIO.DLS.Paddings)
    {
      padding = padding.BaseFormat as Syncfusion.DocIO.DLS.Paddings;
      if (padding.HasKey(1))
        return true;
    }
    return false;
  }

  private bool IsSkipBorder(Border value1, Border value2, bool isFirstRead)
  {
    float lineWeight1 = value1.GetLineWeight();
    float lineWeight2 = value2.GetLineWeight();
    bool flag;
    if ((double) lineWeight1 == (double) lineWeight2)
    {
      int stylePriority1 = value1.GetStylePriority();
      int stylePriority2 = value2.GetStylePriority();
      if (stylePriority1 == stylePriority2)
      {
        int r1 = (int) value1.Color.R;
        int g1 = (int) value1.Color.G;
        int b1 = (int) value1.Color.B;
        int r2 = (int) value2.Color.R;
        int g2 = (int) value2.Color.G;
        int b2 = (int) value2.Color.B;
        flag = r1 + b1 + 2 * g1 != r2 + b2 + 2 * g2 ? r1 + b1 + 2 * g1 > r2 + b2 + 2 * g2 : (b1 + 2 * g1 != b2 + 2 * g2 ? b1 + 2 * g1 > b2 + 2 * g2 : (g1 != g2 ? g1 > g2 : isFirstRead));
      }
      else
        flag = stylePriority1 <= stylePriority2;
    }
    else
      flag = (double) lineWeight1 <= (double) lineWeight2;
    return flag;
  }

  private float GetLeftHalfWidth(int cellIndex)
  {
    Borders borders = this.m_cell.CellFormat.Borders;
    float leftHalfWidth = 0.0f;
    Border border1 = borders.Left;
    if (!border1.IsBorderDefined || border1.IsBorderDefined && border1.BorderType == BorderStyle.None && (double) border1.LineWidth == 0.0 && border1.Color.IsEmpty)
      border1 = cellIndex != 0 ? this.m_cell.OwnerRow.RowFormat.Borders.Vertical : this.m_cell.OwnerRow.RowFormat.Borders.Left;
    if (!border1.IsBorderDefined)
    {
      if (cellIndex != 0 || (double) this.m_cell.OwnerRow.RowFormat.CellSpacing > 0.0 || (double) this.m_cell.OwnerRow.OwnerTable.TableFormat.CellSpacing > 0.0)
        border1 = this.m_cell.OwnerRow.OwnerTable.TableFormat.Borders.Vertical;
      else if (cellIndex == 0)
        border1 = this.m_cell.OwnerRow.OwnerTable.TableFormat.Borders.Left;
    }
    if ((double) this.m_cell.OwnerRow.OwnerTable.TableFormat.CellSpacing > 0.0 || (double) this.m_cell.OwnerRow.RowFormat.CellSpacing > 0.0)
    {
      this.m_leftBorder = new CellLayoutInfo.CellBorder(border1.BorderType, border1.Color, border1.GetLineWidthValue(), border1.LineWidth);
      if (this.m_leftBorder.BorderType == BorderStyle.Cleared || this.m_leftBorder.BorderType == BorderStyle.None)
        this.SkipLeftBorder = true;
      leftHalfWidth = this.m_leftBorder.BorderType == BorderStyle.None || this.m_leftBorder.BorderType == BorderStyle.Cleared ? 0.0f : border1.GetLineWidthValue();
    }
    else if (cellIndex > 0)
    {
      WTableCell previousCell = this.m_cell.GetPreviousCell();
      CellLayoutInfo layoutInfo = ((IWidget) previousCell).LayoutInfo as CellLayoutInfo;
      Border border2 = previousCell.CellFormat.Borders.Right;
      if (!border2.IsBorderDefined || border2.IsBorderDefined && border2.BorderType == BorderStyle.None && (double) border2.LineWidth == 0.0 && border2.Color.IsEmpty)
        border2 = this.m_cell.OwnerRow.RowFormat.Borders.Vertical;
      if (!border2.IsBorderDefined)
        border2 = this.m_cell.OwnerRow.OwnerTable.TableFormat.Borders.Vertical;
      if ((border1.IsBorderDefined || border2.IsBorderDefined) && layoutInfo != null)
      {
        if ((border1.BorderType == BorderStyle.None || border1.BorderType == BorderStyle.Cleared) && (border2.BorderType == BorderStyle.Cleared || border2.BorderType == BorderStyle.None))
          this.SkipLeftBorder = true;
        else if ((border1.BorderType == BorderStyle.None || border1.BorderType == BorderStyle.Cleared) && previousCell.m_layoutInfo != null && !(previousCell.m_layoutInfo as CellLayoutInfo).SkipRightBorder)
        {
          this.m_leftBorder = new CellLayoutInfo.CellBorder(border2.BorderType, border2.Color, border2.GetLineWidthValue(), border2.LineWidth);
          leftHalfWidth = (float) ((this.m_leftBorder.BorderType == BorderStyle.None || this.m_leftBorder.BorderType == BorderStyle.Cleared ? 0.0 : (double) border2.GetLineWidthValue()) / 2.0);
          this.SkipLeftBorder = true;
        }
        else if ((border2.BorderType == BorderStyle.Cleared || border2.BorderType == BorderStyle.None) && previousCell.m_layoutInfo != null && (previousCell.m_layoutInfo as CellLayoutInfo).SkipRightBorder)
        {
          this.m_leftBorder = new CellLayoutInfo.CellBorder(border1.BorderType, border1.Color, border1.GetLineWidthValue(), border1.LineWidth);
          leftHalfWidth = (float) ((this.m_leftBorder.BorderType == BorderStyle.None || this.m_leftBorder.BorderType == BorderStyle.Cleared ? 0.0 : (double) border1.GetLineWidthValue()) / 2.0);
        }
        else if (this.IsSkipBorder(border1, border2, false) && !layoutInfo.SkipRightBorder)
        {
          this.m_leftBorder = new CellLayoutInfo.CellBorder(border2.BorderType, border2.Color, border2.GetLineWidthValue(), border2.LineWidth);
          leftHalfWidth = (float) ((this.m_leftBorder.BorderType == BorderStyle.None || this.m_leftBorder.BorderType == BorderStyle.Cleared ? 0.0 : (double) border2.GetLineWidthValue()) / 2.0);
          this.SkipLeftBorder = true;
        }
        else
        {
          this.m_leftBorder = new CellLayoutInfo.CellBorder(border1.BorderType, border1.Color, border1.GetLineWidthValue(), border1.LineWidth);
          leftHalfWidth = (float) ((this.m_leftBorder.BorderType == BorderStyle.None || this.m_leftBorder.BorderType == BorderStyle.Cleared ? 0.0 : (double) border1.GetLineWidthValue()) / 2.0);
        }
        this.m_prevCellTopBorder = layoutInfo.UpdatedTopBorders.Count > 0 ? new List<CellLayoutInfo.CellBorder>((IEnumerable<CellLayoutInfo.CellBorder>) layoutInfo.UpdatedTopBorders.Keys)[(((IWidget) previousCell).LayoutInfo as CellLayoutInfo).UpdatedTopBorders.Count - 1] : (((IWidget) previousCell).LayoutInfo as CellLayoutInfo).TopBorder;
      }
      if (layoutInfo != null)
        layoutInfo.NextCellTopBorder = this.UpdatedTopBorders.Count > 0 ? new List<CellLayoutInfo.CellBorder>((IEnumerable<CellLayoutInfo.CellBorder>) this.UpdatedTopBorders.Keys)[0] : this.TopBorder;
    }
    else
    {
      this.m_leftBorder = new CellLayoutInfo.CellBorder(border1.BorderType, border1.Color, border1.GetLineWidthValue(), border1.LineWidth);
      if (this.m_leftBorder.BorderType == BorderStyle.Cleared || this.m_leftBorder.BorderType == BorderStyle.None)
        this.SkipLeftBorder = true;
      leftHalfWidth = (float) ((this.m_leftBorder.BorderType == BorderStyle.None || this.m_leftBorder.BorderType == BorderStyle.Cleared ? 0.0 : (double) border1.GetLineWidthValue()) / 2.0);
    }
    return leftHalfWidth;
  }

  private float GetRightHalfWidth(int cellIndex, int cellLast)
  {
    Borders borders = this.m_cell.CellFormat.Borders;
    float rightHalfWidth = 0.0f;
    Border border1 = borders.Right;
    if (this.IsColumnMergeStart)
    {
      cellIndex = this.m_cell.GetHorizontalMergeEndCellIndex();
      border1 = this.m_cell.OwnerRow.Cells[cellIndex].CellFormat.Borders.Right;
    }
    if (!border1.IsBorderDefined || border1.IsBorderDefined && border1.BorderType == BorderStyle.None && (double) border1.LineWidth == 0.0 && border1.Color.IsEmpty)
      border1 = cellIndex != cellLast ? this.m_cell.OwnerRow.RowFormat.Borders.Vertical : this.m_cell.OwnerRow.RowFormat.Borders.Right;
    if (!border1.IsBorderDefined)
    {
      if (cellIndex != cellLast || (double) this.m_cell.OwnerRow.RowFormat.CellSpacing > 0.0 || (double) this.m_cell.OwnerRow.OwnerTable.TableFormat.CellSpacing > 0.0)
        border1 = this.m_cell.OwnerRow.OwnerTable.TableFormat.Borders.Vertical;
      else if (cellIndex == cellLast)
        border1 = this.m_cell.OwnerRow.OwnerTable.TableFormat.Borders.Right;
    }
    if ((double) this.m_cell.OwnerRow.OwnerTable.TableFormat.CellSpacing > 0.0 || (double) this.m_cell.OwnerRow.RowFormat.CellSpacing > 0.0)
    {
      this.m_rightBorder = new CellLayoutInfo.CellBorder(border1.BorderType, border1.Color, border1.GetLineWidthValue(), border1.LineWidth);
      if (this.m_rightBorder.BorderType == BorderStyle.Cleared || this.m_rightBorder.BorderType == BorderStyle.None)
        this.SkipRightBorder = true;
      rightHalfWidth = this.m_rightBorder.BorderType == BorderStyle.None || this.m_rightBorder.BorderType == BorderStyle.Cleared ? 0.0f : border1.GetLineWidthValue();
    }
    else if (cellIndex < cellLast)
    {
      Border border2 = this.m_cell.OwnerRow.Cells[cellIndex + 1].CellFormat.Borders.Left;
      if (!border2.IsBorderDefined || border2.IsBorderDefined && border2.BorderType == BorderStyle.None && (double) border2.LineWidth == 0.0 && border2.Color.IsEmpty)
        border2 = this.m_cell.OwnerRow.RowFormat.Borders.Vertical;
      if (!border2.IsBorderDefined)
        border2 = this.m_cell.OwnerRow.OwnerTable.TableFormat.Borders.Vertical;
      if (border1.IsBorderDefined || border2.IsBorderDefined)
      {
        if ((border1.BorderType == BorderStyle.None || border1.BorderType == BorderStyle.Cleared) && (border2.BorderType == BorderStyle.Cleared || border2.BorderType == BorderStyle.None))
          this.SkipRightBorder = true;
        else if (border1.BorderType == BorderStyle.None || border1.BorderType == BorderStyle.Cleared)
        {
          this.m_rightBorder = new CellLayoutInfo.CellBorder(border2.BorderType, border2.Color, border2.GetLineWidthValue(), border2.LineWidth);
          rightHalfWidth = (float) ((this.m_rightBorder.BorderType == BorderStyle.None || this.m_rightBorder.BorderType == BorderStyle.Cleared ? 0.0 : (double) border2.GetLineWidthValue()) / 2.0);
          this.SkipRightBorder = true;
        }
        else if (border2.BorderType == BorderStyle.Cleared || border2.BorderType == BorderStyle.None)
        {
          this.m_rightBorder = new CellLayoutInfo.CellBorder(border1.BorderType, border1.Color, border1.GetLineWidthValue(), border1.LineWidth);
          rightHalfWidth = (float) ((this.m_rightBorder.BorderType == BorderStyle.None || this.m_rightBorder.BorderType == BorderStyle.Cleared ? 0.0 : (double) border1.GetLineWidthValue()) / 2.0);
        }
        else if (this.IsSkipBorder(border1, border2, true))
        {
          this.m_rightBorder = new CellLayoutInfo.CellBorder(border2.BorderType, border2.Color, border2.GetLineWidthValue(), border2.LineWidth);
          rightHalfWidth = (float) ((this.m_rightBorder.BorderType == BorderStyle.None || this.m_rightBorder.BorderType == BorderStyle.Cleared ? 0.0 : (double) border2.GetLineWidthValue()) / 2.0);
          this.SkipRightBorder = true;
        }
        else
        {
          this.m_rightBorder = new CellLayoutInfo.CellBorder(border1.BorderType, border1.Color, border1.GetLineWidthValue(), border1.LineWidth);
          rightHalfWidth = (float) ((this.m_rightBorder.BorderType == BorderStyle.None || this.m_rightBorder.BorderType == BorderStyle.Cleared ? 0.0 : (double) border1.GetLineWidthValue()) / 2.0);
        }
      }
    }
    else
    {
      this.m_rightBorder = new CellLayoutInfo.CellBorder(border1.BorderType, border1.Color, border1.GetLineWidthValue(), border1.LineWidth);
      if (this.m_rightBorder.BorderType == BorderStyle.Cleared || this.m_rightBorder.BorderType == BorderStyle.None)
        this.SkipRightBorder = true;
      rightHalfWidth = (float) ((this.m_rightBorder.BorderType == BorderStyle.None || this.m_rightBorder.BorderType == BorderStyle.Cleared ? 0.0 : (double) border1.GetLineWidthValue()) / 2.0);
    }
    return rightHalfWidth;
  }

  private void GetBottomHalfWidth(int cellIndex, int cellLast, int rowIndex, int rowLast)
  {
    Border border = this.m_cell.CellFormat.Borders.Bottom;
    if (!border.IsBorderDefined || border.IsBorderDefined && border.BorderType == BorderStyle.None && (double) border.LineWidth == 0.0 && border.Color.IsEmpty)
      border = this.m_cell.OwnerRow.RowFormat.Borders.Bottom;
    if (!border.IsBorderDefined)
      border = this.m_cell.OwnerRow.OwnerTable.TableFormat.Borders.Bottom;
    if (!border.IsBorderDefined && ((double) this.m_cell.OwnerRow.RowFormat.CellSpacing > 0.0 || (double) this.m_cell.OwnerRow.OwnerTable.TableFormat.CellSpacing > 0.0))
      border = this.m_cell.OwnerRow.OwnerTable.TableFormat.Borders.Horizontal;
    if (border.IsBorderDefined)
    {
      this.m_bottomBorder = new CellLayoutInfo.CellBorder(border.BorderType, border.Color, border.GetLineWidthValue(), border.LineWidth);
      if (this.m_bottomBorder.BorderType == BorderStyle.Cleared || this.m_bottomBorder.BorderType == BorderStyle.None)
        this.SkipBottomBorder = true;
      this.m_bottomPadding = this.m_bottomBorder.BorderType == BorderStyle.None || this.m_bottomBorder.BorderType == BorderStyle.Cleared ? 0.0f : border.GetLineWidthValue();
    }
    if (cellIndex > 0)
      this.m_prevCellBottomBorder = (((IWidget) this.m_cell.OwnerRow.Cells[cellIndex - 1]).LayoutInfo as CellLayoutInfo).BottomBorder;
    if (cellIndex <= 0 || cellIndex > cellLast)
      return;
    (((IWidget) this.m_cell.OwnerRow.Cells[cellIndex - 1]).LayoutInfo as CellLayoutInfo).NextCellBottomBorder = this.m_bottomBorder;
  }

  internal float GetTopHalfWidth(
    int cellIndex,
    int rowIndex,
    WTableCell m_cell,
    int previousRowIndex)
  {
    Border border1 = m_cell.CellFormat.Borders.Top;
    if (!border1.IsBorderDefined || border1.IsBorderDefined && border1.BorderType == BorderStyle.None && (double) border1.LineWidth == 0.0 && border1.Color.IsEmpty)
      border1 = rowIndex != 0 ? m_cell.OwnerRow.RowFormat.Borders.Horizontal : m_cell.OwnerRow.RowFormat.Borders.Top;
    if (!border1.IsBorderDefined)
    {
      if (rowIndex != 0 || (double) m_cell.OwnerRow.OwnerTable.TableFormat.CellSpacing > 0.0 || (double) m_cell.OwnerRow.RowFormat.CellSpacing > 0.0)
        border1 = m_cell.OwnerRow.OwnerTable.TableFormat.Borders.Horizontal;
      else if (rowIndex == 0)
        border1 = m_cell.OwnerRow.OwnerTable.TableFormat.Borders.Top;
    }
    if ((double) m_cell.OwnerRow.OwnerTable.TableFormat.CellSpacing > 0.0 || (double) m_cell.OwnerRow.RowFormat.CellSpacing > 0.0)
    {
      this.m_topBorder = new CellLayoutInfo.CellBorder(border1.BorderType, border1.Color, border1.GetLineWidthValue(), border1.LineWidth);
      if (this.m_topBorder.BorderType == BorderStyle.Cleared || this.m_topBorder.BorderType == BorderStyle.None)
        this.SkipTopBorder = true;
      this.m_topPadding = this.m_topBorder.BorderType == BorderStyle.None || this.m_topBorder.BorderType == BorderStyle.Cleared ? 0.0f : border1.GetLineWidthValue();
    }
    else
    {
      CellLayoutInfo.CellBorder cellBorder = new CellLayoutInfo.CellBorder(border1.BorderType, border1.Color, border1.GetLineWidthValue(), border1.LineWidth);
      this.m_topPadding = cellBorder.BorderType == BorderStyle.None || cellBorder.BorderType == BorderStyle.Cleared ? 0.0f : border1.GetLineWidthValue();
      if (rowIndex > 0)
      {
        float cellStartPosition1 = m_cell.CellStartPosition;
        float cellEndPosition1 = m_cell.CellEndPosition;
        List<WTableCell> adjacentRowCell1 = this.GetAdjacentRowCell(cellStartPosition1, cellEndPosition1, previousRowIndex);
        for (int index1 = 0; index1 < adjacentRowCell1.Count; ++index1)
        {
          Border border2 = adjacentRowCell1[index1].CellFormat.Borders.Bottom;
          if (!border2.IsBorderDefined || border2.IsBorderDefined && border2.BorderType == BorderStyle.None && (double) border2.LineWidth == 0.0 && border2.Color.IsEmpty)
            border2 = adjacentRowCell1[index1].OwnerRow.RowFormat.Borders.Horizontal;
          if (!border2.IsBorderDefined)
            border2 = adjacentRowCell1[index1].OwnerRow.OwnerTable.TableFormat.Borders.Horizontal;
          if (border1.IsBorderDefined || border2.IsBorderDefined)
          {
            CellLayoutInfo.CellBorder key1;
            if (border1.BorderType == BorderStyle.None || border1.BorderType == BorderStyle.Cleared)
            {
              float renderingLineWidth = border2.GetLineWidthValue();
              if ((double) (m_cell.Owner as WTableRow).Height < (double) renderingLineWidth && m_cell.GridSpan > (short) 1 && (m_cell.Owner as WTableRow).HeightType == TableRowHeightType.Exactly)
                renderingLineWidth = (m_cell.Owner as WTableRow).Height;
              key1 = new CellLayoutInfo.CellBorder(border2.BorderType, border2.Color, renderingLineWidth, border2.LineWidth);
              float num = key1.BorderType == BorderStyle.None || key1.BorderType == BorderStyle.Cleared ? 0.0f : border2.GetLineWidthValue();
              if ((double) this.m_updatedTopPadding < (double) num)
                this.m_updatedTopPadding = num;
            }
            else if (border2.BorderType == BorderStyle.Cleared || border2.BorderType == BorderStyle.None)
            {
              key1 = new CellLayoutInfo.CellBorder(border1.BorderType, border1.Color, border1.GetLineWidthValue(), border1.LineWidth);
              float num = key1.BorderType == BorderStyle.None || key1.BorderType == BorderStyle.Cleared ? 0.0f : border1.GetLineWidthValue();
              if ((double) this.m_updatedTopPadding < (double) num)
                this.m_updatedTopPadding = num;
            }
            else if (this.IsSkipBorder(border1, border2, true))
            {
              key1 = new CellLayoutInfo.CellBorder(border2.BorderType, border2.Color, border2.GetLineWidthValue(), border2.LineWidth);
              float num = key1.BorderType == BorderStyle.None || key1.BorderType == BorderStyle.Cleared ? 0.0f : border2.GetLineWidthValue();
              if ((double) this.m_updatedTopPadding < (double) num)
                this.m_updatedTopPadding = num;
            }
            else
            {
              key1 = new CellLayoutInfo.CellBorder(border1.BorderType, border1.Color, border1.GetLineWidthValue(), border1.LineWidth);
              float num = key1.BorderType == BorderStyle.None || key1.BorderType == BorderStyle.Cleared ? 0.0f : border1.GetLineWidthValue();
              if ((double) this.m_updatedTopPadding < (double) num)
                this.m_updatedTopPadding = num;
            }
            if (key1 != null)
            {
              float cellStartPosition2 = adjacentRowCell1[index1].CellStartPosition;
              float cellEndPosition2 = adjacentRowCell1[index1].CellEndPosition;
              float num1 = Math.Round((double) cellEndPosition2, 2) != Math.Round((double) cellEndPosition1, 2) || Math.Round((double) cellStartPosition2, 2) != Math.Round((double) cellStartPosition1, 2) ? (Math.Round((double) cellStartPosition2, 2) < Math.Round((double) cellStartPosition1, 2) || Math.Round((double) cellEndPosition2, 2) < Math.Round((double) cellEndPosition1, 2) ? (Math.Round((double) cellStartPosition2, 2) < Math.Round((double) cellStartPosition1, 2) || Math.Round((double) cellEndPosition2, 2) > Math.Round((double) cellEndPosition1, 2) ? (Math.Round((double) cellStartPosition2, 2) > Math.Round((double) cellStartPosition1, 2) || Math.Round((double) cellEndPosition2, 2) > Math.Round((double) cellEndPosition1, 2) ? (Math.Round((double) cellStartPosition2, 2) > Math.Round((double) cellStartPosition1, 2) || Math.Round((double) cellEndPosition2, 2) < Math.Round((double) cellEndPosition1, 2) ? cellEndPosition1 - cellStartPosition1 : cellEndPosition1 - cellStartPosition1) : cellEndPosition2 - cellStartPosition1) : cellEndPosition2 - cellStartPosition2) : cellEndPosition1 - cellStartPosition2) : cellEndPosition1 - cellStartPosition1;
              if ((double) num1 < 0.0)
                num1 = 0.0f;
              if (Math.Round((double) cellStartPosition2, 2) == Math.Round((double) cellStartPosition1, 2) || Math.Round((double) cellStartPosition2, 2) > Math.Round((double) cellStartPosition1, 2))
                key1.AdjCellLeftBorder = (((IWidget) adjacentRowCell1[index1]).LayoutInfo as CellLayoutInfo).LeftBorder;
              if (Math.Round((double) cellEndPosition2, 2) == Math.Round((double) cellEndPosition1, 2) || Math.Round((double) cellEndPosition2, 2) < Math.Round((double) cellEndPosition1, 2))
                key1.AdjCellRightBorder = (((IWidget) adjacentRowCell1[index1]).LayoutInfo as CellLayoutInfo).RightBorder;
              if ((double) num1 > 0.0)
              {
                if (this.UpdatedSplittedTopBorders == null)
                  this.UpdatedTopBorders.Add(key1, num1);
                else
                  this.UpdatedSplittedTopBorders.Add(key1, num1);
              }
              float cellEndPosition3 = m_cell.OwnerRow.Cells[m_cell.OwnerRow.Cells.Count - 1].CellEndPosition;
              WTableCell cell = adjacentRowCell1[index1].OwnerRow.Cells[adjacentRowCell1[index1].OwnerRow.Cells.Count - 1];
              float cellEndPosition4 = cell.CellEndPosition;
              if (index1 == adjacentRowCell1.Count - 1 && (Math.Round((double) cellEndPosition1, 2) != Math.Round((double) cellEndPosition2, 2) || Math.Round((double) cellEndPosition1, 2) == Math.Round((double) cellEndPosition3, 2) && Math.Round((double) cellEndPosition3, 2) != Math.Round((double) cellEndPosition4, 2)))
              {
                if (cellIndex == m_cell.OwnerRow.Cells.Count - 1 && Math.Round((double) cellEndPosition1, 2) < Math.Round((double) cellEndPosition4, 2))
                {
                  if (adjacentRowCell1[index1] == cell && border2.BorderType != BorderStyle.Cleared && border2.BorderType != BorderStyle.None)
                  {
                    CellLayoutInfo.CellBorder key2 = new CellLayoutInfo.CellBorder(border2.BorderType, border2.Color, border2.GetLineWidthValue(), border2.LineWidth);
                    float num2 = cellEndPosition2 - cellEndPosition1;
                    if ((double) num2 < 0.0)
                      num2 = 0.0f;
                    if (this.UpdatedSplittedTopBorders == null)
                      this.UpdatedTopBorders.Add(key2, num2);
                    else
                      this.UpdatedSplittedTopBorders.Add(key2, num2);
                  }
                  else
                  {
                    List<WTableCell> adjacentRowCell2 = this.GetAdjacentRowCell(cellEndPosition1, cellEndPosition4, rowIndex - 1);
                    for (int index2 = 0; index2 < adjacentRowCell2.Count; ++index2)
                    {
                      Border border3 = adjacentRowCell2[index2].CellFormat.Borders.Bottom;
                      if (!border3.IsBorderDefined || border3.IsBorderDefined && border3.BorderType == BorderStyle.None && (double) border3.LineWidth == 0.0 && border3.Color.IsEmpty)
                        border3 = adjacentRowCell2[index2].OwnerRow.RowFormat.Borders.Horizontal;
                      if (!border3.IsBorderDefined)
                        border3 = adjacentRowCell2[index2].OwnerRow.OwnerTable.TableFormat.Borders.Horizontal;
                      if (border3.BorderType != BorderStyle.None)
                      {
                        float cellStartPosition3 = adjacentRowCell2[index2].CellStartPosition;
                        float cellEndPosition5 = adjacentRowCell2[index2].CellEndPosition;
                        CellLayoutInfo.CellBorder key3 = new CellLayoutInfo.CellBorder(border3.BorderType, border3.Color, border3.GetLineWidthValue(), border3.LineWidth);
                        float num3 = index2 != 0 ? cellEndPosition5 - cellStartPosition3 : cellEndPosition5 - cellEndPosition1;
                        if ((double) num3 < 0.0)
                          num3 = 0.0f;
                        if (this.UpdatedSplittedTopBorders == null)
                          this.UpdatedTopBorders.Add(key3, num3);
                        else
                          this.UpdatedSplittedTopBorders.Add(key3, num3);
                      }
                    }
                  }
                }
                else if (Math.Round((double) cellEndPosition1, 2) > Math.Round((double) cellEndPosition2, 2) && adjacentRowCell1[index1].GetCellIndex() == adjacentRowCell1[index1].OwnerRow.Cells.Count - 1 && border1.BorderType != BorderStyle.Cleared && border1.BorderType != BorderStyle.None)
                {
                  CellLayoutInfo.CellBorder key4 = new CellLayoutInfo.CellBorder(border1.BorderType, border1.Color, border1.GetLineWidthValue(), border1.LineWidth);
                  float num4 = cellEndPosition1 - cellEndPosition2;
                  if ((double) num4 < 0.0)
                    num4 = 0.0f;
                  if (this.UpdatedSplittedTopBorders == null)
                    this.UpdatedTopBorders.Add(key4, num4);
                  else
                    this.UpdatedSplittedTopBorders.Add(key4, num4);
                }
              }
            }
          }
        }
        if (adjacentRowCell1.Count == 0)
        {
          CellLayoutInfo.CellBorder key = new CellLayoutInfo.CellBorder(border1.BorderType, border1.Color, border1.GetLineWidthValue(), border1.LineWidth);
          float num = key.BorderType == BorderStyle.None || key.BorderType == BorderStyle.Cleared ? 0.0f : border1.GetLineWidthValue();
          if (key.BorderType == BorderStyle.Cleared || key.BorderType == BorderStyle.None)
            this.SkipTopBorder = true;
          if ((double) this.m_updatedTopPadding < (double) num)
            this.m_updatedTopPadding = num;
          if (this.UpdatedSplittedTopBorders == null)
            this.UpdatedTopBorders.Add(key, cellEndPosition1 - cellStartPosition1);
          else
            this.UpdatedSplittedTopBorders.Add(key, cellEndPosition1 - cellStartPosition1);
        }
        if (this.UpdatedTopBorders.Count > 0)
        {
          this.SkipTopBorder = true;
          foreach (CellLayoutInfo.CellBorder key in this.UpdatedTopBorders.Keys)
          {
            if (key.BorderType != BorderStyle.None && key.BorderType != BorderStyle.Cleared)
            {
              this.SkipTopBorder = false;
              break;
            }
          }
        }
        if (this.UpdatedSplittedTopBorders != null && this.UpdatedSplittedTopBorders.Count > 0)
        {
          this.SkipTopBorder = true;
          foreach (CellLayoutInfo.CellBorder key in this.UpdatedSplittedTopBorders.Keys)
          {
            if (key.BorderType != BorderStyle.None && key.BorderType != BorderStyle.Cleared)
            {
              this.SkipTopBorder = false;
              break;
            }
          }
        }
      }
      else
      {
        this.m_topBorder = new CellLayoutInfo.CellBorder(border1.BorderType, border1.Color, border1.GetLineWidthValue(), border1.LineWidth);
        this.m_topPadding = this.m_topBorder.BorderType == BorderStyle.None || this.m_topBorder.BorderType == BorderStyle.Cleared ? 0.0f : border1.GetLineWidthValue();
        if (this.m_topBorder.BorderType == BorderStyle.Cleared || this.m_topBorder.BorderType == BorderStyle.None)
          this.SkipTopBorder = true;
      }
    }
    return 0.0f;
  }

  internal List<WTableCell> GetAdjacentRowCell(float cellStartPos, float cellEndPos, int rowIndex)
  {
    List<WTableCell> adjacentRowCell = new List<WTableCell>();
    WTableRow row = this.m_cell.OwnerRow.OwnerTable.Rows[rowIndex];
    for (int index1 = 0; index1 < row.Cells.Count; ++index1)
    {
      float cellStartPosition = row.Cells[index1].CellStartPosition;
      float cellEndPosition = row.Cells[index1].CellEndPosition;
      if (Math.Round((double) cellEndPosition, 2) > Math.Round((double) cellStartPos, 2) && Math.Round((double) cellEndPosition, 2) <= Math.Round((double) cellEndPos, 2) || Math.Round((double) cellStartPosition, 2) >= Math.Round((double) cellStartPos, 2) && Math.Round((double) cellStartPosition, 2) < Math.Round((double) cellEndPos, 2) || Math.Round((double) cellStartPosition, 2) <= Math.Round((double) cellStartPos, 2) && Math.Round((double) cellEndPosition, 2) >= Math.Round((double) cellEndPos, 2))
      {
        WTableCell cell = row.Cells[index1];
        if (cell.m_layoutInfo != null && (cell.m_layoutInfo as CellLayoutInfo).IsColumnMergeContinue)
        {
          for (int index2 = index1; index2 >= 0; --index2)
          {
            if (row.Cells[index2].m_layoutInfo != null && (row.Cells[index2].m_layoutInfo as CellLayoutInfo).IsColumnMergeStart)
              cell = row.Cells[index2];
          }
        }
        if (!adjacentRowCell.Contains(cell))
          adjacentRowCell.Add(cell);
      }
      if (Math.Round((double) cellEndPosition, 2) >= Math.Round((double) cellEndPos, 2))
        break;
    }
    return adjacentRowCell;
  }

  internal void InitLayoutInfo()
  {
    this.m_paddings = (Spacings) null;
    this.m_margins = (Spacings) null;
    if (this.m_topBorder != null)
    {
      this.m_topBorder.InitLayoutInfo();
      this.m_topBorder = (CellLayoutInfo.CellBorder) null;
    }
    if (this.m_leftBorder != null)
    {
      this.m_leftBorder.InitLayoutInfo();
      this.m_leftBorder = (CellLayoutInfo.CellBorder) null;
    }
    if (this.m_rightBorder != null)
    {
      this.m_rightBorder.InitLayoutInfo();
      this.m_rightBorder = (CellLayoutInfo.CellBorder) null;
    }
    if (this.m_bottomBorder != null)
    {
      this.m_bottomBorder.InitLayoutInfo();
      this.m_bottomBorder = (CellLayoutInfo.CellBorder) null;
    }
    if (this.m_prevCellTopBorder != null)
    {
      this.m_prevCellTopBorder.InitLayoutInfo();
      this.m_prevCellTopBorder = (CellLayoutInfo.CellBorder) null;
    }
    if (this.m_prevCellBottomBorder != null)
    {
      this.m_prevCellBottomBorder.InitLayoutInfo();
      this.m_prevCellBottomBorder = (CellLayoutInfo.CellBorder) null;
    }
    if (this.m_nextCellTopBorder != null)
    {
      this.m_nextCellTopBorder.InitLayoutInfo();
      this.m_nextCellTopBorder = (CellLayoutInfo.CellBorder) null;
    }
    if (this.m_nextCellBottomBorder != null)
    {
      this.m_nextCellBottomBorder.InitLayoutInfo();
      this.m_nextCellBottomBorder = (CellLayoutInfo.CellBorder) null;
    }
    this.m_cell = (WTableCell) null;
    if (this.m_updatedTopBorders != null)
    {
      this.m_updatedTopBorders.Clear();
      this.m_updatedTopBorders = (Dictionary<CellLayoutInfo.CellBorder, float>) null;
    }
    if (this.m_updatedSplittedTopBorders == null)
      return;
    this.m_updatedSplittedTopBorders.Clear();
    this.m_updatedTopBorders = (Dictionary<CellLayoutInfo.CellBorder, float>) null;
  }

  internal class CellBorder
  {
    private BorderStyle m_borderType;
    private Color m_borderColor;
    private float m_renderingLineWidth;
    private float m_borderLineWidth;
    private CellLayoutInfo.CellBorder m_adjCellLeftBorder;
    private CellLayoutInfo.CellBorder m_adjCellRightBorder;

    internal BorderStyle BorderType => this.m_borderType;

    internal Color BorderColor => this.m_borderColor;

    internal float RenderingLineWidth => this.m_renderingLineWidth;

    internal float BorderLineWidth => this.m_borderLineWidth;

    internal CellLayoutInfo.CellBorder AdjCellLeftBorder
    {
      get => this.m_adjCellLeftBorder;
      set => this.m_adjCellLeftBorder = value;
    }

    internal CellLayoutInfo.CellBorder AdjCellRightBorder
    {
      get => this.m_adjCellRightBorder;
      set => this.m_adjCellRightBorder = value;
    }

    public CellBorder(
      BorderStyle borderStyle,
      Color borderColor,
      float renderingLineWidth,
      float borderLineWidth)
    {
      this.m_borderType = borderStyle;
      this.m_borderColor = borderColor;
      this.m_renderingLineWidth = renderingLineWidth;
      this.m_borderLineWidth = borderLineWidth;
    }

    internal void InitLayoutInfo()
    {
      this.m_adjCellLeftBorder = (CellLayoutInfo.CellBorder) null;
      this.m_adjCellRightBorder = (CellLayoutInfo.CellBorder) null;
    }
  }
}
