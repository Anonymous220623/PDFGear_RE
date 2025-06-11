// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.EncriptionAlgorithm
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>A type of action associated with bookmark</summary>
public enum EncriptionAlgorithm
{
  /// <summary>
  /// The Advanced Encryption Standard algorithm with a key length of 128 bits is used.
  /// </summary>
  AES128,
  /// <summary>
  /// The Advanced Encryption Standard algorithm with a key length of 256 bits is used.
  /// </summary>
  AES256,
  /// <summary>
  /// The Alleged Rivest Cipher 4 algorithm with a key length of 128 bits is used.
  /// </summary>
  /// <remarks>
  /// RC4 is a copyrighted, proprietary algorithm of RSA Security, Inc. Independent
  /// software vendors may be required to license RC4 to develop software that encrypts
  /// or decrypts PDF documents.
  /// </remarks>
  ARCFour128,
}
