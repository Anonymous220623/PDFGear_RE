// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.XmlEncodeLayoutRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers;

[LayoutRenderer("xml-encode")]
[AmbientProperty("XmlEncode")]
[AppDomainFixedOutput]
[ThreadAgnostic]
[ThreadSafe]
public sealed class XmlEncodeLayoutRendererWrapper : WrapperLayoutRendererBase
{
  public XmlEncodeLayoutRendererWrapper() => this.XmlEncode = true;

  [DefaultValue(true)]
  public bool XmlEncode { get; set; }

  [DefaultValue(false)]
  public bool XmlEncodeNewlines { get; set; }

  protected override void RenderInnerAndTransform(
    LogEventInfo logEvent,
    StringBuilder builder,
    int orgLength)
  {
    this.Inner.RenderAppendBuilder(logEvent, builder);
    if (!this.XmlEncode || !this.RequiresXmlEncode(builder, orgLength))
      return;
    string text = builder.ToString(orgLength, builder.Length - orgLength);
    builder.Length = orgLength;
    int num = this.XmlEncodeNewlines ? 1 : 0;
    StringBuilder result = builder;
    XmlHelper.EscapeXmlString(text, num != 0, result);
  }

  protected override string Transform(string text)
  {
    return this.XmlEncode ? XmlHelper.EscapeXmlString(text, this.XmlEncodeNewlines) : text;
  }

  private bool RequiresXmlEncode(StringBuilder target, int startPos = 0)
  {
    for (int index = startPos; index < target.Length; ++index)
    {
      switch (target[index])
      {
        case '\n':
        case '\r':
          if (this.XmlEncodeNewlines)
            return true;
          break;
        case '"':
        case '&':
        case '\'':
        case '<':
        case '>':
          return true;
      }
    }
    return false;
  }
}
