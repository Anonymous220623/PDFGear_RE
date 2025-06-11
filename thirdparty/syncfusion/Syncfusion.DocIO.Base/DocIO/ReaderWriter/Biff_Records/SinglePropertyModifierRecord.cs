// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.SinglePropertyModifierRecord
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class SinglePropertyModifierRecord : BaseWordRecord
{
  private const int DEF_MASK_UNIQUE_ID = 511 /*0x01FF*/;
  private const int DEF_START_UNIQUE_ID = 0;
  private const int DEF_BIT_SPECIAL_HANDLE = 9;
  private const int DEF_MASK_SPRM_TYPE = 7168;
  private const int DEF_START_SPRM_TYPE = 10;
  private const int DEF_MASK_OPERAND_SIZE = 57344 /*0xE000*/;
  private const int DEF_START_OPERAND_SIZE = 13;
  private const int DEF_MASK_WORD = 65535 /*0xFFFF*/;
  private ushort m_usOptions;
  private int m_iOperandLength;
  private byte[] m_arrOperand;
  private short m_length = short.MaxValue;

  internal int UniqueID
  {
    get => BaseWordRecord.GetBitsByMask((int) this.m_usOptions, 511 /*0x01FF*/, 0);
    set
    {
      this.m_usOptions = (ushort) (byte) BaseWordRecord.SetBitsByMask((int) this.m_usOptions, 511 /*0x01FF*/, value << 31 /*0x1F*/);
    }
  }

  internal bool IsSpecialHandling
  {
    get => BaseWordRecord.GetBit((int) this.m_usOptions, 9);
    set
    {
      this.m_usOptions = (ushort) (byte) BaseWordRecord.SetBit((int) this.m_usOptions, 9, value);
    }
  }

  internal WordSprmType SprmType
  {
    get => (WordSprmType) BaseWordRecord.GetBitsByMask((int) this.m_usOptions, 7168, 10);
    set
    {
      this.m_usOptions = (ushort) (BaseWordRecord.SetBitsByMask((int) this.m_usOptions, 7168, (int) value << 10) & (int) ushort.MaxValue);
    }
  }

  internal WordSprmOperandSize OperandSize
  {
    get
    {
      return (WordSprmOperandSize) BaseWordRecord.GetBitsByMask((int) this.m_usOptions, 57344 /*0xE000*/, 13);
    }
    set
    {
      this.m_usOptions = (ushort) (BaseWordRecord.SetBitsByMask((int) this.m_usOptions, 57344 /*0xE000*/, (int) value << 13) & (int) ushort.MaxValue);
    }
  }

  internal int OperandLength => this.m_arrOperand == null ? 0 : this.m_arrOperand.Length;

  internal byte[] Operand
  {
    get
    {
      if (this.m_iOperandLength > 0 && this.m_arrOperand == null)
        this.m_arrOperand = new byte[this.m_iOperandLength];
      return this.m_arrOperand;
    }
    set => this.m_arrOperand = value;
  }

  internal bool BoolValue
  {
    get => this.OperandLength > 0 && this.m_arrOperand[0] != (byte) 0;
    set
    {
      this.m_arrOperand = new byte[SinglePropertyModifierRecord.ConvertToInt(this.OperandSize)];
      this.m_arrOperand[0] = value ? (byte) 1 : (byte) 0;
    }
  }

  internal byte ByteValue
  {
    get => this.OperandLength > 0 ? this.Operand[0] : (byte) 0;
    set
    {
      if ((int) this.ByteValue == (int) value)
        return;
      this.Operand = new byte[1]{ value };
    }
  }

  internal ushort UshortValue
  {
    get => this.OperandLength == 2 ? BitConverter.ToUInt16(this.Operand, 0) : (ushort) 0;
    set
    {
      if ((int) this.UshortValue == (int) value)
        return;
      this.Operand = BitConverter.GetBytes(value);
    }
  }

  internal short ShortValue
  {
    get => this.OperandLength == 2 ? BitConverter.ToInt16(this.Operand, 0) : (short) 0;
    set
    {
      if ((int) this.ShortValue == (int) value)
        return;
      this.Operand = BitConverter.GetBytes(value);
    }
  }

  internal int IntValue
  {
    get => this.OperandLength == 4 ? BitConverter.ToInt32(this.Operand, 0) : 0;
    set
    {
      if (this.IntValue == value)
        return;
      this.Operand = BitConverter.GetBytes(value);
    }
  }

  internal uint UIntValue
  {
    get => this.OperandLength == 4 ? BitConverter.ToUInt32(this.Operand, 0) : 0U;
    set
    {
      if ((int) this.UIntValue == (int) value)
        return;
      this.Operand = BitConverter.GetBytes(value);
    }
  }

  internal byte[] ByteArray
  {
    get => this.Operand;
    set => this.Operand = value;
  }

  internal int TypedOptions
  {
    get => (int) this.m_usOptions;
    set => this.m_usOptions = (ushort) value;
  }

  internal ushort Options
  {
    get => this.m_usOptions;
    set => this.m_usOptions = value;
  }

  internal override int Length
  {
    get
    {
      if (this.m_length == short.MaxValue)
        this.m_length = this.GetSprmLength();
      return (int) this.m_length;
    }
  }

  internal WordSprmOptionType OptionType => (WordSprmOptionType) this.m_usOptions;

  internal SinglePropertyModifierRecord()
  {
  }

  internal SinglePropertyModifierRecord(int options)
  {
    this.m_usOptions = (ushort) options;
    this.m_iOperandLength = SinglePropertyModifierRecord.ConvertToInt(this.OperandSize);
  }

  internal SinglePropertyModifierRecord(Stream stream) => this.Parse(stream);

  internal int Parse(byte[] arrBuffer, int iOffset)
  {
    if (iOffset + 2 > arrBuffer.Length)
      throw new ArgumentOutOfRangeException("iOffset is too large.");
    this.m_usOptions = BitConverter.ToUInt16(arrBuffer, iOffset);
    iOffset += 2;
    if (iOffset + 1 > arrBuffer.Length)
      return iOffset + 1;
    int length;
    if (this.OperandSize == WordSprmOperandSize.Variable)
    {
      if (this.m_usOptions == (ushort) 54792)
      {
        length = (int) BitConverter.ToUInt16(arrBuffer, iOffset) - 1;
        if (length < 0)
          length = 0;
        iOffset += 2;
      }
      else
      {
        length = (int) arrBuffer[iOffset];
        ++iOffset;
      }
    }
    else
      length = SinglePropertyModifierRecord.ConvertToInt(this.OperandSize);
    this.m_arrOperand = new byte[length];
    try
    {
      Array.Copy((Array) arrBuffer, iOffset, (Array) this.m_arrOperand, 0, this.m_arrOperand.Length);
    }
    catch
    {
    }
    iOffset += length;
    return iOffset;
  }

  internal void Parse(Stream stream)
  {
    int num = 0;
    byte[] buffer = new byte[2];
    stream.Read(buffer, 0, 2);
    this.m_usOptions = BitConverter.ToUInt16(buffer, 0);
    int count;
    if (this.OperandSize == WordSprmOperandSize.Variable)
    {
      if (this.m_usOptions == (ushort) 54792)
      {
        stream.Read(buffer, 0, 2);
        num = (int) BitConverter.ToUInt16(buffer, 0) - 1;
      }
      count = stream.ReadByte();
    }
    else
      count = SinglePropertyModifierRecord.ConvertToInt(this.OperandSize);
    this.m_arrOperand = new byte[count];
    stream.Read(this.m_arrOperand, 0, count);
  }

  internal override int Save(byte[] arrData, int iOffset)
  {
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    if (iOffset < 0 || iOffset + this.Length > arrData.Length)
      throw new ArgumentOutOfRangeException(nameof (iOffset));
    int num = iOffset;
    BitConverter.GetBytes(this.m_usOptions).CopyTo((Array) arrData, iOffset);
    iOffset += 2;
    if (this.OperandSize == WordSprmOperandSize.Variable)
    {
      if (this.m_usOptions == (ushort) 54792)
      {
        byte[] bytes = BitConverter.GetBytes((ushort) (this.m_arrOperand.Length + 1));
        arrData[iOffset++] = bytes[0];
        arrData[iOffset++] = bytes[1];
      }
      else
      {
        byte length = (byte) this.m_arrOperand.Length;
        arrData[iOffset++] = length;
      }
    }
    if (this.m_arrOperand == null)
      return iOffset - num;
    this.m_arrOperand.CopyTo((Array) arrData, iOffset);
    return iOffset - num + this.m_arrOperand.Length;
  }

  internal int Save(BinaryWriter writer, Stream stream)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (stream));
    int position = (int) stream.Position;
    writer.Write(this.m_usOptions);
    if (this.OperandSize == WordSprmOperandSize.Variable)
    {
      if (this.m_usOptions == (ushort) 54792)
      {
        ushort num = (ushort) (this.m_arrOperand.Length + 1);
        writer.Write(num);
      }
      else
      {
        byte length = (byte) this.m_arrOperand.Length;
        writer.Write(length);
      }
    }
    if (this.m_arrOperand == null)
      this.m_arrOperand = new byte[SinglePropertyModifierRecord.ConvertToInt(this.OperandSize)];
    writer.Write(this.m_arrOperand);
    return (int) (stream.Position - (long) position);
  }

  internal SinglePropertyModifierRecord Clone()
  {
    if (this.Operand == null)
      return (SinglePropertyModifierRecord) null;
    SinglePropertyModifierRecord propertyModifierRecord = new SinglePropertyModifierRecord();
    propertyModifierRecord.TypedOptions = this.TypedOptions;
    if (this.OperandLength > 0)
      propertyModifierRecord.Operand = new byte[this.OperandLength];
    if (propertyModifierRecord.Operand != null)
      this.Operand.CopyTo((Array) propertyModifierRecord.Operand, 0);
    return propertyModifierRecord;
  }

  internal static int ConvertToInt(WordSprmOperandSize operandSize)
  {
    switch (operandSize)
    {
      case WordSprmOperandSize.OneBit:
      case WordSprmOperandSize.OneByte:
        return 1;
      case WordSprmOperandSize.TwoBytes:
      case WordSprmOperandSize.TwoBytes2:
      case WordSprmOperandSize.TwoBytes3:
        return 2;
      case WordSprmOperandSize.FourBytes:
        return 4;
      case WordSprmOperandSize.Variable:
        return -1;
      case WordSprmOperandSize.ThreeBytes:
        return 3;
      default:
        throw new ArgumentOutOfRangeException(nameof (operandSize));
    }
  }

  internal static void ParseOptions(
    int options,
    out WordSprmType type,
    out WordSprmOperandSize opSize)
  {
    type = (WordSprmType) BaseWordRecord.GetBitsByMask(options, 7168, 10);
    int bitsByMask = BaseWordRecord.GetBitsByMask(options, 57344 /*0xE000*/, 13);
    opSize = (WordSprmOperandSize) bitsByMask;
  }

  private void DBG_TestParseOptions()
  {
  }

  private short GetSprmLength()
  {
    int num = 2;
    if (this.OperandSize == WordSprmOperandSize.Variable)
      ++num;
    if (this.m_usOptions == (ushort) 54792)
      ++num;
    return (short) (num + (this.Operand != null ? this.m_arrOperand.Length : 0));
  }
}
