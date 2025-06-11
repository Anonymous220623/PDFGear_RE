// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.Grouping.RangeGroup
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Collections;
using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections.Grouping;

internal class RangeGroup : CommonObject, IRange, IParentApplication, IEnumerable
{
  protected int m_iFirstRow;
  protected int m_iFirstColumn;
  protected int m_iLastRow;
  protected int m_iLastColumn;
  private WorksheetGroup m_sheetGroup;
  private RichTextStringGroup m_richText;
  private RangeGroup m_rangeEnd;
  protected StyleGroup m_style;

  protected RangeGroup(IApplication application, object parent)
    : base(application, parent)
  {
    this.FindParents();
  }

  public RangeGroup(IApplication application, object parent, int iFirstRow, int iFirstColumn)
    : this(application, parent, iFirstRow, iFirstColumn, iFirstRow, iFirstColumn)
  {
  }

  public RangeGroup(
    IApplication application,
    object parent,
    int iFirstRow,
    int iFirstColumn,
    int iLastRow,
    int iLastColumn)
    : this(application, parent)
  {
    this.m_iFirstRow = iFirstRow;
    this.m_iFirstColumn = iFirstColumn;
    this.m_iLastRow = iLastRow;
    this.m_iLastColumn = iLastColumn;
  }

  public RangeGroup(IApplication application, object parent, string name)
    : this(application, parent, name, false)
  {
  }

  public RangeGroup(IApplication application, object parent, string name, bool IsR1C1Notation)
    : this(application, parent)
  {
    WorksheetGroup worksheetGroup = (WorksheetGroup) parent;
    if (worksheetGroup.IsEmpty)
      throw new NotSupportedException("Sheets collection cannot be empty.");
    IRange range = worksheetGroup[0].Range[name, IsR1C1Notation];
    this.m_iFirstRow = range.Row;
    this.m_iFirstColumn = range.Column;
    this.m_iLastRow = range.LastRow;
    this.m_iLastColumn = range.LastColumn;
  }

  private void FindParents()
  {
    this.m_sheetGroup = this.FindParent(typeof (WorksheetGroup)) as WorksheetGroup;
    if (this.m_sheetGroup == null)
      throw new ArgumentOutOfRangeException("parent", "Can't find parent worksheet group.");
  }

  private IRange GetRange(int iSheetIndex)
  {
    return this.m_sheetGroup[iSheetIndex].Range[this.m_iFirstRow, this.m_iFirstColumn, this.m_iLastRow, this.m_iLastColumn];
  }

  public int Count => this.m_sheetGroup.Count;

  public IRange this[int index] => this.GetRange(index);

  public WorkbookImpl Workbook => this.m_sheetGroup.ParentWorkbook;

  public string Address => throw new NotSupportedException();

  public string AddressLocal
  {
    get
    {
      return RangeImpl.GetAddressLocal(this.m_iFirstRow, this.m_iFirstColumn, this.m_iLastRow, this.m_iLastColumn);
    }
  }

  public string AddressGlobal => throw new NotSupportedException();

  public string AddressR1C1 => throw new NotSupportedException();

  public string AddressR1C1Local
  {
    get
    {
      return RangeImpl.GetAddressLocal(this.m_iFirstRow, this.m_iFirstColumn, this.m_iLastRow, this.m_iLastColumn, true);
    }
  }

  public bool Boolean
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return false;
      bool boolean1 = this.GetRange(0).Boolean;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        bool boolean2 = this.GetRange(iSheetIndex).Boolean;
        if (boolean1 != boolean2)
          return false;
      }
      return boolean1;
    }
    set
    {
      int iSheetIndex = 0;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
        this.GetRange(iSheetIndex).Boolean = value;
    }
  }

  public IBorders Borders => this.CellStyle.Borders;

  public IRange[] Cells => throw new NotImplementedException();

  public int Column => this.m_iFirstColumn;

  public int ColumnGroupLevel
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return int.MinValue;
      int columnGroupLevel1 = this.GetRange(0).ColumnGroupLevel;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        int columnGroupLevel2 = this.GetRange(iSheetIndex).ColumnGroupLevel;
        if (columnGroupLevel1 != columnGroupLevel2)
          return int.MinValue;
      }
      return columnGroupLevel1;
    }
  }

  public double ColumnWidth
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return double.MinValue;
      double columnWidth1 = this.GetRange(0).ColumnWidth;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        double columnWidth2 = this.GetRange(iSheetIndex).ColumnWidth;
        if (columnWidth1 != columnWidth2)
          return double.MinValue;
      }
      return columnWidth1;
    }
    set
    {
      int iSheetIndex = 0;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
        this.GetRange(iSheetIndex).ColumnWidth = value;
    }
  }

  int IRange.Count => throw new NotImplementedException();

  public DateTime DateTime
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return DateTime.MinValue;
      DateTime dateTime1 = this.GetRange(0).DateTime;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        DateTime dateTime2 = this.GetRange(iSheetIndex).DateTime;
        if (dateTime1 != dateTime2)
          return DateTime.MinValue;
      }
      return dateTime1;
    }
    set
    {
      int iSheetIndex = 0;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
        this.GetRange(iSheetIndex).DateTime = value;
    }
  }

  public string DisplayText
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return (string) null;
      string displayText1 = this.GetRange(0).DisplayText;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        string displayText2 = this.GetRange(iSheetIndex).DisplayText;
        if (displayText1 != displayText2)
          return (string) null;
      }
      return displayText1;
    }
  }

  public IRange End
  {
    get
    {
      if (this.m_rangeEnd == null)
        this.m_rangeEnd = new RangeGroup(this.Application, (object) this, this.m_iLastRow, this.m_iLastColumn);
      return (IRange) this.m_rangeEnd;
    }
  }

  public IRange EntireColumn => throw new NotImplementedException();

  public IRange EntireRow => throw new NotImplementedException();

  public string Error
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return (string) null;
      string error1 = this.GetRange(0).Error;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        string error2 = this.GetRange(iSheetIndex).Error;
        if (error1 != error2)
          return (string) null;
      }
      return error1;
    }
    set
    {
      int iSheetIndex = 0;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
        this.GetRange(iSheetIndex).Error = value;
    }
  }

  public string Formula
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return (string) null;
      string formula1 = this.GetRange(0).Formula;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        string formula2 = this.GetRange(iSheetIndex).Formula;
        if (formula1 != formula2)
          return (string) null;
      }
      return formula1;
    }
    set
    {
      int iSheetIndex = 0;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
        this.GetRange(iSheetIndex).Formula = value;
    }
  }

  public string FormulaR1C1
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return (string) null;
      string formulaR1C1_1 = this.GetRange(0).FormulaR1C1;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        string formulaR1C1_2 = this.GetRange(iSheetIndex).FormulaR1C1;
        if (formulaR1C1_1 != formulaR1C1_2)
          return (string) null;
      }
      return formulaR1C1_1;
    }
    set
    {
      int iSheetIndex = 0;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
        this.GetRange(iSheetIndex).FormulaR1C1 = value;
    }
  }

  public string FormulaArray
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return (string) null;
      string formulaArray1 = this.GetRange(0).FormulaArray;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        string formulaArray2 = this.GetRange(iSheetIndex).FormulaArray;
        if (formulaArray1 != formulaArray2)
          return (string) null;
      }
      return formulaArray1;
    }
    set
    {
      int iSheetIndex = 0;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
        this.GetRange(iSheetIndex).FormulaArray = value;
    }
  }

  public string FormulaArrayR1C1
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return (string) null;
      string formulaArrayR1C1_1 = this.GetRange(0).FormulaArrayR1C1;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        string formulaArrayR1C1_2 = this.GetRange(iSheetIndex).FormulaArrayR1C1;
        if (formulaArrayR1C1_1 != formulaArrayR1C1_2)
          return (string) null;
      }
      return formulaArrayR1C1_1;
    }
    set
    {
      int iSheetIndex = 0;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
        this.GetRange(iSheetIndex).FormulaArrayR1C1 = value;
    }
  }

  public bool FormulaHidden
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return false;
      bool formulaHidden1 = this.GetRange(0).FormulaHidden;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        bool formulaHidden2 = this.GetRange(iSheetIndex).FormulaHidden;
        if (formulaHidden1 != formulaHidden2)
          return false;
      }
      return formulaHidden1;
    }
    set
    {
      int iSheetIndex = 0;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
        this.GetRange(iSheetIndex).FormulaHidden = value;
    }
  }

  public DateTime FormulaDateTime
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return DateTime.MinValue;
      DateTime formulaDateTime1 = this.GetRange(0).FormulaDateTime;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        DateTime formulaDateTime2 = this.GetRange(iSheetIndex).FormulaDateTime;
        if (formulaDateTime1 != formulaDateTime2)
          return DateTime.MinValue;
      }
      return formulaDateTime1;
    }
    set
    {
      int iSheetIndex = 0;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
        this.GetRange(iSheetIndex).FormulaDateTime = value;
    }
  }

  public bool HasDataValidation => false;

  public bool HasBoolean
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return false;
      bool hasBoolean1 = this.GetRange(0).HasBoolean;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        bool hasBoolean2 = this.GetRange(iSheetIndex).HasBoolean;
        if (hasBoolean1 != hasBoolean2)
          return false;
      }
      return hasBoolean1;
    }
  }

  public bool HasDateTime
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return false;
      bool hasDateTime1 = this.GetRange(0).HasDateTime;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        bool hasDateTime2 = this.GetRange(iSheetIndex).HasDateTime;
        if (hasDateTime1 != hasDateTime2)
          return false;
      }
      return hasDateTime1;
    }
  }

  public bool HasFormulaBoolValue
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return false;
      bool formulaBoolValue1 = this.GetRange(0).HasFormulaBoolValue;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        bool formulaBoolValue2 = this.GetRange(iSheetIndex).HasFormulaBoolValue;
        if (formulaBoolValue1 != formulaBoolValue2)
          return false;
      }
      return formulaBoolValue1;
    }
  }

  public bool HasFormulaErrorValue
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return false;
      bool formulaErrorValue1 = this.GetRange(0).HasFormulaErrorValue;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        bool formulaErrorValue2 = this.GetRange(iSheetIndex).HasFormulaErrorValue;
        if (formulaErrorValue1 != formulaErrorValue2)
          return false;
      }
      return formulaErrorValue1;
    }
  }

  public bool HasFormulaDateTime
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return false;
      bool hasFormulaDateTime1 = this.GetRange(0).HasFormulaDateTime;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        bool hasFormulaDateTime2 = this.GetRange(iSheetIndex).HasFormulaDateTime;
        if (hasFormulaDateTime1 != hasFormulaDateTime2)
        {
          hasFormulaDateTime1 = false;
          break;
        }
      }
      return hasFormulaDateTime1;
    }
  }

  public bool HasFormulaNumberValue
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return false;
      bool formulaNumberValue1 = this.GetRange(0).HasFormulaNumberValue;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        bool formulaNumberValue2 = this.GetRange(iSheetIndex).HasFormulaNumberValue;
        if (formulaNumberValue1 != formulaNumberValue2)
        {
          formulaNumberValue1 = false;
          break;
        }
      }
      return formulaNumberValue1;
    }
  }

  public bool HasFormulaStringValue
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return false;
      bool formulaStringValue1 = this.GetRange(0).HasFormulaStringValue;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        bool formulaStringValue2 = this.GetRange(iSheetIndex).HasFormulaStringValue;
        if (formulaStringValue1 != formulaStringValue2)
        {
          formulaStringValue1 = false;
          break;
        }
      }
      return formulaStringValue1;
    }
  }

  public bool HasFormula
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return false;
      bool hasFormula1 = this.GetRange(0).HasFormula;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        bool hasFormula2 = this.GetRange(iSheetIndex).HasFormula;
        if (hasFormula1 != hasFormula2)
          return false;
      }
      return hasFormula1;
    }
  }

  public bool HasFormulaArray
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return false;
      bool hasFormulaArray1 = this.GetRange(0).HasFormulaArray;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        bool hasFormulaArray2 = this.GetRange(iSheetIndex).HasFormulaArray;
        if (hasFormulaArray1 != hasFormulaArray2)
          return false;
      }
      return hasFormulaArray1;
    }
  }

  public bool HasNumber
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return false;
      bool hasNumber1 = this.GetRange(0).HasNumber;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        bool hasNumber2 = this.GetRange(iSheetIndex).HasNumber;
        if (hasNumber1 != hasNumber2)
          return false;
      }
      return hasNumber1;
    }
  }

  public bool HasRichText
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return false;
      bool hasRichText1 = this.GetRange(0).HasRichText;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        bool hasRichText2 = this.GetRange(iSheetIndex).HasRichText;
        if (hasRichText1 != hasRichText2)
          return false;
      }
      return hasRichText1;
    }
  }

  public bool HasString
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return false;
      bool hasString1 = this.GetRange(0).HasString;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        bool hasString2 = this.GetRange(iSheetIndex).HasString;
        if (hasString1 != hasString2)
          return false;
      }
      return hasString1;
    }
  }

  public bool HasStyle
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return false;
      bool hasStyle1 = this.GetRange(0).HasStyle;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        bool hasStyle2 = this.GetRange(iSheetIndex).HasStyle;
        if (hasStyle1 != hasStyle2)
          return false;
      }
      return hasStyle1;
    }
  }

  public OfficeHAlign HorizontalAlignment
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return OfficeHAlign.HAlignGeneral;
      OfficeHAlign horizontalAlignment1 = this.GetRange(0).HorizontalAlignment;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        OfficeHAlign horizontalAlignment2 = this.GetRange(iSheetIndex).HorizontalAlignment;
        if (horizontalAlignment1 != horizontalAlignment2)
          return OfficeHAlign.HAlignGeneral;
      }
      return horizontalAlignment1;
    }
    set
    {
      int iSheetIndex = 0;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
        this.GetRange(iSheetIndex).HorizontalAlignment = value;
    }
  }

  public int IndentLevel
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return int.MinValue;
      int indentLevel1 = this.GetRange(0).IndentLevel;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        int indentLevel2 = this.GetRange(iSheetIndex).IndentLevel;
        if (indentLevel1 != indentLevel2)
          return int.MinValue;
      }
      return indentLevel1;
    }
    set
    {
      int iSheetIndex = 0;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
        this.GetRange(iSheetIndex).IndentLevel = value;
    }
  }

  public bool IsBlank
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return false;
      bool isBlank1 = this.GetRange(0).IsBlank;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        bool isBlank2 = this.GetRange(iSheetIndex).IsBlank;
        if (isBlank1 != isBlank2)
          return false;
      }
      return isBlank1;
    }
  }

  public bool IsBoolean
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return false;
      bool isBoolean1 = this.GetRange(0).IsBoolean;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        bool isBoolean2 = this.GetRange(iSheetIndex).IsBoolean;
        if (isBoolean1 != isBoolean2)
          return false;
      }
      return isBoolean1;
    }
  }

  public bool IsError
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return false;
      bool isError1 = this.GetRange(0).IsError;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        bool isError2 = this.GetRange(iSheetIndex).IsError;
        if (isError1 != isError2)
          return false;
      }
      return isError1;
    }
  }

  public bool IsGroupedByColumn
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return false;
      bool isGroupedByColumn1 = this.GetRange(0).IsGroupedByColumn;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        bool isGroupedByColumn2 = this.GetRange(iSheetIndex).IsGroupedByColumn;
        if (isGroupedByColumn1 != isGroupedByColumn2)
          return false;
      }
      return isGroupedByColumn1;
    }
  }

  public bool IsGroupedByRow
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return false;
      bool isGroupedByRow1 = this.GetRange(0).IsGroupedByRow;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        bool isGroupedByRow2 = this.GetRange(iSheetIndex).IsGroupedByRow;
        if (isGroupedByRow1 != isGroupedByRow2)
          return false;
      }
      return isGroupedByRow1;
    }
  }

  public bool IsInitialized
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return false;
      bool isInitialized1 = this.GetRange(0).IsInitialized;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        bool isInitialized2 = this.GetRange(iSheetIndex).IsInitialized;
        if (isInitialized1 != isInitialized2)
          return false;
      }
      return isInitialized1;
    }
  }

  public int LastColumn => this.m_iLastColumn;

  public int LastRow => this.m_iLastRow;

  public double Number
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return double.MinValue;
      double number1 = this.GetRange(0).Number;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        double number2 = this.GetRange(iSheetIndex).Number;
        if (number1 != number2)
          return double.MinValue;
      }
      return number1;
    }
    set
    {
      int iSheetIndex = 0;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        if (BitConverter.DoubleToInt64Bits(value) == BitConverter.DoubleToInt64Bits(-0.0))
          value = 0.0;
        this.GetRange(iSheetIndex).Number = value;
      }
    }
  }

  public string NumberFormat
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return (string) null;
      string numberFormat1 = this.GetRange(0).NumberFormat;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        string numberFormat2 = this.GetRange(iSheetIndex).NumberFormat;
        if (numberFormat1 != numberFormat2)
          return (string) null;
      }
      return numberFormat1;
    }
    set
    {
      int iSheetIndex = 0;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
        this.GetRange(iSheetIndex).NumberFormat = value;
    }
  }

  public int Row => this.m_iFirstRow;

  public int RowGroupLevel
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return int.MinValue;
      int rowGroupLevel1 = this.GetRange(0).RowGroupLevel;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        int rowGroupLevel2 = this.GetRange(iSheetIndex).RowGroupLevel;
        if (rowGroupLevel1 != rowGroupLevel2)
          return int.MinValue;
      }
      return rowGroupLevel1;
    }
  }

  public double RowHeight
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return double.MinValue;
      double rowHeight1 = this.GetRange(0).RowHeight;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        double rowHeight2 = this.GetRange(iSheetIndex).RowHeight;
        if (rowHeight1 != rowHeight2)
          return double.MinValue;
      }
      return rowHeight1;
    }
    set
    {
      int iSheetIndex = 0;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
        this.GetRange(iSheetIndex).RowHeight = value;
    }
  }

  public IRange[] Rows => throw new NotImplementedException();

  public IRange[] Columns => throw new NotImplementedException();

  public IStyle CellStyle
  {
    get
    {
      if (this.m_style == null)
        this.m_style = new StyleGroup(this.Application, (object) this);
      return (IStyle) this.m_style;
    }
    set => throw new NotImplementedException();
  }

  public string CellStyleName
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return (string) null;
      string cellStyleName1 = this.GetRange(0).CellStyleName;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        string cellStyleName2 = this.GetRange(iSheetIndex).CellStyleName;
        if (cellStyleName1 != cellStyleName2)
          return (string) null;
      }
      return cellStyleName1;
    }
    set
    {
      int iSheetIndex = 0;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
        this.GetRange(iSheetIndex).CellStyleName = value;
    }
  }

  public string Text
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return (string) null;
      string text1 = this.GetRange(0).Text;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        string text2 = this.GetRange(iSheetIndex).Text;
        if (text1 != text2)
          return (string) null;
      }
      return text1;
    }
    set
    {
      int iSheetIndex = 0;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
        this.GetRange(iSheetIndex).Text = value;
    }
  }

  public TimeSpan TimeSpan
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return TimeSpan.MinValue;
      TimeSpan timeSpan1 = this.GetRange(0).TimeSpan;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        TimeSpan timeSpan2 = this.GetRange(iSheetIndex).TimeSpan;
        if (timeSpan1 != timeSpan2)
          return TimeSpan.MinValue;
      }
      return timeSpan1;
    }
    set
    {
      int iSheetIndex = 0;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
        this.GetRange(iSheetIndex).TimeSpan = value;
    }
  }

  public string Value
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return (string) null;
      string str1 = this.GetRange(0).Value;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        string str2 = this.GetRange(iSheetIndex).Value;
        if (str1 != str2)
          return (string) null;
      }
      return str1;
    }
    set
    {
      int iSheetIndex = 0;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
        this.GetRange(iSheetIndex).Value = value;
    }
  }

  public string CalculatedValue => (string) null;

  public object Value2
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return (object) null;
      object obj1 = this.GetRange(0).Value2;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        object obj2 = this.GetRange(iSheetIndex).Value2;
        if (obj1 != obj2)
          return (object) null;
      }
      return obj1;
    }
    set
    {
      int iSheetIndex = 0;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
        this.GetRange(iSheetIndex).Value2 = value;
    }
  }

  public OfficeVAlign VerticalAlignment
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return OfficeVAlign.VAlignTop;
      OfficeVAlign verticalAlignment1 = this.GetRange(0).VerticalAlignment;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        OfficeVAlign verticalAlignment2 = this.GetRange(iSheetIndex).VerticalAlignment;
        if (verticalAlignment1 != verticalAlignment2)
          return OfficeVAlign.VAlignTop;
      }
      return verticalAlignment1;
    }
    set
    {
      int iSheetIndex = 0;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
        this.GetRange(iSheetIndex).VerticalAlignment = value;
    }
  }

  public IWorksheet Worksheet => (IWorksheet) this.m_sheetGroup;

  public IRange this[int row, int column]
  {
    get
    {
      return (IRange) new RangeGroup(this.Application, (object) this.m_sheetGroup, row, column, row, column);
    }
    set => throw new NotSupportedException();
  }

  public IRange this[int row, int column, int lastRow, int lastColumn]
  {
    get
    {
      return (IRange) new RangeGroup(this.Application, (object) this.m_sheetGroup, row, column, lastRow, lastColumn);
    }
  }

  public IRange this[string name] => this[name, false];

  public IRange this[string name, bool IsR1C1Notation]
  {
    get
    {
      return (IRange) new RangeGroup(this.Application, (object) this.m_sheetGroup, name, IsR1C1Notation);
    }
  }

  public string FormulaStringValue
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return (string) null;
      string formulaStringValue1 = this.GetRange(0).FormulaStringValue;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        string formulaStringValue2 = this.GetRange(iSheetIndex).FormulaStringValue;
        if (formulaStringValue1 != formulaStringValue2)
          return (string) null;
      }
      return formulaStringValue1;
    }
    set
    {
      int iSheetIndex = 0;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
        this.GetRange(iSheetIndex).FormulaStringValue = value;
    }
  }

  public double FormulaNumberValue
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return double.MinValue;
      double formulaNumberValue1 = this.GetRange(0).FormulaNumberValue;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        double formulaNumberValue2 = this.GetRange(iSheetIndex).FormulaNumberValue;
        if (formulaNumberValue1 != formulaNumberValue2)
          return double.MinValue;
      }
      return formulaNumberValue1;
    }
    set
    {
      int iSheetIndex = 0;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
        this.GetRange(iSheetIndex).FormulaNumberValue = value;
    }
  }

  public bool FormulaBoolValue
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return false;
      bool formulaBoolValue1 = this.GetRange(0).FormulaBoolValue;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        bool formulaBoolValue2 = this.GetRange(iSheetIndex).FormulaBoolValue;
        if (formulaBoolValue1 != formulaBoolValue2)
          return false;
      }
      return formulaBoolValue1;
    }
    set
    {
      int iSheetIndex = 0;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
        this.GetRange(iSheetIndex).FormulaBoolValue = value;
    }
  }

  public string FormulaErrorValue
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return (string) null;
      string formulaErrorValue = this.GetRange(0).FormulaErrorValue;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        string formulaStringValue = this.GetRange(iSheetIndex).FormulaStringValue;
        if (formulaErrorValue != formulaStringValue)
          return (string) null;
      }
      return formulaErrorValue;
    }
    set
    {
      int iSheetIndex = 0;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
        this.GetRange(iSheetIndex).FormulaErrorValue = value;
    }
  }

  public IRichTextString RichText
  {
    get
    {
      if (this.m_richText == null)
        this.m_richText = new RichTextStringGroup(this.Application, (object) this);
      return (IRichTextString) this.m_richText;
    }
  }

  public bool IsMerged
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return false;
      bool isMerged1 = this.GetRange(0).IsMerged;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        bool isMerged2 = this.GetRange(iSheetIndex).IsMerged;
        if (isMerged1 != isMerged2)
          return false;
      }
      return isMerged1;
    }
  }

  public IRange MergeArea => throw new NotImplementedException();

  public bool WrapText
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return false;
      bool wrapText1 = this.GetRange(0).WrapText;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        bool wrapText2 = this.GetRange(iSheetIndex).WrapText;
        if (wrapText1 != wrapText2)
          return false;
      }
      return wrapText1;
    }
    set
    {
      int iSheetIndex = 0;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
        this.GetRange(iSheetIndex).WrapText = value;
    }
  }

  public bool HasExternalFormula
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return false;
      int iSheetIndex = 0;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      {
        if (!this.GetRange(iSheetIndex).HasExternalFormula)
          return false;
      }
      return true;
    }
  }

  public ExcelIgnoreError IgnoreErrorOptions
  {
    get => this.m_sheetGroup.IsEmpty ? ExcelIgnoreError.None : ExcelIgnoreError.All;
    set
    {
    }
  }

  public BuiltInStyles? BuiltInStyle
  {
    get
    {
      if (this.m_sheetGroup.IsEmpty)
        return new BuiltInStyles?();
      BuiltInStyles? builtInStyle1 = this.GetRange(0).BuiltInStyle;
      int iSheetIndex = 1;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count && builtInStyle1.HasValue; ++iSheetIndex)
      {
        BuiltInStyles? builtInStyle2 = this.GetRange(iSheetIndex).BuiltInStyle;
        BuiltInStyles? nullable = builtInStyle1;
        if ((builtInStyle2.GetValueOrDefault() != nullable.GetValueOrDefault() ? 1 : (builtInStyle2.HasValue != nullable.HasValue ? 1 : 0)) != 0)
        {
          builtInStyle1 = new BuiltInStyles?();
          break;
        }
      }
      return builtInStyle1;
    }
    set
    {
      int iSheetIndex = 0;
      for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
        this.GetRange(iSheetIndex).BuiltInStyle = value;
    }
  }

  public IRange Activate() => throw new NotSupportedException();

  public IRange Activate(bool scroll) => throw new NotSupportedException();

  public IRange Group(OfficeGroupBy groupBy) => throw new NotSupportedException();

  public IRange Group(OfficeGroupBy groupBy, bool bCollapsed) => throw new NotSupportedException();

  public void SubTotal(int groupBy, ConsolidationFunction function, int[] totalList)
  {
    throw new NotSupportedException();
  }

  public void SubTotal(
    int groupBy,
    ConsolidationFunction function,
    int[] totalList,
    bool replace,
    bool pageBreaks,
    bool summaryBelowData)
  {
    throw new NotSupportedException();
  }

  public void Merge()
  {
    int iSheetIndex = 0;
    for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      this.GetRange(iSheetIndex).Merge();
  }

  public void Merge(bool clearCells)
  {
    int iSheetIndex = 0;
    for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      this.GetRange(iSheetIndex).Merge(clearCells);
  }

  public IRange Ungroup(OfficeGroupBy groupBy) => throw new NotSupportedException();

  public void UnMerge()
  {
    int iSheetIndex = 0;
    for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      this.GetRange(iSheetIndex).UnMerge();
  }

  public void FreezePanes() => throw new NotSupportedException();

  public void Clear()
  {
    int iSheetIndex = 0;
    for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      this.GetRange(iSheetIndex).Clear();
  }

  public void Clear(bool isClearFormat)
  {
    int iSheetIndex = 0;
    for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      this.GetRange(iSheetIndex).Clear(isClearFormat);
  }

  public void Clear(OfficeMoveDirection direction)
  {
    int iSheetIndex = 0;
    for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      this.GetRange(iSheetIndex).Clear(direction);
  }

  public void Clear(OfficeClearOptions option)
  {
    int iSheetIndex = 0;
    for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      this.GetRange(iSheetIndex).Clear(option);
  }

  public void Clear(OfficeMoveDirection direction, OfficeCopyRangeOptions options)
  {
    int iSheetIndex = 0;
    for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      this.GetRange(iSheetIndex).Clear(direction, options);
  }

  public void MoveTo(IRange destination) => throw new NotImplementedException();

  public void MoveTo(IRange destination, bool bUpdateFormula)
  {
    throw new NotImplementedException();
  }

  public IRange CopyTo(IRange destination) => throw new NotImplementedException();

  public IRange CopyTo(IRange destination, bool bUpdateFormula)
  {
    throw new NotImplementedException();
  }

  public IRange CopyTo(IRange destination, OfficeCopyRangeOptions options)
  {
    throw new NotImplementedException();
  }

  public IRange IntersectWith(IRange range) => throw new NotImplementedException();

  public IRange MergeWith(IRange range) => throw new NotImplementedException();

  public void AutofitRows()
  {
    int iSheetIndex = 0;
    for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      this.GetRange(iSheetIndex).AutofitRows();
  }

  public void AutofitColumns()
  {
    int iSheetIndex = 0;
    for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      this.GetRange(iSheetIndex).AutofitColumns();
  }

  public IRange FindFirst(string findValue, OfficeFindType flags)
  {
    throw new NotImplementedException();
  }

  IRange IRange.FindFirst(double findValue, OfficeFindType flags)
  {
    throw new NotImplementedException();
  }

  IRange IRange.FindFirst(bool findValue) => throw new NotImplementedException();

  IRange IRange.FindFirst(DateTime findValue) => throw new NotImplementedException();

  public IRange FindFirst(TimeSpan findValue) => throw new NotImplementedException();

  public IRange[] FindAll(string findValue, OfficeFindType flags)
  {
    throw new NotImplementedException();
  }

  public IRange[] FindAll(double findValue, OfficeFindType flags)
  {
    throw new NotImplementedException();
  }

  public IRange[] FindAll(bool findValue) => throw new NotImplementedException();

  public IRange[] FindAll(DateTime findValue) => throw new NotImplementedException();

  public IRange[] FindAll(TimeSpan findValue) => throw new NotImplementedException();

  public void CopyToClipboard() => throw new NotSupportedException();

  public void BorderAround() => this.BorderAround(OfficeLineStyle.Thin);

  public void BorderAround(OfficeLineStyle borderLine)
  {
    this.BorderAround(borderLine, OfficeKnownColors.Black);
  }

  public void BorderAround(OfficeLineStyle borderLine, Color borderColor)
  {
    OfficeKnownColors nearestColor = this.Workbook.GetNearestColor(borderColor);
    this.BorderAround(borderLine, nearestColor);
  }

  public void BorderAround(OfficeLineStyle borderLine, OfficeKnownColors borderColor)
  {
    int iSheetIndex = 0;
    for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      this.GetRange(iSheetIndex).BorderAround(borderLine, borderColor);
  }

  public void BorderInside() => this.BorderInside(OfficeLineStyle.Thin);

  public void BorderInside(OfficeLineStyle borderLine)
  {
    this.BorderInside(borderLine, OfficeKnownColors.Black);
  }

  public void BorderInside(OfficeLineStyle borderLine, Color borderColor)
  {
    OfficeKnownColors nearestColor = this.Workbook.GetNearestColor(borderColor);
    this.BorderInside(borderLine, nearestColor);
  }

  public void BorderInside(OfficeLineStyle borderLine, OfficeKnownColors borderColor)
  {
    int iSheetIndex = 0;
    for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      this.GetRange(iSheetIndex).BorderInside(borderLine, borderColor);
  }

  public void BorderNone()
  {
    int iSheetIndex = 0;
    for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      this.GetRange(iSheetIndex).BorderNone();
  }

  public void CollapseGroup(OfficeGroupBy groupBy)
  {
    int iSheetIndex = 0;
    for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      this.GetRange(iSheetIndex).CollapseGroup(groupBy);
  }

  public void ExpandGroup(OfficeGroupBy groupBy)
  {
    int iSheetIndex = 0;
    for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      this.GetRange(iSheetIndex).ExpandGroup(groupBy);
  }

  public void ExpandGroup(OfficeGroupBy groupBy, ExpandCollapseFlags flags)
  {
    int iSheetIndex = 0;
    for (int count = this.m_sheetGroup.Count; iSheetIndex < count; ++iSheetIndex)
      this.GetRange(iSheetIndex).ExpandGroup(groupBy, flags);
  }

  public IEnumerator GetEnumerator() => throw new NotImplementedException();
}
