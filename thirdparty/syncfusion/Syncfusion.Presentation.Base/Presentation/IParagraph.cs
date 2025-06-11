// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.IParagraph
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation;

public interface IParagraph
{
  IListFormat ListFormat { get; }

  string Text { get; set; }

  HorizontalAlignmentType HorizontalAlignment { get; set; }

  ITextParts TextParts { get; }

  IFont Font { get; }

  double FirstLineIndent { get; set; }

  double LeftIndent { get; set; }

  int IndentLevelNumber { get; set; }

  double LineSpacing { get; set; }

  double SpaceAfter { get; set; }

  double SpaceBefore { get; set; }

  ITextPart AddTextPart();

  ITextPart AddTextPart(string text);

  IParagraph Clone();

  IHyperLink AddHyperlink(string textToDisplay, string link);
}
