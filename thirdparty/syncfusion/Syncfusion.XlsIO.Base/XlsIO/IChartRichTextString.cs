// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IChartRichTextString
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser.Biff_Records.Charts;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IChartRichTextString : IParentApplication, IOptimizedUpdate
{
  string Text { get; }

  ChartAlrunsRecord.TRuns[] FormattingRuns { get; }

  void SetFont(int iStartPos, int iEndPos, IFont font);

  IFont GetFont(ChartAlrunsRecord.TRuns tRuns);
}
