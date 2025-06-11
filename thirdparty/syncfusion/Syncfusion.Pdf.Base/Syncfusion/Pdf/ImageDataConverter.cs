// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ImageDataConverter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal class ImageDataConverter : ImgDataAdapter, BlockImageDataSource, ImageData
{
  private DataBlock srcBlk = (DataBlock) new DataBlockInt();
  private BlockImageDataSource src;
  private int fp;

  internal ImageDataConverter(BlockImageDataSource imgSrc, int fp)
    : base((ImageData) imgSrc)
  {
    this.src = imgSrc;
    this.fp = fp;
  }

  internal ImageDataConverter(BlockImageDataSource imgSrc)
    : base((ImageData) imgSrc)
  {
    this.src = imgSrc;
    this.fp = 0;
  }

  public virtual int getFixedPoint(int c) => this.fp;

  public virtual DataBlock getCompData(DataBlock blk, int c) => this.getData(blk, c, false);

  public DataBlock getInternCompData(DataBlock blk, int c) => this.getData(blk, c, true);

  private DataBlock getData(DataBlock blk, int c, bool intern)
  {
    int dataType = blk.DataType;
    DataBlock blk1;
    if (dataType == this.srcBlk.DataType)
    {
      blk1 = blk;
    }
    else
    {
      blk1 = this.srcBlk;
      blk1.ulx = blk.ulx;
      blk1.uly = blk.uly;
      blk1.w = blk.w;
      blk1.h = blk.h;
    }
    this.srcBlk = !intern ? this.src.getCompData(blk1, c) : this.src.getInternCompData(blk1, c);
    if (this.srcBlk.DataType == dataType)
      return this.srcBlk;
    int w = this.srcBlk.w;
    int h = this.srcBlk.h;
    switch (dataType)
    {
      case 3:
        int[] numArray1 = (int[]) blk.Data;
        if (numArray1 == null || numArray1.Length < w * h)
        {
          numArray1 = new int[w * h];
          blk.Data = (object) numArray1;
        }
        blk.scanw = this.srcBlk.w;
        blk.offset = 0;
        blk.progressive = this.srcBlk.progressive;
        float[] data1 = (float[]) this.srcBlk.Data;
        if (this.fp != 0)
        {
          float num1 = (float) (1 << this.fp);
          int num2 = h - 1;
          int index1 = w * h - 1;
          int index2 = this.srcBlk.offset + (h - 1) * this.srcBlk.scanw + w - 1;
          for (; num2 >= 0; --num2)
          {
            int num3 = index1 - w;
            while (index1 > num3)
            {
              numArray1[index1] = (double) data1[index2] <= 0.0 ? (int) ((double) data1[index2] * (double) num1 - 0.5) : (int) ((double) data1[index2] * (double) num1 + 0.5);
              --index1;
              --index2;
            }
            index2 -= this.srcBlk.scanw - w;
          }
          break;
        }
        int num4 = h - 1;
        int index3 = w * h - 1;
        int index4 = this.srcBlk.offset + (h - 1) * this.srcBlk.scanw + w - 1;
        for (; num4 >= 0; --num4)
        {
          int num5 = index3 - w;
          while (index3 > num5)
          {
            numArray1[index3] = (double) data1[index4] <= 0.0 ? (int) ((double) data1[index4] - 0.5) : (int) ((double) data1[index4] + 0.5);
            --index3;
            --index4;
          }
          index4 -= this.srcBlk.scanw - w;
        }
        break;
      case 4:
        float[] numArray2 = (float[]) blk.Data;
        if (numArray2 == null || numArray2.Length < w * h)
        {
          numArray2 = new float[w * h];
          blk.Data = (object) numArray2;
        }
        blk.scanw = this.srcBlk.w;
        blk.offset = 0;
        blk.progressive = this.srcBlk.progressive;
        int[] data2 = (int[]) this.srcBlk.Data;
        this.fp = this.src.getFixedPoint(c);
        if (this.fp != 0)
        {
          float num6 = 1f / (float) (1 << this.fp);
          int num7 = h - 1;
          int index5 = w * h - 1;
          int index6 = this.srcBlk.offset + (h - 1) * this.srcBlk.scanw + w - 1;
          for (; num7 >= 0; --num7)
          {
            int num8 = index5 - w;
            while (index5 > num8)
            {
              numArray2[index5] = (float) data2[index6] * num6;
              --index5;
              --index6;
            }
            index6 -= this.srcBlk.scanw - w;
          }
          break;
        }
        int num9 = h - 1;
        int index7 = w * h - 1;
        int index8 = this.srcBlk.offset + (h - 1) * this.srcBlk.scanw + w - 1;
        for (; num9 >= 0; --num9)
        {
          int num10 = index7 - w;
          while (index7 > num10)
          {
            numArray2[index7] = (float) data2[index8];
            --index7;
            --index8;
          }
          index8 -= this.srcBlk.scanw - w;
        }
        break;
      default:
        throw new ArgumentException("Only integer and float data are supported by JJ2000");
    }
    return blk;
  }
}
