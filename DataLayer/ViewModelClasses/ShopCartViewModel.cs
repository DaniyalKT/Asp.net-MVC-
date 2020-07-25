using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataLayer
{
    public class ShopCartItem
    {
        public int ProductID { get; set; }
        public int Count { get; set; }

    }

    public class ShopCartItemViewModel
    {
        public int ProductID { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public int Count { get; set; }
        public int Sum { get; set; }

    }
    public class ShowOrderViewModel
    {
        public int ProductID { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }

        public int Count { get; set; }
        public int Price { get; set; }
        public int Carry { get; set; }
        public int Sum { get; set; }
        public int SumCarry { get; set; }

    }

    public class ShowFactorViewModel
    {
        public int FactorID { get; set; }
        public string ProductTitle { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public int PhoneNumber { get; set; }
        public DateTime Date { get; set; }
    }
}


