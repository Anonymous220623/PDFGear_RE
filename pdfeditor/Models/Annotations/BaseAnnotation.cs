// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Annotations.BaseAnnotation
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Wrappers;
using pdfeditor.Models.Menus;
using pdfeditor.ViewModels;
using PDFKit.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace pdfeditor.Models.Annotations;

public abstract class BaseAnnotation : IEquatable<BaseAnnotation>
{
  public string AnnotationType { get; protected set; }

  public int PageIndex { get; protected set; }

  public int AnnotIndex { get; internal set; }

  public AnnotationFlags Flags { get; protected set; }

  public FS_COLOR Color { get; protected set; }

  public string Contents { get; protected set; }

  public string Name { get; protected set; }

  public string ModificationDate { get; protected set; }

  public FS_RECTF Rectangle { get; internal set; }

  protected virtual void Init(PdfAnnotation pdfAnnotation)
  {
    PdfTypeBase pdfTypeBase;
    this.AnnotationType = !pdfAnnotation.Dictionary.TryGetValue("Subtype", out pdfTypeBase) || !(pdfTypeBase is PdfTypeName pdfTypeName) ? pdfAnnotation.GetType().FullName : pdfTypeName.Value;
    this.PageIndex = pdfAnnotation.Page.PageIndex;
    this.AnnotIndex = pdfAnnotation.Page.Annots.IndexOf(pdfAnnotation);
    this.Flags = BaseAnnotation.ReturnValueOrDefault<AnnotationFlags>((Func<AnnotationFlags>) (() => pdfAnnotation.Flags));
    this.Color = BaseAnnotation.ReturnValueOrDefault<FS_COLOR>((Func<FS_COLOR>) (() => pdfAnnotation.Color));
    this.Contents = BaseAnnotation.ReturnValueOrDefault<string>((Func<string>) (() => pdfAnnotation.Contents));
    this.Name = BaseAnnotation.ReturnValueOrDefault<string>((Func<string>) (() => pdfAnnotation.Name));
    this.ModificationDate = BaseAnnotation.ReturnValueOrDefault<string>((Func<string>) (() => pdfAnnotation.ModificationDate));
    this.Rectangle = BaseAnnotation.ReturnValueOrDefault<FS_RECTF>((Func<FS_RECTF>) (() => pdfAnnotation.GetRECT()));
  }

  public void Apply(PdfAnnotation annot)
  {
    if (annot == null)
      throw new ArgumentNullException(nameof (annot));
    annot.Flags = this.Flags;
    annot.Color = this.Color;
    annot.Contents = this.Contents;
    annot.Name = this.Name;
    annot.ModificationDate = this.ModificationDate;
    this.ApplyCore(annot);
  }

  protected virtual void ApplyCore(PdfAnnotation annot)
  {
  }

  public virtual object GetValue(AnnotationMode mode, ContextMenuItemType type) => (object) null;

  public static void InitModelProperties(PdfAnnotation annot, BaseAnnotation model)
  {
    if ((PdfWrapper) annot == (PdfWrapper) null || model == null)
      return;
    model.Init(annot);
  }

  public bool Equals(BaseAnnotation other)
  {
    if (other == null)
      return false;
    BaseAnnotation other1 = other;
    return this.Flags == other1.Flags && this.Color == other1.Color && this.Contents == other1.Contents && this.Name == other1.Name && this.ModificationDate == other1.ModificationDate && this.EqualsCore(other1);
  }

  protected virtual bool EqualsCore(BaseAnnotation other) => true;

  public static bool CollectionEqual<T>(IReadOnlyList<T> first, IReadOnlyList<T> second)
  {
    if (first == second)
      return true;
    bool flag1 = first == null || first.Count == 0;
    bool flag2 = second == null || second.Count == 0;
    if (flag1 & flag2)
      return true;
    return !(flag1 | flag2) && first.Count == second.Count && first.SequenceEqual<T>((IEnumerable<T>) second);
  }

  public static bool CollectionEqual<TCollection, T>(
    IReadOnlyList<TCollection> first,
    IReadOnlyList<TCollection> second)
    where TCollection : IReadOnlyList<T>
  {
    if (first == second)
      return true;
    bool flag1 = first == null || first.Count == 0;
    bool flag2 = second == null || second.Count == 0;
    if (flag1 & flag2)
      return true;
    if (flag1 | flag2 || first.Count != second.Count)
      return false;
    bool flag3 = true;
    for (int index = 0; index < first.Count; ++index)
    {
      flag3 &= BaseAnnotation.CollectionEqual<T>((IReadOnlyList<T>) first[index], (IReadOnlyList<T>) second[index]);
      if (!flag3)
        break;
    }
    return flag3;
  }

  protected static T[] ReturnArrayOrEmpty<T>(Func<T[]> action)
  {
    try
    {
      return (action != null ? action() : (T[]) null) ?? Array.Empty<T>();
    }
    catch
    {
    }
    return Array.Empty<T>();
  }

  protected static T ReturnValueOrDefault<T>(Func<T> action)
  {
    try
    {
      if (action != null)
      {
        T objA = action();
        if (!typeof (T).IsValueType)
        {
          if (object.Equals((object) objA, (object) null))
            goto label_5;
        }
        return objA;
      }
    }
    catch
    {
    }
label_5:
    return default (T);
  }
}
