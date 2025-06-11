// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.EventArguments.AppResponseEventArgs
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Net.EventArguments;

/// <summary>
/// Represents the class that contain event data for display response window from SDK
/// </summary>
public class AppResponseEventArgs : EventArgs
{
  /// <summary>The question to be posed to the user.</summary>
  public string Question { get; private set; }

  /// <summary>The title of the dialog box.</summary>
  public string Title { get; private set; }

  /// <summary>
  /// A default value for the answer to the question. If not specified, no default value is presented.
  /// </summary>
  public string Default { get; private set; }

  /// <summary>
  /// A short string to appear in front of and on the same line as the edit text field.
  /// </summary>
  public string cLabel { get; private set; }

  /// <summary>
  /// If true, indicates that the user's response should show as asterisks (*) or bullets (?) to mask the response, which might be sensitive information. The default is false.
  /// </summary>
  public bool IsPassword { get; private set; }

  /// <summary>To receive the user's response.</summary>
  public string UserResponse { get; set; }

  /// <summary>Construct AppAlertEventArgs object.</summary>
  /// <param name="Question">The question to be posed to the user.</param>
  /// <param name="Title">The title of the dialog box.</param>
  /// <param name="Default">A default value for the answer to the question. If not specified, no default value is presented.</param>
  /// <param name="cLabel">A short string to appear in front of and on the same line as the edit text field.</param>
  /// <param name="IsPassword">If true, indicates that the user's response should show as asterisks (*) or bullets (?) to mask the response, which might be sensitive information. The default is false.</param>
  public AppResponseEventArgs(
    string Question,
    string Title,
    string Default,
    string cLabel,
    bool IsPassword)
  {
    this.Question = Question;
    this.Title = Title;
    this.Default = Default;
    this.cLabel = cLabel;
    this.IsPassword = IsPassword;
  }
}
