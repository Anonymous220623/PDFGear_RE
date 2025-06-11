// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfSoundAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfSoundAnnotation : PdfFileAnnotation
{
  private PdfSoundIcon m_icon;
  private PdfSound m_sound;

  public PdfSoundIcon Icon
  {
    get => this.m_icon;
    set
    {
      if (this.m_icon == value)
        return;
      this.m_icon = value;
      this.Dictionary.SetName("Name", this.m_icon.ToString());
    }
  }

  public PdfSound Sound
  {
    get => this.m_sound;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Sound));
      if (value == this.m_sound)
        return;
      this.m_sound = value;
    }
  }

  public PdfPopupAnnotationCollection ReviewHistory
  {
    get
    {
      return this.m_reviewHistory != null ? this.m_reviewHistory : (this.m_reviewHistory = new PdfPopupAnnotationCollection((PdfAnnotation) this, true));
    }
  }

  public PdfPopupAnnotationCollection Comments
  {
    get
    {
      return this.m_comments != null ? this.m_comments : (this.m_comments = new PdfPopupAnnotationCollection((PdfAnnotation) this, false));
    }
  }

  public override string FileName
  {
    get => this.m_sound.FileName;
    set
    {
      switch (value)
      {
        case null:
          throw new ArgumentNullException(nameof (FileName));
        case "":
          throw new ArgumentException("FileName can't be empty");
        default:
          if (!(this.m_sound.FileName != value))
            break;
          this.m_sound.FileName = value;
          break;
      }
    }
  }

  public PdfSoundAnnotation(RectangleF rectangle, string fileName)
    : base(rectangle)
  {
    this.m_sound = fileName != null ? new PdfSound(fileName) : throw new ArgumentNullException(nameof (fileName));
  }

  protected override void Initialize()
  {
    base.Initialize();
    this.Dictionary.SetProperty("Subtype", (IPdfPrimitive) new PdfName("Sound"));
  }

  protected override void Save()
  {
    base.Save();
    this.Dictionary.SetProperty("Sound", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_sound));
  }
}
