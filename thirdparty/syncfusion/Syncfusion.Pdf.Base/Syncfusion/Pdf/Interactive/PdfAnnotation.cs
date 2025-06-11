// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public abstract class PdfAnnotation : IPdfWrapper
{
  internal const string TopCaption = "Top";
  private PdfColor m_color = PdfColor.Empty;
  private PdfAnnotationBorder m_border;
  private RectangleF m_rectangle = RectangleF.Empty;
  private PdfPage m_page;
  internal PdfLoadedPage m_loadedPage;
  private string m_text = string.Empty;
  private string m_author = string.Empty;
  private string m_subject = string.Empty;
  private DateTime m_modifiedDate;
  private PdfAnnotationFlags m_annotationFlags;
  private PdfDictionary m_dictionary = new PdfDictionary();
  private PdfColor m_innerColor;
  private float m_opacity = 1f;
  private bool m_flatten;
  private bool m_flattenPopUps;
  private PdfTag m_tag;
  private PdfAppearance m_appearance;
  private bool m_setAppearanceDictionary;
  internal bool isAuthorExplicitSet;
  private PdfPopupAnnotation m_popup;
  private PdfLayer layer;
  private PdfAnnotationRotateAngle m_angle;
  internal bool rotationModified;
  private PdfMargins m_margins = new PdfMargins();
  internal PdfPopupAnnotationCollection m_reviewHistory;
  internal PdfPopupAnnotationCollection m_comments;
  private List<PdfAnnotation> m_popupAnnotations = new List<PdfAnnotation>();
  private string m_name;
  private float m_borderWidth;
  internal bool m_isStandardAppearance = true;
  private float rotateAngle;
  private bool m_locationDisplaced;

  public virtual PdfColor Color
  {
    get => this.m_color;
    set
    {
      if (!(this.m_color != value))
        return;
      this.m_color = value;
      PdfColorSpace colorSpace = PdfColorSpace.RGB;
      if (this.Page != null)
        colorSpace = this.Page.Section.Parent.Document.ColorSpace;
      this.m_dictionary.SetProperty("C", (IPdfPrimitive) this.m_color.ToArray(colorSpace));
    }
  }

  public virtual float Opacity
  {
    get
    {
      if (this.m_dictionary.Items.ContainsKey(new PdfName("CA")))
        this.m_opacity = (this.m_dictionary.Items[new PdfName("CA")] as PdfNumber).FloatValue;
      return this.m_opacity;
    }
    set
    {
      if ((double) value < 0.0 || (double) value > 1.0)
        throw new ArgumentException("Valid value should be between 0 to 1.");
      if ((double) this.m_opacity == (double) value)
        return;
      this.m_opacity = value;
      this.m_dictionary.SetProperty("CA", (IPdfPrimitive) new PdfNumber(this.m_opacity));
    }
  }

  public virtual PdfColor InnerColor
  {
    get => this.m_innerColor;
    set => this.m_innerColor = value;
  }

  public virtual PdfAnnotationBorder Border
  {
    get
    {
      if (this.m_border == null)
        this.m_border = new PdfAnnotationBorder();
      return this.m_border;
    }
    set
    {
      this.m_border = value;
      this.Dictionary.SetProperty(nameof (Border), (IPdfWrapper) this.m_border);
    }
  }

  public virtual RectangleF Bounds
  {
    get => this.m_rectangle;
    set
    {
      if (!(this.m_rectangle != value))
        return;
      this.m_rectangle = value;
      this.Dictionary.SetProperty("Rect", (IPdfPrimitive) PdfArray.FromRectangle(value));
    }
  }

  public virtual PointF Location
  {
    get => this.m_rectangle.Location;
    set
    {
      this.m_rectangle = this.Bounds;
      this.m_rectangle.Location = value;
      this.Dictionary.SetProperty("Rect", (IPdfPrimitive) PdfArray.FromRectangle(this.m_rectangle));
    }
  }

  public virtual SizeF Size
  {
    get => this.m_rectangle.Size;
    set
    {
      this.m_rectangle = this.Bounds;
      this.m_rectangle.Size = value;
      this.Dictionary.SetProperty("Rect", (IPdfPrimitive) PdfArray.FromRectangle(this.m_rectangle));
    }
  }

  public PdfPage Page => this.m_page;

  internal PdfLoadedPage LoadedPage => this.m_loadedPage;

  public virtual string Text
  {
    get
    {
      if (this.Dictionary.ContainsKey("Contents"))
        this.m_text = (this.Dictionary["Contents"] as PdfString).Value;
      return this.m_text;
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Text));
      if (!(this.m_text != value))
        return;
      this.m_text = value;
      this.Dictionary.SetString("Contents", this.m_text);
    }
  }

  public virtual string Author
  {
    get
    {
      if (this.Dictionary.ContainsKey(nameof (Author)))
        this.m_author = (this.Dictionary[nameof (Author)] as PdfString).Value;
      else if (this.Dictionary.ContainsKey("T"))
        this.m_author = (this.Dictionary["T"] as PdfString).Value;
      return this.m_author;
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Author));
      if (!(this.m_author != value))
        return;
      this.m_author = value;
      this.Dictionary.SetString("T", this.m_author);
      this.isAuthorExplicitSet = true;
    }
  }

  public virtual string Subject
  {
    get
    {
      if (this.Dictionary.ContainsKey(nameof (Subject)))
        this.m_subject = (this.Dictionary[nameof (Subject)] as PdfString).Value;
      else if (this.Dictionary.ContainsKey("Subj"))
        this.m_subject = (this.Dictionary["Subj"] as PdfString).Value;
      return this.m_subject;
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Subject));
      if (!(this.m_subject != value))
        return;
      this.m_subject = value;
      this.Dictionary.SetString("Subj", this.m_subject);
    }
  }

  public virtual DateTime ModifiedDate
  {
    get => this.m_modifiedDate;
    set
    {
      if (!(this.m_modifiedDate != value))
        return;
      this.m_modifiedDate = value;
      this.Dictionary.SetDateTime("M", this.m_modifiedDate);
    }
  }

  public virtual PdfAnnotationFlags AnnotationFlags
  {
    get => this.m_annotationFlags;
    set
    {
      if (this.m_annotationFlags == value)
        return;
      this.m_annotationFlags = value;
      this.m_dictionary.SetNumber("F", (int) this.m_annotationFlags);
    }
  }

  internal PdfDictionary Dictionary
  {
    get => this.m_dictionary;
    set => this.m_dictionary = value;
  }

  public bool Flatten
  {
    get => this.m_flatten;
    set => this.m_flatten = value;
  }

  public bool FlattenPopUps
  {
    get => this.m_flattenPopUps;
    set => this.m_flattenPopUps = value;
  }

  public PdfTag PdfTag
  {
    get => this.m_tag;
    set => this.m_tag = value;
  }

  internal PdfAppearance Appearance
  {
    get
    {
      if (this.m_appearance == null)
      {
        this.m_appearance = new PdfAppearance(this);
        this.m_isStandardAppearance = false;
      }
      return this.m_appearance;
    }
    set
    {
      if (this.m_appearance == value)
        return;
      this.m_appearance = value;
      this.m_isStandardAppearance = false;
    }
  }

  internal bool SetAppearanceDictionary
  {
    get => this.m_setAppearanceDictionary;
    set => this.m_setAppearanceDictionary = value;
  }

  public void SetAppearance(bool appearance) => this.SetAppearanceDictionary = appearance;

  internal PdfPopupAnnotation Popup
  {
    get => this.m_popup;
    set
    {
      this.m_popup = value;
      this.m_popup.Dictionary.SetProperty("Parent", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this));
      this.Dictionary.SetProperty(nameof (Popup), (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_popup));
      if (this.m_popupAnnotations.Contains((PdfAnnotation) this.m_popup))
        return;
      this.m_popupAnnotations.Add((PdfAnnotation) this.m_popup);
    }
  }

  public PdfLayer Layer
  {
    get
    {
      if (this.layer == null)
        this.layer = this.GetDocumentLayer();
      return this.layer;
    }
    set
    {
      if (this.layer != null)
        return;
      this.layer = value;
      if (this.layer != null)
        this.Dictionary.SetProperty("OC", (IPdfPrimitive) this.layer.ReferenceHolder);
      else
        this.Dictionary.Remove("OC");
    }
  }

  public PdfAnnotationRotateAngle Rotate
  {
    get
    {
      this.m_angle = this.GetRotateAngle();
      return this.m_angle;
    }
    set
    {
      this.m_angle = value;
      int num1 = 90;
      int num2 = 360;
      int num3 = (int) this.m_angle * num1;
      if (num3 >= 360)
        num3 %= num2;
      this.Dictionary[nameof (Rotate)] = (IPdfPrimitive) new PdfNumber(num3);
      this.rotationModified = true;
    }
  }

  public string Name
  {
    get
    {
      return this is PdfLoadedStyledAnnotation ? (this as PdfLoadedStyledAnnotation).Name : this.m_name;
    }
    set
    {
      if (value == null || value == string.Empty)
        throw new ArgumentNullException("Name value cannot be null or Empty");
      if (!(this.m_name != value))
        return;
      this.m_name = value;
      this.Dictionary["NM"] = (IPdfPrimitive) new PdfString(this.m_name);
    }
  }

  internal float RotateAngle
  {
    get
    {
      this.rotateAngle = this.GetRotationAngle();
      if ((double) this.rotateAngle < 0.0)
        this.rotateAngle = 360f + this.rotateAngle;
      if ((double) this.rotateAngle >= 360.0)
        this.rotateAngle = 360f - this.rotateAngle;
      return this.rotateAngle;
    }
    set
    {
      if ((double) value == (double) this.GetRotationAngle())
        return;
      if ((double) value < 0.0)
        value = 360f + value;
      if ((double) value >= 360.0)
        value = 360f - value;
      this.rotateAngle = value;
      this.Dictionary.SetProperty("Rotate", (IPdfPrimitive) new PdfNumber(this.rotateAngle));
      this.rotationModified = true;
    }
  }

  internal PdfAnnotation() => this.Initialize();

  protected PdfAnnotation(PdfPageBase page, string text)
  {
    this.Initialize();
    this.m_page = page as PdfPage;
    this.m_text = text;
    this.m_dictionary.SetProperty("Contents", (IPdfPrimitive) new PdfString(text));
  }

  protected PdfAnnotation(RectangleF bounds)
  {
    this.Initialize();
    this.Bounds = bounds;
  }

  internal PdfAnnotation(PdfDictionary dictionary, PdfCrossTable crossTable, RectangleF bounds)
  {
    this.Initialize();
    this.Bounds = bounds;
  }

  internal void SetPage(PdfPageBase page)
  {
    if (page is PdfPage)
    {
      this.m_page = page as PdfPage;
      PdfGraphics graphics = page.Graphics;
      if (this.Dictionary.ContainsKey("Subtype"))
      {
        PdfName pdfName = this.Dictionary.Items[new PdfName("Subtype")] as PdfName;
        if (pdfName != (PdfName) null && (pdfName.Value == "Text" || pdfName.Value == "Square" || this.Flatten))
        {
          this.m_page.Document.Catalog.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
          this.m_page.Document.Catalog.Modify();
        }
      }
      else if (this.Flatten)
      {
        this.m_page.Document.Catalog.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
        this.m_page.Document.Catalog.Modify();
      }
    }
    else if (page is PdfLoadedPage)
    {
      this.m_loadedPage = page as PdfLoadedPage;
      if (this.Dictionary.ContainsKey("Subtype"))
      {
        PdfName pdfName = this.Dictionary.Items[new PdfName("Subtype")] as PdfName;
        if (pdfName != (PdfName) null && (pdfName.Value == "Circle" || pdfName.Value == "Square" || pdfName.Value == "Line" || pdfName.Value == "Polygon" || pdfName.Value == "Ink" || pdfName.Value == "FreeText" || pdfName.Value == "Highlight" || pdfName.Value == "Underline" || pdfName.Value == "StrikeOut" || pdfName.Value == "PolyLine" || pdfName.Value == "Text" || pdfName.Value == "Stamp" || pdfName.Value == "Squiggly" || this.Flatten || pdfName.Value == "Redact"))
        {
          this.LoadedPage.Document.Catalog.BeginSave -= new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
          this.LoadedPage.Document.Catalog.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
          this.LoadedPage.Document.Catalog.Modify();
        }
      }
      else if (this.Flatten)
      {
        this.m_loadedPage.Document.Catalog.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
        this.m_loadedPage.Document.Catalog.Modify();
      }
    }
    if (this.m_page != null)
    {
      this.m_dictionary.SetProperty("P", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_page));
    }
    else
    {
      if (this.LoadedPage == null || this.m_dictionary.ContainsKey("P"))
        return;
      this.m_dictionary.SetProperty("P", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.LoadedPage));
    }
  }

  internal PdfGraphicsUnit GetEqualPdfGraphicsUnit(
    PdfMeasurementUnit measurementUnit,
    out string m_unitString)
  {
    PdfGraphicsUnit equalPdfGraphicsUnit;
    switch (measurementUnit)
    {
      case PdfMeasurementUnit.Inch:
        equalPdfGraphicsUnit = PdfGraphicsUnit.Inch;
        m_unitString = "in";
        break;
      case PdfMeasurementUnit.Pica:
        equalPdfGraphicsUnit = PdfGraphicsUnit.Pica;
        m_unitString = "p";
        break;
      case PdfMeasurementUnit.Point:
        equalPdfGraphicsUnit = PdfGraphicsUnit.Point;
        m_unitString = "pt";
        break;
      case PdfMeasurementUnit.Centimeter:
        equalPdfGraphicsUnit = PdfGraphicsUnit.Centimeter;
        m_unitString = "cm";
        break;
      case PdfMeasurementUnit.Millimeter:
        equalPdfGraphicsUnit = PdfGraphicsUnit.Millimeter;
        m_unitString = "mm";
        break;
      default:
        equalPdfGraphicsUnit = PdfGraphicsUnit.Inch;
        m_unitString = "in";
        break;
    }
    return equalPdfGraphicsUnit;
  }

  internal PdfDictionary CreateMeasureDictioanry(string m_unitString)
  {
    PdfDictionary measureDictioanry = new PdfDictionary();
    PdfArray pdfArray1 = new PdfArray();
    PdfArray pdfArray2 = new PdfArray();
    PdfArray pdfArray3 = new PdfArray();
    PdfArray pdfArray4 = new PdfArray();
    measureDictioanry.Items.Add(new PdfName("A"), (IPdfPrimitive) pdfArray2);
    measureDictioanry.Items.Add(new PdfName("D"), (IPdfPrimitive) pdfArray1);
    measureDictioanry.Items.Add(new PdfName("R"), (IPdfPrimitive) new PdfString($"1 {m_unitString} = 1 {m_unitString}"));
    measureDictioanry.Items.Add(new PdfName("Type"), (IPdfPrimitive) new PdfName("Measure"));
    measureDictioanry.Items.Add(new PdfName("X"), (IPdfPrimitive) pdfArray3);
    pdfArray1.Add((IPdfPrimitive) new PdfDictionary()
    {
      Items = {
        {
          new PdfName("C"),
          (IPdfPrimitive) new PdfNumber(1)
        },
        {
          new PdfName("D"),
          (IPdfPrimitive) new PdfNumber(100)
        },
        {
          new PdfName("F"),
          (IPdfPrimitive) new PdfName("D")
        },
        {
          new PdfName("RD"),
          (IPdfPrimitive) new PdfString(".")
        },
        {
          new PdfName("RT"),
          (IPdfPrimitive) new PdfString("")
        },
        {
          new PdfName("SS"),
          (IPdfPrimitive) new PdfString("")
        },
        {
          new PdfName("U"),
          (IPdfPrimitive) new PdfString(m_unitString)
        }
      }
    });
    pdfArray2.Add((IPdfPrimitive) new PdfDictionary()
    {
      Items = {
        {
          new PdfName("C"),
          (IPdfPrimitive) new PdfNumber(1)
        },
        {
          new PdfName("D"),
          (IPdfPrimitive) new PdfNumber(100)
        },
        {
          new PdfName("F"),
          (IPdfPrimitive) new PdfName("D")
        },
        {
          new PdfName("RD"),
          (IPdfPrimitive) new PdfString(".")
        },
        {
          new PdfName("RT"),
          (IPdfPrimitive) new PdfString("")
        },
        {
          new PdfName("SS"),
          (IPdfPrimitive) new PdfString("")
        },
        {
          new PdfName("U"),
          (IPdfPrimitive) new PdfString("sq " + m_unitString)
        }
      }
    });
    PdfDictionary element = new PdfDictionary();
    switch (m_unitString)
    {
      case "in":
        element.Items.Add(new PdfName("C"), (IPdfPrimitive) new PdfNumber(0.0138889));
        break;
      case "cm":
        element.Items.Add(new PdfName("C"), (IPdfPrimitive) new PdfNumber(0.0352778));
        break;
      case "mm":
        element.Items.Add(new PdfName("C"), (IPdfPrimitive) new PdfNumber(0.352778));
        break;
      case "pt":
        element.Items.Add(new PdfName("C"), (IPdfPrimitive) new PdfNumber(1));
        break;
      case "p":
        element.Items.Add(new PdfName("C"), (IPdfPrimitive) new PdfNumber(0.0833333));
        break;
    }
    element.Items.Add(new PdfName("D"), (IPdfPrimitive) new PdfNumber(100));
    element.Items.Add(new PdfName("F"), (IPdfPrimitive) new PdfName("D"));
    element.Items.Add(new PdfName("RD"), (IPdfPrimitive) new PdfString("."));
    element.Items.Add(new PdfName("RT"), (IPdfPrimitive) new PdfString(""));
    element.Items.Add(new PdfName("SS"), (IPdfPrimitive) new PdfString(""));
    element.Items.Add(new PdfName("U"), (IPdfPrimitive) new PdfString(m_unitString));
    pdfArray3.Add((IPdfPrimitive) element);
    return measureDictioanry;
  }

  internal PdfTemplate CreateNormalAppearance(
    string overlayText,
    PdfFont font,
    bool repeat,
    PdfColor TextColor,
    PdfTextAlignment alignment,
    LineBorder Border)
  {
    this.m_borderWidth = Border.BorderWidth != 1 ? (float) Border.BorderWidth : Border.BorderLineWidth;
    RectangleF rect = new RectangleF(0.0f, 0.0f, this.Bounds.Width, this.Bounds.Height);
    PdfTemplate normalAppearance = new PdfTemplate(rect);
    this.SetMatrix((PdfDictionary) normalAppearance.m_content);
    PaintParams paintParams = new PaintParams();
    PdfGraphics graphics = normalAppearance.Graphics;
    PdfBrush pdfBrush = (PdfBrush) null;
    if (this.InnerColor.A != (byte) 0)
      pdfBrush = (PdfBrush) new PdfSolidBrush(this.InnerColor);
    PdfBrush brush = TextColor.A == (byte) 0 ? (PdfBrush) new PdfSolidBrush(new PdfColor(this.Color.Gray)) : (PdfBrush) new PdfSolidBrush(TextColor);
    float num1 = (float) Border.BorderWidth / 2f;
    paintParams.BackBrush = pdfBrush;
    RectangleF layoutRectangle = new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
    if ((double) this.Opacity < 1.0)
    {
      PdfGraphicsState state = graphics.Save();
      graphics.SetTransparency(this.Opacity);
      FieldPainter.DrawRectangleAnnotation(graphics, paintParams, layoutRectangle.X + num1, layoutRectangle.Y + num1, layoutRectangle.Width - this.m_borderWidth, layoutRectangle.Height - this.m_borderWidth);
      graphics.Restore(state);
    }
    else
      FieldPainter.DrawRectangleAnnotation(graphics, paintParams, layoutRectangle.X + num1, layoutRectangle.Y + num1, layoutRectangle.Width - this.m_borderWidth, layoutRectangle.Height - this.m_borderWidth);
    if (font == null)
      font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10f);
    int int32 = Convert.ToInt32((object) alignment);
    float y = 0.0f;
    float x = 0.0f;
    SizeF sizeF = font.MeasureString(overlayText);
    if (repeat)
    {
      float d = this.Bounds.Width / sizeF.Width;
      float num2 = (float) Math.Floor((double) this.Bounds.Height / (double) font.Size);
      float num3 = Math.Abs(this.Bounds.Width - (float) Math.Floor((double) d) * sizeF.Width);
      if (int32 == 1)
        x = num3 / 2f;
      if (int32 == 2)
        x = num3;
      for (int index1 = 1; (double) index1 < (double) d; ++index1)
      {
        for (int index2 = 0; (double) index2 < (double) num2; ++index2)
        {
          layoutRectangle = new RectangleF(x, y, 0.0f, 0.0f);
          graphics.DrawString(overlayText, font, brush, layoutRectangle);
          y += font.Size;
        }
        x += sizeF.Width;
        y = 0.0f;
      }
    }
    else
    {
      float num4 = Math.Abs(this.Bounds.Width - sizeF.Width);
      if (int32 == 1)
        x = num4 / 2f;
      if (int32 == 2)
        x = num4;
      layoutRectangle = new RectangleF(x, 0.0f, 0.0f, 0.0f);
      graphics.DrawString(overlayText, font, brush, layoutRectangle);
    }
    return normalAppearance;
  }

  internal PdfTemplate CreateBorderAppearance(PdfColor BorderColor, LineBorder Border)
  {
    this.m_borderWidth = Border.BorderWidth != 1 ? (float) Border.BorderWidth : Border.BorderLineWidth;
    RectangleF rect = new RectangleF(0.0f, 0.0f, this.Bounds.Width, this.Bounds.Height);
    PdfTemplate borderAppearance = new PdfTemplate(rect);
    PaintParams paintParams = new PaintParams();
    PdfGraphics graphics = borderAppearance.Graphics;
    if ((double) this.m_borderWidth > 0.0 && BorderColor.A != (byte) 0)
    {
      PdfPen pdfPen = new PdfPen(BorderColor, this.m_borderWidth);
      paintParams.BorderPen = pdfPen;
    }
    float num = this.m_borderWidth / 2f;
    paintParams.ForeBrush = (PdfBrush) new PdfSolidBrush(BorderColor);
    RectangleF rectangleF = new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
    if ((double) this.Opacity < 1.0)
    {
      PdfGraphicsState state = graphics.Save();
      graphics.SetTransparency(this.Opacity);
      FieldPainter.DrawRectangleAnnotation(graphics, paintParams, rectangleF.X + num, rectangleF.Y + num, rectangleF.Width - this.m_borderWidth, rectangleF.Height - this.m_borderWidth);
      graphics.Restore(state);
    }
    else
      FieldPainter.DrawRectangleAnnotation(graphics, paintParams, rectangleF.X + num, rectangleF.Y + num, rectangleF.Width - this.m_borderWidth, rectangleF.Height - this.m_borderWidth);
    return borderAppearance;
  }

  internal string FindOperator(int token)
  {
    return new string[79]
    {
      "b",
      "B",
      "bx",
      "Bx",
      "BDC",
      "BI",
      "BMC",
      "BT",
      "BX",
      "c",
      "cm",
      "CS",
      "cs",
      "d",
      "d0",
      "d1",
      "Do",
      "DP",
      "EI",
      "EMC",
      "ET",
      "EX",
      "f",
      "F",
      "fx",
      "G",
      "g",
      "gs",
      "h",
      "i",
      "ID",
      "j",
      "J",
      "K",
      "k",
      "l",
      "m",
      "M",
      "MP",
      "n",
      "q",
      "Q",
      "re",
      "RG",
      "rg",
      "ri",
      "s",
      "S",
      "SC",
      "sc",
      "SCN",
      "scn",
      "sh",
      "f*",
      "Tx",
      "Tc",
      "Td",
      "TD",
      "Tf",
      "Tj",
      "TJ",
      "TL",
      "Tm",
      "Tr",
      "Ts",
      "Tw",
      "Tz",
      "v",
      "w",
      "W",
      "W*",
      "Wx",
      "y",
      "T*",
      "b*",
      "B*",
      "'",
      "\"",
      "true"
    }.GetValue(token) as string;
  }

  internal string ColorToHex(System.Drawing.Color c)
  {
    return $"#{c.R.ToString("X2")}{c.G.ToString("X2")}{c.B.ToString("X2")}";
  }

  internal void RemoveAnnoationFromPage(PdfPageBase page, PdfAnnotation annot)
  {
    if (page is PdfPage pdfPage)
    {
      pdfPage.Annotations.Remove(annot);
    }
    else
    {
      PdfLoadedPage wrapper = page as PdfLoadedPage;
      PdfDictionary dictionary = wrapper.Dictionary;
      PdfArray primitive = !dictionary.ContainsKey("Annots") ? new PdfArray() : wrapper.CrossTable.GetObject(dictionary["Annots"]) as PdfArray;
      annot.Dictionary.SetProperty("P", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) wrapper));
      primitive.Remove((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) annot));
      page.Dictionary.SetProperty("Annots", (IPdfPrimitive) primitive);
    }
  }

  internal void AssignLocation(PointF location) => this.m_rectangle.Location = location;

  internal virtual void ApplyText(string text)
  {
    this.m_text = text;
    this.Dictionary.SetProperty("Contents", (IPdfPrimitive) new PdfString(text));
  }

  internal void AssignSize(SizeF size) => this.m_rectangle.Size = size;

  protected virtual void Initialize()
  {
    this.m_dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
    this.m_dictionary.SetProperty("Type", (IPdfPrimitive) new PdfName("Annot"));
  }

  internal virtual void Dictionary_BeginSave(object sender, SavePdfPrimitiveEventArgs ars)
  {
    if (!this.IsContainsAnnotation())
      return;
    this.Save();
  }

  protected virtual void Save()
  {
    if ((this.GetType().ToString().Contains("Pdf3DAnnotation") || this.GetType().ToString().Contains("PdfAttachmentAnnotation") || this.GetType().ToString().Contains("PdfSoundAnnotation") || this.GetType().ToString().Contains("PdfActionAnnotation")) && (PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_A1B || PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_A1A || PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_X1A2001))
      throw new PdfConformanceException("The specified annotation type is not supported by PDF/A1-B or PDF/A1-A standard documents.");
    if (this.m_border != null)
      this.m_dictionary.SetProperty("Border", (IPdfWrapper) this.m_border);
    RectangleF nativeRectangle = this.ObtainNativeRectangle();
    if ((double) this.m_innerColor.A != 0.0)
      this.m_dictionary.SetProperty("IC", (IPdfPrimitive) this.m_innerColor.ToArray());
    this.m_dictionary.SetProperty("Rect", (IPdfPrimitive) PdfArray.FromRectangle(nativeRectangle));
    if ((double) this.m_opacity == 1.0)
      return;
    this.Dictionary.SetNumber("CA", this.m_opacity);
  }

  protected RectangleF CalculateBounds(RectangleF Bounds, PdfPage page, PdfLoadedPage loadedpage)
  {
    float x = Bounds.X;
    float y = Bounds.Y;
    float width = Bounds.Width;
    float height = Bounds.Height;
    SizeF sizeF = new SizeF(0.0f, 0.0f);
    PdfNumber pdfNumber = (PdfNumber) null;
    if (page != null)
    {
      PdfMargins margins = page.Document.PageSettings.Margins;
      SizeF size = page.Size;
      if (page.Dictionary.ContainsKey("Rotate"))
      {
        pdfNumber = page.Dictionary["Rotate"] as PdfNumber;
      }
      else
      {
        PdfReferenceHolder pdfReferenceHolder = page.Dictionary["Parent"] as PdfReferenceHolder;
        PdfDictionary pdfDictionary = !(pdfReferenceHolder != (PdfReferenceHolder) null) ? page.Dictionary["Parent"] as PdfDictionary : pdfReferenceHolder.Object as PdfDictionary;
        if (pdfDictionary != null && pdfDictionary.ContainsKey("Rotate"))
          pdfNumber = page.Dictionary.GetValue(page.CrossTable, "Rotate", "Parent") as PdfNumber;
      }
      if (pdfNumber != null)
      {
        if (pdfNumber.IntValue == 90)
        {
          x = Bounds.Y;
          y = size.Height - (margins.Left + margins.Right) - Bounds.X - Bounds.Width;
          width = Bounds.Height;
          height = Bounds.Width;
        }
        else if (pdfNumber.IntValue == 180)
        {
          x = (float) ((double) size.Width - ((double) Bounds.X + (double) Bounds.Width) - ((double) margins.Left + (double) margins.Right));
          y = (float) ((double) size.Height - (double) Bounds.Y - ((double) margins.Top + (double) margins.Bottom)) - Bounds.Height;
        }
        else if (pdfNumber.IntValue == 270)
        {
          x = (float) ((double) size.Width - (double) Bounds.Bottom - ((double) margins.Left + (double) margins.Right));
          y = Bounds.X;
          width = Bounds.Height;
          height = Bounds.Width;
        }
      }
    }
    else if (loadedpage != null)
    {
      SizeF size = loadedpage.Size;
      if (loadedpage.Dictionary.ContainsKey("Rotate"))
      {
        pdfNumber = loadedpage.Dictionary["Rotate"] as PdfNumber;
      }
      else
      {
        PdfReferenceHolder pdfReferenceHolder = loadedpage.Dictionary["Parent"] as PdfReferenceHolder;
        PdfDictionary pdfDictionary = !(pdfReferenceHolder != (PdfReferenceHolder) null) ? loadedpage.Dictionary["Parent"] as PdfDictionary : pdfReferenceHolder.Object as PdfDictionary;
        if (pdfDictionary != null && pdfDictionary.ContainsKey("Rotate"))
          pdfNumber = loadedpage.Dictionary.GetValue(loadedpage.CrossTable, "Rotate", "Parent") as PdfNumber;
      }
      if (pdfNumber != null)
      {
        if (pdfNumber.IntValue == 90)
        {
          x = size.Height - Bounds.Y - Bounds.Height;
          y = Bounds.X;
          width = Bounds.Height;
          height = Bounds.Width;
        }
        else if (pdfNumber.IntValue == 180)
        {
          x = size.Width - (Bounds.X + Bounds.Width);
          y = size.Height - Bounds.Y - Bounds.Height;
        }
        else if (pdfNumber.IntValue == 270)
        {
          x = Bounds.Y;
          y = size.Width - Bounds.X - Bounds.Width;
          width = Bounds.Height;
          height = Bounds.Width;
        }
      }
    }
    return new RectangleF(x, y, width, height);
  }

  internal void DrawAuthor(
    string author,
    string subject,
    RectangleF bounds,
    PdfBrush backBrush,
    PdfBrush aBrush,
    PdfPageBase page,
    out float trackingHeight,
    PdfAnnotationBorder border)
  {
    float num = this.Border.Width / 2f;
    RectangleF rectangle = new RectangleF(bounds.X + num, bounds.Y + num, bounds.Width - border.Width, 20f);
    if (subject != string.Empty && subject != null)
    {
      rectangle.Height += 20f;
      trackingHeight = rectangle.Height;
      this.SaveGraphics(page, PdfBlendMode.HardLight);
      page.Graphics.DrawRectangle(PdfPens.Black, backBrush, rectangle);
      page.Graphics.Restore();
      RectangleF rectangleF = new RectangleF(rectangle.X + 11f, rectangle.Y, rectangle.Width, rectangle.Height / 2f);
      this.SaveGraphics(page, PdfBlendMode.Normal);
      page.Graphics.DrawString(author, (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10.5f, PdfFontStyle.Bold), aBrush, rectangleF, new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle));
      rectangleF = new RectangleF(rectangleF.X, (float) ((double) rectangleF.Y + (double) rectangleF.Height - 2.0), rectangleF.Width, rectangle.Height / 2f);
      this.DrawSubject(subject, rectangleF, page);
      page.Graphics.Restore();
    }
    else
    {
      this.SaveGraphics(page, PdfBlendMode.HardLight);
      page.Graphics.DrawRectangle(PdfPens.Black, backBrush, rectangle);
      page.Graphics.Restore();
      RectangleF layoutRectangle = new RectangleF(rectangle.X + 11f, rectangle.Y, rectangle.Width, rectangle.Height);
      this.SaveGraphics(page, PdfBlendMode.Normal);
      page.Graphics.DrawString(author, (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10.5f), aBrush, layoutRectangle, new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle));
      trackingHeight = rectangle.Height;
      page.Graphics.Restore();
    }
  }

  internal void DrawSubject(string subject, RectangleF bounds, PdfPageBase page)
  {
    page.Graphics.DrawString(subject, (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10.5f, PdfFontStyle.Bold), PdfBrushes.Black, bounds, new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle));
  }

  internal void FlattenPopup()
  {
    if (this.Page != null)
    {
      this.FlattenPopup((PdfPageBase) this.Page, this.Color, this.Bounds, this.Border, this.Author, this.Subject, this.Text);
    }
    else
    {
      if (this.LoadedPage == null)
        return;
      this.FlattenPopup((PdfPageBase) this.LoadedPage, this.Color, this.Bounds, this.Border, this.Author, this.Subject, this.Text);
    }
  }

  internal void FlattenPopup(
    PdfPageBase page,
    PdfColor color,
    RectangleF annotBounds,
    PdfAnnotationBorder border,
    string author,
    string subject,
    string text)
  {
    SizeF empty = SizeF.Empty;
    SizeF sizeF = !(page is PdfLoadedPage) ? (page as PdfPage).GetClientSize() : (page as PdfLoadedPage).Size;
    RectangleF bounds1 = new RectangleF(sizeF.Width - 180f, (double) annotBounds.Y + 142.0 < (double) sizeF.Height ? annotBounds.Y : sizeF.Height - 142f, 180f, 142f);
    if (this.Dictionary["Popup"] != null && PdfCrossTable.Dereference(this.Dictionary["Popup"]) is PdfDictionary pdfDictionary)
    {
      PdfArray pdfArray = PdfCrossTable.Dereference(pdfDictionary["Rect"]) as PdfArray;
      PdfCrossTable pdfCrossTable = page is PdfPage ? (page as PdfPage).CrossTable : (page as PdfLoadedPage).CrossTable;
      if (pdfArray != null)
      {
        PdfNumber pdfNumber1 = pdfCrossTable.GetObject(pdfArray[0]) as PdfNumber;
        PdfNumber pdfNumber2 = pdfCrossTable.GetObject(pdfArray[1]) as PdfNumber;
        PdfNumber pdfNumber3 = pdfCrossTable.GetObject(pdfArray[2]) as PdfNumber;
        PdfNumber pdfNumber4 = pdfCrossTable.GetObject(pdfArray[3]) as PdfNumber;
        bounds1 = new RectangleF(pdfNumber1.FloatValue, pdfNumber2.FloatValue, pdfNumber3.FloatValue - pdfNumber1.FloatValue, pdfNumber4.FloatValue - pdfNumber2.FloatValue);
      }
    }
    PdfBrush pdfBrush = (PdfBrush) new PdfSolidBrush(color);
    float num = border.Width / 2f;
    float trackingHeight = 0.0f;
    PdfBrush aBrush = (PdfBrush) new PdfSolidBrush(this.GetForeColor(color));
    if (author != null && author != string.Empty)
      this.DrawAuthor(author, subject, bounds1, pdfBrush, aBrush, page, out trackingHeight, border);
    else if (subject != null && subject != string.Empty)
    {
      RectangleF rectangle = new RectangleF(bounds1.X + num, bounds1.Y + num, bounds1.Width - border.Width, 40f);
      this.SaveGraphics(page, PdfBlendMode.HardLight);
      page.Graphics.DrawRectangle(PdfPens.Black, pdfBrush, rectangle);
      page.Graphics.Restore();
      RectangleF bounds2 = new RectangleF(rectangle.X + 11f, rectangle.Y, rectangle.Width, rectangle.Height / 2f);
      bounds2 = new RectangleF(bounds2.X, (float) ((double) bounds2.Y + (double) bounds2.Height - 2.0), bounds2.Width, rectangle.Height / 2f);
      this.SaveGraphics(page, PdfBlendMode.Normal);
      this.DrawSubject(this.Subject, bounds2, page);
      page.Graphics.Restore();
      trackingHeight = 40f;
    }
    else
    {
      this.SaveGraphics(page, PdfBlendMode.HardLight);
      RectangleF rectangle = new RectangleF(bounds1.X + num, bounds1.Y + num, bounds1.Width - border.Width, 20f);
      page.Graphics.DrawRectangle(PdfPens.Black, pdfBrush, rectangle);
      trackingHeight = 20f;
      page.Graphics.Restore();
    }
    RectangleF rectangleF = new RectangleF(bounds1.X + num, bounds1.Y + num + trackingHeight, bounds1.Width - border.Width, bounds1.Height - (trackingHeight + border.Width));
    this.SaveGraphics(page, PdfBlendMode.HardLight);
    page.Graphics.DrawRectangle(PdfPens.Black, PdfBrushes.White, rectangleF);
    rectangleF.X += 11f;
    rectangleF.Y += 5f;
    rectangleF.Width -= 22f;
    page.Graphics.Restore();
    this.SaveGraphics(page, PdfBlendMode.Normal);
    page.Graphics.DrawString(text, (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10.5f), PdfBrushes.Black, rectangleF);
    page.Graphics.Restore();
  }

  internal void SaveGraphics(PdfPageBase page, PdfBlendMode blendMode)
  {
    page.Graphics.Save();
    page.Graphics.SetTransparency(0.8f, 0.8f, blendMode);
  }

  internal PdfColor GetForeColor(PdfColor c)
  {
    return (PdfColor) (((int) c.R + (int) c.B + (int) c.G) / 3 > 128 /*0x80*/ ? System.Drawing.Color.FromArgb((int) byte.MaxValue, 0, 0, 0) : System.Drawing.Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
  }

  public void SetValues(string key, string value)
  {
    if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
      return;
    this.Dictionary.SetProperty(key, (IPdfPrimitive) new PdfString(value));
  }

  protected PointF CalculateTemplateBounds(
    RectangleF Bounds,
    PdfPageBase page,
    PdfTemplate template)
  {
    PdfArray pdfArray1 = (PdfArray) null;
    PdfArray pdfArray2 = (PdfArray) null;
    PdfNumber pdfNumber = (PdfNumber) null;
    float x = Bounds.X;
    float y = Bounds.Y;
    if (page != null)
    {
      if (page.Dictionary.ContainsKey("CropBox"))
        pdfArray1 = page.Dictionary["CropBox"] as PdfArray;
      else if (page.Dictionary.ContainsKey("MediaBox"))
        pdfArray2 = page.Dictionary["MediaBox"] as PdfArray;
      if (page.Dictionary.ContainsKey("Rotate"))
      {
        pdfNumber = page.Dictionary["Rotate"] as PdfNumber;
      }
      else
      {
        PdfReferenceHolder pdfReferenceHolder = page.Dictionary["Parent"] as PdfReferenceHolder;
        PdfDictionary pdfDictionary = !(pdfReferenceHolder != (PdfReferenceHolder) null) ? page.Dictionary["Parent"] as PdfDictionary : pdfReferenceHolder.Object as PdfDictionary;
        if (pdfDictionary != null && pdfDictionary.ContainsKey("Rotate"))
          pdfNumber = pdfDictionary["Rotate"] as PdfNumber;
      }
      if (pdfNumber != null)
      {
        if (pdfNumber.IntValue == 90)
        {
          page.Graphics.TranslateTransform(template.Height, 0.0f);
          page.Graphics.RotateTransform(90f);
          x = Bounds.X;
          y = (float) -((double) page.Size.Height - (double) Bounds.Y - (double) Bounds.Height);
        }
        else if (pdfNumber.IntValue == 180)
        {
          page.Graphics.TranslateTransform(template.Width, template.Height);
          page.Graphics.RotateTransform(180f);
          x = (float) -((double) page.Size.Width - ((double) Bounds.X + (double) Bounds.Width));
          y = (float) -((double) page.Size.Height - (double) Bounds.Y - (double) Bounds.Height);
        }
        else if (pdfNumber.IntValue == 270)
        {
          page.Graphics.TranslateTransform(0.0f, template.Width);
          page.Graphics.RotateTransform(270f);
          x = (float) -((double) page.Size.Width - (double) Bounds.X - (double) Bounds.Width);
          y = Bounds.Y;
        }
      }
      if (pdfArray1 != null)
      {
        if ((double) (pdfArray1[0] as PdfNumber).FloatValue != 0.0 || (double) (pdfArray1[1] as PdfNumber).FloatValue != 0.0)
        {
          x -= (pdfArray1[0] as PdfNumber).FloatValue;
          y += (pdfArray1[1] as PdfNumber).FloatValue;
        }
      }
      else if (pdfArray2 != null && ((double) (pdfArray2[0] as PdfNumber).FloatValue != 0.0 || (double) (pdfArray2[1] as PdfNumber).FloatValue != 0.0))
      {
        x -= (pdfArray2[0] as PdfNumber).FloatValue;
        y += (pdfArray2[1] as PdfNumber).FloatValue;
      }
    }
    return new PointF(x, y);
  }

  private PdfLayer GetDocumentLayer()
  {
    if (this.Dictionary.ContainsKey("OC"))
    {
      IPdfPrimitive expectedObject = this.Dictionary["OC"];
      PdfLoadedPage loadedPage = this.m_loadedPage;
      if (expectedObject != null && loadedPage != null && loadedPage.Document != null)
      {
        PdfDocumentLayerCollection layers = loadedPage.Document.Layers;
        if (layers != null)
          this.IsMatched(layers, expectedObject, loadedPage);
      }
    }
    return this.layer;
  }

  private void IsMatched(
    PdfDocumentLayerCollection layerCollection,
    IPdfPrimitive expectedObject,
    PdfLoadedPage page)
  {
    for (int index = 0; index < layerCollection.Count; ++index)
    {
      IPdfPrimitive referenceHolder = (IPdfPrimitive) layerCollection[index].ReferenceHolder;
      if (referenceHolder != null && referenceHolder.Equals((object) expectedObject))
      {
        if (layerCollection[index].Name != null)
        {
          this.layer = layerCollection[index];
          break;
        }
      }
      else if (layerCollection[index].Layers != null && layerCollection[index].Layers.Count > 0)
        this.IsMatched(layerCollection[index].Layers, expectedObject, page);
    }
  }

  internal float GetRotationAngle()
  {
    PdfNumber pdfNumber = (PdfNumber) null;
    if (this.Dictionary.ContainsKey("Rotate"))
      pdfNumber = PdfCrossTable.Dereference(this.Dictionary["Rotate"]) as PdfNumber;
    else if (this.Dictionary.ContainsKey("Rotation"))
      pdfNumber = PdfCrossTable.Dereference(this.Dictionary["Rotation"]) as PdfNumber;
    if (pdfNumber == null)
      pdfNumber = new PdfNumber(0);
    return pdfNumber.FloatValue;
  }

  internal RectangleF GetRotatedBounds(RectangleF bounds, float rotateangle)
  {
    if ((double) bounds.Width <= 0.0 || (double) bounds.Height <= 0.0)
      return bounds;
    PdfTransformationMatrix transformationMatrix = new PdfTransformationMatrix();
    transformationMatrix.Rotate(rotateangle);
    PointF[] pointFArray = new PointF[4]
    {
      new PointF(bounds.Left, bounds.Top),
      new PointF(bounds.Right, bounds.Top),
      new PointF(bounds.Right, bounds.Bottom),
      new PointF(bounds.Left, bounds.Bottom)
    };
    transformationMatrix.Matrix.TransformPoints(pointFArray);
    PdfPath pdfPath = new PdfPath();
    pdfPath.AddRectangle(bounds);
    for (int index = 0; index < 4; ++index)
      pdfPath.PathPoints[index] = pointFArray[index];
    return PdfAnnotation.CalculateBoundingBox(pointFArray) with
    {
      X = bounds.X,
      Y = bounds.Y
    };
  }

  internal static RectangleF CalculateBoundingBox(PointF[] imageCoordinates)
  {
    float x1 = imageCoordinates[0].X;
    float x2 = imageCoordinates[3].X;
    float y1 = imageCoordinates[0].Y;
    float y2 = imageCoordinates[3].Y;
    for (int index = 0; index < 4; ++index)
    {
      if ((double) imageCoordinates[index].X < (double) x1)
        x1 = imageCoordinates[index].X;
      if ((double) imageCoordinates[index].X > (double) x2)
        x2 = imageCoordinates[index].X;
      if ((double) imageCoordinates[index].Y < (double) y1)
        y1 = imageCoordinates[index].Y;
      if ((double) imageCoordinates[index].Y > (double) y2)
        y2 = imageCoordinates[index].Y;
    }
    return new RectangleF(x1, y1, x2 - x1, y2 - y1);
  }

  internal PdfTransformationMatrix GetRotatedTransformMatrix(RectangleF bounds, float angle)
  {
    PdfTransformationMatrix rotatedTransformMatrix = new PdfTransformationMatrix();
    rotatedTransformMatrix.Rotate(angle);
    return rotatedTransformMatrix;
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;

  private PdfAnnotationRotateAngle GetRotateAngle()
  {
    int num = 90;
    PdfNumber pdfNumber = (PdfNumber) null;
    if (this.Dictionary.ContainsKey("Rotate"))
      pdfNumber = PdfCrossTable.Dereference(this.Dictionary["Rotate"]) as PdfNumber;
    if (pdfNumber == null)
      pdfNumber = new PdfNumber(0);
    if (pdfNumber.IntValue < 0)
      pdfNumber.IntValue = 360 + pdfNumber.IntValue;
    return (PdfAnnotationRotateAngle) (pdfNumber.IntValue / num);
  }

  internal bool ValidateTemplateMatrix(PdfDictionary dictionary)
  {
    bool flag = false;
    if (dictionary.ContainsKey("Matrix"))
    {
      if (PdfCrossTable.Dereference(dictionary["Matrix"]) is PdfArray pdfArray1 && pdfArray1.Count > 3 && pdfArray1[0] != null && pdfArray1[1] != null && pdfArray1[2] != null && pdfArray1[3] != null && (double) (pdfArray1[0] as PdfNumber).FloatValue == 1.0 && (double) (pdfArray1[1] as PdfNumber).FloatValue == 0.0 && (double) (pdfArray1[2] as PdfNumber).FloatValue == 0.0 && (double) (pdfArray1[3] as PdfNumber).FloatValue == 1.0)
      {
        flag = true;
        float num1 = 0.0f;
        float num2 = 0.0f;
        float num3 = 0.0f;
        float num4 = 0.0f;
        PdfArray pdfArray = (PdfArray) null;
        if (this.Dictionary.ContainsKey("Rect"))
          pdfArray = PdfCrossTable.Dereference(this.Dictionary["Rect"]) as PdfArray;
        if (pdfArray1.Count > 4)
        {
          num3 = -(pdfArray1[4] as PdfNumber).FloatValue;
          num4 = -(pdfArray1[5] as PdfNumber).FloatValue;
        }
        if (pdfArray != null && pdfArray.Count > 1)
        {
          num1 = (pdfArray[0] as PdfNumber).FloatValue;
          num2 = (pdfArray[1] as PdfNumber).FloatValue;
        }
        if (((double) num1 != (double) num3 || (double) num2 != (double) num4) && (double) num3 == 0.0 && (double) num4 == 0.0)
          this.m_locationDisplaced = true;
      }
    }
    else
      flag = true;
    return flag;
  }

  internal int ObtainGraphicsRotation(PdfTransformationMatrix matrix)
  {
    int graphicsRotation = (int) Math.Round(Math.Atan2((double) matrix.Matrix.Elements[2], (double) matrix.Matrix.Elements[0]) * 180.0 / Math.PI);
    switch (graphicsRotation)
    {
      case -180:
        graphicsRotation = 180;
        break;
      case -90:
        graphicsRotation = 90;
        break;
      case 90:
        graphicsRotation = 270;
        break;
    }
    return graphicsRotation;
  }

  protected RectangleF CalculateTemplateBounds(
    RectangleF bounds,
    PdfPageBase page,
    PdfTemplate template,
    bool isNormalMatrix,
    PdfGraphics graphics)
  {
    RectangleF rectangleF = bounds;
    float x = bounds.X;
    float y = bounds.Y;
    float width = bounds.Width;
    float height = bounds.Height;
    if (!isNormalMatrix && PdfCrossTable.Dereference(this.Dictionary["Rect"]) is PdfArray pdfArray1)
      rectangleF = pdfArray1.ToRectangle();
    if (page != null)
    {
      int graphicsRotation = this.ObtainGraphicsRotation(graphics.Matrix);
      if (page is PdfPage)
      {
        if (graphicsRotation == 0 && !isNormalMatrix && (this.Rotate == PdfAnnotationRotateAngle.RotateAngle90 || this.Rotate == PdfAnnotationRotateAngle.RotateAngle270))
        {
          x = bounds.X;
          y = bounds.Y + bounds.Height - bounds.Width;
          width = bounds.Height;
          height = bounds.Width;
        }
      }
      else if (page is PdfLoadedPage)
      {
        switch (graphicsRotation)
        {
          case 0:
            if (!isNormalMatrix && (this.Rotate == PdfAnnotationRotateAngle.RotateAngle90 || this.Rotate == PdfAnnotationRotateAngle.RotateAngle270))
            {
              x = bounds.X;
              y = bounds.Y + bounds.Height - bounds.Width;
              width = bounds.Height;
              height = bounds.Width;
              break;
            }
            break;
          case 90:
            graphics.TranslateTransform(template.Height, 0.0f);
            graphics.RotateTransform(90f);
            if (isNormalMatrix || this.Rotate == PdfAnnotationRotateAngle.RotateAngle180)
            {
              x = bounds.X;
              y = this.m_locationDisplaced ? (float) -((double) page.Size.Height - ((double) bounds.Height + (double) bounds.Y) + ((double) bounds.Height - (double) template.Height)) : (float) -((double) page.Size.Height - (double) bounds.Y - (double) bounds.Height);
              break;
            }
            x = bounds.X;
            y = (float) -((double) page.Size.Height - ((double) bounds.Height + (double) bounds.Y) + ((double) bounds.Width - (double) template.Height));
            width = bounds.Height;
            height = bounds.Width;
            break;
          case 180:
            graphics.TranslateTransform(template.Width, template.Height);
            graphics.RotateTransform(180f);
            if (isNormalMatrix)
            {
              x = (float) -((double) page.Size.Width - ((double) bounds.X + (double) bounds.Width));
              y = (float) -((double) page.Size.Height - (double) bounds.Y - (double) bounds.Height);
              break;
            }
            x = (float) -((double) page.Size.Width - ((double) bounds.X + (double) template.Width));
            y = (float) -((double) page.Size.Height - (double) bounds.Y - (double) template.Height);
            if (this.Rotate == PdfAnnotationRotateAngle.RotateAngle90 || this.Rotate == PdfAnnotationRotateAngle.RotateAngle270)
            {
              y = (float) (-((double) page.Size.Height - (double) bounds.Y - (double) template.Height) - ((double) bounds.Width - (double) bounds.Height));
              width = bounds.Height;
              height = bounds.Width;
              break;
            }
            break;
          case 270:
            graphics.TranslateTransform(0.0f, template.Width);
            graphics.RotateTransform(270f);
            if (isNormalMatrix || this.Rotate == PdfAnnotationRotateAngle.RotateAngle180)
            {
              x = (float) -((double) page.Size.Width - (double) bounds.X - (double) bounds.Width);
              y = bounds.Y;
              break;
            }
            bool flag = false;
            PdfArray pdfArray2 = PdfCrossTable.Dereference(template.m_content["Matrix"]) as PdfArray;
            PdfArray pdfArray3 = PdfCrossTable.Dereference(template.m_content["BBox"]) as PdfArray;
            if (pdfArray2 != null && pdfArray3 != null && (double) (pdfArray2.Elements[5] as PdfNumber).FloatValue != (double) (pdfArray3.Elements[2] as PdfNumber).FloatValue)
              flag = true;
            x = (float) -((double) page.Size.Width - (double) rectangleF.X - (double) template.Width);
            y = !flag ? bounds.Y + bounds.Height - bounds.Width : bounds.Y - (bounds.Height - bounds.Width);
            width = bounds.Height;
            height = bounds.Width;
            break;
        }
      }
    }
    return new RectangleF(x, y, width, height);
  }

  protected RectangleF CalculateTemplateBounds(
    RectangleF bounds,
    PdfPageBase page,
    PdfTemplate template,
    bool isNormalMatrix)
  {
    return this.CalculateTemplateBounds(bounds, page, template, isNormalMatrix, page.Graphics);
  }

  internal void SetMatrix(PdfDictionary template)
  {
    if (!(template["BBox"] is PdfArray pdfArray))
      return;
    float[] array = new float[6]
    {
      1f,
      0.0f,
      0.0f,
      1f,
      -(pdfArray[0] as PdfNumber).FloatValue,
      -(pdfArray[1] as PdfNumber).FloatValue
    };
    template["Matrix"] = (IPdfPrimitive) new PdfArray(array);
  }

  private RectangleF ObtainNativeRectangle()
  {
    RectangleF nativeRectangle = new RectangleF(this.m_rectangle.X, this.m_rectangle.Bottom, this.m_rectangle.Width, this.m_rectangle.Height);
    PdfArray cropOrMediaBox = (PdfArray) null;
    SizeF sizeF = new SizeF(0.0f, 0.0f);
    if (this.m_page != null)
    {
      PdfSection section = this.m_page.Section;
      nativeRectangle.Location = section.PointToNativePdf(this.Page, nativeRectangle.Location);
      cropOrMediaBox = this.GetCropOrMediaBox((PdfPageBase) this.m_page, cropOrMediaBox);
    }
    else if (this.m_loadedPage != null)
    {
      SizeF size = this.m_loadedPage.Size;
      nativeRectangle.Y = size.Height - this.m_rectangle.Bottom;
      cropOrMediaBox = this.GetCropOrMediaBox((PdfPageBase) this.m_loadedPage, cropOrMediaBox);
    }
    if (cropOrMediaBox != null && cropOrMediaBox.Count > 2 && cropOrMediaBox[0] != null && cropOrMediaBox[1] != null && ((double) (cropOrMediaBox[0] as PdfNumber).FloatValue != 0.0 || (double) (cropOrMediaBox[1] as PdfNumber).FloatValue != 0.0))
    {
      nativeRectangle.X += (cropOrMediaBox[0] as PdfNumber).FloatValue;
      nativeRectangle.Y += (cropOrMediaBox[1] as PdfNumber).FloatValue;
    }
    return nativeRectangle;
  }

  private bool IsContainsAnnotation()
  {
    bool flag = false;
    if (this.Page != null && this.Page.Dictionary.ContainsKey("Annots"))
    {
      if (PdfCrossTable.Dereference(this.Page.Dictionary["Annots"]) is PdfArray pdfArray1 && pdfArray1.Contains((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this)))
        flag = true;
    }
    else if (this.LoadedPage != null && this.LoadedPage.Dictionary.ContainsKey("Annots") && PdfCrossTable.Dereference(this.LoadedPage.Dictionary["Annots"]) is PdfArray pdfArray2 && pdfArray2.Contains((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this)))
      flag = true;
    if (flag)
      this.AddPopUpAnnotation();
    return flag;
  }

  private PdfArray GetCropOrMediaBox(PdfPageBase page, PdfArray cropOrMediaBox)
  {
    if (page.Dictionary.ContainsKey("CropBox"))
      cropOrMediaBox = PdfCrossTable.Dereference(page.Dictionary["CropBox"]) as PdfArray;
    else if (page.Dictionary.ContainsKey("MediaBox"))
      cropOrMediaBox = PdfCrossTable.Dereference(page.Dictionary["MediaBox"]) as PdfArray;
    return cropOrMediaBox;
  }

  internal PdfMargins ObtainMargin()
  {
    if (this.Page != null && this.Page.Section != null && this.Page.Section.PageSettings != null && this.Page.Section.PageSettings.Margins != null)
      this.m_margins = this.Page.Section.PageSettings.Margins;
    return this.m_margins;
  }

  protected void CheckFlatten()
  {
    if (this.LoadedPage == null)
      return;
    PdfLoadedAnnotationCollection annotations1 = ((PdfPageBase) this.LoadedPage).Annotations;
    PdfLoadedAnnotationCollection annotations2 = this.LoadedPage.Annotations;
    if (annotations2 == null || annotations1 == null || annotations2.Count <= 0 && annotations1.Count <= 0 || !annotations1.Flatten && !annotations2.Flatten)
      return;
    this.Flatten = true;
  }

  internal double GetAngle(float x1, float y1, float x2, float y2)
  {
    double angle = Math.Atan(((double) y2 - (double) y1) / ((double) x2 - (double) x1)) * (180.0 / Math.PI);
    if ((double) x2 - (double) x1 < 0.0 || (double) y2 - (double) y1 < 0.0)
      angle += 180.0;
    if ((double) x2 - (double) x1 > 0.0 && (double) y2 - (double) y1 < 0.0)
      angle -= 180.0;
    if (angle < 0.0)
      angle += 360.0;
    return angle;
  }

  internal RectangleF CalculateLineBounds(
    float[] linePoints,
    int m_leaderLineExt,
    int m_leaderLine,
    int leaderOffset,
    PdfArray lineStyle,
    double borderLength)
  {
    RectangleF bounds = this.Bounds;
    PdfPath pdfPath = new PdfPath();
    if (linePoints != null && linePoints.Length == 4)
    {
      float linePoint1 = linePoints[0];
      float linePoint2 = linePoints[1];
      float linePoint3 = linePoints[2];
      float linePoint4 = linePoints[3];
      double num1 = (double) linePoint3 - (double) linePoint1 != 0.0 ? this.GetAngle(linePoint1, linePoint2, linePoint3, linePoint4) : ((double) linePoint4 <= (double) linePoint2 ? 270.0 : 90.0);
      int num2;
      double num3;
      if (m_leaderLine < 0)
      {
        num2 = -m_leaderLine;
        num3 = num1 + 180.0;
      }
      else
      {
        num2 = m_leaderLine;
        num3 = num1;
      }
      float[] numArray1 = new float[2]
      {
        linePoint1,
        linePoint2
      };
      float[] numArray2 = new float[2]
      {
        linePoint3,
        linePoint4
      };
      if (leaderOffset != 0)
      {
        float[] axisValue1 = this.GetAxisValue(numArray1, num3 + 90.0, (double) leaderOffset);
        float[] axisValue2 = this.GetAxisValue(numArray2, num3 + 90.0, (double) leaderOffset);
        linePoints[0] = (float) (int) axisValue1[0];
        linePoints[1] = (float) (int) axisValue1[1];
        linePoints[2] = (float) (int) axisValue2[0];
        linePoints[3] = (float) (int) axisValue2[1];
      }
      float[] axisValue3 = this.GetAxisValue(numArray1, num3 + 90.0, (double) (num2 + leaderOffset));
      float[] axisValue4 = this.GetAxisValue(numArray2, num3 + 90.0, (double) (num2 + leaderOffset));
      float[] axisValue5 = this.GetAxisValue(numArray1, num3 + 90.0, (double) (m_leaderLineExt + num2 + leaderOffset));
      float[] axisValue6 = this.GetAxisValue(numArray2, num3 + 90.0, (double) (m_leaderLineExt + num2 + leaderOffset));
      List<PointF> pointFList = new List<PointF>();
      for (int index = 0; index < lineStyle.Count; ++index)
      {
        PdfName pdfName = lineStyle[index] as PdfName;
        PointF pointF = new PointF();
        if (pdfName != (PdfName) null)
        {
          switch (pdfName.Value.ToString())
          {
            case "Square":
            case "Circle":
            case "Diamond":
              pointF.X = 3f;
              pointF.Y = 3f;
              break;
            case "OpenArrow":
            case "ClosedArrow":
              pointF.X = 1f;
              pointF.Y = 5f;
              break;
            case "ROpenArrow":
            case "RClosedArrow":
              pointF.X = 9f + (float) (borderLength / 2.0);
              pointF.Y = 5f + (float) (borderLength / 2.0);
              break;
            case "Slash":
              pointF.X = 5f;
              pointF.Y = 9f;
              break;
            case "Butt":
              pointF.X = 1f;
              pointF.Y = 3f;
              break;
            default:
              pointF.X = 0.0f;
              pointF.Y = 0.0f;
              break;
          }
        }
        pointFList.Add(pointF);
      }
      float[] numArray3 = new float[2];
      float[] numArray4 = new float[2];
      if (num3 >= 45.0 && num3 <= 135.0 || num3 >= 225.0 && num3 <= 315.0)
      {
        numArray3[0] = pointFList[0].Y;
        numArray4[0] = pointFList[0].X;
        numArray3[1] = pointFList[1].Y;
        numArray4[1] = pointFList[1].X;
      }
      else
      {
        numArray3[0] = pointFList[0].X;
        numArray4[0] = pointFList[0].Y;
        numArray3[1] = pointFList[1].X;
        numArray4[1] = pointFList[1].Y;
      }
      float num4 = Math.Max(numArray3[0], numArray3[1]);
      float num5 = Math.Max(numArray4[0], numArray4[1]);
      if ((double) num4 == 0.0)
        num4 = 1f;
      if ((double) num5 == 0.0)
        num5 = 1f;
      if ((double) axisValue3[0] == (double) Math.Min(axisValue3[0], axisValue4[0]))
      {
        axisValue3[0] -= num4 * (float) borderLength;
        axisValue4[0] += num4 * (float) borderLength;
        axisValue3[0] = Math.Min(axisValue3[0], linePoints[0]);
        axisValue3[0] = Math.Min(axisValue3[0], axisValue5[0]);
        axisValue4[0] = Math.Max(axisValue4[0], linePoints[2]);
        axisValue4[0] = Math.Max(axisValue4[0], axisValue6[0]);
      }
      else
      {
        axisValue3[0] += num4 * (float) borderLength;
        axisValue4[0] -= num4 * (float) borderLength;
        axisValue3[0] = Math.Max(axisValue3[0], linePoints[0]);
        axisValue3[0] = Math.Max(axisValue3[0], axisValue5[0]);
        axisValue4[0] = Math.Min(axisValue4[0], linePoints[2]);
        axisValue4[0] = Math.Min(axisValue4[0], axisValue6[0]);
      }
      if ((double) axisValue3[1] == (double) Math.Min(axisValue3[1], axisValue4[1]))
      {
        axisValue3[1] -= num5 * (float) borderLength;
        axisValue4[1] += num5 * (float) borderLength;
        axisValue3[1] = Math.Min(axisValue3[1], linePoints[1]);
        axisValue3[1] = Math.Min(axisValue3[1], axisValue5[1]);
        axisValue4[1] = Math.Max(axisValue4[1], linePoints[3]);
        axisValue4[1] = Math.Max(axisValue4[1], axisValue6[1]);
      }
      else
      {
        axisValue3[1] += num5 * (float) borderLength;
        axisValue4[1] -= num5 * (float) borderLength;
        axisValue3[1] = Math.Max(axisValue3[1], linePoints[1]);
        axisValue3[1] = Math.Max(axisValue3[1], axisValue5[1]);
        axisValue4[1] = Math.Min(axisValue4[1], linePoints[3]);
        axisValue4[1] = Math.Min(axisValue4[1], axisValue6[1]);
      }
      pdfPath.AddLine(axisValue3[0], axisValue3[1], axisValue4[0], axisValue4[1]);
      bounds = pdfPath.GetBounds();
    }
    return bounds;
  }

  internal PdfGraphics GetLayerGraphics()
  {
    PdfGraphics layerGraphics = (PdfGraphics) null;
    if (this.Layer != null)
    {
      PdfLayer layer = this.Layer;
      if (layer.LayerId == null)
        layer.LayerId = "OCG_" + Guid.NewGuid().ToString();
      PdfPageBase page = this.Page != null ? (PdfPageBase) this.Page : (PdfPageBase) this.LoadedPage;
      layerGraphics = layer.CreateGraphics(page);
    }
    return layerGraphics;
  }

  internal float[] GetAxisValue(float[] value, double angle, double length)
  {
    double num = Math.PI / 180.0;
    return new float[2]
    {
      value[0] + (float) (Math.Cos(angle * num) * length),
      value[1] + (float) (Math.Sin(angle * num) * length)
    };
  }

  internal void SetLineEndingStyles(
    float[] startingPoint,
    float[] endingPoint,
    PdfGraphics graphics,
    double angle,
    PdfPen m_borderPen,
    PdfBrush m_backBrush,
    PdfArray lineStyle,
    double borderLength)
  {
    float[] numArray1 = new float[2];
    if (borderLength == 0.0)
    {
      borderLength = 1.0;
      m_borderPen = (PdfPen) null;
    }
    if (m_backBrush is PdfSolidBrush && (m_backBrush as PdfSolidBrush).Color.IsEmpty)
      m_backBrush = (PdfBrush) null;
    for (int index = 0; index < lineStyle.Count; ++index)
    {
      PdfName pdfName = lineStyle[index] as PdfName;
      float[] numArray2 = index != 0 ? endingPoint : startingPoint;
      if (pdfName != (PdfName) null)
      {
        switch (pdfName.Value.ToString())
        {
          case "Square":
            RectangleF rectangle1 = new RectangleF(numArray2[0] - (float) (3.0 * borderLength), (float) -((double) numArray2[1] + 3.0 * borderLength), (float) (6.0 * borderLength), (float) (6.0 * borderLength));
            graphics.DrawRectangle(m_borderPen, m_backBrush, rectangle1);
            continue;
          case "Circle":
            RectangleF rectangle2 = new RectangleF(numArray2[0] - (float) (3.0 * borderLength), (float) -((double) numArray2[1] + 3.0 * borderLength), (float) (6.0 * borderLength), (float) (6.0 * borderLength));
            graphics.DrawEllipse(m_borderPen, m_backBrush, rectangle2);
            continue;
          case "OpenArrow":
            int num1 = index != 0 ? 150 : 30;
            double length1 = 9.0 * borderLength;
            float[] numArray3 = index != 0 ? this.GetAxisValue(numArray2, angle, -borderLength) : this.GetAxisValue(numArray2, angle, borderLength);
            float[] axisValue1 = this.GetAxisValue(numArray3, angle + (double) num1, length1);
            float[] axisValue2 = this.GetAxisValue(numArray3, angle - (double) num1, length1);
            PdfPath path1 = new PdfPath(m_borderPen);
            path1.AddLine(numArray3[0], -numArray3[1], axisValue1[0], -axisValue1[1]);
            path1.AddLine(numArray3[0], -numArray3[1], axisValue2[0], -axisValue2[1]);
            graphics.DrawPath(m_borderPen, path1);
            continue;
          case "ClosedArrow":
            int num2 = index != 0 ? 150 : 30;
            double length2 = 9.0 * borderLength;
            float[] numArray4 = index != 0 ? this.GetAxisValue(numArray2, angle, -borderLength) : this.GetAxisValue(numArray2, angle, borderLength);
            float[] axisValue3 = this.GetAxisValue(numArray4, angle + (double) num2, length2);
            float[] axisValue4 = this.GetAxisValue(numArray4, angle - (double) num2, length2);
            PointF[] points1 = new PointF[3]
            {
              new PointF(numArray4[0], -numArray4[1]),
              new PointF(axisValue3[0], -axisValue3[1]),
              new PointF(axisValue4[0], -axisValue4[1])
            };
            graphics.DrawPolygon(m_borderPen, m_backBrush, points1);
            continue;
          case "ROpenArrow":
            int num3 = index != 0 ? 30 : 150;
            double length3 = 9.0 * borderLength;
            float[] numArray5 = index != 0 ? this.GetAxisValue(numArray2, angle, borderLength) : this.GetAxisValue(numArray2, angle, -borderLength);
            float[] axisValue5 = this.GetAxisValue(numArray5, angle + (double) num3, length3);
            float[] axisValue6 = this.GetAxisValue(numArray5, angle - (double) num3, length3);
            PdfPath path2 = new PdfPath(m_borderPen);
            path2.AddLine(numArray5[0], -numArray5[1], axisValue5[0], -axisValue5[1]);
            path2.AddLine(numArray5[0], -numArray5[1], axisValue6[0], -axisValue6[1]);
            graphics.DrawPath(m_borderPen, path2);
            continue;
          case "RClosedArrow":
            int num4 = index != 0 ? 30 : 150;
            double length4 = 9.0 * borderLength;
            float[] numArray6 = index != 0 ? this.GetAxisValue(numArray2, angle, borderLength) : this.GetAxisValue(numArray2, angle, -borderLength);
            float[] axisValue7 = this.GetAxisValue(numArray6, angle + (double) num4, length4);
            float[] axisValue8 = this.GetAxisValue(numArray6, angle - (double) num4, length4);
            PointF[] points2 = new PointF[3]
            {
              new PointF(numArray6[0], -numArray6[1]),
              new PointF(axisValue7[0], -axisValue7[1]),
              new PointF(axisValue8[0], -axisValue8[1])
            };
            graphics.DrawPolygon(m_borderPen, m_backBrush, points2);
            continue;
          case "Slash":
            double length5 = 9.0 * borderLength;
            float[] axisValue9 = this.GetAxisValue(numArray2, angle + 60.0, length5);
            float[] axisValue10 = this.GetAxisValue(numArray2, angle - 120.0, length5);
            graphics.DrawLine(m_borderPen, numArray2[0], -numArray2[1], axisValue9[0], -axisValue9[1]);
            graphics.DrawLine(m_borderPen, numArray2[0], -numArray2[1], axisValue10[0], -axisValue10[1]);
            continue;
          case "Diamond":
            double length6 = 3.0 * borderLength;
            float[] axisValue11 = this.GetAxisValue(numArray2, 180.0, length6);
            float[] axisValue12 = this.GetAxisValue(numArray2, 90.0, length6);
            float[] axisValue13 = this.GetAxisValue(numArray2, 0.0, length6);
            float[] axisValue14 = this.GetAxisValue(numArray2, -90.0, length6);
            PointF[] points3 = new PointF[4]
            {
              new PointF(axisValue11[0], -axisValue11[1]),
              new PointF(axisValue12[0], -axisValue12[1]),
              new PointF(axisValue13[0], -axisValue13[1]),
              new PointF(axisValue14[0], -axisValue14[1])
            };
            graphics.DrawPolygon(m_borderPen, m_backBrush, points3);
            continue;
          case "Butt":
            double length7 = 3.0 * borderLength;
            float[] axisValue15 = this.GetAxisValue(numArray2, angle + 90.0, length7);
            float[] axisValue16 = this.GetAxisValue(numArray2, angle - 90.0, length7);
            graphics.DrawLine(m_borderPen, axisValue15[0], -axisValue15[1], axisValue16[0], -axisValue16[1]);
            continue;
          default:
            continue;
        }
      }
    }
  }

  private void AddPopUpAnnotation()
  {
    if (this.Dictionary != null && this.Dictionary.ContainsKey("Subtype"))
    {
      PdfName pdfName = PdfCrossTable.Dereference(this.Dictionary["Subtype"]) as PdfName;
      if (pdfName != (PdfName) null && (pdfName.Value == "FreeText" || pdfName.Value == "Sound" || pdfName.Value == "FileAttachment"))
        return;
    }
    if (this.Popup != null && this.m_popupAnnotations.Contains((PdfAnnotation) this.Popup))
    {
      bool flag = false;
      if (this.Popup.Dictionary != null && this.Popup.Dictionary.ContainsKey("Subtype"))
      {
        PdfName pdfName = PdfCrossTable.Dereference(this.Popup.Dictionary["Subtype"]) as PdfName;
        if (pdfName != (PdfName) null)
          flag = pdfName.Value == "Popup";
      }
      if (flag && this.Page != null && !this.Page.Annotations.Contains((PdfAnnotation) this.Popup))
      {
        this.Page.Annotations.m_savePopup = true;
        this.Page.Annotations.Add((PdfAnnotation) this.Popup);
        this.Page.Annotations.m_savePopup = false;
        this.m_popupAnnotations.Remove((PdfAnnotation) this.Popup);
      }
      else
      {
        if (!flag || this.LoadedPage == null || this.LoadedPage.Annotations.Contains((PdfAnnotation) this.Popup))
          return;
        this.LoadedPage.Annotations.m_savePopup = true;
        this.LoadedPage.Annotations.Add((PdfAnnotation) this.Popup);
        this.LoadedPage.Annotations.m_savePopup = false;
        this.m_popupAnnotations.Remove((PdfAnnotation) this.Popup);
      }
    }
    else
    {
      if (this.LoadedPage == null || !(this is PdfLoadedAnnotation))
        return;
      PdfAnnotation popup = (this as PdfLoadedAnnotation).Popup;
      if (popup == null || !(popup is PdfPopupAnnotation))
        return;
      this.LoadedPage.Annotations.m_savePopup = true;
      this.LoadedPage.Annotations.Add(popup);
      this.LoadedPage.Annotations.m_savePopup = false;
    }
  }
}
