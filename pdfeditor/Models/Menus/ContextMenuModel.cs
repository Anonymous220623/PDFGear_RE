// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Menus.ContextMenuModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;

#nullable disable
namespace pdfeditor.Models.Menus;

public class ContextMenuModel : ContextMenuItemModel
{
  public override IContextMenuModel Parent
  {
    get => (IContextMenuModel) null;
    set => throw new ArgumentException(nameof (ContextMenuModel));
  }

  public override int Level => 0;
}
