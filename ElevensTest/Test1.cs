using Elevens;

namespace ElevensTest
{
    [TestClass]
    public sealed class Test1
    {
        private Deck? testDeck;
        [TestInitialize]
        public void SetUp()
        {
            testDeck = new Deck();
        }
        [TestMethod]
        public void GetCardName()
        {
            int rank = 5;
            Card.Suits suit = Card.Suits.Hearts;
            Card heart5 = new Card(rank, suit);


            string cardName = heart5.GetCard();


            Assert.AreEqual("5 of Hearts", cardName);


        }
    }
}
