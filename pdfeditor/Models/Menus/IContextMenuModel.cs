// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Menus.IContextMenuModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.ComponentModel;

#nullable disable
namespace pdfeditor.Models.Menus;

public interface IContextMenuModel : INotifyPropertyChanging, INotifyPropertyChanged
{
  string Name { get; }

  IContextMenuModel Parent { get; set; }

  int Level { get; }
}
