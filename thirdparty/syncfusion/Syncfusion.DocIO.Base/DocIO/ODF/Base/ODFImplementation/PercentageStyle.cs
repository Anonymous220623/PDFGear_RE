// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.PercentageStyle
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class PercentageStyle : DataStyle
{
  private NumberType m_number;

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

  internal PercentageStyle()
  {
  }

  internal void Dispose()
  {
    if (this.m_number == null)
      return;
    this.m_number.Dispose();
  }
}
