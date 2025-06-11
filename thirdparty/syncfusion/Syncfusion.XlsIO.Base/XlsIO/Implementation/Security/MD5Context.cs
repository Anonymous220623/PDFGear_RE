// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Security.MD5Context
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Security;

[CLSCompliant(false)]
public class MD5Context
{
  private const uint DEF_MAGIC_1 = 1732584193;
  private const uint DEF_MAGIC_2 = 4023233417;
  private const uint DEF_MAGIC_3 = 2562383102;
  private const uint DEF_MAGIC_4 = 271733878;
  private static byte[] PADDING = new byte[64 /*0x40*/]
  {
    (byte) 128 /*0x80*/,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0
  };
  private uint[] m_uiI = new uint[2];
  private uint[] m_buf = new uint[4];
  private byte[] m_in = new byte[64 /*0x40*/];
  private byte[] m_digest = new byte[16 /*0x10*/];

  public uint[] I
  {
    get => this.m_uiI;
    set => this.m_uiI = value;
  }

  public uint[] Buffer
  {
    get => this.m_buf;
    set => this.m_buf = value;
  }

  public byte[] InBuffer
  {
    get => this.m_in;
    set => this.m_in = value;
  }

  public byte[] Digest
  {
    get => this.m_digest;
    set => this.m_digest = value;
  }

  public MD5Context()
  {
    this.m_uiI[0] = this.m_uiI[1] = 0U;
    this.m_buf[0] = 1732584193U;
    this.m_buf[1] = 4023233417U;
    this.m_buf[2] = 2562383102U;
    this.m_buf[3] = 271733878U;
  }

  public void Update(byte[] inBuf, uint inLen)
  {
    uint[] inn = new uint[16 /*0x10*/];
    int num = (int) (this.m_uiI[0] >> 3) & 63 /*0x3F*/;
    if (this.m_uiI[0] + (inLen << 3) < this.m_uiI[0])
      ++this.m_uiI[1];
    this.m_uiI[0] += inLen << 3;
    this.m_uiI[1] += inLen >> 29;
    for (uint index1 = 0; index1 < inLen; ++index1)
    {
      this.m_in[num++] = inBuf[(IntPtr) index1];
      if (num == 64 /*0x40*/)
      {
        uint index2 = 0;
        uint index3 = 0;
        while (index2 < 16U /*0x10*/)
        {
          inn[(IntPtr) index2] = (uint) ((int) this.m_in[(IntPtr) (index3 + 3U)] << 24 | (int) this.m_in[(IntPtr) (index3 + 2U)] << 16 /*0x10*/ | (int) this.m_in[(IntPtr) (index3 + 1U)] << 8) | (uint) this.m_in[(IntPtr) index3];
          ++index2;
          index3 += 4U;
        }
        this.Transform(inn);
        num = 0;
      }
    }
  }

  public void Final()
  {
    uint[] inn = new uint[16 /*0x10*/];
    inn[14] = this.m_uiI[0];
    inn[15] = this.m_uiI[1];
    uint num = this.m_uiI[0] >> 3 & 63U /*0x3F*/;
    uint inLen = num < 56U ? 56U - num : 120U - num;
    this.Update(MD5Context.PADDING, inLen);
    uint index1 = 0;
    uint index2 = 0;
    while (index1 < 14U)
    {
      inn[(IntPtr) index1] = (uint) ((int) this.m_in[(IntPtr) (index2 + 3U)] << 24 | (int) this.m_in[(IntPtr) (index2 + 2U)] << 16 /*0x10*/ | (int) this.m_in[(IntPtr) (index2 + 1U)] << 8) | (uint) this.m_in[(IntPtr) index2];
      ++index1;
      index2 += 4U;
    }
    this.Transform(inn);
    this.StoreDigest();
  }

  public void StoreDigest()
  {
    uint index1 = 0;
    uint index2 = 0;
    while (index1 < 4U)
    {
      this.m_digest[(IntPtr) index2] = (byte) (this.m_buf[(IntPtr) index1] & (uint) byte.MaxValue);
      this.m_digest[(IntPtr) (index2 + 1U)] = (byte) (this.m_buf[(IntPtr) index1] >> 8 & (uint) byte.MaxValue);
      this.m_digest[(IntPtr) (index2 + 2U)] = (byte) (this.m_buf[(IntPtr) index1] >> 16 /*0x10*/ & (uint) byte.MaxValue);
      this.m_digest[(IntPtr) (index2 + 3U)] = (byte) (this.m_buf[(IntPtr) index1] >> 24 & (uint) byte.MaxValue);
      ++index1;
      index2 += 4U;
    }
  }

  private uint F(uint x, uint y, uint z) => (uint) ((int) x & (int) y | ~(int) x & (int) z);

  private uint G(uint x, uint y, uint z) => (uint) ((int) x & (int) z | (int) y & ~(int) z);

  private uint H(uint x, uint y, uint z) => x ^ y ^ z;

  private uint III(uint x, uint y, uint z) => y ^ (x | ~z);

  private uint ROTATE_LEFT(uint x, byte n) => x << (int) n | x >> 32 /*0x20*/ - (int) n;

  private void FF(ref uint a, uint b, uint c, uint d, uint x, byte s, uint ac)
  {
    a += this.F(b, c, d) + x + ac;
    a = this.ROTATE_LEFT(a, s);
    a += b;
  }

  private void GG(ref uint a, uint b, uint c, uint d, uint x, byte s, uint ac)
  {
    a += this.G(b, c, d) + x + ac;
    a = this.ROTATE_LEFT(a, s);
    a += b;
  }

  private void HH(ref uint a, uint b, uint c, uint d, uint x, byte s, uint ac)
  {
    a += this.H(b, c, d) + x + ac;
    a = this.ROTATE_LEFT(a, s);
    a += b;
  }

  private void II(ref uint a, uint b, uint c, uint d, uint x, byte s, uint ac)
  {
    a += this.III(b, c, d) + x + ac;
    a = this.ROTATE_LEFT(a, s);
    a += b;
  }

  private void Transform(uint[] inn)
  {
    uint a1 = this.m_buf[0];
    uint a2 = this.m_buf[1];
    uint a3 = this.m_buf[2];
    uint a4 = this.m_buf[3];
    this.FF(ref a1, a2, a3, a4, inn[0], (byte) 7, 3614090360U);
    this.FF(ref a4, a1, a2, a3, inn[1], (byte) 12, 3905402710U);
    this.FF(ref a3, a4, a1, a2, inn[2], (byte) 17, 606105819U);
    this.FF(ref a2, a3, a4, a1, inn[3], (byte) 22, 3250441966U);
    this.FF(ref a1, a2, a3, a4, inn[4], (byte) 7, 4118548399U);
    this.FF(ref a4, a1, a2, a3, inn[5], (byte) 12, 1200080426U);
    this.FF(ref a3, a4, a1, a2, inn[6], (byte) 17, 2821735955U);
    this.FF(ref a2, a3, a4, a1, inn[7], (byte) 22, 4249261313U);
    this.FF(ref a1, a2, a3, a4, inn[8], (byte) 7, 1770035416U);
    this.FF(ref a4, a1, a2, a3, inn[9], (byte) 12, 2336552879U);
    this.FF(ref a3, a4, a1, a2, inn[10], (byte) 17, 4294925233U);
    this.FF(ref a2, a3, a4, a1, inn[11], (byte) 22, 2304563134U);
    this.FF(ref a1, a2, a3, a4, inn[12], (byte) 7, 1804603682U);
    this.FF(ref a4, a1, a2, a3, inn[13], (byte) 12, 4254626195U);
    this.FF(ref a3, a4, a1, a2, inn[14], (byte) 17, 2792965006U);
    this.FF(ref a2, a3, a4, a1, inn[15], (byte) 22, 1236535329U);
    this.GG(ref a1, a2, a3, a4, inn[1], (byte) 5, 4129170786U);
    this.GG(ref a4, a1, a2, a3, inn[6], (byte) 9, 3225465664U);
    this.GG(ref a3, a4, a1, a2, inn[11], (byte) 14, 643717713U);
    this.GG(ref a2, a3, a4, a1, inn[0], (byte) 20, 3921069994U);
    this.GG(ref a1, a2, a3, a4, inn[5], (byte) 5, 3593408605U);
    this.GG(ref a4, a1, a2, a3, inn[10], (byte) 9, 38016083U);
    this.GG(ref a3, a4, a1, a2, inn[15], (byte) 14, 3634488961U);
    this.GG(ref a2, a3, a4, a1, inn[4], (byte) 20, 3889429448U);
    this.GG(ref a1, a2, a3, a4, inn[9], (byte) 5, 568446438U);
    this.GG(ref a4, a1, a2, a3, inn[14], (byte) 9, 3275163606U);
    this.GG(ref a3, a4, a1, a2, inn[3], (byte) 14, 4107603335U);
    this.GG(ref a2, a3, a4, a1, inn[8], (byte) 20, 1163531501U);
    this.GG(ref a1, a2, a3, a4, inn[13], (byte) 5, 2850285829U);
    this.GG(ref a4, a1, a2, a3, inn[2], (byte) 9, 4243563512U);
    this.GG(ref a3, a4, a1, a2, inn[7], (byte) 14, 1735328473U);
    this.GG(ref a2, a3, a4, a1, inn[12], (byte) 20, 2368359562U);
    this.HH(ref a1, a2, a3, a4, inn[5], (byte) 4, 4294588738U);
    this.HH(ref a4, a1, a2, a3, inn[8], (byte) 11, 2272392833U);
    this.HH(ref a3, a4, a1, a2, inn[11], (byte) 16 /*0x10*/, 1839030562U);
    this.HH(ref a2, a3, a4, a1, inn[14], (byte) 23, 4259657740U);
    this.HH(ref a1, a2, a3, a4, inn[1], (byte) 4, 2763975236U);
    this.HH(ref a4, a1, a2, a3, inn[4], (byte) 11, 1272893353U);
    this.HH(ref a3, a4, a1, a2, inn[7], (byte) 16 /*0x10*/, 4139469664U);
    this.HH(ref a2, a3, a4, a1, inn[10], (byte) 23, 3200236656U);
    this.HH(ref a1, a2, a3, a4, inn[13], (byte) 4, 681279174U);
    this.HH(ref a4, a1, a2, a3, inn[0], (byte) 11, 3936430074U);
    this.HH(ref a3, a4, a1, a2, inn[3], (byte) 16 /*0x10*/, 3572445317U);
    this.HH(ref a2, a3, a4, a1, inn[6], (byte) 23, 76029189U);
    this.HH(ref a1, a2, a3, a4, inn[9], (byte) 4, 3654602809U);
    this.HH(ref a4, a1, a2, a3, inn[12], (byte) 11, 3873151461U);
    this.HH(ref a3, a4, a1, a2, inn[15], (byte) 16 /*0x10*/, 530742520U);
    this.HH(ref a2, a3, a4, a1, inn[2], (byte) 23, 3299628645U);
    this.II(ref a1, a2, a3, a4, inn[0], (byte) 6, 4096336452U);
    this.II(ref a4, a1, a2, a3, inn[7], (byte) 10, 1126891415U);
    this.II(ref a3, a4, a1, a2, inn[14], (byte) 15, 2878612391U);
    this.II(ref a2, a3, a4, a1, inn[5], (byte) 21, 4237533241U);
    this.II(ref a1, a2, a3, a4, inn[12], (byte) 6, 1700485571U);
    this.II(ref a4, a1, a2, a3, inn[3], (byte) 10, 2399980690U);
    this.II(ref a3, a4, a1, a2, inn[10], (byte) 15, 4293915773U);
    this.II(ref a2, a3, a4, a1, inn[1], (byte) 21, 2240044497U);
    this.II(ref a1, a2, a3, a4, inn[8], (byte) 6, 1873313359U);
    this.II(ref a4, a1, a2, a3, inn[15], (byte) 10, 4264355552U);
    this.II(ref a3, a4, a1, a2, inn[6], (byte) 15, 2734768916U);
    this.II(ref a2, a3, a4, a1, inn[13], (byte) 21, 1309151649U);
    this.II(ref a1, a2, a3, a4, inn[4], (byte) 6, 4149444226U);
    this.II(ref a4, a1, a2, a3, inn[11], (byte) 10, 3174756917U);
    this.II(ref a3, a4, a1, a2, inn[2], (byte) 15, 718787259U);
    this.II(ref a2, a3, a4, a1, inn[9], (byte) 21, 3951481745U);
    this.m_buf[0] += a1;
    this.m_buf[1] += a2;
    this.m_buf[2] += a3;
    this.m_buf[3] += a4;
  }
}
