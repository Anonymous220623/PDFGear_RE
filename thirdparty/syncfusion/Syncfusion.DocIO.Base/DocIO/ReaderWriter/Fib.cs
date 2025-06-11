// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Fib
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

internal class Fib
{
  private byte[] m_fibBase;
  private ushort m_csw;
  private byte[] m_fibRgW;
  private ushort m_cslw;
  private byte[] m_fibRgLw;
  private ushort m_cbRgFcLcb;
  private byte[] m_fibRgFcLcbBlob;
  private ushort m_cswNew;
  private byte[] m_fibRgCswNew;
  private Encoding m_encoding = Encoding.Unicode;

  internal ushort FibVersion => this.CswNew > (ushort) 0 ? this.NFibNew : this.NFib;

  internal ushort WIdent
  {
    get => BitConverter.ToUInt16(this.m_fibBase, 0);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibBase, 0, 2);
  }

  internal ushort NFib
  {
    get => BitConverter.ToUInt16(this.m_fibBase, 2);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibBase, 2, 2);
  }

  internal ushort BaseUnused
  {
    get => BitConverter.ToUInt16(this.m_fibBase, 4);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibBase, 4, 2);
  }

  internal ushort Lid
  {
    get => BitConverter.ToUInt16(this.m_fibBase, 6);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibBase, 6, 2);
  }

  internal ushort PnNext
  {
    get => BitConverter.ToUInt16(this.m_fibBase, 8);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibBase, 8, 2);
  }

  internal bool FDot
  {
    get => ((int) this.m_fibBase[10] & 1) != 0;
    set => this.m_fibBase[10] = (byte) ((int) this.m_fibBase[10] & 254 | (value ? 1 : 0));
  }

  internal bool FGlsy
  {
    get => ((int) this.m_fibBase[10] & 2) >> 1 != 0;
    set => this.m_fibBase[10] = (byte) ((int) this.m_fibBase[10] & 253 | (value ? 1 : 0) << 1);
  }

  internal bool FComplex
  {
    get => ((int) this.m_fibBase[10] & 4) >> 2 != 0;
    set => this.m_fibBase[10] = (byte) ((int) this.m_fibBase[10] & 251 | (value ? 1 : 0) << 2);
  }

  internal bool FHasPic
  {
    get => ((int) this.m_fibBase[10] & 8) >> 3 != 0;
    set => this.m_fibBase[10] = (byte) ((int) this.m_fibBase[10] & 247 | (value ? 1 : 0) << 3);
  }

  internal byte CQuickSaves
  {
    get => (byte) ((uint) this.m_fibBase[10] >> 4);
    set
    {
      this.m_fibBase[10] = (byte) ((int) this.m_fibBase[10] & 15 | (value > (byte) 15 ? 15 : (int) value) << 4);
    }
  }

  internal bool FEncrypted
  {
    get => ((int) this.m_fibBase[11] & 1) != 0;
    set => this.m_fibBase[11] = (byte) ((int) this.m_fibBase[11] & 254 | (value ? 1 : 0));
  }

  internal bool FWhichTblStm
  {
    get => ((int) this.m_fibBase[11] & 2) >> 1 != 0;
    set => this.m_fibBase[11] = (byte) ((int) this.m_fibBase[11] & 253 | (value ? 1 : 0) << 1);
  }

  internal bool FReadOnlyRecommended
  {
    get => ((int) this.m_fibBase[11] & 4) >> 2 != 0;
    set => this.m_fibBase[11] = (byte) ((int) this.m_fibBase[11] & 251 | (value ? 1 : 0) << 2);
  }

  internal bool FWriteReservation
  {
    get => ((int) this.m_fibBase[11] & 8) >> 3 != 0;
    set => this.m_fibBase[11] = (byte) ((int) this.m_fibBase[11] & 247 | (value ? 1 : 0) << 3);
  }

  internal bool FExtChar
  {
    get => ((int) this.m_fibBase[11] & 16 /*0x10*/) >> 4 != 0;
    set => this.m_fibBase[11] = (byte) ((int) this.m_fibBase[11] & 239 | (value ? 1 : 0) << 4);
  }

  internal bool FLoadOverride
  {
    get => ((int) this.m_fibBase[11] & 32 /*0x20*/) >> 5 != 0;
    set => this.m_fibBase[11] = (byte) ((int) this.m_fibBase[11] & 223 | (value ? 1 : 0) << 5);
  }

  internal bool FFarEast
  {
    get => ((int) this.m_fibBase[11] & 64 /*0x40*/) >> 6 != 0;
    set => this.m_fibBase[11] = (byte) ((int) this.m_fibBase[11] & 191 | (value ? 1 : 0) << 6);
  }

  internal bool FObfuscated
  {
    get => ((int) this.m_fibBase[11] & 128 /*0x80*/) >> 7 != 0;
    set
    {
      this.m_fibBase[11] = (byte) ((int) this.m_fibBase[11] & (int) sbyte.MaxValue | (value ? 1 : 0) << 7);
    }
  }

  internal ushort NFibBack
  {
    get => BitConverter.ToUInt16(this.m_fibBase, 12);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibBase, 12, 2);
  }

  internal int LKey
  {
    get => BitConverter.ToInt32(this.m_fibBase, 14);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibBase, 14, 4);
  }

  internal byte Envr
  {
    get => this.m_fibBase[18];
    set => this.m_fibBase[18] = value;
  }

  internal bool FMac
  {
    get => ((int) this.m_fibBase[19] & 1) != 0;
    set => this.m_fibBase[19] = (byte) ((int) this.m_fibBase[19] & 254 | (value ? 1 : 0));
  }

  internal bool FEmptySpecial
  {
    get => ((int) this.m_fibBase[19] & 2) >> 1 != 0;
    set => this.m_fibBase[19] = (byte) ((int) this.m_fibBase[19] & 253 | (value ? 1 : 0) << 1);
  }

  internal bool FLoadOverridePage
  {
    get => ((int) this.m_fibBase[19] & 4) >> 2 != 0;
    set => this.m_fibBase[19] = (byte) ((int) this.m_fibBase[19] & 251 | (value ? 1 : 0) << 2);
  }

  internal bool BaseReserved1
  {
    get => ((int) this.m_fibBase[19] & 8) >> 3 != 0;
    set => this.m_fibBase[19] = (byte) ((int) this.m_fibBase[19] & 247 | (value ? 1 : 0) << 3);
  }

  internal bool BaseReserved2
  {
    get => ((int) this.m_fibBase[19] & 16 /*0x10*/) >> 4 != 0;
    set => this.m_fibBase[19] = (byte) ((int) this.m_fibBase[19] & 239 | (value ? 1 : 0) << 4);
  }

  internal byte FSpare0
  {
    get => (byte) ((uint) this.m_fibBase[19] >> 5);
    set
    {
      this.m_fibBase[19] = (byte) ((int) this.m_fibBase[19] & 31 /*0x1F*/ | (value > (byte) 7 ? 7 : (int) value) << 5);
    }
  }

  internal ushort BaseReserved3
  {
    get => BitConverter.ToUInt16(this.m_fibBase, 20);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibBase, 20, 2);
  }

  internal ushort BaseReserved4
  {
    get => BitConverter.ToUInt16(this.m_fibBase, 22);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibBase, 22, 2);
  }

  internal uint BaseReserved5
  {
    get => BitConverter.ToUInt32(this.m_fibBase, 24);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibBase, 24, 4);
  }

  internal uint BaseReserved6
  {
    get => BitConverter.ToUInt32(this.m_fibBase, 28);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibBase, 28, 4);
  }

  internal ushort Csw
  {
    get => this.m_csw;
    set => this.m_csw = value;
  }

  internal ushort FibRgWReserved1
  {
    get => BitConverter.ToUInt16(this.m_fibRgW, 0);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgW, 0, 2);
  }

  internal ushort FibRgWReserved2
  {
    get => BitConverter.ToUInt16(this.m_fibRgW, 2);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgW, 2, 2);
  }

  internal ushort FibRgWReserved3
  {
    get => BitConverter.ToUInt16(this.m_fibRgW, 4);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgW, 4, 2);
  }

  internal ushort FibRgWReserved4
  {
    get => BitConverter.ToUInt16(this.m_fibRgW, 6);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgW, 6, 2);
  }

  internal ushort FibRgWReserved5
  {
    get => BitConverter.ToUInt16(this.m_fibRgW, 8);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgW, 8, 2);
  }

  internal ushort FibRgWReserved6
  {
    get => BitConverter.ToUInt16(this.m_fibRgW, 10);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgW, 10, 2);
  }

  internal ushort FibRgWReserved7
  {
    get => BitConverter.ToUInt16(this.m_fibRgW, 12);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgW, 12, 2);
  }

  internal ushort FibRgWReserved8
  {
    get => BitConverter.ToUInt16(this.m_fibRgW, 14);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgW, 14, 2);
  }

  internal ushort FibRgWReserved9
  {
    get => BitConverter.ToUInt16(this.m_fibRgW, 16 /*0x10*/);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgW, 16 /*0x10*/, 2);
    }
  }

  internal ushort FibRgWReserved10
  {
    get => BitConverter.ToUInt16(this.m_fibRgW, 18);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgW, 18, 2);
  }

  internal ushort FibRgWReserved11
  {
    get => BitConverter.ToUInt16(this.m_fibRgW, 20);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgW, 20, 2);
  }

  internal ushort FibRgWReserved12
  {
    get => BitConverter.ToUInt16(this.m_fibRgW, 22);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgW, 22, 2);
  }

  internal ushort FibRgWReserved13
  {
    get => BitConverter.ToUInt16(this.m_fibRgW, 24);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgW, 24, 2);
  }

  internal ushort FibRgWLidFE
  {
    get => BitConverter.ToUInt16(this.m_fibRgW, 26);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgW, 26, 2);
  }

  internal ushort Cslw
  {
    get => this.m_cslw;
    set => this.m_cslw = value;
  }

  internal int CbMac
  {
    get => BitConverter.ToInt32(this.m_fibRgLw, 0);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgLw, 0, 4);
  }

  internal int RgLwReserved1
  {
    get => BitConverter.ToInt32(this.m_fibRgLw, 4);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgLw, 4, 4);
  }

  internal int RgLwReserved2
  {
    get => BitConverter.ToInt32(this.m_fibRgLw, 8);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgLw, 8, 4);
  }

  internal int CcpText
  {
    get => BitConverter.ToInt32(this.m_fibRgLw, 12);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgLw, 12, 4);
  }

  internal int CcpFtn
  {
    get => BitConverter.ToInt32(this.m_fibRgLw, 16 /*0x10*/);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgLw, 16 /*0x10*/, 4);
    }
  }

  internal int CcpHdd
  {
    get => BitConverter.ToInt32(this.m_fibRgLw, 20);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgLw, 20, 4);
  }

  internal int RgLwReserved3
  {
    get => BitConverter.ToInt32(this.m_fibRgLw, 24);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgLw, 24, 4);
  }

  internal int CcpAtn
  {
    get => BitConverter.ToInt32(this.m_fibRgLw, 28);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgLw, 28, 4);
  }

  internal int CcpEdn
  {
    get => BitConverter.ToInt32(this.m_fibRgLw, 32 /*0x20*/);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgLw, 32 /*0x20*/, 4);
    }
  }

  internal int CcpTxbx
  {
    get => BitConverter.ToInt32(this.m_fibRgLw, 36);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgLw, 36, 4);
  }

  internal int CcpHdrTxbx
  {
    get => BitConverter.ToInt32(this.m_fibRgLw, 40);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgLw, 40, 4);
  }

  internal int RgLwReserved4
  {
    get => BitConverter.ToInt32(this.m_fibRgLw, 44);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgLw, 44, 4);
  }

  internal int RgLwReserved5
  {
    get => BitConverter.ToInt32(this.m_fibRgLw, 48 /*0x30*/);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgLw, 48 /*0x30*/, 4);
    }
  }

  internal int RgLwReserved6
  {
    get => BitConverter.ToInt32(this.m_fibRgLw, 52);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgLw, 52, 4);
  }

  internal int RgLwReserved7
  {
    get => BitConverter.ToInt32(this.m_fibRgLw, 56);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgLw, 56, 4);
  }

  internal int RgLwReserved8
  {
    get => BitConverter.ToInt32(this.m_fibRgLw, 60);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgLw, 60, 4);
  }

  internal int RgLwReserved9
  {
    get => BitConverter.ToInt32(this.m_fibRgLw, 64 /*0x40*/);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgLw, 64 /*0x40*/, 4);
    }
  }

  internal int RgLwReserved10
  {
    get => BitConverter.ToInt32(this.m_fibRgLw, 68);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgLw, 68, 4);
  }

  internal int RgLwReserved11
  {
    get => BitConverter.ToInt32(this.m_fibRgLw, 72);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgLw, 72, 4);
  }

  internal int RgLwReserved12
  {
    get => BitConverter.ToInt32(this.m_fibRgLw, 76);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgLw, 76, 4);
  }

  internal int RgLwReserved13
  {
    get => BitConverter.ToInt32(this.m_fibRgLw, 80 /*0x50*/);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgLw, 80 /*0x50*/, 4);
    }
  }

  internal int RgLwReserved14
  {
    get => BitConverter.ToInt32(this.m_fibRgLw, 84);
    set => Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgLw, 84, 4);
  }

  internal ushort CbRgFcLcb
  {
    get => this.m_cbRgFcLcb;
    set => this.m_cbRgFcLcb = value;
  }

  internal uint FibRgFcLcb97FcStshfOrig
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 0);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 0, 4);
    }
  }

  internal uint FibRgFcLcb97LcbStshfOrig
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 4);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 4, 4);
    }
  }

  internal uint FibRgFcLcb97FcStshf
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 8);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 8, 4);
    }
  }

  internal uint FibRgFcLcb97LcbStshf
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 12);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 12, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcffndRef
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 16 /*0x10*/);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 16 /*0x10*/, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcffndRef
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 20);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 20, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcffndTxt
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 24);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 24, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcffndTxt
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 28);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 28, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfandRef
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 32 /*0x20*/);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 32 /*0x20*/, 4);
    }
  }

  internal uint FibRgFcLcb97lcbPlcfandRef
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 36);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 36, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfandTxt
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 40);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 40, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfandTxt
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 44);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 44, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfSed
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 48 /*0x30*/);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 48 /*0x30*/, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfSed
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 52);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 52, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcPad
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 56);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 56, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcPad
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 60);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 60, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfPhe
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 64 /*0x40*/);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 64 /*0x40*/, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfPhe
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 68);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 68, 4);
    }
  }

  internal uint FibRgFcLcb97FcSttbfGlsy
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 72);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 72, 4);
    }
  }

  internal uint FibRgFcLcb97LcbSttbfGlsy
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 76);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 76, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfGlsy
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 80 /*0x50*/);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 80 /*0x50*/, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfGlsy
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 84);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 84, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfHdd
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 88);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 88, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfHdd
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 92);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 92, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfBteChpx
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 96 /*0x60*/);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 96 /*0x60*/, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfBteChpx
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 100);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 100, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfBtePapx
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 104);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 104, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfBtePapx
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 108);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 108, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfSea
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 112 /*0x70*/);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 112 /*0x70*/, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfSea
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 116);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 116, 4);
    }
  }

  internal uint FibRgFcLcb97FcSttbfFfn
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 120);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 120, 4);
    }
  }

  internal uint FibRgFcLcb97LcbSttbfFfn
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 124);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 124, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfFldMom
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 128 /*0x80*/);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 128 /*0x80*/, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfFldMom
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 132);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 132, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfFldHdr
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 136);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 136, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfFldHdr
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 140);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 140, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfFldFtn
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 144 /*0x90*/);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 144 /*0x90*/, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfFldFtn
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 148);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 148, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfFldAtn
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 152);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 152, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfFldAtn
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 156);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 156, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfFldMcr
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 160 /*0xA0*/);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 160 /*0xA0*/, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfFldMcr
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 164);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 164, 4);
    }
  }

  internal uint FibRgFcLcb97FcSttbfBkmk
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 168);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 168, 4);
    }
  }

  internal uint FibRgFcLcb97LcbSttbfBkmk
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 172);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 172, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfBkf
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 176 /*0xB0*/);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 176 /*0xB0*/, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfBkf
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 180);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 180, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfBkl
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 184);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 184, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfBkl
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 188);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 188, 4);
    }
  }

  internal uint FibRgFcLcb97FcCmds
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 192 /*0xC0*/);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 192 /*0xC0*/, 4);
    }
  }

  internal uint FibRgFcLcb97LcbCmds
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 196);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 196, 4);
    }
  }

  internal uint FibRgFcLcb97FcUnused1
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 200);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 200, 4);
    }
  }

  internal uint FibRgFcLcb97LcbUnused1
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 204);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 204, 4);
    }
  }

  internal uint FibRgFcLcb97FcSttbfMcr
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 208 /*0xD0*/);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 208 /*0xD0*/, 4);
    }
  }

  internal uint FibRgFcLcb97LcbSttbfMcr
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 212);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 212, 4);
    }
  }

  internal uint FibRgFcLcb97FcPrDrvr
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 216);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 216, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPrDrvr
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 220);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 220, 4);
    }
  }

  internal uint FibRgFcLcb97FcPrEnvPort
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 224 /*0xE0*/);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 224 /*0xE0*/, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPrEnvPort
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 228);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 228, 4);
    }
  }

  internal uint FibRgFcLcb97FcPrEnvLand
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 232);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 232, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPrEnvLand
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 236);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 236, 4);
    }
  }

  internal uint FibRgFcLcb97FcWss
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 240 /*0xF0*/);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 240 /*0xF0*/, 4);
    }
  }

  internal uint FibRgFcLcb97LcbWss
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 244);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 244, 4);
    }
  }

  internal uint FibRgFcLcb97FcDop
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 248);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 248, 4);
    }
  }

  internal uint FibRgFcLcb97LcbDop
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 252);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 252, 4);
    }
  }

  internal uint FibRgFcLcb97FcSttbfAssoc
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 256 /*0x0100*/);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 256 /*0x0100*/, 4);
    }
  }

  internal uint FibRgFcLcb97LcbSttbfAssoc
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 260);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 260, 4);
    }
  }

  internal uint FibRgFcLcb97FcClx
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 264);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 264, 4);
    }
  }

  internal uint FibRgFcLcb97LcbClx
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 268);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 268, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfPgdFtn
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 272);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 272, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfPgdFtn
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 276);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 276, 4);
    }
  }

  internal uint FibRgFcLcb97FcAutosaveSource
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 280);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 280, 4);
    }
  }

  internal uint FibRgFcLcb97LcbAutosaveSource
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 284);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 284, 4);
    }
  }

  internal uint FibRgFcLcb97FcGrpXstAtnOwners
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 288);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 288, 4);
    }
  }

  internal uint FibRgFcLcb97LcbGrpXstAtnOwners
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 292);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 292, 4);
    }
  }

  internal uint FibRgFcLcb97FcSttbfAtnBkmk
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 296);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 296, 4);
    }
  }

  internal uint FibRgFcLcb97LcbSttbfAtnBkmk
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 300);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 300, 4);
    }
  }

  internal uint FibRgFcLcb97FcUnused2
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 304);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 304, 4);
    }
  }

  internal uint FibRgFcLcb97LcbUnused2
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 308);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 308, 4);
    }
  }

  internal uint FibRgFcLcb97FcUnused3
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 312);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 312, 4);
    }
  }

  internal uint FibRgFcLcb97LcbUnused3
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 316);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 316, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcSpaMom
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 320);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 320, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcSpaMom
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 324);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 324, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcSpaHdr
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 328);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 328, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcSpaHdr
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 332);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 332, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfAtnBkf
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 336);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 336, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfAtnBkf
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 340);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 340, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfAtnBkl
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 344);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 344, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfAtnBkl
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 348);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 348, 4);
    }
  }

  internal uint FibRgFcLcb97FcPms
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 352);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 352, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPms
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 356);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 356, 4);
    }
  }

  internal uint FibRgFcLcb97FcFormFldSttbs
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 360);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 360, 4);
    }
  }

  internal uint FibRgFcLcb97LcbFormFldSttbs
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 364);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 364, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfendRef
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 368);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 368, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfendRef
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 372);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 372, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfendTxt
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 376);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 376, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfendTxt
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 380);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 380, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfFldEdn
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 384);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 384, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfFldEdn
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 388);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 388, 4);
    }
  }

  internal uint FibRgFcLcb97FcUnused4
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 392);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 392, 4);
    }
  }

  internal uint FibRgFcLcb97LcbUnused4
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 396);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 396, 4);
    }
  }

  internal uint FibRgFcLcb97FcDggInfo
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 400);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 400, 4);
    }
  }

  internal uint FibRgFcLcb97LcbDggInfo
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 404);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 404, 4);
    }
  }

  internal uint FibRgFcLcb97FcSttbfRMark
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 408);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 408, 4);
    }
  }

  internal uint FibRgFcLcb97LcbSttbfRMark
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 412);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 412, 4);
    }
  }

  internal uint FibRgFcLcb97FcSttbfCaption
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 416);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 416, 4);
    }
  }

  internal uint FibRgFcLcb97LcbSttbfCaption
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 420);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 420, 4);
    }
  }

  internal uint FibRgFcLcb97FcSttbfAutoCaption
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 424);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 424, 4);
    }
  }

  internal uint FibRgFcLcb97LcbSttbfAutoCaption
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 428);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 428, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfWkb
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 432);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 432, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfWkb
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 436);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 436, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfSpl
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 440);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 440, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfSpl
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 444);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 444, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcftxbxTxt
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 448);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 448, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcftxbxTxt
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 452);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 452, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfFldTxbx
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 456);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 456, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfFldTxbx
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 460);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 460, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfHdrtxbxTxt
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 464);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 464, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfHdrtxbxTxt
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 468);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 468, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcffldHdrTxbx
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 472);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 472, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcffldHdrTxbx
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 476);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 476, 4);
    }
  }

  internal uint FibRgFcLcb97FcStwUser
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 480);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 480, 4);
    }
  }

  internal uint FibRgFcLcb97LcbStwUser
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 484);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 484, 4);
    }
  }

  internal uint FibRgFcLcb97FcSttbTtmbd
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 488);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 488, 4);
    }
  }

  internal uint FibRgFcLcb97LcbSttbTtmbd
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 492);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 492, 4);
    }
  }

  internal uint FibRgFcLcb97FcCookieData
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 496);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 496, 4);
    }
  }

  internal uint FibRgFcLcb97LcbCookieData
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 500);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 500, 4);
    }
  }

  internal uint FibRgFcLcb97FcPgdMotherOldOld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 504);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 504, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPgdMotherOldOld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 508);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 508, 4);
    }
  }

  internal uint FibRgFcLcb97FcBkdMotherOldOld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 512 /*0x0200*/);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 512 /*0x0200*/, 4);
    }
  }

  internal uint FibRgFcLcb97LcbBkdMotherOldOld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 516);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 516, 4);
    }
  }

  internal uint FibRgFcLcb97FcPgdFtnOldOld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 520);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 520, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPgdFtnOldOld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 524);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 524, 4);
    }
  }

  internal uint FibRgFcLcb97FcBkdFtnOldOld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 528);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 528, 4);
    }
  }

  internal uint FibRgFcLcb97LcbBkdFtnOldOld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 532);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 532, 4);
    }
  }

  internal uint FibRgFcLcb97FcPgdEdnOldOld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 536);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 536, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPgdEdnOldOld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 540);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 540, 4);
    }
  }

  internal uint FibRgFcLcb97FcBkdEdnOldOld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 544);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 544, 4);
    }
  }

  internal uint FibRgFcLcb97LcbBkdEdnOldOld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 548);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 548, 4);
    }
  }

  internal uint FibRgFcLcb97FcSttbfIntlFld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 552);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 552, 4);
    }
  }

  internal uint FibRgFcLcb97LcbSttbfIntlFld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 556);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 556, 4);
    }
  }

  internal uint FibRgFcLcb97FcRouteSlip
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 560);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 560, 4);
    }
  }

  internal uint FibRgFcLcb97LcbRouteSlip
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 564);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 564, 4);
    }
  }

  internal uint FibRgFcLcb97FcSttbSavedBy
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 568);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 568, 4);
    }
  }

  internal uint FibRgFcLcb97LcbSttbSavedBy
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 572);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 572, 4);
    }
  }

  internal uint FibRgFcLcb97FcSttbFnm
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 576);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 576, 4);
    }
  }

  internal uint FibRgFcLcb97LcbSttbFnm
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 580);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 580, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlfLst
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 584);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 584, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlfLst
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 588);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 588, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlfLfo
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 592);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 592, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlfLfo
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 596);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 596, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfTxbxBkd
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 600);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 600, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfTxbxBkd
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 604);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 604, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfTxbxHdrBkd
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 608);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 608, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfTxbxHdrBkd
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 612);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 612, 4);
    }
  }

  internal uint FibRgFcLcb97FcDocUndoWord9
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 616);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 616, 4);
    }
  }

  internal uint FibRgFcLcb97LcbDocUndoWord9
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 620);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 620, 4);
    }
  }

  internal uint FibRgFcLcb97FcRgbUse
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 624);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 624, 4);
    }
  }

  internal uint FibRgFcLcb97LcbRgbUse
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 628);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 628, 4);
    }
  }

  internal uint FibRgFcLcb97FcUsp
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 632);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 632, 4);
    }
  }

  internal uint FibRgFcLcb97LcbUsp
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 636);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 636, 4);
    }
  }

  internal uint FibRgFcLcb97FcUskf
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 640);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 640, 4);
    }
  }

  internal uint FibRgFcLcb97LcbUskf
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 644);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 644, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcupcRgbUse
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 648);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 648, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcupcRgbUse
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 652);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 652, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcupcUsp
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 656);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 656, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcupcUsp
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 660);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 660, 4);
    }
  }

  internal uint FibRgFcLcb97FcSttbGlsyStyle
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 664);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 664, 4);
    }
  }

  internal uint FibRgFcLcb97LcbSttbGlsyStyle
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 668);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 668, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlgosl
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 672);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 672, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlgosl
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 676);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 676, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcocx
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 680);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 680, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcocx
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 684);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 684, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfBteLvc
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 688);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 688, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfBteLvc
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 692);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 692, 4);
    }
  }

  internal uint FibRgFcLcb97DwLowDateTime
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 696);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 696, 4);
    }
  }

  internal uint FibRgFcLcb97DwHighDateTime
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 700);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 700, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfLvcPre10
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 704);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 704, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfLvcPre10
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 708);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 708, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfAsumy
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 712);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 712, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfAsumy
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 716);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 716, 4);
    }
  }

  internal uint FibRgFcLcb97FcPlcfGram
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 720);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 720, 4);
    }
  }

  internal uint FibRgFcLcb97LcbPlcfGram
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 724);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 724, 4);
    }
  }

  internal uint FibRgFcLcb97FcSttbListNames
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 728);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 728, 4);
    }
  }

  internal uint FibRgFcLcb97LcbSttbListNames
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 732);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 732, 4);
    }
  }

  internal uint FibRgFcLcb97fcSttbfUssr
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 736);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 736, 4);
    }
  }

  internal uint FibRgFcLcb97LcbSttbfUssr
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 740);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 740, 4);
    }
  }

  internal uint FibRgFcLcb2000FcPlcfTch
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 744);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 744, 4);
    }
  }

  internal uint FibRgFcLcb2000LcbPlcfTch
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 748);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 748, 4);
    }
  }

  internal uint FibRgFcLcb2000FcRmdThreading
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 752);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 752, 4);
    }
  }

  internal uint FibRgFcLcb2000LcbRmdThreading
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 756);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 756, 4);
    }
  }

  internal uint FibRgFcLcb2000FcMid
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 760);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 760, 4);
    }
  }

  internal uint FibRgFcLcb2000LcbMid
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 764);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 764, 4);
    }
  }

  internal uint FibRgFcLcb2000FcSttbRgtplc
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 768 /*0x0300*/);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 768 /*0x0300*/, 4);
    }
  }

  internal uint FibRgFcLcb2000LcbSttbRgtplc
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 772);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 772, 4);
    }
  }

  internal uint FibRgFcLcb2000FcMsoEnvelope
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 776);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 776, 4);
    }
  }

  internal uint FibRgFcLcb2000LcbMsoEnvelope
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 780);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 780, 4);
    }
  }

  internal uint FibRgFcLcb2000FcPlcfLad
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 784);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 784, 4);
    }
  }

  internal uint FibRgFcLcb2000LcbPlcfLad
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 788);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 788, 4);
    }
  }

  internal uint FibRgFcLcb2000FcRgDofr
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 792);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 792, 4);
    }
  }

  internal uint FibRgFcLcb2000LcbRgDofr
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 796);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 796, 4);
    }
  }

  internal uint FibRgFcLcb2000FcPlcosl
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 800);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 800, 4);
    }
  }

  internal uint FibRgFcLcb2000LcbPlcosl
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 804);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 804, 4);
    }
  }

  internal uint FibRgFcLcb2000FcPlcfCookieOld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 808);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 808, 4);
    }
  }

  internal uint FibRgFcLcb2000LcbPlcfCookieOld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 812);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 812, 4);
    }
  }

  internal uint FibRgFcLcb2000FcPgdMotherOld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 816);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 816, 4);
    }
  }

  internal uint FibRgFcLcb2000LcbPgdMotherOld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 820);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 820, 4);
    }
  }

  internal uint FibRgFcLcb2000FcBkdMotherOld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 824);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 824, 4);
    }
  }

  internal uint FibRgFcLcb2000LcbBkdMotherOld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 828);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 828, 4);
    }
  }

  internal uint FibRgFcLcb2000FcPgdFtnOld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 832);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 832, 4);
    }
  }

  internal uint FibRgFcLcb2000LcbPgdFtnOld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 836);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 836, 4);
    }
  }

  internal uint FibRgFcLcb2000FcBkdFtnOld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 840);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 840, 4);
    }
  }

  internal uint FibRgFcLcb2000LcbBkdFtnOld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 844);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 844, 4);
    }
  }

  internal uint FibRgFcLcb2000FcPgdEdnOld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 848);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 848, 4);
    }
  }

  internal uint FibRgFcLcb2000LcbPgdEdnOld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 852);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 852, 4);
    }
  }

  internal uint FibRgFcLcb2000FcBkdEdnOld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 856);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 856, 4);
    }
  }

  internal uint FibRgFcLcb2000LcbBkdEdnOld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 860);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 860, 4);
    }
  }

  internal uint FibRgFcLcb2002FcUnused1
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 864);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 864, 4);
    }
  }

  internal uint FibRgFcLcb2002LcbUnused1
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 868);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 868, 4);
    }
  }

  internal uint FibRgFcLcb2002FcPlcfPgp
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 872);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 872, 4);
    }
  }

  internal uint FibRgFcLcb2002LcbPlcfPgp
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 876);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 876, 4);
    }
  }

  internal uint FibRgFcLcb2002FcPlcfuim
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 880);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 880, 4);
    }
  }

  internal uint FibRgFcLcb2002LcbPlcfuim
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 884);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 884, 4);
    }
  }

  internal uint FibRgFcLcb2002FcPlfguidUim
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 888);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 888, 4);
    }
  }

  internal uint FibRgFcLcb2002LcbPlfguidUim
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 892);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 892, 4);
    }
  }

  internal uint FibRgFcLcb2002FcAtrdExtra
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 896);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 896, 4);
    }
  }

  internal uint FibRgFcLcb2002LcbAtrdExtra
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 900);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 900, 4);
    }
  }

  internal uint FibRgFcLcb2002FcPlrsid
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 904);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 904, 4);
    }
  }

  internal uint FibRgFcLcb2002LcbPlrsid
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 908);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 908, 4);
    }
  }

  internal uint FibRgFcLcb2002FcSttbfBkmkFactoid
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 912);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 912, 4);
    }
  }

  internal uint FibRgFcLcb2002LcbSttbfBkmkFactoid
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 916);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 916, 4);
    }
  }

  internal uint FibRgFcLcb2002FcPlcfBkfFactoid
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 920);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 920, 4);
    }
  }

  internal uint FibRgFcLcb2002LcbPlcfBkfFactoid
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 924);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 924, 4);
    }
  }

  internal uint FibRgFcLcb2002FcPlcfcookie
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 928);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 928, 4);
    }
  }

  internal uint FibRgFcLcb2002LcbPlcfcookie
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 932);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 932, 4);
    }
  }

  internal uint FibRgFcLcb2002FcPlcfBklFactoid
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 936);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 936, 4);
    }
  }

  internal uint FibRgFcLcb2002LcbPlcfBklFactoid
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 940);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 940, 4);
    }
  }

  internal uint FibRgFcLcb2002FcFactoidData
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 944);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 944, 4);
    }
  }

  internal uint FibRgFcLcb2002LcbFactoidData
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 948);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 948, 4);
    }
  }

  internal uint FibRgFcLcb2002FcDocUndo
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 952);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 952, 4);
    }
  }

  internal uint FibRgFcLcb2002LcbDocUndo
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 956);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 956, 4);
    }
  }

  internal uint FibRgFcLcb2002FcSttbfBkmkFcc
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 960);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 960, 4);
    }
  }

  internal uint FibRgFcLcb2002LcbSttbfBkmkFcc
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 964);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 964, 4);
    }
  }

  internal uint FibRgFcLcb2002FcPlcfBkfFcc
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 968);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 968, 4);
    }
  }

  internal uint FibRgFcLcb2002LcbPlcfBkfFcc
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 972);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 972, 4);
    }
  }

  internal uint FibRgFcLcb2002FcPlcfBklFcc
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 976);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 976, 4);
    }
  }

  internal uint FibRgFcLcb2002LcbPlcfBklFcc
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 980);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 980, 4);
    }
  }

  internal uint FibRgFcLcb2002FcSttbfbkmkBPRepairs
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 984);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 984, 4);
    }
  }

  internal uint FibRgFcLcb2002LcbSttbfbkmkBPRepairs
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 988);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 988, 4);
    }
  }

  internal uint FibRgFcLcb2002FcPlcfbkfBPRepairs
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 992);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 992, 4);
    }
  }

  internal uint FibRgFcLcb2002LcbPlcfbkfBPRepairs
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 996);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 996, 4);
    }
  }

  internal uint FibRgFcLcb2002FcPlcfbklBPRepairs
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1000);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1000, 4);
    }
  }

  internal uint FibRgFcLcb2002LcbPlcfbklBPRepairs
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1004);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1004, 4);
    }
  }

  internal uint FibRgFcLcb2002FcPmsNew
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1008);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1008, 4);
    }
  }

  internal uint FibRgFcLcb2002LcbPmsNew
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1012);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1012, 4);
    }
  }

  internal uint FibRgFcLcb2002FcODSO
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1016);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1016, 4);
    }
  }

  internal uint FibRgFcLcb2002LcbODSO
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1020);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1020, 4);
    }
  }

  internal uint FibRgFcLcb2002FcPlcfpmiOldXP
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1024 /*0x0400*/);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1024 /*0x0400*/, 4);
    }
  }

  internal uint FibRgFcLcb2002LcbPlcfpmiOldXP
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1028);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1028, 4);
    }
  }

  internal uint FibRgFcLcb2002FcPlcfpmiNewXP
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1032);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1032, 4);
    }
  }

  internal uint FibRgFcLcb2002LcbPlcfpmiNewXP
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1036);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1036, 4);
    }
  }

  internal uint FibRgFcLcb2002FcPlcfpmiMixedXP
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1040);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1040, 4);
    }
  }

  internal uint FibRgFcLcb2002LcbPlcfpmiMixedXP
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1044);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1044, 4);
    }
  }

  internal uint FibRgFcLcb2002FcUnused2
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1048);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibBase, 1048, 4);
    }
  }

  internal uint FibRgFcLcb2002LcbUnused2
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1052);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1052, 4);
    }
  }

  internal uint FibRgFcLcb2002FcPlcffactoid
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1056);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1056, 4);
    }
  }

  internal uint FibRgFcLcb2002LcbPlcffactoid
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1060);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1060, 4);
    }
  }

  internal uint FibRgFcLcb2002FcPlcflvcOldXP
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1064);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1064, 4);
    }
  }

  internal uint FibRgFcLcb2002LcbPlcflvcOldXP
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1068);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1068, 4);
    }
  }

  internal uint FibRgFcLcb2002FcPlcflvcNewXP
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1072);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1072, 4);
    }
  }

  internal uint FibRgFcLcb2002LcbPlcflvcNewXP
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1076);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1076, 4);
    }
  }

  internal uint FibRgFcLcb2002FcPlcflvcMixedXP
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1080);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1080, 4);
    }
  }

  internal uint FibRgFcLcb2002LcbPlcflvcMixedXP
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1084);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1084, 4);
    }
  }

  internal uint FibRgFcLcb2003FcHplxsdr
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1088);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1088, 4);
    }
  }

  internal uint FibRgFcLcb2003LcbHplxsdr
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1092);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1092, 4);
    }
  }

  internal uint FibRgFcLcb2003FcSttbfBkmkSdt
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1096);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1096, 4);
    }
  }

  internal uint FibRgFcLcb2003LcbSttbfBkmkSdt
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1100);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1100, 4);
    }
  }

  internal uint FibRgFcLcb2003FcPlcfBkfSdt
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1104);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1104, 4);
    }
  }

  internal uint FibRgFcLcb2003LcbPlcfBkfSdt
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1108);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1108, 4);
    }
  }

  internal uint FibRgFcLcb2003FcPlcfBklSdt
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1112);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1112, 4);
    }
  }

  internal uint FibRgFcLcb2003LcbPlcfBklSdt
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1116);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1116, 4);
    }
  }

  internal uint FibRgFcLcb2003FcCustomXForm
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1120);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1120, 4);
    }
  }

  internal uint FibRgFcLcb2003LcbCustomXForm
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1124);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1124, 4);
    }
  }

  internal uint FibRgFcLcb2003FcSttbfBkmkProt
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1128);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1128, 4);
    }
  }

  internal uint FibRgFcLcb2003LcbSttbfBkmkProt
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1132);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1132, 4);
    }
  }

  internal uint FibRgFcLcb2003FcPlcfBkfProt
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1136);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1136, 4);
    }
  }

  internal uint FibRgFcLcb2003LcbPlcfBkfProt
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1140);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1140, 4);
    }
  }

  internal uint FibRgFcLcb2003FcPlcfBklProt
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1144);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1144, 4);
    }
  }

  internal uint FibRgFcLcb2003LcbPlcfBklProt
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1148);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1148, 4);
    }
  }

  internal uint FibRgFcLcb2003FcSttbProtUser
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1152);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1152, 4);
    }
  }

  internal uint FibRgFcLcb2003LcbSttbProtUser
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1156);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1156, 4);
    }
  }

  internal uint FibRgFcLcb2003FcUnused
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1160);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1160, 4);
    }
  }

  internal uint FibRgFcLcb2003LcbUnused
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1164);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1164, 4);
    }
  }

  internal uint FibRgFcLcb2003FcPlcfpmiOld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1168);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1168, 4);
    }
  }

  internal uint FibRgFcLcb2003LcbPlcfpmiOld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1172);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1172, 4);
    }
  }

  internal uint FibRgFcLcb2003FcPlcfpmiOldInline
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1176);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1176, 4);
    }
  }

  internal uint FibRgFcLcb2003LcbPlcfpmiOldInline
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1180);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1180, 4);
    }
  }

  internal uint FibRgFcLcb2003FcPlcfpmiNew
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1184);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1184, 4);
    }
  }

  internal uint FibRgFcLcb2003LcbPlcfpmiNew
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1188);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1188, 4);
    }
  }

  internal uint FibRgFcLcb2003FcPlcfpmiNewInline
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1192);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1192, 4);
    }
  }

  internal uint FibRgFcLcb2003LcbPlcfpmiNewInline
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1196);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1196, 4);
    }
  }

  internal uint FibRgFcLcb2003FcPlcflvcOld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1200);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1200, 4);
    }
  }

  internal uint FibRgFcLcb2003LcbPlcflvcOld
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1204);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1204, 4);
    }
  }

  internal uint FibRgFcLcb2003FcPlcflvcOldInline
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1208);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1208, 4);
    }
  }

  internal uint FibRgFcLcb2003LcbPlcflvcOldInline
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1212);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1212, 4);
    }
  }

  internal uint FibRgFcLcb2003FcPlcflvcNew
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1216);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1216, 4);
    }
  }

  internal uint FibRgFcLcb2003LcbPlcflvcNew
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1220);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1220, 4);
    }
  }

  internal uint FibRgFcLcb2003FcPlcflvcNewInline
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1224);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1224, 4);
    }
  }

  internal uint FibRgFcLcb2003LcbPlcflvcNewInline
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1228);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1228, 4);
    }
  }

  internal uint FibRgFcLcb2003FcPgdMother
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1232);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1232, 4);
    }
  }

  internal uint FibRgFcLcb2003LcbPgdMother
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1236);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1236, 4);
    }
  }

  internal uint FibRgFcLcb2003FcBkdMother
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1240);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1240, 4);
    }
  }

  internal uint FibRgFcLcb2003LcbBkdMother
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1244);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1244, 4);
    }
  }

  internal uint FibRgFcLcb2003FcAfdMother
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1248);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1248, 4);
    }
  }

  internal uint FibRgFcLcb2003LcbAfdMother
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1252);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1252, 4);
    }
  }

  internal uint FibRgFcLcb2003FcPgdFtn
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1256);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1256, 4);
    }
  }

  internal uint FibRgFcLcb2003LcbPgdFtn
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1260);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1260, 4);
    }
  }

  internal uint FibRgFcLcb2003FcBkdFtn
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1264);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1264, 4);
    }
  }

  internal uint FibRgFcLcb2003LcbBkdFtn
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1268);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1268, 4);
    }
  }

  internal uint FibRgFcLcb2003FcAfdFtn
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1272);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1272, 4);
    }
  }

  internal uint FibRgFcLcb2003LcbAfdFtn
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1276);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1276, 4);
    }
  }

  internal uint FibRgFcLcb2003FcPgdEdn
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1280 /*0x0500*/);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1280 /*0x0500*/, 4);
    }
  }

  internal uint FibRgFcLcb2003LcbPgdEdn
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1284);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1284, 4);
    }
  }

  internal uint FibRgFcLcb2003FcBkdEdn
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1288);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1288, 4);
    }
  }

  internal uint FibRgFcLcb2003LcbBkdEdn
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1292);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1292, 4);
    }
  }

  internal uint FibRgFcLcb2003FcAfdEdn
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1296);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1296, 4);
    }
  }

  internal uint FibRgFcLcb2003LcbAfdEdn
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1300);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1300, 4);
    }
  }

  internal uint FibRgFcLcb2003FcAfd
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1304);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1304, 4);
    }
  }

  internal uint FibRgFcLcb2003LcbAfd
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1308);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1308, 4);
    }
  }

  internal uint FibRgFcLcb2007FcPlcfmthd
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1312);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1312, 4);
    }
  }

  internal uint FibRgFcLcb2007LcbPlcfmthd
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1316);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1316, 4);
    }
  }

  internal uint FibRgFcLcb2007FcSttbfBkmkMoveFrom
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1320);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1320, 4);
    }
  }

  internal uint FibRgFcLcb2007LcbSttbfBkmkMoveFrom
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1324);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1324, 4);
    }
  }

  internal uint FibRgFcLcb2007FcPlcfBkfMoveFrom
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1328);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1328, 4);
    }
  }

  internal uint FibRgFcLcb2007LcbPlcfBkfMoveFrom
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1332);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1332, 4);
    }
  }

  internal uint FibRgFcLcb2007FcPlcfBklMoveFrom
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1336);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1336, 4);
    }
  }

  internal uint FibRgFcLcb2007LcbPlcfBklMoveFrom
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1340);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1340, 4);
    }
  }

  internal uint FibRgFcLcb2007FcSttbfBkmkMoveTo
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1344);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1344, 4);
    }
  }

  internal uint FibRgFcLcb2007LcbSttbfBkmkMoveTo
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1348);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1348, 4);
    }
  }

  internal uint FibRgFcLcb2007FcPlcfBkfMoveTo
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1352);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1352, 4);
    }
  }

  internal uint FibRgFcLcb2007LcbPlcfBkfMoveTo
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1356);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1356, 4);
    }
  }

  internal uint FibRgFcLcb2007FcPlcfBklMoveTo
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1360);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1360, 4);
    }
  }

  internal uint FibRgFcLcb2007LcbPlcfBklMoveTo
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1364);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1364, 4);
    }
  }

  internal uint FibRgFcLcb2007FcUnused1
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1368);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1368, 4);
    }
  }

  internal uint FibRgFcLcb2007LcbUnused1
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1372);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1372, 4);
    }
  }

  internal uint FibRgFcLcb2007FcUnused2
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1376);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1376, 4);
    }
  }

  internal uint FibRgFcLcb2007LcbUnused2
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1380);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1380, 4);
    }
  }

  internal uint FibRgFcLcb2007FcUnused3
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1384);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1384, 4);
    }
  }

  internal uint FibRgFcLcb2007LcbUnused3
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1388);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1388, 4);
    }
  }

  internal uint FibRgFcLcb2007FcSttbfBkmkArto
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1392);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1392, 4);
    }
  }

  internal uint FibRgFcLcb2007LcbSttbfBkmkArto
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1396);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1396, 4);
    }
  }

  internal uint FibRgFcLcb2007FcPlcfBkfArto
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1400);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1400, 4);
    }
  }

  internal uint FibRgFcLcb2007LcbPlcfBkfArto
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1404);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1404, 4);
    }
  }

  internal uint FibRgFcLcb2007FcPlcfBklArto
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1408);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1408, 4);
    }
  }

  internal uint FibRgFcLcb2007LcbPlcfBklArto
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1412);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1412, 4);
    }
  }

  internal uint FibRgFcLcb2007FcArtoData
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1416);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1416, 4);
    }
  }

  internal uint FibRgFcLcb2007LcbArtoData
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1420);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1420, 4);
    }
  }

  internal uint FibRgFcLcb2007FcUnused4
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1424);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1424, 4);
    }
  }

  internal uint FibRgFcLcb2007LcbUnused4
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1428);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1428, 4);
    }
  }

  internal uint FibRgFcLcb2007FcUnused5
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1432);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1432, 4);
    }
  }

  internal uint FibRgFcLcb2007LcbUnused5
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1436);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1436, 4);
    }
  }

  internal uint FibRgFcLcb2007FcUnused6
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1440);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1440, 4);
    }
  }

  internal uint FibRgFcLcb2007LcbUnused6
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1444);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1444, 4);
    }
  }

  internal uint FibRgFcLcb2007FcOssTheme
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1448);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1448, 4);
    }
  }

  internal uint FibRgFcLcb2007LcbOssTheme
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1452);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1452, 4);
    }
  }

  internal uint FibRgFcLcb2007FcColorSchemeMapping
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1456);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1456, 4);
    }
  }

  internal uint FibRgFcLcb2007LcbColorSchemeMapping
  {
    get => BitConverter.ToUInt32(this.m_fibRgFcLcbBlob, 1460);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgFcLcbBlob, 1460, 4);
    }
  }

  internal ushort CswNew
  {
    get => this.m_cswNew;
    set => this.m_cswNew = value;
  }

  internal ushort NFibNew
  {
    get => BitConverter.ToUInt16(this.m_fibRgCswNew, 0);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgCswNew, 0, 2);
    }
  }

  internal ushort CQuickSavesNew
  {
    get => BitConverter.ToUInt16(this.m_fibRgCswNew, 2);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgCswNew, 2, 2);
    }
  }

  internal ushort LidThemeOther
  {
    get => BitConverter.ToUInt16(this.m_fibRgCswNew, 4);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgCswNew, 4, 2);
    }
  }

  internal ushort LidThemeFE
  {
    get => BitConverter.ToUInt16(this.m_fibRgCswNew, 6);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgCswNew, 6, 2);
    }
  }

  internal ushort LidThemeCS
  {
    get => BitConverter.ToUInt16(this.m_fibRgCswNew, 8);
    set
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.m_fibRgCswNew, 8, 2);
    }
  }

  internal int EncodingCharSize => this.m_encoding != Encoding.ASCII ? 2 : 1;

  internal Encoding Encoding
  {
    get => this.m_encoding;
    set => this.m_encoding = value;
  }

  internal Fib() => this.Initialize();

  private void Initialize()
  {
    this.m_fibBase = new byte[32 /*0x20*/];
    this.WIdent = (ushort) 42476;
    this.NFib = (ushort) 193;
    this.BaseUnused = (ushort) 57437;
    this.Lid = (ushort) 1033;
    this.PnNext = (ushort) 0;
    this.FDot = false;
    this.FGlsy = false;
    this.FComplex = false;
    this.FHasPic = false;
    this.CQuickSaves = (byte) 15;
    this.FEncrypted = false;
    this.FWhichTblStm = true;
    this.FReadOnlyRecommended = false;
    this.FWriteReservation = false;
    this.FExtChar = true;
    this.FLoadOverride = false;
    this.FFarEast = false;
    this.FObfuscated = false;
    this.NFibBack = (ushort) 191;
    this.LKey = 0;
    this.Envr = (byte) 0;
    this.FMac = false;
    this.FEmptySpecial = false;
    this.FLoadOverridePage = false;
    this.BaseReserved1 = false;
    this.BaseReserved2 = true;
    this.FSpare0 = (byte) 0;
    this.BaseReserved3 = (ushort) 0;
    this.BaseReserved4 = (ushort) 0;
    this.BaseReserved5 = 2048U /*0x0800*/;
    this.BaseReserved6 = 2048U /*0x0800*/;
    this.m_csw = (ushort) 14;
    this.m_fibRgW = new byte[28];
    this.FibRgWReserved1 = (ushort) 27234;
    this.FibRgWReserved2 = (ushort) 27234;
    this.FibRgWReserved3 = (ushort) 14842;
    this.FibRgWReserved4 = (ushort) 14842;
    this.FibRgWLidFE = (ushort) 1033;
    this.m_cslw = (ushort) 22;
    this.m_fibRgLw = new byte[88];
    this.RgLwReserved1 = 1546671000;
    this.RgLwReserved2 = 1546671000;
    this.RgLwReserved4 = 1048575 /*0x0FFFFF*/;
    this.RgLwReserved7 = 1048575 /*0x0FFFFF*/;
    this.RgLwReserved10 = 1048575 /*0x0FFFFF*/;
    this.m_cbRgFcLcb = (ushort) 183;
    this.m_fibRgFcLcbBlob = new byte[1464];
    this.m_cswNew = (ushort) 5;
    this.m_fibRgCswNew = new byte[10];
    this.NFibNew = (ushort) 274;
  }

  private void InitializeBeforeRead()
  {
    this.m_fibBase = new byte[32 /*0x20*/];
    this.m_csw = (ushort) 0;
    this.m_fibRgW = new byte[28];
    this.m_cslw = (ushort) 0;
    this.m_fibRgLw = new byte[88];
    this.m_cbRgFcLcb = (ushort) 0;
    this.m_fibRgFcLcbBlob = new byte[1464];
    this.m_cswNew = (ushort) 0;
    this.m_fibRgCswNew = new byte[10];
  }

  internal void Read(Stream stream)
  {
    this.InitializeBeforeRead();
    stream.Position = 0L;
    stream.Read(this.m_fibBase, 0, 32 /*0x20*/);
    if (this.NFib >= (ushort) 101 && this.NFib <= (ushort) 105)
      throw new Exception("This file format is not supported");
    byte[] buffer1 = new byte[2];
    stream.Read(buffer1, 0, 2);
    this.m_csw = BitConverter.ToUInt16(buffer1, 0);
    stream.Read(this.m_fibRgW, 0, 28);
    byte[] buffer2 = new byte[2];
    stream.Read(buffer2, 0, 2);
    this.m_cslw = BitConverter.ToUInt16(buffer2, 0);
    if (this.FEncrypted)
      return;
    this.ReadInternal(stream);
  }

  internal void ReadAfterDecryption(Stream stream)
  {
    stream.Position = 64L /*0x40*/;
    this.ReadInternal(stream);
  }

  private void ReadInternal(Stream stream)
  {
    stream.Read(this.m_fibRgLw, 0, 88);
    byte[] buffer1 = new byte[2];
    stream.Read(buffer1, 0, 2);
    this.m_cbRgFcLcb = BitConverter.ToUInt16(buffer1, 0);
    if (this.m_cbRgFcLcb > (ushort) 183)
      this.m_cbRgFcLcb = (ushort) 183;
    int count1 = (int) this.m_cbRgFcLcb * 8;
    stream.Read(this.m_fibRgFcLcbBlob, 0, count1);
    byte[] buffer2 = new byte[2];
    stream.Read(buffer2, 0, 2);
    this.m_cswNew = BitConverter.ToUInt16(buffer2, 0);
    if (this.m_cswNew > (ushort) 5)
      this.m_cswNew = (ushort) 5;
    if (this.m_cswNew == (ushort) 0)
      return;
    int count2 = (int) this.m_cswNew * 2;
    stream.Read(this.m_fibRgCswNew, 0, count2);
  }

  private void ValidateCbRgFcLcb()
  {
    switch (this.NFib)
    {
      case 193:
        if (this.m_cbRgFcLcb == (ushort) 93)
          break;
        this.m_cbRgFcLcb = (ushort) 93;
        break;
      case 217:
        if (this.m_cbRgFcLcb == (ushort) 108)
          break;
        this.m_cbRgFcLcb = (ushort) 108;
        break;
      case 257:
        if (this.m_cbRgFcLcb == (ushort) 136)
          break;
        this.m_cbRgFcLcb = (ushort) 136;
        break;
      case 268:
        if (this.m_cbRgFcLcb == (ushort) 164)
          break;
        this.m_cbRgFcLcb = (ushort) 164;
        break;
      case 274:
        if (this.m_cbRgFcLcb == (ushort) 183)
          break;
        this.m_cbRgFcLcb = (ushort) 183;
        break;
    }
  }

  private void ValidateCswNew()
  {
    switch (this.NFib)
    {
      case 193:
        if (this.m_cswNew == (ushort) 0)
          break;
        this.m_cswNew = (ushort) 0;
        break;
      case 217:
        if (this.m_cswNew == (ushort) 2)
          break;
        this.m_cswNew = (ushort) 2;
        break;
      case 257:
        if (this.m_cswNew == (ushort) 2)
          break;
        this.m_cswNew = (ushort) 2;
        break;
      case 268:
        if (this.m_cswNew == (ushort) 2)
          break;
        this.m_cswNew = (ushort) 2;
        break;
      case 274:
        if (this.m_cswNew == (ushort) 5)
          break;
        this.m_cswNew = (ushort) 5;
        break;
    }
  }

  private void CorrectFib()
  {
    if (this.CcpHdrTxbx > 0)
      --this.CcpHdrTxbx;
    else if (this.CcpTxbx > 0)
      --this.CcpTxbx;
    else if (this.CcpEdn > 0)
      --this.CcpEdn;
    else if (this.CcpAtn > 0)
      --this.CcpAtn;
    else if (this.CcpHdd > 0)
    {
      --this.CcpHdd;
    }
    else
    {
      if (this.CcpFtn <= 0)
        return;
      --this.CcpFtn;
    }
  }

  internal void Write(Stream stream, ushort fibVersion)
  {
    this.CorrectFib();
    this.WriteInternal(stream);
    stream.Write(this.m_fibRgLw, 0, this.m_fibRgLw.Length);
    byte[] bytes1 = BitConverter.GetBytes(this.m_cbRgFcLcb);
    stream.Write(bytes1, 0, bytes1.Length);
    stream.Write(this.m_fibRgFcLcbBlob, 0, (int) this.m_cbRgFcLcb * 8);
    byte[] bytes2 = BitConverter.GetBytes(this.m_cswNew);
    stream.Write(bytes2, 0, bytes2.Length);
    if (this.m_cswNew <= (ushort) 0)
      return;
    stream.Write(this.m_fibRgCswNew, 0, (int) this.m_cswNew * 2);
  }

  private void WriteInternal(Stream stream)
  {
    stream.Position = 0L;
    stream.Write(this.m_fibBase, 0, this.m_fibBase.Length);
    byte[] bytes1 = BitConverter.GetBytes(this.m_csw);
    stream.Write(bytes1, 0, bytes1.Length);
    stream.Write(this.m_fibRgW, 0, this.m_fibRgW.Length);
    byte[] bytes2 = BitConverter.GetBytes(this.m_cslw);
    stream.Write(bytes2, 0, bytes2.Length);
  }

  internal void WriteAfterEncryption(Stream stream) => this.WriteInternal(stream);

  internal void UpdateFcMac()
  {
    this.BaseReserved6 = (uint) ((ulong) this.BaseReserved5 + (ulong) ((this.CcpText + this.CcpFtn + this.CcpHdd + this.CcpAtn + this.CcpEdn + this.CcpTxbx + this.CcpHdrTxbx) * this.EncodingCharSize));
  }

  internal virtual void Close()
  {
    this.m_fibBase = (byte[]) null;
    this.m_fibRgW = (byte[]) null;
    this.m_fibRgLw = (byte[]) null;
    this.m_fibRgFcLcbBlob = (byte[]) null;
    this.m_fibRgCswNew = (byte[]) null;
  }
}
