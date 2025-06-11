// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Sorting.HeapSortImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Sorting;

internal class HeapSortImpl(object[][] data, Type[] types, OrderBy[] orderBy, Color[] m_colors) : 
  SortingAlgorithm(data, types, orderBy, m_colors)
{
  public override void Sort(int left, int right, int columnIndex)
  {
    this.SortOnTypes(left, right, columnIndex);
  }

  public new void SortOnTypes(int left, int right, int columnIndex)
  {
    if (this.types[columnIndex - 1] == typeof (double))
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

  private void CreateIntHeap(int position, int length, int columnIndex)
  {
    for (int right = 2 * position + 1; right <= length; right = 2 * position + 1)
    {
      if (right < length && (int) this.m_data[right][columnIndex] < (int) this.m_data[right + 1][columnIndex])
        ++right;
      if ((double) this.m_data[position][columnIndex] >= (double) (int) this.m_data[right][columnIndex])
        break;
      this.SwapRow(position, right);
      position = right;
    }
  }

  public new void SortInt(int root, int length, int columnIndex)
  {
    for (int position = (length - 1) / 2; position >= root; --position)
      this.CreateIntHeap(position, length - 1, columnIndex);
    for (int left = length - 1; left > root; --left)
    {
      this.SwapRow(left, 0);
      this.CreateIntHeap(0, left - 1, columnIndex);
    }
  }

  private void CreateFloatHeap(int position, int length, int columnIndex)
  {
    for (int right = 2 * position + 1; right <= length; right = 2 * position + 1)
    {
      if (right < length && (double) this.m_data[right][columnIndex] < (double) this.m_data[right + 1][columnIndex])
        ++right;
      if ((double) this.m_data[position][columnIndex] >= (double) this.m_data[right][columnIndex])
        break;
      this.SwapRow(position, right);
      position = right;
    }
  }

  public new void SortFloat(int root, int length, int columnIndex)
  {
    for (int position = (length - 1) / 2; position >= root; --position)
      this.CreateFloatHeap(position, length - 1, columnIndex);
    for (int left = length - 1; left > root; --left)
    {
      this.SwapRow(left, 0);
      this.CreateFloatHeap(0, left - 1, columnIndex);
    }
  }

  private void CreateDateHeap(int position, int length, int columnIndex)
  {
    for (int right = 2 * position + 1; right <= length; right = 2 * position + 1)
    {
      if (right < length && (DateTime) this.m_data[right][columnIndex] < (DateTime) this.m_data[right + 1][columnIndex])
        ++right;
      if (!((DateTime) this.m_data[position][columnIndex] < (DateTime) this.m_data[right][columnIndex]))
        break;
      this.SwapRow(position, right);
      position = right;
    }
  }

  public new void SortDate(int root, int length, int columnIndex)
  {
    for (int position = (length - 1) / 2; position >= root; --position)
      this.CreateDateHeap(position, length - 1, columnIndex);
    for (int left = length - 1; left > root; --left)
    {
      this.SwapRow(left, 0);
      this.CreateDateHeap(0, left - 1, columnIndex);
    }
  }

  private void CreateStringHeap(int position, int length, int columnIndex)
  {
    for (int right = 2 * position + 1; right <= length; right = 2 * position + 1)
    {
      if (right < length && ((string) this.m_data[right][columnIndex]).CompareTo((string) this.m_data[right + 1][columnIndex]) < 0)
        ++right;
      if (((string) this.m_data[position][columnIndex]).CompareTo((string) this.m_data[right][columnIndex]) >= 0)
        break;
      this.SwapRow(position, right);
      position = right;
    }
  }

  public new void SortString(int root, int length, int columnIndex)
  {
    for (int position = (length - 1) / 2; position >= root; --position)
      this.CreateStringHeap(position, length - 1, columnIndex);
    for (int left = length - 1; left > root; --left)
    {
      this.SwapRow(left, 0);
      this.CreateStringHeap(0, left - 1, columnIndex);
    }
  }

  private void CreateDescIntHeap(int position, int length, int columnIndex)
  {
    for (int right = 2 * position + 1; right <= length; right = 2 * position + 1)
    {
      if (right < length && (double) this.m_data[right][columnIndex] > (double) this.m_data[right + 1][columnIndex])
        ++right;
      if ((double) this.m_data[position][columnIndex] <= (double) this.m_data[right][columnIndex])
        break;
      this.SwapRow(position, right);
      position = right;
    }
  }

  public new void SortIntDesc(int root, int length, int columnIndex)
  {
    for (int position = (length - 1) / 2; position >= root; --position)
      this.CreateDescIntHeap(position, length - 1, columnIndex);
    for (int left = length - 1; left > root; --left)
    {
      this.SwapRow(left, 0);
      this.CreateDescIntHeap(0, left - 1, columnIndex);
    }
  }

  private void CreateDescFloatHeap(int position, int length, int columnIndex)
  {
    for (int right = 2 * position + 1; right <= length; right = 2 * position + 1)
    {
      if (right < length && (double) this.m_data[right][columnIndex] > (double) this.m_data[right + 1][columnIndex])
        ++right;
      if ((double) this.m_data[position][columnIndex] <= (double) this.m_data[right][columnIndex])
        break;
      this.SwapRow(position, right);
      position = right;
    }
  }

  public new void SortFloatDesc(int root, int length, int columnIndex)
  {
    for (int position = (length - 1) / 2; position >= root; --position)
      this.CreateDescFloatHeap(position, length - 1, columnIndex);
    for (int left = length - 1; left > root; --left)
    {
      this.SwapRow(left, 0);
      this.CreateDescFloatHeap(0, left - 1, columnIndex);
    }
  }

  private void CreateDescDateHeap(int position, int length, int columnIndex)
  {
    for (int right = 2 * position + 1; right <= length; right = 2 * position + 1)
    {
      if (right < length && (DateTime) this.m_data[right][columnIndex] > (DateTime) this.m_data[right + 1][columnIndex])
        ++right;
      if (!((DateTime) this.m_data[position][columnIndex] > (DateTime) this.m_data[right][columnIndex]))
        break;
      this.SwapRow(position, right);
      position = right;
    }
  }

  public new void SortDateDesc(int root, int length, int columnIndex)
  {
    for (int position = (length - 1) / 2; position >= root; --position)
      this.CreateDescDateHeap(position, length - 1, columnIndex);
    for (int left = length - 1; left > root; --left)
    {
      this.SwapRow(left, 0);
      this.CreateDescDateHeap(0, left - 1, columnIndex);
    }
  }

  private void CreateDescStringHeap(int position, int length, int columnIndex)
  {
    for (int right = 2 * position + 1; right <= length; right = 2 * position + 1)
    {
      if (right < length && ((string) this.m_data[right][columnIndex]).CompareTo((string) this.m_data[right + 1][columnIndex]) > 0)
        ++right;
      if (((string) this.m_data[position][columnIndex]).CompareTo((string) this.m_data[right][columnIndex]) <= 0)
        break;
      this.SwapRow(position, right);
      position = right;
    }
  }

  public new void SortStringDesc(int root, int length, int columnIndex)
  {
    for (int position = (length - 1) / 2; position >= root; --position)
      this.CreateDescStringHeap(position, length - 1, columnIndex);
    for (int left = length - 1; left > root; --left)
    {
      this.SwapRow(left, 0);
      this.CreateDescStringHeap(0, left - 1, columnIndex);
    }
  }

  public new IRange Range
  {
    get => throw new NotSupportedException(nameof (Range));
    set => throw new NotSupportedException(nameof (Range));
  }
}
