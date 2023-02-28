using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetWikiBot;

namespace MakeSpecies
{
    public class Taxonclass
    {
        public int taxonid; //index to various other list; COL id#; if no COLid, then dyntaxaid + 20 million
        public int taxonid13 = -1;
        public int dyntaxaid = -1; //taxon id in Dyntaxa; -1 if none
        public int dyntaxa2 = -1; //index to taxondict entry for second (3rd etc) Dyntaxa taxon
        public int iucnid = -1;
        public int redfiid = -1;
        public int redsvid = -1;
        public string Name = ""; //scientific name
        public string Name_sv = ""; //Swedish name
        public int Level = -1; //taxonomic level, index to rank_name
        public int Parent = -1; //index of parent in COL; -1 if top taxon; -2 if taxon only in dyntaxa; -3 if taxon is double dyntaxa entry.
        public int dyntaxa_Parent = -1; //index of parent in dyntaxa
        public int Parent11 = -1;
        public int Parent13 = -1;
        public string Parentname = "";
        public List<int> Children = new List<int>(); //index of children
        public List<int> Children13 = new List<int>(); //index of children
        public List<int> dyntaxa_Children = new List<int>(); //index of children
        public string dyntaxa_auktor = "";
        public string dyntaxa_name = "";
        public string iucn_auktor = "";
        public string iucn_name = "";
        public string redfi_name = "";
        public string redsv_name = "";
        public List<int> habitats = new List<int>();
        public List<int> synonyms = new List<int>();
        public string regnum;
        public string articlename = "";
        public string status = "";

        public Dictionary<string, string> parentdict = new Dictionary<string, string>();

        public string auktor;
        public string source_dataset;

        public Taxonclass(Taxon tt, COL2019 db, string makelang)
        {
            this.taxonid = tt.TaxonID;
            string rank = tt.TaxonRank;
            if (MakeSpecies.name_rank.ContainsKey(rank))
                this.Level = MakeSpecies.name_rank[rank];
            else if (MakeSpecies.rank_eng_latin.ContainsKey(rank))
            {
                rank = MakeSpecies.rank_eng_latin[rank];
                if (MakeSpecies.name_rank.ContainsKey(rank))
                    this.Level = MakeSpecies.name_rank[rank];
                else
                {
                    //memo("Bad rank " + tt.TaxonRank);
                    //return null;
                    this.Level = -1;
                }
            }
            else
            {
                //memo("Bad rank " + tt.TaxonRank);
                //return null;
                this.Level = -2;
            }
            //if (subspecific.Contains(this.Level))
            //{
            //    memo("subsepcific taxon");
            //    return null;
            //}
            if (tt.TaxonomicStatus != null)// && tt.TaxonomicStatus.Trim() != "accepted name")
            {
                //memo("Status = " + tt.TaxonomicStatus);
                //return null;
                this.status = tt.TaxonomicStatus;
            }

            this.Name = util.dbtaxon_name(tt);

            if (tt.ParentNameUsageID != null)
                this.Parent = (int)tt.ParentNameUsageID;
            if (!MakeSpecies.taxondict.ContainsKey(this.Parent))
            {
                Taxonclass tcp = loadtaxon(this.Parent,db,makelang);
                if (tcp != null)
                    MakeSpecies.taxondictadd(this.Parent, tcp);
            }
            if (MakeSpecies.taxondict.ContainsKey(this.Parent))
                this.Parentname = MakeSpecies.taxondict[this.Parent].Name;

            this.regnum = tt.Kingdom;

            this.parentdict.Add("regnum", tt.Kingdom);
            //if ((tt.Kingdom == "Plantae") || (tt.Kingdom == "Fungi"))
                this.parentdict.Add("divisio", tt.Phylum);
            //else
                this.parentdict.Add("phylum", tt.Phylum);
            this.parentdict.Add("classis", tt.Class);
            this.parentdict.Add("ordo", tt.Order);
            this.parentdict.Add("superfamilia", tt.Superfamily);
            this.parentdict.Add("familia", tt.Family);
            this.parentdict.Add("genus", tt.Genus);

            var q = (from c in db.Taxon
                    where c.ParentNameUsageID == tt.TaxonID
                    select c).OrderBy(c=>c.ScientificName);
            foreach (Taxon kid in q)
                this.Children.Add(kid.TaxonID);

            this.auktor = tt.ScientificNameAuthorship;
            this.source_dataset = tt.DatasetName;
            if (this.source_dataset.Contains("in Species 2000"))
                this.source_dataset = this.source_dataset.Replace(" in Species 2000 & ITIS Catalogue of Life: 2019", "").Trim();

            if (MakeSpecies.swe_name.ContainsKey(this.taxonid))
                this.Name_sv = MakeSpecies.swe_name[this.taxonid];

            var qa = from c in db.Artnametable
                    where c.Taxonid == this.taxonid
                    where c.Lang == makelang
                    select c;
            if (qa.Count() == 1)
                this.articlename = qa.First().Articlename;
            
        }

        public Taxonclass(Page p, COL2019 db, string makelang)
        {
            string[] ranks = { "genus", "familia", "superfamilia", "ordo", "classis", "phylum", "divisio", "regnum" };
            string tbox = util.whichtaxobox(p);
            foreach (string r in ranks)
            {
                this.parentdict.Add(r, p.GetFirstTemplateParameter(tbox, r).Trim(new char[] { ' ', '[', ']', '\'' }));
            }
            string rank = "species";
            int irank = 0;
            for (int i=0;i<ranks.Length;i++)
                if (!String.IsNullOrEmpty(this.parentdict[ranks[i]]))
                {
                    rank = ranks[i];
                    irank = i;
                    break;
                }
            this.Level = MakeSpecies.name_rank[rank];
            this.Name = p.GetFirstTemplateParameter(tbox, "binomial").Trim(new char[]{ ' ','\''});
            if (this.Name.Split().Length > 1)
                rank = "species";
            string parent = "";
            if (rank == "species")
                parent = this.parentdict["genus"];
            else
                parent = this.parentdict[ranks[irank + 1]];
            this.taxonid = MakeSpecies.get_name_id(this.Name, rank, parent,db);
            this.regnum = this.parentdict["regnum"];

            if (MakeSpecies.swe_name.ContainsKey(this.taxonid))
                this.Name_sv = MakeSpecies.swe_name[this.taxonid];

            string an = (from c in db.Artnametable
                         where c.Taxonid == this.taxonid
                         where c.Lang == makelang
                         select c.Articlename).FirstOrDefault();
            if (!String.IsNullOrEmpty(an))
                this.articlename = an;

        }

        public string getnamedgroup()
        {
            string rank = MakeSpecies.rank_name[this.Level];
            if (rank == "regnum")
                return this.Name;
            do
            {
                rank = MakeSpecies.rank_above[rank];
                if (rank == "regnum")
                    return this.regnum;
            }
            while (!MakeSpecies.groupname_sing.ContainsKey(this.parentdict[rank]));
            return this.parentdict[rank];
        }

        public void update_artnametable(COL2019 db,string makelang)
        {
            if (String.IsNullOrEmpty(this.articlename))
                return;

            var q = from c in db.Artnametable where c.Taxonid == this.taxonid where c.Lang == makelang select c;
            if (q.Count() > 0)
            {
                Artnametable an = q.First();
                an.Articlename = this.articlename;
                db.SubmitChanges();
            }
            else
            {
                Artnametable an = new Artnametable();
                an.Id = (from c in db.Artnametable select c.Id).Max() + 1;
                an.Lang = makelang;
                an.Taxonid = this.taxonid;
                an.Articlename = this.articlename;
                db.Artnametable.InsertOnSubmit(an);
                db.SubmitChanges();
            }
        }

        public static Taxonclass loadtaxon(int taxonid, COL2019 db, string makelang)
        {
            var q = from c in db.Taxon where c.TaxonID == taxonid select c;
            if (q.Count() == 0)
            {
                //memo("Taxon " + taxonid + " not in db.");
                return null;
            }
            else if (q.Count() == 1)
            {
                Taxon tt = q.First();
                return new Taxonclass(tt,db, makelang);
            }
            else //double
            {
                //memo("Double db entry for taxon " + taxonid);
                return null;
            }
        }



    }
}
