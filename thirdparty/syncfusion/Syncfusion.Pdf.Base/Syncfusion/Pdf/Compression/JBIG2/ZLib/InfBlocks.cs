// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.ZLib.InfBlocks
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.ZLib;

internal sealed class InfBlocks
{
  private const int MANY = 1440;
  private const int Z_OK = 0;
  private const int Z_STREAM_END = 1;
  private const int Z_NEED_DICT = 2;
  private const int Z_ERRNO = -1;
  private const int Z_STREAM_ERROR = -2;
  private const int Z_DATA_ERROR = -3;
  private const int Z_MEM_ERROR = -4;
  private const int Z_BUF_ERROR = -5;
  private const int Z_VERSION_ERROR = -6;
  private const int TYPE = 0;
  private const int LENS = 1;
  private const int STORED = 2;
  private const int TABLE = 3;
  private const int BTREE = 4;
  private const int DTREE = 5;
  private const int CODES = 6;
  private const int DRY = 7;
  private const int DONE = 8;
  private const int BAD = 9;
  private static readonly int[] inflate_mask = new int[17]
  {
    0,
    1,
    3,
    7,
    15,
    31 /*0x1F*/,
    63 /*0x3F*/,
    (int) sbyte.MaxValue,
    (int) byte.MaxValue,
    511 /*0x01FF*/,
    1023 /*0x03FF*/,
    2047 /*0x07FF*/,
    4095 /*0x0FFF*/,
    8191 /*0x1FFF*/,
    16383 /*0x3FFF*/,
    (int) short.MaxValue,
    (int) ushort.MaxValue
  };
  internal static readonly int[] border = new int[19]
  {
    16 /*0x10*/,
    17,
    18,
    0,
    8,
    7,
    9,
    6,
    10,
    5,
    11,
    4,
    12,
    3,
    13,
    2,
    14,
    1,
    15
  };
  internal int mode;
  internal int left;
  internal int table;
  internal int index;
  internal int[] blens;
  internal int[] bb = new int[1];
  internal int[] tb = new int[1];
  internal InfCodes codes;
  internal int last;
  internal int bitk;
  internal int bitb;
  internal int[] hufts;
  internal byte[] window;
  internal int end;
  internal int read;
  internal int write;
  internal object checkfn;
  internal long check;

  internal InfBlocks(ZStream z, object checkfn, int w)
  {
    this.hufts = new int[4320];
    this.window = new byte[w];
    this.end = w;
    this.checkfn = checkfn;
    this.mode = 0;
    this.reset(z, (long[]) null);
  }

  internal void reset(ZStream z, long[] c)
  {
    if (c != null)
      c[0] = this.check;
    if (this.mode == 4 || this.mode == 5)
      this.blens = (int[]) null;
    if (this.mode == 6)
      this.codes.free(z);
    this.mode = 0;
    this.bitk = 0;
    this.bitb = 0;
    this.read = this.write = 0;
    if (this.checkfn == null)
      return;
    z.adler = this.check = z._adler.adler32(0L, (byte[]) null, 0, 0);
  }

  internal int proc(ZStream z, int r)
  {
    int nextInIndex = z.next_in_index;
    int availIn = z.avail_in;
    int number1 = this.bitb;
    int num1 = this.bitk;
    int dstOffset = this.write;
    int num2 = dstOffset < this.read ? this.read - dstOffset - 1 : this.end - dstOffset;
    int num3;
    int num4;
    while (true)
    {
      int count;
      do
      {
        switch (this.mode)
        {
          case 0:
            for (; num1 < 3; num1 += 8)
            {
              if (availIn != 0)
              {
                r = 0;
                --availIn;
                number1 |= ((int) z.next_in[nextInIndex++] & (int) byte.MaxValue) << num1;
              }
              else
              {
                this.bitb = number1;
                this.bitk = num1;
                z.avail_in = availIn;
                z.total_in += (long) (nextInIndex - z.next_in_index);
                z.next_in_index = nextInIndex;
                this.write = dstOffset;
                return this.inflate_flush(z, r);
              }
            }
            int number2 = number1 & 7;
            this.last = number2 & 1;
            switch (SupportClass.URShift(number2, 1))
            {
              case 0:
                int number3 = SupportClass.URShift(number1, 3);
                int num5 = num1 - 3;
                int bits1 = num5 & 7;
                number1 = SupportClass.URShift(number3, bits1);
                num1 = num5 - bits1;
                this.mode = 1;
                continue;
              case 1:
                int[] bl1 = new int[1];
                int[] bd1 = new int[1];
                int[][] tl1 = new int[1][];
                int[][] td1 = new int[1][];
                InfTree.inflate_trees_fixed(bl1, bd1, tl1, td1, z);
                this.codes = new InfCodes(bl1[0], bd1[0], tl1[0], td1[0], z);
                number1 = SupportClass.URShift(number1, 3);
                num1 -= 3;
                this.mode = 6;
                continue;
              case 2:
                number1 = SupportClass.URShift(number1, 3);
                num1 -= 3;
                this.mode = 3;
                continue;
              case 3:
                int num6 = SupportClass.URShift(number1, 3);
                int num7 = num1 - 3;
                this.mode = 9;
                z.msg = "invalid block type";
                r = -3;
                this.bitb = num6;
                this.bitk = num7;
                z.avail_in = availIn;
                z.total_in += (long) (nextInIndex - z.next_in_index);
                z.next_in_index = nextInIndex;
                this.write = dstOffset;
                return this.inflate_flush(z, r);
              default:
                continue;
            }
          case 1:
            for (; num1 < 32 /*0x20*/; num1 += 8)
            {
              if (availIn != 0)
              {
                r = 0;
                --availIn;
                number1 |= ((int) z.next_in[nextInIndex++] & (int) byte.MaxValue) << num1;
              }
              else
              {
                this.bitb = number1;
                this.bitk = num1;
                z.avail_in = availIn;
                z.total_in += (long) (nextInIndex - z.next_in_index);
                z.next_in_index = nextInIndex;
                this.write = dstOffset;
                return this.inflate_flush(z, r);
              }
            }
            if ((SupportClass.URShift(~number1, 16 /*0x10*/) & (int) ushort.MaxValue) != (number1 & (int) ushort.MaxValue))
            {
              this.mode = 9;
              z.msg = "invalid stored block lengths";
              r = -3;
              this.bitb = number1;
              this.bitk = num1;
              z.avail_in = availIn;
              z.total_in += (long) (nextInIndex - z.next_in_index);
              z.next_in_index = nextInIndex;
              this.write = dstOffset;
              return this.inflate_flush(z, r);
            }
            this.left = number1 & (int) ushort.MaxValue;
            number1 = num1 = 0;
            this.mode = this.left != 0 ? 2 : (this.last != 0 ? 7 : 0);
            continue;
          case 2:
            if (availIn == 0)
            {
              this.bitb = number1;
              this.bitk = num1;
              z.avail_in = availIn;
              z.total_in += (long) (nextInIndex - z.next_in_index);
              z.next_in_index = nextInIndex;
              this.write = dstOffset;
              return this.inflate_flush(z, r);
            }
            if (num2 == 0)
            {
              if (dstOffset == this.end && this.read != 0)
              {
                dstOffset = 0;
                num2 = dstOffset < this.read ? this.read - dstOffset - 1 : this.end - dstOffset;
              }
              if (num2 == 0)
              {
                this.write = dstOffset;
                r = this.inflate_flush(z, r);
                dstOffset = this.write;
                num2 = dstOffset < this.read ? this.read - dstOffset - 1 : this.end - dstOffset;
                if (dstOffset == this.end && this.read != 0)
                {
                  dstOffset = 0;
                  num2 = dstOffset < this.read ? this.read - dstOffset - 1 : this.end - dstOffset;
                }
                if (num2 == 0)
                {
                  this.bitb = number1;
                  this.bitk = num1;
                  z.avail_in = availIn;
                  z.total_in += (long) (nextInIndex - z.next_in_index);
                  z.next_in_index = nextInIndex;
                  this.write = dstOffset;
                  return this.inflate_flush(z, r);
                }
              }
            }
            r = 0;
            count = this.left;
            if (count > availIn)
              count = availIn;
            if (count > num2)
              count = num2;
            Buffer.BlockCopy((Array) z.next_in, nextInIndex, (Array) this.window, dstOffset, count);
            nextInIndex += count;
            availIn -= count;
            dstOffset += count;
            num2 -= count;
            continue;
          case 3:
            goto label_37;
          case 4:
            goto label_46;
          case 5:
            goto label_54;
          case 6:
            goto label_76;
          case 7:
            goto label_81;
          case 8:
            goto label_84;
          case 9:
            goto label_85;
          default:
            goto label_86;
        }
      }
      while ((this.left -= count) != 0);
      this.mode = this.last != 0 ? 7 : 0;
      continue;
label_37:
      for (; num1 < 14; num1 += 8)
      {
        if (availIn != 0)
        {
          r = 0;
          --availIn;
          number1 |= ((int) z.next_in[nextInIndex++] & (int) byte.MaxValue) << num1;
        }
        else
        {
          this.bitb = number1;
          this.bitk = num1;
          z.avail_in = availIn;
          z.total_in += (long) (nextInIndex - z.next_in_index);
          z.next_in_index = nextInIndex;
          this.write = dstOffset;
          return this.inflate_flush(z, r);
        }
      }
      int num8;
      this.table = num8 = number1 & 16383 /*0x3FFF*/;
      if ((num8 & 31 /*0x1F*/) <= 29 && (num8 >> 5 & 31 /*0x1F*/) <= 29)
      {
        this.blens = new int[258 + (num8 & 31 /*0x1F*/) + (num8 >> 5 & 31 /*0x1F*/)];
        number1 = SupportClass.URShift(number1, 14);
        num1 -= 14;
        this.index = 0;
        this.mode = 4;
      }
      else
        break;
label_46:
      while (this.index < 4 + SupportClass.URShift(this.table, 10))
      {
        for (; num1 < 3; num1 += 8)
        {
          if (availIn != 0)
          {
            r = 0;
            --availIn;
            number1 |= ((int) z.next_in[nextInIndex++] & (int) byte.MaxValue) << num1;
          }
          else
          {
            this.bitb = number1;
            this.bitk = num1;
            z.avail_in = availIn;
            z.total_in += (long) (nextInIndex - z.next_in_index);
            z.next_in_index = nextInIndex;
            this.write = dstOffset;
            return this.inflate_flush(z, r);
          }
        }
        this.blens[InfBlocks.border[this.index++]] = number1 & 7;
        number1 = SupportClass.URShift(number1, 3);
        num1 -= 3;
      }
      while (this.index < 19)
        this.blens[InfBlocks.border[this.index++]] = 0;
      this.bb[0] = 7;
      num3 = InfTree.inflate_trees_bits(this.blens, this.bb, this.tb, this.hufts, z);
      if (num3 == 0)
      {
        this.index = 0;
        this.mode = 5;
      }
      else
        goto label_50;
label_54:
      while (true)
      {
        int table1 = this.table;
        if (this.index < 258 + (table1 & 31 /*0x1F*/) + (table1 >> 5 & 31 /*0x1F*/))
        {
          int index1;
          for (index1 = this.bb[0]; num1 < index1; num1 += 8)
          {
            if (availIn != 0)
            {
              r = 0;
              --availIn;
              number1 |= ((int) z.next_in[nextInIndex++] & (int) byte.MaxValue) << num1;
            }
            else
            {
              this.bitb = number1;
              this.bitk = num1;
              z.avail_in = availIn;
              z.total_in += (long) (nextInIndex - z.next_in_index);
              z.next_in_index = nextInIndex;
              this.write = dstOffset;
              return this.inflate_flush(z, r);
            }
          }
          int num9 = this.tb[0];
          int huft1 = this.hufts[(this.tb[0] + (number1 & InfBlocks.inflate_mask[index1])) * 3 + 1];
          int huft2 = this.hufts[(this.tb[0] + (number1 & InfBlocks.inflate_mask[huft1])) * 3 + 2];
          if (huft2 < 16 /*0x10*/)
          {
            number1 = SupportClass.URShift(number1, huft1);
            num1 -= huft1;
            this.blens[this.index++] = huft2;
          }
          else
          {
            int bits2 = huft2 == 18 ? 7 : huft2 - 14;
            int num10 = huft2 == 18 ? 11 : 3;
            for (; num1 < huft1 + bits2; num1 += 8)
            {
              if (availIn != 0)
              {
                r = 0;
                --availIn;
                number1 |= ((int) z.next_in[nextInIndex++] & (int) byte.MaxValue) << num1;
              }
              else
              {
                this.bitb = number1;
                this.bitk = num1;
                z.avail_in = availIn;
                z.total_in += (long) (nextInIndex - z.next_in_index);
                z.next_in_index = nextInIndex;
                this.write = dstOffset;
                return this.inflate_flush(z, r);
              }
            }
            int number4 = SupportClass.URShift(number1, huft1);
            int num11 = num1 - huft1;
            int num12 = num10 + (number4 & InfBlocks.inflate_mask[bits2]);
            number1 = SupportClass.URShift(number4, bits2);
            num1 = num11 - bits2;
            int index2 = this.index;
            int table2 = this.table;
            if (index2 + num12 <= 258 + (table2 & 31 /*0x1F*/) + (table2 >> 5 & 31 /*0x1F*/) && (huft2 != 16 /*0x10*/ || index2 >= 1))
            {
              int blen = huft2 == 16 /*0x10*/ ? this.blens[index2 - 1] : 0;
              do
              {
                this.blens[index2++] = blen;
              }
              while (--num12 != 0);
              this.index = index2;
            }
            else
              goto label_68;
          }
        }
        else
          break;
      }
      this.tb[0] = -1;
      int[] bl2 = new int[1];
      int[] bd2 = new int[1];
      int[] tl2 = new int[1];
      int[] td2 = new int[1];
      bl2[0] = 9;
      bd2[0] = 6;
      int table = this.table;
      num4 = InfTree.inflate_trees_dynamic(257 + (table & 31 /*0x1F*/), 1 + (table >> 5 & 31 /*0x1F*/), this.blens, bl2, bd2, tl2, td2, this.hufts, z);
      switch (num4)
      {
        case -3:
          goto label_73;
        case 0:
          this.codes = new InfCodes(bl2[0], bd2[0], this.hufts, tl2[0], this.hufts, td2[0], z);
          this.blens = (int[]) null;
          this.mode = 6;
          break;
        default:
          goto label_74;
      }
label_76:
      this.bitb = number1;
      this.bitk = num1;
      z.avail_in = availIn;
      z.total_in += (long) (nextInIndex - z.next_in_index);
      z.next_in_index = nextInIndex;
      this.write = dstOffset;
      if ((r = this.codes.proc(this, z, r)) == 1)
      {
        r = 0;
        this.codes.free(z);
        nextInIndex = z.next_in_index;
        availIn = z.avail_in;
        number1 = this.bitb;
        num1 = this.bitk;
        dstOffset = this.write;
        num2 = dstOffset < this.read ? this.read - dstOffset - 1 : this.end - dstOffset;
        if (this.last == 0)
          this.mode = 0;
        else
          goto label_80;
      }
      else
        goto label_77;
    }
    this.mode = 9;
    z.msg = "too many length or distance symbols";
    r = -3;
    this.bitb = number1;
    this.bitk = num1;
    z.avail_in = availIn;
    z.total_in += (long) (nextInIndex - z.next_in_index);
    z.next_in_index = nextInIndex;
    this.write = dstOffset;
    return this.inflate_flush(z, r);
label_50:
    r = num3;
    if (r == -3)
    {
      this.blens = (int[]) null;
      this.mode = 9;
    }
    this.bitb = number1;
    this.bitk = num1;
    z.avail_in = availIn;
    z.total_in += (long) (nextInIndex - z.next_in_index);
    z.next_in_index = nextInIndex;
    this.write = dstOffset;
    return this.inflate_flush(z, r);
label_68:
    this.blens = (int[]) null;
    this.mode = 9;
    z.msg = "invalid bit length repeat";
    r = -3;
    this.bitb = number1;
    this.bitk = num1;
    z.avail_in = availIn;
    z.total_in += (long) (nextInIndex - z.next_in_index);
    z.next_in_index = nextInIndex;
    this.write = dstOffset;
    return this.inflate_flush(z, r);
label_73:
    this.blens = (int[]) null;
    this.mode = 9;
label_74:
    r = num4;
    this.bitb = number1;
    this.bitk = num1;
    z.avail_in = availIn;
    z.total_in += (long) (nextInIndex - z.next_in_index);
    z.next_in_index = nextInIndex;
    this.write = dstOffset;
    return this.inflate_flush(z, r);
label_77:
    return this.inflate_flush(z, r);
label_80:
    this.mode = 7;
label_81:
    this.write = dstOffset;
    r = this.inflate_flush(z, r);
    dstOffset = this.write;
    int num13 = dstOffset < this.read ? this.read - dstOffset - 1 : this.end - dstOffset;
    if (this.read != this.write)
    {
      this.bitb = number1;
      this.bitk = num1;
      z.avail_in = availIn;
      z.total_in += (long) (nextInIndex - z.next_in_index);
      z.next_in_index = nextInIndex;
      this.write = dstOffset;
      return this.inflate_flush(z, r);
    }
    this.mode = 8;
label_84:
    r = 1;
    this.bitb = number1;
    this.bitk = num1;
    z.avail_in = availIn;
    z.total_in += (long) (nextInIndex - z.next_in_index);
    z.next_in_index = nextInIndex;
    this.write = dstOffset;
    return this.inflate_flush(z, r);
label_85:
    r = -3;
    this.bitb = number1;
    this.bitk = num1;
    z.avail_in = availIn;
    z.total_in += (long) (nextInIndex - z.next_in_index);
    z.next_in_index = nextInIndex;
    this.write = dstOffset;
    return this.inflate_flush(z, r);
label_86:
    r = -2;
    this.bitb = number1;
    this.bitk = num1;
    z.avail_in = availIn;
    z.total_in += (long) (nextInIndex - z.next_in_index);
    z.next_in_index = nextInIndex;
    this.write = dstOffset;
    return this.inflate_flush(z, r);
  }

  internal void free(ZStream z)
  {
    this.reset(z, (long[]) null);
    this.window = (byte[]) null;
    this.hufts = (int[]) null;
  }

  internal void set_dictionary(byte[] d, int start, int n)
  {
    Buffer.BlockCopy((Array) d, start, (Array) this.window, 0, n);
    this.read = this.write = n;
  }

  internal int sync_point() => this.mode != 1 ? 0 : 1;

  internal int inflate_flush(ZStream z, int r)
  {
    int nextOutIndex = z.next_out_index;
    int read = this.read;
    int num1 = (read <= this.write ? this.write : this.end) - read;
    if (num1 > z.avail_out)
      num1 = z.avail_out;
    if (num1 != 0 && r == -5)
      r = 0;
    z.avail_out -= num1;
    z.total_out += (long) num1;
    if (this.checkfn != null)
      z.adler = this.check = z._adler.adler32(this.check, this.window, read, num1);
    Buffer.BlockCopy((Array) this.window, read, (Array) z.next_out, nextOutIndex, num1);
    int dstOffset = nextOutIndex + num1;
    int num2 = read + num1;
    if (num2 == this.end)
    {
      int num3 = 0;
      if (this.write == this.end)
        this.write = 0;
      int num4 = this.write - num3;
      if (num4 > z.avail_out)
        num4 = z.avail_out;
      if (num4 != 0 && r == -5)
        r = 0;
      z.avail_out -= num4;
      z.total_out += (long) num4;
      if (this.checkfn != null)
        z.adler = this.check = z._adler.adler32(this.check, this.window, num3, num4);
      Buffer.BlockCopy((Array) this.window, num3, (Array) z.next_out, dstOffset, num4);
      dstOffset += num4;
      num2 = num3 + num4;
    }
    z.next_out_index = dstOffset;
    this.read = num2;
    return r;
  }
}
