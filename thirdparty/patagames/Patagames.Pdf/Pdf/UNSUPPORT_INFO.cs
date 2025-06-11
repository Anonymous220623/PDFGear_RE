// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.UNSUPPORT_INFO
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>Represents UNSUPPORT_INFO structure.</summary>
[StructLayout(LayoutKind.Sequential)]
public class UNSUPPORT_INFO : IStaticCallbacks
{
  /// <summary>Version number of the interface. Currently must be 1.</summary>
  public int Version = 1;
  /// <summary>
  /// User callback function. See <see cref="T:Patagames.Pdf.FSDKUnsupportHandlerCallback" /> delegate for detail
  /// </summary>
  [MarshalAs(UnmanagedType.FunctionPtr)]
  public FSDKUnsupportHandlerCallback FSDKUnsupportHandler;
}
