using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using DotNetWikiBot;


namespace MakeSpecies
{


    public partial class Form1 : Form
    {



        public static COL2019 db;
        static string connectionstring = "Data Source=DESKTOP-JOB29A9;Initial Catalog=\"COL2019\";Integrated Security=True";

        public Form1()
        {
            InitializeComponent();
            db = new COL2019(connectionstring);

        }

        public static statclass stats = new statclass();


        //==============================================================================


        private void Quitbutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CompareTreebutton_Click(object sender, EventArgs e)
        {
            MScomparetrees mct = new MScomparetrees(db);
            mct.Show();
        }

        public void memo(string s)
        {
            richTextBox1.AppendText(s + "\n");
            richTextBox1.ScrollToCaret();
        }


        private void langfixbutton_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> langdict = new Dictionary<string, string>();
            langdict.Add("Visayan", "Cebuano");
            langdict.Add("En", "English");
            langdict.Add("Eng", "English");
            langdict.Add("Jpn", "Japanese");
            langdict.Add("Jpn (Kanji)", "Japanese");
            langdict.Add("Jpn (Katakana)", "Japanese");
            langdict.Add("Spainsh", "Spanish");
            langdict.Add("Swe", "Swedish");

            foreach (string oldlang in langdict.Keys)
            {

                //                UPDATE Customers
                //SET ContactName = 'Alfred Schmidt', City = 'Frankfurt'
                //WHERE CustomerID = 1;

                //string insertStatement = "Insert into Employee values('Experts', 'Comment','alpha.beta@gmail.com')";
                string update = "UPDATE VernacularName SET Language = '" + langdict[oldlang] + "' where Language='" + oldlang + "';";

                memo(oldlang + ": " + update);

                db.ExecuteQuery<VernacularName>(update);

                //(from c in db.VernacularName where c.Language == oldlang select c).ToList().ForEach(x => x.Language = langdict[oldlang]);
                //memo(oldlang + ": " + q.Count());
                //int i = 0;
                //foreach (VernacularName c in q)
                //{
                //    memo(c.Language);
                //    c.Language = langdict[oldlang];
                //    memo(c.Language);
                //    i++;
                //    if (i > 10)
                //        break;
                //}
                //var q1 = (from c in q where c.Language == oldlang select c).ToList();
                //memo(oldlang + " (after): " + q.Count());
                //VernacularName dum = new VernacularName();
                //dum.TaxonID = 9692186;
                //dum.Language = "AAAAAAA";
                //dum.VernacularName1 = "AAAAA";
                //db.VernacularName.InsertOnSubmit(dum);
                //db.SubmitChanges();
            }
            memo("Done");

        }

        private void duplicatebutton_Click(object sender, EventArgs e)
        {
            //int n = 0;
            Dictionary<string, int> namedict = new Dictionary<string, int>();
            Dictionary<string, int> speciesnamedict = new Dictionary<string, int>();
            Dictionary<string, List<int>> doubledict = new Dictionary<string, List<int>>();
            Dictionary<string, List<int>> doublespeciesdict = new Dictionary<string, List<int>>();

            int nmax = 10000000;

            string fn = @"I:\dotnwb3\doubledict_.txt";
            using (StreamWriter sw = new StreamWriter(util.unusedfilename(fn)))
            {
                int n = 0;
                var qg = from c in db.Taxon where c.TaxonRank == "genus" select c;
                memo("Genera: " + qg.Count());

                foreach (Taxon tt in qg)
                {
                    if (tt.TaxonomicStatus != null && tt.TaxonomicStatus != "accepted name")
                        continue;
                    string name = util.dbtaxon_name(tt);
                    if (namedict.ContainsKey(name))
                    {
                        memo("Double found " + name);
                        if (!doubledict.ContainsKey(name))
                        {
                            doubledict.Add(name, new List<int>());
                            doubledict[name].Add(namedict[name]);
                        }
                        doubledict[name].Add(tt.TaxonID);
                    }
                    else
                    {
                        namedict.Add(name, tt.TaxonID);
                    }
                    n++;
                    if (n % 1000 == 0)
                        memo("n = " + n);
                    if (n > nmax)
                        break;
                }

                memo("Genera done. Duplicates found: " + doubledict.Count);
                n = 0;

                foreach (string s in doubledict.Keys)
                {
                    speciesnamedict.Clear();
                    doublespeciesdict.Clear();
                    var q = from c in db.Taxon
                            where c.TaxonRank == "species"
                            where c.TaxonomicStatus == "accepted name"
                            where c.Genus == s
                            select c;
                    foreach (Taxon tt in q)
                    {
                        string name = util.dbtaxon_name(tt);
                        if (!speciesnamedict.ContainsKey(name))
                            speciesnamedict.Add(name, tt.TaxonID);
                        else
                        {
                            memo("Double found " + name);
                            if (!doublespeciesdict.ContainsKey(name))
                            {
                                doublespeciesdict.Add(name, new List<int>());
                                doublespeciesdict[name].Add(speciesnamedict[name]);
                            }
                            doublespeciesdict[name].Add(tt.TaxonID);

                        }
                    }

                    foreach (string dd in doublespeciesdict.Keys)
                    {
                        StringBuilder sb = new StringBuilder(dd);
                        foreach (int i in doublespeciesdict[dd])
                            sb.Append("\t" + i);
                        memo(sb.ToString());
                        sw.WriteLine(sb.ToString());
                    }

                    n++;
                    if (n % 100 == 0)
                        memo("n = " + n);
                    if (n > nmax)
                        break;
                }

                n = 0;

                var qh = from c in db.Taxon where c.Genus == null select c;
                memo("Higher taxa: " + qh.Count());

                foreach (Taxon tt in qh)
                {
                    if (tt.TaxonomicStatus != null && tt.TaxonomicStatus != "accepted name")
                        continue;
                    string name = util.dbtaxon_name(tt);
                    if (namedict.ContainsKey(name))
                    {
                        memo("Double found " + name);
                        if (!doubledict.ContainsKey(name))
                        {
                            doubledict.Add(name, new List<int>());
                            doubledict[name].Add(namedict[name]);
                        }
                        doubledict[name].Add(tt.TaxonID);
                    }
                    else
                    {
                        namedict.Add(name, tt.TaxonID);
                    }
                    n++;
                    if (n % 1000 == 0)
                        memo("n = " + n);
                    if (n > nmax)
                        break;
                }


                foreach (string s in doubledict.Keys)
                {
                    StringBuilder sb = new StringBuilder(s);
                    foreach (int i in doubledict[s])
                        sb.Append("\t" + i);
                    memo(sb.ToString());
                    sw.WriteLine(sb.ToString());
                }
                memo("Duplicates done!");
            }
        }

        private void makebutton_Click(object sender, EventArgs e)
        {
            MakeSpecies ms = new MakeSpecies(db);
            ms.Show();
        }

        private void cebengbutton_Click(object sender, EventArgs e)
        {
            CebEng ce = new CebEng();
            ce.Show();
        }


        private void Badfindbutton_Click(object sender, EventArgs e)
        {
            //InputBox ib = new InputBox("Category to check for bad articles:", false);
            //ib.ShowDialog();
            //string cat = ib.gettext();

            BadArticles ba = new BadArticles(db);
            ba.Show();

        }

        private void Distributionbutton_Click(object sender, EventArgs e)
        {
            DistributiontestForm df = new DistributiontestForm(db);
            df.Show();
        }

        public List<string> getgallery(Page p)
        {
            List<string> ls = new List<string>();

            memo("getgallery");

            if (!p.text.Contains("gallery"))
            {
                memo("No gallery");
                return ls;
            }

            string tx = p.text.Replace("\n", "£");

            string rex = @"\<gallery\>(.*)\<\/gallery\>";
            foreach (Match m in Regex.Matches(tx, rex,RegexOptions.Singleline))
            {
                memo("Match " + m.Groups[1].Value);
                string[] pix = m.Groups[1].Value.Split('£');
                foreach (string pic in pix)
                    ls.Add(pic);
            }

            return ls;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            string cat = "Paghimo ni bot 2012-12";

            Site site = util.login("ceb");

            PageList pl = new PageList(site);
            //pl.FillFromCategory(cat);
            pl.Add(new Page("Pongo pygmaeus"));

            //if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //string fn = openFileDialog1.FileName;

                //using (StreamReader sr = new StreamReader(fn))
                //{
                //    while (!sr.EndOfStream)
                //    {
                //        string line = sr.ReadLine();
                //        pl.Add(new Page(line.Split('\t')[0]));
                //    }
                //}
                int nbot = 0;
                int nhuman = 0;
                int n = 0;

                //using (StreamWriter sw = new StreamWriter(util.unusedfilename(@"I:\dotnwb3\humantouched_.txt")))
                {
                    foreach (Page p in pl)
                    {
                        n++;
                        if (n % 1000 == 0)
                            memo(n.ToString());
                        //if (n > 10)
                        //    break;
                        if (human_touched_stats(p, site))
                        {
                            nhuman++;
                            //memo(p.title);
                            bool imonly = util.human_image_only(p, site);
                            if (!p.Exists())
                                util.tryload(p, 1);
                            memo(p.text);
                            List<string> ls1 = p.GetImages();
                            List<string> ls2 = getgallery(p);
                            memo("ls1 " + ls1.Count);
                            memo("ls2 " + ls2.Count);
                            //sw.WriteLine(p.title + "\t" +imonly);
                            if (imonly)
                                memo(p.title + " image only");
                            else
                                memo(p.title + " NOT image only");
                        }
                        else
                            nbot++;
                    }
                }
                memo("");
                memo(humandict.GetSHist());
                memo("");
                memo(botdict.GetSHist());
                memo("");
                memo(triggerdict.GetSHist());
                memo("");
                memo(attdict.GetSHist());
                memo("");
                memo("nbot = " + nbot);
                memo("nhuman = " + nhuman);
            }
        }

        public static hbookclass botdict = new hbookclass("Not human-touched");
        public static hbookclass humandict = new hbookclass("Human-touched");
        public static hbookclass triggerdict = new hbookclass("Human trigger");

        public bool human_touched_stats(Page p, Site site)
        {
            List<string> userlist = new List<string>();
            PageList plh = new PageList(site);
            string currentrev = "";
            string prevrev = "";
            plh.FillFromPageHistory(p.title, 10);
            foreach (Page ph in plh)
            {
                if (!util.tryload(ph, 2))
                    return true;
                //memo("\n\n  " + ph.lastUser+"\n");
                //userlist.Add(ph.lastUser);
                currentrev = ph.revision;
                if (!String.IsNullOrEmpty(prevrev))
                {
                    List<string> diff = util.diffversions(site, prevrev, currentrev);
                    foreach (string s in diff)
                        memo(s);
                    
                }
                prevrev = currentrev;
                if (util.humanlist.Contains(ph.lastUser))
                {
                    triggerdict.Add(ph.lastUser);
                    return true;
                }
                if (ph.lastUser.ToLower().Contains("bot"))
                {
                    userlist.Add(ph.lastUser);
                    continue;
                }
                if (util.get_alphabet(ph.lastUser.Substring(0, 2)) == "none")
                {
                    userlist.Add(ph.lastUser + " (ip)");
                    continue;
                }
                if (util.neglectlist.Contains(ph.lastUser))
                {
                    userlist.Add(ph.lastUser + " (nl)");
                    continue;
                }
                if (ph.comment.Contains("GlobalReplace"))
                {
                    userlist.Add(ph.lastUser + " (gr)");
                    continue;
                }
                foreach (string s in userlist)
                    humandict.Add(s);
                triggerdict.Add(ph.lastUser);
                return true;
            }
            foreach (string s in userlist)
                botdict.Add(s);
            return false;

        }

        public hbookclass attdict = new hbookclass("Attributes");


        private void namelistbutton_Click(object sender, EventArgs e)
        {
            string fntriv = @"I:\dotnwb3\trivname_ceb.csv";
            string fngroup = @"I:\dotnwb3\groupname_ceb.csv";
            string fncebspec = @"I:\dotnwb3\cebuano species names.txt";

            List<string> trivlist = new List<string>();
            Dictionary<string, namelistclass> namedict = new Dictionary<string, namelistclass>();

            using (StreamReader srtriv = new StreamReader(fntriv))
            {
                while (!srtriv.EndOfStream)
                {
                    string line = srtriv.ReadLine();
                    string[] words = line.Split(';');
                    if (words.Length < 2)
                        continue;
                    if (trivlist.Contains(words[1]))
                        memo("Duplicate trivname " + words[1]);
                    else
                        trivlist.Add(words[1]);

                    if (!namedict.ContainsKey(words[0]))
                    {
                        namelistclass nc = new namelistclass();
                        nc.triv = words[1];
                        if (words[0].Split().Length > 1)
                            nc.isspecies = true;
                        namedict.Add(words[0], nc);
                    }
                    else if (String.IsNullOrEmpty(namedict[words[0]].triv))
                    {
                        namedict[words[0]].triv = words[1];
                    }
                    else
                        memo("Duplicate triv\t" + words[0]);
                }
            }

            using (StreamReader srgroup = new StreamReader(fngroup))
            {
                while (!srgroup.EndOfStream)
                {
                    string line = srgroup.ReadLine();
                    string[] words = line.Split(';');
                    if (words.Length < 2)
                        continue;
                    if (!namedict.ContainsKey(words[0]))
                    {
                        namelistclass nc = new namelistclass();
                        nc.group = words[1];
                        if (words[0].Split().Length > 1)
                            nc.isspecies = true;
                        namedict.Add(words[0], nc);
                    }
                    else if (String.IsNullOrEmpty(namedict[words[0]].group))
                    {
                        namedict[words[0]].group = words[1];
                    }
                    else
                        memo("Duplicate group\t" + words[0]);
                }
            }

            using (StreamReader srcebspec = new StreamReader(fncebspec))
            {
                while (!srcebspec.EndOfStream)
                {
                    string line = srcebspec.ReadLine();
                    string[] words = line.Split('\t');
                    if (words.Length < 4)
                        continue;
                    if (String.IsNullOrEmpty(words[3]))
                        continue;
                    if (!namedict.ContainsKey(words[3]))
                    {
                        namelistclass nc = new namelistclass();
                        nc.cebspec = words[1];
                        if (words[3].Split().Length > 1)
                            nc.isspecies = true;
                        namedict.Add(words[3], nc);
                    }
                    else if (String.IsNullOrEmpty(namedict[words[3]].cebspec))
                    {
                        namedict[words[3]].cebspec = words[1];
                    }
                    else
                        memo("Duplicate cebspec\t" + words[3]);
                }
            }

            foreach (string s in namedict.Keys)
            {
                if (!String.IsNullOrEmpty(namedict[s].triv))
                {
                    if (!String.IsNullOrEmpty(namedict[s].group))
                    {
                        if (namedict[s].triv != namedict[s].group)
                            memo(s + "\t" + namedict[s].ToString());
                    }
                    else if (!String.IsNullOrEmpty(namedict[s].cebspec))
                    {
                        if (namedict[s].triv != namedict[s].cebspec)
                            memo(s + "\t" + namedict[s].ToString());
                    }
                }
                else if (!String.IsNullOrEmpty(namedict[s].cebspec))
                {
                    if (!trivlist.Contains(namedict[s].cebspec))
                    {
                        namedict[s].triv = namedict[s].cebspec;
                        trivlist.Add(namedict[s].cebspec);
                    }
                    else
                        memo("Duplicat trivname from cebspec : " + namedict[s].cebspec);

                }
                
                if (!String.IsNullOrEmpty(namedict[s].group))
                {
                    if (!String.IsNullOrEmpty(namedict[s].cebspec))
                    {
                        if (namedict[s].group != namedict[s].cebspec)
                            memo(s + "\t" + namedict[s].ToString());
                    }

                    //if (String.IsNullOrEmpty(namedict[s].triv))
                    //{
                    //    if (!trivlist.Contains(namedict[s].group))
                    //    {
                    //        namedict[s].triv = namedict[s].group;
                    //        trivlist.Add(namedict[s].group);
                    //    }

                    //}
                }
                else if (!namedict[s].isspecies)
                {
                    if (!String.IsNullOrEmpty(namedict[s].triv))
                        namedict[s].group = namedict[s].triv;
                }
                //memo(s + "\t" + namedict[s].ToString());
            }

            using (StreamWriter swtriv = new StreamWriter(util.unusedfilename(fntriv)))
            using (StreamWriter swgroup = new StreamWriter(util.unusedfilename(fngroup)))
            {
                foreach (string s in namedict.Keys)
                {
                    if (!String.IsNullOrEmpty(namedict[s].triv))
                        swtriv.WriteLine(s + ";" + namedict[s].triv);
                    if (!namedict[s].isspecies)
                        if (!String.IsNullOrEmpty(namedict[s].group))
                            swgroup.WriteLine(s + ";" + namedict[s].group + ";mga " + namedict[s].group + ";" + namedict[s].group);
                }
            }



        }

        private void testbutton_Click(object sender, EventArgs e)
        {
            var q = from c in db.Artnametable where c.Articlename.StartsWith("##") select c;
            foreach (Artnametable aa in q)
            {
                int tid = util.tryconvert(aa.Articlename.Replace("##", ""));
                Taxon tt = (from c in db.Taxon where c.TaxonID == tid select c).First();
                memo(tid + "\t" + util.dbtaxon_name(tt) + "\t" + tt.Family);
                aa.Articlename = util.dbtaxon_name(tt) + " (" + tt.Family + ")";
            }
            db.SubmitChanges();

            //Site site = util.login("ceb");
            //Page p = new Page("Pongo abelii");
            //util.tryload(p, 1);

            //List<string> oldiwlist = new List<string>();
            //bool oldiwfromwd = true;
            //try
            //{
            //    oldiwlist = p.GetInterLanguageLinks();
            //}
            //catch (Exception ee)
            //{
            //    string message = ee.Message;
            //    memo(message);
            //    //Thread.Sleep(10000);//milliseconds
            //}

            //memo("oldiwlist.Count " + oldiwlist.Count);
            //if (oldiwlist.Count > 0)
            //{
            //    //iwfound = true;
            //    string rex = @"\[\[\w{2,3}\:.+?\]\]";
            //    MatchCollection mc = Regex.Matches(p.text, rex);
            //    memo("mc " + mc.Count);
            //    if (mc.Count > 0)
            //    {
            //        oldiwfromwd = false;
            //        p.text += "\r\n";
            //        foreach (string str in oldiwlist)
            //            p.text += "\r\n[[" + str + "]]";

            //    }
            //}


        }

        private void Duplicatefixbutton_Click(object sender, EventArgs e)
        {
            MakeSpecies.read_taxonomic_rank();
            MakeSpecies.read_groupname();

            //Site site = util.login("ceb");

            Dictionary<string, List<int>> doubledict = new Dictionary<string, List<int>>();

            string fn = @"I:\dotnwb3\doubledict_1.txt";
            using (StreamReader sr = new StreamReader(fn))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] words =line.Split('\t');
                    if (words.Length < 3)
                        continue;
                    List<int> ls = new List<int>();
                    for (int i = 1; i < words.Length; i++)
                        ls.Add(util.tryconvert(words[i]));
                    doubledict.Add(words[0], ls);
                }
            }

            memo("Double dict " + doubledict.Count);
            int n = 0;

            foreach (String name in doubledict.Keys)
            {
                if (MakeSpecies.isincert(name))
                    continue;
                List<Taxon> tl = new List<Taxon>();
                List<string> artnamelist = new List<string>();
                
                bool samenamefound = false;
                foreach (int taxonid in doubledict[name])
                {
                    Taxon tt = (from c in db.Taxon where c.TaxonID == taxonid select c).FirstOrDefault();
                    if (tt == null)
                        continue;
                    foreach (Taxon tt2 in tl)
                    {
                        if (util.comparetaxa(tt, tt2))
                            memo("Duplicate found\t" + tt.TaxonID + "\t" + tt2.TaxonID);
                    }
                    tl.Add(tt);

                    Taxonclass tc = new Taxonclass(tt,db,"ceb");
                    string group = tc.getnamedgroup();
                    string artname = tc.Name;
                    if (!String.IsNullOrEmpty(tc.Name_sv))
                        artname = tc.Name_sv;
                    artname += " (" + MakeSpecies.groupname_sing[group] + ")";
                    foreach (string s in artnamelist)
                        if (s == artname)
                            samenamefound = true;
                    artnamelist.Add(artname);
                }

                if (samenamefound)
                {
                    //foreach (Taxon tt in tl)
                    //{
                    //    Taxonclass tc = new Taxonclass(tt, db, "ceb");
                    //    //string group = tc.getnamedgroup();
                    //    string artname = tc.Name;
                    //    if (!String.IsNullOrEmpty(tc.Name_sv))
                    //        artname = tc.Name_sv;
                    //    foreach (Taxon tt2 in tl)
                    //    {
                    //        if (tt2 == tt)
                    //            continue;
                    //        Taxonclass tc2 = new Taxonclass(tt2, db, "ceb");
                    //        string diff = util.taxondiff(tc, tc2);
                    //        string tdiff = "";
                    //        if (diff.Split('\t').Length > 1)
                    //            tdiff = diff.Split('\t')[1];
                    //        if (MakeSpecies.groupname_sing.ContainsKey(tdiff))
                    //            artname += " (" + MakeSpecies.groupname_sing[tdiff] + ")";
                    //        else
                    //            artname += " (" + tdiff + ")";
                    //    }
                    //    StringBuilder sb = new StringBuilder(tt.TaxonID + "\t" + artname);
                    //    foreach (string s in tc.parentdict.Keys)
                    //        sb.Append("\t" + tc.parentdict[s]);

                    //    memo(sb.ToString());
                    //}
                    //memo("");
                }
                else
                {
                    foreach (Taxon tt in tl)
                    {
                        Taxonclass tc = new Taxonclass(tt, db, "ceb");
                        string group = tc.getnamedgroup();
                        string artname = tc.Name;
                        if (!String.IsNullOrEmpty(tc.Name_sv))
                            artname = tc.Name_sv;
                        artname += " (" + MakeSpecies.groupname_sing[group] + ")";
                        StringBuilder sb = new StringBuilder(tt.TaxonID + "\t" + artname);
                        foreach (string s in tc.parentdict.Keys)
                            sb.Append("\t" + tc.parentdict[s]);
                        memo(sb.ToString());
                    }
                    memo("");

                }

                n++;
                if (n % 100 == 0)
                    memo("n = " + n);
                //else
            }
        }

        private void disambigs_in_cat(string cat, Site site)
        {
            PageList pl = new PageList(site);
            pl.FillFromCategory(cat);
            foreach (Page p in pl)
                if (p.title.Contains("("))
                    memo(p.title);

        }

        private void Disambigbutton_Click(object sender, EventArgs e)
        {
            Site site = util.login("ceb");

            //Find old disambigs:

            //disambigs_in_cat("Paghimo ni bot 2012-12",site);
            //for (int year =2013;year<=2014;year++)
            //    for (int month=1;month <=12;month++)
            //    {
            //        string cat = "Paghimo ni bot " + year + "-" + month.ToString("D2");
            //        disambigs_in_cat(cat, site);
            //    }

            //Study old disambigs:

            MakeSpecies.read_taxonomic_rank();
            MakeSpecies.read_groupname();

            string fn = @"I:\dotnwb3\badarticle\disambigs-old.txt";
            using (StreamReader sr = new StreamReader(fn))
            {
                while (!sr.EndOfStream)
                {
                    string tit = sr.ReadLine().Trim('\t');
                    string cleantit = util.remove_disambig(tit);

                    StringBuilder sb = new StringBuilder(tit);

                    Page pdis = new Page(tit);
                    util.tryload(pdis, 4);
                    string group = "";
                    string name = util.taxon_from_page(pdis);
                    string regnum = util.regnum_from_page(pdis);
                    MakeSpecies.regnum_to_make = regnum;
                    //int taxonid = MakeSpecies.get_name_id(name, "", "", db);
                    //Taxonclass tcdis = Taxonclass.loadtaxon(taxonid, db, "ceb");
                    Taxonclass tcdis = new Taxonclass(pdis, db, "ceb");
                    if (tcdis != null)
                        group = tcdis.getnamedgroup();
                    if (MakeSpecies.groupname_sing.ContainsKey(group))
                        group = MakeSpecies.groupname_sing[group];
                    //memo(name + "\tgroup= " + group);

                    //Artnametable an = (from c in db.Artnametable
                    //                   where c.Taxonid == tcdis.taxonid
                    //                   select c).FirstOrDefault();
                    //if (an != null)
                    //    memo("artnametable " + an.Articlename);
                    sb.Append("\t" + regnum + "\t" + group);
                    Page pclean = new Page(cleantit);
                    util.tryload(pclean, 2);

                    string fork = util.findfork(cleantit, site);

                    if (pclean.Exists())
                    {
                        if (pclean.IsRedirect())
                        {
                            if (pclean.RedirectsTo() == tit)
                                sb.Append("\tmain redirects back\t\t"+fork);
                            else
                            {
                                if (pclean.RedirectsTo().Contains("(pagkla"))
                                    sb.Append("\tmain redirects to fork\t" + pclean.RedirectsTo()+"\t"+fork);
                                else
                                {
                                    Page pred = new Page(pclean.RedirectsTo());
                                    util.tryload(pred, 2);
                                    if (pred.Exists())
                                    {
                                        if (pred.text.Contains("{{giklaro"))
                                            sb.Append("\tmain redirects to fork\t" + pclean.RedirectsTo()+"\t"+fork);
                                        else
                                            sb.Append("\tmain redirects to\t" + pclean.RedirectsTo() + "\t" + fork);

                                    }
                                    else
                                    {
                                        sb.Append("\tmain redirects to nonexistent\t" + pclean.RedirectsTo() + "\t" + fork);

                                    }
                                }


                            }
                        }
                        else if (pclean.text.Contains("{{giklaro"))
                        {
                            sb.Append("\tmain is fork" + "\t\t" + fork);
                        }
                        else if (pclean.text.Contains("eobox"))
                        {
                            sb.Append("\tmain is geography" + "\t\t" + fork);
                        }
                        else if (pclean.text.Contains("Komyun"))
                        {
                            sb.Append("\tmain is commune" + "\t\t" + fork);
                        }
                        else
                        {
                            string diff = util.taxondiff(pdis, pclean);
                            if (String.IsNullOrEmpty(diff))
                                sb.Append("\tmain no taxobox");
                            else if (diff == "SAME")
                            {
                                sb.Append("\tSAME");
                                pdis.text = "#REDIRECT[[" + cleantit + "]]";
                                util.trysave(pdis, 2,site);
                            }
                            else
                                sb.Append("\tdiff=\t" + diff + "\t" + fork);
                        }

                    }
                    else
                    {
                        var q = from c in db.Taxon
                                where c.ScientificName == cleantit
                                select c;
                        if (q.Count() == 0)
                            sb.Append("\tmain not existing in db");
                        else if (q.Count() == 1)
                        {
                            sb.Append("\tmain not existing; 1 in db");
                            util.trymove(pdis, cleantit, 2, "Disambig not needed");
                        }
                        else if (q.Count() > 1)
                        {
                            sb.Append("\tmain not existing; " + q.Count() + " in db");
                        }
                    }

                    string newname = tcdis.articlename;
                    if (String.IsNullOrEmpty(newname))
                    {
                        newname = cleantit + " (" + group + ")";
                    }
                    if ( newname != tit)
                    {
                        util.trymove(pdis, newname, 3, "changing diasambig");
                        sb.Append("\tmoved to\t" + newname);
                    }
                    memo(sb.ToString());

                }
            }
        }

        private void disambigDBbutton_Click(object sender, EventArgs e)
        {
            Site site = util.login("ceb");
            MakeSpecies.read_taxonomic_rank();

            string fn = @"I:\dotnwb3\badarticle\old-disambig-artnames.txt";
            int anid = (from c in db.Artnametable select c.Id).Max()+1;
            string makelang = "ceb";
            using (StreamReader sr = new StreamReader(fn))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (String.IsNullOrEmpty(line))
                        continue;
                    Page p = new Page(line);
                    util.tryload(p, 2);
                    Taxonclass tc = new Taxonclass(p, db, "ceb");
                    if (tc.taxonid <= 0)
                        continue;
                    var q = from c in db.Artnametable where c.Taxonid == tc.taxonid select c;
                    if (q.Count() > 0)
                        memo("Already in DB: " + tc.taxonid + "\t" + line+"\t"+q.First().Articlename);
                    else
                    {
                        Artnametable an = new Artnametable();
                        an.Id = anid;
                        anid++;
                        an.Taxonid = tc.taxonid;
                        an.Lang = makelang;
                        an.Articlename = line;
                        db.Artnametable.InsertOnSubmit(an);
                        db.SubmitChanges();
                    }
                }
            }
            //string fn = @"I:\dotnwb3\artname-duplicate2-ceb.txt";
            //int anid = (from c in db.Artnametable select c.Id).Max() + 1;
            //string makelang = "ceb";
            //using (StreamReader sr = new StreamReader(fn))
            //{
            //    while (!sr.EndOfStream)
            //    {
            //        string line = sr.ReadLine();
            //        string[] words = line.Split('\t');
            //        if (words.Length < 2)
            //            continue;
            //        int taxonid = util.tryconvert(words[0]);
            //        if (taxonid <= 0)
            //            continue;
            //        var q = from c in db.Artnametable where c.Taxonid == taxonid select c;
            //        if (q.Count() > 0)
            //            memo("Already in DB: " + taxonid + "\t" + words[1]);
            //        else
            //        {
            //            Artnametable an = new Artnametable();
            //            an.Id = anid;
            //            anid++;
            //            an.Taxonid = taxonid;
            //            an.Lang = makelang;
            //            an.Articlename = words[1];
            //            db.Artnametable.InsertOnSubmit(an);
            //            db.SubmitChanges();
            //        }
            //    }
            //}
        }
    }
}
