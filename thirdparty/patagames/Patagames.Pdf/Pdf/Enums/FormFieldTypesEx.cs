// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.FormFieldTypesEx
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>Field types</summary>
public enum FormFieldTypesEx
{
  /// <summary>Unknown field type</summary>
  Unknown,
  /// <summary>
  /// PA pushbutton is a purely interactive control that responds immediately to user input without retaining a permanent value
  /// </summary>
  PushButton,
  /// <summary>
  /// Radio button fields contain a set of related buttons that can each be on or off. Typically, at most one radio button in a set may be on at any given time, and selecting any one of the buttons automatically deselects all the others.
  /// </summary>
  RadioButton,
  /// <summary>A check box toggles between two states, on and off</summary>
  CheckBox,
  /// <summary>
  /// Text fields are boxes or spaces in which the user can enter text from the keyboard.
  /// </summary>
  Text,
  /// <summary>Rich text</summary>
  RichText,
  /// <summary>File</summary>
  File,
  /// <summary>ListBox</summary>
  ListBox,
  /// <summary>ComboBox</summary>
  ComboBox,
  /// <summary>
  /// Signature fields represent electronic signatures for authenticating the identity of a user and the validity of the document’s contents.
  /// </summary>
  Sign,
}
