// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.OfficeMathLimit
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

#nullable disable
namespace Syncfusion.Office;

internal class OfficeMathLimit : 
  OfficeMathFunctionBase,
  IOfficeMathLimit,
  IOfficeMathFunctionBase,
  IOfficeMathEntity
{
  private MathLimitType m_limitType;
  private OfficeMath m_equation;
  private OfficeMath m_limit;
  internal IOfficeRunFormat m_controlProperties;

  public MathLimitType LimitType
  {
    get => this.m_limitType;
    set => this.m_limitType = value;
  }

  public IOfficeMath Equation => (IOfficeMath) this.m_equation;

  public IOfficeMath Limit => (IOfficeMath) this.m_limit;

  public IOfficeRunFormat ControlProperties
  {
    get
    {
      if (this.m_controlProperties == null)
        this.m_controlProperties = this.GetDefaultControlProperties();
      return this.m_controlProperties;
    }
    set => this.m_controlProperties = value;
  }

  internal OfficeMathLimit(IOfficeMathEntity owner)
    : base(owner)
  {
    this.m_type = MathFunctionType.Limit;
    this.m_equation = new OfficeMath((IOfficeMathEntity) this);
    this.m_limit = new OfficeMath((IOfficeMathEntity) this);
  }

  internal override void Close()
  {
    if (this.m_limit != null)
      this.m_limit.Close();
    if (this.m_equation != null)
      this.m_equation.Close();
    if (this.m_controlProperties != null)
    {
      this.m_controlProperties.Dispose();
      this.m_controlProperties = (IOfficeRunFormat) null;
    }
    base.Close();
  }

  internal override OfficeMathFunctionBase Clone(IOfficeMathEntity owner)
  {
    OfficeMathLimit owner1 = (OfficeMathLimit) this.MemberwiseClone();
    owner1.SetOwner(owner);
    if (owner1.m_controlProperties != null)
      owner1.m_controlProperties = this.m_controlProperties.Clone();
    owner1.m_equation = this.m_equation.CloneImpl((IOfficeMathEntity) owner1);
    owner1.m_limit = this.m_limit.CloneImpl((IOfficeMathEntity) owner1);
    return (OfficeMathFunctionBase) owner1;
  }
}
