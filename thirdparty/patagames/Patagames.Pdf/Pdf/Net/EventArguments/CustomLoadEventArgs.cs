// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.EventArguments.CustomLoadEventArgs
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Net.EventArguments;

/// <summary>
/// Represents the class that contain event data for <see cref="E:Patagames.Pdf.Net.PdfCustomLoader.LoadBlock" /> event
/// </summary>
public class CustomLoadEventArgs : EventArgs
{
  /// <summary>Gets custom user data</summary>
  public byte[] UserData { get; private set; }

  /// <summary>
  /// Gets position is specified by byte offset from beginning of the file.
  /// </summary>
  public uint Position { get; private set; }

  /// <summary>
  /// Gets buffer for loaded data allocated inside Pdfium SDK.
  /// </summary>
  public byte[] Buffer { get; private set; }

  /// <summary>Should be true if successful, false for error.</summary>
  public bool ReturnValue { get; set; }

  /// <summary>Construct CustomLoadEventArgs object</summary>
  /// <param name="userData">Custom user data</param>
  /// <param name="position">Position is specified by byte offset from beginning of the file.</param>
  /// <param name="buffer">Buffer for loaded data allocated inside Pdfium SDK.</param>
  public CustomLoadEventArgs(byte[] userData, uint position, byte[] buffer)
  {
    this.UserData = userData;
    this.Position = position;
    this.Buffer = buffer;
    this.ReturnValue = false;
  }
}
