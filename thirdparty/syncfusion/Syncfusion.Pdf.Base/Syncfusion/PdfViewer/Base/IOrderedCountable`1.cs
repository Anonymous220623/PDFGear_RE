// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.IOrderedCountable`1
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

public interface IOrderedCountable<TElement> : IEnumerable<TElement>, IEnumerable
{
  IOrderedCountable<TElement> CreateOrderedEnumerable<TKey>(
    Func<TElement, TKey> keySelector,
    IComparer<TKey> comparer,
    bool descending);
}
