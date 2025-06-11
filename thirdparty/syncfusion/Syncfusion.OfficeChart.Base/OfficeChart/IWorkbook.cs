// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IWorkbook
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.OfficeChart;

internal interface IWorkbook : IParentApplication
{
  IWorksheet ActiveSheet { get; }

  int ActiveSheetIndex { get; set; }

  string Author { get; set; }

  bool IsHScrollBarVisible { get; set; }

  bool IsVScrollBarVisible { get; set; }

  string CodeName { get; set; }

  bool Date1904 { get; set; }

  bool PrecisionAsDisplayed { get; set; }

  bool IsCellProtection { get; }

  bool IsWindowProtection { get; }

  INames Names { get; }

  bool ReadOnly { get; }

  bool Saved { get; set; }

  IStyles Styles { get; }

  IWorksheets Worksheets { get; }

  bool HasMacros { get; }

  [Obsolete("IWorkbook.Palettte property is obsolete so please use the IWorkbook.Palette property instead. IWorkbook.Palettte will be removed in July 2006. Sorry for the inconvenience")]
  Color[] Palettte { get; }

  Color[] Palette { get; }

  int DisplayedTab { get; set; }

  bool ThrowOnUnknownNames { get; set; }

  bool DisableMacrosStart { get; set; }

  double StandardFontSize { get; set; }

  string StandardFont { get; set; }

  bool Allow3DRangesInDataValidation { get; set; }

  ICalculationOptions CalculationOptions { get; }

  string RowSeparator { get; }

  string ArgumentsSeparator { get; }

  IWorksheetGroup WorksheetGroup { get; }

  bool IsRightToLeft { get; set; }

  bool DisplayWorkbookTabs { get; set; }

  ITabSheets TabSheets { get; }

  bool DetectDateTimeInValue { get; set; }

  bool UseFastStringSearching { get; set; }

  bool ReadOnlyRecommended { get; set; }

  string PasswordToOpen { get; set; }

  int MaxRowCount { get; }

  int MaxColumnCount { get; }

  OfficeVersion Version { get; set; }

  void Activate();

  IOfficeFont AddFont(IOfficeFont fontToAdd);

  void Close(bool SaveChanges, string Filename);

  void Close(bool saveChanges);

  void Close();

  void Close(string Filename);

  void Save();

  void SaveAs(string Filename);

  void SaveAs(string Filename, OfficeSaveType saveType);

  void SaveAsXml(string strFileName, OfficeXmlSaveType saveType);

  void SaveAs(string fileName, string separator);

  IOfficeFont CreateFont(Font nativeFont);

  void Replace(string oldValue, DataTable newValues, bool isFieldNamesShown);

  void Replace(string oldValue, DataColumn newValues, bool isFieldNamesShown);

  void SaveAs(Stream stream);

  void SaveAs(Stream stream, OfficeSaveType saveType);

  void SaveAsXml(XmlWriter writer, OfficeXmlSaveType saveType);

  void SaveAsXml(Stream stream, OfficeXmlSaveType saveType);

  void SaveAs(Stream stream, string separator);

  void SetPaletteColor(int index, Color color);

  void ResetPalette();

  Color GetPaletteColor(OfficeKnownColors color);

  OfficeKnownColors GetNearestColor(Color color);

  OfficeKnownColors GetNearestColor(int r, int g, int b);

  OfficeKnownColors SetColorOrGetNearest(Color color);

  OfficeKnownColors SetColorOrGetNearest(int r, int g, int b);

  IOfficeFont CreateFont();

  IOfficeFont CreateFont(IOfficeFont baseFont);

  void Replace(string oldValue, string newValue);

  void Replace(string oldValue, double newValue);

  void Replace(string oldValue, DateTime newValue);

  void Replace(string oldValue, string[] newValues, bool isVertical);

  void Replace(string oldValue, int[] newValues, bool isVertical);

  void Replace(string oldValue, double[] newValues, bool isVertical);

  IRange FindFirst(string findValue, OfficeFindType flags);

  IRange FindFirst(string findValue, OfficeFindType flags, OfficeFindOptions findOptions);

  IRange FindStringStartsWith(string findValue, OfficeFindType flags);

  IRange FindStringStartsWith(string findValue, OfficeFindType flags, bool ignoreCase);

  IRange FindStringEndsWith(string findValue, OfficeFindType flags);

  IRange FindStringEndsWith(string findValue, OfficeFindType flags, bool ignoreCase);

  IRange FindFirst(double findValue, OfficeFindType flags);

  IRange FindFirst(bool findValue);

  IRange FindFirst(DateTime findValue);

  IRange FindFirst(TimeSpan findValue);

  IRange[] FindAll(string findValue, OfficeFindType flags);

  IRange[] FindAll(string findValue, OfficeFindType flags, OfficeFindOptions findOptions);

  IRange[] FindAll(double findValue, OfficeFindType flags);

  IRange[] FindAll(bool findValue);

  IRange[] FindAll(DateTime findValue);

  IRange[] FindAll(TimeSpan findValue);

  void SetSeparators(char argumentsSeparator, char arrayRowsSeparator);

  void Protect(bool bIsProtectWindow, bool bIsProtectContent);

  void Protect(bool bIsProtectWindow, bool bIsProtectContent, string password);

  void Unprotect();

  void Unprotect(string password);

  IWorkbook Clone();

  void SetWriteProtectionPassword(string password);

  event EventHandler OnFileSaved;

  event ReadOnlyFileEventHandler OnReadOnlyFile;
}
