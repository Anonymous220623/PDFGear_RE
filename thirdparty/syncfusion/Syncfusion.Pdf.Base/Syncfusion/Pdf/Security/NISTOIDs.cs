// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.NISTOIDs
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal sealed class NISTOIDs
{
  internal static readonly DerObjectID NistAlgorithm = new DerObjectID("2.16.840.1.101.3.4");
  internal static readonly DerObjectID HashAlgs = NISTOIDs.NistAlgorithm.Branch("2");
  internal static readonly DerObjectID SHA256 = NISTOIDs.HashAlgs.Branch("1");
  internal static readonly DerObjectID SHA384 = NISTOIDs.HashAlgs.Branch("2");
  internal static readonly DerObjectID SHA512 = NISTOIDs.HashAlgs.Branch("3");
  internal static readonly DerObjectID DSAWithSHA2 = new DerObjectID(NISTOIDs.NistAlgorithm.ToString() + ".3");
  internal static readonly DerObjectID DSAWithSHA256 = new DerObjectID(NISTOIDs.DSAWithSHA2.ToString() + ".2");
  internal static readonly DerObjectID DSAWithSHA384 = new DerObjectID(NISTOIDs.DSAWithSHA2.ToString() + ".3");
  internal static readonly DerObjectID DSAWithSHA512 = new DerObjectID(NISTOIDs.DSAWithSHA2.ToString() + ".4");
  internal static readonly DerObjectID TTTAlgorithm = new DerObjectID("1.3.36.3");
  internal static readonly DerObjectID RipeMD160 = new DerObjectID(NISTOIDs.TTTAlgorithm.ToString() + ".2.1");
  internal static readonly DerObjectID TTTRsaSignatureAlgorithm = new DerObjectID(NISTOIDs.TTTAlgorithm.ToString() + ".3.1");
  internal static readonly DerObjectID RsaSignatureWithRipeMD160 = new DerObjectID(NISTOIDs.TTTRsaSignatureAlgorithm.ToString() + ".2");
}
