// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.OcspConstants
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal abstract class OcspConstants
{
  internal const string OcspId = "1.3.6.1.5.5.7.48.1";
  public static readonly DerObjectID Ocsp = new DerObjectID("1.3.6.1.5.5.7.48.1");
  public static readonly DerObjectID OcspBasic = new DerObjectID("1.3.6.1.5.5.7.48.1.1");
  public static readonly DerObjectID OcspNonce = new DerObjectID(OcspConstants.Ocsp.ToString() + ".2");
  public static readonly DerObjectID OcspCrl = new DerObjectID(OcspConstants.Ocsp.ToString() + ".3");
  public static readonly DerObjectID OcspResponse = new DerObjectID(OcspConstants.Ocsp.ToString() + ".4");
  public static readonly DerObjectID OcspNocheck = new DerObjectID(OcspConstants.Ocsp.ToString() + ".5");
  public static readonly DerObjectID OcspArchiveCutoff = new DerObjectID(OcspConstants.Ocsp.ToString() + ".6");
  public static readonly DerObjectID OcspServiceLocator = new DerObjectID(OcspConstants.Ocsp.ToString() + ".7");
}
