// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Menus.IMenuItem
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Collections.Generic;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.Menus;

public interface IMenuItem
{
  ImageSource ImageUrl { get; set; }

  string Caption { get; set; }

  bool IsBeginGroup { get; set; }

  bool IsEnable { get; set; }

  bool IsVisible { get; set; }

  List<IMenuItem> SubMenus { get; set; }

  int level { get; set; }

  bool IsChecked { get; set; }

  TagDataModel TagData { get; set; }
}
