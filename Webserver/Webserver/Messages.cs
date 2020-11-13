using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Webserver
{
    public class Messages
    {
        string[] messages = new string[1000];
        static int counter = 0;
        public string GetMessages()
        {
            string res = "";
            foreach (var item in messages)
            {
                res+=(item != null)?item+"\n":"";
            }
            return res;
        }
        public int PostMessage(string message)
        {
            messages[counter]=message;
            counter++;
            return counter;
        }
        public string GetMessage(int id)
        {
            if (id > counter&&id-1<0)
                return null;
            return messages[id-1];
        }

        public bool UpdateMessage(int id, string message)
        {
            Console.WriteLine(id +" "+message);
            if (id - 1 < 0)
                return false;
            messages[id-1] = message;
            return true;
        }
        public bool DeleteMessage(int id)
        {
            if (id - 1 < 0)
                return false;
            messages[id-1]=null;
            counter--;
            return true;
        }
    }
}
