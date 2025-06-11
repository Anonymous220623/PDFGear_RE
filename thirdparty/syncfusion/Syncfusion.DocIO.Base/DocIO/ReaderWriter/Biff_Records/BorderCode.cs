// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.BorderCode
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using System;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class BorderCode : BaseWordRecord
{
  private int DEF_NEW_BRC_LENGTH = 8;
  private byte m_dptLineWidth;
  private byte m_brcType;
  private byte m_colorId;
  private Color m_colorExt = Color.Empty;
  private byte m_props;
  private byte m_bFlags = 1;

  internal BorderCode()
  {
  }

  internal BorderCode(byte[] arr, int iOffset) => this.Parse(arr, iOffset);

  internal void Parse(byte[] arr, int iOffset)
  {
    this.m_dptLineWidth = arr[iOffset];
    this.m_brcType = arr[iOffset + 1];
    this.m_colorId = arr[iOffset + 2];
    this.m_props = arr[iOffset + 3];
    this.IsDefault = false;
  }

  internal void ParseNewBrc(byte[] arr, int iOffset)
  {
    if (arr.Length - iOffset < this.DEF_NEW_BRC_LENGTH)
      return;
    this.m_colorExt = WordColor.ConvertRGBToColor(BitConverter.ToUInt32(arr, iOffset));
    this.m_dptLineWidth = arr[iOffset + 4];
    this.m_brcType = arr[iOffset + 5];
    this.m_props = arr[iOffset + 6];
    int num = (int) arr[iOffset + 7];
    this.IsDefault = false;
  }

  internal void SaveBytes(byte[] arr, int iOffset)
  {
    arr[iOffset] = this.m_dptLineWidth;
    arr[iOffset + 1] = this.m_brcType;
    arr[iOffset + 2] = this.m_colorId;
    arr[iOffset + 3] = this.m_props;
    this.IsDefault = false;
  }

  internal void SaveNewBrc(byte[] arr, int iOffset)
  {
    BitConverter.GetBytes(WordColor.ConvertColorToRGB(this.m_colorExt)).CopyTo((Array) arr, iOffset);
    arr[iOffset + 4] = this.m_dptLineWidth;
    arr[iOffset + 5] = this.m_brcType;
    arr[iOffset + 6] = this.m_props;
    arr[iOffset + 7] = (byte) 0;
    this.IsDefault = false;
  }

  internal void Read(BinaryReader reader)
  {
    this.m_dptLineWidth = reader.ReadByte();
    if (this.m_dptLineWidth != byte.MaxValue)
    {
      this.m_brcType = reader.ReadByte();
      this.m_colorId = reader.ReadByte();
      this.m_props = reader.ReadByte();
      this.IsDefault = false;
    }
    else
    {
      int num1 = (int) reader.ReadByte();
      int num2 = (int) reader.ReadByte();
      int num3 = (int) reader.ReadByte();
    }
  }

  internal void Write(Stream stream)
  {
    if (!this.IsClear)
    {
      stream.WriteByte(this.m_dptLineWidth);
      stream.WriteByte(this.m_brcType);
      stream.WriteByte(this.m_colorId);
      stream.WriteByte(this.m_props);
    }
    else
      BaseWordRecord.WriteUInt32(stream, uint.MaxValue);
  }

  internal BorderCode Clone() => this.MemberwiseClone() as BorderCode;

  internal byte LineWidth
  {
    get => this.m_dptLineWidth;
    set
    {
      this.m_dptLineWidth = value;
      this.IsDefault = false;
    }
  }

  internal byte BorderType
  {
    get => this.m_brcType;
    set
    {
      this.m_brcType = value;
      this.IsDefault = false;
    }
  }

  internal byte Space
  {
    get => (byte) ((uint) this.m_props & 31U /*0x1F*/);
    set
    {
      byte num = value;
      this.m_props &= (byte) 224 /*0xE0*/;
      this.m_props += num;
      this.IsDefault = false;
    }
  }

  internal bool Shadow
  {
    get => (byte) ((uint) (byte) ((uint) this.m_props & 32U /*0x20*/) >> 5) == (byte) 1;
    set
    {
      byte num = value ? (byte) 1 : (byte) 0;
      this.m_props &= (byte) 223;
      this.m_props += (byte) ((uint) num << 5);
      this.IsDefault = false;
    }
  }

  internal byte LineColor
  {
    get => this.m_colorId;
    set
    {
      this.m_colorId = value;
      this.IsDefault = false;
    }
  }

  internal Color LineColorExt
  {
    get
    {
      return this.m_colorExt == Color.Empty ? WordColor.ConvertIdToColor((int) this.m_colorId) : this.m_colorExt;
    }
    set
    {
      this.m_colorExt = value;
      this.IsDefault = false;
    }
  }

  internal bool IsDefault
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal bool IsClear => this.m_dptLineWidth == byte.MaxValue;
}
