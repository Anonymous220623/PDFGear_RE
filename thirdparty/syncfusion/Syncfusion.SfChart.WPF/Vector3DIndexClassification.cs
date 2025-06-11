// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.Vector3DIndexClassification
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

internal class Vector3DIndexClassification
{
  private int index;
  private ClassifyPointResult result;
  private bool isCuttingBackPoint;
  private int cuttingBackPairIndex;
  private bool alreadyCuttedBack;
  private bool isCuttingFrontPoint;
  private int cuttingFrontPairIndex;
  private bool alreadyCuttedFront;

  public Vector3DIndexClassification(Vector3D point, int ind, ClassifyPointResult res)
  {
    this.Vector = point;
    this.index = ind;
    this.result = res;
  }

  public Vector3D Vector { get; set; }

  public int Index
  {
    get => this.index;
    set
    {
      if (this.index == value)
        return;
      this.index = value;
    }
  }

  public ClassifyPointResult Result
  {
    get => this.result;
    set
    {
      if (this.result == value)
        return;
      this.result = value;
    }
  }

  public bool CuttingBackPoint
  {
    get => this.isCuttingBackPoint;
    set
    {
      if (this.isCuttingBackPoint == value)
        return;
      this.isCuttingBackPoint = value;
    }
  }

  public bool CuttingFrontPoint
  {
    get => this.isCuttingFrontPoint;
    set
    {
      if (this.isCuttingFrontPoint == value)
        return;
      this.isCuttingFrontPoint = value;
    }
  }

  public int CuttingBackPairIndex
  {
    get => this.cuttingBackPairIndex;
    set
    {
      if (this.cuttingBackPairIndex == value)
        return;
      this.cuttingBackPairIndex = value;
    }
  }

  public int CuttingFrontPairIndex
  {
    get => this.cuttingFrontPairIndex;
    set
    {
      if (this.cuttingFrontPairIndex == value)
        return;
      this.cuttingFrontPairIndex = value;
    }
  }

  public bool AlreadyCuttedBack
  {
    get => this.alreadyCuttedBack;
    set
    {
      if (this.alreadyCuttedBack == value)
        return;
      this.alreadyCuttedBack = value;
    }
  }

  public bool AlreadyCuttedFront
  {
    get => this.alreadyCuttedFront;
    set
    {
      if (this.alreadyCuttedFront == value)
        return;
      this.alreadyCuttedFront = value;
    }
  }
}
