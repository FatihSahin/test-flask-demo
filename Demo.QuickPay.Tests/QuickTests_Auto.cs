﻿/****************************************************************************
*																		 	*
*	This class is auto generated by TestFlask CLI on 12.6.2018 10:42:20	    *
*	https://github.com/FatihSahin/test-flask                                *
*	Implement provider methods and step assertions inside another file.		*
*																		 	*
****************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TestFlask.Aspects.ApiClient;
using TestFlask.Aspects.Enums;
using TestFlask.Models.Context;
using TestFlask.Models.Entity;
using TestFlask.Aspects.Context;
using Demo.QuickPay.Data.Context;
using Demo.QuickPay.Biz.Models;

namespace Demo.QuickPay.Tests
{
    [TestClass]
    public partial class QuickTests
    {
        #region Conventional

        private static IEnumerable<Scenario> embeddedScenarios { get; set; }

        private JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
        };

        [ClassInitialize]
        public static void ClassSetUp(TestContext context)
        {
            embeddedScenarios = ReadEmbeddedScenarios();
            DoClassSetUp(context);
        }



        [ClassCleanup]
        public static void ClassTearDown()
        {
            embeddedScenarios = null;
            DoClassTearDown();
        }



        private static IEnumerable<Scenario> ReadEmbeddedScenarios()
        {
            string fileName = "QuickTests_Auto_Embed.txt";

            if (!File.Exists(fileName))
            {
                return null;
            }

            List<Scenario> embeddedScenarios = new List<Scenario>();

            string line;
            using (System.IO.StreamReader fileReader = new System.IO.StreamReader(fileName))
            {
                while ((line = fileReader.ReadLine()) != null)
                {
                    var json = TestFlask.Models.Utils.CompressUtil.DecompressString(line);
                    var scenario = JsonConvert.DeserializeObject<Scenario>(json);
                    embeddedScenarios.Add(scenario);
                }
            }

            return embeddedScenarios;
        }

        private void ProvideTestFlaskHttpContext(Step step, TestModes testMode)
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://tempuri.org", ""),
                new HttpResponse(new StringWriter())
                );

            var invocation = step.GetRootInvocation();

            // In order to by pass Platform not supported exception
            // http://bigjimindc.blogspot.com.tr/2007/07/ms-kb928365-aspnet-requestheadersadd.html
            AddHeaderToRequest(HttpContext.Current.Request, ContextKeys.ProjectKey, invocation.ProjectKey);
            AddHeaderToRequest(HttpContext.Current.Request, ContextKeys.ScenarioNo, invocation.ScenarioNo.ToString());
            AddHeaderToRequest(HttpContext.Current.Request, ContextKeys.StepNo, invocation.StepNo.ToString());
            AddHeaderToRequest(HttpContext.Current.Request, ContextKeys.TestMode, testMode.ToString());

            TestFlaskContext.LoadedStep = step;
        }

        private void AddHeaderToRequest(HttpRequest request, string key, string value)
        {
            NameValueCollection headers = request.Headers;

            Type t = headers.GetType();
            ArrayList item = new ArrayList();

            // Remove read-only limitation on headers
            t.InvokeMember("MakeReadWrite", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, null, headers, null);
            t.InvokeMember("InvalidateCachedArrays", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, null, headers, null);
            item.Add(value);
            t.InvokeMember("BaseAdd", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, null, headers, new object[] { key, item });
            t.InvokeMember("MakeReadOnly", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, null, headers, null);
        }

        private Step GetLoadedStep(long stepNo, bool isEmbedded)
        {
            Step step = null;

            if (isEmbedded)
            {
                step = embeddedScenarios?.SelectMany(sc => sc.Steps).SingleOrDefault(st => st.StepNo == stepNo);
            }

            if (step == null)
            {
                TestFlaskApi api = new TestFlaskApi();
                step = api.LoadStep(stepNo);
            }

            return step;
        }

        private void HandleAssertion(Invocation rootInvocation, object responseObject, Exception exception, Action stepAssertion)
        {
            if ((!rootInvocation.IsFaulted && exception == null) || (rootInvocation.IsFaulted && exception != null))
            {
                stepAssertion();
            }
            else if (exception != null)
            {
                string exceptionStr = JToken.Parse(JsonConvert.SerializeObject(exception, jsonSerializerSettings)).ToString(Formatting.Indented);
                Assert.Fail($"Expected proper response of type {rootInvocation.ResponseType} but got exception =>{Environment.NewLine}{exceptionStr}{Environment.NewLine}{GetExceptionStackOutput()}");
            }
            else
            {
                string responseStr = JToken.Parse(JsonConvert.SerializeObject(responseObject, jsonSerializerSettings)).ToString(Formatting.Indented);
                Assert.Fail($"Expected exception of type {rootInvocation.ExceptionType} but got response =>{Environment.NewLine}{responseStr}");
            }
        }

        private string GetExceptionStackOutput()
        {
            StringBuilder strBuilder = new StringBuilder();
            IEnumerable<Invocation> exceptionalInvocations = TestFlaskContext.InvocationStack.ExceptionStack;

            strBuilder.AppendLine("**** TestFlask Exception Stack Snapshot ****");
            foreach (var excInv in exceptionalInvocations)
            {
                strBuilder.AppendLine("\t**** Faulty Invocation ****");
                strBuilder.AppendLine($"\t\tMethod => {excInv.InvocationSignature}");
                strBuilder.AppendLine($"\t\tInvocation Mode => {excInv.InvocationMode}");
                if (!string.IsNullOrWhiteSpace(excInv.RequestDisplayInfo))
                {
                    strBuilder.AppendLine($"\t\tRequest Info => {excInv.RequestDisplayInfo}");
                }
                strBuilder.AppendLine($"\t\tRequest => ");
                strBuilder.AppendLine(JToken.Parse(excInv.Request).ToString(Formatting.Indented));
                strBuilder.AppendLine($"\t\tExceptionType => {excInv.ExceptionType}");
                strBuilder.AppendLine($"\t\tException => ");
                strBuilder.AppendLine(JToken.Parse(excInv.Exception).ToString(Formatting.Indented));
            }

            return strBuilder.ToString();
        }

        private Invocation PrepareStep(long stepNo, TestModes testMode, bool isEmbedded)
        {
            Step loadedStep = GetLoadedStep(stepNo, isEmbedded);
            var rootInvocation = loadedStep.GetRootInvocation();
            ProvideTestFlaskHttpContext(loadedStep, testMode);
            ProvideOperationContext(rootInvocation);
            return rootInvocation;
        }



        #endregion

        #region Scenario155_Debit_Account_Closed

        [TestMethod]
        [TestCategory("TestFlask")]
        public void Scenario155_Debit_Account_Closed()
        {
            Scenario155_Debit_Account_Closed_Step327_AutoStep20180611151612271();
        }

        private void Scenario155_Debit_Account_Closed_Step327_AutoStep20180611151612271()
        {
            var rootInvocation = PrepareStep(327, TestFlask.Aspects.Enums.TestModes.Assert, isEmbedded: false);
            var requestObject = JsonConvert.DeserializeObject<object[]>(rootInvocation.Request, jsonSerializerSettings).First() as Demo.QuickPay.Data.Context.Payment;

            //Set up additional behaviour for method args
            SetUp_Scenario155_Debit_Account_Closed_Step327_AutoStep20180611151612271(requestObject);

            Demo.QuickPay.Biz.Models.PaymentResult responseObject = null;
            Exception exception = null;

            try { responseObject = subjectPaymentController.TransferMoney(requestObject); }
            catch (Exception ex) { exception = ex; }

            HandleAssertion(rootInvocation, responseObject, exception,
                () => Assert_Scenario155_Debit_Account_Closed_Step327_AutoStep20180611151612271(responseObject, exception));
        }

        private void Assert_Scenario155_Debit_Account_Closed_Step327_AutoStep20180611151612271(PaymentResult responseObject, Exception exception)
        {
            Assert.AreEqual("DEBIT_ACC_CLOSED", responseObject.ErrorCode);
        }

        private void SetUp_Scenario155_Debit_Account_Closed_Step327_AutoStep20180611151612271(Payment requestObject)
        {
        }

        #endregion

        #region Scenario156_Credit_Account_Closed

        [TestMethod]
        [TestCategory("TestFlask")]
        public void Scenario156_Credit_Account_Closed()
        {
            Scenario156_Credit_Account_Closed_Step328_AutoStep20180611151612271();
        }

        private void Scenario156_Credit_Account_Closed_Step328_AutoStep20180611151612271()
        {
            var rootInvocation = PrepareStep(328, TestFlask.Aspects.Enums.TestModes.Assert, isEmbedded: false);
            var requestObject = JsonConvert.DeserializeObject<object[]>(rootInvocation.Request, jsonSerializerSettings).First() as Demo.QuickPay.Data.Context.Payment;

            //Set up additional behaviour for method args
            SetUp_Scenario156_Credit_Account_Closed_Step328_AutoStep20180611151612271(requestObject);

            Demo.QuickPay.Biz.Models.PaymentResult responseObject = null;
            Exception exception = null;

            try { responseObject = subjectPaymentController.TransferMoney(requestObject); }
            catch (Exception ex) { exception = ex; }

            HandleAssertion(rootInvocation, responseObject, exception,
                () => Assert_Scenario156_Credit_Account_Closed_Step328_AutoStep20180611151612271(responseObject, exception));
        }

        private void Assert_Scenario156_Credit_Account_Closed_Step328_AutoStep20180611151612271(PaymentResult responseObject, Exception exception)
        {
        }

        private void SetUp_Scenario156_Credit_Account_Closed_Step328_AutoStep20180611151612271(Payment requestObject)
        {
        }

        #endregion

        #region Scenario157_Successfulpayment

        [TestMethod]
        [TestCategory("TestFlask")]
        public void Scenario157_Successfulpayment()
        {
            Scenario157_Successfulpayment_Step329_AutoStep20180612102320220();
        }

        private void Scenario157_Successfulpayment_Step329_AutoStep20180612102320220()
        {
            var rootInvocation = PrepareStep(329, TestFlask.Aspects.Enums.TestModes.Assert, isEmbedded: false);
            var requestObject = JsonConvert.DeserializeObject<object[]>(rootInvocation.Request, jsonSerializerSettings).First() as Demo.QuickPay.Data.Context.Payment;

            //Set up additional behaviour for method args
            SetUp_Scenario157_Successfulpayment_Step329_AutoStep20180612102320220(requestObject);

            Demo.QuickPay.Biz.Models.PaymentResult responseObject = null;
            Exception exception = null;

            try { responseObject = subjectPaymentController.TransferMoney(requestObject); }
            catch (Exception ex) { exception = ex; }

            HandleAssertion(rootInvocation, responseObject, exception,
                () => Assert_Scenario157_Successfulpayment_Step329_AutoStep20180612102320220(responseObject, exception));
        }

        private void Assert_Scenario157_Successfulpayment_Step329_AutoStep20180612102320220(PaymentResult responseObject, Exception exception)
        {
            Assert.IsTrue(responseObject.IsSuccessful);
        }

        private void SetUp_Scenario157_Successfulpayment_Step329_AutoStep20180612102320220(Payment requestObject)
        {
        }

        #endregion

        private Demo.QuickPay.WebApi.Controllers.PaymentController subjectPaymentController;
    }
}