// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.ItemSizeHelper
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class ItemSizeHelper
{
  private List<int> m_arrSizeSum = new List<int>();
  private ItemSizeHelper.SizeGetter m_getter;

  public ItemSizeHelper(ItemSizeHelper.SizeGetter sizeGetter)
  {
    this.m_getter = sizeGetter != null ? sizeGetter : throw new ArgumentNullException(nameof (sizeGetter));
    this.m_arrSizeSum.Add(0);
  }

  public int GetTotal(int rowIndex)
  {
    int count = this.m_arrSizeSum.Count;
    if (count <= rowIndex)
    {
      this.m_arrSizeSum.Capacity = Math.Max(this.m_arrSizeSum.Capacity, rowIndex);
      int num = this.m_arrSizeSum[count - 1];
      for (int index = count; index <= rowIndex; ++index)
      {
        num += this.m_getter(index);
        this.m_arrSizeSum.Add(num);
      }
    }
    return this.m_arrSizeSum[rowIndex];
  }

  public int GetTotal(int rowStart, int rowEnd)
  {
    return rowStart > rowEnd ? 0 : this.GetTotal(rowEnd) - this.GetTotal(rowStart - 1);
  }

  public int GetSize(int itemIndex) => this.GetTotal(itemIndex) - this.GetTotal(itemIndex - 1);

  internal delegate int SizeGetter(int index);
}
