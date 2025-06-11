// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.EventArguments.ProgressiveRenderEventArgs
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Net.EventArguments;

/// <summary>
/// Represents the class that contain event data for <see cref="E:Patagames.Pdf.Net.PdfPage.ProgressiveRender" /> event
/// </summary>
public class ProgressiveRenderEventArgs : EventArgs
{
  /// <summary>
  /// A user defined data, used by user's application. Can be NULL.
  /// </summary>
  public byte[] UserData { get; private set; }

  /// <summary>
  /// Application must sets this property to true for pause now or false for continue.
  /// </summary>
  public bool NeedPause { get; set; }

  /// <summary>Construct ProgressiveRenderEventArgs object</summary>
  /// <param name="userData">A user defined data, used by user's application. Can be NULL.</param>
  public ProgressiveRenderEventArgs(byte[] userData) => this.UserData = userData;
}
