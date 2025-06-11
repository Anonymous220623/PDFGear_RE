// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfLoadedTextBoxField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Native;
using Syncfusion.Pdf.Primitives;
using Syncfusion.Pdf.Xfa;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class PdfLoadedTextBoxField : PdfLoadedStyledField
{
  private const string m_passwordValue = "*";
  private PdfLoadedTextBoxItemCollection m_items;
  private PdfColor m_foreColor = new PdfColor((byte) 0, (byte) 0, (byte) 0);
  private bool m_autoSize;
  private bool m_applyAppearence = true;

  public new bool ComplexScript
  {
    get => base.ComplexScript;
    set => base.ComplexScript = value;
  }

  public PdfColor BackColor
  {
    get => this.GetBackColor();
    set => this.AssignBackColor(value);
  }

  public new virtual PdfColor ForeColor
  {
    get
    {
      PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
      if (widgetAnnotation != null && widgetAnnotation.ContainsKey("DA"))
        this.m_foreColor = this.GetForeColour((this.CrossTable.GetObject(widgetAnnotation["DA"]) as PdfString).Value);
      else if (widgetAnnotation != null && widgetAnnotation.GetValue(this.CrossTable, "DA", "Parent") is PdfString pdfString)
        this.m_foreColor = this.GetForeColour(pdfString.Value);
      return this.m_foreColor;
    }
    set
    {
      PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
      float height = 0.0f;
      string key = (string) null;
      if (widgetAnnotation != null && widgetAnnotation.ContainsKey("DA"))
      {
        PdfDictionary pdfDictionary = this.CrossTable.GetObject(this.Form.Resources["Font"]) as PdfDictionary;
        key = this.FontName((widgetAnnotation["DA"] as PdfString).Value, out height);
        PdfReferenceHolder pdfReferenceHolder = pdfDictionary[key] as PdfReferenceHolder;
        if (pdfReferenceHolder != (PdfReferenceHolder) null)
        {
          IPdfPrimitive pdfPrimitive = pdfReferenceHolder.Object;
        }
      }
      else if (widgetAnnotation != null && this.Dictionary.ContainsKey("DA"))
      {
        PdfDictionary pdfDictionary = this.CrossTable.GetObject(this.Form.Resources["Font"]) as PdfDictionary;
        key = this.FontName((this.Dictionary["DA"] as PdfString).Value, out height);
        IPdfPrimitive pdfPrimitive = (pdfDictionary[key] as PdfReferenceHolder).Object;
      }
      if (key != null)
        widgetAnnotation["DA"] = (IPdfPrimitive) new PdfString(new PdfDefaultAppearance()
        {
          FontName = key,
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

  public PdfTextAlignment TextAlignment
  {
    get
    {
      PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
      PdfTextAlignment textAlignment = PdfTextAlignment.Left;
      if (widgetAnnotation.ContainsKey("Q"))
        textAlignment = (PdfTextAlignment) Enum.ToObject(typeof (PdfTextAlignment), (widgetAnnotation["Q"] as PdfNumber).IntValue);
      else if (widgetAnnotation != null && widgetAnnotation.GetValue(this.CrossTable, "Q", "Parent") is PdfNumber pdfNumber)
        textAlignment = (PdfTextAlignment) Enum.ToObject(typeof (PdfTextAlignment), pdfNumber.IntValue);
      return textAlignment;
    }
    set
    {
      if (this.Dictionary.ContainsKey("Kids"))
      {
        PdfArray pdfArray = this.CrossTable.GetObject(this.Dictionary["Kids"]) as PdfArray;
        for (int index = 0; index < pdfArray.Count; ++index)
          (PdfCrossTable.Dereference(pdfArray[index]) as PdfDictionary).SetProperty("Q", (IPdfPrimitive) new PdfNumber((int) value));
      }
      else
        this.GetWidgetAnnotation(this.Dictionary, this.CrossTable).SetProperty("Q", (IPdfPrimitive) new PdfNumber((int) value));
      ((PdfField) this).Form.SetAppearanceDictionary = true;
    }
  }

  public PdfHighlightMode HighlightMode
  {
    get
    {
      PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
      PdfHighlightMode highlightMode = PdfHighlightMode.NoHighlighting;
      if (widgetAnnotation.ContainsKey("H"))
        highlightMode = this.GetHighlightModeFromString(this.CrossTable.GetObject(widgetAnnotation["H"]) as PdfName);
      return highlightMode;
    }
    set
    {
      this.GetWidgetAnnotation(this.Dictionary, this.CrossTable)["H"] = (IPdfPrimitive) new PdfName(this.HighlightModeToString(value));
    }
  }

  public string Text
  {
    get
    {
      string empty = string.Empty;
      PdfReferenceHolder pdfReferenceHolder = this.Dictionary["V"] as PdfReferenceHolder;
      PdfString pdfString;
      if (pdfReferenceHolder != (PdfReferenceHolder) null)
      {
        switch (PdfCrossTable.Dereference((IPdfPrimitive) pdfReferenceHolder))
        {
          case PdfStream _:
            PdfStream pdfStream = pdfReferenceHolder.Object as PdfStream;
            pdfStream.Decompress();
            byte[] numArray = new byte[pdfStream.InternalStream.Length];
            pdfStream.InternalStream.Position = 0L;
            pdfStream.InternalStream.Read(numArray, 0, (int) pdfStream.InternalStream.Length);
            pdfString = new PdfString(Encoding.ASCII.GetString(numArray));
            break;
          case PdfString _:
            pdfString = PdfLoadedField.GetValue(this.Dictionary, this.CrossTable, "V", true) as PdfString;
            break;
          default:
            pdfString = new PdfString(string.Empty);
            break;
        }
      }
      else
        pdfString = PdfLoadedField.GetValue(this.Dictionary, this.CrossTable, "V", true) as PdfString;
      if (pdfString == null && this.Items != null && this.Items.Count > 0)
        pdfString = PdfLoadedField.GetValue(this.Items[0].Dictionary, this.CrossTable, "V", true) as PdfString;
      return pdfString == null ? string.Empty : pdfString.Value;
    }
    set
    {
      if ((FieldFlags.ReadOnly & this.Flags) == FieldFlags.Default)
      {
        if (value == null)
          throw new ArgumentNullException("text");
        this.m_isTextChanged = true;
        if (!string.IsNullOrEmpty(this.Text))
          this.m_isTextModified = true;
        if (this.Dictionary.ContainsKey("AA") && this.Dictionary["AA"] is PdfDictionary pdfDictionary1)
        {
          PdfReferenceHolder pdfReferenceHolder = pdfDictionary1["K"] as PdfReferenceHolder;
          if (pdfReferenceHolder != (PdfReferenceHolder) null && pdfReferenceHolder.Object is PdfDictionary pdfDictionary && PdfCrossTable.Dereference(pdfDictionary["JS"]) is PdfString pdfString)
            this.Dictionary.SetProperty("V", (IPdfPrimitive) new PdfString(pdfString.Value));
        }
        this.Dictionary.SetProperty("V", (IPdfPrimitive) new PdfString(value));
        if (this.Items != null && this.Items.Count > 0)
        {
          PdfLoadedTexBoxItem loadedTexBoxItem = this.Items[0];
          if (loadedTexBoxItem != null && loadedTexBoxItem.Dictionary.ContainsKey("V"))
            loadedTexBoxItem.Dictionary.SetProperty("V", (IPdfPrimitive) new PdfString(value));
        }
        this.CrossTable.GetObject(this.GetWidgetAnnotation(this.Dictionary, this.CrossTable)["MK"]);
        this.Changed = true;
        ((PdfField) this).Form.SetAppearanceDictionary = true;
        if (this.Form is PdfLoadedForm form && form.isUR3)
          this.Dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
      }
      else
        this.Changed = false;
      if (!(this.Form is PdfLoadedForm form1) || !form1.EnableXfaFormFill || !form1.IsXFAForm)
        return;
      string fieldName = this.Name.Replace("\\", string.Empty);
      PdfLoadedXfaField xfaField = form1.GetXfaField(fieldName);
      if (xfaField == null)
        return;
      this.UpdateXfaFieldData(xfaField);
    }
  }

  public string DefaultValue
  {
    get
    {
      string defaultValue = (string) null;
      if (PdfLoadedField.GetValue(this.Dictionary, this.CrossTable, "DV", true) is PdfString pdfString)
        defaultValue = pdfString.Value;
      return defaultValue;
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (DefaultValue));
      this.Dictionary.SetString("DV", value);
      this.Changed = true;
    }
  }

  public bool SpellCheck
  {
    get => (FieldFlags.DoNotSpellCheck & this.Flags) == FieldFlags.Default;
    set
    {
      if (value)
        this.Flags &= ~FieldFlags.DoNotSpellCheck;
      else
        this.Flags |= FieldFlags.DoNotSpellCheck;
    }
  }

  public bool InsertSpaces
  {
    get
    {
      return (FieldFlags.Comb & this.Flags) != FieldFlags.Default && (this.Flags & FieldFlags.Multiline) == FieldFlags.Default && (this.Flags & FieldFlags.Password) == FieldFlags.Default && (this.Flags & FieldFlags.FileSelect) == FieldFlags.Default;
    }
    set
    {
      if (value)
        this.Flags |= FieldFlags.Comb;
      else
        this.Flags &= ~FieldFlags.Comb;
    }
  }

  public bool Multiline
  {
    get => (FieldFlags.Multiline & this.Flags) != FieldFlags.Default;
    set
    {
      if (value)
        this.Flags |= FieldFlags.Multiline;
      else
        this.Flags &= ~FieldFlags.Multiline;
    }
  }

  public bool Password
  {
    get => (FieldFlags.Password & this.Flags) != FieldFlags.Default;
    set
    {
      if (value)
        this.Flags |= FieldFlags.Password;
      else
        this.Flags &= ~FieldFlags.Password;
    }
  }

  public bool Scrollable
  {
    get => (FieldFlags.DoNotScroll & this.Flags) == FieldFlags.Default;
    set
    {
      if (value)
        this.Flags &= ~FieldFlags.DoNotScroll;
      else
        this.Flags |= FieldFlags.DoNotScroll;
    }
  }

  public int MaxLength
  {
    get
    {
      int maxLength = 0;
      if (PdfLoadedField.GetValue(this.Dictionary, this.CrossTable, "MaxLen", true) is PdfNumber pdfNumber)
        maxLength = pdfNumber.IntValue;
      return maxLength;
    }
    set
    {
      this.Dictionary.SetNumber("MaxLen", value);
      this.Changed = true;
    }
  }

  public bool IsAutoFontSize
  {
    get
    {
      bool isAutoFontSize = false;
      if (this.CrossTable.Document is PdfLoadedDocument && (this.CrossTable.Document as PdfLoadedDocument).Form.Dictionary.ContainsKey("DA"))
      {
        PdfString pdfString1 = (this.CrossTable.Document as PdfLoadedDocument).Form.Dictionary.Items[new PdfName("DA")] as PdfString;
        float height = 0.0f;
        this.FontName(pdfString1.Value, out height);
        if ((double) height == 0.0)
        {
          if (this.Dictionary.ContainsKey("Kids"))
          {
            bool flag = false;
            PdfArray pdfArray = this.Dictionary["Kids"] as PdfArray;
            if (this.Dictionary.ContainsKey("DA"))
            {
              PdfString pdfString2 = this.Dictionary.Items[new PdfName("DA")] as PdfString;
              height = 0.0f;
              this.FontName(pdfString2.Value, out height);
              if ((double) height == 0.0)
                flag = true;
            }
            if (flag || !this.Dictionary.ContainsKey("DA"))
            {
              foreach (PdfReferenceHolder element in pdfArray.Elements)
              {
                PdfDictionary pdfDictionary = element.Object as PdfDictionary;
                if (!pdfDictionary.ContainsKey("DA"))
                {
                  isAutoFontSize = true;
                }
                else
                {
                  PdfString pdfString3 = pdfDictionary.Items[new PdfName("DA")] as PdfString;
                  height = 0.0f;
                  this.FontName(pdfString3.Value, out height);
                  if ((double) height == 0.0)
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
            height = 0.0f;
            this.FontName(pdfString4.Value, out height);
            if ((double) height == 0.0)
              isAutoFontSize = true;
          }
        }
      }
      return isAutoFontSize;
    }
  }

  public bool AutoResizeText
  {
    get => this.m_autoSize;
    set
    {
      this.m_autoSize = value;
      this.Changed = true;
    }
  }

  public PdfLoadedTextBoxItemCollection Items
  {
    get => this.m_items;
    internal set => this.m_items = value;
  }

  internal PdfLoadedTextBoxField(PdfDictionary dictionary, PdfCrossTable crossTable)
    : base(dictionary, crossTable)
  {
    PdfArray kids = this.Kids;
    this.m_items = new PdfLoadedTextBoxItemCollection();
    if (kids == null)
      return;
    for (int index = 0; index < kids.Count; ++index)
    {
      PdfDictionary dictionary1 = crossTable.GetObject(kids[index]) as PdfDictionary;
      this.m_items.Add(new PdfLoadedTexBoxItem((PdfLoadedStyledField) this, index, dictionary1));
    }
  }

  private void UpdateXfaFieldData(PdfLoadedXfaField field)
  {
    switch (field)
    {
      case PdfLoadedXfaTextBoxField _:
        (field as PdfLoadedXfaTextBoxField).Text = this.Text;
        break;
      case PdfLoadedXfaNumericField _:
        PdfLoadedXfaNumericField loadedXfaNumericField = field as PdfLoadedXfaNumericField;
        double result1;
        if (double.TryParse(this.Text, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result1))
        {
          loadedXfaNumericField.NumericValue = result1;
          break;
        }
        loadedXfaNumericField.NumericValue = double.NaN;
        break;
      case PdfLoadedXfaDateTimeField _:
        PdfLoadedXfaDateTimeField xfaDateTimeField = field as PdfLoadedXfaDateTimeField;
        DateTime result2;
        if (DateTime.TryParse(this.Text, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result2))
        {
          xfaDateTimeField.SetDate(new DateTime?(result2));
          break;
        }
        xfaDateTimeField.SetDate(new DateTime?());
        break;
    }
  }

  internal virtual void Dictionary_BeginSave(object sender, SavePdfPrimitiveEventArgs ars)
  {
    this.BeginSave();
  }

  private string HighlightModeToString(PdfHighlightMode m_highlightingMode)
  {
    switch (m_highlightingMode)
    {
      case PdfHighlightMode.NoHighlighting:
        return "N";
      case PdfHighlightMode.Outline:
        return "O";
      case PdfHighlightMode.Push:
        return "P";
      default:
        return "I";
    }
  }

  private PdfHighlightMode GetHighlightModeFromString(PdfName hightlightMode)
  {
    switch (hightlightMode.Value)
    {
      case "P":
        return PdfHighlightMode.Push;
      case "N":
        return PdfHighlightMode.NoHighlighting;
      case "O":
        return PdfHighlightMode.Outline;
      default:
        return PdfHighlightMode.Invert;
    }
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
    PdfLoadedTextBoxField loadedTextBoxField = new PdfLoadedTextBoxField(dictionary, crossTable);
    loadedTextBoxField.Page = (PdfPageBase) page;
    loadedTextBoxField.SetName(this.GetFieldName());
    loadedTextBoxField.Widget.Dictionary = this.Widget.Dictionary.Clone(crossTable) as PdfDictionary;
    return (PdfField) loadedTextBoxField;
  }

  internal new PdfLoadedStyledField Clone()
  {
    PdfLoadedTextBoxField field = this.MemberwiseClone() as PdfLoadedTextBoxField;
    this.Dictionary.m_clonedObject = (PdfDictionary) null;
    field.Dictionary = this.Dictionary.Clone(this.CrossTable) as PdfDictionary;
    field.Widget.Dictionary = this.Widget.Dictionary.Clone(this.CrossTable) as PdfDictionary;
    field.Items = new PdfLoadedTextBoxItemCollection();
    for (int index = 0; index < this.Items.Count; ++index)
    {
      PdfLoadedTexBoxItem loadedTexBoxItem = new PdfLoadedTexBoxItem((PdfLoadedStyledField) field, index, this.Items[index].Dictionary);
      field.Items.Add(loadedTexBoxItem);
    }
    return (PdfLoadedStyledField) field;
  }

  internal override PdfLoadedFieldItem CreateLoadedItem(PdfDictionary dictionary)
  {
    base.CreateLoadedItem(dictionary);
    PdfLoadedTexBoxItem loadedItem = new PdfLoadedTexBoxItem((PdfLoadedStyledField) this, this.m_items.Count, dictionary);
    this.m_items.Add(loadedItem);
    if (this.Kids == null)
      this.Dictionary["Kids"] = (IPdfPrimitive) new PdfArray();
    this.Kids.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) dictionary));
    return (PdfLoadedFieldItem) loadedItem;
  }

  private void ApplyAppearance(PdfDictionary widget, PdfLoadedFieldItem item)
  {
    bool needAppearances = ((PdfField) this).Form.NeedAppearances;
    if (!((PdfField) this).Form.SetAppearanceDictionary)
      return;
    if (widget != null && !needAppearances)
    {
      if (this.AutoResizeText && widget.ContainsKey("DA"))
        widget.Remove("DA");
      PdfDictionary pdfDictionary1 = this.CrossTable.GetObject(widget["AP"]) as PdfDictionary;
      PdfDictionary primitive = new PdfDictionary();
      RectangleF rectangleF = item == null ? this.Bounds : item.Bounds;
      PdfTemplate wrapper = (PdfTemplate) null;
      if (widget.ContainsKey("MK") && widget["MK"] is PdfDictionary pdfDictionary2 && pdfDictionary2.ContainsKey("R") && pdfDictionary2["R"] is PdfNumber pdfNumber)
      {
        if ((double) pdfNumber.FloatValue == 90.0)
        {
          wrapper = new PdfTemplate(new SizeF(rectangleF.Size.Height, rectangleF.Size.Width), false);
          wrapper.m_content["Matrix"] = (IPdfPrimitive) new PdfArray(new float[6]
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
          wrapper = new PdfTemplate(rectangleF.Size, false);
          wrapper.m_content["Matrix"] = (IPdfPrimitive) new PdfArray(new float[6]
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
          wrapper = new PdfTemplate(new SizeF(rectangleF.Size.Height, rectangleF.Size.Width), false);
          wrapper.m_content["Matrix"] = (IPdfPrimitive) new PdfArray(new float[6]
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
      if (wrapper == null)
      {
        wrapper = new PdfTemplate(rectangleF.Size, false);
        wrapper.m_content["Matrix"] = (IPdfPrimitive) new PdfArray(new float[6]
        {
          1f,
          0.0f,
          0.0f,
          1f,
          0.0f,
          0.0f
        });
      }
      wrapper.Graphics.StreamWriter.BeginMarkupSequence("Tx");
      wrapper.Graphics.InitializeCoordinates();
      this.DrawTextBox(wrapper.Graphics, item);
      wrapper.Graphics.StreamWriter.EndMarkupSequence();
      primitive.SetProperty("N", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) wrapper));
      widget.SetProperty("AP", (IPdfPrimitive) primitive);
    }
    else
      ((PdfField) this).Form.NeedAppearances = true;
  }

  internal override void Draw()
  {
    base.Draw();
    PdfArray kids = this.Kids;
    if (kids != null)
    {
      for (int index1 = 0; index1 < kids.Count; ++index1)
      {
        PdfLoadedFieldItem pdfLoadedFieldItem = (PdfLoadedFieldItem) this.Items[index1];
        if (pdfLoadedFieldItem.Page != null && pdfLoadedFieldItem.Page is PdfLoadedPage)
          this.DrawTextBox(pdfLoadedFieldItem.Page.Graphics, pdfLoadedFieldItem);
        else if (PdfCrossTable.Dereference(kids[index1]) is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("P") && (pdfDictionary["P"] as PdfReferenceHolder).Reference == (PdfReference) null)
        {
          PdfPage pdfPage = (PdfPage) null;
          bool flag = false;
          if (pdfLoadedFieldItem != null && pdfLoadedFieldItem.Page != null)
            pdfPage = pdfLoadedFieldItem.Page as PdfPage;
          PdfPageBase pdfPageBase;
          if (pdfPage != null && pdfPage.Imported)
          {
            pdfPageBase = (PdfPageBase) pdfPage;
          }
          else
          {
            if (this.Page != null && this.Page is PdfPage page && page.Section != null && page.Section.ParentDocument != null)
              flag = page.Section.ParentDocument.EnableMemoryOptimization;
            pdfPageBase = !flag || pdfLoadedFieldItem == null || pdfLoadedFieldItem.Page == null ? this.Page : pdfLoadedFieldItem.Page;
          }
          if (pdfPageBase != null)
            this.DrawTextBox(pdfPageBase.Graphics, pdfLoadedFieldItem);
        }
        else if (this.Form.m_pageMap.Count > 0 && pdfDictionary != null && PdfCrossTable.Dereference(pdfDictionary["P"]) is PdfDictionary key)
        {
          PdfPageBase page = this.Form.m_pageMap[key];
          if (page.Dictionary["Annots"] is PdfArray pdfArray)
          {
            for (int index2 = 0; index2 < pdfArray.Count - 1; ++index2)
            {
              if ((object) (pdfArray[index2] as PdfReferenceHolder) != null && PdfCrossTable.Dereference(pdfArray[index2]) is PdfDictionary pdfDictionary1 && pdfDictionary1.ContainsKey("Parent") && PdfCrossTable.Dereference(pdfDictionary1["Parent"]) is PdfDictionary pdfDictionary2 && pdfDictionary2.ContainsKey("T") && pdfDictionary2["T"] is PdfString pdfString && pdfString.Value == this.Name)
                pdfArray.RemoveAt(index2);
            }
          }
          this.DrawTextBox(page.Graphics, pdfLoadedFieldItem);
        }
      }
    }
    else
      this.DrawTextBox(this.Page.Graphics, (PdfLoadedFieldItem) null);
  }

  private void DrawTextBox(PdfGraphics graphics, PdfLoadedFieldItem item)
  {
    PdfLoadedStyledField.GraphicsProperties graphicsProperties;
    this.GetGraphicsProperties(out graphicsProperties, item);
    if (this.Flatten && (double) graphicsProperties.BorderWidth == 0.0 && graphicsProperties.Pen != null && !this.GetWidgetAnnotation(this.Dictionary, this.CrossTable).ContainsKey("BS"))
    {
      graphicsProperties.BorderWidth = 1f;
      graphicsProperties.Pen.Width = 1f;
    }
    if (graphics.Layer == null)
      graphicsProperties.Rect.Size = graphics.Size;
    if (!this.Flatten)
      graphicsProperties.Rect.Location = new PointF(0.0f, 0.0f);
    string str = this.Text;
    if (this.Password)
    {
      str = string.Empty;
      for (int index = 0; index < this.Text.Length; ++index)
        str += "*";
    }
    PdfBrush backBrush = graphicsProperties.BackBrush;
    ushort[] numArray = new ushort[str.Length];
    KernelApi.GetStringTypeExW(2048U /*0x0800*/, StringInfoType.CT_TYPE2, str, str.Length, numArray);
    graphicsProperties.StringFormat.RightToLeft = this.IsRTLText(numArray);
    graphicsProperties.StringFormat.LineLimit = false;
    if (!this.Multiline)
    {
      graphicsProperties.StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
      if (((PdfField) this).Form.NeedAppearances)
        graphicsProperties.StringFormat.WordWrap = PdfWordWrapType.None;
    }
    if (!this.Multiline && this.Flatten)
    {
      graphicsProperties.StringFormat.WordWrap = PdfWordWrapType.Character;
      if ((double) graphicsProperties.Font.Height < (double) graphicsProperties.Rect.Height)
        graphicsProperties.StringFormat.LineLimit = true;
    }
    if (graphicsProperties.Font is PdfTrueTypeFont && ((PdfField) this).Form.SetAppearanceDictionary && !this.Flatten && graphicsProperties.RotationAngle == 0 && str.Length < (int) byte.MaxValue && !this.Flatten)
    {
      SizeF sizeF = graphicsProperties.Font.MeasureString(str);
      if ((double) sizeF.Width > (double) graphicsProperties.Rect.Width && (double) sizeF.Height < (double) graphicsProperties.Rect.Height && !this.InsertSpaces && !this.Multiline)
        graphicsProperties.Rect.Width = sizeF.Width;
    }
    PaintParams paintParams = new PaintParams(graphicsProperties.Rect, graphicsProperties.BackBrush, graphicsProperties.ForeBrush, graphicsProperties.Pen, graphicsProperties.Style, graphicsProperties.BorderWidth, graphicsProperties.ShadowBrush, graphicsProperties.RotationAngle);
    if (this.Page != null)
      paintParams.PageRotationAngle = this.Page.Rotation;
    paintParams.isFlatten = this.Flatten;
    paintParams.InsertSpace = this.InsertSpaces;
    if (graphicsProperties.Font.Name.Equals("TimesLTStd-Roman") && graphicsProperties.Font is PdfStandardFont font1)
      graphicsProperties.Font = (PdfFont) new PdfStandardFont(font1.FontFamily, graphicsProperties.Font.Size, graphicsProperties.Font.Style);
    bool flag1 = false;
    if (graphicsProperties.Font is PdfStandardFont)
    {
      PdfStandardFont font2 = graphicsProperties.Font as PdfStandardFont;
      if (!graphicsProperties.Font.Name.Equals(font2.FontFamily.ToString()) && this.Page != null)
      {
        this.Page.GetResources().OriginalFontName = font2.Name;
        flag1 = true;
      }
    }
    if (PdfString.IsUnicode(str) && this.Font is PdfStandardFont)
      graphics.isStandardUnicode = this.IsUnicodeStandardFont();
    if (str != string.Empty && (this.Flatten || this.m_isCustomFontSize) && (this.AutoResizeText || this.m_isCustomFontSize) && !this.Multiline)
      this.SetFittingFontSize(ref graphicsProperties, paintParams, str);
    else if ((this.AutoResizeText || this.m_isCustomFontSize) && this.Multiline)
    {
      if (!((PdfField) this).Form.NeedAppearances && !this.Flatten)
      {
        PdfDictionary pdfDictionary1 = (PdfDictionary) null;
        float height = 0.0f;
        string key = "";
        if (this.Dictionary["DA"] is PdfString pdfString)
          key = this.FontName(pdfString.Value, out height);
        if (PdfCrossTable.Dereference(graphics.StreamWriter.GetStream()["Resources"]) is PdfDictionary pdfDictionary3 && pdfDictionary3.ContainsKey("Font"))
        {
          PdfDictionary pdfDictionary2 = PdfCrossTable.Dereference(pdfDictionary3["Font"]) as PdfDictionary;
          if (!pdfDictionary2.ContainsKey(key))
          {
            pdfDictionary2[new PdfName(key)] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.Font);
            pdfDictionary3["Font"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary2);
          }
        }
        else if (pdfDictionary1 == null)
          pdfDictionary3["Font"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) new PdfDictionary()
          {
            [new PdfName(key)] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.Font)
          });
        if ((double) graphicsProperties.BorderWidth == 0.0 && graphicsProperties.Pen != null && !this.GetWidgetAnnotation(this.Dictionary, this.CrossTable).ContainsKey("BS"))
        {
          graphicsProperties.BorderWidth = 1f;
          graphicsProperties.Pen.Width = 1f;
        }
      }
      if (str != string.Empty)
        this.SetMultiLineFontSize(ref graphicsProperties, str);
      FieldPainter.isAutoFontSize = true;
    }
    if (graphics != null && graphics.IsTemplateGraphics)
    {
      graphics.Save();
      if (this.AutoResizeText)
        this.SetFittingFontSize(ref graphicsProperties, paintParams, str);
      FieldPainter.DrawTextBox(graphics, paintParams, str, graphicsProperties.Font, graphicsProperties.StringFormat, this.Multiline, this.Scrollable, this.MaxLength);
      graphics.Restore();
    }
    else
    {
      graphics.Save();
      if (this.Dictionary.ContainsKey("Rect") || this.Dictionary.ContainsKey("Kids"))
      {
        if (this.InsertSpaces && this.MaxLength != 0)
        {
          FieldPainter.DrawTextBox(graphics, paintParams, str, graphicsProperties.Font, graphicsProperties.StringFormat, this.Multiline, this.Scrollable, this.MaxLength);
        }
        else
        {
          bool flag2 = false;
          if (this.DefaultValue != null && this.DefaultValue == this.Text && PdfString.IsUnicode(this.DefaultValue) && graphicsProperties.Font is PdfStandardFont && (!this.Dictionary.ContainsKey("DA") || this.Dictionary.ContainsKey("AP")) && !this.Dictionary.ContainsKey("Q"))
            flag2 = true;
          if (!this.AutoResizeText && this.RotationAngle == 0 && !this.m_isTextChanged && !flag2 && !this.Form.Dictionary.ContainsKey("NeedAppearances") && !this.Dictionary.ContainsKey("DV") && (PdfString.IsUnicode(this.Text) && this.Dictionary.ContainsKey("AP") || this.Dictionary.ContainsKey("AP") && this.Dictionary.ContainsKey("PMD") && this.Flatten || item != null && item.Dictionary != null && item.Dictionary.ContainsKey("AP") && item.Dictionary.ContainsKey("PMD") && this.Flatten))
            flag2 = true;
          if (this.m_isCustomFontSize && this.m_applyAppearence && this.Dictionary.ContainsKey("AP") && !this.m_isTextChanged && graphicsProperties.Font is PdfTrueTypeFont)
            flag2 = true;
          if (flag1 && !this.AutoResizeText && !flag2 && this.RotationAngle == 0 && !this.m_isTextChanged && this.Dictionary.ContainsKey("AP") && PdfCrossTable.Dereference(this.Dictionary["AP"]) is PdfDictionary pdfDictionary4 && pdfDictionary4.ContainsKey("N") && PdfCrossTable.Dereference(pdfDictionary4["N"]) is PdfStream apperenceStream)
            flag2 = this.FindAppearanceDrawFromStream(apperenceStream);
          if (!flag1 && !flag2 && graphicsProperties.Font is PdfStandardFont && this.Dictionary.ContainsKey("Parent") && this.Dictionary.ContainsKey("Kids") && (graphicsProperties.Font as PdfStandardFont).FontInternal is PdfDictionary fontInternal && fontInternal.ContainsKey("Encoding") && PdfCrossTable.Dereference(fontInternal["Encoding"]) is PdfDictionary pdfDictionary7 && pdfDictionary7.ContainsKey("Differences") && !fontInternal.ContainsKey("Widths") && !this.m_isTextChanged && this.Dictionary.ContainsKey("Kids"))
          {
            PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
            PdfDictionary pdfDictionary5 = PdfCrossTable.Dereference(widgetAnnotation["MK"]) as PdfDictionary;
            pdfNumber = (PdfNumber) null;
            int num = 0;
            if (pdfDictionary5 != null && pdfDictionary5.ContainsKey("R") && PdfCrossTable.Dereference(pdfDictionary5["R"]) is PdfNumber pdfNumber)
              num = pdfNumber.IntValue;
            if (widgetAnnotation != null && (pdfNumber != null && num == 0 || pdfNumber == null) && this.RotationAngle == 0 && widgetAnnotation.ContainsKey("AP") && PdfCrossTable.Dereference(widgetAnnotation["AP"]) is PdfDictionary pdfDictionary6 && pdfDictionary6.ContainsKey("N") && PdfCrossTable.Dereference(pdfDictionary6["N"]) is PdfStream pdfStream && pdfStream.Data.Length > 0)
              flag2 = true;
          }
          if (flag2 || this.Dictionary.ContainsKey("Kids") && !this.Dictionary.ContainsKey("DA") || this.Dictionary.ContainsKey("AP") && this.Dictionary.ContainsKey("AA") && !this.Dictionary.ContainsKey("Q") && !this.Dictionary.ContainsKey("BS") && this.Dictionary.ContainsKey("Parent"))
          {
            PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
            if (widgetAnnotation.ContainsKey("AP") && !this.m_isTextChanged && this.m_applyAppearence)
            {
              if (this.Flatten && graphics.Page != null)
              {
                graphics.Save();
                if (graphics.Page.Rotation == PdfPageRotateAngle.RotateAngle90)
                {
                  graphics.TranslateTransform(graphics.Size.Width, graphics.Size.Height);
                  graphics.RotateTransform(90f);
                }
                else if (graphics.Page.Rotation == PdfPageRotateAngle.RotateAngle180)
                {
                  graphics.TranslateTransform(graphics.Size.Width, graphics.Size.Height);
                  graphics.RotateTransform(-180f);
                }
                else if (graphics.Page.Rotation == PdfPageRotateAngle.RotateAngle270)
                {
                  graphics.TranslateTransform(graphics.Size.Width, graphics.Size.Height);
                  graphics.RotateTransform(270f);
                }
              }
              if (PdfCrossTable.Dereference(widgetAnnotation["AP"]) is PdfDictionary pdfDictionary9)
              {
                IPdfPrimitive pdfPrimitive = pdfDictionary9["N"];
                if (pdfPrimitive != null && PdfCrossTable.Dereference(pdfPrimitive) is PdfDictionary pdfDictionary8 && pdfDictionary8 is PdfStream template1)
                {
                  PdfTemplate template = new PdfTemplate(template1);
                  if (template != null)
                  {
                    if (template.m_content != null)
                    {
                      PdfDictionary content = (PdfDictionary) template.m_content;
                      if (!content.ContainsKey("Subtype"))
                      {
                        PdfName name = content.GetName("Form");
                        content["Subtype"] = (IPdfPrimitive) name;
                      }
                      if (!content.ContainsKey("Type"))
                      {
                        PdfName name = content.GetName("XObject");
                        content["Type"] = (IPdfPrimitive) name;
                      }
                    }
                    if (item != null)
                      graphics.DrawPdfTemplate(template, item.Location);
                    else
                      graphics.DrawPdfTemplate(template, this.Bounds.Location, this.Bounds.Size);
                  }
                }
              }
              graphics.Restore();
            }
            else
            {
              if (this.AutoResizeText)
                this.SetFittingFontSize(ref graphicsProperties, paintParams, str);
              FieldPainter.DrawTextBox(graphics, paintParams, str, graphicsProperties.Font, graphicsProperties.StringFormat, this.Multiline, this.Scrollable);
            }
          }
          else
          {
            if (graphics.isStandardUnicode && str.Contains("�"))
              graphicsProperties.Font = (PdfFont) new PdfTrueTypeFont(new System.Drawing.Font(this.Font.Name, this.Font.Size), true);
            if (graphicsProperties.Font is PdfStandardFont && graphics.isStandardUnicode && PdfLoadedTextBoxField.IsChineseString(str))
              graphicsProperties.Font = (PdfFont) new PdfCjkStandardFont(PdfCjkFontFamily.HeiseiMinchoW3, graphicsProperties.Font.Size);
            FieldPainter.DrawTextBox(graphics, paintParams, str, graphicsProperties.Font, graphicsProperties.StringFormat, this.Multiline, this.Scrollable);
          }
        }
      }
      graphics.Restore();
    }
  }

  internal bool IsRTLChar(char input)
  {
    bool flag = false;
    int num = (int) input;
    if (num >= 1424 && num <= 1535 /*0x05FF*/)
      flag = true;
    else if (num >= 1536 /*0x0600*/ && num <= 1791 /*0x06FF*/ || num >= 1872 && num <= 1919 || num >= 2208 && num <= 2303 /*0x08FF*/ || num >= 64336 && num <= 65279 || num >= 126464 && num <= 126719)
      flag = true;
    else if (num >= 67648 && num <= 67679)
      flag = true;
    else if (num >= 66464 && num <= 66527)
      flag = true;
    return flag;
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

  private void SetMultiLineFontSize(ref PdfLoadedStyledField.GraphicsProperties gp, string text)
  {
    PdfLoadedStyledField.GraphicsProperties graphicsProperties = gp;
    bool flag = false;
    float size = graphicsProperties.Font.Metrics.Size;
    PdfStringLayouter pdfStringLayouter = new PdfStringLayouter();
    graphicsProperties.Rect.Width -= 2f * graphicsProperties.BorderWidth;
    graphicsProperties.Rect.Height -= 6f * graphicsProperties.BorderWidth;
    if (text.EndsWith(" "))
      graphicsProperties.StringFormat.MeasureTrailingSpaces = true;
    SizeF sizeF = graphicsProperties.Font.MeasureString(text, graphicsProperties.StringFormat);
    float num1 = (float) (int) ((double) graphicsProperties.Rect.Height / (double) sizeF.Height);
    for (float num2 = num1 * graphicsProperties.Rect.Width; (double) num2 <= (double) sizeF.Width; num2 = num1 * graphicsProperties.Rect.Width)
    {
      flag = true;
      size -= 0.2f;
      graphicsProperties.Font.Metrics.Size = size;
      sizeF = graphicsProperties.Font.MeasureString(text, graphicsProperties.StringFormat);
      num1 = (float) (int) ((double) graphicsProperties.Rect.Height / (double) sizeF.Height);
    }
    if (!flag)
      return;
    PdfStringLayoutResult stringLayoutResult = pdfStringLayouter.Layout(text, graphicsProperties.Font, graphicsProperties.StringFormat, graphicsProperties.Rect.Size);
    if ((double) num1 != (double) stringLayoutResult.LineCount && (double) stringLayoutResult.LineCount > (double) num1)
      gp.Font.Metrics.Size = size - 0.4f;
    else
      gp.Font.Metrics.Size = size - 0.2f;
  }

  private bool isRTL(string text) => new Regex("[\\u0600-\\u06ff]\\?[ ]\\?[0-9]\\?").IsMatch(text);

  private bool IsRTLText(ushort[] characterCodes)
  {
    bool flag = false;
    int index = 0;
    for (int length = characterCodes.Length; index < length; ++index)
    {
      if (characterCodes[index] == (ushort) 2 || characterCodes[index] == (ushort) 6)
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  internal override float GetFontHeight(PdfFontFamily family)
  {
    PdfStandardFont pdfStandardFont = new PdfStandardFont(family, 12f);
    float fontHeight;
    if (!this.Multiline)
    {
      float num = (float) (8.0 * ((double) this.Bounds.Size.Width - 4.0 * (double) this.BorderWidth)) / pdfStandardFont.MeasureString(this.Text).Width;
      fontHeight = (double) num > 8.0 ? 8f : num;
    }
    else
      fontHeight = 12.5f;
    return fontHeight;
  }

  private bool IsUnicodeStandardFont()
  {
    if (this.Font.FontInternal is PdfDictionary fontInternal && fontInternal.ContainsKey("Encoding"))
    {
      PdfName pdfName = fontInternal["Encoding"] as PdfName;
      if (pdfName != (PdfName) null && pdfName.Value == "WinAnsiEncoding")
        return false;
    }
    return true;
  }

  private static bool IsChineseString(string text) => Regex.IsMatch(text, "[一-龥]");

  private bool FindAppearanceDrawFromStream(PdfStream apperenceStream)
  {
    bool appearanceDrawFromStream = false;
    apperenceStream.Decompress();
    MemoryStream memoryStream = new MemoryStream();
    apperenceStream.InternalStream.WriteTo((Stream) memoryStream);
    foreach (PdfRecord pdfRecord in new ContentParser(memoryStream.ToArray()).ReadContent())
    {
      string empty = string.Empty;
      string[] operands = pdfRecord.Operands;
      string operatorName = pdfRecord.OperatorName;
      if (operatorName == "Tj" || operatorName == "'" || operatorName == "TJ" || operatorName == "m" || operatorName == "l" || operatorName == "S")
      {
        appearanceDrawFromStream = true;
        break;
      }
    }
    return appearanceDrawFromStream;
  }

  private string FindOperator(int token)
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

  public void RemoveAt(int index)
  {
    if (this.Items == null || this.Items.Count == 0)
      return;
    this.Remove(this.Items[index]);
  }

  public void Remove(PdfLoadedTexBoxItem item)
  {
    int index = item != null ? this.Items.Remove(item) : throw new NullReferenceException(nameof (item));
    if (!(this.Dictionary["Kids"] is PdfArray pdfArray1))
      return;
    PdfDictionary element1 = PdfCrossTable.Dereference(pdfArray1[index]) as PdfDictionary;
    PdfReferenceHolder element2 = pdfArray1[index] as PdfReferenceHolder;
    if (element1 != null)
    {
      if (item.Page != null && element2 != (PdfReferenceHolder) null && PdfCrossTable.Dereference(item.Page.Dictionary["Annots"]) is PdfArray pdfArray2)
      {
        pdfArray2.Remove((IPdfPrimitive) element2);
        pdfArray2.MarkChanged();
      }
      if (this.CrossTable.PdfObjects.Contains((IPdfPrimitive) element1))
        this.CrossTable.PdfObjects.Remove(this.CrossTable.PdfObjects.IndexOf((IPdfPrimitive) element1));
    }
    pdfArray1.RemoveAt(index);
    pdfArray1.MarkChanged();
  }
}
