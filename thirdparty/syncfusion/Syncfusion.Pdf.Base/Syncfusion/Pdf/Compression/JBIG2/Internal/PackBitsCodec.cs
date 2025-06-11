// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.PackBitsCodec
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class PackBitsCodec(Tiff tif, Syncfusion.Pdf.Compression.JBIG2.Compression scheme, string name) : 
  TiffCodec(tif, scheme, name)
{
  private int m_rowsize;

  public override bool Init() => true;

  public override bool CanDecode => true;

  public override bool DecodeRow(byte[] buffer, int offset, int count, short plane)
  {
    return this.PackBitsDecode(buffer, offset, count, plane);
  }

  public override bool DecodeStrip(byte[] buffer, int offset, int count, short plane)
  {
    return this.PackBitsDecode(buffer, offset, count, plane);
  }

  public override bool DecodeTile(byte[] buffer, int offset, int count, short plane)
  {
    return this.PackBitsDecode(buffer, offset, count, plane);
  }

  private bool PackBitsDecode(byte[] buffer, int offset, int count, short plane)
  {
    int rawcp = this.m_tif.m_rawcp;
    int rawcc = this.m_tif.m_rawcc;
    while (rawcc > 0 && count > 0)
    {
      int num1 = (int) this.m_tif.m_rawdata[rawcp];
      ++rawcp;
      --rawcc;
      if (num1 >= 128 /*0x80*/)
        num1 -= 256 /*0x0100*/;
      if (num1 < 0)
      {
        if (num1 != (int) sbyte.MinValue)
        {
          int num2 = -num1 + 1;
          if (count < num2)
            num2 = count;
          count -= num2;
          int num3 = (int) this.m_tif.m_rawdata[rawcp];
          ++rawcp;
          --rawcc;
          while (num2-- > 0)
          {
            buffer[offset] = (byte) num3;
            ++offset;
          }
        }
      }
      else
      {
        if (count < num1 + 1)
          num1 = count - 1;
        int num4;
        Buffer.BlockCopy((Array) this.m_tif.m_rawdata, rawcp, (Array) buffer, offset, num4 = num1 + 1);
        offset += num4;
        count -= num4;
        rawcp += num4;
        rawcc -= num4;
      }
    }
    this.m_tif.m_rawcp = rawcp;
    this.m_tif.m_rawcc = rawcc;
    return count <= 0;
  }

  private enum EncodingState
  {
    BASE,
    LITERAL,
    RUN,
    LITERAL_RUN,
  }
}
