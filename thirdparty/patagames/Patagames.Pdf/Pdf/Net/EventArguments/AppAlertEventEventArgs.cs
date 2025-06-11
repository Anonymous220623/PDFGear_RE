// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.EventArguments.AppAlertEventEventArgs
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using System;

#nullable disable
namespace Patagames.Pdf.Net.EventArguments;

/// <summary>
/// Represents the class that contain event data for display alert window from SDK
/// </summary>
public class AppAlertEventEventArgs : EventArgs
{
  /// <summary>Gets displayed text.</summary>
  public string Text { get; private set; }

  /// <summary>Gets dispayed title.</summary>
  public string Title { get; private set; }

  /// <summary>Gets button type for displayed window</summary>
  public ButtonTypes ButtonType { get; private set; }

  /// <summary>Gets icon type for displayed window</summary>
  public IconTypes IconType { get; private set; }

  /// <summary>
  /// Sets the button type that the user clicked in the alert window
  /// </summary>
  public DialogResults DialogResult { get; set; }

  /// <summary>Construct AppAlertEventArgs object.</summary>
  /// <param name="Text">A string containing the message to be displayed.</param>
  /// <param name="Title">The title of the dialog.</param>
  /// <param name="ButtonType">The stype of button group. 0-OK(default); 1-OK,Cancel; 2-Yes,NO; 3-Yes, NO, Cancel.</param>
  /// <param name="IconType">The icon type</param>
  public AppAlertEventEventArgs(
    string Text,
    string Title,
    ButtonTypes ButtonType,
    IconTypes IconType)
  {
    this.Text = Text;
    this.Title = Title;
    this.ButtonType = ButtonType;
    this.IconType = IconType;
  }
}
