// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.HttpHelperDownloadResponse
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

#nullable disable
namespace CommomLib.Commom;

public class HttpHelperDownloadResponse
{
  private double? progress;

  internal HttpHelperDownloadResponse(long? contentLength, long position, double? progress)
  {
    this.ContentLength = contentLength;
    this.Position = position;
    this.progress = progress;
  }

  public long? ContentLength { get; }

  public long Position { get; }

  public double Progress
  {
    get
    {
      if (this.progress.HasValue)
        return this.progress.Value;
      long? contentLength = this.ContentLength;
      if (!contentLength.HasValue)
        return 0.0;
      double num1 = 1.0 * (double) this.Position;
      contentLength = this.ContentLength;
      double num2 = (double) contentLength.Value;
      return num1 / num2;
    }
  }
}
