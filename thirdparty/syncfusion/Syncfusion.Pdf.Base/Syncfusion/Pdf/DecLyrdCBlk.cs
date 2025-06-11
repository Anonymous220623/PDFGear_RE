// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.DecLyrdCBlk
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class DecLyrdCBlk : CodedCBlk
{
  public int ulx;
  public int uly;
  public int w;
  public int h;
  public int dl;
  public bool prog;
  public int nl;
  public int ftpIdx;
  public int nTrunc;
  public int[] tsLengths;

  public override string ToString()
  {
    string str1 = $"Coded code-block ({(object) this.m},{(object) this.n}): {(object) this.skipMSBP} MSB skipped, {(object) this.dl} bytes, {(object) this.nTrunc} truncation points, {(object) this.nl} layers, progressive={(object) this.prog}, ulx={(object) this.ulx}, uly={(object) this.uly}, w={(object) this.w}, h={(object) this.h}, ftpIdx={(object) this.ftpIdx}";
    if (this.tsLengths != null)
    {
      string str2 = str1 + " {";
      for (int index = 0; index < this.tsLengths.Length; ++index)
        str2 = $"{str2} {(object) this.tsLengths[index]}";
      str1 = str2 + " }";
    }
    return str1;
  }
}
