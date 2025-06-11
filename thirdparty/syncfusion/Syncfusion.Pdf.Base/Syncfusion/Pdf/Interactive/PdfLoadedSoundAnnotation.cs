// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfLoadedSoundAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfLoadedSoundAnnotation : PdfLoadedStyledAnnotation
{
  private PdfCrossTable m_crossTable;
  private PdfSound m_sound;
  private PdfDictionary m_dictionary;
  private PdfSoundIcon m_icon;
  private PdfAppearance m_appearance;

  public PdfLoadedPopupAnnotationCollection ReviewHistory
  {
    get
    {
      if (this.m_reviewHistory == null)
        this.m_reviewHistory = new PdfLoadedPopupAnnotationCollection(this.Page, this.Dictionary, true);
      return this.m_reviewHistory;
    }
  }

  public PdfLoadedPopupAnnotationCollection Comments
  {
    get
    {
      if (this.m_comments == null)
        this.m_comments = new PdfLoadedPopupAnnotationCollection(this.Page, this.Dictionary, false);
      return this.m_comments;
    }
  }

  public PdfSound Sound
  {
    get => this.ObtainSound();
    set
    {
      this.m_sound = value;
      this.Dictionary.Remove(nameof (Sound));
      this.Dictionary.SetProperty(nameof (Sound), (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_sound));
      this.Dictionary.Modify();
    }
  }

  public string FileName => this.ObtainFileName();

  public PdfSoundIcon Icon
  {
    get => this.ObtainIcon();
    set
    {
      this.m_icon = value;
      this.Dictionary.SetName("Name", this.m_icon.ToString());
    }
  }

  private string ObtainFileName()
  {
    string empty = string.Empty;
    if (this.Dictionary.ContainsKey("Sound"))
      empty = ((this.m_crossTable.GetObject(this.Dictionary["Sound"]) as PdfDictionary)["T"] as PdfString).Value.ToString();
    return empty;
  }

  private PdfSoundIcon ObtainIcon()
  {
    PdfSoundIcon icon = PdfSoundIcon.Speaker;
    if (this.Dictionary.ContainsKey("Name"))
      icon = this.GetIconName((this.Dictionary["Name"] as PdfName).Value.ToString());
    return icon;
  }

  private PdfSoundIcon GetIconName(string iType)
  {
    PdfSoundIcon iconName = PdfSoundIcon.Speaker;
    switch (iType)
    {
      case "Mic":
        iconName = PdfSoundIcon.Mic;
        break;
      case "Speaker":
        iconName = PdfSoundIcon.Speaker;
        break;
    }
    return iconName;
  }

  private PdfSound ObtainSound()
  {
    PdfSound sound = new PdfSound(this.ObtainFileName());
    if (this.Dictionary.ContainsKey("Sound"))
    {
      PdfDictionary pdfDictionary = this.m_crossTable.GetObject(this.Dictionary["Sound"]) as PdfDictionary;
      if (pdfDictionary.ContainsKey("B"))
        sound.Bits = (pdfDictionary["B"] as PdfNumber).IntValue;
      if (pdfDictionary.ContainsKey("R"))
        sound.Rate = (pdfDictionary["R"] as PdfNumber).IntValue;
      if (pdfDictionary.ContainsKey("C"))
        sound.Channels = (pdfDictionary["C"] as PdfNumber).IntValue != 1 ? PdfSoundChannels.Stereo : PdfSoundChannels.Mono;
      if (pdfDictionary.ContainsKey("E"))
      {
        PdfName pdfName = pdfDictionary["E"] as PdfName;
        sound.Encoding = this.GetEncodigType(pdfName.Value.ToString());
      }
    }
    return sound;
  }

  private PdfSoundEncoding GetEncodigType(string eType)
  {
    PdfSoundEncoding encodigType = PdfSoundEncoding.Raw;
    switch (eType)
    {
      case "Raw":
        encodigType = PdfSoundEncoding.Raw;
        break;
      case "Signed":
        encodigType = PdfSoundEncoding.Signed;
        break;
      case "MuLaw":
        encodigType = PdfSoundEncoding.MuLaw;
        break;
      case "ALaw":
        encodigType = PdfSoundEncoding.ALaw;
        break;
    }
    return encodigType;
  }

  internal PdfLoadedSoundAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rectangle)
    : base(dictionary, crossTable)
  {
    if (PdfCrossTable.Dereference((PdfCrossTable.Dereference(dictionary[nameof (Sound)]) as PdfDictionary)["T"]) is PdfString pdfString)
    {
      string fileName = pdfString.Value;
      PdfReferenceHolder pdfReferenceHolder = dictionary[nameof (Sound)] as PdfReferenceHolder;
      if (pdfReferenceHolder == (PdfReferenceHolder) null)
        throw new ArgumentNullException();
      byte[] data = (pdfReferenceHolder.Object as PdfStream).Data;
      this.m_dictionary = dictionary;
      this.m_crossTable = crossTable;
      this.m_sound = new PdfSound(fileName, true);
    }
    else
    {
      this.m_dictionary = !(dictionary[nameof (Sound)] as PdfReferenceHolder == (PdfReferenceHolder) null) ? dictionary : throw new ArgumentNullException();
      this.m_crossTable = crossTable;
      this.m_sound = new PdfSound();
    }
  }
}
