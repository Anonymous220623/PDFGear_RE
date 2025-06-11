// Decompiled with JetBrains decompiler
// Type: NLog.Internal.NetworkSenders.WebRequestFactory
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Net;

#nullable disable
namespace NLog.Internal.NetworkSenders;

internal class WebRequestFactory : IWebRequestFactory
{
  public static IWebRequestFactory Instance { get; } = (IWebRequestFactory) new WebRequestFactory();

  public WebRequest CreateWebRequest(Uri address) => WebRequest.Create(address);
}
