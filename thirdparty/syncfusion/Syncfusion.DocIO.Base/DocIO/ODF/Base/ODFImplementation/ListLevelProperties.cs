// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.ListLevelProperties
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class ListLevelProperties
{
  private string m_numberSufix;
  private string m_numberPrefix;
  private float m_spaceBefore;
  private float m_minimumLabelWidth;
  private float m_leftMargin;
  private float m_textIndent;
  private ListNumberFormat m_numberFormat;
  private string m_bulletCharacter;
  private ODFStyle m_odfStyle;
  private TextProperties m_TextProperties;
  private bool m_isPictureBullet;
  private string m_href;
  private OPicture m_pictureBullet;
  private TextAlign m_textAlign;

  internal string PictureHRef
  {
    get => this.m_href;
    set => this.m_href = value;
  }

  internal OPicture PictureBullet
  {
    get => this.m_pictureBullet;
    set => this.m_pictureBullet = value;
  }

  internal bool IsPictureBullet
  {
    get => this.m_isPictureBullet;
    set => this.m_isPictureBullet = value;
  }

  internal ODFStyle Style
  {
    get => this.m_odfStyle;
    set => this.m_odfStyle = value;
  }

  internal TextProperties TextProperties
  {
    get => this.m_TextProperties;
    set => this.m_TextProperties = value;
  }

  internal string BulletCharacter
  {
    get => this.m_bulletCharacter;
    set => this.m_bulletCharacter = value;
  }

  internal string NumberSufix
  {
    get => this.m_numberSufix;
    set => this.m_numberSufix = value;
  }

  internal string NumberPrefix
  {
    get => this.m_numberPrefix;
    set => this.m_numberPrefix = value;
  }

  internal float SpaceBefore
  {
    get => this.m_spaceBefore;
    set => this.m_spaceBefore = value;
  }

  internal float MinimumLabelWidth
  {
    get => this.m_minimumLabelWidth;
    set => this.m_minimumLabelWidth = value;
  }

  internal float LeftMargin
  {
    get => this.m_leftMargin;
    set => this.m_leftMargin = value;
  }

  internal float TextIndent
  {
    get => this.m_textIndent;
    set => this.m_textIndent = value;
  }

  internal ListNumberFormat NumberFormat
  {
    get => this.m_numberFormat;
    set => this.m_numberFormat = value;
  }

  internal TextAlign TextAlignment
  {
    get => this.m_textAlign;
    set => this.m_textAlign = value;
  }

  internal void Close()
  {
    if (this.m_odfStyle == null)
      return;
    this.m_odfStyle.Close();
    this.m_odfStyle = (ODFStyle) null;
  }
}
