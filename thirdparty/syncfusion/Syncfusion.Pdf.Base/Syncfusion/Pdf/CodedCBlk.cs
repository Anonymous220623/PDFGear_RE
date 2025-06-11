// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.CodedCBlk
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class CodedCBlk
{
  public int n;
  public int m;
  public int skipMSBP;
  public byte[] data;

  public CodedCBlk()
  {
  }

  public CodedCBlk(int m, int n, int skipMSBP, byte[] data)
  {
    this.m = m;
    this.n = n;
    this.skipMSBP = skipMSBP;
    this.data = data;
  }

  public override string ToString()
  {
    return $"m={(object) this.m}, n={(object) this.n}, skipMSBP={(object) this.skipMSBP}, data.length={(this.data != null ? (object) string.Concat((object) this.data.Length) : (object) "(null)")}";
  }
}
