// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.RateItem
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools.Extension;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_Icon", Type = typeof (FrameworkElement))]
public class RateItem : Control
{
  private const string ElementIcon = "PART_Icon";
  public static readonly DependencyProperty AllowClearProperty = Rate.AllowClearProperty.AddOwner(typeof (RateItem));
  public static readonly DependencyProperty AllowHalfProperty = Rate.AllowHalfProperty.AddOwner(typeof (RateItem), new PropertyMetadata(new PropertyChangedCallback(RateItem.OnAllowHalfChanged)));
  public static readonly DependencyProperty IconProperty = Rate.IconProperty.AddOwner(typeof (RateItem));
  public static readonly DependencyProperty IsReadOnlyProperty = Rate.IsReadOnlyProperty.AddOwner(typeof (RateItem));
  internal static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof (IsSelected), typeof (bool), typeof (RateItem), new PropertyMetadata(ValueBoxes.FalseBox, new PropertyChangedCallback(RateItem.OnIsSelectedChanged)));
  public static readonly RoutedEvent SelectedChangedEvent = EventManager.RegisterRoutedEvent("SelectedChanged", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (RateItem));
  public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (RateItem));
  private FrameworkElement _icon;
  private bool _isHalf;
  private bool _isLoaded;
  private bool _isMouseLeftButtonDown;
  private bool _isSentValue;

  public RateItem()
  {
    this.Loaded += (RoutedEventHandler) ((s, e) =>
    {
      if (this._isLoaded)
        return;
      this._isLoaded = true;
      this.OnApplyTemplate();
    });
  }

  public bool AllowClear
  {
    get => (bool) this.GetValue(RateItem.AllowClearProperty);
    set => this.SetValue(RateItem.AllowClearProperty, ValueBoxes.BooleanBox(value));
  }

  public bool AllowHalf
  {
    get => (bool) this.GetValue(RateItem.AllowHalfProperty);
    set => this.SetValue(RateItem.AllowHalfProperty, ValueBoxes.BooleanBox(value));
  }

  public Geometry Icon
  {
    get => (Geometry) this.GetValue(RateItem.IconProperty);
    set => this.SetValue(RateItem.IconProperty, (object) value);
  }

  internal bool IsSelected
  {
    get => (bool) this.GetValue(RateItem.IsSelectedProperty);
    set => this.SetValue(RateItem.IsSelectedProperty, ValueBoxes.BooleanBox(value));
  }

  public bool IsReadOnly
  {
    get => (bool) this.GetValue(RateItem.IsReadOnlyProperty);
    set => this.SetValue(RateItem.IsReadOnlyProperty, ValueBoxes.BooleanBox(value));
  }

  internal bool IsHalf
  {
    get => this._isHalf;
    set
    {
      if (this._isHalf == value)
        return;
      this._isHalf = value;
      if (this._icon == null)
        return;
      this._icon.Width = value ? this.Width / 2.0 : this.Width;
    }
  }

  internal int Index { get; set; }

  private static void OnAllowHalfChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((RateItem) d).HandleMouseMoveEvent((bool) e.NewValue);
  }

  private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    FrameworkElement icon = ((RateItem) d)._icon;
    if (icon == null)
      return;
    icon.Show((bool) e.NewValue);
  }

  public event RoutedEventHandler SelectedChanged
  {
    add => this.AddHandler(RateItem.SelectedChangedEvent, (Delegate) value);
    remove => this.RemoveHandler(RateItem.SelectedChangedEvent, (Delegate) value);
  }

  public event RoutedEventHandler ValueChanged
  {
    add => this.AddHandler(RateItem.ValueChangedEvent, (Delegate) value);
    remove => this.RemoveHandler(RateItem.ValueChangedEvent, (Delegate) value);
  }

  private void HandleMouseMoveEvent(bool handle)
  {
    if (handle)
      this.MouseMove += new MouseEventHandler(this.RateItem_MouseMove);
    else
      this.MouseMove -= new MouseEventHandler(this.RateItem_MouseMove);
  }

  private void RateItem_MouseMove(object sender, MouseEventArgs e)
  {
    if (this.IsReadOnly || !this.AllowHalf)
      return;
    this.IsHalf = e.GetPosition((IInputElement) this).X < this.Width / 2.0;
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this._icon = this.GetTemplateChild("PART_Icon") as FrameworkElement;
    if (!this._isLoaded || this._icon == null)
      return;
    this._icon.Show(this.IsSelected);
    this._icon.Width = this.IsHalf ? this.Width / 2.0 : this.Width;
  }

  protected override void OnMouseEnter(MouseEventArgs e)
  {
    base.OnMouseEnter(e);
    if (this.IsReadOnly)
      return;
    this._isSentValue = false;
    this.IsSelected = true;
    this.RaiseEvent(new RoutedEventArgs(RateItem.SelectedChangedEvent)
    {
      Source = (object) this
    });
  }

  protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    base.OnMouseLeftButtonDown(e);
    if (this.IsReadOnly)
      return;
    this._isMouseLeftButtonDown = true;
  }

  protected override void OnMouseLeave(MouseEventArgs e)
  {
    base.OnMouseLeave(e);
    if (this.IsReadOnly)
      return;
    this._isMouseLeftButtonDown = false;
  }

  protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    base.OnMouseLeftButtonUp(e);
    if (this.IsReadOnly || !this._isMouseLeftButtonDown)
      return;
    if (this.Index == 1 && this.AllowClear)
    {
      if (this.IsSelected)
      {
        if (!this._isSentValue)
        {
          this.RaiseEvent(new RoutedEventArgs(RateItem.ValueChangedEvent)
          {
            Source = (object) this
          });
          this._isMouseLeftButtonDown = false;
          this._isSentValue = true;
          return;
        }
        this._isSentValue = false;
        this.IsSelected = false;
        this.IsHalf = false;
      }
      else
      {
        this.IsSelected = true;
        if (this.AllowHalf)
          this.IsHalf = e.GetPosition((IInputElement) this).X < this.Width / 2.0;
      }
    }
    this.RaiseEvent(new RoutedEventArgs(RateItem.ValueChangedEvent)
    {
      Source = (object) this
    });
    this._isMouseLeftButtonDown = false;
  }
}
