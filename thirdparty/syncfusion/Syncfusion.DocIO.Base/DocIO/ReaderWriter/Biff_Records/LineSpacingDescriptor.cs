// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.LineSpacingDescriptor
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

internal class LineSpacingDescriptor
{
  private short m_dyaLine;
  private bool m_fMultLinespace;

  internal short LineSpacing
  {
    get => this.m_dyaLine;
    set
    {
      switch (this.LineSpacingRule)
      {
        case LineSpacingRule.AtLeast:
          this.m_dyaLine = value;
          break;
        case LineSpacingRule.Exactly:
          this.m_dyaLine = -value;
          break;
        default:
          throw new Exception("Trying to set unsupported line spacing rule.");
      }
    }
  }

  internal LineSpacingRule LineSpacingRule
  {
    get
    {
      return this.m_fMultLinespace ? (this.m_dyaLine <= (short) 0 ? LineSpacingRule.Exactly : LineSpacingRule.Multiple) : (this.m_dyaLine < (short) 0 ? LineSpacingRule.Exactly : LineSpacingRule.AtLeast);
    }
    set
    {
      switch (value)
      {
        case LineSpacingRule.AtLeast:
          this.m_fMultLinespace = false;
          this.m_dyaLine = Math.Abs(this.m_dyaLine);
          break;
        case LineSpacingRule.Exactly:
          this.m_fMultLinespace = false;
          this.m_dyaLine = -Math.Abs(this.m_dyaLine);
          break;
        case LineSpacingRule.Multiple:
          this.m_fMultLinespace = true;
          this.m_dyaLine = Math.Abs(this.m_dyaLine);
          break;
      }
    }
  }

  internal LineSpacingDescriptor()
  {
  }

  internal LineSpacingDescriptor(byte[] operand) => this.Parse(operand);

  internal void Parse(byte[] operand)
  {
    this.m_dyaLine = BitConverter.ToInt16(operand, 0);
    this.m_fMultLinespace = BitConverter.ToInt16(operand, 2) != (short) 0;
  }

  internal byte[] Save()
  {
    byte[] numArray = new byte[4];
    BitConverter.GetBytes(this.m_dyaLine).CopyTo((Array) numArray, 0);
    numArray[2] = this.m_fMultLinespace ? (byte) 1 : (byte) 0;
    return numArray;
  }
}
