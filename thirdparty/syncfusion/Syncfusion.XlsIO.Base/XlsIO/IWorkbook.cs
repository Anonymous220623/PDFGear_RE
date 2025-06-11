// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IWorkbook
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Office;
using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Implementation.Collections;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IWorkbook : IParentApplication
{
  IVbaProject VbaProject { get; }

  ITableStyles TableStyles { get; }

  IWorksheet ActiveSheet { get; }

  int ActiveSheetIndex { get; set; }

  IAddInFunctions AddInFunctions { get; }

  string Author { get; set; }

  bool IsHScrollBarVisible { get; set; }

  bool IsVScrollBarVisible { get; set; }

  IBuiltInDocumentProperties BuiltInDocumentProperties { get; }

  string CodeName { get; set; }

  ICustomDocumentProperties CustomDocumentProperties { get; }

  IMetaProperties ContentTypeProperties { get; }

  ICustomXmlPartCollection CustomXmlparts { get; }

  string FullFileName { get; }

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

  ICharts Charts { get; }

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

  ExcelVersion Version { get; set; }

  IPivotCaches PivotCaches { get; }

  IConnections Connections { get; }

  IDataSort CreateDataSorter();

  void Activate();

  IFont AddFont(IFont fontToAdd);

  void Close(bool SaveChanges, string Filename);

  void Close(bool saveChanges);

  void Close();

  void Close(string Filename);

  void Save();

  void SaveAs(string Filename);

  void SaveAs(string Filename, ExcelSaveType saveType);

  void SaveAsXml(string strFileName, ExcelXmlSaveType saveType);

  void SaveAs(string fileName, string separator);

  void SaveAs(string fileName, string separator, Encoding encoding);

  void SaveAsHtml(string filename, HtmlSaveOptions saveOptions);

  void SaveAsHtml(Stream stream);

  void SaveAsHtml(Stream stream, HtmlSaveOptions saveOptions);

  IFont CreateFont(Font nativeFont);

  void Replace(string oldValue, DataTable newValues, bool isFieldNamesShown);

  void Replace(string oldValue, DataColumn newValues, bool isFieldNamesShown);

  IHFEngine CreateHFEngine();

  void CopyToClipboard();

  void SaveAs(
    string fileName,
    string separator,
    HttpResponse response,
    ExcelDownloadType downloadType,
    ExcelHttpContentType contentType);

  void SaveAs(
    string fileName,
    string separator,
    HttpResponse response,
    ExcelDownloadType downloadType,
    ExcelHttpContentType contentType,
    Encoding encoding);

  void SaveAs(string fileName, ExcelSaveType saveType, HttpResponse response);

  void SaveAs(
    string fileName,
    ExcelSaveType saveType,
    HttpResponse response,
    ExcelHttpContentType contentType);

  void SaveAs(string fileName, HttpResponse response, ExcelHttpContentType contentType);

  void SaveAs(
    string fileName,
    ExcelSaveType saveType,
    HttpResponse response,
    ExcelDownloadType downloadType);

  void SaveAs(
    string fileName,
    ExcelSaveType saveType,
    HttpResponse response,
    ExcelDownloadType downloadType,
    ExcelHttpContentType contentType);

  void SaveAs(string fileName, HttpResponse response, ExcelDownloadType downloadType);

  void SaveAs(
    string fileName,
    HttpResponse response,
    ExcelDownloadType downloadType,
    ExcelHttpContentType contentType);

  ITemplateMarkersProcessor CreateTemplateMarkersProcessor();

  void MarkAsFinal();

  void SaveAs(Stream stream);

  void SaveAs(Stream stream, ExcelSaveType saveType);

  void SaveAsXml(XmlWriter writer, ExcelXmlSaveType saveType);

  void SaveAsXml(Stream stream, ExcelXmlSaveType saveType);

  void SaveAs(Stream stream, string separator);

  void SaveAs(Stream stream, string separator, Encoding encoding);

  void SaveAsJson(string filename);

  void SaveAsJson(string filename, bool isSchema);

  void SaveAsJson(string filename, IWorksheet worksheet);

  void SaveAsJson(string filename, IWorksheet worksheet, bool isSchema);

  void SaveAsJson(string filename, IRange range);

  void SaveAsJson(string filename, IRange range, bool isSchema);

  void SaveAsJson(Stream stream);

  void SaveAsJson(Stream stream, bool isSchema);

  void SaveAsJson(Stream stream, IWorksheet worksheet);

  void SaveAsJson(Stream stream, IWorksheet worksheet, bool isSchema);

  void SaveAsJson(Stream stream, IRange range);

  void SaveAsJson(Stream stream, IRange range, bool IsSchema);

  void SetPaletteColor(int index, Color color);

  void ResetPalette();

  Color GetPaletteColor(ExcelKnownColors color);

  ExcelKnownColors GetNearestColor(Color color);

  ExcelKnownColors GetNearestColor(int r, int g, int b);

  ExcelKnownColors SetColorOrGetNearest(Color color);

  ExcelKnownColors SetColorOrGetNearest(int r, int g, int b);

  IFont CreateFont();

  IFont CreateFont(IFont baseFont);

  void Replace(string oldValue, string newValue);

  void Replace(string oldValue, string newValue, ExcelFindOptions findOptions);

  void Replace(string oldValue, double newValue);

  void Replace(string oldValue, DateTime newValue);

  void Replace(string oldValue, string[] newValues, bool isVertical);

  void Replace(string oldValue, int[] newValues, bool isVertical);

  void Replace(string oldValue, double[] newValues, bool isVertical);

  IRange FindFirst(string findValue, ExcelFindType flags);

  IRange FindFirst(string findValue, ExcelFindType flags, ExcelFindOptions findOptions);

  IRange FindStringStartsWith(string findValue, ExcelFindType flags);

  IRange FindStringStartsWith(string findValue, ExcelFindType flags, bool ignoreCase);

  IRange FindStringEndsWith(string findValue, ExcelFindType flags);

  IRange FindStringEndsWith(string findValue, ExcelFindType flags, bool ignoreCase);

  IRange FindFirst(double findValue, ExcelFindType flags);

  IRange FindFirst(bool findValue);

  IRange FindFirst(DateTime findValue);

  IRange FindFirst(TimeSpan findValue);

  IRange[] FindAll(string findValue, ExcelFindType flags);

  IRange[] FindAll(string findValue, ExcelFindType flags, ExcelFindOptions findOptions);

  IRange[] FindAll(double findValue, ExcelFindType flags);

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

  void ImportXml(string fileName);

  void ImportXml(Stream stream);

  XmlMapCollection XmlMaps { get; }

  event EventHandler OnFileSaved;

  event ReadOnlyFileEventHandler OnReadOnlyFile;
}
