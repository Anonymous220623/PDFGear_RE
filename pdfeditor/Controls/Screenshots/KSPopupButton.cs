// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Screenshots.KSPopupButton
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

#nullable disable
namespace pdfeditor.Controls.Screenshots;

public class KSPopupButton : Button
{
  private Timer closeTimer;
  private Timer openTimer;
  public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof (CornerRadius), typeof (CornerRadius), typeof (KSPopupButton), new PropertyMetadata((object) new CornerRadius(0.0)));
  public static readonly DependencyProperty PopupHorizontalOffsetProperty = DependencyProperty.Register(nameof (PopupHorizontalOffset), typeof (double), typeof (KSPopupButton), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty PopupVerticalOffsetProperty = DependencyProperty.Register(nameof (PopupVerticalOffset), typeof (double), typeof (KSPopupButton), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty PopupPlacementProperty = DependencyProperty.Register(nameof (PopupPlacement), typeof (PlacementMode), typeof (KSPopupButton), new PropertyMetadata((object) PlacementMode.Top));
  public static readonly DependencyProperty PopupOpenModeProperty = DependencyProperty.Register(nameof (PopupOpenMode), typeof (EnumKSPopupOpenMode), typeof (KSPopupButton), (PropertyMetadata) new UIPropertyMetadata((object) EnumKSPopupOpenMode.OpenOnHover, new PropertyChangedCallback(KSPopupButton.OnPopupOpenModeChanged)));
  public static readonly DependencyProperty PopupContentProperty = DependencyProperty.Register(nameof (PopupContent), typeof (object), typeof (KSPopupButton), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(KSPopupButton.OnPopupContentChanged)));
  public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof (IsOpen), typeof (bool), typeof (KSPopupButton), new PropertyMetadata((object) false, new PropertyChangedCallback(KSPopupButton.OnIsOpenChanged)));
  public static readonly DependencyProperty UseAnimationProperty = DependencyProperty.Register(nameof (UseAnimation), typeof (bool), typeof (KSPopupButton), new PropertyMetadata((object) true));
  private Border bd_content;

  static KSPopupButton()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (KSPopupButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (KSPopupButton)));
  }

  public KSPopupButton() => this.Loaded += new RoutedEventHandler(this.KSPopupButton_Loaded);

  private void KSPopupButton_Loaded(object sender, RoutedEventArgs e)
  {
    Window window = Window.GetWindow((DependencyObject) this);
    if (window == null)
      return;
    window.Deactivated -= new EventHandler(this.Window_Deactivated);
    window.Deactivated += new EventHandler(this.Window_Deactivated);
    window.PreviewMouseDown -= new MouseButtonEventHandler(this.Window_PreviewMouseDown);
    window.PreviewMouseDown += new MouseButtonEventHandler(this.Window_PreviewMouseDown);
  }

  private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
  {
    if (this.InnerPopup == null || !this.InnerPopup.IsOpen || this.IsMouseOver)
      return;
    this.InnerPopup.IsOpen = false;
  }

  private void Window_MouseDown(object sender, MouseButtonEventArgs e)
  {
    if (this.InnerPopup == null || !this.InnerPopup.IsOpen || this.IsMouseOver || this.PopupOpenMode == EnumKSPopupOpenMode.OpenOnCode)
      return;
    this.InnerPopup.IsOpen = false;
  }

  private void Window_Deactivated(object sender, EventArgs e)
  {
    if (this.InnerPopup == null || this.PopupOpenMode == EnumKSPopupOpenMode.OpenOnCode)
      return;
    this.InnerPopup.IsOpen = false;
  }

  private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    KSPopupButton ksPopupButton = d as KSPopupButton;
    if (ksPopupButton.IsOpen)
      ksPopupButton.InnerPopup.IsOpen = true;
    else
      ksPopupButton.InnerPopup.IsOpen = false;
  }

  private static void OnPopupContentChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    KSPopupButton ksPopupButton = d as KSPopupButton;
    ksPopupButton.RemoveLogicalChild(e.OldValue);
    ksPopupButton.AddLogicalChild(e.NewValue);
  }

  public CornerRadius CornerRadius
  {
    get => (CornerRadius) this.GetValue(KSPopupButton.CornerRadiusProperty);
    set => this.SetValue(KSPopupButton.CornerRadiusProperty, (object) value);
  }

  public PlacementMode PopupPlacement
  {
    get => (PlacementMode) this.GetValue(KSPopupButton.PopupPlacementProperty);
    set => this.SetValue(KSPopupButton.PopupPlacementProperty, (object) value);
  }

  public double PopupVerticalOffset
  {
    get => (double) this.GetValue(KSPopupButton.PopupVerticalOffsetProperty);
    set => this.SetValue(KSPopupButton.PopupVerticalOffsetProperty, (object) value);
  }

  public double PopupHorizontalOffset
  {
    get => (double) this.GetValue(KSPopupButton.PopupHorizontalOffsetProperty);
    set => this.SetValue(KSPopupButton.PopupHorizontalOffsetProperty, (object) value);
  }

  public EnumKSPopupOpenMode PopupOpenMode
  {
    get => (EnumKSPopupOpenMode) this.GetValue(KSPopupButton.PopupOpenModeProperty);
    set => this.SetValue(KSPopupButton.PopupOpenModeProperty, (object) value);
  }

  public object PopupContent
  {
    get => this.GetValue(KSPopupButton.PopupContentProperty);
    set => this.SetValue(KSPopupButton.PopupContentProperty, value);
  }

  public bool IsOpen
  {
    get => (bool) this.GetValue(KSPopupButton.IsOpenProperty);
    set => this.SetValue(KSPopupButton.IsOpenProperty, (object) value);
  }

  public bool UseAnimation
  {
    get => (bool) this.GetValue(KSPopupButton.UseAnimationProperty);
    set => this.SetValue(KSPopupButton.UseAnimationProperty, (object) value);
  }

  private static void OnPopupOpenModeChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    KSPopupButton ksPopupButton = d as KSPopupButton;
    if (ksPopupButton.InnerPopup == null)
      return;
    ksPopupButton.InnerPopup.IsOpen = false;
  }

  public Popup InnerPopup { get; private set; }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.InnerPopup = this.GetTemplateChild("my_popup") as Popup;
    if (this.InnerPopup != null)
    {
      this.InnerPopup.Opened += new EventHandler(this.Popup_Opened);
      this.InnerPopup.Closed += new EventHandler(this.Popup_Closed);
    }
    this.bd_content = this.GetTemplateChild("bd_content") as Border;
    if (this.bd_content == null)
      return;
    this.bd_content.PreviewMouseDown += new MouseButtonEventHandler(this.Bd_content_PreviewMouseDown);
  }

  private void Bd_content_PreviewMouseDown(object sender, MouseButtonEventArgs e)
  {
    if (this.PopupOpenMode != EnumKSPopupOpenMode.OpenOnClick)
      return;
    if (this.InnerPopup.IsOpen)
    {
      this.InnerPopup.IsOpen = false;
    }
    else
    {
      this.InnerPopup.IsOpen = true;
      this.Focus();
    }
  }

  private void Popup_Closed(object sender, EventArgs e) => this.IsOpen = false;

  private void Popup_Opened(object sender, EventArgs e) => this.IsOpen = true;

  protected override void OnMouseEnter(MouseEventArgs e)
  {
    base.OnMouseEnter(e);
    if (this.PopupOpenMode != EnumKSPopupOpenMode.OpenOnHover)
      return;
    if (this.closeTimer != null)
    {
      this.closeTimer.Change(-1, -1);
      this.closeTimer.Dispose();
      this.closeTimer = (Timer) null;
    }
    if (this.UseAnimation)
    {
      if (this.openTimer != null)
        return;
      this.openTimer = new Timer((TimerCallback) (obj => this.Dispatcher?.Invoke((Action) (() =>
      {
        this.InnerPopup.IsOpen = true;
        try
        {
          if (this.openTimer == null)
            return;
          this.openTimer.Change(-1, -1);
          this.openTimer.Dispose();
          this.openTimer = (Timer) null;
        }
        catch
        {
        }
      }))), (object) null, 100, -1);
    }
    else
      this.InnerPopup.IsOpen = true;
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    base.OnMouseMove(e);
    if (this.PopupOpenMode != EnumKSPopupOpenMode.OpenOnHover)
      return;
    if (this.closeTimer != null)
    {
      this.closeTimer.Change(-1, -1);
      this.closeTimer.Dispose();
      this.closeTimer = (Timer) null;
    }
    if (this.openTimer != null || this.InnerPopup.IsOpen)
      return;
    this.InnerPopup.IsOpen = true;
  }

  protected override void OnMouseLeave(MouseEventArgs e)
  {
    base.OnMouseLeave(e);
    if (this.PopupOpenMode != EnumKSPopupOpenMode.OpenOnHover)
      return;
    if (this.openTimer != null)
    {
      this.openTimer.Change(-1, -1);
      this.openTimer.Dispose();
      this.openTimer = (Timer) null;
    }
    if (this.closeTimer != null)
    {
      this.closeTimer.Change(-1, -1);
      this.closeTimer.Dispose();
      this.closeTimer = (Timer) null;
    }
    this.closeTimer = new Timer((TimerCallback) (obj => this.Dispatcher?.Invoke((Action) (() => this.InnerPopup.IsOpen = false))), (object) null, 300, -1);
  }

  public void OpenPopup()
  {
    if (this.InnerPopup == null)
      return;
    this.InnerPopup.IsOpen = true;
  }

  public void ClosePopup()
  {
    if (this.InnerPopup == null)
      return;
    this.InnerPopup.IsOpen = false;
  }
}
