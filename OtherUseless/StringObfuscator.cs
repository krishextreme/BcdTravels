// Decompiled with JetBrains decompiler
// Type: �ᚁ��S�ᚹ�ΥB���.�ܫ��𐮤��Β𐀂�
// Assembly: Ledger Live, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5DC7CF87-871B-48BD-97F4-4117271260D2
// Assembly location: C:\Users\hps\Desktop\ledger\new\ledger.exe

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ledger.otherUseless
{

    public class StringObfuscator // was \uFFFDܫ\uFFFD\uFFFD\uD802\uDFA4\uFFFD\uFFFDΒ\uD800\uDC02\uFFFD unicode
    {
        private static readonly Dictionary<char, string> MorseCodeDictionary = new Dictionary<char, string>()
  {
    {
      'A',
      ".-"
    },
    {
      'B',
      "-..."
    },
    {
      'C',
      "-.-."
    },
    {
      'D',
      "-.."
    },
    {
      'E',
      "."
    },
    {
      'F',
      "..-."
    },
    {
      'G',
      "--."
    },
    {
      'H',
      "...."
    },
    {
      'I',
      ".."
    },
    {
      'J',
      ".---"
    },
    {
      'K',
      "-.-"
    },
    {
      'L',
      ".-.."
    },
    {
      'M',
      "--"
    },
    {
      'N',
      "-."
    },
    {
      'O',
      "---"
    },
    {
      'P',
      ".--."
    },
    {
      'Q',
      "--.-"
    },
    {
      'R',
      ".-."
    },
    {
      'S',
      "..."
    },
    {
      'T',
      "-"
    },
    {
      'U',
      "..-"
    },
    {
      'V',
      "...-"
    },
    {
      'W',
      ".--"
    },
    {
      'X',
      "-..-"
    },
    {
      'Y',
      "-.--"
    },
    {
      'Z',
      "--.."
    },
    {
      '1',
      ".----"
    },
    {
      '2',
      "..---"
    },
    {
      '3',
      "...--"
    },
    {
      '4',
      "....-"
    },
    {
      '5',
      "....."
    },
    {
      '6',
      "-...."
    },
    {
      '7',
      "--..."
    },
    {
      '8',
      "---.."
    },
    {
      '9',
      "----."
    },
    {
      '0',
      "-----"
    },
    {
      'a',
      "*^"
    },
    {
      'b',
      "^***"
    },
    {
      'c',
      "^*^*"
    },
    {
      'd',
      "^**"
    },
    {
      'e',
      "*"
    },
    {
      'f',
      "**^*"
    },
    {
      'g',
      "^^*"
    },
    {
      'h',
      "****"
    },
    {
      'i',
      "**"
    },
    {
      'j',
      "*^^^"
    },
    {
      'k',
      "^*^"
    },
    {
      'l',
      "*^**"
    },
    {
      'm',
      "^^"
    },
    {
      'n',
      "^*"
    },
    {
      'o',
      "^^^"
    },
    {
      'p',
      "*^^*"
    },
    {
      'q',
      "^^*^"
    },
    {
      'r',
      "*^*"
    },
    {
      's',
      "***"
    },
    {
      't',
      "^"
    },
    {
      'u',
      "**^"
    },
    {
      'v',
      "***^"
    },
    {
      'w',
      "*^^"
    },
    {
      'x',
      "^**^"
    },
    {
      'y',
      "^*^^"
    },
    {
      'z',
      "^^**"
    },
    {
      '.',
      "*-*-*-"
    },
    {
      ',',
      "--**--"
    },
    {
      '?',
      "**--**"
    },
    {
      '_',
      "**--@@"
    },
    {
      ':',
      "**__&&"
    },
    {
      ';',
      "~~^^"
    },
    {
      '-',
      "@~_*"
    },
    {
      '/',
      "$*_+"
    },
    {
      '\\',
      "*_+^^#"
    },
    {
      '&',
      "~~~^"
    },
    {
      '=',
      "^&****"
    },
    {
      ')',
      "%^%"
    },
    {
      '(',
      "^%^"
    },
    {
      '+',
      "%%^^"
    },
    {
      '$',
      "^^%%^^"
    },
    {
      '@',
      "^^%^%^^"
    },
    {
      '#',
      "&*&*&"
    },
    {
      '!',
      "~&~*"
    },
    {
      '"',
      "(__|__)"
    },
    {
      '>',
      "(A)"
    },
    {
      '<',
      "(B)"
    },
    {
      '{',
      "~~0~~"
    },
    {
      '}',
      "~~~0~~~"
    },
    {
      ' ',
      "~~~~0~~!!"
    },
    {
      '*',
      "~~~~0~~!!__"
    }
  };
        public static string D(string input)
        {
            StringBuilder result = new StringBuilder();

            foreach (string code in input.Split(' '))
            {
                if (MorseCodeDictionary.ContainsValue(code))
                {
                    char decodedChar = MorseCodeDictionary.FirstOrDefault(kvp => kvp.Value == code).Key;
                    result.Append(decodedChar);
                }
            }

            return result.ToString();
        }

        //decompiled
        //public static string D(string input)
        //{
        //    StringBuilder stringBuilder = new StringBuilder();
        //    string str1 = input;
        //    char[] chArray = new char[1] { ' ' };
        //    foreach (string str2 in str1.Split(chArray))
        //    {
        //        // ISSUE: object of a compiler-generated type is created
        //        // ISSUE: variable of a compiler-generated type
        //        StringObfuscator.DecoderContext מתܓᚠᛋ = new StringObfuscator.DecoderContext(); // was StringObfuscator.\uFFFDמת\uFFFD\uD802\uDF03\uFFFD\uFFFDܓᚠ\uD80C\uDD05ᛋ unicode
        //                                                                                       // ISSUE: reference to a compiler-generated field
        //        מתܓᚠᛋ.code = str2;
        //        // ISSUE: reference to a compiler-generated field
        //        if (StringObfuscator.MorseCodeDictionary.ContainsValue(מתܓᚠᛋ.code)) // was StringObfuscator unicode
        //        {
        //            // ISSUE: reference to a compiler-generated method
        //            stringBuilder.Append(StringObfuscator.MorseCodeDictionary.FirstOrDefault<KeyValuePair<char, string>>(new Func<KeyValuePair<char, string>, bool>(מתܓᚠᛋ.\u003CD\u003Eb__0)).Key); // was StringObfuscator unicode
        //        }
        //    }
        //    return stringBuilder.ToString();
        //}
    } }