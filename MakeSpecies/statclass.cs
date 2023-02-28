using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DotNetWikiBot;
using System.Threading;

namespace MakeSpecies
{
    public class statclass
    {
        public List<int> sizelist = new List<int>();
        public int nart = 0;
        public int nredirect = 0;
        public int ncat = 0;
        public int nbot = 0;
        public int ntalk = 0;
        public int milestone = 0;
        public int milestone_interval = 1000;
        public bool skipmilestone = false;
        public int ntowait = 0;
        public int nwaited = 0;

        public int ArticleCount(Site countsite)
        {


            //string xmlSrc = countsite.PostDataAndGetResultHTM(countsite.site + "/w/api.php", "action=query&format=xml&meta=siteinfo&siprop=statistics");
            string xmlSrc = countsite.PostDataAndGetResult(countsite.address + "/w/api.php", "action=query&format=xml&meta=siteinfo&siprop=statistics");

            //Console.WriteLine(xmlSrc);


            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlSrc);
            string ts = doc.GetElementsByTagName("statistics")[0].Attributes.GetNamedItem("articles").Value;

            Console.WriteLine("ts = " + ts);

            return Convert.ToInt32(ts);

        }

        public void SetMilestone(int mint, Site countsite)
        {
            milestone_interval = mint;

            int ac = ArticleCount(countsite);

            milestone = ((ac / milestone_interval) + 1) * milestone_interval;

            Console.WriteLine("Articlecount = " + ac.ToString() + ", milestone = " + milestone.ToString());

            if ((milestone - ac) > 100)
                ntowait = (milestone - ac) / 2;
            else
                ntowait = 0;

            nwaited = 0;

        }

        public void Add(string title, string text, Page pstats)
        {
            if (title.Contains(MakeSpecies.mp(1, null)))
                ncat++;
            else if (title.Contains(MakeSpecies.botname))
                nbot++;
            else if (text.Contains(MakeSpecies.mp(2, null)))
                nredirect++;
            else if (title.Contains(MakeSpecies.mp(38, null)))
                ntalk++;
            else if (!title.Contains(":"))
            {
                nart++;
                sizelist.Add(text.Length);

                nwaited++;
                if (nwaited >= ntowait)
                {
                    int ac = ArticleCount(MakeSpecies.makesite);

                    if (ac >= milestone)
                    {
                        Console.WriteLine("Milestone reached: ac = " + ac.ToString());
                        SetMilestone(milestone_interval, MakeSpecies.makesite);
                        pstats.text += "\n\nMilstolpe: artikel #" + ac.ToString() + " är [[" + title + "]]\n\n";
                        util.trysave(pstats, 1, "Updating statistics");
                    }

                    Console.WriteLine("Articlecount = " + ac.ToString() + ", milestone = " + milestone.ToString());

                    if ((milestone - ac) > 10)
                        ntowait = (milestone - ac) / 2;
                    else if (!skipmilestone || ((milestone - ac) > 1))
                        ntowait = 0;
                    else
                    {
                        while (ac < milestone)
                        {
                            Console.WriteLine("Waiting for milestone...");
                            Thread.Sleep(60000);//milliseconds
                            ac = ArticleCount(MakeSpecies.makesite);
                        }
                        ntowait = 0;
                    }

                    nwaited = 0;
                }
            }
        }

        public string GetStat()
        {
            string s = "* Antal artiklar: " + nart.ToString() + "\n";

            //int sum = 0;

            SortedDictionary<int, int> hist = new SortedDictionary<int, int>();

            int isum = 0;
            int mean = 0;
            foreach (int i in sizelist)
            {
                isum += i;
                if (hist.ContainsKey(i))
                    hist[i]++;
                else
                    hist.Add(i, 1);
            }

            if (nart > 0)
                mean = isum / nart;
            else
            {
                return s;
            }

            int icum = 0;
            int median = 0;
            foreach (int i in hist.Keys)
            {
                icum += hist[i];
                if (icum >= (nart / 2))
                {
                    median = i;
                    break;
                }
            }

            s += "** Medellängd: " + mean.ToString() + " bytes\n";
            s += "** Medianlängd: " + median.ToString() + " bytes\n";

            s += "* Antal kategorier: " + ncat.ToString() + "\n";
            s += "* Antal omdirigeringar: " + nredirect.ToString() + "\n";
            s += "* Antal diskussionssidor: " + ntalk.ToString() + "\n";
            s += "* Antal anomalier: " + nbot.ToString() + "\n";
            s += "* Totalt antal artiklar på " + MakeSpecies.makelang + "wiki: " + ArticleCount(MakeSpecies.makesite) + "\n";
            s += "\n";

            return s;
        }

    }
}
