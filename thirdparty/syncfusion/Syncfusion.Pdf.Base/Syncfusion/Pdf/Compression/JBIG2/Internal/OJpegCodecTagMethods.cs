// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.OJpegCodecTagMethods
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class OJpegCodecTagMethods : TiffTagMethods
{
  public override bool SetField(Tiff tif, Syncfusion.Pdf.Compression.JBIG2.TiffTag tag, FieldValue[] ap)
  {
    OJpegCodec currentCodec = tif.m_currentCodec as OJpegCodec;
    switch (tag)
    {
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGPROC:
        currentCodec.m_jpeg_proc = ap[0].ToByte();
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGIFOFFSET:
        currentCodec.m_jpeg_interchange_format = ap[0].ToUInt();
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGIFBYTECOUNT:
        currentCodec.m_jpeg_interchange_format_length = ap[0].ToUInt();
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGRESTARTINTERVAL:
        currentCodec.m_restart_interval = ap[0].ToUShort();
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGQTABLES:
        uint num1 = ap[0].ToUInt();
        switch (num1)
        {
          case 0:
            break;
          case 1:
          case 2:
          case 3:
            currentCodec.m_qtable_offset_count = (byte) num1;
            uint[] uintArray1 = ap[1].ToUIntArray();
            for (uint index = 0; index < num1; ++index)
              currentCodec.m_qtable_offset[(IntPtr) index] = uintArray1[(IntPtr) index];
            break;
          default:
            return false;
        }
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGDCTABLES:
        uint num2 = ap[0].ToUInt();
        switch (num2)
        {
          case 0:
            break;
          case 1:
          case 2:
          case 3:
            currentCodec.m_dctable_offset_count = (byte) num2;
            uint[] uintArray2 = ap[1].ToUIntArray();
            for (uint index = 0; index < num2; ++index)
              currentCodec.m_dctable_offset[(IntPtr) index] = uintArray2[(IntPtr) index];
            break;
          default:
            return false;
        }
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGACTABLES:
        uint num3 = ap[0].ToUInt();
        switch (num3)
        {
          case 0:
            break;
          case 1:
          case 2:
          case 3:
            currentCodec.m_actable_offset_count = (byte) num3;
            uint[] uintArray3 = ap[1].ToUIntArray();
            for (uint index = 0; index < num3; ++index)
              currentCodec.m_actable_offset[(IntPtr) index] = uintArray3[(IntPtr) index];
            break;
          default:
            return false;
        }
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.YCBCRSUBSAMPLING:
        currentCodec.m_subsampling_tag = true;
        currentCodec.m_subsampling_hor = ap[0].ToByte();
        currentCodec.m_subsampling_ver = ap[1].ToByte();
        tif.m_dir.td_ycbcrsubsampling[0] = (short) currentCodec.m_subsampling_hor;
        tif.m_dir.td_ycbcrsubsampling[1] = (short) currentCodec.m_subsampling_ver;
        break;
      default:
        return base.SetField(tif, tag, ap);
    }
    TiffFieldInfo tiffFieldInfo = tif.FieldWithTag(tag);
    if (tiffFieldInfo == null)
      return false;
    tif.setFieldBit((int) tiffFieldInfo.Bit);
    tif.m_flags |= TiffFlags.DIRTYDIRECT;
    return true;
  }

  public override FieldValue[] GetField(Tiff tif, Syncfusion.Pdf.Compression.JBIG2.TiffTag tag)
  {
    OJpegCodec currentCodec = tif.m_currentCodec as OJpegCodec;
    FieldValue[] field;
    switch (tag)
    {
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGPROC:
        field = new FieldValue[1];
        field[0].Set((object) currentCodec.m_jpeg_proc);
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGIFOFFSET:
        field = new FieldValue[1];
        field[0].Set((object) currentCodec.m_jpeg_interchange_format);
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGIFBYTECOUNT:
        field = new FieldValue[1];
        field[0].Set((object) currentCodec.m_jpeg_interchange_format_length);
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGRESTARTINTERVAL:
        field = new FieldValue[1];
        field[0].Set((object) currentCodec.m_restart_interval);
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGQTABLES:
        field = new FieldValue[2];
        field[0].Set((object) currentCodec.m_qtable_offset_count);
        field[1].Set((object) currentCodec.m_qtable_offset);
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGDCTABLES:
        field = new FieldValue[2];
        field[0].Set((object) currentCodec.m_dctable_offset_count);
        field[1].Set((object) currentCodec.m_dctable_offset);
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGACTABLES:
        field = new FieldValue[2];
        field[0].Set((object) currentCodec.m_actable_offset_count);
        field[1].Set((object) currentCodec.m_actable_offset);
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.YCBCRSUBSAMPLING:
        if (!currentCodec.m_subsamplingcorrect_done)
          currentCodec.OJPEGSubsamplingCorrect();
        field = new FieldValue[2];
        field[0].Set((object) currentCodec.m_subsampling_hor);
        field[1].Set((object) currentCodec.m_subsampling_ver);
        break;
      default:
        return base.GetField(tif, tag);
    }
    return field;
  }
}
