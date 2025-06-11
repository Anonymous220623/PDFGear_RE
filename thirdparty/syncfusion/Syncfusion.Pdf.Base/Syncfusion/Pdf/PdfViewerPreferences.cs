// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfViewerPreferences
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfViewerPreferences : IPdfWrapper
{
  private bool m_centerWindow;
  private bool m_displayDocTitle;
  private bool m_fitWindow;
  private bool m_hideMenubar;
  private bool m_hideToolbar;
  private bool m_hideWindowUI;
  private PdfPageMode m_pageMode;
  private PdfPageLayout m_pageLayout;
  private PdfCatalog m_catalog;
  private PdfDictionary m_dictionary = new PdfDictionary();
  private PageScalingMode m_pageScaling;
  private DuplexMode m_duplex = DuplexMode.None;

  internal PdfViewerPreferences()
  {
  }

  internal PdfViewerPreferences(PdfCatalog catalog)
  {
    this.m_catalog = catalog != null ? catalog : throw new ArgumentNullException(nameof (catalog));
  }

  public bool CenterWindow
  {
    get
    {
      if (this.m_catalog.LoadedDocument != null)
      {
        this.m_dictionary = (PdfDictionary) this.m_catalog;
        if ((object) (this.m_dictionary["ViewerPreferences"] as PdfReferenceHolder) != null)
        {
          PdfDictionary pdfDictionary = (this.m_dictionary["ViewerPreferences"] as PdfReferenceHolder).Object as PdfDictionary;
          if (pdfDictionary.ContainsKey(nameof (CenterWindow)))
            this.m_centerWindow = bool.Parse((pdfDictionary[nameof (CenterWindow)] as PdfBoolean).Value.ToString());
        }
        else if (this.m_dictionary["ViewerPreferences"] is PdfDictionary)
        {
          PdfDictionary pdfDictionary = this.m_dictionary["ViewerPreferences"] as PdfDictionary;
          if (pdfDictionary.ContainsKey(nameof (CenterWindow)))
            this.m_centerWindow = bool.Parse((pdfDictionary[nameof (CenterWindow)] as PdfBoolean).Value.ToString());
        }
      }
      return this.m_centerWindow;
    }
    set
    {
      this.m_centerWindow = value;
      this.m_dictionary = (PdfDictionary) this.m_catalog;
      if ((object) (this.m_dictionary["ViewerPreferences"] as PdfReferenceHolder) != null)
      {
        ((this.m_dictionary["ViewerPreferences"] as PdfReferenceHolder).Object as PdfDictionary).SetBoolean(nameof (CenterWindow), this.m_centerWindow);
      }
      else
      {
        if (!(this.m_dictionary["ViewerPreferences"] is PdfDictionary))
          return;
        (this.m_dictionary["ViewerPreferences"] as PdfDictionary).SetBoolean(nameof (CenterWindow), this.m_centerWindow);
      }
    }
  }

  public bool DisplayTitle
  {
    get
    {
      if (this.m_catalog.LoadedDocument != null)
      {
        this.m_dictionary = (PdfDictionary) this.m_catalog;
        if ((object) (this.m_dictionary["ViewerPreferences"] as PdfReferenceHolder) != null)
        {
          PdfDictionary pdfDictionary = (this.m_dictionary["ViewerPreferences"] as PdfReferenceHolder).Object as PdfDictionary;
          if (pdfDictionary.ContainsKey("DisplayDocTitle"))
            this.m_displayDocTitle = bool.Parse((pdfDictionary["DisplayDocTitle"] as PdfBoolean).Value.ToString());
        }
        else if (this.m_dictionary["ViewerPreferences"] is PdfDictionary)
        {
          PdfDictionary pdfDictionary = this.m_dictionary["ViewerPreferences"] as PdfDictionary;
          if (pdfDictionary.ContainsKey("DisplayDocTitle"))
            this.m_displayDocTitle = bool.Parse((pdfDictionary["DisplayDocTitle"] as PdfBoolean).Value.ToString());
        }
      }
      return this.m_displayDocTitle;
    }
    set
    {
      this.m_displayDocTitle = value;
      this.m_dictionary = (PdfDictionary) this.m_catalog;
      if ((object) (this.m_dictionary["ViewerPreferences"] as PdfReferenceHolder) != null)
      {
        ((this.m_dictionary["ViewerPreferences"] as PdfReferenceHolder).Object as PdfDictionary).SetBoolean("DisplayDocTitle", this.m_displayDocTitle);
      }
      else
      {
        if (!(this.m_dictionary["ViewerPreferences"] is PdfDictionary))
          return;
        (this.m_dictionary["ViewerPreferences"] as PdfDictionary).SetBoolean("DisplayDocTitle", this.m_displayDocTitle);
      }
    }
  }

  public bool FitWindow
  {
    get
    {
      if (this.m_catalog.LoadedDocument != null)
      {
        this.m_dictionary = (PdfDictionary) this.m_catalog;
        if ((object) (this.m_dictionary["ViewerPreferences"] as PdfReferenceHolder) != null)
        {
          PdfDictionary pdfDictionary = (this.m_dictionary["ViewerPreferences"] as PdfReferenceHolder).Object as PdfDictionary;
          if (pdfDictionary.ContainsKey(nameof (FitWindow)))
            this.m_fitWindow = bool.Parse((pdfDictionary[nameof (FitWindow)] as PdfBoolean).Value.ToString());
        }
        else if (this.m_dictionary["ViewerPreferences"] is PdfDictionary)
        {
          PdfDictionary pdfDictionary = this.m_dictionary["ViewerPreferences"] as PdfDictionary;
          if (pdfDictionary.ContainsKey(nameof (FitWindow)))
            this.m_fitWindow = bool.Parse((pdfDictionary[nameof (FitWindow)] as PdfBoolean).Value.ToString());
        }
      }
      return this.m_fitWindow;
    }
    set
    {
      this.m_fitWindow = value;
      this.m_dictionary = (PdfDictionary) this.m_catalog;
      if ((object) (this.m_dictionary["ViewerPreferences"] as PdfReferenceHolder) != null)
      {
        ((this.m_dictionary["ViewerPreferences"] as PdfReferenceHolder).Object as PdfDictionary).SetBoolean(nameof (FitWindow), this.m_fitWindow);
      }
      else
      {
        if (!(this.m_dictionary["ViewerPreferences"] is PdfDictionary))
          return;
        (this.m_dictionary["ViewerPreferences"] as PdfDictionary).SetBoolean(nameof (FitWindow), this.m_fitWindow);
      }
    }
  }

  public bool HideMenubar
  {
    get
    {
      if (this.m_catalog.LoadedDocument != null)
      {
        this.m_dictionary = (PdfDictionary) this.m_catalog;
        if ((object) (this.m_dictionary["ViewerPreferences"] as PdfReferenceHolder) != null)
        {
          PdfDictionary pdfDictionary = (this.m_dictionary["ViewerPreferences"] as PdfReferenceHolder).Object as PdfDictionary;
          if (pdfDictionary.ContainsKey(nameof (HideMenubar)))
            this.m_hideMenubar = bool.Parse((pdfDictionary[nameof (HideMenubar)] as PdfBoolean).Value.ToString());
        }
        else if (this.m_dictionary["ViewerPreferences"] is PdfDictionary)
        {
          PdfDictionary pdfDictionary = this.m_dictionary["ViewerPreferences"] as PdfDictionary;
          if (pdfDictionary.ContainsKey(nameof (HideMenubar)))
            this.m_hideMenubar = bool.Parse((pdfDictionary[nameof (HideMenubar)] as PdfBoolean).Value.ToString());
        }
      }
      return this.m_hideMenubar;
    }
    set
    {
      this.m_hideMenubar = value;
      this.m_dictionary = (PdfDictionary) this.m_catalog;
      if ((object) (this.m_dictionary["ViewerPreferences"] as PdfReferenceHolder) != null)
      {
        ((this.m_dictionary["ViewerPreferences"] as PdfReferenceHolder).Object as PdfDictionary).SetBoolean(nameof (HideMenubar), this.m_hideMenubar);
      }
      else
      {
        if (!(this.m_dictionary["ViewerPreferences"] is PdfDictionary))
          return;
        (this.m_dictionary["ViewerPreferences"] as PdfDictionary).SetBoolean(nameof (HideMenubar), this.m_hideMenubar);
      }
    }
  }

  public bool HideToolbar
  {
    get
    {
      if (this.m_catalog.LoadedDocument != null)
      {
        this.m_dictionary = (PdfDictionary) this.m_catalog;
        if ((object) (this.m_dictionary["ViewerPreferences"] as PdfReferenceHolder) != null)
        {
          PdfDictionary pdfDictionary = (this.m_dictionary["ViewerPreferences"] as PdfReferenceHolder).Object as PdfDictionary;
          if (pdfDictionary.ContainsKey(nameof (HideToolbar)))
            this.m_hideToolbar = bool.Parse((pdfDictionary[nameof (HideToolbar)] as PdfBoolean).Value.ToString());
        }
        else if (this.m_dictionary["ViewerPreferences"] is PdfDictionary)
        {
          PdfDictionary pdfDictionary = this.m_dictionary["ViewerPreferences"] as PdfDictionary;
          if (pdfDictionary.ContainsKey(nameof (HideToolbar)))
            this.m_hideToolbar = bool.Parse((pdfDictionary[nameof (HideToolbar)] as PdfBoolean).Value.ToString());
        }
      }
      return this.m_hideToolbar;
    }
    set
    {
      this.m_hideToolbar = value;
      this.m_dictionary = (PdfDictionary) this.m_catalog;
      if ((object) (this.m_dictionary["ViewerPreferences"] as PdfReferenceHolder) != null)
      {
        ((this.m_dictionary["ViewerPreferences"] as PdfReferenceHolder).Object as PdfDictionary).SetBoolean(nameof (HideToolbar), this.m_hideToolbar);
      }
      else
      {
        if (!(this.m_dictionary["ViewerPreferences"] is PdfDictionary))
          return;
        (this.m_dictionary["ViewerPreferences"] as PdfDictionary).SetBoolean(nameof (HideToolbar), this.m_hideToolbar);
      }
    }
  }

  public bool HideWindowUI
  {
    get
    {
      if (this.m_catalog.LoadedDocument != null)
      {
        this.m_dictionary = (PdfDictionary) this.m_catalog;
        if ((object) (this.m_dictionary["ViewerPreferences"] as PdfReferenceHolder) != null)
        {
          PdfDictionary pdfDictionary = (this.m_dictionary["ViewerPreferences"] as PdfReferenceHolder).Object as PdfDictionary;
          if (pdfDictionary.ContainsKey(nameof (HideWindowUI)))
            this.m_hideWindowUI = bool.Parse((pdfDictionary[nameof (HideWindowUI)] as PdfBoolean).Value.ToString());
        }
        else if (this.m_dictionary["ViewerPreferences"] is PdfDictionary)
        {
          PdfDictionary pdfDictionary = this.m_dictionary["ViewerPreferences"] as PdfDictionary;
          if (pdfDictionary.ContainsKey(nameof (HideWindowUI)))
            this.m_hideWindowUI = bool.Parse((pdfDictionary[nameof (HideWindowUI)] as PdfBoolean).Value.ToString());
        }
      }
      return this.m_hideWindowUI;
    }
    set
    {
      this.m_hideWindowUI = value;
      this.m_dictionary = (PdfDictionary) this.m_catalog;
      if ((object) (this.m_dictionary["ViewerPreferences"] as PdfReferenceHolder) != null)
      {
        ((this.m_dictionary["ViewerPreferences"] as PdfReferenceHolder).Object as PdfDictionary).SetBoolean(nameof (HideWindowUI), this.m_hideWindowUI);
      }
      else
      {
        if (!(this.m_dictionary["ViewerPreferences"] is PdfDictionary))
          return;
        (this.m_dictionary["ViewerPreferences"] as PdfDictionary).SetBoolean(nameof (HideWindowUI), this.m_hideWindowUI);
      }
    }
  }

  public PdfPageMode PageMode
  {
    get
    {
      if (this.m_catalog.LoadedDocument != null)
      {
        this.m_dictionary = (PdfDictionary) this.m_catalog;
        if (this.m_dictionary[nameof (PageMode)] != null)
          this.m_pageMode = (PdfPageMode) Enum.Parse(typeof (PdfPageMode), (this.m_dictionary[nameof (PageMode)] as PdfName).Value, true);
      }
      return this.m_pageMode;
    }
    set
    {
      this.m_pageMode = value;
      PdfDictionary.SetName((PdfDictionary) this.m_catalog, nameof (PageMode), this.m_pageMode.ToString());
    }
  }

  public PdfPageLayout PageLayout
  {
    get
    {
      if (this.m_catalog.LoadedDocument != null)
      {
        this.m_dictionary = (PdfDictionary) this.m_catalog;
        if (this.m_dictionary.ContainsKey(nameof (PageLayout)))
          this.m_pageLayout = !Enum.IsDefined(typeof (PdfPageLayout), (object) (this.m_dictionary[nameof (PageLayout)] as PdfName).Value.ToString()) ? PdfPageLayout.SinglePage : (PdfPageLayout) Enum.Parse(typeof (PdfPageLayout), (this.m_dictionary[nameof (PageLayout)] as PdfName).Value.ToString(), true);
      }
      return this.m_pageLayout;
    }
    set
    {
      this.m_pageLayout = value;
      PdfDictionary.SetName((PdfDictionary) this.m_catalog, nameof (PageLayout), this.m_pageLayout.ToString());
    }
  }

  public DuplexMode Duplex
  {
    get
    {
      if (this.m_catalog.LoadedDocument != null)
      {
        this.m_dictionary = (PdfDictionary) this.m_catalog;
        if (this.m_dictionary.ContainsKey(nameof (Duplex)))
          this.m_duplex = !Enum.IsDefined(typeof (DuplexMode), (object) (this.m_dictionary[nameof (Duplex)] as PdfName).Value.ToString()) ? DuplexMode.None : (DuplexMode) Enum.Parse(typeof (DuplexMode), (this.m_dictionary[nameof (Duplex)] as PdfName).Value.ToString(), true);
      }
      return this.m_duplex;
    }
    set
    {
      this.m_duplex = value;
      PdfDictionary.SetName((PdfDictionary) this.m_catalog, nameof (Duplex), this.m_duplex.ToString());
    }
  }

  public PageScalingMode PageScaling
  {
    get
    {
      if (this.m_catalog.LoadedDocument != null)
      {
        this.m_dictionary = (PdfDictionary) this.m_catalog;
        if ((object) (this.m_dictionary["ViewerPreferences"] as PdfReferenceHolder) != null)
        {
          PdfDictionary pdfDictionary = (this.m_dictionary["ViewerPreferences"] as PdfReferenceHolder).Object as PdfDictionary;
          if (pdfDictionary.ContainsKey("PrintScaling"))
            this.m_pageScaling = (PageScalingMode) Enum.Parse(typeof (PageScalingMode), (pdfDictionary["PrintScaling"] as PdfName).Value.ToString(), true);
        }
        else if (this.m_dictionary["ViewerPreferences"] is PdfDictionary)
        {
          PdfDictionary pdfDictionary = this.m_dictionary["ViewerPreferences"] as PdfDictionary;
          if (pdfDictionary.ContainsKey("PrintScaling"))
            this.m_pageScaling = (PageScalingMode) Enum.Parse(typeof (PageScalingMode), (pdfDictionary["PrintScaling"] as PdfName).Value.ToString(), true);
        }
      }
      return this.m_pageScaling;
    }
    set
    {
      this.m_pageScaling = value;
      this.m_dictionary = (PdfDictionary) this.m_catalog;
      if ((object) (this.m_dictionary["ViewerPreferences"] as PdfReferenceHolder) != null)
      {
        PdfDictionary pdfDictionary = (this.m_dictionary["ViewerPreferences"] as PdfReferenceHolder).Object as PdfDictionary;
        if (this.m_pageScaling != PageScalingMode.AppDefault)
        {
          pdfDictionary.SetName("PrintScaling", this.m_pageScaling.ToString());
        }
        else
        {
          if (!pdfDictionary.ContainsKey("PrintScaling"))
            return;
          pdfDictionary.Remove("PrintScaling");
        }
      }
      else
      {
        if (!(this.m_dictionary["ViewerPreferences"] is PdfDictionary))
          return;
        PdfDictionary pdfDictionary = this.m_dictionary["ViewerPreferences"] as PdfDictionary;
        if (this.m_pageScaling != PageScalingMode.AppDefault)
        {
          pdfDictionary.SetName("PrintScaling", this.m_pageScaling.ToString());
        }
        else
        {
          if (!pdfDictionary.ContainsKey("PrintScaling"))
            return;
          pdfDictionary.Remove("PrintScaling");
        }
      }
    }
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
