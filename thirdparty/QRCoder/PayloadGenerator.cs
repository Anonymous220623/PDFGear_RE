// Decompiled with JetBrains decompiler
// Type: QRCoder.PayloadGenerator
// Assembly: QRCoder, Version=1.4.3.0, Culture=neutral, PublicKeyToken=c4ed5b9ae8358a28
// MVID: 5D0AD632-49D4-4E68-92A0-261964209740
// Assembly location: D:\PDFGear\bin\QRCoder.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace QRCoder;

public static class PayloadGenerator
{
  private static bool IsValidIban(string iban)
  {
    string input = iban.ToUpper().Replace(" ", "").Replace("-", "");
    bool flag1 = Regex.IsMatch(input, "^[a-zA-Z]{2}[0-9]{2}([a-zA-Z0-9]?){16,30}$");
    string str = ((IEnumerable<char>) (input.Substring(4) + input.Substring(0, 4)).ToCharArray()).Aggregate<char, string>("", (Func<string, char, string>) ((current, c) => current + (char.IsLetter(c) ? ((int) c - 55).ToString() : c.ToString())));
    int result = 0;
    for (int index = 0; index < (int) Math.Ceiling((double) (str.Length - 2) / 7.0); ++index)
    {
      int num = index == 0 ? 0 : 2;
      int startIndex = index * 7 + num;
      if (int.TryParse((index == 0 ? "" : result.ToString()) + str.Substring(startIndex, Math.Min(9 - num, str.Length - startIndex)), NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result))
        result %= 97;
      else
        break;
    }
    bool flag2 = result == 1;
    return flag1 & flag2;
  }

  private static bool IsValidQRIban(string iban)
  {
    bool flag = false;
    try
    {
      int int32 = Convert.ToInt32(iban.ToUpper().Replace(" ", "").Replace("-", "").Substring(4, 5));
      flag = int32 >= 30000 && int32 <= 31999;
    }
    catch
    {
    }
    return PayloadGenerator.IsValidIban(iban) & flag;
  }

  private static bool IsValidBic(string bic)
  {
    return Regex.IsMatch(bic.Replace(" ", ""), "^([a-zA-Z]{4}[a-zA-Z]{2}[a-zA-Z0-9]{2}([a-zA-Z0-9]{3})?)$");
  }

  private static string ConvertStringToEncoding(string message, string encoding)
  {
    Encoding encoding1 = Encoding.GetEncoding(encoding);
    Encoding utF8 = Encoding.UTF8;
    byte[] bytes1 = utF8.GetBytes(message);
    byte[] bytes2 = Encoding.Convert(utF8, encoding1, bytes1);
    return encoding1.GetString(bytes2, 0, bytes2.Length);
  }

  private static string EscapeInput(string inp, bool simple = false)
  {
    char[] chArray = new char[4]{ '\\', ';', ',', ':' };
    if (simple)
      chArray = new char[1]{ ':' };
    foreach (char ch in chArray)
      inp = inp.Replace(ch.ToString(), "\\" + ch.ToString());
    return inp;
  }

  public static bool ChecksumMod10(string digits)
  {
    if (string.IsNullOrEmpty(digits) || digits.Length < 2)
      return false;
    int[] numArray = new int[10]
    {
      0,
      9,
      4,
      6,
      8,
      2,
      7,
      1,
      3,
      5
    };
    int num1 = 0;
    for (int index = 0; index < digits.Length - 1; ++index)
    {
      int num2 = Convert.ToInt32(digits[index]) - 48 /*0x30*/;
      num1 = numArray[(num2 + num1) % 10];
    }
    return (10 - num1) % 10 == Convert.ToInt32(digits[digits.Length - 1]) - 48 /*0x30*/;
  }

  private static bool isHexStyle(string inp)
  {
    return Regex.IsMatch(inp, "\\A\\b[0-9a-fA-F]+\\b\\Z") || Regex.IsMatch(inp, "\\A\\b(0[xX])?[0-9a-fA-F]+\\b\\Z");
  }

  public abstract class Payload
  {
    public virtual int Version => -1;

    public virtual QRCodeGenerator.ECCLevel EccLevel => QRCodeGenerator.ECCLevel.M;

    public virtual QRCodeGenerator.EciMode EciMode => QRCodeGenerator.EciMode.Default;

    public abstract override string ToString();
  }

  public class WiFi : PayloadGenerator.Payload
  {
    private readonly string ssid;
    private readonly string password;
    private readonly string authenticationMode;
    private readonly bool isHiddenSsid;

    public WiFi(
      string ssid,
      string password,
      PayloadGenerator.WiFi.Authentication authenticationMode,
      bool isHiddenSSID = false,
      bool escapeHexStrings = true)
    {
      this.ssid = PayloadGenerator.EscapeInput(ssid);
      this.ssid = !escapeHexStrings || !PayloadGenerator.isHexStyle(this.ssid) ? this.ssid : $"\"{this.ssid}\"";
      this.password = PayloadGenerator.EscapeInput(password);
      this.password = !escapeHexStrings || !PayloadGenerator.isHexStyle(this.password) ? this.password : $"\"{this.password}\"";
      this.authenticationMode = authenticationMode.ToString();
      this.isHiddenSsid = isHiddenSSID;
    }

    public override string ToString()
    {
      return $"WIFI:T:{this.authenticationMode};S:{this.ssid};P:{this.password};{(this.isHiddenSsid ? "H:true" : string.Empty)};";
    }

    public enum Authentication
    {
      WEP,
      WPA,
      nopass,
    }
  }

  public class Mail : PayloadGenerator.Payload
  {
    private readonly string mailReceiver;
    private readonly string subject;
    private readonly string message;
    private readonly PayloadGenerator.Mail.MailEncoding encoding;

    public Mail(
      string mailReceiver = null,
      string subject = null,
      string message = null,
      PayloadGenerator.Mail.MailEncoding encoding = PayloadGenerator.Mail.MailEncoding.MAILTO)
    {
      this.mailReceiver = mailReceiver;
      this.subject = subject;
      this.message = message;
      this.encoding = encoding;
    }

    public override string ToString()
    {
      string str = string.Empty;
      switch (this.encoding)
      {
        case PayloadGenerator.Mail.MailEncoding.MAILTO:
          List<string> source = new List<string>();
          if (!string.IsNullOrEmpty(this.subject))
            source.Add("subject=" + Uri.EscapeDataString(this.subject));
          if (!string.IsNullOrEmpty(this.message))
            source.Add("body=" + Uri.EscapeDataString(this.message));
          str = $"mailto:{this.mailReceiver}{(source.Any<string>() ? "?" + string.Join("&", source.ToArray()) : "")}";
          break;
        case PayloadGenerator.Mail.MailEncoding.MATMSG:
          str = $"MATMSG:TO:{this.mailReceiver};SUB:{PayloadGenerator.EscapeInput(this.subject)};BODY:{PayloadGenerator.EscapeInput(this.message)};;";
          break;
        case PayloadGenerator.Mail.MailEncoding.SMTP:
          str = $"SMTP:{this.mailReceiver}:{PayloadGenerator.EscapeInput(this.subject, true)}:{PayloadGenerator.EscapeInput(this.message, true)}";
          break;
      }
      return str;
    }

    public enum MailEncoding
    {
      MAILTO,
      MATMSG,
      SMTP,
    }
  }

  public class SMS : PayloadGenerator.Payload
  {
    private readonly string number;
    private readonly string subject;
    private readonly PayloadGenerator.SMS.SMSEncoding encoding;

    public SMS(string number, PayloadGenerator.SMS.SMSEncoding encoding = PayloadGenerator.SMS.SMSEncoding.SMS)
    {
      this.number = number;
      this.subject = string.Empty;
      this.encoding = encoding;
    }

    public SMS(string number, string subject, PayloadGenerator.SMS.SMSEncoding encoding = PayloadGenerator.SMS.SMSEncoding.SMS)
    {
      this.number = number;
      this.subject = subject;
      this.encoding = encoding;
    }

    public override string ToString()
    {
      string str1 = string.Empty;
      switch (this.encoding)
      {
        case PayloadGenerator.SMS.SMSEncoding.SMS:
          string str2 = string.Empty;
          if (!string.IsNullOrEmpty(this.subject))
            str2 = "?body=" + Uri.EscapeDataString(this.subject);
          str1 = $"sms:{this.number}{str2}";
          break;
        case PayloadGenerator.SMS.SMSEncoding.SMSTO:
          str1 = $"SMSTO:{this.number}:{this.subject}";
          break;
        case PayloadGenerator.SMS.SMSEncoding.SMS_iOS:
          string str3 = string.Empty;
          if (!string.IsNullOrEmpty(this.subject))
            str3 = ";body=" + Uri.EscapeDataString(this.subject);
          str1 = $"sms:{this.number}{str3}";
          break;
      }
      return str1;
    }

    public enum SMSEncoding
    {
      SMS,
      SMSTO,
      SMS_iOS,
    }
  }

  public class MMS : PayloadGenerator.Payload
  {
    private readonly string number;
    private readonly string subject;
    private readonly PayloadGenerator.MMS.MMSEncoding encoding;

    public MMS(string number, PayloadGenerator.MMS.MMSEncoding encoding = PayloadGenerator.MMS.MMSEncoding.MMS)
    {
      this.number = number;
      this.subject = string.Empty;
      this.encoding = encoding;
    }

    public MMS(string number, string subject, PayloadGenerator.MMS.MMSEncoding encoding = PayloadGenerator.MMS.MMSEncoding.MMS)
    {
      this.number = number;
      this.subject = subject;
      this.encoding = encoding;
    }

    public override string ToString()
    {
      string str1 = string.Empty;
      switch (this.encoding)
      {
        case PayloadGenerator.MMS.MMSEncoding.MMS:
          string str2 = string.Empty;
          if (!string.IsNullOrEmpty(this.subject))
            str2 = "?body=" + Uri.EscapeDataString(this.subject);
          str1 = $"mms:{this.number}{str2}";
          break;
        case PayloadGenerator.MMS.MMSEncoding.MMSTO:
          string str3 = string.Empty;
          if (!string.IsNullOrEmpty(this.subject))
            str3 = "?subject=" + Uri.EscapeDataString(this.subject);
          str1 = $"mmsto:{this.number}{str3}";
          break;
      }
      return str1;
    }

    public enum MMSEncoding
    {
      MMS,
      MMSTO,
    }
  }

  public class Geolocation : PayloadGenerator.Payload
  {
    private readonly string latitude;
    private readonly string longitude;
    private readonly PayloadGenerator.Geolocation.GeolocationEncoding encoding;

    public Geolocation(
      string latitude,
      string longitude,
      PayloadGenerator.Geolocation.GeolocationEncoding encoding = PayloadGenerator.Geolocation.GeolocationEncoding.GEO)
    {
      this.latitude = latitude.Replace(",", ".");
      this.longitude = longitude.Replace(",", ".");
      this.encoding = encoding;
    }

    public override string ToString()
    {
      switch (this.encoding)
      {
        case PayloadGenerator.Geolocation.GeolocationEncoding.GEO:
          return $"geo:{this.latitude},{this.longitude}";
        case PayloadGenerator.Geolocation.GeolocationEncoding.GoogleMaps:
          return $"http://maps.google.com/maps?q={this.latitude},{this.longitude}";
        default:
          return "geo:";
      }
    }

    public enum GeolocationEncoding
    {
      GEO,
      GoogleMaps,
    }
  }

  public class PhoneNumber : PayloadGenerator.Payload
  {
    private readonly string number;

    public PhoneNumber(string number) => this.number = number;

    public override string ToString() => "tel:" + this.number;
  }

  public class SkypeCall : PayloadGenerator.Payload
  {
    private readonly string skypeUsername;

    public SkypeCall(string skypeUsername) => this.skypeUsername = skypeUsername;

    public override string ToString() => $"skype:{this.skypeUsername}?call";
  }

  public class Url : PayloadGenerator.Payload
  {
    private readonly string url;

    public Url(string url) => this.url = url;

    public override string ToString()
    {
      return this.url.StartsWith("http") ? this.url : "http://" + this.url;
    }
  }

  public class WhatsAppMessage : PayloadGenerator.Payload
  {
    private readonly string number;
    private readonly string message;

    public WhatsAppMessage(string number, string message)
    {
      this.number = number;
      this.message = message;
    }

    public WhatsAppMessage(string message)
    {
      this.number = string.Empty;
      this.message = message;
    }

    public override string ToString()
    {
      return $"https://wa.me/{Regex.Replace(this.number, "^[0+]+|[ ()-]", string.Empty)}?text={Uri.EscapeDataString(this.message)}";
    }
  }

  public class Bookmark : PayloadGenerator.Payload
  {
    private readonly string url;
    private readonly string title;

    public Bookmark(string url, string title)
    {
      this.url = PayloadGenerator.EscapeInput(url);
      this.title = PayloadGenerator.EscapeInput(title);
    }

    public override string ToString() => $"MEBKM:TITLE:{this.title};URL:{this.url};;";
  }

  public class ContactData : PayloadGenerator.Payload
  {
    private readonly string firstname;
    private readonly string lastname;
    private readonly string nickname;
    private readonly string org;
    private readonly string orgTitle;
    private readonly string phone;
    private readonly string mobilePhone;
    private readonly string workPhone;
    private readonly string email;
    private readonly DateTime? birthday;
    private readonly string website;
    private readonly string street;
    private readonly string houseNumber;
    private readonly string city;
    private readonly string zipCode;
    private readonly string stateRegion;
    private readonly string country;
    private readonly string note;
    private readonly PayloadGenerator.ContactData.ContactOutputType outputType;
    private readonly PayloadGenerator.ContactData.AddressOrder addressOrder;

    public ContactData(
      PayloadGenerator.ContactData.ContactOutputType outputType,
      string firstname,
      string lastname,
      string nickname = null,
      string phone = null,
      string mobilePhone = null,
      string workPhone = null,
      string email = null,
      DateTime? birthday = null,
      string website = null,
      string street = null,
      string houseNumber = null,
      string city = null,
      string zipCode = null,
      string country = null,
      string note = null,
      string stateRegion = null,
      PayloadGenerator.ContactData.AddressOrder addressOrder = PayloadGenerator.ContactData.AddressOrder.Default,
      string org = null,
      string orgTitle = null)
    {
      this.firstname = firstname;
      this.lastname = lastname;
      this.nickname = nickname;
      this.org = org;
      this.orgTitle = orgTitle;
      this.phone = phone;
      this.mobilePhone = mobilePhone;
      this.workPhone = workPhone;
      this.email = email;
      this.birthday = birthday;
      this.website = website;
      this.street = street;
      this.houseNumber = houseNumber;
      this.city = city;
      this.stateRegion = stateRegion;
      this.zipCode = zipCode;
      this.country = country;
      this.addressOrder = addressOrder;
      this.note = note;
      this.outputType = outputType;
    }

    public override string ToString()
    {
      string empty1 = string.Empty;
      string str1;
      if (this.outputType == PayloadGenerator.ContactData.ContactOutputType.MeCard)
      {
        string str2 = empty1 + "MECARD+\r\n";
        if (!string.IsNullOrEmpty(this.firstname) && !string.IsNullOrEmpty(this.lastname))
          str2 = $"{str2}N:{this.lastname}, {this.firstname}\r\n";
        else if (!string.IsNullOrEmpty(this.firstname) || !string.IsNullOrEmpty(this.lastname))
          str2 = $"{str2}N:{this.firstname}{this.lastname}\r\n";
        if (!string.IsNullOrEmpty(this.org))
          str2 = $"{str2}ORG:{this.org}\r\n";
        if (!string.IsNullOrEmpty(this.orgTitle))
          str2 = $"{str2}TITLE:{this.orgTitle}\r\n";
        if (!string.IsNullOrEmpty(this.phone))
          str2 = $"{str2}TEL:{this.phone}\r\n";
        if (!string.IsNullOrEmpty(this.mobilePhone))
          str2 = $"{str2}TEL:{this.mobilePhone}\r\n";
        if (!string.IsNullOrEmpty(this.workPhone))
          str2 = $"{str2}TEL:{this.workPhone}\r\n";
        if (!string.IsNullOrEmpty(this.email))
          str2 = $"{str2}EMAIL:{this.email}\r\n";
        if (!string.IsNullOrEmpty(this.note))
          str2 = $"{str2}NOTE:{this.note}\r\n";
        if (this.birthday.HasValue)
          str2 = $"{str2}BDAY:{this.birthday.Value.ToString("yyyyMMdd")}\r\n";
        string empty2 = string.Empty;
        string str3;
        if (this.addressOrder == PayloadGenerator.ContactData.AddressOrder.Default)
          str3 = $"ADR:,,{(!string.IsNullOrEmpty(this.street) ? this.street + " " : "")}{(!string.IsNullOrEmpty(this.houseNumber) ? this.houseNumber : "")},{(!string.IsNullOrEmpty(this.zipCode) ? this.zipCode : "")},{(!string.IsNullOrEmpty(this.city) ? this.city : "")},{(!string.IsNullOrEmpty(this.stateRegion) ? this.stateRegion : "")},{(!string.IsNullOrEmpty(this.country) ? this.country : "")}\r\n";
        else
          str3 = $"ADR:,,{(!string.IsNullOrEmpty(this.houseNumber) ? this.houseNumber + " " : "")}{(!string.IsNullOrEmpty(this.street) ? this.street : "")},{(!string.IsNullOrEmpty(this.city) ? this.city : "")},{(!string.IsNullOrEmpty(this.stateRegion) ? this.stateRegion : "")},{(!string.IsNullOrEmpty(this.zipCode) ? this.zipCode : "")},{(!string.IsNullOrEmpty(this.country) ? this.country : "")}\r\n";
        string str4 = str2 + str3;
        if (!string.IsNullOrEmpty(this.website))
          str4 = $"{str4}URL:{this.website}\r\n";
        if (!string.IsNullOrEmpty(this.nickname))
          str4 = $"{str4}NICKNAME:{this.nickname}\r\n";
        str1 = str4.Trim('\r', '\n');
      }
      else
      {
        string str5 = this.outputType.ToString().Substring(5);
        string str6 = str5.Length <= 1 ? str5 + ".0" : str5.Insert(1, ".");
        string str7 = $"{$"{$"{empty1 + "BEGIN:VCARD\r\n"}VERSION:{str6}\r\n"}N:{(!string.IsNullOrEmpty(this.lastname) ? this.lastname : "")};{(!string.IsNullOrEmpty(this.firstname) ? this.firstname : "")};;;\r\n"}FN:{(!string.IsNullOrEmpty(this.firstname) ? this.firstname + " " : "")}{(!string.IsNullOrEmpty(this.lastname) ? this.lastname : "")}\r\n";
        if (!string.IsNullOrEmpty(this.org))
          str7 = $"{str7}ORG:{this.org}\r\n";
        if (!string.IsNullOrEmpty(this.orgTitle))
          str7 = $"{str7}TITLE:{this.orgTitle}\r\n";
        if (!string.IsNullOrEmpty(this.phone))
        {
          string str8 = str7 + "TEL;";
          str7 = (this.outputType != PayloadGenerator.ContactData.ContactOutputType.VCard21 ? (this.outputType != PayloadGenerator.ContactData.ContactOutputType.VCard3 ? $"{str8}TYPE=home,voice;VALUE=uri:tel:{this.phone}" : $"{str8}TYPE=HOME,VOICE:{this.phone}") : $"{str8}HOME;VOICE:{this.phone}") + "\r\n";
        }
        if (!string.IsNullOrEmpty(this.mobilePhone))
        {
          string str9 = str7 + "TEL;";
          str7 = (this.outputType != PayloadGenerator.ContactData.ContactOutputType.VCard21 ? (this.outputType != PayloadGenerator.ContactData.ContactOutputType.VCard3 ? $"{str9}TYPE=home,cell;VALUE=uri:tel:{this.mobilePhone}" : $"{str9}TYPE=HOME,CELL:{this.mobilePhone}") : $"{str9}HOME;CELL:{this.mobilePhone}") + "\r\n";
        }
        if (!string.IsNullOrEmpty(this.workPhone))
        {
          string str10 = str7 + "TEL;";
          str7 = (this.outputType != PayloadGenerator.ContactData.ContactOutputType.VCard21 ? (this.outputType != PayloadGenerator.ContactData.ContactOutputType.VCard3 ? $"{str10}TYPE=work,voice;VALUE=uri:tel:{this.workPhone}" : $"{str10}TYPE=WORK,VOICE:{this.workPhone}") : $"{str10}WORK;VOICE:{this.workPhone}") + "\r\n";
        }
        string str11 = str7 + "ADR;";
        string str12 = this.outputType != PayloadGenerator.ContactData.ContactOutputType.VCard21 ? (this.outputType != PayloadGenerator.ContactData.ContactOutputType.VCard3 ? str11 + "TYPE=home,pref:" : str11 + "TYPE=HOME,PREF:") : str11 + "HOME;PREF:";
        string empty3 = string.Empty;
        string str13;
        if (this.addressOrder == PayloadGenerator.ContactData.AddressOrder.Default)
          str13 = $";;{(!string.IsNullOrEmpty(this.street) ? this.street + " " : "")}{(!string.IsNullOrEmpty(this.houseNumber) ? this.houseNumber : "")};{(!string.IsNullOrEmpty(this.zipCode) ? this.zipCode : "")};{(!string.IsNullOrEmpty(this.city) ? this.city : "")};{(!string.IsNullOrEmpty(this.stateRegion) ? this.stateRegion : "")};{(!string.IsNullOrEmpty(this.country) ? this.country : "")}\r\n";
        else
          str13 = $";;{(!string.IsNullOrEmpty(this.houseNumber) ? this.houseNumber + " " : "")}{(!string.IsNullOrEmpty(this.street) ? this.street : "")};{(!string.IsNullOrEmpty(this.city) ? this.city : "")};{(!string.IsNullOrEmpty(this.stateRegion) ? this.stateRegion : "")};{(!string.IsNullOrEmpty(this.zipCode) ? this.zipCode : "")};{(!string.IsNullOrEmpty(this.country) ? this.country : "")}\r\n";
        string str14 = str12 + str13;
        if (this.birthday.HasValue)
          str14 = $"{str14}BDAY:{this.birthday.Value.ToString("yyyyMMdd")}\r\n";
        if (!string.IsNullOrEmpty(this.website))
          str14 = $"{str14}URL:{this.website}\r\n";
        if (!string.IsNullOrEmpty(this.email))
          str14 = $"{str14}EMAIL:{this.email}\r\n";
        if (!string.IsNullOrEmpty(this.note))
          str14 = $"{str14}NOTE:{this.note}\r\n";
        if (this.outputType != PayloadGenerator.ContactData.ContactOutputType.VCard21 && !string.IsNullOrEmpty(this.nickname))
          str14 = $"{str14}NICKNAME:{this.nickname}\r\n";
        str1 = str14 + "END:VCARD";
      }
      return str1;
    }

    public enum ContactOutputType
    {
      MeCard,
      VCard21,
      VCard3,
      VCard4,
    }

    public enum AddressOrder
    {
      Default,
      Reversed,
    }
  }

  public class BitcoinLikeCryptoCurrencyAddress : PayloadGenerator.Payload
  {
    private readonly PayloadGenerator.BitcoinLikeCryptoCurrencyAddress.BitcoinLikeCryptoCurrencyType currencyType;
    private readonly string address;
    private readonly string label;
    private readonly string message;
    private readonly double? amount;

    public BitcoinLikeCryptoCurrencyAddress(
      PayloadGenerator.BitcoinLikeCryptoCurrencyAddress.BitcoinLikeCryptoCurrencyType currencyType,
      string address,
      double? amount,
      string label = null,
      string message = null)
    {
      this.currencyType = currencyType;
      this.address = address;
      if (!string.IsNullOrEmpty(label))
        this.label = Uri.EscapeDataString(label);
      if (!string.IsNullOrEmpty(message))
        this.message = Uri.EscapeDataString(message);
      this.amount = amount;
    }

    public override string ToString()
    {
      string str = (string) null;
      KeyValuePair<string, string>[] source = new KeyValuePair<string, string>[3]
      {
        new KeyValuePair<string, string>("label", this.label),
        new KeyValuePair<string, string>("message", this.message),
        new KeyValuePair<string, string>("amount", this.amount.HasValue ? this.amount.Value.ToString("#.########", (IFormatProvider) CultureInfo.InvariantCulture) : (string) null)
      };
      if (((IEnumerable<KeyValuePair<string, string>>) source).Any<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (keyPair => !string.IsNullOrEmpty(keyPair.Value))))
        str = "?" + string.Join("&", ((IEnumerable<KeyValuePair<string, string>>) source).Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (keyPair => !string.IsNullOrEmpty(keyPair.Value))).Select<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (keyPair => $"{keyPair.Key}={keyPair.Value}")).ToArray<string>());
      return $"{Enum.GetName(typeof (PayloadGenerator.BitcoinLikeCryptoCurrencyAddress.BitcoinLikeCryptoCurrencyType), (object) this.currencyType).ToLower()}:{this.address}{str}";
    }

    public enum BitcoinLikeCryptoCurrencyType
    {
      Bitcoin,
      BitcoinCash,
      Litecoin,
    }
  }

  public class BitcoinAddress(string address, double? amount, string label = null, string message = null) : 
    PayloadGenerator.BitcoinLikeCryptoCurrencyAddress(PayloadGenerator.BitcoinLikeCryptoCurrencyAddress.BitcoinLikeCryptoCurrencyType.Bitcoin, address, amount, label, message)
  {
  }

  public class BitcoinCashAddress(string address, double? amount, string label = null, string message = null) : 
    PayloadGenerator.BitcoinLikeCryptoCurrencyAddress(PayloadGenerator.BitcoinLikeCryptoCurrencyAddress.BitcoinLikeCryptoCurrencyType.BitcoinCash, address, amount, label, message)
  {
  }

  public class LitecoinAddress(string address, double? amount, string label = null, string message = null) : 
    PayloadGenerator.BitcoinLikeCryptoCurrencyAddress(PayloadGenerator.BitcoinLikeCryptoCurrencyAddress.BitcoinLikeCryptoCurrencyType.Litecoin, address, amount, label, message)
  {
  }

  public class SwissQrCode : PayloadGenerator.Payload
  {
    private readonly string br = "\r\n";
    private readonly string alternativeProcedure1;
    private readonly string alternativeProcedure2;
    private readonly PayloadGenerator.SwissQrCode.Iban iban;
    private readonly Decimal? amount;
    private readonly PayloadGenerator.SwissQrCode.Contact creditor;
    private readonly PayloadGenerator.SwissQrCode.Contact ultimateCreditor;
    private readonly PayloadGenerator.SwissQrCode.Contact debitor;
    private readonly PayloadGenerator.SwissQrCode.Currency currency;
    private readonly DateTime? requestedDateOfPayment;
    private readonly PayloadGenerator.SwissQrCode.Reference reference;
    private readonly PayloadGenerator.SwissQrCode.AdditionalInformation additionalInformation;

    public SwissQrCode(
      PayloadGenerator.SwissQrCode.Iban iban,
      PayloadGenerator.SwissQrCode.Currency currency,
      PayloadGenerator.SwissQrCode.Contact creditor,
      PayloadGenerator.SwissQrCode.Reference reference,
      PayloadGenerator.SwissQrCode.AdditionalInformation additionalInformation = null,
      PayloadGenerator.SwissQrCode.Contact debitor = null,
      Decimal? amount = null,
      DateTime? requestedDateOfPayment = null,
      PayloadGenerator.SwissQrCode.Contact ultimateCreditor = null,
      string alternativeProcedure1 = null,
      string alternativeProcedure2 = null)
    {
      this.iban = iban;
      this.creditor = creditor;
      this.ultimateCreditor = ultimateCreditor;
      this.additionalInformation = additionalInformation != null ? additionalInformation : new PayloadGenerator.SwissQrCode.AdditionalInformation();
      if (amount.HasValue && amount.ToString().Length > 12)
        throw new PayloadGenerator.SwissQrCode.SwissQrCodeException("Amount (including decimals) must be shorter than 13 places.");
      this.amount = amount;
      this.currency = currency;
      this.requestedDateOfPayment = requestedDateOfPayment;
      this.debitor = debitor;
      if (iban.IsQrIban && reference.RefType != PayloadGenerator.SwissQrCode.Reference.ReferenceType.QRR)
        throw new PayloadGenerator.SwissQrCode.SwissQrCodeException("If QR-IBAN is used, you have to choose \"QRR\" as reference type!");
      if (!iban.IsQrIban && reference.RefType == PayloadGenerator.SwissQrCode.Reference.ReferenceType.QRR)
        throw new PayloadGenerator.SwissQrCode.SwissQrCodeException("If non QR-IBAN is used, you have to choose either \"SCOR\" or \"NON\" as reference type!");
      this.reference = reference;
      this.alternativeProcedure1 = alternativeProcedure1 == null || alternativeProcedure1.Length <= 100 ? alternativeProcedure1 : throw new PayloadGenerator.SwissQrCode.SwissQrCodeException("Alternative procedure information block 1 must be shorter than 101 chars.");
      this.alternativeProcedure2 = alternativeProcedure2 == null || alternativeProcedure2.Length <= 100 ? alternativeProcedure2 : throw new PayloadGenerator.SwissQrCode.SwissQrCodeException("Alternative procedure information block 2 must be shorter than 101 chars.");
    }

    public override string ToString()
    {
      string str1 = $"{$"{"SPC" + this.br}0200{this.br}"}1{this.br}" + this.iban.ToString() + this.br + this.creditor.ToString() + string.Concat(Enumerable.Repeat<string>(this.br, 7).ToArray<string>()) + (this.amount.HasValue ? $"{this.amount:0.00}".Replace(",", ".") : string.Empty) + this.br + this.currency.ToString() + this.br;
      string str2 = (this.debitor == null ? str1 + string.Concat(Enumerable.Repeat<string>(this.br, 7).ToArray<string>()) : str1 + this.debitor.ToString()) + this.reference.RefType.ToString() + this.br + (!string.IsNullOrEmpty(this.reference.ReferenceText) ? this.reference.ReferenceText : string.Empty) + this.br + (!string.IsNullOrEmpty(this.additionalInformation.UnstructureMessage) ? this.additionalInformation.UnstructureMessage : string.Empty) + this.br + this.additionalInformation.Trailer + this.br + (!string.IsNullOrEmpty(this.additionalInformation.BillInformation) ? this.additionalInformation.BillInformation : string.Empty) + this.br;
      if (!string.IsNullOrEmpty(this.alternativeProcedure1))
        str2 = str2 + this.alternativeProcedure1.Replace("\n", "") + this.br;
      if (!string.IsNullOrEmpty(this.alternativeProcedure2))
        str2 = str2 + this.alternativeProcedure2.Replace("\n", "") + this.br;
      if (str2.EndsWith(this.br))
        str2 = str2.Remove(str2.Length - this.br.Length);
      return str2;
    }

    public class AdditionalInformation
    {
      private readonly string unstructuredMessage;
      private readonly string billInformation;
      private readonly string trailer;

      public AdditionalInformation(string unstructuredMessage = null, string billInformation = null)
      {
        if ((unstructuredMessage != null ? unstructuredMessage.Length : 0) + (billInformation != null ? billInformation.Length : 0) > 140)
          throw new PayloadGenerator.SwissQrCode.AdditionalInformation.SwissQrCodeAdditionalInformationException("Unstructured message and bill information must be shorter than 141 chars in total/combined.");
        this.unstructuredMessage = unstructuredMessage;
        this.billInformation = billInformation;
        this.trailer = "EPD";
      }

      public string UnstructureMessage
      {
        get
        {
          return string.IsNullOrEmpty(this.unstructuredMessage) ? (string) null : this.unstructuredMessage.Replace("\n", "");
        }
      }

      public string BillInformation
      {
        get
        {
          return string.IsNullOrEmpty(this.billInformation) ? (string) null : this.billInformation.Replace("\n", "");
        }
      }

      public string Trailer => this.trailer;

      public class SwissQrCodeAdditionalInformationException : Exception
      {
        public SwissQrCodeAdditionalInformationException()
        {
        }

        public SwissQrCodeAdditionalInformationException(string message)
          : base(message)
        {
        }

        public SwissQrCodeAdditionalInformationException(string message, Exception inner)
          : base(message, inner)
        {
        }
      }
    }

    public class Reference
    {
      private readonly PayloadGenerator.SwissQrCode.Reference.ReferenceType referenceType;
      private readonly string reference;
      private readonly PayloadGenerator.SwissQrCode.Reference.ReferenceTextType? referenceTextType;

      public Reference(
        PayloadGenerator.SwissQrCode.Reference.ReferenceType referenceType,
        string reference = null,
        PayloadGenerator.SwissQrCode.Reference.ReferenceTextType? referenceTextType = null)
      {
        this.referenceType = referenceType;
        this.referenceTextType = referenceTextType;
        if (referenceType == PayloadGenerator.SwissQrCode.Reference.ReferenceType.NON && reference != null)
          throw new PayloadGenerator.SwissQrCode.Reference.SwissQrCodeReferenceException("Reference is only allowed when referenceType not equals \"NON\"");
        if (referenceType != PayloadGenerator.SwissQrCode.Reference.ReferenceType.NON && reference != null && !referenceTextType.HasValue)
          throw new PayloadGenerator.SwissQrCode.Reference.SwissQrCodeReferenceException("You have to set an ReferenceTextType when using the reference text.");
        PayloadGenerator.SwissQrCode.Reference.ReferenceTextType? nullable = referenceTextType;
        PayloadGenerator.SwissQrCode.Reference.ReferenceTextType referenceTextType1 = PayloadGenerator.SwissQrCode.Reference.ReferenceTextType.QrReference;
        if (nullable.GetValueOrDefault() == referenceTextType1 & nullable.HasValue && reference != null && reference.Length > 27)
          throw new PayloadGenerator.SwissQrCode.Reference.SwissQrCodeReferenceException("QR-references have to be shorter than 28 chars.");
        nullable = referenceTextType;
        PayloadGenerator.SwissQrCode.Reference.ReferenceTextType referenceTextType2 = PayloadGenerator.SwissQrCode.Reference.ReferenceTextType.QrReference;
        if (nullable.GetValueOrDefault() == referenceTextType2 & nullable.HasValue && reference != null && !Regex.IsMatch(reference, "^[0-9]+$"))
          throw new PayloadGenerator.SwissQrCode.Reference.SwissQrCodeReferenceException("QR-reference must exist out of digits only.");
        nullable = referenceTextType;
        PayloadGenerator.SwissQrCode.Reference.ReferenceTextType referenceTextType3 = PayloadGenerator.SwissQrCode.Reference.ReferenceTextType.QrReference;
        if (nullable.GetValueOrDefault() == referenceTextType3 & nullable.HasValue && reference != null && !PayloadGenerator.ChecksumMod10(reference))
          throw new PayloadGenerator.SwissQrCode.Reference.SwissQrCodeReferenceException("QR-references is invalid. Checksum error.");
        nullable = referenceTextType;
        PayloadGenerator.SwissQrCode.Reference.ReferenceTextType referenceTextType4 = PayloadGenerator.SwissQrCode.Reference.ReferenceTextType.CreditorReferenceIso11649;
        if (nullable.GetValueOrDefault() == referenceTextType4 & nullable.HasValue && reference != null && reference.Length > 25)
          throw new PayloadGenerator.SwissQrCode.Reference.SwissQrCodeReferenceException("Creditor references (ISO 11649) have to be shorter than 26 chars.");
        this.reference = reference;
      }

      public PayloadGenerator.SwissQrCode.Reference.ReferenceType RefType => this.referenceType;

      public string ReferenceText
      {
        get
        {
          return string.IsNullOrEmpty(this.reference) ? (string) null : this.reference.Replace("\n", "");
        }
      }

      public enum ReferenceType
      {
        QRR,
        SCOR,
        NON,
      }

      public enum ReferenceTextType
      {
        QrReference,
        CreditorReferenceIso11649,
      }

      public class SwissQrCodeReferenceException : Exception
      {
        public SwissQrCodeReferenceException()
        {
        }

        public SwissQrCodeReferenceException(string message)
          : base(message)
        {
        }

        public SwissQrCodeReferenceException(string message, Exception inner)
          : base(message, inner)
        {
        }
      }
    }

    public class Iban
    {
      private string iban;
      private PayloadGenerator.SwissQrCode.Iban.IbanType ibanType;

      public Iban(
        string iban,
        PayloadGenerator.SwissQrCode.Iban.IbanType ibanType)
      {
        if (ibanType == PayloadGenerator.SwissQrCode.Iban.IbanType.Iban && !PayloadGenerator.IsValidIban(iban))
          throw new PayloadGenerator.SwissQrCode.Iban.SwissQrCodeIbanException("The IBAN entered isn't valid.");
        if (ibanType == PayloadGenerator.SwissQrCode.Iban.IbanType.QrIban && !PayloadGenerator.IsValidQRIban(iban))
          throw new PayloadGenerator.SwissQrCode.Iban.SwissQrCodeIbanException("The QR-IBAN entered isn't valid.");
        this.iban = iban.StartsWith("CH") || iban.StartsWith("LI") ? iban : throw new PayloadGenerator.SwissQrCode.Iban.SwissQrCodeIbanException("The IBAN must start with \"CH\" or \"LI\".");
        this.ibanType = ibanType;
      }

      public bool IsQrIban => this.ibanType == PayloadGenerator.SwissQrCode.Iban.IbanType.QrIban;

      public override string ToString()
      {
        return this.iban.Replace("-", "").Replace("\n", "").Replace(" ", "");
      }

      public enum IbanType
      {
        Iban,
        QrIban,
      }

      public class SwissQrCodeIbanException : Exception
      {
        public SwissQrCodeIbanException()
        {
        }

        public SwissQrCodeIbanException(string message)
          : base(message)
        {
        }

        public SwissQrCodeIbanException(string message, Exception inner)
          : base(message, inner)
        {
        }
      }
    }

    public class Contact
    {
      private static readonly HashSet<string> twoLetterCodes = PayloadGenerator.SwissQrCode.Contact.ValidTwoLetterCodes();
      private string br = "\r\n";
      private string name;
      private string streetOrAddressline1;
      private string houseNumberOrAddressline2;
      private string zipCode;
      private string city;
      private string country;
      private PayloadGenerator.SwissQrCode.Contact.AddressType adrType;

      [Obsolete("This constructor is deprecated. Use WithStructuredAddress instead.")]
      public Contact(
        string name,
        string zipCode,
        string city,
        string country,
        string street = null,
        string houseNumber = null)
        : this(name, zipCode, city, country, street, houseNumber, PayloadGenerator.SwissQrCode.Contact.AddressType.StructuredAddress)
      {
      }

      [Obsolete("This constructor is deprecated. Use WithCombinedAddress instead.")]
      public Contact(string name, string country, string addressLine1, string addressLine2)
        : this(name, (string) null, (string) null, country, addressLine1, addressLine2, PayloadGenerator.SwissQrCode.Contact.AddressType.CombinedAddress)
      {
      }

      public static PayloadGenerator.SwissQrCode.Contact WithStructuredAddress(
        string name,
        string zipCode,
        string city,
        string country,
        string street = null,
        string houseNumber = null)
      {
        return new PayloadGenerator.SwissQrCode.Contact(name, zipCode, city, country, street, houseNumber, PayloadGenerator.SwissQrCode.Contact.AddressType.StructuredAddress);
      }

      public static PayloadGenerator.SwissQrCode.Contact WithCombinedAddress(
        string name,
        string country,
        string addressLine1,
        string addressLine2)
      {
        return new PayloadGenerator.SwissQrCode.Contact(name, (string) null, (string) null, country, addressLine1, addressLine2, PayloadGenerator.SwissQrCode.Contact.AddressType.CombinedAddress);
      }

      private Contact(
        string name,
        string zipCode,
        string city,
        string country,
        string streetOrAddressline1,
        string houseNumberOrAddressline2,
        PayloadGenerator.SwissQrCode.Contact.AddressType addressType)
      {
        string pattern = "^([a-zA-Z0-9\\.,;:'\\ \\+\\-/\\(\\)?\\*\\[\\]\\{\\}\\\\`´~ ]|[!\"#%&<>÷=@_$£]|[àáâäçèéêëìíîïñòóôöùúûüýßÀÁÂÄÇÈÉÊËÌÍÎÏÒÓÔÖÙÚÛÜÑ])*$";
        this.adrType = addressType;
        if (string.IsNullOrEmpty(name))
          throw new PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException("Name must not be empty.");
        if (name.Length > 70)
          throw new PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException("Name must be shorter than 71 chars.");
        this.name = Regex.IsMatch(name, pattern) ? name : throw new PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException("Name must match the following pattern as defined in pain.001: " + pattern);
        if (this.adrType == PayloadGenerator.SwissQrCode.Contact.AddressType.StructuredAddress)
        {
          if (!string.IsNullOrEmpty(streetOrAddressline1) && streetOrAddressline1.Length > 70)
            throw new PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException("Street must be shorter than 71 chars.");
          this.streetOrAddressline1 = string.IsNullOrEmpty(streetOrAddressline1) || Regex.IsMatch(streetOrAddressline1, pattern) ? streetOrAddressline1 : throw new PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException("Street must match the following pattern as defined in pain.001: " + pattern);
          this.houseNumberOrAddressline2 = string.IsNullOrEmpty(houseNumberOrAddressline2) || houseNumberOrAddressline2.Length <= 16 /*0x10*/ ? houseNumberOrAddressline2 : throw new PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException("House number must be shorter than 17 chars.");
        }
        else
        {
          if (!string.IsNullOrEmpty(streetOrAddressline1) && streetOrAddressline1.Length > 70)
            throw new PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException("Address line 1 must be shorter than 71 chars.");
          this.streetOrAddressline1 = string.IsNullOrEmpty(streetOrAddressline1) || Regex.IsMatch(streetOrAddressline1, pattern) ? streetOrAddressline1 : throw new PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException("Address line 1 must match the following pattern as defined in pain.001: " + pattern);
          if (string.IsNullOrEmpty(houseNumberOrAddressline2))
            throw new PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException("Address line 2 must be provided for combined addresses (address line-based addresses).");
          if (!string.IsNullOrEmpty(houseNumberOrAddressline2) && houseNumberOrAddressline2.Length > 70)
            throw new PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException("Address line 2 must be shorter than 71 chars.");
          this.houseNumberOrAddressline2 = string.IsNullOrEmpty(houseNumberOrAddressline2) || Regex.IsMatch(houseNumberOrAddressline2, pattern) ? houseNumberOrAddressline2 : throw new PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException("Address line 2 must match the following pattern as defined in pain.001: " + pattern);
        }
        if (this.adrType == PayloadGenerator.SwissQrCode.Contact.AddressType.StructuredAddress)
        {
          if (string.IsNullOrEmpty(zipCode))
            throw new PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException("Zip code must not be empty.");
          if (zipCode.Length > 16 /*0x10*/)
            throw new PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException("Zip code must be shorter than 17 chars.");
          this.zipCode = Regex.IsMatch(zipCode, pattern) ? zipCode : throw new PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException("Zip code must match the following pattern as defined in pain.001: " + pattern);
          if (string.IsNullOrEmpty(city))
            throw new PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException("City must not be empty.");
          if (city.Length > 35)
            throw new PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException("City name must be shorter than 36 chars.");
          this.city = Regex.IsMatch(city, pattern) ? city : throw new PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException("City name must match the following pattern as defined in pain.001: " + pattern);
        }
        else
          this.zipCode = this.city = string.Empty;
        this.country = PayloadGenerator.SwissQrCode.Contact.IsValidTwoLetterCode(country) ? country : throw new PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException("Country must be a valid \"two letter\" country code as defined by  ISO 3166-1, but it isn't.");
      }

      private static bool IsValidTwoLetterCode(string code)
      {
        return PayloadGenerator.SwissQrCode.Contact.twoLetterCodes.Contains(code);
      }

      private static HashSet<string> ValidTwoLetterCodes()
      {
        return new HashSet<string>((IEnumerable<string>) new string[249]
        {
          "AF",
          "AL",
          "DZ",
          "AS",
          "AD",
          "AO",
          "AI",
          "AQ",
          "AG",
          "AR",
          "AM",
          "AW",
          "AU",
          "AT",
          "AZ",
          "BS",
          "BH",
          "BD",
          "BB",
          "BY",
          "BE",
          "BZ",
          "BJ",
          "BM",
          "BT",
          "BO",
          "BQ",
          "BA",
          "BW",
          "BV",
          "BR",
          "IO",
          "BN",
          "BG",
          "BF",
          "BI",
          "CV",
          "KH",
          "CM",
          "CA",
          "KY",
          "CF",
          "TD",
          "CL",
          "CN",
          "CX",
          "CC",
          "CO",
          "KM",
          "CG",
          "CD",
          "CK",
          "CR",
          "CI",
          "HR",
          "CU",
          "CW",
          "CY",
          "CZ",
          "DK",
          "DJ",
          "DM",
          "DO",
          "EC",
          "EG",
          "SV",
          "GQ",
          "ER",
          "EE",
          "SZ",
          "ET",
          "FK",
          "FO",
          "FJ",
          "FI",
          "FR",
          "GF",
          "PF",
          "TF",
          "GA",
          "GM",
          "GE",
          "DE",
          "GH",
          "GI",
          "GR",
          "GL",
          "GD",
          "GP",
          "GU",
          "GT",
          "GG",
          "GN",
          "GW",
          "GY",
          "HT",
          "HM",
          "VA",
          "HN",
          "HK",
          "HU",
          "IS",
          "IN",
          "ID",
          "IR",
          "IQ",
          "IE",
          "IM",
          "IL",
          "IT",
          "JM",
          "JP",
          "JE",
          "JO",
          "KZ",
          "KE",
          "KI",
          "KP",
          "KR",
          "KW",
          "KG",
          "LA",
          "LV",
          "LB",
          "LS",
          "LR",
          "LY",
          "LI",
          "LT",
          "LU",
          "MO",
          "MG",
          "MW",
          "MY",
          "MV",
          "ML",
          "MT",
          "MH",
          "MQ",
          "MR",
          "MU",
          "YT",
          "MX",
          "FM",
          "MD",
          "MC",
          "MN",
          "ME",
          "MS",
          "MA",
          "MZ",
          "MM",
          "NA",
          "NR",
          "NP",
          "NL",
          "NC",
          "NZ",
          "NI",
          "NE",
          "NG",
          "NU",
          "NF",
          "MP",
          "MK",
          "NO",
          "OM",
          "PK",
          "PW",
          "PS",
          "PA",
          "PG",
          "PY",
          "PE",
          "PH",
          "PN",
          "PL",
          "PT",
          "PR",
          "QA",
          "RE",
          "RO",
          "RU",
          "RW",
          "BL",
          "SH",
          "KN",
          "LC",
          "MF",
          "PM",
          "VC",
          "WS",
          "SM",
          "ST",
          "SA",
          "SN",
          "RS",
          "SC",
          "SL",
          "SG",
          "SX",
          "SK",
          "SI",
          "SB",
          "SO",
          "ZA",
          "GS",
          "SS",
          "ES",
          "LK",
          "SD",
          "SR",
          "SJ",
          "SE",
          "CH",
          "SY",
          "TW",
          "TJ",
          "TZ",
          "TH",
          "TL",
          "TG",
          "TK",
          "TO",
          "TT",
          "TN",
          "TR",
          "TM",
          "TC",
          "TV",
          "UG",
          "UA",
          "AE",
          "GB",
          "US",
          "UM",
          "UY",
          "UZ",
          "VU",
          "VE",
          "VN",
          "VG",
          "VI",
          "WF",
          "EH",
          "YE",
          "ZM",
          "ZW",
          "AX"
        }, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      }

      public override string ToString()
      {
        return (this.adrType == PayloadGenerator.SwissQrCode.Contact.AddressType.StructuredAddress ? "S" : "K") + this.br + this.name.Replace("\n", "") + this.br + (!string.IsNullOrEmpty(this.streetOrAddressline1) ? this.streetOrAddressline1.Replace("\n", "") : string.Empty) + this.br + (!string.IsNullOrEmpty(this.houseNumberOrAddressline2) ? this.houseNumberOrAddressline2.Replace("\n", "") : string.Empty) + this.br + this.zipCode.Replace("\n", "") + this.br + this.city.Replace("\n", "") + this.br + this.country + this.br;
      }

      public enum AddressType
      {
        StructuredAddress,
        CombinedAddress,
      }

      public class SwissQrCodeContactException : Exception
      {
        public SwissQrCodeContactException()
        {
        }

        public SwissQrCodeContactException(string message)
          : base(message)
        {
        }

        public SwissQrCodeContactException(string message, Exception inner)
          : base(message, inner)
        {
        }
      }
    }

    public enum Currency
    {
      CHF = 756, // 0x000002F4
      EUR = 978, // 0x000003D2
    }

    public class SwissQrCodeException : Exception
    {
      public SwissQrCodeException()
      {
      }

      public SwissQrCodeException(string message)
        : base(message)
      {
      }

      public SwissQrCodeException(string message, Exception inner)
        : base(message, inner)
      {
      }
    }
  }

  public class Girocode : PayloadGenerator.Payload
  {
    private string br = "\n";
    private readonly string iban;
    private readonly string bic;
    private readonly string name;
    private readonly string purposeOfCreditTransfer;
    private readonly string remittanceInformation;
    private readonly string messageToGirocodeUser;
    private readonly Decimal amount;
    private readonly PayloadGenerator.Girocode.GirocodeVersion version;
    private readonly PayloadGenerator.Girocode.GirocodeEncoding encoding;
    private readonly PayloadGenerator.Girocode.TypeOfRemittance typeOfRemittance;

    public Girocode(
      string iban,
      string bic,
      string name,
      Decimal amount,
      string remittanceInformation = "",
      PayloadGenerator.Girocode.TypeOfRemittance typeOfRemittance = PayloadGenerator.Girocode.TypeOfRemittance.Unstructured,
      string purposeOfCreditTransfer = "",
      string messageToGirocodeUser = "",
      PayloadGenerator.Girocode.GirocodeVersion version = PayloadGenerator.Girocode.GirocodeVersion.Version1,
      PayloadGenerator.Girocode.GirocodeEncoding encoding = PayloadGenerator.Girocode.GirocodeEncoding.ISO_8859_1)
    {
      this.version = version;
      this.encoding = encoding;
      this.iban = PayloadGenerator.IsValidIban(iban) ? iban.Replace(" ", "").ToUpper() : throw new PayloadGenerator.Girocode.GirocodeException("The IBAN entered isn't valid.");
      this.bic = PayloadGenerator.IsValidBic(bic) ? bic.Replace(" ", "").ToUpper() : throw new PayloadGenerator.Girocode.GirocodeException("The BIC entered isn't valid.");
      this.name = name.Length <= 70 ? name : throw new PayloadGenerator.Girocode.GirocodeException("(Payee-)Name must be shorter than 71 chars.");
      if (amount.ToString().Replace(",", ".").Contains("."))
      {
        if (amount.ToString().Replace(",", ".").Split('.')[1].TrimEnd('0').Length > 2)
          throw new PayloadGenerator.Girocode.GirocodeException("Amount must have less than 3 digits after decimal point.");
      }
      this.amount = !(amount < 0.01M) && !(amount > 999999999.99M) ? amount : throw new PayloadGenerator.Girocode.GirocodeException("Amount has to at least 0.01 and must be smaller or equal to 999999999.99.");
      this.purposeOfCreditTransfer = purposeOfCreditTransfer.Length <= 4 ? purposeOfCreditTransfer : throw new PayloadGenerator.Girocode.GirocodeException("Purpose of credit transfer can only have 4 chars at maximum.");
      if (typeOfRemittance == PayloadGenerator.Girocode.TypeOfRemittance.Unstructured && remittanceInformation.Length > 140)
        throw new PayloadGenerator.Girocode.GirocodeException("Unstructured reference texts have to shorter than 141 chars.");
      if (typeOfRemittance == PayloadGenerator.Girocode.TypeOfRemittance.Structured && remittanceInformation.Length > 35)
        throw new PayloadGenerator.Girocode.GirocodeException("Structured reference texts have to shorter than 36 chars.");
      this.typeOfRemittance = typeOfRemittance;
      this.remittanceInformation = remittanceInformation;
      this.messageToGirocodeUser = messageToGirocodeUser.Length <= 70 ? messageToGirocodeUser : throw new PayloadGenerator.Girocode.GirocodeException("Message to the Girocode-User reader texts have to shorter than 71 chars.");
    }

    public override string ToString()
    {
      return PayloadGenerator.ConvertStringToEncoding($"{"BCD" + this.br + (this.version == PayloadGenerator.Girocode.GirocodeVersion.Version1 ? "001" : "002") + this.br + ((int) (this.encoding + 1)).ToString() + this.br}SCT{this.br}" + this.bic + this.br + this.name + this.br + this.iban + this.br + $"EUR{this.amount:0.00}".Replace(",", ".") + this.br + this.purposeOfCreditTransfer + this.br + (this.typeOfRemittance == PayloadGenerator.Girocode.TypeOfRemittance.Structured ? this.remittanceInformation : string.Empty) + this.br + (this.typeOfRemittance == PayloadGenerator.Girocode.TypeOfRemittance.Unstructured ? this.remittanceInformation : string.Empty) + this.br + this.messageToGirocodeUser, this.encoding.ToString().Replace("_", "-"));
    }

    public enum GirocodeVersion
    {
      Version1,
      Version2,
    }

    public enum TypeOfRemittance
    {
      Structured,
      Unstructured,
    }

    public enum GirocodeEncoding
    {
      UTF_8,
      ISO_8859_1,
      ISO_8859_2,
      ISO_8859_4,
      ISO_8859_5,
      ISO_8859_7,
      ISO_8859_10,
      ISO_8859_15,
    }

    public class GirocodeException : Exception
    {
      public GirocodeException()
      {
      }

      public GirocodeException(string message)
        : base(message)
      {
      }

      public GirocodeException(string message, Exception inner)
        : base(message, inner)
      {
      }
    }
  }

  public class BezahlCode : PayloadGenerator.Payload
  {
    private readonly string name;
    private readonly string iban;
    private readonly string bic;
    private readonly string account;
    private readonly string bnc;
    private readonly string sepaReference;
    private readonly string reason;
    private readonly string creditorId;
    private readonly string mandateId;
    private readonly string periodicTimeunit;
    private readonly Decimal amount;
    private readonly int postingKey;
    private readonly int periodicTimeunitRotation;
    private readonly PayloadGenerator.BezahlCode.Currency currency;
    private readonly PayloadGenerator.BezahlCode.AuthorityType authority;
    private readonly DateTime executionDate;
    private readonly DateTime dateOfSignature;
    private readonly DateTime periodicFirstExecutionDate;
    private readonly DateTime periodicLastExecutionDate;

    public BezahlCode(
      PayloadGenerator.BezahlCode.AuthorityType authority,
      string name,
      string account = "",
      string bnc = "",
      string iban = "",
      string bic = "",
      string reason = "")
      : this(authority, name, account, bnc, iban, bic, 0M, string.Empty, creditorId: string.Empty, mandateId: string.Empty, reason: reason, sepaReference: string.Empty, internalMode: 1)
    {
    }

    public BezahlCode(
      PayloadGenerator.BezahlCode.AuthorityType authority,
      string name,
      string account,
      string bnc,
      Decimal amount,
      string periodicTimeunit = "",
      int periodicTimeunitRotation = 0,
      DateTime? periodicFirstExecutionDate = null,
      DateTime? periodicLastExecutionDate = null,
      string reason = "",
      int postingKey = 0,
      PayloadGenerator.BezahlCode.Currency currency = PayloadGenerator.BezahlCode.Currency.EUR,
      DateTime? executionDate = null)
      : this(authority, name, account, bnc, string.Empty, string.Empty, amount, periodicTimeunit, periodicTimeunitRotation, periodicFirstExecutionDate, periodicLastExecutionDate, string.Empty, string.Empty, reason: reason, postingKey: postingKey, sepaReference: string.Empty, currency: currency, executionDate: executionDate, internalMode: 2)
    {
    }

    public BezahlCode(
      PayloadGenerator.BezahlCode.AuthorityType authority,
      string name,
      string iban,
      string bic,
      Decimal amount,
      string periodicTimeunit = "",
      int periodicTimeunitRotation = 0,
      DateTime? periodicFirstExecutionDate = null,
      DateTime? periodicLastExecutionDate = null,
      string creditorId = "",
      string mandateId = "",
      DateTime? dateOfSignature = null,
      string reason = "",
      string sepaReference = "",
      PayloadGenerator.BezahlCode.Currency currency = PayloadGenerator.BezahlCode.Currency.EUR,
      DateTime? executionDate = null)
      : this(authority, name, string.Empty, string.Empty, iban, bic, amount, periodicTimeunit, periodicTimeunitRotation, periodicFirstExecutionDate, periodicLastExecutionDate, creditorId, mandateId, dateOfSignature, reason, sepaReference: sepaReference, currency: currency, executionDate: executionDate, internalMode: 3)
    {
    }

    public BezahlCode(
      PayloadGenerator.BezahlCode.AuthorityType authority,
      string name,
      string account,
      string bnc,
      string iban,
      string bic,
      Decimal amount,
      string periodicTimeunit = "",
      int periodicTimeunitRotation = 0,
      DateTime? periodicFirstExecutionDate = null,
      DateTime? periodicLastExecutionDate = null,
      string creditorId = "",
      string mandateId = "",
      DateTime? dateOfSignature = null,
      string reason = "",
      int postingKey = 0,
      string sepaReference = "",
      PayloadGenerator.BezahlCode.Currency currency = PayloadGenerator.BezahlCode.Currency.EUR,
      DateTime? executionDate = null,
      int internalMode = 0)
    {
      switch (internalMode)
      {
        case 1:
          if (authority != PayloadGenerator.BezahlCode.AuthorityType.contact && authority != PayloadGenerator.BezahlCode.AuthorityType.contact_v2)
            throw new PayloadGenerator.BezahlCode.BezahlCodeException("The constructor without an amount may only ne used with authority types 'contact' and 'contact_v2'.");
          if (authority == PayloadGenerator.BezahlCode.AuthorityType.contact && (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(bnc)))
            throw new PayloadGenerator.BezahlCode.BezahlCodeException("When using authority type 'contact' the parameters 'account' and 'bnc' must be set.");
          if (authority != PayloadGenerator.BezahlCode.AuthorityType.contact_v2)
          {
            bool flag1 = !string.IsNullOrEmpty(account) && !string.IsNullOrEmpty(bnc);
            bool flag2 = !string.IsNullOrEmpty(iban) && !string.IsNullOrEmpty(bic);
            if (!flag1 && !flag2 || flag1 & flag2)
              throw new PayloadGenerator.BezahlCode.BezahlCodeException("When using authority type 'contact_v2' either the parameters 'account' and 'bnc' or the parameters 'iban' and 'bic' must be set. Leave the other parameter pair empty.");
            break;
          }
          break;
        case 2:
          if (authority != PayloadGenerator.BezahlCode.AuthorityType.periodicsinglepayment && authority != PayloadGenerator.BezahlCode.AuthorityType.singledirectdebit && authority != PayloadGenerator.BezahlCode.AuthorityType.singlepayment)
            throw new PayloadGenerator.BezahlCode.BezahlCodeException("The constructor with 'account' and 'bnc' may only be used with 'non SEPA' authority types. Either choose another authority type or switch constructor.");
          if (authority == PayloadGenerator.BezahlCode.AuthorityType.periodicsinglepayment && (string.IsNullOrEmpty(periodicTimeunit) || periodicTimeunitRotation == 0))
            throw new PayloadGenerator.BezahlCode.BezahlCodeException("When using 'periodicsinglepayment' as authority type, the parameters 'periodicTimeunit' and 'periodicTimeunitRotation' must be set.");
          break;
        case 3:
          if (authority != PayloadGenerator.BezahlCode.AuthorityType.periodicsinglepaymentsepa && authority != PayloadGenerator.BezahlCode.AuthorityType.singledirectdebitsepa && authority != PayloadGenerator.BezahlCode.AuthorityType.singlepaymentsepa)
            throw new PayloadGenerator.BezahlCode.BezahlCodeException("The constructor with 'iban' and 'bic' may only be used with 'SEPA' authority types. Either choose another authority type or switch constructor.");
          if (authority == PayloadGenerator.BezahlCode.AuthorityType.periodicsinglepaymentsepa && (string.IsNullOrEmpty(periodicTimeunit) || periodicTimeunitRotation == 0))
            throw new PayloadGenerator.BezahlCode.BezahlCodeException("When using 'periodicsinglepaymentsepa' as authority type, the parameters 'periodicTimeunit' and 'periodicTimeunitRotation' must be set.");
          break;
      }
      this.authority = authority;
      this.name = name.Length <= 70 ? name : throw new PayloadGenerator.BezahlCode.BezahlCodeException("(Payee-)Name must be shorter than 71 chars.");
      this.reason = reason.Length <= 27 ? reason : throw new PayloadGenerator.BezahlCode.BezahlCodeException("Reasons texts have to be shorter than 28 chars.");
      bool flag3 = !string.IsNullOrEmpty(account) && !string.IsNullOrEmpty(bnc);
      bool flag4 = !string.IsNullOrEmpty(iban) && !string.IsNullOrEmpty(bic);
      if (authority == PayloadGenerator.BezahlCode.AuthorityType.periodicsinglepayment || authority == PayloadGenerator.BezahlCode.AuthorityType.singledirectdebit || authority == PayloadGenerator.BezahlCode.AuthorityType.singlepayment || authority == PayloadGenerator.BezahlCode.AuthorityType.contact || authority == PayloadGenerator.BezahlCode.AuthorityType.contact_v2 & flag3)
      {
        this.account = Regex.IsMatch(account.Replace(" ", ""), "^[0-9]{1,9}$") ? account.Replace(" ", "").ToUpper() : throw new PayloadGenerator.BezahlCode.BezahlCodeException("The account entered isn't valid.");
        this.bnc = Regex.IsMatch(bnc.Replace(" ", ""), "^[0-9]{1,9}$") ? bnc.Replace(" ", "").ToUpper() : throw new PayloadGenerator.BezahlCode.BezahlCodeException("The bnc entered isn't valid.");
        if (authority != PayloadGenerator.BezahlCode.AuthorityType.contact && authority != PayloadGenerator.BezahlCode.AuthorityType.contact_v2)
          this.postingKey = postingKey >= 0 && postingKey < 100 ? postingKey : throw new PayloadGenerator.BezahlCode.BezahlCodeException("PostingKey must be within 0 and 99.");
      }
      if (authority == PayloadGenerator.BezahlCode.AuthorityType.periodicsinglepaymentsepa || authority == PayloadGenerator.BezahlCode.AuthorityType.singledirectdebitsepa || authority == PayloadGenerator.BezahlCode.AuthorityType.singlepaymentsepa || authority == PayloadGenerator.BezahlCode.AuthorityType.contact_v2 & flag4)
      {
        this.iban = PayloadGenerator.IsValidIban(iban) ? iban.Replace(" ", "").ToUpper() : throw new PayloadGenerator.BezahlCode.BezahlCodeException("The IBAN entered isn't valid.");
        this.bic = PayloadGenerator.IsValidBic(bic) ? bic.Replace(" ", "").ToUpper() : throw new PayloadGenerator.BezahlCode.BezahlCodeException("The BIC entered isn't valid.");
        if (authority != PayloadGenerator.BezahlCode.AuthorityType.contact_v2)
        {
          this.sepaReference = sepaReference.Length <= 35 ? sepaReference : throw new PayloadGenerator.BezahlCode.BezahlCodeException("SEPA reference texts have to be shorter than 36 chars.");
          this.creditorId = string.IsNullOrEmpty(creditorId) || Regex.IsMatch(creditorId.Replace(" ", ""), "^[a-zA-Z]{2,2}[0-9]{2,2}([A-Za-z0-9]|[\\+|\\?|/|\\-|:|\\(|\\)|\\.|,|']){3,3}([A-Za-z0-9]|[\\+|\\?|/|\\-|:|\\(|\\)|\\.|,|']){1,28}$") ? creditorId : throw new PayloadGenerator.BezahlCode.BezahlCodeException("The creditorId entered isn't valid.");
          this.mandateId = string.IsNullOrEmpty(mandateId) || Regex.IsMatch(mandateId.Replace(" ", ""), "^([A-Za-z0-9]|[\\+|\\?|/|\\-|:|\\(|\\)|\\.|,|']){1,35}$") ? mandateId : throw new PayloadGenerator.BezahlCode.BezahlCodeException("The mandateId entered isn't valid.");
          if (dateOfSignature.HasValue)
            this.dateOfSignature = dateOfSignature.Value;
        }
      }
      if (authority == PayloadGenerator.BezahlCode.AuthorityType.contact || authority == PayloadGenerator.BezahlCode.AuthorityType.contact_v2)
        return;
      if (amount.ToString().Replace(",", ".").Contains("."))
      {
        if (amount.ToString().Replace(",", ".").Split('.')[1].TrimEnd('0').Length > 2)
          throw new PayloadGenerator.BezahlCode.BezahlCodeException("Amount must have less than 3 digits after decimal point.");
      }
      this.amount = !(amount < 0.01M) && !(amount > 999999999.99M) ? amount : throw new PayloadGenerator.BezahlCode.BezahlCodeException("Amount has to at least 0.01 and must be smaller or equal to 999999999.99.");
      this.currency = currency;
      if (!executionDate.HasValue)
      {
        this.executionDate = DateTime.Now;
      }
      else
      {
        if (DateTime.Today.Ticks > executionDate.Value.Ticks)
          throw new PayloadGenerator.BezahlCode.BezahlCodeException("Execution date must be today or in future.");
        this.executionDate = executionDate.Value;
      }
      if (authority != PayloadGenerator.BezahlCode.AuthorityType.periodicsinglepayment && authority != PayloadGenerator.BezahlCode.AuthorityType.periodicsinglepaymentsepa)
        return;
      this.periodicTimeunit = !(periodicTimeunit.ToUpper() != "M") || !(periodicTimeunit.ToUpper() != "W") ? periodicTimeunit : throw new PayloadGenerator.BezahlCode.BezahlCodeException("The periodicTimeunit must be either 'M' (monthly) or 'W' (weekly).");
      this.periodicTimeunitRotation = periodicTimeunitRotation >= 1 && periodicTimeunitRotation <= 52 ? periodicTimeunitRotation : throw new PayloadGenerator.BezahlCode.BezahlCodeException("The periodicTimeunitRotation must be 1 or greater. (It means repeat the payment every 'periodicTimeunitRotation' weeks/months.");
      if (periodicFirstExecutionDate.HasValue)
        this.periodicFirstExecutionDate = periodicFirstExecutionDate.Value;
      if (!periodicLastExecutionDate.HasValue)
        return;
      this.periodicLastExecutionDate = periodicLastExecutionDate.Value;
    }

    public override string ToString()
    {
      string str1 = $"{$"bank://{this.authority}?"}name={Uri.EscapeDataString(this.name)}&";
      if (this.authority != PayloadGenerator.BezahlCode.AuthorityType.contact && this.authority != PayloadGenerator.BezahlCode.AuthorityType.contact_v2)
      {
        string str2;
        DateTime dateTime;
        if (this.authority == PayloadGenerator.BezahlCode.AuthorityType.periodicsinglepayment || this.authority == PayloadGenerator.BezahlCode.AuthorityType.singledirectdebit || this.authority == PayloadGenerator.BezahlCode.AuthorityType.singlepayment)
        {
          str2 = $"{$"{str1}account={this.account}&"}bnc={this.bnc}&";
          if (this.postingKey > 0)
            str2 += $"postingkey={this.postingKey}&";
        }
        else
        {
          str2 = $"{$"{str1}iban={this.iban}&"}bic={this.bic}&";
          if (!string.IsNullOrEmpty(this.sepaReference))
            str2 = $"{str2}separeference={Uri.EscapeDataString(this.sepaReference)}&";
          if (this.authority == PayloadGenerator.BezahlCode.AuthorityType.singledirectdebitsepa)
          {
            if (!string.IsNullOrEmpty(this.creditorId))
              str2 = $"{str2}creditorid={Uri.EscapeDataString(this.creditorId)}&";
            if (!string.IsNullOrEmpty(this.mandateId))
              str2 = $"{str2}mandateid={Uri.EscapeDataString(this.mandateId)}&";
            if (this.dateOfSignature != DateTime.MinValue)
            {
              string str3 = str2;
              dateTime = this.dateOfSignature;
              string str4 = dateTime.ToString("ddMMyyyy");
              str2 = $"{str3}dateofsignature={str4}&";
            }
          }
        }
        string str5 = str2 + $"amount={this.amount:0.00}&".Replace(".", ",");
        if (!string.IsNullOrEmpty(this.reason))
          str5 = $"{str5}reason={Uri.EscapeDataString(this.reason)}&";
        string str6 = str5 + $"currency={this.currency}&";
        dateTime = this.executionDate;
        string str7 = dateTime.ToString("ddMMyyyy");
        str1 = $"{str6}executiondate={str7}&";
        if (this.authority == PayloadGenerator.BezahlCode.AuthorityType.periodicsinglepayment || this.authority == PayloadGenerator.BezahlCode.AuthorityType.periodicsinglepaymentsepa)
        {
          str1 = $"{str1}periodictimeunit={this.periodicTimeunit}&" + $"periodictimeunitrotation={this.periodicTimeunitRotation}&";
          if (this.periodicFirstExecutionDate != DateTime.MinValue)
          {
            string str8 = str1;
            dateTime = this.periodicFirstExecutionDate;
            string str9 = dateTime.ToString("ddMMyyyy");
            str1 = $"{str8}periodicfirstexecutiondate={str9}&";
          }
          if (this.periodicLastExecutionDate != DateTime.MinValue)
          {
            string str10 = str1;
            dateTime = this.periodicLastExecutionDate;
            string str11 = dateTime.ToString("ddMMyyyy");
            str1 = $"{str10}periodiclastexecutiondate={str11}&";
          }
        }
      }
      else
      {
        if (this.authority == PayloadGenerator.BezahlCode.AuthorityType.contact)
          str1 = $"{$"{str1}account={this.account}&"}bnc={this.bnc}&";
        else if (this.authority == PayloadGenerator.BezahlCode.AuthorityType.contact_v2)
          str1 = string.IsNullOrEmpty(this.account) || string.IsNullOrEmpty(this.bnc) ? $"{$"{str1}iban={this.iban}&"}bic={this.bic}&" : $"{$"{str1}account={this.account}&"}bnc={this.bnc}&";
        if (!string.IsNullOrEmpty(this.reason))
          str1 = $"{str1}reason={Uri.EscapeDataString(this.reason)}&";
      }
      return str1.Trim('&');
    }

    public enum Currency
    {
      ALL = 8,
      DZD = 12, // 0x0000000C
      ARS = 32, // 0x00000020
      AUD = 36, // 0x00000024
      BSD = 44, // 0x0000002C
      BHD = 48, // 0x00000030
      BDT = 50, // 0x00000032
      AMD = 51, // 0x00000033
      BBD = 52, // 0x00000034
      BMD = 60, // 0x0000003C
      BTN = 64, // 0x00000040
      BOB = 68, // 0x00000044
      BWP = 72, // 0x00000048
      BZD = 84, // 0x00000054
      SBD = 90, // 0x0000005A
      BND = 96, // 0x00000060
      MMK = 104, // 0x00000068
      BIF = 108, // 0x0000006C
      KHR = 116, // 0x00000074
      CAD = 124, // 0x0000007C
      CVE = 132, // 0x00000084
      KYD = 136, // 0x00000088
      LKR = 144, // 0x00000090
      CLP = 152, // 0x00000098
      CNY = 156, // 0x0000009C
      COP = 170, // 0x000000AA
      KMF = 174, // 0x000000AE
      CRC = 188, // 0x000000BC
      HRK = 191, // 0x000000BF
      CUP = 192, // 0x000000C0
      CZK = 203, // 0x000000CB
      DKK = 208, // 0x000000D0
      DOP = 214, // 0x000000D6
      SVC = 222, // 0x000000DE
      ETB = 230, // 0x000000E6
      ERN = 232, // 0x000000E8
      FKP = 238, // 0x000000EE
      FJD = 242, // 0x000000F2
      DJF = 262, // 0x00000106
      GMD = 270, // 0x0000010E
      GIP = 292, // 0x00000124
      GTQ = 320, // 0x00000140
      GNF = 324, // 0x00000144
      GYD = 328, // 0x00000148
      HTG = 332, // 0x0000014C
      HNL = 340, // 0x00000154
      HKD = 344, // 0x00000158
      HUF = 348, // 0x0000015C
      ISK = 352, // 0x00000160
      INR = 356, // 0x00000164
      IDR = 360, // 0x00000168
      IRR = 364, // 0x0000016C
      IQD = 368, // 0x00000170
      ILS = 376, // 0x00000178
      JMD = 388, // 0x00000184
      JPY = 392, // 0x00000188
      KZT = 398, // 0x0000018E
      JOD = 400, // 0x00000190
      KES = 404, // 0x00000194
      KPW = 408, // 0x00000198
      KRW = 410, // 0x0000019A
      KWD = 414, // 0x0000019E
      KGS = 417, // 0x000001A1
      LAK = 418, // 0x000001A2
      LBP = 422, // 0x000001A6
      LSL = 426, // 0x000001AA
      LRD = 430, // 0x000001AE
      LYD = 434, // 0x000001B2
      MOP = 446, // 0x000001BE
      MWK = 454, // 0x000001C6
      MYR = 458, // 0x000001CA
      MVR = 462, // 0x000001CE
      MRO = 478, // 0x000001DE
      MUR = 480, // 0x000001E0
      MXN = 484, // 0x000001E4
      MNT = 496, // 0x000001F0
      MDL = 498, // 0x000001F2
      MAD = 504, // 0x000001F8
      OMR = 512, // 0x00000200
      NAD = 516, // 0x00000204
      NPR = 524, // 0x0000020C
      ANG = 532, // 0x00000214
      AWG = 533, // 0x00000215
      VUV = 548, // 0x00000224
      NZD = 554, // 0x0000022A
      NIO = 558, // 0x0000022E
      NGN = 566, // 0x00000236
      NOK = 578, // 0x00000242
      PKR = 586, // 0x0000024A
      PAB = 590, // 0x0000024E
      PGK = 598, // 0x00000256
      PYG = 600, // 0x00000258
      PEN = 604, // 0x0000025C
      PHP = 608, // 0x00000260
      QAR = 634, // 0x0000027A
      RUB = 643, // 0x00000283
      RWF = 646, // 0x00000286
      SHP = 654, // 0x0000028E
      STD = 678, // 0x000002A6
      SAR = 682, // 0x000002AA
      SCR = 690, // 0x000002B2
      SLL = 694, // 0x000002B6
      SGD = 702, // 0x000002BE
      VND = 704, // 0x000002C0
      SOS = 706, // 0x000002C2
      ZAR = 710, // 0x000002C6
      SSP = 728, // 0x000002D8
      SZL = 748, // 0x000002EC
      SEK = 752, // 0x000002F0
      CHF = 756, // 0x000002F4
      SYP = 760, // 0x000002F8
      THB = 764, // 0x000002FC
      TOP = 776, // 0x00000308
      TTD = 780, // 0x0000030C
      AED = 784, // 0x00000310
      TND = 788, // 0x00000314
      UGX = 800, // 0x00000320
      MKD = 807, // 0x00000327
      EGP = 818, // 0x00000332
      GBP = 826, // 0x0000033A
      TZS = 834, // 0x00000342
      USD = 840, // 0x00000348
      UYU = 858, // 0x0000035A
      UZS = 860, // 0x0000035C
      WST = 882, // 0x00000372
      YER = 886, // 0x00000376
      TWD = 901, // 0x00000385
      CUC = 931, // 0x000003A3
      ZWL = 932, // 0x000003A4
      TMT = 934, // 0x000003A6
      GHS = 936, // 0x000003A8
      VEF = 937, // 0x000003A9
      SDG = 938, // 0x000003AA
      UYI = 940, // 0x000003AC
      RSD = 941, // 0x000003AD
      MZN = 943, // 0x000003AF
      AZN = 944, // 0x000003B0
      RON = 946, // 0x000003B2
      CHE = 947, // 0x000003B3
      CHW = 948, // 0x000003B4
      TRY = 949, // 0x000003B5
      XAF = 950, // 0x000003B6
      XCD = 951, // 0x000003B7
      XOF = 952, // 0x000003B8
      XPF = 953, // 0x000003B9
      XBA = 955, // 0x000003BB
      XBB = 956, // 0x000003BC
      XBC = 957, // 0x000003BD
      XBD = 958, // 0x000003BE
      XAU = 959, // 0x000003BF
      XDR = 960, // 0x000003C0
      XAG = 961, // 0x000003C1
      XPT = 962, // 0x000003C2
      XTS = 963, // 0x000003C3
      XPD = 964, // 0x000003C4
      XUA = 965, // 0x000003C5
      ZMW = 967, // 0x000003C7
      SRD = 968, // 0x000003C8
      MGA = 969, // 0x000003C9
      COU = 970, // 0x000003CA
      AFN = 971, // 0x000003CB
      TJS = 972, // 0x000003CC
      AOA = 973, // 0x000003CD
      BYR = 974, // 0x000003CE
      BGN = 975, // 0x000003CF
      CDF = 976, // 0x000003D0
      BAM = 977, // 0x000003D1
      EUR = 978, // 0x000003D2
      MXV = 979, // 0x000003D3
      UAH = 980, // 0x000003D4
      GEL = 981, // 0x000003D5
      BOV = 984, // 0x000003D8
      PLN = 985, // 0x000003D9
      BRL = 986, // 0x000003DA
      CLF = 990, // 0x000003DE
      XSU = 994, // 0x000003E2
      USN = 997, // 0x000003E5
      XXX = 999, // 0x000003E7
    }

    public enum AuthorityType
    {
      [Obsolete] singlepayment,
      singlepaymentsepa,
      [Obsolete] singledirectdebit,
      singledirectdebitsepa,
      [Obsolete] periodicsinglepayment,
      periodicsinglepaymentsepa,
      contact,
      contact_v2,
    }

    public class BezahlCodeException : Exception
    {
      public BezahlCodeException()
      {
      }

      public BezahlCodeException(string message)
        : base(message)
      {
      }

      public BezahlCodeException(string message, Exception inner)
        : base(message, inner)
      {
      }
    }
  }

  public class CalendarEvent : PayloadGenerator.Payload
  {
    private readonly string subject;
    private readonly string description;
    private readonly string location;
    private readonly string start;
    private readonly string end;
    private readonly PayloadGenerator.CalendarEvent.EventEncoding encoding;

    public CalendarEvent(
      string subject,
      string description,
      string location,
      DateTime start,
      DateTime end,
      bool allDayEvent,
      PayloadGenerator.CalendarEvent.EventEncoding encoding = PayloadGenerator.CalendarEvent.EventEncoding.Universal)
    {
      this.subject = subject;
      this.description = description;
      this.location = location;
      this.encoding = encoding;
      string format = allDayEvent ? "yyyyMMdd" : "yyyyMMddTHHmmss";
      this.start = start.ToString(format);
      this.end = end.ToString(format);
    }

    public override string ToString()
    {
      string str = $"{$"{$"{"BEGIN:VEVENT" + Environment.NewLine}SUMMARY:{this.subject}{Environment.NewLine}" + (!string.IsNullOrEmpty(this.description) ? $"DESCRIPTION:{this.description}{Environment.NewLine}" : "") + (!string.IsNullOrEmpty(this.location) ? $"LOCATION:{this.location}{Environment.NewLine}" : "")}DTSTART:{this.start}{Environment.NewLine}"}DTEND:{this.end}{Environment.NewLine}" + "END:VEVENT";
      if (this.encoding == PayloadGenerator.CalendarEvent.EventEncoding.iCalComplete)
        str = $"BEGIN:VCALENDAR{Environment.NewLine}VERSION:2.0{Environment.NewLine}{str}{Environment.NewLine}END:VCALENDAR";
      return str;
    }

    public enum EventEncoding
    {
      iCalComplete,
      Universal,
    }
  }

  public class OneTimePassword : PayloadGenerator.Payload
  {
    public PayloadGenerator.OneTimePassword.OneTimePasswordAuthType Type { get; set; }

    public string Secret { get; set; }

    public PayloadGenerator.OneTimePassword.OneTimePasswordAuthAlgorithm AuthAlgorithm { get; set; }

    [Obsolete("This property is obsolete, use AuthAlgorithm instead", false)]
    public PayloadGenerator.OneTimePassword.OoneTimePasswordAuthAlgorithm Algorithm
    {
      get
      {
        return (PayloadGenerator.OneTimePassword.OoneTimePasswordAuthAlgorithm) Enum.Parse(typeof (PayloadGenerator.OneTimePassword.OoneTimePasswordAuthAlgorithm), this.AuthAlgorithm.ToString());
      }
      set
      {
        this.AuthAlgorithm = (PayloadGenerator.OneTimePassword.OneTimePasswordAuthAlgorithm) Enum.Parse(typeof (PayloadGenerator.OneTimePassword.OneTimePasswordAuthAlgorithm), value.ToString());
      }
    }

    public string Issuer { get; set; }

    public string Label { get; set; }

    public int Digits { get; set; } = 6;

    public int? Counter { get; set; }

    public int? Period { get; set; } = new int?(30);

    public override string ToString()
    {
      switch (this.Type)
      {
        case PayloadGenerator.OneTimePassword.OneTimePasswordAuthType.TOTP:
          return this.TimeToString();
        case PayloadGenerator.OneTimePassword.OneTimePasswordAuthType.HOTP:
          return this.HMACToString();
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private string HMACToString()
    {
      StringBuilder sb = new StringBuilder("otpauth://hotp/");
      this.ProcessCommonFields(sb);
      int num = this.Counter ?? 1;
      sb.Append("&counter=" + num.ToString());
      return sb.ToString();
    }

    private string TimeToString()
    {
      if (!this.Period.HasValue)
        throw new Exception("Period must be set when using OneTimePasswordAuthType.TOTP");
      StringBuilder sb = new StringBuilder("otpauth://totp/");
      this.ProcessCommonFields(sb);
      int? period = this.Period;
      int num = 30;
      if (!(period.GetValueOrDefault() == num & period.HasValue))
      {
        StringBuilder stringBuilder = sb;
        period = this.Period;
        string str = "&period=" + period.ToString();
        stringBuilder.Append(str);
      }
      return sb.ToString();
    }

    private void ProcessCommonFields(StringBuilder sb)
    {
      string str1 = !String40Methods.IsNullOrWhiteSpace(this.Secret) ? this.Secret.Replace(" ", "") : throw new Exception("Secret must be a filled out base32 encoded string");
      string str2 = (string) null;
      string str3 = (string) null;
      if (!String40Methods.IsNullOrWhiteSpace(this.Issuer))
        str2 = !this.Issuer.Contains(":") ? Uri.EscapeDataString(this.Issuer) : throw new Exception("Issuer must not have a ':'");
      if (!String40Methods.IsNullOrWhiteSpace(this.Label) && this.Label.Contains(":"))
        throw new Exception("Label must not have a ':'");
      if (this.Label != null && this.Issuer != null)
        str3 = $"{this.Issuer}:{this.Label}";
      else if (this.Issuer != null)
        str3 = this.Issuer;
      if (str3 != null)
        sb.Append(str3);
      sb.Append("?secret=" + str1);
      if (str2 != null)
        sb.Append("&issuer=" + str2);
      if (this.Digits == 6)
        return;
      sb.Append("&digits=" + this.Digits.ToString());
    }

    public enum OneTimePasswordAuthType
    {
      TOTP,
      HOTP,
    }

    public enum OneTimePasswordAuthAlgorithm
    {
      SHA1,
      SHA256,
      SHA512,
    }

    [Obsolete("This enum is obsolete, use OneTimePasswordAuthAlgorithm instead", false)]
    public enum OoneTimePasswordAuthAlgorithm
    {
      SHA1,
      SHA256,
      SHA512,
    }
  }

  public class ShadowSocksConfig : PayloadGenerator.Payload
  {
    private readonly string hostname;
    private readonly string password;
    private readonly string tag;
    private readonly string methodStr;
    private readonly string parameter;
    private readonly PayloadGenerator.ShadowSocksConfig.Method method;
    private readonly int port;
    private Dictionary<string, string> encryptionTexts = new Dictionary<string, string>()
    {
      {
        "Chacha20IetfPoly1305",
        "chacha20-ietf-poly1305"
      },
      {
        "Aes128Gcm",
        "aes-128-gcm"
      },
      {
        "Aes192Gcm",
        "aes-192-gcm"
      },
      {
        "Aes256Gcm",
        "aes-256-gcm"
      },
      {
        "XChacha20IetfPoly1305",
        "xchacha20-ietf-poly1305"
      },
      {
        "Aes128Cfb",
        "aes-128-cfb"
      },
      {
        "Aes192Cfb",
        "aes-192-cfb"
      },
      {
        "Aes256Cfb",
        "aes-256-cfb"
      },
      {
        "Aes128Ctr",
        "aes-128-ctr"
      },
      {
        "Aes192Ctr",
        "aes-192-ctr"
      },
      {
        "Aes256Ctr",
        "aes-256-ctr"
      },
      {
        "Camellia128Cfb",
        "camellia-128-cfb"
      },
      {
        "Camellia192Cfb",
        "camellia-192-cfb"
      },
      {
        "Camellia256Cfb",
        "camellia-256-cfb"
      },
      {
        "Chacha20Ietf",
        "chacha20-ietf"
      },
      {
        "Aes256Cb",
        "aes-256-cfb"
      },
      {
        "Aes128Ofb",
        "aes-128-ofb"
      },
      {
        "Aes192Ofb",
        "aes-192-ofb"
      },
      {
        "Aes256Ofb",
        "aes-256-ofb"
      },
      {
        "Aes128Cfb1",
        "aes-128-cfb1"
      },
      {
        "Aes192Cfb1",
        "aes-192-cfb1"
      },
      {
        "Aes256Cfb1",
        "aes-256-cfb1"
      },
      {
        "Aes128Cfb8",
        "aes-128-cfb8"
      },
      {
        "Aes192Cfb8",
        "aes-192-cfb8"
      },
      {
        "Aes256Cfb8",
        "aes-256-cfb8"
      },
      {
        "Chacha20",
        "chacha20"
      },
      {
        "BfCfb",
        "bf-cfb"
      },
      {
        "Rc4Md5",
        "rc4-md5"
      },
      {
        "Salsa20",
        "salsa20"
      },
      {
        "DesCfb",
        "des-cfb"
      },
      {
        "IdeaCfb",
        "idea-cfb"
      },
      {
        "Rc2Cfb",
        "rc2-cfb"
      },
      {
        "Cast5Cfb",
        "cast5-cfb"
      },
      {
        "Salsa20Ctr",
        "salsa20-ctr"
      },
      {
        "Rc4",
        "rc4"
      },
      {
        "SeedCfb",
        "seed-cfb"
      },
      {
        "Table",
        "table"
      }
    };
    private Dictionary<string, string> UrlEncodeTable = new Dictionary<string, string>()
    {
      [" "] = "+",
      ["\0"] = "%00",
      ["\t"] = "%09",
      ["\n"] = "%0a",
      ["\r"] = "%0d",
      ["\""] = "%22",
      ["#"] = "%23",
      ["$"] = "%24",
      ["%"] = "%25",
      ["&"] = "%26",
      ["'"] = "%27",
      ["+"] = "%2b",
      [","] = "%2c",
      ["/"] = "%2f",
      [":"] = "%3a",
      [";"] = "%3b",
      ["<"] = "%3c",
      ["="] = "%3d",
      [">"] = "%3e",
      ["?"] = "%3f",
      ["@"] = "%40",
      ["["] = "%5b",
      ["\\"] = "%5c",
      ["]"] = "%5d",
      ["^"] = "%5e",
      ["`"] = "%60",
      ["{"] = "%7b",
      ["|"] = "%7c",
      ["}"] = "%7d",
      ["~"] = "%7e"
    };

    public ShadowSocksConfig(
      string hostname,
      int port,
      string password,
      PayloadGenerator.ShadowSocksConfig.Method method,
      string tag = null)
      : this(hostname, port, password, method, (Dictionary<string, string>) null, tag)
    {
    }

    public ShadowSocksConfig(
      string hostname,
      int port,
      string password,
      PayloadGenerator.ShadowSocksConfig.Method method,
      string plugin,
      string pluginOption,
      string tag = null)
    {
      string hostname1 = hostname;
      int port1 = port;
      string password1 = password;
      int num = (int) method;
      Dictionary<string, string> parameters = new Dictionary<string, string>();
      parameters[nameof (plugin)] = plugin + (string.IsNullOrEmpty(pluginOption) ? "" : ";" + pluginOption);
      string tag1 = tag;
      // ISSUE: explicit constructor call
      this.\u002Ector(hostname1, port1, password1, (PayloadGenerator.ShadowSocksConfig.Method) num, parameters, tag1);
    }

    private string UrlEncode(string i)
    {
      string str = i;
      foreach (KeyValuePair<string, string> keyValuePair in this.UrlEncodeTable)
        str = str.Replace(keyValuePair.Key, keyValuePair.Value);
      return str;
    }

    public ShadowSocksConfig(
      string hostname,
      int port,
      string password,
      PayloadGenerator.ShadowSocksConfig.Method method,
      Dictionary<string, string> parameters,
      string tag = null)
    {
      this.hostname = Uri.CheckHostName(hostname) == UriHostNameType.IPv6 ? $"[{hostname}]" : hostname;
      this.port = port >= 1 && port <= (int) ushort.MaxValue ? port : throw new PayloadGenerator.ShadowSocksConfig.ShadowSocksConfigException("Value of 'port' must be within 0 and 65535.");
      this.password = password;
      this.method = method;
      this.methodStr = this.encryptionTexts[method.ToString()];
      this.tag = tag;
      if (parameters == null)
        return;
      this.parameter = string.Join("&", parameters.Select<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (kv => $"{this.UrlEncode(kv.Key)}={this.UrlEncode(kv.Value)}")).ToArray<string>());
    }

    public override string ToString()
    {
      return string.IsNullOrEmpty(this.parameter) ? $"ss://{Convert.ToBase64String(Encoding.UTF8.GetBytes($"{this.methodStr}:{this.password}@{this.hostname}:{this.port}"))}{(!string.IsNullOrEmpty(this.tag) ? "#" + this.tag : string.Empty)}" : $"ss://{Convert.ToBase64String(Encoding.UTF8.GetBytes($"{this.methodStr}:{this.password}")).Replace('+', '-').Replace('/', '_').TrimEnd('=')}@{this.hostname}:{this.port}/?{this.parameter}{(!string.IsNullOrEmpty(this.tag) ? (object) ("#" + this.tag) : (object) string.Empty)}";
    }

    public enum Method
    {
      Chacha20IetfPoly1305,
      Aes128Gcm,
      Aes192Gcm,
      Aes256Gcm,
      XChacha20IetfPoly1305,
      Aes128Cfb,
      Aes192Cfb,
      Aes256Cfb,
      Aes128Ctr,
      Aes192Ctr,
      Aes256Ctr,
      Camellia128Cfb,
      Camellia192Cfb,
      Camellia256Cfb,
      Chacha20Ietf,
      Aes256Cb,
      Aes128Ofb,
      Aes192Ofb,
      Aes256Ofb,
      Aes128Cfb1,
      Aes192Cfb1,
      Aes256Cfb1,
      Aes128Cfb8,
      Aes192Cfb8,
      Aes256Cfb8,
      Chacha20,
      BfCfb,
      Rc4Md5,
      Salsa20,
      DesCfb,
      IdeaCfb,
      Rc2Cfb,
      Cast5Cfb,
      Salsa20Ctr,
      Rc4,
      SeedCfb,
      Table,
    }

    public class ShadowSocksConfigException : Exception
    {
      public ShadowSocksConfigException()
      {
      }

      public ShadowSocksConfigException(string message)
        : base(message)
      {
      }

      public ShadowSocksConfigException(string message, Exception inner)
        : base(message, inner)
      {
      }
    }
  }

  public class MoneroTransaction : PayloadGenerator.Payload
  {
    private readonly string address;
    private readonly string txPaymentId;
    private readonly string recipientName;
    private readonly string txDescription;
    private readonly float? txAmount;

    public MoneroTransaction(
      string address,
      float? txAmount = null,
      string txPaymentId = null,
      string recipientName = null,
      string txDescription = null)
    {
      this.address = !string.IsNullOrEmpty(address) ? address : throw new PayloadGenerator.MoneroTransaction.MoneroTransactionException("The address is mandatory and has to be set.");
      if (txAmount.HasValue)
      {
        float? nullable = txAmount;
        float num = 0.0f;
        if ((double) nullable.GetValueOrDefault() <= (double) num & nullable.HasValue)
          throw new PayloadGenerator.MoneroTransaction.MoneroTransactionException("Value of 'txAmount' must be greater than 0.");
      }
      this.txAmount = txAmount;
      this.txPaymentId = txPaymentId;
      this.recipientName = recipientName;
      this.txDescription = txDescription;
    }

    public override string ToString()
    {
      return ($"monero://{this.address}{(!string.IsNullOrEmpty(this.txPaymentId) || !string.IsNullOrEmpty(this.recipientName) || !string.IsNullOrEmpty(this.txDescription) || this.txAmount.HasValue ? "?" : string.Empty)}" + (!string.IsNullOrEmpty(this.txPaymentId) ? $"tx_payment_id={Uri.EscapeDataString(this.txPaymentId)}&" : string.Empty) + (!string.IsNullOrEmpty(this.recipientName) ? $"recipient_name={Uri.EscapeDataString(this.recipientName)}&" : string.Empty) + (this.txAmount.HasValue ? $"tx_amount={this.txAmount.ToString().Replace(",", ".")}&" : string.Empty) + (!string.IsNullOrEmpty(this.txDescription) ? "tx_description=" + Uri.EscapeDataString(this.txDescription) : string.Empty)).TrimEnd('&');
    }

    public class MoneroTransactionException : Exception
    {
      public MoneroTransactionException()
      {
      }

      public MoneroTransactionException(string message)
        : base(message)
      {
      }

      public MoneroTransactionException(string message, Exception inner)
        : base(message, inner)
      {
      }
    }
  }

  public class SlovenianUpnQr : PayloadGenerator.Payload
  {
    private string _payerName = "";
    private string _payerAddress = "";
    private string _payerPlace = "";
    private string _amount = "";
    private string _code = "";
    private string _purpose = "";
    private string _deadLine = "";
    private string _recipientIban = "";
    private string _recipientName = "";
    private string _recipientAddress = "";
    private string _recipientPlace = "";
    private string _recipientSiModel = "";
    private string _recipientSiReference = "";

    public override int Version => 15;

    public override QRCodeGenerator.ECCLevel EccLevel => QRCodeGenerator.ECCLevel.M;

    public override QRCodeGenerator.EciMode EciMode => QRCodeGenerator.EciMode.Iso8859_2;

    private string LimitLength(string value, int maxLength)
    {
      return value.Length > maxLength ? value.Substring(0, maxLength) : value;
    }

    public SlovenianUpnQr(
      string payerName,
      string payerAddress,
      string payerPlace,
      string recipientName,
      string recipientAddress,
      string recipientPlace,
      string recipientIban,
      string description,
      double amount,
      string recipientSiModel = "SI00",
      string recipientSiReference = "",
      string code = "OTHR")
      : this(payerName, payerAddress, payerPlace, recipientName, recipientAddress, recipientPlace, recipientIban, description, amount, new DateTime?(), recipientSiModel, recipientSiReference, code)
    {
    }

    public SlovenianUpnQr(
      string payerName,
      string payerAddress,
      string payerPlace,
      string recipientName,
      string recipientAddress,
      string recipientPlace,
      string recipientIban,
      string description,
      double amount,
      DateTime? deadline,
      string recipientSiModel = "SI99",
      string recipientSiReference = "",
      string code = "OTHR")
    {
      this._payerName = this.LimitLength(payerName.Trim(), 33);
      this._payerAddress = this.LimitLength(payerAddress.Trim(), 33);
      this._payerPlace = this.LimitLength(payerPlace.Trim(), 33);
      this._amount = this.FormatAmount(amount);
      this._code = this.LimitLength(code.Trim().ToUpper(), 4);
      this._purpose = this.LimitLength(description.Trim(), 42);
      this._deadLine = !deadline.HasValue ? "" : deadline?.ToString("dd.MM.yyyy");
      this._recipientIban = this.LimitLength(recipientIban.Trim(), 34);
      this._recipientName = this.LimitLength(recipientName.Trim(), 33);
      this._recipientAddress = this.LimitLength(recipientAddress.Trim(), 33);
      this._recipientPlace = this.LimitLength(recipientPlace.Trim(), 33);
      this._recipientSiModel = this.LimitLength(recipientSiModel.Trim().ToUpper(), 4);
      this._recipientSiReference = this.LimitLength(recipientSiReference.Trim(), 22);
    }

    private string FormatAmount(double amount) => $"{(int) Math.Round(amount * 100.0):00000000000}";

    private int CalculateChecksum()
    {
      return 5 + this._payerName.Length + this._payerAddress.Length + this._payerPlace.Length + this._amount.Length + this._code.Length + this._purpose.Length + this._deadLine.Length + this._recipientIban.Length + this._recipientName.Length + this._recipientAddress.Length + this._recipientPlace.Length + this._recipientSiModel.Length + this._recipientSiReference.Length + 19;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("UPNQR");
      stringBuilder.Append('\n').Append('\n').Append('\n').Append('\n').Append('\n');
      stringBuilder.Append(this._payerName).Append('\n');
      stringBuilder.Append(this._payerAddress).Append('\n');
      stringBuilder.Append(this._payerPlace).Append('\n');
      stringBuilder.Append(this._amount).Append('\n').Append('\n').Append('\n');
      stringBuilder.Append(this._code.ToUpper()).Append('\n');
      stringBuilder.Append(this._purpose).Append('\n');
      stringBuilder.Append(this._deadLine).Append('\n');
      stringBuilder.Append(this._recipientIban.ToUpper()).Append('\n');
      stringBuilder.Append(this._recipientSiModel).Append(this._recipientSiReference).Append('\n');
      stringBuilder.Append(this._recipientName).Append('\n');
      stringBuilder.Append(this._recipientAddress).Append('\n');
      stringBuilder.Append(this._recipientPlace).Append('\n');
      stringBuilder.AppendFormat("{0:000}", (object) this.CalculateChecksum()).Append('\n');
      return stringBuilder.ToString();
    }
  }

  public class RussiaPaymentOrder : PayloadGenerator.Payload
  {
    private PayloadGenerator.RussiaPaymentOrder.CharacterSets characterSet;
    private PayloadGenerator.RussiaPaymentOrder.MandatoryFields mFields;
    private PayloadGenerator.RussiaPaymentOrder.OptionalFields oFields;
    private string separator = "|";

    private RussiaPaymentOrder()
    {
      this.mFields = new PayloadGenerator.RussiaPaymentOrder.MandatoryFields();
      this.oFields = new PayloadGenerator.RussiaPaymentOrder.OptionalFields();
    }

    public RussiaPaymentOrder(
      string name,
      string personalAcc,
      string bankName,
      string BIC,
      string correspAcc,
      PayloadGenerator.RussiaPaymentOrder.OptionalFields optionalFields = null,
      PayloadGenerator.RussiaPaymentOrder.CharacterSets characterSet = PayloadGenerator.RussiaPaymentOrder.CharacterSets.utf_8)
      : this()
    {
      this.characterSet = characterSet;
      this.mFields.Name = PayloadGenerator.RussiaPaymentOrder.ValidateInput(name, "Name", "^.{1,160}$");
      this.mFields.PersonalAcc = PayloadGenerator.RussiaPaymentOrder.ValidateInput(personalAcc, "PersonalAcc", "^[1-9]\\d{4}[0-9ABCEHKMPTX]\\d{14}$");
      this.mFields.BankName = PayloadGenerator.RussiaPaymentOrder.ValidateInput(bankName, "BankName", "^.{1,45}$");
      this.mFields.BIC = PayloadGenerator.RussiaPaymentOrder.ValidateInput(BIC, nameof (BIC), "^\\d{9}$");
      this.mFields.CorrespAcc = PayloadGenerator.RussiaPaymentOrder.ValidateInput(correspAcc, "CorrespAcc", "^[1-9]\\d{4}[0-9ABCEHKMPTX]\\d{14}$");
      if (optionalFields == null)
        return;
      this.oFields = optionalFields;
    }

    public override string ToString()
    {
      string name = this.characterSet.ToString().Replace("_", "-");
      byte[] bytes = this.ToBytes();
      return Encoding.GetEncoding(name).GetString(bytes);
    }

    public byte[] ToBytes()
    {
      this.separator = this.DetermineSeparator();
      string str = $"ST0001{((int) this.characterSet).ToString()}{this.separator}Name={this.mFields.Name}{this.separator}PersonalAcc={this.mFields.PersonalAcc}{this.separator}BankName={this.mFields.BankName}{this.separator}BIC={this.mFields.BIC}{this.separator}CorrespAcc={this.mFields.CorrespAcc}";
      List<string> optionalFieldsAsList = this.GetOptionalFieldsAsList();
      if (optionalFieldsAsList.Count > 0)
        str = $"{str}|{string.Join("|", optionalFieldsAsList.ToArray())}";
      string s = str + this.separator;
      byte[] numArray = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(this.characterSet.ToString().Replace("_", "-")), Encoding.UTF8.GetBytes(s));
      return numArray.Length <= 300 ? numArray : throw new PayloadGenerator.RussiaPaymentOrder.RussiaPaymentOrderException($"Data too long. Payload must not exceed 300 bytes, but actually is {numArray.Length} bytes long. Remove additional data fields or shorten strings/values.");
    }

    private string DetermineSeparator()
    {
      List<string> mandatoryFieldsAsList = this.GetMandatoryFieldsAsList();
      List<string> optionalFieldsAsList = this.GetOptionalFieldsAsList();
      string[] strArray = new string[21]
      {
        "|",
        "#",
        ";",
        ":",
        "^",
        "_",
        "~",
        "{",
        "}",
        "!",
        "#",
        "$",
        "%",
        "&",
        "(",
        ")",
        "*",
        "+",
        ",",
        "/",
        "@"
      };
      foreach (string str in strArray)
      {
        string sepCandidate = str;
        if (!mandatoryFieldsAsList.Any<string>((Func<string, bool>) (x => x.Contains(sepCandidate))) && !optionalFieldsAsList.Any<string>((Func<string, bool>) (x => x.Contains(sepCandidate))))
          return sepCandidate;
      }
      throw new PayloadGenerator.RussiaPaymentOrder.RussiaPaymentOrderException("No valid separator found.");
    }

    private List<string> GetOptionalFieldsAsList()
    {
      return ((IEnumerable<PropertyInfo>) this.oFields.GetType().GetProperties()).Where<PropertyInfo>((Func<PropertyInfo, bool>) (field => field.GetValue((object) this.oFields, (object[]) null) != null)).Select<PropertyInfo, string>((Func<PropertyInfo, string>) (field =>
      {
        object obj = field.GetValue((object) this.oFields, (object[]) null);
        string str = field.PropertyType.Equals(typeof (DateTime?)) ? ((DateTime) obj).ToString("dd.MM.yyyy") : obj.ToString();
        return $"{field.Name}={str}";
      })).ToList<string>();
    }

    private List<string> GetMandatoryFieldsAsList()
    {
      return ((IEnumerable<FieldInfo>) this.mFields.GetType().GetFields()).Where<FieldInfo>((Func<FieldInfo, bool>) (field => field.GetValue((object) this.mFields) != null)).Select<FieldInfo, string>((Func<FieldInfo, string>) (field =>
      {
        object obj = field.GetValue((object) this.mFields);
        string str = field.FieldType.Equals(typeof (DateTime?)) ? ((DateTime) obj).ToString("dd.MM.yyyy") : obj.ToString();
        return $"{field.Name}={str}";
      })).ToList<string>();
    }

    private static string ValidateInput(
      string input,
      string fieldname,
      string pattern,
      string errorText = null)
    {
      return PayloadGenerator.RussiaPaymentOrder.ValidateInput(input, fieldname, new string[1]
      {
        pattern
      }, errorText);
    }

    private static string ValidateInput(
      string input,
      string fieldname,
      string[] patterns,
      string errorText = null)
    {
      if (input == null)
        throw new PayloadGenerator.RussiaPaymentOrder.RussiaPaymentOrderException($"The input for '{fieldname}' must not be null.");
      foreach (string pattern in patterns)
      {
        if (!Regex.IsMatch(input, pattern))
        {
          string message = errorText;
          if (message == null)
            message = $"The input for '{fieldname}' ({input}) doesn't match the pattern {pattern}";
          throw new PayloadGenerator.RussiaPaymentOrder.RussiaPaymentOrderException(message);
        }
      }
      return input;
    }

    private class MandatoryFields
    {
      public string Name;
      public string PersonalAcc;
      public string BankName;
      public string BIC;
      public string CorrespAcc;
    }

    public class OptionalFields
    {
      private string _sum;
      private string _purpose;
      private string _payeeInn;
      private string _payerInn;
      private string _drawerStatus;
      private string _kpp;
      private string _cbc;
      private string _oktmo;
      private string _paytReason;
      private string _taxPeriod;
      private string _docNo;
      private string _taxPaytKind;

      public string Sum
      {
        get => this._sum;
        set
        {
          this._sum = PayloadGenerator.RussiaPaymentOrder.ValidateInput(value, nameof (Sum), "^\\d{1,18}$");
        }
      }

      public string Purpose
      {
        get => this._purpose;
        set
        {
          this._purpose = PayloadGenerator.RussiaPaymentOrder.ValidateInput(value, nameof (Purpose), "^.{1,160}$");
        }
      }

      public string PayeeINN
      {
        get => this._payeeInn;
        set
        {
          this._payeeInn = PayloadGenerator.RussiaPaymentOrder.ValidateInput(value, nameof (PayeeINN), "^.{1,12}$");
        }
      }

      public string PayerINN
      {
        get => this._payerInn;
        set
        {
          this._payerInn = PayloadGenerator.RussiaPaymentOrder.ValidateInput(value, nameof (PayerINN), "^.{1,12}$");
        }
      }

      public string DrawerStatus
      {
        get => this._drawerStatus;
        set
        {
          this._drawerStatus = PayloadGenerator.RussiaPaymentOrder.ValidateInput(value, nameof (DrawerStatus), "^.{1,2}$");
        }
      }

      public string KPP
      {
        get => this._kpp;
        set
        {
          this._kpp = PayloadGenerator.RussiaPaymentOrder.ValidateInput(value, nameof (KPP), "^.{1,9}$");
        }
      }

      public string CBC
      {
        get => this._cbc;
        set
        {
          this._cbc = PayloadGenerator.RussiaPaymentOrder.ValidateInput(value, nameof (CBC), "^.{1,20}$");
        }
      }

      public string OKTMO
      {
        get => this._oktmo;
        set
        {
          this._oktmo = PayloadGenerator.RussiaPaymentOrder.ValidateInput(value, nameof (OKTMO), "^.{1,11}$");
        }
      }

      public string PaytReason
      {
        get => this._paytReason;
        set
        {
          this._paytReason = PayloadGenerator.RussiaPaymentOrder.ValidateInput(value, nameof (PaytReason), "^.{1,2}$");
        }
      }

      public string TaxPeriod
      {
        get => this._taxPeriod;
        set
        {
          this._taxPeriod = PayloadGenerator.RussiaPaymentOrder.ValidateInput(value, "ТaxPeriod", "^.{1,10}$");
        }
      }

      public string DocNo
      {
        get => this._docNo;
        set
        {
          this._docNo = PayloadGenerator.RussiaPaymentOrder.ValidateInput(value, nameof (DocNo), "^.{1,15}$");
        }
      }

      public DateTime? DocDate { get; set; }

      public string TaxPaytKind
      {
        get => this._taxPaytKind;
        set
        {
          this._taxPaytKind = PayloadGenerator.RussiaPaymentOrder.ValidateInput(value, nameof (TaxPaytKind), "^.{1,2}$");
        }
      }

      public string LastName { get; set; }

      public string FirstName { get; set; }

      public string MiddleName { get; set; }

      public string PayerAddress { get; set; }

      public string PersonalAccount { get; set; }

      public string DocIdx { get; set; }

      public string PensAcc { get; set; }

      public string Contract { get; set; }

      public string PersAcc { get; set; }

      public string Flat { get; set; }

      public string Phone { get; set; }

      public string PayerIdType { get; set; }

      public string PayerIdNum { get; set; }

      public string ChildFio { get; set; }

      public DateTime? BirthDate { get; set; }

      public string PaymTerm { get; set; }

      public string PaymPeriod { get; set; }

      public string Category { get; set; }

      public string ServiceName { get; set; }

      public string CounterId { get; set; }

      public string CounterVal { get; set; }

      public string QuittId { get; set; }

      public DateTime? QuittDate { get; set; }

      public string InstNum { get; set; }

      public string ClassNum { get; set; }

      public string SpecFio { get; set; }

      public string AddAmount { get; set; }

      public string RuleId { get; set; }

      public string ExecId { get; set; }

      public string RegType { get; set; }

      public string UIN { get; set; }

      public PayloadGenerator.RussiaPaymentOrder.TechCode? TechCode { get; set; }
    }

    public enum TechCode
    {
      Мобильная_связь_стационарный_телефон = 1,
      Коммунальные_услуги_ЖКХAFN = 2,
      ГИБДД_налоги_пошлины_бюджетные_платежи = 3,
      Охранные_услуги = 4,
      Услуги_оказываемые_УФМС = 5,
      ПФР = 6,
      Погашение_кредитов = 7,
      Образовательные_учреждения = 8,
      Интернет_и_ТВ = 9,
      Электронные_деньги = 10, // 0x0000000A
      Отдых_и_путешествия = 11, // 0x0000000B
      Инвестиции_и_страхование = 12, // 0x0000000C
      Спорт_и_здоровье = 13, // 0x0000000D
      Благотворительные_и_общественные_организации = 14, // 0x0000000E
      Прочие_услуги = 15, // 0x0000000F
    }

    public enum CharacterSets
    {
      windows_1251 = 1,
      utf_8 = 2,
      koi8_r = 3,
    }

    public class RussiaPaymentOrderException(string message) : Exception(message)
    {
    }
  }
}
