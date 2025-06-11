// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.PercentageStyle
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

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
