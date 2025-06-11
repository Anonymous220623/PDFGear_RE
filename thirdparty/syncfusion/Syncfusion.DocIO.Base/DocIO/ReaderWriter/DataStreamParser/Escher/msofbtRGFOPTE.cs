// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.msofbtRGFOPTE
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

internal class msofbtRGFOPTE : DocIOSortedList<int, FOPTEBase>
{
  public const int DEF_TXID = 128 /*0x80*/;
  public const uint DEF_LINE_WIDTH_PT = 12700;
  public const float DEF_LINE_WIDTH = 0.75f;
  public const uint DEF_COLOR_EMPTY = 4278190080 /*0xFF000000*/;
  public const uint DEF_NO_LINE = 524288 /*0x080000*/;
  public const uint DEF_COLOR_FILL = 1048592 /*0x100010*/;
  public const uint DEF_NO_COLOR_FILL = 1048576 /*0x100000*/;
  public const uint DEF_BEHIND_DOC = 2097184 /*0x200020*/;
  public const uint DEF_NOT_BEHIND_DOC = 2097152 /*0x200000*/;
  public const uint DEF_FIT_TEXT_TO_SHAPE = 131072 /*0x020000*/;
  public const uint DEF_BACKGROND_SHAPE = 65537 /*0x010001*/;
  private int m_prevPid;

  internal void Add(FOPTEBase fopteBase) => this.Add(fopteBase.Id, fopteBase);

  internal int Write(Stream stream)
  {
    int position = (int) stream.Position;
    foreach (FOPTEBase fopteBase1 in (IEnumerable<FOPTEBase>) this.Values)
    {
      if (fopteBase1.Id > 10000)
      {
        FOPTEBase fopteBase2 = fopteBase1.Clone();
        fopteBase2.Id -= 10000;
        fopteBase2.Write(stream);
      }
      else
        fopteBase1.Write(stream);
    }
    foreach (FOPTEBase fopteBase in (IEnumerable<FOPTEBase>) this.Values)
    {
      if (fopteBase is FOPTEComplex fopteComplex)
        fopteComplex.WriteData(stream);
    }
    return (int) (stream.Position - (long) position);
  }

  internal void Read(Stream stream, int length)
  {
    long num1 = stream.Position + (long) length;
    while (stream.Position < num1)
    {
      byte[] buffer = new byte[4];
      stream.Read(buffer, 0, 2);
      int int16 = (int) BitConverter.ToInt16(buffer, 0);
      int num2 = int16 & 16383 /*0x3FFF*/;
      bool isBid = (int16 & 16384 /*0x4000*/) != 0;
      bool flag = (int16 & 32768 /*0x8000*/) != 0;
      stream.Read(buffer, 0, 4);
      uint uint32 = BitConverter.ToUInt32(buffer, 0);
      if (flag)
      {
        this.Add(num2, (FOPTEBase) new FOPTEComplex(num2, isBid, (int) uint32));
        num1 -= (long) uint32;
        this.m_prevPid = num2;
      }
      else
      {
        if (num2 < this.m_prevPid)
        {
          this.m_prevPid = num2;
          num2 += 10000;
        }
        else
          this.m_prevPid = num2;
        this.Add(num2, (FOPTEBase) new FOPTEBid(num2, isBid, uint32));
      }
    }
    foreach (FOPTEBase fopteBase in (IEnumerable<FOPTEBase>) this.Values)
    {
      if (fopteBase is FOPTEComplex fopteComplex)
        fopteComplex.ReadData(stream);
    }
  }

  internal List<FOPTEBase> GetPostProps()
  {
    List<FOPTEBase> postProps = new List<FOPTEBase>();
    foreach (FOPTEBase fopteBase in (IEnumerable<FOPTEBase>) this.Values)
    {
      if (fopteBase.Id < 118 && fopteBase.Id != 4)
        postProps.Add(fopteBase);
    }
    return postProps;
  }
}
