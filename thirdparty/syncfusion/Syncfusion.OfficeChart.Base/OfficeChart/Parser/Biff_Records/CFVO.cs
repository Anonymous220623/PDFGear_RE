// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.CFVO
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using Syncfusion.OfficeChart.Parser.Biff_Records.Formula;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

internal class CFVO
{
  private const ushort DEF_MINIMUM_SIZE = 3;
  private byte m_cfvoType = 1;
  private ushort m_formulaLength;
  private byte[] m_arrFormula = new byte[0];
  private Ptg[] m_arrFormulaParsed;
  private double m_numValue;
  private string m_value;

  public ushort FormulaSize => this.m_formulaLength;

  public Ptg[] FormulaPtgs
  {
    get => this.m_arrFormulaParsed;
    set
    {
      this.m_arrFormula = FormulaUtil.PtgArrayToByteArray(value, OfficeVersion.Excel2007);
      this.m_arrFormulaParsed = value;
      this.m_formulaLength = (ushort) this.m_arrFormula.Length;
    }
  }

  public byte[] FormulaBytes => this.m_arrFormula;

  public double NumValue
  {
    get => this.m_numValue;
    set => this.m_numValue = value;
  }

  public string Value
  {
    get => this.m_value;
    set => this.m_value = value;
  }

  public int ParseCFVO(DataProvider provider, int iOffset, OfficeVersion version)
  {
    this.m_cfvoType = provider.ReadByte(iOffset);
    ++iOffset;
    this.m_formulaLength = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_arrFormula = new byte[(int) this.m_formulaLength];
    provider.ReadArray(iOffset, this.m_arrFormula);
    iOffset += (int) this.m_formulaLength;
    this.m_arrFormulaParsed = FormulaUtil.ParseExpression((DataProvider) new ByteArrayDataProvider(this.m_arrFormula), (int) this.m_formulaLength, version);
    if (version != OfficeVersion.Excel2007 && this.m_formulaLength > (ushort) 0)
    {
      this.m_arrFormula = FormulaUtil.PtgArrayToByteArray(this.m_arrFormulaParsed, OfficeVersion.Excel2007);
      this.m_formulaLength = (ushort) this.m_arrFormula.Length;
    }
    return iOffset;
  }

  public int SerializeCFVO(DataProvider provider, int iOffset, OfficeVersion version)
  {
    if (this.m_arrFormulaParsed != null && this.m_arrFormulaParsed.Length > 0)
    {
      this.m_arrFormula = FormulaUtil.PtgArrayToByteArray(this.m_arrFormulaParsed, version);
      this.m_formulaLength = (ushort) this.m_arrFormula.Length;
    }
    else
    {
      this.m_arrFormula = (byte[]) null;
      this.m_formulaLength = (ushort) 0;
    }
    provider.WriteByte(iOffset, this.m_cfvoType);
    ++iOffset;
    provider.WriteUInt16(iOffset, this.m_formulaLength);
    iOffset += 2;
    provider.WriteBytes(iOffset, this.m_arrFormula, 0, (int) this.m_formulaLength);
    iOffset += (int) this.m_formulaLength;
    return iOffset;
  }

  public int GetStoreSize(OfficeVersion version) => 3 + (int) this.m_formulaLength;

  internal void ClearAll()
  {
    this.m_arrFormula = (byte[]) null;
    this.m_arrFormulaParsed = (Ptg[]) null;
  }
}
