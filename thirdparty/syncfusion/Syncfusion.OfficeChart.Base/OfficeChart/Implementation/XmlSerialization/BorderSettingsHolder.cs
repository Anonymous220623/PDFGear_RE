// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.XmlSerialization.BorderSettingsHolder
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.XmlSerialization;

internal class BorderSettingsHolder : IBorder, IParentApplication, ICloneable
{
  private ChartColor m_color = new ChartColor(OfficeKnownColors.Black);
  private OfficeLineStyle m_lineStyle;
  private bool m_bShowDiagonalLine;
  private bool m_bIsEmptyBorder = true;

  public OfficeKnownColors Color
  {
    get
    {
      return this.m_color.ColorType != ColorType.Indexed ? OfficeKnownColors.Black : (OfficeKnownColors) this.m_color.Value;
    }
    set => this.m_color.SetIndexed(value);
  }

  public ChartColor ColorObject => this.m_color;

  public System.Drawing.Color ColorRGB
  {
    get
    {
      return this.m_color.ColorType != ColorType.RGB ? ColorExtension.Empty : ColorExtension.FromArgb(this.m_color.Value);
    }
    set => this.m_color.SetRGB(value);
  }

  public OfficeLineStyle LineStyle
  {
    get => this.m_lineStyle;
    set => this.m_lineStyle = value;
  }

  public bool ShowDiagonalLine
  {
    get => this.m_bShowDiagonalLine;
    set => this.m_bShowDiagonalLine = value;
  }

  internal bool IsEmptyBorder
  {
    get => this.m_bIsEmptyBorder;
    set => this.m_bIsEmptyBorder = value;
  }

  public IApplication Application => throw new NotImplementedException();

  public object Parent => throw new NotImplementedException();

  public object Clone() => this.MemberwiseClone();
}
