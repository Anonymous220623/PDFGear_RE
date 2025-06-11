// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Menus.SpeedContextMenuItemModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.AppTheme;
using System;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Models.Menus;

public class SpeedContextMenuItemModel : ContextMenuItemModel
{
  public override bool IsChecked
  {
    get => base.IsChecked;
    set
    {
      base.IsChecked = value;
      if (!value)
        this.Icon = (ImageSource) null;
      else
        this.Icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Speech/Checked.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Speech/Checked.png"));
    }
  }
}
