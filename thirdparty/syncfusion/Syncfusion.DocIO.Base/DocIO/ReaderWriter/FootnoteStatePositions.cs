// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.FootnoteStatePositions
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal class FootnoteStatePositions : StatePositionsBase
{
  internal FootnoteStatePositions(WordFKPData fkp)
    : base(fkp)
  {
  }

  internal override int MoveToItem(int itemIndex)
  {
    this.m_iItemIndex = itemIndex;
    int ccpText = this.m_fkp.Fib.CcpText;
    if (this.m_iStartItemPos == 0)
    {
      this.m_iStartItemPos = (int) this.m_fkp.Tables.ConvertCharPosToFileCharPos((uint) ccpText);
      this.m_iEndText = (int) this.m_fkp.Tables.ConvertCharPosToFileCharPos((uint) (ccpText + this.m_fkp.Fib.CcpFtn));
    }
    this.m_iStartText = (int) this.m_fkp.Tables.ConvertCharPosToFileCharPos((uint) (ccpText + this.m_fkp.Tables.Footnotes.GetTxtPosition(this.m_iItemIndex)));
    this.m_iEndItemPos = (int) this.m_fkp.Tables.ConvertCharPosToFileCharPos((uint) (ccpText + this.m_fkp.Tables.Footnotes.GetTxtPosition(this.m_iItemIndex + 1)));
    this.MoveToCurrentChpxPapx();
    return this.m_iStartText;
  }

  internal override bool UpdateItemEndPos(long iEndPos)
  {
    if (iEndPos < (long) this.m_iEndItemPos)
      return false;
    uint ccpText = (uint) this.m_fkp.Fib.CcpText;
    ++this.m_iItemIndex;
    this.m_iEndItemPos = (int) this.m_fkp.Tables.ConvertCharPosToFileCharPos((uint) ((ulong) ccpText + (ulong) this.m_fkp.Tables.Footnotes.GetTxtPosition(this.m_iItemIndex + 1)));
    return true;
  }
}
