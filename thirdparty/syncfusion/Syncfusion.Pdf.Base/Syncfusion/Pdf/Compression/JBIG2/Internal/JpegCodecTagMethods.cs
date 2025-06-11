// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.JpegCodecTagMethods
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class JpegCodecTagMethods : TiffTagMethods
{
  public override bool SetField(Tiff tif, Syncfusion.Pdf.Compression.JBIG2.TiffTag tag, FieldValue[] ap)
  {
    JpegCodec currentCodec = tif.m_currentCodec as JpegCodec;
    switch (tag)
    {
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.PHOTOMETRIC:
        bool flag = base.SetField(tif, tag, ap);
        currentCodec.JPEGResetUpsampled();
        return flag;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGTABLES:
        int count = ap[0].ToInt();
        if (count == 0)
          return false;
        currentCodec.m_jpegtables = new byte[count];
        Buffer.BlockCopy((Array) ap[1].ToByteArray(), 0, (Array) currentCodec.m_jpegtables, 0, count);
        currentCodec.m_jpegtables_length = count;
        tif.setFieldBit(66);
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.YCBCRSUBSAMPLING:
        currentCodec.m_ycbcrsampling_fetched = true;
        return base.SetField(tif, tag, ap);
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.FAXRECVPARAMS:
        currentCodec.m_recvparams = ap[0].ToInt();
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.FAXSUBADDRESS:
        Tiff.setString(out currentCodec.m_subaddress, ap[0].ToString());
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.FAXRECVTIME:
        currentCodec.m_recvtime = ap[0].ToInt();
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.FAXDCS:
        Tiff.setString(out currentCodec.m_faxdcs, ap[0].ToString());
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGQUALITY:
        currentCodec.m_jpegquality = ap[0].ToInt();
        return true;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGCOLORMODE:
        currentCodec.m_jpegcolormode = (JpegColorMode) ap[0].ToShort();
        currentCodec.JPEGResetUpsampled();
        return true;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGTABLESMODE:
        currentCodec.m_jpegtablesmode = (JpegTablesMode) ap[0].ToShort();
        return true;
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
    JpegCodec currentCodec = tif.m_currentCodec as JpegCodec;
    FieldValue[] field;
    switch (tag)
    {
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGTABLES:
        field = new FieldValue[2];
        field[0].Set((object) currentCodec.m_jpegtables_length);
        field[1].Set((object) currentCodec.m_jpegtables);
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.YCBCRSUBSAMPLING:
        JpegCodecTagMethods.JPEGFixupTestSubsampling(tif);
        return base.GetField(tif, tag);
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.FAXRECVPARAMS:
        field = new FieldValue[1];
        field[0].Set((object) currentCodec.m_recvparams);
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.FAXSUBADDRESS:
        field = new FieldValue[1];
        field[0].Set((object) currentCodec.m_subaddress);
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.FAXRECVTIME:
        field = new FieldValue[1];
        field[0].Set((object) currentCodec.m_recvtime);
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.FAXDCS:
        field = new FieldValue[1];
        field[0].Set((object) currentCodec.m_faxdcs);
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGQUALITY:
        field = new FieldValue[1];
        field[0].Set((object) currentCodec.m_jpegquality);
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGCOLORMODE:
        field = new FieldValue[1];
        field[0].Set((object) currentCodec.m_jpegcolormode);
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.JPEGTABLESMODE:
        field = new FieldValue[1];
        field[0].Set((object) currentCodec.m_jpegtablesmode);
        break;
      default:
        return base.GetField(tif, tag);
    }
    return field;
  }

  private static void JPEGFixupTestSubsampling(Tiff tif)
  {
    JpegCodec currentCodec = tif.m_currentCodec as JpegCodec;
    currentCodec.InitializeJpeg(false, false);
    if (!currentCodec.m_common.IsDecompressor || currentCodec.m_ycbcrsampling_fetched || tif.m_dir.td_photometric != Photometric.YCBCR)
      return;
    currentCodec.m_ycbcrsampling_fetched = true;
    if (tif.IsTiled())
    {
      if (!tif.fillTile(0))
        return;
    }
    else if (!tif.fillStrip(0))
      return;
    tif.SetField(Syncfusion.Pdf.Compression.JBIG2.TiffTag.YCBCRSUBSAMPLING, (object) currentCodec.m_h_sampling, (object) currentCodec.m_v_sampling);
    tif.m_curstrip = -1;
  }
}
