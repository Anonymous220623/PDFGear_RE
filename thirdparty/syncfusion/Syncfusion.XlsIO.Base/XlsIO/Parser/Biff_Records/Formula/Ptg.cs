// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Formula.Ptg
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.Drawing;
using System.Globalization;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Formula;

[Preserve(AllMembers = true)]
public abstract class Ptg : ICloneable
{
  private const int DEF_PTG_INDEX_DELTA = 32 /*0x20*/;
  private FormulaToken m_Code;

  [Preserve]
  protected Ptg()
  {
  }

  [Preserve]
  protected Ptg(DataProvider provider, int offset, ExcelVersion version)
  {
    this.m_Code = (FormulaToken) provider.ReadByte(offset);
    ++offset;
    this.InfillPTG(provider, ref offset, version);
  }

  public virtual bool IsOperation => false;

  public virtual FormulaToken TokenCode
  {
    get => this.m_Code;
    set => this.m_Code = value;
  }

  public static string GetString16Bit(byte[] data, int offset)
  {
    return Ptg.GetString16Bit(data, offset, out int _);
  }

  public static string GetString16Bit(byte[] data, int offset, out int iFullLength)
  {
    if (offset + 3 >= data.Length)
      throw new ArgumentOutOfRangeException("GetString16Bit: data array too small.");
    ushort uint16 = BitConverter.ToUInt16(data, offset);
    offset += 2;
    bool boolean = BitConverter.ToBoolean(data, offset);
    ++offset;
    iFullLength = boolean ? 3 + (int) uint16 * 2 : 3 + (int) uint16;
    if (iFullLength >= data.Length)
      throw new ArgumentOutOfRangeException("GetString16Bit: data array too small.");
    return !boolean ? BiffRecordRaw.LatinEncoding.GetString(data, offset, (int) uint16) : Encoding.Unicode.GetString(data, offset, (int) uint16 * 2);
  }

  public virtual void InfillPTG(DataProvider provider, ref int offset, ExcelVersion version)
  {
  }

  public abstract int GetSize(ExcelVersion version);

  public virtual byte[] ToByteArray(ExcelVersion version)
  {
    byte[] byteArray = new byte[this.GetSize(version)];
    byteArray[0] = (byte) this.TokenCode;
    return byteArray;
  }

  public override string ToString() => this.ToString((FormulaUtil) null, 0, 0, false);

  public virtual string ToString(FormulaUtil formulaUtil)
  {
    return this.ToString(formulaUtil, 0, 0, false);
  }

  public virtual string ToString(FormulaUtil formulaUtil, int iRow, int iColumn, bool bR1C1)
  {
    return this.ToString(formulaUtil, iRow, iColumn, bR1C1, (NumberFormatInfo) null, false);
  }

  public virtual string ToString(int row, int col, bool bR1C1)
  {
    return this.ToString((FormulaUtil) null, row, col, bR1C1);
  }

  public virtual string ToString(
    FormulaUtil formulaUtil,
    int row,
    int col,
    bool bR1C1,
    NumberFormatInfo numberFormat)
  {
    return this.ToString(formulaUtil, row, col, bR1C1, numberFormat, false);
  }

  public virtual string ToString(
    FormulaUtil formulaUtil,
    int row,
    int col,
    bool bR1C1,
    NumberFormatInfo numberFormat,
    bool isForSerialization)
  {
    return base.ToString();
  }

  public virtual Ptg Offset(int iRowOffset, int iColumnOffset, WorkbookImpl book)
  {
    return (Ptg) this.Clone();
  }

  public virtual Ptg Offset(
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
    return (Ptg) this.Clone();
  }

  public static bool RectangleContains(Rectangle rect, int iRow, int iColumn)
  {
    return rect.Left <= iColumn && rect.Right >= iColumn && rect.Top <= iRow && rect.Bottom >= iRow;
  }

  internal static bool RectangleIntersects(
    Rectangle rect,
    int iFirstRow,
    int iFirstColumn,
    int iLastRow,
    int iLastColumn)
  {
    return rect.Left <= iLastColumn && rect.Right >= iFirstColumn && rect.Top <= iLastRow && rect.Bottom >= iFirstRow;
  }

  public virtual Ptg ConvertSharedToken(IWorkbook parent, int iRow, int iColumn)
  {
    return (Ptg) this.Clone();
  }

  public virtual Ptg ConvertPtgToNPtg(IWorkbook parent, int iRow, int iColumn) => this;

  public int CompareTo(Ptg token)
  {
    if (token == null)
      return 1;
    int num = this.TokenCode - token.TokenCode;
    if (num == 0)
      num = this.GetSize(ExcelVersion.Excel2007) - token.GetSize(ExcelVersion.Excel2007);
    if (num == 0)
      num = this.CompareContent(token);
    return num;
  }

  protected int CompareContent(Ptg token)
  {
    if (token == null)
      throw new ArgumentNullException(nameof (token));
    return !BiffRecordRaw.CompareArrays(this.ToByteArray(ExcelVersion.Excel2007), token.ToByteArray(ExcelVersion.Excel2007)) ? 1 : 0;
  }

  public static bool CompareArrays(Ptg[] arrTokens1, Ptg[] arrTokens2)
  {
    if (arrTokens1 == null && arrTokens2 == null)
      return true;
    if (arrTokens1 == null || arrTokens2 == null)
      return false;
    int length = arrTokens1.Length;
    if (length != arrTokens2.Length)
      return false;
    for (int index = 0; index < length; ++index)
    {
      if (arrTokens1[index].CompareTo(arrTokens2[index]) != 0)
        return false;
    }
    return true;
  }

  public virtual string ToString(
    FormulaUtil formulaUtil,
    int row,
    int col,
    bool bR1C1,
    NumberFormatInfo numberInfo,
    bool isForSerialization,
    IWorksheet sheet)
  {
    return this.ToString(formulaUtil, row, col, bR1C1, numberInfo, isForSerialization);
  }

  public object Clone() => this.MemberwiseClone();

  public static FormulaToken IndexToCode(FormulaToken baseToken, int index)
  {
    if (index < 1 || index > 3)
      throw new ArgumentOutOfRangeException(nameof (index), "Value cannot be less than 1 or greater than 3.");
    return baseToken + (index - 1) * 32 /*0x20*/;
  }
}
