using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Stripe;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Stripe.Net.AddOn.Core;
using Stripe.Net.AddOn.Services;

namespace Stripe.Net.AddOn.Tests
{
    [TestClass]
    public class UnitTests
    {
        Account account = new Account();
        Services.Billing billing = new Services.Billing();
        Invoices invoices = new Invoices();
        [TestMethod]
        public async Task Single_Charge_Succeeds_With_Test_Customer()
        {
            try
            {
                string name = Guid.NewGuid().ToString();
                StripeSourceOptions testCustomer = new StripeSourceOptions
                {
                    AddressCountry = "USA",
                    AddressCity = "Seattle",
                    AddressState = "WA",
                    AddressLine1 = "6942 Ham Street",
                    AddressZip = "53132",
                    Cvc = "123",
                    ExpirationMonth = "01",
                    ExpirationYear = "2018",
                    Name = name,
                    Number = "4242424242424242",
                    Object = "card"
                };
                string customerToken = await account.CreateCustomerAsync(testCustomer).ConfigureAwait(false);
                string charge = await billing.ChargeCustomerAsync(12540, customerToken, "Test charge").ConfigureAwait(false);
                string result = JsonConvert.DeserializeObject<StripeCharge>(charge).Status;
                Assert.AreEqual(result, StripeAttributes.Succeeded);
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }



        [TestMethod]
        public async Task Check_If_Customer_Has_Invalid_Card_Number_On_Single_Charge()
        {
            try
            {
                string name = Guid.NewGuid().ToString();
                StripeSourceOptions testcustomer = new StripeSourceOptions
                {
                    AddressCountry = "USA",
                    AddressCity = "Seattle",
                    AddressState = "WA",
                    AddressLine1 = "6942 Ham Street",
                    AddressZip = "53132",
                    Cvc = "123",
                    ExpirationMonth = "01",
                    ExpirationYear = "2018",
                    Name = name,
                    Number = "123222",
                    Object = "card"
                };
                string customer = await account.CreateCustomerAsync(testcustomer).ConfigureAwait(false);
                Assert.IsTrue(Regex.IsMatch(customer, "This card number looks invalid"), customer);

            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }


        [TestMethod]
        public async Task Check_If_Customer_Has_Invalid_Card_Expiration_Date_On_Single_Charge()
        {
            try
            {
                string name = Guid.NewGuid().ToString();
                StripeSourceOptions testcustomer = new StripeSourceOptions
                {
                    AddressCountry = "USA",
                    AddressCity = "Seattle",
                    AddressState = "WA",
                    AddressLine1 = "6942 Ham Street",
                    AddressZip = "53132",
                    Cvc = "123",
                    ExpirationMonth = "01",
                    ExpirationYear = "2010",
                    Name = name,
                    Number = "4242424242424242",
                    Object = "card"
                };
                string customer = await account.CreateCustomerAsync(testcustomer).ConfigureAwait(false);
                Assert.IsTrue(Regex.IsMatch(customer, "expiration year is invalid"), customer);

            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }




        [TestMethod]
        public async Task Create_Payment_Plan_And_Then_Delete_The_Plan()
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                StripePlanCreateOptions testplan = new StripePlanCreateOptions
                {
                    Id = id,
                    Amount = 500,
                    Currency = StripeAttributes.Currency,
                    Interval = "month",
                    IntervalCount = 3,
                    Name = "My plan",
                    TrialPeriodDays = 30
                };
                string createdPlan = await account.CreatePaymentPlanAsync(testplan).ConfigureAwait(false);
                var planObject = JsonConvert.DeserializeObject<StripePlan>(createdPlan);
                Assert.AreEqual(planObject.Id, id); //Should verify whether it is an error or the plan object.
                string deletedPlan = await account.DeletePaymentPlanAsync(id).ConfigureAwait(false);
                Assert.IsNull(deletedPlan); //Should verify if it is an error or the plan object.
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

    }
}
