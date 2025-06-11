// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WTableColumnCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class WTableColumnCollection : CollectionImpl
{
  private IWordDocument m_doc;

  internal IWordDocument Document => this.m_doc;

  internal WTableColumn this[int index] => this.InnerList[index] as WTableColumn;

  internal WTableColumnCollection(WordDocument doc)
    : base(doc, (OwnerHolder) doc)
  {
    this.m_doc = (IWordDocument) doc;
  }

  internal void AddColumns(float offset)
  {
    this.InnerList.Add((object) new WTableColumn()
    {
      EndOffset = offset
    });
  }

  internal void UpdateColumns(
    int currentColumnIndex,
    int columnSpan,
    float preferredWidthEndOffset,
    ColumnSizeInfo sizeInfo,
    Dictionary<int, float> preferredWidths)
  {
    if (preferredWidths.ContainsKey(columnSpan - 1))
    {
      if ((double) preferredWidths[columnSpan - 1] < (double) preferredWidthEndOffset)
        preferredWidths[columnSpan - 1] = preferredWidthEndOffset;
    }
    else
      preferredWidths.Add(columnSpan - 1, preferredWidthEndOffset);
    if ((double) sizeInfo.MinimumWordWidth > (double) (this.InnerList[columnSpan - 1] as WTableColumn).MinimumWordWidth)
      (this.InnerList[columnSpan - 1] as WTableColumn).MinimumWordWidth = sizeInfo.MinimumWordWidth;
    if (columnSpan - currentColumnIndex == 1 && (double) sizeInfo.MaximumWordWidth > (double) (this.InnerList[columnSpan - 1] as WTableColumn).MaximumWordWidth)
    {
      (this.InnerList[columnSpan - 1] as WTableColumn).MaximumWordWidth = sizeInfo.MaximumWordWidth;
      (this.InnerList[columnSpan - 1] as WTableColumn).HasMaximumWordWidth = !(this.InnerList[columnSpan - 1] as WTableColumn).HasMaximumWordWidth ? sizeInfo.HasMaximumWordWidth : (this.InnerList[columnSpan - 1] as WTableColumn).HasMaximumWordWidth;
    }
    if ((double) sizeInfo.MinimumWidth > (double) (this.InnerList[columnSpan - 1] as WTableColumn).MinimumWidth)
      (this.InnerList[columnSpan - 1] as WTableColumn).MinimumWidth = sizeInfo.MinimumWidth;
    if ((double) sizeInfo.MaxParaWidth <= (double) (this.InnerList[columnSpan - 1] as WTableColumn).MaxParaWidth)
      return;
    (this.InnerList[columnSpan - 1] as WTableColumn).MaxParaWidth = sizeInfo.MaxParaWidth;
  }

  internal void UpdatePreferredWidhToColumns(Dictionary<int, float> preferredWidths)
  {
    for (int index = this.InnerList.Count - 1; index >= 0; --index)
    {
      WTableColumn inner = this.InnerList[index] as WTableColumn;
      if (preferredWidths.ContainsKey(index))
      {
        float num = preferredWidths[index] - this.GetPreviousColumnWidth(index, preferredWidths);
        if ((double) num > 0.0)
          inner.PreferredWidth = num;
      }
    }
    preferredWidths.Clear();
  }

  private float GetPreviousColumnWidth(int columnIndex, Dictionary<int, float> preferredWidths)
  {
    for (int key = columnIndex - 1; key >= 0; --key)
    {
      if (preferredWidths.ContainsKey(key))
        return preferredWidths[key];
    }
    return 0.0f;
  }

  internal int IndexOf(WTableColumn ownerColumn) => this.InnerList.IndexOf((object) ownerColumn);

  internal int IndexOf(float offSet)
  {
    int num = -1;
    foreach (WTableColumn inner in (IEnumerable) this.InnerList)
    {
      if ((double) inner.EndOffset == (double) offSet)
        return this.InnerList.IndexOf((object) inner);
    }
    return num;
  }

  internal bool Contains(float offSet)
  {
    foreach (WTableColumn inner in (IEnumerable) this.InnerList)
    {
      if ((double) inner.EndOffset == (double) offSet)
        return true;
    }
    return false;
  }

  internal void InsertColumn(int coulmnIndex, float offset)
  {
    this.InnerList.Insert(coulmnIndex, (object) new WTableColumn()
    {
      EndOffset = offset
    });
  }

  internal void RemoveColumn(int columnIndex) => this.InnerList.RemoveAt(columnIndex);

  internal void ResetColumns() => this.InnerList.Clear();

  internal WTableColumnCollection Clone()
  {
    WTableColumnCollection columnCollection = new WTableColumnCollection(this.m_doc as WordDocument);
    foreach (WTableColumn inner in (IEnumerable) this.InnerList)
      columnCollection.AddColumns(inner.EndOffset);
    return columnCollection;
  }

  internal void AutoFitColumns(
    float containerWidth,
    float preferredTableWidth,
    bool isAutoWidth,
    bool forceAutoFitToContent)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = 0.0f;
    for (int index = 0; index < this.InnerList.Count; ++index)
    {
      WTableColumn inner = this.InnerList[index] as WTableColumn;
      num1 += (double) inner.PreferredWidth > (double) inner.MaximumWordWidth ? inner.PreferredWidth : inner.MaximumWordWidth;
      num2 += (double) inner.PreferredWidth > (double) inner.MinimumWordWidth ? inner.PreferredWidth : inner.MinimumWordWidth;
      float num4 = (double) inner.PreferredWidth == 0.0 ? inner.MinimumWordWidth : ((double) inner.PreferredWidth > (double) inner.MinimumWordWidth ? inner.PreferredWidth : inner.MinimumWordWidth);
      float num5 = inner.MaximumWordWidth - num4;
      num3 += (double) num5 > 0.0 ? num5 : 0.0f;
    }
    if (Math.Round((double) num1, 2) <= Math.Round((double) preferredTableWidth, 2))
    {
      for (int index = 0; index < this.InnerList.Count; ++index)
      {
        WTableColumn inner = this.InnerList[index] as WTableColumn;
        if ((double) inner.PreferredWidth < (double) inner.MaximumWordWidth)
          inner.PreferredWidth = inner.MaximumWordWidth;
      }
      if (isAutoWidth)
        return;
      this.FitColumns(containerWidth, preferredTableWidth, isAutoWidth, (WTable) null);
    }
    else
    {
      if (!isAutoWidth)
      {
        float totalWidth = this.GetTotalWidth((byte) 1);
        containerWidth = (double) preferredTableWidth < (double) totalWidth ? ((double) totalWidth < (double) containerWidth ? totalWidth : containerWidth) : preferredTableWidth;
      }
      if (Math.Round((double) num2, 2) <= Math.Round((double) preferredTableWidth, 2) || Math.Round((double) num2, 2) <= Math.Round((double) containerWidth))
      {
        float num6 = ((double) containerWidth > (double) preferredTableWidth ? containerWidth : preferredTableWidth) - num2;
        for (int index = 0; index < this.InnerList.Count; ++index)
        {
          WTableColumn inner = this.InnerList[index] as WTableColumn;
          if ((double) inner.PreferredWidth < (double) inner.MinimumWordWidth)
            inner.PreferredWidth = inner.MinimumWordWidth;
          float num7 = inner.MaximumWordWidth - inner.PreferredWidth;
          float num8 = (double) num7 > 0.0 ? num7 : 0.0f;
          float num9 = num6 * (num8 / num3);
          inner.PreferredWidth += this.IsNaNOrInfinity(num9) ? 0.0f : num9;
        }
      }
      else
      {
        float totalWidth1 = this.GetTotalWidth((byte) 1);
        float totalWidth2 = this.GetTotalWidth((byte) 3);
        float num10 = (double) totalWidth2 < (double) containerWidth ? containerWidth - totalWidth2 : 0.0f;
        for (int index = 0; index < this.InnerList.Count; ++index)
        {
          WTableColumn inner = this.InnerList[index] as WTableColumn;
          float num11 = num10 * inner.MinimumWordWidth / totalWidth1;
          float num12 = this.IsNaNOrInfinity(num11) ? 0.0f : num11;
          inner.PreferredWidth = inner.MinimumWidth + num12;
        }
      }
    }
  }

  internal float GetTotalWidth(byte type)
  {
    float totalWidth = 0.0f;
    for (int index = 0; index < this.InnerList.Count; ++index)
    {
      WTableColumn inner = this.InnerList[index] as WTableColumn;
      double num1 = (double) totalWidth;
      double num2;
      switch (type)
      {
        case 0:
          num2 = (double) inner.PreferredWidth;
          break;
        case 1:
          num2 = (double) inner.MinimumWordWidth;
          break;
        case 2:
          num2 = (double) inner.MaximumWordWidth;
          break;
        case 3:
          num2 = (double) inner.MaxParaWidth;
          break;
        default:
          num2 = (double) inner.MinimumWidth;
          break;
      }
      totalWidth = (float) (num1 + num2);
    }
    return totalWidth;
  }

  internal bool IsNaNOrInfinity(float value) => float.IsNaN(value) || float.IsInfinity(value);

  internal void FitColumns(
    float containerWidth,
    float preferredTableWidth,
    bool isAutoWidth,
    WTable table)
  {
    float totalWidth1 = this.GetTotalWidth((byte) 0);
    float num1 = preferredTableWidth;
    float ownerWidth = table.GetOwnerWidth();
    float totalWidth2 = this.GetTotalWidth((byte) 3);
    WTableColumnCollection tableGrid1 = table.TableGrid;
    float totalMaximumWordWidth = 0.0f;
    float totalSpaceToExpand = 0.0f;
    if (isAutoWidth)
    {
      Entity ownerSection = table != null ? (table.IsInCell || table.Owner == null || table.Owner is WTextBox ? (Entity) null : table.GetOwnerSection((Entity) table)) : (Entity) null;
      num1 = ownerSection == null || table.TableFormat.WrapTextAround || !table.TableFormat.IsAutoResized || (double) totalWidth1 <= (double) containerWidth || (double) (ownerSection as WSection).PageSetup.Margins.Left + (double) table.IndentFromLeft + (double) totalWidth1 <= (double) (ownerSection as WSection).PageSetup.PageSize.Width ? totalWidth1 : containerWidth;
    }
    if ((double) totalWidth1 > (double) preferredTableWidth && !isAutoWidth)
      num1 = totalWidth1;
    if (Math.Round((double) totalWidth1, 2) != Math.Round((double) num1, 2))
    {
      if (this.IsResizeColumnsToPrefTableWidth(table, tableGrid1, ownerWidth, ref totalMaximumWordWidth, ref totalSpaceToExpand))
        this.ResizeColumnsToPrefTableWidth(tableGrid1, totalMaximumWordWidth, totalSpaceToExpand);
      else if (table.IsNeedToRecalculate && (double) tableGrid1.GetTotalWidth((byte) 2) > (double) ownerWidth)
      {
        this.ResizeColumnsToClientWidth(tableGrid1, tableGrid1.GetTotalWidth((byte) 2), ownerWidth);
        foreach (WTable recalculateTable in table.RecalculateTables)
        {
          WTableColumnCollection tableGrid2 = recalculateTable.TableGrid;
          if ((double) tableGrid2.GetTotalWidth((byte) 2) > (double) tableGrid1[recalculateTable.GetOwnerTableCell().Index].PreferredWidth)
          {
            this.ResizeColumnsToClientWidth(tableGrid2, tableGrid2.GetTotalWidth((byte) 2), tableGrid1[recalculateTable.GetOwnerTableCell().Index].PreferredWidth);
            recalculateTable.SetCellWidthAsColumnPreferredWidth(recalculateTable, tableGrid2);
            recalculateTable.UpdateRowBeforeAfter(recalculateTable);
            tableGrid2.UpdateEndOffset();
          }
        }
        if (table.m_recalculateTables != null)
        {
          table.m_recalculateTables.Clear();
          table.m_recalculateTables = (List<WTable>) null;
        }
      }
      else
      {
        float num2 = num1 / totalWidth1;
        float num3 = this.IsNaNOrInfinity(num2) ? 1f : num2;
        for (int index = 0; index < this.InnerList.Count; ++index)
        {
          WTableColumn inner = this.InnerList[index] as WTableColumn;
          inner.PreferredWidth = num3 * inner.PreferredWidth;
        }
      }
    }
    else if (table.IsNeedToRecalculate && (double) tableGrid1.GetTotalWidth((byte) 2) <= (double) ownerWidth)
    {
      foreach (WTableColumn wtableColumn in (CollectionImpl) tableGrid1)
        wtableColumn.PreferredWidth = wtableColumn.MaximumWordWidth;
    }
    else if ((this.m_doc as WordDocument).IsDOCX() && !table.IsInCell && table.TableFormat.IsAutoResized && table.TableFormat.PreferredWidth.WidthType == FtsWidth.Percentage && !table.HasAutoPreferredCellWidth && !table.HasPercentPreferredCellWidth && !table.HasPointPreferredCellWidth && table.HasNonePreferredCellWidth && table.HasOnlyParagraphs && table.MaximumCellCount() == this.Count && (double) num1 < (double) ownerWidth && (double) totalWidth1 > (double) totalWidth2)
    {
      float num4 = num1 - totalWidth2;
      if ((double) num4 > 0.0)
      {
        foreach (WTableColumn wtableColumn in (CollectionImpl) this)
        {
          float num5 = wtableColumn.MaxParaWidth / totalWidth2;
          wtableColumn.PreferredWidth = wtableColumn.MaxParaWidth + num5 * num4;
        }
      }
    }
    else if (this.IsResizeColumnsToPrefTableWidth(table, tableGrid1, ownerWidth, ref totalMaximumWordWidth, ref totalSpaceToExpand) || this.IsResizeHtmlTableColumnsBasedOnContent(table, tableGrid1, ownerWidth, ref totalMaximumWordWidth, ref totalSpaceToExpand))
      this.ResizeColumnsToPrefTableWidth(tableGrid1, totalMaximumWordWidth, totalSpaceToExpand);
    else if (this.IsResizeColumnsToClientWidth(table, tableGrid1, ownerWidth, ref totalMaximumWordWidth))
      this.ResizeColumnsToClientWidth(tableGrid1, totalMaximumWordWidth, ownerWidth);
    else if (this.IsExpandColumnsToClientWidth(table, tableGrid1, ownerWidth, ref totalMaximumWordWidth))
      this.ExpandColumnsToClientWidth(tableGrid1, totalMaximumWordWidth, ownerWidth);
    if (!table.IsInCell)
      return;
    this.ChecksNestedTableNeedtoRecalculate(table, tableGrid1);
  }

  private void ChecksNestedTableNeedtoRecalculate(
    WTable nestedTable,
    WTableColumnCollection columns)
  {
    WTableCell ownerTableCell = nestedTable.GetOwnerTableCell();
    WTable ownerTable = ownerTableCell != null ? (ownerTableCell.OwnerRow != null ? ownerTableCell.OwnerRow.OwnerTable : (WTable) null) : (WTable) null;
    WTableColumnCollection tableGrid = ownerTable.TableGrid;
    if ((this.m_doc as WordDocument).IsDOCX() && ownerTable != null && nestedTable.HasOnlyParagraphs && !ownerTable.IsInCell && ownerTable.TableFormat.IsAutoResized && nestedTable.TableFormat.IsAutoResized && (double) nestedTable.TableFormat.PreferredWidth.Width == 0.0 && nestedTable.TableFormat.PreferredWidth.WidthType == FtsWidth.Auto && (double) ownerTable.TableFormat.PreferredWidth.Width == 0.0 && ownerTable.TableFormat.PreferredWidth.WidthType == FtsWidth.Auto && nestedTable.HasNonePreferredCellWidth && nestedTable.HasPointPreferredCellWidth && !nestedTable.HasPercentPreferredCellWidth && !nestedTable.HasAutoPreferredCellWidth && ownerTable.HasNonePreferredCellWidth && ownerTable.HasPointPreferredCellWidth && !ownerTable.HasPercentPreferredCellWidth && !ownerTable.HasAutoPreferredCellWidth && nestedTable.MaximumCellCount() == columns.Count && ownerTable.MaximumCellCount() == tableGrid.Count)
    {
      foreach (WTableColumn column in (CollectionImpl) columns)
        column.PreferredWidth = column.MaximumWordWidth;
      ownerTable.IsNeedToRecalculate = true;
      ownerTable.RecalculateTables.Add(nestedTable);
    }
    else
    {
      if (!(this.m_doc as WordDocument).IsDOCX() || ownerTable == null || ownerTable.IsInCell || nestedTable.HasOnlyParagraphs || !ownerTable.TableFormat.IsAutoResized || !nestedTable.TableFormat.IsAutoResized || (double) nestedTable.TableFormat.PreferredWidth.Width <= 0.0 || nestedTable.TableFormat.PreferredWidth.WidthType != FtsWidth.Point || (double) ownerTable.TableFormat.PreferredWidth.Width <= 0.0 || (double) ownerTable.TableFormat.PreferredWidth.Width > (double) ownerTable.GetOwnerWidth() || ownerTable.TableFormat.PreferredWidth.WidthType != FtsWidth.Point || !nestedTable.HasNonePreferredCellWidth || nestedTable.HasPercentPreferredCellWidth || nestedTable.HasAutoPreferredCellWidth || !ownerTable.HasNonePreferredCellWidth || ownerTable.HasPointPreferredCellWidth || ownerTable.HasPercentPreferredCellWidth || ownerTable.HasAutoPreferredCellWidth || nestedTable.MaximumCellCount() != columns.Count || ownerTable.MaximumCellCount() != tableGrid.Count)
        return;
      foreach (WTableColumn column in (CollectionImpl) columns)
        column.PreferredWidth = column.MaximumWordWidth;
      ownerTable.IsNeedToRecalculate = true;
    }
  }

  private bool IsExpandColumnsToClientWidth(
    WTable table,
    WTableColumnCollection columns,
    float clientWidth,
    ref float totalMaximumWordWidth)
  {
    if (!(this.m_doc as WordDocument).IsDOCX() || table.IsInCell || !table.TableFormat.IsAutoResized || table.TableFormat.PreferredWidth.WidthType != FtsWidth.None || (double) table.TableFormat.PreferredWidth.Width != 0.0 || !table.HasOnlyParagraphs || table.MaximumCellCount() != columns.Count || !table.IsColumnNotHaveEnoughWidth(clientWidth) || !table.HasPointPreferredCellWidth || !table.HasNonePreferredCellWidth || table.HasPercentPreferredCellWidth || table.HasAutoPreferredCellWidth)
      return false;
    totalMaximumWordWidth = this.GetTotalWidth((byte) 2);
    return (double) totalMaximumWordWidth < (double) clientWidth;
  }

  private void ExpandColumnsToClientWidth(
    WTableColumnCollection columns,
    float totalMaximumWordWidth,
    float clientWidth)
  {
    float num1 = 0.0f;
    foreach (WTableColumn column in (CollectionImpl) columns)
    {
      if (Math.Round((double) column.PreferredWidth, 1) <= Math.Round((double) column.MaximumWordWidth, 1))
      {
        column.PreferredWidth = column.MaximumWordWidth;
        num1 += column.MaximumWordWidth;
      }
    }
    float num2 = clientWidth - num1;
    float num3 = totalMaximumWordWidth - num1;
    foreach (WTableColumn column in (CollectionImpl) columns)
    {
      if ((double) column.PreferredWidth != (double) column.MaximumWordWidth)
      {
        float num4 = column.MaximumWordWidth / num3;
        column.PreferredWidth = num2 * num4;
      }
    }
  }

  private bool IsResizeColumnsToClientWidth(
    WTable table,
    WTableColumnCollection columns,
    float clientWidth,
    ref float totalMaximumWordWidth)
  {
    if (!(this.m_doc as WordDocument).IsDOCX() || table.IsInCell || !table.TableFormat.IsAutoResized || table.TableFormat.PreferredWidth.WidthType != FtsWidth.None || (double) table.TableFormat.PreferredWidth.Width != 0.0 || table.MaximumCellCount() != this.Count || !table.HasOnlyParagraphs || table.HasPercentPreferredCellWidth || table.HasAutoPreferredCellWidth || table.HasNonePreferredCellWidth || !table.HasPointPreferredCellWidth)
      return false;
    totalMaximumWordWidth = columns.GetTotalWidth((byte) 2);
    return (double) totalMaximumWordWidth > (double) clientWidth;
  }

  private void ResizeColumnsToClientWidth(
    WTableColumnCollection columns,
    float totalMaximumWordWidth,
    float clientWidth)
  {
    foreach (WTableColumn column in (CollectionImpl) columns)
    {
      float num = column.MaximumWordWidth / totalMaximumWordWidth;
      column.PreferredWidth = clientWidth * num;
    }
  }

  private bool IsResizeHtmlTableColumnsBasedOnContent(
    WTable table,
    WTableColumnCollection columns,
    float clientWidth,
    ref float totalMaximumWordWidth,
    ref float totalSpaceToExpand)
  {
    if ((this.m_doc as WordDocument).IsDOCX() && !table.HasAutoPreferredCellWidth && !table.HasNonePreferredCellWidth && !table.HasPercentPreferredCellWidth && !table.HasPointPreferredCellWidth && !table.IsInCell && table.TableFormat.IsAutoResized && table.PreferredTableWidth.WidthType == FtsWidth.Point && (double) table.PreferredTableWidth.Width < (double) clientWidth && table.IsColumnNotHaveEnoughWidth(clientWidth))
    {
      totalMaximumWordWidth = this.GetTotalWidth((byte) 2);
      if ((double) table.PreferredTableWidth.Width > (double) totalMaximumWordWidth)
      {
        totalSpaceToExpand = table.PreferredTableWidth.Width - totalMaximumWordWidth;
        return true;
      }
    }
    return false;
  }

  private bool IsResizeColumnsToPrefTableWidth(
    WTable table,
    WTableColumnCollection columns,
    float clientWidth,
    ref float totalMaximumWordWidth,
    ref float totalSpaceToExpand)
  {
    WSection ownerSection = table.GetOwnerSection((Entity) table) as WSection;
    float num1 = 0.0f;
    if (table.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 && (double) table.PreferredTableWidth.Width > 0.0 && table.PreferredTableWidth.WidthType == FtsWidth.Percentage && table.TableFormat.IsAutoResized && !table.IsInCell && ownerSection != null && table.MaximumCellCount() == table.TableGrid.Count && !table.HasPercentPreferredCellWidth && (table.HasPointPreferredCellWidth || table.HasNonePreferredCellWidth || table.HasAutoPreferredCellWidth))
      num1 = (float) ((double) clientWidth * (double) table.PreferredTableWidth.Width / 100.0);
    if ((double) num1 <= 0.0 || (double) num1 <= (double) ownerSection.PageSetup.ClientWidth || (double) num1 >= (double) ownerSection.PageSetup.PageSize.Width)
      return false;
    totalMaximumWordWidth = this.GetTotalWidth((byte) 2);
    float num2 = 0.0f;
    float leftPad = 0.0f;
    float rightPad = 0.0f;
    table.CalculatePaddingOfTableWidth(ref leftPad, ref rightPad);
    float num3 = num2 + (leftPad + rightPad);
    if ((double) table.TableFormat.CellSpacing > 0.0)
      num3 = num3 + (table.TableFormat.CellSpacing * 2f + table.TableFormat.Borders.Left.GetLineWidthValue()) + (table.TableFormat.CellSpacing * 2f + table.TableFormat.Borders.Right.GetLineWidthValue());
    totalSpaceToExpand = num1 - totalMaximumWordWidth;
    return (double) totalSpaceToExpand > (double) num3;
  }

  private void ResizeColumnsToPrefTableWidth(
    WTableColumnCollection columns,
    float totalMaximumWordWidth,
    float totalSpaceToExpand)
  {
    foreach (WTableColumn column in (CollectionImpl) columns)
    {
      float num = column.MaximumWordWidth / totalMaximumWordWidth;
      column.PreferredWidth = column.MaximumWordWidth + num * totalSpaceToExpand;
    }
  }

  internal float GetCellWidth(int columnIndex, int columnSpan)
  {
    float cellWidth = 0.0f;
    for (int index = 0; index < columnSpan; ++index)
      cellWidth += (this.InnerList[index + columnIndex] as WTableColumn).PreferredWidth;
    return cellWidth;
  }

  internal void UpdateEndOffset()
  {
    float a = 0.0f;
    for (int index = 0; index < this.InnerList.Count; ++index)
    {
      WTableColumn inner = this.InnerList[index] as WTableColumn;
      if (index == 0)
        a = inner.PreferredWidth * 20f;
      else
        a += inner.PreferredWidth * 20f;
      inner.EndOffset = (float) Math.Round((double) a);
    }
  }

  internal void ValidateColumnWidths()
  {
    for (int index = 0; index < this.InnerList.Count; ++index)
    {
      if (index == 0)
        (this.InnerList[index] as WTableColumn).PreferredWidth = (this.InnerList[index] as WTableColumn).EndOffset / 20f;
      else
        (this.InnerList[index] as WTableColumn).PreferredWidth = (float) (((double) (this.InnerList[index] as WTableColumn).EndOffset - (double) (this.InnerList[index - 1] as WTableColumn).EndOffset) / 20.0);
    }
  }
}
