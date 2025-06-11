// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.WordSubdocumentWriter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal abstract class WordSubdocumentWriter : 
  WordWriterBase,
  IWordSubdocumentWriter,
  IWordWriterBase
{
  internal WordSubdocumentWriter(WordWriter mainWriter)
    : base(mainWriter.m_streamsManager)
  {
    this.m_docInfo = mainWriter.m_docInfo;
    this.m_styleSheet = mainWriter.StyleSheet;
    this.InitClass();
    this.m_listProperties = mainWriter.ListProperties;
    this.m_listProperties.UpdatePAPX(this.m_papx);
  }

  public WordSubdocument Type => this.m_type;

  public abstract void WriteDocumentEnd();

  public virtual void WriteItemStart()
  {
  }

  public virtual void WriteItemEnd() => this.WriteMarker(WordChunkType.ParagraphEnd);

  protected override void InitClass()
  {
    base.InitClass();
    this.m_iStartText = (int) this.m_streamsManager.MainStream.Position;
  }
}
