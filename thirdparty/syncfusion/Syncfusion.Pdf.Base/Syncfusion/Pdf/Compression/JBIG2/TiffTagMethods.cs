// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.TiffTagMethods
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Compression.JBIG2.Internal;
using System;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2;

internal class TiffTagMethods
{
  private const short DATATYPE_VOID = 0;
  private const short DATATYPE_INT = 1;
  private const short DATATYPE_UINT = 2;
  private const short DATATYPE_IEEEFP = 3;

  public virtual bool SetField(Tiff tif, TiffTag tag, FieldValue[] value)
  {
    TiffDirectory dir = tif.m_dir;
    bool flag1 = true;
    int v = 0;
    bool flag2 = false;
    bool flag3 = false;
    bool flag4 = false;
    switch (tag)
    {
      case TiffTag.SUBFILETYPE:
        dir.td_subfiletype = (FileType) value[0].ToByte();
        break;
      case TiffTag.IMAGEWIDTH:
        dir.td_imagewidth = value[0].ToInt();
        break;
      case TiffTag.IMAGELENGTH:
        dir.td_imagelength = value[0].ToInt();
        break;
      case TiffTag.BITSPERSAMPLE:
        dir.td_bitspersample = value[0].ToShort();
        if ((tif.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
        {
          if (dir.td_bitspersample == (short) 16 /*0x10*/)
          {
            tif.m_postDecodeMethod = Tiff.PostDecodeMethodType.pdmSwab16Bit;
            break;
          }
          if (dir.td_bitspersample == (short) 24)
          {
            tif.m_postDecodeMethod = Tiff.PostDecodeMethodType.pdmSwab24Bit;
            break;
          }
          if (dir.td_bitspersample == (short) 32 /*0x20*/)
          {
            tif.m_postDecodeMethod = Tiff.PostDecodeMethodType.pdmSwab32Bit;
            break;
          }
          if (dir.td_bitspersample == (short) 64 /*0x40*/)
          {
            tif.m_postDecodeMethod = Tiff.PostDecodeMethodType.pdmSwab64Bit;
            break;
          }
          if (dir.td_bitspersample == (short) 128 /*0x80*/)
          {
            tif.m_postDecodeMethod = Tiff.PostDecodeMethodType.pdmSwab64Bit;
            break;
          }
          break;
        }
        break;
      case TiffTag.COMPRESSION:
        Syncfusion.Pdf.Compression.JBIG2.Compression scheme = (Syncfusion.Pdf.Compression.JBIG2.Compression) (value[0].ToInt() & (int) ushort.MaxValue);
        if (tif.fieldSet(7))
        {
          if (dir.td_compression != scheme)
          {
            tif.m_currentCodec.Cleanup();
            tif.m_flags &= ~TiffFlags.CODERSETUP;
          }
          else
            break;
        }
        flag1 = tif.setCompressionScheme(scheme);
        if (flag1)
        {
          dir.td_compression = scheme;
          break;
        }
        flag1 = false;
        break;
      case TiffTag.PHOTOMETRIC:
        dir.td_photometric = (Photometric) value[0].ToInt();
        break;
      case TiffTag.THRESHHOLDING:
        dir.td_threshholding = (Threshold) value[0].ToByte();
        break;
      case TiffTag.FILLORDER:
        FillOrder fillOrder = (FillOrder) value[0].ToInt();
        switch (fillOrder)
        {
          case FillOrder.MSB2LSB:
          case FillOrder.LSB2MSB:
            dir.td_fillorder = fillOrder;
            break;
          default:
            flag3 = true;
            break;
        }
        break;
      case TiffTag.ORIENTATION:
        Orientation orientation = (Orientation) value[0].ToInt();
        if (orientation < Orientation.TOPLEFT || Orientation.LEFTBOT < orientation)
        {
          flag3 = true;
          break;
        }
        dir.td_orientation = orientation;
        break;
      case TiffTag.SAMPLESPERPIXEL:
        int num1 = value[0].ToInt();
        if (num1 == 0)
        {
          flag3 = true;
          break;
        }
        dir.td_samplesperpixel = (short) num1;
        break;
      case TiffTag.ROWSPERSTRIP:
        int num2 = value[0].ToInt();
        if (num2 == 0)
        {
          flag4 = true;
          break;
        }
        dir.td_rowsperstrip = num2;
        if (!tif.fieldSet(2))
        {
          dir.td_tilelength = num2;
          dir.td_tilewidth = dir.td_imagewidth;
          break;
        }
        break;
      case TiffTag.MINSAMPLEVALUE:
        dir.td_minsamplevalue = value[0].ToUShort();
        break;
      case TiffTag.MAXSAMPLEVALUE:
        dir.td_maxsamplevalue = value[0].ToUShort();
        break;
      case TiffTag.XRESOLUTION:
        dir.td_xresolution = value[0].ToFloat();
        break;
      case TiffTag.YRESOLUTION:
        dir.td_yresolution = value[0].ToFloat();
        break;
      case TiffTag.PLANARCONFIG:
        PlanarConfig planarConfig = (PlanarConfig) value[0].ToInt();
        switch (planarConfig)
        {
          case PlanarConfig.CONTIG:
          case PlanarConfig.SEPARATE:
            dir.td_planarconfig = planarConfig;
            break;
          default:
            flag3 = true;
            break;
        }
        break;
      case TiffTag.XPOSITION:
        dir.td_xposition = value[0].ToFloat();
        break;
      case TiffTag.YPOSITION:
        dir.td_yposition = value[0].ToFloat();
        break;
      case TiffTag.RESOLUTIONUNIT:
        ResUnit resUnit = (ResUnit) value[0].ToInt();
        if (resUnit < ResUnit.NONE || ResUnit.CENTIMETER < resUnit)
        {
          flag3 = true;
          break;
        }
        dir.td_resolutionunit = resUnit;
        break;
      case TiffTag.PAGENUMBER:
        dir.td_pagenumber[0] = value[0].ToShort();
        dir.td_pagenumber[1] = value[1].ToShort();
        break;
      case TiffTag.TRANSFERFUNCTION:
        int num3 = (int) dir.td_samplesperpixel - (int) dir.td_extrasamples > 1 ? 3 : 1;
        for (int index = 0; index < num3; ++index)
          Tiff.setShortArray(out dir.td_transferfunction[index], value[0].ToShortArray(), 1 << (int) dir.td_bitspersample);
        break;
      case TiffTag.COLORMAP:
        int n1 = 1 << (int) dir.td_bitspersample;
        Tiff.setShortArray(out dir.td_colormap[0], value[0].ToShortArray(), n1);
        Tiff.setShortArray(out dir.td_colormap[1], value[1].ToShortArray(), n1);
        Tiff.setShortArray(out dir.td_colormap[2], value[2].ToShortArray(), n1);
        break;
      case TiffTag.HALFTONEHINTS:
        dir.td_halftonehints[0] = value[0].ToShort();
        dir.td_halftonehints[1] = value[1].ToShort();
        break;
      case TiffTag.TILEWIDTH:
        int num4 = value[0].ToInt();
        if (num4 % 16 /*0x10*/ != 0 && tif.m_mode != 0)
        {
          flag4 = true;
          break;
        }
        dir.td_tilewidth = num4;
        tif.m_flags |= TiffFlags.ISTILED;
        break;
      case TiffTag.TILELENGTH:
        int num5 = value[0].ToInt();
        if (num5 % 16 /*0x10*/ != 0 && tif.m_mode != 0)
        {
          flag4 = true;
          break;
        }
        dir.td_tilelength = num5;
        tif.m_flags |= TiffFlags.ISTILED;
        break;
      case TiffTag.SUBIFD:
        if ((tif.m_flags & TiffFlags.INSUBIFD) != TiffFlags.INSUBIFD)
        {
          dir.td_nsubifd = value[0].ToShort();
          Tiff.setLongArray(out dir.td_subifd, value[1].ToIntArray(), (int) dir.td_nsubifd);
          break;
        }
        flag1 = false;
        break;
      case TiffTag.INKNAMES:
        int slen = value[0].ToInt();
        string str = value[1].ToString();
        int n2 = TiffTagMethods.checkInkNamesString(tif, slen, str);
        flag1 = n2 > 0;
        if (n2 > 0)
        {
          TiffTagMethods.setNString(out dir.td_inknames, str, n2);
          dir.td_inknameslen = n2;
          break;
        }
        break;
      case TiffTag.EXTRASAMPLES:
        if (!TiffTagMethods.setExtraSamples(dir, ref v, value))
        {
          flag3 = true;
          break;
        }
        break;
      case TiffTag.SAMPLEFORMAT:
        SampleFormat sampleFormat1 = (SampleFormat) value[0].ToInt();
        if (sampleFormat1 < SampleFormat.UINT || SampleFormat.COMPLEXIEEEFP < sampleFormat1)
        {
          flag3 = true;
          break;
        }
        dir.td_sampleformat = sampleFormat1;
        if (dir.td_sampleformat == SampleFormat.COMPLEXINT && dir.td_bitspersample == (short) 32 /*0x20*/ && tif.m_postDecodeMethod == Tiff.PostDecodeMethodType.pdmSwab32Bit)
        {
          tif.m_postDecodeMethod = Tiff.PostDecodeMethodType.pdmSwab16Bit;
          break;
        }
        if ((dir.td_sampleformat == SampleFormat.COMPLEXINT || dir.td_sampleformat == SampleFormat.COMPLEXIEEEFP) && dir.td_bitspersample == (short) 64 /*0x40*/ && tif.m_postDecodeMethod == Tiff.PostDecodeMethodType.pdmSwab64Bit)
        {
          tif.m_postDecodeMethod = Tiff.PostDecodeMethodType.pdmSwab32Bit;
          break;
        }
        break;
      case TiffTag.SMINSAMPLEVALUE:
        dir.td_sminsamplevalue = value[0].ToDouble();
        break;
      case TiffTag.SMAXSAMPLEVALUE:
        dir.td_smaxsamplevalue = value[0].ToDouble();
        break;
      case TiffTag.YCBCRSUBSAMPLING:
        dir.td_ycbcrsubsampling[0] = value[0].ToShort();
        dir.td_ycbcrsubsampling[1] = value[1].ToShort();
        break;
      case TiffTag.YCBCRPOSITIONING:
        dir.td_ycbcrpositioning = (YCbCrPosition) value[0].ToByte();
        break;
      case TiffTag.REFERENCEBLACKWHITE:
        Tiff.setFloatArray(out dir.td_refblackwhite, value[0].ToFloatArray(), 6);
        break;
      case TiffTag.MATTEING:
        dir.td_extrasamples = value[0].ToShort() == (short) 0 ? (short) 0 : (short) 1;
        if (dir.td_extrasamples != (short) 0)
        {
          dir.td_sampleinfo = new ExtraSample[1];
          dir.td_sampleinfo[0] = ExtraSample.ASSOCALPHA;
          break;
        }
        break;
      case TiffTag.DATATYPE:
        int num6 = value[0].ToInt();
        SampleFormat sampleFormat2 = SampleFormat.VOID;
        switch (num6)
        {
          case 0:
            sampleFormat2 = SampleFormat.VOID;
            break;
          case 1:
            sampleFormat2 = SampleFormat.INT;
            break;
          case 2:
            sampleFormat2 = SampleFormat.UINT;
            break;
          case 3:
            sampleFormat2 = SampleFormat.IEEEFP;
            break;
          default:
            flag3 = true;
            break;
        }
        if (!flag3)
        {
          dir.td_sampleformat = sampleFormat2;
          break;
        }
        break;
      case TiffTag.IMAGEDEPTH:
        dir.td_imagedepth = value[0].ToInt();
        break;
      case TiffTag.TILEDEPTH:
        int num7 = value[0].ToInt();
        if (num7 == 0)
        {
          flag4 = true;
          break;
        }
        dir.td_tiledepth = num7;
        break;
      default:
        TiffFieldInfo fieldInfo = tif.FindFieldInfo(tag, TiffType.NOTYPE);
        if (fieldInfo == null || fieldInfo.Bit != (short) 65)
        {
          flag1 = false;
          break;
        }
        int index1 = -1;
        for (int index2 = 0; index2 < dir.td_customValueCount; ++index2)
        {
          if (dir.td_customValues[index2].info.Tag == tag)
          {
            index1 = index2;
            dir.td_customValues[index2].value = (byte[]) null;
            break;
          }
        }
        if (index1 == -1)
        {
          ++dir.td_customValueCount;
          TiffTagValue[] tiffTagValueArray = Tiff.Realloc(dir.td_customValues, dir.td_customValueCount - 1, dir.td_customValueCount);
          dir.td_customValues = tiffTagValueArray;
          index1 = dir.td_customValueCount - 1;
          dir.td_customValues[index1].info = fieldInfo;
          dir.td_customValues[index1].value = (byte[]) null;
          dir.td_customValues[index1].count = 0;
        }
        int num8 = Tiff.dataSize(fieldInfo.Type);
        if (num8 == 0)
        {
          flag1 = false;
          flag2 = true;
          break;
        }
        int num9 = 0;
        dir.td_customValues[index1].count = !fieldInfo.PassCount ? (fieldInfo.WriteCount == (short) -1 || fieldInfo.WriteCount == (short) -3 ? 1 : (fieldInfo.WriteCount != (short) -2 ? (int) fieldInfo.WriteCount : (int) dir.td_samplesperpixel)) : (fieldInfo.WriteCount != (short) -3 ? value[num9++].ToInt() : value[num9++].ToInt());
        int num10;
        if (fieldInfo.Type == TiffType.ASCII)
        {
          string s;
          ref string local = ref s;
          FieldValue[] fieldValueArray = value;
          int index3 = num9;
          num10 = index3 + 1;
          string cp = fieldValueArray[index3].ToString();
          Tiff.setString(out local, cp);
          dir.td_customValues[index1].value = Tiff.Latin1Encoding.GetBytes(s);
          break;
        }
        dir.td_customValues[index1].value = new byte[num8 * dir.td_customValues[index1].count];
        if ((fieldInfo.PassCount || fieldInfo.WriteCount == (short) -1 || fieldInfo.WriteCount == (short) -3 || fieldInfo.WriteCount == (short) -2 || dir.td_customValues[index1].count > 1) && fieldInfo.Tag != TiffTag.PAGENUMBER && fieldInfo.Tag != TiffTag.HALFTONEHINTS && fieldInfo.Tag != TiffTag.YCBCRSUBSAMPLING && fieldInfo.Tag != TiffTag.DOTRANGE)
        {
          FieldValue[] fieldValueArray = value;
          int index4 = num9;
          num10 = index4 + 1;
          byte[] bytes = fieldValueArray[index4].GetBytes();
          Buffer.BlockCopy((Array) bytes, 0, (Array) dir.td_customValues[index1].value, 0, Math.Min(bytes.Length, dir.td_customValues[index1].value.Length));
          break;
        }
        byte[] dst = dir.td_customValues[index1].value;
        int index5 = 0;
        int num11 = 0;
        while (num11 < dir.td_customValues[index1].count)
        {
          switch (fieldInfo.Type)
          {
            case TiffType.BYTE:
            case TiffType.UNDEFINED:
              dst[index5] = value[num9 + num11].GetBytes()[0];
              break;
            case TiffType.SHORT:
              Buffer.BlockCopy((Array) BitConverter.GetBytes(value[num9 + num11].ToShort()), 0, (Array) dst, index5, num8);
              break;
            case TiffType.LONG:
            case TiffType.IFD:
              Buffer.BlockCopy((Array) BitConverter.GetBytes(value[num9 + num11].ToInt()), 0, (Array) dst, index5, num8);
              break;
            case TiffType.RATIONAL:
            case TiffType.SRATIONAL:
            case TiffType.FLOAT:
              Buffer.BlockCopy((Array) BitConverter.GetBytes(value[num9 + num11].ToFloat()), 0, (Array) dst, index5, num8);
              break;
            case TiffType.SBYTE:
              dst[index5] = value[num9 + num11].ToByte();
              break;
            case TiffType.SSHORT:
              Buffer.BlockCopy((Array) BitConverter.GetBytes(value[num9 + num11].ToShort()), 0, (Array) dst, index5, num8);
              break;
            case TiffType.SLONG:
              Buffer.BlockCopy((Array) BitConverter.GetBytes(value[num9 + num11].ToInt()), 0, (Array) dst, index5, num8);
              break;
            case TiffType.DOUBLE:
              Buffer.BlockCopy((Array) BitConverter.GetBytes(value[num9 + num11].ToDouble()), 0, (Array) dst, index5, num8);
              break;
            default:
              Array.Clear((Array) dst, index5, num8);
              flag1 = false;
              break;
          }
          ++num11;
          index5 += num8;
        }
        break;
    }
    if (!flag2 && !flag3 && !flag4 && flag1)
    {
      tif.setFieldBit((int) tif.FieldWithTag(tag).Bit);
      tif.m_flags |= TiffFlags.DIRTYDIRECT;
    }
    return !flag3 && !flag4 && flag1;
  }

  public virtual FieldValue[] GetField(Tiff tif, TiffTag tag)
  {
    TiffDirectory dir = tif.m_dir;
    FieldValue[] field = (FieldValue[]) null;
    switch (tag)
    {
      case TiffTag.SUBFILETYPE:
        field = new FieldValue[1];
        field[0].Set((object) dir.td_subfiletype);
        break;
      case TiffTag.IMAGEWIDTH:
        field = new FieldValue[1];
        field[0].Set((object) dir.td_imagewidth);
        break;
      case TiffTag.IMAGELENGTH:
        field = new FieldValue[1];
        field[0].Set((object) dir.td_imagelength);
        break;
      case TiffTag.BITSPERSAMPLE:
        field = new FieldValue[1];
        field[0].Set((object) dir.td_bitspersample);
        break;
      case TiffTag.COMPRESSION:
        field = new FieldValue[1];
        field[0].Set((object) dir.td_compression);
        break;
      case TiffTag.PHOTOMETRIC:
        field = new FieldValue[1];
        field[0].Set((object) dir.td_photometric);
        break;
      case TiffTag.THRESHHOLDING:
        field = new FieldValue[1];
        field[0].Set((object) dir.td_threshholding);
        break;
      case TiffTag.FILLORDER:
        field = new FieldValue[1];
        field[0].Set((object) dir.td_fillorder);
        break;
      case TiffTag.STRIPOFFSETS:
      case TiffTag.TILEOFFSETS:
        field = new FieldValue[1];
        field[0].Set((object) dir.td_stripoffset);
        break;
      case TiffTag.ORIENTATION:
        field = new FieldValue[1];
        field[0].Set((object) dir.td_orientation);
        break;
      case TiffTag.SAMPLESPERPIXEL:
        field = new FieldValue[1];
        field[0].Set((object) dir.td_samplesperpixel);
        break;
      case TiffTag.ROWSPERSTRIP:
        field = new FieldValue[1];
        field[0].Set((object) dir.td_rowsperstrip);
        break;
      case TiffTag.STRIPBYTECOUNTS:
      case TiffTag.TILEBYTECOUNTS:
        field = new FieldValue[1];
        field[0].Set((object) dir.td_stripbytecount);
        break;
      case TiffTag.MINSAMPLEVALUE:
        field = new FieldValue[1];
        field[0].Set((object) dir.td_minsamplevalue);
        break;
      case TiffTag.MAXSAMPLEVALUE:
        field = new FieldValue[1];
        field[0].Set((object) dir.td_maxsamplevalue);
        break;
      case TiffTag.XRESOLUTION:
        field = new FieldValue[1];
        field[0].Set((object) dir.td_xresolution);
        break;
      case TiffTag.YRESOLUTION:
        field = new FieldValue[1];
        field[0].Set((object) dir.td_yresolution);
        break;
      case TiffTag.PLANARCONFIG:
        field = new FieldValue[1];
        field[0].Set((object) dir.td_planarconfig);
        break;
      case TiffTag.XPOSITION:
        field = new FieldValue[1];
        field[0].Set((object) dir.td_xposition);
        break;
      case TiffTag.YPOSITION:
        field = new FieldValue[1];
        field[0].Set((object) dir.td_yposition);
        break;
      case TiffTag.RESOLUTIONUNIT:
        field = new FieldValue[1];
        field[0].Set((object) dir.td_resolutionunit);
        break;
      case TiffTag.PAGENUMBER:
        field = new FieldValue[2];
        field[0].Set((object) dir.td_pagenumber[0]);
        field[1].Set((object) dir.td_pagenumber[1]);
        break;
      case TiffTag.TRANSFERFUNCTION:
        field = new FieldValue[3];
        field[0].Set((object) dir.td_transferfunction[0]);
        if ((int) dir.td_samplesperpixel - (int) dir.td_extrasamples > 1)
        {
          field[1].Set((object) dir.td_transferfunction[1]);
          field[2].Set((object) dir.td_transferfunction[2]);
          break;
        }
        break;
      case TiffTag.COLORMAP:
        field = new FieldValue[3];
        field[0].Set((object) dir.td_colormap[0]);
        field[1].Set((object) dir.td_colormap[1]);
        field[2].Set((object) dir.td_colormap[2]);
        break;
      case TiffTag.HALFTONEHINTS:
        field = new FieldValue[2];
        field[0].Set((object) dir.td_halftonehints[0]);
        field[1].Set((object) dir.td_halftonehints[1]);
        break;
      case TiffTag.TILEWIDTH:
        field = new FieldValue[1];
        field[0].Set((object) dir.td_tilewidth);
        break;
      case TiffTag.TILELENGTH:
        field = new FieldValue[1];
        field[0].Set((object) dir.td_tilelength);
        break;
      case TiffTag.SUBIFD:
        field = new FieldValue[2];
        field[0].Set((object) dir.td_nsubifd);
        field[1].Set((object) dir.td_subifd);
        break;
      case TiffTag.INKNAMES:
        field = new FieldValue[1];
        field[0].Set((object) dir.td_inknames);
        break;
      case TiffTag.EXTRASAMPLES:
        field = new FieldValue[2];
        field[0].Set((object) dir.td_extrasamples);
        field[1].Set((object) dir.td_sampleinfo);
        break;
      case TiffTag.SAMPLEFORMAT:
        field = new FieldValue[1];
        field[0].Set((object) dir.td_sampleformat);
        break;
      case TiffTag.SMINSAMPLEVALUE:
        field = new FieldValue[1];
        field[0].Set((object) dir.td_sminsamplevalue);
        break;
      case TiffTag.SMAXSAMPLEVALUE:
        field = new FieldValue[1];
        field[0].Set((object) dir.td_smaxsamplevalue);
        break;
      case TiffTag.YCBCRSUBSAMPLING:
        field = new FieldValue[2];
        field[0].Set((object) dir.td_ycbcrsubsampling[0]);
        field[1].Set((object) dir.td_ycbcrsubsampling[1]);
        break;
      case TiffTag.YCBCRPOSITIONING:
        field = new FieldValue[1];
        field[0].Set((object) dir.td_ycbcrpositioning);
        break;
      case TiffTag.REFERENCEBLACKWHITE:
        if (dir.td_refblackwhite != null)
        {
          field = new FieldValue[1];
          field[0].Set((object) dir.td_refblackwhite);
          break;
        }
        break;
      case TiffTag.MATTEING:
        field = new FieldValue[1];
        field[0].Set((object) (bool) (dir.td_extrasamples != (short) 1 ? 0 : (dir.td_sampleinfo[0] == ExtraSample.ASSOCALPHA ? 1 : 0)));
        break;
      case TiffTag.DATATYPE:
        switch (dir.td_sampleformat)
        {
          case SampleFormat.UINT:
            field = new FieldValue[1];
            field[0].Set((object) (short) 2);
            break;
          case SampleFormat.INT:
            field = new FieldValue[1];
            field[0].Set((object) (short) 1);
            break;
          case SampleFormat.IEEEFP:
            field = new FieldValue[1];
            field[0].Set((object) (short) 3);
            break;
          case SampleFormat.VOID:
            field = new FieldValue[1];
            field[0].Set((object) (short) 0);
            break;
        }
        break;
      case TiffTag.IMAGEDEPTH:
        field = new FieldValue[1];
        field[0].Set((object) dir.td_imagedepth);
        break;
      case TiffTag.TILEDEPTH:
        field = new FieldValue[1];
        field[0].Set((object) dir.td_tiledepth);
        break;
      default:
        TiffFieldInfo fieldInfo = tif.FindFieldInfo(tag, TiffType.NOTYPE);
        if (fieldInfo == null || fieldInfo.Bit != (short) 65)
        {
          field = (FieldValue[]) null;
          break;
        }
        field = (FieldValue[]) null;
        for (int index1 = 0; index1 < dir.td_customValueCount; ++index1)
        {
          TiffTagValue tdCustomValue = dir.td_customValues[index1];
          if (tdCustomValue.info.Tag == tag)
          {
            if (fieldInfo.PassCount)
            {
              field = new FieldValue[2];
              if (fieldInfo.ReadCount == (short) -3)
                field[0].Set((object) tdCustomValue.count);
              else
                field[0].Set((object) tdCustomValue.count);
              field[1].Set((object) tdCustomValue.value);
              break;
            }
            if ((fieldInfo.Type == TiffType.ASCII || fieldInfo.ReadCount == (short) -1 || fieldInfo.ReadCount == (short) -3 || fieldInfo.ReadCount == (short) -2 || tdCustomValue.count > 1) && fieldInfo.Tag != TiffTag.PAGENUMBER && fieldInfo.Tag != TiffTag.HALFTONEHINTS && fieldInfo.Tag != TiffTag.YCBCRSUBSAMPLING && fieldInfo.Tag != TiffTag.DOTRANGE)
            {
              field = new FieldValue[1];
              byte[] numArray = tdCustomValue.value;
              if (fieldInfo.Type == TiffType.ASCII && tdCustomValue.value.Length > 0 && tdCustomValue.value[tdCustomValue.value.Length - 1] == (byte) 0)
              {
                numArray = new byte[Math.Max(tdCustomValue.value.Length - 1, 0)];
                Buffer.BlockCopy((Array) tdCustomValue.value, 0, (Array) numArray, 0, numArray.Length);
              }
              field[0].Set((object) numArray);
              break;
            }
            field = new FieldValue[tdCustomValue.count];
            byte[] numArray1 = tdCustomValue.value;
            int startIndex = 0;
            int index2 = 0;
            while (index2 < tdCustomValue.count)
            {
              switch (fieldInfo.Type)
              {
                case TiffType.BYTE:
                case TiffType.SBYTE:
                case TiffType.UNDEFINED:
                  field[index2].Set((object) numArray1[startIndex]);
                  break;
                case TiffType.SHORT:
                case TiffType.SSHORT:
                  field[index2].Set((object) BitConverter.ToInt16(numArray1, startIndex));
                  break;
                case TiffType.LONG:
                case TiffType.SLONG:
                case TiffType.IFD:
                  field[index2].Set((object) BitConverter.ToInt32(numArray1, startIndex));
                  break;
                case TiffType.RATIONAL:
                case TiffType.SRATIONAL:
                case TiffType.FLOAT:
                  field[index2].Set((object) BitConverter.ToSingle(numArray1, startIndex));
                  break;
                case TiffType.DOUBLE:
                  field[index2].Set((object) BitConverter.ToDouble(numArray1, startIndex));
                  break;
                default:
                  field = (FieldValue[]) null;
                  break;
              }
              ++index2;
              startIndex += Tiff.dataSize(tdCustomValue.info.Type);
            }
            break;
          }
        }
        break;
    }
    return field;
  }

  private static bool setExtraSamples(TiffDirectory td, ref int v, FieldValue[] ap)
  {
    v = ap[0].ToInt();
    if (v > (int) td.td_samplesperpixel)
      return false;
    byte[] byteArray = ap[1].ToByteArray();
    if (v > 0 && byteArray == null)
      return false;
    for (int startIndex = 0; startIndex < v; ++startIndex)
    {
      if (byteArray[startIndex] > (byte) 2)
      {
        if (startIndex >= v - 1)
          return false;
        if (BitConverter.ToInt16(byteArray, startIndex) == (short) 999)
          byteArray[startIndex] = (byte) 2;
      }
    }
    td.td_extrasamples = (short) v;
    td.td_sampleinfo = new ExtraSample[(int) td.td_extrasamples];
    for (int index = 0; index < (int) td.td_extrasamples; ++index)
      td.td_sampleinfo[index] = (ExtraSample) byteArray[index];
    return true;
  }

  private static int checkInkNamesString(Tiff tif, int slen, string s)
  {
    bool flag = false;
    short tdSamplesperpixel = tif.m_dir.td_samplesperpixel;
    if (slen > 0)
    {
      int num = slen;
      int index = 0;
      for (; tdSamplesperpixel > (short) 0; --tdSamplesperpixel)
      {
        for (; s[index] != char.MinValue; ++index)
        {
          if (index >= num)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          ++index;
        else
          break;
      }
      if (!flag)
        return index;
    }
    return 0;
  }

  private static void setNString(out string cpp, string cp, int n) => cpp = cp.Substring(0, n);
}
