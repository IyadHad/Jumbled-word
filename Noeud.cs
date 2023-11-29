using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LectureImage
{
    public class Noeud
    {
        private Pixel pix;
        private int freq;
        List<int> chemin;
        private Noeud droit;
        private Noeud gauche;


        /// <summary>
        /// Propriété Chemin
        /// </summary>
        public List<int> Chemin
        {
            get{ return chemin; }
            set { chemin = value; }
        }

        /// <summary>
        /// Propriété pixel
        /// </summary>
        public Pixel Pix
        {
            get { return pix; } 
            set { pix = value; }
        }

        /// <summary>
        /// Propriété fréquence
        /// </summary>
        public int Freq
        {
            get { return freq; }
            set { freq = value; }
        }

        /// <summary>
        /// Propriété du noeud droit
        /// </summary>
        public Noeud Droit
        {
            get { return droit; }
            set { droit = value; }
        }

        /// <summary>
        /// Propriété du noeud gauche
        /// </summary>
        public Noeud Gauche
        {
            get { return gauche; }
            set { gauche = value; }
        }

        /// <summary>
        /// Cosntructeur du noeud
        /// </summary>
        /// <param name="p">pixel</param>
        /// <param name="Freq">fréquence</param>
        /// <param name="Droit">sous-noeud droit</param>
        /// <param name="Gauche">sous-noeud gauche</param>
        public Noeud(Pixel p, int Freq, Noeud Droit, Noeud Gauche)
        {
            this.pix = p;
            this.freq = Freq;
            this.droit = Droit;
            this.gauche = Gauche;
            this.chemin = new List<int>();
        }


        
    }
}
