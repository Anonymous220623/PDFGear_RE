// Decompiled with JetBrains decompiler
// Type: Syncfusion.Licensing.UnlockKeyInfo
// Assembly: Syncfusion.Licensing, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=632609b4d040f6b4
// MVID: 46253E3A-52AF-49F3-BF00-D811A33B7BC6
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Licensing.dll

using Syncfusion.Lic.util.encoders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace Syncfusion.Licensing;

[EditorBrowsable(EditorBrowsableState.Never)]
public class UnlockKeyInfo
{
  public string fullKey = "";
  public string version = "";
  public string encodedKeyPart = "";
  public string encodedVersion = "";
  public KeyLogic kc;
  public string[] assemblies = new string[0];
  public string filterVersion = "";
  public int[] prodArray;

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(Environment.NewLine);
    stringBuilder.Append(Environment.NewLine);
    if (this.IsPossiblyValidKey(this.fullKey))
    {
      string[] strArray = this.ProductNamesFromAssemblyNames(this.assemblies);
      int index = 0;
      if (index < strArray.Length)
      {
        string str = strArray[index];
        if (str.ToLower().Contains("dashboardplatform"))
          stringBuilder.AppendFormat("This key licenses Syncfusion Dashboard Platform version {0}", (object) this.version);
        else if (str.ToLower().Contains("reportplatform"))
          stringBuilder.AppendFormat("This key licenses Syncfusion Report Platform version {0}", (object) this.version);
        else
          stringBuilder.AppendFormat("This key licenses Essential Studio version {0}", (object) this.version);
      }
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(Environment.NewLine);
    }
    else
    {
      stringBuilder.Append("This key does not appear to be valid");
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(Environment.NewLine);
    }
    if (this.kc != null)
    {
      if (this.encodedVersion != this.version)
      {
        stringBuilder.Append("Invalid Key - encoded version not matching.");
      }
      else
      {
        if (this.kc.Type == 1UL)
        {
          stringBuilder.AppendFormat("Evaluation Expiry Date - {0}", (object) this.kc.DateExpire);
          stringBuilder.Append(Environment.NewLine);
          stringBuilder.Append(Environment.NewLine);
        }
        else if (this.kc.Type == 2UL)
        {
          stringBuilder.Append("LinkLicense");
          stringBuilder.Append(Environment.NewLine);
          stringBuilder.Append(Environment.NewLine);
        }
        stringBuilder.AppendFormat("Syncfusion products licensed with this key are listed below - ");
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append(string.Join(Environment.NewLine, this.ProductNamesFromAssemblyNames(this.assemblies)));
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.AppendFormat("Diagnostic - Email Checksum = H{0:x}", (object) this.kc.EmailChecksum);
      }
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(Environment.NewLine);
    }
    return stringBuilder.ToString();
  }

  public UnlockKeyInfo(string key)
    : this(key, "")
  {
  }

  private string DecryptString(string encrString)
  {
    try
    {
      return Encoding.ASCII.GetString(Convert.FromBase64String(encrString));
    }
    catch
    {
      throw;
    }
  }

  public UnlockKeyInfo(string key, string filterVersion)
  {
    if (filterVersion != null && filterVersion.EndsWith(".*"))
      this.filterVersion = filterVersion;
    this.fullKey = key;
    if (!this.IsPossiblyValidKey(key))
    {
      this.version = "Unable to interpret key. Key may be obsolete or invalid.";
    }
    else
    {
      this.version = UnlockKeyInfo.ExtractPrefixVersionInfo(key);
      if (this.filterVersion != "" && this.version != this.filterVersion)
        return;
      this.encodedKeyPart = this.version[1].Equals('.') || int.Parse(this.version.Substring(0, 2)) < 10 ? key.Substring(11) : key.Substring(13);
      this.kc = new KeyLogic(int.Parse(this.version.Substring(0, 2).Trim('.')), PublicKeyDecryptProxy.SyncfusionDecode(this.encodedKeyPart));
      this.encodedVersion = $"{this.kc.V1.ToString()}{(object) '.'}{this.kc.V2.ToString()}{(object) '.'}{this.kc.V3.ToString()}.*";
      Sweep.GetProdArray(this.kc.Prods, ref this.prodArray);
      this.assemblies = (string[]) this.DecodeAssembliesFromProductIds(this.prodArray).ToArray(typeof (string));
    }
  }

  private bool IsPossiblyValidKey(string key) => key.StartsWith("@") && key.EndsWith("=");

  private ArrayList DecodeAssembliesFromProductIds(int[] prodArray)
  {
    Sweep.GetS1();
    string[] strArray1 = Sweep.s1.Split('|');
    ArrayList assms = new ArrayList();
    foreach (string str1 in strArray1)
    {
      char[] chArray1 = new char[1]{ ',' };
      string[] strArray2 = str1.Split(chArray1);
      string str2 = strArray2[1];
      char[] chArray2 = new char[1]{ ';' };
      foreach (string s in str2.Split(chArray2))
      {
        try
        {
          int result;
          if (int.TryParse(s, out result))
          {
            if (result < prodArray.Length)
            {
              if (prodArray[result] == 1)
                assms.Add((object) strArray2[0]);
            }
          }
        }
        catch
        {
        }
      }
    }
    assms.Sort();
    return this.RemoveDuplicates(assms);
  }

  public ArrayList RemoveDuplicates(ArrayList assms)
  {
    ArrayList arrayList = new ArrayList();
    string str = "";
    foreach (string assm in assms)
    {
      if (!(assm == str))
      {
        str = assm;
        arrayList.Add((object) assm);
      }
    }
    return arrayList;
  }

  public static string ByteArray2String(byte[] bs)
  {
    StringBuilder stringBuilder = new StringBuilder();
    for (int index = 0; index < bs.Length; ++index)
      stringBuilder.Append(Convert.ToChar(bs[index]));
    return stringBuilder.ToString();
  }

  public static string ExtractPrefixVersionInfo(string str)
  {
    str = UnlockKeyInfo.ByteArray2String(Hex.decode(str.Substring(1)));
    string[] strArray = str.Split('.');
    string str1 = strArray[2];
    if (str1.Length > 1)
      str1 = str1.Substring(0, 1);
    return $"{strArray[0]}{(object) '.'}{strArray[1]}{(object) '.'}{str1.Substring(0, 1)}.*";
  }

  private string[] ProductNamesFromAssemblyNames(string[] assemblyNames)
  {
    if (assemblyNames.GetLength(0) <= 0)
      return assemblyNames;
    string[] strArray1 = new string[assemblyNames.GetLength(0)];
    for (int index = 0; index < assemblyNames.GetLength(0); ++index)
    {
      string[] strArray2 = assemblyNames[index].Split('.');
      if (strArray2[1] != "core")
        strArray1[index] = !(strArray2[1] == "shared") ? (strArray2[1].ToLower().Contains("dashboardplatform") || strArray2[1].ToLower().Contains("reportplatform") || strArray2[1].ToLower().Contains("sdk") ? $"Syncfusion {this.ToCapitalCase(strArray2[1])}" : $"Syncfusion Essential {this.ToCapitalCase(strArray2[1])}") : $"Syncfusion Infrastructure Library {this.ToCapitalCase(strArray2[1])}";
    }
    List<string> stringList = new List<string>();
    foreach (string str in strArray1)
    {
      if (!string.IsNullOrEmpty(str))
        stringList.Add(str);
    }
    return stringList.ToArray();
  }

  private string ToCapitalCase(string s)
  {
    if (s == string.Empty)
      return s;
    char[] charArray = s.ToCharArray();
    charArray[0] = new string(charArray[0], 1).ToUpper().ToCharArray()[0];
    return new string(charArray);
  }
}
