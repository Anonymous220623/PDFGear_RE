// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ImageNodeVisitedEventArgs
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class ImageNodeVisitedEventArgs : EventArgs
{
  private Stream m_imageStream;
  private string m_uri = string.Empty;

  public Stream ImageStream
  {
    get => this.m_imageStream;
    set => this.m_imageStream = value;
  }

  public string Uri
  {
    get => this.m_uri;
    set => this.m_uri = value;
  }

  internal ImageNodeVisitedEventArgs(Stream imageStream, string uri)
  {
    this.m_uri = uri;
    this.m_imageStream = imageStream;
  }
}
