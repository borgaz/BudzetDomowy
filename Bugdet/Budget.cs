using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace Bugdet
{
    public class Budget
    {
        private String name; // nazwa budzetu
        private String note; // notatka
        private String password; // haslo do budzetu
        private Dictionary<int, Payment> payments; // slownik platnosci
        private Dictionary<int, Category> categories; // slownik platnosci
        private Dictionary<int, SavingsTarget> savingsTargets; //slownik celow, na ktore oszczedzamy
        private BalanceLog balance; // aktualnie najnowsze saldo, w przyszłosci przerobimy na słownik
        private int numberOfPeople; // ilosc osob, dla ktorych prowadzony jest budzet domowy
        private DateTime creationDate; // data stworzenia budzetu

        public override string ToString()
        {
            return "NAME: " + name + " NOTE: " + note + " PASSWORD: " + password + " NUMBER_OF_PEOPLE: " + numberOfPeople
                + " CREATION_DATE: " + creationDate + " BALANCE: \n" + balance;
        }

        public Budget(String note, String name, String password, Dictionary<int, Payment> payments, Dictionary<int, Category> categories,
            Dictionary<int, SavingsTarget> savingsTargets, BalanceLog balance, int numberOfPeople, DateTime creationDate)
        {
            this.note = note;
            this.name = name;
            this.password = password;
            this.payments = payments;
            this.categories = categories;
            this.savingsTargets = savingsTargets;
            this.balance = balance;
            this.numberOfPeople = numberOfPeople;
            this.creationDate = creationDate;
        }

        public String Note
        {
            get
            {
                return this.note;
            }
        }

        public String Name
        {
            get
            {
                return this.name;
            }
        }

        public String Password
        {
            get
            {
                return this.password;
            }
        }

        public int NumberOfPeople
        {
            get
            {
                return this.numberOfPeople;
            }
        }

        public BalanceLog Balance
        {
            get
            {
                return this.balance;
            }
        }

        public Dictionary<int, Payment> Payments
        {
            get
            {
                return this.payments;
            }
        }

        public Dictionary<int, Category> Categories
        {
            get
            {
                return this.categories;
            }
        }

        public Dictionary<int, SavingsTarget> SavingsTargets
        {
            get
            {
                return this.savingsTargets;
            }
        }

        public void SetNumberOfPeople(int number)
        {
            this.numberOfPeople = number;
        }

        public void AddNote(String note)
        {
            this.note = note; 
        }

        public void DeleteNumberOfPeople ()
        {
            this.numberOfPeople = 0;
        }

        public void AddSinglePayment(int index, SinglePayment payment)
        {
            payments.Add(index, payment);
        }

        public void DeleteSinglePayment(int index)
        {
            payments.Remove(index);
        }

        public void AddSavingsTarget(int index, SavingsTarget target)
        {
            savingsTargets.Add(index, target);
        }

        public void DeleteSavingsTarget(int index)
        {
            savingsTargets.Remove(index);
        }

        public void AddPeriodPayment(int index, PeriodPayment payment)
        {
            payments.Add(index, payment);
        }

        public void DeletePeriodPayment(int index)
        {
            payments.Remove(index);
        }

        public void SetDefaultCategories()
        {
            var defaultCategories = new Dictionary<int, Category>();
            try
            {
                this.categories.Add(1, new Category("Paliwo", "Benzyna do samochodu"));
                this.categories.Add(2, new Category("Jedzenie", "Zakupy okresowe"));
                this.categories.Add(3, new Category("Prąd", "Rachunki za energie"));
                this.categories.Add(4, new Category("Woda", "Rachunki za wodę"));
                this.categories.Add(5, new Category("Gaz", "Rachunki za gaz"));
                this.categories.Add(6, new Category("Internet", "Rachunki za internet"));
                this.categories.Add(7, new Category("Praca", "wypłata"));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Wystąpił błąd");
                Console.WriteLine(ex.GetBaseException() + "\n addDefaultCategories()");
            }
        }
    }
}