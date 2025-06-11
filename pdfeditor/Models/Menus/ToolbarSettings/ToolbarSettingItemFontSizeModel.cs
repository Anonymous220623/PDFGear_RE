// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Menus.ToolbarSettings.ToolbarSettingItemFontSizeModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

#nullable disable
namespace pdfeditor.Models.Menus.ToolbarSettings;

public class ToolbarSettingItemFontSizeModel : ToolbarSettingItemModel
{
  private ObservableCollection<float> standardItems;

  public ToolbarSettingItemFontSizeModel(ContextMenuItemType type)
    : base(type)
  {
  }

  public ToolbarSettingItemFontSizeModel(ContextMenuItemType type, string configCacheKey)
    : base(type, configCacheKey)
  {
  }

  protected override void OnSelectedValueChanged() => base.OnSelectedValueChanged();

  public ObservableCollection<float> StandardItems
  {
    get => this.standardItems;
    set
    {
      ObservableCollection<float> standardItems = this.standardItems;
      if (!this.SetProperty<ObservableCollection<float>>(ref this.standardItems, value, nameof (StandardItems)))
        return;
      if (standardItems != null)
        WeakEventManager<ObservableCollection<float>, CollectionChangeEventArgs>.RemoveHandler(standardItems, "CollectionChanged", new EventHandler<CollectionChangeEventArgs>(this.OnStandardColorsItemChanged));
      if (this.standardItems != null)
        WeakEventManager<ObservableCollection<float>, CollectionChangeEventArgs>.AddHandler(this.standardItems, "CollectionChanged", new EventHandler<CollectionChangeEventArgs>(this.OnStandardColorsItemChanged));
      EventHandler standardItemsChanged = this.StandardItemsChanged;
      if (standardItemsChanged == null)
        return;
      standardItemsChanged((object) this, EventArgs.Empty);
    }
  }

  private void OnStandardColorsItemChanged(object sender, CollectionChangeEventArgs e)
  {
    EventHandler standardItemsChanged = this.StandardItemsChanged;
    if (standardItemsChanged == null)
      return;
    standardItemsChanged((object) this, EventArgs.Empty);
  }

  public event EventHandler StandardItemsChanged;

  protected override void SaveConfigCore(Dictionary<string, string> dict)
  {
    base.SaveConfigCore(dict);
    dict["SelectedValue"] = this.NontransientSelectedValue.ToString();
  }

  protected override void ApplyConfigCore(Dictionary<string, string> dict)
  {
    base.ApplyConfigCore(dict);
    string s;
    float result;
    if (!dict.TryGetValue("SelectedValue", out s) || !float.TryParse(s, out result))
      return;
    this.NontransientSelectedValue = (object) result;
  }
}
