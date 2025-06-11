// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.CoverView
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace HandyControl.Controls;

public class CoverView : RegularItemsControl
{
  private readonly CoverViewContent _viewContent;
  private CoverViewItem _selectedItem;
  private IEnumerable _itemsSourceInternal;
  private readonly Dictionary<object, CoverViewItem> _entryDic = new Dictionary<object, CoverViewItem>();
  private bool _isRefresh;
  public static readonly DependencyProperty CoverViewContentStyleProperty = DependencyProperty.Register(nameof (CoverViewContentStyle), typeof (Style), typeof (CoverView), new PropertyMetadata((object) null));
  internal static readonly DependencyProperty GroupsProperty = DependencyProperty.Register(nameof (Groups), typeof (int), typeof (CoverView), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Int5Box, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(CoverView.OnGroupsChanged), new CoerceValueCallback(CoverView.CoerceGroups)));
  public static readonly DependencyProperty ItemContentHeightProperty = DependencyProperty.Register(nameof (ItemContentHeight), typeof (double), typeof (CoverView), new PropertyMetadata(ValueBoxes.Double300Box), new ValidateValueCallback(ValidateHelper.IsInRangeOfPosDoubleIncludeZero));
  public static readonly DependencyProperty ItemContentHeightFixedProperty = DependencyProperty.Register(nameof (ItemContentHeightFixed), typeof (bool), typeof (CoverView), new PropertyMetadata(ValueBoxes.TrueBox));
  public static readonly DependencyProperty ItemHeaderTemplateProperty = DependencyProperty.Register(nameof (ItemHeaderTemplate), typeof (DataTemplate), typeof (CoverView), new PropertyMetadata((object) null));

  public CoverView()
  {
    this._viewContent = new CoverViewContent();
    this.AddHandler(SelectableItem.SelectedEvent, (Delegate) new RoutedEventHandler(this.CoverViewItem_OnSelected));
    this._viewContent.SetBinding(CoverViewContent.ContentHeightProperty, (BindingBase) new Binding(CoverView.ItemContentHeightProperty.Name)
    {
      Source = (object) this
    });
    this._viewContent.SetBinding(FrameworkElement.WidthProperty, (BindingBase) new Binding(FrameworkElement.ActualWidthProperty.Name)
    {
      Source = (object) this
    });
  }

  private void CoverViewItem_OnSelected(object sender, RoutedEventArgs e)
  {
    if (!(e.OriginalSource is CoverViewItem originalSource))
      return;
    if (this._selectedItem == null)
    {
      originalSource.SetCurrentValue(SelectableItem.IsSelectedProperty, ValueBoxes.TrueBox);
      this._selectedItem = originalSource;
      if (this._viewContent == null)
        return;
      this._viewContent.Content = originalSource.Content;
      this._viewContent.ContentTemplate = this.ItemTemplate;
      this.UpdateCoverViewContent(true);
    }
    else if (!object.Equals((object) this._selectedItem, (object) originalSource))
    {
      this._selectedItem.SetCurrentValue(SelectableItem.IsSelectedProperty, ValueBoxes.FalseBox);
      originalSource.SetCurrentValue(SelectableItem.IsSelectedProperty, ValueBoxes.TrueBox);
      this._selectedItem = originalSource;
      if (this._viewContent == null)
        return;
      this._viewContent.Content = originalSource.Content;
      this.UpdateCoverViewContent(true);
    }
    else
    {
      if (this._viewContent != null)
      {
        this._viewContent.Content = (object) null;
        this._viewContent.ContentTemplate = (DataTemplate) null;
        this.UpdateCoverViewContent(false);
      }
      this._selectedItem.SetCurrentValue(SelectableItem.IsSelectedProperty, ValueBoxes.FalseBox);
      this._selectedItem = (CoverViewItem) null;
    }
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    CoverViewContent viewContent = this._viewContent;
    if (viewContent.Style == null)
    {
      Style viewContentStyle;
      viewContent.Style = viewContentStyle = this.CoverViewContentStyle;
    }
    if (this._selectedItem == null)
      return;
    this.UpdateCoverViewContent(this._selectedItem != null);
  }

  protected override DependencyObject GetContainerForItemOverride()
  {
    return (DependencyObject) new CoverViewItem();
  }

  private void SetBindingForItem(FrameworkElement item)
  {
    item.SetBinding(FrameworkElement.MarginProperty, (BindingBase) new Binding(RegularItemsControl.ItemMarginProperty.Name)
    {
      Source = (object) this
    });
    item.SetBinding(FrameworkElement.WidthProperty, (BindingBase) new Binding(RegularItemsControl.ItemWidthProperty.Name)
    {
      Source = (object) this
    });
    item.SetBinding(FrameworkElement.HeightProperty, (BindingBase) new Binding(RegularItemsControl.ItemHeightProperty.Name)
    {
      Source = (object) this
    });
    item.SetBinding(HeaderedSelectableItem.HeaderTemplateProperty, (BindingBase) new Binding(CoverView.ItemHeaderTemplateProperty.Name)
    {
      Source = (object) this
    });
  }

  protected override bool IsItemItsOwnContainerOverride(object item) => item is CoverViewItem;

  public Style CoverViewContentStyle
  {
    get => (Style) this.GetValue(CoverView.CoverViewContentStyleProperty);
    set => this.SetValue(CoverView.CoverViewContentStyleProperty, (object) value);
  }

  private static object CoerceGroups(DependencyObject d, object basevalue)
  {
    return (int) basevalue >= 1 ? basevalue : (object) 1;
  }

  internal static void OnGroupsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is CoverView coverView))
      return;
    coverView.UpdateCoverViewContent(coverView._viewContent.IsOpen);
  }

  public int Groups
  {
    get => (int) this.GetValue(CoverView.GroupsProperty);
    set => this.SetValue(CoverView.GroupsProperty, (object) value);
  }

  public double ItemContentHeight
  {
    get => (double) this.GetValue(CoverView.ItemContentHeightProperty);
    set => this.SetValue(CoverView.ItemContentHeightProperty, (object) value);
  }

  public bool ItemContentHeightFixed
  {
    get => (bool) this.GetValue(CoverView.ItemContentHeightFixedProperty);
    set => this.SetValue(CoverView.ItemContentHeightFixedProperty, ValueBoxes.BooleanBox(value));
  }

  public DataTemplate ItemHeaderTemplate
  {
    get => (DataTemplate) this.GetValue(CoverView.ItemHeaderTemplateProperty);
    set => this.SetValue(CoverView.ItemHeaderTemplateProperty, (object) value);
  }

  protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
  {
    base.OnRenderSizeChanged(sizeInfo);
    this.Groups = (int) (sizeInfo.NewSize.Width / (this.ItemWidth + this.ItemMargin.Left + this.ItemMargin.Right));
  }

  protected override void Refresh()
  {
    if (this.ItemsHost == null)
      return;
    this._entryDic.Clear();
    this._isRefresh = true;
    foreach (object obj in this.Items)
      this.AddItem(obj);
    this._isRefresh = false;
    this.GenerateIndex();
    this.UpdateCoverViewContent(this._viewContent.IsOpen);
  }

  private void UpdateCoverViewContent(bool isOpen)
  {
    if (this._selectedItem == null)
      return;
    this.ItemsHost.Children.Remove((UIElement) this._viewContent);
    if (!this.ItemContentHeightFixed)
    {
      this._viewContent.ManualHeight = 0.0;
      if (this._viewContent.Content is FrameworkElement content)
      {
        content.VerticalAlignment = VerticalAlignment.Top;
        content.Arrange(new Rect(new Size(double.MaxValue, double.MaxValue)));
        this._viewContent.ManualHeight = content.ActualHeight;
      }
    }
    this.UpdateCoverViewContentPosition();
    if (!this._viewContent.CanSwitch)
      return;
    this._viewContent.IsOpen = isOpen;
  }

  private void UpdateCoverViewContentPosition()
  {
    if (this._viewContent.Parent != null)
      this.ItemsHost.Children.Remove((UIElement) this._viewContent);
    if (this._selectedItem == null)
      return;
    int num1 = this._entryDic.Count + 1;
    int num2 = num1 / this.Groups + (num1 % this.Groups > 0 ? 1 : 0);
    if (num1 <= this.Groups)
    {
      this.ItemsHost.Children.Add((UIElement) this._viewContent);
    }
    else
    {
      int num3 = this._selectedItem.Index / this.Groups + 1;
      if (num3 == num2)
        this.ItemsHost.Children.Add((UIElement) this._viewContent);
      else
        this.ItemsHost.Children.Insert(num3 * this.Groups, (UIElement) this._viewContent);
    }
    this._viewContent.UpdatePosition(this._selectedItem.Index, this.Groups, this._selectedItem.DesiredSize.Width);
  }

  protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    if (this._itemsSourceInternal != null)
    {
      if (this._itemsSourceInternal is INotifyCollectionChanged itemsSourceInternal)
        itemsSourceInternal.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.InternalCollectionChanged);
      this.Items.Clear();
      this.ClearItems();
    }
    this._itemsSourceInternal = newValue;
    if (this._itemsSourceInternal != null)
    {
      if (this._itemsSourceInternal is INotifyCollectionChanged itemsSourceInternal)
        itemsSourceInternal.CollectionChanged += new NotifyCollectionChangedEventHandler(this.InternalCollectionChanged);
      foreach (object obj in this._itemsSourceInternal)
        this.AddItem(obj);
    }
    if (this.ItemsHost == null)
      return;
    this.GenerateIndex();
  }

  private void ClearItems()
  {
    this._selectedItem = (CoverViewItem) null;
    this._viewContent.Content = (object) null;
    this._viewContent.ContentTemplate = (DataTemplate) null;
    if (this.ItemsHost != null)
    {
      this.ItemsHost.Children.Remove((UIElement) this._viewContent);
      this.ItemsHost.Children.Clear();
    }
    this._entryDic.Clear();
  }

  private void RemoveItem(object item)
  {
    CoverViewItem element;
    if (!this._entryDic.TryGetValue(item, out element))
      return;
    if (element == this._selectedItem)
    {
      this._selectedItem = (CoverViewItem) null;
      if (this._viewContent != null)
      {
        this._viewContent.Content = (object) null;
        this._viewContent.IsOpen = false;
        this.ItemsHost.Children.Remove((UIElement) this._viewContent);
      }
    }
    this.ItemsHost.Children.Remove((UIElement) element);
    this.Items.Remove(item);
    this._entryDic.Remove(item);
  }

  private void AddItem(object item) => this.InsertItem(this._entryDic.Count, item);

  private void InsertItem(int index, object item)
  {
    if (this.ItemsHost == null)
    {
      this.Items.Insert(index, item);
      this._entryDic.Add(item, (CoverViewItem) null);
    }
    else
    {
      DependencyObject element1;
      if (this.IsItemItsOwnContainerOverride(item))
      {
        element1 = item as DependencyObject;
      }
      else
      {
        element1 = this.GetContainerForItemOverride();
        this.PrepareContainerForItemOverride(element1, item);
      }
      if (!(element1 is CoverViewItem element2))
        return;
      this.SetBindingForItem((FrameworkElement) element2);
      element2.Style = this.ItemContainerStyle;
      CoverViewItem coverViewItem = element2;
      if (coverViewItem.Header == null)
      {
        object obj;
        coverViewItem.Header = obj = item;
      }
      this._entryDic[item] = element2;
      this.ItemsHost.Children.Insert(index, (UIElement) element2);
      if (!this.IsLoaded || this._isRefresh || this._itemsSourceInternal == null)
        return;
      this.Items.Insert(index, item);
    }
  }

  private void GenerateIndex()
  {
    int num = 0;
    foreach (CoverViewItem coverViewItem in this._entryDic.Values)
      coverViewItem.Index = num++;
  }

  private void InternalCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    if (this.ItemsHost == null)
      return;
    if (e.Action == NotifyCollectionChangedAction.Reset)
    {
      if (this._entryDic.Count == 0)
        return;
      this.ClearItems();
      this.Items.Clear();
    }
    else
    {
      if (e.OldItems != null)
      {
        foreach (object oldItem in (IEnumerable) e.OldItems)
          this.RemoveItem(oldItem);
      }
      if (e.NewItems != null)
      {
        if (this._viewContent.IsOpen)
          this.ItemsHost.Children.Remove((UIElement) this._viewContent);
        int num = 0;
        foreach (object newItem in (IEnumerable) e.NewItems)
          this.InsertItem(e.NewStartingIndex + num++, newItem);
      }
      this.GenerateIndex();
      if (!this._viewContent.IsOpen)
        return;
      this.UpdateCoverViewContentPosition();
    }
  }

  protected override void OnItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    if (this._itemsSourceInternal != null)
      return;
    this.InternalCollectionChanged(sender, e);
  }

  protected override void OnItemTemplateChanged(DependencyPropertyChangedEventArgs e)
  {
  }

  protected override void OnItemContainerStyleChanged(DependencyPropertyChangedEventArgs e)
  {
  }
}
