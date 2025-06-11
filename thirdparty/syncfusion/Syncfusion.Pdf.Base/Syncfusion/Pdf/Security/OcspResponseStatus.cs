// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.OcspResponseStatus
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class OcspResponseStatus : DerCatalogue
{
  internal const int Successful = 0;
  internal const int MalformedRequest = 1;
  internal const int InternalError = 2;
  internal const int TryLater = 3;
  internal const int SignatureRequired = 5;
  internal const int Unauthorized = 6;

  internal OcspResponseStatus(DerCatalogue value)
    : base(value.Value.IntValue)
  {
  }
}
