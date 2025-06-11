// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Annotations.PdfAnnotationPropertyAccessor
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Net.Annotations;
using pdfeditor.Models.Menus;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using System;
using System.Linq;

#nullable disable
namespace pdfeditor.Models.Annotations;

public class PdfAnnotationPropertyAccessor
{
  private readonly BaseAnnotation annotation;

  public PdfAnnotationPropertyAccessor(PdfAnnotation pdfAnnotation, AnnotationMode mode)
  {
    if (pdfAnnotation == null)
      throw new ArgumentNullException(nameof (pdfAnnotation));
    this.Mode = mode;
    if (this.Mode == AnnotationMode.None)
      return;
    this.annotation = AnnotationFactory.Create(pdfAnnotation);
  }

  public AnnotationMode Mode { get; }

  public object GetValue(ContextMenuItemType type)
  {
    return this.annotation == null ? (object) null : this.annotation.GetValue(this.Mode, type);
  }

  public ContextMenuItemModel GetOrCreateContextMenuItem(
    TypedContextMenuItemModel menu,
    Action<ContextMenuItemModel> action)
  {
    if (menu == null)
      throw new ArgumentNullException(nameof (menu));
    if (!(menu.Parent is TypedContextMenuModel parent) || parent.Mode != this.Mode)
      return (ContextMenuItemModel) null;
    ContextMenuItemType type = menu.Type;
    if (type == ContextMenuItemType.None)
      return (ContextMenuItemModel) null;
    object value = this.GetValue(type);
    if (value == null)
      return (ContextMenuItemModel) null;
    ContextMenuItemModel contextMenuItem1 = menu.OfType<ContextMenuItemModel>().FirstOrDefault<ContextMenuItemModel>((Func<ContextMenuItemModel, bool>) (c => !(c is ColorMoreItemContextMenuItemModel) && ToolbarContextMenuValueEqualityComparer.MenuValueEquals(this.Mode, type, c.TagData.MenuItemValue, value)));
    if (contextMenuItem1 != null)
      return contextMenuItem1;
    ContextMenuItemModel contextMenuItem2 = ToolbarContextMenuHelper.CreateContextMenuItem(this.Mode, type, value, true, action);
    for (int index = menu.Count - 1; index >= 0; --index)
    {
      if (!(menu[index] is ColorMoreItemContextMenuItemModel))
      {
        menu.Insert(index + 1, (IContextMenuModel) contextMenuItem2);
        break;
      }
    }
    return contextMenuItem2;
  }
}
