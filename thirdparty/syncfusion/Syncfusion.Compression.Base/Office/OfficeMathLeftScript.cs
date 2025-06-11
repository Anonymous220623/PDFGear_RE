// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.OfficeMathLeftScript
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

#nullable disable
namespace Syncfusion.Office;

internal class OfficeMathLeftScript : 
  OfficeMathFunctionBase,
  IOfficeMathLeftScript,
  IOfficeMathFunctionBase,
  IOfficeMathEntity
{
  private OfficeMath m_equation;
  private OfficeMath m_subScript;
  private OfficeMath m_superScript;
  internal IOfficeRunFormat m_controlProperties;

  public IOfficeMath Equation => (IOfficeMath) this.m_equation;

  public IOfficeMath Subscript => (IOfficeMath) this.m_subScript;

  public IOfficeMath Superscript => (IOfficeMath) this.m_superScript;

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

  internal OfficeMathLeftScript(IOfficeMathEntity owner)
    : base(owner)
  {
    this.m_type = MathFunctionType.LeftSubSuperscript;
    this.m_equation = new OfficeMath((IOfficeMathEntity) this);
    this.m_subScript = new OfficeMath((IOfficeMathEntity) this);
    this.m_superScript = new OfficeMath((IOfficeMathEntity) this);
  }

  internal override void Close()
  {
    if (this.m_superScript != null)
      this.m_superScript.Close();
    if (this.m_subScript != null)
      this.m_subScript.Close();
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
    OfficeMathLeftScript owner1 = (OfficeMathLeftScript) this.MemberwiseClone();
    owner1.SetOwner(owner);
    if (owner1.m_controlProperties != null)
      owner1.m_controlProperties = this.m_controlProperties.Clone();
    owner1.m_equation = this.m_equation.CloneImpl((IOfficeMathEntity) owner1);
    owner1.m_subScript = this.m_subScript.CloneImpl((IOfficeMathEntity) owner1);
    owner1.m_superScript = this.m_superScript.CloneImpl((IOfficeMathEntity) owner1);
    return (OfficeMathFunctionBase) owner1;
  }
}
