﻿using Microsoft.AspNetCore.Mvc;
using System.Xml;

namespace Proje.ViewComponents
{
	public class Exchange : ViewComponent
	{
        public string Invoke()
        {
            string url = "https://www.tcmb.gov.tr/kurlar/today.xml";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(url);

            string dolar = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteSelling").InnerXml;

            string usdsatis = dolar.Substring(0, 5);
            return $"{usdsatis}";
        }
    }
}
