// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.JBIG2Image
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.Pdf;

internal class JBIG2Image
{
  private int m_width;
  private int m_height;
  private int m_line;
  private BitArray data;
  private ArithmeticDecoder m_arithmeticDecoder;
  private HuffmanDecoder m_huffmanDecoder;
  private MMRDecoder m_mmrDecoder;
  private BitOperation m_bitOperation = new BitOperation();
  internal int BitmapNumber;

  internal JBIG2Image(
    int width,
    int height,
    ArithmeticDecoder arithmeticDecoder,
    HuffmanDecoder huffmanDecoder,
    MMRDecoder mmrDecoder)
  {
    this.m_width = width;
    this.m_height = height;
    this.m_arithmeticDecoder = arithmeticDecoder;
    this.m_huffmanDecoder = huffmanDecoder;
    this.m_mmrDecoder = mmrDecoder;
    this.m_line = width + 7 >> 3;
    this.data = new BitArray(width * height);
  }

  internal int Width => this.m_width;

  internal int Height => this.m_height;

  internal void ReadBitmap(
    bool useMMR,
    int template,
    bool typicalPredictionGenericDecodingOn,
    bool useSkip,
    JBIG2Image skipBitmap,
    short[] adaptiveTemplateX,
    short[] adaptiveTemplateY,
    int mmrDataLength)
  {
    if (useMMR)
    {
      this.m_mmrDecoder.Reset();
      int[] numArray1 = new int[this.m_width + 2];
      int[] numArray2 = new int[this.m_width + 2];
      numArray2[0] = numArray2[1] = this.m_width;
      for (int row = 0; row < this.m_height; ++row)
      {
        int index1;
        for (index1 = 0; numArray2[index1] < this.m_width; ++index1)
          numArray1[index1] = numArray2[index1];
        numArray1[index1] = numArray1[index1 + 1] = this.m_width;
        int index2 = 0;
        int num1 = 0;
        int num2 = 0;
        do
        {
          switch (this.m_mmrDecoder.Get2DCode())
          {
            case 0:
              if (numArray1[index2] < this.m_width)
              {
                num2 = numArray1[index2 + 1];
                index2 += 2;
                break;
              }
              break;
            case 1:
              int num3;
              int num4;
              if ((num1 & 1) != 0)
              {
                num3 = 0;
                int num5;
                do
                {
                  num3 += num5 = this.m_mmrDecoder.GetblackCode();
                }
                while (num5 >= 64 /*0x40*/);
                num4 = 0;
                int whiteCode;
                do
                {
                  num4 += whiteCode = this.m_mmrDecoder.GetWhiteCode();
                }
                while (whiteCode >= 64 /*0x40*/);
              }
              else
              {
                num3 = 0;
                int whiteCode;
                do
                {
                  num3 += whiteCode = this.m_mmrDecoder.GetWhiteCode();
                }
                while (whiteCode >= 64 /*0x40*/);
                num4 = 0;
                int num6;
                do
                {
                  num4 += num6 = this.m_mmrDecoder.GetblackCode();
                }
                while (num6 >= 64 /*0x40*/);
              }
              if (num3 > 0 || num4 > 0)
              {
                int[] numArray3 = numArray2;
                int index3 = num1;
                int num7 = index3 + 1;
                int num8;
                int num9 = num8 = num2 + num3;
                numArray3[index3] = num8;
                int num10 = num9;
                int[] numArray4 = numArray2;
                int index4 = num7;
                num1 = index4 + 1;
                int num11;
                int num12 = num11 = num10 + num4;
                numArray4[index4] = num11;
                num2 = num12;
                while (numArray1[index2] <= num2 && numArray1[index2] < this.m_width)
                  index2 += 2;
                break;
              }
              break;
            case 2:
              num2 = numArray2[num1++] = numArray1[index2];
              if (numArray1[index2] < this.m_width)
              {
                ++index2;
                break;
              }
              break;
            case 3:
              num2 = numArray2[num1++] = numArray1[index2] + 1;
              if (numArray1[index2] < this.m_width)
              {
                ++index2;
                while (numArray1[index2] <= num2 && numArray1[index2] < this.m_width)
                  index2 += 2;
                break;
              }
              break;
            case 4:
              try
              {
                num2 = numArray2[num1++] = numArray1[index2] - 1;
                if (index2 > 0)
                  --index2;
                else
                  ++index2;
                for (; numArray1[index2] <= num2; index2 += 2)
                {
                  if (numArray1[index2] >= this.m_width)
                    break;
                }
                break;
              }
              catch
              {
                break;
              }
            case 5:
              num2 = numArray2[num1++] = numArray1[index2] + 2;
              if (numArray1[index2] < this.m_width)
              {
                ++index2;
                while (numArray1[index2] <= num2 && numArray1[index2] < this.m_width)
                  index2 += 2;
                break;
              }
              break;
            case 6:
              num2 = numArray2[num1++] = numArray1[index2] - 2;
              if (index2 > 0)
                --index2;
              else
                ++index2;
              while (numArray1[index2] <= num2 && numArray1[index2] < this.m_width)
                index2 += 2;
              break;
            case 7:
              num2 = numArray2[num1++] = numArray1[index2] + 3;
              if (numArray1[index2] < this.m_width)
              {
                ++index2;
                while (numArray1[index2] <= num2 && numArray1[index2] < this.m_width)
                  index2 += 2;
                break;
              }
              break;
            case 8:
              num2 = numArray2[num1++] = numArray1[index2] - 3;
              if (index2 > 0)
                --index2;
              else
                ++index2;
              while (numArray1[index2] <= num2 && numArray1[index2] < this.m_width)
                index2 += 2;
              break;
          }
        }
        while (num2 < this.m_width);
        int[] numArray5 = numArray2;
        int index5 = num1;
        int num13 = index5 + 1;
        int width = this.m_width;
        numArray5[index5] = width;
        for (int index6 = 0; numArray2[index6] < this.m_width; index6 += 2)
        {
          for (int col = numArray2[index6]; col < numArray2[index6 + 1]; ++col)
            this.SetPixel(col, row, 1);
        }
      }
      if (mmrDataLength >= 0)
        this.m_mmrDecoder.SkipTo(mmrDataLength);
      else
        this.m_mmrDecoder.Get24Bits();
    }
    else
    {
      ImagePointer imagePointer1 = new ImagePointer(this);
      ImagePointer imagePointer2 = new ImagePointer(this);
      ImagePointer imagePointer3 = new ImagePointer(this);
      ImagePointer imagePointer4 = new ImagePointer(this);
      ImagePointer imagePointer5 = new ImagePointer(this);
      ImagePointer imagePointer6 = new ImagePointer(this);
      long context1 = 0;
      if (typicalPredictionGenericDecodingOn)
      {
        switch (template)
        {
          case 0:
            context1 = 14675L;
            break;
          case 1:
            context1 = 1946L;
            break;
          case 2:
            context1 = 227L;
            break;
          case 3:
            context1 = 394L;
            break;
        }
      }
      bool flag = false;
      for (int index = 0; index < this.m_height; ++index)
      {
        if (typicalPredictionGenericDecodingOn)
        {
          if (this.m_arithmeticDecoder.DecodeBit(context1, this.m_arithmeticDecoder.GenericRegionStats) != 0)
            flag = !flag;
          if (flag)
          {
            this.DuplicateRow(index, index - 1);
            continue;
          }
        }
        switch (template)
        {
          case 0:
            imagePointer1.SetPointer(0, index - 2);
            long number1 = this.m_bitOperation.Bit32Shift((long) imagePointer1.NextPixel(), 1, 0) | (long) imagePointer1.NextPixel();
            imagePointer2.SetPointer(0, index - 1);
            long number2 = this.m_bitOperation.Bit32Shift(this.m_bitOperation.Bit32Shift((long) imagePointer2.NextPixel(), 1, 0) | (long) imagePointer2.NextPixel(), 1, 0) | (long) imagePointer2.NextPixel();
            long number3 = 0;
            imagePointer3.SetPointer((int) adaptiveTemplateX[0], index + (int) adaptiveTemplateY[0]);
            imagePointer4.SetPointer((int) adaptiveTemplateX[1], index + (int) adaptiveTemplateY[1]);
            imagePointer5.SetPointer((int) adaptiveTemplateX[2], index + (int) adaptiveTemplateY[2]);
            imagePointer6.SetPointer((int) adaptiveTemplateX[3], index + (int) adaptiveTemplateY[3]);
            for (int col = 0; col < this.m_width; ++col)
            {
              long context2 = this.m_bitOperation.Bit32Shift(number1, 13, 0) | this.m_bitOperation.Bit32Shift(number2, 8, 0) | this.m_bitOperation.Bit32Shift(number3, 4, 0) | (long) (imagePointer3.NextPixel() << 3) | (long) (imagePointer4.NextPixel() << 2) | (long) (imagePointer5.NextPixel() << 1) | (long) imagePointer6.NextPixel();
              int num;
              if (useSkip && skipBitmap.GetPixel(col, index) != 0)
              {
                num = 0;
              }
              else
              {
                num = this.m_arithmeticDecoder.DecodeBit(context2, this.m_arithmeticDecoder.GenericRegionStats);
                if (num != 0)
                  this.SetPixel(col, index, 1);
              }
              number1 = (this.m_bitOperation.Bit32Shift(number1, 1, 0) | (long) imagePointer1.NextPixel()) & 7L;
              number2 = (this.m_bitOperation.Bit32Shift(number2, 1, 0) | (long) imagePointer2.NextPixel()) & 31L /*0x1F*/;
              number3 = (this.m_bitOperation.Bit32Shift(number3, 1, 0) | (long) num) & 15L;
            }
            continue;
          case 1:
            imagePointer1.SetPointer(0, index - 2);
            long number4 = this.m_bitOperation.Bit32Shift(this.m_bitOperation.Bit32Shift((long) imagePointer1.NextPixel(), 1, 0) | (long) imagePointer1.NextPixel(), 1, 0) | (long) imagePointer1.NextPixel();
            imagePointer2.SetPointer(0, index - 1);
            long number5 = this.m_bitOperation.Bit32Shift(this.m_bitOperation.Bit32Shift((long) imagePointer2.NextPixel(), 1, 0) | (long) imagePointer2.NextPixel(), 1, 0) | (long) imagePointer2.NextPixel();
            long number6 = 0;
            imagePointer3.SetPointer((int) adaptiveTemplateX[0], index + (int) adaptiveTemplateY[0]);
            for (int col = 0; col < this.m_width; ++col)
            {
              long context3 = this.m_bitOperation.Bit32Shift(number4, 9, 0) | this.m_bitOperation.Bit32Shift(number5, 4, 0) | this.m_bitOperation.Bit32Shift(number6, 1, 0) | (long) imagePointer3.NextPixel();
              int num;
              if (useSkip && skipBitmap.GetPixel(col, index) != 0)
              {
                num = 0;
              }
              else
              {
                num = this.m_arithmeticDecoder.DecodeBit(context3, this.m_arithmeticDecoder.GenericRegionStats);
                if (num != 0)
                  this.SetPixel(col, index, 1);
              }
              number4 = (this.m_bitOperation.Bit32Shift(number4, 1, 0) | (long) imagePointer1.NextPixel()) & 15L;
              number5 = (this.m_bitOperation.Bit32Shift(number5, 1, 0) | (long) imagePointer2.NextPixel()) & 31L /*0x1F*/;
              number6 = (this.m_bitOperation.Bit32Shift(number6, 1, 0) | (long) num) & 7L;
            }
            continue;
          case 2:
            imagePointer1.SetPointer(0, index - 2);
            long number7 = this.m_bitOperation.Bit32Shift((long) imagePointer1.NextPixel(), 1, 0) | (long) imagePointer1.NextPixel();
            imagePointer2.SetPointer(0, index - 1);
            long number8 = this.m_bitOperation.Bit32Shift((long) imagePointer2.NextPixel(), 1, 0) | (long) imagePointer2.NextPixel();
            long number9 = 0;
            imagePointer3.SetPointer((int) adaptiveTemplateX[0], index + (int) adaptiveTemplateY[0]);
            for (int col = 0; col < this.m_width; ++col)
            {
              long context4 = this.m_bitOperation.Bit32Shift(number7, 7, 0) | this.m_bitOperation.Bit32Shift(number8, 3, 0) | this.m_bitOperation.Bit32Shift(number9, 1, 0) | (long) imagePointer3.NextPixel();
              int num;
              if (useSkip && skipBitmap.GetPixel(col, index) != 0)
              {
                num = 0;
              }
              else
              {
                num = this.m_arithmeticDecoder.DecodeBit(context4, this.m_arithmeticDecoder.GenericRegionStats);
                if (num != 0)
                  this.SetPixel(col, index, 1);
              }
              number7 = (this.m_bitOperation.Bit32Shift(number7, 1, 0) | (long) imagePointer1.NextPixel()) & 7L;
              number8 = (this.m_bitOperation.Bit32Shift(number8, 1, 0) | (long) imagePointer2.NextPixel()) & 15L;
              number9 = (this.m_bitOperation.Bit32Shift(number9, 1, 0) | (long) num) & 3L;
            }
            continue;
          case 3:
            imagePointer2.SetPointer(0, index - 1);
            long number10 = this.m_bitOperation.Bit32Shift((long) imagePointer2.NextPixel(), 1, 0) | (long) imagePointer2.NextPixel();
            long number11 = 0;
            imagePointer3.SetPointer((int) adaptiveTemplateX[0], index + (int) adaptiveTemplateY[0]);
            for (int col = 0; col < this.m_width; ++col)
            {
              long context5 = this.m_bitOperation.Bit32Shift(number10, 5, 0) | this.m_bitOperation.Bit32Shift(number11, 1, 0) | (long) imagePointer3.NextPixel();
              int num;
              if (useSkip && skipBitmap.GetPixel(col, index) != 0)
              {
                num = 0;
              }
              else
              {
                num = this.m_arithmeticDecoder.DecodeBit(context5, this.m_arithmeticDecoder.GenericRegionStats);
                if (num != 0)
                  this.SetPixel(col, index, 1);
              }
              number10 = (this.m_bitOperation.Bit32Shift(number10, 1, 0) | (long) imagePointer2.NextPixel()) & 31L /*0x1F*/;
              number11 = (this.m_bitOperation.Bit32Shift(number11, 1, 0) | (long) num) & 15L;
            }
            continue;
          default:
            continue;
        }
      }
    }
  }

  internal void ReadGenericRefinementRegion(
    int template,
    bool typicalPredictionGenericRefinementOn,
    JBIG2Image referredToBitmap,
    int referenceDX,
    int referenceDY,
    short[] adaptiveTemplateX,
    short[] adaptiveTemplateY)
  {
    long context;
    ImagePointer imagePointer1;
    ImagePointer imagePointer2;
    ImagePointer imagePointer3;
    ImagePointer imagePointer4;
    ImagePointer imagePointer5;
    ImagePointer imagePointer6;
    ImagePointer imagePointer7;
    ImagePointer imagePointer8;
    ImagePointer imagePointer9;
    ImagePointer imagePointer10;
    if (template != 0)
    {
      context = 8L;
      imagePointer1 = new ImagePointer(this);
      imagePointer2 = new ImagePointer(this);
      imagePointer3 = new ImagePointer(referredToBitmap);
      imagePointer4 = new ImagePointer(referredToBitmap);
      imagePointer5 = new ImagePointer(referredToBitmap);
      imagePointer6 = new ImagePointer(this);
      imagePointer7 = new ImagePointer(this);
      imagePointer8 = new ImagePointer(referredToBitmap);
      imagePointer9 = new ImagePointer(referredToBitmap);
      imagePointer10 = new ImagePointer(referredToBitmap);
    }
    else
    {
      context = 16L /*0x10*/;
      imagePointer1 = new ImagePointer(this);
      imagePointer2 = new ImagePointer(this);
      imagePointer3 = new ImagePointer(referredToBitmap);
      imagePointer4 = new ImagePointer(referredToBitmap);
      imagePointer5 = new ImagePointer(referredToBitmap);
      imagePointer6 = new ImagePointer(this);
      imagePointer7 = new ImagePointer(referredToBitmap);
      imagePointer8 = new ImagePointer(referredToBitmap);
      imagePointer9 = new ImagePointer(referredToBitmap);
      imagePointer10 = new ImagePointer(referredToBitmap);
    }
    bool flag = false;
    for (int index = 0; index < this.m_height; ++index)
    {
      if (template != 0)
      {
        imagePointer1.SetPointer(0, index - 1);
        long number1 = (long) imagePointer1.NextPixel();
        imagePointer2.SetPointer(-1, index);
        imagePointer3.SetPointer(-referenceDX, index - 1 - referenceDY);
        imagePointer4.SetPointer(-1 - referenceDX, index - referenceDY);
        long number2 = this.m_bitOperation.Bit32Shift((long) imagePointer4.NextPixel(), 1, 0) | (long) imagePointer4.NextPixel();
        imagePointer5.SetPointer(-referenceDX, index + 1 - referenceDY);
        long number3 = (long) imagePointer5.NextPixel();
        long num;
        long number4 = num = 0L;
        long number5 = num;
        long number6 = num;
        if (typicalPredictionGenericRefinementOn)
        {
          imagePointer8.SetPointer(-1 - referenceDX, index - 1 - referenceDY);
          number6 = this.m_bitOperation.Bit32Shift(this.m_bitOperation.Bit32Shift((long) imagePointer8.NextPixel(), 1, 0) | (long) imagePointer8.NextPixel(), 1, 0) | (long) imagePointer8.NextPixel();
          imagePointer9.SetPointer(-1 - referenceDX, index - referenceDY);
          number5 = this.m_bitOperation.Bit32Shift(this.m_bitOperation.Bit32Shift((long) imagePointer9.NextPixel(), 1, 0) | (long) imagePointer9.NextPixel(), 1, 0) | (long) imagePointer9.NextPixel();
          imagePointer10.SetPointer(-1 - referenceDX, index + 1 - referenceDY);
          number4 = this.m_bitOperation.Bit32Shift(this.m_bitOperation.Bit32Shift((long) imagePointer10.NextPixel(), 1, 0) | (long) imagePointer10.NextPixel(), 1, 0) | (long) imagePointer10.NextPixel();
        }
        for (int col = 0; col < this.m_width; ++col)
        {
          number1 = (this.m_bitOperation.Bit32Shift(number1, 1, 0) | (long) imagePointer1.NextPixel()) & 7L;
          number2 = (this.m_bitOperation.Bit32Shift(number2, 1, 0) | (long) imagePointer4.NextPixel()) & 7L;
          number3 = (this.m_bitOperation.Bit32Shift(number3, 1, 0) | (long) imagePointer5.NextPixel()) & 3L;
          if (typicalPredictionGenericRefinementOn)
          {
            number6 = (this.m_bitOperation.Bit32Shift(number6, 1, 0) | (long) imagePointer8.NextPixel()) & 7L;
            number5 = (this.m_bitOperation.Bit32Shift(number5, 1, 0) | (long) imagePointer9.NextPixel()) & 7L;
            number4 = (this.m_bitOperation.Bit32Shift(number4, 1, 0) | (long) imagePointer10.NextPixel()) & 7L;
            if (this.m_arithmeticDecoder.DecodeBit(context, this.m_arithmeticDecoder.RefinementRegionStats) != 0)
              flag = !flag;
            if (number6 == 0L && number5 == 0L && number4 == 0L)
            {
              this.SetPixel(col, index, 0);
              continue;
            }
            if (number6 == 7L && number5 == 7L && number4 == 7L)
            {
              this.SetPixel(col, index, 1);
              continue;
            }
          }
          if (this.m_arithmeticDecoder.DecodeBit(this.m_bitOperation.Bit32Shift(number1, 7, 0) | (long) (imagePointer2.NextPixel() << 6) | (long) (imagePointer3.NextPixel() << 5) | this.m_bitOperation.Bit32Shift(number2, 2, 0) | number3, this.m_arithmeticDecoder.RefinementRegionStats) == 1)
            this.SetPixel(col, index, 1);
        }
      }
      else
      {
        imagePointer1.SetPointer(0, index - 1);
        long number7 = (long) imagePointer1.NextPixel();
        imagePointer2.SetPointer(-1, index);
        imagePointer3.SetPointer(-referenceDX, index - 1 - referenceDY);
        long number8 = (long) imagePointer3.NextPixel();
        imagePointer4.SetPointer(-1 - referenceDX, index - referenceDY);
        long number9 = this.m_bitOperation.Bit32Shift((long) imagePointer4.NextPixel(), 1, 0) | (long) imagePointer4.NextPixel();
        imagePointer5.SetPointer(-1 - referenceDX, index + 1 - referenceDY);
        long number10 = this.m_bitOperation.Bit32Shift((long) imagePointer5.NextPixel(), 1, 0) | (long) imagePointer5.NextPixel();
        imagePointer6.SetPointer((int) adaptiveTemplateX[0], index + (int) adaptiveTemplateY[0]);
        imagePointer7.SetPointer((int) adaptiveTemplateX[1] - referenceDX, index + (int) adaptiveTemplateY[1] - referenceDY);
        long num;
        long number11 = num = 0L;
        long number12 = num;
        long number13 = num;
        if (typicalPredictionGenericRefinementOn)
        {
          imagePointer8.SetPointer(-1 - referenceDX, index - 1 - referenceDY);
          number13 = this.m_bitOperation.Bit32Shift(this.m_bitOperation.Bit32Shift((long) imagePointer8.NextPixel(), 1, 0) | (long) imagePointer8.NextPixel(), 1, 0) | (long) imagePointer8.NextPixel();
          imagePointer9.SetPointer(-1 - referenceDX, index - referenceDY);
          number12 = this.m_bitOperation.Bit32Shift(this.m_bitOperation.Bit32Shift((long) imagePointer9.NextPixel(), 1, 0) | (long) imagePointer9.NextPixel(), 1, 0) | (long) imagePointer9.NextPixel();
          imagePointer10.SetPointer(-1 - referenceDX, index + 1 - referenceDY);
          number11 = this.m_bitOperation.Bit32Shift(this.m_bitOperation.Bit32Shift((long) imagePointer10.NextPixel(), 1, 0) | (long) imagePointer10.NextPixel(), 1, 0) | (long) imagePointer10.NextPixel();
        }
        for (int col = 0; col < this.m_width; ++col)
        {
          number7 = (this.m_bitOperation.Bit32Shift(number7, 1, 0) | (long) imagePointer1.NextPixel()) & 3L;
          number8 = (this.m_bitOperation.Bit32Shift(number8, 1, 0) | (long) imagePointer3.NextPixel()) & 3L;
          number9 = (this.m_bitOperation.Bit32Shift(number9, 1, 0) | (long) imagePointer4.NextPixel()) & 7L;
          number10 = (this.m_bitOperation.Bit32Shift(number10, 1, 0) | (long) imagePointer5.NextPixel()) & 7L;
          if (typicalPredictionGenericRefinementOn)
          {
            number13 = (this.m_bitOperation.Bit32Shift(number13, 1, 0) | (long) imagePointer8.NextPixel()) & 7L;
            number12 = (this.m_bitOperation.Bit32Shift(number12, 1, 0) | (long) imagePointer9.NextPixel()) & 7L;
            number11 = (this.m_bitOperation.Bit32Shift(number11, 1, 0) | (long) imagePointer10.NextPixel()) & 7L;
            if (this.m_arithmeticDecoder.DecodeBit(context, this.m_arithmeticDecoder.RefinementRegionStats) == 1)
              flag = !flag;
            if (number13 == 0L && number12 == 0L && number11 == 0L)
            {
              this.SetPixel(col, index, 0);
              continue;
            }
            if (number13 == 7L && number12 == 7L && number11 == 7L)
            {
              this.SetPixel(col, index, 1);
              continue;
            }
          }
          if (this.m_arithmeticDecoder.DecodeBit(this.m_bitOperation.Bit32Shift(number7, 11, 0) | (long) (imagePointer2.NextPixel() << 10) | this.m_bitOperation.Bit32Shift(number8, 8, 0) | this.m_bitOperation.Bit32Shift(number9, 5, 0) | this.m_bitOperation.Bit32Shift(number10, 2, 0) | (long) (imagePointer6.NextPixel() << 1) | (long) imagePointer7.NextPixel(), this.m_arithmeticDecoder.RefinementRegionStats) == 1)
            this.SetPixel(col, index, 1);
        }
      }
    }
  }

  internal void ReadTextRegion(
    bool huffman,
    bool symbolRefine,
    int noOfSymbolInstances,
    int logStrips,
    int noOfSymbols,
    int[][] symbolCodeTable,
    int symbolCodeLength,
    JBIG2Image[] symbols,
    int defaultPixel,
    int combinationOperator,
    bool transposed,
    int referenceCorner,
    int sOffset,
    int[][] huffmanFSTable,
    int[][] huffmanDSTable,
    int[][] huffmanDTTable,
    int[][] huffmanRDWTable,
    int[][] huffmanRDHTable,
    int[][] huffmanRDXTable,
    int[][] huffmanRDYTable,
    int[][] huffmanRSizeTable,
    int template,
    short[] symbolRegionAdaptiveTemplateX,
    short[] symbolRegionAdaptiveTemplateY,
    JBIG2StreamDecoder decoder)
  {
    int num1 = 1 << logStrips;
    this.Clear(defaultPixel);
    int num2 = (!huffman ? this.m_arithmeticDecoder.DecodeInt(this.m_arithmeticDecoder.IadtStats).IntResult : this.m_huffmanDecoder.DecodeInt(huffmanDTTable).IntResult) * -num1;
    int num3 = 0;
    int num4 = 0;
label_25:
    while (num3 < noOfSymbolInstances)
    {
      int num5 = !huffman ? this.m_arithmeticDecoder.DecodeInt(this.m_arithmeticDecoder.IadtStats).IntResult : this.m_huffmanDecoder.DecodeInt(huffmanDTTable).IntResult;
      num2 += num5 * num1;
      int num6 = !huffman ? this.m_arithmeticDecoder.DecodeInt(this.m_arithmeticDecoder.IafsStats).IntResult : this.m_huffmanDecoder.DecodeInt(huffmanFSTable).IntResult;
      num4 += num6;
      int num7 = num4;
      while (true)
      {
        int num8 = num1 != 1 ? (!huffman ? this.m_arithmeticDecoder.DecodeInt(this.m_arithmeticDecoder.IaitStats).IntResult : decoder.ReadBits(logStrips)) : 0;
        int num9 = num2 + num8;
        long index = !huffman ? this.m_arithmeticDecoder.DecodeIAID((long) symbolCodeLength, this.m_arithmeticDecoder.IaidStats) : (symbolCodeTable == null ? (long) decoder.ReadBits(symbolCodeLength) : (long) this.m_huffmanDecoder.DecodeInt(symbolCodeTable).IntResult);
        if (index < (long) noOfSymbols)
        {
          JBIG2Image bitmap;
          if ((!symbolRefine ? 0 : (!huffman ? this.m_arithmeticDecoder.DecodeInt(this.m_arithmeticDecoder.IariStats).IntResult : decoder.ReadBit())) != 0)
          {
            int intResult1;
            int intResult2;
            int intResult3;
            int intResult4;
            if (huffman)
            {
              intResult1 = this.m_huffmanDecoder.DecodeInt(huffmanRDWTable).IntResult;
              intResult2 = this.m_huffmanDecoder.DecodeInt(huffmanRDHTable).IntResult;
              intResult3 = this.m_huffmanDecoder.DecodeInt(huffmanRDXTable).IntResult;
              intResult4 = this.m_huffmanDecoder.DecodeInt(huffmanRDYTable).IntResult;
              decoder.ConsumeRemainingBits();
              this.m_arithmeticDecoder.Start();
            }
            else
            {
              intResult1 = this.m_arithmeticDecoder.DecodeInt(this.m_arithmeticDecoder.IardwStats).IntResult;
              intResult2 = this.m_arithmeticDecoder.DecodeInt(this.m_arithmeticDecoder.IardhStats).IntResult;
              intResult3 = this.m_arithmeticDecoder.DecodeInt(this.m_arithmeticDecoder.IardxStats).IntResult;
              intResult4 = this.m_arithmeticDecoder.DecodeInt(this.m_arithmeticDecoder.IardyStats).IntResult;
            }
            int referenceDX = (intResult1 >= 0 ? intResult1 : intResult1 - 1) / 2 + intResult3;
            int referenceDY = (intResult2 >= 0 ? intResult2 : intResult2 - 1) / 2 + intResult4;
            bitmap = new JBIG2Image(intResult1 + symbols[(int) index].m_width, intResult2 + symbols[(int) index].m_height, this.m_arithmeticDecoder, this.m_huffmanDecoder, this.m_mmrDecoder);
            bitmap.ReadGenericRefinementRegion(template, false, symbols[(int) index], referenceDX, referenceDY, symbolRegionAdaptiveTemplateX, symbolRegionAdaptiveTemplateY);
          }
          else
            bitmap = symbols[(int) index];
          int num10 = bitmap.m_width - 1;
          int num11 = bitmap.m_height - 1;
          if (transposed)
          {
            switch (referenceCorner)
            {
              case 0:
                this.Combine(bitmap, num9, num7, (long) combinationOperator);
                break;
              case 1:
                this.Combine(bitmap, num9, num7, (long) combinationOperator);
                break;
              case 2:
                this.Combine(bitmap, num9 - num10, num7, (long) combinationOperator);
                break;
              case 3:
                this.Combine(bitmap, num9 - num10, num7, (long) combinationOperator);
                break;
            }
            num7 += num11;
          }
          else
          {
            switch (referenceCorner)
            {
              case 0:
                this.Combine(bitmap, num7, num9 - num11, (long) combinationOperator);
                break;
              case 1:
                this.Combine(bitmap, num7, num9, (long) combinationOperator);
                break;
              case 2:
                this.Combine(bitmap, num7, num9 - num11, (long) combinationOperator);
                break;
              case 3:
                this.Combine(bitmap, num7, num9, (long) combinationOperator);
                break;
            }
            num7 += num10;
          }
        }
        ++num3;
        DecodeIntResult decodeIntResult = !huffman ? this.m_arithmeticDecoder.DecodeInt(this.m_arithmeticDecoder.IadsStats) : this.m_huffmanDecoder.DecodeInt(huffmanDSTable);
        if (decodeIntResult.BooleanResult)
        {
          int intResult = decodeIntResult.IntResult;
          num7 += sOffset + intResult;
        }
        else
          goto label_25;
      }
    }
  }

  internal void Clear(int defPixel) => this.data.Set(0, defPixel == 1);

  internal void Combine(JBIG2Image bitmap, int x, int y, long combOp)
  {
    int width = bitmap.m_width;
    int height = bitmap.m_height;
    int row1 = 0;
    int col1 = 0;
    for (int row2 = y; row2 < y + height && y + height <= this.m_height; ++row2)
    {
      for (int col2 = x; col2 < x + width; ++col2)
      {
        int pixel = bitmap.GetPixel(col1, row1);
        switch ((int) combOp)
        {
          case 0:
            this.SetPixel(col2, row2, this.GetPixel(col2, row2) | pixel);
            break;
          case 1:
            this.SetPixel(col2, row2, this.GetPixel(col2, row2) & pixel);
            break;
          case 2:
            this.SetPixel(col2, row2, this.GetPixel(col2, row2) ^ pixel);
            break;
          case 3:
            if (this.GetPixel(col2, row2) == 1 && pixel == 1 || this.GetPixel(col2, row2) == 0 && pixel == 0)
            {
              this.SetPixel(col2, row2, 1);
              break;
            }
            this.SetPixel(col2, row2, 0);
            break;
          case 4:
            this.SetPixel(col2, row2, pixel);
            break;
        }
        ++col1;
      }
      col1 = 0;
      ++row1;
    }
  }

  private void DuplicateRow(int yDest, int ySrc)
  {
    for (int col = 0; col < this.m_width; ++col)
      this.SetPixel(col, yDest, this.GetPixel(col, ySrc));
  }

  internal byte[] GetData(bool switchPixelColor)
  {
    byte[] data = new byte[this.m_height * this.m_line];
    int index1 = 0;
    int num1 = 0;
    for (int index2 = 0; index2 < this.m_height; ++index2)
    {
      for (int index3 = 0; index3 < this.m_width; ++index3)
      {
        if (this.data.Get(index1))
        {
          int index4 = (index1 + num1) / 8;
          int num2 = (index1 + num1) % 8;
          data[index4] |= (byte) (1 << 7 - num2);
        }
        ++index1;
      }
      num1 = this.m_line * 8 * (index2 + 1) - index1;
    }
    if (switchPixelColor)
    {
      for (int index5 = 0; index5 < data.Length; ++index5)
        data[index5] ^= byte.MaxValue;
    }
    return data;
  }

  internal JBIG2Image GetSlice(int x, int y, int width, int height)
  {
    JBIG2Image slice = new JBIG2Image(width, height, this.m_arithmeticDecoder, this.m_huffmanDecoder, this.m_mmrDecoder);
    int row1 = 0;
    int col1 = 0;
    for (int row2 = y; row2 < height; ++row2)
    {
      for (int col2 = x; col2 < x + width; ++col2)
      {
        slice.SetPixel(col1, row1, this.GetPixel(col2, row2));
        ++col1;
      }
      col1 = 0;
      ++row1;
    }
    return slice;
  }

  private void SetPixel(int col, int row, BitArray data, int value)
  {
    int index = row * this.m_width + col;
    data.Set(index, value == 1);
  }

  internal void SetPixel(int col, int row, int value) => this.SetPixel(col, row, this.data, value);

  internal int GetPixel(int col, int row)
  {
    try
    {
      return this.data.Get(row * this.m_width + col) ? 1 : 0;
    }
    catch
    {
      return 0;
    }
  }

  internal void Expand(int newHeight, int defaultPixel)
  {
    BitArray data = new BitArray(newHeight * this.m_width);
    for (int row = 0; row < this.m_height; ++row)
    {
      for (int col = 0; col < this.m_width; ++col)
        this.SetPixel(col, row, data, this.GetPixel(col, row));
    }
    this.m_height = newHeight;
    this.data = data;
  }
}
