// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Interfaces.IInternalExtendedFormat
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;

#nullable disable
namespace Syncfusion.OfficeChart.Interfaces;

internal interface IInternalExtendedFormat : IExtendedFormat, IParentApplication
{
  ChartColor BottomBorderColor { get; }

  ChartColor TopBorderColor { get; }

  ChartColor LeftBorderColor { get; }

  ChartColor RightBorderColor { get; }

  ChartColor DiagonalBorderColor { get; }

  OfficeLineStyle LeftBorderLineStyle { get; set; }

  OfficeLineStyle RightBorderLineStyle { get; set; }

  OfficeLineStyle TopBorderLineStyle { get; set; }

  OfficeLineStyle BottomBorderLineStyle { get; set; }

  OfficeLineStyle DiagonalUpBorderLineStyle { get; set; }

  OfficeLineStyle DiagonalDownBorderLineStyle { get; set; }

  bool DiagonalUpVisible { get; set; }

  bool DiagonalDownVisible { get; set; }

  WorkbookImpl Workbook { get; }

  void BeginUpdate();

  void EndUpdate();
}
