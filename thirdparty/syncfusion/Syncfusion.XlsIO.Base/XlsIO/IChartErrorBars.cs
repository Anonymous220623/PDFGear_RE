// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IChartErrorBars
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO;

public interface IChartErrorBars
{
  IChartBorder Border { get; }

  ExcelErrorBarInclude Include { get; set; }

  bool HasCap { get; set; }

  ExcelErrorBarType Type { get; set; }

  double NumberValue { get; set; }

  IRange PlusRange { get; set; }

  IRange MinusRange { get; set; }

  IShadow Shadow { get; }

  IThreeDFormat Chart3DOptions { get; }

  void ClearFormats();

  void Delete();
}
