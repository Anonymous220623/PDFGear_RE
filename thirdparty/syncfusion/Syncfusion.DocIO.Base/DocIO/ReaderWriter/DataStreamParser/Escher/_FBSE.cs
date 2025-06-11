// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher._FBSE
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

internal class _FBSE : BaseWordRecord
{
  public const int DEF_GUID_LENGTH = 16 /*0x10*/;
  public const int DEF_FBSE_LENGTH = 36;
  internal int m_btWin32;
  internal int m_btMacOS;
  internal byte[] m_rgbUid;
  internal int m_tag;
  internal int m_size;
  internal int m_cRef;
  internal int m_foDelay;
  internal int m_usage;
  internal int m_cbName;
  internal int m_unused2;
  internal int m_unused3;

  public void Read(Stream stream)
  {
    this.m_btWin32 = stream.ReadByte();
    this.m_btMacOS = stream.ReadByte();
    this.m_rgbUid = this.ReadBytes(stream, 16 /*0x10*/);
    this.m_tag = (int) BaseWordRecord.ReadInt16(stream);
    this.m_size = BaseWordRecord.ReadInt32(stream);
    this.m_cRef = BaseWordRecord.ReadInt32(stream);
    this.m_foDelay = BaseWordRecord.ReadInt32(stream);
    this.m_usage = stream.ReadByte();
    this.m_cbName = stream.ReadByte();
    this.m_unused2 = stream.ReadByte();
    this.m_unused3 = stream.ReadByte();
    if (this.m_cbName > 0)
      throw new NotImplementedException("A BLIP with a name was found.");
  }

  public void Write(Stream stream)
  {
    stream.WriteByte((byte) this.m_btWin32);
    stream.WriteByte((byte) this.m_btMacOS);
    stream.Write(this.m_rgbUid, 0, 16 /*0x10*/);
    BaseWordRecord.WriteInt16(stream, (short) this.m_tag);
    BaseWordRecord.WriteInt32(stream, this.m_size);
    BaseWordRecord.WriteInt32(stream, this.m_cRef);
    BaseWordRecord.WriteInt32(stream, this.m_foDelay);
    stream.WriteByte((byte) this.m_usage);
    stream.WriteByte((byte) this.m_cbName);
    stream.WriteByte((byte) this.m_unused2);
    stream.WriteByte((byte) this.m_unused3);
  }

  public _FBSE Clone()
  {
    _FBSE fbse = (_FBSE) this.MemberwiseClone();
    fbse.m_rgbUid = new byte[this.m_rgbUid.Length];
    this.m_rgbUid.CopyTo((Array) fbse.m_rgbUid, 0);
    return fbse;
  }
}
