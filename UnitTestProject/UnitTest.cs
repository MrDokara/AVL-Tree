using Microsoft.VisualStudio.TestTools.UnitTesting;
using AVLTreeLib;
using System;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestingMethodContains()
        {
            var tree = new AVLTree<int, int>();
            tree.Add(5, 0);
            tree.Add(2, 0);
            tree.Add(7, 0);
            Assert.AreEqual(true, tree.ContainsKey(5));
            Assert.AreEqual(true, tree.ContainsKey(2));
            Assert.AreEqual(true, tree.ContainsKey(7));
            Assert.AreEqual(false, tree.ContainsKey(4));
        }

        [TestMethod]
        public void TestingMethodRemove()
        {
            var tree = new AVLTree<int, int>();
            tree.Add(5, 0);
            tree.Add(2, 0);
            tree.Add(7, 0);
            tree.RemoveKey(2);
            Assert.AreEqual(true, tree.ContainsKey(5));
            Assert.AreEqual(false, tree.ContainsKey(2));
            Assert.AreEqual(true, tree.ContainsKey(7));
            Assert.AreEqual(false, tree.ContainsKey(4));
        }

        [TestMethod]
        public void DuplicateException()
        {
            var tree = new AVLTree<int, int>();
            tree.Add(5, 0);
            tree.Add(2, 0);
            tree.Add(7, 0);
            Assert.ThrowsException<ArgumentException>((() => tree.Add(2, 0)));
        }
    }
}
