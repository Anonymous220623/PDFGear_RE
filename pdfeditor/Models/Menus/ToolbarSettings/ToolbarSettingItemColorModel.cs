// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Menus.ToolbarSettings.ToolbarSettingItemColorModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Newtonsoft.Json;
using pdfeditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Models.Menus.ToolbarSettings;

public class ToolbarSettingItemColorModel : ToolbarSettingItemModel
{
  private string recentColorsKey;
  private ObservableCollection<string> standardColors;
  private ObservableCollection<string> recentColors;
  private bool init;

  public ToolbarSettingItemColorModel(ContextMenuItemType type)
    : base(type)
  {
  }

  public ToolbarSettingItemColorModel(ContextMenuItemType type, string configCacheKey)
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
      this.init = true;
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

  public ObservableCollection<string> RecentColors
  {
    get => this.recentColors;
    protected set
    {
      ObservableCollection<string> recentColors = this.recentColors;
      if (!this.SetProperty<ObservableCollection<string>>(ref this.recentColors, value, nameof (RecentColors)))
        return;
      if (recentColors != null)
        WeakEventManager<ObservableCollection<string>, CollectionChangeEventArgs>.RemoveHandler(recentColors, "CollectionChanged", new EventHandler<CollectionChangeEventArgs>(this.OnRecentColorsItemChanged));
      if (this.recentColors != null)
        WeakEventManager<ObservableCollection<string>, CollectionChangeEventArgs>.AddHandler(this.recentColors, "CollectionChanged", new EventHandler<CollectionChangeEventArgs>(this.OnRecentColorsItemChanged));
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

  private void OnRecentColorsItemChanged(object sender, CollectionChangeEventArgs e)
  {
    EventHandler colorsChanged = this.ColorsChanged;
    if (colorsChanged == null)
      return;
    colorsChanged((object) this, EventArgs.Empty);
  }

  protected override void OnSelectedValueChanged()
  {
    base.OnSelectedValueChanged();
    if (this.init && this.SelectedValue is string selectedValue)
      this.AddToRecentColor(selectedValue);
    this.init = true;
    if (this.Id.AnnotationMode != AnnotationMode.Ink)
      return;
    (this.Parent[3] as ToolbarSettingInkEraserModel).IsChecked = false;
  }

  private void AddToRecentColor(string color)
  {
    try
    {
      ObservableCollection<string> recentColors1 = this.RecentColors;
      List<string> list = (recentColors1 != null ? recentColors1.ToList<string>() : (List<string>) null) ?? new List<string>();
      Color color1 = (Color) ColorConverter.ConvertFromString(color);
      string str = $"#{color1.A:X2}{color1.R:X2}{color1.G:X2}{color1.B:X2}";
      ObservableCollection<string> standardColors = this.StandardColors;
      HashSet<Color> colorSet1 = new HashSet<Color>((standardColors != null ? standardColors.Select<string, Color?>((Func<string, Color?>) (x =>
      {
        try
        {
          return (Color?) ColorConverter.ConvertFromString(x);
        }
        catch
        {
        }
        return new Color?();
      })).Where<Color?>((Func<Color?, bool>) (x => x.HasValue)).Select<Color?, Color>((Func<Color?, Color>) (x => x.Value)).Distinct<Color>() : (IEnumerable<Color>) null) ?? Enumerable.Empty<Color>());
      ObservableCollection<string> recentColors2 = this.RecentColors;
      HashSet<Color> colorSet2 = new HashSet<Color>((recentColors2 != null ? recentColors2.Select<string, Color?>((Func<string, Color?>) (x =>
      {
        try
        {
          return (Color?) ColorConverter.ConvertFromString(x);
        }
        catch
        {
        }
        return new Color?();
      })).Where<Color?>((Func<Color?, bool>) (x => x.HasValue)).Select<Color?, Color>((Func<Color?, Color>) (x => x.Value)).Distinct<Color>() : (IEnumerable<Color>) null) ?? Enumerable.Empty<Color>());
      Color color2 = color1;
      if (colorSet1.Contains(color2) || colorSet2.Contains(color1))
        return;
      list.Add(str);
      for (int index = 0; index < list.Count - 5; ++index)
        list.RemoveAt(index);
      this.RecentColors = new ObservableCollection<string>(list);
    }
    catch
    {
    }
  }

  public event EventHandler ColorsChanged;

  protected override void SaveConfigCore(Dictionary<string, string> dict)
  {
    base.SaveConfigCore(dict);
    dict["SelectedValue"] = (string) this.NontransientSelectedValue;
    Dictionary<string, string> dictionary = dict;
    ObservableCollection<string> recentColors = this.recentColors;
    string str = JsonConvert.SerializeObject((object) ((recentColors != null ? (IEnumerable<string>) recentColors.ToList<string>() : (IEnumerable<string>) null) ?? Enumerable.Empty<string>()));
    dictionary["RecentColors"] = str;
  }

  protected override void ApplyConfigCore(Dictionary<string, string> dict)
  {
    base.ApplyConfigCore(dict);
    string str1;
    if (dict.TryGetValue("SelectedValue", out str1))
      this.NontransientSelectedValue = (object) str1;
    string str2;
    if (!dict.TryGetValue("RecentColors", out str2))
      return;
    if (!string.IsNullOrEmpty(str2))
    {
      if (!(str2 == "[]"))
      {
        try
        {
          this.RecentColors = new ObservableCollection<string>((IEnumerable<string>) JsonConvert.DeserializeObject<string[]>(str2));
          return;
        }
        catch
        {
          return;
        }
      }
    }
    this.RecentColors = (ObservableCollection<string>) null;
  }
}
