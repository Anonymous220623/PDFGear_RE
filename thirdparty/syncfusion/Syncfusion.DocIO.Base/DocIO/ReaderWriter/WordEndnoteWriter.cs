// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.WordEndnoteWriter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal class WordEndnoteWriter : WordSubdocumentWriter
{
  internal WordEndnoteWriter(WordWriter mainWriter)
    : base(mainWriter)
  {
    this.m_type = WordSubdocument.Endnote;
  }

  public override void WriteItemStart()
  {
    this.m_docInfo.TablesData.Endnotes.AddTxtPosition(this.m_docInfo.Fib.CcpEdn);
  }

  public override void WriteDocumentEnd()
  {
    this.m_docInfo.TablesData.Endnotes.AddTxtPosition(this.m_docInfo.Fib.CcpEdn);
    this.m_docInfo.TablesData.Endnotes.AddTxtPosition(this.m_docInfo.Fib.CcpEdn + 3);
    this.WriteChar('\r');
  }

  protected override void IncreaseCcp(int dataLength) => this.m_docInfo.Fib.CcpEdn += dataLength;
}
