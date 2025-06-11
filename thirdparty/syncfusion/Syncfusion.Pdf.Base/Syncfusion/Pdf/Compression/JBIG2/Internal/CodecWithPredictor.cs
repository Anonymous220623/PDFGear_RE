// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.CodecWithPredictor
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class CodecWithPredictor : TiffCodec
{
  public const int FIELD_PREDICTOR = 66;
  private static readonly TiffFieldInfo[] m_predictFieldInfo = new TiffFieldInfo[1]
  {
    new TiffFieldInfo(Syncfusion.Pdf.Compression.JBIG2.TiffTag.PREDICTOR, (short) 1, (short) 1, Syncfusion.Pdf.Compression.JBIG2.TiffType.SHORT, (short) 66, false, false, "Predictor")
  };
  private Predictor m_predictor;
  private int m_stride;
  private int m_rowSize;
  private TiffTagMethods m_parentTagMethods;
  private TiffTagMethods m_tagMethods;
  private TiffTagMethods m_childTagMethods;
  private bool m_passThruDecode;
  private bool m_passThruEncode;
  private CodecWithPredictor.PredictorType m_predictorType;

  public CodecWithPredictor(Tiff tif, Syncfusion.Pdf.Compression.JBIG2.Compression scheme, string name)
    : base(tif, scheme, name)
  {
    this.m_tagMethods = (TiffTagMethods) new CodecWithPredictorTagMethods();
  }

  public void TIFFPredictorInit(TiffTagMethods tagMethods)
  {
    this.m_tif.MergeFieldInfo(CodecWithPredictor.m_predictFieldInfo, CodecWithPredictor.m_predictFieldInfo.Length);
    this.m_childTagMethods = tagMethods;
    this.m_parentTagMethods = this.m_tif.m_tagmethods;
    this.m_tif.m_tagmethods = this.m_tagMethods;
    this.m_predictor = Predictor.NONE;
    this.m_predictorType = CodecWithPredictor.PredictorType.ptNone;
  }

  public void TIFFPredictorCleanup() => this.m_tif.m_tagmethods = this.m_parentTagMethods;

  public override bool SetupDecode() => this.PredictorSetupDecode();

  public override bool DecodeRow(byte[] buffer, int offset, int count, short plane)
  {
    return !this.m_passThruDecode ? this.PredictorDecodeRow(buffer, offset, count, plane) : this.predictor_decoderow(buffer, offset, count, plane);
  }

  public override bool DecodeStrip(byte[] buffer, int offset, int count, short plane)
  {
    return !this.m_passThruDecode ? this.PredictorDecodeTile(buffer, offset, count, plane) : this.predictor_decodestrip(buffer, offset, count, plane);
  }

  public override bool DecodeTile(byte[] buffer, int offset, int count, short plane)
  {
    return !this.m_passThruDecode ? this.PredictorDecodeTile(buffer, offset, count, plane) : this.predictor_decodetile(buffer, offset, count, plane);
  }

  public virtual bool predictor_setupdecode() => base.SetupDecode();

  public virtual bool predictor_decoderow(byte[] buffer, int offset, int count, short plane)
  {
    return base.DecodeRow(buffer, offset, count, plane);
  }

  public virtual bool predictor_decodestrip(byte[] buffer, int offset, int count, short plane)
  {
    return base.DecodeStrip(buffer, offset, count, plane);
  }

  public virtual bool predictor_decodetile(byte[] buffer, int offset, int count, short plane)
  {
    return base.DecodeTile(buffer, offset, count, plane);
  }

  public Predictor GetPredictorValue() => this.m_predictor;

  public void SetPredictorValue(Predictor value) => this.m_predictor = value;

  public TiffTagMethods GetChildTagMethods() => this.m_childTagMethods;

  private void predictorFunc(byte[] buffer, int offset, int count)
  {
    switch (this.m_predictorType)
    {
      case CodecWithPredictor.PredictorType.ptHorAcc8:
        this.horAcc8(buffer, offset, count);
        break;
      case CodecWithPredictor.PredictorType.ptHorAcc16:
        this.horAcc16(buffer, offset, count);
        break;
      case CodecWithPredictor.PredictorType.ptHorAcc32:
        this.horAcc32(buffer, offset, count);
        break;
      case CodecWithPredictor.PredictorType.ptSwabHorAcc16:
        this.swabHorAcc16(buffer, offset, count);
        break;
      case CodecWithPredictor.PredictorType.ptSwabHorAcc32:
        this.swabHorAcc32(buffer, offset, count);
        break;
      case CodecWithPredictor.PredictorType.ptHorDiff8:
        this.horDiff8(buffer, offset, count);
        break;
      case CodecWithPredictor.PredictorType.ptHorDiff16:
        this.horDiff16(buffer, offset, count);
        break;
      case CodecWithPredictor.PredictorType.ptHorDiff32:
        this.horDiff32(buffer, offset, count);
        break;
      case CodecWithPredictor.PredictorType.ptFpAcc:
        this.fpAcc(buffer, offset, count);
        break;
      case CodecWithPredictor.PredictorType.ptFpDiff:
        this.fpDiff(buffer, offset, count);
        break;
    }
  }

  private void horAcc8(byte[] buffer, int offset, int count)
  {
    int index = offset;
    if (count <= this.m_stride)
      return;
    count -= this.m_stride;
    if (this.m_stride == 3)
    {
      int num1 = (int) buffer[index];
      int num2 = (int) buffer[index + 1];
      int num3 = (int) buffer[index + 2];
      do
      {
        count -= 3;
        index += 3;
        num1 += (int) buffer[index];
        buffer[index] = (byte) num1;
        num2 += (int) buffer[index + 1];
        buffer[index + 1] = (byte) num2;
        num3 += (int) buffer[index + 2];
        buffer[index + 2] = (byte) num3;
      }
      while (count > 0);
    }
    else if (this.m_stride == 4)
    {
      int num4 = (int) buffer[index];
      int num5 = (int) buffer[index + 1];
      int num6 = (int) buffer[index + 2];
      int num7 = (int) buffer[index + 3];
      do
      {
        count -= 4;
        index += 4;
        num4 += (int) buffer[index];
        buffer[index] = (byte) num4;
        num5 += (int) buffer[index + 1];
        buffer[index + 1] = (byte) num5;
        num6 += (int) buffer[index + 2];
        buffer[index + 2] = (byte) num6;
        num7 += (int) buffer[index + 3];
        buffer[index + 3] = (byte) num7;
      }
      while (count > 0);
    }
    else
    {
      do
      {
        for (int stride = this.m_stride; stride > 0; --stride)
        {
          buffer[index + this.m_stride] = (byte) ((uint) buffer[index + this.m_stride] + (uint) buffer[index]);
          ++index;
        }
        count -= this.m_stride;
      }
      while (count > 0);
    }
  }

  private void horAcc16(byte[] buffer, int offset, int count)
  {
    short[] shorts = Tiff.ByteArrayToShorts(buffer, offset, count);
    int index = 0;
    int num1 = count / 2;
    if (num1 > this.m_stride)
    {
      int num2 = num1 - this.m_stride;
      do
      {
        for (int stride = this.m_stride; stride > 0; --stride)
        {
          shorts[index + this.m_stride] += shorts[index];
          ++index;
        }
        num2 -= this.m_stride;
      }
      while (num2 > 0);
    }
    Tiff.ShortsToByteArray(shorts, 0, count / 2, buffer, offset);
  }

  private void horAcc32(byte[] buffer, int offset, int count)
  {
    int[] ints = Tiff.ByteArrayToInts(buffer, offset, count);
    int index = 0;
    int num1 = count / 4;
    if (num1 > this.m_stride)
    {
      int num2 = num1 - this.m_stride;
      do
      {
        for (int stride = this.m_stride; stride > 0; --stride)
        {
          ints[index + this.m_stride] += ints[index];
          ++index;
        }
        num2 -= this.m_stride;
      }
      while (num2 > 0);
    }
    Tiff.IntsToByteArray(ints, 0, count / 4, buffer, offset);
  }

  private void swabHorAcc16(byte[] buffer, int offset, int count)
  {
    short[] shorts = Tiff.ByteArrayToShorts(buffer, offset, count);
    int index = 0;
    int count1 = count / 2;
    if (count1 > this.m_stride)
    {
      Tiff.SwabArrayOfShort(shorts, count1);
      int num = count1 - this.m_stride;
      do
      {
        for (int stride = this.m_stride; stride > 0; --stride)
        {
          shorts[index + this.m_stride] += shorts[index];
          ++index;
        }
        num -= this.m_stride;
      }
      while (num > 0);
    }
    Tiff.ShortsToByteArray(shorts, 0, count / 2, buffer, offset);
  }

  private void swabHorAcc32(byte[] buffer, int offset, int count)
  {
    int[] ints = Tiff.ByteArrayToInts(buffer, offset, count);
    int index = 0;
    int count1 = count / 4;
    if (count1 > this.m_stride)
    {
      Tiff.SwabArrayOfLong(ints, count1);
      int num = count1 - this.m_stride;
      do
      {
        for (int stride = this.m_stride; stride > 0; --stride)
        {
          ints[index + this.m_stride] += ints[index];
          ++index;
        }
        num -= this.m_stride;
      }
      while (num > 0);
    }
    Tiff.IntsToByteArray(ints, 0, count / 4, buffer, offset);
  }

  private void horDiff8(byte[] buffer, int offset, int count)
  {
    if (count <= this.m_stride)
      return;
    count -= this.m_stride;
    int index1 = offset;
    if (this.m_stride == 3)
    {
      int num1 = (int) buffer[index1];
      int num2 = (int) buffer[index1 + 1];
      int num3 = (int) buffer[index1 + 2];
      do
      {
        int num4 = (int) buffer[index1 + 3];
        buffer[index1 + 3] = (byte) (num4 - num1);
        num1 = num4;
        int num5 = (int) buffer[index1 + 4];
        buffer[index1 + 4] = (byte) (num5 - num2);
        num2 = num5;
        int num6 = (int) buffer[index1 + 5];
        buffer[index1 + 5] = (byte) (num6 - num3);
        num3 = num6;
        index1 += 3;
      }
      while ((count -= 3) > 0);
    }
    else if (this.m_stride == 4)
    {
      int num7 = (int) buffer[index1];
      int num8 = (int) buffer[index1 + 1];
      int num9 = (int) buffer[index1 + 2];
      int num10 = (int) buffer[index1 + 3];
      do
      {
        int num11 = (int) buffer[index1 + 4];
        buffer[index1 + 4] = (byte) (num11 - num7);
        num7 = num11;
        int num12 = (int) buffer[index1 + 5];
        buffer[index1 + 5] = (byte) (num12 - num8);
        num8 = num12;
        int num13 = (int) buffer[index1 + 6];
        buffer[index1 + 6] = (byte) (num13 - num9);
        num9 = num13;
        int num14 = (int) buffer[index1 + 7];
        buffer[index1 + 7] = (byte) (num14 - num10);
        num10 = num14;
        index1 += 4;
      }
      while ((count -= 4) > 0);
    }
    else
    {
      int index2 = index1 + (count - 1);
      do
      {
        for (int stride = this.m_stride; stride > 0; --stride)
        {
          buffer[index2 + this.m_stride] -= buffer[index2];
          --index2;
        }
      }
      while ((count -= this.m_stride) > 0);
    }
  }

  private void horDiff16(byte[] buffer, int offset, int count)
  {
    short[] shorts = Tiff.ByteArrayToShorts(buffer, offset, count);
    int num1 = 0;
    int num2 = count / 2;
    if (num2 > this.m_stride)
    {
      int num3 = num2 - this.m_stride;
      int index = num1 + (num3 - 1);
      do
      {
        for (int stride = this.m_stride; stride > 0; --stride)
        {
          shorts[index + this.m_stride] -= shorts[index];
          --index;
        }
        num3 -= this.m_stride;
      }
      while (num3 > 0);
    }
    Tiff.ShortsToByteArray(shorts, 0, count / 2, buffer, offset);
  }

  private void horDiff32(byte[] buffer, int offset, int count)
  {
    int[] ints = Tiff.ByteArrayToInts(buffer, offset, count);
    int num1 = 0;
    int num2 = count / 4;
    if (num2 > this.m_stride)
    {
      int num3 = num2 - this.m_stride;
      int index = num1 + (num3 - 1);
      do
      {
        for (int stride = this.m_stride; stride > 0; --stride)
        {
          ints[index + this.m_stride] -= ints[index];
          --index;
        }
        num3 -= this.m_stride;
      }
      while (num3 > 0);
    }
    Tiff.IntsToByteArray(ints, 0, count / 4, buffer, offset);
  }

  private void fpAcc(byte[] buffer, int offset, int count)
  {
    int num1 = (int) this.m_tif.m_dir.td_bitspersample / 8;
    int num2 = count / num1;
    int num3 = count;
    int index1 = offset;
    for (; num3 > this.m_stride; num3 -= this.m_stride)
    {
      for (int stride = this.m_stride; stride > 0; --stride)
      {
        buffer[index1 + this.m_stride] += buffer[index1];
        ++index1;
      }
    }
    byte[] dst = new byte[count];
    Buffer.BlockCopy((Array) buffer, offset, (Array) dst, 0, count);
    for (int index2 = 0; index2 < num2; ++index2)
    {
      for (int index3 = 0; index3 < num1; ++index3)
        buffer[offset + num1 * index2 + index3] = dst[(num1 - index3 - 1) * num2 + index2];
    }
  }

  private void fpDiff(byte[] buffer, int offset, int count)
  {
    byte[] dst = new byte[count];
    Buffer.BlockCopy((Array) buffer, offset, (Array) dst, 0, count);
    int num1 = (int) this.m_tif.m_dir.td_bitspersample / 8;
    int num2 = count / num1;
    for (int index1 = 0; index1 < num2; ++index1)
    {
      for (int index2 = 0; index2 < num1; ++index2)
        buffer[offset + (num1 - index2 - 1) * num2 + index1] = dst[num1 * index1 + index2];
    }
    int index3 = offset + count - this.m_stride - 1;
    for (int index4 = count; index4 > this.m_stride; index4 -= this.m_stride)
    {
      for (int stride = this.m_stride; stride > 0; --stride)
      {
        buffer[index3 + this.m_stride] -= buffer[index3];
        --index3;
      }
    }
  }

  private bool PredictorDecodeRow(byte[] buffer, int offset, int count, short plane)
  {
    if (!this.predictor_decoderow(buffer, offset, count, plane))
      return false;
    this.predictorFunc(buffer, offset, count);
    return true;
  }

  private bool PredictorDecodeTile(byte[] buffer, int offset, int count, short plane)
  {
    if (!this.predictor_decodetile(buffer, offset, count, plane))
      return false;
    while (count > 0)
    {
      this.predictorFunc(buffer, offset, this.m_rowSize);
      count -= this.m_rowSize;
      offset += this.m_rowSize;
    }
    return true;
  }

  private bool PredictorSetupDecode()
  {
    if (!this.predictor_setupdecode() || !this.PredictorSetup())
      return false;
    this.m_passThruDecode = true;
    if (this.m_predictor == Predictor.HORIZONTAL)
    {
      switch (this.m_tif.m_dir.td_bitspersample)
      {
        case 8:
          this.m_predictorType = CodecWithPredictor.PredictorType.ptHorAcc8;
          break;
        case 16 /*0x10*/:
          this.m_predictorType = CodecWithPredictor.PredictorType.ptHorAcc16;
          break;
        case 32 /*0x20*/:
          this.m_predictorType = CodecWithPredictor.PredictorType.ptHorAcc32;
          break;
      }
      this.m_passThruDecode = false;
      if ((this.m_tif.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
      {
        if (this.m_predictorType == CodecWithPredictor.PredictorType.ptHorAcc16)
        {
          this.m_predictorType = CodecWithPredictor.PredictorType.ptSwabHorAcc16;
          this.m_tif.m_postDecodeMethod = Tiff.PostDecodeMethodType.pdmNone;
        }
        else if (this.m_predictorType == CodecWithPredictor.PredictorType.ptHorAcc32)
        {
          this.m_predictorType = CodecWithPredictor.PredictorType.ptSwabHorAcc32;
          this.m_tif.m_postDecodeMethod = Tiff.PostDecodeMethodType.pdmNone;
        }
      }
    }
    else if (this.m_predictor == Predictor.FLOATINGPOINT)
    {
      this.m_predictorType = CodecWithPredictor.PredictorType.ptFpAcc;
      this.m_passThruDecode = false;
      if ((this.m_tif.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
        this.m_tif.m_postDecodeMethod = Tiff.PostDecodeMethodType.pdmNone;
    }
    return true;
  }

  private bool PredictorSetup()
  {
    TiffDirectory dir = this.m_tif.m_dir;
    switch (this.m_predictor)
    {
      case Predictor.NONE:
        return true;
      case Predictor.HORIZONTAL:
        if (dir.td_bitspersample != (short) 8 && dir.td_bitspersample != (short) 16 /*0x10*/ && dir.td_bitspersample != (short) 32 /*0x20*/)
          return false;
        break;
      case Predictor.FLOATINGPOINT:
        if (dir.td_sampleformat != SampleFormat.IEEEFP)
          return false;
        break;
      default:
        return false;
    }
    this.m_stride = dir.td_planarconfig == PlanarConfig.CONTIG ? (int) dir.td_samplesperpixel : 1;
    this.m_rowSize = !this.m_tif.IsTiled() ? this.m_tif.ScanlineSize() : this.m_tif.TileRowSize();
    return true;
  }

  private enum PredictorType
  {
    ptNone,
    ptHorAcc8,
    ptHorAcc16,
    ptHorAcc32,
    ptSwabHorAcc16,
    ptSwabHorAcc32,
    ptHorDiff8,
    ptHorDiff16,
    ptHorDiff32,
    ptFpAcc,
    ptFpDiff,
  }
}
