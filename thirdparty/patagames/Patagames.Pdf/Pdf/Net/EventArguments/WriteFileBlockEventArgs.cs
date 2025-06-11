// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.EventArguments.WriteFileBlockEventArgs
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Net.EventArguments;

/// <summary>
/// Provides data for the <see cref="E:Patagames.Pdf.Net.PdfDocument.WriteBlock" /> event.
/// </summary>
public class WriteFileBlockEventArgs : EventArgs
{
  /// <summary>Pointer to a buffer to output</summary>
  public byte[] Buffer { get; private set; }

  /// <summary>Should be True if successful, False for error</summary>
  public bool ReturnValue { get; set; }

  /// <summary>
  /// Initialize a ne instance of WriteFileBlockEventArgs class
  /// </summary>
  /// <param name="buffer">Pointer to a buffer to output</param>
  public WriteFileBlockEventArgs(ref byte[] buffer)
  {
    this.Buffer = buffer;
    this.ReturnValue = false;
  }
}
