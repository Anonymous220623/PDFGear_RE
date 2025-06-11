// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Menus.ToolbarSettings.ToolBarSettingTextBlock
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

#nullable disable
namespace pdfeditor.Models.Menus.ToolbarSettings;

public class ToolBarSettingTextBlock : ToolbarSettingItemModel
{
  private string text;

  public ToolBarSettingTextBlock()
    : this((string) null)
  {
  }

  public ToolBarSettingTextBlock(string text)
    : base(ContextMenuItemType.None)
  {
    if (!string.IsNullOrEmpty(text))
      this.text = text;
    else
      this.text = "You are in editing mode.";
  }

  public string Text
  {
    get => this.text;
    set => this.SetProperty<string>(ref this.text, value, nameof (Text));
  }
}
