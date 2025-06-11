// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.CoveredCellRange
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class CoveredCellRange
{
  public CoveredCellRange()
  {
  }

  public CoveredCellRange(int top, int left, int bottom, int right)
  {
    this.Top = top;
    this.Bottom = bottom;
    this.Left = left;
    this.Right = right;
  }

  public int Top { get; set; }

  public int Left { get; set; }

  public int Bottom { get; set; }

  public int Right { get; set; }

  public override string ToString() => $"({this.Top},{this.Left})-({this.Bottom},{this.Right})";
}
