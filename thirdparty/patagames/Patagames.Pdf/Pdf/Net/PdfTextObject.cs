// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfTextObject
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Exceptions;
using System;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>Represents a text object.</summary>
public class PdfTextObject : PdfPageObject
{
  private string _textUnicode;
  private string _textAscii;
  private PdfFont _font;

  /// <summary>
  /// Gets or sets the character spacing, which is a number expressed in unscaled text space units.
  /// </summary>
  public float CharSpacing
  {
    get => Pdfium.FPDFTextObj_GetCharSpacing(this.Handle);
    set
    {
      Pdfium.FPDFTextObj_SetCharSpacing(this.Handle, value);
      this.RecalcPositionData();
    }
  }

  /// <summary>
  /// Gets or sets the word spacing, which is a number expressed in unscaled text space units.
  /// </summary>
  public float WordSpacing
  {
    get => Pdfium.FPDFTextObj_GetWordSpacing(this.Handle);
    set
    {
      Pdfium.FPDFTextObj_SetWordSpacing(this.Handle, value);
      this.RecalcPositionData();
    }
  }

  /// <summary>Gets the number of characters from a text object.</summary>
  public int CharsCount => Pdfium.FPDFTextObj_CountChars(this.Handle);

  /// <summary>Gets item information</summary>
  /// <param name="index">The index of the item to get the information.</param>
  /// <param name="charCode">Gets an int value that represents the unicode character.</param>
  /// <param name="originX">Gets the horizontal position of the <paramref name="charCode" /> at the specified <paramref name="index" />.</param>
  /// <param name="originY">Gets the vertical position of the <paramref name="charCode" /> at the specified <paramref name="index" />.</param>
  public void GetItemInfo(int index, out int charCode, out float originX, out float originY)
  {
    Pdfium.FPDFTextObj_GetItemInfo(this.Handle, index, out charCode, out originX, out originY);
    this.Matrix.TransformPoint(ref originX, ref originY);
    if (this.Container == null || this.Container.Form == null)
      return;
    this.Container.Form.Matrix.TransformPoint(ref originX, ref originY);
  }

  /// <summary>
  /// Gets the unicode of a special character in a text object and kerning.
  /// </summary>
  /// <param name="index">The index of the character to get the unicode.</param>
  /// <param name="charCode">Gets an int value that represents the unicode character.</param>
  /// <param name="kerning">Pointer to a float value receiving the kerning</param>
  public void GetCharInfo(int index, out int charCode, out float kerning)
  {
    Pdfium.FPDFTextObj_GetCharInfo(this.Handle, index, out charCode, out kerning);
  }

  /// <summary>
  /// Gets the unicode of a special character in a text object and its placement.
  /// </summary>
  /// <param name="index">The index of the character to get.</param>
  /// <param name="charCode">Gets an int value that represents the unicode character.</param>
  /// <param name="originX">Gets the horizontal position of the <paramref name="charCode" /> at the specified <paramref name="index" />.</param>
  /// <param name="originY">Gets the vertical position of the <paramref name="charCode" /> at the specified <paramref name="index" />.</param>
  public void GetCharInfo(int index, out int charCode, out float originX, out float originY)
  {
    Pdfium.FPDFTextObj_GetCharInfo(this.Handle, index, out charCode, out originX, out originY);
    this.Matrix.TransformPoint(ref originX, ref originY);
    if (this.Container == null || this.Container.Form == null)
      return;
    this.Container.Form.Matrix.TransformPoint(ref originX, ref originY);
  }

  /// <summary>Gets the width of the specified character</summary>
  /// <param name="charCode">Character code the width of which is necessary to obtain.</param>
  /// <returns>The width of <paramref name="charCode" /> of the current text object.</returns>
  public float GetCharWidth(int charCode)
  {
    float width;
    Pdfium.FPDFTextObj_GetCharWidth(this.Handle, charCode, out width);
    this.Matrix.TransformXDistance(ref width);
    if (this.Container != null && this.Container.Form != null)
      this.Container.Form.Matrix.TransformXDistance(ref width);
    return width;
  }

  /// <summary>Gets the width of space character</summary>
  /// <returns>The width of space character from the font of the current text object.</returns>
  public float GetSpaceWidth()
  {
    float width;
    Pdfium.FPDFTextObj_GetSpaceCharWidth(this.Handle, out width);
    this.Matrix.TransformXDistance(ref width);
    if (this.Container != null && this.Container.Form != null)
      this.Container.Form.Matrix.TransformXDistance(ref width);
    return width;
  }

  /// <summary>Gets character bounding box</summary>
  /// <param name="index">The index of the character to get the bbox.</param>
  /// <returns>Character bounding box.</returns>
  public FS_RECTF GetCharRect(int index)
  {
    FS_RECTF rect;
    Pdfium.FPDFTextObj_GetCharRect(this.Handle, index, out rect.left, out rect.bottom, out rect.right, out rect.top, this.Matrix);
    if (this.Container != null && this.Container.Form != null)
      this.Container.Form.Matrix.TransformRect(ref rect);
    return rect;
  }

  /// <summary>Gets character bounding box</summary>
  /// <param name="index">The index of the character to get the bbox.</param>
  /// <param name="matrix">The matrix that should be applied to the output rectangle. Typically it's <see cref="P:Patagames.Pdf.Net.PdfTextObject.TextMatrix" /> </param>
  /// <returns>Character bounding box.</returns>
  public FS_RECTF GetCharRect(int index, FS_MATRIX matrix)
  {
    FS_RECTF charRect;
    Pdfium.FPDFTextObj_GetCharRect(this.Handle, index, out charRect.left, out charRect.bottom, out charRect.right, out charRect.top, matrix);
    return charRect;
  }

  /// <summary>
  /// Get the offsets to start and end of each character in text object
  /// </summary>
  /// <returns>An array of character offsets.</returns>
  public float[] CalcCharPos()
  {
    float[] pPosArray = new float[this.CharsCount * 2];
    Pdfium.FPDFTextObj_CalcCharPos(this.Handle, pPosArray);
    for (int index = 0; index < pPosArray.Length; ++index)
    {
      this.Matrix.TransformXDistance(ref pPosArray[index]);
      if (this.Container != null && this.Container.Form != null)
        this.Container.Form.Matrix.TransformXDistance(ref pPosArray[index]);
    }
    return pPosArray;
  }

  /// <summary>Recalculate charactes positions</summary>
  public void RecalcPositionData() => Pdfium.FPDFTextObj_RecalcPositionData(this.Handle);

  /// <summary>Gets or sets the Font of a text object.</summary>
  public PdfFont Font
  {
    get
    {
      if (this._font == null)
      {
        IntPtr font = Pdfium.FPDFTextObj_GetFont(this.Handle);
        this._font = !(font == IntPtr.Zero) ? new PdfFont(font) : throw new FontNotFoundException();
      }
      return this._font;
    }
    set
    {
      if (this._font == value)
        return;
      if (value == null)
        throw new ArgumentNullException();
      Pdfium.FPDFTextObj_SetFont(this.Handle, value.Handle);
      this._font = value;
    }
  }

  /// <summary>Gets or sets font size.</summary>
  public float FontSize
  {
    get
    {
      float size;
      Pdfium.FPDFTextObj_GetFontSize(this.Handle, out size);
      return size;
    }
    set
    {
      Pdfium.FPDFTextObj_SetFontSize(this.Handle, value);
      this.RecalcPositionData();
    }
  }

  /// <summary>Gets or sets the text rendering mode.</summary>
  public TextRenderingModes RenderMode
  {
    get => Pdfium.FPDFTextObj_GetRenderMode(this.Handle);
    set => Pdfium.FPDFTextObj_SetRenderMode(this.Handle, value);
  }

  /// <summary>
  /// Gets or sets the coordinates of the bottom-left corner of the text object relative to the down-left corner of its page.
  /// </summary>
  public FS_POINTF Location
  {
    get
    {
      float x;
      float y;
      Pdfium.FPDFTextObj_GetPos(this.Handle, out x, out y);
      if (this.Container != null && this.Container.Form != null)
        this.Container.Form.Matrix.TransformPoint(ref x, ref y);
      return new FS_POINTF(x, y);
    }
    set
    {
      float x = value.X;
      float y = value.Y;
      if (this.Container != null && this.Container.Form != null)
      {
        FS_MATRIX fsMatrix = new FS_MATRIX();
        fsMatrix.SetReverse(this.Container.Form.Matrix);
        fsMatrix.TransformPoint(ref x, ref y);
      }
      Pdfium.FPDFTextObj_SetPosition(this.Handle, x, y);
    }
  }

  /// <summary>
  /// This property is obsolete. Please use <see cref="P:Patagames.Pdf.Net.PdfPageObject.Matrix" /> property instead.
  /// </summary>
  [Obsolete("This property is obsolete. Please use Matrix property instead.", false)]
  public FS_MATRIX TextMatrix => this.Matrix;

  /// <summary>Gets or sets unicode string from text object</summary>
  public string TextUnicode
  {
    get
    {
      if (this._textUnicode == null)
        this._textUnicode = Pdfium.FPDFTextObj_GetTextUnicode(this.Handle);
      return this._textUnicode;
    }
    set
    {
      if (!(this._textUnicode != value))
        return;
      if (string.IsNullOrEmpty(value))
      {
        Pdfium.FPDFTextObj_SetEmpty(this.Handle);
      }
      else
      {
        this.AppendChars(value);
        Pdfium.FPDFTextObj_SetTextUnicode(this.Handle, value);
      }
      this._textUnicode = (string) null;
    }
  }

  /// <summary>Gets/sets ANSI string from/to text object</summary>
  public string TextAnsi
  {
    get
    {
      if (this._textAscii == null)
        this._textAscii = Pdfium.FPDFTextObj_GetText(this.Handle, PdfCommon.DefaultAnsiEncoding);
      return this._textAscii;
    }
    set
    {
      if (!(this._textAscii != value))
        return;
      if (string.IsNullOrEmpty(value))
        Pdfium.FPDFTextObj_SetEmpty(this.Handle);
      else
        Pdfium.FPDFTextObj_SetText(this.Handle, value, PdfCommon.DefaultAnsiEncoding);
      this._textAscii = (string) null;
    }
  }

  /// <summary>Gets/sets ANSI string from/to text object</summary>
  [Obsolete("This property is obsolete. Please use TextAnsi instead", false)]
  public string TextAscii
  {
    get => this.TextAnsi;
    set => this.TextAnsi = value;
  }

  /// <summary>
  /// Gets or sets the text knockout flag, which determines the behavior of overlapping glyphs within a text object in the transparent imaging model.
  /// </summary>
  /// <remarks>
  /// If its value is false, each glyph in a text object is treated as a separate elementary object; when glyphs overlap, they composite with one another.
  /// If the parameter is true, all glyphs in the text object are treated together as a single elementary object; when glyphs overlap, later glyphs overwrite(“knock out”) earlier ones in the area of overlap.
  /// </remarks>
  public bool TextKnockout
  {
    get => Pdfium.FPDFPageObj_GetTextKnockoutFlag(this.Handle);
    set => Pdfium.FPDFPageObj_SetTextKnockoutFlag(this.Handle, value);
  }

  internal PdfTextObject(IntPtr pathHandle)
    : base(pathHandle)
  {
  }

  /// <summary>Create new instance of PdfTextObject class</summary>
  /// <returns>New instance of PdfTextObject</returns>
  [Obsolete("This method is obsolete. Please use Create(string, float, float, font, fontsize) instead")]
  public static PdfTextObject Create()
  {
    IntPtr pathHandle = PdfPageObject.CreateObject(PageObjectTypes.PDFPAGE_TEXT);
    return pathHandle == IntPtr.Zero ? (PdfTextObject) null : new PdfTextObject(pathHandle);
  }

  /// <summary>Create new instance of PdfTextObject class</summary>
  /// <param name="text">Sets unicode string to text object</param>
  /// <param name="x">Sets the horizontal coordinate of the bottom-left corner of the text object relative to the down-left corner of its page.</param>
  /// <param name="y">Sets the vertical coordinate of the bottom-left corner of the text object relative to the down-left corner of its page.</param>
  /// <param name="font">Sets the Font of a text object.</param>
  /// <param name="fontsize">Sets font size.</param>
  /// <returns>New instance of PdfTextObject</returns>
  public static PdfTextObject Create(string text, float x, float y, PdfFont font, float fontsize)
  {
    IntPtr pathHandle = PdfPageObject.CreateObject(PageObjectTypes.PDFPAGE_TEXT);
    if (pathHandle == IntPtr.Zero)
      return (PdfTextObject) null;
    return new PdfTextObject(pathHandle)
    {
      Font = font,
      FontSize = fontsize,
      Location = new FS_POINTF(x, y),
      TextUnicode = text
    };
  }

  private void AppendChars(string text)
  {
    if (this.Font == null || this.Font.FontType != FontTypes.PDFFONT_CIDFONT || !Pdfium.IsFullAPI || !this.Font.Dictionary.ContainsKey("Encoding") || !this.Font.Dictionary["Encoding"].Is<PdfTypeName>())
      return;
    string str = this.Font.Dictionary["Encoding"].As<PdfTypeName>().Value;
    if (str != "Identity-V" && str != "Identity-H")
      return;
    foreach (uint unicode in text)
    {
      uint cmap = Pdfium.FPDFFont_AddToCMap(this.Font.Handle, unicode);
      if (cmap > 0U)
        Pdfium.FPDFFont_AddToWidths(this.Font.Handle, cmap);
    }
  }
}
