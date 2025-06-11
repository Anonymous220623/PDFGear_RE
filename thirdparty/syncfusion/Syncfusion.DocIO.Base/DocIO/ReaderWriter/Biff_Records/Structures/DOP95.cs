// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures.DOP95
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;

[CLSCompliant(false)]
internal class DOP95
{
  private Copts80 m_copts80;
  private DOPDescriptor m_dopBase;

  internal Copts80 Copts80
  {
    get
    {
      if (this.m_copts80 == null)
        this.m_copts80 = new Copts80(this.m_dopBase);
      return this.m_copts80;
    }
  }

  internal DOP95(DOPDescriptor dopBase) => this.m_dopBase = dopBase;

  internal void Parse(Stream stream) => this.Copts80.Parse(stream);

  internal void Write(Stream stream) => this.Copts80.Write(stream);
}
