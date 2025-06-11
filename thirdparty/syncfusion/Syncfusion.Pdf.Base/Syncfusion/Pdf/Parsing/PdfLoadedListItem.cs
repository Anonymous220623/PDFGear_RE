// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfLoadedListItem
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class PdfLoadedListItem
{
  private string m_text;
  private string m_value;
  private PdfLoadedChoiceField m_field;
  private PdfCrossTable m_crossTable;

  public string Text
  {
    get => this.m_text;
    set => this.AssignText(value);
  }

  public string Value
  {
    get => this.m_value ?? this.Text;
    set => this.AssignValue(value);
  }

  internal PdfLoadedListItem(
    string text,
    string value,
    PdfLoadedChoiceField field,
    PdfCrossTable cTable)
    : this(text, value)
  {
    this.m_field = field;
    this.m_crossTable = cTable;
  }

  public PdfLoadedListItem(string text, string value)
  {
    this.m_text = text != null ? text : throw new ArgumentNullException(nameof (text));
    this.m_value = value;
  }

  private void AssignText(string value)
  {
    if (value == null)
      throw new ArgumentNullException("text");
    if (!(this.m_text != value))
      return;
    PdfDictionary dictionary = this.m_field.Dictionary;
    if (!dictionary.ContainsKey("Opt"))
      return;
    PdfArray primitive = this.m_crossTable.GetObject(dictionary["Opt"]) as PdfArray;
    PdfArray element = new PdfArray();
    element.Add((IPdfPrimitive) new PdfString(this.m_value));
    element.Add((IPdfPrimitive) new PdfString(value));
    int index = 0;
    for (int count = primitive.Count; index < count; ++index)
    {
      if ((this.m_crossTable.GetObject((this.m_crossTable.GetObject(primitive[index]) as PdfArray)[1]) as PdfString).Value == this.m_text)
      {
        this.m_text = value;
        primitive.RemoveAt(index);
        primitive.Insert(index, (IPdfPrimitive) element);
      }
    }
    dictionary.SetProperty("Opt", (IPdfPrimitive) primitive);
    this.m_field.Changed = true;
  }

  private void AssignValue(string value)
  {
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    if (!(this.m_value != value))
      return;
    PdfDictionary dictionary = this.m_field.Dictionary;
    if (!dictionary.ContainsKey("Opt"))
      return;
    PdfArray primitive = this.m_crossTable.GetObject(dictionary["Opt"]) as PdfArray;
    PdfArray element = new PdfArray();
    element.Add((IPdfPrimitive) new PdfString(value));
    element.Add((IPdfPrimitive) new PdfString(this.m_text));
    int index = 0;
    for (int count = primitive.Count; index < count; ++index)
    {
      if ((this.m_crossTable.GetObject((this.m_crossTable.GetObject(primitive[index]) as PdfArray)[1]) as PdfString).Value == this.m_value)
      {
        this.m_value = value;
        primitive.RemoveAt(index);
        primitive.Insert(index, (IPdfPrimitive) element);
      }
    }
    dictionary.SetProperty("Opt", (IPdfPrimitive) primitive);
    this.m_field.Changed = true;
  }
}
