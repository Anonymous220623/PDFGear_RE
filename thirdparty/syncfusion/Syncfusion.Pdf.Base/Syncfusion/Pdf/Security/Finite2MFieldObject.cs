// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.Finite2MFieldObject
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class Finite2MFieldObject : EllipticCurveElements
{
  public const int intergerX = 1;
  public const int intergerY = 2;
  public const int intergerZ = 3;
  private int valueA;
  private int valueB;
  private int valueC;
  private int valueD;
  private int valueE;
  private PdfIntArray numberPX;
  private readonly int valueF;

  public Finite2MFieldObject(int valueB, int valueC, int valueD, int valueE, Number numberPX)
  {
    this.valueF = valueB + 31 /*0x1F*/ >> 5;
    this.numberPX = new PdfIntArray(numberPX, this.valueF);
    if (valueD == 0 && valueE == 0)
    {
      this.valueA = 2;
    }
    else
    {
      if (valueD >= valueE)
        throw new ArgumentException("value must be smaller");
      if (valueD <= 0)
        throw new ArgumentException("value must be larger than 0");
      this.valueA = 3;
    }
    if (numberPX.SignValue < 0)
      throw new ArgumentException("value cannot be negative");
    this.valueB = valueB;
    this.valueC = valueC;
    this.valueD = valueD;
    this.valueE = valueE;
  }

  private Finite2MFieldObject(
    int valueB,
    int valueC,
    int valueD,
    int valueE,
    PdfIntArray numberPX)
  {
    this.valueF = valueB + 31 /*0x1F*/ >> 5;
    this.numberPX = numberPX;
    this.valueB = valueB;
    this.valueC = valueC;
    this.valueD = valueD;
    this.valueE = valueE;
    if (valueD == 0 && valueE == 0)
      this.valueA = 2;
    else
      this.valueA = 3;
  }

  public override Number ToIntValue() => this.numberPX.ToBigInteger();

  public override string ECElementName => "F2m";

  public override int ElementSize => this.valueB;

  public static void ValidateElements(EllipticCurveElements curveA, EllipticCurveElements value)
  {
    Finite2MFieldObject finite2MfieldObject1 = curveA is Finite2MFieldObject && value is Finite2MFieldObject ? (Finite2MFieldObject) curveA : throw new ArgumentException(nameof (Finite2MFieldObject));
    Finite2MFieldObject finite2MfieldObject2 = (Finite2MFieldObject) value;
    if (finite2MfieldObject1.valueB != finite2MfieldObject2.valueB || finite2MfieldObject1.valueC != finite2MfieldObject2.valueC || finite2MfieldObject1.valueD != finite2MfieldObject2.valueD || finite2MfieldObject1.valueE != finite2MfieldObject2.valueE)
      throw new ArgumentException("F2M field");
    if (finite2MfieldObject1.valueA != finite2MfieldObject2.valueA)
      throw new ArgumentException("elements has incorrect");
  }

  public override EllipticCurveElements SumValue(EllipticCurveElements value)
  {
    PdfIntArray numberPX = this.numberPX.Copy();
    Finite2MFieldObject finite2MfieldObject = (Finite2MFieldObject) value;
    numberPX.AddShifted(finite2MfieldObject.numberPX, 0);
    return (EllipticCurveElements) new Finite2MFieldObject(this.valueB, this.valueC, this.valueD, this.valueE, numberPX);
  }

  public override EllipticCurveElements Subtract(EllipticCurveElements value)
  {
    return this.SumValue(value);
  }

  public override EllipticCurveElements Multiply(EllipticCurveElements value)
  {
    PdfIntArray numberPX = this.numberPX.Multiply(((Finite2MFieldObject) value).numberPX, this.valueB);
    numberPX.Reduce(this.valueB, new int[3]
    {
      this.valueC,
      this.valueD,
      this.valueE
    });
    return (EllipticCurveElements) new Finite2MFieldObject(this.valueB, this.valueC, this.valueD, this.valueE, numberPX);
  }

  public override EllipticCurveElements Divide(EllipticCurveElements value)
  {
    return this.Multiply(value.Invert());
  }

  public override EllipticCurveElements Negate() => (EllipticCurveElements) this;

  public override EllipticCurveElements Square()
  {
    PdfIntArray numberPX = this.numberPX.Square(this.valueB);
    numberPX.Reduce(this.valueB, new int[3]
    {
      this.valueC,
      this.valueD,
      this.valueE
    });
    return (EllipticCurveElements) new Finite2MFieldObject(this.valueB, this.valueC, this.valueD, this.valueE, numberPX);
  }

  public override EllipticCurveElements Invert()
  {
    PdfIntArray pdfIntArray1 = this.numberPX.Copy();
    PdfIntArray pdfIntArray2 = new PdfIntArray(this.valueF);
    pdfIntArray2.SetBit(this.valueB);
    pdfIntArray2.SetBit(0);
    pdfIntArray2.SetBit(this.valueC);
    if (this.valueA == 3)
    {
      pdfIntArray2.SetBit(this.valueD);
      pdfIntArray2.SetBit(this.valueE);
    }
    PdfIntArray pdfIntArray3 = new PdfIntArray(this.valueF);
    pdfIntArray3.SetBit(0);
    PdfIntArray numberPX = new PdfIntArray(this.valueF);
    while (pdfIntArray1.GetLength() > 0)
    {
      int num = pdfIntArray1.BitLength - pdfIntArray2.BitLength;
      if (num < 0)
      {
        PdfIntArray pdfIntArray4 = pdfIntArray1;
        pdfIntArray1 = pdfIntArray2;
        pdfIntArray2 = pdfIntArray4;
        PdfIntArray pdfIntArray5 = pdfIntArray3;
        pdfIntArray3 = numberPX;
        numberPX = pdfIntArray5;
        num = -num;
      }
      int shift = num >> 5;
      int number = num & 31 /*0x1F*/;
      PdfIntArray values1 = pdfIntArray2.ShiftLeft(number);
      pdfIntArray1.AddShifted(values1, shift);
      PdfIntArray values2 = numberPX.ShiftLeft(number);
      pdfIntArray3.AddShifted(values2, shift);
    }
    return (EllipticCurveElements) new Finite2MFieldObject(this.valueB, this.valueC, this.valueD, this.valueE, numberPX);
  }

  public override EllipticCurveElements SquareRoot()
  {
    throw new ArithmeticException("Function not implemented");
  }

  public override bool Equals(object element)
  {
    if (element == this)
      return true;
    return element is Finite2MFieldObject element1 && this.Equals(element1);
  }

  protected bool Equals(Finite2MFieldObject element)
  {
    return this.valueB == element.valueB && this.valueC == element.valueC && this.valueD == element.valueD && this.valueE == element.valueE && this.valueA == element.valueA && this.Equals((EllipticCurveElements) element);
  }

  public override int GetHashCode()
  {
    return this.valueB.GetHashCode() ^ this.valueC.GetHashCode() ^ this.valueD.GetHashCode() ^ this.valueE.GetHashCode() ^ this.valueA.GetHashCode() ^ base.GetHashCode();
  }
}
