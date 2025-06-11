// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.TextBoxStoryDescriptor
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class TextBoxStoryDescriptor : BaseWordRecord
{
  internal static int DEF_TXBX_LENGTH = 22;
  private int m_cTxbxAndiNextReuse;
  private int m_cReusable;
  private bool m_fReusable;
  private uint m_reserved;
  private int m_lid;
  private int m_txidUndo;

  internal int TextBoxCnt
  {
    get => this.m_cTxbxAndiNextReuse;
    set => this.m_cTxbxAndiNextReuse = value;
  }

  internal int ReusableCnt
  {
    get => this.m_cReusable;
    set => this.m_cReusable = value;
  }

  internal bool IsReusable
  {
    get => this.m_fReusable;
    set => this.m_fReusable = value;
  }

  internal int ShapeIdent
  {
    get => this.m_lid;
    set => this.m_lid = value;
  }

  internal uint Reserved
  {
    get => this.m_reserved;
    set => this.m_reserved = value;
  }

  internal TextBoxStoryDescriptor()
  {
  }

  internal TextBoxStoryDescriptor(Stream stream) => this.Read(stream);

  internal void Read(Stream stream)
  {
    this.m_cTxbxAndiNextReuse = BaseWordRecord.ReadInt32(stream);
    this.m_cReusable = BaseWordRecord.ReadInt32(stream);
    this.m_fReusable = BaseWordRecord.ReadInt16(stream) == (short) 1;
    this.m_reserved = BaseWordRecord.ReadUInt32(stream);
    this.m_lid = BaseWordRecord.ReadInt32(stream);
    this.m_txidUndo = BaseWordRecord.ReadInt32(stream);
  }

  internal void Write(Stream stream)
  {
    BaseWordRecord.WriteInt32(stream, this.m_cTxbxAndiNextReuse);
    BaseWordRecord.WriteInt32(stream, this.m_cReusable);
    if (this.m_fReusable)
      BaseWordRecord.WriteInt16(stream, (short) 1);
    else
      BaseWordRecord.WriteInt16(stream, (short) 0);
    BaseWordRecord.WriteInt32(stream, (int) this.m_reserved);
    BaseWordRecord.WriteInt32(stream, this.m_lid);
    BaseWordRecord.WriteInt32(stream, this.m_txidUndo);
  }
}
