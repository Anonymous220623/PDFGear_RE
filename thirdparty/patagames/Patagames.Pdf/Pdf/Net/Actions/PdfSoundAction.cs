// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Actions.PdfSoundAction
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Wrappers;
using Patagames.Pdf.Properties;
using System;

#nullable disable
namespace Patagames.Pdf.Net.Actions;

/// <summary>
/// Represents a sound action. A sound action plays a sound through the computer’s speakers.
/// </summary>
public class PdfSoundAction : PdfAction
{
  private PdfSound _sound;
  private PdfIndirectList _list;

  /// <summary>
  /// Gets or sets a sound object defining the sound to be played.
  /// </summary>
  public PdfSound Sound
  {
    get
    {
      if (!this.Dictionary.ContainsKey(nameof (Sound)))
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
        if (value.Stream.ObjectNumber == 0 && Pdfium.FPDFOBJ_GetParentObj(value.Stream.Handle) != IntPtr.Zero)
          throw new ArgumentException(string.Format(Error.err0067, (object) "sound", (object) "object"));
        this._list.Add((PdfTypeBase) value.Stream);
        this.Dictionary.SetIndirectAt(nameof (Sound), this._list, (PdfTypeBase) value.Stream);
      }
      this._sound = value;
    }
  }

  /// <summary>
  /// Gets or sets the volume at which to play the sound, in the range −1.0 to 1.0;
  /// </summary>
  public float Volume
  {
    get
    {
      return !this.Dictionary.ContainsKey(nameof (Volume)) || !this.Dictionary[nameof (Volume)].Is<PdfTypeNumber>() ? 1f : this.Dictionary[nameof (Volume)].As<PdfTypeNumber>().FloatValue;
    }
    set
    {
      this.Dictionary[nameof (Volume)] = (PdfTypeBase) PdfTypeNumber.Create((double) value < -1.0 ? -1f : ((double) value > 1.0 ? 1f : value));
    }
  }

  /// <summary>
  /// Gets or sets a flag specifying whether to play the sound synchronously or asynchronously.
  /// </summary>
  /// <remarks>If this flag is true, the viewer application retains control, allowing no further user interaction other than canceling the sound, until the sound has been completely played. </remarks>
  public bool Synchronous
  {
    get
    {
      return this.Dictionary.ContainsKey(nameof (Synchronous)) && this.Dictionary[nameof (Synchronous)].Is<PdfTypeBoolean>() && this.Dictionary[nameof (Synchronous)].As<PdfTypeBoolean>().Value;
    }
    set => this.Dictionary[nameof (Synchronous)] = (PdfTypeBase) PdfTypeBoolean.Create(value);
  }

  /// <summary>
  /// Gets or sets a flag specifying whether to repeat the sound indefinitely.
  /// If this property is set to true, the <see cref="P:Patagames.Pdf.Net.Actions.PdfSoundAction.Synchronous" /> property is ignored.
  /// </summary>
  public bool Repeat
  {
    get
    {
      return this.Dictionary.ContainsKey(nameof (Repeat)) && this.Dictionary[nameof (Repeat)].Is<PdfTypeBoolean>() && this.Dictionary[nameof (Repeat)].As<PdfTypeBoolean>().Value;
    }
    set => this.Dictionary[nameof (Repeat)] = (PdfTypeBase) PdfTypeBoolean.Create(value);
  }

  /// <summary>
  /// Gets or sets a flag specifying whether to mix this sound with any other sound already playing.
  /// </summary>
  /// <remarks>If this flag is false, any previously playing sound is stopped before starting this sound; this can be used to stop a repeating sound.</remarks>
  public bool Mix
  {
    get
    {
      return this.Dictionary.ContainsKey(nameof (Mix)) && this.Dictionary[nameof (Mix)].Is<PdfTypeBoolean>() && this.Dictionary[nameof (Mix)].As<PdfTypeBoolean>().Value;
    }
    set => this.Dictionary[nameof (Mix)] = (PdfTypeBase) PdfTypeBoolean.Create(value);
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Actions.PdfSoundAction" /> class with the document and the destination.
  /// </summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="sound">A sound object defining the sound to be played.</param>
  public PdfSoundAction(PdfDocument document, PdfSound sound)
    : base(document, PdfTypeDictionary.Create().Handle)
  {
    if ((PdfWrapper) sound == (PdfWrapper) null)
      throw new ArgumentNullException(nameof (sound));
    this._list = PdfIndirectList.FromPdfDocument(this.Document);
    this.Dictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("Action");
    this.Dictionary["S"] = (PdfTypeBase) PdfTypeName.Create(nameof (Sound));
    this.Sound = sound;
  }

  /// <summary>Initializes a new instance of the PdfAction class.</summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="handle">Pdfium SDK handle that the action is bound to</param>
  public PdfSoundAction(PdfDocument document, IntPtr handle)
    : base(document, handle)
  {
    this._list = PdfIndirectList.FromPdfDocument(this.Document);
  }
}
