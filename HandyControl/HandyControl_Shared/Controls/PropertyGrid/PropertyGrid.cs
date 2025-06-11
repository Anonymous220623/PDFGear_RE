// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.PropertyGrid
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools.Extension;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_ItemsControl", Type = typeof (ItemsControl))]
[TemplatePart(Name = "PART_SearchBar", Type = typeof (SearchBar))]
public class PropertyGrid : Control
{
  private const string ElementItemsControl = "PART_ItemsControl";
  private const string ElementSearchBar = "PART_SearchBar";
  private ItemsControl _itemsControl;
  private ICollectionView _dataView;
  private SearchBar _searchBar;
  private string _searchKey;
  public static readonly RoutedEvent SelectedObjectChangedEvent = EventManager.RegisterRoutedEvent("SelectedObjectChanged", RoutingStrategy.Bubble, typeof (RoutedPropertyChangedEventHandler<object>), typeof (PropertyGrid));
  public static readonly DependencyProperty SelectedObjectProperty = DependencyProperty.Register(nameof (SelectedObject), typeof (object), typeof (PropertyGrid), new PropertyMetadata((object) null, new PropertyChangedCallback(PropertyGrid.OnSelectedObjectChanged)));
  public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(nameof (Description), typeof (string), typeof (PropertyGrid), new PropertyMetadata((object) null));
  public static readonly DependencyProperty MaxTitleWidthProperty = DependencyProperty.Register(nameof (MaxTitleWidth), typeof (double), typeof (PropertyGrid), new PropertyMetadata(ValueBoxes.Double0Box));
  public static readonly DependencyProperty MinTitleWidthProperty = DependencyProperty.Register(nameof (MinTitleWidth), typeof (double), typeof (PropertyGrid), new PropertyMetadata(ValueBoxes.Double0Box));
  public static readonly DependencyProperty ShowSortButtonProperty = DependencyProperty.Register(nameof (ShowSortButton), typeof (bool), typeof (PropertyGrid), new PropertyMetadata(ValueBoxes.TrueBox));

  public PropertyGrid()
  {
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.SortByCategory, new ExecutedRoutedEventHandler(this.SortByCategory), (CanExecuteRoutedEventHandler) ((s, e) => e.CanExecute = this.ShowSortButton)));
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.SortByName, new ExecutedRoutedEventHandler(this.SortByName), (CanExecuteRoutedEventHandler) ((s, e) => e.CanExecute = this.ShowSortButton)));
  }

  public virtual PropertyResolver PropertyResolver { get; } = new PropertyResolver();

  public event RoutedPropertyChangedEventHandler<object> SelectedObjectChanged
  {
    add => this.AddHandler(PropertyGrid.SelectedObjectChangedEvent, (Delegate) value);
    remove => this.RemoveHandler(PropertyGrid.SelectedObjectChangedEvent, (Delegate) value);
  }

  private static void OnSelectedObjectChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((PropertyGrid) d).OnSelectedObjectChanged(e.OldValue, e.NewValue);
  }

  public object SelectedObject
  {
    get => this.GetValue(PropertyGrid.SelectedObjectProperty);
    set => this.SetValue(PropertyGrid.SelectedObjectProperty, value);
  }

  protected virtual void OnSelectedObjectChanged(object oldValue, object newValue)
  {
    this.UpdateItems(newValue);
    this.RaiseEvent((RoutedEventArgs) new RoutedPropertyChangedEventArgs<object>(oldValue, newValue, PropertyGrid.SelectedObjectChangedEvent));
  }

  public string Description
  {
    get => (string) this.GetValue(PropertyGrid.DescriptionProperty);
    set => this.SetValue(PropertyGrid.DescriptionProperty, (object) value);
  }

  public double MaxTitleWidth
  {
    get => (double) this.GetValue(PropertyGrid.MaxTitleWidthProperty);
    set => this.SetValue(PropertyGrid.MaxTitleWidthProperty, (object) value);
  }

  public double MinTitleWidth
  {
    get => (double) this.GetValue(PropertyGrid.MinTitleWidthProperty);
    set => this.SetValue(PropertyGrid.MinTitleWidthProperty, (object) value);
  }

  public bool ShowSortButton
  {
    get => (bool) this.GetValue(PropertyGrid.ShowSortButtonProperty);
    set => this.SetValue(PropertyGrid.ShowSortButtonProperty, ValueBoxes.BooleanBox(value));
  }

  public override void OnApplyTemplate()
  {
    if (this._searchBar != null)
      this._searchBar.SearchStarted -= new EventHandler<FunctionEventArgs<string>>(this.SearchBar_SearchStarted);
    base.OnApplyTemplate();
    this._itemsControl = this.GetTemplateChild("PART_ItemsControl") as ItemsControl;
    this._searchBar = this.GetTemplateChild("PART_SearchBar") as SearchBar;
    if (this._searchBar != null)
      this._searchBar.SearchStarted += new EventHandler<FunctionEventArgs<string>>(this.SearchBar_SearchStarted);
    this.UpdateItems(this.SelectedObject);
  }

  private void UpdateItems(object obj)
  {
    if (obj == null || this._itemsControl == null)
      return;
    this._dataView = CollectionViewSource.GetDefaultView((object) TypeDescriptor.GetProperties(obj.GetType()).OfType<PropertyDescriptor>().Where<PropertyDescriptor>((Func<PropertyDescriptor, bool>) (item => this.PropertyResolver.ResolveIsBrowsable(item))).Select<PropertyDescriptor, PropertyItem>(new Func<PropertyDescriptor, PropertyItem>(this.CreatePropertyItem)).Do<PropertyItem>((Action<PropertyItem>) (item => item.InitElement())));
    this.SortByCategory((object) null, (ExecutedRoutedEventArgs) null);
    this._itemsControl.ItemsSource = (IEnumerable) this._dataView;
  }

  private void SortByCategory(object sender, ExecutedRoutedEventArgs e)
  {
    if (this._dataView == null)
      return;
    using (this._dataView.DeferRefresh())
    {
      this._dataView.GroupDescriptions.Clear();
      this._dataView.SortDescriptions.Clear();
      this._dataView.SortDescriptions.Add(new SortDescription(PropertyItem.CategoryProperty.Name, ListSortDirection.Ascending));
      this._dataView.SortDescriptions.Add(new SortDescription(PropertyItem.DisplayNameProperty.Name, ListSortDirection.Ascending));
      this._dataView.GroupDescriptions.Add((GroupDescription) new PropertyGroupDescription(PropertyItem.CategoryProperty.Name));
    }
  }

  private void SortByName(object sender, ExecutedRoutedEventArgs e)
  {
    if (this._dataView == null)
      return;
    using (this._dataView.DeferRefresh())
    {
      this._dataView.GroupDescriptions.Clear();
      this._dataView.SortDescriptions.Clear();
      this._dataView.SortDescriptions.Add(new SortDescription(PropertyItem.PropertyNameProperty.Name, ListSortDirection.Ascending));
    }
  }

  private void SearchBar_SearchStarted(object sender, FunctionEventArgs<string> e)
  {
    if (this._dataView == null)
      return;
    this._searchKey = e.Info;
    if (string.IsNullOrEmpty(this._searchKey))
    {
      foreach (UIElement element in (IEnumerable) this._dataView)
        element.Show();
    }
    else
    {
      foreach (PropertyItem element in (IEnumerable) this._dataView)
        element.Show(element.PropertyName.ToLower().Contains(this._searchKey) || element.DisplayName.ToLower().Contains(this._searchKey));
    }
  }

  protected virtual PropertyItem CreatePropertyItem(PropertyDescriptor propertyDescriptor)
  {
    return new PropertyItem()
    {
      Category = this.PropertyResolver.ResolveCategory(propertyDescriptor),
      DisplayName = this.PropertyResolver.ResolveDisplayName(propertyDescriptor),
      Description = this.PropertyResolver.ResolveDescription(propertyDescriptor),
      IsReadOnly = this.PropertyResolver.ResolveIsReadOnly(propertyDescriptor),
      DefaultValue = this.PropertyResolver.ResolveDefaultValue(propertyDescriptor),
      Editor = this.PropertyResolver.ResolveEditor(propertyDescriptor),
      Value = this.SelectedObject,
      PropertyName = propertyDescriptor.Name,
      PropertyType = propertyDescriptor.PropertyType,
      PropertyTypeName = $"{propertyDescriptor.PropertyType.Namespace}.{propertyDescriptor.PropertyType.Name}"
    };
  }

  protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
  {
    base.OnRenderSizeChanged(sizeInfo);
    TitleElement.SetTitleWidth((DependencyObject) this, new GridLength(Math.Max(this.MinTitleWidth, Math.Min(this.MaxTitleWidth, this.ActualWidth / 3.0))));
  }
}
