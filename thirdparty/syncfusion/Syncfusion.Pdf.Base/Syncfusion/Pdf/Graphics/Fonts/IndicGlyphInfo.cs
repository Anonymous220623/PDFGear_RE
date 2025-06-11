// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.IndicGlyphInfo
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class IndicGlyphInfo : OtfGlyphInfo
{
  private static int[] m_initialChars = new int[12]
  {
    2352,
    2480,
    2544,
    2608,
    2736,
    2864,
    2992,
    3120,
    3248,
    3376,
    3515,
    6042
  };
  private int m_group = -1;
  private int m_position = -1;
  private int m_mask;
  private bool m_substitute;
  private bool m_ligate;

  internal int Group
  {
    get => this.m_group;
    set => this.m_group = value;
  }

  internal int Position
  {
    get => this.m_position;
    set => this.m_position = value;
  }

  internal int Mask
  {
    get => this.m_mask;
    set => this.m_mask = value;
  }

  internal virtual bool Substitute
  {
    get => this.m_substitute;
    set => this.m_substitute = value;
  }

  internal virtual bool Ligate
  {
    get => this.m_ligate;
    set => this.m_ligate = value;
  }

  internal IndicGlyphInfo(OtfGlyphInfo glyph)
    : base(glyph)
  {
    this.Update();
  }

  internal IndicGlyphInfo(OtfGlyphInfo glyph, int category, int indicPos, int mask)
    : base(glyph)
  {
    this.Group = category;
    this.Position = indicPos;
    this.Mask = mask;
  }

  internal IndicGlyphInfo(
    OtfGlyphInfo glyph,
    int category,
    int position,
    int mask,
    bool substitute,
    bool ligate)
    : base(glyph)
  {
    this.m_group = category;
    this.m_position = position;
    this.m_mask = mask;
    this.m_substitute = substitute;
    this.m_ligate = ligate;
  }

  public override bool Equals(object obj)
  {
    if (!base.Equals(obj))
      return false;
    IndicGlyphInfo indicGlyphInfo = obj as IndicGlyphInfo;
    return this.Group == indicGlyphInfo.Group && this.Position == indicGlyphInfo.Position && this.Mask == indicGlyphInfo.Mask && this.Substitute == indicGlyphInfo.Substitute && this.Ligate == indicGlyphInfo.Ligate;
  }

  internal void Update()
  {
    if (this.CharCode <= -1)
      return;
    int charCode = this.CharCode;
    IndicCharacterClassifier characterClassifier = new IndicCharacterClassifier();
    int num1 = characterClassifier.GetClass(charCode);
    int num2 = num1 & (int) sbyte.MaxValue;
    int side = num1 >> 8;
    if (charCode >= 2385 && charCode <= 2386 || charCode >= 7376 && charCode <= 7378 || charCode >= 7380 && charCode <= 7393 || charCode == 7412)
      num2 = 10;
    else if (charCode >= 2387 && charCode <= 2388)
      num2 = 8;
    else if (charCode >= 2674 && charCode <= 2675 || charCode >= 7413 && charCode <= 7414)
      num2 = 1;
    else if (charCode >= 7394 && charCode <= 7400)
      num2 = 10;
    else if (charCode == 7405)
      num2 = 10;
    else if (charCode >= 43250 && charCode <= 43255 || charCode >= 7401 && charCode <= 7404 || charCode >= 7406 && charCode <= 7409)
      num2 = 18;
    else if (charCode >= 6093 && charCode <= 6097 || charCode == 6091 || charCode == 6099 || charCode == 6109)
    {
      num2 = 7;
      side = 6;
    }
    else
    {
      switch (charCode)
      {
        case 6086:
          num2 = 3;
          break;
        case 6098:
          num2 = 14;
          break;
        default:
          if (charCode >= 8208 && charCode <= 8209)
          {
            num2 = 11;
            break;
          }
          switch (charCode)
          {
            case 9676:
              num2 = 12;
              break;
            case 43394:
              num2 = 8;
              break;
            case 43453:
              num2 = 7;
              side = 11;
              break;
            case 43454:
              num2 = 31 /*0x1F*/;
              break;
          }
          break;
      }
    }
    if ((1L << num2 & 2147563526L) != 0L)
    {
      side = 4;
      for (int index = 0; index < IndicGlyphInfo.m_initialChars.Length; ++index)
      {
        if (IndicGlyphInfo.m_initialChars[index] == charCode)
        {
          num2 = 16 /*0x10*/;
          break;
        }
      }
    }
    else if (num2 == 7)
      side = characterClassifier.GetPosition(charCode, side);
    else if ((1L << num2 & 263936L /*0x040700*/) != 0L)
      side = 14;
    if (charCode == 2817)
      side = 7;
    this.Group = num2;
    this.Position = side;
  }
}
