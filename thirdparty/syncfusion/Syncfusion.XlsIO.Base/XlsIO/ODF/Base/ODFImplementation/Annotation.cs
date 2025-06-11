// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.Annotation
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

internal class Annotation
{
  private string m_captionPointX;
  private string m_captionPointY;
  private string m_classNames;
  private int m_cornerRadius;
  private int m_drawId;
  private string m_layer;
  private string m_drawName;
  private string m_styleName;
  private string m_textStyleName;
  private string m_transform;
  private string m_zIndex;
  private bool m_display;
  private string m_name;
  private float m_height;
  private float m_width;
  private float m_x;
  private float m_y;
  private string m_endCellAddr;
  private string m_endX;
  private string m_endY;
  private int m_anchorPageNumber;
  private string m_anchorType;
  private int m_id;
  private string m_creator;
  private DateTime m_date;
  private string m_dateString;
  private string m_list;
  private System.Collections.Generic.List<OParagraph> m_paras;

  internal string CaptionPointX
  {
    get => this.m_captionPointX;
    set => this.m_captionPointX = value;
  }

  internal string CaptionPointY
  {
    get => this.m_captionPointY;
    set => this.m_captionPointY = value;
  }

  internal string ClassNames
  {
    get => this.m_classNames;
    set => this.m_classNames = value;
  }

  internal int CornerRadius
  {
    get => this.m_cornerRadius;
    set => this.m_cornerRadius = value;
  }

  internal int DrawId
  {
    get => this.m_drawId;
    set => this.m_drawId = value;
  }

  internal string Layer
  {
    get => this.m_layer;
    set => this.m_layer = value;
  }

  internal string DrawName
  {
    get => this.m_drawName;
    set => this.m_drawName = value;
  }

  internal string StyleName
  {
    get => this.m_styleName;
    set => this.m_styleName = value;
  }

  internal string TextStyleName
  {
    get => this.m_textStyleName;
    set => this.m_textStyleName = value;
  }

  internal string Transform
  {
    get => this.m_transform;
    set => this.m_transform = value;
  }

  internal string ZIndex
  {
    get => this.m_zIndex;
    set => this.m_zIndex = value;
  }

  internal bool Display
  {
    get => this.m_display;
    set => this.m_display = value;
  }

  internal string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  internal float Height
  {
    get => this.m_height;
    set => this.m_height = value;
  }

  internal float Width
  {
    get => this.m_width;
    set => this.m_width = value;
  }

  internal float X
  {
    get => this.m_x;
    set => this.m_x = value;
  }

  internal float Y
  {
    get => this.m_y;
    set => this.m_y = value;
  }

  internal string EndCellAddr
  {
    get => this.m_endCellAddr;
    set => this.m_endCellAddr = value;
  }

  internal string EndX
  {
    get => this.m_endX;
    set => this.m_endX = value;
  }

  internal string EndY
  {
    get => this.m_endY;
    set => this.m_endY = value;
  }

  internal int AnchorPageNumber
  {
    get => this.m_anchorPageNumber;
    set => this.m_anchorPageNumber = value;
  }

  internal string AnchorType
  {
    get => this.m_anchorType;
    set => this.m_anchorType = value;
  }

  internal int Id
  {
    get => this.m_id;
    set => this.m_id = value;
  }

  public string Creator
  {
    get => this.m_creator;
    set => this.m_creator = value;
  }

  public DateTime Date
  {
    get => this.m_date;
    set => this.m_date = value;
  }

  public string DateString
  {
    get => this.m_dateString;
    set => this.m_dateString = value;
  }

  public string List
  {
    get => this.m_list;
    set => this.m_list = value;
  }

  internal System.Collections.Generic.List<OParagraph> Paras
  {
    get
    {
      if (this.m_paras == null)
        this.m_paras = new System.Collections.Generic.List<OParagraph>();
      return this.m_paras;
    }
    set => this.m_paras = value;
  }
}
