using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System;

class Formatter
{
    static string[] suffixes = new string[] { "", "k", "M", "B", "T" };
    public static string IdleValue(double value, string vFormat = "0")
    {
        if (value < 1000)
            return value.ToString(vFormat);

        string rep = value.ToString("0");
        int suffixNum = (rep.Length - 1) / 3;

        int integerLen = rep.Length % 3;
        string format = "0.#";
        if (integerLen == 0)
        {
            integerLen = 3;
            format = "0";
        }

        string shortRep = rep.Substring(0, integerLen);
        if (rep.Length > integerLen)
            shortRep += "." + rep.Substring(integerLen, 2);
        double v = double.Parse(shortRep, CultureInfo.InvariantCulture);
        shortRep = v.ToString(format);
        if (integerLen == 3 && shortRep.Length > 3)
        {
            shortRep = shortRep[0].ToString();
            ++suffixNum;
        }

        string suffix = "";
        if (suffixNum < suffixes.Length)
        {
            suffix = suffixes[suffixNum];
        }
        else
        {
            suffixNum -= suffixes.Length;
            suffix += ((char)(97 + suffixNum / 25)).ToString() + ((char)(97 + suffixNum % 25)).ToString();
        }

        return shortRep + suffix;
    }

    public static string TimeOffset(float time)
    {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public static string TimeOffsetSuffix(float time, string format = "00", bool hideTrailingZeros = false)
    {
        int hours = (int)(time / 3600);
        int minutes = (int)(time % 3600 / 60);
        int seconds = (int)(time % 60);
        string result = "";
        if (hours > 0)
            result += string.Format("{0:" + format + "}h ", hours);

        if (minutes > 0 || !hideTrailingZeros || hours == 0)
            result += string.Format("{0:" + format + "}m ", minutes);

        if ((seconds > 0 || !hideTrailingZeros) && hours == 0)
            result += string.Format("{0:" + format + "}s", seconds);

        return result;
    }

    public static string BoostValue(double value)
    {
        if (value >= 1)
            return "x" + value.ToString("0");
        else
            return (100.0 - value * 100.0).ToString("0") + "%";
    }

    public static string RewardValue(Dictionary<string, object> rewardData)
    {
        if (Convert.ToString(rewardData["reward_type"]) == "currency")
            return IdleValue(Convert.ToDouble(rewardData["reward_value"]));
        else
            return TimeOffsetSuffix(Convert.ToSingle(rewardData["reward_value"]) * 60F, "0", true);

    }

    public static string TimeFormatter(float seconds, bool forceHHMMSS = false)
    {
      return  TimeSpan.FromSeconds(seconds).ToString(@"m\:ss");
    }
}