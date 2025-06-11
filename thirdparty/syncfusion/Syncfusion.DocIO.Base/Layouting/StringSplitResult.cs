// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.StringSplitResult
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class StringSplitResult
{
  public TextLineInfo[] Lines;
  public string Remainder;
  public SizeF ActualSize;
  public float LineHeight;

  public bool Empty => this.Lines == null || this.Lines.Length == 0;

  public int Count => !this.Empty ? this.Lines.Length : 0;
}
