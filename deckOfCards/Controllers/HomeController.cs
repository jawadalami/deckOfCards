using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace deckOfCards.Controllers
{
    public class HomeController : Controller
    {

        String[] Initialdeck= { "c2", "c3", "c4", "c5", "c6", "c7", "c8", "c9", "c10", "cJ", "cQ", "cK", "cA", "d2", "d3", "d4", "d5", "d6", "d7", "d8", "d9", "d10", "dJ", "dQ", "dK", "dA", "h2", "h3", "h4", "h5", "h6", "h7", "h8", "h9", "h10", "hJ", "hQ", "hK", "hA", "s2", "s3", "s4", "s5", "s6", "s7", "s8", "s9", "s10", "sJ", "sQ", "sK", "sA" }; // initial deck of cards with all card symbols included (ex: s4 = Spade 4)

        List<string> InitialDealtCards = new List<string>(new string[] { }); // empty list of strings created only for better understanding of the code
        public ActionResult Index()
        {
                if (Session["Deck"] == null) // checking if this is the first visit in the session
                {
                    // instantiate a new one
                    Session["Deck"] = Initialdeck; // initializing the deck
                    Session["ErrorMessage"] = null;
                 }

                if (Session["DealtCards"] == null) // checking if this is the first visit in the session
                {   
                     // instantiate a new one
                    Session["DealtCards"] = InitialDealtCards; // initializing to the empty list of strings ( to be populated later on)
                }

            //ViewData is used to pass data from controller to view. Another approach was to access the session variables directly, but that would be unprofessional
            ViewData["DeckArray"] = Session["Deck"]; 
            ViewData["DealtCardsArray"] = Session["DealtCards"];

            return View();
        }
     
        public ActionResult Shuffle() //shuffle the deck
        {
            
            //  Algorithm is Based on Java code from wikipedia:
            //  http://en.wikipedia.org/wiki/Fisher-Yates_shuffle
                 Random r = new Random();
                string[] deck = (String[])Session["Deck"]; //getting the deck elements from the session variable

                if (deck.Length != 0) //checking if there are elements in the deck
                {
                    for (int n = deck.Length - 1; n > 0; --n) //shuffling
                    {
                        int k = r.Next(n + 1); 
                        string temp = deck[n];
                        deck[n] = deck[k];
                        deck[k] = temp;
                    }
                    Session["Deck"] = deck; // session variable = shuffled deck
                }

                else // there are no more elements in the deck
                {
                    Session["ErrorMessage"] = "No more cards in deck, click Start Again";
                }

                    return RedirectToAction("Index"); 
            
        }

        public ActionResult DealOneCard()
        {
           
            string[] deck = (String[])Session["Deck"]; //getting deck from session
            List<string> DealtCards =  (List<string>)Session["DealtCards"]; //getting list of dealt cards

            if (deck.Length != 0) // there are more cards in the deck
            {
                DealtCards.Add(deck[0]); // dealing the first card in the deck
                Session["DealtCards"] = DealtCards; //updating list of dealt cards to the session
                Session["Deck"] = deck.Skip(1).ToArray(); //updating deck to the session
            }
            else // in case no more cards are in the deck
            {
                Session["ErrorMessage"] = "No more cards in deck, click Start Again";
            }

            return RedirectToAction("Index");
        }

        public ActionResult Clear()
        {
            // restoring initial conditions
            Session["Deck"] = Initialdeck;
            Session["DealtCards"] = InitialDealtCards;
            Session["ErrorMessage"] = null;

            return RedirectToAction("Index");
        }

    }

}