// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotItemOptions
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

public class PivotItemOptions
{
  private bool m_hasItemProperties;
  private bool m_bHasChildItems;
  private bool m_bIsExpaned;
  private bool m_bDrillAcross;
  private bool m_bIsCalculatedItem;
  private bool m_bIsHidden;
  private bool m_bIsMissing;
  private string m_userCaption;
  private bool m_bIsChar;
  private bool m_bIsHiddenDetails;
  private PivotItemType m_itemType;

  internal bool HasItemProperties
  {
    get => this.m_hasItemProperties;
    set => this.m_hasItemProperties = true;
  }

  public bool HasChildItems
  {
    get => this.m_bHasChildItems;
    set
    {
      this.m_bHasChildItems = value;
      if (!this.m_bHasChildItems || this.m_hasItemProperties)
        return;
      this.m_hasItemProperties = true;
    }
  }

  public bool IsExpaned
  {
    get => this.m_bIsExpaned;
    set
    {
      this.m_bIsExpaned = value;
      if (!this.m_bIsExpaned || this.m_hasItemProperties)
        return;
      this.m_hasItemProperties = true;
    }
  }

  public bool DrillAcross
  {
    get => this.m_bDrillAcross;
    set
    {
      this.m_bDrillAcross = value;
      if (!this.m_bDrillAcross || this.m_hasItemProperties)
        return;
      this.m_hasItemProperties = true;
    }
  }

  public bool IsCalculatedItem
  {
    get => this.m_bIsCalculatedItem;
    set
    {
      this.m_bIsCalculatedItem = value;
      if (!this.m_bIsCalculatedItem || this.m_hasItemProperties)
        return;
      this.m_hasItemProperties = true;
    }
  }

  public bool IsHidden
  {
    get => this.m_bIsHidden;
    set
    {
      this.m_bIsHidden = value;
      if (!this.m_bIsHidden || this.m_hasItemProperties)
        return;
      this.m_hasItemProperties = true;
    }
  }

  public bool IsMissing
  {
    get => this.m_bIsMissing;
    set
    {
      this.m_bIsMissing = value;
      if (!this.m_bIsMissing || this.m_hasItemProperties)
        return;
      this.m_hasItemProperties = true;
    }
  }

  public string UserCaption
  {
    get => this.m_userCaption;
    set
    {
      this.m_userCaption = value;
      if (this.m_userCaption == null || this.m_hasItemProperties)
        return;
      this.m_hasItemProperties = true;
    }
  }

  public bool IsChar
  {
    get => this.m_bIsChar;
    set
    {
      this.m_bIsChar = value;
      if (!this.m_bIsChar || this.m_hasItemProperties)
        return;
      this.m_hasItemProperties = true;
    }
  }

  public bool IsHiddenDetails
  {
    get => this.m_bIsHiddenDetails;
    set
    {
      this.m_bIsHiddenDetails = value;
      if (!this.m_bIsHiddenDetails || this.m_hasItemProperties)
        return;
      this.m_hasItemProperties = true;
    }
  }

  public PivotItemType ItemType
  {
    get => this.m_itemType;
    set => this.m_itemType = value;
  }

  public PivotItemOptions() => this.m_bIsHiddenDetails = true;
}
