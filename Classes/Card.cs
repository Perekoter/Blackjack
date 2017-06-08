using System;

namespace Blackjack
{
	public class Card
	{
		public int Value { get; set; }

		public string Suit { get; set; }

		public string Title { get; set; }

		public string getFullTitle()
		{
			return Suit + " " + Title + " (" + Value + ")";
		}
	}
}

