// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Tables.ListObjectColumn
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Tables;

internal class ListObjectColumn : IListObjectColumn
{
  private const string SubTotalFormat = "=SUBTOTAL({0}{1}{3}[{2}])";
  private const string CalcualtedFormulaRangeFormat = "R{0}C{2}:R{1}C{2}";
  private const RegexOptions DEF_REGEX = RegexOptions.Compiled;
  private string m_strName;
  private int m_iIndex;
  private ExcelTotalsCalculation m_totals;
  private string m_strTotalsLabel;
  private ListObject m_parentTable;
  private int m_iId;
  private Ptg[] m_calculatedFormulaPtgs;
  private int m_queryTableFieldId;
  private bool m_IsColumnNameModified;
  internal bool m_IsArrayFormula;
  private int m_mapId;
  private string m_xPath;
  private string m_dataType;

  internal bool IsColumnNameModified
  {
    get => this.m_IsColumnNameModified;
    set => this.m_IsColumnNameModified = value;
  }

  public string Name
  {
    get
    {
      IRange location = this.m_parentTable.Location;
      int row = location.Row;
      int columnIndex = this.GetColumnIndex(location);
      string str = !this.m_parentTable.Worksheet[row, columnIndex].HasString || this.m_parentTable.Worksheet[row, columnIndex].CellStyle.IsFirstSymbolApostrophe ? this.m_parentTable.Worksheet[row, columnIndex].DisplayText : this.m_parentTable.Worksheet[row, columnIndex].Text;
      if (this.m_parentTable.ShowHeaderRow && str != string.Empty && str != this.m_strName)
        this.m_strName = str;
      return this.m_strName;
    }
    set
    {
      IRange location = this.m_parentTable.Location;
      int columnIndex = this.GetColumnIndex(location);
      int column = location.Column;
      int index1 = columnIndex - column;
      List<string> columnNames = new List<string>();
      int num1 = location.Column - column;
      int num2 = location.LastColumn - column;
      for (int index2 = num1; index2 <= num2; ++index2)
        columnNames.Add(this.m_parentTable.Columns[index2].Name);
      columnNames.RemoveAt(index1);
      columnNames.Insert(index1, value);
      this.m_parentTable.UpdateColumnNames(columnNames);
      value = columnNames[index1];
      this.SetName(value);
    }
  }

  public int Index
  {
    get
    {
      this.m_iIndex = this.m_parentTable.Columns.IndexOf((IListObjectColumn) this) + 1;
      return this.m_iIndex;
    }
  }

  public ExcelTotalsCalculation TotalsCalculation
  {
    get => this.m_totals;
    set
    {
      this.m_totals = value;
      IRange totalCell = this.TotalCell;
      WorkbookImpl workbook = totalCell.Worksheet.Workbook as WorkbookImpl;
      if (workbook.Loading)
        return;
      string argumentsSeparator = workbook.ArgumentsSeparator;
      if (value != ExcelTotalsCalculation.None)
      {
        string name = this.Name;
        bool throwOnUnknownNames = workbook.ThrowOnUnknownNames;
        workbook.ThrowOnUnknownNames = false;
        string str = name.Replace("'", "''").Replace("[", "'[").Replace("]", "']").Replace("#", "'#");
        totalCell.Formula = string.Format("=SUBTOTAL({0}{1}{3}[{2}])", (object) (int) value, (object) argumentsSeparator, (object) str, (object) this.m_parentTable.Name);
        if (value == ExcelTotalsCalculation.Count || value == ExcelTotalsCalculation.CountNums)
          totalCell.NumberFormat = "General";
        workbook.ThrowOnUnknownNames = throwOnUnknownNames;
      }
      else
        totalCell.Value = string.Empty;
    }
  }

  public string TotalsRowLabel
  {
    get => this.m_strTotalsLabel;
    set
    {
      this.m_strTotalsLabel = value;
      if (this.Workbook.Loading)
        return;
      this.TotalCell.Value = value;
    }
  }

  internal IRange TotalCell
  {
    get
    {
      IRange location = this.m_parentTable.Location;
      return location.Worksheet[location.LastRow, location.Column + this.Index - 1];
    }
  }

  internal IRange HeaderCell
  {
    get
    {
      IRange location = this.m_parentTable.Location;
      return location.Worksheet[location.Row, location.Column + this.Index - 1];
    }
  }

  public int Id
  {
    get => this.m_iId;
    set => this.m_iId = value;
  }

  public string CalculatedFormula
  {
    get
    {
      string calculatedFormula = this.Workbook.Saving ? new FormulaUtil(this.Workbook.Application, (object) this.Workbook, NumberFormatInfo.InvariantInfo, ',', ';').ParsePtgArray(this.m_calculatedFormulaPtgs, 0, 0, false, true) : this.Workbook.FormulaUtil.ParsePtgArray(this.m_calculatedFormulaPtgs);
      if (this.m_calculatedFormulaPtgs == null || calculatedFormula == null || !this.m_parentTable.TableModified || this.IsColumnNameModified)
        return calculatedFormula;
      foreach (Ptg calculatedFormulaPtg in this.m_calculatedFormulaPtgs)
      {
        if (calculatedFormulaPtg.TokenCode == FormulaToken.tName1 || calculatedFormulaPtg.TokenCode == FormulaToken.tName2 || calculatedFormulaPtg.TokenCode == FormulaToken.tName3 || calculatedFormulaPtg.TokenCode == FormulaToken.tNameX1 || calculatedFormulaPtg.TokenCode == FormulaToken.tNameX2 || calculatedFormulaPtg.TokenCode == FormulaToken.tNameX3)
        {
          NameImpl nameImpl = this.Workbook.InnerNamesColection[(calculatedFormulaPtg as NamePtg).ExternNameIndexInt - 1] as NameImpl;
          int num1 = nameImpl.m_isFormulaNamedRange ? 1 : 0;
          int num2 = nameImpl.m_isTableNamedRange ? 1 : 0;
          foreach (Capture match in Regex.Matches(nameImpl.Name, "\\[.*?\\]", RegexOptions.Compiled))
          {
            string str1 = Regex.Replace(match.Value, "\\[|\\]|\\@", "");
            switch (str1)
            {
              case "#All":
              case "#Headers":
              case "#Totals":
              case "#Data":
              case "#This Row":
                continue;
              default:
                string str2 = $"{this.m_parentTable.Name}[{str1}]";
                if (!this.Workbook.InnerNamesColection.m_hashNameToIName.ContainsKey(str2) || this.Workbook.InnerNamesColection[str2] != null && !(this.Workbook.InnerNamesColection[str2] as NameImpl).m_isTableNamedRange && (this.Workbook.InnerNamesColection[str2] as NameImpl).m_isTableNamedRangeDeleted)
                {
                  calculatedFormula = calculatedFormula.Replace(nameImpl.Name, "#REF!");
                  calculatedFormula = calculatedFormula[0] == '=' ? calculatedFormula.Remove(0, 1) : calculatedFormula;
                  continue;
                }
                continue;
            }
          }
        }
      }
      return calculatedFormula;
    }
    set
    {
      if (value[0] == '=')
        value = value.Substring(1);
      this.IsArrayFormula = false;
      if (this.Workbook.IsLoaded && this.m_calculatedFormulaPtgs == null && !this.Workbook.m_bisCopy)
      {
        this.m_calculatedFormulaPtgs = this.Workbook.FormulaUtil.ParseString(value);
        if (this.Workbook.Loading)
          return;
        string name = string.Format("R{0}C{2}:R{1}C{2}", (object) (this.m_parentTable.Location.Columns[this.Index - 1].Row + 1), (object) (this.m_parentTable.Location.Columns[this.Index - 1].LastRow - this.m_parentTable.TotalsRowCount), (object) this.m_parentTable.Location.Columns[this.Index - 1].Column);
        string formula1 = this.m_parentTable.Worksheet.Range[name, true].Formula;
        if (formula1 != null && !(formula1 != value))
          return;
        if (this.Workbook.FormulaUtil.HasCellReference(value))
          this.Workbook.Application.EnableIncrementalFormula = true;
        this.m_parentTable.Worksheet.Range[name, true].Formula = value;
        string formula2 = this.m_parentTable.Worksheet.Range[name, true].Formula;
        if (formula2 == null && this.Workbook.Application.EnableIncrementalFormula)
        {
          string formula3 = this.m_parentTable.Worksheet.Range[string.Format("R{0}C{2}:R{1}C{2}", (object) (this.m_parentTable.Location.Columns[this.Index - 1].Row + 1), (object) (this.m_parentTable.Location.Columns[this.Index - 1].Row + 1), (object) this.m_parentTable.Location.Columns[this.Index - 1].Column), true].Formula;
          this.m_calculatedFormulaPtgs = this.Workbook.FormulaUtil.ParseString(formula3[0] == '=' ? formula3.Remove(0, 1) : formula3);
        }
        else
          this.m_calculatedFormulaPtgs = this.Workbook.FormulaUtil.ParseString(formula2[0] == '=' ? formula2.Remove(0, 1) : formula2);
      }
      else
      {
        if (this.m_calculatedFormulaPtgs != null && this.m_IsColumnNameModified && this.m_calculatedFormulaPtgs == this.Workbook.FormulaUtil.ParseString(value))
          return;
        this.m_calculatedFormulaPtgs = this.Workbook.FormulaUtil.ParseString(value);
        string name = string.Format("R{0}C{2}:R{1}C{2}", (object) (this.m_parentTable.Location.Columns[this.Index - 1].Row + 1), (object) (this.m_parentTable.Location.Columns[this.Index - 1].LastRow - this.m_parentTable.TotalsRowCount), (object) this.m_parentTable.Location.Columns[this.Index - 1].Column);
        if (this.Workbook.FormulaUtil.HasCellReference(value))
          this.Workbook.Application.EnableIncrementalFormula = true;
        this.m_parentTable.Worksheet.Range[name, true].Formula = value;
        string formula4 = this.m_parentTable.Worksheet.Range[name, true].Formula;
        if (formula4 == null && this.Workbook.Application.EnableIncrementalFormula)
        {
          string formula5 = this.m_parentTable.Worksheet.Range[string.Format("R{0}C{2}:R{1}C{2}", (object) (this.m_parentTable.Location.Columns[this.Index - 1].Row + 1), (object) (this.m_parentTable.Location.Columns[this.Index - 1].Row + 1), (object) this.m_parentTable.Location.Columns[this.Index - 1].Column), true].Formula;
          this.m_calculatedFormulaPtgs = this.Workbook.FormulaUtil.ParseString(formula5[0] == '=' ? formula5.Remove(0, 1) : formula5);
        }
        else
          this.m_calculatedFormulaPtgs = this.Workbook.FormulaUtil.ParseString(formula4[0] == '=' ? formula4.Remove(0, 1) : formula4);
      }
    }
  }

  internal Ptg[] CalculatedFormulaPtgs
  {
    get => this.m_calculatedFormulaPtgs;
    set => this.m_calculatedFormulaPtgs = value;
  }

  private WorkbookImpl Workbook => this.m_parentTable.Worksheet.Workbook as WorkbookImpl;

  public int QueryTableFieldId
  {
    get => this.m_queryTableFieldId;
    set => this.m_queryTableFieldId = value;
  }

  internal int MapId
  {
    get => this.m_mapId;
    set => this.m_mapId = value;
  }

  internal string XPath
  {
    get => this.m_xPath;
    set => this.m_xPath = value;
  }

  internal string XmlDataType
  {
    get => this.m_dataType;
    set => this.m_dataType = value;
  }

  internal bool IsArrayFormula
  {
    get => this.m_IsArrayFormula;
    set => this.m_IsArrayFormula = value;
  }

  public ListObjectColumn(string name, int index, ListObject parentTable, int id)
  {
    if (parentTable == null)
      throw new ArgumentNullException(nameof (parentTable));
    this.m_strName = name;
    this.m_iIndex = index;
    this.m_parentTable = parentTable;
    this.m_iId = id;
  }

  private int GetColumnIndex(IRange range) => range.Column + this.Index - 1;

  internal void SetName(string value)
  {
    IRange location = this.m_parentTable.Location;
    int row = location.Row;
    int columnIndex = this.GetColumnIndex(location);
    WorksheetImpl worksheet = this.m_parentTable.Location.Worksheet as WorksheetImpl;
    WorkbookImpl workbook = worksheet.Workbook as WorkbookImpl;
    if (!worksheet.IsParsing && workbook.InnerNamesColection != null)
    {
      for (int index = 0; index < workbook.InnerNamesColection.Count; ++index)
      {
        IName nameByIndex = workbook.InnerNamesColection.GetNameByIndex(index);
        if (nameByIndex.Name.Contains(this.m_parentTable.Name + "[") && nameByIndex.Name.Contains($"[{this.m_strName}]") && !nameByIndex.Name.Contains("[#Headers]") && !nameByIndex.Name.Contains("[#Data]") && !nameByIndex.Name.Contains("[#All]") && !nameByIndex.Name.Contains("[#Totals]"))
        {
          workbook.InnerNamesColection.m_hashNameToIName.Remove($"{this.m_parentTable.Name}[{value}]");
          nameByIndex.Name = nameByIndex.Name.Replace(this.m_strName, value);
        }
        else if (nameByIndex.Name.Contains(this.m_parentTable.Name + "[") && nameByIndex.Name.Contains($"[{this.m_strName}]") && nameByIndex.Name.Contains("[#Headers]"))
        {
          workbook.InnerNamesColection.m_hashNameToIName.Remove($"{this.m_parentTable.Name}[[#Headers][{value}]]");
          nameByIndex.Name = nameByIndex.Name.Replace(this.m_strName, value);
        }
        else if (nameByIndex.Name.Contains(this.m_parentTable.Name + "[") && nameByIndex.Name.Contains($"[{this.m_strName}]") && nameByIndex.Name.Contains("[#Data]"))
        {
          workbook.InnerNamesColection.m_hashNameToIName.Remove($"{this.m_parentTable.Name}[[#Data][{value}]]");
          nameByIndex.Name = nameByIndex.Name.Replace(this.m_strName, value);
        }
        else if (nameByIndex.Name.Contains(this.m_parentTable.Name + "[") && nameByIndex.Name.Contains($"[{this.m_strName}]") && nameByIndex.Name.Contains("[#All]"))
        {
          workbook.InnerNamesColection.m_hashNameToIName.Remove($"{this.m_parentTable.Name}[[#All][{value}]]");
          nameByIndex.Name = nameByIndex.Name.Replace(this.m_strName, value);
        }
        else if (nameByIndex.Name.Contains(this.m_parentTable.Name + "[") && nameByIndex.Name.Contains($"[{this.m_strName}]") && nameByIndex.Name.Contains("[#Totals]"))
        {
          workbook.InnerNamesColection.m_hashNameToIName.Remove($"{this.m_parentTable.Name}[[#Totals][{value}]]");
          nameByIndex.Name = nameByIndex.Name.Replace(this.m_strName, value);
        }
      }
    }
    if (this.m_strName != null && this.m_strName != value)
      this.UpdateTableFormulaCells(this.m_strName, value);
    if (this.m_parentTable.ShowHeaderRow && (this.m_parentTable.Worksheet[row, columnIndex].Text == null || !this.m_parentTable.Worksheet[row, columnIndex].Text.Equals(value)))
      this.m_parentTable.Worksheet[row, columnIndex].Text = value;
    this.AddToNamedRange();
    this.m_strName = value;
  }

  internal void SetTotalsCalculation(ExcelTotalsCalculation totalsCalculation)
  {
    this.m_totals = totalsCalculation;
  }

  internal void SetTotalsLabel(string totalsLabel) => this.m_strTotalsLabel = totalsLabel;

  internal void SetCalculatedFormula(string calculatedFormula)
  {
    this.m_calculatedFormulaPtgs = this.Workbook.FormulaUtil.ParseString(calculatedFormula);
  }

  private void UpdateTableFormulaCells(string oldValue, string newValue)
  {
    for (int index = 0; index < this.m_parentTable.Columns.Count; ++index)
    {
      ListObjectColumn column = this.m_parentTable.Columns[index] as ListObjectColumn;
      if (column.CalculatedFormula != null && column.CalculatedFormula.Contains(oldValue))
      {
        column.IsColumnNameModified = true;
        (this.m_parentTable.Columns[index] as ListObjectColumn).SetCalculatedFormula(this.m_parentTable.Columns[index].CalculatedFormula.Replace(oldValue, newValue));
      }
    }
  }

  public ListObjectColumn Clone(ListObject parentTable)
  {
    if (parentTable == null)
      throw new ArgumentNullException(nameof (parentTable));
    ListObjectColumn listObjectColumn = (ListObjectColumn) this.MemberwiseClone();
    listObjectColumn.m_calculatedFormulaPtgs = CloneUtils.ClonePtgArray(this.m_calculatedFormulaPtgs);
    listObjectColumn.m_parentTable = parentTable;
    return listObjectColumn;
  }

  private void AddToNamedRange()
  {
    WorkbookImpl workbook = (this.m_parentTable.Location.Worksheet as WorksheetImpl).Workbook as WorkbookImpl;
    NameImpl nameImpl1 = workbook.InnerNamesColection.Add($"{this.m_parentTable.Name}[{this.m_strName}]") as NameImpl;
    int row1;
    int lastRow1;
    if (this.m_parentTable.ShowHeaderRow)
    {
      row1 = this.m_parentTable.LocalRange.Row + 1;
      lastRow1 = this.m_parentTable.LocalRange.LastRow - this.m_parentTable.TotalsRowCount;
    }
    else
    {
      row1 = this.m_parentTable.LocalRange.Row;
      lastRow1 = this.m_parentTable.LocalRange.LastRow - this.m_parentTable.TotalsRowCount;
    }
    int num1 = this.m_parentTable.LocalRange.Column + this.Index - 1;
    nameImpl1.SetValue(workbook.FormulaUtil.ParseString(this.m_parentTable.Worksheet[row1, num1, lastRow1, num1].AddressGlobal));
    nameImpl1.RefersToRange = this.m_parentTable.Worksheet[row1, num1, lastRow1, num1];
    nameImpl1.SheetIndex = this.m_parentTable.Worksheet.Index;
    nameImpl1.Visible = false;
    nameImpl1.m_isTableNamedRange = true;
    if (this.m_parentTable.ShowHeaderRow)
    {
      string str = "#Headers";
      NameImpl nameImpl2 = workbook.InnerNamesColection.Add($"{this.m_parentTable.Name}[[{str}][{this.m_strName}]]") as NameImpl;
      int row2 = this.m_parentTable.LocalRange.Row;
      int num2 = this.m_parentTable.LocalRange.Column + this.Index - 1;
      int row3 = this.m_parentTable.LocalRange.Row;
      nameImpl2.SetValue(workbook.FormulaUtil.ParseString(this.m_parentTable.Worksheet[row2, num2, row3, num2].AddressGlobal));
      nameImpl2.RefersToRange = this.m_parentTable.Worksheet[row2, num2, row3, num2];
      nameImpl2.SheetIndex = this.m_parentTable.Worksheet.Index;
      nameImpl2.Visible = false;
      nameImpl2.m_isTableNamedRange = true;
    }
    if (this.m_parentTable.ShowTotals)
    {
      string str = "#Totals";
      NameImpl nameImpl3 = workbook.InnerNamesColection.Add($"{this.m_parentTable.Name}[[{str}][{this.m_strName}]]") as NameImpl;
      int lastRow2 = this.m_parentTable.LocalRange.LastRow;
      int num3 = this.m_parentTable.LocalRange.Column + this.Index - 1;
      int lastRow3 = this.m_parentTable.LocalRange.LastRow;
      nameImpl3.SetValue(workbook.FormulaUtil.ParseString(this.m_parentTable.Worksheet[lastRow2, num3, lastRow3, num3].AddressGlobal));
      nameImpl3.RefersToRange = this.m_parentTable.Worksheet[lastRow2, num3, lastRow3, num3];
      nameImpl3.SheetIndex = this.m_parentTable.Worksheet.Index;
      nameImpl3.Visible = false;
      nameImpl3.m_isTableNamedRange = true;
    }
    string str1 = "#Data";
    NameImpl nameImpl4 = workbook.InnerNamesColection.Add($"{this.m_parentTable.Name}[[{str1}][{this.m_strName}]]") as NameImpl;
    int row4;
    int num4;
    int lastRow4;
    if (this.m_parentTable.ShowHeaderRow)
    {
      row4 = this.m_parentTable.LocalRange.Row + 1;
      num4 = this.m_parentTable.LocalRange.Column + this.Index - 1;
      lastRow4 = this.m_parentTable.LocalRange.LastRow - this.m_parentTable.TotalsRowCount;
    }
    else
    {
      row4 = this.m_parentTable.LocalRange.Row;
      num4 = this.m_parentTable.LocalRange.Column + this.Index - 1;
      lastRow4 = this.m_parentTable.LocalRange.LastRow - this.m_parentTable.TotalsRowCount;
    }
    nameImpl4.SetValue(workbook.FormulaUtil.ParseString(this.m_parentTable.Worksheet[row4, num4, lastRow4, num4].AddressGlobal));
    nameImpl4.RefersToRange = this.m_parentTable.Worksheet[row4, num4, lastRow4, num4];
    nameImpl4.SheetIndex = this.m_parentTable.Worksheet.Index;
    nameImpl4.Visible = false;
    nameImpl4.m_isTableNamedRange = true;
    string str2 = "#All";
    NameImpl nameImpl5 = workbook.InnerNamesColection.Add($"{this.m_parentTable.Name}[[{str2}][{this.m_strName}]]") as NameImpl;
    int row5;
    int lastRow5;
    if (this.m_parentTable.ShowHeaderRow)
    {
      row5 = this.m_parentTable.LocalRange.Row + 1;
      lastRow5 = this.m_parentTable.LocalRange.LastRow - this.m_parentTable.TotalsRowCount;
    }
    else
    {
      row5 = this.m_parentTable.LocalRange.Row;
      lastRow5 = this.m_parentTable.LocalRange.LastRow - this.m_parentTable.TotalsRowCount;
    }
    int num5 = this.m_parentTable.LocalRange.Column + this.Index - 1;
    nameImpl5.SetValue(workbook.FormulaUtil.ParseString(this.m_parentTable.Worksheet[row5, num5, lastRow5, num5].AddressGlobal));
    nameImpl5.RefersToRange = this.m_parentTable.Worksheet[row5, num5, lastRow5, num5];
    nameImpl5.SheetIndex = this.m_parentTable.Worksheet.Index;
    nameImpl5.Visible = false;
    nameImpl5.m_isTableNamedRange = true;
  }
}
