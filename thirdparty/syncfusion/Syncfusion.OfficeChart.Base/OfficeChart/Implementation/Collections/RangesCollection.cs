// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.RangesCollection
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser.Biff_Records.Formula;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections;

internal class RangesCollection : 
  CollectionBaseEx<IRange>,
  IEnumerable<IRange>,
  IRanges,
  ICombinedRange,
  IRange,
  IParentApplication,
  IEnumerable,
  INativePTG
{
  private const string DEF_WRONG_WORKSHEET = "Can't operate with ranges from different worksheet";
  private IWorksheet m_worksheet;
  private int m_iFirstRow;
  private int m_iFirstColumn;
  private int m_iLastRow;
  private int m_iLastColumn;
  private RTFStringArray m_rtfString;

  public RangesCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
    this.m_iFirstRow = this.m_worksheet.Workbook.MaxRowCount + 1;
    this.m_iFirstColumn = this.m_worksheet.Workbook.MaxColumnCount + 1;
  }

  private void SetParents()
  {
    this.m_worksheet = this.FindParent(typeof (IWorksheet)) as IWorksheet;
    if (this.m_worksheet == null)
      throw new ArgumentNullException("Worksheet", "Can't find parent worksheet");
  }

  public string Address
  {
    get
    {
      this.CheckDisposed();
      StringBuilder stringBuilder = new StringBuilder();
      if (this.Count == 0)
        return string.Empty;
      IRange inner1 = this.InnerList[0];
      stringBuilder.Append(inner1.Address);
      string addressSeparator = this.GetAddressSeparator();
      int index = 1;
      for (int count = this.Count; index < count; ++index)
      {
        stringBuilder.Append(addressSeparator);
        IRange inner2 = this.InnerList[index];
        stringBuilder.Append(inner2.Address);
      }
      return stringBuilder.ToString();
    }
  }

  public string AddressLocal
  {
    get
    {
      this.CheckDisposed();
      StringBuilder stringBuilder = new StringBuilder();
      if (this.Count == 0)
        return string.Empty;
      IRange inner1 = this.InnerList[0];
      stringBuilder.Append(inner1.AddressLocal);
      string addressSeparator = this.GetAddressSeparator();
      int index = 1;
      for (int count = this.Count; index < count; ++index)
      {
        stringBuilder.Append(addressSeparator);
        IRange inner2 = this.InnerList[index];
        stringBuilder.Append(inner2.AddressLocal);
      }
      return stringBuilder.ToString();
    }
  }

  public string AddressGlobal
  {
    get
    {
      this.CheckDisposed();
      StringBuilder stringBuilder = new StringBuilder();
      if (this.Count == 0)
        return string.Empty;
      IRange inner1 = this.InnerList[0];
      stringBuilder.Append(inner1.AddressGlobal);
      string addressSeparator = this.GetAddressSeparator();
      int index = 1;
      for (int count = this.Count; index < count; ++index)
      {
        stringBuilder.Append(addressSeparator);
        IRange inner2 = this.InnerList[index];
        stringBuilder.Append(inner2.AddressGlobal);
      }
      return stringBuilder.ToString();
    }
  }

  public string AddressR1C1
  {
    get
    {
      this.CheckDisposed();
      StringBuilder stringBuilder = new StringBuilder();
      if (this.Count == 0)
        return string.Empty;
      IRange inner1 = this.InnerList[0];
      stringBuilder.Append(inner1.AddressR1C1);
      string addressSeparator = this.GetAddressSeparator();
      int index = 1;
      for (int count = this.Count; index < count; ++index)
      {
        stringBuilder.Append(addressSeparator);
        IRange inner2 = this.InnerList[index];
        stringBuilder.Append(inner2.AddressR1C1);
      }
      return stringBuilder.ToString();
    }
  }

  public string AddressR1C1Local
  {
    get
    {
      this.CheckDisposed();
      StringBuilder stringBuilder = new StringBuilder();
      if (this.Count == 0)
        return string.Empty;
      IRange inner1 = this.InnerList[0];
      stringBuilder.Append(inner1.AddressR1C1Local);
      string addressSeparator = this.GetAddressSeparator();
      int index = 1;
      for (int count = this.Count; index < count; ++index)
      {
        stringBuilder.Append(addressSeparator);
        IRange inner2 = this.InnerList[index];
        stringBuilder.Append(inner2.AddressR1C1Local);
      }
      return stringBuilder.ToString();
    }
  }

  public bool Boolean
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return false;
      bool boolean = this.InnerList[0].Boolean;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (boolean != inner.Boolean)
          return false;
      }
      return boolean;
    }
    set
    {
      this.CheckDisposed();
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this.InnerList[index].Boolean = value;
    }
  }

  public IBorders Borders
  {
    get
    {
      this.CheckDisposed();
      return this.CellStyle.Borders;
    }
  }

  public IRange[] Cells
  {
    get
    {
      this.CheckDisposed();
      System.Collections.Generic.List<IRange> rangeList = new System.Collections.Generic.List<IRange>();
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        rangeList.AddRange((IEnumerable<IRange>) inner.Cells);
      }
      return rangeList.ToArray();
    }
  }

  public int Column
  {
    get
    {
      this.CheckDisposed();
      return this.m_iFirstColumn;
    }
  }

  public int ColumnGroupLevel
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return int.MinValue;
      int columnGroupLevel = this.InnerList[0].ColumnGroupLevel;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (columnGroupLevel != inner.ColumnGroupLevel)
          return int.MinValue;
      }
      return columnGroupLevel;
    }
  }

  public double ColumnWidth
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return double.MinValue;
      double columnWidth = this.InnerList[0].ColumnWidth;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (columnWidth != inner.ColumnWidth)
          return double.MinValue;
      }
      return columnWidth;
    }
    set
    {
      this.CheckDisposed();
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this.InnerList[index].ColumnWidth = value;
    }
  }

  int IRange.Count
  {
    get
    {
      this.CheckDisposed();
      int count1 = 0;
      int index = 0;
      for (int count2 = this.Count; index < count2; ++index)
      {
        IRange inner = this.InnerList[index];
        count1 += inner.Count;
      }
      return count1;
    }
  }

  public DateTime DateTime
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return DateTime.MinValue;
      DateTime dateTime = this.InnerList[0].DateTime;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (dateTime != inner.DateTime)
          return DateTime.MinValue;
      }
      return dateTime;
    }
    set
    {
      this.CheckDisposed();
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this.InnerList[index].DateTime = value;
    }
  }

  public string DisplayText
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return (string) null;
      string displayText = this.InnerList[0].DisplayText;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (displayText != inner.DisplayText)
          return (string) null;
      }
      return displayText;
    }
  }

  public IRange End
  {
    get
    {
      this.CheckDisposed();
      return this.m_iLastRow < 1 || this.m_iLastColumn < 1 ? (IRange) null : this.Worksheet[this.m_iLastRow, this.m_iLastColumn];
    }
  }

  public IRange EntireColumn
  {
    get
    {
      this.CheckDisposed();
      return this.GetEntireColumnRow(true);
    }
  }

  public IRange EntireRow
  {
    get
    {
      this.CheckDisposed();
      return this.GetEntireColumnRow(false);
    }
  }

  public string Error
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return (string) null;
      string error = this.InnerList[0].Error;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (error != inner.Error)
          return (string) null;
      }
      return error;
    }
    set
    {
      this.CheckDisposed();
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this.InnerList[index].Error = value;
    }
  }

  public string Formula
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return (string) null;
      string formula = this.InnerList[0].Formula;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (formula != inner.Formula)
          return (string) null;
      }
      return formula;
    }
    set
    {
      this.CheckDisposed();
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this.InnerList[index].Formula = value;
    }
  }

  public string FormulaR1C1
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return (string) null;
      string formulaR1C1 = this.InnerList[0].FormulaR1C1;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (formulaR1C1 != inner.FormulaR1C1)
          return (string) null;
      }
      return formulaR1C1;
    }
    set
    {
      this.CheckDisposed();
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this.InnerList[index].FormulaR1C1 = value;
    }
  }

  public string FormulaArray
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return (string) null;
      string formulaArray = this.InnerList[0].FormulaArray;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (formulaArray != inner.FormulaArray)
          return (string) null;
      }
      return formulaArray;
    }
    set
    {
      this.CheckDisposed();
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this.InnerList[index].FormulaArray = value;
    }
  }

  public string FormulaArrayR1C1
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return (string) null;
      string formulaArrayR1C1 = this.InnerList[0].FormulaArrayR1C1;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (formulaArrayR1C1 != inner.FormulaArrayR1C1)
          return (string) null;
      }
      return formulaArrayR1C1;
    }
    set
    {
      this.CheckDisposed();
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this.InnerList[index].FormulaArrayR1C1 = value;
    }
  }

  public bool FormulaHidden
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return false;
      bool formulaHidden = this.InnerList[0].FormulaHidden;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (formulaHidden != inner.FormulaHidden)
          return false;
      }
      return formulaHidden;
    }
    set
    {
      this.CheckDisposed();
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this.InnerList[index].FormulaHidden = value;
    }
  }

  public DateTime FormulaDateTime
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return DateTime.MinValue;
      DateTime formulaDateTime = this.InnerList[0].FormulaDateTime;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (formulaDateTime != inner.FormulaDateTime)
          return DateTime.MinValue;
      }
      return formulaDateTime;
    }
    set
    {
      this.CheckDisposed();
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this.InnerList[index].FormulaDateTime = value;
    }
  }

  public bool HasDataValidation
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return false;
      IRange inner = this.InnerList[0];
      return false;
    }
  }

  public bool HasBoolean
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return false;
      bool hasBoolean = this.InnerList[0].HasBoolean;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (hasBoolean != inner.HasBoolean)
          return false;
      }
      return hasBoolean;
    }
  }

  public bool HasDateTime
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return false;
      bool hasDateTime = this.InnerList[0].HasDateTime;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (hasDateTime != inner.HasDateTime)
          return false;
      }
      return hasDateTime;
    }
  }

  public bool HasFormulaBoolValue
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return false;
      bool formulaBoolValue = this.InnerList[0].HasFormulaBoolValue;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (formulaBoolValue != inner.HasFormulaBoolValue)
          return false;
      }
      return formulaBoolValue;
    }
  }

  public bool HasFormulaErrorValue
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return false;
      bool formulaErrorValue = this.InnerList[0].HasFormulaErrorValue;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (formulaErrorValue != inner.HasFormulaErrorValue)
          return false;
      }
      return formulaErrorValue;
    }
  }

  public bool HasFormulaDateTime
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return false;
      bool hasFormulaDateTime = this.InnerList[0].HasFormulaDateTime;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (hasFormulaDateTime != inner.HasFormulaDateTime)
          return false;
      }
      return hasFormulaDateTime;
    }
  }

  public bool HasFormulaNumberValue
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return false;
      bool formulaNumberValue = this.InnerList[0].HasFormulaNumberValue;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (formulaNumberValue != inner.HasFormulaNumberValue)
          return false;
      }
      return formulaNumberValue;
    }
  }

  public bool HasFormulaStringValue
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return false;
      bool formulaStringValue = this.InnerList[0].HasFormulaStringValue;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (formulaStringValue != inner.HasFormulaStringValue)
          return false;
      }
      return formulaStringValue;
    }
  }

  public bool HasFormula
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return false;
      bool hasFormula = this.InnerList[0].HasFormula;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (hasFormula != inner.HasFormula)
          return false;
      }
      return hasFormula;
    }
  }

  public bool HasFormulaArray
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return false;
      bool hasFormulaArray = this.InnerList[0].HasFormulaArray;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (hasFormulaArray != inner.HasFormulaArray)
          return false;
      }
      return hasFormulaArray;
    }
  }

  public bool HasNumber
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return false;
      bool hasNumber = this.InnerList[0].HasNumber;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (hasNumber != inner.HasNumber)
          return false;
      }
      return hasNumber;
    }
  }

  public bool HasRichText
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return false;
      bool hasRichText = this.InnerList[0].HasRichText;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (hasRichText != inner.HasRichText)
          return false;
      }
      return hasRichText;
    }
  }

  public bool HasString
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return false;
      bool hasString = this.InnerList[0].HasString;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (hasString != inner.HasString)
          return false;
      }
      return hasString;
    }
  }

  public bool HasStyle
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return false;
      bool hasStyle = this.InnerList[0].HasStyle;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (hasStyle != inner.HasStyle)
          return false;
      }
      return hasStyle;
    }
  }

  public OfficeHAlign HorizontalAlignment
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return OfficeHAlign.HAlignGeneral;
      OfficeHAlign horizontalAlignment = this.InnerList[0].HorizontalAlignment;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (horizontalAlignment != inner.HorizontalAlignment)
          return OfficeHAlign.HAlignGeneral;
      }
      return horizontalAlignment;
    }
    set
    {
      this.CheckDisposed();
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this.InnerList[index].HorizontalAlignment = value;
    }
  }

  public int IndentLevel
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return int.MinValue;
      int indentLevel = this.InnerList[0].IndentLevel;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (indentLevel != inner.IndentLevel)
          return int.MinValue;
      }
      return indentLevel;
    }
    set
    {
      this.CheckDisposed();
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this.InnerList[index].IndentLevel = value;
    }
  }

  public bool IsBlank
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return false;
      bool isBlank = this.InnerList[0].IsBlank;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (isBlank != inner.IsBlank)
          return false;
      }
      return isBlank;
    }
  }

  public bool IsBoolean
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return false;
      bool isBoolean = this.InnerList[0].IsBoolean;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (isBoolean != inner.IsBoolean)
          return false;
      }
      return isBoolean;
    }
  }

  public bool IsError
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return false;
      bool isError = this.InnerList[0].IsError;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (isError != inner.IsError)
          return false;
      }
      return isError;
    }
  }

  public bool IsGroupedByColumn
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return false;
      bool isGroupedByColumn = this.InnerList[0].IsGroupedByColumn;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (isGroupedByColumn != inner.IsGroupedByColumn)
          return false;
      }
      return isGroupedByColumn;
    }
  }

  public bool IsGroupedByRow
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return false;
      bool isGroupedByRow = this.InnerList[0].IsGroupedByRow;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (isGroupedByRow != inner.IsGroupedByRow)
          return false;
      }
      return isGroupedByRow;
    }
  }

  public bool IsInitialized
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return false;
      bool isInitialized = this.InnerList[0].IsInitialized;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (isInitialized != inner.IsInitialized)
          return false;
      }
      return isInitialized;
    }
  }

  public int LastColumn => this.m_iLastColumn;

  public int LastRow => this.m_iLastRow;

  public double Number
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return double.MinValue;
      double number = this.InnerList[0].Number;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (number != inner.Number)
          return double.MinValue;
      }
      return number;
    }
    set
    {
      this.CheckDisposed();
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this.InnerList[index].Number = value;
    }
  }

  public string NumberFormat
  {
    get
    {
      this.CheckDisposed();
      return RangeImpl.GetNumberFormat((IList) this.InnerList);
    }
    set
    {
      this.CheckDisposed();
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this.InnerList[index].NumberFormat = value;
    }
  }

  public int Row
  {
    get
    {
      this.CheckDisposed();
      return this.m_iFirstRow;
    }
  }

  public int RowGroupLevel
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return int.MinValue;
      int rowGroupLevel = this.InnerList[0].RowGroupLevel;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (rowGroupLevel != inner.RowGroupLevel)
          return int.MinValue;
      }
      return rowGroupLevel;
    }
  }

  public double RowHeight
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return double.MinValue;
      double rowHeight = this.InnerList[0].RowHeight;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (rowHeight != inner.RowHeight)
          return double.MinValue;
      }
      return rowHeight;
    }
    set
    {
      this.CheckDisposed();
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this.InnerList[index].RowHeight = value;
    }
  }

  public IRange[] Rows
  {
    get
    {
      this.CheckDisposed();
      return this.GetColumnRows(false);
    }
  }

  public IRange[] Columns
  {
    get
    {
      this.CheckDisposed();
      return this.GetColumnRows(true);
    }
  }

  public IStyle CellStyle
  {
    get
    {
      this.CheckDisposed();
      return (IStyle) new StyleArrayWrapper((IRange) this);
    }
    set
    {
      this.CheckDisposed();
      this.CellStyleName = value != null ? value.Name : throw new ArgumentNullException(nameof (CellStyle));
    }
  }

  public string CellStyleName
  {
    get
    {
      this.CheckDisposed();
      return RangeImpl.GetCellStyleName(this.List);
    }
    set
    {
      this.CheckDisposed();
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this.InnerList[index].CellStyleName = value;
    }
  }

  public string Text
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return (string) null;
      string text = this.InnerList[0].Text;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (text != inner.Text)
          return (string) null;
      }
      return text;
    }
    set
    {
      this.CheckDisposed();
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this.InnerList[index].Text = value;
    }
  }

  public TimeSpan TimeSpan
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return TimeSpan.MinValue;
      TimeSpan timeSpan = this.InnerList[0].TimeSpan;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (timeSpan != inner.TimeSpan)
          return TimeSpan.MinValue;
      }
      return timeSpan;
    }
    set
    {
      this.CheckDisposed();
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this.InnerList[index].TimeSpan = value;
    }
  }

  public string Value
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return (string) null;
      string str = this.InnerList[0].Value;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (str != inner.Value)
          return (string) null;
      }
      return str;
    }
    set
    {
      this.CheckDisposed();
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this.InnerList[index].Value = value;
    }
  }

  public object Value2
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return (object) null;
      object obj = this.InnerList[0].Value2;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (!obj.Equals(inner.Value2))
          return (object) null;
      }
      return obj;
    }
    set
    {
      this.CheckDisposed();
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this.InnerList[index].Value2 = value;
    }
  }

  public OfficeVAlign VerticalAlignment
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return OfficeVAlign.VAlignTop;
      OfficeVAlign verticalAlignment = this.InnerList[0].VerticalAlignment;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (verticalAlignment != inner.VerticalAlignment)
          return OfficeVAlign.VAlignTop;
      }
      return verticalAlignment;
    }
    set
    {
      this.CheckDisposed();
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this.InnerList[index].VerticalAlignment = value;
    }
  }

  public IWorksheet Worksheet
  {
    get
    {
      this.CheckDisposed();
      return this.m_worksheet;
    }
  }

  public IRange this[int row, int column]
  {
    get
    {
      this.CheckDisposed();
      return this.Worksheet.UsedRange[row, column];
    }
    set
    {
      this.CheckDisposed();
      this.Worksheet.UsedRange[row, column] = value;
    }
  }

  public IRange this[int row, int column, int lastRow, int lastColumn]
  {
    get
    {
      this.CheckDisposed();
      return this.Worksheet.UsedRange[row, column, lastRow, lastColumn];
    }
  }

  public IRange this[string name] => this[name, false];

  public IRange this[string name, bool IsR1C1Notation]
  {
    get
    {
      this.CheckDisposed();
      return this.Worksheet.UsedRange[name, IsR1C1Notation];
    }
  }

  public string FormulaStringValue
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return (string) null;
      string formulaStringValue = this.InnerList[0].FormulaStringValue;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (formulaStringValue != inner.FormulaStringValue)
          return (string) null;
      }
      return formulaStringValue;
    }
    set
    {
      this.CheckDisposed();
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this.InnerList[index].FormulaStringValue = value;
    }
  }

  public double FormulaNumberValue
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return double.MinValue;
      double formulaNumberValue = this.InnerList[0].FormulaNumberValue;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (formulaNumberValue != inner.FormulaNumberValue)
          return double.MinValue;
      }
      return formulaNumberValue;
    }
    set
    {
      this.CheckDisposed();
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this.InnerList[index].FormulaNumberValue = value;
    }
  }

  public bool FormulaBoolValue
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return false;
      bool formulaBoolValue = this.InnerList[0].FormulaBoolValue;
      int index = 1;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (formulaBoolValue != inner.FormulaBoolValue)
          return false;
      }
      return formulaBoolValue;
    }
    set
    {
      this.CheckDisposed();
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this.InnerList[index].FormulaBoolValue = value;
    }
  }

  public string FormulaErrorValue
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return (string) null;
      string formulaErrorValue = this.InnerList[0].FormulaErrorValue;
      int index = 1;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (formulaErrorValue != inner.FormulaErrorValue)
          return (string) null;
      }
      return formulaErrorValue;
    }
    set
    {
      this.CheckDisposed();
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this.InnerList[index].FormulaErrorValue = value;
    }
  }

  public IRichTextString RichText
  {
    get
    {
      this.CheckDisposed();
      if (this.m_rtfString == null)
        this.m_rtfString = new RTFStringArray((IRange) this);
      return (IRichTextString) this.m_rtfString;
    }
  }

  public bool IsMerged
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return false;
      bool isMerged = this.InnerList[0].IsMerged;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        if (isMerged != inner.IsMerged)
          return false;
      }
      return isMerged;
    }
  }

  public IRange MergeArea
  {
    get
    {
      this.CheckDisposed();
      RangesCollection rangesCollection = this.AppImplementation.CreateRangesCollection((object) this.Worksheet);
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        RangeImpl inner = (RangeImpl) this.InnerList[index];
        rangesCollection.Add(inner.MergeArea);
      }
      return (IRange) rangesCollection;
    }
  }

  public bool WrapText
  {
    get
    {
      this.CheckDisposed();
      return RangeImpl.GetWrapText((IList) this.Cells);
    }
    set
    {
      this.CheckDisposed();
      RangeImpl.SetWrapText((IList) this.Cells, value);
    }
  }

  public bool HasExternalFormula
  {
    get
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        if (!this.InnerList[index].HasExternalFormula)
          return false;
      }
      return true;
    }
  }

  public ExcelIgnoreError IgnoreErrorOptions
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return ExcelIgnoreError.None;
      ExcelIgnoreError ignoreErrorOptions = ExcelIgnoreError.All;
      System.Collections.Generic.List<IRange> innerList = this.InnerList;
      return ignoreErrorOptions;
    }
    set
    {
      this.CheckDisposed();
      System.Collections.Generic.List<IRange> innerList = this.InnerList;
    }
  }

  public bool? IsStringsPreserved
  {
    get => (this.m_worksheet as WorksheetImpl).GetStringPreservedValue((ICombinedRange) this);
    set
    {
      (this.m_worksheet as WorksheetImpl).SetStringPreservedValue((ICombinedRange) this, value);
    }
  }

  public BuiltInStyles? BuiltInStyle
  {
    get
    {
      this.CheckDisposed();
      if (this.Count == 0)
        return new BuiltInStyles?();
      BuiltInStyles? builtInStyle1 = this.InnerList[0].BuiltInStyle;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IRange inner = this.InnerList[index];
        BuiltInStyles? nullable = builtInStyle1;
        BuiltInStyles? builtInStyle2 = inner.BuiltInStyle;
        if ((nullable.GetValueOrDefault() != builtInStyle2.GetValueOrDefault() ? 1 : (nullable.HasValue != builtInStyle2.HasValue ? 1 : 0)) != 0)
        {
          builtInStyle1 = new BuiltInStyles?();
          break;
        }
      }
      return builtInStyle1;
    }
    set
    {
      this.CheckDisposed();
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this.InnerList[index].BuiltInStyle = value;
    }
  }

  public string AddressGlobal2007 => this.AddressGlobal;

  public IRange Activate()
  {
    this.CheckDisposed();
    return (IRange) null;
  }

  public IRange Activate(bool scroll)
  {
    this.CheckDisposed();
    return (IRange) null;
  }

  public IRange Group(OfficeGroupBy groupBy)
  {
    this.CheckDisposed();
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this.InnerList[index].Group(groupBy);
    return (IRange) this;
  }

  public IRange Group(OfficeGroupBy groupBy, bool bCollapsed)
  {
    this.CheckDisposed();
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this.InnerList[index].Group(groupBy, bCollapsed);
    return (IRange) this;
  }

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
    this.CheckDisposed();
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this.InnerList[index].Merge();
  }

  public void Merge(bool clearCells)
  {
    this.CheckDisposed();
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this.InnerList[index].Merge(clearCells);
  }

  public IRange Ungroup(OfficeGroupBy groupBy)
  {
    this.CheckDisposed();
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this.InnerList[index].Ungroup(groupBy);
    return (IRange) this;
  }

  public void UnMerge()
  {
    this.CheckDisposed();
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this.InnerList[index].UnMerge();
  }

  public void FreezePanes()
  {
    this.CheckDisposed();
    if (this.Count != 1)
      return;
    this.InnerList[0].FreezePanes();
  }

  void IRange.Clear()
  {
    this.CheckDisposed();
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this.InnerList[index].Clear();
  }

  void IRange.Clear(bool isClearFormat)
  {
    this.CheckDisposed();
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this.InnerList[index].Clear(isClearFormat);
  }

  void IRange.Clear(OfficeClearOptions option)
  {
    this.CheckDisposed();
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this.InnerList[index].Clear(option);
  }

  void IRange.Clear(OfficeMoveDirection direction)
  {
    this.CheckDisposed();
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this.InnerList[index].Clear(direction);
  }

  void IRange.Clear(OfficeMoveDirection direction, OfficeCopyRangeOptions options)
  {
    this.CheckDisposed();
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this.InnerList[index].Clear(direction, options);
  }

  public void MoveTo(IRange destination)
  {
    this.CheckDisposed();
    if (destination == null)
      throw new ArgumentNullException(nameof (destination));
    int num1 = destination.Row - this.Row;
    int num2 = destination.Column - this.Column;
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      IRange inner = this.InnerList[index];
      int row = inner.Row + num1;
      int column = inner.Column + num2;
      if (row <= this.m_worksheet.Workbook.MaxRowCount && row > 0 && column <= this.m_worksheet.Workbook.MaxColumnCount && column > 0)
        inner.MoveTo(destination.Worksheet[row, column]);
    }
  }

  public IRange CopyTo(IRange destination) => this.CopyTo(destination, OfficeCopyRangeOptions.All);

  public IRange CopyTo(IRange destination, OfficeCopyRangeOptions options)
  {
    this.CheckDisposed();
    if (destination == null)
      throw new ArgumentNullException(nameof (destination));
    int num1 = destination.Row - this.Row;
    int num2 = destination.Column - this.Column;
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      IRange inner = this.InnerList[index];
      int row = inner.Row + num1;
      int column = inner.Column + num2;
      if (row <= this.m_worksheet.Workbook.MaxRowCount && row > 0 && column <= this.m_worksheet.Workbook.MaxColumnCount && column > 0)
        inner.CopyTo(destination.Worksheet[row, column], options);
    }
    return destination;
  }

  public IRange IntersectWith(IRange range)
  {
    this.CheckDisposed();
    RangesCollection rangesCollection = this.AppImplementation.CreateRangesCollection((object) this.Worksheet);
    int index = 0;
    for (int count = rangesCollection.Count; index < count; ++index)
    {
      IRange inner = this.InnerList[index];
      rangesCollection.Add(inner.IntersectWith(range));
    }
    return rangesCollection.Count <= 0 ? (IRange) null : (IRange) rangesCollection;
  }

  public IRange MergeWith(IRange range) => throw new NotImplementedException();

  public void AutofitRows()
  {
    this.CheckDisposed();
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this.InnerList[index].AutofitRows();
  }

  public void AutofitColumns()
  {
    this.CheckDisposed();
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this.InnerList[index].AutofitColumns();
  }

  public IRange FindFirst(string findValue, OfficeFindType flags)
  {
    this.CheckDisposed();
    if (findValue == null)
      return (IRange) null;
    bool flag1 = (flags & OfficeFindType.Formula) == OfficeFindType.Formula;
    bool flag2 = (flags & OfficeFindType.Text) == OfficeFindType.Text;
    bool flag3 = (flags & OfficeFindType.FormulaStringValue) == OfficeFindType.FormulaStringValue;
    bool flag4 = (flags & OfficeFindType.Error) == OfficeFindType.Error;
    if (!flag1 && !flag2 && !flag3 && !flag4)
      throw new ArgumentException("Parameter flag is not valid.", nameof (flags));
    IList innerList = (IList) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      IRange first = ((IRange) innerList[index]).FindFirst(findValue, flags);
      if (first != null)
        return first;
    }
    return (IRange) null;
  }

  public IRange FindFirst(double findValue, OfficeFindType flags)
  {
    this.CheckDisposed();
    bool flag1 = (flags & OfficeFindType.FormulaValue) == OfficeFindType.FormulaValue;
    bool flag2 = (flags & OfficeFindType.Number) == OfficeFindType.Number;
    if (!flag1 && !flag2)
      throw new ArgumentException("Parameter flag is not valid.", nameof (flags));
    IList innerList = (IList) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      IRange first = ((IRange) innerList[index]).FindFirst(findValue, flags);
      if (first != null)
        return first;
    }
    return (IRange) null;
  }

  public IRange FindFirst(bool findValue)
  {
    this.CheckDisposed();
    IList innerList = (IList) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      IRange first = ((IRange) innerList[index]).FindFirst(findValue);
      if (first != null)
        return first;
    }
    return (IRange) null;
  }

  public IRange FindFirst(DateTime findValue)
  {
    this.CheckDisposed();
    IList innerList = (IList) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      IRange first = ((IRange) innerList[index]).FindFirst(findValue);
      if (first != null)
        return first;
    }
    return (IRange) null;
  }

  public IRange FindFirst(TimeSpan findValue)
  {
    this.CheckDisposed();
    IList innerList = (IList) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      IRange first = ((IRange) innerList[index]).FindFirst(findValue);
      if (first != null)
        return first;
    }
    return (IRange) null;
  }

  public IRange[] FindAll(DateTime findValue)
  {
    this.CheckDisposed();
    System.Collections.Generic.List<IRange> rangeList = new System.Collections.Generic.List<IRange>();
    IList<IRange> innerList = (IList<IRange>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      IRange[] all = innerList[index].FindAll(findValue);
      if (all != null)
        rangeList.AddRange((IEnumerable<IRange>) all);
    }
    return rangeList.Count == 0 ? (IRange[]) null : rangeList.ToArray();
  }

  public IRange[] FindAll(TimeSpan findValue)
  {
    this.CheckDisposed();
    System.Collections.Generic.List<IRange> rangeList = new System.Collections.Generic.List<IRange>();
    System.Collections.Generic.List<IRange> innerList = this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      IRange[] all = innerList[index].FindAll(findValue);
      if (all != null)
        rangeList.AddRange((IEnumerable<IRange>) all);
    }
    return rangeList.Count == 0 ? (IRange[]) null : rangeList.ToArray();
  }

  public IRange[] FindAll(string findValue, OfficeFindType flags)
  {
    this.CheckDisposed();
    if (findValue == null)
      return (IRange[]) null;
    bool flag1 = (flags & OfficeFindType.Formula) == OfficeFindType.Formula;
    bool flag2 = (flags & OfficeFindType.Text) == OfficeFindType.Text;
    bool flag3 = (flags & OfficeFindType.FormulaStringValue) == OfficeFindType.FormulaStringValue;
    bool flag4 = (flags & OfficeFindType.Error) == OfficeFindType.Error;
    if (!flag1 && !flag2 && !flag3 && !flag4)
      throw new ArgumentException("Parameter flag is not valid.", nameof (flags));
    if (findValue == null)
      return (IRange[]) null;
    System.Collections.Generic.List<IRange> rangeList = new System.Collections.Generic.List<IRange>();
    System.Collections.Generic.List<IRange> innerList = this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      IRange[] all = innerList[index].FindAll(findValue, flags);
      if (all != null)
        rangeList.AddRange((IEnumerable<IRange>) all);
    }
    return rangeList.Count == 0 ? (IRange[]) null : rangeList.ToArray();
  }

  public IRange[] FindAll(double findValue, OfficeFindType flags)
  {
    this.CheckDisposed();
    bool flag1 = (flags & OfficeFindType.FormulaValue) == OfficeFindType.FormulaValue;
    bool flag2 = (flags & OfficeFindType.Number) == OfficeFindType.Number;
    if (!flag1 && !flag2)
      throw new ArgumentException("Parameter flag is not valid.", nameof (flags));
    System.Collections.Generic.List<IRange> rangeList = new System.Collections.Generic.List<IRange>();
    System.Collections.Generic.List<IRange> innerList = this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      IRange[] all = innerList[index].FindAll(findValue, flags);
      if (all != null)
        rangeList.AddRange((IEnumerable<IRange>) all);
    }
    return rangeList.Count == 0 ? (IRange[]) null : rangeList.ToArray();
  }

  public IRange[] FindAll(bool findValue)
  {
    this.CheckDisposed();
    System.Collections.Generic.List<IRange> rangeList = new System.Collections.Generic.List<IRange>();
    System.Collections.Generic.List<IRange> innerList = this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      IRange[] all = innerList[index].FindAll(findValue);
      if (all != null)
        rangeList.AddRange((IEnumerable<IRange>) all);
    }
    return rangeList.Count == 0 ? (IRange[]) null : rangeList.ToArray();
  }

  public void CopyToClipboard() => throw new NotSupportedException();

  public void BorderAround() => this.BorderAround(OfficeLineStyle.Thin);

  public void BorderAround(OfficeLineStyle borderLine)
  {
    this.BorderAround(borderLine, OfficeKnownColors.Black);
  }

  public void BorderAround(OfficeLineStyle borderLine, Color borderColor)
  {
    OfficeKnownColors nearestColor = this.m_worksheet.Workbook.GetNearestColor(borderColor);
    this.BorderAround(borderLine, nearestColor);
  }

  public void BorderAround(OfficeLineStyle borderLine, OfficeKnownColors borderColor)
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this.InnerList[index].BorderAround(borderLine, borderColor);
  }

  public void BorderInside() => this.BorderInside(OfficeLineStyle.Thin);

  public void BorderInside(OfficeLineStyle borderLine)
  {
    this.BorderInside(borderLine, OfficeKnownColors.Black);
  }

  public void BorderInside(OfficeLineStyle borderLine, Color borderColor)
  {
    OfficeKnownColors nearestColor = this.m_worksheet.Workbook.GetNearestColor(borderColor);
    this.BorderInside(borderLine, nearestColor);
  }

  public void BorderInside(OfficeLineStyle borderLine, OfficeKnownColors borderColor)
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this.InnerList[index].BorderInside(borderLine, borderColor);
  }

  public void BorderNone()
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this.InnerList[index].BorderNone();
  }

  public void CollapseGroup(OfficeGroupBy groupBy)
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this.InnerList[index].CollapseGroup(groupBy);
  }

  public void ExpandGroup(OfficeGroupBy groupBy)
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this.InnerList[index].ExpandGroup(groupBy);
  }

  public void ExpandGroup(OfficeGroupBy groupBy, ExpandCollapseFlags flags)
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this.InnerList[index].ExpandGroup(groupBy, flags);
  }

  public string GetNewAddress(Dictionary<string, string> names, out string strSheetName)
  {
    strSheetName = this.m_worksheet.Name;
    if (names == null)
      return this.Address;
    StringBuilder stringBuilder = new StringBuilder();
    int count = this.Count;
    if (count == 0)
      return string.Empty;
    IRange inner1 = this.InnerList[0];
    stringBuilder.Append(((ICombinedRange) inner1).GetNewAddress(names, out strSheetName));
    string addressSeparator = this.GetAddressSeparator();
    for (int index = 1; index < count; ++index)
    {
      stringBuilder.Append(addressSeparator);
      IRange inner2 = this.InnerList[index];
      stringBuilder.Append(((ICombinedRange) inner2).GetNewAddress(names, out strSheetName));
    }
    return stringBuilder.ToString();
  }

  public IRange Clone(object parent, Dictionary<string, string> hashNewNames, WorkbookImpl book)
  {
    RangesCollection parent1 = new RangesCollection(this.Application, (object) (this.m_worksheet as IInternalWorksheet).GetClonedObject(hashNewNames, book));
    System.Collections.Generic.List<IRange> innerList = this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      object obj = (object) ((ICombinedRange) innerList[index]).Clone((object) parent1, hashNewNames, book);
      parent1.Add((IRange) obj);
    }
    return (IRange) parent1;
  }

  public int CellsCount
  {
    get
    {
      int cellsCount = 0;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        ICombinedRange inner = (ICombinedRange) this.InnerList[index];
        cellsCount += inner.CellsCount;
      }
      return cellsCount;
    }
  }

  public void ClearConditionalFormats()
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      ((ICombinedRange) this[index]).ClearConditionalFormats();
  }

  public Rectangle[] GetRectangles()
  {
    int length = 0;
    int count = this.Count;
    for (int index = 0; index < count; ++index)
    {
      ICombinedRange combinedRange = (ICombinedRange) this[index];
      length += combinedRange.GetRectanglesCount();
    }
    Rectangle[] rectangles1 = new Rectangle[length];
    int index1 = 0;
    int index2 = 0;
    for (; index1 < count; ++index1)
    {
      Rectangle[] rectangles2 = ((ICombinedRange) this[index1]).GetRectangles();
      rectangles2.CopyTo((Array) rectangles1, index2);
      index2 += rectangles2.Length;
    }
    return rectangles1;
  }

  public int GetRectanglesCount()
  {
    int rectanglesCount = 0;
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      ICombinedRange combinedRange = (ICombinedRange) this[index];
      rectanglesCount += combinedRange.GetRectanglesCount();
    }
    return rectanglesCount;
  }

  public string WorksheetName => this.Worksheet.Name;

  public new void Add(IRange range)
  {
    this.CheckDisposed();
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    if (range.Worksheet != this.Worksheet)
      throw new ArgumentException("Can't operate with ranges from different worksheet");
    this.m_iFirstRow = Math.Min(this.m_iFirstRow, range.Row);
    this.m_iFirstColumn = Math.Min(this.m_iFirstColumn, range.Column);
    this.m_iLastRow = Math.Max(this.m_iLastRow, range.LastRow);
    this.m_iLastColumn = Math.Max(this.m_iLastColumn, range.LastColumn);
    this.InnerList.Add(range);
  }

  public void AddRange(IRange range)
  {
    this.CheckDisposed();
    if (range is RangesCollection)
    {
      RangesCollection rangesCollection = (RangesCollection) range;
      int index = 0;
      for (int count = rangesCollection.Count; index < count; ++index)
        rangesCollection.Add(rangesCollection[index]);
    }
    else
      this.Add(range);
  }

  public void Remove(IRange range)
  {
    this.CheckDisposed();
    System.Collections.Generic.List<IRange> innerList = this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      IRange range1 = innerList[index];
      if (range.Worksheet == range1.Worksheet && range.AddressLocal == range1.AddressLocal)
      {
        innerList.RemoveAt(index);
        --index;
        --count;
      }
    }
    this.InnerList.Remove(range);
    this.EvaluateDimensions();
  }

  public new IRange this[int index]
  {
    get
    {
      this.CheckDisposed();
      return this.InnerList[index];
    }
    set
    {
      this.CheckDisposed();
      this.InnerList[index] = value != null ? value : throw new ArgumentNullException();
    }
  }

  private void EvaluateDimensions()
  {
    this.CheckDisposed();
    this.m_iFirstRow = this.m_worksheet.Workbook.MaxRowCount + 1;
    this.m_iFirstColumn = this.m_worksheet.Workbook.MaxColumnCount + 1;
    this.m_iLastRow = 0;
    this.m_iLastColumn = 0;
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      IRange inner = this.InnerList[index];
      this.m_iFirstRow = Math.Min(this.m_iFirstRow, inner.Row);
      this.m_iFirstColumn = Math.Min(this.m_iFirstColumn, inner.Column);
      this.m_iLastRow = Math.Max(this.m_iLastRow, inner.LastRow);
      this.m_iLastColumn = Math.Max(this.m_iLastColumn, inner.LastColumn);
    }
  }

  private SortedList<int, KeyValuePair<int, int>> GetColumnRowIndexes(bool bIsColumn)
  {
    this.CheckDisposed();
    SortedList<int, KeyValuePair<int, int>> list = new SortedList<int, KeyValuePair<int, int>>();
    int index1 = 0;
    for (int count1 = this.Count; index1 < count1; ++index1)
    {
      IRange inner = this.InnerList[index1];
      if (inner is RangesCollection)
      {
        SortedList<int, KeyValuePair<int, int>> columnRowIndexes = ((RangesCollection) inner).GetColumnRowIndexes(bIsColumn);
        IList<int> keys = columnRowIndexes.Keys;
        IList<KeyValuePair<int, int>> values = columnRowIndexes.Values;
        int index2 = 0;
        for (int count2 = columnRowIndexes.Count; index2 < count2; ++index2)
          this.AddRowColumnIndex(list, keys[index2], values[index2]);
      }
      else
      {
        int num1 = bIsColumn ? inner.Column : inner.Row;
        int num2 = bIsColumn ? inner.LastColumn : inner.LastRow;
        int iSecondaryStart = bIsColumn ? inner.Row : inner.Column;
        int iSecondaryEnd = bIsColumn ? inner.LastRow : inner.LastColumn;
        for (int iIndex = num1; iIndex <= num2; ++iIndex)
          this.AddRowColumnIndex(list, iIndex, iSecondaryStart, iSecondaryEnd);
      }
    }
    return list;
  }

  private void AddRowColumnIndex(
    SortedList<int, KeyValuePair<int, int>> list,
    int iIndex,
    KeyValuePair<int, int> entry)
  {
    this.CheckDisposed();
    if (list == null)
      throw new ArgumentNullException(nameof (list));
    if (list.ContainsKey(iIndex))
    {
      KeyValuePair<int, int> keyValuePair = list[iIndex];
      int key = entry.Key;
      int val2 = entry.Value;
      entry = new KeyValuePair<int, int>(Math.Min(keyValuePair.Key, key), Math.Max(keyValuePair.Value, val2));
    }
    list[iIndex] = entry;
  }

  private void AddRowColumnIndex(
    SortedList<int, KeyValuePair<int, int>> list,
    int iIndex,
    int iSecondaryStart,
    int iSecondaryEnd)
  {
    this.CheckDisposed();
    if (list == null)
      throw new ArgumentNullException(nameof (list));
    KeyValuePair<int, int> keyValuePair1;
    if (!list.ContainsKey(iIndex))
    {
      keyValuePair1 = new KeyValuePair<int, int>(iSecondaryStart, iSecondaryEnd);
    }
    else
    {
      KeyValuePair<int, int> keyValuePair2 = list[iIndex];
      keyValuePair1 = new KeyValuePair<int, int>(Math.Min(keyValuePair2.Key, iSecondaryStart), Math.Max(keyValuePair2.Value, iSecondaryEnd));
    }
    list[iIndex] = keyValuePair1;
  }

  private IRange GetEntireColumnRow(bool bIsColumn)
  {
    this.CheckDisposed();
    RangesCollection rangesCollection = this.AppImplementation.CreateRangesCollection((object) this.Worksheet);
    SortedList<int, KeyValuePair<int, int>> columnRowIndexes = this.GetColumnRowIndexes(bIsColumn);
    if (columnRowIndexes.Count == 0)
      return (IRange) null;
    IList<int> keys = columnRowIndexes.Keys;
    int num1 = keys[0];
    int num2 = num1;
    int num3 = bIsColumn ? this.m_worksheet.UsedRange.Row : this.m_worksheet.UsedRange.Column;
    int num4 = bIsColumn ? this.m_worksheet.UsedRange.LastRow : this.m_worksheet.UsedRange.LastColumn;
    int index = 1;
    for (int count = columnRowIndexes.Count; index < count; ++index)
    {
      int num5 = keys[index];
      if (num5 - num2 == 1)
      {
        num2 = num5;
      }
      else
      {
        IRange range = bIsColumn ? this.Worksheet.Range[num3, num1, num4, num2] : this.Worksheet.Range[num1, num3, num2, num4];
        rangesCollection.Add(range);
        num1 = num2 = num5;
      }
    }
    IRange range1 = bIsColumn ? this.Worksheet.Range[num3, num1, num4, num2] : this.Worksheet.Range[num1, num3, num2, num4];
    rangesCollection.Add(range1);
    return rangesCollection.Count == 1 ? rangesCollection[0] : (IRange) rangesCollection;
  }

  private IRange[] GetColumnRows(bool bIsColumn)
  {
    this.CheckDisposed();
    SortedList<int, KeyValuePair<int, int>> columnRowIndexes = this.GetColumnRowIndexes(bIsColumn);
    IList<int> keys = columnRowIndexes.Keys;
    IList<KeyValuePair<int, int>> values = columnRowIndexes.Values;
    IRange[] columnRows = new IRange[columnRowIndexes.Count];
    int index = 0;
    for (int count = columnRowIndexes.Count; index < count; ++index)
    {
      int num1 = keys[index];
      KeyValuePair<int, int> keyValuePair = values[index];
      int key = keyValuePair.Key;
      int num2 = keyValuePair.Value;
      columnRows[index] = bIsColumn ? this.m_worksheet.Range[key, num1, num2, num1] : this.m_worksheet.Range[num1, key, num1, num2];
    }
    return columnRows;
  }

  private void CheckDisposed()
  {
  }

  private string GetAddressSeparator()
  {
    bool isFormulaParsed = this.AppImplementation.IsFormulaParsed;
    this.AppImplementation.IsFormulaParsed = false;
    string listSeparator = this.AppImplementation.CheckAndApplySeperators().TextInfo.ListSeparator;
    this.AppImplementation.IsFormulaParsed = isFormulaParsed;
    return listSeparator;
  }

  public Ptg[] GetNativePtg()
  {
    int count = this.List.Count;
    if (count == 0)
      return (Ptg[]) null;
    if (this.List[0] is RangesCollection)
      throw new NotSupportedException("Not supported : Range collection as element in range collection");
    System.Collections.Generic.List<Ptg> ptgList = new System.Collections.Generic.List<Ptg>();
    INativePTG nativePtg1 = (INativePTG) this.List[0];
    Ptg ptg1 = FormulaUtil.CreatePtg(FormulaToken.tCellRangeList, (object) ",");
    ptgList.Add(nativePtg1.GetNativePtg()[0]);
    for (int index = 1; index < count; ++index)
    {
      INativePTG nativePtg2 = !(this.List[index] is RangesCollection) ? (INativePTG) this.List[index] : throw new NotSupportedException("Not supported : Range collection as element in range collection");
      ptgList.Add(nativePtg2.GetNativePtg()[0]);
      ptgList.Add(ptg1);
    }
    if (count > 1)
    {
      Ptg ptg2 = FormulaUtil.CreatePtg(FormulaToken.tParentheses, (object) "(");
      ptgList.Add(ptg2);
    }
    return ptgList.ToArray();
  }

  public IEnumerator GetEnumerator() => this.Cells.GetEnumerator();
}
