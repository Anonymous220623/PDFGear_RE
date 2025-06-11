// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.FileSystemNormalizeLayoutRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using System;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers;

[LayoutRenderer("filesystem-normalize")]
[AmbientProperty("FSNormalize")]
[AppDomainFixedOutput]
[ThreadAgnostic]
[ThreadSafe]
public sealed class FileSystemNormalizeLayoutRendererWrapper : WrapperLayoutRendererBuilderBase
{
  public FileSystemNormalizeLayoutRendererWrapper() => this.FSNormalize = true;

  [DefaultValue(true)]
  public bool FSNormalize { get; set; }

  protected override void RenderInnerAndTransform(
    LogEventInfo logEvent,
    StringBuilder builder,
    int orgLength)
  {
    this.Inner.RenderAppendBuilder(logEvent, builder);
    if (!this.FSNormalize || builder.Length <= orgLength)
      return;
    FileSystemNormalizeLayoutRendererWrapper.TransformFileSystemNormalize(builder, orgLength);
  }

  [Obsolete("Inherit from WrapperLayoutRendererBase and override RenderInnerAndTransform() instead. Marked obsolete in NLog 4.6")]
  protected override void TransformFormattedMesssage(StringBuilder target)
  {
  }

  private static void TransformFileSystemNormalize(StringBuilder builder, int startPos)
  {
    for (int index = startPos; index < builder.Length; ++index)
    {
      if (!FileSystemNormalizeLayoutRendererWrapper.IsSafeCharacter(builder[index]))
        builder[index] = '_';
    }
  }

  private static bool IsSafeCharacter(char c)
  {
    return char.IsLetterOrDigit(c) || c == '_' || c == '-' || c == '.' || c == ' ';
  }
}
