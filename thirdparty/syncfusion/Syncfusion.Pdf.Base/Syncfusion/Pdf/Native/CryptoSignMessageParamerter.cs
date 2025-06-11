// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Native.CryptoSignMessageParamerter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Native;

internal struct CryptoSignMessageParamerter
{
  public uint SizeInBytes;
  public uint EncodingType;
  public IntPtr SigningCertPointer;
  public CRYPT_ALGORITHM_IDENTIFIER HashAlgorithm;
  public IntPtr HashAuxInfo;
  public uint MessageCertificateCount;
  public IntPtr MessageCertificate;
  public uint MessageCrlCount;
  public IntPtr MessageCrl;
  public uint AuthenticatedAttributeCount;
  public IntPtr AuthenticatedAttribute;
  public uint UnauthenticatedAttributeCount;
  public IntPtr UnauthenticatedAttribute;
  public uint CrytographicSilentFlag;
  public uint InnerContentType;
  public CRYPT_ALGORITHM_IDENTIFIER HashEncryptionAlgorithm;
  public IntPtr HashEncryptionAux;
}
