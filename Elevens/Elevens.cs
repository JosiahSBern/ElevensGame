using System;
using System.Collections.Generic;
using System.Linq;

namespace Elevens
{
    public class Card
    {
        public enum Suits
        {
            None,
            Spades,
            Hearts,
            Diamonds,
            Clubs
        }

        public int Rank { get; private set; }
        private Suits Suit { get; set; }
        public bool IsFaceUp { get; set; } = false;

        // Constructor for Card class
        public Card(int rank, Suits suit)
        {
            Rank = rank;
            Suit = suit;
        }

        // Returns a string representation of the card
        public string GetCard()
        {
            return $"{Rank} of {Suit}";
        }

        // Flip the card to change its face-up state
        public void Flip()
        {
            IsFaceUp = !IsFaceUp;
        }
    }

    public class Deck
    {
        private List<Card> Cards;

        // Deck Constructor
        public Deck()
        {
            Cards = new List<Card>();
        }

        // Initialize the deck with cards
        public void InitializeFullDeck()
        {
            for (int i = 1; i <= 13; i++)
            {
                foreach (Card.Suits suit in Enum.GetValues(typeof(Card.Suits)))
                {
                    if (suit != Card.Suits.None)
                    {
                        Cards.Add(new Card(i, suit));
                    }
                }
            }
        }

        public void Shuffle()
        {
            Random rng = new Random();
            Cards = Cards.OrderBy(c => rng.Next()).ToList();
        }

        // Deal a card from the deck
        public Card? DealCard()
        {
            if (Cards.Count > 0)
            {
                Card card = Cards[0];
                Cards.RemoveAt(0);
                return card;
            }
            else
            {
                return null; // If no cards are left
            }
        }

        // To get the number of cards remaining in the deck
        public int GetRemainingCards()
        {
            return Cards.Count;
        }
    }

    public class Game
    {
        private Deck deck;
        private Card[,] grid;

        public Game()
        {
            deck = new Deck();
            deck.InitializeFullDeck();
            deck.Shuffle();
            grid = new Card[3, 3];
            DealGrid();
        }

        private void DealGrid()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (grid[i, j] == null) // Only fill empty slots
                    {
                        var card = deck.DealCard();
                        if (card != null)
                        {
                            grid[i, j] = card;
                        }
                    }
                }
            }
        }

        public void DisplayGrid()
        {
            Console.WriteLine("Current Grid:");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    string location = $"({i},{j})";
                    Console.Write(grid[i, j] != null ? $"{location} {grid[i, j].GetCard()}\t" : $"{location} Empty\t");
                }
                Console.WriteLine();
            }
        }

        public bool CheckForPairs()
        {
            var cards = GetGridCards();
            for (int i = 0; i < cards.Count; i++)
            {
                for (int j = i + 1; j < cards.Count; j++)
                {
                    if (cards[i].Rank + cards[j].Rank == 11)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CheckForFaceSet()
        {
            var cards = GetGridCards();
            bool hasJack = cards.Any(c => c.Rank == 11);
            bool hasQueen = cards.Any(c => c.Rank == 12);
            bool hasKing = cards.Any(c => c.Rank == 13);
            return hasJack && hasQueen && hasKing;
        }

        public void RemovePair(int x1, int y1, int x2, int y2)
        {
            if (grid[x1, y1] != null && grid[x2, y2] != null &&
                grid[x1, y1].Rank + grid[x2, y2].Rank == 11)
            {
                grid[x1, y1] = null;
                grid[x2, y2] = null;
                DealGrid(); // Refill the grid after removing a pair
            }
        }

        public void RemoveFaceSet(int x1, int y1, int x2, int y2, int x3, int y3)
        {
            if (grid[x1, y1] != null && grid[x2, y2] != null && grid[x3, y3] != null)
            {
                var ranks = new[] { grid[x1, y1].Rank, grid[x2, y2].Rank, grid[x3, y3].Rank };
                if (ranks.Contains(11) && ranks.Contains(12) && ranks.Contains(13))
                {
                    grid[x1, y1] = null;
                    grid[x2, y2] = null;
                    grid[x3, y3] = null;
                    DealGrid(); // Refill the grid after removing a face set
                }
            }
        }

        public bool IsGameWon()
        {
            return GetGridCards().Count == 0 && deck.GetRemainingCards() == 0;
        }

        public bool IsGameLost()
        {
            return !CheckForPairs() && !CheckForFaceSet() && deck.GetRemainingCards() == 0;
        }

        private List<Card> GetGridCards()
        {
            var cards = new List<Card>();
            foreach (var card in grid)
            {
                if (card != null)
                {
                    cards.Add(card);
                }
            }
            return cards;
        }
    }

    class Elevens
    {
        static void Main(string[] args)
        {
            Game game = new Game();

            while (true)
            {
                game.DisplayGrid();

                if (game.IsGameWon())
                {
                    Console.WriteLine("You win!");
                    break;
                }

                if (game.IsGameLost())
                {
                    Console.WriteLine("No more moves available. You lose!");
                    break;
                }

                Console.WriteLine("Enter your move:");
                Console.WriteLine("1. Remove a pair (enter coordinates x1 y1 x2 y2)");
                Console.WriteLine("2. Remove a face set (enter coordinates x1 y1 x2 y2 x3 y3)");
                Console.WriteLine("3. Draw cards to refill the grid");

                string input = Console.ReadLine();
                string[] parts = input.Split(' ');

                if (parts.Length == 4)
                {
                    int x1 = int.Parse(parts[0]);
                    int y1 = int.Parse(parts[1]);
                    int x2 = int.Parse(parts[2]);
                    int y2 = int.Parse(parts[3]);
                    game.RemovePair(x1, y1, x2, y2);
                }
                else if (parts.Length == 6)
                {
                    int x1 = int.Parse(parts[0]);
                    int y1 = int.Parse(parts[1]);
                    int x2 = int.Parse(parts[2]);
                    int y2 = int.Parse(parts[3]);
                    int x3 = int.Parse(parts[4]);
                    int y3 = int.Parse(parts[5]);
                    game.RemoveFaceSet(x1, y1, x2, y2, x3, y3);
                }
                else if (parts.Length == 1 && parts[0] == "3")
                {
                    Console.WriteLine("Refilling the grid...");
                    game.DisplayGrid();
                }
                else
                {
                    Console.WriteLine("Invalid input. Try again.");
                }
            }
        }
    }
}
