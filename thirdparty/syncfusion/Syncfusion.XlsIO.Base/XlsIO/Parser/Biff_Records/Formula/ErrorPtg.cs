// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Formula.ErrorPtg
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Formula;

[Token(FormulaToken.tError)]
[Syncfusion.XlsIO.Parser.Biff_Records.Formula.ErrorCode("#VALUE!", 15)]
[Syncfusion.XlsIO.Parser.Biff_Records.Formula.ErrorCode("#N/A", 42)]
[Preserve(AllMembers = true)]
[Syncfusion.XlsIO.Parser.Biff_Records.Formula.ErrorCode("#NULL!", 0)]
[Syncfusion.XlsIO.Parser.Biff_Records.Formula.ErrorCode("#DIV/0!", 7)]
[Syncfusion.XlsIO.Parser.Biff_Records.Formula.ErrorCode("#NAME?", 29)]
[Syncfusion.XlsIO.Parser.Biff_Records.Formula.ErrorCode("#NUM!", 36)]
public class ErrorPtg : Ptg
{
  public const string DEF_ERROR_NAME = "#N/A";
  public static Dictionary<string, int> ErrorNameToCode = new Dictionary<string, int>(6);
  public static Dictionary<int, string> ErrorCodeToName = new Dictionary<int, string>(6);
  private byte m_errorCode;

  [Preserve]
  static ErrorPtg()
  {
    ErrorCodeAttribute[] errorCodeAttributeArray = new ErrorCodeAttribute[6]
    {
      new ErrorCodeAttribute("#NULL!", 0),
      new ErrorCodeAttribute("#DIV/0!", 7),
      new ErrorCodeAttribute("#VALUE!", 15),
      new ErrorCodeAttribute("#NAME?", 29),
      new ErrorCodeAttribute("#NUM!", 36),
      new ErrorCodeAttribute("#N/A", 42)
    };
    int index = 0;
    for (int length = errorCodeAttributeArray.Length; index < length; ++index)
    {
      ErrorCodeAttribute errorCodeAttribute = errorCodeAttributeArray[index];
      ErrorPtg.ErrorNameToCode.Add(errorCodeAttribute.StringValue, errorCodeAttribute.ErrorCode);
      ErrorPtg.ErrorCodeToName.Add(errorCodeAttribute.ErrorCode, errorCodeAttribute.StringValue);
    }
  }

  [Preserve]
  public ErrorPtg()
  {
  }

  [Preserve]
  public ErrorPtg(DataProvider provider, int offset, ExcelVersion version)
    : base(provider, offset, version)
  {
  }

  [Preserve]
  public ErrorPtg(int errorCode)
  {
    this.TokenCode = FormulaToken.tError;
    this.m_errorCode = (byte) errorCode;
  }

  [Preserve]
  public ErrorPtg(string errorName)
    : this(ErrorPtg.ErrorNameToCode[errorName])
  {
  }

  public byte ErrorCode
  {
    get => this.m_errorCode;
    set => this.m_errorCode = value;
  }

  public override int GetSize(ExcelVersion version) => 2;

  public override string ToString(
    FormulaUtil formulaUtil,
    int iRow,
    int iColumn,
    bool bR1C1,
    NumberFormatInfo numberFormat,
    bool isForSerialization)
  {
    string str;
    return !ErrorPtg.ErrorCodeToName.TryGetValue((int) this.m_errorCode, out str) ? "#N/A" : str;
  }

  public override byte[] ToByteArray(ExcelVersion version)
  {
    byte[] byteArray = base.ToByteArray(version);
    byteArray[1] = this.m_errorCode;
    return byteArray;
  }

  public override void InfillPTG(DataProvider provider, ref int offset, ExcelVersion version)
  {
    this.m_errorCode = provider.ReadByte(offset++);
  }
}
