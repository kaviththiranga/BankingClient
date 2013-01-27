using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankingClient
{
    public class CDItem
    {
        public String CDID
        {
            get;
            set;
        }
        public String Title
        {
            get;
            set;
        }

        public String Artist
        {
            set;
            get;
        }

        public String Description
        {
            get;
            set;
        }

        public double Price
        {
            get;
            set;
        }
    }
}
