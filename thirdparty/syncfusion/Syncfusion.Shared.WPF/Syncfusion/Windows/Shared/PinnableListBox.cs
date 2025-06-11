// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.PinnableListBox
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Licensing;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

#nullable disable
namespace Syncfusion.Windows.Shared;

[SkinType(SkinVisualStyle = Skin.Office2007Blue, Type = typeof (PinnableListBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/PinnableListBox/Themes/Office2007BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Transparent, Type = typeof (PinnableListBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/PinnableListBox/Themes/TransparentStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Black, Type = typeof (PinnableListBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/PinnableListBox/Themes/Office2007BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Silver, Type = typeof (PinnableListBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/PinnableListBox/Themes/Office2007SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Blue, Type = typeof (PinnableListBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/PinnableListBox/Themes/Office2010BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Black, Type = typeof (PinnableListBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/PinnableListBox/Themes/Office2010BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Silver, Type = typeof (PinnableListBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/PinnableListBox/Themes/Office2010SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Blend, Type = typeof (PinnableListBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/PinnableListBox/Themes/BlendStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Default, Type = typeof (PinnableListBox), XamlResource = "/Syncfusion.Shared.Wpf;component/Controls/PinnableListBox/Themes/Generic.xaml")]
[SkinType(SkinVisualStyle = Skin.VS2010, Type = typeof (PinnableListBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/PinnableListBox/Themes/VS2010Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Metro, Type = typeof (PinnableListBox), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/PinnableListBox/Themes/MetroStyle.xaml")]
public class PinnableListBox : ItemsControl
{
  internal bool isCalledByUpdateItems;
  internal string default_StoreFile = AppDomain.CurrentDomain.FriendlyName + ".dat";
  internal PinnableListBoxItem pinnableItem;
  internal PinnableItemsControl pinnedItems;
  internal PinnableItemsControl unpinnedItems;
  public static readonly DependencyProperty PinItemsSortDescriptionProperty = DependencyProperty.Register(nameof (PinItemsSortDescription), typeof (string), typeof (PinnableListBox), (PropertyMetadata) new UIPropertyMetadata((object) ""));
  public static readonly DependencyProperty UnPinItemsSortDescriptionProperty = DependencyProperty.Register(nameof (UnPinItemsSortDescription), typeof (string), typeof (PinnableListBox), (PropertyMetadata) new UIPropertyMetadata((object) ""));
  public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof (Header), typeof (string), typeof (PinnableListBox), (PropertyMetadata) new UIPropertyMetadata((object) string.Empty));
  public static readonly DependencyProperty PinnedItemsProperty = DependencyProperty.Register(nameof (PinnedItems), typeof (ObservableCollection<object>), typeof (PinnableListBox), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty PinnedSortDirectionProperty = DependencyProperty.Register(nameof (PinnedSortDirection), typeof (ListSortDirection), typeof (PinnableListBox), (PropertyMetadata) new UIPropertyMetadata((object) ListSortDirection.Ascending));
  public static readonly DependencyProperty UnPinnedSortDirectionProperty = DependencyProperty.Register(nameof (UnPinnedSortDirection), typeof (ListSortDirection), typeof (PinnableListBox), (PropertyMetadata) new UIPropertyMetadata((object) ListSortDirection.Ascending));
  public static readonly DependencyProperty UnpinnedItemsProperty = DependencyProperty.Register(nameof (UnpinnedItems), typeof (ObservableCollection<object>), typeof (PinnableListBox), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof (SelectedItem), typeof (object), typeof (PinnableListBox), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));

  static PinnableListBox()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (PinnableListBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (PinnableListBox)));
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  public PinnableListBox()
  {
    this.PinnedItems = new ObservableCollection<object>();
    this.UnpinnedItems = new ObservableCollection<object>();
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  public event RoutedEventHandler PinStatusChangedEvent;

  public void SaveState(string fileName)
  {
    if (fileName != null && fileName != string.Empty)
    {
      this.default_StoreFile = fileName;
    }
    else
    {
      string name = Assembly.GetCallingAssembly().GetName().Name;
      if (name != null && name != string.Empty)
        this.default_StoreFile = name + ".dat";
    }
    IsolatedStorageFile store = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, (Type) null, (Type) null);
    PinnableListBoxParams pinnableListBoxParams = new PinnableListBoxParams(this.PinItemsSortDescription, this.UnPinItemsSortDescription);
    IsolatedStorageFileStream storageFileStream = new IsolatedStorageFileStream(this.default_StoreFile, FileMode.Create, FileAccess.Write, store);
    XamlWriter.Save((object) pinnableListBoxParams, (Stream) storageFileStream);
    storageFileStream.Close();
  }

  public void LoadState(string fileName)
  {
    IsolatedStorageFile store = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, (Type) null, (Type) null);
    if (fileName != null && fileName != string.Empty)
    {
      this.default_StoreFile = fileName;
    }
    else
    {
      string name = Assembly.GetCallingAssembly().GetName().Name;
      if (name != null && name != string.Empty)
        this.default_StoreFile = name + ".dat";
    }
    if (store.GetFileNames(this.default_StoreFile).Length <= 0)
      return;
    Stream stream = (Stream) new IsolatedStorageFileStream(this.default_StoreFile, FileMode.Open, store);
    PinnableListBoxParams stateparams = XamlReader.Load(stream) as PinnableListBoxParams;
    stream.Close();
    this.ApplyStateParams(stateparams);
  }

  internal void ApplyStateParams(PinnableListBoxParams stateparams)
  {
    this.PinItemsSortDescription = stateparams.PinItemsSortDescription;
    this.UnPinItemsSortDescription = stateparams.UnPinItemsSortDescription;
  }

  public override void OnApplyTemplate()
  {
    this.pinnedItems = this.GetTemplateChild("PART_PinnedItems") as PinnableItemsControl;
    this.unpinnedItems = this.GetTemplateChild("PART_UnpinnedItems") as PinnableItemsControl;
    if (this.pinnedItems != null)
    {
      this.pinnedItems.pinnableListBox = this;
      this.pinnedItems.ItemContainerStyle = this.ItemContainerStyle;
      this.pinnedItems.ItemContainerStyleSelector = this.ItemContainerStyleSelector;
      this.pinnedItems.ItemTemplate = this.ItemTemplate;
      this.pinnedItems.ItemTemplateSelector = this.ItemTemplateSelector;
    }
    if (this.unpinnedItems != null)
    {
      this.unpinnedItems.pinnableListBox = this;
      this.unpinnedItems.ItemContainerStyle = this.ItemContainerStyle;
      this.unpinnedItems.ItemContainerStyleSelector = this.ItemContainerStyleSelector;
      this.unpinnedItems.ItemTemplate = this.ItemTemplate;
      this.unpinnedItems.ItemTemplateSelector = this.ItemTemplateSelector;
    }
    base.OnApplyTemplate();
  }

  protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    if (this.PinnedItems != null)
      this.PinnedItems.Clear();
    if (this.UnpinnedItems != null)
      this.UnpinnedItems.Clear();
    foreach (object obj in (IEnumerable) this.Items)
      this.UpdatePinItems(this, obj, false);
    base.OnItemsSourceChanged(oldValue, newValue);
  }

  protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
  {
    if (e.Action == NotifyCollectionChangedAction.Add)
    {
      foreach (object newItem in (IEnumerable) e.NewItems)
        this.UpdatePinItems(this, newItem, false);
    }
    else if (e.Action == NotifyCollectionChangedAction.Reset)
    {
      if (this.PinnedItems != null)
        this.PinnedItems.Clear();
      if (this.UnpinnedItems != null)
        this.UnpinnedItems.Clear();
      foreach (object obj in (IEnumerable) this.Items)
        this.UpdatePinItems(this, obj, false);
    }
    else if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems.Count > 0)
    {
      foreach (object oldItem in (IEnumerable) e.OldItems)
      {
        if (this.PinnedItems != null && this.PinnedItems.Contains(oldItem))
          this.PinnedItems.Remove(oldItem);
        else if (this.UnpinnedItems != null && this.UnpinnedItems.Contains(oldItem))
          this.UnpinnedItems.Remove(oldItem);
      }
    }
    base.OnItemsChanged(e);
  }

  protected override void OnInitialized(EventArgs e) => base.OnInitialized(e);

  public string PinItemsSortDescription
  {
    get => (string) this.GetValue(PinnableListBox.PinItemsSortDescriptionProperty);
    set => this.SetValue(PinnableListBox.PinItemsSortDescriptionProperty, (object) value);
  }

  public string UnPinItemsSortDescription
  {
    get => (string) this.GetValue(PinnableListBox.UnPinItemsSortDescriptionProperty);
    set => this.SetValue(PinnableListBox.UnPinItemsSortDescriptionProperty, (object) value);
  }

  public string Header
  {
    get => (string) this.GetValue(PinnableListBox.HeaderProperty);
    set => this.SetValue(PinnableListBox.HeaderProperty, (object) value);
  }

  public ObservableCollection<object> PinnedItems
  {
    get => (ObservableCollection<object>) this.GetValue(PinnableListBox.PinnedItemsProperty);
    set => this.SetValue(PinnableListBox.PinnedItemsProperty, (object) value);
  }

  public ListSortDirection PinnedSortDirection
  {
    get => (ListSortDirection) this.GetValue(PinnableListBox.PinnedSortDirectionProperty);
    set => this.SetValue(PinnableListBox.PinnedSortDirectionProperty, (object) value);
  }

  public ListSortDirection UnPinnedSortDirection
  {
    get => (ListSortDirection) this.GetValue(PinnableListBox.UnPinnedSortDirectionProperty);
    set => this.SetValue(PinnableListBox.UnPinnedSortDirectionProperty, (object) value);
  }

  public ObservableCollection<object> UnpinnedItems
  {
    get => (ObservableCollection<object>) this.GetValue(PinnableListBox.UnpinnedItemsProperty);
    set => this.SetValue(PinnableListBox.UnpinnedItemsProperty, (object) value);
  }

  public object SelectedItem
  {
    get => this.GetValue(PinnableListBox.SelectedItemProperty);
    set => this.SetValue(PinnableListBox.SelectedItemProperty, value);
  }

  internal void FirePinStatusChanged()
  {
    if (this.PinStatusChangedEvent == null)
      return;
    PinnableListBoxEventArgs e = new PinnableListBoxEventArgs();
    e.PinnablelistboxItem = (object) this.pinnableItem;
    this.SelectedItem = (object) this.pinnableItem;
    this.PinStatusChangedEvent((object) this, (RoutedEventArgs) e);
  }

  internal void UpdatePinItems(PinnableListBox control, object item, bool ispinned)
  {
    this.isCalledByUpdateItems = true;
    if (item is PinnableListBoxItem pinnableListBoxItem && control.ItemsSource != null && pinnableListBoxItem.DataContext != null)
      item = pinnableListBoxItem.DataContext;
    if (pinnableListBoxItem != null && control.ItemsSource == null && pinnableListBoxItem.DataContext == null)
    {
      if (ispinned)
      {
        control.UnpinnedItems.Remove((object) pinnableListBoxItem);
        if (!control.PinnedItems.Contains((object) pinnableListBoxItem))
          control.PinnedItems.Add((object) pinnableListBoxItem);
      }
      else
      {
        control.PinnedItems.Remove((object) pinnableListBoxItem);
        if (!control.UnpinnedItems.Contains((object) pinnableListBoxItem))
          control.UnpinnedItems.Add((object) pinnableListBoxItem);
      }
    }
    else if (ispinned)
    {
      control.UnpinnedItems.Remove(item);
      if (!control.PinnedItems.Contains(item))
        control.PinnedItems.Add(item);
    }
    else
    {
      control.PinnedItems.Remove(item);
      if (!control.UnpinnedItems.Contains(item))
        control.UnpinnedItems.Add(item);
    }
    if (pinnableListBoxItem != null && pinnableListBoxItem.IsPinned != ispinned && !ispinned)
      pinnableListBoxItem.IsPinned = ispinned;
    this.SortCollection();
    control.SelectedItem = item;
    this.isCalledByUpdateItems = false;
  }

  public void SortCollection()
  {
    if (!string.IsNullOrEmpty(this.UnPinItemsSortDescription))
    {
      ListCollectionView defaultView = CollectionViewSource.GetDefaultView((object) this.UnpinnedItems) as ListCollectionView;
      using (defaultView.DeferRefresh())
      {
        defaultView.SortDescriptions.Clear();
        defaultView.SortDescriptions.Add(new SortDescription(this.UnPinItemsSortDescription, this.UnPinnedSortDirection));
      }
    }
    if (string.IsNullOrEmpty(this.PinItemsSortDescription))
      return;
    ListCollectionView defaultView1 = CollectionViewSource.GetDefaultView((object) this.PinnedItems) as ListCollectionView;
    using (defaultView1.DeferRefresh())
    {
      defaultView1.SortDescriptions.Clear();
      defaultView1.SortDescriptions.Add(new SortDescription(this.PinItemsSortDescription, this.PinnedSortDirection));
    }
  }
}
