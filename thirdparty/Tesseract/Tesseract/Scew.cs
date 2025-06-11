// Decompiled with JetBrains decompiler
// Type: Tesseract.Scew
// Assembly: Tesseract, Version=4.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: C5D5562D-D917-402B-968F-9F8B28C3D951
// Assembly location: D:\PDFGear\bin\Tesseract.dll

#nullable disable
namespace Tesseract;

public struct Scew(float angle, float confidence)
{
  private float angle = angle;
  private float confidence = confidence;

  public float Angle => this.angle;

  public float Confidence => this.confidence;

  public override string ToString() => $"Scew: {this.Angle} [conf: {this.Confidence}]";

  public override bool Equals(object obj) => obj is Scew other && this.Equals(other);

  public bool Equals(Scew other)
  {
    return (double) this.confidence == (double) other.confidence && (double) this.angle == (double) other.angle;
  }

  public override int GetHashCode()
  {
    return 0 + 1000000007 * this.angle.GetHashCode() + 1000000009 * this.confidence.GetHashCode();
  }

  public static bool operator ==(Scew lhs, Scew rhs) => lhs.Equals(rhs);

  public static bool operator !=(Scew lhs, Scew rhs) => !(lhs == rhs);
}
