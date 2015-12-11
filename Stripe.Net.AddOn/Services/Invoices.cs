using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stripe.Net.AddOn.Interfaces;

namespace Stripe.Net.AddOn.Services
{
    public class Invoices : IInvoices
    {

        /// <summary>
        /// Returns a payment plan from Stripe, given the ID of a plan you have created in Stripe.
        /// </summary>
        /// <param name="planId">The ID of the payment plan to return. This must be a valid plan ID that you have created in Stripe.</param>
        /// <returns>Returns a string that will either contain a JSON representation of the payment plan, or any errors that occurred.</returns>
        public async Task<string> GetPaymentPlanAsync(string planId)
        {
            try
            {
                var planService = new StripePlanService();
                string json = JsonConvert.SerializeObject(
                    await Task.Run(
                        () => planService.Get(planId)).ConfigureAwait(false));
                return json;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// Gets a Customer's invoices for payment plans, both past, current, payed and unpayed.
        /// </summary>
        /// <param name="customerToken">The token that represents the customer to Stripe.</param>
        /// <returns></returns>
        public async Task<IEnumerable<StripeInvoice>> GetInvoicesAsync(string customerToken)
        {
            try
            {
                var invoiceService = new StripeInvoiceService();
                IEnumerable<StripeInvoice> enuminv = await Task.Run(
                    () => invoiceService.List(
                        new StripeInvoiceListOptions
                        {
                            CustomerId = customerToken
                        })
                    ).ConfigureAwait(false);
                return enuminv;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Gets a Customer's charges to their account, both past and pending.
        /// </summary>
        /// <param name="customerToken">The token that represents the customer to Stripe.</param>
        /// <returns></returns>
        public async Task<IEnumerable<StripeCharge>> GetChargesAsync(string customerToken)
        {
            try
            {
                var invoiceService = new StripeChargeService();
                IEnumerable<StripeCharge> enuminv = await Task.Run(
                    () => invoiceService.List(new StripeChargeListOptions
                    {
                        CustomerId = customerToken
                    })
                    ).ConfigureAwait(false);
                return enuminv;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Gets the list of items on a Customer's invoice.
        /// </summary>
        /// <param name="customerToken">The token that represents the customer to Stripe.</param>
        /// <returns></returns>
        public async Task<IEnumerable<StripeInvoiceLineItem>> GetLineChargesAsync(string customerToken)
        {
            try
            {
                var invoiceService = new StripeInvoiceItemService();
                IEnumerable<StripeInvoiceLineItem> enuminv = await Task.Run(
                    () => invoiceService.List(new StripeInvoiceItemListOptions
                    {
                        CustomerId = customerToken
                    })
                    );
                return enuminv;
            }
            catch
            {
                return null;
            }
        }
    }
}
