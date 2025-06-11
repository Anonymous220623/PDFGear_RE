// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfStringFormat
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public sealed class PdfStringFormat : ICloneable
{
  private PdfTextAlignment m_alignment;
  private PdfVerticalAlignment m_lineAlignment;
  private bool m_rightToLeft;
  private PdfTextDirection m_textDirection;
  private float m_characterSpacing;
  private float m_wordSpacing;
  private float m_leading;
  private bool m_clip;
  private PdfSubSuperScript m_subSuperScript;
  private float m_scalingFactor = 100f;
  private float m_firstLineIndent;
  private float m_paragraphIndent;
  private bool m_lineLimit;
  private bool m_measureTrailingSpaces;
  private bool m_noClip;
  private PdfWordWrapType m_wrapType;
  internal bool isCustomRendering;
  private bool m_complexScript;
  private bool m_baseLine;
  internal bool m_isList;

  public PdfStringFormat()
  {
    this.m_lineLimit = true;
    this.m_wrapType = PdfWordWrapType.Word;
  }

  public PdfStringFormat(PdfTextAlignment alignment)
    : this()
  {
    this.m_alignment = alignment;
  }

  public PdfStringFormat(string columnFormat)
    : this()
  {
  }

  public PdfStringFormat(PdfTextAlignment alignment, PdfVerticalAlignment lineAlignment)
    : this(alignment)
  {
    this.m_lineAlignment = lineAlignment;
  }

  public PdfTextDirection TextDirection
  {
    get => this.m_textDirection;
    set => this.m_textDirection = value;
  }

  public bool ComplexScript
  {
    get => this.m_complexScript;
    set => this.m_complexScript = value;
  }

  public PdfTextAlignment Alignment
  {
    get => this.m_alignment;
    set => this.m_alignment = value;
  }

  public PdfVerticalAlignment LineAlignment
  {
    get => this.m_lineAlignment;
    set => this.m_lineAlignment = value;
  }

  public bool EnableBaseline
  {
    get => this.m_baseLine;
    set => this.m_baseLine = value;
  }

  [Obsolete("Please use PdfStringFormat.TextDirection (eg:TextDirection = PdfTextDirection.RightToLeft) instead")]
  public bool RightToLeft
  {
    get => this.m_rightToLeft;
    set => this.m_rightToLeft = value;
  }

  public float CharacterSpacing
  {
    get => this.m_characterSpacing;
    set => this.m_characterSpacing = value;
  }

  public float WordSpacing
  {
    get => this.m_wordSpacing;
    set => this.m_wordSpacing = value;
  }

  public float LineSpacing
  {
    get => this.m_leading;
    set => this.m_leading = value;
  }

  public bool ClipPath
  {
    get => this.m_clip;
    set => this.m_clip = value;
  }

  public PdfSubSuperScript SubSuperScript
  {
    get => this.m_subSuperScript;
    set => this.m_subSuperScript = value;
  }

  public float ParagraphIndent
  {
    get => this.m_paragraphIndent;
    set
    {
      this.m_paragraphIndent = value;
      this.FirstLineIndent = value;
    }
  }

  public bool LineLimit
  {
    get => this.m_lineLimit;
    set => this.m_lineLimit = value;
  }

  public bool MeasureTrailingSpaces
  {
    get => this.m_measureTrailingSpaces;
    set => this.m_measureTrailingSpaces = value;
  }

  public bool NoClip
  {
    get => this.m_noClip;
    set => this.m_noClip = value;
  }

  public PdfWordWrapType WordWrap
  {
    get => this.m_wrapType;
    set => this.m_wrapType = value;
  }

  internal float HorizontalScalingFactor
  {
    get => this.m_scalingFactor;
    set
    {
      this.m_scalingFactor = (double) value > 0.0 ? value : throw new ArgumentOutOfRangeException("The scaling factor can't be less of equal to zero.", "ScalingFactor");
    }
  }

  internal float FirstLineIndent
  {
    get => this.m_firstLineIndent;
    set => this.m_firstLineIndent = value;
  }

  public object Clone() => (object) (PdfStringFormat) this.MemberwiseClone();
}
