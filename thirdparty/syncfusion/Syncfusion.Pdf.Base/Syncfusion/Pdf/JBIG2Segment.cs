// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.JBIG2Segment
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal abstract class JBIG2Segment
{
  public const int SYMBOL_DICTIONARY = 0;
  public const int INTERMEDIATE_TEXT_REGION = 4;
  public const int IMMEDIATE_TEXT_REGION = 6;
  public const int IMMEDIATE_LOSSLESS_TEXT_REGION = 7;
  public const int PATTERN_DICTIONARY = 16 /*0x10*/;
  public const int INTERMEDIATE_HALFTONE_REGION = 20;
  public const int IMMEDIATE_HALFTONE_REGION = 22;
  public const int IMMEDIATE_LOSSLESS_HALFTONE_REGION = 23;
  public const int INTERMEDIATE_GENERIC_REGION = 36;
  public const int IMMEDIATE_GENERIC_REGION = 38;
  public const int IMMEDIATE_LOSSLESS_GENERIC_REGION = 39;
  public const int INTERMEDIATE_GENERIC_REFINEMENT_REGION = 40;
  public const int IMMEDIATE_GENERIC_REFINEMENT_REGION = 42;
  public const int IMMEDIATE_LOSSLESS_GENERIC_REFINEMENT_REGION = 43;
  public const int PAGE_INFORMATION = 48 /*0x30*/;
  public const int END_OF_PAGE = 49;
  public const int END_OF_STRIPE = 50;
  public const int END_OF_FILE = 51;
  public const int PROFILES = 52;
  public const int TABLES = 53;
  public const int EXTENSION = 62;
  public const int BITMAP = 70;
  protected internal SegmentHeader m_segmentHeader;
  protected internal HuffmanDecoder m_huffmanDecoder;
  protected internal ArithmeticDecoder m_arithmeticDecoder;
  protected internal MMRDecoder m_mmrDecoder;
  protected internal JBIG2StreamDecoder m_decoder;

  internal SegmentHeader SegHeader
  {
    get => this.m_segmentHeader;
    set => this.m_segmentHeader = value;
  }

  public JBIG2Segment(JBIG2StreamDecoder streamDecoder)
  {
    this.m_decoder = streamDecoder;
    this.m_huffmanDecoder = this.m_decoder.HuffDecoder;
    this.m_arithmeticDecoder = this.m_decoder.ArithDecoder;
    this.m_mmrDecoder = this.m_decoder.MmrDecoder;
  }

  protected short ReadAtValue()
  {
    short num;
    if (((int) (num = this.m_decoder.ReadByte()) & 128 /*0x80*/) != 0)
      num |= (short) -256;
    return num;
  }

  public abstract void readSegment();
}
