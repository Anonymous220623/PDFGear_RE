// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.SetDefaultAppUtils
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using pdfeditor.Controls;
using pdfeditor.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

#nullable disable
namespace pdfeditor.Utils;

internal static class SetDefaultAppUtils
{
  public static async Task TrySetDefaultAppAsync()
  {
    if (AppIdHelper.HasUserChoiceLatest || ((IEnumerable<string>) AppManager.GetDefaultFileExts()).All<string>((Func<string, bool>) (c => AppIdHelper.GetDefaultAppProgId(c) == "PdfGear.App.1")))
      return;
    string action = await ConfigManager.GetDefaultAppActionAsync();
    if (!action.StartsWith("Silence_"))
    {
      SetDefaultAppDialog defaultAppDialog1 = new SetDefaultAppDialog();
      defaultAppDialog1.Owner = (Window) App.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>();
      SetDefaultAppDialog defaultAppDialog2 = defaultAppDialog1;
      defaultAppDialog2.WindowStartupLocation = defaultAppDialog2.Owner == null ? WindowStartupLocation.CenterScreen : WindowStartupLocation.CenterOwner;
      defaultAppDialog2.ShowDialog();
      action = defaultAppDialog2.Action;
      await ConfigManager.SetDefaultAppActionAsync(action);
    }
    GAManager.SendEvent("ExtDefaultApp", action, "Count", 1L);
    switch (action)
    {
      case "Silence_SetDefault":
      case "SetDefault":
        AppManager.RegisterFileAssociations(true);
        break;
    }
    action = (string) null;
  }
}
