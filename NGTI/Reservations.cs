namespace NGTI
{

    public class Reservations
    {
        
        public string name;
        public DateTime startTime;
        public DateTime endTime;
        public bool bhv;

        public Reservation(string Name, DateTime StartTime, DateTime EndTime)
        {
            this.Name = name;
            this.startTime = StartTime;
            this.endTime = EndTime;
            this.bhv = false;
        }
    }

    public class SoloReservation : Reservations
    {
        public Seat seat;

        public SoloReservation(Seat Seat) : base(name, startTime, endTime)
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
    public class GroupReservation : Reservations
    {
        public Table table;
        public int groupSize;

        public GroupReservation(Table Table) : base(name, startTime, endTime)
        {
            this.table = Table;
        }

        public void endReservation()
        {
              if(DateTime.Now >= endTime){
                  seat.clear();
              }
        }
    }

}