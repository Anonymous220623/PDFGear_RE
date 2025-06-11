// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IOfficeChartRichTextString
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

#nullable disable
namespace Syncfusion.OfficeChart;

internal interface IOfficeChartRichTextString : IParentApplication, IOptimizedUpdate
{
  string Text { get; }

  ChartAlrunsRecord.TRuns[] FormattingRuns { get; }

  void SetFont(int iStartPos, int iEndPos, IOfficeFont font);

  IOfficeFont GetFont(ChartAlrunsRecord.TRuns tRuns);
}
