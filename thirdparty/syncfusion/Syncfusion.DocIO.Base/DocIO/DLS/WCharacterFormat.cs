// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WCharacterFormat
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using Syncfusion.DocIO.ReaderWriter;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.Office;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WCharacterFormat : FormatBase, IOfficeRunFormat
{
  internal const string DEF_FONTFAMILY = "Times New Roman";
  internal const float DEF_FONTSIZE = 10f;
  internal const float DEF_SCALINGSIZE = 100f;
  internal const short FontKey = 0;
  internal const short TextColorKey = 1;
  internal const short FontNameKey = 2;
  internal const short FontSizeKey = 3;
  internal const short BoldKey = 4;
  internal const short ItalicKey = 5;
  internal const short StrikeKey = 6;
  internal const short UnderlineKey = 7;
  internal const short TextBkgColorKey = 9;
  internal const short SubSuperScriptKey = 10;
  internal const short DoubleStrikeKey = 14;
  internal const short AllCapsKey = 54;
  internal const short PositionKey = 17;
  internal const short SpacingKey = 18;
  internal const short LineBreakKey = 20;
  internal const short ShadowKey = 50;
  internal const short EmbossKey = 51;
  internal const short EngraveKey = 52;
  internal const short HiddenKey = 53;
  internal const short SmallCapsKey = 55;
  internal const short SpecVanishKey = 24;
  internal const short BidiKey = 58;
  internal const short BoldBidiKey = 59;
  internal const short ItalicBidiKey = 60;
  internal const short FontNameBidiKey = 61;
  internal const short FontSizeBidiKey = 62;
  internal const short HighlightColorKey = 63 /*0x3F*/;
  internal const short BorderKey = 67;
  internal const short FontNameAsciiKey = 68;
  internal const short FontNameFarEastKey = 69;
  internal const short FontNameNonFarEastKey = 70;
  internal const short OutlineKey = 71;
  internal const short IdctHintKey = 72;
  internal const short LocaleIdASCIIKey = 73;
  internal const short LocaleIdFarEastKey = 74;
  internal const short LidBiKey = 75;
  internal const short NoProofKey = 76;
  internal const short ForeColorKey = 77;
  internal const short TextureStyleKey = 78;
  internal const short FieldVanishKey = 109;
  internal const short EmphasisKey = 79;
  internal const short TextEffectkey = 80 /*0x50*/;
  internal const short SnapToGridKey = 81;
  internal const short UnderlineColorKey = 90;
  internal const short CharStyleNameKey = 91;
  internal const short WebHiddenKey = 92;
  internal const short ComplexScriptKey = 99;
  internal const short InserteRevisionKey = 103;
  internal const short DeleteRevisionKey = 104;
  internal const short ChangedFormatKey = 105;
  internal const short SpecialKey = 106;
  internal const short ListPicIndexKey = 107;
  internal const short ListHasPicKey = 108;
  internal const short ContextualAlternatesKey = 120;
  internal const short LigaturesKey = 121;
  internal const short NumberFormKey = 122;
  internal const short NumberSpacingKey = 123;
  internal const short StylisticSetKey = 124;
  internal const short KernKey = 125;
  internal const short BreakClearKey = 126;
  internal const short ScalingKey = 127 /*0x7F*/;
  internal const short AuthorNameKey = 8;
  internal const short FormatChangeAuthorNameKey = 12;
  internal const short DateTimeKey = 11;
  internal const short FormatChangeDateTimeKey = 15;
  internal const short RevisionNameKey = 128 /*0x80*/;
  internal const short CFELayoutKey = 13;
  internal const short FitTextKey = 16 /*0x10*/;
  internal const short FitTextIDKey = 19;
  protected string m_charStyleName;
  protected string m_symExFontName;
  private WCharacterFormat m_tableStyleCharacterFormat;
  private byte m_bFlags;
  private float m_reducedFontSize;
  private List<Stream> m_xmlProps;
  private BiDirectionalOverride m_biDirectionalOverride;

  private bool CancelOnChange
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal bool CrossRefChecked
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  internal float ReducedFontSize
  {
    get => this.m_reducedFontSize;
    set => this.m_reducedFontSize = value;
  }

  internal string SymExFontName
  {
    get => this.m_symExFontName;
    set => this.m_symExFontName = value;
  }

  internal bool IsDocReading
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 247 | (value ? 1 : 0) << 3);
  }

  public Font Font
  {
    get
    {
      FontStyle fontStyle = FontStyle.Regular;
      float fontSize = (double) this.FontSize == 0.0 ? 0.5f : this.FontSize;
      if (this.Bold)
        fontStyle |= FontStyle.Bold;
      if (this.Italic)
        fontStyle |= FontStyle.Italic;
      if (this.UnderlineStyle != UnderlineStyle.None)
        fontStyle |= FontStyle.Underline;
      if (this.Strikeout)
        fontStyle |= FontStyle.Strikeout;
      try
      {
        return this.Document.FontSettings.GetFont(this.SymExFontName != null ? this.SymExFontName : this.FontName, fontSize, fontStyle);
      }
      catch (Exception ex)
      {
        FontFamily fontFamily = new FontFamily(this.FontName);
        if (fontFamily.IsStyleAvailable(FontStyle.Bold))
          fontStyle |= FontStyle.Bold;
        if (fontFamily.IsStyleAvailable(FontStyle.Italic))
          fontStyle |= FontStyle.Italic;
        if (fontFamily.IsStyleAvailable(FontStyle.Underline))
          fontStyle |= FontStyle.Underline;
        if (fontFamily.IsStyleAvailable(FontStyle.Strikeout))
          fontStyle |= FontStyle.Strikeout;
        return this.Document.FontSettings.GetFont(this.SymExFontName != null ? this.SymExFontName : this.FontName, fontSize, fontStyle);
      }
    }
    set
    {
      this.FontName = value.Name;
      this.SetPropertyValue(3, (object) value.SizeInPoints);
      this.Bold = value.Bold;
      this.Italic = value.Italic;
      this.Strikeout = value.Strikeout;
      this.UnderlineStyle = value.Underline ? UnderlineStyle.Single : UnderlineStyle.None;
    }
  }

  public string FontName
  {
    get => this.GetFontName((short) 2);
    set
    {
      this[2] = (object) value;
      if (!this.Document.IsOpening || !this.HasValue(68))
        this.FontNameAscii = value;
      if (!this.Document.IsOpening && !this.HasValue(61))
        this.FontNameBidi = value;
      if (!this.Document.IsOpening)
        this.FontNameFarEast = value;
      if (!this.Document.IsOpening || !this.HasValue(70))
        this.FontNameNonFarEast = value;
      this.CheckCrossRef();
      this.UpdateUsedFontsCollection();
    }
  }

  internal BreakClearType BreakClear
  {
    get => (BreakClearType) this.GetPropertyValue(126);
    set => this.SetPropertyValue(126, (object) value);
  }

  public float FontSize
  {
    get => (float) this.GetPropertyValue(3);
    set
    {
      if ((double) value < 0.0 || (double) value > 1638.0)
        throw new ArgumentException("FontSize must be between 0 and 1638");
      this.SetPropertyValue(3, (object) value);
    }
  }

  internal float Scaling
  {
    get => (float) this.GetPropertyValue((int) sbyte.MaxValue);
    set => this.SetPropertyValue((int) sbyte.MaxValue, (object) value);
  }

  internal float Kern
  {
    get => (float) this.GetPropertyValue(125);
    set => this.SetPropertyValue(125, (object) value);
  }

  internal bool IsKernFont
  {
    get
    {
      float kern = this.Kern;
      return (double) kern > 0.0 && (double) kern <= (double) this.FontSize;
    }
  }

  public bool ComplexScript
  {
    get => this.GetBoolPropertyValue((short) 99);
    set => this.SetPropertyValue(99, (object) value);
  }

  public bool Bold
  {
    get => this.GetBoolPropertyValue((short) 4);
    set => this.SetPropertyValue(4, (object) value);
  }

  public bool Italic
  {
    get => this.GetBoolPropertyValue((short) 5);
    set => this.SetPropertyValue(5, (object) value);
  }

  public bool Strikeout
  {
    get => this.GetBoolPropertyValue((short) 6);
    set
    {
      if (value && this.DoubleStrike)
        this.DoubleStrike = false;
      this.SetPropertyValue(6, (object) value);
    }
  }

  public bool DoubleStrike
  {
    get => this.GetBoolPropertyValue((short) 14);
    set
    {
      if (value && this.Strikeout)
        this.Strikeout = false;
      this.SetPropertyValue(14, (object) value);
    }
  }

  public UnderlineStyle UnderlineStyle
  {
    get => (UnderlineStyle) this.GetPropertyValue(7);
    set
    {
      if (value == UnderlineStyle.DotDot)
        value = UnderlineStyle.None;
      if (value.ToString().Length <= 3)
        return;
      this.SetPropertyValue(7, (object) value);
    }
  }

  internal Color UnderlineColor
  {
    get => (Color) this.GetPropertyValue(90);
    set => this.SetPropertyValue(90, (object) value);
  }

  public Color TextColor
  {
    get => (Color) this.GetPropertyValue(1);
    set => this.SetPropertyValue(1, (object) value);
  }

  public Color TextBackgroundColor
  {
    get => (Color) this.GetPropertyValue(9);
    set => this.SetPropertyValue(9, (object) value);
  }

  public SubSuperScript SubSuperScript
  {
    get => (SubSuperScript) this.GetPropertyValue(10);
    set => this.SetPropertyValue(10, (object) value);
  }

  public float CharacterSpacing
  {
    get => (float) this.GetPropertyValue(18);
    set
    {
      if ((double) value < -1584.0 || (double) value > 1584.0)
        throw new ArgumentException("CharacterSpacing must be between -1584 and 1584");
      this.SetPropertyValue(18, (object) value);
    }
  }

  public float Position
  {
    get => (float) this.GetPropertyValue(17);
    set
    {
      if ((double) value < -1584.0 || (double) value > 1584.0)
        throw new ArgumentException("Position must be between -1584 and 1584");
      this.SetPropertyValue(17, (object) value);
    }
  }

  internal bool LineBreak
  {
    get => this.IsLineBreakNext();
    set => this.SetLineBreakNext();
  }

  public bool Shadow
  {
    get => this.GetBoolPropertyValue((short) 50);
    set
    {
      if (value && (this.HasValue(51) && this.Emboss || this.HasValue(52) && this.Engrave))
      {
        this.Emboss = false;
        this.Engrave = false;
      }
      this.SetPropertyValue(50, (object) value);
    }
  }

  public bool Emboss
  {
    get => this.GetBoolPropertyValue((short) 51);
    set
    {
      if (value && (this.HasValue(50) && this.Shadow || this.HasValue(71) && this.OutLine || this.HasValue(52) && this.Engrave))
      {
        this.Shadow = false;
        this.OutLine = false;
        this.Engrave = false;
      }
      this.SetPropertyValue(51, (object) value);
    }
  }

  public bool Engrave
  {
    get => this.GetBoolPropertyValue((short) 52);
    set
    {
      if (value && (this.HasValue(50) && this.Shadow || this.HasValue(71) && this.OutLine || this.HasValue(51) && this.Emboss))
      {
        this.Shadow = false;
        this.OutLine = false;
        this.Emboss = false;
      }
      this.SetPropertyValue(52, (object) value);
    }
  }

  public bool Hidden
  {
    get => this.GetBoolPropertyValue((short) 53);
    set => this.SetPropertyValue(53, (object) value);
  }

  public bool AllCaps
  {
    get => this.GetBoolPropertyValue((short) 54);
    set
    {
      if (value && this.SmallCaps && !this.Document.IsOpening)
        this.SmallCaps = false;
      this.SetPropertyValue(54, (object) value);
    }
  }

  public bool SmallCaps
  {
    get => this.GetBoolPropertyValue((short) 55);
    set
    {
      if (value && this.AllCaps && !this.Document.IsOpening)
        this.AllCaps = false;
      this.SetPropertyValue(55, (object) value);
    }
  }

  internal bool SpecVanish
  {
    get => this.GetBoolPropertyValue((short) 24);
    set => this.SetPropertyValue(24, (object) value);
  }

  internal BiDirectionalOverride BiDirectionalOverride
  {
    get => this.m_biDirectionalOverride;
    set => this.m_biDirectionalOverride = value;
  }

  public bool Bidi
  {
    get => this.GetBoolPropertyValue((short) 58);
    set => this.SetPropertyValue(58, (object) value);
  }

  public bool BoldBidi
  {
    get => this.GetBoolPropertyValue((short) 59);
    set => this.SetPropertyValue(59, (object) value);
  }

  public bool ItalicBidi
  {
    get => this.GetBoolPropertyValue((short) 60);
    set => this.SetPropertyValue(60, (object) value);
  }

  public float FontSizeBidi
  {
    get => (float) this.GetPropertyValue(62);
    set
    {
      if ((double) value < 0.0 || (double) value > 1638.0)
        throw new ArgumentException("FontSizeBi must be between 0 and 1638");
      this.SetPropertyValue(62, (object) value);
    }
  }

  public string FontNameBidi
  {
    get => (string) this[61];
    set
    {
      this[61] = (object) value;
      this.CheckCrossRef();
    }
  }

  public Color HighlightColor
  {
    get => (Color) this.GetPropertyValue(63 /*0x3F*/);
    set => this.SetPropertyValue(63 /*0x3F*/, (object) value);
  }

  public Border Border => this.GetPropertyValue(67) as Border;

  internal EmphasisType EmphasisType
  {
    get => (EmphasisType) this.GetPropertyValue(79);
    set => this.SetPropertyValue(79, (object) value);
  }

  internal TextEffect TextEffect
  {
    get => (TextEffect) this.GetPropertyValue(80 /*0x50*/);
    set => this.SetPropertyValue(80 /*0x50*/, (object) value);
  }

  internal bool SnapToGrid
  {
    get => this.GetBoolPropertyValue((short) 81);
    set => this.SetPropertyValue(81, (object) value);
  }

  internal bool WebHidden
  {
    get => this.GetBoolPropertyValue((short) 92);
    set => this.SetPropertyValue(92, (object) value);
  }

  internal string FontNameAscii
  {
    get => (string) this[68];
    set => this[68] = (object) value;
  }

  internal string FontNameFarEast
  {
    get => (string) this[69];
    set => this[69] = (object) value;
  }

  internal string FontNameNonFarEast
  {
    get => (string) this[70];
    set => this[70] = (object) value;
  }

  internal FontHintType IdctHint
  {
    get => (FontHintType) Convert.ToInt16(this.GetPropertyValue(72));
    set => this.SetPropertyValue(72, (object) value);
  }

  public short LocaleIdASCII
  {
    get => (short) this.GetPropertyValue(73);
    set => this.SetPropertyValue(73, (object) value);
  }

  public short LocaleIdFarEast
  {
    get => (short) this.GetPropertyValue(74);
    set => this.SetPropertyValue(74, (object) value);
  }

  public short LocaleIdBidi
  {
    get => (short) this.GetPropertyValue(75);
    set => this.SetPropertyValue(75, (object) value);
  }

  internal bool NoProof
  {
    get => this.GetBoolPropertyValue((short) 76);
    set => this.SetPropertyValue(76, (object) value);
  }

  internal Color ForeColor
  {
    get => (Color) this.GetPropertyValue(77);
    set => this.SetPropertyValue(77, (object) value);
  }

  internal TextureStyle TextureStyle
  {
    get => (TextureStyle) this.GetPropertyValue(78);
    set => this.SetPropertyValue(78, (object) value);
  }

  public bool OutLine
  {
    get => this.GetBoolPropertyValue((short) 71);
    set
    {
      if (value && (this.Emboss || this.Engrave))
      {
        this.Emboss = false;
        this.Engrave = false;
      }
      this.SetPropertyValue(71, (object) value);
    }
  }

  internal bool Special
  {
    get => this.GetBoolPropertyValue((short) 106);
    set => this.SetPropertyValue(106, (object) value);
  }

  public string CharStyleName
  {
    get => (string) this.GetPropertyValue(91);
    internal set
    {
      if (this.OwnerBase != null)
      {
        if (this.OwnerBase.Document.Styles.FindByName(this.CharStyleName) is WCharacterStyle byName1 && !byName1.IsRemoving && byName1.IsCustom && byName1.RangeCollection.Contains(this.OwnerBase as Entity))
          byName1.RangeCollection.Remove(this.OwnerBase as Entity);
        if (this.OwnerBase.Document.Styles.FindByName(value) is WCharacterStyle byName2 && byName2.IsCustom)
          byName2.RangeCollection.Add(this.OwnerBase as Entity);
      }
      this.SetPropertyValue(91, (object) value);
    }
  }

  internal string CharStyleId
  {
    get => this.m_charStyleName;
    set => this.m_charStyleName = value;
  }

  internal bool IsInsertRevision
  {
    get => this.GetBoolPropertyValue((short) 103);
    set => this.SetPropertyValue(103, (object) value);
  }

  internal bool IsDeleteRevision
  {
    get => this.GetBoolPropertyValue((short) 104);
    set => this.SetPropertyValue(104, (object) value);
  }

  internal bool IsChangedFormat
  {
    get => (bool) this.GetPropertyValue(105);
    set
    {
      if (!value)
        return;
      this.SetPropertyValue(105, (object) value);
    }
  }

  internal int ListPictureIndex
  {
    get => (int) this.GetPropertyValue(107);
    set => this.SetPropertyValue(107, (object) value);
  }

  internal bool ListHasPicture
  {
    get => (bool) this.GetPropertyValue(108);
    set => this.SetPropertyValue(108, (object) value);
  }

  internal WCharacterStyle CharStyle => this.GetCharStyleValue();

  internal bool FieldVanish
  {
    get => this.GetBoolPropertyValue((short) 109);
    set => this.SetPropertyValue(109, (object) value);
  }

  internal WCharacterFormat TableStyleCharacterFormat
  {
    get => this.m_tableStyleCharacterFormat;
    set => this.m_tableStyleCharacterFormat = value;
  }

  internal string AuthorName
  {
    get => (string) this.GetPropertyValue(8);
    set => this.SetPropertyValue(8, (object) value);
  }

  internal string FormatChangeAuthorName
  {
    get => (string) this.GetPropertyValue(12);
    set => this.SetPropertyValue(12, (object) value);
  }

  internal DateTime RevDateTime
  {
    get => (DateTime) this.GetPropertyValue(11);
    set => this.SetPropertyValue(11, (object) value);
  }

  internal string RevisionName
  {
    get => (string) this.GetPropertyValue(128 /*0x80*/);
    set => this.SetPropertyValue(128 /*0x80*/, (object) value);
  }

  internal DateTime FormatChangeDateTime
  {
    get => (DateTime) this.GetPropertyValue(15);
    set => this.SetPropertyValue(15, (object) value);
  }

  internal CFELayout CFELayout
  {
    get => (CFELayout) this.GetPropertyValue(13);
    set => this.SetPropertyValue(13, (object) value);
  }

  internal int FitTextWidth
  {
    get => (int) this.GetPropertyValue(16 /*0x10*/);
    set => this.SetPropertyValue(16 /*0x10*/, (object) value);
  }

  internal int FitTextID
  {
    get => (int) this.GetPropertyValue(19);
    set => this.SetPropertyValue(19, (object) value);
  }

  public bool UseContextualAlternates
  {
    get => (bool) this.GetPropertyValue(120);
    set => this.SetPropertyValue(120, (object) value);
  }

  public LigatureType Ligatures
  {
    get => (LigatureType) this.GetPropertyValue(121);
    set => this.SetPropertyValue(121, (object) value);
  }

  public NumberFormType NumberForm
  {
    get => (NumberFormType) this.GetPropertyValue(122);
    set => this.SetPropertyValue(122, (object) value);
  }

  public NumberSpacingType NumberSpacing
  {
    get => (NumberSpacingType) this.GetPropertyValue(123);
    set => this.SetPropertyValue(123, (object) value);
  }

  public StylisticSetType StylisticSet
  {
    get => (StylisticSetType) this.GetPropertyValue(124);
    set => this.SetPropertyValue(124, (object) value);
  }

  internal List<Stream> XmlProps
  {
    get
    {
      if (this.m_xmlProps == null)
        this.m_xmlProps = new List<Stream>();
      return this.m_xmlProps;
    }
  }

  private WCharacterFormat()
  {
  }

  public WCharacterFormat(IWordDocument doc)
    : base(doc)
  {
  }

  internal WCharacterFormat(IWordDocument doc, Entity owner)
    : base(doc, owner)
  {
  }

  internal void RemoveFontNames()
  {
    this.PropertiesHash.Remove(2);
    this.PropertiesHash.Remove(68);
    this.PropertiesHash.Remove(61);
    this.PropertiesHash.Remove(69);
    this.PropertiesHash.Remove(70);
  }

  internal bool HasValueWithParent(int propertyKey)
  {
    bool flag = this.HasValue(propertyKey);
    if (!flag && this.BaseFormat != null && this.BaseFormat is WCharacterFormat)
      flag = (this.BaseFormat as WCharacterFormat).HasValueWithParent(propertyKey);
    if (!flag && this != null && this.CharStyle != null)
      flag = this.CharStyle.CharacterFormat.HasValue(propertyKey);
    if (!flag && this.Document != null && this.Document.DefCharFormat != null && this.Document.DefCharFormat != this)
      flag = this.Document.DefCharFormat.HasValue(propertyKey);
    return flag;
  }

  internal Font GetFontToRender(FontScriptType scriptType)
  {
    FontStyle fontStyle = FontStyle.Regular;
    float fontSize = (double) this.GetFontSizeToRender() == 0.0 ? 0.5f : this.GetFontSizeToRender();
    if (this.GetBoldToRender())
      fontStyle |= FontStyle.Bold;
    if (this.GetItalicToRender())
      fontStyle |= FontStyle.Italic;
    if (this.UnderlineStyle != UnderlineStyle.None)
      fontStyle |= FontStyle.Underline;
    if (this.Strikeout)
      fontStyle |= FontStyle.Strikeout;
    try
    {
      return this.Document.FontSettings.GetFont(this.GetFontNameToRender(scriptType), fontSize, fontStyle);
    }
    catch (Exception ex)
    {
      FontFamily fontFamily = new FontFamily(this.GetFontNameToRender(scriptType));
      if (fontFamily.IsStyleAvailable(FontStyle.Bold))
        fontStyle |= FontStyle.Bold;
      if (fontFamily.IsStyleAvailable(FontStyle.Italic))
        fontStyle |= FontStyle.Italic;
      if (fontFamily.IsStyleAvailable(FontStyle.Underline))
        fontStyle |= FontStyle.Underline;
      if (fontFamily.IsStyleAvailable(FontStyle.Strikeout))
        fontStyle |= FontStyle.Strikeout;
      return this.Document.FontSettings.GetFont(this.GetFontNameToRender(scriptType), fontSize, fontStyle);
    }
  }

  internal float GetFontSizeToRender()
  {
    if ((double) this.ReducedFontSize != 0.0)
      return this.ReducedFontSize;
    return this.Bidi || this.ComplexScript ? this.FontSizeBidi : this.FontSize;
  }

  internal bool GetBoldToRender() => this.Bidi || this.ComplexScript ? this.BoldBidi : this.Bold;

  internal bool GetItalicToRender()
  {
    return this.Bidi || this.ComplexScript ? this.ItalicBidi : this.Italic;
  }

  internal FontStyle GetFontStyle()
  {
    FontStyle fontStyle = FontStyle.Regular;
    if (this.GetBoldToRender())
      fontStyle |= FontStyle.Bold;
    if (this.GetItalicToRender())
      fontStyle |= FontStyle.Italic;
    if (this.Document.RevisionOptions.ShowRevisionMarks)
    {
      if (this.IsInsertRevision && this.IsNeedToShowInsertionMarkups())
        fontStyle |= this.GetTextEffect(this.Document.RevisionOptions.InsertedTextEffect);
      else if (this.IsDeleteRevision && this.IsNeedToShowDeletionMarkups())
        fontStyle |= this.GetTextEffect(this.Document.RevisionOptions.DeletedTextEffect);
    }
    return fontStyle;
  }

  private FontStyle GetTextEffect(RevisedTextEffect effect)
  {
    FontStyle textEffect = FontStyle.Regular;
    switch (effect)
    {
      case RevisedTextEffect.Bold:
        textEffect = FontStyle.Bold;
        break;
      case RevisedTextEffect.Italic:
        textEffect = FontStyle.Italic;
        break;
      case RevisedTextEffect.Underline:
        textEffect = FontStyle.Underline;
        break;
      case RevisedTextEffect.StrikeThrough:
        textEffect = FontStyle.Strikeout;
        break;
    }
    return textEffect;
  }

  internal bool IsNeedToShowInsertionMarkups()
  {
    return this.Document != null && (this.Document.RevisionOptions.ShowMarkup & RevisionType.Insertions) == RevisionType.Insertions;
  }

  internal bool IsNeedToShowDeletionMarkups()
  {
    return this.Document != null && (this.Document.RevisionOptions.ShowMarkup & RevisionType.Deletions) == RevisionType.Deletions && this.Document.RevisionOptions.ShowDeletedText;
  }

  private bool IsLineBreakNext()
  {
    bool flag = false;
    OwnerHolder ownerBase1 = this.OwnerBase;
    if (ownerBase1 != null && ownerBase1.OwnerBase is WParagraph)
    {
      WParagraph ownerBase2 = ownerBase1.OwnerBase as WParagraph;
      int num = ownerBase2.Items.IndexOf(ownerBase1 as IEntity);
      if (num < ownerBase2.Items.Count - 1 && ownerBase2.Items[num + 1] is Break)
        flag = (ownerBase2.Items[num + 1] as Break).BreakType == BreakType.LineBreak;
    }
    return flag;
  }

  private void SetLineBreakNext()
  {
    OwnerHolder ownerBase1 = this.OwnerBase;
    if (ownerBase1 == null || !(ownerBase1.OwnerBase is WParagraph))
      return;
    WParagraph ownerBase2 = ownerBase1.OwnerBase as WParagraph;
    int index = ownerBase2.Items.IndexOf(ownerBase1 as IEntity) + 1;
    ownerBase2.Items.Insert(index, (IEntity) new Break((IWordDocument) ownerBase2.Document, BreakType.LineBreak));
  }

  internal object GetPropertyValue(int propKey) => this[propKey];

  internal bool GetBoolPropertyValue(short propKey)
  {
    bool flag = this.HasKey((int) propKey);
    bool complexBoolValue = this.GetComplexBoolValue((int) propKey);
    if (!flag)
    {
      for (FormatBase baseFormat = this.BaseFormat; baseFormat != null && baseFormat is WCharacterFormat; baseFormat = baseFormat.BaseFormat)
      {
        flag = baseFormat.HasKey((int) propKey);
        if (flag)
          break;
      }
    }
    return !flag && this.m_tableStyleCharacterFormat != null && this.m_tableStyleCharacterFormat.GetBoolPropertyValue(propKey) ? !complexBoolValue : complexBoolValue;
  }

  internal void SetPropertyValue(int propKey, object value)
  {
    if (this.IsBooleanProperty(propKey))
      value = this.GetComplexBoolValue(value);
    this[propKey] = value;
    this.OnStateChange((object) this);
  }

  private bool IsBooleanProperty(int key)
  {
    switch (key)
    {
      case 4:
      case 5:
      case 6:
      case 14:
      case 24:
      case 50:
      case 51:
      case 52:
      case 53:
      case 54:
      case 55:
      case 58:
      case 59:
      case 60:
      case 71:
      case 76:
      case 81:
      case 92:
      case 99:
      case 103:
      case 104:
      case 106:
      case 109:
        return true;
      default:
        return false;
    }
  }

  private bool SerializeAllData() => this.Document != null && this.m_sprms == null;

  internal bool GetComplexBoolValue(int optionKey)
  {
    byte num = 0;
    object obj = (object) null;
    if (this.HasKey(optionKey))
      obj = this.PropertiesHash[this.GetFullKey(optionKey)];
    else
      num = byte.MaxValue;
    if (obj is ToggleOperand)
      num = (byte) obj;
    if (num == (byte) 1)
      return true;
    if (num == (byte) 0)
      return false;
    bool flag = false;
    if (this.BaseFormat is WCharacterFormat)
      flag = (this.BaseFormat as WCharacterFormat).GetComplexBoolValue(optionKey);
    if (this.BaseFormat == null && this.Document != null && this != this.Document.DefCharFormat && this.Document.DefCharFormat != null)
      flag = this.Document.DefCharFormat.GetComplexBoolValue(optionKey);
    if (this.Document != null && this.Document.Styles != null)
    {
      Style style = this.CharStyleId != null ? (this.Document.Styles as StyleCollection).FindStyleById(this.CharStyleId) : this.Document.Styles.FindByName(this.CharStyleName, StyleType.CharacterStyle) as Style;
      if (style != null)
      {
        try
        {
          if (this.IsDocReading && style.CharacterFormat.PropertiesHash.ContainsKey(optionKey) && (ToggleOperand) style.CharacterFormat.PropertiesHash[optionKey] == ToggleOperand.True)
          {
            flag = true;
          }
          else
          {
            bool complexBoolValue = style.CharacterFormat.GetComplexBoolValue(optionKey);
            flag = flag != complexBoolValue;
          }
        }
        catch
        {
          bool complexBoolValue = style.CharacterFormat.GetComplexBoolValue(optionKey);
          flag = flag != complexBoolValue;
        }
      }
    }
    return num == (byte) 129 ? !flag : flag;
  }

  private WCharacterFormat GetBaseFormat(WCharacterFormat format)
  {
    if (format == null)
      return (WCharacterFormat) null;
    return format.CharStyleName != null && this.Document.Styles.FindByName(format.CharStyleName, StyleType.CharacterStyle) is WCharacterStyle byName ? byName.CharacterFormat : format.BaseFormat as WCharacterFormat;
  }

  internal override void RemoveChanges()
  {
    this.CheckCrossRef();
    base.RemoveChanges();
  }

  internal override void AcceptChanges()
  {
    this[104] = (object) false;
    this[103] = (object) false;
    this[105] = (object) false;
    if (this.OldPropertiesHash.Count <= 0)
      return;
    this.OldPropertiesHash.Clear();
  }

  internal void CheckCrossRef()
  {
    if (this.Document.IsOpening || this.m_sprms == null || this.CrossRefChecked)
      return;
    this.CrossRefChecked = true;
  }

  private WCharacterStyle GetCharStyleValue()
  {
    WCharacterStyle charStyleValue = (WCharacterStyle) null;
    string name = this.PropertiesHash.ContainsKey(91) ? this.PropertiesHash[91] as string : (string) null;
    if (!string.IsNullOrEmpty(name) && this.Document != null)
      charStyleValue = this.CharStyleId != null ? (this.Document.Styles as StyleCollection).FindStyleById(this.CharStyleId) as WCharacterStyle : this.Document.Styles.FindByName(name, StyleType.CharacterStyle) as WCharacterStyle;
    return charStyleValue;
  }

  internal string GetFontName(short fontKey) => (string) this[(int) fontKey];

  internal string GetFontNameToRender(FontScriptType scriptType)
  {
    if (this.Bidi || this.ComplexScript)
      return this.GetFontNameBidiToRender(scriptType);
    if (TextSplitter.IsEastAsiaScript(scriptType))
      return this.GetFontNameEAToRender(scriptType);
    return this.SymExFontName == null ? this.FontName : this.SymExFontName;
  }

  internal string GetFontNameBidiToRender(FontScriptType scriptType)
  {
    string fontNameBidi = this.FontNameBidi;
    return string.IsNullOrEmpty(fontNameBidi) || this.IsThemeFont(fontNameBidi) ? this.GetFontNameFromTheme(fontNameBidi, scriptType, FontHintType.CS) : fontNameBidi;
  }

  private string GetFontNameEAToRender(FontScriptType scriptType)
  {
    string fontNameFarEast = this.FontNameFarEast;
    return string.IsNullOrEmpty(fontNameFarEast) || this.IsThemeFont(fontNameFarEast) ? this.GetFontNameFromTheme(fontNameFarEast, scriptType, FontHintType.EastAsia) : fontNameFarEast;
  }

  private string GetFontNameFromTheme(
    string fontName,
    FontScriptType scriptType,
    FontHintType hintType)
  {
    FontScheme fontScheme = (FontScheme) null;
    if (this.Document != null && this.Document.DocHasThemes && this.Document.Themes != null && this.Document.Themes.FontScheme != null)
      fontScheme = this.Document.Themes.FontScheme;
    switch (fontName)
    {
      case "majorAscii":
      case "majorBidi":
      case "majorEastAsia":
      case "majorHAnsi":
        MajorMinorFontScheme majorMinorFontScheme1 = (MajorMinorFontScheme) null;
        if (fontScheme != null && fontScheme.MajorFontScheme != null)
          majorMinorFontScheme1 = fontScheme.MajorFontScheme;
        this.UpdateFontNameFromTheme(majorMinorFontScheme1, scriptType, ref fontName, hintType);
        break;
      case "minorAscii":
      case "minorBidi":
      case "minorEastAsia":
      case "minorHAnsi":
        MajorMinorFontScheme majorMinorFontScheme2 = (MajorMinorFontScheme) null;
        if (fontScheme != null && fontScheme.MajorFontScheme != null)
          majorMinorFontScheme2 = fontScheme.MinorFontScheme;
        this.UpdateFontNameFromTheme(majorMinorFontScheme2, scriptType, ref fontName, hintType);
        break;
    }
    if (string.IsNullOrEmpty(fontName) || this.IsThemeFont(fontName))
      fontName = "Times New Roman";
    return fontName;
  }

  private void UpdateFontNameFromTheme(
    MajorMinorFontScheme majorMinorFontScheme,
    FontScriptType scriptType,
    ref string fontName,
    FontHintType hintType)
  {
    string str = "";
    if (majorMinorFontScheme != null && majorMinorFontScheme.FontSchemeList != null && majorMinorFontScheme.FontSchemeList.Count > 0)
    {
      foreach (FontSchemeStruct fontScheme in majorMinorFontScheme.FontSchemeList)
      {
        if (fontScheme.Name == "cs" && (fontName == "majorBidi" || fontName == "minorBidi"))
          str = fontScheme.Typeface;
        else if (fontScheme.Name == "ea" && (fontName == "majorEastAsia" || fontName == "minorEastAsia"))
          str = fontScheme.Typeface;
        else if (fontScheme.Name == "latin" && (fontName == "majorAscii" || fontName == "majorHAnsi" || fontName == "minorAscii" || fontName == "minorHAnsi"))
          str = fontScheme.Typeface;
      }
    }
    if (majorMinorFontScheme != null && majorMinorFontScheme.FontTypeface != null)
    {
      switch (hintType)
      {
        case FontHintType.EastAsia:
          if (this.Document != null && this.Document.Settings.ThemeFontLanguages != null && this.Document.Settings.ThemeFontLanguages.HasValue(74))
          {
            fontName = this.GetFontNameWithFontScript(majorMinorFontScheme, this.Document.Settings.ThemeFontLanguages.LocaleIdFarEast, hintType);
            if (fontName != null)
            {
              str = fontName;
              break;
            }
            break;
          }
          break;
        case FontHintType.CS:
          if (this.ComplexScript && this.Document != null && this.Document.Settings.ThemeFontLanguages != null && this.Document.Settings.ThemeFontLanguages.HasValue(75))
          {
            fontName = this.GetFontNameWithFontScript(majorMinorFontScheme, this.Document.Settings.ThemeFontLanguages.LocaleIdBidi, hintType);
            if (fontName != null)
            {
              str = fontName;
              break;
            }
            break;
          }
          if (majorMinorFontScheme.FontTypeface.ContainsKey("Arab"))
          {
            str = majorMinorFontScheme.FontTypeface["Arab"];
            break;
          }
          break;
      }
    }
    if (string.IsNullOrEmpty(str))
      str = "Times New Roman";
    fontName = str;
  }

  private string GetFontNameWithFontScript(
    MajorMinorFontScheme majorMinorFontScheme,
    short localeID,
    FontHintType hintType)
  {
    string nameWithFontScript = (string) null;
    LocaleIDs localeIds = (LocaleIDs) localeID;
    if ((localeIds == LocaleIDs.gu_IN || localeID == (short) 71) && majorMinorFontScheme.FontTypeface.ContainsKey("Gujr"))
      nameWithFontScript = majorMinorFontScheme.FontTypeface["Gujr"];
    else if ((localeIds == LocaleIDs.hi_IN || localeIds == LocaleIDs.mr_IN || localeID == (short) 57 || localeID == (short) 78) && majorMinorFontScheme.FontTypeface.ContainsKey("Deva"))
      nameWithFontScript = majorMinorFontScheme.FontTypeface["Deva"];
    else if ((localeIds == LocaleIDs.ko_KR || localeID == (short) 18) && majorMinorFontScheme.FontTypeface.ContainsKey("Hang"))
      nameWithFontScript = majorMinorFontScheme.FontTypeface["Hang"];
    else if ((localeIds == LocaleIDs.zh_CN || localeIds == LocaleIDs.zh_SG || localeID == (short) 4) && majorMinorFontScheme.FontTypeface.ContainsKey("Hans"))
      nameWithFontScript = majorMinorFontScheme.FontTypeface["Hans"];
    else if ((localeIds == LocaleIDs.zh_TW || localeIds == LocaleIDs.zh_HK || localeIds == LocaleIDs.zh_MO) && majorMinorFontScheme.FontTypeface.ContainsKey("Hant"))
      nameWithFontScript = majorMinorFontScheme.FontTypeface["Hant"];
    else if ((localeIds == LocaleIDs.ja_JP || localeID == (short) 17) && majorMinorFontScheme.FontTypeface.ContainsKey("Jpan"))
      nameWithFontScript = majorMinorFontScheme.FontTypeface["Jpan"];
    else if ((localeIds == LocaleIDs.ta_IN || localeID == (short) 73) && majorMinorFontScheme.FontTypeface.ContainsKey("Taml"))
      nameWithFontScript = majorMinorFontScheme.FontTypeface["Taml"];
    else if ((localeIds == LocaleIDs.te_IN || localeID == (short) 74) && majorMinorFontScheme.FontTypeface.ContainsKey("Telu"))
      nameWithFontScript = majorMinorFontScheme.FontTypeface["Telu"];
    else if ((localeIds == LocaleIDs.he_IL || localeIds == LocaleIDs.yi_Hebr || localeID == (short) 13) && majorMinorFontScheme.FontTypeface.ContainsKey("Hebr"))
      nameWithFontScript = majorMinorFontScheme.FontTypeface["Hebr"];
    else if ((localeIds == LocaleIDs.th_TH || localeID == (short) 30) && majorMinorFontScheme.FontTypeface.ContainsKey("Thai"))
      nameWithFontScript = majorMinorFontScheme.FontTypeface["Thai"];
    else if (hintType == FontHintType.CS && majorMinorFontScheme.FontTypeface.ContainsKey("Arab"))
      nameWithFontScript = majorMinorFontScheme.FontTypeface["Arab"];
    return nameWithFontScript;
  }

  internal bool ContainsValue(int key)
  {
    if (this.PropertiesHash.ContainsKey(key) || this.CharStyle != null && this.CharStyle.CharacterFormat.HasValue(key))
      return true;
    return this.BaseFormat != null && (this.BaseFormat as WCharacterFormat).ContainsValue(key);
  }

  internal void SetDefaultProperties()
  {
    this.PropertiesHash.Add(3, (object) 10f);
    this.PropertiesHash.Add(1, (object) Color.Empty);
    this.PropertiesHash.Add(2, (object) "Times New Roman");
    this.PropertiesHash.Add(4, (object) false);
    this.PropertiesHash.Add(5, (object) false);
    this.PropertiesHash.Add(7, (object) UnderlineStyle.None);
    this.PropertiesHash.Add(63 /*0x3F*/, (object) Color.Empty);
    this.PropertiesHash.Add(50, (object) false);
    this.PropertiesHash.Add(14, (object) false);
    this.PropertiesHash.Add(51, (object) false);
    this.PropertiesHash.Add(52, (object) false);
    this.PropertiesHash.Add(10, (object) SubSuperScript.None);
    this.PropertiesHash.Add(9, (object) Color.Empty);
    this.PropertiesHash.Add(54, (object) false);
    this.PropertiesHash.Add(59, (object) false);
    this.PropertiesHash.Add(53, (object) false);
    this.PropertiesHash.Add(24, (object) false);
    this.PropertiesHash.Add(55, (object) false);
    this.PropertiesHash.Add(18, (object) 0.0f);
  }

  protected override void InitXDLSHolder()
  {
    if (this.m_sprms != null)
      return;
    this.XDLSHolder.AddElement("text-border", (object) this.Border);
  }

  protected override object GetDefValue(int key)
  {
    if (this.Document == null)
      return (object) null;
    if (this.Document != null && this.Document.DefCharFormat != null && this.Document.DefCharFormat != this)
      return this.Document.DefCharFormat[key];
    switch (key)
    {
      case 0:
        return (object) this.Document.FontSettings.GetFont("Times New Roman", 10f, FontStyle.Regular);
      case 1:
      case 9:
      case 63 /*0x3F*/:
      case 77:
      case 90:
        return (object) Color.Empty;
      case 2:
      case 68:
        return !string.IsNullOrEmpty(this.m_doc.StandardAsciiFont) ? (object) this.m_doc.StandardAsciiFont : (object) "Times New Roman";
      case 3:
      case 62:
        return (object) 10f;
      case 4:
      case 5:
      case 6:
      case 14:
      case 20:
      case 24:
      case 50:
      case 51:
      case 52:
      case 53:
      case 54:
      case 55:
      case 58:
      case 59:
      case 60:
      case 71:
      case 76:
      case 92:
      case 99:
      case 103:
      case 104:
      case 105:
      case 106:
      case 108:
      case 109:
      case 120:
        return (object) false;
      case 7:
        return (object) UnderlineStyle.None;
      case 8:
      case 12:
        return (object) string.Empty;
      case 10:
        return (object) SubSuperScript.None;
      case 11:
      case 15:
        return (object) DateTime.MinValue;
      case 13:
        return (object) null;
      case 17:
      case 18:
        return (object) 0.0f;
      case 61:
        return !string.IsNullOrEmpty(this.m_doc.StandardBidiFont) ? (object) this.m_doc.StandardBidiFont : (object) "Times New Roman";
      case 69:
        return !string.IsNullOrEmpty(this.m_doc.StandardFarEastFont) ? (object) this.m_doc.StandardFarEastFont : (object) "Times New Roman";
      case 70:
        return !string.IsNullOrEmpty(this.m_doc.StandardNonFarEastFont) ? (object) this.m_doc.StandardNonFarEastFont : (object) "Times New Roman";
      case 72:
        return (object) FontHintType.Default;
      case 73:
      case 74:
        return (object) (short) 1033;
      case 75:
        return (object) (short) 1025;
      case 78:
        return (object) TextureStyle.TextureNone;
      case 79:
        return (object) EmphasisType.NoEmphasis;
      case 80 /*0x50*/:
        return (object) TextEffect.None;
      case 81:
        return (object) true;
      case 91:
        return (object) null;
      case 107:
        return (object) int.MaxValue;
      case 121:
        return (object) LigatureType.None;
      case 122:
        return (object) NumberFormType.Default;
      case 123:
        return (object) NumberSpacingType.Default;
      case 124:
        return (object) StylisticSetType.StylisticSetDefault;
      case 125:
        return (object) 0.0f;
      case 126:
        return (object) BreakClearType.None;
      case (int) sbyte.MaxValue:
        return (object) 100f;
      case 128 /*0x80*/:
        return (object) string.Empty;
      default:
        throw new ArgumentException("key has invalid value");
    }
  }

  protected override FormatBase GetDefComposite(int key)
  {
    return key == 67 ? this.GetDefComposite(67, (FormatBase) new Border((FormatBase) this, 67)) : (FormatBase) null;
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (reader.HasAttribute("FontName"))
      this[2] = (object) reader.ReadString("FontName");
    if (reader.HasAttribute("FontNameBidi"))
      this[61] = (object) reader.ReadString("FontNameBidi");
    if (reader.HasAttribute("FontNameAscii"))
      this[68] = (object) reader.ReadString("FontNameAscii");
    if (reader.HasAttribute("FontNameFarEast"))
      this[69] = (object) reader.ReadString("FontNameFarEast");
    if (reader.HasAttribute("FontNameNonFarEast"))
      this[70] = (object) reader.ReadString("FontNameNonFarEast");
    if (reader.HasAttribute("CharStyleName"))
      this.CharStyleName = reader.ReadString("CharStyleName");
    if (reader.HasAttribute("Underline"))
      this.UnderlineStyle = (UnderlineStyle) reader.ReadEnum("Underline", typeof (UnderlineStyle));
    if (reader.HasAttribute("TextColor"))
      this.TextColor = reader.ReadColor("TextColor");
    if (reader.HasAttribute("FontSize"))
      this.SetPropertyValue(3, (object) reader.ReadFloat("FontSize"));
    if (reader.HasAttribute("Bold"))
      this.Bold = reader.ReadBoolean("Bold");
    if (reader.HasAttribute("Italic"))
      this.Italic = reader.ReadBoolean("Italic");
    if (reader.HasAttribute("Strike"))
      this.Strikeout = reader.ReadBoolean("Strike");
    if (reader.HasAttribute("DoubleStrike"))
      this.DoubleStrike = reader.ReadBoolean("DoubleStrike");
    if (reader.HasAttribute("LineSpacing"))
      this.SetPropertyValue(18, (object) reader.ReadFloat("LineSpacing"));
    if (reader.HasAttribute("Position"))
      this.SetPropertyValue(17, (object) reader.ReadFloat("Position"));
    if (reader.HasAttribute("SubSuperScript"))
      this.SubSuperScript = (SubSuperScript) reader.ReadEnum("SubSuperScript", typeof (SubSuperScript));
    if (reader.HasAttribute("TextBackgroundColor"))
      this.TextBackgroundColor = reader.ReadColor("TextBackgroundColor");
    if (reader.HasAttribute("LineBreak"))
      this.LineBreak = reader.ReadBoolean("LineBreak");
    if (reader.HasAttribute("Shadow"))
      this.Shadow = reader.ReadBoolean("Shadow");
    if (reader.HasAttribute("Emboss"))
      this.Emboss = reader.ReadBoolean("Emboss");
    if (reader.HasAttribute("Engrave"))
      this.Engrave = reader.ReadBoolean("Engrave");
    if (reader.HasAttribute("Hidden"))
      this.Hidden = reader.ReadBoolean("Hidden");
    if (reader.HasAttribute("AllCaps"))
      this.AllCaps = reader.ReadBoolean("AllCaps");
    if (reader.HasAttribute("SmallCaps"))
      this.SmallCaps = reader.ReadBoolean("SmallCaps");
    if (reader.HasAttribute("Bidi"))
      this.Bidi = reader.ReadBoolean("Bidi");
    if (reader.HasAttribute("BoldBidi"))
      this.BoldBidi = reader.ReadBoolean("BoldBidi");
    if (reader.HasAttribute("ItalicBidi"))
      this.ItalicBidi = reader.ReadBoolean("ItalicBidi");
    if (reader.HasAttribute("FontSizeBidi"))
      this.SetPropertyValue(62, (object) reader.ReadFloat("FontSizeBidi"));
    if (reader.HasAttribute("HighlightColor"))
      this.HighlightColor = reader.ReadColor("HighlightColor");
    if (reader.HasAttribute("hint"))
    {
      switch (reader.ReadString("hint"))
      {
        case "cs":
          this.IdctHint = FontHintType.CS;
          break;
        case "eastAsia":
          this.IdctHint = FontHintType.EastAsia;
          break;
        default:
          this.IdctHint = FontHintType.Default;
          break;
      }
    }
    if (reader.HasAttribute("RgLid0"))
      this.LocaleIdASCII = reader.ReadShort("RgLid0");
    if (reader.HasAttribute("RgLid1"))
      this.LocaleIdFarEast = reader.ReadShort("RgLid1");
    if (reader.HasAttribute("LidBi"))
      this.LocaleIdBidi = reader.ReadShort("LidBi");
    if (reader.HasAttribute("NoProof"))
      this.NoProof = reader.ReadBoolean("NoProof");
    if (reader.HasAttribute("ForeColor"))
      this.ForeColor = reader.ReadColor("ForeColor");
    if (reader.HasAttribute("Texture"))
      this.TextureStyle = (TextureStyle) reader.ReadEnum("Texture", typeof (TextureStyle));
    if (!reader.HasAttribute("Outline"))
      return;
    this.OutLine = reader.ReadBoolean("Outline");
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    if (this.HasKey(2))
      writer.WriteValue("FontName", this.FontName);
    if (this.HasKey(61))
      writer.WriteValue("FontNameBidi", this.FontNameBidi);
    if (this.HasKey(69))
      writer.WriteValue("FontNameFarEast", this.FontNameFarEast);
    if (this.HasKey(70))
      writer.WriteValue("FontNameNonFarEast", this.FontNameNonFarEast);
    if (this.HasKey(68))
      writer.WriteValue("FontNameAscii", this.FontNameAscii);
    if (this.CharStyleName != null)
      writer.WriteValue("CharStyleName", this.CharStyleName);
    if (!this.SerializeAllData())
      return;
    if (this.LineBreak)
      writer.WriteValue("LineBreak", this.LineBreak);
    if (!this.TextColor.IsEmpty)
      writer.WriteValue("TextColor", this.TextColor);
    if (this.HasValue(3))
      writer.WriteValue("FontSize", this.FontSize);
    if (this.HasValue(4))
      writer.WriteValue("Bold", this.Bold);
    if (this.HasValue(5))
      writer.WriteValue("Italic", this.Italic);
    if (this.HasValue(6))
      writer.WriteValue("Strike", this.Strikeout);
    if (this.HasValue(14))
      writer.WriteValue("DoubleStrike", this.DoubleStrike);
    if (this.HasValue(7))
      writer.WriteValue("Underline", (int) this.UnderlineStyle);
    if (this.HasValue(10))
      writer.WriteValue("SubSuperScript", (Enum) this.SubSuperScript);
    if (this.HasValue(18))
      writer.WriteValue("LineSpacing", this.CharacterSpacing);
    if (this.HasValue(17))
      writer.WriteValue("Position", this.Position);
    if (this.HasValue(9))
      writer.WriteValue("TextBackgroundColor", this.TextBackgroundColor);
    if (this.HasValue(50))
      writer.WriteValue("Shadow", this.Shadow);
    if (this.HasValue(51))
      writer.WriteValue("Emboss", this.Emboss);
    if (this.HasValue(52))
      writer.WriteValue("Engrave", this.Engrave);
    if (this.HasValue(53))
      writer.WriteValue("Hidden", this.Hidden);
    if (this.HasValue(24))
      writer.WriteValue("SpecVanish", this.SpecVanish);
    if (this.HasValue(54))
      writer.WriteValue("AllCaps", this.AllCaps);
    if (this.HasValue(55))
      writer.WriteValue("SmallCaps", this.SmallCaps);
    if (this.HasValue(58))
      writer.WriteValue("Bidi", this.Bidi);
    if (this.HasValue(59))
      writer.WriteValue("BoldBidi", this.BoldBidi);
    if (this.HasValue(60))
      writer.WriteValue("ItalicBidi", this.ItalicBidi);
    if (this.HasValue(62))
      writer.WriteValue("FontSizeBidi", this.FontSizeBidi);
    if (this.HasValue(63 /*0x3F*/))
      writer.WriteValue("HighlightColor", this.HighlightColor);
    if (this.HasValue(72))
      writer.WriteValue("hint", (Enum) this.IdctHint);
    if (this.HasValue(73))
      writer.WriteValue("RgLid0", (int) this.LocaleIdASCII);
    if (this.HasValue(74))
      writer.WriteValue("RgLid1", (int) this.LocaleIdFarEast);
    if (this.HasValue(75))
      writer.WriteValue("LidBi", (int) this.LocaleIdBidi);
    if (this.HasValue(76))
      writer.WriteValue("NoProof", this.NoProof);
    if (this.HasValue(77))
      writer.WriteValue("ForeColor", this.ForeColor);
    if (this.HasValue(78))
      writer.WriteValue("Texture", (Enum) this.TextureStyle);
    if (!this.HasValue(71))
      return;
    writer.WriteValue("Outline", this.OutLine);
  }

  protected override void WriteXmlContent(IXDLSContentWriter writer)
  {
    base.WriteXmlContent(writer);
    if (this.m_sprms == null)
      return;
    byte[] arrData = new byte[this.m_sprms.Length];
    this.m_sprms.Save(arrData, 0);
    writer.WriteChildBinaryElement("internal-data", arrData);
  }

  protected override bool ReadXmlContent(IXDLSContentReader reader)
  {
    bool flag = base.ReadXmlContent(reader);
    if (reader.TagName == "internal-data")
    {
      SinglePropertyModifierArray CHPModifierArray = new SinglePropertyModifierArray(reader.ReadChildBinaryElement());
      flag = true;
      CharacterPropertiesConverter.SprmsToFormat(CHPModifierArray, this, (WordStyleSheet) null, (Dictionary<int, string>) null, false);
      CHPModifierArray.Clear();
    }
    return flag;
  }

  protected internal new void ImportContainer(FormatBase format)
  {
    base.ImportContainer(format);
    if (!(format is WCharacterFormat format1))
      return;
    this.ImportXmlProps(format1);
  }

  private void ImportXmlProps(WCharacterFormat format)
  {
    if (format.m_xmlProps == null || format.m_xmlProps.Count <= 0)
      return;
    foreach (Stream xmlProp in format.XmlProps)
      this.XmlProps.Add(this.CloneStream(xmlProp));
  }

  protected override void ImportMembers(FormatBase format)
  {
    base.ImportMembers(format);
    if (!(format is WCharacterFormat wcharacterFormat))
      return;
    if (wcharacterFormat.PropertiesHash.Count > 0)
    {
      foreach (KeyValuePair<int, object> keyValuePair in wcharacterFormat.PropertiesHash)
      {
        this.PropertiesHash[keyValuePair.Key] = keyValuePair.Value;
        this.IsDefault = false;
      }
    }
    string charStyleName = wcharacterFormat.CharStyleName;
    if (charStyleName == null)
      return;
    WordDocument document = this.Document;
    if ((wcharacterFormat.CharStyleId != null ? (document.Styles as StyleCollection).FindStyleById(wcharacterFormat.CharStyleId) as WCharacterStyle : document.Styles.FindByName(charStyleName) as WCharacterStyle) == null)
    {
      IStyle style = wcharacterFormat.CharStyleId != null ? (IStyle) (wcharacterFormat.Document.Styles as StyleCollection).FindStyleById(wcharacterFormat.CharStyleId) : wcharacterFormat.Document.Styles.FindByName(charStyleName);
      if (style != null)
        document.Styles.Add(style.Clone());
    }
    this.CharStyleName = charStyleName;
    this.CharStyleId = wcharacterFormat.CharStyleId;
  }

  public override void ClearFormatting()
  {
    if (this.m_propertiesHash != null)
      this.m_propertiesHash.Clear();
    if (this.m_xmlProps == null)
      return;
    this.m_xmlProps.Clear();
  }

  protected override void OnChange(FormatBase format, int propKey)
  {
  }

  internal override void ApplyBase(FormatBase baseFormat)
  {
    Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
    if (this.Document.IsCloning && this.Document != baseFormat.Document && !baseFormat.Document.ImportStyles)
    {
      List<int> intList = new List<int>((IEnumerable<int>) new int[18]
      {
        109,
        99,
        4,
        5,
        6,
        14,
        50,
        51,
        52,
        53,
        54,
        55,
        58,
        59,
        60,
        72,
        71,
        106
      });
      foreach (int num in intList)
      {
        if (this.HasValue(num))
          dictionary.Add(num, this.GetComplexBoolValue(num));
      }
      intList.Clear();
    }
    base.ApplyBase(baseFormat);
    foreach (KeyValuePair<int, bool> keyValuePair in dictionary)
    {
      if (keyValuePair.Value != this.GetComplexBoolValue(keyValuePair.Key) && this.HasValue(keyValuePair.Key))
      {
        if (this.GetBoolComplexValue(keyValuePair.Key, keyValuePair.Value) == (byte) 129)
          this.SetPropertyValue(keyValuePair.Key, (object) ToggleOperand.PositiveComplexValue);
        else
          this.SetPropertyValue(keyValuePair.Key, (object) ToggleOperand.NegativeComplexValue);
      }
    }
    dictionary.Clear();
    this.UpdateUsedFontsCollection();
  }

  public void Dispose() => this.Close();

  internal override void Close()
  {
    base.Close();
    if (this.m_tableStyleCharacterFormat != null)
    {
      this.m_tableStyleCharacterFormat.Close();
      this.m_tableStyleCharacterFormat = (WCharacterFormat) null;
    }
    if (this.m_xmlProps == null)
      return;
    this.m_xmlProps.Clear();
    this.m_xmlProps = (List<Stream>) null;
  }

  private void UpdateUsedFontsCollection()
  {
    string fontName = this.HasValue(68) ? this.GetFontName((short) 68) : (string) null;
    if (string.IsNullOrEmpty(fontName))
      return;
    FontStyle fontStyle = FontStyle.Regular;
    if (this.HasValue(4) && this.Bold)
      fontStyle |= FontStyle.Bold;
    if (this.HasValue(5) && this.Italic)
      fontStyle |= FontStyle.Italic;
    if (this.HasValue(7) && this.UnderlineStyle != UnderlineStyle.None)
      fontStyle |= FontStyle.Underline;
    if (this.HasValue(6) && this.Strikeout)
      fontStyle |= FontStyle.Strikeout;
    Font font;
    try
    {
      font = this.Document.FontSettings.GetFont(fontName, 11f, fontStyle);
    }
    catch (Exception ex)
    {
      FontFamily fontFamily = new FontFamily(fontName);
      if (fontFamily.IsStyleAvailable(FontStyle.Bold))
        fontStyle |= FontStyle.Bold;
      if (fontFamily.IsStyleAvailable(FontStyle.Italic))
        fontStyle |= FontStyle.Italic;
      if (fontFamily.IsStyleAvailable(FontStyle.Underline))
        fontStyle |= FontStyle.Underline;
      if (fontFamily.IsStyleAvailable(FontStyle.Strikeout))
        fontStyle |= FontStyle.Strikeout;
      font = this.Document.FontSettings.GetFont(fontName, 11f, fontStyle);
    }
    if (this.m_doc.UsedFontNames.Contains(font))
      return;
    this.m_doc.UsedFontNames.Add(font);
  }

  internal byte GetBoolComplexValue(int propertyKey, bool value)
  {
    byte boolComplexValue;
    if (!(this.OwnerBase is WListLevel))
    {
      bool flag = false;
      if (this.BaseFormat is WCharacterFormat)
        flag = (this.BaseFormat as WCharacterFormat).GetComplexBoolValue(propertyKey);
      if (this.Document.Styles.FindByName(this.CharStyleName, StyleType.CharacterStyle) is Style byName)
      {
        bool complexBoolValue = byName.CharacterFormat.GetComplexBoolValue(propertyKey);
        flag = flag != complexBoolValue;
      }
      boolComplexValue = value == flag ? (byte) 128 /*0x80*/ : (byte) 129;
    }
    else
      boolComplexValue = value ? (byte) 1 : (byte) 0;
    return boolComplexValue;
  }

  internal object GetComplexBoolValue(object value)
  {
    switch (value)
    {
      case bool flag:
        return (object) (ToggleOperand) (flag ? 1 : 0);
      case byte complexBoolValue:
        return (object) (ToggleOperand) complexBoolValue;
      default:
        return value;
    }
  }

  internal override bool HasValue(int propertyKey) => this.HasKey(propertyKey);

  internal override int GetSprmOption(int propertyKey)
  {
    switch (propertyKey)
    {
      case 1:
        return 10818;
      case 2:
      case 68:
        return 19023;
      case 3:
        return 19011;
      case 4:
        return 2101;
      case 5:
        return 2102;
      case 6:
        return 2103;
      case 7:
        return 10814;
      case 9:
      case 77:
      case 78:
        return 18534;
      case 10:
        return 10824;
      case 13:
        return 51832;
      case 14:
        return 10835;
      case 17:
        return 18501;
      case 18:
        return 34880;
      case 24:
        return 2072;
      case 50:
        return 2105;
      case 51:
        return 2136;
      case 52:
        return 2132;
      case 53:
        return 2108;
      case 54:
        return 2107;
      case 55:
        return 2106;
      case 58:
        return 2138;
      case 59:
        return 2140;
      case 60:
        return 2141;
      case 61:
        return 19038;
      case 62:
        return 19041;
      case 63 /*0x3F*/:
        return 10764;
      case 67:
        return 26725;
      case 69:
        return 19024;
      case 70:
        return 19025;
      case 71:
        return 2104;
      case 72:
        return 10351;
      case 73:
        return 18547;
      case 74:
        return 18548;
      case 75:
        return 18527;
      case 76:
        return 2165;
      case 79:
        return 10804;
      case 80 /*0x50*/:
        return 10329;
      case 81:
        return 2152;
      case 90:
        return 26743;
      case 91:
        return 18992;
      case 92:
        return 2065;
      case 99:
        return 2178;
      case 103:
        return 2049;
      case 104:
        return 2048 /*0x0800*/;
      case 105:
        return 51799;
      case 106:
        return 2133;
      case 107:
        return 26759;
      case 108:
        return 18568;
      case 109:
        return 2050;
      case 125:
        return 18507;
      case 126:
        return 10361;
      case (int) sbyte.MaxValue:
        return 18514;
      default:
        return int.MaxValue;
    }
  }

  private void WriteComplexAttr(IXDLSAttributeWriter writer, int propKey, string xdlsConstant)
  {
    SinglePropertyModifierRecord sprm = this.m_sprms[this.GetSprmOption(propKey)];
    writer.WriteValue(xdlsConstant, (int) sprm.ByteValue);
  }

  internal string GetFontHint()
  {
    string empty = string.Empty;
    string fontHint;
    switch (this.IdctHint)
    {
      case FontHintType.EastAsia:
        fontHint = "eastAsia";
        break;
      case FontHintType.CS:
        fontHint = "cs";
        break;
      default:
        fontHint = "default";
        break;
    }
    return fontHint;
  }

  internal string GetFontNameFromHint(FontScriptType scriptType)
  {
    string fontNameFromHint = this.GetFontNameToRender(scriptType);
    if (this.Bidi || this.ComplexScript || TextSplitter.IsEastAsiaScript(scriptType))
      return fontNameFromHint;
    switch (this.IdctHint)
    {
      case FontHintType.Default:
        if (!this.IsThemeFont(this.FontNameNonFarEast))
        {
          fontNameFromHint = this.FontNameNonFarEast;
          break;
        }
        break;
      case FontHintType.EastAsia:
        return fontNameFromHint;
      case FontHintType.CS:
        if (!this.IsThemeFont(this.FontNameBidi))
        {
          fontNameFromHint = this.FontNameBidi;
          break;
        }
        break;
    }
    return fontNameFromHint;
  }

  internal void MergeFormat(WCharacterFormat destinationFormat)
  {
    Dictionary<int, object> properties = new Dictionary<int, object>();
    if (this.Bidi)
      properties.Add(58, (object) true);
    if (this.Bold)
      properties.Add(4, (object) true);
    if (this.BoldBidi)
      properties.Add(59, (object) true);
    if (this.ComplexScript)
      properties.Add(99, (object) true);
    if (this.Hidden)
      properties.Add(53, (object) true);
    if (this.Italic)
      properties.Add(5, (object) true);
    if (this.ItalicBidi)
      properties.Add(60, (object) true);
    if (this.SubSuperScript != SubSuperScript.None)
      properties.Add(10, (object) this.SubSuperScript);
    if (this.UnderlineStyle != UnderlineStyle.None)
      properties.Add(7, (object) this.UnderlineStyle);
    properties.Add(73, (object) this.LocaleIdASCII);
    properties.Add(74, (object) this.LocaleIdFarEast);
    properties.Add(75, (object) this.LocaleIdBidi);
    this.CharStyleName = (string) null;
    this.ImportContainer((FormatBase) destinationFormat);
    this.CopyProperties((FormatBase) destinationFormat);
    this.ApplyBase(destinationFormat.BaseFormat);
    this.UpdateFormattings(properties);
    properties.Clear();
  }

  private void UpdateFormattings(Dictionary<int, object> properties)
  {
    foreach (KeyValuePair<int, object> property in properties)
    {
      switch (property.Key)
      {
        case 4:
          if (this.Bold != (bool) property.Value)
          {
            this.Bold = (bool) property.Value;
            continue;
          }
          continue;
        case 5:
          if (this.Italic != (bool) property.Value)
          {
            this.Italic = (bool) property.Value;
            continue;
          }
          continue;
        case 7:
          if (this.UnderlineStyle != (UnderlineStyle) property.Value)
          {
            this.UnderlineStyle = (UnderlineStyle) property.Value;
            continue;
          }
          continue;
        case 10:
          if (this.SubSuperScript != (SubSuperScript) property.Value)
          {
            this.SubSuperScript = (SubSuperScript) property.Value;
            continue;
          }
          continue;
        case 24:
          if (this.SpecVanish != (bool) property.Value)
          {
            this.SpecVanish = (bool) property.Value;
            continue;
          }
          continue;
        case 50:
          if (this.Shadow != (bool) property.Value)
          {
            this.Shadow = (bool) property.Value;
            continue;
          }
          continue;
        case 53:
          if (this.Hidden != (bool) property.Value)
          {
            this.Hidden = (bool) property.Value;
            continue;
          }
          continue;
        case 58:
          if (this.Bidi != (bool) property.Value)
          {
            this.Bidi = (bool) property.Value;
            continue;
          }
          continue;
        case 59:
          if (this.BoldBidi != (bool) property.Value)
          {
            this.BoldBidi = (bool) property.Value;
            continue;
          }
          continue;
        case 60:
          if (this.ItalicBidi != (bool) property.Value)
          {
            this.ItalicBidi = (bool) property.Value;
            continue;
          }
          continue;
        case 73:
          if ((int) this.LocaleIdASCII != (int) (short) property.Value)
          {
            this.LocaleIdASCII = (short) property.Value;
            continue;
          }
          continue;
        case 74:
          if ((int) this.LocaleIdFarEast != (int) (short) property.Value)
          {
            this.LocaleIdFarEast = (short) property.Value;
            continue;
          }
          continue;
        case 75:
          if ((int) this.LocaleIdBidi != (int) (short) property.Value)
          {
            this.LocaleIdBidi = (short) property.Value;
            continue;
          }
          continue;
        case 99:
          if (this.ComplexScript != (bool) property.Value)
          {
            this.ComplexScript = (bool) property.Value;
            continue;
          }
          continue;
        default:
          continue;
      }
    }
  }

  internal new void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    if ((doc.ImportOptions & ImportOptions.UseDestinationStyles) == (ImportOptions) 0)
      this.UpdateFormatting(doc);
    this.CloneRelationsTo(doc);
  }

  public IOfficeRunFormat Clone()
  {
    WCharacterFormat wcharacterFormat = new WCharacterFormat((IWordDocument) this.m_doc);
    wcharacterFormat.ImportContainer((FormatBase) this);
    wcharacterFormat.CopyProperties((FormatBase) this);
    return (IOfficeRunFormat) wcharacterFormat;
  }

  internal void CloneRelationsTo(WordDocument doc)
  {
    if (string.IsNullOrEmpty(this.CharStyleName) || (doc.ImportOptions & ImportOptions.UseDestinationStyles) == (ImportOptions) 0 || !(this.Document.Styles.FindByName(this.CharStyleName, StyleType.CharacterStyle) is WCharacterStyle byName) || !(byName.ImportStyleTo(doc, false) is WCharacterStyle wcharacterStyle))
      return;
    this.CharStyleName = wcharacterStyle.Name;
  }

  private void UpdateFormatting(WordDocument doc)
  {
    if ((doc.ImportOptions & ImportOptions.MergeFormatting) != (ImportOptions) 0)
    {
      WParagraph wparagraph = doc.LastParagraph ?? new WParagraph((IWordDocument) doc);
      Dictionary<int, object> dictionary = new Dictionary<int, object>();
      if (this != null && this.OwnerBase is WTextRange && (this.OwnerBase as WTextRange).m_revisions != null && (this.OwnerBase as WTextRange).m_revisions.Count != 0)
      {
        foreach (KeyValuePair<int, object> keyValuePair in this.PropertiesHash)
        {
          if (keyValuePair.Key == 8 && keyValuePair.Value is string || keyValuePair.Key == 11 && keyValuePair.Value is DateTime || (keyValuePair.Key == 103 || keyValuePair.Key == 104) && keyValuePair.Value is ToggleOperand)
            dictionary.Add(keyValuePair.Key, keyValuePair.Value);
        }
      }
      this.MergeFormat(wparagraph.BreakCharacterFormat);
      if (dictionary.Count == 0)
        return;
      foreach (KeyValuePair<int, object> keyValuePair in dictionary)
        this.PropertiesHash.Add(keyValuePair.Key, keyValuePair.Value);
    }
    else
    {
      WParagraphStyle byName = doc.Styles.FindByName("Normal", StyleType.ParagraphStyle) as WParagraphStyle;
      if ((doc.ImportOptions & ImportOptions.KeepSourceFormatting) == (ImportOptions) 0)
        return;
      this.UpdateSourceFormat(byName.CharacterFormat);
    }
  }

  internal void UpdateSourceFormat(WCharacterFormat destBaseFormat)
  {
    WCharacterFormat format = new WCharacterFormat((IWordDocument) destBaseFormat.Document);
    format.ImportContainer((FormatBase) this);
    format.CopyProperties((FormatBase) this);
    format.ApplyBase((FormatBase) destBaseFormat);
    format.CharStyleName = (string) null;
    this.UpdateSourceFormatting(format);
    if (this.IsThemeFont(format.FontNameAscii))
      format.FontNameAscii = this.FontName;
    if (this.IsThemeFont(format.FontNameBidi))
      format.FontNameBidi = this.FontName;
    if (this.IsThemeFont(format.FontNameFarEast))
      format.FontNameFarEast = this.FontName;
    if (this.IsThemeFont(format.FontNameNonFarEast))
      format.FontNameNonFarEast = this.FontName;
    this.ImportContainer((FormatBase) format);
    this.CopyProperties((FormatBase) format);
    format.Close();
  }

  internal void UpdateSourceFormatting(WCharacterFormat format)
  {
    if (format.AllCaps != this.AllCaps)
      format.AllCaps = this.AllCaps;
    if (format.Bidi != this.Bidi)
      format.Bidi = this.Bidi;
    if (format.Bold != this.Bold)
      format.Bold = this.Bold;
    if (format.BoldBidi != this.BoldBidi)
      format.BoldBidi = this.BoldBidi;
    if ((double) format.CharacterSpacing != (double) this.CharacterSpacing)
      format.SetPropertyValue(18, (object) this.CharacterSpacing);
    if (format.ComplexScript != this.ComplexScript)
      format.ComplexScript = this.ComplexScript;
    if (format.DoubleStrike != this.DoubleStrike)
      format.DoubleStrike = this.DoubleStrike;
    if (format.Emboss != this.Emboss)
      format.Emboss = this.Emboss;
    if (format.Engrave != this.Engrave)
      format.Engrave = this.Engrave;
    if (format.FieldVanish != this.FieldVanish)
      format.FieldVanish = this.FieldVanish;
    if (format.FontName != this.FontName)
      format.FontName = this.FontName;
    if (format.FontNameAscii != this.FontNameAscii)
      format.FontNameAscii = this.FontNameAscii;
    if (format.FontNameBidi != this.FontNameBidi)
      format.FontNameBidi = this.FontNameBidi;
    if (format.FontNameFarEast != this.FontNameFarEast)
      format.FontNameFarEast = this.FontNameFarEast;
    if (format.FontNameNonFarEast != this.FontNameNonFarEast)
      format.FontNameNonFarEast = this.FontNameNonFarEast;
    if ((double) format.FontSize != (double) this.FontSize)
      format.SetPropertyValue(3, (object) this.FontSize);
    if ((double) format.FontSizeBidi != (double) this.FontSizeBidi)
      format.SetPropertyValue(62, (object) this.FontSizeBidi);
    if (format.ForeColor != this.ForeColor)
      format.ForeColor = this.ForeColor;
    if (format.Hidden != this.Hidden)
      format.Hidden = this.Hidden;
    if (format.HighlightColor != this.HighlightColor)
      format.HighlightColor = this.HighlightColor;
    if (format.IdctHint != this.IdctHint)
      format.IdctHint = this.IdctHint;
    if (format.Italic != this.Italic)
      format.Italic = this.Italic;
    if (format.ItalicBidi != this.ItalicBidi)
      format.ItalicBidi = this.ItalicBidi;
    if ((int) format.LocaleIdBidi != (int) this.LocaleIdBidi)
      format.LocaleIdBidi = this.LocaleIdBidi;
    if (format.Ligatures != this.Ligatures)
      format.Ligatures = this.Ligatures;
    if ((int) format.LocaleIdASCII != (int) this.LocaleIdASCII)
      format.LocaleIdASCII = this.LocaleIdASCII;
    if ((int) format.LocaleIdFarEast != (int) this.LocaleIdFarEast)
      format.LocaleIdFarEast = this.LocaleIdFarEast;
    if (format.NoProof != this.NoProof)
      format.NoProof = this.NoProof;
    if (format.NumberForm != this.NumberForm)
      format.NumberForm = this.NumberForm;
    if (format.NumberSpacing != this.NumberSpacing)
      format.NumberSpacing = this.NumberSpacing;
    if (format.OutLine != this.OutLine)
      format.OutLine = this.OutLine;
    if ((double) format.Position != (double) this.Position)
      format.SetPropertyValue(17, (object) this.Position);
    if (format.Shadow != this.Shadow)
      format.Shadow = this.Shadow;
    if (format.SmallCaps != this.SmallCaps)
      format.SmallCaps = this.SmallCaps;
    if (format.Special != this.Special)
      format.Special = this.Special;
    if (format.SpecVanish != this.SpecVanish)
      format.SpecVanish = this.SpecVanish;
    if ((double) format.Kern != (double) this.Kern)
      format.Kern = this.Kern;
    if ((double) format.Scaling != (double) this.Scaling)
      format.Scaling = this.Scaling;
    if (format.Strikeout != this.Strikeout)
      format.Strikeout = this.Strikeout;
    if (format.StylisticSet != this.StylisticSet)
      format.StylisticSet = this.StylisticSet;
    if (format.SubSuperScript != this.SubSuperScript)
      format.SubSuperScript = this.SubSuperScript;
    if (format.TextBackgroundColor != this.TextBackgroundColor)
      format.TextBackgroundColor = this.TextBackgroundColor;
    if (format.TextColor != this.TextColor)
      format.TextColor = this.TextColor;
    if (format.TextureStyle != this.TextureStyle)
      format.TextureStyle = this.TextureStyle;
    if (format.UnderlineStyle != this.UnderlineStyle)
      format.UnderlineStyle = this.UnderlineStyle;
    if (format.UseContextualAlternates != this.UseContextualAlternates)
      format.UseContextualAlternates = this.UseContextualAlternates;
    this.Border.UpdateSourceFormatting(format.Border);
  }

  internal bool IsThemeFont(string fontName)
  {
    return fontName == "majorAscii" || fontName == "majorBidi" || fontName == "majorEastAsia" || fontName == "majorHAnsi" || fontName == "minorAscii" || fontName == "minorBidi" || fontName == "minorEastAsia" || fontName == "minorHAnsi";
  }

  internal bool UpdateDocDefaults(int propertyKey)
  {
    if (this.PropertiesHash.ContainsKey(propertyKey) || this.BaseFormatHasFontInfoKey(this, propertyKey))
      return false;
    if (propertyKey == 3)
      this.PropertiesHash.Add(3, (object) 10f);
    return true;
  }

  internal bool BaseFormatHasFontInfoKey(WCharacterFormat characterFormat, int propertyKey)
  {
    for (; characterFormat != null; characterFormat = characterFormat.BaseFormat == null || !(characterFormat.BaseFormat is WCharacterFormat) ? (characterFormat == this.Document.DefCharFormat || this.Document.DefCharFormat == null ? (WCharacterFormat) null : this.Document.DefCharFormat) : characterFormat.BaseFormat as WCharacterFormat)
    {
      if (characterFormat.PropertiesHash.ContainsKey(propertyKey))
        return true;
    }
    return false;
  }

  internal bool Compare(WCharacterFormat characterFormat)
  {
    bool flag = false;
    if (this != this.Document.DefCharFormat && this.OwnerBase is WParagraphStyle && (this.OwnerBase as WParagraphStyle).Name == "Normal" && !this.PropertiesHash.ContainsKey(3) && characterFormat.PropertiesHash.ContainsKey(3) && (double) characterFormat.FontSize == 10.0)
      flag = this.UpdateDocDefaults(3);
    if (!this.Compare(2, (FormatBase) characterFormat))
      return false;
    if (!this.Compare(3, (FormatBase) characterFormat))
    {
      if (flag)
        this.PropertiesHash.Remove(3);
      return false;
    }
    if (!this.Compare(99, (FormatBase) characterFormat) || !this.Compare(4, (FormatBase) characterFormat) || !this.Compare(5, (FormatBase) characterFormat) || !this.Compare(6, (FormatBase) characterFormat) || !this.Compare(54, (FormatBase) characterFormat) || !this.Compare(1, (FormatBase) characterFormat) || !this.Compare(14, (FormatBase) characterFormat) || !this.Compare(69, (FormatBase) characterFormat) || !this.Compare(62, (FormatBase) characterFormat) || !this.Compare(9, (FormatBase) characterFormat) || !this.Compare(51, (FormatBase) characterFormat) || !this.Compare(52, (FormatBase) characterFormat) || !this.Compare(63 /*0x3F*/, (FormatBase) characterFormat) || !this.Compare(68, (FormatBase) characterFormat) || !this.Compare(61, (FormatBase) characterFormat) || !this.Compare(71, (FormatBase) characterFormat) || !this.Compare(17, (FormatBase) characterFormat) || !this.Compare(50, (FormatBase) characterFormat) || !this.Compare(10, (FormatBase) characterFormat) || !this.Compare(55, (FormatBase) characterFormat) || !this.Compare(106, (FormatBase) characterFormat) || this.CharStyleName != characterFormat.CharStyleName || !this.Compare(18, (FormatBase) characterFormat) || !this.Compare(109, (FormatBase) characterFormat) || !this.Compare(70, (FormatBase) characterFormat) || !this.Compare(53, (FormatBase) characterFormat) || !this.Compare(60, (FormatBase) characterFormat) || !this.Compare(75, (FormatBase) characterFormat) || !this.Compare(123, (FormatBase) characterFormat) || !this.Compare(7, (FormatBase) characterFormat) || !this.Compare(59, (FormatBase) characterFormat) || !this.Compare(78, (FormatBase) characterFormat) || !this.Border.Compare(characterFormat.Border))
      return false;
    if (flag)
      this.PropertiesHash.Remove(3);
    return true;
  }

  internal void SetKernSize(float value)
  {
    if ((double) value < 0.0 || (double) value > 1638.0)
      value = this.FontSize;
    this.Kern = value;
  }

  internal void SetPositionValue(float value)
  {
    if ((double) Math.Abs(value) > 1584.0)
      value = 0.0f;
    this.SetPropertyValue(17, (object) value);
  }

  internal void SetScalingValue(float value)
  {
    if ((double) value < 1.0 || (double) value > 600.0)
      value = 100f;
    this.Scaling = value;
  }

  internal void SetCharacterSpacingValue(float value)
  {
    if ((double) Math.Abs(value) > 1584.0)
      value = 0.0f;
    this.SetPropertyValue(18, (object) value);
  }
}
