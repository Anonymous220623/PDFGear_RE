// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfRadioButtonListField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfRadioButtonListField : PdfField
{
  private PdfRadioButtonItemCollection m_items;
  private int m_selectedIndex = -1;

  public PdfRadioButtonListField(PdfPageBase page, string name)
    : base(page, name)
  {
    this.Flags |= FieldFlags.Radio;
    this.Dictionary.SetProperty("FT", (IPdfPrimitive) new PdfName("Btn"));
  }

  public int SelectedIndex
  {
    get => this.m_selectedIndex;
    set
    {
      if (value < 0 || value >= this.Items.Count)
        throw new ArgumentOutOfRangeException(nameof (SelectedIndex));
      if (this.m_selectedIndex == value)
        return;
      this.m_selectedIndex = value;
      PdfRadioButtonListItem radioButtonListItem = this.m_items[this.m_selectedIndex];
      this.Dictionary.SetName("V", radioButtonListItem.Value);
      this.Dictionary.SetName("DV", radioButtonListItem.Value);
    }
  }

  public string SelectedValue
  {
    get => this.m_items[this.m_selectedIndex].Value;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (SelectedValue));
      PdfRadioButtonListItem radioButtonListItem = this.m_items[this.m_selectedIndex];
      if (!(radioButtonListItem.Value != value))
        return;
      radioButtonListItem.Value = value;
    }
  }

  public PdfRadioButtonListItem SelectedItem
  {
    get
    {
      PdfRadioButtonListItem selectedItem = (PdfRadioButtonListItem) null;
      if (this.m_selectedIndex != -1)
        selectedItem = this.m_items[this.m_selectedIndex];
      return selectedItem;
    }
  }

  public PdfRadioButtonItemCollection Items
  {
    get
    {
      if (this.m_items == null)
      {
        this.m_items = new PdfRadioButtonItemCollection(this);
        this.Dictionary.SetProperty("Kids", (IPdfWrapper) this.m_items);
      }
      return this.m_items;
    }
  }

  internal override void Draw()
  {
    int index = 0;
    for (int count = this.Items.Count; index < count; ++index)
      this.Items[index].Draw();
  }
}
