using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DotNetWikiBot;
using System.Text.RegularExpressions;

namespace MakeSpecies
{
    public partial class CebEng : Form
    {
        Site site = null;
        public CebEng()
        {
            InitializeComponent();
        }

        public static Site login()
        {
            return login("en");
        }

        public static Site login(string makelang)
        {

            //Console.Write("Password: ");
            string password = util.get_password();
            string botkonto = "Lsjbot";
            Site newsite = new Site("https://" + makelang + ".wikipedia.org", botkonto, password);
            newsite.defaultEditComment = "Fixing mistake";
            newsite.minorEditByDefault = true;
            MakeSpecies.loggedin = true;
            return newsite;
        }

        public void memo(string s)
        {
            richTextBox1.AppendText(s + "\n");
            richTextBox1.ScrollToCaret();
        }



        private void Gobutton_Click(object sender, EventArgs e)
        {
            site = login();

            string fn = @"i:\dotnwb3\cebuano species names.txt";

            using (StreamReader sr = new StreamReader(fn))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] words = line.Split('\t');
                    string cebname = words[1];
                    string latinname = "***";
                    if (words.Length < 4 || String.IsNullOrEmpty(words[3]))
                    {
                        if (words.Length < 3 || String.IsNullOrEmpty(words[2]))
                            continue;
                        Page p = new Page(site, words[2]);
                        util.tryload(p, 1);
                        if (p.Exists())
                        {
                            p.ResolveRedirect();
                            if (p.title != words[2])
                                latinname = p.title;
                            else
                                latinname += "\t" + p.title;
                        }
                    }
                    else
                        latinname = words[3];
                    memo("*"+cebname + "\t" + latinname + "[[:en:"+words[2]+"]]");
                }
            }


        }

        private void Quitbutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Distbutton_Click(object sender, EventArgs e)
        {
            site = login();

            string fn = @"i:\dotnwb3\distribution.txt";

            using (StreamReader sr = new StreamReader(fn))
            {
                string header = sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] words = line.Split('\t');
                    if (String.IsNullOrEmpty(words[0]))
                        continue;
                    string distname = words[0].Trim().Replace("[","").Replace("]","");
                    string eez = "Exclusive Economic Zone";
                    if (distname.EndsWith(eez))
                    {
                        distname = distname.Replace(eez,"").Trim();
                        eez = eez + " sa ";
                    }
                    else
                        eez = "";
                    string cebname = "***";
                    if ((words.Length > 3) && (!String.IsNullOrEmpty(words[3])))
                        cebname = words[3];
                    string enname = distname;
                    Page p = new Page(site, distname);
                    util.tryload(p, 1);
                    if (p.Exists())
                    {
                        p.ResolveRedirect();
                        List<string> iwlist = p.GetInterLanguageLinks();
                        foreach (string iw in iwlist)
                        {
                            if ( iw.StartsWith("ceb:"))
                            {
                                cebname = "[[" + iw.Split(':')[1] + "]]";
                            }
                        }
                        enname = "[["+enname + "]]";
                    }
                    memo(words[0] + "\t"+ words[1] + "\t" +enname + " " + eez + "\t" + eez + cebname);
                }
            }


        }

        private void Partofbutton_Click(object sender, EventArgs e)
        {
            if (distributionclass.maindistlist.Count == 0)
                distributionclass.read_distributionlinks(Form1.db);

            memo("maindistlist " + distributionclass.maindistlist.Count);

            Regex rex = new Regex(@"\{\{flag\|(.+?)\}\}");

            site = login("ceb");

            string fn = util.unusedfilename(@"I:\dotnwb3\distributionlinks.txt");
            memo(fn);
            using (StreamWriter sw = new StreamWriter(fn))
            {

                Page p = new Page("dumdum");
                foreach (distributionclass dc in distributionclass.maindistlist)
                {
                    string country = "";
                    p.text = dc.ceblink;
                    PageList pl = p.GetLinks();
                    if (pl.Count() > 0)
                    {
                        Page p1 = pl[0];
                        if (util.tryload(p1, 3) && p1.Exists())
                        {
                            //string country = p1.GetFirstTemplateParameter("flag", "1");
                            foreach (Match m in rex.Matches(p1.text))
                            {
                                country = "[["+m.Groups[1].Value+"]]";
                                memo(p1.title + "\tcountry = " + country);
                                break;
                            }
                        }
                    }
                    sw.WriteLine(dc.maindist + "\t\t\t" + dc.ceblink + "\t" + country);
                }
            }
        }
    }
}
