#define SPEEDUP

using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using TechTalk.SpecFlow;
using WebDriverWrapper.Properties;

namespace WebDriverWrapper.Utils
{
    public partial class WebDriverUtils
    {
        [DebuggerHidden]
        [DebuggerStepThrough]
        public void RepeatIfExeption(Action action)
        {
            bool keepNodeAlive = false;

            try
            {
                //Keep doing the action until the action was a success or the timeout is reached. Repeats if certain exceptions occured (declared in the Wait constructor)
                Wait.Until((d) =>
                {
                    action.Invoke();

                    return true; //will reach here if no exception was thrown and leave the loop due to returning true
                });
            }
            catch (Exception ex)
            {
                keepNodeAlive = true;

                AddLastKnownSelectorToException(ex);

                throw;
            }
            finally
            {
                if (keepNodeAlive)
                    KeepNodeConnectionOpen();
            }
        }

        [DebuggerHidden]
        [DebuggerStepThrough]
        public TResult RepeatIfExeptionNullFalse<TResult>(Func<IWebDriver, TResult> action)
        {
            bool keepNodeAlive = false;

            try
            {
                //Keep doing the action until the action was a success or the timeout is reached. Repeats if certain exceptions occured (declared in the Wait constructor)
                return Wait.Until((d) =>
                {
                    var result = action(Driver);

                    return result; //will reach here if no exception was thrown and will leave the loop if (not false or not null)
                });
            }
            catch (Exception ex)
            {
                keepNodeAlive = true;

                AddLastKnownSelectorToException(ex);

                throw;
            }
            finally
            {
                if (keepNodeAlive)
                    KeepNodeConnectionOpen();
            }
        }

        [DebuggerHidden]
        [DebuggerStepThrough]
        public TResult RepeatIfExeptionNullFalse<TResult>(Func<TResult> action)
        {
            bool keepNodeAlive = false;

            try
            {
                //Keep doing the action until the action was a success or the timeout is reached. Repeats if certain exceptions occured (declared in the Wait constructor)
                return Wait.Until((d) =>
                {
                    var result = action();

                    return result; //will reach here if no exception was thrown and will leave the loop if (not false or not null)
                });
            }
            catch (Exception ex)
            {
                keepNodeAlive = true;

                AddLastKnownSelectorToException(ex);

                throw;
            }
            finally
            {
                if (keepNodeAlive)
                    KeepNodeConnectionOpen();
            }
        }

        private void AddLastKnownSelectorToException(Exception ex)
        {
            string lastknownSelector = $"Last known selector: {CurrentSelector}";

            if (ex is WebDriverException)
                ex.HelpLink = lastknownSelector;
        }

        public void UsingExtendedWait(Action p_DoAction)
        {
            UsingSpecificWait(p_DoAction, SettingsWebDriver.Default.ExtendedTimeout);
        }

        public T UsingExtendedWait<T>(Func<T> p_DoAction)
        {
            return UsingSpecificWait(p_DoAction, SettingsWebDriver.Default.ExtendedTimeout);
        }

        public T UsingSpecificWait<T>(Func<T> p_DoAction, double p_Second)
        {
            double originalTimeout = Wait.Timeout.TotalSeconds;

            try
            {
                SetWebDriverTimeout(p_Second);
                return p_DoAction();
            }
            finally
            {
                //Reset
                SetWebDriverTimeout(originalTimeout);
            }
        }

        public void UsingSpecificWait(Action p_DoAction, double p_Second)
        {
            double originalTimeout = Wait.Timeout.TotalSeconds;

            try
            {
                SetWebDriverTimeout(p_Second);
                p_DoAction();
            }
            finally
            {
                //Reset
                SetWebDriverTimeout(originalTimeout);
            }
        }

        public void WaitForAnyPageActivityToFinish()
        {
        }

        private List<IWebElement> list = new List<IWebElement>();

        public void KeepNodeConnectionOpen()
        {
            //Keeps the node session from shutting permaturely (when using selenium grid)

            try
            {
                string pageSource = Driver.PageSource;
            }
            catch (Exception) { }
            {
            }
        }

        public void TakeScreenshot(string desiredLocationAndFilenameOfScreenshot)
        {
            try
            {
                //Zoom out and take screenshot
                ((IJavaScriptExecutor)Driver).ExecuteScript("document.body.style.zoom = '50%'");
                ((ITakesScreenshot)Driver).GetScreenshot().SaveAsFile(desiredLocationAndFilenameOfScreenshot, ScreenshotImageFormat.Png);
            }
            catch (Exception)
            {
            }
            finally
            {
                ((IJavaScriptExecutor)Driver).ExecuteScript("document.body.style.zoom = '100%'");
            }
        }

        public static void WakeupServer(string url, int waitMin, bool isHeadless = true)
        {
            Stopwatch watch = Stopwatch.StartNew();

            IWebDriver driver = null;

            do
            {
                try
                {
                    WebDriverFactory driverFactory = new WebDriverFactory();
                    driver = driverFactory.NewDriver(isHeadless);
                    driver.Url = url;

                    break;
                }
                catch (WebDriverException) { }
                finally
                {
                    driver?.Close();
                }
            } while (watch.Elapsed.Minutes < waitMin);
        }

        public IWebElement ScrollToElement(By p_Selector, bool UsingJavaSCript = false, int p_Delay = 500)
        {
            return RepeatIfExeptionNullFalse(() =>
            {
                CurrentElement = GetElement(p_Selector);

                ScrollToElement(CurrentElement, UsingJavaSCript, p_Delay);

                if (UsingJavaSCript == false)
                    WaitForElement(p_Selector, WaitType.Clickable);

                return CurrentElement;
            });
        }

        public void ScrollToElement(IWebElement p_Element, bool UsingJavaSCript = false, int p_Delay = 500)
        {
            if (UsingJavaSCript)
            {
                IJavaScriptExecutor executor = (IJavaScriptExecutor)Driver;
                executor.ExecuteScript("arguments[0].scrollIntoView(true);", p_Element);
            }
            else
            {
                Actions actions = new Actions(Driver);
                actions.MoveToElement(p_Element).Perform();
            }

            Thread.Sleep(p_Delay);
        }

        private static readonly object lockObj = new object();

        public static void DoOutput(string p_Output, ConsoleColor p_Colour = ConsoleColor.Gray)
        {
            lock (lockObj)
            {
                ConsoleColor beforeColour = Console.ForegroundColor;
                Console.ForegroundColor = p_Colour;
                Console.WriteLine(p_Output);
                Console.ForegroundColor = beforeColour;
            }
        }

        public static string RemoveCharacters(string p_input, string p_Pattern)
        {
            //string p_Pattern = "[£,]";
            string p_Replacement = "";
            Regex regEx = new Regex(p_Pattern);

            return regEx.Replace(p_input, p_Replacement);
        }

        public void SelectByText(By p_ElementSel, string p_OptionText)
        {
            //Trimming is important. If the UI option text has spaces on the end then SelectByText wont work even if we get the text exact (strange).
            //However if we SelectByText using a trimmed string, then it works..

            //Selenium has a problem with texts that have double spaces in.
            p_OptionText = p_OptionText.Replace("  ", " ").Trim();

            bool blnIsRetrying = false;
            do
            {
                try
                {
                    if (GetAllOptionTexts(p_ElementSel).Select(t => t.Replace("  ", " ").Trim()).Where(t => t == p_OptionText).Any())
                    {
                        SelectElement select = GetSelect(p_ElementSel);
                        select.SelectByText(p_OptionText);
                    }
                    else
                        throw new Exception($"Select option by text: {p_OptionText} - not found");
                }
                catch (StaleElementReferenceException)
                {
                    //  Retry stale elements once
                    if (blnIsRetrying)
                    {
                        throw;
                    }
                    else
                    {
                        blnIsRetrying = true;
                    }
                }
                finally
                {
                    WaitForAnyPageActivityToFinish();
                }
            } while (blnIsRetrying);
        }

        public void SelectByValue(By p_ElementSel, string p_OptionValue, By p_SelectorWaitFor = null)
        {
            try
            {
                SelectElement select = GetSelect(p_ElementSel);
                select.SelectByValue(p_OptionValue);
            }
            //So that we can see the p_Selector values in the Exception output to make debugging alot easier.
            catch (Exception)
            {
                throw;
            }

            //specify for any adhoc selectors to wait for after selection complete
            if (p_SelectorWaitFor != null)
                WaitForElement(p_SelectorWaitFor);
        }

        public string SelectByRandom(By p_ElementSel, int p_Min = 0, By p_SelectorWaitFor = null)
        {
            string randomSelection;

            try
            {
                SelectElement select = GetSelect(p_ElementSel);
                IEnumerable<string> we = GetAllOptionTexts(p_ElementSel);

                randomSelection = we.ElementAt(new Random().Next(p_Min, we.Count()));

                select.SelectByText(randomSelection);
            }
            //So that we can see the p_Selector values in the Exception output to make debugging alot easier.
            catch (Exception)
            {
                throw;
            }

            //specify for any adhoc selectors to wait for after selection complete
            if (p_SelectorWaitFor != null)
                WaitForElement(p_SelectorWaitFor);

            return randomSelection;
        }

        public int SelectCount(By p_ElementSel)
        {
            try
            {
                SelectElement select = GetSelect(p_ElementSel);
                IEnumerable<string> we = GetAllOptionTexts(p_ElementSel);

                return we.Count();
            }
            //So that we can see the p_Selector values in the Exception output to make debugging alot easier.
            catch (Exception)
            {
                throw;
            }
        }

        public void SwitchToParentFrame()
        {
            Driver.SwitchTo().DefaultContent();
            Thread.Sleep(1000);
        }

        public void SwitchToIFrame(By p_Selector = null)
        {
            Driver.SwitchTo().Frame(p_Selector == null ? GetElement(By.CssSelector("iframe")) : GetElement(p_Selector));
            Thread.Sleep(1000);
        }

        public void SwitchToActiveElement()
        {
            Driver.SwitchTo().ActiveElement();
            Thread.Sleep(1000);
        }

        public void InteractWithIframe(Action action)
        {
            SwitchToIFrame();

            action();

            SwitchToParentFrame();
        }

        public void WaitForAllOtherWindowsToClose()
        {
            //FYI doesnt work in headless
            UsingExtendedWait(() => RepeatIfExeptionNullFalse(() => Driver.WindowHandles.Count == 1));
        }

        public void CheckPageError(By genericErrorObject)
        {
            //========================================
            //Detect if application crashed
            //========================================

            if (Driver == null)
                return;

            if (Driver.FindElements(genericErrorObject).Where(e => IsElementVisible(e)).Any())
                throw new Exception($"Generic error detected on page");
        }

        public object GetSpecFlowScenarioKey(ScenarioContext p_ScenarioContext, string p_Key)
        {
            if (p_ScenarioContext == null)
                return null;

            var scenario = p_ScenarioContext["Scenario"]; //Main key object which stores all dynamic SpecFlow data objects. Scenario key name not expected to change.

            return scenario?.GetType().GetProperty(p_Key)?.GetValue(scenario, null); //return specific key object within Scenario
        }

        private TimeSpan Accumulative { get; set; } = new TimeSpan();

        //Example usage, measuring method:
        //GetSpeed(() => ClickElement(By.CssSelector(LabelFor(p_ForName)));
        public void GetSpeed(Action action)
        {
            Stopwatch timer = null;

            try
            {
                timer = Stopwatch.StartNew();
                action();
            }
            finally
            {
                timer.Stop();

                Accumulative = Accumulative.Add(timer.Elapsed);

                DoOutput($"Current:{timer.Elapsed.ToString("mm':'ss':'fff")}  Accumulative:{Accumulative.Duration().ToString("mm':'ss':'fff")}", ConsoleColor.Magenta);
            }
        }

        private void Debug_ShowHighlighter(IWebElement p_Element)
        {
#if DEBUG
            try
            {
                if (SettingsWebDriver.Default.Debug_ShowHighlighter == false || p_Element == null || p_Element.Displayed == false)
                    return;

                IJavaScriptExecutor jsDriver = (IJavaScriptExecutor)Driver;

                string highlightJavascript = @"$(arguments[0]).css({ ""border-width"" : ""2px"", ""border-style"" : ""solid"", ""border-color"" : ""blue"" });";
                jsDriver.ExecuteScript(highlightJavascript, new object[] { p_Element });
            }
            catch
            {
                //Not important if this fails, but we dont want it impacting the test}
            }
#endif
        }
    }
}