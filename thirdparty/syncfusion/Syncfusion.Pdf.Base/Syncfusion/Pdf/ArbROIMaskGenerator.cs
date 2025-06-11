// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ArbROIMaskGenerator
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class ArbROIMaskGenerator : ROIMaskGenerator
{
  private Quantizer src;
  private int[][] roiMask;
  private int[] maskLineLow;
  private int[] maskLineHigh;
  private int[] paddedMaskLine;
  private new bool roiInTile;

  public ArbROIMaskGenerator(ROI[] rois, int nrc, Quantizer src)
    : base(rois, nrc)
  {
    this.roiMask = new int[nrc][];
    this.src = src;
  }

  internal override bool getROIMask(DataBlockInt db, Subband sb, int magbits, int c)
  {
    int ulx = db.ulx;
    int uly = db.uly;
    int w1 = db.w;
    int h1 = db.h;
    int w2 = sb.w;
    int h2 = sb.h;
    int[] data = (int[]) db.Data;
    if (!this.tileMaskMade[c])
    {
      this.makeMask(sb, magbits, c);
      this.tileMaskMade[c] = true;
    }
    if (!this.roiInTile)
      return false;
    int[] numArray = this.roiMask[c];
    int index1 = (uly + h1 - 1) * w2 + ulx + w1 - 1;
    int index2 = w1 * h1 - 1;
    int num1 = w2 - w1;
    for (int index3 = h1; index3 > 0; --index3)
    {
      int num2 = w1;
      while (num2 > 0)
      {
        data[index2] = numArray[index1];
        --num2;
        --index1;
        --index2;
      }
      index1 -= num1;
    }
    return true;
  }

  public override string ToString() => "Fast rectangular ROI mask generator";

  internal override void makeMask(Subband sb, int magbits, int c)
  {
    ROI[] roiArray = this.roi_array;
    int ulcx = sb.ulcx;
    int ulcy = sb.ulcy;
    int w = sb.w;
    int h = sb.h;
    int num1 = w > h ? w : h;
    int[] numArray;
    if (this.roiMask[c] == null || this.roiMask[c].Length < w * h)
    {
      this.roiMask[c] = new int[w * h];
      numArray = this.roiMask[c];
    }
    else
    {
      numArray = this.roiMask[c];
      for (int index = w * h - 1; index >= 0; --index)
        numArray[index] = 0;
    }
    if (this.maskLineLow == null || this.maskLineLow.Length < (num1 + 1) / 2)
      this.maskLineLow = new int[(num1 + 1) / 2];
    if (this.maskLineHigh == null || this.maskLineHigh.Length < (num1 + 1) / 2)
      this.maskLineHigh = new int[(num1 + 1) / 2];
    this.roiInTile = false;
    for (int index1 = roiArray.Length - 1; index1 >= 0; --index1)
    {
      if (roiArray[index1].comp == c)
      {
        int num2 = magbits;
        if (roiArray[index1].arbShape)
        {
          ImgReaderPGM maskPgm = roiArray[index1].maskPGM;
          int imgUlx = this.src.ImgULX;
          int imgUly = this.src.ImgULY;
          int num3 = imgUlx + this.src.ImgWidth - 1;
          int num4 = imgUly + this.src.ImgHeight - 1;
          if (imgUlx <= ulcx + w && imgUly <= ulcy + h && num3 >= ulcx && num4 >= ulcy)
          {
            int num5 = imgUlx - ulcx;
            int num6 = num3 - ulcx;
            int num7 = imgUly - ulcy;
            int num8 = num4 - ulcy;
            int num9 = 0;
            int num10 = 0;
            if (num5 < 0)
            {
              num9 = -num5;
              num5 = 0;
            }
            if (num7 < 0)
            {
              num10 = -num7;
              num7 = 0;
            }
            int num11 = num6 > w - 1 ? w - num5 : num6 + 1 - num5;
            int num12 = num8 > h - 1 ? h - num7 : num8 + 1 - num7;
            DataBlockInt dataBlockInt = new DataBlockInt();
            int num13 = -ImgReaderPGM.DC_OFFSET;
            int num14 = 0;
            dataBlockInt.ulx = num9;
            dataBlockInt.w = num11;
            dataBlockInt.h = 1;
            int index2 = (num7 + num12 - 1) * w + num5 + num11 - 1;
            int num15 = num11;
            int num16 = w - num15;
            for (int index3 = num12; index3 > 0; --index3)
            {
              dataBlockInt.uly = num10 + index3 - 1;
              int[] dataInt = dataBlockInt.DataInt;
              int num17 = num15;
              while (num17 > 0)
              {
                if (dataInt[num17 - 1] != num13)
                {
                  numArray[index2] = num2;
                  ++num14;
                }
                --num17;
                --index2;
              }
              index2 -= num16;
            }
            if (num14 != 0)
              this.roiInTile = true;
          }
        }
        else if (roiArray[index1].rect)
        {
          int ulx = roiArray[index1].ulx;
          int uly = roiArray[index1].uly;
          int num18 = roiArray[index1].w + ulx - 1;
          int num19 = roiArray[index1].h + uly - 1;
          if (ulx <= ulcx + w && uly <= ulcy + h && num18 >= ulcx && num19 >= ulcy)
          {
            this.roiInTile = true;
            int num20 = ulx - ulcx;
            int num21 = num18 - ulcx;
            int num22 = uly - ulcy;
            int num23 = num19 - ulcy;
            int num24 = num20 < 0 ? 0 : num20;
            int num25 = num22 < 0 ? 0 : num22;
            int num26 = num21 > w - 1 ? w - num24 : num21 + 1 - num24;
            int num27 = num23 > h - 1 ? h - num25 : num23 + 1 - num25;
            int index4 = (num25 + num27 - 1) * w + num24 + num26 - 1;
            int num28 = num26;
            int num29 = w - num28;
            for (int index5 = num27; index5 > 0; --index5)
            {
              int num30 = num28;
              while (num30 > 0)
              {
                numArray[index4] = num2;
                --num30;
                --index4;
              }
              index4 -= num29;
            }
          }
        }
        else
        {
          int num31 = roiArray[index1].x - ulcx;
          int num32 = roiArray[index1].y - ulcy;
          int r = roiArray[index1].r;
          int index6 = h * w - 1;
          for (int index7 = h - 1; index7 >= 0; --index7)
          {
            int num33 = w - 1;
            while (num33 >= 0)
            {
              if ((num33 - num31) * (num33 - num31) + (index7 - num32) * (index7 - num32) < r * r)
              {
                numArray[index6] = num2;
                this.roiInTile = true;
              }
              --num33;
              --index6;
            }
          }
        }
      }
    }
    if (!sb.isNode)
      return;
    WaveletFilter verWfilter = sb.VerWFilter;
    WaveletFilter horWfilter = sb.HorWFilter;
    int num34 = verWfilter.SynLowNegSupport + verWfilter.SynLowPosSupport;
    int num35 = verWfilter.SynHighNegSupport + verWfilter.SynHighPosSupport;
    int num36 = horWfilter.SynLowNegSupport + horWfilter.SynLowPosSupport;
    int num37 = horWfilter.SynHighNegSupport + horWfilter.SynHighPosSupport;
    int num38 = num34 > num35 ? num34 : num35;
    int num39 = num36 > num37 ? num36 : num37;
    int num40 = num38 > num39 ? num38 : num39;
    this.paddedMaskLine = new int[num1 + num40];
    if (!this.roiInTile)
      return;
    this.decomp(sb, w, h, c);
  }

  private void decomp(Subband sb, int tilew, int tileh, int c)
  {
    int ulx = sb.ulx;
    int uly = sb.uly;
    int w = sb.w;
    int h = sb.h;
    int num1 = 0;
    int[] numArray = this.roiMask[c];
    int[] maskLineLow = this.maskLineLow;
    int[] maskLineHigh = this.maskLineHigh;
    int[] paddedMaskLine = this.paddedMaskLine;
    if (!sb.isNode)
      return;
    WaveletFilter horWfilter = sb.HorWFilter;
    int synLowNegSupport1 = horWfilter.SynLowNegSupport;
    int synHighNegSupport1 = horWfilter.SynHighNegSupport;
    int synLowPosSupport1 = horWfilter.SynLowPosSupport;
    int synHighPosSupport1 = horWfilter.SynHighPosSupport;
    int num2 = synLowNegSupport1 + synLowPosSupport1 + 1;
    int num3 = synHighNegSupport1 + synHighPosSupport1 + 1;
    int num4 = sb.ulcx % 2;
    int num5;
    int num6;
    if (sb.w % 2 == 0)
    {
      num5 = w / 2 - 1;
      num6 = num5;
    }
    else if (num4 == 0)
    {
      num5 = (w + 1) / 2 - 1;
      num6 = w / 2 - 1;
    }
    else
    {
      num6 = (w + 1) / 2 - 1;
      num5 = w / 2 - 1;
    }
    int num7 = synLowNegSupport1 > synHighNegSupport1 ? synLowNegSupport1 : synHighNegSupport1;
    int num8 = synLowPosSupport1 > synHighPosSupport1 ? synLowPosSupport1 : synHighPosSupport1;
    for (int index = num7 - 1; index >= 0; --index)
      paddedMaskLine[index] = 0;
    for (int index = num7 + w - 1 + num8; index >= w; --index)
      paddedMaskLine[index] = 0;
    int num9 = (uly + h) * tilew + ulx + w - 1;
    for (int index1 = h - 1; index1 >= 0; --index1)
    {
      num9 -= tilew;
      int index2 = num9;
      int num10 = w;
      int index3 = w - 1 + num7;
      while (num10 > 0)
      {
        paddedMaskLine[index3] = numArray[index2];
        --num10;
        --index2;
        --index3;
      }
      int num11 = num7 + num4 + 2 * num5 + synLowPosSupport1;
      int index4 = num5;
      while (index4 >= 0)
      {
        int index5 = num11;
        int num12 = num2;
        while (num12 > 0)
        {
          int num13 = paddedMaskLine[index5];
          if (num13 > num1)
            num1 = num13;
          --num12;
          --index5;
        }
        maskLineLow[index4] = num1;
        num1 = 0;
        --index4;
        num11 -= 2;
      }
      int num14 = num7 - num4 + 2 * num6 + 1 + synHighPosSupport1;
      int index6 = num6;
      while (index6 >= 0)
      {
        int index7 = num14;
        int num15 = num3;
        while (num15 > 0)
        {
          int num16 = paddedMaskLine[index7];
          if (num16 > num1)
            num1 = num16;
          --num15;
          --index7;
        }
        maskLineHigh[index6] = num1;
        num1 = 0;
        --index6;
        num14 -= 2;
      }
      int index8 = num9;
      int index9 = num6;
      while (index9 >= 0)
      {
        numArray[index8] = maskLineHigh[index9];
        --index9;
        --index8;
      }
      int index10 = num5;
      while (index10 >= 0)
      {
        numArray[index8] = maskLineLow[index10];
        --index10;
        --index8;
      }
    }
    WaveletFilter verWfilter = sb.VerWFilter;
    int synLowNegSupport2 = verWfilter.SynLowNegSupport;
    int synHighNegSupport2 = verWfilter.SynHighNegSupport;
    int synLowPosSupport2 = verWfilter.SynLowPosSupport;
    int synHighPosSupport2 = verWfilter.SynHighPosSupport;
    int num17 = synLowNegSupport2 + synLowPosSupport2 + 1;
    int num18 = synHighNegSupport2 + synHighPosSupport2 + 1;
    int num19 = sb.ulcy % 2;
    int num20;
    int num21;
    if (sb.h % 2 == 0)
    {
      num20 = h / 2 - 1;
      num21 = num20;
    }
    else if (sb.ulcy % 2 == 0)
    {
      num20 = (h + 1) / 2 - 1;
      num21 = h / 2 - 1;
    }
    else
    {
      num21 = (h + 1) / 2 - 1;
      num20 = h / 2 - 1;
    }
    int num22 = synLowNegSupport2 > synHighNegSupport2 ? synLowNegSupport2 : synHighNegSupport2;
    int num23 = synLowPosSupport2 > synHighPosSupport2 ? synLowPosSupport2 : synHighPosSupport2;
    for (int index = num22 - 1; index >= 0; --index)
      paddedMaskLine[index] = 0;
    for (int index = num22 + h - 1 + num23; index >= h; --index)
      paddedMaskLine[index] = 0;
    int num24 = (uly + h - 1) * tilew + ulx + w;
    for (int index11 = w - 1; index11 >= 0; --index11)
    {
      --num24;
      int index12 = num24;
      int num25 = h;
      int index13 = num25 - 1 + num22;
      while (num25 > 0)
      {
        paddedMaskLine[index13] = numArray[index12];
        --num25;
        index12 -= tilew;
        --index13;
      }
      int num26 = num22 + num19 + 2 * num20 + synLowPosSupport2;
      int index14 = num20;
      while (index14 >= 0)
      {
        int index15 = num26;
        int num27 = num17;
        while (num27 > 0)
        {
          int num28 = paddedMaskLine[index15];
          if (num28 > num1)
            num1 = num28;
          --num27;
          --index15;
        }
        maskLineLow[index14] = num1;
        num1 = 0;
        --index14;
        num26 -= 2;
      }
      int num29 = num22 - num19 + 2 * num21 + 1 + synHighPosSupport2;
      int index16 = num21;
      while (index16 >= 0)
      {
        int index17 = num29;
        int num30 = num18;
        while (num30 > 0)
        {
          int num31 = paddedMaskLine[index17];
          if (num31 > num1)
            num1 = num31;
          --num30;
          --index17;
        }
        maskLineHigh[index16] = num1;
        num1 = 0;
        --index16;
        num29 -= 2;
      }
      int index18 = num24;
      int index19 = num21;
      while (index19 >= 0)
      {
        numArray[index18] = maskLineHigh[index19];
        --index19;
        index18 -= tilew;
      }
      int index20 = num20;
      while (index20 >= 0)
      {
        numArray[index18] = maskLineLow[index20];
        --index20;
        index18 -= tilew;
      }
    }
    if (!sb.isNode)
      return;
    this.decomp(sb.HH, tilew, tileh, c);
    this.decomp(sb.LH, tilew, tileh, c);
    this.decomp(sb.HL, tilew, tileh, c);
    this.decomp(sb.LL, tilew, tileh, c);
  }
}
