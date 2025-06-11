// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.ITextBody
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation;

public interface ITextBody
{
  double MarginBottom { get; set; }

  double MarginLeft { get; set; }

  double MarginRight { get; set; }

  double MarginTop { get; set; }

  IParagraphs Paragraphs { get; }

  string Text { get; set; }

  bool WrapText { get; set; }

  VerticalAlignmentType VerticalAlignment { get; set; }

  bool AnchorCenter { get; set; }

  TextDirectionType TextDirection { get; set; }

  IParagraph AddParagraph();

  IParagraph AddParagraph(string text);

  FitTextOption FitTextOption { get; set; }
}
