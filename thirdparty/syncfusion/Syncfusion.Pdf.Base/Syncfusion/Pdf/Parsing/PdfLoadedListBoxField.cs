// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfLoadedListBoxField
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

public class PdfLoadedListBoxField : PdfLoadedChoiceField
{
  private PdfLoadedListFieldItemCollection m_items;

  public new bool ComplexScript
  {
    get => base.ComplexScript;
    set => base.ComplexScript = value;
  }

  public bool MultiSelect
  {
    get => (FieldFlags.MultiSelect & this.Flags) != FieldFlags.Default;
    set
    {
      if (value)
        this.Flags |= FieldFlags.MultiSelect;
      else
        this.Flags &= ~FieldFlags.MultiSelect;
    }
  }

  public PdfLoadedListFieldItemCollection Items
  {
    get => this.m_items;
    internal set => this.m_items = value;
  }

  internal PdfLoadedListBoxField(PdfDictionary dictionary, PdfCrossTable crossTable)
    : base(dictionary, crossTable)
  {
    PdfArray kids = this.Kids;
    this.m_items = new PdfLoadedListFieldItemCollection();
    if (kids == null)
      return;
    for (int index = 0; index < kids.Count; ++index)
    {
      PdfDictionary dictionary1 = crossTable.GetObject(kids[index]) as PdfDictionary;
      this.m_items.Add(new PdfLoadedListFieldItem((PdfLoadedStyledField) this, index, dictionary1));
    }
  }

  internal override void Draw()
  {
    base.Draw();
    PdfGraphics graphics = this.Page.Graphics;
    PdfTemplate template1 = new PdfTemplate(this.Bounds.Size);
    if (this.Flatten && graphics.Page != null)
    {
      graphics.Save();
      if (graphics.Page.Rotation == PdfPageRotateAngle.RotateAngle90)
      {
        this.Page.Graphics.TranslateTransform(this.Page.Graphics.Size.Width, this.Page.Graphics.Size.Height);
        this.Page.Graphics.RotateTransform(90f);
      }
      else if (graphics.Page.Rotation == PdfPageRotateAngle.RotateAngle180)
      {
        this.Page.Graphics.TranslateTransform(this.Page.Graphics.Size.Width, this.Page.Graphics.Size.Height);
        this.Page.Graphics.RotateTransform(-180f);
      }
      else if (graphics.Page.Rotation == PdfPageRotateAngle.RotateAngle270)
      {
        this.Page.Graphics.TranslateTransform(this.Page.Graphics.Size.Width, this.Page.Graphics.Size.Height);
        this.Page.Graphics.RotateTransform(270f);
      }
    }
    PdfArray kids = this.Kids;
    if (kids != null && kids.Count > 1)
    {
      for (int index = 0; index < kids.Count; ++index)
      {
        PdfLoadedFieldItem pdfLoadedFieldItem = (PdfLoadedFieldItem) this.Items[index];
        PdfTemplate template2 = new PdfTemplate(pdfLoadedFieldItem.Size);
        this.DrawListBox(template2.Graphics, pdfLoadedFieldItem);
        if (pdfLoadedFieldItem.Page != null && pdfLoadedFieldItem.Page.Graphics != null)
          pdfLoadedFieldItem.Page.Graphics.DrawPdfTemplate(template2, this.Bounds.Location);
      }
    }
    else
    {
      this.DrawListBox(template1.Graphics, (PdfLoadedFieldItem) null);
      this.Page.Graphics.DrawPdfTemplate(template1, this.Bounds.Location);
    }
    graphics.Restore();
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
    PdfLoadedListBoxField loadedListBoxField = new PdfLoadedListBoxField(dictionary, crossTable);
    loadedListBoxField.Page = (PdfPageBase) page;
    loadedListBoxField.SetName(this.GetFieldName());
    loadedListBoxField.Widget.Dictionary = this.Widget.Dictionary.Clone(crossTable) as PdfDictionary;
    return (PdfField) loadedListBoxField;
  }

  internal new PdfLoadedStyledField Clone()
  {
    PdfLoadedListBoxField field = this.MemberwiseClone() as PdfLoadedListBoxField;
    field.Dictionary = this.Dictionary.Clone(this.CrossTable) as PdfDictionary;
    field.Widget.Dictionary = this.Widget.Dictionary.Clone(this.CrossTable) as PdfDictionary;
    field.Items = new PdfLoadedListFieldItemCollection();
    for (int index = 0; index < this.Items.Count; ++index)
    {
      PdfLoadedListFieldItem loadedListFieldItem = new PdfLoadedListFieldItem((PdfLoadedStyledField) field, index, this.Items[index].Dictionary);
      field.Items.Add(loadedListFieldItem);
    }
    return (PdfLoadedStyledField) field;
  }

  internal override PdfLoadedFieldItem CreateLoadedItem(PdfDictionary dictionary)
  {
    base.CreateLoadedItem(dictionary);
    PdfLoadedListFieldItem loadedItem = new PdfLoadedListFieldItem((PdfLoadedStyledField) this, this.m_items.Count, dictionary);
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
      PdfTemplate wrapper = new PdfTemplate((item == null ? this.Bounds : item.Bounds).Size, false);
      wrapper.Graphics.StreamWriter.BeginMarkupSequence("Tx");
      wrapper.Graphics.InitializeCoordinates();
      this.DrawListBox(wrapper.Graphics, item);
      wrapper.Graphics.StreamWriter.EndMarkupSequence();
      primitive.Remove("N");
      primitive.SetProperty("N", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) wrapper));
      widget.SetProperty("AP", (IPdfPrimitive) primitive);
    }
    else
    {
      if (!((PdfField) this).Form.SetAppearanceDictionary)
        return;
      ((PdfField) this).Form.NeedAppearances = true;
    }
  }

  private void DrawListBox(PdfGraphics graphics, PdfLoadedFieldItem item)
  {
    PdfLoadedStyledField.GraphicsProperties graphicsProperties;
    this.GetGraphicsProperties(out graphicsProperties, item);
    graphicsProperties.Rect.Location = PointF.Empty;
    PaintParams paintParams = new PaintParams(graphicsProperties.Rect, graphicsProperties.BackBrush, graphicsProperties.ForeBrush, graphicsProperties.Pen, graphicsProperties.Style, graphicsProperties.BorderWidth, graphicsProperties.ShadowBrush, graphicsProperties.RotationAngle);
    if (!this.Form.SetAppearanceDictionary && !this.Form.Flatten)
      paintParams.BackBrush = (PdfBrush) null;
    PdfListFieldItemCollection listItems = this.ConvertToListItems(this.Values);
    FieldPainter.DrawListBox(graphics, paintParams, listItems, this.SelectedIndex, graphicsProperties.Font, graphicsProperties.StringFormat);
  }

  private PdfListFieldItemCollection ConvertToListItems(PdfLoadedListItemCollection items)
  {
    PdfListFieldItemCollection listItems = new PdfListFieldItemCollection();
    foreach (PdfLoadedListItem pdfLoadedListItem in (PdfCollection) items)
      listItems.Add(new PdfListFieldItem(pdfLoadedListItem.Text, pdfLoadedListItem.Value));
    return listItems;
  }

  internal override float GetFontHeight(PdfFontFamily family)
  {
    PdfLoadedListItemCollection values = this.Values;
    float fontHeight = 0.0f;
    if (values.Count > 0)
    {
      PdfFont pdfFont = (PdfFont) new PdfStandardFont(family, 12f);
      float num1 = pdfFont.MeasureString(values[0].Text).Width;
      int index = 1;
      for (int count = values.Count; index < count; ++index)
      {
        float width = pdfFont.MeasureString(values[index].Text).Width;
        num1 = (double) num1 > (double) width ? num1 : width;
      }
      float num2 = (float) (12.0 * ((double) this.Bounds.Size.Width - 4.0 * (double) this.BorderWidth)) / num1;
      fontHeight = (double) num2 > 12.0 ? 12f : num2;
    }
    return fontHeight;
  }

  public void RemoveAt(int index)
  {
    if (this.Items == null || this.Items.Count == 0)
      return;
    this.Remove(this.Items[index]);
  }

  public void Remove(PdfLoadedListFieldItem item)
  {
    int index = item != null ? this.Items.Remove(item) : throw new NullReferenceException(nameof (item));
    if (!(this.Dictionary["Kids"] is PdfArray pdfArray))
      return;
    pdfArray.RemoveAt(index);
    pdfArray.MarkChanged();
  }
}
