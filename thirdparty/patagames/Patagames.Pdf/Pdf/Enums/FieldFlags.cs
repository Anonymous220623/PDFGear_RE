// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.FieldFlags
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>
///  Field's flags specifying various characteristics of the field.
/// </summary>
[Flags]
public enum FieldFlags
{
  /// <summary>
  /// If set, the user may not change the value of the field. Any associated widget
  /// annotations will not interact with the user; that is, they will not respond to
  /// mouse clicks or change their appearance in response to mouse motions. This
  /// flag is useful for fields whose values are computed or imported from a database.
  /// </summary>
  /// <remarks>Applied to all field types.</remarks>
  ReadOnly = 1,
  /// <summary>
  /// If set, the field must have a value at the time it is exported by a submit-form action.
  /// </summary>
  /// <remarks>Applied to all field types.</remarks>
  Required = 2,
  /// <summary>
  /// If set, the field must not be exported by a submit-form action
  /// </summary>
  /// <remarks>Applied to all field types.</remarks>
  NoExport = 4,
  /// <summary>
  /// If set, exactly one radio button must be selected at all times; clicking the currently selected button has no effect. If clear, clicking the selected button deselects it, leaving no button selected.
  /// </summary>
  /// <remarks>Applied to radio buttons only</remarks>
  NoToggleToOff = 256, // 0x00000100
  /// <summary>
  /// Radio buttons(PDF 1.5) If set, a group of radio buttons within a radio button field that use the same value for the on state will turn on and off in unison; that is if one is checked, they are all checked. If clear, the buttons are mutually exclusive (the same behavior as HTML radio buttons).
  /// </summary>
  /// <remarks>Applied to radio buttons</remarks>
  RadiosInUnison = 512, // 0x00000200
  /// <summary>
  /// If set, the field can contain multiple lines of text; if clear, the field’s text is restricted to a single line.
  /// </summary>
  /// <remarks>Applied to text fields</remarks>
  Multiline = NoToggleToOff, // 0x00000100
  /// <summary>
  /// If set, the field is intended for entering a secure password that should not be echoed visibly to the screen.
  /// </summary>
  /// <remarks>
  /// Characters typed from the keyboard should instead be echoed in some unreadable form, such as asterisks or bullet characters.
  /// <para>To protect password confidentiality, viewer applications should neverstore the value of the text field in the PDF file if this flag is set.</para>
  /// </remarks>
  /// <remarks>Applied to text fields</remarks>
  Password = RadiosInUnison, // 0x00000200
  /// <summary>
  /// If set, the field does not scroll (horizontally for single-line fields, vertically for multiple-line fields) to accommodate more text than fits within its annotation rectangle. Once the field is full, no further text is accepted.
  /// </summary>
  /// <remarks>(PDF 1.4) Applied to text fields</remarks>
  DoNotScroll = 1024, // 0x00000400
  /// <summary>
  /// Meaningful only if the MaxLen entry is present in the text field dictionary and if the Multiline, Password, and FileSelect flags are clear. If set, the field is automatically divided into as many equally spaced positions, or combs, as the value of MaxLen, and the text is laid out into those combs.
  /// </summary>
  /// <remarks>(PDF 1.5) Applied to text fields</remarks>
  Comb = 2048, // 0x00000800
  /// <summary>
  /// If set, the combo box includes an editable text box as well as a dropdown list; if clear, it includes only a drop-down list. This flag is meaningful only if the Combo flag is set.
  /// </summary>
  /// <remarks>Applied to combo box</remarks>
  Edit = Multiline, // 0x00000100
  /// <summary>
  /// If set, more than one of the field’s option items may be selected simultaneously; if clear, no more than one item at a time may be selected.
  /// </summary>
  /// <remarks>(PDF 1.4) Applied to list</remarks>
  MultiSelect = Edit, // 0x00000100
}
