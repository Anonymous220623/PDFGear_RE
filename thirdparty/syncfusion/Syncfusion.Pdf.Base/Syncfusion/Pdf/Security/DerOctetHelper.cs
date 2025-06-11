// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.DerOctetHelper
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class DerOctetHelper : IAsn1Octet, IAsn1
{
  private readonly Asn1StreamHelper m_stream;

  internal DerOctetHelper(Asn1StreamHelper stream) => this.m_stream = stream;

  public Stream GetOctetStream() => (Stream) this.m_stream;

  public Asn1 GetAsn1()
  {
    try
    {
      return (Asn1) new DerOctet(this.m_stream.ToArray());
    }
    catch (Exception ex)
    {
      throw new Exception(ex.Message);
    }
  }
}
