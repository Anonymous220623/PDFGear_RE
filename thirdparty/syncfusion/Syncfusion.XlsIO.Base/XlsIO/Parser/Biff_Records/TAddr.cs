// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.TAddr
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
public struct TAddr
{
  private int m_iFirstRow;
  private int m_iLastRow;
  private int m_iFirstCol;
  private int m_iLastCol;

  public TAddr(int iFirstRow, int iFirstCol, int iLastRow, int iLastCol)
  {
    this.m_iFirstRow = iFirstRow;
    this.m_iFirstCol = iFirstCol;
    this.m_iLastRow = iLastRow;
    this.m_iLastCol = iLastCol;
  }

  public TAddr(int iTopLeftIndex, int iBottomRightIndex)
  {
    this.m_iFirstRow = RangeImpl.GetRowFromCellIndex((long) iTopLeftIndex);
    this.m_iFirstCol = RangeImpl.GetColumnFromCellIndex((long) iTopLeftIndex);
    this.m_iLastRow = RangeImpl.GetRowFromCellIndex((long) iBottomRightIndex);
    this.m_iLastCol = RangeImpl.GetColumnFromCellIndex((long) iBottomRightIndex);
  }

  public TAddr(Rectangle rect)
  {
    this.m_iFirstCol = rect.X;
    this.m_iFirstRow = rect.Y;
    this.m_iLastCol = rect.Right;
    this.m_iLastRow = rect.Bottom;
  }

  public int FirstCol
  {
    get => this.m_iFirstCol;
    set => this.m_iFirstCol = value;
  }

  public int FirstRow
  {
    get => this.m_iFirstRow;
    set => this.m_iFirstRow = value;
  }

  public int LastCol
  {
    get => this.m_iLastCol;
    set => this.m_iLastCol = value;
  }

  public int LastRow
  {
    get => this.m_iLastRow;
    set => this.m_iLastRow = value;
  }

  public override string ToString()
  {
    return $"{base.ToString()} ( {this.m_iFirstRow.ToString()}, {this.m_iFirstCol.ToString()} ) - ( {this.m_iLastRow.ToString()}, {this.m_iLastCol.ToString()} )";
  }

  public Rectangle GetRectangle()
  {
    return Rectangle.FromLTRB(this.FirstCol, this.FirstRow, this.LastCol, this.LastRow);
  }
}
