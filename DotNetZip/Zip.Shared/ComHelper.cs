// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ComHelper
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Ionic.Zip;

[Guid("ebc25cf6-9120-4283-b972-0e5520d0000F")]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.AutoDispatch)]
public class ComHelper
{
  public bool IsZipFile(string filename) => ZipFile.IsZipFile(filename);

  public bool IsZipFileWithExtract(string filename) => ZipFile.IsZipFile(filename, true);

  public bool CheckZip(string filename) => ZipFile.CheckZip(filename);

  public bool CheckZipPassword(string filename, string password)
  {
    return ZipFile.CheckZipPassword(filename, password);
  }

  public void FixZipDirectory(string filename) => ZipFile.FixZipDirectory(filename);

  public string GetZipLibraryVersion() => ZipFile.LibraryVersion.ToString();
}
