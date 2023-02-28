using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MakeSpecies
{
        public class countryclass //class for country data
        {
            public string Name = ""; //main name
            public string Name_ml = ""; //name in makelang language 
            public string asciiname = ""; //name in plain ascii
            public List<string> altnames = new List<string>(); //alternative names
            public string iso = "XX";
            public string iso3 = "XXX";
            public int isonumber = 0;
            public string fips = "XX";
            public string capital = "";
            public int capital_gnid = 0;
            public double area = 0.0;
            public long population = 0;
            public string continent = "EU";
            public string tld = ".xx";
            public string currencycode = "USD";
            public string currencyname = "Dollar";
            public string phone = "1-999";
            public string postalcode = "#####";
            public string nativewiki = "";
            public List<int> languages = new List<int>();
            public List<string> bordering = new List<string>();
            public double clat = 9999; //lat, long of centroid of country shape
            public double clon = 9999;

        public static List<countryclass> countrylist = new List<countryclass>();
        public static Dictionary<string, int> countrystat = new Dictionary<string, int>();


        public static void read_country_info()
        {
            int n = 0;
            if (countrylist.Count > 0)
            {
                return;
            }

            using (StreamReader sr = new StreamReader(util.geonamesfolder + "countryInfo.txt"))
            {
                int makelangcol = -1;
                string makelang = "ceb";
                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();

                    if (line[0] == '#')
                        continue;

                    //if (n > 250)
                    //    Console.WriteLine(line);

                    string[] words = line.Split('\t');

                    //foreach (string s in words)
                    //    Console.WriteLine(s);

                    //Console.WriteLine(words[0] + "|" + words[1]);

                    if (words[0] == "ISO") //headline
                    {
                        for (int i = 1; i < words.Length; i++)
                        {
                            if (words[i] == makelang)
                                makelangcol = i;
                        }
                        continue;
                    }

                    int geonameid = -1;

                    countryclass country = new countryclass();

                    country.Name = words[4];
                    geonameid = util.tryconvert(words[16]);
                    country.iso = words[0];
                    country.iso3 = words[1];
                    country.isonumber = util.tryconvert(words[2]);
                    country.fips = words[3];
                    country.capital = words[5];
                    country.area = util.tryconvertdouble(words[6]);
                    country.population = util.tryconvertlong(words[7]);
                    country.continent = words[8];
                    country.tld = words[9];
                    country.currencycode = words[10];
                    country.currencyname = words[11];
                    country.phone = words[12];
                    country.postalcode = words[13];
                    foreach (string ll in words[15].Split(','))
                    {
                        //Console.WriteLine("ll.Split('-')[0] = " + ll.Split('-')[0]);
                        string lcode = ll.Split('-')[0];
                        if (String.IsNullOrEmpty(country.nativewiki))
                            country.nativewiki = lcode;
                        //if (langtoint.ContainsKey(lcode))
                        //    country.languages.Add(langtoint[lcode]);
                    }
                    foreach (string ll in words[17].Split(','))
                        country.bordering.Add(ll);

                    if (makelangcol > 0)
                    {
                        country.Name_ml = words[makelangcol];
                    }
                    else
                    {
                        country.Name_ml = country.Name;
                    }

                    countrylist.Add(country);
                    //countryml.Add(country.Name, country.Name_ml);
                    //countryiso.Add(country.Name, country.iso);

                    //if (geonameid > 0)
                    //{
                    //    countryid.Add(country.iso, geonameid);

                    //    countrydict.Add(geonameid, country);
                    //    //Console.WriteLine(country.iso+":"+geonameid.ToString());
                    //}

                    n++;
                    if ((n % 10) == 0)
                    {
                        Console.WriteLine("n (country_info)   = " + n.ToString());

                    }

                }

                Console.WriteLine("n    (country_info)= " + n.ToString());



            }

            //fill_motherdict();
            //fill_nocapital();
        }

        public static string getcountry(string isopar)
        {
            if (countryclass.countrylist.Count == 0)
                countryclass.read_country_info();
            string iso = isopar;
            if (isopar.Length <= 6)
            {
                if (!countrystat.ContainsKey(isopar))
                    countrystat.Add(isopar, 0);
            }
            if (iso.EndsWith("OO"))
            {
                iso = iso.Replace("OO", "").Trim('-');
            }
            var q = from c in countryclass.countrylist
                    where ((c.iso == iso.ToUpper()) || (c.iso3 == iso.ToUpper()))
                    select c;

            if (q.Count() == 1)
            {
                if (isopar.Length <= 6)
                    countrystat[isopar]++;
                return q.First().Name;
            }
            else
            {
                string ic = util.initialcap(iso);
                var qic = from c in countryclass.countrylist where c.Name == ic select c;
                if (qic.Count() == 1)
                {
                    if (isopar.Length <= 6)
                        countrystat[isopar]++;
                    return qic.First().Name;
                }
                if (isopar.Length <= 6)
                    countrystat[isopar]--;
                return isopar;
            }

        }

        public static string getcountryml(string isopar)
        {
            if (countryclass.countrylist.Count == 0)
                countryclass.read_country_info();
            string iso = isopar;
            if (iso.EndsWith("OO"))
            {
                iso = iso.Replace("OO", "").Trim('-');
            }
            var q = from c in countryclass.countrylist
                    where ((c.iso == iso.ToUpper()) || (c.iso3 == iso.ToUpper()))
                    select c;

            if (q.Count() == 1)
            {
                //if (isopar.Length <= 6)
                //    countrystat[iso.ToUpper()]++;
                return q.First().Name_ml;
            }
            else
            {
                string ic = util.initialcap(iso);
                var qic = from c in countryclass.countrylist where c.Name == ic select c;
                if (qic.Count() == 1)
                {
                    return qic.First().Name_ml;
                }
                return isopar;
            }

        }




    }



}
