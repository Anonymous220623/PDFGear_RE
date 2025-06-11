// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.ListInfo
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

internal class ListInfo : BaseWordRecord
{
  private const int DEF_LIST_ID = 1720085641;
  private const string DEF_BULLLET_FIRST = "\uF0B7";
  private const string DEF_BULLLET_SECOND = "o";
  private const string DEF_BULLLET_THIRD = "\uF0A7";
  private const int DEF_MULTIPLIER = 1440;
  private int m_listid = 1720085641;
  private ListFormats m_listFormats;
  private ListFormatOverrides m_listFormatOverrides;

  internal ListInfo()
  {
    this.m_listFormats = new ListFormats();
    this.m_listFormatOverrides = new ListFormatOverrides();
  }

  internal ListInfo(Fib fib, Stream stream)
  {
    this.m_listFormats = new ListFormats();
    this.m_listFormatOverrides = new ListFormatOverrides();
    this.ReadLst(fib, stream);
    this.ReadLfo(fib, stream);
    this.ReadStringTable(fib, stream);
  }

  internal void ReadLst(Fib fib, Stream stream)
  {
    if (fib.FibRgFcLcb97LcbPlfLst == 0U)
      return;
    stream.Position = (long) fib.FibRgFcLcb97FcPlfLst;
    int num = (int) BaseWordRecord.ReadInt16(stream);
    for (int index = 0; index < num; ++index)
      this.m_listFormats.Add(new ListData(stream));
    foreach (ListData listFormat in (List<ListData>) this.m_listFormats)
      listFormat.ReadLvl(stream);
  }

  internal void ReadLfo(Fib fib, Stream stream)
  {
    if (fib.FibRgFcLcb97LcbPlfLfo == 0U)
      return;
    stream.Position = (long) fib.FibRgFcLcb97FcPlfLfo;
    int num = BaseWordRecord.ReadInt32(stream);
    for (int index = 0; index < num; ++index)
      this.m_listFormatOverrides.Add(new ListFormatOverride(stream));
    foreach (ListFormatOverride listFormatOverride in (List<ListFormatOverride>) this.m_listFormatOverrides)
      listFormatOverride.ReadLfoLvls(stream);
  }

  internal void ReadStringTable(Fib fib, Stream stream)
  {
    stream.Position = (long) fib.FibRgFcLcb97FcSttbListNames;
  }

  internal int WriteLfo(Stream stream)
  {
    if (this.m_listFormatOverrides.Count == 0)
      return 0;
    int position = (int) stream.Position;
    BaseWordRecord.WriteInt32(stream, this.m_listFormatOverrides.Count);
    foreach (ListFormatOverride listFormatOverride in (List<ListFormatOverride>) this.m_listFormatOverrides)
      listFormatOverride.WriteLfo(stream);
    foreach (ListFormatOverride listFormatOverride in (List<ListFormatOverride>) this.m_listFormatOverrides)
      listFormatOverride.WriteLfoLvls(stream);
    return (int) stream.Position - position;
  }

  internal int WriteLst(Stream stream)
  {
    if (this.m_listFormats.Count == 0)
      return 0;
    int position = (int) stream.Position;
    BaseWordRecord.WriteInt16(stream, (short) this.m_listFormats.Count);
    foreach (ListData listFormat in (List<ListData>) this.m_listFormats)
      listFormat.WriteListData(stream);
    int num = (int) stream.Position - position;
    foreach (ListData listFormat in (List<ListData>) this.m_listFormats)
      listFormat.WriteLvl(stream);
    return num;
  }

  internal int WriteStringTable(Stream stream)
  {
    byte[] buffer = new byte[8];
    buffer[0] = buffer[1] = byte.MaxValue;
    buffer[2] = (byte) 1;
    stream.Write(buffer, 0, buffer.Length);
    return buffer.Length;
  }

  internal short ApplyNumberList()
  {
    ListData listData = new ListData(this.m_listid);
    ++this.m_listid;
    this.m_listFormats.Add(listData);
    int num1 = 0;
    for (float num2 = 0.5f; (double) num2 < 4.5; num2 += 1.5f)
    {
      ListLevels levels1 = listData.Levels;
      int dxLeft1 = (int) (1440.0 * (double) num2);
      int levelNumber1 = num1;
      int num3 = levelNumber1 + 1;
      ListLevel numberLvl1 = this.CreateNumberLvl(dxLeft1, levelNumber1, ListPatternType.Arabic, ListNumberAlignment.Left);
      levels1.Add((object) numberLvl1);
      ListLevels levels2 = listData.Levels;
      int dxLeft2 = (int) (1440.0 * ((double) num2 + 0.5));
      int levelNumber2 = num3;
      int num4 = levelNumber2 + 1;
      ListLevel numberLvl2 = this.CreateNumberLvl(dxLeft2, levelNumber2, ListPatternType.LowLetter, ListNumberAlignment.Right);
      levels2.Add((object) numberLvl2);
      ListLevels levels3 = listData.Levels;
      int dxLeft3 = (int) (1440.0 * ((double) num2 + 1.0));
      int levelNumber3 = num4;
      num1 = levelNumber3 + 1;
      ListLevel numberLvl3 = this.CreateNumberLvl(dxLeft3, levelNumber3, ListPatternType.LowRoman, ListNumberAlignment.Left);
      levels3.Add((object) numberLvl3);
    }
    this.m_listFormatOverrides.Add(new ListFormatOverride()
    {
      ListID = listData.ListID
    });
    return Convert.ToInt16(this.m_listFormatOverrides.Count);
  }

  internal ListData GetLevelFormat(int levelNumber)
  {
    return this.m_listFormats.FindListData(this.m_listFormatOverrides[levelNumber - 1].ListID);
  }

  internal ListInfo Clone() => this.MemberwiseClone() as ListInfo;

  internal short ApplyBulletList()
  {
    ListData listData = new ListData(this.m_listid);
    ++this.m_listid;
    this.m_listFormats.Add(listData);
    for (float num = 0.5f; (double) num < 4.5; num += 1.5f)
    {
      listData.Levels.Add((object) this.CreateBuletLvl((int) (1440.0 * (double) num), "\uF0B7"));
      listData.Levels.Add((object) this.CreateBuletLvl((int) (1440.0 * ((double) num + 0.5)), "o"));
      listData.Levels.Add((object) this.CreateBuletLvl((int) (1440.0 * ((double) num + 1.0)), "\uF0A7"));
    }
    this.m_listFormatOverrides.Add(new ListFormatOverride()
    {
      ListID = listData.ListID
    });
    return Convert.ToInt16(this.m_listFormatOverrides.Count);
  }

  internal short ApplyList(ListData listData, WListFormat listFormat, WordStyleSheet styleSheet)
  {
    this.m_listFormats.Add(listData);
    return this.ApplyLFO(listData, listFormat, styleSheet);
  }

  internal short ApplyLFO(ListData listData, WListFormat listFormat, WordStyleSheet styleSheet)
  {
    ListFormatOverride lfo = new ListFormatOverride();
    if (listFormat.LFOStyleName != null)
    {
      string lfoStyleName = listFormat.LFOStyleName;
      ListOverrideStyle byName = listFormat.Document.ListOverrides.FindByName(lfoStyleName);
      if (byName != null)
        ListPropertiesConverter.ImportListOverride(byName, lfo, styleSheet);
    }
    lfo.ListID = listData.ListID;
    this.m_listFormatOverrides.Add(lfo);
    return Convert.ToInt16(this.m_listFormatOverrides.Count);
  }

  internal ListFormatOverrides ListFormatOverrides => this.m_listFormatOverrides;

  internal ListFormats ListFormats => this.m_listFormats;

  internal override void Close()
  {
    base.Close();
    if (this.m_listFormats != null)
    {
      foreach (ListData listFormat in (List<ListData>) this.m_listFormats)
        listFormat?.Close();
      this.m_listFormats.Clear();
      this.m_listFormats = (ListFormats) null;
    }
    if (this.m_listFormatOverrides == null)
      return;
    foreach (ListFormatOverride listFormatOverride in (List<ListFormatOverride>) this.m_listFormatOverrides)
      listFormatOverride?.Close();
    this.m_listFormatOverrides.Clear();
    this.m_listFormatOverrides = (ListFormatOverrides) null;
  }

  private ListLevel CreateBuletLvl(int dxLeft, string str) => new ListLevel();

  private ListLevel CreateNumberLvl(
    int dxLeft,
    int levelNumber,
    ListPatternType nfc,
    ListNumberAlignment align)
  {
    return new ListLevel();
  }
}
