// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.EllipticCurveElements
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal abstract class EllipticCurveElements
{
  public abstract Number ToIntValue();

  public abstract string ECElementName { get; }

  public abstract int ElementSize { get; }

  public abstract EllipticCurveElements SumValue(EllipticCurveElements value);

  public abstract EllipticCurveElements Subtract(EllipticCurveElements value);

  public abstract EllipticCurveElements Multiply(EllipticCurveElements value);

  public abstract EllipticCurveElements Divide(EllipticCurveElements value);

  public abstract EllipticCurveElements Negate();

  public abstract EllipticCurveElements Square();

  public abstract EllipticCurveElements Invert();

  public abstract EllipticCurveElements SquareRoot();

  public override bool Equals(object element)
  {
    if (element == this)
      return true;
    return element is EllipticCurveElements element1 && this.Equals(element1);
  }

  protected bool Equals(EllipticCurveElements element)
  {
    return this.ToIntValue().Equals((object) element.ToIntValue());
  }

  public override int GetHashCode() => this.ToIntValue().GetHashCode();

  public override string ToString() => this.ToIntValue().ToString(2);
}
