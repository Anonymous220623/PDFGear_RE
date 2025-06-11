// Decompiled with JetBrains decompiler
// Type: Syncfusion.Drawing.StyleEntryShapeProperties
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart;

#nullable disable
namespace Syncfusion.Drawing;

internal class StyleEntryShapeProperties
{
  private byte m_flag;
  private OfficeFillType m_shapeFillType;
  private double m_borderWeight;
  private ColorModel m_shapeFillColorModelType;
  private string m_shapeFillColorValue;
  private double m_shapeFillLumModValue = -1.0;
  private double m_shapeFillLumOffValue1 = -1.0;
  private double m_shapeFillLumOffValue2 = -1.0;
  private ColorModel m_borderFillColorModelType;
  private string m_borderFillColorValue;
  private double m_borderFillLumModValue = -1.0;
  private double m_borderFillLumOffValue1 = -1.0;
  private double m_borderFillLumOffValue2 = -1.0;
  private Excel2007ShapeLineStyle m_borderLineStyle;
  private bool m_borderIsRound;
  private EndLineCap m_lineCap;
  private bool m_isInsetPenAlignment;

  internal OfficeFillType ShapeFillType
  {
    get => this.m_shapeFillType;
    set
    {
      this.m_shapeFillType = value;
      this.m_flag |= (byte) 1;
    }
  }

  internal double BorderWeight
  {
    get => this.m_borderWeight;
    set
    {
      this.m_borderWeight = value;
      this.m_flag |= (byte) 4;
      this.m_flag |= (byte) 2;
    }
  }

  internal ColorModel ShapeFillColorModelType
  {
    get => this.m_shapeFillColorModelType;
    set
    {
      this.m_shapeFillColorModelType = value;
      if (value == ColorModel.none)
        return;
      this.ShapeFillType = OfficeFillType.SolidColor;
    }
  }

  internal string ShapeFillColorValue
  {
    get => this.m_shapeFillColorValue;
    set => this.m_shapeFillColorValue = value;
  }

  internal double ShapeFillLumModValue
  {
    get => this.m_shapeFillLumModValue;
    set => this.m_shapeFillLumModValue = value;
  }

  internal double ShapeFillLumOffValue1
  {
    get => this.m_shapeFillLumOffValue1;
    set => this.m_shapeFillLumOffValue1 = value;
  }

  internal double ShapeFillLumOffValue2
  {
    get => this.m_shapeFillLumOffValue2;
    set => this.m_shapeFillLumOffValue2 = value;
  }

  internal ColorModel BorderFillColorModelType
  {
    get => this.m_borderFillColorModelType;
    set
    {
      this.m_borderFillColorModelType = value;
      this.m_flag |= (byte) 2;
    }
  }

  internal string BorderFillColorValue
  {
    get => this.m_borderFillColorValue;
    set
    {
      this.m_borderFillColorValue = value;
      this.m_flag |= (byte) 2;
    }
  }

  internal double BorderFillLumModValue
  {
    get => this.m_borderFillLumModValue;
    set => this.m_borderFillLumModValue = value;
  }

  internal double BorderFillLumOffValue1
  {
    get => this.m_borderFillLumOffValue1;
    set => this.m_borderFillLumOffValue1 = value;
  }

  internal double BorderFillLumOffValue2
  {
    get => this.m_borderFillLumOffValue2;
    set => this.m_borderFillLumOffValue2 = value;
  }

  internal Excel2007ShapeLineStyle BorderLineStyle
  {
    get => this.m_borderLineStyle;
    set
    {
      this.m_borderLineStyle = value;
      this.m_flag |= (byte) 16 /*0x10*/;
      this.m_flag |= (byte) 2;
    }
  }

  internal bool BorderIsRound
  {
    get => this.m_borderIsRound;
    set => this.m_borderIsRound = value;
  }

  internal EndLineCap LineCap
  {
    get => this.m_lineCap;
    set
    {
      this.m_lineCap = value;
      this.m_flag |= (byte) 8;
      this.m_flag |= (byte) 2;
    }
  }

  internal bool IsInsetPenAlignment
  {
    get => this.m_isInsetPenAlignment;
    set
    {
      this.m_isInsetPenAlignment = value;
      this.m_flag |= (byte) 32 /*0x20*/;
      this.m_flag |= (byte) 2;
    }
  }

  internal byte FlagOptions => this.m_flag;

  internal StyleEntryShapeProperties()
  {
  }
}
