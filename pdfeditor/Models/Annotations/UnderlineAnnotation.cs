// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Annotations.UnderlineAnnotation
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Models.Menus;
using pdfeditor.ViewModels;

#nullable disable
namespace pdfeditor.Models.Annotations;

public class UnderlineAnnotation : BaseTextMarkupAnnotation
{
  protected override bool EqualsCore(BaseAnnotation other)
  {
    return base.EqualsCore(other) && other is UnderlineAnnotation;
  }

  public override object GetValue(AnnotationMode mode, ContextMenuItemType type)
  {
    if (mode != AnnotationMode.Underline)
      return base.GetValue(mode, type);
    return type == ContextMenuItemType.StrokeColor ? (object) this.Color : (object) null;
  }
}
