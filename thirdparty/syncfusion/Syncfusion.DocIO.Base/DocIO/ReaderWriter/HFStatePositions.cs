// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.HFStatePositions
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal class HFStatePositions : StatePositionsBase
{
  private int m_iSectionIndex;
  private bool m_isNextItemText;

  internal int SectionIndex
  {
    get => this.m_iSectionIndex;
    set => this.m_iSectionIndex = value;
  }

  internal HFStatePositions(WordFKPData fkp)
    : base(fkp)
  {
  }

  internal override int MoveToItem(int itemIndex)
  {
    this.m_iItemIndex = this.m_iSectionIndex * 6 + itemIndex;
    int charPos = this.m_fkp.Fib.CcpText + this.m_fkp.Fib.CcpFtn;
    if (this.m_iStartItemPos == 0)
    {
      this.m_iStartItemPos = (int) this.m_fkp.Tables.ConvertCharPosToFileCharPos((uint) charPos);
      this.m_iEndText = (int) this.m_fkp.Tables.ConvertCharPosToFileCharPos((uint) (charPos + this.m_fkp.Fib.CcpHdd));
    }
    this.m_iStartText = (int) this.m_fkp.Tables.ConvertCharPosToFileCharPos((uint) (charPos + this.m_fkp.Tables.HeaderFooterCharPosTable.Positions[this.m_iItemIndex]));
    this.m_iEndItemPos = (int) this.m_fkp.Tables.ConvertCharPosToFileCharPos((uint) (charPos + this.m_fkp.Tables.HeaderFooterCharPosTable.Positions[this.m_iItemIndex + 1]));
    this.MoveToCurrentChpxPapx();
    return this.m_iStartText;
  }

  internal void MoveToNextHeaderPos()
  {
    ++this.m_iItemIndex;
    this.m_iEndItemPos = (int) this.m_fkp.Tables.ConvertCharPosToFileCharPos((uint) (this.m_fkp.Fib.CcpText + this.m_fkp.Fib.CcpFtn + this.m_fkp.Tables.HeaderFooterCharPosTable.Positions[this.m_iItemIndex + 1]));
  }

  internal bool UpdateHeaderEndPos(long iEndPos, HeaderType headerType)
  {
    this.m_isNextItemText = iEndPos >= (long) this.m_iEndItemPos && headerType < HeaderType.FirstPageFooter;
    if (this.m_isNextItemText)
    {
      ++headerType;
      this.MoveToNextHeaderPos();
    }
    else if (iEndPos >= (long) this.m_iEndItemPos)
      this.m_iEndItemPos = -1;
    return this.m_isNextItemText;
  }

  internal override bool IsEndOfSubdocItemText(long iPos) => this.m_isNextItemText;
}
