// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.Tuple`3
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class Tuple<T1, T2, T3>
{
  private readonly T1 item1;
  private readonly T2 item2;
  private readonly T3 item3;

  public T1 Item1 => this.item1;

  public T2 Item2 => this.item2;

  public T3 Item3 => this.item3;

  public Tuple()
  {
  }

  public Tuple(T1 item1, T2 item2, T3 item3)
  {
    this.item1 = item1;
    this.item2 = item2;
    this.item3 = item3;
  }
}
