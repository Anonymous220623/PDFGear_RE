// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.RKRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.RK)]
internal class RKRecord : CellPositionBase, IDoubleValue, IValueHolder
{
  internal const int DEF_RECORD_SIZE = 10;
  internal const int DEF_RECORD_SIZE_WITH_HEADER = 14;
  internal const int DEF_NUMBER_OFFSET = 6;
  internal const int DEF_HEADER_NUMBER_OFFSET = 10;
  public const uint DEF_RK_MASK = 4294967292;
  private const int MaxRkNumber = 536870912 /*0x20000000*/;
  private const int MinRkNumber = -536870912 /*0xE0000000*/;
  [BiffRecordPos(6, 4, true)]
  private int m_iNumber;
  [BiffRecordPos(6, 0, TFieldType.Bit)]
  private bool m_bValueNotChanged;
  [BiffRecordPos(6, 1, TFieldType.Bit)]
  private bool m_bIEEEFloat;

  public int RKNumberInt
  {
    get => this.m_iNumber;
    set
    {
      this.m_iNumber = value;
      this.m_bValueNotChanged = (value & 1) != 0;
      this.m_bIEEEFloat = (value & 2) != 0;
    }
  }

  public double RKNumber
  {
    get
    {
      long num1 = (long) (this.m_iNumber >> 2);
      if (this.IsNotFloat)
      {
        double num2 = (double) num1;
        return !this.IsValueChanged ? num2 : num2 / 100.0;
      }
      double num3 = BitConverterGeneral.Int64BitsToDouble(num1 << 34);
      return !this.IsValueChanged ? num3 : num3 / 100.0;
    }
    set => this.SetRKNumber(value);
  }

  public override int MinimumRecordSize => 10;

  public override int MaximumRecordSize => 10;

  public override int MaximumMemorySize => 10;

  public bool IsNotFloat
  {
    get => this.m_bIEEEFloat;
    set => this.m_bIEEEFloat = value;
  }

  public bool IsValueChanged
  {
    get => this.m_bValueNotChanged;
    set => this.m_bValueNotChanged = value;
  }

  protected override void ParseCellData(DataProvider provider, int iOffset, OfficeVersion version)
  {
    this.m_iNumber = provider.ReadInt32(iOffset);
    this.m_bIEEEFloat = provider.ReadBit(iOffset, 1);
    this.m_bValueNotChanged = provider.ReadBit(iOffset, 0);
  }

  protected override void InfillCellData(DataProvider provider, int iOffset, OfficeVersion version)
  {
    provider.WriteInt32(iOffset, this.m_iNumber);
    provider.WriteBit(iOffset, this.m_bIEEEFloat, 1);
    provider.WriteBit(iOffset, this.m_bValueNotChanged, 0);
  }

  public override int GetStoreSize(OfficeVersion version)
  {
    int storeSize = 10;
    if (version != OfficeVersion.Excel97to2003)
      storeSize += 4;
    return storeSize;
  }

  public void SetRKNumber(string value)
  {
    double result;
    if (!double.TryParse(value, NumberStyles.Any, (IFormatProvider) null, out result))
      return;
    this.SetRKNumber(result);
  }

  public void SetRKNumber(double value)
  {
    this.m_iNumber = RKRecord.ConvertToRKNumber(value);
    this.m_bValueNotChanged = (this.m_iNumber & 1) != 0;
    this.m_bIEEEFloat = (this.m_iNumber & 2) != 0;
  }

  public void SetConvertedNumber(int rkNumber)
  {
    this.m_iNumber = rkNumber;
    this.m_bValueNotChanged = (this.m_iNumber & 1) != 0;
    this.m_bIEEEFloat = (this.m_iNumber & 2) != 0;
  }

  public void SetRKRecord(MulRKRecord.RkRec rc)
  {
    this.m_usExtendedFormat = rc.ExtFormatIndex;
    this.m_iNumber = rc.Rk;
    this.m_bIEEEFloat = (this.m_iNumber & 2) == 2;
    this.m_bValueNotChanged = (this.m_iNumber & 1) == 1;
  }

  public MulRKRecord.RkRec GetAsRkRec()
  {
    if (this.m_bValueNotChanged)
      this.m_iNumber |= 1;
    if (this.m_bIEEEFloat)
      this.m_iNumber |= 2;
    return new MulRKRecord.RkRec(this.m_usExtendedFormat, this.m_iNumber);
  }

  public static int ConvertToRKNumber(string value)
  {
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    double result;
    return double.TryParse(value, NumberStyles.Any, (IFormatProvider) null, out result) ? RKRecord.ConvertToRKNumber(result) : int.MaxValue;
  }

  public static int ConvertToRKNumber(double value)
  {
    if (value > 536870912.0 || value < -536870912.0)
      return int.MaxValue;
    long int64Bits1 = BitConverterGeneral.DoubleToInt64Bits(value);
    int num1 = 0;
    bool flag = true;
    if ((int64Bits1 & 17179869183L /*0x03FFFFFFFF*/) == 0L)
    {
      num1 = RKRecord.ConvertDouble(int64Bits1, false);
      flag = false;
    }
    if (flag)
    {
      int num2 = (int) Math.Round(value, 0);
      if (value - (double) num2 == 0.0 && num2 > 0 && num2 <= 1073741823 /*0x3FFFFFFF*/)
      {
        num1 = num2 << 2 | 2;
        flag = false;
      }
    }
    if (flag)
    {
      value *= 100.0;
      long int64Bits2 = BitConverterGeneral.DoubleToInt64Bits(value);
      if ((int64Bits2 & 17179869183L /*0x03FFFFFFFF*/) == 0L)
      {
        num1 = RKRecord.ConvertDouble(int64Bits2, true);
        flag = false;
      }
    }
    if (flag)
    {
      int num3 = (int) Math.Round(value, 0);
      if (value - (double) num3 == 0.0 && num3 > 0 && num3 <= 1073741823 /*0x3FFFFFFF*/)
        num1 = num3 << 2 | 3;
    }
    return !flag ? num1 : int.MaxValue;
  }

  public static double ConvertToDouble(int rkNumber)
  {
    bool flag1 = (rkNumber & 1) != 0;
    bool flag2 = (rkNumber & 2) != 0;
    long num1 = (long) (rkNumber >> 2);
    if (flag2)
    {
      double num2 = (double) num1;
      return !flag1 ? num2 : num2 / 100.0;
    }
    double num3 = BitConverterGeneral.Int64BitsToDouble(num1 << 34);
    return !flag1 ? num3 : num3 / 100.0;
  }

  private static int ConvertDouble(long value, bool bValueNotChanged)
  {
    int num = (int) (value >> 32 /*0x20*/);
    if (bValueNotChanged)
      num |= 1;
    return num;
  }

  public static double EncodeRK(int value)
  {
    double num = (value & 2) <= 0 ? RKRecord.SafeGetDouble(value) : (double) (value >> 2);
    if ((value & 1) > 0)
      num /= 100.0;
    return num;
  }

  private static double SafeGetDouble(int value)
  {
    byte[] dst = new byte[8];
    Buffer.BlockCopy((Array) BitConverter.GetBytes((long) value & 4294967292L), 0, (Array) dst, 4, 4);
    return BitConverter.ToDouble(dst, 0);
  }

  public static int ReadValue(DataProvider provider, int recordStart, OfficeVersion version)
  {
    recordStart += 10;
    if (version != OfficeVersion.Excel97to2003)
      recordStart += 4;
    return provider.ReadInt32(recordStart);
  }

  public double DoubleValue => this.RKNumber;

  public object Value
  {
    get => (object) this.RKNumber;
    set => this.RKNumber = (double) value;
  }
}
