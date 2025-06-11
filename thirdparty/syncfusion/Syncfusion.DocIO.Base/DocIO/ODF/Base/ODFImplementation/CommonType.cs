// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.CommonType
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class CommonType
{
  internal const byte DecimalPlacesKey = 0;
  internal const byte GroupingKey = 1;
  internal const byte MinIntegerDigitsKey = 2;
  private int m_decimalPlaces;
  private bool m_grouping;
  private int m_minIntegerDigits;
  internal byte nFormatFlags;

  internal int DecimalPlaces
  {
    get => this.m_decimalPlaces;
    set
    {
      this.nFormatFlags = (byte) ((int) this.nFormatFlags & 254 | 1);
      this.m_decimalPlaces = value;
    }
  }

  internal bool Grouping
  {
    get => this.m_grouping;
    set
    {
      this.nFormatFlags = (byte) ((int) this.nFormatFlags & 253 | 2);
      this.m_grouping = value;
    }
  }

  internal int MinIntegerDigits
  {
    get => this.m_minIntegerDigits;
    set
    {
      this.nFormatFlags = (byte) ((int) this.nFormatFlags & 251 | 4);
      this.m_minIntegerDigits = value;
    }
  }
}
