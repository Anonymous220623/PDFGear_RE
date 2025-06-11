// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.OfficeMathScript
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

#nullable disable
namespace Syncfusion.Office;

internal class OfficeMathScript : 
  OfficeMathFunctionBase,
  IOfficeMathScript,
  IOfficeMathFunctionBase,
  IOfficeMathEntity
{
  private MathScriptType m_scriptType;
  private OfficeMath m_equation;
  private OfficeMath m_script;
  internal IOfficeRunFormat m_controlProperties;

  public MathScriptType ScriptType
  {
    get => this.m_scriptType;
    set => this.m_scriptType = value;
  }

  public IOfficeMath Equation => (IOfficeMath) this.m_equation;

  public IOfficeMath Script => (IOfficeMath) this.m_script;

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

  internal OfficeMathScript(IOfficeMathEntity owner)
    : base(owner)
  {
    this.m_equation = new OfficeMath((IOfficeMathEntity) this);
    this.m_script = new OfficeMath((IOfficeMathEntity) this);
    this.m_type = MathFunctionType.SubSuperscript;
  }

  internal override void Close()
  {
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
    OfficeMathScript owner1 = (OfficeMathScript) this.MemberwiseClone();
    owner1.SetOwner(owner);
    owner1.m_equation = this.m_equation.CloneImpl((IOfficeMathEntity) owner1);
    if (owner1.m_controlProperties != null)
      owner1.m_controlProperties = this.m_controlProperties.Clone();
    return (OfficeMathFunctionBase) owner1;
  }
}
