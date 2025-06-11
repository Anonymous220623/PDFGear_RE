// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.HTMLImportSettings
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class HTMLImportSettings
{
  private byte m_bFlags;

  public bool IsConsiderListStyleAttribute
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal bool IsEventSubscribed => this.ImageNodeVisited != null;

  public event ImageDownloadingFailedEventHandler ImageDownloadingFailed;

  internal ImageDownloadingFailedEventArgs ExecuteImageDownloadingFailedEvent(string URI)
  {
    ImageDownloadingFailedEventArgs args = new ImageDownloadingFailedEventArgs(URI);
    if (this.ImageDownloadingFailed != null)
      this.ImageDownloadingFailed((object) this, args);
    return args;
  }

  public event ImageNodeVisitedEventHandler ImageNodeVisited;

  internal ImageNodeVisitedEventArgs ExecuteImageNodeVisitedEvent(Stream imageStream, string uri)
  {
    ImageNodeVisitedEventArgs args = new ImageNodeVisitedEventArgs(imageStream, uri);
    if (this.ImageNodeVisited != null)
      this.ImageNodeVisited((object) this, args);
    return args;
  }
}
