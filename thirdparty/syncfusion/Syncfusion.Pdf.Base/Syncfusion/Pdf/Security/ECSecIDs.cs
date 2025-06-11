// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.ECSecIDs
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal abstract class ECSecIDs
{
  public static readonly DerObjectID EllipticCurve = new DerObjectID("1.3.132.0");
  public static readonly DerObjectID ECSECG163k1 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".1");
  public static readonly DerObjectID ECSECG163r1 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".2");
  public static readonly DerObjectID ECSECG239k1 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".3");
  public static readonly DerObjectID ECSECG113r1 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".4");
  public static readonly DerObjectID ECSECG113r2 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".5");
  public static readonly DerObjectID ECSECP112r1 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".6");
  public static readonly DerObjectID ECSECP112r2 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".7");
  public static readonly DerObjectID ECSECP160r1 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".8");
  public static readonly DerObjectID ECSECP160k1 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".9");
  public static readonly DerObjectID ECSECP256k1 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".10");
  public static readonly DerObjectID ECSECG163r2 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".15");
  public static readonly DerObjectID ECSECG283k1 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".16");
  public static readonly DerObjectID ECSECG283r1 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".17");
  public static readonly DerObjectID ECSECG131r1 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".22");
  public static readonly DerObjectID ECSECG131r2 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".23");
  public static readonly DerObjectID ECSECG193r1 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".24");
  public static readonly DerObjectID ECSECG193r2 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".25");
  public static readonly DerObjectID ECSECG233k1 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".26");
  public static readonly DerObjectID ECSECG233r1 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".27");
  public static readonly DerObjectID ECSECP128r1 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".28");
  public static readonly DerObjectID ECSECP128r2 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".29");
  public static readonly DerObjectID ECSECP160r2 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".30");
  public static readonly DerObjectID ECSECP192k1 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".31");
  public static readonly DerObjectID ECSECP224k1 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".32");
  public static readonly DerObjectID ECSECP224r1 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".33");
  public static readonly DerObjectID ECSECP384r1 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".34");
  public static readonly DerObjectID ECSECP521r1 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".35");
  public static readonly DerObjectID ECSECG409k1 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".36");
  public static readonly DerObjectID ECSECG409r1 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".37");
  public static readonly DerObjectID ECSECG571k1 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".38");
  public static readonly DerObjectID ECSECG571r1 = new DerObjectID(ECSecIDs.EllipticCurve.ToString() + ".39");
  public static readonly DerObjectID ECSECP192r1 = ECDSAOIDs.ECPC192v1;
  public static readonly DerObjectID ECSECP256r1 = ECDSAOIDs.ECPC256v1;
}
