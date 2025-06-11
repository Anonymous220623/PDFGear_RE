// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.OfficeMathRunElement
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

#nullable disable
namespace Syncfusion.Office;

internal class OfficeMathRunElement : 
  OfficeMathFunctionBase,
  IOfficeMathRunElement,
  IOfficeMathFunctionBase,
  IOfficeMathEntity
{
  internal IOfficeRun m_item;
  private OfficeMathFormat m_mathFormat;

  public IOfficeRun Item
  {
    get => this.m_item;
    set
    {
      this.m_item = value;
      this.m_item.OwnerMathRunElement = (IOfficeMathRunElement) this;
    }
  }

  public IOfficeMathFormat MathFormat => (IOfficeMathFormat) this.m_mathFormat;

  internal OfficeMathRunElement(IOfficeMathEntity owner)
    : base(owner)
  {
    this.m_type = MathFunctionType.RunElement;
    this.m_mathFormat = new OfficeMathFormat((IOfficeMathEntity) this);
  }

  internal override void Close()
  {
    if (this.m_mathFormat != null)
    {
      this.m_mathFormat.Close();
      this.m_mathFormat = (OfficeMathFormat) null;
    }
    if (this.m_item != null)
    {
      this.m_item.Dispose();
      this.m_item = (IOfficeRun) null;
    }
    base.Close();
  }

  internal override OfficeMathFunctionBase Clone(IOfficeMathEntity owner)
  {
    OfficeMathRunElement owner1 = (OfficeMathRunElement) this.MemberwiseClone();
    owner1.SetOwner(owner);
    owner1.m_mathFormat = this.m_mathFormat.Clone((IOfficeMathEntity) owner1);
    owner1.m_item = this.m_item.CloneRun();
    owner1.m_item.OwnerMathRunElement = (IOfficeMathRunElement) owner1;
    return (OfficeMathFunctionBase) owner1;
  }
}
