// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.JsonEncodeLayoutRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Targets;
using System;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers;

[LayoutRenderer("json-encode")]
[AmbientProperty("JsonEncode")]
[AppDomainFixedOutput]
[ThreadAgnostic]
[ThreadSafe]
public sealed class JsonEncodeLayoutRendererWrapper : WrapperLayoutRendererBuilderBase
{
  internal bool? EscapeForwardSlashInternal;

  public JsonEncodeLayoutRendererWrapper()
  {
    this.JsonEncode = true;
    this.EscapeUnicode = true;
  }

  [DefaultValue(true)]
  public bool JsonEncode { get; set; }

  [DefaultValue(true)]
  public bool EscapeUnicode { get; set; }

  [DefaultValue(true)]
  public bool EscapeForwardSlash
  {
    get => this.EscapeForwardSlashInternal ?? true;
    set => this.EscapeForwardSlashInternal = new bool?(value);
  }

  protected override void RenderInnerAndTransform(
    LogEventInfo logEvent,
    StringBuilder builder,
    int orgLength)
  {
    this.Inner.RenderAppendBuilder(logEvent, builder);
    if (!this.JsonEncode || builder.Length <= orgLength || !this.RequiresJsonEncode(builder, orgLength))
      return;
    string text = builder.ToString(orgLength, builder.Length - orgLength);
    builder.Length = orgLength;
    DefaultJsonSerializer.AppendStringEscape(builder, text, this.EscapeUnicode, this.EscapeForwardSlash);
  }

  [Obsolete("Inherit from WrapperLayoutRendererBase and override RenderInnerAndTransform() instead. Marked obsolete in NLog 4.6")]
  protected override void TransformFormattedMesssage(StringBuilder target)
  {
  }

  private bool RequiresJsonEncode(StringBuilder target, int startPos = 0)
  {
    for (int index = startPos; index < target.Length; ++index)
    {
      if (DefaultJsonSerializer.RequiresJsonEscape(target[index], this.EscapeUnicode, this.EscapeForwardSlash))
        return true;
    }
    return false;
  }
}
