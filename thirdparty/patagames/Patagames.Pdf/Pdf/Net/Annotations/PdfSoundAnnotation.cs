// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Annotations.PdfSoundAnnotation
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Wrappers;
using Patagames.Pdf.Properties;
using System;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net.Annotations;

/// <summary>
/// A sound annotation (PDF 1.2) is analogous to a text annotation except that instead of a text note,
/// it contains sound recorded from the computer’s microphone
/// or imported from a file. When the annotation is activated, the sound is played.
/// The annotation behaves like a text annotation in most ways, with a different icon
/// (by default, a speaker) to indicate that it represents a sound.
/// </summary>
public class PdfSoundAnnotation : PdfMarkupAnnotation
{
  private PdfSound _sound;

  /// <summary>
  /// The name of an icon to be used in displaying the annotation.
  /// </summary>
  /// <remarks>
  /// Additional names may be supported as well. In this case please use <see cref="P:Patagames.Pdf.Net.Annotations.PdfSoundAnnotation.ExtendedIconName" /> property.
  /// <note type="note">The annotation dictionary’s AP entry, if present, takes precedence over
  /// the StandardIconName and <see cref="P:Patagames.Pdf.Net.Annotations.PdfSoundAnnotation.ExtendedIconName" /> properties.</note>
  /// <para>Default value: <strong>Speaker</strong>.</para>
  /// </remarks>
  public SoundIconNames StandardIconName
  {
    get
    {
      if (!this.IsExists("Name"))
        return SoundIconNames.Speaker;
      SoundIconNames result;
      return Pdfium.GetEnumDescription<SoundIconNames>(this.Dictionary["Name"].As<PdfTypeName>().Value, out result) ? result : SoundIconNames.Extended;
    }
    set
    {
      string initialVal = value != SoundIconNames.Extended ? Pdfium.GetEnumDescription((Enum) value) : throw new ArgumentException(Error.err0043);
      this.Dictionary["Name"] = !((initialVal ?? "").Trim() == "") ? (PdfTypeBase) PdfTypeName.Create(initialVal) : throw new ArgumentException(string.Format(Error.err0047, (object) nameof (StandardIconName), (object) "is one of the SoundIconNames enumerator"));
    }
  }

  /// <summary>
  /// The name of an icon to be used in displaying the annotation.
  /// </summary>
  public string ExtendedIconName
  {
    get => !this.IsExists("Name") ? (string) null : this.Dictionary["Name"].As<PdfTypeName>().Value;
    set
    {
      if (value == null && this.Dictionary.ContainsKey("Name"))
      {
        this.Dictionary.Remove("Name");
      }
      else
      {
        if (value == null)
          return;
        this.Dictionary["Name"] = (PdfTypeBase) PdfTypeName.Create(value);
      }
    }
  }

  /// <summary>
  /// Gets or sets a sound object defining the sound to be played when the annotation is activated.
  /// </summary>
  public PdfSound Sound
  {
    get
    {
      if (!this.IsExists(nameof (Sound)))
        return (PdfSound) null;
      if ((PdfWrapper) this._sound == (PdfWrapper) null || this._sound.Stream.IsDisposed || this._sound.Dictionary.IsDisposed)
        this._sound = new PdfSound(this.Dictionary[nameof (Sound)].As<PdfTypeStream>());
      return this._sound;
    }
    set
    {
      if ((PdfWrapper) value == (PdfWrapper) null && this.Dictionary.ContainsKey(nameof (Sound)))
        this.Dictionary.Remove(nameof (Sound));
      else if ((PdfWrapper) value != (PdfWrapper) null)
      {
        this.ListOfIndirectObjects.Add((PdfTypeBase) value.Stream);
        this.Dictionary.SetIndirectAt(nameof (Sound), this.ListOfIndirectObjects, (PdfTypeBase) value.Stream);
      }
      this._sound = value;
    }
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfSoundAnnotation" />.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  public PdfSoundAnnotation(PdfPage page)
    : base(page)
  {
    this.Dictionary["Subtype"] = (PdfTypeBase) PdfTypeName.Create(nameof (Sound));
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfSoundAnnotation" /> with specified parameters.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="icon">The name of a standard icon to be used in displaying the annotation.</param>
  /// <param name="color">Annotation color.</param>
  /// <param name="x">The x-coordinate of this annotation.</param>
  /// <param name="y">The y-coordinate of this annotation.</param>
  /// <param name="sampleData">An array of sample values.</param>
  /// <param name="encoding">The encoding format for the sample data.</param>
  /// <param name="bps">The number of bits per sample value per channel.</param>
  /// <param name="rate">The sampling rate, in samples per second.</param>
  /// <param name="channels">The number of sound channels.</param>
  /// <remarks>
  /// For a description of the sampleData format, see the remarks section of <see cref="T:Patagames.Pdf.Net.Wrappers.PdfSound" /> class.
  /// </remarks>
  public PdfSoundAnnotation(
    PdfPage page,
    SoundIconNames icon,
    FS_COLOR color,
    float x,
    float y,
    byte[] sampleData,
    SoundEncodingFormats encoding,
    int bps,
    int rate,
    int channels)
    : this(page, icon, color, x, y, new PdfSound(sampleData, encoding, bps, rate, channels))
  {
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfSoundAnnotation" /> with specified parameters.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="icon">The name of a standard icon to be used in displaying the annotation.</param>
  /// <param name="x">The x-coordinate of this annotation.</param>
  /// <param name="y">The y-coordinate of this annotation.</param>
  /// <param name="sampleData">An array of sample values.</param>
  /// <param name="encoding">The encoding format for the sample data.</param>
  /// <param name="bps">The number of bits per sample value per channel.</param>
  /// <param name="rate">The sampling rate, in samples per second.</param>
  /// <param name="channels">The number of sound channels.</param>
  /// <remarks>
  /// For a description of the sampleData format, see the remarks section of <see cref="T:Patagames.Pdf.Net.Wrappers.PdfSound" /> class.
  /// </remarks>
  public PdfSoundAnnotation(
    PdfPage page,
    SoundIconNames icon,
    float x,
    float y,
    byte[] sampleData,
    SoundEncodingFormats encoding,
    int bps,
    int rate,
    int channels)
    : this(page, icon, new FS_COLOR((int) byte.MaxValue, 0, 192 /*0xC0*/, (int) byte.MaxValue), x, y, sampleData, encoding, bps, rate, channels)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Annotations.PdfScreenAnnotation" /> class based on the specified dictionary.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="dictionary">The annotation dictionary or indirect dictionary.</param>
  public PdfSoundAnnotation(PdfPage page, PdfTypeBase dictionary)
    : base(page, dictionary)
  {
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfSoundAnnotation" /> with specified parameters.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="icon">The name of a standard icon to be used in displaying the annotation.</param>
  /// <param name="color">Annotation color.</param>
  /// <param name="x">The x-coordinate of this annotation.</param>
  /// <param name="y">The y-coordinate of this annotation.</param>
  /// <param name="sound">An instance of <see cref="T:Patagames.Pdf.Net.Wrappers.PdfSound" /> contained sampled data.</param>
  public PdfSoundAnnotation(
    PdfPage page,
    SoundIconNames icon,
    FS_COLOR color,
    float x,
    float y,
    PdfSound sound)
    : this(page)
  {
    this.Color = color;
    this.Opacity = (float) color.A / (float) byte.MaxValue;
    this.Flags = AnnotationFlags.Print | AnnotationFlags.NoZoom | AnnotationFlags.NoRotate;
    if (icon == SoundIconNames.Extended)
      this.ExtendedIconName = "Ear";
    else
      this.StandardIconName = icon;
    this.Sound = sound;
    this.Rectangle = new FS_RECTF(x, y + 20f, x + 20f, y);
    this.RegenerateAppearances();
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfSoundAnnotation" /> with specified parameters.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="icon">The name of a standard icon to be used in displaying the annotation.</param>
  /// <param name="x">The x-coordinate of this annotation.</param>
  /// <param name="y">The y-coordinate of this annotation.</param>
  /// <param name="sound">An instance of <see cref="T:Patagames.Pdf.Net.Wrappers.PdfSound" /> contained sampled data.</param>
  public PdfSoundAnnotation(PdfPage page, SoundIconNames icon, float x, float y, PdfSound sound)
    : this(page, icon, new FS_COLOR((int) byte.MaxValue, 0, 192 /*0xC0*/, (int) byte.MaxValue), x, y, sound)
  {
  }

  /// <summary>
  /// Re-creates the appearance of the annotation based on its properties.
  /// </summary>
  /// <remarks>
  /// When the annotation does not have an Appearance stream (old style of annotations),
  /// they are not rendered by the Pdfium engine.
  /// Calling this function creates an appearance stream based on the default parameters and the properties of this annotation.
  /// </remarks>
  public override void RegenerateAppearances()
  {
    this.CreateEmptyAppearance(AppearanceStreamModes.Normal);
    float left = this.Rectangle.left;
    float bottom = this.Rectangle.bottom;
    FS_COLOR fillColor = new FS_COLOR(this.Opacity, this.Color);
    FS_COLOR strokeColor = new FS_COLOR(this.Opacity, 0.0f, 0.0f, 0.0f);
    List<PdfPathObject> pdfPathObjectList = (List<PdfPathObject>) null;
    switch (this.StandardIconName)
    {
      case SoundIconNames.Speaker:
        pdfPathObjectList = AnnotDrawing.CreateSpeaker(fillColor, strokeColor);
        break;
      case SoundIconNames.Mic:
        pdfPathObjectList = AnnotDrawing.CreateMic(fillColor, strokeColor);
        break;
      case SoundIconNames.Extended:
        pdfPathObjectList = AnnotDrawing.CreateEar(fillColor, strokeColor);
        break;
    }
    if (pdfPathObjectList != null)
    {
      foreach (PdfPageObject pdfPageObject in pdfPathObjectList)
        this.NormalAppearance.Add(pdfPageObject);
    }
    this.GenerateAppearance(AppearanceStreamModes.Normal);
    PdfTypeStream.Create(Pdfium.FPDFAnnot_GetAppearanceStream(this.Dictionary.Handle, AppearanceStreamModes.Normal));
    double l = (double) left;
    double num1 = (double) bottom;
    FS_RECTF rectangle = this.Rectangle;
    double height = (double) rectangle.Height;
    double t = num1 + height;
    double num2 = (double) left;
    rectangle = this.Rectangle;
    double width = (double) rectangle.Width;
    double r = num2 + width;
    double b = (double) bottom;
    this.Rectangle = new FS_RECTF((float) l, (float) t, (float) r, (float) b);
  }
}
