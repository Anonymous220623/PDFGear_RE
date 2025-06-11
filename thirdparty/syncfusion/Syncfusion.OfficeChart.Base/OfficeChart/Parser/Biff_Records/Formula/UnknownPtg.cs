// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Formula.UnknownPtg
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Formula;

[Preserve(AllMembers = true)]
internal class UnknownPtg : Ptg
{
  [Preserve]
  public UnknownPtg()
  {
  }

  [Preserve]
  public UnknownPtg(DataProvider provider, int offset, OfficeVersion version)
    : base(provider, offset, version)
  {
    ++offset;
  }

  public override int GetSize(OfficeVersion version) => 1;

  public override string ToString() => $"( not implemented or UNKNOWN {this.TokenCode.ToString()})";
}
