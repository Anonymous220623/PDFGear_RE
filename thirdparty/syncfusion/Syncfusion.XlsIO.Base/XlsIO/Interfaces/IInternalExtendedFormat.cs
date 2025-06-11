// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Interfaces.IInternalExtendedFormat
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;

#nullable disable
namespace Syncfusion.XlsIO.Interfaces;

public interface IInternalExtendedFormat : IExtendedFormat, IParentApplication
{
  ColorObject BottomBorderColor { get; }

  ColorObject TopBorderColor { get; }

  ColorObject LeftBorderColor { get; }

  ColorObject RightBorderColor { get; }

  ColorObject DiagonalBorderColor { get; }

  ExcelLineStyle LeftBorderLineStyle { get; set; }

  ExcelLineStyle RightBorderLineStyle { get; set; }

  ExcelLineStyle TopBorderLineStyle { get; set; }

  ExcelLineStyle BottomBorderLineStyle { get; set; }

  ExcelLineStyle DiagonalUpBorderLineStyle { get; set; }

  ExcelLineStyle DiagonalDownBorderLineStyle { get; set; }

  bool DiagonalUpVisible { get; set; }

  bool DiagonalDownVisible { get; set; }

  WorkbookImpl Workbook { get; }

  void BeginUpdate();

  void EndUpdate();
}
