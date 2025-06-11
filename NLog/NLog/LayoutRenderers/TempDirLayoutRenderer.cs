// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.TempDirLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System.IO;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("tempdir")]
[AppDomainFixedOutput]
[ThreadAgnostic]
[ThreadSafe]
public class TempDirLayoutRenderer : LayoutRenderer
{
  private static string tempDir;

  public string File { get; set; }

  public string Dir { get; set; }

  protected override void InitializeLayoutRenderer()
  {
    if (TempDirLayoutRenderer.tempDir == null)
      TempDirLayoutRenderer.tempDir = Path.GetTempPath();
    base.InitializeLayoutRenderer();
  }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    string str = PathHelpers.CombinePaths(TempDirLayoutRenderer.tempDir, this.Dir, this.File);
    builder.Append(str);
  }
}
