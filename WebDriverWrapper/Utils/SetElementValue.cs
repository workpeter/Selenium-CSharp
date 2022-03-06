#define SPEEDUP

using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Text;
using System.Threading;

namespace WebDriverWrapper.Utils
{
    public partial class WebDriverUtils
    {
        public void SendKeys(By p_Selector, string p_ToWrite, bool clearFirst = false)
        {
            RepeatIfExeption(() =>
            {
                CurrentElement = GetElement(p_Selector, WaitType.Clickable);

                if (clearFirst)
                    CurrentElement.Clear();

                CurrentElement.SendKeys(p_ToWrite);
            });
        }

        public void SendKeys(IWebElement p_IWebElement, string p_ToWrite, bool clearFirst = false)
        {
            CurrentElement = p_IWebElement;

            RepeatIfExeption(() =>
            {
                if (clearFirst)
                    p_IWebElement.Clear();

                p_IWebElement.SendKeys(p_ToWrite);
            });
        }

        public void SendKeys_OneAtAtTime(By p_Selector, string p_ToWrite, int waitTimePerChar = 300, bool clearFirst = false)
        {
            RepeatIfExeption(() =>
            {
                CurrentElement = GetElement(p_Selector, WaitType.Clickable);

                if (clearFirst)
                    CurrentElement.Clear();

                for (int i = 0; i < p_ToWrite.Length; i++)
                {
                    char c = p_ToWrite[i];
                    string s = new StringBuilder().Append(c).ToString();
                    CurrentElement.SendKeys(s);
                    Thread.Sleep(waitTimePerChar);
                }
            });
        }

        public void Clear(By p_Selector)
        {
            RepeatIfExeption(() => GetElement(p_Selector, WaitType.Clickable).Clear());
        }

        public void HighlightText(By p_Selector)
        {
            CurrentElement = GetElement(p_Selector, WaitType.Clickable);

            var text = GetElementText(CurrentElement);

            if (text.Length == 0)
            {
                text = GetElementValue(CurrentElement);
            }

            //ensure it has focus
            CurrentElement.Click();

            Actions actions = new Actions(Driver);

            // goto the start of input
            actions.SendKeys(Keys.Home).Build().Perform();
            int length = text.Length;

            actions.KeyDown(Keys.LeftShift);
            for (int i = 0; i < length; i++)
                actions.SendKeys(Keys.ArrowRight);

            actions.KeyUp(Keys.LeftShift);

            actions.Build().Perform();
        }

        public void SelectByIndex(By p_Selector, int p_Index)
        {
            RepeatIfExeption(() =>
            {
                SelectElement select = GetSelect(p_Selector);
                select.SelectByIndex(p_Index);
            });
        }
    }
}