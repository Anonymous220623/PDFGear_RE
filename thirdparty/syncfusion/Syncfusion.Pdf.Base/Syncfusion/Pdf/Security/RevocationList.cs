// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.RevocationList
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class RevocationList
{
  private IList<string> m_urls = (IList<string>) new List<string>();
  private CertificateUtililty m_utility = new CertificateUtililty();

  internal RevocationList(ICollection<X509Certificate> chain)
  {
    foreach (X509Certificate certificate in (IEnumerable<X509Certificate>) chain)
      this.Initialize(certificate);
  }

  internal RevocationList(X509Certificate certificate) => this.Initialize(certificate);

  private void Initialize(X509Certificate certificate)
  {
    try
    {
      string crlUrl = this.m_utility.GetCrlUrl(certificate);
      if (crlUrl == null)
        return;
      this.AddUrl(crlUrl);
    }
    catch
    {
      throw new Exception("Invalid CRL URL");
    }
  }

  protected virtual void AddUrl(string url)
  {
    if (this.m_urls.Contains(url))
      return;
    this.m_urls.Add(url);
  }

  internal ICollection<byte[]> GetEncoded(X509Certificate certificate, string url)
  {
    if (certificate == null)
      return (ICollection<byte[]>) null;
    List<string> stringList = new List<string>((IEnumerable<string>) this.m_urls);
    if (stringList.Count == 0)
    {
      try
      {
        if (url == null)
          url = this.m_utility.GetCrlUrl(certificate);
        if (url == null)
          throw new ArgumentNullException();
        stringList.Add(url);
      }
      catch (Exception ex)
      {
      }
    }
    List<byte[]> encoded = new List<byte[]>();
    foreach (string requestUriString in stringList)
    {
      try
      {
        Stream responseStream = WebRequest.Create(requestUriString).GetResponse().GetResponseStream();
        byte[] buffer = new byte[1024 /*0x0400*/];
        MemoryStream memoryStream = new MemoryStream();
        while (true)
        {
          int count = responseStream.Read(buffer, 0, buffer.Length);
          if (count > 0)
            memoryStream.Write(buffer, 0, count);
          else
            break;
        }
        try
        {
          responseStream.Close();
        }
        catch
        {
        }
        encoded.Add(memoryStream.ToArray());
      }
      catch (Exception ex)
      {
      }
    }
    return (ICollection<byte[]>) encoded;
  }
}
