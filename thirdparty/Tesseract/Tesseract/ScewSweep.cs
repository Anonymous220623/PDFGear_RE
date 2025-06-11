// Decompiled with JetBrains decompiler
// Type: Tesseract.ScewSweep
// Assembly: Tesseract, Version=4.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: C5D5562D-D917-402B-968F-9F8B28C3D951
// Assembly location: D:\PDFGear\bin\Tesseract.dll

#nullable disable
namespace Tesseract;

public struct ScewSweep(int reduction = 4, float range = 7f, float delta = 1f)
{
  public static ScewSweep Default = new ScewSweep();
  public const int DefaultReduction = 4;
  public const float DefaultRange = 7f;
  public const float DefaultDelta = 1f;
  private int reduction = reduction;
  private float range = range;
  private float delta = delta;

  public int Reduction => this.reduction;

  public float Range => this.range;

  public float Delta => this.delta;
}
