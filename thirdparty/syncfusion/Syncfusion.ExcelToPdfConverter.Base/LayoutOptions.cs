// Decompiled with JetBrains decompiler
// Type: Syncfusion.ExcelToPdfConverter.LayoutOptions
// Assembly: Syncfusion.ExcelToPDFConverter.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 4304B189-CB46-46CF-B5C1-2287263DCC93
// Assembly location: C:\Program Files\PDFgear\Syncfusion.ExcelToPdfConverter.Base.dll

#nullable disable
namespace Syncfusion.ExcelToPdfConverter;

public enum LayoutOptions
{
  FitSheetOnOnePage = 1,
  NoScaling = 2,
  FitAllColumnsOnOnePage = 4,
  FitAllRowsOnOnePage = 8,
  CustomScaling = 16, // 0x00000010
  Automatic = 32, // 0x00000020
}
