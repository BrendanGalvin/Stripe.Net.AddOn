using System.Threading.Tasks;

namespace Stripe.Net.AddOn.Interfaces
{
    public interface IAccount
    {
        /// <summary>
        /// Creates and registers with Stripe, a new customer, based on the given credit card information.
        /// </summary>
        /// <param name="model">The StripeSourceOptions that is passed to create a new customer out of.</param>
        /// <returns></returns>
        Task<string> CreateCustomerAsync(StripeSourceOptions model);

        /// <summary>
        /// Retrieve a Customer's data from Stripe, using their stored token.
        /// </summary>
        /// <param name="customerToken">The token that represents the customer to Stripe.</param>
        /// <returns></returns>
        Task<string> GetCustomerAsync(string customerToken);

        /// <summary>
        /// Returns a JSON representation of a payment plan, from the plan ID specified.
        /// </summary>
        /// <param name="planId">The identifier of the plan in Stripe. This is a string, and can contain alphanumeric characters.</param>
        /// <returns></returns>
        Task<string> GetPlanAsync(string planId);

        /// <summary>
        /// Attempts to sign up a customer for a billing plan. Returns false if an error is thrown.
        /// </summary>
        /// <param name="customerToken">The token that represents the customer to Stripe.</param>
        /// <param name="planId">The payment plan ID (should be a string containing only one integer) to sign up the customer to.</param>
        /// <returns></returns>
        Task<string> SubscribeCustomerAsync(string customerToken, string planId);

        /// <summary>
        /// Deletes a payment plan from Stripe.
        /// 
        /// NOTE: If customers are signed up to a payment plan that is then deleted, unknown behavior may occur. Subscribe customers to a different payment plan before deleting a payment plan.
        /// </summary>
        /// <param name="planId">The ID of the plan to delete. This should be a string containing only an integer and nothing else.</param>
        /// <returns></returns>
        Task<string> DeletePaymentPlanAsync(string planId);

        /// <summary>
        /// Attempts to unsubscribe a customer from a payment plan.
        /// </summary>
        /// <param name="planId">The ID of the plan to unsubscribe from.</param>
        /// <param name="customerToken">The customer token, which represents the customer object to Stripe.</param>        /// <returns></returns>
        Task<string> UnsubscribeCustomerAsync(string customerToken, string planId);

        /// <summary>
        /// Attempts to unsubscribe the customer from an old payment plan, and then subscribe them to a new one.
        /// </summary>
        /// <param name="oldPlanId">The ID of the plan that you would like to unsubscribe the customer from.</param>
        /// <param name="newPlanId">The ID of the new plan you would like to subscribe the customer from.</param>
        /// <param name="customerToken">The token that represents the customer to Stripe.</param>
        /// <returns></returns>
        Task<bool> SwitchSubscriptionAsync(string oldPlanId, string newPlanId, string customerToken);

        /// <summary>
        /// Attempts to delete a customer from Stripe.
        /// </summary>
        /// <param name="customerToken">The token that represents the customer to Stripe.</param>
        /// <returns></returns>
        Task<string> DeleteCustomerAsync(string customerToken);

        /// <summary>
        /// Attempts to create a new payment plan with which to subscribe users in the future.
        /// </summary>
        /// <param name="model">The MVC model to pass, with the parameters filled from user input, which is then mapped to the new plan that is created.</param>
        /// <returns></returns>
        Task<string> CreatePaymentPlanAsync(StripePlanCreateOptions model);
    }
}