// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.MainStatePositions
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal class MainStatePositions : StatePositionsBase
{
  private int m_iEndSecPos;
  private int m_iCurrentSepxIndex;

  internal int SectionIndex => this.m_iCurrentSepxIndex;

  internal SectionPropertyException CurrentSepx => this.m_fkp.GetSepx(this.m_iCurrentSepxIndex);

  internal MainStatePositions(WordFKPData fkp)
    : base(fkp)
  {
  }

  internal bool NextSepx(out int iEndPos)
  {
    bool flag = true;
    if (this.m_iCurrentSepxIndex < this.Tables.SectionsTable.Positions.Length - 2)
      ++this.m_iCurrentSepxIndex;
    else
      flag = false;
    iEndPos = this.Tables.SectionsTable.Positions[this.m_iCurrentSepxIndex + 1];
    return flag;
  }

  internal bool UpdateSepxEndPos(long iEndPos)
  {
    bool flag = false;
    if (iEndPos >= (long) this.m_iEndSecPos)
    {
      int iEndPos1;
      if (this.NextSepx(out iEndPos1))
      {
        this.m_iEndSecPos = (int) this.Tables.ConvertCharPosToFileCharPos((uint) iEndPos1);
        flag = true;
      }
      else
        this.m_iEndSecPos = -1;
    }
    return flag;
  }

  internal override void InitStartEndPos()
  {
    base.InitStartEndPos();
    this.m_iEndSecPos = (int) this.Tables.ConvertCharPosToFileCharPos((uint) this.Tables.SectionsTable.Positions[1]);
  }

  internal override long GetMinEndPos(long curPos)
  {
    return Math.Min(base.GetMinEndPos(curPos), (long) this.m_iEndSecPos);
  }
}
