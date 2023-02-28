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
using System.Web;
using System.Net;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using DotNetWikiBot;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using System.Xml;
using HtmlAgilityPack;
using System.Media;



namespace MakeSpecies
{
    public partial class MakeSpecies : Form
    {
        COL2019 db;

        public static int itodo = 0;
        public static int istatus4 = 0;
        public static int idone = 0;
        public static int nskip = 1;  //Make only every nskip:th article. Set to 1 to make all. Set to 9 999 999 to do a dry run. 
        public static int offset = 0; //When skipping articles, create with offset from exactly divisble by nskip
        public static bool reallymake = !(nskip > 1);
        public static string bottomrank = "species"; //Create down to rank bottomskip; skip lower taxa
        public static bool plant_divisio = true;
        public static string regnum_to_make = "Plantae";
        public static string regnum_to_make_sv;
        public static bool makemonotypic = false;
        public static string makelang="ceb";
        public static string makewiki;
        public static string resume_at = "";
        public static string colyear = "2019";
        public static string testprefix = "";
        public static bool savelocally = false;
        public static string savefolder = @"I:\dotnwb3\testmakespecies\";

        public static XmlDocument currentxml = new XmlDocument();
        public static XmlNode currentnode;
        public static bool found13 = false;
        public static bool found11 = false;

        //public static Taxonclass[] taxonarray = new Taxonclass[1600000];
        public static Dictionary<int, Taxonclass> taxondict = new Dictionary<int, Taxonclass>();
        public static Dictionary<string, List<int>> name_id = new Dictionary<string, List<int>>();
        public static Dictionary<int, IUCNClass> iucndict = new Dictionary<int, IUCNClass>();
        public static Dictionary<int, IUCNClass> redfidict = new Dictionary<int, IUCNClass>();
        public static Dictionary<int, IUCNClass> redsvdict = new Dictionary<int, IUCNClass>();
        public static Dictionary<string, int> redsvname_id = new Dictionary<string, int>();
        public static Dictionary<string, int> iucnnamedict = new Dictionary<string, int>();
        public static Dictionary<string, string> COL_sourcedict = new Dictionary<string, string>();
        public static Dictionary<string, List<int>> doubledict = new Dictionary<string, List<int>>();


        public static Dictionary<int, string> habitatdict = new Dictionary<int, string>();
        public static Dictionary<string, int> habitatcodedict = new Dictionary<string, int>();
        public static Dictionary<string, string> iucnstatusdict = new Dictionary<string, string>();
        //public static Dictionary<int, string> provincenamedict = new Dictionary<int, string>();
        public static Dictionary<int, char[]> provincedict = new Dictionary<int, char[]>();

        public static Dictionary<int, int> id1311 = new Dictionary<int, int>();

        public static string COLfolder = "I:\\dotnwb3\\2011AC_26July\\tables\\";
        public static string dyntaxafolder = "I:\\dotnwb3\\";
        public static string botname = "Lsjbot";
        public static string password;
        public static bool loggedin = false;

        public static statclass stats = new statclass();

        public static int dyntaxadiff = 20000000; //added to dyntaxa-id, to be able to mix with COL-id in same dictionary

        public static DateTime oldtime = DateTime.Now;

        public static bool dynloop = false;
        public static bool dynread = false;
        public static bool dynonly = false;

        public static Dictionary<string, string> groupname_sing = new Dictionary<string, string>();
        public static Dictionary<string, string> groupname_plural = new Dictionary<string, string>();
        public static Dictionary<string, string> groupname_attr = new Dictionary<string, string>();

        public static Dictionary<int, string> swe_name = new Dictionary<int, string>();
        public static Dictionary<string, string> latin_swe_name = new Dictionary<string, string>();
        public static Dictionary<int, int> swe_status = new Dictionary<int, int>();
        public static Dictionary<int, int> swe_immi = new Dictionary<int, int>();
        public static Dictionary<int, string> swe_status_element = new Dictionary<int, string>();
        public static Dictionary<int, string> dyntaxa_url = new Dictionary<int, string>();
        public static Dictionary<string, string> dynstatus_text = new Dictionary<string, string>();

        public static Dictionary<int, string> rank_name = new Dictionary<int, string>();
        public static Dictionary<string, int> name_rank = new Dictionary<string, int>();
        public static Dictionary<string, int> taxotop = new Dictionary<string, int>();  //fix filling of this one!

        public static Dictionary<string, string> rank_eng_latin = new Dictionary<string, string>();
        public static Dictionary<string, string> rank_latin_swe = new Dictionary<string, string>();
        public static Dictionary<string, string> rank_name_swe = new Dictionary<string, string>();
        public static Dictionary<string, string> rank_latin_swe_indef = new Dictionary<string, string>();
        public static Dictionary<string, string> rank_latin_swe_def = new Dictionary<string, string>();
        public static Dictionary<string, int> rank_order = new Dictionary<string, int>();

        //public static Dictionary<string, string> svampfakta = new Dictionary<string, string>();
        //public static Dictionary<string, string> auktordict = new Dictionary<string, string>();
        //public static Dictionary<string, string> zooauktordict = new Dictionary<string, string>();
        //public static Dictionary<string, string> botauktordict = new Dictionary<string, string>();
        //public static Dictionary<string, List<string>> forkauktordict = new Dictionary<string, List<string>>();

        public static Dictionary<string, string> rank_above = new Dictionary<string, string>();
        public static Dictionary<string, string> rank_below = new Dictionary<string, string>();

        public static int swecode = 0;
        public static int nprint = 100000;

        public static Dictionary<string, int> langhist = new Dictionary<string, int>();
        public static Dictionary<string, int> rankhist = new Dictionary<string, int>();

        public static Dictionary<string, string> taxospecial = new Dictionary<string, string>();

        public static List<int> subspecific = new List<int>();
        public static List<int> donetree = new List<int>();
        public static List<string> donecatconf = new List<string>();

        public static string tabstring = "\t";
        public static char tabchar = tabstring[0];


        public static List<string> refnamelist = new List<string>();
        public static string reflist = "<references>";

        public static Site makesite;
        public static List<Site> iwsites = new List<Site>();
        public static string[] iwlang = { "sv", "en", "nl", "de", "ceb", "war" };
        //public static Site cmsite;
        //public static Site wssite;
        public static PageList plregnum;
        public static Page pconflict;
        public static Page pfail;
        public static Page ptree;
        public static Page pdynsplit;
        public static Page pstats;

        public static string talkprefix = "Diskussion";
        public static List<string> donecats = new List<string>();
        public static List<string> donetemplates = new List<string>();
        public static Dictionary<int, string> phrases = new Dictionary<int, string>();
        public static string taxon_to_make;

        public static int ispecies = 0;


        public MakeSpecies(COL2019 dbpar)
        {
            InitializeComponent();

            db = dbpar;

            LBwiki.Items.Add("ceb");
            LBwiki.Items.Add("sv");
            LBwiki.Items.Add("diq");
            LBwiki.SelectedItem = "ceb";

            LBlang.Items.Add("ceb");
            LBlang.Items.Add("sv");
            LBlang.Items.Add("diq");
            LBlang.SelectedItem = "ceb";

            LBregnum.Items.Add("Animalia");
            LBregnum.Items.Add("Plantae");
            LBregnum.Items.Add("Fungi");
            LBregnum.Items.Add("Protozoa");
            LBregnum.Items.Add("Chromista");
            LBregnum.Items.Add("Bacteria");
            LBregnum.Items.Add("Archaea");
            LBregnum.Items.Add("Viruses");
            LBregnum.SelectedItem = "Plantae";

            util.oldtime = DateTime.Now;

        }

        private void maketaxon(string ss)
        {
            int ssid = get_name_id(ss, "", "");
            maketaxon(ss, ssid);
        }
        private void maketaxon(string ss, int ssid)
        {
            taxon_to_make = ss;

            if (!taxondict_from_taxonid(ssid))
            {
                memo("Bad taxonID " + ssid);
                return;
            }

            if ((ssid > 0) && (taxondict[ssid].regnum != regnum_to_make))
            {
                memo("Wrong regnum");
                ssid = -1;
            }
            if (ssid > 0)
            {
                if (latin_swe_name.ContainsKey(ss))
                    memo("Trivial name = " + latin_swe_name[ss]);
                else
                    memo("No trivial name");

                itodo = 0;
                istatus4 = 0;
                if (!dynonly)
                {
                    dynloop = false;
                    itodo = countarticle(ssid);
                }
                if (dynread)
                {
                    dynloop = true;
                    itodo = countarticle(ssid);
                }

                memo(itodo.ToString() + " articles to do.");
                memo(istatus4.ToString() + " articles with status provisionally accepted.");

                //Console.Write("OK? (y/n)");
                //string choice = Console.ReadLine();
                //if (choice == "n")
                //    continue;

                //if (itodo > nskip)
                //{
                //findcat(ssid);
                //memo("Syncat");
                //findsyncat(ssid);
                //if (makelang == "sv")
                //{
                //    memo("Swecat");
                //    findswecat(ssid);
                //}
                //memo("Cats done");
                //}

                makesite.defaultEditComment = mp(60, null) + " " + taxon_to_make;


                idone = 0;

                //*** actually make! (recursive)
                makearticle(ssid);
                //***

                if (CB_recursive.Checked)
                    donetree.Add(ssid);


            }

        }

        private void Gobutton_Click(object sender, EventArgs e)
        {
            memo("Regnum_to_make_sv: " + regnum_to_make_sv);

            oldtime = oldtime.AddSeconds(5);

            if (String.IsNullOrEmpty(TBtaxon.Text))
            {
                memo("No taxon!");
                return;
            }

            string ss = util.initialcap(TBtaxon.Text);
            int ssid = util.tryconvert(ss);
            if (ssid > 0)
            {
                memo("TaxonID " + ssid);
                Taxon tt = (from c in db.Taxon where c.TaxonID == ssid select c).First();
                if (tt == null)
                {
                    ssid = -1;
                    memo("Invalid taxonID " + ss);
                }
                else
                {
                    ss = util.dbtaxon_name(tt);
                    memo(ss);
                }
            }
            else
                ssid = get_name_id(ss, "", "");

            if (ssid < 0)
            {
                memo("Invalid taxon " + ss);
                return;
            }

            maketaxon(ss,ssid);

            //Print statistics at the end:
            memo(stats.GetStat());
            pstats.text += "\n\n== [[" + taxon_to_make + "]] ==\n\n";
            pstats.text += stats.GetStat();
            if (CBmemo.Checked)
                memo(pstats.text);
            else
                util.trysave(pstats, 1, "Statistics");
            stats.nart = 0;
            stats.nredirect = 0;
            stats.ncat = 0;
            stats.nbot = 0;
            stats.ntalk = 0;
            stats.sizelist.Clear();

            memo("============================");
            memo("Done Go");
            memo("============================");

        }


        public void filldonetree()
        {
            if (makelang == "diq")
            {
                //donetree.Add("");
                //donetree.Add("");


            }
            else if (makelang == "sv")
            {
                //donetree.Add("");
                //donetree.Add("");


            }
            else if (makelang == "ceb")
            {
                //donetree.Add("");
                //donetree.Add("");

            }
            else if (makelang == "war")
            {
                //donetree.Add("Afrolicania");
                //donetree.Add("");

            }

            var q = from c in db.DoneTable
                    where c.Makelang == makelang
                    select c;

            foreach (DoneTable dt in q)
            {
                donetree.Add((int)dt.TaxonID);
            }
        }


        public static void read_phrases()
        {
            using (StreamReader sr = new StreamReader(dyntaxafolder + "phraselist.csv"))
            {

                String headline = "";
                headline = sr.ReadLine();

                int icol = 0;
                string[] langs = headline.Split(';');
                for (icol = 0; icol < langs.Length; icol++)
                {
                    if (langs[icol] == makelang)
                    {
                        break;
                    }
                }

                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();
                    //memo(line);

                    string[] words = line.Split(';');
                    for (int jj = 1; jj < words.Length; jj++)
                    {
                        words[jj] = words[jj].Trim();
                    }
                    int ip = Convert.ToInt32(words[0]);
                    phrases.Add(ip, words[icol]);
                }
            }

        }

        public static string mp(int np, string[] param)
        {
            if (phrases.Count == 0)
                read_phrases();

            int ip = 0;
            string sret = phrases[np];
            if (param != null)
                foreach (string s in param)
                {
                    ip++;
                    sret = sret.Replace("#" + ip.ToString(), s);
                }

            return sret;
        }

        public static void read_taxonomic_rank()
        {
            int imax = 0;
            int n = 0;
            int i = 0;

            using (StreamReader sr = new StreamReader(COLfolder + "rank_eng_latin.txt"))
            {
                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();
                    string[] words = line.Split(tabchar);
                    if (words.Length < 2)
                        continue;
                    //memo(words[0] + "|" + words[1]);

                    rank_eng_latin.Add(words[0], words[1]);

                    n++;

                }

                //memo("n    (rank-eng-latin) = " + n.ToString());

            }

            n = 0;

            using (StreamReader sr = new StreamReader(COLfolder + "rank_latin_" + makelang + ".txt"))
            {
                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();
                    string[] words = line.Split(tabchar);
                    //memo(words[0] + "|" + words[1]);

                    if (words.Length >= 2)
                        rank_latin_swe.Add(words[0], words[1]);
                    else
                    {
                        //memo("Line = " + line);
                        //Console.ReadLine();
                    }

                    n++;

                }

                //memo("n    (rank-latin-sv) = " + n.ToString());

            }

            n = 0;


            using (StreamReader sr = new StreamReader(COLfolder + "rank_" + makelang + "_linked.txt"))
            {
                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();
                    string[] words = line.Split(tabchar);
                    //memo(words[0] + "|" + words[1]);

                    rank_latin_swe_indef.Add(words[0], words[1]);
                    rank_latin_swe_def.Add(words[0], words[2]);

                    n++;

                }

                //memo("n    (rank-" + makelang + "-linked) = " + n.ToString());

            }

            n = 0;


            using (StreamReader sr = new StreamReader(COLfolder + "taxonomic_rank.txt"))
            {
                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();
                    string[] words = line.Split(tabchar);
                    //memo(words[0] + "|" + words[1]);
                    i = Convert.ToInt32(words[0]);
                    //memo("i    = " + i.ToString());
                    if (i > imax)
                        imax = i;
                    //memo("imax    = " + imax.ToString());


                    foreach (string k in rank_eng_latin.Keys)
                    {
                        words[1] = words[1].Replace(k, rank_eng_latin[k]);
                    }

                    string swerank = String.Copy(words[1]);

                    foreach (string k in rank_latin_swe.Keys)
                    {
                        swerank = swerank.Replace(k, rank_latin_swe[k]);
                    }

                    //scientific_name_element.Add(i, words[1]);
                    rank_name.Add(i, words[1]);
                    name_rank.Add(words[1], i);

                    rank_name_swe.Add(words[1], swerank);
                    //memo(words[1] + "; " + swerank);

                    n++;
                    if ((n % 1000) == 0)
                    {
                        //memo("n (rank-name)    = " + n.ToString());

                    }
                }

                //memo("imax (rank-name) = " + imax.ToString());
                //memo("n    (rank-name) = " + n.ToString());

                rank_order.Add("species", 0);
                rank_order.Add("genus", 10);
                rank_order.Add("familia", 20);
                rank_order.Add("ordo", 30);
                rank_order.Add("classis", 40);
                rank_order.Add("phylum", 50);
                rank_order.Add("divisio", 50);
                rank_order.Add("regnum", 60);
                rank_order.Add("subspecies", -10);
                rank_order.Add("varietas", -20);
                rank_order.Add("forma", -30);
                rank_order.Add("subphylum", 48);
                rank_order.Add("subdivisio", 48);
                rank_order.Add("superfamilia", 22);

            }


            //memo("subranks");
            string[] subranks = new string[] { "not assigned", "aggregate", "bio-variety", "convar", "cultivar", "cultivar-group", "form", "graft-chimaera", "hybrid", "infraspecifictaxon", "infraspecies", "infravariety", "microspecies", "morph", "morphovar", "mutant", "patho-variety", "serovar", "strain", "sub-sub-variety", "sub-variety", "subspecies", "subspecificaggregate", "subsubform", "subform", "supervariety", "variety" };
            List<string> sublist = new List<string>();
            foreach (string s in subranks)
            {
                foreach (string k in rank_eng_latin.Keys)
                {
                    sublist.Add(s.Replace(k, rank_eng_latin[k]));
                }
            }

            foreach (int j in rank_name.Keys)
            {
                if (sublist.Contains(rank_name[j]))
                {
                    subspecific.Add(j);
                    //memo(rank_name[j]);
                }
            }

            rank_above.Add("species", "genus");
            rank_above.Add("genus", "familia");
            rank_above.Add("familia", "ordo");
            rank_above.Add("superfamilia", "ordo");
            rank_above.Add("ordo", "classis");
            if (plant_divisio && ((regnum_to_make == "Plantae") || (regnum_to_make == "Fungi")))
                rank_above.Add("classis", "divisio");
            else
                rank_above.Add("classis", "phylum");
            rank_above.Add("phylum", "regnum");
            rank_above.Add("divisio", "regnum");

            foreach (string r in rank_above.Keys)
                if (!rank_below.ContainsKey(rank_above[r]))
                    rank_below.Add(rank_above[r], r);

            //memo("Press return");//to make pause here, in case of compilation errors
            //string dummy = Console.ReadLine();

        }


        public static void read_trivname()
        {
            string fn = dyntaxafolder + "trivname_" + makelang + ".csv";
            //using (StreamWriter sw = new StreamWriter(util.unusedfilename(fn)))
            using (StreamReader sr = new StreamReader(fn))
            {
                int n = 0;


                String headline = "";
                headline = sr.ReadLine();

                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();
                    //memo(line);

                    string[] words = line.Split(';');
                    for (int jj = 1; jj < words.Length; jj++)
                    {
                        words[jj] = words[jj].Trim();
                        if (words[jj].Length < 2)
                            words[jj] = "";
                    }

                    //memo("Length = " + words.Length.ToString());
                    if (words.Length >= 2)
                    {
                        string taxon = words[0];
                        if (String.IsNullOrEmpty(taxon))
                            continue;
                        string trivname = words[1].ToLower();
                        if (String.IsNullOrEmpty(trivname))
                            continue;
                        if (!latin_swe_name.ContainsKey(taxon))
                            latin_swe_name.Add(taxon, trivname);
                        int taxid = -1;
                        if (words.Length >= 3)
                        {
                            taxid = util.tryconvert(words[2]);
                            //if (!taxondict_from_taxonid(taxid))
                            //    taxid = -1;
                        }
                        if (taxid < 0)
                            continue;
                        //--------------
                        //if (taxid < 0)
                        //    taxid = get_name_id(taxon, "", "");

                        if (taxid > 0)
                        {
                            //taxondict[taxid].Name_sv = trivname;
                            if (!swe_name.ContainsKey(taxid))
                                swe_name.Add(taxid, trivname);
                        }
                        //else
                        //{
                        //    taxid = get_name_id(taxon, "phylum", "");
                        //    if (taxid > 0)
                        //    {
                        //        taxondict[taxid].Name_sv = trivname;
                        //        if (!swe_name.ContainsKey(taxid))
                        //            swe_name.Add(taxid, trivname);
                        //    }
                        //}
                        //sw.WriteLine(taxon + ";" + trivname + ";"+taxid);
                        n++;
                    }

                    //if (name_id.ContainsKey(words[0]))
                    //{
                    //    int taxid = name_id[words[0]];
                    //    if (String.IsNullOrEmpty(taxondict[taxid].Name_sv))
                    //        taxondict[taxid].Name_sv = groupname_plural[words[0]];
                    //    if (!swe_name.ContainsKey(taxid))
                    //        swe_name.Add(taxid, groupname_plural[words[0]]);
                    //}
                }

                //memo("n (trivname) = " + n.ToString());
                //memo("<return>");
                //Console.ReadLine();

            }
        }


        public static void read_groupname()
        {
            using (StreamReader sr = new StreamReader(dyntaxafolder + "groupname_" + makelang + ".csv"))
            {
                int n = 0;


                String headline = "";
                headline = sr.ReadLine();

                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();
                    //memo(line);

                    string[] words = line.Split(';');
                    for (int jj = 1; jj < words.Length; jj++)
                    {
                        words[jj] = words[jj].Trim();
                        if (words[jj].Length < 2)
                            words[jj] = "";
                    }

                    if ((words.Length >= 4) && (!groupname_sing.ContainsKey(words[0])))
                    {
                        groupname_sing.Add(words[0], words[1]);
                        groupname_plural.Add(words[0], words[2]);
                        groupname_attr.Add(words[0], words[3]);
                        n++;
                    }

                    //if (name_id.ContainsKey(words[0]))
                    //{
                    //    int taxid = name_id[words[0]];
                    //    if (String.IsNullOrEmpty(taxondict[taxid].Name_sv))
                    //        taxondict[taxid].Name_sv = groupname_plural[words[0]];
                    //    if (!swe_name.ContainsKey(taxid))
                    //        swe_name.Add(taxid, groupname_plural[words[0]]);
                    //}
                }

                //memo("n (groupname) = " + n.ToString());
            }

            List<string> lichenlist = new List<string>();
            lichenlist.Add("Lecanoromycetes");
            lichenlist.Add("Allarthonia"); lichenlist.Add("Arthonia"); lichenlist.Add("Arthothelium"); lichenlist.Add("Coniarthonia"); lichenlist.Add("Cryptothecia"); lichenlist.Add("Sporostigma"); lichenlist.Add("Stirtonia");
            lichenlist.Add("Chrysothrix");
            lichenlist.Add("Melaspilea");
            lichenlist.Add("Bactrospora"); lichenlist.Add("Chiodecton"); lichenlist.Add("Combea"); lichenlist.Add("Cresponea"); lichenlist.Add("Dendrographa"); lichenlist.Add("Dirina"); lichenlist.Add("Enterographa"); lichenlist.Add("Hubbsia"); lichenlist.Add("Lecanactis"); lichenlist.Add("Lecanographa"); lichenlist.Add("Mazosia"); lichenlist.Add("Opegrapha"); lichenlist.Add("Phoebus"); lichenlist.Add("Plectocarpon"); lichenlist.Add("Reinkella"); lichenlist.Add("Roccella"); lichenlist.Add("Roccellina"); lichenlist.Add("Schismatomma"); lichenlist.Add("Schizopelte"); lichenlist.Add("Sclerophyton"); lichenlist.Add("Sigridea"); lichenlist.Add("Syncesia");
            lichenlist.Add("Arthophacopsis"); lichenlist.Add("Llimonaea"); lichenlist.Add("Perigrapha");
            lichenlist.Add("Echinothecium");
            lichenlist.Add("Cystocoleus");
            lichenlist.Add("Clypeococcum"); lichenlist.Add("Dacampia"); lichenlist.Add("Eopyrenula"); lichenlist.Add("Polycoccum"); lichenlist.Add("Pyrenidium");
            lichenlist.Add("Collemopsidium"); lichenlist.Add("Pyrenocollema"); lichenlist.Add("Zwackhiomyces");
            lichenlist.Add("Melanomma");
            lichenlist.Add("Taeniolella");
            lichenlist.Add("Peridiothelia");
            lichenlist.Add("Epicoccum"); lichenlist.Add("Leptosphaerulina");
            lichenlist.Add("Monoblastiopsis"); lichenlist.Add("Phoma");
            lichenlist.Add("Epigloea");
            lichenlist.Add("Arthopyrenia"); lichenlist.Add("Mycomicrothelia");
            lichenlist.Add("Didymosphaeria");
            lichenlist.Add("Lichenostigma"); lichenlist.Add("Lichenothelia");
            lichenlist.Add("Lichenopeltella");
            lichenlist.Add("Sphaerellothecium"); lichenlist.Add("Sphaerulina"); lichenlist.Add("Stigmidium");
            lichenlist.Add("Jarxia"); lichenlist.Add("Leptorhaphis"); lichenlist.Add("Tomasellia");
            lichenlist.Add("Parmularia");
            lichenlist.Add("Myxophora"); lichenlist.Add("Raciborskiomyces"); lichenlist.Add("Wentiomyces");
            lichenlist.Add("Pyrenothrix");
            lichenlist.Add("Protothelenella"); lichenlist.Add("Thrombium");
            lichenlist.Add("Buelliella"); lichenlist.Add("Cercidospora"); lichenlist.Add("Hassea"); lichenlist.Add("Homostegia"); lichenlist.Add("Karschia"); lichenlist.Add("Monodictys"); lichenlist.Add("Mycoglaena"); lichenlist.Add("Mycoporellum"); lichenlist.Add("Rosellinula"); lichenlist.Add("Trematosphaeriopsis");
            lichenlist.Add("Capronia"); lichenlist.Add("Racodium");
            lichenlist.Add("Acrocordia"); lichenlist.Add("Anisomeridium"); lichenlist.Add("Monoblastia");
            lichenlist.Add("Anthracothecium"); lichenlist.Add("Distopyrenis"); lichenlist.Add("Granulopyrenis"); lichenlist.Add("Lithothelium"); lichenlist.Add("Polypyrenula"); lichenlist.Add("Pyrenula"); lichenlist.Add("Pyrgillus"); lichenlist.Add("Sulcopyrenula");
            lichenlist.Add("Requienella");
            lichenlist.Add("Astrothelium"); lichenlist.Add("Bathelium"); lichenlist.Add("Campylothelium"); lichenlist.Add("Laurera"); lichenlist.Add("Polymeridium"); lichenlist.Add("Pseudopyrenula"); lichenlist.Add("Trypethelium");
            lichenlist.Add("Celothelium"); lichenlist.Add("Mycoporum");
            lichenlist.Add("Adelococcus"); lichenlist.Add("Sagediopsis");
            lichenlist.Add("Agonimia"); lichenlist.Add("Bagliettoa"); lichenlist.Add("Bellemerella"); lichenlist.Add("Catapyrenium"); lichenlist.Add("Clavascidium"); lichenlist.Add("Dermatocarpon"); lichenlist.Add("Endocarpon"); lichenlist.Add("Endococcus"); lichenlist.Add("Henrica"); lichenlist.Add("Heterocarpon"); lichenlist.Add("Heteroplacidium"); lichenlist.Add("Involucropyrenium"); lichenlist.Add("Lauderlindsaya"); lichenlist.Add("Leucocarpia"); lichenlist.Add("Merismatium"); lichenlist.Add("Muellerella"); lichenlist.Add("Neocatapyrenium"); lichenlist.Add("Phaeospora"); lichenlist.Add("Placidiopsis"); lichenlist.Add("Placidium"); lichenlist.Add("Placopyrenium"); lichenlist.Add("Polyblastia"); lichenlist.Add("Psoroglaena"); lichenlist.Add("Staurothele"); lichenlist.Add("Thelidium"); lichenlist.Add("Trimmatothele"); lichenlist.Add("Verrucaria");
            lichenlist.Add("Geisleria"); lichenlist.Add("Strigula");
            lichenlist.Add("Chaenothecopsis"); lichenlist.Add("Mycocalicium"); lichenlist.Add("Phaeocalicium"); lichenlist.Add("Stenocybe");
            lichenlist.Add("Sphinctrina");
            lichenlist.Add("Bryoscyphus"); lichenlist.Add("Unguiculariopsis");
            lichenlist.Add("Pezizella");
            lichenlist.Add("Llimoniella"); lichenlist.Add("Phaeopyxis"); lichenlist.Add("Phragmonaevia"); lichenlist.Add("Rhymbocarpus"); lichenlist.Add("Skyttella");
            lichenlist.Add("Myxotrichum");
            lichenlist.Add("Gloeoheppia");
            lichenlist.Add("Heppia"); lichenlist.Add("Solorinaria");
            lichenlist.Add("Anema"); lichenlist.Add("Collemopsis"); lichenlist.Add("Cryptothele"); lichenlist.Add("Digitothyrea"); lichenlist.Add("Ephebe"); lichenlist.Add("Euopsis"); lichenlist.Add("Harpidium"); lichenlist.Add("Lemmopsis"); lichenlist.Add("Lempholemma"); lichenlist.Add("Leprocollema"); lichenlist.Add("Lichina"); lichenlist.Add("Lichinella"); lichenlist.Add("Lichinodium"); lichenlist.Add("Metamelanea"); lichenlist.Add("Paulia"); lichenlist.Add("Peccania"); lichenlist.Add("Phloeopeccania"); lichenlist.Add("Phylliscum"); lichenlist.Add("Porocyphus"); lichenlist.Add("Psorotichia"); lichenlist.Add("Pterygiopsis"); lichenlist.Add("Pyrenopsis"); lichenlist.Add("Stromatella"); lichenlist.Add("Synalissa"); lichenlist.Add("Thelignya"); lichenlist.Add("Thermutis"); lichenlist.Add("Thyrea"); lichenlist.Add("Zahlbrucknerella");
            lichenlist.Add("Peltula");
            lichenlist.Add("Lasiosphaeriopsis"); lichenlist.Add("Rhagadostoma");
            lichenlist.Add("Dendrodochium"); lichenlist.Add("Paranectria"); lichenlist.Add("Pronectria"); lichenlist.Add("Trichonectria");
            lichenlist.Add("Niesslia");
            lichenlist.Add("Illosporiopsis"); lichenlist.Add("Illosporium");
            lichenlist.Add("Graphium");
            lichenlist.Add("Lichenochora");
            lichenlist.Add("Globosphaeria"); lichenlist.Add("Roselliniella"); lichenlist.Add("Roselliniopsis");
            lichenlist.Add("Physalospora");
            lichenlist.Add("Obryzum");
            lichenlist.Add("Neolamya"); lichenlist.Add("Sarcopyrenia"); lichenlist.Add("Thelidiella");
            lichenlist.Add("Lahmia");
            lichenlist.Add("Aspidothelium");
            lichenlist.Add("Kohlmeyera"); lichenlist.Add("Mastodia"); lichenlist.Add("Turgidosculum");
            lichenlist.Add("Julella"); lichenlist.Add("Thelenella");
            lichenlist.Add("Baeomyces");
            lichenlist.Add("Coccotrema");
            lichenlist.Add("Sarcosagium"); lichenlist.Add("Thelocarpon");
            lichenlist.Add("Abrothallus"); lichenlist.Add("Acaroconium"); lichenlist.Add("Bispora"); lichenlist.Add("Cheiromycina"); lichenlist.Add("Coniambigua"); lichenlist.Add("Dictyocatenulata"); lichenlist.Add("Flakea"); lichenlist.Add("Hawksworthiana"); lichenlist.Add("Heterocyphelium"); lichenlist.Add("Hobsoniopsis"); lichenlist.Add("Intralichen"); lichenlist.Add("Kalchbrenneriella"); lichenlist.Add("Kirschsteiniothelia"); lichenlist.Add("Lichenopuccinia"); lichenlist.Add("Minutoexcipula"); lichenlist.Add("Normandina"); lichenlist.Add("Patriciomyces"); lichenlist.Add("Phaeosporobolus"); lichenlist.Add("Refractohilum"); lichenlist.Add("Sclerococcum"); lichenlist.Add("Talpapellis"); lichenlist.Add("Tylophoron"); lichenlist.Add("Vouauxiomyces");
            lichenlist.Add("Acantholichen"); lichenlist.Add("Cyphellostereum"); lichenlist.Add("Dictyonema"); lichenlist.Add("Lichenomphalia");
            lichenlist.Add("Arrhenia"); lichenlist.Add("Fayodia"); lichenlist.Add("Omphalina");
            lichenlist.Add("Semiomphalina");
            lichenlist.Add("Lepidostroma");

            lichenlist.Add("Multiclavula");
            lichenlist.Add("Marchandiomyces");
            lichenlist.Add("Syzygospora");
            lichenlist.Add("Marchandiomphalina");
            lichenlist.Add("Chionosphaera");
            lichenlist.Add("Hobsonia");
            lichenlist.Add("Chrysopsora");
            lichenlist.Add("Biatoropsis"); lichenlist.Add("Cystobasidium");
            lichenlist.Add("Asterophoma"); lichenlist.Add("Bachmanniomyces"); lichenlist.Add("Cornutispora"); lichenlist.Add("Dinemasporium"); lichenlist.Add("Diplolaeviopsis"); lichenlist.Add("Epaphroconidia"); lichenlist.Add("Epicladonia"); lichenlist.Add("Everniicola"); lichenlist.Add("Karsteniomyces"); lichenlist.Add("Laeviomyces"); lichenlist.Add("Lichenoconium"); lichenlist.Add("Lichenodiplis"); lichenlist.Add("Lichenosticta"); lichenlist.Add("Nigropuncta"); lichenlist.Add("Pyrenotrichum"); lichenlist.Add("Rhabdospora"); lichenlist.Add("Vouauxiella"); lichenlist.Add("Xanthopsora");

            foreach (string lav in lichenlist)
            {
                switch (makelang)
                {
                    case "sv":
                        groupname_sing.Add(lav, "lav");
                        groupname_plural.Add(lav, "lavar");
                        groupname_attr.Add(lav, "lav");
                        break;
                    case "en":
                        groupname_sing.Add(lav, "lichen");
                        groupname_plural.Add(lav, "lichens");
                        groupname_attr.Add(lav, "lichen");
                        break;
                    case "ceb":
                        groupname_sing.Add(lav, "lumot");
                        groupname_plural.Add(lav, "mga lumot");
                        groupname_attr.Add(lav, "lumot");
                        break;
                    default:
                        break;
                }
            }

            if (makelang == "sv")
            {

                taxospecial.Add("ordo:Diprotodontia", "infraclassis:Marsupialia&infraclassis_sv:[[Pungdjur]]");
                taxospecial.Add("ordo:Dasyuromorphia", "infraclassis:Marsupialia&infraclassis_sv:[[Pungdjur]]");
                taxospecial.Add("ordo:Didelphimorphia", "infraclassis:Marsupialia&infraclassis_sv:[[Pungdjur]]");
                taxospecial.Add("ordo:Microbiotheria", "infraclassis:Marsupialia&infraclassis_sv:[[Pungdjur]]");
                taxospecial.Add("ordo:Notoryctemorphia", "infraclassis:Marsupialia&infraclassis_sv:[[Pungdjur]]");
                taxospecial.Add("ordo:Paucituberculata", "infraclassis:Marsupialia&infraclassis_sv:[[Pungdjur]]");
                taxospecial.Add("ordo:Peramelemorphia", "infraclassis:Marsupialia&infraclassis_sv:[[Pungdjur]]");
                taxospecial.Add("ordo:Afrosoricida", "infraclassis:Eutheria&infraclassis_sv:[[Högre däggdjur]]");
                taxospecial.Add("ordo:Macroscelidea", "infraclassis:Eutheria&infraclassis_sv:[[Högre däggdjur]]");
                taxospecial.Add("ordo:Tubulidentata", "infraclassis:Eutheria&infraclassis_sv:[[Högre däggdjur]]");
                taxospecial.Add("ordo:Hyracoidea", "infraclassis:Eutheria&infraclassis_sv:[[Högre däggdjur]]");
                taxospecial.Add("ordo:Proboscidea", "infraclassis:Eutheria&infraclassis_sv:[[Högre däggdjur]]");
                taxospecial.Add("ordo:Sirenia", "infraclassis:Eutheria&infraclassis_sv:[[Högre däggdjur]]");
                taxospecial.Add("ordo:Pilosa", "infraclassis:Eutheria&infraclassis_sv:[[Högre däggdjur]]");
                taxospecial.Add("ordo:Cingulata", "infraclassis:Eutheria&infraclassis_sv:[[Högre däggdjur]]");
                taxospecial.Add("ordo:Pholidota", "infraclassis:Eutheria&infraclassis_sv:[[Högre däggdjur]]");
                taxospecial.Add("ordo:Chiroptera", "infraclassis:Eutheria&infraclassis_sv:[[Högre däggdjur]]");
                taxospecial.Add("ordo:Eulipotyphla", "infraclassis:Eutheria&infraclassis_sv:[[Högre däggdjur]]");
                taxospecial.Add("ordo:Carnivora", "infraclassis:Eutheria&infraclassis_sv:[[Högre däggdjur]]");
                taxospecial.Add("ordo:Artiodactyla", "infraclassis:Eutheria&infraclassis_sv:[[Högre däggdjur]]");
                taxospecial.Add("ordo:Cetacea", "infraclassis:Eutheria&infraclassis_sv:[[Högre däggdjur]]");
                taxospecial.Add("ordo:Perissodactyla", "infraclassis:Eutheria&infraclassis_sv:[[Högre däggdjur]]");
                taxospecial.Add("ordo:Dermoptera", "infraclassis:Eutheria&infraclassis_sv:[[Högre däggdjur]]");
                taxospecial.Add("ordo:Primates", "infraclassis:Eutheria&infraclassis_sv:[[Högre däggdjur]]");
                taxospecial.Add("ordo:Scandentia", "infraclassis:Eutheria&infraclassis_sv:[[Högre däggdjur]]");
                taxospecial.Add("ordo:Rodentia", "infraclassis:Eutheria&infraclassis_sv:[[Högre däggdjur]]");
                taxospecial.Add("ordo:Lagomorphia", "infraclassis:Eutheria&infraclassis_sv:[[Högre däggdjur]]");
                taxospecial.Add("classis:Mammalia", "subphylum:Vertebrata&subphylum_sv:[[Ryggradsdjur]]");
                taxospecial.Add("classis:Aves", "subphylum:Vertebrata&subphylum_sv:[[Ryggradsdjur]]");
                taxospecial.Add("classis:Amphibia", "subphylum:Vertebrata&subphylum_sv:[[Ryggradsdjur]]");
                taxospecial.Add("classis:Actinopterygii", "subphylum:Vertebrata&subphylum_sv:[[Ryggradsdjur]]&superclassis:Osteichthyes&superclassis_sv:[[Benfiskar]]");
                taxospecial.Add("classis:Sarcopterygii", "subphylum:Vertebrata&subphylum_sv:[[Ryggradsdjur]]&superclassis:Osteichthyes&superclassis_sv:[[Benfiskar]]");
                taxospecial.Add("classis:Elasmobranchii", "subphylum:Vertebrata&subphylum_sv:[[Ryggradsdjur]]&superclassis:Chondrichthyes&superclassis_sv:[[Broskfiskar]]");
                taxospecial.Add("classis:Holocephali", "subphylum:Vertebrata&subphylum_sv:[[Ryggradsdjur]]&superclassis:Chondrichthyes&superclassis_sv:[[Broskfiskar]]");
                taxospecial.Add("classis:Reptilia", "subphylum:Vertebrata&subphylum_sv:[[Ryggradsdjur]]");
                taxospecial.Add("ordo:Sarcoptiformes", "subclassis:Acari&subclassis_sv:[[Kvalster]]");
                taxospecial.Add("ordo:Trombidiformes", "subclassis:Acari&subclassis_sv:[[Kvalster]]");
                taxospecial.Add("ordo:Ixodida", "subclassis:Acari&subclassis_sv:[[Kvalster]]");
                taxospecial.Add("classis:Diplopoda", "subphylum:Myriapoda&subphylum_sv:[[Mångfotingar]]");
                taxospecial.Add("classis:Chilopoda", "subphylum:Myriapoda&subphylum_sv:[[Mångfotingar]]");
                taxospecial.Add("classis:Pauropoda", "subphylum:Myriapoda&subphylum_sv:[[Mångfotingar]]");
                taxospecial.Add("classis:Symphyla", "subphylum:Myriapoda&subphylum_sv:[[Mångfotingar]]");
                taxospecial.Add("classis:Malacostraca", "subphylum:Crustacea&subphylum_sv:[[Kräftdjur]]");
                taxospecial.Add("classis:Ostracoda", "subphylum:Crustacea&subphylum_sv:[[Kräftdjur]]");
                taxospecial.Add("classis:Maxillopoda", "subphylum:Crustacea&subphylum_sv:[[Kräftdjur]]");
                taxospecial.Add("classis:Branchiopoda", "subphylum:Crustacea&subphylum_sv:[[Kräftdjur]]");
                taxospecial.Add("classis:Remipedia", "subphylum:Crustacea&subphylum_sv:[[Kräftdjur]]");
                taxospecial.Add("classis:Cephalocarida", "subphylum:Crustacea&subphylum_sv:[[Kräftdjur]]");
                taxospecial.Add("classis:Insecta", "subphylum:Hexapoda&subphylum_sv:[[Sexfotingar]]");
                taxospecial.Add("classis:Diplura", "subphylum:Hexapoda&subphylum_sv:[[Sexfotingar]]");
                taxospecial.Add("classis:Collembola", "subphylum:Hexapoda&subphylum_sv:[[Sexfotingar]]");
                taxospecial.Add("classis:Entognatha", "subphylum:Hexapoda&subphylum_sv:[[Sexfotingar]]");
            }
            else
            {

                taxospecial.Add("ordo:Diprotodontia", "infraclassis:[[Marsupialia]]");
                taxospecial.Add("ordo:Dasyuromorphia", "infraclassis:[[Marsupialia]]");
                taxospecial.Add("ordo:Didelphimorphia", "infraclassis:[[Marsupialia]]");
                taxospecial.Add("ordo:Microbiotheria", "infraclassis:[[Marsupialia]]");
                taxospecial.Add("ordo:Notoryctemorphia", "infraclassis:[[Marsupialia]]");
                taxospecial.Add("ordo:Paucituberculata", "infraclassis:[[Marsupialia]]");
                taxospecial.Add("ordo:Peramelemorphia", "infraclassis:[[Marsupialia]]");
                taxospecial.Add("ordo:Afrosoricida", "infraclassis:[[Eutheria]]");
                taxospecial.Add("ordo:Macroscelidea", "infraclassis:[[Eutheria]]");
                taxospecial.Add("ordo:Tubulidentata", "infraclassis:[[Eutheria]]");
                taxospecial.Add("ordo:Hyracoidea", "infraclassis:[[Eutheria]]");
                taxospecial.Add("ordo:Proboscidea", "infraclassis:[[Eutheria]]");
                taxospecial.Add("ordo:Sirenia", "infraclassis:[[Eutheria]]");
                taxospecial.Add("ordo:Pilosa", "infraclassis:[[Eutheria]]");
                taxospecial.Add("ordo:Cingulata", "infraclassis:[[Eutheria]]");
                taxospecial.Add("ordo:Pholidota", "infraclassis:[[Eutheria]]");
                taxospecial.Add("ordo:Chiroptera", "infraclassis:[[Eutheria]]");
                taxospecial.Add("ordo:Eulipotyphla", "infraclassis:[[Eutheria]]");
                taxospecial.Add("ordo:Carnivora", "infraclassis:[[Eutheria]]");
                taxospecial.Add("ordo:Artiodactyla", "infraclassis:[[Eutheria]]");
                taxospecial.Add("ordo:Cetacea", "infraclassis:[[Eutheria]]");
                taxospecial.Add("ordo:Perissodactyla", "infraclassis:[[Eutheria]]");
                taxospecial.Add("ordo:Dermoptera", "infraclassis:[[Eutheria]]");
                taxospecial.Add("ordo:Primates", "infraclassis:[[Eutheria]]");
                taxospecial.Add("ordo:Scandentia", "infraclassis:[[Eutheria]]");
                taxospecial.Add("ordo:Rodentia", "infraclassis:[[Eutheria]]");
                taxospecial.Add("ordo:Lagomorphia", "infraclassis:[[Eutheria]]");
                taxospecial.Add("classis:Mammalia", "subphylum:[[Vertebrata]]");
                taxospecial.Add("classis:Aves", "subphylum:[[Vertebrata]]");
                taxospecial.Add("classis:Amphibia", "subphylum:[[Vertebrata]]");
                taxospecial.Add("classis:Actinopterygii", "subphylum:[[Vertebrata]]&superclassis:[[Osteichthyes]]");
                taxospecial.Add("classis:Sarcopterygii", "subphylum:[[Vertebrata]]&superclassis:[[Osteichthyes]]");
                taxospecial.Add("classis:Elasmobranchii", "subphylum:[[Vertebrata]]&superclassis:[[Chondrichthyes]]");
                taxospecial.Add("classis:Holocephali", "subphylum:[[Vertebrata]]&superclassis:[[Chondrichthyes]]");
                taxospecial.Add("classis:Reptilia", "subphylum:[[Vertebrata]]");
                taxospecial.Add("ordo:Sarcoptiformes", "subclassis:[[Acari]]");
                taxospecial.Add("ordo:Trombidiformes", "subclassis:[[Acari]]");
                taxospecial.Add("ordo:Ixodida", "subclassis:[[Acari]]");
                taxospecial.Add("classis:Diplopoda", "subphylum:[[Myriapoda]]");
                taxospecial.Add("classis:Chilopoda", "subphylum:[[Myriapoda]]");
                taxospecial.Add("classis:Pauropoda", "subphylum:[[Myriapoda]]");
                taxospecial.Add("classis:Symphyla", "subphylum:[[Myriapoda]]");
                taxospecial.Add("classis:Malacostraca", "subphylum:[[Crustacea]]");
                taxospecial.Add("classis:Ostracoda", "subphylum:[[Crustacea]]");
                taxospecial.Add("classis:Maxillopoda", "subphylum:[[Crustacea]]");
                taxospecial.Add("classis:Branchiopoda", "subphylum:[[Crustacea]]");
                taxospecial.Add("classis:Remipedia", "subphylum:[[Crustacea]]");
                taxospecial.Add("classis:Cephalocarida", "subphylum:[[Crustacea]]");
                taxospecial.Add("classis:Insecta", "subphylum:[[Hexapoda]]");
                taxospecial.Add("classis:Diplura", "subphylum:[[Hexapoda]]");
                taxospecial.Add("classis:Collembola", "subphylum:[[Hexapoda]]");
                taxospecial.Add("classis:Entognatha", "subphylum:[[Hexapoda]]");
            }

        }





        public void read_IUCN()
        {
            using (StreamReader sr = new StreamReader(dyntaxafolder + "iucn-allspecies-export-34317.csv"))
            {
                int n = 0;
                int nfound = 0;
                int imax = 0;


                String headline = "";
                headline = sr.ReadLine();
                headline = sr.ReadLine();

                string[] parents = new string[6];
                parents[0] = "";
                parents[1] = "";
                parents[2] = "";
                parents[3] = "";
                parents[4] = "";
                parents[5] = "";

                string[] ranks = new string[7];
                ranks[0] = "regnum";
                ranks[1] = "phylum";
                ranks[2] = "classis";
                ranks[3] = "ordo";
                ranks[4] = "familia";
                ranks[5] = "genus";
                ranks[6] = "species";

                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();
                    //memo(line);

                    string[] words = line.Split(';');
                    for (int jj = 1; jj < words.Length; jj++)
                    {
                        words[jj] = words[jj].Trim();
                        if (words[jj].Length < 2)
                            words[jj] = "";
                    }

                    int i = Convert.ToInt32(words[0]);
                    if (i > imax)
                        imax = i;

                    n++;

                    string name = words[6] + " " + words[7]; ;
                    string name_en = words[14];
                    string name_fr = words[15];
                    string name_es = words[16];
                    string iucnurl = "http://www.iucnredlist.org/details/" + words[0] + "/0";
                    string iucnauk = words[8];
                    string iucnstatus = words[17];
                    string iucnyear = words[20];
                    string iucnpop = words[21];
                    string iucncrit = words[18];

                    if (iucnstatus.Contains("/"))
                    {
                        string[] st = iucnstatus.Split('/');
                        iucnstatus = st[1].ToUpper();
                    }

                    for (int jj = 0; jj < 6; jj++)
                        parents[jj] = util.initialcap(words[jj + 1].ToLower());

                    IUCNClass ic = new IUCNClass();
                    ic.Name = name;
                    ic.status = iucnstatus;
                    ic.year = iucnyear;
                    ic.pop = iucnpop;
                    ic.criteria = iucncrit;

                    iucndict.Add(i, ic);
                    iucnnamedict.Add(ic.Name, i);

                    //int taxid = -1;
                    //int taxid = get_name_id(name, "species", "");
                    //if (name_id.ContainsKey(name))
                    //    taxid = name_id[name];
                    //if (taxid < 0)
                    //    if (synonym_name_taxon.ContainsKey(name))
                    //        taxid = synonym_name_taxon[name];

                    //if ((taxid > 0) && (taxondict[taxid].regnum.ToLower() == parents[0].ToLower()))
                    //{
                    //    nfound++;
                    //    taxondict[taxid].iucnid = i;
                    //    if (name != taxondict[taxid].Name)
                    //        taxondict[taxid].iucn_name = name;
                    //    taxondict[taxid].iucn_auktor = iucnauk;
                    //}

                    if (n % nprint == 0)
                        memo("n (IUCN) = " + n.ToString());


                }

                memo("n (IUCN) = " + n.ToString());
                memo("nfound (IUCN) = " + nfound.ToString());
            }



        }


        public static string addref(string rn, string rstring)
        {
            string refname = "\"" + rn + "\"";
            if (!refnamelist.Contains(refname))
            {
                refnamelist.Add(refname);

                string refref = "<ref name = " + refname + ">" + rstring + "</ref>";
                reflist += "\n" + refref;
            }
            string shortref = "<ref name = " + refname + "/>";
            return shortref;

        }


        public string makeref(int taxonID)
        {
            //input is index to CoL reference list.
            string refref = "";
            //if (!references.ContainsKey(iref))
            //    return refref;
            //else
            //{

            string refname = "\"col" + taxonID.ToString() + "\"";
            //string shortref = "";
            if (!refnamelist.Contains(refname))
            {
                refnamelist.Add(refname);
                var q = from c in db.Reference where c.TaxonID == taxonID select c;
                if (q.Count() == 0)
                    return "";

                Reference dbref = q.First();

                string titlestring = dbref.Title;
                if (!String.IsNullOrEmpty(titlestring))
                    titlestring = " ''" + titlestring + "'' ";
                else
                    titlestring = "";

                string yearstring = dbref.Date;
                if (!String.IsNullOrEmpty(yearstring))
                    yearstring = " (" + yearstring + ") ";
                else
                    yearstring = "";

                string descstring = dbref.Description;
                if (String.IsNullOrEmpty(descstring))
                    descstring = "";

                refref += "<ref name = " + refname + ">" + dbref.Creator + yearstring + titlestring + ", " + descstring + "</ref>";
                reflist += "\n" + refref;
            }
            string shortref = "<ref name = " + refname + "/>";
            return shortref;
            //}
        }


        public string makesynparam(int taxonid, string ptitle, string syncat)
        {
            //return "DUMMY";
            
            var q = from c in db.Taxon
                    where c.AcceptedNameUsageID == taxonid
                    select c;

            int synfound = 0;
            try
            {
                synfound = q.Count();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception " + e.Message);
            }

            if (synfound == 0)
            {
                memo("No synonyms in CoL");
                return "";
            }
            else
            {
                memo(synfound + " synonyms found.");
                string synparam = "";

                SortedDictionary<string, string> synsort = new SortedDictionary<string, string>();

                foreach (Taxon syn in q)
                {
                    //Check if synonym exists as valid taxon.

                    string synname = util.dbtaxon_name(syn);
                    //var q2 = null;
                    if (syn.TaxonRank == "species")
                    {
                        var q2 = from c in db.Taxon
                                 where c.Genus == syn.Genus
                                 where c.SpecificEpithet == syn.SpecificEpithet
                                 where c.TaxonomicStatus == "accepted name"
                                 select c;
                        if (q2.Count() > 0)
                            continue;
                    }
                    else
                    {
                        var q2 = from c in db.Taxon
                                 where c.ScientificName == syn.ScientificName
                                 where c.TaxonomicStatus == "accepted name"
                                 select c;
                        if (q2.Count() > 0)
                            continue;
                    }

                    //********

                    string synref = makeref(syn.TaxonID);

                    //if (synonym_ref.ContainsKey(synid))
                    //{
                    //    foreach (int rr in synonym_ref[synid])
                    //    {
                    //        synref += makeref(rr);
                    //    }
                    //    if (synonym_ref[synid].Count > 10)
                    //        synref = "";
                    //}

                    string auktor = "";
                    string aukyear = "";

                    if (syn.ScientificNameAuthorship != null)
                    {
                        auktor = syn.ScientificNameAuthorship;
                        //get year from auktor:
                        Regex rex = new Regex(@"\d{4}");
                        Match m = rex.Match(auktor);
                        aukyear = m.Value;
                    }
                    //if ((synonym_author.ContainsKey(synid)) && (author.ContainsKey(synonym_author[synid])))
                    //{
                    //    auktor = author[synonym_author[synid]];
                    //    auktor = auktor.Replace("..", ".");
                    //}

                    //String aukyear = "";
                    //if ((auktor.IndexOf('1') >= 0) && ((auktor.IndexOf('2') < 0) || (auktor.IndexOf('1') < auktor.IndexOf('2'))))

                    //{
                    //    aukyear = auktor.Substring(auktor.IndexOf('1'));

                    //}
                    //if ((String.IsNullOrEmpty(aukyear)) && (auktor.IndexOf('2') >= 0))
                    //{
                    //    aukyear = auktor.Substring(auktor.IndexOf('2'));

                    //}


                    string onesyn = "''" + synname + "'' <small>" + auktorclass.boxauthor(auktor) + "</small>" + synref;

                    while (synsort.ContainsKey(aukyear))
                        aukyear += "a";
                    synsort.Add(aukyear, onesyn);

                    util.make_redirect(testprefix+synname, ptitle, syncat, reallymake && !CBmemo.Checked);


                }

                foreach (string sy in synsort.Keys)
                {
                    if (synparam != "")
                        synparam = "<br>" + synparam;
                    synparam = synsort[sy] + synparam;
                }

                return synparam;
            }

        }

        public int countarticle(int taxonid)
        {
            return countarticle(taxonid, false);
        }

        public int countarticle(int taxonid, bool speciesonly)
        {
            //int n = 0;
            memo("countarticle taxonid = " + taxonid);
            Taxon tt = (from c in db.Taxon where c.TaxonID == taxonid select c).FirstOrDefault();
            if (tt == null)
                return -1;


            memo("tt " + tt.ScientificName+" "+tt.TaxonRank);

            if (tt.TaxonRank == "species")
                return 1;

            if (isincert(tt.ScientificName))
                return 0;

            IEnumerable<Taxon> q = null;
            switch (tt.TaxonRank)
            {
                case "kingdom":
                    q = from c in db.Taxon where c.Kingdom == tt.ScientificName where c.InfraspecificEpithet == null select c;
                    break;
                case "phylum":
                    q = from c in db.Taxon where c.Phylum == tt.ScientificName where c.InfraspecificEpithet == null select c;
                    break;
                case "class":
                    q = from c in db.Taxon where c.Class == tt.ScientificName where c.InfraspecificEpithet == null select c;
                    break;
                case "order":
                    q = from c in db.Taxon where c.Order == tt.ScientificName where c.InfraspecificEpithet == null select c;
                    break;
                case "superfamily":
                    q = from c in db.Taxon where c.Superfamily == tt.ScientificName where c.InfraspecificEpithet == null select c;
                    break;
                case "family":
                    q = from c in db.Taxon where c.Family == tt.ScientificName where c.InfraspecificEpithet == null select c;
                    break;
                case "genus":
                    q = from c in db.Taxon where c.Genus == tt.ScientificName where c.InfraspecificEpithet == null select c;
                    break;
                default:
                    memo("Rank not found");
                    break;
            }

            int speciescount = (from c in q where c.TaxonRank == "species" where c.TaxonomicStatus == "accepted name" select c).Count();
            if (speciesonly)
            {
                return speciescount; 
            }

            int highercount = (from c in q where c.SpecificEpithet == null select c).Count();

            if ((itodo > 0) && (highercount+speciescount > itodo+10))
            {
                throw new Exception("Funny countarticle result");
            }
            
            return highercount+speciescount;

            //if (!taxondict.ContainsKey(taxonid))
            //    return;

            //string stitle = taxondict[taxonid].Name;

            ////skip taxon trees already done:
            //if (donetree.Contains(stitle))
            //    return;

            //itodo++;

            ////if (taxon_namestatus.ContainsKey(taxonid) && taxon_namestatus[taxonid] == 4)
            ////    istatus4++;

            //string rank = rank_name[taxondict[taxonid].Level];
            ////memo("name, rank = " + taxondict[taxonid].Name + ", " + rank);

            //if (!rank_order.ContainsKey(rank))
            //{
            //    memo("name, rank = " + taxondict[taxonid].Name + ", " + rank);
            //    return;
            //}

            //if (rank_order[rank] > rank_order[bottomrank]) //Don't count articles for ranks below bottomrank
            //    if (dynloop)
            //    {
            //        foreach (int j in taxondict[taxonid].dyntaxa_Children)
            //            countarticle(j);
            //    }
            //    else //loop over COL-taxa
            //    {
            //        foreach (int j in taxondict[taxonid].Children)
            //            countarticle(j);
            //    }

        }

        public int countspecies(int taxonidpar)
        {
            return countarticle(taxonidpar, true);
            //int taxonid = taxonidpar;
            //if (!taxondict.ContainsKey(taxonid))
            //{
            //    //if (id1311.ContainsKey(taxonid))
            //    //    taxonid = id1311[taxonid];
            //    //else
            //    //{
            //    //    XmlNode taxonnode = getxmlfromid(taxonid);
            //    //    if (taxonnode != null)
            //    //        countspeciesxml(taxonnode);
            //    //    else
            //    //    {
            //    //        memo("Taxonnode = null! <return>");
            //    //        Console.ReadLine();
            //    //    }
            //    //    return;
            //    //}
            //    return;
            //}

            //string stitle = taxondict[taxonid].Name;

            //string rank = rank_name[taxondict[taxonid].Level];
            //memo("name, rank, kids = " + taxondict[taxonid].Name + ", " + rank + ", " + taxondict[taxonid].Children13.Count.ToString());

            //if (!rank_order.ContainsKey(rank))
            //{
            //    memo("name, rank ERROR = " + taxondict[taxonid].Name + ", " + rank);
            //    Console.ReadLine();
            //    return;
            //}

            //if (rank == "species")
            //{
            //    ispecies++;
            //    memo("ispecies++" + ispecies.ToString());

            //}
            //else if (rank_order[rank] > rank_order["species"]) //Don't count articles for ranks below bottomrank
            //{
            //    if (dynloop)
            //    {
            //        foreach (int j in taxondict[taxonid].dyntaxa_Children)
            //            countspecies(j);
            //    }
            //    else //loop over COL-taxa
            //    {
            //        foreach (int j in taxondict[taxonid].Children13)
            //            countspecies(j);
            //    }
            //}

        }

        public void findcat(int taxonid)
        {
            if (!taxondict.ContainsKey(taxonid))
            {
                //memo("taxonid not found in COL 2011");
                Taxonclass tc = Taxonclass.loadtaxon(taxonid,db,makelang);
                if (tc != null)
                {
                    taxondictadd(taxonid, tc);
                    found11 = true;
                }
                //return;
            }

            if (!taxondict.ContainsKey(taxonid))
                return;

            Page pcat = new Page(makesite, mp(1, null) + taxondict[taxonid].Name);

            util.tryload(pcat, 3);
            if (pcat.Exists())
            {
                int itry = 0;
                while (itry < 3)
                {
                    try
                    {
                        plregnum.FillFromCategoryTree(taxondict[taxonid].Name);
                        break;
                    }
                    catch (Exception e)
                    {
                        string message = e.Message;
                        Console.Error.WriteLine(message);
                        itry++;
                    }
                }

            }
            else
                findcat(taxondict[taxonid].Parent);
        }

        public void findswecat(int taxonid)
        {
            if (!taxondict.ContainsKey(taxonid))
            {
                //memo("taxonid not found in COL 2011");
                Taxonclass tc = Taxonclass.loadtaxon(taxonid,db, makelang);
                if (tc != null)
                {
                    taxondictadd(taxonid, tc);
                    found11 = true;
                }
                //return;
            }

            if (!taxondict.ContainsKey(taxonid))
                return;

            if (!String.IsNullOrEmpty(taxondict[taxonid].Name_sv))
            {
                Page pcat = new Page(makesite, mp(1, null) + taxondict[taxonid].Name_sv);
                util.tryload(pcat, 3);
                if (pcat.Exists())
                {
                    int itry = 0;
                    while (itry < 3)
                    {
                        try
                        {
                            plregnum.FillFromCategoryTree(taxondict[taxonid].Name_sv);
                            break;
                        }
                        catch (Exception e)
                        {
                            string message = e.Message;
                            Console.Error.WriteLine(message);
                            itry++;
                        }
                    }
                }
                //else
                //    findswecat(taxondict[taxonid].Parent);
            }
            //else
            //    findswecat(taxondict[taxonid].Parent);

        }

        public void findsyncat(int taxonid)
        {
            if (!taxondict.ContainsKey(taxonid))
            {
                //memo("taxonid not found in COL 2011");
                Taxonclass tc = Taxonclass.loadtaxon(taxonid,db, makelang);
                if (tc != null)
                {
                    taxondictadd(taxonid, tc);
                    found11 = true;
                }
                //return;
            }

            if (!taxondict.ContainsKey(taxonid))
                return;

            if ((rank_order[rank_name[taxondict[taxonid].Level]] >= rank_order["ordo"]) && groupname_attr.ContainsKey(taxondict[taxonid].Name))
            {
                string[] p57 = new string[1] { groupname_attr[taxondict[taxonid].Name] };
                Page pcat = new Page(makesite, mp(1, null) + mp(57, p57));
                memo("syncat = " + pcat.title);

                util.tryload(pcat, 1);
                if (pcat.Exists())
                    plregnum.FillFromCategoryTree(pcat.title);

            }
            //else
            //    findsyncat(taxondict[taxonid].Parent);
        }

        public static bool isincert(string name)
        {
            string incert1 = "not assigned";
            string incert2 = "incertae sedis";
            string incert3 = "dummy";
            string incert4 = "unassigned";
            return (name.ToLower().Contains(incert1) || name.ToLower().Contains(incert2) || name.ToLower().Contains(incert3) || name.ToLower().Contains(incert4));

        }

        public static string linktaxon(int taxonid)
        {
            string show = taxondict[taxonid].Name;
            string link = taxondict[taxonid].Name;

            if (!String.IsNullOrEmpty(taxondict[taxonid].articlename))
            {
                link = taxondict[taxonid].articlename;

            }
            if (!String.IsNullOrEmpty(taxondict[taxonid].Name_sv))
            {
                show = taxondict[taxonid].Name_sv;

            }

            if (link == show)
                return "[[" + link + "]]";
            else
                return "[[" + link + "|" + show + "]]";

        }

        public static string linklatin(int taxonid)
        {
            //memo("linklatin");
            string show = taxondict[taxonid].Name;
            string link = taxondict[taxonid].Name;

            if (!String.IsNullOrEmpty(taxondict[taxonid].articlename))
            {
                link = taxondict[taxonid].articlename;

            }

            if (link == show)
                return "[[" + link + "]]";
            else
                return "[[" + link + "|" + show + "]]";

        }

        public static void printtree(int taxonid, int level, int depth)
        {
            //if (level > depth)
            //    return;
            //if (rank_order[rank_name[taxondict[taxonid].Level]] < rank_order[bottomrank])
            //{
            //    Console.WriteLine(taxondict[taxonid].Name + " below bottomrank.");
            //    return;
            //}

            //string ggname = regnum_to_make;
            //if (groupname_sing.ContainsKey(taxondict[taxonid].Name))
            //    ggname = taxondict[taxonid].Name;
            //else
            //{


            //    int[] parent = new int[20];
            //    string incert1 = "Not assigned";


            //    int jj = 0;
            //    int jjmax = -1;

            //    for (int ij = 0; ij < 20; ij++)
            //    {
            //        parent[ij] = -1;
            //    }


            //    parent[jj] = taxondict[taxonid].Parent;
            //    while ((parent[jj] > 0) && taxondict[parent[jj]].Name.Contains(incert1))
            //        parent[jj] = taxondict[parent[jj]].Parent;

            //    while (parent[jj] > 0)
            //    {
            //        parent[jj + 1] = taxondict[parent[jj]].Parent;
            //        if (parent[jj + 1] > 0)
            //            while (taxondict[parent[jj + 1]].Name.Contains(incert1))
            //            {
            //                parent[jj + 1] = taxondict[parent[jj + 1]].Parent;
            //                Console.WriteLine("Parent loop: " + parent[jj + 1].ToString() + taxondict[parent[jj + 1]].Name);
            //            }

            //        jjmax = jj;
            //        jj++;

            //    }

            //    for (int ij = 0; ij < 20; ij++)
            //    {
            //        if ((parent[ij] > 0) && (groupname_sing.ContainsKey(taxondict[parent[ij]].Name)))
            //        {
            //            ggname = taxondict[parent[ij]].Name;
            //            break;
            //        }
            //    }
            //}



            //string stars = "";
            //for (int j = 0; j < level; j++)
            //    stars += "*";
            //string regionstring = "; Region: ";
            //if (taxon_region.ContainsKey(taxonid))
            //{
            //    foreach (int r in taxon_region[taxonid])
            //    {
            //        regionstring += region_name[r];
            //        regionstring += "; ";
            //    }
            //}
            //else if (taxon_region_free.ContainsKey(taxonid))
            //{
            //    foreach (int r in taxon_region_free[taxonid])
            //    {
            //        regionstring += region_free[r];
            //        regionstring += "; ";
            //    }
            //}
            //else
            //{
            //    regionstring = "";
            //}

            //string commonstring = "; Common names: ";
            //if (taxon_common_name.ContainsKey(taxonid))
            //{
            //    foreach (int r in taxon_common_name[taxonid])
            //    {
            //        commonstring += common_name[common_name_element[r]];
            //        commonstring += "; ";
            //    }
            //}
            //else
            //{
            //    commonstring = "";
            //}
            //if (swe_name.ContainsKey(taxonid))
            //    commonstring += swe_name[taxonid];

            //if (String.IsNullOrEmpty(commonstring))
            //{
            //    Page pp = new Page(makesite, taxondict[taxonid].Name);
            //    tryload(pp, 1);
            //    if (pp.Exists())
            //        if (pp.IsRedirect())
            //            commonstring = pp.RedirectsTo();
            //}

            //string parentstring = "";
            //if (taxondict[taxonid].Parent > 0)
            //    parentstring = taxondict[taxondict[taxonid].Parent].Name;

            //string refstring = "";
            //if (taxon_ref.ContainsKey(taxonid))
            //{
            //    foreach (int iref in taxon_ref[taxonid])
            //    {
            //        refstring += references[iref].author + " (" + references[iref].year + ") " + references[iref].title + "; " + references[iref].text + "; " + references[iref].uri_id.ToString();

            //    }
            //}

            //ptree.text += stars + " [[" + taxondict[taxonid].Name + "]]; " + rank_name[taxondict[taxonid].Level] + " [[" + commonstring + "]]" + regionstring + " \"" + groupname_sing[ggname] + "\"" + "\n";
            //Console.WriteLine(stars + " [[" + taxondict[taxonid].Name + "]]; " + rank_name[taxondict[taxonid].Level] + " [[" + commonstring + "]]" + regionstring + " \"" + groupname_sing[ggname] + "\"");
            ////Console.WriteLine(taxondict[taxonid].Name + ";" + commonstring + ";" + rank_name[taxondict[taxonid].Level] + ";" + parentstring + "; " + refstring);

            //if (rank_order[rank_name[taxondict[taxonid].Level]] > rank_order[bottomrank]) //Don't make articles for ranks below bottomrank

            //    foreach (int j in taxondict[taxonid].Children)
            //        printtree(j, level + 1, depth);
        }



        public static void name_id_add(string name, int taxonid)
        {
            if (name_id.ContainsKey(name))
                name_id[name].Add(taxonid);
            else
            {
                List<int> it = new List<int>();
                it.Add(taxonid);
                name_id.Add(name, it);
            }

        }

        public static void taxondictadd(int taxid, Taxonclass tc)
        {
            if ( !taxondict.ContainsKey(taxid))
            {
                taxondict.Add(taxid, tc);
            }
        }


        //public static bool human_touched(Page p, Site site) //determines if an article has been edited by a human user with account (not ip or bot)
        //{
        //    string xmlSrc;
        //    try
        //    {
        //        xmlSrc = site.PostDataAndGetResult(site.address + "/w/api.php", "action=query&format=xml&prop=revisions&titles=" + WebUtility.UrlEncode(p.title) + "&rvlimit=20&rvprop=user");
        //    }
        //    catch (Exception e)
        //    {
        //        string message = e.Message;
        //        Console.Error.WriteLine(message);
        //        return true;
        //    }

        //    XmlDocument xd = new XmlDocument();
        //    xd.LoadXml(xmlSrc);

        //    XmlNodeList elemlist = xd.GetElementsByTagName("rev");

        //    Console.WriteLine("elemlist.Count = " + elemlist.Count);
        //    //Console.WriteLine(xmlSrc);
        //    bool humantouch = false;

        //    foreach (XmlNode ee in elemlist)
        //    {

        //        try
        //        {

        //            string username = ee.Attributes.GetNamedItem("user").Value;
        //            Console.WriteLine(username);
        //            if (username.ToLower().Contains("bot"))
        //            {
        //                continue;
        //            }
        //            if (username == "CommonsDelinker")
        //            {
        //                continue;
        //            }
        //            if (util.get_alphabet(username) == "none")
        //            {
        //                continue;
        //            }

        //        }
        //        catch (NullReferenceException e)
        //        {
        //            string message = e.Message;
        //            Console.Error.WriteLine(message);
        //        }
        //        return true;
        //    }

        //    return false;
        //}

        public bool taxondict_from_taxonid(int taxonid)
        {
            if (taxondict.ContainsKey(taxonid))
                return true;
            Taxonclass tc = Taxonclass.loadtaxon(taxonid,db, makelang);
            if (tc != null)
            {
                taxondictadd(taxonid, tc);
                return true;
            }
            else
                return false;
        }

        public int get_name_id(string topname, string rank, string parent)
        {
            return get_name_id(topname, rank, parent, db);
        }


        public static int get_name_id(string topname, string rank, string parent, COL2019 db)
        {
            var q = from c in db.Taxon
                    where c.ScientificName == topname
                    where (c.TaxonomicStatus == null) || (c.TaxonomicStatus == "accepted name")
                    select c;
            if ( q.Count() == 0)
            {
                string[] nameparts = topname.Split();
                if (nameparts.Length > 1)
                    q = from c in db.Taxon
                        where c.Genus == nameparts[0]
                        where c.SpecificEpithet == nameparts[1]
                        where (c.TaxonomicStatus == null) || (c.TaxonomicStatus == "accepted name")
                        where c.TaxonRank == "species"
                        select c;
            }
            if (q.Count() == 0)
            {
                //memo(topname + " not found in database.");
                return -2;
            }
            else if (q.Count() > 1)
            {
                q = from c in q where c.Kingdom == regnum_to_make select c;
            }
            
            if (q.Count() == 1)
            {
                Taxon tt = q.First();
                Taxonclass tc = new Taxonclass(tt, db, makelang);
                if (tc != null)
                {
                    taxondictadd(tt.TaxonID, tc);
                    return tt.TaxonID;
                }
            }
            //else if (q.Count() > 1)
                //memo(topname + " found " + q.Count() + " times in database.");

            return -1;

            ////if (name == "Ctenophora")
            ////    memo("get_name_id (Ctenophora," + rank + parent+")");
            //int iret = -1;
            //if (isincert(name))
            //    return iret;

            //if (name_id.ContainsKey(name))
            //{
            //    //if (name == "Ctenophora")
            //    //    memo("count = "+name_id[name].Count.ToString());
            //    if (name_id[name].Count == 1)
            //        iret = name_id[name][0];
            //    else
            //    {
            //        int ifound = 0;
            //        foreach (int i in name_id[name])
            //        {
            //            if (taxondict.ContainsKey(i) && taxondict[i].regnum == regnum_to_make)
            //            {
            //                ifound++;
            //                iret = i;
            //            }
            //        }
            //        //if (name == "Ctenophora")
            //        //    memo("ifound (regnum) = "+ifound.ToString());
            //        if (ifound > 1)
            //        {
            //            ifound = 0;
            //            iret = -2;
            //            foreach (int i in name_id[name])
            //            {
            //                if (taxondict.ContainsKey(i) && (taxondict[i].regnum == regnum_to_make) && (rank_name[taxondict[i].Level] == rank))
            //                {
            //                    //if (name == "Ctenophora")
            //                    //    memo("i, name, parent = "+i.ToString()+taxondict[i].Name+taxondict[taxondict[i].Parent].Name);

            //                    iret = i;
            //                    ifound++;
            //                }
            //            }
            //            //if (name == "Ctenophora")
            //            //    memo("ifound (rank) = " + ifound.ToString());
            //            if (ifound > 1)
            //            {
            //                ifound = 0;
            //                iret = -2;
            //                foreach (int i in name_id[name])
            //                {
            //                    if (taxondict.ContainsKey(i) && taxondict.ContainsKey(taxondict[i].Parent) && (taxondict[taxondict[i].Parent].Name == parent) && (taxondict[i].regnum == regnum_to_make) && (rank_name[taxondict[i].Level] == rank))
            //                    {
            //                        iret = i;
            //                        ifound++;
            //                    }
            //                }
            //                //if (name == "Ctenophora")
            //                //    memo("ifound (parent) = " + ifound.ToString());

            //                if (ifound > 1)
            //                {
            //                    iret = -2;
            //                }


            //            }


            //        }
            //    }
            //}
            //if (name == "Ctenophora")
            //{
            //    memo("iret = " + iret.ToString());
            //    //Console.ReadLine();
            //}
            //if (!taxondict.ContainsKey(iret))
            //    iret = -1;
            ////else if (taxondict[iret].regnum != regnum_to_make)
            ////    iret = -1;

            //return -1;
        }

        //public int add_taxon(string name, string parent, string rank)
        //{
        //    Taxonclass taxonarray = new Taxonclass();

        //    int j = dyntaxadiff - 1;
        //    while (taxondict.ContainsKey(j))
        //        j--;

        //    taxonarray.Name = name;
        //    taxonarray.taxonid = j;
        //    taxonarray.Level = name_rank[rank];

        //    taxonarray.Parent = get_name_id(parent, "", "");
        //    memo("Parent " + parent + ": " + taxonarray.Parent.ToString());
        //    taxondict[taxonarray.Parent].Children.Add(j);

        //    taxondictadd(j, taxonarray);



        //    if (!subspecific.Contains(taxondict[j].Level))
        //    {
        //        name_id_add(taxondict[j].Name, j);
        //    }

        //    memo("Taxon " + j.ToString() + " added: " + name);

        //    return j;
        //}

        public bool move_taxon(string name, string newparent)
        {
            int j = get_name_id(name, "", "");
            memo(name + ", j = " + j.ToString());
            if (j < 0)
                return false;

            int jold = taxondict[j].Parent;
            int jnew = get_name_id(newparent, "", "");
            memo(newparent + ", jnew = " + jnew.ToString());
            if (jnew < 0)
                return false;

            taxondict[j].Parent = jnew;
            taxondict[jold].Children.Remove(j);
            taxondict[jnew].Children.Add(j);

            memo("Taxon " + j.ToString() + " moved from " + taxondict[jold].Name + " to " + newparent);


            return true;
        }

        public static void fill_statusdicts()
        {
            iucnstatusdict.Add("EX", mp(16, null));
            iucnstatusdict.Add("RE", mp(17, null));
            iucnstatusdict.Add("CR", mp(18, null));
            iucnstatusdict.Add("EN", mp(19, null));
            iucnstatusdict.Add("VU", mp(20, null));
            iucnstatusdict.Add("NT", mp(21, null));
            iucnstatusdict.Add("DD", mp(22, null));
            iucnstatusdict.Add("LC", mp(23, null));
            iucnstatusdict.Add("NR", mp(24, null));
        }

        public static string makegallery(List<string> piclist)
        {
            StringBuilder sb = new StringBuilder("\n<gallery>\n");
            foreach (string s in piclist)
                sb.Append(s + "\n");
            sb.Append("</gallery>\n");


            return sb.ToString();
        }

        public void read_COL_source_ref()
        {
            //memo("Fixa READ COL SOURCE REF!");
            using (StreamReader sr = new StreamReader(dyntaxafolder + "COL_source_refs.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] words = line.Split('\t');
                    if ( words.Length >= 2)
                    {
                        COL_sourcedict.Add(words[0], words[1]);
                    }
                }
            }
        }

        public void read_doubles()
        {
            using (StreamReader sr = new StreamReader(dyntaxafolder + "COL-doubles.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] words = line.Split(',');
                    if (words.Length > 2)
                    {
                        doubledict.Add(words[0], new List<int>());
                        for (int i = 1; i < words.Length; i++)
                            doubledict[words[0]].Add(Convert.ToInt32(words[i].Trim()));
                    }
                }
            }
        }

        public void memo(string s)
        {
            richTextBox1.AppendText(s + "\n");
            richTextBox1.ScrollToCaret();
        }

        Taxonclass taxonfromdb(Taxon tt)
        {
            //            public int taxonid; //index to various other list; COL id#; if no COLid, then dyntaxaid + 20 million
            //public int taxonid13 = -1;
            //public int dyntaxaid = -1; //taxon id in Dyntaxa; -1 if none
            //public int dyntaxa2 = -1; //index to taxondict entry for second (3rd etc) Dyntaxa taxon
            //public int iucnid = -1;
            //public int redfiid = -1;
            //public int redsvid = -1;
            //public string Name = ""; //scientific name
            //public string Name_sv = ""; //Swedish name
            //public int Level = -1; //taxonomic level, index to rank_name
            //public int Parent = -1; //index of parent in COL; -1 if top taxon; -2 if taxon only in dyntaxa; -3 if taxon is double dyntaxa entry.
            //public int dyntaxa_Parent = -1; //index of parent in dyntaxa
            //public int Parent11 = -1;
            //public int Parent13 = -1;
            //public string Parentname = "";
            //public List<int> Children = new List<int>(); //index of children
            //public List<int> Children13 = new List<int>(); //index of children
            //public List<int> dyntaxa_Children = new List<int>(); //index of children
            //public string dyntaxa_auktor = "";
            //public string dyntaxa_name = "";
            //public string iucn_auktor = "";
            //public string iucn_name = "";
            //public string redfi_name = "";
            //public string redsv_name = "";
            //public List<int> habitats = new List<int>();
            //public List<int> synonyms = new List<int>();
            //public string regnum;
            //public string articlename = "";
            Taxonclass tc = new Taxonclass(tt,db,makelang);

            return tc;

    }

        public string redirect(Page p)
        {
            if (p.text[0] != '#')
            {
                //memo("|" + p.text.Substring(0, 10));
                return null;
            }
            else
            {
                PageList pl = p.GetLinks();
                if (pl.Count() > 0)
                    return pl[0].title;
                else
                {
                    memo("No links");
                    return null;
                }
            }
        }

        public bool overwrite_allowed(Page p)
        {
            //Check if existing article should be updated or scrapped. 
            //Don't touch it if it has been edited by human other than adding images.

            if (p.IsRedirect())
                return true;

            if (util.human_touched(p, makesite))
            {
                if (!util.human_image_only(p, makesite))
                    return false;
                else
                {
                    memo("Overwriting old article with human-added images");
                    return true;
                }
            }
            else if (p.text.Contains("geobox") || p.text.Contains("giklaro paghimo ni bot"))
            {
                memo("Bot-written geography article. Do not overwrite");
                return false;
            }
            else
            {
                memo("Overwriting old bot-written article");
                return true;
            }

        }


        public string getarticlename(Page p, int taxonid) //assumes p is loaded already
        {                                                 //assumes taxondict has taxonid 
            string tit = p.title;

            if (util.remove_disambig(p.title) == p.title)
            {
                var q = from c in db.Artnametable where c.Taxonid == taxonid where c.Lang == makelang select c;
                if (q.Count() > 0)
                {
                    tit = q.First().Articlename;
                    if (tit != p.title)
                    {
                        p = new Page(makesite, tit);
                        util.tryload(p, 2);
                    }
                }
            }
            //string origname = p.title;

            if (!p.Exists())
                return p.title;

            if (p.IsRedirect())
            {
                Page pold = new Page(makesite,p.RedirectsTo());
                util.tryload(pold, 1);
                if (!pold.Exists())
                    return p.title;

                if (pold.text.Contains("geobox") || pold.text.Contains("giklaro paghimo ni bot"))
                {
                    memo("Redirect to geography article. Overwrite redirect");
                    return p.title;
                }
                else if (pold.text.Contains("axobox"))
                {
                    string tboxname = util.taxon_from_page(pold);
                    if (String.IsNullOrEmpty(tboxname))
                    {
                        return p.title;
                    }
                    else if (tboxname.ToLower() == taxondict[taxonid].Name.ToLower())
                    {
                        bool overwrite = overwrite_allowed(pold);
                        if (overwrite)
                        {
                            if (util.trymove(pold, p.title, 2, "Move to common name"))
                                return p.title;
                            else
                                return null;
                        }
                        else
                            return null;
                    }
                    else
                    {
                        return p.title;
                    }
                }
                else
                {
                    memo("Redirect to something else. Overwrite redirect");
                    return p.title;
                }
            }

            //not redirect:
            else
            {
                if (p.text.Contains("axobox"))
                {
                    string tboxname = util.taxon_from_page(p);
                    if (String.IsNullOrEmpty(tboxname))
                    {
                        return p.title;
                    }
                    else if (tboxname == taxondict[taxonid].Name)
                    {
                        if (overwrite_allowed(p))
                            return p.title;
                        else
                            return null;
                    }
                    else //wrong taxon in taxobox
                    {
                        if (util.remove_disambig(p.title) != p.title)
                            return null;
                        else
                        {
                            return null;
                            //string group = taxondict[taxonid].getnamedgroup();
                            //p = new Page(makesite,p.title + " (" + MakeSpecies.groupname_sing[group] + ")");
                            //util.tryload(p, 2);
                            //return getarticlename(p, taxonid);
                        }
                    }
                }
                else
                {
                    if (util.remove_disambig(p.title) != p.title)
                        return null;
                    else
                    {
                        string group = taxondict[taxonid].getnamedgroup();
                        p = new Page(makesite,p.title + " (" + MakeSpecies.groupname_sing[group] + ")");
                        util.tryload(p, 2);
                        return getarticlename(p, taxonid);
                    }
                }

            }



            //================================================= old stuff ================
            //if (p.Exists())
            //{
            ////skipmaking = true;
            //memo("1: " + DateTime.Now.ToString());

            //bool inregnum = plregnum.Contains(p);
            //memo("2: " + DateTime.Now.ToString());


            //memo("Exists already. Inregnum = " + inregnum);

            //string red = redirect(p);

            //if (!String.IsNullOrEmpty(red) && (p.title == taxondict[taxonid].Name))
            //{
            //    p = new Page(makesite, red);
            //    util.tryload(p, 1);
            //    if (p.Exists())
            //    {
            //        memo("Redirects to " + p.title);
            //        //int parpar = taxondict[taxonid].Parent;
            //        //if (!taxondict.ContainsKey(parpar))
            //        //    parpar = taxondict[taxonid].dyntaxa_Parent;
            //        bool righttaxon = false;
            //        foreach (string tt in p.GetTemplateParameter("Taxobox", "taxon"))
            //            if (tt == taxondict[taxonid].Name)
            //                righttaxon = true;

            //        memo("3: " + DateTime.Now.ToString());

            //        if (plregnum.Contains(p) && !swe_name.ContainsKey(taxonid) && righttaxon) // && p.text.Contains(taxondict[taxonid].Name) && p.text.Contains(taxondict[parpar].Name))
            //        {
            //            swe_name.Add(taxonid, p.title.ToLower());
            //            taxondict[taxonid].Name_sv = p.title.ToLower();
            //            taxondict[taxonid].articlename = p.title.ToLower();
            //        }
            //        memo("4: " + DateTime.Now.ToString());

            //        inregnum = (inregnum || plregnum.Contains(p));
            //        memo("5: " + DateTime.Now.ToString());

            //    }

            //}

            //p.title = origname;

            //    if (!inregnum)
            //    {
            //        memo("6: " + skipmaking + " " + DateTime.Now.ToString());

            //        memo("Not in regnum.");
            //        int parpar = taxondict[taxonid].Parent13;
            //        if (!taxondict.ContainsKey(parpar))
            //            parpar = taxondict[taxonid].Parent;
            //        if (!taxondict.ContainsKey(parpar))
            //            parpar = taxondict[taxonid].dyntaxa_Parent;
            //        memo("7: " + DateTime.Now.ToString());

            //        if (taxondict.ContainsKey(parpar))
            //        {
            //            if (!p.text.Contains("axobox") && !p.text.Contains(taxondict[parpar].Name))
            //            {
            //                string separator = regnum_to_make_sv;
            //                if (rank == "species")
            //                {
            //                    if (separator == "svampar")
            //                        separator = "svamp";
            //                    else if (separator == "växter")
            //                        separator = "växt";
            //                    if (separator == "fungi")
            //                        separator = "fungus";
            //                    else if (separator == "plants")
            //                        separator = "plant";
            //                    else if (separator == "animals")
            //                        separator = "animal";

            //                    if (separator.Contains("mga "))
            //                        separator = separator.Replace("mga ", "");
            //                }

            //                p.title += " (" + separator + ")";

            //                memo("8: " + DateTime.Now.ToString());

            //                if (!plregnum.Contains(p))
            //                {

            //                    memo("Name changed to " + p.title);
            //                    util.tryload(pconflict, 1);
            //                    pconflict.text += "\n*[[" + origname + "]] ";
            //                    pconflict.text += "[[" + p.title + "]]";
            //                    util.trysave(pconflict, 1, "Logging conflict");
            //                    p.text = "";
            //                    skipmaking = false;

            //                    taxondict[taxonid].articlename = p.title;

            //                    DateTime thismonth = DateTime.Now;
            //                    string monthstring = thismonth.Month.ToString();
            //                    while (monthstring.Length < 2)
            //                        monthstring = "0" + monthstring;
            //                    string datestring = thismonth.Year.ToString() + "-" + monthstring;

            //                    string[] p9 = new string[2] { origname, datestring };
            //                    jointemplate = mp(9, p9);
            //                    if (!util.tryload(p, 4))
            //                    {
            //                        pfail.text += "Make failed at load for " + p.title;
            //                        util.trysave(pfail, 1, "Logging make failure");
            //                        skipmaking = true;
            //                    }
            //                    else
            //                    {
            //                        Page ptalk = new Page(makesite, talkprefix + ":" + p.title);
            //                        util.tryload(ptalk, 2);
            //                        ptalk.text += "\n\n== " + mp(11, null) + " ==\n";
            //                        string[] p12 = new string[1] { origname };
            //                        ptalk.text += mp(12, p12);
            //                        util.trysave(ptalk, 2, "Logging possible duplicate");

            //                    }
            //                    memo("9: " + skipmaking + " " + DateTime.Now.ToString());

            //                }
            //            }
            //        }
            //    }
            //}
            //else if (stitle != taxondict[taxonid].Name)
            //{
            //    memo("Fixing title.");
            //    p.title = testprefix + taxondict[taxonid].Name;
            //    if (reallymake && plregnum.Contains(p))
            //        skipmaking = true;
            //    else if (!util.tryload(p, 4))
            //    {
            //        pfail.text += "Make failed at load for " + p.title;
            //        util.trysave(pfail, 1, "Logging load failure");
            //        skipmaking = true;
            //    }
            //    else if (p.Exists())
            //        skipmaking = true;
            //    p.title = testprefix + stitle;

            //}
            //else
            //    memo("Not existing");


            //return tit;
        }

        public void makearticle(int taxonidpar)
        {
            int taxonid = taxonidpar;
            found13 = false;
            found11 = false;
            int taxonid13 = taxonid;
            bool indyn = false;


            if (!taxondict.ContainsKey(taxonid))
            {
                //memo("taxonid not found in COL 2011");
                Taxonclass tc = Taxonclass.loadtaxon(taxonid, db, makelang);
                if (tc != null)
                {
                    taxondictadd(taxonid, tc);
                    found11 = true;
                }
                //return;
            }

            if (!taxondict.ContainsKey(taxonid))
            {
                memo("taxonid not found in COL 2019");
                return;
            }
            else
                found11 = true;


            string stitle = "";
            if (found11 || indyn)
                stitle = taxondict[taxonid].Name;

            memo(stitle + " " + taxonid.ToString());


            if (!found11 && !found13 && !dynloop)
            {
                memo("!found11 && !found13");
                return;
            }

            string strim = " ()[],;?*/|:";
            char[] trimchars = strim.ToCharArray();

            string rank = "";

            //skip taxon trees already done:
            if (donetree.Contains(taxonid) && !CB_overridedone.Checked)
            {
                memo("Already in donetree:" + stitle + "|");
                //idone += countarticle(taxonid);
                //if (idone > itodo)
                //{
                //    memo(stitle + " countarticle " + countarticle(taxonid));
                //}
                return;
            }

            bool skipmaking = false;

            if (resume_at == stitle)
            {
                reallymake = true;
                resume_at = "";
            }
            else if (!string.IsNullOrEmpty(resume_at))
            {
                skipmaking = true;
                //return;
            }

            //if (!skipmaking)
            //{
            if (swe_name.ContainsKey(taxonid))
            {
                stitle = swe_name[taxonid].ToLower();
                if (stitle.Length == 0)
                    stitle = taxondict[taxonid].Name;
            }

            if (latin_swe_name.ContainsKey(stitle))
            {
                stitle = latin_swe_name[stitle];
                if (String.IsNullOrEmpty(taxondict[taxonid].Name_sv))
                {
                    taxondict[taxonid].Name_sv = stitle;
                }
                if (!swe_name.ContainsKey(taxonid))
                    swe_name.Add(taxonid, stitle);
            }

            if (stitle.Length == 0)
            {
                memo("Title length = 0");
                return;
            }


            stitle = util.initialcap(stitle);

            //stitle = testprefix + stitle;

            if (found11 || indyn)
            {
                rank = rank_name[taxondict[taxonid].Level];
            }

            memo("title = " + stitle);
            memo("name,rank = " + taxondict[taxonid].Name + ", " + taxondict[taxonid].Name_sv + ", " + rank);

            if (!rank_order.ContainsKey(rank))
            {
                memo("WRONG RANK ORDER: name, rank = " + taxondict[taxonid].Name + ", " + rank);
                return;
            }


            if (rank_order[rank] < rank_order[bottomrank])
            {
                memo(stitle + " below bottomrank.");
                return;
            }

            //memo("title = " + stitle);
            //memo("name,rank = " + taxondict[taxonid].Name + ", " + taxondict[taxonid].Name_sv + ", " + rank);

            skipmaking = skipmaking || isincert(taxondict[taxonid].Name);

            if (skipmaking)
                memo("Incertae sedis");

            string jointemplate = "";

            Page p = new Page(makesite, testprefix + stitle);

            bool incol = found11 || found13; // (taxondict[taxonid].taxonid < dyntaxadiff);
            indyn = (taxondict[taxonid].dyntaxaid > 0);

            if (dynloop & incol)
            {
                skipmaking = true;
                memo("Dynloop & incol");
            }

            List<string> oldpiclist = new List<string>();
            Dictionary<string, string> oldimageparams = new Dictionary<string, string>();

            bool iwfound = false;

            List<string> oldiwlist = new List<string>();
            bool oldiwfromwd = true;



            if (!skipmaking)
            {

                if (!util.tryload(p, 4))
                {
                    pfail.text += "Make failed at load for " + p.title;
                    util.trysave(pfail, 1, "Logging failure");
                    skipmaking = true;
                }

                //------------------------------------------

                string newtit = getarticlename(p, taxonid);

                //------------------------------------------


                if (String.IsNullOrEmpty(newtit))
                    skipmaking = true;
                else
                {
                    taxondict[taxonid].articlename = newtit;
                    taxondict[taxonid].update_artnametable(db, makelang);

                    if (p.title != newtit)
                    {
                        p = new Page(makesite, newtit);
                    }
                    else //store info from old article if it exists
                    {
                        if (p.Exists())
                        {
                            if (!skipmaking)
                            {
                                oldpiclist = p.GetImages();
                                List<string> oldgallery = util.getgallery(p);
                                foreach (string og in oldgallery)
                                    oldpiclist.Add(og);
                                List<string> templates = p.GetTemplates(true, false);
                                foreach (string temp in templates)
                                {
                                    if (temp.Contains("axobox"))
                                        oldimageparams = Page.ParseTemplate(temp);
                                }

                                try
                                {
                                    oldiwlist = p.GetInterLanguageLinks();
                                }
                                catch (Exception e)
                                {
                                    string message = e.Message;
                                    memo(message);
                                    Thread.Sleep(10000);//milliseconds
                                }

                                memo("oldiwlist.Count " + oldiwlist.Count);
                                if (oldiwlist.Count > 0)
                                {
                                    //iwfound = true;
                                    string rex = @"\[\[\w{2,3}\:.+?\]\]";
                                    MatchCollection mc = Regex.Matches(p.text, rex);
                                    memo("mc " + mc.Count);
                                    if (mc.Count > 0)
                                    {
                                        oldiwfromwd = false;

                                    }
                                }

                            }
                        }

                    }
                    p.text = null;
                }

                memo("10: " + skipmaking + " " + DateTime.Now.ToString());

                if (incol && !p.Exists() && !skipmaking && !makemonotypic && (rank != "species"))
                {
                    int nkids = taxondict[taxonid].Children.Count;
                    int nkids13 = taxondict[taxonid].Children13.Count;

                    if (nkids + nkids13 == 0)
                    {
                        memo("No kids!");
                        //return;
                    }
                    else
                    {
                        bool ismonotypic = true;
                        if (nkids > 1)
                            ismonotypic = false;
                        if (nkids13 > 1)
                            ismonotypic = false;
                        if (ismonotypic)
                        {
                            memo("Monotypic.");
                            int kid = -1;
                            if (nkids >= 1)
                                kid = taxondict[taxonid].Children[0];

                            if (!taxondict.ContainsKey(kid))
                            {
                                Taxonclass tcp = Taxonclass.loadtaxon(kid, db, makelang);
                                if (tcp != null)
                                    taxondictadd(kid, tcp);
                            }
                            if (taxondict.ContainsKey(kid))
                                util.make_redirect(p.title, taxondict[kid].Name, taxondict[taxonid].Name, reallymake && !CBmemo.Checked);
                            memo("Skipping monotype");
                            skipmaking = true;
                        }
                    }
                }
            }

            List<distributionclass> dcl = null;

            if (rank == "species")
            {
                if (!skipmaking)
                {
                    dcl = distributionclass.getdistribution(taxonid, db);
                    if (CB_onlydistribution.Checked && dcl.Count == 0)
                    {
                        skipmaking = true;
                        memo("No distribution; skip!");
                    }
                }
            }

            if (!skipmaking) //Start building the article!
            {
                memo("Make!");
                int[] parent = new int[20];
                //int[] dynparent = new int[20];
                string[] parentname = new string[20];
                //string[] dynparentname = new string[20];

                int diffparent = -1;
                int famparent = -1;
                int sweparent = -1;
                int dynsweparent = -1;

                int jj = 0;
                int jjmax = -1;
                int dynjjmax = -1;

                //Clear reference list

                reflist = "<references>";
                refnamelist.Clear();

                for (int ij = 0; ij < 20; ij++)
                {
                    parent[ij] = -1;
                    //dynparent[ij] = -1;
                }

                string itgenus = "";

                if (incol)
                {
                    parent[jj] = taxondict[taxonid].Parent;
                    if (!taxondict.ContainsKey(parent[jj]))
                    {
                        Taxonclass tpar = Taxonclass.loadtaxon(parent[jj], db, makelang);
                        if (tpar != null)
                            taxondictadd(parent[jj], tpar);
                        else
                            parent[jj] = -1;
                        //if (id1311.ContainsKey(parent[jj]))
                        //    parent[jj] = id1311[parent[jj]];
                        //else
                        //{
                        //    XmlNode parentnode = getxmlfromid(parent[jj]);
                        //    if (parentnode != null)
                        //        parent[jj] = addxmltaxon(parentnode);
                        //    else
                        //        parent[jj] = taxondict[parent[jj]].Parent;
                        //}
                    }
                    while ((parent[jj] > 0) && isincert(taxondict[parent[jj]].Name))
                    {
                        parent[jj] = taxondict[parent[jj]].Parent;
                    }


                    while (parent[jj] > 0)
                    {
                        parent[jj + 1] = taxondict[parent[jj]].Parent;
                        if (!taxondict.ContainsKey(parent[jj + 1]))
                        {
                            if (parent[jj + 1] > 0)
                            {
                                Taxonclass tpar = Taxonclass.loadtaxon(parent[jj + 1], db, makelang);
                                if (tpar != null)
                                    taxondictadd(parent[jj + 1], tpar);
                                else
                                    parent[jj + 1] = -1;
                            }
                        }
                        if (!taxondict.ContainsKey(parent[jj + 1]))
                        {
                            memo("Grandparent missing in taxondict!");
                            parent[jj + 1] = -2;
                        }
                        else
                        {
                            if (parent[jj + 1] > 0)
                                while (taxondict.ContainsKey(parent[jj + 1]) && isincert(taxondict[parent[jj + 1]].Name))
                                {
                                    parent[jj + 1] = taxondict[parent[jj + 1]].Parent;
                                    if (!taxondict.ContainsKey(parent[jj + 1]))
                                    {
                                        parent[jj + 1] = -2;
                                        memo("Parent loop: " + parent[jj + 1].ToString());
                                    }
                                    else
                                        memo("Parent loop: " + parent[jj + 1].ToString() + taxondict[parent[jj + 1]].Name);
                                }
                        }
                        if (!String.IsNullOrEmpty(taxondict[parent[jj]].Name_sv))
                        {
                            if (sweparent < 0)
                                sweparent = parent[jj];
                            parentname[jj] = taxondict[parent[jj]].Name_sv.ToLower();
                        }
                        else
                        {
                            parentname[jj] = taxondict[parent[jj]].Name;
                            if (latin_swe_name.ContainsKey(parentname[jj]))
                            {
                                if (sweparent < 0)
                                    sweparent = parent[jj];
                                parentname[jj] = latin_swe_name[parentname[jj]];
                                taxondict[parent[jj]].Name_sv = parentname[jj];
                            }
                        }
                        if (rank_name[taxondict[parent[jj]].Level] == "familia")
                            famparent = jj;

                        if (rank_name[taxondict[parent[jj]].Level] == "genus")
                        {
                            if (parentname[jj] == taxondict[parent[jj]].Name_sv)
                                itgenus = "";
                            else
                                itgenus = "''";
                        }


                        jjmax = jj;
                        jj++;

                    }
                }

                jj = 0;


                memo("Parentname: ");
                foreach (string pp in parentname)
                    if (!string.IsNullOrEmpty(pp))
                        memo(pp + ";");
                memo("\n");

                string ggname = taxondict[taxonid].getnamedgroup();

                //string ggname = regnum_to_make;
                //for (int ij = 0; ij < 20; ij++)
                //{
                //    //memo(ij.ToString() + " " +parent[ij].ToString()+" "+ taxondict[parent[ij]].Name);
                //    //memo(taxondict[parent[ij]].Level.ToString() + " " + rank_name[taxondict[parent[ij]].Level]);

                //    if (taxondict[taxonid].Name == regnum_to_make)
                //    {
                //        ggname = taxondict[taxonid].Name;
                //        break;
                //    }
                //    else if (rank_order[rank_name[taxondict[parent[ij]].Level]] < rank_order["familia"])
                //    {
                //        continue;
                //    }
                //    else if (groupname_sing.ContainsKey(taxondict[parent[ij]].Name))
                //    {
                //        ggname = taxondict[parent[ij]].Name;
                //        break;
                //    }
                //    else if ((ij > 1) && (taxotop.ContainsKey(taxondict[parent[ij]].Name)))
                //    {
                //        Page pg = new Page(makesite, mp(13, null) + botname + "/DoubleTaxon");
                //        pg.Load();
                //        pg.text += "\n* Double taxon: " + taxondict[taxonid].Name + "; \n** parents = ";
                //        foreach (string pp in parentname)
                //            pg.text += pp + ";";
                //        pg.text += "\n";
                //        pg.Save();

                //        break;
                //    }

                //}


                string COLref = "";

                string COLurl = "http://www.catalogueoflife.org/DCA_Export/zip-fixed/2019-annual.zip";
                //string COLyear = "2019";
                string COLauthor = "Roskov Y., Kunze T., Orrell T., Abucay L., Paglinawan L., Culham A., Bailly N., Kirk P., Bourgoin T., Baillargeon G., Decock W., De Wever A., Didžiulis V. (ed)";
                string COLdate = "2019-11-11";


                string[] p14 = new string[6] { COLurl, "Species 2000 & ITIS Catalogue of Life: " + colyear + " Annual Checklist.", COLdate, COLauthor, colyear, "Species 2000: Naturalis, Leiden, the Netherlands. ISSN 2405-884X. TaxonID: " + taxonid };
                //string[] p14 = new string[6] { "http://www.catalogueoflife.org/services/res/2011AC_26July.zip", "Species 2000 & ITIS Catalogue of Life: 2011 Annual Checklist.", "24 september 2012", "Bisby F.A., Roskov Y.R., Orrell T.M., Nicolson D., Paglinawan L.E., Bailly N., Kirk P.M., Bourgoin T., Baillargeon G., Ouvrard D. (red.)", "2011", "Species 2000: Reading, UK." };

                //string COLtaxon = "";
                //string COLtaxon "http://www.catalogueoflife.org/col/browse/tree/id/" + taxonid.ToString();
                //string COLrefstring = "{{webbref |url= http://www.catalogueoflife.org/services/res/2011AC_26July.zip|titel= Species 2000 & ITIS Catalogue of Life: 2011 Annual Checklist.|hämtdatum=24 september 2012 |författare= Bisby F.A., Roskov Y.R., Orrell T.M., Nicolson D., Paglinawan L.E., Bailly N., Kirk P.M., Bourgoin T., Baillargeon G., Ouvrard D. (red.)|datum= 2011|verk= |utgivare=Species 2000: Reading, UK.}}";
                string COLrefstring = mp(14, p14); // +" [" + COLtaxon + " " + taxondict[taxonid].Name + "]";
                if (incol)
                    COLref = addref("COL", COLrefstring);
                string COLref_short = COLref;


                //string dynref = "";
                ////string dynref2 = "";
                //if ((indyn) && (dyntaxa_url.ContainsKey(taxonid)))
                //{
                //    dynref = addref("dyn", "[" + dyntaxa_url[taxonid] + "?changeRoot=True Dyntaxa " + taxondict[taxonid].Name + "]");
                //    //dynref2 = "<ref name=\"dyn\"/>";
                //}

                string ittit = "";
                if ((rank == "species") || (rank == "genus"))
                    ittit = "''";
                if (stitle != taxondict[taxonid].Name)
                    ittit = "";


                //string svampfaktamall = "{{Svampfakta\n|             namn = " + p.title + "\n|      hymeniumtyp = \n|         hattform = \n|          skivtyp = \n|      fotkaraktär = \n| sporavtrycksfärg = \n|    ekologisk_typ = \n|         ätlighet = \n}}";
                //string svampfaktaplats = mp(33, null);

                //=============================================================================================
                //Starting with actual article text here!
                //=============================================================================================

                //Templates at the beginning:
                DateTime thismonth = DateTime.Now;
                string monthstring = thismonth.Month.ToString();
                while (monthstring.Length < 2)
                    monthstring = "0" + monthstring;
                string datestring = thismonth.Year.ToString() + "-" + monthstring;
                string[] p10 = new string[3] { botname, groupname_attr[ggname], datestring };
                string[] p15 = new string[1] { ittit + util.initialcap(stitle) + ittit };
                p.text = jointemplate + mp(10, p10) + "\n" + mp(15, p15).Replace("\\n", "\n") + "\n\n";

                //Article text:
                if (makelang != "war")
                    p.text += "\n";
                else
                    p.text += "An ";
                string titname = "";
                if (stitle != taxondict[taxonid].Name)
                    titname = "'''" + stitle + "''' (''" + taxondict[taxonid].Name + "'')";
                else
                    titname = "'''" + ittit + taxondict[taxonid].Name + ittit + "'''";

                if (incol)
                {
                    if (makelang == "ceb")
                        titname += "." + COLref;
                    else
                        titname += COLref;
                }

                if (rank == "species")
                {
                    string[] p34 = new string[2] { groupname_plural[ggname], groupname_sing[ggname] };
                    string m34 = mp(34, p34);
                    if (groupname_plural[ggname] == groupname_sing[ggname])
                    {
                        m34 = m34.Replace(groupname_plural[ggname] + "|" + groupname_sing[ggname], groupname_sing[ggname]);
                    }
                    if (makelang == "ceb")
                        p.text += m34 + " " + titname;
                    else
                        p.text += titname + " " + m34;

                    if (ggname == regnum_to_make)
                    {
                        for (int ij = 0; ij < jjmax; ij++)
                        {
                            if ((rank_name[taxondict[parent[ij]].Level] == "phylum") || (rank_name[taxondict[parent[ij]].Level] == "divisio"))
                                p.text += " " + mp(35, null) + " " + rank_latin_swe_def[rank_name[taxondict[parent[ij]].Level]] + " " + linktaxon(parent[ij]) + ", " + mp(3, null);
                        }

                    }
                }
                else //higher taxa
                {
                    if (makelang == "ceb")
                        p.text += util.initialcap(rank_latin_swe_indef[rank]) + " " + mp(6, null) + " [[" + groupname_plural[ggname] + "]] ang " + titname;
                    else
                        p.text += titname + " " + mp(4, null) + " " + rank_latin_swe_indef[rank] + " " + mp(6, null) + " [[" + groupname_plural[ggname] + "]]";
                }




                string auktor = taxondict[taxonid].auktor;

                if (!String.IsNullOrEmpty(auktor))
                {
                    string ta = auktorclass.textauthor(auktor);
                    memo("textauthor = " + ta);
                    if (makelang == "ceb")
                        p.text += " " + util.initialcap(ta.Trim()) + makeref(taxonid) + " ";
                    else
                        p.text += ta + makeref(taxonid) + " ";
                }
                else
                {
                    if (makelang == "ceb")
                        p.text += " ";
                    else
                        p.text += ". ";
                }

                string sourceref = "";
                if (!String.IsNullOrEmpty(taxondict[taxonid].source_dataset))
                {
                    //sourceref = addref("source", taxondict[taxonid].source_dataset);
                    if (COL_sourcedict.ContainsKey(taxondict[taxonid].source_dataset))
                        sourceref = addref("source", COL_sourcedict[taxondict[taxonid].source_dataset]);
                }

                //if (found13)
                //{
                //    string sourcebasename = getxmlpar("source_database", currentnode);
                //    string sourcebaseurl = getxmlpar("source_database_url", currentnode);
                //    if (!String.IsNullOrEmpty(sourcebaseurl))
                //        sourceref = addref("source", "[" + sourcebaseurl + " " + sourcebasename + "]");
                //}
                //if (String.IsNullOrEmpty(sourceref))
                //    if (taxon_database.ContainsKey(taxonid))
                //        sourceref = addref("source", source_database[taxon_database[taxonid]]);


                string allref = "";
                if (incol)
                    allref += COLref + sourceref;

                if ((rank != "phylum") && (rank != "divisio"))
                {
                    if (famparent >= 0) //taxon below family
                    {
                        //string ital ="";
                        //if ( stitle == taxondict[taxonid].Name)
                        //    ital="''";
                        //if ((diffparent > famparent) || (diffparent < 0) || (dynparent[famparent] < 0))
                        {
                            p.text += mp(59, null) + " " + ittit + util.initialcap(stitle) + ittit + " " + mp(5, null) + " ";
                            if (famparent == 0)
                                p.text += rank_latin_swe_def[rank_name[taxondict[parent[0]].Level]] + " " + linktaxon(parent[0]) + "." + allref + " ";
                            else
                            {
                                for (int ij = 0; ij < famparent; ij++)
                                {
                                    p.text += rank_latin_swe_def[rank_name[taxondict[parent[ij]].Level]] + " " + itgenus + linktaxon(parent[ij]) + itgenus + ", ";
                                }
                                p.text += mp(3, null) + " " + rank_latin_swe_def[rank_name[taxondict[parent[famparent]].Level]] + " [[" + parentname[famparent] + "]]." + allref + " ";
                            }
                        }
                    }
                    else //taxon family or above
                    {
                        p.text += mp(59, null) + " " + ittit + util.initialcap(stitle) + ittit + " " + mp(5, null) + " ";
                        if (jjmax == 0)
                            p.text += rank_latin_swe_def[rank_name[taxondict[parent[0]].Level]] + " " + linktaxon(parent[0]) + "." + allref + " ";
                        else
                        {
                            for (int ij = 0; ij < jjmax; ij++)
                            {
                                p.text += rank_latin_swe_def[rank_name[taxondict[parent[ij]].Level]] + " " + linktaxon(parent[ij]) + ", ";
                            }
                            p.text += mp(3, null) + " " + rank_latin_swe_def[rank_name[taxondict[parent[jjmax]].Level]] + " " + linktaxon(parent[jjmax]) + "." + allref + " ";
                        }
                    }

                }

                string redsvref = "";

                //string redfiref = "";

                string iucnref = "";
                int iud = -1;

                List<string> distcatlist = new List<string>();

                if (rank == "species")
                {
                    //iud = -1;
                    if (taxondict[taxonid].iucnid > 0)
                    {
                        iud = taxondict[taxonid].iucnid;
                    }
                    else if (iucnnamedict.ContainsKey(taxondict[taxonid].Name))
                    {
                        iud = iucnnamedict[taxondict[taxonid].Name];
                    }
                    memo("iud = " + iud);
                    if (iud > 0)
                    {
                        if (iucndict.ContainsKey(iud) && iucnstatusdict.ContainsKey(iucndict[iud].status))
                        {
                            string[] p36 = new string[3] { iucndict[iud].year, iud.ToString(), taxondict[taxonid].Name };
                            iucnref = addref("iucn", mp(36, p36));
                            p.text += " " + mp(7, null) + " " + iucnstatusdict[iucndict[iud].status] + "." + iucnref;
                        }
                    }

                    // dcl assigned earlier, to enable skip making without distribution
                    //dcl = distributionclass.getdistribution(taxonid, db);

                    if (dcl.Count > 0)
                    {
                        memo("dcl " + dcl.Count);
                        int nfail = 0;
                        int nsuccess = 0;
                        StringBuilder distsb = new StringBuilder("\n\n" + mp(37, null) + ":\n");
                        foreach (distributionclass dc in dcl)
                        {
                            //if (dcl.Count > ndcl)
                            //    memo(dc.towikistring("*"));
                            distsb.Append(dc.towikistring("*"));
                            if (dc.maindist == distributionclass.failstring)
                                nfail++;
                            else
                                nsuccess++;
                            foreach (distributionclass dcsub in dc.subdist)
                            {
                                //distsb.Append(dcsub.towikistring("**"));
                                if (dcsub.maindist == distributionclass.failstring)
                                    nfail++;
                                else
                                    nsuccess++;
                            }
                            distcatlist = distcatlist.Union(dc.getcats()).ToList();

                        }
                        if ((nfail == 0) || (nsuccess > 2 * nfail))
                        {
                            p.text += distsb.ToString() + "\n";
                        }
                    }




                    //if (taxon_region.ContainsKey(taxonid))
                    //{
                    //    if (taxon_region[taxonid].Count > 1)
                    //    {
                    //        p.text += "\n\n" + mp(37, null) + COLref + ":\n";
                    //        foreach (int ir in taxon_region[taxonid])
                    //            p.text += "* " + util.initialcap(region_name[ir]) + ".\n";
                    //        p.text += "\n";
                    //    }
                    //    else
                    //    {
                    //        foreach (int ir in taxon_region[taxonid])
                    //            p.text += "\n\n" + mp(37, null) + " " + region_name[ir] + "." + COLref + " ";

                    //    }
                    //}

                    //if (taxon_region_free.ContainsKey(taxonid))
                    //    if (makelang != "en")
                    //    {
                    //        Page ptalk = new Page(makesite, talkprefix + ":" + p.title);
                    //        util.tryload(ptalk, 2);
                    //        ptalk.text += "\n\n== " + mp(39, null) + " ==";
                    //        string[] p40 = new string[1] { datestring };
                    //        if (taxon_region_free[taxonid].Count > 1)
                    //        {

                    //            ptalk.text += "\n\n" + mp(40, p40) + ":\n";

                    //            foreach (int ir in taxon_region_free[taxonid])
                    //                ptalk.text += "* " + util.initialcap(region_free[ir]) + ".\n";
                    //            ptalk.text += mp(41, null) + ": <nowiki><ref name = \"COL\"/></nowiki>[http://www.catalogueoflife.org/services/res/2011AC_26July.zip] ~~~~\n\n";
                    //        }
                    //        else
                    //        {
                    //            foreach (int ir in taxon_region_free[taxonid])
                    //                ptalk.text += "\n\n" + mp(40, p40) + " ''\"" + region_free[ir] + "\"''. " + mp(41, null) + ": <nowiki><ref name = \"COL\"/></nowiki>[http://www.catalogueoflife.org/services/res/2011AC_26July.zip] ~~~~\n\n";

                    //        }
                    //        ptalk.text = ptalk.text.Replace(datestring + "}} ", datestring + "}}\n\n");
                    //        trysave(ptalk, 2);
                    //    }
                    //    else
                    //    {
                    //        if (taxon_region_free[taxonid].Count > 1)
                    //        {
                    //            p.text += "\n\nThe geographic distribution of the species is:\n";
                    //            foreach (int ir in taxon_region_free[taxonid])
                    //                p.text += "* " + util.initialcap(region_free[ir]) + ".";
                    //            p.text += COLref + "\n";

                    //        }
                    //        else
                    //        {
                    //            foreach (int ir in taxon_region_free[taxonid])
                    //                p.text += "\n\nThe geographic distribution of the species is " + region_free[ir] + COLref + "\n\n";

                    //        }

                    //    }

                    //if (taxondict[taxonid].redfiid > 0)
                    //{
                    //    redfiref = addref("redfi", "{{bokref|titel=Suomen lajien uhanalaisuus 2010= / The 2010 red list of Finnish species|år=2010|utgivare=Ympäristöministeriö|utgivningsort=Helsinki|språk=finska/engelska|isbn=978-952-11-3805-8|libris=12130085|url=http://www.ymparisto.fi/default.asp?contentid=371161&lan=en}}");
                    //    int rs = taxondict[taxonid].redfiid;

                    //    if (redfidict.ContainsKey(rs) && iucnstatusdict.ContainsKey(redfidict[rs].status))
                    //    {
                    //        p.text += " Enligt den finländska [[rödlistning|rödlistan]]" + redfiref + " är arten " + iucnstatusdict[redfidict[rs].status] + " i [[Finland]].";
                    //    }
                    //}

                    //if (taxondict[taxonid].redsvid > 0)
                    //{
                    //    redsvref = addref("redsv", "{{bokref|redaktör=Gärdenfors Ulf|titel=Rödlistade arter i Sverige 2010 = The 2010 red list of Swedish species|år=2010|utgivare=Artdatabanken i samarbete med Naturvårdsverket|utgivningsort=Uppsala|språk=swe|isbn=978-91-88506-35-1|libris=11818177|url=http://www.slu.se/sv/centrumbildningar-och-projekt/artdatabanken/om-oss/publikationer/bocker/2010-rodlistade-arter-i-sverige-2010/}}");
                    //    int rs = taxondict[taxonid].redsvid;

                    //    if (redsvdict.ContainsKey(rs) && iucnstatusdict.ContainsKey(redsvdict[rs].status))
                    //    {
                    //        p.text += " Enligt den svenska [[rödlistning|rödlistan]]" + redsvref + " är arten " + iucnstatusdict[redsvdict[rs].status] + " i Sverige.";
                    //    }
                    //    if (provincedict.ContainsKey(rs))
                    //        p.text += " " + provincetext(rs) + redsvref + " ";
                    //}
                    //else if (swe_status.ContainsKey(taxonid) && (taxondict[taxonid].dyntaxa2 < 0))
                    //    if (dynstatus_text.ContainsKey(swe_status_element[swe_status[taxonid]]))
                    //        p.text += " " + dynstatus_text[swe_status_element[swe_status[taxonid]]] + dynref;
                    //    else
                    //        p.text += " Artens status i [[Sverige]] är: " + swe_status_element[swe_status[taxonid]].ToLower() + "." + dynref;




                    //if (taxondict[taxonid].habitats.Count > 0)
                    //{
                    //    p.text += " Artens [[livsmiljö]] är ";
                    //    string sep = "";
                    //    string habref = "";
                    //    foreach (int ih in taxondict[taxonid].habitats)
                    //    {
                    //        p.text += sep + habitatdict[ih];
                    //        sep = ", ";
                    //        if (ih < 200)
                    //            habref = redfiref;
                    //        else
                    //            habref = redsvref;
                    //    }
                    //    p.text += "." + habref + " ";

                    //}


                    //else
                    //    p.text += " Arten har inte påträffats i Sverige.";

                    if (incol)
                    {

                        string[] si = taxondict[taxonid].Name.Split(' ');
                        string speciesinitials = si[0].Substring(0, 1) + ". " + si[1].Substring(0, 1) + ". ";

                        int nkids = taxondict[taxonid].Children.Count;

                        switch (nkids)
                        {
                            case 0:
                                p.text += " " + mp(42, null) + COLref + "\n";
                                break;
                            case 1:
                                p.text += " " + mp(43, null) + " ''";
                                foreach (int uus in taxondict[taxonid].Children)
                                {
                                    int us = uus;
                                    if (!taxondict.ContainsKey(uus))
                                    {
                                        Taxonclass tc = Taxonclass.loadtaxon(uus, db, makelang);
                                        if (tc != null)
                                        {
                                            taxondictadd(uus, tc);
                                        }
                                        else
                                            continue;
                                    }
                                    string[] names = taxondict[us].Name.Split(' ');
                                    string subname = taxondict[us].Name;

                                    if (names.Length > 2)
                                    {
                                        subname = "";
                                        for (int ii = 2; ii < names.Length; ii++)
                                            subname += names[ii];
                                    }
                                    p.text += speciesinitials + subname + "''.<ref name=\"COL\"/>";
                                }
                                break;
                            default:
                                p.text += "\n\n== " + mp(44, null) + " ==\n\n" + mp(45, null) + COLref + "\n\n";
                                foreach (int uus in taxondict[taxonid].Children)
                                {
                                    int us = uus;
                                    if (!taxondict.ContainsKey(uus))
                                    {
                                        Taxonclass tc = Taxonclass.loadtaxon(uus, db, makelang);
                                        if (tc != null)
                                        {
                                            taxondictadd(uus, tc);
                                        }
                                        else
                                            continue;
                                    }
                                    string[] names = taxondict[us].Name.Split(' ');
                                    string subname = taxondict[us].Name;

                                    if (names.Length > 2)
                                    {
                                        subname = "";
                                        for (int ii = 2; ii < names.Length; ii++)
                                            subname += names[ii];
                                    }
                                    p.text += "* ''" + speciesinitials + subname + "''\n";
                                }
                                break;
                        }

                        //else
                        //{
                        //    List<string> kids13 = getxmlsubspecies(currentnode);

                        //    switch (nkids13)
                        //    {
                        //        case 0:
                        //            p.text += " " + mp(42, null) + COLref + "\n";
                        //            break;
                        //        case 1:
                        //            p.text += " " + mp(43, null) + " ''";
                        //            foreach (string us in kids13)
                        //                p.text += speciesinitials + us + "''.<ref name=\"COL\"/>";
                        //            break;
                        //        default:
                        //            p.text += "\n\n== " + mp(44, null) + " ==\n\n" + mp(45, null) + COLref + "\n\n";
                        //            foreach (string us in kids13)
                        //                p.text += "* ''" + speciesinitials + us + "''\n";
                        //            break;
                        //    }
                        //}
                    }

                }

                if (incol & (rank_order[rank] >= rank_order["familia"]))
                {
                    ispecies = countspecies(taxonid); //FIXA countspecies så att den funkar med COL2014!!
                    memo("After countspecies: <return>");
                    //Console.ReadLine();

                    if (ispecies > 0)
                    {
                        string[] p65 = new string[3] { rank_latin_swe_def[rank], taxondict[taxonid].Name, ispecies.ToString() };
                        p.text += " " + mp(65, p65) + COLref + ". ";
                    }
                }

                p.text += "\n\n<!-- " + mp(46, null) + " -->\n\n";

                //Brödtexten klar.


                //Gör kladogram:

                string itkid = "";
                if (rank_order[rank] <= rank_order["familia"])
                    itkid = "''";


                if (incol && (rank != "species"))
                {
                    int braces = 0;

                    int nsiblings = taxondict[parent[0]].Children13.Count;
                    List<int> siblist = taxondict[parent[0]].Children13;
                    if (nsiblings <= 0)
                    {
                        nsiblings = taxondict[parent[0]].Children.Count;
                        siblist = taxondict[parent[0]].Children;
                    }

                    int nkids = taxondict[taxonid].Children.Count;
                    List<int> kidlist = taxondict[taxonid].Children;
                    //if (nkids <= 0)
                    //{
                    //    nkids = taxondict[parent[0]].Children.Count;
                    //    kidlist = taxondict[parent[0]].Children;
                    //}

                    memo("# children for " + taxondict[taxonid].Name + " = " + kidlist.Count.ToString());

                    string cladestring = "";
                    string cladestart = "{{Clade";
                    if (makewiki == "diq")
                        cladestart = "{{Clade2";

                    //siblist = fixlist13(siblist);
                    //kidlist = fixlist13(kidlist);

                    List<int> badkids = new List<int>();

                    foreach (int kid in kidlist)
                    {
                        if (!taxondict.ContainsKey(kid))
                        {
                            Taxonclass tc = Taxonclass.loadtaxon(kid, db, makelang);
                            if (tc != null)
                            {
                                taxondictadd(kid, tc);
                                found11 = true;
                            }
                            else
                            {
                                badkids.Add(kid);
                            }
                        }

                    }

                    foreach (int kid in badkids)
                        kidlist.Remove(kid);

                    badkids.Clear();

                    foreach (int kid in siblist)
                    {
                        if (!taxondict.ContainsKey(kid))
                        {
                            Taxonclass tc = Taxonclass.loadtaxon(kid, db, makelang);
                            if (tc != null)
                            {
                                taxondictadd(kid, tc);
                                found11 = true;
                            }
                            else
                            {
                                badkids.Add(kid);
                            }
                        }
                    }

                    foreach (int kid in badkids)
                        siblist.Remove(kid);

                    nkids = kidlist.Count;

                    if (nkids == 1)
                    {
                        int kid = kidlist[0];
                        //if (makemonotypic)
                        cladestring += util.initialcap(rank_latin_swe_def[rank]) + " " + mp(47, null) + " " + rank_latin_swe_def[rank_name[taxondict[kid].Level]] + " " + linktaxon(kid) + "." + COLref + " ";

                    }
                    if (nsiblings == 1)
                    {
                        string[] p48 = new string[2] { ittit + util.initialcap(stitle) + ittit, rank_latin_swe_def[rank] };
                        cladestring += mp(48, p48) + " " + rank_latin_swe_def[rank_name[taxondict[parent[0]].Level]] + " " + linktaxon(parent[0]) + "." + COLref + " ";
                        nsiblings = 9999;
                    }

                    if ((nkids > 1) && (nsiblings > 1))
                    {
                        if ((nkids < 30) && (nsiblings < 30))
                        {
                            cladestring += "\n\n" + mp(49, null) + COLref + ":\n\n" + cladestart + "\n";
                            cladestring += "| label1 = " + linktaxon(parent[0]) + "&nbsp;\n";
                            cladestring += "| 1=" + cladestart + "\n"; braces++; braces++;
                            int ikid = 1;

                            //First list the article taxon:
                            int kid = taxonid;
                            //if (kid == taxonid)
                            //{
                            cladestring += "  | label" + ikid.ToString() + " = " + ittit + "'''&nbsp;" + stitle + "&nbsp;'''" + ittit + "\n";
                            cladestring += "  | " + ikid.ToString() + "=" + cladestart + "\n";
                            SortedList<string, int> lkids2 = new SortedList<string, int>();
                            foreach (int kid2 in kidlist)
                            {
                                if (!lkids2.ContainsKey(taxondict[kid2].Name) && !isincert(taxondict[kid2].Name))
                                    lkids2.Add(taxondict[kid2].Name, kid2);
                            }
                            foreach (int kid2 in taxondict[taxonid].dyntaxa_Children)
                            {
                                string kidname = taxondict[kid2].Name;
                                if (!String.IsNullOrEmpty(taxondict[kid2].dyntaxa_name))
                                {
                                    if ((rank != "genus") || (taxondict[kid2].dyntaxa_name.Contains(taxondict[taxonid].Name)))
                                        kidname = taxondict[kid2].dyntaxa_name;

                                }
                                if ((rank != "genus") || (kidname.Contains(taxondict[taxonid].Name)))
                                    if (!lkids2.ContainsKey(kidname) && !isincert(kidname))
                                        lkids2.Add(kidname, kid2);
                            }
                            int ikid2 = 1;
                            foreach (string kidname2 in lkids2.Keys)
                            {
                                int kid2 = lkids2[kidname2];
                                cladestring += "    | " + ikid2.ToString() + "=" + itkid + linktaxon(kid2) + itkid + "\n";
                                ikid2++;
                            }
                            cladestring += "    }}\n";
                            ikid++;

                            //}

                            //Then the rest, alphabetically:
                            SortedList<string, int> lkids = new SortedList<string, int>();
                            foreach (int kid2 in siblist)
                            {
                                if ((kid2 != taxonid) && (taxondict[kid2].Name != taxondict[taxonid].Name))
                                    if (!lkids.ContainsKey(taxondict[kid2].Name) && !isincert(taxondict[kid2].Name))
                                        lkids.Add(taxondict[kid2].Name, kid2);
                            }
                            foreach (int kid2 in taxondict[parent[0]].dyntaxa_Children)
                            {
                                string kidname = taxondict[kid2].Name;
                                if (!String.IsNullOrEmpty(taxondict[kid2].dyntaxa_name))
                                    if ((rank != "genus") || (taxondict[kid2].dyntaxa_name.Contains(taxondict[taxonid].Name)))
                                        kidname = taxondict[kid2].dyntaxa_name;

                                if ((kid2 != taxonid) && (taxondict[kid2].Name != taxondict[taxonid].Name))
                                    if (!lkids.ContainsKey(kidname) && !isincert(kidname))
                                        lkids.Add(kidname, kid2);
                            }
                            foreach (string kidname in lkids.Keys)
                            {
                                kid = lkids[kidname];

                                cladestring += "  | " + ikid.ToString() + "=" + ittit + linktaxon(kid) + ittit + "\n";
                                ikid++;
                            }
                            cladestring += "  }}\n";
                            cladestring += "}}\n";
                        }
                        else if (nkids < 30)
                        {
                            string anddyn = "";
                            cladestring += "\n\n" + mp(49, null) + COLref + anddyn + ":\n\n" + cladestart + "\n";
                            cladestring += "| label1 = " + ittit + "'''" + stitle + "&nbsp;'''" + ittit + "\n";
                            cladestring += "| 1=" + cladestart + "\n"; braces++; braces++;
                            int ikid = 1;
                            SortedList<string, int> lkids = new SortedList<string, int>();
                            foreach (int kid in kidlist)
                            {
                                if (!lkids.ContainsKey(taxondict[kid].Name) && !isincert(taxondict[kid].Name))
                                    lkids.Add(taxondict[kid].Name, kid);
                            }
                            foreach (int kid in taxondict[taxonid].dyntaxa_Children)
                            {
                                string kidname = taxondict[kid].Name;
                                if (!String.IsNullOrEmpty(taxondict[kid].dyntaxa_name))
                                    if ((rank != "genus") || (taxondict[kid].dyntaxa_name.Contains(taxondict[taxonid].Name)))
                                        kidname = taxondict[kid].dyntaxa_name;

                                if ((rank != "genus") || (kidname.Contains(taxondict[taxonid].Name)))
                                    if (!lkids.ContainsKey(kidname))
                                        lkids.Add(kidname, kid);
                            }
                            foreach (string kidname in lkids.Keys)
                            {
                                int kid = lkids[kidname];

                                cladestring += "  | " + ikid.ToString() + "=" + itkid + linktaxon(kid) + ittit + "\n";
                                ikid++;
                            }
                            cladestring += "  }}\n";
                            cladestring += "}}\n";
                        }
                        else //Lots of kids; make list
                        {
                            string anddyn = "";
                            //cladestring += util.initialcap(rank_latin_swe_def[rank]) + " "+ittit + stitle + ittit+" "+mp(50,null)+ COLref + anddyn + ":\n\n";
                            //string[] p66 = new string[2] {rank_latin_swe_def[rank],  ittit + stitle + ittit};
                            string[] p66 = new string[2] { "", ittit + stitle + ittit };
                            cladestring += "\n== " + mp(66, p66) + COLref + anddyn + " ==\n\n";
                            SortedList<string, int> lkids = new SortedList<string, int>();
                            foreach (int kid in kidlist)
                            {
                                if (!lkids.ContainsKey(taxondict[kid].Name) && !isincert(taxondict[kid].Name))
                                    lkids.Add(taxondict[kid].Name, kid);
                            }
                            foreach (int kid in taxondict[taxonid].dyntaxa_Children)
                            {
                                string kidname = taxondict[kid].Name;
                                if (!String.IsNullOrEmpty(taxondict[kid].dyntaxa_name))
                                    if ((rank != "genus") || (taxondict[kid].dyntaxa_name.Contains(taxondict[taxonid].Name)))
                                        kidname = taxondict[kid].dyntaxa_name;

                                if ((rank != "genus") || (kidname.Contains(taxondict[taxonid].Name)))
                                    if (!lkids.ContainsKey(kidname) && !isincert(kidname))
                                        lkids.Add(kidname, kid);
                            }
                            foreach (string kidname in lkids.Keys)
                            {
                                cladestring += "* " + itkid + "[[" + kidname + "]]" + itkid + "\n";
                            }

                        }
                    }

                    p.text += cladestring;
                }




                //{{Clade
                //| label1='''Euarchontoglires'''
                //| 1={{Clade
                //  | label1=[[Glires]]
                //  | 1={{Clade
                //     | 1=[[Gnagare]]
                //     | 2=[[Hardjur]]}}
                //  | label2=[[Euarchonta]]
                //  | 2={{Clade
                //     | 1=[[Spetsekorrar]]
                //     | 2={{Clade
                //        | 1=[[Pälsfladdrare]]
                //        | 2={{Clade
                //           | 1=†[[Plesiadapiformes]]
                //           | 2=[[Primater]]}}}}}}}}
                //}}




                //Fyll Taxobox
                //Först med standardparametrar:
                //Animals:
                string mainparamstring = "";

                if (makelang == "sv")
                {
                    if ((regnum_to_make != "Plantae") && (regnum_to_make != "Fungi"))
                        mainparamstring = "status|image|image_caption|domain_sv|domain|regnum_sv|regnum|phylum_sv|phylum|classis_sv|classis|ordo_sv|ordo|familia_sv|familia|genus_sv|genus|species_sv|species|taxon|taxon_authority|range_map|range_map_caption|image2|image2_caption";
                    else     //Plants:
                        mainparamstring = "status|image|image_caption|domain_sv|domain|regnum_sv|regnum|divisio_sv|divisio|classis_sv|classis|ordo_sv|ordo|familia_sv|familia|genus_sv|genus|species_sv|species|taxon|taxon_authority|range_map|range_map_caption|image2|image2_caption";
                }
                else
                {
                    if ((regnum_to_make != "Plantae") && (regnum_to_make != "Fungi"))
                        mainparamstring = "status|image|image_caption|domain|regnum|phylum|classis|ordo|familia|genus|species|binomial|binomial_authority|range_map|range_map_caption|image2|image2_caption";
                    else     //Plants:
                        mainparamstring = "status|image|image_caption|domain|regnum|divisio|classis|ordo|familia|genus|species|binomial|binomial_authority|range_map|range_map_caption|image2|image2_caption";

                }

                string[] mainparams = mainparamstring.Split('|');

                foreach (string par in mainparams)
                    p.SetTemplateParameter("Taxobox", par, "", true);

                if (makelang == "sv")
                    p.SetTemplateParameter("Taxobox", "range_map_caption", "Utbredningsområde", true);


                //... sedan med incertae sedis för de nivåer som inte blir satta

                string rrr = rank_name[taxondict[taxonid].Level];

                while (rank_above[rrr] != "regnum")
                {
                    rrr = rank_above[rrr];
                    p.SetTemplateParameter("Taxobox", rrr, "''[[incertae sedis]]''", true);
                }

                //... sedan riktiga parametrar

                for (int jp = jjmax; jp > -1; jp--)
                {
                    // first specials
                    string tskey = rank_name[taxondict[parent[jp]].Level] + ":" + taxondict[parent[jp]].Name;
                    if (taxospecial.ContainsKey(tskey))
                    {
                        string[] tss = taxospecial[tskey].Split('&');
                        foreach (string ts in tss)
                        {
                            string[] ts2 = ts.Split(':');
                            p.SetTemplateParameter("Taxobox", ts2[0], ts2[1], true);
                        }
                    }


                    //... then actual parameters  
                    if (String.IsNullOrEmpty(taxondict[parent[jp]].Name_sv) || (makelang != "sv"))
                    {
                        rrr = rank_name[taxondict[parent[jp]].Level];
                        if ((rrr == "phylum") && (regnum_to_make == "Plantae"))
                            rrr = "divisio";
                        memo("linklatin " + linklatin(parent[jp]));
                        string itg = "";
                        if (rrr == "genus")
                            itg = "''";
                        p.SetTemplateParameter("Taxobox", rrr, itg + linklatin(parent[jp]) + itg, true);
                        if (makelang == "sv")
                            p.SetTemplateParameter("Taxobox", rrr + "_sv", util.initialcap(taxondict[parent[jp]].Name_sv), true);
                    }
                    else
                    {
                        rrr = rank_name[taxondict[parent[jp]].Level];
                        if ((rrr == "phylum") && (regnum_to_make == "Plantae"))
                            rrr = "divisio";
                        p.SetTemplateParameter("Taxobox", rrr, taxondict[parent[jp]].Name, true);
                        p.SetTemplateParameter("Taxobox", rrr + "_sv", "[[" + util.initialcap(taxondict[parent[jp]].Name_sv) + "]]", true);
                    }
                }

                p.SetTemplateParameter("Taxobox", rank_name[taxondict[taxonid].Level], ittit + "'''" + taxondict[taxonid].Name + "'''" + ittit, true);
                if (makelang == "sv")
                    p.SetTemplateParameter("Taxobox", rank_name[taxondict[taxonid].Level] + "_sv", util.initialcap(taxondict[taxonid].Name_sv), true);


                if (makelang == "sv")
                {
                    p.SetTemplateParameter("Taxobox", "taxon", taxondict[taxonid].Name, true);
                    p.SetTemplateParameter("Taxobox", "taxon_authority", auktorclass.boxauthor(auktor), true);
                }
                else
                {
                    p.SetTemplateParameter("Taxobox", "binomial", ittit + taxondict[taxonid].Name + ittit, true);
                    p.SetTemplateParameter("Taxobox", "binomial_authority", auktorclass.boxauthor(auktor), true);
                }

                string[] p57 = new string[1] { groupname_attr[ggname] };


                p.SetTemplateParameter("Taxobox", "synonyms", makesynparam(taxonid, p.title, mp(1, null) + mp(57, p57)), true);

                //...sedan rödlistestatus
                //{{IUCN2012.2| assessors = | år = | id = | titel = | hämtdatum = }}

                iud = -1;
                if (taxondict[taxonid].iucnid > 0)
                {
                    iud = taxondict[taxonid].iucnid;
                }
                else if (iucnnamedict.ContainsKey(taxondict[taxonid].Name))
                {
                    iud = iucnnamedict[taxondict[taxonid].Name];
                }

                if (iud > 0)
                {
                    if (iucndict.ContainsKey(iud))
                    {
                        p.SetTemplateParameter("Taxobox", "status", iucndict[iud].status, true);
                        p.SetTemplateParameter("Taxobox", "status_ref", iucnref, true);
                        if (makelang != "sv")
                            p.SetTemplateParameter("Taxobox", "status_system", "iucn3.1", true);
                    }
                }

                if (taxondict[taxonid].redsvid > 0)
                {
                    int rs = taxondict[taxonid].redsvid;

                    if (redsvdict.ContainsKey(rs))
                    {
                        p.SetTemplateParameter("Taxobox", "sverigestatus", redsvdict[rs].status, true);
                        p.SetTemplateParameter("Taxobox", "sverigestatus_ref", redsvref, true);

                    }
                }



                //Reference list:
                reflist += "\n</references>\n\n";
                p.text += "\n\n== " + mp(51, null) + " ==\n\n" + reflist;

                //Do not make "live" iw and cats if in test mode:
                if (!String.IsNullOrEmpty(testprefix))
                    p.text += "\n<nowiki>\n";

                //Find iw:

                if (oldiwlist.Count > 0)
                {
                    iwfound = true;
                    if (!oldiwfromwd)
                    {
                        p.text += "\r\n";
                        foreach (string str in oldiwlist)
                            p.text += "\r\n[[" + str + "]]";
                    }
                }

                foreach (Site iwsite in iwsites)
                {
                    //if (iwfound)
                    //    break;

                    string iwlang = iwsite.language;
                    memo("iwlang = " + iwlang);

                    Page enp = new Page(iwsite, taxondict[taxonid].Name);
                    //enp.Load();
                    if (util.tryload(enp, 1) && enp.Exists())
                    {
                        int iloop = 0;
                        string red = redirect(enp);
                        //while (enp.IsRedirect())
                        if (!String.IsNullOrEmpty(red))
                        {
                            memo("resolving redirect " + red);
                            enp = new Page(iwsite, red);
                            if (!util.tryload(enp, 1))
                                continue;
                            if (!enp.Exists())
                                continue;
                            //iloop++;
                            //if (iloop > 2)
                            //    break;
                        }
                        if ((enp.title != taxondict[taxonid].Name) && (get_name_id(enp.title, "", "") > 0))
                        {
                            memo(enp.title + " found in database; breaking off.");
                            continue;
                        }
                        string templatename = "Taxobox";
                        if (!enp.text.Contains("axobox"))
                        {
                            if (enp.text.Contains("peciesbox"))
                                templatename = "Speciesbox";
                            else
                            {
                                memo("No taxobox");
                                if (enp.text.Length > 100)
                                    memo(enp.text.Substring(0, 100));
                                else
                                    memo(enp.text);
                                continue;
                            }
                        }

                        //String[] eniw = enp.GetInterWikiLinks();

                        if (!iwfound)
                        {

                            List<string> iwlist = new List<string>();
                            try
                            {
                                iwlist = enp.GetInterLanguageLinks();
                            }
                            catch (Exception e)
                            {
                                string message = e.Message;
                                memo(message);
                                Thread.Sleep(10000);//milliseconds
                            }

                            memo("iwlist.Count " + iwlist.Count);


                            bool makefound = false;
                            foreach (string iiw in iwlist)
                            {
                                if (iiw.Contains(makelang + ":"))
                                    makefound = true;
                            }

                            if (makefound)
                                continue;



                            iwlist.Add(iwlang + ":" + enp.title);
                            if (iwlist.Count > 0)
                            {
                                //p.AddInterWikiLinks(iwlist);
                                p.text += "\r\n";
                                foreach (string str in iwlist)
                                    p.text += "\r\n[[" + str + "]]";

                                iwfound = true;
                            }
                        }

                        List<string> imageparams = new List<string>() { "image", "range_map", "image2" };
                        foreach (string imp in imageparams)
                        {
                            List<string> images = new List<string>();
                            images = enp.GetTemplateParameter(templatename, imp);
                            if (images.Count == 0)
                            {
                                if (imp == "image")
                                {
                                    if (iwlang == "nl")
                                    {
                                        //many different taxoboxes in nl...
                                        foreach (string template in enp.GetTemplates(false, false))
                                        {
                                            if (template.Contains("axobox"))
                                            {
                                                memo("template = " + template);
                                                images = enp.GetTemplateParameter(template, "afbeelding");
                                                if (images.Count == 0)
                                                    images = enp.GetTemplateParameter(templatename, "image");
                                            }
                                        }

                                    }
                                    else if (iwlang == "sv")
                                        images = enp.GetTemplateParameter(templatename, "bild");
                                }
                            }
                            if (images.Count == 0)
                            {
                                if (oldimageparams.ContainsKey(imp))
                                    images.Add(oldimageparams[imp]);
                            }
                            foreach (string im in images)
                            {
                                if (!String.IsNullOrEmpty(im))
                                {
                                    memo("im = " + im);
                                    p.SetTemplateParameter("Taxobox", imp, im, true);
                                }

                            }
                        }

                        //if (regnum_to_make == "Fungi")
                        //{
                        //    string templateTitle = "Mycomorphbox";
                        //    Regex templateTitleRegex = new Regex("^\\s*(" +
                        //        Bot.Capitalize(Regex.Escape(templateTitle)) + "|" +
                        //        Bot.Uncapitalize(Regex.Escape(templateTitle)) +
                        //        ")\\s*\\|");

                        //    //bool mmbfound = false;
                        //    //foreach (string template in enp.GetTemplatesWithParams())
                        //    //{
                        //    //    if (templateTitleRegex.IsMatch(template))
                        //    //    {
                        //    //        //Ok, found the right template. Now get params:
                        //    //        //mmbfound = true;
                        //    //        memo(template);
                        //    //        p.text = p.text.Replace(svampfaktaplats, svampfaktamall);
                        //    //        p.SetTemplateParameter("Svampfakta", "namn", p.title,true);
                        //    //        Dictionary<string, string> enparameters = makesite.ParseTemplate(template);
                        //    //        foreach (string op in enparameters.Keys)
                        //    //        {
                        //    //            memo("op=" + op + "|");
                        //    //            if (svampfakta.ContainsKey(op) && svampfakta.ContainsKey(enparameters[op]))
                        //    //            {
                        //    //                memo("setting param |" + svampfakta[op] + " = " + svampfakta[enparameters[op]]);
                        //    //                p.SetTemplateParameter("Svampfakta", svampfakta[op], svampfakta[enparameters[op]], true);
                        //    //            }
                        //    //        }
                        //    //    }
                        //    //}
                        //    //if (!mmbfound)
                        //    //p.text = p.text.Replace(svampfaktaplats, "");
                        //}




                    }
                }


                string extlinks = "";

                //Find commonscat:
                //Page cmp = new Page(cmsite, "Category:"+taxondict[taxonid].Name);
                //cmp.Load();
                //if (util.tryload(cmp,1)&&cmp.Exists())
                //{
                //    extlinks += "\n{{commonscat|" + taxondict[taxonid].Name + "|"+ittit + taxondict[taxonid].Name + ittit + "}}\n";
                //}

                ////Find wikispecies:
                //Page wsp = new Page(wssite, taxondict[taxonid].Name);
                ////wsp.Load();
                //if (util.tryload(wsp, 1) && wsp.Exists())
                //{
                //    extlinks += "\n{{wikispecies|" + taxondict[taxonid].Name + "|" + ittit + taxondict[taxonid].Name + ittit + "}}\n";
                //}

                //Artfaktablad: http://www.artfakta.se/artfaktablad/Antrodia_Primaeva_72.pdf (numret är rödliste-id)
                if (taxondict[taxonid].redsvid > 0)
                {
                    string artfakta = "\n* [http://www.artfakta.se/artfaktablad/";
                    string[] namesplit = taxondict[taxonid].Name.Split();
                    if (namesplit.Length == 2)
                    {
                        artfakta += util.initialcap(namesplit[0]) + "_" + util.initialcap(namesplit[1]) + "_" + taxondict[taxonid].redsvid.ToString() + ".pdf Artfaktablad för " + taxondict[taxonid].Name + "]\n";
                        extlinks += artfakta;
                    }
                }

                if (!String.IsNullOrEmpty(extlinks))
                    p.text += "\n\n== " + mp(52, null) + " ==\n\n" + extlinks + "\n\n";

                //picture gallery:

                if (oldpiclist.Count > 0)
                    p.text += makegallery(oldpiclist);

                // stub template:

                if ((makelang == "sv") && (sweparent > 0))
                {
                    //string[] p6162 = new string[1] { taxondict[sweparent].Name_sv.ToLower() };
                    //string templatename = mp(62, p6162);
                    //p.text += "\n\n{{" + templatename + "}}\n\n";
                    //if (!donetemplates.Contains(templatename))
                    //{
                    //    Page ptemp = new Page(makesite, mp(63, null) + templatename);
                    //    util.tryload(ptemp, 1);
                    //    if (!ptemp.Exists())
                    //    {
                    //        ptemp.text = mp(61, p6162);
                    //        if (CBmemo.Checked)
                    //            memo(ptemp.text);
                    //        else
                    //                    if (util.trysave(ptemp, 2,"Make template"))
                    //            donetemplates.Add(templatename);

                    //        Page pcat = new Page(makesite, mp(1, null) + mp(64, p6162));
                    //        pcat.text = "";
                    //        bool swepfound = false;
                    //        List<string> swepars = new List<string>();

                    //        for (int ij = 0; ij <= jjmax; ij++)
                    //        {
                    //            if ((swepfound) && (parentname[ij] != taxondict[parent[ij]].Name))
                    //            {
                    //                swepars.Add(parentname[ij]);
                    //            }
                    //            if (parent[ij] == sweparent)
                    //                swepfound = true;

                    //        }
                    //        foreach (string scat in swepars)
                    //        {
                    //            if (String.IsNullOrEmpty(scat))
                    //                break;
                    //            p6162[0] = scat;
                    //            pcat.AddToCategory(mp(64, p6162));
                    //            if (CBmemo.Checked)
                    //                memo(pcat.text);
                    //            else if ((util.trysave(pcat, 2,"Making category")) && reallymake)
                    //            {
                    //                string sscat = pcat.title;
                    //                donecats.Add(sscat);
                    //            }
                    //            pcat.title = mp(1, null) + mp(64, p6162);
                    //            pcat.text = null;
                    //            if (donecats.Contains(pcat.title))
                    //                break;
                    //            util.tryload(pcat, 1);
                    //            if (pcat.Exists())
                    //            {
                    //                string sscat = pcat.title;
                    //                donecats.Add(sscat);
                    //                break;
                    //            }
                    //            pcat.text = "";
                    //        }

                    //    }
                    //}


                }

                // Category by trivial name:

                if ((sweparent > 0) && (makelang != "war"))
                {
                    //p.AddToCategory(util.initialcap(taxondict[sweparent].Name_sv));

                    string preposition = " " + mp(93, null) + " ";

                    //First make the basic categories...
                    List<string> catending = new List<string>(){ "", " "+mp(95,null) };
                    //... then make regnumcats for these...
                    foreach (string catend in catending)
                        make_regnumcat(catend, "",false,"");
                    //... then add geography categories...
                    foreach (string ds in distcatlist)
                        if (!string.IsNullOrEmpty(ds))
                        {
                            catending.Add(preposition + ds);
                            make_regnumcat(ds, preposition, true, distributionclass.getpartof(ds));
                        }
                    //... and geography regnumcats will be made inside and after the main catend loop.

                    foreach (string catend in catending)
                    {

                        string pcattit = mp(1, null) + util.initialcap(taxondict[sweparent].Name_sv + catend);
                        memo("pcattit=" + pcattit);
                        if (catend != " paghimo ni bot")
                            p.AddToCategory(pcattit);
                        Page pcat = new Page(makesite, pcattit);
                        if (pcat == null)
                            memo("pcat is null!");
                        if (util.tryload(pcat, 2))
                            if (!pcat.Exists())
                            {
                                pcat.text = "";
                                if (!string.IsNullOrEmpty(catend))
                                {
                                    if (catend.Contains(mp(95,null)))
                                        pcat.AddToCategory(util.initialcap(taxondict[sweparent].Name_sv));
                                    else
                                    {
                                        string bygeotitle = mp(1, null) + util.initialcap(taxondict[sweparent].Name_sv) + " " + mp(94, null);
                                        pcat.AddToCategory(bygeotitle);
                                        if (!donecats.Contains(bygeotitle))
                                        {
                                            Page pbygeo = new Page(makesite, bygeotitle);
                                            util.tryload(pbygeo, 1);
                                            if (!pbygeo.Exists())
                                            {
                                                pbygeo.text = "";
                                                pbygeo.AddToCategory(taxondict[sweparent].Name_sv);
                                                if (util.trysave(pbygeo, 2, "Making category"))
                                                    donecats.Add(bygeotitle);
                                            }
                                            else
                                                donecats.Add(bygeotitle);
                                        }
                                    }

                                }
                                bool swepfound = false;
                                List<string> swepars = new List<string>();

                                for (int ij = 0; ij <= jjmax; ij++)
                                {
                                    if ((swepfound) && (parentname[ij] != taxondict[parent[ij]].Name))
                                    {
                                        swepars.Add(parentname[ij]);
                                    }
                                    if (parent[ij] == sweparent)
                                        swepfound = true;

                                }
                                //List<string> geopartlist = new List<string>();
                                string oldscat = taxondict[sweparent].Name_sv;
                                foreach (string scat in swepars)
                                {
                                    if (String.IsNullOrEmpty(scat))
                                        break;
                                    pcat.AddToCategory(scat + catend);
                                    if (!string.IsNullOrEmpty(catend))
                                    {
                                        string geopartof = distributionclass.getpartof(catend.Substring(preposition.Length));
                                        memo("geopartof(" + catend.Substring(preposition.Length) + ") = " + geopartof);
                                        string geocatname = pcat.title.Replace(catend, preposition + geopartof);//mp(1, null) + util.initialcap(scat) + preposition + geopartof;
                                        if (!string.IsNullOrEmpty(geopartof))
                                        {
                                            pcat.AddToCategory(geocatname);
                                            //pcat.AddToCategory(catend);
                                            make_regnumcat(catend, preposition, false, geopartof);
                                        }
                                        else
                                        {
                                            make_regnumcat(catend, preposition, true, "");

                                        }
                                        while (!string.IsNullOrEmpty(geopartof))
                                        {
                                            //make geographical hierarchy
                                            if (donecats.Contains(geocatname))
                                                break;
                                            Page pgeocat = new Page(makesite, geocatname);
                                            util.tryload(pgeocat, 1);
                                            if (!pgeocat.Exists())
                                            {
                                                pgeocat.text = "";
                                                //pgeocat.AddToCategory(geopartof);
                                                string oldgeopartof = geopartof;
                                                geopartof = distributionclass.getpartof(geopartof);
                                                memo("geopartof(" + oldgeopartof + ") = " + geopartof);
                                                if (!string.IsNullOrEmpty(geopartof))
                                                {
                                                    pgeocat.AddToCategory(util.initialcap(oldscat) + preposition + geopartof);
                                                    make_regnumcat(oldgeopartof, preposition, false, geopartof);
                                                }
                                                else
                                                {
                                                    pgeocat.AddToCategory(util.initialcap(oldscat) + " " + mp(94, null));
                                                    pgeocat.AddToCategory(util.initialcap(scat) + preposition + oldgeopartof);
                                                    make_regnumcat(oldgeopartof, preposition, true, "");
                                                    //if (!geopartlist.Contains(geopartof))
                                                    //{
                                                    //    geopartlist.Add(oldgeopartof);
                                                    //    memo("geopartlist " + oldgeopartof);
                                                    //}
                                                }
                                                if (util.trysave(pgeocat, 2, "Making category"))
                                                {
                                                    donecats.Add(pgeocat.title);
                                                }
                                                geocatname = geocatname.Replace(oldgeopartof, geopartof);
                                            }
                                            else
                                                donecats.Add(pgeocat.title);
                                        }

                                    }

                                    if (CBmemo.Checked)
                                        memo(pcat.text);
                                    else if ((util.trysave(pcat, 2, "Making category")) && reallymake)
                                    {
                                        string sscat = pcat.title;
                                        donecats.Add(sscat);
                                    }
                                    pcat = new Page(makesite, mp(1, null) + util.initialcap(scat) + catend);
                                    if (donecats.Contains(pcat.title))
                                        break;
                                    util.tryload(pcat, 1);
                                    if (pcat.Exists())
                                    {
                                        string sscat = pcat.title;
                                        donecats.Add(sscat);
                                        break;
                                    }
                                    pcat.text = "";
                                    if (!string.IsNullOrEmpty(catend))
                                    {
                                        if (catend.Contains(mp(95, null)))
                                            pcat.AddToCategory(util.initialcap(scat));
                                        else
                                        {
                                            string bygeotitle = mp(1, null) + util.initialcap(scat) + " " + mp(94, null);
                                            pcat.AddToCategory(bygeotitle);
                                            if (!donecats.Contains(bygeotitle))
                                            {
                                                Page pbygeo = new Page(makesite, bygeotitle);
                                                util.tryload(pbygeo, 1);
                                                if (!pbygeo.Exists())
                                                {
                                                    pbygeo.text = "";
                                                    pbygeo.AddToCategory(util.initialcap(scat));
                                                    if (util.trysave(pbygeo, 2, "Making category"))
                                                        donecats.Add(bygeotitle);
                                                }
                                                else
                                                    donecats.Add(bygeotitle);
                                            }

                                        }
                                    }
                                    oldscat = scat;
                                }
                                //foreach (string gp in geopartlist)
                                //    make_regnumcat(gp, preposition,true);

                                //string regnumcattit = mp(1, null) + regnum_to_make_sv.ToLower().Replace("mga ","") + catend;
                                //if (!donecats.Contains(regnumcattit))
                                //{
                                //    Page pregnumcat = new Page(makesite, regnumcattit);
                                //    util.tryload(pregnumcat, 1);
                                //    if (!pregnumcat.Exists())
                                //    {
                                //        pregnumcat.text = "";
                                //        if (String.IsNullOrEmpty(catend))
                                //            pregnumcat.AddToCategory("Biyolohiya");
                                //        else if (catend.Contains(mp(95,null)))
                                //            pregnumcat.AddToCategory(regnum_to_make_sv.ToLower().Replace("mga ", ""));
                                //        else
                                //        {
                                //            pregnumcat.AddToCategory(regnum_to_make_sv.ToLower().Replace("mga ", "") + " " + mp(94, null));
                                //            pregnumcat.AddToCategory(catend.Substring(preposition.Length));
                                //        }
                                //        if (util.trysave(pregnumcat, 2, "Making category"))
                                //            donecats.Add(regnumcattit);
                                //    }
                                //    else
                                //        donecats.Add(regnumcattit);
                                //}
                                
                            }
                    }
                }

                //Category by scientific name:

                string latincat = taxondict[taxonid].Name;
                int nextpar = 0;
                if (rank == "species")
                {
                    latincat = taxondict[parent[0]].Name;
                    nextpar = 1;
                }

                if (p.title != taxondict[taxonid].Name && String.IsNullOrEmpty(testprefix))
                {
                    util.make_redirect(testprefix + taxondict[taxonid].Name, p.title, latincat, reallymake && !CBmemo.Checked);
                    if (makelang == "ceb") //redirect from plural to base form:
                    {
                        if (!p.title.Contains("mga") && String.IsNullOrEmpty(testprefix))
                            util.make_redirect("Mga " + p.title.ToLower(), p.title, "", reallymake && !CBmemo.Checked);
                    }
                }
                else
                {
                    p.AddToCategory(latincat);
                }

                Page pcat2 = new Page(makesite, mp(1, null) + latincat);
                pcat2.text = null;

                string prevcat = "";

                for (int ij = nextpar; ij < 19; ij++)
                {
                    memo("pcat2.title = " + pcat2.title);
                    string nextcat = taxondict[parent[ij]].Name;

                    if (donecats.Contains(pcat2.title))
                    {
                        memo("donecat-break");
                        break;
                    }
                    if (!util.tryload(pcat2, 1))
                        break;
                    if (pcat2.Exists())
                    {
                        if (!pcat2.text.Contains(nextcat))
                        {
                            string origtitle = pcat2.title;
                            pcat2 = new Page(makesite, pcat2.title + " (" + nextcat + ")");

                            memo("Category name changed to " + pcat2.title);

                            if (!donecatconf.Contains(pcat2.title))
                            {
                                donecatconf.Add(pcat2.title);
                                util.tryload(pconflict, 1);
                                pconflict.text += "\n*[[:" + origtitle + "]] ";
                                pconflict.text += "[[:" + pcat2.title + "]]";
                                util.trysave(pconflict, 1, "Logging conflict");
                            }

                            util.tryload(pcat2, 1);
                            if (ij == nextpar)
                            {
                                if (p.title == taxondict[taxonid].Name)
                                {
                                    p.RemoveFromCategory(origtitle);
                                    p.AddToCategory(pcat2.title);
                                }
                                //else
                                //{
                                //    Page ppc = new Page(makesite, taxondict[taxonid].Name);
                                //    if (util.tryload(ppc, 1))
                                //    {
                                //        ppc.RemoveFromCategory(origtitle);
                                //        ppc.AddToCategory(pcat2.title);
                                //        trysave(ppc, 2);
                                //    }

                                //}
                            }
                            //else
                            //{
                            //    Page ppc = new Page(makesite, prevcat);
                            //    if (util.tryload(ppc, 1))
                            //    {
                            //        ppc.RemoveFromCategory(origtitle);
                            //        ppc.AddToCategory(pcat2.title);
                            //        trysave(ppc, 2);
                            //    }

                            //}

                            if (pcat2.Exists())
                            {
                                string scat = pcat2.title;
                                donecats.Add(scat);
                                break;
                            }
                        }
                        else
                        {
                            string scat = pcat2.title;
                            donecats.Add(scat);
                            break;
                        }
                    }

                    pcat2.text = "";

                    if (nextcat == regnum_to_make)
                    {
                        string[] p53 = new string[1] { groupname_plural[regnum_to_make] };
                        nextcat = util.initialcap(mp(53, p53));
                    }
                    else if (taxotop.ContainsKey(nextcat))
                    {
                        Page pg = new Page(makesite, mp(13, null) + botname + "/DoubleTaxon");
                        pg.Load();
                        pg.text += "\n* Double taxon: " + taxondict[taxonid].Name + "; \n** parents = ";
                        foreach (string pp in parentname)
                            pg.text += pp + ";";
                        pg.text += "\n";
                        pg.text += "\n";
                        pg.Save();

                        break;
                    }
                    pcat2.AddToCategory(nextcat);
                    if (CBmemo.Checked)
                        memo(pcat2.text);
                    else
                        if ((util.trysave(pcat2, 2, "Making category")) && reallymake)
                        donecats.Add(pcat2.title);
                    if (nextcat.Contains(mp(54, null)))
                        break;
                    prevcat = pcat2.title;
                    pcat2 = new Page(makesite, mp(1, null) + nextcat);
                    pcat2.text = null;

                }
                memo("After latincat loop");

                //Do not make "live" iw and cats if in test mode:
                if (!String.IsNullOrEmpty(testprefix))
                    p.text += "\n</nowiki>\n";

                //Cleanup:
                p.text = p.text.Replace("  ", " ");
                p.text = p.text.Replace("\n ", "\n");
                p.text = p.text.Replace("\n\n\n", "\n\n");

                //FINISHED!  Now display and save:
                //memo(p.text);
                if (CBmemo.Checked)
                    memo(p.text);
                else if (!util.trysave(p, 4, "Bot-making new article"))
                {
                    pfail.text += "Make failed at save for " + p.title;
                    if (util.failures > 3)
                    {
                        //Print statistics at the end:
                        memo(stats.GetStat());
                        pstats.text += "\n\n== [[" + taxon_to_make + " (partial)]] ==\n\n";
                        pstats.text += stats.GetStat();
                        //if (CBmemo.Checked)
                        //    memo(pstats.text);
                        //else
                        //    util.trysave(pstats, 1, "Statistics");

                        throw new Exception("Too many failed saves");
                    }
                    util.trysave(pfail, 1, "Logging failure");
                    memo("SAVE FAILED " + p.title);
                    return;
                }
                else
                {
                    plregnum.Add(p);
                }


            }

            idone++;
            int iremain = itodo - idone;

            memo(idone.ToString() + " articles done, " + iremain.ToString() + " remaining.");

            memo("=========================================================================");

            //if ((idone % 100) == 0)
            //{
            //    memo("Garbage collection:");
            //    GC.Collect();
            //}
            //}

            if (!string.IsNullOrEmpty(resume_at))
            {
                //memo("Resume at      = " + resume_at);
                reallymake = false;
            }
            else
                reallymake = ((idone % nskip) == offset);

            if (CB_recursive.Checked)
            {
                if (rank_order[rank] > rank_order[bottomrank]) //Don't make articles for ranks below bottomrank
                {
                    int nkids = taxondict[taxonid].Children.Count;
                    memo("# children for " + taxondict[taxonid].Name + " = " + nkids.ToString());
                    foreach (int j in taxondict[taxonid].Children)
                        makearticle(j);

                    string donerank = "familia";
                    //string donerank = "genus";
                    if (rank_order[rank] >= rank_order[donerank])
                    {
                        var q = from c in db.DoneTable
                                where c.TaxonID == taxonid
                                where c.Makelang == makelang
                                select c;
                        if (q.Count() == 0)
                        {
                            DoneTable dt = new DoneTable();
                            dt.ID = (from c in db.DoneTable select c.ID).Max() + 1;
                            dt.Name = taxondict[taxonid].Name;
                            dt.TaxonID = taxonid;
                            dt.Makelang = makelang;
                            db.DoneTable.InsertOnSubmit(dt);
                            db.SubmitChanges();
                        }
                    }
                }
            }
        }

        public void make_regnumcat(string catendpar, string preposition, bool saheyo,string geopartof)
        {
            string catend = catendpar;
            if (catendpar.StartsWith(preposition))
                catend = catendpar.Substring(preposition.Length);
            string regnumcattit = mp(1, null) + regnum_to_make_sv.ToLower().Replace("mga ", "") + preposition + catend;
            memo("makeregnumcat: " + regnumcattit);
            if (!donecats.Contains(regnumcattit))
            {
                Page pregnumcat = new Page(makesite, regnumcattit);
                util.tryload(pregnumcat, 1);
                if (!pregnumcat.Exists())
                {
                    pregnumcat.text = "";
                    if (String.IsNullOrEmpty(catend))
                        pregnumcat.AddToCategory("Biyolohiya");
                    else if (catend.Contains(mp(95, null)))
                        pregnumcat.AddToCategory(regnum_to_make_sv.ToLower().Replace("mga ", ""));
                    else
                    {
                        if (saheyo)
                            pregnumcat.AddToCategory(regnum_to_make_sv.ToLower().Replace("mga ", "") + " " + mp(94, null));
                        if (!String.IsNullOrEmpty(geopartof))
                            pregnumcat.AddToCategory(regnum_to_make_sv.ToLower().Replace("mga ", "") + preposition+geopartof);
                        pregnumcat.AddToCategory(catend);
                    }
                    if (util.trysave(pregnumcat, 2, "Making category"))
                        donecats.Add(regnumcattit);
                }
                else
                    donecats.Add(regnumcattit);
            }

        }
        public bool login(string makelang,string makewiki)
        {
            //Console.Write("Password: ");
            password = util.get_password();
            botname = "Lsjbot";
            try
            {
                makesite = new Site("https://" + makewiki + ".wikipedia.org", botname, password);
                makesite.defaultEditComment = "Making articles";
                makesite.minorEditByDefault = true;
                return true;
            }
            catch (Exception e)
            {
                memo(e.Message);
                return false;
            }

        }

        private void fill_taxotop()
        {
            if (taxotop.Count > 0)
                return;

            var q = from c in db.Taxon
                    where c.TaxonRank == "kingdom"
                    select c;
            foreach (Taxon tt in q)
            {
                taxotop.Add(tt.ScientificName, tt.TaxonID);
            }
        }

        

        private void Setupbutton_Click(object sender, EventArgs e)
        {

            makelang = LBlang.SelectedItem.ToString();
            makewiki = LBwiki.SelectedItem.ToString();


            //regnum_to_make = "Plantae";
            regnum_to_make = LBregnum.SelectedItem.ToString();

            switch (regnum_to_make)
            {
                case "Fungi":
                    regnum_to_make_sv = mp(25, null);
                    break;
                case "Plantae":
                    regnum_to_make_sv = mp(26, null);
                    break;
                case "Animalia":
                    regnum_to_make_sv = mp(27, null);
                    break;
                case "Bacteria":
                    regnum_to_make_sv = mp(28, null);
                    break;
                case "Protozoa":
                    regnum_to_make_sv = mp(29, null);
                    break;
                case "Archaea":
                    regnum_to_make_sv = mp(30, null);
                    break;
                case "Viruses":
                    regnum_to_make_sv = mp(31, null);
                    break;
                case "Chromista":
                    regnum_to_make_sv = mp(32, null);
                    break;
            }


            if (taxotop.Count == 0) //otherwise dicts are already filled
            {
                memo("Fill taxotop");
                fill_taxotop();

                memo("Filling dicts:");
                auktorclass.fill_auktordict(regnum_to_make);
                memo("auktordict done");
                fill_statusdicts();
                memo("statusdicts done");
                filldonetree();
                memo("donetree done");


                read_taxonomic_rank();
                memo("ranks done");
                read_COL_source_ref();
                memo("COL sources done");

                read_doubles();
                memo("doubles done");

                read_groupname();

                read_trivname();
                memo("trivnames done");

                read_IUCN();
                memo("IUCN done");
            }

            //memo("Reading species tree from db");

            string ss = TBtaxon.Text;
            int ssid = util.tryconvert(ss);
            if (ssid > 0)
            {
                memo("TaxonID " + ssid);
                Taxon tt = (from c in db.Taxon where c.TaxonID == ssid select c).First();
                if (tt == null)
                {
                    ssid = -1;
                    memo("Invalid taxonID " + ss);
                }
                else
                {
                    ss = util.dbtaxon_name(tt);
                    memo(ss);
                }
            }
            else
               ssid = get_name_id(ss, "", "");

            if ( ssid < 0)
            {
                memo("Invalid taxon " + ss);
                return;
            }


            resume_at = TBresumeat.Text;
            if (!string.IsNullOrEmpty(resume_at))
            {
                memo("Resume at      = " + resume_at);
                reallymake = false;
                Taxonclass tc = Taxonclass.loadtaxon(ssid, db, makelang);
                if (tc.parentdict["regnum"] != regnum_to_make)
                {
                    memo("Wrong regnum");
                    return;
                }

                //memo("Front-loading species tree from db");

                //taxondictadd(ssid, tc);

                //List<Taxon> taxonlist = util.get_subordinates(tc, db);

                //memo("#subordinates: " + taxonlist.Count);

                //int nt = 0;
                //foreach (Taxon tt in taxonlist)
                //{
                //    taxondictadd(tt.TaxonID, new Taxonclass(tt, db, makelang));
                //    nt++;
                //    if (nt % 100 == 0)
                //        memo(nt + ": " + tt.ScientificName);
                //}

            }


            SystemSounds.Beep.Play();

            if (!loggedin)
                loggedin = login(makelang,makewiki);

            if (!loggedin)
                return;

            stats.SetMilestone(10000, makesite);


            if (nskip == 1)
                offset = 0;

            talkprefix = mp(38, null);

            testprefix = TBtestprefix.Text;
            if (makewiki != "ceb")
                testprefix = testprefix.Replace("Gumagamit:", mp(13, null));

            //makelang = LBlang.SelectedItem.ToString();

            memo("Logging in to iw sites:");
            foreach (string iwl in iwlang)
            {
                if (iwl != makelang)
                {
                    memo(iwl);
                    Site ssite = new Site("https://" + iwl + ".wikipedia.org", botname, password);
                    iwsites.Add(ssite);
                }
            }
            //cmsite = new Site("https://commons.wikimedia.org", botname, password);
            //wssite = new Site("https://species.wikimedia.org", botname, password);



            plregnum = new PageList(makesite);
            //plregnum.FillFromCategoryTree(initialcap(regnum_to_make_sv));

            memo("Making auxiliary pages:");

            pconflict = new Page(makesite, mp(13, null) + botname + "/Naming conflicts");
            pconflict.Load();
            pfail = new Page(makesite, mp(13, null) + botname + "/Uncreated");
            pfail.Load();
            ptree = new Page(makesite, mp(13, null) + botname + "/Tree-Plantae");
            ptree.Load();
            pdynsplit = new Page(makesite, mp(13, null) + botname + "/Dynsplit");
            pdynsplit.Load();
            pstats = new Page(makesite, mp(13, null) + botname + "/Statistics");
            pstats.Load();


            foreach (int t in taxondict.Keys)
            {
                if (!String.IsNullOrEmpty(taxondict[t].Name_sv))
                    if (!swe_name.ContainsKey(t))
                        swe_name.Add(t, taxondict[t].Name_sv);
            }
            //foreach (int t in swe_name.Keys)
            //{
            //    if (!String.IsNullOrEmpty(swe_name[t]))
            //        if (String.IsNullOrEmpty(taxondict[t].Name_sv))
            //            taxondict[t].Name_sv = swe_name[t].ToLower();
            //}
            memo("swename done");

            memo("============================");
            memo("Done Setup");
            memo("============================");



            Gobutton.Enabled = true;
            makelistbutton.Enabled = true;
            SystemSounds.Asterisk.Play();
        }

        private void LBlang_SelectedIndexChanged(object sender, EventArgs e)
        {
            LBwiki.SelectedIndex = LBlang.SelectedIndex;
        }

        private void treebutton_Click(object sender, EventArgs e)
        {
            //if (!loggedin)
            //    loggedin = login();
            //ptree.text += "\n\n==" + regnum_to_make + "==\n\n";
            //printtree(taxotop[regnum_to_make], 1, 5);
            ////printtree(get_name_id("Aves","",""),1,3);
            //ptree.Save();
            memo("DUMMY");
        }

        private void Quitbutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Makelistbutton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "List with taxa to make";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fn = openFileDialog1.FileName;

                List<string> vetolist = new List<string>();
                if (CB_veto.Checked)
                {
                    openFileDialog1.Title = "Veto list with taxa NOT to make";
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string fnveto = openFileDialog1.FileName;
                        using (StreamReader sr = new StreamReader(fnveto))
                        {
                            while (!sr.EndOfStream)
                            {
                                string line = sr.ReadLine();
                                string[] words = line.Split('\t');
                                vetolist.Add(words[0]);
                            }
                        }
                    }
                }

                using (StreamReader sr = new StreamReader(fn))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] words = line.Split('\t');
                        if (!vetolist.Contains(words[0]))
                            maketaxon(words[0]);
                    }
                }
            }

            //Print statistics at the end:
            memo(stats.GetStat());
            pstats.text += "\n\n== [[" + taxon_to_make + "]] ==\n\n";
            pstats.text += stats.GetStat();
            if (CBmemo.Checked)
                memo(pstats.text);
            else
                util.trysave(pstats, 1, "Statistics");
            stats.nart = 0;
            stats.nredirect = 0;
            stats.ncat = 0;
            stats.nbot = 0;
            stats.ntalk = 0;
            stats.sizelist.Clear();

            memo("===========================");
            memo("Done making list");
            memo("===========================");
        }

        private void disttestbutton_Click(object sender, EventArgs e)
        {
            string ss = util.initialcap(TBtaxon.Text);
            int ssid = util.tryconvert(ss);
            if (ssid > 0)
            {
                memo("TaxonID " + ssid);
                Taxon tt = (from c in db.Taxon where c.TaxonID == ssid select c).First();
                if (tt == null)
                {
                    ssid = -1;
                    memo("Invalid taxonID " + ss);
                }
                else
                {
                    ss = util.dbtaxon_name(tt);
                    memo(ss);
                }
            }
            else
                ssid = get_name_id(ss, "", "");

            if (ssid < 0)
            {
                memo("Invalid taxon " + ss);
                return;
            }


            List<distributionclass> dcl = distributionclass.getdistribution(ssid, db);
            if (dcl.Count > 0)
            {
                memo("dcl " + dcl.Count);
                int nfail = 0;
                int nsuccess = 0;
                StringBuilder distsb = new StringBuilder("\n\n" + mp(37, null) + ":\n");
                foreach (distributionclass dc in dcl)
                {
                    //if (dcl.Count > ndcl)
                    //    memo(dc.towikistring("*"));
                    distsb.Append(dc.towikistring("*"));
                    if (dc.maindist == distributionclass.failstring)
                        nfail++;
                    else
                        nsuccess++;
                    foreach (distributionclass dcsub in dc.subdist)
                    {
                        //distsb.Append(dcsub.towikistring("**"));
                        if (dcsub.maindist == distributionclass.failstring)
                            nfail++;
                        else
                            nsuccess++;
                    }
                    //distcatlist = distcatlist.Union(dc.getcats()).ToList();

                }
                memo("nfail = " + nfail);
                memo("nsuccess = " + nsuccess);
                if ((nfail == 0) || (nsuccess > 2 * nfail))
                {
                    memo(distsb.ToString());
                }
            }

        }

        private void TBtestprefix_TextChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if ((DateTime.Now - util.oldtime).Minutes > 9)
            {
                Page pdum = new Page(makesite, "Gumagamit:Lsjbot/Dummy");
                pdum.text = "Dummy save to keep login active. " + DateTime.Now.ToShortTimeString();
                util.trysave(pdum, 1,"Dummy access to keep login active");
            }
        }
    }
}
