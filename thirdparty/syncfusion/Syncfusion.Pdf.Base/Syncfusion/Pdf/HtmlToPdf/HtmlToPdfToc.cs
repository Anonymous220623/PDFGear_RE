// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.HtmlToPdf.HtmlToPdfToc
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.HtmlToPdf;

public class HtmlToPdfToc
{
  private const int beginRect = 15;
  private const int beginTitle = 12;
  private const int leftRectPadding = 15;
  private const int rightRectPadding = 15;
  private const int rectLineSpacing = 3;
  private const int maxHeaderLevel = 6;
  private string m_title;
  private PdfTextAlignment m_titleAlignment;
  private HtmlToPdfTocStyle m_titleStyle = new HtmlToPdfTocStyle();
  private HtmlToPdfTocStyle m_headerStyle = new HtmlToPdfTocStyle();
  private HtmlToPdfToc.TabLeaderStyle m_tabLeader;
  private int m_tocPageCount;
  private bool m_isBlinkRenderingEngine;
  private char m_dotStyle;
  private char m_tabLeaderChar;
  private int m_maximumHeaderLevel;
  private int m_startingPageNumber;
  private bool isNextPage;
  private bool isFirstPage = true;
  private bool isTabLeaderChar;
  private PdfLayoutResult m_pageLayoutResult;
  private float m_pageLayoutBottom;
  private PdfTemplate m_template;
  private PdfPage m_pageTemplate;
  private List<HtmlToPdfTocStyle> m_headerStyleCollection = new List<HtmlToPdfTocStyle>();
  private float m_headerHeight;
  private float m_footerHeight;

  public string Title
  {
    get => this.m_title;
    set => this.m_title = value;
  }

  public PdfTextAlignment TitleAlignment
  {
    get => this.m_titleAlignment;
    set => this.m_titleAlignment = value;
  }

  public HtmlToPdfTocStyle TitleStyle
  {
    get => this.m_titleStyle;
    set => this.m_titleStyle = value;
  }

  public HtmlToPdfToc.TabLeaderStyle TabLeader
  {
    get => this.m_tabLeader;
    set => this.m_tabLeader = value;
  }

  public char TabLeaderChar
  {
    get => this.m_tabLeaderChar;
    set => this.m_tabLeaderChar = value;
  }

  public int MaximumHeaderLevel
  {
    get => this.m_maximumHeaderLevel;
    set => this.m_maximumHeaderLevel = value;
  }

  public int StartingPageNumber
  {
    get => this.m_startingPageNumber;
    set => this.m_startingPageNumber = value;
  }

  internal HtmlToPdfTocStyle HeaderStyle
  {
    get => this.m_headerStyle;
    set => this.m_headerStyle = value;
  }

  internal int TocPageCount
  {
    get => this.m_tocPageCount;
    set => this.m_tocPageCount = value;
  }

  internal bool IsBlinkRenderingEngine
  {
    get => this.m_isBlinkRenderingEngine;
    set => this.m_isBlinkRenderingEngine = value;
  }

  internal float HeaderHeight
  {
    get => this.m_headerHeight;
    set => this.m_headerHeight = value;
  }

  internal float FooterHeight
  {
    get => this.m_footerHeight;
    set => this.m_footerHeight = value;
  }

  public HtmlToPdfToc()
  {
    this.TabLeader = HtmlToPdfToc.TabLeaderStyle.Dot;
    this.MaximumHeaderLevel = 6;
    this.IsBlinkRenderingEngine = false;
    if (PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_A1B || PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_A1A)
      return;
    this.Title = "Table of Contents";
    this.TitleStyle.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.TimesRoman, 16f, PdfFontStyle.Regular);
    this.TitleStyle.ForeColor = PdfBrushes.DarkBlue;
    this.TitleAlignment = PdfTextAlignment.Left;
    this.m_headerStyleCollection.Add(new HtmlToPdfTocStyle()
    {
      Font = (PdfFont) new PdfStandardFont(PdfFontFamily.TimesRoman, 11.5f, PdfFontStyle.Regular),
      ForeColor = PdfBrushes.Black,
      Padding = new PdfPaddings(0.0f, 0.0f, 2f, 2f)
    });
    this.m_headerStyleCollection.Add(new HtmlToPdfTocStyle()
    {
      Font = (PdfFont) new PdfStandardFont(PdfFontFamily.TimesRoman, 9.5f, PdfFontStyle.Regular),
      ForeColor = PdfBrushes.Black,
      Padding = new PdfPaddings(0.0f, 0.0f, 2f, 2f)
    });
    this.m_headerStyleCollection.Add(new HtmlToPdfTocStyle()
    {
      Font = (PdfFont) new PdfStandardFont(PdfFontFamily.TimesRoman, 8f, PdfFontStyle.Regular),
      ForeColor = PdfBrushes.Black,
      Padding = new PdfPaddings(0.0f, 0.0f, 2f, 2f)
    });
    this.m_headerStyleCollection.Add(new HtmlToPdfTocStyle()
    {
      Font = (PdfFont) new PdfStandardFont(PdfFontFamily.TimesRoman, 8f, PdfFontStyle.Italic),
      ForeColor = PdfBrushes.Black,
      Padding = new PdfPaddings(0.0f, 0.0f, 2f, 2f)
    });
    this.m_headerStyleCollection.Add(new HtmlToPdfTocStyle()
    {
      Font = (PdfFont) new PdfStandardFont(PdfFontFamily.TimesRoman, 8f, PdfFontStyle.Regular),
      ForeColor = PdfBrushes.Black,
      Padding = new PdfPaddings(0.0f, 0.0f, 2f, 2f)
    });
    this.m_headerStyleCollection.Add(new HtmlToPdfTocStyle()
    {
      Font = (PdfFont) new PdfStandardFont(PdfFontFamily.TimesRoman, 8f, PdfFontStyle.Regular),
      ForeColor = PdfBrushes.Black,
      Padding = new PdfPaddings(0.0f, 0.0f, 2f, 2f)
    });
  }

  internal int GetRectangleHeightAndTocPageCount(
    PdfPageBase page,
    List<HtmlInternalLink> internalLinkCollection)
  {
    int num1 = 0;
    SizeF sizeF = this.TitleStyle.Font.MeasureString(this.Title);
    float num2 = 0.0f;
    float num3 = (float) (12.0 + (double) sizeF.Height + 15.0) + this.HeaderHeight;
    float num4 = page.Graphics.ClientSize.Width - 15f;
    int num5 = 0;
    int num6 = 0;
    bool flag = true;
    int index1 = 0;
    HtmlInternalLink internalLink1 = internalLinkCollection[index1];
    for (int index2 = 0; index2 < internalLinkCollection.Count; ++index2)
    {
      HtmlInternalLink internalLink2 = internalLinkCollection[index2];
      if (internalLink2.HeaderTagLevel != null)
      {
        int currentHeaderLevel = int.Parse(internalLink2.HeaderTagLevel.Split('H')[1]);
        if (currentHeaderLevel <= this.MaximumHeaderLevel)
        {
          if (flag)
          {
            num6 = currentHeaderLevel;
            flag = false;
          }
          if (currentHeaderLevel <= num6)
            num2 = 15f;
          else if (currentHeaderLevel > num5)
            num2 += 15f;
          else if (currentHeaderLevel < num5)
          {
            int num7 = index2 - 1;
            int num8 = 0;
            int num9 = 0;
            while (true)
            {
              int num10;
              do
              {
                ++num9;
                num10 = int.Parse(internalLinkCollection[num7 - num9].HeaderTagLevel.Split('H')[1]);
                if (num10 <= this.MaximumHeaderLevel)
                {
                  if (currentHeaderLevel <= num6)
                  {
                    num2 = 15f;
                    goto label_19;
                  }
                  if (currentHeaderLevel > num10)
                  {
                    num2 = internalLinkCollection[num7 - (num9 - num8 - 1)].TocXcoordinate;
                    goto label_19;
                  }
                }
                else
                  goto label_18;
              }
              while (currentHeaderLevel != num10);
              break;
label_18:
              num8 += num8;
            }
            num2 = internalLinkCollection[num7 - num9].TocXcoordinate;
label_19:
            index2 = num7 + 1;
          }
          HtmlInternalLink internalLink3 = internalLinkCollection[index2];
          internalLink3.TocXcoordinate = num2;
          num5 = currentHeaderLevel;
          float rectangleHeight = this.GetRectangleHeight(internalLink3, num4 - num2, currentHeaderLevel);
          internalLink3.TocRectHeight = rectangleHeight;
          float num11 = num3 + rectangleHeight;
          if ((double) num11 > (double) page.Graphics.ClientSize.Height - (double) this.FooterHeight)
          {
            ++num1;
            num3 = rectangleHeight + 3f;
          }
          else
            num3 = num11 + 3f;
        }
      }
    }
    return num1 + 1;
  }

  private float GetRectangleHeight(
    HtmlInternalLink htmlToc,
    float rectWidth,
    int currentHeaderLevel)
  {
    this.HeaderStyle = this.m_headerStyleCollection[currentHeaderLevel - 1];
    SizeF sizeF = this.HeaderStyle.Font.MeasureString(htmlToc.HeaderContent);
    float rectangleHeight;
    if ((double) sizeF.Width < (double) rectWidth - ((double) this.HeaderStyle.Padding.Left + (double) this.HeaderStyle.Padding.Right))
    {
      rectangleHeight = sizeF.Height + this.HeaderStyle.Padding.Top + this.HeaderStyle.Padding.Bottom;
    }
    else
    {
      int linesFilled = 0;
      this.HeaderStyle.Font.MeasureString(htmlToc.HeaderContent, rectWidth - (this.HeaderStyle.Padding.Left + this.HeaderStyle.Padding.Right), new PdfStringFormat()
      {
        Alignment = PdfTextAlignment.Left
      }, out int _, out linesFilled);
      rectangleHeight = sizeF.Height * (float) linesFilled + this.HeaderStyle.Padding.Top + this.HeaderStyle.Padding.Bottom;
    }
    return rectangleHeight;
  }

  internal void DrawTable(
    PdfDocument lDoc,
    PdfPage page,
    List<HtmlInternalLink> internalLinkCollection)
  {
    int index1 = 0;
    this.m_template = lDoc.Pages[index1].CreateTemplate();
    PdfGraphics graphics = lDoc.Pages[index1].Graphics;
    SizeF sizeF = this.TitleStyle.Font.MeasureString(this.Title);
    RectangleF rectangle = new RectangleF(12f, 12f + this.HeaderHeight, page.Graphics.ClientSize.Width - 27f, sizeF.Height - this.FooterHeight);
    graphics.DrawRectangle(this.TitleStyle.BackgroundColor, rectangle);
    graphics.DrawString(this.Title, this.TitleStyle.Font, this.TitleStyle.ForeColor, new RectangleF(12f, 12f + this.HeaderHeight, page.Graphics.ClientSize.Width, page.Graphics.ClientSize.Height), new PdfStringFormat(this.TitleAlignment));
    float y = (float) (12.0 + (double) sizeF.Height + 15.0) + this.HeaderHeight;
    int index2 = 0;
    HtmlInternalLink internalLink1 = internalLinkCollection[index2];
    for (int index3 = 0; index3 < internalLinkCollection.Count; ++index3)
    {
      HtmlInternalLink internalLink2 = internalLinkCollection[index3];
      if (internalLink2.HeaderTagLevel != null)
      {
        int num = int.Parse(internalLink2.HeaderTagLevel.Split('H')[1]);
        if (num <= this.MaximumHeaderLevel)
        {
          float tocRectHeight = internalLink2.TocRectHeight;
          if ((double) y + (double) tocRectHeight > (double) page.Graphics.ClientSize.Height - (double) this.FooterHeight)
          {
            ++index1;
            this.isNextPage = true;
            this.m_template = lDoc.Pages[index1].CreateTemplate();
            graphics = lDoc.Pages[index1].Graphics;
            y = 0.0f;
          }
          float tocXcoordinate = internalLink2.TocXcoordinate;
          float width = page.Graphics.ClientSize.Width - (tocXcoordinate + 15f);
          RectangleF rectangleF = new RectangleF(tocXcoordinate, y, width, tocRectHeight);
          this.HeaderStyle = this.m_headerStyleCollection[num - 1];
          graphics.DrawRectangle(this.HeaderStyle.BackgroundColor, rectangleF);
          this.DrawHeaderContent(rectangleF, graphics, internalLink2);
          if (this.IsBlinkRenderingEngine)
          {
            rectangleF.Y -= this.HeaderHeight;
            PdfDocumentLinkAnnotation annotation = new PdfDocumentLinkAnnotation(rectangleF);
            internalLink2.DestinationPage = (PdfPageBase) lDoc.Pages[internalLink2.DestinationPageNumber + (this.TocPageCount - 1)];
            PdfDestination pdfDestination = new PdfDestination(internalLink2.DestinationPage);
            internalLink2.Destination = new PointF(internalLink2.Destination.X, internalLink2.Destination.Y - this.HeaderHeight);
            pdfDestination.Location = internalLink2.Destination;
            annotation.Destination = pdfDestination;
            annotation.Border.Width = 0.0f;
            lDoc.Pages[index1].Annotations.Add((PdfAnnotation) annotation);
          }
          else
            this.AddDocumentLinkAnnotation(rectangleF, lDoc, page, internalLink2);
          y += tocRectHeight + 3f;
        }
      }
    }
  }

  private void DrawHeaderContent(
    RectangleF rectBounds,
    PdfGraphics graphics,
    HtmlInternalLink htmlToc)
  {
    PdfStringFormat format = new PdfStringFormat();
    format.Alignment = PdfTextAlignment.Left;
    graphics.DrawString(htmlToc.HeaderContent, this.HeaderStyle.Font, this.HeaderStyle.ForeColor, new RectangleF(rectBounds.X + this.HeaderStyle.Padding.Left, rectBounds.Y + this.HeaderStyle.Padding.Top, rectBounds.Width - (this.HeaderStyle.Padding.Right + this.HeaderStyle.Padding.Left), rectBounds.Height), format);
    int num = htmlToc.DestinationPageNumber + this.TocPageCount + this.StartingPageNumber;
    SizeF sizeF = this.HeaderStyle.Font.MeasureString(num.ToString());
    SizeF contentSize = this.HeaderStyle.Font.MeasureString(htmlToc.HeaderContent);
    if (this.TabLeader != HtmlToPdfToc.TabLeaderStyle.None)
      this.DrawTabLeader(rectBounds, graphics, contentSize, sizeF.Width, htmlToc.HeaderContent);
    format.Alignment = PdfTextAlignment.Right;
    if ((double) contentSize.Width < (double) rectBounds.Width - ((double) sizeF.Width + (double) this.HeaderStyle.Padding.Left))
      graphics.DrawString(num.ToString(), this.HeaderStyle.Font, this.HeaderStyle.ForeColor, new RectangleF(rectBounds.X, rectBounds.Y + this.HeaderStyle.Padding.Top, rectBounds.Width - this.HeaderStyle.Padding.Right, rectBounds.Height), format);
    else
      graphics.DrawString(num.ToString(), this.HeaderStyle.Font, this.HeaderStyle.ForeColor, new RectangleF(rectBounds.X, (float) ((double) rectBounds.Y + (double) rectBounds.Height - ((double) contentSize.Height + (double) this.HeaderStyle.Padding.Bottom)), rectBounds.Width - this.HeaderStyle.Padding.Right, rectBounds.Height), format);
  }

  private void DrawTabLeader(
    RectangleF rectBounds,
    PdfGraphics graphics,
    SizeF contentSize,
    float pageNumberWidth,
    string HeaderContent)
  {
    if (this.TabLeaderChar == char.MinValue && !this.isTabLeaderChar)
    {
      this.isTabLeaderChar = true;
      switch (this.TabLeader)
      {
        case HtmlToPdfToc.TabLeaderStyle.Dash:
          this.TabLeaderChar = '-';
          break;
        case HtmlToPdfToc.TabLeaderStyle.Dot:
          this.TabLeaderChar = '.';
          break;
        case HtmlToPdfToc.TabLeaderStyle.Solid:
          this.TabLeaderChar = '_';
          break;
      }
    }
    bool flag = true;
    PdfFont font = (PdfFont) new PdfStandardFont(PdfFontFamily.TimesRoman, 9.5f, PdfFontStyle.Regular);
    SizeF sizeF1 = font.MeasureString(this.TabLeaderChar.ToString());
    PdfStringFormat format = new PdfStringFormat();
    float num1 = this.HeaderStyle.Padding.Left;
    SizeF sizeF2 = this.HeaderStyle.Font.MeasureString(" ");
    float num2;
    if ((double) contentSize.Width < (double) rectBounds.Width - ((double) pageNumberWidth + (double) this.HeaderStyle.Padding.Right))
    {
      num2 = contentSize.Width + this.HeaderStyle.Padding.Left + sizeF1.Width;
    }
    else
    {
      string str = HeaderContent;
      char[] chArray = new char[1]{ ' ' };
      foreach (string text in str.Split(chArray))
      {
        SizeF sizeF3 = this.HeaderStyle.Font.MeasureString(text);
        float num3 = num1 + sizeF3.Width;
        num1 = (double) num3 <= (double) rectBounds.Width - (double) this.HeaderStyle.Padding.Right ? num3 + sizeF2.Width : this.HeaderStyle.Padding.Left + sizeF3.Width + sizeF2.Width;
      }
      num2 = num1;
      flag = false;
    }
    string s = "";
    for (; (double) num2 < (double) rectBounds.Width - ((double) pageNumberWidth + (double) this.HeaderStyle.Padding.Right); num2 += sizeF1.Width)
      s += (string) (object) this.TabLeaderChar;
    float num4 = this.HeaderStyle.Font.Size - font.Size;
    if (flag)
      graphics.DrawString(s, font, this.HeaderStyle.ForeColor, new RectangleF(rectBounds.X + contentSize.Width + this.HeaderStyle.Padding.Left, rectBounds.Y + this.HeaderStyle.Padding.Top + num4, rectBounds.Width - this.HeaderStyle.Padding.Right, rectBounds.Height), format);
    else
      graphics.DrawString(s, font, this.HeaderStyle.ForeColor, new RectangleF(rectBounds.X + num1 - sizeF2.Width, (float) ((double) rectBounds.Y + (double) num4 + (double) rectBounds.Height - ((double) contentSize.Height + (double) this.HeaderStyle.Padding.Bottom)), rectBounds.Width, rectBounds.Height), format);
  }

  private void AddDocumentLinkAnnotation(
    RectangleF rectBounds,
    PdfDocument lDoc,
    PdfPage page,
    HtmlInternalLink htmltoc)
  {
    HtmlToPdfFormat format = new HtmlToPdfFormat();
    format.HtmlInternalLinksCollection.Add(new HtmlInternalLink()
    {
      Bounds = rectBounds,
      Destination = new PointF(htmltoc.Destination.X, htmltoc.Destination.Y),
      DestinationPage = (PdfPageBase) lDoc.Pages[htmltoc.DestinationPageNumber + (this.TocPageCount - 1)]
    });
    if (this.isFirstPage)
    {
      this.m_pageTemplate = page;
      this.isFirstPage = false;
    }
    if (this.isNextPage)
    {
      this.m_pageTemplate = this.m_pageLayoutResult.Page;
      this.m_pageLayoutBottom = this.m_pageLayoutResult.Bounds.Bottom;
      this.isNextPage = false;
    }
    this.m_pageLayoutResult = this.m_template.Draw(this.m_pageTemplate, format, new RectangleF(0.0f, this.m_pageLayoutBottom, page.Graphics.ClientSize.Width, page.Graphics.ClientSize.Height));
  }

  public void SetItemStyle(int headingStyle, HtmlToPdfTocStyle tocStyle)
  {
    if (headingStyle > 6 || headingStyle <= 0)
      return;
    HtmlToPdfTocStyle headerStyle = this.m_headerStyleCollection[headingStyle - 1];
    if (tocStyle.BackgroundColor != null)
      headerStyle.BackgroundColor = tocStyle.BackgroundColor;
    if (tocStyle.Font != null)
      headerStyle.Font = tocStyle.Font;
    if (tocStyle.ForeColor != null)
      headerStyle.ForeColor = tocStyle.ForeColor;
    if (tocStyle.Padding == null)
      return;
    headerStyle.Padding = tocStyle.Padding;
  }

  public enum TabLeaderStyle
  {
    None = 0,
    Dash = 45, // 0x0000002D
    Dot = 46, // 0x0000002E
    Solid = 95, // 0x0000005F
  }
}
