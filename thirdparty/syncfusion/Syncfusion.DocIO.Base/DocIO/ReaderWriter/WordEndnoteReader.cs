// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.WordEndnoteReader
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal class WordEndnoteReader(WordReader mainReader) : WordFootnoteReader(mainReader)
{
  protected override bool CheckPosition()
  {
    int position = this.CalcCP(this.StatePositions.StartText, this.m_textChunk.Length);
    return this.m_docInfo.FkpData.Tables.Endnotes.HasPosition(position) && position != 0;
  }

  protected override void Init() => this.m_type = WordSubdocument.Endnote;

  protected override void InitStatePositions()
  {
    this.m_statePositions = (StatePositionsBase) new EndnoteStatePositions(this.m_docInfo.FkpData);
  }

  protected override bool IsEndOfItems()
  {
    return this.m_docInfo.FkpData.Tables.Endnotes.Count == this.m_itemIndex + 1;
  }
}
