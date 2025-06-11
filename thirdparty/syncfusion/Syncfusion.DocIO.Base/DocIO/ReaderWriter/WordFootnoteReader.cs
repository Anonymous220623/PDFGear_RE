// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.WordFootnoteReader
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal class WordFootnoteReader : WordSubdocumentReader
{
  protected int m_prevStreamPos = -1;

  public WordFootnoteReader(WordReader mainReader)
    : base(mainReader)
  {
    this.Init();
  }

  public bool IsNextItem
  {
    get
    {
      bool isNextItem = false;
      bool flag = this.CheckPosition();
      if (flag)
      {
        isNextItem = this.m_streamsManager.MainStream.Position != (long) this.m_prevStreamPos && flag;
        this.m_prevStreamPos = (int) this.m_streamsManager.MainStream.Position;
      }
      return isNextItem;
    }
  }

  protected override void CreateStatePositions()
  {
    this.InitStatePositions();
    base.CreateStatePositions();
  }

  protected virtual bool CheckPosition()
  {
    int position = this.CalcCP(this.StatePositions.StartText, this.m_textChunk.Length);
    return this.m_docInfo.FkpData.Tables.Footnotes.HasPosition(position) && position != 0;
  }

  protected virtual void Init() => this.m_type = WordSubdocument.Footnote;

  protected virtual void InitStatePositions()
  {
    this.m_statePositions = (StatePositionsBase) new FootnoteStatePositions(this.m_docInfo.FkpData);
  }

  protected virtual bool IsEndOfItems()
  {
    return this.m_docInfo.FkpData.Tables.Footnotes.Count == this.m_itemIndex + 1;
  }

  public override WordChunkType ReadChunk()
  {
    WordChunkType wordChunkType = base.ReadChunk();
    if (this.IsEndOfItems())
      wordChunkType = WordChunkType.DocumentEnd;
    return wordChunkType;
  }
}
