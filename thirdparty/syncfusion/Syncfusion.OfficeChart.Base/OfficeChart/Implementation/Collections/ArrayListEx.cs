// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.ArrayListEx
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections;

internal class ArrayListEx
{
  private RowStorage[] m_items;
  private int m_iCount;

  public ArrayListEx()
  {
  }

  public ArrayListEx(int iCount)
  {
    this.m_iCount = iCount > 0 ? iCount : throw new ArgumentOutOfRangeException(nameof (iCount));
    this.m_items = new RowStorage[this.m_iCount];
  }

  public RowStorage this[int index]
  {
    get => index >= 0 && index < this.m_iCount ? this.m_items[index] : (RowStorage) null;
    set
    {
      if (this.m_iCount <= index)
        this.UpdateSize(index + 1);
      this.m_items[index] = value;
    }
  }

  public void UpdateSize(int iCount)
  {
    if (iCount <= this.m_iCount)
      return;
    int num = this.m_iCount * 2;
    this.m_iCount = iCount >= num ? iCount : num;
    RowStorage[] rowStorageArray = new RowStorage[this.m_iCount];
    if (this.m_items != null)
      this.m_items.CopyTo((Array) rowStorageArray, 0);
    this.m_items = rowStorageArray;
  }

  public void ReduceSizeIfNecessary(int iCount)
  {
    if (iCount < 0)
      throw new ArgumentOutOfRangeException(nameof (iCount));
    if (iCount >= this.m_iCount)
      return;
    RowStorage[] destinationArray = new RowStorage[iCount];
    Array.Copy((Array) this.m_items, 0, (Array) destinationArray, 0, iCount);
    this.m_items = destinationArray;
    this.m_iCount = iCount;
  }

  public void Insert(int index, int count, int length)
  {
    Array.Copy((Array) this.m_items, index, (Array) this.m_items, index + count, length);
    int index1 = index;
    for (int index2 = index + count; index1 < index2; ++index1)
      this.m_items[index1] = (RowStorage) null;
  }

  internal int GetCount() => this.m_iCount;

  internal bool GetRowIndex(int row, out int arrIndex)
  {
    if (this.m_iCount == 0)
    {
      arrIndex = 0;
      return false;
    }
    int num1 = 0;
    int num2 = 0;
    int num3 = this.m_iCount - 1;
    if (num3 == row)
    {
      arrIndex = row;
      return true;
    }
    if (num3 < row)
    {
      arrIndex = num3 + 1;
      return false;
    }
    while (num2 <= num3)
    {
      num1 = (num2 + num3) / 2;
      int num4 = row - num1;
      if (num4 == 0)
      {
        arrIndex = num1;
        return true;
      }
      if (num4 < 0)
        num3 = num1 - 1;
      else
        num2 = num1 + 1;
    }
    arrIndex = row <= num1 ? num1 : num1 + 1;
    return false;
  }
}
