namespace NGTI
{
    public class Seat
    {
        public static int LastNr = 0;
        public int Nr;
        public string type;
        public bool taken;
        public Seat(int number, string Type, bool Taken)
        {
            this.Nr = LastNr;
            this.type = Type;
            this.taken = Taken;
            LastNr++;
        }

        public bool isReservered()
        {
            return taken;
        }
        
        public string getType()
        {
            return type;
        }

        public void reserve()
        {
            if(!isReservered()){
                taken = true;
            }
        }

        public void clear()
        {
            taken = false;
        }
    }



}