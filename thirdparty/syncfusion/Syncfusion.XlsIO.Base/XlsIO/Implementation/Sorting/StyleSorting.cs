// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Sorting.StyleSorting
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Sorting;

internal class StyleSorting(object[][] data, Type[] types, OrderBy[] orderBy, Color[] colors) : 
  SortingAlgorithm(data, types, orderBy, colors)
{
  public override void Sort(int left, int right, int columnIndex)
  {
    this.SortByAlign(left, right, columnIndex);
  }

  private void SortByAlign(int left, int right, int columnIndex)
  {
    if (this.orderBy[columnIndex - 1] == OrderBy.OnBottom)
      this.SortBottomByCellColor(left, right, columnIndex);
    else
      this.SortTopByCellColor(left, right, columnIndex);
  }

  private void SortTopByCellColor(int left, int right, int columnIndex)
  {
    for (int iTopPosition = this.m_iTopPosition; iTopPosition <= right; ++iTopPosition)
    {
      if (this.Compare(this.m_colors[columnIndex - 1], (Color) this.m_data[iTopPosition][columnIndex]))
      {
        this.MoveUp(iTopPosition, this.m_iTopPosition);
        ++this.m_iTopPosition;
      }
    }
  }

  private bool Compare(Color color1, Color color2)
  {
    return (int) color1.R == (int) color2.R && (int) color1.G == (int) color2.G && (int) color1.B == (int) color2.B;
  }

  private void SortBottomByCellColor(int left, int right, int columnIndex)
  {
    for (int iBottomPosition = this.m_iBottomPosition; iBottomPosition >= left; --iBottomPosition)
    {
      if (this.Compare(this.m_colors[columnIndex - 1], (Color) this.m_data[iBottomPosition][columnIndex]))
      {
        this.MoveDown(iBottomPosition, this.m_iBottomPosition);
        --this.m_iBottomPosition;
      }
    }
  }

  internal void MoveDown(int srcIndex, int destIndex)
  {
    for (int left = srcIndex; left < destIndex; ++left)
      this.SwapRow(left, left + 1);
  }

  internal void MoveUp(int srcIndex, int destIndex)
  {
    for (int left = srcIndex; left > destIndex; --left)
      this.SwapRow(left, left - 1);
  }

  public new void SortInt(int left, int right, int columnIndex)
  {
    throw new NotImplementedException();
  }

  public new void SortFloat(int left, int right, int columnIndex)
  {
    throw new NotImplementedException();
  }

  public new void SortDate(int left, int right, int columnIndex)
  {
    throw new NotImplementedException();
  }

  public new void SortString(int left, int right, int columnIndex)
  {
    throw new NotImplementedException();
  }

  public new void SortOnTypes(int left, int right, int columnIndex)
  {
    throw new NotImplementedException();
  }

  public new void SortIntDesc(int left, int right, int columnIndex)
  {
    throw new NotImplementedException();
  }

  public new void SortFloatDesc(int left, int right, int columnIndex)
  {
    throw new NotImplementedException();
  }

  public new void SortDateDesc(int left, int right, int columnIndex)
  {
    throw new NotImplementedException();
  }

  public new void SortStringDesc(int left, int right, int columnIndex)
  {
    throw new NotImplementedException();
  }
}
