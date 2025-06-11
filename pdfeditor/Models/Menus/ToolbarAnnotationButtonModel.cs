// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Menus.ToolbarAnnotationButtonModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Models.Menus.ToolbarSettings;
using pdfeditor.ViewModels;
using System;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Models.Menus;

public class ToolbarAnnotationButtonModel : ToolbarButtonModel
{
  private ToolbarChildButtonModel childButtonModel;
  private ToolbarSettingModel toolbarSettingModel;

  public ToolbarAnnotationButtonModel(AnnotationMode mode)
  {
    this.Mode = mode;
    this.Name = mode.ToString();
  }

  public AnnotationMode Mode { get; }

  protected override void OnChildButtonModelChanged(
    ToolbarChildButtonModel newValue,
    ToolbarChildButtonModel oldValue)
  {
    base.OnChildButtonModelChanged(newValue, oldValue);
    if (oldValue is ToolbarChildCheckableButtonModel source1)
      WeakEventManager<ToolbarChildCheckableButtonModel, SelectedAccessorSelectionChangedEventArgs>.RemoveHandler(source1, "ContextMenuSelectionChanged", new EventHandler<SelectedAccessorSelectionChangedEventArgs>(this.ChildButtonModel_ContextMenuSelectionChanged));
    if (newValue is ToolbarChildCheckableButtonModel source2)
      WeakEventManager<ToolbarChildCheckableButtonModel, SelectedAccessorSelectionChangedEventArgs>.AddHandler(source2, "ContextMenuSelectionChanged", new EventHandler<SelectedAccessorSelectionChangedEventArgs>(this.ChildButtonModel_ContextMenuSelectionChanged));
    this.UpdateIndicatorBrush();
  }

  public ToolbarSettingModel ToolbarSettingModel
  {
    get => this.toolbarSettingModel;
    set
    {
      this.SetProperty<ToolbarSettingModel>(ref this.toolbarSettingModel, value, nameof (ToolbarSettingModel));
    }
  }

  private void ChildButtonModel_ContextMenuSelectionChanged(
    object sender,
    SelectedAccessorSelectionChangedEventArgs e)
  {
    this.UpdateIndicatorBrush();
    EventHandler<SelectedAccessorSelectionChangedEventArgs> selectionChanged = this.ContextMenuSelectionChanged;
    if (selectionChanged == null)
      return;
    selectionChanged((object) this, e);
  }

  private void UpdateIndicatorBrush()
  {
    if (this.ChildButtonModel is ToolbarChildCheckableButtonModel childButtonModel && childButtonModel.ContextMenu is TypedContextMenuModel contextMenu)
    {
      string str = "";
      switch (this.Mode)
      {
        case AnnotationMode.Line:
        case AnnotationMode.Arrow:
        case AnnotationMode.Ink:
        case AnnotationMode.Highlight:
        case AnnotationMode.Underline:
        case AnnotationMode.Strike:
        case AnnotationMode.HighlightArea:
          if (contextMenu.SelectedItems.StrokeColor?.TagData?.MenuItemValue is string menuItemValue1)
          {
            str = menuItemValue1;
            break;
          }
          break;
        case AnnotationMode.Shape:
        case AnnotationMode.Ellipse:
        case AnnotationMode.TextBox:
        case AnnotationMode.Link:
          if (contextMenu.SelectedItems.StrokeColor?.TagData?.MenuItemValue is string menuItemValue2)
          {
            str = menuItemValue2;
            break;
          }
          break;
        case AnnotationMode.Text:
          if (contextMenu.SelectedItems.FontColor?.TagData?.MenuItemValue is string menuItemValue3)
          {
            str = menuItemValue3;
            break;
          }
          break;
      }
      if (string.IsNullOrEmpty(str))
      {
        this.IndicatorBrush = (SolidColorBrush) null;
      }
      else
      {
        try
        {
          this.IndicatorBrush = new SolidColorBrush((Color) ColorConverter.ConvertFromString(str));
        }
        catch
        {
          this.IndicatorBrush = (SolidColorBrush) null;
        }
      }
    }
    else
      this.IndicatorBrush = (SolidColorBrush) null;
  }

  public event EventHandler<SelectedAccessorSelectionChangedEventArgs> ContextMenuSelectionChanged;
}
