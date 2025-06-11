// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Formula.StringConstantPtg
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using System;
using System.Globalization;
using System.Text;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Formula;

[Token(FormulaToken.tStringConstant)]
[Preserve(AllMembers = true)]
internal class StringConstantPtg : Ptg
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
  public StringConstantPtg(DataProvider provider, int offset, OfficeVersion version)
    : base(provider, offset, version)
  {
  }

  public string Value
  {
    get => this.m_strValue;
    set
    {
      this.m_strValue = value.Length <= (int) byte.MaxValue ? value : throw new ArgumentOutOfRangeException(nameof (value), "String is too long.");
      this.m_compressed = (byte) 1;
    }
  }

  public override int GetSize(OfficeVersion version)
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

  public override byte[] ToByteArray(OfficeVersion version)
  {
    byte[] numArray = this.m_compressed != (byte) 1 ? BiffRecordRaw.LatinEncoding.GetBytes(this.m_strValue) : Encoding.Unicode.GetBytes(this.m_strValue);
    byte[] byteArray = new byte[numArray.Length + 3];
    byteArray[0] = (byte) this.TokenCode;
    byteArray[1] = (byte) this.m_strValue.Length;
    byteArray[2] = this.m_compressed;
    numArray.CopyTo((Array) byteArray, 3);
    return byteArray;
  }

  public override void InfillPTG(DataProvider provider, ref int offset, OfficeVersion version)
  {
    this.m_compressed = provider.ReadByte(offset + 1);
    int iFullLength;
    this.m_strValue = provider.ReadString8Bit(offset, out iFullLength);
    offset += iFullLength;
  }
}
