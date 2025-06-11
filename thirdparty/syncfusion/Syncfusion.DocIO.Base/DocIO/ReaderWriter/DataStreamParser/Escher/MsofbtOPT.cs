// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.MsofbtOPT
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Escher;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

internal class MsofbtOPT : BaseEscherRecord
{
  public const int DEF_PIB_ID = 260;
  public const int DEF_PIBFLAGS_ID = 262;
  public const int DEF_TXID = 128 /*0x80*/;
  public const int DEF_WRAP_REXT = 133;
  internal const uint DEF_WRAP_DIST = 114300;
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

  internal FOPTEBid Pib
  {
    get => this.m_prop.ContainsKey(260) ? this.m_prop[260] as FOPTEBid : (FOPTEBid) null;
  }

  internal FOPTEBid Txid
  {
    get
    {
      FOPTEBid txid = (FOPTEBid) null;
      if (this.m_prop.ContainsKey(128 /*0x80*/))
        txid = this.m_prop[128 /*0x80*/] as FOPTEBid;
      return txid;
    }
  }

  internal msofbtRGFOPTE Properties
  {
    get => this.m_prop;
    set => this.m_prop = value;
  }

  internal uint LayoutInTableCell
  {
    get => this.GetPropertyValue(959);
    set => this.SetPropertyValue(959, value);
  }

  internal bool AllowInTableCell
  {
    get
    {
      return this.LayoutInTableCell == uint.MaxValue || (this.LayoutInTableCell & 2147483648U /*0x80000000*/) >> 31 /*0x1F*/ == 0U || (this.LayoutInTableCell & 32768U /*0x8000*/) >> 15 != 0U;
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

  internal uint DistanceFromBottom
  {
    get
    {
      uint propertyValue = this.GetPropertyValue(903);
      return propertyValue == uint.MaxValue ? 0U : propertyValue;
    }
    set => this.SetPropertyValue(903, value);
  }

  internal uint DistanceFromLeft
  {
    get
    {
      uint propertyValue = this.GetPropertyValue(900);
      return propertyValue == uint.MaxValue ? 114300U : propertyValue;
    }
    set => this.SetPropertyValue(900, value);
  }

  internal uint DistanceFromRight
  {
    get
    {
      uint propertyValue = this.GetPropertyValue(902);
      return propertyValue == uint.MaxValue ? 114300U : propertyValue;
    }
    set => this.SetPropertyValue(902, value);
  }

  internal uint DistanceFromTop
  {
    get
    {
      uint propertyValue = this.GetPropertyValue(901);
      return propertyValue == uint.MaxValue ? 0U : propertyValue;
    }
    set => this.SetPropertyValue(901, value);
  }

  internal uint Roation
  {
    get
    {
      uint propertyValue = this.GetPropertyValue(4);
      return propertyValue == uint.MaxValue ? 0U : propertyValue;
    }
    set => this.SetPropertyValue(4, value);
  }

  internal MsofbtOPT(WordDocument doc)
    : base(MSOFBT.msofbtOPT, 3, doc)
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
    MsofbtOPT msofbtOpt = new MsofbtOPT(this.m_doc);
    foreach (FOPTEBase fopteBase in (IEnumerable<FOPTEBase>) this.m_prop.Values)
      msofbtOpt.m_prop.Add(fopteBase);
    msofbtOpt.m_doc = this.m_doc;
    return (BaseEscherRecord) msofbtOpt;
  }

  internal override void Close()
  {
    base.Close();
    if (this.m_prop == null)
      return;
    this.m_prop.Clear();
    this.m_prop = (msofbtRGFOPTE) null;
  }

  public uint GetPropertyValue(int key)
  {
    return this.Properties.ContainsKey(key) && this.Properties[key] is FOPTEBid property ? property.Value : uint.MaxValue;
  }

  internal void SetPropertyValue(int key, uint value)
  {
    if (this.m_prop.ContainsKey(key))
      (this.m_prop[key] as FOPTEBid).Value = value;
    else
      this.m_prop.Add(key, (FOPTEBase) new FOPTEBid(key, false, value));
  }

  public byte[] GetComplexPropValue(int key)
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
