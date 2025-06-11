// Decompiled with JetBrains decompiler
// Type: Syncfusion.ExcelToPdfConverter.TableStyleRenderer
// Assembly: Syncfusion.ExcelToPDFConverter.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 4304B189-CB46-46CF-B5C1-2287263DCC93
// Assembly location: C:\Program Files\PDFgear\Syncfusion.ExcelToPdfConverter.Base.dll

using Syncfusion.XlsIO;
using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Implementation.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.ExcelToPdfConverter;

internal class TableStyleRenderer
{
  private readonly Dictionary<long, Dictionary<ExcelBordersIndex, Color>> _borderColorList;
  private readonly Dictionary<TableBuiltInStyles, Color> _borderList;
  private readonly List<TableBuiltInStyles> _colorFonts;
  private readonly List<TableBuiltInStyles> _darkStyles;
  private readonly Dictionary<TableBuiltInStyles, Color> _doubleBorderList;
  private readonly Dictionary<IRange, Color> _fontColorCollections;
  private readonly List<TableBuiltInStyles> _lightStyleBorder;
  private readonly IListObjects _listObjects;
  private readonly List<TableBuiltInStyles> _mediumStyleBorder;
  private readonly List<TableBuiltInStyles> _mediumStyleWithBorder;
  private readonly List<TableBuiltInStyles> _mediumStyleWithoutBorder;
  private readonly Dictionary<TableBuiltInStyles, Color> _topSolidList;
  private TableBuiltInStyles _builtInStyle;
  private bool _columnStripes;
  private bool _firstColumn;
  private Color _headerColor = Color.Empty;
  private bool _headerRow;
  private bool _lastColumn;
  private bool _rowStripes;

  public TableStyleRenderer()
  {
    this._colorFonts = new List<TableBuiltInStyles>();
    this._borderList = new Dictionary<TableBuiltInStyles, Color>();
    this._doubleBorderList = new Dictionary<TableBuiltInStyles, Color>();
    this._topSolidList = new Dictionary<TableBuiltInStyles, Color>();
    this._borderColorList = new Dictionary<long, Dictionary<ExcelBordersIndex, Color>>();
    this._fontColorCollections = new Dictionary<IRange, Color>();
    this._darkStyles = new List<TableBuiltInStyles>();
    this._mediumStyleWithBorder = new List<TableBuiltInStyles>();
    this._mediumStyleWithoutBorder = new List<TableBuiltInStyles>();
    this._lightStyleBorder = new List<TableBuiltInStyles>();
    this._mediumStyleBorder = new List<TableBuiltInStyles>();
  }

  public TableStyleRenderer(IListObjects sheetListObjects)
    : this()
  {
    this._listObjects = sheetListObjects;
    WorkbookImpl workbook = sheetListObjects[0].Worksheet.Workbook as WorkbookImpl;
    bool isTabelStyleNewVersion = workbook.DefaultThemeVersion == "166925" || workbook.DefaultThemeVersion == "164011" || workbook.DefaultThemeVersion == "153222";
    this.InitializeBorders(isTabelStyleNewVersion);
    this.InitializeColorFonts();
    this.InitializeLightStyle();
    this.InitializeDoubleBorders(isTabelStyleNewVersion);
    this.InitializeTopSolid(isTabelStyleNewVersion);
  }

  internal Dictionary<long, Dictionary<ExcelBordersIndex, Color>> ApplyStyles(
    IWorksheet sheet,
    out Dictionary<IRange, Color> fontColorCollection)
  {
    foreach (IListObject listObject in (IEnumerable<IListObject>) this._listObjects)
    {
      this._columnStripes = listObject.ShowTableStyleColumnStripes;
      this._rowStripes = listObject.ShowTableStyleRowStripes;
      this._firstColumn = listObject.ShowFirstColumn;
      this._lastColumn = listObject.ShowLastColumn;
      this._headerRow = listObject.ShowHeaderRow;
      if (this._firstColumn || this._lastColumn)
        this.InitializeColumnSettings();
      this.InitializeMediumStyle();
      if (!string.IsNullOrEmpty(listObject.TableStyleName) && listObject.TableStyleName != listObject.BuiltInTableStyle.ToString())
      {
        ITableStyle tableStyle = sheet.Workbook.TableStyles[listObject.TableStyleName];
        ExcelTableStyleElementType[] styleElementTypeArray = new ExcelTableStyleElementType[13]
        {
          ExcelTableStyleElementType.LastTotalCell,
          ExcelTableStyleElementType.FirstTotalCell,
          ExcelTableStyleElementType.LastHeaderCell,
          ExcelTableStyleElementType.FirstHeaderCell,
          ExcelTableStyleElementType.TotalRow,
          ExcelTableStyleElementType.HeaderRow,
          ExcelTableStyleElementType.LastColumn,
          ExcelTableStyleElementType.FirstColumn,
          ExcelTableStyleElementType.FirstRowStripe,
          ExcelTableStyleElementType.SecondRowStripe,
          ExcelTableStyleElementType.FirstColumnStripe,
          ExcelTableStyleElementType.SecondColumnStripe,
          ExcelTableStyleElementType.WholeTable
        };
        int firstColumnStripeSize = 0;
        int secondColumnStripeSize = 0;
        int firstRowStripeSize = 0;
        int secondRowStripeSize = 0;
        foreach (ITableStyleElement tableStyleElement in (IEnumerable) tableStyle.TableStyleElements)
        {
          if (tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.FirstColumnStripe)
            firstColumnStripeSize = tableStyleElement.StripeSize;
          if (tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.SecondColumnStripe)
            secondColumnStripeSize = tableStyleElement.StripeSize;
          if (tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.FirstRowStripe)
            firstRowStripeSize = tableStyleElement.StripeSize;
          if (tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.SecondRowStripe)
            secondRowStripeSize = tableStyleElement.StripeSize;
        }
        for (int index1 = 0; index1 < styleElementTypeArray.Length; ++index1)
        {
          for (int index2 = 0; index2 < tableStyle.TableStyleElements.Count; ++index2)
          {
            if (tableStyle.TableStyleElements[index2].TableStyleElementType == styleElementTypeArray[index1])
            {
              this.DrawLocationAndCustomTableStyle(sheet, tableStyle.TableStyleElements[index2] as TableStyleElement, listObject.Location, listObject.ShowTotals, firstColumnStripeSize, secondColumnStripeSize, firstRowStripeSize, secondRowStripeSize);
              break;
            }
          }
        }
      }
      else
        this.DrawLocationAndStyle(sheet, listObject.BuiltInTableStyle, listObject.Location, listObject.ShowTotals);
    }
    fontColorCollection = this._fontColorCollections;
    return this._borderColorList;
  }

  private void InitializeLightStyle()
  {
    this._lightStyleBorder.Add(TableBuiltInStyles.TableStyleLight8);
    this._lightStyleBorder.Add(TableBuiltInStyles.TableStyleLight9);
    this._lightStyleBorder.Add(TableBuiltInStyles.TableStyleLight10);
    this._lightStyleBorder.Add(TableBuiltInStyles.TableStyleLight11);
    this._lightStyleBorder.Add(TableBuiltInStyles.TableStyleLight12);
    this._lightStyleBorder.Add(TableBuiltInStyles.TableStyleLight13);
    this._lightStyleBorder.Add(TableBuiltInStyles.TableStyleLight14);
  }

  private void InitializeBorders(bool isTabelStyleNewVersion)
  {
    if (isTabelStyleNewVersion)
    {
      this._borderList.Add(TableBuiltInStyles.TableStyleLight1, Color.FromArgb(0, 0, 0));
      this._borderList.Add(TableBuiltInStyles.TableStyleLight2, Color.FromArgb(68, 114, 196));
      this._borderList.Add(TableBuiltInStyles.TableStyleLight3, Color.FromArgb(237, 125, 49));
      this._borderList.Add(TableBuiltInStyles.TableStyleLight4, Color.FromArgb(165, 165, 165));
      this._borderList.Add(TableBuiltInStyles.TableStyleLight5, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 0));
      this._borderList.Add(TableBuiltInStyles.TableStyleLight6, Color.FromArgb(91, 155, 213));
      this._borderList.Add(TableBuiltInStyles.TableStyleLight7, Color.FromArgb(112 /*0x70*/, 173, 71));
    }
    else
    {
      this._borderList.Add(TableBuiltInStyles.TableStyleLight1, Color.FromArgb(0, 0, 0));
      this._borderList.Add(TableBuiltInStyles.TableStyleLight2, Color.FromArgb(79, 129, 189));
      this._borderList.Add(TableBuiltInStyles.TableStyleLight3, Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77));
      this._borderList.Add(TableBuiltInStyles.TableStyleLight4, Color.FromArgb(155, 187, 89));
      this._borderList.Add(TableBuiltInStyles.TableStyleLight5, Color.FromArgb(128 /*0x80*/, 100, 162));
      this._borderList.Add(TableBuiltInStyles.TableStyleLight6, Color.FromArgb(75, 172, 198));
      this._borderList.Add(TableBuiltInStyles.TableStyleLight7, Color.FromArgb(247, 150, 70));
    }
    this._borderList.Add(TableBuiltInStyles.TableStyleMedium16, Color.Black);
    this._borderList.Add(TableBuiltInStyles.TableStyleMedium17, Color.Black);
    this._borderList.Add(TableBuiltInStyles.TableStyleMedium18, Color.Black);
    this._borderList.Add(TableBuiltInStyles.TableStyleMedium19, Color.Black);
    this._borderList.Add(TableBuiltInStyles.TableStyleMedium20, Color.Black);
    this._borderList.Add(TableBuiltInStyles.TableStyleMedium21, Color.Black);
  }

  private void InitializeColorFonts()
  {
    this._colorFonts.Add(TableBuiltInStyles.TableStyleLight2);
    this._colorFonts.Add(TableBuiltInStyles.TableStyleLight3);
    this._colorFonts.Add(TableBuiltInStyles.TableStyleLight4);
    this._colorFonts.Add(TableBuiltInStyles.TableStyleLight5);
    this._colorFonts.Add(TableBuiltInStyles.TableStyleLight6);
    this._colorFonts.Add(TableBuiltInStyles.TableStyleLight7);
  }

  private void InitializeDoubleBorders(bool isTabelStyleNewVersion)
  {
    if (isTabelStyleNewVersion)
    {
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleLight8, Color.FromArgb(0, 0, 0));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleLight9, Color.FromArgb(68, 114, 196));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleLight10, Color.FromArgb(237, 125, 49));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleLight11, Color.FromArgb(165, 165, 165));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleLight12, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 0));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleLight13, Color.FromArgb(91, 155, 213));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleLight14, Color.FromArgb(112 /*0x70*/, 173, 71));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleLight15, Color.FromArgb(0, 0, 0));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleLight16, Color.FromArgb(68, 114, 196));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleLight17, Color.FromArgb(237, 125, 49));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleLight18, Color.FromArgb(165, 165, 165));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleLight19, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 0));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleLight20, Color.FromArgb(91, 155, 213));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleLight21, Color.FromArgb(112 /*0x70*/, 173, 71));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleMedium1, Color.FromArgb(0, 0, 0));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleMedium2, Color.FromArgb(68, 114, 196));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleMedium3, Color.FromArgb(237, 125, 49));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleMedium4, Color.FromArgb(165, 165, 165));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleMedium5, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 0));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleMedium6, Color.FromArgb(91, 155, 213));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleMedium7, Color.FromArgb(112 /*0x70*/, 173, 71));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleMedium15, Color.FromArgb(0, 0, 0));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleMedium16, Color.FromArgb(0, 0, 0));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleMedium17, Color.FromArgb(0, 0, 0));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleMedium18, Color.FromArgb(0, 0, 0));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleMedium19, Color.FromArgb(0, 0, 0));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleMedium20, Color.FromArgb(0, 0, 0));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleMedium21, Color.FromArgb(0, 0, 0));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleDark8, Color.FromArgb(0, 0, 0));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleDark9, Color.FromArgb(0, 0, 0));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleDark10, Color.FromArgb(0, 0, 0));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleDark11, Color.FromArgb(0, 0, 0));
    }
    else
    {
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleLight8, Color.FromArgb(0, 0, 0));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleLight9, Color.FromArgb(79, 129, 189));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleLight10, Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleLight11, Color.FromArgb(155, 187, 89));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleLight12, Color.FromArgb(128 /*0x80*/, 100, 162));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleLight13, Color.FromArgb(75, 172, 198));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleLight14, Color.FromArgb(247, 150, 70));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleLight15, Color.FromArgb(0, 0, 0));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleLight16, Color.FromArgb(79, 129, 189));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleLight17, Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleLight18, Color.FromArgb(155, 187, 89));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleLight19, Color.FromArgb(128 /*0x80*/, 100, 162));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleLight20, Color.FromArgb(75, 172, 198));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleLight21, Color.FromArgb(247, 150, 70));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleMedium1, Color.FromArgb(0, 0, 0));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleMedium2, Color.FromArgb(79, 129, 189));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleMedium3, Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleMedium4, Color.FromArgb(155, 187, 89));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleMedium5, Color.FromArgb(128 /*0x80*/, 100, 162));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleMedium6, Color.FromArgb(75, 172, 198));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleMedium7, Color.FromArgb(247, 150, 70));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleMedium15, Color.FromArgb(0, 0, 0));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleMedium16, Color.FromArgb(0, 0, 0));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleMedium17, Color.FromArgb(0, 0, 0));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleMedium18, Color.FromArgb(0, 0, 0));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleMedium19, Color.FromArgb(0, 0, 0));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleMedium20, Color.FromArgb(0, 0, 0));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleMedium21, Color.FromArgb(0, 0, 0));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleDark8, Color.FromArgb(0, 0, 0));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleDark9, Color.FromArgb(0, 0, 0));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleDark10, Color.FromArgb(0, 0, 0));
      this._doubleBorderList.Add(TableBuiltInStyles.TableStyleDark11, Color.FromArgb(0, 0, 0));
    }
  }

  private void InitializeTopSolid(bool isTabelStyleNewVersion)
  {
    if (isTabelStyleNewVersion)
    {
      this._topSolidList.Add(TableBuiltInStyles.TableStyleMedium22, Color.FromArgb(0, 0, 0));
      this._topSolidList.Add(TableBuiltInStyles.TableStyleMedium23, Color.FromArgb(64 /*0x40*/, 114, 196));
      this._topSolidList.Add(TableBuiltInStyles.TableStyleMedium24, Color.FromArgb(237, 125, 49));
      this._topSolidList.Add(TableBuiltInStyles.TableStyleMedium25, Color.FromArgb(165, 165, 165));
      this._topSolidList.Add(TableBuiltInStyles.TableStyleMedium26, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 0));
      this._topSolidList.Add(TableBuiltInStyles.TableStyleMedium27, Color.FromArgb(91, 155, 213));
      this._topSolidList.Add(TableBuiltInStyles.TableStyleMedium28, Color.FromArgb(112 /*0x70*/, 173, 71));
    }
    else
    {
      this._topSolidList.Add(TableBuiltInStyles.TableStyleMedium22, Color.FromArgb(0, 0, 0));
      this._topSolidList.Add(TableBuiltInStyles.TableStyleMedium23, Color.FromArgb(79, 129, 189));
      this._topSolidList.Add(TableBuiltInStyles.TableStyleMedium24, Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77));
      this._topSolidList.Add(TableBuiltInStyles.TableStyleMedium25, Color.FromArgb(155, 187, 89));
      this._topSolidList.Add(TableBuiltInStyles.TableStyleMedium26, Color.FromArgb(128 /*0x80*/, 100, 162));
      this._topSolidList.Add(TableBuiltInStyles.TableStyleMedium27, Color.FromArgb(75, 172, 198));
      this._topSolidList.Add(TableBuiltInStyles.TableStyleMedium28, Color.FromArgb(247, 150, 70));
    }
  }

  private void InitializeColumnSettings()
  {
    this._darkStyles.Add(TableBuiltInStyles.TableStyleDark1);
    this._darkStyles.Add(TableBuiltInStyles.TableStyleDark2);
    this._darkStyles.Add(TableBuiltInStyles.TableStyleDark3);
    this._darkStyles.Add(TableBuiltInStyles.TableStyleDark4);
    this._darkStyles.Add(TableBuiltInStyles.TableStyleDark5);
    this._darkStyles.Add(TableBuiltInStyles.TableStyleDark6);
    this._darkStyles.Add(TableBuiltInStyles.TableStyleDark7);
    this._mediumStyleWithBorder.Add(TableBuiltInStyles.TableStyleMedium8);
    this._mediumStyleWithBorder.Add(TableBuiltInStyles.TableStyleMedium9);
    this._mediumStyleWithBorder.Add(TableBuiltInStyles.TableStyleMedium10);
    this._mediumStyleWithBorder.Add(TableBuiltInStyles.TableStyleMedium11);
    this._mediumStyleWithBorder.Add(TableBuiltInStyles.TableStyleMedium12);
    this._mediumStyleWithBorder.Add(TableBuiltInStyles.TableStyleMedium13);
    this._mediumStyleWithBorder.Add(TableBuiltInStyles.TableStyleMedium14);
    this._mediumStyleWithoutBorder.Add(TableBuiltInStyles.TableStyleMedium15);
    this._mediumStyleWithoutBorder.Add(TableBuiltInStyles.TableStyleMedium16);
    this._mediumStyleWithoutBorder.Add(TableBuiltInStyles.TableStyleMedium17);
    this._mediumStyleWithoutBorder.Add(TableBuiltInStyles.TableStyleMedium18);
    this._mediumStyleWithoutBorder.Add(TableBuiltInStyles.TableStyleMedium19);
    this._mediumStyleWithoutBorder.Add(TableBuiltInStyles.TableStyleMedium20);
    this._mediumStyleWithoutBorder.Add(TableBuiltInStyles.TableStyleMedium21);
  }

  private void InitializeMediumStyle()
  {
    this._mediumStyleBorder.Add(TableBuiltInStyles.TableStyleMedium1);
    this._mediumStyleBorder.Add(TableBuiltInStyles.TableStyleMedium2);
    this._mediumStyleBorder.Add(TableBuiltInStyles.TableStyleMedium3);
    this._mediumStyleBorder.Add(TableBuiltInStyles.TableStyleMedium4);
    this._mediumStyleBorder.Add(TableBuiltInStyles.TableStyleMedium5);
    this._mediumStyleBorder.Add(TableBuiltInStyles.TableStyleMedium6);
    this._mediumStyleBorder.Add(TableBuiltInStyles.TableStyleMedium7);
    this._mediumStyleBorder.Add(TableBuiltInStyles.TableStyleMedium15);
  }

  private void DrawLocationAndCustomTableStyle(
    IWorksheet sheet,
    TableStyleElement tableStyleElement,
    IRange objectLocation,
    bool showTotals,
    int firstColumnStripeSize,
    int secondColumnStripeSize,
    int firstRowStripeSize,
    int secondRowStripeSize)
  {
    switch (tableStyleElement.TableStyleElementType)
    {
      case ExcelTableStyleElementType.WholeTable:
        this.WholeTableStyle(sheet, objectLocation, tableStyleElement);
        break;
      case ExcelTableStyleElementType.FirstColumnStripe:
        if (!this._columnStripes)
          break;
        this.FirstColumnStripeStyle(sheet, objectLocation, tableStyleElement, showTotals, secondColumnStripeSize);
        break;
      case ExcelTableStyleElementType.SecondColumnStripe:
        if (!this._columnStripes)
          break;
        this.SecondColumnStripeStyle(sheet, objectLocation, tableStyleElement, showTotals, firstColumnStripeSize);
        break;
      case ExcelTableStyleElementType.FirstRowStripe:
        if (!this._rowStripes)
          break;
        this.FirstRowStripeStyle(sheet, objectLocation, tableStyleElement, showTotals, secondRowStripeSize);
        break;
      case ExcelTableStyleElementType.SecondRowStripe:
        if (!this._rowStripes)
          break;
        this.SecondRowStripeStyle(sheet, objectLocation, tableStyleElement, showTotals, firstRowStripeSize);
        break;
      case ExcelTableStyleElementType.FirstColumn:
        if (!this._firstColumn)
          break;
        this.TableStyleElementStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.LastRow, objectLocation.Column].AddressLocal, tableStyleElement);
        break;
      case ExcelTableStyleElementType.LastColumn:
        if (!this._lastColumn)
          break;
        this.TableStyleElementStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.LastColumn, objectLocation.LastRow, objectLocation.LastColumn].AddressLocal, tableStyleElement);
        break;
      case ExcelTableStyleElementType.HeaderRow:
        if (!this._headerRow)
          break;
        this.TableStyleElementStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, tableStyleElement);
        break;
      case ExcelTableStyleElementType.TotalRow:
        if (!showTotals)
          break;
        this.TableStyleElementStyle(sheet, sheet.Range[objectLocation.LastRow, objectLocation.Column, objectLocation.LastRow, objectLocation.LastColumn].AddressLocal, tableStyleElement);
        break;
      case ExcelTableStyleElementType.FirstHeaderCell:
        if (!this._headerRow || !this._firstColumn)
          break;
        this.TableStyleElementStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.Column].AddressLocal, tableStyleElement);
        break;
      case ExcelTableStyleElementType.LastHeaderCell:
        if (!this._headerRow || !this._lastColumn)
          break;
        this.TableStyleElementStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.LastColumn, objectLocation.Row, objectLocation.LastColumn].AddressLocal, tableStyleElement);
        break;
      case ExcelTableStyleElementType.FirstTotalCell:
        if (!showTotals || !this._firstColumn)
          break;
        this.TableStyleElementStyle(sheet, sheet.Range[objectLocation.LastRow, objectLocation.Column, objectLocation.LastRow, objectLocation.Column].AddressLocal, tableStyleElement);
        break;
      case ExcelTableStyleElementType.LastTotalCell:
        if (!showTotals || !this._lastColumn)
          break;
        this.TableStyleElementStyle(sheet, sheet.Range[objectLocation.LastRow, objectLocation.LastColumn, objectLocation.LastRow, objectLocation.LastColumn].AddressLocal, tableStyleElement);
        break;
    }
  }

  private void WholeTableStyle(IWorksheet sheet, IRange range, TableStyleElement tableStyleElement)
  {
    this.ApplyWholeTableStyleBorder(sheet, range, tableStyleElement);
    this.ApplyFontFormatAndColor(range, tableStyleElement);
    this.ApplyBGColorAndFormat(range, tableStyleElement);
  }

  private void TableStyleElementStyle(
    IWorksheet sheet,
    string elementRange,
    TableStyleElement tableStyleElement)
  {
    IRange range = sheet.Range[elementRange];
    this.ApplyBorderFormatAndLineStyle(range, tableStyleElement);
    this.ApplyFontFormatAndColor(range, tableStyleElement);
    this.ApplyBGColorAndFormat(range, tableStyleElement);
  }

  private void FirstColumnStripeStyle(
    IWorksheet sheet,
    IRange headerRange,
    TableStyleElement tableStyleElement,
    bool showTotals,
    int secondColumnStripeSize)
  {
    int row = headerRange.Row;
    int lastRow = headerRange.LastRow;
    if (this._headerRow)
      ++row;
    if (showTotals)
      --lastRow;
    IRange range1 = sheet.Range[row, headerRange.Column, lastRow, headerRange.LastColumn];
    int num = 1;
    if (secondColumnStripeSize > 1)
      num = secondColumnStripeSize;
    int lastColumn;
    for (int column = range1.Column; column <= range1.LastColumn; column = lastColumn + num + 1)
    {
      lastColumn = column;
      if (tableStyleElement.StripeSize > 1)
        lastColumn += tableStyleElement.StripeSize - 1;
      if (lastColumn > range1.LastColumn)
        lastColumn = range1.LastColumn;
      IRange range2 = sheet.Range[range1.Row, column, range1.LastRow, lastColumn];
      this.ApplyBorderFormatAndLineStyle(range2, tableStyleElement);
      this.ApplyFontFormatAndColor(range2, tableStyleElement);
      this.ApplyBGColorAndFormat(range2, tableStyleElement);
    }
  }

  private void SecondColumnStripeStyle(
    IWorksheet sheet,
    IRange headerRange,
    TableStyleElement tableStyleElement,
    bool showTotals,
    int firstColumnStripeSize)
  {
    int row = headerRange.Row;
    int lastRow = headerRange.LastRow;
    if (this._headerRow)
      ++row;
    if (showTotals)
      --lastRow;
    IRange range1 = sheet.Range[row, headerRange.Column, lastRow, headerRange.LastColumn];
    int num = 1;
    if (firstColumnStripeSize > 1)
      num = firstColumnStripeSize;
    int lastColumn;
    for (int column = range1.Column + num; column <= range1.LastColumn; column = lastColumn + num + 1)
    {
      lastColumn = column;
      if (tableStyleElement.StripeSize > 1)
        lastColumn += tableStyleElement.StripeSize - 1;
      if (lastColumn > range1.LastColumn)
        lastColumn = range1.LastColumn;
      IRange range2 = sheet.Range[range1.Row, column, range1.LastRow, lastColumn];
      this.ApplyBorderFormatAndLineStyle(range2, tableStyleElement);
      this.ApplyFontFormatAndColor(range2, tableStyleElement);
      this.ApplyBGColorAndFormat(range2, tableStyleElement);
    }
  }

  private void FirstRowStripeStyle(
    IWorksheet sheet,
    IRange headerRange,
    TableStyleElement tableStyleElement,
    bool showTotals,
    int secondRowStripeSize)
  {
    int row1 = headerRange.Row;
    int lastRow1 = headerRange.LastRow;
    if (this._headerRow)
      ++row1;
    if (showTotals)
      --lastRow1;
    IRange range1 = sheet.Range[row1, headerRange.Column, lastRow1, headerRange.LastColumn];
    int num = 1;
    if (secondRowStripeSize > 1)
      num = secondRowStripeSize;
    int lastRow2;
    for (int row2 = range1.Row; row2 <= range1.LastRow; row2 = lastRow2 + num + 1)
    {
      lastRow2 = row2;
      if (tableStyleElement.StripeSize > 1)
        lastRow2 += tableStyleElement.StripeSize - 1;
      if (lastRow2 > range1.LastRow)
        lastRow2 = range1.LastRow;
      IRange range2 = sheet.Range[row2, range1.Column, lastRow2, range1.LastColumn];
      this.ApplyBorderFormatAndLineStyle(range2, tableStyleElement);
      this.ApplyFontFormatAndColor(range2, tableStyleElement);
      this.ApplyBGColorAndFormat(range2, tableStyleElement);
    }
  }

  private void SecondRowStripeStyle(
    IWorksheet sheet,
    IRange headerRange,
    TableStyleElement tableStyleElement,
    bool showTotals,
    int firstRowStripeSize)
  {
    int row1 = headerRange.Row;
    int lastRow1 = headerRange.LastRow;
    if (this._headerRow)
      ++row1;
    if (showTotals)
      --lastRow1;
    IRange range1 = sheet.Range[row1, headerRange.Column, lastRow1, headerRange.LastColumn];
    int num = 1;
    if (firstRowStripeSize > 1)
      num = firstRowStripeSize;
    int lastRow2;
    for (int row2 = range1.Row + num; row2 <= range1.LastRow; row2 = lastRow2 + num + 1)
    {
      lastRow2 = row2;
      if (tableStyleElement.StripeSize > 1)
        lastRow2 += tableStyleElement.StripeSize - 1;
      if (lastRow2 > range1.LastRow)
        lastRow2 = range1.LastRow;
      IRange range2 = sheet.Range[row2, range1.Column, lastRow2, range1.LastColumn];
      this.ApplyBorderFormatAndLineStyle(range2, tableStyleElement);
      this.ApplyFontFormatAndColor(range2, tableStyleElement);
      this.ApplyBGColorAndFormat(range2, tableStyleElement);
    }
  }

  private void DrawLocationAndStyle(
    IWorksheet sheet,
    TableBuiltInStyles listObjectBuiltInStyle,
    IRange objectLocation,
    bool showTotals)
  {
    this._builtInStyle = listObjectBuiltInStyle;
    WorkbookImpl workbook = sheet.Workbook as WorkbookImpl;
    bool flag = workbook.DefaultThemeVersion == "166925" || workbook.DefaultThemeVersion == "164011" || workbook.DefaultThemeVersion == "153222";
    switch (this._builtInStyle)
    {
      case TableBuiltInStyles.None:
        break;
      case TableBuiltInStyles.TableStyleMedium28:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(169, 208 /*0xD0*/, 142), false, Color.FromArgb(226, 239, 218), TableStyleRenderer.SolidStyle.None, Color.Black);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(169, 208 /*0xD0*/, 142), false, Color.FromArgb(226, 239, 218), TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(169, 208 /*0xD0*/, 142), false, Color.FromArgb(198, 224 /*0xE0*/, 180), Color.FromArgb(226, 239, 218), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(250, 191, 143), false, Color.FromArgb(253, 233, 217), TableStyleRenderer.SolidStyle.None, Color.Black);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(250, 191, 143), false, Color.FromArgb(253, 233, 217), TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(250, 191, 143), false, Color.FromArgb(252, 213, 180), Color.FromArgb(253, 233, 217), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        break;
      case TableBuiltInStyles.TableStyleMedium27:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(155, 194, 230), false, Color.FromArgb(221, 235, 247), TableStyleRenderer.SolidStyle.None, Color.Black);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(155, 194, 230), false, Color.FromArgb(221, 235, 247), TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(155, 194, 230), false, Color.FromArgb(189, 215, 238), Color.FromArgb(221, 235, 247), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(146, 205, 220), false, Color.FromArgb(218, 238, 243), TableStyleRenderer.SolidStyle.None, Color.Black);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(146, 205, 220), false, Color.FromArgb(218, 238, 243), TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(146, 205, 220), false, Color.FromArgb(183, 222, 232), Color.FromArgb(218, 238, 243), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        break;
      case TableBuiltInStyles.TableStyleMedium26:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, 217, 102), false, Color.FromArgb((int) byte.MaxValue, 242, 204), TableStyleRenderer.SolidStyle.None, Color.Black);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, 217, 102), false, Color.FromArgb((int) byte.MaxValue, 242, 204), TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, 217, 102), false, Color.FromArgb((int) byte.MaxValue, 230, 153), Color.FromArgb((int) byte.MaxValue, 242, 204), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(177, 160 /*0xA0*/, 199), false, Color.FromArgb(228, 223, 236), TableStyleRenderer.SolidStyle.None, Color.Black);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(177, 160 /*0xA0*/, 199), false, Color.FromArgb(228, 223, 236), TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(177, 160 /*0xA0*/, 199), false, Color.FromArgb(204, 192 /*0xC0*/, 218), Color.FromArgb(228, 223, 236), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        break;
      case TableBuiltInStyles.TableStyleMedium25:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(201, 201, 201), false, Color.FromArgb(237, 237, 237), TableStyleRenderer.SolidStyle.None, Color.Black);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(201, 201, 201), false, Color.FromArgb(237, 237, 237), TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(201, 201, 201), false, Color.FromArgb(219, 219, 219), Color.FromArgb(237, 237, 237), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(196, 215, 155), false, Color.FromArgb(235, 241, 222), TableStyleRenderer.SolidStyle.None, Color.Black);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(196, 215, 155), false, Color.FromArgb(235, 241, 222), TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(196, 215, 155), false, Color.FromArgb(216, 228, 188), Color.FromArgb(235, 241, 222), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        break;
      case TableBuiltInStyles.TableStyleMedium24:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(244, 176 /*0xB0*/, 132), false, Color.FromArgb(252, 228, 214), TableStyleRenderer.SolidStyle.None, Color.Black);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(244, 176 /*0xB0*/, 132), false, Color.FromArgb(252, 228, 214), TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(244, 176 /*0xB0*/, 132), false, Color.FromArgb(248, 203, 173), Color.FromArgb(252, 228, 214), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(218, 150, 148), false, Color.FromArgb(242, 220, 219), TableStyleRenderer.SolidStyle.None, Color.Black);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(218, 150, 148), false, Color.FromArgb(242, 220, 219), TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(218, 150, 148), false, Color.FromArgb(230, 184, 183), Color.FromArgb(242, 220, 219), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        break;
      case TableBuiltInStyles.TableStyleMedium23:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(142, 169, 219), false, Color.FromArgb(217, 225, 242), TableStyleRenderer.SolidStyle.None, Color.Black);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(142, 169, 219), false, Color.FromArgb(217, 225, 242), TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(142, 169, 219), false, Color.FromArgb(180, 198, 231), Color.FromArgb(217, 225, 242), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(149, 179, 215), false, Color.FromArgb(220, 230, 241), TableStyleRenderer.SolidStyle.None, Color.Black);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(149, 179, 215), false, Color.FromArgb(220, 230, 241), TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(149, 179, 215), false, Color.FromArgb(184, 201, 228), Color.FromArgb(220, 230, 241), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        break;
      case TableBuiltInStyles.TableStyleMedium22:
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(0, 0, 0), false, Color.FromArgb(217, 217, 217), TableStyleRenderer.SolidStyle.None, Color.Black);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(0, 0, 0), false, Color.FromArgb(217, 217, 217), TableStyleRenderer.SolidStyle.Top, showTotals, Color.Black, true);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(0, 0, 0), false, Color.FromArgb(166, 166, 166), Color.FromArgb(217, 217, 217), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        break;
      case TableBuiltInStyles.TableStyleMedium21:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(0, 0, 0), true, Color.FromArgb(112 /*0x70*/, 173, 71), TableStyleRenderer.SolidStyle.TopBottom, Color.White);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(217, 217, 217), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(0, 0, 0), true, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.Bottom, showTotals, Color.Black, true);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(0, 0, 0), true, Color.FromArgb(247, 150, 70), TableStyleRenderer.SolidStyle.TopBottom, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(217, 217, 217), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(0, 0, 0), true, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.Bottom, showTotals, Color.Black, true);
        break;
      case TableBuiltInStyles.TableStyleMedium20:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(0, 0, 0), true, Color.FromArgb(91, 155, 213), TableStyleRenderer.SolidStyle.TopBottom, Color.White);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(217, 217, 217), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(0, 0, 0), true, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.Bottom, showTotals, Color.Black, false);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(0, 0, 0), true, Color.FromArgb(75, 172, 198), TableStyleRenderer.SolidStyle.TopBottom, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(217, 217, 217), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(0, 0, 0), true, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.Bottom, showTotals, Color.Black, false);
        break;
      case TableBuiltInStyles.TableStyleMedium19:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(0, 0, 0), true, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 0), TableStyleRenderer.SolidStyle.TopBottom, Color.White);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(217, 217, 217), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(0, 0, 0), true, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.Bottom, showTotals, Color.Black, false);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(0, 0, 0), true, Color.FromArgb(128 /*0x80*/, 100, 162), TableStyleRenderer.SolidStyle.TopBottom, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(217, 217, 217), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(0, 0, 0), true, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.Bottom, showTotals, Color.Black, false);
        break;
      case TableBuiltInStyles.TableStyleMedium18:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(0, 0, 0), true, Color.FromArgb(165, 165, 165), TableStyleRenderer.SolidStyle.TopBottom, Color.White);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(217, 217, 217), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(0, 0, 0), true, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.Bottom, showTotals, Color.Black, false);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(0, 0, 0), true, Color.FromArgb(155, 187, 89), TableStyleRenderer.SolidStyle.TopBottom, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(217, 217, 217), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(0, 0, 0), true, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.Bottom, showTotals, Color.Black, false);
        break;
      case TableBuiltInStyles.TableStyleMedium17:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(0, 0, 0), true, Color.FromArgb(237, 125, 49), TableStyleRenderer.SolidStyle.TopBottom, Color.White);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(217, 217, 217), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(0, 0, 0), true, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.Bottom, showTotals, Color.Black, false);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(0, 0, 0), true, Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77), TableStyleRenderer.SolidStyle.TopBottom, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(217, 217, 217), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(0, 0, 0), true, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.Bottom, showTotals, Color.Black, false);
        break;
      case TableBuiltInStyles.TableStyleMedium16:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(0, 0, 0), true, Color.FromArgb(68, 114, 196), TableStyleRenderer.SolidStyle.TopBottom, Color.White);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(217, 217, 217), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(0, 0, 0), true, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.Bottom, showTotals, Color.Black, false);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(0, 0, 0), true, Color.FromArgb(79, 129, 189), TableStyleRenderer.SolidStyle.TopBottom, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(217, 217, 217), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(0, 0, 0), true, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.Bottom, showTotals, Color.Black, false);
        break;
      case TableBuiltInStyles.TableStyleMedium15:
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(0, 0, 0), false, Color.FromArgb(0, 0, 0), TableStyleRenderer.SolidStyle.TopBottom, Color.White);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(0, 0, 0), false, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.Bottom, showTotals, Color.Black, false);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.Black, false, Color.FromArgb(217, 217, 217), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        break;
      case TableBuiltInStyles.TableStyleMedium14:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), false, Color.FromArgb(112 /*0x70*/, 173, 71), TableStyleRenderer.SolidStyle.Bottom, Color.White);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.White, false, Color.FromArgb(198, 224 /*0xE0*/, 180), Color.FromArgb(226, 239, 218), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), false, Color.FromArgb(112 /*0x70*/, 173, 71), TableStyleRenderer.SolidStyle.Top, showTotals, Color.White, true);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), false, Color.FromArgb(247, 150, 70), TableStyleRenderer.SolidStyle.Bottom, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.White, false, Color.FromArgb(252, 213, 180), Color.FromArgb(253, 233, 217), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), false, Color.FromArgb(247, 150, 70), TableStyleRenderer.SolidStyle.Top, showTotals, Color.White, true);
        break;
      case TableBuiltInStyles.TableStyleMedium13:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), false, Color.FromArgb(91, 155, 213), TableStyleRenderer.SolidStyle.Bottom, Color.White);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.White, false, Color.FromArgb(189, 215, 238), Color.FromArgb(221, 235, 247), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), false, Color.FromArgb(91, 155, 213), TableStyleRenderer.SolidStyle.Top, showTotals, Color.White, true);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), false, Color.FromArgb(75, 172, 198), TableStyleRenderer.SolidStyle.Bottom, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.White, false, Color.FromArgb(183, 222, 232), Color.FromArgb(218, 238, 243), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), false, Color.FromArgb(75, 172, 198), TableStyleRenderer.SolidStyle.Top, showTotals, Color.White, true);
        break;
      case TableBuiltInStyles.TableStyleMedium12:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), false, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 0), TableStyleRenderer.SolidStyle.Bottom, Color.White);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.White, false, Color.FromArgb((int) byte.MaxValue, 230, 153), Color.FromArgb((int) byte.MaxValue, 242, 204), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), false, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 0), TableStyleRenderer.SolidStyle.Top, showTotals, Color.White, true);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), false, Color.FromArgb(128 /*0x80*/, 100, 162), TableStyleRenderer.SolidStyle.Bottom, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.White, false, Color.FromArgb(204, 192 /*0xC0*/, 218), Color.FromArgb(228, 223, 236), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), false, Color.FromArgb(128 /*0x80*/, 100, 162), TableStyleRenderer.SolidStyle.Top, showTotals, Color.White, true);
        break;
      case TableBuiltInStyles.TableStyleMedium11:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), false, Color.FromArgb(165, 165, 165), TableStyleRenderer.SolidStyle.Bottom, Color.White);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.White, false, Color.FromArgb(219, 219, 219), Color.FromArgb(237, 237, 237), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), false, Color.FromArgb(165, 165, 165), TableStyleRenderer.SolidStyle.Top, showTotals, Color.White, true);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), false, Color.FromArgb(155, 187, 89), TableStyleRenderer.SolidStyle.Bottom, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.White, false, Color.FromArgb(216, 228, 188), Color.FromArgb(235, 241, 222), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), false, Color.FromArgb(155, 187, 89), TableStyleRenderer.SolidStyle.Top, showTotals, Color.White, true);
        break;
      case TableBuiltInStyles.TableStyleMedium10:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), false, Color.FromArgb(237, 125, 49), TableStyleRenderer.SolidStyle.Bottom, Color.White);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.White, false, Color.FromArgb(248, 203, 173), Color.FromArgb(252, 228, 214), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), false, Color.FromArgb(237, 125, 49), TableStyleRenderer.SolidStyle.Top, showTotals, Color.White, true);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), false, Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77), TableStyleRenderer.SolidStyle.Bottom, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.White, false, Color.FromArgb(230, 184, 183), Color.FromArgb(242, 220, 219), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), false, Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77), TableStyleRenderer.SolidStyle.Top, showTotals, Color.White, true);
        break;
      case TableBuiltInStyles.TableStyleMedium9:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), false, Color.FromArgb(68, 114, 196), TableStyleRenderer.SolidStyle.Bottom, Color.White);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.White, false, Color.FromArgb(180, 198, 231), Color.FromArgb(217, 225, 242), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), false, Color.FromArgb(68, 114, 196), TableStyleRenderer.SolidStyle.Top, showTotals, Color.White, true);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), false, Color.FromArgb(79, 129, 189), TableStyleRenderer.SolidStyle.Bottom, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.White, false, Color.FromArgb(184, 204, 228), Color.FromArgb(220, 230, 241), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), false, Color.FromArgb(79, 129, 189), TableStyleRenderer.SolidStyle.Top, showTotals, Color.White, true);
        break;
      case TableBuiltInStyles.TableStyleMedium8:
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), false, Color.FromArgb(0, 0, 0), TableStyleRenderer.SolidStyle.Bottom, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.White, false, Color.FromArgb(166, 166, 166), Color.FromArgb(217, 217, 217), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), false, Color.FromArgb(0, 0, 0), TableStyleRenderer.SolidStyle.Top, showTotals, Color.White, true);
        break;
      case TableBuiltInStyles.TableStyleMedium7:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(169, 208 /*0xD0*/, 142), true, Color.FromArgb(112 /*0x70*/, 173, 71), TableStyleRenderer.SolidStyle.None, Color.White);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(169, 208 /*0xD0*/, 142), true, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(169, 208 /*0xD0*/, 142), true, Color.FromArgb(226, 239, 218), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(250, 191, 143), true, Color.FromArgb(247, 150, 70), TableStyleRenderer.SolidStyle.None, Color.White);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(250, 191, 143), true, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(250, 191, 143), true, Color.FromArgb(253, 233, 217), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        break;
      case TableBuiltInStyles.TableStyleMedium6:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(155, 194, 230), true, Color.FromArgb(91, 155, 213), TableStyleRenderer.SolidStyle.None, Color.White);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(155, 194, 230), true, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(155, 194, 230), true, Color.FromArgb(221, 235, 247), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(146, 205, 220), true, Color.FromArgb(75, 172, 198), TableStyleRenderer.SolidStyle.None, Color.White);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(146, 205, 220), true, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(146, 205, 220), true, Color.FromArgb(218, 238, 243), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        break;
      case TableBuiltInStyles.TableStyleMedium5:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, 217, 102), true, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 0), TableStyleRenderer.SolidStyle.None, Color.White);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, 217, 102), true, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, 217, 102), true, Color.FromArgb((int) byte.MaxValue, 242, 204), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(177, 160 /*0xA0*/, 199), true, Color.FromArgb(128 /*0x80*/, 100, 162), TableStyleRenderer.SolidStyle.None, Color.White);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(177, 160 /*0xA0*/, 199), true, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(177, 160 /*0xA0*/, 199), true, Color.FromArgb(228, 223, 236), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        break;
      case TableBuiltInStyles.TableStyleMedium4:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(201, 201, 201), true, Color.FromArgb(165, 165, 165), TableStyleRenderer.SolidStyle.None, Color.White);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(201, 201, 201), true, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(201, 201, 201), true, Color.FromArgb(237, 237, 237), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(196, 215, 155), true, Color.FromArgb(155, 187, 89), TableStyleRenderer.SolidStyle.None, Color.White);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(196, 215, 155), true, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(196, 215, 155), true, Color.FromArgb(235, 241, 222), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        break;
      case TableBuiltInStyles.TableStyleMedium3:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(244, 176 /*0xB0*/, 132), true, Color.FromArgb(237, 125, 49), TableStyleRenderer.SolidStyle.None, Color.White);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(244, 176 /*0xB0*/, 132), true, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(244, 176 /*0xB0*/, 132), true, Color.FromArgb(252, 228, 214), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(218, 150, 148), true, Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77), TableStyleRenderer.SolidStyle.None, Color.White);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(218, 150, 148), true, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(218, 150, 148), true, Color.FromArgb(242, 220, 219), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        break;
      case TableBuiltInStyles.TableStyleMedium2:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(142, 169, 219), true, Color.FromArgb(68, 114, 196), TableStyleRenderer.SolidStyle.None, Color.White);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(142, 169, 219), true, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(142, 169, 219), true, Color.FromArgb(217, 225, 242), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(149, 179, 215), true, Color.FromArgb(79, 129, 189), TableStyleRenderer.SolidStyle.None, Color.White);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(149, 179, 215), true, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(149, 179, 215), true, Color.FromArgb(220, 230, 241), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        break;
      case TableBuiltInStyles.TableStyleMedium1:
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(0, 0, 0), true, Color.FromArgb(0, 0, 0), TableStyleRenderer.SolidStyle.None, Color.White);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(0, 0, 0), true, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(0, 0, 0), true, Color.FromArgb(217, 217, 217), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        break;
      case TableBuiltInStyles.TableStyleLight21:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(112 /*0x70*/, 173, 71), false, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.Bottom, Color.Black);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(112 /*0x70*/, 173, 71), false, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(112 /*0x70*/, 173, 71), false, Color.FromArgb(226, 239, 218), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(247, 150, 70), false, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.Bottom, Color.Black);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(247, 150, 70), false, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(247, 150, 70), false, Color.FromArgb(253, 233, 217), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        break;
      case TableBuiltInStyles.TableStyleLight20:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(91, 155, 213), false, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.Bottom, Color.Black);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(91, 155, 213), false, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(91, 155, 213), false, Color.FromArgb(221, 235, 247), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(75, 172, 198), false, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.Bottom, Color.Black);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(75, 172, 198), false, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(75, 172, 198), false, Color.FromArgb(218, 238, 243), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        break;
      case TableBuiltInStyles.TableStyleLight19:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 0), false, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.Bottom, Color.Black);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 0), false, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 0), false, Color.FromArgb((int) byte.MaxValue, 242, 204), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(128 /*0x80*/, 100, 162), false, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.Bottom, Color.Black);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(128 /*0x80*/, 100, 162), false, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(128 /*0x80*/, 100, 162), false, Color.FromArgb(228, 223, 236), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        break;
      case TableBuiltInStyles.TableStyleLight18:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(165, 165, 165), false, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.Bottom, Color.Black);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(165, 165, 165), false, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(165, 165, 165), false, Color.FromArgb(237, 237, 237), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(155, 187, 89), false, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.Bottom, Color.Black);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(155, 187, 89), false, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(155, 187, 89), false, Color.FromArgb(235, 241, 222), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        break;
      case TableBuiltInStyles.TableStyleLight17:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(237, 125, 49), false, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.Bottom, Color.Black);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(237, 125, 49), false, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(237, 125, 49), false, Color.FromArgb(252, 228, 214), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77), false, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.Bottom, Color.Black);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77), false, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77), false, Color.FromArgb(242, 220, 219), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        break;
      case TableBuiltInStyles.TableStyleLight16:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(68, 114, 196), false, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.Bottom, Color.Black);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(68, 114, 196), false, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(68, 114, 196), false, Color.FromArgb(217, 225, 242), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(79, 129, 189), false, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.Bottom, Color.Black);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(79, 129, 189), false, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(79, 129, 189), false, Color.FromArgb(220, 230, 241), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        break;
      case TableBuiltInStyles.TableStyleLight15:
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(0, 0, 0), false, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.None, Color.Black);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(0, 0, 0), false, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(0, 0, 0), false, Color.FromArgb(217, 217, 217), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        break;
      case TableBuiltInStyles.TableStyleLight14:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(112 /*0x70*/, 173, 71), true, Color.FromArgb(112 /*0x70*/, 173, 71), TableStyleRenderer.SolidStyle.None, Color.White);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(112 /*0x70*/, 173, 71), true, Color.Empty, Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(112 /*0x70*/, 173, 71), true, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(247, 150, 70), true, Color.FromArgb(247, 150, 70), TableStyleRenderer.SolidStyle.None, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(247, 150, 70), true, Color.Empty, Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(247, 150, 70), true, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        break;
      case TableBuiltInStyles.TableStyleLight13:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(91, 155, 213), true, Color.FromArgb(91, 155, 213), TableStyleRenderer.SolidStyle.None, Color.White);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(91, 155, 213), true, Color.Empty, Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(91, 155, 213), true, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(75, 172, 198), true, Color.FromArgb(75, 172, 198), TableStyleRenderer.SolidStyle.None, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(75, 172, 198), true, Color.Empty, Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(75, 172, 198), true, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        break;
      case TableBuiltInStyles.TableStyleLight12:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 0), true, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 0), TableStyleRenderer.SolidStyle.None, Color.White);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 0), true, Color.Empty, Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 0), true, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(128 /*0x80*/, 100, 162), true, Color.FromArgb(128 /*0x80*/, 100, 162), TableStyleRenderer.SolidStyle.None, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(128 /*0x80*/, 100, 162), true, Color.Empty, Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(128 /*0x80*/, 100, 162), true, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        break;
      case TableBuiltInStyles.TableStyleLight11:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(165, 165, 165), true, Color.FromArgb(165, 165, 165), TableStyleRenderer.SolidStyle.None, Color.White);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(165, 165, 165), true, Color.Empty, Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(165, 165, 165), true, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(155, 187, 89), true, Color.FromArgb(155, 187, 89), TableStyleRenderer.SolidStyle.None, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(155, 187, 89), true, Color.Empty, Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(155, 187, 89), true, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        break;
      case TableBuiltInStyles.TableStyleLight10:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(237, 125, 49), true, Color.FromArgb(237, 125, 49), TableStyleRenderer.SolidStyle.None, Color.White);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(237, 125, 49), true, Color.Empty, Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(237, 125, 49), true, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77), true, Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77), TableStyleRenderer.SolidStyle.None, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77), true, Color.Empty, Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77), true, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        break;
      case TableBuiltInStyles.TableStyleLight9:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(68, 114, 196), true, Color.FromArgb(68, 114, 196), TableStyleRenderer.SolidStyle.None, Color.White);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(68, 114, 196), true, Color.Empty, Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(68, 114, 196), true, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(79, 129, 189), true, Color.FromArgb(79, 129, 189), TableStyleRenderer.SolidStyle.None, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(79, 129, 189), true, Color.Empty, Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(79, 129, 189), true, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        break;
      case TableBuiltInStyles.TableStyleLight8:
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(0, 0, 0), true, Color.Black, TableStyleRenderer.SolidStyle.None, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.Black, true, Color.Empty, Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(0, 0, 0), true, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        break;
      case TableBuiltInStyles.TableStyleLight7:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(112 /*0x70*/, 173, 71), true, Color.Empty, TableStyleRenderer.SolidStyle.None, Color.FromArgb(84, 130, 53));
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, true, Color.FromArgb(226, 239, 218), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.FromArgb(84, 130, 53), showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(112 /*0x70*/, 173, 71), true, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.FromArgb(84, 130, 53), true);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(247, 150, 70), true, Color.Empty, TableStyleRenderer.SolidStyle.None, Color.FromArgb(226, 107, 10));
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, true, Color.FromArgb(253, 233, 217), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.FromArgb(226, 107, 10), showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(247, 150, 70), true, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.FromArgb(226, 107, 10), true);
        break;
      case TableBuiltInStyles.TableStyleLight6:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(91, 155, 213), true, Color.Empty, TableStyleRenderer.SolidStyle.None, Color.FromArgb(47, 117, 181));
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, true, Color.FromArgb(221, 235, 247), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.FromArgb(47, 117, 181), showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(91, 155, 213), true, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.None, showTotals, Color.FromArgb(47, 117, 181), true);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(75, 172, 198), true, Color.Empty, TableStyleRenderer.SolidStyle.None, Color.FromArgb(49, 134, 155));
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, true, Color.FromArgb(218, 238, 243), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.FromArgb(49, 134, 155), showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(75, 172, 198), true, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.None, showTotals, Color.FromArgb(49, 134, 155), true);
        break;
      case TableBuiltInStyles.TableStyleLight5:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 0), true, Color.Empty, TableStyleRenderer.SolidStyle.None, Color.FromArgb(191, 143, 0));
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, true, Color.FromArgb((int) byte.MaxValue, 242, 204), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.FromArgb(191, 143, 0), showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 0), true, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.None, showTotals, Color.FromArgb(191, 143, 0), true);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(128 /*0x80*/, 100, 162), true, Color.Empty, TableStyleRenderer.SolidStyle.None, Color.FromArgb(96 /*0x60*/, 73, 122));
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, true, Color.FromArgb(228, 223, 236), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.FromArgb(96 /*0x60*/, 73, 122), showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(128 /*0x80*/, 100, 162), true, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.None, showTotals, Color.FromArgb(96 /*0x60*/, 73, 122), true);
        break;
      case TableBuiltInStyles.TableStyleLight4:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(165, 165, 165), true, Color.Empty, TableStyleRenderer.SolidStyle.None, Color.FromArgb(123, 123, 123));
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, true, Color.FromArgb(237, 237, 237), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.FromArgb(123, 123, 123), showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(165, 165, 165), true, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.None, showTotals, Color.FromArgb(123, 123, 123), true);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(155, 187, 89), true, Color.Empty, TableStyleRenderer.SolidStyle.None, Color.FromArgb(118, 147, 60));
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, true, Color.FromArgb(235, 241, 222), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.FromArgb(118, 147, 60), showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(155, 187, 89), true, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.None, showTotals, Color.FromArgb(118, 147, 60), true);
        break;
      case TableBuiltInStyles.TableStyleLight3:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(237, 125, 49), true, Color.Empty, TableStyleRenderer.SolidStyle.None, Color.FromArgb(198, 89, 17));
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, true, Color.FromArgb(252, 228, 214), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.FromArgb(198, 89, 17), showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(237, 125, 49), true, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.None, showTotals, Color.FromArgb(198, 89, 17), true);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77), true, Color.Empty, TableStyleRenderer.SolidStyle.None, Color.FromArgb(150, 54, 52));
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, true, Color.FromArgb(242, 220, 219), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.FromArgb(150, 54, 52), showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77), true, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.None, showTotals, Color.FromArgb(150, 54, 52), true);
        break;
      case TableBuiltInStyles.TableStyleLight2:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(68, 114, 196), true, Color.Empty, TableStyleRenderer.SolidStyle.None, Color.FromArgb(48 /*0x30*/, 84, 150));
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, true, Color.FromArgb(217, 225, 242), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.FromArgb(48 /*0x30*/, 84, 150), showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(68, 114, 196), true, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.None, showTotals, Color.FromArgb(48 /*0x30*/, 84, 150), true);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(79, 129, 189), true, Color.Empty, TableStyleRenderer.SolidStyle.None, Color.FromArgb(54, 96 /*0x60*/, 146));
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, true, Color.FromArgb(220, 230, 241), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.FromArgb(54, 96 /*0x60*/, 146), showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(79, 129, 189), true, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.None, showTotals, Color.FromArgb(54, 96 /*0x60*/, 146), true);
        break;
      case TableBuiltInStyles.TableStyleLight1:
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.TopBottom, Color.Black, true, Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, true, Color.FromArgb(217, 217, 217), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.TopBottom, Color.FromArgb(0, 0, 0), true, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        break;
      case TableBuiltInStyles.TableStyleDark11:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.None, Color.Empty, true, Color.FromArgb(112 /*0x70*/, 173, 71), TableStyleRenderer.SolidStyle.None, Color.White);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(189, 215, 238), Color.FromArgb(221, 235, 247), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.Top, Color.FromArgb(0, 0, 0), true, Color.FromArgb(221, 235, 247), TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.None, Color.Empty, true, Color.FromArgb(247, 150, 70), TableStyleRenderer.SolidStyle.None, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(183, 222, 232), Color.FromArgb(218, 238, 243), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.Top, Color.FromArgb(0, 0, 0), true, Color.FromArgb(218, 238, 243), TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        break;
      case TableBuiltInStyles.TableStyleDark10:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.None, Color.Empty, true, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 0), TableStyleRenderer.SolidStyle.None, Color.White);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(219, 219, 219), Color.FromArgb(237, 237, 237), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.Top, Color.FromArgb(0, 0, 0), true, Color.FromArgb(237, 237, 237), TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.None, Color.Empty, true, Color.FromArgb(128 /*0x80*/, 100, 162), TableStyleRenderer.SolidStyle.None, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(216, 228, 188), Color.FromArgb(235, 241, 222), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.Top, Color.FromArgb(0, 0, 0), true, Color.FromArgb(235, 241, 222), TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        break;
      case TableBuiltInStyles.TableStyleDark9:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.None, Color.Empty, true, Color.FromArgb(237, 125, 49), TableStyleRenderer.SolidStyle.None, Color.White);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(180, 198, 231), Color.FromArgb(217, 225, 242), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.Top, Color.FromArgb(0, 0, 0), true, Color.FromArgb(217, 225, 242), TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.None, Color.Empty, true, Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77), TableStyleRenderer.SolidStyle.None, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(184, 204, 228), Color.FromArgb(220, 230, 241), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.Top, Color.FromArgb(0, 0, 0), true, Color.FromArgb(220, 230, 241), TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        break;
      case TableBuiltInStyles.TableStyleDark8:
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.None, Color.Empty, true, Color.FromArgb(0, 0, 0), TableStyleRenderer.SolidStyle.None, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(166, 166, 166), Color.FromArgb(217, 217, 217), TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.Top, Color.FromArgb(0, 0, 0), true, Color.FromArgb(217, 217, 217), TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        break;
      case TableBuiltInStyles.TableStyleDark7:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.Bottom, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), true, Color.FromArgb(0, 0, 0), TableStyleRenderer.SolidStyle.Bottom, Color.White);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(84, 130, 53), Color.FromArgb(112 /*0x70*/, 173, 71), TableStyleRenderer.SolidStyle.None, Color.White, showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.Top, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), true, Color.FromArgb(55, 86, 35), TableStyleRenderer.SolidStyle.Top, showTotals, Color.White, true);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.Bottom, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), true, Color.FromArgb(0, 0, 0), TableStyleRenderer.SolidStyle.Bottom, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(226, 107, 10), Color.FromArgb(247, 150, 70), TableStyleRenderer.SolidStyle.None, Color.White, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.Top, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), true, Color.FromArgb(151, 71, 6), TableStyleRenderer.SolidStyle.Top, showTotals, Color.White, true);
        break;
      case TableBuiltInStyles.TableStyleDark6:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.Bottom, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), true, Color.FromArgb(0, 0, 0), TableStyleRenderer.SolidStyle.Bottom, Color.White);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(47, 117, 181), Color.FromArgb(91, 155, 213), TableStyleRenderer.SolidStyle.None, Color.White, showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.Top, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), true, Color.FromArgb(31 /*0x1F*/, 78, 120), TableStyleRenderer.SolidStyle.Top, showTotals, Color.White, true);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.Bottom, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), true, Color.FromArgb(0, 0, 0), TableStyleRenderer.SolidStyle.Bottom, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(49, 134, 155), Color.FromArgb(75, 172, 198), TableStyleRenderer.SolidStyle.None, Color.White, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.Top, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), true, Color.FromArgb(33, 89, 103), TableStyleRenderer.SolidStyle.Top, showTotals, Color.White, true);
        break;
      case TableBuiltInStyles.TableStyleDark5:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.Bottom, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), true, Color.FromArgb(0, 0, 0), TableStyleRenderer.SolidStyle.Bottom, Color.White);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(191, 143, 0), Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 0), TableStyleRenderer.SolidStyle.None, Color.White, showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.Top, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), true, Color.FromArgb(128 /*0x80*/, 96 /*0x60*/, 0), TableStyleRenderer.SolidStyle.Top, showTotals, Color.White, true);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.Bottom, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), true, Color.FromArgb(0, 0, 0), TableStyleRenderer.SolidStyle.Bottom, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(96 /*0x60*/, 73, 122), Color.FromArgb(128 /*0x80*/, 100, 162), TableStyleRenderer.SolidStyle.None, Color.White, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.Top, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), true, Color.FromArgb(64 /*0x40*/, 49, 81), TableStyleRenderer.SolidStyle.Top, showTotals, Color.White, true);
        break;
      case TableBuiltInStyles.TableStyleDark4:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.Bottom, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), true, Color.FromArgb(0, 0, 0), TableStyleRenderer.SolidStyle.Bottom, Color.White);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(123, 123, 123), Color.FromArgb(165, 165, 165), TableStyleRenderer.SolidStyle.None, Color.White, showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.Top, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), true, Color.FromArgb(82, 82, 82), TableStyleRenderer.SolidStyle.Top, showTotals, Color.White, true);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.Bottom, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), true, Color.FromArgb(0, 0, 0), TableStyleRenderer.SolidStyle.Bottom, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(118, 147, 60), Color.FromArgb(155, 187, 89), TableStyleRenderer.SolidStyle.None, Color.White, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.Top, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), true, Color.FromArgb(79, 98, 40), TableStyleRenderer.SolidStyle.Top, showTotals, Color.White, true);
        break;
      case TableBuiltInStyles.TableStyleDark3:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.Bottom, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), true, Color.FromArgb(0, 0, 0), TableStyleRenderer.SolidStyle.Bottom, Color.White);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(198, 89, 17), Color.FromArgb(237, 125, 49), TableStyleRenderer.SolidStyle.None, Color.White, showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.Top, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), true, Color.FromArgb(131, 60, 12), TableStyleRenderer.SolidStyle.Top, showTotals, Color.White, true);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.Bottom, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), true, Color.FromArgb(0, 0, 0), TableStyleRenderer.SolidStyle.Bottom, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(150, 54, 52), Color.FromArgb(192 /*0xC0*/, 80 /*0x50*/, 77), TableStyleRenderer.SolidStyle.None, Color.White, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.Top, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), true, Color.FromArgb(99, 37, 35), TableStyleRenderer.SolidStyle.Top, showTotals, Color.White, true);
        break;
      case TableBuiltInStyles.TableStyleDark2:
        if (flag)
        {
          this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.Bottom, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), true, Color.FromArgb(0, 0, 0), TableStyleRenderer.SolidStyle.Bottom, Color.White);
          this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(48 /*0x30*/, 84, 150), Color.FromArgb(68, 114, 196), TableStyleRenderer.SolidStyle.None, Color.White, showTotals);
          this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.Top, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), true, Color.FromArgb(32 /*0x20*/, 55, 100), TableStyleRenderer.SolidStyle.Top, showTotals, Color.White, true);
          break;
        }
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.Bottom, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), true, Color.FromArgb(0, 0, 0), TableStyleRenderer.SolidStyle.Bottom, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(54, 96 /*0x60*/, 146), Color.FromArgb(79, 129, 189), TableStyleRenderer.SolidStyle.None, Color.White, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.Top, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), true, Color.FromArgb(36, 64 /*0x40*/, 98), TableStyleRenderer.SolidStyle.Top, showTotals, Color.White, true);
        break;
      case TableBuiltInStyles.TableStyleDark1:
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.Bottom, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), true, Color.FromArgb(0, 0, 0), TableStyleRenderer.SolidStyle.Bottom, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.None, Color.Empty, false, Color.FromArgb(64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/), Color.FromArgb(115, 115, 115), TableStyleRenderer.SolidStyle.None, Color.White, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.Top, Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), true, Color.FromArgb(38, 38, 38), TableStyleRenderer.SolidStyle.Top, showTotals, Color.White, true);
        break;
      default:
        this.HeaderStyle(sheet, sheet.Range[objectLocation.Row, objectLocation.Column, objectLocation.Row, objectLocation.LastColumn].AddressLocal, TableStyleRenderer.BorderStyle.All, Color.FromArgb(149, 179, 215), true, Color.FromArgb(79, 129, 189), TableStyleRenderer.SolidStyle.None, Color.White);
        this.ContentStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(149, 179, 215), true, Color.FromArgb(220, 230, 241), Color.Empty, TableStyleRenderer.SolidStyle.None, Color.Black, showTotals);
        this.TotalStyle(sheet, objectLocation, TableStyleRenderer.BorderStyle.All, Color.FromArgb(149, 179, 215), true, Color.Empty, TableStyleRenderer.SolidStyle.None, showTotals, Color.Black, true);
        break;
    }
  }

  private void TotalStyle(
    IWorksheet sheet,
    IRange objectLocation,
    TableStyleRenderer.BorderStyle borderStyle,
    Color borderColor,
    bool entireRow,
    Color backgroundColor,
    TableStyleRenderer.SolidStyle solidStyle,
    bool showTotals,
    Color fontColor,
    bool boldFont)
  {
    if (!showTotals)
      return;
    IRange range = sheet.Range[objectLocation.LastRow, objectLocation.Column, objectLocation.LastRow, objectLocation.LastColumn];
    if (this._mediumStyleWithoutBorder.Contains(this._builtInStyle))
    {
      if (this._lastColumn && this._firstColumn)
        range = sheet.Range[range.Row, range.Column + 1, range.LastRow, range.LastColumn - 1];
      else if (this._firstColumn)
        range = sheet.Range[range.Row, range.Column + 1, range.LastRow, range.LastColumn];
      else if (this._lastColumn)
        range = sheet.Range[range.Row, range.Column, range.LastRow, range.LastColumn - 1];
    }
    this.ApplyFirstLastColumnBorder(sheet, range, solidStyle, borderStyle, borderColor, entireRow);
    range.CellStyle.Font.Bold = boldFont;
    this.ApplyFontColor(fontColor, range);
    this.ApplyBGColor(backgroundColor, range);
  }

  private void ApplyFontColor(Color fontColor, IRange range)
  {
    ExcelKnownColors color = (range.Worksheet.Workbook as WorkbookImpl).InnerFonts[0].Color;
    foreach (IRange key in (IEnumerable<IRange>) range)
    {
      if (key.CellStyle.Font.Color == color && (key.CellStyleName != null && key.CellStyleName != "Normal" || !key.NumberFormat.Contains("Red")))
      {
        if (this._colorFonts.Contains(this._builtInStyle))
          this._fontColorCollections.Add(key, fontColor);
        else
          key.CellStyle.Font.RGBColor = fontColor;
      }
    }
  }

  private void ApplyBorderFormatAndLineStyle(IRange range, TableStyleElement tableStyleElement)
  {
    if (!tableStyleElement.IsBorderFormatPresent)
      return;
    if (tableStyleElement.IsLeftBorderModified && (tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.FirstColumn || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.LastColumn || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.FirstHeaderCell || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.LastHeaderCell || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.FirstTotalCell || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.LastTotalCell || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.FirstColumnStripe || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.SecondColumnStripe || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.FirstRowStripe || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.SecondRowStripe || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.HeaderRow || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.TotalRow))
    {
      foreach (IRange range1 in (IEnumerable<IRange>) range.Worksheet[range.Row, range.Column, range.LastRow, range.Column])
      {
        IStyle cellStyle = range1.CellStyle;
        if (cellStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle == ExcelLineStyle.None && tableStyleElement.LeftBorderStyle != ExcelLineStyle.None)
        {
          cellStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = tableStyleElement.LeftBorderStyle;
          cellStyle.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = tableStyleElement.LeftBorderColorRGB;
        }
      }
    }
    if (tableStyleElement.IsBottomBorderModified && (tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.FirstColumn || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.LastColumn || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.HeaderRow || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.TotalRow || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.FirstHeaderCell || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.LastHeaderCell || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.FirstTotalCell || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.LastTotalCell || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.FirstColumnStripe || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.SecondColumnStripe || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.FirstRowStripe || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.SecondRowStripe))
    {
      foreach (IRange range2 in (IEnumerable<IRange>) range.Worksheet[range.LastRow, range.Column, range.LastRow, range.LastColumn])
      {
        IStyle cellStyle = range2.CellStyle;
        if (cellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle == ExcelLineStyle.None && tableStyleElement.BottomBorderStyle != ExcelLineStyle.None)
        {
          cellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = tableStyleElement.BottomBorderStyle;
          cellStyle.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = tableStyleElement.BottomBorderColorRGB;
        }
      }
    }
    if (tableStyleElement.IsRightBorderModified && (tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.FirstColumn || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.LastColumn || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.FirstHeaderCell || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.LastHeaderCell || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.FirstTotalCell || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.LastTotalCell || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.HeaderRow || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.TotalRow || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.FirstColumnStripe || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.SecondColumnStripe || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.FirstRowStripe || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.SecondRowStripe))
    {
      foreach (IRange range3 in (IEnumerable<IRange>) range.Worksheet[range.Row, range.LastColumn, range.LastRow, range.LastColumn])
      {
        IStyle cellStyle = range3.CellStyle;
        if (cellStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle == ExcelLineStyle.None && tableStyleElement.RightBorderStyle != ExcelLineStyle.None)
        {
          cellStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = tableStyleElement.RightBorderStyle;
          cellStyle.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = tableStyleElement.RightBorderColorRGB;
        }
      }
    }
    if (tableStyleElement.IsTopBorderModified && (tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.FirstColumn || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.LastColumn || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.HeaderRow || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.TotalRow || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.FirstHeaderCell || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.LastHeaderCell || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.FirstTotalCell || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.LastTotalCell || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.FirstColumnStripe || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.SecondColumnStripe || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.FirstRowStripe || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.SecondRowStripe))
    {
      foreach (IRange range4 in (IEnumerable<IRange>) range.Worksheet[range.Row, range.Column, range.Row, range.LastColumn])
      {
        IStyle cellStyle = range4.CellStyle;
        if (cellStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle == ExcelLineStyle.None && tableStyleElement.TopBorderStyle != ExcelLineStyle.None)
        {
          cellStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = tableStyleElement.TopBorderStyle;
          cellStyle.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = tableStyleElement.TopBorderColorRGB;
        }
      }
    }
    if (tableStyleElement.IsVerticalBorderModified && (tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.HeaderRow || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.TotalRow || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.FirstColumnStripe || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.SecondColumnStripe || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.FirstRowStripe || tableStyleElement.TableStyleElementType == ExcelTableStyleElementType.SecondRowStripe) && range.Column < range.LastColumn)
    {
      foreach (IRange range5 in (IEnumerable<IRange>) range.Worksheet[range.Row, range.Column, range.LastRow, range.LastColumn - 1])
      {
        IStyle cellStyle = range5.CellStyle;
        if (cellStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle == ExcelLineStyle.None && tableStyleElement.VerticalBorderStyle != ExcelLineStyle.None)
        {
          cellStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = tableStyleElement.VerticalBorderStyle;
          cellStyle.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = tableStyleElement.VerticalBorderColorRGB;
        }
      }
    }
    if (!tableStyleElement.IsHorizontalBorderModified || tableStyleElement.TableStyleElementType != ExcelTableStyleElementType.FirstColumn && tableStyleElement.TableStyleElementType != ExcelTableStyleElementType.LastColumn && tableStyleElement.TableStyleElementType != ExcelTableStyleElementType.FirstColumnStripe && tableStyleElement.TableStyleElementType != ExcelTableStyleElementType.SecondColumnStripe && tableStyleElement.TableStyleElementType != ExcelTableStyleElementType.FirstRowStripe && tableStyleElement.TableStyleElementType != ExcelTableStyleElementType.SecondRowStripe || range.Row >= range.LastRow)
      return;
    foreach (IRange range6 in (IEnumerable<IRange>) range.Worksheet[range.Row, range.Column, range.LastRow - 1, range.LastColumn])
    {
      IStyle cellStyle = range6.CellStyle;
      if (cellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle == ExcelLineStyle.None && tableStyleElement.HorizontalBorderStyle != ExcelLineStyle.None)
      {
        cellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = tableStyleElement.HorizontalBorderStyle;
        cellStyle.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = tableStyleElement.HorizontalBorderColorRGB;
      }
    }
  }

  private void ApplyWholeTableStyleBorder(
    IWorksheet sheet,
    IRange range,
    TableStyleElement tableStyleElement)
  {
    if (!tableStyleElement.IsBorderFormatPresent)
      return;
    if (tableStyleElement.IsTopBorderModified)
    {
      foreach (IRange range1 in (IEnumerable<IRange>) sheet.Range[range.Row, range.Column, range.Row, range.LastColumn])
      {
        IStyle cellStyle = range1.CellStyle;
        if (cellStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle == ExcelLineStyle.None && tableStyleElement.TopBorderStyle != ExcelLineStyle.None)
        {
          cellStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = tableStyleElement.TopBorderStyle;
          cellStyle.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = tableStyleElement.TopBorderColorRGB;
        }
      }
    }
    if (tableStyleElement.IsBottomBorderModified)
    {
      foreach (IRange range2 in (IEnumerable<IRange>) sheet.Range[range.LastRow, range.Column, range.LastRow, range.LastColumn])
      {
        IStyle cellStyle = range2.CellStyle;
        if (cellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle == ExcelLineStyle.None && tableStyleElement.BottomBorderStyle != ExcelLineStyle.None)
        {
          cellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = tableStyleElement.BottomBorderStyle;
          cellStyle.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = tableStyleElement.BottomBorderColorRGB;
        }
      }
    }
    if (tableStyleElement.IsRightBorderModified)
    {
      foreach (IRange range3 in (IEnumerable<IRange>) sheet.Range[range.Row, range.LastColumn, range.LastRow, range.LastColumn])
      {
        IStyle cellStyle = range3.CellStyle;
        if (cellStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle == ExcelLineStyle.None && tableStyleElement.RightBorderStyle != ExcelLineStyle.None)
        {
          cellStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = tableStyleElement.RightBorderStyle;
          cellStyle.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = tableStyleElement.RightBorderColorRGB;
        }
      }
    }
    if (tableStyleElement.IsLeftBorderModified)
    {
      foreach (IRange range4 in (IEnumerable<IRange>) sheet.Range[range.Row, range.Column, range.LastRow, range.Column])
      {
        IStyle cellStyle = range4.CellStyle;
        if (cellStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle == ExcelLineStyle.None && tableStyleElement.LeftBorderStyle != ExcelLineStyle.None)
        {
          cellStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = tableStyleElement.LeftBorderStyle;
          cellStyle.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = tableStyleElement.LeftBorderColorRGB;
        }
      }
    }
    if (tableStyleElement.IsVerticalBorderModified && range.Column < range.LastColumn)
    {
      foreach (IRange range5 in (IEnumerable<IRange>) sheet.Range[range.Row, range.Column, range.LastRow, range.LastColumn - 1])
      {
        IStyle cellStyle = range5.CellStyle;
        if (cellStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle == ExcelLineStyle.None && tableStyleElement.VerticalBorderStyle != ExcelLineStyle.None)
        {
          cellStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = tableStyleElement.VerticalBorderStyle;
          cellStyle.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = tableStyleElement.VerticalBorderColorRGB;
        }
      }
    }
    if (!tableStyleElement.IsHorizontalBorderModified)
      return;
    foreach (IRange range6 in (IEnumerable<IRange>) sheet.Range[range.Row, range.Column, range.LastRow - 1, range.LastColumn])
    {
      IStyle cellStyle = range6.CellStyle;
      if (cellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle == ExcelLineStyle.None && tableStyleElement.HorizontalBorderStyle != ExcelLineStyle.None)
      {
        cellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = tableStyleElement.HorizontalBorderStyle;
        cellStyle.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = tableStyleElement.HorizontalBorderColorRGB;
      }
    }
  }

  private void ApplyFontFormatAndColor(IRange range, TableStyleElement tableStyleElement)
  {
    ExcelKnownColors color = (range.Worksheet.Workbook as WorkbookImpl).InnerFonts[0].Color;
    if (tableStyleElement.IsFontColorPresent)
    {
      foreach (IRange range1 in (IEnumerable<IRange>) range)
      {
        if (range1.CellStyle.Font.Color == color && (range1.CellStyleName != null && range1.CellStyleName != "Normal" || !range1.NumberFormat.Contains("Red")))
          range1.CellStyle.Font.RGBColor = tableStyleElement.FontColorRGB;
      }
    }
    if (!tableStyleElement.IsFontFormatPresent)
      return;
    foreach (IRange range2 in (IEnumerable<IRange>) range)
    {
      if (tableStyleElement.Bold)
        range2.CellStyle.Font.Bold = true;
      if (tableStyleElement.Italic)
        range2.CellStyle.Font.Italic = true;
      if (tableStyleElement.StrikeThrough)
        range2.CellStyle.Font.Strikethrough = true;
      if (tableStyleElement.Underline == ExcelUnderline.Single)
        range2.CellStyle.Font.Underline = ExcelUnderline.Single;
      else if (tableStyleElement.Underline == ExcelUnderline.Double)
        range2.CellStyle.Font.Underline = ExcelUnderline.Double;
    }
  }

  private void ApplyBGColorAndFormat(IRange range, TableStyleElement tableStyleElement)
  {
    if (tableStyleElement.IsBackgroundColorPresent && tableStyleElement.BackColorRGB != Color.Empty)
    {
      foreach (IRange range1 in (IEnumerable<IRange>) range)
      {
        IStyle cellStyle = range1.CellStyle;
        if (cellStyle.Color.Name == Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue).Name || cellStyle.Color.Name == Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue).Name)
          cellStyle.Color = tableStyleElement.BackColorRGB;
      }
    }
    if (!tableStyleElement.IsPatternFormatPresent)
      return;
    foreach (IRange range2 in (IEnumerable<IRange>) range)
    {
      IStyle cellStyle = range2.CellStyle;
      if (cellStyle.FillPattern == ExcelPattern.Solid && tableStyleElement.PatternStyle != ExcelPattern.None)
      {
        cellStyle.FillPattern = tableStyleElement.PatternStyle;
        cellStyle.PatternColor = tableStyleElement.PatternColorRGB;
      }
    }
  }

  private void HeaderStyle(
    IWorksheet sheet,
    string headerRange,
    TableStyleRenderer.BorderStyle headerBorderStyle,
    Color borderColor,
    bool entireRow,
    Color backgroundColor,
    TableStyleRenderer.SolidStyle solidLineStyle,
    Color fontColor)
  {
    IRange range = sheet.Range[headerRange];
    if (this._mediumStyleWithoutBorder != null && (this._mediumStyleWithoutBorder.Contains(this._builtInStyle) || this._mediumStyleWithBorder.Contains(this._builtInStyle)))
      this._headerColor = backgroundColor;
    if (!this._headerRow)
      return;
    this.ApplyBorder(sheet, range, solidLineStyle, headerBorderStyle, borderColor, entireRow);
    this.ApplyFontColor(fontColor, range);
    range.CellStyle.Font.Bold = true;
    if (range.CellStyle.FillPattern != ExcelPattern.None)
      return;
    this.ApplyBGColor(backgroundColor, range);
  }

  private void ContentStyle(
    IWorksheet sheet,
    IRange headerRange,
    TableStyleRenderer.BorderStyle contentBorderStyle,
    Color borderColor,
    bool entireRow,
    Color backgroundFirstColor,
    Color backgroundSecondColor,
    TableStyleRenderer.SolidStyle solidLineStyle,
    Color fontColor,
    bool showTotals)
  {
    int row1 = headerRange.Row;
    int lastRow = headerRange.LastRow;
    if (this._headerRow)
      ++row1;
    if (showTotals)
      --lastRow;
    IRange range1 = sheet.Range[row1, headerRange.Column, lastRow, headerRange.LastColumn];
    if (this._firstColumn || this._lastColumn)
      range1 = this.ApplyFirstLastColumnStyle(sheet, range1, fontColor, backgroundFirstColor, showTotals);
    if (this._colorFonts.Contains(this._builtInStyle))
      this._fontColorCollections.Add(range1, fontColor);
    else if (fontColor != Color.Black)
      range1.CellStyle.Font.RGBColor = fontColor;
    bool flag = range1.Row % 2 != 0;
    if (!this._columnStripes && !this._rowStripes)
    {
      if (this._lightStyleBorder.Contains(this._builtInStyle))
      {
        this.ApplyBorder(sheet, sheet.Range[range1.Row, range1.Column, range1.LastRow, range1.Column], TableStyleRenderer.SolidStyle.None, TableStyleRenderer.BorderStyle.Left, borderColor, false);
        this.ApplyBorder(sheet, sheet.Range[range1.Row, range1.LastColumn, range1.LastRow, range1.LastColumn], TableStyleRenderer.SolidStyle.None, TableStyleRenderer.BorderStyle.Right, borderColor, false);
        this.ApplyBorder(sheet, sheet.Range[range1.LastRow, range1.Column, range1.LastRow, range1.LastColumn], TableStyleRenderer.SolidStyle.None, TableStyleRenderer.BorderStyle.Bottom, borderColor, false);
      }
      else
        this.ApplyBorder(sheet, range1, solidLineStyle, contentBorderStyle, borderColor, entireRow);
      this.ApplyBGColor(backgroundSecondColor, range1);
    }
    if (this._columnStripes && this._rowStripes)
    {
      if (this._lightStyleBorder.Contains(this._builtInStyle))
        entireRow = false;
      if (this._mediumStyleBorder.Contains(this._builtInStyle))
      {
        if (flag)
        {
          for (int row2 = range1.Row; row2 <= range1.LastRow; ++row2)
          {
            if (row2 % 2 == 0)
            {
              IRange borderRange = sheet.Range[row2, range1.Column, row2, range1.LastColumn];
              this.ApplyBorder(sheet, borderRange, solidLineStyle, contentBorderStyle, borderColor, entireRow);
            }
            else
            {
              IRange borderRange = sheet.Range[row2, range1.Column, row2, range1.LastColumn];
              this.ApplyBorder(sheet, borderRange, solidLineStyle, contentBorderStyle, borderColor, entireRow);
            }
          }
        }
        else
        {
          for (int row3 = range1.Row; row3 <= range1.LastRow; ++row3)
          {
            if (row3 % 2 == 0)
            {
              IRange borderRange = sheet.Range[row3, range1.Column, row3, range1.LastColumn];
              this.ApplyBorder(sheet, borderRange, solidLineStyle, contentBorderStyle, borderColor, entireRow);
            }
            else
            {
              IRange borderRange = sheet.Range[row3, range1.Column, row3, range1.LastColumn];
              this.ApplyBorder(sheet, borderRange, solidLineStyle, contentBorderStyle, borderColor, entireRow);
            }
          }
        }
      }
      if (!this._firstColumn ? range1.Column % 2 != 0 : (range1.Column - 1) % 2 != 0)
      {
        for (int column = range1.Column; column <= range1.LastColumn; ++column)
        {
          if (column % 2 == 0)
          {
            IRange borderRange = sheet.Range[range1.Row, column, range1.LastRow, column];
            if (!this._mediumStyleBorder.Contains(this._builtInStyle))
              this.ApplyBorder(sheet, borderRange, solidLineStyle, contentBorderStyle, borderColor, entireRow);
            if (borderRange.Row % 2 == 0)
            {
              for (int row4 = borderRange.Row; row4 <= borderRange.LastRow; ++row4)
              {
                IRange entireRange = sheet.Range[row4, borderRange.Column, row4, borderRange.LastColumn];
                if (row4 % 2 == 0)
                  this.ApplyBGColor(backgroundFirstColor, entireRange);
                else
                  this.ApplyBGColor(backgroundSecondColor, entireRange);
              }
            }
            else
            {
              for (int row5 = borderRange.Row; row5 <= borderRange.LastRow; ++row5)
              {
                IRange entireRange = sheet.Range[row5, borderRange.Column, row5, borderRange.LastColumn];
                if (row5 % 2 == 0)
                  this.ApplyBGColor(backgroundSecondColor, entireRange);
                else
                  this.ApplyBGColor(backgroundFirstColor, entireRange);
              }
            }
          }
          else
          {
            IRange range2 = sheet.Range[range1.Row, column, range1.LastRow, column];
            if (!this._mediumStyleBorder.Contains(this._builtInStyle))
              this.ApplyBorder(sheet, range2, solidLineStyle, contentBorderStyle, borderColor, entireRow);
            this.ApplyBGColor(backgroundFirstColor, range2);
          }
        }
      }
      else
      {
        for (int column = range1.Column; column <= range1.LastColumn; ++column)
        {
          if (column % 2 == 0)
          {
            IRange range3 = sheet.Range[range1.Row, column, range1.LastRow, column];
            if (!this._mediumStyleBorder.Contains(this._builtInStyle))
              this.ApplyBorder(sheet, range3, solidLineStyle, contentBorderStyle, borderColor, entireRow);
            this.ApplyBGColor(backgroundFirstColor, range3);
          }
          else
          {
            IRange borderRange = sheet.Range[range1.Row, column, range1.LastRow, column];
            if (!this._mediumStyleBorder.Contains(this._builtInStyle))
              this.ApplyBorder(sheet, borderRange, solidLineStyle, contentBorderStyle, borderColor, entireRow);
            if (borderRange.Row % 2 == 0)
            {
              for (int row6 = borderRange.Row; row6 <= borderRange.LastRow; ++row6)
              {
                IRange entireRange = sheet.Range[row6, borderRange.Column, row6, borderRange.LastColumn];
                this.ApplyBGColor(row6 % 2 == 0 ? backgroundFirstColor : backgroundSecondColor, entireRange);
              }
            }
            else
            {
              for (int row7 = borderRange.Row; row7 <= borderRange.LastRow; ++row7)
              {
                IRange entireRange = sheet.Range[row7, borderRange.Column, row7, borderRange.LastColumn];
                this.ApplyBGColor(row7 % 2 == 0 ? backgroundSecondColor : backgroundFirstColor, entireRange);
              }
            }
          }
        }
      }
    }
    else if (this._rowStripes)
    {
      if (flag)
      {
        for (int row8 = range1.Row; row8 <= range1.LastRow; ++row8)
        {
          if (row8 % 2 == 0)
          {
            IRange range4 = sheet.Range[row8, range1.Column, row8, range1.LastColumn];
            this.ApplyBorder(sheet, range4, solidLineStyle, contentBorderStyle, borderColor, entireRow);
            this.ApplyBGColor(backgroundSecondColor, range4);
          }
          else
          {
            IRange range5 = sheet.Range[row8, range1.Column, row8, range1.LastColumn];
            this.ApplyBorder(sheet, range5, solidLineStyle, contentBorderStyle, borderColor, entireRow);
            if (backgroundFirstColor != Color.Empty)
              this.ApplyBGColor(backgroundFirstColor, range5);
          }
        }
      }
      else
      {
        for (int row9 = range1.Row; row9 <= range1.LastRow; ++row9)
        {
          if (row9 % 2 == 0)
          {
            IRange range6 = sheet.Range[row9, range1.Column, row9, range1.LastColumn];
            this.ApplyBorder(sheet, range6, solidLineStyle, contentBorderStyle, borderColor, entireRow);
            this.ApplyBGColor(backgroundFirstColor, range6);
          }
          else
          {
            IRange range7 = sheet.Range[row9, range1.Column, row9, range1.LastColumn];
            this.ApplyBorder(sheet, range7, solidLineStyle, contentBorderStyle, borderColor, entireRow);
            this.ApplyBGColor(backgroundSecondColor, range7);
          }
        }
      }
    }
    else if (this._columnStripes)
    {
      if (this._lightStyleBorder.Contains(this._builtInStyle))
        contentBorderStyle = TableStyleRenderer.BorderStyle.LeftRight;
      if (this._mediumStyleBorder.Contains(this._builtInStyle))
      {
        if (flag)
        {
          for (int row10 = range1.Row; row10 <= range1.LastRow; ++row10)
          {
            if (row10 % 2 == 0)
            {
              IRange borderRange = sheet.Range[row10, range1.Column, row10, range1.LastColumn];
              this.ApplyBorder(sheet, borderRange, solidLineStyle, contentBorderStyle, borderColor, entireRow);
            }
            else
            {
              IRange borderRange = sheet.Range[row10, range1.Column, row10, range1.LastColumn];
              this.ApplyBorder(sheet, borderRange, solidLineStyle, contentBorderStyle, borderColor, entireRow);
            }
          }
        }
        else
        {
          for (int row11 = range1.Row; row11 <= range1.LastRow; ++row11)
          {
            if (row11 % 2 == 0)
            {
              IRange borderRange = sheet.Range[row11, range1.Column, row11, range1.LastColumn];
              this.ApplyBorder(sheet, borderRange, solidLineStyle, contentBorderStyle, borderColor, entireRow);
            }
            else
            {
              IRange borderRange = sheet.Range[row11, range1.Column, row11, range1.LastColumn];
              this.ApplyBorder(sheet, borderRange, solidLineStyle, contentBorderStyle, borderColor, entireRow);
            }
          }
        }
      }
      if (!this._firstColumn ? range1.Column % 2 != 0 : (range1.Column - 1) % 2 != 0)
      {
        for (int column = range1.Column; column <= range1.LastColumn; ++column)
        {
          if (column % 2 == 0)
          {
            IRange range8 = sheet.Range[range1.Row, column, range1.LastRow, column];
            if (!this._mediumStyleBorder.Contains(this._builtInStyle))
              this.ApplyBorder(sheet, range8, solidLineStyle, contentBorderStyle, borderColor, entireRow);
            this.ApplyBGColor(backgroundSecondColor, range8);
          }
          else
          {
            IRange range9 = sheet.Range[range1.Row, column, range1.LastRow, column];
            if (!this._mediumStyleBorder.Contains(this._builtInStyle))
              this.ApplyBorder(sheet, range9, solidLineStyle, contentBorderStyle, borderColor, entireRow);
            this.ApplyBGColor(backgroundFirstColor, range9);
          }
        }
      }
      else
      {
        for (int column = range1.Column; column <= range1.LastColumn; ++column)
        {
          if (column % 2 == 0)
          {
            IRange range10 = sheet.Range[range1.Row, column, range1.LastRow, column];
            if (!this._mediumStyleBorder.Contains(this._builtInStyle))
              this.ApplyBorder(sheet, range10, solidLineStyle, contentBorderStyle, borderColor, entireRow);
            this.ApplyBGColor(backgroundFirstColor, range10);
          }
          else
          {
            IRange range11 = sheet.Range[range1.Row, column, range1.LastRow, column];
            if (!this._mediumStyleBorder.Contains(this._builtInStyle))
              this.ApplyBorder(sheet, range11, solidLineStyle, contentBorderStyle, borderColor, entireRow);
            this.ApplyBGColor(backgroundSecondColor, range11);
          }
        }
      }
    }
    if (showTotals || !this._borderList.ContainsKey(this._builtInStyle) && !this._doubleBorderList.ContainsKey(this._builtInStyle))
      return;
    Color borderColor1;
    if (this._borderList.TryGetValue(this._builtInStyle, out borderColor1))
    {
      this.ApplyBorder(sheet, sheet.Range[headerRange.LastRow, headerRange.Column, headerRange.LastRow, headerRange.LastColumn], TableStyleRenderer.SolidStyle.None, TableStyleRenderer.BorderStyle.Bottom, borderColor1, true);
    }
    else
    {
      if (!this._doubleBorderList.TryGetValue(this._builtInStyle, out borderColor1))
        return;
      this.ApplyBorder(sheet, sheet.Range[headerRange.LastRow, headerRange.Column, headerRange.LastRow, headerRange.LastColumn], TableStyleRenderer.SolidStyle.None, TableStyleRenderer.BorderStyle.Bottom, borderColor1, true);
    }
  }

  private void ApplyBGColor(Color backgroundColor, IRange entireRange)
  {
    if (!(backgroundColor != Color.Empty))
      return;
    foreach (IRange range in (IEnumerable<IRange>) entireRange)
    {
      IStyle cellStyle = range.CellStyle;
      if (cellStyle.Color.Name == Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue).Name || cellStyle.Color.Name == Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue).Name)
        cellStyle.Color = backgroundColor;
    }
  }

  private void ApplyBorder(
    IWorksheet sheet,
    IRange borderRange,
    TableStyleRenderer.SolidStyle solidStyle,
    TableStyleRenderer.BorderStyle borderStyle,
    Color borderColor,
    bool entireRow)
  {
    if (this._borderColorList.Count == 0)
    {
      foreach (IRange range in (IEnumerable<IRange>) borderRange)
        this._borderColorList.Add(RangeImpl.GetCellIndex(range.Column, range.Row), new Dictionary<ExcelBordersIndex, Color>());
    }
    if (entireRow)
    {
      switch (borderStyle)
      {
        case TableStyleRenderer.BorderStyle.Top:
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeTop, solidStyle, TableStyleRenderer.SolidStyle.Top, TableStyleRenderer.SolidStyle.TopBottom, borderColor);
          break;
        case TableStyleRenderer.BorderStyle.Bottom:
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeBottom, solidStyle, TableStyleRenderer.SolidStyle.Bottom, TableStyleRenderer.SolidStyle.TopBottom, borderColor);
          break;
        case TableStyleRenderer.BorderStyle.Left:
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeLeft, solidStyle, TableStyleRenderer.SolidStyle.Left, TableStyleRenderer.SolidStyle.LeftRight, borderColor);
          break;
        case TableStyleRenderer.BorderStyle.Right:
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeRight, solidStyle, TableStyleRenderer.SolidStyle.Right, TableStyleRenderer.SolidStyle.LeftRight, borderColor);
          break;
        case TableStyleRenderer.BorderStyle.TopBottom:
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeTop, solidStyle, TableStyleRenderer.SolidStyle.Top, TableStyleRenderer.SolidStyle.TopBottom, borderColor);
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeBottom, solidStyle, TableStyleRenderer.SolidStyle.Bottom, TableStyleRenderer.SolidStyle.TopBottom, borderColor);
          break;
        case TableStyleRenderer.BorderStyle.LeftRight:
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeLeft, solidStyle, TableStyleRenderer.SolidStyle.Left, TableStyleRenderer.SolidStyle.LeftRight, borderColor);
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeRight, solidStyle, TableStyleRenderer.SolidStyle.Right, TableStyleRenderer.SolidStyle.LeftRight, borderColor);
          break;
        case TableStyleRenderer.BorderStyle.All:
          this.ApplySingleBorder(borderRange.Cells[0], ExcelBordersIndex.EdgeLeft, solidStyle, TableStyleRenderer.SolidStyle.Left, TableStyleRenderer.SolidStyle.LeftRight, borderColor);
          this.ApplySingleBorder(borderRange.Cells[borderRange.Cells.Length - 1], ExcelBordersIndex.EdgeRight, solidStyle, TableStyleRenderer.SolidStyle.Right, TableStyleRenderer.SolidStyle.LeftRight, borderColor);
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeTop, solidStyle, TableStyleRenderer.SolidStyle.Top, TableStyleRenderer.SolidStyle.TopBottom, borderColor);
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeBottom, solidStyle, TableStyleRenderer.SolidStyle.Bottom, TableStyleRenderer.SolidStyle.TopBottom, borderColor);
          break;
      }
    }
    else
    {
      switch (borderStyle)
      {
        case TableStyleRenderer.BorderStyle.Top:
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeTop, solidStyle, TableStyleRenderer.SolidStyle.Top, TableStyleRenderer.SolidStyle.TopBottom, borderColor);
          break;
        case TableStyleRenderer.BorderStyle.Bottom:
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeBottom, solidStyle, TableStyleRenderer.SolidStyle.Bottom, TableStyleRenderer.SolidStyle.TopBottom, borderColor);
          break;
        case TableStyleRenderer.BorderStyle.Left:
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeLeft, solidStyle, TableStyleRenderer.SolidStyle.Left, TableStyleRenderer.SolidStyle.LeftRight, borderColor);
          break;
        case TableStyleRenderer.BorderStyle.Right:
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeRight, solidStyle, TableStyleRenderer.SolidStyle.Right, TableStyleRenderer.SolidStyle.LeftRight, borderColor);
          break;
        case TableStyleRenderer.BorderStyle.TopBottom:
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeTop, solidStyle, TableStyleRenderer.SolidStyle.Top, TableStyleRenderer.SolidStyle.TopBottom, borderColor);
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeBottom, solidStyle, TableStyleRenderer.SolidStyle.Bottom, TableStyleRenderer.SolidStyle.TopBottom, borderColor);
          break;
        case TableStyleRenderer.BorderStyle.LeftRight:
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeLeft, solidStyle, TableStyleRenderer.SolidStyle.Left, TableStyleRenderer.SolidStyle.LeftRight, borderColor);
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeRight, solidStyle, TableStyleRenderer.SolidStyle.Right, TableStyleRenderer.SolidStyle.LeftRight, borderColor);
          break;
        case TableStyleRenderer.BorderStyle.All:
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeLeft, solidStyle, TableStyleRenderer.SolidStyle.Left, TableStyleRenderer.SolidStyle.LeftRight, borderColor);
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeRight, solidStyle, TableStyleRenderer.SolidStyle.Right, TableStyleRenderer.SolidStyle.LeftRight, borderColor);
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeTop, solidStyle, TableStyleRenderer.SolidStyle.Top, TableStyleRenderer.SolidStyle.TopBottom, borderColor);
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeBottom, solidStyle, TableStyleRenderer.SolidStyle.Bottom, TableStyleRenderer.SolidStyle.TopBottom, borderColor);
          break;
      }
    }
  }

  private void ApplySingleBorder(
    IRange borderRange,
    ExcelBordersIndex borderIndex,
    TableStyleRenderer.SolidStyle solidStyle,
    TableStyleRenderer.SolidStyle firstStyle,
    TableStyleRenderer.SolidStyle secondStyle,
    Color borderColor)
  {
    WorkbookImpl workbook = borderRange.Worksheet.Workbook as WorkbookImpl;
    if (borderRange.Cells.Length == 1)
    {
      if ((workbook.InnerExtFormats[(int) (borderRange as RangeImpl).ExtendedFormatIndex].Borders[borderIndex] as BorderImpl).LineStyle != ExcelLineStyle.None)
        return;
      (borderRange.Borders[borderIndex] as BorderImpl).LineStyle = solidStyle == firstStyle || solidStyle == secondStyle ? ExcelLineStyle.Medium : ExcelLineStyle.Thin;
      long cellIndex = RangeImpl.GetCellIndex(borderRange.Column, borderRange.Row);
      if (this._borderColorList.ContainsKey(cellIndex))
      {
        Dictionary<ExcelBordersIndex, Color> dictionary;
        this._borderColorList.TryGetValue(cellIndex, out dictionary);
        dictionary.Add(borderIndex, borderColor);
      }
      else
        this._borderColorList.Add(cellIndex, new Dictionary<ExcelBordersIndex, Color>()
        {
          {
            borderIndex,
            borderColor
          }
        });
    }
    else
    {
      foreach (IRange range in (IEnumerable<IRange>) borderRange)
      {
        if ((workbook.InnerExtFormats[(int) (range as RangeImpl).ExtendedFormatIndex].Borders[borderIndex] as BorderImpl).LineStyle == ExcelLineStyle.None)
        {
          (range.Borders[borderIndex] as BorderImpl).LineStyle = solidStyle == firstStyle || solidStyle == secondStyle ? ExcelLineStyle.Medium : ExcelLineStyle.Thin;
          long cellIndex = RangeImpl.GetCellIndex(range.Column, range.Row);
          if (this._borderColorList.ContainsKey(cellIndex))
          {
            Dictionary<ExcelBordersIndex, Color> dictionary;
            this._borderColorList.TryGetValue(cellIndex, out dictionary);
            dictionary.Add(borderIndex, borderColor);
          }
          else
            this._borderColorList.Add(cellIndex, new Dictionary<ExcelBordersIndex, Color>()
            {
              {
                borderIndex,
                borderColor
              }
            });
        }
      }
    }
  }

  private void ApplyFirstLastColumnBorder(
    IWorksheet sheet,
    IRange borderRange,
    TableStyleRenderer.SolidStyle solidStyle,
    TableStyleRenderer.BorderStyle borderStyle,
    Color borderColor,
    bool entireRow)
  {
    if (this._borderColorList.Count == 0)
    {
      foreach (IRange range in (IEnumerable<IRange>) borderRange)
        this._borderColorList.Add(RangeImpl.GetCellIndex(range.Column, range.Row), new Dictionary<ExcelBordersIndex, Color>());
    }
    if (entireRow)
    {
      switch (borderStyle)
      {
        case TableStyleRenderer.BorderStyle.Top:
          this.ApplyFirstLastColumnTopBorder(sheet, borderRange, solidStyle, borderColor);
          break;
        case TableStyleRenderer.BorderStyle.Bottom:
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeBottom, solidStyle, TableStyleRenderer.SolidStyle.Bottom, TableStyleRenderer.SolidStyle.TopBottom, borderColor);
          break;
        case TableStyleRenderer.BorderStyle.Left:
          this.ApplySingleBorder(borderRange.Cells[0], ExcelBordersIndex.EdgeLeft, solidStyle, TableStyleRenderer.SolidStyle.Left, TableStyleRenderer.SolidStyle.LeftRight, borderColor);
          break;
        case TableStyleRenderer.BorderStyle.Right:
          this.ApplySingleBorder(borderRange.Cells[borderRange.Cells.Length - 1], ExcelBordersIndex.EdgeRight, solidStyle, TableStyleRenderer.SolidStyle.Right, TableStyleRenderer.SolidStyle.LeftRight, borderColor);
          break;
        case TableStyleRenderer.BorderStyle.TopBottom:
          this.ApplyFirstLastColumnTopBorder(sheet, borderRange, solidStyle, borderColor);
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeBottom, solidStyle, TableStyleRenderer.SolidStyle.Bottom, TableStyleRenderer.SolidStyle.TopBottom, borderColor);
          break;
        case TableStyleRenderer.BorderStyle.LeftRight:
          this.ApplySingleBorder(borderRange.Cells[0], ExcelBordersIndex.EdgeLeft, solidStyle, TableStyleRenderer.SolidStyle.Left, TableStyleRenderer.SolidStyle.LeftRight, borderColor);
          this.ApplySingleBorder(borderRange.Cells[borderRange.Cells.Length - 1], ExcelBordersIndex.EdgeRight, solidStyle, TableStyleRenderer.SolidStyle.Right, TableStyleRenderer.SolidStyle.LeftRight, borderColor);
          break;
        case TableStyleRenderer.BorderStyle.All:
          this.ApplyFirstLastColumnTopBorder(sheet, borderRange, solidStyle, borderColor);
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeBottom, solidStyle, TableStyleRenderer.SolidStyle.Bottom, TableStyleRenderer.SolidStyle.TopBottom, borderColor);
          this.ApplySingleBorder(borderRange.Cells[0], ExcelBordersIndex.EdgeLeft, solidStyle, TableStyleRenderer.SolidStyle.Left, TableStyleRenderer.SolidStyle.LeftRight, borderColor);
          this.ApplySingleBorder(borderRange.Cells[borderRange.Cells.Length - 1], ExcelBordersIndex.EdgeRight, solidStyle, TableStyleRenderer.SolidStyle.Right, TableStyleRenderer.SolidStyle.LeftRight, borderColor);
          break;
      }
    }
    else
    {
      switch (borderStyle)
      {
        case TableStyleRenderer.BorderStyle.Top:
          this.ApplyFirstLastColumnTopBorder(sheet, borderRange, solidStyle, borderColor);
          break;
        case TableStyleRenderer.BorderStyle.Bottom:
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeBottom, solidStyle, TableStyleRenderer.SolidStyle.Bottom, TableStyleRenderer.SolidStyle.TopBottom, borderColor);
          break;
        case TableStyleRenderer.BorderStyle.Left:
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeLeft, solidStyle, TableStyleRenderer.SolidStyle.Left, TableStyleRenderer.SolidStyle.LeftRight, borderColor);
          break;
        case TableStyleRenderer.BorderStyle.Right:
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeRight, solidStyle, TableStyleRenderer.SolidStyle.Right, TableStyleRenderer.SolidStyle.LeftRight, borderColor);
          break;
        case TableStyleRenderer.BorderStyle.TopBottom:
          this.ApplyFirstLastColumnTopBorder(sheet, borderRange, solidStyle, borderColor);
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeBottom, solidStyle, TableStyleRenderer.SolidStyle.Bottom, TableStyleRenderer.SolidStyle.TopBottom, borderColor);
          break;
        case TableStyleRenderer.BorderStyle.LeftRight:
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeLeft, solidStyle, TableStyleRenderer.SolidStyle.Left, TableStyleRenderer.SolidStyle.LeftRight, borderColor);
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeRight, solidStyle, TableStyleRenderer.SolidStyle.Right, TableStyleRenderer.SolidStyle.LeftRight, borderColor);
          break;
        case TableStyleRenderer.BorderStyle.All:
          this.ApplyFirstLastColumnTopBorder(sheet, borderRange, solidStyle, borderColor);
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeBottom, solidStyle, TableStyleRenderer.SolidStyle.Bottom, TableStyleRenderer.SolidStyle.TopBottom, borderColor);
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeLeft, solidStyle, TableStyleRenderer.SolidStyle.Left, TableStyleRenderer.SolidStyle.LeftRight, borderColor);
          this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeRight, solidStyle, TableStyleRenderer.SolidStyle.Right, TableStyleRenderer.SolidStyle.LeftRight, borderColor);
          break;
      }
    }
  }

  private void ApplyFirstLastColumnTopBorder(
    IWorksheet sheet,
    IRange borderRange,
    TableStyleRenderer.SolidStyle solidStyle,
    Color borderColor)
  {
    WorkbookImpl workbook = sheet.Workbook as WorkbookImpl;
    if (this._doubleBorderList.ContainsKey(this._builtInStyle))
    {
      sheet.Range[borderRange.Row - 1, borderRange.Column, borderRange.LastRow - 1, borderRange.LastColumn].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.None;
      Color color;
      this._doubleBorderList.TryGetValue(this._builtInStyle, out color);
      foreach (IRange range in (IEnumerable<IRange>) borderRange)
      {
        if (workbook.InnerExtFormats[(int) (range as RangeImpl).ExtendedFormatIndex].Borders[ExcelBordersIndex.EdgeTop].LineStyle == ExcelLineStyle.None)
        {
          range.CellStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Double;
          long cellIndex = RangeImpl.GetCellIndex(range.Column, range.Row);
          if (this._borderColorList.ContainsKey(cellIndex))
          {
            Dictionary<ExcelBordersIndex, Color> dictionary;
            this._borderColorList.TryGetValue(cellIndex, out dictionary);
            dictionary.Add(ExcelBordersIndex.EdgeTop, color);
          }
          else
            this._borderColorList.Add(cellIndex, new Dictionary<ExcelBordersIndex, Color>()
            {
              {
                ExcelBordersIndex.EdgeTop,
                color
              }
            });
        }
      }
    }
    else if (this._topSolidList.ContainsKey(this._builtInStyle))
    {
      sheet.Range[borderRange.Row - 1, borderRange.Column, borderRange.LastRow - 1, borderRange.LastColumn].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.None;
      Color color;
      this._topSolidList.TryGetValue(this._builtInStyle, out color);
      foreach (IRange range in (IEnumerable<IRange>) borderRange)
      {
        if (workbook.InnerExtFormats[(int) (range as RangeImpl).ExtendedFormatIndex].Borders[ExcelBordersIndex.EdgeTop].LineStyle == ExcelLineStyle.None)
        {
          range.CellStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Medium;
          long cellIndex = RangeImpl.GetCellIndex(range.Column, range.Row);
          if (this._borderColorList.ContainsKey(cellIndex))
          {
            Dictionary<ExcelBordersIndex, Color> dictionary;
            this._borderColorList.TryGetValue(cellIndex, out dictionary);
            dictionary.Add(ExcelBordersIndex.EdgeTop, color);
          }
          else
            this._borderColorList.Add(cellIndex, new Dictionary<ExcelBordersIndex, Color>()
            {
              {
                ExcelBordersIndex.EdgeTop,
                color
              }
            });
        }
      }
    }
    else
      this.ApplySingleBorder(borderRange, ExcelBordersIndex.EdgeTop, solidStyle, TableStyleRenderer.SolidStyle.Top, TableStyleRenderer.SolidStyle.TopBottom, borderColor);
  }

  private IRange ApplyFirstLastColumnStyle(
    IWorksheet sheet,
    IRange contentRange,
    Color fontColor,
    Color backgroundFirstColor,
    bool showTotals)
  {
    RangesCollection rangesCollection1 = (RangesCollection) null;
    if (this._firstColumn && this._lastColumn)
    {
      rangesCollection1 = new RangesCollection(sheet.Application, (object) sheet);
      rangesCollection1.Add(sheet.Range[contentRange.Row, contentRange.Column, contentRange.LastRow, contentRange.Column]);
      rangesCollection1.Add(sheet.Range[contentRange.Row, contentRange.LastColumn, contentRange.LastRow, contentRange.LastColumn]);
      if (!this.CheckStyle())
      {
        rangesCollection1.CellStyle.Font.Bold = true;
        this._firstColumn = false;
        this._lastColumn = false;
        return contentRange;
      }
      contentRange = sheet.Range[contentRange.Row, contentRange.Column + 1, contentRange.LastRow, contentRange.LastColumn - 1];
    }
    else if (this._firstColumn)
    {
      rangesCollection1 = new RangesCollection(sheet.Application, (object) sheet);
      rangesCollection1.Add(sheet.Range[contentRange.Row, contentRange.Column, contentRange.LastRow, contentRange.Column]);
      if (!this.CheckStyle())
      {
        rangesCollection1.CellStyle.Font.Bold = true;
        this._firstColumn = false;
        return contentRange;
      }
      contentRange = sheet.Range[contentRange.Row, contentRange.Column + 1, contentRange.LastRow, contentRange.LastColumn];
    }
    else if (this._lastColumn)
    {
      rangesCollection1 = new RangesCollection(sheet.Application, (object) sheet);
      rangesCollection1.Add(sheet.Range[contentRange.Row, contentRange.LastColumn, contentRange.LastRow, contentRange.LastColumn]);
      if (!this.CheckStyle())
      {
        rangesCollection1.CellStyle.Font.Bold = true;
        this._lastColumn = false;
        return contentRange;
      }
      contentRange = sheet.Range[contentRange.Row, contentRange.Column, contentRange.LastRow, contentRange.LastColumn - 1];
    }
    if (this._colorFonts.Contains(this._builtInStyle))
      this._fontColorCollections.Add(rangesCollection1[0], fontColor);
    else
      rangesCollection1.CellStyle.Font.RGBColor = fontColor;
    rangesCollection1.CellStyle.Font.Bold = true;
    if (this._darkStyles.Contains(this._builtInStyle))
    {
      if (this._firstColumn && this._lastColumn)
      {
        this.ApplyBorder(sheet, rangesCollection1[0], TableStyleRenderer.SolidStyle.None, TableStyleRenderer.BorderStyle.Right, Color.White, true);
        this.ApplyBorder(sheet, rangesCollection1[1], TableStyleRenderer.SolidStyle.None, TableStyleRenderer.BorderStyle.Left, Color.White, true);
      }
      else if (this._firstColumn)
        this.ApplyBorder(sheet, rangesCollection1[0], TableStyleRenderer.SolidStyle.None, TableStyleRenderer.BorderStyle.Right, Color.White, true);
      else if (this._lastColumn)
        this.ApplyBorder(sheet, rangesCollection1[0], TableStyleRenderer.SolidStyle.None, TableStyleRenderer.BorderStyle.Left, Color.White, true);
      rangesCollection1.CellStyle.Color = backgroundFirstColor;
    }
    else if (this._mediumStyleWithBorder.Contains(this._builtInStyle))
    {
      if (this._firstColumn && this._lastColumn)
      {
        this.ApplyFirstLastColumnBorder(sheet, rangesCollection1[0], TableStyleRenderer.SolidStyle.None, TableStyleRenderer.BorderStyle.All, Color.White, false);
        this.ApplyFirstLastColumnBorder(sheet, rangesCollection1[1], TableStyleRenderer.SolidStyle.None, TableStyleRenderer.BorderStyle.All, Color.White, false);
      }
      else
        this.ApplyFirstLastColumnBorder(sheet, rangesCollection1[0], TableStyleRenderer.SolidStyle.None, TableStyleRenderer.BorderStyle.All, Color.White, false);
      rangesCollection1.CellStyle.Font.RGBColor = Color.White;
      rangesCollection1.CellStyle.Color = this._headerColor;
    }
    else if (this._mediumStyleWithoutBorder.Contains(this._builtInStyle))
    {
      RangesCollection rangesCollection2 = new RangesCollection(sheet.Application, (object) sheet);
      if (showTotals)
      {
        if (this._firstColumn && this._lastColumn)
        {
          rangesCollection2.Add(sheet.Range[rangesCollection1[0].Row, rangesCollection1[0].Column, rangesCollection1[0].LastRow + 1, rangesCollection1[0].Column]);
          rangesCollection2.Add(sheet.Range[rangesCollection1[1].Row, rangesCollection1[1].Column, rangesCollection1[1].LastRow + 1, rangesCollection1[1].Column]);
          this.ApplyFirstLastColumnBorder(sheet, sheet.Range[rangesCollection2[0].LastRow, rangesCollection2[0].LastColumn], TableStyleRenderer.SolidStyle.Bottom, TableStyleRenderer.BorderStyle.TopBottom, Color.Black, true);
          this.ApplyFirstLastColumnBorder(sheet, sheet.Range[rangesCollection2[1].LastRow, rangesCollection2[1].LastColumn], TableStyleRenderer.SolidStyle.Bottom, TableStyleRenderer.BorderStyle.TopBottom, Color.Black, true);
        }
        else
        {
          rangesCollection2.Add(sheet.Range[rangesCollection1[0].Row, rangesCollection1[0].Column, rangesCollection1[0].LastRow + 1, rangesCollection1[0].Column]);
          this.ApplyFirstLastColumnBorder(sheet, sheet.Range[rangesCollection2[0].LastRow, rangesCollection2[0].LastColumn], TableStyleRenderer.SolidStyle.Bottom, TableStyleRenderer.BorderStyle.TopBottom, Color.Black, true);
        }
      }
      else
        rangesCollection2 = rangesCollection1;
      rangesCollection2.CellStyle.Font.RGBColor = Color.White;
      rangesCollection2.CellStyle.Color = this._headerColor;
    }
    return contentRange;
  }

  private bool CheckStyle()
  {
    return this._darkStyles.Contains(this._builtInStyle) || this._mediumStyleWithBorder.Contains(this._builtInStyle) || this._mediumStyleWithoutBorder.Contains(this._builtInStyle);
  }

  private enum BorderStyle
  {
    None,
    Top,
    Bottom,
    Left,
    Right,
    TopBottom,
    LeftRight,
    All,
  }

  private enum SolidStyle
  {
    None,
    Top,
    Bottom,
    Right,
    Left,
    TopBottom,
    LeftRight,
  }
}
