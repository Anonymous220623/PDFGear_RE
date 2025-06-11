// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Sorting.InsertionSortImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Sorting;

internal class InsertionSortImpl(object[][] data, Type[] types, OrderBy[] orderBy, Color[] colors) : 
  SortingAlgorithm(data, types, orderBy, colors)
{
  public new void SortInt(int left, int right, int columnIndex)
  {
    int index = columnIndex;
    for (int rowIndex = left + 1; rowIndex <= right; ++rowIndex)
    {
      this.ExtractSingleRow(rowIndex);
      int num = (int) this.m_data[rowIndex][index];
      for (int right1 = rowIndex; right1 > left && (int) this.m_data[right1 - 1][index] >= num; --right1)
      {
        if (columnIndex + 1 <= this.count && num == (int) this.m_data[right1 - 1][index] && columnIndex + 1 < this.count)
          this.SortOnTypes(right1 - 1, right1, columnIndex + 1);
        else
          this.SwapRow(right1 - 1, right1);
      }
    }
  }

  public new void SortFloat(int left, int right, int columnIndex)
  {
    int index = columnIndex;
    for (int rowIndex = left + 1; rowIndex <= right; ++rowIndex)
    {
      this.ExtractSingleRow(rowIndex);
      double num = (double) this.m_data[rowIndex][index];
      for (int right1 = rowIndex; right1 > left && (double) this.m_data[right1 - 1][index] >= num; --right1)
      {
        if (columnIndex + 1 <= this.count && num == (double) this.m_data[right1 - 1][index])
          this.SortOnTypes(right1 - 1, right1, columnIndex + 1);
        else
          this.SwapRow(right1 - 1, right1);
      }
    }
  }

  public new void SortDate(int left, int right, int columnIndex)
  {
    int index = columnIndex;
    for (int rowIndex = left + 1; rowIndex <= right; ++rowIndex)
    {
      this.ExtractSingleRow(rowIndex);
      DateTime dateTime = (DateTime) this.m_data[rowIndex][index];
      for (int right1 = rowIndex; right1 > left && (DateTime) this.m_data[right1 - 1][index] >= dateTime; --right1)
      {
        if (columnIndex + 1 <= this.count && dateTime == (DateTime) this.m_data[right1 - 1][index])
          this.SortOnTypes(right1 - 1, right1, columnIndex + 1);
        else
          this.SwapRow(right1 - 1, right1);
      }
    }
  }

  public new void SortString(int left, int right, int columnIndex)
  {
    int index = columnIndex;
    for (int rowIndex = left + 1; rowIndex <= right; ++rowIndex)
    {
      this.ExtractSingleRow(rowIndex);
      string strB = (string) this.m_data[rowIndex][index];
      for (int right1 = rowIndex; right1 > left && ((string) this.m_data[right1 - 1][index]).CompareTo(strB) >= 0; --right1)
      {
        if (columnIndex + 1 <= this.count && strB == (string) this.m_data[right1 - 1][index])
          this.SortOnTypes(right1 - 1, right1, columnIndex + 1);
        else
          this.SwapRow(right1 - 1, right1);
      }
    }
  }

  public new void SortOnTypes(int left, int right, int columnIndex)
  {
    if (this.types[columnIndex - 1] == typeof (int))
      this.SortInt(left, right, columnIndex);
    else if (this.types[columnIndex - 1] == typeof (double))
    {
      if (this.orderBy[columnIndex - 1] == OrderBy.Ascending)
        this.SortFloat(left, right, columnIndex);
      else
        this.SortFloatDesc(left, right, columnIndex);
    }
    else if (this.types[columnIndex - 1] == typeof (string))
    {
      if (this.orderBy[columnIndex - 1] == OrderBy.Ascending)
        this.SortString(left, right, columnIndex);
      else
        this.SortStringDesc(left, right, columnIndex);
    }
    else
    {
      if (!(this.types[columnIndex - 1] == typeof (DateTime)))
        return;
      if (this.orderBy[columnIndex - 1] == OrderBy.Ascending)
        this.SortDate(left, right, columnIndex);
      else
        this.SortDateDesc(left, right, columnIndex);
    }
  }

  public new void SortIntDesc(int left, int right, int columnIndex)
  {
    int index = columnIndex;
    for (int rowIndex = left + 1; rowIndex <= right; ++rowIndex)
    {
      this.ExtractSingleRow(rowIndex);
      int num = (int) this.m_data[rowIndex][index];
      for (int right1 = rowIndex; right1 > left && (int) this.m_data[right1 - 1][index] <= num; --right1)
      {
        if (columnIndex + 1 <= this.count && num == (int) this.m_data[right1 - 1][index])
          this.SortOnTypes(right1 - 1, right1, columnIndex + 1);
        else
          this.SwapRow(right1 - 1, right1);
      }
    }
  }

  public new void SortFloatDesc(int left, int right, int columnIndex)
  {
    int index = columnIndex;
    for (int rowIndex = left + 1; rowIndex <= right; ++rowIndex)
    {
      this.ExtractSingleRow(rowIndex);
      double num = (double) this.m_data[rowIndex][index];
      for (int right1 = rowIndex; right1 > left && (double) this.m_data[right1 - 1][index] <= num; --right1)
      {
        if (columnIndex + 1 <= this.count && num == (double) this.m_data[right1 - 1][index])
          this.SortOnTypes(right1 - 1, right1, columnIndex + 1);
        else
          this.SwapRow(right1 - 1, right1);
      }
    }
  }

  public new void SortDateDesc(int left, int right, int columnIndex)
  {
    int index = columnIndex;
    for (int rowIndex = left + 1; rowIndex <= right; ++rowIndex)
    {
      this.ExtractSingleRow(rowIndex);
      DateTime dateTime = (DateTime) this.m_data[rowIndex][index];
      for (int right1 = rowIndex; right1 > left && (DateTime) this.m_data[right1 - 1][index] <= dateTime; --right1)
      {
        if (columnIndex + 1 <= this.count && dateTime == (DateTime) this.m_data[right1 - 1][index])
          this.SortOnTypes(right1 - 1, right1, columnIndex + 1);
        else
          this.SwapRow(right1 - 1, right1);
      }
    }
  }

  public new void SortStringDesc(int left, int right, int columnIndex)
  {
    int index = columnIndex;
    for (int rowIndex = left + 1; rowIndex <= right; ++rowIndex)
    {
      this.ExtractSingleRow(rowIndex);
      string strB = (string) this.m_data[rowIndex][index];
      for (int right1 = rowIndex; right1 > left && ((string) this.m_data[right1 - 1][index]).CompareTo(strB) <= 0; --right1)
      {
        if (columnIndex + 1 <= this.count && strB == (string) this.m_data[right1 - 1][index])
          this.SortOnTypes(right1 - 1, right1, columnIndex + 1);
        else
          this.SwapRow(right1 - 1, right1);
      }
    }
  }

  public override void Sort(int left, int right, int columnIndex)
  {
    this.SortOnTypes(left, right, columnIndex);
  }

  public new IRange Range
  {
    get => throw new NotSupportedException(nameof (Range));
    set => throw new NotSupportedException(nameof (Range));
  }
}
