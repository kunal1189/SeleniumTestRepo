using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace seleniumtest
{
    class CustomData
    {
        public long CreationTime;
        public int Name;
        public int ThreadNum;
        public string url;
        public bool Result { get; internal set; }
    }

    public class SampleTest
    {
       
            

       
        public void test()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Url = "https://egov.uscis.gov/casestatus/mycasestatus.do";
            IWebElement casenumber = driver.FindElement(By.Id("receipt_number"));
            IWebElement submitnutton=driver.FindElement(By.Id("caseStatusSearchBtn"));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            driver.FindElement(By.XPath("/html/body/div[4]/div[3]/div/button/span")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            casenumber.SendKeys("WAC2028550812");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            submitnutton.Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            var status= driver.FindElement(By.XPath("/html/body/div[2]/form/div/div[1]/div/div/div[2]/div[3]/h1")).Text;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(status);
            sendEmail(status);
            Console.ResetColor();
            driver.Close();
            driver.Dispose();            
            Thread.Sleep(3000);
        }

        public void test2()
        {

            var j = 0;

            var urls = new List<string>();
            var configurationurls = System.Configuration.ConfigurationManager.AppSettings["urls"];

            var urlarray= configurationurls.Split(',');

            Dictionary<string, string> users = new Dictionary<string, string>() {
                {"roy5.kumar5.111189@gmail.com","dav22977" }
                //{ "kunal.k1111.drive@gmail.com","dav22977"}
            };



            Task[] taskArray = new Task[urlarray.Length];


            for (int i = 0; i <= urlarray.Length-1; i++)
            {
                
                taskArray[i] = Task.Factory.StartNew((object obj ) =>
                {
                     
                    
                    CustomData data = obj as CustomData;
                    if (data == null) return;
                    bool result = LikeAndSubscribe(j, data.url);
                    data.ThreadNum = Thread.CurrentThread.ManagedThreadId;
                    data.Result = result;
                },
                new CustomData() { Name = i, CreationTime = DateTime.Now.Ticks,url= urlarray[i].ToString() });
            }

            Task.WaitAll(taskArray);
            foreach (var task in taskArray)
            {
                var data = task.AsyncState as CustomData;
                if (data != null)
                {
                    Console.WriteLine("Task #{0} created at {1}, ran on thread #{2}.",
                                      data.Name, data.CreationTime, data.ThreadNum);
                //if data result is false then trigger notification for failing tasks.
                
                }
            }
        }


        private bool LikeAndSubscribe(int interation, string url, string user = null , string pass = null)
        {
            Console.WriteLine("interation:" + interation.ToString());
            Thread.Sleep((interation % 10) * 2000);

            ChromeOptions options = new ChromeOptions();
            bool headless = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["headless"]);

            if (headless)
            options.AddArguments("--incognito", "--headless");
            else
            options.AddArguments("--incognito");

            
            IWebDriver driver = new ChromeDriver(options);
            driver.Url = url;


            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);

            try
            {

                try
                {
                    //check if video or advertisement is loaded?
                    //if add is loaded skip it
                    // check if something is playing or not. 
                    // if not then play it.
                    driver.FindElement(By.CssSelector(".ytp-large-play-button.ytp-button")).Click();

                    //*[@id="movie_player"]/div[5]/button
                    //skip add and play 
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                    //click skip.
                    driver.FindElement(By.XPath("/html/body/ytd-app/div/ytd-page-manager/ytd-watch-flexy/div[4]/div[1]/div/div[1]/div/div/div/ytd-player/div/div/div[4]/div/div[3]/div/div[2]/span/button")).Click();

                }
                catch (Exception ex)
                {

                }
                goto aa;
                //finally {
                //    driver.FindElement(By.XPath("//*[@id=\"movie_player\"]/div[4]/button")).Click();
                //}
                //click on like
                //then signin
                //future plan
                //then create new account
                // click on like again 
                //click on subscribe button
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

                try
                {
                    //like and signin
                    //like

                    driver.FindElement(By.CssSelector("ytd-toggle-button-renderer a")).Click();

                    //driver.FindElement(By.XPath("/html/body/ytd-app/div/ytd-page-manager/ytd-watch-flexy/div[4]/div[1]/div/div[7]/div[2]/ytd-video-primary-info-renderer/div/div/div[3]/div/ytd-menu-renderer/div/ytd-toggle-button-renderer[1]/a")).Click();
                    //click signin
                    driver.FindElement(By.CssSelector("ytd-modal-with-title-and-button-renderer ytd-button-renderer a.ytd-button-renderer")).Click();

                    //driver.FindElement(By.XPath("/html/body/ytd-app/ytd-popup-container/iron-dropdown[2]/div/ytd-modal-with-title-and-button-renderer/div/ytd-button-renderer/a")).Click();
                    //create account
                    //driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div[2]/div/div[2]/div/div/div[2]/div/div[2]/div/div[2]/div/div")).Click();
                    driver.FindElement(By.CssSelector("div[role] span[jsslot] span")).Click();

                    //need some time to dropdown to load
                    Thread.Sleep(2000);
                    //for mysellf
                    var dropdownvalues = driver.FindElements(By.CssSelector("div[jscontroller] div[jsaction] span[jsslot][jsaction] div div"));
                    dropdownvalues[0].Click();


                }
                catch (Exception ex)
                {

                }
                try
                {
                    Thread.Sleep(2000);

                    var createNewgmailButton = driver.FindElements(By.CssSelector("button"));
                    foreach (var button in createNewgmailButton)
                    {
                        if (button.Text.Contains("Create a new Gmail address instead"))
                        {
                            button.Click();
                            Thread.Sleep(500);
                        }
                    }
                    var elements = driver.FindElements(By.CssSelector("div[jscontroller][role] div[jsshadow]"));


                    foreach (var elemt in elements)
                    {
                        if (elemt.Text.Contains("First name"))
                        {
                            elemt.FindElement(By.TagName("input")).SendKeys("Roy");
                        }
                        else if (elemt.Text.Contains("Last name"))
                        {
                            elemt.FindElement(By.TagName("input")).SendKeys("Kumar");
                        }
                        else if (elemt.Text.Contains("Username"))
                        {
                            elemt.FindElement(By.TagName("input")).SendKeys(user);
                        }
                        else if (elemt.Text.Contains("Password"))
                        {
                            elemt.FindElement(By.TagName("input")).SendKeys(pass);
                        }
                        else if (elemt.Text.Contains("Confirm"))
                        {
                            elemt.FindElement(By.TagName("input")).SendKeys(pass);
                        }
                    }

                    driver.FindElement(By.Id("accountDetailsNext")).Click();

                    //Enter Mobile number
                    driver.FindElement(By.Id("phoneNumberId")).SendKeys("4847959382");
                    driver.FindElement(By.TagName("button")).Click();
                }
                catch (Exception ex)
                {

                }

                {
                    //Verify mobile- Manual
                }


                //enter DOB
                driver.FindElement(By.Id("month")).SendKeys("November");
                driver.FindElement(By.Id("day")).SendKeys("11");
                driver.FindElement(By.Id("year")).SendKeys("1989");
                driver.FindElement(By.Id("gender")).SendKeys("Male");
                driver.FindElement(By.TagName("button")).Click();
                //other google service skip or i am in 
                var buttons = driver.FindElements(By.CssSelector("div[jscontroller][jsaction] button[jscontroller][jsaction]"));
                foreach (var button in buttons)
                {
                    if (button.Text.Contains("Skip"))
                    {
                        button.Click();
                    }
                }
                //agree term and conditions
                driver.FindElement(By.Id("termsofserviceNext")).Click();


            //Roy4sharma4.111189@gmail.com
            //Roy3.sharma2.111189@gmail.com

            aa:
                Thread.Sleep(int.Parse(System.Configuration.ConfigurationManager.AppSettings["videowatchTime"]));
            }
            catch (Exception e)
            {
                return false;//log error
            }


            driver.Close();
            driver.Dispose();

            return true;

        }






        private void sendEmail(string status)
        {
            var fromAddress = new MailAddress("kunal.k1111.drive@gmail.com", "Kunal Kumar");
            var toAddress = new MailAddress("kunal.k1111@gmail.com", "Kunal Kumar");
            const string fromPassword = "dav22977";
            string subject = "Case Status : " + status;
            string body = "status :" +status;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

       
    }
}
