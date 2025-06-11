// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.CurrencyStyle
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class CurrencyStyle : DataStyle
{
  private CurrencySymbol m_currencySymbol;
  private NumberType m_number;
  private bool m_automaticOrder;

  internal CurrencySymbol CurrencySymbol
  {
    get
    {
      if (this.m_currencySymbol == null)
        this.m_currencySymbol = new CurrencySymbol();
      return this.m_currencySymbol;
    }
    set => this.m_currencySymbol = value;
  }

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

  internal bool AutomaticOrder
  {
    get => this.m_automaticOrder;
    set => this.m_automaticOrder = value;
  }

  public override bool Equals(object obj)
  {
    if (!(obj is CurrencyStyle currencyStyle))
      return false;
    bool flag1 = this.m_currencySymbol.Equals((object) currencyStyle.CurrencySymbol);
    if (!flag1)
      return flag1;
    bool flag2 = this.m_number.Equals((object) currencyStyle.Number);
    return !flag2 ? flag2 : flag2;
  }

  internal void Dispose()
  {
    if (this.m_number != null)
      this.m_number.Dispose();
    if (this.m_currencySymbol == null)
      return;
    this.m_currencySymbol = (CurrencySymbol) null;
  }
}
