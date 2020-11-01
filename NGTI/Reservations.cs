using System;

namespace NGTI
{

    public class Reservation
    {
        
        public string name;
        public DateTime startTime;
        public DateTime endTime;
        public bool bhv;

        public Reservation(string Name, DateTime StartTime, DateTime EndTime)
        {
            this.name = Name;
            this.startTime = StartTime;
            this.endTime = EndTime;
            this.bhv = false;
        }
    }

    public class SoloReservation : Reservation
    {
        public Seat seat;

        public SoloReservation(string name, DateTime startTime, DateTime endTime, Seat Seat) :base(name, startTime, endTime)
        {
            this.seat = Seat;
        }

        public void endReservation()
        {
              if(DateTime.Now >= endTime){
                  seat.clear();
              }
        }
    }
    public class GroupReservation : Reservation
    {
        public Table table;
        public int groupSize;

        public GroupReservation(string name, DateTime startTime, DateTime endTime, Table Table) : base(name, startTime, endTime)
        {
            this.table = Table;
        }

        public void endReservation()
        {
              if(DateTime.Now >= endTime){
                table.clear();
              }
        }
    }

}