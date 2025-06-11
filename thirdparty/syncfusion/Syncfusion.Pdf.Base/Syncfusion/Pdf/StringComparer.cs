// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.StringComparer
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf;

internal class StringComparer : IComparer<string>, IComparer
{
  public int Compare(object x, object y)
  {
    string strA = x as string;
    string strB = y as string;
    return strA != null && strB != null ? string.CompareOrdinal(strA, strB) : 0;
  }

  public int Compare(string x, string y)
  {
    return x != null && y != null ? string.CompareOrdinal(x, y) : 0;
  }
}
