// Decompiled with JetBrains decompiler
// Type: NLog.LogReceiverService.NLogEvent
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

#nullable disable
namespace NLog.LogReceiverService;

[DataContract(Name = "e", Namespace = "http://nlog-project.org/ws/")]
[XmlType(Namespace = "http://nlog-project.org/ws/")]
[DebuggerDisplay("Event ID = {Id} Level={LevelName} Values={Values.Count}")]
public class NLogEvent
{
  public NLogEvent() => this.ValueIndexes = (IList<int>) new List<int>();

  [DataMember(Name = "id", Order = 0)]
  [XmlElement("id", Order = 0)]
  public int Id { get; set; }

  [DataMember(Name = "lv", Order = 1)]
  [XmlElement("lv", Order = 1)]
  public int LevelOrdinal { get; set; }

  [DataMember(Name = "lg", Order = 2)]
  [XmlElement("lg", Order = 2)]
  public int LoggerOrdinal { get; set; }

  [DataMember(Name = "ts", Order = 3)]
  [XmlElement("ts", Order = 3)]
  public long TimeDelta { get; set; }

  [DataMember(Name = "m", Order = 4)]
  [XmlElement("m", Order = 4)]
  public int MessageOrdinal { get; set; }

  [DataMember(Name = "val", Order = 100)]
  [XmlElement("val", Order = 100)]
  public string Values
  {
    get
    {
      StringBuilder stringBuilder = new StringBuilder();
      string str = string.Empty;
      if (this.ValueIndexes != null)
      {
        foreach (int valueIndex in (IEnumerable<int>) this.ValueIndexes)
        {
          stringBuilder.Append(str);
          stringBuilder.Append(valueIndex);
          str = "|";
        }
      }
      return stringBuilder.ToString();
    }
    set
    {
      if (this.ValueIndexes != null)
        this.ValueIndexes.Clear();
      else
        this.ValueIndexes = (IList<int>) new List<int>();
      if (string.IsNullOrEmpty(value))
        return;
      string str1 = value;
      char[] chArray = new char[1]{ '|' };
      foreach (string str2 in str1.Split(chArray))
        this.ValueIndexes.Add(Convert.ToInt32(str2, (IFormatProvider) CultureInfo.InvariantCulture));
    }
  }

  [IgnoreDataMember]
  [XmlIgnore]
  internal IList<int> ValueIndexes { get; private set; }

  internal LogEventInfo ToEventInfo(NLogEvents context, string loggerNamePrefix)
  {
    LogEventInfo eventInfo = new LogEventInfo(NLog.LogLevel.FromOrdinal(this.LevelOrdinal), loggerNamePrefix + context.Strings[this.LoggerOrdinal], context.Strings[this.MessageOrdinal]);
    eventInfo.TimeStamp = new DateTime(context.BaseTimeUtc + this.TimeDelta, DateTimeKind.Utc).ToLocalTime();
    for (int index = 0; index < context.LayoutNames.Count; ++index)
    {
      string layoutName = context.LayoutNames[index];
      string str = context.Strings[this.ValueIndexes[index]];
      eventInfo.Properties[(object) layoutName] = (object) str;
    }
    return eventInfo;
  }
}
