// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Menus.ToolbarSettings.ToolbarSettingItemApplyToDefaultModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

#nullable disable
namespace pdfeditor.Models.Menus.ToolbarSettings;

public class ToolbarSettingItemApplyToDefaultModel : ToolbarSettingItemModel
{
  public ToolbarSettingItemApplyToDefaultModel()
    : base(ContextMenuItemType.None)
  {
    this.InitCommand();
  }

  private void InitCommand()
  {
    this.Command = (ICommand) new RelayCommand((Action) (() =>
    {
      if (!this.InTransientScope)
        return;
      ToolbarSettingModel parent = this.Parent;
      if (parent == null)
        return;
      foreach (ToolbarSettingItemModel settingItemModel in (Collection<ToolbarSettingItemModel>) parent)
        settingItemModel.ApplyTransient(false);
    }));
  }
}
