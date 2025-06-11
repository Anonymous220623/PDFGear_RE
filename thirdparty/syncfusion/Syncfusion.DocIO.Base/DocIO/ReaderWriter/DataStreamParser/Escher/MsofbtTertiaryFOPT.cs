// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.MsofbtTertiaryFOPT
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Escher;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

internal class MsofbtTertiaryFOPT : BaseEscherRecord
{
  private const int DEF_UNKNOWN2_PID = 1343;
  private const uint DEF_NOTALLOWINCELL = 2147483648 /*0x80000000*/;
  private msofbtRGFOPTE m_prop;
  private LineStyleBooleanProperties m_lineProps;
  private WrapPolygonVertices m_wrapPolygonVetrices;

  internal LineStyleBooleanProperties LineProperties
  {
    get
    {
      if (this.m_lineProps == null)
        this.m_lineProps = new LineStyleBooleanProperties(this.m_prop, 511 /*0x01FF*/);
      return this.m_lineProps;
    }
  }

  internal WrapPolygonVertices WrapPolygonVertices
  {
    get
    {
      if (this.m_wrapPolygonVetrices == null)
        this.m_wrapPolygonVetrices = new WrapPolygonVertices(this.m_prop, 899);
      return this.m_wrapPolygonVetrices;
    }
  }

  internal msofbtRGFOPTE Properties
  {
    get => this.m_prop;
    set => this.m_prop = value;
  }

  public uint XAlign
  {
    get => this.GetPropertyValue(911);
    set => this.SetPropertyValue(911, value);
  }

  public uint XRelTo
  {
    get => this.GetPropertyValue(912);
    set => this.SetPropertyValue(912, value);
  }

  public uint YAlign
  {
    get => this.GetPropertyValue(913);
    set => this.SetPropertyValue(913, value);
  }

  public uint YRelTo
  {
    get => this.GetPropertyValue(914);
    set => this.SetPropertyValue(914, value);
  }

  public uint LayoutInTableCell
  {
    get => this.GetPropertyValue(959);
    set => this.SetPropertyValue(959, value);
  }

  public uint Unknown1
  {
    get => this.GetPropertyValue(447);
    set => this.SetPropertyValue(447, value);
  }

  public uint Unknown2
  {
    get => this.GetPropertyValue(1343);
    set => this.SetPropertyValue(1343, value);
  }

  internal bool AllowInTableCell
  {
    get
    {
      return this.LayoutInTableCell == uint.MaxValue || (this.LayoutInTableCell & 2147483648U /*0x80000000*/) >> 31 /*0x1F*/ == 0U || (this.LayoutInTableCell & 32768U /*0x8000*/) >> 15 != 0U;
    }
    set
    {
      if (this.LayoutInTableCell == uint.MaxValue)
        this.LayoutInTableCell = 2147483648U /*0x80000000*/;
      this.LayoutInTableCell = (uint) ((ulong) (this.LayoutInTableCell & (uint) int.MaxValue) | 18446744071562067968UL);
      this.LayoutInTableCell = (uint) ((long) (this.LayoutInTableCell & 4294934527U) | (long) ((value ? 1 : 0) << 15));
    }
  }

  internal bool AllowOverlap
  {
    get
    {
      return this.LayoutInTableCell == uint.MaxValue || (this.LayoutInTableCell & 33554432U /*0x02000000*/) >> 25 == 0U || (this.LayoutInTableCell & 512U /*0x0200*/) >> 9 != 0U;
    }
    set
    {
      if (this.LayoutInTableCell == uint.MaxValue)
        this.LayoutInTableCell = 33554432U /*0x02000000*/;
      this.LayoutInTableCell = (uint) ((int) this.LayoutInTableCell & -33554433 | 33554432 /*0x02000000*/);
      this.LayoutInTableCell = (uint) ((long) (this.LayoutInTableCell & 4294966783U) | (long) ((value ? 1 : 0) << 9));
    }
  }

  internal bool Visible
  {
    get
    {
      return this.LayoutInTableCell == uint.MaxValue || (this.LayoutInTableCell & 131072U /*0x020000*/) >> 17 == 0U || (this.LayoutInTableCell & 2U) >> 1 == 0U;
    }
    set
    {
      if (this.LayoutInTableCell == uint.MaxValue)
        this.LayoutInTableCell = 131072U /*0x020000*/;
      this.LayoutInTableCell = (uint) ((long) (this.LayoutInTableCell & 4294836223U) | (value ? 0L : 131072L /*0x020000*/));
      this.LayoutInTableCell = (uint) ((long) (this.LayoutInTableCell & 4294967293U) | (value ? 0L : 2L));
    }
  }

  internal MsofbtTertiaryFOPT(WordDocument doc)
    : base(MSOFBT.msofbtTertiaryFOPT, 3, doc)
  {
    this.m_prop = new msofbtRGFOPTE();
  }

  protected override void ReadRecordData(Stream stream)
  {
    this.m_prop.Clear();
    this.m_prop.Read(stream, this.Header.Length);
  }

  protected override void WriteRecordData(Stream stream)
  {
    this.Header.Instance = this.CountInstanceValue();
    this.m_prop.Write(stream);
  }

  internal override BaseEscherRecord Clone()
  {
    MsofbtTertiaryFOPT msofbtTertiaryFopt = new MsofbtTertiaryFOPT(this.m_doc);
    foreach (FOPTEBase fopteBase in (IEnumerable<FOPTEBase>) this.m_prop.Values)
      msofbtTertiaryFopt.m_prop.Add(fopteBase);
    msofbtTertiaryFopt.m_doc = this.m_doc;
    return (BaseEscherRecord) msofbtTertiaryFopt;
  }

  internal override void Close()
  {
    base.Close();
    if (this.m_prop == null)
      return;
    this.m_prop.Clear();
    this.m_prop = (msofbtRGFOPTE) null;
  }

  internal uint GetPropertyValue(int key)
  {
    return this.m_prop.ContainsKey(key) && this.m_prop[key] is FOPTEBid fopteBid ? fopteBid.Value : uint.MaxValue;
  }

  internal void SetPropertyValue(int key, uint value)
  {
    if (this.m_prop.ContainsKey(key))
      (this.m_prop[key] as FOPTEBid).Value = value;
    else
      this.m_prop.Add(key, (FOPTEBase) new FOPTEBid(key, false, value));
  }

  internal byte[] GetComplexPropValue(int key)
  {
    return this.Properties.ContainsKey(key) ? ((FOPTEComplex) this.Properties[key]).Value : (byte[]) null;
  }

  private int CountInstanceValue()
  {
    int num = 0;
    foreach (object obj in (IEnumerable<FOPTEBase>) this.m_prop.Values)
    {
      if ((obj as FOPTEBase).Id < 10000)
        ++num;
    }
    return num;
  }
}
