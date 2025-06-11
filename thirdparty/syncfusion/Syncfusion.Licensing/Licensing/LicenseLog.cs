// Decompiled with JetBrains decompiler
// Type: Syncfusion.Licensing.LicenseLog
// Assembly: Syncfusion.Licensing, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=632609b4d040f6b4
// MVID: 46253E3A-52AF-49F3-BF00-D811A33B7BC6
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Licensing.dll

using System;
using System.Collections;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace Syncfusion.Licensing;

internal class LicenseLog
{
  public LicenseUsageMode UsageMode;
  public bool isASPDotNet;
  public string runtimeLicenseKey = "No runtime license key found in licx.";
  public string executingVersionInfo = "";
  public string executingVersionInfoFromAssembly = "";
  public ArrayList runtimeKeys = new ArrayList();
  public string ErrorMessage = "";
  public ArrayList licxFoundAssemblyList = new ArrayList();
  public ArrayList LicxEntries = new ArrayList();
  public bool isExpired;
  public bool failed;

  public LicenseError GetErrorKind()
  {
    if (!this.failed)
      return LicenseError.None;
    if (this.UsageMode == LicenseUsageMode.Runtime)
    {
      if (this.licxFoundAssemblyList.Count == 0)
        return LicenseError.RuntimeNoLicx;
      if (this.isExpired)
        return LicenseError.RuntimeKeyExpired;
      return this.runtimeKeys.Count == 0 ? LicenseError.RuntimeLicensedComponentEntryMissing : LicenseError.RuntimeNotLicensedControl;
    }
    if (this.isExpired)
      return LicenseError.DesigntimeKeyExpired;
    foreach (UnlockKeyInfo unlockKeyInfo in Sweep.GetUnlockKeyInfos())
    {
      if (unlockKeyInfo.encodedVersion != null && this.executingVersionInfo.IndexOf(unlockKeyInfo.encodedVersion.Replace("*", "0")) != -1)
        return LicenseError.DesigntimeNotLicensedControl;
    }
    return LicenseError.DesigntimeNoKey;
  }

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.AppendFormat("Syncfusion Licensing System - Detailed diagnostics.");
    stringBuilder.Append(Environment.NewLine);
    stringBuilder.Append(Environment.NewLine);
    stringBuilder.AppendFormat("ErrorKind = {0}", (object) this.GetErrorKind());
    stringBuilder.Append(Environment.NewLine);
    stringBuilder.Append(Environment.NewLine);
    stringBuilder.AppendFormat("UsageMode = {0}", (object) this.UsageMode);
    stringBuilder.Append(Environment.NewLine);
    stringBuilder.Append(Environment.NewLine);
    stringBuilder.AppendFormat("IsAspDotNet = {0}", (object) this.isASPDotNet);
    stringBuilder.Append(Environment.NewLine);
    stringBuilder.Append(Environment.NewLine);
    stringBuilder.AppendFormat("RuntimeLicenseKey = {0}", (object) this.runtimeLicenseKey);
    stringBuilder.Append(Environment.NewLine);
    stringBuilder.Append(Environment.NewLine);
    stringBuilder.AppendFormat("ExecutingVersionInfo = {0}", (object) this.executingVersionInfo);
    stringBuilder.Append(Environment.NewLine);
    stringBuilder.Append(Environment.NewLine);
    stringBuilder.AppendFormat("ExecutingVersionInfoFromAssembly = {0}", (object) this.executingVersionInfoFromAssembly);
    stringBuilder.Append(Environment.NewLine);
    stringBuilder.Append(Environment.NewLine);
    stringBuilder.AppendFormat("ErrorMessage = {0}", (object) this.ErrorMessage);
    stringBuilder.Append(Environment.NewLine);
    stringBuilder.Append(Environment.NewLine);
    if (this.runtimeKeys.Count > 0)
    {
      stringBuilder.Append("Runtime Keys:");
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(Environment.NewLine);
      try
      {
        foreach (RuntimeKeyInfo runtimeKey in this.runtimeKeys)
        {
          if (runtimeKey.unlockKeyInfo != null)
            stringBuilder.Append(runtimeKey.unlockKeyInfo.ToString());
          stringBuilder.Append(Environment.NewLine);
        }
      }
      catch (Exception ex)
      {
        stringBuilder.Append(ex.Message);
      }
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(Environment.NewLine);
    }
    if (this.licxFoundAssemblyList.Count > 0)
    {
      stringBuilder.Append("Assemblies with licenses.licx resource:");
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(Environment.NewLine);
      try
      {
        foreach (string licxFoundAssembly in this.licxFoundAssemblyList)
        {
          stringBuilder.Append(licxFoundAssembly);
          stringBuilder.Append(Environment.NewLine);
        }
      }
      catch (Exception ex)
      {
        stringBuilder.Append(ex.Message);
      }
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(Environment.NewLine);
    }
    else
    {
      stringBuilder.Append("No Assemblies with licenses.licx resource found.");
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(Environment.NewLine);
    }
    if (this.LicxEntries.Count > 0)
    {
      stringBuilder.Append("Syncfusion.Core entries found in licenses.licx files:");
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(Environment.NewLine);
      try
      {
        foreach (string licxEntry in this.LicxEntries)
        {
          stringBuilder.Append(licxEntry);
          stringBuilder.Append(Environment.NewLine);
        }
      }
      catch (Exception ex)
      {
        stringBuilder.Append(ex.Message);
      }
    }
    else
    {
      stringBuilder.Append("No Syncfusion.Core entries found in licenses.licx files.");
      stringBuilder.Append(Environment.NewLine);
    }
    return stringBuilder.ToString();
  }

  public bool HasLicxEntries() => this.LicxEntries.Count > 0;
}
