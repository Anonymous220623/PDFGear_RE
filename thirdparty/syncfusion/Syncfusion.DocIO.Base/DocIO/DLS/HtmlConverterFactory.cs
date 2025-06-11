// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.HtmlConverterFactory
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class HtmlConverterFactory
{
  [ThreadStatic]
  private static IHtmlConverter s_htmlConverter;

  public static IHtmlConverter GetInstance()
  {
    if (HtmlConverterFactory.s_htmlConverter == null)
      HtmlConverterFactory.s_htmlConverter = (IHtmlConverter) new HTMLConverterImpl();
    return HtmlConverterFactory.s_htmlConverter;
  }

  public static void Register(IHtmlConverter converter)
  {
    HtmlConverterFactory.s_htmlConverter = converter != null ? converter : throw new ArgumentNullException("convertor");
  }
}
