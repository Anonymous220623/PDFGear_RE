// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.TemplateMarkers.MarkerArgument
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.TemplateMarkers;

public abstract class MarkerArgument : ICloneable
{
  protected const char DEF_PARTS_SEPARATOR = ':';
  private static int m_maxRowIndex = -1;

  internal static int MaxRowIndex
  {
    get => MarkerArgument.m_maxRowIndex;
    set => MarkerArgument.m_maxRowIndex = value;
  }

  public virtual MarkerArgument TryParse(string strArgument)
  {
    if (strArgument == null || strArgument.Length == 0)
      return (MarkerArgument) null;
    Match m = this.ArgumentChecker.Match(strArgument);
    return !m.Success || m.Length != strArgument.Length ? (MarkerArgument) null : this.Parse(m);
  }

  protected virtual MarkerArgument Parse(Match m) => throw new NotImplementedException();

  public virtual void ApplyArgument(
    IWorksheet sheet,
    Point pOldPosition,
    ref int iRow,
    ref int iColumn,
    IList<long> arrMarkerCells,
    MarkerOptionsImpl options,
    int count)
  {
    throw new NotImplementedException();
  }

  public virtual void PrepareOptions(MarkerOptionsImpl options)
  {
    throw new NotImplementedException();
  }

  public virtual object Clone() => this.MemberwiseClone();

  protected virtual Regex ArgumentChecker => throw new NotImplementedException();

  public virtual int Priority => int.MaxValue;

  public virtual bool IsPreparing => false;

  public virtual bool IsApplyable => false;

  public virtual bool IsAllowMultiple => false;

  protected static void InsertRow(IList<long> arrCells, int i, int iRowIndex, int count)
  {
    int num = arrCells != null ? arrCells.Count : throw new ArgumentNullException(nameof (arrCells));
    if (i < 0 || i > num - 1)
      throw new ArgumentOutOfRangeException(nameof (i), "Value cannot be less than 0 and greater than iCount - 1.");
    if (MarkerArgument.MaxRowIndex < iRowIndex)
      return;
    for (; i < num; ++i)
    {
      long arrCell = arrCells[i];
      int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(arrCell);
      if (rowFromCellIndex >= iRowIndex)
      {
        int firstRow;
        if (count > 1)
        {
          if (MarkerArgument.MaxRowIndex == rowFromCellIndex)
            MarkerArgument.MaxRowIndex += count;
          firstRow = rowFromCellIndex + count;
        }
        else
        {
          if (MarkerArgument.MaxRowIndex == rowFromCellIndex)
            ++MarkerArgument.MaxRowIndex;
          firstRow = rowFromCellIndex + 1;
        }
        long cellIndex = RangeImpl.GetCellIndex(RangeImpl.GetColumnFromCellIndex(arrCell), firstRow);
        arrCells[i] = cellIndex;
      }
    }
  }

  protected static void InsertColumn(IList<long> arrCells, int i, int iColumnIndex, int count)
  {
    int num = arrCells != null ? arrCells.Count : throw new ArgumentNullException(nameof (arrCells));
    if (i < 0 || i > num - 1)
      throw new ArgumentOutOfRangeException(nameof (i), "Value cannot be less than 0 and greater than iCount - 1.");
    for (; i < num; ++i)
    {
      long arrCell = arrCells[i];
      int columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(arrCell);
      if (columnFromCellIndex >= iColumnIndex)
      {
        long cellIndex = RangeImpl.GetCellIndex(count <= 1 ? columnFromCellIndex + 1 : columnFromCellIndex + count, RangeImpl.GetRowFromCellIndex(arrCell));
        arrCells[i] = cellIndex;
      }
    }
  }
}
