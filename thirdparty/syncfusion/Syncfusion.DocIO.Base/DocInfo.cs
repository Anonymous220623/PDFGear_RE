// Decompiled with JetBrains decompiler
// Type: DocInfo
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter;
using Syncfusion.DocIO.ReaderWriter.Escher;
using System;
using System.Text;

#nullable disable
[CLSCompliant(false)]
internal class DocInfo
{
  private Fib m_fib;
  private WPTablesData m_tablesData;
  private WordFKPData m_fkpData;
  private WordImageWriter m_imageWriter;

  internal DocInfo(StreamsManager streamsManager)
  {
    this.m_fib = new Fib();
    this.m_tablesData = new WPTablesData(this.m_fib);
    this.m_fkpData = new WordFKPData(this.m_fib, this.m_tablesData);
    this.m_imageWriter = new WordImageWriter(streamsManager.DataStream);
  }

  internal Fib Fib => this.m_fib;

  internal WPTablesData TablesData => this.m_tablesData;

  internal WordFKPData FkpData => this.m_fkpData;

  internal WordImageWriter ImageWriter => this.m_imageWriter;

  internal WordImageReader GetImageReader(
    StreamsManager streamsManager,
    int offset,
    WordDocument doc)
  {
    return new WordImageReader(streamsManager.DataStream, offset, doc);
  }

  internal void Close()
  {
    if (this.m_fib != null)
    {
      this.m_fib.Encoding = (Encoding) null;
      this.m_fib = (Fib) null;
    }
    if (this.m_tablesData != null)
    {
      this.m_tablesData.Close();
      this.m_tablesData = (WPTablesData) null;
    }
    if (this.m_fkpData != null)
    {
      this.m_fkpData.Close();
      this.m_fkpData = (WordFKPData) null;
    }
    if (this.m_imageWriter == null)
      return;
    this.m_imageWriter.Close();
    this.m_imageWriter = (WordImageWriter) null;
  }
}
