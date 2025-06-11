// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Lists.PdfListItemCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;

#nullable disable
namespace Syncfusion.Pdf.Lists;

public class PdfListItemCollection : PdfCollection
{
  public PdfListItem this[int index]
  {
    get
    {
      return index >= 0 && index < this.Count ? (PdfListItem) this.List[index] : throw new IndexOutOfRangeException("The index should be less than item's count or more or equel to 0");
    }
  }

  public PdfListItemCollection()
  {
  }

  public PdfListItemCollection(string[] items)
    : this()
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    foreach (string text in items)
      this.Add(text);
  }

  public int Add(PdfListItem item)
  {
    if (item == null)
      throw new ArgumentNullException(nameof (item));
    this.List.Add((object) item);
    return this.List.Count - 1;
  }

  public int Add(PdfListItem item, float itemIndent)
  {
    item.TextIndent = itemIndent;
    return this.Add(item);
  }

  public PdfListItem Add(string text)
  {
    PdfListItem pdfListItem = text != null ? new PdfListItem(text) : throw new ArgumentNullException(nameof (text));
    this.List.Add((object) pdfListItem);
    return pdfListItem;
  }

  public PdfListItem Add(string text, float itemIndent)
  {
    PdfListItem pdfListItem = this.Add(text);
    pdfListItem.TextIndent = itemIndent;
    return pdfListItem;
  }

  public PdfListItem Add(string text, PdfFont font)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    PdfListItem pdfListItem = font != null ? new PdfListItem(text, font) : throw new ArgumentNullException(nameof (font));
    this.List.Add((object) pdfListItem);
    return pdfListItem;
  }

  public PdfListItem Add(string text, PdfFont font, float itemIndent)
  {
    PdfListItem pdfListItem = this.Add(text, font);
    pdfListItem.TextIndent = itemIndent;
    return pdfListItem;
  }

  public void Insert(int index, PdfListItem item)
  {
    if (index < 0 || index >= this.Count)
      throw new ArgumentException("The index should be less than item's count or more or equal to 0", nameof (index));
    if (item == null)
      throw new ArgumentNullException(nameof (item));
    this.List.Insert(index, (object) item);
  }

  public void Insert(int index, PdfListItem item, float itemIndent)
  {
    item.TextIndent = itemIndent;
    this.List.Insert(index, (object) item);
  }

  public void Remove(PdfListItem item)
  {
    if (item == null)
      throw new ArgumentNullException(nameof (item));
    if (!this.List.Contains((object) item))
      throw new ArgumentException("The list doesn't contain this item", nameof (item));
    this.List.Remove((object) item);
  }

  public void RemoveAt(int index)
  {
    if (index < 0 || index >= this.Count)
      throw new ArgumentException("The index should be less than item's count or more or equal to 0", nameof (index));
    this.List.RemoveAt(index);
  }

  public int IndexOf(PdfListItem item)
  {
    return item != null ? this.List.IndexOf((object) item) : throw new ArgumentNullException(nameof (item));
  }

  public void Clear() => this.List.Clear();
}
