// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IChartLegend
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO;

public interface IChartLegend
{
  IChartFrameFormat FrameFormat { get; }

  IChartTextArea TextArea { get; }

  int X { get; set; }

  int Y { get; set; }

  ExcelLegendPosition Position { get; set; }

  bool IsVerticalLegend { get; set; }

  IChartLegendEntries LegendEntries { get; }

  bool IncludeInLayout { get; set; }

  IChartLayout Layout { get; set; }

  void Clear();

  void Delete();
}
