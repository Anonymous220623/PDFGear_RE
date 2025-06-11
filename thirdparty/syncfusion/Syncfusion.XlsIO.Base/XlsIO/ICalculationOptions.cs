// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ICalculationOptions
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO;

public interface ICalculationOptions : IParentApplication
{
  int MaximumIteration { get; set; }

  bool RecalcOnSave { get; set; }

  double MaximumChange { get; set; }

  bool IsIterationEnabled { get; set; }

  bool R1C1ReferenceMode { get; set; }

  ExcelCalculationMode CalculationMode { get; set; }
}
