using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    internal class PassGen
    {
        // identiski simboliai
        const int Maximum_Identical = 2;
        // mazosios raides
        const string lower_chars = "abcdefghijklmnopqrstuvwxyz";
        // didziosios raides
        const string capital_chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        // numeriai
        const string numbers = "0123456789";
        // simboliai
        const string symbol = @"!#$%&*@\";

        public string PGen(int lenght, bool lower, bool upper, bool number, bool symbols) {

            List<char[]> PasswordSet = new List<char[]>();
            List<char[]> charSet = new List<char[]>();
            List<int[]> countSet = new List<int[]>();

            //patikrina kas yra suzymeta
            if (lower) charSet.Add(lower_chars.ToArray());
            if (upper) charSet.Add(capital_chars.ToArray());
            if (number) charSet.Add(numbers.ToArray());
            if (symbols) charSet.Add(symbol.ToArray());

            foreach (var c in charSet)
                countSet.Add(new int[c.Length]);

            Random random = new Random();
            for (int i = 0; i < charSet.Count; i++)
            {
                var lng = 1;
                var p0 = "";
                while (true)
                {
                    var ind = random.Next(0, charSet[i].Length);
                    if (countSet[i][ind] < Maximum_Identical)
                    {
                        countSet[i][ind] += 1;
                        lng++;
                        p0 += charSet[i][ind];
                    }
                    if (lng == lenght) break;
                }
                PasswordSet.Add(p0.ToArray());
            }

            var password = "";
            for (int i = 0; i < lenght; i++)
            {
                char p;
                if (i < PasswordSet.Count)
                {
                    int id;
                    do
                    {
                        id = random.Next(0, PasswordSet[i].Length);
                        p = PasswordSet[i][id];

                    } while (p == '\0');
                    password += p;
                    PasswordSet[i][id] = '\0';
                }
                else
                {
                    int id0;
                    int id1;
                    do
                    {
                        id0 = random.Next(0, PasswordSet.Count);
                        id1 = random.Next(0, PasswordSet[id0].Length);
                        p = PasswordSet[id0][id1];
                    } while (p == '\0');
                    password += p;
                    PasswordSet[id0][id1] = '\0';

                }
            }

            password = Shuffle.StringMixer(password);

            return password;
        }





    }
}
