// Decompiled with JetBrains decompiler
// Type: Syncfusion.ExcelChartToImageConverter.MarkerSetting
// Assembly: Syncfusion.ExcelChartToImageConverter.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 8A92A829-7139-4C93-8632-144655877EB3
// Assembly location: C:\Program Files\PDFgear\Syncfusion.ExcelChartToImageConverter.Wpf.dll

using System.Windows.Media;

#nullable disable
namespace Syncfusion.ExcelChartToImageConverter;

internal class MarkerSetting
{
  private Brush m_borderBrush;
  private Brush m_fillBrush;
  private double m_markerSize;
  private double m_borderThickness;
  private int m_markerTypeInInt;

  internal Brush BorderBrush
  {
    get => this.m_borderBrush;
    set => this.m_borderBrush = value;
  }

  internal Brush FillBrush
  {
    get => this.m_fillBrush;
    set => this.m_fillBrush = value;
  }

  internal double MarkerSize
  {
    get => this.m_markerSize;
    set => this.m_markerSize = value;
  }

  internal double BorderThickness
  {
    get => this.m_borderThickness;
    set => this.m_borderThickness = value;
  }

  internal int MarkerTypeInInt
  {
    get => this.m_markerTypeInInt;
    set => this.m_markerTypeInInt = value;
  }

  internal MarkerSetting()
  {
  }
}
