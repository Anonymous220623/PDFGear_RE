// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IChartInterior
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IChartInterior
{
  Color ForegroundColor { get; set; }

  Color BackgroundColor { get; set; }

  ExcelPattern Pattern { get; set; }

  ExcelKnownColors ForegroundColorIndex { get; set; }

  ExcelKnownColors BackgroundColorIndex { get; set; }

  bool UseAutomaticFormat { get; set; }

  bool SwapColorsOnNegative { get; set; }
}
