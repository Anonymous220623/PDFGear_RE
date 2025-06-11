// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.OfficeMathMatrixRow
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

#nullable disable
namespace Syncfusion.Office;

internal class OfficeMathMatrixRow : OwnerHolder, IOfficeMathMatrixRow, IOfficeMathEntity
{
  internal OfficeMaths m_args;

  public int RowIndex
  {
    get
    {
      return this.OwnerMathEntity is OfficeMathMatrix ownerMathEntity ? (ownerMathEntity.Rows as OfficeMathMatrixRows).InnerList.IndexOf((object) this) : -1;
    }
  }

  public IOfficeMaths Arguments => (IOfficeMaths) this.m_args;

  internal OfficeMathMatrixRow(IOfficeMathEntity owner)
    : base(owner)
  {
    this.m_args = new OfficeMaths((IOfficeMathEntity) this);
  }

  internal OfficeMathMatrixRow Clone(IOfficeMathEntity owner)
  {
    OfficeMathMatrixRow owner1 = (OfficeMathMatrixRow) this.MemberwiseClone();
    owner1.SetOwner(owner);
    owner1.m_args = new OfficeMaths((IOfficeMathEntity) owner1);
    this.m_args.CloneItemsTo(owner1.m_args);
    return owner1;
  }

  internal override void Close()
  {
    if (this.m_args != null)
    {
      this.m_args.Close();
      this.m_args.Clear();
      this.m_args = (OfficeMaths) null;
    }
    base.Close();
  }

  internal void OnRowAdded()
  {
    OfficeMathMatrix ownerMathEntity = this.OwnerMathEntity as OfficeMathMatrix;
    if (ownerMathEntity.Columns.Count <= 0)
      return;
    ownerMathEntity.CreateArguments(0, this.RowIndex, ownerMathEntity.Columns.Count - 1, this.RowIndex);
  }
}
