using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace NGTI.Models
{
    public class ReservationsViewModel
    {
        public SoloReservation[] soloList;
        public GroupReservation[] groupList;
        public ReservationsViewModel()
        {
            this.soloList = new SoloReservation[]
            {
                new SoloReservation() { IdSoloReservation = 1, Name = "john", Date = new DateTime(2020,10,27), StartTime = new DateTime(2020,10,27,9,30,0), EndTime = new DateTime(2020,10,27,15,0,0), Reason = "", TableId = 1, Table = null},
                new SoloReservation() { IdSoloReservation = 2, Name = "sponge", Date = new DateTime(2020,10,27), StartTime = new DateTime(2020,10,27,9,30,0), EndTime = new DateTime(2020,10,27,20,0,0), Reason = "", TableId = 2, Table = null},
                new SoloReservation() { IdSoloReservation = 3, Name = "bob", Date = new DateTime(2020,10,27), StartTime = new DateTime(2020,10,27,9,30,0), EndTime = new DateTime(2020,10,27,18,0,0), Reason = "", TableId = 5, Table = null},
                new SoloReservation() { IdSoloReservation = 4, Name = "patrick", Date = new DateTime(2020,10,28), StartTime = new DateTime(2020,10,28,9,30,0), EndTime = new DateTime(2020,10,28,15,0,0), Reason = "", TableId = 3, Table = null}
            };
            this.groupList = new GroupReservation[]
            {
                new GroupReservation() { IdGroupReservation = 1, Name = "Ger", Teamname = "Bean", Date = new DateTime(2020,10,28), StartTime = new DateTime(2020,10,28,10,20,0), EndTime = new DateTime(2020,10,28,14,0,0), Reason = "e", TableId = 3, Table = null}
            };
        }
    }
}
