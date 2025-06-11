// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.PreviewSlider
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Interactivity;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_Track", Type = typeof (Track))]
[TemplatePart(Name = "PART_Thumb", Type = typeof (FrameworkElement))]
public class PreviewSlider : Slider
{
  private AdornerContainer _adorner;
  private const string TrackKey = "PART_Track";
  private const string ThumbKey = "PART_Thumb";
  private FrameworkElement _previewContent;
  private FrameworkElement _thumb;
  private TranslateTransform _transform;
  private Track _track;
  public static readonly DependencyProperty PreviewContentProperty = DependencyProperty.Register(nameof (PreviewContent), typeof (object), typeof (PreviewSlider), new PropertyMetadata((object) null));
  public static readonly DependencyProperty PreviewContentOffsetProperty = DependencyProperty.Register(nameof (PreviewContentOffset), typeof (double), typeof (PreviewSlider), new PropertyMetadata((object) 9.0));
  public static readonly DependencyProperty PreviewPositionProperty = DependencyProperty.RegisterAttached(nameof (PreviewPosition), typeof (double), typeof (PreviewSlider), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly RoutedEvent PreviewPositionChangedEvent = EventManager.RegisterRoutedEvent("PreviewPositionChanged", RoutingStrategy.Bubble, typeof (EventHandler<FunctionEventArgs<double>>), typeof (PreviewSlider));

  public object PreviewContent
  {
    get => this.GetValue(PreviewSlider.PreviewContentProperty);
    set => this.SetValue(PreviewSlider.PreviewContentProperty, value);
  }

  public double PreviewContentOffset
  {
    get => (double) this.GetValue(PreviewSlider.PreviewContentOffsetProperty);
    set => this.SetValue(PreviewSlider.PreviewContentOffsetProperty, (object) value);
  }

  public static void SetPreviewPosition(DependencyObject element, double value)
  {
    element.SetValue(PreviewSlider.PreviewPositionProperty, (object) value);
  }

  public static double GetPreviewPosition(DependencyObject element)
  {
    return (double) element.GetValue(PreviewSlider.PreviewPositionProperty);
  }

  public double PreviewPosition
  {
    get => PreviewSlider.GetPreviewPosition((DependencyObject) this._previewContent);
    set => PreviewSlider.SetPreviewPosition((DependencyObject) this._previewContent, value);
  }

  public event EventHandler<FunctionEventArgs<double>> PreviewPositionChanged
  {
    add => this.AddHandler(PreviewSlider.PreviewPositionChangedEvent, (Delegate) value);
    remove => this.RemoveHandler(PreviewSlider.PreviewPositionChangedEvent, (Delegate) value);
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    base.OnMouseMove(e);
    if (this._previewContent == null)
      return;
    Point position = e.GetPosition((IInputElement) this._adorner);
    double maximum = this.Maximum;
    double minimum = this.Minimum;
    if (this.Orientation == Orientation.Horizontal)
    {
      Point point1;
      double num1;
      if (this.IsDirectionReversed)
      {
        point1 = e.GetPosition((IInputElement) this);
        num1 = (1.0 - (point1.X - this._thumb.ActualWidth * 0.5) / this._track.ActualWidth) * (maximum - minimum) + minimum;
      }
      else
        num1 = (e.GetPosition((IInputElement) this).X - this._thumb.ActualWidth * 0.5) / this._track.ActualWidth * (maximum - minimum) + minimum;
      double num2 = num1;
      if (num2 > maximum || num2 < 0.0)
      {
        if (!this._thumb.IsMouseCaptureWithin)
          return;
        this.PreviewPosition = this.Value;
        return;
      }
      this._transform.X = position.X - this._previewContent.ActualWidth * 0.5;
      TranslateTransform transform = this._transform;
      FrameworkElement thumb = this._thumb;
      point1 = new Point();
      Point point2 = point1;
      AdornerContainer adorner = this._adorner;
      point1 = thumb.TranslatePoint(point2, (UIElement) adorner);
      double num3 = point1.Y - this._previewContent.ActualHeight - this.PreviewContentOffset;
      transform.Y = num3;
      this.PreviewPosition = this._thumb.IsMouseCaptureWithin ? this.Value : num2;
    }
    else
    {
      Point point3;
      double num4;
      if (this.IsDirectionReversed)
      {
        num4 = (e.GetPosition((IInputElement) this).Y - this._thumb.ActualHeight * 0.5) / this._track.ActualHeight * (maximum - minimum) + minimum;
      }
      else
      {
        point3 = e.GetPosition((IInputElement) this);
        num4 = (1.0 - (point3.Y - this._thumb.ActualHeight * 0.5) / this._track.ActualHeight) * (maximum - minimum) + minimum;
      }
      double num5 = num4;
      if (num5 > maximum || num5 < 0.0)
      {
        if (!this._thumb.IsMouseCaptureWithin)
          return;
        this.PreviewPosition = this.Value;
        return;
      }
      TranslateTransform transform = this._transform;
      FrameworkElement thumb = this._thumb;
      point3 = new Point();
      Point point4 = point3;
      AdornerContainer adorner = this._adorner;
      point3 = thumb.TranslatePoint(point4, (UIElement) adorner);
      double num6 = point3.X - this._previewContent.ActualWidth - this.PreviewContentOffset;
      transform.X = num6;
      this._transform.Y = position.Y - this._previewContent.ActualHeight * 0.5;
      this.PreviewPosition = this._thumb.IsMouseCaptureWithin ? this.Value : num5;
    }
    this.RaiseEvent((RoutedEventArgs) new FunctionEventArgs<double>(PreviewSlider.PreviewPositionChangedEvent, (object) this)
    {
      Info = this.PreviewPosition
    });
  }

  protected override void OnMouseEnter(MouseEventArgs e)
  {
    base.OnMouseEnter(e);
    if (this._adorner != null)
      return;
    AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer((Visual) this);
    if (adornerLayer == null)
      return;
    this._adorner = new AdornerContainer((UIElement) adornerLayer)
    {
      Child = (UIElement) this._previewContent
    };
    adornerLayer.Add((Adorner) this._adorner);
  }

  protected override void OnMouseLeave(MouseEventArgs e)
  {
    base.OnMouseLeave(e);
    AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer((Visual) this);
    if (adornerLayer != null)
    {
      adornerLayer.Remove((Adorner) this._adorner);
    }
    else
    {
      AdornerContainer adorner = this._adorner;
      if (adorner != null && adorner.Parent is AdornerLayer parent)
        parent.Remove((Adorner) this._adorner);
    }
    if (this._adorner == null)
      return;
    this._adorner.Child = (UIElement) null;
    this._adorner = (AdornerContainer) null;
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    ContentControl contentControl1 = new ContentControl();
    contentControl1.DataContext = (object) this;
    ContentControl contentControl2 = contentControl1;
    contentControl2.SetBinding(ContentControl.ContentProperty, (BindingBase) new Binding(PreviewSlider.PreviewContentProperty.Name)
    {
      Source = (object) this
    });
    this._previewContent = (FrameworkElement) contentControl2;
    this._track = this.Template.FindName("PART_Track", (FrameworkElement) this) as Track;
    this._thumb = this.Template.FindName("PART_Thumb", (FrameworkElement) this) as FrameworkElement;
    if (this._previewContent == null)
      return;
    this._transform = new TranslateTransform();
    this._previewContent.HorizontalAlignment = HorizontalAlignment.Left;
    this._previewContent.VerticalAlignment = VerticalAlignment.Top;
    this._previewContent.RenderTransform = (Transform) this._transform;
  }
}
