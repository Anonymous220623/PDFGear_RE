// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Annotations.CircleAnnotation
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Models.Menus;
using pdfeditor.ViewModels;

#nullable disable
namespace pdfeditor.Models.Annotations;

public class CircleAnnotation : BaseFigureAnnotation
{
  protected override bool EqualsCore(BaseAnnotation other)
  {
    return base.EqualsCore(other) && other is CircleAnnotation;
  }

  public override object GetValue(AnnotationMode mode, ContextMenuItemType type)
  {
    if (mode != AnnotationMode.Ellipse)
      return base.GetValue(mode, type);
    switch (type)
    {
      case ContextMenuItemType.StrokeColor:
        return (object) this.Color;
      case ContextMenuItemType.FillColor:
        return (object) this.InteriorColor;
      case ContextMenuItemType.StrokeThickness:
        PdfBorderStyleModel borderStyle = this.BorderStyle;
        return (object) (float) (borderStyle != null ? (double) borderStyle.Width : 1.0);
      default:
        return (object) null;
    }
  }
}
