// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.ECNamedCurves
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal sealed class ECNamedCurves
{
  internal static readonly IDictionary curvesObjIds = (IDictionary) new Hashtable();
  internal static readonly IDictionary curveNames = (IDictionary) new Hashtable();

  private ECNamedCurves()
  {
  }

  private static void CreateNamedCurves(string name, DerObjectID oid)
  {
    ECNamedCurves.curvesObjIds.Add((object) name, (object) oid);
    ECNamedCurves.curveNames.Add((object) oid, (object) name);
  }

  static ECNamedCurves()
  {
    ECNamedCurves.CreateNamedCurves("B-571", ECSecIDs.ECSECG571r1);
    ECNamedCurves.CreateNamedCurves("B-409", ECSecIDs.ECSECG409r1);
    ECNamedCurves.CreateNamedCurves("B-283", ECSecIDs.ECSECG283r1);
    ECNamedCurves.CreateNamedCurves("B-233", ECSecIDs.ECSECG233r1);
    ECNamedCurves.CreateNamedCurves("B-163", ECSecIDs.ECSECG163r2);
    ECNamedCurves.CreateNamedCurves("K-571", ECSecIDs.ECSECG571k1);
    ECNamedCurves.CreateNamedCurves("K-409", ECSecIDs.ECSECG409k1);
    ECNamedCurves.CreateNamedCurves("K-283", ECSecIDs.ECSECG283k1);
    ECNamedCurves.CreateNamedCurves("K-233", ECSecIDs.ECSECG233k1);
    ECNamedCurves.CreateNamedCurves("K-163", ECSecIDs.ECSECG163k1);
    ECNamedCurves.CreateNamedCurves("P-521", ECSecIDs.ECSECP521r1);
    ECNamedCurves.CreateNamedCurves("P-384", ECSecIDs.ECSECP384r1);
    ECNamedCurves.CreateNamedCurves("P-256", ECSecIDs.ECSECP256r1);
    ECNamedCurves.CreateNamedCurves("P-224", ECSecIDs.ECSECP224r1);
    ECNamedCurves.CreateNamedCurves("P-192", ECSecIDs.ECSECP192r1);
  }

  public static ECX9Field GetByOid(DerObjectID oid) => SECGCurves.GetByOid(oid);
}
