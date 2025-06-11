// Decompiled with JetBrains decompiler
// Type: Patagames.Activation.FileSystemFeature
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Activation;

[Flags]
internal enum FileSystemFeature : uint
{
  None = 0,
  /// <summary>
  /// The file system preserves the case of file names when it places a name on disk.
  /// </summary>
  CasePreservedNames = 2,
  /// <summary>The file system supports case-sensitive file names.</summary>
  CaseSensitiveSearch = 1,
  /// <summary>
  /// The specified volume is a direct access (DAX) volume. This flag was introduced in Windows 10, version 1607.
  /// </summary>
  DaxVolume = 536870912, // 0x20000000
  /// <summary>The file system supports file-based compression.</summary>
  FileCompression = 16, // 0x00000010
  /// <summary>The file system supports named streams.</summary>
  NamedStreams = 262144, // 0x00040000
  /// <summary>
  /// The file system preserves and enforces access control lists (ACL).
  /// </summary>
  PersistentACLS = 8,
  /// <summary>The specified volume is read-only.</summary>
  ReadOnlyVolume = 524288, // 0x00080000
  /// <summary>The volume supports a single sequential write.</summary>
  SequentialWriteOnce = 1048576, // 0x00100000
  /// <summary>
  /// The file system supports the Encrypted File System (EFS).
  /// </summary>
  SupportsEncryption = 131072, // 0x00020000
  /// <summary>
  /// The specified volume supports extended attributes. An extended attribute is a piece of
  /// application-specific metadata that an application can associate with a file and is not part
  /// of the file's data.
  /// </summary>
  SupportsExtendedAttributes = 8388608, // 0x00800000
  /// <summary>
  /// The specified volume supports hard links. For more information, see Hard Links and Junctions.
  /// </summary>
  SupportsHardLinks = 4194304, // 0x00400000
  /// <summary>The file system supports object identifiers.</summary>
  SupportsObjectIDs = 65536, // 0x00010000
  /// <summary>
  /// The file system supports open by FileID. For more information, see FILE_ID_BOTH_DIR_INFO.
  /// </summary>
  SupportsOpenByFileId = 16777216, // 0x01000000
  /// <summary>The file system supports re-parse points.</summary>
  SupportsReparsePoints = 128, // 0x00000080
  /// <summary>The file system supports sparse files.</summary>
  SupportsSparseFiles = 64, // 0x00000040
  /// <summary>The volume supports transactions.</summary>
  SupportsTransactions = 2097152, // 0x00200000
  /// <summary>
  /// The specified volume supports update sequence number (USN) journals. For more information,
  /// see Change Journal Records.
  /// </summary>
  SupportsUsnJournal = 33554432, // 0x02000000
  /// <summary>
  /// The file system supports Unicode in file names as they appear on disk.
  /// </summary>
  UnicodeOnDisk = 4,
  /// <summary>
  /// The specified volume is a compressed volume, for example, a DoubleSpace volume.
  /// </summary>
  VolumeIsCompressed = 32768, // 0x00008000
  /// <summary>The file system supports disk quotas.</summary>
  VolumeQuotas = 32, // 0x00000020
}
