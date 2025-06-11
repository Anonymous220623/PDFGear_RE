// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.FOPTEBase
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

internal abstract class FOPTEBase : BaseWordRecord
{
  private int m_id;
  private bool m_isBid;

  internal int Id
  {
    get => this.m_id;
    set => this.m_id = value;
  }

  internal bool IsBid => this.m_isBid;

  internal FOPTEBase(int id, bool isBid)
  {
    this.m_id = id;
    this.m_isBid = isBid;
  }

  internal abstract void Write(Stream stream);

  internal abstract FOPTEBase Clone();
}
