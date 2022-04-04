using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LapsUtility
{
   
    public class StockType
    {
        public const int PurchaseorStock = 1;
        public const int AdditionOrupdate = 2;
        public const int DeductionOrdamage = 3;
        public const int DeductionOrsale = 4;
        public const int DeductionOrReplacement = 5;
        public const int AddOrSalesUpdate = 6;

    }

  
   public class SaleStates
    {
        public class State
        {
            public static int Save
            {
                get { return 1; }
            }

            public static int SaveAsBooked
            {
                get { return 2; }
            }

            public static int Unrecognized
            {
                get { return 3; }
            }

            public static int SaveAsDraft
            {
                get { return 4; }
            }

            public static int Sold
            {
                get { return 5; }
            }

        }
    }

    public class DiscountType
    {
        public class Code
        {
            public static string CashDiscount
            {
                get { return "01"; }
            }

            public static string SpecialDiscount
            {
                get { return "02"; }
            }
        }
    }

    public class StockCategoryType
    {
        public static int Sale
        {
            get { return 1; }
        }

        public static int Replacement
        {
            get { return 2; }
        }

    }
}
