// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.SideMenuItem
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools.Extension;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

#nullable disable
namespace HandyControl.Controls;

public class SideMenuItem : HeaderedSimpleItemsControl, ISelectable, ICommandSource
{
  private bool _isMouseLeftButtonDown;
  public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof (Icon), typeof (object), typeof (SideMenuItem), new PropertyMetadata((object) null));
  internal static readonly DependencyProperty ExpandModeProperty = SideMenu.ExpandModeProperty.AddOwner(typeof (SideMenuItem), new PropertyMetadata((object) ExpandMode.ShowOne));
  public static readonly RoutedEvent SelectedEvent = EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (SideMenuItem));
  public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof (IsSelected), typeof (bool), typeof (SideMenuItem), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty RoleProperty = DependencyProperty.Register(nameof (Role), typeof (SideMenuItemRole), typeof (SideMenuItem), new PropertyMetadata((object) SideMenuItemRole.Header));
  public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof (Command), typeof (ICommand), typeof (SideMenuItem), new PropertyMetadata((object) null, new PropertyChangedCallback(SideMenuItem.OnCommandChanged)));
  public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof (CommandParameter), typeof (object), typeof (SideMenuItem), new PropertyMetadata((object) null));
  public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register(nameof (CommandTarget), typeof (IInputElement), typeof (SideMenuItem), new PropertyMetadata((object) null));

  public object Icon
  {
    get => this.GetValue(SideMenuItem.IconProperty);
    set => this.SetValue(SideMenuItem.IconProperty, value);
  }

  public SideMenuItem()
  {
    this.SetBinding(SideMenuItem.ExpandModeProperty, (BindingBase) new Binding(SideMenu.ExpandModeProperty.Name)
    {
      RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof (SideMenu), 1)
    });
  }

  internal ExpandMode ExpandMode
  {
    get => (ExpandMode) this.GetValue(SideMenuItem.ExpandModeProperty);
    set => this.SetValue(SideMenuItem.ExpandModeProperty, (object) value);
  }

  protected override void Refresh()
  {
    if (this.ItemsHost == null)
      return;
    this.ItemsHost.Children.Clear();
    foreach (object obj in this.Items)
    {
      DependencyObject element1;
      if (this.IsItemItsOwnContainerOverride(obj))
      {
        element1 = obj as DependencyObject;
      }
      else
      {
        element1 = this.GetContainerForItemOverride();
        this.PrepareContainerForItemOverride(element1, obj);
      }
      if (element1 is FrameworkElement element2)
      {
        element2.Style = this.ItemContainerStyle;
        this.ItemsHost.Children.Add((UIElement) element2);
      }
    }
    if (!this.IsLoaded)
      return;
    this.SwitchPanelArea(this.ExpandMode == ExpandMode.ShowAll || this.IsSelected);
  }

  protected virtual void OnSelected(RoutedEventArgs e)
  {
    this.RaiseEvent(e);
    ICommand command = this.Command;
    if (command == null)
      return;
    if (command is RoutedCommand routedCommand)
      routedCommand.Execute(this.CommandParameter, this.CommandTarget);
    else
      this.Command.Execute(this.CommandParameter);
  }

  public event RoutedEventHandler Selected
  {
    add => this.AddHandler(SideMenuItem.SelectedEvent, (Delegate) value);
    remove => this.RemoveHandler(SideMenuItem.SelectedEvent, (Delegate) value);
  }

  public bool IsSelected
  {
    get => (bool) this.GetValue(SideMenuItem.IsSelectedProperty);
    set => this.SetValue(SideMenuItem.IsSelectedProperty, ValueBoxes.BooleanBox(value));
  }

  public SideMenuItemRole Role
  {
    get => (SideMenuItemRole) this.GetValue(SideMenuItem.RoleProperty);
    internal set => this.SetValue(SideMenuItem.RoleProperty, (object) value);
  }

  protected override DependencyObject GetContainerForItemOverride()
  {
    return (DependencyObject) new SideMenuItem();
  }

  protected override bool IsItemItsOwnContainerOverride(object item) => item is SideMenuItem;

  protected override void OnMouseLeave(MouseEventArgs e)
  {
    base.OnMouseLeave(e);
    this._isMouseLeftButtonDown = false;
  }

  protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    base.OnMouseLeftButtonDown(e);
    this._isMouseLeftButtonDown = true;
  }

  protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    base.OnMouseLeftButtonUp(e);
    if (!this._isMouseLeftButtonDown)
      return;
    this.IsSelected = true;
    this.OnSelected(new RoutedEventArgs(SideMenuItem.SelectedEvent, (object) this));
    this._isMouseLeftButtonDown = false;
  }

  internal void SelectDefaultItem()
  {
    if (this.Role != SideMenuItemRole.Header || this.ItemsHost.Children.Count <= 0)
      return;
    SideMenuItem source = this.ItemsHost.Children.OfType<SideMenuItem>().FirstOrDefault<SideMenuItem>();
    if (source == null || source.IsSelected)
      return;
    source.OnSelected(new RoutedEventArgs(SideMenuItem.SelectedEvent, (object) source));
  }

  internal void SwitchPanelArea(bool isShow)
  {
    if (this.ItemsHost == null || this.Role != SideMenuItemRole.Header)
      return;
    this.ItemsHost.Show(isShow);
  }

  private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    SideMenuItem sideMenuItem = (SideMenuItem) d;
    if (e.OldValue is ICommand oldValue)
      oldValue.CanExecuteChanged -= new EventHandler(sideMenuItem.CanExecuteChanged);
    if (!(e.NewValue is ICommand newValue))
      return;
    newValue.CanExecuteChanged += new EventHandler(sideMenuItem.CanExecuteChanged);
  }

  public ICommand Command
  {
    get => (ICommand) this.GetValue(SideMenuItem.CommandProperty);
    set => this.SetValue(SideMenuItem.CommandProperty, (object) value);
  }

  public object CommandParameter
  {
    get => this.GetValue(SideMenuItem.CommandParameterProperty);
    set => this.SetValue(SideMenuItem.CommandParameterProperty, value);
  }

  public IInputElement CommandTarget
  {
    get => (IInputElement) this.GetValue(SideMenuItem.CommandTargetProperty);
    set => this.SetValue(SideMenuItem.CommandTargetProperty, (object) value);
  }

  private void CanExecuteChanged(object sender, EventArgs e)
  {
    if (this.Command == null)
      return;
    this.IsEnabled = this.Command is RoutedCommand command ? command.CanExecute(this.CommandParameter, this.CommandTarget) : this.Command.CanExecute(this.CommandParameter);
  }
}
