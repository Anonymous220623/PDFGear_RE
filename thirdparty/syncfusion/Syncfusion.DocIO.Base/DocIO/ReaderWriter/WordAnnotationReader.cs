// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.WordAnnotationReader
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal class WordAnnotationReader : WordSubdocumentReader
{
  public bool IsStartAnnotation
  {
    get
    {
      return this.m_docInfo.FkpData.Tables.Annotations.HasPosition(this.CalcCP(this.StatePositions.StartText, this.m_textChunk.Length));
    }
  }

  public WordAnnotationReader(WordReader mainReader)
    : base(mainReader)
  {
    this.m_type = WordSubdocument.Annotation;
  }

  public AnnotationDescriptor Descriptor
  {
    get => this.m_docInfo.TablesData.Annotations.GetDescriptor(this.m_itemIndex);
  }

  public string User => this.m_docInfo.TablesData.Annotations.GetUser(this.m_itemIndex);

  public int BookmarkStartOffset
  {
    get => this.m_docInfo.TablesData.Annotations.GetBookmarkStartOffset(this.m_itemIndex);
  }

  public int BookmarkEndOffset
  {
    get => this.m_docInfo.TablesData.Annotations.GetBookmarkEndOffset(this.m_itemIndex);
  }

  public override WordChunkType ReadChunk()
  {
    int num = (int) base.ReadChunk();
    if (this.m_docInfo.FkpData.Tables.Annotations.Count == this.m_itemIndex + 1)
      this.m_chunkType = WordChunkType.DocumentEnd;
    return this.m_chunkType;
  }

  public int Position => this.m_docInfo.TablesData.Annotations.GetPosition(this.m_itemIndex);

  protected override void CreateStatePositions()
  {
    this.m_statePositions = (StatePositionsBase) new AtnStatePositions(this.m_docInfo.FkpData);
    base.CreateStatePositions();
  }
}
