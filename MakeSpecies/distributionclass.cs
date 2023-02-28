using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace MakeSpecies
{
    class distributionclass
    {
        public string maindist = "";
        public string compass = "";
        public List<distributionclass> subdist = new List<distributionclass>();
        public string ceblink = "";
        public string partof = "";

        //======================================

        public static string failstring = "##FAIL##";

        public static List<distributionclass> maindistlist = new List<distributionclass>();
        public static List<string> distlist = new List<string>();
        public static Dictionary<string, string> localitydict = new Dictionary<string, string>();
        public static Dictionary<string, string> countryaliasdict = new Dictionary<string, string>();
        public static Dictionary<string, string> statedict = new Dictionary<string, string>();

        public static distributionclass checkmain(string desc)
        {
            string dd = desc.Trim();
            distributionclass dcc = (from c in maindistlist where c.maindist == dd select c).FirstOrDefault();
            if (dcc != null)
                return dcc.copy();
            else
                return null;
        }

        public distributionclass copy()
        {
            distributionclass dc = new distributionclass();
            dc.maindist = String.Copy(this.maindist);
            dc.ceblink = String.Copy(this.ceblink);
            dc.partof = String.Copy(this.partof);
            dc.compass = String.Copy(this.compass);
            foreach (distributionclass sdc in this.subdist)
                dc.subdist.Add(sdc.copy());
            return dc;
        }

        public static distributionclass makefaileddist(string w)
        {
            distributionclass dc = new distributionclass();
            dc.maindist = failstring;
            dc.ceblink = w;
            return dc;
        }

        public static string removestartword(string ww,string startword, string secondword)
        {
            string s = ww;
            if (ww.StartsWith(startword+" "))
            {
                if (ww.StartsWith(startword+" "+secondword+" "))
                    s = ww.Substring(startword.Length+secondword.Length+2);
                else
                    s = ww.Substring(startword.Length+1);
            }
            return s;
        }

        public static distributionclass miniparse(string ss, List<string> distlist)
        {
            string sss = aliascheck(ss.Trim(), distlist);
            distributionclass dc3 = checkmain(sss);
            if (dc3 != null)
            {
                return dc3;
                //ls.Add(dc3);
                //nfound++;
            }
            else
            {
                dc3 = compassaliascheck2(sss, distlist);
                if (dc3 != null)
                {
                    return dc3;
                    //ls.Add(dc3);
                    //nfound++;
                }
                else if (sss.ToUpper() == sss)
                {
                    sss = countryclass.getcountry(sss);
                    dc3 = checkmain(sss);
                    if (dc3 != null)
                    {
                        return dc3;
                        //ls.Add(dc3);
                        //nfound++;
                    }
                    else
                    {
                        return makefaileddist(sss);
                        //ls.Add(makefaileddist(sss));
                        if (!descstat.ContainsKey(sss))
                            descstat.Add(sss, 1);
                        else
                            descstat[sss]++;
                    }

                }
                else
                {
                    return makefaileddist(sss);
                    //ls.Add(makefaileddist(sss));
                    if (!descstat.ContainsKey(sss))
                        descstat.Add(sss, 1);
                    else
                        descstat[sss]++;
                }
            }

        }
        public static List<distributionclass> parsedescription(Description dd)
        {
            return parsedescription(dd.Description1, "");
        }

        public static List<distributionclass> parsedescription(Description dd, string above)
        {
            return parsedescription(dd.Description1, above);
        }

        public static Dictionary<string, int> descstat = new Dictionary<string, int>();
        public static List<distributionclass> parsedescription(string descpar, string above)
        {
            Console.WriteLine("pd:" + descpar + " ! " + above);
            List<distributionclass> ls = new List<distributionclass>();

            if (statedict.Count == 0)
                statedict = getstatedict();
            Dictionary<string, int> provincestat = new Dictionary<string, int>();
            Dictionary<string, int> occstat = new Dictionary<string, int>();
            Dictionary<string, int> establstat = new Dictionary<string, int>();
            //List<string> compasslist = util.getcompasslist();
            //Dictionary<string, string> aliasdict = util.getcountryalias();

            int n = 0;

            Regex rb = new Regex(@"\(\#\#(\d\d\d)\)");
            Regex ryear = new Regex(@"\d{4}");
            Regex rnot = new Regex(@"\(not .+?\)");


            //string desc = dd.Description1;
            string desc = descpar;

            desc = desc.Replace(" (I)", "").Replace("(I)", ""); //remove (I) that bothers the parser
            MatchCollection mnot = rnot.Matches(desc); //remove "(not XXX)"
            foreach (Match m in mnot)
                desc = desc.Replace(m.Value, "");
            desc = desc.Replace(", (", " (");
            desc = desc.Replace("; (", " (");

            string[] brackets = util.findtextinbrackets(desc, "(", ")");
            if (brackets.Length > 0)
            {
                for (int i = 0; i < brackets.Length; i++)
                {
                    desc = desc.Replace("(" + brackets[i] + ")", "(##" + i.ToString("D3") + ")");
                }
            }
            int nfound = 0;
            distributionclass dc1 = checkmain(desc);
            if (dc1 != null)
            {
                ls.Add(dc1);
                nfound++;
                return ls;
            }
            else
            {
                string[] dw;
                if (desc.Contains(";"))
                {
                    dw = desc.Split(';');
                    int maxlength = -1;
                    foreach (string s in dw)
                        if (s.Length > maxlength)
                            maxlength = s.Length;
                    if (maxlength > 50)
                        dw = desc.Split(new char[] { ';', ',' });
                }
                else if (desc.Contains(","))
                    dw = desc.Split(',');
                else if ((desc.ToUpper() == desc) && (brackets.Length == 0))
                    dw = desc.Split(' ');
                else
                    dw = new string[] { desc };
                foreach (string word in dw)
                {
                    string w = word.Trim();
                    //if (w.Contains(":"))
                    //{
                    //    string[] wcol = w.Split(':');
                    //    if (diststat.ContainsKey(wcol[1].Trim(new char[] { ' ', '.' })))
                    //    {
                    //        w = wcol[1].Trim(new char[] { ' ', '.' });
                    //    }
                    //    else if (diststat.ContainsKey(wcol[0].Trim(new char[] { ' ', '.' })))
                    //    {
                    //        w = wcol[0].Trim(new char[] { ' ', '.' });
                    //    }

                    //}
                    distributionclass dc2 = new distributionclass();
                    dc2 = checkmain(w);
                    if (dc2 != null)
                    {
                        ls.Add(dc2);
                        nfound++;
                    }
                    else if (localitydict.ContainsKey(w))
                    {
                        dc2 = checkmain(localitydict[w]);
                        ls.Add(dc2);
                        nfound++;
                    }
                    else
                    {
                        //find parentheses
                        List<int> bracketlist = new List<int>();
                        MatchCollection mb = rb.Matches(w);
                        foreach (Match m in mb)
                        {
                            bracketlist.Add(Convert.ToInt32(m.Groups[1].Value));
                            w = w.Replace(m.Value, "").Trim();
                        }
                        dc2 = checkmain(w);
                        if (dc2 != null)
                        {
                            ls.Add(dc2);
                            nfound++;
                        }
                        else
                        {
                            dc2 = compassaliascheck2(w, distlist);

                            if (dc2 != null)
                            {
                                ls.Add(dc2);
                                nfound++;
                            }
                            else
                            {
                                string ww = aliascheck(w.Trim(new char[] { ' ', '.','=','+' }),distlist);
                                ww = removestartword(ww,"and","the");
                                ww = removestartword(ww, "in", "the");
                                ww = removestartword(ww, "from", "the");
                                ww = removestartword(ww, "to", "the");
                                ww = removestartword(ww, "including", "the");
                                ww = removestartword(ww, "the", "the");
                                dc2 = compassaliascheck2(ww, distlist);
                                if (dc2 != null)
                                {
                                    ls.Add(dc2);
                                    nfound++;
                                }
                                else
                                {
                                    if ((ww.Contains(",")) || (ww.Contains("&")))
                                    {
                                        foreach (string ss in ww.Split(new char[]{ ',','&'}))
                                        {
                                            distributionclass dc3 = miniparse(ss, distlist);
                                            ls.Add(dc3);
                                            //string sss = aliascheck(ss.Trim(), distlist);
                                            //distributionclass dc3 = checkmain(sss);
                                            //if (dc3 != null)
                                            //{
                                            //    ls.Add(dc3);
                                            //    nfound++;
                                            //}
                                            //else
                                            //{
                                            //    dc3 = compassaliascheck2(sss, distlist);
                                            //    if (dc3 != null)
                                            //    {
                                            //        ls.Add(dc3);
                                            //        nfound++;
                                            //    }
                                            //    else if (sss.ToUpper() == sss)
                                            //    {
                                            //        sss = countryclass.getcountry(sss);
                                            //        dc3 = checkmain(sss);
                                            //        if (dc3 != null)
                                            //        {
                                            //            ls.Add(dc3);
                                            //            nfound++;
                                            //        }
                                            //        else
                                            //        {
                                            //            ls.Add(makefaileddist(sss));
                                            //            if (!descstat.ContainsKey(sss))
                                            //                descstat.Add(sss, 1);
                                            //            else
                                            //                descstat[sss]++;
                                            //        }

                                            //    }
                                            //    else
                                            //    {
                                            //        ls.Add(makefaileddist(sss));
                                            //        if (!descstat.ContainsKey(sss))
                                            //            descstat.Add(sss, 1);
                                            //        else
                                            //            descstat[sss]++;
                                            //    }
                                            //}
                                            dc2 = dc3;
                                        }
                                    }
                                    else if (ww.Contains(" and "))
                                    {
                                        foreach (string ss in ww.Split(new string[] { " and " },StringSplitOptions.RemoveEmptyEntries))
                                        {
                                            distributionclass dc3 = miniparse(ss, distlist);
                                            ls.Add(dc3);
                                            dc2 = dc3;
                                        }
                                    }
                                    else if (ww.Contains(" to "))
                                    {
                                        string[] toto = ww.Split(new string[] { " to " }, StringSplitOptions.RemoveEmptyEntries);
                                        if ( toto.Length == 2)
                                        {
                                            distributionclass dcto0 = checkmain(toto[0]);
                                            distributionclass dcto1 = checkmain(toto[1]);
                                            if (dcto0 != null && dcto1 != null)
                                            {
                                                dc2 = new distributionclass();
                                                dc2.maindist = ww;
                                                string[] p92 = new string[2] { dcto0.ceblink, dcto1.ceblink };
                                                dc2.ceblink = MakeSpecies.mp(92, p92);
                                            }
                                            else
                                            {
                                                ls.Add(makefaileddist(ww));
                                                if (!descstat.ContainsKey(ww))
                                                    descstat.Add(ww, 1);
                                                else
                                                    descstat[ww]++;
                                            }
                                        }
                                        else
                                        {
                                            ls.Add(makefaileddist(ww));
                                            if (!descstat.ContainsKey(ww))
                                                descstat.Add(ww, 1);
                                            else
                                                descstat[ww]++;
                                        }
                                    }
                                    else if (!String.IsNullOrEmpty(above))
                                    {
                                        if (above.Contains("USA") || above.Contains("Canada") || above.Contains("Australia") || above.Contains("United States") || above.Contains("Europe"))
                                        {
                                            if (above.Contains("Australia"))
                                                ww = getstate(ww + "-au");
                                            else if (above.Contains("Europe"))
                                                ww = countryclass.getcountry(ww);
                                            else
                                                ww = getstate(ww);
                                            dc2 = checkmain(ww);
                                            if (dc2 != null)
                                            {
                                                ls.Add(dc2);
                                                nfound++;
                                            }
                                        }
                                    }
                                    else if (ww.ToUpper() == ww)
                                    {
                                        if (ww.EndsWith("OO"))
                                        {
                                            ww = ww.Replace("OO", "").Trim('-');
                                        }
                                        ww = countryclass.getcountry(ww);
                                        dc2 = checkmain(ww);
                                        if (dc2 != null)
                                        {
                                            ls.Add(dc2);
                                            nfound++;
                                        }
                                        else
                                        {
                                            ls.Add(makefaileddist(ww));
                                            if (!descstat.ContainsKey(ww))
                                                descstat.Add(ww, 1);
                                            else
                                                descstat[ww]++;
                                        }

                                    }
                                    else
                                    {
                                        ls.Add(makefaileddist(ww));
                                        if (!descstat.ContainsKey(ww))
                                            descstat.Add(ww, 1);
                                        else
                                            descstat[ww]++;
                                    }
                                }


                            }
                        }
                        //At this point, dc2 contains any successful match for maindist. 
                        //If null, but brackets exist: create dummy. 
                        if (( dc2 == null) && (bracketlist.Count > 0))
                        {
                            dc2 = makefaileddist(w);
                            ls.Add(dc2);
                        }
                        if (dc2 != null)
                            dc2.subdist.Clear();

                        //Add bracket to dc2.subdist.
                        foreach (int ib in bracketlist)
                        {
                            MatchCollection matches = ryear.Matches(brackets[ib]);
                            if (matches.Count == 0) //skip brackets with year
                            {
                                dc2.subdist = parsedescription(brackets[ib], dc2.maindist);
                                //string[] provinces = brackets[ib].Split(new char[] { ';', ',' });
                                //if ((provinces.Length == 1) && (brackets[ib].ToUpper() == brackets[ib]))
                                //{
                                //    provinces = brackets[ib].Split();
                                //}
                                //foreach (string pword in provinces)
                                //{
                                //    string pp = pword.Trim();
                                //    //if (pp.Contains(":"))
                                //    //{
                                //    //    string[] wcol = pp.Split(':');
                                //    //    if (diststat.ContainsKey(wcol[1].Trim(new char[] { ' ', '.' })))
                                //    //    {
                                //    //        pp = wcol[1].Trim(new char[] { ' ', '.' });
                                //    //    }
                                //    //    else if (diststat.ContainsKey(wcol[0].Trim(new char[] { ' ', '.' })))
                                //    //    {
                                //    //        pp = wcol[0].Trim(new char[] { ' ', '.' });
                                //    //    }

                                //    //}
                                //    if (diststat.ContainsKey(pp))
                                //    {
                                //        diststat[pp]++;
                                //        nfound++;
                                //    }
                                //    else if (util.localitydict.ContainsKey(w))
                                //    {
                                //        diststat[util.localitydict[w]]++;
                                //        nfound++;
                                //    }
                                //    else
                                //    {
                                //        pp = util.compassaliascheck(pp, diststat);

                                //        if (diststat.ContainsKey(pp))
                                //        {
                                //            diststat[pp]++;
                                //            nfound++;
                                //        }
                                //        else
                                //        {
                                //            string ww = pp.Trim(new char[] { ' ', '.' });
                                //            if (diststat.ContainsKey(ww))
                                //            {
                                //                diststat[ww]++;
                                //                nfound++;
                                //            }
                                //            else if (w.Contains("USA") || w.Contains("Canada") || w.Contains("Australia") || w.Contains("United States") || w.Contains("Europe"))
                                //            {
                                //                if (w.Contains("Australia"))
                                //                    ww = util.getstate(ww + "-au");
                                //                else if (w.Contains("Europe"))
                                //                    ww = countryclass.getcountry(ww);
                                //                else
                                //                    ww = util.getstate(ww);
                                //                if (diststat.ContainsKey(ww))
                                //                {
                                //                    diststat[ww]++;
                                //                    nfound++;
                                //                }
                                //                else
                                //                {
                                //                    string ppp = pp + "!!" + w;
                                //                    if (!provincestat.ContainsKey(ppp))
                                //                        provincestat.Add(ppp, 1);
                                //                    else
                                //                        provincestat[ppp]++;
                                //                    if (w.Length > 200)
                                //                        memo("!!" + dd.Description1);
                                //                }
                                //            }
                                //            else
                                //            {
                                //                string ppp = pp + "!" + w;
                                //                if (!provincestat.ContainsKey(ppp))
                                //                    provincestat.Add(ppp, 1);
                                //                else
                                //                    provincestat[ppp]++;
                                //                if (nprint < 200 && String.IsNullOrEmpty(w))
                                //                {
                                //                    memo(dd.Description1 + "!" + pp);
                                //                    nprint++;
                                //                }
                                //            }

                                //        }
                                //    }
                                //}
                            }

                        }
                    }
                }
            }
            //if (nfound > 0)
            //{
            //    if (!taxonstat.ContainsKey(dd.TaxonID))
            //        taxonstat.Add(dd.TaxonID, nfound);
            //    else
            //        taxonstat[dd.TaxonID] += nfound;
            //}
            
            //memo("Diststat: " + diststat.Count());
            //foreach (string s in diststat.Keys)
            //    memo(s + "\t" + diststat[s]);

            //memo("\n\n================================================================\nDescription");
            //memo("Descstat: " + descstat.Count());
            //n = 0;
            //foreach (string s in descstat.Keys)
            //{
            //    n++;
            //    if (descstat[s] > 100)
            //        memo(s + "\t" + descstat[s]);
            //}
            //memo("\n\n================================================================\nProvince");
            //memo("Provincestat: " + provincestat.Count());
            //n = 0;
            //foreach (string s in provincestat.Keys)
            //{
            //    n++;
            //    if (provincestat[s] > 100)
            //        memo(s + "\t" + provincestat[s]);
            //}
            //memo("Provincestat: " + provincestat.Count());
            //memo("\n====\ntaxonstat" + taxonstat.Count());

            //descstat.Clear();
            //foreach (Description dd in db.Description)
            //{
            //    if (!taxonstat.ContainsKey(dd.TaxonID))
            //    {
            //        if (!descstat.ContainsKey(dd.Description1))
            //            descstat.Add(dd.Description1, 1);
            //        else
            //            descstat[dd.Description1]++;
            //    }
            //}
            //memo("\n\n================================================================\nFailures");
            //memo("Descstat: " + descstat.Count());
            //n = 0;
            //foreach (string s in descstat.Keys)
            //{
            //    n++;
            //    if (descstat[s] > 20)
            //        memo(s + "\t" + descstat[s]);
            //}
            //memo("\n====\ntaxonstat" + taxonstat.Count());

            //memo("\n\n================================================================");

            //foreach (string state in util.statestat.Keys)
            //    memo(state + "\t" + util.statestat[state]);

            //memo("\n====\ntaxonstat" + taxonstat.Count());

            //memo("\n\n================================================================");
            //foreach (string state in util.countrystat.Keys)
            //    memo(state + "\t" + util.countrystat[state]);

            //memo("\n====\ntaxonstat" + taxonstat.Count());

            //memo("\n\n================================================================");


            return ls;
        }

        public static string linkrex = @"\[\[(.+?)\]\]";

        public string getcatstring()
        {
            foreach (Match m in Regex.Matches(this.ceblink, linkrex))
                return m.Groups[1].Value.Split('|')[0];
            return "";
        }

        public List<string> getcats()
        {
            List<string> ls = new List<string>();

            ls.Add(this.getcatstring());
            if (this.subdist.Count > 0)
            {
                foreach (distributionclass dcsub in this.subdist)
                    ls = ls.Union(dcsub.getcats()).ToList();
            }

            return ls;
        }

        public static List<distributionclass> getdistribution(int taxonid, COL2019 db)
        {
            if (maindistlist.Count == 0)
            {
                read_distributionlinks(db);
            }
            //if (localitydict.Count == 0)
            //{
            //    foreach (Distribution dd in db.Distribution)
            //    {
            //        if (!localitydict.ContainsKey(dd.LocationID))
            //            localitydict.Add(dd.LocationID, dd.Locality);
            //    }
            //}

            List<distributionclass> ls = new List<distributionclass>();
            foreach (Distribution dd in (from c in db.Distribution where c.TaxonID == taxonid select c))
            {
                var q = from c in maindistlist where c.maindist == dd.Locality select c;
                if (q.Count() == 1)
                    ls.Add(q.First().copy());
                else
                {
                    distributionclass dc = new distributionclass();
                    dc.ceblink = failstring;
                    dc.maindist = dd.Locality;
                    foreach (distributionclass dcq in q)
                        dc.subdist.Add(dcq.copy());
                    ls.Add(dc);
                }
            }

            //if (ls.Count > 0)
            //    return ls;

            foreach (Description dd in (from c in db.Description where c.TaxonID == taxonid select c))
            {
                foreach (distributionclass dc in parsedescription(dd))
                    ls.Add(dc);
            }

            ls = consolidatelist(ls);

            return ls;
        }

        public static List<distributionclass> consolidatelist(List<distributionclass> ls)
        {
            List<distributionclass> lsnew = new List<distributionclass>();

            List<string> ceblist = (from c in ls select c.ceblink).Distinct().ToList();
            if (ceblist.Count == ls.Count)
                return ls;

            foreach (string cs in ceblist)
            {
                var q = from c in ls where c.ceblink == cs select c;
                if (q.Count() == 1)
                    lsnew.Add(q.First().copy());
                else
                {
                    distributionclass dcnew = q.First().copy();
                    bool firstround = true;
                    foreach (distributionclass dc in q)
                    {
                        if ( firstround)
                        {
                            firstround = false;
                            continue;
                        }
                        else
                        {
                            dcnew.merge(dc);
                        }
                    }
                    if (dcnew.subdist.Count > 1)
                        dcnew.subdist = consolidatelist(dcnew.subdist);
                    lsnew.Add(dcnew);
                }
            }
            
            return lsnew;
        }

        public void merge(distributionclass dc2)
        {
            if (dc2.subdist.Count > 0)
            {
                foreach (distributionclass dcsub in dc2.subdist)
                    this.subdist.Add(dcsub.copy());
                //this.subdist = consolidatelist(this.subdist);
            }
            if (!String.IsNullOrEmpty(dc2.compass))
            {
                if (!String.IsNullOrEmpty(this.compass))
                {
                    if ( this.compass != dc2.compass)
                    this.compass += " & " + dc2.compass;
                }
                else
                {
                    this.compass = string.Copy(dc2.compass);
                }
            }

        }

        public static void read_distributionlinks(COL2019 db)
        {
            countryclass.read_country_info();
            string fn = @"I:\dotnwb3\distlinks2.txt";
            using (StreamReader sr = new StreamReader(fn))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] words = line.Split('\t');
                    if (words.Length < 4)
                        continue;
                    distributionclass dc = new distributionclass();
                    dc.maindist = words[0];
                    dc.ceblink = words[3];
                    if (words.Length >= 5)
                    {
                        dc.partof = words[4];
                        if ( dc.partof.Length == 2)
                        {
                            dc.partof = countryclass.getcountryml(dc.partof);
                            if ((dc.partof.Length > 2) && (!dc.partof.Contains("[[")))
                                dc.partof = "[[" + dc.partof + "]]";
                        }
                        if (dc.partof == dc.ceblink)
                            dc.partof = "";
                    }
                    maindistlist.Add(dc);
                }
            }

            distlist = (from c in maindistlist select c.maindist).ToList();

            if (localitydict.Count == 0)
            {
                foreach (Distribution dd in db.Distribution)
                {
                    string loc = dd.LocationID.Replace("TDWG:", "");
                    if (!localitydict.ContainsKey(loc))
                        localitydict.Add(loc, dd.Locality);
                    if (loc.Contains("-"))
                    {
                        loc = loc.Replace("-", "");
                        if (!localitydict.ContainsKey(loc))
                            localitydict.Add(loc, dd.Locality);
                    }
                }
            }


        }

        public static string getpartof(string part)
        {
            if (String.IsNullOrEmpty(part))
                return "";
            var q = from c in maindistlist where c.getcatstring() == part select c;
            if (q.Count() == 0)
                return "";
            else
            {
                foreach (distributionclass dq in q)
                {
                    string po = dq.partof.Trim(new char[] { '[', ']' ,' '});
                    if (String.IsNullOrEmpty(po))
                        continue;
                    if (po == part)
                        continue;
                    return po;
                }
                return "";
            }
        }

        public string towikistring(string prefix)
        {
            StringBuilder sb = new StringBuilder(prefix+compass+" "+ceblink+"\n");

            foreach (distributionclass dcsub in this.subdist)
                sb.Append(dcsub.towikistring(prefix + "*"));

            return sb.ToString();
        }

        public static Dictionary<string, int> compassdict = new Dictionary<string, int>() {
            {"North",71},
            {"Northwest",72},
            {"Northeast",73},
            {"Northeastern",74},
            {"North-Central",75},
            {"East",76},
            {"West",77},
            {"South",78},
            {"Southwest",79},
            {"Southwestern",79},
            {"South West",80},
            {"Southeast",81},
            {"Southeastern",82},
            {"South-Central",83},
            {"Central",84},
            {"West-Central",85},
            {"East-Central",86},
            {"Western Central",85},
            {"Eastern Central",86},
            {"Eastern",87},
            {"Western",88},
            {"Southern",89},
            {"Northern",90},
            {"north",71},
            {"northwest",72},
            {"northeast",73},
            {"northeastern",74},
            {"north-central",75},
            {"east",76},
            {"west",77},
            {"south",78},
            {"southwest",79},
            {"southwestern",79},
            {"south west",80},
            {"southeast",81},
            {"southeastern",82},
            {"south-central",83},
            {"central",84},
            {"west-central",85},
            {"east-central",86},
            {"eastern",87},
            {"western",88},
            {"southern",89},
            {"northern",90}};


        //{ 
        //    ls.Add("North");
        //    ls.Add("Northwest");
        //    ls.Add("Northeast");
        //    ls.Add("Northeastern");
        //    ls.Add("North-Central");
        //    ls.Add("East");
        //    ls.Add("West");
        //    ls.Add("South");
        //    ls.Add("Southwest");
        //    ls.Add("South West");
        //    ls.Add("Southeast");
        //    ls.Add("Southeastern");
        //    ls.Add("South-Central");
        //    ls.Add("Central");
        //    ls.Add("West-Central");
        //    ls.Add("East-Central");
        //    ls.Add("Eastern");
        //    ls.Add("Western");
        //    ls.Add("Southern");
        //    ls.Add("Northern");
        //    return ls;

        //}

        public static Dictionary<string, string> hyphendict = new Dictionary<string, string>()
            {
                {"N-","North" },
                {"NE-","Northeast" },
                {"NW-","Northwest" },
                {"S-","South" },
                {"SE-","Southeast" },
                {"SW-","Southwest" },
                {"NC-","North-Central" },
                {"SC-","South-Central" },
                {"W-","West" },
                {"E-", "East" },
                {"WC-","West-Central" },
                {"C-","Central" },
                {"EC-", "East-Central" }
            };


        public static string[] extractcompass(string input)
        {
            string[] output = new string[2] { "", "" };
            string[] words = input.Split();
            foreach (string s in words)
            {
                if (compassdict.Keys.Contains(s))
                {
                    output[0] = MakeSpecies.mp(compassdict[s], null);
                    output[1] = input.Replace(s, "").Trim();
                    return output;
                }
            }
            foreach (string hh in hyphendict.Keys)
            {
                if (input.StartsWith(hh))
                {
                    output[0] = MakeSpecies.mp(compassdict[hyphendict[hh]], null);
                    output[1] = input.Replace(hh, "").Trim();
                    return output;
                }
            }
            output[1] = input;
            return output;
        }

        public static string compasscheck(string w)
        {
            string[] ec = extractcompass(w);
            if (!String.IsNullOrEmpty(ec[0]))
            {
                return ec[1];
            }
            return w;
        }

        public static List<string> hyphencheck = new List<string>() { "United States of America", "Canada", "Australia", "Brazil" };

        public static string aliascheck(string w, List<string> distlist)
        {
            if (countryaliasdict.Count == 0)
                countryaliasdict = getcountryalias();
            if (countryaliasdict.ContainsKey(w))
                return countryaliasdict[w];
            string wic = util.initialcap(w);
            if (distlist.Contains(wic))
                return wic;
            if (countryaliasdict.ContainsKey(wic))
                return countryaliasdict[wic];
            if (w.Contains(":"))
            {
                string[] wcol = w.Split(':');
                if (distlist.Contains(wcol[1].Trim(new char[] { ' ', '.' })))
                {
                    return wcol[1].Trim(new char[] { ' ', '.' });
                }
                else if (distlist.Contains(wcol[0].Trim(new char[] { ' ', '.' })))
                {
                    return wcol[0].Trim(new char[] { ' ', '.' });
                }
                else if (countryaliasdict.ContainsKey(wcol[1].Trim(new char[] { ' ', '.' })))
                {
                    return countryaliasdict[wcol[1].Trim(new char[] { ' ', '.' })];
                }
                else if (countryaliasdict.ContainsKey(wcol[0].Trim(new char[] { ' ', '.' })))
                {
                    return countryaliasdict[wcol[0].Trim(new char[] { ' ', '.' })];
                }
            }
            foreach (string hc in hyphencheck)
            {
                if (w.Contains(hc + "-"))
                    return w.Replace(hc + "-", "");
            }

            return w;
        }

        public static string compassaliascheck(string w, Dictionary<string, int> diststat)
        {
            if (diststat.ContainsKey(w))
                return w;
            w = aliascheck(w, diststat.Keys.ToList());
            if (diststat.ContainsKey(w))
                return w;
            else
                return compasscheck(w);
        }

        public static distributionclass compassaliascheck2(string wpar, List<string> distlist)
        {
            string w = wpar.Trim();
            if (w.Contains("?"))
            {
                distributionclass dc = compassaliascheck2(w.Replace("?", "").Trim(), distlist);
                if ( dc != null)
                {
                    string maybe = MakeSpecies.mp(91, null);
                    if (String.IsNullOrEmpty(dc.compass))
                        dc.compass = maybe;
                    else
                        dc.compass = maybe + " " + dc.compass;
                }
                return dc;
            }
            if (distlist.Contains(w))
                return checkmain(w);
            w = aliascheck(w, distlist);
            if (distlist.Contains(w))
                return checkmain(w);
            else
            {
                string[] ec = extractcompass(w);
                if (String.IsNullOrEmpty(ec[0]))
                {
                    return null;
                }
                else
                {
                    distributionclass dc = checkmain(ec[1]);
                    if ( dc == null)
                    {
                        string newec = aliascheck(ec[1], distlist);
                        dc = checkmain(newec);
                    }
                    if (dc != null)
                        dc.compass = ec[0];
                    return dc;
                }

            }
        }

        public static Dictionary<string, string> getcountryalias()
        {
            Dictionary<string, string> dd = new Dictionary<string, string>();
            dd.Add("Zare", "Democratic Republic of the Congo");
            dd.Add("Zaire", "Democratic Republic of the Congo");
            dd.Add("ZAI", "Democratic Republic of the Congo");
            dd.Add("ZAIOO", "Democratic Republic of the Congo");
            dd.Add("ZAI-OO", "Democratic Republic of the Congo");
            dd.Add("CON", "Democratic Republic of the Congo");
            dd.Add("CONOO", "Democratic Republic of the Congo");
            dd.Add("CON-OO", "Democratic Republic of the Congo");
            dd.Add("E-D.R.Congo", "Democratic Republic of the Congo");
            dd.Add("SE-D.R.Congo", "Democratic Republic of the Congo");
            dd.Add("W-D.R.Congo", "Democratic Republic of the Congo");
            dd.Add("E-D.R. Congo", "Democratic Republic of the Congo");
            dd.Add("SE-D.R. Congo", "Democratic Republic of the Congo");
            dd.Add("Democratic Republic of Congo", "Democratic Republic of the Congo");
            dd.Add("Congo, Democratic Republic of the", "Democratic Republic of the Congo");
            dd.Add("D.R. Congo", "Democratic Republic of the Congo");
            dd.Add("Belgian Congo", "Democratic Republic of the Congo");
            dd.Add("D.R.Congo", "Democratic Republic of the Congo");
            dd.Add("DR Congo", "Democratic Republic of the Congo");
            dd.Add("Rpublique Populaire du Congo", "Congo-Brazzaville");
            dd.Add("Republic Popular Congo", "Congo-Brazzaville");
            //dd.Add("D.R.Congo", "Democratic Republic of the Congo");
            dd.Add("Republic Democratic Congo", "Democratic Republic of the Congo");
            dd.Add("Rpublique Dmocratique du Congo", "Democratic Republic of the Congo");
            dd.Add("Runion", "Réunion");
            dd.Add("La Runion", "Réunion");
            dd.Add("Papua New Guinea .", "Papua New Guinea");
            dd.Add("Malaya", "Malaysia");
            dd.Add("Panam", "Panama");
            dd.Add("Masachusettes", "Massachusetts");
            dd.Add("Northwest Territories", "Northwest Territorie");
            dd.Add("USA", "United States of America");
            dd.Add("US", "United States of America");
            dd.Add("USA .", "United States of America");
            dd.Add("U.S.A.", "United States of America");
            dd.Add("U.S.", "United States of America");
            dd.Add("United States", "United States of America");
            dd.Add("Etats-Unis", "United States of America");
            dd.Add("US Species? YES", "United States of America");
            dd.Add("Qubec", "Quebec");
            dd.Add("Cte d'Ivoire", "Ivory Coast");
            dd.Add("Cameroun", "Cameroon");
            dd.Add("Russian Far East", "Siberia");
            dd.Add("Russian: FE", "Siberia");
            dd.Add("Russia: FE", "Siberia");
            dd.Add("Russia in Asia", "Siberia");
            dd.Add("Russia (Far East)", "Siberia");
            dd.Add("Russia. Far East", "Siberia");
            dd.Add("East Palearctic region", "Siberia");
            dd.Add("Congo (Brazzaville)", "Congo-Brazzaville");
            dd.Add("European Russi", "European Russia");
            dd.Add("European Rus", "European Russia");
            dd.Add("European R", "European Russia");
            dd.Add("Europe: Russia", "European Russia");
            dd.Add("Russia in Europe", "European Russia");
            dd.Add("Russie d'Europe", "European Russia");
            dd.Add("Australie", "Australia");
            dd.Add("Tanzanie", "Tanzania");
            dd.Add("Brsil", "Brazil");
            dd.Add("Bolivie", "Bolivia");
            dd.Add("Ile de Borneo", "Borneo");
            dd.Add("Inde", "India");
            dd.Add("Malaisie", "Malaysia");
            dd.Add("Indonsie", "Indonesia");
            dd.Add("Papouasie-Nouvelle-Guine", "Papua New Guinea");
            dd.Add("W-Cape Prov.", "Western Cape Province");
            dd.Add("E-Cape Prov.", "Eastern Cape Province");
            dd.Add("N-Cape Prov.", "Northern Cape Province");
            dd.Add("NSW", "New South Wales");
            dd.Add("Myanmar [Burma]", "Myanmar");
            dd.Add("Burma", "Myanmar");
            dd.Add("Atlantic", "Atlantic Ocean");
            dd.Add("Pacific", "Pacific Ocean");
            dd.Add("Galapagos", "Galapagos Is.");
            dd.Add("Eastern Galapagos Islands", "Galapagos Is.");
            dd.Add("Galapagos Islands", "Galapagos Is.");
            dd.Add("BOR-SR", "Borneo");
            dd.Add("BOR", "Borneo");
            dd.Add("BOR-SB", "Borneo");
            dd.Add("BOR-KA", "Borneo");
            dd.Add("BOR-BR", "Borneo");
            dd.Add("PH", "Philippines");
            dd.Add("PHI", "Philippines");
            dd.Add("PHIOO", "Philippines");
            dd.Add("PHI-OO", "Philippines");
            dd.Add("SUM", "Sumatra");
            dd.Add("THA", "Thailand");
            dd.Add("THAOO", "Thailand");
            dd.Add("THA-OO", "Thailand");
            dd.Add("SUL", "Sulawesi");
            dd.Add("MOL", "Moluccas");
            dd.Add("NWG-PN", "Papua New Guinea");
            dd.Add("NWG-IJ", "Irian Jaya");
            dd.Add("NWGPN", "Papua New Guinea");
            dd.Add("NWGIJ", "Irian Jaya");
            dd.Add("LAO", "Laos");
            dd.Add("VIE", "Vietnam");
            dd.Add("VIEOO", "Vietnam");
            dd.Add("VIE-OO", "Vietnam");
            dd.Add("VNM", "Vietnam");
            dd.Add("Cape Prov.", "Cape Provinces");
            dd.Add("Middle America", "Central America");
            dd.Add("Americas  North America", "North America");
            dd.Add("Americas  North America (incl Mexico)", "North America");
            dd.Add("TUR", "Turkey");
            dd.Add("UZB", "Uzbekistan");
            dd.Add("PER", "Peru");
            dd.Add("Chine", "China");
            dd.Add("NWG", "New Guinea");
            dd.Add("Japon", "Japan");
            dd.Add("JP", "Japan");
            dd.Add("JAP", "Japan");
            dd.Add("MDG", "Madagascar");
            dd.Add("MDGOO", "Madagascar");
            dd.Add("MDG-OO", "Madagascar");
            dd.Add("Nouvelle Caldonie", "New Caledonia");
            dd.Add("Nouvelle Zlande", "New Zealand");
            dd.Add("ECU", "Ecuador");
            dd.Add("ECUOO", "Ecuador");
            dd.Add("ECU-OO", "Ecuador");
            dd.Add("Madagascan", "Madagascar");
            dd.Add("Haiti, Dominican Republic", "Hispaniola");
            dd.Add("Australia-Queensland", "Queensland");
            dd.Add("Mexique", "Mexico");
            dd.Add("RU", "Russia");
            dd.Add("Turkey in Asia", "Turkey");
            dd.Add("Turquie", "Turkey");
            dd.Add("Europe & Asia", "Eurasia");
            dd.Add("Palaearctic", "Eurasia  Asia-Temperate");
            dd.Add("Palearctic", "Eurasia  Asia-Temperate");
            dd.Add("PalaeArctic", "Eurasia  Asia-Temperate");
            dd.Add("peninsular Malaysia", "Peninsular Malaysia");
            dd.Add("Malaysia-peninsular", "Peninsular Malaysia");
            dd.Add("Malaysia-Sabah", "Sabah");
            dd.Add("Malaysia-Sarawak", "Sarawak");
            dd.Add("Malesia", "Malaysia");
            dd.Add("Antarctic Ocean", "Southern Ocean");
            dd.Add("Russian SFSR", "Russia");
            dd.Add("USSR", "Russia");
            dd.Add("Antarctic", "Antarctica");
            dd.Add("Afrotropic", "Afrotropical");
            dd.Add("AfroTropical", "Afrotropical");
            dd.Add("NeoTropical", "South America");
            dd.Add("Neotropical", "South America");
            dd.Add("Neotropical region", "South America");
            dd.Add("Neotropic", "South America");
            dd.Add("BR", "Brazil");
            dd.Add("Indomalaya", "Southeast Asia");
            dd.Add("Rpublique Sud Africaine", "South Africa");
            dd.Add("Eurasia  Asia-Temperate", "Eurasia");
            dd.Add("Serbia & Kosovo", "Yugoslavia");
            dd.Add("Bosnia & Hercegovina", "Bosnia and Herzegovina");
            dd.Add("Herzegovina", "Bosnia and Herzegovina");
            dd.Add("Bosnia and Hercegovina", "Bosnia and Herzegovina");
            dd.Add("Bosnia", "Bosnia and Herzegovina");
            dd.Add("Canary Isl.", "Canary Islands");
            dd.Add("Canary Is", "Canary Islands");
            dd.Add("Ryukyu Isl.", "Ryukyu Is.");
            dd.Add("Jordania", "Jordan");
            dd.Add("Rpublique Centrafricaine", "Central African Republic");
            dd.Add("Central African Repu", "Central African Republic");
            dd.Add("Central African Rep.", "Central African Republic");
            dd.Add("Oceania  Pacific Islands", "Oceania");
            dd.Add("Britain", "Great Britain");
            dd.Add("Nearctic", "North America");
            dd.Add("NeArctic", "North America");
            dd.Add("Nearctic region", "North America");
            dd.Add("QLDQU", "Queensland");
            dd.Add("QLD", "Queensland");
            dd.Add("Qld", "Queensland");
            dd.Add("Qld.", "Queensland");
            dd.Add("QLD-QU", "Queensland");
            dd.Add("Australia. Queensland", "Queensland");
            dd.Add("Philippines:Luzon", "Luzon");
            dd.Add("Asia: Japan", "Japan");
            dd.Add("USA. Hawaii", "Hawaii");
            dd.Add("KEN", "Kenya");
            dd.Add("KEN-OO", "Kenya");
            dd.Add("KENOO", "Kenya");
            dd.Add("FRAFR", "France");
            dd.Add("ITA", "Italy");
            dd.Add("India-Kerala", "Kerala");
            dd.Add("GHA", "Ghana");
            dd.Add("Colombie", "Colombia");
            dd.Add("Australia. New South Wales", "New South Wales");
            dd.Add("Thalande", "Thailand");
            dd.Add("Cape Province", "Cape Provinces");
            dd.Add("India-Uttar Pradesh", "Uttar Pradesh");
            dd.Add("USA: California", "California");
            dd.Add("incl Madagascar", "Madagascar");
            dd.Add("Windward Islands", "Windward Is.");
            dd.Add("Leeward Islands", "Leeward Is.");
            dd.Add("Bismarck Arch.", "Bismarck Archipelago");
            dd.Add("Bismarck Arch", "Bismarck Archipelago");
            dd.Add("Jammu & Kashmir", "Jammu-Kashmir");
            dd.Add("Pakistani Kashmir", "Jammu-Kashmir");
            dd.Add("Mediterranean", "Mediterranean Sea");
            dd.Add("Argentine", "Argentina");
            dd.Add("Equateur", "Ecuador");
            dd.Add("Czechia", "Czech Republic");
            dd.Add("Galapagos Isl.", "Galapagos Is.");
            dd.Add("Philippines [Luzon", "Luzon");
            dd.Add("Philippines [Luzon]", "Luzon");
            dd.Add("Celebes", "Sulawesi");
            dd.Add("Galpagos", "Galapagos Is.");
            dd.Add("China: Yunnan", "Yunnan");
            dd.Add("Oriental(Philippines:Mindanao)", "Mindanao");
            dd.Add("Asia: Mongolia", "Mongolia");
            dd.Add("Peoples' Republic of China", "China");
            dd.Add("Asia: China", "China");
            dd.Add("Asia: China.", "China");
            dd.Add("Asia:  China.", "China");
            dd.Add("Asia: India.", "India");
            dd.Add("Asia:  India.", "India");
            dd.Add("Asia:  India", "India");
            dd.Add("Eurasia  Europe", "Eurasia");
            dd.Add("Tropical Indo-Pacific", "Indo-Pacific");
            dd.Add("Indo-West Pacific", "Indo-Pacific");
            dd.Add("Guyane Franaise", "French Guiana");
            dd.Add("Peoples' Republic of China-Fujian", "Fujian");
            dd.Add("Americas  South and Central America", "South America");
            //dd.Add("Americas  North America", "North America");
            //dd.Add("Eurasia  Europe","Europe");
            dd.Add("Aegean Is", "Aegean Isl");
            dd.Add("India-Karnataka", "Karnataka");
            dd.Add("Turks & Caicos Isl", "Turks-Caicos Is.");
            dd.Add("Indonesia: Borneo", "Borneo");
            dd.Add("India-Tamil Nadu", "Tamil Nadu");
            dd.Add("Falkland Is", "Falkland Islands");
            dd.Add("Espagne", "Spain");
            dd.Add("Slovaquie", "Slovakia");
            dd.Add("Bulgarie", "Bulgaria");
            dd.Add("Moldavia", "Moldova");
            dd.Add("Ile Andaman", "Andaman Is.");
            dd.Add("Andaman Islands", "Andaman Is.");
            dd.Add("Andaman Is", "Andaman Is.");
            dd.Add("Andaman Isl", "Andaman Is.");
            dd.Add("Andamans", "Andaman Is.");
            dd.Add("Society Isl", "Society Is.");
            dd.Add("Society Islands", "Society Is.");
            dd.Add("FIJ", "Fiji");
            dd.Add("Fiji Is", "Fiji");
            dd.Add("FIJOO", "Fiji");
            dd.Add("Fiji Islands", "Fiji");
            dd.Add("Lesser Sunda Isl", "Lesser Sunda Is.");
            dd.Add("EQG", "Equatorial Guinea");
            dd.Add("EQGOO", "Equatorial Guinea");
            dd.Add("EQG-OO", "Equatorial Guinea");
            dd.Add("Australian", "Australia");
            dd.Add("New Zealand  I", "New Zealand");
            dd.Add("Sinai peninsula", "Sinai");
            dd.Add("WAU", "Western Australia");
            dd.Add("Australia. Western Australia", "Western Australia");
            dd.Add("Northwest Pacific:  Japan.", "Japan");
            //dd.Add("Oriental(Philippines:Mindanao)", "Mindanao");
            dd.Add("Philippines [Mindanao]", "Mindanao");
            dd.Add("Prou", "Peru");
            dd.Add("So Paulo", "Sao Paulo");
            dd.Add("USA: Arizona", "Arizona");
            dd.Add("West Palearctic region", "Europe");
            dd.Add("AUstralasian", "Australasia");
            dd.Add("BZL-SP", "Sao Paulo");
            dd.Add("BZN-PA", "Brazil");
            dd.Add("CLM", "Colombia");
            dd.Add("CLMOO", "Colombia");
            dd.Add("CLM-OO", "Colombia");
            dd.Add("U.S. Continental Shelf", "United States Exclusive Economic Zone");
            dd.Add("Italie", "Italy");
            dd.Add("Chili", "Chile");
            dd.Add("Borneo Island", "Borneo");
            dd.Add("Hawaiian Islands", "Hawaii");
            dd.Add("ROM", "Romania");
            dd.Add("GER", "Germany");
            dd.Add("BUL", "Bulgaria");
            dd.Add("Trinidad & Tobago", "Trinidad and Tobago");
            dd.Add("trop. Africa", "Afrotropical");
            dd.Add("peninsular Thailand", "Thailand");
            dd.Add("Indian Subcontinent", "India");
            dd.Add("Czech Rep", "Czech Republic");
            dd.Add("Solomon Isl", "Solomon Islands");
            dd.Add("SRL", "Sri Lanka");
            dd.Add("SRLOO", "Sri Lanka");
            dd.Add("SRL-OO", "Sri Lanka");
            dd.Add("TEX", "Texas");
            dd.Add("COS", "Costa Rica");
            dd.Add("COSOO", "Costa Rica");
            dd.Add("COS-OO", "Costa Rica");
            dd.Add("Costa Rica", "Costa Rica");
            dd.Add("Afrotropical region", "Afrotropical");
            dd.Add("TAI", "Taiwan");
            dd.Add("TAIOO", "Taiwan");
            dd.Add("TAI-OO", "Taiwan");
            dd.Add("Iles Salomon", "Solomon Islands");
            dd.Add("Solomon Is", "Solomon Islands");
            dd.Add("Solomons", "Solomon Islands");
            dd.Add("St. Helena", "St.Helena");
            dd.Add("TAN", "Tanzania");
            dd.Add("TANOO", "Tanzania");
            dd.Add("TAN-OO", "Tanzania");
            dd.Add("Sahul Shelf", "Arafura Sea");
            dd.Add("South China sea", "South China Sea");
            dd.Add("Afrique", "Africa");
            dd.Add("Aleutian Islands", "Aleutian Is.");
            dd.Add("ALG", "Algeria");
            dd.Add("ALGOO", "Algeria");
            dd.Add("ALG-OO", "Algeria");
            dd.Add("Amrique du Nord", "North America");
            dd.Add("Andalusia", "Spain");
            dd.Add("ANG", "Angola");
            dd.Add("ANGOO", "Angola");
            dd.Add("ANG-OO", "Angola");
            dd.Add("Australia. Australian Capital Territory", "Australian Capital Territory");
            dd.Add("Australia. South Australia", "South Australia");
            dd.Add("Australia. Tasmania", "Tasmania");
            dd.Add("Australia. Victoria", "Victoria");
            dd.Add("Bahama Is.", "Bahamas");
            dd.Add("Balearic Islands", "Balearic Islands");
            dd.Add("Antarctica/Southern Ocean", "Southern Ocean");
            dd.Add("Antarctic  Antarctica", "Antarctica");
            dd.Add("Antarctic Peninsula", "Antarctica");
            dd.Add("Antarctic  Southern Subpolar Islands", "Antarctica");
            dd.Add("AUT-AU", "Austria");
            dd.Add("Belau", "Palau");
            dd.Add("Bhoutan", "Bhutan");
            dd.Add("Bougainville", "North Solomons");
            dd.Add("Brazil", "Brazil");
            dd.Add("Republic of South Africa", "South Africa");
            dd.Add("Cape Verde Is.", "Cape Verde");
            dd.Add("Cape Verde Islands", "Cape Verde");
            dd.Add("Caroline Islands", "Caroline Islands");
            dd.Add("Caucasia", "Caucasus");
            dd.Add("Caucasus Region", "Caucasus");
            dd.Add("CAL", "California");
            dd.Add("FLA", "Florida");
            dd.Add("Floridian", "Florida");
            dd.Add("Cambodge", "Cambodia");
            dd.Add("CBD", "Cambodia");
            dd.Add("CBDOO", "Cambodia");
            dd.Add("CBD-OO", "Cambodia");
            dd.Add("Cape Horn", "Tierra del Fuego (Argentina)");
            dd.Add("Cape Howe", "New South Wales");
            dd.Add("Central Kuroshio Current", "Eastern China Sea");
            dd.Add("East China Sea", "Eastern China Sea");
            dd.Add("Chatham Isl.", "Chatham Is.");
            dd.Add("China. Fujian", "Fujian");
            dd.Add("China. Hainan", "Hainan");
            dd.Add("China. Sichuan", "Sichuan");
            dd.Add("China. Yunnan", "Yunnan");
            dd.Add("China. Zhejiang", "Zhejiang");
            dd.Add("CHINA: Yunnan Prov.", "Yunnan");
            dd.Add("CHH", "Hainan");
            dd.Add("CHT", "Tibet");
            dd.Add("CHX", "Xinjiang");
            dd.Add("CMN", "Cameroon");
            dd.Add("CMNOO", "Cameroon");
            dd.Add("CMN-OO", "Cameroon");
            dd.Add("Comores", "Comoros");
            dd.Add("Comoro Islands", "Comoros");
            dd.Add("CPP", "Cape Provinces");
            dd.Add("Dominican Rep.", "Dominican Republic");
            dd.Add("Nippon", "Japan");
            dd.Add("Eastern Central Atlantic", "Atlantic Ocean");
            dd.Add("Eastern Central Pacific:  Hawaiian Islands.", "Hawaii");
            dd.Add("Eastern Central Pacific:  Marquesas Islands.", "Marquesas");
            dd.Add("ECUADOR", "Ecuador");
            dd.Add("Equatorial Africa", "Afrotropical");
            dd.Add("Equatorial West Africa", "Afrotropical");
            dd.Add("Equatorial West and East Africa", "Afrotropical");
            dd.Add("Ethiopie", "Ethiopia");
            dd.Add("Europe to Central Asia", "Eurasia");
            dd.Add("Fernando Po", "Bioko");
            dd.Add("Fernando Poo", "Bioko");
            dd.Add("Bioko Isl", "Bioko");
            dd.Add("Former Yugoslavia", "Yugoslavia");
            dd.Add("CRL", "Caroline Is.");
            dd.Add("CRLPA", "Palau");
            dd.Add("CRL-PA", "Palau");
            dd.Add("FRA-FR", "France");
            dd.Add("France. Corsica", "Corsica");
            dd.Add("FRG", "French Guiana");
            dd.Add("FRGOO", "French Guiana");
            dd.Add("FRG-OO", "French Guiana");
            dd.Add("Galicia", "Spain");
            dd.Add("Grce", "Greece");
            dd.Add("GUA", "Guatemala");
            dd.Add("GUAOO", "Guatemala");
            dd.Add("GUA-OO", "Guatemala");
            dd.Add("GUI", "Guinea");
            dd.Add("Guine", "Guinea");
            dd.Add("Guine quatoriale", "Equatorial Guinea");
            dd.Add("Gulf of Tonkin", "South China Sea");
            dd.Add("India [Assam]", "Assam");
            dd.Add("Inida-Assam", "Assam");
            dd.Add("India [Himachal Pradesh]", "Himachal Pradesh");
            dd.Add("India [Kerala]", "Kerala");
            dd.Add("India [Madras]", "Madras");
            dd.Add("India [Maharashtra]", "Maharashtra");
            dd.Add("India [Uttar Pradesh]", "Uttar Pradesh");
            dd.Add("India [W Bengal]", "West Bengal");
            dd.Add("India. Himachal Pradesh", "Himachal Pradesh");
            dd.Add("India?", "India");
            dd.Add("India-Uttaranchal", "Uttaranchal");
            dd.Add("India-West Bengal", "West Bengal");
            dd.Add("Indochina", "Southeast Asia");
            dd.Add("Indonesia-Java", "Java");
            dd.Add("Indonesia-Sulawesi", "Sulawesi");
            dd.Add("Isla del Coco", "Coco Is.");
            dd.Add("ITAIT", "Italy");
            dd.Add("ITA-IT", "Italy");
            dd.Add("IVO", "Ivory Coast");
            dd.Add("Jamaque", "Jamaica");
            dd.Add("JAPAN: Ryukyu Islands", "Ryukyu Is.");
            dd.Add("JAW", "Java");
            dd.Add("Juan Fdz. Isl.", "Juan Fernández Islands");
            dd.Add("Juan Fernandez Is.", "Juan Fernández Islands");
            dd.Add("Juan Fernndez Is.", "Juan Fernández Islands");
            dd.Add("Karakorum", "Karakoram");
            dd.Add("Kerguelen Islands", "Kerguelen");
            dd.Add("Kimberley", "Western Australia");
            dd.Add("Kirgizia", "Kirgizstan");
            dd.Add("LBS-LB", "Lebanon");
            dd.Add("LBS-SY", "Lebanon-Syria");
            dd.Add("LEE-GU", "Leeward Is.");
            dd.Add("LEE-MO", "Leeward Is.");
            dd.Add("Leeuwin", "Australian Exclusive Economic Zone");
            dd.Add("Lord Howe I", "Lord Howe I.");
            dd.Add("Lord Howe Isl.", "Lord Howe I.");
            dd.Add("Lord Howe Island", "Lord Howe I.");
            dd.Add("Lousiana", "Louisiana");
            dd.Add("Madagascaar", "Madagascar");
            dd.Add("MADAGASCAR", "Madagascar");
            dd.Add("LSI", "Lesser Sunda Is.");
            dd.Add("LSILS", "Lesser Sunda Is.");
            dd.Add("LSI-LS", "Lesser Sunda Is.");
            dd.Add("LSIBA", "Bali");
            dd.Add("LSI-BA", "Bali");
            dd.Add("LSIET", "Timor");
            dd.Add("LSI-ET", "Timor");
            dd.Add("Malvinas/Falklands", "Falkland Islands");
            dd.Add("Manning-Hawkesbury", "Australian Exclusive Economic Zone");
            dd.Add("Port Jackson", "Australian Exclusive Economic Zone");
            dd.Add("Port Phillip Bay", "Australian Exclusive Economic Zone");
            dd.Add("Central and Southern Great Barrier Reef", "Australian Exclusive Economic Zone");
            dd.Add("Marquesas Is.", "Marquesas");
            dd.Add("Marquesas Isl.", "Marquesas");
            dd.Add("Marquesas Islands", "Marquesas");
            dd.Add("Mexican Pacific Is.", "Mexico");
            dd.Add("Mexican Tropical Pacific", "Mexico");
            dd.Add("Micronsie", "Micronesia");
            dd.Add("MLW", "Malawi");
            dd.Add("Mongolie", "Mongolia");
            dd.Add("MOR", "Morocco");
            dd.Add("MORMO", "Morocco");
            dd.Add("MOR-MO", "Morocco");
            dd.Add("MXE-NL", "Nuevo Leon");
            dd.Add("MXE-SL", "San Luis Potosi");
            dd.Add("MXS-MI", "Michoacan");
            dd.Add("MXT-YU", "Yucatan");
            dd.Add("MYA", "Myanmar");
            dd.Add("MYAOO", "Myanmar");
            dd.Add("MYA-OO", "Myanmar");
            dd.Add("N Thailand", "Thailand");
            dd.Add("N.Amer.", "North America");
            dd.Add("Namaqua", "Namibia");
            dd.Add("Namibie", "Namibia");
            dd.Add("NE Australia", "Queensland");
            dd.Add("NAT", "KwaZulu-Natal");
            dd.Add("NATOO", "KwaZulu-Natal");
            dd.Add("NAT-OO", "KwaZulu-Natal");
            dd.Add("NEP", "Nepal");
            dd.Add("NEP-OO", "Nepal");
            dd.Add("New Ireland", "New Ireland (island)");
            dd.Add("NEW ZEALAND", "New Zealand");
            dd.Add("Norfolk Isl.", "Norfolk Is.");
            dd.Add("North West Atlantic", "Atlantic Ocean");
            dd.Add("NTA", "Northern Territory");
            dd.Add("NWC", "New Caledonia");
            dd.Add("NWCOO", "New Caledonia");
            dd.Add("NWC-OO", "New Caledonia");
            dd.Add("NWM", "New Mexico");
            dd.Add("NWMOO", "New Mexico");
            dd.Add("NWM-OO", "New Mexico");
            dd.Add("NZN", "New Zealand North");
            dd.Add("NZS", "New Zealand South");
            dd.Add("NZNOO", "New Zealand North");
            dd.Add("NZSOO", "New Zealand South");
            dd.Add("NZN-OO", "New Zealand North");
            dd.Add("NZS-OO", "New Zealand South");
            dd.Add("New Zealand North I", "New Zealand North");
            dd.Add("New Zealand South I", "New Zealand South");
            dd.Add("Oahu Island", "Hawaii");
            dd.Add("ORE", "Oregon");
            dd.Add("OREOO", "Oregon");
            dd.Add("ORE-OO", "Oregon");
            dd.Add("Oriental(Nias)", "Nias");
            dd.Add("Ouganda", "Uganda");
            dd.Add("Oyashio Current", "Pacific Ocean");
            dd.Add("Pacific Coast of Mexico", "Mexico");
            dd.Add("PAL", "Israel");
            dd.Add("Palau Islands", "Palau");
            dd.Add("PALIS", "Israel");
            dd.Add("PAL-IS", "Israel");
            dd.Add("PAL-JO", "Jordan");
            dd.Add("PAR", "Paraguay");
            dd.Add("Par", "Paraguay");
            dd.Add("PAROO", "Paraguay");
            dd.Add("Peoples' Republic of China-Beijing", "Beijing");
            dd.Add("Peoples' Republic of China-Hebei", "Hebei");
            dd.Add("Peoples' Republic of China-Heilongjiang", "Heilongjiang");
            dd.Add("Peoples' Republic of China-Jiangxi", "Jiangxi");
            dd.Add("Peoples' Republic of China-Liaoning", "Liaoning");
            dd.Add("Peoples' Republic of China-Shaanxi", "Shaanxi");
            dd.Add("Peoples' Republic of China-Yunnan", "Yunnan");
            dd.Add("Philippines [Palawan]", "Palawan");
            dd.Add("POR", "Portugal");
            dd.Add("POROO", "Portugal");
            dd.Add("POR-OO", "Portugal");
            dd.Add("Porto Rico", "Puerto Rico");
            dd.Add("PUE", "Puerto Rico");
            dd.Add("PUEOO", "Puerto Rico");
            dd.Add("PUE-OO", "Puerto Rico");
            dd.Add("Raiatea", "Society Is.");
            dd.Add("Raja Ampat", "New Guinea");
            dd.Add("Republic of Guyana", "Guyana");
            dd.Add("Rio De Janeiro", "Rio de Janeiro");
            dd.Add("Rpublique de Core", "Korea");
            dd.Add("Rpublique Dominicaine", "Dominican Republic");
            dd.Add("Rpublique Dominicaine, Hati", "Hispaniola");
            dd.Add("Russia-Primor'ye Kray", "Primorye");
            dd.Add("S Africa: Cape Province", "Cape Provinces");
            dd.Add("S. Africa", "South Africa");
            dd.Add("Republic South Africa", "South Africa");
            dd.Add("Republic South Africa:Cape", "Cape Provinces");
            dd.Add("Republic South Africa:Eastern Cape", "Eastern Cape Province");
            dd.Add("Republic South Africa:Western Cape", "Western Cape Province");
            dd.Add("Republic South Africa:Northern Cape", "Northern Cape Province");
            dd.Add("Republic South Africa:KwaZulu Natal", "KwaZulu-Natal");
            dd.Add("Republic of Guinea", "Guinea");
            dd.Add("s.California", "California");
            dd.Add("SAM", "Samoa");
            dd.Add("SAMWS", "Samoa");
            dd.Add("SE Australia", "Australia");
            dd.Add("SEY", "Seychelles");
            dd.Add("SEYOO", "Seychelles");
            dd.Add("SEY-OO", "Seychelles");
            dd.Add("Sao Tome", "Sao Tome and Principe");
            dd.Add("Shaanix", "Shaanxi");
            dd.Add("Sngal", "Senegal");
            dd.Add("So Tom and Prncipe", "Sao Tome and Prncipe");
            dd.Add("So Tom", "Sao Tome and Principe");
            dd.Add("Somalie", "Somalia");
            dd.Add("South America.", "South America");
            dd.Add("South Coast of South Africa", "South Africa");
            dd.Add("South European Atlantic Shelf", "Atlantic Ocean");
            dd.Add("Southern California Bight", "California");
            dd.Add("South West Atlantic", "Atlantic Ocean");
            dd.Add("SPA", "Spain");
            dd.Add("Spain-F.E.", "Spain");
            dd.Add("SPASP", "Spain");
            dd.Add("SPA-SP", "Spain");
            dd.Add("St Vincent & Grenadines", "St.Vincent");
            dd.Add("SUD", "Sudan");
            dd.Add("SUDOO", "Sudan");
            dd.Add("SUD-OO", "Sudan");
            dd.Add("Sulawesi Sea/Makassar Strait", "Makassar Strait");
            dd.Add("Tadjikistan", "Tajikistan");
            dd.Add("TAIWAN", "Taiwan");
            dd.Add("Temperate Australasia", "Australia");
            dd.Add("Temperate Northern Atlantic", "Atlantic Ocean");
            dd.Add("Temperate Northern Pacific", "Pacific Ocean");
            dd.Add("Tierra del Fuego", "Tierra del Fuego (Argentina)");
            dd.Add("Timor Island", "Timor");
            dd.Add("Torres Strait Northern Great Barrier Reef", "Australian Exclusive Economic Zone");
            dd.Add("Transcaucasia", "Transcaucasus");
            dd.Add("Trinidad y Tobago", "Trinidad-Tobago");
            dd.Add("TRT", "Trinidad-Tobago");
            dd.Add("trop. America", "South America");
            dd.Add("Tropical Atlantic", "Atlantic Ocean");
            dd.Add("Tuamotu Islands", "Tuamotu");
            dd.Add("TUNISIA", "Tunisia");
            dd.Add("UAE", "United Arab Emirates");
            dd.Add("UK", "United Kingdom");
            dd.Add("United Kingdom-England", "United Kingdom");
            dd.Add("URU", "Uruguay");
            dd.Add("URUOO", "Uruguay");
            dd.Add("URU-OO", "Uruguay");
            dd.Add("VAN", "Vanuatu");
            dd.Add("VANOO", "Vanuatu");
            dd.Add("VAN-OO", "Vanuatu");
            dd.Add("Virgin Islands", "Virgin Is.");
            dd.Add("Western Cape", "Western Cape Province");
            dd.Add("West Indies", "Caribbean");
            dd.Add("Western and Northern Madagascar", "Madagascar");
            dd.Add("Western South Atlantic", "Atlantic Ocean");
            dd.Add("West Central Atlantic", "Atlantic Ocean");
            dd.Add("Widespread Europe", "Europe");
            dd.Add("widespread mainland Afrotrop. Reg.", "Africa");
            dd.Add("Wyo.", "Wyoming");
            dd.Add("Xizang", "Tibet");
            dd.Add("Yukon Territory", "Yukon");
            dd.Add("ZAM", "Zambia");
            dd.Add("Zambie", "Zambia");
            dd.Add("ZIM", "Zimbabwe");
            dd.Add("Antioquia", "Antioquia Department");
            dd.Add("Bolivar", "Bolívar (state)");
            dd.Add("Parana", "Paraná (state)");
            dd.Add("Para", "Pará");
            dd.Add("Valle", "Valle del Cauca Department");
            dd.Add("Choc", "Chocó Department");
            dd.Add("Cauca", "Cauca Department");
            dd.Add("Nario", "Nariño Department");
            dd.Add("Santander", "Santander Department");
            dd.Add("Boyac", "Boyacá Department");
            dd.Add("Arauca", "Arauca Department");
            dd.Add("Atlntico", "Atlántico Department");
            //dd.Add("Boyac", "Boyacá Department");
            dd.Add("Caldas", "Caldas Department");
            dd.Add("Caquet", "Caquet Department");
            dd.Add("Casanare", "Casanare Department");
            dd.Add("Cesar", "Cesar Department");
            dd.Add("Crdoba", "Córdoba Department");
            dd.Add("Cundinamarca", "Cundinamarca Department");
            dd.Add("Guaina", "Guainía Department");
            dd.Add("Guaviare", "Guaviare Department");
            dd.Add("Huila", "Huila Department");
            dd.Add("La Guajira", "La Guajira Department");
            dd.Add("Magdalena", "Magdalena Department");
            dd.Add("Meta", "Meta Department");
            dd.Add("Norte de Santander", "Norte de Santander Department");
            dd.Add("Putumayo", "Putumayo Department");
            dd.Add("Quindo", "Quindío Department");
            dd.Add("Risaralda", "Risaralda Department");
            dd.Add("San Andrs", "Archipelago of San Andrés, Providencia and Santa Catalina");
            //dd.Add("Sucre", "Sucre Department");
            dd.Add("Tolima", "Tolima Department");
            dd.Add("Vaups", "Vaupés Department");
            dd.Add("Vichada", "Vichada Department");
            dd.Add("Europe & Northern Asia", "Eurasia");
            dd.Add("Inner Anatolia", "Anatolia");
            dd.Add("SSW-Anatolia", "Anatolia");
            dd.Add("Brazzaville", "Congo-Brazzaville");
            dd.Add("Washington State", "Washington");
            dd.Add("Merida", "Mérida (state)");
            dd.Add("Tachira", "Táchira (state)");
            dd.Add("Trujillo", "Trujillo (state)");
            dd.Add("Vargas", "Vargas (state)");
            dd.Add("Miranda", "Miranda (state)");
            dd.Add("Carabobo", "Carabobo (state)");
            dd.Add("Falcon", "Falcón (state)");
            dd.Add("Lara", "Lara (state)");
            dd.Add("Yaracuy", "Yaracuy (state)");
            dd.Add("Apure", "Apure (state)");
            dd.Add("Barinas", "Barinas (state)");
            dd.Add("Cojedes", "Cojedes (state)");
            dd.Add("Guarico", "Guárico (state)");
            dd.Add("Portuguesa", "Portuguesa (state)");
            dd.Add("Anzoategui", "Anzoátegui (state)");
            dd.Add("Monagas", "Monagas (state)");
            dd.Add("Virgin Isl", "Virgin Is.");
            dd.Add("Palau Isl", "Palau");
            dd.Add("Caribbean-TRP", "Caribbean");
            dd.Add("Darjeeling", "Darjeeling district");
            dd.Add("Moluques", "Moluccas");
            dd.Add("Cape Verde Isl", "Cape Verde");
            dd.Add("Nicobar Isl", "Nicobar Is.");
            dd.Add("Nicobar Isl.", "Nicobar Is.");
            dd.Add("Baltic sea", "Baltic Sea");
            dd.Add("Cayman Isl", "Cayman Islands");
            dd.Add("Mexico to Costa Rica", "Central America");
            dd.Add("Mexico to Panama", "Central America");
            dd.Add("HAI", "Haiti");
            dd.Add("HAIHA", "Haiti");
            dd.Add("HAI-HA", "Haiti");
            dd.Add("Anatolia: Bithynia", "Anatolia");
            dd.Add("Chuquisaca", "Chuquisaca Department");
            dd.Add("Tarija", "Tarija Department");
            dd.Add("Cochabamba", "Cochabamba Department");
            dd.Add("Oruro", "Oruro Department");
            dd.Add("Pando", "Pando Department");
            dd.Add("Potosi", "Potosí Department");
            dd.Add("Beni", "Beni Department");
            dd.Add("Bolvar", "Bolívar Department");
            dd.Add("Oriental", "Asia");
            dd.Add("European Turkey", "Turkey-in-Europe");
            dd.Add("Turkey in Europe", "Turkey-in-Europe");
            //dd.Add("","");
            //dd.Add("","");
            //dd.Add("","");
            //dd.Add("","");

            List<string> islandlist = new List<string>();
            islandlist.Add(" Is");
            islandlist.Add(" Isl");
            islandlist.Add(" Island");
            islandlist.Add(" Islands");
            islandlist.Add(" Is.");
            islandlist.Add(" Isl.");

            List<string> dummylist = dd.Keys.ToList();
            foreach (string s in dummylist)
            {
                foreach (string isl in islandlist)
                {
                    if ( s.EndsWith(isl))
                    {
                        foreach (string isl2 in islandlist)
                        {
                            if (isl2 != isl)
                            {
                                string s2 = s.Replace(isl, isl2);
                                if (!dd.ContainsKey(s2))
                                    dd.Add(s2, dd[s]);
                            }
                        }
                    }
                }
            }
            
            foreach (string s in distlist)
            {
                foreach (string isl in islandlist)
                {
                    if (s.EndsWith(isl))
                    {
                        foreach (string isl2 in islandlist)
                        {
                            if (isl2 != isl)
                            {
                                string s2 = s.Replace(isl, isl2);
                                if (!dd.ContainsKey(s2))
                                    dd.Add(s2, s);
                            }
                        }
                    }
                }
            }
            return dd;
        }

        public static Dictionary<string, string> getstatedict() //US/CA/Aus state abbreviations
        {
            Dictionary<string, string> localstatedict = new Dictionary<string, string>();
            localstatedict.Add("AL", "Alabama");
            localstatedict.Add("AK", "Alaska");
            localstatedict.Add("AZ", "Arizona");
            localstatedict.Add("AR", "Arkansas");
            localstatedict.Add("CA", "California");
            localstatedict.Add("CO", "Colorado");
            localstatedict.Add("CT", "Connecticut");
            localstatedict.Add("DE", "Delaware");
            localstatedict.Add("DC", "District of Columbia");
            localstatedict.Add("D.C", "District of Columbia");
            localstatedict.Add("FL", "Florida");
            localstatedict.Add("GA", "Georgia");
            localstatedict.Add("HI", "Hawaii");
            localstatedict.Add("ID", "Idaho");
            localstatedict.Add("IL", "Illinois");
            localstatedict.Add("IN", "Indiana");
            localstatedict.Add("IA", "Iowa");
            localstatedict.Add("KS", "Kansas");
            localstatedict.Add("KY", "Kentucky");
            localstatedict.Add("LA", "Louisiana");
            localstatedict.Add("ME", "Maine");
            localstatedict.Add("MD", "Maryland");
            localstatedict.Add("MA", "Massachusetts");
            localstatedict.Add("MI", "Michigan");
            localstatedict.Add("MN", "Minnesota");
            localstatedict.Add("MS", "Mississippi");
            localstatedict.Add("MO", "Missouri");
            localstatedict.Add("MT", "Montana");
            localstatedict.Add("NE", "Nebraska");
            localstatedict.Add("NV", "Nevada");
            localstatedict.Add("NH", "New Hampshire");
            localstatedict.Add("NJ", "New Jersey");
            localstatedict.Add("NM", "New Mexico");
            localstatedict.Add("NY", "New York");
            localstatedict.Add("NC", "North Carolina");
            localstatedict.Add("ND", "North Dakota");
            localstatedict.Add("OH", "Ohio");
            localstatedict.Add("OK", "Oklahoma");
            localstatedict.Add("OR", "Oregon");
            localstatedict.Add("PA", "Pennsylvania");
            localstatedict.Add("RI", "Rhode Island");
            localstatedict.Add("SC", "South Carolina");
            localstatedict.Add("SD", "South Dakota");
            localstatedict.Add("TN", "Tennessee");
            localstatedict.Add("TX", "Texas");
            localstatedict.Add("UT", "Utah");
            localstatedict.Add("VT", "Vermont");
            localstatedict.Add("VA", "Virginia");
            localstatedict.Add("WA", "Washington");
            localstatedict.Add("WV", "West Virginia");
            localstatedict.Add("WI", "Wisconsin");
            localstatedict.Add("WY", "Wyoming");

            localstatedict.Add("Ariz", "Arizona");
            localstatedict.Add("Ark", "Arkansas");
            localstatedict.Add("Calif", "California");
            localstatedict.Add("Colo", "Colorado");
            localstatedict.Add("Conn", "Connecticut");
            localstatedict.Add("De", "Delaware");
            localstatedict.Add("D.C.", "District of Columbia");
            localstatedict.Add("Fla", "Florida");
            localstatedict.Add("Ga", "Georgia");
            localstatedict.Add("Ill", "Illinois");
            localstatedict.Add("Ind", "Indiana");
            localstatedict.Add("Kans", "Kansas");
            localstatedict.Add("La", "Louisiana");
            localstatedict.Add("Mass", "Massachusetts");
            localstatedict.Add("Mich", "Michigan");
            localstatedict.Add("Minn", "Minnesota");
            localstatedict.Add("Miss", "Mississippi");
            localstatedict.Add("Mo", "Missouri");
            localstatedict.Add("Mont", "Montana");
            localstatedict.Add("Nebr", "Nebraska");
            localstatedict.Add("Nev", "Nevada");
            localstatedict.Add("NMex", "New Mexico");
            localstatedict.Add("Okl", "Oklahoma");
            localstatedict.Add("Ore", "Oregon");
            localstatedict.Add("Oreg", "Oregon");
            localstatedict.Add("Pa", "Pennsylvania");
            localstatedict.Add("Penn", "Pennsylvania");
            localstatedict.Add("Tenn", "Tennessee");
            localstatedict.Add("Ut", " Utah");
            localstatedict.Add("Vt", " Vermont");
            localstatedict.Add("Va", " Virginia");
            localstatedict.Add("Wash", " Washington");
            localstatedict.Add("Wis", " Wisconsin");
            localstatedict.Add("Wyo", " Wyoming");




            localstatedict.Add("ON", "Ontario");
            localstatedict.Add("QC", "Quebec");
            localstatedict.Add("NS", "Nova Scotia");
            localstatedict.Add("NB", "New Brunswick");

            localstatedict.Add("MB", "Manitoba");
            localstatedict.Add("BC", "British Columbia");
            localstatedict.Add("PE", "Prince Edward Island");
            localstatedict.Add("SK", "Saskatchewan");
            localstatedict.Add("AB", "Alberta");
            localstatedict.Add("NL", "Newfoundland and Labrador");
            localstatedict.Add("YT", "Yukon");
            localstatedict.Add("NT", "Northwest Territories");
            localstatedict.Add("NU", "Nunavut");

            localstatedict.Add("Ont", "Ontario");
            localstatedict.Add("Que", "Quebec");

            localstatedict.Add("Man", "Manitoba");
            localstatedict.Add("Sask", "Saskatchewan");
            localstatedict.Add("Alb", "Alberta");
            localstatedict.Add("Labr", "Newfoundland and Labrador");
            localstatedict.Add("LB", "Newfoundland and Labrador");
            localstatedict.Add("NF", "Newfoundland and Labrador");
            localstatedict.Add("Nfld", "Newfoundland and Labrador");
            localstatedict.Add("NWT", "Northwest Territories");
            localstatedict.Add("N.W.T.", "Northwest Territories");
            localstatedict.Add("N.W.T", "Northwest Territories");
            localstatedict.Add("Nun", "Nunavut");

            localstatedict.Add("NSW-au", "New South Wales");
            localstatedict.Add("Qld-au", "Queensland");
            localstatedict.Add("Qld.-au", "Queensland");
            localstatedict.Add("SA-au", "South Australia");
            localstatedict.Add("TAS-au", "Tasmania");
            localstatedict.Add("VIC-au", "Victoria");
            localstatedict.Add("QLD-au", "Queensland");
            localstatedict.Add("Tas-au", "Tasmania");
            localstatedict.Add("Tas.-au", "Tasmania");
            localstatedict.Add("Vic-au", "Victoria");
            localstatedict.Add("Vic.-au", "Victoria");
            localstatedict.Add("WA-au", "Western Australia");



            localstatedict.Add("ACT-au", "Australian Capital Territory");
            localstatedict.Add("NT-au", "Northern Territory");

            return localstatedict;
        }

        public static Dictionary<string, int> statestat = new Dictionary<string, int>();

        public static string getstate(string shortstate)
        {
            if (statedict.Count == 0)
                statedict = getstatedict();
            if (shortstate.Length < 10)
                if (!statestat.ContainsKey(shortstate))
                    statestat.Add(shortstate, 0);
            if (statedict.ContainsKey(shortstate))
            {
                statestat[shortstate]++;
                return statedict[shortstate];
            }
            else
            {
                if (shortstate.Length < 10)
                    statestat[shortstate]--;
                return shortstate;
            }
        }

    }
}

