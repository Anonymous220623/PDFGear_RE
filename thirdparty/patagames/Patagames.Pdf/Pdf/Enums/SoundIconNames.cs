// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.SoundIconNames
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.ComponentModel;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>
/// Represents the name of an icon to be used in displaying the <see cref="T:Patagames.Pdf.Net.Annotations.PdfSoundAnnotation" />
/// </summary>
/// <remarks>
/// 
/// Viewer applications should provide predefined icon appearances for at least the following standard names
/// </remarks>
public enum SoundIconNames
{
  /// <summary>Speaker stamp icon</summary>
  [Description("Speaker")] Speaker,
  /// <summary>Microphone stamp icon</summary>
  [Description("Mic")] Mic,
  /// <summary>
  /// Please see <see cref="P:Patagames.Pdf.Net.Annotations.PdfSoundAnnotation.ExtendedIconName" /> property
  /// </summary>
  Extended,
}
