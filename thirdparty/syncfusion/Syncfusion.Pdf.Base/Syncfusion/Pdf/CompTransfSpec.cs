// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.CompTransfSpec
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class CompTransfSpec(int nt, int nc, byte type) : ModuleSpec(nt, nc, type)
{
  public virtual bool CompTransfUsed
  {
    get
    {
      if ((int) this.def != 0)
        return true;
      if (this.tileDef != null)
      {
        for (int index = this.nTiles - 1; index >= 0; --index)
        {
          if (this.tileDef[index] != null && (int) this.tileDef[index] != 0)
            return true;
        }
      }
      return false;
    }
  }
}
