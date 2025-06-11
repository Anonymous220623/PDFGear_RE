// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.ListData
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

internal class ListData : BaseWordRecord
{
  private const int DEF_LEVELS_COUNT = 9;
  private const int DEF_RGISTD = 4095 /*0x0FFF*/;
  private const int DEF_SIMPLE_BIT = 1;
  private const int DEF_HYBRID_BIT = 16 /*0x10*/;
  private int m_lsid;
  private int m_tplc;
  private int[] m_rgistd;
  private int m_Options;
  private ListLevels m_levels;
  private string m_name;

  internal ListData(int lsid)
    : this(lsid, true, false)
  {
  }

  internal ListData(int lsid, bool isHybrid, bool isSimpleList)
  {
    this.m_rgistd = new int[9];
    this.m_levels = new ListLevels();
    this.m_lsid = lsid;
    this.m_tplc = ~lsid;
    int num = isSimpleList ? 1 : 9;
    for (int index = 0; index < num; ++index)
      this.m_rgistd[index] = 4095 /*0x0FFF*/;
    if (isSimpleList)
      this.m_Options |= 1;
    if (isHybrid)
      this.m_Options |= 16 /*0x10*/;
    this.m_name = "";
  }

  internal ListData(Stream reader)
  {
    this.m_rgistd = new int[9];
    this.m_levels = new ListLevels();
    this.ReadListData(reader);
  }

  internal ListLevels Levels => this.m_levels;

  internal string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  internal bool RestartHeading => (this.m_Options & 2) != 0;

  internal bool SimpleList
  {
    get => (this.m_Options & 1) != 0;
    set
    {
      this.m_Options &= 254;
      this.m_Options |= value ? 1 : 0;
    }
  }

  internal bool IsHybridMultilevel => (this.m_Options & 16 /*0x10*/) != 0;

  internal int ListID
  {
    get => this.m_lsid;
    set => this.m_lsid = value;
  }

  internal override void Close()
  {
    base.Close();
    this.m_rgistd = (int[]) null;
    if (this.m_levels == null)
      return;
    this.m_levels.Clear();
    this.m_levels = (ListLevels) null;
  }

  internal void ReadLvl(Stream stream)
  {
    int num = this.SimpleList ? 1 : 9;
    for (int index = 0; index < num; ++index)
      this.m_levels.Add((object) new ListLevel(stream));
  }

  internal void WriteListData(Stream stream)
  {
    if (this.m_levels.Count == 1)
      this.SimpleList = true;
    BaseWordRecord.WriteInt32(stream, this.m_lsid);
    BaseWordRecord.WriteInt32(stream, this.m_tplc);
    for (int index = 0; index < this.m_rgistd.Length; ++index)
      BaseWordRecord.WriteInt16(stream, (short) this.m_rgistd[index]);
    BaseWordRecord.WriteUInt16(stream, (ushort) this.m_Options);
  }

  internal void ReadListData(Stream stream)
  {
    this.m_lsid = BaseWordRecord.ReadInt32(stream);
    this.m_tplc = BaseWordRecord.ReadInt32(stream);
    for (int index = 0; index < this.m_rgistd.Length; ++index)
      this.m_rgistd[index] = (int) BaseWordRecord.ReadInt16(stream);
    this.m_Options = (int) BaseWordRecord.ReadUInt16(stream);
  }

  internal void WriteLvl(Stream stream)
  {
    foreach (ListLevel level in (List<object>) this.m_levels)
      level.Write(stream);
  }
}
