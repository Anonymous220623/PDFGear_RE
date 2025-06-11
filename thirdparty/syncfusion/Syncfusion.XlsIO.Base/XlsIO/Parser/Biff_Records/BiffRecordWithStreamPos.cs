// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.BiffRecordWithStreamPos
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
public class BiffRecordWithStreamPos : BiffRecordRaw
{
  protected long m_lStreamPosition;

  protected BiffRecordWithStreamPos()
  {
  }

  protected BiffRecordWithStreamPos(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  protected BiffRecordWithStreamPos(BinaryReader reader, out int itemSize)
    : base(reader, out itemSize)
  {
  }

  protected BiffRecordWithStreamPos(int iReserve)
    : base(iReserve)
  {
  }

  public override long StreamPos
  {
    get => this.m_lStreamPosition;
    set => this.m_lStreamPosition = value;
  }
}
