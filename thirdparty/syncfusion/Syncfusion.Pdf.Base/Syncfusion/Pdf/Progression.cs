// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Progression
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class Progression
{
  public int type;
  public int cs;
  public int ce;
  public int rs;
  public int re;
  public int lye;

  public Progression(int type, int cs, int ce, int rs, int re, int lye)
  {
    this.type = type;
    this.cs = cs;
    this.ce = ce;
    this.rs = rs;
    this.re = re;
    this.lye = lye;
  }

  public override string ToString()
  {
    string str = "type= ";
    switch (this.type)
    {
      case 0:
        str += "layer, ";
        break;
      case 1:
        str += "res, ";
        break;
      case 2:
        str += "res-pos, ";
        break;
      case 3:
        str += "pos-comp, ";
        break;
      case 4:
        str += "pos-comp, ";
        break;
    }
    return $"{$"{$"{str}comp.: {(object) this.cs}-{(object) this.ce}, "}res.: {(object) this.rs}-{(object) this.re}, "}layer: up to {(object) this.lye}";
  }
}
