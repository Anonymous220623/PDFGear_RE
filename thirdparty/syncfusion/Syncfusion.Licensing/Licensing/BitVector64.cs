// Decompiled with JetBrains decompiler
// Type: Syncfusion.Licensing.BitVector64
// Assembly: Syncfusion.Licensing, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=632609b4d040f6b4
// MVID: 46253E3A-52AF-49F3-BF00-D811A33B7BC6
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Licensing.dll

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace Syncfusion.Licensing;

[EditorBrowsable(EditorBrowsableState.Never)]
public struct BitVector64
{
  private ulong vectorData;

  public BitVector64(long initialValue) => this.vectorData = (ulong) initialValue;

  public BitVector64(int initialValue) => this.vectorData = (ulong) initialValue;

  public BitVector64(BitVector32 initialValue) => this.vectorData = (ulong) initialValue.Data;

  public BitVector64(BitVector64 initialValue) => this.vectorData = initialValue.Data;

  public ulong Data => this.vectorData;

  public bool this[uint bit]
  {
    get
    {
      if (bit > 63U /*0x3F*/)
        bit = 63U /*0x3F*/;
      return (this.vectorData & (ulong) Math.Pow(2.0, (double) bit)) > 0UL;
    }
    set
    {
      if (value)
        this.vectorData |= (ulong) bit;
      else
        this.vectorData &= (ulong) ~bit;
    }
  }

  public ulong this[BitVector64.Section section]
  {
    get => (this.vectorData & (ulong) section.Mask << (int) section.Offset) >> (int) section.Offset;
    set
    {
      ulong num = (ulong) (uint.MaxValue & (uint) section.Mask) << (int) section.Offset;
      this.vectorData = (ulong) ((long) this.vectorData & ~(long) num | (long) value << (int) section.Offset & (long) num);
    }
  }

  public static ulong CreateMask() => BitVector64.CreateMask(0UL);

  public static ulong CreateMask(ulong previous)
  {
    ulong mask = 1;
    if (previous != 0UL)
    {
      if (previous == 9223372036854775808UL /*0x8000000000000000*/)
        throw new InvalidOperationException("BitVectorFull");
      mask = previous << 1;
    }
    return mask;
  }

  public static BitVector64.Section CreateSection(short maxValue)
  {
    return BitVector64.CreateSectionHelper(maxValue, (ushort) 0, (ushort) 0);
  }

  public static BitVector64.Section CreateSection(short maxValue, BitVector64.Section previous)
  {
    return BitVector64.CreateSectionHelper(maxValue, previous.Mask, previous.Offset);
  }

  public static string ToString(BitVector64 value)
  {
    StringBuilder stringBuilder = new StringBuilder(45);
    stringBuilder.Append("BitVector64{");
    ulong data = value.Data;
    for (int index = 0; index < 64 /*0x40*/; ++index)
    {
      if (((long) data & long.MinValue) != 0L)
        stringBuilder.Append("1");
      else
        stringBuilder.Append("0");
      data <<= 1;
    }
    stringBuilder.Append("}");
    return stringBuilder.ToString();
  }

  public override string ToString() => BitVector64.ToString(this);

  public override bool Equals(object objectValue)
  {
    bool flag = false;
    if (objectValue is BitVector64 bitVector64)
      flag = (long) this.Data == (long) bitVector64.Data;
    return flag;
  }

  public override int GetHashCode() => base.GetHashCode();

  private static ushort CreateMaskFromHighValue(int highValue)
  {
    int num = 32 /*0x20*/;
    for (; ((long) highValue & long.MinValue) == 0L; highValue <<= 1)
      --num;
    uint maskFromHighValue = 0;
    while (num > 0)
    {
      --num;
      maskFromHighValue = maskFromHighValue << 1 | 1U;
    }
    return (ushort) maskFromHighValue;
  }

  private static BitVector64.Section CreateSectionHelper(
    short maxValue,
    ushort priorMask,
    ushort priorOffset)
  {
    if (maxValue < (short) 1)
      throw new ArgumentException("Argument_InvalidValue maxValue");
    ushort offset = (ushort) ((uint) priorOffset + (uint) BitVector64.CountBitsSet(priorMask));
    return offset < (ushort) 64 /*0x40*/ ? new BitVector64.Section(BitVector64.CreateMaskFromHighValue((int) maxValue), offset) : throw new InvalidOperationException("BitVectorFull");
  }

  private static ushort CountBitsSet(ushort mask)
  {
    ushort num = 0;
    for (; ((int) mask & 1) != 0; mask >>= 1)
      ++num;
    return num;
  }

  public struct Section(ushort mask, ushort offset)
  {
    private readonly ushort mask = mask;
    private readonly ushort offset = offset;

    public ushort Mask => this.mask;

    public ushort Offset => this.offset;

    public static bool operator ==(BitVector64.Section a, BitVector64.Section b) => a.Equals(b);

    public static bool operator !=(BitVector64.Section a, BitVector64.Section b) => !(a == b);

    public static string ToString(BitVector64.Section value)
    {
      return $"Section{{0x{value.Mask:X}, 0x{value.Offset:X}}}";
    }

    public override string ToString() => BitVector64.Section.ToString(this);

    public override bool Equals(object objectValue)
    {
      bool flag = false;
      if (objectValue is BitVector64.Section section)
        flag = this.Equals(section);
      return flag;
    }

    public bool Equals(BitVector64.Section section)
    {
      bool flag = false;
      if ((int) section.mask == (int) this.mask)
        flag = (int) section.offset == (int) this.offset;
      return flag;
    }

    public override int GetHashCode() => base.GetHashCode();
  }
}
