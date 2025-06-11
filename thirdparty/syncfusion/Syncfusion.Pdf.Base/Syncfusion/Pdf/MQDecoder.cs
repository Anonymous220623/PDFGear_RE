// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.MQDecoder
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal class MQDecoder
{
  internal static readonly uint[] qe = new uint[47]
  {
    22017U,
    13313U,
    6145U,
    2753U,
    1313U,
    545U,
    22017U,
    21505U,
    18433U,
    14337U,
    12289U,
    9217U,
    7169U,
    5633U,
    22017U,
    21505U,
    20737U,
    18433U,
    14337U,
    13313U,
    12289U,
    10241U,
    9217U,
    8705U,
    7169U,
    6145U,
    5633U,
    5121U,
    4609U,
    4353U,
    2753U,
    2497U,
    2209U,
    1313U,
    1089U,
    673U,
    545U,
    321U,
    273U,
    133U,
    73U,
    37U,
    21U,
    9U,
    5U,
    1U,
    22017U
  };
  internal static readonly int[] nMPS = new int[47]
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
  internal static readonly int[] nLPS = new int[47]
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
  internal static readonly int[] switchLM = new int[47]
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
  internal ByteInputBuffer in_Renamed;
  internal int[] mPS;
  internal int[] I;
  internal uint c;
  internal uint cT;
  internal uint a;
  internal uint b;
  internal bool markerFound;
  internal int[] initStates;

  public virtual int NumCtxts => this.I.Length;

  public virtual ByteInputBuffer ByteInputBuffer => this.in_Renamed;

  public MQDecoder(ByteInputBuffer iStream, int nrOfContexts, int[] initStates)
  {
    this.in_Renamed = iStream;
    this.I = new int[nrOfContexts];
    this.mPS = new int[nrOfContexts];
    this.initStates = initStates;
    this.init();
    this.resetCtxts();
  }

  internal bool fastDecodeSymbols(int[] bits, int ctxt, uint n)
  {
    int index1 = this.I[ctxt];
    uint num1 = MQDecoder.qe[index1];
    if (num1 < 16384U /*0x4000*/ && n <= (uint) ((int) this.a - (int) (this.c >> 16 /*0x10*/) - 1) / num1 && n <= (this.a - 32768U /*0x8000*/) / num1 + 1U)
    {
      this.a -= n * num1;
      if (this.a >= 32768U /*0x8000*/)
      {
        bits[0] = this.mPS[ctxt];
        return true;
      }
      this.I[ctxt] = MQDecoder.nMPS[index1];
      if (this.cT == 0U)
        this.byteIn();
      this.a <<= 1;
      this.c <<= 1;
      --this.cT;
      bits[0] = this.mPS[ctxt];
      return true;
    }
    uint num2 = this.a;
    for (int index2 = 0; (long) index2 < (long) n; ++index2)
    {
      num2 -= num1;
      if (this.c >> 16 /*0x10*/ < num2)
      {
        if (num2 >= 32768U /*0x8000*/)
          bits[index2] = this.mPS[ctxt];
        else if (num2 >= num1)
        {
          bits[index2] = this.mPS[ctxt];
          index1 = MQDecoder.nMPS[index1];
          num1 = MQDecoder.qe[index1];
          if (this.cT == 0U)
            this.byteIn();
          num2 <<= 1;
          this.c <<= 1;
          --this.cT;
        }
        else
        {
          bits[index2] = 1 - this.mPS[ctxt];
          if (MQDecoder.switchLM[index1] == 1)
            this.mPS[ctxt] = 1 - this.mPS[ctxt];
          index1 = MQDecoder.nLPS[index1];
          num1 = MQDecoder.qe[index1];
          do
          {
            if (this.cT == 0U)
              this.byteIn();
            num2 <<= 1;
            this.c <<= 1;
            --this.cT;
          }
          while (num2 < 32768U /*0x8000*/);
        }
      }
      else
      {
        this.c -= num2 << 16 /*0x10*/;
        if (num2 < num1)
        {
          uint num3 = num1;
          bits[index2] = this.mPS[ctxt];
          index1 = MQDecoder.nMPS[index1];
          num1 = MQDecoder.qe[index1];
          if (this.cT == 0U)
            this.byteIn();
          num2 = num3 << 1;
          this.c <<= 1;
          --this.cT;
        }
        else
        {
          num2 = num1;
          bits[index2] = 1 - this.mPS[ctxt];
          if (MQDecoder.switchLM[index1] == 1)
            this.mPS[ctxt] = 1 - this.mPS[ctxt];
          index1 = MQDecoder.nLPS[index1];
          num1 = MQDecoder.qe[index1];
          do
          {
            if (this.cT == 0U)
              this.byteIn();
            num2 <<= 1;
            this.c <<= 1;
            --this.cT;
          }
          while (num2 < 32768U /*0x8000*/);
        }
      }
    }
    this.a = num2;
    this.I[ctxt] = index1;
    return false;
  }

  public void decodeSymbols(int[] bits, int[] cX, int n)
  {
    for (int index1 = 0; index1 < n; ++index1)
    {
      int index2 = cX[index1];
      int index3 = this.I[index2];
      uint num1 = MQDecoder.qe[index3];
      this.a -= num1;
      if (this.c >> 16 /*0x10*/ < this.a)
      {
        if (this.a >= 32768U /*0x8000*/)
        {
          bits[index1] = this.mPS[index2];
        }
        else
        {
          uint a = this.a;
          if (a >= num1)
          {
            bits[index1] = this.mPS[index2];
            this.I[index2] = MQDecoder.nMPS[index3];
            if (this.cT == 0U)
              this.byteIn();
            a <<= 1;
            this.c <<= 1;
            --this.cT;
          }
          else
          {
            bits[index1] = 1 - this.mPS[index2];
            if (MQDecoder.switchLM[index3] == 1)
              this.mPS[index2] = 1 - this.mPS[index2];
            this.I[index2] = MQDecoder.nLPS[index3];
            do
            {
              if (this.cT == 0U)
                this.byteIn();
              a <<= 1;
              this.c <<= 1;
              --this.cT;
            }
            while (a < 32768U /*0x8000*/);
          }
          this.a = a;
        }
      }
      else
      {
        uint a = this.a;
        this.c -= a << 16 /*0x10*/;
        uint num2;
        if (a < num1)
        {
          uint num3 = num1;
          bits[index1] = this.mPS[index2];
          this.I[index2] = MQDecoder.nMPS[index3];
          if (this.cT == 0U)
            this.byteIn();
          num2 = num3 << 1;
          this.c <<= 1;
          --this.cT;
        }
        else
        {
          num2 = num1;
          bits[index1] = 1 - this.mPS[index2];
          if (MQDecoder.switchLM[index3] == 1)
            this.mPS[index2] = 1 - this.mPS[index2];
          this.I[index2] = MQDecoder.nLPS[index3];
          do
          {
            if (this.cT == 0U)
              this.byteIn();
            num2 <<= 1;
            this.c <<= 1;
            --this.cT;
          }
          while (num2 < 32768U /*0x8000*/);
        }
        this.a = num2;
      }
    }
  }

  public int decodeSymbol(int context)
  {
    int index = this.I[context];
    uint num1 = MQDecoder.qe[index];
    this.a -= num1;
    int num2;
    if (this.c >> 16 /*0x10*/ < this.a)
    {
      if (this.a >= 32768U /*0x8000*/)
      {
        num2 = this.mPS[context];
      }
      else
      {
        uint a = this.a;
        if (a >= num1)
        {
          num2 = this.mPS[context];
          this.I[context] = MQDecoder.nMPS[index];
          if (this.cT == 0U)
            this.byteIn();
          a <<= 1;
          this.c <<= 1;
          --this.cT;
        }
        else
        {
          num2 = 1 - this.mPS[context];
          if (MQDecoder.switchLM[index] == 1)
            this.mPS[context] = 1 - this.mPS[context];
          this.I[context] = MQDecoder.nLPS[index];
          do
          {
            if (this.cT == 0U)
              this.byteIn();
            a <<= 1;
            this.c <<= 1;
            --this.cT;
          }
          while (a < 32768U /*0x8000*/);
        }
        this.a = a;
      }
    }
    else
    {
      uint a = this.a;
      this.c -= a << 16 /*0x10*/;
      uint num3;
      if (a < num1)
      {
        uint num4 = num1;
        num2 = this.mPS[context];
        this.I[context] = MQDecoder.nMPS[index];
        if (this.cT == 0U)
          this.byteIn();
        num3 = num4 << 1;
        this.c <<= 1;
        --this.cT;
      }
      else
      {
        num3 = num1;
        num2 = 1 - this.mPS[context];
        if (MQDecoder.switchLM[index] == 1)
          this.mPS[context] = 1 - this.mPS[context];
        this.I[context] = MQDecoder.nLPS[index];
        do
        {
          if (this.cT == 0U)
            this.byteIn();
          num3 <<= 1;
          this.c <<= 1;
          --this.cT;
        }
        while (num3 < 32768U /*0x8000*/);
      }
      this.a = num3;
    }
    return num2;
  }

  public virtual bool checkPredTerm()
  {
    if (this.b != (uint) byte.MaxValue && !this.markerFound || this.cT != 0U && !this.markerFound)
      return true;
    if (this.cT == 1U)
      return false;
    if (this.cT == 0U)
    {
      if (!this.markerFound)
      {
        this.b = (uint) (this.in_Renamed.read() & (int) byte.MaxValue);
        if (this.b <= 143U)
          return true;
      }
      this.cT = 8U;
    }
    uint num = 32768U /*0x8000*/ >> (int) this.cT - 1;
    this.a -= num;
    if (this.c >> 16 /*0x10*/ < this.a)
      return true;
    this.c -= this.a << 16 /*0x10*/;
    this.a = num;
    do
    {
      if (this.cT == 0U)
        this.byteIn();
      this.a <<= 1;
      this.c <<= 1;
      --this.cT;
    }
    while (this.a < 32768U /*0x8000*/);
    return false;
  }

  private void byteIn()
  {
    if (!this.markerFound)
    {
      if (this.b == (uint) byte.MaxValue)
      {
        this.b = (uint) (this.in_Renamed.read() & (int) byte.MaxValue);
        if (this.b > 143U)
        {
          this.markerFound = true;
          this.cT = 8U;
        }
        else
        {
          this.c += (uint) (65024 - ((int) this.b << 9));
          this.cT = 7U;
        }
      }
      else
      {
        this.b = (uint) (this.in_Renamed.read() & (int) byte.MaxValue);
        this.c += (uint) (65280 - ((int) this.b << 8));
        this.cT = 8U;
      }
    }
    else
      this.cT = 8U;
  }

  public void resetCtxt(int c)
  {
    this.I[c] = this.initStates[c];
    this.mPS[c] = 0;
  }

  public void resetCtxts()
  {
    Array.Copy((Array) this.initStates, 0, (Array) this.I, 0, this.I.Length);
    ArrayUtil.intArraySet(this.mPS, 0);
  }

  public void nextSegment(byte[] buf, int off, int len)
  {
    this.in_Renamed.setByteArray(buf, off, len);
    this.init();
  }

  private void init()
  {
    this.markerFound = false;
    this.b = (uint) (this.in_Renamed.read() & (int) byte.MaxValue);
    this.c = (uint) (((int) this.b ^ (int) byte.MaxValue) << 16 /*0x10*/);
    this.byteIn();
    this.c <<= 7;
    this.cT -= 7U;
    this.a = 32768U /*0x8000*/;
  }
}
