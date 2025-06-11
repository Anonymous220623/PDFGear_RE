// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.SimpleItemsControl
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace HandyControl.Controls;

[DefaultProperty("Items")]
[ContentProperty("Items")]
[TemplatePart(Name = "PART_Panel", Type = typeof (Panel))]
public class SimpleItemsControl : Control
{
  private const string ElementPanel = "PART_Panel";
  public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(nameof (ItemTemplate), typeof (DataTemplate), typeof (SimpleItemsControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, new PropertyChangedCallback(SimpleItemsControl.OnItemTemplateChanged)));
  public static readonly DependencyProperty ItemContainerStyleProperty = DependencyProperty.Register(nameof (ItemContainerStyle), typeof (Style), typeof (SimpleItemsControl), new PropertyMetadata((object) null, new PropertyChangedCallback(SimpleItemsControl.OnItemContainerStyleChanged)));
  public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof (ItemsSource), typeof (IEnumerable), typeof (SimpleItemsControl), new PropertyMetadata((object) null, new PropertyChangedCallback(SimpleItemsControl.OnItemsSourceChanged)));
  internal static readonly DependencyPropertyKey HasItemsPropertyKey = DependencyProperty.RegisterReadOnly(nameof (HasItems), typeof (bool), typeof (SimpleItemsControl), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty HasItemsProperty = SimpleItemsControl.HasItemsPropertyKey.DependencyProperty;

  public SimpleItemsControl()
  {
    ObservableCollection<object> observableCollection = new ObservableCollection<object>();
    observableCollection.CollectionChanged += (NotifyCollectionChangedEventHandler) ((s, e) =>
    {
      if (e.NewItems != null && e.NewItems.Count > 0)
        this.SetValue(SimpleItemsControl.HasItemsPropertyKey, ValueBoxes.TrueBox);
      this.OnItemsChanged(s, e);
    });
    this.Items = (Collection<object>) observableCollection;
  }

  public IEnumerable ItemsSource
  {
    get => (IEnumerable) this.GetValue(SimpleItemsControl.ItemsSourceProperty);
    set => this.SetValue(SimpleItemsControl.ItemsSourceProperty, (object) value);
  }

  [Bindable(true)]
  [Category("Content")]
  public Style ItemContainerStyle
  {
    get => (Style) this.GetValue(SimpleItemsControl.ItemContainerStyleProperty);
    set => this.SetValue(SimpleItemsControl.ItemContainerStyleProperty, (object) value);
  }

  [Bindable(true)]
  public DataTemplate ItemTemplate
  {
    get => (DataTemplate) this.GetValue(SimpleItemsControl.ItemTemplateProperty);
    set => this.SetValue(SimpleItemsControl.ItemTemplateProperty, (object) value);
  }

  [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
  [Bindable(true)]
  public Collection<object> Items { get; }

  internal Panel ItemsHost { get; set; }

  private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((SimpleItemsControl) d).OnItemsSourceChanged((IEnumerable) e.OldValue, (IEnumerable) e.NewValue);
  }

  protected virtual void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
  }

  public override void OnApplyTemplate()
  {
    this.ItemsHost?.Children.Clear();
    base.OnApplyTemplate();
    this.ItemsHost = this.GetTemplateChild("PART_Panel") as Panel;
    this.Refresh();
  }

  protected virtual void OnItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    this.Refresh();
    this.UpdateItems();
  }

  protected virtual DependencyObject GetContainerForItemOverride()
  {
    return (DependencyObject) new ContentPresenter();
  }

  protected virtual bool IsItemItsOwnContainerOverride(object item) => item is ContentPresenter;

  protected virtual void PrepareContainerForItemOverride(DependencyObject element, object item)
  {
    switch (element)
    {
      case ContentControl contentControl:
        contentControl.Content = item;
        contentControl.ContentTemplate = this.ItemTemplate;
        break;
      case ContentPresenter contentPresenter:
        contentPresenter.Content = item;
        contentPresenter.ContentTemplate = this.ItemTemplate;
        break;
    }
  }

  private static void OnItemTemplateChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SimpleItemsControl simpleItemsControl))
      return;
    simpleItemsControl.OnItemTemplateChanged(e);
  }

  protected virtual void OnItemTemplateChanged(DependencyPropertyChangedEventArgs e)
  {
    this.Refresh();
  }

  private static void OnItemContainerStyleChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SimpleItemsControl simpleItemsControl))
      return;
    simpleItemsControl.OnItemContainerStyleChanged(e);
  }

  protected virtual void OnItemContainerStyleChanged(DependencyPropertyChangedEventArgs e)
  {
    this.Refresh();
  }

  public bool HasItems => (bool) this.GetValue(SimpleItemsControl.HasItemsProperty);

  protected virtual void Refresh()
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
  }

  protected virtual void UpdateItems()
  {
  }
}
