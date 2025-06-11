// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.PopupWindow
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Properties.Langs;
using HandyControl.Tools;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_MainBorder", Type = typeof (Border))]
[TemplatePart(Name = "PART_TitleBlock", Type = typeof (TextBlock))]
public class PopupWindow : System.Windows.Window
{
  private const string ElementMainBorder = "PART_MainBorder";
  private const string ElementTitleBlock = "PART_TitleBlock";
  private Border _mainBorder;
  private TextBlock _titleBlock;
  private bool _showBackground = true;
  private FrameworkElement _targetElement;
  internal static readonly DependencyProperty ContentStrProperty = DependencyProperty.Register(nameof (ContentStr), typeof (string), typeof (PopupWindow), new PropertyMetadata((object) null));
  public static readonly DependencyProperty ShowTitleProperty = DependencyProperty.Register(nameof (ShowTitle), typeof (bool), typeof (PopupWindow), new PropertyMetadata(ValueBoxes.TrueBox));
  public static readonly DependencyProperty ShowCancelProperty = DependencyProperty.Register(nameof (ShowCancel), typeof (bool), typeof (PopupWindow), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty ShowBorderProperty = DependencyProperty.Register(nameof (ShowBorder), typeof (bool), typeof (PopupWindow), new PropertyMetadata(ValueBoxes.FalseBox));

  public override void OnApplyTemplate()
  {
    if (this._titleBlock != null)
      this._titleBlock.MouseLeftButtonDown -= new MouseButtonEventHandler(this.TitleBlock_OnMouseLeftButtonDown);
    base.OnApplyTemplate();
    this._mainBorder = this.GetTemplateChild("PART_MainBorder") as Border;
    this._titleBlock = this.GetTemplateChild("PART_TitleBlock") as TextBlock;
    if (this._titleBlock != null)
      this._titleBlock.MouseLeftButtonDown += new MouseButtonEventHandler(this.TitleBlock_OnMouseLeftButtonDown);
    if (this.PopupElement == null)
      return;
    this._mainBorder.Child = (UIElement) this.PopupElement;
  }

  internal string ContentStr
  {
    get => (string) this.GetValue(PopupWindow.ContentStrProperty);
    set => this.SetValue(PopupWindow.ContentStrProperty, (object) value);
  }

  private bool IsDialog { get; set; }

  public PopupWindow()
  {
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Close, new ExecutedRoutedEventHandler(this.CloseButton_OnClick)));
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Confirm, new ExecutedRoutedEventHandler(this.ButtonOk_OnClick)));
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Cancel, new ExecutedRoutedEventHandler(this.ButtonCancle_OnClick)));
    this.Closing += (CancelEventHandler) ((sender, args) =>
    {
      if (this.IsDialog)
        return;
      this.Owner?.Focus();
    });
    this.Loaded += (RoutedEventHandler) ((s, e) =>
    {
      if (this._showBackground)
        return;
      Point point = ArithmeticHelper.CalSafePoint(this._targetElement, this.PopupElement, this.BorderThickness);
      this.Left = point.X;
      this.Top = point.Y;
      this.Opacity = 1.0;
    });
    try
    {
      this.Owner = Application.Current.MainWindow;
    }
    catch
    {
    }
  }

  public PopupWindow(System.Windows.Window owner)
    : this()
  {
    this.Owner = owner;
  }

  private void CloseButton_OnClick(object sender, RoutedEventArgs e) => this.Close();

  public FrameworkElement PopupElement { get; set; }

  public bool ShowTitle
  {
    get => (bool) this.GetValue(PopupWindow.ShowTitleProperty);
    set => this.SetValue(PopupWindow.ShowTitleProperty, ValueBoxes.BooleanBox(value));
  }

  public bool ShowCancel
  {
    get => (bool) this.GetValue(PopupWindow.ShowCancelProperty);
    set => this.SetValue(PopupWindow.ShowCancelProperty, ValueBoxes.BooleanBox(value));
  }

  public bool ShowBorder
  {
    get => (bool) this.GetValue(PopupWindow.ShowBorderProperty);
    set => this.SetValue(PopupWindow.ShowBorderProperty, ValueBoxes.BooleanBox(value));
  }

  private void TitleBlock_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (e.LeftButton != MouseButtonState.Pressed)
      return;
    this.DragMove();
  }

  public void Show(FrameworkElement element, bool showBackground = true)
  {
    if (!showBackground)
    {
      this.Opacity = 0.0;
      this.AllowsTransparency = true;
      this.WindowStyle = WindowStyle.None;
      this.ShowTitle = false;
      this.MinWidth = 0.0;
      this.MinHeight = 0.0;
    }
    this._showBackground = showBackground;
    this._targetElement = element;
    this.Show();
  }

  public void ShowDialog(FrameworkElement element, bool showBackground = true)
  {
    if (!showBackground)
    {
      this.Opacity = 0.0;
      this.AllowsTransparency = true;
      this.WindowStyle = WindowStyle.None;
      this.ShowTitle = false;
      this.MinWidth = 0.0;
      this.MinHeight = 0.0;
    }
    this._showBackground = showBackground;
    this._targetElement = element;
    this.ShowDialog();
  }

  public void Show(System.Windows.Window element, Point point)
  {
    this.Left = element.Left + point.X;
    this.Top = element.Top + point.Y;
    this.Show();
  }

  public static void Show(string message)
  {
    PopupWindow popupWindow = new PopupWindow();
    popupWindow.AllowsTransparency = true;
    popupWindow.WindowStyle = WindowStyle.None;
    popupWindow.ContentStr = message;
    popupWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
    popupWindow.Background = HandyControl.Tools.ResourceHelper.GetResourceInternal<Brush>("PrimaryBrush");
    popupWindow.Show();
  }

  public static bool? ShowDialog(string message, string title = null, bool showCancel = false)
  {
    PopupWindow popupWindow = new PopupWindow();
    popupWindow.AllowsTransparency = true;
    popupWindow.WindowStyle = WindowStyle.None;
    popupWindow.ContentStr = message;
    popupWindow.IsDialog = true;
    popupWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
    popupWindow.ShowBorder = true;
    popupWindow.Title = string.IsNullOrEmpty(title) ? Lang.Tip : title;
    popupWindow.ShowCancel = showCancel;
    return popupWindow.ShowDialog();
  }

  private void ButtonOk_OnClick(object sender, RoutedEventArgs e)
  {
    if (this.IsDialog)
      this.DialogResult = new bool?(true);
    this.Close();
  }

  private void ButtonCancle_OnClick(object sender, RoutedEventArgs e)
  {
    if (this.IsDialog)
      this.DialogResult = new bool?(false);
    this.Close();
  }

  protected override void OnClosed(EventArgs e)
  {
    base.OnClosed(e);
    this.PopupElement = (FrameworkElement) null;
  }
}
