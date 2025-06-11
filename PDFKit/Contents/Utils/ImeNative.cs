// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.Utils.ImeNative
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

#nullable disable
namespace PDFKit.Contents.Utils;

internal static class ImeNative
{
  [ThreadStatic]
  private static bool TextFrameworkThreadMgrInitialized;
  [ThreadStatic]
  private static ImeNative.ITfThreadMgr TextFrameworkThreadMgr;
  internal const int CPS_CANCEL = 4;
  internal const int NI_COMPOSITIONSTR = 21;
  internal const int GCS_COMPSTR = 8;
  internal const int WM_IME_COMPOSITION = 271;
  internal const int WM_IME_SETCONTEXT = 641;
  internal const int WM_INPUTLANGCHANGE = 81;
  internal const int IMC_SETCANDIDATEPOS = 153;

  [DllImport("imm32.dll")]
  internal static extern IntPtr ImmAssociateContext(IntPtr hWnd, IntPtr hIMC);

  [DllImport("imm32.dll")]
  internal static extern IntPtr ImmGetContext(IntPtr hWnd);

  [DllImport("imm32.dll")]
  internal static extern IntPtr ImmGetDefaultIMEWnd(IntPtr hWnd);

  [DllImport("imm32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  internal static extern bool ImmReleaseContext(IntPtr hWnd, IntPtr hIMC);

  [DllImport("imm32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  internal static extern bool ImmNotifyIME(IntPtr hIMC, int dwAction, int dwIndex, int dwValue = 0);

  [DllImport("imm32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  internal static extern bool ImmSetCompositionWindow(
    IntPtr hIMC,
    ref ImeNative.CompositionForm form);

  [DllImport("imm32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  internal static extern bool ImmGetCompositionWindow(
    IntPtr hIMC,
    out ImeNative.CompositionForm form);

  [DllImport("imm32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  internal static extern bool ImmGetCandidateWindow(
    IntPtr hIMC,
    int dwIndex,
    out ImeNative.CandidateForm form);

  [DllImport("imm32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  internal static extern bool ImmSetCandidateWindow(IntPtr hIMC, ref ImeNative.CandidateForm form);

  [DllImport("imm32.dll", CharSet = CharSet.Unicode)]
  [return: MarshalAs(UnmanagedType.Bool)]
  internal static extern bool ImmSetCompositionFont(IntPtr hIMC, ref ImeNative.LOGFONT font);

  [DllImport("imm32.dll")]
  internal static extern int ImmGetCompositionString(
    IntPtr hIMC,
    int dwIndex,
    byte[] data,
    int bufLen);

  [DllImport("msctf.dll")]
  internal static extern int TF_CreateThreadMgr(out ImeNative.ITfThreadMgr threadMgr);

  internal static ImeNative.ITfThreadMgr GetTextFrameworkThreadManager()
  {
    if (ImeNative.TextFrameworkThreadMgrInitialized)
      return ImeNative.TextFrameworkThreadMgr;
    ImeNative.TextFrameworkThreadMgrInitialized = true;
    ImeNative.TF_CreateThreadMgr(out ImeNative.TextFrameworkThreadMgr);
    return ImeNative.TextFrameworkThreadMgr;
  }

  internal struct TF_LANGUAGEPROFILE
  {
    internal Guid clsid;
    internal short langid;
    internal Guid catid;
    [MarshalAs(UnmanagedType.Bool)]
    internal bool fActive;
    internal Guid guidProfile;
  }

  internal struct POINT
  {
    public int x;
    public int y;

    public override string ToString() => $"{this.x.ToString()} {this.y.ToString()}";
  }

  internal struct RECT
  {
    public int left;
    public int top;
    public int right;
    public int bottom;

    public override string ToString()
    {
      return $"{this.left.ToString()} {this.top.ToString()} {this.right.ToString()} {this.bottom.ToString()}";
    }
  }

  internal struct CompositionForm
  {
    public int dwStyle;
    public ImeNative.POINT ptCurrentPos;
    public ImeNative.RECT rcArea;
  }

  internal struct CandidateForm
  {
    public int dwIndex;
    public int dwStyle;
    public ImeNative.POINT ptCurrentPos;
    public ImeNative.RECT rcArea;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  internal struct LOGFONT
  {
    public int lfHeight;
    public int lfWidth;
    public int lfEscapement;
    public int lfOrientation;
    public int lfWeight;
    public byte lfItalic;
    public byte lfUnderline;
    public byte lfStrikeOut;
    public byte lfCharSet;
    public byte lfOutPrecision;
    public byte lfClipPrecision;
    public byte lfQuality;
    public byte lfPitchAndFamily;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32 /*0x20*/)]
    public string lfFaceName;
  }

  internal static class TSF_NativeAPI
  {
    public static readonly Guid GUID_TFCAT_TIP_KEYBOARD = new Guid(880041059U, (ushort) 45808, (ushort) 18308, (byte) 139, (byte) 103, (byte) 94, (byte) 18, (byte) 200, (byte) 112 /*0x70*/, (byte) 26, (byte) 49);

    [SecurityCritical]
    [SuppressUnmanagedCodeSecurity]
    [DllImport("msctf.dll")]
    public static extern int TF_CreateInputProcessorProfiles(
      out ImeNative.ITfInputProcessorProfiles profiles);
  }

  [Guid("aa80e801-2021-11d2-93e0-0060b067b86e")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  internal interface ITfThreadMgr
  {
    void Activate(out int clientId);

    void Deactivate();

    void CreateDocumentMgr(out IntPtr docMgr);

    void EnumDocumentMgrs(out IntPtr enumDocMgrs);

    void GetFocus(out IntPtr docMgr);

    void SetFocus(IntPtr docMgr);

    void AssociateFocus(IntPtr hwnd, IntPtr newDocMgr, out IntPtr prevDocMgr);

    void IsThreadFocus([MarshalAs(UnmanagedType.Bool)] out bool isFocus);

    void GetFunctionProvider(ref Guid classId, out IntPtr funcProvider);

    void EnumFunctionProviders(out IntPtr enumProviders);

    void GetGlobalCompartment(out IntPtr compartmentMgr);
  }

  [SecurityCritical]
  [SuppressUnmanagedCodeSecurity]
  [Guid("1F02B6C5-7842-4EE6-8A0B-9A24183A95CA")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  internal interface ITfInputProcessorProfiles
  {
    [SecurityCritical]
    void Register();

    [SecurityCritical]
    void Unregister();

    [SecurityCritical]
    void AddLanguageProfile();

    [SecurityCritical]
    void RemoveLanguageProfile();

    [SecurityCritical]
    void EnumInputProcessorInfo();

    [SecurityCritical]
    int GetDefaultLanguageProfile(short langid, ref Guid catid, out Guid clsid, out Guid profile);

    [SecurityCritical]
    void SetDefaultLanguageProfile();

    [SecurityCritical]
    int ActivateLanguageProfile(ref Guid clsid, short langid, ref Guid guidProfile);

    [SecurityCritical]
    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetActiveLanguageProfile(ref Guid clsid, out short langid, out Guid profile);

    [SecurityCritical]
    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetLanguageProfileDescription(
      ref Guid clsid,
      short langid,
      ref Guid profile,
      out IntPtr desc);

    [SecurityCritical]
    void GetCurrentLanguage(out short langid);

    [SecurityCritical]
    [MethodImpl(MethodImplOptions.PreserveSig)]
    int ChangeCurrentLanguage(short langid);

    [SecurityCritical]
    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetLanguageList(out IntPtr langids, out int count);

    [SecurityCritical]
    int EnumLanguageProfiles(short langid, out ImeNative.IEnumTfLanguageProfiles enumIPP);

    [SecurityCritical]
    int EnableLanguageProfile();

    [SecurityCritical]
    int IsEnabledLanguageProfile(ref Guid clsid, short langid, ref Guid profile, out bool enabled);

    [SecurityCritical]
    void EnableLanguageProfileByDefault();

    [SecurityCritical]
    void SubstituteKeyboardLayout();
  }

  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("3d61bf11-ac5f-42c8-a4cb-931bcc28c744")]
  [ComImport]
  internal interface IEnumTfLanguageProfiles
  {
    void Clone(out ImeNative.IEnumTfLanguageProfiles enumIPP);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Next(int count, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2), Out] ImeNative.TF_LANGUAGEPROFILE[] profiles, out int fetched);

    void Reset();

    void Skip(int count);
  }
}
