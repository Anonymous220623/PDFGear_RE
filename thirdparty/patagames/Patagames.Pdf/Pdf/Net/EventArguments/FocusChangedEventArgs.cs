// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.EventArguments.FocusChangedEventArgs
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Net.EventArguments;

/// <summary>
/// Represents the class that contain event data for <see cref="E:Patagames.Pdf.Net.PdfForms.FocusChanged" /> event
/// </summary>
/// <remarks>Currently,only support text field and combobox field.</remarks>
public class FocusChangedEventArgs : EventArgs
{
  /// <summary>The string value of the form field.</summary>
  public string Value { get; private set; }

  /// <summary>
  /// True if the form field is getting a focus, False for losing a focus.
  /// </summary>
  public bool IsFocused { get; private set; }

  /// <summary>Construct FocusChangedEventArgs object</summary>
  /// <param name="value">The string value of the form field.</param>
  /// <param name="isFocused">True if the form field is getting a focus, False for losing a focus.</param>
  public FocusChangedEventArgs(string value, bool isFocused)
  {
    this.Value = value;
    this.IsFocused = isFocused;
  }
}
