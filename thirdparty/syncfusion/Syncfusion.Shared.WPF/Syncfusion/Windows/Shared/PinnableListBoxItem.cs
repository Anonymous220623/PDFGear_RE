// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.PinnableListBoxItem
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

[SkinType(SkinVisualStyle = Skin.Metro, Type = typeof (PinnableListBoxItem), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/PinnableListBox/Themes/MetroStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Silver, Type = typeof (PinnableListBoxItem), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/PinnableListBox/Themes/Office2010SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Blue, Type = typeof (PinnableListBoxItem), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/PinnableListBox/Themes/Office2010BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Blue, Type = typeof (PinnableListBoxItem), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/PinnableListBox/Themes/Office2007BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Black, Type = typeof (PinnableListBoxItem), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/PinnableListBox/Themes/Office2007BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Silver, Type = typeof (PinnableListBoxItem), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/PinnableListBox/Themes/Office2007SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Blend, Type = typeof (PinnableListBoxItem), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/PinnableListBox/Themes/BlendStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Black, Type = typeof (PinnableListBoxItem), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/PinnableListBox/Themes/Office2010BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Default, Type = typeof (PinnableListBoxItem), XamlResource = "/Syncfusion.Shared.Wpf;component/Controls/PinnableListBox/Themes/Generic.xaml")]
[SkinType(SkinVisualStyle = Skin.VS2010, Type = typeof (PinnableListBoxItem), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/PinnableListBox/Themes/VS2010Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Transparent, Type = typeof (PinnableListBoxItem), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/PinnableListBox/Themes/TransparentStyle.xaml")]
public class PinnableListBoxItem : ContentControl, ICommandSource
{
  private ContextMenu cMenu;
  internal PinnableListBox pinnableListBox;
  public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof (Icon), typeof (ImageSource), typeof (PinnableListBoxItem), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty AddedTimeProperty = DependencyProperty.Register(nameof (AddedTime), typeof (DateTime), typeof (PinnableListBoxItem), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty IsPinnedProperty = DependencyProperty.Register(nameof (IsPinned), typeof (bool), typeof (PinnableListBoxItem), new PropertyMetadata((object) false, new PropertyChangedCallback(PinnableListBoxItem.OnIsPinnedChanged)));
  public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof (CornerRadius), typeof (Thickness), typeof (PinnableListBoxItem), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof (IsSelected), typeof (bool), typeof (PinnableListBoxItem), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
  public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(nameof (Description), typeof (string), typeof (PinnableListBoxItem), (PropertyMetadata) new UIPropertyMetadata((object) ""));
  public static readonly RoutedCommand PinCommand = new RoutedCommand(nameof (PinCommand), typeof (PinnableListBoxItem));
  public static readonly RoutedCommand RemoveCommand = new RoutedCommand(nameof (RemoveCommand), typeof (PinnableListBoxItem));
  public static readonly RoutedCommand ClearCommand = new RoutedCommand(nameof (ClearCommand), typeof (PinnableListBoxItem));
  public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof (Command), typeof (ICommand), typeof (PinnableListBoxItem), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(PinnableListBoxItem.OnCommandChanged)));
  public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof (CommandParameter), typeof (object), typeof (PinnableListBoxItem), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register(nameof (CommandTarget), typeof (IInputElement), typeof (PinnableListBoxItem), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));

  static PinnableListBoxItem()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (PinnableListBoxItem), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (PinnableListBoxItem)));
  }

  public PinnableListBoxItem()
  {
    this.DefaultStyleKey = (object) typeof (PinnableListBoxItem);
    this.Loaded += new RoutedEventHandler(this.PinnableListBoxItem_Loaded);
    this.Unloaded += new RoutedEventHandler(this.PinnableListBoxItem_Unloaded);
  }

  private void PinnableListBoxItem_Unloaded(object sender, RoutedEventArgs e)
  {
    if (Application.Current.MainWindow != null)
      this.Loaded -= new RoutedEventHandler(this.PinnableListBoxItem_Loaded);
    if (this.cMenu == null)
      return;
    this.cMenu.CommandBindings.Clear();
    this.cMenu.Items.Clear();
    this.cMenu = (ContextMenu) null;
  }

  private void PinnableListBoxItem_Loaded(object sender, RoutedEventArgs e)
  {
    CommandBinding commandBinding1 = new CommandBinding((ICommand) PinnableListBoxItem.PinCommand, new ExecutedRoutedEventHandler(PinnableListBoxItem.OnPinExecute), new CanExecuteRoutedEventHandler(PinnableListBoxItem.OnPinCanExecute));
    this.CommandBindings.Add(commandBinding1);
    this.cMenu = new ContextMenu();
    ItemCollection items1 = this.cMenu.Items;
    MenuItem menuItem1 = new MenuItem();
    menuItem1.Header = (object) "Pin to List";
    menuItem1.Command = (ICommand) PinnableListBoxItem.PinCommand;
    MenuItem newItem1 = menuItem1;
    items1.Add((object) newItem1);
    ItemCollection items2 = this.cMenu.Items;
    MenuItem menuItem2 = new MenuItem();
    menuItem2.Header = (object) "_Remove from list";
    menuItem2.Command = (ICommand) PinnableListBoxItem.RemoveCommand;
    MenuItem newItem2 = menuItem2;
    items2.Add((object) newItem2);
    ItemCollection items3 = this.cMenu.Items;
    MenuItem menuItem3 = new MenuItem();
    menuItem3.Header = (object) "Cl_ear unpinned Documents";
    menuItem3.Command = (ICommand) PinnableListBoxItem.ClearCommand;
    MenuItem newItem3 = menuItem3;
    items3.Add((object) newItem3);
    this.ContextMenu = this.cMenu;
    if (this.cMenu != null)
    {
      this.cMenu.Tag = (object) this;
      CommandBinding commandBinding2 = new CommandBinding((ICommand) PinnableListBoxItem.RemoveCommand, new ExecutedRoutedEventHandler(PinnableListBoxItem.OnRemoveExecute), new CanExecuteRoutedEventHandler(PinnableListBoxItem.OnRemoveCanExecute));
      CommandBinding commandBinding3 = new CommandBinding((ICommand) PinnableListBoxItem.ClearCommand, new ExecutedRoutedEventHandler(PinnableListBoxItem.OnClearExecute), new CanExecuteRoutedEventHandler(PinnableListBoxItem.OnCLearCanExecute));
      this.cMenu.CommandBindings.Add(commandBinding1);
      this.cMenu.CommandBindings.Add(commandBinding2);
      this.cMenu.CommandBindings.Add(commandBinding3);
    }
    if (this.ContextMenu == null)
      this.ContextMenu = this.cMenu;
    if (this.pinnableListBox == null)
      return;
    if (this.pinnableListBox.ItemsSource != null)
    {
      if (this.pinnableListBox.PinnedItems.Contains(this.DataContext) || !this.IsPinned)
        return;
      this.pinnableListBox.PinnedItems.Add(this.DataContext);
      if (!this.pinnableListBox.UnpinnedItems.Contains(this.DataContext))
        return;
      this.pinnableListBox.UnpinnedItems.Remove(this.DataContext);
    }
    else
    {
      if (this.pinnableListBox.PinnedItems.Contains((object) this) || !this.IsPinned)
        return;
      this.pinnableListBox.PinnedItems.Add((object) this);
    }
  }

  public ImageSource Icon
  {
    get => (ImageSource) this.GetValue(PinnableListBoxItem.IconProperty);
    set => this.SetValue(PinnableListBoxItem.IconProperty, (object) value);
  }

  public DateTime AddedTime
  {
    get => (DateTime) this.GetValue(PinnableListBoxItem.AddedTimeProperty);
    set => this.SetValue(PinnableListBoxItem.AddedTimeProperty, (object) value);
  }

  public bool IsPinned
  {
    get => (bool) this.GetValue(PinnableListBoxItem.IsPinnedProperty);
    set => this.SetValue(PinnableListBoxItem.IsPinnedProperty, (object) value);
  }

  public static void OnIsPinnedChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    if (obj is PinnableListBoxItem pinnableListBoxItem)
    {
      if (pinnableListBoxItem.pinnableListBox == null && pinnableListBoxItem.Parent is PinnableListBox)
        pinnableListBoxItem.pinnableListBox = pinnableListBoxItem.Parent as PinnableListBox;
      if (pinnableListBoxItem.pinnableListBox != null && !pinnableListBoxItem.pinnableListBox.isCalledByUpdateItems)
      {
        pinnableListBoxItem.pinnableListBox.pinnableItem = pinnableListBoxItem;
        if (pinnableListBoxItem.pinnableListBox.ItemsSource != null)
        {
          if (pinnableListBoxItem != null)
            pinnableListBoxItem.pinnableListBox.UpdatePinItems(pinnableListBoxItem.pinnableListBox, (object) pinnableListBoxItem, (bool) args.NewValue);
          else
            pinnableListBoxItem.pinnableListBox.UpdatePinItems(pinnableListBoxItem.pinnableListBox, pinnableListBoxItem.DataContext, (bool) args.NewValue);
        }
        else
          pinnableListBoxItem.pinnableListBox.UpdatePinItems(pinnableListBoxItem.pinnableListBox, (object) pinnableListBoxItem, (bool) args.NewValue);
      }
    }
    ((PinnableListBoxItem) obj)?.OnIsPinnedChanged(args);
  }

  protected void OnIsPinnedChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.ContextMenu == null)
    {
      this.cMenu = new ContextMenu();
      ItemCollection items1 = this.cMenu.Items;
      MenuItem menuItem1 = new MenuItem();
      menuItem1.Header = (object) "Pin to List";
      menuItem1.Command = (ICommand) PinnableListBoxItem.PinCommand;
      MenuItem newItem1 = menuItem1;
      items1.Add((object) newItem1);
      ItemCollection items2 = this.cMenu.Items;
      MenuItem menuItem2 = new MenuItem();
      menuItem2.Header = (object) "_Remove from list";
      menuItem2.Command = (ICommand) PinnableListBoxItem.RemoveCommand;
      MenuItem newItem2 = menuItem2;
      items2.Add((object) newItem2);
      ItemCollection items3 = this.cMenu.Items;
      MenuItem menuItem3 = new MenuItem();
      menuItem3.Header = (object) "Cl_ear unpinned Documents";
      menuItem3.Command = (ICommand) PinnableListBoxItem.ClearCommand;
      MenuItem newItem3 = menuItem3;
      items3.Add((object) newItem3);
    }
    if (this.pinnableListBox == null)
      return;
    this.pinnableListBox.FirePinStatusChanged();
  }

  public Thickness CornerRadius
  {
    get => (Thickness) this.GetValue(PinnableListBoxItem.CornerRadiusProperty);
    set => this.SetValue(PinnableListBoxItem.CornerRadiusProperty, (object) value);
  }

  public bool IsSelected
  {
    get => (bool) this.GetValue(PinnableListBoxItem.IsSelectedProperty);
    set => this.SetValue(PinnableListBoxItem.IsSelectedProperty, (object) value);
  }

  public string Description
  {
    get => (string) this.GetValue(PinnableListBoxItem.DescriptionProperty);
    set => this.SetValue(PinnableListBoxItem.DescriptionProperty, (object) value);
  }

  protected override void OnInitialized(EventArgs e) => base.OnInitialized(e);

  protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    this.IsSelected = true;
    if (this.pinnableListBox != null)
    {
      if (this.pinnableListBox.pinnableItem != null)
        this.pinnableListBox.pinnableItem.IsSelected = false;
      this.pinnableListBox.SelectedItem = (object) this;
      this.pinnableListBox.pinnableItem = this;
    }
    if (this.Command is RoutedCommand command)
      command.Execute(this.CommandParameter, this.CommandTarget);
    else if (this.Command != null)
      this.Command.Execute(this.CommandParameter);
    base.OnMouseLeftButtonUp(e);
  }

  private static void OnPinExecute(object sender, ExecutedRoutedEventArgs e)
  {
    if (!(e.Source is PinnableListBoxItem pinnableListBoxItem))
    {
      if (sender is PinnableListBoxItem)
        pinnableListBoxItem = sender as PinnableListBoxItem;
      else if (sender is ContextMenu)
        pinnableListBoxItem = ((ContextMenu) sender).PlacementTarget as PinnableListBoxItem;
    }
    if (pinnableListBoxItem == null)
      return;
    pinnableListBoxItem.IsPinned = !pinnableListBoxItem.IsPinned;
  }

  private static void OnPinCanExecute(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = true;
  }

  private static void OnRemoveExecute(object sender, ExecutedRoutedEventArgs args)
  {
    if (!(((ContextMenu) sender).PlacementTarget is PinnableListBoxItem placementTarget) || placementTarget.pinnableListBox == null)
      return;
    if (placementTarget.pinnableListBox.ItemsSource == null)
    {
      placementTarget.pinnableListBox.Items.Remove((object) placementTarget);
      if (!placementTarget.IsPinned)
        placementTarget.pinnableListBox.UnpinnedItems.Remove((object) placementTarget);
      else
        placementTarget.pinnableListBox.PinnedItems.Remove((object) placementTarget);
    }
    else
    {
      ((IList) placementTarget.pinnableListBox.ItemsSource).Remove(placementTarget.DataContext);
      if (placementTarget.IsPinned)
        placementTarget.pinnableListBox.PinnedItems.Remove(placementTarget.DataContext);
      else
        placementTarget.pinnableListBox.UnpinnedItems.Remove(placementTarget.DataContext);
    }
  }

  private static void OnRemoveCanExecute(object sender, CanExecuteRoutedEventArgs args)
  {
    args.CanExecute = true;
  }

  private static void OnClearExecute(object sender, ExecutedRoutedEventArgs args)
  {
    if (!(((FrameworkElement) sender).Tag is PinnableListBoxItem tag) || tag.pinnableListBox == null)
      return;
    if (tag.pinnableListBox.ItemsSource == null)
    {
      foreach (object unpinnedItem in (Collection<object>) tag.pinnableListBox.UnpinnedItems)
      {
        tag.pinnableListBox.Items.Remove(unpinnedItem);
        tag.pinnableListBox.UnpinnedItems.Remove(unpinnedItem);
      }
    }
    else
    {
      foreach (object obj in tag.pinnableListBox.UnpinnedItems.ToList<object>())
      {
        ((IList) tag.pinnableListBox.ItemsSource).Remove(obj);
        tag.pinnableListBox.UnpinnedItems.Remove(obj);
      }
    }
  }

  private static void OnCLearCanExecute(object sender, CanExecuteRoutedEventArgs args)
  {
    if (!(((FrameworkElement) sender).Tag is PinnableListBoxItem tag) || tag.pinnableListBox == null || tag.pinnableListBox.UnpinnedItems.Count <= 0)
      return;
    args.CanExecute = true;
  }

  public ICommand Command
  {
    get => (ICommand) this.GetValue(PinnableListBoxItem.CommandProperty);
    set => this.SetValue(PinnableListBoxItem.CommandProperty, (object) value);
  }

  public object CommandParameter
  {
    get => this.GetValue(PinnableListBoxItem.CommandParameterProperty);
    set => this.SetValue(PinnableListBoxItem.CommandParameterProperty, value);
  }

  public IInputElement CommandTarget
  {
    get => (IInputElement) this.GetValue(PinnableListBoxItem.CommandTargetProperty);
    set => this.SetValue(PinnableListBoxItem.CommandTargetProperty, (object) value);
  }

  private static void OnCommandChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs args)
  {
    ICommand oldValue = args.OldValue as ICommand;
    ICommand newValue = args.NewValue as ICommand;
    if (!(sender is PinnableListBoxItem pinnableListBoxItem))
      return;
    pinnableListBoxItem.HookCommand(oldValue, newValue);
  }

  private void HookCommand(ICommand olcommand, ICommand newommand)
  {
    if (olcommand != null)
    {
      EventHandler eventHandler = new EventHandler(this.CanExecuteChanged);
      olcommand.CanExecuteChanged -= eventHandler;
    }
    if (newommand == null)
      return;
    EventHandler eventHandler1 = new EventHandler(this.CanExecuteChanged);
    newommand.CanExecuteChanged += eventHandler1;
  }

  private void CanExecuteChanged(object sender, EventArgs e)
  {
    if (this.Command is RoutedCommand command)
    {
      if (command.CanExecute(this.CommandParameter, this.CommandTarget))
        this.IsEnabled = true;
      else
        this.IsEnabled = false;
    }
    else if (this.Command.CanExecute(this.CommandParameter))
      this.IsEnabled = true;
    else
      this.IsEnabled = false;
  }
}
