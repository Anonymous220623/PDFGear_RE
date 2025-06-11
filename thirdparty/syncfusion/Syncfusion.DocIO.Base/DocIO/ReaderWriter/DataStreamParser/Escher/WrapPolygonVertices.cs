// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.WrapPolygonVertices
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

internal class WrapPolygonVertices
{
  private int m_key;
  private msofbtRGFOPTE m_prop;
  private List<PointF> m_coords;
  private uint m_nelems;
  private uint m_nelemsalloc;
  private uint m_cbelem;

  internal WrapPolygonVertices(msofbtRGFOPTE prop, int key)
  {
    this.m_prop = prop;
    this.m_key = key;
    this.m_coords = new List<PointF>();
    this.readArrayData();
  }

  internal List<PointF> Coords
  {
    get => this.m_coords;
    set => this.m_coords = value;
  }

  internal uint nElems
  {
    get => this.m_nelems;
    set => this.m_nelems = value;
  }

  internal uint nElemsAlloc
  {
    get => this.m_nelemsalloc;
    set => this.m_nelemsalloc = value;
  }

  internal uint cbElem
  {
    get => this.m_cbelem;
    set => this.m_cbelem = value;
  }

  private void readArrayData()
  {
    MemoryStream memoryStream = new MemoryStream((this.m_prop[this.m_key] as FOPTEComplex).Value);
    StreamReader streamReader = new StreamReader((Stream) memoryStream);
    byte[] buffer1 = new byte[2];
    memoryStream.Read(buffer1, 0, 2);
    this.m_nelems = (uint) BitConverter.ToUInt16(buffer1, 0);
    memoryStream.Read(buffer1, 0, 2);
    this.m_nelemsalloc = (uint) BitConverter.ToUInt16(buffer1, 0);
    memoryStream.Read(buffer1, 0, 2);
    this.m_cbelem = (uint) BitConverter.ToUInt16(buffer1, 0);
    for (int index = 0; index < (int) this.m_nelems; ++index)
    {
      byte[] buffer2 = new byte[(int) this.m_cbelem];
      memoryStream.Read(buffer2, 0, (int) this.m_cbelem);
      this.m_coords.Add(new PointF((float) BitConverter.ToInt32(buffer2, 0), (float) BitConverter.ToInt32(buffer2, 4)));
    }
    memoryStream.Dispose();
  }
}
