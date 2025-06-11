// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.CodecWithPredictorTagMethods
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class CodecWithPredictorTagMethods : TiffTagMethods
{
  public override bool SetField(Tiff tif, Syncfusion.Pdf.Compression.JBIG2.TiffTag tag, FieldValue[] ap)
  {
    CodecWithPredictor currentCodec = tif.m_currentCodec as CodecWithPredictor;
    if (tag == Syncfusion.Pdf.Compression.JBIG2.TiffTag.PREDICTOR)
    {
      currentCodec.SetPredictorValue((Predictor) ap[0].ToByte());
      tif.setFieldBit(66);
      tif.m_flags |= TiffFlags.DIRTYDIRECT;
      return true;
    }
    TiffTagMethods childTagMethods = currentCodec.GetChildTagMethods();
    return childTagMethods != null ? childTagMethods.SetField(tif, tag, ap) : base.SetField(tif, tag, ap);
  }

  public override FieldValue[] GetField(Tiff tif, Syncfusion.Pdf.Compression.JBIG2.TiffTag tag)
  {
    CodecWithPredictor currentCodec = tif.m_currentCodec as CodecWithPredictor;
    if (tag == Syncfusion.Pdf.Compression.JBIG2.TiffTag.PREDICTOR)
    {
      FieldValue[] field = new FieldValue[1];
      field[0].Set((object) currentCodec.GetPredictorValue());
      return field;
    }
    TiffTagMethods childTagMethods = currentCodec.GetChildTagMethods();
    return childTagMethods != null ? childTagMethods.GetField(tif, tag) : base.GetField(tif, tag);
  }
}
