// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ZipEntryTimestamp
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

using System;

#nullable disable
namespace Ionic.Zip;

[Flags]
public enum ZipEntryTimestamp
{
  None = 0,
  DOS = 1,
  Windows = 2,
  Unix = 4,
  InfoZip1 = 8,
}
