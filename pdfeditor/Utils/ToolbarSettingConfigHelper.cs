// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.ToolbarSettingConfigHelper
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Config;
using pdfeditor.Models.Menus;
using pdfeditor.Models.Menus.ToolbarSettings;
using pdfeditor.ViewModels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace pdfeditor.Utils;

internal static class ToolbarSettingConfigHelper
{
  private const string ToolbarSettingConfigTemplate = "ToolbarSettingConfig_{0}_{1}";
  private const string ToolbarSettingColorPickerRecentKeyTemplate = "TS_RecentColors_{0}_{1}";

  public static string BuildConfigKey(ToolbarSettingId id, ContextMenuItemType type)
  {
    if (id.Type == ToolbarSettingType.Annotation)
      return $"ToolbarSettingConfig_{(int) id.AnnotationMode}_{(int) type}";
    return id.Type == ToolbarSettingType.EditDocument ? $"ToolbarSettingConfig_{"editdoc"}_{(int) type}" : string.Empty;
  }

  public static string BuildRecentColorKey(ToolbarSettingId id, ContextMenuItemType type)
  {
    if (id.Type == ToolbarSettingType.Annotation)
      return ToolbarSettingConfigHelper.BuildRecentColorKey(id.AnnotationMode, type);
    return id.Type == ToolbarSettingType.EditDocument ? $"TS_RecentColors_{"editdoc"}_{(int) type}" : string.Empty;
  }

  public static string BuildRecentColorKey(AnnotationMode mode, ContextMenuItemType type)
  {
    return $"TS_RecentColors_{(int) mode}_{(int) type}";
  }

  public static async Task SaveConfigAsync(string key, Dictionary<string, string> dict)
  {
    if (string.IsNullOrEmpty(key))
      return;
    if (!key.StartsWith("ToolbarSettingConfig_"))
      return;
    try
    {
      int num = await ConfigUtils.TrySetAsync<Dictionary<string, string>>(key, dict, new CancellationToken()).ConfigureAwait(false) ? 1 : 0;
    }
    catch
    {
    }
  }

  public static async Task<Dictionary<string, string>> LoadConfigAsync(string key)
  {
    int num;
    if (num != 0 && (string.IsNullOrEmpty(key) || !key.StartsWith("ToolbarSettingConfig_")))
      return (Dictionary<string, string>) null;
    try
    {
      (bool, Dictionary<string, string>) valueTuple = await ConfigUtils.TryGetAsync<Dictionary<string, string>>(key, new CancellationToken()).ConfigureAwait(false);
      if (valueTuple.Item1)
        return valueTuple.Item2;
    }
    catch
    {
    }
    return (Dictionary<string, string>) null;
  }
}
