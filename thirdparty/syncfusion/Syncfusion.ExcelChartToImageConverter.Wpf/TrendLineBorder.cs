// Decompiled with JetBrains decompiler
// Type: Syncfusion.ExcelChartToImageConverter.TrendLineBorder
// Assembly: Syncfusion.ExcelChartToImageConverter.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 8A92A829-7139-4C93-8632-144655877EB3
// Assembly location: C:\Program Files\PDFgear\Syncfusion.ExcelChartToImageConverter.Wpf.dll

using Syncfusion.XlsIO;
using Syncfusion.XlsIO.Implementation.Charts;
using System.Drawing;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.ExcelChartToImageConverter;

internal class TrendLineBorder
{
  private RectangleF m_layout;
  private Border m_border;
  private bool m_manualLayout;

  internal Border Border
  {
    get => this.m_border;
    set => this.m_border = value;
  }

  internal RectangleF TrendLineLayout
  {
    get => this.m_layout;
    set => this.m_layout = value;
  }

  internal float X
  {
    get => this.m_layout.X;
    set => this.m_layout.X = value;
  }

  internal float Y
  {
    get => this.m_layout.Y;
    set => this.m_layout.Y = value;
  }

  internal bool isManualLayout
  {
    get => this.m_manualLayout;
    set => this.m_manualLayout = value;
  }

  internal void TrendLineBorderAndLayout(
    ChartTrendLineImpl trendLineImpl,
    out RectangleF manualRect,
    TextBlock block,
    ChartCommon chartCommon,
    bool isDefaultText)
  {
    ChartTextAreaImpl trendLineTextArea = trendLineImpl.TrendLineTextArea as ChartTextAreaImpl;
    ChartLayoutImpl layout = trendLineTextArea.Layout as ChartLayoutImpl;
    manualRect = new RectangleF(-1f, -1f, 0.0f, 0.0f);
    this.m_manualLayout = layout.IsManualLayout;
    if (layout.IsManualLayout)
    {
      ChartManualLayoutImpl manualLayout = layout.ManualLayout as ChartManualLayoutImpl;
      if (manualLayout.FlagOptions != (byte) 0 && (manualLayout.LeftMode == LayoutModes.edge || manualLayout.WidthMode != LayoutModes.edge) && (manualLayout.TopMode == LayoutModes.edge || manualLayout.HeightMode != LayoutModes.edge))
        manualRect = chartCommon.CalculateManualLayout(manualLayout, out bool _, true);
    }
    if (trendLineTextArea.RichText != null && trendLineTextArea.RichText.FormattingRuns.Length > 1)
      chartCommon.SetTextBlockInlines(trendLineTextArea, block);
    else if (isDefaultText)
    {
      block.Text += trendLineImpl.DisplayEquation ? " Eq" : "";
      block.Text += trendLineImpl.DisplayRSquared ? " Rsq" : "";
    }
    else
      block.Text = trendLineImpl.TrendLineTextArea.Text;
  }
}
