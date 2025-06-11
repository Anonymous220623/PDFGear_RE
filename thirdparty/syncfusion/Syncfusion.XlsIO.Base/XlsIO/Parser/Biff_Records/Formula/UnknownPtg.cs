// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Formula.UnknownPtg
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Formula;

[Preserve(AllMembers = true)]
public class UnknownPtg : Ptg
{
  [Preserve]
  public UnknownPtg()
  {
  }

  [Preserve]
  public UnknownPtg(DataProvider provider, int offset, ExcelVersion version)
    : base(provider, offset, version)
  {
    ++offset;
  }

  public override int GetSize(ExcelVersion version) => 1;

  public override string ToString() => $"( not implemented or UNKNOWN {this.TokenCode.ToString()})";
}
