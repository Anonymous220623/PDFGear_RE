// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.OcspResponseHelper
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class OcspResponseHelper
{
  private OcspResponse m_response;

  public OcspResponseHelper(Stream stream)
    : this(new Asn1Stream(stream))
  {
  }

  private OcspResponseHelper(Asn1Stream stream)
  {
    try
    {
      this.m_response = new OcspResponse().GetOcspResponse((object) stream.ReadAsn1());
    }
    catch (Exception ex)
    {
      throw new IOException("Invalid response");
    }
  }

  internal int Status => this.m_response.ResponseStatus.Value.IntValue;

  internal object GetResponseObject()
  {
    RevocationResponseBytes responseBytes = this.m_response.ResponseBytes;
    if (responseBytes == null)
      return (object) null;
    if (!responseBytes.ResponseType.Equals((object) OcspConstants.OcspBasic))
      return (object) responseBytes.Response;
    try
    {
      return (object) new X509RevocationResponse(new OcspHelper().GetOcspStructure((object) Asn1.FromByteArray(responseBytes.Response.GetOctets())));
    }
    catch (Exception ex)
    {
      throw new Exception("Invalid response detected");
    }
  }
}
