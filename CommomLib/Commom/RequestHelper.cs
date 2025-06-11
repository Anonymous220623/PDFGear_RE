// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.RequestHelper
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System.IO;
using System.Net;
using System.Text;

#nullable disable
namespace CommomLib.Commom;

public class RequestHelper
{
  public const string baseURL = "https://fds.pdfgear.com/1/";

  public static HttpWebRequest CreateRequest(string relativeURL, string boundary)
  {
    HttpWebRequest request = (HttpWebRequest) WebRequest.Create("https://fds.pdfgear.com/1/" + relativeURL);
    request.Headers.Clear();
    request.ContentType = "multipart/form-data; boundary=" + boundary;
    request.Method = "POST";
    request.KeepAlive = true;
    return request;
  }

  public static void WriteCRLF(Stream o)
  {
    byte[] bytes = Encoding.UTF8.GetBytes("\r\n");
    o.Write(bytes, 0, bytes.Length);
  }

  public static void WriteBoundaryBytes(Stream o, string b, bool isFinalBoundary)
  {
    byte[] bytes = Encoding.UTF8.GetBytes(isFinalBoundary ? $"--{b}--" : $"--{b}\r\n");
    o.Write(bytes, 0, bytes.Length);
  }

  public static void WriteContentDispositionFormDataHeader(Stream o, string name)
  {
    byte[] bytes = Encoding.UTF8.GetBytes($"Content-Disposition: form-data; name=\"{name}\"\r\n\r\n");
    o.Write(bytes, 0, bytes.Length);
  }

  public static void WriteContentDispositionFileHeader(
    Stream o,
    string name,
    string fileName,
    string contentType)
  {
    byte[] bytes = Encoding.UTF8.GetBytes($"{$"Content-Disposition: form-data; name=\"{name}\"; filename=\"{fileName}\"\r\n"}Content-Type: {contentType}\r\n\r\n");
    o.Write(bytes, 0, bytes.Length);
  }

  public static void WriteString(Stream o, string data)
  {
    byte[] bytes = Encoding.UTF8.GetBytes(data);
    o.Write(bytes, 0, bytes.Length);
  }
}
