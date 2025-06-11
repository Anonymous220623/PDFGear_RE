// Decompiled with JetBrains decompiler
// Type: RegExt.FileAssociations.RegisterFileExtension
// Assembly: RegExt, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: DBF16820-DB7E-4C29-8C11-DD0B94851B7F
// Assembly location: C:\Program Files\PDFgear\RegExt.exe

using Microsoft.Win32;
using System;
using System.Collections.Generic;

#nullable disable
namespace RegExt.FileAssociations;

internal class RegisterFileExtension
{
  public string FileExtension { get; }

  public string ContentType { get; set; }

  public string PerceivedType { get; set; }

  public string DefaultProgramId { get; set; }

  public IList<string> OpenWithProgramIds { get; set; } = (IList<string>) new List<string>();

  public RegisterFileExtension(string fileExtension)
  {
    if (string.IsNullOrWhiteSpace(fileExtension))
      throw new ArgumentNullException(nameof (fileExtension));
    this.FileExtension = fileExtension.StartsWith(".", StringComparison.Ordinal) ? fileExtension : throw new ArgumentException(fileExtension + " is not a valid file extension. it must start with \".\"", nameof (fileExtension));
  }

  public void WriteToCurrentUser() => this.WriteToRegistry(RegistryHive.CurrentUser);

  public void WriteToAllUser() => this.WriteToRegistry(RegistryHive.LocalMachine);

  private void WriteToRegistry(RegistryHive registryHive)
  {
    registryHive.Write32(this.BuildRegistryPath(this.FileExtension), this.DefaultProgramId ?? string.Empty);
    if (this.ContentType != null && !string.IsNullOrWhiteSpace(this.ContentType))
      registryHive.Write32(this.BuildRegistryPath(this.FileExtension), "Content Type", this.ContentType);
    if (this.PerceivedType != null && !string.IsNullOrWhiteSpace(this.PerceivedType))
      registryHive.Write32(this.BuildRegistryPath(this.FileExtension), "PerceivedType", this.PerceivedType);
    if (this.OpenWithProgramIds.Count <= 0)
      return;
    foreach (string openWithProgramId in (IEnumerable<string>) this.OpenWithProgramIds)
      registryHive.Write32(this.BuildRegistryPath(this.FileExtension + "\\OpenWithProgids"), openWithProgramId, string.Empty);
  }

  private string BuildRegistryPath(string relativePath) => "Software\\Classes\\" + relativePath;
}
