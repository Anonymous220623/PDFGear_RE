// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.FIDCL
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

internal class FIDCL : BaseWordRecord
{
  internal int m_dgid;
  internal int m_cspidCur;

  internal FIDCL(Stream stream) => this.Read(stream);

  internal FIDCL(int dgid, int cspidCur)
  {
    this.m_dgid = dgid;
    this.m_cspidCur = cspidCur;
  }

  internal void Write(Stream stream)
  {
    BaseWordRecord.WriteInt32(stream, this.m_dgid);
    BaseWordRecord.WriteInt32(stream, this.m_cspidCur);
  }

  internal void Read(Stream stream)
  {
    this.m_dgid = BaseWordRecord.ReadInt32(stream);
    this.m_cspidCur = BaseWordRecord.ReadInt32(stream);
  }
}
