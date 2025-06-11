// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.ColorPickers.ColorFamilyListViewItemTemplateSelector
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace pdfeditor.Controls.ColorPickers;

public class ColorFamilyListViewItemTemplateSelector : DataTemplateSelector
{
  public override DataTemplate SelectTemplate(object item, DependencyObject container)
  {
    return item is ColorValue colorValue ? (colorValue.Color.A != (byte) 0 ? this.SolidColorTemplate : this.TransparentTemplate) : (item == null ? this.TransparentTemplate : base.SelectTemplate(item, container));
  }

  public DataTemplate SolidColorTemplate { get; set; }

  public DataTemplate TransparentTemplate { get; set; }
}
