// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.ColumnDescriptor
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class ColumnDescriptor
{
  private const ushort DEF_WIDTH = 1000;
  private const ushort DEF_SPACE = 720;
  private SinglePropertyModifierRecord m_widthRecord;
  private SinglePropertyModifierRecord m_spaceRecord;

  internal ColumnDescriptor(SinglePropertyModifierRecord width, SinglePropertyModifierRecord space)
  {
    this.m_widthRecord = width;
    this.m_spaceRecord = space;
    this.Width = (ushort) 1000;
    this.Space = (ushort) 720;
  }

  internal ColumnDescriptor()
  {
    this.m_widthRecord = new SinglePropertyModifierRecord(61955);
    this.m_widthRecord.ByteArray = new byte[3];
    this.m_spaceRecord = new SinglePropertyModifierRecord(61956);
    this.m_spaceRecord.ByteArray = new byte[3];
    this.Width = (ushort) 1000;
    this.Space = (ushort) 720;
  }

  internal ushort Width
  {
    get => BitConverter.ToUInt16(this.m_widthRecord.ByteArray, 1);
    set
    {
      byte[] bytes = BitConverter.GetBytes(value);
      this.m_widthRecord.ByteArray[1] = bytes[0];
      this.m_widthRecord.ByteArray[2] = bytes[1];
    }
  }

  internal ushort Space
  {
    get => BitConverter.ToUInt16(this.m_spaceRecord.ByteArray, 1);
    set
    {
      byte[] bytes = BitConverter.GetBytes(value);
      this.m_spaceRecord.ByteArray[1] = bytes[0];
      this.m_spaceRecord.ByteArray[2] = bytes[1];
    }
  }
}
