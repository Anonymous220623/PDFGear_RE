// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.SplitButtonAdv
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Licensing;
using Syncfusion.Windows.Controls.Tools.AutomationPeer;
using Syncfusion.Windows.Shared;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

[SkinType(SkinVisualStyle = Skin.Office2007Black, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/SplitButton/Themes/Office2007BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Silver, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/SplitButton/Themes/Office2007SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Blue, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/SplitButton/Themes/Office2010BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Black, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/SplitButton/Themes/Office2010BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Silver, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/SplitButton/Themes/Office2010SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Blend, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/SplitButton/Themes/BlendStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.VS2010, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/SplitButton/Themes/VS2010Style.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyRed, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/SplitButton/Themes/ShinyRedStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyBlue, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/SplitButton/Themes/ShinyBlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Default, Type = typeof (TileViewControl), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/ButtonControls/SplitButton/Themes/SplitButton.xaml")]
[SkinType(SkinVisualStyle = Skin.Metro, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/SplitButton/Themes/MetroStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Transparent, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/SplitButton/Themes/TransparentStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Blue, Type = typeof (TileViewControl), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/SplitButton/Themes/Office2007BlueStyle.xaml")]
public class SplitButtonAdv : DropDownButtonAdv, ICommandSource
{
  private Border _button;
  private Border _dropdownbutton;
  private Border _buttonNormal;
  private Border _dropdownbuttonNormal;
  private new Popup _dropdown;
  public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof (Command), typeof (ICommand), typeof (SplitButtonAdv), new PropertyMetadata((object) null, new PropertyChangedCallback(SplitButtonAdv.CommandChanged)));
  public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof (CommandParameter), typeof (object), typeof (SplitButtonAdv), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty IsDefaultProperty = DependencyProperty.Register(nameof (IsDefault), typeof (bool), typeof (SplitButtonAdv), new PropertyMetadata(new PropertyChangedCallback(SplitButtonAdv.OnIsDefaultChanged)));
  public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register(nameof (CommandTarget), typeof (IInputElement), typeof (SplitButtonAdv), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty IsDropDownPressedProperty = DependencyProperty.Register(nameof (IsDropDownPressed), typeof (bool), typeof (SplitButtonAdv), (PropertyMetadata) new UIPropertyMetadata((object) false));
  private EventHandler canExecuteChangedEventHandler;

  public SplitButtonAdv()
  {
    this.DefaultStyleKey = (object) typeof (SplitButtonAdv);
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  static SplitButtonAdv() => FusionLicenseProvider.GetLicenseType(Platform.WPF);

  public event RoutedEventHandler Click;

  protected virtual void OnClick()
  {
    CommandManager.RequerySuggested += new EventHandler(this.CanExecuteChanged);
    this.IsPressed = true;
    if (this.Click != null)
      this.Click((object) this, new RoutedEventArgs());
    if (this.Command != null && this.Command.CanExecute(this.CommandParameter))
      this.Command.Execute(this.CommandParameter);
    CommandManager.RequerySuggested -= new EventHandler(this.CanExecuteChanged);
  }

  [Category("Common Properties")]
  [Description("Gets or sets the command when the Button is pressed")]
  public ICommand Command
  {
    get => (ICommand) this.GetValue(SplitButtonAdv.CommandProperty);
    set => this.SetValue(SplitButtonAdv.CommandProperty, (object) value);
  }

  [Description("Gets or sets the parameter to pass the Command property")]
  [Category("Common Properties")]
  public object CommandParameter
  {
    get => this.GetValue(SplitButtonAdv.CommandParameterProperty);
    set => this.SetValue(SplitButtonAdv.CommandParameterProperty, value);
  }

  public bool IsDefault
  {
    get => (bool) this.GetValue(SplitButtonAdv.IsDefaultProperty);
    set => this.SetValue(SplitButtonAdv.IsDefaultProperty, (object) value);
  }

  public IInputElement CommandTarget
  {
    get => (IInputElement) this.GetValue(SplitButtonAdv.CommandTargetProperty);
    set => this.SetValue(SplitButtonAdv.CommandTargetProperty, (object) value);
  }

  public bool IsDropDownPressed
  {
    get => (bool) this.GetValue(SplitButtonAdv.IsDropDownPressedProperty);
    protected internal set
    {
      this.SetValue(SplitButtonAdv.IsDropDownPressedProperty, (object) value);
    }
  }

  protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    if (!this.IsDropDownOpen)
      this.IsPressed = false;
    if (!(e.Source is DropDownMenuItem) || this._dropdown == null || !this._dropdown.IsOpen)
      return;
    this._dropdown.IsOpen = false;
  }

  protected override void OnMouseWheel(MouseWheelEventArgs e)
  {
    if (this.IsDropDownOpen)
      e.Handled = true;
    else
      base.OnMouseWheel(e);
  }

  protected override void OnMouseLeave(MouseEventArgs e)
  {
    if (this.IsDropDownOpen)
      return;
    this.IsPressed = false;
  }

  private void _button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    this.IsPressed = true;
  }

  private void _button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    this.Focus();
    this.IsPressed = false;
    this.IsDropDownPressed = false;
  }

  public override void OnApplyTemplate()
  {
    this._button = this.GetTemplateChild("PART_Button") as Border;
    this._dropdownbutton = this.GetTemplateChild("PART_DropDownButton") as Border;
    this._buttonNormal = this.GetTemplateChild("PART_ButtonNormal") as Border;
    this._dropdownbuttonNormal = this.GetTemplateChild("PART_DropDownButtonNormal") as Border;
    this._dropdown = this.GetTemplateChild("PART_DropDown") as Popup;
    if (this._dropdown != null)
      this._dropdown.Closed += new EventHandler(this._dropdown_Closed);
    if (this._button != null)
    {
      this._button.MouseLeftButtonDown += new MouseButtonEventHandler(this._button_MouseLeftButtonDown);
      this._button.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this._buttonNormal_PreviewMouseLeftButtonDown);
    }
    if (this._dropdownbutton != null)
      this._dropdownbutton.MouseLeftButtonUp += new MouseButtonEventHandler(this._dropdownbutton_MouseLeftButtonUp);
    if (this._buttonNormal != null)
    {
      this._buttonNormal.MouseLeftButtonDown += new MouseButtonEventHandler(this._button_MouseLeftButtonDown);
      this._buttonNormal.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this._buttonNormal_PreviewMouseLeftButtonDown);
      this._buttonNormal.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(this._button_MouseLeftButtonUp);
    }
    if (this._dropdownbuttonNormal != null)
      this._dropdownbuttonNormal.MouseLeftButtonUp += new MouseButtonEventHandler(this._dropdownbutton_MouseLeftButtonUp);
    base.OnApplyTemplate();
  }

  private static void OnAccessKeyPressed(object sender, AccessKeyPressedEventArgs e)
  {
    if (e.Handled || e.Scope != null || e.Target != null)
      return;
    e.Target = (UIElement) sender;
  }

  protected override void OnAccessKey(AccessKeyEventArgs e)
  {
    base.OnAccessKey(e);
    this.OnClick();
  }

  private void _buttonNormal_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    this.OnPreviewMouseLeftButtonDown(e);
    this.Focus();
    if (e.ButtonState == MouseButtonState.Pressed)
      this.OnClick();
    this.IsDropDownPressed = false;
    e.Handled = true;
  }

  private void _dropdown_Closed(object sender, EventArgs e) => this.IsDropDownPressed = false;

  protected override void OnKeyUp(KeyEventArgs e)
  {
    if ((e.Key == Key.Return || e.Key == Key.Space) && (this._button != null && this._button.IsFocused || this._buttonNormal != null && this._buttonNormal.IsFocused))
    {
      if (this.IsPressed)
        this.IsPressed = false;
      else
        this.IsPressed = true;
    }
    base.OnKeyUp(e);
  }

  protected override void OnPreviewKeyDown(KeyEventArgs e)
  {
    switch (e.Key)
    {
      case Key.Return:
      case Key.Space:
      case Key.F4:
        if (this._button != null && this._button.IsFocused || this._buttonNormal != null && this._buttonNormal.IsFocused)
        {
          this.OnClick();
          this.IsDropDownPressed = false;
        }
        if (((this._dropdownbutton != null || this._dropdownbuttonNormal != null) && this.Content is DropDownMenuGroup && (this.Content as DropDownMenuGroup).Items.Count != 0 || this.Content is FrameworkElement && this.Content is FrameworkElement) && (this._dropdownbutton != null && this._dropdownbutton.IsFocused || this._dropdownbuttonNormal != null && this._dropdownbuttonNormal.IsFocused || this.IsDropDownOpen && this._dropdownbutton != null && !this._dropdownbutton.IsFocused || this.IsDropDownOpen && this._dropdownbuttonNormal != null && !this._dropdownbuttonNormal.IsFocused))
        {
          if (this.IsDropDownOpen)
          {
            this.IsDropDownOpen = false;
            this.IsPressed = false;
            if (this._dropdownbutton != null)
              this._dropdownbutton.Focus();
            else if (this._dropdownbuttonNormal != null)
              this._dropdownbuttonNormal.Focus();
          }
          else
          {
            this.IsDropDownOpen = true;
            this.IsPressed = true;
            if (this.Content is FrameworkElement)
              (this.Content as FrameworkElement).Focus();
          }
        }
        e.Handled = true;
        break;
      case Key.Escape:
        if (this.IsDropDownOpen)
        {
          this.IsDropDownOpen = false;
          if (this._dropdownbutton != null)
          {
            this._dropdownbutton.Focus();
            break;
          }
          if (this._dropdownbuttonNormal != null)
          {
            this._dropdownbuttonNormal.Focus();
            break;
          }
          break;
        }
        break;
    }
    if (ModifierKeys.Alt != Keyboard.Modifiers || e.SystemKey != Key.Down && e.SystemKey != Key.Up)
      return;
    if (this._dropdownbutton != null)
    {
      this._dropdownbutton.Focus();
    }
    else
    {
      if (this._dropdownbuttonNormal == null)
        return;
      this._dropdownbuttonNormal.Focus();
    }
  }

  private void _dropdownbutton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    FrameworkElement content = this.Content as FrameworkElement;
    if (!this.IsDropDownOpen && content != null && !this.isopened)
    {
      this.IsPressed = true;
      this.IsDropDownOpen = true;
      content.Focus();
    }
    else
    {
      this.IsPressed = false;
      this.IsDropDownOpen = false;
      this.Focus();
    }
  }

  protected override System.Windows.Automation.Peers.AutomationPeer OnCreateAutomationPeer()
  {
    return (System.Windows.Automation.Peers.AutomationPeer) new SplitButtonAutomationPeer(this);
  }

  private static void OnIsDefaultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    SplitButtonAdv element = d as SplitButtonAdv;
    if ((bool) e.NewValue)
      AccessKeyManager.Register("\r", (IInputElement) element);
    else
      AccessKeyManager.Unregister("\r", (IInputElement) element);
  }

  public new void Dispose()
  {
    if (this._dropdown != null)
      this._dropdown.Closed -= new EventHandler(this._dropdown_Closed);
    if (this._button != null)
    {
      this._button.MouseLeftButtonDown -= new MouseButtonEventHandler(this._button_MouseLeftButtonDown);
      this._button.PreviewMouseLeftButtonDown -= new MouseButtonEventHandler(this._buttonNormal_PreviewMouseLeftButtonDown);
    }
    if (this._dropdownbutton != null)
      this._dropdownbutton.MouseLeftButtonUp -= new MouseButtonEventHandler(this._dropdownbutton_MouseLeftButtonUp);
    if (this._buttonNormal != null)
    {
      this._buttonNormal.MouseLeftButtonDown -= new MouseButtonEventHandler(this._button_MouseLeftButtonDown);
      this._buttonNormal.PreviewMouseLeftButtonDown -= new MouseButtonEventHandler(this._buttonNormal_PreviewMouseLeftButtonDown);
      this._buttonNormal.PreviewMouseLeftButtonUp -= new MouseButtonEventHandler(this._button_MouseLeftButtonUp);
    }
    if (this._dropdownbuttonNormal == null)
      return;
    this._dropdownbuttonNormal.MouseLeftButtonUp -= new MouseButtonEventHandler(this._dropdownbutton_MouseLeftButtonUp);
  }

  private static void CommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((SplitButtonAdv) d).HookUpCommand((ICommand) e.OldValue, (ICommand) e.NewValue);
  }

  private void HookUpCommand(ICommand oldCommand, ICommand newCommand)
  {
    if (oldCommand != null)
      this.RemoveCommand(oldCommand, newCommand);
    this.AddCommand(oldCommand, newCommand);
  }

  private void RemoveCommand(ICommand oldCommand, ICommand newCommand)
  {
    EventHandler eventHandler = new EventHandler(this.CanExecuteChanged);
    oldCommand.CanExecuteChanged -= eventHandler;
  }

  private void AddCommand(ICommand oldCommand, ICommand newCommand)
  {
    this.canExecuteChangedEventHandler = new EventHandler(this.CanExecuteChanged);
    if (newCommand == null)
      return;
    newCommand.CanExecuteChanged += this.canExecuteChangedEventHandler;
  }

  private void CanExecuteChanged(object sender, EventArgs e)
  {
    if (this.Command == null)
      return;
    if (this.Command is RoutedCommand command)
    {
      if (command.CanExecute(this.CommandParameter, this.CommandTarget))
        this.IsEnabled = true;
      else
        this.IsEnabled = false;
    }
    else if (!this.Command.CanExecute(this.CommandParameter))
      this.IsEnabled = false;
    else
      this.IsEnabled = true;
  }
}
