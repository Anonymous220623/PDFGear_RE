// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.ListFormatOverrideLevel
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

internal class ListFormatOverrideLevel : BaseWordRecord
{
  internal int m_startAt;
  internal int m_ilvl;
  internal bool m_bStartAt;
  internal bool m_bFormatting;
  internal int m_reserved1;
  internal int m_reserved2;
  internal int m_reserved3;
  internal ListLevel m_lvl;

  internal ListFormatOverrideLevel(bool overrideLvl)
  {
    if (!overrideLvl)
      return;
    this.m_lvl = new ListLevel();
  }

  internal ListFormatOverrideLevel(Stream stream)
  {
    int num1 = (int) BaseWordRecord.ReadUInt32(stream);
    int num2 = stream.ReadByte();
    this.m_ilvl = num2 & 15;
    this.m_bStartAt = (num2 & 16 /*0x10*/) != 0;
    this.m_bFormatting = (num2 & 32 /*0x20*/) != 0;
    this.m_reserved1 = stream.ReadByte();
    this.m_reserved2 = stream.ReadByte();
    this.m_reserved3 = stream.ReadByte();
    if (this.m_bFormatting)
    {
      this.m_lvl = new ListLevel(stream);
    }
    else
    {
      if (!this.m_bStartAt)
        return;
      this.m_startAt = num1;
    }
  }

  internal void Write(Stream stream)
  {
    BaseWordRecord.WriteUInt32(stream, (uint) this.m_startAt);
    int num = 0 | this.m_ilvl | (this.m_bStartAt ? 16 /*0x10*/ : 0) | (this.m_bFormatting ? 32 /*0x20*/ : 0);
    stream.WriteByte((byte) num);
    stream.WriteByte((byte) this.m_reserved1);
    stream.WriteByte((byte) this.m_reserved2);
    stream.WriteByte((byte) this.m_reserved3);
    if (!this.m_bFormatting)
      return;
    this.m_lvl.Write(stream);
  }
}
