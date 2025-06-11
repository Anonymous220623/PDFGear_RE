// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.ContainerCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

internal class ContainerCollection : List<object>
{
  private WordDocument m_doc;

  internal ContainerCollection(WordDocument doc) => this.m_doc = doc;

  internal int Write(Stream stream)
  {
    long position = stream.Position;
    foreach (BaseEscherRecord baseEscherRecord in (List<object>) this)
    {
      if (!(baseEscherRecord is MsofbtClientData) || baseEscherRecord.Header.Version == 15)
        baseEscherRecord.WriteMsofbhWithRecord(stream);
    }
    return (int) (stream.Position - position);
  }

  internal void Read(Stream stream, int length)
  {
    long num = stream.Position + (long) length;
    while (stream.Position < num && stream.Position < stream.Length)
    {
      BaseEscherRecord baseEscherRecord = _MSOFBH.ReadHeaderWithRecord(stream, this.m_doc);
      if (baseEscherRecord != null)
        this.Add((object) baseEscherRecord);
    }
  }
}
