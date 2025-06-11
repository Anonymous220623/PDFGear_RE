// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfRubberStampAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfRubberStampAnnotation : PdfAnnotation
{
  private PdfRubberStampAnnotationIcon m_rubberStampAnnotaionIcon = PdfRubberStampAnnotationIcon.Draft;
  private PdfAppearance m_appearance;
  private string m_icon;
  private float m_stampWidth;
  private bool m_standardStampAppearance;
  private float rotateAngle;
  private SizeF m_size;
  private PointF m_location;
  private bool m_saved;

  public PdfRubberStampAnnotationIcon Icon
  {
    get => this.m_rubberStampAnnotaionIcon;
    set
    {
      this.m_rubberStampAnnotaionIcon = value;
      if (this.m_appearance != null)
        return;
      this.Dictionary.SetName("Name", "#23" + this.m_rubberStampAnnotaionIcon.ToString());
    }
  }

  public new PdfAppearance Appearance
  {
    get
    {
      if (this.m_appearance == null)
      {
        this.m_appearance = new PdfAppearance((PdfAnnotation) this);
        if (!this.m_standardStampAppearance)
          this.Dictionary.Remove("Name");
      }
      return this.m_appearance;
    }
    set
    {
      if (this.m_appearance == value)
        return;
      if (this.m_appearance == null && !this.m_standardStampAppearance)
        this.Dictionary.Remove("Name");
      this.m_appearance = value;
      if ((double) this.Opacity >= 1.0)
        return;
      this.m_appearance.Normal.Graphics.SetTransparency(this.Opacity);
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

  private string IconName => this.ObtainIconName(this.Icon);

  private PdfColor BackGroundColor => this.ObtainBackGroundColor();

  private PdfColor BorderColor => this.ObtainBorderColor();

  private Font Font => this.ObtainFont();

  public PdfRubberStampAnnotation()
  {
  }

  public PdfRubberStampAnnotation(RectangleF rectangle)
    : base(rectangle)
  {
  }

  public PdfRubberStampAnnotation(RectangleF rectangle, string text)
    : base(rectangle)
  {
    this.Text = text != null ? text : throw new ArgumentNullException(nameof (text));
  }

  protected override void Initialize()
  {
    base.Initialize();
    this.Dictionary.SetProperty("Subtype", (IPdfPrimitive) new PdfName("Stamp"));
  }

  private string ObtainIconName(PdfRubberStampAnnotationIcon icon)
  {
    switch (icon)
    {
      case PdfRubberStampAnnotationIcon.Approved:
        this.m_icon = "APPROVED";
        this.m_stampWidth = 126f;
        break;
      case PdfRubberStampAnnotationIcon.AsIs:
        this.m_icon = "AS IS";
        this.m_stampWidth = 75f;
        break;
      case PdfRubberStampAnnotationIcon.Confidential:
        this.m_icon = "CONFIDENTIAL";
        this.m_stampWidth = 166f;
        break;
      case PdfRubberStampAnnotationIcon.Departmental:
        this.m_icon = "DEPARTMENTAL";
        this.m_stampWidth = 186f;
        break;
      case PdfRubberStampAnnotationIcon.Draft:
        this.m_icon = "DRAFT";
        this.m_stampWidth = 90f;
        break;
      case PdfRubberStampAnnotationIcon.Experimental:
        this.m_icon = "EXPERIMENTAL";
        this.m_stampWidth = 176f;
        break;
      case PdfRubberStampAnnotationIcon.Expired:
        this.m_icon = "EXPIRED";
        this.m_stampWidth = 116f;
        break;
      case PdfRubberStampAnnotationIcon.Final:
        this.m_icon = "FINAL";
        this.m_stampWidth = 90f;
        break;
      case PdfRubberStampAnnotationIcon.ForComment:
        this.m_icon = "FOR COMMENT";
        this.m_stampWidth = 166f;
        break;
      case PdfRubberStampAnnotationIcon.ForPublicRelease:
        this.m_icon = "FOR PUBLIC RELEASE";
        this.m_stampWidth = 240f;
        break;
      case PdfRubberStampAnnotationIcon.NotApproved:
        this.m_icon = "NOT APPROVED";
        this.m_stampWidth = 186f;
        break;
      case PdfRubberStampAnnotationIcon.NotForPublicRelease:
        this.m_icon = "NOT FOR PUBLIC RELEASE";
        this.m_stampWidth = 290f;
        break;
      case PdfRubberStampAnnotationIcon.Sold:
        this.m_icon = "SOLD";
        this.m_stampWidth = 75f;
        break;
      case PdfRubberStampAnnotationIcon.TopSecret:
        this.m_icon = "TOP SECRET";
        this.m_stampWidth = 146f;
        break;
      case PdfRubberStampAnnotationIcon.Completed:
        this.m_icon = "COMPLETED";
        this.m_stampWidth = 136f;
        break;
      case PdfRubberStampAnnotationIcon.Void:
        this.m_icon = "VOID";
        this.m_stampWidth = 75f;
        break;
      case PdfRubberStampAnnotationIcon.InformationOnly:
        this.m_icon = "INFORMATION ONLY";
        this.m_stampWidth = 230f;
        break;
      case PdfRubberStampAnnotationIcon.PreliminaryResults:
        this.m_icon = "PRELIMINARY RESULTS";
        this.m_stampWidth = 260f;
        break;
    }
    return this.m_icon;
  }

  private PdfColor ObtainBackGroundColor()
  {
    PdfColor backGroundColor = new PdfColor();
    backGroundColor = this.Icon == PdfRubberStampAnnotationIcon.NotApproved || this.Icon == PdfRubberStampAnnotationIcon.Void ? new PdfColor(0.9843137f, 0.870588243f, 0.8666667f) : (this.Icon == PdfRubberStampAnnotationIcon.Approved || this.Icon == PdfRubberStampAnnotationIcon.Final || this.Icon == PdfRubberStampAnnotationIcon.Completed ? new PdfColor(0.8980392f, 0.933333337f, 0.870588243f) : new PdfColor(0.858823538f, 0.8901961f, 0.9411765f));
    return backGroundColor;
  }

  private PdfColor ObtainBorderColor()
  {
    PdfColor borderColor = new PdfColor();
    borderColor = this.Icon == PdfRubberStampAnnotationIcon.NotApproved || this.Icon == PdfRubberStampAnnotationIcon.Void ? new PdfColor(0.5921569f, 0.09019608f, 0.05882353f) : (this.Icon == PdfRubberStampAnnotationIcon.Approved || this.Icon == PdfRubberStampAnnotationIcon.Final || this.Icon == PdfRubberStampAnnotationIcon.Completed ? new PdfColor(0.286274523f, 0.431372553f, 0.149019614f) : new PdfColor(0.09411765f, 0.145098045f, 0.392156869f));
    return borderColor;
  }

  private Font ObtainFont() => new Font("Stencil", 21f);

  protected override void Save()
  {
    if (this.m_saved)
      return;
    this.CheckFlatten();
    PdfTemplate appearance = this.CreateAppearance();
    if (this.Flatten)
    {
      if (appearance != null)
      {
        if (this.Page != null)
          this.FlattenAnnotation((PdfPageBase) this.Page, appearance);
        else if (this.LoadedPage != null)
          this.FlattenAnnotation((PdfPageBase) this.LoadedPage, appearance);
      }
    }
    else if (appearance != null)
    {
      this.Appearance.Normal = appearance;
      this.Dictionary.SetProperty("AP", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.Appearance));
    }
    if (this.FlattenPopUps)
      this.FlattenPopup();
    if (!this.Flatten)
      base.Save();
    this.m_saved = true;
  }

  private void FlattenAnnotation(PdfPageBase page, PdfTemplate appearance)
  {
    PdfGraphics layerGraphics = this.GetLayerGraphics();
    if (this.Rotate != PdfAnnotationRotateAngle.RotateAngle0)
    {
      appearance.IsAnnotationTemplate = true;
      appearance.NeedScaling = true;
    }
    bool isNormalMatrix = this.Rotate == PdfAnnotationRotateAngle.RotateAngle0;
    page.Graphics.Save();
    if (!isNormalMatrix)
      isNormalMatrix = page.Rotation != PdfPageRotateAngle.RotateAngle0;
    RectangleF templateBounds = this.CalculateTemplateBounds(this.Bounds, page, appearance, isNormalMatrix);
    if ((double) this.RotateAngle == 0.0)
    {
      this.m_size = templateBounds.Size;
      this.m_location = templateBounds.Location;
    }
    else
    {
      this.m_size = appearance.Size;
      this.m_location = templateBounds.Location;
    }
    if (this.Rotate != PdfAnnotationRotateAngle.RotateAngle0)
    {
      if (this.Rotate == PdfAnnotationRotateAngle.RotateAngle90)
      {
        if (page.Rotation == PdfPageRotateAngle.RotateAngle270)
        {
          this.m_location.X += this.m_size.Width - this.m_size.Height;
          this.m_location.Y += this.m_size.Width - this.m_size.Height;
        }
        if (page.Rotation == PdfPageRotateAngle.RotateAngle180)
          this.m_location.X += this.m_size.Width;
        else
          this.m_location.X += this.m_size.Height;
      }
      else if (this.Rotate == PdfAnnotationRotateAngle.RotateAngle270)
      {
        if (page.Rotation == PdfPageRotateAngle.RotateAngle270)
        {
          this.m_location.X += this.m_size.Width - this.m_size.Height;
          this.m_location.Y += this.m_size.Width - this.m_size.Height;
        }
        if (page.Rotation == PdfPageRotateAngle.RotateAngle180)
        {
          this.m_location.X += this.m_size.Width - this.m_size.Height;
          this.m_location.Y += -this.m_size.Width;
        }
        else
          this.m_location.Y += -this.m_size.Width;
      }
      else if (this.Rotate == PdfAnnotationRotateAngle.RotateAngle180)
      {
        this.m_location.X += this.m_size.Width;
        this.m_location.Y += -this.m_size.Height;
      }
    }
    if ((double) this.Opacity < 1.0)
      page.Graphics.SetTransparency(this.Opacity);
    if (layerGraphics != null)
      layerGraphics.DrawPdfTemplate(appearance, this.m_location, this.m_size);
    else
      page.Graphics.DrawPdfTemplate(appearance, this.m_location, this.m_size);
    this.RemoveAnnoationFromPage(page, (PdfAnnotation) this);
    page.Graphics.Restore();
  }

  private PdfTemplate CreateAppearance()
  {
    PdfTemplate template = (PdfTemplate) null;
    if (this.m_appearance != null)
    {
      template = this.CustomAppearance(template);
      this.SetCustomStampIcon();
    }
    if (this.m_appearance == null && !this.Dictionary.ContainsKey("Name"))
      this.SetDefaultIcon();
    if (this.m_appearance == null)
    {
      template = this.CreateStampAppearance(template);
      this.m_standardStampAppearance = true;
    }
    return template;
  }

  private PdfTemplate CreateStampAppearance(PdfTemplate template)
  {
    PdfBrush mBackBrush = (PdfBrush) new PdfSolidBrush(this.BackGroundColor);
    PdfPen mBorderPen = new PdfPen(this.BorderColor, this.Border.Width);
    this.m_icon = this.IconName;
    StringFormat format = new StringFormat();
    format.Alignment = StringAlignment.Center;
    format.LineAlignment = StringAlignment.Center;
    format.FormatFlags = StringFormatFlags.FitBlackBox;
    template = new PdfTemplate(new RectangleF(0.0f, 0.0f, this.Bounds.Width, this.Bounds.Height));
    if (this.Rotate != PdfAnnotationRotateAngle.RotateAngle0 || (double) this.RotateAngle != 0.0)
    {
      this.rotateAngle = this.RotateAngle;
      if ((double) this.rotateAngle == 0.0)
        this.rotateAngle = (float) ((int) this.Rotate * 90);
      this.Bounds = this.GetRotatedBounds(this.Bounds, this.rotateAngle);
    }
    this.SetMatrix((PdfDictionary) template.m_content);
    PdfGraphics graphics = template.Graphics;
    graphics.ScaleTransform(template.Size.Width / (this.m_stampWidth + 4f), template.Size.Height / 28f);
    GraphicsPath graphicsPath = new GraphicsPath();
    graphicsPath.AddString(this.m_icon, this.Font.FontFamily, 2, this.Font.Size, new PointF(this.m_stampWidth / 2f, 15f), format);
    PdfPath path = new PdfPath(graphicsPath.PathPoints, graphicsPath.PathTypes);
    if (path != null)
    {
      if ((double) this.Opacity < 1.0)
      {
        PdfGraphicsState state = graphics.Save();
        graphics.SetTransparency(this.Opacity);
        this.DrawRubberStamp(graphics, path, mBorderPen, mBackBrush);
        graphics.Restore(state);
      }
      else
        this.DrawRubberStamp(graphics, path, mBorderPen, mBackBrush);
    }
    return template;
  }

  private void SetDefaultIcon()
  {
    if (!(this.IconName == "DRAFT"))
      return;
    this.Dictionary.SetName("Name", "#23" + this.m_rubberStampAnnotaionIcon.ToString());
  }

  private PdfTemplate CustomAppearance(PdfTemplate template)
  {
    if (this.m_appearance != null && this.m_appearance.Normal != null)
    {
      if (this.Rotate != PdfAnnotationRotateAngle.RotateAngle0 || (double) this.RotateAngle != 0.0)
      {
        this.rotateAngle = this.RotateAngle;
        if ((double) this.rotateAngle == 0.0)
          this.rotateAngle = (float) ((int) this.Rotate * 90);
        this.Bounds = this.GetRotatedBounds(this.Bounds, this.rotateAngle);
      }
      this.SetMatrix((PdfDictionary) this.Appearance.Normal.m_content);
      template = this.m_appearance.Normal;
    }
    return template;
  }

  private void DrawRubberStamp(
    PdfGraphics graphics,
    PdfPath path,
    PdfPen mBorderPen,
    PdfBrush mBackBrush)
  {
    graphics.DrawRoundedRectangle(new RectangleF(2f, 1f, this.m_stampWidth, 26f), 3, mBorderPen, mBackBrush);
    graphics.DrawPath((PdfBrush) new PdfSolidBrush(this.BorderColor), path);
  }

  private void DrawRubberStamp(
    PdfGraphics graphics,
    PdfPen mBorderPen,
    PdfBrush mBackBrush,
    PdfFont font,
    PdfStringFormat stringFormat)
  {
    graphics.DrawRoundedRectangle(new RectangleF(2f, 1f, this.m_stampWidth, 26f), 3, mBorderPen, mBackBrush);
    PdfBrush brush = (PdfBrush) new PdfSolidBrush(this.BorderColor);
    graphics.DrawString(this.m_icon, font, brush, new PointF((float) ((double) this.m_stampWidth / 2.0 + 1.0), 15f), stringFormat);
  }

  private new void SetMatrix(PdfDictionary template)
  {
    PdfArray pdfArray1 = (PdfArray) null;
    float[] numArray = new float[0];
    if (template["BBox"] is PdfArray pdfArray2)
    {
      if (this.Rotate != PdfAnnotationRotateAngle.RotateAngle0 || (double) this.RotateAngle != 0.0)
      {
        this.rotateAngle = this.RotateAngle;
        if ((double) this.rotateAngle == 0.0)
          this.rotateAngle = (float) ((int) this.Rotate * 90);
        pdfArray1 = new PdfArray(this.GetRotatedTransformMatrix(this.Bounds, this.rotateAngle).Matrix.Elements);
      }
      else
        pdfArray1 = new PdfArray(new float[6]
        {
          1f,
          0.0f,
          0.0f,
          1f,
          -(pdfArray2[0] as PdfNumber).FloatValue,
          -(pdfArray2[1] as PdfNumber).FloatValue
        });
    }
    if (pdfArray1 == null)
      return;
    template["Matrix"] = (IPdfPrimitive) pdfArray1;
  }

  private void SetCustomStampIcon()
  {
    if (this.Dictionary.ContainsKey("Name"))
      return;
    this.Dictionary.SetName("Name", "#23CustomStamp");
  }
}
