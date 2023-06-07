using System;
using UnityEngine;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;

public class ImageGenerator : MonoBehaviour
{
    public string GetImageAboutText(string query)
    {
        var service = EdgeDriverService.CreateDefaultService(@"Assets/Packages", @"msedgedriver.exe");
        service.HideCommandPromptWindow = true;
        var options = new EdgeOptions();
        options.AddArgument("headless");
        var edge = new EdgeDriver(service, options);
        edge.Navigate().GoToUrl("https://lexica.art/?q=" + query.Replace(' ', '+'));

        var now = DateTime.Now;
        new WebDriverWait(edge, TimeSpan.FromSeconds(1.5)).Until(wd => (DateTime.Now - now) - TimeSpan.FromSeconds(1.5) > TimeSpan.Zero);

        var imageSrc = edge.FindElement(By.XPath("//body/div/div/div/div/div/div/div/a/img")).GetAttribute("src");
        edge.Quit();

        imageSrc = imageSrc.Replace("/md2/", "/full_jpg/");
        return imageSrc;
    }
}

//new WebDriverWait(edge, TimeSpan.FromSeconds(2)).Until(wd => (DateTime.Now - now) - TimeSpan.FromSeconds(2) > TimeSpan.Zero);

//var textArea = edge.FindElement(By.Id("main-search"));
//textArea.Clear();
//textArea.SendKeys(query);

//now = DateTime.Now;
//edge.FindElements(By.TagName("button"))[4].Click();