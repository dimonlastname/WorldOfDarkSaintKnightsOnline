using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace servergame
{
    static public class Commands
    {
        public const string login = "1000";
        public const string CharacterCreate = "1010";
        public const string CharacterGoToWorld = "1020";
        public const string CharacterMove = "0001";
        public const string gameconnect = "";



        public static void Execute(Request action, Client c, string CommandType, string request)
        {
            switch (CommandType)
            {
                case login: //is connect
                    action.GameClientConnecting(request);    //receive new client connect (id)
                    break;
                case CharacterCreate: //Create character
                    action.CreateCharacter(request);

                    break;
                case CharacterGoToWorld: //choosen character and going to game world
                    action.GameStarting(request);
                    break;
                case CharacterMove: //move character in da world
                    c.CharMove(request);

                    break;
                case "0002":

                    break;
                default:
                    Console.WriteLine("[Server] Invalid command '" + CommandType + "', connection will be close");
                    c.ClientKiller(c);
                    break;
            }
        }
    }
}
