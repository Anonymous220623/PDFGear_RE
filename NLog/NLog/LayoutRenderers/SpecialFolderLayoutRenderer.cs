// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.SpecialFolderLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("specialfolder")]
[AppDomainFixedOutput]
[ThreadAgnostic]
[ThreadSafe]
public class SpecialFolderLayoutRenderer : LayoutRenderer
{
  [DefaultParameter]
  public Environment.SpecialFolder Folder { get; set; }

  public string File { get; set; }

  public string Dir { get; set; }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    string str = PathHelpers.CombinePaths(Environment.GetFolderPath(this.Folder), this.Dir, this.File);
    builder.Append(str);
  }
}
