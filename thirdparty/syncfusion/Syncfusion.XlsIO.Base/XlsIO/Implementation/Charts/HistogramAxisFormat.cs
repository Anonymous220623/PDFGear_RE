// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Charts.HistogramAxisFormat
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Charts;

internal class HistogramAxisFormat
{
  private byte m_flagOptions;
  private int m_numberOfBins;
  private double m_binWidth;
  private double m_overflowBinValue;
  private double m_underflowBinValue;
  private bool m_isNotAutomaticUnderFlowValue;

  internal bool HasAutomaticBins
  {
    get => ((int) this.m_flagOptions & 15) == 0 || ((int) this.m_flagOptions & 1) == 1;
    set
    {
      if (value)
        this.ResetValues((byte) 0);
      else
        this.m_flagOptions &= (byte) 254;
    }
  }

  internal bool IsBinningByCategory
  {
    get => ((int) this.m_flagOptions & 2) == 2;
    set
    {
      if (value)
        this.ResetValues((byte) 1);
      else
        this.m_flagOptions &= (byte) 253;
    }
  }

  internal double BinWidth
  {
    get => this.m_binWidth;
    set
    {
      if (value <= 0.0)
        return;
      this.ResetValues((byte) 2);
      this.m_binWidth = value;
    }
  }

  internal int NumberOfBins
  {
    get => this.m_numberOfBins;
    set
    {
      if (value <= 0)
        return;
      this.ResetValues((byte) 3);
      this.m_numberOfBins = value;
    }
  }

  internal double OverflowBinValue
  {
    get => this.m_overflowBinValue;
    set
    {
      this.m_flagOptions |= (byte) 16 /*0x10*/;
      this.m_overflowBinValue = value;
      this.IsNotAutomaticOverFlowValue = true;
    }
  }

  internal double UnderflowBinValue
  {
    get => this.m_underflowBinValue;
    set
    {
      this.m_flagOptions |= (byte) 32 /*0x20*/;
      this.m_underflowBinValue = value;
      this.IsNotAutomaticUnderFlowValue = true;
    }
  }

  internal bool IsIntervalClosedinLeft
  {
    get => ((int) this.m_flagOptions & 64 /*0x40*/) == 64 /*0x40*/;
    set
    {
      if (value)
        this.m_flagOptions |= (byte) 64 /*0x40*/;
      else
        this.m_flagOptions &= (byte) 191;
    }
  }

  internal byte FlagOptions => this.m_flagOptions;

  internal bool IsNotAutomaticOverFlowValue
  {
    get => ((int) this.m_flagOptions & 128 /*0x80*/) == 128 /*0x80*/;
    set
    {
      if (value)
        this.m_flagOptions |= (byte) 128 /*0x80*/;
      else
        this.m_flagOptions &= (byte) 127 /*0x7F*/;
    }
  }

  internal bool IsNotAutomaticUnderFlowValue
  {
    get => this.m_isNotAutomaticUnderFlowValue;
    set => this.m_isNotAutomaticUnderFlowValue = value;
  }

  private void ResetValues(byte bitPosition)
  {
    this.m_flagOptions &= (byte) 240 /*0xF0*/;
    this.m_flagOptions |= (byte) (1U << (int) bitPosition);
    if (bitPosition != (byte) 2)
    {
      this.m_binWidth = 0.0;
    }
    else
    {
      if (bitPosition == (byte) 3)
        return;
      this.m_numberOfBins = 0;
    }
  }

  internal void Clone(HistogramAxisFormat inputFormat)
  {
    this.m_flagOptions = inputFormat.m_flagOptions;
    this.m_binWidth = inputFormat.m_binWidth;
    this.m_numberOfBins = inputFormat.m_numberOfBins;
    this.m_overflowBinValue = inputFormat.m_overflowBinValue;
    this.m_underflowBinValue = inputFormat.m_underflowBinValue;
    this.m_isNotAutomaticUnderFlowValue = inputFormat.m_isNotAutomaticUnderFlowValue;
  }

  public override bool Equals(object obj)
  {
    HistogramAxisFormat histogramAxisFormat = obj as HistogramAxisFormat;
    return (int) this.m_flagOptions == (int) histogramAxisFormat.m_flagOptions && this.m_binWidth == histogramAxisFormat.m_binWidth && this.m_numberOfBins == histogramAxisFormat.m_numberOfBins && this.m_overflowBinValue == histogramAxisFormat.m_overflowBinValue && this.m_underflowBinValue == histogramAxisFormat.m_underflowBinValue && this.m_isNotAutomaticUnderFlowValue == histogramAxisFormat.m_isNotAutomaticUnderFlowValue;
  }
}
