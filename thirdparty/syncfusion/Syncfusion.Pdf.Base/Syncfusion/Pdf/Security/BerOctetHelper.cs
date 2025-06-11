// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.BerOctetHelper
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class BerOctetHelper : IAsn1Octet, IAsn1
{
  private Asn1Parser m_helper;

  internal BerOctetHelper(Asn1Parser helper) => this.m_helper = helper;

  public Stream GetOctetStream() => (Stream) new OctetStream(this.m_helper);

  public Asn1 GetAsn1()
  {
    try
    {
      return (Asn1) new BerOctet(this.ReadAll(this.GetOctetStream()));
    }
    catch (IOException ex)
    {
      throw new Exception(ex.ToString());
    }
  }

  internal byte[] ReadAll(Stream stream)
  {
    MemoryStream output = new MemoryStream();
    this.PipeAll(stream, (Stream) output);
    return output.ToArray();
  }

  private void PipeAll(Stream input, Stream output)
  {
    byte[] buffer = new byte[512 /*0x0200*/];
    int count;
    while ((count = input.Read(buffer, 0, buffer.Length)) > 0)
      output.Write(buffer, 0, count);
  }
}
