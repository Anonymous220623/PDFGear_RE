// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.UrlEncodeLayoutRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers;

[LayoutRenderer("url-encode")]
[AppDomainFixedOutput]
[ThreadAgnostic]
[ThreadSafe]
public sealed class UrlEncodeLayoutRendererWrapper : WrapperLayoutRendererBase
{
  public UrlEncodeLayoutRendererWrapper() => this.SpaceAsPlus = true;

  public bool SpaceAsPlus { get; set; }

  public bool EscapeDataRfc3986 { get; set; }

  public bool EscapeDataNLogLegacy { get; set; }

  protected override string Transform(string text)
  {
    if (string.IsNullOrEmpty(text))
      return string.Empty;
    UrlHelper.EscapeEncodingOptions stringEncodingFlags = UrlHelper.GetUriStringEncodingFlags(this.EscapeDataNLogLegacy, this.SpaceAsPlus, this.EscapeDataRfc3986);
    StringBuilder target = new StringBuilder(text.Length + 20);
    UrlHelper.EscapeDataEncode(text, target, stringEncodingFlags);
    return target.ToString();
  }
}
