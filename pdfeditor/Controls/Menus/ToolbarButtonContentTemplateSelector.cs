// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Menus.ToolbarButtonContentTemplateSelector
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Models.Menus;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace pdfeditor.Controls.Menus;

public class ToolbarButtonContentTemplateSelector : DataTemplateSelector
{
  public override DataTemplate SelectTemplate(object item, DependencyObject container)
  {
    switch (item)
    {
      case ToolbarButtonModel toolbarButtonModel:
        if (toolbarButtonModel.ChildButtonModel is ToolbarChildCheckableButtonModel)
          return this.ToggleButtonTemplate;
        if (toolbarButtonModel.ChildButtonModel != null || toolbarButtonModel.ChildButtonModel == null)
          return this.TextTemplate;
        break;
      case string _:
        return this.PlainTextTemplate;
    }
    return base.SelectTemplate(item, container);
  }

  public DataTemplate TextTemplate { get; set; }

  public DataTemplate ToggleButtonTemplate { get; set; }

  public DataTemplate PlainTextTemplate { get; set; }
}
