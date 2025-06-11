// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Menus.ToolbarSettings.ToolbarSettingModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

#nullable disable
namespace pdfeditor.Models.Menus.ToolbarSettings;

public class ToolbarSettingModel : ObservableCollection<ToolbarSettingItemModel>
{
  private ToolbarSettingId id;

  public ToolbarSettingModel(AnnotationMode mode)
  {
    this.id = ToolbarSettingId.CreateAnnotation(mode);
  }

  public ToolbarSettingModel(ToolbarSettingId id) => this.id = id;

  public ToolbarSettingId Id => this.id;

  protected override void InsertItem(int index, ToolbarSettingItemModel item)
  {
    item.Parent = this;
    base.InsertItem(index, item);
  }

  protected override void SetItem(int index, ToolbarSettingItemModel item)
  {
    ToolbarSettingItemModel settingItemModel = this[index];
    base.SetItem(index, item);
    settingItemModel.Parent = (ToolbarSettingModel) null;
    item.Parent = this;
  }

  protected override void RemoveItem(int index)
  {
    ToolbarSettingItemModel settingItemModel = this[index];
    base.RemoveItem(index);
    settingItemModel.Parent = (ToolbarSettingModel) null;
  }

  protected override void ClearItems()
  {
    foreach (ToolbarSettingItemModel settingItemModel in (Collection<ToolbarSettingItemModel>) this)
      settingItemModel.Parent = (ToolbarSettingModel) null;
    base.ClearItems();
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

  public async Task SaveConfigAsync()
  {
    await Task.WhenAll(this.Select<ToolbarSettingItemModel, Task>((Func<ToolbarSettingItemModel, Task>) (c => ((Func<Task>) (async () =>
    {
      try
      {
        await c.SaveConfigAsync().ConfigureAwait(false);
      }
      catch
      {
      }
    }))())));
  }

  public async Task LoadConfigAsync()
  {
    await Task.WhenAll(this.Select<ToolbarSettingItemModel, Task>((Func<ToolbarSettingItemModel, Task>) (c => ((Func<Task>) (async () =>
    {
      try
      {
        await c.LoadConfigAsync().ConfigureAwait(false);
      }
      catch
      {
      }
    }))())));
  }
}
