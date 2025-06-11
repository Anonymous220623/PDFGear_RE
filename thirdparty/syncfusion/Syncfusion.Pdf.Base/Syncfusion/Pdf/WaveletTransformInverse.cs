// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.WaveletTransformInverse
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal abstract class WaveletTransformInverse : InvWTAdapter, BlockImageDataSource, ImageData
{
  internal WaveletTransformInverse(MultiResImgData src, DecodeHelper decSpec)
    : base(src, decSpec)
  {
  }

  internal static WaveletTransformInverse createInstance(CBlkWTDataSrcDec src, DecodeHelper decSpec)
  {
    return (WaveletTransformInverse) new InvWTFull(src, decSpec);
  }

  public abstract int getFixedPoint(int param1);

  public abstract DataBlock getInternCompData(DataBlock param1, int param2);

  public abstract DataBlock getCompData(DataBlock param1, int param2);
}
