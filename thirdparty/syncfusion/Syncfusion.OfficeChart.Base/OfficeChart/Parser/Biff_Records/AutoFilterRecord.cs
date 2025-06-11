// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.AutoFilterRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.AutoFilter)]
[CLSCompliant(false)]
internal class AutoFilterRecord : BiffRecordRaw, ICloneable
{
  private const int DEF_RECORD_MIN_SIZE = 24;
  private const int DEF_TOP10_BITMASK = 65408;
  private const int DEF_TOP10_FIRSTBIT = 7;
  private const int DEF_FIRST_CONDITION_OFFSET = 4;
  private const int DEF_SECOND_CONDITION_OFFSET = 14;
  private const int DEF_ADDITIONAL_OFFSET = 24;
  [BiffRecordPos(0, 2)]
  private ushort m_usIndex;
  [BiffRecordPos(2, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(2, 0, TFieldType.Bit)]
  private bool m_bOr;
  [BiffRecordPos(2, 2, TFieldType.Bit)]
  private bool m_bSimple1;
  [BiffRecordPos(2, 3, TFieldType.Bit)]
  private bool m_bSimple2;
  [BiffRecordPos(2, 4, TFieldType.Bit)]
  private bool m_bTop10;
  [BiffRecordPos(2, 5, TFieldType.Bit)]
  private bool m_bTop;
  [BiffRecordPos(2, 6, TFieldType.Bit)]
  private bool m_bPercent;
  private AutoFilterRecord.DOPER m_firstCondition = new AutoFilterRecord.DOPER();
  private AutoFilterRecord.DOPER m_secondCondition = new AutoFilterRecord.DOPER();

  public override bool NeedDataArray => true;

  public override int MinimumRecordSize => 24;

  public ushort Index
  {
    get => this.m_usIndex;
    set => this.m_usIndex = value;
  }

  public ushort Options => this.m_usOptions;

  public bool IsSimple1
  {
    get => this.m_bSimple1;
    set => this.m_bSimple1 = value;
  }

  public bool IsSimple2
  {
    get => this.m_bSimple2;
    set => this.m_bSimple2 = value;
  }

  public bool IsTop10
  {
    get => this.m_bTop10;
    set => this.m_bTop10 = value;
  }

  public bool IsTop
  {
    get => this.m_bTop;
    set => this.m_bTop = value;
  }

  public bool IsPercent
  {
    get => this.m_bPercent;
    set => this.m_bPercent = value;
  }

  public bool IsAnd
  {
    get => !this.m_bOr;
    set => this.m_bOr = !value;
  }

  public int Top10Number
  {
    get => (int) BiffRecordRaw.GetUInt16BitsByMask(this.m_usOptions, (ushort) 65408) >> 7;
    set
    {
      if (value < 0 || value > 500)
        throw new ArgumentOutOfRangeException(nameof (Top10Number));
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usOptions, (ushort) 65408, (ushort) (value << 7));
    }
  }

  public AutoFilterRecord.DOPER FirstCondition => this.m_firstCondition;

  public AutoFilterRecord.DOPER SecondCondition => this.m_secondCondition;

  public bool IsBlank
  {
    get
    {
      return this.FirstCondition.DataType == AutoFilterRecord.DOPER.DOPERDataType.MatchBlanks && this.SecondCondition.DataType == AutoFilterRecord.DOPER.DOPERDataType.FilterNotUsed;
    }
  }

  public bool IsNonBlank
  {
    get
    {
      return this.FirstCondition.DataType == AutoFilterRecord.DOPER.DOPERDataType.MatchNonBlanks && this.SecondCondition.DataType == AutoFilterRecord.DOPER.DOPERDataType.FilterNotUsed;
    }
  }

  public AutoFilterRecord()
  {
  }

  public AutoFilterRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public AutoFilterRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    int num = iOffset;
    this.m_usIndex = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usOptions = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_bOr = BiffRecordRaw.GetBitFromVar(this.m_usOptions, 0);
    this.m_bSimple1 = BiffRecordRaw.GetBitFromVar(this.m_usOptions, 2);
    this.m_bSimple2 = BiffRecordRaw.GetBitFromVar(this.m_usOptions, 3);
    this.m_bTop10 = BiffRecordRaw.GetBitFromVar(this.m_usOptions, 4);
    this.m_bTop = BiffRecordRaw.GetBitFromVar(this.m_usOptions, 5);
    this.m_bPercent = BiffRecordRaw.GetBitFromVar(this.m_usOptions, 6);
    this.m_firstCondition.Parse(provider, num + 4);
    this.m_secondCondition.Parse(provider, num + 14);
    iOffset = num + 24;
    iOffset += this.m_firstCondition.ParseAdditionalData(provider, iOffset);
    this.m_secondCondition.ParseAdditionalData(provider, iOffset);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iLength = this.GetStoreSize(OfficeVersion.Excel97to2003);
    provider.WriteUInt16(iOffset, this.m_usIndex);
    iOffset += 2;
    this.SetBitInVar(ref this.m_usOptions, this.m_bOr, 0);
    this.SetBitInVar(ref this.m_usOptions, this.m_bSimple1, 2);
    this.SetBitInVar(ref this.m_usOptions, this.m_bSimple2, 3);
    this.SetBitInVar(ref this.m_usOptions, this.m_bTop10, 4);
    this.SetBitInVar(ref this.m_usOptions, this.m_bTop, 5);
    this.SetBitInVar(ref this.m_usOptions, this.m_bPercent, 6);
    provider.WriteUInt16(iOffset, this.m_usOptions);
    iOffset += 2;
    iOffset += this.m_firstCondition.Serialize(provider, iOffset);
    iOffset += this.m_secondCondition.Serialize(provider, iOffset);
    iOffset += this.m_firstCondition.SerializeAdditionalData(provider, iOffset);
    iOffset += this.m_secondCondition.SerializeAdditionalData(provider, iOffset);
  }

  public override int GetStoreSize(OfficeVersion version)
  {
    return 4 + this.m_firstCondition.Length + this.m_secondCondition.Length;
  }

  public new object Clone()
  {
    AutoFilterRecord autoFilterRecord = (AutoFilterRecord) base.Clone();
    if (this.m_firstCondition != null)
      autoFilterRecord.m_firstCondition = (AutoFilterRecord.DOPER) this.m_firstCondition.Clone();
    if (this.m_secondCondition != null)
      autoFilterRecord.m_secondCondition = (AutoFilterRecord.DOPER) this.m_secondCondition.Clone();
    return (object) autoFilterRecord;
  }

  public class DOPER : ICloneable
  {
    private const int DEF_SIZE = 10;
    private const int DEF_DATATYPE_OFFSET = 0;
    private const int DEF_SIGN_OFFSET = 1;
    private const int DEF_VALUE_OFFSET = 2;
    private const int DEF_STRING_LENGTH_OFFSET = 6;
    private byte[] m_data = new byte[10];
    private string m_strValue = string.Empty;

    public AutoFilterRecord.DOPER.DOPERDataType DataType
    {
      get => (AutoFilterRecord.DOPER.DOPERDataType) this.m_data[0];
      set => this.m_data[0] = (byte) value;
    }

    public AutoFilterRecord.DOPER.DOPERComparisonSign ComparisonSign
    {
      get => (AutoFilterRecord.DOPER.DOPERComparisonSign) this.m_data[1];
      set => this.m_data[1] = (byte) value;
    }

    public int RKNumber
    {
      get => BitConverter.ToInt32(this.m_data, 2);
      set
      {
        this.DataType = AutoFilterRecord.DOPER.DOPERDataType.RKNumber;
        BitConverter.GetBytes(value).CopyTo((Array) this.m_data, 2);
      }
    }

    public double Number
    {
      get => BitConverter.ToDouble(this.m_data, 2);
      set
      {
        this.DataType = AutoFilterRecord.DOPER.DOPERDataType.Number;
        BitConverter.GetBytes(value).CopyTo((Array) this.m_data, 2);
      }
    }

    public bool IsBool
    {
      get
      {
        return this.DataType == AutoFilterRecord.DOPER.DOPERDataType.BoolOrError && this.m_data[2] == (byte) 1;
      }
    }

    public bool Boolean
    {
      get => this.m_data[3] != (byte) 0;
      set
      {
        this.DataType = AutoFilterRecord.DOPER.DOPERDataType.BoolOrError;
        this.m_data[2] = (byte) 1;
        this.m_data[3] = value ? (byte) 1 : (byte) 0;
      }
    }

    public byte ErrorCode
    {
      get => this.m_data[3];
      set
      {
        this.DataType = AutoFilterRecord.DOPER.DOPERDataType.BoolOrError;
        this.m_data[2] = (byte) 0;
        this.m_data[3] = value;
      }
    }

    public bool HasAdditionalData => this.DataType == AutoFilterRecord.DOPER.DOPERDataType.String;

    public byte StringLength
    {
      get => this.m_data[6];
      set
      {
        this.DataType = AutoFilterRecord.DOPER.DOPERDataType.String;
        this.m_data[6] = value;
      }
    }

    public string StringValue
    {
      get => this.m_strValue;
      set
      {
        this.m_data[7] = (byte) 1;
        this.m_strValue = value;
        this.StringLength = (byte) value.Length;
      }
    }

    public int Length => 10 + (this.StringLength > (byte) 0 ? (int) this.StringLength * 2 + 1 : 0);

    public int Parse(DataProvider provider, int iOffset)
    {
      provider.ReadArray(iOffset, this.m_data);
      return 10;
    }

    public int ParseAdditionalData(DataProvider provider, int iOffset)
    {
      if (!this.HasAdditionalData || this.DataType != AutoFilterRecord.DOPER.DOPERDataType.String || this.StringLength <= (byte) 0)
        return 0;
      int iBytesInString;
      this.m_strValue = provider.ReadString(iOffset, (int) this.StringLength, out iBytesInString, false);
      return iBytesInString;
    }

    public int Serialize(DataProvider provider, int iOffset)
    {
      if (provider == null)
        throw new ArgumentNullException(nameof (provider));
      provider.WriteBytes(iOffset, this.m_data, 0, 10);
      return 10;
    }

    public int SerializeAdditionalData(DataProvider provider, int iOffset)
    {
      if (this.m_strValue == null || this.m_strValue.Length == 0)
        return 0;
      if (provider == null)
        throw new ArgumentNullException(nameof (provider));
      int num = iOffset >= 0 ? iOffset : throw new ArgumentOutOfRangeException(nameof (iOffset));
      provider.WriteStringNoLenUpdateOffset(ref iOffset, this.m_strValue, true);
      return iOffset - num;
    }

    public object Clone()
    {
      AutoFilterRecord.DOPER doper = (AutoFilterRecord.DOPER) this.MemberwiseClone();
      doper.m_data = new byte[10];
      Buffer.BlockCopy((Array) this.m_data, 0, (Array) doper.m_data, 0, 10);
      return (object) doper;
    }

    public enum DOPERDataType
    {
      FilterNotUsed = 0,
      RKNumber = 2,
      Number = 4,
      String = 6,
      BoolOrError = 8,
      MatchBlanks = 12, // 0x0000000C
      MatchNonBlanks = 14, // 0x0000000E
    }

    public enum DOPERComparisonSign
    {
      Less = 1,
      Equal = 2,
      LessOrEqual = 3,
      Greater = 4,
      NotEqual = 5,
      GreaterOrEqual = 6,
    }
  }
}
