// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures.DOP2007
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;

[CLSCompliant(false)]
internal class DOP2007
{
  private ushort m_flagsA = 1059;
  private DopMth m_dopMath;
  private DOPDescriptor m_dopBase;

  internal bool RMTrackFormatting
  {
    get => ((int) this.m_flagsA & 1) != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65534 | (value ? 1 : 0));
  }

  internal bool RMTrackMoves
  {
    get => ((int) this.m_flagsA & 2) >> 1 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65533 | (value ? 1 : 0) << 1);
  }

  internal byte Ssm
  {
    get => (byte) (((int) this.m_flagsA & 480) >> 5);
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65055 | (int) value << 5);
  }

  internal bool ReadingModeInkLockDownActualPage
  {
    get => ((int) this.m_flagsA & 512 /*0x0200*/) >> 9 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65023 | (value ? 1 : 0) << 9);
  }

  internal bool AutoCompressPictures
  {
    get => ((int) this.m_flagsA & 1024 /*0x0400*/) >> 10 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 64511 | (value ? 1 : 0) << 10);
  }

  internal DopMth DopMath
  {
    get
    {
      if (this.m_dopMath == null)
        this.m_dopMath = new DopMth();
      return this.m_dopMath;
    }
  }

  internal DOP2007(DOPDescriptor dopBase) => this.m_dopBase = dopBase;

  internal void Parse(Stream stream)
  {
    int num1 = (int) BaseWordRecord.ReadUInt32(stream);
    this.m_flagsA = BaseWordRecord.ReadUInt16(stream);
    int num2 = (int) BaseWordRecord.ReadUInt16(stream);
    int num3 = (int) BaseWordRecord.ReadUInt32(stream);
    int num4 = (int) BaseWordRecord.ReadUInt32(stream);
    int num5 = (int) BaseWordRecord.ReadUInt32(stream);
    int num6 = (int) BaseWordRecord.ReadUInt32(stream);
    this.DopMath.Parse(stream);
  }

  internal void Write(Stream stream)
  {
    BaseWordRecord.WriteUInt32(stream, 0U);
    BaseWordRecord.WriteUInt16(stream, this.m_flagsA);
    BaseWordRecord.WriteUInt16(stream, (ushort) 0);
    BaseWordRecord.WriteUInt32(stream, 0U);
    BaseWordRecord.WriteUInt32(stream, 0U);
    BaseWordRecord.WriteUInt32(stream, 0U);
    BaseWordRecord.WriteUInt32(stream, 0U);
    this.DopMath.Write(stream);
  }
}
