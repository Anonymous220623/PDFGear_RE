// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.SynWTFilterSpec
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class SynWTFilterSpec(int nt, int nc, byte type) : ModuleSpec(nt, nc, type)
{
  public virtual int getWTDataType(int t, int c)
  {
    return ((SynWTFilter[][]) this.getSpec(t, c))[0][0].DataType;
  }

  public virtual SynWTFilter[] getHFilters(int t, int c)
  {
    return ((SynWTFilter[][]) this.getSpec(t, c))[0];
  }

  public virtual SynWTFilter[] getVFilters(int t, int c)
  {
    return ((SynWTFilter[][]) this.getSpec(t, c))[1];
  }

  public override string ToString()
  {
    string str1 = $"nTiles={(object) this.nTiles}\nnComp={(object) this.nComp}\n\n";
    for (int t = 0; t < this.nTiles; ++t)
    {
      for (int c = 0; c < this.nComp; ++c)
      {
        SynWTFilter[][] spec = (SynWTFilter[][]) this.getSpec(t, c);
        string str2 = $"{str1}(t:{(object) t},c:{(object) c})\n" + "\tH:";
        for (int index = 0; index < spec[0].Length; ++index)
          str2 = $"{str2} {(object) spec[0][index]}";
        string str3 = str2 + "\n\tV:";
        for (int index = 0; index < spec[1].Length; ++index)
          str3 = $"{str3} {(object) spec[1][index]}";
        str1 = str3 + "\n";
      }
    }
    return str1;
  }

  public virtual bool isReversible(int t, int c)
  {
    SynWTFilter[] hfilters = this.getHFilters(t, c);
    SynWTFilter[] vfilters = this.getVFilters(t, c);
    for (int index = hfilters.Length - 1; index >= 0; --index)
    {
      if (!hfilters[index].Reversible || !vfilters[index].Reversible)
        return false;
    }
    return true;
  }
}
