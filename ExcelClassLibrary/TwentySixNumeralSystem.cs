using System;
using System.IO.Pipes;

namespace ExcelClassLibrary
{  public  struct Index
    {
       public int column;
       public int row;
    }
    
   static public class TwentySixNumeralSystem
    {
        public static string  ToTwentySixBasedNumeralSystem(int i) {
            string res = "";
            int[] arr = new int[50];
            int k = 0;
            while(i>25)
            {
                arr[k] = i / 26 - 1;
                k++;
                i = i % 26;
            }
            arr[k] = i;
            for(int j=0;j<=k;j++)
            {
                res += ((char)('A' + arr[j])).ToString();
            }
            return res;
            }
        public static Index FromTwentySixBasedNumeralSystem(string name)
        {
            Index res = new Index();
            res.column = 0;
            res.row = 0;
            for(int i=0;i<name.Length;i++)
            {
                if((name[i]>='A')&&(name[i]<='Z'))
                {
                    res.column *= 26;
                    res.column += (name[i] - 'A');
                }
                else if ((name[i]>='0')&&(name[i]<='9'))
                {
                    res.row *= 10;
                    res.row += (name[i] - '0');
                }
                
            }
            return res;
        }

    }

}
