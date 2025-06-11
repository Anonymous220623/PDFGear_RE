// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.IFont
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation;

public interface IFont
{
  TextCapsType CapsType { get; set; }

  IColor Color { get; set; }

  bool Bold { get; set; }

  bool Italic { get; set; }

  bool Subscript { get; set; }

  bool Superscript { get; set; }

  string FontName { get; set; }

  float FontSize { get; set; }

  TextStrikethroughType StrikeType { get; set; }

  TextUnderlineType Underline { get; set; }

  short LanguageID { get; set; }
}
