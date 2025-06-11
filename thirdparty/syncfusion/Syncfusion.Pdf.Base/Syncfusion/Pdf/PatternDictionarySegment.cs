// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PatternDictionarySegment
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class PatternDictionarySegment(JBIG2StreamDecoder streamDecoder) : JBIG2Segment(streamDecoder)
{
  private PatternDictionaryFlags m_patternDictionaryFlags = new PatternDictionaryFlags();
  private int m_width;
  private int m_height;
  private int m_grayMax;
  private JBIG2Image[] m_bitmaps;
  private int m_size;
  private BitOperation m_bitOperation = new BitOperation();

  internal int Size => this.m_size;

  public override void readSegment()
  {
    this.ReadPatternDictionaryFlags();
    this.m_width = (int) this.m_decoder.ReadByte();
    this.m_height = (int) this.m_decoder.ReadByte();
    short[] numArray = new short[4];
    this.m_decoder.ReadByte(numArray);
    this.m_grayMax = this.m_bitOperation.GetInt32(numArray);
    bool useMMR = this.m_patternDictionaryFlags.GetFlagValue("HD_MMR") == 1;
    int flagValue = this.m_patternDictionaryFlags.GetFlagValue("HD_TEMPLATE");
    if (!useMMR)
    {
      this.m_arithmeticDecoder.ResetGenericStats(flagValue, (ArithmeticDecoderStats) null);
      this.m_arithmeticDecoder.Start();
    }
    short[] adaptiveTemplateX = new short[4];
    short[] adaptiveTemplateY = new short[4];
    adaptiveTemplateX[0] = (short) -this.m_width;
    adaptiveTemplateY[0] = (short) 0;
    adaptiveTemplateX[1] = (short) -3;
    adaptiveTemplateY[1] = (short) -1;
    adaptiveTemplateX[2] = (short) 2;
    adaptiveTemplateY[2] = (short) -2;
    adaptiveTemplateX[3] = (short) -2;
    adaptiveTemplateY[3] = (short) -2;
    this.m_size = this.m_grayMax + 1;
    JBIG2Image jbiG2Image = new JBIG2Image(this.m_size * this.m_width, this.m_height, this.m_arithmeticDecoder, this.m_huffmanDecoder, this.m_mmrDecoder);
    jbiG2Image.Clear(0);
    jbiG2Image.ReadBitmap(useMMR, flagValue, false, false, (JBIG2Image) null, adaptiveTemplateX, adaptiveTemplateY, this.m_segmentHeader.DataLength - 7);
    JBIG2Image[] jbiG2ImageArray = new JBIG2Image[this.m_size];
    int x = 0;
    for (int index = 0; index < this.m_size; ++index)
    {
      jbiG2ImageArray[index] = jbiG2Image.GetSlice(x, 0, this.m_width, this.m_height);
      x += this.m_width;
    }
    this.m_bitmaps = jbiG2ImageArray;
  }

  internal JBIG2Image[] GetBitmaps() => this.m_bitmaps;

  private void ReadPatternDictionaryFlags()
  {
    this.m_patternDictionaryFlags.setFlags((int) this.m_decoder.ReadByte());
  }
}
