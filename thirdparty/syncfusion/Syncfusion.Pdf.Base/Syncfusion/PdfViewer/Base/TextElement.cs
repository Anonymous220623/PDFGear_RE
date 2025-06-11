// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.TextElement
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Threading;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class TextElement
{
  internal float TextHorizontalScaling = 100f;
  private SystemFontOpenTypeFontSource openTypeFontSource;
  private string[] names;
  private SystemFontFontsManager systemFontsManager;
  private CharCode? firstCode;
  internal static StdFontsAssistant manager = new StdFontsAssistant();
  internal Matrix documentMatrix = new Matrix();
  internal int Rise;
  internal Matrix textLineMatrix;
  internal Matrix Ctm;
  internal Matrix transformMatrix;
  private FontSource FontSource;
  private TransformationStack transformations;
  internal string FontID = string.Empty;
  private bool m_isMpdfFont;
  internal static ConcurrentDictionary<string, FontSource> fontSourceCache = new ConcurrentDictionary<string, FontSource>();
  internal Dictionary<string, double> ReverseMapTable = new Dictionary<string, double>();
  internal bool IsType1Font;
  internal bool Is1C;
  internal CffGlyphs m_cffGlyphs = new CffGlyphs();
  internal Dictionary<string, byte[]> m_type1FontGlyphs = new Dictionary<string, byte[]>();
  internal Font textFont;
  internal string renderedText = string.Empty;
  private float CharSizeMultiplier = 1f / 1000f;
  internal string m_fontName;
  internal FontStyle m_fontStyle;
  internal float m_fontSize;
  internal string m_fontEncoding;
  internal string m_text;
  private bool m_spaceCheck;
  private GraphicsPath pathGeom = new GraphicsPath();
  internal Brush m_pathBrush;
  internal Brush m_pathNonStrokeBrush;
  internal GraphicsPath GlyfDatapath;
  internal float m_wordSpacing;
  internal float m_characterSpacing;
  internal float m_textScaling = 100f;
  internal int m_renderingMode;
  private Font m_font;
  internal bool isNegativeFont;
  private static Dictionary<string, string> fontList = new Dictionary<string, string>();
  private PdfViewerExceptions exceptions = new PdfViewerExceptions();
  internal Dictionary<int, int> FontGlyphWidths;
  internal float DefaultGlyphWidth;
  internal bool IsTransparentText;
  internal bool IsCID;
  internal bool IsFindText;
  internal bool IsPdfium;
  internal Dictionary<double, string> CharacterMapTable;
  internal Dictionary<int, string> differenceTable = new Dictionary<int, string>();
  internal Dictionary<string, string> differenceMappedTable = new Dictionary<string, string>();
  internal Dictionary<int, int> OctDecMapTable;
  internal Dictionary<int, int> EncodedTextBytes;
  internal Dictionary<int, int> CidToGidReverseMapTable;
  internal Dictionary<int, string> UnicodeCharMapTable;
  internal FontFile2 Fontfile2Glyph;
  internal float Textscalingfactor;
  internal FontStructure structure;
  internal bool IsContainFontfile2;
  public float currentGlyphWidth;
  internal bool Isembeddedfont;
  internal int FontFlag;
  internal float LineWidth;
  internal Dictionary<SystemFontFontDescriptor, SystemFontOpenTypeFontSource> testdict;
  internal Image type3GlyphImage;
  internal Matrix type3TextMatrix = new Matrix();
  internal List<Glyph> textElementGlyphList;
  internal float pageRotation;
  internal float zoomFactor = 1f;
  internal List<object> htmldata;
  internal Dictionary<string, string> SubstitutedFontsList = new Dictionary<string, string>();
  private readonly object fontResourceLocker = new object();
  private bool m_isExtractTextData;
  private string m_embeddedFontFamily;
  internal bool m_isRectation;
  private long[] MacRomanToUnicode = new long[128 /*0x80*/]
  {
    196L,
    197L,
    199L,
    201L,
    209L,
    214L,
    220L,
    225L,
    224L /*0xE0*/,
    226L,
    228L,
    227L,
    229L,
    231L,
    233L,
    232L,
    234L,
    235L,
    237L,
    236L,
    238L,
    239L,
    241L,
    243L,
    242L,
    244L,
    246L,
    245L,
    250L,
    249L,
    251L,
    252L,
    8224L,
    176L /*0xB0*/,
    162L,
    163L,
    167L,
    8226L,
    182L,
    223L,
    174L,
    169L,
    8482L,
    180L,
    168L,
    8800L,
    198L,
    216L,
    8734L,
    177L,
    8804L,
    8805L,
    165L,
    181L,
    8706L,
    8721L,
    8719L,
    960L,
    8747L,
    170L,
    186L,
    937L,
    230L,
    248L,
    191L,
    161L,
    172L,
    8730L,
    402L,
    8776L,
    8710L,
    171L,
    187L,
    8230L,
    160L /*0xA0*/,
    192L /*0xC0*/,
    195L,
    213L,
    338L,
    339L,
    8211L,
    8212L,
    8220L,
    8221L,
    8216L,
    8217L,
    247L,
    9674L,
    (long) byte.MaxValue,
    376L,
    8260L,
    8364L,
    8249L,
    8250L,
    64257L,
    64258L,
    8225L,
    183L,
    8218L,
    8222L,
    8240L,
    194L,
    202L,
    193L,
    203L,
    200L,
    205L,
    206L,
    207L,
    204L,
    211L,
    212L,
    63743L,
    210L,
    218L,
    219L,
    217L,
    305L,
    710L,
    732L,
    175L,
    728L,
    729L,
    730L,
    184L,
    733L,
    731L,
    711L
  };
  private string m_zapfPostScript;
  internal Color m_brushColor;
  private static readonly object m_locker = new object();
  private Dictionary<int, string> m_macEncodeTable;

  internal bool IsExtractTextData
  {
    get => this.m_isExtractTextData;
    set => this.m_isExtractTextData = value;
  }

  internal TextElement(string text) => this.m_text = text;

  internal TextElement(string text, Matrix transformMatrix)
  {
    this.m_text = text;
    this.transformations = new TransformationStack();
    if (this.transformations != null)
      this.transformations.Clear();
    this.transformations = new TransformationStack(transformMatrix);
    this.textElementGlyphList = new List<Glyph>();
  }

  internal TextElement(Image img, Matrix transformMatrix)
  {
    this.type3GlyphImage = img;
    this.transformations = new TransformationStack();
    if (this.transformations != null)
      this.transformations.Clear();
    this.transformations = new TransformationStack(transformMatrix);
    this.textElementGlyphList = new List<Glyph>();
  }

  internal string FontName
  {
    get => this.m_fontName;
    set => this.m_fontName = value;
  }

  internal Font Font
  {
    get => this.m_font;
    set => this.m_font = value;
  }

  internal CharCode CharID { get; set; }

  internal FontStyle FontStyle
  {
    get => this.m_fontStyle;
    set => this.m_fontStyle = value;
  }

  internal float FontSize
  {
    get => this.m_fontSize;
    set => this.m_fontSize = value;
  }

  public string ZapfPostScript
  {
    get => this.m_zapfPostScript;
    set => this.m_zapfPostScript = value;
  }

  internal string FontEncoding
  {
    get => this.m_fontEncoding;
    set => this.m_fontEncoding = value;
  }

  internal string Text
  {
    get => this.m_text;
    set => this.m_text = value;
  }

  internal Brush PathBrush
  {
    get => this.m_pathBrush;
    set => this.m_pathBrush = value;
  }

  internal Brush PathNonStrokeBrush
  {
    get => this.m_pathNonStrokeBrush;
    set => this.m_pathNonStrokeBrush = value;
  }

  internal float WordSpacing
  {
    get => this.m_wordSpacing;
    set => this.m_wordSpacing = value;
  }

  internal float CharacterSpacing
  {
    get => this.m_characterSpacing;
    set => this.m_characterSpacing = value;
  }

  internal float TextScaling
  {
    get => this.m_textScaling;
    set => this.m_textScaling = value;
  }

  internal int RenderingMode
  {
    get => this.m_renderingMode;
    set => this.m_renderingMode = value;
  }

  private Matrix GetTextRenderingMatrix()
  {
    return new Matrix()
    {
      M11 = (double) this.FontSize * ((double) this.TextHorizontalScaling / 100.0),
      M22 = -(double) this.FontSize,
      OffsetY = ((double) this.FontSize + (double) this.Rise)
    } * this.textLineMatrix * this.Ctm;
  }

  private string[] GetStandardFontEncodingNames()
  {
    return this.structure.FontEncoding != "MacRomanEncoding" ? PredefinedTextEncoding.GetPredefinedEncoding("WinAnsiEncoding").GetNames() : PredefinedTextEncoding.GetPredefinedEncoding("MacRomanEncoding").GetNames();
  }

  public virtual double GetGlyphWidth(Glyph glyph)
  {
    int intValue = glyph.CharId.IntValue;
    if (this.FontGlyphWidths != null && this.structure.fontType.Value == "TrueType" && this.FontGlyphWidths.ContainsKey(intValue))
    {
      double num = (double) this.FontGlyphWidths[intValue] * (double) this.CharSizeMultiplier;
      glyph.AdvancedWidth = num;
    }
    else if (this.FontSource != null)
      this.FontSource.GetAdvancedWidth(glyph);
    else
      glyph.AdvancedWidth = 1.0;
    return glyph.AdvancedWidth;
  }

  private double GetSystemFontGlyphWidth(Glyph glyph, System.Drawing.Graphics g)
  {
    int intValue = glyph.CharId.IntValue;
    if (this.FontGlyphWidths != null && this.structure.fontType.Value == "TrueType" && this.FontGlyphWidths.ContainsKey(intValue))
    {
      double num = (double) this.FontGlyphWidths[intValue] * (double) this.CharSizeMultiplier;
      glyph.AdvancedWidth = num;
    }
    else
    {
      if (this.FontSource == null)
        return -1.0;
      this.FontSource.GetAdvancedWidth(glyph);
    }
    return glyph.AdvancedWidth;
  }

  private string GetGlyphName(Glyph glyph)
  {
    if (this.FontSource != null)
      this.FontSource.GetGlyphName(glyph);
    return glyph.Name;
  }

  private void GlyphToSLCoordinates(Glyph glyph)
  {
    Matrix transformMatrix = glyph.TransformMatrix;
    transformMatrix.Translate(0.0, -glyph.Ascent / 1000.0);
    glyph.TransformMatrix = transformMatrix;
  }

  private System.Drawing.Drawing2D.Matrix GetTransformationMatrix(Matrix transform)
  {
    return new System.Drawing.Drawing2D.Matrix((float) transform.M11, (float) transform.M12, (float) transform.M21, (float) transform.M22, (float) transform.OffsetX, (float) transform.OffsetY);
  }

  private void UpdateTextMatrix(double tj)
  {
    double x = -(tj * 0.001 * (double) this.FontSize * (double) this.TextHorizontalScaling / 100.0);
    Point point1 = this.textLineMatrix.Transform(new Point(0.0, 0.0));
    Point point2 = this.textLineMatrix.Transform(new Point(x, 0.0));
    if (point1.X != point2.X)
      this.textLineMatrix.OffsetX = point2.X;
    else
      this.textLineMatrix.OffsetY = point2.Y;
  }

  private void UpdateTextMatrix(Glyph glyph)
  {
    this.textLineMatrix = this.CalculateTextMatrix(this.textLineMatrix, glyph);
  }

  private Matrix CalculateTextMatrix(Matrix m, Glyph glyph)
  {
    if (glyph.CharId.IntValue == 32 /*0x20*/)
      glyph.WordSpacing = (double) this.WordSpacing;
    return new Matrix(1.0, 0.0, 0.0, 1.0, (glyph.Width * glyph.FontSize + glyph.CharSpacing + glyph.WordSpacing) * (glyph.HorizontalScaling / 100.0), 0.0) * m;
  }

  internal Color BrushColor
  {
    get => this.m_brushColor;
    set => this.m_brushColor = value;
  }

  internal float Render(System.Drawing.Graphics g, PointF currentLocation)
  {
    this.renderedText = string.Empty;
    string fontName = this.FontName;
    this.textFont = (Font) null;
    PdfUnitConvertor pdfUnitConvertor = new PdfUnitConvertor();
    float x = currentLocation.X;
    if ((double) this.FontSize < 0.0)
      this.FontSize = -this.FontSize;
    if (this.Font != null)
    {
      this.FontName = TextElement.CheckFontName(this.Font.Name);
      this.textFont = this.Font;
    }
    else
    {
      this.CheckFontStyle(this.FontName);
      this.FontName = TextElement.CheckFontName(this.FontName);
      this.textFont = new Font(this.FontName, this.FontSize, this.FontStyle);
    }
    string[] strArray = this.Text.Split(' ');
    g.MeasureString(this.Text, this.textFont);
    PointF point = currentLocation;
    int num1 = 0;
    foreach (string text in strArray)
    {
      ++num1;
      g.MeasureString(text, this.textFont);
      int num2 = 0;
      StringFormat stringFormat = new StringFormat(StringFormat.GenericTypographic);
      point.X += this.CharacterSpacing;
      foreach (char key in text)
      {
        ++num2;
        SizeF sizeF = g.MeasureString(key.ToString(), this.textFont, PointF.Empty, stringFormat);
        float num3 = sizeF.Width / 100f * this.TextScaling;
        if (this.FontGlyphWidths != null)
        {
          if (this.FontGlyphWidths.ContainsKey((int) key))
            num3 = (float) this.FontGlyphWidths[(int) key] * (this.CharSizeMultiplier * this.FontSize) / 100f * this.TextScaling;
        }
        try
        {
          if ((byte) key > (byte) 126 && this.m_fontEncoding == "MacRomanEncoding")
          {
            char ch = (char) this.MacRomanToUnicode[(int) (byte) key - 128 /*0x80*/];
            if (this.isNegativeFont)
            {
              GraphicsState gstate = g.Save();
              g.MultiplyTransform(new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, -1f, 0.0f, (float) (2.0 * (double) point.Y + 2.0 * (double) sizeF.Height)));
              g.DrawString(key.ToString(), this.textFont, new Pen(this.BrushColor).Brush, point, stringFormat);
              g.Restore(gstate);
            }
            else if (this.CharacterMapTable != null && this.CharacterMapTable.ContainsKey((double) key))
            {
              string s = this.CharacterMapTable[(double) key];
              g.DrawString(s, this.textFont, new Pen(this.BrushColor).Brush, point, stringFormat);
            }
            else
              g.DrawString(ch.ToString(), this.textFont, new Pen(this.BrushColor).Brush, point, stringFormat);
          }
          else
          {
            if (this.isNegativeFont)
            {
              GraphicsState gstate = g.Save();
              g.MultiplyTransform(new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, -1f, 0.0f, (float) (2.0 * (double) point.Y + 2.0 * (double) sizeF.Height)));
              g.DrawString(key.ToString(), this.textFont, new Pen(this.BrushColor).Brush, point, stringFormat);
              g.Restore(gstate);
            }
            else if (key > '\u007F' && key <= 'ÿ' && this.m_fontEncoding == "WinAnsiEncoding")
            {
              string str = Encoding.Default.GetString(new byte[1]
              {
                (byte) key
              });
              g.DrawString(str, this.textFont, new Pen(this.BrushColor).Brush, point, stringFormat);
              sizeF = g.MeasureString(str, this.textFont, PointF.Empty, stringFormat);
              num3 = sizeF.Width / 100f * this.TextScaling;
            }
            else if (this.CharacterMapTable != null && this.CharacterMapTable.ContainsKey((double) key))
            {
              string s = this.CharacterMapTable[(double) key];
              g.DrawString(s, this.textFont, new Pen(this.BrushColor).Brush, point, stringFormat);
            }
            else
              g.DrawString(key.ToString(), this.textFont, new Pen(this.BrushColor).Brush, point, stringFormat);
            this.renderedText += key.ToString();
          }
        }
        catch (Exception ex)
        {
          this.exceptions.Exceptions.Append($"\r\nCharacter not rendered {key.ToString()}\r\n{ex.StackTrace}");
          continue;
        }
        if (num2 < text.Length)
          point.X += num3 + this.CharacterSpacing;
        else
          point.X += num3;
      }
      float width = g.MeasureString(" ", this.textFont, PointF.Empty, new StringFormat(StringFormat.GenericTypographic)
      {
        FormatFlags = StringFormatFlags.MeasureTrailingSpaces
      }).Width;
      if (num1 < strArray.Length)
        point.X += this.WordSpacing + width + this.CharacterSpacing;
    }
    return point.X - x;
  }

  internal float RenderWithSpace(
    System.Drawing.Graphics g,
    PointF currentLocation,
    List<string> decodedList,
    List<float> characterSpacings)
  {
    this.renderedText = string.Empty;
    string fontName = this.FontName;
    this.textFont = (Font) null;
    float x = currentLocation.X;
    PdfUnitConvertor pdfUnitConvertor = new PdfUnitConvertor();
    if ((double) this.FontSize < 0.0)
      this.FontSize = -this.FontSize;
    if (this.Font != null)
    {
      this.FontName = TextElement.CheckFontName(this.Font.Name);
      this.textFont = this.Font;
    }
    else
    {
      this.CheckFontStyle(this.FontName);
      this.FontName = TextElement.CheckFontName(this.FontName);
      this.textFont = new Font(this.FontName, this.FontSize, this.FontStyle);
    }
    g.MeasureString(this.Text, this.textFont);
    PointF point = currentLocation;
    foreach (string decoded in decodedList)
    {
      StringFormat stringFormat = new StringFormat(StringFormat.GenericTypographic);
      float width = g.MeasureString(" ", this.textFont, PointF.Empty, new StringFormat(StringFormat.GenericTypographic)
      {
        FormatFlags = StringFormatFlags.MeasureTrailingSpaces
      }).Width;
      float result;
      if (float.TryParse(decoded, out result))
      {
        float sizeInPoints = this.textFont.SizeInPoints;
        result *= sizeInPoints / 1000f;
        result -= this.CharacterSpacing;
        point.X -= result;
      }
      else
      {
        string str = decoded.Remove(decoded.Length - 1, 1);
        int num1 = 0;
        foreach (char key in str)
        {
          ++num1;
          if (key == ' ')
          {
            point.X += width + this.WordSpacing;
            this.renderedText += " ";
          }
          else
          {
            SizeF sizeF = g.MeasureString(key.ToString(), this.textFont, PointF.Empty, stringFormat);
            float num2 = sizeF.Width / 100f * this.TextScaling;
            if (this.FontGlyphWidths != null)
            {
              if (this.FontGlyphWidths.ContainsKey((int) key))
                num2 = (float) this.FontGlyphWidths[(int) key] * (this.CharSizeMultiplier * this.FontSize) / 100f * this.TextScaling;
            }
            try
            {
              if ((byte) key > (byte) 126 && this.m_fontEncoding == "MacRomanEncoding")
              {
                char ch = (char) this.MacRomanToUnicode[(int) (byte) key - 128 /*0x80*/];
                if (this.isNegativeFont)
                {
                  GraphicsState gstate = g.Save();
                  g.MultiplyTransform(new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, -1f, 0.0f, (float) (2.0 * (double) point.Y + 2.0 * (double) sizeF.Height)));
                  g.DrawString(key.ToString(), this.textFont, new Pen(this.BrushColor).Brush, point, stringFormat);
                  g.Restore(gstate);
                }
                else if (this.CharacterMapTable != null && this.CharacterMapTable.ContainsKey((double) key))
                {
                  string s = this.CharacterMapTable[(double) key];
                  g.DrawString(s, this.textFont, new Pen(this.BrushColor).Brush, point, stringFormat);
                }
                else
                  g.DrawString(ch.ToString(), this.textFont, new Pen(this.BrushColor).Brush, point, stringFormat);
              }
              else
              {
                if (this.isNegativeFont)
                {
                  GraphicsState gstate = g.Save();
                  g.MultiplyTransform(new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, -1f, 0.0f, (float) (2.0 * (double) point.Y + 2.0 * (double) sizeF.Height)));
                  g.DrawString(key.ToString(), this.textFont, new Pen(this.BrushColor).Brush, point, stringFormat);
                  g.Restore(gstate);
                }
                else if (this.CharacterMapTable != null && this.CharacterMapTable.ContainsKey((double) key))
                {
                  string s = this.CharacterMapTable[(double) key];
                  g.DrawString(s, this.textFont, new Pen(this.BrushColor).Brush, point, stringFormat);
                }
                else
                  g.DrawString(key.ToString(), this.textFont, new Pen(this.BrushColor).Brush, point, stringFormat);
                this.renderedText += key.ToString();
              }
            }
            catch (Exception ex)
            {
              this.exceptions.Exceptions.Append($"\r\nCharacter not rendered {key.ToString()}\r\n{ex.StackTrace}");
              continue;
            }
            if (num1 < str.Length)
              point.X += num2 + this.CharacterSpacing;
            else
              point.X += num2;
          }
        }
      }
    }
    return point.X - x;
  }

  internal float Render(
    System.Drawing.Graphics g,
    PointF currentLocation,
    double textScaling,
    Dictionary<int, int> gWidths,
    double type1Height,
    Dictionary<int, string> differenceTable,
    Dictionary<string, string> differenceMappedTable,
    Dictionary<int, string> differenceEncoding,
    out Matrix txtMatrix)
  {
    Monitor.Enter(this.fontResourceLocker);
    try
    {
      this.m_isMpdfFont = this.isMpdfaaFonts();
      if (this.type3GlyphImage != null)
      {
        txtMatrix = new Matrix();
        this.DrawType3Glyphs(this.type3GlyphImage, g);
        return 0.0f;
      }
      txtMatrix = Matrix.Identity;
      string fontName1 = this.FontName;
      this.renderedText = string.Empty;
      string fontName2 = this.FontName;
      int index1 = 0;
      string[] strArray1 = (string[]) null;
      this.textFont = (Font) null;
      PdfUnitConvertor pdfUnitConvertor1 = new PdfUnitConvertor();
      float x = currentLocation.X;
      if (this.Font != null && this.Isembeddedfont)
      {
        this.BackupEmbededFontName(fontName1);
        this.FontName = TextElement.CheckFontName(this.Font.Name);
        this.textFont = this.Font;
      }
      else
      {
        this.CheckFontStyle(this.FontName);
        this.FontName = TextElement.CheckFontName(this.FontName);
        this.textFont = (double) this.FontSize >= 0.0 ? new Font(this.FontName, this.FontSize, this.FontStyle) : new Font(this.FontName, -this.FontSize, this.FontStyle);
      }
      string[] strArray2 = this.Text.Split(' ');
      g.MeasureString(this.Text, this.textFont);
      PointF newLocation = currentLocation;
      if (this.IsTransparentText)
        this.PathBrush = Pens.Transparent.Brush;
      double fontSize1 = (double) this.FontSize;
      if (!this.IsType1Font && this.m_fontEncoding != "MacRomanEncoding" && this.m_fontEncoding != "WinAnsiEncoding" && !this.isNegativeFont)
      {
        string str = strArray2[0];
        for (int index2 = 1; index2 < strArray2.Length; ++index2)
          str = str + (object) ' ' + strArray2[index2];
        new string[1][0] = str;
      }
      string str1 = this.ResolveFontName(fontName1);
      if (StdFontsAssistant.IsStandardFontName(str1) && !this.Isembeddedfont)
      {
        if (this.m_fontStyle == FontStyle.Bold && !str1.Contains("Bold"))
          str1 = !StdFontsAssistant.IsAlternativeStdFontAvailable(str1) ? str1 + "-Bold" : str1 + ",Bold";
        if (this.m_fontStyle == FontStyle.Italic && !str1.Contains("Italic"))
          str1 = !StdFontsAssistant.IsAlternativeStdFontAvailable(str1) ? (str1.Contains("Courier") || str1.Contains("Helvetica") ? str1 + "-Oblique" : str1 + "-Italic") : str1 + ",Italic";
        if (this.m_fontStyle == (FontStyle.Bold | FontStyle.Italic) && !str1.Contains("Italic") && !str1.Contains("Bold"))
          str1 = !StdFontsAssistant.IsAlternativeStdFontAvailable(str1) ? (str1.Contains("Courier") || str1.Contains("Helvetica") ? str1 + "-Bold" + "Oblique" : str1 + "-Bold" + "Italic") : str1 + ",Bold" + "Italic";
        GraphicsUnit pageUnit = g.PageUnit;
        System.Drawing.Drawing2D.Matrix transform = g.Transform;
        g.PageUnit = GraphicsUnit.Pixel;
        if (!TextElement.fontSourceCache.ContainsKey(this.FontID + this.structure.FontRefNumber))
        {
          this.FontSource = (FontSource) TextElement.manager.GetStandardFontSource(str1);
          TextElement.fontSourceCache.TryAdd(this.FontID + this.structure.FontRefNumber, this.FontSource);
        }
        else
          this.FontSource = TextElement.fontSourceCache[this.FontID + this.structure.FontRefNumber];
        if (this.ZapfPostScript != null)
          strArray1 = this.ZapfPostScript.Split(new char[1]
          {
            ' '
          }, StringSplitOptions.RemoveEmptyEntries);
        foreach (char index3 in this.Text)
        {
          g.Transform = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
          Glyph glyph1 = new Glyph();
          glyph1.FontSize = (double) this.FontSize;
          glyph1.FontFamily = this.FontName;
          glyph1.FontStyle = this.FontStyle;
          glyph1.Stroke = this.PathBrush;
          glyph1.NonStroke = this.PathNonStrokeBrush;
          glyph1.TransformMatrix = this.GetTextRenderingMatrix();
          glyph1.Name = index3.ToString();
          glyph1.HorizontalScaling = (double) this.TextHorizontalScaling;
          glyph1.CharId = new CharCode((int) index3);
          glyph1.CharSpacing = (double) this.CharacterSpacing;
          string[] fontEncodingNames = this.GetStandardFontEncodingNames();
          byte[] bytes = Encoding.UTF8.GetBytes(index3.ToString());
          int num1;
          if (this.structure.ReverseDictMapping.ContainsKey(index3.ToString()))
          {
            float num2 = (float) this.structure.ReverseDictMapping[index3.ToString()];
            if (this.structure.DifferencesDictionary.ContainsKey(num2.ToString()))
            {
              glyph1.Name = FontStructure.GetCharCode(this.structure.DifferencesDictionary[num2.ToString()]);
            }
            else
            {
              bytes[0] = (byte) num2;
              glyph1.Name = fontEncodingNames[(int) bytes[0]];
            }
          }
          else if (this.OctDecMapTable != null && this.OctDecMapTable.ContainsKey((int) index3) && this.structure.FontName != "Symbol")
          {
            char index4 = (char) this.OctDecMapTable[(int) index3];
            glyph1.Name = fontEncodingNames[(int) index4];
          }
          else
          {
            if (this.structure.DifferencesDictionary != null)
            {
              Dictionary<string, string> differencesDictionary1 = this.structure.DifferencesDictionary;
              num1 = (int) index3;
              string key1 = num1.ToString();
              if (differencesDictionary1.ContainsKey(key1) && this.structure.BaseFontEncoding != "WinAnsiEncoding")
              {
                Glyph glyph2 = glyph1;
                Dictionary<string, string> differencesDictionary2 = this.structure.DifferencesDictionary;
                num1 = (int) index3;
                string key2 = num1.ToString();
                string charCode = FontStructure.GetCharCode(differencesDictionary2[key2]);
                glyph2.Name = charCode;
                goto label_45;
              }
            }
            if (fontEncodingNames.Length > (int) index3 && this.structure.FontName != "Symbol" && this.structure.FontName != "ZapfDingbats")
              glyph1.Name = fontEncodingNames[(int) index3];
            else if (this.structure.FontName == "Symbol")
            {
              if (this.structure.FontEncoding == "Encoding")
                glyph1.Name = FontStructure.GetCharCode(index3.ToString());
              else
                glyph1.Name = this.GetGlyphName(glyph1);
            }
            else if (this.structure.FontName == "ZapfDingbats")
            {
              if (index1 < this.ZapfPostScript.Length)
              {
                glyph1.Name = strArray1[index1].Trim();
                ++index1;
              }
            }
            else
              glyph1.Name = fontEncodingNames[(int) bytes[0]];
          }
label_45:
          if (PdfDocument.EnableThreadSafe)
          {
            lock (TextElement.m_locker)
              glyph1.Width = this.GetGlyphWidth(glyph1);
          }
          else
            glyph1.Width = this.GetGlyphWidth(glyph1);
          this.FontSource.GetGlyphOutlines(glyph1, 100.0);
          GraphicsPath path = new PdfElementsRenderer().RenderGlyph(glyph1);
          Matrix identity = Matrix.Identity;
          identity.Scale(0.01, 0.01, 0.0, 0.0);
          identity.Translate(0.0, 1.0);
          this.transformations.PushTransform(identity * glyph1.TransformMatrix);
          System.Drawing.Drawing2D.Matrix matrix = g.Transform.Clone();
          matrix.Multiply(this.GetTransformationMatrix(this.transformations.CurrentTransform));
          g.Transform = matrix;
          g.SmoothingMode = SmoothingMode.AntiAlias;
          if (!this.IsPdfium)
          {
            num1 = this.RenderingMode;
            switch (num1)
            {
              case 0:
                g.FillPath(glyph1.Stroke, path);
                break;
              case 1:
                g.DrawPath(new Pen(glyph1.NonStroke, this.LineWidth), path);
                break;
              case 2:
                g.FillPath(glyph1.Stroke, path);
                if (glyph1.NonStroke != null)
                {
                  g.DrawPath(new Pen(glyph1.NonStroke, this.LineWidth), path);
                  break;
                }
                break;
            }
          }
          float num3 = 0.0f;
          if (glyph1.TransformMatrix.M11 > 0.0)
            num3 = (float) glyph1.TransformMatrix.M11;
          else if (glyph1.TransformMatrix.M12 != 0.0 && glyph1.TransformMatrix.M21 != 0.0)
            num3 = glyph1.TransformMatrix.M12 >= 0.0 ? (float) glyph1.TransformMatrix.M12 : (float) -glyph1.TransformMatrix.M12;
          else if (glyph1.TransformMatrix.M11 != 0.0)
          {
            if (glyph1.TransformMatrix.M11 < 0.0)
              num3 = -(float) glyph1.TransformMatrix.M11;
          }
          else
            num3 = glyph1.FontSize <= 0.0 ? 0.0f : (float) glyph1.FontSize;
          if ((int) num3 == 0)
            num3 = (float) (int) glyph1.FontSize;
          string str2 = index3.ToString();
          if (!this.structure.IsMappingDone)
          {
            if (this.CidToGidReverseMapTable != null && this.CidToGidReverseMapTable.ContainsKey((int) Convert.ToChar(str2)) && this.structure.CharacterMapTable != null && this.structure.CharacterMapTable.Count > 0)
              str2 = this.CharacterMapTable[(double) this.CidToGidReverseMapTable[(int) Convert.ToChar(str2)]];
            else if (this.structure.CharacterMapTable != null && this.structure.CharacterMapTable.Count > 0)
              str2 = this.structure.tempStringList.Count <= 0 ? this.structure.MapCharactersFromTable(str2.ToString()) : this.structure.CharacterMapTable[(double) Convert.ToChar(str2)];
            else if (this.structure.DifferencesDictionary != null && this.structure.DifferencesDictionary.Count > 0)
              str2 = this.structure.MapDifferences(str2.ToString());
            else if (this.structure.CidToGidReverseMapTable != null && this.structure.CidToGidReverseMapTable.ContainsKey((int) Convert.ToChar(str2)))
              str2 = ((char) this.structure.CidToGidReverseMapTable[(int) Convert.ToChar(str2)]).ToString();
            if (str2.Contains("\u0092"))
              str2 = str2.Replace("\u0092", "’");
          }
          glyph1.ToUnicode = str2;
          if ((double) this.pageRotation == 90.0 || (double) this.pageRotation == 270.0)
          {
            if ((double) matrix.Elements[1] == 0.0 && (double) matrix.Elements[2] == 0.0)
            {
              glyph1.IsRotated = false;
              glyph1.BoundingRect = new Rect(new Point((double) pdfUnitConvertor1.ConvertFromPixels(matrix.OffsetX, PdfGraphicsUnit.Point) / (double) this.zoomFactor, ((double) pdfUnitConvertor1.ConvertFromPixels(matrix.OffsetY, PdfGraphicsUnit.Point) - (double) pdfUnitConvertor1.ConvertFromPixels((float) ((double) num3 * (double) this.zoomFactor * ((double) g.DpiY / 96.0)), PdfGraphicsUnit.Point)) / (double) this.zoomFactor), new Size(glyph1.Width * (double) num3, (double) num3));
            }
            else
            {
              glyph1.IsRotated = true;
              glyph1.BoundingRect = !this.IsFindText || (double) this.pageRotation != 90.0 ? new Rect(new Point((double) pdfUnitConvertor1.ConvertFromPixels(matrix.OffsetX + (num3 + (float) (glyph1.Ascent / 1000.0)) * matrix.Elements[2], PdfGraphicsUnit.Point) / (double) this.zoomFactor, (double) pdfUnitConvertor1.ConvertFromPixels(matrix.OffsetY - num3 * matrix.Elements[2], PdfGraphicsUnit.Point) / (double) this.zoomFactor), new Size((double) num3, glyph1.Width * (double) num3)) : new Rect(new Point((double) pdfUnitConvertor1.ConvertFromPixels(matrix.OffsetX + (num3 + (float) (glyph1.Ascent / 1000.0)) * matrix.Elements[2], PdfGraphicsUnit.Point) / (double) this.zoomFactor, (double) pdfUnitConvertor1.ConvertFromPixels(matrix.OffsetY - num3 * matrix.Elements[2], PdfGraphicsUnit.Point) / (double) this.zoomFactor), new Size((double) num3, glyph1.Width * (double) num3));
            }
          }
          else
          {
            if ((double) this.pageRotation == 180.0)
              glyph1.IsRotated = (double) matrix.Elements[1] == 0.0 && (double) matrix.Elements[2] == 0.0;
            float height = 0.0f;
            if (glyph1.TransformMatrix.M12 != 0.0 && glyph1.TransformMatrix.M21 != 0.0)
              height = glyph1.TransformMatrix.M12 >= 0.0 ? (float) glyph1.TransformMatrix.M12 : (float) -glyph1.TransformMatrix.M12;
            else if (glyph1.TransformMatrix.M11 != 0.0 && glyph1.FontSize <= 1.0)
              height = (float) Math.Abs(glyph1.TransformMatrix.M11);
            if ((int) height == 0)
              height = (double) num3 == 0.0 ? (float) (int) glyph1.FontSize : (float) (int) num3;
            double m21 = glyph1.TransformMatrix.M21;
            double m11 = glyph1.TransformMatrix.M11;
            glyph1.BoundingRect = Math.Round(Math.Atan2(m21, m11) * 180.0 / Math.PI) != -90.0 ? new Rect(new Point((double) pdfUnitConvertor1.ConvertFromPixels(matrix.OffsetX, PdfGraphicsUnit.Point) / (double) this.zoomFactor, (double) pdfUnitConvertor1.ConvertFromPixels(matrix.OffsetY - (float) ((double) num3 * (double) this.zoomFactor * ((double) g.DpiY / 96.0)), PdfGraphicsUnit.Point) / (double) this.zoomFactor), new Size(glyph1.Width * (double) height, (double) height)) : new Rect(new Point((double) pdfUnitConvertor1.ConvertFromPixels(matrix.OffsetX, PdfGraphicsUnit.Point) / (double) this.zoomFactor, (double) pdfUnitConvertor1.ConvertFromPixels(matrix.OffsetY, PdfGraphicsUnit.Point) / (double) this.zoomFactor), new Size(glyph1.Width * (double) height, (double) height));
          }
          if (this.structure.CharacterMapTable != null && this.structure.CharacterMapTable.ContainsKey((double) index3))
            glyph1.ToUnicode = this.structure.CharacterMapTable[(double) index3];
          if (glyph1.ToUnicode.Length != 1)
          {
            this.textElementGlyphList.Add(glyph1);
            for (int index5 = 0; index5 < glyph1.ToUnicode.Length - 1; ++index5)
              this.textElementGlyphList.Add(new Glyph());
          }
          else
            this.textElementGlyphList.Add(glyph1);
          if (this.m_isExtractTextData && glyph1.FontSize != (double) num3)
            glyph1.MatrixFontSize = (double) num3;
          this.UpdateTextMatrix(glyph1);
          this.transformations.PopTransform();
          if (this.structure.CharacterMapTable != null && this.structure.CharacterMapTable.ContainsKey((double) index3))
            this.renderedText += glyph1.ToString();
          else
            this.renderedText += index3.ToString();
        }
        g.Transform = transform;
        g.PageUnit = pageUnit;
        txtMatrix = this.textLineMatrix;
      }
      else
      {
        int num4 = 0;
        string str3 = string.Empty;
        for (int index6 = 0; index6 < this.Text.Length; ++index6)
        {
          char ch1 = this.Text[index6];
          bool flag = false;
          if (this.IsType1Font && !this.structure.IsOpenTypeFont && differenceMappedTable.Count == 0)
          {
            str3 = this.Text[index6].ToString();
            int num5 = 0;
            if (this.ReverseMapTable.ContainsKey(str3))
            {
              foreach (KeyValuePair<double, string> keyValuePair in this.CharacterMapTable)
              {
                if (keyValuePair.Value.IndexOf(ch1) > -1)
                  ++num5;
              }
            }
            if ((!this.ReverseMapTable.ContainsKey(str3) || num5 > 1) && (char.IsLetter(ch1) || char.IsPunctuation(ch1) || char.IsSymbol(ch1)))
            {
              if (index6 != this.Text.Length - 1)
              {
                str3 = this.Text.Substring(index6);
                index6 += str3.Length - 1;
              }
            }
            if (str3 != ch1.ToString())
            {
              for (int index7 = 0; index7 < str3.Length; ++index7)
              {
                if (!this.ReverseMapTable.ContainsKey(str3))
                {
                  str3 = str3.Remove(str3.Length - 1);
                  --index6;
                  index7 = 0;
                  if (str3 == this.Text[index6].ToString())
                    str3 = string.Empty;
                  if (this.ReverseMapTable.ContainsKey(str3))
                    break;
                }
              }
            }
            else
              str3 = string.Empty;
          }
          Glyph glyph = new Glyph();
          StringFormat stringFormat = new StringFormat(StringFormat.GenericTypographic);
          ++num4;
          if (this.IsType1Font && !this.structure.IsOpenTypeFont)
          {
            string str4 = ch1.ToString();
            int index8 = (int) ch1;
            if (this.structure.DifferencesDictionary.ContainsValue(this.Text))
            {
              str4 = this.Text;
              index6 = this.Text.Length - 1;
            }
            if (this.EncodedTextBytes != null && this.ReverseMapTable.Count == this.CharacterMapTable.Count && this.OctDecMapTable.Count == 0 && index6 < this.EncodedTextBytes.Count && differenceMappedTable != null)
            {
              Dictionary<string, string> dictionary1 = differenceMappedTable;
              int encodedTextByte = this.EncodedTextBytes[index6];
              string key3 = encodedTextByte.ToString();
              if (dictionary1.ContainsKey(key3))
              {
                Dictionary<string, string> dictionary2 = differenceMappedTable;
                encodedTextByte = this.EncodedTextBytes[index6];
                string key4 = encodedTextByte.ToString();
                str4 = dictionary2[key4];
              }
            }
            if (differenceTable.ContainsValue(str4) && differenceMappedTable.ContainsValue(str4))
            {
              foreach (KeyValuePair<int, string> keyValuePair in differenceTable)
              {
                if (keyValuePair.Value == str4)
                {
                  index8 = keyValuePair.Key;
                  break;
                }
              }
            }
            else if ((this.ReverseMapTable.ContainsKey(str4) || this.ReverseMapTable.ContainsKey(str3)) && (this.ReverseMapTable.Count == differenceTable.Count || this.ReverseMapTable.Count == this.CharacterMapTable.Count))
            {
              index8 = !(str3 == string.Empty) ? (int) this.ReverseMapTable[str3] : (int) this.ReverseMapTable[str4];
              if (differenceTable.ContainsKey(index8))
                str4 = differenceTable[index8];
            }
            else if (this.CharacterMapTable.ContainsValue(str4) && this.CharacterMapTable.Count == differenceTable.Count)
            {
              foreach (KeyValuePair<double, string> keyValuePair in this.CharacterMapTable)
              {
                if (keyValuePair.Value == str4)
                {
                  index8 = (int) keyValuePair.Key;
                  if (differenceTable.ContainsKey(index8))
                  {
                    str4 = differenceTable[index8];
                    break;
                  }
                  break;
                }
              }
            }
            else if (differenceMappedTable.ContainsValue(str4))
            {
              foreach (KeyValuePair<string, string> keyValuePair in differenceMappedTable)
              {
                if (keyValuePair.Value == str4)
                {
                  index8 = int.Parse(keyValuePair.Key);
                  if (differenceTable.ContainsKey(index8))
                  {
                    str4 = differenceTable[index8];
                    break;
                  }
                  break;
                }
              }
            }
            else if (differenceMappedTable.ContainsValue(str4))
            {
              foreach (KeyValuePair<string, string> keyValuePair in differenceMappedTable)
              {
                if (keyValuePair.Value == str4)
                {
                  index8 = int.Parse(keyValuePair.Key);
                  if (differenceTable.ContainsKey(index8))
                  {
                    str4 = differenceTable[index8];
                    break;
                  }
                  break;
                }
              }
            }
            else if (differenceTable.ContainsKey(index8))
              str4 = differenceTable[index8];
            else if (this.m_cffGlyphs.DifferenceEncoding.ContainsKey(index8) && this.structure.FontEncoding != "MacRomanEncoding")
              str4 = this.m_cffGlyphs.DifferenceEncoding[index8];
            else if (this.structure.FontEncoding == "MacRomanEncoding")
            {
              if (this.structure.m_macRomanMapTable.ContainsKey(index8))
                str4 = this.structure.m_macRomanMapTable[index8];
            }
            else if (this.structure.FontEncoding == "WinAnsiEncoding")
            {
              if (this.structure.m_winansiMapTable.ContainsKey(index8))
                str4 = this.structure.m_winansiMapTable[index8];
            }
            try
            {
              if (this.Is1C)
              {
                double fontSize2 = (double) this.FontSize;
              }
              GlyphWriter glyphWriter = new GlyphWriter(this.m_cffGlyphs.Glyphs, this.m_cffGlyphs.GlobalBias, this.Is1C);
              glyphWriter.is1C = this.Is1C;
              if (this.structure.BaseFontEncoding == "WinAnsiEncoding" && this.structure.FontEncoding != "Encoding")
                glyphWriter.HasBaseEncoding = true;
              if (this.structure.m_isContainFontfile && this.structure.FontFileType1Font.m_hasFontMatrix)
                glyphWriter.FontMatrix = this.structure.FontFileType1Font.m_fontMatrix;
              if (this.structure.IsContainFontfile3 && this.structure.fontFile3Type1Font.hasFontMatrix)
                glyphWriter.FontMatrix = this.structure.fontFile3Type1Font.FontMatrix;
              if (this.FontEncoding == "MacRomanEncoding" && ch1 > '~')
              {
                this.GetMacEncodeTable();
                if (this.OctDecMapTable != null && this.OctDecMapTable.ContainsKey((int) ch1))
                {
                  index8 = this.OctDecMapTable[(int) ch1];
                  ch1 = (char) index8;
                }
                string decodedCharacter = this.m_macEncodeTable[(int) ch1];
                str4 = FontStructure.GetCharCode(decodedCharacter);
                ch1 = decodedCharacter[0];
              }
              if (!this.IsCID)
              {
                if (this.ReverseMapTable.ContainsKey(str4) && (this.ReverseMapTable.Count == differenceTable.Count || this.ReverseMapTable.Count == this.CharacterMapTable.Count) && this.m_cffGlyphs.DifferenceEncoding.ContainsKey(index8) && this.structure.FontEncoding != "MacRomanEncoding")
                  str4 = this.m_cffGlyphs.DifferenceEncoding[index8];
                if (glyphWriter.glyphs.ContainsKey(str4))
                {
                  System.Drawing.Drawing2D.Matrix transform = g.Transform;
                  PdfUnitConvertor pdfUnitConvertor2 = new PdfUnitConvertor();
                  if (!this.m_cffGlyphs.RenderedPath.ContainsKey(str4))
                  {
                    this.pathGeom = (GraphicsPath) glyphWriter.glyphParser(str4, index8, this.UnicodeCharMapTable);
                    this.m_cffGlyphs.RenderedPath.Add(str4, (object) this.pathGeom);
                  }
                  else
                    this.pathGeom = (GraphicsPath) this.m_cffGlyphs.RenderedPath[str4];
                }
                else
                {
                  string charCode1 = FontStructure.GetCharCode(str4);
                  if (glyphWriter.glyphs.ContainsKey(charCode1))
                  {
                    System.Drawing.Drawing2D.Matrix transform = g.Transform;
                    PdfUnitConvertor pdfUnitConvertor3 = new PdfUnitConvertor();
                    if (!this.m_cffGlyphs.RenderedPath.ContainsKey(charCode1))
                    {
                      this.pathGeom = (GraphicsPath) glyphWriter.glyphParser(charCode1, index8, this.UnicodeCharMapTable);
                      this.m_cffGlyphs.RenderedPath.Add(charCode1, (object) this.pathGeom);
                    }
                    else
                      this.pathGeom = (GraphicsPath) this.m_cffGlyphs.RenderedPath[charCode1];
                  }
                  else
                  {
                    if (this.m_cffGlyphs.DiffTable != null)
                      str4 = this.m_cffGlyphs.DiffTable[index8];
                    else if (this.UnicodeCharMapTable.ContainsKey(index8))
                      str4 = this.UnicodeCharMapTable[index8];
                    string charCode2 = FontStructure.GetCharCode(str4);
                    if (glyphWriter.glyphs.ContainsKey(charCode2))
                    {
                      System.Drawing.Drawing2D.Matrix transform = g.Transform;
                      PdfUnitConvertor pdfUnitConvertor4 = new PdfUnitConvertor();
                      if (!this.m_cffGlyphs.RenderedPath.ContainsKey(charCode2))
                      {
                        this.pathGeom = (GraphicsPath) glyphWriter.glyphParser(charCode2, index8, this.UnicodeCharMapTable);
                        this.m_cffGlyphs.RenderedPath.Add(charCode2, (object) this.pathGeom);
                      }
                      else
                        this.pathGeom = (GraphicsPath) this.m_cffGlyphs.RenderedPath[charCode2];
                    }
                  }
                }
              }
              else
              {
                string charCode = FontStructure.GetCharCode(str4);
                if (this.ReverseMapTable.ContainsKey(str4) && this.FontEncoding != "Identity-H")
                {
                  string str5 = this.ReverseMapTable[str4].ToString();
                  if (glyphWriter.glyphs.ContainsKey(str5))
                  {
                    System.Drawing.Drawing2D.Matrix transform = g.Transform;
                    PdfUnitConvertor pdfUnitConvertor5 = new PdfUnitConvertor();
                    if (!this.m_cffGlyphs.RenderedPath.ContainsKey(str5))
                    {
                      this.pathGeom = (GraphicsPath) glyphWriter.glyphParser(str5, index8, this.UnicodeCharMapTable);
                      this.m_cffGlyphs.RenderedPath.Add(str5, (object) this.pathGeom);
                    }
                    else
                      this.pathGeom = (GraphicsPath) this.m_cffGlyphs.RenderedPath[str5];
                  }
                }
                else
                {
                  string str6 = index8.ToString();
                  if (glyphWriter.glyphs.ContainsKey(charCode))
                  {
                    System.Drawing.Drawing2D.Matrix transform = g.Transform;
                    PdfUnitConvertor pdfUnitConvertor6 = new PdfUnitConvertor();
                    if (!this.m_cffGlyphs.RenderedPath.ContainsKey(charCode))
                    {
                      this.pathGeom = (GraphicsPath) glyphWriter.glyphParser(charCode, index8, this.UnicodeCharMapTable);
                      this.m_cffGlyphs.RenderedPath.Add(charCode, (object) this.pathGeom);
                    }
                    else
                      this.pathGeom = (GraphicsPath) this.m_cffGlyphs.RenderedPath[charCode];
                  }
                  else if (glyphWriter.glyphs.ContainsKey(str6))
                  {
                    System.Drawing.Drawing2D.Matrix transform = g.Transform;
                    PdfUnitConvertor pdfUnitConvertor7 = new PdfUnitConvertor();
                    if (!this.m_cffGlyphs.RenderedPath.ContainsKey(str6))
                    {
                      this.pathGeom = (GraphicsPath) glyphWriter.glyphParser(str6, index8, this.UnicodeCharMapTable);
                      this.m_cffGlyphs.RenderedPath.Add(str6, (object) this.pathGeom);
                    }
                    else
                      this.pathGeom = (GraphicsPath) this.m_cffGlyphs.RenderedPath[str6];
                  }
                }
              }
              if (gWidths != null)
              {
                if (!differenceTable.ContainsValue(str4) && index8 == 0)
                {
                  foreach (KeyValuePair<double, string> keyValuePair in this.CharacterMapTable)
                  {
                    if (keyValuePair.Value.Equals(str4))
                      index8 = (int) keyValuePair.Key;
                  }
                }
                if (gWidths.ContainsKey(index8))
                {
                  this.currentGlyphWidth = (float) gWidths[index8];
                  this.currentGlyphWidth *= this.CharSizeMultiplier;
                }
                else if (this.OctDecMapTable != null && this.OctDecMapTable.Count != 0)
                {
                  int key = this.OctDecMapTable[index8];
                  if (gWidths.ContainsKey(key))
                  {
                    this.currentGlyphWidth = (float) gWidths[key];
                    this.currentGlyphWidth *= this.CharSizeMultiplier;
                  }
                  else
                  {
                    this.currentGlyphWidth = this.DefaultGlyphWidth;
                    this.currentGlyphWidth *= this.CharSizeMultiplier;
                  }
                }
                else if (this.CharacterMapTable.Count != 0)
                {
                  foreach (KeyValuePair<double, string> keyValuePair in this.CharacterMapTable)
                  {
                    if (keyValuePair.Value.Equals(str4))
                      index8 = (int) keyValuePair.Key;
                  }
                  if (gWidths.ContainsKey(index8))
                  {
                    this.currentGlyphWidth = (float) gWidths[index8];
                    this.currentGlyphWidth *= this.CharSizeMultiplier;
                  }
                  else
                  {
                    this.currentGlyphWidth = this.DefaultGlyphWidth;
                    this.currentGlyphWidth *= this.CharSizeMultiplier;
                  }
                }
                else
                {
                  this.currentGlyphWidth = this.DefaultGlyphWidth;
                  this.currentGlyphWidth *= this.CharSizeMultiplier;
                }
              }
              else
              {
                this.currentGlyphWidth = this.DefaultGlyphWidth;
                this.currentGlyphWidth *= this.CharSizeMultiplier;
              }
            }
            catch
            {
              SizeF sizeF = g.MeasureString(ch1.ToString(), this.textFont, PointF.Empty, stringFormat);
              float num6 = sizeF.Width / 100f * this.TextScaling;
              if (this.FontGlyphWidths != null)
              {
                if (this.FontGlyphWidths.ContainsKey((int) ch1))
                {
                  this.currentGlyphWidth = (float) this.FontGlyphWidths[(int) ch1];
                  this.currentGlyphWidth *= this.CharSizeMultiplier;
                }
              }
              try
              {
                if ((byte) ch1 > (byte) 126 && this.m_fontEncoding == "MacRomanEncoding")
                {
                  long num7 = this.MacRomanToUnicode[(int) (byte) ch1 - 128 /*0x80*/];
                  if (this.isNegativeFont)
                  {
                    GraphicsState gstate = g.Save();
                    g.MultiplyTransform(new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, -1f, 0.0f, (float) (2.0 * (double) newLocation.Y + 2.0 * (double) sizeF.Height)));
                    flag = true;
                    this.DrawSystemFontGlyphShape(ch1, g, out txtMatrix);
                    g.Restore(gstate);
                  }
                  else
                  {
                    flag = true;
                    this.DrawSystemFontGlyphShape(ch1, g, out txtMatrix);
                  }
                }
                else
                {
                  if (this.isNegativeFont)
                  {
                    GraphicsState gstate = g.Save();
                    g.MultiplyTransform(new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, -1f, 0.0f, (float) (2.0 * (double) newLocation.Y + 2.0 * (double) sizeF.Height)));
                    flag = true;
                    this.DrawSystemFontGlyphShape(ch1, g, out txtMatrix);
                    g.Restore(gstate);
                  }
                  else if (ch1 > '\u007F' && ch1 <= 'ÿ' && this.m_fontEncoding == "WinAnsiEncoding")
                  {
                    Encoding.Default.GetString(new byte[1]
                    {
                      (byte) ch1
                    });
                    flag = true;
                    this.DrawSystemFontGlyphShape(ch1, g, out txtMatrix);
                  }
                  else
                  {
                    flag = true;
                    this.DrawSystemFontGlyphShape(ch1, g, out txtMatrix);
                  }
                  this.renderedText += ch1.ToString();
                }
              }
              catch (Exception ex)
              {
                this.exceptions.Exceptions.Append($"\r\nCharacter not rendered {ch1.ToString()}\r\n{ex.StackTrace}");
                continue;
              }
              if (num4 < str4.Length)
                newLocation.X += num6 + this.CharacterSpacing;
              else
                newLocation.X += num6;
            }
            if (!flag)
            {
              if (str3 == string.Empty)
                this.DrawGlyphs(this.pathGeom, this.currentGlyphWidth, g, out txtMatrix, ch1.ToString());
              else
                this.DrawGlyphs(this.pathGeom, this.currentGlyphWidth, g, out txtMatrix, str3);
            }
          }
          else
          {
            SizeF sizeF = g.MeasureString(ch1.ToString(), this.textFont, PointF.Empty, stringFormat);
            if ((double) (sizeF.Width / 100f * this.TextScaling) == 0.0)
            {
              if (ch1 == ' ')
                this.currentGlyphWidth = g.MeasureString(" ", this.textFont, PointF.Empty, new StringFormat(StringFormat.GenericTypographic)
                {
                  FormatFlags = StringFormatFlags.MeasureTrailingSpaces
                }).Width;
            }
            try
            {
              if ((byte) ch1 > (byte) 126 && this.m_fontEncoding == "MacRomanEncoding" && !this.Isembeddedfont)
              {
                long num8 = this.MacRomanToUnicode[(int) (byte) ch1 - 128 /*0x80*/];
                if (this.isNegativeFont)
                {
                  GraphicsState gstate = g.Save();
                  g.MultiplyTransform(new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, -1f, 0.0f, (float) (2.0 * (double) newLocation.Y + 2.0 * (double) sizeF.Height)));
                  flag = true;
                  this.DrawSystemFontGlyphShape(ch1, g, out txtMatrix);
                  g.Restore(gstate);
                }
                else
                {
                  flag = true;
                  this.DrawSystemFontGlyphShape(ch1, g, out txtMatrix);
                }
              }
              else if (this.isNegativeFont)
              {
                GraphicsState gstate = g.Save();
                g.MultiplyTransform(new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, -1f, 0.0f, (float) (2.0 * (double) newLocation.Y + 2.0 * (double) sizeF.Height)));
                flag = true;
                this.DrawSystemFontGlyphShape(ch1, g, out txtMatrix);
                g.Restore(gstate);
              }
              else if (ch1 > '\u007F' && ch1 <= 'ÿ' && this.m_fontEncoding == "WinAnsiEncoding" && !this.Isembeddedfont && !this.m_isMpdfFont)
              {
                Encoding.Default.GetString(new byte[1]
                {
                  (byte) ch1
                });
                flag = true;
                this.DrawSystemFontGlyphShape(ch1, g, out txtMatrix);
              }
              else if (this.FontEncoding != "Identity-H" && this.structure.fontType.Value == "TrueType" && this.structure.GlyphFontFile2 != null)
              {
                if (this.OctDecMapTable != null && this.OctDecMapTable.ContainsKey((int) ch1))
                  ch1 = (char) this.OctDecMapTable[(int) ch1];
                if (this.structure.FontEncoding == "WinAnsiEncoding" && this.structure.m_winansiMapTable.ContainsKey((int) ch1))
                  ch1 = this.structure.m_winansiMapTable[(int) ch1][0];
                this.pathGeom = this.structure.GetGlyph(ch1);
              }
              else if (this.structure.GlyphFontFile2 != null)
              {
                this.pathGeom = this.structure.GlyphFontFile2.GetCIDGlyphs(ch1, this.ReverseMapTable, this.CidToGidReverseMapTable);
              }
              else
              {
                flag = true;
                if (!this.DrawSystemFontGlyphShape(ch1, g, out txtMatrix))
                  this.DrawSystemFontGlyph(ch1.ToString(), this.textFont, new Pen(this.PathBrush).Brush, newLocation, stringFormat, g, out newLocation);
                txtMatrix = this.textLineMatrix;
              }
              if (this.CharacterMapTable.ContainsKey((double) ch1) && this.CharacterMapTable.Count > 0)
              {
                string str7 = this.CharacterMapTable[(double) ch1];
                char[] chArray = new char[str7.Length];
                char ch2 = str7.ToCharArray()[0];
                if (this.FontGlyphWidths != null)
                {
                  if (this.structure.fontType.Value == "Type0")
                  {
                    if (this.CidToGidReverseMapTable != null && this.CidToGidReverseMapTable.ContainsKey((int) ch1) && !this.structure.IsMappingDone)
                    {
                      if (this.FontGlyphWidths.ContainsKey(this.CidToGidReverseMapTable[(int) ch1]))
                      {
                        this.currentGlyphWidth = (float) this.FontGlyphWidths[this.CidToGidReverseMapTable[(int) ch1]];
                        this.currentGlyphWidth *= this.CharSizeMultiplier;
                      }
                      else
                      {
                        this.currentGlyphWidth = this.DefaultGlyphWidth;
                        this.currentGlyphWidth *= this.CharSizeMultiplier;
                      }
                    }
                    else if (this.FontGlyphWidths.ContainsKey((int) ch1))
                    {
                      this.currentGlyphWidth = (float) this.FontGlyphWidths[(int) ch1];
                      this.currentGlyphWidth *= this.CharSizeMultiplier;
                    }
                    else if (this.ReverseMapTable.ContainsKey(ch2.ToString()))
                    {
                      if (this.FontGlyphWidths.ContainsKey((int) this.ReverseMapTable[ch2.ToString()]))
                      {
                        this.currentGlyphWidth = g.MeasureString(ch2.ToString(), this.textFont, PointF.Empty, stringFormat).Width / 100f * this.TextScaling / this.textFont.Size;
                      }
                      else
                      {
                        this.currentGlyphWidth = this.DefaultGlyphWidth;
                        this.currentGlyphWidth *= this.CharSizeMultiplier;
                      }
                    }
                    else
                    {
                      this.currentGlyphWidth = this.DefaultGlyphWidth;
                      this.currentGlyphWidth *= this.CharSizeMultiplier;
                    }
                  }
                  else if (this.structure.fontType.Value == "TrueType")
                  {
                    if (this.FontGlyphWidths.ContainsKey((int) ch1))
                    {
                      this.currentGlyphWidth = (float) this.FontGlyphWidths[(int) ch1];
                      this.currentGlyphWidth *= this.CharSizeMultiplier;
                    }
                  }
                }
                else if (this.FontGlyphWidths == null)
                {
                  this.currentGlyphWidth = this.DefaultGlyphWidth;
                  this.currentGlyphWidth *= this.CharSizeMultiplier;
                }
              }
              else if (this.structure.CidToGidReverseMapTable.ContainsKey((int) ch1))
              {
                int key = this.structure.CidToGidReverseMapTable[(int) ch1];
                if (this.FontGlyphWidths.ContainsKey(key))
                {
                  this.currentGlyphWidth = (float) this.FontGlyphWidths[key];
                  this.currentGlyphWidth *= this.CharSizeMultiplier;
                }
              }
              else if (this.FontGlyphWidths != null && this.FontGlyphWidths.Count > 0 && this.FontGlyphWidths.ContainsKey((int) ch1))
              {
                this.currentGlyphWidth = (float) this.FontGlyphWidths[(int) ch1];
                this.currentGlyphWidth *= this.CharSizeMultiplier;
              }
              else
                this.currentGlyphWidth = this.DefaultGlyphWidth * this.CharSizeMultiplier;
            }
            catch (Exception ex)
            {
              this.exceptions.Exceptions.Append($"\r\nCharacter not rendered {ch1.ToString()}\r\n{ex.StackTrace}");
              continue;
            }
            if (num4 < ch1.ToString().Length)
              newLocation.X += this.CharacterSpacing;
            if (!flag)
              this.DrawGlyphs(this.pathGeom, this.currentGlyphWidth, g, out txtMatrix, ch1.ToString());
          }
        }
        txtMatrix = this.textLineMatrix;
      }
      return newLocation.X - x;
    }
    finally
    {
      Monitor.Exit(this.fontResourceLocker);
    }
  }

  private string ResolveFontName(string matrixImplFontName)
  {
    if (matrixImplFontName.Contains("times") || matrixImplFontName.Contains("Times"))
      return "Times New Roman";
    return matrixImplFontName.Contains("Helvetica") ? "Helvetica" : matrixImplFontName;
  }

  internal bool IsNonsymbolic => this.GetFlag((byte) 6);

  private bool GetFlag(byte bit)
  {
    --bit;
    return this.GetBit(this.FontFlag, bit);
  }

  public bool GetBit(int n, byte bit) => (n & 1 << (int) bit) != 0;

  private bool DrawSystemFontGlyphShape(char letter, System.Drawing.Graphics g, out Matrix temptextmatrix)
  {
    GraphicsUnit pageUnit = g.PageUnit;
    System.Drawing.Drawing2D.Matrix transform = g.Transform;
    g.PageUnit = GraphicsUnit.Pixel;
    if (this.SubstitutedFontsList != null && this.SubstitutedFontsList.Count > 0)
    {
      string key = this.m_fontName;
      if (this.m_fontStyle.ToString() != "Regular")
        key = $"{key} {(object) this.m_fontStyle}";
      if (this.SubstitutedFontsList.ContainsKey(key))
      {
        string str = this.SubstitutedFontsList[key];
        if (str.Contains("Bold"))
        {
          str = str.Replace("Bold", "");
          this.m_fontStyle = FontStyle.Bold;
        }
        else if (str.Contains("Italic"))
        {
          str = str.Replace("Italic", "");
          this.m_fontStyle = FontStyle.Italic;
        }
        else
          this.m_fontStyle = FontStyle.Regular;
        this.m_fontName = str.Trim();
      }
    }
    SystemFontFontDescriptor fontFontDescriptor = new SystemFontFontDescriptor(this.m_fontName, this.m_fontStyle);
    if (!this.structure.IsOpenTypeFont)
    {
      if (this.testdict.ContainsKey(fontFontDescriptor))
      {
        this.openTypeFontSource = this.testdict[fontFontDescriptor];
      }
      else
      {
        this.openTypeFontSource = this.SystemFontsManager.GetFontSource(fontFontDescriptor) as SystemFontOpenTypeFontSource;
        this.testdict.Add(fontFontDescriptor, this.openTypeFontSource);
      }
    }
    else if (this.testdict.ContainsKey(fontFontDescriptor))
    {
      this.openTypeFontSource = this.testdict[fontFontDescriptor];
    }
    else
    {
      this.openTypeFontSource = new SystemFontOpenTypeFontSource(new SystemFontOpenTypeFontReader(this.structure.FontStream.ToArray()));
      this.testdict.Add(fontFontDescriptor, this.openTypeFontSource);
    }
    SystemFontGlyph glyph1 = new SystemFontGlyph();
    g.Transform = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
    glyph1.Name = letter.ToString();
    glyph1.FontSize = (double) this.m_fontSize;
    if (this.OctDecMapTable != null && this.OctDecMapTable.ContainsKey((int) letter))
      letter = (char) this.OctDecMapTable[(int) letter];
    else if (this.structure.FontEncoding == "WinAnsiEncoding" && this.structure.m_winansiMapTable != null && this.structure.m_winansiMapTable.ContainsKey((int) letter) && this.structure.CharacterMapTable.ContainsKey((double) letter))
      letter = this.structure.m_winansiMapTable[(int) letter].ToCharArray()[0];
    else if (this.CidToGidReverseMapTable != null && this.CidToGidReverseMapTable.Count > 0 && this.CidToGidReverseMapTable.ContainsKey((int) letter))
      letter = (char) this.CidToGidReverseMapTable[(int) letter];
    byte b = (byte) letter;
    string mapChar = letter.ToString();
    glyph1.CharId = new CharCode(b);
    glyph1.CharSpacing = (double) this.m_characterSpacing;
    glyph1.FontStyle = this.m_fontStyle;
    glyph1.HorizontalScaling = (double) this.TextHorizontalScaling;
    string str1 = letter.ToString();
    ushort glyphId = this.GetGlyphID(letter.ToString());
    if (!this.structure.IsMappingDone)
    {
      if (this.CidToGidReverseMapTable != null && this.CidToGidReverseMapTable.ContainsKey((int) Convert.ToChar(str1)) && this.structure.CharacterMapTable != null && this.structure.CharacterMapTable.Count > 0)
        str1 = this.CharacterMapTable[(double) this.CidToGidReverseMapTable[(int) Convert.ToChar(str1)]];
      else if (this.structure.CharacterMapTable != null && this.structure.CharacterMapTable.Count > 0)
        str1 = this.structure.MapCharactersFromTable(str1);
      else if (this.structure.DifferencesDictionary != null && this.structure.DifferencesDictionary.Count > 0)
        str1 = this.structure.MapDifferences(str1);
    }
    glyph1.GlyphId = glyphId;
    Glyph glyph2 = new Glyph();
    glyph1.Stroke = this.PathBrush;
    glyph1.NonStroke = this.PathNonStrokeBrush;
    glyph2.Stroke = this.PathBrush;
    glyph2.FontStyle = this.m_fontStyle;
    glyph2.HorizontalScaling = (double) this.TextHorizontalScaling;
    glyph2.CharSpacing = (double) this.CharacterSpacing;
    glyph2.FontSize = glyph1.FontSize;
    glyph2.Name = glyph1.Name;
    glyph2.FontFamily = this.m_fontName;
    glyph2.CharId = glyph1.CharId;
    glyph2.TransformMatrix = this.GetTextRenderingMatrix();
    glyph1.TransformMatrix = new SystemFontMatrix(glyph2.TransformMatrix.M11, glyph2.TransformMatrix.M12, glyph2.TransformMatrix.M21, glyph2.TransformMatrix.M22, glyph2.TransformMatrix.OffsetX, glyph2.TransformMatrix.OffsetY);
    this.openTypeFontSource.GetGlyphOutlines(glyph1, 100.0);
    glyph1.Width = this.GetSystemFontGlyphWidth(glyph2, g);
    if (glyph1.Width == -1.0)
    {
      this.openTypeFontSource.GetAdvancedWidth(glyph1);
      glyph1.Width = glyph1.AdvancedWidth;
    }
    if (this.FontGlyphWidths != null && this.FontGlyphWidths.Count > 0 && this.FontGlyphWidths.ContainsKey((int) letter))
    {
      if (this.ReverseMapTable != null && this.ReverseMapTable.ContainsKey(letter.ToString()))
      {
        double key = this.ReverseMapTable[letter.ToString()];
        glyph1.Width = (double) this.FontGlyphWidths[(int) key];
        glyph1.Width *= (double) this.CharSizeMultiplier;
      }
      else
      {
        glyph1.Width = (double) this.FontGlyphWidths[(int) letter];
        glyph1.Width *= (double) this.CharSizeMultiplier;
      }
    }
    if (glyph1.AdvancedWidth > 0.0 && this.IsCID && !this.IsType1Font && this.structure.IsAdobeIdentity)
      glyph1.Width = glyph1.AdvancedWidth;
    glyph2.CharSpacing = (double) this.CharacterSpacing;
    if (letter.ToString() == " " && (this.CharID.BytesCount == 1 && this.CharID.Bytes[0] == (byte) 32 /*0x20*/ || this.CharID.IsEmpty))
      glyph2.WordSpacing = (double) this.WordSpacing;
    if (glyph1.Width != -1.0)
    {
      glyph2.Width = glyph1.Width;
      Matrix identity = Matrix.Identity;
      identity.Scale(0.01, 0.01, 0.0, 0.0);
      identity.Translate(0.0, 1.0);
      this.transformations.PushTransform(identity * glyph2.TransformMatrix);
      System.Drawing.Drawing2D.Matrix matrix = g.Transform.Clone();
      matrix.Multiply(this.GetTransformationMatrix(this.transformations.CurrentTransform));
      g.Transform = matrix;
      g.SmoothingMode = SmoothingMode.AntiAlias;
      if (this.structure.ReverseDictMapping.ContainsKey(glyph1.Name))
      {
        byte index = (byte) (float) this.structure.ReverseDictMapping[glyph1.Name];
        string[] fontEncodingNames = this.GetStandardFontEncodingNames();
        glyph1.Name = fontEncodingNames[(int) index];
      }
      if (!this.m_isMpdfFont && !this.IsPdfium)
        this.DrawPath(g, glyph1, glyph1.Name);
      float num = glyph2.TransformMatrix.M11 <= 0.0 ? (glyph2.TransformMatrix.M12 == 0.0 || glyph2.TransformMatrix.M21 == 0.0 ? (float) glyph2.FontSize : (glyph2.TransformMatrix.M12 >= 0.0 ? (float) glyph2.TransformMatrix.M12 : (float) -glyph2.TransformMatrix.M12)) : (float) glyph2.TransformMatrix.M11;
      glyph2.ToUnicode = str1;
      PdfUnitConvertor pdfUnitConvertor = new PdfUnitConvertor();
      if ((double) this.pageRotation == 90.0 || (double) this.pageRotation == 270.0)
      {
        if ((double) matrix.Elements[1] == 0.0 && (double) matrix.Elements[2] == 0.0)
        {
          glyph2.IsRotated = false;
          glyph2.BoundingRect = new Rect(new Point((double) pdfUnitConvertor.ConvertFromPixels(matrix.OffsetX, PdfGraphicsUnit.Point) / (double) this.zoomFactor, ((double) pdfUnitConvertor.ConvertFromPixels(matrix.OffsetY, PdfGraphicsUnit.Point) - (double) pdfUnitConvertor.ConvertFromPixels((float) ((double) num * (double) this.zoomFactor * ((double) g.DpiY / 96.0)), PdfGraphicsUnit.Point)) / (double) this.zoomFactor), new Size(glyph2.Width * (double) num, (double) num));
        }
        else
        {
          glyph2.IsRotated = true;
          glyph2.BoundingRect = !this.IsFindText || (double) this.pageRotation != 90.0 ? new Rect(new Point((double) pdfUnitConvertor.ConvertFromPixels(matrix.OffsetX + (num + (float) (glyph2.Ascent / 1000.0)) * matrix.Elements[2], PdfGraphicsUnit.Point) / (double) this.zoomFactor, (double) pdfUnitConvertor.ConvertFromPixels(matrix.OffsetY + num * matrix.Elements[2], PdfGraphicsUnit.Point) / (double) this.zoomFactor), new Size((double) num, glyph2.Width * (double) num)) : new Rect(new Point((double) pdfUnitConvertor.ConvertFromPixels(matrix.OffsetY, PdfGraphicsUnit.Point) / (double) this.zoomFactor, ((double) pdfUnitConvertor.ConvertFromPixels(matrix.OffsetX, PdfGraphicsUnit.Point) - (double) pdfUnitConvertor.ConvertFromPixels((float) ((double) num * (double) this.zoomFactor * ((double) g.DpiY / 96.0)), PdfGraphicsUnit.Point)) / (double) this.zoomFactor), new Size(glyph2.Width * (double) num, (double) num));
        }
      }
      else
      {
        if ((double) this.pageRotation == 180.0)
          glyph2.IsRotated = (double) matrix.Elements[1] == 0.0 && (double) matrix.Elements[2] == 0.0;
        glyph2.BoundingRect = new Rect(new Point((double) pdfUnitConvertor.ConvertFromPixels(matrix.OffsetX, PdfGraphicsUnit.Point) / (double) this.zoomFactor, (double) pdfUnitConvertor.ConvertFromPixels(matrix.OffsetY - (float) ((double) num * (double) this.zoomFactor * ((double) g.DpiY / 96.0)), PdfGraphicsUnit.Point) / (double) this.zoomFactor), new Size(glyph2.Width * (double) num, (double) num));
      }
      if (this.structure.IsAdobeJapanFont)
      {
        if (this.structure.AdobeJapanCidMapTable.ContainsKey((int) Convert.ToChar(mapChar)))
          mapChar = this.structure.AdobeJapanCidMapTableGlyphParser(mapChar);
        glyph2.ToUnicode = mapChar;
      }
      if (glyph2.ToUnicode.Length != 1)
      {
        this.textElementGlyphList.Add(glyph2);
        for (int index = 0; index < glyph2.ToUnicode.Length - 1; ++index)
          this.textElementGlyphList.Add(new Glyph());
      }
      else
        this.textElementGlyphList.Add(glyph2);
      this.UpdateTextMatrix(glyph2);
      this.transformations.PopTransform();
      this.renderedText += str1;
      g.Transform = transform;
      g.PageUnit = pageUnit;
      temptextmatrix = this.textLineMatrix;
      return !this.m_isMpdfFont;
    }
    temptextmatrix = this.textLineMatrix;
    return false;
  }

  private int GetInt(byte[] val)
  {
    int num = 0;
    int length = val.Length;
    for (int index = 0; index < length; ++index)
    {
      num |= length <= index ? 0 : (int) val[index] & (int) byte.MaxValue;
      if (index < length - 1)
        num <<= 8;
    }
    return num;
  }

  internal float RenderWithSpace(
    System.Drawing.Graphics g,
    PointF currentLocation,
    List<string> decodedList,
    List<float> characterSpacings,
    double textScaling,
    Dictionary<int, int> gWidths,
    double type1Height,
    Dictionary<int, string> differenceTable,
    Dictionary<string, string> differenceMappedTable,
    Dictionary<int, string> differenceEncoding,
    out Matrix textmatrix)
  {
    textmatrix = Matrix.Identity;
    string fontName1 = this.FontName;
    this.renderedText = string.Empty;
    string fontName2 = this.FontName;
    this.textFont = (Font) null;
    float num1 = 0.0f;
    float x = currentLocation.X;
    int index1 = 0;
    string[] strArray = (string[]) null;
    PdfUnitConvertor pdfUnitConvertor1 = new PdfUnitConvertor();
    this.m_isMpdfFont = this.isMpdfaaFonts();
    if (this.Font != null && this.Isembeddedfont)
    {
      this.BackupEmbededFontName(fontName1);
      this.FontName = TextElement.CheckFontName(this.Font.Name);
      this.textFont = this.Font;
    }
    else
    {
      this.CheckFontStyle(this.FontName);
      this.FontName = TextElement.CheckFontName(this.FontName);
      this.textFont = (double) this.FontSize >= 0.0 ? new Font(this.FontName, this.FontSize, this.FontStyle) : new Font(this.FontName, -this.FontSize, this.FontStyle);
    }
    if (this.ZapfPostScript != null)
      strArray = this.ZapfPostScript.Split(new char[1]
      {
        ' '
      }, StringSplitOptions.RemoveEmptyEntries);
    g.MeasureString(this.Text, this.textFont);
    PointF newLocation = currentLocation;
    float fontSize1 = this.FontSize;
    double y = (double) currentLocation.Y;
    bool flag1 = false;
    string str1 = this.ResolveFontName(fontName1);
    foreach (string decoded in decodedList)
    {
      bool flag2 = false;
      double result;
      if (StdFontsAssistant.IsStandardFontName(str1) && !this.Isembeddedfont)
      {
        if (this.m_fontStyle == FontStyle.Bold && !str1.Contains("Bold"))
          str1 = !StdFontsAssistant.IsAlternativeStdFontAvailable(str1) ? str1 + "-Bold" : str1 + ",Bold";
        if (this.m_fontStyle == FontStyle.Italic && !str1.Contains("Italic"))
          str1 = !StdFontsAssistant.IsAlternativeStdFontAvailable(str1) ? (str1.Contains("Courier") || str1.Contains("Helvetica") ? str1 + "-Oblique" : str1 + "-Italic") : str1 + ",Italic";
        if (this.m_fontStyle == (FontStyle.Bold | FontStyle.Italic) && !str1.Contains("Italic") && !str1.Contains("Bold"))
          str1 = !StdFontsAssistant.IsAlternativeStdFontAvailable(str1) ? (str1.Contains("Courier") || str1.Contains("Helvetica") ? str1 + "-Bold" + "Oblique" : str1 + "-Bold" + "Italic") : str1 + ",Bold" + "Italic";
        GraphicsUnit pageUnit = g.PageUnit;
        System.Drawing.Drawing2D.Matrix transform = g.Transform;
        g.PageUnit = GraphicsUnit.Pixel;
        if (double.TryParse(decoded, out result))
        {
          this.UpdateTextMatrix(result);
        }
        else
        {
          if (!TextElement.fontSourceCache.ContainsKey(this.FontID + this.structure.FontRefNumber))
          {
            this.FontSource = (FontSource) TextElement.manager.GetStandardFontSource(str1);
            TextElement.fontSourceCache.TryAdd(this.FontID + this.structure.FontRefNumber, this.FontSource);
          }
          else
            this.FontSource = TextElement.fontSourceCache[this.FontID + this.structure.FontRefNumber];
          string str2 = decoded.Remove(decoded.Length - 1, 1);
          flag1 = false;
          foreach (char index2 in str2)
          {
            flag1 = false;
            g.Transform = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
            Glyph glyph1 = new Glyph();
            glyph1.FontSize = (double) this.FontSize;
            glyph1.FontFamily = this.FontName;
            glyph1.FontStyle = this.FontStyle;
            glyph1.Stroke = this.PathBrush;
            glyph1.NonStroke = this.PathNonStrokeBrush;
            glyph1.TransformMatrix = this.GetTextRenderingMatrix();
            glyph1.Name = index2.ToString();
            glyph1.HorizontalScaling = (double) this.TextHorizontalScaling;
            glyph1.CharId = new CharCode((int) index2);
            glyph1.CharSpacing = (double) this.CharacterSpacing;
            string[] fontEncodingNames = this.GetStandardFontEncodingNames();
            byte[] bytes = Encoding.UTF8.GetBytes(index2.ToString());
            if (this.structure.ReverseDictMapping.ContainsKey(index2.ToString()))
            {
              float num2 = (float) this.structure.ReverseDictMapping[index2.ToString()];
              if (this.structure.DifferencesDictionary.ContainsKey(num2.ToString()))
              {
                glyph1.Name = FontStructure.GetCharCode(this.structure.DifferencesDictionary[num2.ToString()]);
              }
              else
              {
                bytes[0] = (byte) num2;
                glyph1.Name = fontEncodingNames[(int) bytes[0]];
              }
            }
            else if (this.OctDecMapTable != null && this.OctDecMapTable.ContainsKey((int) index2))
            {
              char index3 = (char) this.OctDecMapTable[(int) index2];
              glyph1.Name = fontEncodingNames[(int) index3];
            }
            else
            {
              if (this.structure.DifferencesDictionary != null)
              {
                Dictionary<string, string> differencesDictionary1 = this.structure.DifferencesDictionary;
                int num3 = (int) index2;
                string key1 = num3.ToString();
                if (differencesDictionary1.ContainsKey(key1) && this.structure.BaseFontEncoding != "WinAnsiEncoding")
                {
                  Glyph glyph2 = glyph1;
                  Dictionary<string, string> differencesDictionary2 = this.structure.DifferencesDictionary;
                  num3 = (int) index2;
                  string key2 = num3.ToString();
                  string charCode = FontStructure.GetCharCode(differencesDictionary2[key2]);
                  glyph2.Name = charCode;
                  goto label_37;
                }
              }
              if (fontEncodingNames.Length > (int) index2 && this.structure.FontName != "ZapfDingbats")
              {
                if (this.FontName == "Symbol")
                  glyph1.Name = FontStructure.GetCharCode(index2.ToString());
                else
                  glyph1.Name = fontEncodingNames[(int) index2];
              }
              else if (this.structure.FontName == "ZapfDingbats")
              {
                if (index1 < this.ZapfPostScript.Length)
                {
                  glyph1.Name = strArray[index1].Trim();
                  ++index1;
                }
              }
              else
                glyph1.Name = fontEncodingNames[(int) bytes[0]];
            }
label_37:
            glyph1.Width = this.GetGlyphWidth(glyph1);
            this.FontSource.GetGlyphOutlines(glyph1, 100.0);
            GraphicsPath path = new PdfElementsRenderer().RenderGlyph(glyph1);
            Matrix identity = Matrix.Identity;
            identity.Scale(0.01, 0.01, 0.0, 0.0);
            identity.Translate(0.0, 1.0);
            this.transformations.PushTransform(identity * glyph1.TransformMatrix);
            System.Drawing.Drawing2D.Matrix matrix = g.Transform.Clone();
            matrix.Multiply(this.GetTransformationMatrix(this.transformations.CurrentTransform));
            g.Transform = matrix;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.FillPath(glyph1.Stroke, path);
            glyph1.ToUnicode = index2.ToString();
            float num4 = glyph1.TransformMatrix.M11 <= 0.0 ? (glyph1.TransformMatrix.M12 == 0.0 || glyph1.TransformMatrix.M21 == 0.0 ? (float) glyph1.FontSize : (glyph1.TransformMatrix.M12 >= 0.0 ? (float) glyph1.TransformMatrix.M12 : (float) -glyph1.TransformMatrix.M12)) : (float) glyph1.TransformMatrix.M11;
            if ((double) this.pageRotation == 90.0 || (double) this.pageRotation == 270.0)
            {
              if ((double) matrix.Elements[1] == 0.0 && (double) matrix.Elements[2] == 0.0)
              {
                glyph1.IsRotated = false;
                glyph1.BoundingRect = new Rect(new Point((double) pdfUnitConvertor1.ConvertFromPixels(matrix.OffsetX, PdfGraphicsUnit.Point) / (double) this.zoomFactor, ((double) pdfUnitConvertor1.ConvertFromPixels(matrix.OffsetY, PdfGraphicsUnit.Point) - (double) pdfUnitConvertor1.ConvertFromPixels((float) ((double) num4 * (double) this.zoomFactor * ((double) g.DpiY / 96.0)), PdfGraphicsUnit.Point)) / (double) this.zoomFactor), new Size(glyph1.Width * (double) num4, (double) num4));
              }
              else
              {
                glyph1.IsRotated = true;
                glyph1.BoundingRect = !this.IsFindText || (double) this.pageRotation != 90.0 ? new Rect(new Point((double) pdfUnitConvertor1.ConvertFromPixels(matrix.OffsetX + (num4 + (float) (glyph1.Ascent / 1000.0)) * matrix.Elements[2], PdfGraphicsUnit.Point) / (double) this.zoomFactor, ((double) pdfUnitConvertor1.ConvertFromPixels(matrix.OffsetY - num4 * matrix.Elements[2], PdfGraphicsUnit.Point) - (double) pdfUnitConvertor1.ConvertFromPixels((float) ((double) num4 * (double) this.zoomFactor * ((double) g.DpiY / 96.0)), PdfGraphicsUnit.Point)) / (double) this.zoomFactor), new Size((double) num4 - glyph1.Ascent / 1000.0, glyph1.Width * (double) num4)) : new Rect(new Point((double) pdfUnitConvertor1.ConvertFromPixels(matrix.OffsetY, PdfGraphicsUnit.Point) / (double) this.zoomFactor, ((double) pdfUnitConvertor1.ConvertFromPixels(matrix.OffsetX, PdfGraphicsUnit.Point) - (double) pdfUnitConvertor1.ConvertFromPixels((float) ((double) num4 * (double) this.zoomFactor * ((double) g.DpiY / 96.0)), PdfGraphicsUnit.Point)) / (double) this.zoomFactor), new Size(glyph1.Width * (double) num4, (double) num4));
              }
            }
            else
            {
              if ((double) this.pageRotation == 180.0)
                glyph1.IsRotated = (double) matrix.Elements[1] == 0.0 && (double) matrix.Elements[2] == 0.0;
              glyph1.BoundingRect = new Rect(new Point((double) pdfUnitConvertor1.ConvertFromPixels(matrix.OffsetX, PdfGraphicsUnit.Point) / (double) this.zoomFactor, (double) pdfUnitConvertor1.ConvertFromPixels(matrix.OffsetY - (float) ((double) num4 * (double) this.zoomFactor * ((double) g.DpiY / 96.0)), PdfGraphicsUnit.Point) / (double) this.zoomFactor), new Size(glyph1.Width * (double) num4, (double) num4));
            }
            if (this.structure.CharacterMapTable != null && this.structure.CharacterMapTable.ContainsKey((double) index2))
              glyph1.ToUnicode = this.structure.CharacterMapTable[(double) index2];
            if (glyph1.ToUnicode.Length != 1)
            {
              this.textElementGlyphList.Add(glyph1);
              for (int index4 = 0; index4 < glyph1.ToUnicode.Length - 1; ++index4)
                this.textElementGlyphList.Add(new Glyph());
            }
            else
              this.textElementGlyphList.Add(glyph1);
            this.GetFontSize(glyph1, num4);
            this.UpdateTextMatrix(glyph1);
            this.transformations.PopTransform();
            if (this.structure.CharacterMapTable != null && this.structure.CharacterMapTable.ContainsKey((double) index2))
              this.renderedText += glyph1.ToString();
            else
              this.renderedText += index2.ToString();
          }
        }
        g.Transform = transform;
        g.PageUnit = pageUnit;
        textmatrix = this.textLineMatrix;
      }
      else
      {
        StringFormat stringFormat = new StringFormat(StringFormat.GenericTypographic);
        float width = g.MeasureString(" ", this.textFont, PointF.Empty, new StringFormat(StringFormat.GenericTypographic)
        {
          FormatFlags = StringFormatFlags.MeasureTrailingSpaces
        }).Width;
        if (double.TryParse(decoded, out result))
        {
          this.UpdateTextMatrix(result);
          float sizeInPoints = this.textFont.SizeInPoints;
          float num5 = (float) result * (sizeInPoints / 1000f) - this.CharacterSpacing;
          newLocation.X -= num5;
          textmatrix = this.textLineMatrix;
        }
        else if (decoded[0] >= '\u0E00' && decoded[0] <= '\u0E7F')
        {
          string empty = string.Empty;
          List<char> charList = new List<char>();
          string str3 = decoded.Remove(decoded.Length - 1, 1);
          flag1 = true;
          this.DrawSystemFontGlyph(str3, this.textFont, new Pen(this.PathBrush).Brush, newLocation, stringFormat, g, out newLocation);
        }
        else
        {
          string s = decoded.Remove(decoded.Length - 1, 1);
          int num6 = 0;
          Dictionary<int, int> dictionary = new Dictionary<int, int>();
          if (this.IsCID && !this.IsType1Font)
          {
            byte[] bytes = Encoding.Unicode.GetBytes(s);
            int key = 0;
            for (int index5 = 0; index5 < bytes.Length; index5 += 2)
            {
              dictionary.Add(key, (int) bytes[index5]);
              ++key;
            }
          }
          for (int index6 = 0; index6 < s.Length; ++index6)
          {
            char ch1 = s[index6];
            string str4 = string.Empty;
            if (this.IsType1Font && !this.structure.IsOpenTypeFont && differenceTable.Count == 0)
            {
              str4 = s[index6].ToString();
              int num7 = 0;
              if (this.ReverseMapTable.ContainsKey(str4))
              {
                foreach (KeyValuePair<double, string> keyValuePair in this.CharacterMapTable)
                {
                  if (keyValuePair.Value.IndexOf(ch1) > -1)
                    ++num7;
                }
              }
              if ((!this.ReverseMapTable.ContainsKey(str4) || num7 > 1) && (char.IsLetter(ch1) || char.IsPunctuation(ch1) || char.IsSymbol(ch1)))
              {
                if (index6 != s.Length - 1)
                {
                  str4 = s.Substring(index6);
                  index6 += str4.Length - 1;
                }
              }
              if (str4 != ch1.ToString())
              {
                for (int index7 = 0; index7 < str4.Length; ++index7)
                {
                  if (!this.ReverseMapTable.ContainsKey(str4))
                  {
                    str4 = str4.Remove(str4.Length - 1);
                    --index6;
                    index7 = 0;
                    if (str4 == s[index6].ToString())
                      str4 = string.Empty;
                    if (this.ReverseMapTable.ContainsKey(str4))
                      break;
                  }
                }
              }
              else
                str4 = string.Empty;
            }
            if (this.IsCID && !this.IsType1Font)
            {
              byte[] val = new byte[2];
              int key3 = (int) s[index6];
              int key4 = -1;
              if (index6 + 1 < s.Length)
              {
                key4 = (int) s[index6 + 1];
                if (this.CidToGidReverseMapTable != null && this.CidToGidReverseMapTable.Count > 0 && this.CidToGidReverseMapTable.ContainsKey((int) s[index6 + 1]))
                  key4 = this.CidToGidReverseMapTable[(int) s[index6 + 1]];
              }
              if (s.Length > index6 && this.CidToGidReverseMapTable != null && this.CidToGidReverseMapTable.Count > 0 && this.CidToGidReverseMapTable.ContainsKey((int) s[index6]))
                key3 = this.CidToGidReverseMapTable[(int) s[index6]];
              if (this.CharacterMapTable.Count != 0 && index6 + 1 < s.Length && (!this.CharacterMapTable.ContainsKey((double) key4) || this.CharacterMapTable.ContainsKey((double) key3) && this.CharacterMapTable.ContainsKey((double) key4) && this.structure.CidToGidMap == null && !this.structure.IsHexaDecimalString && !this.structure.IsMappingDone))
              {
                val[0] = (byte) dictionary[index6];
                val[1] = (byte) dictionary[index6 + 1];
                ch1 = (char) this.GetInt(val);
                if (!this.CharacterMapTable.ContainsKey((double) ch1))
                  ch1 = s[index6];
                else
                  ++index6;
              }
              else if (this.CharacterMapTable.Count != 0 && !this.CharacterMapTable.ContainsKey((double) key3))
              {
                if (index6 + 1 < s.Length)
                {
                  val[0] = (byte) dictionary[index6];
                  val[1] = (byte) dictionary[index6 + 1];
                  ch1 = (char) this.GetInt(val);
                  if (!this.CharacterMapTable.ContainsKey((double) ch1))
                  {
                    byte[] numArray = new byte[2];
                    ch1 = (char) this.GetInt(Encoding.Unicode.GetBytes(s[index6].ToString()));
                  }
                  else
                    ++index6;
                }
                else if (!this.CharacterMapTable.ContainsKey((double) ch1))
                {
                  ch1 = (char) this.GetInt(Encoding.Unicode.GetBytes(s[index6].ToString()));
                  if (!this.CharacterMapTable.ContainsKey((double) ch1))
                    ch1 = s[index6];
                }
              }
            }
            ++num6;
            if (this.IsType1Font && !this.structure.IsOpenTypeFont)
            {
              GlyphWriter glyphWriter1 = new GlyphWriter(this.m_cffGlyphs.Glyphs, this.Is1C);
              ch1.ToString();
              string str5 = ch1.ToString();
              int num8 = (int) ch1;
              if (this.structure.DifferencesDictionary.ContainsValue(s))
              {
                str5 = s;
                index6 = s.Length - 1;
              }
              if (differenceTable.ContainsValue(s) && differenceMappedTable.ContainsValue(s))
              {
                str5 = s;
                foreach (KeyValuePair<int, string> keyValuePair in differenceTable)
                {
                  if (keyValuePair.Value == s)
                  {
                    num8 = keyValuePair.Key;
                    index6 = s.Length - 1;
                    break;
                  }
                }
              }
              else if ((this.ReverseMapTable.ContainsKey(str5) || this.ReverseMapTable.ContainsKey(str4)) && (this.ReverseMapTable.Count == differenceTable.Count || this.ReverseMapTable.Count == this.CharacterMapTable.Count))
              {
                num8 = !(str4 == string.Empty) ? (int) this.ReverseMapTable[str4] : (int) this.ReverseMapTable[str5];
                if (differenceTable.ContainsKey(num8))
                {
                  byte b = Convert.ToByte(num8);
                  str5 = differenceTable[num8];
                  this.CharID = new CharCode(b);
                }
              }
              else if (differenceMappedTable.ContainsValue(str5))
              {
                foreach (KeyValuePair<string, string> keyValuePair in differenceMappedTable)
                {
                  if (keyValuePair.Value == str5)
                  {
                    num8 = int.Parse(keyValuePair.Key);
                    if (differenceTable.ContainsKey(num8))
                    {
                      str5 = differenceTable[num8];
                      break;
                    }
                    break;
                  }
                }
              }
              else if (differenceMappedTable.ContainsKey(num8.ToString()))
                str5 = differenceMappedTable[num8.ToString()];
              else if (differenceTable.ContainsKey((int) ch1))
              {
                str5 = differenceTable[(int) ch1];
                num8 = (int) ch1;
              }
              else if (differenceMappedTable.ContainsValue(s))
              {
                using (Dictionary<string, string>.Enumerator enumerator = differenceMappedTable.GetEnumerator())
                {
                  if (enumerator.MoveNext())
                  {
                    KeyValuePair<string, string> current = enumerator.Current;
                    if (current.Value == s)
                      num8 = int.Parse(current.Key);
                    if (differenceTable.ContainsKey(num8))
                      str5 = differenceTable[num8];
                  }
                }
              }
              else if (this.CharacterMapTable.ContainsValue(str5) && this.CharacterMapTable.Count == differenceTable.Count)
              {
                foreach (KeyValuePair<double, string> keyValuePair in this.CharacterMapTable)
                {
                  if (keyValuePair.Value == str5)
                  {
                    num8 = (int) keyValuePair.Key;
                    if (differenceTable.ContainsKey(num8))
                    {
                      str5 = differenceTable[num8];
                      break;
                    }
                    break;
                  }
                }
              }
              else if (differenceTable.ContainsKey(num8))
                str5 = differenceTable[num8];
              else if (this.m_cffGlyphs.DifferenceEncoding.ContainsKey(num8) && this.structure.FontEncoding != "MacRomanEncoding")
                str5 = this.m_cffGlyphs.DifferenceEncoding[num8];
              else if (this.structure.FontEncoding == "MacRomanEncoding")
              {
                if (this.structure.m_macRomanMapTable.ContainsKey(num8))
                  str5 = this.structure.m_macRomanMapTable[num8];
              }
              else if (this.structure.FontEncoding == "WinAnsiEncoding")
              {
                if (this.structure.m_winansiMapTable.ContainsKey(num8))
                  str5 = this.structure.m_winansiMapTable[num8];
              }
              try
              {
                double num9 = 0.0;
                if (this.Is1C)
                {
                  double fontSize2 = (double) this.FontSize;
                }
                GlyphWriter glyphWriter2 = new GlyphWriter(this.m_cffGlyphs.Glyphs, this.m_cffGlyphs.GlobalBias, this.Is1C);
                glyphWriter2.is1C = this.Is1C;
                if (this.structure.BaseFontEncoding == "WinAnsiEncoding" && this.structure.FontEncoding != "Encoding")
                  glyphWriter2.HasBaseEncoding = true;
                if (this.structure.m_isContainFontfile && this.structure.FontFileType1Font.m_hasFontMatrix)
                  glyphWriter2.FontMatrix = this.structure.FontFileType1Font.m_fontMatrix;
                if (this.structure.IsContainFontfile3 && this.structure.fontFile3Type1Font.hasFontMatrix)
                  glyphWriter2.FontMatrix = this.structure.fontFile3Type1Font.FontMatrix;
                if (this.FontEncoding == "MacRomanEncoding" && ch1 > '~')
                {
                  this.GetMacEncodeTable();
                  if (this.OctDecMapTable != null && this.OctDecMapTable.ContainsKey((int) ch1))
                  {
                    num8 = this.OctDecMapTable[(int) ch1];
                    ch1 = (char) num8;
                  }
                  string decodedCharacter = this.m_macEncodeTable[(int) ch1];
                  str5 = FontStructure.GetCharCode(decodedCharacter);
                  ch1 = decodedCharacter[0];
                }
                if (!this.IsCID)
                {
                  if (glyphWriter2.glyphs.ContainsKey(str5))
                  {
                    System.Drawing.Drawing2D.Matrix transform = g.Transform;
                    if (!this.m_cffGlyphs.RenderedPath.ContainsKey(str5))
                    {
                      this.pathGeom = (GraphicsPath) glyphWriter2.glyphParser(str5, num8, this.UnicodeCharMapTable);
                      Matrix matrix = new Matrix(0.1, 0.0, 0.0, -0.1, 0.0, 0.0);
                      this.m_cffGlyphs.RenderedPath.Add(str5, (object) this.pathGeom);
                      this.m_spaceCheck = true;
                    }
                    else
                    {
                      this.pathGeom = (GraphicsPath) this.m_cffGlyphs.RenderedPath[str5];
                      this.m_spaceCheck = true;
                    }
                  }
                  else
                  {
                    string charCode = FontStructure.GetCharCode(str5);
                    if (glyphWriter2.glyphs.ContainsKey(charCode))
                    {
                      System.Drawing.Drawing2D.Matrix transform = g.Transform;
                      PdfUnitConvertor pdfUnitConvertor2 = new PdfUnitConvertor();
                      if (!this.m_cffGlyphs.RenderedPath.ContainsKey(charCode))
                      {
                        this.pathGeom = (GraphicsPath) glyphWriter2.glyphParser(charCode, num8, this.UnicodeCharMapTable);
                        this.m_cffGlyphs.RenderedPath.Add(charCode, (object) this.pathGeom);
                      }
                      else
                        this.pathGeom = (GraphicsPath) this.m_cffGlyphs.RenderedPath[charCode];
                    }
                    if (this.m_spaceCheck)
                    {
                      this.m_spaceCheck = false;
                      this.pathGeom = new GraphicsPath();
                    }
                  }
                }
                else
                {
                  FontStructure.GetCharCode(str5);
                  if (this.ReverseMapTable.ContainsKey(str5) && this.FontEncoding != "Identity-H")
                  {
                    string str6 = this.ReverseMapTable[str5].ToString();
                    if (glyphWriter2.glyphs.ContainsKey(str6))
                    {
                      System.Drawing.Drawing2D.Matrix transform = g.Transform;
                      PdfUnitConvertor pdfUnitConvertor3 = new PdfUnitConvertor();
                      if (!this.m_cffGlyphs.RenderedPath.ContainsKey(str6))
                      {
                        this.pathGeom = (GraphicsPath) glyphWriter2.glyphParser(str6, num8, this.UnicodeCharMapTable);
                        this.m_cffGlyphs.RenderedPath.Add(str6, (object) this.pathGeom);
                      }
                      else
                        this.pathGeom = (GraphicsPath) this.m_cffGlyphs.RenderedPath[str6];
                    }
                  }
                  else
                  {
                    string str7 = num8.ToString();
                    if (glyphWriter2.glyphs.ContainsKey(str7))
                    {
                      System.Drawing.Drawing2D.Matrix transform = g.Transform;
                      PdfUnitConvertor pdfUnitConvertor4 = new PdfUnitConvertor();
                      if (!this.m_cffGlyphs.RenderedPath.ContainsKey(str7))
                      {
                        this.pathGeom = (GraphicsPath) glyphWriter2.glyphParser(str7, num8, this.UnicodeCharMapTable);
                        this.m_cffGlyphs.RenderedPath.Add(str7, (object) this.pathGeom);
                      }
                      else
                        this.pathGeom = (GraphicsPath) this.m_cffGlyphs.RenderedPath[str7];
                    }
                  }
                }
                if (gWidths != null)
                {
                  if (!differenceTable.ContainsValue(str5) && num8 == 0)
                  {
                    foreach (KeyValuePair<double, string> keyValuePair in this.CharacterMapTable)
                    {
                      if (keyValuePair.Value.Equals(str5))
                        num8 = (int) keyValuePair.Key;
                    }
                  }
                  if (gWidths.ContainsKey(num8))
                  {
                    this.currentGlyphWidth = (float) gWidths[num8];
                    this.currentGlyphWidth *= this.CharSizeMultiplier;
                  }
                  else if (this.OctDecMapTable != null && this.OctDecMapTable.Count != 0)
                  {
                    num8 = this.OctDecMapTable[num8];
                    if (gWidths.ContainsKey(num8))
                    {
                      this.currentGlyphWidth = (float) gWidths[num8];
                      this.currentGlyphWidth *= this.CharSizeMultiplier;
                    }
                    else
                    {
                      this.currentGlyphWidth = this.DefaultGlyphWidth;
                      this.currentGlyphWidth *= this.CharSizeMultiplier;
                    }
                  }
                  else if (this.CharacterMapTable.Count != 0)
                  {
                    foreach (KeyValuePair<double, string> keyValuePair in this.CharacterMapTable)
                    {
                      if (keyValuePair.Value.Equals(str5))
                        num8 = (int) keyValuePair.Key;
                    }
                    if (gWidths.ContainsKey(num8))
                    {
                      this.currentGlyphWidth = (float) gWidths[num8];
                      this.currentGlyphWidth *= this.CharSizeMultiplier;
                    }
                    else
                    {
                      this.currentGlyphWidth = this.DefaultGlyphWidth;
                      this.currentGlyphWidth *= this.CharSizeMultiplier;
                    }
                  }
                  else
                  {
                    this.currentGlyphWidth = this.DefaultGlyphWidth;
                    this.currentGlyphWidth *= this.CharSizeMultiplier;
                  }
                }
                else
                {
                  this.currentGlyphWidth = this.DefaultGlyphWidth;
                  this.currentGlyphWidth *= this.CharSizeMultiplier;
                }
                float num10 = (float) (num9 / 100.0) * (float) textScaling;
                if (num6 < s.Length)
                  newLocation.X += num10 + this.CharacterSpacing;
                else
                  newLocation.X += num10;
                if (num8 != 32 /*0x20*/)
                {
                  if (ch1 != ' ')
                    goto label_240;
                }
                newLocation.X += this.WordSpacing;
              }
              catch
              {
                if (ch1 == ' ')
                {
                  newLocation.X += width + this.WordSpacing;
                  this.renderedText += " ";
                  continue;
                }
                SizeF sizeF = g.MeasureString(ch1.ToString(), this.textFont, PointF.Empty, stringFormat);
                float num11 = sizeF.Width / 100f * this.TextScaling;
                if (this.FontGlyphWidths != null)
                {
                  if (this.FontGlyphWidths.ContainsKey((int) ch1))
                    num11 = (float) this.FontGlyphWidths[(int) ch1] * (this.CharSizeMultiplier * this.FontSize) / 100f * this.TextScaling;
                }
                try
                {
                  if ((byte) ch1 > (byte) 126 && this.m_fontEncoding == "MacRomanEncoding")
                  {
                    char letter = (char) this.MacRomanToUnicode[(int) (byte) ch1 - 128 /*0x80*/];
                    if (this.isNegativeFont)
                    {
                      GraphicsState gstate = g.Save();
                      g.MultiplyTransform(new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, -1f, 0.0f, (float) (2.0 * (double) newLocation.Y + 2.0 * (double) sizeF.Height)));
                      flag2 = true;
                      this.DrawSystemFontGlyphShape(ch1, g, out textmatrix);
                      g.Restore(gstate);
                    }
                    else
                    {
                      flag2 = true;
                      this.DrawSystemFontGlyphShape(letter, g, out textmatrix);
                    }
                  }
                  else
                  {
                    if (this.isNegativeFont)
                    {
                      GraphicsState gstate = g.Save();
                      g.MultiplyTransform(new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, -1f, 0.0f, (float) (2.0 * (double) newLocation.Y + 2.0 * (double) sizeF.Height)));
                      flag2 = true;
                      this.DrawSystemFontGlyphShape(ch1, g, out textmatrix);
                      g.Restore(gstate);
                    }
                    else if (this.RenderingMode == 1)
                    {
                      GraphicsPath path = new GraphicsPath();
                      FontFamily family = new FontFamily(this.textFont.Name);
                      path.AddString(ch1.ToString(), family, 1, this.textFont.Size, (System.Drawing.Point) new Point((double) (int) newLocation.X, (double) (int) newLocation.Y), stringFormat);
                      if (!this.IsPdfium)
                        g.DrawPath(new Pen(this.PathBrush), path);
                      path.Dispose();
                      family.Dispose();
                    }
                    else
                    {
                      flag2 = true;
                      this.DrawSystemFontGlyphShape(ch1, g, out textmatrix);
                    }
                    this.renderedText += ch1.ToString();
                  }
                }
                catch (Exception ex)
                {
                  this.exceptions.Exceptions.Append($"\r\nCharacter not rendered {ch1.ToString()}\r\n{ex.StackTrace}");
                  continue;
                }
                if (num6 < s.Length)
                  newLocation.X += num11 + this.CharacterSpacing;
                else
                  newLocation.X += num11;
              }
label_240:
              if (!flag2)
              {
                if (str4 == string.Empty)
                  this.DrawGlyphs(this.pathGeom, this.currentGlyphWidth, g, out textmatrix, ch1.ToString());
                else
                  this.DrawGlyphs(this.pathGeom, this.currentGlyphWidth, g, out textmatrix, str4);
              }
              this.m_spaceCheck = false;
            }
            else
            {
              SizeF sizeF = g.MeasureString(ch1.ToString(), this.textFont, PointF.Empty, stringFormat);
              num1 = sizeF.Width / 100f * this.TextScaling;
              try
              {
                if ((byte) ch1 > (byte) 126 && this.m_fontEncoding == "MacRomanEncoding" && !this.Isembeddedfont)
                {
                  long num12 = this.MacRomanToUnicode[(int) (byte) ch1 - 128 /*0x80*/];
                  if (this.isNegativeFont)
                  {
                    GraphicsState gstate = g.Save();
                    g.MultiplyTransform(new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, -1f, 0.0f, (float) (2.0 * (double) newLocation.Y + 2.0 * (double) sizeF.Height)));
                    flag2 = true;
                    this.DrawSystemFontGlyphShape(ch1, g, out textmatrix);
                    g.Restore(gstate);
                  }
                  else
                  {
                    flag2 = true;
                    this.DrawSystemFontGlyphShape(ch1, g, out textmatrix);
                  }
                }
                else
                {
                  if (this.isNegativeFont && !this.Isembeddedfont)
                  {
                    GraphicsState gstate = g.Save();
                    g.MultiplyTransform(new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, -1f, 0.0f, (float) (2.0 * (double) newLocation.Y + 2.0 * (double) sizeF.Height)));
                    flag2 = true;
                    this.DrawSystemFontGlyphShape(ch1, g, out textmatrix);
                    g.Restore(gstate);
                  }
                  else if (this.RenderingMode == 1)
                  {
                    flag2 = true;
                    this.DrawSystemFontGlyphShape(ch1, g, out textmatrix);
                  }
                  else if (this.FontEncoding != "Identity-H" && this.structure.fontType.Value == "TrueType" && this.structure.GlyphFontFile2 != null)
                  {
                    if (this.OctDecMapTable != null && this.OctDecMapTable.ContainsKey((int) ch1))
                      ch1 = (char) this.OctDecMapTable[(int) ch1];
                    this.pathGeom = this.structure.GetGlyph(ch1);
                    if (this.pathGeom == null && this.CharacterMapTable.Count > 0 && this.CharacterMapTable.ContainsKey((double) ch1))
                    {
                      char letter = this.CharacterMapTable[(double) ch1].ToCharArray()[0];
                      flag2 = true;
                      bool flag3 = false;
                      if (this.structure != null && !this.structure.IsMappingDone)
                      {
                        this.structure.IsMappingDone = true;
                        flag3 = true;
                      }
                      this.DrawSystemFontGlyphShape(letter, g, out textmatrix);
                      if (flag3)
                        this.structure.IsMappingDone = false;
                    }
                    else if (this.pathGeom != null && this.pathGeom.PathData.Points.Length == 0 && this.structure.m_winansiMapTable.ContainsKey((int) ch1))
                    {
                      flag2 = true;
                      this.DrawSystemFontGlyphShape(ch1, g, out textmatrix);
                    }
                  }
                  else if (this.structure.GlyphFontFile2 != null)
                    this.pathGeom = this.ReverseMapTable.Count != this.CharacterMapTable.Count ? this.structure.GlyphFontFile2.GetCIDGlyphs(ch1, this.CharacterMapTable) : this.structure.GlyphFontFile2.GetCIDGlyphs(ch1, this.ReverseMapTable, this.CidToGidReverseMapTable);
                  else if (this.CharacterMapTable.Count > 0 && this.CharacterMapTable.ContainsKey((double) ch1))
                  {
                    char letter = this.CharacterMapTable[(double) ch1].ToCharArray()[0];
                    flag2 = true;
                    bool flag4 = false;
                    if (this.structure != null && !this.structure.IsMappingDone)
                    {
                      this.structure.IsMappingDone = true;
                      flag4 = true;
                    }
                    this.DrawSystemFontGlyphShape(letter, g, out textmatrix);
                    if (flag4)
                      this.structure.IsMappingDone = false;
                  }
                  else
                  {
                    flag2 = true;
                    if (!this.DrawSystemFontGlyphShape(ch1, g, out textmatrix))
                      this.DrawSystemFontGlyph(ch1.ToString(), this.textFont, new Pen(this.PathBrush).Brush, newLocation, stringFormat, g, out newLocation);
                  }
                  if (this.CharacterMapTable.ContainsKey((double) ch1) && this.CharacterMapTable.Count > 0)
                  {
                    string str8 = this.CharacterMapTable[(double) ch1];
                    char[] chArray = new char[str8.Length];
                    char ch2 = str8.ToCharArray()[0];
                    if (this.FontGlyphWidths != null)
                    {
                      if (this.structure.fontType.Value == "Type0")
                      {
                        if (this.CidToGidReverseMapTable != null && this.CidToGidReverseMapTable.ContainsKey((int) ch1) && !this.structure.IsMappingDone)
                        {
                          this.currentGlyphWidth = (float) this.FontGlyphWidths[this.CidToGidReverseMapTable[(int) ch1]];
                          this.currentGlyphWidth *= this.CharSizeMultiplier;
                        }
                        else if (this.FontGlyphWidths.ContainsKey((int) ch1))
                        {
                          this.currentGlyphWidth = (float) this.FontGlyphWidths[(int) ch1];
                          this.currentGlyphWidth *= this.CharSizeMultiplier;
                        }
                        else if (this.ReverseMapTable.ContainsKey(ch2.ToString()))
                        {
                          if (this.FontGlyphWidths.ContainsKey((int) this.ReverseMapTable[ch2.ToString()]))
                          {
                            this.currentGlyphWidth = g.MeasureString(ch2.ToString(), this.textFont, PointF.Empty, stringFormat).Width / 100f * this.TextScaling / this.textFont.Size;
                          }
                          else
                          {
                            this.currentGlyphWidth = this.DefaultGlyphWidth;
                            this.currentGlyphWidth *= this.CharSizeMultiplier;
                          }
                        }
                      }
                      else if (this.structure.fontType.Value == "TrueType")
                      {
                        if (this.FontGlyphWidths.ContainsKey((int) ch1))
                        {
                          this.currentGlyphWidth = (float) this.FontGlyphWidths[(int) ch1];
                          this.currentGlyphWidth *= this.CharSizeMultiplier;
                        }
                      }
                    }
                    else if (this.FontGlyphWidths == null)
                    {
                      this.currentGlyphWidth = this.DefaultGlyphWidth;
                      this.currentGlyphWidth *= this.CharSizeMultiplier;
                    }
                  }
                  else if (this.CidToGidReverseMapTable != null && this.CidToGidReverseMapTable.Count > 1)
                  {
                    if (this.CidToGidReverseMapTable.ContainsKey((int) ch1))
                    {
                      int key = this.CidToGidReverseMapTable[(int) ch1];
                      if (this.FontGlyphWidths != null)
                      {
                        if (this.FontGlyphWidths.ContainsKey(key))
                        {
                          this.currentGlyphWidth = (float) this.FontGlyphWidths[key];
                          this.currentGlyphWidth *= this.CharSizeMultiplier;
                        }
                      }
                    }
                  }
                  else if (this.FontGlyphWidths != null)
                  {
                    if (this.FontGlyphWidths.ContainsKey((int) ch1))
                    {
                      this.currentGlyphWidth = (float) this.FontGlyphWidths[(int) ch1];
                      this.currentGlyphWidth *= this.CharSizeMultiplier;
                    }
                    else
                      this.currentGlyphWidth = this.DefaultGlyphWidth * this.CharSizeMultiplier;
                  }
                }
              }
              catch (Exception ex)
              {
                this.exceptions.Exceptions.Append($"\r\nCharacter not rendered {ch1.ToString()}\r\n{ex.StackTrace}");
                continue;
              }
              if (num6 < s.Length)
                newLocation.X += this.CharacterSpacing;
              if (!flag2)
                this.DrawGlyphs(this.pathGeom, this.currentGlyphWidth, g, out textmatrix, ch1.ToString());
              if (this.pathGeom != null && this.pathGeom.PathData.Points.Length == 0 && (this.structure.m_winansiMapTable.ContainsKey((int) ch1) || this.structure.CidToGidMap != null && this.CharacterMapTable != null && this.CidToGidReverseMapTable != null && this.CharacterMapTable.Count == this.structure.CidToGidMap.Count && this.CidToGidReverseMapTable.Count > 0 && this.CidToGidReverseMapTable.ContainsKey((int) ch1)))
                flag2 = false;
            }
          }
        }
      }
    }
    return newLocation.X - x;
  }

  private bool IsTextGlyphAdded { set; get; }

  private void GetMacEncodeTable()
  {
    this.m_macEncodeTable = new Dictionary<int, string>();
    this.m_macEncodeTable.Add((int) sbyte.MaxValue, " ");
    this.m_macEncodeTable.Add(128 /*0x80*/, "Ä");
    this.m_macEncodeTable.Add(129, "Å");
    this.m_macEncodeTable.Add(130, "Ç");
    this.m_macEncodeTable.Add(131, "É");
    this.m_macEncodeTable.Add(132, "Ñ");
    this.m_macEncodeTable.Add(133, "Ö");
    this.m_macEncodeTable.Add(134, "Ü");
    this.m_macEncodeTable.Add(135, "á");
    this.m_macEncodeTable.Add(136, "à");
    this.m_macEncodeTable.Add(137, "â");
    this.m_macEncodeTable.Add(138, "ä");
    this.m_macEncodeTable.Add(139, "ã");
    this.m_macEncodeTable.Add(140, "å");
    this.m_macEncodeTable.Add(141, "ç");
    this.m_macEncodeTable.Add(142, "é");
    this.m_macEncodeTable.Add(143, "è");
    this.m_macEncodeTable.Add(144 /*0x90*/, "ê");
    this.m_macEncodeTable.Add(145, "ë");
    this.m_macEncodeTable.Add(146, "í");
    this.m_macEncodeTable.Add(147, "ì");
    this.m_macEncodeTable.Add(148, "î");
    this.m_macEncodeTable.Add(149, "ï");
    this.m_macEncodeTable.Add(150, "ñ");
    this.m_macEncodeTable.Add(151, "ó");
    this.m_macEncodeTable.Add(152, "ò");
    this.m_macEncodeTable.Add(153, "ô");
    this.m_macEncodeTable.Add(154, "ö");
    this.m_macEncodeTable.Add(155, "õ");
    this.m_macEncodeTable.Add(156, "ú");
    this.m_macEncodeTable.Add(157, "ù");
    this.m_macEncodeTable.Add(158, "û");
    this.m_macEncodeTable.Add(159, "ü");
    this.m_macEncodeTable.Add(160 /*0xA0*/, "†");
    this.m_macEncodeTable.Add(161, "°");
    this.m_macEncodeTable.Add(162, "¢");
    this.m_macEncodeTable.Add(163, "£");
    this.m_macEncodeTable.Add(164, "§");
    this.m_macEncodeTable.Add(165, "•");
    this.m_macEncodeTable.Add(166, "¶");
    this.m_macEncodeTable.Add(167, "ß");
    this.m_macEncodeTable.Add(168, "®");
    this.m_macEncodeTable.Add(169, "©");
    this.m_macEncodeTable.Add(170, "™");
    this.m_macEncodeTable.Add(171, "´");
    this.m_macEncodeTable.Add(172, "¨");
    this.m_macEncodeTable.Add(173, "≠");
    this.m_macEncodeTable.Add(174, "Æ");
    this.m_macEncodeTable.Add(175, "Ø");
    this.m_macEncodeTable.Add(176 /*0xB0*/, "∞");
    this.m_macEncodeTable.Add(177, "±");
    this.m_macEncodeTable.Add(178, "≤");
    this.m_macEncodeTable.Add(179, "≥");
    this.m_macEncodeTable.Add(180, "¥");
    this.m_macEncodeTable.Add(181, "µ");
    this.m_macEncodeTable.Add(182, "∂");
    this.m_macEncodeTable.Add(183, "∑");
    this.m_macEncodeTable.Add(184, "∏");
    this.m_macEncodeTable.Add(185, "π");
    this.m_macEncodeTable.Add(186, "∫");
    this.m_macEncodeTable.Add(187, "ª");
    this.m_macEncodeTable.Add(188, "º");
    this.m_macEncodeTable.Add(189, "Ω");
    this.m_macEncodeTable.Add(190, "æ");
    this.m_macEncodeTable.Add(191, "ø");
    this.m_macEncodeTable.Add(192 /*0xC0*/, "¿");
    this.m_macEncodeTable.Add(193, "¡");
    this.m_macEncodeTable.Add(194, "¬");
    this.m_macEncodeTable.Add(195, "√");
    this.m_macEncodeTable.Add(196, "ƒ");
    this.m_macEncodeTable.Add(197, "≈");
    this.m_macEncodeTable.Add(198, "∆");
    this.m_macEncodeTable.Add(199, "«");
    this.m_macEncodeTable.Add(200, "»");
    this.m_macEncodeTable.Add(201, "…");
    this.m_macEncodeTable.Add(202, " ");
    this.m_macEncodeTable.Add(203, "À");
    this.m_macEncodeTable.Add(204, "Ã");
    this.m_macEncodeTable.Add(205, "Õ");
    this.m_macEncodeTable.Add(206, "Œ");
    this.m_macEncodeTable.Add(207, "œ");
    this.m_macEncodeTable.Add(208 /*0xD0*/, "–");
    this.m_macEncodeTable.Add(209, "—");
    this.m_macEncodeTable.Add(210, "“");
    this.m_macEncodeTable.Add(211, "”");
    this.m_macEncodeTable.Add(212, "‘");
    this.m_macEncodeTable.Add(213, "’");
    this.m_macEncodeTable.Add(214, "÷");
    this.m_macEncodeTable.Add(215, "◊");
    this.m_macEncodeTable.Add(216, "ÿ");
    this.m_macEncodeTable.Add(217, "Ÿ");
    this.m_macEncodeTable.Add(218, "⁄");
    this.m_macEncodeTable.Add(219, "€");
    this.m_macEncodeTable.Add(220, "‹");
    this.m_macEncodeTable.Add(221, "›");
    this.m_macEncodeTable.Add(222, "ﬁ");
    this.m_macEncodeTable.Add(223, "ﬂ");
    this.m_macEncodeTable.Add(224 /*0xE0*/, "‡");
    this.m_macEncodeTable.Add(225, "·");
    this.m_macEncodeTable.Add(226, ",");
    this.m_macEncodeTable.Add(227, "„");
    this.m_macEncodeTable.Add(228, "‰");
    this.m_macEncodeTable.Add(229, "Â");
    this.m_macEncodeTable.Add(230, "Ê");
    this.m_macEncodeTable.Add(231, "Á");
    this.m_macEncodeTable.Add(232, "Ë");
    this.m_macEncodeTable.Add(233, "È");
    this.m_macEncodeTable.Add(234, "Í");
    this.m_macEncodeTable.Add(235, "Î");
    this.m_macEncodeTable.Add(236, "Ï");
    this.m_macEncodeTable.Add(237, "Ì");
    this.m_macEncodeTable.Add(238, "Ó");
    this.m_macEncodeTable.Add(239, "Ô");
    this.m_macEncodeTable.Add(240 /*0xF0*/, "\uF8FF");
    this.m_macEncodeTable.Add(241, "Ò");
    this.m_macEncodeTable.Add(242, "Ú");
    this.m_macEncodeTable.Add(243, "Û");
    this.m_macEncodeTable.Add(244, "Ù");
    this.m_macEncodeTable.Add(245, "ı");
    this.m_macEncodeTable.Add(246, "ˆ");
    this.m_macEncodeTable.Add(247, "˜");
    this.m_macEncodeTable.Add(248, "¯");
    this.m_macEncodeTable.Add(249, "˘");
    this.m_macEncodeTable.Add(250, "˙");
    this.m_macEncodeTable.Add(251, "˚");
    this.m_macEncodeTable.Add(252, "¸");
    this.m_macEncodeTable.Add(253, "˝");
    this.m_macEncodeTable.Add(254, "˛");
    this.m_macEncodeTable.Add((int) byte.MaxValue, "ˇ");
  }

  private void RenderReverseMapTableByte(char character, System.Drawing.Graphics g)
  {
    g.Transform = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
    Glyph glyph = new Glyph();
    glyph.FontSize = (double) this.FontSize;
    glyph.FontFamily = this.FontName;
    glyph.FontStyle = this.FontStyle;
    glyph.Stroke = this.PathBrush;
    glyph.NonStroke = this.PathNonStrokeBrush;
    glyph.TransformMatrix = this.GetTextRenderingMatrix();
    glyph.Name = character.ToString();
    glyph.HorizontalScaling = (double) this.TextHorizontalScaling;
    glyph.CharId = new CharCode((int) character);
    glyph.CharSpacing = (double) this.CharacterSpacing;
    string[] fontEncodingNames = this.GetStandardFontEncodingNames();
    byte[] bytes = Encoding.UTF8.GetBytes(character.ToString());
    if (this.structure.ReverseDictMapping.ContainsKey(character.ToString()))
    {
      float num = (float) this.structure.ReverseDictMapping[character.ToString()];
      if (this.structure.DifferencesDictionary.ContainsKey(num.ToString()))
      {
        glyph.Name = FontStructure.GetCharCode(this.structure.DifferencesDictionary[((int) character).ToString()]);
      }
      else
      {
        bytes[0] = (byte) num;
        glyph.Name = fontEncodingNames[(int) bytes[0]];
      }
    }
    else if (this.OctDecMapTable != null && this.OctDecMapTable.ContainsKey((int) character))
    {
      char index = (char) this.OctDecMapTable[(int) character];
      glyph.Name = fontEncodingNames[(int) index];
    }
    else if (this.structure.DifferencesDictionary.ContainsKey(((int) character).ToString()))
      glyph.Name = FontStructure.GetCharCode(this.structure.DifferencesDictionary[((int) character).ToString()]);
    else if (fontEncodingNames.Length > (int) character)
      glyph.Name = fontEncodingNames[(int) character];
    else
      glyph.Name = fontEncodingNames[(int) bytes[0]];
    glyph.Width = this.GetGlyphWidth(glyph);
    this.FontSource.GetGlyphOutlines(glyph, 100.0);
    this.GlyphToSLCoordinates(glyph);
    GraphicsPath path = new PdfElementsRenderer().RenderGlyph(glyph);
    Matrix identity = Matrix.Identity;
    identity.Scale(0.01, 0.01, 0.0, 0.0);
    identity.Translate(0.0, 1.0);
    this.transformations.PushTransform(identity * glyph.TransformMatrix);
    System.Drawing.Drawing2D.Matrix matrix = g.Transform.Clone();
    matrix.Multiply(this.GetTransformationMatrix(this.transformations.CurrentTransform));
    g.Transform = matrix;
    g.SmoothingMode = SmoothingMode.AntiAlias;
    g.FillPath(glyph.Stroke, path);
    this.UpdateTextMatrix(glyph);
    this.transformations.PopTransform();
  }

  private void DrawPath(System.Drawing.Graphics g, SystemFontGlyph glyph, string charString)
  {
    GraphicsPath path = new GraphicsPath();
    foreach (SystemFontPathFigure outline in (List<SystemFontPathFigure>) glyph.Outlines)
    {
      path.StartFigure();
      PointF pointF = new PointF((float) outline.StartPoint.X, (float) outline.StartPoint.Y);
      System.Drawing.Drawing2D.Matrix transformationMatrix = this.GetTransformationMatrix(SystemFontMatrix.Identity);
      foreach (SystemFontPathSegment segment in outline.Segments)
      {
        switch (segment)
        {
          case SystemFontLineSegment _:
            SystemFontLineSegment systemFontLineSegment = (SystemFontLineSegment) segment;
            PointF[] pts1 = new PointF[2]
            {
              pointF,
              new PointF((float) systemFontLineSegment.Point.X, (float) systemFontLineSegment.Point.Y)
            };
            transformationMatrix.TransformPoints(pts1);
            path.AddLine(pts1[0], pts1[1]);
            pointF = new PointF((float) systemFontLineSegment.Point.X, (float) systemFontLineSegment.Point.Y);
            continue;
          case SystemFontBezierSegment _:
            SystemFontBezierSegment fontBezierSegment = segment as SystemFontBezierSegment;
            PointF[] pts2 = new PointF[4]
            {
              pointF,
              new PointF((float) fontBezierSegment.Point1.X, (float) fontBezierSegment.Point1.Y),
              new PointF((float) fontBezierSegment.Point2.X, (float) fontBezierSegment.Point2.Y),
              new PointF((float) fontBezierSegment.Point3.X, (float) fontBezierSegment.Point3.Y)
            };
            transformationMatrix.TransformPoints(pts2);
            path.AddBezier(pts2[0], pts2[1], pts2[2], pts2[3]);
            pointF = new PointF((float) fontBezierSegment.Point3.X, (float) fontBezierSegment.Point3.Y);
            continue;
          case SystemFontQuadraticBezierSegment _:
            SystemFontQuadraticBezierSegment quadraticBezierSegment = segment as SystemFontQuadraticBezierSegment;
            PointF[] pts3 = new PointF[3]
            {
              pointF,
              new PointF((float) quadraticBezierSegment.Point1.X, (float) quadraticBezierSegment.Point1.Y),
              new PointF((float) quadraticBezierSegment.Point2.X, (float) quadraticBezierSegment.Point2.Y)
            };
            transformationMatrix.TransformPoints(pts3);
            path.AddBezier(pts3[0], pts3[1], pts3[2], pts3[2]);
            pointF = new PointF((float) quadraticBezierSegment.Point2.X, (float) quadraticBezierSegment.Point2.Y);
            continue;
          default:
            continue;
        }
      }
      if (outline.IsClosed)
        path.CloseFigure();
    }
    g.SmoothingMode = SmoothingMode.AntiAlias;
    g.PageUnit = GraphicsUnit.Pixel;
    Brush pathBrush = this.PathBrush;
    switch (this.RenderingMode)
    {
      case 0:
        g.FillPath(glyph.Stroke, path);
        break;
      case 1:
        g.DrawPath(new Pen(glyph.NonStroke, this.LineWidth), path);
        break;
      case 2:
        g.FillPath(glyph.Stroke, path);
        g.DrawPath(new Pen(glyph.NonStroke, this.LineWidth), path);
        break;
      case 3:
        g.FillPath(glyph.Stroke, path);
        break;
    }
  }

  private System.Drawing.Drawing2D.Matrix GetTransformationMatrix(SystemFontMatrix transform)
  {
    return new System.Drawing.Drawing2D.Matrix((float) transform.M11, (float) transform.M12, (float) transform.M21, (float) transform.M22, (float) transform.OffsetX, (float) transform.OffsetY);
  }

  private ushort GetGlyphID(string glyphName)
  {
    if (this.IsNonsymbolic || this.structure.BaseFontEncoding == "MacRomanEncoding" || this.structure.BaseFontEncoding == "WinAnsiEncoding")
    {
      SystemFontCMapTable cmapTable1 = this.openTypeFontSource.CMap.GetCMapTable((ushort) 3, (ushort) 1);
      if (this.structure.CharacterMapTable != null && this.structure.CharacterMapTable.ContainsValue(glyphName) && this.structure.fontType.Value == "Type0" && this.structure.CidToGidReverseMapTable.Count == 0)
      {
        foreach (int key in this.structure.CharacterMapTable.Keys)
        {
          if (this.structure.CharacterMapTable[(double) key] == glyphName)
            return (ushort) key;
        }
      }
      if (cmapTable1 != null)
        return this.FontEncoding == "Identity-H" ? cmapTable1.GetGlyphId((ushort) glyphName[0]) : this.GetGlyphsFromMicrosoftUnicodeWithEncoding(cmapTable1, glyphName);
      SystemFontCMapTable cmapTable2 = this.openTypeFontSource.CMap.GetCMapTable((ushort) 1, (ushort) 0);
      if (cmapTable2 == null)
        return 0;
      return this.FontEncoding == "Identity-H" ? cmapTable2.GetGlyphId((ushort) glyphName[0]) : this.GetGlyphsFromMacintoshRomanWithEncoding(cmapTable2, glyphName);
    }
    SystemFontCMapTable cmapTable3 = this.openTypeFontSource.CMap.GetCMapTable((ushort) 3, (ushort) 0);
    if (cmapTable3 != null)
      return this.GetGlyphsFromMicrosoftSymbolWithoutEncoding(cmapTable3, glyphName);
    SystemFontCMapTable cmapTable4 = this.openTypeFontSource.CMap.GetCMapTable((ushort) 1, (ushort) 0);
    if (cmapTable4 != null)
      return this.GetGlyphsFromMacintoshRomanWithoutEncoding(cmapTable4, glyphName);
    if (this.FontEncoding != "GBK-EUC-H")
    {
      SystemFontCMapTable cmapTable5 = this.openTypeFontSource.CMap.GetCMapTable((ushort) 3, (ushort) 1);
      if (cmapTable5 != null)
        return this.GetGlyphsFromMacintoshRomanWithoutEncoding(cmapTable5, glyphName);
    }
    return 0;
  }

  private ushort GetGlyphsFromMacintoshRomanWithoutEncoding(
    SystemFontCMapTable roman,
    string glyphName)
  {
    byte num = (byte) glyphName[0];
    Glyph glyph = new Glyph();
    glyph.CharId = new CharCode(num);
    glyph.GlyphId = roman.GetGlyphId((ushort) num);
    return glyph.GlyphId;
  }

  private ushort GetGlyphsFromMicrosoftSymbolWithoutEncoding(
    SystemFontCMapTable unicode,
    string glyphName)
  {
    this.CalculateByteToAppend(unicode);
    byte b = (byte) glyphName[0];
    ushort res;
    if (!this.TryAppendByte(b, out res))
      return 0;
    Glyph glyph = new Glyph();
    glyph.CharId = new CharCode(b);
    glyph.GlyphId = unicode.GetGlyphId(res);
    return glyph.GlyphId;
  }

  private void CalculateByteToAppend(SystemFontCMapTable unicode)
  {
    if (this.firstCode.HasValue)
      return;
    try
    {
      this.firstCode = new CharCode?(new CharCode(unicode.FirstCode));
    }
    catch (NotSupportedException ex)
    {
      this.firstCode = new CharCode?(new CharCode());
    }
  }

  private bool TryAppendByte(byte b, out ushort res)
  {
    res = (ushort) 0;
    if (this.firstCode.HasValue)
    {
      if (!this.firstCode.Value.IsEmpty)
      {
        try
        {
          CharCode charCode = new CharCode(new byte[2]
          {
            this.firstCode.Value.Bytes[0],
            b
          });
          res = (ushort) charCode.IntValue;
          return true;
        }
        catch
        {
          return false;
        }
      }
    }
    return false;
  }

  private ushort GetGlyphsFromMacintoshRomanWithEncoding(
    SystemFontCMapTable cMapTable,
    string glyphName)
  {
    byte b = (byte) glyphName[0];
    Glyph glyph = new Glyph();
    glyph.CharId = new CharCode(b);
    byte charId = SystemFontPredefinedEncoding.StandardMacRomanEncoding.GetCharId(glyphName);
    glyph.GlyphId = cMapTable.GetGlyphId((ushort) charId);
    return glyph.GlyphId;
  }

  private ushort GetGlyphsFromMicrosoftUnicodeWithEncoding(
    SystemFontCMapTable unicode,
    string glyphName)
  {
    ushort unicodeWithEncoding = 0;
    try
    {
      byte b = (byte) glyphName[0];
      string name = this.GetName(b);
      if (name != null)
      {
        string charCode = FontStructure.GetCharCode(name);
        unicodeWithEncoding = !SystemFontAdobeGlyphList.IsSupportedPdfName(name) ? (!SystemFontAdobeGlyphList.IsSupportedPdfName(charCode) ? this.GetGlyphIdFromPostTable(this.GetName(b)) : unicode.GetGlyphId((ushort) SystemFontAdobeGlyphList.GetUnicode(charCode))) : unicode.GetGlyphId((ushort) SystemFontAdobeGlyphList.GetUnicode(name));
      }
    }
    catch
    {
      unicodeWithEncoding = (ushort) 0;
    }
    return unicodeWithEncoding;
  }

  private string GetName(byte b)
  {
    this.Initialize();
    return this.names[(int) b];
  }

  private void Initialize()
  {
    if (this.m_fontEncoding != null && this.m_fontEncoding != string.Empty)
    {
      SystemFontPredefinedEncoding predefinedEncoding1 = SystemFontPredefinedEncoding.GetPredefinedEncoding(this.m_fontEncoding);
      if (predefinedEncoding1 != null)
      {
        this.names = predefinedEncoding1.GetNames();
      }
      else
      {
        SystemFontPredefinedEncoding predefinedEncoding2 = SystemFontPredefinedEncoding.GetPredefinedEncoding(this.structure.BaseFontEncoding);
        if (predefinedEncoding2 != null)
          this.names = predefinedEncoding2.GetNames();
      }
    }
    if (this.names != null)
      return;
    this.names = SystemFontPredefinedEncoding.StandardEncoding.GetNames();
  }

  private void MapDifferenceElement()
  {
    if (this.structure.DifferencesDictionary == null)
      return;
    int result = 0;
    List<string> stringList1 = new List<string>((IEnumerable<string>) this.structure.DifferencesDictionary.Keys);
    List<string> stringList2 = new List<string>((IEnumerable<string>) this.structure.DifferencesDictionary.Values);
    for (int index = 0; index < stringList1.Count; ++index)
    {
      int.TryParse(stringList1[index], out result);
      if (result < 256 /*0x0100*/)
        this.names[result] = stringList2[index];
    }
  }

  private ushort GetGlyphIdFromPostTable(string name)
  {
    return this.openTypeFontSource.Post == null ? (ushort) 0 : this.openTypeFontSource.Post.GetGlyphId(name);
  }

  internal SystemFontFontsManager SystemFontsManager
  {
    get
    {
      if (this.systemFontsManager == null)
        this.systemFontsManager = new SystemFontFontsManager();
      return this.systemFontsManager;
    }
  }

  private void DrawSystemFontGlyph(
    string str,
    Font textFont,
    Brush brush,
    PointF currentLocation,
    StringFormat format,
    System.Drawing.Graphics g,
    out PointF newLocation)
  {
    newLocation = currentLocation;
    if (str == " ")
    {
      float width = g.MeasureString(" ", textFont, PointF.Empty, new StringFormat(StringFormat.GenericTypographic)
      {
        FormatFlags = StringFormatFlags.MeasureTrailingSpaces
      }).Width;
      newLocation.X += width;
    }
    else
    {
      g.PageUnit = GraphicsUnit.Point;
      g.DrawString(str, textFont, new Pen(this.PathBrush).Brush, currentLocation, format);
      float num = g.MeasureString(str, textFont, PointF.Empty, format).Width;
      if (this.FontGlyphWidths != null && !this.m_isMpdfFont && this.FontGlyphWidths.ContainsKey((int) str[0]))
        num = (float) this.FontGlyphWidths[(int) str[0]] * (this.CharSizeMultiplier * this.FontSize);
      newLocation.X += num;
    }
    this.renderedText += str;
  }

  public void DrawGlyphs(
    GraphicsPath path,
    float glyphwidth,
    System.Drawing.Graphics g,
    out Matrix temptextmatrix,
    string glyphChar)
  {
    GraphicsUnit pageUnit = g.PageUnit;
    System.Drawing.Drawing2D.Matrix transform = g.Transform;
    g.PageUnit = GraphicsUnit.Pixel;
    g.Transform = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
    Glyph glyph = new Glyph();
    glyph.FontSize = (double) this.FontSize;
    glyph.FontFamily = this.FontName;
    this.SetEmbededFontName(glyph);
    glyph.FontStyle = this.FontStyle;
    glyph.Stroke = this.PathBrush;
    glyph.NonStroke = this.PathNonStrokeBrush;
    glyph.TransformMatrix = this.GetTextRenderingMatrix();
    glyph.HorizontalScaling = (double) this.TextHorizontalScaling;
    glyph.Width = (double) glyphwidth;
    glyph.CharSpacing = (double) this.CharacterSpacing;
    if (glyphChar == " " && (this.CharID.BytesCount == 1 && this.CharID.Bytes[0] == (byte) 32 /*0x20*/ || this.CharID.IsEmpty))
      glyph.WordSpacing = (double) this.WordSpacing;
    Matrix identity = Matrix.Identity;
    identity.Scale(0.01, 0.01, 0.0, 0.0);
    identity.Translate(0.0, 1.0);
    this.transformations.PushTransform(identity * glyph.TransformMatrix);
    System.Drawing.Drawing2D.Matrix matrix = g.Transform.Clone();
    matrix.Multiply(this.GetTransformationMatrix(this.transformations.CurrentTransform));
    g.Transform = matrix;
    g.SmoothingMode = SmoothingMode.AntiAlias;
    if (!this.IsPdfium && path != null)
    {
      switch (this.RenderingMode)
      {
        case 0:
          g.FillPath(glyph.Stroke, path);
          break;
        case 1:
          g.DrawPath(new Pen(glyph.NonStroke, this.LineWidth), path);
          break;
        case 2:
          g.FillPath(glyph.Stroke, path);
          g.DrawPath(new Pen(glyph.Stroke, this.LineWidth), path);
          break;
        case 3:
          g.FillPath(glyph.Stroke, path);
          break;
      }
    }
    if (!this.structure.IsMappingDone)
    {
      if (this.CidToGidReverseMapTable != null && this.CidToGidReverseMapTable.ContainsKey((int) Convert.ToChar(glyphChar)) && this.structure.CharacterMapTable != null && this.structure.CharacterMapTable.Count > 0)
        glyphChar = this.CharacterMapTable[(double) this.CidToGidReverseMapTable[(int) Convert.ToChar(glyphChar)]];
      else if (this.structure.CharacterMapTable != null && this.structure.CharacterMapTable.Count > 0)
        glyphChar = this.structure.tempStringList.Count > 0 || this.structure.CharacterMapTable.Count != this.structure.ReverseMapTable.Count && this.structure.FontName == "AllAndNone" ? this.structure.CharacterMapTable[(double) Convert.ToChar(glyphChar)] : this.structure.MapCharactersFromTable(glyphChar.ToString());
      else if (this.structure.DifferencesDictionary != null && this.structure.DifferencesDictionary.Count > 0)
        glyphChar = this.structure.MapDifferences(glyphChar.ToString());
      else if (this.structure.CidToGidReverseMapTable != null && this.structure.CidToGidReverseMapTable.ContainsKey((int) Convert.ToChar(glyphChar)))
        glyphChar = ((char) this.structure.CidToGidReverseMapTable[(int) Convert.ToChar(glyphChar)]).ToString();
      if (glyphChar.Contains("\u0092"))
        glyphChar = glyphChar.Replace("\u0092", "’");
    }
    float num = glyph.TransformMatrix.M11 <= 0.0 ? (glyph.TransformMatrix.M12 == 0.0 || glyph.TransformMatrix.M21 == 0.0 ? (float) glyph.FontSize : (glyph.TransformMatrix.M12 >= 0.0 ? (float) glyph.TransformMatrix.M12 : (float) -glyph.TransformMatrix.M12)) : (float) glyph.TransformMatrix.M11;
    glyph.ToUnicode = glyphChar;
    PdfUnitConvertor pdfUnitConvertor = new PdfUnitConvertor();
    if ((double) this.pageRotation == 90.0 || (double) this.pageRotation == 270.0)
    {
      if ((double) matrix.Elements[1] == 0.0 && (double) matrix.Elements[2] == 0.0)
      {
        glyph.IsRotated = false;
        glyph.BoundingRect = new Rect(new Point((double) pdfUnitConvertor.ConvertFromPixels(matrix.OffsetX, PdfGraphicsUnit.Point) / (double) this.zoomFactor, ((double) pdfUnitConvertor.ConvertFromPixels(matrix.OffsetY, PdfGraphicsUnit.Point) - (double) pdfUnitConvertor.ConvertFromPixels((float) ((double) num * (double) this.zoomFactor * ((double) g.DpiY / 96.0)), PdfGraphicsUnit.Point)) / (double) this.zoomFactor), new Size(glyph.Width * (double) num, (double) num));
      }
      else
      {
        glyph.IsRotated = true;
        glyph.BoundingRect = !this.IsFindText || (double) this.pageRotation != 90.0 ? new Rect(new Point((double) pdfUnitConvertor.ConvertFromPixels(matrix.OffsetX + (num + (float) (glyph.Ascent / 1000.0)) * matrix.Elements[2], PdfGraphicsUnit.Point) / (double) this.zoomFactor, (double) pdfUnitConvertor.ConvertFromPixels(matrix.OffsetY + num * matrix.Elements[2], PdfGraphicsUnit.Point) / (double) this.zoomFactor), new Size((double) num, glyph.Width * (double) num)) : new Rect(new Point((double) pdfUnitConvertor.ConvertFromPixels(matrix.OffsetX + (num + (float) (glyph.Ascent / 1000.0)) * matrix.Elements[2], PdfGraphicsUnit.Point) / (double) this.zoomFactor, (double) pdfUnitConvertor.ConvertFromPixels(matrix.OffsetY - num * matrix.Elements[2], PdfGraphicsUnit.Point) / (double) this.zoomFactor), new Size((double) num, glyph.Width * (double) num));
      }
    }
    else if ((double) matrix.Elements[1] != 0.0 && (double) matrix.Elements[2] != 0.0)
    {
      glyph.IsRotated = true;
      if ((double) matrix.Elements[1] < 0.0 && (double) matrix.Elements[2] > 0.0)
        glyph.RotationAngle = 270;
      else if ((double) matrix.Elements[1] > 0.0 && (double) matrix.Elements[2] < 0.0)
        glyph.RotationAngle = 90;
      else if ((double) matrix.Elements[1] < 0.0 && (double) matrix.Elements[2] < 0.0)
        glyph.RotationAngle = 180;
      glyph.BoundingRect = !this.IsFindText || (double) this.pageRotation != 90.0 ? new Rect(new Point((double) pdfUnitConvertor.ConvertFromPixels(matrix.OffsetX + (num + (float) (glyph.Ascent / 1000.0)) * matrix.Elements[2], PdfGraphicsUnit.Point) / (double) this.zoomFactor, ((double) pdfUnitConvertor.ConvertFromPixels(matrix.OffsetY - num * matrix.Elements[2], PdfGraphicsUnit.Point) - (double) pdfUnitConvertor.ConvertFromPixels((float) ((double) num * (double) this.zoomFactor * ((double) g.DpiY / 96.0)), PdfGraphicsUnit.Point)) / (double) this.zoomFactor), new Size(glyph.Width * (double) num, (double) num)) : new Rect(new Point((double) pdfUnitConvertor.ConvertFromPixels(matrix.OffsetY, PdfGraphicsUnit.Point) / (double) this.zoomFactor, ((double) pdfUnitConvertor.ConvertFromPixels(matrix.OffsetX, PdfGraphicsUnit.Point) - (double) pdfUnitConvertor.ConvertFromPixels((float) ((double) num * (double) this.zoomFactor * ((double) g.DpiY / 96.0)), PdfGraphicsUnit.Point)) / (double) this.zoomFactor), new Size(glyph.Width * (double) num, (double) num));
    }
    else
    {
      if ((double) this.pageRotation == 180.0)
        glyph.IsRotated = (double) matrix.Elements[1] != 0.0 || (double) matrix.Elements[2] != 0.0;
      glyph.BoundingRect = new Rect(new Point((double) pdfUnitConvertor.ConvertFromPixels(matrix.OffsetX, PdfGraphicsUnit.Point) / (double) this.zoomFactor, (double) pdfUnitConvertor.ConvertFromPixels(matrix.OffsetY - (float) ((double) num * (double) this.zoomFactor * ((double) g.DpiY / 96.0)), PdfGraphicsUnit.Point) / (double) this.zoomFactor), new Size(glyph.Width * (double) num, (double) num));
    }
    if (this.structure.IsAdobeJapanFont)
    {
      if (this.structure.AdobeJapanCidMapTable.ContainsKey((int) Convert.ToChar(glyphChar)))
        glyphChar = this.structure.AdobeJapanCidMapTableGlyphParser(glyphChar);
      glyph.ToUnicode = glyphChar;
    }
    if (glyph.ToUnicode.Length != 1)
    {
      this.textElementGlyphList.Add(glyph);
      for (int index = 0; index < glyph.ToUnicode.Length - 1; ++index)
        this.textElementGlyphList.Add(new Glyph());
    }
    else
    {
      if (glyphChar.ToCharArray()[0] == ' ')
      {
        glyphChar = ' '.ToString();
        glyph.ToUnicode = glyphChar;
      }
      this.textElementGlyphList.Add(glyph);
    }
    this.GetFontSize(glyph, num);
    this.UpdateTextMatrix(glyph);
    this.transformations.PopTransform();
    g.Transform = transform;
    g.PageUnit = pageUnit;
    temptextmatrix = this.textLineMatrix;
    this.renderedText += glyphChar;
  }

  private void SetEmbededFontName(Glyph glyph)
  {
    if (glyph == null || !this.m_isExtractTextData || string.IsNullOrEmpty(this.m_embeddedFontFamily) || !(this.m_embeddedFontFamily != glyph.FontFamily))
      return;
    glyph.EmbededFontFamily = this.m_embeddedFontFamily;
  }

  private void BackupEmbededFontName(string matrixImplFontName)
  {
    if (!this.m_isExtractTextData)
      return;
    this.m_embeddedFontFamily = matrixImplFontName;
  }

  private void GetFontSize(Glyph glyph, float tempFontSize)
  {
    if (glyph == null || !this.m_isExtractTextData || (double) tempFontSize <= 0.0 || (double) tempFontSize == glyph.FontSize)
      return;
    glyph.MatrixFontSize = (double) tempFontSize;
  }

  public void DrawType3Glyphs(Image image, System.Drawing.Graphics g)
  {
    GraphicsUnit pageUnit = g.PageUnit;
    System.Drawing.Drawing2D.Matrix transform = g.Transform;
    g.PageUnit = GraphicsUnit.Pixel;
    g.Transform = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
    this.transformations.PushTransform(Matrix.Identity * new Glyph()
    {
      FontSize = ((double) this.FontSize),
      Stroke = this.PathBrush,
      TransformMatrix = this.GetTextRenderingMatrix(),
      HorizontalScaling = ((double) this.TextHorizontalScaling),
      CharSpacing = ((double) this.CharacterSpacing)
    }.TransformMatrix);
    System.Drawing.Drawing2D.Matrix matrix = g.Transform.Clone();
    matrix.Multiply(this.GetTransformationMatrix(this.transformations.CurrentTransform));
    g.Transform = matrix;
    g.SmoothingMode = SmoothingMode.AntiAlias;
    PdfUnitConvertor pdfUnitConvertor = new PdfUnitConvertor();
    g.DrawImage(image, new RectangleF(0.0f, 0.0f, 1f, 1f));
    this.transformations.PopTransform();
    g.Transform = transform;
    g.PageUnit = pageUnit;
  }

  internal string MapEscapeSequence(char letter)
  {
    switch (letter)
    {
      case '\t':
        return '\uFFFA'.ToString();
      case '\n':
        return '\uFFFB'.ToString();
      case '\v':
        return '￼'.ToString();
      case '\r':
        return '\uFFF9'.ToString();
      case ' ':
        return char.MinValue.ToString();
      default:
        return letter.ToString();
    }
  }

  internal bool isMpdfaaFonts()
  {
    bool flag = false;
    if (this.structure.FontDictionary.ContainsKey("BaseFont"))
    {
      PdfName font = this.structure.FontDictionary["BaseFont"] as PdfName;
      if (font != (PdfName) null)
      {
        string str;
        if (font.Value.Contains("+"))
          str = font.Value.Split('+')[0];
        else
          str = font.Value;
        if (str == "MPDFAA")
          flag = true;
      }
    }
    return flag;
  }

  private string SkipEscapeSequence(string text)
  {
    int startIndex = -1;
    do
    {
      startIndex = text.IndexOf("\\", startIndex + 1);
      if (startIndex >= 0)
      {
        if (text.Length > startIndex + 1)
        {
          string str = text[startIndex + 1].ToString();
          if (startIndex >= 0 && (str == "\\" || str == "(" || str == ")" || str == "\n"))
            text = text.Remove(startIndex, 1);
        }
        else
        {
          text = text.Remove(startIndex, 1);
          startIndex = -1;
        }
      }
    }
    while (startIndex >= 0);
    if (text.Contains("\n"))
      text = text.Replace("\n", "");
    return text;
  }

  internal void CheckFontStyle(string fontName)
  {
    if (fontName.Contains("Regular"))
      this.FontStyle = FontStyle.Regular;
    else if (fontName.Contains("Bold"))
    {
      this.FontStyle = FontStyle.Bold;
    }
    else
    {
      if (!fontName.Contains("Italic"))
        return;
      this.FontStyle = FontStyle.Italic;
    }
  }

  private bool IsFontInstalled(string fontName)
  {
    using (Font font = new Font(fontName, 8f))
      return 0 == string.Compare(fontName, font.Name, StringComparison.InvariantCultureIgnoreCase);
  }

  internal static string CheckFontName(string fontName)
  {
    string str1 = fontName;
    if (str1.Contains("#20"))
      str1 = str1.Replace("#20", " ");
    string[] sourceArray = new string[1]{ "" };
    int length = 0;
    for (int startIndex = 0; startIndex < str1.Length; ++startIndex)
    {
      string str2 = str1.Substring(startIndex, 1);
      if ("ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".Contains(str2) && startIndex > 0 && !"ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".Contains(str1[startIndex - 1].ToString()))
      {
        ++length;
        string[] destinationArray = new string[length + 1];
        System.Array.Copy((System.Array) sourceArray, 0, (System.Array) destinationArray, 0, length);
        sourceArray = destinationArray;
      }
      string[] strArray;
      IntPtr index;
      (strArray = sourceArray)[(int) (index = (IntPtr) length)] = strArray[index] + str2;
    }
    fontName = string.Empty;
    foreach (string str3 in sourceArray)
    {
      string str4 = str3.Trim();
      fontName = $"{fontName}{str4} ";
    }
    if (fontName.Contains("Zapf"))
      fontName = "MS Gothic";
    if (fontName.Contains("Times"))
      fontName = "Times New Roman";
    if (fontName == "Bookshelf Symbol Seven")
      fontName = "Bookshelf Symbol 7";
    if (fontName.Contains("Courier"))
      fontName = "Courier New";
    if (fontName.Contains("Song Std"))
      fontName = "Adobe Song Std L";
    if (fontName.Contains("Free") && fontName.Contains("9"))
      fontName = "Free 3 of 9";
    if (fontName.Contains("Regular"))
      fontName = fontName.Replace("Regular", "");
    else if (fontName.Contains("Bold"))
      fontName = fontName.Replace("Bold", "");
    else if (fontName.Contains("Italic"))
      fontName = fontName.Replace("Italic", "");
    fontName = fontName.Trim();
    return fontName;
  }
}
