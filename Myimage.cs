using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace LectureImage
{
    public class Myimage
    {
        private Pixel[,] image;
        private string type_fichier;
        private int tailleF;
        private int tailleOf;
        private int largeur;
        private int hauteur;
        private int nbBits;

        /// <summary>
        /// Propriété de la matrice de pixels de l'image
        /// </summary>
        public Pixel[,] Image
        {
            get { return this.image; }
            set { this.image = value; }
        }

        /// <summary>
        /// Constructeur de la matrice de pixels de l'image
        /// </summary>
        /// <param name="tab">toutes les valeurs à rentrer</param>
        /// <param name="longueur">longueur de l'image</param>
        /// <param name="hauteur">hauteur de l'image</param>
        /// <returns>matrice de l'image créée</returns>
        static Pixel[,] matPixel(byte[] tab, int longueur, int hauteur)
        {
            Pixel[,] res = new Pixel[hauteur, longueur];
            int k = 0;
            int z = 0;
            for (int i = 54; i < tab.Length; i = i + longueur * 3)
            {
                for (int j = i; j < i + longueur * 3; j = j + 3)
                {
                    res[k, z] = new Pixel(tab[j + 1], tab[j + 2], tab[j]);
                    z = z + 1;
                }
                k++;
                z = 0;
            }
            return res;
        }

        /// <summary>
        /// Retranscrire l'image venant d'un fichier extérieur
        /// </summary>
        /// <param name="myfile">image venant de l'extérieur</param>
        public Myimage(string myfile)
        {
            this.type_fichier = "BM";
            byte[] Rfile = File.ReadAllBytes(myfile);
            byte[] tabEnd = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                tabEnd[i] = Rfile[i + 2];

            }
            this.tailleF = Convertir_Endian_To_Int(tabEnd);
            for (int i = 0; i < 4; i++)
            {
                tabEnd[i] = Rfile[i + 14];
            }
            this.tailleOf = Convertir_Endian_To_Int(tabEnd);
            for (int i = 0; i < 4; i++)
            {
                tabEnd[i] = Rfile[i + 18];
            }
            this.largeur = Convertir_Endian_To_Int(tabEnd);
            for (int i = 0; i < 4; i++)
            {
                tabEnd[i] = Rfile[i + 22];
            }
            this.hauteur = Convertir_Endian_To_Int(tabEnd);
            while(this.largeur%4!=0)
            {
                this.largeur++;
            }
            byte[] tab2O = new byte[2];
            for (int i = 0; i < 2; i++)
            {
                tab2O[i] = Rfile[i + 26];
            }
            this.nbBits = Convertir_Endian_To_Int(tab2O);
            this.image = matPixel(Rfile, this.largeur, this.hauteur);
        }

        /// <summary>
        /// Conversion d'un entier en forme endian
        /// </summary>
        /// <param name="val">valeur entière à convertir</param>
        /// <returns>même valeur sous le forma endian</returns>
        public byte[] Convertir_Int_To_Endian(int val)
        {
            byte[] end = new byte[4];
            while (val >= Math.Pow(256, 3) && end[3] <= 255)
            {
                val -= Convert.ToInt32(Math.Pow(256, 3));
                end[3] += 1;
            }
            while (val >= Math.Pow(256, 2) && end[2] <= 255)
            {
                val -= Convert.ToInt32(Math.Pow(256, 2));
                end[2] += 1;
            }
            while (val >= Math.Pow(256, 1) && end[1] <= 255)
            {
                val -= Convert.ToInt32(Math.Pow(256, 1));
                end[1] += 1;
            }
            while (val >= Math.Pow(256, 0) && end[0] <= 255)
            {
                val -= Convert.ToInt32(Math.Pow(256, 0));
                end[0] += 1;
            }
            return end;
        }

        /// <summary>
        /// Conversion d'une valeur endian en entier
        /// </summary>
        /// <param name="tab">valeur à convertir</param>
        /// <returns>entier converti</returns>
        public int Convertir_Endian_To_Int(byte[] tab)
        {
            int end = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                end += Convert.ToInt32(tab[i] * Math.Pow(256, i));
            }
            return end;
        }

        /// <summary>
        /// Permet de rentrer un tableau dans un tableau plus graand à partir d'une certaine ligne
        /// </summary>
        /// <param name="petitTableau">tableau à inserer</param>
        /// <param name="grandTableau">tableau ou l'insersion se fait</param>
        /// <param name="index_deb">indice à partir duquel on a les valeurs à rentrer</param>
        /// <returns>nouveau tableau rempli</returns>
        public byte[] Rentrer_tab_tab(byte[] petitTableau, byte[] grandTableau, int index_deb)
        {
            foreach (byte k in grandTableau)
            {
                petitTableau[index_deb] = k;
                index_deb++;
            }
            return petitTableau;
        }

        /// <summary>
        /// Permettre de transformer une matrice de pixel ainsi que ces attribut en tableau de Byte au format bmp
        /// </summary>
        /// <returns>image sous forme de fichier qu'on pourra afficher</returns>
        public byte[] From_Image_To_File()
        {
            byte[] res = new byte[55 + (this.hauteur * this.largeur) * 3];
            int i = 2;
            res[0] = 66;
            res[1] = 77;
            byte[] taillefichiertab = Convertir_Int_To_Endian(this.tailleF);
            Rentrer_tab_tab(res, taillefichiertab, i);
            i = 1 + taillefichiertab.Length;
            byte[] header = new byte[] { 0, 0, 0, 0, 0, 54, 0, 0, 0 };
            Rentrer_tab_tab(res, header, i);
            i += header.Length;
            byte[] tailleheader = Convertir_Int_To_Endian(this.tailleOf);
            Rentrer_tab_tab(res, tailleheader, i);
            i += tailleheader.Length;
            byte[] largeuroctet = Convertir_Int_To_Endian(this.largeur);
            Rentrer_tab_tab(res, largeuroctet, i);
            i += largeuroctet.Length;
            byte[] hauteuroctet = Convertir_Int_To_Endian(this.hauteur);
            Rentrer_tab_tab(res, hauteuroctet, i);
            i += hauteuroctet.Length;
            byte[] constante = new byte[] { 1, 0, 24, 0, 0, 0, 0, 0 };
            Rentrer_tab_tab(res, constante, i);
            i += constante.Length;
            byte[] tailleimmage = Convertir_Int_To_Endian(this.hauteur * this.largeur * 3);
            Rentrer_tab_tab(res, tailleimmage, i);
            i += tailleimmage.Length;
            byte[] constante2 = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            Rentrer_tab_tab(res, constante2, i);
            i = 55;
            for (int k = 0; k < image.GetLength(0); k++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    res[i] = image[k, j].Rouge;
                    res[i + 1] = image[k, j].Vert;
                    res[i + 2] = image[k, j].Bleu;
                    i = i + 3;
                }
            }
            return res;
        }

        /// <summary>
        /// Passe l'image en gris
        /// </summary>
        public void MettreenGris()
        {
            if (image != null && image.Length != 0)
            {
                for (int i = 0; i < image.GetLength(0); i++)
                {
                    for (int j = 0; j < image.GetLength(1); j++)
                    {
                        byte var = Convert.ToByte((image[i, j].Rouge + image[i, j].Vert + image[i, j].Bleu) / 3);
                        image[i, j].Rouge = Convert.ToByte((image[i, j].Rouge + image[i, j].Vert + image[i, j].Bleu) / 3);
                        image[i, j].Vert = Convert.ToByte((image[i, j].Rouge + image[i, j].Vert + image[i, j].Bleu) / 3);
                        image[i, j].Bleu = Convert.ToByte((image[i, j].Rouge + image[i, j].Vert + image[i, j].Bleu) / 3);
                    }
                }
            }
        }

        /// <summary>
        /// Passage de l'image sous la forme d'une chaine de caractères lisible
        /// </summary>
        /// <returns>chaîne de caractères caractérisant l'image</returns>
        public string toString()
        {
            string res = "";
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    res += image[i, j].toString() + " ";
                }
                res += "\n";
            }
            return res;
        }

        /// <summary>
        /// Permet de tourner l'image d'un angle angle
        /// </summary>
        /// <param name="deg">angle de la rotation à effectuer en radian</param>
        public void rotation(double deg)
        {
            int hauteur = Convert.ToInt32(Math.Round(image.GetLength(0) * Math.Abs(Math.Cos(deg)) + image.GetLength(1) * Math.Abs(Math.Sin(deg))));
            int largeur = Convert.ToInt32(Math.Round(image.GetLength(0) * Math.Abs(Math.Sin(deg)) + image.GetLength(1) * Math.Abs(Math.Cos(deg))));
            while(largeur%4!=0)
            {
                largeur++;
            }
            Pixel[,] res = new Pixel[hauteur, largeur];
            double[,] mat = { { Math.Cos(deg), -1 * Math.Sin(deg) }, { Math.Sin(deg), Math.Cos(deg) } };
            int[] tab = new int[2];
            int[] val;
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    tab[0] = i;
                    tab[1] = j;
                    val = multiplication_matrie(mat, tab);
                    res[val[0], val[1]] = new Pixel(image[i, j].Rouge, image[i, j].Vert, image[i, j].Bleu);
                }
            }
            this.image = res;
            this.hauteur = image.GetLength(0);
            this.largeur = image.GetLength(1);
            this.tailleF = this.hauteur * this.largeur * 24;
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    if (image[i, j] == null)
                    {
                        image[i, j] = new Pixel(255, 255, 255);
                    }
                }
            }
        }

        /// <summary>
        /// Permet d'étendre une couleur en remplissant un carré de cette couleur
        /// </summary>
        /// <param name="couleur">pixel de couleur à étendre</param>
        /// <param name="xval">abscisse a partir duquel étendre</param>
        /// /// <param name="yval">ordonnée sur l'image</param>
        /// <param name="cote_carre">longueur du nouveau carré</param>
        /// <param name="impiv">carré aggrandi</param>
        /// <param name="test">valeur test non utile</param>
        /// <returns></returns>
        public Pixel[,] remplir_carre_image(Pixel couleur, int xval, int yval, int cote_carre, Pixel[,] impiv,int test)
        {
            for (int i=0;i<cote_carre ;i++)///&& i < impiv.GetLength(0)
            {
                for (int j=0;j<cote_carre ;j++)///&& j < impiv.GetLength(1)
                {
                    impiv[xval+i, yval+j] = new Pixel(couleur.Rouge, couleur.Vert, couleur.Bleu);
                }
            }
            return impiv;
        }

        /// <summary>
        /// Permet d'agrandir une image en étendant les pixels de couleur
        /// </summary>
        /// <param name="IndAggrandissement">entier définissant le degré d'agrandissmenet</param>
        public void AggrandissementImage(int IndAggrandissement)
        {
            Pixel[,] imageAggrandie = new Pixel[0, 0];
            int k1 = 0;
            int k2 = 0;
            int test = 0;
            if (image != null && image.Length != 0)
            {
                int h = Convert.ToInt32(image.GetLength(0) * IndAggrandissement);
                int l = Convert.ToInt32(image.GetLength(1) * IndAggrandissement);
                while (l%4!=0)
                {
                    l++;
                }
                imageAggrandie = new Pixel[h, l];
                for (int i = 0; i < this.hauteur;i++)
                {
                    for (int j = 0; j < this.largeur ;j++)
                    {
                        imageAggrandie = remplir_carre_image(this.Image[i, j], k2, k1, IndAggrandissement,imageAggrandie,test);
                        k1 += IndAggrandissement;
                        test++;
                    }
                    k2 += IndAggrandissement;
                    k1 = 0;
                }

            }
            this.image = imageAggrandie;
            this.hauteur = this.image.GetLength(0);
            this.largeur = this.image.GetLength(1);
            this.tailleF = this.hauteur * this.largeur * 24;
            for (int i=0;i<image.GetLength(0);i++)
            {
                for(int j=0;j<image.GetLength(1);j++)
                {
                    if (image[i,j]==null)
                    {
                        image[i, j] = new Pixel(0, 0, 0);
                    }
                }
            }
        }

        /// <summary>
        /// Multiplication des matrices dans le cadre de l'utilisation d'une matrice de rotation
        /// </summary>
        /// <param name="mat">matrice de rotation</param>
        /// <param name="tab">abcisse et ordonné du Pixel</param>
        /// <returns>résultats des nouveaux index retournés</returns>
        public int[] multiplication_matrie(double[,] mat, int[] tab)
        {
            int[] res = new int[2];
            res[0] = Convert.ToInt32(Math.Round(mat[0, 0] * tab[0] + mat[0, 1] * tab[1]));
            res[1] = Convert.ToInt32(Math.Round(mat[1, 0] * tab[0] + mat[1, 1] * tab[1]));
            if (res[0] < 0)
            {
                res[0] = res[0] * (-1);
            }
            if (res[1] < 0)
            {
                res[1] = res[1] * (-1);
            }
            return res;
        }

        /// <summary>
        /// Permet de mettre en valeur les bords de l'image
        /// </summary>
        public void detection_bord()
        {
            double[,] matconv = new double[,] { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } };
            Convolution(matconv);
        }

        /// <summary>
        /// Permet de renforcer les bords de l'image en les rendant plus marquées
        /// </summary>
        public void RenforcementDesBord()
        {
            double[,] matconv = new double[,] { { 0, 0, 0 }, { -1, 2, 0 }, { 0, 0, 0 } };
            Convolution(matconv);
        }

        /// <summary>
        /// Permet de rendre flou une image
        /// </summary>
        public void Flou()
        {
            double[,] matconv = new double[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
            for (int i=0;i<matconv.GetLength(0);i++)
            {
                for (int j=0;j<matconv.GetLength(1);j++)
                {
                    matconv[i,j] = matconv[i,j]/matconv.Length;
                }
            }
            Convolution(matconv);
        }

        /// <summary>
        /// Permet de faire ressortir les détails de l'image
        /// </summary>
        public void Repoussage()
        {
            double[,] matconv = new double[,] { { 0, -1, 0 }, { -1, 5, -1 }, { 0, -1, 0 } };
            Convolution(matconv);
        }

        /// <summary>
        /// Permet d'effectuer la convolution avec la matrice placée en paramètre
        /// </summary>
        /// <param name="MatConv">matrice à utiliser pour la convolution</param>
        public void Convolution(double[,] MatConv)
        {
            Pixel[,] mattemp;
            Pixel prod;
            Pixel[,] cop = new Pixel[this.hauteur, this.largeur];
            for (int i = 0; i < this.image.GetLength(0); i++)
            {
                for (int j = 0; j < this.image.GetLength(1); j++)
                {
                    mattemp = Mat_autour_pixel(MatConv.GetLength(0), i, j);
                    prod = superposition_pixel(mattemp, MatConv);
                    cop[i, j] = prod;
                }
            }
            this.image = cop;
        }

        /// <summary>
        /// Etape calculatoire de la convolution
        /// </summary>
        /// <param name="a">matrice sur laquelle agir</param>
        /// <param name="b">matrice pour la convolution</param>
        /// <returns>valeurs du pixel après l'étape de convolution</returns>
        public Pixel superposition_pixel(Pixel[,] a, double[,] b)
        {
            Pixel prod;
            double Rouge = 0;
            double Vert = 0;
            double Bleu = 0;
            for (int i = 0; i < b.GetLength(0); i++)
            {
                for (int j = 0; j < b.GetLength(1); j++)
                {
                    Rouge = Rouge + a[i, j].Rouge * b[i, j];
                    Bleu = Bleu + a[i, j].Bleu * b[i, j];
                    Vert = Vert + a[i, j].Vert * b[i, j];
                }
            }
            prod = new Pixel(convertir_in_byte(Rouge),convertir_in_byte(Vert), convertir_in_byte(Bleu));
            return prod;
        }

        /// <summary>
        /// Replace un nombre réel entre 0 et 255 en format byte
        /// </summary>
        /// <param name="i">nombre réel à transformer</param>
        /// <returns>byte correspondant au nombre réel placé en paramètre</returns>
        public byte convertir_in_byte(double i)
        {
            if (i> 255)
            {
                i = 255;
            }
            else if (i < 0)
            {
                i = 0;
            }
            return Convert.ToByte(i);
        }

        /// <summary>
        /// Permet de selectionner les Pixels autours d'un certain index afin de le superposer à la matrice de convolution
        /// </summary>
        /// <param name="taille_mat">dimension de la matrice carrée</param>
        /// <param name="mil_x">abcisse du Pixel du millieu</param>
        /// <param name="mil_y">ordonnée du Pixel du millie</param>
        /// <returns>matrice de pixels representant les pixel autours du pixel du millieux de la matrice dans l'image</returns>
        public Pixel[,] Mat_autour_pixel(int taille_mat, int mil_x, int mil_y)
        {
            Pixel[,] res = new Pixel[taille_mat, taille_mat];
            for (int i = 0; i < res.GetLength(0); i++)
            {
                for (int j = 0; j < res.GetLength(1); j++)
                {
                    if (mil_x - taille_mat / 2 + i >= 0 && mil_y - taille_mat / 2 + j >= 0 && mil_x - taille_mat / 2 + i < this.image.GetLength(0) && mil_y - taille_mat / 2 + j < this.image.GetLength(1))
                    {
                        res[i, j] = this.image[mil_x - taille_mat / 2 + i, mil_y - taille_mat / 2 + j];
                    }
                    else
                    {
                        res[i, j] = new Pixel(0, 0, 0);
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Permet de mettre une valeur entre 0 et 255 sous la forme d'un octet constitué de bits
        /// </summary>
        /// <param name="a">valeur byte à convertir</param>
        /// <returns>valeur après son changement en un octet</returns>
        public byte[] tranformation_byte_octet(byte a)
        {
            byte[] res = new byte[8];
            for (int i=0;i<8;i++)
            {
                if ( a-Math.Pow(2,7-i) <0)
                {
                    res[i] = 0;
                }
                else
                {
                    res[i] = 1;
                }
            }
            return res;
        }

        /// <summary>
        /// etape du codage d'une image dans une image dans laquelle on place les bits de poid fort de cahque image pour former un noiuvel octet modifié
        /// </summary>
        /// <param name="a">premier tableau de bytes à utiliser</param>
        /// <param name="b">deuxième tableau de bytes à utiliser</param>
        /// <returns>résultat dans un seul byte</returns>
        public byte tranformation_ImagedansImageCouleur(byte[] a, byte[] b)
        {
            byte res = 0;
            for (int i=0;i<8;i++)
            {
                if (i<=3)
                {
                    res += Convert.ToByte(Math.Pow(2, 7 - i) * a[i]);
                }
                else
                {
                    res += Convert.ToByte(Math.Pow(2, 7 - i-4) * b[i-4]);
                }
            }
            return res;
        }

        /// <summary>
        /// Permet de placer une image dans une autre image
        /// </summary>
        /// <param name="ima">image à placer</param>
        public void CoderImageDansImage(Pixel[,] ima)
        {
             Pixel[,] result = null;
            int h = Math.Max(ima.GetLength(0), this.image.GetLength(0));
            int l = Math.Max(this.image.GetLength(1), ima.GetLength(1));
            result = new Pixel[h, l];
            int test = 0;
            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    if ((i>=ima.GetLength(0) || j>= ima.GetLength(1)) && i < image.GetLength(0) && j < image.GetLength(1))
                    {
                        result[i,j] = new Pixel(this.image[i, j].Rouge, this.image[i, j].Vert, this.image[i, j].Bleu);
                        test++;
                    }
                    else if ((i >= image.GetLength(0) || j >= image.GetLength(1)) && i < ima.GetLength(0) && j < ima.GetLength(1))
                    {
                            result[i, j] = new Pixel(ima[i, j].Rouge, ima[i, j].Vert, ima[i, j].Bleu);
                    }
                    else if (i < image.GetLength(0) && j < image.GetLength(1) && i < ima.GetLength(0) && j < ima.GetLength(1))
                    {
                            result[i, j] = new Pixel(tranformation_ImagedansImageCouleur(tranformation_byte_octet(this.image[i, j].Rouge), tranformation_byte_octet(ima[i, j].Rouge)), tranformation_ImagedansImageCouleur(tranformation_byte_octet(this.image[i, j].Vert), tranformation_byte_octet(ima[i, j].Vert)), tranformation_ImagedansImageCouleur(tranformation_byte_octet(this.image[i, j].Bleu), tranformation_byte_octet(ima[i, j].Bleu)));
                    }
                    else
                    {
                        result[i, j] = new Pixel(0, 0, 0);
              
                    }
                }
            }
            this.image = new Pixel[h, l];
            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    this.image[i, j] = new Pixel(result[i, j].Rouge, result[i, j].Vert, result[i, j].Bleu);
                }
            }
            this.hauteur = h;
            this.largeur = l;
            Console.WriteLine(test);
            this.tailleF = this.hauteur * this.largeur * 24;
        }

        /// <summary>
        /// transformer les points faible d'un octet afin de retrouver l'octet d'origine de l'image placer dans une autre image 
        /// </summary>
        /// <param name="a">octet à convertir</param>
        /// <returns>résultat sous forme d'un byte</returns>
        public byte transfo_octet_byte(byte[] a)
        {
            byte res = 0;
            for (int i = 4; i < 8; i++)
            {
                 res += Convert.ToByte(Math.Pow(2, 7 - i) * a[i]);
            }
            return res;
        }

        /// <summary>
        /// transformer les points fort d'un octet afin de retrouver l'octet d'origine de l'image placer en arrière plan
        /// </summary>
        /// <param name="a">octet à convertir</param>
        /// <returns>résultat sous forme d'un byte</returns>
        public byte transfo_octet_byte2(byte[] a)
        {
            byte res = 0;
            for (int i = 0; i < 4; i++)
            {
                res += Convert.ToByte(Math.Pow(2, 7 - i) * a[i]);
            }
            return res;
        }

        /// <summary>
        /// Extraire l'image placée dans une autre image
        /// </summary>
        /// <returns>image qu'on a réussi à extraire</returns>
        public Pixel[,] DécoderImageDansImage1()
        {
            byte rouge;
            byte vert;
            byte bleu;
            Pixel[,] result = new Pixel[hauteur, largeur];
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    rouge = transfo_octet_byte(tranformation_byte_octet(this.image[i, j].Rouge));
                    vert =  transfo_octet_byte(tranformation_byte_octet(this.image[i, j].Vert));
                    bleu =  transfo_octet_byte(tranformation_byte_octet(this.image[i, j].Bleu));
                    this.image[i, j] = new Pixel(rouge,vert,bleu);
                }
            }
            return result;
        }

        /// <summary>
        /// Extraire l'image placée en arrière plan
        /// </summary>
        /// <returns>image qu'on a réussi à extraire</returns>
        public Pixel[,] DécoderImageDansImage2()
        {
            byte rouge;
            byte vert;
            byte bleu;
            Pixel[,] result = new Pixel[hauteur, largeur];
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    rouge = transfo_octet_byte2(tranformation_byte_octet(this.image[i, j].Rouge));
                    vert = transfo_octet_byte2(tranformation_byte_octet(this.image[i, j].Vert));
                    bleu = transfo_octet_byte2(tranformation_byte_octet(this.image[i, j].Bleu));
                    this.image[i, j] = new Pixel(rouge, vert, bleu);
                }
            }
            return result;
        }

        /// <summary>
        /// Permet de fabriquer la fractale d'une image
        /// </summary>
        /// <param name="nbRépétitions">nombre de répétitions à effectuer pour obtenir la fractale</param>
        public void Fractale(int nbRépétitions)
        {
            Pixel[,] image_copy = image;
            double Xmin = -2.1;//coordonnées universelles de l'ensemble de Mandlebrot
            double Xmax = 0.6;
            double Ymin = -1.2;
            double Ymax = 1.2;
            double zoomX = (Xmax - Xmin) / image.GetLength(0);//on cherche l'aggrandissement associée entre la matrice image et x et y
            double zoomY = (Ymax - Ymin) / image.GetLength(1);
            int nbRepetitions = 100;

            for (int x = 0; x < image.GetLength(0); x++) //remplir pour chaque coordonées de x,y
            {
                for (int y = 0; y < image.GetLength(1); y++)
                {
                    double coorX = Xmin + x * zoomX; //on parcours chaque pixel de la nouvelle échelle
                    double coorY = Ymin + y * zoomY;
                    double zreel = 0.0; //création du nombre complexe z correspondant à la distance entre l'origine du repère et le point de coordonées (x,y)
                    double zim = 0.0;
                    int n = 0;
                    for (n = 0; n < nbRepetitions; n++)//à partir de 50 on peut faire une estimation précise de la nature des coordonées et si elles restente dans l'ensemble
                    {
                        double z1 = (zreel * zreel - zim * zim) + coorX; // création de variables temporaire et calcul des valeurs de z selon l'ensemble de Mandlebrot
                        double z2 = 2 * (zreel * zim) + coorY;
                        zreel = z1;
                        zim = z2;
                        if (zreel * zreel + zim * zim > 4) //racourcir la complexité car si le module de z >2 les coordonées sortent de l'ensemble 
                        {
                            break;
                        }
                    }

                    if (n == nbRepetitions)
                    {
                        image[x, y] = new Pixel(0, 255, 0);
                    }
                    else
                    {
                        image[x, y] = new Pixel(0, 0, 0);
                    }
                }
            }
        }

        /// <summary>
        /// Permet de placer les noeuds d'huffmann par ordre croissant de fréquence
        /// </summary>
        /// <param name="list_tri">différents sous arbres d'huffmann a trier</param>
        /// <returns>nouvel ordre des sous arbres d'huffmann dans la liste</returns>
        public List<Noeud> tri_freq_desc(List<Noeud> list_tri)
        {
            Noeud piv;
            for (int i =0;i<list_tri.Count;i++)
            {
                for (int j = 0; j < list_tri.Count-i-1; j++)
                {
                    if (list_tri[j].Freq > list_tri[j++].Freq)
                    {
                        piv = list_tri[j];
                        list_tri[j] = list_tri[j++];
                        list_tri[j++] = piv;
                    }
                }
            }
            return list_tri;
        }

        /// <summary>
        /// Etape de mise au point dans l'arbre d'Huffmann
        /// </summary>
        /// <returns>retourne tous les noeuds de l'abres d'hufmann trier par fréuence croissantes</returns>
        public List<Noeud> PixFreq()
        {
            List<Noeud> noeuds_freq = new List<Noeud>();
            Noeud piv;
            foreach(Pixel i in this.image)
            {
                piv = new Noeud(i, 1, null, null);
                if(noeuds_freq.Contains(piv))
                {
                    noeuds_freq[noeuds_freq.IndexOf(piv)].Freq++;
                }
                else
                {
                    noeuds_freq.Add(piv);
                }
            }
            noeuds_freq = tri_freq_desc(noeuds_freq);
            return noeuds_freq;
        }
    }
}
