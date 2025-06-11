// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ItemSizeHelper
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class ItemSizeHelper
{
  internal List<double> m_arrSizeSum = new List<double>();
  private ItemSizeHelper.SizeGetter m_getter;
  internal double ScaledCellHeight = 1.0;
  internal double ScaledCellWidth = 1.0;

  public ItemSizeHelper(ItemSizeHelper.SizeGetter sizeGetter)
  {
    this.m_getter = sizeGetter != null ? sizeGetter : throw new ArgumentNullException(nameof (sizeGetter));
    this.m_arrSizeSum.Add(0.0);
  }

  public float GetTotal(int rowIndex) => (float) this.GetTotalInDouble(rowIndex);

  internal double GetTotalInDouble(int rowIndex)
  {
    int count = this.m_arrSizeSum.Count;
    if (count <= rowIndex)
    {
      this.m_arrSizeSum.Capacity = Math.Max(this.m_arrSizeSum.Capacity, rowIndex);
      double num1 = this.m_arrSizeSum[count - 1];
      for (int index = count; index <= rowIndex; ++index)
      {
        double num2 = (double) this.m_getter(index);
        if (this.m_getter.Method.Name == "GetColumnWidthInPixels")
          num1 += num2 * this.ScaledCellWidth;
        if (this.m_getter.Method.Name == "GetRowHeightInPixels")
          num1 += num2 * this.ScaledCellHeight;
        this.m_arrSizeSum.Add(num1);
      }
    }
    return this.m_arrSizeSum[rowIndex];
  }

  internal void UpdateColumnIndexValue(int columnIndex, float value)
  {
    this.m_arrSizeSum[columnIndex] = (double) (int) value + this.m_arrSizeSum[columnIndex - 1];
  }

  internal void UpdateRowIndexValue(int rowIndex, float value)
  {
    if (rowIndex >= this.m_arrSizeSum.Count || this.m_arrSizeSum.Count == 1)
      return;
    double num = this.m_arrSizeSum[rowIndex - 1] + (double) value - this.m_arrSizeSum[rowIndex];
    this.m_arrSizeSum[rowIndex] = this.m_arrSizeSum[rowIndex - 1] + (double) value;
    if (this.m_arrSizeSum.Count <= rowIndex + 1 || num == 0.0)
      return;
    for (int index = rowIndex + 1; index < this.m_arrSizeSum.Count; ++index)
      this.m_arrSizeSum[index] += num;
  }

  public float GetTotal(int rowStart, int rowEnd)
  {
    return rowStart > rowEnd ? 0.0f : this.GetTotal(rowEnd) - this.GetTotal(rowStart - 1);
  }

  public float GetSize(int itemIndex) => this.GetTotal(itemIndex) - this.GetTotal(itemIndex - 1);

  internal double GetSizeInDouble(int itemIndex)
  {
    return this.GetTotalInDouble(itemIndex) - this.GetTotalInDouble(itemIndex - 1);
  }

  public delegate int SizeGetter(int index);
}
