// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfLoadedButtonField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class PdfLoadedButtonField : PdfLoadedStyledField
{
  private PdfLoadedButtonItemCollection m_items;

  public new bool ComplexScript
  {
    get => base.ComplexScript;
    set => base.ComplexScript = value;
  }

  public string Text
  {
    get => this.ObtainText();
    set
    {
      if ((FieldFlags.ReadOnly & this.Flags) != FieldFlags.Default)
        return;
      ((PdfField) this).Form.SetAppearanceDictionary = true;
      this.AssignText(value);
    }
  }

  public PdfLoadedButtonItemCollection Items
  {
    get => this.m_items;
    internal set => this.m_items = value;
  }

  internal PdfLoadedButtonField(PdfDictionary dictionary, PdfCrossTable crossTable)
    : base(dictionary, crossTable)
  {
    PdfArray kids = this.Kids;
    this.m_items = new PdfLoadedButtonItemCollection();
    if (kids == null)
      return;
    for (int index = 0; index < kids.Count; ++index)
    {
      PdfDictionary dictionary1 = crossTable.GetObject(kids[index]) as PdfDictionary;
      this.m_items.Add(new PdfLoadedButtonItem((PdfLoadedStyledField) this, index, dictionary1));
    }
  }

  private string ObtainText()
  {
    PdfDictionary pdfDictionary1 = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable) ?? this.Dictionary;
    string text = (string) null;
    if (pdfDictionary1.ContainsKey("MK"))
    {
      PdfDictionary pdfDictionary2 = this.CrossTable.GetObject(pdfDictionary1["MK"]) as PdfDictionary;
      if (pdfDictionary2.ContainsKey("CA"))
        text = (this.CrossTable.GetObject(pdfDictionary2["CA"]) as PdfString).Value;
    }
    if (text == null)
    {
      if (!(this.CrossTable.GetObject(this.Dictionary["V"]) is PdfString pdfString))
        pdfString = PdfLoadedField.GetValue(this.Dictionary, this.CrossTable, "V", true) as PdfString;
      text = pdfString == null ? "" : pdfString.Value;
    }
    return text;
  }

  private void AssignText(string value)
  {
    string str = value;
    PdfDictionary pdfDictionary1 = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable) ?? this.Dictionary;
    if (pdfDictionary1.ContainsKey("MK"))
    {
      PdfDictionary pdfDictionary2 = this.CrossTable.GetObject(pdfDictionary1["MK"]) as PdfDictionary;
      pdfDictionary2.SetString("CA", str);
      pdfDictionary1.SetProperty("MK", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary2));
    }
    else
    {
      PdfDictionary pdfDictionary3 = new PdfDictionary();
      pdfDictionary3.SetString("CA", str);
      pdfDictionary1.SetProperty("MK", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary3));
    }
    this.Changed = true;
  }

  internal override void Draw()
  {
    base.Draw();
    PdfArray kids = this.Kids;
    if (kids != null && kids.Count > 0)
    {
      for (int index = 0; index < kids.Count; ++index)
      {
        PdfLoadedFieldItem pdfLoadedFieldItem = (PdfLoadedFieldItem) this.Items[index];
        if (pdfLoadedFieldItem.Page != null)
          this.DrawButton(pdfLoadedFieldItem.Page.Graphics, pdfLoadedFieldItem);
      }
    }
    else
      this.DrawButton(this.Page.Graphics, (PdfLoadedFieldItem) null);
  }

  internal override void BeginSave()
  {
    base.BeginSave();
    PdfArray kids = this.Kids;
    if (kids != null)
    {
      for (int index = 0; index < kids.Count; ++index)
        this.ApplyAppearance(this.CrossTable.GetObject(kids[index]) as PdfDictionary, (PdfLoadedFieldItem) this.Items[index]);
    }
    else
      this.ApplyAppearance(this.GetWidgetAnnotation(this.Dictionary, this.CrossTable), (PdfLoadedFieldItem) null);
  }

  internal new PdfField Clone(PdfDictionary dictionary, PdfPage page)
  {
    PdfCrossTable crossTable = page.Section.ParentDocument.CrossTable;
    PdfLoadedButtonField loadedButtonField = new PdfLoadedButtonField(dictionary, crossTable);
    loadedButtonField.Page = (PdfPageBase) page;
    loadedButtonField.SetName(this.GetFieldName());
    loadedButtonField.Widget.Dictionary = this.Widget.Dictionary.Clone(crossTable) as PdfDictionary;
    return (PdfField) loadedButtonField;
  }

  internal new PdfLoadedStyledField Clone()
  {
    PdfLoadedButtonField field = this.MemberwiseClone() as PdfLoadedButtonField;
    field.Dictionary = this.Dictionary.Clone(this.CrossTable) as PdfDictionary;
    field.Widget.Dictionary = this.Widget.Dictionary.Clone(this.CrossTable) as PdfDictionary;
    field.Items = new PdfLoadedButtonItemCollection();
    for (int index = 0; index < this.Items.Count; ++index)
    {
      PdfLoadedButtonItem loadedButtonItem = new PdfLoadedButtonItem((PdfLoadedStyledField) field, index, this.Items[index].Dictionary);
      field.Items.Add(loadedButtonItem);
    }
    return (PdfLoadedStyledField) field;
  }

  internal override PdfLoadedFieldItem CreateLoadedItem(PdfDictionary dictionary)
  {
    base.CreateLoadedItem(dictionary);
    PdfLoadedButtonItem loadedItem = new PdfLoadedButtonItem((PdfLoadedStyledField) this, this.m_items.Count, dictionary);
    this.m_items.Add(loadedItem);
    if (this.Kids == null)
      this.Dictionary["Kids"] = (IPdfPrimitive) new PdfArray();
    this.Kids.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) dictionary));
    return (PdfLoadedFieldItem) loadedItem;
  }

  private void ApplyAppearance(PdfDictionary widget, PdfLoadedFieldItem item)
  {
    if (widget != null && widget.ContainsKey("AP"))
    {
      if (!(this.CrossTable.GetObject(widget["AP"]) is PdfDictionary primitive) || !primitive.ContainsKey("N"))
        return;
      RectangleF rectangleF = item == null ? this.Bounds : item.Bounds;
      PdfTemplate wrapper1 = new PdfTemplate(rectangleF.Size);
      PdfTemplate wrapper2 = new PdfTemplate(rectangleF.Size);
      if (widget.ContainsKey("MK") && widget["MK"] is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("R") && pdfDictionary["R"] is PdfNumber pdfNumber)
      {
        if ((double) pdfNumber.FloatValue == 90.0)
        {
          wrapper1 = new PdfTemplate(new SizeF(rectangleF.Size.Height, rectangleF.Size.Width), false);
          wrapper1.m_content["Matrix"] = (IPdfPrimitive) new PdfArray(new float[6]
          {
            0.0f,
            1f,
            -1f,
            0.0f,
            rectangleF.Size.Width,
            0.0f
          });
        }
        else if ((double) pdfNumber.FloatValue == 180.0)
        {
          wrapper1 = new PdfTemplate(rectangleF.Size, false);
          wrapper1.m_content["Matrix"] = (IPdfPrimitive) new PdfArray(new float[6]
          {
            -1f,
            0.0f,
            0.0f,
            -1f,
            rectangleF.Size.Width,
            rectangleF.Size.Height
          });
        }
        else if ((double) pdfNumber.FloatValue == 270.0)
        {
          wrapper1 = new PdfTemplate(new SizeF(rectangleF.Size.Height, rectangleF.Size.Width), false);
          wrapper1.m_content["Matrix"] = (IPdfPrimitive) new PdfArray(new float[6]
          {
            0.0f,
            -1f,
            1f,
            0.0f,
            0.0f,
            rectangleF.Size.Height
          });
        }
      }
      if (wrapper1 == null)
      {
        wrapper1 = new PdfTemplate(rectangleF.Size, false);
        wrapper1.m_content["Matrix"] = (IPdfPrimitive) new PdfArray(new float[6]
        {
          1f,
          0.0f,
          0.0f,
          1f,
          0.0f,
          0.0f
        });
      }
      this.DrawButton(wrapper1.Graphics, item);
      this.DrawButton(wrapper2.Graphics, item);
      primitive.SetProperty("N", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) wrapper1));
      primitive.SetProperty("D", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) wrapper2));
      widget.SetProperty("AP", (IPdfPrimitive) primitive);
    }
    else
    {
      if (!((PdfField) this).Form.SetAppearanceDictionary)
        return;
      ((PdfField) this).Form.NeedAppearances = true;
    }
  }

  private void DrawButton(PdfGraphics graphics, PdfLoadedFieldItem item)
  {
    PdfLoadedStyledField.GraphicsProperties graphicsProperties;
    this.GetGraphicsProperties(out graphicsProperties, item);
    if (!this.Flatten)
      graphicsProperties.Rect.Location = new PointF(0.0f, 0.0f);
    PaintParams paintParams = new PaintParams(graphicsProperties.Rect, graphicsProperties.BackBrush, graphicsProperties.ForeBrush, graphicsProperties.Pen, graphicsProperties.Style, graphicsProperties.BorderWidth, graphicsProperties.ShadowBrush, graphicsProperties.RotationAngle);
    if (this.Flatten || this.Changed && this.ComplexScript)
      graphicsProperties.StringFormat.Alignment = PdfTextAlignment.Center;
    if (this.Dictionary.ContainsKey("AP") && !this.ComplexScript && (graphics.Layer == null || graphics.Page.Rotation == PdfPageRotateAngle.RotateAngle0) && paintParams.RotationAngle <= 0)
    {
      if (!(PdfCrossTable.Dereference(this.Dictionary["AP"]) is PdfDictionary pdfDictionary1) || !(PdfCrossTable.Dereference(pdfDictionary1["N"]) is PdfDictionary pdfDictionary2) || !(pdfDictionary2 is PdfStream template1))
        return;
      PdfTemplate template2 = new PdfTemplate(template1);
      if (template2 == null)
        return;
      this.Page.Graphics.DrawPdfTemplate(template2, this.Bounds.Location);
    }
    else if (this.Dictionary.ContainsKey("Kids") && item != null && !this.ComplexScript && (graphics.Layer == null || graphics.Page.Rotation == PdfPageRotateAngle.RotateAngle0) && paintParams.RotationAngle <= 0)
    {
      if (!(PdfCrossTable.Dereference(item.Dictionary["AP"]) is PdfDictionary pdfDictionary3) || !(PdfCrossTable.Dereference(pdfDictionary3["N"]) is PdfDictionary pdfDictionary4) || !(pdfDictionary4 is PdfStream template3))
        return;
      PdfTemplate template4 = new PdfTemplate(template3);
      if (template4 == null)
        return;
      this.Page.Graphics.DrawPdfTemplate(template4, this.Bounds.Location);
    }
    else
      FieldPainter.DrawButton(graphics, paintParams, this.Text, graphicsProperties.Font, graphicsProperties.StringFormat);
  }

  internal override float GetFontHeight(PdfFontFamily family)
  {
    float num = (float) (12.0 * ((double) this.Bounds.Size.Width - 4.0 * (double) this.BorderWidth)) / new PdfStandardFont(family, 12f).MeasureString(this.Text).Width;
    return (double) num > 12.0 ? 12f : num;
  }

  public void AddPrintAction()
  {
    PdfDictionary primitive = new PdfDictionary();
    primitive.SetProperty("N", (IPdfPrimitive) new PdfName("Print"));
    primitive.SetProperty("S", (IPdfPrimitive) new PdfName("Named"));
    if (this.Dictionary["Kids"] is PdfArray pdfArray)
      ((pdfArray[0] as PdfReferenceHolder).Object as PdfDictionary).SetProperty("A", (IPdfPrimitive) primitive);
    else
      this.Dictionary.SetProperty("A", (IPdfPrimitive) primitive);
  }

  public void RemoveAt(int index)
  {
    if (this.Items == null || this.Items.Count == 0)
      return;
    this.Remove(this.Items[index]);
  }

  public void Remove(PdfLoadedButtonItem item)
  {
    int index = item != null ? this.Items.Remove(item) : throw new NullReferenceException(nameof (item));
    if (!(this.Dictionary["Kids"] is PdfArray pdfArray))
      return;
    pdfArray.RemoveAt(index);
    pdfArray.MarkChanged();
  }
}
