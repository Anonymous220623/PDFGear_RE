// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.WidgetAnnotationCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

internal class WidgetAnnotationCollection : PdfCollection, IPdfWrapper
{
  private PdfArray m_array = new PdfArray();

  public WidgetAnnotation this[int index] => (WidgetAnnotation) this.List[index];

  public int Add(WidgetAnnotation annotation)
  {
    return annotation != null ? this.DoAdd(annotation) : throw new ArgumentNullException(nameof (annotation));
  }

  public void Insert(int index, WidgetAnnotation annotation)
  {
    if (annotation == null)
      throw new ArgumentNullException(nameof (annotation));
    this.DoInsert(index, annotation);
  }

  public void Remove(WidgetAnnotation annotation)
  {
    if (annotation == null)
      throw new ArgumentNullException(nameof (annotation));
    this.DoRemove(annotation);
  }

  public void RemoveAt(int index) => this.DoRemoveAt(index);

  public int IndexOf(WidgetAnnotation annotation)
  {
    return annotation != null ? this.List.IndexOf((object) annotation) : throw new ArgumentNullException(nameof (annotation));
  }

  public bool Contains(WidgetAnnotation annotation)
  {
    return annotation != null ? this.List.Contains((object) annotation) : throw new ArgumentNullException(nameof (annotation));
  }

  public void Clear() => this.DoClear();

  private int DoAdd(WidgetAnnotation annotation)
  {
    this.m_array.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) annotation));
    this.List.Add((object) annotation);
    return this.List.Count - 1;
  }

  private void DoInsert(int index, WidgetAnnotation annotation)
  {
    this.m_array.Insert(index, (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) annotation));
    this.List.Insert(index, (object) annotation);
  }

  private void DoRemove(WidgetAnnotation annotation)
  {
    int index = this.List.IndexOf((object) annotation);
    this.m_array.RemoveAt(index);
    this.List.RemoveAt(index);
  }

  private void DoRemoveAt(int index)
  {
    this.m_array.RemoveAt(index);
    this.List.RemoveAt(index);
  }

  private new void DoClear()
  {
    this.m_array.Clear();
    this.List.Clear();
  }

  public IPdfPrimitive Element => (IPdfPrimitive) this.m_array;
}
