// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Menus.ToolbarSettings.ToolbarSettingId
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.ViewModels;
using System;

#nullable disable
namespace pdfeditor.Models.Menus.ToolbarSettings;

public class ToolbarSettingId : IEquatable<ToolbarSettingId>
{
  private static ToolbarSettingId none;
  private ToolbarSettingType type;
  private AnnotationMode annotationMode;

  public static ToolbarSettingId None
  {
    get
    {
      ToolbarSettingId none1 = ToolbarSettingId.none;
      if ((object) none1 != null)
        return none1;
      ToolbarSettingId none2 = new ToolbarSettingId();
      none2.type = ToolbarSettingType.None;
      none2.annotationMode = AnnotationMode.None;
      ToolbarSettingId.none = none2;
      return none2;
    }
  }

  private ToolbarSettingId()
  {
  }

  public ToolbarSettingType Type => this.type;

  public AnnotationMode AnnotationMode => this.annotationMode;

  public static ToolbarSettingId CreateAnnotation(AnnotationMode annotationMode)
  {
    return new ToolbarSettingId()
    {
      type = ToolbarSettingType.Annotation,
      annotationMode = annotationMode
    };
  }

  public static ToolbarSettingId CreateEditDocument()
  {
    return new ToolbarSettingId()
    {
      type = ToolbarSettingType.EditDocument
    };
  }

  public static bool operator ==(ToolbarSettingId left, ToolbarSettingId right)
  {
    if ((object) left == null && (object) right == null)
      return true;
    return (object) left != null && (object) right != null && left.Equals(right);
  }

  public static bool operator !=(ToolbarSettingId left, ToolbarSettingId right) => !(left == right);

  public override int GetHashCode()
  {
    return HashCode.Combine<ToolbarSettingType, AnnotationMode>(this.type, this.annotationMode);
  }

  public override bool Equals(object obj)
  {
    ToolbarSettingId toolbarSettingId = obj as ToolbarSettingId;
    return (object) toolbarSettingId != null && toolbarSettingId.Equals(this);
  }

  public bool Equals(ToolbarSettingId other)
  {
    return this.type == other.type && this.annotationMode == other.annotationMode;
  }
}
