// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.WordHeaderFooterWriter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal class WordHeaderFooterWriter : WordSubdocumentWriter
{
  private const int DEF_HEADER_INDEX = 7;
  protected HeaderType m_headerType;
  private int m_iItemIndex;
  private int m_iSectionIndex;

  internal WordHeaderFooterWriter(WordWriter mainWriter)
    : base(mainWriter)
  {
    this.m_type = WordSubdocument.HeaderFooter;
  }

  internal HeaderType HeaderType
  {
    get => this.m_headerType;
    set
    {
      if (value < this.m_headerType)
        throw new ArgumentOutOfRangeException($"HeaderType must be greater from {this.m_headerType}");
      this.ClosePrevHeaderTypes(value);
    }
  }

  public override void WriteDocumentEnd()
  {
    this.ClosePrevHeaderTypes(HeaderType.EvenFooter | HeaderType.FirstPageHeader);
    this.WriteChar('\r');
    this.m_docInfo.TablesData.HeaderPositions[this.m_iItemIndex] = this.GetTextPos() + 3;
  }

  internal void WriteSectionEnd()
  {
    this.HeaderType = HeaderType.EvenFooter | HeaderType.FirstPageHeader;
    ++this.m_iSectionIndex;
    this.m_iItemIndex = (this.m_iSectionIndex + 1) * 6 + 1;
    this.m_headerType = HeaderType.EvenHeader;
  }

  internal void ClosePrevSeparator()
  {
    int textPos = this.GetTextPos();
    if (textPos != this.m_docInfo.TablesData.HeaderPositions[this.m_iItemIndex - 1])
    {
      this.WriteChar('\r');
      this.m_docInfo.TablesData.HeaderPositions[this.m_iItemIndex] = this.GetTextPos();
    }
    else
      this.m_docInfo.TablesData.HeaderPositions[this.m_iItemIndex] = textPos;
    ++this.m_iItemIndex;
  }

  protected void ClosePrevHeaderTypes(HeaderType headerType)
  {
    while (this.m_headerType != headerType)
    {
      if (this.GetTextPos() != this.m_docInfo.TablesData.HeaderPositions[this.m_iItemIndex - 1])
      {
        this.WriteChar('\r');
        ++this.m_headerType;
        this.m_docInfo.TablesData.HeaderPositions[this.m_iItemIndex] = this.GetTextPos();
      }
      else
      {
        ++this.m_headerType;
        this.m_docInfo.TablesData.HeaderPositions[this.m_iItemIndex] = this.GetTextPos();
      }
      ++this.m_iItemIndex;
    }
  }

  protected override void IncreaseCcp(int dataLength) => this.m_docInfo.Fib.CcpHdd += dataLength;

  protected override void InitClass()
  {
    base.InitClass();
    this.m_docInfo.TablesData.HeaderPositions = new int[7 + this.m_docInfo.FkpData.SepxAddedCount * 6 + 1];
    this.WriteHeaderFooterHead();
    this.m_headerType = HeaderType.EvenHeader;
    this.m_curTxbxId = 4050;
    this.m_curPicId = 4500;
  }

  private void WriteHeaderFooterHead() => this.m_iItemIndex = 1;
}
