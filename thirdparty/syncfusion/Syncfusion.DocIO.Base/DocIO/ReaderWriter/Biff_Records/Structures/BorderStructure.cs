// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures.BorderStructure
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;

[CLSCompliant(false)]
[StructLayout(LayoutKind.Sequential)]
internal class BorderStructure : IDataStructure
{
  private const int DEF_RECORD_SIZE = 4;
  private byte m_dptLineWidth;
  private byte m_brcType;
  private byte m_color;
  private byte m_props;

  public BorderStructure(byte[] arr, int iOffset) => this.Parse(arr, iOffset);

  public BorderStructure()
  {
  }

  public byte LineWidth
  {
    get => this.m_dptLineWidth;
    set => this.m_dptLineWidth = value;
  }

  public byte BorderType
  {
    get => this.m_brcType;
    set => this.m_brcType = value;
  }

  public byte Space
  {
    get => (byte) ((uint) this.m_props & 31U /*0x1F*/);
    set
    {
      byte num = value;
      this.m_props &= (byte) 224 /*0xE0*/;
      this.m_props += num;
    }
  }

  public bool Shadow
  {
    get => (byte) ((uint) (byte) ((uint) this.m_props & 32U /*0x20*/) >> 5) == (byte) 1;
    set
    {
      byte num = value ? (byte) 1 : (byte) 0;
      this.m_props &= (byte) 223;
      this.m_props += (byte) ((uint) num << 5);
    }
  }

  public byte LineColor
  {
    get => this.m_color;
    set => this.m_color = value;
  }

  public bool IsClear => this.m_dptLineWidth == byte.MaxValue;

  internal byte Props
  {
    get => this.m_props;
    set => this.m_props = value;
  }

  public int Length => 4;

  internal BorderStructure Clone()
  {
    return new BorderStructure()
    {
      m_brcType = this.m_brcType,
      m_color = this.m_color,
      m_dptLineWidth = this.m_dptLineWidth,
      m_props = this.m_props
    };
  }

  public void Parse(byte[] arr, int iOffset)
  {
    this.m_dptLineWidth = arr[iOffset];
    this.m_brcType = arr[iOffset + 1];
    this.m_color = arr[iOffset + 2];
    this.m_props = arr[iOffset + 3];
  }

  public int Save(byte[] arr, int iOffset)
  {
    arr[iOffset] = this.m_dptLineWidth;
    arr[iOffset + 1] = this.m_brcType;
    arr[iOffset + 2] = this.m_color;
    arr[iOffset + 3] = this.m_props;
    return 4;
  }
}
