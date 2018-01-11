using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace deckOfCards.Controllers
{
    public class MultiplayerController : Controller
    {

        String[] Initialdeck = { "c2", "c3", "c4", "c5", "c6", "c7", "c8", "c9", "c10", "cJ", "cQ", "cK", "cA", "d2", "d3", "d4", "d5", "d6", "d7", "d8", "d9", "d10", "dJ", "dQ", "dK", "dA", "h2", "h3", "h4", "h5", "h6", "h7", "h8", "h9", "h10", "hJ", "hQ", "hK", "hA", "s2", "s3", "s4", "s5", "s6", "s7", "s8", "s9", "s10", "sJ", "sQ", "sK", "sA" }; // initial deck of cards with all card symbols included (ex: s4 = Spade 4)

        List<string> InitialDealtCards = new List<string>(new string[] { }); // empty list of strings created only for better understanding of the code
        public ActionResult Index()
        {
            if (Session["MultiplayerDeck"] == null) // checking if this is the first visit in the session
            {
                // instantiate a new one
                Session["MultiplayerDeck"] = Initialdeck; // initializing the deck
                Session["NumberOfPlayers"] = 1; // initializing the deck
            }

            if (Session["MultiplayerDealtCards"+ Session["NumberOfPlayers"].ToString()] == null) // checking if this is the first visit in the session or number of players increased
            {
                // instantiate a new one
                Session["MultiplayerDealtCards"+ Session["NumberOfPlayers"].ToString()] = InitialDealtCards; // initializing to the empty list of strings ( to be populated later on)
                Session["ErrorMessageMultiplayer"] = null;
            }

            //ViewData is used to pass data from controller to view. Another approach was to access the session variables directly, but that would be unprofessional
            ViewData["DeckArray"] = Session["MultiplayerDeck"];

            int PlayerNumber = (int)Session["NumberOfPlayers"];
            while (PlayerNumber > 0)
            {
                ViewData["MultiplayerDealtCards" + PlayerNumber] = Session["MultiplayerDealtCards" + PlayerNumber.ToString()];
                PlayerNumber--;
            }
            return View();
        }

        public ActionResult Shuffle() //shuffle the deck
        {

            //  Algorithm is Based on Java code from wikipedia:
            //  http://en.wikipedia.org/wiki/Fisher-Yates_shuffle
            Random r = new Random();
            string[] deck = (String[])Session["MultiplayerDeck"]; //getting the deck elements from the session variable

            if (deck.Length != 0) //checking if there are elements in the deck
            {
                for (int n = deck.Length - 1; n > 0; --n) //shuffling
                {
                    int k = r.Next(n + 1);
                    string temp = deck[n];
                    deck[n] = deck[k];
                    deck[k] = temp;
                }
                Session["MultiplayerDeck"] = deck; // session variable = shuffled deck
            }

            else // there are no more elements in the deck
            {
                Session["ErrorMessageMultiplayer"] = "No more cards in deck, click Start Again";
            }

            return RedirectToAction("Index");

        }

        public ActionResult DealOneCard(string playernumber)
        {

            string[] deck = (String[])Session["MultiplayerDeck"]; //getting deck from session
            List<string> DealtCards = (List<string>)Session["MultiplayerDealtCards"+ playernumber.ToString()]; //getting list of dealt cards

            if (deck.Length != 0) // there are more cards in the deck
            {
                DealtCards.Add(deck[0]); // dealing the first card in the deck
                Session["MultiplayerDealtCards" + playernumber] = DealtCards; //updating list of dealt cards to the session
                Session["MultiplayerDeck"] = deck.Skip(1).ToArray(); //updating deck to the session
            }
            else // in case no more cards are in the deck
            {
                Session["ErrorMessageMultiplayer"] = "No more cards in deck, click Start Again";
            }

            return RedirectToAction("Index");
        }

        public ActionResult Clear()
        {
            // restoring initial conditions
            Session["NumberOfPlayers"]=1;
            Session["MultiplayerDealtCards1"] = InitialDealtCards;
            Session["MultiplayerDeck"] = Initialdeck; // initializing the deck
            Session["ErrorMessageMultiplayer"] = null;


            return RedirectToAction("Index");
        }

        public ActionResult AddPlayer()
        {
            Session["NumberOfPlayers"] = (int)Session["NumberOfPlayers"] + 1; // getting players number
            Session["MultiplayerDealtCards" + Session["NumberOfPlayers"].ToString()] = InitialDealtCards; //setting the initial dealt cards of the new player to zero

            return RedirectToAction("Index");
        }

    }
}
