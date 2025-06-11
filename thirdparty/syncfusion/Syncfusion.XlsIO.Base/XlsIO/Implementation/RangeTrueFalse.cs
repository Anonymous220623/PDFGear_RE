// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.RangeTrueFalse
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class RangeTrueFalse
{
  private RangesOperations m_trueValues = new RangesOperations();
  private RangesOperations m_falseValues = new RangesOperations();

  public bool? GetRangeValue(ICombinedRange range)
  {
    Rectangle[] rectangles = range.GetRectangles();
    bool? rangeValue = new bool?();
    if (this.m_trueValues.Contains(rectangles))
      rangeValue = new bool?(true);
    else if (this.m_falseValues.Contains(rectangles))
      rangeValue = new bool?(false);
    return rangeValue;
  }

  public void SetRange(ICombinedRange range, bool? value)
  {
    Rectangle[] rectangles = range.GetRectangles();
    if (!value.HasValue)
    {
      this.m_trueValues.Remove(rectangles);
      this.m_falseValues.Remove(rectangles);
    }
    else
    {
      bool? nullable = value;
      if ((!nullable.GetValueOrDefault() ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
      {
        this.m_trueValues.AddRectangles((IList<Rectangle>) rectangles);
        this.m_falseValues.Remove(rectangles);
      }
      else
      {
        this.m_trueValues.Remove(rectangles);
        this.m_falseValues.AddRectangles((IList<Rectangle>) rectangles);
      }
    }
  }

  public void Clear()
  {
    this.m_trueValues.Clear();
    this.m_falseValues.Clear();
  }
}
