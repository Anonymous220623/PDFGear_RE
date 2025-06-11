// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.XySegmentDraggingBase
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class XySegmentDraggingBase : XyDataSeries
{
  public static readonly DependencyProperty DragTooltipStyleProperty = DependencyProperty.Register(nameof (DragTooltipStyle), typeof (ChartDragTooltipStyle), typeof (XySegmentDraggingBase), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty EnableDragTooltipProperty = DependencyProperty.Register(nameof (EnableDragTooltip), typeof (bool), typeof (XySegmentDraggingBase), new PropertyMetadata((object) true));
  public static readonly DependencyProperty DragTooltipTemplateProperty = DependencyProperty.Register(nameof (DragTooltipTemplate), typeof (DataTemplate), typeof (XySegmentDraggingBase), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty RoundToDecimalProperty = DependencyProperty.Register(nameof (RoundToDecimal), typeof (int), typeof (XySegmentDraggingBase), new PropertyMetadata((object) 0));
  public static readonly DependencyProperty SnapToPointProperty = DependencyProperty.Register(nameof (SnapToPoint), typeof (SnapToPoint), typeof (XySegmentDraggingBase), new PropertyMetadata((object) SnapToPoint.None));
  public static readonly DependencyProperty EnableSegmentDraggingProperty = DependencyProperty.Register(nameof (EnableSegmentDragging), typeof (bool), typeof (XySegmentDraggingBase), new PropertyMetadata((object) false));
  public static readonly DependencyProperty UpdateSourceProperty = DependencyProperty.Register(nameof (UpdateSource), typeof (bool), typeof (XySegmentDraggingBase), new PropertyMetadata((object) false));
  public static readonly DependencyProperty DragCancelKeyModifiersProperty = DependencyProperty.Register(nameof (DragCancelKeyModifiers), typeof (ModifierKeys), typeof (XySegmentDraggingBase), new PropertyMetadata((object) ModifierKeys.None));
  internal double prevDraggedXValue;
  internal double prevDraggedValue;
  internal double delta;
  internal double DeltaX;
  private double tooltipX;
  private double tooltipY;
  private Storyboard ellipseAnimation;
  private DataTemplate oppRightTootip;
  private DataTemplate normalTooltip;
  private DataTemplate oppLeftTooltip;

  public event EventHandler<XySegmentEnterEventArgs> SegmentEnter;

  public event EventHandler<ChartDragStartEventArgs> DragStart;

  public event EventHandler<Syncfusion.UI.Xaml.Charts.DragDelta> DragDelta;

  public event EventHandler<ChartDragEndEventArgs> DragEnd;

  public event EventHandler<XyPreviewEndEventArgs> PreviewDragEnd;

  public ChartDragTooltipStyle DragTooltipStyle
  {
    get => (ChartDragTooltipStyle) this.GetValue(XySegmentDraggingBase.DragTooltipStyleProperty);
    set => this.SetValue(XySegmentDraggingBase.DragTooltipStyleProperty, (object) value);
  }

  public bool EnableDragTooltip
  {
    get => (bool) this.GetValue(XySegmentDraggingBase.EnableDragTooltipProperty);
    set => this.SetValue(XySegmentDraggingBase.EnableDragTooltipProperty, (object) value);
  }

  public DataTemplate DragTooltipTemplate
  {
    get => (DataTemplate) this.GetValue(XySegmentDraggingBase.DragTooltipTemplateProperty);
    set => this.SetValue(XySegmentDraggingBase.DragTooltipTemplateProperty, (object) value);
  }

  public int RoundToDecimal
  {
    get => (int) this.GetValue(XySegmentDraggingBase.RoundToDecimalProperty);
    set => this.SetValue(XySegmentDraggingBase.RoundToDecimalProperty, (object) value);
  }

  public SnapToPoint SnapToPoint
  {
    get => (SnapToPoint) this.GetValue(XySegmentDraggingBase.SnapToPointProperty);
    set => this.SetValue(XySegmentDraggingBase.SnapToPointProperty, (object) value);
  }

  public bool EnableSegmentDragging
  {
    get => (bool) this.GetValue(XySegmentDraggingBase.EnableSegmentDraggingProperty);
    set => this.SetValue(XySegmentDraggingBase.EnableSegmentDraggingProperty, (object) value);
  }

  public bool UpdateSource
  {
    get => (bool) this.GetValue(XySegmentDraggingBase.UpdateSourceProperty);
    set => this.SetValue(XySegmentDraggingBase.UpdateSourceProperty, (object) value);
  }

  public ModifierKeys DragCancelKeyModifiers
  {
    get => (ModifierKeys) this.GetValue(XySegmentDraggingBase.DragCancelKeyModifiersProperty);
    set => this.SetValue(XySegmentDraggingBase.DragCancelKeyModifiersProperty, (object) value);
  }

  internal FrameworkElement AnimationElement { get; set; }

  protected int SegmentIndex { get; set; }

  protected ContentControl DragSpliter { get; set; }

  protected double DraggedXValue { get; set; }

  protected double DraggedValue { get; set; }

  protected ContentControl Tooltip { get; set; }

  protected ChartDragPointinfo DragInfo { get; set; }

  protected Storyboard EllipseAnimation
  {
    get => this.ellipseAnimation;
    set => this.ellipseAnimation = value;
  }

  internal static ChartSegment GetDraggingSegment(object element)
  {
    if (!(element is FrameworkElement frameworkElement))
      return (ChartSegment) null;
    if (frameworkElement.Tag is ChartSegment tag)
      return tag;
    return frameworkElement.DataContext is ChartAdornment ? (ChartSegment) null : frameworkElement.DataContext as ChartSegment;
  }

  internal virtual void UpdateDragSpliter(
    FrameworkElement rect,
    ChartSegment indexSegment,
    string position)
  {
    int index = this.Segments.IndexOf(indexSegment);
    XySegmentEnterEventArgs args = new XySegmentEnterEventArgs()
    {
      XValue = this.GetActualXValue(index),
      SegmentIndex = index,
      CanDrag = true,
      YValue = (object) this.YValues[index]
    };
    this.RaiseDragEnter(args);
    if (!args.CanDrag)
      return;
    if (this.DragSpliter == null)
    {
      this.DragSpliter = new ContentControl();
      this.SeriesPanel.Children.Add((UIElement) this.DragSpliter);
      if (position == "Left" || position == "Right")
        this.DragSpliter.Template = ChartDictionaries.GenericCommonDictionary[(object) "DragSpliterLeft"] as ControlTemplate;
      else
        this.DragSpliter.Template = ChartDictionaries.GenericCommonDictionary[(object) "DragSpliterTop"] as ControlTemplate;
    }
    double num1 = 0.0;
    double num2 = 0.0;
    double num3 = 0.0;
    double num4 = 0.0;
    switch (position)
    {
      case "Top":
        double top = Canvas.GetTop((UIElement) rect);
        double width1 = rect.Width;
        double num5 = width1 / 3.0;
        num4 = width1 - num5 * 2.0;
        this.DragSpliter.Margin = new Thickness().GetThickness(num5, 0.0, num5, 0.0);
        num1 = Canvas.GetLeft((UIElement) rect);
        num2 = top + 7.0;
        num3 = num4 / 5.0;
        break;
      case "Bottom":
        double num6 = Canvas.GetTop((UIElement) rect) + rect.Height;
        double width2 = rect.Width;
        double num7 = width2 / 3.0;
        num4 = width2 - num7 * 2.0;
        this.DragSpliter.Margin = new Thickness().GetThickness(num7, 0.0, num7, 0.0);
        num1 = Canvas.GetLeft((UIElement) rect);
        num2 = num6 - 7.0;
        num3 = num4 / 5.0;
        break;
      case "Right":
        double num8 = Canvas.GetLeft((UIElement) rect) + rect.Width;
        double height1 = rect.Height;
        double num9 = height1 / 3.0;
        num3 = height1 - num9 * 2.0;
        this.DragSpliter.Margin = new Thickness().GetThickness(0.0, num9, 0.0, num9);
        num2 = Canvas.GetTop((UIElement) rect);
        num1 = num8 - 20.0;
        num4 = num3 / 5.0;
        break;
      case "Left":
        double left = Canvas.GetLeft((UIElement) rect);
        double height2 = rect.Height;
        double num10 = height2 / 3.0;
        num3 = height2 - num10 * 2.0;
        this.DragSpliter.Margin = new Thickness().GetThickness(0.0, num10, 0.0, num10);
        num2 = Canvas.GetTop((UIElement) rect);
        num1 = left + 10.0;
        num4 = num3 / 5.0;
        break;
    }
    this.DragSpliter.SetValue(Canvas.LeftProperty, (object) num1);
    this.DragSpliter.SetValue(Canvas.TopProperty, (object) num2);
    this.DragSpliter.Height = num3;
    this.DragSpliter.Width = num4;
  }

  internal virtual void ActivateDragging(Point mousePos, object element)
  {
    Keyboard.Focus((IInputElement) this);
    this.delta = 0.0;
    ChartSegment draggingSegment = XySegmentDraggingBase.GetDraggingSegment(element);
    if (draggingSegment == null)
      return;
    int segmentIndex = this.SegmentIndex;
    this.SegmentIndex = !(this.ActualXAxis is CategoryAxis) || (this.ActualXAxis as CategoryAxis).IsIndexed ? this.Segments.IndexOf(draggingSegment) : this.ActualData.IndexOf(draggingSegment.Item);
    if (segmentIndex != this.SegmentIndex)
      this.prevDraggedValue = 0.0;
    this.KeyDown += new KeyEventHandler(this.CoreWindow_KeyDown);
    ChartDragStartEventArgs args = new ChartDragStartEventArgs()
    {
      BaseXValue = this.GetActualXValue(this.SegmentIndex)
    };
    if (this.EmptyPointIndexes != null)
    {
      foreach (int num in this.EmptyPointIndexes[0])
      {
        if (this.SegmentIndex == num)
        {
          args.EmptyPoint = true;
          break;
        }
      }
    }
    this.RaiseDragStart(args);
    if (args.Cancel)
    {
      this.ResetDraggingElements("Cancel", true);
      this.SegmentIndex = -1;
    }
    this.UnHoldPanning(false);
  }

  internal void CoreWindow_KeyDown(object sender, KeyEventArgs e)
  {
    bool flag = false;
    if (e.Key == Key.Escape && Keyboard.Modifiers == this.DragCancelKeyModifiers)
      flag = true;
    if (!flag)
      return;
    Mouse.OverrideCursor = Cursors.Arrow;
    this.ResetDraggingElements("EscapeKey", true);
  }

  internal void UnHoldPanning(bool value)
  {
    foreach (ChartZoomPanBehavior chartZoomPanBehavior in this.Area.Behaviors.OfType<ChartZoomPanBehavior>())
    {
      chartZoomPanBehavior.InternalEnablePanning = value;
      chartZoomPanBehavior.InternalEnableSelectionZooming = value;
    }
  }

  internal double GetSnapToPoint(double actualValue)
  {
    double snapToPoint = actualValue;
    switch (this.SnapToPoint)
    {
      case SnapToPoint.Round:
        snapToPoint = Math.Round(actualValue, this.RoundToDecimal);
        break;
      case SnapToPoint.Floor:
        snapToPoint = Math.Floor(actualValue);
        break;
      case SnapToPoint.Ceil:
        snapToPoint = Math.Ceiling(actualValue);
        break;
    }
    return snapToPoint;
  }

  internal void UpdateSegmentDragValueToolTip(
    Point pos,
    ChartSegment segment,
    double newXValue,
    double newYValue,
    double offsetX,
    double offsetY)
  {
    if (!this.EnableDragTooltip)
      return;
    ScatterSeries scatterSeries = this as ScatterSeries;
    if (this.Tooltip == null)
    {
      this.DragInfo = (ChartDragPointinfo) new ChartDragSegmentInfo();
      this.Tooltip = new ContentControl()
      {
        Content = (object) this.DragInfo
      };
      this.SeriesPanel.Children.Add((UIElement) this.Tooltip);
      if (this.DragTooltipStyle != null)
      {
        this.DragInfo.FontFamily = this.DragTooltipStyle.FontFamily;
        this.DragInfo.FontSize = this.DragTooltipStyle.FontSize;
        this.DragInfo.FontStyle = this.DragTooltipStyle.FontStyle;
        this.DragInfo.Foreground = this.DragTooltipStyle.Foreground;
      }
      else
      {
        this.DragInfo.FontFamily = TextBlock.FontFamilyProperty.GetMetadata(typeof (TextBlock)).DefaultValue as FontFamily;
        this.DragInfo.FontSize = 20.0;
        this.DragInfo.FontStyle = FontStyles.Normal;
        this.DragInfo.Foreground = (Brush) new SolidColorBrush(Colors.White);
      }
      if (this.DragTooltipTemplate != null)
      {
        this.normalTooltip = this.oppLeftTooltip = this.oppRightTootip = this.DragTooltipTemplate;
      }
      else
      {
        string str = "";
        if (scatterSeries != null && scatterSeries.DragDirection == DragType.XY)
        {
          str = "Xy";
          this.DragInfo.PrefixLabelTemplate = this.ActualYAxis.PrefixLabelTemplate;
          this.DragInfo.PostfixLabelTemplate = this.ActualYAxis.PostfixLabelTemplate;
          this.DragInfo.PrefixLabelTemplateX = this.ActualXAxis.PrefixLabelTemplate;
          this.DragInfo.PostfixLabelTemplateX = this.ActualXAxis.PostfixLabelTemplate;
        }
        else if (scatterSeries != null && scatterSeries.DragDirection == DragType.X)
        {
          this.DragInfo.PrefixLabelTemplate = this.ActualXAxis.PrefixLabelTemplate;
          this.DragInfo.PostfixLabelTemplate = this.ActualXAxis.PostfixLabelTemplate;
        }
        else
        {
          this.DragInfo.PrefixLabelTemplate = this.ActualYAxis.PrefixLabelTemplate;
          this.DragInfo.PostfixLabelTemplate = this.ActualYAxis.PostfixLabelTemplate;
        }
        this.normalTooltip = ChartDictionaries.GenericCommonDictionary[(object) (str + "SegmentDragInfo")] as DataTemplate;
        this.oppLeftTooltip = ChartDictionaries.GenericCommonDictionary[(object) (str + "SegmentDragInfoOppLeft")] as DataTemplate;
        this.oppRightTootip = ChartDictionaries.GenericCommonDictionary[(object) (str + "SegmentDragInfoOppRight")] as DataTemplate;
        if (this.IsActualTransposed)
          this.RightAlignTooltip(pos.X, pos.Y, offsetX);
        else
          this.TopAlignTooltip(pos.X, pos.Y, offsetY);
      }
    }
    this.DragInfo.Brush = this.DragTooltipStyle == null || this.DragTooltipStyle.Background == null ? segment.Interior : this.DragTooltipStyle.Background;
    this.DragInfo.ScreenCoordinates = pos;
    ChartDragSegmentInfo dragInfo = this.DragInfo as ChartDragSegmentInfo;
    if (scatterSeries != null && scatterSeries.DragDirection == DragType.XY)
    {
      dragInfo.NewXValue = this.ActualXAxis.GetLabelContent(newXValue);
      dragInfo.NewValue = (object) newYValue;
    }
    else
      dragInfo.NewValue = scatterSeries == null || scatterSeries.DragDirection != DragType.X ? (object) newYValue : this.ActualXAxis.GetLabelContent(newXValue);
    this.DragInfo.Segment = segment;
    double length1 = 0.0;
    double length2 = 0.0;
    double actualWidth = this.ActualWidth;
    double actualHeight = this.ActualHeight;
    bool flag = this is ColumnSeries || this is BarSeries;
    if (this.IsActualTransposed)
    {
      if (pos.X + offsetX + this.Tooltip.DesiredSize.Width >= actualWidth)
      {
        if (flag)
        {
          this.tooltipY = pos.Y - offsetY - this.Tooltip.DesiredSize.Height;
          Canvas.SetTop((UIElement) this.Tooltip, pos.Y + offsetY);
        }
        else
          this.LeftAlignTooltip(pos.X, pos.Y, offsetX);
      }
      else
        this.RightAlignTooltip(pos.X, pos.Y, offsetX);
    }
    else if (pos.X - this.Tooltip.DesiredSize.Width / 2.0 <= length1)
      this.RightAlignTooltip(pos.X, pos.Y, offsetX);
    else if (pos.X + this.Tooltip.DesiredSize.Width / 2.0 >= actualWidth || pos.Y - offsetY - this.Tooltip.DesiredSize.Height <= length2 || flag && pos.Y >= actualHeight)
      this.LeftAlignTooltip(pos.X, pos.Y, offsetX);
    else
      this.TopAlignTooltip(pos.X, pos.Y, offsetY);
    if (this.tooltipX <= length1)
      Canvas.SetLeft((UIElement) this.Tooltip, length1);
    else if (this.tooltipX + this.Tooltip.DesiredSize.Width >= actualWidth)
      Canvas.SetLeft((UIElement) this.Tooltip, actualWidth - this.Tooltip.DesiredSize.Width);
    else
      Canvas.SetLeft((UIElement) this.Tooltip, this.tooltipX);
    if (this.tooltipY <= length2)
      Canvas.SetTop((UIElement) this.Tooltip, length2);
    else if (this.tooltipY + this.Tooltip.DesiredSize.Height >= actualHeight)
      Canvas.SetTop((UIElement) this.Tooltip, actualHeight - this.Tooltip.DesiredSize.Height);
    else
      Canvas.SetTop((UIElement) this.Tooltip, this.tooltipY);
  }

  internal double GetActualDelta()
  {
    this.delta += this.prevDraggedValue != 0.0 ? this.DraggedValue - this.prevDraggedValue : this.DraggedValue - this.YValues[this.SegmentIndex];
    return this.delta;
  }

  internal object GetActualXDelta(double prevDraggedXValue, double draggedXValue, ref double delta)
  {
    delta += prevDraggedXValue != 0.0 ? draggedXValue - prevDraggedXValue : draggedXValue - ((IList<double>) this.ActualXValues)[this.SegmentIndex];
    return this.XAxisValueType == ChartValueType.DateTime || this.XAxisValueType == ChartValueType.TimeSpan ? (object) TimeSpan.FromMilliseconds(delta) : (object) delta;
  }

  internal object GetDraggedActualXValue(double deltaX)
  {
    object draggedActualXvalue;
    switch (this.XAxisValueType)
    {
      case ChartValueType.DateTime:
        draggedActualXvalue = (object) deltaX.FromOADate();
        break;
      case ChartValueType.TimeSpan:
        draggedActualXvalue = (object) TimeSpan.FromMilliseconds(deltaX);
        break;
      default:
        draggedActualXvalue = (object) deltaX;
        break;
    }
    return draggedActualXvalue;
  }

  internal void EllipseIdealAnimation(UIElement ellipse)
  {
    this.EllipseAnimation = new Storyboard();
    DoubleAnimationUsingKeyFrames animationUsingKeyFrames1 = new DoubleAnimationUsingKeyFrames();
    animationUsingKeyFrames1.RepeatBehavior = RepeatBehavior.Forever;
    DoubleAnimationUsingKeyFrames element1 = animationUsingKeyFrames1;
    Storyboard.SetTargetProperty((DependencyObject) element1, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)", new object[0]));
    Storyboard.SetTarget((DependencyObject) element1, (DependencyObject) ellipse);
    EasingDoubleKeyFrame easingDoubleKeyFrame1 = new EasingDoubleKeyFrame();
    easingDoubleKeyFrame1.Value = 1.0;
    EasingDoubleKeyFrame keyFrame1 = easingDoubleKeyFrame1;
    keyFrame1.KeyTime = keyFrame1.KeyTime.GetKeyTime(new TimeSpan(0L));
    element1.KeyFrames.Add((DoubleKeyFrame) keyFrame1);
    EasingDoubleKeyFrame easingDoubleKeyFrame2 = new EasingDoubleKeyFrame();
    easingDoubleKeyFrame2.Value = 3.6;
    easingDoubleKeyFrame2.EasingFunction = (IEasingFunction) new CircleEase();
    EasingDoubleKeyFrame keyFrame2 = easingDoubleKeyFrame2;
    keyFrame2.KeyTime = keyFrame2.KeyTime.GetKeyTime(new TimeSpan(0, 0, 0, 0, 600));
    element1.KeyFrames.Add((DoubleKeyFrame) keyFrame2);
    this.EllipseAnimation.Children.Add((Timeline) element1);
    DoubleAnimationUsingKeyFrames animationUsingKeyFrames2 = new DoubleAnimationUsingKeyFrames();
    animationUsingKeyFrames2.RepeatBehavior = RepeatBehavior.Forever;
    DoubleAnimationUsingKeyFrames element2 = animationUsingKeyFrames2;
    Storyboard.SetTargetProperty((DependencyObject) element2, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)", new object[0]));
    Storyboard.SetTarget((DependencyObject) element2, (DependencyObject) ellipse);
    EasingDoubleKeyFrame easingDoubleKeyFrame3 = new EasingDoubleKeyFrame();
    easingDoubleKeyFrame3.Value = 1.0;
    EasingDoubleKeyFrame keyFrame3 = easingDoubleKeyFrame3;
    keyFrame3.KeyTime = keyFrame3.KeyTime.GetKeyTime(new TimeSpan(0L));
    element2.KeyFrames.Add((DoubleKeyFrame) keyFrame3);
    EasingDoubleKeyFrame easingDoubleKeyFrame4 = new EasingDoubleKeyFrame();
    easingDoubleKeyFrame4.Value = 3.6;
    easingDoubleKeyFrame4.EasingFunction = (IEasingFunction) new CircleEase();
    EasingDoubleKeyFrame keyFrame4 = easingDoubleKeyFrame4;
    keyFrame4.KeyTime = keyFrame4.KeyTime.GetKeyTime(new TimeSpan(0, 0, 0, 0, 600));
    element2.KeyFrames.Add((DoubleKeyFrame) keyFrame4);
    this.EllipseAnimation.Children.Add((Timeline) element2);
    DoubleAnimationUsingKeyFrames animationUsingKeyFrames3 = new DoubleAnimationUsingKeyFrames();
    animationUsingKeyFrames3.RepeatBehavior = RepeatBehavior.Forever;
    DoubleAnimationUsingKeyFrames element3 = animationUsingKeyFrames3;
    Storyboard.SetTargetProperty((DependencyObject) element3, new PropertyPath("(UIElement.Opacity)", new object[0]));
    Storyboard.SetTarget((DependencyObject) element3, (DependencyObject) ellipse);
    EasingDoubleKeyFrame easingDoubleKeyFrame5 = new EasingDoubleKeyFrame();
    easingDoubleKeyFrame5.Value = 1.0;
    EasingDoubleKeyFrame keyFrame5 = easingDoubleKeyFrame5;
    keyFrame5.KeyTime = keyFrame5.KeyTime.GetKeyTime(new TimeSpan(0L));
    element3.KeyFrames.Add((DoubleKeyFrame) keyFrame5);
    EasingDoubleKeyFrame easingDoubleKeyFrame6 = new EasingDoubleKeyFrame();
    easingDoubleKeyFrame6.Value = 3.6;
    easingDoubleKeyFrame6.EasingFunction = (IEasingFunction) new CircleEase();
    EasingDoubleKeyFrame keyFrame6 = easingDoubleKeyFrame6;
    keyFrame6.KeyTime = keyFrame6.KeyTime.GetKeyTime(new TimeSpan(0, 0, 0, 0, 600));
    element3.KeyFrames.Add((DoubleKeyFrame) keyFrame6);
    this.EllipseAnimation.Children.Add((Timeline) element3);
  }

  internal void AddAnimationEllipse(
    ControlTemplate template,
    double height,
    double width,
    double left,
    double top,
    object bindingObject,
    bool isAdornment)
  {
    if (this.AnimationElement == null)
    {
      ContentControl contentControl = new ContentControl();
      contentControl.Background = this.Segments[this.SegmentIndex == 0 ? 0 : this.SegmentIndex - 1].Interior;
      contentControl.RenderTransform = (Transform) new ScaleTransform();
      contentControl.RenderTransformOrigin = new Point(0.5, 0.5);
      contentControl.DataContext = (object) this;
      contentControl.Template = template;
      this.AnimationElement = (FrameworkElement) contentControl;
      this.EllipseIdealAnimation((UIElement) this.AnimationElement);
      this.SeriesPanel.Children.Add((UIElement) this.AnimationElement);
      this.AnimationElement.Height = height - height / 2.0;
      this.AnimationElement.Width = width - width / 2.0;
      if (this is ScatterSeries)
      {
        this.AnimationElement.IsHitTestVisible = false;
        Binding binding;
        if (isAdornment)
          binding = new Binding()
          {
            Source = (object) this.AdornmentsInfo,
            Path = new PropertyPath("SymbolInterior", new object[0])
          };
        else
          binding = new Binding()
          {
            Source = bindingObject,
            Path = new PropertyPath("Interior", new object[0])
          };
        this.AnimationElement.SetBinding(Control.BackgroundProperty, (BindingBase) binding);
      }
      this.ellipseAnimation.Begin();
    }
    if (this.IsActualTransposed)
    {
      Canvas.SetTop((UIElement) this.AnimationElement, left - this.AnimationElement.Width / 2.0);
      Canvas.SetLeft((UIElement) this.AnimationElement, top - this.AnimationElement.Height / 2.0);
    }
    else
    {
      Canvas.SetLeft((UIElement) this.AnimationElement, left - this.AnimationElement.Width / 2.0);
      Canvas.SetTop((UIElement) this.AnimationElement, top - this.AnimationElement.Height / 2.0);
    }
  }

  internal void AnimateSegmentTemplate(double positionX, double positionY, ChartSegment segment)
  {
    if (!(this is ScatterSeries scatterSeries) || !(scatterSeries.CustomTemplate.LoadContent() is Canvas canvas) || canvas.Children.Count < 1)
      return;
    this.AnimationElement = (FrameworkElement) canvas;
    this.SeriesPanel.Children.Add((UIElement) this.AnimationElement);
    for (int index = 0; index < canvas.Children.Count; ++index)
    {
      if (canvas.Children[index] is FrameworkElement child)
      {
        child.DataContext = (object) segment;
        child.UpdateLayout();
        this.EllipseIdealAnimation((UIElement) child);
        child.RenderTransform = (Transform) new ScaleTransform();
        child.RenderTransformOrigin = new Point(0.5, 0.5);
        child.Height -= child.ActualHeight / 2.0;
        child.Width -= child.ActualWidth / 2.0;
        this.EllipseAnimation.Begin();
        if (this.IsActualTransposed)
        {
          Canvas.SetLeft((UIElement) child, positionY - child.Width / 2.0);
          Canvas.SetTop((UIElement) child, positionX - child.Height / 2.0);
        }
        else
        {
          Canvas.SetLeft((UIElement) child, positionX - child.Width / 2.0);
          Canvas.SetTop((UIElement) child, positionY - child.Height / 2.0);
        }
      }
    }
  }

  internal void AnimateAdornmentSymbolTemplate(double positionX, double positionY)
  {
    if (this.AdornmentsInfo.SymbolTemplate == null)
      return;
    this.AnimationElement = (FrameworkElement) (this.AdornmentsInfo.SymbolTemplate.LoadContent() as Shape);
    if (this.AnimationElement == null)
      return;
    this.SeriesPanel.Children.Add((UIElement) this.AnimationElement);
    this.AnimationElement.IsHitTestVisible = false;
    this.AnimationElement.UpdateLayout();
    Binding binding = new Binding()
    {
      Source = (object) this.Adornments[this.SegmentIndex],
      Path = new PropertyPath("Interior", new object[0])
    };
    BindingOperations.SetBinding((DependencyObject) (this.AnimationElement as Shape), Shape.FillProperty, (BindingBase) binding);
    this.AnimationElement.DataContext = (object) this.Adornments[this.SegmentIndex];
    if (this.AnimationElement == null)
      return;
    this.EllipseIdealAnimation((UIElement) this.AnimationElement);
    this.AnimationElement.RenderTransform = (Transform) new ScaleTransform();
    this.AnimationElement.RenderTransformOrigin = new Point(0.5, 0.5);
    this.AnimationElement.Height -= this.AnimationElement.ActualHeight / 2.0;
    this.AnimationElement.Width -= this.AnimationElement.ActualWidth / 2.0;
    this.EllipseAnimation.Begin();
    if (this.IsActualTransposed)
    {
      Canvas.SetLeft((UIElement) this.AnimationElement, positionY - this.AnimationElement.Width / 2.0);
      Canvas.SetTop((UIElement) this.AnimationElement, positionX - this.AnimationElement.Height / 2.0);
    }
    else
    {
      Canvas.SetLeft((UIElement) this.AnimationElement, positionX - this.AnimationElement.Width / 2.0);
      Canvas.SetTop((UIElement) this.AnimationElement, positionY - this.AnimationElement.Height / 2.0);
    }
  }

  protected virtual void ResetDraggingElements(string reason, bool dragEndEvent)
  {
    this.KeyDown -= new KeyEventHandler(this.CoreWindow_KeyDown);
    this.UnHoldPanning(true);
    if (dragEndEvent)
    {
      if (this is ScatterSeries)
        this.RaiseDragEnd((ChartDragEndEventArgs) new ChartXyDragEndEventArgs());
      else
        this.RaiseDragEnd(new ChartDragEndEventArgs());
    }
    this.ResetSegmentDragTooltipInfo();
  }

  protected virtual void ResetDragSpliter()
  {
    if (this.DragSpliter == null || this.SeriesPanel == null)
      return;
    this.SeriesPanel.Children.Remove((UIElement) this.DragSpliter);
    this.DragSpliter = (ContentControl) null;
  }

  protected virtual void OnChartDragStart(Point mousePos, object originalSource)
  {
  }

  protected virtual void OnChartDragDelta(Point mousePos, object originalSource)
  {
  }

  protected virtual void OnChartDragEnd(Point mousePos, object originalSource)
  {
  }

  protected virtual void OnChartDragEntered(Point mousePos, object originalSource)
  {
  }

  protected virtual void OnChartDragExited(Point mousePos, object originalSource)
  {
    this.ResetDragSpliter();
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    if (this.EnableSegmentDragging)
      this.OnChartDragDelta(e.GetPosition((IInputElement) this.SeriesPanel), e.OriginalSource);
    base.OnMouseMove(e);
  }

  protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    if (!this.EnableSegmentDragging)
      return;
    this.SeriesPanel.CaptureMouse();
    this.OnChartDragStart(e.GetPosition((IInputElement) this.SeriesPanel), e.OriginalSource);
    base.OnMouseLeftButtonDown(e);
  }

  protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    if (this.EnableSegmentDragging)
    {
      this.SeriesPanel.ReleaseMouseCapture();
      this.OnChartDragEnd(e.GetPosition((IInputElement) this.SeriesPanel), e.OriginalSource);
    }
    base.OnMouseLeftButtonUp(e);
  }

  protected override void OnMouseEnter(MouseEventArgs e)
  {
    if (this.EnableSegmentDragging)
      this.OnChartDragEntered(e.GetPosition((IInputElement) this.SeriesPanel), (object) e.MouseDevice.DirectlyOver);
    base.OnMouseEnter(e);
  }

  protected override void OnMouseLeave(MouseEventArgs e)
  {
    if (this.EnableSegmentDragging)
      this.OnChartDragExited(e.GetPosition((IInputElement) this.SeriesPanel), e.OriginalSource);
    base.OnMouseLeave(e);
  }

  protected void RaiseDragStart(ChartDragStartEventArgs args)
  {
    if (this.DragStart == null)
      return;
    this.DragStart((object) this, args);
  }

  protected void RaiseDragEnd(ChartDragEndEventArgs args)
  {
    if (this.DragEnd == null)
      return;
    this.DragEnd((object) this, args);
  }

  protected void RaiseDragDelta(Syncfusion.UI.Xaml.Charts.DragDelta args)
  {
    if (this.DragDelta == null)
      return;
    this.DragDelta((object) this, args);
  }

  protected void RaiseDragEnter(XySegmentEnterEventArgs args)
  {
    if (this.SegmentEnter == null)
      return;
    this.SegmentEnter((object) this, args);
  }

  protected void RaisePreviewEnd(XyPreviewEndEventArgs args)
  {
    if (this.PreviewDragEnd == null)
      return;
    this.PreviewDragEnd((object) this, args);
  }

  protected void UpdateUnderLayingModel(string path, int index, object updatedData)
  {
    IEnumerator enumerator = this.ItemsSource is DataTable ? (this.ItemsSource as DataTable).Rows.GetEnumerator() : (this.ItemsSource as IEnumerable).GetEnumerator();
    if (!enumerator.MoveNext())
      return;
    int num = 0;
    while (num != index)
    {
      ++num;
      if (!enumerator.MoveNext())
        return;
    }
    ChartSeriesBase.SetPropertyValue(enumerator.Current, path.Split('.'), updatedData);
  }

  private void ResetSegmentDragTooltipInfo()
  {
    if (this.Tooltip == null)
      return;
    this.SeriesPanel.Children.Remove((UIElement) this.Tooltip);
    this.Tooltip = (ContentControl) null;
    this.DragInfo = (ChartDragPointinfo) null;
  }

  private void TopAlignTooltip(double x, double y, double offsetY)
  {
    this.Tooltip.ContentTemplate = this.normalTooltip;
    this.UpdateTooltip();
    this.tooltipX = x - this.Tooltip.DesiredSize.Width / 2.0;
    this.tooltipY = y - this.Tooltip.DesiredSize.Height - offsetY;
  }

  private void LeftAlignTooltip(double x, double y, double offsetX)
  {
    this.Tooltip.ContentTemplate = this.oppLeftTooltip;
    this.UpdateTooltip();
    this.tooltipX = x - this.Tooltip.DesiredSize.Width - offsetX;
    this.tooltipY = y - this.Tooltip.DesiredSize.Height / 2.0;
  }

  private void RightAlignTooltip(double x, double y, double offsetX)
  {
    this.Tooltip.ContentTemplate = this.oppRightTootip;
    this.UpdateTooltip();
    this.tooltipX = x + offsetX;
    this.tooltipY = y - this.Tooltip.DesiredSize.Height / 2.0;
  }

  private void UpdateTooltip() => this.Tooltip.UpdateLayout();
}
