// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.ZLib.InfCodes
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.ZLib;

internal sealed class InfCodes
{
  private const int Z_OK = 0;
  private const int Z_STREAM_END = 1;
  private const int Z_NEED_DICT = 2;
  private const int Z_ERRNO = -1;
  private const int Z_STREAM_ERROR = -2;
  private const int Z_DATA_ERROR = -3;
  private const int Z_MEM_ERROR = -4;
  private const int Z_BUF_ERROR = -5;
  private const int Z_VERSION_ERROR = -6;
  private const int START = 0;
  private const int LEN = 1;
  private const int LENEXT = 2;
  private const int DIST = 3;
  private const int DISTEXT = 4;
  private const int COPY = 5;
  private const int LIT = 6;
  private const int WASH = 7;
  private const int END = 8;
  private const int BADCODE = 9;
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
  internal int mode;
  internal int len;
  internal int[] tree;
  internal int tree_index;
  internal int need;
  internal int lit;
  internal int get_Renamed;
  internal int dist;
  internal byte lbits;
  internal byte dbits;
  internal int[] ltree;
  internal int ltree_index;
  internal int[] dtree;
  internal int dtree_index;

  internal InfCodes(int bl, int bd, int[] tl, int tl_index, int[] td, int td_index, ZStream z)
  {
    this.mode = 0;
    this.lbits = (byte) bl;
    this.dbits = (byte) bd;
    this.ltree = tl;
    this.ltree_index = tl_index;
    this.dtree = td;
    this.dtree_index = td_index;
  }

  internal InfCodes(int bl, int bd, int[] tl, int[] td, ZStream z)
  {
    this.mode = 0;
    this.lbits = (byte) bl;
    this.dbits = (byte) bd;
    this.ltree = tl;
    this.ltree_index = 0;
    this.dtree = td;
    this.dtree_index = 0;
  }

  internal int proc(InfBlocks s, ZStream z, int r)
  {
    int nextInIndex = z.next_in_index;
    int availIn = z.avail_in;
    int number = s.bitb;
    int bitk = s.bitk;
    int num1 = s.write;
    int num2 = num1 < s.read ? s.read - num1 - 1 : s.end - num1;
    while (true)
    {
      switch (this.mode)
      {
        case 0:
          if (num2 >= 258 && availIn >= 10)
          {
            s.bitb = number;
            s.bitk = bitk;
            z.avail_in = availIn;
            z.total_in += (long) (nextInIndex - z.next_in_index);
            z.next_in_index = nextInIndex;
            s.write = num1;
            r = this.inflate_fast((int) this.lbits, (int) this.dbits, this.ltree, this.ltree_index, this.dtree, this.dtree_index, s, z);
            nextInIndex = z.next_in_index;
            availIn = z.avail_in;
            number = s.bitb;
            bitk = s.bitk;
            num1 = s.write;
            num2 = num1 < s.read ? s.read - num1 - 1 : s.end - num1;
            int num3;
            switch (r)
            {
              case 0:
                goto label_7;
              case 1:
                num3 = 7;
                break;
              default:
                num3 = 9;
                break;
            }
            this.mode = num3;
            continue;
          }
label_7:
          this.need = (int) this.lbits;
          this.tree = this.ltree;
          this.tree_index = this.ltree_index;
          this.mode = 1;
          goto case 1;
        case 1:
          int need1;
          for (need1 = this.need; bitk < need1; bitk += 8)
          {
            if (availIn != 0)
            {
              r = 0;
              --availIn;
              number |= ((int) z.next_in[nextInIndex++] & (int) byte.MaxValue) << bitk;
            }
            else
            {
              s.bitb = number;
              s.bitk = bitk;
              z.avail_in = availIn;
              z.total_in += (long) (nextInIndex - z.next_in_index);
              z.next_in_index = nextInIndex;
              s.write = num1;
              return s.inflate_flush(z, r);
            }
          }
          int index1 = (this.tree_index + (number & InfCodes.inflate_mask[need1])) * 3;
          number = SupportClass.URShift(number, this.tree[index1 + 1]);
          bitk -= this.tree[index1 + 1];
          int num4 = this.tree[index1];
          if (num4 == 0)
          {
            this.lit = this.tree[index1 + 2];
            this.mode = 6;
            continue;
          }
          if ((num4 & 16 /*0x10*/) != 0)
          {
            this.get_Renamed = num4 & 15;
            this.len = this.tree[index1 + 2];
            this.mode = 2;
            continue;
          }
          if ((num4 & 64 /*0x40*/) == 0)
          {
            this.need = num4;
            this.tree_index = index1 / 3 + this.tree[index1 + 2];
            continue;
          }
          if ((num4 & 32 /*0x20*/) != 0)
          {
            this.mode = 7;
            continue;
          }
          goto label_21;
        case 2:
          int getRenamed1;
          for (getRenamed1 = this.get_Renamed; bitk < getRenamed1; bitk += 8)
          {
            if (availIn != 0)
            {
              r = 0;
              --availIn;
              number |= ((int) z.next_in[nextInIndex++] & (int) byte.MaxValue) << bitk;
            }
            else
            {
              s.bitb = number;
              s.bitk = bitk;
              z.avail_in = availIn;
              z.total_in += (long) (nextInIndex - z.next_in_index);
              z.next_in_index = nextInIndex;
              s.write = num1;
              return s.inflate_flush(z, r);
            }
          }
          this.len += number & InfCodes.inflate_mask[getRenamed1];
          number >>= getRenamed1;
          bitk -= getRenamed1;
          this.need = (int) this.dbits;
          this.tree = this.dtree;
          this.tree_index = this.dtree_index;
          this.mode = 3;
          goto case 3;
        case 3:
          int need2;
          for (need2 = this.need; bitk < need2; bitk += 8)
          {
            if (availIn != 0)
            {
              r = 0;
              --availIn;
              number |= ((int) z.next_in[nextInIndex++] & (int) byte.MaxValue) << bitk;
            }
            else
            {
              s.bitb = number;
              s.bitk = bitk;
              z.avail_in = availIn;
              z.total_in += (long) (nextInIndex - z.next_in_index);
              z.next_in_index = nextInIndex;
              s.write = num1;
              return s.inflate_flush(z, r);
            }
          }
          int index2 = (this.tree_index + (number & InfCodes.inflate_mask[need2])) * 3;
          number >>= this.tree[index2 + 1];
          bitk -= this.tree[index2 + 1];
          int num5 = this.tree[index2];
          if ((num5 & 16 /*0x10*/) != 0)
          {
            this.get_Renamed = num5 & 15;
            this.dist = this.tree[index2 + 2];
            this.mode = 4;
            continue;
          }
          if ((num5 & 64 /*0x40*/) == 0)
          {
            this.need = num5;
            this.tree_index = index2 / 3 + this.tree[index2 + 2];
            continue;
          }
          goto label_37;
        case 4:
          int getRenamed2;
          for (getRenamed2 = this.get_Renamed; bitk < getRenamed2; bitk += 8)
          {
            if (availIn != 0)
            {
              r = 0;
              --availIn;
              number |= ((int) z.next_in[nextInIndex++] & (int) byte.MaxValue) << bitk;
            }
            else
            {
              s.bitb = number;
              s.bitk = bitk;
              z.avail_in = availIn;
              z.total_in += (long) (nextInIndex - z.next_in_index);
              z.next_in_index = nextInIndex;
              s.write = num1;
              return s.inflate_flush(z, r);
            }
          }
          this.dist += number & InfCodes.inflate_mask[getRenamed2];
          number >>= getRenamed2;
          bitk -= getRenamed2;
          this.mode = 5;
          goto case 5;
        case 5:
          int num6 = num1 - this.dist;
          while (num6 < 0)
            num6 += s.end;
          for (; this.len != 0; --this.len)
          {
            if (num2 == 0)
            {
              if (num1 == s.end && s.read != 0)
              {
                num1 = 0;
                num2 = num1 < s.read ? s.read - num1 - 1 : s.end - num1;
              }
              if (num2 == 0)
              {
                s.write = num1;
                r = s.inflate_flush(z, r);
                num1 = s.write;
                num2 = num1 < s.read ? s.read - num1 - 1 : s.end - num1;
                if (num1 == s.end && s.read != 0)
                {
                  num1 = 0;
                  num2 = num1 < s.read ? s.read - num1 - 1 : s.end - num1;
                }
                if (num2 == 0)
                {
                  s.bitb = number;
                  s.bitk = bitk;
                  z.avail_in = availIn;
                  z.total_in += (long) (nextInIndex - z.next_in_index);
                  z.next_in_index = nextInIndex;
                  s.write = num1;
                  return s.inflate_flush(z, r);
                }
              }
            }
            s.window[num1++] = s.window[num6++];
            --num2;
            if (num6 == s.end)
              num6 = 0;
          }
          this.mode = 0;
          continue;
        case 6:
          if (num2 == 0)
          {
            if (num1 == s.end && s.read != 0)
            {
              num1 = 0;
              num2 = num1 < s.read ? s.read - num1 - 1 : s.end - num1;
            }
            if (num2 == 0)
            {
              s.write = num1;
              r = s.inflate_flush(z, r);
              num1 = s.write;
              num2 = num1 < s.read ? s.read - num1 - 1 : s.end - num1;
              if (num1 == s.end && s.read != 0)
              {
                num1 = 0;
                num2 = num1 < s.read ? s.read - num1 - 1 : s.end - num1;
              }
              if (num2 == 0)
                goto label_67;
            }
          }
          r = 0;
          s.window[num1++] = (byte) this.lit;
          --num2;
          this.mode = 0;
          continue;
        case 7:
          goto label_69;
        case 8:
          goto label_74;
        case 9:
          goto label_75;
        default:
          goto label_76;
      }
    }
label_21:
    this.mode = 9;
    z.msg = "invalid literal/length code";
    r = -3;
    s.bitb = number;
    s.bitk = bitk;
    z.avail_in = availIn;
    z.total_in += (long) (nextInIndex - z.next_in_index);
    z.next_in_index = nextInIndex;
    s.write = num1;
    return s.inflate_flush(z, r);
label_37:
    this.mode = 9;
    z.msg = "invalid distance code";
    r = -3;
    s.bitb = number;
    s.bitk = bitk;
    z.avail_in = availIn;
    z.total_in += (long) (nextInIndex - z.next_in_index);
    z.next_in_index = nextInIndex;
    s.write = num1;
    return s.inflate_flush(z, r);
label_67:
    s.bitb = number;
    s.bitk = bitk;
    z.avail_in = availIn;
    z.total_in += (long) (nextInIndex - z.next_in_index);
    z.next_in_index = nextInIndex;
    s.write = num1;
    return s.inflate_flush(z, r);
label_69:
    if (bitk > 7)
    {
      bitk -= 8;
      ++availIn;
      --nextInIndex;
    }
    s.write = num1;
    r = s.inflate_flush(z, r);
    num1 = s.write;
    int num7 = num1 < s.read ? s.read - num1 - 1 : s.end - num1;
    if (s.read != s.write)
    {
      s.bitb = number;
      s.bitk = bitk;
      z.avail_in = availIn;
      z.total_in += (long) (nextInIndex - z.next_in_index);
      z.next_in_index = nextInIndex;
      s.write = num1;
      return s.inflate_flush(z, r);
    }
    this.mode = 8;
label_74:
    r = 1;
    s.bitb = number;
    s.bitk = bitk;
    z.avail_in = availIn;
    z.total_in += (long) (nextInIndex - z.next_in_index);
    z.next_in_index = nextInIndex;
    s.write = num1;
    return s.inflate_flush(z, r);
label_75:
    r = -3;
    s.bitb = number;
    s.bitk = bitk;
    z.avail_in = availIn;
    z.total_in += (long) (nextInIndex - z.next_in_index);
    z.next_in_index = nextInIndex;
    s.write = num1;
    return s.inflate_flush(z, r);
label_76:
    r = -2;
    s.bitb = number;
    s.bitk = bitk;
    z.avail_in = availIn;
    z.total_in += (long) (nextInIndex - z.next_in_index);
    z.next_in_index = nextInIndex;
    s.write = num1;
    return s.inflate_flush(z, r);
  }

  internal void free(ZStream z)
  {
  }

  internal int inflate_fast(
    int bl,
    int bd,
    int[] tl,
    int tl_index,
    int[] td,
    int td_index,
    InfBlocks s,
    ZStream z)
  {
    int nextInIndex = z.next_in_index;
    int availIn = z.avail_in;
    int num1 = s.bitb;
    int num2 = s.bitk;
    int dstOffset = s.write;
    int num3 = dstOffset < s.read ? s.read - dstOffset - 1 : s.end - dstOffset;
    int num4 = InfCodes.inflate_mask[bl];
    int num5 = InfCodes.inflate_mask[bd];
    do
    {
      for (; num2 < 20; num2 += 8)
      {
        --availIn;
        num1 |= ((int) z.next_in[nextInIndex++] & (int) byte.MaxValue) << num2;
      }
      int num6 = num1 & num4;
      int[] numArray1 = tl;
      int num7 = tl_index;
      int index1;
      if ((index1 = numArray1[(num7 + num6) * 3]) == 0)
      {
        num1 >>= numArray1[(num7 + num6) * 3 + 1];
        num2 -= numArray1[(num7 + num6) * 3 + 1];
        s.window[dstOffset++] = (byte) numArray1[(num7 + num6) * 3 + 2];
        --num3;
      }
      else
      {
        do
        {
          num1 >>= numArray1[(num7 + num6) * 3 + 1];
          num2 -= numArray1[(num7 + num6) * 3 + 1];
          if ((index1 & 16 /*0x10*/) != 0)
          {
            int index2 = index1 & 15;
            int count1 = numArray1[(num7 + num6) * 3 + 2] + (num1 & InfCodes.inflate_mask[index2]);
            int num8 = num1 >> index2;
            int num9;
            for (num9 = num2 - index2; num9 < 15; num9 += 8)
            {
              --availIn;
              num8 |= ((int) z.next_in[nextInIndex++] & (int) byte.MaxValue) << num9;
            }
            int num10 = num8 & num5;
            int[] numArray2 = td;
            int num11 = td_index;
            int index3 = numArray2[(num11 + num10) * 3];
            while (true)
            {
              num8 >>= numArray2[(num11 + num10) * 3 + 1];
              num9 -= numArray2[(num11 + num10) * 3 + 1];
              if ((index3 & 16 /*0x10*/) == 0)
              {
                if ((index3 & 64 /*0x40*/) == 0)
                {
                  num10 = num10 + numArray2[(num11 + num10) * 3 + 2] + (num8 & InfCodes.inflate_mask[index3]);
                  index3 = numArray2[(num11 + num10) * 3];
                }
                else
                  goto label_30;
              }
              else
                break;
            }
            int index4;
            for (index4 = index3 & 15; num9 < index4; num9 += 8)
            {
              --availIn;
              num8 |= ((int) z.next_in[nextInIndex++] & (int) byte.MaxValue) << num9;
            }
            int num12 = numArray2[(num11 + num10) * 3 + 2] + (num8 & InfCodes.inflate_mask[index4]);
            num1 = num8 >> index4;
            num2 = num9 - index4;
            num3 -= count1;
            int srcOffset1;
            int num13;
            if (dstOffset >= num12)
            {
              int srcOffset2 = dstOffset - num12;
              if (dstOffset - srcOffset2 > 0 && 2 > dstOffset - srcOffset2)
              {
                byte[] window1 = s.window;
                int index5 = dstOffset;
                int num14 = index5 + 1;
                byte[] window2 = s.window;
                int index6 = srcOffset2;
                int num15 = index6 + 1;
                int num16 = (int) window2[index6];
                window1[index5] = (byte) num16;
                int num17 = count1 - 1;
                byte[] window3 = s.window;
                int index7 = num14;
                dstOffset = index7 + 1;
                byte[] window4 = s.window;
                int index8 = num15;
                srcOffset1 = index8 + 1;
                int num18 = (int) window4[index8];
                window3[index7] = (byte) num18;
                count1 = num17 - 1;
              }
              else
              {
                Buffer.BlockCopy((Array) s.window, srcOffset2, (Array) s.window, dstOffset, 2);
                dstOffset += 2;
                srcOffset1 = srcOffset2 + 2;
                count1 -= 2;
              }
            }
            else
            {
              srcOffset1 = dstOffset - num12;
              do
              {
                srcOffset1 += s.end;
              }
              while (srcOffset1 < 0);
              int count2 = s.end - srcOffset1;
              if (count1 > count2)
              {
                count1 -= count2;
                if (dstOffset - srcOffset1 > 0 && count2 > dstOffset - srcOffset1)
                {
                  do
                  {
                    s.window[dstOffset++] = s.window[srcOffset1++];
                  }
                  while (--count2 != 0);
                }
                else
                {
                  Buffer.BlockCopy((Array) s.window, srcOffset1, (Array) s.window, dstOffset, count2);
                  dstOffset += count2;
                  num13 = srcOffset1 + count2;
                }
                srcOffset1 = 0;
              }
            }
            if (dstOffset - srcOffset1 > 0 && count1 > dstOffset - srcOffset1)
            {
              do
              {
                s.window[dstOffset++] = s.window[srcOffset1++];
              }
              while (--count1 != 0);
              goto label_37;
            }
            Buffer.BlockCopy((Array) s.window, srcOffset1, (Array) s.window, dstOffset, count1);
            dstOffset += count1;
            num13 = srcOffset1 + count1;
            goto label_37;
label_30:
            z.msg = "invalid distance code";
            int num19 = z.avail_in - availIn;
            int num20 = num9 >> 3 < num19 ? num9 >> 3 : num19;
            int num21 = availIn + num20;
            int num22 = nextInIndex - num20;
            int num23 = num9 - (num20 << 3);
            s.bitb = num8;
            s.bitk = num23;
            z.avail_in = num21;
            z.total_in += (long) (num22 - z.next_in_index);
            z.next_in_index = num22;
            s.write = dstOffset;
            return -3;
          }
          if ((index1 & 64 /*0x40*/) == 0)
            num6 = num6 + numArray1[(num7 + num6) * 3 + 2] + (num1 & InfCodes.inflate_mask[index1]);
          else
            goto label_34;
        }
        while ((index1 = numArray1[(num7 + num6) * 3]) != 0);
        num1 >>= numArray1[(num7 + num6) * 3 + 1];
        num2 -= numArray1[(num7 + num6) * 3 + 1];
        s.window[dstOffset++] = (byte) numArray1[(num7 + num6) * 3 + 2];
        --num3;
        goto label_37;
label_34:
        if ((index1 & 32 /*0x20*/) != 0)
        {
          int num24 = z.avail_in - availIn;
          int num25 = num2 >> 3 < num24 ? num2 >> 3 : num24;
          int num26 = availIn + num25;
          int num27 = nextInIndex - num25;
          int num28 = num2 - (num25 << 3);
          s.bitb = num1;
          s.bitk = num28;
          z.avail_in = num26;
          z.total_in += (long) (num27 - z.next_in_index);
          z.next_in_index = num27;
          s.write = dstOffset;
          return 1;
        }
        z.msg = "invalid literal/length code";
        int num29 = z.avail_in - availIn;
        int num30 = num2 >> 3 < num29 ? num2 >> 3 : num29;
        int num31 = availIn + num30;
        int num32 = nextInIndex - num30;
        int num33 = num2 - (num30 << 3);
        s.bitb = num1;
        s.bitk = num33;
        z.avail_in = num31;
        z.total_in += (long) (num32 - z.next_in_index);
        z.next_in_index = num32;
        s.write = dstOffset;
        return -3;
      }
label_37:;
    }
    while (num3 >= 258 && availIn >= 10);
    int num34 = z.avail_in - availIn;
    int num35 = num2 >> 3 < num34 ? num2 >> 3 : num34;
    int num36 = availIn + num35;
    int num37 = nextInIndex - num35;
    int num38 = num2 - (num35 << 3);
    s.bitb = num1;
    s.bitk = num38;
    z.avail_in = num36;
    z.total_in += (long) (num37 - z.next_in_index);
    z.next_in_index = num37;
    s.write = dstOffset;
    return 0;
  }
}
