// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ExcelEngine
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Licensing;
using Syncfusion.XlsIO.Implementation;
using System;
using System.Diagnostics;
using System.Security;
using System.Security.Permissions;

#nullable disable
namespace Syncfusion.XlsIO;

public class ExcelEngine : IDisposable
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
    int num = ExcelEngine.IsSecurityGranted ? 1 : 0;
    bool flag = ExcelEngine.ValidateLicense();
    this.m_appl = new ApplicationImpl(this);
    this.m_appl.EvalExpired = flag;
  }

  ~ExcelEngine()
  {
    this.m_bAskSaveOnDestroy = false;
    this.Dispose();
  }

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
    get
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

  internal static bool ValidateLicense()
  {
    string empty = string.Empty;
    return !string.IsNullOrEmpty(FusionLicenseProvider.GetLicenseType(Platform.FileFormats));
  }
}
