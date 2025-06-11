// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontEmptyEnumerable`1
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontEmptyEnumerable<TElement>
{
  private static TElement[] instance;

  public static IEnumerable<TElement> Instance
  {
    get
    {
      if (SystemFontEmptyEnumerable<TElement>.instance == null)
        SystemFontEmptyEnumerable<TElement>.instance = new TElement[0];
      return (IEnumerable<TElement>) SystemFontEmptyEnumerable<TElement>.instance;
    }
  }
}
