// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Formula.AttrPtg
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Formula;

[CLSCompliant(false)]
[Preserve(AllMembers = true)]
[Token(FormulaToken.tAttr)]
public class AttrPtg : FunctionVarPtg
{
  public const int SIZE = 4;
  private const int DEF_WORD_SIZE = 2;
  private const string DEF_SUM = "SUM";
  private const string DEF_IF = "IF";
  private const string DEF_GOTO = "GOTO";
  private const string DEF_CHOOSE = "CHOOSE";
  private const string DEF_NOT_IMPLEMENTED = "( tAttr not implemented )";
  private const ushort DEF_SPACE_AFTER_MASK = 4;
  private byte m_Options;
  private ushort m_usData;
  private ushort[] m_arrOffsets;

  [Preserve]
  public AttrPtg()
  {
    this.m_Options = (byte) 0;
    this.m_usData = (ushort) 0;
    this.NumberOfArguments = (byte) 1;
    this.TokenCode = FormulaToken.tAttr;
  }

  [Preserve]
  public AttrPtg(DataProvider provider, int iOffset, ExcelVersion version)
    : base(provider, iOffset, version)
  {
  }

  [Preserve]
  public AttrPtg(byte options, ushort usData)
  {
    this.TokenCode = FormulaToken.tAttr;
    this.m_Options = options;
    this.m_usData = usData;
    this.NumberOfArguments = this.m_Options == (byte) 1 ? (byte) 0 : (byte) 1;
  }

  [Preserve]
  public AttrPtg(int options, int data)
    : this((byte) options, (ushort) data)
  {
  }

  public byte Options => this.m_Options;

  public ushort AttrData
  {
    get => this.m_usData;
    set => this.m_usData = value;
  }

  public int AttrData1
  {
    get => (int) this.m_usData & (int) byte.MaxValue;
    internal set
    {
      this.m_usData = (ushort) ((int) this.m_usData & 65280 | value & (int) byte.MaxValue);
    }
  }

  public int SpaceCount
  {
    get => this.HasSpace ? (int) this.m_usData >> 8 : -1;
    set
    {
      if (!this.HasSpace)
        throw new NotSupportedException();
      if (value < 1 || value > (int) byte.MaxValue)
        throw new ArgumentOutOfRangeException(nameof (value), "Value cannot be less than 1 and greater than 255.");
      this.m_usData = (ushort) (((int) this.m_usData & (int) byte.MaxValue) + value << 8);
    }
  }

  public bool SpaceAfterToken
  {
    get => this.HasSpace && ((int) this.m_usData & 4) != 0;
    set
    {
      if (!this.HasSpace)
        throw new NotSupportedException();
      this.m_usData = value ? (ushort) ((int) this.m_usData | 4) : (ushort) ((int) this.m_usData & -5);
    }
  }

  public bool HasSemiVolatile
  {
    get => ((int) this.m_Options & 1) == 1;
    set
    {
      if (value)
        this.m_Options |= (byte) 1;
      else
        this.m_Options &= (byte) 254;
    }
  }

  public bool HasOptimizedIf
  {
    get => ((int) this.m_Options & 2) == 2;
    set
    {
      if (value)
        this.m_Options |= (byte) 2;
      else
        this.m_Options &= (byte) 253;
    }
  }

  public bool HasOptimizedChoose
  {
    get => ((int) this.m_Options & 4) == 4;
    set
    {
      if (value)
        this.m_Options |= (byte) 4;
      else
        this.m_Options &= (byte) 251;
    }
  }

  public bool HasOptGoto
  {
    get => ((int) this.m_Options & 8) == 8;
    set
    {
      if (value)
        this.m_Options |= (byte) 8;
      else
        this.m_Options &= (byte) 247;
    }
  }

  public bool HasSum
  {
    get => ((int) this.m_Options & 16 /*0x10*/) == 16 /*0x10*/;
    set
    {
      if (value)
        this.m_Options |= (byte) 16 /*0x10*/;
      else
        this.m_Options &= (byte) 239;
    }
  }

  public bool HasBaxcel
  {
    get => ((int) this.m_Options & 32 /*0x20*/) == 32 /*0x20*/;
    set
    {
      if (value)
        this.m_Options |= (byte) 32 /*0x20*/;
      else
        this.m_Options &= (byte) 223;
    }
  }

  public bool HasSpace
  {
    get => ((int) this.m_Options & 64 /*0x40*/) == 64 /*0x40*/;
    set
    {
      if (value)
        this.m_Options |= (byte) 64 /*0x40*/;
      else
        this.m_Options &= (byte) 191;
    }
  }

  public override int GetSize(ExcelVersion version)
  {
    return this.HasOptimizedChoose ? 4 + this.m_arrOffsets.Length * 2 : 4;
  }

  public override void PushResultToStack(
    FormulaUtil formulaUtil,
    Stack<object> operands,
    bool isForSerialization)
  {
    if (this.HasSpace)
    {
      string str1 = new string(' ', this.SpaceCount);
      if (this.SpaceAfterToken)
      {
        object obj = operands.Pop();
        string str2 = obj.ToString();
        if (str2.EndsWith(" ") && str2[0] == ' ')
          str2 = operands.Pop().ToString();
        else
          obj = (object) null;
        operands.Push((object) (str2 + str1));
        if (obj == null)
          return;
        operands.Push(obj);
      }
      else
        operands.Push((object) this);
    }
    else
    {
      if (this.m_Options == (byte) 0)
        return;
      base.PushResultToStack(formulaUtil, operands, isForSerialization);
    }
  }

  public override string ToString(
    FormulaUtil formulaUtil,
    int iRow,
    int iColumn,
    bool bR1C1,
    NumberFormatInfo numberFormat,
    bool isForSerialization)
  {
    if (this.HasSum)
      return "SUM";
    if (this.HasOptimizedIf)
      return "IF";
    if (this.HasOptGoto)
      return "GOTO";
    if (this.HasOptimizedChoose)
      return "CHOOSE";
    if (this.HasSpace)
      return new string(' ', this.SpaceCount);
    return this.m_Options == (byte) 0 ? string.Empty : "( tAttr not implemented )";
  }

  public override byte[] ToByteArray(ExcelVersion version)
  {
    byte[] dst = new byte[this.GetSize(version)];
    int num1 = 0;
    byte[] numArray1 = dst;
    int index1 = num1;
    int num2 = index1 + 1;
    numArray1[index1] = (byte) 25;
    byte[] numArray2 = dst;
    int index2 = num2;
    int dstOffset1 = index2 + 1;
    int options = (int) this.m_Options;
    numArray2[index2] = (byte) options;
    Buffer.BlockCopy((Array) BitConverter.GetBytes(this.m_usData), 0, (Array) dst, dstOffset1, 2);
    int dstOffset2 = dstOffset1 + 2;
    if (this.HasOptimizedChoose)
      Buffer.BlockCopy((Array) this.m_arrOffsets, 0, (Array) dst, dstOffset2, this.GetSize(version) - dstOffset2);
    return dst;
  }

  public override void InfillPTG(DataProvider provider, ref int offset, ExcelVersion version)
  {
    int num = offset;
    this.TokenCode = FormulaToken.tAttr;
    this.m_Options = provider.ReadByte(offset++);
    this.m_usData = provider.ReadUInt16(offset);
    offset += 2;
    if (this.HasOptimizedChoose)
    {
      int length = (int) this.m_usData + 1;
      this.m_arrOffsets = new ushort[length];
      for (int index = 0; index < length; ++index)
      {
        this.m_arrOffsets[index] = provider.ReadUInt16(offset);
        offset += 2;
      }
    }
    else
      this.NumberOfArguments = this.m_Options == (byte) 1 ? (byte) 0 : (byte) 1;
    offset = num + this.GetSize(version) - 1;
  }
}
