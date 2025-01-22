using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PersonalBankingAssistant
{
    internal class BankingAPIMocks
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

        [KernelFunction("get_bill_details")]
        [Description("Fetches detailed information about a bill based on its billId.")]
        public String getBillDetails(String billId)
        {
            Console.WriteLine($"Fetching details for billId: {billId}");
            if (!tHistory.ContainsKey(billId))
            {
                return $"BillId {billId} not found.";
            }
            return $"BillId: {billId}, Payee: {tHistory[billId]}, Status: Paid.";
        }
        // Added overlapping functionality to get_bill_details
        [KernelFunction("fetch_bill_summary")]
        [Description("Fetches a brief summary of the bill, including billId and amount.")]
        public string fetchBillSummary(string billId)
        {
            Console.WriteLine($"Fetching summary for billId: {billId}");
            return $"BillId: {billId}, Amount: {rnd.NextDouble() * 1000}";
        }
        // Overlap with fetch_bill_summary && get_bill_details
        [KernelFunction("fetch_detailed_bill_info")]
        [Description("Fetches detailed information of the bill, including billId, amount, payeeName, and due date.")]
        public string fetchDetailedBillInfo(string billId)
        {
            Console.WriteLine($"Fetching detailed info for billId: {billId}");
            return $"BillId: {billId}, Amount: {rnd.NextDouble() * 1000}, Payee: Davide Antelmo, Due Date: {DateTime.Now.AddDays(7).ToShortDateString()}";
        }

        [KernelFunction("categorize_bill")]
        [Description("Categorizes a bill into predefined categories (e.g., utilities, subscriptions, or one-time purchases).")]
        public string categorizeBill(double amount, string payeeName)
        {
            Console.WriteLine($"Categorizing bill for payee: {payeeName}, amount: {amount}");
            if (amount < 50) return "Subscription";
            if (amount < 500) return "Utilities";
            return "One-time Purchase";
        }

        [KernelFunction("apply_discount")]
        [Description("Applies a discount to the bill amount if the bill is categorized as a subscription.")]
        public double applyDiscount(string category, double amount)
        {
            Console.WriteLine($"Applying discount for category: {category}, amount: {amount}");
            return category == "Subscription" ? amount * 0.9 : amount;
        }

        [KernelFunction("detect_fraud")]
        [Description("Detects potential fraud based on bill amount and payeeName.")]
        public bool detectFraud(double amount, string payeeName)
        {
            Console.WriteLine($"Checking for fraud: payee={payeeName}, amount={amount}");
            return amount > 1000 || payeeName.Contains("Suspicious");
        }

        [KernelFunction("validate_bill_payment")]
        [Description("Validates if a bill can be paid based on its amount and payee.")]
        public bool validateBillPayment(string billId, double amount, string payeeName)
        {
            Console.WriteLine($"Validating payment for BillId: {billId}, Amount: {amount}, Payee: {payeeName}");
            return amount < 1000; // Reject payments above 1000 for demo purposes
        }

        [KernelFunction("send_payment_confirmation")]
        [Description("Sends a payment confirmation to the user.")]
        public void sendPaymentConfirmation(string billId, double amount, string payeeName)
        {
            Console.WriteLine($"Payment confirmation sent for BillId: {billId}, Amount: {amount}, Payee: {payeeName}");
        }

    }
}
