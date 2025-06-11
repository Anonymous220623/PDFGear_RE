// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Formula.StringConstantPtg
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.Globalization;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Formula;

[Token(FormulaToken.tStringConstant)]
[Preserve(AllMembers = true)]
public class StringConstantPtg : Ptg
{
  public string m_strValue = string.Empty;
  public byte m_compressed = 1;

  [Preserve]
  public StringConstantPtg()
  {
  }

  [Preserve]
  public StringConstantPtg(string value)
  {
    this.TokenCode = FormulaToken.tStringConstant;
    this.Value = value;
  }

  [Preserve]
  public StringConstantPtg(DataProvider provider, int offset, ExcelVersion version)
    : base(provider, offset, version)
  {
  }

  public string Value
  {
    get => this.m_strValue;
    set
    {
      this.m_strValue = value;
      this.m_compressed = (byte) 1;
    }
  }

  public override int GetSize(ExcelVersion version)
  {
    return this.m_compressed != (byte) 1 ? this.m_strValue.Length + 3 : this.m_strValue.Length * 2 + 3;
  }

  public override string ToString(
    FormulaUtil formulaUtil,
    int iRow,
    int iColumn,
    bool bR1C1,
    NumberFormatInfo numberFormat,
    bool isForSerialization)
  {
    string str = this.m_strValue;
    if (str != null)
      str = str.Replace("\"", "\"\"");
    return $"\"{str}\"";
  }

  public override byte[] ToByteArray(ExcelVersion version)
  {
    byte[] numArray = this.m_compressed != (byte) 1 ? BiffRecordRaw.LatinEncoding.GetBytes(this.m_strValue) : Encoding.Unicode.GetBytes(this.m_strValue);
    byte[] byteArray = new byte[numArray.Length + 3];
    byteArray[0] = (byte) this.TokenCode;
    byteArray[1] = (byte) this.m_strValue.Length;
    byteArray[2] = this.m_compressed;
    numArray.CopyTo((Array) byteArray, 3);
    return byteArray;
  }

  public override void InfillPTG(DataProvider provider, ref int offset, ExcelVersion version)
  {
    this.m_compressed = provider.ReadByte(offset + 1);
    int iFullLength;
    this.m_strValue = provider.ReadString8Bit(offset, out iFullLength);
    this.m_strValue = this.m_strValue.Replace("\0", ",");
    offset += iFullLength;
  }
}
