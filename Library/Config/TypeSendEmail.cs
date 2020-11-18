﻿using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Library.Config
{
    public class TypeSendEmail
    {
        public static int Contact = 1;
        public static int BookTour = 2;
        public static int BookRoom = 3;
        public static int CustomTrip = 4;
        public static int BookCruise = 5;
        public static int Book = 6;
        public static int BookTicket = 7;
        public static int BookVisa = 8;
        public static int RenewPassport = 9;
        public static List<ListItem> ListTypeSendEmail()
        {
            var listTypeSendEmail = new List<ListItem>
            {
                new ListItem
                {
                    Value = Contact.ToString(),
                    Text = "Contact",
                },
                new ListItem
                {
                    Value = BookTour.ToString(),
                    Text = "Book tour",
                },
                new ListItem
                {
                    Value = BookRoom.ToString(),
                    Text = "Book room",
                },
                 new ListItem
                {
                     Value = CustomTrip.ToString(),
                     Text = "Custom your Trip",
                },
                 new ListItem
                {
                    Value = BookCruise.ToString(),
                    Text = "Book cruise",
                },
                  new ListItem
                {
                    Value = Book.ToString(),
                    Text = "Book ",
                },
                   new ListItem
                {
                    Value = BookTicket.ToString(),
                    Text = "Book Ticket",
                },
                   new ListItem
                {
                    Value = RenewPassport.ToString(),
                    Text = "Renew Passport",
                }
                   ,
                   new ListItem
                {
                    Value = BookVisa.ToString(),
                    Text = "Book Visa",
                }
            };

            return listTypeSendEmail;
        }
    }
}