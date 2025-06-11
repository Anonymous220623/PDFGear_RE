// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.FX_FILEAVAIL
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>
/// Interface for checking whether the section of the file is available.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public class FX_FILEAVAIL : IStaticCallbacks
{
  /// <summary>Version number of the interface. Currently must be 1.</summary>
  public int Version = 1;
  /// <summary>
  /// User callback function. See <see cref="T:Patagames.Pdf.IsDataAvailableCallback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public IsDataAvailableCallback IsDataAvailable;
}
