using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MakeSpecies
{
    public partial class DistributiontestForm : Form
    {
        COL2019 db;
        public DistributiontestForm(COL2019 dbpar)
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



        private void Statsbutton_Click(object sender, EventArgs e)
        {
            Dictionary<int, int> taxonstat = new Dictionary<int, int>();
            Dictionary<string, int> diststat = new Dictionary<string, int>()
            {
            { "Guatemala",0 },
            { "Panay",0 },
            { "Guam",0 },
            { "Manchuria",0 },
            { "Galapagos Is.",0 },
            { "Turkmenistan",0 },
            { "Irrawaddy",0 },
{"Zambia",0},
{"Botswana",0},
{"Malawi",0},
{"Somalia",0},
{"Swaziland",0},
{"Kenya",0},
{"Tanzania",0},
{"Mozambique",0},
{"Zimbabwe",0},
{"Senegal",0},
{"Northern Provinces",0},
{"Ethiopia",0},
{"Uganda",0},
{"Sudan",0},
{"Rwanda",0},
{"Guinea",0},
{"Cameroon",0},
{"Sierra Leone",0},
{"Congo",0},
{"Liberia",0},
{"Nigeria",0},
{"Gabon",0},
{"Equatorial Guinea",0},
{"Central African Republic",0},
{"Ghana",0},
{"Benin",0},
{"Guinea-Bissau",0},
{"Togo",0},
{"Angola",0},
{"Namibia",0},
{"Borneo",0},
{"Paraguay",0},
            { "Réunion",0 },
            { "Papua New Guinea",0 },
 {"Thailand",0},
 {"Philippines",0},
{"Myanmar",0},
{"Bangladesh",0},
{"Peru",0},
{"Guyana",0},
{"Cuba",0},
{"Dominican Republic",0},
{"Puerto Rico",0},
{"Bahamas",0},
{"Leeward Is.",0},
{"Ecuador",0},
{"Venezuela",0},
{"Haiti",0},
{"Jamaica",0},
{"French Guiana",0},
{"Colombia",0},
{"Windward Is.",0},
{"Honduras",0},
{"El Salvador",0},
{"Madagascar",0},
{"Gulf of Guinea Is.",0},
{"New Guinea",0},
{"Costa Rica",0},
{"Nicaragua",0},
{"Belize",0},
{"Comoros",0},
{"Sri Lanka",0},
{"Bolivia",0},
{"Seychelles",0},
{"Caroline Is.",0},
{"New Caledonia",0},
{"Marianas",0},
{"Tonga",0},
{"Niue",0},
{"Cambodia",0},
{"Taiwan",0},
{"Vanuatu",0},
{"Fiji",0},
{"Cape Provinces",0},
{"Uruguay",0},
{"Mauritius",0},
{"Lesotho",0},
{"Bermuda",0},
{"Spain",0},
{"Germany",0},
{"Austria",0},
{"Georgia",0},
{"Portugal",0},
{"Norfolk Is.",0},
{"Korea",0},
{"Mauritania",0},
{"Eritrea",0},
{"Pakistan",0},
{"Niger",0},
{"Afghanistan",0},
{"Morocco",0},
{"Algeria",0},
{"Tunisia",0},
{"Nepal",0},
{"Romania",0},
{"Bulgaria",0},
{"Ukraine",0},
{"Hungary",0},
{"France",0},
{"Greece",0},
{"Turkey",0},
{"Albania",0},
{"Belgium",0},
{"Netherlands",0},
{"Poland",0},
{"Switzerland",0},
{"Denmark",0},
{"Baltic States",0},
{"Kirgizstan",0},
{"European Russia",0},
{"Palestine",0},
{"Libya",0},
{"Egypt",0},
{"Western Sahara",0},
{"Ireland",0},
{"Cyprus",0},
{"Iraq",0},
{"Burundi",0},
{"Chad",0},
{"Maldives",0},
{"Samoa",0},
{"Nauru",0},
{"Tuvalu",0},
{"Saudi Arabia",0},
{"Uzbekistan",0},
{"Kazakhstan",0},
{"Mongolia",0},
{"Sweden",0},
{"Norway",0},
{"Djibouti",0},
{"Oman",0},
{"Aruba",0},
{"Finland",0},
{"Iceland",0},
{"Greenland",0},
{"Kuwait",0},
{"Antarctica",0},
{"Czech Republic",0},
{"Lithuania",0},
{"Slovakia",0},
{"Slovenia",0},
{"Latvia",0},
{"Israel",0},
          { "Russia",0 },
{"Estonia",0},
{"Scandinavia",0},
{"Hong Kong",0},
{"Mexico Gulf",0},
{"Himalaya",0},
            { "Quebec",0 },
{"Hawaii",0},
{"Italy",0},
{"Caribbean",0},
{"Belarus",0},
{"Caucasus",0},
{"Mali",0},
{"Iran",0},
{"Solomon Is.",0},
{"Mediterranean Sea",0},
{"England",0},
{"Dominica",0},
{"Singapore",0},
{"Trinidad",0},
{"Croatia",0},
{"Serbia",0},
{"Macedonia",0},
{"Montenegro",0},
{"Crimea",0},
{"Andorra",0},
{"Azerbaijan",0},
{"Java",0},
{"Corsica",0},
{"Siberia",0},
{"Sumatra",0},
{"Hispaniola",0},
{"Crete",0},
{"Sicily",0},
{"Armenia",0},
{"Sardinia",0},
{"Malta",0},
{"Moluccas",0},
{"Lebanon",0},
            { "Luzon",0 },
            { "Mindanao",0 },
            { "Congo-Brazzaville",0 },
{"Arctic",0},
{"Atlantic Ocean",0},
{"Gulf of Saint Lawrence",0},
{"Pacific Ocean",0},
{"Bhutan",0},
{"Luxembourg",0},
{"Ryukyu Is.",0},
{"Martinique",0},
{"St. Vincent",0},
{"Guadeloupe",0},
{"Tobago",0},
{"Palawan",0},
{"Canary Islands",0},
            { "Melanesia",0 },
            { "Micronesia",0 },
            { "Polynesia",0 },
            { "Leyte",0 },
            { "Mindoro",0 },
            { "Anatolia",0 },
            { "North America",0 },
            { "Southern Asia",0 },
            { "South America",0 },
            { "Central America",0 },
            { "Oceania",0 },
            { "Inner Mongolia",0 },
            { "Transcaucasus",0 },
            { "Czechoslovakia",0 },
            { "Samar",0 },
            { "Baikal Lake",0 },
            { "Asia",0 },
            { "Eurasia",0 },
            { "Europe",0 },
            { "Europe & Northern Asia",0 },
            { "Afrotropical",0 },
            { "Southeast Asia",0 },
            { "Moldova",0 },
            { "Yugoslavia",0 },
            { "Jordan",0 },
            { "Scotland",0 },
            { "Liechtenstein",0 },
            { "Wales",0 },
            { "Michoacan",0 },
            { "Mexico State",0 },
            { "San Luis Potosi",0 },
            { "Queretaro",0 },
            { "Sao Paulo",0 },
            { "Nuevo Leon",0 },
            { "Baja California Norte",0 },
            { "Pahang",0 },
            { "Perak",0 },
            { "Kivu",0 },
            { "Cundinamarca",0 },
            { "Selangor",0 },
            { "Lombok",0 },
            { "Flores",0 },
            { "Timor",0 },
            { "Halmahera",0 },
            { "American Samoa",0 },
            { "Georgia [Caucasus]",0 },
            { "Zanzibar",0 },
            { "Transvaal",0 },
            { "Indo-Pacific",0 },
            { "Tahiti",0 },
            { "Natal",0 },
{"Lebanon-Syria",0},
{"Oceania  Australasia",0},
{"Eurasia  Asia-Tropical",0},
            { "Eurasia  Asia-Temperate",0 },
{"Aegaean Isl",0},
 {"Weddell Sea",0},
{"Ross Sea",0},
           { "Lesser Antilles",0 },
            { "South Australian Gulfs",0 },
            { "Great Austrlian Bight",0 },
            { "Rhodos",0 },
            { "Scotia Sea",0 },
            { "Balearic Islands",0 },
            { "Caspian Sea",0 },
            { "Greater Antilles",0 },
            { "Juan Fernández Islands",0 },
            { "Karakoram",0 },
            { "Yucatan",0 },
            { "New Britain",0 },
            { "New Hebrides",0 },
            { "New Ireland (island)",0 },
            { "Nias",0 },
            { "Patagonia",0 },
            { "South India and Sri Lanka",0 },
            { "Madras",0 },
            { "La Paz",0 },
            { "Antioquia Department",0 },
            { "Bolívar (state)",0 },
            { "Paraná (state)",0 },
            { "Goias",0 },
            { "Pará",0 },
            { "Valle del Cauca Department",0 },
            { "Chocó Department",0 },
            { "Cauca Department",0 },
            { "Nariño Department",0 },
            { "Santander Department",0 },
{ "Arauca Department",0 },
{ "Atlántico Department",0 },
{ "Boyacá Department",0 },
{ "Caldas Department",0 },
{ "Caquet Department",0 },
{ "Casanare Department",0 },
{ "Cesar Department",0 },
{ "Córdoba Department",0 },
{ "Cundinamarca Department",0 },
{ "Guainía Department",0 },
{ "Guaviare Department",0 },
{ "Huila Department",0 },
{ "La Guajira Department",0 },
{ "Magdalena Department",0 },
{ "Meta Department",0 },
{ "Norte de Santander Department",0 },
{ "Putumayo Department",0 },
{ "Quindío Department",0 },
{ "Risaralda Department",0 },
{ "Archipelago of San Andrés, Providencia and Santa Catalina",0 },
//{ "Sucre Department",0 },
{ "Tolima Department",0 },
{ "Vaupés Department",0 },
{ "Vichada Department",0 },
            { "Aragua",0 },
            { "Yaracuy",0 },
            { "Delta Amacuro",0 },
            { "Nueva Esparta",0 },
            { "Zulia",0 },
{ "Mérida (state)",0 },
{ "Táchira (state)",0 },
{ "Trujillo (state)",0 },
{ "Vargas (state)",0 },
{ "Miranda (state)",0 },
{ "Carabobo (state)",0 },
{ "Falcón (state)",0 },
{ "Lara (state)",0 },
{ "Yaracuy (state)",0 },
{ "Apure (state)",0 },
{ "Barinas (state)",0 },
{ "Cojedes (state)",0 },
{ "Guárico (state)",0 },
{ "Portuguesa (state)",0 },
{ "Anzoátegui (state)",0 },
{ "Monagas (state)",0 },
            { "Rondonia",0 },
            { "Sumba",0 },
            { "Sumbawa",0 },
            { "Darjeeling district",0 },
            { "Mpulamanga",0 },
            { "Limpopo",0 },
{ "Chuquisaca Department",0 },
{ "Tarija Department",0 },
{ "Cochabamba Department",0 },
{ "Oruro Department",0 },
{ "Pando Department",0 },
{ "Potosí Department",0 },
            { "Beni Department",0 },
{ "Bolívar Department",0 },
            //{ "",0 },
            //{ "",0 },
            //{ "",0 },
            //{ "",0 },
            //{ "",0 },
            { "",0 }
            };

            //if (util.localitydict.Count == 0)
            //{
            //    foreach (Distribution dd in db.Distribution)
            //    {
            //        string loc = dd.LocationID.Replace("TDWG:", "");
            //        if (!util.localitydict.ContainsKey(loc))
            //            util.localitydict.Add(loc, dd.Locality);
            //        if (loc.Contains("-"))
            //        {
            //            loc = loc.Replace("-", "");
            //            if (!util.localitydict.ContainsKey(loc))
            //                util.localitydict.Add(loc, dd.Locality);
            //        }
            //    }
            //}

            //util.statedict = util.getstatedict();
            //foreach (string s in util.statedict.Keys)
            //{
            //    if (!diststat.ContainsKey(util.statedict[s]))
            //        diststat.Add(util.statedict[s], 0);
            //}
            //countryclass.read_country_info();
            //foreach (countryclass c in countryclass.countrylist)
            //{
            //    if (!diststat.ContainsKey(c.Name))
            //        diststat.Add(c.Name, 0);
            //}
            //Dictionary<string, int> descstat = new Dictionary<string, int>();
            //Dictionary<string, int> provincestat = new Dictionary<string, int>();
            //Dictionary<string, int> occstat = new Dictionary<string, int>();
            //Dictionary<string, int> establstat = new Dictionary<string, int>();
            ////List<string> compasslist = util.getcompasslist();
            //Dictionary<string, string> aliasdict = util.getcountryalias();

            //int n = 0;
            //foreach (Distribution dd in db.Distribution)
            //{
            //    n++;
            //    if (n % 100000 == 0)
            //        memo(n.ToString());
            //    if (!taxonstat.ContainsKey(dd.TaxonID))
            //        taxonstat.Add(dd.TaxonID, 1);
            //    else
            //        taxonstat[dd.TaxonID]++;
            //    if (!diststat.ContainsKey(dd.Locality))
            //        diststat.Add(dd.Locality, 1);
            //    else
            //        diststat[dd.Locality]++;
            //    string occ = dd.OccurrenceStatus;
            //    if (string.IsNullOrEmpty(occ))
            //        occ = "NULL";
            //    if (!occstat.ContainsKey(occ))
            //        occstat.Add(occ, 1);
            //    else
            //        occstat[occ]++;
            //    string est = dd.EstablishmentMeans;
            //    if (String.IsNullOrEmpty(est))
            //        est = "NULL";
            //    if (!establstat.ContainsKey(est))
            //        establstat.Add(est, 1);
            //    else
            //        establstat[est]++;

            //}

            //memo("Diststat: " + diststat.Count());
            ////foreach (string s in diststat.Keys)
            ////    memo(s + "\t" + diststat[s]);
            ////memo("\n=====\noccstat "+occstat.Count()+"\n============");
            ////foreach (string s in occstat.Keys)
            ////    memo(s + "\t" + occstat[s]);
            ////memo("\n=====\nestablstat "+establstat.Count()+"\n============");
            ////foreach (string s in establstat.Keys)
            ////    memo(s + "\t" + establstat[s]);
            //memo("\n====\ntaxonstat" + taxonstat.Count());

            //memo("\n\n================================================================\nDescription");

            //Regex rb = new Regex(@"\(\#\#(\d\d\d)\)");
            //Regex ryear = new Regex(@"\d{4}");
            //Regex rnot = new Regex(@"\(not .+?\)");

            //int nprint = 101;
            //n = 0;
            //foreach (Description dd in db.Description)
            //{
            //    n++;
            //    if (n % 100000 == 0)
            //        memo(n.ToString());

            //    string desc = dd.Description1;

            //    desc = desc.Replace(" (I)", "").Replace("(I)", ""); //remove (I) that bothers the parser
            //    MatchCollection mnot = rnot.Matches(desc); //remove "(not XXX)"
            //    foreach (Match m in mnot)
            //        desc = desc.Replace(m.Value, "");
            //    desc = desc.Replace(", (", " (");
            //    desc = desc.Replace("; (", " (");

            //    string[] brackets = util.findtextinbrackets(desc, "(", ")");
            //    if (brackets.Length > 0)
            //    {
            //        if (desc.Length > 100 && nprint < 100)
            //            memo(desc);
            //        for (int i = 0; i < brackets.Length; i++)
            //        {
            //            desc = desc.Replace("(" + brackets[i] + ")", "(##" + i.ToString("D3") + ")");
            //        }
            //        if (desc.Length > 100 && nprint < 100)
            //        {
            //            memo(desc);
            //            nprint++;
            //        }
            //    }
            //    int nfound = 0;
            //    if (diststat.ContainsKey(desc))
            //    {
            //        diststat[desc]++;
            //        nfound++;
            //    }
            //    else
            //    {
            //        string[] dw;
            //        if (desc.Contains(";"))
            //        {
            //            dw = desc.Split(';');
            //            int maxlength = -1;
            //            foreach (string s in dw)
            //                if (s.Length > maxlength)
            //                    maxlength = s.Length;
            //            if (maxlength > 50)
            //                dw = desc.Split(new char[] { ';', ',' });
            //        }
            //        else if (desc.Contains(","))
            //            dw = desc.Split(',');
            //        else if ((desc.ToUpper() == desc) && (brackets.Length == 0))
            //            dw = desc.Split(' ');
            //        else
            //            dw = new string[] { desc };
            //        foreach (string word in dw)
            //        {
            //            string w = word.Trim();
            //            //if (w.Contains(":"))
            //            //{
            //            //    string[] wcol = w.Split(':');
            //            //    if (diststat.ContainsKey(wcol[1].Trim(new char[] { ' ', '.' })))
            //            //    {
            //            //        w = wcol[1].Trim(new char[] { ' ', '.' });
            //            //    }
            //            //    else if (diststat.ContainsKey(wcol[0].Trim(new char[] { ' ', '.' })))
            //            //    {
            //            //        w = wcol[0].Trim(new char[] { ' ', '.' });
            //            //    }

            //            //}
            //            if (diststat.ContainsKey(w))
            //            {
            //                diststat[w]++;
            //                nfound++;
            //            }
            //            else if (util.localitydict.ContainsKey(w))
            //            {
            //                diststat[util.localitydict[w]]++;
            //                nfound++;
            //            }
            //            else
            //            {
            //                //find parentheses
            //                List<int> bracketlist = new List<int>();
            //                MatchCollection mb = rb.Matches(w);
            //                foreach (Match m in mb)
            //                {
            //                    bracketlist.Add(Convert.ToInt32(m.Groups[1].Value));
            //                    w = w.Replace(m.Value, "").Trim();
            //                }
            //                if (diststat.ContainsKey(w))
            //                {
            //                    diststat[w]++;
            //                    nfound++;
            //                }
            //                else
            //                {
            //                    w = util.compassaliascheck(w, diststat);

            //                    if (diststat.ContainsKey(w))
            //                    {
            //                        diststat[w]++;
            //                        nfound++;
            //                    }
            //                    else
            //                    {
            //                        string ww = w.Trim(new char[] { ' ', '.' });
            //                        ww = util.compassaliascheck(ww, diststat);
            //                        if (diststat.ContainsKey(ww))
            //                        {
            //                            diststat[ww]++;
            //                            nfound++;
            //                        }
            //                        else
            //                        {
            //                            if (ww.Contains(","))
            //                            {
            //                                foreach (string ss in ww.Split(','))
            //                                {
            //                                    string sss = ss.Trim();
            //                                    if (diststat.ContainsKey(sss))
            //                                    {
            //                                        diststat[sss]++;
            //                                        nfound++;
            //                                    }
            //                                    else
            //                                    {
            //                                        sss = util.compassaliascheck(sss, diststat);
            //                                        if (diststat.ContainsKey(sss))
            //                                        {
            //                                            diststat[sss]++;
            //                                            nfound++;
            //                                        }
            //                                        else if (sss.ToUpper() == sss)
            //                                        {
            //                                            sss = countryclass.getcountry(sss);
            //                                            if (diststat.ContainsKey(sss))
            //                                            {
            //                                                diststat[sss]++;
            //                                                nfound++;
            //                                            }
            //                                            else
            //                                            {

            //                                                if (!descstat.ContainsKey(sss))
            //                                                    descstat.Add(sss, 1);
            //                                                else
            //                                                    descstat[sss]++;
            //                                            }

            //                                        }
            //                                        else
            //                                        {

            //                                            if (!descstat.ContainsKey(sss))
            //                                                descstat.Add(sss, 1);
            //                                            else
            //                                                descstat[sss]++;
            //                                        }
            //                                    }

            //                                }
            //                            }
            //                            else if (ww.ToUpper() == ww)
            //                            {
            //                                if (ww.EndsWith("OO"))
            //                                {
            //                                    ww = ww.Replace("OO", "").Trim('-');
            //                                }
            //                                ww = countryclass.getcountry(ww);
            //                                if (diststat.ContainsKey(ww))
            //                                {
            //                                    diststat[ww]++;
            //                                    nfound++;
            //                                }
            //                                else
            //                                {

            //                                    if (!descstat.ContainsKey(ww))
            //                                        descstat.Add(ww, 1);
            //                                    else
            //                                        descstat[ww]++;
            //                                }

            //                            }
            //                            else
            //                            {
            //                                if (!descstat.ContainsKey(ww))
            //                                    descstat.Add(ww, 1);
            //                                else
            //                                    descstat[ww]++;
            //                            }
            //                        }


            //                    }
            //                }
            //                foreach (int ib in bracketlist)
            //                {
            //                    MatchCollection matches = ryear.Matches(brackets[ib]);
            //                    if (matches.Count == 0) //skip brackets with year
            //                    {
            //                        string[] provinces = brackets[ib].Split(new char[] { ';', ',' });
            //                        if ((provinces.Length == 1) && (brackets[ib].ToUpper() == brackets[ib]))
            //                        {
            //                            provinces = brackets[ib].Split();
            //                        }
            //                        foreach (string pword in provinces)
            //                        {
            //                            string pp = pword.Trim();
            //                            //if (pp.Contains(":"))
            //                            //{
            //                            //    string[] wcol = pp.Split(':');
            //                            //    if (diststat.ContainsKey(wcol[1].Trim(new char[] { ' ', '.' })))
            //                            //    {
            //                            //        pp = wcol[1].Trim(new char[] { ' ', '.' });
            //                            //    }
            //                            //    else if (diststat.ContainsKey(wcol[0].Trim(new char[] { ' ', '.' })))
            //                            //    {
            //                            //        pp = wcol[0].Trim(new char[] { ' ', '.' });
            //                            //    }

            //                            //}
            //                            if (diststat.ContainsKey(pp))
            //                            {
            //                                diststat[pp]++;
            //                                nfound++;
            //                            }
            //                            else if (util.localitydict.ContainsKey(w))
            //                            {
            //                                diststat[util.localitydict[w]]++;
            //                                nfound++;
            //                            }
            //                            else
            //                            {
            //                                pp = util.compassaliascheck(pp, diststat);

            //                                if (diststat.ContainsKey(pp))
            //                                {
            //                                    diststat[pp]++;
            //                                    nfound++;
            //                                }
            //                                else
            //                                {
            //                                    string ww = pp.Trim(new char[] { ' ', '.' });
            //                                    if (diststat.ContainsKey(ww))
            //                                    {
            //                                        diststat[ww]++;
            //                                        nfound++;
            //                                    }
            //                                    else if (w.Contains("USA") || w.Contains("Canada") || w.Contains("Australia") || w.Contains("United States") || w.Contains("Europe"))
            //                                    {
            //                                        if (w.Contains("Australia"))
            //                                            ww = util.getstate(ww + "-au");
            //                                        else if (w.Contains("Europe"))
            //                                            ww = countryclass.getcountry(ww);
            //                                        else
            //                                            ww = util.getstate(ww);
            //                                        if (diststat.ContainsKey(ww))
            //                                        {
            //                                            diststat[ww]++;
            //                                            nfound++;
            //                                        }
            //                                        else
            //                                        {
            //                                            string ppp = pp + "!!" + w;
            //                                            if (!provincestat.ContainsKey(ppp))
            //                                                provincestat.Add(ppp, 1);
            //                                            else
            //                                                provincestat[ppp]++;
            //                                            if (w.Length > 200)
            //                                                memo("!!" + dd.Description1);
            //                                        }
            //                                    }
            //                                    else
            //                                    {
            //                                        string ppp = pp + "!" + w;
            //                                        if (!provincestat.ContainsKey(ppp))
            //                                            provincestat.Add(ppp, 1);
            //                                        else
            //                                            provincestat[ppp]++;
            //                                        if (nprint < 200 && String.IsNullOrEmpty(w))
            //                                        {
            //                                            memo(dd.Description1 + "!" + pp);
            //                                            nprint++;
            //                                        }
            //                                    }

            //                                }
            //                            }
            //                        }
            //                    }

            //                }
            //            }
            //        }
            //    }
            //    if (nfound > 0)
            //    {
            //        if (!taxonstat.ContainsKey(dd.TaxonID))
            //            taxonstat.Add(dd.TaxonID, nfound);
            //        else
            //            taxonstat[dd.TaxonID] += nfound;
            //    }
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

            ////memo("\n\n================================================================");

            ////foreach (string state in util.statestat.Keys)
            ////    memo(state + "\t" + util.statestat[state]);

            ////memo("\n====\ntaxonstat" + taxonstat.Count());

            ////memo("\n\n================================================================");
            ////foreach (string state in util.countrystat.Keys)
            ////    memo(state + "\t" + util.countrystat[state]);

            ////memo("\n====\ntaxonstat" + taxonstat.Count());

            //memo("\n\n================================================================");

        }

        private Dictionary<string, int> parsestat = new Dictionary<string, int>();

        private void Parsebutton_Click(object sender, EventArgs e)
        {
            if (distributionclass.maindistlist.Count == 0)
            {
                distributionclass.read_distributionlinks(db);
                memo("maindistlist: " + distributionclass.maindistlist.Count);
            }

            int maxspecies = 9999999;
            int n = 0;

            parsestat.Add("no data", 0);
            parsestat.Add("failed parse", 0);
            parsestat.Add("partial parse", 0);
            parsestat.Add("almost full parse", 0);
            parsestat.Add("full parse", 0);

            memo("test parsing for " + maxspecies);

            int offset = 3; //offset = 1 -> Virginia bug, 2 => Durango, Sao Paulo etc
            var qtt = (from c in db.Taxon where c.TaxonID % 1000 == offset select c);
            //var qtt = (from c in db.Taxon where c.TaxonID == 8739002 || c.TaxonID == 8700002 select c);
            foreach (Taxon tt in qtt)
            {
                n++;
                if (n > maxspecies)
                    break;
                if (n % 10 == 0)
                    memo("n = " + n);
                if (tt.TaxonomicStatus != "accepted name")
                {
                    //memo(tt.ScientificName + " " + tt.TaxonomicStatus);
                    continue;
                }
                List<distributionclass> dcl = distributionclass.getdistribution(tt.TaxonID, db);
                //var q = from c in db.Description where c.TaxonID == tt.TaxonID select c;
                //if (q.Count() > 0)
                //{
                //    memo(tt.ScientificName + " q:" + q.Count());
                //    foreach (Description dd in q)
                //    {
                //        memo("description: " + dd.Description1);
                //        List<distributionclass> dcl = distributionclass.parsedescription(dd);
                int ndcl = 2;
                if (dcl.Count > ndcl)
                {
                    memo("parses " + dcl.Count+" taxonid "+tt.TaxonID);
                }
                int nfail = 0;
                int nsuccess = 0;
                foreach (distributionclass dc in dcl)
                {
                    if (dcl.Count > ndcl)
                        memo(dc.towikistring("*"));
                    if (dc.maindist == distributionclass.failstring)
                        nfail++;
                    else
                        nsuccess++;
                    foreach (distributionclass dcsub in dc.subdist)
                    {
                        if (dcsub.maindist == distributionclass.failstring)
                            nfail++;
                        else
                            nsuccess++;
                    }

                }
                if (dcl.Count == 0)
                    parsestat["no data"]++;
                else if (nsuccess == 0)
                    parsestat["failed parse"]++;
                else if (nfail == 1 && nsuccess > 2)
                    parsestat["almost full parse"]++;
                else if (nfail > 0)
                    parsestat["partial parse"]++;
                else
                    parsestat["full parse"]++;

                
                //    }
                //}
            }

            memo("\n\n================================================================\nFailures");
            memo("Descstat: " + distributionclass.descstat.Count());
            n = 0;
            foreach (string s in distributionclass.descstat.Keys)
            {
                n++;
                if (distributionclass.descstat[s] > 0)
                    memo(s + "\t" + distributionclass.descstat[s]);
            }

            foreach (string s in parsestat.Keys)
            {
                memo(s + ": " + parsestat[s]);
            }
        }
    }
}
