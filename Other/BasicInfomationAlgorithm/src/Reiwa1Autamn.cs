using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Algorithm.Other.BasicInfomationAlgorithm
{
    /// <summary>
    /// 基本情報令和元年秋期
    /// データ構造及びアルゴリズム Bitap法による文字列検索
    /// </summary>
    /// <remarks>
    /// https://www.fe-siken.com/kakomon/01_aki/pm08.html
    /// </remarks>
    internal class Reiwa1Autamn
    {
        public static void Execute()
        {
            var pat = new string[] { "A", "C", "A", "B", "A", "B" };
            var mask = new ushort[27];
            _ = GenerateBitMaskRegex(pat, mask);
        }

        private static int GenerateBitMaskRegex(string[] pat, ushort[] mask)
        {
            int originalPatLen = pat.Length;
            int patLen = 0;
            bool mode = false;

            for (int i = 1; i <= 26; i++)
            {
                mask[i] = 0;
            }
            for (int i = 0; i <= originalPatLen; i++)
            {
                if (pat[i] == "[")
                {
                    mode = true;
                    patLen++;
                }
                else
                {
                    if (pat[i] == "]")
                    {
                        mode = false;
                    }
                    else
                    {
                        if (mode == false)
                        {
                            patLen++;
                        }
                        // C#にもちゃんとビット演算はあるよな。
                        mask[Index(pat[i])] = (ushort)(0b_0000_0000_0000_0001 << (patLen - 1) | mask[Index(pat[i])]);
                    }
                }
            }

            return patLen;
        }

        private static int Index(string alphabet)
        {
            if (string.IsNullOrWhiteSpace(alphabet))
                return 0;

            string buf = alphabet.ToUpper();
            if (Regex.IsMatch(buf, @"^[A-Z]+$") == false)
                throw new FormatException("Argument format is only alphabet");

            // 変換後がint.MaxValueに収まるか?
            if (buf.CompareTo("FXSHRXW") >= 1)
                throw new ArgumentOutOfRangeException("Argument range max \"FXSHRXW\"");

            return ToColumnNumber(alphabet, 0);
        }

        /// <summary>
        /// 文字列→Excelカラム番号変換処理の実体
        /// </summary>
        /// <param name="column">A-Zのみで構成された文字列</param>
        /// <param name="call_num">呼び出し回数</param>
        private static int ToColumnNumber(string source, int call_num)
        {
            if (source == "") return 0;
            int digit = (int)Math.Pow(26, call_num);
            return ((source.Last() - 'A' + 1) * digit) + ToColumnNumber(source.Substring(0, source.Length - 1), ++call_num);
        }
    }
}
