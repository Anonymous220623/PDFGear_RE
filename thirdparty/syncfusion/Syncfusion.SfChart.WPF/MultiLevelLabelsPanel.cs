// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.MultiLevelLabelsPanel
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class MultiLevelLabelsPanel
{
  private Panel labelsPanel;
  private UIElementsRecycler<TextBlock> textBlockRecycler;
  private UIElementsRecycler<Border> borderRecycler;
  private UIElementsRecycler<Polyline> polylineRecycler;
  private List<Size> desiredSizes;
  private Size desiredSize;
  private IEnumerable<IGrouping<int, ChartMultiLevelLabel>> groupedLabels;
  private Thickness margin = new Thickness().GetThickness(2.0, 2.0, 2.0, 2.0);
  private double borderPadding = 10.0;
  private double top;
  private double left;
  private double startValue;
  private double endValue;
  private double start;
  private double end;
  private bool isOpposed;
  private int currentBorderPos;
  private int currentBracePos;
  private ChartMultiLevelLabel currentLabel;
  private double height;
  private double width;
  private double txtBlockLeft;
  private double txtBlockTop;
  private PointCollection pointCollection1;
  private PointCollection pointCollection2;
  private int currentRow;

  public MultiLevelLabelsPanel(Panel panel)
  {
    this.labelsPanel = panel;
    this.textBlockRecycler = new UIElementsRecycler<TextBlock>(panel);
    this.polylineRecycler = new UIElementsRecycler<Polyline>(panel);
    this.borderRecycler = new UIElementsRecycler<Border>(panel);
  }

  internal Size DesiredSize => this.desiredSize;

  internal Panel Panel => this.labelsPanel;

  internal ChartAxisBase2D Axis { get; set; }

  internal void Dispose()
  {
    this.Axis = (ChartAxisBase2D) null;
    if (this.labelsPanel == null)
      return;
    this.labelsPanel.Children.Clear();
  }

  internal void DetachElements()
  {
    this.labelsPanel = (Panel) null;
    if (this.textBlockRecycler != null)
      this.textBlockRecycler.Clear();
    if (this.borderRecycler != null)
      this.borderRecycler.Clear();
    if (this.polylineRecycler == null)
      return;
    this.polylineRecycler.Clear();
  }

  internal Size Measure(Size availableSize)
  {
    this.desiredSizes = new List<Size>();
    this.groupedLabels = this.Axis.MultiLevelLabels.OrderBy<ChartMultiLevelLabel, int>((Func<ChartMultiLevelLabel, int>) (label => label.Level)).GroupBy<ChartMultiLevelLabel, int>((Func<ChartMultiLevelLabel, int>) (label => label.Level));
    foreach (TextBlock textBlock in this.textBlockRecycler)
    {
      if (textBlock.Width > 0.0)
      {
        textBlock.ClearValue(FrameworkElement.WidthProperty);
        textBlock.UpdateLayout();
      }
      textBlock.Measure(availableSize);
      this.desiredSizes.Add(textBlock.DesiredSize);
      if (this.Axis.Orientation == Orientation.Vertical)
      {
        int num = this.Axis.OpposedPosition ? 90 : -90;
        textBlock.RenderTransform = (Transform) new RotateTransform()
        {
          Angle = (double) num
        };
      }
      else
        textBlock.RenderTransform = (Transform) null;
    }
    this.CalculateActualPlotOffset();
    this.desiredSize = this.CalculateDesiredSize(availableSize);
    return this.desiredSize;
  }

  internal void UpdateElements()
  {
    if (this.Axis != null && this.Axis.MultiLevelLabels != null && this.Axis.MultiLevelLabels.Count > 0)
    {
      this.GenerateContainer(this.Axis.MultiLevelLabels.Count);
    }
    else
    {
      if (this.textBlockRecycler.Count > 0)
        this.textBlockRecycler.Clear();
      if (this.borderRecycler.Count > 0)
        this.borderRecycler.Clear();
      if (this.polylineRecycler.Count <= 0)
        return;
      this.polylineRecycler.Clear();
    }
  }

  internal Size Arrange(Size finalSize)
  {
    this.isOpposed = this.IsOpposed();
    double labelBorderWidth = this.Axis.LabelBorderWidth;
    if (this.Axis.Orientation == Orientation.Horizontal)
    {
      double y = !this.Axis.ShowLabelBorder ? -labelBorderWidth : -this.margin.Top;
      Rect rect = new Rect(this.Axis.GetActualPlotOffsetStart() - labelBorderWidth, y, finalSize.Width + 2.0 * labelBorderWidth - this.Axis.GetActualPlotOffset(), finalSize.Height + this.margin.Top + this.margin.Bottom);
      rect.Height = !this.Axis.ShowLabelBorder ? rect.Height + this.Axis.LabelBorderWidth : rect.Height;
      this.labelsPanel.Clip = (Geometry) new RectangleGeometry()
      {
        Rect = rect
      };
      this.ArrangeHorizontalLabels(finalSize);
    }
    else
    {
      Rect rect = new Rect(!this.Axis.ShowLabelBorder ? -labelBorderWidth : -this.margin.Left, this.Axis.GetActualPlotOffsetEnd() - labelBorderWidth, finalSize.Width + this.margin.Left + this.margin.Right, finalSize.Height + 2.0 * labelBorderWidth - this.Axis.GetActualPlotOffset());
      rect.Width = this.Axis.ShowLabelBorder ? rect.Width : rect.Width + this.Axis.LabelBorderWidth;
      this.labelsPanel.Clip = (Geometry) new RectangleGeometry()
      {
        Rect = rect
      };
      this.ArrangeVerticalLables(finalSize);
    }
    return finalSize;
  }

  private void ArrangeVerticalLables(Size finalSize)
  {
    int num = 0;
    int row = 0;
    this.left = this.isOpposed ? this.width - this.margin.Right : finalSize.Width - (this.width - (this.margin.Right + this.margin.Left));
    foreach (IGrouping<int, ChartMultiLevelLabel> groupedLabel in this.groupedLabels)
    {
      foreach (ChartMultiLevelLabel label in this.Axis.IsInversed ? groupedLabel.Reverse<ChartMultiLevelLabel>() : (IEnumerable<ChartMultiLevelLabel>) groupedLabel)
      {
        this.currentLabel = label;
        if (this.SetValues(label))
        {
          this.CheckLabelRange();
        }
        else
        {
          this.SetTextBlockPosition();
          this.SetBorderStyle(row, false);
          ++num;
        }
      }
      if (this.isOpposed)
        this.left += this.width;
      else
        this.left -= this.width;
      num = 0;
      ++row;
    }
    this.ResetLabelValues();
  }

  private double CalculateActualLeft() => this.isOpposed ? this.left - this.width : this.left;

  private double CalculateActualTop() => this.isOpposed ? this.top - this.height : this.top;

  private void ResetLabelValues()
  {
    this.currentBorderPos = 0;
    this.currentBracePos = 0;
    this.currentLabel = (ChartMultiLevelLabel) null;
    this.currentRow = 0;
    if (this.Axis.Orientation == Orientation.Horizontal)
    {
      this.height = 0.0;
      this.top = 0.0;
    }
    else
    {
      this.width = 0.0;
      this.left = 0.0;
    }
  }

  private bool SetValues(ChartMultiLevelLabel label)
  {
    this.startValue = this.CalculatePosition(label.Start);
    this.endValue = this.CalculatePosition(label.End);
    if (this.startValue > this.endValue || this.startValue == this.endValue)
      return true;
    if (this.Axis.Orientation == Orientation.Horizontal)
    {
      this.start = Math.Round(this.Axis.ValueToCoefficient(this.startValue) * this.Axis.RenderedRect.Width) + this.Axis.GetActualPlotOffsetStart();
      this.end = Math.Round(this.Axis.ValueToCoefficient(this.endValue) * this.Axis.RenderedRect.Width) + this.Axis.GetActualPlotOffsetStart();
    }
    else
    {
      this.start = (1.0 - this.Axis.ValueToCoefficientCalc(this.startValue)) * this.Axis.RenderedRect.Height + this.Axis.GetActualPlotOffsetEnd();
      this.end = (1.0 - this.Axis.ValueToCoefficientCalc(this.endValue)) * this.Axis.RenderedRect.Height + this.Axis.GetActualPlotOffsetEnd();
    }
    return false;
  }

  private void SetVerticalLabelRectangle(int row)
  {
    double labelBorderWidth = this.Axis.LabelBorderWidth;
    this.borderRecycler[this.currentBorderPos].BorderThickness = row != 0 || !this.Axis.ShowLabelBorder ? new Thickness().GetThickness(labelBorderWidth, labelBorderWidth, labelBorderWidth, labelBorderWidth) : (this.isOpposed ? new Thickness().GetThickness(0.0, labelBorderWidth, labelBorderWidth, labelBorderWidth) : new Thickness().GetThickness(labelBorderWidth, labelBorderWidth, 0.0, labelBorderWidth));
    this.SetBorderPosition();
  }

  private void SetVerticalBorderWithoutTopAndBottom()
  {
    double labelBorderWidth = this.Axis.LabelBorderWidth;
    this.borderRecycler[this.currentBorderPos].BorderThickness = new Thickness().GetThickness(0.0, labelBorderWidth, 0.0, labelBorderWidth);
    this.SetBorderPosition();
  }

  private void SetBorderPosition()
  {
    Border border = this.borderRecycler[this.currentBorderPos];
    if (this.Axis.MultiLevelLabelsBorderType == BorderType.None)
      border.BorderThickness = new Thickness().GetThickness(0.0, 0.0, 0.0, 0.0);
    if (this.Axis.Orientation == Orientation.Horizontal)
      this.SetHorizontalBorder(border);
    else
      this.SetVerticalBorder(border);
  }

  private void SetHorizontalBorder(Border border)
  {
    double length1;
    double length2;
    if (this.Axis.MultiLevelLabelsBorderType != BorderType.None)
    {
      if (this.currentRow == 0 && this.Axis.ShowLabelBorder)
        border.Height = this.height;
      else
        border.Height = this.height + this.Axis.LabelBorderWidth;
      border.Width = (this.Axis.IsInversed ? this.start - this.end : this.end - this.start) + this.Axis.LabelBorderWidth;
      length1 = (this.Axis.IsInversed ? this.end : this.start) - this.Axis.LabelBorderWidth / 2.0;
      length2 = (this.currentRow != 0 || this.isOpposed) && !this.isOpposed || !this.Axis.ShowLabelBorder ? this.CalculateActualTop() - this.Axis.LabelBorderWidth : this.CalculateActualTop();
    }
    else
    {
      border.Height = this.height;
      border.Width = this.Axis.IsInversed ? this.start - this.end : this.end - this.start;
      length1 = this.Axis.IsInversed ? this.end : this.start;
      length2 = this.CalculateActualTop();
    }
    Canvas.SetLeft((UIElement) border, length1);
    Canvas.SetTop((UIElement) border, length2);
  }

  private void SetVerticalBorder(Border border)
  {
    double length1;
    double length2;
    if (this.Axis.MultiLevelLabelsBorderType != BorderType.None)
    {
      border.Width = this.width + this.Axis.LabelBorderWidth;
      border.Height = (this.Axis.IsInversed ? this.end - this.start : this.start - this.end) + this.Axis.LabelBorderWidth;
      length1 = this.Axis.ShowLabelBorder ? (this.isOpposed ? this.CalculateActualLeft() - this.Axis.LabelBorderWidth : this.CalculateActualLeft()) : (this.isOpposed ? this.CalculateActualLeft() + this.margin.Right : this.CalculateActualLeft() - this.margin.Left - this.Axis.LabelBorderWidth);
      length2 = (this.Axis.IsInversed ? this.start : this.end) - this.Axis.LabelBorderWidth / 2.0;
    }
    else
    {
      border.Width = this.width;
      border.Height = this.Axis.IsInversed ? this.end - this.start : this.start - this.end;
      length1 = this.CalculateActualLeft();
      length2 = this.Axis.IsInversed ? this.start : this.end;
    }
    Canvas.SetLeft((UIElement) border, length1);
    Canvas.SetTop((UIElement) border, length2);
  }

  private void SetVerticalLabelAlignment(TextBlock txtBlock)
  {
    this.CalculateTextBlockPosition(txtBlock);
    double num = !this.Axis.OpposedPosition ? this.CalculateActualLeft() + this.borderPadding / 2.0 : (this.Axis.GetLabelPosition() == AxisElementPosition.Outside ? this.left - this.borderPadding + this.Axis.LabelBorderWidth / 2.0 : this.left + this.width - this.borderPadding + this.Axis.LabelBorderWidth / 2.0);
    double length = this.Axis.ShowLabelBorder ? (this.Axis.MultiLevelLabelsBorderType != BorderType.Brace ? (this.isOpposed ? num - this.Axis.LabelBorderWidth / 2.0 : num + this.Axis.LabelBorderWidth / 2.0) : (this.isOpposed ? num - this.Axis.LabelBorderWidth / 2.0 : num)) : (this.isOpposed ? num + this.Axis.LabelBorderWidth / 2.0 : num - this.Axis.LabelBorderWidth / 2.0);
    Canvas.SetLeft((UIElement) txtBlock, length);
    Canvas.SetTop((UIElement) txtBlock, this.txtBlockTop);
  }

  private void CalculateTextBlockPosition(TextBlock txtBlock)
  {
    switch (this.currentLabel.LabelAlignment)
    {
      case LabelAlignment.Center:
        double midValue = this.startValue + (this.endValue - this.startValue) / 2.0;
        this.txtBlockTop = this.Axis.OpposedPosition ? this.CalculateMidValue(midValue) - txtBlock.Width / 2.0 : this.CalculateMidValue(midValue) + txtBlock.Width / 2.0;
        break;
      case LabelAlignment.Far:
        double num = !this.Axis.OpposedPosition ? (this.Axis.IsInversed ? this.start : this.end) + (this.borderPadding - this.margin.Right + this.Axis.LabelBorderWidth) : (this.Axis.IsInversed ? this.end : this.start) - (this.borderPadding - this.margin.Right + this.Axis.LabelBorderWidth);
        this.txtBlockTop = this.Axis.OpposedPosition ? num - txtBlock.Width : num + txtBlock.Width;
        break;
      case LabelAlignment.Near:
        if (this.Axis.OpposedPosition)
        {
          this.txtBlockTop = (this.Axis.IsInversed ? this.start : this.end) + (this.margin.Left + this.margin.Right + this.Axis.LabelBorderWidth);
          break;
        }
        this.txtBlockTop = (this.Axis.IsInversed ? this.end : this.start) - (this.margin.Left + this.margin.Right + this.Axis.LabelBorderWidth);
        break;
    }
  }

  private double CalculateMidValue(double midValue)
  {
    return (1.0 - this.Axis.ValueToCoefficientCalc(midValue)) * this.Axis.RenderedRect.Height + this.Axis.GetActualPlotOffsetEnd();
  }

  private void ArrangeHorizontalLabels(Size finalSize)
  {
    int num = 0;
    int row = 0;
    this.top = this.isOpposed ? finalSize.Height : 0.0;
    foreach (IGrouping<int, ChartMultiLevelLabel> groupedLabel in this.groupedLabels)
    {
      foreach (ChartMultiLevelLabel chartMultiLevelLabel in this.Axis.IsInversed ? groupedLabel.Reverse<ChartMultiLevelLabel>() : (IEnumerable<ChartMultiLevelLabel>) groupedLabel)
      {
        this.currentLabel = chartMultiLevelLabel;
        if (this.SetValues(this.currentLabel))
        {
          this.CheckLabelRange();
        }
        else
        {
          this.SetTextBlockPosition();
          this.SetBorderStyle(row, true);
          ++num;
        }
      }
      if (this.isOpposed)
        this.top -= this.height;
      else
        this.top += this.height;
      num = 0;
      ++row;
      this.currentRow = row;
    }
    this.ResetLabelValues();
  }

  private void CheckLabelRange()
  {
    if (this.Axis.MultiLevelLabelsBorderType == BorderType.Brace)
    {
      this.polylineRecycler.Remove(this.polylineRecycler[this.currentBracePos]);
      this.polylineRecycler.Remove(this.polylineRecycler[this.currentBracePos + 1]);
    }
    else
      this.borderRecycler.Remove(this.borderRecycler[this.currentBorderPos]);
    this.textBlockRecycler.Remove(this.textBlockRecycler.Where<TextBlock>((Func<TextBlock, bool>) (txtBlock => txtBlock.Tag as ChartMultiLevelLabel == this.currentLabel)).First<TextBlock>());
  }

  private void SetBorderStyle(int row, bool isHorizontalAxis)
  {
    if (this.Axis.MultiLevelLabelsBorderType == BorderType.Brace)
    {
      if (isHorizontalAxis)
        this.SetHorizontalBrace();
      else
        this.SetVerticalBrace();
      this.currentBracePos += 2;
    }
    else
    {
      if (this.Axis.MultiLevelLabelsBorderType == BorderType.None)
        this.SetBorderPosition();
      else if (this.Axis.MultiLevelLabelsBorderType == BorderType.Rectangle)
      {
        if (isHorizontalAxis)
          this.SetHorizontalLabelRectangle(row);
        else
          this.SetVerticalLabelRectangle(row);
      }
      else if (isHorizontalAxis)
        this.SetHorizontalBorderWithoutTopandBottom();
      else
        this.SetVerticalBorderWithoutTopAndBottom();
      ++this.currentBorderPos;
    }
  }

  private void SetHorizontalLabelRectangle(int row)
  {
    Border border = this.borderRecycler[this.currentBorderPos];
    double labelBorderWidth = this.Axis.LabelBorderWidth;
    border.BorderThickness = row != 0 || !this.Axis.ShowLabelBorder ? new Thickness().GetThickness(labelBorderWidth, labelBorderWidth, labelBorderWidth, labelBorderWidth) : (this.isOpposed ? new Thickness().GetThickness(labelBorderWidth, labelBorderWidth, labelBorderWidth, 0.0) : new Thickness().GetThickness(labelBorderWidth, 0.0, labelBorderWidth, labelBorderWidth));
    this.SetBorderPosition();
  }

  private void SetHorizontalBorderWithoutTopandBottom()
  {
    Border border = this.borderRecycler[this.currentBorderPos];
    double labelBorderWidth = this.Axis.LabelBorderWidth;
    border.BorderThickness = new Thickness(labelBorderWidth, 0.0, labelBorderWidth, 0.0);
    this.SetBorderPosition();
  }

  private void SetHorizontalBrace()
  {
    Polyline polyline1 = this.polylineRecycler[this.currentBracePos];
    Polyline polyline2 = this.polylineRecycler[this.currentBracePos + 1];
    double num1 = 0.0;
    double num2 = 0.0;
    TextBlock textBlock = this.textBlockRecycler.Where<TextBlock>((Func<TextBlock, bool>) (child => child.Tag as ChartMultiLevelLabel == this.currentLabel)).FirstOrDefault<TextBlock>();
    if (textBlock != null)
    {
      double num3 = textBlock.ActualHeight > 0.0 ? textBlock.ActualHeight / 2.0 : textBlock.DesiredSize.Height / 2.0;
      num1 = this.isOpposed ? num3 - this.margin.Top : num3 + this.margin.Top;
      num2 = textBlock.Width;
    }
    double x1 = this.Axis.IsInversed ? this.end : this.start;
    double x2 = this.Axis.IsInversed ? this.start : this.end;
    double x3 = this.txtBlockLeft + num2 + this.margin.Right;
    double y1 = this.top;
    if (!this.Axis.ShowLabelBorder || this.currentRow != 0)
      y1 = this.isOpposed ? y1 - (this.margin.Top + this.margin.Bottom) : y1 + (this.margin.Top + this.margin.Bottom);
    double y2 = this.CalculateActualTop() + this.borderPadding + num1;
    this.pointCollection1 = new PointCollection();
    this.pointCollection2 = new PointCollection();
    this.SetHorizontalBracePoints(x1, x2, x3, y1, y2);
    polyline1.Points = this.pointCollection1;
    polyline2.Points = this.pointCollection2;
    this.txtBlockLeft = 0.0;
  }

  private void SetHorizontalBracePoints(double x1, double x2, double x3, double y1, double y2)
  {
    this.pointCollection1.Add(new Point(x1, y1));
    this.pointCollection1.Add(new Point(x1, y2));
    this.pointCollection1.Add(new Point(this.txtBlockLeft - this.margin.Left, y2));
    this.pointCollection2.Add(new Point(x2, y1));
    this.pointCollection2.Add(new Point(x2, y2));
    this.pointCollection2.Add(new Point(x3, y2));
  }

  private void SetVerticalBrace()
  {
    Polyline polyline1 = this.polylineRecycler[this.currentBracePos];
    Polyline polyline2 = this.polylineRecycler[this.currentBracePos + 1];
    double num1 = 0.0;
    double num2 = 0.0;
    TextBlock textBlock = this.textBlockRecycler.Where<TextBlock>((Func<TextBlock, bool>) (child => child.Tag as ChartMultiLevelLabel == this.currentLabel)).FirstOrDefault<TextBlock>();
    double actualLeft = this.CalculateActualLeft();
    if (textBlock != null)
    {
      num1 = textBlock.ActualHeight > 0.0 ? textBlock.ActualHeight / 2.0 : textBlock.DesiredSize.Height / 2.0;
      num2 = textBlock.Width + this.Axis.LabelBorderWidth / 2.0;
    }
    double x1 = this.isOpposed ? actualLeft : actualLeft + this.width + this.Axis.LabelBorderWidth;
    if (!this.Axis.ShowLabelBorder || this.currentRow != 0)
      x1 = this.isOpposed ? x1 + (this.margin.Left + this.margin.Right) : x1 - (this.margin.Left + this.margin.Right);
    double x2 = actualLeft + this.borderPadding / 2.0 + num1;
    double y1 = this.Axis.IsInversed ? this.start : this.end;
    double y2 = this.Axis.IsInversed ? this.end : this.start;
    double y3 = (this.Axis.OpposedPosition ? this.txtBlockTop : this.txtBlockTop - num2) - this.margin.Left;
    double y4 = (this.Axis.OpposedPosition ? this.txtBlockTop + num2 : this.txtBlockTop) + this.margin.Left;
    this.pointCollection1 = new PointCollection();
    this.pointCollection2 = new PointCollection();
    this.SetVerticalBracePoints(x1, x2, y1, y2, y3, y4);
    polyline1.Points = this.pointCollection1;
    polyline2.Points = this.pointCollection2;
    this.txtBlockTop = 0.0;
  }

  private void SetVerticalBracePoints(
    double x1,
    double x2,
    double y1,
    double y2,
    double y3,
    double y4)
  {
    this.pointCollection1.Add(new Point(x1, y1));
    this.pointCollection1.Add(new Point(x2, y1));
    this.pointCollection1.Add(new Point(x2, y3));
    this.pointCollection2.Add(new Point(x1, y2));
    this.pointCollection2.Add(new Point(x2, y2));
    this.pointCollection2.Add(new Point(x2, y4));
  }

  private void SetTextBlockPosition()
  {
    TextBlock textBlock = this.textBlockRecycler.Where<TextBlock>((Func<TextBlock, bool>) (child => child.Tag as ChartMultiLevelLabel == this.currentLabel)).FirstOrDefault<TextBlock>();
    if (textBlock == null)
      return;
    double num = this.Axis.Orientation != Orientation.Vertical ? (this.Axis.IsInversed ? this.start - this.end : this.end - this.start) - (this.margin.Left + this.margin.Right) : (this.Axis.IsInversed ? this.end - this.start : this.start - this.end) - (this.margin.Left + this.margin.Right);
    textBlock.Width = textBlock.ActualWidth < textBlock.DesiredSize.Width ? textBlock.DesiredSize.Width : textBlock.ActualWidth;
    if (textBlock.Width > num)
    {
      textBlock.Width = num;
      textBlock.TextTrimming = TextTrimming.CharacterEllipsis;
      textBlock.TextWrapping = TextWrapping.NoWrap;
      ToolTipService.SetToolTip((DependencyObject) textBlock, (object) this.currentLabel.Text);
    }
    else
      ToolTipService.SetToolTip((DependencyObject) textBlock, (object) null);
    if (this.Axis.Orientation == Orientation.Horizontal)
      this.SetHorizontalLabelAlignment(textBlock);
    else
      this.SetVerticalLabelAlignment(textBlock);
  }

  private void SetHorizontalLabelAlignment(TextBlock txtBlock)
  {
    switch (this.currentLabel.LabelAlignment)
    {
      case LabelAlignment.Center:
        double num = this.startValue + (this.endValue - this.startValue) / 2.0;
        this.txtBlockLeft = this.Axis.GetActualPlotOffsetStart() + (this.Axis.ValueToCoefficientCalc(num) * this.Axis.RenderedRect.Width - txtBlock.Width / 2.0);
        break;
      case LabelAlignment.Far:
        this.txtBlockLeft = (this.Axis.IsInversed ? this.start : this.end) - txtBlock.Width;
        this.txtBlockLeft -= this.margin.Left + this.margin.Right + this.Axis.LabelBorderWidth;
        break;
      case LabelAlignment.Near:
        this.txtBlockLeft = this.Axis.IsInversed ? this.end : this.start;
        this.txtBlockLeft += this.margin.Left + this.margin.Right + this.Axis.LabelBorderWidth;
        break;
    }
    this.SetTextBlockPosition(txtBlock);
  }

  private void SetTextBlockPosition(TextBlock txtBlock)
  {
    Canvas.SetLeft((UIElement) txtBlock, this.txtBlockLeft);
    double num = this.isOpposed ? this.borderPadding - this.margin.Top - this.margin.Bottom : this.borderPadding + this.margin.Top;
    if (this.Axis.MultiLevelLabelsBorderType == BorderType.Rectangle)
    {
      if (this.Axis.ShowLabelBorder)
      {
        if (this.isOpposed)
          Canvas.SetTop((UIElement) txtBlock, this.CalculateActualTop() + (this.borderPadding / 2.0 + this.margin.Top) + this.Axis.LabelBorderWidth / 2.0);
        else
          Canvas.SetTop((UIElement) txtBlock, this.CalculateActualTop() + (this.borderPadding / 2.0 + this.margin.Top) - this.Axis.LabelBorderWidth / 2.0);
      }
      else
        Canvas.SetTop((UIElement) txtBlock, this.CalculateActualTop() + (this.borderPadding / 2.0 + this.margin.Top) - this.Axis.LabelBorderWidth / 2.0);
    }
    else if (this.Axis.MultiLevelLabelsBorderType == BorderType.Brace)
      Canvas.SetTop((UIElement) txtBlock, this.CalculateActualTop() + num);
    else
      Canvas.SetTop((UIElement) txtBlock, this.CalculateActualTop() + (this.borderPadding / 2.0 + this.margin.Top));
  }

  private double CalculatePosition(object data)
  {
    if (this.Axis is NumericalAxis)
      return Convert.ToDouble(data);
    if (this.Axis is DateTimeAxis)
    {
      switch (data)
      {
        case DateTime dateTime:
          return dateTime.ToOADate();
        case string _:
          return Convert.ToDateTime(data.ToString()).ToOADate();
        default:
          return Convert.ToDouble(data);
      }
    }
    else if (this.Axis is TimeSpanAxis)
    {
      switch (data)
      {
        case TimeSpan timeSpan:
          return timeSpan.TotalMilliseconds;
        case string _:
          return TimeSpan.Parse(data.ToString()).TotalMilliseconds;
        default:
          return Convert.ToDouble(data);
      }
    }
    else
    {
      if (!(this.Axis is LogarithmicAxis))
        return Convert.ToDouble(data);
      data = Convert.ToDouble(data) > 0.0 ? data : (object) 1;
      return Math.Log(Convert.ToDouble(data), (this.Axis as LogarithmicAxis).LogarithmicBase);
    }
  }

  private bool IsOpposed()
  {
    if (this.Axis.OpposedPosition && this.Axis.GetLabelPosition() == AxisElementPosition.Outside)
      return true;
    return !this.Axis.OpposedPosition && this.Axis.GetLabelPosition() == AxisElementPosition.Inside;
  }

  private Size CalculateDesiredSize(Size availableSize)
  {
    double val1 = this.textBlockRecycler.Max<TextBlock>((Func<TextBlock, double>) (txtBlock => txtBlock.DesiredSize.Height)) + (this.borderPadding + this.margin.Top + this.margin.Bottom);
    double num = Math.Max(val1, this.Axis.LabelExtent) * (double) this.groupedLabels.Count<IGrouping<int, ChartMultiLevelLabel>>();
    if (!this.Axis.ShowLabelBorder && this.Axis.MultiLevelLabelsBorderType != BorderType.None)
      num += this.Axis.LabelBorderWidth;
    if (this.Axis.Orientation == Orientation.Horizontal)
    {
      this.height = val1;
      this.desiredSize = new Size(availableSize.Width, num);
    }
    else
    {
      this.width = val1;
      this.desiredSize = new Size(num, availableSize.Height);
    }
    return this.desiredSize;
  }

  private void CalculateActualPlotOffset()
  {
    this.Axis.ActualPlotOffset = this.Axis.PlotOffset < 0.0 ? 0.0 : this.Axis.PlotOffset;
    this.Axis.ActualPlotOffsetStart = this.Axis.PlotOffsetStart < 0.0 ? 0.0 : this.Axis.PlotOffsetStart;
    this.Axis.ActualPlotOffsetEnd = this.Axis.PlotOffsetEnd < 0.0 ? 0.0 : this.Axis.PlotOffsetEnd;
  }

  private void GenerateContainer(int labelsCount)
  {
    Binding binding = new Binding();
    binding.Source = (object) this.Axis;
    binding.Path = new PropertyPath("Visibility", new object[0]);
    if (!this.textBlockRecycler.BindingProvider.Keys.Contains<DependencyProperty>(UIElement.VisibilityProperty))
      this.textBlockRecycler.BindingProvider.Add(UIElement.VisibilityProperty, binding);
    this.textBlockRecycler.GenerateElements(labelsCount);
    if (this.Axis.MultiLevelLabelsBorderType != BorderType.Brace)
    {
      if (this.polylineRecycler != null && this.polylineRecycler.Count > 0)
        this.polylineRecycler.Clear();
      if (!this.borderRecycler.BindingProvider.Keys.Contains<DependencyProperty>(UIElement.VisibilityProperty))
        this.borderRecycler.BindingProvider.Add(UIElement.VisibilityProperty, binding);
      this.borderRecycler.GenerateElements(labelsCount);
    }
    else
    {
      if (this.borderRecycler != null && this.borderRecycler.Count > 0)
        this.borderRecycler.Clear();
      if (!this.polylineRecycler.BindingProvider.Keys.Contains<DependencyProperty>(UIElement.VisibilityProperty))
        this.polylineRecycler.BindingProvider.Add(UIElement.VisibilityProperty, binding);
      this.polylineRecycler.GenerateElements(2 * labelsCount);
    }
    this.SetLabelProperty();
  }

  private void SetLabelProperty()
  {
    int index1 = 0;
    int index2 = 0;
    int index3 = 0;
    foreach (ChartMultiLevelLabel multiLevelLabel in (Collection<ChartMultiLevelLabel>) this.Axis.MultiLevelLabels)
    {
      this.textBlockRecycler[index1].TextTrimming = TextTrimming.None;
      if (this.Axis.MultiLevelLabelsBorderType == BorderType.Brace)
      {
        multiLevelLabel.SetBraceVisualBinding(this.textBlockRecycler[index1], (Shape) this.polylineRecycler[index3], (Shape) this.polylineRecycler[index3 + 1], this.Axis);
        index3 += 2;
      }
      else
      {
        multiLevelLabel.SetVisualBinding(this.textBlockRecycler[index1], this.borderRecycler[index2], this.Axis);
        ++index2;
      }
      ++index1;
    }
  }
}
