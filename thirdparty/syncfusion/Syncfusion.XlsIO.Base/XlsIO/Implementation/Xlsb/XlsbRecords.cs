// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Xlsb.XlsbRecords
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Xlsb;

internal enum XlsbRecords
{
  Blank = 1,
  Number = 2,
  Error = 3,
  Boolean = 4,
  Double = 5,
  LabelText = 6,
  SSTItem = 7,
  FormulaStr = 8,
  FormulaNum = 9,
  FormulaBool = 10, // 0x0000000A
  FormulaErr = 11, // 0x0000000B
  StrRecord = 19, // 0x00000013
  Name = 39, // 0x00000027
  Font = 43, // 0x0000002B
  NumberFmt = 44, // 0x0000002C
  Fill = 45, // 0x0000002D
  Border = 46, // 0x0000002E
  CellStyleXf = 47, // 0x0000002F
  CellStyle = 48, // 0x00000030
  BeginWorksheet = 385, // 0x00000181
  EndWorksheet = 386, // 0x00000182
  BeginWorkbook = 387, // 0x00000183
  EndWorkbook = 388, // 0x00000184
  SheetViewsBegin = 389, // 0x00000185
  SheetViewsEnd = 390, // 0x00000186
  SheetViewBegin = 393, // 0x00000189
  SheetViewEnd = 394, // 0x0000018A
  SheetsColBegin = 399, // 0x0000018F
  SheetsColEnd = 400, // 0x00000190
  BeginSheetData = 401, // 0x00000191
  EndSheetData = 402, // 0x00000192
  SheetPr = 403, // 0x00000193
  SheetDimension = 404, // 0x00000194
  SheetSelect = 408, // 0x00000198
  SheetRecord = 412, // 0x0000019C
  BeginSST = 415, // 0x0000019F
  EndSST = 416, // 0x000001A0
  BeginStyleSheet = 662, // 0x00000296
  EndStyleSheet = 663, // 0x00000297
  ColumnsBegin = 902, // 0x00000386
  ColumnsEnd = 903, // 0x00000387
  BeginSheetFormat = 997, // 0x000003E5
  Drawings = 1190, // 0x000004A6
  BeginFillCol = 1243, // 0x000004DB
  EndFillCol = 1244, // 0x000004DC
  BeginFontsCol = 1251, // 0x000004E3
  EndFontsCol = 1252, // 0x000004E4
  BeginBorderCol = 1253, // 0x000004E5
  EndBorderCol = 1254, // 0x000004E6
  BeginNumFmtCol = 1255, // 0x000004E7
  EndNumFmtCol = 1256, // 0x000004E8
  BeginCellXfs = 1257, // 0x000004E9
  EndCellXfs = 1258, // 0x000004EA
  BeginCellStyles = 1259, // 0x000004EB
  EndCellStyle = 1260, // 0x000004EC
  BeginCellStyleXfs = 1266, // 0x000004F2
  EndCellStyleXfs = 1267, // 0x000004F3
  Column = 4668, // 0x0000123C
  Row = 6400, // 0x00001900
}
