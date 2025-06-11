// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.StatePositionsBase
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal abstract class StatePositionsBase
{
  protected int m_iStartItemPos;
  protected int m_iEndItemPos;
  protected int m_iItemIndex;
  protected long m_iEndCHPxPos;
  internal long m_iEndPAPxPos;
  internal long m_iEndPieceTablePos;
  internal long m_iStartPieceTablePos;
  protected int m_iStartText;
  protected int m_iEndText;
  protected WordFKPData m_fkp;
  protected BookmarkInfo[] m_bookmarks;
  private int m_curTextPosition;
  private int m_iCurrentPapxFKPIndex;
  private int m_iCurrentChpxFKPIndex;
  private int m_iCurrentPapxIndex;
  private int m_iCurrentChpxIndex;

  internal int StartItemPos => this.m_iStartItemPos;

  internal int EndItemPos => this.m_iEndItemPos;

  internal int ItemIndex
  {
    get => this.m_iItemIndex;
    set => this.m_iItemIndex = value;
  }

  internal int StartText
  {
    get => this.m_iStartText;
    set => this.m_iStartText = value;
  }

  internal CharacterPropertyException CurrentChpx
  {
    get
    {
      return this.m_fkp.GetChpxPage(this.m_iCurrentChpxFKPIndex).CharacterProperties[this.m_iCurrentChpxIndex];
    }
  }

  internal ParagraphPropertyException CurrentPapx
  {
    get
    {
      return (ParagraphPropertyException) this.m_fkp.GetPapxPage(this.m_iCurrentPapxFKPIndex).ParagraphProperties[this.m_iCurrentPapxIndex];
    }
  }

  internal int CurrentTextPosition
  {
    get => this.m_curTextPosition;
    set => this.m_curTextPosition = value;
  }

  protected WPTablesData Tables => this.m_fkp.Tables;

  protected bool IsCurrentPapxPosition
  {
    get
    {
      uint fileCharPo1 = this.CurrentPapxPage.FileCharPos[this.m_iCurrentPapxIndex];
      uint fileCharPo2 = this.CurrentPapxPage.FileCharPos[this.m_iCurrentPapxIndex + 1];
      return (long) this.m_iStartText >= (long) fileCharPo1 && (long) this.m_iStartText <= (long) fileCharPo2;
    }
  }

  protected bool IsCurrentChpxPosition
  {
    get
    {
      uint fileCharPo1 = this.CurrentChpxPage.FileCharPos[this.m_iCurrentChpxIndex];
      uint fileCharPo2 = this.CurrentChpxPage.FileCharPos[this.m_iCurrentChpxIndex + 1];
      return (long) this.m_iStartText >= (long) fileCharPo1 && (long) this.m_iStartText <= (long) fileCharPo2;
    }
  }

  private CharacterPropertiesPage CurrentChpxPage
  {
    get => this.m_fkp.GetChpxPage(this.m_iCurrentChpxFKPIndex);
  }

  private ParagraphPropertiesPage CurrentPapxPage
  {
    get => this.m_fkp.GetPapxPage(this.m_iCurrentPapxFKPIndex);
  }

  internal StatePositionsBase(WordFKPData fkp) => this.m_fkp = fkp;

  internal virtual void InitStartEndPos()
  {
    this.m_iStartText = (int) this.Tables.ConvertCharPosToFileCharPos(0U);
    this.m_iEndText = (int) this.Tables.ConvertCharPosToFileCharPos((uint) this.m_fkp.Fib.CcpText);
    this.m_iEndPAPxPos = (long) (int) this.m_fkp.GetPapxPage(0).FileCharPos[1];
    this.m_iEndCHPxPos = (long) (int) this.m_fkp.GetChpxPage(0).FileCharPos[1];
  }

  internal bool NextChpx()
  {
    CharacterPropertiesPage currentChpxPage = this.CurrentChpxPage;
    if (this.m_iCurrentChpxIndex < currentChpxPage.RunsCount - 1)
    {
      ++this.m_iCurrentChpxIndex;
    }
    else
    {
      if (this.m_iCurrentChpxFKPIndex + 1 > this.m_fkp.Tables.CHPXBinaryTable.EntriesCount - 1)
      {
        this.m_iEndCHPxPos = -1L;
        return false;
      }
      ++this.m_iCurrentChpxFKPIndex;
      this.m_iCurrentChpxIndex = 0;
      currentChpxPage = this.CurrentChpxPage;
    }
    this.m_iEndCHPxPos = (long) (int) currentChpxPage.FileCharPos[this.m_iCurrentChpxIndex + 1];
    return true;
  }

  internal bool NextPapx()
  {
    ParagraphPropertiesPage currentPapxPage = this.CurrentPapxPage;
    if (this.m_iCurrentPapxIndex < currentPapxPage.RunsCount - 1)
    {
      ++this.m_iCurrentPapxIndex;
    }
    else
    {
      if (this.m_iCurrentPapxFKPIndex + 1 > this.m_fkp.Tables.PAPXBinaryTable.EntriesCount - 1)
      {
        this.m_iEndPAPxPos = -1L;
        return false;
      }
      ++this.m_iCurrentPapxFKPIndex;
      this.m_iCurrentPapxIndex = 0;
      currentPapxPage = this.CurrentPapxPage;
    }
    this.m_iEndPAPxPos = (long) (int) currentPapxPage.FileCharPos[this.m_iCurrentPapxIndex + 1];
    return true;
  }

  internal virtual long GetMinEndPos(long curPos)
  {
    int index1 = this.m_iEndPieceTablePos != 0L ? this.Tables.PieceTablePositions.IndexOf((uint) this.m_iEndPieceTablePos) : 0;
    long num1 = index1 > 0 ? (long) (this.Tables.m_pieceTable.FileCharacterPos[index1] - this.Tables.m_pieceTable.FileCharacterPos[index1 - 1]) * (long) this.Tables.m_pieceTableEncodings[index1 - 1].GetByteCount("a") : 0L;
    if (this.m_iEndPieceTablePos <= curPos || curPos == this.m_iStartPieceTablePos + num1)
    {
      List<uint> pieceTablePositions = this.Tables.PieceTablePositions;
      int index2 = index1;
      for (int index3 = pieceTablePositions.Count - 1; index2 < index3; ++index2)
      {
        uint num2 = pieceTablePositions[index2];
        uint num3 = pieceTablePositions[index2 + 1];
        if (curPos < (long) num3)
        {
          this.m_iStartPieceTablePos = (long) num2;
          this.m_iEndPieceTablePos = (long) (int) num3;
          ++index1;
          break;
        }
      }
    }
    long minEndPos = this.m_iEndCHPxPos != -1L && this.m_iStartPieceTablePos > this.m_iEndCHPxPos || this.m_iEndPAPxPos != -1L && this.m_iStartPieceTablePos > this.m_iEndPAPxPos ? Math.Min(this.m_iStartPieceTablePos, this.m_iEndPieceTablePos) : Math.Min(Math.Min(this.m_iEndCHPxPos, this.m_iEndPAPxPos), this.m_iEndPieceTablePos);
    long num4 = this.m_iStartPieceTablePos + (index1 > 0 ? (long) (this.Tables.m_pieceTable.FileCharacterPos[index1] - this.Tables.m_pieceTable.FileCharacterPos[index1 - 1]) * (long) this.Tables.m_pieceTableEncodings[index1 - 1].GetByteCount("a") : 0L);
    if (minEndPos > num4 && curPos < num4)
      minEndPos = num4;
    return minEndPos;
  }

  internal bool UpdateCHPxEndPos(long iEndPos) => iEndPos >= this.m_iEndCHPxPos && this.NextChpx();

  internal bool UpdatePAPxEndPos(long iEndPos) => iEndPos >= this.m_iEndPAPxPos && this.NextPapx();

  internal bool IsFirstPass(long iPos) => iPos < (long) this.m_iStartText;

  internal bool IsEndOfText(long iPos) => iPos >= (long) this.m_iEndText;

  internal virtual bool UpdateItemEndPos(long iEndPos) => true;

  internal virtual int MoveToItem(int itemIndex) => 0;

  internal virtual bool IsEndOfSubdocItemText(long iPos) => false;

  protected void MoveToCurrentChpxPapx()
  {
    if (!this.IsCurrentPapxPosition)
    {
      while ((long) this.m_iStartText >= this.m_iEndPAPxPos && this.NextPapx())
        ;
    }
    if (this.IsCurrentChpxPosition)
      return;
    do
      ;
    while ((long) this.m_iStartText >= this.m_iEndCHPxPos && this.NextChpx());
  }
}
