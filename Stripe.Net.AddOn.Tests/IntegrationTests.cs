using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Stripe.Net.AddOn.Core;
using Stripe.Net.AddOn.Services;

namespace Stripe.Net.AddOn.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        private string PersistentToken = "";
        private string Plan_ID = "";
        Account account = new Account();
        Services.Billing billing = new Services.Billing();
        Invoices invoices = new Invoices();

        [TestMethod]
        public async Task Subscribe_Customer_With_Plan_With_Trial_Period()
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
                string planId = Guid.NewGuid().ToString(); // If you try to create two plans with the same ID, Stripe kicks back an error. Testing should avoid that.
                StripePlanCreateOptions testplan = new StripePlanCreateOptions
                {
                    Id = planId,
                    Amount = 500,
                    Currency = StripeAttributes.Currency,
                    Interval = "month",
                    IntervalCount = 3,
                    Name = "My plan",
                    TrialPeriodDays = 30
                };
                string plan = await account.CreatePaymentPlanAsync(testplan).ConfigureAwait(false);
                var planObject = JsonConvert.DeserializeObject<StripePlan>(plan);
                Assert.AreEqual(planObject.Id, planId, "Plan ID is not correct."); //Should verify whether it is an error or the plan object.
                var returnedPlan = JsonConvert.DeserializeObject<StripeSubscription>(await account.SubscribeCustomerAsync(customerToken, planObject.Id).ConfigureAwait(false));
                Assert.AreEqual(returnedPlan.Status, StripeAttributes.Trialing, "Plan status is not equal to the 'Trialing' attribute.");
                var customerRetrieval = JsonConvert.DeserializeObject<StripeCustomer>(await account.GetCustomerAsync(customerToken).ConfigureAwait(false));
                Assert.AreEqual(customerToken, customerRetrieval.Id, "Customer token does not match the original Token."); //This is just a way to check if the customer was created properly.
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }



        [TestMethod]
        public async Task Subscribe_Customer_With_Plan_Without_Trial_Period()
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
                string planId = Guid.NewGuid().ToString(); // If you try to create two plans with the same ID, Stripe kicks back an error. Testing should avoid that.
                StripePlanCreateOptions testplan = new StripePlanCreateOptions
                {
                    Id = planId,
                    Amount = 500,
                    Currency = StripeAttributes.Currency,
                    Interval = "month",
                    IntervalCount = 3,
                    Name = "My plan"
                };
                string plan = await account.CreatePaymentPlanAsync(testplan).ConfigureAwait(false);
                var planObject = JsonConvert.DeserializeObject<StripePlan>(plan);
                Assert.AreEqual(planObject.Id, planId, "Plan ID is not correct."); //Should verify whether it is an error or the plan object.
                var returnedPlan = JsonConvert.DeserializeObject<StripeSubscription>(await account.SubscribeCustomerAsync(customerToken, planObject.Id).ConfigureAwait(false));
                Assert.AreEqual(returnedPlan.Status, StripeAttributes.Active, "Plan status is not equal to the 'Active' attribute.");
                var customerRetrieval = JsonConvert.DeserializeObject<StripeCustomer>(await account.GetCustomerAsync(customerToken).ConfigureAwait(false));
                Assert.AreEqual(customerToken, customerRetrieval.Id, "Customer token does not match the original Token."); //This is just a way to check if the customer was created properly.
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }


        private async Task Create_Persistent_Customer()
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
                this.PersistentToken = customerToken;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private async Task Create_Persistent_Plan()
        {
            try
            {

                string id = Guid.NewGuid().ToString();
                StripePlanCreateOptions testPlan = new StripePlanCreateOptions
                {
                    Id = id,
                    Amount = 500,
                    Currency = StripeAttributes.Currency,
                    Interval = "month",
                    IntervalCount = 3,
                    Name = "My plan",
                    TrialPeriodDays = 30
                };
                string plan = await account.CreatePaymentPlanAsync(testPlan).ConfigureAwait(false);
                var planObject = JsonConvert.DeserializeObject<StripePlan>(plan);
                Assert.AreEqual(planObject.Id, id); //Should verify whether it is an error or the plan object.
                this.Plan_ID = JsonConvert.DeserializeObject<StripePlan>(plan).Id;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        private async Task Subscribe_Persistent_Customer_To_Persistent_Plan()
        {
            try
            {

                string JSONreturned = "";
                if (PersistentToken.Length > 0 && Plan_ID.Length > 0)
                {
                    var result = await account.SubscribeCustomerAsync(PersistentToken, Plan_ID).ConfigureAwait(false);
                }
                else
                {
                    Assert.Fail("The tokens were not initialized before this method ran.");
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }


        [TestMethod]
        public async Task Integrated_Persistent_Plan_Creation_And_Customer_Creation_And_Subscription()
        {
            try
            {
                await Create_Persistent_Customer().ConfigureAwait(false);
                await Create_Persistent_Plan().ConfigureAwait(false);
                await Subscribe_Persistent_Customer_To_Persistent_Plan().ConfigureAwait(false);
            }
            catch
            {
                Assert.Fail("Failed to create persistent test objects.");
            }
        }


        [TestMethod]
        public async Task Integrated_Test_For_Charging_Customer_And_Getting_Invoices()
        {
            //Get the customer's invoices
            //Print them to a file.


            await Create_Persistent_Customer().ConfigureAwait(false);
            await Create_Persistent_Plan().ConfigureAwait(false);
            await Subscribe_Persistent_Customer_To_Persistent_Plan().ConfigureAwait(false);
            //We're going to charge this customer three times, and add three items to their invoice for their payment plan.
            //Then we're going to get their complete invoice and output it.
            // You can, alternatively, print the invoice to a file.
            var chargeOne = await billing.ChargeCustomerAsync(578, PersistentToken, "Test charge 1").ConfigureAwait(false);
            string chargeOneResult = JsonConvert.DeserializeObject<StripeCharge>(chargeOne).Status;
            Assert.AreEqual(chargeOneResult, StripeAttributes.Succeeded);
            var chargeTwo = await billing.ChargeCustomerAsync(76, PersistentToken, "Test charge 2").ConfigureAwait(false);
            string chargeTwoResult = JsonConvert.DeserializeObject<StripeCharge>(chargeTwo).Status;
            Assert.AreEqual(chargeTwoResult, StripeAttributes.Succeeded);
            var chargeThree = await billing.ChargeCustomerAsync(59, PersistentToken, "Test charge 3").ConfigureAwait(false);
            string chargeThreeResult = JsonConvert.DeserializeObject<StripeCharge>(chargeThree).Status;
            Assert.AreEqual(chargeThreeResult, StripeAttributes.Succeeded);

            var itemOne = await billing.AddInvoiceItemAsync(PersistentToken, 89, "Test item 1").ConfigureAwait(false);
            var itemTwo = await billing.AddInvoiceItemAsync(PersistentToken, 89, "Test item 2").ConfigureAwait(false);
            var itemThree = await billing.AddInvoiceItemAsync(PersistentToken, 89, "Test item 3").ConfigureAwait(false);
            //At this point in development, the AddInvoiceItemAsync is not properly tested.
            //This integration test is the only test that uses that method, as of yet.

            List<StripeInvoice> listInvoice = (await invoices.GetInvoicesAsync(PersistentToken).ConfigureAwait(false)).ToList();
            List<StripeCharge> listCharge = (await invoices.GetChargesAsync(PersistentToken).ConfigureAwait(false)).ToList();
            List<StripeInvoiceLineItem> listItem = (await invoices.GetLineChargesAsync(PersistentToken).ConfigureAwait(false)).ToList();
            // Write the string to a file.
            try
            {
                string InvoiceJson = JsonConvert.SerializeObject(listInvoice, Formatting.Indented);
                string ChargeJson = JsonConvert.SerializeObject(listCharge, Formatting.Indented);
                string ItemJson = JsonConvert.SerializeObject(listItem, Formatting.Indented);
                Console.WriteLine(InvoiceJson);
                Console.WriteLine(ChargeJson);
                Console.WriteLine(ItemJson);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }



    }
}
