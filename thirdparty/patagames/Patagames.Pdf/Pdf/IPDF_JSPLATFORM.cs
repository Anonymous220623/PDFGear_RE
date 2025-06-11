// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.IPDF_JSPLATFORM
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>class that provides functionality of JS PDF forms</summary>
[StructLayout(LayoutKind.Sequential)]
public class IPDF_JSPLATFORM : IStaticCallbacks
{
  /// <summary>Version number of the interface. Currently must be 1.</summary>
  public int Version = 1;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.app_alert_callback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public app_alert_callback app_alert;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.app_beep_callback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public app_beep_callback app_beep;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.app_response_callback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public app_response_callback app_response;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.Doc_getFilePath_callback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public Doc_getFilePath_callback Doc_getFilePath;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.Doc_mail_callback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public Doc_mail_callback Doc_mail;
  /// <summary>
  ///  Application defined callback function. See <see cref="T:Patagames.Pdf.Doc_print_callback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public Doc_print_callback Doc_print;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.Doc_submitForm_callback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public Doc_submitForm_callback Doc_submitForm;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.Doc_gotoPage_callback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public Doc_gotoPage_callback Doc_gotoPage;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.Field_browse_callback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public Field_browse_callback Field_browse;
  /// <summary>
  /// pointer to <see cref="T:Patagames.Pdf.FPDF_FORMFILLINFO" /> interface.
  /// </summary>
  private IntPtr m_pFormfillinfo;
}
