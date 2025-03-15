using System;
using System.Collections.Generic;

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

        public enum FaceCards
        {
            None,
            King = 13,
            Queen = 12,
            Jack = 11
        }

        private int Rank { get; set; }
        private Suits Suit { get; set; }
        public bool IsFaceUp { get; set; } = false;

        //Constructor for Card class
        public Card(int rank, Suits suit)
        {
            Rank = rank;
            Suit = suit;
        }

        //Returns a string representation of the card
        public string GetCard()
        {
            return $"{Rank} of {Suit}";
        }

        //Flip the card to change its face-up state
        public void Flip()
        {
            IsFaceUp = !IsFaceUp;
        }
    }

    public class Deck
    {
        private List<Card> Cards;

        //Deck Constructor
        public Deck()
        {
            Cards = new List<Card>();
        }

        //Initialize the deck with cards
        public void InitializeFullDeck()
        {
            for (int i = 1; i <= 13; i++)
            {
                //Add 4 cards for each rank, one for each suit
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
        
        }

        //Deal a card from the deck
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
                return null;  //If no cards are left
            }
        }

        //To get the number of cards remaining in the deck
        public int GetRemainingCards()
        {
            return Cards.Count;
        }

        public void AddToDeck(Card card)
        {
            Cards[-1] = card;
        }
    }

    class Elevens
    {
        static void Main(string[] args)
        {
            Deck deck = new Deck();

            deck.Shuffle();

            Card? dealtCard = deck.DealCard();
            if (dealtCard != null)
            {
                Console.WriteLine(dealtCard.GetCard());
            }

            Console.WriteLine("Cards left in deck: " + deck.GetRemainingCards());
        }
    }
}
