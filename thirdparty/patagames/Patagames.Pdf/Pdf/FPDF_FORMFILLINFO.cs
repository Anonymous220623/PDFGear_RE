// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.FPDF_FORMFILLINFO
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>Class for manipulates with PDF forms</summary>
[StructLayout(LayoutKind.Sequential)]
public class FPDF_FORMFILLINFO : IStaticCallbacks
{
  /// <summary>Version number of the interface. Currently must be 1.</summary>
  public int Version = 1;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.ReleaseCallback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: No.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public ReleaseCallback Release;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.FFI_InvalidateCallback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public FFI_InvalidateCallback FFI_Invalidate;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.FFI_OutputSelectedRectCallback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: No.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public FFI_OutputSelectedRectCallback FFI_OutputSelectedRect;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.FFI_SetCursorCallback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public FFI_SetCursorCallback FFI_SetCursor;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.FFI_SetTimerCallback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public FFI_SetTimerCallback FFI_SetTimer;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.FFI_KillTimerCallback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public FFI_KillTimerCallback FFI_KillTimer;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.FFI_GetLocalTimeCallback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public FFI_GetLocalTimeCallback FFI_GetLocalTime;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.FFI_OnChangeCallback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: No.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public FFI_OnChangeCallback FFI_OnChange;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.FFI_GetPageCallback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public FFI_GetPageCallback FFI_GetPage;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.FFI_GetCurrentPageCallback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public FFI_GetCurrentPageCallback FFI_GetCurrentPage;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.FFI_GetRotationCallback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public FFI_GetRotationCallback FFI_GetRotation;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.FFI_ExecuteNamedActionCallback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public FFI_ExecuteNamedActionCallback FFI_ExecuteNamedAction;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.FFI_SetTextFieldFocusCallback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: No.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public FFI_SetTextFieldFocusCallback FFI_SetTextFieldFocus;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.FFI_DoURIActionCallback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public FFI_DoURIActionCallback FFI_DoURIAction;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.FFI_DoGoToActionCallback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: No.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public FFI_DoGoToActionCallback FFI_DoGoToAction;
  /// <summary>
  /// pointer to <see cref="T:Patagames.Pdf.IPDF_JSPLATFORM" /> interface
  /// </summary>
  private IntPtr m_pJsPlatform;
}
