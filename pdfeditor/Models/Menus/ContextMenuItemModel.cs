// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Menus.ContextMenuItemModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom.HotKeys;
using pdfeditor.Controls.Menus;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Models.Menus;

public class ContextMenuItemModel : 
  ObservableCollection<IContextMenuModel>,
  IContextMenuModel,
  INotifyPropertyChanging,
  INotifyPropertyChanged
{
  private IContextMenuModel parent;
  private ImageSource icon;
  private string name;
  private string caption;
  private bool isEnabled = true;
  private bool isChecked;
  private bool? isCheckable;
  private TagDataModel tagData;
  private bool isEndPoint = true;
  private ICommand command;
  private string hotKeyInvokeWhen = string.Empty;
  private string hotKeyString = string.Empty;
  private HotKeyInvokeAction hotKeyInvokeAction = HotKeyInvokeAction.Invoke;

  public virtual IContextMenuModel Parent
  {
    get => this.parent;
    set
    {
      if (this.parent != value)
        this.OnPropertyChanging("Level");
      if (!this.SetProperty<IContextMenuModel>(ref this.parent, value, nameof (Parent)))
        return;
      this.OnPropertyChanged("Level");
    }
  }

  public virtual int Level
  {
    get
    {
      IContextMenuModel parent = this.Parent;
      return parent == null ? -1 : parent.Level + 1;
    }
  }

  public bool IsEndPoint
  {
    get => this.isEndPoint;
    private set
    {
      if (!this.SetProperty<bool>(ref this.isEndPoint, value, nameof (IsEndPoint)))
        return;
      this.OnPropertyChanged("IsCheckable");
    }
  }

  public ImageSource Icon
  {
    get => this.icon;
    set => this.SetProperty<ImageSource>(ref this.icon, value, nameof (Icon));
  }

  public string Name
  {
    get => this.name;
    set => this.SetProperty<string>(ref this.name, value, nameof (Name));
  }

  public string Caption
  {
    get => this.caption;
    set => this.SetProperty<string>(ref this.caption, value, nameof (Caption));
  }

  public bool IsEnabled
  {
    get => this.isEnabled;
    set => this.SetProperty<bool>(ref this.isEnabled, value, nameof (IsEnabled));
  }

  public virtual bool IsChecked
  {
    get => this.isChecked;
    set => this.SetProperty<bool>(ref this.isChecked, value, nameof (IsChecked));
  }

  public virtual bool IsCheckable
  {
    get => this.isCheckable ?? this.IsEndPoint;
    set => this.SetProperty<bool?>(ref this.isCheckable, new bool?(value), nameof (IsCheckable));
  }

  public TagDataModel TagData
  {
    get => this.tagData;
    set => this.SetProperty<TagDataModel>(ref this.tagData, value, nameof (TagData));
  }

  public ICommand Command
  {
    get => this.command;
    set => this.SetProperty<ICommand>(ref this.command, value, nameof (Command));
  }

  public string HotKeyInvokeWhen
  {
    get => this.hotKeyInvokeWhen;
    set => this.SetProperty<string>(ref this.hotKeyInvokeWhen, value, nameof (HotKeyInvokeWhen));
  }

  public string HotKeyString
  {
    get => this.hotKeyString;
    set => this.SetProperty<string>(ref this.hotKeyString, value, nameof (HotKeyString));
  }

  public HotKeyInvokeAction HotKeyInvokeAction
  {
    get => this.hotKeyInvokeAction;
    set
    {
      this.SetProperty<HotKeyInvokeAction>(ref this.hotKeyInvokeAction, value, nameof (HotKeyInvokeAction));
    }
  }

  protected override void InsertItem(int index, IContextMenuModel item)
  {
    if (item is ContextMenuModel)
      throw new ArgumentException("ContextMenuModel");
    item.Parent = (IContextMenuModel) this;
    base.InsertItem(index, item);
  }

  protected override void SetItem(int index, IContextMenuModel item)
  {
    if (item is ContextMenuModel)
      throw new ArgumentException("ContextMenuModel");
    IContextMenuModel contextMenuModel = this[index];
    base.SetItem(index, item);
    contextMenuModel.Parent = (IContextMenuModel) null;
    item.Parent = (IContextMenuModel) this;
  }

  protected override void RemoveItem(int index)
  {
    IContextMenuModel contextMenuModel = this[index];
    base.RemoveItem(index);
    contextMenuModel.Parent = (IContextMenuModel) null;
  }

  protected override void ClearItems()
  {
    foreach (IContextMenuModel contextMenuModel in (Collection<IContextMenuModel>) this)
      contextMenuModel.Parent = (IContextMenuModel) null;
    base.ClearItems();
  }

  protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
  {
    base.OnCollectionChanged(e);
    this.IsEndPoint = this.Count == 0;
  }

  public event PropertyChangingEventHandler PropertyChanging;

  protected void OnPropertyChanging(PropertyChangingEventArgs e)
  {
    PropertyChangingEventHandler propertyChanging = this.PropertyChanging;
    if (propertyChanging == null)
      return;
    propertyChanging((object) this, e);
  }

  protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
  {
    this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
  }

  protected void OnPropertyChanging([CallerMemberName] string propertyName = null)
  {
    this.OnPropertyChanging(new PropertyChangingEventArgs(propertyName));
  }

  protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
  {
    if (EqualityComparer<T>.Default.Equals(field, newValue))
      return false;
    this.OnPropertyChanging(propertyName);
    field = newValue;
    this.OnPropertyChanged(propertyName);
    return true;
  }

  protected bool SetProperty<T>(
    ref T field,
    T newValue,
    IEqualityComparer<T> comparer,
    [CallerMemberName] string propertyName = null)
  {
    if (comparer.Equals(field, newValue))
      return false;
    this.OnPropertyChanging(propertyName);
    field = newValue;
    this.OnPropertyChanged(propertyName);
    return true;
  }
}
