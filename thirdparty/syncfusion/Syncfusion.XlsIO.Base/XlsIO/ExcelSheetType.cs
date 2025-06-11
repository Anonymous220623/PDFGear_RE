// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ExcelSheetType
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.ComponentModel;

#nullable disable
namespace Syncfusion.XlsIO;

public enum ExcelSheetType
{
  [Description("Worksheets")] Worksheet = 0,
  [Description("Charts")] Chart = 2,
  [Description("Dialogs")] DialogSheet = 3,
  [Description("Excel 4.0 Intl Marcos")] Excel4IntlMacroSheet = 4,
  [Description("Excel 4.0 Macros")] Excel4MacroSheet = 5,
}
