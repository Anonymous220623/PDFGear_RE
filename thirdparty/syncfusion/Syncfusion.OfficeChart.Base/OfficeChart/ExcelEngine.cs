// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.ExcelEngine
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using System;
using System.Diagnostics;
using System.Security;
using System.Security.Permissions;

#nullable disable
namespace Syncfusion.OfficeChart;

internal class ExcelEngine : IDisposable
{
  private ApplicationImpl m_appl;
  private bool m_bDisposed;
  private bool m_bAskSaveOnDestroy;

  public IApplication Excel
  {
    [DebuggerStepThrough] get
    {
      if (this.m_bDisposed)
        throw new ObjectDisposedException("Application", "Cannot use dipose object.");
      return (IApplication) this.m_appl;
    }
  }

  public bool ThrowNotSavedOnDestroy
  {
    get => this.m_bAskSaveOnDestroy;
    set => this.m_bAskSaveOnDestroy = value;
  }

  public ExcelEngine()
  {
    bool flag = false;
    if (ExcelEngine.IsSecurityGranted)
      flag = ExcelEngine.ValidateLicense();
    this.m_appl = new ApplicationImpl(this);
    this.m_appl.EvalExpired = flag;
  }

  ~ExcelEngine() => this.Dispose();

  public void Dispose()
  {
    if (this.m_bDisposed)
      return;
    if (this.m_bAskSaveOnDestroy && !this.m_appl.IsSaved)
      throw new ExcelWorkbookNotSavedException("Object cannot be disposed. Save workbook or set property ThrowNotSavedOnDestoy to false.");
    IWorkbooks workbooks = this.m_appl.Workbooks;
    for (int Index = workbooks.Count - 1; Index >= 0; --Index)
    {
      (workbooks[Index] as WorkbookImpl).ClearExtendedFormats();
      workbooks[Index].Close();
    }
    this.m_appl.Dispose();
    this.m_appl = (ApplicationImpl) null;
    this.m_bDisposed = true;
    GC.SuppressFinalize((object) this);
  }

  internal static bool IsSecurityGranted
  {
    [SecuritySafeCritical] get
    {
      bool isSecurityGranted = false;
      SecurityPermission securityPermission = new SecurityPermission(PermissionState.Unrestricted);
      try
      {
        securityPermission.Demand();
        isSecurityGranted = true;
      }
      catch (SecurityException ex)
      {
      }
      return isSecurityGranted;
    }
  }

  internal static bool ValidateLicense() => false;
}
