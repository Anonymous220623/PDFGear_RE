// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.WordHeaderFooterReader
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;
using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal class WordHeaderFooterReader : WordSubdocumentReader
{
  public HFStatePositions StatePositions => (HFStatePositions) this.m_statePositions;

  public WordHeaderFooterReader(WordReader mainReader)
    : base(mainReader)
  {
    this.m_type = WordSubdocument.HeaderFooter;
  }

  public override void Reset()
  {
    base.Reset();
    this.MoveToSection(this.DEF_SECTION_NUMBER);
  }

  public void MoveToSection(int iSectionNumber)
  {
    this.StatePositions.SectionIndex = iSectionNumber - 1;
    this.MoveToHeader(HeaderType.EvenHeader);
    this.UpdateCharacterProperties();
    this.UpdateParagraphProperties();
  }

  public void MoveToHeader(HeaderType hType)
  {
    this.m_headerType = hType;
    this.UpdateStreamPosition();
  }

  public override void MoveToItem(int itemIndex) => this.MoveToHeader((HeaderType) itemIndex);

  public override FileShapeAddress GetFSPA()
  {
    int CP = this.CalcCP(this.StatePositions.StartItemPos, 1);
    return this.m_docInfo.TablesData.FileArtObjects == null ? (FileShapeAddress) null : this.m_docInfo.TablesData.FileArtObjects.FindFileShape(this.m_type, CP);
  }

  protected override void UpdateStreamPosition()
  {
    if (this.m_docInfo.TablesData.HeaderFooterCharPosTable == null)
    {
      this.m_chunkType = WordChunkType.DocumentEnd;
      this.m_headerType = HeaderType.InvalidValue;
    }
    else
    {
      this.UnfreezeStreamPos();
      this.m_chunkType = WordChunkType.Text;
      this.m_streamsManager.MainStream.Position = (long) this.StatePositions.MoveToItem((int) this.m_headerType);
    }
  }

  protected override void CreateStatePositions()
  {
    if (this.m_statePositions != null)
      return;
    this.m_statePositions = (StatePositionsBase) new HFStatePositions(this.m_docInfo.FkpData);
  }
}
