// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.QuantTypeSpec
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class QuantTypeSpec(int nt, int nc, byte type) : ModuleSpec(nt, nc, type)
{
  public virtual bool isDerived(int t, int c)
  {
    return ((string) this.getTileCompVal(t, c)).Equals("derived");
  }

  public virtual bool isReversible(int t, int c)
  {
    return ((string) this.getTileCompVal(t, c)).Equals("reversible");
  }
}
