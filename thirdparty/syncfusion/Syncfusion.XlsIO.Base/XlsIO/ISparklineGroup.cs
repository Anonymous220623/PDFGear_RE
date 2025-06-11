// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ISparklineGroup
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO;

public interface ISparklineGroup : 
  IList<ISparklines>,
  ICollection<ISparklines>,
  IEnumerable<ISparklines>,
  IEnumerable
{
  bool DisplayAxis { get; set; }

  bool DisplayHiddenRC { get; set; }

  bool PlotRightToLeft { get; set; }

  bool ShowFirstPoint { get; set; }

  bool ShowLastPoint { get; set; }

  bool ShowLowPoint { get; set; }

  bool ShowHighPoint { get; set; }

  bool ShowNegativePoint { get; set; }

  bool ShowMarkers { get; set; }

  ISparklineVerticalAxis VerticalAxisMaximum { get; set; }

  ISparklineVerticalAxis VerticalAxisMinimum { get; set; }

  SparklineType SparklineType { get; set; }

  bool HorizontalDateAxis { get; set; }

  SparklineEmptyCells DisplayEmptyCellsAs { get; set; }

  IRange HorizontalDateAxisRange { get; set; }

  Color AxisColor { get; set; }

  Color FirstPointColor { get; set; }

  Color HighPointColor { get; set; }

  Color LastPointColor { get; set; }

  double LineWeight { get; set; }

  Color LowPointColor { get; set; }

  Color MarkersColor { get; set; }

  Color NegativePointColor { get; set; }

  Color SparklineColor { get; set; }

  ISparklines Add();
}
