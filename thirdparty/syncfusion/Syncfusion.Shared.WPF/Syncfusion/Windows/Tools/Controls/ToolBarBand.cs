// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.ToolBarBand
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections.Generic;
using System.Windows;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

public class ToolBarBand
{
  private bool isWindowResizing = true;

  public ToolBarBand() => this.ToolBars = new List<ToolBarAdv>();

  public double Size { get; set; }

  public int BandNo { get; set; }

  internal bool IsWindowResizing
  {
    get => this.isWindowResizing;
    set => this.isWindowResizing = value;
  }

  internal List<ToolBarAdv> ToolBars { get; set; }

  public Rect BoundingRectangle { get; internal set; }

  public void Insert(ToolBarAdv toolBar)
  {
    toolBar.ToolBarBand = this;
    this.ToolBars.Add(toolBar);
  }

  public void InsertAt(int pos, ToolBarAdv bar)
  {
    bar.ToolBarBand = this;
    if (this.ToolBars.Count >= pos)
      this.ToolBars.Insert(pos, bar);
    else
      this.ToolBars.Add(bar);
  }

  public void Remove(ToolBarAdv bar)
  {
    if (!this.ToolBars.Contains(bar))
      return;
    this.ToolBars.Remove(bar);
  }

  public void CorrectOrder()
  {
    this.ToolBars.Sort(new Comparison<ToolBarAdv>(this.CompareBandIndex));
    this.CorrectBandIndexes();
  }

  private void CorrectBandIndexes()
  {
    int num = 0;
    foreach (ToolBarAdv toolBar in this.ToolBars)
    {
      toolBar.BandIndex = num;
      ++num;
    }
  }

  public int GetPosition(double xPos)
  {
    int position = -1;
    foreach (ToolBarAdv toolBar in this.ToolBars)
    {
      if (xPos < OrientedValue.GetOrientedRightValue(toolBar.BoundingRectangle, toolBar.Orientation))
      {
        position = this.ToolBars.IndexOf(toolBar) + 1;
        break;
      }
    }
    if (position == -1)
      position = this.ToolBars.Count;
    return position;
  }

  private int CompareBandIndex(ToolBarAdv bar1, ToolBarAdv bar2)
  {
    if (bar1.BandIndex > bar2.BandIndex)
      return 1;
    return bar1.BandIndex == bar2.BandIndex ? 0 : -1;
  }

  internal System.Windows.Size ArrangeToolBars(double x, double y)
  {
    double num1 = 0.0;
    double num2 = this.ToolBars[0].Orientation == System.Windows.Controls.Orientation.Horizontal ? x : y;
    this.Size = this.FindMaxSize();
    foreach (ToolBarAdv toolBar in this.ToolBars)
    {
      if (toolBar.Visibility == Visibility.Visible || toolBar.Visibility == Visibility.Hidden)
      {
        double width = toolBar.Orientation == System.Windows.Controls.Orientation.Horizontal ? toolBar.GetDesiredSize().Width : this.Size;
        double height = toolBar.Orientation == System.Windows.Controls.Orientation.Horizontal ? this.Size : toolBar.GetDesiredSize().Height;
        double x1 = toolBar.Orientation == System.Windows.Controls.Orientation.Horizontal ? num2 : x;
        double y1 = toolBar.Orientation == System.Windows.Controls.Orientation.Horizontal ? y : num2;
        toolBar.BoundingRectangle = new Rect(x1, y1, width, height);
        toolBar.isArranged = false;
        toolBar.Arrange(toolBar.BoundingRectangle);
        if (!toolBar.isArranged)
          toolBar.Arrange(new System.Windows.Size(width, height));
        num1 += OrientedValue.GetOrientedWidthValue(toolBar.GetDesiredSize(), toolBar.Orientation);
        num2 += OrientedValue.GetOrientedWidthValue(toolBar.GetDesiredSize(), toolBar.Orientation);
      }
    }
    double width1 = this.ToolBars[0].Orientation == System.Windows.Controls.Orientation.Horizontal ? num1 : this.Size;
    double height1 = this.ToolBars[0].Orientation == System.Windows.Controls.Orientation.Horizontal ? this.Size : num1;
    this.BoundingRectangle = new Rect(x, y, width1, height1);
    return new System.Windows.Size(width1, height1);
  }

  internal System.Windows.Size Measure(System.Windows.Size availableSize)
  {
    return this.Measure(availableSize, this.ToolBars.Count - 1);
  }

  internal System.Windows.Size Measure(System.Windows.Size availableSize, int index)
  {
    bool flag = true;
    double num1 = 0.0;
    int pointerIndex = index;
    for (int index1 = 0; index1 <= index; ++index1)
      this.ToolBars[index1].ClearTempItems();
    System.Windows.Size size;
    while (flag)
    {
      flag = false;
      for (int index2 = index; index2 >= 0; --index2)
      {
        if (this.ToolBars[index2].Visibility != Visibility.Collapsed)
        {
          if (this.ToolBars[index2].CanToolStripItemsMoveToOverflow)
          {
            size = availableSize;
            if (index2 == pointerIndex)
            {
              double val2 = OrientedValue.GetOrientedWidthValue(availableSize, this.ToolBars[index2].Orientation) - this.GetSizeExceptPointerElement(pointerIndex, this.IsWindowResizing);
              size = new System.Windows.Size(this.ToolBars[index2].Orientation == System.Windows.Controls.Orientation.Horizontal ? Math.Max(0.0, val2) : size.Width, this.ToolBars[index2].Orientation == System.Windows.Controls.Orientation.Horizontal ? size.Height : Math.Max(0.0, val2));
            }
            this.ToolBars[index2].Resize(size);
            double num2 = num1 + OrientedValue.GetOrientedWidthValue(this.ToolBars[index2].GetDesiredSize(), this.ToolBars[index2].Orientation);
            flag = (((flag | this.ToolBars[index2].CanToolStripItemsMoveToOverflow) & num2 > OrientedValue.GetOrientedWidthValue(availableSize, this.ToolBars[index2].Orientation) ? 1 : 0) & (index2 == 0 ? 0 : (!this.IsWindowResizing ? 1 : 0))) != 0;
          }
          else if (index2 == 0 && !this.IsWindowResizing)
          {
            flag = false;
          }
          else
          {
            double num3 = num1 + OrientedValue.GetOrientedWidthValue(this.ToolBars[index2].GetDesiredSize(), this.ToolBars[index2].Orientation);
          }
          if (!this.ToolBars[index2].CanToolStripItemsMoveToOverflow && index2 == pointerIndex)
            --pointerIndex;
        }
        num1 = 0.0;
      }
    }
    double num4 = this.FindMaxSize();
    size = new System.Windows.Size();
    foreach (ToolBarAdv toolBar in this.ToolBars)
    {
      size.Width += toolBar.Orientation == System.Windows.Controls.Orientation.Horizontal ? toolBar.RequiredSize.Width : num4;
      size.Height += toolBar.Orientation == System.Windows.Controls.Orientation.Horizontal ? num4 : toolBar.RequiredSize.Height;
      num4 = 0.0;
    }
    this.Size = OrientedValue.GetOrientedHeightValue(size, this.ToolBars[0].Orientation);
    return size;
  }

  private double GetSizeExceptPointerElement(int pointerIndex, bool flag)
  {
    double exceptPointerElement = 0.0;
    int num = flag ? this.ToolBars.Count : pointerIndex;
    for (int index = 0; index < num; ++index)
    {
      if (index != pointerIndex)
        exceptPointerElement += OrientedValue.GetOrientedWidthValue(this.ToolBars[index].GetDesiredSize(), this.ToolBars[index].Orientation);
    }
    return exceptPointerElement;
  }

  private double FindMaxSize()
  {
    double val2 = 0.0;
    foreach (ToolBarAdv toolBar in this.ToolBars)
      val2 = Math.Max(OrientedValue.GetOrientedHeightValue(toolBar.DesiredSize, toolBar.Orientation), val2);
    return val2;
  }

  internal static int CompareBand(ToolBarBand bar1, ToolBarBand bar2)
  {
    if (bar1.BandNo > bar2.BandNo)
      return 1;
    return bar1.BandNo == bar2.BandNo ? 0 : -1;
  }
}
