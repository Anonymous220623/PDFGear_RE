// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IChartBorder
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IChartBorder
{
  Color LineColor { get; set; }

  ExcelChartLinePattern LinePattern { get; set; }

  ExcelChartLineWeight LineWeight { get; set; }

  bool AutoFormat { get; set; }

  bool IsAutoLineColor { get; set; }

  ExcelKnownColors ColorIndex { get; set; }

  bool DrawTickLabels { get; set; }

  double Transparency { get; set; }

  double Weight { get; set; }
}
