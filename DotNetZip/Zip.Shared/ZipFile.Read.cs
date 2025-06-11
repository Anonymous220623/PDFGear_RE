// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ReadOptions
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Ionic.Zip;

public class ReadOptions
{
  public EventHandler<ReadProgressEventArgs> ReadProgress { get; set; }

  public TextWriter StatusMessageWriter { get; set; }

  public Encoding Encoding { get; set; }
}
