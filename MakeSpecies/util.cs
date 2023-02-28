using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Threading;
using System.IO;
using DotNetWikiBot;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;


namespace MakeSpecies
{
    class util
    {
        public static DateTime oldtime;
        public static int pausetime = 7; //time between saves, modified depending on task
        public static bool pauseaftersave = true;
        public static int failures = 0;

        

        public static string[] findtextinbrackets(string input, string start, string end)
        {
          
            Regex r = new Regex(Regex.Escape(start) +"(.*?)"  +Regex.Escape(end));
            MatchCollection matches = r.Matches(input);
            string[] rs = new string[matches.Count];
            int n = 0;
            foreach (Match m in matches)
            {
                rs[n] = m.Groups[1].Value;
                n++;
            }
            return rs;
        }

        public static string fixcase(string ss)
        {
            string s = String.Copy(ss);
            for (int i = 1; i < s.Length; i++)
            {
                if ((s[i - 1] != ' ') && (s[i - 1] != '.'))
                {
                    s = s.Remove(i, 1).Insert(i, Char.ToLower(s[i]).ToString());
                }
            }
            return s;
        }

        public static string unusedfilename(string fn0)
        {
            int n = 1;
            string fn = fn0;
            while (File.Exists(fn))
            {
                fn = fn0.Replace(".", n.ToString() + ".");
                n++;
            }
            return fn;
        }

        public static List<Taxon> get_subordinates(Taxonclass tc, COL2019 db)
        {
            string rank = MakeSpecies.rank_name[tc.Level];

            List<Taxon> rl;
            switch (rank)
            {
                case "regnum":
                    rl = (from c in db.Taxon
                          where c.Kingdom == tc.Name
                          where c.InfraspecificEpithet == null
                          select c).ToList();
                    break;
                case "phylum":
                    rl = (from c in db.Taxon
                          where c.Phylum == tc.Name
                          where c.InfraspecificEpithet == null
                          select c).ToList();
                    break;
                case "classis":
                    rl = (from c in db.Taxon
                          where c.Class == tc.Name
                          where c.InfraspecificEpithet == null
                          select c).ToList();
                    break;
                case "ordo":
                    rl = (from c in db.Taxon
                          where c.Order == tc.Name
                          where c.InfraspecificEpithet == null
                          select c).ToList();
                    break;
                case "superfamilia":
                    rl = (from c in db.Taxon
                          where c.Superfamily == tc.Name
                          where c.InfraspecificEpithet == null
                          select c).ToList();
                    break;
                case "familia":
                    rl = (from c in db.Taxon
                          where c.Family == tc.Name
                          where c.InfraspecificEpithet == null
                          select c).ToList();
                    break;
                case "genus":
                    rl = (from c in db.Taxon
                          where c.Genus == tc.Name
                          where c.InfraspecificEpithet == null
                          select c).ToList();
                    break;
                default:
                    rl = new List<Taxon>();
                    break;

            };

            return rl;
        }

        //public static List<string>

        public static string geonamesfolder = @"I:\dotnwb3\Geonames\"; 

        public static int tryconvert(string word)
        {
            int i = -1;

            if (word.Length == 0)
                return i;

            try
            {
                i = Convert.ToInt32(word);
            }
            catch (OverflowException)
            {
                Console.WriteLine("i Outside the range of the Int32 type: " + word);
            }
            catch (FormatException)
            {
                //if ( !String.IsNullOrEmpty(word))
                //    Console.WriteLine("i Not in a recognizable format: " + word);
            }

            return i;

        }

        public static long tryconvertlong(string word)
        {
            long i = -1;

            if (word.Length == 0)
                return i;

            try
            {
                i = Convert.ToInt64(word);
            }
            catch (OverflowException)
            {
                Console.WriteLine("i Outside the range of the Int64 type: " + word);
            }
            catch (FormatException)
            {
                //Console.WriteLine("i Not in a recognizable long format: " + word);
            }

            return i;

        }

        public static double tryconvertdouble(string word)
        {
            double i = -1;

            if (word.Length == 0)
                return i;

            try
            {
                i = Convert.ToDouble(word);
            }
            catch (OverflowException)
            {
                Console.WriteLine("i Outside the range of the Double type: " + word);
            }
            catch (FormatException)
            {
                try
                {
                    i = Convert.ToDouble(word.Replace(".", ","));
                }
                catch (FormatException)
                {
                    //Console.WriteLine("i Not in a recognizable double format: " + word.Replace(".", ","));
                }
                //Console.WriteLine("i Not in a recognizable double format: " + word);
            }

            return i;

        }

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

        public static string whichtaxobox(Page p)
        {
            //determines which variety of taxobox in an article
            string[] boxes = { "Taxobox2", "Taxobox", "taxobox2", "taxobox" };
            foreach (string b in boxes)
                if (p.text.Contains(b))
                    return b;
            return null;
        }

        public static string taxondiff(Page p1, Page p2)
        {
            //returns the highest taxonomic level at which p1 and p2 differes
            //if about the same taxon, return "SAME"
            //if either has no taxobox, return null

            string b1 = whichtaxobox(p1);
            if (b1 == null)
                return null;
            string b2 = whichtaxobox(p2);
            if (b2 == null)
                return null;

            string[] ranks = { "regnum", "phylum", "classis", "ordo", "familia", "genus", "species" };

            for (int i=0;i<ranks.Length;i++)
            {
                string s1 = p1.GetFirstTemplateParameter(b1, ranks[i]).Trim().Replace("[","").Replace("]","").ToLower();
                string s2 = p1.GetFirstTemplateParameter(b2, ranks[i]).Trim().Replace("[", "").Replace("]", "").ToLower();
                if (s1 != s2)
                    return ranks[i]+"\t"+s1+"\t"+s2;
            }
            return "SAME";

        }

        public static string taxondiff(Taxonclass t1,Taxonclass t2)
        {
            string[] ranks = { "regnum", "phylum", "classis", "ordo", "familia", "genus", "species" };

            for (int i = 0; i < ranks.Length; i++)
            {
                string s1 = "";
                if (t1.parentdict.ContainsKey(ranks[i]))
                    s1 = t1.parentdict[ranks[i]];
                string s2 = "";
                if (t2.parentdict.ContainsKey(ranks[i]))
                    s2 = t2.parentdict[ranks[i]];
                if (s1 != s2)
                    return ranks[i]+"\t"+s1 + "\t" + s2;
            }
            return "SAME";

        }

        public static string taxondiff(Taxon t1,Taxon t2)
        {
            if (t1.Kingdom != t2.Kingdom)
                return "regnum\t" + t1.Kingdom + "\t" + t2.Kingdom;
            if (t1.Phylum != t2.Phylum)
                return "phylum\t" + t1.Phylum + "\t" + t2.Phylum;
            if (t1.Class != t2.Class)
                return "classis\t" + t1.Class + "\t" + t2.Class;
            if (t1.Order != t2.Order)
                return "ordo\t" + t1.Order + "\t" + t2.Order;
            if (t1.Superfamily != t2.Superfamily)
                return "superfamilia\t" + t1.Superfamily + "\t" + t2.Superfamily;
            if (t1.Family != t2.Family)
                return "familia\t" + t1.Family + "\t" + t2.Family;
            if (t1.SpecificEpithet != t2.SpecificEpithet)
                return "species\t" + t1.SpecificEpithet + "\t" + t2.SpecificEpithet;
            return "SAME";
        }


        public static string dbtaxon_name(Taxon tt)
        {
            return dbtaxon_name(tt, true);
        }

        public static string dbtaxon_name(Taxon tt, bool withsub)
        {
            string name = tt.ScientificName;
            if (tt.SpecificEpithet != null)
            {
                name = tt.Genus + " " + tt.SpecificEpithet;
                if (withsub && tt.InfraspecificEpithet != null)
                    name += " " + tt.InfraspecificEpithet;
            }

            return name;
        }

        public static string taxon_from_page(Page p)
        {
            string tbox = whichtaxobox(p);
            return p.GetFirstTemplateParameter(tbox, "binomial").Trim('\'');
        }

        public static string regnum_from_page(Page p)
        {
            string tbox = whichtaxobox(p);
            return parse_wikilink(p.GetFirstTemplateParameter(tbox, "regnum"),true);
        }

        public static string parse_wikilink(string link,bool displaytext)
        {
            string s = link.Trim(new char[] { '[', ']' });
            if (s.Contains("|"))
            {
                if (displaytext)
                    return s.Split('|')[1];
                else
                    return s.Split('|')[0];
            }
            else
                return s;
        }

        public static string remove_disambig(string title)
        {
            string tit = title;
            if (tit.IndexOf("(") > 0)
                tit = tit.Remove(tit.IndexOf("(")).Trim();
            else if (tit.IndexOf(",") > 0)
                tit = tit.Remove(tit.IndexOf(",")).Trim();
            //if (tit != title)
            //    Console.WriteLine(title + " |" + tit + "|");
            return tit;
        }

        public static Site login(string makelang)
        {
            string password = util.get_password();
            string botkonto = "Lsjbot";
            Site newsite = new Site("https://" + makelang + ".wikipedia.org", botkonto, password);
            newsite.defaultEditComment = "Fixing mistake";
            newsite.minorEditByDefault = true;
            MakeSpecies.loggedin = true;
            return newsite;
        }



        public static string get_password()
        {
            InputBox ib = new InputBox("Password:",true);
            ib.ShowDialog();
            return ib.gettext();
        }


        public static void make_redirect(string frompage, string topage, string cat)
        {
            make_redirect(frompage, topage, cat, true);
        }

        public static void make_redirect(string frompage, string topage, string cat, bool reallymake)
        {
            Page pred = new Page(MakeSpecies.makesite, frompage);
            if (tryload(pred, 1))
            {
                if (!pred.Exists())
                {
                    pred.text = "#" + MakeSpecies.mp(2, null) + " [[" + topage + "]]";
                    if (!String.IsNullOrEmpty(cat))
                        pred.AddToCategory(cat);
                    trysave(pred, 2,"redirect");
                }

            }

        }

        public static string initialcap(string orig)
        {
            if (String.IsNullOrEmpty(orig))
                return "";

            int initialpos = 0;
            if (orig.IndexOf("[[") == 0)
            {
                if ((orig.IndexOf('|') > 0) && (orig.IndexOf('|') < orig.IndexOf(']')))
                    initialpos = orig.IndexOf('|') + 1;
                else
                    initialpos = 2;
            }
            string s = orig.Substring(initialpos, 1);
            s = s.ToUpper();
            string final = orig;
            final = final.Remove(initialpos, 1).Insert(initialpos, s);
            //s += orig.Remove(0, 1);
            return final;
        }



        public static bool tryload(Page p, int iattempt)
        {
            int itry = 1;


            while (true)
            {

                try
                {
                    p.Load();
                    return true;
                }
                catch (Exception e)
                {
                    string message = e.Message;
                    Console.Error.WriteLine(message);
                    itry++;
                    if (itry > iattempt)
                        return false;
                }
            }

        }

        public static bool trysave(Page p, int iattempt, Site site)
        {
            return trysave(p, iattempt, site.defaultEditComment);
        }

        public static bool trysave(Page p, int iattempt, string editcomment)
        {
            int itry = 1;


            while (true)
            {

                try
                {
                    //Bot.editComment = mp(60);
                    if (!MakeSpecies.savelocally)
                    {
                        p.Save(editcomment, false);
                        DateTime newtime = DateTime.Now;
                        while (newtime < oldtime)
                        {
                            newtime = DateTime.Now;
                            Thread.Sleep(1000);
                        }
                        oldtime = newtime.AddSeconds(pausetime);
                        MakeSpecies.stats.Add(p.title, p.text, MakeSpecies.pstats);
                    }
                    else
                    {
                        using (StreamWriter sw = new StreamWriter(MakeSpecies.savefolder + p.title + ".txt"))
                        {
                            sw.WriteLine(p.text);
                        }
                    }


                    if (pauseaftersave)
                    {
                        //Console.WriteLine("<ret>");
                        //Console.ReadKey();
                    }
                    failures = 0;
                    return true;
                }
                catch (Exception e)
                {
                    string message = e.Message;
                    Console.Error.WriteLine("trysave " + message);
                    if (message.Contains("CSRF"))
                    {
                        failures += 99;
                        itry += 99;
                    }
                    itry++;
                    if (itry > iattempt)
                    {
                        failures++;
                        return false;
                    }
                    else
                        Thread.Sleep(600000);//milliseconds
                }

            }

        }

        public static bool trymove(Page p, string newname, int iattempt, string editcomment)
        {
            int itry = 1;


            while (true)
            {

                try
                {
                    //Bot.editComment = mp(60);
                    if (!MakeSpecies.savelocally)
                    {
                        p.RenameTo(newname,editcomment);
                        DateTime newtime = DateTime.Now;
                        while (newtime < oldtime)
                        {
                            newtime = DateTime.Now;
                            Thread.Sleep(1000);
                        }
                        oldtime = newtime.AddSeconds(pausetime);
                    }
                    else
                    {
                        using (StreamWriter sw = new StreamWriter(MakeSpecies.savefolder + p.title + ".txt"))
                        {
                            sw.WriteLine(p.text);
                        }
                    }


                    if (pauseaftersave)
                    {
                        //Console.WriteLine("<ret>");
                        //Console.ReadKey();
                    }
                    return true;
                }
                catch (WebException e)
                {
                    string message = e.Message;
                    Console.Error.WriteLine("ts we " + message);
                    itry++;
                    if (itry > iattempt)
                        return false;
                    else
                        Thread.Sleep(600000);//milliseconds
                }
                catch (WikiBotException e)
                {
                    string message = e.Message;
                    Console.Error.WriteLine("ts wbe " + message);
                    if (message.Contains("Bad title"))
                        return false;
                    itry++;
                    if (itry > iattempt)
                        return false;
                    else
                        Thread.Sleep(600000);//milliseconds
                }
                catch (IOException e)
                {
                    string message = e.Message;
                    Console.Error.WriteLine("ts ioe " + message);
                    itry++;
                    if (itry > iattempt)
                        return false;
                    else
                        Thread.Sleep(600000);//milliseconds
                }

            }

        }

        public static List<string> findtag(string line)
        {
            string s = line;
            List<string> taglist = new List<string>();
            while (s.IndexOf('<') >= 0)
            {
                if (s.IndexOf('>') < s.IndexOf('<'))
                {
                    Console.WriteLine("Mismatched <> in " + line);
                    s = s.Substring(s.IndexOf('<') + 1);
                }
                else
                {
                    string tag = s.Substring(s.IndexOf('<'), s.IndexOf('>') - s.IndexOf('<') + 1);
                    //Console.WriteLine("Tag = " + tag);
                    taglist.Add(tag);
                    s = s.Substring(s.IndexOf('>') + 1);
                }

            }
            return taglist;
        }

        public static string fixtag(string line)
        {

            string s = line;

            s = s.Replace("<B>", "<b>");
            s = s.Replace("</B>", "</b>");
            s = s.Replace("<I>", "<i>");
            s = s.Replace("</I>", "</i>");

            if (s.Contains("<b>"))
            {
                if (s.Contains("</b>"))
                    s = s.Replace("<b>", "'''").Replace("</b>", "'''");
                else if (s.Contains("</i") && !s.Contains("<i>"))
                    s = s.Replace("<b>", "'''").Replace("</i>", "'''");
            }

            if (s.Contains("<i>"))
            {
                if (s.Contains("</i>"))
                    s = s.Replace("<i>", "''").Replace("</i>", "''");
                else if (s.Contains("</b") && !s.Contains("<b>"))
                    s = s.Replace("<i>", "''").Replace("</b>", "''");
            }

            s = s.Replace("<b>", "");
            s = s.Replace("</b>", "");
            s = s.Replace("<i>", "");
            s = s.Replace("</i>", "");


            return s;
        }

        public static bool is_latin(string name)
        {
            return (get_alphabet(name) == "latin");
        }

        public static string get_alphabet(string name)
        {
            char[] letters = name.ToCharArray();
            //char[] letters = remove_disambig(name).ToCharArray();
            int n = 0;
            int sum = 0;
            //int nlatin = 0;
            Dictionary<string, int> alphdir = new Dictionary<string, int>();
            foreach (char c in letters)
            {
                int uc = Convert.ToInt32(c);
                sum += uc;
                string alphabet = "none";
                if (uc <= 0x0040) alphabet = "none";
                //else if ((uc >= 0x0030) && (uc <= 0x0039)) alphabet = "number";
                //else if ((uc >= 0x0020) && (uc <= 0x0040)) alphabet = "punctuation";
                else if ((uc >= 0x0041) && (uc <= 0x007F)) alphabet = "latin";
                else if ((uc >= 0x00A0) && (uc <= 0x00FF)) alphabet = "latin";
                else if ((uc >= 0x0100) && (uc <= 0x017F)) alphabet = "latin";
                else if ((uc >= 0x0180) && (uc <= 0x024F)) alphabet = "latin";
                else if ((uc >= 0x0250) && (uc <= 0x02AF)) alphabet = "phonetic";
                else if ((uc >= 0x02B0) && (uc <= 0x02FF)) alphabet = "spacing modifier letters";
                else if ((uc >= 0x0300) && (uc <= 0x036F)) alphabet = "combining diacritical marks";
                else if ((uc >= 0x0370) && (uc <= 0x03FF)) alphabet = "greek and coptic";
                else if ((uc >= 0x0400) && (uc <= 0x04FF)) alphabet = "cyrillic";
                else if ((uc >= 0x0500) && (uc <= 0x052F)) alphabet = "cyrillic";
                else if ((uc >= 0x0530) && (uc <= 0x058F)) alphabet = "armenian";
                else if ((uc >= 0x0590) && (uc <= 0x05FF)) alphabet = "hebrew";
                else if ((uc >= 0x0600) && (uc <= 0x06FF)) alphabet = "arabic";
                else if ((uc >= 0x0700) && (uc <= 0x074F)) alphabet = "syriac";
                else if ((uc >= 0x0780) && (uc <= 0x07BF)) alphabet = "thaana";
                else if ((uc >= 0x0900) && (uc <= 0x097F)) alphabet = "devanagari";
                else if ((uc >= 0x0980) && (uc <= 0x09FF)) alphabet = "bengali";
                else if ((uc >= 0x0A00) && (uc <= 0x0A7F)) alphabet = "gurmukhi";
                else if ((uc >= 0x0A80) && (uc <= 0x0AFF)) alphabet = "gujarati";
                else if ((uc >= 0x0B00) && (uc <= 0x0B7F)) alphabet = "oriya";
                else if ((uc >= 0x0B80) && (uc <= 0x0BFF)) alphabet = "tamil";
                else if ((uc >= 0x0C00) && (uc <= 0x0C7F)) alphabet = "telugu";
                else if ((uc >= 0x0C80) && (uc <= 0x0CFF)) alphabet = "kannada";
                else if ((uc >= 0x0D00) && (uc <= 0x0D7F)) alphabet = "malayalam";
                else if ((uc >= 0x0D80) && (uc <= 0x0DFF)) alphabet = "sinhala";
                else if ((uc >= 0x0E00) && (uc <= 0x0E7F)) alphabet = "thai";
                else if ((uc >= 0x0E80) && (uc <= 0x0EFF)) alphabet = "lao";
                else if ((uc >= 0x0F00) && (uc <= 0x0FFF)) alphabet = "tibetan";
                else if ((uc >= 0x1000) && (uc <= 0x109F)) alphabet = "myanmar";
                else if ((uc >= 0x10A0) && (uc <= 0x10FF)) alphabet = "georgian";
                else if ((uc >= 0x1100) && (uc <= 0x11FF)) alphabet = "korean";
                else if ((uc >= 0x1200) && (uc <= 0x137F)) alphabet = "ethiopic";
                else if ((uc >= 0x13A0) && (uc <= 0x13FF)) alphabet = "cherokee";
                else if ((uc >= 0x1400) && (uc <= 0x167F)) alphabet = "unified canadian aboriginal syllabics";
                else if ((uc >= 0x1680) && (uc <= 0x169F)) alphabet = "ogham";
                else if ((uc >= 0x16A0) && (uc <= 0x16FF)) alphabet = "runic";
                else if ((uc >= 0x1700) && (uc <= 0x171F)) alphabet = "tagalog";
                else if ((uc >= 0x1720) && (uc <= 0x173F)) alphabet = "hanunoo";
                else if ((uc >= 0x1740) && (uc <= 0x175F)) alphabet = "buhid";
                else if ((uc >= 0x1760) && (uc <= 0x177F)) alphabet = "tagbanwa";
                else if ((uc >= 0x1780) && (uc <= 0x17FF)) alphabet = "khmer";
                else if ((uc >= 0x1800) && (uc <= 0x18AF)) alphabet = "mongolian";
                else if ((uc >= 0x1900) && (uc <= 0x194F)) alphabet = "limbu";
                else if ((uc >= 0x1950) && (uc <= 0x197F)) alphabet = "tai le";
                else if ((uc >= 0x19E0) && (uc <= 0x19FF)) alphabet = "khmer";
                else if ((uc >= 0x1D00) && (uc <= 0x1D7F)) alphabet = "phonetic";
                else if ((uc >= 0x1E00) && (uc <= 0x1EFF)) alphabet = "latin";
                else if ((uc >= 0x1F00) && (uc <= 0x1FFF)) alphabet = "greek and coptic";
                else if ((uc >= 0x2000) && (uc <= 0x206F)) alphabet = "none";
                else if ((uc >= 0x2070) && (uc <= 0x209F)) alphabet = "none";
                else if ((uc >= 0x20A0) && (uc <= 0x20CF)) alphabet = "none";
                else if ((uc >= 0x20D0) && (uc <= 0x20FF)) alphabet = "combining diacritical marks for symbols";
                else if ((uc >= 0x2100) && (uc <= 0x214F)) alphabet = "letterlike symbols";
                else if ((uc >= 0x2150) && (uc <= 0x218F)) alphabet = "none";
                else if ((uc >= 0x2190) && (uc <= 0x21FF)) alphabet = "none";
                else if ((uc >= 0x2200) && (uc <= 0x22FF)) alphabet = "none";
                else if ((uc >= 0x2300) && (uc <= 0x23FF)) alphabet = "none";
                else if ((uc >= 0x2400) && (uc <= 0x243F)) alphabet = "none";
                else if ((uc >= 0x2440) && (uc <= 0x245F)) alphabet = "optical character recognition";
                else if ((uc >= 0x2460) && (uc <= 0x24FF)) alphabet = "enclosed alphanumerics";
                else if ((uc >= 0x2500) && (uc <= 0x257F)) alphabet = "none";
                else if ((uc >= 0x2580) && (uc <= 0x259F)) alphabet = "none";
                else if ((uc >= 0x25A0) && (uc <= 0x25FF)) alphabet = "none";
                else if ((uc >= 0x2600) && (uc <= 0x26FF)) alphabet = "none";
                else if ((uc >= 0x2700) && (uc <= 0x27BF)) alphabet = "none";
                else if ((uc >= 0x27C0) && (uc <= 0x27EF)) alphabet = "none";
                else if ((uc >= 0x27F0) && (uc <= 0x27FF)) alphabet = "none";
                else if ((uc >= 0x2800) && (uc <= 0x28FF)) alphabet = "braille";
                else if ((uc >= 0x2900) && (uc <= 0x297F)) alphabet = "none";
                else if ((uc >= 0x2980) && (uc <= 0x29FF)) alphabet = "none";
                else if ((uc >= 0x2A00) && (uc <= 0x2AFF)) alphabet = "none";
                else if ((uc >= 0x2B00) && (uc <= 0x2BFF)) alphabet = "none";
                else if ((uc >= 0x2E80) && (uc <= 0x2EFF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x2F00) && (uc <= 0x2FDF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x2FF0) && (uc <= 0x2FFF)) alphabet = "none";
                else if ((uc >= 0x3000) && (uc <= 0x303F)) alphabet = "chinese/japanese";
                else if ((uc >= 0x3040) && (uc <= 0x309F)) alphabet = "chinese/japanese";
                else if ((uc >= 0x30A0) && (uc <= 0x30FF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x3100) && (uc <= 0x312F)) alphabet = "bopomofo";
                else if ((uc >= 0x3130) && (uc <= 0x318F)) alphabet = "korean";
                else if ((uc >= 0x3190) && (uc <= 0x319F)) alphabet = "chinese/japanese";
                else if ((uc >= 0x31A0) && (uc <= 0x31BF)) alphabet = "bopomofo";
                else if ((uc >= 0x31F0) && (uc <= 0x31FF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x3200) && (uc <= 0x32FF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x3300) && (uc <= 0x33FF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x3400) && (uc <= 0x4DBF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x4DC0) && (uc <= 0x4DFF)) alphabet = "none";
                else if ((uc >= 0x4E00) && (uc <= 0x9FFF)) alphabet = "chinese/japanese";
                else if ((uc >= 0xA000) && (uc <= 0xA48F)) alphabet = "chinese/japanese";
                else if ((uc >= 0xA490) && (uc <= 0xA4CF)) alphabet = "chinese/japanese";
                else if ((uc >= 0xAC00) && (uc <= 0xD7AF)) alphabet = "korean";
                else if ((uc >= 0xD800) && (uc <= 0xDB7F)) alphabet = "high surrogates";
                else if ((uc >= 0xDB80) && (uc <= 0xDBFF)) alphabet = "high private use surrogates";
                else if ((uc >= 0xDC00) && (uc <= 0xDFFF)) alphabet = "low surrogates";
                else if ((uc >= 0xE000) && (uc <= 0xF8FF)) alphabet = "private use area";
                else if ((uc >= 0xF900) && (uc <= 0xFAFF)) alphabet = "chinese/japanese";
                else if ((uc >= 0xFB00) && (uc <= 0xFB4F)) alphabet = "alphabetic presentation forms";
                else if ((uc >= 0xFB50) && (uc <= 0xFDFF)) alphabet = "arabic";
                else if ((uc >= 0xFE00) && (uc <= 0xFE0F)) alphabet = "variation selectors";
                else if ((uc >= 0xFE20) && (uc <= 0xFE2F)) alphabet = "combining half marks";
                else if ((uc >= 0xFE30) && (uc <= 0xFE4F)) alphabet = "chinese/japanese";
                else if ((uc >= 0xFE50) && (uc <= 0xFE6F)) alphabet = "small form variants";
                else if ((uc >= 0xFE70) && (uc <= 0xFEFF)) alphabet = "arabic";
                else if ((uc >= 0xFF00) && (uc <= 0xFFEF)) alphabet = "halfwidth and fullwidth forms";
                else if ((uc >= 0xFFF0) && (uc <= 0xFFFF)) alphabet = "specials";
                else if ((uc >= 0x10000) && (uc <= 0x1007F)) alphabet = "linear b";
                else if ((uc >= 0x10080) && (uc <= 0x100FF)) alphabet = "linear b";
                else if ((uc >= 0x10100) && (uc <= 0x1013F)) alphabet = "aegean numbers";
                else if ((uc >= 0x10300) && (uc <= 0x1032F)) alphabet = "old italic";
                else if ((uc >= 0x10330) && (uc <= 0x1034F)) alphabet = "gothic";
                else if ((uc >= 0x10380) && (uc <= 0x1039F)) alphabet = "ugaritic";
                else if ((uc >= 0x10400) && (uc <= 0x1044F)) alphabet = "deseret";
                else if ((uc >= 0x10450) && (uc <= 0x1047F)) alphabet = "shavian";
                else if ((uc >= 0x10480) && (uc <= 0x104AF)) alphabet = "osmanya";
                else if ((uc >= 0x10800) && (uc <= 0x1083F)) alphabet = "cypriot syllabary";
                else if ((uc >= 0x1D000) && (uc <= 0x1D0FF)) alphabet = "byzantine musical symbols";
                else if ((uc >= 0x1D100) && (uc <= 0x1D1FF)) alphabet = "musical symbols";
                else if ((uc >= 0x1D300) && (uc <= 0x1D35F)) alphabet = "tai xuan jing symbols";
                else if ((uc >= 0x1D400) && (uc <= 0x1D7FF)) alphabet = "none";
                else if ((uc >= 0x20000) && (uc <= 0x2A6DF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x2F800) && (uc <= 0x2FA1F)) alphabet = "chinese/japanese";
                else if ((uc >= 0xE0000) && (uc <= 0xE007F)) alphabet = "none";

                bool ucprint = false;
                if (alphabet != "none")
                {
                    n++;
                    if (!alphdir.ContainsKey(alphabet))
                        alphdir.Add(alphabet, 0);
                    alphdir[alphabet]++;
                }
                else if (uc != 0x0020)
                {
                    //Console.Write("c=" + c.ToString() + ", uc=0x" + uc.ToString("x5") + "|");
                    //ucprint = true;
                }
                if (ucprint)
                    Console.WriteLine();
            }

            int nmax = 0;
            string alphmax = "none";
            foreach (string alph in alphdir.Keys)
            {
                //Console.WriteLine("ga:" + alph + " " + alphdir[alph].ToString());
                if (alphdir[alph] > nmax)
                {
                    nmax = alphdir[alph];
                    alphmax = alph;
                }
            }

            if (letters.Length > 2 * n) //mostly non-alphabetic
                return "none";
            else if (nmax > n / 2) //mostly same alphabet
                return alphmax;
            else
                return "mixed"; //mixed alphabets
        }

        public static bool comparetaxa(Taxon t1,Taxon t2)
        {
            if (t1.ScientificName != t2.ScientificName)
                return false;
            if (t1.Genus != t2.Genus)
                return false;
            if (t1.Family != t2.Family)
                return false;
            if (t1.Order != t2.Order)
                return false;
            if (t1.Class != t2.Class)
                return false;
            if (t1.Phylum != t2.Phylum)
                return false;
            if (t1.Kingdom != t2.Kingdom)
                return false;
            return true;
        }

        public static List<string> getgallery(Page p)
        {
            List<string> ls = new List<string>();

            //memo("getgallery");

            if (!p.text.Contains("gallery"))
            {
                //memo("No gallery");
                return ls;
            }

            string tx = p.text.Replace("\n", "£");

            string rex = @"\<gallery\>(.*)\<\/gallery\>";
            foreach (Match m in Regex.Matches(tx, rex, RegexOptions.Singleline))
            {
                //memo("Match " + m.Groups[1].Value);
                string[] pix = m.Groups[1].Value.Split('£');
                foreach (string pic in pix)
                    ls.Add(pic);
            }

            return ls;
        }
        public static bool human_image_only(Page p, Site site)
        {
            //List<string> userlist = new List<string>();
            PageList plh = new PageList(site);
            string currentrev = "";
            string prevrev = "";
            if (String.IsNullOrEmpty(p.text))
                util.tryload(p, 2);
            List<string> piclist = p.GetImages();
            piclist.Add("image");
            piclist.Add("range_map");
            piclist.Add(".jpg");
            piclist.Add(".png");
            piclist.Add(".gif");
            piclist.Add("gallery");
            piclist.Add("[[Category:"); //skip categories too!
            piclist.Add("[[" + MakeSpecies.mp(1, null));
            try
            {
                plh.FillFromPageHistory(p.title, 10);
            }
            catch (Exception e)
            {
                return false;
            }
            bool humanprev = false;
            foreach (Page ph in plh)
            {
                if (!util.tryload(ph, 2))
                    continue;
                currentrev = ph.revision;
                if (humanprev)
                {
                    List<string> diff = util.diffversions(site, currentrev, prevrev);
                    foreach (string s in diff)
                    {
                        bool picfound = false;
                        if (String.IsNullOrEmpty(s))
                            continue;
                        foreach (string im in piclist)
                            if (s.Contains(im))
                            {
                                picfound = true;
                            }
                        if (!picfound)
                            return false;
                    }

                }
                humanprev = false;
                prevrev = currentrev;
                //memo("\n\n  " + ph.lastUser + "\n");
                //userlist.Add(ph.lastUser);
                if (util.humanlist.Contains(ph.lastUser))
                {
                    humanprev = true;
                }
                else if (ph.lastUser.ToLower().Contains("bot"))
                {
                    humanprev = false;
                }
                else if (util.get_alphabet(ph.lastUser.Substring(0, 2)) == "none")
                {
                    humanprev = false;
                }
                else if (util.neglectlist.Contains(ph.lastUser))
                {
                    humanprev = false;
                }
                else if (ph.comment.Contains("GlobalReplace"))
                {
                    humanprev = false;
                }
                else
                    humanprev = true;

            }

            return true;
        }



        public static List<string> goodvalues = new List<string>() { "diff-addedline", "diff-deletedline" };

        public static List<string> diffversions(Site site, string rev1, string rev2)
        {
            //string s = "";
            List<string> ls = new List<string>();

            string result = site.PostDataAndGetResult(site.address + "/w/api.php", "action=compare&format=json&fromrev=" + rev1 + "&torev=" + rev2);
            //memo(result);
            JObject jtop = JObject.Parse(result);

            if (!jtop.ContainsKey("compare"))
            {
                ls.Add("##ERROR## no compare");
                return ls;
            }
            else
            {
                JToken jj = jtop["compare"];



                //if (jj.ContainsKey("code"))
                //{
                //    return "##ERROR## " + jj["code"];
                //}
                if (jj["*"] == null)
                {
                    ls.Add("##ERROR## no *");
                    return ls;
                }

                string html = jj["*"].ToString();

                HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
                htmlDocument.LoadHtml(html);

                HtmlNodeCollection nodes = htmlDocument.DocumentNode.ChildNodes;

                //StringBuilder sb = new StringBuilder("");
                foreach (HtmlNode node in nodes)
                {
                    //memo(node.Name + ": " + node.OuterHtml);
                    //sb.Append(node.OuterHtml + "\n");
                    foreach (HtmlNode kid in node.ChildNodes)
                    {
                        foreach (HtmlAttribute ha in kid.Attributes)
                        {
                            //memo("ha:" + ha.Name + " : " + ha.Value);
                            if (ha.Name == "class")
                            {
                                //attdict.Add(ha.Value);
                                if (goodvalues.Contains(ha.Value))
                                    ls.Add(kid.InnerHtml);
                            }
                        }
                    }
                }


            }
            return ls;
        }

        public static string findfork(string basename,Site site)
        {
            //determines if fork page with basename and fork suffix exists
            //returns title if it does, otherwise empty string
            string forkname = basename + " (pagklaro)";
            Page p = new Page(site,forkname);
            util.tryload(p, 2);
            if (p.Exists())
                return p.title;
            else
                return "";
        }

        public static List<string> neglectlist = new List<string>() { "CommonsDelinker", "Lsj","BrunoBoehmler" };
        public static List<string> humanlist = new List<string>() { "Josefwintzent Libot", "99of9","187.190.198.136","187.190.203.72","189.203.101.72" };


        public static bool human_touched(Page p, Site site)
        {

            PageList plh = new PageList(site);
            try
            {
                plh.FillFromPageHistory(p.title, 10);
                foreach (Page ph in plh)
                {
                    if (!util.tryload(ph, 2))
                        return true;
                    //memo("  " + ph.lastUser);
                    if (humanlist.Contains(ph.lastUser))
                    {
                        return true;
                    }
                    if (ph.lastUser.ToLower().Contains("bot"))
                        continue;
                    if (util.get_alphabet(ph.lastUser.Substring(0, 2)) == "none")
                        continue;
                    if (neglectlist.Contains(ph.lastUser))
                        continue;
                    if (ph.comment.Contains("GlobalReplace"))
                        continue;
                    return true;
                }
            }
            catch (Exception e)
            {
                return true;
            }
            return false;

        }

    }


}
