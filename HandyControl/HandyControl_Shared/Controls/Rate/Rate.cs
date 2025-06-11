// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.Rate
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

public class Rate : RegularItemsControl
{
  public static readonly DependencyProperty AllowHalfProperty = DependencyProperty.Register(nameof (AllowHalf), typeof (bool), typeof (Rate), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty AllowClearProperty = DependencyProperty.Register(nameof (AllowClear), typeof (bool), typeof (Rate), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof (Icon), typeof (Geometry), typeof (Rate), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty CountProperty = DependencyProperty.Register(nameof (Count), typeof (int), typeof (Rate), new PropertyMetadata(ValueBoxes.Int5Box));
  public static readonly DependencyProperty DefaultValueProperty = DependencyProperty.Register(nameof (DefaultValue), typeof (double), typeof (Rate), new PropertyMetadata(ValueBoxes.Double0Box));
  public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (double), typeof (Rate), new PropertyMetadata(ValueBoxes.Double0Box, new PropertyChangedCallback(Rate.OnValueChanged)));
  public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (Rate), new PropertyMetadata((object) null));
  public static readonly DependencyProperty ShowTextProperty = DependencyProperty.Register(nameof (ShowText), typeof (bool), typeof (Rate), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(nameof (IsReadOnly), typeof (bool), typeof (Rate), new PropertyMetadata(ValueBoxes.FalseBox));
  private bool _isLoaded;
  private bool _updateItems;
  public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof (EventHandler<FunctionEventArgs<double>>), typeof (Rate));

  static Rate()
  {
    RegularItemsControl.ItemWidthProperty.OverrideMetadata(typeof (Rate), new PropertyMetadata(ValueBoxes.Double20Box));
    RegularItemsControl.ItemHeightProperty.OverrideMetadata(typeof (Rate), new PropertyMetadata(ValueBoxes.Double20Box));
  }

  private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((Rate) d).OnValueChanged(new FunctionEventArgs<double>(Rate.ValueChangedEvent, (object) d)
    {
      Info = (double) e.NewValue
    });
  }

  protected virtual void OnValueChanged(FunctionEventArgs<double> e)
  {
    this.RaiseEvent((RoutedEventArgs) e);
    this.UpdateItems();
  }

  public event EventHandler<FunctionEventArgs<double>> ValueChanged
  {
    add => this.AddHandler(Rate.ValueChangedEvent, (Delegate) value);
    remove => this.RemoveHandler(Rate.ValueChangedEvent, (Delegate) value);
  }

  public Rate()
  {
    this.AddHandler(RateItem.SelectedChangedEvent, (Delegate) new RoutedEventHandler(this.RateItemSelectedChanged));
    this.AddHandler(RateItem.ValueChangedEvent, (Delegate) new RoutedEventHandler(this.RateItemValueChanged));
    this.Loaded += (RoutedEventHandler) ((s, e) =>
    {
      if (DesignerHelper.IsInDesignMode)
        return;
      this._updateItems = false;
      this.OnApplyTemplateInternal();
      this._updateItems = true;
      this.UpdateItems();
      if (this._isLoaded)
        return;
      this._isLoaded = true;
      if (this.Value <= 0.0)
      {
        if (this.DefaultValue <= 0.0)
          return;
        this.Value = this.DefaultValue;
      }
      else
        this.UpdateItems();
    });
  }

  public bool AllowHalf
  {
    get => (bool) this.GetValue(Rate.AllowHalfProperty);
    set => this.SetValue(Rate.AllowHalfProperty, ValueBoxes.BooleanBox(value));
  }

  public bool AllowClear
  {
    get => (bool) this.GetValue(Rate.AllowClearProperty);
    set => this.SetValue(Rate.AllowClearProperty, ValueBoxes.BooleanBox(value));
  }

  public Geometry Icon
  {
    get => (Geometry) this.GetValue(Rate.IconProperty);
    set => this.SetValue(Rate.IconProperty, (object) value);
  }

  public int Count
  {
    get => (int) this.GetValue(Rate.CountProperty);
    set => this.SetValue(Rate.CountProperty, (object) value);
  }

  public double DefaultValue
  {
    get => (double) this.GetValue(Rate.DefaultValueProperty);
    set => this.SetValue(Rate.DefaultValueProperty, (object) value);
  }

  public double Value
  {
    get => (double) this.GetValue(Rate.ValueProperty);
    set => this.SetValue(Rate.ValueProperty, (object) value);
  }

  public string Text
  {
    get => (string) this.GetValue(Rate.TextProperty);
    set => this.SetValue(Rate.TextProperty, (object) value);
  }

  public bool ShowText
  {
    get => (bool) this.GetValue(Rate.ShowTextProperty);
    set => this.SetValue(Rate.ShowTextProperty, ValueBoxes.BooleanBox(value));
  }

  public bool IsReadOnly
  {
    get => (bool) this.GetValue(Rate.IsReadOnlyProperty);
    set => this.SetValue(Rate.IsReadOnlyProperty, ValueBoxes.BooleanBox(value));
  }

  private void RateItemValueChanged(object sender, RoutedEventArgs e)
  {
    this.Value = this.Items.Cast<RateItem>().Where<RateItem>((Func<RateItem, bool>) (item => item.IsSelected)).Select<RateItem, double>((Func<RateItem, double>) (item => !item.IsHalf ? 1.0 : 0.5)).Sum();
  }

  private void RateItemSelectedChanged(object sender, RoutedEventArgs e)
  {
    if (!(e.OriginalSource is RateItem originalSource))
      return;
    int index1 = originalSource.Index;
    for (int index2 = 0; index2 < index1; ++index2)
    {
      if (this.Items[index2] is RateItem rateItem)
      {
        rateItem.IsSelected = true;
        rateItem.IsHalf = false;
      }
    }
    for (int index3 = index1; index3 < this.Count; ++index3)
    {
      if (this.Items[index3] is RateItem rateItem)
      {
        rateItem.IsSelected = false;
        rateItem.IsHalf = false;
      }
    }
  }

  protected override bool IsItemItsOwnContainerOverride(object item) => item is RateItem;

  protected override DependencyObject GetContainerForItemOverride()
  {
    return (DependencyObject) new RateItem();
  }

  private void OnApplyTemplateInternal()
  {
    this.Items.Clear();
    for (int index = 1; index <= this.Count; ++index)
    {
      RateItem rateItem = new RateItem();
      rateItem.Index = index;
      rateItem.Width = this.ItemWidth;
      rateItem.Height = this.ItemHeight;
      rateItem.Margin = this.ItemMargin;
      rateItem.AllowHalf = this.AllowHalf;
      rateItem.AllowClear = this.AllowClear;
      rateItem.Icon = this.Icon;
      rateItem.IsReadOnly = this.IsReadOnly;
      rateItem.Background = this.Background;
      this.Items.Add((object) rateItem);
    }
  }

  public override void OnApplyTemplate()
  {
    if (!this._isLoaded)
    {
      this._updateItems = true;
      this.OnApplyTemplateInternal();
      this._updateItems = false;
    }
    base.OnApplyTemplate();
  }

  protected override void OnMouseLeave(MouseEventArgs e)
  {
    base.OnMouseLeave(e);
    this.UpdateItems();
  }

  protected override void UpdateItems()
  {
    if (!this._isLoaded || !this._updateItems)
      return;
    int index1 = (int) this.Value;
    for (int index2 = 0; index2 < index1; ++index2)
    {
      if (this.Items[index2] is RateItem rateItem)
      {
        rateItem.IsSelected = true;
        rateItem.IsHalf = false;
      }
    }
    if (this.Value > (double) index1)
    {
      if (this.Items[index1] is RateItem rateItem)
      {
        rateItem.IsSelected = true;
        rateItem.IsHalf = true;
      }
      ++index1;
    }
    for (int index3 = index1; index3 < this.Count; ++index3)
    {
      if (this.Items[index3] is RateItem rateItem)
      {
        rateItem.IsSelected = false;
        rateItem.IsHalf = false;
      }
    }
  }

  public void Reset() => this.Value = this.DefaultValue;
}
