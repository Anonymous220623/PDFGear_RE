// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Menus.ToolbarSettings.ToolbarSettingContentTemplateSelector
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Models.Menus;
using pdfeditor.Models.Menus.ToolbarSettings;
using pdfeditor.ViewModels;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace pdfeditor.Controls.Menus.ToolbarSettings;

internal class ToolbarSettingContentTemplateSelector : DataTemplateSelector
{
  public override DataTemplate SelectTemplate(object item, DependencyObject container)
  {
    switch (item)
    {
      case ToolbarSettingItemStrokeThicknessModel _:
        return this.StrokeThicknessPicker;
      case ToolbarSettingItemFontSizeModel _:
        return this.FontSizePicker;
      case ToolbarSettingItemIconModel _:
        return this.IconPicker;
      case ToolbarSettingItemTextEditingButtonsModel _:
        return this.TextEditingButtons;
      case ToolbarSettingItemExitModel _:
        return this.ExitEditingButton;
      case ToolbarSettingItemImageExitModel _:
        return this.ImageExitEditingButton;
      case ToolBarSettingTextBlock _:
        return this.TextBlock;
      case ToolbarSettingItemApplyToDefaultModel _:
        return this.ApplyToDefaultButton;
      case ToolbarSettingItemBoldModel _:
        return this.FontStyleButton;
      case ToolbarSettingItemItalicModel _:
        return this.FontStyleButton;
      case ToolbarSettingItemFontNameModel _:
        return this.FontNamePicker;
      case ToolbarSettingInkEraserModel _:
        return this.EraserPicker;
      case ToolbarSettingItemModel model:
        if (this.IsColorModel(model))
          return item is ToolbarSettingItemColorModel ? this.TextMarkupColorPicker : this.ColorPicker;
        break;
    }
    return base.SelectTemplate(item, container);
  }

  public DataTemplate TextMarkupColorPicker { get; set; }

  public DataTemplate ColorPicker { get; set; }

  public DataTemplate StrokeThicknessPicker { get; set; }

  public DataTemplate FontSizePicker { get; set; }

  public DataTemplate IconPicker { get; set; }

  public DataTemplate ExitEditingButton { get; set; }

  public DataTemplate ImageExitEditingButton { get; set; }

  public DataTemplate TextBlock { get; set; }

  public DataTemplate EraserPicker { get; set; }

  public DataTemplate ApplyToDefaultButton { get; set; }

  public DataTemplate FontStyleButton { get; set; }

  public DataTemplate FontNamePicker { get; set; }

  public DataTemplate TextEditingButtons { get; set; }

  private bool IsColorModel(ToolbarSettingItemModel model)
  {
    if (model == null)
      return false;
    return model.Type == ContextMenuItemType.StrokeColor || model.Type == ContextMenuItemType.FontColor || model.Type == ContextMenuItemType.FillColor;
  }

  private bool IsTextMarkupModel(ToolbarSettingItemModel model)
  {
    if (model == null)
      return false;
    return model.Id.AnnotationMode == AnnotationMode.Highlight || model.Id.AnnotationMode == AnnotationMode.Underline || model.Id.AnnotationMode == AnnotationMode.Strike || model.Id.AnnotationMode == AnnotationMode.HighlightArea;
  }
}
