// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Menus.ToolbarSettings.ToolbarSettingItemItalicModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Collections.Generic;

#nullable disable
namespace pdfeditor.Models.Menus.ToolbarSettings;

internal class ToolbarSettingItemItalicModel : ToolbarSettingItemModel
{
  public ToolbarSettingItemItalicModel(ContextMenuItemType type)
    : base(type)
  {
  }

  public ToolbarSettingItemItalicModel(ContextMenuItemType type, string configCacheKey)
    : base(type, configCacheKey)
  {
  }

  protected override void SaveConfigCore(Dictionary<string, string> dict)
  {
    base.SaveConfigCore(dict);
    dict["SelectedValue"] = !(this.NontransientSelectedValue is bool nontransientSelectedValue) || !nontransientSelectedValue ? "false" : "true";
  }

  protected override void ApplyConfigCore(Dictionary<string, string> dict)
  {
    base.ApplyConfigCore(dict);
    string str;
    if (!dict.TryGetValue("SelectedValue", out str))
      return;
    this.NontransientSelectedValue = (object) (str == "true");
  }
}
