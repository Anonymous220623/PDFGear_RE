// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.FieldDescriptor
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class FieldDescriptor : BaseWordRecord
{
  private byte m_ch;
  private byte m_reserved;
  private byte m_fieldType;

  internal bool HasSeparator
  {
    get => ((int) this.m_fieldType & 128 /*0x80*/) != 0;
    set => this.m_fieldType = (byte) BaseWordRecord.SetBit((int) this.m_fieldType, 7, value);
  }

  internal bool IsResultDirty
  {
    get => ((int) this.m_fieldType & 4) != 0;
    set => this.m_fieldType = (byte) BaseWordRecord.SetBit((int) this.m_fieldType, 2, value);
  }

  internal bool IsResultEdited
  {
    get => ((int) this.m_fieldType & 8) != 0;
    set => this.m_fieldType = (byte) BaseWordRecord.SetBit((int) this.m_fieldType, 3, value);
  }

  internal bool IsLocked
  {
    get => this.m_ch == (byte) 21 && BaseWordRecord.GetBit(this.m_fieldType, 4);
    set => this.m_fieldType = (byte) BaseWordRecord.SetBit((int) this.m_fieldType, 4, value);
  }

  internal bool IsNested
  {
    get => ((int) this.m_fieldType & 64 /*0x40*/) != 0;
    set => this.m_fieldType = (byte) BaseWordRecord.SetBit((int) this.m_fieldType, 6, value);
  }

  internal FieldType Type
  {
    get => (FieldType) this.m_fieldType;
    set => this.m_fieldType = (byte) value;
  }

  internal byte FieldBoundary
  {
    get => this.m_ch;
    set => this.m_ch = value;
  }

  internal FieldDescriptor(BinaryReader reader) => this.Read(reader);

  internal FieldDescriptor()
  {
  }

  internal void Parse(short sh)
  {
    byte[] bytes = BitConverter.GetBytes(sh);
    this.m_ch = (byte) ((uint) bytes[0] & 31U /*0x1F*/);
    this.m_reserved = (byte) ((uint) bytes[0] & 224U /*0xE0*/);
    this.m_fieldType = bytes[1];
  }

  internal short Save()
  {
    return BitConverter.ToInt16(new byte[2]
    {
      (byte) ((uint) this.m_ch | (uint) this.m_reserved),
      this.m_fieldType
    }, 0);
  }

  internal FieldDescriptor Clone()
  {
    return new FieldDescriptor()
    {
      HasSeparator = this.HasSeparator,
      IsNested = this.IsNested,
      IsResultDirty = this.IsResultDirty,
      IsResultEdited = this.IsResultEdited,
      FieldBoundary = this.FieldBoundary,
      Type = this.Type
    };
  }

  internal void Read(BinaryReader reader)
  {
    byte[] numArray = reader.ReadBytes(2);
    this.m_ch = (byte) ((uint) numArray[0] & 31U /*0x1F*/);
    this.m_reserved = (byte) ((uint) numArray[0] & 224U /*0xE0*/);
    this.m_fieldType = numArray[1];
  }

  internal void Write(Stream stream)
  {
    stream.WriteByte((byte) ((uint) this.m_ch | (uint) this.m_reserved));
    stream.WriteByte(this.m_fieldType);
  }
}
