// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.WordSubdocumentReader
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal abstract class WordSubdocumentReader : 
  WordReaderBase,
  IWordSubdocumentReader,
  IWordReaderBase
{
  protected int DEF_SECTION_NUMBER = 1;
  protected HeaderType m_headerType = HeaderType.InvalidValue;
  protected int m_itemIndex = -1;
  private bool m_bIsNextItemPos;
  private WordReader m_mainReader;

  public WordSubdocumentReader(WordReader mainReader)
    : base(mainReader.m_streamsManager)
  {
    this.m_mainReader = mainReader;
    this.m_styleSheet = mainReader.StyleSheet;
    this.m_currStyleIndex = mainReader.CurrentStyleIndex;
    this.m_mainReader.FreezeStreamPos();
    this.InitClass();
  }

  public WordSubdocument Type => this.m_type;

  public HeaderType HeaderType
  {
    get => this.m_headerType;
    set => this.m_headerType = value;
  }

  public int ItemNumber => this.m_itemIndex;

  public StatePositionsBase StatePositions => this.m_statePositions;

  internal bool IsNextItemPos => this.m_bIsNextItemPos;

  public override FieldDescriptor GetFld()
  {
    return this.m_docInfo.TablesData.Fields.FindFld(this.m_type, this.CalcCP(this.StatePositions.StartItemPos, 1));
  }

  public virtual void Reset()
  {
    this.m_docInfo = this.m_mainReader.m_docInfo;
    this.CreateStatePositions();
    this.UpdateCharacterProperties();
    this.UpdateParagraphProperties();
  }

  public virtual void MoveToItem(int itemIndex)
  {
    this.m_itemIndex = itemIndex;
    this.UpdateStreamPosition();
  }

  protected virtual void CreateStatePositions() => this.MoveToItem(0);

  protected override long GetChunkEndPosition(long iCurrentPos)
  {
    return Math.Min(base.GetChunkEndPosition(iCurrentPos), (long) this.StatePositions.EndItemPos);
  }

  protected override void InitClass() => this.Reset();

  protected virtual void UpdateStreamPosition()
  {
    this.m_streamsManager.MainStream.Position = (long) this.StatePositions.MoveToItem(this.m_itemIndex);
  }

  protected override void UpdateEndPositions(long iEndPos)
  {
    this.m_bIsNextItemPos = false;
    base.UpdateEndPositions(iEndPos);
    if (this.m_type == WordSubdocument.HeaderFooter)
    {
      if ((this.StatePositions as HFStatePositions).UpdateHeaderEndPos(iEndPos, this.m_headerType))
        ++this.m_headerType;
    }
    else if (this.StatePositions.UpdateItemEndPos(iEndPos))
    {
      ++this.m_itemIndex;
      this.m_bIsNextItemPos = true;
    }
    this.UpdateCharacterProperties();
    this.UpdateParagraphProperties();
  }

  public override void UnfreezeStreamPos()
  {
    this.m_mainReader.FreezeStreamPos();
    base.UnfreezeStreamPos();
  }
}
