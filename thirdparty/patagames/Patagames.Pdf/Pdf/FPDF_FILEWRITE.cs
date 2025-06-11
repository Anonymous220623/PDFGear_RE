// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.FPDF_FILEWRITE
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>Class for custom file write</summary>
[StructLayout(LayoutKind.Sequential)]
public class FPDF_FILEWRITE
{
  /// <summary>Version number of the interface. Currently must be 1.</summary>
  private int Version = 1;
  /// <summary>
  /// UserCallbackFunction. See <see cref="T:Patagames.Pdf.WriteBlockCallback" /> delegate for detail
  /// </summary>
  /// <remarks>Required: Yes.</remarks>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public WriteBlockCallback WriteBlock;
}
