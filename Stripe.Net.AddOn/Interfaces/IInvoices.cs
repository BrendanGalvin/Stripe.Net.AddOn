using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stripe.Net.AddOn.Services
{
    public interface IInvoices
    {
        /// <summary>
        /// Returns a payment plan from Stripe, given the ID of a plan you have created in Stripe.
        /// </summary>
        /// <param name="planId">The ID of the payment plan to return. This must be a valid plan ID that you have created in Stripe.</param>
        /// <returns>Returns a string that will either contain a JSON representation of the payment plan, or any errors that occurred.</returns>
        Task<string> GetPaymentPlanAsync(string planId);

        /// <summary>
        /// Gets a Customer's invoices, both past, current, payed and unpayed.
        /// </summary>
        /// <param name="customerToken">The token that represents the customer to Stripe.</param>
        /// <returns></returns>
        Task<IEnumerable<StripeInvoice>> GetInvoicesAsync(string customerToken);

        /// <summary>
        /// Gets a Customer's charges to their account, both past and pending.
        /// </summary>
        /// <param name="customerToken">The token that represents the customer to Stripe.</param>
        /// <returns></returns>
        Task<IEnumerable<StripeCharge>> GetChargesAsync(string customerToken);

        /// <summary>
        /// Gets the list of items on a Customer's invoice.
        /// </summary>
        /// <param name="customerToken">The token that represents the customer to Stripe.</param>
        /// <returns></returns>
        Task<IEnumerable<StripeInvoiceLineItem>> GetLineChargesAsync(string customerToken);
    }
}