using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TabibV1.Models;

namespace TabibV1.Controllers
{
    public class SupportController : ApiController
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET api/support
        public Dictionary<DateTime, List<TimeSpan>> GetTimeofDoctor(string idDoctor)   //List<DateTime>
        {
            DoctorAppointment myTime = db.myDoctors.Where(x => x.doctorsId == idDoctor).Select(x => x.DoctorAppointments).FirstOrDefault();
            string[] Split = { " , " };
            List<string> DayOfDoctor = myTime.GetStringDays().Split(Split, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<appointments> myAppointments = db.myAppointments.Where(x => x.doctor.doctorsId == idDoctor).ToList();

            Dictionary<DateTime, List<TimeSpan>> RightTime = new Dictionary<DateTime, List<TimeSpan>>();

            for (int i = 1, countDay = 0; i <= 21 && countDay < 3; i++)
            {
                if (DayOfDoctor.Contains(DateTime.Now.AddDays(i).DayOfWeek.ToString().ToLower())  )
                {
                    countDay++;
                    List<TimeSpan> tempTime = new List<TimeSpan>();
                    for (int k = 0; myTime.firstFrom.AddMinutes(k * 30).TimeOfDay < myTime.firstTo.TimeOfDay; k++)
                    {
                        if (!myAppointments.Any(x => x.appointmentsTime.TimeOfDay.Equals(myTime.firstFrom.AddMinutes(k * 30).TimeOfDay)))
                        {
                            tempTime.Add(myTime.firstFrom.AddMinutes(k * 30).TimeOfDay);
                        }
                    }

                    for (int k = 0; myTime.secondFrom.AddMinutes(k * 30).TimeOfDay < myTime.secondTo.TimeOfDay; k++)
                    {
                        if (!myAppointments.Any(x => x.appointmentsTime.TimeOfDay.Equals(myTime.secondFrom.AddMinutes(k * 30).TimeOfDay)))
                        {
                            tempTime.Add(myTime.secondFrom.AddMinutes(k * 30).TimeOfDay);
                        }
                    }
                    if (tempTime.Count > 0)
                        RightTime.Add(DateTime.Now.AddDays(i), tempTime);        
                }
            }
            return RightTime;
        }

    }
}
