// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Menus.TagDataModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Models.Menus;
using pdfeditor.ViewModels;

#nullable disable
namespace pdfeditor.Controls.Menus;

public class TagDataModel
{
  public TagDataModel()
  {
  }

  public TagDataModel(bool isTransient) => this.IsTransient = isTransient;

  public ContextMenuItemType MenuItemType { get; set; }

  public object MenuItemValue { get; set; }

  public AnnotationMode AnnotationMode { get; set; }

  public bool IsTransient { get; }
}
