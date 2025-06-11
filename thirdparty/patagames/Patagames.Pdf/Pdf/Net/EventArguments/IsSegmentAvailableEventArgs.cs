// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.EventArguments.IsSegmentAvailableEventArgs
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Net.EventArguments;

/// <summary>
/// Represents the class that contain event data for <see cref="E:Patagames.Pdf.Net.PdfAvailabilityProvider.IsSegmentAvailable" /> event. This event called by SDK to check whether the data section is ready.
/// </summary>
/// <remarks>Report whether the specified data section is available. A section is available only if all bytes in the section is available.</remarks>
public class IsSegmentAvailableEventArgs : EventArgs
{
  /// <summary>The offset of the data section in the file.</summary>
  public int SegmentOffset { get; private set; }

  /// <summary>The size of the data section</summary>
  public int SegmentSize { get; private set; }

  /// <summary>True means the specified data section is available.</summary>
  public bool IsSegmentAvailable { get; set; }

  /// <summary>
  /// Construct <see cref="T:Patagames.Pdf.Net.EventArguments.IsSegmentAvailableEventArgs" /> object
  /// </summary>
  /// <param name="segmentOffset">The offset of the data section in the file.</param>
  /// <param name="segmentSize">The size of the data section</param>
  public IsSegmentAvailableEventArgs(int segmentOffset, int segmentSize)
  {
    this.SegmentOffset = segmentOffset;
    this.SegmentSize = segmentSize;
  }
}
