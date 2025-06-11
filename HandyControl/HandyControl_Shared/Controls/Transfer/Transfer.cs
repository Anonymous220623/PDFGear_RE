// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.Transfer
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Collections;
using HandyControl.Data;
using HandyControl.Interactivity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_SelectedListBox", Type = typeof (ListBox))]
[DefaultEvent("TransferredItemsChanged")]
public class Transfer : ListBox
{
  private const string ElementSelectedListBox = "PART_SelectedListBox";
  public static readonly RoutedEvent TransferredItemsChangedEvent = EventManager.RegisterRoutedEvent("TransferredItemsChanged", RoutingStrategy.Bubble, typeof (SelectionChangedEventHandler), typeof (Transfer));
  private ListBox _selectedListBox;
  private static readonly DependencyPropertyKey TransferredItemsPropertyKey = DependencyProperty.RegisterReadOnly(nameof (TransferredItems), typeof (IList), typeof (Transfer), (PropertyMetadata) new FrameworkPropertyMetadata((object) null));
  private static readonly DependencyProperty TransferredItemsImplProperty = Transfer.TransferredItemsPropertyKey.DependencyProperty;

  [Category("Behavior")]
  public event SelectionChangedEventHandler TransferredItemsChanged
  {
    add => this.AddHandler(Transfer.TransferredItemsChangedEvent, (Delegate) value);
    remove => this.RemoveHandler(Transfer.TransferredItemsChangedEvent, (Delegate) value);
  }

  [Bindable(true)]
  [Category("Appearance")]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public IList TransferredItems => this.TransferredItemsImpl;

  private IList TransferredItemsImpl
  {
    get => (IList) this.GetValue(Transfer.TransferredItemsImplProperty);
  }

  public Transfer()
  {
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Selected, new ExecutedRoutedEventHandler(this.SelectItems)));
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Cancel, new ExecutedRoutedEventHandler(this.DeselectItems)));
    this.Loaded += new RoutedEventHandler(this.OnLoaded);
  }

  private void OnLoaded(object sender, RoutedEventArgs e)
  {
    this.SelectItems((object) null, (ExecutedRoutedEventArgs) null);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this._selectedListBox = this.GetTemplateChild("PART_SelectedListBox") as ListBox;
  }

  protected virtual void OnTransferredItemsChanged(SelectionChangedEventArgs e)
  {
    this.RaiseEvent((RoutedEventArgs) e);
  }

  protected override bool IsItemItsOwnContainerOverride(object item) => item is TransferItem;

  protected override DependencyObject GetContainerForItemOverride()
  {
    return (DependencyObject) new TransferItem();
  }

  private void SelectItems(object sender, ExecutedRoutedEventArgs e)
  {
    if (this._selectedListBox == null || this.SelectedItems.Count == 0)
      return;
    foreach (object selectedItem in (IEnumerable) this.SelectedItems)
    {
      if (this.ItemContainerGenerator.ContainerFromItem(selectedItem) is TransferItem transferItem1 && !transferItem1.IsTransferred)
      {
        transferItem1.IsTransferred = true;
        TransferItem transferItem = new TransferItem();
        transferItem.Tag = selectedItem;
        TransferItem newItem = transferItem;
        if (this.ItemsSource != null)
          newItem.SetBinding(ContentControl.ContentProperty, (BindingBase) new Binding(this.DisplayMemberPath)
          {
            Source = selectedItem
          });
        else
          newItem.Content = this.IsItemItsOwnContainerOverride(selectedItem) ? ((ContentControl) selectedItem).Content : selectedItem;
        this._selectedListBox.Items.Add((object) newItem);
      }
    }
    this.SetTransferredItems((IEnumerable) this._selectedListBox.Items.OfType<TransferItem>().Select<TransferItem, object>((Func<TransferItem, object>) (item => item.Tag)));
    SelectionChangedEventArgs e1 = new SelectionChangedEventArgs(Transfer.TransferredItemsChangedEvent, (IList) new List<object>(), this.SelectedItems);
    e1.Source = (object) this;
    this.OnTransferredItemsChanged(e1);
    this.UnselectAll();
  }

  private void DeselectItems(object sender, ExecutedRoutedEventArgs e)
  {
    if (this._selectedListBox == null)
      return;
    List<object> removedItems = new List<object>();
    foreach (TransferItem removeItem in this._selectedListBox.Items.OfType<TransferItem>().ToList<TransferItem>())
    {
      if (removeItem.IsSelected && this.ItemContainerGenerator.ContainerFromItem(removeItem.Tag) is TransferItem transferItem)
      {
        this._selectedListBox.Items.Remove((object) removeItem);
        removedItems.Add(removeItem.Tag);
        transferItem.SetCurrentValue(TransferItem.IsTransferredProperty, ValueBoxes.FalseBox);
        transferItem.SetCurrentValue(ListBoxItem.IsSelectedProperty, ValueBoxes.FalseBox);
      }
    }
    this.SetTransferredItems((IEnumerable) this._selectedListBox.Items.OfType<TransferItem>().Select<TransferItem, object>((Func<TransferItem, object>) (item => item.Tag)));
    SelectionChangedEventArgs e1 = new SelectionChangedEventArgs(Transfer.TransferredItemsChangedEvent, (IList) removedItems, (IList) new List<object>());
    e1.Source = (object) this;
    this.OnTransferredItemsChanged(e1);
  }

  private void SetTransferredItems(IEnumerable selectedItems)
  {
    ManualObservableCollection<object> observableCollection = (ManualObservableCollection<object>) this.GetValue(Transfer.TransferredItemsImplProperty);
    if (observableCollection == null)
    {
      observableCollection = new ManualObservableCollection<object>();
      this.SetValue(Transfer.TransferredItemsPropertyKey, (object) observableCollection);
    }
    observableCollection.CanNotify = false;
    observableCollection.Clear();
    foreach (object selectedItem in selectedItems)
      observableCollection.Add(selectedItem);
    observableCollection.CanNotify = true;
  }
}
