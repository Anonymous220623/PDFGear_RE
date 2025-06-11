// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfLoadedPopupAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfLoadedPopupAnnotation : PdfLoadedStyledAnnotation
{
  private PdfCrossTable m_crossTable;
  private bool m_open;
  private PdfPopupIcon m_name;
  private PdfAnnotationState m_state;
  private PdfAnnotationStateModel m_statemodel;

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

  public PdfAnnotationState State
  {
    get => this.ObtainState();
    set
    {
      this.m_state = value;
      this.Dictionary.SetProperty(nameof (State), (IPdfPrimitive) new PdfString(this.m_state.ToString()));
    }
  }

  public PdfAnnotationStateModel StateModel
  {
    get => this.ObtainStateModel();
    set
    {
      this.m_statemodel = value;
      this.Dictionary.SetProperty(nameof (StateModel), (IPdfPrimitive) new PdfString(this.m_statemodel.ToString()));
    }
  }

  public bool Open
  {
    get => this.ObtainOpen();
    set
    {
      this.m_open = value;
      this.Dictionary.SetBoolean(nameof (Open), this.m_open);
    }
  }

  public PdfPopupIcon Icon
  {
    get => this.ObtainIcon();
    set
    {
      this.m_name = value;
      this.Dictionary.SetName("Name", this.m_name.ToString());
    }
  }

  internal PdfLoadedPopupAnnotation(
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
    this.Text = text;
  }

  private bool ObtainOpen()
  {
    bool open = false;
    if (this.Dictionary.ContainsKey("Open"))
      open = (this.Dictionary["Open"] as PdfBoolean).Value;
    return open;
  }

  private PdfPopupIcon ObtainIcon()
  {
    PdfPopupIcon icon = PdfPopupIcon.NewParagraph;
    if (this.Dictionary.ContainsKey("Name"))
      icon = this.GetIconName((this.Dictionary["Name"] as PdfName).Value.ToString());
    return icon;
  }

  private PdfPopupIcon GetIconName(string name)
  {
    PdfPopupIcon iconName = PdfPopupIcon.NewParagraph;
    switch (name)
    {
      case "Note":
        iconName = PdfPopupIcon.Note;
        break;
      case "Comment":
        iconName = PdfPopupIcon.Comment;
        break;
      case "Help":
        iconName = PdfPopupIcon.Help;
        break;
      case "Insert":
        iconName = PdfPopupIcon.Insert;
        break;
      case "Key":
        iconName = PdfPopupIcon.Key;
        break;
      case "NewParagraph":
        iconName = PdfPopupIcon.NewParagraph;
        break;
      case "Paragraph":
        iconName = PdfPopupIcon.Paragraph;
        break;
    }
    return iconName;
  }

  protected override void Save()
  {
    PdfPageBase page = (PdfPageBase) this.Page;
    if (page.Annotations.Flatten)
      this.Page.Annotations.Flatten = page.Annotations.Flatten;
    if (!this.Flatten && !this.Page.Annotations.Flatten)
      return;
    bool flag = true;
    if (this.Dictionary.ContainsKey("F") && this.Dictionary["F"] is PdfNumber pdfNumber && pdfNumber.IntValue == 30 && !this.FlattenPopUps)
      flag = false;
    if (this.Dictionary["AP"] != null && flag && PdfCrossTable.Dereference(this.Dictionary["AP"]) is PdfDictionary pdfDictionary1 && PdfCrossTable.Dereference(pdfDictionary1["N"]) is PdfDictionary pdfDictionary2 && pdfDictionary2 is PdfStream template1)
    {
      PdfTemplate template = new PdfTemplate(template1);
      if (template != null)
      {
        PdfGraphics pdfGraphics = this.ObtainlayerGraphics();
        PdfGraphicsState state = this.Page.Graphics.Save();
        if ((double) this.Opacity < 1.0)
          this.Page.Graphics.SetTransparency(this.Opacity);
        if (pdfGraphics != null)
          pdfGraphics.DrawPdfTemplate(template, this.Bounds.Location, this.Bounds.Size);
        else
          this.Page.Graphics.DrawPdfTemplate(template, this.Bounds.Location, this.Bounds.Size);
        this.Page.Graphics.Restore(state);
      }
    }
    this.RemoveAnnoationFromPage((PdfPageBase) this.Page, (PdfAnnotation) this);
    if (!this.FlattenPopUps)
      return;
    this.FlattenLoadedPopup();
  }

  internal void ChangeBounds(RectangleF bounds)
  {
    if (this.Dictionary["Popup"] == null || !(PdfCrossTable.Dereference(this.Dictionary["Popup"]) is PdfDictionary pdfDictionary))
      return;
    if (PdfCrossTable.Dereference(pdfDictionary["Rect"]) is PdfArray primitive)
    {
      (this.m_crossTable.GetObject(primitive[0]) as PdfNumber).FloatValue = bounds.X;
      (this.m_crossTable.GetObject(primitive[1]) as PdfNumber).FloatValue = this.Page.Size.Height - (bounds.Y + bounds.Height);
      (this.m_crossTable.GetObject(primitive[2]) as PdfNumber).FloatValue = bounds.X + bounds.Width;
      (this.m_crossTable.GetObject(primitive[3]) as PdfNumber).FloatValue = this.Page.Size.Height - bounds.Y;
    }
    pdfDictionary.SetProperty("Rect", (IPdfPrimitive) primitive);
  }
}
