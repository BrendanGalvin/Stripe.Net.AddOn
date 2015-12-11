using System;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using Stripe.Net.AddOn.Core;
using Stripe.Net.AddOn.Interfaces;

namespace Stripe.Net.AddOn.Services
{
    public  class Account : IAccount
    {

        /// <summary>
        /// Creates and registers with Stripe, a new customer, based on the given credit card information. It returns the customers Token that represents the customer to Stripe.
        /// </summary>
        /// <param name="model">The StripeSourceOptions that is passed to create a new customer out of.</param>
        /// <returns></returns>
        public async Task<string> CreateCustomerAsync(StripeSourceOptions model)
        {
            try
            {
                var mycust = new StripeCustomerCreateOptions();
                mycust.Source = model;
                var createservice = new StripeCustomerService();
                var customer = await Task.Run(() => createservice.Create(mycust)).ConfigureAwait(false);
                return customer.Id;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }


        /// <summary>
        /// Retrieve a Customer's data from Stripe, using their stored token.
        /// </summary>
        /// <param name="customerToken">The token that represents the customer to Stripe.</param>
        /// <returns></returns>
        public async Task<string> GetCustomerAsync(string customerToken)
        {
            try
            {
                var customerService = new StripeCustomerService();
                var json = JsonConvert.SerializeObject(
                    await Task.Run(
                        () => customerService.Get(customerToken)));
                return json;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }


        /// <summary>
        /// Returns a JSON representation of a payment plan, from the plan ID specified.
        /// </summary>
        /// <param name="planId">The identifier of the plan in Stripe. This is a string, and can contain alphanumeric characters.</param>
        /// <returns></returns>
        public async Task<string> GetPlanAsync(string planId)
        {
            try
            {
                var planService = new StripePlanService();
                string json = JsonConvert.SerializeObject(
                    await Task.Run(
                        () => planService.Get(planId)));
                return json;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        

        /// <summary>
        /// Attempts to sign up a customer for a billing plan. Returns false if an error is thrown.
        /// </summary>
        /// <param name="customerToken">The token that represents the customer to Stripe.</param>
        /// <param name="planId">The payment plan ID (should be a string containing only one integer) to sign up the customer to.</param>
        /// <returns></returns>
        public async Task<string> SubscribeCustomerAsync(string customerToken, string planId)
        {
            try
            {
                var subscriptionService = new StripeSubscriptionService();
                return JsonConvert.SerializeObject(
                    subscriptionService.Create(
                        customerToken, planId));
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }



        /// <summary>
        /// Deletes a payment plan from Stripe.
        /// 
        /// NOTE: If customers are signed up to a payment plan that is then deleted, unknown behavior may occur. Subscribe customers to a different payment plan before deleting a payment plan.
        /// </summary>
        /// <param name="planId">The ID of the plan to delete. This should be a string containing only an integer and nothing else.</param>
        /// <returns></returns>
        public async Task<string> DeletePaymentPlanAsync(string planId)
        {
            try
            {
                var planService = new StripePlanService();
                planService.Delete(planId);
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }


        /// <summary>
        /// Attempts to unsubscribe a customer from a payment plan.
        /// </summary>
        /// <param name="planId">The ID of the plan to unsubscribe from.</param>
        /// <param name="customerToken">The customer token, which represents the customer object to Stripe.</param>        /// <returns></returns>
        public async Task<string> UnsubscribeCustomerAsync(string customerToken, string planId)
        {
            try
            {
                var subscriptionService = new StripeSubscriptionService();
                string json = JsonConvert.SerializeObject(
                    await Task.Run(
                        () => subscriptionService.Cancel(customerToken, planId)).ConfigureAwait(false)); // optional cancelAtPeriodEnd flag
                return json;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }


        /// <summary>
        /// Attempts to unsubscribe the customer from an old payment plan, and then subscribe them to a new one.
        /// </summary>
        /// <param name="oldPlanId">The ID of the plan that you would like to unsubscribe the customer from.</param>
        /// <param name="newPlanId">The ID of the new plan you would like to subscribe the customer from.</param>
        /// <param name="customerToken">The token that represents the customer to Stripe.</param>
        /// <returns></returns>
        public async Task<bool> SwitchSubscriptionAsync(string oldPlanId, string newPlanId, string customerToken)
        {
            try
            {
                var result = await UnsubscribeCustomerAsync(customerToken, oldPlanId).ConfigureAwait(false);
                var result2 = await SubscribeCustomerAsync(customerToken, newPlanId).ConfigureAwait(false);
                if (result != null && result2 != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }

        }

        /// <summary>
        /// Attempts to delete a customer from Stripe.
        /// </summary>
        /// <param name="customerToken">The token that represents the customer to Stripe.</param>
        /// <returns></returns>
        public async Task<string> DeleteCustomerAsync(string customerToken)
        {
            try
            {
                var customerService = new StripeCustomerService();
                await Task.Run(
                    () => customerService.Delete(customerToken)).ConfigureAwait(false);
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// Attempts to create a new payment plan with which to subscribe users in the future.
        /// </summary>
        /// <param name="model">The MVC model to pass, with the parameters filled from user input, which is then mapped to the new plan that is created.</param>
        /// <returns></returns>
        public async Task<string> CreatePaymentPlanAsync(StripePlanCreateOptions model)
        {
            try
            {
                var planService = new StripePlanService();
                string json = JsonConvert.SerializeObject(
                    await Task.Run(
                        () => planService.Create(model)).ConfigureAwait(false));
                return json;
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }

        

    }
}
