// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Escher.MSOFBH
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Escher;

internal class MSOFBH : BaseWordRecord
{
  private uint m_ver;
  private uint m_inst;
  private MSOFBT m_fbt;
  private uint m_cbLength;

  public void Read(Stream stream)
  {
    uint num = BaseWordRecord.ReadUInt32(stream);
    this.m_ver = num & 15U;
    this.m_inst = (num & 65520U) >> 4;
    this.m_fbt = (MSOFBT) ((num & 4294901760U) >> 16 /*0x10*/);
    this.m_cbLength = BaseWordRecord.ReadUInt32(stream);
  }

  public void Write(Stream stream)
  {
    uint num = this.m_ver + (this.m_inst << 4) + ((uint) this.m_fbt << 16 /*0x10*/);
    BaseWordRecord.WriteUInt32(stream, num);
    BaseWordRecord.WriteUInt32(stream, this.m_cbLength);
  }

  public uint Version
  {
    get => this.m_ver;
    set => this.m_ver = value;
  }

  public uint Inst
  {
    get => this.m_inst;
    set => this.m_inst = value;
  }

  internal MSOFBT Msofbt
  {
    get => this.m_fbt;
    set => this.m_fbt = value;
  }

  public uint Length
  {
    get => this.m_cbLength;
    set => this.m_cbLength = value;
  }
}
