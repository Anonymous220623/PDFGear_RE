// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.SelfExtractorSaveOptions
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

using System;

#nullable disable
namespace Ionic.Zip;

public class SelfExtractorSaveOptions
{
  public SelfExtractorFlavor Flavor { get; set; }

  public string PostExtractCommandLine { get; set; }

  public string DefaultExtractDirectory { get; set; }

  public string IconFile { get; set; }

  public bool Quiet { get; set; }

  public ExtractExistingFileAction ExtractExistingFile { get; set; }

  public bool RemoveUnpackedFilesAfterExecute { get; set; }

  public Version FileVersion { get; set; }

  public string ProductVersion { get; set; }

  public string Copyright { get; set; }

  public string Description { get; set; }

  public string ProductName { get; set; }

  public string SfxExeWindowTitle { get; set; }

  public string AdditionalCompilerSwitches { get; set; }

  public string CompilerVersion { get; set; }
}
