// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.FieldFlagsEx
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
public enum FieldFlagsEx
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
  /// <remarks>Applied to radio buttons only.</remarks>
  NoToggleToOff = 16384, // 0x00004000
  /// <summary>
  /// If set, the field is a set of radio buttons; if clear, the field is a check box. This flag is meaningful only if the Pushbutton flag is clear.
  /// </summary>
  /// <remarks>Applied to radio buttons only.</remarks>
  Radio = 32768, // 0x00008000
  /// <summary>
  /// If set, the field is a pushbutton that does not retain a permanent value.
  /// </summary>
  /// <remarks>Applied to push buttons only.</remarks>
  Pushbutton = 65536, // 0x00010000
  /// <summary>
  /// If set, a group of radio buttons within a radio button field that use the same value for the on state will turn on and off in unison; that is if one is checked, they are all checked. If clear, the buttons are mutually exclusive (the same behavior as HTML radio buttons).
  /// </summary>
  /// <remarks>Applied to radio buttons only.</remarks>
  RadiosInUnison = 33554432, // 0x02000000
  /// <summary>
  /// If set, the field can contain multiple lines of text; if clear, the field’s text is restricted to a single line.
  /// </summary>
  /// <remarks>Applied to text fields only.</remarks>
  Multiline = 4096, // 0x00001000
  /// <summary>
  /// If set, the field is intended for entering a secure password that should not be echoed visibly to the screen.
  /// </summary>
  /// <remarks>
  /// Characters typed from the keyboard should instead be echoed in some unreadable form, such as asterisks or bullet characters.
  /// <para>To protect password confidentiality, viewer applications should neverstore the value of the text field in the PDF file if this flag is set.</para>
  /// </remarks>
  /// <remarks>Applied to text fields only.</remarks>
  Password = 8192, // 0x00002000
  /// <summary>
  /// If set, the text entered in the field represents the pathname of a file whose contents are to be submitted as the value of the field.
  /// </summary>
  /// <remarks>Applied to text fields only.</remarks>
  FileSelect = 1048576, // 0x00100000
  /// <summary>
  /// If set, text entered in the field is not spell-checked; For combobox fields this flag is meaningful only if the Combo and Edit flags are both set.
  /// </summary>
  /// <remarks>Applied to text and combobox fields.</remarks>
  DoNotSpellCheck = 4194304, // 0x00400000
  /// <summary>
  /// If set, the field does not scroll (horizontally for single-line fields, vertically for multiple-line fields) to accommodate more text than fits within its annotation rectangle. Once the field is full, no further text is accepted.
  /// </summary>
  /// <remarks>Applied to text fields only.</remarks>
  DoNotScroll = 8388608, // 0x00800000
  /// <summary>
  /// Meaningful only if the MaxLen entry is present in the text field dictionary and if the Multiline, Password, and FileSelect flags are clear.
  /// If set, the field is automatically divided into as many equally spaced positions, or combs, as the value of MaxLen, and the text is laid out into those combs.
  /// </summary>
  /// <remarks>Applied to text fields only.</remarks>
  Comb = 16777216, // 0x01000000
  /// <summary>
  /// If set, the value of this field should be represented as a rich text string.
  /// If the field has a value, the RV entry of the field dictionary specifies the rich text string.
  /// </summary>
  /// <remarks>Applied to text fields only.</remarks>
  RichText = RadiosInUnison, // 0x02000000
  /// <summary>
  /// If set, the field is a combo box; if clear, the field is a list box.
  /// </summary>
  /// <remarks>Applied to combobox fields only.</remarks>
  Combo = 131072, // 0x00020000
  /// <summary>
  /// If set, the combo box includes an editable text box as well as a dropdown list; if clear, it includes only a drop-down list. This flag is meaningful only if the Combo flag is set.
  /// </summary>
  /// <remarks>Applied to combobox fields only.</remarks>
  Edit = 262144, // 0x00040000
  /// <summary>
  /// If set, the field’s option items should be sorted alphabetically. This flag is intended for use by form authoring tools,
  /// not by PDF viewer applications. Viewers should simply display the options in the order in which they occur in the Opt array.
  /// </summary>
  /// <remarks>Applied to combobox and listbox fields.</remarks>
  Sort = 524288, // 0x00080000
  /// <summary>
  /// If set, more than one of the field’s option items may be selected simultaneously; if clear, no more than one item at a time may be selected.
  /// </summary>
  /// <remarks>Applied to combobox and listbox fields.</remarks>
  MultiSelect = 2097152, // 0x00200000
  /// <summary>
  /// If set, the new value is committed as soon as a selection is made with the pointing device.
  /// This option enables applications to perform an action once a selection is made, without requiring the user
  /// to exit the field. If clear, the new value is not committed until the user exits the field.
  /// </summary>
  /// <remarks>Applied to all field types.</remarks>
  CommitOnSelChange = 67108864, // 0x04000000
}
