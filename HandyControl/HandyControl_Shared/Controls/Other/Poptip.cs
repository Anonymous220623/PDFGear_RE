// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.Poptip
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Data.Enum;
using HandyControl.Expression.Drawing;
using HandyControl.Tools;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

#nullable disable
namespace HandyControl.Controls;

public class Poptip : AdornerElement
{
  private readonly Popup _popup;
  private DispatcherTimer _openTimer;
  public static readonly DependencyProperty HitModeProperty = DependencyProperty.RegisterAttached(nameof (HitMode), typeof (HitMode), typeof (Poptip), new PropertyMetadata((object) HitMode.Hover));
  public static readonly DependencyProperty ContentProperty = DependencyProperty.RegisterAttached(nameof (Content), typeof (object), typeof (Poptip), new PropertyMetadata((object) null, new PropertyChangedCallback(Poptip.OnContentChanged)));
  public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register(nameof (ContentTemplate), typeof (DataTemplate), typeof (Poptip), new PropertyMetadata((object) null));
  public static readonly DependencyProperty ContentStringFormatProperty = DependencyProperty.Register(nameof (ContentStringFormat), typeof (string), typeof (Poptip), new PropertyMetadata((object) null));
  public static readonly DependencyProperty ContentTemplateSelectorProperty = DependencyProperty.Register(nameof (ContentTemplateSelector), typeof (DataTemplateSelector), typeof (Poptip), new PropertyMetadata((object) null));
  public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.RegisterAttached(nameof (VerticalOffset), typeof (double), typeof (Poptip), new PropertyMetadata(ValueBoxes.Double0Box));
  public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.RegisterAttached(nameof (HorizontalOffset), typeof (double), typeof (Poptip), new PropertyMetadata(ValueBoxes.Double0Box));
  public static readonly DependencyProperty PlacementTypeProperty = DependencyProperty.RegisterAttached(nameof (PlacementType), typeof (PlacementType), typeof (Poptip), new PropertyMetadata((object) PlacementType.Top));
  public static readonly DependencyProperty IsOpenProperty = DependencyProperty.RegisterAttached(nameof (IsOpen), typeof (bool), typeof (Poptip), new PropertyMetadata(ValueBoxes.FalseBox, new PropertyChangedCallback(Poptip.OnIsOpenChanged)));
  public static readonly DependencyProperty DelayProperty = DependencyProperty.Register(nameof (Delay), typeof (double), typeof (Poptip), new PropertyMetadata((object) 1000.0), new ValidateValueCallback(ValidateHelper.IsInRangeOfPosDoubleIncludeZero));

  public Poptip()
  {
    this._popup = new Popup()
    {
      AllowsTransparency = true,
      Child = (UIElement) this,
      Placement = PlacementMode.Relative
    };
    this._popup.SetBinding(FrameworkElement.DataContextProperty, (BindingBase) new Binding(FrameworkElement.DataContextProperty.Name)
    {
      Source = (object) this
    });
  }

  public static void SetHitMode(DependencyObject element, HitMode value)
  {
    element.SetValue(Poptip.HitModeProperty, (object) value);
  }

  public static HitMode GetHitMode(DependencyObject element)
  {
    return (HitMode) element.GetValue(Poptip.HitModeProperty);
  }

  public HitMode HitMode
  {
    get => (HitMode) this.GetValue(Poptip.HitModeProperty);
    set => this.SetValue(Poptip.HitModeProperty, (object) value);
  }

  private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (d is Poptip || AdornerElement.GetInstance(d) != null)
      return;
    AdornerElement.SetInstance(d, (AdornerElement) Poptip.Default);
    AdornerElement.SetIsInstance(d, false);
  }

  public static void SetContent(DependencyObject element, object value)
  {
    element.SetValue(Poptip.ContentProperty, value);
  }

  public static object GetContent(DependencyObject element)
  {
    return element.GetValue(Poptip.ContentProperty);
  }

  public object Content
  {
    get => this.GetValue(Poptip.ContentProperty);
    set => this.SetValue(Poptip.ContentProperty, value);
  }

  public DataTemplate ContentTemplate
  {
    get => (DataTemplate) this.GetValue(Poptip.ContentTemplateProperty);
    set => this.SetValue(Poptip.ContentTemplateProperty, (object) value);
  }

  public string ContentStringFormat
  {
    get => (string) this.GetValue(Poptip.ContentStringFormatProperty);
    set => this.SetValue(Poptip.ContentStringFormatProperty, (object) value);
  }

  public DataTemplateSelector ContentTemplateSelector
  {
    get => (DataTemplateSelector) this.GetValue(Poptip.ContentTemplateSelectorProperty);
    set => this.SetValue(Poptip.ContentTemplateSelectorProperty, (object) value);
  }

  public static void SetVerticalOffset(DependencyObject element, double value)
  {
    element.SetValue(Poptip.VerticalOffsetProperty, (object) value);
  }

  public static double GetVerticalOffset(DependencyObject element)
  {
    return (double) element.GetValue(Poptip.VerticalOffsetProperty);
  }

  public double VerticalOffset
  {
    get => (double) this.GetValue(Poptip.VerticalOffsetProperty);
    set => this.SetValue(Poptip.VerticalOffsetProperty, (object) value);
  }

  public static void SetHorizontalOffset(DependencyObject element, double value)
  {
    element.SetValue(Poptip.HorizontalOffsetProperty, (object) value);
  }

  public static double GetHorizontalOffset(DependencyObject element)
  {
    return (double) element.GetValue(Poptip.HorizontalOffsetProperty);
  }

  public double HorizontalOffset
  {
    get => (double) this.GetValue(Poptip.HorizontalOffsetProperty);
    set => this.SetValue(Poptip.HorizontalOffsetProperty, (object) value);
  }

  public static void SetPlacement(DependencyObject element, PlacementType value)
  {
    element.SetValue(Poptip.PlacementTypeProperty, (object) value);
  }

  public static PlacementType GetPlacement(DependencyObject element)
  {
    return (PlacementType) element.GetValue(Poptip.PlacementTypeProperty);
  }

  public PlacementType PlacementType
  {
    get => (PlacementType) this.GetValue(Poptip.PlacementTypeProperty);
    set => this.SetValue(Poptip.PlacementTypeProperty, (object) value);
  }

  private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (d is Poptip poptip)
      poptip.SwitchPoptip((bool) e.NewValue);
    else
      ((Poptip) AdornerElement.GetInstance(d))?.SwitchPoptip((bool) e.NewValue);
  }

  public static void SetIsOpen(DependencyObject element, bool value)
  {
    element.SetValue(Poptip.IsOpenProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetIsOpen(DependencyObject element)
  {
    return (bool) element.GetValue(Poptip.IsOpenProperty);
  }

  public bool IsOpen
  {
    get => (bool) this.GetValue(Poptip.IsOpenProperty);
    set => this.SetValue(Poptip.IsOpenProperty, ValueBoxes.BooleanBox(value));
  }

  public double Delay
  {
    get => (double) this.GetValue(Poptip.DelayProperty);
    set => this.SetValue(Poptip.DelayProperty, (object) value);
  }

  public static Poptip Default => new Poptip();

  protected sealed override void OnTargetChanged(FrameworkElement element, bool isNew)
  {
    base.OnTargetChanged(element, isNew);
    if (element == null)
      return;
    if (!isNew)
    {
      element.MouseEnter -= new MouseEventHandler(this.Element_MouseEnter);
      element.MouseLeave -= new MouseEventHandler(this.Element_MouseLeave);
      element.GotFocus -= new RoutedEventHandler(this.Element_GotFocus);
      element.LostFocus -= new RoutedEventHandler(this.Element_LostFocus);
      this.ElementTarget = (FrameworkElement) null;
    }
    else
    {
      element.MouseEnter += new MouseEventHandler(this.Element_MouseEnter);
      element.MouseLeave += new MouseEventHandler(this.Element_MouseLeave);
      element.GotFocus += new RoutedEventHandler(this.Element_GotFocus);
      element.LostFocus += new RoutedEventHandler(this.Element_LostFocus);
      this.ElementTarget = element;
      this._popup.PlacementTarget = (UIElement) this.ElementTarget;
    }
  }

  protected override void Dispose() => this.SwitchPoptip(false);

  private void UpdateLocation()
  {
    double width1 = this.Target.RenderSize.Width;
    double height1 = this.Target.RenderSize.Height;
    this.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
    Size desiredSize = this.DesiredSize;
    double width2 = desiredSize.Width;
    double height2 = desiredSize.Height;
    double num1 = 0.0;
    double num2 = 0.0;
    Poptip instance = (Poptip) AdornerElement.GetInstance((DependencyObject) this.Target);
    PlacementType placementType = instance.PlacementType;
    double horizontalOffset = instance.HorizontalOffset;
    double verticalOffset = instance.VerticalOffset;
    switch (placementType)
    {
      case PlacementType.LeftTop:
        this._popup.HorizontalOffset = num1 + horizontalOffset;
        this._popup.VerticalOffset = num2 + verticalOffset;
        break;
      case PlacementType.Left:
        num2 = -(height2 - height1) * 0.5;
        goto case PlacementType.LeftTop;
      case PlacementType.LeftBottom:
        num2 = -(height2 - height1);
        goto case PlacementType.LeftTop;
      case PlacementType.TopLeft:
        num1 = width2;
        num2 = -height2;
        goto case PlacementType.LeftTop;
      case PlacementType.Top:
        num1 = (width2 + width1) * 0.5;
        num2 = -height2;
        goto case PlacementType.LeftTop;
      case PlacementType.TopRight:
        num1 = width1;
        num2 = -height2;
        goto case PlacementType.LeftTop;
      case PlacementType.RightTop:
        num1 = width2 + width1;
        goto case PlacementType.LeftTop;
      case PlacementType.Right:
        num1 = width2 + width1;
        num2 = -(height2 - height1) * 0.5;
        goto case PlacementType.LeftTop;
      case PlacementType.RightBottom:
        num1 = width2 + width1;
        num2 = -(height2 - height1);
        goto case PlacementType.LeftTop;
      case PlacementType.BottomLeft:
        num1 = width2;
        num2 = height1;
        goto case PlacementType.LeftTop;
      case PlacementType.Bottom:
        num1 = (width2 + width1) * 0.5;
        num2 = height1;
        goto case PlacementType.LeftTop;
      case PlacementType.BottomRight:
        num1 = width1;
        num2 = height1;
        goto case PlacementType.LeftTop;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  private void SwitchPoptip(bool isShow)
  {
    if (isShow)
    {
      if (!AdornerElement.GetIsInstance((DependencyObject) this.Target))
      {
        this.SetCurrentValue(Poptip.ContentProperty, Poptip.GetContent((DependencyObject) this.Target));
        this.SetCurrentValue(Poptip.PlacementTypeProperty, (object) Poptip.GetPlacement((DependencyObject) this.Target));
        this.SetCurrentValue(Poptip.HitModeProperty, (object) Poptip.GetHitMode((DependencyObject) this.Target));
        this.SetCurrentValue(Poptip.HorizontalOffsetProperty, (object) Poptip.GetHorizontalOffset((DependencyObject) this.Target));
        this.SetCurrentValue(Poptip.VerticalOffsetProperty, (object) Poptip.GetVerticalOffset((DependencyObject) this.Target));
        this.SetCurrentValue(Poptip.IsOpenProperty, (object) Poptip.GetIsOpen((DependencyObject) this.Target));
      }
      this._popup.PlacementTarget = (UIElement) this.Target;
      this.UpdateLocation();
    }
    this.ResetTimer();
    double delay = this.Delay;
    if (!isShow || this.HitMode != HitMode.Hover || MathHelper.IsVerySmall(delay))
    {
      this._popup.IsOpen = isShow;
      this.Target.SetCurrentValue(Poptip.IsOpenProperty, (object) isShow);
    }
    else
    {
      this._openTimer = new DispatcherTimer()
      {
        Interval = TimeSpan.FromMilliseconds(delay)
      };
      this._openTimer.Tick += new EventHandler(this.OpenTimer_Tick);
      this._openTimer.Start();
    }
  }

  private void ResetTimer()
  {
    if (this._openTimer == null)
      return;
    this._openTimer.Stop();
    this._openTimer = (DispatcherTimer) null;
  }

  private void OpenTimer_Tick(object sender, EventArgs e)
  {
    this._popup.IsOpen = true;
    this.Target.SetCurrentValue(Poptip.IsOpenProperty, (object) true);
    this.ResetTimer();
  }

  private void Element_MouseEnter(object sender, MouseEventArgs e)
  {
    if ((AdornerElement.GetIsInstance((DependencyObject) this.Target) ? (int) this.HitMode : (int) Poptip.GetHitMode((DependencyObject) this.Target)) != 1)
      return;
    this.SwitchPoptip(true);
  }

  private void Element_MouseLeave(object sender, MouseEventArgs e)
  {
    if ((AdornerElement.GetIsInstance((DependencyObject) this.Target) ? (int) this.HitMode : (int) Poptip.GetHitMode((DependencyObject) this.Target)) != 1)
      return;
    this.SwitchPoptip(false);
  }

  private void Element_GotFocus(object sender, RoutedEventArgs e)
  {
    if ((AdornerElement.GetIsInstance((DependencyObject) this.Target) ? (int) this.HitMode : (int) Poptip.GetHitMode((DependencyObject) this.Target)) != 2)
      return;
    this.SwitchPoptip(true);
  }

  private void Element_LostFocus(object sender, RoutedEventArgs e)
  {
    if ((AdornerElement.GetIsInstance((DependencyObject) this.Target) ? (int) this.HitMode : (int) Poptip.GetHitMode((DependencyObject) this.Target)) != 2)
      return;
    this.SwitchPoptip(false);
  }
}
