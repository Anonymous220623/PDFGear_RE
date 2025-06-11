// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfLoadedFieldItem
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class PdfLoadedFieldItem
{
  private PdfLoadedStyledField m_field;
  private int m_collectionIndex;
  private PdfDictionary m_dictionary;
  private PdfPageBase m_page;

  protected PdfLoadedStyledField Field => this.m_field;

  internal PdfLoadedStyledField Parent => this.m_field;

  internal PdfCrossTable CrossTable => this.Parent.CrossTable;

  internal PdfDictionary Dictionary => this.m_dictionary;

  public RectangleF Bounds
  {
    get
    {
      int defaultIndex = this.m_field.DefaultIndex;
      this.m_field.DefaultIndex = this.m_collectionIndex;
      RectangleF bounds = this.m_field.Bounds;
      this.m_field.DefaultIndex = defaultIndex;
      return bounds;
    }
    set
    {
      int defaultIndex = this.m_field.DefaultIndex;
      this.m_field.DefaultIndex = this.m_collectionIndex;
      this.m_field.Bounds = value;
      this.m_field.DefaultIndex = defaultIndex;
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

  internal PdfPen BorderPen
  {
    get
    {
      int defaultIndex = this.m_field.DefaultIndex;
      this.m_field.DefaultIndex = this.m_collectionIndex;
      PdfPen borderPen = this.m_field.BorderPen;
      this.m_field.DefaultIndex = defaultIndex;
      return borderPen;
    }
  }

  internal PdfBorderStyle BorderStyle
  {
    get
    {
      int defaultIndex = this.m_field.DefaultIndex;
      this.m_field.DefaultIndex = this.m_collectionIndex;
      PdfBorderStyle borderStyle = this.m_field.BorderStyle;
      this.m_field.DefaultIndex = defaultIndex;
      return borderStyle;
    }
  }

  internal float[] DashPatern
  {
    get
    {
      int defaultIndex = this.m_field.DefaultIndex;
      this.m_field.DefaultIndex = this.m_collectionIndex;
      float[] dashPatern = this.m_field.DashPatern;
      this.m_field.DefaultIndex = defaultIndex;
      return dashPatern;
    }
  }

  internal float BorderWidth
  {
    get
    {
      int defaultIndex = this.m_field.DefaultIndex;
      this.m_field.DefaultIndex = this.m_collectionIndex;
      float borderWidth = this.m_field.BorderWidth;
      this.m_field.DefaultIndex = defaultIndex;
      return borderWidth;
    }
  }

  internal PdfStringFormat StringFormat
  {
    get
    {
      int defaultIndex = this.m_field.DefaultIndex;
      this.m_field.DefaultIndex = this.m_collectionIndex;
      PdfStringFormat stringFormat = this.m_field.StringFormat;
      this.m_field.DefaultIndex = defaultIndex;
      return stringFormat;
    }
  }

  internal PdfBrush BackBrush
  {
    get
    {
      int defaultIndex = this.m_field.DefaultIndex;
      this.m_field.DefaultIndex = this.m_collectionIndex;
      PdfBrush backBrush = this.m_field.BackBrush;
      this.m_field.DefaultIndex = defaultIndex;
      return backBrush;
    }
  }

  internal PdfBrush ForeBrush
  {
    get
    {
      int defaultIndex = this.m_field.DefaultIndex;
      this.m_field.DefaultIndex = this.m_collectionIndex;
      PdfBrush foreBrush = this.m_field.ForeBrush;
      this.m_field.DefaultIndex = defaultIndex;
      return foreBrush;
    }
  }

  internal PdfBrush ShadowBrush
  {
    get
    {
      int defaultIndex = this.m_field.DefaultIndex;
      this.m_field.DefaultIndex = this.m_collectionIndex;
      PdfBrush shadowBrush = this.m_field.ShadowBrush;
      this.m_field.DefaultIndex = defaultIndex;
      return shadowBrush;
    }
  }

  internal PdfFont Font
  {
    get
    {
      int defaultIndex = this.m_field.DefaultIndex;
      this.m_field.DefaultIndex = this.m_collectionIndex;
      PdfFont font = this.m_field.Font;
      this.m_field.DefaultIndex = defaultIndex;
      return font;
    }
  }

  public PdfPageBase Page
  {
    get
    {
      if (this.m_page == null)
      {
        int defaultIndex = this.m_field.DefaultIndex;
        this.m_field.DefaultIndex = this.m_collectionIndex;
        this.m_page = this.m_field.Page;
        PdfName key = new PdfName("P");
        if (this.m_field.Kids != null && this.m_field.Kids.Count > 0 && this.CrossTable.Document is PdfLoadedDocument document)
        {
          if (this.m_dictionary.ContainsKey(key))
          {
            if (this.CrossTable.GetObject(this.m_dictionary["P"]) is PdfDictionary dic)
            {
              PdfReference reference = this.CrossTable.GetReference((IPdfPrimitive) this.m_dictionary);
              foreach (PdfPageBase page in document.Pages)
              {
                PdfArray annotations = page.ObtainAnnotations();
                if (annotations != null)
                {
                  for (int index = 0; index < annotations.Count; ++index)
                  {
                    PdfReferenceHolder pdfReferenceHolder = annotations[index] as PdfReferenceHolder;
                    if (pdfReferenceHolder != (PdfReferenceHolder) null && pdfReferenceHolder.Reference == reference)
                    {
                      this.m_page = document.Pages.GetPage(dic);
                      this.m_field.DefaultIndex = defaultIndex;
                      return this.m_page;
                    }
                  }
                }
              }
              this.m_field.DefaultIndex = defaultIndex;
              this.m_page = (PdfPageBase) null;
            }
          }
          else
          {
            PdfReference reference = this.CrossTable.GetReference((IPdfPrimitive) this.m_dictionary);
            foreach (PdfLoadedPage page in document.Pages)
            {
              PdfArray annotations = page.ObtainAnnotations();
              if (annotations != null)
              {
                for (int index = 0; index < annotations.Count; ++index)
                {
                  if ((annotations[index] as PdfReferenceHolder).Reference == reference)
                  {
                    this.m_page = (PdfPageBase) page;
                    return this.m_page;
                  }
                }
              }
            }
            this.m_page = (PdfPageBase) null;
          }
        }
        this.m_field.DefaultIndex = defaultIndex;
      }
      return this.m_page;
    }
    internal set => this.m_page = value;
  }

  internal PdfLoadedFieldItem(PdfLoadedStyledField field, int index, PdfDictionary dictionary)
  {
    this.m_field = field;
    this.m_collectionIndex = index;
    this.m_dictionary = dictionary;
  }
}
