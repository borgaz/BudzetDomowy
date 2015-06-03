using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Budget.Utility_Classes
{
    class UtilityFunctions
    {
        static public bool IsValueCorrect (string value)
        {
            int count = value.Split(',').Length - 1; //sprawdzamy ile razy wystepuje kropka lub przecinek w cyfrze
            count += value.Split('.').Length - 1;
            if (count > 1)
            {
                MessageBox.Show("Niepoprawnie wpisana kwota.");
                return false;
            }

            var tab = value.Split('.');
            var tab2 = value.Split(',');
            if (tab.Last().Length > 2 && tab2.Last().Length > 2) // sprawdza ilosc cyfr po przecinku
            {
                MessageBox.Show("Niepoprawnie wpisana kwota.");
                return false;
            }
            return true;
        }
    }
}
