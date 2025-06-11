// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.CFIconSet
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

internal class CFIconSet
{
  private const ushort DEF_MINIMUM_SIZE = 6;
  private ushort m_undefined;
  private byte m_iconStates;
  private byte m_iconSet;
  private bool m_isIconOnly;
  private bool m_iconIsReversed;
  private List<CFIconMultiState> m_arrMultistate = new List<CFIconMultiState>();

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

  public CFIconSet() => this.m_arrMultistate = new List<CFIconMultiState>();

  private void CopyIconSet()
  {
  }

  public int ParseIconSet(DataProvider provider, int iOffset, OfficeVersion version)
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

  public int GetStoreSize(OfficeVersion version)
  {
    int num = 0;
    foreach (CFIconMultiState cfIconMultiState in this.m_arrMultistate)
      num += cfIconMultiState.GetStoreSize(version);
    return 6 + num;
  }

  internal void ClearAll()
  {
    foreach (CFIconMultiState cfIconMultiState in this.m_arrMultistate)
      cfIconMultiState.ClearAll();
    this.m_arrMultistate.Clear();
    this.m_arrMultistate = (List<CFIconMultiState>) null;
  }
}
