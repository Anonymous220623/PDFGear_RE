// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Escher.FSP
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Escher;

internal class FSP : BaseWordRecord
{
  private uint m_spid;
  private uint m_grfPersistent;

  public void Read(Stream stream)
  {
    this.m_spid = BaseWordRecord.ReadUInt32(stream);
    this.m_grfPersistent = BaseWordRecord.ReadUInt32(stream);
  }

  public void Write(Stream stream)
  {
    BaseWordRecord.WriteUInt32(stream, this.m_spid);
    BaseWordRecord.WriteUInt32(stream, this.m_grfPersistent);
  }

  public uint Spid
  {
    get => this.m_spid;
    set => this.m_spid = value;
  }

  public uint GzfPersistent
  {
    get => this.m_grfPersistent;
    set => this.m_grfPersistent = value;
  }

  public bool IsGroup => ((int) this.m_grfPersistent & 1) == 1;

  public bool ISChild => ((int) this.m_grfPersistent & 2) == 1;

  public bool IsPatriarch => ((int) this.m_grfPersistent & 4) == 1;

  public bool IsDeleted => ((int) this.m_grfPersistent & 8) == 1;

  public bool IsOleShape => ((int) this.m_grfPersistent & 16 /*0x10*/) == 1;

  public bool IsHaveMaster => ((int) this.m_grfPersistent & 32 /*0x20*/) == 1;

  public bool IsFliph => ((int) this.m_grfPersistent & 64 /*0x40*/) == 1;

  public bool IsFlipv => ((int) this.m_grfPersistent & 128 /*0x80*/) == 1;

  public bool IsConnector => ((int) this.m_grfPersistent & 256 /*0x0100*/) == 1;

  public bool IsHaveAnchor => ((int) this.m_grfPersistent & 512 /*0x0200*/) == 1;

  public bool IsBackground => ((int) this.m_grfPersistent & 1024 /*0x0400*/) == 1;

  public bool IsHavespt => ((int) this.m_grfPersistent & 2048 /*0x0800*/) == 1;
}
