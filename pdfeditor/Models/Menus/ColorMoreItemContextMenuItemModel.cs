// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Menus.ColorMoreItemContextMenuItemModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Windows.Media;

#nullable disable
namespace pdfeditor.Models.Menus;

public class ColorMoreItemContextMenuItemModel : MoreItemContextMenuItemModel
{
  private Color? defaultColor;
  private string recentColorsKey;

  public Color? DefaultColor
  {
    get => this.defaultColor;
    set => this.SetProperty<Color?>(ref this.defaultColor, value, nameof (DefaultColor));
  }

  public string RecentColorsKey
  {
    get => this.recentColorsKey;
    set => this.SetProperty<string>(ref this.recentColorsKey, value, nameof (RecentColorsKey));
  }
}
