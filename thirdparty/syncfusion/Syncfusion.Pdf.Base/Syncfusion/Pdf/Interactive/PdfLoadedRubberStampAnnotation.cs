// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfLoadedRubberStampAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfLoadedRubberStampAnnotation : PdfLoadedStyledAnnotation
{
  private PdfCrossTable m_crossTable;
  private PdfRubberStampAnnotationIcon m_name;
  private string m_icon;
  private PdfNumber rotation;
  private float m_stampWidth;
  private bool isStampAppearance;
  private RectangleF innerTemplateBounds = RectangleF.Empty;
  private float rotateAngle;
  private SizeF m_size;
  private PointF m_location;

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

  public PdfRubberStampAnnotationIcon Icon
  {
    get => this.ObtainIcon();
    set
    {
      this.m_name = value;
      this.Dictionary.SetName("Name", "#23" + this.m_name.ToString());
      this.isStampAppearance = true;
    }
  }

  internal RectangleF InnerTemplateBounds
  {
    get
    {
      this.innerTemplateBounds = this.ObtainInnerBounds();
      this.innerTemplateBounds.X = this.Bounds.X;
      this.innerTemplateBounds.Y = this.Bounds.Y;
      return this.innerTemplateBounds;
    }
  }

  private PdfColor BackGroundColor => this.ObtainBackGroundColor();

  private PdfColor BorderColor => this.ObtainBorderColor();

  private Font Font => this.ObtainFont();

  internal PdfLoadedRubberStampAnnotation(
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

  private RectangleF ObtainInnerBounds()
  {
    RectangleF innerBounds = RectangleF.Empty;
    if (this.Dictionary.ContainsKey("AP") && PdfCrossTable.Dereference(this.Dictionary["AP"]) is PdfDictionary pdfDictionary1 && pdfDictionary1 != null && PdfCrossTable.Dereference(pdfDictionary1["N"]) is PdfDictionary pdfDictionary2 && pdfDictionary2.ContainsKey("BBox") && PdfCrossTable.Dereference(pdfDictionary2["BBox"]) is PdfArray pdfArray && pdfArray.Count == 4)
      innerBounds = pdfArray.ToRectangle();
    return innerBounds;
  }

  private PdfRubberStampAnnotationIcon ObtainIcon()
  {
    PdfRubberStampAnnotationIcon icon = PdfRubberStampAnnotationIcon.Draft;
    if (this.Dictionary.ContainsKey("Name"))
      icon = this.GetIconName((this.Dictionary["Name"] as PdfName).Value.ToString());
    return icon;
  }

  private PdfRubberStampAnnotationIcon GetIconName(string name)
  {
    name = name.TrimStart("#23".ToCharArray());
    PdfRubberStampAnnotationIcon iconName = PdfRubberStampAnnotationIcon.Draft;
    switch (name)
    {
      case "Approved":
      case "SBApproved":
        iconName = PdfRubberStampAnnotationIcon.Approved;
        this.m_icon = "APPROVED";
        this.m_stampWidth = 126f;
        break;
      case "AsIs":
      case "SBAsIs":
        iconName = PdfRubberStampAnnotationIcon.AsIs;
        this.m_icon = "AS IS";
        this.m_stampWidth = 75f;
        break;
      case "Confidential":
      case "SBConfidential":
        iconName = PdfRubberStampAnnotationIcon.Confidential;
        this.m_icon = "CONFIDENTIAL";
        this.m_stampWidth = 166f;
        break;
      case "Departmental":
      case "SBDepartmental":
        iconName = PdfRubberStampAnnotationIcon.Departmental;
        this.m_icon = "DEPARTMENTAL";
        this.m_stampWidth = 186f;
        break;
      case "Draft":
      case "SBDraft":
        iconName = PdfRubberStampAnnotationIcon.Draft;
        this.m_icon = "DRAFT";
        this.m_stampWidth = 90f;
        break;
      case "Experimental":
      case "SBExperimental":
        iconName = PdfRubberStampAnnotationIcon.Experimental;
        this.m_icon = "EXPERIMENTAL";
        this.m_stampWidth = 176f;
        break;
      case "Expired":
      case "SBExpired":
        iconName = PdfRubberStampAnnotationIcon.Expired;
        this.m_icon = "EXPIRED";
        this.m_stampWidth = 116f;
        break;
      case "Final":
      case "SBFinal":
        iconName = PdfRubberStampAnnotationIcon.Final;
        this.m_icon = "FINAL";
        this.m_stampWidth = 90f;
        break;
      case "ForComment":
      case "SBForComment":
        iconName = PdfRubberStampAnnotationIcon.ForComment;
        this.m_icon = "FOR COMMENT";
        this.m_stampWidth = 166f;
        break;
      case "ForPublicRelease":
      case "SBForPublicRelease":
        iconName = PdfRubberStampAnnotationIcon.ForPublicRelease;
        this.m_icon = "FOR PUBLIC RELEASE";
        this.m_stampWidth = 240f;
        break;
      case "NotApproved":
      case "SBNotApproved":
        iconName = PdfRubberStampAnnotationIcon.NotApproved;
        this.m_icon = "NOT APPROVED";
        this.m_stampWidth = 186f;
        break;
      case "NotForPublicRelease":
      case "SBNotForPublicRelease":
        iconName = PdfRubberStampAnnotationIcon.NotForPublicRelease;
        this.m_icon = "NOT FOR PUBLIC RELEASE";
        this.m_stampWidth = 290f;
        break;
      case "Sold":
      case "SBSold":
        iconName = PdfRubberStampAnnotationIcon.Sold;
        this.m_icon = "SOLD";
        this.m_stampWidth = 75f;
        break;
      case "TopSecret":
      case "SBTopSecret":
        iconName = PdfRubberStampAnnotationIcon.TopSecret;
        this.m_icon = "TOP SECRET";
        this.m_stampWidth = 146f;
        break;
      case "Void":
      case "SBVoid":
        iconName = PdfRubberStampAnnotationIcon.Void;
        this.m_icon = "VOID";
        this.m_stampWidth = 75f;
        break;
      case "InformationOnly":
      case "SBInformationOnly":
        iconName = PdfRubberStampAnnotationIcon.InformationOnly;
        this.m_icon = "INFORMATION ONLY";
        this.m_stampWidth = 230f;
        break;
      case "PreliminaryResults":
      case "SBPreliminaryResults":
        iconName = PdfRubberStampAnnotationIcon.PreliminaryResults;
        this.m_icon = "PRELIMINARY RESULTS";
        this.m_stampWidth = 260f;
        break;
      case "Completed":
      case "SBCompleted":
        iconName = PdfRubberStampAnnotationIcon.Completed;
        this.m_icon = "COMPLETED";
        this.m_stampWidth = 136f;
        break;
    }
    return iconName;
  }

  private PdfColor ObtainBackGroundColor()
  {
    PdfColor backGroundColor = new PdfColor();
    backGroundColor = this.m_name == PdfRubberStampAnnotationIcon.NotApproved || this.m_name == PdfRubberStampAnnotationIcon.Void ? new PdfColor(0.9843137f, 0.870588243f, 0.8666667f) : (this.m_name == PdfRubberStampAnnotationIcon.Approved || this.m_name == PdfRubberStampAnnotationIcon.Final || this.m_name == PdfRubberStampAnnotationIcon.Completed ? new PdfColor(0.8980392f, 0.933333337f, 0.870588243f) : new PdfColor(0.858823538f, 0.8901961f, 0.9411765f));
    return backGroundColor;
  }

  private PdfColor ObtainBorderColor()
  {
    PdfColor borderColor = new PdfColor();
    borderColor = this.m_name == PdfRubberStampAnnotationIcon.NotApproved || this.m_name == PdfRubberStampAnnotationIcon.Void ? new PdfColor(0.5921569f, 0.09019608f, 0.05882353f) : (this.m_name == PdfRubberStampAnnotationIcon.Approved || this.m_name == PdfRubberStampAnnotationIcon.Final || this.m_name == PdfRubberStampAnnotationIcon.Completed ? new PdfColor(0.286274523f, 0.431372553f, 0.149019614f) : new PdfColor(0.09411765f, 0.145098045f, 0.392156869f));
    return borderColor;
  }

  private Font ObtainFont() => new Font("Stencil", 21f);

  protected override void Save()
  {
    this.CheckFlatten();
    if (this.Flatten || this.Page.Annotations.Flatten || this.SetAppearanceDictionary)
    {
      PdfTemplate appearance = this.CreateAppearance();
      if (this.Flatten || this.Page.Annotations.Flatten)
        this.FlattenAnnotation((PdfPageBase) this.Page, appearance);
      else if (appearance != null)
      {
        this.Appearance.Normal = appearance;
        this.Dictionary.SetProperty("AP", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.Appearance));
      }
    }
    if (!this.FlattenPopUps)
      return;
    this.FlattenLoadedPopup();
  }

  private void FlattenAnnotation(PdfPageBase page, PdfTemplate appearance)
  {
    if (this.Dictionary.ContainsKey("AP") && appearance == null)
    {
      if (!(PdfCrossTable.Dereference(this.Dictionary["AP"]) is PdfDictionary pdfDictionary) || !(PdfCrossTable.Dereference(pdfDictionary["N"]) is PdfDictionary dictionary) || !(dictionary is PdfStream template))
        return;
      bool flag1 = this.Page.Rotation == PdfPageRotateAngle.RotateAngle0 && this.Rotate == PdfAnnotationRotateAngle.RotateAngle0;
      if (!flag1)
        flag1 = this.Page.Rotation != PdfPageRotateAngle.RotateAngle0 && this.Rotate == PdfAnnotationRotateAngle.RotateAngle0;
      bool flag2 = flag1;
      appearance = new PdfTemplate(template, flag2);
      if (appearance == null)
        return;
      bool isNormalMatrix = this.ValidateTemplateMatrix(dictionary);
      if (flag2)
        this.FlattenAnnotationTemplate(appearance, flag2);
      else
        this.FlattenAnnotationTemplate(appearance, isNormalMatrix);
    }
    else if (!this.Dictionary.ContainsKey("AP") && appearance != null)
    {
      bool isNormalMatrix = this.ValidateTemplateMatrix((PdfDictionary) appearance.m_content);
      this.FlattenAnnotationTemplate(appearance, isNormalMatrix);
    }
    else
    {
      if (!this.Dictionary.ContainsKey("AP") || appearance == null)
        return;
      bool isNormalMatrix = this.ValidateTemplateMatrix((PdfDictionary) appearance.m_content);
      this.FlattenAnnotationTemplate(appearance, isNormalMatrix);
    }
  }

  private new void FlattenAnnotationTemplate(PdfTemplate appearance, bool isNormalMatrix)
  {
    PdfGraphics pdfGraphics = this.ObtainlayerGraphics();
    PdfGraphicsState state = this.Page.Graphics.Save();
    if (!isNormalMatrix && this.Rotate != PdfAnnotationRotateAngle.RotateAngle180)
    {
      appearance.IsAnnotationTemplate = true;
      appearance.NeedScaling = true;
    }
    if ((double) this.Opacity < 1.0)
      this.Page.Graphics.SetTransparency(this.Opacity);
    RectangleF templateBounds = this.CalculateTemplateBounds(this.Bounds, (PdfPageBase) this.Page, appearance, isNormalMatrix);
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
    bool flag1 = false;
    if (this.Rotate != PdfAnnotationRotateAngle.RotateAngle0 && appearance.m_content != null && appearance.m_content.ContainsKey("Matrix") && PdfCrossTable.Dereference(appearance.m_content["Matrix"]) is PdfArray pdfArray && pdfArray.Count == 6 && (double) (pdfArray[4] as PdfNumber).FloatValue == 0.0 && (double) (pdfArray[5] as PdfNumber).FloatValue == 0.0)
      flag1 = true;
    float num1 = (double) appearance.Width > 0.0 ? templateBounds.Size.Width / appearance.Width : 1f;
    float num2 = (double) appearance.Height > 0.0 ? templateBounds.Size.Height / appearance.Height : 1f;
    bool flag2 = (double) num1 != 1.0 || (double) num2 != 1.0;
    if (this.Rotate != PdfAnnotationRotateAngle.RotateAngle0 && flag1)
    {
      if (this.Rotate == PdfAnnotationRotateAngle.RotateAngle90)
      {
        if (this.Page.Rotation == PdfPageRotateAngle.RotateAngle270)
        {
          if (flag2 && ((double) this.Bounds.X != 0.0 || (double) this.Bounds.Y != 0.0))
          {
            this.m_location.X += this.m_size.Width - this.m_size.Height;
            this.m_location.Y += this.m_size.Width;
          }
          else
          {
            this.m_location.X += this.m_size.Height;
            this.m_location.Y += (float) ((double) this.m_size.Width - (double) this.m_size.Height + ((double) this.m_size.Width - (double) this.m_size.Height));
          }
        }
        else
          this.m_location.X += this.m_size.Height;
      }
      else if (this.Rotate == PdfAnnotationRotateAngle.RotateAngle270)
      {
        if (this.Page.Rotation == PdfPageRotateAngle.RotateAngle270)
        {
          if (flag2)
            this.m_location.Y += this.m_size.Width - this.m_size.Height;
        }
        else if (!flag2 && ((double) this.Bounds.X != 0.0 || (double) this.Bounds.Y != 0.0))
          this.m_location.Y += -this.m_size.Width;
        else
          this.m_location.Y += (float) -((double) this.m_size.Width - (double) this.m_size.Height);
      }
      else if (this.Rotate == PdfAnnotationRotateAngle.RotateAngle180)
      {
        this.m_location.X += this.m_size.Width;
        this.m_location.Y += -this.m_size.Height;
      }
    }
    if (pdfGraphics != null && this.Page.Rotation == PdfPageRotateAngle.RotateAngle0)
      pdfGraphics.DrawPdfTemplate(appearance, this.m_location, templateBounds.Size);
    else
      this.Page.Graphics.DrawPdfTemplate(appearance, this.m_location, templateBounds.Size);
    this.Page.Graphics.Restore(state);
    this.RemoveAnnoationFromPage((PdfPageBase) this.Page, (PdfAnnotation) this);
  }

  private PdfTemplate CreateAppearance()
  {
    PdfTemplate appearance = (PdfTemplate) null;
    if (this.isStampAppearance && this.SetAppearanceDictionary)
      appearance = this.CreateStampAppearance();
    else if (this.SetAppearanceDictionary && this.Dictionary.ContainsKey("AP"))
      this.ModifyTemplateAppearance(this.Bounds);
    return appearance;
  }

  private PdfTemplate CreateStampAppearance()
  {
    PdfBrush mBackBrush = (PdfBrush) new PdfSolidBrush(this.BackGroundColor);
    PdfPen mBorderPen = new PdfPen(this.BorderColor, this.Border.Width);
    this.m_name = this.Icon;
    StringFormat format = new StringFormat();
    format.Alignment = StringAlignment.Center;
    format.LineAlignment = StringAlignment.Center;
    format.FormatFlags = StringFormatFlags.FitBlackBox;
    PdfTemplate stampAppearance = new PdfTemplate(new RectangleF(0.0f, 0.0f, this.Bounds.Width, this.Bounds.Height));
    if (this.Rotate != PdfAnnotationRotateAngle.RotateAngle0 || (double) this.RotateAngle != 0.0)
    {
      this.rotateAngle = this.RotateAngle;
      if ((double) this.rotateAngle == 0.0)
        this.rotateAngle = (float) ((int) this.Rotate * 90);
      this.Bounds = this.GetRotatedBounds(this.Bounds, this.rotateAngle);
    }
    this.SetMatrix((PdfDictionary) stampAppearance.m_content);
    PdfGraphics graphics = stampAppearance.Graphics;
    graphics.ScaleTransform(stampAppearance.Size.Width / (this.m_stampWidth + 4f), stampAppearance.Size.Height / 28f);
    GraphicsPath graphicsPath = new GraphicsPath();
    graphicsPath.AddString(this.m_icon, this.Font.FontFamily, 2, this.Font.Size, new PointF(this.m_stampWidth / 2f, 15f), format);
    PdfPath path = new PdfPath(graphicsPath.PathPoints, graphicsPath.PathTypes);
    if (path != null)
    {
      if ((double) this.Opacity < 1.0)
      {
        graphics.Save();
        graphics.SetTransparency(this.Opacity);
        this.DrawRubberStamp(graphics, path, mBorderPen, mBackBrush);
        graphics.Restore();
      }
      else
        this.DrawRubberStamp(graphics, path, mBorderPen, mBackBrush);
    }
    return stampAppearance;
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

  protected void ModifyTemplateAppearance(RectangleF bounds)
  {
    if (!(PdfCrossTable.Dereference(this.Dictionary["AP"]) is PdfDictionary pdfDictionary1) || !pdfDictionary1.ContainsKey("N") || !(PdfCrossTable.Dereference(pdfDictionary1["N"]) is PdfDictionary pdfDictionary2) || !(pdfDictionary2 is PdfStream template1))
      return;
    int num = 0;
    PdfTemplate template2 = new PdfTemplate(template1);
    this.Appearance = new PdfAppearance((PdfAnnotation) this);
    this.Appearance.Normal.Graphics.SetBBox(new RectangleF(0.0f, 0.0f, this.Bounds.Width, this.Bounds.Height));
    if (PdfCrossTable.Dereference(template2.m_content["Matrix"]) is PdfArray pdfArray && pdfArray.Count > 5)
      num = this.ObtainGraphicsRotation(new PdfTransformationMatrix()
      {
        Matrix = new Matrix((pdfArray.Elements[0] as PdfNumber).FloatValue, (pdfArray.Elements[1] as PdfNumber).FloatValue, (pdfArray.Elements[2] as PdfNumber).FloatValue, (pdfArray.Elements[3] as PdfNumber).FloatValue, (pdfArray.Elements[4] as PdfNumber).FloatValue, (pdfArray.Elements[5] as PdfNumber).FloatValue)
      });
    if (num == 90 || num == 270)
      this.Appearance.Normal.Graphics.ScaleTransform(this.Bounds.Width / template2.Height, this.Bounds.Height / template2.Width);
    else
      this.Appearance.Normal.Graphics.ScaleTransform(this.Bounds.Width / template2.Width, this.Bounds.Height / template2.Height);
    if (this.rotationModified)
      this.SetMatrix((PdfDictionary) this.Appearance.Normal.m_content);
    else
      base.SetMatrix((PdfDictionary) this.Appearance.Normal.m_content);
    this.Appearance.Normal.Graphics.Save();
    if ((double) this.Opacity < 1.0)
      this.Appearance.Normal.Graphics.SetTransparency(this.Opacity);
    this.Appearance.Normal.Graphics.DrawPdfTemplate(template2, new PointF(0.0f, 0.0f));
    this.Appearance.Normal.Graphics.Restore();
    this.Dictionary["AP"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.Appearance);
  }

  private new void SetMatrix(PdfDictionary template)
  {
    PdfArray pdfArray1 = (PdfArray) null;
    float[] numArray = new float[0];
    if (PdfCrossTable.Dereference(template["BBox"]) is PdfArray pdfArray2)
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
}
