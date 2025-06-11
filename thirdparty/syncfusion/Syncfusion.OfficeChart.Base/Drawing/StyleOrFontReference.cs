// Decompiled with JetBrains decompiler
// Type: Syncfusion.Drawing.StyleOrFontReference
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart;

#nullable disable
namespace Syncfusion.Drawing;

internal class StyleOrFontReference
{
  private int m_index;
  private ColorModel m_colorModelType;
  private string m_colorValue;
  private double m_lumModValue;
  private double m_lumOffValue1;
  private double m_lumOffValue2;
  private double m_shadeValue;

  internal int Index
  {
    get => this.m_index;
    set => this.m_index = value;
  }

  internal ColorModel ColorModelType
  {
    get => this.m_colorModelType;
    set => this.m_colorModelType = value;
  }

  internal string ColorValue
  {
    get => this.m_colorValue;
    set => this.m_colorValue = value;
  }

  internal double LumModValue
  {
    get => this.m_lumModValue;
    set => this.m_lumModValue = value;
  }

  internal double LumOffValue1
  {
    get => this.m_lumOffValue1;
    set => this.m_lumOffValue1 = value;
  }

  internal double LumOffValue2
  {
    get => this.m_lumOffValue2;
    set => this.m_lumOffValue2 = value;
  }

  internal double ShadeValue
  {
    get => this.m_shadeValue;
    set => this.m_shadeValue = value;
  }

  internal StyleOrFontReference(
    int index,
    ColorModel colorModel,
    string colorValue,
    double lumModValue,
    double lumOffValue1,
    double lumOffValue2,
    double shadeValue)
  {
    this.m_index = index;
    this.m_colorModelType = colorModel;
    this.m_colorValue = colorValue;
    this.m_lumModValue = lumModValue;
    this.m_lumOffValue1 = lumOffValue1;
    this.m_lumOffValue2 = lumOffValue2;
    this.m_shadeValue = shadeValue;
  }
}
