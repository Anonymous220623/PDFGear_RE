// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.TemplateMarkers.RangeBuilder
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.TemplateMarkers;

public class RangeBuilder
{
  private List<Rectangle> m_arrRanges = new List<Rectangle>();

  public Rectangle this[int index] => this.m_arrRanges[index];

  public int Count => this.m_arrRanges.Count;

  public void Add(int iRow, int iColumn)
  {
    int count = this.Count;
    bool flag = true;
    Rectangle empty = Rectangle.Empty;
    if (count > 0)
    {
      empty = this[count - 1];
      if (empty.Width == 0 && empty.Left == iColumn)
      {
        flag = false;
        if (empty.Top - 1 == iRow)
        {
          ++empty.Height;
          --empty.Y;
        }
        else if (empty.Bottom + 1 == iRow)
          ++empty.Height;
        else
          flag = true;
      }
      else if (empty.Height == 0)
      {
        flag = false;
        if (empty.Left - 1 == iColumn)
        {
          ++empty.Width;
          --empty.X;
        }
        else if (empty.Right + 1 == iColumn)
          ++empty.Width;
        else
          flag = true;
      }
    }
    if (flag)
      this.m_arrRanges.Add(RangeBuilder.GetRectangle(iRow, iColumn));
    else
      this.m_arrRanges[count - 1] = empty;
  }

  public void Clear() => this.m_arrRanges.Clear();

  public IRange ToRange(IWorksheet parentWorksheet)
  {
    if (parentWorksheet == null)
      throw new ArgumentNullException(nameof (parentWorksheet));
    int count = this.m_arrRanges.Count;
    switch (count)
    {
      case 0:
        return (IRange) null;
      case 1:
        Rectangle rectangle1 = this[0];
        return parentWorksheet[rectangle1.Top, rectangle1.Left, rectangle1.Bottom, rectangle1.Right];
      default:
        IRanges rangesCollection = parentWorksheet.CreateRangesCollection();
        for (int index = 0; index < count; ++index)
        {
          Rectangle rectangle2 = this[index];
          rangesCollection.Add(parentWorksheet.Range[rectangle2.Top, rectangle2.Left, rectangle2.Bottom, rectangle2.Right]);
        }
        return (IRange) rangesCollection;
    }
  }

  public static Rectangle GetRectangle(int iRow, int iColumn)
  {
    return Rectangle.FromLTRB(iColumn, iRow, iColumn, iRow);
  }
}
