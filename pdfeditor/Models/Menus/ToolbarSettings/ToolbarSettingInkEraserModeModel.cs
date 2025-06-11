// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Menus.ToolbarSettings.ToolbarSettingInkEraserModeModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

#nullable disable
namespace pdfeditor.Models.Menus.ToolbarSettings;

public class ToolbarSettingInkEraserModeModel : ToolbarSettingItemModel
{
  private bool isCheckable = true;
  private bool isChecked;

  public ToolbarSettingInkEraserModeModel()
    : base(ContextMenuItemType.None)
  {
  }

  public bool IsCheckable
  {
    get => this.isCheckable;
    set
    {
      if (!this.SetProperty<bool>(ref this.isCheckable, value, nameof (IsCheckable)) || value || !this.IsChecked)
        return;
      this.IsChecked = false;
    }
  }

  public bool IsChecked
  {
    get => this.isChecked;
    set => this.SetProperty<bool>(ref this.isChecked, value, nameof (IsChecked));
  }
}
