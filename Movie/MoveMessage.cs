using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Windows.Controls;

namespace Movie
{
    /// <summary>
    /// Objects of this class represent actions of the user
    /// and are used to serialize/deserialize JSON exchanged between users in the room
    /// </summary>
    class MoveMessage
    {
        public String sender;
        public String TextBoxName;
        public String piece;
        public String type;

        public static MoveMessage buildMessage(byte[] update)
        {
            JObject jsonObj = JObject.Parse(System.Text.Encoding.UTF8.GetString(update, 0, update.Length));
            MoveMessage msg = new MoveMessage();
            msg.sender = jsonObj["sender"].ToString();
            msg.type = jsonObj["type"].ToString();
            if (msg.type == "move")
            {
                msg.TextBoxName = jsonObj["TextBoxName"].ToString();
                msg.piece = jsonObj["piece"].ToString();
            }
            return msg;
        }

        public static byte[] buildMessageBytes(TextBlock tb, String piece)
        {
            JObject moveObj = new JObject();
            moveObj.Add("TextBoxName", tb.Name);
            moveObj.Add("sender", Global.localUsername);
            moveObj.Add("piece", piece);
            moveObj.Add("type", "move");
            return System.Text.Encoding.UTF8.GetBytes(moveObj.ToString());
        }

        public static byte[] buildNewGameMessageBytes()
        {
            JObject moveObj = new JObject();
            moveObj.Add("sender", Global.localUsername);
            moveObj.Add("type", "new");
            return System.Text.Encoding.UTF8.GetBytes(moveObj.ToString());
        }
    }
}
