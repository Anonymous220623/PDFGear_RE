// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.Tuple`2
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class Tuple<T1, T2>
{
  private bool isEmpty;
  private T1 item1;
  private T2 item2;

  public T1 Item1
  {
    get => this.item1;
    set
    {
      this.item1 = value;
      this.isEmpty = false;
    }
  }

  public T2 Item2
  {
    get => this.item2;
    set
    {
      this.item2 = value;
      this.isEmpty = false;
    }
  }

  public Tuple()
  {
    this.item1 = default (T1);
    this.item2 = default (T2);
    this.isEmpty = true;
  }

  public Tuple(T1 item1, T2 item2)
  {
    this.item1 = item1;
    this.item2 = item2;
  }

  public bool IsEmpty() => this.isEmpty;
}
