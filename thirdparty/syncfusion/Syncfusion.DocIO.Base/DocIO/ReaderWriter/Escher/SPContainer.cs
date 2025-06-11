// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Escher.SPContainer
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Escher;

internal class SPContainer : BaseWordRecord
{
  private const int DEF_MSOFBH_LENGTH = 8;
  private const int DEF_SP_LENGTH = 8;
  private const int DEF_FOPTE_LENGTH = 6;
  private const int DEF_RECT_LENGTH = 4;
  private FSP m_sp;
  private List<FOPTE> m_opt;
  private Rect m_anchor;
  private List<MSOFBH> m_msofbhArray;

  public SPContainer() => this.m_opt = new List<FOPTE>();

  public new int Length => 0 + 16 /*0x10*/ + this.GetFoptesLength() + 12;

  public void Read(Stream stream, uint lenght)
  {
    this.m_msofbhArray = new List<MSOFBH>();
    uint num = (uint) stream.Position + lenght;
    while (stream.Position < (long) num)
    {
      MSOFBH msofbh = new MSOFBH();
      msofbh.Read(stream);
      this.m_msofbhArray.Add(msofbh);
      switch (msofbh.Msofbt)
      {
        case MSOFBT.msofbtSp:
          this.m_sp = new FSP();
          this.m_sp.Read(stream);
          continue;
        case MSOFBT.msofbtOPT:
          this.ReadFoptes(stream, msofbh);
          continue;
        case MSOFBT.msofbtAnchor:
        case MSOFBT.msofbtChildAnchor:
        case MSOFBT.msofbtClientAnchor:
          this.m_anchor = new Rect();
          this.m_anchor.Read(stream);
          continue;
        default:
          stream.Position += (long) msofbh.Length;
          continue;
      }
    }
  }

  public void Write(Stream stream)
  {
    this.GenerateDefaultOPT();
    new MSOFBH()
    {
      Msofbt = MSOFBT.msofbtSpContainer,
      Inst = 0U,
      Length = ((uint) this.Length),
      Version = 15U
    }.Write(stream);
    this.WriteFSP(stream);
    this.WriteFoptes(stream);
    this.m_anchor = new Rect();
    this.m_anchor.Bottom = 120L;
    new MSOFBH()
    {
      Msofbt = MSOFBT.msofbtClientAnchor,
      Length = 4U,
      Inst = 0U,
      Version = 0U
    }.Write(stream);
    this.m_anchor.Write(stream);
  }

  private void ReadFoptes(Stream stream, MSOFBH msofbh)
  {
    int num1 = (int) msofbh.Length / 6;
    int num2 = 0;
    for (int index = 0; index < num1 && (long) num2 < (long) msofbh.Length; ++index)
    {
      FOPTE fopte = new FOPTE();
      num2 += fopte.Read(stream);
      this.m_opt.Add(fopte);
    }
    for (int index = 0; index < this.m_opt.Count; ++index)
    {
      if (this.m_opt[index].IsComplex)
        stream.Read(this.m_opt[index].NameBytes, 0, this.m_opt[index].NameBytes.Length);
    }
  }

  private void WriteFoptes(Stream stream)
  {
    new MSOFBH()
    {
      Msofbt = MSOFBT.msofbtOPT,
      Length = ((uint) this.GetFoptesLength()),
      Inst = 4U,
      Version = 3U
    }.Write(stream);
    for (int index = 0; index < this.m_opt.Count; ++index)
      this.m_opt[index].Write(stream);
    for (int index = 0; index < this.m_opt.Count; ++index)
    {
      if (this.m_opt[index].IsComplex)
        stream.Write(this.m_opt[index].NameBytes, 0, this.m_opt[index].NameBytes.Length);
    }
  }

  private int GetFoptesLength()
  {
    int foptesLength = 0;
    if (this.m_opt != null)
    {
      foptesLength += 8;
      for (int index = 0; index < this.m_opt.Count; ++index)
      {
        if (this.m_opt[index].IsComplex)
          foptesLength += (int) this.m_opt[index].Op + 6;
        else
          foptesLength += 6;
      }
    }
    return foptesLength;
  }

  private void GenerateDefaultOPT()
  {
    this.m_opt = new List<FOPTE>();
    this.m_opt.Add(new FOPTE()
    {
      IsBid = true,
      IsComplex = false,
      Pid = (ushort) 260,
      Op = 1U
    });
    FOPTE fopte = new FOPTE()
    {
      IsBid = true,
      IsComplex = true,
      Pid = 261,
      NameBytes = Encoding.Unicode.GetBytes("autowalls_ru_17\0")
    };
    fopte.Op = (uint) fopte.NameBytes.Length;
    this.m_opt.Add(fopte);
    this.m_opt.Add(new FOPTE()
    {
      IsBid = false,
      IsComplex = false,
      Pid = (ushort) 262,
      Op = 2U
    });
    this.m_opt.Add(new FOPTE()
    {
      IsBid = false,
      IsComplex = false,
      Pid = (ushort) 511 /*0x01FF*/,
      Op = 524288U /*0x080000*/
    });
  }

  private void WriteFSP(Stream stream)
  {
    new MSOFBH()
    {
      Msofbt = MSOFBT.msofbtSp,
      Inst = 75U,
      Version = 2U,
      Length = 8U
    }.Write(stream);
    new FSP() { Spid = 1026U, GzfPersistent = 2560U /*0x0A00*/ }.Write(stream);
  }
}
