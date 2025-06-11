// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.IHtmlConverter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

public interface IHtmlConverter
{
  void AppendToTextBody(
    ITextBody dlsTextBody,
    string html,
    int paragraphIndex,
    int paragraphItemIndex);

  void AppendToTextBody(
    ITextBody dlsTextBody,
    string html,
    int paragraphIndex,
    int paragraphItemIndex,
    IWParagraphStyle style,
    ListStyle listStyle);

  bool IsValid(string html, XHTMLValidationType type);

  bool IsValid(string html, XHTMLValidationType type, out string exceptionMessage);
}
