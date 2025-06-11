// Decompiled with JetBrains decompiler
// Type: RegExt.FileAssociations.RegisterProgram
// Assembly: RegExt, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: DBF16820-DB7E-4C29-8C11-DD0B94851B7F
// Assembly location: C:\Program Files\PDFgear\RegExt.exe

using Microsoft.Win32;
using System;

#nullable disable
namespace RegExt.FileAssociations;

internal class RegisterProgram
{
  public string ProgramId { get; }

  public string TypeName { get; set; }

  public string FriendlyTypeName { get; set; }

  public string DefaultIcon { get; set; }

  public bool? IsAlwaysShowExt { get; set; }

  public string Operation { get; set; }

  public string Command { get; set; }

  public string FriendlyAppName { get; set; }

  public string AppUserModelID { get; set; }

  public RegisterProgram(string programId)
  {
    this.ProgramId = !string.IsNullOrWhiteSpace(programId) ? programId : throw new ArgumentNullException(nameof (programId));
  }

  public void WriteToCurrentUser() => this.WriteToRegistry(RegistryHive.CurrentUser);

  public void WriteToAllUser() => this.WriteToRegistry(RegistryHive.LocalMachine);

  public void RemoveFromCurrentUser()
  {
    using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32))
      registryKey.DeleteSubKeyTree(this.BuildRegistryPath(this.ProgramId), false);
  }

  public void RemoveFromAllUser()
  {
    using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
      registryKey.DeleteSubKeyTree(this.BuildRegistryPath(this.ProgramId), false);
  }

  private void WriteToRegistry(RegistryHive registryHive)
  {
    registryHive.Write32(this.BuildRegistryPath(this.ProgramId), this.TypeName ?? string.Empty);
    if (this.FriendlyTypeName != null && !string.IsNullOrWhiteSpace(this.FriendlyTypeName))
      registryHive.Write32(this.BuildRegistryPath(this.ProgramId), "FriendlyTypeName", this.FriendlyTypeName);
    if (!string.IsNullOrWhiteSpace(this.AppUserModelID))
      registryHive.Write32(this.BuildRegistryPath(this.ProgramId), "AppUserModelID", this.AppUserModelID);
    if (this.IsAlwaysShowExt.HasValue)
      registryHive.Write32(this.BuildRegistryPath(this.ProgramId), "IsAlwaysShowExt", this.IsAlwaysShowExt.Value ? "1" : "0");
    if (this.DefaultIcon != null && !string.IsNullOrWhiteSpace(this.DefaultIcon))
      registryHive.Write32(this.BuildRegistryPath(this.ProgramId + "\\DefaultIcon"), this.DefaultIcon);
    if (this.Operation == null || string.IsNullOrWhiteSpace(this.Operation))
      return;
    registryHive.Write32(this.BuildRegistryPath($"{this.ProgramId}\\shell\\{this.Operation}\\command"), this.Command ?? string.Empty);
    if (string.IsNullOrWhiteSpace(this.FriendlyAppName))
      return;
    registryHive.Write32(this.BuildRegistryPath($"{this.ProgramId}\\shell\\{this.Operation}"), "FriendlyAppName", this.FriendlyAppName);
  }

  private string BuildRegistryPath(string relativePath) => "Software\\Classes\\" + relativePath;
}
