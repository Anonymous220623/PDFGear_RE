// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Menus.ToolbarButtonContextMenuStyleSelector
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Models.Menus;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace pdfeditor.Controls.Menus;

public class ToolbarButtonContextMenuStyleSelector : StyleSelector
{
  public override Style SelectStyle(object item, DependencyObject container)
  {
    switch (item)
    {
      case ContextMenuSeparator _:
        return this.SeparatorStyle;
      case SpeedContextMenuItemModel _:
      case SpeechToolContextMenuItemModel _:
        return this.SpeedPresetsMenuItem;
      case StampContextMenuItemModel _:
        return this.StampItemStyle;
      case PresetsItemContextMenuItemModel _:
        return this.StampPresetsMenuItem;
      case StampCustomMenuItemModel _:
        return this.StampPickerItemStyle;
      case ConvertContextMenuItemModel _:
        return this.ConvertItemStyle;
      case BackgroundContextMenuItemModel _:
        return this.BackgroundStyle;
      case ColorMoreItemContextMenuItemModel _:
        return this.ColorMoreItemStyle;
      case ContextMenuItemModel contextMenuItemModel1 when object.Equals((object) contextMenuItemModel1.Name, (object) "ColorPicker"):
        return this.ColorPickerItemStyle;
      case ContextMenuItemModel contextMenuItemModel2 when object.Equals((object) contextMenuItemModel2.Name, (object) "SignaturePicker"):
        return this.SignaturePickerItemStyle;
      case ContextMenuItemModel contextMenuItemModel3 when contextMenuItemModel3.Parent is TypedContextMenuItemModel parent:
        switch (parent.Type)
        {
          case ContextMenuItemType.StrokeColor:
          case ContextMenuItemType.FillColor:
          case ContextMenuItemType.FontColor:
            return this.ColorItemStyle;
          case ContextMenuItemType.StrokeThickness:
          case ContextMenuItemType.FontSize:
            return this.StrokeThicknessItemStyle;
        }
        break;
    }
    return this.DefaultItemStyle ?? base.SelectStyle(item, container);
  }

  public Style StrokeThicknessItemStyle { get; set; }

  public Style ColorItemStyle { get; set; }

  public Style DefaultItemStyle { get; set; }

  public Style StampItemStyle { get; set; }

  public Style ConvertItemStyle { get; set; }

  public Style BackgroundStyle { get; set; }

  public Style StampPresetsMenuItem { get; set; }

  public Style ColorMoreItemStyle { get; set; }

  public Style ColorPickerItemStyle { get; set; }

  public Style SignaturePickerItemStyle { get; set; }

  public Style SeparatorStyle { get; set; }

  public Style StampPickerItemStyle { get; set; }

  public Style SpeedPresetsMenuItem { get; set; }
}
