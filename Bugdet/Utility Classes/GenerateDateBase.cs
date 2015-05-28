using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Utility_Classes
{
    public class GenerateDataBase
    {
        string[][] tablica = new string[11][];
        Random amount = new Random(); //losowanie kwoty wydatku
        Random name = new Random(); //losowanie nazwy wydatku
        DateTime data = new DateTime(); 

        int numberOfData = 1000; // ilość rekordów dodanych do bazy
        int minValue = 1; // dolna granica losowanej kwoty
        int maxValue = 300; //górna granica losowanej kwoty
        int salary = 10000; //stała pensja

        private void SetDates()
        {
            // dane dla poszczegolnych kategorii
            tablica[1] = new string[] { "BP", "Orlen", "Shell" };
            tablica[2] = new string[] { "Biedronka", "Żabka", "Auchan", "Tesco", "Dino" };
            //tablica[3] = new string[] { "Rachunki za prąd" };
            //tablica[4] = new string[] { "Rachunki za wodę"};
            //tablica[5] = new string[] { "Rachunki za gaz"};
            //tablica[6] = new string[] { "Rachunki za internet"};
            tablica[7] = new string[] { "Przychód" };
        }
        public void Generate()
        {
            SetDates();    
            data = DateTime.Today.AddDays(-(numberOfData/2)); //wyliczenie daty z przeszłosci
            for (int i = 0; i < numberOfData; i++)
            {
                    int drawnCategory = i % 2 + 1; //rownomierne rozkladanie kategorii
                    int count = tablica[drawnCategory].Count(); //liczba elementow w kategorii
                    Main_Classes.Budget.Instance.AddSinglePayment(i, new Main_Classes.SinglePayment
                        ("", amount.Next(minValue, maxValue), drawnCategory, true, tablica[drawnCategory][name.Next(0, count)], data));

                    if ((i % 2) == 1) // dzien zmienia sie co 2 dodane wydatki
                    {
                        if (data.Month != data.AddDays(1).Month) // pensja i wydatki uwzględniane na koniec miesiąca
                        {
                            Main_Classes.Budget.Instance.AddSinglePayment(++i, new Main_Classes.SinglePayment
                                ("", salary, 7, false, tablica[7][0], data)); //stałe wynagrodzenie
                            Main_Classes.Budget.Instance.AddSinglePayment(++i, new Main_Classes.SinglePayment
                                ("", amount.Next(minValue, maxValue), 3, true, "Rachunki za prąd", data));
                            Main_Classes.Budget.Instance.AddSinglePayment(++i, new Main_Classes.SinglePayment
                                ("", amount.Next(minValue, maxValue), 4, true, "Rachunki za wodę", data));
                            Main_Classes.Budget.Instance.AddSinglePayment(++i, new Main_Classes.SinglePayment
                                ("", amount.Next(minValue, maxValue), 5, true, "Rachunki za gaz", data));
                            Main_Classes.Budget.Instance.AddSinglePayment(++i, new Main_Classes.SinglePayment
                                ("", amount.Next(minValue, maxValue), 6, true, "Rachunki za Internet", data));
                        }
                        data = data.AddDays(1);
                    }
            }
        }
    }
}
