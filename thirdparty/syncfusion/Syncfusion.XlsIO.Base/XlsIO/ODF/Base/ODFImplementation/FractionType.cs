// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.FractionType
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

internal class FractionType : CommonType
{
  private int m_minDenominatorDigits;
  private int m_minNumeratorDigits;
  private int m_denominatorValue;

  internal int MinDenominatorDigits
  {
    get => this.m_minDenominatorDigits;
    set => this.m_minDenominatorDigits = value;
  }

  internal int MinNumeratorDigits
  {
    get => this.m_minNumeratorDigits;
    set => this.m_minNumeratorDigits = value;
  }

  internal int DenominatorValue
  {
    get => this.m_denominatorValue;
    set => this.m_denominatorValue = value;
  }
}
