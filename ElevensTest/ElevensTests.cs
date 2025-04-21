using Microsoft.VisualStudio.TestTools.UnitTesting;
using Elevens;
using System.Collections.Generic;

namespace ElevensTests
{
    [TestClass]
    public class ElevensTests
    {
        [TestMethod]
        public void TestCardInitialization()
        {
            // Arrange
            var card = new Card(5, Card.Suits.Hearts);

            // Act & Assert
            Assert.AreEqual(5, card.Rank);
            Assert.AreEqual("5 of Hearts", card.GetCard());
        }

        [TestMethod]
        public void TestDeckInitialization()
        {
            // Arrange
            var deck = new Deck();

            // Act
            deck.InitializeFullDeck();

            // Assert
            Assert.AreEqual(52, deck.GetRemainingCards());
        }

        [TestMethod]
        public void TestDeckShuffle()
        {
            // Arrange
            var deck = new Deck();
            deck.InitializeFullDeck();
            var originalOrder = new List<Card>(deck.GetRemainingCards());

            // Act
            deck.Shuffle();
            var shuffledOrder = new List<Card>(deck.GetRemainingCards());

            // Assert
            CollectionAssert.AreNotEqual(originalOrder, shuffledOrder, "Deck should be shuffled.");
        }

        [TestMethod]
        public void TestDealCard()
        {
            // Arrange
            var deck = new Deck();
            deck.InitializeFullDeck();

            // Act
            var card = deck.DealCard();

            // Assert
            Assert.IsNotNull(card);
            Assert.AreEqual(51, deck.GetRemainingCards());
        }

        [TestMethod]
        public void TestGameInitialization()
        {
            // Arrange
            var game = new Game();

            // Act
            var gridCards = game.GetType()
                                .GetMethod("GetGridCards", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                                .Invoke(game, null) as List<Card>;

            // Assert
            Assert.AreEqual(9, gridCards.Count, "The grid should be initialized with 9 cards.");
        }

        [TestMethod]
        public void TestCheckForPairs()
        {
            // Arrange
            var game = new Game();
            var grid = new Card[3, 3]
            {
                { new Card(5, Card.Suits.Hearts), new Card(6, Card.Suits.Spades), null },
                { null, null, null },
                { null, null, null }
            };

            // Use reflection to set the private grid field
            var gridField = typeof(Game).GetField("grid", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            gridField.SetValue(game, grid);

            // Act
            var hasPairs = game.CheckForPairs();

            // Assert
            Assert.IsTrue(hasPairs, "The grid should have a pair that adds up to 11.");
        }

        [TestMethod]
        public void TestCheckForFaceSet()
        {
            // Arrange
            var game = new Game();
            var grid = new Card[3, 3]
            {
                { new Card(11, Card.Suits.Hearts), new Card(12, Card.Suits.Spades), new Card(13, Card.Suits.Diamonds) },
                { null, null, null },
                { null, null, null }
            };

            // Use reflection to set the private grid field
            var gridField = typeof(Game).GetField("grid", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            gridField.SetValue(game, grid);

            // Act
            var hasFaceSet = game.CheckForFaceSet();

            // Assert
            Assert.IsTrue(hasFaceSet, "The grid should have a face set (Jack, Queen, King).");
        }

        [TestMethod]
        public void TestRemovePair()
        {
            // Arrange
            var game = new Game();
            var grid = new Card[3, 3]
            {
                { new Card(5, Card.Suits.Hearts), new Card(6, Card.Suits.Spades), null },
                { null, null, null },
                { null, null, null }
            };

            // Use reflection to set the private grid field
            var gridField = typeof(Game).GetField("grid", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            gridField.SetValue(game, grid);

            // Act
            game.RemovePair(0, 0, 0, 1);

            // Assert
            Assert.IsNull(grid[0, 0], "The first card in the pair should be removed.");
            Assert.IsNull(grid[0, 1], "The second card in the pair should be removed.");
        }

        [TestMethod]
        public void TestRemoveFaceSet()
        {
            // Arrange
            var game = new Game();
            var grid = new Card[3, 3]
            {
                { new Card(11, Card.Suits.Hearts), new Card(12, Card.Suits.Spades), new Card(13, Card.Suits.Diamonds) },
                { null, null, null },
                { null, null, null }
            };

            // Use reflection to set the private grid field
            var gridField = typeof(Game).GetField("grid", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            gridField.SetValue(game, grid);

            // Act
            game.RemoveFaceSet(0, 0, 0, 1, 0, 2);

            // Assert
            Assert.IsNull(grid[0, 0], "The Jack should be removed.");
            Assert.IsNull(grid[0, 1], "The Queen should be removed.");
            Assert.IsNull(grid[0, 2], "The King should be removed.");
        }

        [TestMethod]
        public void TestIsGameWon()
        {
            // Arrange
            var game = new Game();
            var grid = new Card[3, 3]
            {
                { null, null, null },
                { null, null, null },
                { null, null, null }
            };

            // Use reflection to set the private grid field
            var gridField = typeof(Game).GetField("grid", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            gridField.SetValue(game, grid);

            // Act
            var isWon = game.IsGameWon();

            // Assert
            Assert.IsTrue(isWon, "The game should be won when the grid is empty.");
        }

        [TestMethod]
        public void TestIsGameLost()
        {
            // Arrange
            var game = new Game();
            var grid = new Card[3, 3]
            {
                { new Card(2, Card.Suits.Hearts), new Card(3, Card.Suits.Spades), new Card(4, Card.Suits.Diamonds) },
                { null, null, null },
                { null, null, null }
            };

            // Use reflection to set the private grid field
            var gridField = typeof(Game).GetField("grid", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            gridField.SetValue(game, grid);

            // Act
            var isLost = game.IsGameLost();

            // Assert
            Assert.IsTrue(isLost, "The game should be lost when there are no valid moves.");
        }
    }
}