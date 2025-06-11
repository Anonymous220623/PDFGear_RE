// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.PageContents.PageBaseObject
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.BasicTypes;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace pdfeditor.Models.PageContents;

public abstract class PageBaseObject : IEquatable<PageBaseObject>
{
  public abstract PageObjectTypes ObjectType { get; }

  public List<MarkedContentModel> MarkedContent { get; protected set; }

  public FS_COLOR FillColor { get; protected set; }

  public FS_COLOR StrokeColor { get; protected set; }

  public float Flatness { get; protected set; }

  public float Smoothness { get; protected set; }

  public BlendTypes BlendMode { get; protected set; }

  public RenderIntent RenderIntent { get; protected set; }

  public OverprintModes OverprintMode { get; protected set; }

  public bool StrokeOverprint { get; protected set; }

  public bool FillOverprint { get; protected set; }

  public bool AlphaShape { get; protected set; }

  public FS_MATRIX Matrix { get; protected set; }

  protected virtual void Init(PdfPageObject pageObject)
  {
    if (pageObject.ObjectType != this.ObjectType)
      throw new ArgumentException(nameof (pageObject));
    this.FillColor = PageBaseObject.ReturnValueOrDefault<FS_COLOR>((Func<FS_COLOR>) (() => pageObject.FillColor));
    this.StrokeColor = PageBaseObject.ReturnValueOrDefault<FS_COLOR>((Func<FS_COLOR>) (() => pageObject.StrokeColor));
    this.Flatness = PageBaseObject.ReturnValueOrDefault<float>((Func<float>) (() => pageObject.Flatness));
    this.Smoothness = PageBaseObject.ReturnValueOrDefault<float>((Func<float>) (() => pageObject.Smoothness));
    this.BlendMode = PageBaseObject.ReturnValueOrDefault<BlendTypes>((Func<BlendTypes>) (() => pageObject.BlendMode));
    this.RenderIntent = PageBaseObject.ReturnValueOrDefault<RenderIntent>((Func<RenderIntent>) (() => pageObject.RenderIntent));
    this.OverprintMode = PageBaseObject.ReturnValueOrDefault<OverprintModes>((Func<OverprintModes>) (() => pageObject.OverprintMode));
    this.StrokeOverprint = PageBaseObject.ReturnValueOrDefault<bool>((Func<bool>) (() => pageObject.StrokeOverprint));
    this.FillOverprint = PageBaseObject.ReturnValueOrDefault<bool>((Func<bool>) (() => pageObject.FillOverprint));
    this.AlphaShape = PageBaseObject.ReturnValueOrDefault<bool>((Func<bool>) (() => pageObject.AlphaShape));
    this.Matrix = PageBaseObject.ReturnValueOrDefault<FS_MATRIX>((Func<FS_MATRIX>) (() => new FS_MATRIX((PdfTypeBase) pageObject.Matrix.ToArray())));
    this.MarkedContent = PageBaseObject.ReturnValueOrDefault<List<MarkedContentModel>>((Func<List<MarkedContentModel>>) (() =>
    {
      PdfMarkedContentCollection markedContent = pageObject.MarkedContent;
      return (markedContent != null ? markedContent.Select<PdfMarkedContent, MarkedContentModel>((Func<PdfMarkedContent, MarkedContentModel>) (c => MarkedContentModel.Create(c))).ToList<MarkedContentModel>() : (List<MarkedContentModel>) null) ?? new List<MarkedContentModel>();
    }));
  }

  public void Apply(PdfPageObject pageObject)
  {
    PageObjectTypes? objectType1 = pageObject?.ObjectType;
    PageObjectTypes objectType2 = this.ObjectType;
    if (!(objectType1.GetValueOrDefault() == objectType2 & objectType1.HasValue))
      throw new ArgumentException(nameof (pageObject));
    pageObject.FillColor = this.FillColor;
    pageObject.StrokeColor = this.StrokeColor;
    pageObject.Flatness = this.Flatness;
    pageObject.Smoothness = this.Smoothness;
    pageObject.BlendMode = this.BlendMode;
    pageObject.RenderIntent = this.RenderIntent;
    pageObject.OverprintMode = this.OverprintMode;
    pageObject.StrokeOverprint = this.StrokeOverprint;
    pageObject.FillOverprint = this.FillOverprint;
    pageObject.AlphaShape = this.AlphaShape;
    pageObject.Matrix = this.Matrix;
    if (pageObject.MarkedContent != null)
      pageObject.MarkedContent.Clear();
    if (this.MarkedContent != null && this.MarkedContent.Count > 0)
    {
      foreach (MarkedContentModel markedContentModel in this.MarkedContent)
        pageObject.MarkedContent.Add(markedContentModel.ToMarkedContent());
    }
    this.ApplyCore(pageObject);
  }

  protected virtual void ApplyCore(PdfPageObject pageObject)
  {
  }

  public static void InitModelProperties(PdfPageObject pageObject, PageBaseObject model)
  {
    if (pageObject == null || model == null)
      return;
    model.Init(pageObject);
  }

  public bool Equals(PageBaseObject other)
  {
    if (other != null)
    {
      PageBaseObject other1 = other;
      if (this.FillColor == other1.FillColor && this.StrokeColor == other1.StrokeColor && (double) this.Flatness == (double) other1.Flatness && (double) this.Smoothness == (double) other1.Smoothness && this.BlendMode == other1.BlendMode && this.RenderIntent == other1.RenderIntent && this.OverprintMode == other1.OverprintMode && this.StrokeOverprint == other1.StrokeOverprint && this.FillOverprint == other1.FillOverprint && this.AlphaShape == other1.AlphaShape && PageBaseObject.FsMatrixEquals(this.Matrix, other1.Matrix) && PageBaseObject.CollectionEqual<MarkedContentModel>((System.Collections.Generic.IReadOnlyList<MarkedContentModel>) this.MarkedContent, (System.Collections.Generic.IReadOnlyList<MarkedContentModel>) other1.MarkedContent))
        return this.EqualsCore(other1);
    }
    return false;
  }

  protected virtual bool EqualsCore(PageBaseObject other) => true;

  public static bool CollectionEqual<T>(System.Collections.Generic.IReadOnlyList<T> first, System.Collections.Generic.IReadOnlyList<T> second)
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
    System.Collections.Generic.IReadOnlyList<TCollection> first,
    System.Collections.Generic.IReadOnlyList<TCollection> second)
    where TCollection : System.Collections.Generic.IReadOnlyList<T>
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
      flag3 &= PageBaseObject.CollectionEqual<T>((System.Collections.Generic.IReadOnlyList<T>) first[index], (System.Collections.Generic.IReadOnlyList<T>) second[index]);
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

  private static bool FsMatrixEquals(FS_MATRIX matrix1, FS_MATRIX matrix2)
  {
    if (matrix1 == null && matrix2 == null)
      return true;
    return matrix1 != null && matrix2 != null && (double) matrix1.a == (double) matrix2.a && (double) matrix1.b == (double) matrix2.b && (double) matrix1.c == (double) matrix2.c && (double) matrix1.d == (double) matrix2.d && (double) matrix1.e == (double) matrix2.e && (double) matrix1.f == (double) matrix2.f;
  }
}
