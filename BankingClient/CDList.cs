using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankingClient
{
    public class CDList
    {
        private static List<CDItem> allCds = new List<CDItem>();

        static CDList() {

            CDItem cd = new CDItem();
            cd.CDID = "CD001";
            cd.Title = "Clarence Sihiwatana No 1";
            cd.Description = "Re recorded as a tribute to Clerance Wijewardana.";
            cd.Artist = "Clerance Wijewardana.";
            cd.Price = 490;
            allCds.Add(cd);

            cd = new CDItem();
            cd.CDID = "CD002";
            cd.Title = "Clarence Sihiwatana No 2";
            cd.Description = "Re recorded as a tribute to Clerance Wijewardana.";
            cd.Artist = "Clerance Wijewardana.";
            cd.Price = 495;
            allCds.Add(cd);

            cd = new CDItem();
            cd.CDID = "CD003";
            cd.Title = "Clarence Sihiwatana No 3";
            cd.Description = "Re recorded as a tribute to Clerance Wijewardana.";
            cd.Artist = "Clerance Wijewardana.";
            cd.Price = 435;
            allCds.Add(cd);

            cd = new CDItem();
            cd.CDID = "CD004";
            cd.Title = "Clarence Sihiwatana No 4";
            cd.Description = "Re recorded as a tribute to Clerance Wijewardana.";
            cd.Artist = "Clerance Wijewardana.";
            cd.Price = 390;
            allCds.Add(cd);

            cd = new CDItem();
            cd.CDID = "CD005";
            cd.Title = "Clarence Sihiwatana No 5";
            cd.Description = "Re recorded as a tribute to Clerance Wijewardana.";
            cd.Artist = "Clerance Wijewardana.";
            cd.Price = 499;
            allCds.Add(cd);

        }
        public static List<String> getCdTitleList() {
            List<String> allTitles = new List<String>();

            foreach(CDItem cd in allCds){
                allTitles.Add(cd.CDID+" : "+cd.Title);
            }
            return allTitles;
        }

        public static CDItem getCdByTitleAndID(string title){

            String id = title.Split(':')[0].Trim();

            foreach (CDItem cd in allCds)
            {
                if(cd.CDID.Equals(id)){
                    return cd;
                }
            }

            return null;
        }
    }
}
