// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.Printing.PrintPreviewPanel
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared.Printing;

public sealed class PrintPreviewPanel : Panel, IScrollInfo
{
  private const double LineSize = 16.0;
  private const double WheelSize = 48.0;
  private double yPosition;
  private double xPosition;
  private int previousPageIndex;
  private PrintManager printManager;
  internal Action<int> SetPageIndex;
  internal Action InValidateParent;
  private readonly Size InfiniteSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
  private Size _Extent;
  private Size _Viewport;
  private Point _Offset;
  private ScrollViewer _ScrollOwner;
  private bool _CanVerticallyScroll;
  private bool _CanHorizontallyScroll;

  public PrintPreviewPanel()
  {
  }

  public PrintPreviewPanel(PrintManager printManager) => this.SetPrintManager(printManager);

  internal PrintPageControl Child { get; set; }

  protected override Size MeasureOverride(Size availableSize)
  {
    if (this.Child == null)
      return availableSize;
    this.Child.Measure(this.InfiniteSize);
    Size desiredSize = this.Child.DesiredSize;
    Size size = new Size()
    {
      Width = double.IsInfinity(availableSize.Width) ? desiredSize.Width : availableSize.Width,
      Height = double.IsInfinity(availableSize.Height) ? desiredSize.Height : availableSize.Height
    };
    if (this.Child.DesiredSize.Height > availableSize.Height)
      this.UpdateScrollInfo(size, new Size(this.Child.DesiredSize.Width, this.Child.DesiredSize.Height * (double) this.printManager.PageCount));
    else
      this.UpdateScrollInfo(size, new Size(this.Child.DesiredSize.Width, availableSize.Height * (double) this.printManager.PageCount));
    return base.MeasureOverride(size);
  }

  protected override Size ArrangeOverride(Size finalSize)
  {
    if (this.Child == null)
      return finalSize;
    double num1 = (finalSize.Width - this.Child.DesiredSize.Width) / 2.0;
    double num2 = this.VerticalOffset - (double) (int) (this.VerticalOffset / this.Child.DesiredSize.Height) * this.Child.DesiredSize.Height;
    this.xPosition = num1 < 0.0 ? -this.HorizontalOffset : num1;
    if (this.Child.DesiredSize.Height > finalSize.Height)
    {
      int num3 = (this.VerticalOffset + finalSize.Height) % this.Child.DesiredSize.Height > 0.0 ? 1 : 0;
      int pageIndex = (int) ((this.VerticalOffset + finalSize.Height) / this.Child.DesiredSize.Height) + num3;
      if (this.previousPageIndex != pageIndex && pageIndex >= 1 && pageIndex <= this.printManager.PageCount)
      {
        this.Child.PageIndex = pageIndex;
        this.printManager.CreatePage(pageIndex, this.Child);
        this.previousPageIndex = pageIndex;
      }
    }
    else
    {
      int pageIndex = (int) Math.Round((this.VerticalOffset + finalSize.Height) / finalSize.Height);
      if (pageIndex > 0 && this.previousPageIndex != pageIndex && pageIndex <= this.printManager.PageCount)
      {
        this.Child.PageIndex = pageIndex;
        this.printManager.CreatePage(pageIndex, this.Child);
        this.previousPageIndex = pageIndex;
      }
      else if (this.Child.PageIndex != 1 && this.VerticalOffset == 0.0)
      {
        this.Child.PageIndex = 1;
        this.printManager.CreatePage(1, this.Child);
        this.previousPageIndex = pageIndex;
      }
    }
    this.yPosition = this.VerticalOffset - (double) (int) (this.VerticalOffset / this.Child.DesiredSize.Height) * this.Child.DesiredSize.Height + finalSize.Height > this.Child.DesiredSize.Height ? (finalSize.Height > this.Child.DesiredSize.Height ? finalSize.Height / 2.0 - this.Child.DesiredSize.Height / 2.0 : 0.0) : -num2;
    this.Child.Arrange(new Rect(this.xPosition, this.yPosition, this.Child.DesiredSize.Width, this.Child.DesiredSize.Height));
    if (this.Child.DesiredSize.Height > finalSize.Height)
      this.UpdateScrollInfo(finalSize, new Size(this.Child.DesiredSize.Width, this.Child.DesiredSize.Height * (double) this.printManager.PageCount));
    else
      this.UpdateScrollInfo(finalSize, new Size(this.Child.DesiredSize.Width, finalSize.Height * (double) this.printManager.PageCount));
    if (this.SetPageIndex != null)
      this.SetPageIndex(this.Child.PageIndex);
    return base.ArrangeOverride(finalSize);
  }

  internal void SetPrintManager(PrintManager printManager)
  {
    if (printManager == null)
      return;
    this.printManager = printManager;
    printManager.InValidatePreviewPanel = new Action(this.InValidate);
    printManager.InitializePrint();
    this.Child = printManager.CreatePage(1);
    this.Children.Add((UIElement) this.Child);
  }

  internal void UpdateScrollInfo(Size viewport, Size extent)
  {
    if (double.IsInfinity(viewport.Width))
      viewport.Width = extent.Width;
    if (double.IsInfinity(viewport.Height))
      viewport.Height = extent.Height;
    this._Extent = extent;
    this._Viewport = viewport;
    this._Offset.X = Math.Max(0.0, Math.Min(this._Offset.X, this.ExtentWidth - this.ViewportWidth));
    this._Offset.Y = Math.Max(0.0, Math.Min(this._Offset.Y, this.ExtentHeight - this.ViewportHeight));
    if (this.ScrollOwner == null)
      return;
    this.ScrollOwner.InvalidateScrollInfo();
  }

  internal void InValidate()
  {
    this.printManager.InitializePrint();
    if (this.printManager.PageCount <= 0)
      return;
    this.Children.Clear();
    if (this.Child == null)
      return;
    this.Child = this.printManager.CreatePage(1, this.Child);
    this.Children.Add((UIElement) this.Child);
    if (this.InValidateParent == null)
      return;
    this.InValidateParent();
  }

  public bool CanHorizontallyScroll
  {
    get => this._CanHorizontallyScroll;
    set => this._CanHorizontallyScroll = value;
  }

  public bool CanVerticallyScroll
  {
    get => this._CanVerticallyScroll;
    set => this._CanVerticallyScroll = value;
  }

  public double ExtentHeight => this._Extent.Height;

  public double ExtentWidth => this._Extent.Width;

  public double HorizontalOffset => this._Offset.X;

  public double VerticalOffset => this._Offset.Y;

  public double ViewportHeight => this._Viewport.Height;

  public double ViewportWidth => this._Viewport.Width;

  public ScrollViewer ScrollOwner
  {
    get => this._ScrollOwner;
    set => this._ScrollOwner = value;
  }

  public void LineDown() => this.SetVerticalOffset(this.VerticalOffset + 16.0);

  public void LineUp() => this.SetVerticalOffset(this.VerticalOffset - 16.0);

  public void LineLeft() => this.SetHorizontalOffset(this.HorizontalOffset - 16.0);

  public void LineRight() => this.SetHorizontalOffset(this.HorizontalOffset + 16.0);

  public void MouseWheelDown() => this.SetVerticalOffset(this.VerticalOffset + 48.0);

  public void MouseWheelUp() => this.SetVerticalOffset(this.VerticalOffset - 48.0);

  public void MouseWheelLeft() => this.SetHorizontalOffset(this.HorizontalOffset - 48.0);

  public void MouseWheelRight() => this.SetHorizontalOffset(this.HorizontalOffset + 48.0);

  public void PageDown() => this.SetVerticalOffset(this.VerticalOffset + this.ViewportHeight);

  public void PageUp() => this.SetVerticalOffset(this.VerticalOffset - this.ViewportHeight);

  public void PageLeft() => this.SetHorizontalOffset(this.HorizontalOffset - this.ViewportWidth);

  public void PageRight() => this.SetHorizontalOffset(this.HorizontalOffset + this.ViewportWidth);

  public Rect MakeVisible(Visual visual, Rect rectangle) => rectangle;

  public void SetHorizontalOffset(double offset)
  {
    offset = Math.Max(0.0, Math.Min(offset, this.ExtentWidth - this.ViewportWidth));
    if (offset == this._Offset.Y)
      return;
    this._Offset.X = offset;
    this.InvalidateArrange();
  }

  public void SetVerticalOffset(double offset)
  {
    offset = Math.Max(0.0, Math.Min(offset, this.ExtentHeight - this.ViewportHeight));
    if (offset == this._Offset.Y)
      return;
    this._Offset.Y = offset;
    this.InvalidateArrange();
  }
}
