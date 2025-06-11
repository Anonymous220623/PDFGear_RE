// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfLoadedStyledField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Graphics.Fonts;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class PdfLoadedStyledField : PdfLoadedField
{
  private const byte ShadowShift = 64 /*0x40*/;
  private PdfFieldActions m_actions;
  private WidgetAnnotation m_widget = new WidgetAnnotation();
  private PdfAction m_mouseEnter;
  private PdfAction m_mouseLeave;
  private PdfAction m_mouseDown;
  private PdfAction m_mouseUp;
  private PdfAction m_gotFocus;
  private PdfAction m_lostFocus;
  private PdfPen m_borderPen;
  internal PdfFont m_font;
  private PdfFormFieldVisibility m_visibility;
  internal PdfArray m_array = new PdfArray();
  internal bool m_isTextChanged;
  internal bool m_isCustomFontSize;
  internal int m_angle;
  internal bool isRotationModified;
  internal bool m_isTextModified;

  public PdfAction MouseEnter
  {
    get
    {
      if (this.m_mouseEnter == null)
      {
        PdfDictionary pdfDictionary = this.CrossTable.GetObject(this.GetWidgetAnnotation(this.Dictionary, this.CrossTable)["AA"]) as PdfDictionary;
        if (pdfDictionary.Items.ContainsKey(new PdfName("E")))
          this.m_mouseEnter = (PdfAction) new PdfJavaScriptAction(((pdfDictionary.Items[new PdfName("E")] as PdfDictionary).Items[new PdfName("JS")] as PdfString).Value);
      }
      return this.m_mouseEnter;
    }
    set
    {
      if (value == null)
        return;
      this.m_mouseEnter = value;
      this.m_actions = new PdfFieldActions(this.Widget.Actions);
      PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
      widgetAnnotation.SetProperty("AA", (IPdfWrapper) this.m_actions);
      PdfDictionary primitive = this.CrossTable.GetObject(widgetAnnotation["AA"]) as PdfDictionary;
      primitive.SetProperty("E", (IPdfWrapper) this.m_mouseEnter);
      widgetAnnotation.SetProperty("AA", (IPdfPrimitive) primitive);
      this.Changed = true;
    }
  }

  public PdfAction MouseUp
  {
    get
    {
      if (this.m_mouseUp == null)
      {
        PdfDictionary pdfDictionary = this.CrossTable.GetObject(this.GetWidgetAnnotation(this.Dictionary, this.CrossTable)["AA"]) as PdfDictionary;
        if (pdfDictionary.Items.ContainsKey(new PdfName("U")))
          this.m_mouseUp = (PdfAction) new PdfJavaScriptAction(((pdfDictionary.Items[new PdfName("U")] as PdfDictionary).Items[new PdfName("JS")] as PdfString).Value);
      }
      return this.m_mouseUp;
    }
    set
    {
      if (value == null)
        return;
      this.m_mouseUp = value;
      this.m_actions = new PdfFieldActions(this.Widget.Actions);
      PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
      widgetAnnotation.SetProperty("AA", (IPdfWrapper) this.m_actions);
      PdfDictionary primitive = this.CrossTable.GetObject(widgetAnnotation["AA"]) as PdfDictionary;
      primitive.SetProperty("U", (IPdfWrapper) this.m_mouseUp);
      widgetAnnotation.SetProperty("AA", (IPdfPrimitive) primitive);
      this.Changed = true;
    }
  }

  public PdfAction MouseDown
  {
    get
    {
      if (this.m_mouseDown == null)
      {
        PdfDictionary pdfDictionary = this.CrossTable.GetObject(this.GetWidgetAnnotation(this.Dictionary, this.CrossTable)["AA"]) as PdfDictionary;
        if (pdfDictionary.Items.ContainsKey(new PdfName("D")))
          this.m_mouseDown = (PdfAction) new PdfJavaScriptAction(((pdfDictionary.Items[new PdfName("D")] as PdfDictionary).Items[new PdfName("JS")] as PdfString).Value);
      }
      return this.m_mouseDown;
    }
    set
    {
      if (value == null)
        return;
      this.m_mouseDown = value;
      this.m_actions = new PdfFieldActions(this.Widget.Actions);
      PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
      widgetAnnotation.SetProperty("AA", (IPdfWrapper) this.m_actions);
      PdfDictionary primitive = this.CrossTable.GetObject(widgetAnnotation["AA"]) as PdfDictionary;
      primitive.SetProperty("D", (IPdfWrapper) this.m_mouseDown);
      widgetAnnotation.SetProperty("AA", (IPdfPrimitive) primitive);
      this.Changed = true;
    }
  }

  public PdfAction MouseLeave
  {
    get
    {
      if (this.m_mouseLeave == null)
      {
        PdfDictionary pdfDictionary = this.CrossTable.GetObject(this.GetWidgetAnnotation(this.Dictionary, this.CrossTable)["AA"]) as PdfDictionary;
        if (pdfDictionary.Items.ContainsKey(new PdfName("X")))
          this.m_mouseLeave = (PdfAction) new PdfJavaScriptAction(((pdfDictionary.Items[new PdfName("X")] as PdfDictionary).Items[new PdfName("JS")] as PdfString).Value);
      }
      return this.m_mouseLeave;
    }
    set
    {
      if (value == null)
        return;
      this.m_mouseLeave = value;
      this.m_actions = new PdfFieldActions(this.Widget.Actions);
      PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
      widgetAnnotation.SetProperty("AA", (IPdfWrapper) this.m_actions);
      PdfDictionary primitive = this.CrossTable.GetObject(widgetAnnotation["AA"]) as PdfDictionary;
      primitive.SetProperty("X", (IPdfWrapper) this.m_mouseLeave);
      widgetAnnotation.SetProperty("AA", (IPdfPrimitive) primitive);
      this.Changed = true;
    }
  }

  public PdfAction GotFocus
  {
    get
    {
      if (this.m_gotFocus == null)
      {
        PdfDictionary pdfDictionary = this.CrossTable.GetObject(this.GetWidgetAnnotation(this.Dictionary, this.CrossTable)["AA"]) as PdfDictionary;
        if (pdfDictionary.Items.ContainsKey(new PdfName("F0")))
          this.m_gotFocus = (PdfAction) new PdfJavaScriptAction(((pdfDictionary.Items[new PdfName("Fo")] as PdfDictionary).Items[new PdfName("JS")] as PdfString).Value);
      }
      return this.m_gotFocus;
    }
    set
    {
      if (value == null)
        return;
      this.m_gotFocus = value;
      this.m_actions = new PdfFieldActions(this.Widget.Actions);
      PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
      widgetAnnotation.SetProperty("AA", (IPdfWrapper) this.m_actions);
      PdfDictionary primitive = this.CrossTable.GetObject(widgetAnnotation["AA"]) as PdfDictionary;
      primitive.SetProperty("Fo", (IPdfWrapper) this.m_gotFocus);
      widgetAnnotation.SetProperty("AA", (IPdfPrimitive) primitive);
      this.Changed = true;
    }
  }

  internal PdfColor ForeColor
  {
    get
    {
      PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
      PdfColor foreColor = new PdfColor((byte) 0, (byte) 0, (byte) 0);
      if (widgetAnnotation != null && widgetAnnotation.ContainsKey("DA"))
        foreColor = this.GetForeColour((this.CrossTable.GetObject(widgetAnnotation["DA"]) as PdfString).Value);
      else if (widgetAnnotation != null && widgetAnnotation.GetValue(this.CrossTable, "DA", "Parent") is PdfString pdfString)
        foreColor = this.GetForeColour(pdfString.Value);
      return foreColor;
    }
    set
    {
      PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
      float height = 0.0f;
      string str = (string) null;
      if (widgetAnnotation != null && widgetAnnotation.ContainsKey("DA"))
        str = this.FontName((widgetAnnotation["DA"] as PdfString).Value, out height);
      else if (widgetAnnotation != null && this.Dictionary.ContainsKey("DA"))
        str = this.FontName((this.Dictionary["DA"] as PdfString).Value, out height);
      if (str != null)
        widgetAnnotation["DA"] = (IPdfPrimitive) new PdfString(new PdfDefaultAppearance()
        {
          FontName = str,
          FontSize = height,
          ForeColor = value
        }.ToString());
      else
        widgetAnnotation["DA"] = (IPdfPrimitive) new PdfString(new PdfDefaultAppearance()
        {
          FontName = this.Font.Name,
          FontSize = this.Font.Size,
          ForeColor = value
        }.ToString());
      ((PdfField) this).Form.SetAppearanceDictionary = true;
    }
  }

  public PdfAction LostFocus
  {
    get
    {
      if (this.m_lostFocus == null)
      {
        PdfDictionary pdfDictionary = this.CrossTable.GetObject(this.GetWidgetAnnotation(this.Dictionary, this.CrossTable)["AA"]) as PdfDictionary;
        if (pdfDictionary.Items.ContainsKey(new PdfName("Bl")))
          this.m_lostFocus = (PdfAction) new PdfJavaScriptAction(((pdfDictionary.Items[new PdfName("Bl")] as PdfDictionary).Items[new PdfName("JS")] as PdfString).Value);
      }
      return this.m_lostFocus;
    }
    set
    {
      if (value == null)
        return;
      this.m_lostFocus = value;
      this.m_actions = new PdfFieldActions(this.Widget.Actions);
      PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
      widgetAnnotation.SetProperty("AA", (IPdfWrapper) this.m_actions);
      PdfDictionary primitive = this.CrossTable.GetObject(widgetAnnotation["AA"]) as PdfDictionary;
      primitive.SetProperty("Bl", (IPdfWrapper) this.m_lostFocus);
      widgetAnnotation.SetProperty("AA", (IPdfPrimitive) primitive);
      this.Changed = true;
    }
  }

  internal WidgetAnnotation Widget => this.m_widget;

  public RectangleF Bounds
  {
    get
    {
      RectangleF bounds = this.GetBounds(this.Dictionary, this.CrossTable);
      if (this.Page != null && this.Page.Dictionary.ContainsKey("CropBox"))
      {
        PdfArray pdfArray = !(this.Page.Dictionary["CropBox"] is PdfArray) ? (this.Page.Dictionary["CropBox"] as PdfReferenceHolder).Object as PdfArray : this.Page.Dictionary["CropBox"] as PdfArray;
        if ((double) (pdfArray[0] as PdfNumber).FloatValue != 0.0 || (double) (pdfArray[1] as PdfNumber).FloatValue != 0.0 || (double) this.Page.Size.Width == (double) (pdfArray[2] as PdfNumber).FloatValue || (double) this.Page.Size.Height == (double) (pdfArray[3] as PdfNumber).FloatValue)
        {
          bounds.X -= (pdfArray[0] as PdfNumber).FloatValue;
          bounds.Y = (pdfArray[3] as PdfNumber).FloatValue - (bounds.Y + bounds.Height);
        }
        else
          bounds.Y = this.Page.Size.Height - (bounds.Y + bounds.Height);
      }
      else if (this.Page != null && this.Page.Dictionary.ContainsKey("MediaBox"))
      {
        PdfArray pdfArray = (PdfArray) null;
        if (PdfCrossTable.Dereference(this.Page.Dictionary["MediaBox"]) is PdfArray)
          pdfArray = PdfCrossTable.Dereference(this.Page.Dictionary["MediaBox"]) as PdfArray;
        if ((double) (pdfArray[0] as PdfNumber).FloatValue > 0.0 || (double) (pdfArray[1] as PdfNumber).FloatValue > 0.0 || (double) this.Page.Size.Width == (double) (pdfArray[2] as PdfNumber).FloatValue || (double) this.Page.Size.Height == (double) (pdfArray[3] as PdfNumber).FloatValue)
        {
          bounds.X -= (pdfArray[0] as PdfNumber).FloatValue;
          bounds.Y = (pdfArray[3] as PdfNumber).FloatValue - (bounds.Y + bounds.Height);
        }
        else
          bounds.Y = this.Page.Size.Height - (bounds.Y + bounds.Height);
      }
      else if (this.Page != null)
        bounds.Y = this.Page.Size.Height - (bounds.Y + bounds.Height);
      else
        bounds.Y += bounds.Height;
      return bounds;
    }
    set
    {
      RectangleF rectangleF = value;
      if (rectangleF == RectangleF.Empty)
        throw new ArgumentNullException("rectangle");
      float height = this.Page.Size.Height;
      PdfNumber[] pdfNumberArray = new PdfNumber[4]
      {
        new PdfNumber(rectangleF.X),
        new PdfNumber(height - (rectangleF.Y + rectangleF.Height)),
        new PdfNumber(rectangleF.X + rectangleF.Width),
        new PdfNumber(height - rectangleF.Y)
      };
      PdfDictionary pdfDictionary = this.Dictionary;
      if (!pdfDictionary.ContainsKey("Rect"))
        pdfDictionary = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
      pdfDictionary.SetArray("Rect", (IPdfPrimitive[]) pdfNumberArray);
      this.Changed = true;
    }
  }

  public PointF Location
  {
    get => this.Bounds.Location;
    set => this.Bounds = new RectangleF(value, this.Bounds.Size);
  }

  public SizeF Size
  {
    get => this.Bounds.Size;
    set => this.Bounds = new RectangleF(this.Bounds.Location, value);
  }

  internal PdfPen BorderPen => this.ObtainBorderPen();

  public PdfBorderStyle BorderStyle
  {
    get => this.ObtainBorderStyle();
    set
    {
      this.AssignBorderStyle(value);
      this.CreateBorderPen();
    }
  }

  public PdfColor BorderColor
  {
    get
    {
      PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
      PdfColor borderColor = new PdfColor((byte) 0, (byte) 0, (byte) 0);
      if (widgetAnnotation.ContainsKey("MK"))
      {
        PdfDictionary pdfDictionary = this.CrossTable.GetObject(widgetAnnotation["MK"]) as PdfDictionary;
        if (pdfDictionary.ContainsKey("BC"))
          borderColor = this.CreateColor(pdfDictionary["BC"] as PdfArray);
      }
      return borderColor;
    }
    set
    {
      ((PdfField) this).Form.SetAppearanceDictionary = true;
      this.m_widget.WidgetAppearance.BorderColor = value;
      this.AssignBorderColor(value);
    }
  }

  internal float[] DashPatern => this.ObtainDashPatern();

  public float BorderWidth
  {
    get => this.ObtainBorderWidth();
    set
    {
      this.m_widget.WidgetBorder.Width = value;
      this.AssignBorderWidth(value);
    }
  }

  internal PdfStringFormat StringFormat => this.AssignStringFormat();

  internal PdfBrush BackBrush
  {
    get => this.ObtainBackBrush();
    set => this.AssignBackBrush(value);
  }

  internal PdfBrush ForeBrush => this.ObtainForeBrush();

  internal PdfBrush ShadowBrush => this.ObtainShadowBrush();

  public PdfFont Font
  {
    get
    {
      if (this.m_font != null && this.Dictionary.ContainsKey("Kids"))
      {
        if ((PdfCrossTable.Dereference(this.Dictionary["Kids"]) as PdfArray).Count <= 1)
          return this.m_font;
      }
      else if (this.m_font != null)
        return this.m_font;
      PdfFont font = PdfDocument.ConformanceLevel != PdfConformanceLevel.None ? (PdfFont) new PdfTrueTypeFont(new System.Drawing.Font("Microsoft Sans Serif", 8f), 8f) : PdfDocument.DefaultFont;
      PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
      if (widgetAnnotation != null && (widgetAnnotation.ContainsKey("DA") || this.Dictionary.ContainsKey("DA")))
      {
        if (!(this.CrossTable.GetObject(widgetAnnotation["DA"]) is PdfString pdfString))
          pdfString = this.CrossTable.GetObject(this.Dictionary["DA"]) as PdfString;
        string name = (string) null;
        if (pdfString != null)
        {
          bool isCorrectFont;
          if (PdfDocument.ConformanceLevel == PdfConformanceLevel.None)
          {
            font = this.GetFont(pdfString.Value, out isCorrectFont, out name);
          }
          else
          {
            float height = 0.0f;
            this.FontName(pdfString.Value, out height);
            font = (PdfFont) new PdfTrueTypeFont(new System.Drawing.Font("Microsoft Sans Serif", height), height);
            isCorrectFont = true;
          }
          if (!isCorrectFont && name != null)
          {
            string newValue = "/Helv";
            widgetAnnotation.SetProperty("DA", (IPdfPrimitive) new PdfString(pdfString.Value.Replace(name, newValue)));
          }
        }
      }
      return font;
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Font));
      if (this.m_font == value)
        return;
      this.m_font = value;
      if (((PdfField) this).Form != null)
        ((PdfField) this).Form.SetAppearanceDictionary = true;
      PdfDefaultAppearance defaultAppearance = new PdfDefaultAppearance();
      defaultAppearance.FontSize = this.m_font.Size;
      defaultAppearance.ForeColor = this.ForeColor;
      PdfName name = ((PdfField) this).Form.Resources.GetName((IPdfWrapper) this.m_font);
      defaultAppearance.FontName = name.Value;
      if (this.Dictionary.ContainsKey("Kids"))
      {
        PdfArray pdfArray = this.Dictionary["Kids"] as PdfArray;
        for (int index = 0; index < pdfArray.Count; ++index)
        {
          if ((pdfArray[index] as PdfReferenceHolder).Object is PdfDictionary pdfDictionary && (this.Dictionary.ContainsKey("DA") || pdfDictionary.ContainsKey("DA")))
          {
            pdfDictionary["DA"] = (IPdfPrimitive) new PdfString(defaultAppearance.ToString());
            if (this.Dictionary.ContainsKey("DA"))
              this.Dictionary["DA"] = (IPdfPrimitive) new PdfString(defaultAppearance.ToString());
          }
        }
      }
      else
      {
        PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
        if (widgetAnnotation == null || !this.Dictionary.ContainsKey("DA") && !widgetAnnotation.ContainsKey("DA"))
          return;
        widgetAnnotation["DA"] = (IPdfPrimitive) new PdfString(defaultAppearance.ToString());
        if (!this.Dictionary.ContainsKey("DA"))
          return;
        this.Dictionary["DA"] = (IPdfPrimitive) new PdfString(defaultAppearance.ToString());
      }
    }
  }

  public new int DefaultIndex
  {
    get => base.DefaultIndex;
    set => base.DefaultIndex = value >= 0 ? value : throw new IndexOutOfRangeException("index");
  }

  internal PdfArray Kids => this.ObtainKids();

  public bool Visible => this.ObtainVisible();

  public PdfFormFieldVisibility Visibility
  {
    get
    {
      this.m_visibility = this.ObtainVisibility();
      return this.m_visibility;
    }
    set
    {
      this.m_visibility = value;
      this.SetVisibility();
    }
  }

  public new int RotationAngle
  {
    get => this.ObtainRotationAngle();
    set
    {
      this.m_angle = value;
      int num = 360;
      if (this.m_angle >= 360)
        this.m_angle %= num;
      if (this.m_angle < 45)
        this.m_angle = 0;
      else if (this.m_angle >= 45 && this.m_angle < 135)
        this.m_angle = 90;
      else if (this.m_angle >= 135 && this.m_angle < 225)
        this.m_angle = 180;
      else if (this.m_angle >= 225 && this.m_angle < 315)
        this.m_angle = 270;
      int angle = this.m_angle;
      ((PdfField) this).Form.SetAppearanceDictionary = true;
      this.m_widget.WidgetAppearance.RotationAngle = angle;
      this.SetRotationAngle(angle);
      this.isRotationModified = true;
    }
  }

  internal PdfCheckBoxStyle Style
  {
    get => this.ObtainStyle();
    set => this.AssignStyle(value);
  }

  internal PdfLoadedStyledField(PdfDictionary dictionary, PdfCrossTable crossTable)
    : base(dictionary, crossTable)
  {
  }

  private PdfFormFieldVisibility ObtainVisibility()
  {
    PdfDictionary pdfDictionary = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
    PdfFormFieldVisibility visibility = PdfFormFieldVisibility.Visible;
    if (pdfDictionary == null)
      pdfDictionary = this.Dictionary;
    if (pdfDictionary != null)
    {
      if (pdfDictionary.ContainsKey("F"))
      {
        switch ((this.CrossTable.GetObject(pdfDictionary["F"]) as PdfNumber).IntValue)
        {
          case 0:
            visibility = PdfFormFieldVisibility.VisibleNotPrintable;
            break;
          case 2:
          case 6:
            visibility = PdfFormFieldVisibility.Hidden;
            break;
          case 4:
            visibility = PdfFormFieldVisibility.Visible;
            break;
          case 36:
            visibility = PdfFormFieldVisibility.HiddenPrintable;
            break;
        }
      }
      else
        visibility = PdfFormFieldVisibility.VisibleNotPrintable;
    }
    return visibility;
  }

  private PdfFormFieldVisibility ObtainKidsVisibility(PdfDictionary dictionary)
  {
    PdfDictionary pdfDictionary = dictionary;
    PdfFormFieldVisibility kidsVisibility = PdfFormFieldVisibility.Visible;
    if (pdfDictionary == null)
      pdfDictionary = this.Dictionary;
    if (pdfDictionary != null)
    {
      if (pdfDictionary.ContainsKey("F"))
      {
        if (this.CrossTable.GetObject(pdfDictionary["F"]) is PdfNumber pdfNumber)
        {
          switch (pdfNumber.IntValue)
          {
            case 0:
              kidsVisibility = PdfFormFieldVisibility.VisibleNotPrintable;
              break;
            case 2:
            case 6:
              kidsVisibility = PdfFormFieldVisibility.Hidden;
              break;
            case 4:
              kidsVisibility = PdfFormFieldVisibility.Visible;
              break;
            case 36:
              kidsVisibility = PdfFormFieldVisibility.HiddenPrintable;
              break;
          }
        }
      }
      else
        kidsVisibility = PdfFormFieldVisibility.VisibleNotPrintable;
    }
    return kidsVisibility;
  }

  private void SetVisibility()
  {
    PdfDictionary widget = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable) ?? this.Dictionary;
    if (this.Dictionary.ContainsKey("Kids"))
    {
      PdfArray pdfArray = PdfCrossTable.Dereference(this.Dictionary["Kids"]) as PdfArray;
      if (pdfArray.Count <= 0 || pdfArray == null)
        return;
      for (int index = 0; index < pdfArray.Count; ++index)
        this.SetVisiblityToWidget(PdfCrossTable.Dereference(pdfArray[index]) as PdfDictionary, true);
    }
    else
      this.SetVisiblityToWidget(widget, false);
  }

  private void SetVisiblityToWidget(PdfDictionary widget, bool kids)
  {
    if (widget == null)
      return;
    PdfFormFieldVisibility formFieldVisibility = kids ? this.ObtainKidsVisibility(widget) : this.ObtainVisibility();
    if (formFieldVisibility == this.m_visibility)
      return;
    if (widget.ContainsKey("F"))
      widget.Remove("F");
    switch (this.m_visibility)
    {
      case PdfFormFieldVisibility.Visible:
        widget.Items.Add(new PdfName("F"), (IPdfPrimitive) new PdfNumber(4));
        if (widget.ContainsKey("DV"))
          widget.Remove("DV");
        if (widget.ContainsKey("MK"))
        {
          PdfReferenceHolder pdfReferenceHolder = widget["MK"] as PdfReferenceHolder;
          if (pdfReferenceHolder != (PdfReferenceHolder) null)
          {
            if (pdfReferenceHolder.Object is PdfDictionary pdfDictionary1 && pdfDictionary1.ContainsKey("BG"))
              pdfDictionary1.Remove("BG");
          }
          else if (widget["MK"] is PdfDictionary pdfDictionary2 && pdfDictionary2.ContainsKey("BG"))
            pdfDictionary2.Remove("BG");
        }
        if (formFieldVisibility == PdfFormFieldVisibility.Hidden)
          this.Changed = true;
        widget.Modify();
        break;
      case PdfFormFieldVisibility.Hidden:
        widget.Items.Add(new PdfName("F"), (IPdfPrimitive) new PdfNumber(2));
        widget.Modify();
        break;
      case PdfFormFieldVisibility.HiddenPrintable:
        widget.Items.Add(new PdfName("F"), (IPdfPrimitive) new PdfNumber(36));
        widget.Modify();
        break;
    }
  }

  protected void GetGraphicsProperties(
    out PdfLoadedStyledField.GraphicsProperties graphicsProperties,
    PdfLoadedFieldItem item)
  {
    if (item != null)
      graphicsProperties = new PdfLoadedStyledField.GraphicsProperties(item);
    else
      graphicsProperties = new PdfLoadedStyledField.GraphicsProperties(this);
  }

  private PdfBorderStyle CreateBorderStyle(PdfDictionary bs)
  {
    PdfBorderStyle borderStyle = PdfBorderStyle.Solid;
    if (bs.ContainsKey("S"))
    {
      PdfName pdfName = this.CrossTable.GetObject(bs["S"]) as PdfName;
      if (pdfName != (PdfName) null)
      {
        switch (pdfName.Value.ToLower())
        {
          case "d":
            borderStyle = PdfBorderStyle.Dashed;
            break;
          case "b":
            borderStyle = PdfBorderStyle.Beveled;
            break;
          case "i":
            borderStyle = PdfBorderStyle.Inset;
            break;
          case "u":
            borderStyle = PdfBorderStyle.Underline;
            break;
        }
      }
    }
    return borderStyle;
  }

  private void AssignBorderStyle(PdfBorderStyle borderStyle)
  {
    string str = "";
    PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
    if (!widgetAnnotation.ContainsKey("BS"))
      return;
    this.CrossTable.GetObject(widgetAnnotation["BS"]);
    switch (borderStyle)
    {
      case PdfBorderStyle.Solid:
        str = "S";
        break;
      case PdfBorderStyle.Dashed:
        str = "D";
        break;
      case PdfBorderStyle.Beveled:
        str = "B";
        break;
      case PdfBorderStyle.Inset:
        str = "I";
        break;
      case PdfBorderStyle.Underline:
        str = "U";
        break;
    }
    (widgetAnnotation["BS"] as PdfDictionary)["S"] = (IPdfPrimitive) new PdfName(str);
    this.Widget.WidgetBorder.Style = borderStyle;
  }

  internal PdfPen AssignBorderColor(PdfColor borderColor)
  {
    PdfPen pdfPen = (PdfPen) null;
    if (this.Dictionary.ContainsKey("Kids"))
    {
      if (this.CrossTable.GetObject(this.Dictionary["Kids"]) is PdfArray pdfArray)
      {
        for (int index = 0; index < pdfArray.Count; ++index)
        {
          if (PdfCrossTable.Dereference(pdfArray[index]) is PdfDictionary pdfDictionary3)
          {
            if (pdfDictionary3.ContainsKey("MK"))
            {
              if (this.CrossTable.GetObject(pdfDictionary3["MK"]) is PdfDictionary pdfDictionary1)
              {
                PdfArray array = borderColor.ToArray();
                if (borderColor.A == (byte) 0)
                {
                  pdfDictionary1["BC"] = (IPdfPrimitive) new PdfArray(new float[0]);
                  pdfPen = new PdfPen(borderColor);
                }
                else
                {
                  pdfDictionary1["BC"] = (IPdfPrimitive) array;
                  pdfPen = new PdfPen(this.CreateColor(array));
                }
              }
            }
            else
            {
              PdfDictionary pdfDictionary2 = new PdfDictionary();
              PdfArray array = borderColor.ToArray();
              if (borderColor.A == (byte) 0)
              {
                pdfDictionary2["BC"] = (IPdfPrimitive) new PdfArray(new float[0]);
                pdfPen = new PdfPen(borderColor);
              }
              else
              {
                pdfDictionary2["BC"] = (IPdfPrimitive) array;
                pdfPen = new PdfPen(this.CreateColor(array));
              }
              pdfDictionary3["MK"] = (IPdfPrimitive) pdfDictionary2;
            }
          }
          PdfBorderStyle borderStyle = this.BorderStyle;
          float borderWidth = this.BorderWidth;
          if (pdfPen != null)
          {
            pdfPen.Width = borderWidth;
            if (borderStyle == PdfBorderStyle.Dashed)
            {
              float[] dashPatern = this.DashPatern;
              pdfPen.DashStyle = PdfDashStyle.Custom;
              if (dashPatern != null)
                pdfPen.DashPattern = dashPatern;
              else
                pdfPen.DashPattern = new float[1]
                {
                  3f / borderWidth
                };
            }
          }
        }
      }
    }
    else
    {
      PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
      if (widgetAnnotation != null)
      {
        if (widgetAnnotation.ContainsKey("MK"))
        {
          if (this.CrossTable.GetObject(widgetAnnotation["MK"]) is PdfDictionary pdfDictionary4)
          {
            PdfArray array = borderColor.ToArray();
            if (borderColor.A == (byte) 0)
            {
              pdfDictionary4["BC"] = (IPdfPrimitive) new PdfArray(new float[0]);
              pdfPen = new PdfPen(borderColor);
            }
            else
            {
              pdfDictionary4["BC"] = (IPdfPrimitive) array;
              pdfPen = new PdfPen(this.CreateColor(array));
            }
          }
        }
        else
        {
          PdfDictionary pdfDictionary5 = new PdfDictionary();
          PdfArray array = borderColor.ToArray();
          if (borderColor.A == (byte) 0)
          {
            pdfDictionary5["BC"] = (IPdfPrimitive) new PdfArray(new float[0]);
            pdfPen = new PdfPen(borderColor);
          }
          else
          {
            pdfDictionary5["BC"] = (IPdfPrimitive) array;
            pdfPen = new PdfPen(this.CreateColor(array));
          }
          widgetAnnotation["MK"] = (IPdfPrimitive) pdfDictionary5;
        }
      }
      PdfBorderStyle borderStyle = this.BorderStyle;
      float borderWidth = this.BorderWidth;
      if (pdfPen != null)
      {
        pdfPen.Width = borderWidth;
        if (borderStyle == PdfBorderStyle.Dashed)
        {
          float[] dashPatern = this.DashPatern;
          pdfPen.DashStyle = PdfDashStyle.Custom;
          if (dashPatern != null)
            pdfPen.DashPattern = dashPatern;
          else
            pdfPen.DashPattern = new float[1]
            {
              3f / borderWidth
            };
        }
      }
    }
    return pdfPen;
  }

  internal void AssignBackColor(PdfColor value)
  {
    PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
    if (widgetAnnotation == null)
      return;
    if (widgetAnnotation.ContainsKey("MK"))
    {
      (this.CrossTable.GetObject(widgetAnnotation["MK"]) as PdfDictionary)["BG"] = (IPdfPrimitive) value.ToArray();
    }
    else
    {
      PdfDictionary pdfDictionary = new PdfDictionary();
      PdfArray array = value.ToArray();
      pdfDictionary["BG"] = (IPdfPrimitive) array;
      widgetAnnotation["MK"] = (IPdfPrimitive) pdfDictionary;
    }
    ((PdfField) this).Form.SetAppearanceDictionary = true;
  }

  private RectangleF GetBounds(PdfDictionary dictionary, PdfCrossTable crossTable)
  {
    PdfArray pdfArray = (PdfArray) null;
    if (dictionary.ContainsKey("Kids"))
    {
      PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(dictionary, crossTable);
      if (widgetAnnotation.ContainsKey("Rect"))
        pdfArray = crossTable.GetObject(widgetAnnotation["Rect"]) as PdfArray;
    }
    else
    {
      if (dictionary.ContainsKey("Parent"))
      {
        PdfDictionary dictionary1 = (dictionary["Parent"] as PdfReferenceHolder).Object as PdfDictionary;
        if (dictionary1.ContainsKey("Kids") && dictionary1.ContainsKey("FT") && (dictionary1["FT"] as PdfName).Value == "Btn")
        {
          PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(dictionary1, crossTable);
          if (widgetAnnotation.ContainsKey("Rect"))
            pdfArray = crossTable.GetObject(widgetAnnotation["Rect"]) as PdfArray;
        }
      }
      if (pdfArray == null && dictionary.ContainsKey("Rect"))
        pdfArray = crossTable.GetObject(dictionary["Rect"]) as PdfArray;
    }
    RectangleF bounds;
    if (pdfArray != null)
    {
      bounds = pdfArray.ToRectangle();
      bounds.Height = (float) Math.Round((double) bounds.Height, 3);
      if ((double) (PdfCrossTable.Dereference(pdfArray[1]) as PdfNumber).FloatValue < 0.0)
      {
        bounds.Y = (PdfCrossTable.Dereference(pdfArray[1]) as PdfNumber).FloatValue;
        if ((double) (PdfCrossTable.Dereference(pdfArray[1]) as PdfNumber).FloatValue > (double) (PdfCrossTable.Dereference(pdfArray[3]) as PdfNumber).FloatValue)
          bounds.Y -= bounds.Height;
      }
    }
    else
      bounds = new RectangleF();
    return bounds;
  }

  private string GetHighLightString(PdfHighlightMode mode)
  {
    string highLightString = (string) null;
    switch (mode)
    {
      case PdfHighlightMode.NoHighlighting:
        highLightString = "N";
        break;
      case PdfHighlightMode.Invert:
        highLightString = "I";
        break;
      case PdfHighlightMode.Outline:
        highLightString = "O";
        break;
      case PdfHighlightMode.Push:
        highLightString = "P";
        break;
    }
    return highLightString;
  }

  private PdfColor CreateColor(PdfArray array)
  {
    int count1 = array.Count;
    PdfColor color = PdfColor.Empty;
    float[] numArray = new float[array.Count];
    int index = 0;
    for (int count2 = array.Count; index < count2; ++index)
    {
      PdfNumber pdfNumber = this.CrossTable.GetObject(array[index]) as PdfNumber;
      numArray[index] = pdfNumber.FloatValue;
    }
    switch (count1)
    {
      case 1:
        color = (double) numArray[0] <= 0.0 || (double) numArray[0] > 1.0 ? new PdfColor((float) (byte) numArray[0]) : new PdfColor(numArray[0]);
        break;
      case 3:
        color = (double) numArray[0] > 0.0 && (double) numArray[0] <= 1.0 || (double) numArray[1] > 0.0 && (double) numArray[1] <= 1.0 || (double) numArray[2] > 0.0 && (double) numArray[2] <= 1.0 ? new PdfColor(numArray[0], numArray[1], numArray[2]) : new PdfColor((byte) numArray[0], (byte) numArray[1], (byte) numArray[2]);
        break;
      case 4:
        color = (double) numArray[0] > 0.0 && (double) numArray[0] <= 1.0 || (double) numArray[1] > 0.0 && (double) numArray[1] <= 1.0 || (double) numArray[2] > 0.0 && (double) numArray[2] <= 1.0 || (double) numArray[3] > 0.0 && (double) numArray[3] <= 1.0 ? new PdfColor(numArray[0], numArray[1], numArray[2], numArray[3]) : new PdfColor((byte) numArray[0], (byte) numArray[1], (byte) numArray[2], (byte) numArray[3]);
        break;
    }
    return color;
  }

  internal PdfColor GetForeColour(string defaultAppearance)
  {
    PdfColor foreColour = new PdfColor((byte) 0, (byte) 0, (byte) 0);
    if (defaultAppearance == null || defaultAppearance == string.Empty)
    {
      foreColour = new PdfColor((byte) 0, (byte) 0, (byte) 0);
    }
    else
    {
      PdfReader pdfReader = new PdfReader((Stream) new MemoryStream(Encoding.UTF8.GetBytes(defaultAppearance)));
      pdfReader.Position = 0L;
      bool flag = false;
      Stack<string> stringStack = new Stack<string>();
      string nextToken = pdfReader.GetNextToken();
      if (nextToken == "/")
        flag = true;
      while (nextToken != null && nextToken != string.Empty)
      {
        if (flag)
          nextToken = pdfReader.GetNextToken();
        flag = true;
        switch (nextToken)
        {
          case "g":
            foreColour = new PdfColor(this.ParseFloatColour(stringStack.Pop()));
            continue;
          case "rg":
            byte blue = (byte) ((double) this.ParseFloatColour(stringStack.Pop()) * (double) byte.MaxValue);
            byte green = (byte) ((double) this.ParseFloatColour(stringStack.Pop()) * (double) byte.MaxValue);
            foreColour = new PdfColor((byte) ((double) this.ParseFloatColour(stringStack.Pop()) * (double) byte.MaxValue), green, blue);
            continue;
          case "k":
            float floatColour1 = this.ParseFloatColour(stringStack.Pop());
            float floatColour2 = this.ParseFloatColour(stringStack.Pop());
            float floatColour3 = this.ParseFloatColour(stringStack.Pop());
            foreColour = new PdfColor(this.ParseFloatColour(stringStack.Pop()), floatColour3, floatColour2, floatColour1);
            continue;
          default:
            stringStack.Push(nextToken);
            continue;
        }
      }
    }
    return foreColour;
  }

  private float ParseFloatColour(string text)
  {
    double result;
    try
    {
      result = double.Parse(text, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture);
    }
    catch
    {
      if (double.TryParse(text, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result))
        result = double.Parse(text, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture);
    }
    return (float) result;
  }

  internal string FontName(string fontString, out float height)
  {
    if (fontString.Contains("#2C"))
    {
      StringBuilder stringBuilder = new StringBuilder(fontString);
      stringBuilder.Replace("#2C", ",");
      fontString = stringBuilder.ToString();
    }
    PdfReader pdfReader = new PdfReader((Stream) new MemoryStream(Encoding.UTF8.GetBytes(fontString)));
    pdfReader.Position = 0L;
    string s = pdfReader.GetNextToken();
    string nextToken = pdfReader.GetNextToken();
    string str = (string) null;
    height = 0.0f;
    while (nextToken != null && nextToken != string.Empty)
    {
      str = s;
      s = nextToken;
      nextToken = pdfReader.GetNextToken();
      if (nextToken == "Tf")
      {
        try
        {
          height = (float) double.Parse(s, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture);
          break;
        }
        catch (Exception ex)
        {
          double result;
          height = !double.TryParse(s, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result) ? (float) result : (float) double.Parse(s, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture);
          break;
        }
      }
    }
    return str;
  }

  private PdfFontMetrics CreateFont(PdfDictionary fontDictionary, float height, PdfName baseFont)
  {
    PdfFontMetrics font1 = new PdfFontMetrics();
    if (fontDictionary.ContainsKey("FontDescriptor"))
    {
      PdfReferenceHolder font2 = fontDictionary["FontDescriptor"] as PdfReferenceHolder;
      PdfDictionary pdfDictionary = !(font2 != (PdfReferenceHolder) null) ? fontDictionary["FontDescriptor"] as PdfDictionary : font2.Object as PdfDictionary;
      font1.Ascent = (float) (pdfDictionary["Ascent"] as PdfNumber).IntValue;
      font1.Descent = (float) (pdfDictionary["Descent"] as PdfNumber).IntValue;
      font1.Size = height;
      font1.Height = font1.Ascent - font1.Descent;
      font1.PostScriptName = baseFont.Value;
    }
    if (fontDictionary.ContainsKey("Widths"))
    {
      if ((object) (fontDictionary["Widths"] as PdfReferenceHolder) != null)
      {
        PdfArray pdfArray = (new PdfReferenceHolder(fontDictionary["Widths"]).Object as PdfReferenceHolder).Object as PdfArray;
        int[] widths = new int[pdfArray.Count];
        for (int index = 0; index < pdfArray.Count; ++index)
          widths[index] = (pdfArray[index] as PdfNumber).IntValue;
        font1.WidthTable = (WidthTable) new StandardWidthTable(widths);
      }
      else
      {
        PdfArray font3 = fontDictionary["Widths"] as PdfArray;
        int[] widths = new int[font3.Count];
        for (int index = 0; index < font3.Count; ++index)
          widths[index] = (font3[index] as PdfNumber).IntValue;
        font1.WidthTable = (WidthTable) new StandardWidthTable(widths);
      }
    }
    font1.Name = baseFont.Value;
    return font1;
  }

  private PdfFont GetFontByName(string name, float height)
  {
    PdfFont fontByName = (PdfFont) null;
    switch (name)
    {
      case "CoBO":
        fontByName = (PdfFont) new PdfStandardFont(PdfFontFamily.Courier, height, PdfFontStyle.Bold | PdfFontStyle.Italic);
        break;
      case "CoBo":
        fontByName = (PdfFont) new PdfStandardFont(PdfFontFamily.Courier, height, PdfFontStyle.Bold);
        break;
      case "CoOb":
        fontByName = (PdfFont) new PdfStandardFont(PdfFontFamily.Courier, height, PdfFontStyle.Italic);
        break;
      case "Courier":
      case "Cour":
        fontByName = (PdfFont) new PdfStandardFont(PdfFontFamily.Courier, height, PdfFontStyle.Regular);
        break;
      case "HeBO":
        fontByName = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, height, PdfFontStyle.Bold | PdfFontStyle.Italic);
        break;
      case "HeBo":
        fontByName = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, height, PdfFontStyle.Bold);
        break;
      case "HeOb":
        fontByName = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, height, PdfFontStyle.Italic);
        break;
      case "Helv":
        fontByName = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, height, PdfFontStyle.Regular);
        break;
      case "Symb":
        fontByName = (PdfFont) new PdfStandardFont(PdfFontFamily.Symbol, height);
        break;
      case "TiBI":
        fontByName = (PdfFont) new PdfStandardFont(PdfFontFamily.TimesRoman, height, PdfFontStyle.Bold | PdfFontStyle.Italic);
        break;
      case "TiBo":
        fontByName = (PdfFont) new PdfStandardFont(PdfFontFamily.TimesRoman, height, PdfFontStyle.Bold);
        break;
      case "TiIt":
        fontByName = (PdfFont) new PdfStandardFont(PdfFontFamily.TimesRoman, height, PdfFontStyle.Italic);
        break;
      case "TiRo":
        fontByName = (PdfFont) new PdfStandardFont(PdfFontFamily.TimesRoman, height, PdfFontStyle.Regular);
        break;
      case "ZaDb":
        fontByName = (PdfFont) new PdfStandardFont(PdfFontFamily.ZapfDingbats, height);
        break;
    }
    return fontByName;
  }

  private bool CheckFontStyle(string fontFamilyString, out PdfFontStyle style)
  {
    bool flag = false;
    style = PdfFontStyle.Regular;
    if (fontFamilyString != null)
    {
      if (fontFamilyString.Contains("BoldItalic") || fontFamilyString.Contains("BoldOblique") || fontFamilyString.Contains("BoldItalicMT"))
      {
        style = PdfFontStyle.Bold | PdfFontStyle.Italic;
        flag = true;
      }
      else if (fontFamilyString.Contains("Italic") || fontFamilyString.Contains("Oblique") || fontFamilyString.Contains("ItalicMT") || fontFamilyString.Contains("It"))
      {
        style = PdfFontStyle.Italic;
        flag = true;
      }
      else if (fontFamilyString.Contains("Bold") || fontFamilyString.Contains("BoldMT"))
      {
        style = PdfFontStyle.Bold;
        flag = true;
      }
    }
    return flag;
  }

  private PdfFontStyle GetFontStyle(string fontFamilyString)
  {
    int num = fontFamilyString.IndexOf("-");
    if (num < 0)
      num = fontFamilyString.IndexOf(",");
    PdfFontStyle fontStyle = PdfFontStyle.Regular;
    if (num >= 0)
    {
      switch (fontFamilyString.Substring(num + 1, fontFamilyString.Length - num - 1))
      {
        case "Italic":
        case "Oblique":
        case "ItalicMT":
        case "It":
          fontStyle = PdfFontStyle.Italic;
          break;
        case "Bold":
        case "BoldMT":
          fontStyle = PdfFontStyle.Bold;
          break;
        case "BoldItalic":
        case "BoldOblique":
        case "BoldItalicMT":
          fontStyle = PdfFontStyle.Bold | PdfFontStyle.Italic;
          break;
      }
    }
    return fontStyle;
  }

  internal string GetFontName(string fontFamilyString)
  {
    if (fontFamilyString.Contains("-") || fontFamilyString.Contains("PSMT") || fontFamilyString.Contains("MT") || fontFamilyString.Contains(","))
    {
      string str = fontFamilyString;
      if (fontFamilyString.Contains("-"))
        str = fontFamilyString.Replace("-", " ");
      if (fontFamilyString.Contains(","))
        return fontFamilyString.Split(',')[0];
      foreach (FontFamily family in FontFamily.Families)
      {
        string name = family.Name;
        if (str == name)
          return name;
      }
      if (fontFamilyString.Contains("PSMT"))
        return fontFamilyString.Replace("PSMT", "");
      if (fontFamilyString.Contains("MT") && !fontFamilyString.Contains("-"))
        return fontFamilyString.Replace("MT", "");
    }
    return fontFamilyString.Split('-')[0];
  }

  private PdfFontFamily GetFontFamily(string fontFamilyString, out string standardName)
  {
    int length = fontFamilyString.IndexOf("-");
    PdfFontFamily fontFamily = PdfFontFamily.Helvetica;
    standardName = fontFamilyString;
    if (length >= 0)
      standardName = fontFamilyString.Substring(0, length);
    if (standardName == "Times")
    {
      fontFamily = PdfFontFamily.TimesRoman;
      standardName = (string) null;
    }
    else
    {
      foreach (string name in Enum.GetNames(typeof (PdfFontFamily)))
      {
        try
        {
          if (name.Contains(standardName))
          {
            fontFamily = (PdfFontFamily) Enum.Parse(typeof (PdfFontFamily), standardName, true);
            standardName = (string) null;
            break;
          }
        }
        catch (ArgumentException ex)
        {
          return PdfFontFamily.Helvetica;
        }
      }
    }
    return fontFamily;
  }

  internal PdfColor GetBackColor()
  {
    PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
    PdfColor backColor = new PdfColor();
    if (widgetAnnotation.ContainsKey("MK"))
    {
      PdfDictionary pdfDictionary = this.CrossTable.GetObject(widgetAnnotation["MK"]) as PdfDictionary;
      if (pdfDictionary.ContainsKey("BG"))
        backColor = this.CreateColor(pdfDictionary["BG"] as PdfArray);
    }
    return backColor;
  }

  private PdfBorderStyle ObtainBorderStyle()
  {
    PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
    PdfBorderStyle borderStyle = PdfBorderStyle.Solid;
    if (widgetAnnotation.ContainsKey("BS"))
      borderStyle = this.CreateBorderStyle(this.CrossTable.GetObject(widgetAnnotation["BS"]) as PdfDictionary);
    return borderStyle;
  }

  private float[] ObtainDashPatern()
  {
    float[] dashPatern = (float[]) null;
    if (this.BorderStyle == PdfBorderStyle.Dashed)
    {
      PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
      if (widgetAnnotation.ContainsKey("D"))
      {
        PdfArray pdfArray = this.CrossTable.GetObject(widgetAnnotation["D"]) as PdfArray;
        if (dashPatern.Length == 2)
        {
          dashPatern = new float[2];
          PdfNumber pdfNumber1 = pdfArray[0] as PdfNumber;
          dashPatern[0] = (float) pdfNumber1.IntValue;
          PdfNumber pdfNumber2 = pdfArray[1] as PdfNumber;
          dashPatern[1] = (float) pdfNumber2.IntValue;
        }
        else
        {
          dashPatern = new float[1];
          PdfNumber pdfNumber = pdfArray[0] as PdfNumber;
          dashPatern[0] = (float) pdfNumber.IntValue;
        }
      }
    }
    return dashPatern;
  }

  private float ObtainBorderWidth()
  {
    PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
    float borderWidth = 0.0f;
    PdfName pdfName = this.CrossTable.GetObject(widgetAnnotation["FT"]) as PdfName;
    if (widgetAnnotation.ContainsKey("BS"))
    {
      borderWidth = 1f;
      if (this.CrossTable.GetObject((this.CrossTable.GetObject(widgetAnnotation["BS"]) as PdfDictionary)["W"]) is PdfNumber pdfNumber)
        borderWidth = pdfNumber.FloatValue;
    }
    else if (pdfName != (PdfName) null && pdfName.Value == "Btn")
      borderWidth = 1f;
    return borderWidth;
  }

  private void AssignBorderWidth(float width)
  {
    PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
    if (!widgetAnnotation.ContainsKey("BS"))
      return;
    (widgetAnnotation["BS"] as PdfDictionary)["W"] = (IPdfPrimitive) new PdfNumber(width);
    this.CreateBorderPen();
  }

  private PdfStringFormat AssignStringFormat()
  {
    PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
    PdfStringFormat pdfStringFormat = new PdfStringFormat()
    {
      LineAlignment = PdfVerticalAlignment.Middle
    };
    pdfStringFormat.LineAlignment = (this.Flags & FieldFlags.Multiline) > FieldFlags.Default ? PdfVerticalAlignment.Top : PdfVerticalAlignment.Middle;
    PdfNumber pdfNumber = (PdfNumber) null;
    if (widgetAnnotation != null && widgetAnnotation.ContainsKey("Q"))
      pdfNumber = this.CrossTable.GetObject(widgetAnnotation["Q"]) as PdfNumber;
    else if (this.Dictionary.ContainsKey("Q"))
      pdfNumber = this.CrossTable.GetObject(this.Dictionary["Q"]) as PdfNumber;
    if (pdfNumber != null && (pdfNumber.IsInteger || pdfNumber.IsLong))
      pdfStringFormat.Alignment = !pdfNumber.IsInteger ? (PdfTextAlignment) pdfNumber.LongValue : (PdfTextAlignment) pdfNumber.IntValue;
    if (this.ComplexScript || this.Form != null && this.Form.ComplexScript)
      pdfStringFormat.ComplexScript = true;
    return pdfStringFormat;
  }

  private PdfBrush ObtainBackBrush()
  {
    PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
    PdfBrush backBrush = (PdfBrush) null;
    if (widgetAnnotation != null && widgetAnnotation.ContainsKey("MK"))
    {
      PdfDictionary pdfDictionary = this.CrossTable.GetObject(widgetAnnotation["MK"]) as PdfDictionary;
      if (pdfDictionary.ContainsKey("BG") && this.CrossTable.GetObject(pdfDictionary["BG"]) is PdfArray array)
        backBrush = (PdfBrush) new PdfSolidBrush(this.CreateColor(array));
    }
    return backBrush;
  }

  private void AssignBackBrush(PdfBrush brush)
  {
    PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
    if (widgetAnnotation == null || !(brush is PdfSolidBrush))
      return;
    PdfDictionary pdfDictionary;
    if (widgetAnnotation.ContainsKey("MK"))
    {
      pdfDictionary = this.CrossTable.GetObject(widgetAnnotation["MK"]) as PdfDictionary;
    }
    else
    {
      pdfDictionary = new PdfDictionary();
      widgetAnnotation["MK"] = (IPdfPrimitive) pdfDictionary;
    }
    PdfArray array = (brush as PdfSolidBrush).Color.ToArray();
    pdfDictionary["BG"] = (IPdfPrimitive) array;
  }

  private PdfFont UpdateFontEncoding(PdfFont font, PdfDictionary fontDictionary)
  {
    PdfDictionary fontInternal = font.FontInternal as PdfDictionary;
    if (fontDictionary.Items.ContainsKey(new PdfName("Encoding")))
    {
      PdfName key = new PdfName("Encoding");
      PdfReferenceHolder pdfReferenceHolder = fontDictionary.Items[new PdfName("Encoding")] as PdfReferenceHolder;
      if (pdfReferenceHolder != (PdfReferenceHolder) null)
      {
        if (pdfReferenceHolder.Object is PdfDictionary pdfDictionary1)
        {
          if (fontInternal.Items.ContainsKey(new PdfName("Encoding")))
            fontInternal.Items.Remove(new PdfName("Encoding"));
          fontInternal.Items.Add(key, (IPdfPrimitive) pdfDictionary1);
        }
      }
      else if (fontDictionary.Items[new PdfName("Encoding")] is PdfDictionary pdfDictionary2)
      {
        if (fontInternal.Items.ContainsKey(new PdfName("Encoding")))
          fontInternal.Items.Remove(new PdfName("Encoding"));
        fontInternal.Items.Add(key, (IPdfPrimitive) pdfDictionary2);
      }
    }
    return font;
  }

  private PdfBrush ObtainForeBrush()
  {
    PdfBrush foreBrush = PdfBrushes.Black;
    PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
    if (widgetAnnotation != null && widgetAnnotation.ContainsKey("DA") && this.CrossTable.GetObject(widgetAnnotation["DA"]) is PdfString pdfString)
      foreBrush = (PdfBrush) new PdfSolidBrush(this.GetForeColour(pdfString.Value));
    return foreBrush;
  }

  private PdfBrush ObtainShadowBrush()
  {
    PdfBrush shadowBrush = PdfBrushes.White;
    PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
    if (widgetAnnotation != null && widgetAnnotation.ContainsKey("DA"))
    {
      this.CrossTable.GetObject(widgetAnnotation["DA"]);
      PdfColor color = new PdfColor(byte.MaxValue, byte.MaxValue, byte.MaxValue);
      if (this.BackBrush is PdfSolidBrush backBrush)
        color = backBrush.Color;
      color.R = (int) color.R - 64 /*0x40*/ >= 0 ? (byte) ((int) color.R - 64 /*0x40*/) : (byte) 0;
      color.G = (int) color.G - 64 /*0x40*/ >= 0 ? (byte) ((int) color.G - 64 /*0x40*/) : (byte) 0;
      color.B = (int) color.B - 64 /*0x40*/ >= 0 ? (byte) ((int) color.B - 64 /*0x40*/) : (byte) 0;
      shadowBrush = (PdfBrush) new PdfSolidBrush(color);
    }
    return shadowBrush;
  }

  internal override void Draw()
  {
  }

  internal override PdfLoadedFieldItem CreateLoadedItem(PdfDictionary dictionary)
  {
    return (PdfLoadedFieldItem) null;
  }

  internal override void BeginSave()
  {
    base.BeginSave();
    if (!(this.BackBrush is PdfSolidBrush backBrush) || !backBrush.Color.IsEmpty)
      return;
    PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
    PdfDictionary primitive1 = new PdfDictionary();
    PdfArray primitive2 = new PdfArray(new float[3]
    {
      1f,
      1f,
      1f
    });
    primitive1.SetProperty("BG", (IPdfPrimitive) primitive2);
    widgetAnnotation.SetProperty("MK", (IPdfPrimitive) primitive1);
  }

  internal virtual float GetFontHeight(PdfFontFamily family) => 0.0f;

  internal PdfPen ObtainBorderPen()
  {
    PdfDictionary pdfDictionary1 = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
    PdfPen borderPen = (PdfPen) null;
    if (pdfDictionary1 == null)
      pdfDictionary1 = this.Dictionary;
    if (pdfDictionary1 != null && pdfDictionary1.ContainsKey("MK"))
    {
      PdfDictionary pdfDictionary2 = this.CrossTable.GetObject(pdfDictionary1["MK"]) as PdfDictionary;
      if (pdfDictionary2.ContainsKey("BC"))
        borderPen = new PdfPen(this.CreateColor(this.CrossTable.GetObject(pdfDictionary2["BC"]) as PdfArray));
    }
    PdfBorderStyle borderStyle = this.BorderStyle;
    float borderWidth = this.BorderWidth;
    if (borderPen != null)
    {
      borderPen.Width = borderWidth;
      if (borderStyle == PdfBorderStyle.Dashed)
      {
        float[] dashPatern = this.DashPatern;
        borderPen.DashStyle = PdfDashStyle.Custom;
        if (dashPatern != null)
          borderPen.DashPattern = dashPatern;
        else if ((double) borderWidth > 0.0)
          borderPen.DashPattern = new float[1]
          {
            3f / borderWidth
          };
      }
    }
    return borderPen;
  }

  private PdfArray ObtainKids()
  {
    PdfArray kids = (PdfArray) null;
    if (this.Dictionary.ContainsKey("Kids"))
      kids = this.CrossTable.GetObject(this.Dictionary["Kids"]) as PdfArray;
    return kids;
  }

  private bool ObtainVisible()
  {
    PdfDictionary pdfDictionary = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable) ?? this.Dictionary;
    return pdfDictionary == null || !pdfDictionary.ContainsKey("F") || (this.CrossTable.GetObject(pdfDictionary["F"]) as PdfNumber).IntValue != 2;
  }

  private void CreateBorderPen()
  {
    float width = this.m_widget.WidgetBorder.Width;
    this.m_borderPen = new PdfPen(this.m_widget.WidgetAppearance.BorderColor, width);
    if (this.Widget.WidgetBorder.Style != PdfBorderStyle.Dashed)
      return;
    this.m_borderPen.DashStyle = PdfDashStyle.Custom;
    this.m_borderPen.DashPattern = new float[1]
    {
      3f / width
    };
  }

  protected override void DefineDefaultAppearance()
  {
    if (this.Form == null || this.m_font == null)
      return;
    PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
    PdfName name = this.Form.Resources.GetName((IPdfWrapper) this.m_font);
    this.Form.Resources.Add(this.m_font, name);
    this.Form.NeedAppearances = true;
    widgetAnnotation["DA"] = (IPdfPrimitive) new PdfString(new PdfDefaultAppearance()
    {
      FontName = name.Value,
      FontSize = this.m_font.Size,
      ForeColor = this.ForeColor
    }.ToString());
  }

  private int ObtainRotationAngle()
  {
    int rotationAngle = 0;
    PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
    if (widgetAnnotation.ContainsKey("MK"))
    {
      if (this.CrossTable.GetObject(widgetAnnotation["MK"]) is PdfDictionary pdfDictionary1)
        rotationAngle = pdfDictionary1.ContainsKey("R") ? (pdfDictionary1["R"] as PdfNumber).IntValue : 0;
    }
    else if (this.Dictionary.ContainsKey("Kids"))
    {
      PdfArray pdfArray = PdfCrossTable.Dereference(this.Dictionary["Kids"]) as PdfArray;
      for (int index = 0; index < pdfArray.Count; ++index)
      {
        if (PdfCrossTable.Dereference(pdfArray[index]) is PdfDictionary pdfDictionary2 && pdfDictionary2.ContainsKey("MK") && PdfCrossTable.Dereference(pdfDictionary2["MK"]) is PdfDictionary pdfDictionary3)
          return pdfDictionary3.ContainsKey("R") ? (pdfDictionary3["R"] as PdfNumber).IntValue : 0;
      }
    }
    return rotationAngle;
  }

  internal void SetRotationAngle(int angle)
  {
    PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
    if (!widgetAnnotation.ContainsKey("MK"))
      return;
    if (this.CrossTable.GetObject(widgetAnnotation["MK"]) is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("R"))
    {
      pdfDictionary["R"] = (IPdfPrimitive) new PdfNumber(angle);
    }
    else
    {
      if (pdfDictionary.ContainsKey("R"))
        return;
      pdfDictionary.SetProperty("R", (IPdfPrimitive) new PdfNumber(angle));
    }
  }

  internal PdfField Clone(PdfDictionary dictionary, PdfPage page)
  {
    PdfCrossTable crossTable = page.Section.ParentDocument.CrossTable;
    PdfLoadedStyledField loadedStyledField = new PdfLoadedStyledField(dictionary, crossTable);
    loadedStyledField.Page = (PdfPageBase) page;
    loadedStyledField.Widget.Dictionary = this.Widget.Dictionary.Clone(crossTable) as PdfDictionary;
    return (PdfField) loadedStyledField;
  }

  internal PdfCheckBoxStyle ObtainStyle()
  {
    PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
    PdfCheckBoxStyle style = PdfCheckBoxStyle.Check;
    if (widgetAnnotation.ContainsKey("MK"))
      style = this.CreateStyle(this.CrossTable.GetObject(widgetAnnotation["MK"]) as PdfDictionary);
    return style;
  }

  private PdfCheckBoxStyle CreateStyle(PdfDictionary bs)
  {
    PdfCheckBoxStyle style = PdfCheckBoxStyle.Check;
    if (bs.ContainsKey("CA") && this.CrossTable.GetObject(bs["CA"]) is PdfString pdfString)
    {
      switch (pdfString.Value.ToLower())
      {
        case "4":
          style = PdfCheckBoxStyle.Check;
          break;
        case "l":
          style = PdfCheckBoxStyle.Circle;
          break;
        case "8":
          style = PdfCheckBoxStyle.Cross;
          break;
        case "u":
          style = PdfCheckBoxStyle.Diamond;
          break;
        case "n":
          style = PdfCheckBoxStyle.Square;
          break;
        case "h":
          style = PdfCheckBoxStyle.Star;
          break;
      }
    }
    return style;
  }

  internal void AssignStyle(PdfCheckBoxStyle checkStyle)
  {
    string str = "";
    PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
    if (!widgetAnnotation.ContainsKey("MK"))
      return;
    this.CrossTable.GetObject(widgetAnnotation["MK"]);
    switch (checkStyle)
    {
      case PdfCheckBoxStyle.Check:
        str = "4";
        break;
      case PdfCheckBoxStyle.Circle:
        str = "l";
        break;
      case PdfCheckBoxStyle.Cross:
        str = "8";
        break;
      case PdfCheckBoxStyle.Diamond:
        str = "u";
        break;
      case PdfCheckBoxStyle.Square:
        str = "n";
        break;
      case PdfCheckBoxStyle.Star:
        str = "H";
        break;
    }
    (widgetAnnotation["MK"] as PdfDictionary)["CA"] = (IPdfPrimitive) new PdfString(str);
    this.Widget.WidgetAppearance.NormalCaption = str;
  }

  protected string StyleToString(PdfCheckBoxStyle style)
  {
    switch (style)
    {
      case PdfCheckBoxStyle.Circle:
        return "l";
      case PdfCheckBoxStyle.Cross:
        return "8";
      case PdfCheckBoxStyle.Diamond:
        return "u";
      case PdfCheckBoxStyle.Square:
        return "n";
      case PdfCheckBoxStyle.Star:
        return "H";
      default:
        return "4";
    }
  }

  internal PdfLoadedStyledField Clone()
  {
    PdfLoadedStyledField loadedStyledField = (PdfLoadedStyledField) null;
    switch (this)
    {
      case PdfLoadedCheckBoxField _:
        return (this as PdfLoadedCheckBoxField).Clone();
      case PdfLoadedTextBoxField _:
        return (this as PdfLoadedTextBoxField).Clone();
      case PdfLoadedComboBoxField _:
        return (this as PdfLoadedComboBoxField).Clone();
      case PdfLoadedRadioButtonListField _:
        return (this as PdfLoadedRadioButtonListField).Clone();
      case PdfLoadedListBoxField _:
        return (this as PdfLoadedListBoxField).Clone();
      case PdfLoadedButtonField _:
        return (this as PdfLoadedButtonField).Clone();
      default:
        return loadedStyledField;
    }
  }

  private PdfFont GetFont(string fontString, out bool isCorrectFont, out string name)
  {
    float height = 0.0f;
    isCorrectFont = true;
    string str1 = (string) null;
    name = this.FontName(fontString, out height);
    PdfFont font1 = (PdfFont) new PdfStandardFont((PdfStandardFont) PdfDocument.DefaultFont, height);
    PdfFontStyle textFontStyle = PdfFontStyle.Regular;
    PdfDictionary fontDictionary1 = (PdfDictionary) null;
    if (this.Form.Resources != null && this.Form.Resources.ContainsKey("Font"))
      fontDictionary1 = PdfCrossTable.Dereference(this.Form.Resources["Font"]) as PdfDictionary;
    bool unicode = false;
    if (this.Dictionary.ContainsKey("V"))
    {
      if (PdfCrossTable.Dereference(this.Dictionary["V"]) is PdfString pdfString1 && pdfString1.Value != null)
        unicode = PdfString.IsUnicode(pdfString1.Value);
      if (this.Dictionary.ContainsKey("FT"))
      {
        PdfName pdfName = this.Dictionary["FT"] as PdfName;
        if (pdfString1 != null && pdfName != (PdfName) null && pdfName.Value.Equals("Ch") && this.Dictionary.ContainsKey("Opt") && PdfCrossTable.Dereference(this.Dictionary["Opt"]) is PdfArray pdfArray1)
        {
          for (int index = 0; index < pdfArray1.Count; ++index)
          {
            if (PdfCrossTable.Dereference(pdfArray1[index]) is PdfArray pdfArray && pdfArray.Count > 1)
            {
              PdfString pdfString2 = PdfCrossTable.Dereference(pdfArray[0]) as PdfString;
              PdfString pdfString3 = PdfCrossTable.Dereference(pdfArray[1]) as PdfString;
              if (pdfString2 != null && pdfString3 != null && pdfString2.Value == pdfString1.Value && pdfString3.Value != null)
              {
                unicode = PdfString.IsUnicode(pdfString3.Value);
                break;
              }
            }
          }
        }
      }
    }
    if (fontDictionary1 != null && name != null)
      str1 = this.FindFontName(fontDictionary1, name, out textFontStyle);
    if ((double) height == 0.0)
    {
      if (font1 is PdfStandardFont pdfStandardFont)
      {
        height = this.GetFontHeight(pdfStandardFont.FontFamily);
        if (float.IsNaN(height) || (double) height == 0.0)
          height = 12f;
        pdfStandardFont.Size = height;
      }
      this.m_isCustomFontSize = true;
    }
    if (fontDictionary1 != null && name != null && fontDictionary1.ContainsKey(name))
    {
      PdfDictionary fontDictionary2 = PdfCrossTable.Dereference(fontDictionary1[name]) as PdfDictionary;
      name = str1;
      if (fontDictionary2 != null && fontDictionary2.ContainsKey("Subtype"))
      {
        PdfName pdfName1 = PdfCrossTable.Dereference(fontDictionary2["Subtype"]) as PdfName;
        if (pdfName1 != (PdfName) null)
        {
          PdfName baseFont = (PdfName) null;
          PdfFontStyle style1 = PdfFontStyle.Regular;
          string standardName = string.Empty;
          PdfFontFamily pdfFontFamily = PdfFontFamily.Helvetica;
          if (fontDictionary2.ContainsKey("BaseFont"))
          {
            baseFont = PdfCrossTable.Dereference(fontDictionary2["BaseFont"]) as PdfName;
            if (baseFont != (PdfName) null)
            {
              style1 = this.GetFontStyle(PdfName.DecodeName(baseFont.Value));
              pdfFontFamily = this.GetFontFamily(PdfName.DecodeName(baseFont.Value), out standardName);
            }
          }
          if (baseFont != (PdfName) null && pdfName1.Value == "Type1")
          {
            PdfFontMetrics font2 = this.CreateFont(fontDictionary2, height, baseFont);
            font1 = (PdfFont) new PdfStandardFont(pdfFontFamily, height, style1);
            if (!this.m_isTextChanged)
              font1 = this.UpdateFontEncoding(font1, fontDictionary2);
            if (standardName == null)
            {
              if (this.m_isCustomFontSize)
              {
                float fontHeight = this.GetFontHeight(pdfFontFamily);
                if ((double) fontHeight != 0.0)
                  font1.Size = fontHeight;
              }
              return font1;
            }
            string familyName = baseFont.Value;
            if (PdfName.DecodeName(baseFont.Value).Contains("-"))
              familyName = PdfName.DecodeName(baseFont.Value).Replace('-', ' ');
            System.Drawing.Font font3 = new System.Drawing.Font(familyName, height, (FontStyle) style1);
            if (font3.Name == familyName)
            {
              font1 = (PdfFont) new PdfTrueTypeFont(font3, unicode);
            }
            else
            {
              bool flag = false;
              if (fontDictionary2.ContainsKey("Encoding") && PdfCrossTable.Dereference(fontDictionary2["Encoding"]) is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("Differences") && !fontDictionary2.ContainsKey("Widths"))
                flag = true;
              if (!flag && !this.m_isTextChanged || standardName != null)
                font1.Metrics = font2 == null || (double) font2.Height <= 0.0 || !fontDictionary2.ContainsKey("FontDescriptor") ? font1.Metrics : font2;
              font1.FontInternal = (IPdfPrimitive) fontDictionary2;
            }
          }
          else if (baseFont != (PdfName) null && pdfName1.Value == "TrueType")
          {
            string str2 = baseFont.Value;
            if (PdfName.DecodeName(baseFont.Value).Contains("-") && this.GetFontStyle(PdfName.DecodeName(baseFont.Value)) == PdfFontStyle.Regular)
              name = PdfName.DecodeName(baseFont.Value).Replace('-', ' ');
            font1 = (PdfFont) new PdfTrueTypeFont(new System.Drawing.Font(name, height, (FontStyle) style1), unicode);
            if (font1.Name != name)
            {
              font1.FontInternal = (IPdfPrimitive) fontDictionary2;
              WidthTable widthTable = font1.Metrics.WidthTable;
              PdfFontMetrics font4 = this.CreateFont(fontDictionary2, height, baseFont);
              if (font4 != null)
                font1.Metrics = font4;
              font1.Metrics.WidthTable = widthTable;
            }
          }
          else if (pdfName1.Value == "Type0")
          {
            fontDictionary3 = fontDictionary2;
            if (!(PdfCrossTable.Dereference(fontDictionary2["FontDescriptor"]) is PdfDictionary pdfDictionary) && fontDictionary2.ContainsKey("DescendantFonts"))
            {
              if (PdfCrossTable.Dereference(fontDictionary2["DescendantFonts"]) is PdfArray pdfArray && pdfArray.Count > 0 && PdfCrossTable.Dereference(pdfArray[0]) is PdfDictionary fontDictionary3)
                pdfDictionary = PdfCrossTable.Dereference(fontDictionary3["FontDescriptor"]) as PdfDictionary;
              if (pdfDictionary != null && pdfDictionary.ContainsKey("FontName"))
              {
                PdfName pdfName2 = pdfDictionary["FontName"] as PdfName;
                if (pdfName2 != (PdfName) null)
                {
                  string str3 = pdfName2.Value.Substring(pdfName2.Value.IndexOf('+') + 1);
                  PdfFontMetrics font5 = this.CreateFont(fontDictionary3, height, new PdfName(str3));
                  string familyName = name;
                  FontStyle style2 = (FontStyle) textFontStyle;
                  foreach (FontFamily family in FontFamily.Families)
                  {
                    string str4 = family.Name.Replace(" ", string.Empty);
                    if (familyName.Equals(str4))
                    {
                      familyName = family.Name;
                      break;
                    }
                  }
                  PdfFont font6 = (PdfFont) new PdfTrueTypeFont(new System.Drawing.Font(familyName, height, style2), true);
                  if (!this.m_isTextModified && font6.Name != familyName)
                  {
                    font6.FontInternal = (IPdfPrimitive) fontDictionary2;
                    WidthTable widthTable = font6.Metrics.WidthTable;
                    if (font5 != null)
                      font6.Metrics = font5;
                    font6.Metrics.WidthTable = widthTable;
                  }
                  return font6;
                }
              }
            }
          }
        }
      }
    }
    else
    {
      PdfFont fontByName = this.GetFontByName(name, height);
      if (fontByName != null)
        font1 = fontByName;
      else
        isCorrectFont = false;
    }
    return font1;
  }

  private string FindFontName(
    PdfDictionary fontDictionary,
    string name,
    out PdfFontStyle textFontStyle)
  {
    textFontStyle = PdfFontStyle.Regular;
    fontDictionary = PdfCrossTable.Dereference(fontDictionary[name]) as PdfDictionary;
    if (fontDictionary != null && fontDictionary.ContainsKey("BaseFont"))
    {
      PdfName pdfName = PdfCrossTable.Dereference(fontDictionary["BaseFont"]) as PdfName;
      if (pdfName != (PdfName) null)
      {
        name = PdfName.DecodeName(pdfName.Value);
        textFontStyle = this.GetFontStyle(pdfName.Value);
        if (name.Contains("PSMT"))
          name = name.Remove(name.IndexOf("PSMT"));
        if (name.Contains("PS"))
          name = name.Remove(name.IndexOf("PS"));
        if (name.Contains("-"))
          name = name.Remove(name.IndexOf("-"));
      }
    }
    else if (fontDictionary != null)
    {
      if (!(PdfCrossTable.Dereference(fontDictionary["FontDescriptor"]) is PdfDictionary pdfDictionary1) && fontDictionary.ContainsKey("DescendantFonts") && PdfCrossTable.Dereference(fontDictionary["DescendantFonts"]) is PdfArray pdfArray && pdfArray.Count > 0)
      {
        if (PdfCrossTable.Dereference(pdfArray[0]) is PdfDictionary pdfDictionary)
          pdfDictionary1 = PdfCrossTable.Dereference(pdfDictionary["FontDescriptor"]) as PdfDictionary;
        if (pdfDictionary1 != null && pdfDictionary1.ContainsKey("FontName"))
        {
          PdfName pdfName = pdfDictionary1["FontName"] as PdfName;
          if (pdfName != (PdfName) null)
          {
            string fontFamilyString = pdfName.Value.Substring(pdfName.Value.IndexOf('+') + 1);
            textFontStyle = this.GetFontStyle(fontFamilyString);
            if (fontFamilyString.Contains("PSMT"))
              fontFamilyString = fontFamilyString.Remove(fontFamilyString.IndexOf("PSMT"));
            if (fontFamilyString.Contains("PS"))
              fontFamilyString = fontFamilyString.Remove(fontFamilyString.IndexOf("PS"));
            if (fontFamilyString.Contains("-"))
              fontFamilyString.Remove(fontFamilyString.IndexOf("-"));
          }
        }
      }
    }
    else if (name != null)
      textFontStyle = this.GetFontStyle(name);
    if (name.Contains("#"))
    {
      string[] strArray = name.Split('#');
      string str1 = string.Empty;
      foreach (string str2 in strArray)
        str1 = str2.Contains("20") ? str1 + str2.Substring(2) : str1 + str2;
      name = str1;
    }
    string str3 = name;
    string[] sourceArray = new string[1]{ "" };
    int length = 0;
    for (int startIndex = 0; startIndex < str3.Length; ++startIndex)
    {
      string str4 = str3.Substring(startIndex, 1);
      if ("ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".Contains(str4) && startIndex > 0 && !"ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".Contains(str3[startIndex - 1].ToString()))
      {
        ++length;
        string[] destinationArray = new string[length + 1];
        System.Array.Copy((System.Array) sourceArray, 0, (System.Array) destinationArray, 0, length);
        sourceArray = destinationArray;
      }
      if (!str4.Contains(" "))
      {
        string[] strArray;
        IntPtr index;
        (strArray = sourceArray)[(int) (index = (IntPtr) length)] = strArray[index] + str4;
      }
    }
    name = string.Empty;
    foreach (string str5 in sourceArray)
      name = $"{name}{str5} ";
    name = name.Substring(name.IndexOf('+') + 1);
    name = name.Trim();
    name = name.Split(',')[0];
    if (name == "Arial MT")
      name = "Arial";
    return name;
  }

  protected struct GraphicsProperties
  {
    public RectangleF Rect;
    public PdfPen Pen;
    public PdfBorderStyle Style;
    public float BorderWidth;
    public PdfBrush BackBrush;
    public PdfBrush ForeBrush;
    public PdfBrush ShadowBrush;
    public PdfFont Font;
    public PdfStringFormat StringFormat;
    public int RotationAngle;

    public GraphicsProperties(PdfLoadedStyledField field)
    {
      this.Rect = field != null ? field.Bounds : throw new ArgumentNullException(nameof (field));
      this.Pen = field.BorderPen;
      this.Style = field.BorderStyle;
      this.BorderWidth = field.BorderWidth;
      this.BackBrush = field.BackBrush;
      this.ForeBrush = field.ForeBrush;
      this.ShadowBrush = field.ShadowBrush;
      this.Font = field.Font;
      this.StringFormat = field.StringFormat;
      this.RotationAngle = field.RotationAngle;
      if (field.Page == null || field.Page.Rotation == PdfPageRotateAngle.RotateAngle0)
        return;
      this.Rect = this.RotateTextbox(field.Bounds, field.Page.Size, field.Page.Rotation);
    }

    public GraphicsProperties(PdfLoadedFieldItem item)
    {
      this.Rect = item != null ? item.Bounds : throw new ArgumentNullException(nameof (item));
      this.Pen = item.BorderPen;
      this.Style = item.BorderStyle;
      this.BorderWidth = item.BorderWidth;
      this.BackBrush = item.BackBrush;
      this.ForeBrush = item.ForeBrush;
      this.ShadowBrush = item.ShadowBrush;
      this.Font = item.Font;
      this.StringFormat = item.StringFormat;
      this.RotationAngle = 0;
      PdfName key1 = new PdfName("MK");
      if (item.Dictionary.ContainsKey(key1))
      {
        PdfDictionary pdfDictionary = !(item.Dictionary["MK"] as PdfReferenceHolder != (PdfReferenceHolder) null) ? item.Dictionary["MK"] as PdfDictionary : PdfCrossTable.Dereference(item.Dictionary["MK"]) as PdfDictionary;
        PdfName key2 = new PdfName("R");
        if (pdfDictionary != null && pdfDictionary.ContainsKey(key2) && pdfDictionary.Items[new PdfName("R")] is PdfNumber pdfNumber)
          this.RotationAngle = pdfNumber.IntValue;
      }
      if (item.Page == null || item.Page.Rotation == PdfPageRotateAngle.RotateAngle0)
        return;
      this.Rect = this.RotateTextbox(item.Bounds, item.Page.Size, item.Page.Rotation);
    }

    private RectangleF RotateTextbox(RectangleF rect, SizeF size, PdfPageRotateAngle angle)
    {
      RectangleF rectangleF = new RectangleF();
      if (angle == PdfPageRotateAngle.RotateAngle180)
        rectangleF = new RectangleF(size.Width - (rect.X + rect.Width), size.Height - (rect.Y + rect.Height), rect.Width, rect.Height);
      if (angle == PdfPageRotateAngle.RotateAngle270)
      {
        float y = size.Width - (rect.X + rect.Width);
        rectangleF = new RectangleF(rect.Y, y, rect.Height, rect.Width);
      }
      if (angle == PdfPageRotateAngle.RotateAngle90)
        rectangleF = new RectangleF(size.Height - (rect.Y + rect.Height), rect.X, rect.Height, rect.Width);
      return rectangleF;
    }
  }
}
