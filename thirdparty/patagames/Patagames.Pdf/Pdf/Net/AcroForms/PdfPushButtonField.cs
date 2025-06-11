// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.AcroForms.PdfPushButtonField
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using System;

#nullable disable
namespace Patagames.Pdf.Net.AcroForms;

/// <summary>
/// Represents a pushbutton that is a purely interactive control that responds immediately to user input without retaining a permanent value.
/// </summary>
public class PdfPushButtonField : PdfButtonField
{
  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.AcroForms.PdfPushButtonField" /> class.
  /// </summary>
  /// <param name="forms">Interactive forms.</param>
  /// <param name="handle">The field's handle.</param>
  internal PdfPushButtonField(PdfInteractiveForms forms, IntPtr handle)
    : base(forms, handle)
  {
  }

  /// <summary>
  /// Create new Pushbutton field and add it into interactive forms.
  /// </summary>
  /// <param name="forms">Interactive forms.</param>
  /// <param name="name">The partial field name. Cannot contain a period.</param>
  /// <param name="parent">The parent field. Only non-terminal fields are accepted.</param>
  /// <param name="defFont">Default font used for all controls assigned with this field.</param>
  /// <param name="fontSize">Default font's size.</param>
  /// <param name="captionColor">The default button caption color used when the control does not specify a title color.</param>
  public PdfPushButtonField(
    PdfInteractiveForms forms,
    string name = null,
    PdfField parent = null,
    PdfFont defFont = null,
    float fontSize = 0.0f,
    FS_COLOR? captionColor = null)
    : base(forms, name, parent, defFont, fontSize, captionColor)
  {
    this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create(65536 /*0x010000*/);
  }
}
