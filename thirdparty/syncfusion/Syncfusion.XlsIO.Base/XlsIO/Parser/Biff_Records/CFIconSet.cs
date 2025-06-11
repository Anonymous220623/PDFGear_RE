// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.CFIconSet
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

public class CFIconSet
{
  private const ushort DEF_MINIMUM_SIZE = 6;
  private ushort m_undefined;
  private byte m_iconStates;
  private byte m_iconSet;
  private bool m_isIconOnly;
  private bool m_iconIsReversed;
  private List<CFIconMultiState> m_arrMultistate = new List<CFIconMultiState>();
  private IconSetImpl m_iconSetImpl;

  public ExcelIconSetType IconSetType
  {
    get => (ExcelIconSetType) this.m_iconSet;
    set => this.m_iconSet = (byte) value;
  }

  public List<CFIconMultiState> ListCFIconSet
  {
    get => this.m_arrMultistate;
    set => this.m_arrMultistate = value;
  }

  public ushort DefaultRecordSize => 6;

  public IIconSet IconsetImpl => (IIconSet) this.m_iconSetImpl;

  public CFIconSet()
  {
    this.m_arrMultistate = new List<CFIconMultiState>();
    this.m_iconSetImpl = new IconSetImpl();
  }

  private void CopyIconSet()
  {
    this.m_iconSetImpl.IconSet = this.IconSetType;
    this.m_iconSetImpl.ShowIconOnly = this.m_isIconOnly;
    this.m_iconSetImpl.ReverseOrder = this.m_iconIsReversed;
    for (int index = 0; index < this.m_arrMultistate.Count; ++index)
    {
      CFIconMultiState cfIconMultiState = this.m_arrMultistate[index];
      ConditionValue conditionValue = new ConditionValue();
      this.m_iconSetImpl.IconCriteria[index].Type = cfIconMultiState.CFVO.CFVOType;
      (this.m_iconSetImpl.IconCriteria[index] as ConditionValue).RefPtg = cfIconMultiState.CFVO.RefPtg;
      this.m_iconSetImpl.IconCriteria[index].Value = cfIconMultiState.CFVO.Value;
    }
  }

  public int ParseIconSet(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_undefined = provider.ReadUInt16(iOffset);
    iOffset += 2;
    int num = (int) provider.ReadByte(iOffset);
    ++iOffset;
    this.m_iconStates = provider.ReadByte(iOffset);
    ++iOffset;
    this.m_iconSet = provider.ReadByte(iOffset);
    ++iOffset;
    this.m_isIconOnly = provider.ReadBit(iOffset, 0);
    provider.ReadBit(iOffset, 1);
    this.m_iconIsReversed = provider.ReadBit(iOffset, 2);
    ++iOffset;
    for (int index = 0; index < (int) this.m_iconStates; ++index)
    {
      CFIconMultiState cfIconMultiState = new CFIconMultiState();
      iOffset = cfIconMultiState.ParseCFIconMultistate(provider, iOffset, version);
      this.m_arrMultistate.Add(cfIconMultiState);
    }
    this.CopyIconSet();
    return iOffset;
  }

  public int SerializeIconSet(
    DataProvider provider,
    int iOffset,
    ExcelVersion version,
    IIconSet iIconSet)
  {
    if (iIconSet != null)
      this.ListCFIconSet = this.UpdateIconSet(iIconSet);
    provider.WriteUInt16(iOffset, this.m_undefined);
    iOffset += 2;
    provider.WriteByte(iOffset, (byte) 0);
    ++iOffset;
    provider.WriteByte(iOffset, this.m_iconStates);
    ++iOffset;
    provider.WriteByte(iOffset, this.m_iconSet);
    ++iOffset;
    provider.WriteByte(iOffset, this.CalculateIconOnlyAndReverseOrder());
    ++iOffset;
    foreach (CFIconMultiState listCfIcon in this.ListCFIconSet)
      iOffset = listCfIcon.SerializeCFIconMultistate(provider, iOffset, version);
    return iOffset;
  }

  private byte CalculateIconOnlyAndReverseOrder()
  {
    byte onlyAndReverseOrder = 0;
    if (this.m_iconIsReversed && this.m_isIconOnly)
      onlyAndReverseOrder = (byte) 5;
    if (this.m_iconIsReversed && !this.m_isIconOnly)
      onlyAndReverseOrder = (byte) 4;
    if (!this.m_iconIsReversed && this.m_isIconOnly)
      onlyAndReverseOrder = (byte) 1;
    return onlyAndReverseOrder;
  }

  public int GetStoreSize(ExcelVersion version)
  {
    int num = 0;
    foreach (CFIconMultiState cfIconMultiState in this.m_arrMultistate)
      num += cfIconMultiState.GetStoreSize(version);
    return 6 + num;
  }

  private List<CFIconMultiState> UpdateIconSet(IIconSet updateIconset)
  {
    List<CFIconMultiState> cfIconMultiStateList = new List<CFIconMultiState>();
    switch (updateIconset.IconSet)
    {
      case ExcelIconSetType.ThreeArrows:
        this.IconSetType = ExcelIconSetType.ThreeArrows;
        this.m_iconStates = (byte) 3;
        break;
      case ExcelIconSetType.ThreeArrowsGray:
        this.IconSetType = ExcelIconSetType.ThreeArrowsGray;
        this.m_iconStates = (byte) 3;
        break;
      case ExcelIconSetType.ThreeFlags:
        this.IconSetType = ExcelIconSetType.ThreeFlags;
        this.m_iconStates = (byte) 3;
        break;
      case ExcelIconSetType.ThreeTrafficLights1:
        this.IconSetType = ExcelIconSetType.ThreeTrafficLights1;
        this.m_iconStates = (byte) 3;
        break;
      case ExcelIconSetType.ThreeTrafficLights2:
        this.IconSetType = ExcelIconSetType.ThreeTrafficLights2;
        this.m_iconStates = (byte) 3;
        break;
      case ExcelIconSetType.ThreeSigns:
        this.IconSetType = ExcelIconSetType.ThreeSigns;
        this.m_iconStates = (byte) 3;
        break;
      case ExcelIconSetType.ThreeSymbols:
        this.IconSetType = ExcelIconSetType.ThreeSymbols;
        this.m_iconStates = (byte) 3;
        break;
      case ExcelIconSetType.ThreeSymbols2:
        this.IconSetType = ExcelIconSetType.ThreeSymbols2;
        this.m_iconStates = (byte) 3;
        break;
      case ExcelIconSetType.FourArrows:
        this.IconSetType = ExcelIconSetType.FourArrows;
        this.m_iconStates = (byte) 4;
        break;
      case ExcelIconSetType.FourArrowsGray:
        this.IconSetType = ExcelIconSetType.FourArrowsGray;
        this.m_iconStates = (byte) 4;
        break;
      case ExcelIconSetType.FourRedToBlack:
        this.IconSetType = ExcelIconSetType.FourRedToBlack;
        this.m_iconStates = (byte) 4;
        break;
      case ExcelIconSetType.FourRating:
        this.IconSetType = ExcelIconSetType.FourRating;
        this.m_iconStates = (byte) 4;
        break;
      case ExcelIconSetType.FourTrafficLights:
        this.IconSetType = ExcelIconSetType.FourTrafficLights;
        this.m_iconStates = (byte) 4;
        break;
      case ExcelIconSetType.FiveArrows:
        this.IconSetType = ExcelIconSetType.FiveArrows;
        this.m_iconStates = (byte) 5;
        break;
      case ExcelIconSetType.FiveArrowsGray:
        this.IconSetType = ExcelIconSetType.FiveArrowsGray;
        this.m_iconStates = (byte) 5;
        break;
      case ExcelIconSetType.FiveRating:
        this.IconSetType = ExcelIconSetType.FiveRating;
        this.m_iconStates = (byte) 5;
        break;
      case ExcelIconSetType.FiveQuarters:
        this.IconSetType = ExcelIconSetType.FiveQuarters;
        this.m_iconStates = (byte) 5;
        break;
    }
    this.m_isIconOnly = updateIconset.ShowIconOnly;
    this.m_iconIsReversed = updateIconset.ReverseOrder;
    if (updateIconset != null)
    {
      this.ListCFIconSet.Clear();
      foreach (IConditionValue iconCriterion in (IEnumerable<IConditionValue>) updateIconset.IconCriteria)
        cfIconMultiStateList.Add(new CFIconMultiState()
        {
          CFVO = {
            CFVOType = iconCriterion.Type,
            Value = iconCriterion.Value
          },
          IsEqulal = (byte) iconCriterion.Operator
        });
    }
    return cfIconMultiStateList;
  }

  public void UpdateIconSetColor(IList<IConditionValue> m_IconCriteria)
  {
    if (m_IconCriteria == null)
      return;
    this.ListCFIconSet.Clear();
    foreach (IConditionValue mIconCriterion in (IEnumerable<IConditionValue>) m_IconCriteria)
      this.ListCFIconSet.Add(new CFIconMultiState()
      {
        CFVO = {
          CFVOType = mIconCriterion.Type,
          Value = mIconCriterion.Value
        }
      });
  }

  internal void ClearAll()
  {
    foreach (CFIconMultiState cfIconMultiState in this.m_arrMultistate)
      cfIconMultiState.ClearAll();
    this.m_arrMultistate.Clear();
    this.m_arrMultistate = (List<CFIconMultiState>) null;
    this.m_iconSetImpl.ClearAll();
    this.m_iconSetImpl = (IconSetImpl) null;
  }
}
