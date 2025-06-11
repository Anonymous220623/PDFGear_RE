// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.EntropyDecoder
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal abstract class EntropyDecoder : 
  MultiResImgDataAdapter,
  CBlkQuantDataSrcDec,
  InvWTData,
  MultiResImgData
{
  public const char OPT_PREFIX = 'C';
  private static readonly string[][] pinfo = new string[2][]
  {
    new string[4]
    {
      "Cverber",
      "[on|off]",
      "Specifies if the entropy decoder should be verbose about detected errors. If 'on' a message is printed whenever an error is detected.",
      "on"
    },
    new string[4]
    {
      "Cer",
      "[on|off]",
      "Specifies if error detection should be performed by the entropy decoder engine. If errors are detected they will be concealed and the resulting distortion will be less important. Note that errors can only be detected if the encoder that generated the data included error resilience information.",
      "on"
    }
  };
  internal CodedCBlkDataSrcDec src;

  public virtual int CbULX => this.src.CbULX;

  public virtual int CbULY => this.src.CbULY;

  public static string[][] ParameterInfo => EntropyDecoder.pinfo;

  internal EntropyDecoder(CodedCBlkDataSrcDec src)
    : base((MultiResImgData) src)
  {
    this.src = src;
  }

  public override SubbandSyn getSynSubbandTree(int t, int c)
  {
    return ((InvWTData) this.src).getSynSubbandTree(t, c);
  }

  public abstract DataBlock getCodeBlock(
    int param1,
    int param2,
    int param3,
    SubbandSyn param4,
    DataBlock param5);

  public abstract DataBlock getInternCodeBlock(
    int param1,
    int param2,
    int param3,
    SubbandSyn param4,
    DataBlock param5);
}
