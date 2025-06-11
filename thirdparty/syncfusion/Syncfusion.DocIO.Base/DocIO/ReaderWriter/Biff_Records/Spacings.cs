// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.Spacings
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class Spacings
{
  private short m_left = -1;
  private short m_right = -1;
  private short m_top = -1;
  private short m_bottom = -1;
  private byte m_cellNumber;

  internal short Left
  {
    get => this.m_left;
    set
    {
      if (value < (short) 0 || value > (short) 31680)
        this.m_left = (short) 0;
      else
        this.m_left = value;
    }
  }

  internal short Right
  {
    get => this.m_right;
    set
    {
      if (value < (short) 0 || value > (short) 31680)
        this.m_right = (short) 0;
      else
        this.m_right = value;
    }
  }

  internal short Top
  {
    get => this.m_top;
    set
    {
      if (value < (short) 0 || value > (short) 31680)
        this.m_top = (short) 0;
      else
        this.m_top = value;
    }
  }

  internal short Bottom
  {
    get => this.m_bottom;
    set
    {
      if (value < (short) 0 || value > (short) 31680)
        this.m_bottom = (short) 0;
      else
        this.m_bottom = value;
    }
  }

  internal int CellNumber
  {
    get => (int) this.m_cellNumber;
    set => this.m_cellNumber = (byte) value;
  }

  internal bool IsEmpty
  {
    get
    {
      return this.m_left == (short) -1 && this.m_top == (short) -1 && this.m_bottom == (short) -1 && this.m_right == (short) -1;
    }
  }

  internal Spacings()
  {
  }

  internal Spacings(SinglePropertyModifierRecord sprm) => this.Parse(sprm);

  internal void Parse(SinglePropertyModifierRecord sprm)
  {
    this.m_cellNumber = sprm.ByteArray[0];
    byte num = sprm.ByteArray[2];
    if (num > (byte) 15)
      return;
    switch (sprm.ByteArray[3])
    {
      case 0:
      case 3:
        short int16 = BitConverter.ToInt16(sprm.ByteArray, 4);
        if (((int) num & 1) != 0)
          this.Top = int16;
        if (((int) num & 2) != 0)
          this.Left = int16;
        if (((int) num & 4) != 0)
          this.Bottom = int16;
        if (((int) num & 8) == 0)
          break;
        this.Right = int16;
        break;
    }
  }

  internal void Save(SinglePropertyModifierArray modifierArray, int options, int cellNumber)
  {
    if (this.m_top != (short) -1)
      modifierArray.Add(this.SaveSingleRecord((byte) 1, this.m_top, options));
    if (this.m_left != (short) -1)
      modifierArray.Add(this.SaveSingleRecord((byte) 2, this.m_left, options));
    if (this.m_bottom != (short) -1)
      modifierArray.Add(this.SaveSingleRecord((byte) 4, this.m_bottom, options));
    if (this.m_right == (short) -1)
      return;
    modifierArray.Add(this.SaveSingleRecord((byte) 8, this.m_right, options));
  }

  internal Spacings Clone()
  {
    return new Spacings()
    {
      m_bottom = this.Bottom,
      m_cellNumber = (byte) this.CellNumber,
      m_left = this.Left,
      m_right = this.Right,
      m_top = this.Top
    };
  }

  private SinglePropertyModifierRecord SaveSingleRecord(byte type, short dist, int options)
  {
    byte[] numArray = new byte[6]
    {
      this.m_cellNumber,
      (byte) ((uint) this.m_cellNumber + 1U),
      type,
      (byte) 3,
      (byte) 0,
      (byte) 0
    };
    BitConverter.GetBytes(dist).CopyTo((Array) numArray, 4);
    return new SinglePropertyModifierRecord(options)
    {
      ByteArray = numArray
    };
  }
}
