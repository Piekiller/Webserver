﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Webserver
{
    public class Messages
    {
        public const int SIZE=100;
        string[] messages = new string[SIZE];
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
            if (counter - 1 >= SIZE)
                return -1;
            messages[counter]=message;
            counter++;

            return counter;
        }
        public string GetMessage(int id)
        {
            if (id-1<0||id-1>SIZE)
                return null;
            return messages[id-1];
        }

        public bool PutMessage(int id, string message)
        {
            Console.WriteLine(id +" "+message);
            if (id - 1 < 0 || id - 1 > SIZE)
                return false;
            messages[id-1] = message;
            return true;
        }
        public bool DeleteMessage(int id)
        {
            if (id - 1 < 0||id-1> SIZE||messages[id-1]==null)
                return false;
            messages[id-1]=null;
            counter--;
            return true;
        }
    }
}
