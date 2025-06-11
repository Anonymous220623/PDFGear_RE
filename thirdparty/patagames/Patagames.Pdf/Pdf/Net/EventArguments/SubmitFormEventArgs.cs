// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.EventArguments.SubmitFormEventArgs
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Net.EventArguments;

/// <summary>
/// Represents the class that contain event data for <see cref="E:Patagames.Pdf.Net.PdfForms.SubmitForm" /> event
/// </summary>
public class SubmitFormEventArgs : EventArgs
{
  /// <summary>Gets the form data to be sent.</summary>
  public byte[] FormData { get; private set; }

  /// <summary>The URL to send to.</summary>
  public string Url { get; private set; }

  /// <summary>Construct SubmitFormEventArgs object</summary>
  /// <param name="formData">The form data to be sent.</param>
  /// <param name="url">The URL to send to.</param>
  public SubmitFormEventArgs(byte[] formData, string url)
  {
    this.FormData = formData;
    this.Url = url;
  }
}
