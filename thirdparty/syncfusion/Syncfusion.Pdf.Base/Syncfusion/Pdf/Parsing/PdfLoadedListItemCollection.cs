// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfLoadedListItemCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class PdfLoadedListItemCollection : PdfCollection
{
  private PdfLoadedChoiceField m_field;

  public PdfLoadedListItem this[int index]
  {
    get
    {
      if (index < 0 || index >= this.List.Count)
        throw new IndexOutOfRangeException("Index");
      return this.List[index] as PdfLoadedListItem;
    }
  }

  internal PdfLoadedListItemCollection(PdfLoadedChoiceField field) => this.m_field = field;

  public int Add(PdfLoadedListItem item)
  {
    if (item == null)
      throw new ArgumentNullException(nameof (item));
    PdfArray items = this.GetItems();
    PdfArray array = this.GetArray(item);
    items.Add((IPdfPrimitive) array);
    this.m_field.Dictionary.SetProperty("Opt", (IPdfPrimitive) items);
    this.List.Add((object) item);
    return this.List.Count - 1;
  }

  internal int AddItem(PdfLoadedListItem item)
  {
    if (item == null)
      throw new ArgumentNullException(nameof (item));
    this.List.Add((object) item);
    return this.List.Count - 1;
  }

  public void Insert(int index, PdfLoadedListItem item)
  {
    if (index < 0 || index > this.List.Count)
      throw new IndexOutOfRangeException(nameof (index));
    if (item == null)
      throw new ArgumentNullException(nameof (item));
    PdfArray items = this.GetItems();
    PdfArray array = this.GetArray(item);
    items.Insert(index, (IPdfPrimitive) array);
    this.m_field.Dictionary.SetProperty("Opt", (IPdfPrimitive) items);
    this.List.Insert(index, (object) item);
  }

  public void RemoveAt(int index)
  {
    if (index < 0 || index > this.List.Count)
      throw new IndexOutOfRangeException(nameof (index));
    PdfArray items = this.GetItems();
    items.RemoveAt(index);
    this.m_field.Dictionary.SetProperty("Opt", (IPdfPrimitive) items);
    this.List.RemoveAt(index);
  }

  public void Clear()
  {
    PdfArray items = this.GetItems();
    items.Clear();
    this.m_field.Dictionary.SetProperty("Opt", (IPdfPrimitive) items);
    this.List.Clear();
  }

  private PdfArray GetItems()
  {
    PdfArray items = new PdfArray();
    if (this.m_field.Dictionary.ContainsKey("Opt"))
      items = this.m_field.CrossTable.GetObject(this.m_field.Dictionary["Opt"]) as PdfArray;
    return items;
  }

  private PdfArray GetArray(PdfLoadedListItem item)
  {
    PdfArray array = new PdfArray();
    if (item.Value != string.Empty)
      array.Add((IPdfPrimitive) new PdfString(item.Value));
    if (item.Text != string.Empty)
      array.Add((IPdfPrimitive) new PdfString(item.Text));
    return array;
  }
}
