// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Menus.ToolbarSettings.ToolbarSettingItemColorCollapseModel
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

public class ToolbarSettingItemColorCollapseModel : ToolbarSettingItemModel
{
  private string recentColorsKey;
  private ObservableCollection<string> standardColors;

  public ToolbarSettingItemColorCollapseModel(ContextMenuItemType type)
    : base(type)
  {
  }

  public ToolbarSettingItemColorCollapseModel(ContextMenuItemType type, string configCacheKey)
    : base(type, configCacheKey)
  {
  }

  public string RecentColorsKey
  {
    get => this.recentColorsKey;
    set => this.SetProperty<string>(ref this.recentColorsKey, value, nameof (RecentColorsKey));
  }

  public ObservableCollection<string> StandardColors
  {
    get => this.standardColors;
    set
    {
      ObservableCollection<string> standardColors = this.standardColors;
      if (!this.SetProperty<ObservableCollection<string>>(ref this.standardColors, value, nameof (StandardColors)))
        return;
      if (standardColors != null)
        WeakEventManager<ObservableCollection<string>, CollectionChangeEventArgs>.RemoveHandler(standardColors, "CollectionChanged", new EventHandler<CollectionChangeEventArgs>(this.OnStandardColorsItemChanged));
      if (this.standardColors != null)
        WeakEventManager<ObservableCollection<string>, CollectionChangeEventArgs>.AddHandler(this.standardColors, "CollectionChanged", new EventHandler<CollectionChangeEventArgs>(this.OnStandardColorsItemChanged));
      EventHandler colorsChanged = this.ColorsChanged;
      if (colorsChanged == null)
        return;
      colorsChanged((object) this, EventArgs.Empty);
    }
  }

  private void OnStandardColorsItemChanged(object sender, CollectionChangeEventArgs e)
  {
    EventHandler colorsChanged = this.ColorsChanged;
    if (colorsChanged == null)
      return;
    colorsChanged((object) this, EventArgs.Empty);
  }

  public event EventHandler ColorsChanged;

  protected override void SaveConfigCore(Dictionary<string, string> dict)
  {
    base.SaveConfigCore(dict);
    dict["SelectedValue"] = (string) this.NontransientSelectedValue;
  }

  protected override void ApplyConfigCore(Dictionary<string, string> dict)
  {
    base.ApplyConfigCore(dict);
    string str;
    if (!dict.TryGetValue("SelectedValue", out str))
      return;
    this.NontransientSelectedValue = (object) str;
  }
}
