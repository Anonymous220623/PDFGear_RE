// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ArithmeticDecoder
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class ArithmeticDecoder
{
  private Jbig2StreamReader reader;
  private ArithmeticDecoderStats m_genericRegionStats;
  private ArithmeticDecoderStats m_refinementRegionStats;
  private ArithmeticDecoderStats m_iadhStats;
  private ArithmeticDecoderStats m_iadwStats;
  private ArithmeticDecoderStats m_iaexStats;
  private ArithmeticDecoderStats m_iaaiStats;
  private ArithmeticDecoderStats m_iadtStats;
  private ArithmeticDecoderStats m_iaitStats;
  private ArithmeticDecoderStats m_iafsStats;
  private ArithmeticDecoderStats m_iadsStats;
  private ArithmeticDecoderStats m_iardxStats;
  private ArithmeticDecoderStats m_iardyStats;
  private ArithmeticDecoderStats m_iardwStats;
  private ArithmeticDecoderStats m_iardhStats;
  private ArithmeticDecoderStats m_iariStats;
  private ArithmeticDecoderStats m_iaidStats;
  private BitOperation m_bitOperation = new BitOperation();
  private int[] m_contextSize = new int[4]
  {
    16 /*0x10*/,
    13,
    10,
    10
  };
  internal int[] referredToContextSize = new int[2]
  {
    13,
    10
  };
  private long m_buffer0;
  private long m_buffer1;
  private long c;
  private long a;
  private long m_previous;
  private int m_counter;
  private int[] qeTable = new int[47]
  {
    1442906112 /*0x56010000*/,
    872480768 /*0x34010000*/,
    402718720 /*0x18010000*/,
    180420608 /*0x0AC10000*/,
    86048768 /*0x05210000*/,
    35717120 /*0x02210000*/,
    1442906112 /*0x56010000*/,
    1409351680 /*0x54010000*/,
    1208025088 /*0x48010000*/,
    939589632 /*0x38010000*/,
    805371904 /*0x30010000*/,
    604045312 /*0x24010000*/,
    469827584 /*0x1C010000*/,
    369164288 /*0x16010000*/,
    1442906112 /*0x56010000*/,
    1409351680 /*0x54010000*/,
    1359020032 /*0x51010000*/,
    1208025088 /*0x48010000*/,
    939589632 /*0x38010000*/,
    872480768 /*0x34010000*/,
    805371904 /*0x30010000*/,
    671154176 /*0x28010000*/,
    604045312 /*0x24010000*/,
    570490880 /*0x22010000*/,
    469827584 /*0x1C010000*/,
    402718720 /*0x18010000*/,
    369164288 /*0x16010000*/,
    335609856 /*0x14010000*/,
    302055424 /*0x12010000*/,
    285278208 /*0x11010000*/,
    180420608 /*0x0AC10000*/,
    163643392 /*0x09C10000*/,
    144769024 /*0x08A10000*/,
    86048768 /*0x05210000*/,
    71368704 /*0x04410000*/,
    44105728 /*0x02A10000*/,
    35717120 /*0x02210000*/,
    21037056 /*0x01410000*/,
    17891328 /*0x01110000*/,
    8716288 /*0x850000*/,
    4784128 /*0x490000*/,
    2424832 /*0x250000*/,
    1376256 /*0x150000*/,
    589824 /*0x090000*/,
    327680 /*0x050000*/,
    65536 /*0x010000*/,
    1442906112 /*0x56010000*/
  };
  private int[] nmpsTable = new int[47]
  {
    1,
    2,
    3,
    4,
    5,
    38,
    7,
    8,
    9,
    10,
    11,
    12,
    13,
    29,
    15,
    16 /*0x10*/,
    17,
    18,
    19,
    20,
    21,
    22,
    23,
    24,
    25,
    26,
    27,
    28,
    29,
    30,
    31 /*0x1F*/,
    32 /*0x20*/,
    33,
    34,
    35,
    36,
    37,
    38,
    39,
    40,
    41,
    42,
    43,
    44,
    45,
    45,
    46
  };
  private int[] nlpsTable = new int[47]
  {
    1,
    6,
    9,
    12,
    29,
    33,
    6,
    14,
    14,
    14,
    17,
    18,
    20,
    21,
    14,
    14,
    15,
    16 /*0x10*/,
    17,
    18,
    19,
    19,
    20,
    21,
    22,
    23,
    24,
    25,
    26,
    27,
    28,
    29,
    30,
    31 /*0x1F*/,
    32 /*0x20*/,
    33,
    34,
    35,
    36,
    37,
    38,
    39,
    40,
    41,
    42,
    43,
    46
  };
  private int[] switchTable = new int[47]
  {
    1,
    0,
    0,
    0,
    0,
    0,
    1,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    1,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0
  };

  internal ArithmeticDecoderStats GenericRegionStats => this.m_genericRegionStats;

  internal ArithmeticDecoderStats RefinementRegionStats => this.m_refinementRegionStats;

  internal ArithmeticDecoderStats IadhStats => this.m_iadhStats;

  internal ArithmeticDecoderStats IadwStats => this.m_iadwStats;

  internal ArithmeticDecoderStats IaexStats => this.m_iaexStats;

  internal ArithmeticDecoderStats IaaiStats => this.m_iaaiStats;

  internal ArithmeticDecoderStats IadtStats => this.m_iadtStats;

  internal ArithmeticDecoderStats IaitStats => this.m_iaitStats;

  internal ArithmeticDecoderStats IafsStats => this.m_iafsStats;

  internal ArithmeticDecoderStats IadsStats => this.m_iadsStats;

  internal ArithmeticDecoderStats IardxStats => this.m_iardxStats;

  internal ArithmeticDecoderStats IardyStats => this.m_iardyStats;

  internal ArithmeticDecoderStats IardwStats => this.m_iardwStats;

  internal ArithmeticDecoderStats IardhStats => this.m_iardhStats;

  internal ArithmeticDecoderStats IariStats => this.m_iariStats;

  internal ArithmeticDecoderStats IaidStats => this.m_iaidStats;

  private ArithmeticDecoder()
  {
  }

  internal ArithmeticDecoder(Jbig2StreamReader reader)
  {
    this.reader = reader;
    this.m_genericRegionStats = new ArithmeticDecoderStats(2);
    this.m_refinementRegionStats = new ArithmeticDecoderStats(2);
    this.m_iadhStats = new ArithmeticDecoderStats(512 /*0x0200*/);
    this.m_iadwStats = new ArithmeticDecoderStats(512 /*0x0200*/);
    this.m_iaexStats = new ArithmeticDecoderStats(512 /*0x0200*/);
    this.m_iaaiStats = new ArithmeticDecoderStats(512 /*0x0200*/);
    this.m_iadtStats = new ArithmeticDecoderStats(512 /*0x0200*/);
    this.m_iaitStats = new ArithmeticDecoderStats(512 /*0x0200*/);
    this.m_iafsStats = new ArithmeticDecoderStats(512 /*0x0200*/);
    this.m_iadsStats = new ArithmeticDecoderStats(512 /*0x0200*/);
    this.m_iardxStats = new ArithmeticDecoderStats(512 /*0x0200*/);
    this.m_iardyStats = new ArithmeticDecoderStats(512 /*0x0200*/);
    this.m_iardwStats = new ArithmeticDecoderStats(512 /*0x0200*/);
    this.m_iardhStats = new ArithmeticDecoderStats(512 /*0x0200*/);
    this.m_iariStats = new ArithmeticDecoderStats(512 /*0x0200*/);
    this.m_iaidStats = new ArithmeticDecoderStats(2);
  }

  internal void ResetIntegerStats(int symbolCodeLength)
  {
    this.m_iadhStats.reset();
    this.m_iadwStats.reset();
    this.m_iaexStats.reset();
    this.m_iaaiStats.reset();
    this.m_iadtStats.reset();
    this.m_iaitStats.reset();
    this.m_iafsStats.reset();
    this.m_iadsStats.reset();
    this.m_iardxStats.reset();
    this.m_iardyStats.reset();
    this.m_iardwStats.reset();
    this.m_iardhStats.reset();
    this.m_iariStats.reset();
    if (this.m_iaidStats.ContextSize == 1 << symbolCodeLength + 1)
      this.m_iaidStats.reset();
    else
      this.m_iaidStats = new ArithmeticDecoderStats(1 << symbolCodeLength + 1);
  }

  internal void ResetGenericStats(int template, ArithmeticDecoderStats previousStats)
  {
    int num = this.m_contextSize[template];
    if (previousStats != null && previousStats.ContextSize == num)
    {
      if (this.m_genericRegionStats.ContextSize == num)
        this.m_genericRegionStats.overwrite(previousStats);
      else
        this.m_genericRegionStats = previousStats.copy();
    }
    else if (this.m_genericRegionStats.ContextSize == num)
      this.m_genericRegionStats.reset();
    else
      this.m_genericRegionStats = new ArithmeticDecoderStats(1 << num);
  }

  internal void ResetRefinementStats(int template, ArithmeticDecoderStats previousStats)
  {
    int num = this.referredToContextSize[template];
    if (previousStats != null && previousStats.ContextSize == num)
    {
      if (this.m_refinementRegionStats.ContextSize == num)
        this.m_refinementRegionStats.overwrite(previousStats);
      else
        this.m_refinementRegionStats = previousStats.copy();
    }
    else if (this.m_refinementRegionStats.ContextSize == num)
      this.m_refinementRegionStats.reset();
    else
      this.m_refinementRegionStats = new ArithmeticDecoderStats(1 << num);
  }

  internal void Start()
  {
    this.m_buffer0 = (long) this.reader.ReadByte();
    this.m_buffer1 = (long) this.reader.ReadByte();
    this.c = this.m_bitOperation.Bit32Shift(this.m_buffer0 ^ (long) byte.MaxValue, 16 /*0x10*/, 0);
    this.ReadByte();
    this.c = this.m_bitOperation.Bit32Shift(this.c, 7, 0);
    this.m_counter -= 7;
    this.a = 2147483648L /*0x80000000*/;
  }

  internal DecodeIntResult DecodeInt(ArithmeticDecoderStats stats)
  {
    this.m_previous = 1L;
    int num = this.DecodeIntBit(stats);
    long intResult1;
    if (this.DecodeIntBit(stats) != 0)
    {
      if (this.DecodeIntBit(stats) != 0)
      {
        if (this.DecodeIntBit(stats) != 0)
        {
          if (this.DecodeIntBit(stats) != 0)
          {
            if (this.DecodeIntBit(stats) != 0)
            {
              long number = 0;
              for (int index = 0; index < 32 /*0x20*/; ++index)
                number = this.m_bitOperation.Bit32Shift(number, 1, 0) | (long) this.DecodeIntBit(stats);
              intResult1 = number + 4436L;
            }
            else
            {
              long number = 0;
              for (int index = 0; index < 12; ++index)
                number = this.m_bitOperation.Bit32Shift(number, 1, 0) | (long) this.DecodeIntBit(stats);
              intResult1 = number + 340L;
            }
          }
          else
          {
            long number = 0;
            for (int index = 0; index < 8; ++index)
              number = this.m_bitOperation.Bit32Shift(number, 1, 0) | (long) this.DecodeIntBit(stats);
            intResult1 = number + 84L;
          }
        }
        else
        {
          long number = 0;
          for (int index = 0; index < 6; ++index)
            number = this.m_bitOperation.Bit32Shift(number, 1, 0) | (long) this.DecodeIntBit(stats);
          intResult1 = number + 20L;
        }
      }
      else
        intResult1 = (this.m_bitOperation.Bit32Shift(this.m_bitOperation.Bit32Shift(this.m_bitOperation.Bit32Shift((long) this.DecodeIntBit(stats), 1, 0) | (long) this.DecodeIntBit(stats), 1, 0) | (long) this.DecodeIntBit(stats), 1, 0) | (long) this.DecodeIntBit(stats)) + 4L;
    }
    else
      intResult1 = this.m_bitOperation.Bit32Shift((long) this.DecodeIntBit(stats), 1, 0) | (long) this.DecodeIntBit(stats);
    int intResult2;
    if (num != 0)
    {
      if (intResult1 == 0L)
        return new DecodeIntResult((int) intResult1, false);
      intResult2 = (int) -intResult1;
    }
    else
      intResult2 = (int) intResult1;
    return new DecodeIntResult(intResult2, true);
  }

  internal long DecodeIAID(long codeLen, ArithmeticDecoderStats stats)
  {
    this.m_previous = 1L;
    for (long index = 0; index < codeLen; ++index)
    {
      int num = this.DecodeBit(this.m_previous, stats);
      this.m_previous = this.m_bitOperation.Bit32Shift(this.m_previous, 1, 0) | (long) num;
    }
    return this.m_previous - (long) (1 << (int) codeLen);
  }

  internal int DecodeBit(long context, ArithmeticDecoderStats stats)
  {
    int index = this.m_bitOperation.Bit8Shift(stats.getContextCodingTableValue((int) context), 1, 1);
    int num1 = stats.getContextCodingTableValue((int) context) & 1;
    int num2 = this.qeTable[index];
    this.a -= (long) num2;
    int num3;
    if (this.c < this.a)
    {
      if ((this.a & 2147483648L /*0x80000000*/) != 0L)
      {
        num3 = num1;
      }
      else
      {
        if (this.a < (long) num2)
        {
          num3 = 1 - num1;
          if (this.switchTable[index] != 0)
            stats.setContextCodingTableValue((int) context, this.nlpsTable[index] << 1 | 1 - num1);
          else
            stats.setContextCodingTableValue((int) context, this.nlpsTable[index] << 1 | num1);
        }
        else
        {
          num3 = num1;
          stats.setContextCodingTableValue((int) context, this.nmpsTable[index] << 1 | num1);
        }
        do
        {
          if (this.m_counter == 0)
            this.ReadByte();
          this.a = this.m_bitOperation.Bit32Shift(this.a, 1, 0);
          this.c = this.m_bitOperation.Bit32Shift(this.c, 1, 0);
          --this.m_counter;
        }
        while ((this.a & 2147483648L /*0x80000000*/) == 0L);
      }
    }
    else
    {
      this.c -= this.a;
      if (this.a < (long) num2)
      {
        num3 = num1;
        stats.setContextCodingTableValue((int) context, this.nmpsTable[index] << 1 | num1);
      }
      else
      {
        num3 = 1 - num1;
        if (this.switchTable[index] != 0)
          stats.setContextCodingTableValue((int) context, this.nlpsTable[index] << 1 | 1 - num1);
        else
          stats.setContextCodingTableValue((int) context, this.nlpsTable[index] << 1 | num1);
      }
      this.a = (long) num2;
      do
      {
        if (this.m_counter == 0)
          this.ReadByte();
        this.a = this.m_bitOperation.Bit32Shift(this.a, 1, 0);
        this.c = this.m_bitOperation.Bit32Shift(this.c, 1, 0);
        --this.m_counter;
      }
      while ((this.a & 2147483648L /*0x80000000*/) == 0L);
    }
    return num3;
  }

  private void ReadByte()
  {
    if (this.m_buffer0 == (long) byte.MaxValue)
    {
      if (this.m_buffer1 > 143L)
      {
        this.m_counter = 8;
      }
      else
      {
        this.m_buffer0 = this.m_buffer1;
        this.m_buffer1 = (long) this.reader.ReadByte();
        this.c = this.c + 65024L - this.m_bitOperation.Bit32Shift(this.m_buffer0, 9, 0);
        this.m_counter = 7;
      }
    }
    else
    {
      this.m_buffer0 = this.m_buffer1;
      this.m_buffer1 = (long) this.reader.ReadByte();
      this.c = this.c + 65280L - this.m_bitOperation.Bit32Shift(this.m_buffer0, 8, 0);
      this.m_counter = 8;
    }
  }

  private int DecodeIntBit(ArithmeticDecoderStats stats)
  {
    int num = this.DecodeBit(this.m_previous, stats);
    this.m_previous = this.m_previous >= 256L /*0x0100*/ ? (this.m_bitOperation.Bit32Shift(this.m_previous, 1, 0) | (long) num) & 511L /*0x01FF*/ | 256L /*0x0100*/ : this.m_bitOperation.Bit32Shift(this.m_previous, 1, 0) | (long) num;
    return num;
  }
}
