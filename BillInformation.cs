using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBankingAssistant
{
    public class BillInformation
    {
        public String billId;
        public Double amount;
        public String payeeName;

        override
        public String ToString()
        {
            return $"billId: {billId}, amount: {amount}, payeeName: {payeeName}";
        }
    }
}
