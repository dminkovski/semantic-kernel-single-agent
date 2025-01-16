using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PersonalBankingAssistant
{
    internal class BankingAssistant
    {
        Random rnd = new Random();
        Dictionary<String, String> tHistory = new Dictionary<String, String>();

        [KernelFunction("scan_image_of_bill")]
        [Description("Scans an image of a bill based on the fileName parameter and returns a string that contains billId, amount and payeeName.")]
        public String scanImageOfBill(string fileName)
        {
            Console.WriteLine($"Scanning file: {fileName}");
            String billId = Guid.NewGuid().ToString();
            var billInfo = new BillInformation { billId = billId, amount = rnd.NextDouble() * 1000, payeeName = "Davide Antelmo" };
            return billInfo.ToString();
        }

        [KernelFunction("search_transaction_history_for_bill_id")]
        [Description("Searches the transaction history with the billId to check whether a bill has been paid already.")]
        public bool searchTransactionHistoryForBillId(string billId)
        {
            Console.WriteLine($"Checking transaction history: {billId}");
            return tHistory.ContainsKey(billId);
        }

        [KernelFunction("pay_bill")]
        [Description("Pays the bill and returns whether it was successful or not.")]
        public bool payBillWithBillinformation(String billId, Double amount, String payeeName)
        {
            Console.WriteLine($"Paying bill: {billId}");
            tHistory.Add(billId, payeeName);
            return true;
        }
    }
}
