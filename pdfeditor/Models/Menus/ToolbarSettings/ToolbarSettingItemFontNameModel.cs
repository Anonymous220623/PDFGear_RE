// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Menus.ToolbarSettings.ToolbarSettingItemFontNameModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using PDFKit.Contents;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace pdfeditor.Models.Menus.ToolbarSettings;

public class ToolbarSettingItemFontNameModel : ToolbarSettingItemModel
{
  private static IReadOnlyList<string> _allFontNames;
  private static object locker = new object();

  private static IReadOnlyList<string> _AllFontNames
  {
    get
    {
      if (ToolbarSettingItemFontNameModel._allFontNames == null)
      {
        lock (ToolbarSettingItemFontNameModel.locker)
        {
          if (ToolbarSettingItemFontNameModel._allFontNames == null)
            ToolbarSettingItemFontNameModel._allFontNames = (IReadOnlyList<string>) ((IEnumerable<WindowsFontFamily>) WindowsFonts.GetAllFontFamilies()).Select<WindowsFontFamily, string>((Func<WindowsFontFamily, string>) (c => c.Name)).ToList<string>();
        }
      }
      return ToolbarSettingItemFontNameModel._allFontNames;
    }
  }

  public ToolbarSettingItemFontNameModel(ContextMenuItemType type)
    : base(type)
  {
    this.NontransientSelectedValue = (object) "Arial";
  }

  public ToolbarSettingItemFontNameModel(ContextMenuItemType type, string configCacheKey)
    : base(type, configCacheKey)
  {
    this.NontransientSelectedValue = (object) "Arial";
  }

  public IReadOnlyList<string> AllFonts => ToolbarSettingItemFontNameModel._AllFontNames;
}
