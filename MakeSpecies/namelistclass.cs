using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeSpecies
{
    class namelistclass
    {
        public string triv = "";
        public string group = "";
        public string cebspec = "";
        public bool isspecies = false;

        public override string ToString()
        {
            return triv + "\t" + group + "\t" + cebspec+"\t"+isspecies;
        }
    }
}
