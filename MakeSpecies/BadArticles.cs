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


namespace MakeSpecies
{

    public partial class BadArticles : Form
    {
        COL2019 db;
        public BadArticles(COL2019 dbpar)
        {
            InitializeComponent();
            db = dbpar;
        }

        private void Quitbutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void memo(string s)
        {
            richTextBox1.AppendText(s + "\n");
            richTextBox1.ScrollToCaret();
        }

        private IEnumerable<Taxon> getqfromname(string tit)
        {
            string[] titsplit = tit.Split();
            IEnumerable<Taxon> q;
            if (titsplit.Length == 1)
            {
                q = from c in db.Taxon
                    where c.ScientificName == tit
                    select c;
            }
            else
            {
                q = from c in db.Taxon
                    where (c.Genus == titsplit[0] || c.GenericName == titsplit[0])
                    where c.SpecificEpithet == titsplit[1]
                    where c.TaxonRank == "species"
                    select c;
            }
            return q;
        }

        public List<string> read_trivname(string makelang)
        {
            List<string> ll = new List<string>();
            using (StreamReader sr = new StreamReader(MakeSpecies.dyntaxafolder + "trivname_" + makelang + ".csv"))
            {
                int n = 0;

                //String headline = "";
                //headline = sr.ReadLine();

                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();
                    memo(line);

                    string[] words = line.Split(';');
                    for (int jj = 1; jj < words.Length; jj++)
                    {
                        words[jj] = words[jj].Trim();
                        if (words[jj].Length < 2)
                            words[jj] = "";
                    }

                    memo("Length = " + words.Length.ToString());
                    if (words.Length >= 2)
                    {
                        string taxon = words[0];
                        if (String.IsNullOrEmpty(taxon))
                            continue;
                        string trivname = words[1];
                        if (String.IsNullOrEmpty(trivname))
                            continue;
                        ll.Add(trivname.ToLower());
                        n++;
                    }

                }

                memo("n (trivname) = " + n.ToString());

            }
            return ll;
        }

        private void dobadpagelist(PageList pl, string fn, bool resume, string resumename)
        {
            List<string> botlang = new List<string>() { "sv", "war", "tr", "azb", "nl", "vn" };
            List<string> trivlist = read_trivname("ceb");
            char[] trimchars = " .,()'#:;".ToCharArray();
            using (StreamWriter sw = new StreamWriter(fn))
            {
                foreach (Page p in pl)
                {
                    string tit = util.remove_disambig(p.title);
                    if (!resume && tit == resumename)
                        resume = true;
                    if (!resume)
                        continue;
                    if (trivlist.Contains(tit.ToLower()))
                    {
                        memo("trivname " + tit);
                        continue;
                    }
                    //if (!tit.StartsWith("O"))
                    //    continue;
                    IEnumerable<Taxon> q = getqfromname(tit);
                    bool trimtit = false;
                    if (tit != tit.Trim(trimchars))
                    {
                        trimtit = true;
                        tit = tit.Trim(trimchars);
                        q = getqfromname(tit);
                    }
                    if (q.Count() == 0)
                    {
                        //memo(tit + ": not found in db");
                        //sw.WriteLine(tit + "\tnot found in db");
                        bool goodload = util.tryload(p, 4);
                        if (goodload && p.Exists())
                        {
                            if (p.text.Contains("axobox"))
                            {
                                try
                                {
                                    List<string> iwlist = p.GetInterLanguageLinks();
                                    bool found = false;
                                    foreach (string s in iwlist)
                                    {
                                        string[] ss = s.Split(':');
                                        if (!botlang.Contains(ss[0]))
                                            found = true;
                                    }
                                    if (found)
                                    {
                                        //memo(" found");
                                        string newname = "";
                                        foreach (string s in iwlist)
                                        {
                                            //memo(s);
                                            string[] ss = s.Split(':');
                                            if (ss[1] != tit)
                                            {
                                                var qss = getqfromname(ss[1]);
                                                if (qss.Count() >= 1)
                                                {
                                                    newname = ss[1];
                                                    //memo(newname);
                                                    break;
                                                }
                                            }
                                        }
                                        if (!String.IsNullOrEmpty(newname))
                                        {
                                            sw.WriteLine(p.title + "\tmove to (iw)\t" + newname);
                                            memo(p.title + "\tmove to (iw)\t" + newname);
                                        }
                                        else
                                        {
                                            sw.WriteLine(p.title + "\tmanual check");
                                            memo(p.title + "\tmanual check");
                                        }
                                    }
                                    else
                                    {
                                        string[] titsplit = tit.Split();
                                        if (titsplit.Length == 2)
                                        {
                                            string family = p.GetFirstTemplateParameter("Taxobox", "familia");
                                            if (!String.IsNullOrEmpty(family))
                                            {
                                                var qf = from c in db.Taxon
                                                         where c.Family == family
                                                         where c.SpecificEpithet == titsplit[1]
                                                         select c;
                                                if (qf.Count() == 1)
                                                {
                                                    sw.WriteLine(p.title + "\tmove to (db)\t" + qf.First().Genus + " " + titsplit[1]);
                                                    memo(p.title + "\tmove to (db)\t" + qf.First().Genus + " " + titsplit[1]);
                                                }
                                                else
                                                {
                                                    sw.WriteLine(p.title + "\tdelete");
                                                    memo(p.title + "\tdelete1");
                                                }
                                            }
                                            else
                                            {
                                                sw.WriteLine(p.title + "\tdelete");
                                                memo(p.title + "\tdelete2");
                                            }
                                        }
                                        else
                                        {
                                            sw.WriteLine(p.title + "\tdelete");
                                            memo(p.title + "\tdelete3");
                                        }
                                    }
                                }
                                catch (Exception ee)
                                {
                                    memo(ee.Message);
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            sw.WriteLine(p.title + "\tfailed load");
                            memo(p.title + "\tfailed load");

                        }
                    }
                    else
                    {
                        var qacc = from c in q
                                   where (c.TaxonomicStatus == null) || (c.TaxonomicStatus == "accepted name")
                                   select c;
                        if (qacc.Count() == 0)
                        {
                            Taxon tt = q.First();
                            if (tt.AcceptedNameUsageID != null)
                            {
                                Taxon tmain = (from c in db.Taxon
                                               where c.TaxonID == tt.AcceptedNameUsageID
                                               select c).FirstOrDefault();
                                if (tmain != null)
                                {
                                    memo(tit + ": not accepted name. Synonym of " + util.dbtaxon_name(tmain));
                                    sw.WriteLine(p.title + "\tredirect\t" + util.dbtaxon_name(tmain,false));
                                }
                                else
                                {
                                    memo(tit + ":" + tt.TaxonomicStatus);
                                    sw.WriteLine(p.title + "\t" + tt.TaxonomicStatus + "\ttarget not found " + tt.AcceptedNameUsageID);
                                }
                            }
                            else
                            {
                                memo(tit + ":" + tt.TaxonomicStatus);
                                sw.WriteLine(p.title + "\t" + tt.TaxonomicStatus);
                            }
                        }
                        else if (qacc.Count() == 1)
                        {
                            if (trimtit)
                            {
                                memo(tit + "\ttrimtit");
                                sw.WriteLine(p.title + "\tmove to (trim)\t" + tit);
                            }
                            else
                                memo(tit + ": OK!");
                        }
                        else
                        {
                            memo(tit + ": multiply found");
                            sw.Write(p.title + "\tmultiple");
                            foreach (Taxon tt in qacc)
                            {
                                sw.Write("\t" + tit + " (" + tt.Kingdom + ")\t" + tt.TaxonID);
                            }
                            sw.WriteLine();
                        }
                    }
                }
            }

        }

        private void dobadcat(string cat, Site site)
        {
            memo(cat);
            PageList pl = new PageList(site);
            pl.FillFromCategoryTree(cat);
            if (pl.Count() == 0)
                return;

            bool resume = true;
            //if (cat == "Paghimo ni bot 2013-02")
            //    resume = false;

            string fn = util.unusedfilename(@"I:\dotnwb3\badarticle\bad-" + cat + "-.txt");


            dobadpagelist(pl,fn,resume,"");

            memo("=========== DONE " + cat + " ================");
        }



        private void CatCheckbutton_Click(object sender, EventArgs e)
        {
            Site site = util.login("ceb");

            //string cat = "Paghimo ni bot 2013-02";
            //dobadcat(cat, site);
            //cat = "Paghimo ni bot 2014-08";
            //dobadcat(cat, site);
            //cat = "Paghimo ni bot 2014-09";
            //dobadcat(cat, site);
            //cat = "Paghimo ni bot 2014-10";
            //dobadcat(cat, site);
            //string cat = "Apoidea";
            //dobadcat(cat, site);
            //for (int year = 2013; year <= 2014; year++)
            //    for (int month = 1; month <= 12; month++)
            //    {
            //        cat = "Paghimo ni bot " + year.ToString() + "-" + month.ToString("D2");
            //        dobadcat(cat, site);
            //    }

            PageList pl = new PageList(site);
            pl.Add(new Page("Liris aurulentus"));
            pl.Add(new Page("Philanthus ventrilabris"));
            pl.Add(new Page("Bembix americanus"));
            string fn = util.unusedfilename(@"I:\dotnwb3\badarticle\apoidea-reload-.txt");
            dobadpagelist(pl, fn,true,"");

        }

        private void Sortbutton_Click(object sender, EventArgs e)
        {
            string dir = @"I:\dotnwb3\badarticle\";
            Dictionary<string, StreamWriter> outdict = new Dictionary<string, StreamWriter>();
            Dictionary<string, Dictionary<string, int>> statdict = new Dictionary<string, Dictionary<string, int>>();
            statdict.Add("all", new Dictionary<string, int>());
            foreach (string fn in Directory.GetFiles(dir))
            {
                memo(fn);
                if (fn.Contains("badtype"))
                    continue;
                statdict.Add(fn, new Dictionary<string, int>());
                try
                {
                    using (StreamReader sr = new StreamReader(fn))
                    {
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            string[] words = line.Split('\t');
                            if (words.Length < 2)
                                continue;
                            if (!outdict.ContainsKey(words[1]))
                            {
                                outdict.Add(words[1], new StreamWriter(util.unusedfilename(dir + "badtype-" + words[1] + ".txt")));
                            }
                            outdict[words[1]].WriteLine(line);
                            if (!statdict["all"].ContainsKey(words[1]))
                                statdict["all"].Add(words[1], 1);
                            else
                                statdict["all"][words[1]]++;
                            if (!statdict[fn].ContainsKey(words[1]))
                                statdict[fn].Add(words[1], 1);
                            else
                                statdict[fn][words[1]]++;

                        }
                    }
                }
                catch (IOException ee)
                {
                    memo(ee.Message);
                    continue;
                }
            }
            foreach (string s in outdict.Keys)
                outdict[s].Close();
            string header = "";
            foreach (string badtype in statdict["all"].Keys)
            {
                header += "\t" + badtype;
            }
            memo(header);
            foreach (string fn in statdict.Keys)
            {
                string line = fn;
                foreach (string badtype in statdict["all"].Keys)
                {
                    if (statdict[fn].ContainsKey(badtype))
                        line += "\t" + statdict[fn][badtype].ToString();
                    else
                        line += "\t0";
                }
                memo(line);
            }
        }

        private void retrybutton_Click(object sender, EventArgs e)
        {
            Site site = util.login("ceb");

            string dir = @"I:\dotnwb3\badarticle\";
            foreach (string fn in Directory.GetFiles(dir))
            {
                memo(fn);
                if (!fn.Contains("badtype-failed"))
                    continue;
                PageList pl = new PageList(site);
                try
                {
                    using (StreamReader sr = new StreamReader(fn))
                    {
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            string[] words = line.Split('\t');
                            if (words.Length < 2)
                                continue;
                            pl.Add(new Page(words[0]));
                        }
                    }
                }
                catch (IOException ee)
                {
                    memo(ee.Message);
                    continue;
                }
                string fnout = util.unusedfilename(fn.Replace("badtype-failed", "bad-retry"));
                dobadpagelist(pl, fnout, true, "");

            }

        }

        private void towikibutton_Click(object sender, EventArgs e)
        {
            Site site = util.login("ceb");

            string dir = @"I:\dotnwb3\badarticle\";
            string wikiprefix = "Gumagamit:Lsjbot/COL2019-check/";
            string wikimain = "Gumagamit:Lsjbot/COL2019-check/Types of articles";
            Page pmain = new Page(site, wikimain);
            //util.tryload(pmain,1);
            foreach (string fn in Directory.GetFiles(dir))
            {
                memo(fn);
                if (!fn.Contains("badtype"))
                    continue;
                if (fn.Contains("badtype-failed"))
                    continue;
                PageList pl = new PageList(site);
                try
                {
                    using (StreamReader sr = new StreamReader(fn))
                    {
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            string[] words = line.Split('\t');
                            if (words.Length < 2)
                                continue;
                            Page pp = new Page(words[0]);
                            pp.text = "* [[" + words[0] + "]]";
                            if ( fn.Contains("move to") || fn.Contains("redirect"))
                            {
                                if (words.Length >= 3)
                                    pp.text += " ==> [[" + words[2]+"]]";
                            }
                            pl.Add(pp);
                        }
                    }
                }
                catch (IOException ee)
                {
                    memo(ee.Message);
                    continue;
                }
                //string fnout = util.unusedfilename(fn.Replace("badtype-failed", "bad-retry"));
                string wikipage = wikiprefix + fn.Replace(dir,"").Replace(".txt", "001").Replace("badtype-", "");
                pmain.text += "\n* [[" + wikipage + "]]";
                int npage = 1;
                int n = 0;
                Page p = new Page(site, wikipage);
                foreach (Page pp in pl)
                {
                    p.text += "\n"+pp.text;
                    n++;
                    //if (n > 10)
                    //    break;
                    if ( n > 1000)
                    {
                        n = 0;
                        util.trysave(p, 2, "COL2019 maintenance");
                        npage++;
                        p = new Page(site, wikipage.Replace("001", npage.ToString("D3")));
                        pmain.text += "\n* [[" + p.title + "]]";
                    }
                }
                util.trysave(p, 2, "COL2019 maintenance");
            }

            util.trysave(pmain, 2, "COL2019 maintenance");


        }

        private void Trivnamebutton_Click(object sender, EventArgs e)
        {
            Site site = util.login("ceb");

            string dir = @"I:\dotnwb3\badarticle\";
            foreach (string fn in Directory.GetFiles(dir))
            {
                memo(fn);
                if (!fn.Contains("badtype-move"))
                    continue;
                if (fn.Contains("badtype-failed"))
                    continue;
                PageList pl = new PageList(site);
                try
                {
                    using (StreamReader sr = new StreamReader(fn))
                    {
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            string[] words = line.Split('\t');
                            if (words.Length < 2)
                                continue;
                            Page pp = new Page(words[0]);
                            pp.text = "* [[" + words[0] + "]]";
                            pl.Add(pp);
                        }
                    }
                }
                catch (IOException ee)
                {
                    memo(ee.Message);
                    continue;
                }
                string fnout = util.unusedfilename(dir+"trivname_extract.txt");
                //int npage = 1;
                int n = 0;
                using (StreamWriter sw = new StreamWriter(fnout))
                {
                    foreach (Page pp in pl)
                    {
                        n++;
                        //if (n < 55000)
                        //    continue;
                        if (n % 100 == 0)
                            memo("n = " + n);
                        util.tryload(pp, 2);
                        if (!pp.Exists())
                            continue;
                        string name = pp.GetFirstTemplateParameter("Taxobox", "name").Trim('\'');
                        string binomial = pp.GetFirstTemplateParameter("Taxobox", "binomial");
                        if (name != binomial)
                        {
                            memo("name, binomial " + name + ", " + binomial);
                            sw.WriteLine(binomial + "\t" + name.ToLower());
                        }
                    }
                }
            }



        }
    }
}
