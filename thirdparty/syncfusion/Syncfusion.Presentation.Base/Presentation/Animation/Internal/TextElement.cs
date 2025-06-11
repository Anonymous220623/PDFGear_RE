// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.Internal.TextElement
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.Animation.Internal;

internal class TextElement
{
  private RangeValues characterRange;
  private RangeValues paragraphRange;

  internal RangeValues CharacterRange
  {
    get => this.characterRange;
    set => this.characterRange = value;
  }

  internal RangeValues ParagraphRange
  {
    get => this.paragraphRange;
    set => this.paragraphRange = value;
  }

  internal TextElement Clone()
  {
    TextElement textElement = (TextElement) this.MemberwiseClone();
    if (this.characterRange != null)
      textElement.characterRange = this.characterRange.Clone();
    if (this.paragraphRange != null)
      textElement.paragraphRange = this.paragraphRange.Clone();
    return textElement;
  }
}
