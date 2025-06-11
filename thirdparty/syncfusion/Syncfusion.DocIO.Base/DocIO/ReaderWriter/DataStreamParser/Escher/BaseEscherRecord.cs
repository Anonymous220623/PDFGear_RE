// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.BaseEscherRecord
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.ReaderWriter.Escher;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

internal abstract class BaseEscherRecord : BaseWordRecord
{
  private _MSOFBH m_msofbh;
  internal WordDocument m_doc;

  internal _MSOFBH Header
  {
    get => this.m_msofbh;
    set => this.m_msofbh = value;
  }

  internal BaseEscherRecord(WordDocument doc)
  {
    this.m_doc = doc;
    this.m_msofbh = new _MSOFBH(doc);
  }

  internal BaseEscherRecord(MSOFBT type, int version, WordDocument doc)
    : this(doc)
  {
    this.m_msofbh.Type = type;
    this.m_msofbh.Version = version;
  }

  protected abstract void ReadRecordData(Stream stream);

  protected abstract void WriteRecordData(Stream stream);

  internal abstract BaseEscherRecord Clone();

  internal bool ReadRecord(_MSOFBH msofbh, Stream stream)
  {
    this.m_msofbh = msofbh;
    int position = (int) stream.Position;
    this.ReadRecordData(stream);
    return (int) stream.Position - position == this.m_msofbh.Length;
  }

  internal void ReadMsofbhWithRecord(Stream stream)
  {
    this.ReadRecord(new _MSOFBH(stream, this.m_doc), stream);
  }

  internal int WriteMsofbhWithRecord(Stream stream)
  {
    int int32_1 = Convert.ToInt32(stream.Position);
    this.Header.Write(stream);
    int int32_2 = Convert.ToInt32(stream.Position);
    this.WriteRecordData(stream);
    int int32_3 = Convert.ToInt32(stream.Position);
    this.Header.Length = int32_3 - int32_2;
    stream.Position = (long) int32_1;
    this.Header.Write(stream);
    stream.Position = (long) int32_3;
    return int32_3 - int32_1;
  }

  internal new virtual void Close() => this.m_doc = (WordDocument) null;
}
