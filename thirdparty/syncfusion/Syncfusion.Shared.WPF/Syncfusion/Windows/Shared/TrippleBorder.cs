// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.TrippleBorder
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class TrippleBorder : Decorator
{
  private const int CornerRadiusOffset = 2;
  private Rect m_rectOutside;
  private Rect m_rectBorder;
  private Rect m_rectInside;
  private Pen m_outsidePen;
  private Pen m_borderPen;
  private Pen m_insidePen;
  private UIElement m_adornerChild;
  public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof (CornerRadius), typeof (CornerRadius), typeof (TrippleBorder), (PropertyMetadata) new FrameworkPropertyMetadata((object) new CornerRadius(0.0), FrameworkPropertyMetadataOptions.AffectsMeasure));
  public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register(nameof (BorderThickness), typeof (double), typeof (TrippleBorder), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.0, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(TrippleBorder.OnBorderThicknessChanged)));
  public static readonly DependencyProperty InsideBorderBrushProperty = DependencyProperty.Register(nameof (InsideBorderBrush), typeof (Brush), typeof (TrippleBorder), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(TrippleBorder.OnInsideBorderBrushChanged)));
  public static readonly DependencyProperty InsideBorderThicknessProperty = DependencyProperty.Register(nameof (InsideBorderThickness), typeof (double), typeof (TrippleBorder), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.0, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(TrippleBorder.OnInsideBorderThicknessChanged)));
  public static readonly DependencyProperty OutsideBorderBrushProperty = DependencyProperty.Register(nameof (OutsideBorderBrush), typeof (Brush), typeof (TrippleBorder), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(TrippleBorder.OnOutsideBorderBrushChanged)));
  public static readonly DependencyProperty OutsideBorderThicknessProperty = DependencyProperty.Register(nameof (OutsideBorderThickness), typeof (double), typeof (TrippleBorder), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.0, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(TrippleBorder.OnOutsideBorderThicknessChanged)));

  public UIElement AdornerChild
  {
    get => this.m_adornerChild;
    set
    {
      if (this.m_adornerChild == value)
        return;
      if (this.m_adornerChild != null)
      {
        this.RemoveVisualChild((Visual) this.m_adornerChild);
        this.RemoveLogicalChild((object) this.m_adornerChild);
      }
      this.m_adornerChild = value;
      if (this.m_adornerChild != null)
      {
        this.AddLogicalChild((object) value);
        this.AddVisualChild((Visual) value);
      }
      this.InvalidateMeasure();
    }
  }

  public CornerRadius CornerRadius
  {
    get => (CornerRadius) this.GetValue(TrippleBorder.CornerRadiusProperty);
    set => this.SetValue(TrippleBorder.CornerRadiusProperty, (object) value);
  }

  public Brush BorderBrush
  {
    get => (Brush) this.GetValue(Border.BorderBrushProperty);
    set => this.SetValue(Border.BorderBrushProperty, (object) value);
  }

  public double BorderThickness
  {
    get => (double) this.GetValue(TrippleBorder.BorderThicknessProperty);
    set => this.SetValue(TrippleBorder.BorderThicknessProperty, (object) value);
  }

  public Brush InsideBorderBrush
  {
    get => (Brush) this.GetValue(TrippleBorder.InsideBorderBrushProperty);
    set => this.SetValue(TrippleBorder.InsideBorderBrushProperty, (object) value);
  }

  public double InsideBorderThickness
  {
    get => (double) this.GetValue(TrippleBorder.InsideBorderThicknessProperty);
    set => this.SetValue(TrippleBorder.InsideBorderThicknessProperty, (object) value);
  }

  public Brush OutsideBorderBrush
  {
    get => (Brush) this.GetValue(TrippleBorder.OutsideBorderBrushProperty);
    set => this.SetValue(TrippleBorder.OutsideBorderBrushProperty, (object) value);
  }

  public double OutsideBorderThickness
  {
    get => (double) this.GetValue(TrippleBorder.OutsideBorderThicknessProperty);
    set => this.SetValue(TrippleBorder.OutsideBorderThicknessProperty, (object) value);
  }

  public Brush Background
  {
    get => (Brush) this.GetValue(Border.BackgroundProperty);
    set => this.SetValue(Border.BackgroundProperty, (object) value);
  }

  static TrippleBorder()
  {
    Border.BorderBrushProperty.AddOwner(typeof (TrippleBorder), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(TrippleBorder.OnBorderBrushChanged)));
    Border.BackgroundProperty.AddOwner(typeof (TrippleBorder), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender));
  }

  public TrippleBorder()
  {
    this.m_outsidePen = new Pen(this.OutsideBorderBrush, this.OutsideBorderThickness);
    this.m_borderPen = new Pen(this.BorderBrush, this.BorderThickness);
    this.m_insidePen = new Pen(this.InsideBorderBrush, this.InsideBorderThickness);
  }

  protected override Size ArrangeOverride(Size arrangeSize)
  {
    double outsideBorderThickness = this.OutsideBorderThickness;
    double borderThickness = this.BorderThickness;
    double insideBorderThickness = this.InsideBorderThickness;
    double val2 = 2.0 * (outsideBorderThickness + insideBorderThickness + borderThickness);
    arrangeSize.Width = Math.Max(arrangeSize.Width, val2);
    arrangeSize.Height = Math.Max(arrangeSize.Height, val2);
    double num1 = 0.5 * outsideBorderThickness;
    Point location1 = new Point(num1, num1);
    Size size1 = new Size(arrangeSize.Width - outsideBorderThickness, arrangeSize.Height - outsideBorderThickness);
    this.m_rectOutside = new Rect(location1, size1);
    double num2 = outsideBorderThickness + 0.5 * borderThickness;
    Point location2 = new Point(num2, num2);
    Size size2 = new Size(size1.Width - outsideBorderThickness - borderThickness, size1.Height - outsideBorderThickness - borderThickness);
    this.m_rectBorder = new Rect(location2, size2);
    double num3 = outsideBorderThickness + borderThickness + 0.5 * insideBorderThickness;
    Point location3 = new Point(num3, num3);
    Size size3 = new Size(size2.Width - borderThickness - insideBorderThickness, size2.Height - borderThickness - insideBorderThickness);
    this.m_rectInside = new Rect(location3, size3);
    double num4 = outsideBorderThickness + borderThickness + insideBorderThickness;
    Rect finalRect = new Rect(new Point(num4, num4), new Size(size3.Width - insideBorderThickness, size3.Height - insideBorderThickness));
    this.Child?.Arrange(finalRect);
    if (this.m_adornerChild != null)
      this.m_adornerChild.Arrange(finalRect);
    return arrangeSize;
  }

  protected override void OnRender(DrawingContext drawingContext)
  {
    double topLeft = this.CornerRadius.TopLeft;
    double bottomLeft = this.CornerRadius.BottomLeft;
    int num = topLeft <= 0.0 || bottomLeft <= 0.0 ? 0 : 2;
    if (this.m_outsidePen.Thickness > 0.0)
      drawingContext.DrawRoundedRectangle((Brush) null, this.m_outsidePen, this.m_rectOutside, topLeft, bottomLeft);
    if (this.m_borderPen.Thickness > 0.0)
      drawingContext.DrawRoundedRectangle((Brush) null, this.m_borderPen, this.m_rectBorder, topLeft - (double) num, bottomLeft - (double) num);
    if (this.m_insidePen.Thickness <= 0.0 && this.Background == null)
      return;
    drawingContext.DrawRectangle(this.Background, this.m_insidePen, this.m_rectInside);
  }

  protected override Size MeasureOverride(Size constraint)
  {
    UIElement child = this.Child;
    Size availableSize = new Size(0.0, 0.0);
    child?.Measure(availableSize);
    if (this.m_adornerChild != null)
      this.m_adornerChild.Measure(availableSize);
    return availableSize;
  }

  protected override Visual GetVisualChild(int index)
  {
    switch (index)
    {
      case 0:
        return (Visual) this.Child;
      case 1:
        return (Visual) this.m_adornerChild;
      default:
        throw new ArgumentOutOfRangeException(nameof (index), (object) index, "Index can be 0 or 1.");
    }
  }

  protected override IEnumerator LogicalChildren
  {
    get
    {
      if (this.Child != null)
        yield return (object) this.Child;
      if (this.m_adornerChild != null)
        yield return (object) this.m_adornerChild;
    }
  }

  protected override int VisualChildrenCount
  {
    get
    {
      int visualChildrenCount = 0;
      if (this.Child != null)
        ++visualChildrenCount;
      if (this.m_adornerChild != null)
        ++visualChildrenCount;
      return visualChildrenCount;
    }
  }

  private static void OnBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    TrippleBorder trippleBorder = (TrippleBorder) d;
    trippleBorder.ChangePen(ref trippleBorder.m_borderPen, trippleBorder.BorderBrush, trippleBorder.BorderThickness);
    trippleBorder.OnBorderBrushChanged(e);
  }

  private static void OnBorderThicknessChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    TrippleBorder trippleBorder = (TrippleBorder) d;
    trippleBorder.ChangePen(ref trippleBorder.m_borderPen, trippleBorder.BorderBrush, trippleBorder.BorderThickness);
    trippleBorder.OnBorderThicknessChanged(e);
  }

  private static void OnInsideBorderBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    TrippleBorder trippleBorder = (TrippleBorder) d;
    trippleBorder.ChangePen(ref trippleBorder.m_insidePen, trippleBorder.InsideBorderBrush, trippleBorder.InsideBorderThickness);
    trippleBorder.OnBorderBrushChanged(e);
  }

  private static void OnInsideBorderThicknessChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    TrippleBorder trippleBorder = (TrippleBorder) d;
    trippleBorder.ChangePen(ref trippleBorder.m_insidePen, trippleBorder.InsideBorderBrush, trippleBorder.InsideBorderThickness);
    trippleBorder.OnInsideBorderThicknessChanged(e);
  }

  private static void OnOutsideBorderBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    TrippleBorder trippleBorder = (TrippleBorder) d;
    trippleBorder.ChangePen(ref trippleBorder.m_outsidePen, trippleBorder.OutsideBorderBrush, trippleBorder.OutsideBorderThickness);
    trippleBorder.OnOutsideBorderBrushChanged(e);
  }

  private static void OnOutsideBorderThicknessChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    TrippleBorder trippleBorder = (TrippleBorder) d;
    trippleBorder.ChangePen(ref trippleBorder.m_outsidePen, trippleBorder.OutsideBorderBrush, trippleBorder.OutsideBorderThickness);
    trippleBorder.OnOutsideBorderThicknessChanged(e);
  }

  private void OnBorderBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.BorderBrushChanged == null)
      return;
    this.BorderBrushChanged((DependencyObject) this, e);
  }

  private void OnBorderThicknessChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.BorderThicknessChanged == null)
      return;
    this.BorderThicknessChanged((DependencyObject) this, e);
  }

  private void OnInsideBorderBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.InsideBorderBrushChanged == null)
      return;
    this.InsideBorderBrushChanged((DependencyObject) this, e);
  }

  private void OnInsideBorderThicknessChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.InsideBorderThicknessChanged == null)
      return;
    this.InsideBorderThicknessChanged((DependencyObject) this, e);
  }

  private void OnOutsideBorderBrushChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.OutsideBorderBrushChanged == null)
      return;
    this.OutsideBorderBrushChanged((DependencyObject) this, e);
  }

  private void OnOutsideBorderThicknessChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.OutsideBorderThicknessChanged == null)
      return;
    this.OutsideBorderThicknessChanged((DependencyObject) this, e);
  }

  private void ChangePen(ref Pen pen, Brush brush, double thickness)
  {
    pen.Brush = brush;
    pen.Thickness = thickness;
  }

  public event PropertyChangedCallback BorderBrushChanged;

  public event PropertyChangedCallback BorderThicknessChanged;

  public event PropertyChangedCallback InsideBorderBrushChanged;

  public event PropertyChangedCallback InsideBorderThicknessChanged;

  public event PropertyChangedCallback OutsideBorderBrushChanged;

  public event PropertyChangedCallback OutsideBorderThicknessChanged;
}
