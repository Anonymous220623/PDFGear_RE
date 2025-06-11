// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontBuffer`1
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal struct SystemFontBuffer<TElement>
{
  internal TElement[] items;
  internal int count;

  internal SystemFontBuffer(IEnumerable<TElement> source)
  {
    TElement[] elementArray = (TElement[]) null;
    int length = 0;
    if (source is ICollection<TElement> elements)
    {
      length = elements.Count;
      if (length > 0)
      {
        elementArray = new TElement[length];
        elements.CopyTo(elementArray, 0);
      }
    }
    else
    {
      foreach (TElement element in source)
      {
        if (elementArray == null)
          elementArray = new TElement[4];
        else if (elementArray.Length == length)
        {
          TElement[] destinationArray = new TElement[length * 2];
          System.Array.Copy((System.Array) elementArray, 0, (System.Array) destinationArray, 0, length);
          elementArray = destinationArray;
        }
        elementArray[length] = element;
        ++length;
      }
    }
    this.items = elementArray;
    this.count = length;
  }

  internal TElement[] ToArray()
  {
    if (this.count == 0)
      return new TElement[0];
    if (this.items.Length == this.count)
      return this.items;
    TElement[] destinationArray = new TElement[this.count];
    System.Array.Copy((System.Array) this.items, 0, (System.Array) destinationArray, 0, this.count);
    return destinationArray;
  }
}
