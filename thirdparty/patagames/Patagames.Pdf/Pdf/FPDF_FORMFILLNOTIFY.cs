// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.FPDF_FORMFILLNOTIFY
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>
/// Represents a class that contains callbacks which receives the notification from the forms.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public class FPDF_FORMFILLNOTIFY : IStaticCallbacks
{
  /// <summary>Version number of the interface. Currently must be 1.</summary>
  public int Version = 1;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.BeforeValueChangeCallback" /> delegate for details
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public BeforeValueChangeCallback BeforeValueChange;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.AfterValueChangeCallback" /> delegate for details
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public AfterValueChangeCallback AfterValueChange;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.BeforeSelectionChangeCallback" /> delegate for details
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public BeforeSelectionChangeCallback BeforeSelectionChange;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.AfterSelectionChangeCallback" /> delegate for details
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public AfterSelectionChangeCallback AfterSelectionChange;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.AfterCheckedStatusChangeCallback" /> delegate for details
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public AfterCheckedStatusChangeCallback AfterCheckedStatusChange;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.BeforeFormResetCallback" /> delegate for details
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public BeforeFormResetCallback BeforeFormReset;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.AfterFormResetCallback" /> delegate for details
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public AfterFormResetCallback AfterFormReset;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.BeforeFormImportDataCallback" /> delegate for details
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public BeforeFormImportDataCallback BeforeFormImportData;
  /// <summary>
  /// Application defined callback function. See <see cref="T:Patagames.Pdf.AfterFormImportDataCallback" /> delegate for details
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public AfterFormImportDataCallback AfterFormImportData;
}
