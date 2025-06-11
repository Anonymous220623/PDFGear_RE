// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.Rot13LayoutRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Layouts;
using System;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers;

[LayoutRenderer("rot13")]
[AppDomainFixedOutput]
[ThreadAgnostic]
[ThreadSafe]
public sealed class Rot13LayoutRendererWrapper : WrapperLayoutRendererBuilderBase
{
  public Layout Text
  {
    get => this.Inner;
    set => this.Inner = value;
  }

  public static string DecodeRot13(string encodedValue)
  {
    StringBuilder encodedValue1 = new StringBuilder(encodedValue.Length);
    encodedValue1.Append(encodedValue);
    Rot13LayoutRendererWrapper.DecodeRot13(encodedValue1, 0);
    return encodedValue1.ToString();
  }

  protected override void RenderInnerAndTransform(
    LogEventInfo logEvent,
    StringBuilder builder,
    int orgLength)
  {
    this.Inner.RenderAppendBuilder(logEvent, builder);
    if (builder.Length <= orgLength)
      return;
    Rot13LayoutRendererWrapper.DecodeRot13(builder, orgLength);
  }

  [Obsolete("Inherit from WrapperLayoutRendererBase and override RenderInnerAndTransform() instead. Marked obsolete in NLog 4.6")]
  protected override void TransformFormattedMesssage(StringBuilder target)
  {
  }

  internal static void DecodeRot13(StringBuilder encodedValue, int startPos)
  {
    if (encodedValue == null)
      return;
    for (int index = startPos; index < encodedValue.Length; ++index)
      encodedValue[index] = Rot13LayoutRendererWrapper.DecodeRot13Char(encodedValue[index]);
  }

  private static char DecodeRot13Char(char c)
  {
    if (c >= 'A' && c <= 'M')
      return (char) (78 + ((int) c - 65));
    if (c >= 'a' && c <= 'm')
      return (char) (110 + ((int) c - 97));
    if (c >= 'N' && c <= 'Z')
      return (char) (65 + ((int) c - 78));
    return c >= 'n' && c <= 'z' ? (char) (97 + ((int) c - 110)) : c;
  }
}
