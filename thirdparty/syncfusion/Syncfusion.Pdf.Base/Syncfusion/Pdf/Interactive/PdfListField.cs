// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfListField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public abstract class PdfListField : PdfAppearanceField
{
  private PdfListFieldItemCollection m_items;
  private int m_selectedIndex = -1;
  private int[] m_selectedIndexes = new int[0];

  public PdfListField(PdfPageBase page, string name)
    : base(page, name)
  {
  }

  internal PdfListField()
  {
  }

  public PdfListFieldItemCollection Items
  {
    get
    {
      if (this.m_items == null)
      {
        this.m_items = new PdfListFieldItemCollection();
        this.Dictionary.SetProperty("Opt", (IPdfWrapper) this.m_items);
      }
      return this.m_items;
    }
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
      this.Dictionary.SetProperty("I", (IPdfPrimitive) new PdfArray(new int[1]
      {
        this.m_selectedIndex
      }));
    }
  }

  public string SelectedValue
  {
    get => this.m_items[this.m_selectedIndex].Value;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (SelectedValue));
      for (int index = 0; index < this.m_items.Count; ++index)
      {
        if (value == this.m_items[index].Text)
        {
          this.m_selectedIndex = index;
          break;
        }
      }
      PdfListFieldItem pdfListFieldItem = this.m_items[this.m_selectedIndex];
      if (pdfListFieldItem.Value != value)
        pdfListFieldItem.Value = value;
      this.Dictionary.SetProperty("I", (IPdfPrimitive) new PdfArray(new int[1]
      {
        this.m_selectedIndex
      }));
    }
  }

  public PdfListFieldItem SelectedItem => this.m_items[this.m_selectedIndex];

  internal int[] SelectedIndexes
  {
    get => this.m_selectedIndexes;
    set
    {
      foreach (int num in value)
      {
        if (num < 0 || num >= this.Items.Count)
          throw new ArgumentOutOfRangeException("SelectedIndex");
      }
      this.m_selectedIndexes = value;
      if (this.m_selectedIndexes.Length <= 1)
        return;
      this.Dictionary.SetProperty("I", (IPdfPrimitive) new PdfArray(this.m_selectedIndexes));
    }
  }

  internal override void Draw() => base.Draw();

  protected override void Initialize()
  {
    base.Initialize();
    this.Dictionary.SetProperty("FT", (IPdfPrimitive) new PdfName("Ch"));
  }
}
