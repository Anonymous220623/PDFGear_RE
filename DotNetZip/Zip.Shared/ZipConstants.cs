// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ZipConstants
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

#nullable disable
namespace Ionic.Zip;

internal static class ZipConstants
{
  public const uint PackedToRemovableMedia = 808471376;
  public const uint Zip64EndOfCentralDirectoryRecordSignature = 101075792;
  public const uint Zip64EndOfCentralDirectoryLocatorSignature = 117853008;
  public const uint EndOfCentralDirectorySignature = 101010256;
  public const int ZipEntrySignature = 67324752;
  public const int ZipEntryDataDescriptorSignature = 134695760;
  public const int SplitArchiveSignature = 134695760;
  public const int ZipDirEntrySignature = 33639248;
  public const int AesKeySize = 192 /*0xC0*/;
  public const int AesBlockSize = 128 /*0x80*/;
  public const ushort AesAlgId128 = 26126;
  public const ushort AesAlgId192 = 26127;
  public const ushort AesAlgId256 = 26128;
}
