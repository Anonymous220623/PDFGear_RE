// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ExtendedFormatStandAlone
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class ExtendedFormatStandAlone : ExtendedFormatImpl
{
  private static readonly DataBarDirection DefaultDataBarDirection = DataBarDirection.context;
  private static readonly Color DefaultColor = Color.FromArgb((int) byte.MaxValue, 99, 142, 198);
  private FontImpl font;
  private ExcelIconSetType m_iconSetType;
  private int m_iconId = -1;
  private string m_iconName;
  private Image m_advancedCFIcon;
  private bool m_showIconOnly;
  private double m_dataBarPercent = (double) int.MinValue;
  private DataBarDirection m_direction = ExtendedFormatStandAlone.DefaultDataBarDirection;
  private bool m_hasExtensionList;
  internal string ST_GUID;
  private Color m_dataBarBorderColor;
  private bool m_hasDataBarBorder;
  private bool m_bShowValue = true;
  private bool m_bNegativeBar = true;
  private double m_nagativeBarPoint = -1.0;
  private Color m_axisColor;
  private Color m_negativeDataBarBorderColor;
  private Color m_negativeFillColor;
  private bool m_bHasDiffNegativeBarColor;
  private bool m_bHasDiffNegativeBarBorderColor = true;
  private Color m_barColor = ExtendedFormatStandAlone.DefaultColor;
  private bool m_hasDataBar;
  private bool m_hasIconSet;
  private double m_minValue;
  private double m_maxValue;

  public ExtendedFormatStandAlone(ExtendedFormatImpl format)
    : base((IApplication) format.AppImplementation, format.Parent)
  {
    format.CopyTo((ExtendedFormatImpl) this);
    if (format is ExtendedFormatStandAlone)
    {
      ExtendedFormatStandAlone formatStandAlone = format as ExtendedFormatStandAlone;
      this.IconId = formatStandAlone.IconId;
      this.IconName = formatStandAlone.IconName;
      this.IconSet = formatStandAlone.IconSet;
      this.ShowIconOnly = formatStandAlone.ShowIconOnly;
      this.HasDataBar = formatStandAlone.HasDataBar;
      this.HasIconSet = formatStandAlone.HasIconSet;
      this.MinValue = formatStandAlone.MinValue;
      this.MaxValue = formatStandAlone.MaxValue;
      this.DataBarPercent = formatStandAlone.DataBarPercent;
      this.HasDataBarBorder = formatStandAlone.HasDataBarBorder;
      this.DataBarBorderColor = formatStandAlone.DataBarBorderColor;
      this.BarAxisColor = formatStandAlone.BarAxisColor;
      this.IsNegativeBar = formatStandAlone.IsNegativeBar;
      this.DataBarDirection = formatStandAlone.DataBarDirection;
      this.ShowValue = formatStandAlone.ShowValue;
      this.NegativeBarPoint = formatStandAlone.NegativeBarPoint;
      this.NegativeFillColor = formatStandAlone.NegativeFillColor;
      this.NegativeDataBarBorderColor = formatStandAlone.NegativeDataBarBorderColor;
    }
    this.font = this.AppImplementation.CreateFont(format.Font);
    this.FontIndex = -1;
    this.InitializeColors();
    this.CopyColorsFrom(format);
    this.UpdateIconSetValues(format);
  }

  public override IFont Font => (IFont) this.font;

  internal int IconId
  {
    get => this.m_iconId;
    set => this.m_iconId = value;
  }

  internal ExcelIconSetType IconSet
  {
    get => this.m_iconSetType;
    set => this.m_iconSetType = value;
  }

  internal string IconName
  {
    get => this.m_iconName;
    set => this.m_iconName = value;
  }

  internal Image AdvancedCFIcon
  {
    get => this.m_advancedCFIcon;
    set => this.m_advancedCFIcon = value;
  }

  internal DataBarDirection DataBarDirection
  {
    get => this.m_direction;
    set => this.m_direction = value;
  }

  internal Color BarAxisColor
  {
    get => this.m_axisColor;
    set => this.m_axisColor = value;
  }

  internal double MinValue
  {
    get => this.m_minValue;
    set => this.m_minValue = value;
  }

  internal bool HasDataBar
  {
    get => this.m_hasDataBar;
    set => this.m_hasDataBar = value;
  }

  internal bool HasIconSet
  {
    get => this.m_hasIconSet;
    set => this.m_hasIconSet = value;
  }

  internal double MaxValue
  {
    get => this.m_maxValue;
    set => this.m_maxValue = value;
  }

  internal Color NegativeDataBarBorderColor
  {
    get => this.m_negativeDataBarBorderColor;
    set
    {
      this.m_negativeDataBarBorderColor = value;
      this.HasDiffNegativeBarBorderColor = true;
    }
  }

  internal bool HasDiffNegativeBarColor
  {
    get => this.m_bHasDiffNegativeBarColor;
    set => this.m_bHasDiffNegativeBarColor = value;
  }

  internal bool HasDiffNegativeBarBorderColor
  {
    get => this.m_bHasDiffNegativeBarBorderColor;
    set => this.m_bHasDiffNegativeBarBorderColor = value;
  }

  internal Color NegativeFillColor
  {
    get => this.HasDiffNegativeBarColor ? this.m_negativeFillColor : this.m_barColor;
    set
    {
      this.m_negativeFillColor = value;
      this.HasDiffNegativeBarColor = true;
    }
  }

  internal bool ShowValue
  {
    get => this.m_bShowValue;
    set => this.m_bShowValue = value;
  }

  internal bool IsNegativeBar
  {
    get => this.m_bNegativeBar;
    set => this.m_bNegativeBar = value;
  }

  internal Color DataBarBorderColor
  {
    get => this.m_dataBarBorderColor;
    set
    {
      this.m_dataBarBorderColor = value;
      this.m_hasDataBarBorder = true;
    }
  }

  internal bool HasDataBarBorder
  {
    get => this.m_hasDataBarBorder;
    set => this.m_hasDataBarBorder = value;
  }

  internal double DataBarPercent
  {
    get => this.m_dataBarPercent;
    set => this.m_dataBarPercent = value;
  }

  internal double NegativeBarPoint
  {
    get => this.m_nagativeBarPoint;
    set => this.m_nagativeBarPoint = value;
  }

  internal bool ShowIconOnly
  {
    get => this.m_showIconOnly;
    set => this.m_showIconOnly = value;
  }

  private void UpdateIconSetValues(ExtendedFormatImpl format)
  {
    if (!(format is ExtendedFormatStandAlone formatStandAlone) || formatStandAlone.AdvancedCFIcon == null)
      return;
    this.m_advancedCFIcon = formatStandAlone.AdvancedCFIcon;
    this.m_showIconOnly = formatStandAlone.ShowIconOnly;
  }
}
