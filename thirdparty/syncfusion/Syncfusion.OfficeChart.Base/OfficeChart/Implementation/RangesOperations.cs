// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.RangesOperations
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class RangesOperations
{
  private const int DEF_MAXIMUM_SPLIT_COUNT = 4;
  private List<Rectangle> m_arrCells;

  public RangesOperations()
    : this(new List<Rectangle>())
  {
  }

  public RangesOperations(List<Rectangle> arrCells) => this.m_arrCells = arrCells;

  public virtual List<Rectangle> CellList
  {
    get => this.m_arrCells;
    set => this.m_arrCells = value;
  }

  public bool Contains(Rectangle[] arrRanges) => this.Contains(arrRanges, 0);

  public bool Contains(Rectangle[] arrRanges, int iStartIndex)
  {
    if (arrRanges == null)
      return true;
    int length = arrRanges.Length;
    if (length == 0)
      return true;
    if (iStartIndex < 0)
      iStartIndex = 0;
    for (int index = 0; index < length; ++index)
    {
      if (!this.Contains(arrRanges[index], iStartIndex))
        return false;
    }
    return true;
  }

  public bool Contains(IList<Rectangle> arrRanges) => this.Contains(arrRanges, 0);

  public bool Contains(IList<Rectangle> arrRanges, int iStartIndex)
  {
    if (arrRanges == null)
      return true;
    int count = arrRanges.Count;
    if (count == 0)
      return true;
    if (iStartIndex < 0)
      iStartIndex = 0;
    for (int index = 0; index < count; ++index)
    {
      if (!this.Contains(arrRanges[index], iStartIndex))
        return false;
    }
    return true;
  }

  public bool Contains(Rectangle range) => this.Contains(range, 0);

  public bool Contains(Rectangle range, int iStartIndex)
  {
    List<Rectangle> cellList = this.CellList;
    int num = iStartIndex;
    for (int count = cellList.Count; num < count; ++num)
    {
      Rectangle rectRemove = cellList[num];
      IList<Rectangle> arrRanges = this.SplitRectangle(range, rectRemove);
      if (arrRanges != null)
        return this.Contains(arrRanges, num);
    }
    return false;
  }

  public int ContainsCount(Rectangle range)
  {
    List<Rectangle> cellList = this.CellList;
    int num = 0;
    int index = 0;
    for (int count = cellList.Count; index < count; ++index)
    {
      Rectangle rectRemove = cellList[index];
      if (this.SplitRectangle(range, rectRemove) != null)
        ++num;
    }
    return num;
  }

  public void AddCells(IList<Rectangle> arrCells)
  {
    if (arrCells == null)
      return;
    int index = 0;
    for (int count = arrCells.Count; index < count; ++index)
      this.AddRange(arrCells[index]);
  }

  public void AddRectangles(IList<Rectangle> arrCells)
  {
    if (arrCells == null)
      return;
    int index = 0;
    for (int count = arrCells.Count; index < count; ++index)
      this.AddRange(arrCells[index]);
  }

  public void AddRange(IRange range)
  {
    Rectangle[] rectangleArray = range != null ? ((ICombinedRange) range).GetRectangles() : throw new ArgumentNullException(nameof (range));
    int index = 0;
    for (int length = rectangleArray.Length; index < length; ++index)
      this.AddRange(rectangleArray[index]);
  }

  public void AddRange(Rectangle rect)
  {
    List<Rectangle> cellList = this.CellList;
    int index1 = 0;
    for (int count = cellList.Count; index1 < count / 2; index1 += 2)
    {
      Rectangle curRange = cellList[index1];
      if (this.CheckAndAddRange(ref curRange, cellList[index1 + 1]))
      {
        cellList.RemoveAt(index1);
        cellList.RemoveAt(index1);
        cellList.Insert(index1, curRange);
      }
    }
    int index2 = 0;
    for (int count = cellList.Count; index2 < count; ++index2)
    {
      Rectangle curRange = cellList[index2];
      if (this.CheckAndAddRange(ref curRange, rect))
      {
        cellList[index2] = curRange;
        return;
      }
      if (curRange.Contains(rect))
        return;
    }
    cellList.Add(rect);
  }

  public void Clear() => this.m_arrCells.Clear();

  public RangesOperations GetPart(
    Rectangle rect,
    bool remove,
    int rowIncrement,
    int columnIncrement)
  {
    RangesOperations rangesOperations = new RangesOperations();
    int index = 0;
    for (int count = this.m_arrCells.Count; index < count; ++index)
    {
      Rectangle arrCell = this.m_arrCells[index];
      if (UtilityMethods.Intersects(arrCell, rect))
      {
        Rectangle rect1 = Rectangle.FromLTRB(Math.Max(arrCell.X, rect.X), Math.Max(arrCell.Y, rect.Y), Math.Min(arrCell.Right, rect.Right), Math.Min(arrCell.Bottom, rect.Bottom));
        rect1.Offset(columnIncrement, rowIncrement);
        rangesOperations.AddRange(rect1);
      }
    }
    if (remove)
      this.Remove(rect);
    return rangesOperations.m_arrCells.Count <= 0 ? (RangesOperations) null : rangesOperations;
  }

  private bool CheckAndAddRange(ref Rectangle curRange, Rectangle rangeToAdd)
  {
    bool flag1 = curRange.Left == rangeToAdd.Left && curRange.Right == rangeToAdd.Right;
    bool flag2 = false;
    if (flag1)
    {
      if (curRange.Bottom == rangeToAdd.Top - 1)
      {
        curRange.Height = rangeToAdd.Bottom - curRange.Top;
        flag2 = true;
      }
      else if (rangeToAdd.Bottom == curRange.Top - 1)
      {
        curRange.Height = curRange.Top - rangeToAdd.Bottom + curRange.Height + rangeToAdd.Height;
        curRange.Y = rangeToAdd.Y;
        flag2 = true;
      }
      else if (curRange.Top == rangeToAdd.Bottom + 1)
      {
        curRange.Y = rangeToAdd.Top;
        flag2 = true;
      }
    }
    if (!flag2 && curRange.Top == rangeToAdd.Top && curRange.Bottom == rangeToAdd.Bottom)
    {
      if (curRange.Right == rangeToAdd.Left - 1)
      {
        curRange.Width = rangeToAdd.Right - curRange.Left;
        flag2 = true;
      }
      else if (rangeToAdd.Right == curRange.Left - 1)
      {
        curRange.Width = curRange.Left - rangeToAdd.Right + curRange.Width + rangeToAdd.Width;
        curRange.X = rangeToAdd.X;
        flag2 = true;
      }
      else if (curRange.Left == rangeToAdd.Right + 1)
      {
        curRange.X = rangeToAdd.Left;
        flag2 = true;
      }
    }
    return flag2;
  }

  public void Remove(Rectangle[] arrRanges)
  {
    if (arrRanges == null)
      return;
    int length = arrRanges.Length;
    for (int index = 0; index < length; ++index)
      this.Remove(arrRanges[index]);
  }

  public RangesOperations Clone()
  {
    RangesOperations rangesOperations = (RangesOperations) this.MemberwiseClone();
    rangesOperations.m_arrCells = new List<Rectangle>();
    int index = 0;
    for (int count = this.m_arrCells.Count; index < count; ++index)
      rangesOperations.m_arrCells.Add(this.m_arrCells[index]);
    return rangesOperations;
  }

  private int Remove(Rectangle rect)
  {
    List<Rectangle> cellList = this.CellList;
    int count1 = 0;
    int count2 = cellList.Count;
    List<Rectangle> arrCells = (List<Rectangle>) null;
    int num = count2;
    for (int index1 = 0; index1 < num; ++index1)
    {
      Rectangle rectangle1 = cellList[index1];
      if (UtilityMethods.Intersects(rectangle1, rect))
      {
        int index2 = num - 1;
        Rectangle rectangle2 = cellList[index2];
        cellList[index2] = cellList[index1];
        cellList[index1] = rectangle2;
        ++count1;
        --num;
        --index1;
        IList<Rectangle> collection = this.SplitRectangle(rectangle1, rect);
        if (collection != null)
        {
          if (arrCells == null)
            arrCells = new List<Rectangle>((IEnumerable<Rectangle>) collection);
          else
            arrCells.AddRange((IEnumerable<Rectangle>) collection);
        }
      }
    }
    if (count1 > 0)
      cellList.RemoveRange(count2 - count1, count1);
    if (arrCells != null)
      this.AddRectangles((IList<Rectangle>) arrCells);
    return num;
  }

  private IList<Rectangle> SplitRectangle(Rectangle rectSource, Rectangle rectRemove)
  {
    if (!UtilityMethods.Intersects(rectRemove, rectSource))
      return (IList<Rectangle>) null;
    rectRemove.Intersect(rectSource);
    List<Rectangle> rectangleList = new List<Rectangle>(4);
    if (rectSource.Top < rectRemove.Top)
    {
      Rectangle rectangle = Rectangle.FromLTRB(rectSource.Left, rectSource.Top, rectSource.Right, rectRemove.Top - 1);
      rectangleList.Add(rectangle);
    }
    if (rectSource.Bottom > rectRemove.Bottom)
    {
      Rectangle rectangle = Rectangle.FromLTRB(rectSource.Left, rectRemove.Bottom + 1, rectSource.Right, rectSource.Bottom);
      rectangleList.Add(rectangle);
    }
    if (rectSource.Left < rectRemove.Left)
    {
      Rectangle rectangle = Rectangle.FromLTRB(rectSource.Left, rectRemove.Top, rectRemove.Left - 1, rectRemove.Bottom);
      rectangleList.Add(rectangle);
    }
    if (rectSource.Right > rectRemove.Right)
    {
      Rectangle rectangle = Rectangle.FromLTRB(rectRemove.Right + 1, rectRemove.Top, rectSource.Right, rectRemove.Bottom);
      rectangleList.Add(rectangle);
    }
    return (IList<Rectangle>) rectangleList;
  }

  public void OptimizeStorage()
  {
    this.SortAndTryAdd(new RangesOperations.SortKeyGetter(this.TopValueGetter), new RangesOperations.SortKeyGetter(this.LeftValueGetter), new RangesOperations.CombineRectangles(this.CombineSameRowRectangles));
    this.SortAndTryAdd(new RangesOperations.SortKeyGetter(this.LeftValueGetter), new RangesOperations.SortKeyGetter(this.TopValueGetter), new RangesOperations.CombineRectangles(this.CombineSameColumnRectangles));
  }

  private void SortAndTryAdd(
    RangesOperations.SortKeyGetter topLevelKeyGetter,
    RangesOperations.SortKeyGetter lowLevelKeyGetter,
    RangesOperations.CombineRectangles combine)
  {
    if (this.m_arrCells.Count <= 1)
      return;
    int count = this.m_arrCells.Count;
    int num;
    do
    {
      num = count;
      SortedDictionary<int, SortedList<int, Rectangle>> dictionary = this.SortBy(topLevelKeyGetter, lowLevelKeyGetter);
      this.Clear();
      this.OptimizeAndAdd(dictionary, combine);
      count = this.m_arrCells.Count;
    }
    while (num != count);
  }

  private void OptimizeAndAdd(
    SortedDictionary<int, SortedList<int, Rectangle>> dictionary,
    RangesOperations.CombineRectangles combine)
  {
    foreach (int key in dictionary.Keys)
    {
      IList<Rectangle> values = dictionary[key].Values;
      IList<Rectangle> rectangleList = combine(values);
      int index = 0;
      for (int count = rectangleList.Count; index < count; ++index)
        this.AddRange(rectangleList[index]);
    }
  }

  private int TopValueGetter(Rectangle rect) => rect.Top;

  private int LeftValueGetter(Rectangle rect) => rect.Left;

  private SortedDictionary<int, SortedList<int, Rectangle>> SortBy(
    RangesOperations.SortKeyGetter keyGetter,
    RangesOperations.SortKeyGetter secondLevelKeyGetter)
  {
    SortedDictionary<int, SortedList<int, Rectangle>> sortedDictionary = new SortedDictionary<int, SortedList<int, Rectangle>>();
    int index = 0;
    for (int count = this.m_arrCells.Count; index < count; ++index)
    {
      Rectangle arrCell = this.m_arrCells[index];
      int key = keyGetter(arrCell);
      SortedList<int, Rectangle> sortedList;
      if (!sortedDictionary.TryGetValue(key, out sortedList))
      {
        sortedList = new SortedList<int, Rectangle>();
        sortedDictionary.Add(key, sortedList);
      }
      if (!sortedList.ContainsKey(secondLevelKeyGetter(arrCell)))
        sortedList.Add(secondLevelKeyGetter(arrCell), arrCell);
    }
    return sortedDictionary;
  }

  private IList<Rectangle> CombineSameRowRectangles(IList<Rectangle> lstRects)
  {
    if (lstRects == null || lstRects.Count == 0)
      return lstRects;
    List<Rectangle> rectangleList = new List<Rectangle>();
    rectangleList.Add(lstRects[0]);
    int index1 = 1;
    for (int count = lstRects.Count; index1 < count; ++index1)
    {
      int index2 = rectangleList.Count - 1;
      Rectangle rectangle = rectangleList[index2];
      Rectangle lstRect = lstRects[index1];
      if (rectangle.Top == lstRect.Top && rectangle.Bottom == lstRect.Bottom && rectangle.Right + 1 == lstRect.Left)
      {
        rectangle = Rectangle.FromLTRB(rectangle.Left, rectangle.Top, lstRect.Right, rectangle.Bottom);
        rectangleList[index2] = rectangle;
      }
      else
        rectangleList.Add(lstRect);
    }
    return (IList<Rectangle>) rectangleList;
  }

  private IList<Rectangle> CombineSameColumnRectangles(IList<Rectangle> lstRects)
  {
    if (lstRects == null || lstRects.Count == 0)
      return lstRects;
    List<Rectangle> rectangleList = new List<Rectangle>();
    rectangleList.Add(lstRects[0]);
    int index1 = 1;
    for (int count = lstRects.Count; index1 < count; ++index1)
    {
      int index2 = rectangleList.Count - 1;
      Rectangle rectangle = rectangleList[index2];
      Rectangle lstRect = lstRects[index1];
      if (rectangle.Left == lstRect.Left && rectangle.Right == lstRect.Right && rectangle.Bottom + 1 == lstRect.Top)
      {
        rectangle = Rectangle.FromLTRB(rectangle.Left, rectangle.Top, lstRect.Right, lstRect.Bottom);
        rectangleList[index2] = rectangle;
      }
      else
        rectangleList.Add(lstRect);
    }
    return (IList<Rectangle>) rectangleList;
  }

  internal void Offset(int iRowDelta, int iColumnDelta, WorkbookImpl book)
  {
    int index = 0;
    for (int count = this.m_arrCells.Count; index < count; ++index)
    {
      Rectangle arrCell = this.m_arrCells[index];
      arrCell.Offset(iColumnDelta, iRowDelta);
      if (arrCell.Y >= book.MaxRowCount)
        arrCell.Y = book.MaxRowCount - 1;
      if (arrCell.Bottom >= book.MaxRowCount)
        arrCell.Height -= arrCell.Bottom - book.MaxRowCount + 1;
      if (arrCell.X >= book.MaxColumnCount)
        arrCell.X = book.MaxColumnCount - 1;
      if (arrCell.Right >= book.MaxColumnCount)
        arrCell.Width -= arrCell.Right - book.MaxColumnCount + 1;
      this.m_arrCells[index] = arrCell;
    }
  }

  public void SetLength(int maxLength)
  {
    if (this.m_arrCells.Count <= maxLength)
      return;
    this.m_arrCells.RemoveRange(maxLength, this.m_arrCells.Count - maxLength);
  }

  private delegate int SortKeyGetter(Rectangle rect);

  private delegate IList<Rectangle> CombineRectangles(IList<Rectangle> lstRects);
}
