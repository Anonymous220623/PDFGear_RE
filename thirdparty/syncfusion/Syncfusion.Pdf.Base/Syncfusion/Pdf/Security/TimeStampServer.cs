// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.TimeStampServer
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;
using System.Net;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Security;

public class TimeStampServer
{
  private Uri m_server;
  private string m_username;
  private string m_password;
  private int m_timeOut;

  public Uri Server
  {
    get => this.m_server;
    set
    {
      this.m_server = !(value == (Uri) null) ? value : throw new ArgumentNullException(nameof (Server));
    }
  }

  public string UserName
  {
    get => this.m_username;
    set => this.m_username = value;
  }

  public string Password
  {
    get => this.m_password;
    set => this.m_password = value;
  }

  public int TimeOut
  {
    get => this.m_timeOut;
    set => this.m_timeOut = value;
  }

  public bool IsValid => this.IsValidTimeStamp();

  public TimeStampServer(Uri server)
  {
    this.m_server = !(server == (Uri) null) ? server : throw new ArgumentNullException("Sever");
  }

  public TimeStampServer(Uri server, string username, string password)
    : this(server)
  {
    this.m_username = username;
    this.m_password = password;
  }

  public TimeStampServer(Uri server, string username, string password, int timeOut)
    : this(server, username, password)
  {
    this.m_timeOut = timeOut;
  }

  internal bool IsValidTimeStamp()
  {
    try
    {
      this.GetTimeStampResponse(new TimeStampRequestCreator(true).GetAsnEncodedTimestampRequest(new MessageDigestAlgorithms().Digest((Stream) new MemoryStream(Encoding.ASCII.GetBytes("Test data")), "SHA256")));
      return true;
    }
    catch
    {
      return false;
    }
  }

  internal byte[] GetTimeStampResponse(byte[] request)
  {
    HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(this.m_server.ToString());
    httpWebRequest.UserAgent = "syncfusion";
    httpWebRequest.ProtocolVersion = HttpVersion.Version10;
    httpWebRequest.ContentLength = (long) request.Length;
    httpWebRequest.ContentType = "application/timestamp-query";
    httpWebRequest.Method = "POST";
    if (!string.IsNullOrEmpty(this.m_username))
    {
      string base64String = Convert.ToBase64String(Encoding.Default.GetBytes($"{this.m_username}:{this.m_password}"));
      httpWebRequest.Headers["Authorization"] = "Basic " + base64String;
    }
    try
    {
      Stream requestStream = httpWebRequest.GetRequestStream();
      requestStream.Write(request, 0, request.Length);
      requestStream.Close();
    }
    catch (Exception ex)
    {
      throw new Exception("The remote name could not be resolved: " + this.m_server.ToString());
    }
    HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse();
    Stream stream = response.StatusCode == HttpStatusCode.OK ? response.GetResponseStream() : throw new Exception("Server returned unexpected response code : " + response.StatusCode.ToString());
    MemoryStream memoryStream = new MemoryStream();
    byte[] buffer = new byte[1024 /*0x0400*/];
    int count;
    while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
      memoryStream.Write(buffer, 0, count);
    byte[] bytes = memoryStream.ToArray();
    string contentEncoding = response.ContentEncoding;
    if (!string.IsNullOrEmpty(contentEncoding) && contentEncoding.Equals("base64", StringComparison.InvariantCultureIgnoreCase))
      bytes = Convert.FromBase64String(Encoding.ASCII.GetString(bytes));
    stream.Close();
    response.Close();
    return bytes;
  }
}
