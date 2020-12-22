using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AZTU_Akademik.Classes
{
    public class CodeGenerator
    {
        public static string Generate()
        {
            byte[] codeArray = new byte[16];
            byte randomNumber = 0, minVal = 0, maxVal = 0;
            int randomIndex = 1;

            Random random = new Random();

            for (int i = 0; i < codeArray.Length; i++)
            {
                codeArray[i] = (byte)random.Next(33, 127);

                if (i < 8)
                {
                    if (i < 2)
                    {
                        minVal = 65;
                        maxVal = 91;
                    }
                    else if (i < 4)
                    {
                        minVal = 91;
                        maxVal = 97;
                    }
                    else if (i < 6)
                    {
                        minVal = 33;
                        maxVal = 65;
                    }
                    else if (i < 8)
                    {
                        minVal = 97;
                        maxVal = 127;
                    }

                    randomNumber = (byte)random.Next(minVal, maxVal);
                    randomIndex = (byte)random.Next(0, codeArray.Length);


                    while (codeArray.Contains(randomNumber) || !(codeArray[randomIndex] > 0))
                    {
                        if (codeArray.Contains(randomNumber))
                            randomNumber = (byte)random.Next(minVal, maxVal);

                        if (!(codeArray[randomIndex] > 0))
                            randomIndex = (byte)random.Next(0, codeArray.Length);

                    }

                    codeArray[randomIndex] = randomNumber;

                }
            }

            return string.Join("", Array.ConvertAll(codeArray, x => (char)x));
        }
    }
}
