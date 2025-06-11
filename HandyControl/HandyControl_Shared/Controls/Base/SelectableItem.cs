// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.SelectableItem
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace HandyControl.Controls;

public class SelectableItem : ContentControl, ISelectable
{
  private bool _isMouseLeftButtonDown;
  public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof (IsSelected), typeof (bool), typeof (SelectableItem), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty SelfManageProperty = DependencyProperty.Register(nameof (SelfManage), typeof (bool), typeof (SelectableItem), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty CanDeselectProperty = DependencyProperty.Register(nameof (CanDeselect), typeof (bool), typeof (SelectableItem), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly RoutedEvent SelectedEvent = EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (SelectableItem));
  public static readonly RoutedEvent DeselectedEvent = EventManager.RegisterRoutedEvent("Deselected", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (SelectableItem));

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
    if (this.SelfManage)
    {
      if (!this.IsSelected)
      {
        this.IsSelected = true;
        this.OnSelected(new RoutedEventArgs(SelectableItem.SelectedEvent, (object) this));
      }
      else if (this.CanDeselect)
      {
        this.IsSelected = false;
        this.OnSelected(new RoutedEventArgs(SelectableItem.DeselectedEvent, (object) this));
      }
    }
    else if (this.CanDeselect)
      this.OnSelected(this.IsSelected ? new RoutedEventArgs(SelectableItem.DeselectedEvent, (object) this) : new RoutedEventArgs(SelectableItem.SelectedEvent, (object) this));
    else
      this.OnSelected(new RoutedEventArgs(SelectableItem.SelectedEvent, (object) this));
    this._isMouseLeftButtonDown = false;
  }

  protected virtual void OnSelected(RoutedEventArgs e) => this.RaiseEvent(e);

  public bool IsSelected
  {
    get => (bool) this.GetValue(SelectableItem.IsSelectedProperty);
    set => this.SetValue(SelectableItem.IsSelectedProperty, ValueBoxes.BooleanBox(value));
  }

  public bool SelfManage
  {
    get => (bool) this.GetValue(SelectableItem.SelfManageProperty);
    set => this.SetValue(SelectableItem.SelfManageProperty, ValueBoxes.BooleanBox(value));
  }

  public bool CanDeselect
  {
    get => (bool) this.GetValue(SelectableItem.CanDeselectProperty);
    set => this.SetValue(SelectableItem.CanDeselectProperty, ValueBoxes.BooleanBox(value));
  }

  public event RoutedEventHandler Selected
  {
    add => this.AddHandler(SelectableItem.SelectedEvent, (Delegate) value);
    remove => this.RemoveHandler(SelectableItem.SelectedEvent, (Delegate) value);
  }

  public event RoutedEventHandler Deselected
  {
    add => this.AddHandler(SelectableItem.DeselectedEvent, (Delegate) value);
    remove => this.RemoveHandler(SelectableItem.DeselectedEvent, (Delegate) value);
  }
}
