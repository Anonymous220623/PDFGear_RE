// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IChartTrendLine
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO;

public interface IChartTrendLine
{
  IChartBorder Border { get; }

  double Backward { get; set; }

  double Forward { get; set; }

  bool DisplayEquation { get; set; }

  bool DisplayRSquared { get; set; }

  double Intercept { get; set; }

  bool InterceptIsAuto { get; set; }

  ExcelTrendLineType Type { get; set; }

  int Order { get; set; }

  bool NameIsAuto { get; set; }

  string Name { get; set; }

  IChartTextArea DataLabel { get; }

  IShadow Shadow { get; }

  IThreeDFormat Chart3DOptions { get; }

  void ClearFormats();
}
