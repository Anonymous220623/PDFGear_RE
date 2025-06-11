// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.DropDownMenuItem
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Controls.Tools.AutomationPeer;
using Syncfusion.Windows.Shared;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

[SkinType(SkinVisualStyle = Skin.VS2010, Type = typeof (DropDownMenuItem), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/VS2010Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Black, Type = typeof (DropDownMenuItem), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/Office2007BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Blue, Type = typeof (DropDownMenuItem), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/Office2010BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Black, Type = typeof (DropDownMenuItem), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/Office2010BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Silver, Type = typeof (DropDownMenuItem), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/Office2010SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Blend, Type = typeof (DropDownMenuItem), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/BlendStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Blue, Type = typeof (DropDownMenuItem), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/Office2007BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyRed, Type = typeof (DropDownMenuItem), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/ShinyRedStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyBlue, Type = typeof (DropDownMenuItem), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/ShinyBlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Default, Type = typeof (DropDownMenuItem), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/ButtonControls/DropDownButton/Themes/DropDownButton.xaml")]
[SkinType(SkinVisualStyle = Skin.Metro, Type = typeof (DropDownMenuItem), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/MetroStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Transparent, Type = typeof (DropDownMenuItem), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/TransparentStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Silver, Type = typeof (DropDownMenuItem), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/Office2007SilverStyle.xaml")]
public class DropDownMenuItem : HeaderedItemsControl, ICommandSource, IDisposable
{
  internal Popup _dropdown;
  internal ColumnDefinition _column;
  private ColumnDefinition subItemPathColumn;
  private EventHandler CanExecuteChangedHandler;
  public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(nameof (IconSize), typeof (Size), typeof (DropDownMenuItem), new PropertyMetadata((object) new Size(16.0, 16.0)));
  public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register(nameof (IsPressed), typeof (bool), typeof (DropDownMenuItem), new PropertyMetadata((object) false));
  public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof (Icon), typeof (object), typeof (DropDownMenuItem), new PropertyMetadata((PropertyChangedCallback) null));
  public new static readonly DependencyProperty HasItemsProperty = DependencyProperty.Register(nameof (HasItems), typeof (bool), typeof (DropDownMenuItem), new PropertyMetadata((object) false));
  public static readonly DependencyProperty IsCheckableProperty = DependencyProperty.Register(nameof (IsCheckable), typeof (bool), typeof (DropDownMenuItem), new PropertyMetadata((object) false, new PropertyChangedCallback(DropDownMenuItem.OnIsCheckableChanged)));
  public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof (IsChecked), typeof (bool), typeof (DropDownMenuItem), new PropertyMetadata((object) false, new PropertyChangedCallback(DropDownMenuItem.OnIsCheckedChanged)));
  public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof (Command), typeof (ICommand), typeof (DropDownMenuItem), new PropertyMetadata((object) null, new PropertyChangedCallback(DropDownMenuItem.OnCommandChanged)));
  public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof (CommandParameter), typeof (object), typeof (DropDownMenuItem), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register(nameof (CommandTarget), typeof (IInputElement), typeof (DropDownMenuItem), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));

  public event PropertyChangedCallback IsCheckedChanged;

  public event RoutedEventHandler Click;

  internal DropDownMenuItem ParentRibbonMenuItem { get; set; }

  internal DropDownButtonAdv ParentDropDown
  {
    get
    {
      Visual ancestor = VisualUtils.FindAncestor((Visual) this, typeof (DropDownMenuGroup));
      return ancestor != null ? ((FrameworkElement) ancestor).Parent as DropDownButtonAdv : (DropDownButtonAdv) null;
    }
  }

  public DropDownMenuItem()
  {
    this.DefaultStyleKey = (object) typeof (DropDownMenuItem);
    this.Loaded += new RoutedEventHandler(this.DropDownMenuItem_Loaded);
  }

  private void DropDownMenuItem_Loaded(object sender, RoutedEventArgs e)
  {
    if (VisualUtils.FindAncestor((Visual) this, typeof (DropDownMenuGroup)) is DropDownMenuGroup ancestor && this._column != null)
      this._column.Width = new GridLength(ancestor.maximumTrayBarWidth + 7.0);
    if (ancestor == null)
      return;
    this.RemoveSubMenuItemArrow();
  }

  public object Icon
  {
    get => this.GetValue(DropDownMenuItem.IconProperty);
    set => this.SetValue(DropDownMenuItem.IconProperty, value);
  }

  public Size IconSize
  {
    get => (Size) this.GetValue(DropDownMenuItem.IconSizeProperty);
    set => this.SetValue(DropDownMenuItem.IconSizeProperty, (object) value);
  }

  public bool IsPressed
  {
    get => (bool) this.GetValue(DropDownMenuItem.IsPressedProperty);
    set => this.SetValue(DropDownMenuItem.IsPressedProperty, (object) value);
  }

  public new bool HasItems
  {
    get => (bool) this.GetValue(DropDownMenuItem.HasItemsProperty);
    internal set => this.SetValue(DropDownMenuItem.HasItemsProperty, (object) value);
  }

  [Category("Common Properties")]
  [Description("Represents the value, whether the element can be checkable or not")]
  public bool IsCheckable
  {
    get => (bool) this.GetValue(DropDownMenuItem.IsCheckableProperty);
    set => this.SetValue(DropDownMenuItem.IsCheckableProperty, (object) value);
  }

  [Description("Represents the value, whether the element is checked or not")]
  [Category("Common Properties")]
  public bool IsChecked
  {
    get => (bool) this.GetValue(DropDownMenuItem.IsCheckedProperty);
    set => this.SetValue(DropDownMenuItem.IsCheckedProperty, (object) value);
  }

  private static void OnCommandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
  {
    ((DropDownMenuItem) obj).OnCommandChanged((ICommand) e.OldValue, (ICommand) e.NewValue);
  }

  private void OnCommandChanged(ICommand oldCommand, ICommand newCommand)
  {
    EventHandler eventHandler = new EventHandler(this.OnCanExecuteChanged);
    if (oldCommand != null)
      oldCommand.CanExecuteChanged -= eventHandler;
    if (newCommand == null)
      return;
    this.UpdateCanExecute();
    this.CanExecuteChangedHandler = eventHandler;
    newCommand.CanExecuteChanged += eventHandler;
  }

  private static void OnIsCheckableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as DropDownMenuItem).OnIsCheckableChanged(e);
  }

  private void OnIsCheckableChanged(DependencyPropertyChangedEventArgs e)
  {
    if ((bool) e.NewValue || !this.IsChecked)
      return;
    this.IsChecked = false;
  }

  private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    DropDownMenuItem d1 = d as DropDownMenuItem;
    if (d1.IsCheckedChanged == null)
      return;
    d1.IsCheckedChanged((DependencyObject) d1, e);
  }

  protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
  {
    this.HasItems = this.Items.Count > 0;
    if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems[0] is DropDownMenuItem newItem)
      newItem.ParentRibbonMenuItem = this;
    base.OnItemsChanged(e);
  }

  protected override System.Windows.Automation.Peers.AutomationPeer OnCreateAutomationPeer()
  {
    return (System.Windows.Automation.Peers.AutomationPeer) new DropDownMenuItemAutomationPeer(this);
  }

  [Category("Common Properties")]
  public ICommand Command
  {
    get => (ICommand) this.GetValue(DropDownMenuItem.CommandProperty);
    set => this.SetValue(DropDownMenuItem.CommandProperty, (object) value);
  }

  [Category("Common Properties")]
  public object CommandParameter
  {
    get => this.GetValue(DropDownMenuItem.CommandParameterProperty);
    set => this.SetValue(DropDownMenuItem.CommandParameterProperty, value);
  }

  public IInputElement CommandTarget
  {
    get => (IInputElement) this.GetValue(DropDownMenuItem.CommandTargetProperty);
    set => this.SetValue(DropDownMenuItem.CommandTargetProperty, (object) value);
  }

  private void UpdateCanExecute()
  {
    if (this.Command == null || ItemsControl.ItemsControlFromItemContainer((DependencyObject) this) is DropDownMenuItem)
      return;
    if (!DropDownMenuItem.CanExecuteCommandSource(this))
      this.IsEnabled = false;
    else
      this.IsEnabled = true;
  }

  private void OnCanExecuteChanged(object sender, EventArgs e) => this.UpdateCanExecute();

  private static bool CanExecuteCommandSource(DropDownMenuItem commandSource)
  {
    ICommand command = commandSource.Command;
    object commandParameter = commandSource.CommandParameter;
    if (command == null)
      return false;
    RoutedCommand routedCommand = command as RoutedCommand;
    IInputElement target = commandSource.CommandTarget;
    if (routedCommand == null)
      return command.CanExecute(commandParameter);
    if (target == null)
      target = (IInputElement) commandSource;
    return routedCommand.CanExecute(commandParameter, target);
  }

  private void RemoveSubMenuItemArrow()
  {
    DropDownMenuGroup ancestor = VisualUtils.FindAncestor((Visual) this, typeof (DropDownMenuGroup)) as DropDownMenuGroup;
    bool flag = false;
    for (int index1 = 0; index1 < ancestor.Items.Count; ++index1)
    {
      if (ancestor.Items[index1] is DropDownMenuItem dropDownMenuItem)
      {
        for (int index2 = 0; index2 < dropDownMenuItem.Items.Count; ++index2)
        {
          if ((dropDownMenuItem.Items[index2] as DropDownMenuGroup).Items.Count > 0)
          {
            flag = true;
            break;
          }
        }
      }
      else
      {
        flag = true;
        break;
      }
    }
    if (flag || this.subItemPathColumn == null)
      return;
    this.subItemPathColumn.Width = new GridLength(5.0);
  }

  protected virtual void OnClick()
  {
    if (VisualUtils.FindLogicalAncestor((Visual) this, typeof (DropDownButtonAdv)) is DropDownButtonAdv logicalAncestor)
      logicalAncestor.dropitem = this;
    if (this.Click != null)
      this.Click((object) this, new RoutedEventArgs());
    if (this.Command != null)
    {
      if (this.Command.CanExecute(this.CommandParameter))
      {
        this.Command.Execute(this.CommandParameter);
      }
      else
      {
        IInputElement target = this.CommandTarget != null ? this.CommandTarget : (IInputElement) this;
        if (this.Command is RoutedCommand && (this.Command as RoutedCommand).CanExecute(this.CommandParameter, target))
          (this.Command as RoutedCommand).Execute(this.CommandParameter, target);
      }
    }
    if (this.ParentRibbonMenuItem != null)
    {
      this.ParentRibbonMenuItem._dropdown.IsOpen = false;
      if (this.ParentRibbonMenuItem.ParentDropDown != null && this.Items.Count == 0 && !this.ParentRibbonMenuItem.ParentDropDown.StayDropDownOnClick)
        this.ParentRibbonMenuItem.ParentDropDown.IsDropDownOpen = false;
    }
    if (this.ParentDropDown != null && this.Items.Count == 0 && !this.ParentDropDown.StayDropDownOnClick)
      this.ParentDropDown.IsDropDownOpen = false;
    if (this.Parent == null || !(this.Parent is DropDownMenuItem parent))
      return;
    parent._dropdown.IsOpen = false;
  }

  protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    this.IsPressed = true;
    if (!this.IsCheckable)
      return;
    this.IsChecked = !this.IsChecked;
  }

  protected override void OnKeyUp(KeyEventArgs e)
  {
    if (e.Key == Key.Return || e.Key == Key.Space)
    {
      if (!this.IsCheckable)
        this.OnClick();
      if (this.IsCheckable)
        this.IsChecked = !this.IsChecked;
    }
    base.OnKeyUp(e);
  }

  protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    this.IsPressed = false;
    if (!(e.OriginalSource is ICommandSource))
      this.OnClick();
    if (this.ParentDropDown != null && !this.ParentDropDown.StayDropDownOnClick)
      this.ParentDropDown.IsDropDownOpen = false;
    e.Handled = true;
    base.OnMouseLeftButtonUp(e);
  }

  public override void OnApplyTemplate()
  {
    this._dropdown = this.GetTemplateChild("PART_DropDown") as Popup;
    this._column = this.GetTemplateChild("Icontraywidth") as ColumnDefinition;
    this.subItemPathColumn = this.GetTemplateChild("SubItemPath") as ColumnDefinition;
    if (VisualUtils.FindAncestor((Visual) this, typeof (DropDownMenuGroup)) is DropDownMenuGroup ancestor && this._column != null)
      this._column.Width = new GridLength(ancestor.maximumTrayBarWidth + 7.0);
    base.OnApplyTemplate();
  }

  protected override void OnMouseEnter(MouseEventArgs e)
  {
    if (this.Items.Count <= 0 && this._dropdown != null)
      this._dropdown.IsOpen = false;
    if (this.Parent is DropDownMenuGroup parent)
    {
      foreach (object obj in (IEnumerable) parent.Items)
      {
        if (obj is DropDownMenuItem && (obj as DropDownMenuItem)._dropdown != null && (obj as DropDownMenuItem)._dropdown.IsOpen)
          (obj as DropDownMenuItem)._dropdown.IsOpen = false;
      }
    }
    else if (this.Parent is DropDownMenuItem)
    {
      foreach (object obj in (IEnumerable) (this.Parent as DropDownMenuItem).Items)
      {
        if (obj is DropDownMenuItem && (obj as DropDownMenuItem)._dropdown != null && (obj as DropDownMenuItem)._dropdown.IsOpen)
          (obj as DropDownMenuItem)._dropdown.IsOpen = false;
      }
    }
    if (this._dropdown != null && !this._dropdown.IsOpen && this.Items.Count > 0)
      this._dropdown.IsOpen = true;
    if (this.ParentRibbonMenuItem == null)
      return;
    this.ParentRibbonMenuItem._dropdown.IsOpen = true;
  }

  protected override void OnMouseLeave(MouseEventArgs e)
  {
    if (this._dropdown == null || !this._dropdown.IsOpen || this.GetType().ToString().Contains("RibbonMenuItem") || this.GetType().ToString().Contains(nameof (DropDownMenuItem)))
      return;
    this._dropdown.IsOpen = false;
  }

  internal void Dispose(bool disposing)
  {
    if (!disposing)
      return;
    this.Loaded -= new RoutedEventHandler(this.DropDownMenuItem_Loaded);
    if (this._dropdown != null)
      this._dropdown = (Popup) null;
    this.Click = (RoutedEventHandler) null;
    if (this.ParentRibbonMenuItem == null)
      return;
    this.ParentRibbonMenuItem = (DropDownMenuItem) null;
  }

  public void Dispose() => this.Dispose(true);
}
