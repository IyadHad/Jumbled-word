using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
namespace LectureImage
{
    [TestClass]
    public class TestProject4
    {

        Myimage coco = new Myimage("./Images/coco.bmp");

        [TestMethod]
        
       
        public void TestConversion_Int_To_Endian_ValRandom()
        {
            
            Random random = new Random();
            int val =10;
            byte[] val1 = BitConverter.GetBytes(val);
            byte[] val2 = coco.Convertir_Int_To_Endian(val);
            Assert.AreEqual(val1, val2);
        }

        [TestMethod]

        public void TestMultiplicationMatricee()
        {
            
            double[,] matI = new double[,] { { 0, 0 }, { 0, 0 } };
            int[] tab = new int[] { 1, 2 };
            int[] val1 = new int[] { 0, 0 };
            int[] val2 = coco.multiplication_matrie(matI, tab);
            Assert.AreEqual(val1, val2);
        }

        [TestMethod]

        public void Test_Rentrer_tab_tab()
        {
            
            byte[] Tab1 = new byte[5] { 9, 9, 0, 0, 0 };
            byte[] Tab2 = new byte[3] { 9, 8, 9 };
            int index_deb = 2;
            byte[] TabFinal = new byte[5] { 9, 9, 9, 8, 9 };            
            byte[] SommeTab = coco.Rentrer_tab_tab(Tab1, Tab2, index_deb);
            CollectionAssert.AreEqual(TabFinal, SommeTab);
        }

        [TestMethod]

        public void Test_Convertir_Endian_To_Int()
        {
            
            byte[] val = new byte[] { 1, 0, 0, 1 };
            int val1 = 257;
            int val2 = coco.Convertir_Endian_To_Int(val);
            Assert.AreEqual(val1, val2);
        }

        [TestMethod]

        public void Test_Convertir_In_Byte()
        {
            
            byte val1 = coco.convertir_in_byte(0);
            byte val2 = (byte)0;
            Assert.AreEqual(val1, val2);
        }



    }
}