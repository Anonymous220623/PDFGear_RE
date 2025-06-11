// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfPopupAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfPopupAnnotation : PdfAnnotation
{
  private bool m_open;
  private PdfPopupIcon m_icon;
  private PdfAppearance m_appearance;
  private PdfAnnotationState m_state;
  private PdfAnnotationStateModel m_statemodel;

  public PdfPopupIcon Icon
  {
    get => this.m_icon;
    set
    {
      this.m_icon = value;
      this.Dictionary.SetName("Name", this.m_icon.ToString());
    }
  }

  public bool Open
  {
    get => this.m_open;
    set
    {
      if (this.m_open == value)
        return;
      this.m_open = value;
      this.Dictionary.SetBoolean(nameof (Open), this.m_open);
    }
  }

  public new PdfAppearance Appearance
  {
    get
    {
      if (this.m_appearance == null)
        this.m_appearance = new PdfAppearance((PdfAnnotation) this);
      return this.m_appearance;
    }
    set
    {
      if (this.m_appearance == value)
        return;
      this.m_appearance = value;
    }
  }

  public PdfAnnotationState State
  {
    get => this.m_state;
    set
    {
      this.m_state = value;
      this.Dictionary.SetProperty(nameof (State), (IPdfPrimitive) new PdfString(this.m_state.ToString()));
    }
  }

  public PdfAnnotationStateModel StateModel
  {
    get => this.m_statemodel;
    set
    {
      this.m_statemodel = value;
      this.Dictionary.SetProperty(nameof (StateModel), (IPdfPrimitive) new PdfString(this.m_statemodel.ToString()));
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

  public PdfPopupAnnotation()
  {
  }

  public PdfPopupAnnotation(RectangleF rectangle)
    : base(rectangle)
  {
  }

  public PdfPopupAnnotation(RectangleF rectangle, string text)
    : base(rectangle)
  {
    this.Text = text != null ? text : throw new ArgumentNullException(nameof (text));
  }

  internal PdfPopupAnnotation(RectangleF bounds, bool isPopup)
    : base(bounds)
  {
    this.Dictionary.SetProperty("Subtype", (IPdfPrimitive) new PdfName("Popup"));
    this.Dictionary.SetProperty("Rect", (IPdfPrimitive) PdfArray.FromRectangle(bounds));
  }

  protected override void Initialize()
  {
    base.Initialize();
    this.Dictionary.SetProperty("Subtype", (IPdfPrimitive) new PdfName("Text"));
  }

  protected override void Save()
  {
    this.CheckFlatten();
    if (this.SetAppearanceDictionary)
      this.DrawIcon(this.Appearance.Normal.Graphics);
    if (this.Flatten)
    {
      this.Dictionary.isSkip = true;
      if (this.m_appearance != null && this.m_appearance.Normal != null)
      {
        if (this.Page != null)
        {
          if ((double) this.Opacity < 1.0)
          {
            PdfGraphicsState state = this.Page.Graphics.Save();
            this.Page.Graphics.SetTransparency(this.Opacity);
            this.Page.Graphics.DrawPdfTemplate(this.m_appearance.Normal, this.Bounds.Location, this.Bounds.Size);
            this.Page.Graphics.Restore(state);
          }
          else
            this.Page.Graphics.DrawPdfTemplate(this.m_appearance.Normal, this.Bounds.Location, this.Bounds.Size);
        }
        else if (this.LoadedPage != null)
        {
          if ((double) this.Opacity < 1.0)
          {
            PdfGraphicsState state = this.LoadedPage.Graphics.Save();
            this.LoadedPage.Graphics.SetTransparency(this.Opacity);
            this.LoadedPage.Graphics.DrawPdfTemplate(this.m_appearance.Normal, this.Bounds.Location, this.Bounds.Size);
            this.LoadedPage.Graphics.Restore(state);
          }
          else
            this.LoadedPage.Graphics.DrawPdfTemplate(this.m_appearance.Normal, this.Bounds.Location, this.Bounds.Size);
        }
      }
      if (!this.FlattenPopUps)
        return;
      this.FlattenPopup();
      if (this.Page != null)
      {
        this.RemoveAnnoationFromPage((PdfPageBase) this.Page, (PdfAnnotation) this);
      }
      else
      {
        if (this.m_loadedPage == null)
          return;
        this.RemoveAnnoationFromPage((PdfPageBase) this.m_loadedPage, (PdfAnnotation) this);
        int index = this.LoadedPage.CrossTable.PdfObjects.IndexOf(this.LoadedPage.CrossTable.GetObject((IPdfPrimitive) this.Dictionary));
        if (index != -1)
          this.LoadedPage.CrossTable.PdfObjects.Remove(index);
        this.LoadedPage.TerminalAnnotation.Remove(this.Dictionary);
      }
    }
    else
    {
      base.Save();
      if (this.m_appearance == null || this.m_appearance.Normal == null)
        return;
      this.Dictionary.SetProperty("AP", (IPdfWrapper) this.m_appearance);
    }
  }

  private void DrawIcon(PdfGraphics graphics)
  {
    if (this.Page == null)
    {
      SizeF size1 = this.m_loadedPage.Size;
    }
    else
    {
      SizeF size2 = this.Page.Size;
    }
    if (!this.Dictionary.ContainsKey("Name") || !(this.Dictionary["Name"].ToString() == "/Comment") || this.Dictionary["Rect"] == null)
      return;
    PdfArray pdfArray = this.Dictionary["Rect"] as PdfArray;
    PdfCrossTable pdfCrossTable = this.Page != null ? this.Page.CrossTable : this.m_loadedPage.CrossTable;
    if (pdfArray == null)
      return;
    pdfCrossTable.GetObject(pdfArray[0]);
    pdfCrossTable.GetObject(pdfArray[1]);
    PdfPen pen1 = new PdfPen(new PdfColor((byte) 0, (byte) 0, (byte) 0), 0.3f);
    PdfBrush brush = (PdfBrush) new PdfSolidBrush(this.Color);
    PdfPen pen2 = new PdfPen((PdfBrush) new PdfSolidBrush(new PdfColor(byte.MaxValue, byte.MaxValue, byte.MaxValue), true));
    PointF[] points = new PointF[3]
    {
      new PointF(6f, 8f),
      new PointF(9f, 8f),
      new PointF(4f, 12f)
    };
    PdfPath path = new PdfPath();
    if (this.Color.IsEmpty)
      brush = PdfBrushes.Gold;
    PdfTemplate template = new PdfTemplate(15f, 14f);
    template.Graphics.DrawRectangle(pen1, brush, new RectangleF(0.0f, 0.0f, 15f, 14f));
    template.Graphics.DrawPolygon(pen1, PdfBrushes.White, points);
    path.AddEllipse(2f, 2f, 11f, 8f);
    template.Graphics.DrawPath(pen1, PdfBrushes.White, path);
    template.Graphics.DrawArc(pen2, new RectangleF(2f, 2f, 11f, 8f), 108f, 12.7f);
    template.Graphics.DrawLine(pen1, new PointF(4f, 12f), new PointF(6.5f, 10f));
    graphics.DrawPdfTemplate(template, new PointF(0.0f, 0.0f), new SizeF(this.Bounds.Width, this.Bounds.Height));
  }
}
