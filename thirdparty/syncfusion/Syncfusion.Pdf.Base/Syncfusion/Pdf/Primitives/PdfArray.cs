// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Primitives.PdfArray
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Primitives;

internal class PdfArray : IPdfPrimitive, IEnumerable, IPdfChangable
{
  public const string StartMark = "[";
  public const string EndMark = "]";
  private List<IPdfPrimitive> m_elements;
  private bool m_bChanged;
  private ObjectStatus m_status;
  private bool m_isSaving;
  private int m_index;
  private int m_position = -1;
  private PdfCrossTable m_crossTable;
  private PdfArray m_clonedObject;
  private bool isFont;

  internal IPdfPrimitive this[int index]
  {
    get
    {
      return index >= 0 && index < this.Count ? this.m_elements[index] : throw new ArgumentOutOfRangeException(nameof (index), "The index can't be less then zero or greater then Count.");
    }
  }

  internal int Count => this.m_elements.Count;

  public ObjectStatus Status
  {
    get => this.m_status;
    set => this.m_status = value;
  }

  public bool IsSaving
  {
    get => this.m_isSaving;
    set => this.m_isSaving = value;
  }

  public int ObjectCollectionIndex
  {
    get => this.m_index;
    set => this.m_index = value;
  }

  public int Position
  {
    get => this.m_position;
    set => this.m_position = value;
  }

  internal List<IPdfPrimitive> Elements => this.m_elements;

  internal PdfCrossTable CrossTable => this.m_crossTable;

  public IPdfPrimitive ClonedObject => (IPdfPrimitive) this.m_clonedObject;

  internal bool IsFont
  {
    get => this.isFont;
    set => this.isFont = value;
  }

  internal PdfArray() => this.m_elements = new List<IPdfPrimitive>();

  internal PdfArray(PdfArray array)
  {
    this.m_elements = new List<IPdfPrimitive>((IEnumerable<IPdfPrimitive>) array.m_elements);
  }

  internal PdfArray(List<PdfArray> array)
    : this()
  {
    foreach (PdfArray array1 in array)
      this.Add((IPdfPrimitive) new PdfArray(array1));
  }

  internal PdfArray(int[] array)
    : this()
  {
    foreach (int num in array)
      this.Add((IPdfPrimitive) new PdfNumber(num));
  }

  internal PdfArray(float[] array)
    : this()
  {
    foreach (float num in array)
      this.Add((IPdfPrimitive) new PdfNumber(num));
  }

  public PdfArray(double[] array)
    : this()
  {
    foreach (double num in array)
      this.Add((IPdfPrimitive) new PdfNumber(num));
  }

  public static PdfArray FromRectangle(RectangleF rectangle)
  {
    return new PdfArray(new float[4]
    {
      rectangle.Left,
      rectangle.Top,
      rectangle.Right,
      rectangle.Bottom
    });
  }

  public static PdfArray FromRectangle(Rectangle rectangle)
  {
    return new PdfArray(new int[4]
    {
      rectangle.Left,
      rectangle.Top,
      rectangle.Right,
      rectangle.Bottom
    });
  }

  internal void Add(IPdfPrimitive element)
  {
    if (element == null)
      throw new ArgumentNullException("obj");
    this.m_elements.Add(element);
    this.MarkChanged();
  }

  internal void Add(params IPdfPrimitive[] list)
  {
    foreach (IPdfPrimitive pdfPrimitive in list)
    {
      if (pdfPrimitive == null)
        throw new ArgumentNullException(nameof (list));
      this.m_elements.Add(pdfPrimitive);
    }
    if (list.Length <= 0)
      return;
    this.MarkChanged();
  }

  internal bool Contains(IPdfPrimitive element) => this.m_elements.Contains(element);

  internal void Insert(int index, IPdfPrimitive element)
  {
    this.m_elements.Insert(index, element);
    this.MarkChanged();
  }

  internal int IndexOf(IPdfPrimitive element) => this.m_elements.IndexOf(element);

  internal void Remove(IPdfPrimitive element)
  {
    if (element == null)
      throw new ArgumentNullException(nameof (element));
    this.m_elements.Remove(element);
    this.MarkChanged();
  }

  internal void ReArrange(int[] orderArray)
  {
    int length = orderArray.Length;
    PdfReferenceHolder[] pdfReferenceHolderArray = new PdfReferenceHolder[this.Count];
    int[] numArray = new int[this.Count];
    for (int index = 0; index < this.Count; ++index)
      pdfReferenceHolderArray[index] = this.m_elements[index] as PdfReferenceHolder;
    if (length <= this.Count)
    {
      if (PdfLoadedPageCollection.m_repeatIndex != 0)
      {
        for (int index = 0; index < PdfLoadedPageCollection.m_repeatIndex; ++index)
        {
          this.m_elements[index] = (IPdfPrimitive) pdfReferenceHolderArray[orderArray[index]];
          numArray[orderArray[index]] = 1;
        }
      }
      else
      {
        for (int index = 0; index < length; ++index)
        {
          this.m_elements[index] = (IPdfPrimitive) pdfReferenceHolderArray[orderArray[index]];
          numArray[orderArray[index]] = 1;
        }
      }
    }
    if (length > this.Count)
    {
      for (int index = 0; index < this.Count; ++index)
      {
        this.m_elements[index] = (IPdfPrimitive) pdfReferenceHolderArray[orderArray[index]];
        numArray[orderArray[index]] = 1;
      }
    }
    if (this.Count != length)
    {
      int num = PdfLoadedPageCollection.m_nestedPages != 1 ? PdfLoadedPageCollection.m_parentKidsCount : PdfLoadedPageCollection.m_parentKidsCounttemp;
      for (int index = 0; index < num; ++index)
      {
        if (numArray[index] == 0)
        {
          if (PdfLoadedPageCollection.m_repeatIndex != 0)
            this.RemoveAt(PdfLoadedPageCollection.m_repeatIndex);
          else
            this.RemoveAt(length);
        }
      }
    }
    this.MarkChanged();
  }

  internal void RemoveAt(int index)
  {
    this.m_elements.RemoveAt(index);
    this.MarkChanged();
  }

  internal void Clear()
  {
    this.m_elements.Clear();
    this.MarkChanged();
  }

  public RectangleF ToRectangle()
  {
    if (this.Count < 4)
      throw new InvalidOperationException("Can't convert to rectangle.");
    float floatValue1 = this.GetNumber(0).FloatValue;
    float floatValue2 = this.GetNumber(1).FloatValue;
    float floatValue3 = this.GetNumber(2).FloatValue;
    float floatValue4 = this.GetNumber(3).FloatValue;
    return new RectangleF(Math.Min(floatValue1, floatValue3), Math.Min(floatValue2, floatValue4), Math.Abs(floatValue1 - floatValue3), Math.Abs(floatValue2 - floatValue4));
  }

  public virtual void Save(IPdfWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.Write("[");
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      this[index].Save(writer);
      if (index + 1 != count)
        writer.Write(" ");
    }
    writer.Write("]");
  }

  public IEnumerator GetEnumerator() => (IEnumerator) this.m_elements.GetEnumerator();

  public void MarkChanged() => this.m_bChanged = true;

  public bool Changed => this.m_bChanged;

  public void FreezeChanges(object freezer)
  {
    if (!(freezer is PdfParser) && freezer != this)
      return;
    this.m_bChanged = false;
  }

  private PdfNumber GetNumber(int index)
  {
    return PdfCrossTable.Dereference(this[index]) is PdfNumber pdfNumber ? pdfNumber : throw new InvalidOperationException("Can't convert to rectangle.");
  }

  public IPdfPrimitive Clone(PdfCrossTable crossTable)
  {
    if (this.m_clonedObject != null && this.m_clonedObject.CrossTable == crossTable && !this.IsFont)
      return (IPdfPrimitive) this.m_clonedObject;
    this.m_clonedObject = (PdfArray) null;
    PdfArray pdfArray = new PdfArray();
    foreach (IPdfPrimitive element in this.m_elements)
      pdfArray.Add(element.Clone(crossTable));
    pdfArray.m_crossTable = crossTable;
    this.m_clonedObject = pdfArray;
    return (IPdfPrimitive) pdfArray;
  }
}
