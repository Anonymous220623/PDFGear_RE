// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.CCITTCodecTagMethods
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class CCITTCodecTagMethods : TiffTagMethods
{
  public override bool SetField(Tiff tif, Syncfusion.Pdf.Compression.JBIG2.TiffTag tag, FieldValue[] ap)
  {
    CCITTCodec currentCodec = tif.m_currentCodec as CCITTCodec;
    switch (tag)
    {
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.GROUP3OPTIONS:
        if (tif.m_dir.td_compression == Syncfusion.Pdf.Compression.JBIG2.Compression.CCITTFAX3)
        {
          currentCodec.m_groupoptions = (Group3Opt) ap[0].ToShort();
          break;
        }
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.GROUP4OPTIONS:
        if (tif.m_dir.td_compression == Syncfusion.Pdf.Compression.JBIG2.Compression.CCITTFAX4)
        {
          currentCodec.m_groupoptions = (Group3Opt) ap[0].ToShort();
          break;
        }
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.BADFAXLINES:
        currentCodec.m_badfaxlines = ap[0].ToInt();
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.CLEANFAXDATA:
        currentCodec.m_cleanfaxdata = (CleanFaxData) ap[0].ToByte();
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.CONSECUTIVEBADFAXLINES:
        currentCodec.m_badfaxrun = ap[0].ToInt();
        break;
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
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.FAXMODE:
        currentCodec.m_mode = (FaxMode) ap[0].ToShort();
        return true;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.FAXFILLFUNC:
        currentCodec.fill = ap[0].Value as Tiff.FaxFillFunc;
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
    CCITTCodec currentCodec = tif.m_currentCodec as CCITTCodec;
    FieldValue[] field = new FieldValue[1];
    switch (tag)
    {
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.GROUP3OPTIONS:
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.GROUP4OPTIONS:
        field[0].Set((object) currentCodec.m_groupoptions);
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.BADFAXLINES:
        field[0].Set((object) currentCodec.m_badfaxlines);
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.CLEANFAXDATA:
        field[0].Set((object) currentCodec.m_cleanfaxdata);
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.CONSECUTIVEBADFAXLINES:
        field[0].Set((object) currentCodec.m_badfaxrun);
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.FAXRECVPARAMS:
        field[0].Set((object) currentCodec.m_recvparams);
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.FAXSUBADDRESS:
        field[0].Set((object) currentCodec.m_subaddress);
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.FAXRECVTIME:
        field[0].Set((object) currentCodec.m_recvtime);
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.FAXDCS:
        field[0].Set((object) currentCodec.m_faxdcs);
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.FAXMODE:
        field[0].Set((object) currentCodec.m_mode);
        break;
      case Syncfusion.Pdf.Compression.JBIG2.TiffTag.FAXFILLFUNC:
        field[0].Set((object) currentCodec.fill);
        break;
      default:
        return base.GetField(tif, tag);
    }
    return field;
  }
}
