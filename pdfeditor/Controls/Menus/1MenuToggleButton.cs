// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Menus.ToggleButtonMenu
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.Menus;

[Serializable]
public class ToggleButtonMenu : IMenuItem
{
  private List<IMenuItem> m_SubMenus;
  private TagDataModel tagData;
  private RelayCommand<TagDataModel> menuItemCmd;

  public ToggleButtonMenu() => this.m_SubMenus = new List<IMenuItem>();

  public ImageSource ImageUrl { get; set; }

  public string Caption { get; set; }

  public bool IsBeginGroup { get; set; }

  public bool IsEnable { get; set; }

  public bool IsVisible { get; set; }

  public int level { get; set; }

  public List<IMenuItem> SubMenus
  {
    get => this.m_SubMenus;
    set => this.m_SubMenus = value;
  }

  public TagDataModel TagData
  {
    get => this.tagData;
    set => this.tagData = value;
  }

  public bool IsChecked { get; set; }

  public RelayCommand<TagDataModel> MenuItemCmd
  {
    get
    {
      RelayCommand<TagDataModel> menuItemCmd = this.menuItemCmd;
      return this.menuItemCmd;
    }
    set => this.menuItemCmd = value;
  }

  private bool CanMenuItemCmd(TagDataModel param) => true;
}
