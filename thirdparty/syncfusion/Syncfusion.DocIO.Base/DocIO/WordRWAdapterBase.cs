// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.WordRWAdapterBase
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter;
using Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;
using System;

#nullable disable
namespace Syncfusion.DocIO;

[CLSCompliant(false)]
internal abstract class WordRWAdapterBase
{
  protected IWordReader m_mainReader;
  protected IWordWriter m_mainWriter;
  protected int m_textPos;

  internal WordRWAdapterBase()
  {
  }

  protected void Read(IWordReader reader)
  {
    this.m_mainReader = reader;
    this.m_mainReader.ReadDocumentHeader((WordDocument) null);
    this.ReadBody(this.m_mainReader);
    this.m_mainReader.ReadDocumentEnd();
  }

  protected virtual void ReadBody(IWordReader reader)
  {
    this.ReadStyleSheet(reader);
    this.ReadSubDocumentBody((IWordReaderBase) reader, WordSubdocument.Footnote);
    this.ReadSubDocumentBody((IWordReaderBase) reader, WordSubdocument.Annotation);
    this.ReadSubDocumentBody((IWordReaderBase) reader, WordSubdocument.Endnote);
    this.ReadShapeObjectsBody(reader);
    (reader as WordReader).UnfreezeStreamPos();
    this.m_textPos = reader.CurrentTextPosition;
    while (reader.ReadChunk() != WordChunkType.DocumentEnd)
    {
      this.ReadChunkBefore((IWordReaderBase) reader);
      this.ReadChunk((IWordReaderBase) reader);
    }
    this.ReadHFBody(reader);
  }

  protected abstract void ReadHFBody(IWordReader reader);

  protected abstract void ReadTextBoxBody(WordSubdocumentReader txbxReader, int txbxIndex);

  protected virtual void ReadChunk(IWordReaderBase baseReader)
  {
    IWordReader reader = baseReader as IWordReader;
    switch (reader.ChunkType)
    {
      case WordChunkType.SectionEnd:
        this.ReadSectionEnd(reader);
        break;
      case WordChunkType.PageBreak:
        this.ReadBreak(reader, BreakType.PageBreak);
        break;
      case WordChunkType.ColumnBreak:
        this.ReadBreak(reader, BreakType.ColumnBreak);
        break;
      case WordChunkType.Footnote:
        this.ReadFootnote(reader);
        break;
      case WordChunkType.Annotation:
        this.ReadAnnotation(reader);
        break;
      default:
        this.ReadChunkBase((IWordReaderBase) reader);
        return;
    }
    this.m_textPos = reader.CurrentTextPosition;
  }

  protected virtual void ReadChunkBase(IWordReaderBase reader)
  {
    switch (reader.ChunkType)
    {
      case WordChunkType.Text:
        if (reader is IWordReader)
        {
          IWordReader reader1 = reader as IWordReader;
          if (reader1.IsEndnote || reader1.IsFootnote)
          {
            this.ReadFootnote(reader1);
            break;
          }
        }
        this.ReadText(reader);
        break;
      case WordChunkType.ParagraphEnd:
        this.ReadParagraphEnd(reader);
        break;
      case WordChunkType.Image:
        this.ReadImage(reader);
        break;
      case WordChunkType.Shape:
        this.ReadShape(reader);
        break;
      case WordChunkType.Table:
        this.ReadTable(reader);
        break;
      case WordChunkType.TableRow:
        this.ReadTableRow(reader);
        break;
      case WordChunkType.TableCell:
        this.ReadTableCell(reader);
        break;
      case WordChunkType.Footnote:
        switch (reader)
        {
          case WordFootnoteReader _:
          case WordEndnoteReader _:
            this.ReadFootnoteMarker(reader);
            break;
        }
        break;
      case WordChunkType.FieldBeginMark:
        this.ReadField(reader);
        break;
      case WordChunkType.FieldEndMark:
        this.ReadFieldEnd(reader);
        break;
      case WordChunkType.LineBreak:
        this.ReadLineBreak(reader);
        break;
      case WordChunkType.Symbol:
        if (reader is IWordReader)
        {
          IWordReader reader2 = reader as IWordReader;
          if (reader2.IsEndnote || reader2.IsFootnote)
          {
            this.ReadFootnote(reader2);
            break;
          }
        }
        this.ReadSymbol(reader);
        break;
      case WordChunkType.CurrentPageNumber:
        this.ReadCurrentPageNumber(reader);
        break;
    }
    this.m_textPos = reader.CurrentTextPosition;
  }

  protected abstract void ReadSubDocumentBody(IWordReaderBase reader, WordSubdocument subDocument);

  protected abstract void ReadChunkBefore(IWordReaderBase reader);

  protected abstract void ReadPageBreak(IWordReader reader);

  protected abstract void ReadBreak(IWordReader reader, BreakType breakType);

  protected abstract void ReadSectionEnd(IWordReader reader);

  protected abstract void ReadField(IWordReaderBase reader);

  protected abstract void ReadTable(IWordReaderBase reader);

  protected abstract void ReadTableRow(IWordReaderBase reader);

  protected abstract void ReadTableCell(IWordReaderBase reader);

  protected abstract void ReadImage(IWordReaderBase reader);

  protected abstract void ReadParagraphEnd(IWordReaderBase reader);

  protected abstract void ReadText(IWordReaderBase reader);

  protected abstract void ReadStyleSheet(IWordReader reader);

  protected abstract void ReadLineBreak(IWordReaderBase reader);

  protected abstract void ReadShape(IWordReaderBase reader);

  protected abstract void ReadTextBoxShape(IWordReaderBase reader, TextBoxShape txtShape);

  protected abstract void ReadImageShape(IWordReaderBase reader, PictureShape imageShape);

  protected abstract void ReadShapeObjectsBody(IWordReader reader);

  protected abstract void ReadSymbol(IWordReaderBase reader);

  protected abstract void ReadFieldEnd(IWordReaderBase reader);

  protected abstract void ReadCurrentPageNumber(IWordReaderBase reader);

  protected abstract void ReadAnnotationBody(IWordReaderBase reader);

  protected abstract void ReadAnnotation(IWordReader reader);

  protected abstract void ReadFootnoteBody(IWordReaderBase reader);

  protected abstract void ReadFootnote(IWordReader reader);

  protected abstract void ReadFootnoteMarker(IWordReaderBase reader);
}
