using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeSpecies
{
    class Treenodeclass
    {
        public string name = "";
        public string rank = "";
        public Treenodeclass parent;
        public List<Treenodeclass> children;
        public int? taxonID = null; //COL
        public string article = null; //Wikipedia
        public string cebname = null;

        public Treenodeclass(Taxon tt, Treenodeclass parentpar)
        {
            this.name = tt.ScientificName;
            this.taxonID = tt.TaxonID;
            this.rank = tt.TaxonRank;
            this.parent = parentpar;

        }
    }
}
