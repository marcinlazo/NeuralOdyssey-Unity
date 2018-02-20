using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class StringKeys
{
    private Dictionary<string, string> keyDictionary;

    public StringKeys()
    {
        string pp = "<", po = ">";
        keyDictionary = new Dictionary<string, string>
        {
            {pp+"name"+po, PlayerData.name},
            {pp+"nickname"+po, PlayerData.nickname},
        };
    }
    public string ReplaceKeys(string input)
    {
        string result = Regex.Replace(input,
                                   @"<[\w\s]*>", // Gets any word or space character between braces @"{([\w\s]*)}",
                                   m => keyDictionary.ContainsKey(m.Groups[0].Value) ? keyDictionary[m.Groups[0].Value] : "[unknown: " + m.Groups[0].Value + "]");
        return result;
    }
}
