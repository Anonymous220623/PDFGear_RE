// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Formula.ArrayPtg
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Formula;

[Token(FormulaToken.tArray3)]
[Preserve(AllMembers = true)]
[Token(FormulaToken.tArray1)]
[Token(FormulaToken.tArray2)]
internal class ArrayPtg : Ptg, IAdditionalData, ICloneable
{
  public const byte DOUBLEVALUE = 1;
  public const byte STRINGVALUE = 2;
  public const byte BOOLEANVALUE = 4;
  public const byte ERRORCODEVALUE = 16 /*0x10*/;
  public const byte NilValue = 0;
  public static readonly string RowSeparator = ";";
  public static readonly string ColSeparator = ",";
  private byte m_ColumnNumber;
  private ushort m_usRowNumber;
  private object[,] m_arrCachedValue;

  [Preserve]
  public ArrayPtg()
  {
  }

  [Preserve]
  public ArrayPtg(string strFormula, FormulaUtil formulaParser)
  {
    strFormula = strFormula.Substring(1, strFormula.Length - 2);
    List<string> stringList = formulaParser.SplitArray(strFormula, formulaParser.ArrayRowSeparator);
    List<string>[] arrValues = new List<string>[stringList.Count];
    int index = 0;
    for (int count = stringList.Count; index < count; ++index)
    {
      string strFormula1 = stringList[index];
      arrValues[index] = formulaParser.SplitArray(strFormula1, formulaParser.OperandsSeparator);
      if (index > 0 && arrValues[index].Count != arrValues[index - 1].Count)
        throw new ArgumentException("Each row in the tArray must have the same column number.");
    }
    this.FillList(arrValues, formulaParser);
    this.SetReferenceIndex(2);
  }

  [Preserve]
  public ArrayPtg(DataProvider provider, int offset, OfficeVersion version)
    : base(provider, offset, version)
  {
  }

  public int AdditionalDataSize
  {
    get
    {
      int additionalDataSize = 0;
      if (this.m_arrCachedValue != null)
      {
        additionalDataSize += 3;
        object[,] arrCachedValue = this.m_arrCachedValue;
        int upperBound1 = arrCachedValue.GetUpperBound(0);
        int upperBound2 = arrCachedValue.GetUpperBound(1);
        for (int lowerBound1 = arrCachedValue.GetLowerBound(0); lowerBound1 <= upperBound1; ++lowerBound1)
        {
          for (int lowerBound2 = arrCachedValue.GetLowerBound(1); lowerBound2 <= upperBound2; ++lowerBound2)
          {
            object s = arrCachedValue[lowerBound1, lowerBound2];
            switch (s)
            {
              case double _:
              case bool _:
              case byte _:
              case null:
                additionalDataSize += 9;
                break;
              case string _:
                additionalDataSize += Encoding.Unicode.GetByteCount((string) s) + 4;
                break;
              default:
                throw new ArrayTypeMismatchException("Unexpected type in tArray.");
            }
          }
        }
      }
      return additionalDataSize;
    }
  }

  public int ReadArray(DataProvider provider, int offset)
  {
    this.m_ColumnNumber = provider.ReadByte(offset);
    this.m_usRowNumber = (ushort) provider.ReadInt16(offset + 1);
    return this.FillList(provider, offset + 3, (int) this.m_ColumnNumber + 1, (int) this.m_usRowNumber + 1);
  }

  private int FillList(DataProvider provider, int offset, int ColumnNumber, int RowNumber)
  {
    this.m_arrCachedValue = new object[RowNumber, ColumnNumber];
    for (int index1 = 0; index1 < RowNumber; ++index1)
    {
      for (int index2 = 0; index2 < ColumnNumber; ++index2)
      {
        byte num1 = provider.ReadByte(offset++);
        switch (num1)
        {
          case 0:
            this.m_arrCachedValue[index1, index2] = (object) null;
            offset += 8;
            break;
          case 1:
            double num2 = provider.ReadDouble(offset);
            this.m_arrCachedValue[index1, index2] = (object) num2;
            offset += 8;
            break;
          case 2:
            int iFullLength;
            string str = provider.ReadString16Bit(offset, out iFullLength);
            this.m_arrCachedValue[index1, index2] = (object) str;
            offset += iFullLength;
            break;
          case 4:
            bool flag = provider.ReadBoolean(offset);
            this.m_arrCachedValue[index1, index2] = (object) flag;
            offset += 8;
            break;
          case 16 /*0x10*/:
            this.m_arrCachedValue[index1, index2] = (object) provider.ReadByte(offset);
            offset += 8;
            break;
          default:
            throw new ArgumentOutOfRangeException("Unknown type in tArray: " + (object) num1);
        }
      }
    }
    return offset;
  }

  private int FillList(byte[] data, int offset, int ColumnNumber, int RowNumber)
  {
    this.m_arrCachedValue = new object[RowNumber, ColumnNumber];
    for (int index1 = 0; index1 < RowNumber; ++index1)
    {
      for (int index2 = 0; index2 < ColumnNumber; ++index2)
      {
        byte num1 = data[offset++];
        switch (num1)
        {
          case 1:
            if (offset + 8 > data.Length)
              throw new ArgumentOutOfRangeException("FillList: data array too small.");
            double num2 = BitConverter.ToDouble(data, offset);
            this.m_arrCachedValue[index1, index2] = (object) num2;
            offset += 8;
            break;
          case 2:
            int iFullLength;
            string string16Bit = Ptg.GetString16Bit(data, offset, out iFullLength);
            this.m_arrCachedValue[index1, index2] = (object) string16Bit;
            offset += iFullLength;
            break;
          case 4:
            if (offset + 8 > data.Length)
              throw new ArgumentOutOfRangeException("FillList: data array too small.");
            bool boolean = BitConverter.ToBoolean(data, offset);
            this.m_arrCachedValue[index1, index2] = (object) boolean;
            offset += 8;
            break;
          case 16 /*0x10*/:
            if (offset + 8 > data.Length)
              throw new ArgumentOutOfRangeException("FillList: data array too small.");
            this.m_arrCachedValue[index1, index2] = (object) data[offset];
            offset += 8;
            break;
          default:
            throw new ArgumentOutOfRangeException("Unknown type in tArray: " + (object) num1);
        }
      }
    }
    return offset;
  }

  private void FillList(List<string>[] arrValues, FormulaUtil formulaParser)
  {
    int length = arrValues.Length;
    int count = arrValues[0].Count;
    this.m_ColumnNumber = (byte) (count - 1);
    this.m_usRowNumber = (ushort) (length - 1);
    this.m_arrCachedValue = new object[length, count];
    for (int index1 = 0; index1 < length; ++index1)
    {
      if (arrValues[index1].Count != count)
      {
        this.m_arrCachedValue = (object[,]) null;
        throw new ArgumentException("Each row in the tArray must have same number of columns.");
      }
      for (int index2 = 0; index2 < count; ++index2)
        this.m_arrCachedValue[index1, index2] = this.ParseConstant(arrValues[index1][index2], formulaParser);
    }
  }

  private object ParseConstant(string value, FormulaUtil formulaParser)
  {
    if (value.Length == 0)
      throw new ArgumentException("Constant string can't be empty.");
    double result;
    if (double.TryParse(value, NumberStyles.Any, (IFormatProvider) formulaParser.NumberFormat, out result))
      return (object) result;
    try
    {
      if (!(value.ToLower() == "true"))
      {
        if (!(value.ToLower() == "false"))
          goto label_8;
      }
      return (object) bool.Parse(value.ToLower());
    }
    catch (FormatException ex)
    {
    }
label_8:
    if (value[0] == '"' && '"' == value[value.Length - 1])
      return (object) value.Substring(1, value.Length - 2);
    Ptg[] ptgArray = formulaParser.ParseString(value);
    return ptgArray != null && ptgArray.Length == 1 && ptgArray[0] is ErrorPtg errorPtg ? (object) errorPtg.ErrorCode : (object) null;
  }

  public BytesList GetListBytes()
  {
    BytesList listBytes = new BytesList(false);
    listBytes.Add(this.m_ColumnNumber);
    listBytes.AddRange(BitConverter.GetBytes(this.m_usRowNumber));
    object[,] arrCachedValue = this.m_arrCachedValue;
    int upperBound1 = arrCachedValue.GetUpperBound(0);
    int upperBound2 = arrCachedValue.GetUpperBound(1);
    for (int lowerBound1 = arrCachedValue.GetLowerBound(0); lowerBound1 <= upperBound1; ++lowerBound1)
    {
      for (int lowerBound2 = arrCachedValue.GetLowerBound(1); lowerBound2 <= upperBound2; ++lowerBound2)
      {
        object obj = arrCachedValue[lowerBound1, lowerBound2];
        switch (obj)
        {
          case double num1:
            listBytes.Add((byte) 1);
            listBytes.AddRange(this.GetDoubleBytes(num1));
            break;
          case string _:
            listBytes.Add((byte) 2);
            listBytes.AddRange(this.GetStringBytes((string) obj));
            break;
          case bool flag:
            listBytes.Add((byte) 4);
            listBytes.AddRange(this.GetBoolBytes(flag));
            break;
          case byte num2:
            listBytes.Add((byte) 16 /*0x10*/);
            listBytes.AddRange(this.GetErrorCodeBytes(num2));
            break;
          case null:
            listBytes.Add((byte) 0);
            listBytes.AddRange(this.GetNilBytes());
            break;
          default:
            throw new ArrayTypeMismatchException("Unexpected type in tArray.");
        }
      }
    }
    return listBytes;
  }

  private byte[] GetBoolBytes(bool value)
  {
    byte[] boolBytes = new byte[8];
    if (value)
      boolBytes[0] = (byte) 1;
    return boolBytes;
  }

  private byte[] GetErrorCodeBytes(byte value)
  {
    byte[] errorCodeBytes = new byte[8];
    errorCodeBytes[0] = value;
    return errorCodeBytes;
  }

  private byte[] GetNilBytes() => new byte[8];

  private byte[] GetDoubleBytes(double value) => BitConverter.GetBytes(value);

  private byte[] GetStringBytes(string value)
  {
    byte[] bytes = Encoding.Unicode.GetBytes(value);
    byte[] stringBytes = new byte[bytes.Length + 3];
    bytes.CopyTo((Array) stringBytes, 3);
    BitConverter.GetBytes((ushort) value.Length).CopyTo((Array) stringBytes, 0);
    stringBytes[2] = (byte) 1;
    return stringBytes;
  }

  private void SetReferenceIndex(int referenceIndex)
  {
    switch (referenceIndex)
    {
      case 1:
        this.TokenCode = FormulaToken.tArray1;
        break;
      case 2:
        this.TokenCode = FormulaToken.tArray2;
        break;
      case 3:
        this.TokenCode = FormulaToken.tArray3;
        break;
      default:
        throw new ArgumentOutOfRangeException("index");
    }
  }

  public override int GetSize(OfficeVersion version) => 8;

  public override string ToString(
    FormulaUtil formulaUtil,
    int iRow,
    int iColumn,
    bool bR1C1,
    NumberFormatInfo numberFormat,
    bool isForSerialization)
  {
    string str1 = string.Empty + "{";
    string str2 = ArrayPtg.ColSeparator;
    string str3 = ArrayPtg.RowSeparator;
    if (formulaUtil != null)
    {
      str2 = formulaUtil.OperandsSeparator;
      str3 = formulaUtil.ArrayRowSeparator;
    }
    for (int index1 = 0; index1 <= (int) this.m_usRowNumber; ++index1)
    {
      int index2;
      for (index2 = 0; index2 < (int) this.m_ColumnNumber; ++index2)
        str1 = str1 + (this.m_arrCachedValue[index1, index2] is string ? (object) ('"'.ToString() + this.m_arrCachedValue[index1, index2].ToString() + (object) '"') : this.m_arrCachedValue[index1, index2]) + str2;
      str1 += (string) (this.m_arrCachedValue[index1, index2] is string ? (object) ('"'.ToString() + this.m_arrCachedValue[index1, index2].ToString() + (object) '"') : this.m_arrCachedValue[index1, (int) this.m_ColumnNumber]);
      if (index1 != (int) this.m_usRowNumber)
        str1 += str3;
    }
    return str1 + "}";
  }

  public override byte[] ToByteArray(OfficeVersion version)
  {
    byte[] byteArray = base.ToByteArray(version);
    byteArray[1] = (byte) 0;
    return byteArray;
  }

  public static FormulaToken IndexToCode(int index)
  {
    switch (index)
    {
      case 1:
        return FormulaToken.tArray1;
      case 2:
        return FormulaToken.tArray2;
      case 3:
        return FormulaToken.tArray3;
      default:
        throw new ArgumentOutOfRangeException(nameof (index));
    }
  }

  public new object Clone()
  {
    object obj = base.Clone();
    object[,] arrCachedValue = this.m_arrCachedValue;
    if (this.m_arrCachedValue != null)
    {
      int length1 = arrCachedValue.GetLength(0);
      int length2 = arrCachedValue.GetLength(1);
      for (int index1 = 0; index1 < length1; ++index1)
      {
        for (int index2 = 0; index2 < length2; ++index2)
          this.m_arrCachedValue[index1, index2] = arrCachedValue[index1, index2];
      }
    }
    return obj;
  }

  public override void InfillPTG(DataProvider provider, ref int offset, OfficeVersion version)
  {
    offset += this.GetSize(version) - 1;
  }
}
