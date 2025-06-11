// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.NumberStyle
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class NumberStyle : DataStyle
{
  private NumberType m_number;
  private FractionType m_fraction;
  private ScientificNumberType m_scientificNumber;

  internal NumberType Number
  {
    get
    {
      if (this.m_number == null)
        this.m_number = new NumberType();
      return this.m_number;
    }
    set => this.m_number = value;
  }

  internal FractionType Fraction
  {
    get
    {
      if (this.m_fraction == null)
        this.m_fraction = new FractionType();
      return this.m_fraction;
    }
    set => this.m_fraction = value;
  }

  internal ScientificNumberType ScientificNumber
  {
    get
    {
      if (this.m_scientificNumber == null)
        this.m_scientificNumber = new ScientificNumberType();
      return this.m_scientificNumber;
    }
    set => this.m_scientificNumber = value;
  }

  internal bool HasKey(int propertyKey, int flagname)
  {
    return (flagname & (int) (ushort) Math.Pow(2.0, (double) propertyKey)) >> propertyKey != 0;
  }

  internal void Dispose()
  {
    if (this.m_number != null)
      this.m_number.Dispose();
    if (this.m_fraction != null)
      this.m_fraction = (FractionType) null;
    if (this.m_scientificNumber != null)
      this.m_scientificNumber = (ScientificNumberType) null;
    this.Dispose1();
  }
}
