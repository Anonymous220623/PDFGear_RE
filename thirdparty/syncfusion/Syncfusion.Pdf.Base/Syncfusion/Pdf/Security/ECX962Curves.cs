// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.ECX962Curves
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal sealed class ECX962Curves
{
  internal static readonly IDictionary objectIds = (IDictionary) new Hashtable();
  internal static readonly IDictionary m_curves = (IDictionary) new Hashtable();
  internal static readonly IDictionary curveNames = (IDictionary) new Hashtable();

  private ECX962Curves()
  {
  }

  private static void CreateCurve(string name, DerObjectID oid, ECX9Params holder)
  {
    ECX962Curves.objectIds.Add((object) name, (object) oid);
    ECX962Curves.curveNames.Add((object) oid, (object) name);
    ECX962Curves.m_curves.Add((object) oid, (object) holder);
  }

  static ECX962Curves()
  {
    ECX962Curves.CreateCurve("prime192v1", ECDSAOIDs.ECPC192v1, ECX962Curves.ECPrime191V1.primeField);
    ECX962Curves.CreateCurve("prime192v2", ECDSAOIDs.ECPC192v2, ECX962Curves.ECPrime191V2.primeField);
    ECX962Curves.CreateCurve("prime192v3", ECDSAOIDs.ECPC192v3, ECX962Curves.ECPrime191V3.primeField);
    ECX962Curves.CreateCurve("prime239v1", ECDSAOIDs.ECPC239v1, ECX962Curves.ECPrime239V1.primeField);
    ECX962Curves.CreateCurve("prime239v2", ECDSAOIDs.ECPC239v2, ECX962Curves.ECPrime239V2.primeField);
    ECX962Curves.CreateCurve("prime239v3", ECDSAOIDs.ECPC239v3, ECX962Curves.ECPrime239V3.primeField);
    ECX962Curves.CreateCurve("prime256v1", ECDSAOIDs.ECPC256v1, ECX962Curves.ECPrime256V1.primeField);
    ECX962Curves.CreateCurve("c2pnb163v1", ECDSAOIDs.ECP163v1, ECX962Curves.ECPrime163V1.primeField);
    ECX962Curves.CreateCurve("c2pnb163v2", ECDSAOIDs.ECP163v2, ECX962Curves.ECPrime163V2.primeField);
    ECX962Curves.CreateCurve("c2pnb163v3", ECDSAOIDs.ECP163v3, ECX962Curves.ECPrime163V3.primeField);
    ECX962Curves.CreateCurve("c2pnb176w1", ECDSAOIDs.ECP176w1, ECX962Curves.ECPrime176W1.primeField);
    ECX962Curves.CreateCurve("c2tnb191v1", ECDSAOIDs.ECP191v1, ECX962Curves.ECPrimeT191V1.primeField);
    ECX962Curves.CreateCurve("c2tnb191v2", ECDSAOIDs.ECP191v2, ECX962Curves.ECPrimeT191V2.primeField);
    ECX962Curves.CreateCurve("c2tnb191v3", ECDSAOIDs.ECP191v3, ECX962Curves.ECPrimeT191W1.primeField);
    ECX962Curves.CreateCurve("c2pnb208w1", ECDSAOIDs.ECP208w1, ECX962Curves.ECPrimeT208W1.primeField);
    ECX962Curves.CreateCurve("c2tnb239v1", ECDSAOIDs.ECP239v1, ECX962Curves.ECPrimeT239V1.primeField);
    ECX962Curves.CreateCurve("c2tnb239v2", ECDSAOIDs.ECP239v2, ECX962Curves.ECPrimeT239V2.primeField);
    ECX962Curves.CreateCurve("c2tnb239v3", ECDSAOIDs.ECP239v3, ECX962Curves.ECPrimeT239.primeField);
    ECX962Curves.CreateCurve("c2pnb272w1", ECDSAOIDs.ECP272w1, ECX962Curves.ECPrimeT272.primeField);
    ECX962Curves.CreateCurve("c2pnb304w1", ECDSAOIDs.ECP304w1, ECX962Curves.ECPrimeT304.primeField);
    ECX962Curves.CreateCurve("c2tnb359v1", ECDSAOIDs.ECP359v1, ECX962Curves.ECPrimeT359.primeField);
    ECX962Curves.CreateCurve("c2pnb368w1", ECDSAOIDs.ECP368w1, ECX962Curves.ECPrimeT368.primeField);
    ECX962Curves.CreateCurve("c2tnb431r1", ECDSAOIDs.ECP431r1, ECX962Curves.ECPrimeT431.primeField);
  }

  public static ECX9Field GetByOid(DerObjectID oid)
  {
    return ((ECX9Params) ECX962Curves.m_curves[(object) oid])?.Parameters;
  }

  internal class ECPrime191V1 : ECX9Params
  {
    internal static readonly ECX9Params primeField = (ECX9Params) new ECX962Curves.ECPrime191V1();

    private ECPrime191V1()
    {
    }

    protected override ECX9Field DefineParameters()
    {
      EllipticCurves curve = (EllipticCurves) new FiniteCurves(new Number(PdfHexEncoder.DecodeString("NjI3NzEwMTczNTM4NjY4MDc2MzgzNTc4OTQyMzIwNzY2NjQxNjA4MzkwODcwMDM5MDMyNDk2MTI3OQ==")), new Number(PdfHexEncoder.DecodeString("ZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmVmZmZmZmZmZmZmZmZmZmZj"), 16 /*0x10*/), new Number(PdfHexEncoder.DecodeString("NjQyMTA1MTllNTljODBlNzBmYTdlOWFiNzIyNDMwNDlmZWI4ZGVlY2MxNDZiOWIx"), 16 /*0x10*/));
      return new ECX9Field(curve, curve.GetDecodedECPoint(PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("MDMxODhkYTgwZWIwMzA5MGY2N2NiZjIwZWI0M2ExODgwMGY0ZmYwYWZkODJmZjEwMTI="))), new Number(PdfHexEncoder.DecodeString("ZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmOTlkZWY4MzYxNDZiYzliMWI0ZDIyODMx"), 16 /*0x10*/), Number.One, PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("MzA0NUFFNkZDODQyMmY2NEVENTc5NTI4RDM4MTIwRUFFMTIxOTZENQ==")));
    }
  }

  internal class ECPrime191V2 : ECX9Params
  {
    internal static readonly ECX9Params primeField = (ECX9Params) new ECX962Curves.ECPrime191V2();

    private ECPrime191V2()
    {
    }

    protected override ECX9Field DefineParameters()
    {
      EllipticCurves curve = (EllipticCurves) new FiniteCurves(new Number(PdfHexEncoder.DecodeString("NjI3NzEwMTczNTM4NjY4MDc2MzgzNTc4OTQyMzIwNzY2NjQxNjA4MzkwODcwMDM5MDMyNDk2MTI3OQ==")), new Number(PdfHexEncoder.DecodeString("ZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmVmZmZmZmZmZmZmZmZmZmZj"), 16 /*0x10*/), new Number(PdfHexEncoder.DecodeString("Y2MyMmQ2ZGZiOTVjNmIyNWU0OWMwZDYzNjRhNGU1OTgwYzM5M2FhMjE2NjhkOTUz"), 16 /*0x10*/));
      return new ECX9Field(curve, curve.GetDecodedECPoint(PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("MDNlZWEyYmFlN2UxNDk3ODQyZjJkZTc3NjljZmU5Yzk4OWMwNzJhZDY5NmY0ODAzNGE="))), new Number(PdfHexEncoder.DecodeString("ZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmZlNWZiMWE3MjRkYzgwNDE4NjQ4ZDhkZDMx"), 16 /*0x10*/), Number.One, PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("MzFhOTJlZTIwMjlmZDEwZDkwMWIxMTNlOTkwNzEwZjBkMjFhYzZiNg==")));
    }
  }

  internal class ECPrime191V3 : ECX9Params
  {
    internal static readonly ECX9Params primeField = (ECX9Params) new ECX962Curves.ECPrime191V3();

    private ECPrime191V3()
    {
    }

    protected override ECX9Field DefineParameters()
    {
      EllipticCurves curve = (EllipticCurves) new FiniteCurves(new Number(PdfHexEncoder.DecodeString("NjI3NzEwMTczNTM4NjY4MDc2MzgzNTc4OTQyMzIwNzY2NjQxNjA4MzkwODcwMDM5MDMyNDk2MTI3OQ==")), new Number(PdfHexEncoder.DecodeString("ZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmVmZmZmZmZmZmZmZmZmZmZj"), 16 /*0x10*/), new Number(PdfHexEncoder.DecodeString("MjIxMjNkYzIzOTVhMDVjYWE3NDIzZGFlY2NjOTQ3NjBhN2Q0NjIyNTZiZDU2OTE2"), 16 /*0x10*/));
      return new ECX9Field(curve, curve.GetDecodedECPoint(PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("MDI3ZDI5Nzc4MTAwYzY1YTFkYTE3ODM3MTY1ODhkY2UyYjhiNGFlZThlMjI4ZjE4OTY="))), new Number(PdfHexEncoder.DecodeString("ZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmN2E2MmQwMzFjODNmNDI5NGY2NDBlYzEz"), 16 /*0x10*/), Number.One, PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("YzQ2OTY4NDQzNWRlYjM3OGM0YjY1Y2E5NTkxZTJhNTc2MzA1OWEyZQ==")));
    }
  }

  internal class ECPrime239V1 : ECX9Params
  {
    internal static readonly ECX9Params primeField = (ECX9Params) new ECX962Curves.ECPrime239V1();

    private ECPrime239V1()
    {
    }

    protected override ECX9Field DefineParameters()
    {
      EllipticCurves curve = (EllipticCurves) new FiniteCurves(new Number(PdfHexEncoder.DecodeString("ODgzNDIzNTMyMzg5MTkyMTY0NzkxNjQ4NzUwMzYwMzA4ODg1MzE0NDc2NTk3MjUyOTYwMzYyNzkyNDUwODYwNjA5Njk5ODM5")), new Number(PdfHexEncoder.DecodeString("N2ZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmN2ZmZmZmZmZmZmZmODAwMDAwMDAwMDAwN2ZmZmZmZmZmZmZj"), 16 /*0x10*/), new Number(PdfHexEncoder.DecodeString("NmIwMTZjM2JkY2YxODk0MWQwZDY1NDkyMTQ3NWNhNzFhOWRiMmZiMjdkMWQzNzc5NjE4NWMyOTQyYzBh"), 16 /*0x10*/));
      return new ECX9Field(curve, curve.GetDecodedECPoint(PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("MDIwZmZhOTYzY2RjYTg4MTZjY2MzM2I4NjQyYmVkZjkwNWMzZDM1ODU3M2QzZjI3ZmJiZDNiM2NiOWFhYWY="))), new Number(PdfHexEncoder.DecodeString("N2ZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmN2ZmZmZmOWU1ZTlhOWY1ZDkwNzFmYmQxNTIyNjg4OTA5ZDBi"), 16 /*0x10*/), Number.One, PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("ZTQzYmI0NjBmMGI4MGNjMGMwYjA3NTc5OGU5NDgwNjBmODMyMWI3ZA==")));
    }
  }

  internal class ECPrime239V2 : ECX9Params
  {
    internal static readonly ECX9Params primeField = (ECX9Params) new ECX962Curves.ECPrime239V2();

    private ECPrime239V2()
    {
    }

    protected override ECX9Field DefineParameters()
    {
      EllipticCurves curve = (EllipticCurves) new FiniteCurves(new Number(PdfHexEncoder.DecodeString("ODgzNDIzNTMyMzg5MTkyMTY0NzkxNjQ4NzUwMzYwMzA4ODg1MzE0NDc2NTk3MjUyOTYwMzYyNzkyNDUwODYwNjA5Njk5ODM5")), new Number(PdfHexEncoder.DecodeString("N2ZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmN2ZmZmZmZmZmZmZmODAwMDAwMDAwMDAwN2ZmZmZmZmZmZmZj"), 16 /*0x10*/), new Number(PdfHexEncoder.DecodeString("NjE3ZmFiNjgzMjU3NmNiYmZlZDUwZDk5ZjAyNDljM2ZlZTU4Yjk0YmEwMDM4YzdhZTg0YzhjODMyZjJj"), 16 /*0x10*/));
      return new ECX9Field(curve, curve.GetDecodedECPoint(PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("MDIzOGFmMDlkOTg3Mjc3MDUxMjBjOTIxYmI1ZTllMjYyOTZhM2NkY2YyZjM1NzU3YTBlYWZkODdiODMwZTc="))), new Number(PdfHexEncoder.DecodeString("N2ZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmODAwMDAwY2ZhN2U4NTk0Mzc3ZDQxNGMwMzgyMWJjNTgyMDYz"), 16 /*0x10*/), Number.One, PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("ZThiNDAxMTYwNDA5NTMwM2NhM2I4MDk5OTgyYmUwOWZjYjlhZTYxNg==")));
    }
  }

  internal class ECPrime239V3 : ECX9Params
  {
    internal static readonly ECX9Params primeField = (ECX9Params) new ECX962Curves.ECPrime239V3();

    private ECPrime239V3()
    {
    }

    protected override ECX9Field DefineParameters()
    {
      EllipticCurves curve = (EllipticCurves) new FiniteCurves(new Number(PdfHexEncoder.DecodeString("ODgzNDIzNTMyMzg5MTkyMTY0NzkxNjQ4NzUwMzYwMzA4ODg1MzE0NDc2NTk3MjUyOTYwMzYyNzkyNDUwODYwNjA5Njk5ODM5")), new Number(PdfHexEncoder.DecodeString("N2ZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmN2ZmZmZmZmZmZmZmODAwMDAwMDAwMDAwN2ZmZmZmZmZmZmZj"), 16 /*0x10*/), new Number(PdfHexEncoder.DecodeString("MjU1NzA1ZmEyYTMwNjY1NGIxZjRjYjAzZDZhNzUwYTMwYzI1MDEwMmQ0OTg4NzE3ZDliYTE1YWI2ZDNl"), 16 /*0x10*/));
      return new ECX9Field(curve, curve.GetDecodedECPoint(PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("MDM2NzY4YWU4ZTE4YmI5MmNmY2YwMDVjOTQ5YWEyYzZkOTQ4NTNkMGU2NjBiYmY4NTRiMWM5NTA1ZmU5NWE="))), new Number(PdfHexEncoder.DecodeString("N2ZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmN2ZmZmZmOTc1ZGViNDFiM2E2MDU3YzNjNDMyMTQ2NTI2NTUx"), 16 /*0x10*/), Number.One, PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("N2Q3Mzc0MTY4ZmZlMzQ3MWI2MGE4NTc2ODZhMTk0NzVkM2JmYTJmZg==")));
    }
  }

  internal class ECPrime256V1 : ECX9Params
  {
    internal static readonly ECX9Params primeField = (ECX9Params) new ECX962Curves.ECPrime256V1();

    private ECPrime256V1()
    {
    }

    protected override ECX9Field DefineParameters()
    {
      EllipticCurves curve = (EllipticCurves) new FiniteCurves(new Number(PdfHexEncoder.DecodeString("MTE1NzkyMDg5MjEwMzU2MjQ4NzYyNjk3NDQ2OTQ5NDA3NTczNTMwMDg2MTQzNDE1MjkwMzE0MTk1NTMzNjMxMzA4ODY3MDk3ODUzOTUx")), new Number(PdfHexEncoder.DecodeString("ZmZmZmZmZmYwMDAwMDAwMTAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMGZmZmZmZmZmZmZmZmZmZmZmZmZmZmZmYw=="), 16 /*0x10*/), new Number(PdfHexEncoder.DecodeString("NWFjNjM1ZDhhYTNhOTNlN2IzZWJiZDU1NzY5ODg2YmM2NTFkMDZiMGNjNTNiMGY2M2JjZTNjM2UyN2QyNjA0Yg=="), 16 /*0x10*/));
      return new ECX9Field(curve, curve.GetDecodedECPoint(PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("MDM2YjE3ZDFmMmUxMmM0MjQ3ZjhiY2U2ZTU2M2E0NDBmMjc3MDM3ZDgxMmRlYjMzYTBmNGExMzk0NWQ4OThjMjk2"))), new Number(PdfHexEncoder.DecodeString("ZmZmZmZmZmYwMDAwMDAwMGZmZmZmZmZmZmZmZmZmZmZiY2U2ZmFhZGE3MTc5ZTg0ZjNiOWNhYzJmYzYzMjU1MQ=="), 16 /*0x10*/), Number.One, PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("YzQ5ZDM2MDg4NmU3MDQ5MzZhNjY3OGUxMTM5ZDI2Yjc4MTlmN2U5MA==")));
    }
  }

  internal class ECPrime163V1 : ECX9Params
  {
    internal static readonly ECX9Params primeField = (ECX9Params) new ECX962Curves.ECPrime163V1();

    private ECPrime163V1()
    {
    }

    protected override ECX9Field DefineParameters()
    {
      Number number1 = new Number(PdfHexEncoder.DecodeString("MDQwMDAwMDAwMDAwMDAwMDAwMDAwMUU2MEZDODgyMUNDNzREQUVBRkMx"), 16 /*0x10*/);
      Number number2 = Number.ValueOf(2L);
      EllipticCurves curve = (EllipticCurves) new Field2MCurves(163, 1, 2, 8, new Number(PdfHexEncoder.DecodeString("MDcyNTQ2QjU0MzUyMzRBNDIyRTA3ODk2NzVGNDMyQzg5NDM1REU1MjQy"), 16 /*0x10*/), new Number(PdfHexEncoder.DecodeString("MDBDOTUxN0QwNkQ1MjQwRDNDRkYzOEM3NEIyMEI2Q0Q0RDZGOURENEQ5"), 16 /*0x10*/), number1, number2);
      return new ECX9Field(curve, curve.GetDecodedECPoint(PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("MDMwN0FGNjk5ODk1NDYxMDNENzkzMjlGQ0MzRDc0ODgwRjMzQkJFODAzQ0I="))), number1, number2, PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("RDJDT0ZCMTU3NjA4NjBERUYxRUVGNEQ2OTZFNjc2ODc1NjE1MTc1NA==")));
    }
  }

  internal class ECPrime163V2 : ECX9Params
  {
    internal static readonly ECX9Params primeField = (ECX9Params) new ECX962Curves.ECPrime163V2();

    private ECPrime163V2()
    {
    }

    protected override ECX9Field DefineParameters()
    {
      Number number1 = new Number(PdfHexEncoder.DecodeString("MDNGRkZGRkZGRkZGRkZGRkZGRkZGREY2NERFMTE1MUFEQkI3OEYxMEE3"), 16 /*0x10*/);
      Number number2 = Number.ValueOf(2L);
      EllipticCurves curve = (EllipticCurves) new Field2MCurves(163, 1, 2, 8, new Number(PdfHexEncoder.DecodeString("MDEwOEIzOUU3N0M0QjEwOEJFRDk4MUVEMEU4OTBFMTE3QzUxMUNGMDcy"), 16 /*0x10*/), new Number(PdfHexEncoder.DecodeString("MDY2N0FDRUIzOEFGNEU0ODhDNDA3NDMzRkZBRTRGMUM4MTE2MzhERjIw"), 16 /*0x10*/), number1, number2);
      return new ECX9Field(curve, curve.GetDecodedECPoint(PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("MDMwMDI0MjY2RTRFQjUxMDZEMEE5NjREOTJDNDg2MEUyNjcxREI5QjZDQzU="))), number1, number2, (byte[]) null);
    }
  }

  internal class ECPrime163V3 : ECX9Params
  {
    internal static readonly ECX9Params primeField = (ECX9Params) new ECX962Curves.ECPrime163V3();

    private ECPrime163V3()
    {
    }

    protected override ECX9Field DefineParameters()
    {
      Number number1 = new Number(PdfHexEncoder.DecodeString("MDNGRkZGRkZGRkZGRkZGRkZGRkZGRTFBRUUxNDBGMTEwQUZGOTYxMzA5"), 16 /*0x10*/);
      Number number2 = Number.ValueOf(2L);
      EllipticCurves curve = (EllipticCurves) new Field2MCurves(163, 1, 2, 8, new Number(PdfHexEncoder.DecodeString("MDdBNTI2QzYzRDNFMjVBMjU2QTAwNzY5OUY1NDQ3RTMyQUU0NTZCNTBF"), 16 /*0x10*/), new Number(PdfHexEncoder.DecodeString("MDNGNzA2MTc5OEVCOTlFMjM4RkQ2RjFCRjk1QjQ4RkVFQjQ4NTQyNTJC"), 16 /*0x10*/), number1, number2);
      return new ECX9Field(curve, curve.GetDecodedECPoint(PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("MDIwMkY5Rjg3QjdDNTc0RDBCREVDRjhBMjJFNjUyNDc3NUY5OENERUJEQ0I="))), number1, number2, (byte[]) null);
    }
  }

  internal class ECPrime176W1 : ECX9Params
  {
    internal static readonly ECX9Params primeField = (ECX9Params) new ECX962Curves.ECPrime176W1();

    private ECPrime176W1()
    {
    }

    protected override ECX9Field DefineParameters()
    {
      Number number1 = new Number(PdfHexEncoder.DecodeString("MDEwMDkyNTM3Mzk3RUNBNEY2MTQ1Nzk5RDYyQjBBMTlDRTA2RkUyNkFE"), 16 /*0x10*/);
      Number number2 = Number.ValueOf(65390L);
      EllipticCurves curve = (EllipticCurves) new Field2MCurves(176 /*0xB0*/, 1, 2, 43, new Number(PdfHexEncoder.DecodeString("MDBFNEU2REIyOTk1MDY1QzQwN0Q5RDM5QjhEMDk2N0I5NjcwNEJBOEU5QzkwQg=="), 16 /*0x10*/), new Number(PdfHexEncoder.DecodeString("MDA1RERBNDcwQUJFNjQxNERFOEVDMTMzQUUyOEU5QkJEN0ZDRUMwQUUwRkZGMg=="), 16 /*0x10*/), number1, number2);
      return new ECX9Field(curve, curve.GetDecodedECPoint(PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("MDM4RDE2QzI4NjY3OThCNjAwRjlGMDhCQjRBOEU4NjBGMzI5OENFMDRBNTc5OA=="))), number1, number2, (byte[]) null);
    }
  }

  internal class ECPrimeT191V1 : ECX9Params
  {
    internal static readonly ECX9Params primeField = (ECX9Params) new ECX962Curves.ECPrimeT191V1();

    private ECPrimeT191V1()
    {
    }

    protected override ECX9Field DefineParameters()
    {
      Number number1 = new Number(PdfHexEncoder.DecodeString("NDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDRBMjBFOTBDMzkwNjdDODkzQkJCOUE1"), 16 /*0x10*/);
      Number number2 = Number.ValueOf(2L);
      EllipticCurves curve = (EllipticCurves) new Field2MCurves(191, 9, new Number(PdfHexEncoder.DecodeString("Mjg2NjUzN0I2NzY3NTI2MzZBNjhGNTY1NTRFMTI2NDAyNzZCNjQ5RUY3NTI2MjY3"), 16 /*0x10*/), new Number(PdfHexEncoder.DecodeString("MkU0NUVGNTcxRjAwNzg2RjY3QjAwODFCOTQ5NUEzRDk1NDYyRjVERTBBQTE4NUVD"), 16 /*0x10*/), number1, number2);
      return new ECX9Field(curve, curve.GetDecodedECPoint(PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("MDIzNkIzREFGOEEyMzIwNkY5QzRGMjk5RDdCMjFBOUMzNjkxMzdGMkM4NEFFMUFBMEQ="))), number1, number2, PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("NEUxM0NBNTQyNzQ0RDY5NkU2NzY4NzU2MTUxNzU1MkYyNzlBOEM4NA==")));
    }
  }

  internal class ECPrimeT191V2 : ECX9Params
  {
    internal static readonly ECX9Params primeField = (ECX9Params) new ECX962Curves.ECPrimeT191V2();

    private ECPrimeT191V2()
    {
    }

    protected override ECX9Field DefineParameters()
    {
      Number number1 = new Number(PdfHexEncoder.DecodeString("MjAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwNTA1MDhDQjg5RjY1MjgyNEUwNkI4MTcz"), 16 /*0x10*/);
      Number number2 = Number.ValueOf(4L);
      EllipticCurves curve = (EllipticCurves) new Field2MCurves(191, 9, new Number(PdfHexEncoder.DecodeString("NDAxMDI4Nzc0RDc3NzdDN0I3NjY2RDEzNjZFQTQzMjA3MTI3NEY4OUZGMDFFNzE4"), 16 /*0x10*/), new Number(PdfHexEncoder.DecodeString("MDYyMDA0OEQyOEJDQkQwM0I2MjQ5Qzk5MTgyQjdDOENEMTk3MDBDMzYyQzQ2QTAx"), 16 /*0x10*/), number1, number2);
      return new ECX9Field(curve, curve.GetDecodedECPoint(PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("MDIzODA5QjJCN0NDMUIyOENDNUE4NzkyNkFBRDgzRkQyODc4OUU4MUUyQzlFM0JGMTA="))), number1, number2, (byte[]) null);
    }
  }

  internal class ECPrimeT191W1 : ECX9Params
  {
    internal static readonly ECX9Params primeField = (ECX9Params) new ECX962Curves.ECPrimeT191W1();

    private ECPrimeT191W1()
    {
    }

    protected override ECX9Field DefineParameters()
    {
      Number number1 = new Number(PdfHexEncoder.DecodeString("MTU1NTU1NTU1NTU1NTU1NTU1NTU1NTU1NjEwQzBCMTk2ODEyQkZCNjI4OEEzRUEz"), 16 /*0x10*/);
      Number number2 = Number.ValueOf(6L);
      EllipticCurves curve = (EllipticCurves) new Field2MCurves(191, 9, new Number(PdfHexEncoder.DecodeString("NkMwMTA3NDc1NjA5OTEyMjIyMTA1NjkxMUM3N0Q3N0U3N0E3NzdFN0U3RTc3RkNC"), 16 /*0x10*/), new Number(PdfHexEncoder.DecodeString("NzFGRTFBRjkyNkNGODQ3OTg5RUZFRjhEQjQ1OUY2NjM5NEQ5MEYzMkFEM0YxNUU4"), 16 /*0x10*/), number1, number2);
      return new ECX9Field(curve, curve.GetDecodedECPoint(PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("MDMzNzVENENFMjRGREU0MzQ0ODlERTg3NDZFNzE3ODYwMTUwMDlFNjZFMzhBOTI2REQ="))), number1, number2, (byte[]) null);
    }
  }

  internal class ECPrimeT208W1 : ECX9Params
  {
    internal static readonly ECX9Params primeField = (ECX9Params) new ECX962Curves.ECPrimeT208W1();

    private ECPrimeT208W1()
    {
    }

    protected override ECX9Field DefineParameters()
    {
      Number number1 = new Number(PdfHexEncoder.DecodeString("MDEwMUJBRjk1Qzk3MjNDNTdCNkMyMURBMkVGRjJENUVENTg4QkRENTcxN0UyMTJGOUQ="), 16 /*0x10*/);
      Number number2 = Number.ValueOf(65096L);
      EllipticCurves curve = (EllipticCurves) new Field2MCurves(208 /*0xD0*/, 1, 2, 83, new Number("0", 16 /*0x10*/), new Number(PdfHexEncoder.DecodeString("MDBDODYxOUVENDVBNjJFNjIxMkUxMTYwMzQ5RTJCRkE4NDQ0MzlGQUZDMkEzRkQxNjM4RjlF"), 16 /*0x10*/), number1, number2);
      return new ECX9Field(curve, curve.GetDecodedECPoint(PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("MDI4OUZERkJFNEFCRTE5M0RGOTU1OUVDRjA3QUMwQ0U3ODU1NEUyNzg0RUI4QzFFRDFBNTdB"))), number1, number2, (byte[]) null);
    }
  }

  internal class ECPrimeT239V1 : ECX9Params
  {
    internal static readonly ECX9Params primeField = (ECX9Params) new ECX962Curves.ECPrimeT239V1();

    private ECPrimeT239V1()
    {
    }

    protected override ECX9Field DefineParameters()
    {
      Number number1 = new Number(PdfHexEncoder.DecodeString("MjAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMEY0RDQyRkZFMTQ5MkE0OTkzRjFDQUQ2NjZFNDQ3"), 16 /*0x10*/);
      Number number2 = Number.ValueOf(4L);
      EllipticCurves curve = (EllipticCurves) new Field2MCurves(239, 36, new Number(PdfHexEncoder.DecodeString("MzIwMTA4NTcwNzdDNTQzMTEyM0E0NkI4MDg5MDY3NTZGNTQzNDIzRThEMjc4Nzc1NzgxMjU3NzhBQzc2"), 16 /*0x10*/), new Number(PdfHexEncoder.DecodeString("NzkwNDA4RjJFRURBRjM5MkIwMTJFREVGQjMzOTJGMzBGNDMyN0MwQ0EzRjMxRkMzODNDNDIyQUE4QzE2"), 16 /*0x10*/), number1, number2);
      return new ECX9Field(curve, curve.GetDecodedECPoint(PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("MDI1NzkyNzA5OEZBOTMyRTdDMEE5NkQzRkQ1QjcwNkVGN0U1RjVDMTU2RTE2QjdFN0M4NjAzODU1MkU5MUQ="))), number1, number2, (byte[]) null);
    }
  }

  internal class ECPrimeT239V2 : ECX9Params
  {
    internal static readonly ECX9Params primeField = (ECX9Params) new ECX962Curves.ECPrimeT239V2();

    private ECPrimeT239V2()
    {
    }

    protected override ECX9Field DefineParameters()
    {
      Number number1 = new Number(PdfHexEncoder.DecodeString("MTU1NTU1NTU1NTU1NTU1NTU1NTU1NTU1NTU1NTU1M0M2RjI4ODUyNTlDMzFFM0ZDREYxNTQ2MjQ1MjJE"), 16 /*0x10*/);
      Number number2 = Number.ValueOf(6L);
      EllipticCurves curve = (EllipticCurves) new Field2MCurves(239, 36, new Number(PdfHexEncoder.DecodeString("NDIzMDAxNzc1N0E3NjdGQUU0MjM5ODU2OUI3NDYzMjVENDUzMTNBRjA3NjYyNjY0NzlCNzU2NTRFNjVG"), 16 /*0x10*/), new Number(PdfHexEncoder.DecodeString("NTAzN0VBNjU0MTk2Q0ZGMENEODJCMkMxNEEyRkNGMkUzRkY4Nzc1Mjg1QjU0NTcyMkYwM0VBQ0RCNzRC"), 16 /*0x10*/), number1, number2);
      return new ECX9Field(curve, curve.GetDecodedECPoint(PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("MDIyOEY5RDA0RTkwMDA2OUM4REM0N0EwODUzNEZFNzZEMkI5MDBCN0Q3RUYzMUY1NzA5RjIwMEM0Q0EyMDU="))), number1, number2, (byte[]) null);
    }
  }

  internal class ECPrimeT239 : ECX9Params
  {
    internal static readonly ECX9Params primeField = (ECX9Params) new ECX962Curves.ECPrimeT239();

    private ECPrimeT239()
    {
    }

    protected override ECX9Field DefineParameters()
    {
      Number number1 = new Number(PdfHexEncoder.DecodeString("MENDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQUM0OTEyRDJEOURGOTAzRUY5ODg4QjhBMEU0Q0ZG"), 16 /*0x10*/);
      Number number2 = Number.ValueOf(10L);
      EllipticCurves curve = (EllipticCurves) new Field2MCurves(239, 36, new Number(PdfHexEncoder.DecodeString("MDEyMzg3NzQ2NjZBNjc3NjZENjY3NkY3NzhFNjc2QjY2OTk5MTc2NjY2RTY4NzY2NkQ4NzY2QzY2QTlG"), 16 /*0x10*/), new Number(PdfHexEncoder.DecodeString("NkE5NDE5NzdCQTlGNkE0MzUxOTlBQ0ZDNTEwNjdFRDU4N0Y1MTlDNUVDQjU0MUI4RTQ0MTExREUxRDQw"), 16 /*0x10*/), number1, number2);
      return new ECX9Field(curve, curve.GetDecodedECPoint(PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("MDM3MEY2RTlEMDREMjg5QzRFODk5MTNDRTM1MzBCRkRFOTAzOTc3RDQyQjE0NkQ1MzlCRjFCREU0RTlDOTI="))), number1, number2, (byte[]) null);
    }
  }

  internal class ECPrimeT272 : ECX9Params
  {
    internal static readonly ECX9Params primeField = (ECX9Params) new ECX962Curves.ECPrimeT272();

    private ECPrimeT272()
    {
    }

    protected override ECX9Field DefineParameters()
    {
      Number number1 = new Number(PdfHexEncoder.DecodeString("MDEwMEZBRjUxMzU0RTBFMzlFNDg5MkRGNkUzMTlDNzJDODE2MTYwM0ZBNDVBQTdCOTk4QTE2N0I4RjFFNjI5NTIx"), 16 /*0x10*/);
      Number number2 = Number.ValueOf(65286L);
      EllipticCurves curve = (EllipticCurves) new Field2MCurves(272, 1, 3, 56, new Number(PdfHexEncoder.DecodeString("MDA5MUEwOTFGMDNCNUZCQTRBQjJDQ0Y0OUM0RUREMjIwRkIwMjg3MTJENDJCRTc1MkIyQzQwMDk0REJBQ0RCNTg2RkIyMA=="), 16 /*0x10*/), new Number(PdfHexEncoder.DecodeString("NzE2N0VGQzkyQkIyRTNDRTdDOEFBQUZGMzRFMTJBOUM1NTcwMDNEN0M3M0E2RkFGMDAzRjk5RjZDQzg0ODJFNTQwRjc="), 16 /*0x10*/), number1, number2);
      return new ECX9Field(curve, curve.GetDecodedECPoint(PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("MDI2MTA4QkFCQjJDRUVCQ0Y3ODcwNThBMDU2Q0JFMENGRTYyMkQ3NzIzQTI4OUUwOEEwN0FFMTNFRjBEMTBEMTcxREQ4RA=="))), number1, number2, (byte[]) null);
    }
  }

  internal class ECPrimeT304 : ECX9Params
  {
    internal static readonly ECX9Params primeField = (ECX9Params) new ECX962Curves.ECPrimeT304();

    private ECPrimeT304()
    {
    }

    protected override ECX9Field DefineParameters()
    {
      Number number1 = new Number(PdfHexEncoder.DecodeString("MDEwMUQ1NTY1NzJBQUJBQzgwMDEwMUQ1NTY1NzJBQUJBQzgwMDEwMjJENUM5MUREMTczRjhGQjU2MURBNjg5OTE2NDQ0MzA1MUQ="), 16 /*0x10*/);
      Number number2 = Number.ValueOf(65070L);
      EllipticCurves curve = (EllipticCurves) new Field2MCurves(304, 1, 2, 11, new Number(PdfHexEncoder.DecodeString("MDBGRDBENjkzMTQ5QTExOEY2NTFFNkRDRTY4MDIwODUzNzdFNUY4ODJEMUI1MTBCNDQxNjAwNzRDMTI4ODA3ODM2NUEwMzk2QzhFNjgx"), 16 /*0x10*/), new Number(PdfHexEncoder.DecodeString("MDBCRERCOTdFNTU1QTUwQTkwOEU0M0IwMUM3OThFQTVEQUE2Nzg4RjFFQTI3OTRFRkNGNTcxNjZCOEMxNDAzOTYwMUU1NTgyNzM0MEJF"), 16 /*0x10*/), number1, number2);
      return new ECX9Field(curve, curve.GetDecodedECPoint(PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("MDIxOTdCMDc4NDVFOUJFMkQ5NkFEQjBGNUYzQzdGMkNGRkJEN0EzRUI4QjZGRUMzNUM3RkQ2N0YyNkRERjYyODVBNjQ0Rjc0MEEyNjE0"))), number1, number2, (byte[]) null);
    }
  }

  internal class ECPrimeT359 : ECX9Params
  {
    internal static readonly ECX9Params primeField = (ECX9Params) new ECX962Curves.ECPrimeT359();

    private ECPrimeT359()
    {
    }

    protected override ECX9Field DefineParameters()
    {
      Number number1 = new Number(PdfHexEncoder.DecodeString("MDFBRjI4NkJDQTFBRjI4NkJDQTFBRjI4NkJDQTFBRjI4NkJDQTFBRjI4NkJDOUZCOEY2Qjg1QzU1Njg5MkMyMEE3RUI5NjRGRTc3MTlFNzRGNDkwNzU4RDNC"), 16 /*0x10*/);
      Number number2 = Number.ValueOf(76L);
      EllipticCurves curve = (EllipticCurves) new Field2MCurves(359, 68, new Number(PdfHexEncoder.DecodeString("NTY2NzY3NkE2NTRCMjA3NTRGMzU2RUE5MjAxN0Q5NDY1NjdDNDY2NzU1NTZGMTk1NTZBMDQ2MTZCNTY3RDIyM0E1RTA1NjU2RkI1NDkwMTZBOTY2NTZBNTU3"), 16 /*0x10*/), new Number(PdfHexEncoder.DecodeString("MjQ3MkUyRDAxOTdDNDkzNjNGMUZFN0Y1QjZEQjA3NUQ1MkI2OTQ3RDEzNUQ4Q0E0NDU4MDVEMzlCQzM0NTYyNjA4OTY4Nzc0MkI2MzI5RTcwNjgwMjMxOTg4"), 16 /*0x10*/), number1, number2);
      return new ECX9Field(curve, curve.GetDecodedECPoint(PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("MDMzQzI1OEVGMzA0Nzc2N0U3RURFMEYxRkRBQTc5REFFRTM4NDEzNjZBMTMyRTE2M0FDRUQ0RUQyNDAxREY5QzZCRENERTk4RThFNzA3QzA3QTIyMzlCMUIwOTc="))), number1, number2, (byte[]) null);
    }
  }

  internal class ECPrimeT368 : ECX9Params
  {
    internal static readonly ECX9Params primeField = (ECX9Params) new ECX962Curves.ECPrimeT368();

    private ECPrimeT368()
    {
    }

    protected override ECX9Field DefineParameters()
    {
      Number number1 = new Number(PdfHexEncoder.DecodeString("MDEwMDkwNTEyREE5QUY3MkIwODM0OUQ5OEE1REQ0QzdCMDUzMkVDQTUxQ0UwM0UyRDEwRjNCN0FDNTc5QkQ4N0U5MDlBRTQwQTZGMTMxRTlDRkNFNUJEOTY3"), 16 /*0x10*/);
      Number number2 = Number.ValueOf(65392L);
      EllipticCurves curve = (EllipticCurves) new Field2MCurves(368, 1, 2, 85, new Number(PdfHexEncoder.DecodeString("MDBFMEQyRUUyNTA5NTIwNkY1RTJBNEY5RUQyMjlGMUYyNTZFNzlBMEUyQjQ1NTk3MEQ4RDBEODY1QkQ5NDc3OEM1NzZENjJGMEFCNzUxOUNDRDJBMUE5MDZBRTMwRA=="), 16 /*0x10*/), new Number(PdfHexEncoder.DecodeString("MDBGQzEyMTdENDMyMEE5MDQ1MkM3NjBBNThFRENEMzBDOEREMDY5QjNDMzQ0NTM4MzdBMzRFRDUwQ0I1NDkxN0UxQzIxMTJEODREMTY0RjQ0NEY4Rjc0Nzg2MDQ2QQ=="), 16 /*0x10*/), number1, number2);
      return new ECX9Field(curve, curve.GetDecodedECPoint(PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("MDIxMDg1RTI3NTUzODFEQ0NDRTNDMTU1N0FGQTEwQzJGMEMwQzI4MjU2NDZDNUIzNEEzOTRDQkNGQThCQzE2QjIyRTdFNzg5RTkyN0JFMjE2RjAyRTFGQjEzNkE1Rg=="))), number1, number2, (byte[]) null);
    }
  }

  internal class ECPrimeT431 : ECX9Params
  {
    internal static readonly ECX9Params primeField = (ECX9Params) new ECX962Curves.ECPrimeT431();

    private ECPrimeT431()
    {
    }

    protected override ECX9Field DefineParameters()
    {
      Number number1 = new Number(PdfHexEncoder.DecodeString("MDM0MDM0MDM0MDM0MDM0MDM0MDM0MDM0MDM0MDM0MDM0MDM0MDM0MDM0MDM0MDM0MDM0MDM0MDMyM0MzMTNGQUI1MDU4OTcwM0I1RUM2OEQzNTg3RkVDNjBEMTYxQ0MxNDlDMUFENEE5MQ=="), 16 /*0x10*/);
      Number number2 = Number.ValueOf(10080L);
      EllipticCurves curve = (EllipticCurves) new Field2MCurves(431, 120, new Number(PdfHexEncoder.DecodeString("MUE4MjdFRjAwREQ2RkMwRTIzNENBRjA0NkM2QTVEOEE4NTM5NUIyMzZDQzRBRDJDRjMyQTBDQURCREM5RERGNjIwQjBFQjk5MDZEMDk1N0Y2QzZGRUFDRDYxNTQ2OERGMTA0REUyOTZDRDhG"), 16 /*0x10*/), new Number(PdfHexEncoder.DecodeString("MTBEOUI0QTNEOTA0N0Q4QjE1NDM1OUFCRkIxQjdGNTQ4NUIwNENFQjg2ODIzN0REQzlERURBOTgyQTY3OUE1QTkxOUI2MjZENEU1MEE4REQ3MzFCMTA3QTk5NjIzODFGQjVEODA3QkYyNjE4"), 16 /*0x10*/), number1, number2);
      return new ECX9Field(curve, curve.GetDecodedECPoint(PdfHexEncoder.Decode(PdfHexEncoder.DecodeString("MDIxMjBGQzA1RDNDNjdBOTlERTE2MUQyRjQwOTI2MjJGRUNBNzAxQkU0RjUwRjQ3NTg3MTRFOEE4N0JCRjJBNjU4RUY4QzIxRTdDNUVGRTk2NTM2MUY2QzI5OTlDMEMyNDdCMERCRDcwQ0U2Qjc="))), number1, number2, (byte[]) null);
    }
  }
}
