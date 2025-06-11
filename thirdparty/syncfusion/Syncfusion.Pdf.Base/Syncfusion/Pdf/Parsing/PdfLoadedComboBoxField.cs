// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfLoadedComboBoxField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class PdfLoadedComboBoxField : PdfLoadedChoiceField
{
  private PdfLoadedComboBoxItemCollection m_items;

  public new bool ComplexScript
  {
    get => base.ComplexScript;
    set => base.ComplexScript = value;
  }

  internal bool IsAutoFontSize
  {
    get
    {
      bool isAutoFontSize = false;
      if (this.CrossTable.Document is PdfLoadedDocument document)
      {
        PdfLoadedForm form = document.Form;
        if (form != null && form.Dictionary.ContainsKey("DA"))
        {
          PdfString pdfString1 = form.Dictionary.Items[new PdfName("DA")] as PdfString;
          float height1 = 0.0f;
          if (pdfString1 != null)
          {
            this.FontName(pdfString1.Value, out height1);
            if ((double) height1 == 0.0)
            {
              if (this.Dictionary.ContainsKey("Kids"))
              {
                bool flag = false;
                PdfArray pdfArray = this.Dictionary["Kids"] as PdfArray;
                PdfDictionary pdfDictionary = (PdfDictionary) null;
                float height2;
                if (this.Dictionary.ContainsKey("DA"))
                {
                  PdfString pdfString2 = this.Dictionary.Items[new PdfName("DA")] as PdfString;
                  height2 = 0.0f;
                  if (pdfString2 != null)
                    this.FontName(pdfString2.Value, out height2);
                  if ((double) height2 == 0.0)
                    flag = true;
                }
                if (pdfArray != null && (flag || !this.Dictionary.ContainsKey("DA")))
                {
                  foreach (PdfReferenceHolder element in pdfArray.Elements)
                  {
                    if (element != (PdfReferenceHolder) null)
                      pdfDictionary = element.Object as PdfDictionary;
                    if (pdfDictionary != null && !pdfDictionary.ContainsKey("DA"))
                    {
                      isAutoFontSize = true;
                    }
                    else
                    {
                      PdfString pdfString3 = pdfDictionary.Items[new PdfName("DA")] as PdfString;
                      height2 = 0.0f;
                      if (pdfString3 != null)
                        this.FontName(pdfString3.Value, out height2);
                      if ((double) height2 == 0.0)
                        isAutoFontSize = true;
                    }
                  }
                }
              }
              else if (!this.Dictionary.ContainsKey("DA"))
              {
                isAutoFontSize = true;
              }
              else
              {
                PdfString pdfString4 = this.Dictionary.Items[new PdfName("DA")] as PdfString;
                float height3 = 0.0f;
                if (pdfString4 != null)
                  this.FontName(pdfString4.Value, out height3);
                if ((double) height3 == 0.0)
                  isAutoFontSize = true;
              }
            }
          }
        }
      }
      return isAutoFontSize;
    }
  }

  public bool Editable
  {
    get => (FieldFlags.Edit & this.Flags) != FieldFlags.Default;
    set
    {
      if (value)
        this.Flags |= FieldFlags.Edit;
      else
        this.Flags -= FieldFlags.Edit;
    }
  }

  public PdfLoadedComboBoxItemCollection Items
  {
    get => this.m_items;
    internal set => this.m_items = value;
  }

  public int SelectedIndex
  {
    get => this.ObtainSelectedIndex()[0];
    set => this.AssignSelectedIndex(new int[1]{ value });
  }

  public string SelectedValue
  {
    get
    {
      string[] selectedValue = this.ObtainSelectedValue();
      return selectedValue.Length > 0 ? selectedValue[0] : (string) null;
    }
    set => this.AssignSelectedValue(new string[1]{ value });
  }

  internal PdfLoadedComboBoxField(PdfDictionary dictionary, PdfCrossTable crossTable)
    : base(dictionary, crossTable)
  {
    PdfArray kids = this.Kids;
    this.m_items = new PdfLoadedComboBoxItemCollection();
    if (kids == null)
      return;
    for (int index = 0; index < kids.Count; ++index)
    {
      PdfDictionary dictionary1 = crossTable.GetObject(kids[index]) as PdfDictionary;
      this.m_items.Add(new PdfLoadedComboBoxItem((PdfLoadedStyledField) this, index, dictionary1));
    }
  }

  internal override void Draw()
  {
    base.Draw();
    RectangleF bounds1 = this.Bounds with
    {
      Location = PointF.Empty
    };
    string str = string.Empty;
    if (this.SelectedIndex != -1)
      str = this.SelectedItem[0].Text;
    else if (this.SelectedIndex == -1 && this.Dictionary.ContainsKey("V"))
    {
      if (PdfCrossTable.Dereference(this.Dictionary["V"]) is PdfString pdfString)
        str = pdfString.Value;
    }
    else if (this.Dictionary.ContainsKey("DV"))
      str = !(this.Dictionary["DV"] is PdfString) ? (PdfCrossTable.Dereference(this.Dictionary["DV"]) as PdfString).Value : (this.Dictionary["DV"] as PdfString).Value;
    PdfTemplate pdfTemplate = new PdfTemplate(bounds1.Size);
    PdfArray kids = this.Kids;
    if (kids != null && kids.Count > 1)
    {
      for (int index = 0; index < kids.Count; ++index)
      {
        PdfLoadedFieldItem pdfLoadedFieldItem = (PdfLoadedFieldItem) this.Items[index];
        PdfTemplate template = new PdfTemplate(pdfLoadedFieldItem.Size);
        RectangleF bounds2 = pdfLoadedFieldItem.Bounds with
        {
          Location = PointF.Empty
        };
        this.DrawComboBox(template.Graphics, pdfLoadedFieldItem);
        template.Graphics.DrawString(str, pdfLoadedFieldItem.Font, pdfLoadedFieldItem.ForeBrush, bounds2, pdfLoadedFieldItem.StringFormat);
        if (pdfLoadedFieldItem.Page != null && pdfLoadedFieldItem.Page.Graphics != null)
          pdfLoadedFieldItem.Page.Graphics.DrawPdfTemplate(template, pdfLoadedFieldItem.Bounds.Location);
      }
    }
    else
      this.DrawComboBox(this.Page.Graphics, (PdfLoadedFieldItem) null, str);
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
    PdfLoadedComboBoxField loadedComboBoxField = new PdfLoadedComboBoxField(dictionary, crossTable);
    loadedComboBoxField.Page = (PdfPageBase) page;
    loadedComboBoxField.SetName(this.GetFieldName());
    loadedComboBoxField.Widget.Dictionary = this.Widget.Dictionary.Clone(crossTable) as PdfDictionary;
    return (PdfField) loadedComboBoxField;
  }

  internal new PdfLoadedStyledField Clone()
  {
    PdfLoadedComboBoxField field = this.MemberwiseClone() as PdfLoadedComboBoxField;
    field.Dictionary = this.Dictionary.Clone(this.CrossTable) as PdfDictionary;
    field.Widget.Dictionary = this.Widget.Dictionary.Clone(this.CrossTable) as PdfDictionary;
    field.Items = new PdfLoadedComboBoxItemCollection();
    for (int index = 0; index < this.Items.Count; ++index)
    {
      PdfLoadedComboBoxItem loadedComboBoxItem = new PdfLoadedComboBoxItem((PdfLoadedStyledField) field, index, this.Items[index].Dictionary);
      field.Items.Add(loadedComboBoxItem);
    }
    return (PdfLoadedStyledField) field;
  }

  internal override PdfLoadedFieldItem CreateLoadedItem(PdfDictionary dictionary)
  {
    base.CreateLoadedItem(dictionary);
    PdfLoadedComboBoxItem loadedItem = new PdfLoadedComboBoxItem((PdfLoadedStyledField) this, this.m_items.Count, dictionary);
    this.m_items.Add(loadedItem);
    if (this.Kids == null)
      this.Dictionary["Kids"] = (IPdfPrimitive) new PdfArray();
    this.Kids.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) dictionary));
    return (PdfLoadedFieldItem) loadedItem;
  }

  private void ApplyAppearance(PdfDictionary widget, PdfLoadedFieldItem item)
  {
    if (widget != null && widget.ContainsKey("AP") && !this.Form.NeedAppearances)
    {
      if (!(this.CrossTable.GetObject(widget["AP"]) is PdfDictionary primitive) || !primitive.ContainsKey("N"))
        return;
      if (item != null)
      {
        RectangleF bounds1 = item.Bounds;
      }
      else
      {
        RectangleF bounds2 = this.Bounds;
      }
      PdfTemplate wrapper = new PdfTemplate(this.Bounds.Size);
      this.DrawComboBox(wrapper.Graphics, item);
      primitive.Remove("N");
      primitive.SetProperty("N", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) wrapper));
      widget.SetProperty("AP", (IPdfPrimitive) primitive);
    }
    else if (((PdfField) this).Form.ReadOnly || this.ReadOnly)
    {
      ((PdfField) this).Form.SetAppearanceDictionary = true;
    }
    else
    {
      if (!((PdfField) this).Form.SetAppearanceDictionary)
        return;
      ((PdfField) this).Form.NeedAppearances = true;
    }
  }

  private void DrawComboBox(PdfGraphics graphics, PdfLoadedFieldItem item)
  {
    PdfLoadedStyledField.GraphicsProperties graphicsProperties;
    this.GetGraphicsProperties(out graphicsProperties, item);
    graphicsProperties.Rect.Location = PointF.Empty;
    PaintParams paintParams = new PaintParams(graphicsProperties.Rect, graphicsProperties.BackBrush, graphicsProperties.ForeBrush, graphicsProperties.Pen, graphicsProperties.Style, graphicsProperties.BorderWidth, graphicsProperties.ShadowBrush, graphicsProperties.RotationAngle);
    string str = (string) null;
    if (this.SelectedItem.Count > 0)
    {
      int selectedIndex = this.SelectedIndex;
      if (this.SelectedIndex != -1 && !this.Flatten)
      {
        str = this.SelectedItem[0].Text;
        goto label_5;
      }
    }
    if (this.Dictionary.ContainsKey("DV") && !this.Flatten && PdfCrossTable.Dereference(this.Dictionary["DV"]) is PdfString pdfString)
      str = pdfString.Value;
label_5:
    if (this.SelectedItem.Count == 0)
    {
      string selectedValue = this.SelectedValue;
      FieldPainter.DrawComboBox(graphics, paintParams, selectedValue ?? "", graphicsProperties.Font, graphicsProperties.StringFormat);
    }
    else if (str != null && !this.Flatten)
      FieldPainter.DrawComboBox(graphics, paintParams, str ?? "", graphicsProperties.Font, graphicsProperties.StringFormat);
    else
      FieldPainter.DrawComboBox(graphics, paintParams);
  }

  private void DrawComboBox(PdfGraphics graphics, PdfLoadedFieldItem item, string text)
  {
    PdfLoadedStyledField.GraphicsProperties graphicsProperties;
    this.GetGraphicsProperties(out graphicsProperties, item);
    PaintParams paintParams = new PaintParams(graphicsProperties.Rect, graphicsProperties.BackBrush, graphicsProperties.ForeBrush, graphicsProperties.Pen, graphicsProperties.Style, graphicsProperties.BorderWidth, graphicsProperties.ShadowBrush, graphicsProperties.RotationAngle);
    if ((double) graphicsProperties.Font.Height > (double) this.Bounds.Height)
      this.SetFittingFontSize(ref graphicsProperties, paintParams, text);
    FieldPainter.DrawComboBox(graphics, paintParams, text, graphicsProperties.Font, graphicsProperties.StringFormat);
  }

  private void SetFittingFontSize(
    ref PdfLoadedStyledField.GraphicsProperties gp,
    PaintParams prms,
    string text)
  {
    float num1 = prms.BorderStyle == PdfBorderStyle.Beveled || prms.BorderStyle == PdfBorderStyle.Inset ? gp.Rect.Width - 8f * prms.BorderWidth : gp.Rect.Width - 4f * prms.BorderWidth;
    float num2 = gp.Rect.Height - 2f * gp.BorderWidth;
    float num3 = 0.248f;
    PdfStandardFont font = gp.Font as PdfStandardFont;
    if (text.EndsWith(" "))
      gp.StringFormat.MeasureTrailingSpaces = true;
    for (float num4 = 0.0f; (double) num4 <= (double) gp.Rect.Height; ++num4)
    {
      gp.Font.Size = !(gp.Font is PdfStandardFont) ? num4 : num4;
      SizeF sizeF = gp.Font.MeasureString(text, gp.StringFormat);
      if ((double) sizeF.Width > (double) gp.Rect.Width || (double) sizeF.Height > (double) num2)
      {
        float num5 = num4;
        do
        {
          num5 -= 1f / 1000f;
          gp.Font.Size = num5;
          float lineWidth = gp.Font.GetLineWidth(text, gp.StringFormat);
          if ((double) num5 < (double) num3)
          {
            gp.Font.Size = num3;
            break;
          }
          sizeF = gp.Font.MeasureString(text, gp.StringFormat);
          if ((double) lineWidth < (double) num1 && (double) sizeF.Height < (double) num2)
          {
            gp.Font.Size = num5;
            break;
          }
        }
        while ((double) num5 > (double) num3);
        break;
      }
    }
  }

  internal override float GetFontHeight(PdfFontFamily family)
  {
    List<float> floatList = new List<float>();
    foreach (PdfLoadedListItem pdfLoadedListItem in (PdfCollection) this.SelectedItem)
    {
      PdfFont pdfFont = (PdfFont) new PdfStandardFont(family, 12f);
      floatList.Add(pdfFont.MeasureString(pdfLoadedListItem.Text).Width);
    }
    if (this.SelectedItem.Count == 0 && this.Values.Count != 0)
    {
      PdfFont pdfFont = (PdfFont) new PdfStandardFont(family, 12f);
      float num = pdfFont.MeasureString(this.Values[0].Text).Width;
      int index = 1;
      for (int count = this.Values.Count; index < count; ++index)
      {
        float width = pdfFont.MeasureString(this.Values[index].Text).Width;
        num = (double) num > (double) width ? num : width;
        floatList.Add(num);
      }
    }
    floatList.Sort();
    float size = floatList.Count <= 0 ? 12f : (float) (12.0 * ((double) this.Bounds.Size.Width - 4.0 * (double) this.BorderWidth)) / floatList[floatList.Count - 1];
    if (this.SelectedItem.Count != 0)
    {
      PdfFont pdfFont = (PdfFont) new PdfStandardFont(family, size);
      string selectedValue = this.SelectedValue;
      SizeF sizeF1 = pdfFont.MeasureString(selectedValue);
      if ((double) sizeF1.Width > (double) this.Bounds.Width || (double) sizeF1.Height > (double) this.Bounds.Height)
      {
        float num1 = this.Bounds.Width - 4f * this.BorderWidth;
        float num2 = this.Bounds.Height - 4f * this.BorderWidth;
        float num3 = 0.248f;
        for (float num4 = 1f; (double) num4 <= (double) this.Bounds.Height; ++num4)
        {
          pdfFont.Size = num4;
          SizeF sizeF2 = pdfFont.MeasureString(selectedValue);
          if ((double) sizeF2.Width > (double) this.Bounds.Width || (double) sizeF2.Height > (double) num2)
          {
            float num5 = num4;
            do
            {
              num5 -= 1f / 1000f;
              pdfFont.Size = num5;
              float lineWidth = pdfFont.GetLineWidth(selectedValue, this.StringFormat);
              if ((double) num5 < (double) num3)
              {
                pdfFont.Size = num3;
                break;
              }
              sizeF2 = pdfFont.MeasureString(selectedValue, this.StringFormat);
              if ((double) lineWidth < (double) num1 && (double) sizeF2.Height < (double) num2)
              {
                pdfFont.Size = num5;
                break;
              }
            }
            while ((double) num5 > (double) num3);
            size = num5;
            break;
          }
        }
      }
    }
    else if ((double) size > 12.0)
      size = 12f;
    return size;
  }

  public void RemoveAt(int index)
  {
    if (this.Items == null || this.Items.Count == 0)
      return;
    this.Remove(this.Items[index]);
  }

  public void Remove(PdfLoadedComboBoxItem item)
  {
    int index = item != null ? this.Items.Remove(item) : throw new NullReferenceException(nameof (item));
    if (!(this.Dictionary["Kids"] is PdfArray pdfArray))
      return;
    pdfArray.RemoveAt(index);
    pdfArray.MarkChanged();
  }
}
