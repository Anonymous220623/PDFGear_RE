// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.EllipticGOSTCurves
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal sealed class EllipticGOSTCurves
{
  internal static readonly IDictionary curveIds = (IDictionary) new Hashtable();
  internal static readonly IDictionary parameters = (IDictionary) new Hashtable();
  internal static readonly IDictionary curveNames = (IDictionary) new Hashtable();

  private EllipticGOSTCurves()
  {
  }

  static EllipticGOSTCurves()
  {
    Number number1 = new Number(PdfHexEncoder.DecodeString("MTE1NzkyMDg5MjM3MzE2MTk1NDIzNTcwOTg1MDA4Njg3OTA3ODUzMjY5OTg0NjY1NjQwNTY0MDM5NDU3NTg0MDA3OTEzMTI5NjM5MzE5"));
    Number numberX1 = new Number(PdfHexEncoder.DecodeString("MTE1NzkyMDg5MjM3MzE2MTk1NDIzNTcwOTg1MDA4Njg3OTA3ODUzMDczNzYyOTA4NDk5MjQzMjI1Mzc4MTU1ODA1MDc5MDY4ODUwMzIz"));
    FiniteCurves ecCurve1 = new FiniteCurves(number1, new Number(PdfHexEncoder.DecodeString("MTE1NzkyMDg5MjM3MzE2MTk1NDIzNTcwOTg1MDA4Njg3OTA3ODUzMjY5OTg0NjY1NjQwNTY0MDM5NDU3NTg0MDA3OTEzMTI5NjM5MzE2")), new Number("166"));
    EllipticCurveParams ellipticCurveParams1 = new EllipticCurveParams((EllipticCurves) ecCurve1, ecCurve1.ECPoints(Number.One, new Number(PdfHexEncoder.DecodeString("NjQwMzM4ODExNDI5MjcyMDI2ODM2NDk4ODE0NTA0MzM0NzM5ODU5MzE3NjAyNjg4ODQ5NDEyODg4NTI3NDU4MDM5MDg4Nzg2Mzg2MTI=")), false), numberX1);
    EllipticGOSTCurves.parameters[(object) CRYPTOIDs.IDR3410A] = (object) ellipticCurveParams1;
    Number number2 = new Number(PdfHexEncoder.DecodeString("MTE1NzkyMDg5MjM3MzE2MTk1NDIzNTcwOTg1MDA4Njg3OTA3ODUzMjY5OTg0NjY1NjQwNTY0MDM5NDU3NTg0MDA3OTEzMTI5NjM5MzE5"));
    Number numberX2 = new Number(PdfHexEncoder.DecodeString("MTE1NzkyMDg5MjM3MzE2MTk1NDIzNTcwOTg1MDA4Njg3OTA3ODUzMDczNzYyOTA4NDk5MjQzMjI1Mzc4MTU1ODA1MDc5MDY4ODUwMzIz"));
    FiniteCurves ecCurve2 = new FiniteCurves(number2, new Number(PdfHexEncoder.DecodeString("MTE1NzkyMDg5MjM3MzE2MTk1NDIzNTcwOTg1MDA4Njg3OTA3ODUzMjY5OTg0NjY1NjQwNTY0MDM5NDU3NTg0MDA3OTEzMTI5NjM5MzE2")), new Number("166"));
    EllipticCurveParams ellipticCurveParams2 = new EllipticCurveParams((EllipticCurves) ecCurve2, ecCurve2.ECPoints(Number.One, new Number(PdfHexEncoder.DecodeString("NjQwMzM4ODExNDI5MjcyMDI2ODM2NDk4ODE0NTA0MzM0NzM5ODU5MzE3NjAyNjg4ODQ5NDEyODg4NTI3NDU4MDM5MDg4Nzg2Mzg2MTI=")), false), numberX2);
    EllipticGOSTCurves.parameters[(object) CRYPTOIDs.IDR3410XA] = (object) ellipticCurveParams2;
    Number number3 = new Number(PdfHexEncoder.DecodeString("NTc4OTYwNDQ2MTg2NTgwOTc3MTE3ODU0OTI1MDQzNDM5NTM5MjY2MzQ5OTIzMzI4MjAyODIwMTk3Mjg3OTIwMDM5NTY1NjQ4MjMxOTM="));
    Number numberX3 = new Number(PdfHexEncoder.DecodeString("NTc4OTYwNDQ2MTg2NTgwOTc3MTE3ODU0OTI1MDQzNDM5NTM5MjcxMDIxMzMxNjAyNTU4MjY4MjAwNjg4NDQ0OTYwODc3MzIwNjY3MDM="));
    FiniteCurves ecCurve3 = new FiniteCurves(number3, new Number(PdfHexEncoder.DecodeString("NTc4OTYwNDQ2MTg2NTgwOTc3MTE3ODU0OTI1MDQzNDM5NTM5MjY2MzQ5OTIzMzI4MjAyODIwMTk3Mjg3OTIwMDM5NTY1NjQ4MjMxOTA=")), new Number(PdfHexEncoder.DecodeString("MjgwOTEwMTkzNTMwNTgwOTAwOTY5OTY5NzkwMDAzMDk1NjA3NTkxMjQzNjg1NTgwMTQ4NjU5NTc2NTU4NDI4NzIzOTczMDEyNjc1OTU=")));
    EllipticCurveParams ellipticCurveParams3 = new EllipticCurveParams((EllipticCurves) ecCurve3, ecCurve3.ECPoints(Number.One, new Number(PdfHexEncoder.DecodeString("Mjg3OTI2NjU4MTQ4NTQ2MTEyOTY5OTIzNDc0NTgzODAyODQxMzUwMjg2MzY3NzgyMjkxMTMwMDU3NTYzMzQ3MzA5OTYzMDM4ODgxMjQ=")), false), numberX3);
    EllipticGOSTCurves.parameters[(object) CRYPTOIDs.IDR3410B] = (object) ellipticCurveParams3;
    Number number4 = new Number(PdfHexEncoder.DecodeString("NzAzOTAwODUzNTIwODMzMDUxOTk1NDc3MTgwMTkwMTg0Mzc4NDEwNzk1MTY2MzAwNDUxODA0NzEyODQzNDY4NDM3MDU2MzM1MDI2MTk="));
    Number numberX4 = new Number(PdfHexEncoder.DecodeString("NzAzOTAwODUzNTIwODMzMDUxOTk1NDc3MTgwMTkwMTg0Mzc4NDA5MjA4ODI2NDcxNjQwODEwMzUzMjI2MDE0NTgzNTIyOTgzOTY2MDE="));
    FiniteCurves ecCurve4 = new FiniteCurves(number4, new Number(PdfHexEncoder.DecodeString("NzAzOTAwODUzNTIwODMzMDUxOTk1NDc3MTgwMTkwMTg0Mzc4NDEwNzk1MTY2MzAwNDUxODA0NzEyODQzNDY4NDM3MDU2MzM1MDI2MTY=")), new Number("32858"));
    EllipticCurveParams ellipticCurveParams4 = new EllipticCurveParams((EllipticCurves) ecCurve4, ecCurve4.ECPoints(Number.Zero, new Number(PdfHexEncoder.DecodeString("Mjk4MTg4OTM5MTc3MzEyNDA3MzM0NzEyNzMyNDAzMTQ3Njk5MjcyNDA1NTA4MTIzODM2OTU2ODkxNDY0OTUyNjE2MDQ1NjU5OTAyNDc=")), false), numberX4);
    EllipticGOSTCurves.parameters[(object) CRYPTOIDs.IDR3410XB] = (object) ellipticCurveParams4;
    Number number5 = new Number(PdfHexEncoder.DecodeString("NzAzOTAwODUzNTIwODMzMDUxOTk1NDc3MTgwMTkwMTg0Mzc4NDEwNzk1MTY2MzAwNDUxODA0NzEyODQzNDY4NDM3MDU2MzM1MDI2MTk="));
    Number numberX5 = new Number(PdfHexEncoder.DecodeString("NzAzOTAwODUzNTIwODMzMDUxOTk1NDc3MTgwMTkwMTg0Mzc4NDA5MjA4ODI2NDcxNjQwODEwMzUzMjI2MDE0NTgzNTIyOTgzOTY2MDE="));
    FiniteCurves ecCurve5 = new FiniteCurves(number5, new Number(PdfHexEncoder.DecodeString("NzAzOTAwODUzNTIwODMzMDUxOTk1NDc3MTgwMTkwMTg0Mzc4NDEwNzk1MTY2MzAwNDUxODA0NzEyODQzNDY4NDM3MDU2MzM1MDI2MTY=")), new Number("32858"));
    EllipticCurveParams ellipticCurveParams5 = new EllipticCurveParams((EllipticCurves) ecCurve5, ecCurve5.ECPoints(Number.Zero, new Number(PdfHexEncoder.DecodeString("Mjk4MTg4OTM5MTc3MzEyNDA3MzM0NzEyNzMyNDAzMTQ3Njk5MjcyNDA1NTA4MTIzODM2OTU2ODkxNDY0OTUyNjE2MDQ1NjU5OTAyNDc=")), false), numberX5);
    EllipticGOSTCurves.parameters[(object) CRYPTOIDs.IDR3410C] = (object) ellipticCurveParams5;
    EllipticGOSTCurves.curveIds[(object) "IDR3410-2001-CryptoPro-A"] = (object) CRYPTOIDs.IDR3410A;
    EllipticGOSTCurves.curveIds[(object) "IDR3410-2001-CryptoPro-B"] = (object) CRYPTOIDs.IDR3410B;
    EllipticGOSTCurves.curveIds[(object) "IDR3410-2001-CryptoPro-C"] = (object) CRYPTOIDs.IDR3410C;
    EllipticGOSTCurves.curveIds[(object) "IDR3410-2001-CryptoPro-XchA"] = (object) CRYPTOIDs.IDR3410XA;
    EllipticGOSTCurves.curveIds[(object) "IDR3410-2001-CryptoPro-XchB"] = (object) CRYPTOIDs.IDR3410XB;
    EllipticGOSTCurves.curveNames[(object) CRYPTOIDs.IDR3410A] = (object) "IDR3410-2001-CryptoPro-A";
    EllipticGOSTCurves.curveNames[(object) CRYPTOIDs.IDR3410B] = (object) "IDR3410-2001-CryptoPro-B";
    EllipticGOSTCurves.curveNames[(object) CRYPTOIDs.IDR3410C] = (object) "IDR3410-2001-CryptoPro-C";
    EllipticGOSTCurves.curveNames[(object) CRYPTOIDs.IDR3410XA] = (object) "IDR3410-2001-CryptoPro-XchA";
    EllipticGOSTCurves.curveNames[(object) CRYPTOIDs.IDR3410XB] = (object) "IDR3410-2001-CryptoPro-XchB";
  }

  public static EllipticCurveParams GetByOid(DerObjectID oid)
  {
    return (EllipticCurveParams) EllipticGOSTCurves.parameters[(object) oid];
  }
}
