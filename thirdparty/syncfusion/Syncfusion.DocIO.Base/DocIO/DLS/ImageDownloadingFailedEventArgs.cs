// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ImageDownloadingFailedEventArgs
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class ImageDownloadingFailedEventArgs : EventArgs
{
  private string m_URI;
  private string m_UserName;
  private string m_Password;

  public string URI
  {
    get => this.m_URI;
    internal set => this.m_URI = value;
  }

  public string UserName
  {
    get => this.m_UserName;
    set => this.m_UserName = value;
  }

  public string Password
  {
    get => this.m_Password;
    set => this.m_Password = value;
  }

  internal ImageDownloadingFailedEventArgs(string URI) => this.m_URI = URI;
}
