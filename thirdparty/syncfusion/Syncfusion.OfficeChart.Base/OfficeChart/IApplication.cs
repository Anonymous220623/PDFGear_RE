// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IApplication
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart;

internal interface IApplication : IParentApplication
{
  IRange ActiveCell { get; }

  IWorksheet ActiveSheet { get; }

  IWorkbook ActiveWorkbook { get; }

  IWorkbooks Workbooks { get; }

  IWorksheets Worksheets { get; }

  IRange Range { get; }

  bool FixedDecimal { get; set; }

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

  OfficeSkipExtRecords SkipOnSave { get; set; }

  double StandardHeight { get; set; }

  bool StandardHeightFlag { get; set; }

  double StandardWidth { get; set; }

  bool OptimizeFonts { get; set; }

  bool OptimizeImport { get; set; }

  char RowSeparator { get; set; }

  char ArgumentsSeparator { get; set; }

  string CSVSeparator { get; set; }

  string StandardFont { get; set; }

  double StandardFontSize { get; set; }

  [Obsolete("Use DataProviderType property instead")]
  bool UseNativeOptimization { get; set; }

  bool UseFastRecordParsing { get; set; }

  int RowStorageAllocationBlockSize { get; set; }

  bool DeleteDestinationFile { get; set; }

  OfficeVersion DefaultVersion { get; set; }

  bool UseNativeStorage { get; set; }

  OfficeDataProviderType DataProviderType { get; set; }

  Syncfusion.Compression.CompressionLevel? CompressionLevel { get; set; }

  bool PreserveCSVDataTypes { get; set; }

  IOfficeChartToImageConverter ChartToImageConverter { get; set; }

  double CentimetersToPoints(double Centimeters);

  double InchesToPoints(double Inches);

  void Save(string Filename);

  bool IsSupported(string FilePath);

  bool IsSupported(Stream Stream);

  double ConvertUnits(double value, MeasureUnits from, MeasureUnits to);

  event ProgressEventHandler ProgressEvent;

  event PasswordRequiredEventHandler OnPasswordRequired;

  event PasswordRequiredEventHandler OnWrongPassword;
}
