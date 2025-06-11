// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.WordFootnoteWriter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal class WordFootnoteWriter : WordSubdocumentWriter
{
  internal WordFootnoteWriter(WordWriter mainWriter)
    : base(mainWriter)
  {
    this.m_type = WordSubdocument.Footnote;
  }

  public override void WriteDocumentEnd()
  {
    this.m_docInfo.TablesData.Footnotes.AddTxtPosition(this.m_docInfo.Fib.CcpFtn);
    this.m_docInfo.TablesData.Footnotes.AddTxtPosition(this.m_docInfo.Fib.CcpFtn + 3);
    this.WriteChar('\r');
  }

  public override void WriteItemStart()
  {
    this.m_docInfo.TablesData.Footnotes.AddTxtPosition(this.m_docInfo.Fib.CcpFtn);
  }

  protected override void IncreaseCcp(int dataLength) => this.m_docInfo.Fib.CcpFtn += dataLength;
}
