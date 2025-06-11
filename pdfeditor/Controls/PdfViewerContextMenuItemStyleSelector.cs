// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PdfViewerContextMenuItemStyleSelector
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Models.Menus;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace pdfeditor.Controls;

public class PdfViewerContextMenuItemStyleSelector : StyleSelector
{
  public override Style SelectStyle(object item, DependencyObject container)
  {
    switch (item)
    {
      case TypedContextMenuItemModel contextMenuItemModel1 when contextMenuItemModel1.Type == ContextMenuItemType.StrokeColor || contextMenuItemModel1.Type == ContextMenuItemType.FillColor:
        return this.ColorMenuStyle;
      case ContextMenuItemModel contextMenuItemModel2 when contextMenuItemModel2.TagData != null && (contextMenuItemModel2.TagData.MenuItemType == ContextMenuItemType.FillColor || contextMenuItemModel2.TagData.MenuItemType == ContextMenuItemType.StrokeColor):
        return this.ColorItemStyle;
      case ContextMenuSeparator _:
        return this.SeparatorStyle;
      case ContextMenuHorizontalButton _:
        return this.HorizontalButtonStyle;
      default:
        return base.SelectStyle(item, container);
    }
  }

  public Style ColorItemStyle { get; set; }

  public Style ColorMenuStyle { get; set; }

  public Style SeparatorStyle { get; set; }

  public Style HorizontalButtonStyle { get; set; }
}
