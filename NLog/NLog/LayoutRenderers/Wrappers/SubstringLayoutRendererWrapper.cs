// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.SubstringLayoutRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using System;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers;

[LayoutRenderer("substring")]
[AppDomainFixedOutput]
[ThreadAgnostic]
[ThreadSafe]
public sealed class SubstringLayoutRendererWrapper : WrapperLayoutRendererBase
{
  public SubstringLayoutRendererWrapper() => this.Start = 0;

  [DefaultValue(0)]
  public int Start { get; set; }

  [DefaultValue(null)]
  public int? Length { get; set; }

  protected override void RenderInnerAndTransform(
    LogEventInfo logEvent,
    StringBuilder builder,
    int orgLength)
  {
    int? length1 = this.Length;
    int num = 0;
    if (length1.GetValueOrDefault() == num & length1.HasValue)
      return;
    this.Inner.RenderAppendBuilder(logEvent, builder);
    int textLength = builder.Length - orgLength;
    if (textLength <= 0)
      return;
    int start = this.CalcStart(textLength);
    int length2 = this.CalcLength(textLength, start);
    string str = builder.ToString(orgLength + start, length2);
    builder.Length = orgLength;
    builder.Append(str);
  }

  protected override string Transform(string text) => throw new NotSupportedException();

  private int CalcStart(int textLength)
  {
    int num = this.Start;
    if (num > textLength)
      num = textLength;
    if (num < 0)
    {
      num = textLength + num;
      if (num < 0)
        num = 0;
    }
    return num;
  }

  private int CalcLength(int textLength, int start)
  {
    int num = textLength - start;
    if (this.Length.HasValue && textLength > this.Length.Value + start)
      num = this.Length.Value;
    if (num < 0)
      num = 0;
    return num;
  }
}
