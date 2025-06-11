// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.EventArguments.ReadSegmentEventArgs
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Net.EventArguments;

/// <summary>
/// Represents the class that contain event data for <see cref="E:Patagames.Pdf.Net.PdfAvailabilityProvider.ReadSegment" /> event. This event called by SDK to read some data from custom source.
/// </summary>
/// <remarks>SegmentOffset is specified by byte offset from beginning of the file. It may be possible for SDK to call this event multiple times for same position.</remarks>
public class ReadSegmentEventArgs : EventArgs
{
  /// <summary>Custom user data</summary>
  public byte[] UserData { get; private set; }

  /// <summary>
  /// Position is specified by byte offset from beginning of the file.
  /// </summary>
  public int SegmentOffset { get; private set; }

  /// <summary>Buffer for data allocated inside SDK</summary>
  public byte[] DstBuffer { get; private set; }

  /// <summary>Buffer size</summary>
  public int SegmentSize { get; private set; }

  /// <summary>Should be true if successful, false for error.</summary>
  public bool IsSuccess { get; set; }

  /// <summary>
  /// Construct <see cref="T:Patagames.Pdf.Net.EventArguments.ReadSegmentEventArgs" /> object.
  /// </summary>
  /// <param name="userData">Custom user data</param>
  /// <param name="segmentOffset">Position is specified by byte offset from beginning of the file.</param>
  /// <param name="dstBuffer">Buffer for data allocated inside SDK</param>
  /// <param name="segmentSize">Buffer size</param>
  public ReadSegmentEventArgs(
    byte[] userData,
    int segmentOffset,
    byte[] dstBuffer,
    int segmentSize)
  {
    this.UserData = userData;
    this.SegmentOffset = segmentOffset;
    this.DstBuffer = dstBuffer;
    this.SegmentSize = segmentSize;
  }
}
