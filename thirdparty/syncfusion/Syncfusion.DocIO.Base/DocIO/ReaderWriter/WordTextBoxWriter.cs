// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.WordTextBoxWriter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal class WordTextBoxWriter : WordSubdocumentWriter
{
  protected const uint DEF_TEXTBOX_RESERVED_DATA = 4294967295 /*0xFFFFFFFF*/;
  protected int m_lastTxbxPosition;
  protected int m_txbxBkDCnt;
  protected long m_dataPosition;

  internal WordTextBoxWriter(WordWriter mainWriter)
    : base(mainWriter)
  {
    this.m_type = WordSubdocument.TextBox;
  }

  public override void WriteDocumentEnd()
  {
    for (int index = 0; index < 2; ++index)
      this.WriteChar('\r');
    this.AddNewTxbx(true, 0);
  }

  public override void WriteItemEnd()
  {
    base.WriteItemEnd();
    this.AddNewTxbx(false, 0);
  }

  internal void WriteTextBoxEnd(int spid)
  {
    this.WriteMarker(WordChunkType.ParagraphEnd);
    this.AddNewTxbx(false, spid);
  }

  protected override void IncreaseCcp(int dataLength) => this.m_docInfo.Fib.CcpTxbx += dataLength;

  protected virtual void AddNewTxbx(bool isLast, int spid)
  {
    TextBoxStoryDescriptor txbxStoryDesc = new TextBoxStoryDescriptor();
    BreakDescriptor txbxBKDesc = new BreakDescriptor();
    txbxStoryDesc.TextBoxCnt = 1;
    if (!isLast)
    {
      txbxBKDesc.Ipgd = (short) this.m_txbxBkDCnt;
      txbxBKDesc.Options = (byte) 16 /*0x10*/;
      txbxStoryDesc.ShapeIdent = spid;
      txbxStoryDesc.Reserved = uint.MaxValue;
    }
    else
    {
      txbxBKDesc.Ipgd = (short) -1;
      txbxBKDesc.Options = (byte) 0;
      txbxStoryDesc.ShapeIdent = 0;
    }
    this.m_docInfo.TablesData.ArtObj.AddTxbx(WordSubdocument.Main, txbxStoryDesc, txbxBKDesc, this.m_lastTxbxPosition);
    this.m_lastTxbxPosition = this.m_docInfo.Fib.CcpTxbx;
    ++this.m_txbxBkDCnt;
  }
}
