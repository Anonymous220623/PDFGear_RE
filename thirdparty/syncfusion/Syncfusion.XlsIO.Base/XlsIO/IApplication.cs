// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IApplication
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IApplication : IParentApplication
{
  bool UseStringDelimiter { get; set; }

  IRange ActiveCell { get; }

  IWorksheet ActiveSheet { get; }

  IWorkbook ActiveWorkbook { get; }

  IWorkbooks Workbooks { get; }

  IWorksheets Worksheets { get; }

  IRange Range { get; }

  bool FixedDecimal { get; set; }

  bool IgnoreSheetNameException { get; set; }

  bool UseSystemSeparators { get; set; }

  int Build { get; }

  int FixedDecimalPlaces { get; set; }

  int SheetsInNewWorkbook { get; set; }

  string DecimalSeparator { get; set; }

  string DefaultFilePath { get; set; }

  string PathSeparator { get; }

  string ThousandsSeparator { get; set; }

  string UserName { get; set; }

  string Value { get; }

  bool ChangeStyleOnCellEdit { get; set; }

  SkipExtRecords SkipOnSave { get; set; }

  double StandardHeight { get; set; }

  bool StandardHeightFlag { get; set; }

  double StandardWidth { get; set; }

  bool OptimizeFonts { get; set; }

  bool OptimizeImport { get; set; }

  char RowSeparator { get; set; }

  char ArgumentsSeparator { get; set; }

  string CSVSeparator { get; set; }

  string CsvQualifier { get; set; }

  string CsvRecordDelimiter { get; set; }

  string StandardFont { get; set; }

  double StandardFontSize { get; set; }

  [Obsolete("Use DataProviderType property instead")]
  bool UseNativeOptimization { get; set; }

  bool UseFastRecordParsing { get; set; }

  int RowStorageAllocationBlockSize { get; set; }

  bool DeleteDestinationFile { get; set; }

  ExcelVersion DefaultVersion { get; set; }

  bool UseNativeStorage { get; set; }

  ExcelDataProviderType DataProviderType { get; set; }

  Syncfusion.Compression.CompressionLevel? CompressionLevel { get; set; }

  bool PreserveCSVDataTypes { get; set; }

  IChartToImageConverter ChartToImageConverter { get; set; }

  bool IsChartCacheEnabled { get; set; }

  bool EnableIncrementalFormula { get; set; }

  bool UpdateSheetFormulaReference { get; set; }

  bool EnablePartialTrustCode { get; set; }

  ExcelRangeIndexerMode RangeIndexerMode { get; set; }

  bool SkipAutoFitRow { get; set; }

  bool ExcludeAdditionalCharacters { get; set; }

  double CentimetersToPoints(double Centimeters);

  double InchesToPoints(double Inches);

  void Save(string Filename);

  bool IsSupported(string FilePath);

  bool IsSupported(Stream Stream);

  double ConvertUnits(double value, MeasureUnits from, MeasureUnits to);

  event ProgressEventHandler ProgressEvent;

  event PasswordRequiredEventHandler OnPasswordRequired;

  event PasswordRequiredEventHandler OnWrongPassword;

  event SubstituteFontEventHandler SubstituteFont;

  event ExportEventHandler TypeMismatchOnExport;
}
