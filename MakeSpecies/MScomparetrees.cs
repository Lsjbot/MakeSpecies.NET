using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetWikiBot;
using Microsoft.VisualBasic;

namespace MakeSpecies
{
    public partial class MScomparetrees : Form
    {
        COL2019 db;
        Site site;
        //bool loggedin = false;
        public MScomparetrees(COL2019 dbpar)
        {
            InitializeComponent();
            db = dbpar;
            LBwiki.Items.Add("ceb");
            LBwiki.Items.Add("sv");
            LBwiki.Items.Add("diq");
            LBwiki.SelectedItem = "ceb";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void memo(string s)
        {
            richTextBox1.AppendText(s + "\n");
            richTextBox1.ScrollToCaret();
        }

        private void listdbtree(Taxon tt, string prefix)
        {
            memo(prefix + tt.ScientificName);
            var q = from c in db.Taxon
                    where c.ParentNameUsageID == tt.TaxonID
                    select c;
            foreach (Taxon tc in q.ToList())
            {
                listdbtree(tc, prefix + "  ");
            }
        }


        private void COLbutton_Click(object sender, EventArgs e)
        {
            string topname = TBtaxon.Text;

            var q = from c in db.Taxon
                        where c.ScientificName == topname
                        select c;

            if (q.Count() == 0)
                memo(topname + " not found in database.");
            else if (q.Count() > 1)
                memo(topname + " found "+q.Count()+" times in database.");
            else
            {
                Taxon tt = q.First();
                listdbtree(tt,"");
            }


        }

        public void listwikitree(string cat,string prefix)
        {
            memo(prefix + cat);
            PageList pl = new PageList(site);
            pl.FillAllFromCategory(cat);
            foreach (Page p in pl)
            {
                if (p.title.Contains(":"))
                    listwikitree(p.title.Split(':')[1], "   " + prefix);
                else
                    memo(prefix + "   " + p.title);
            }

        }

        public void login()
        {
            string makelang = LBwiki.SelectedItem.ToString();
            //Console.Write("Password: ");
            string password = util.get_password();
            string botkonto = "Lsjbot";
            site = new Site("https://" + makelang + ".wikipedia.org", botkonto, password);
            site.defaultEditComment = "Fixing mistake";
            site.minorEditByDefault = true;
            MakeSpecies.loggedin = true;

        }

        private void Wikibutton_Click(object sender, EventArgs e)
        {
            if (!MakeSpecies.loggedin)
                login();

            string topname = TBtaxon.Text;

            listwikitree(topname, "");


        }

        private void add_db_kids(Treenodeclass papa,Taxon ttpapa)
        {
            var q = from c in db.Taxon where c.ParentNameUsageID == papa.taxonID select c;
            foreach (Taxon tt in q)
            {
                Treenodeclass tn = new Treenodeclass(tt, papa);
                add_db_kids(tn, tt);
            }
        }

        private void loadtreebutton_Click(object sender, EventArgs e)
        {
            if (!MakeSpecies.loggedin)
                login();

            var q = from c in db.Taxon
                    where c.ScientificName == TBtaxon.Text
                    select c;

            if (q.Count() == 0)
                memo(TBtaxon.Text + " not found in database.");
            else if (q.Count() > 1)
                memo(TBtaxon.Text + " found " + q.Count() + " times in database.");
            else
            {
                Taxon tt = q.First();
                Treenodeclass root = new Treenodeclass(tt,null);
                root.name = TBtaxon.Text;
                root.taxonID = tt.TaxonID;
                root.rank = tt.TaxonRank;
                root.parent = null;

                add_db_kids(root,tt);
                //listdbtree(tt, "");
            }

        }
    }
}
