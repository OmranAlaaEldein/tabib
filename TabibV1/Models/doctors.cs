using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TabibV1.Models;

namespace TabibV1.Models
{
    public class doctors
    {
        [Key, ForeignKey("user")]
        public string doctorsId { get; set; }
        public string specialty { get; set; }
        public int evaluate { get; set; }

        public virtual DoctorAppointment DoctorAppointments { get; set; }
        public virtual Address address { get; set; }
        

        public virtual ApplicationUser user { get; set; }
        public virtual ICollection<consulations> consulation { get; set; }
        public virtual ICollection<appointments> appointment { get; set; }
        public virtual ICollection<searches> searche { get; set; }

        // time of Doctor
    }

    public class Address {
        
        [Key]
        public int Id { get; set; }
        public string lang { set; get; }
        public string lat { set; get; }
        public string textAddress { set; get; }
    }

    public class DoctorAppointment
    {
        [Key]
        public int Id { get; set; }
        public DoctorAppointment()
        {
            TmieOfAppointment = 30;
        }
        public int TmieOfAppointment { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh\\:mm tt}")]
        public DateTime firstFrom { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh\\:mm tt}")]
        public DateTime firstTo { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh\\:mm tt}")]
        public DateTime secondFrom { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh\\:mm tt}")]
        public DateTime secondTo { get; set; }

        public int Days { get; set; }   // all Days : 1111111

        public Dictionary<string, bool> GetDays()
        {
            char[] temchar = Convert.ToString(Days, 2).ToCharArray();
            Dictionary<string, bool> tempDays = new Dictionary<string, bool>();

            tempDays.Add("saturday", (temchar.Length > 0 && temchar[0] == '1'));
            tempDays.Add("sunday", temchar.Length > 1 && temchar[1] == '1');
            tempDays.Add("monday", temchar.Length > 2 && temchar[2] == '1');
            tempDays.Add("tuesday", temchar.Length > 3 && temchar[3] == '1');
            tempDays.Add("wenthday", temchar.Length > 4 && temchar[4] == '1');
            tempDays.Add("thursday", temchar.Length > 5 && temchar[5] == '1');
            tempDays.Add("friday", temchar.Length > 6 && temchar[6] == '1');

            return tempDays;
            }

        public string GetStringDays()
        {
            char[] temchar = Convert.ToString(Days, 2).ToCharArray();
            string tempDays = "";

            tempDays = tempDays + ((temchar.Length > 0 && temchar[0] == '1') ? "saturday , " : "");
            tempDays = tempDays + ((temchar.Length > 1 && temchar[1] == '1') ? "sunday , " : "");
            tempDays = tempDays + ((temchar.Length > 2 && temchar[2] == '1') ? "monday , " : "");
            tempDays = tempDays + ((temchar.Length > 3 && temchar[3] == '1') ? "tuesday , " : "");
            tempDays = tempDays + ((temchar.Length > 4 && temchar[4] == '1') ? "wenthday , " : "");
            tempDays = tempDays + ((temchar.Length > 5 && temchar[5] == '1') ? "thursday , " : "");
            tempDays = tempDays + ((temchar.Length > 6 && temchar[6] == '1') ? "friday , " : "");

            return tempDays;
        }
    }

    public enum Days
    {
        Saturday,
        Sanday,
        Monday,
        Tuesday,
        Wensday,
        thursday,
        Friday
    }


    public enum specialty
    {
        all,
        Bonne,
        Dentist,
        Brain,
        inner,
        iye,
        ears

    }


    public class DoctorViewModel
    {
        
        public string doctorsId { get; set; }
         [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }
         [DataType(DataType.DateTime)]
        public DateTime DateBirthday { get; set; }
         [DataType(DataType.ImageUrl)]
         public string PathOfImage { get; set; }


        public int evaluate { get; set; }
        public string specialty { get; set; }


        [DisplayFormat(DataFormatString = "{0:hh\\:mm tt }")]
        public DateTime firstFrom { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh\\:mm tt }")]
        public DateTime firstTo { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh\\:mm tt }")]
        public DateTime secondFrom { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh\\:mm tt }")]
        public DateTime secondTo { get; set; }


        private int _Days;

        public int GetDays() {
            return _Days;
        }


        public Dictionary<string, bool> Days
        {
            get
            {
                char[] temchar = Convert.ToString(_Days, 2).ToCharArray();
                Dictionary<string,bool> tempDays = new Dictionary<string,bool>();

                tempDays.Add("saturday", (temchar.Length>0 &&  temchar[0]=='1'));
                tempDays.Add("sunday", temchar.Length>1  && temchar[1] == '1');
                tempDays.Add("monday", temchar.Length> 2 && temchar[2] == '1');
                tempDays.Add("tuesday", temchar.Length>3 && temchar[3] == '1');
                tempDays.Add("wenthday", temchar.Length>4 && temchar[4] == '1');
                tempDays.Add("thursday", temchar.Length>5 && temchar[5] == '1');
                tempDays.Add("friday", temchar.Length>6 && temchar[6] == '1');
            
                return tempDays;
            }
            set
            {
                _Days = 0;
                if (value["saturday"])
                    _Days = _Days + 64;
                if (value["sunday"])
                    _Days = _Days + 32;
                if (value["monday"])
                    _Days = _Days + 16;
                if (value["tuesday"])
                    _Days = _Days + 8;
                if (value["wenthday"])
                    _Days = _Days + 4;
                if (value["thursday"])
                    _Days = _Days + 2;
                if (value["friday"])
                    _Days = _Days + 1;
            }
        } 

       // public Boolean[] days { get; set; }

        //public bool saturday { get; set; }
        //public bool sunday { get; set; }
        //public bool monday { get; set; }
        //public bool tuesday { get; set; }
        //public bool wenthday { get; set; }
        //public bool thursday { get; set; }
        //public bool friday { get; set; }    


        public string lang { get; set; }
        public string lat { set; get; }
        public string textAddress { set; get; }

      
    }

}