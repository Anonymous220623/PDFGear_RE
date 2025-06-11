// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.ComboBoxItemAdv
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class ComboBoxItemAdv : ContentControl
{
  internal CheckBox CheckBox;
  public static readonly RoutedEvent SelectedEvent = EventManager.RegisterRoutedEvent(nameof (SelectedEvent), RoutingStrategy.Direct, typeof (RoutedEventHandler), typeof (ComboBoxItemAdv));
  public static readonly RoutedEvent UnSelectedEvent = EventManager.RegisterRoutedEvent(nameof (UnSelectedEvent), RoutingStrategy.Direct, typeof (RoutedEventHandler), typeof (ComboBoxItemAdv));
  public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof (IsSelected), typeof (bool), typeof (ComboBoxItemAdv), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(ComboBoxItemAdv.OnIsSelectedChanged)));
  internal static readonly DependencyProperty MultiSelectProperty = DependencyProperty.Register(nameof (MultiSelect), typeof (bool), typeof (ComboBoxItemAdv), (PropertyMetadata) new FrameworkPropertyMetadata((object) false));
  public static readonly DependencyProperty IsHighlightedProperty = DependencyProperty.Register(nameof (IsHighlighted), typeof (bool), typeof (ComboBoxItemAdv), new PropertyMetadata((object) false));
  public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register(nameof (IsPressed), typeof (bool), typeof (ComboBoxItemAdv), new PropertyMetadata((object) false));

  public ComboBoxItemAdv() => this.DefaultStyleKey = (object) typeof (ComboBoxItemAdv);

  internal ComboBoxAdv Parent
  {
    get
    {
      parent = (ComboBoxAdv) null;
      if (!this.IsSelectAll)
      {
        ItemsControl container = this.ParentItemsControl;
        while (true)
        {
          switch (container)
          {
            case null:
            case ComboBoxAdv parent:
              goto label_5;
            default:
              container = ItemsControl.ItemsControlFromItemContainer((DependencyObject) container);
              continue;
          }
        }
      }
      else
        parent = this.TemplatedParent as ComboBoxAdv;
label_5:
      if (parent != null)
        this.MultiSelect = parent.AllowMultiSelect;
      return parent;
    }
  }

  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public bool MultiSelect
  {
    get => (bool) this.GetValue(ComboBoxItemAdv.MultiSelectProperty);
    set => this.SetValue(ComboBoxItemAdv.MultiSelectProperty, (object) value);
  }

  public ItemsControl ParentItemsControl
  {
    get => ItemsControl.ItemsControlFromItemContainer((DependencyObject) this);
  }

  protected internal bool IsSelectAll { get; set; }

  public event RoutedEventHandler Selected
  {
    add => this.AddHandler(ComboBoxItemAdv.SelectedEvent, (Delegate) value);
    remove => this.RemoveHandler(ComboBoxItemAdv.SelectedEvent, (Delegate) value);
  }

  public event RoutedEventHandler UnSelected
  {
    add => this.AddHandler(ComboBoxItemAdv.UnSelectedEvent, (Delegate) value);
    remove => this.RemoveHandler(ComboBoxItemAdv.SelectedEvent, (Delegate) value);
  }

  public bool IsSelected
  {
    get => (bool) this.GetValue(ComboBoxItemAdv.IsSelectedProperty);
    set => this.SetValue(ComboBoxItemAdv.IsSelectedProperty, (object) value);
  }

  public bool IsHighlighted
  {
    get => (bool) this.GetValue(ComboBoxItemAdv.IsHighlightedProperty);
    internal set => this.SetValue(ComboBoxItemAdv.IsHighlightedProperty, (object) value);
  }

  public bool IsPressed
  {
    get => (bool) this.GetValue(ComboBoxItemAdv.IsPressedProperty);
    protected set => this.SetValue(ComboBoxItemAdv.IsPressedProperty, (object) value);
  }

  protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    base.OnMouseLeftButtonDown(e);
    this.IsPressed = true;
  }

  protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    base.OnMouseLeftButtonUp(e);
    this.UpdateSelection();
    if (this.Parent != null && !this.Parent.AllowMultiSelect && this.Parent.IsDropDownOpen)
      this.Parent.IsDropDownOpen = false;
    this.IsPressed = false;
  }

  internal void UpdateCheckBoxBinding()
  {
    Binding binding = new Binding()
    {
      Path = new PropertyPath("IsSelected", new object[0]),
      Mode = BindingMode.TwoWay,
      Source = (object) this
    };
    if (this.CheckBox == null)
      return;
    this.CheckBox.SetBinding(ToggleButton.IsCheckedProperty, (BindingBase) binding);
  }

  internal void UpdateSelection()
  {
    if (!this.IsSelected)
    {
      if (this.Content != null)
        this.IsSelected = true;
      if (this.Parent == null || this.Parent.AllowMultiSelect)
        return;
      for (int index = 0; index < this.Parent.Items.Count; ++index)
      {
        if (!(this.Parent.ItemContainerGenerator.ContainerFromIndex(index) is ComboBoxItemAdv comboBoxItemAdv))
          comboBoxItemAdv = this.Parent.Items[index] as ComboBoxItemAdv;
        if (comboBoxItemAdv != null && comboBoxItemAdv != this)
          comboBoxItemAdv.IsSelected = false;
      }
      this.IsSelected = false;
    }
    else
    {
      if (!this.Parent.AllowMultiSelect)
        return;
      this.IsSelected = false;
    }
  }

  protected override void OnGotFocus(RoutedEventArgs e)
  {
    base.OnGotFocus(e);
    Syncfusion.Windows.VisualStateManager.GoToState((Control) this, "Focussed", true);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.CheckBox = this.GetTemplateChild("PART_CheckBox") as CheckBox;
    this.IsSelectAll = this.TemplatedParent is ComboBoxAdv;
    if (this.CheckBox != null)
    {
      this.CheckBox.Checked -= new RoutedEventHandler(this.CheckBox_Checked);
      this.CheckBox.Unchecked -= new RoutedEventHandler(this.CheckBox_Unchecked);
      this.CheckBox.Checked += new RoutedEventHandler(this.CheckBox_Checked);
      this.CheckBox.Unchecked += new RoutedEventHandler(this.CheckBox_Unchecked);
    }
    if (!this.IsSelectAll)
      this.UpdateCheckBoxBinding();
    if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Minor == 1 && Environment.OSVersion.Version.Major == 6 && !Standard.NativeMethods.DwmIsCompositionEnabled())
    {
      Brush resource1 = this.FindResource((object) SystemColors.InactiveBorderBrushKey) as Brush;
      Brush resource2 = this.FindResource((object) SystemColors.InactiveCaptionTextBrushKey) as Brush;
      if (resource1 != null && resource2 != null && resource1.ToString() == resource2.ToString())
        this.Parent.InactiveBrush = this.FindResource((object) SystemColors.InactiveCaptionBrushKey) as Brush;
    }
    if (this.Parent == null)
      return;
    this.Parent.UpdateItemSelectMode(this);
    if (!this.Parent.AllowMultiSelect)
      return;
    this.Parent.removeFlag = false;
    if (this.Parent.newItem != null && this.Parent.Items.Contains(this.Parent.newItem))
    {
      foreach (object obj1 in (IEnumerable) this.Parent.SelItemsInternal)
      {
        foreach (object obj2 in (IEnumerable) this.Parent.Items)
        {
          if (obj2 != null && obj2.Equals(obj1) && this.Parent.newItem.Equals(obj2))
          {
            if (this.Parent.itemcount >= 1 && (this.Parent.ItemContainerGenerator.ContainerFromItem(this.Parent.newItem) is ComboBoxItemAdv comboBoxItemAdv && comboBoxItemAdv.IsSelected || comboBoxItemAdv == null))
              this.Parent.removeFlag = true;
            ++this.Parent.itemcount;
          }
        }
      }
      if (this.Parent.removeFlag && this.Parent.SelItemsInternal.Count > 0)
        this.Parent.SelItemsInternal.Remove(this.Parent.newItem);
      this.Parent.SelectItems();
      this.Parent.oldItem = (object) null;
      this.Parent.itemcount = 0;
    }
    if (this.Parent.oldItem != null && this.DataContext != null)
    {
      if (this.DataContext.Equals(this.Parent.oldItem))
      {
        if (this.CheckBox == null)
          return;
        this.Parent.internalSelect = true;
        this.CheckBox.IsChecked = new bool?(false);
        this.Parent.internalSelect = false;
      }
      else
        this.Parent.SelectItems();
    }
    else
      this.Parent.SelectItems();
  }

  private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
  {
    if (this.Parent != null && !this.Parent.internalSelect && this.IsPressed)
      this.IsSelected = false;
    if (this.Parent == null || !this.Parent.AllowMultiSelect || this.Parent.EnableOKCancel || this.IsSelectAll || this.Parent.internalSelect)
      return;
    ObservableCollection<object> observableCollection = this.Parent.SelectedItems != null ? new ObservableCollection<object>(this.Parent.SelectedItems.Cast<object>()) : (ObservableCollection<object>) null;
    if (observableCollection != null && observableCollection.Count <= 0)
      this.Parent.SelectedIndex = -1;
    this.Parent.UpdateSelectionBox();
  }

  private void CheckBox_Checked(object sender, RoutedEventArgs e)
  {
    if (this.Parent == null || this.Parent.internalSelect || this.Content == null)
      return;
    this.IsSelected = true;
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    base.OnMouseMove(e);
    this.Parent.IsGotKeyBoardFocus = false;
    Syncfusion.Windows.VisualStateManager.GoToState((Control) this, "MouseOver", true);
  }

  protected override void OnMouseEnter(MouseEventArgs e)
  {
    if (this.Parent != null && !this.Parent.IsGotKeyBoardFocus)
      this.Parent.NotifyComboBoxItemAdvEnter(this, true);
    base.OnMouseEnter(e);
  }

  protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
  {
    if (this.Parent != null)
    {
      this.Parent.IsGotKeyBoardFocus = true;
      this.Parent.NotifyComboBoxItemAdvEnter(this, true);
    }
    base.OnGotKeyboardFocus(e);
  }

  protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
  {
    if (this.Parent != null)
      this.Parent.NotifyComboBoxItemAdvEnter(this, false);
    base.OnLostKeyboardFocus(e);
  }

  protected override void OnMouseLeave(MouseEventArgs e)
  {
    base.OnMouseLeave(e);
    Syncfusion.Windows.VisualStateManager.GoToState((Control) this, "Normal", true);
    if (this.Parent != null && !this.Parent.IsGotKeyBoardFocus)
      this.Parent.NotifyComboBoxItemAdvEnter(this, false);
    if (this.IsSelected)
      return;
    this.IsPressed = false;
  }

  internal void UpdateParentSelectedItems()
  {
    object obj = (object) null;
    if ((this.Parent.ItemsSource != null || this.Parent.Items.Count > 0 && !(this.Parent.Items[0] is ComboBoxItemAdv)) && this.DataContext != null)
      obj = this.DataContext;
    else if (this.Content != null)
      obj = (object) this;
    if (obj == null)
      return;
    bool flag = false;
    if (this.IsSelected)
    {
      if (!this.Parent.SelItemsInternal.Contains(obj))
      {
        this.Parent.SelItemsInternal.Add(obj);
        flag = true;
      }
    }
    else if (this.Parent.SelItemsInternal.Contains(obj))
    {
      this.Parent.SelItemsInternal.Remove(obj);
      flag = true;
    }
    if (!flag)
      return;
    if (this.Parent.ChangedItems.Contains(obj))
      this.Parent.ChangedItems.Remove(obj);
    else
      this.Parent.ChangedItems.Add(obj);
  }

  private static void OnIsSelectedChanged(object sender, DependencyPropertyChangedEventArgs args)
  {
    ComboBoxItemAdv comboBoxItemAdv1 = sender as ComboBoxItemAdv;
    ComboBoxAdv parent = comboBoxItemAdv1.Parent;
    if (comboBoxItemAdv1 == null)
      return;
    if (comboBoxItemAdv1.IsSelected)
      comboBoxItemAdv1.RaiseEvent(new RoutedEventArgs()
      {
        RoutedEvent = ComboBoxItemAdv.SelectedEvent,
        Source = (object) comboBoxItemAdv1
      });
    else
      comboBoxItemAdv1.RaiseEvent(new RoutedEventArgs()
      {
        RoutedEvent = ComboBoxItemAdv.UnSelectedEvent,
        Source = (object) comboBoxItemAdv1
      });
    if (parent != null && parent.AllowMultiSelect)
    {
      if (comboBoxItemAdv1.IsSelectAll && !parent.internalSelect)
      {
        bool isSelected = comboBoxItemAdv1.IsSelected;
        parent.internalSelect = true;
        parent.internalChange = true;
        foreach (object obj in (IEnumerable) parent.Items)
        {
          ComboBoxItemAdv comboBoxItemAdv2 = parent.ItemContainerGenerator.ContainerFromItem(obj) as ComboBoxItemAdv;
          if (parent.SelItemsInternal.Contains(obj))
            parent.SelItemsInternal.Remove(obj);
          if (comboBoxItemAdv2 != null)
            comboBoxItemAdv2.IsSelected = isSelected;
          bool flag = false;
          if (isSelected && !parent.SelItemsInternal.Contains(obj))
          {
            if (parent.AllowMultiSelect && parent.EnableToken && !parent.EnableOKCancel)
            {
              if (!parent.SelItemsInternal.Contains(obj))
                parent.SelItemsInternal.Add(obj);
              flag = true;
            }
            else
            {
              parent.SelItemsInternal.Add(obj);
              flag = true;
            }
          }
          else if (!isSelected && parent.SelItemsInternal.Contains(obj))
          {
            if (parent.AllowMultiSelect && parent.EnableToken && !parent.EnableOKCancel)
            {
              if (parent.SelItemsInternal.Contains(obj))
                parent.SelItemsInternal.Remove(obj);
              flag = true;
            }
            else
            {
              parent.SelItemsInternal.Remove(obj);
              flag = true;
            }
          }
          if (flag)
          {
            if (parent.ChangedItems.Contains(obj))
              parent.ChangedItems.Remove(obj);
            else
              parent.ChangedItems.Add(obj);
          }
        }
        if (comboBoxItemAdv1.IsSelected && parent.ChangedItems.Count > 0)
          parent.OnItemChecked((object) comboBoxItemAdv1, new ObservableCollection<object>(parent.SelItemsInternal.Cast<object>()));
        else if (!comboBoxItemAdv1.IsSelected && parent.ChangedItems.Count > 0)
          parent.OnItemUnchecked((object) comboBoxItemAdv1, new ObservableCollection<object>(parent.SelItemsInternal.Cast<object>()));
        if (parent.SelectedItems == null && parent.SelItemsInternal.Count > 0)
          parent.SelectedItems = (IEnumerable) parent.SelItemsInternal;
        parent.internalChange = false;
        parent.internalSelect = false;
        parent.UpdateSelectAllItemState();
        if (!parent.EnableOKCancel)
          parent.UpdateSelectedItems();
      }
      else if (!comboBoxItemAdv1.IsSelectAll && !parent.internalSelect)
      {
        if (parent.EnableOKCancel)
        {
          parent.internalChange = true;
          comboBoxItemAdv1.UpdateParentSelectedItems();
          ObservableCollection<object> observableCollection1 = new ObservableCollection<object>(parent.SelItemsInternal.Cast<object>());
          ObservableCollection<object> observableCollection2 = new ObservableCollection<object>((IEnumerable<object>) observableCollection1);
          ObservableCollection<object> observableCollection3 = comboBoxItemAdv1.IsSelected ? (parent.ChangedItems.Count > 0 ? parent.OnItemChecked((object) comboBoxItemAdv1, observableCollection1) : (ObservableCollection<object>) null) : (parent.ChangedItems.Count > 0 ? parent.OnItemUnchecked((object) comboBoxItemAdv1, observableCollection1) : (ObservableCollection<object>) null);
          if (parent.SelItemsInternal.Count > 0 && parent.ChangedItems.Count > 0)
          {
            foreach (object obj in (Collection<object>) observableCollection3)
            {
              if (!parent.ChangedItems.Contains(obj) && !parent.SelItemsInternal.Contains(obj))
                parent.ChangedItems.Add(obj);
            }
            if (!comboBoxItemAdv1.IsSelected)
            {
              foreach (object obj in (Collection<object>) observableCollection2)
              {
                if (!parent.ChangedItems.Contains(obj) && !observableCollection3.Contains(obj))
                  parent.ChangedItems.Add(obj);
              }
            }
            if (observableCollection3 != null)
              parent.SelectedItems = (IEnumerable) observableCollection3;
          }
          parent.internalChange = false;
        }
        parent.UpdateSelectAllItemState();
      }
      if (comboBoxItemAdv1.IsSelectAll || parent.EnableOKCancel)
        return;
    }
    if (parent == null)
      return;
    if (!parent.AllowMultiSelect)
      parent.SelItemsInternal.Clear();
    if (parent.AllowMultiSelect && parent.SelectedItems == null && parent.SelItemsInternal != null)
      parent.SelectedItems = (IEnumerable) parent.SelItemsInternal;
    if (!parent.internalSelect)
    {
      if ((parent.ItemsSource != null || parent.Items.Count > 0 && !(parent.Items[0] is ComboBoxItemAdv)) && comboBoxItemAdv1.DataContext != null)
      {
        if (comboBoxItemAdv1.IsSelected)
          parent.SelItemsInternal.Add(comboBoxItemAdv1.DataContext);
        else if (parent.SelItemsInternal.Contains(comboBoxItemAdv1.DataContext))
          parent.SelItemsInternal.Remove(comboBoxItemAdv1.DataContext);
      }
      else if (comboBoxItemAdv1.Content != null)
      {
        if (comboBoxItemAdv1.IsSelected)
          parent.SelItemsInternal.Add((object) comboBoxItemAdv1);
        else if (parent.SelItemsInternal.Contains((object) comboBoxItemAdv1))
          parent.SelItemsInternal.Remove((object) comboBoxItemAdv1);
      }
    }
    if (!parent.AllowMultiSelect && parent.SelectedItems == null && parent.SelItemsInternal.Count > 0)
      parent.SelectedItems = (IEnumerable) parent.SelItemsInternal;
    if (parent.SelectedItems == null || comboBoxItemAdv1.IsSelectAll)
      return;
    ObservableCollection<object> observableCollection4 = new ObservableCollection<object>(parent.SelectedItems.Cast<object>());
    ObservableCollection<ComboBoxItemAdv> observableCollection5 = new ObservableCollection<ComboBoxItemAdv>();
    foreach (object selectedItem in parent.SelectedItems)
    {
      ComboBoxItemAdv comboBoxItemAdv3 = (ComboBoxItemAdv) null;
      if (parent.ItemContainerGenerator.Status != GeneratorStatus.NotStarted)
      {
        comboBoxItemAdv3 = parent.ItemContainerGenerator.ContainerFromItem(selectedItem) as ComboBoxItemAdv;
      }
      else
      {
        int index = parent.Items.IndexOf(selectedItem);
        if (parent.Items.Count > index)
          comboBoxItemAdv3 = parent.Items[index] as ComboBoxItemAdv;
      }
      if (comboBoxItemAdv3 != null)
        observableCollection5.Add(comboBoxItemAdv3);
      else if (selectedItem is ComboBoxItemAdv)
        observableCollection5.Add(selectedItem as ComboBoxItemAdv);
    }
    int count = parent.Items.Count;
    for (int index = 0; index < count; ++index)
    {
      ComboBoxItemAdv comboBoxItemAdv4 = (ComboBoxItemAdv) null;
      if (parent.ItemContainerGenerator.Status != GeneratorStatus.NotStarted)
        comboBoxItemAdv4 = parent.ItemContainerGenerator.ContainerFromIndex(index) as ComboBoxItemAdv;
      else if (count > index)
        comboBoxItemAdv4 = parent.Items[index] as ComboBoxItemAdv;
      if (comboBoxItemAdv4 != null)
      {
        if (comboBoxItemAdv4.IsSelected)
        {
          Syncfusion.Windows.VisualStateManager.GoToState((Control) comboBoxItemAdv4, "Selected", true);
          if (!parent.AllowMultiSelect && observableCollection5.Contains(comboBoxItemAdv4) && observableCollection5.Count > 0)
            parent.SelectedIndex = index;
          if (parent.Items.Count > 0 && index < parent.Items.Count && !observableCollection4.Contains(parent.Items[index]))
            observableCollection4.Add(parent.Items[index]);
        }
        else
          Syncfusion.Windows.VisualStateManager.GoToState((Control) comboBoxItemAdv4, "Unselected", true);
      }
    }
    if (!parent.AllowMultiSelect)
      parent.UpdateSelectionBox();
    parent.UpdateDefaultTextVisibility();
  }

  protected override AutomationPeer OnCreateAutomationPeer()
  {
    return (AutomationPeer) new ComboBoxItemAdvAutomationPeer(this);
  }
}
