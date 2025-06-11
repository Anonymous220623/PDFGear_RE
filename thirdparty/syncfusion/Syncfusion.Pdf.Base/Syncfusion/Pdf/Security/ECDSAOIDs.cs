// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.ECDSAOIDs
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal abstract class ECDSAOIDs
{
  internal const string IDx962 = "1.2.840.10045";
  public static readonly DerObjectID X90IDx962_1 = new DerObjectID("1.2.840.10045");
  public static readonly DerObjectID X90FieldID = ECDSAOIDs.X90IDx962_1.Branch("1");
  public static readonly DerObjectID X90UniqueID = ECDSAOIDs.X90FieldID.Branch("1");
  public static readonly DerObjectID X90RecordID = ECDSAOIDs.X90FieldID.Branch("2");
  public static readonly DerObjectID X90TNObjID = ECDSAOIDs.X90RecordID.Branch("3.2");
  public static readonly DerObjectID X90PPObjID = ECDSAOIDs.X90RecordID.Branch("3.3");
  public static readonly DerObjectID X90SignType = ECDSAOIDs.X90IDx962_1.Branch("4");
  public static readonly DerObjectID ECDSAwithSHA1 = ECDSAOIDs.X90SignType.Branch("1");
  public static readonly DerObjectID X90KeyType = ECDSAOIDs.X90IDx962_1.Branch("2");
  public static readonly DerObjectID IdECPublicKey = ECDSAOIDs.X90KeyType.Branch("1");
  public static readonly DerObjectID ECDSAwithSHA2 = ECDSAOIDs.X90SignType.Branch("3");
  public static readonly DerObjectID ECDSAwithSHA224 = ECDSAOIDs.ECDSAwithSHA2.Branch("1");
  public static readonly DerObjectID ECDSAwithSHA256 = ECDSAOIDs.ECDSAwithSHA2.Branch("2");
  public static readonly DerObjectID ECDSAwithSHA384 = ECDSAOIDs.ECDSAwithSHA2.Branch("3");
  public static readonly DerObjectID ECDSAwithSHA512 = ECDSAOIDs.ECDSAwithSHA2.Branch("4");
  public static readonly DerObjectID EllipticCurve = ECDSAOIDs.X90IDx962_1.Branch("3");
  public static readonly DerObjectID Curves = ECDSAOIDs.EllipticCurve.Branch("0");
  public static readonly DerObjectID ECP163v1 = ECDSAOIDs.Curves.Branch("1");
  public static readonly DerObjectID ECP163v2 = ECDSAOIDs.Curves.Branch("2");
  public static readonly DerObjectID ECP163v3 = ECDSAOIDs.Curves.Branch("3");
  public static readonly DerObjectID ECP176w1 = ECDSAOIDs.Curves.Branch("4");
  public static readonly DerObjectID ECP191v1 = ECDSAOIDs.Curves.Branch("5");
  public static readonly DerObjectID ECP191v2 = ECDSAOIDs.Curves.Branch("6");
  public static readonly DerObjectID ECP191v3 = ECDSAOIDs.Curves.Branch("7");
  public static readonly DerObjectID ECP208w1 = ECDSAOIDs.Curves.Branch("10");
  public static readonly DerObjectID ECP239v1 = ECDSAOIDs.Curves.Branch("11");
  public static readonly DerObjectID ECP239v2 = ECDSAOIDs.Curves.Branch("12");
  public static readonly DerObjectID ECP239v3 = ECDSAOIDs.Curves.Branch("13");
  public static readonly DerObjectID ECP272w1 = ECDSAOIDs.Curves.Branch("16");
  public static readonly DerObjectID ECP304w1 = ECDSAOIDs.Curves.Branch("17");
  public static readonly DerObjectID ECP359v1 = ECDSAOIDs.Curves.Branch("18");
  public static readonly DerObjectID ECP368w1 = ECDSAOIDs.Curves.Branch("19");
  public static readonly DerObjectID ECP431r1 = ECDSAOIDs.Curves.Branch("20");
  public static readonly DerObjectID ECPC = ECDSAOIDs.EllipticCurve.Branch("1");
  public static readonly DerObjectID ECPC192v1 = ECDSAOIDs.ECPC.Branch("1");
  public static readonly DerObjectID ECPC192v2 = ECDSAOIDs.ECPC.Branch("2");
  public static readonly DerObjectID ECPC192v3 = ECDSAOIDs.ECPC.Branch("3");
  public static readonly DerObjectID ECPC239v1 = ECDSAOIDs.ECPC.Branch("4");
  public static readonly DerObjectID ECPC239v2 = ECDSAOIDs.ECPC.Branch("5");
  public static readonly DerObjectID ECPC239v3 = ECDSAOIDs.ECPC.Branch("6");
  public static readonly DerObjectID ECPC256v1 = ECDSAOIDs.ECPC.Branch("7");
}
