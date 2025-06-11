// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.Magnifier
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools;
using HandyControl.Tools.Extension;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_VisualBrush", Type = typeof (VisualBrush))]
public class Magnifier : AdornerElement
{
  private AdornerContainer _adornerContainer;
  private Size _viewboxSize;
  private const string ElementVisualBrush = "PART_VisualBrush";
  private VisualBrush _visualBrush = new VisualBrush();
  private readonly TranslateTransform _translateTransform;
  public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register(nameof (HorizontalOffset), typeof (double), typeof (Magnifier), new PropertyMetadata(ValueBoxes.Double0Box));
  public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register(nameof (VerticalOffset), typeof (double), typeof (Magnifier), new PropertyMetadata(ValueBoxes.Double0Box));
  public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(nameof (Scale), typeof (double), typeof (Magnifier), new PropertyMetadata((object) 5.0, new PropertyChangedCallback(Magnifier.OnScaleChanged)), new ValidateValueCallback(ValidateHelper.IsInRangeOfPosDouble));

  public double HorizontalOffset
  {
    get => (double) this.GetValue(Magnifier.HorizontalOffsetProperty);
    set => this.SetValue(Magnifier.HorizontalOffsetProperty, (object) value);
  }

  public double VerticalOffset
  {
    get => (double) this.GetValue(Magnifier.VerticalOffsetProperty);
    set => this.SetValue(Magnifier.VerticalOffsetProperty, (object) value);
  }

  public static Magnifier Default => new Magnifier();

  private static void OnScaleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((Magnifier) d).UpdateViewboxSize();
  }

  public double Scale
  {
    get => (double) this.GetValue(Magnifier.ScaleProperty);
    set => this.SetValue(Magnifier.ScaleProperty, (object) value);
  }

  public Magnifier()
  {
    this._translateTransform = new TranslateTransform();
    this.RenderTransform = (Transform) this._translateTransform;
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    if (!(this.GetTemplateChild("PART_VisualBrush") is VisualBrush visualBrush1))
      visualBrush1 = new VisualBrush();
    VisualBrush visualBrush2 = visualBrush1;
    visualBrush2.Viewbox = this._visualBrush.Viewbox;
    this._visualBrush = visualBrush2;
  }

  protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
  {
    base.OnRenderSizeChanged(sizeInfo);
    this.UpdateViewboxSize();
  }

  private void UpdateViewboxSize()
  {
    this._viewboxSize = new Size(this.ActualWidth / this.Scale, this.ActualHeight / this.Scale);
  }

  protected sealed override void OnTargetChanged(FrameworkElement element, bool isNew)
  {
    base.OnTargetChanged(element, isNew);
    if (element == null)
      return;
    if (!isNew)
    {
      element.MouseEnter -= new MouseEventHandler(this.Element_MouseEnter);
      element.MouseLeave -= new MouseEventHandler(this.Element_MouseLeave);
      element.MouseMove -= new MouseEventHandler(this.Element_MouseMove);
      this.ElementTarget = (FrameworkElement) null;
    }
    else
    {
      element.MouseEnter += new MouseEventHandler(this.Element_MouseEnter);
      element.MouseLeave += new MouseEventHandler(this.Element_MouseLeave);
      element.MouseMove += new MouseEventHandler(this.Element_MouseMove);
      this.ElementTarget = element;
    }
  }

  protected override void Dispose() => this.HideAdornerElement();

  private void UpdateLocation()
  {
    Point position1 = Mouse.GetPosition((IInputElement) this.Target);
    double num1 = position1.X - this._visualBrush.Viewbox.Width / 2.0;
    double num2 = position1.Y - this._visualBrush.Viewbox.Height / 2.0;
    Vector offset = VisualTreeHelper.GetOffset((Visual) this.Target);
    this._visualBrush.Viewbox = new Rect(new Point(num1 + offset.X, num2 + offset.Y), this._viewboxSize);
    Point position2 = Mouse.GetPosition((IInputElement) this._adornerContainer);
    this._translateTransform.X = position2.X + this.HorizontalOffset;
    this._translateTransform.Y = position2.Y + this.VerticalOffset;
  }

  private void Element_MouseMove(object sender, MouseEventArgs e) => this.UpdateLocation();

  private void Element_MouseLeave(object sender, MouseEventArgs e) => this.HideAdornerElement();

  private void Element_MouseEnter(object sender, MouseEventArgs e) => this.ShowAdornerElement();

  private void HideAdornerElement()
  {
    if (this._adornerContainer == null)
      return;
    AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer((Visual) this.Target);
    if (adornerLayer != null)
    {
      adornerLayer.Remove((Adorner) this._adornerContainer);
    }
    else
    {
      AdornerContainer adornerContainer = this._adornerContainer;
      if (adornerContainer != null && adornerContainer.Parent is AdornerLayer parent)
        parent.Remove((Adorner) this._adornerContainer);
    }
    if (this._adornerContainer == null)
      return;
    this._adornerContainer.Child = (UIElement) null;
    this._adornerContainer = (AdornerContainer) null;
  }

  private void ShowAdornerElement()
  {
    if (this._adornerContainer == null)
    {
      AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer((Visual) this.Target);
      if (adornerLayer == null)
        return;
      this._adornerContainer = new AdornerContainer((UIElement) adornerLayer)
      {
        Child = (UIElement) this
      };
      adornerLayer.Add((Adorner) this._adornerContainer);
    }
    this.Show();
  }
}
