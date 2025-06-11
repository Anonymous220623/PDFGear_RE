// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Dequantizer
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal abstract class Dequantizer : 
  MultiResImgDataAdapter,
  CBlkWTDataSrcDec,
  InvWTData,
  MultiResImgData
{
  public const char OPT_PREFIX = 'Q';
  private static readonly string[][] pinfo;
  internal CBlkQuantDataSrcDec src;
  internal int[] rb;
  internal int[] utrb;
  private CompTransfSpec cts;
  private SynWTFilterSpec wfs;

  public virtual int CbULX => this.src.CbULX;

  public virtual int CbULY => this.src.CbULY;

  public static string[][] ParameterInfo => Dequantizer.pinfo;

  internal Dequantizer(CBlkQuantDataSrcDec src, int[] utrb, DecodeHelper decSpec)
    : base((MultiResImgData) src)
  {
    if (utrb.Length != src.NumComps)
      throw new ArgumentException();
    this.src = src;
    this.utrb = utrb;
    this.cts = decSpec.cts;
    this.wfs = decSpec.wfs;
  }

  public virtual int getNomRangeBits(int c) => this.rb[c];

  public override SubbandSyn getSynSubbandTree(int t, int c)
  {
    return ((InvWTData) this.src).getSynSubbandTree(t, c);
  }

  public override void setTile(int x, int y)
  {
    this.src.setTile(x, y);
    this.tIdx = this.TileIdx;
    int num1;
    if ((int) this.cts.getTileDef(this.tIdx) == 0)
    {
      num1 = 0;
    }
    else
    {
      int num2 = this.src.NumComps > 3 ? 3 : this.src.NumComps;
      int num3 = 0;
      for (int c = 0; c < num2; ++c)
        num3 += this.wfs.isReversible(this.tIdx, c) ? 1 : 0;
      if (num3 == 3)
      {
        num1 = 1;
      }
      else
      {
        if (num3 != 0)
          throw new ArgumentException("Wavelet transformation and component transformation not coherent in tile" + (object) this.tIdx);
        num1 = 2;
      }
    }
    switch (num1)
    {
      case 0:
        this.rb = this.utrb;
        break;
      case 1:
        this.rb = InverseComponetTransformation.calcMixedBitDepths(this.utrb, 1, (int[]) null);
        break;
      case 2:
        this.rb = InverseComponetTransformation.calcMixedBitDepths(this.utrb, 2, (int[]) null);
        break;
      default:
        throw new ArgumentException("Non JPEG 2000 part I component transformation for tile: " + (object) this.tIdx);
    }
  }

  public override void nextTile()
  {
    this.src.nextTile();
    this.tIdx = this.TileIdx;
    switch ((int) this.cts.getTileDef(this.tIdx))
    {
      case 0:
        this.rb = this.utrb;
        break;
      case 1:
        this.rb = InverseComponetTransformation.calcMixedBitDepths(this.utrb, 1, (int[]) null);
        break;
      case 2:
        this.rb = InverseComponetTransformation.calcMixedBitDepths(this.utrb, 2, (int[]) null);
        break;
      default:
        throw new ArgumentException("Non JPEG 2000 part I component transformation for tile: " + (object) this.tIdx);
    }
  }

  public abstract DataBlock getCodeBlock(
    int param1,
    int param2,
    int param3,
    SubbandSyn param4,
    DataBlock param5);

  public abstract int getFixedPoint(int param1);

  public abstract DataBlock getInternCodeBlock(
    int param1,
    int param2,
    int param3,
    SubbandSyn param4,
    DataBlock param5);
}
