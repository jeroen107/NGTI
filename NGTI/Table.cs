namespace NGTI
{
    public class Table
    {
        
        public int tableNr;
        public int chairs;
        public bool taken;
        public Table(int TableNR, int Chairs)
        {
            this.tableNr = TableNR;
            this.chairs = Chairs;
            this.taken = false;
        }

        public void clear()
        {
            taken = false;
        }
    }
}