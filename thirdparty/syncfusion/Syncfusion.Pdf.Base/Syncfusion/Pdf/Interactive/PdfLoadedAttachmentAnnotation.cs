// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfLoadedAttachmentAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfLoadedAttachmentAnnotation : PdfLoadedStyledAnnotation
{
  private PdfCrossTable m_crossTable;
  private PdfAttachmentIcon m_icon;

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

  public PdfAttachmentIcon Icon
  {
    get => this.ObtainIcon();
    set
    {
      this.m_icon = value;
      this.Dictionary.SetName("Name", this.m_icon.ToString());
    }
  }

  public string FileName
  {
    get
    {
      PdfDictionary pdfDictionary = this.m_crossTable.GetObject(this.Dictionary["FS"]) as PdfDictionary;
      string fileName = " ";
      if (pdfDictionary.ContainsKey("F"))
        fileName = (pdfDictionary["F"] as PdfString).Value;
      else if (pdfDictionary.ContainsKey("Desc"))
        fileName = (pdfDictionary["Desc"] as PdfString).Value;
      else if (pdfDictionary.ContainsKey("UF"))
        fileName = (pdfDictionary["UF"] as PdfString).Value;
      return fileName;
    }
  }

  public byte[] Data
  {
    get
    {
      byte[] data = (byte[]) null;
      if (this.m_crossTable.GetObject(this.Dictionary["FS"]) is PdfDictionary pdfDictionary1 && pdfDictionary1.ContainsKey("EF"))
      {
        PdfDictionary pdfDictionary = !(pdfDictionary1["EF"] is PdfDictionary) ? (pdfDictionary1["EF"] as PdfReferenceHolder).Object as PdfDictionary : pdfDictionary1["EF"] as PdfDictionary;
        if (pdfDictionary != null && pdfDictionary.ContainsKey("F"))
        {
          PdfReferenceHolder pdfReferenceHolder = pdfDictionary["F"] as PdfReferenceHolder;
          if (pdfReferenceHolder != (PdfReferenceHolder) null && pdfReferenceHolder.Object is PdfStream pdfStream)
          {
            pdfStream.Decompress();
            data = PdfStream.StreamToBytes((Stream) pdfStream.InternalStream);
          }
        }
      }
      return data;
    }
  }

  internal PdfLoadedAttachmentAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rectangle,
    string text)
    : base(dictionary, crossTable)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    this.Dictionary = dictionary;
    this.m_crossTable = crossTable;
  }

  private PdfAttachmentIcon ObtainIcon()
  {
    PdfAttachmentIcon icon = PdfAttachmentIcon.PushPin;
    if (this.Dictionary.ContainsKey("Name"))
    {
      switch ((this.Dictionary["Name"] as PdfName).Value.ToString())
      {
        case "Graph":
          icon = PdfAttachmentIcon.Graph;
          break;
        case "Paperclip":
          icon = PdfAttachmentIcon.Paperclip;
          break;
        case "PushPin":
          icon = PdfAttachmentIcon.PushPin;
          break;
        case "Tag":
          icon = PdfAttachmentIcon.Tag;
          break;
      }
    }
    return icon;
  }
}
