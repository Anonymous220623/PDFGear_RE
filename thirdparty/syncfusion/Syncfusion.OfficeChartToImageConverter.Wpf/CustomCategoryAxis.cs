// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChartToImageConverter.CustomCategoryAxis
// Assembly: Syncfusion.OfficeChartToImageConverter.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 82053128-0A33-4E43-8DD1-E8016B1463BC
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChartToImageConverter.Wpf.dll

using Syncfusion.UI.Xaml.Charts;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.OfficeChartToImageConverter;

internal class CustomCategoryAxis : CategoryAxis
{
  private Canvas m_labelCanvas;
  private bool m_isVerticalAxis;

  internal Canvas LabelsCanvas => this.m_labelCanvas;

  internal bool IsVerticalAxis
  {
    get => this.m_isVerticalAxis;
    set => this.m_isVerticalAxis = value;
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.m_labelCanvas = this.GetTemplateChild("axisLabelsPanel") as Canvas;
  }
}
