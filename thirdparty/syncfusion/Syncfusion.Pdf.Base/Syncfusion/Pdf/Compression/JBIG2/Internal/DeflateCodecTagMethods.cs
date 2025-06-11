// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.DeflateCodecTagMethods
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class DeflateCodecTagMethods : TiffTagMethods
{
  public override bool SetField(Tiff tif, Syncfusion.Pdf.Compression.JBIG2.TiffTag tag, FieldValue[] ap)
  {
    DeflateCodec currentCodec = tif.m_currentCodec as DeflateCodec;
    if (tag != Syncfusion.Pdf.Compression.JBIG2.TiffTag.ZIPQUALITY)
      return base.SetField(tif, tag, ap);
    currentCodec.m_zipquality = ap[0].ToInt();
    return (currentCodec.m_state & 2) == 0 || currentCodec.m_stream.deflateParams(currentCodec.m_zipquality, 0) == 0;
  }

  public override FieldValue[] GetField(Tiff tif, Syncfusion.Pdf.Compression.JBIG2.TiffTag tag)
  {
    DeflateCodec currentCodec = tif.m_currentCodec as DeflateCodec;
    if (tag != Syncfusion.Pdf.Compression.JBIG2.TiffTag.ZIPQUALITY)
      return base.GetField(tif, tag);
    FieldValue[] field = new FieldValue[1];
    field[0].Set((object) currentCodec.m_zipquality);
    return field;
  }
}
