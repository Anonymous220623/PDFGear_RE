// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.Deflate64.IFileFormatReader
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

#nullable disable
namespace Ionic.Zip.Deflate64;

internal interface IFileFormatReader
{
  bool ReadHeader(InputBuffer input);

  bool ReadFooter(InputBuffer input);

  void UpdateWithBytesRead(byte[] buffer, int offset, int bytesToCopy);

  void Validate();
}
