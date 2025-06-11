// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.WordAnnotationWriter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal class WordAnnotationWriter : WordSubdocumentWriter
{
  internal WordAnnotationWriter(WordWriter mainWriter)
    : base(mainWriter)
  {
    this.m_type = WordSubdocument.Annotation;
  }

  public override void WriteDocumentEnd()
  {
    this.m_docInfo.TablesData.Annotations.AddTxtPosition(this.m_docInfo.Fib.CcpAtn);
    this.m_docInfo.TablesData.Annotations.AddTxtPosition(this.m_docInfo.Fib.CcpAtn + 3);
    this.WriteChar('\r');
  }

  public override void WriteItemStart()
  {
    this.m_docInfo.TablesData.Annotations.AddTxtPosition(this.m_docInfo.Fib.CcpAtn);
    this.WriteMarker(WordChunkType.Annotation);
  }

  protected override void IncreaseCcp(int dataLength) => this.m_docInfo.Fib.CcpAtn += dataLength;
}
