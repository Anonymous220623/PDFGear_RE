// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.ListFormatOverride
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

internal class ListFormatOverride : BaseWordRecord
{
  private int m_lsid;
  private int m_unused1;
  private int m_unused2;
  internal int m_res1;
  internal int m_res2;
  private List<object> m_levels;
  private int m_clfolvl;

  internal ListFormatOverride() => this.m_levels = (List<object>) new ListLevels();

  internal ListFormatOverride(Stream stream)
  {
    this.m_levels = (List<object>) new ListLevels();
    this.ReadLfo(stream);
  }

  internal override void Close()
  {
    base.Close();
    if (this.m_levels == null)
      return;
    foreach (object level in this.m_levels)
    {
      if (level is ListFormatOverrideLevel)
        (level as ListFormatOverrideLevel).Close();
    }
    this.m_levels.Clear();
    this.m_levels = (List<object>) null;
  }

  internal List<object> Levels => this.m_levels;

  internal int ListID
  {
    get => this.m_lsid;
    set => this.m_lsid = value;
  }

  internal void WriteLfo(Stream stream)
  {
    BaseWordRecord.WriteInt32(stream, this.m_lsid);
    BaseWordRecord.WriteInt32(stream, this.m_unused1);
    BaseWordRecord.WriteInt32(stream, this.m_unused2);
    stream.WriteByte((byte) this.m_levels.Count);
    stream.WriteByte((byte) this.m_res1);
    BaseWordRecord.WriteInt16(stream, (short) this.m_res2);
  }

  internal void ReadLfo(Stream stream)
  {
    long position = stream.Position;
    this.m_lsid = BaseWordRecord.ReadInt32(stream);
    this.m_unused1 = BaseWordRecord.ReadInt32(stream);
    this.m_unused2 = BaseWordRecord.ReadInt32(stream);
    this.m_clfolvl = stream.ReadByte();
    this.m_res1 = stream.ReadByte();
    this.m_res2 = (int) BaseWordRecord.ReadInt16(stream);
  }

  internal void WriteLfoLvls(Stream stream)
  {
    BaseWordRecord.WriteUInt32(stream, uint.MaxValue);
    int index = 0;
    for (int count = this.m_levels.Count; index < count; ++index)
      ((ListFormatOverrideLevel) this.m_levels[index]).Write(stream);
  }

  internal void ReadLfoLvls(Stream stream)
  {
    int num = (int) BaseWordRecord.ReadUInt32(stream);
    for (int index = 0; index < this.m_clfolvl; ++index)
      this.m_levels.Add((object) new ListFormatOverrideLevel(stream));
  }
}
