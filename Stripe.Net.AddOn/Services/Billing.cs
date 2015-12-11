using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stripe.Net.AddOn.Interfaces;

namespace Stripe.Net.AddOn.Services
{
    public class Billing : IBilling
    {

        /// <summary>
        /// Attempts to charge a customer a certain amount, which produces a new invoice and attempts to charge the customer.
        /// </summary>
        /// <param name="chargeAmount">The amount, in USD cents, to charge the customer. Only USD is supported currently.</param>
        /// <param name="customerToken">The token that represents the customer to Stripe.</param>
        /// <param name="description">The description that will appear on their invoice.</param>
        /// <returns></returns>
        public async Task<string> ChargeCustomerAsync(int chargeAmount, string customerToken, string description)
        {
            try
            {
                if (chargeAmount == 0)
                {
                    return null;
                }
                var myCharge = new StripeChargeCreateOptions
                {
                    Amount = chargeAmount,
                    Currency = Core.StripeAttributes.Currency, //Currently, the Stripe library only supports USD.
                    Description = description
                };
                myCharge.CustomerId = customerToken;
                var chargeService = new StripeChargeService();
                string json = JsonConvert.SerializeObject(
                    await Task.Run(
                        () => chargeService.Create(myCharge)).ConfigureAwait(false));
                return json;
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }


        /// <summary>
        /// Attempts to add an item to the customer's invoice, if they are on a payment plan. This item will be listed on their invoice at the end of their billing cycle when they receive their invoice, in addition to any fee the plan itself incurs on the customer.
        /// 
        /// NOTE: If you want to immediately charge a customer for something, or charge a customer for something and they do not have a payment plan, use TryChargeCustomer instead.
        /// </summary>
        /// <param name="customerToken">The token string that represents the customer to stripe.</param>
        /// <param name="price">The amount of money, in USD cents, to charge for this invoice item. Only USD is supported currently.</param>
        /// <param name="description">The description of the item to add onto the invoice.</param>
        /// <returns></returns>
        public async Task<string> AddInvoiceItemAsync(string customerToken, int price, string description)
        {
            try
            {
                var myItem = new StripeInvoiceItemCreateOptions
                {
                    Amount = price,
                    Currency = Core.StripeAttributes.Currency,
                    CustomerId = customerToken,
                    Description = description
                };

                var invoiceItemService = new StripeInvoiceItemService();
                string json = JsonConvert.SerializeObject(
                    await Task.Run(
                        () => invoiceItemService.Create(myItem)).ConfigureAwait(false));
                return json;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        
    }
}
