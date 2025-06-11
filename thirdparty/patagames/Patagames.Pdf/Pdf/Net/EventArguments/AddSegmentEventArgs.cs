// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.EventArguments.AddSegmentEventArgs
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Net.EventArguments;

/// <summary>
/// Represents the class that contain event data for <see cref="E:Patagames.Pdf.Net.PdfAvailabilityProvider.AddSegment" /> event. This event called by SDK to report some downloading hints for download manager.
/// </summary>
/// <remarks>The position and size of section may be not accurate, part of the section might be already available.
/// The download manager must deal with that to maximize download efficiency.</remarks>
public class AddSegmentEventArgs : EventArgs
{
  /// <summary>
  /// Gets the offset of the hint reported to be downloaded.
  /// </summary>
  public int SegmentOffset { get; private set; }

  /// <summary>Gets the size of the hint reported to be downloaded.</summary>
  public int SegmentSize { get; private set; }

  /// <summary>
  /// Construct <see cref="T:Patagames.Pdf.Net.EventArguments.AddSegmentEventArgs" /> object
  /// </summary>
  /// <param name="segmentOffset">The offset of the hint reported to be downloaded.</param>
  /// <param name="segmentSize">The size of the hint reported to be downloaded.</param>
  public AddSegmentEventArgs(int segmentOffset, int segmentSize)
  {
    this.SegmentOffset = segmentOffset;
    this.SegmentSize = segmentSize;
  }
}
