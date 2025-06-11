// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Formula.Area3DPtg
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using Syncfusion.OfficeChart.Implementation.Exceptions;
using System;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Formula;

[Token(FormulaToken.tArea3d2)]
[CLSCompliant(false)]
[Preserve(AllMembers = true)]
[Token(FormulaToken.tArea3d1)]
[Token(FormulaToken.tArea3d3)]
internal class Area3DPtg : AreaPtg, ISheetReference, IReference, IRangeGetter
{
  private ushort m_usRefIndex;

  [Preserve]
  public Area3DPtg()
  {
  }

  [Preserve]
  public Area3DPtg(DataProvider provider, int offset, OfficeVersion version)
    : base(provider, offset, version)
  {
  }

  [Preserve]
  public Area3DPtg(string strFormula, IWorkbook parent)
  {
    Match m = FormulaUtil.CellRange3DRegex.Match(strFormula);
    if (!m.Success || !(m.Value == strFormula))
    {
      m = FormulaUtil.CellRange3DRegex2.Match(strFormula);
      if (!m.Success || !(m.Value == strFormula) || !(m.Groups["SheetName"].Value == m.Groups["SheetName2"].Value))
        throw new ArgumentException("Not valid area 3D string.");
    }
    this.SetValues(m, parent);
  }

  [Preserve]
  public Area3DPtg(Area3DPtg ptg)
    : base((AreaPtg) ptg)
  {
    this.m_usRefIndex = ptg.m_usRefIndex;
  }

  [Preserve]
  public Area3DPtg(
    int iSheetIndex,
    int iFirstRow,
    int iFirstCol,
    int iLastRow,
    int iLastCol,
    byte firstOptions,
    byte lastOptions)
    : base(iFirstRow, iFirstCol, iLastRow, iLastCol, firstOptions, lastOptions)
  {
    this.m_usRefIndex = (ushort) iSheetIndex;
  }

  [Preserve]
  public Area3DPtg(
    int iCellRow,
    int iCellColumn,
    int iRefIndex,
    string strFirstRow,
    string strFirstColumn,
    string strLastRow,
    string strLastColumn,
    bool bR1C1,
    IWorkbook book)
    : base(iCellRow, iCellColumn, strFirstRow, strFirstColumn, strLastRow, strLastColumn, bR1C1, book)
  {
    this.m_usRefIndex = (ushort) iRefIndex;
  }

  public ushort RefIndex
  {
    get => this.m_usRefIndex;
    set => this.m_usRefIndex = value;
  }

  public override int GetSize(OfficeVersion version) => base.GetSize(version) + 2;

  public override byte[] ToByteArray(OfficeVersion version)
  {
    byte[] byteArray = base.ToByteArray(version);
    Buffer.BlockCopy((Array) byteArray, 1, (Array) byteArray, 3, byteArray.Length - 3);
    BitConverter.GetBytes(this.m_usRefIndex).CopyTo((Array) byteArray, 1);
    return byteArray;
  }

  public override string ToString(
    FormulaUtil formulaUtil,
    int iRow,
    int iColumn,
    bool bR1C1,
    NumberFormatInfo numberFormat,
    bool isForSerialization)
  {
    string str1 = base.ToString(formulaUtil, iRow, iColumn, bR1C1, numberFormat, isForSerialization);
    string str2;
    if (formulaUtil == null)
    {
      str2 = $"[ReferenceIndex = {(object) this.m_usRefIndex} ] ";
    }
    else
    {
      string sheetNameByReference = ((WorkbookImpl) formulaUtil.ParentWorkbook).GetSheetNameByReference((int) this.m_usRefIndex, false);
      if (sheetNameByReference != null)
      {
        string str3 = sheetNameByReference.Replace("'", "''");
        str2 = !Area3DPtg.ValidateSheetName(str3) ? str3 + "!" : $"'{str3}'!";
      }
      else
        str2 = string.Empty;
    }
    return str2 + str1;
  }

  public static bool ValidateSheetName(string value)
  {
    char[] chArray = new char[26]
    {
      '!',
      '@',
      '#',
      '$',
      '%',
      '^',
      '&',
      '(',
      ')',
      '-',
      '\'',
      ';',
      ' ',
      '"',
      '[',
      ']',
      '~',
      '{',
      '}',
      '+',
      '|',
      ',',
      '=',
      '<',
      '>',
      '`'
    };
    int[] numArray = new int[3]{ 88, 70, 68 };
    int num = value.IndexOfAny("123456789".ToCharArray(), 0);
    char ch1 = 'R';
    char ch2 = 'C';
    value = value.ToUpper();
    char[] charArray = value.ToCharArray();
    for (int index = 0; index < chArray.Length - 1; ++index)
    {
      if (value.Contains(chArray[index].ToString()))
        return true;
    }
    if (char.IsDigit(charArray[0]) || charArray.Length == 1 && ((int) charArray[0] == (int) ch2 || (int) charArray[0] == (int) ch1) || ((int) charArray[0] == (int) ch2 || (int) charArray[0] == (int) ch1) && charArray[1] != '0' && char.IsDigit(charArray[1]))
      return true;
    if (num >= 4 || num == -1)
      return false;
    for (int index = num; index < charArray.Length; ++index)
    {
      if (char.IsLetter(charArray[index]) || index > num + 5)
        return false;
    }
    if (num == 3)
    {
      for (int index = 0; index < num && (int) charArray[index] >= numArray[index]; ++index)
      {
        if ((int) charArray[index] != numArray[index])
          return false;
      }
    }
    return true;
  }

  public override int CodeToIndex() => Area3DPtg.CodeToIndex(this.TokenCode);

  public override FormulaToken GetCorrespondingErrorCode()
  {
    return AreaError3DPtg.IndexToCode(this.CodeToIndex());
  }

  public override Ptg Offset(
    int iCurSheetIndex,
    int iTokenRow,
    int iTokenColumn,
    int iSourceSheetIndex,
    Rectangle rectSource,
    int iDestSheetIndex,
    Rectangle rectDest,
    out bool bChanged,
    WorkbookImpl book)
  {
    bChanged = false;
    if ((int) this.m_usRefIndex != (int) (ushort) iSourceSheetIndex)
      return (Ptg) this.Clone();
    Area3DPtg area3Dptg = (Area3DPtg) base.Offset(iDestSheetIndex, iTokenRow, iTokenColumn, iDestSheetIndex, rectSource, iDestSheetIndex, rectDest, out bChanged, book);
    if (bChanged)
      area3Dptg.m_usRefIndex = (ushort) iDestSheetIndex;
    return (Ptg) area3Dptg;
  }

  public override AreaPtg ConvertToErrorPtg() => (AreaPtg) new AreaError3DPtg(this);

  public string BaseToString(FormulaUtil formulaUtil, int iRow, int iColumn, bool bR1C1)
  {
    return this.ToString(formulaUtil, iRow, iColumn, bR1C1);
  }

  protected void SetSheetIndex(string sheetName, IWorkbook parent)
  {
    WorkbookImpl workbookImpl = (WorkbookImpl) parent;
    if (sheetName[0] == '\'')
    {
      if (sheetName[sheetName.Length - 1] == '\'')
        sheetName = sheetName.Substring(1, sheetName.Length - 2);
    }
    try
    {
      this.m_usRefIndex = (ushort) workbookImpl.AddSheetReference(sheetName);
    }
    catch (ArgumentException ex)
    {
      throw new ParseException();
    }
  }

  protected void SetValues(Match m, IWorkbook parent)
  {
    string sheetName = m.Groups["SheetName"].Value;
    string column1 = m.Groups["Column1"].Value;
    string row1 = m.Groups["Row1"].Value;
    string column2 = m.Groups["Column2"].Value;
    string row2 = m.Groups["Row2"].Value;
    this.SetArea(0, 0, row1, column1, row2, column2, false, parent);
    this.SetSheetIndex(sheetName, parent);
  }

  public override void InfillPTG(DataProvider provider, ref int offset, OfficeVersion version)
  {
    this.m_usRefIndex = provider.ReadUInt16(offset);
    offset += 2;
    base.InfillPTG(provider, ref offset, version);
  }

  public new static FormulaToken IndexToCode(int index)
  {
    switch (index)
    {
      case 1:
        return FormulaToken.tArea3d1;
      case 2:
        return FormulaToken.tArea3d2;
      case 3:
        return FormulaToken.tArea3d3;
      default:
        throw new ArgumentOutOfRangeException(nameof (index), "Must be less than 4 and greater than than 0.");
    }
  }

  public new static int CodeToIndex(FormulaToken code)
  {
    switch (code)
    {
      case FormulaToken.tArea3d1:
        return 1;
      case FormulaToken.tArea3d2:
        return 2;
      case FormulaToken.tArea3d3:
        return 3;
      default:
        throw new ArgumentOutOfRangeException("index");
    }
  }

  public new IRange GetRange(IWorkbook book, IWorksheet sheet)
  {
    WorkbookImpl workbookImpl = book != null ? (WorkbookImpl) book : throw new ArgumentNullException(nameof (book));
    IRange range;
    if (!workbookImpl.IsExternalReference((int) this.m_usRefIndex))
    {
      sheet = workbookImpl.GetSheetByReference((int) this.m_usRefIndex, false);
      range = sheet == null ? workbookImpl.Worksheets[0][this.FirstRow + 1, this.FirstColumn + 1, this.LastRow + 1, this.LastColumn + 1] : sheet[this.FirstRow + 1, this.FirstColumn + 1, this.LastRow + 1, this.LastColumn + 1];
    }
    else
      range = (IRange) new ExternalRange(workbookImpl.GetExternSheet((int) this.m_usRefIndex), this.FirstRow + 1, this.FirstColumn + 1, this.LastRow + 1, this.LastColumn + 1);
    return range;
  }
}
