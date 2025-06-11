// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.MsofbtSecondaryFOPT
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Escher;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

internal class MsofbtSecondaryFOPT : BaseEscherRecord
{
  private msofbtRGFOPTE m_prop;

  internal msofbtRGFOPTE Properties
  {
    get => this.m_prop;
    set => this.m_prop = value;
  }

  internal MsofbtSecondaryFOPT(WordDocument doc)
    : base(MSOFBT.msofbtSecondaryFOPT, 3, doc)
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
    MsofbtSecondaryFOPT msofbtSecondaryFopt = new MsofbtSecondaryFOPT(this.m_doc);
    foreach (FOPTEBase fopteBase in (IEnumerable<FOPTEBase>) this.m_prop.Values)
      msofbtSecondaryFopt.m_prop.Add(fopteBase);
    msofbtSecondaryFopt.m_doc = this.m_doc;
    return (BaseEscherRecord) msofbtSecondaryFopt;
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
