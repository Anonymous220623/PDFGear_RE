// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.AnWTFilter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal abstract class AnWTFilter : WaveletFilter
{
  public const char OPT_PREFIX = 'F';
  private static readonly string[][] pinfo = new string[1][]
  {
    new string[4]
    {
      "Ffilters",
      "[<tile-component idx>] <id> [ [<tile-component idx>] <id> ...]",
      "Specifies which filters to use for specified tile-component. If this option is not used, the encoder choses the filters  of the tile-components according to their quantization  type. If this option is used, a component transformation is applied to the three first components.\n<tile-component idx>: see general note\n<id>: ',' separates horizontal and vertical filters, ':' separates decomposition levels filters. JPEG 2000 part 1 only supports w5x3 and w9x7 filters.",
      null
    }
  };

  public abstract int FilterType { get; }

  public static string[][] ParameterInfo => AnWTFilter.pinfo;

  public abstract int AnHighPosSupport { get; }

  public abstract int AnLowNegSupport { get; }

  public abstract int AnLowPosSupport { get; }

  public abstract bool Reversible { get; }

  public abstract int ImplType { get; }

  public abstract int SynHighNegSupport { get; }

  public abstract int SynHighPosSupport { get; }

  public abstract int AnHighNegSupport { get; }

  public abstract int DataType { get; }

  public abstract int SynLowNegSupport { get; }

  public abstract int SynLowPosSupport { get; }

  public abstract void analyze_lpf(
    object inSig,
    int inOff,
    int inLen,
    int inStep,
    object lowSig,
    int lowOff,
    int lowStep,
    object highSig,
    int highOff,
    int highStep);

  public abstract void analyze_hpf(
    object inSig,
    int inOff,
    int inLen,
    int inStep,
    object lowSig,
    int lowOff,
    int lowStep,
    object highSig,
    int highOff,
    int highStep);

  public abstract float[] getLPSynthesisFilter();

  public abstract float[] getHPSynthesisFilter();

  public virtual float[] getLPSynWaveForm(float[] in_Renamed, float[] out_Renamed)
  {
    return AnWTFilter.upsampleAndConvolve(in_Renamed, this.getLPSynthesisFilter(), out_Renamed);
  }

  public virtual float[] getHPSynWaveForm(float[] in_Renamed, float[] out_Renamed)
  {
    return AnWTFilter.upsampleAndConvolve(in_Renamed, this.getHPSynthesisFilter(), out_Renamed);
  }

  private static float[] upsampleAndConvolve(float[] in_Renamed, float[] wf, float[] out_Renamed)
  {
    if (in_Renamed == null)
    {
      in_Renamed = new float[1];
      in_Renamed[0] = 1f;
    }
    if (out_Renamed == null)
      out_Renamed = new float[in_Renamed.Length * 2 + wf.Length - 2];
    int index1 = 0;
    for (int index2 = in_Renamed.Length * 2 + wf.Length - 2; index1 < index2; ++index1)
    {
      float num1 = 0.0f;
      int index3 = (index1 - wf.Length + 2) / 2;
      if (index3 < 0)
        index3 = 0;
      int num2 = index1 / 2 + 1;
      if (num2 > in_Renamed.Length)
        num2 = in_Renamed.Length;
      int index4 = 2 * index3 - index1 + wf.Length - 1;
      while (index3 < num2)
      {
        num1 += in_Renamed[index3] * wf[index4];
        ++index3;
        index4 += 2;
      }
      out_Renamed[index1] = num1;
    }
    return out_Renamed;
  }

  public abstract bool isSameAsFullWT(int param1, int param2, int param3);
}
