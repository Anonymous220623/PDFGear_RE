// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Sorting.SortingAlgorithm
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Sorting;

internal abstract class SortingAlgorithm : ISortingAlgorithm
{
  protected object[][] m_data;
  protected int count;
  protected Type[] types;
  protected OrderBy[] orderBy;
  protected int m_iTopPosition;
  protected int m_iBottomPosition;
  protected Color[] m_colors;

  public object[][] Data => this.m_data;

  public IRange Range
  {
    get => throw new NotSupportedException(nameof (Range));
    set => throw new NotSupportedException(nameof (Range));
  }

  public SortingAlgorithm(object[][] data, Type[] types, OrderBy[] orderBy, Color[] colors)
  {
    this.m_data = data;
    this.types = types;
    this.orderBy = orderBy;
    this.m_colors = colors;
    this.count = types.Length;
    this.m_iTopPosition = 0;
    this.m_iBottomPosition = data.Length - 1;
  }

  public abstract void Sort(int left, int right, int columnIndex);

  protected object[] ExtractSingleRow(int rowIndex)
  {
    object[] singleRow = new object[this.m_data[0].Length];
    for (int index = 0; index < singleRow.Length; ++index)
      singleRow[index] = this.m_data[rowIndex][index];
    return singleRow;
  }

  protected object[] ExtractSingleColumn(int rowIndex)
  {
    object[] singleColumn = new object[this.m_data.Length];
    for (int index = 0; index < singleColumn.Length; ++index)
      singleColumn[index] = this.m_data[index][rowIndex];
    return singleColumn;
  }

  protected void SwapRow(int left, int right) => this.Swap(this.m_data[left], this.m_data[right]);

  private void Swap(object[] left, object[] right)
  {
    for (int index = 0; index < left.Length; ++index)
    {
      object obj = right[index];
      right[index] = left[index];
      left[index] = obj;
    }
  }

  protected void SwapColumn(int left, int right)
  {
    for (int index = 0; index < this.m_data[0].Length; ++index)
    {
      object obj = this.m_data[index][left];
      this.m_data[index][left] = this.m_data[index][right];
      this.m_data[index][right] = obj;
    }
  }

  private void SwapColumn(object[] left, object[] right)
  {
    for (int index = 0; index < left.Length; ++index)
    {
      object obj = right[index];
      right[index] = left[index];
      left[index] = obj;
    }
  }

  public virtual void SortInt(int left, int right, int columnIndex)
  {
    throw new NotImplementedException();
  }

  public virtual void SortFloat(int left, int right, int columnIndex)
  {
    throw new NotImplementedException();
  }

  public virtual void SortDate(int left, int right, int columnIndex)
  {
    throw new NotImplementedException();
  }

  public virtual void SortString(int left, int right, int columnIndex)
  {
    throw new NotImplementedException();
  }

  public virtual void SortOnTypes(int left, int right, int columnIndex)
  {
    throw new NotImplementedException();
  }

  public virtual void SortIntDesc(int left, int right, int columnIndex)
  {
    throw new NotImplementedException();
  }

  public virtual void SortFloatDesc(int left, int right, int columnIndex)
  {
    throw new NotImplementedException();
  }

  public virtual void SortDateDesc(int left, int right, int columnIndex)
  {
    throw new NotImplementedException();
  }

  public virtual void SortStringDesc(int left, int right, int columnIndex)
  {
    throw new NotImplementedException();
  }
}
