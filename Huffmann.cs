using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LectureImage
{
    public class Huffmann
    {
        private Noeud root;
        private List<Noeud> freq_pixel;

        /// <summary>
        /// Propriété du noeuds situé à la racine
        /// </summary>
        public Noeud Root
        {
            get { return root; }
            set { root = value; }
        }

        /// <summary>
        /// Propriété des noeuds et leur fréquences
        /// </summary>
        public List<Noeud> Frequencies
        {
            get { return freq_pixel; }
            set { freq_pixel = value; }
        }

        /// <summary>
        /// Constructeur de l'arbre avec sa racine et ses branches
        /// </summary>
        /// <param name="freq_pixel">Arbre sous forme de liste de noeuds</param>
        public Huffmann(List<Noeud> freq_pixel)
        {
            this.freq_pixel = freq_pixel;
            this.root = transfo_arbre(freq_pixel);
        }

        /// <summary>
        /// tri les sous arbres par ordre croissante de fréquence
        /// </summary>
        /// <param name="freq_pixel">Liste de des noeuds</param>
        /// <returns>Rliste des sous arbres triés</returns>
        public List<Noeud> tri_freq(List<Noeud> freq_pix)
        {
            int i = freq_pix.Count - 1;
            Noeud piv;
            while (i >= 1&&freq_pix[i].Freq < freq_pix[i - 1].Freq)
            {
                piv = freq_pix[i];
                freq_pix[i]= freq_pix[i - 1];
                freq_pix[i - 1] = piv;
                i--;
            }
            return freq_pix;
        }

        /// <summary>
        /// Creaction de l'arbre d'huffmann
        /// </summary>
        /// <param name="freq_pixel">Liste de des noeuds</param>
        /// <returns>retourne l'arbre d'huffmann</returns>
        public Noeud transfo_arbre(List<Noeud> freq_pixel)
        {
            freq_pixel = tri_freq(freq_pixel);
            Noeud piv;
            while (freq_pixel.Count != 1)
            {
                piv = new Noeud(null, freq_pixel[0].Freq + freq_pixel[1].Freq, freq_pixel[0], freq_pixel[1]);
                freq_pixel.RemoveRange(0, 2);
                freq_pixel.Add(piv);
                freq_pixel = tri_freq(freq_pixel);
            }
            return freq_pixel[0];
        }

        /// <summary>
        /// parcours de l'arbre d'huffmann
        /// </summary>
        /// <param name="pixel">pixel à placer</param>
        /// <param name="pts">noeud de l'arbre</param>
        /// <param name="liste_bits">taille de l'arbre</param>
        /// <returns>retournele noeud dans lequel se trouve le pixel recherché</returns>
        public Noeud trouver_chemin_pixel(Pixel pixel,int freq_pixel,Noeud pts)
        {
            if (pts == null)
            {
                return null;
                pts.Chemin.Clear();
            }
            else if (pixel == pts.Pix)
            {
                return pts;
            }
            else if (pts.Freq < this.freq_pixel[freq_pixel].Freq)
            {
                pts.Chemin.Add(1);
                return trouver_chemin_pixel(pixel, freq_pixel,pts.Gauche);
            }
            else
            {
                pts.Chemin.Add(0);
                return trouver_chemin_pixel(pixel, freq_pixel, pts.Droit);
            }
        }
    }
}