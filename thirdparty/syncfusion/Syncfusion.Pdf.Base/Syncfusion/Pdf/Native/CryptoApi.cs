// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Native.CryptoApi
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.Pdf.Native;

internal sealed class CryptoApi
{
  private CryptoApi()
  {
  }

  [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  public static extern IntPtr CertOpenStore(
    [MarshalAs(UnmanagedType.LPStr)] string storeProvider,
    uint dwMsgAndCertEncodingType,
    IntPtr hCryptProv,
    uint dwFlags,
    string cchNameString);

  [DllImport("crypt32.dll", SetLastError = true)]
  public static extern IntPtr CertFindCertificateInStore(
    IntPtr hCertStore,
    uint dwCertEncodingType,
    uint dwFindFlags,
    uint dwFindType,
    [MarshalAs(UnmanagedType.LPWStr), In] string pszFindString,
    IntPtr pPrevCertCntxt);

  [DllImport("crypt32.dll", SetLastError = true)]
  public static extern IntPtr CertEnumCertificatesInStore(
    IntPtr storeProvider,
    IntPtr prevCertContext);

  [DllImport("crypt32.dll", SetLastError = true)]
  public static extern IntPtr CertDuplicateCertificateContext(IntPtr pCertContext);

  [DllImport("crypt32.dll", SetLastError = true)]
  public static extern bool CertFreeCertificateContext(IntPtr pCertContext);

  [DllImport("Crypt32.dll", CharSet = CharSet.Ansi)]
  public static extern bool CryptSignMessage(
    ref CryptoSignMessageParamerter pSignPara,
    bool fDetachedSignature,
    uint cToBeSigned,
    IntPtr[] rgpbToBeSigned,
    int[] rgcbToBeSigned,
    IntPtr pbSignedBlob,
    ref uint pcbSignedBlob);

  [DllImport("CRYPT32.DLL", CharSet = CharSet.Auto, SetLastError = true)]
  [return: MarshalAs(UnmanagedType.Bool)]
  public static extern bool CertCloseStore(IntPtr storeProvider, int flags);

  [DllImport("crypt32.dll")]
  public static extern bool CryptDecodeObject(
    uint CertEncodingType,
    uint lpszStructType,
    IntPtr pbEncoded,
    int cbEncoded,
    uint flags,
    IntPtr pvStructInfo,
    ref int cbStructInfo);

  [DllImport("crypt32.dll", SetLastError = true)]
  public static extern bool PFXIsPFXBlob(ref CryptographicDataStore pPfx);

  [DllImport("crypt32.dll", SetLastError = true)]
  public static extern IntPtr PFXImportCertStore(
    ref CryptographicDataStore pPfx,
    [MarshalAs(UnmanagedType.LPWStr)] string szPassword,
    uint dwFlags);

  [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  public static extern IntPtr CertOpenSystemStore(IntPtr hCryptProv, string storename);

  [DllImport("Cryptdll.dll", CharSet = CharSet.Ansi)]
  public static extern void MD5Init(ref Md5_Ctx context);

  [DllImport("Cryptdll.dll", CharSet = CharSet.Ansi)]
  public static extern void MD5Update(ref Md5_Ctx context, byte[] input, int inlen);

  [DllImport("Cryptdll.dll", CharSet = CharSet.Ansi)]
  public static extern void MD5Final(ref Md5_Ctx context);
}
