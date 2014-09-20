using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Collections.Generic;


using com.shephertz.app42.gaming.multiplayer.client;

namespace Movie
{
    class Global
    {
        public static String[] TriviaH = { "Arnold Schwarzegger is honoured with the title ' The most perfectly built man on Earth'.",
                                           "Die Hard originated from the failed script of Commando 2.", 
                                           "Samuel L. Jackson demanded that the studio keep Snakes on a Plane as the title because it was the only reason he accepted the role.",
                                           "Rather than use CGI, Tim Burton had 40 squirrels trained to crack nuts for Charlie & The Chocolate Factory.",
                                           "Due to a zipper breaking, Olivia Newton-John had to be sewn into the trousers she wears in the last carnival scene of Grease.",
                                           "The sounds made by the Brachiosaurs in Jurassic Park were a combination of whale and donkey sounds.", 
                                           "In National Treasure, the good guys in the movie use Google and the bad guys use Yahoo!",
                                           "Django Unchained is the first time in 16 years that Leonardo DiCaprio didn't get the top billing.",
                                           "For Dr. Strangelove, Peter Sellers was paid $1 million, 55 percent of the film's budget. Stanley Kubrick quipped 'I got three for the price of six'.",
                                           "For LOTR: Return of the King, Elijah Wood had Alka Seltzer tablets in his mouth so he'd foam when Shelob stabbed him."};

        public static String[] TriviaB = { "Akshay Kumar lost weight to look 12 years younger for the movie HOLIDAY.",
                                           "Mr India was written for Big B.",
                                           "Sunny Leone likes horror stories but doesn't believe in ghosts.",
                                           "Farah Khan is scared of Nana Patekar.",
                                           "Hrithik Roshan went from flab to fab in 10 weeks during his shoot for Krrish 3."};
        public static String[,] s = new String[5, 6];

        public static String temp = "";

        public static int p1Score = 0;
        public static String[,] s1 = new String[15, 2];


        public static int p2Score = 0;

        public static String[] links = new String[5];

        public static String[] itemName = {"B","H"};

        public static MediaElement[] MyAudio = new MediaElement[5];

        public static int[] maxClips = {61,36};

        public static bool disconnectSuccess = false;

        public static bool deleteSuccess = false;

        public static bool roomJoined = false;

        public static bool connectSuccess = false;

        public static String GameRoomId = "1400752150"; //room: test1 -- 2 users max
        public static String DynRoomId = null;
        internal static bool PlayerIsFirst = false;
        public static WarpClient warpClient;
        public static ConnectionListener conListenObj;
        public static RoomReqListener roomReqListenerObj;
        public static ZoneRequestListener zoneReqListenerObj;
        public static NotificationListener notificationListenerObj;
        public static String localUsername ;
        public static String opponentUsername = "";
        public static string app42sessionid;
        
        public static string email;
        public static string password;

        public static Dictionary<string, object> p = new Dictionary<string, object>();

        public static Dictionary<string, object> dictionary = new Dictionary<string, object>();

        public static string[] ques = new string[5];

        public static int findxp(int p1, int p2)
        {
            int xp = 50;
            if (p1 > p2)
            {
                xp = xp + 100;
            }
            else if (p1 == p2)
            {
                xp = xp + 50;
            }
            else
            {
                xp = xp + 0;

            }
            xp = xp + p1;

            return xp;

        }
        public static int levelFromXP(double x)
        {
            return (int)Math.Floor(x / 350.0 + 0.5);
        }





    }
}
