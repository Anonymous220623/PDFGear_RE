// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.LZWCodec
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class LZWCodec(Tiff tif, Syncfusion.Pdf.Compression.JBIG2.Compression scheme, string name) : 
  CodecWithPredictor(tif, scheme, name)
{
  private const short BITS_MIN = 9;
  private const short BITS_MAX = 12;
  private const short CODE_CLEAR = 256 /*0x0100*/;
  private const short CODE_EOI = 257;
  private const short CODE_FIRST = 258;
  private const short CODE_MAX = 4095 /*0x0FFF*/;
  private const short CODE_MIN = 511 /*0x01FF*/;
  private const int HSIZE = 9001;
  private const int HSHIFT = 5;
  private const int CSIZE = 5119;
  private const int CHECK_GAP = 10000;
  private bool LZW_CHECKEOS = true;
  private bool m_compatDecode;
  private short m_nbits;
  private short m_maxcode;
  private short m_free_ent;
  private int m_nextdata;
  private int m_nextbits;
  private int m_rw_mode;
  private int m_dec_nbitsmask;
  private int m_dec_restart;
  private int m_dec_bitsleft;
  private bool m_oldStyleCodeFound;
  private int m_dec_codep;
  private int m_dec_oldcodep;
  private int m_dec_free_entp;
  private int m_dec_maxcodep;
  private LZWCodec.code_t[] m_dec_codetab;
  private int m_enc_oldcode;
  private int m_enc_checkpoint;
  private int m_enc_ratio;
  private int m_enc_incount;
  private int m_enc_outcount;
  private int m_enc_rawlimit;
  private LZWCodec.hash_t[] m_enc_hashtab;

  public override bool Init()
  {
    this.m_dec_codetab = (LZWCodec.code_t[]) null;
    this.m_oldStyleCodeFound = false;
    this.m_enc_hashtab = (LZWCodec.hash_t[]) null;
    this.m_rw_mode = this.m_tif.m_mode;
    this.m_compatDecode = false;
    this.TIFFPredictorInit((TiffTagMethods) null);
    return true;
  }

  public override bool CanDecode => true;

  public override bool PreDecode(short plane) => this.LZWPreDecode(plane);

  public override void Cleanup()
  {
    this.LZWCleanup();
    this.m_tif.m_mode = this.m_rw_mode;
  }

  public override bool predictor_setupdecode() => this.LZWSetupDecode();

  public override bool predictor_decoderow(byte[] buffer, int offset, int count, short plane)
  {
    return this.m_compatDecode ? this.LZWDecodeCompat(buffer, offset, count, plane) : this.LZWDecode(buffer, offset, count, plane);
  }

  public override bool predictor_decodestrip(byte[] buffer, int offset, int count, short plane)
  {
    return this.m_compatDecode ? this.LZWDecodeCompat(buffer, offset, count, plane) : this.LZWDecode(buffer, offset, count, plane);
  }

  public override bool predictor_decodetile(byte[] buffer, int offset, int count, short plane)
  {
    return this.m_compatDecode ? this.LZWDecodeCompat(buffer, offset, count, plane) : this.LZWDecode(buffer, offset, count, plane);
  }

  private bool LZWSetupDecode()
  {
    if (this.m_dec_codetab == null)
    {
      this.m_dec_codetab = new LZWCodec.code_t[5119];
      int maxValue = (int) byte.MaxValue;
      do
      {
        this.m_dec_codetab[maxValue].value = (byte) maxValue;
        this.m_dec_codetab[maxValue].firstchar = (byte) maxValue;
        this.m_dec_codetab[maxValue].length = (short) 1;
        this.m_dec_codetab[maxValue].next = -1;
      }
      while (maxValue-- != 0);
      Array.Clear((Array) this.m_dec_codetab, 256 /*0x0100*/, 2);
    }
    return true;
  }

  private bool LZWPreDecode(short s)
  {
    if (this.m_dec_codetab == null)
      this.SetupDecode();
    if (this.m_tif.m_rawdata[0] == (byte) 0 && ((int) this.m_tif.m_rawdata[1] & 1) != 0)
    {
      if (!this.m_oldStyleCodeFound)
      {
        this.m_compatDecode = true;
        this.SetupDecode();
        this.m_oldStyleCodeFound = true;
      }
      this.m_maxcode = (short) 511 /*0x01FF*/;
    }
    else
    {
      this.m_maxcode = (short) 510;
      this.m_oldStyleCodeFound = false;
    }
    this.m_nbits = (short) 9;
    this.m_nextbits = 0;
    this.m_nextdata = 0;
    this.m_dec_restart = 0;
    this.m_dec_nbitsmask = 511 /*0x01FF*/;
    this.m_dec_bitsleft = this.m_tif.m_rawcc << 3;
    this.m_dec_free_entp = 258;
    Array.Clear((Array) this.m_dec_codetab, this.m_dec_free_entp, 4861);
    this.m_dec_oldcodep = -1;
    this.m_dec_maxcodep = this.m_dec_nbitsmask - 1;
    return true;
  }

  private bool LZWDecode(byte[] buffer, int offset, int count, short plane)
  {
    if (this.m_dec_restart != 0)
    {
      int index = this.m_dec_codep;
      int num1 = (int) this.m_dec_codetab[index].length - this.m_dec_restart;
      if (num1 > count)
      {
        this.m_dec_restart += count;
        do
        {
          index = this.m_dec_codetab[index].next;
        }
        while (--num1 > count && index != -1);
        if (index != -1)
        {
          int num2 = count;
          do
          {
            --num2;
            buffer[offset + num2] = this.m_dec_codetab[index].value;
            index = this.m_dec_codetab[index].next;
          }
          while (--count != 0 && index != -1);
        }
        return true;
      }
      offset += num1;
      count -= num1;
      int num3 = 0;
      do
      {
        --num3;
        int num4 = (int) this.m_dec_codetab[index].value;
        index = this.m_dec_codetab[index].next;
        buffer[offset + num3] = (byte) num4;
      }
      while (--num1 != 0 && index != -1);
      this.m_dec_restart = 0;
    }
    while (count > 0)
    {
      short _code;
      this.NextCode(out _code, false);
      switch (_code)
      {
        case 256 /*0x0100*/:
          this.m_dec_free_entp = 258;
          Array.Clear((Array) this.m_dec_codetab, this.m_dec_free_entp, 4861);
          this.m_nbits = (short) 9;
          this.m_dec_nbitsmask = 511 /*0x01FF*/;
          this.m_dec_maxcodep = this.m_dec_nbitsmask - 1;
          this.NextCode(out _code, false);
          switch (_code)
          {
            case 256 /*0x0100*/:
              return false;
            case 257:
              goto label_38;
            default:
              buffer[offset] = (byte) _code;
              ++offset;
              --count;
              this.m_dec_oldcodep = (int) _code;
              continue;
          }
        case 257:
          goto label_38;
        default:
          int index = (int) _code;
          if (this.m_dec_free_entp < 0 || this.m_dec_free_entp >= 5119)
            return false;
          this.m_dec_codetab[this.m_dec_free_entp].next = this.m_dec_oldcodep;
          if (this.m_dec_codetab[this.m_dec_free_entp].next < 0 || this.m_dec_codetab[this.m_dec_free_entp].next >= 5119)
            return false;
          this.m_dec_codetab[this.m_dec_free_entp].firstchar = this.m_dec_codetab[this.m_dec_codetab[this.m_dec_free_entp].next].firstchar;
          this.m_dec_codetab[this.m_dec_free_entp].length = (short) ((int) this.m_dec_codetab[this.m_dec_codetab[this.m_dec_free_entp].next].length + 1);
          this.m_dec_codetab[this.m_dec_free_entp].value = index < this.m_dec_free_entp ? this.m_dec_codetab[index].firstchar : this.m_dec_codetab[this.m_dec_free_entp].firstchar;
          if (++this.m_dec_free_entp > this.m_dec_maxcodep)
          {
            if (++this.m_nbits > (short) 12)
              this.m_nbits = (short) 12;
            this.m_dec_nbitsmask = LZWCodec.MAXCODE((int) this.m_nbits);
            this.m_dec_maxcodep = this.m_dec_nbitsmask - 1;
          }
          this.m_dec_oldcodep = (int) _code;
          if (_code >= (short) 256 /*0x0100*/)
          {
            if (this.m_dec_codetab[index].length == (short) 0)
              return false;
            if ((int) this.m_dec_codetab[index].length > count)
            {
              this.m_dec_codep = (int) _code;
              do
              {
                index = this.m_dec_codetab[index].next;
              }
              while (index != -1 && (int) this.m_dec_codetab[index].length > count);
              if (index != -1)
              {
                this.m_dec_restart = count;
                int num = count;
                do
                {
                  --num;
                  buffer[offset + num] = this.m_dec_codetab[index].value;
                  index = this.m_dec_codetab[index].next;
                }
                while (--count != 0 && index != -1);
                goto label_38;
              }
              goto label_38;
            }
            int length = (int) this.m_dec_codetab[index].length;
            int num5 = length;
            do
            {
              --num5;
              int num6 = (int) this.m_dec_codetab[index].value;
              index = this.m_dec_codetab[index].next;
              buffer[offset + num5] = (byte) num6;
            }
            while (index != -1 && num5 > 0);
            if (index == -1)
            {
              offset += length;
              count -= length;
              continue;
            }
            goto label_38;
          }
          buffer[offset] = (byte) _code;
          ++offset;
          --count;
          continue;
      }
    }
label_38:
    return count <= 0;
  }

  private bool LZWDecodeCompat(byte[] buffer, int offset, int count, short plane)
  {
    if (this.m_dec_restart != 0)
    {
      int index = this.m_dec_codep;
      int num1 = (int) this.m_dec_codetab[index].length - this.m_dec_restart;
      if (num1 > count)
      {
        this.m_dec_restart += count;
        do
        {
          index = this.m_dec_codetab[index].next;
        }
        while (--num1 > count);
        int num2 = count;
        do
        {
          --num2;
          buffer[offset + num2] = this.m_dec_codetab[index].value;
          index = this.m_dec_codetab[index].next;
        }
        while (--count != 0);
        return true;
      }
      offset += num1;
      count -= num1;
      int num3 = 0;
      do
      {
        --num3;
        buffer[offset + num3] = this.m_dec_codetab[index].value;
        index = this.m_dec_codetab[index].next;
      }
      while (--num1 != 0);
      this.m_dec_restart = 0;
    }
    while (count > 0)
    {
      short _code;
      this.NextCode(out _code, true);
      switch (_code)
      {
        case 256 /*0x0100*/:
          this.m_dec_free_entp = 258;
          Array.Clear((Array) this.m_dec_codetab, this.m_dec_free_entp, 4861);
          this.m_nbits = (short) 9;
          this.m_dec_nbitsmask = 511 /*0x01FF*/;
          this.m_dec_maxcodep = this.m_dec_nbitsmask;
          this.NextCode(out _code, true);
          switch (_code)
          {
            case 256 /*0x0100*/:
              return false;
            case 257:
              goto label_34;
            default:
              buffer[offset] = (byte) _code;
              ++offset;
              --count;
              this.m_dec_oldcodep = (int) _code;
              continue;
          }
        case 257:
          goto label_34;
        default:
          int index1 = (int) _code;
          if (this.m_dec_free_entp < 0 || this.m_dec_free_entp >= 5119)
            return false;
          this.m_dec_codetab[this.m_dec_free_entp].next = this.m_dec_oldcodep;
          if (this.m_dec_codetab[this.m_dec_free_entp].next < 0 || this.m_dec_codetab[this.m_dec_free_entp].next >= 5119)
            return false;
          this.m_dec_codetab[this.m_dec_free_entp].firstchar = this.m_dec_codetab[this.m_dec_codetab[this.m_dec_free_entp].next].firstchar;
          this.m_dec_codetab[this.m_dec_free_entp].length = (short) ((int) this.m_dec_codetab[this.m_dec_codetab[this.m_dec_free_entp].next].length + 1);
          this.m_dec_codetab[this.m_dec_free_entp].value = index1 < this.m_dec_free_entp ? this.m_dec_codetab[index1].firstchar : this.m_dec_codetab[this.m_dec_free_entp].firstchar;
          if (++this.m_dec_free_entp > this.m_dec_maxcodep)
          {
            if (++this.m_nbits > (short) 12)
              this.m_nbits = (short) 12;
            this.m_dec_nbitsmask = LZWCodec.MAXCODE((int) this.m_nbits);
            this.m_dec_maxcodep = this.m_dec_nbitsmask;
          }
          this.m_dec_oldcodep = (int) _code;
          if (_code >= (short) 256 /*0x0100*/)
          {
            int num4 = offset;
            if (this.m_dec_codetab[index1].length == (short) 0)
              return false;
            if ((int) this.m_dec_codetab[index1].length > count)
            {
              this.m_dec_codep = (int) _code;
              do
              {
                index1 = this.m_dec_codetab[index1].next;
              }
              while ((int) this.m_dec_codetab[index1].length > count);
              this.m_dec_restart = count;
              int num5 = count;
              do
              {
                --num5;
                buffer[offset + num5] = this.m_dec_codetab[index1].value;
                index1 = this.m_dec_codetab[index1].next;
              }
              while (--count != 0);
              goto label_34;
            }
            offset += (int) this.m_dec_codetab[index1].length;
            count -= (int) this.m_dec_codetab[index1].length;
            int index2 = offset;
            do
            {
              --index2;
              buffer[index2] = this.m_dec_codetab[index1].value;
              index1 = this.m_dec_codetab[index1].next;
            }
            while (index1 != -1 && index2 > num4);
            continue;
          }
          buffer[offset] = (byte) _code;
          ++offset;
          --count;
          continue;
      }
    }
label_34:
    return count <= 0;
  }

  private void LZWCleanup()
  {
    this.m_dec_codetab = (LZWCodec.code_t[]) null;
    this.m_enc_hashtab = (LZWCodec.hash_t[]) null;
  }

  private static int MAXCODE(int n) => (1 << n) - 1;

  private void NextCode(out short _code, bool compat)
  {
    if (this.LZW_CHECKEOS)
    {
      if (this.m_dec_bitsleft < (int) this.m_nbits)
      {
        _code = (short) 257;
      }
      else
      {
        if (compat)
          this.GetNextCodeCompat(out _code);
        else
          this.GetNextCode(out _code);
        this.m_dec_bitsleft -= (int) this.m_nbits;
      }
    }
    else if (compat)
      this.GetNextCodeCompat(out _code);
    else
      this.GetNextCode(out _code);
  }

  private void GetNextCode(out short code)
  {
    this.m_nextdata = this.m_nextdata << 8 | (int) this.m_tif.m_rawdata[this.m_tif.m_rawcp];
    ++this.m_tif.m_rawcp;
    this.m_nextbits += 8;
    if (this.m_nextbits < (int) this.m_nbits)
    {
      this.m_nextdata = this.m_nextdata << 8 | (int) this.m_tif.m_rawdata[this.m_tif.m_rawcp];
      ++this.m_tif.m_rawcp;
      this.m_nextbits += 8;
    }
    code = (short) (this.m_nextdata >> this.m_nextbits - (int) this.m_nbits & this.m_dec_nbitsmask);
    this.m_nextbits -= (int) this.m_nbits;
  }

  private void GetNextCodeCompat(out short code)
  {
    this.m_nextdata |= (int) this.m_tif.m_rawdata[this.m_tif.m_rawcp] << this.m_nextbits;
    ++this.m_tif.m_rawcp;
    this.m_nextbits += 8;
    if (this.m_nextbits < (int) this.m_nbits)
    {
      this.m_nextdata |= (int) this.m_tif.m_rawdata[this.m_tif.m_rawcp] << this.m_nextbits;
      ++this.m_tif.m_rawcp;
      this.m_nextbits += 8;
    }
    code = (short) (this.m_nextdata & this.m_dec_nbitsmask);
    this.m_nextdata >>= (int) this.m_nbits;
    this.m_nextbits -= (int) this.m_nbits;
  }

  private struct code_t
  {
    public int next;
    public short length;
    public byte value;
    public byte firstchar;
  }

  private struct hash_t
  {
    public int hash;
    public short code;
  }
}
