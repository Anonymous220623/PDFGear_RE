// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfListFieldItemCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfListFieldItemCollection : PdfCollection, IPdfWrapper
{
  private PdfArray m_items = new PdfArray();

  public PdfListFieldItem this[int index] => (PdfListFieldItem) this.List[index];

  public int Add(PdfListFieldItem item)
  {
    return item != null ? this.DoAdd(item) : throw new ArgumentNullException(nameof (item));
  }

  public void Insert(int index, PdfListFieldItem item)
  {
    if (item == null)
      throw new ArgumentNullException(nameof (item));
    this.DoInsert(index, item);
  }

  public void Remove(PdfListFieldItem item)
  {
    if (item == null)
      throw new ArgumentNullException(nameof (item));
    if (!this.List.Contains((object) item))
      return;
    this.DoRemove(item);
  }

  public void RemoveAt(int index)
  {
    if (index < 0 || index >= this.List.Count)
      throw new ArgumentNullException(nameof (index));
    this.DoRemoveAt(index);
  }

  public bool Contains(PdfListFieldItem item) => this.List.Contains((object) item);

  public int IndexOf(PdfListFieldItem item) => this.List.IndexOf((object) item);

  public void Clear()
  {
    this.m_items.Clear();
    this.List.Clear();
  }

  private int DoAdd(PdfListFieldItem item)
  {
    this.m_items.Add(((IPdfWrapper) item).Element);
    this.List.Add((object) item);
    return this.List.Count - 1;
  }

  private void DoInsert(int index, PdfListFieldItem item)
  {
    this.m_items.Insert(index, ((IPdfWrapper) item).Element);
    this.List.Insert(index, (object) item);
  }

  private void DoRemoveAt(int index)
  {
    this.m_items.RemoveAt(index);
    this.List.RemoveAt(index);
  }

  private void DoRemove(PdfListFieldItem item)
  {
    int index = this.List.IndexOf((object) item);
    this.m_items.RemoveAt(index);
    this.List.RemoveAt(index);
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_items;
}
