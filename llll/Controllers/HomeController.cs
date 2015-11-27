using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using System.IO;
using System.Numerics;
using System.Threading;
using System.Globalization;

namespace llll.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            HttpWebRequest h = (HttpWebRequest)WebRequest.Create("http://asymcryptwebservice.appspot.com/znp/serverKey");
            h.AllowAutoRedirect = false;
            CookieContainer cc = new CookieContainer();
            h.CookieContainer = cc;
            HttpWebResponse r = (HttpWebResponse)h.GetResponse();
            foreach (Cookie c in r.Cookies)
                cc.Add(c);
            
            Stream stream = r.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string Out = sr.ReadToEnd();
            sr.Close();


            string url = "http://asymcryptwebservice.appspot.com/znp/challenge";
            BigInteger t = 0, root = 0,y=0,n;
            int count = 0;

            n = BigInteger.Parse(getNumbetFromRequest(Out), NumberStyles.HexNumber);
            while (root == t) 
            {
                t = GenerateNumber(1000);

                y = BigInteger.ModPow(t, 2, n);
                string _root = sendY(url, "y=" + y.ToString("X"), cc);
                root = BigInteger.Parse(getNumbetFromRequest(_root), NumberStyles.HexNumber);

                count++;

            }
          
            
          
    

                BigInteger p = gcd(t+root,n);
                BigInteger q = n/p;
                BigInteger _n = p*q;


            ViewBag.Greeting0 = n.ToString("X");
            ViewBag.Greeting1 = t.ToString("X");
            ViewBag.Greeting2 = root.ToString("X");
            ViewBag.Greeting3 = _n.ToString("X");
            ViewBag.Greeting4 = p.ToString("X");
            ViewBag.Greeting5 = q.ToString("X");
            ViewBag.Greeting6 = count;
            ViewBag.Greeting7 = y.ToString("X"); ;
          
            return View();
        }

        private static BigInteger GenerateNumber(int binaryLength)
        {
            BigInteger result;
            string strResult = "";
            Random rand = new Random((int)DateTime.Now.Millisecond);
            int length = rand.Next(0, binaryLength + 1);

            for (int i = 0; i < length; i++)
                strResult = strResult + rand.Next(0, 2);
            
            result = strResult.Aggregate(new BigInteger(), (b, c) => b * 2 + c - '0');
            if (result == 0 || result == 1)
                return GenerateNumber(binaryLength);

            Thread.Sleep(100);
            return result;
        }

        private static string sendY(string url, string parameter, CookieContainer cc)
        {
            HttpWebRequest h = (HttpWebRequest)WebRequest.Create(url +"?"+ parameter);
            h.AllowAutoRedirect = false;
            h.CookieContainer = cc;
            HttpWebResponse r = (HttpWebResponse)h.GetResponse();


            Stream stream = r.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string Out = sr.ReadToEnd();
            sr.Close();

            return Out; 
        }

        private static string getNumbetFromRequest(string Out) 
        {
            int i = 0, j = 0;
            string n = "";

            while (j < 4)
            {
                if (j == 3 && Out[i] != '"')
                    n += Out[i];
                if (Out[i] == '"')
                    j++;
              i++;
            }

            return n;

        }

        public static BigInteger gcd(BigInteger a, BigInteger b)
        {
            while (b != 0)
                b = a % (a = b);
            return a;
        }
    }
}