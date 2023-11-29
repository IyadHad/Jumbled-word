using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Collections;

namespace LectureImage
{
    class Program
    {
        static List<int> image_compressé(Myimage im)
        {
            Huffmann decomp = new Huffmann(im.PixFreq());
            int f=0;
            foreach (Pixel i in im.Image)
            {
                foreach (Noeud k in decomp.Frequencies)
                {
                    if (k.Pix == i)
                    {
                        f = k.Freq;
                    }
                }
                decomp.trouver_chemin_pixel(i,f,decomp.Root);
            }
            return decomp.Root.Chemin;
        }

        static void lireHufmann(Myimage coco)
        {
            List<int> a = image_compressé(coco);
            foreach (int i in a)
            {
                Console.WriteLine(i + " ");
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Que voulez-vous ?");
            Console.WriteLine("1 : agrandissment");
            Console.WriteLine("2 : rotation");
            Console.WriteLine("3 : détection des bords");
            Console.WriteLine("4 : renforcement des bords");
            Console.WriteLine("5 : flou");
            Console.WriteLine("6 : repoussage");
            Console.WriteLine("7 : coder image dans image");
            Console.WriteLine("8 : décoder l'image d'arrière plan");
            Console.WriteLine("9 : décoder l'image en premier plan");
            Console.WriteLine("10 : fractale");
            Myimage coco = new Myimage("./Images/coco.bmp");
            Myimage imint = new Myimage("./Images/lena.bmp");
            Myimage imint2 = new Myimage("./Images/coco.bmp");
            int rep = Convert.ToInt32(Console.ReadLine());
            switch (rep)
            {
                case 1:
                    Console.WriteLine("Quel coéficient d'agrandissement voulez vous?");
                    string i= "n" ;
                    int res;
                    while (int.TryParse(i,out res) != true)
                    {
                        i = Console.ReadLine();
                    }
                    coco.AggrandissementImage(res);
                    break;
                case 2:
                    coco.rotation(1.40);
                    break;
                case 3:
                    coco.detection_bord();
                    break;
                case 4:
                    coco.RenforcementDesBord();
                    break;
                case 5:
                    coco.Flou();
                    break;
                case 6:
                    coco.Repoussage();
                    break;
                case 7:
                    coco.CoderImageDansImage(imint.Image);
                    break;
                case 8:
                    imint2.Repoussage();
                    coco.CoderImageDansImage(imint2.Image);
                    coco.DécoderImageDansImage1();
                    break;
                case 9:
                    imint2.Repoussage();
                    coco.CoderImageDansImage(imint2.Image);
                    coco.DécoderImageDansImage2();
                    break;
                case 10:
                    Console.WriteLine("Quel nombre de repetition voulez vous?");
                    string j = "n";
                    int res2;
                    while (int.TryParse(j, out res2) != true)
                    {
                        j = Console.ReadLine();
                    }
                    coco.Fractale(1000);
                    break;
            }
            byte[] var = coco.From_Image_To_File();
            File.WriteAllBytes("./Images/coco2.bmp", var);
            /*Myimage coco = new Myimage("./Images/test.bmp");
            Myimage imint = new Myimage("./Images/coco.bmp");
            ///coco.AggrandissementImage();
            ///coco.rotation(1.40);
            ///coco.detection_bord();
            ///coco.RenforcementDesBord();
            ///coco.Flou();
            ///coco.Repoussage();
            ///coco.AggrandissementImage(3);
            ///imint.CoderImageDansImage(coco.Image);
            ///coco.DécoderImageDansImage();
            ///coco.Fractale(1000);
            ///imint.DécoderImageDansImage1();
            ///imint.DécoderImageDansImage2();
            byte[] var = coco.From_Image_To_File();
            File.WriteAllBytes("./Images/coco2.bmp", var);
            lireHufmann(coco);*/
            Console.ReadLine();
        }
    }
}
