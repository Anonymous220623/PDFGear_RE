// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures.BXStructure
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;

[CLSCompliant(false)]
[StructLayout(LayoutKind.Sequential)]
internal class BXStructure : IDataStructure
{
  internal const int DEF_RECORD_SIZE = 13;
  private byte m_btOffset;
  private ParagraphHeight m_height = new ParagraphHeight();

  internal BXStructure()
  {
  }

  internal byte Offset
  {
    get => this.m_btOffset;
    set => this.m_btOffset = value;
  }

  internal ParagraphHeight Height => this.m_height;

  public int Length => 13;

  public void Parse(byte[] arrData, int iOffset)
  {
    this.m_btOffset = arrData[iOffset];
    ++iOffset;
  }

  public int Save(byte[] arrData, int iOffset)
  {
    arrData[iOffset] = this.m_btOffset;
    ++iOffset;
    return this.m_height.Save(arrData, iOffset) + 1;
  }

  internal void Save(BinaryWriter writer)
  {
    writer.Write(this.m_btOffset);
    ParagraphHeightStructure structure = this.m_height.Structure;
    writer.Write(structure.Options);
    writer.Write(structure.Width);
    writer.Write(structure.Height);
  }
}
