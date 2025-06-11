// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.VbaProject
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using Syncfusion.Compression.Zip;
using System;
using System.Globalization;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Office;

internal class VbaProject : IVbaProject
{
  private VbaModulesCollection m_modules;
  private ReferenceRecordsCollection m_references;
  protected object m_parent;
  private SystemKind m_kind;
  private string m_name;
  private string m_description;
  private string m_password;
  private string m_constants;
  private bool m_IsViewLocked;
  private string m_helpFile1;
  private string m_helpFile2;
  private uint m_helpTopic;
  private uint m_lcId;
  private uint m_lcInvoke;
  private ushort m_codePage;
  private uint m_majorVersion;
  private ushort m_minorVersion;
  private string m_clsID;
  private string m_protectionData;
  private string m_passwordData;
  private string m_lockviewData;

  internal VbaProject(object parent)
  {
    this.m_parent = parent;
    this.m_name = "VBAProject";
    this.m_lcId = 1033U;
    this.m_lcInvoke = 1033U;
    this.m_codePage = (ushort) 1252;
    this.m_minorVersion = (ushort) 3;
    this.m_majorVersion = 1602932494U;
    this.m_clsID = "{EE89B4E9-F8E5-45FE-9D4D-0A42D7254B84}";
    this.m_kind = SystemKind.Win32;
    this.Description = string.Empty;
    this.HelpFile = string.Empty;
    this.SecondaryHelpFile = string.Empty;
    this.m_modules = new VbaModulesCollection(this);
  }

  internal SystemKind SystemType
  {
    get => this.m_kind;
    set => this.m_kind = value;
  }

  public string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  public string Description
  {
    get => this.m_description;
    set => this.m_description = value;
  }

  internal string Password
  {
    get => this.m_password;
    set => this.m_password = value;
  }

  public string Constants
  {
    get => this.m_constants;
    set => this.m_constants = value;
  }

  internal bool LockView
  {
    get => this.m_IsViewLocked;
    set => this.m_IsViewLocked = value;
  }

  public string HelpFile
  {
    get => this.m_helpFile1;
    set => this.m_helpFile1 = value;
  }

  internal string SecondaryHelpFile
  {
    get => this.m_helpFile2;
    set => this.m_helpFile2 = value;
  }

  public uint HelpContextId
  {
    get => this.m_helpTopic;
    set => this.m_helpTopic = value;
  }

  internal uint LcId
  {
    get => this.m_lcId;
    set => this.m_lcId = value;
  }

  internal uint LcInvoke
  {
    get => this.m_lcInvoke;
    set => this.m_lcInvoke = value;
  }

  internal ushort CodePage
  {
    get => this.m_codePage;
    set => this.m_codePage = value;
  }

  internal uint MajorVersion
  {
    get => this.m_majorVersion;
    set => this.m_majorVersion = value;
  }

  internal ushort MinorVersion
  {
    get => this.m_minorVersion;
    set => this.m_minorVersion = value;
  }

  internal Encoding EncodingType => Encoding.GetEncoding((int) this.CodePage);

  internal string ProjectCLSID
  {
    get => this.m_clsID;
    set => this.m_clsID = value;
  }

  public IVbaModules Modules
  {
    get => (IVbaModules) this.m_modules;
    set => this.m_modules = value as VbaModulesCollection;
  }

  internal ReferenceRecordsCollection References
  {
    get
    {
      if (this.m_references == null)
        this.m_references = new ReferenceRecordsCollection();
      return this.m_references;
    }
  }

  internal void ParseDirStream(Stream dirStream)
  {
    using (dirStream = VbaDataProcess.Decompress(dirStream))
    {
      dirStream.Position = 0L;
      this.ParseProjectInfo(dirStream);
      this.ParseReferences(dirStream);
      dirStream.Position += 6L;
      ushort num = ZipArchive.ReadUInt16(dirStream);
      dirStream.Position += 8L;
      for (; num > (ushort) 0; --num)
      {
        VbaModule vbaModule = new VbaModule(this.Modules as VbaModulesCollection);
        vbaModule.ParseModuleRecord(dirStream);
        (this.Modules as VbaModulesCollection).Add(vbaModule);
      }
      dirStream.Position += 6L;
    }
  }

  internal void ParseModuleStream(Stream dirStream, VbaModule module)
  {
    dirStream.Position += (long) module.ModuleOffSet;
    byte[] buffer = new byte[dirStream.Length - (long) module.ModuleOffSet];
    dirStream.Read(buffer, 0, buffer.Length);
    Stream stream;
    using (stream = VbaDataProcess.Decompress((Stream) new MemoryStream(buffer)))
    {
      byte[] array = (stream as MemoryStream).ToArray();
      string str1 = this.EncodingType.GetString(array, 0, array.Length);
      int startIndex1 = 0;
      int startIndex2 = 0;
      string str2;
      for (str2 = str1.Replace("\r\n", "\n"); str2.IndexOf("Attribute", startIndex1) == startIndex2; startIndex2 = startIndex1 + 1)
      {
        startIndex1 = str2.IndexOf("\n", startIndex2);
        string[] strArray = str2.Substring(startIndex2 + 10, startIndex1 - (startIndex2 + 10)).Split('=');
        module.Attributes.Add(new VbaAttribute()
        {
          Name = strArray[0].Trim(),
          Value = strArray[1].Trim().Replace("\"", string.Empty),
          IsText = strArray[1].Trim().StartsWith("\"")
        });
      }
      module.Code = str2.Substring(startIndex2);
    }
  }

  internal void ParseProjectStream(Stream stream)
  {
    byte[] numArray = new byte[stream.Length];
    stream.Read(numArray, 0, numArray.Length);
    string[] strArray1 = this.EncodingType.GetString(numArray, 0, numArray.Length).Split(new string[1]
    {
      "\r\n"
    }, StringSplitOptions.None);
    string str1 = (string) null;
    foreach (string str2 in strArray1)
    {
      string[] strArray2 = str2.Split('=');
      strArray2[0] = strArray2[0].Replace("\"", string.Empty);
      switch (strArray2[0])
      {
        case "ID":
          this.m_clsID = strArray2[1].Replace("\"", string.Empty);
          break;
        case "Package":
          str1 = strArray2[1].Replace("\"", string.Empty);
          break;
        case "Document":
          if (this.m_modules[strArray2[1].Substring(0, strArray2[1].IndexOf("/&H"))] is VbaModule module1)
          {
            module1.Type = VbaModuleType.Document;
            break;
          }
          break;
        case "Class":
          if (this.m_modules[strArray2[1]] is VbaModule module2)
          {
            module2.Type = VbaModuleType.ClassModule;
            break;
          }
          break;
        case "Module":
          if (this.m_modules[strArray2[1]] is VbaModule module3)
          {
            module3.Type = VbaModuleType.StdModule;
            break;
          }
          break;
        case "BaseClass":
          if (this.m_modules[strArray2[1]] is VbaModule module4)
          {
            module4.Type = VbaModuleType.MsForm;
            module4.Package = str1;
            break;
          }
          break;
        case "CMG":
          this.m_protectionData = str2;
          break;
        case "DPB":
          this.m_passwordData = str2;
          break;
        case "GC":
          this.m_lockviewData = str2;
          break;
      }
    }
  }

  internal void ParseProjectInfo(Stream dirData)
  {
    dirData.Position = 6L;
    this.SystemType = (SystemKind) VbaProject.ReadUInt32(dirData);
    if (VbaProject.ReadUInt16(dirData) == (ushort) 74)
      dirData.Position += 8L;
    else
      dirData.Position -= 2L;
    dirData.Position += 6L;
    this.LcId = VbaProject.ReadUInt32(dirData);
    dirData.Position += 6L;
    this.LcInvoke = VbaProject.ReadUInt32(dirData);
    dirData.Position += 6L;
    this.CodePage = VbaProject.ReadUInt16(dirData);
    dirData.Position += 2L;
    int count1 = (int) VbaProject.ReadUInt32(dirData);
    byte[] numArray1 = new byte[count1];
    dirData.Read(numArray1, 0, count1);
    Encoding encodingType = this.EncodingType;
    this.Name = encodingType.GetString(numArray1, 0, numArray1.Length);
    dirData.Position += 2L;
    int num1 = (int) VbaProject.ReadUInt32(dirData);
    dirData.Position += (long) num1;
    dirData.Position += 2L;
    int count2 = (int) VbaProject.ReadUInt32(dirData);
    byte[] numArray2 = new byte[count2];
    dirData.Read(numArray2, 0, count2);
    this.Description = Encoding.Unicode.GetString(numArray2, 0, numArray2.Length);
    dirData.Position += 2L;
    int count3 = (int) VbaProject.ReadUInt32(dirData);
    byte[] numArray3 = new byte[count3];
    dirData.Read(numArray3, 0, count3);
    this.HelpFile = encodingType.GetString(numArray3, 0, numArray3.Length);
    dirData.Position += 2L;
    int count4 = (int) VbaProject.ReadUInt32(dirData);
    byte[] numArray4 = new byte[count4];
    dirData.Read(numArray4, 0, count4);
    this.SecondaryHelpFile = encodingType.GetString(numArray4, 0, numArray4.Length);
    dirData.Position += 6L;
    this.HelpContextId = VbaProject.ReadUInt32(dirData);
    dirData.Position += 10L;
    dirData.Position += 6L;
    this.MajorVersion = VbaProject.ReadUInt32(dirData);
    this.MinorVersion = VbaProject.ReadUInt16(dirData);
    if (ZipArchive.ReadUInt16(dirData) == (ushort) 12)
    {
      int count5 = (int) VbaProject.ReadUInt32(dirData);
      byte[] numArray5 = new byte[count5];
      dirData.Read(numArray5, 0, count5);
      this.Constants = encodingType.GetString(numArray5, 0, numArray5.Length);
      dirData.Position += 2L;
      int num2 = (int) VbaProject.ReadUInt32(dirData);
      dirData.Position += (long) num2;
    }
    else
      dirData.Position -= 2L;
  }

  internal void ParseReferences(Stream dirData)
  {
    for (ushort type1 = VbaProject.ReadUInt16(dirData); type1 != (ushort) 15; type1 = VbaProject.ReadUInt16(dirData))
    {
      ReferenceRecord referenceRecord;
      if (type1 == (ushort) 22)
      {
        int count = (int) VbaProject.ReadUInt32(dirData);
        byte[] numArray = new byte[count];
        dirData.Read(numArray, 0, count);
        string str = this.EncodingType.GetString(numArray, 0, numArray.Length);
        ushort type2 = VbaProject.ReadUInt16(dirData);
        if (type2 == (ushort) 62)
        {
          int num = (int) VbaProject.ReadUInt32(dirData);
          dirData.Position += (long) num;
          type2 = VbaProject.ReadUInt16(dirData);
        }
        referenceRecord = this.References.Add((VbaReferenceType) type2);
        referenceRecord.Name = str;
      }
      else
        referenceRecord = this.References.Add((VbaReferenceType) type1);
      if (referenceRecord != null)
      {
        referenceRecord.EncodingType = this.EncodingType;
        referenceRecord.ParseRecord(dirData);
      }
    }
    dirData.Position -= 2L;
  }

  internal void SerializeVbaStream(Stream dirStream)
  {
    dirStream.Write(BitConverter.GetBytes(25036), 0, 2);
    dirStream.Write(BitConverter.GetBytes((int) ushort.MaxValue), 0, 2);
    dirStream.WriteByte((byte) 0);
    dirStream.Write(BitConverter.GetBytes(0), 0, 2);
    dirStream.Flush();
  }

  internal void SerializeDirStream(Stream dirStream)
  {
    Stream stream1 = (Stream) new MemoryStream();
    this.SerializeProjectInfo(stream1);
    this.SerializeReferences(stream1);
    this.SerializeModules(stream1);
    stream1.Write(BitConverter.GetBytes(16 /*0x10*/), 0, 2);
    stream1.Write(BitConverter.GetBytes(0), 0, 4);
    stream1.Position = 0L;
    Stream stream2 = VbaDataProcess.Compress((Stream) (stream1 as MemoryStream));
    stream2.Position = 0L;
    byte[] array = (stream2 as MemoryStream).ToArray();
    dirStream.Write(array, 0, array.Length);
    dirStream.Flush();
  }

  internal void SerializeProjectInfo(Stream dirStream)
  {
    dirStream.Write(BitConverter.GetBytes(1), 0, 2);
    dirStream.Write(BitConverter.GetBytes(4), 0, 4);
    dirStream.Write(BitConverter.GetBytes((int) this.m_kind), 0, 4);
    dirStream.Write(BitConverter.GetBytes(2), 0, 2);
    dirStream.Write(BitConverter.GetBytes(4), 0, 4);
    dirStream.Write(BitConverter.GetBytes(1033), 0, 4);
    dirStream.Write(BitConverter.GetBytes(20), 0, 2);
    dirStream.Write(BitConverter.GetBytes(4), 0, 4);
    dirStream.Write(BitConverter.GetBytes(1033), 0, 4);
    dirStream.Write(BitConverter.GetBytes(3), 0, 2);
    dirStream.Write(BitConverter.GetBytes(2), 0, 4);
    dirStream.Write(BitConverter.GetBytes(this.CodePage), 0, 2);
    byte[] bytes1 = this.EncodingType.GetBytes(this.Name);
    dirStream.Write(BitConverter.GetBytes(4), 0, 2);
    dirStream.Write(BitConverter.GetBytes(bytes1.Length), 0, 4);
    dirStream.Write(bytes1, 0, bytes1.Length);
    byte[] bytes2 = this.EncodingType.GetBytes(this.Description);
    dirStream.Write(BitConverter.GetBytes(5), 0, 2);
    dirStream.Write(BitConverter.GetBytes(bytes2.Length), 0, 4);
    dirStream.Write(bytes2, 0, bytes2.Length);
    byte[] bytes3 = Encoding.Unicode.GetBytes(this.Description);
    dirStream.Write(BitConverter.GetBytes(64 /*0x40*/), 0, 2);
    dirStream.Write(BitConverter.GetBytes(bytes3.Length), 0, 4);
    dirStream.Write(bytes3, 0, bytes3.Length);
    byte[] bytes4 = this.EncodingType.GetBytes(this.m_helpFile1);
    dirStream.Write(BitConverter.GetBytes(6), 0, 2);
    dirStream.Write(BitConverter.GetBytes(bytes4.Length), 0, 4);
    dirStream.Write(bytes4, 0, bytes4.Length);
    byte[] bytes5 = Encoding.Unicode.GetBytes(this.m_helpFile2);
    dirStream.Write(BitConverter.GetBytes(61), 0, 2);
    dirStream.Write(BitConverter.GetBytes(bytes5.Length), 0, 4);
    dirStream.Write(bytes5, 0, bytes5.Length);
    dirStream.Write(BitConverter.GetBytes(7), 0, 2);
    dirStream.Write(BitConverter.GetBytes(4), 0, 4);
    dirStream.Write(BitConverter.GetBytes(this.HelpContextId), 0, 4);
    dirStream.Write(BitConverter.GetBytes(8), 0, 2);
    dirStream.Write(BitConverter.GetBytes(4), 0, 4);
    dirStream.Write(BitConverter.GetBytes(0), 0, 4);
    dirStream.Write(BitConverter.GetBytes(9), 0, 2);
    dirStream.Write(BitConverter.GetBytes(4), 0, 4);
    dirStream.Write(BitConverter.GetBytes(this.MajorVersion), 0, 4);
    dirStream.Write(BitConverter.GetBytes(this.MinorVersion), 0, 2);
    if (string.IsNullOrEmpty(this.Constants))
      return;
    byte[] bytes6 = this.EncodingType.GetBytes(this.Constants);
    if (bytes6.Length > 1015)
      throw new Exception("Constants length should be less than or equal to 1015 characters");
    dirStream.Write(BitConverter.GetBytes(12), 0, 2);
    dirStream.Write(BitConverter.GetBytes(bytes6.Length), 0, 4);
    dirStream.Write(bytes6, 0, bytes6.Length);
    byte[] bytes7 = Encoding.Unicode.GetBytes(this.Constants);
    dirStream.Write(BitConverter.GetBytes(60), 0, 2);
    dirStream.Write(BitConverter.GetBytes(bytes7.Length), 0, 4);
    dirStream.Write(bytes7, 0, bytes7.Length);
  }

  internal void SerializeReferences(Stream stream)
  {
    foreach (ReferenceRecord reference in (CollectionBase<ReferenceRecord>) this.References)
      reference.SerializeRecord(stream);
  }

  internal void SerializeModules(Stream stream)
  {
    stream.Write(BitConverter.GetBytes(15), 0, 2);
    stream.Write(BitConverter.GetBytes(2), 0, 4);
    stream.Write(BitConverter.GetBytes(this.m_modules.Count), 0, 2);
    long num = stream.Position - 2L;
    stream.Write(BitConverter.GetBytes(19), 0, 2);
    stream.Write(BitConverter.GetBytes(2), 0, 4);
    stream.Write(BitConverter.GetBytes((int) ushort.MaxValue), 0, 2);
    int count = this.m_modules.Count;
    foreach (VbaModule module in (CollectionBase<VbaModule>) this.m_modules)
    {
      if (module.Type == VbaModuleType.MsForm && module.DesignerStorage == null)
      {
        --count;
        long position = stream.Position;
        stream.Position = num;
        stream.Write(BitConverter.GetBytes(count), 0, 2);
        stream.Position = position;
      }
      else
        module.SerializeModuleRecord(stream);
    }
  }

  internal void SerializeModuleStream(VbaModule module, Stream stream)
  {
    string str = "";
    foreach (VbaAttribute attribute in (CollectionBase<VbaAttribute>) module.Attributes)
    {
      str = $"{str}Attribute {attribute.Name} = ";
      str = !attribute.IsText ? str + attribute.Value : $"{str}\"{attribute.Value}\"";
      str += "\r\n";
    }
    byte[] array = (VbaDataProcess.Compress((Stream) new MemoryStream(this.EncodingType.GetBytes(str + module.Code))) as MemoryStream).ToArray();
    stream.Write(array, 0, array.Length);
    stream.Flush();
  }

  internal void SerializeProjectStream(Stream stream)
  {
    string str1 = $"{(string) null}ID=\"{this.m_clsID}\"\r\n";
    foreach (VbaModule module in (CollectionBase<VbaModule>) this.m_modules)
    {
      if (module.Type != VbaModuleType.MsForm || module.DesignerStorage != null)
      {
        switch (module.Type)
        {
          case VbaModuleType.StdModule:
            str1 = $"{str1}Module={module.Name}\r\n";
            continue;
          case VbaModuleType.ClassModule:
            str1 = $"{str1}Class={module.Name}\r\n";
            continue;
          case VbaModuleType.MsForm:
            if (!string.IsNullOrEmpty(module.Package))
              str1 = $"{str1}Package={module.Package}\r\n";
            str1 = $"{str1}BaseClass={module.Name}\r\n";
            continue;
          case VbaModuleType.Document:
            str1 = $"{str1}Document={module.Name}/&H00000000\r\n";
            continue;
          default:
            continue;
        }
      }
    }
    if (!string.IsNullOrEmpty(this.HelpFile))
      str1 = $"{str1}HelpFile=\"{this.HelpFile}\"\r\n";
    string str2 = $"{$"{str1}Name=\"{this.Name}\"\r\n"}HelpContextID={(object) this.HelpContextId}\r\n";
    if (!string.IsNullOrEmpty(this.Description))
      str2 = $"{str2}Description=\"{this.Description}\"\r\n";
    string str3 = str2 + "VersionCompatible32=\"393222000\"\r\n";
    if (!string.IsNullOrEmpty(this.m_protectionData))
      str3 = $"{str3}{this.m_protectionData}\r\n";
    if (!string.IsNullOrEmpty(this.m_passwordData))
      str3 = $"{str3}{this.m_passwordData}\r\n";
    if (!string.IsNullOrEmpty(this.m_lockviewData))
      str3 = $"{str3}{this.m_lockviewData}\r\n";
    string s = str3 + "\r\n" + "[Host Extender Info]\r\n" + "&H00000001={3832D640-CF90-11CF-8E43-00A0C911005A};VBE;&H00000000\r\n" + "\r\n" + "[Workspace]\r\n";
    foreach (VbaModule module in (CollectionBase<VbaModule>) this.m_modules)
    {
      if (module.Type != VbaModuleType.MsForm || module.DesignerStorage != null)
        s = $"{s}{module.Name}=0, 0, 0, 0, C \r\n";
    }
    byte[] bytes = this.EncodingType.GetBytes(s);
    stream.Write(bytes, 0, bytes.Length);
    stream.Flush();
  }

  internal void SerializeProjectWmStream(Stream projectWm)
  {
    foreach (VbaModule module in (CollectionBase<VbaModule>) this.m_modules)
    {
      if (module.Type != VbaModuleType.MsForm || module.DesignerStorage != null)
      {
        byte[] bytes1 = this.EncodingType.GetBytes(module.Name + "\0");
        projectWm.Write(bytes1, 0, bytes1.Length);
        byte[] bytes2 = Encoding.Unicode.GetBytes(module.Name + "\0");
        projectWm.Write(bytes2, 0, bytes2.Length);
      }
    }
    projectWm.Write(BitConverter.GetBytes(0), 0, 2);
    projectWm.Flush();
  }

  private byte[] ConvertHexString(string value)
  {
    switch (value)
    {
      case null:
        throw new ArgumentNullException(value);
      case "":
        return new byte[0];
      default:
        if (value.Length % 2 != 0)
          throw new ArgumentException(value);
        int length = value.Length >> 1;
        byte[] numArray = new byte[length];
        for (int index = 0; index < length; ++index)
          numArray[index] = byte.Parse(value.Substring(index * 2, 2), NumberStyles.HexNumber);
        return numArray;
    }
  }

  private string ConvertByteArray(byte[] value)
  {
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    if (value.Length == 0)
      return string.Empty;
    int num = value.Length % 2 == 0 ? value.Length : throw new ArgumentException(nameof (value));
    string empty = string.Empty;
    for (int index = 0; index < num; ++index)
      empty += $"{value[index]:x2}";
    return empty.ToUpperInvariant();
  }

  public static uint ReadUInt32(Stream stream)
  {
    byte[] buffer = new byte[4];
    return stream.Read(buffer, 0, 4) == 4 ? BitConverter.ToUInt32(buffer, 0) : throw new Exception("Unable to read value at the specified position - end of stream was reached.");
  }

  public static short ReadInt16(Stream stream)
  {
    byte[] buffer = new byte[2];
    return stream.Read(buffer, 0, 2) == 2 ? BitConverter.ToInt16(buffer, 0) : throw new Exception("Unable to read value at the specified position - end of stream was reached.");
  }

  public static ushort ReadUInt16(Stream stream)
  {
    byte[] buffer = new byte[2];
    return stream.Read(buffer, 0, 2) == 2 ? BitConverter.ToUInt16(buffer, 0) : throw new Exception("Unable to read value at the specified position - end of stream was reached.");
  }

  internal void Dispose()
  {
    if (this.m_modules != null)
    {
      this.m_modules.Dispose();
      this.m_modules = (VbaModulesCollection) null;
    }
    if (this.m_references == null)
      return;
    this.m_references.Dispose();
    this.m_references = (ReferenceRecordsCollection) null;
  }

  internal VbaProject Clone(object parent)
  {
    VbaProject parent1 = (VbaProject) this.MemberwiseClone();
    parent1.m_parent = parent;
    if (this.m_references != null)
      parent1.m_references = this.m_references.Clone(parent1);
    if (this.m_modules != null)
      parent1.m_modules = this.m_modules.Clone(parent1);
    return parent1;
  }
}
