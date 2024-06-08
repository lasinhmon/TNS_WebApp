using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNS_Task.Areas.Tasks.Models
{
    public class Function
    {
        public static bool ConvertDateVN(string DateVN, out DateTime DateCorrect)
        {
            DateCorrect = DateTime.Now;
            bool zResult = false;
            string[] zValue = DateVN.Split('-');
            if (zValue.Length == 3)
            {
                try
                {
                    DateCorrect = new DateTime(int.Parse(zValue[2]), int.Parse(zValue[1]), int.Parse(zValue[0]));
                    zResult = true;
                }
                catch (Exception ex)
                {
                    zResult = false;
                }

            }
            else
            {
                zValue = DateVN.Split('/');
                if (zValue.Length == 3)
                {
                    try
                    {
                        DateCorrect = new DateTime(int.Parse(zValue[2]), int.Parse(zValue[1]), int.Parse(zValue[0]));
                        zResult = true;
                    }
                    catch (Exception ex)
                    {
                        zResult = false;
                    }
                }
            }

            return zResult;
        }
    }
}
